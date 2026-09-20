using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public class SiteAuditScannerEngine
    {
        public delegate void ProgressCallback(int scannedPages, int totalPages, string currentPageUrl);

        /// <summary>
        /// Webs skipped by the last scan, with the reason. Empty when every web was audited.
        /// </summary>
        public static List<string> LastSkippedWebs { get; private set; }

        /// <summary>
        /// Maps the scope chosen in the UI onto the webs to audit.
        /// "All" (or empty) walks every web in the site collection; "Root" audits the root web
        /// plus its /ar/ and /en/ language webs; anything else is treated as a server-relative
        /// or absolute web URL.
        /// </summary>
        private static List<SPWeb> ResolveTargetWebs(SPSite site, string scope)
        {
            List<SPWeb> targetWebs = new List<SPWeb>();

            if (string.IsNullOrEmpty(scope) || scope.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                foreach (SPWeb web in site.AllWebs)
                {
                    targetWebs.Add(web);
                }

                return targetWebs;
            }

            if (scope.Equals("Root", StringComparison.OrdinalIgnoreCase))
            {
                targetWebs.Add(site.OpenWeb(site.RootWeb.ServerRelativeUrl));

                foreach (string language in new[] { "ar", "en" })
                {
                    SPWeb languageWeb = null;
                    try
                    {
                        languageWeb = site.OpenWeb(language);
                        if (languageWeb != null && languageWeb.Exists)
                        {
                            targetWebs.Add(languageWeb);
                            languageWeb = null;
                        }
                    }
                    catch { }
                    finally
                    {
                        if (languageWeb != null) languageWeb.Dispose();
                    }
                }

                return targetWebs;
            }

            SPWeb specificWeb = null;
            try
            {
                specificWeb = site.OpenWeb(scope);
                if (specificWeb != null && specificWeb.Exists)
                {
                    targetWebs.Add(specificWeb);
                    specificWeb = null;
                }
            }
            catch { }
            finally
            {
                if (specificWeb != null) specificWeb.Dispose();
            }

            return targetWebs;
        }

        public static AuditScanSummaryDto RunAuditScan(string startWebUrl, string scannedBy, ProgressCallback onProgress = null)
        {
            SiteAuditReportDal.EnsureTablesExist();
            long scanId = SiteAuditReportDal.CreateScanSession(scannedBy, string.IsNullOrEmpty(startWebUrl) ? "All Subsites" : startWebUrl);
            if (scanId <= 0)
            {
                throw new InvalidOperationException("تعذر إنشاء جلسة فحص جديدة في قاعدة البيانات (SiteAuditScans).");
            }

            int totalPages = 0;
            int criticalCount = 0;
            int warningCount = 0;
            int infoCount = 0;
            int passedCount = 0;

            Exception scanError = null;
            List<string> skippedWebs = new List<string>();
            List<AuditIssueDto> allIssues = new List<AuditIssueDto>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string rootUrl = SPContext.Current != null && SPContext.Current.Site != null
                        ? SPContext.Current.Site.Url
                        : "https://pnu.edu.sa";

                    using (SPSite site = new SPSite(rootUrl))
                    {
                        List<SPWeb> targetWebs = ResolveTargetWebs(site, startWebUrl);
                        if (targetWebs.Count == 0)
                        {
                            throw new InvalidOperationException(
                                string.Format("لم يتم العثور على أي موقع مطابق لنطاق الفحص المحدد: '{0}'.", startWebUrl));
                        }

                        // Collect and audit pages
                        foreach (SPWeb web in targetWebs)
                        {
                            try
                            {
                                if (!PublishingWeb.IsPublishingWeb(web))
                                {
                                    continue;
                                }

                                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                                PublishingPageCollection pages = pubWeb.GetPublishingPages();

                                foreach (PublishingPage page in pages)
                                {
                                    if (page == null) continue;

                                    string pageName = page.Name ?? string.Empty;

                                    // Skip non-ASPX files (images, videos, PDFs mistakenly uploaded into the Pages library)
                                    if (!pageName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    // Skip SharePoint system pages (e.g. VariationRoot redirector)
                                    if (pageName.Equals("VariationRoot.aspx", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    totalPages++;
                                    string pageUrl = page.Uri != null ? page.Uri.ToString() : (web.Url + "/" + page.Name);

                                    if (onProgress != null)
                                    {
                                        try { onProgress(totalPages, 0, pageUrl); } catch { }
                                    }

                                    List<AuditIssueDto> pageIssues = AuditSinglePublishingPage(scanId, web, page);
                                    if (pageIssues != null && pageIssues.Count > 0)
                                    {
                                        foreach (var issue in pageIssues)
                                        {
                                            if (issue.Severity == "Critical") criticalCount++;
                                            else if (issue.Severity == "Warning") warningCount++;
                                            else infoCount++;

                                            allIssues.Add(issue);
                                        }
                                    }
                                    else
                                    {
                                        passedCount++;
                                    }

                                    // Periodic batch insert to prevent high memory usage
                                    if (allIssues.Count >= 100)
                                    {
                                        SiteAuditReportDal.BatchInsertIssues(scanId, allIssues);
                                        allIssues.Clear();
                                    }
                                }
                            }
                            catch (Exception webEx)
                            {
                                skippedWebs.Add(web.Url + " — " + webEx.Message);
                            }
                            finally
                            {
                                web.Dispose();
                            }
                        }

                        // Insert remaining issues
                        if (allIssues.Count > 0)
                        {
                            SiteAuditReportDal.BatchInsertIssues(scanId, allIssues);
                            allIssues.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    scanError = ex;
                }
            });

            SiteAuditReportDal.CompleteScanSession(scanId, totalPages, criticalCount, warningCount, infoCount, passedCount);

            // The session row is closed above so a partial scan is still readable,
            // but the caller must know the scan did not finish cleanly.
            if (scanError != null)
            {
                throw new InvalidOperationException("توقف الفحص قبل اكتماله: " + scanError.Message, scanError);
            }

            LastSkippedWebs = skippedWebs;
            return SiteAuditReportDal.GetScanSummary(scanId);
        }

        public static List<AuditIssueDto> AuditSinglePublishingPage(long scanId, SPWeb web, PublishingPage page)
        {
            List<AuditIssueDto> issues = new List<AuditIssueDto>();
            if (page == null || page.ListItem == null) return issues;

            string pageName = page.Name ?? string.Empty;
            if (!pageName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) ||
                pageName.Equals("VariationRoot.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return issues;
            }

            SPListItem item = page.ListItem;
            string pageUrl = page.Uri != null ? page.Uri.ToString() : (web.Url + "/" + page.Name);
            string subsiteUrl = web.Url;
            string pageTitle = item.Title ?? page.Name;
            string pageLayoutName = page.Layout != null ? page.Layout.Name : string.Empty;

            string contentHtml = string.Empty;
            if (item.Fields.ContainsField("PublishingPageContent") && item["PublishingPageContent"] != null)
            {
                contentHtml = item["PublishingPageContent"].ToString();
            }

            string commentsDesc = string.Empty;
            if (item.Fields.ContainsField("Comments") && item["Comments"] != null)
            {
                commentsDesc = item["Comments"].ToString();
            }

            // =========================================================================
            // DOMAIN 1: SEO, Title, Meta Description & Canonicals (Issues 3, 8, 9, 12-16, 21, 38, 39, 45, 51)
            // =========================================================================

            // Issue 12: Duplicate/Empty Page Title
            if (string.IsNullOrEmpty(pageTitle) || pageTitle.Trim().Length < 3)
            {
                issues.Add(new AuditIssueDto
                {
                    ScanId = scanId,
                    IssueCode = 12,
                    IssueCategory = "SEO & Metadata",
                    IssueName = "Page Titles - Missing or Too Short",
                    Severity = "Critical",
                    PageUrl = pageUrl,
                    SubsiteUrl = subsiteUrl,
                    PageTitle = pageTitle,
                    Details = "The page title is empty or less than 3 characters long.",
                    RecommendedFix = "Specify a descriptive title in the Page Title field."
                });
            }
            else if (pageTitle.Length > 70)
            {
                issues.Add(new AuditIssueDto
                {
                    ScanId = scanId,
                    IssueCode = 12,
                    IssueCategory = "SEO & Metadata",
                    IssueName = "Page Titles - Exceeds 70 Characters",
                    Severity = "Warning",
                    PageUrl = pageUrl,
                    SubsiteUrl = subsiteUrl,
                    PageTitle = pageTitle,
                    Details = $"The page title is {pageTitle.Length} characters long and may get truncated in search engine snippets.",
                    RecommendedFix = "Shorten the title to between 40 and 65 characters."
                });
            }

            // Issue 13: Meta Description - Missing
            if (string.IsNullOrEmpty(commentsDesc) || commentsDesc.Trim().Length < 10)
            {
                issues.Add(new AuditIssueDto
                {
                    ScanId = scanId,
                    IssueCode = 13,
                    IssueCategory = "SEO & Metadata",
                    IssueName = "Meta Description - Missing / Thin Description",
                    Severity = "Warning",
                    PageUrl = pageUrl,
                    SubsiteUrl = subsiteUrl,
                    PageTitle = pageTitle,
                    Details = "The Comments/Description field is empty on this publishing page.",
                    RecommendedFix = "Add a 120-160 character description in the page Comments field."
                });
            }

            // Issue 15 & 51: H1 Missing / Multiple H1s
            if (!string.IsNullOrEmpty(contentHtml))
            {
                var h1Matches = Regex.Matches(contentHtml, @"<h1[^>]*>([\s\S]*?)<\/h1>", RegexOptions.IgnoreCase);
                if (h1Matches.Count > 1)
                {
                    issues.Add(new AuditIssueDto
                    {
                        ScanId = scanId,
                        IssueCode = 16,
                        IssueCategory = "SEO & Structure",
                        IssueName = "H1 - Multiple H1 Headings in Content",
                        Severity = "Warning",
                        PageUrl = pageUrl,
                        SubsiteUrl = subsiteUrl,
                        PageTitle = pageTitle,
                        Details = $"Found {h1Matches.Count} <h1> tags inside the rich-text content.",
                        RecommendedFix = "Reserve <h1> for the page title and use <h2>/<h3> for sub-headings."
                    });
                }
            }

            // =========================================================================
            // Issue 4: Content - Low Content Pages (Thin Content / عداد الكلمات)
            // =========================================================================
            string plainContent = string.Empty;
            if (!string.IsNullOrEmpty(contentHtml))
            {
                // تجريد وسوم HTML والسكربتات والتنسيقات للحصول على النص الصافي
                plainContent = Regex.Replace(contentHtml, @"<script[\s\S]*?<\/script>|<style[\s\S]*?<\/style>|<[^>]+>", " ", RegexOptions.IgnoreCase);
                plainContent = Regex.Replace(plainContent, @"&[a-zA-Z0-9#]+;", " ").Trim();
            }

            int wordCount = string.IsNullOrEmpty(plainContent) ? 0 : Regex.Matches(plainContent, @"[\S]+").Count;

            // الصفحات التي يقل نصها عن 100 كلمة تعتبر Thin Content وفق معايير Google
            if (wordCount < 100)
            {
                issues.Add(new AuditIssueDto
                {
                    ScanId = scanId,
                    IssueCode = 4,
                    IssueCategory = "Content & Structure",
                    IssueName = "Content - Low Content Pages (Thin Content)",
                    Severity = "Warning",
                    PageUrl = pageUrl,
                    SubsiteUrl = subsiteUrl,
                    PageTitle = pageTitle,
                    Details = $"الصفحة تحتوي على {wordCount} كلمة فقط في حقل المحتوى (أقل من الحد الأدنى الموصى به 100 كلمة)، مما يعتبره محرك البحث محتوى ضعيفاً (Thin Content).",
                    ElementSnippet = plainContent.Length > 150 ? plainContent.Substring(0, 147) + "..." : (string.IsNullOrEmpty(plainContent) ? "[محتوى نصي فارغ]" : plainContent),
                    RecommendedFix = "إثراء محتوى الصفحة بالنصوص والمعلومات الكافية (100 كلمة على الأقل) أو دمجها مع صفحة أخرى ذات صلة."
                });
            }

            // =========================================================================
            // DOMAIN 2: Security & Protocol Integrity (Issues 17, 18, 30, 74)
            // =========================================================================

            if (!string.IsNullOrEmpty(contentHtml))
            {
                // Issue 18 & 30: Unsafe Cross-Origin Links (target="_blank" without noopener)
                var blankLinks = Regex.Matches(contentHtml, @"<a\s+[^>]*target\s*=\s*['""]_blank['""][^>]*>", RegexOptions.IgnoreCase);
                foreach (Match m in blankLinks)
                {
                    string tag = m.Value;
                    if (!tag.Contains("noopener") || !tag.Contains("noreferrer"))
                    {
                        issues.Add(new AuditIssueDto
                        {
                            ScanId = scanId,
                            IssueCode = 18,
                            IssueCategory = "Security",
                            IssueName = "Security - Unsafe Cross-Origin Link",
                            Severity = "Warning",
                            PageUrl = pageUrl,
                            SubsiteUrl = subsiteUrl,
                            PageTitle = pageTitle,
                            Details = "Found link with target='_blank' missing rel='noopener noreferrer'.",
                            ElementSnippet = tag.Length > 200 ? tag.Substring(0, 197) + "..." : tag,
                            RecommendedFix = "Add rel='noopener noreferrer' to target='_blank' links."
                        });
                        break; // Log once per page to avoid flooding
                    }
                }

                // Issue 74: Mixed Content / Insecure HTTP
                var httpRefs = Regex.Matches(contentHtml, @"(?:src|href)\s*=\s*['""](http:\/\/[^'""]+)['""]", RegexOptions.IgnoreCase);
                foreach (Match m in httpRefs)
                {
                    string url = m.Groups[1].Value;
                    if (!url.Contains("localhost") && !url.Contains("schemas.microsoft.com") && !url.Contains("w3.org"))
                    {
                        issues.Add(new AuditIssueDto
                        {
                            ScanId = scanId,
                            IssueCode = 74,
                            IssueCategory = "Security",
                            IssueName = "Security - Insecure HTTP Mixed Content",
                            Severity = "Critical",
                            PageUrl = pageUrl,
                            SubsiteUrl = subsiteUrl,
                            PageTitle = pageTitle,
                            Details = $"Found insecure HTTP reference in page content: {url}",
                            ElementSnippet = url,
                            RecommendedFix = "Upgrade asset/link URL to HTTPS."
                        });
                        break;
                    }
                }
            }

            // =========================================================================
            // DOMAIN 3: Accessibility & Images (Issues 25, 27, 28, 41, 46)
            // =========================================================================

            if (!string.IsNullOrEmpty(contentHtml))
            {
                var imgTags = Regex.Matches(contentHtml, @"<img\s+[^>]*>", RegexOptions.IgnoreCase);
                foreach (Match m in imgTags)
                {
                    string img = m.Value;
                    if (!Regex.IsMatch(img, @"\balt\s*=", RegexOptions.IgnoreCase))
                    {
                        issues.Add(new AuditIssueDto
                        {
                            ScanId = scanId,
                            IssueCode = 27,
                            IssueCategory = "Accessibility",
                            IssueName = "Images Missing Alt Attribute",
                            Severity = "Warning",
                            PageUrl = pageUrl,
                            SubsiteUrl = subsiteUrl,
                            PageTitle = pageTitle,
                            Details = "Image tag in page content is missing the alt attribute.",
                            ElementSnippet = img.Length > 200 ? img.Substring(0, 197) + "..." : img,
                            RecommendedFix = "Add an alt attribute describing the image or alt='' for decorative graphics."
                        });
                        break;
                    }
                }
            }

            // =========================================================================
            // DOMAIN 4: URL Structure, Unencoded Spaces & Slashes (Issues 32, 48)
            // =========================================================================

            if (pageUrl.Contains(" ") || (!string.IsNullOrEmpty(contentHtml) && contentHtml.Contains("/Style Library/")))
            {
                issues.Add(new AuditIssueDto
                {
                    ScanId = scanId,
                    IssueCode = 48,
                    IssueCategory = "URL & Routing",
                    IssueName = "URL - Contains Unencoded Space",
                    Severity = "Info",
                    PageUrl = pageUrl,
                    SubsiteUrl = subsiteUrl,
                    PageTitle = pageTitle,
                    Details = "Page URL or content contains unencoded spaces instead of %20 or hyphens.",
                    RecommendedFix = "Encode URL spaces with %20 or replace with hyphens."
                });
            }

            return issues;
        }
    }
}
