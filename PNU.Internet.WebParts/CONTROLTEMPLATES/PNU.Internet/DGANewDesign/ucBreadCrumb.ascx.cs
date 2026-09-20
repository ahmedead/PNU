using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class ucBreadCrumb : UserControl
    {
        private static readonly ConcurrentDictionary<string, string> _webTitleCache =
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, (string ar, string en)> SectionNames =
            new Dictionary<string, (string ar, string en)>(StringComparer.OrdinalIgnoreCase)
        {
            { "faculties", ("الكليات", "Colleges") },
            { "colleges", ("الكليات", "Colleges") },
            { "deanship", ("العمادات", "Deanships") },
            { "deanships", ("العمادات", "Deanships") },
            { "agencies", ("وكالات الجامعة", "University Vice Presidencies") },
            { "agency", ("وكالات الجامعة", "University Vice Presidencies") },
            { "regadm", ("عمادة القبول والتسجيل", "Deanship of Admission and Registration") },
            { "dga", ("وكالة الجامعة للدراسات العليا والبحث العلمي", "Vice Presidency for Postgraduate Studies and Scientific Research") },
            { "centers", ("المراكز", "Centers") },
            { "departments", ("الأقسام", "Departments") },
            { "administration", ("الإدارات", "Administration") },
            { "admin", ("الإدارات", "Administration") },
            { "community", ("خدمة المجتمع", "Community Service") },
            { "about", ("عن الجامعة", "About University") },
            { "mediacenter", ("المركز الإعلامي", "Media Center") },
            { "eservices", ("الخدمات الإلكترونية", "E-Services") },
            { "research", ("البحث العلمي", "Scientific Research") },
            { "postgraduatekit", ("دليل الدراسات العليا", "Postgraduate Kit") },
            { "announcements", ("الإعلانات", "Announcements") },
            { "news", ("الأخبار", "News") },
            { "events", ("الفعاليات", "Events") }
        };

        private static readonly Dictionary<string, (string ar, string en)> SubsiteNames =
            new Dictionary<string, (string ar, string en)>(StringComparer.OrdinalIgnoreCase)
        {
            { "it", ("كلية علوم الحاسب والمعلومات", "College of Computer and Information Sciences") },
            { "cbas", ("كلية إدارة الأعمال", "College of Business Administration") },
            { "medicine", ("كلية الطب البشري", "College of Medicine") },
            { "pharmacy", ("كلية الصيدلة", "College of Pharmacy") },
            { "nursing", ("كلية التمريض", "College of Nursing") },
            { "dentistry", ("كلية طب الأسنان", "College of Dentistry") },
            { "science", ("كلية العلوم", "College of Science") },
            { "engineering", ("كلية الهندسة", "College of Engineering") },
            { "art", ("كلية التصاميم والفنون", "College of Art and Design") },
            { "education", ("كلية التربية", "College of Education") },
            { "humanities", ("كلية العلوم الإنسانية والاجتماعية", "College of Humanities and Social Sciences") },
            { "languages", ("كلية اللغات", "College of Languages") },
            { "law", ("كلية القانون", "College of Law") },
            { "health", ("كلية الصحة وعلوم التأهيل", "College of Health and Rehabilitation Sciences") },
            { "applied", ("الكلية التطبيقية", "Applied College") }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateBreadcrumbs();
            }
        }

        private void GenerateBreadcrumbs()
        {
            try
            {
                string rawUrl = string.Empty;
                if (Request != null && Request.Url != null)
                {
                    rawUrl = Request.Url.AbsolutePath;
                }
                else if (Page != null && Page.Request != null)
                {
                    rawUrl = Page.Request.ServerVariables["SCRIPT_NAME"] ?? Page.Request.Path ?? string.Empty;
                }

                SPWeb currentWeb = null;
                try
                {
                    if (SPContext.Current != null)
                    {
                        currentWeb = SPContext.Current.Web;
                    }
                }
                catch { }

                bool isArabic = true;
                if (rawUrl.IndexOf("/en", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (currentWeb != null && currentWeb.Language != 1025 && currentWeb.Language != 0))
                {
                    isArabic = false;
                }

                string homeTitle = isArabic ? "الرئيسية" : "Home";
                string rootHomeUrl = isArabic ? "/ar/" : "/en/";

                // Parse URL segments to identify hierarchy safely without ParentWeb or OpenWeb
                string[] parts = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> pathSegments = new List<string>();
                string pageFileName = string.Empty;

                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i];
                    if (part.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        pageFileName = part;
                    }
                    else if (!part.Equals("Pages", StringComparison.OrdinalIgnoreCase))
                    {
                        pathSegments.Add(part);
                    }
                }

                // If first segment is language code (ar/en), skip it for hierarchy since rootHomeUrl covers it
                int startIdx = 0;
                if (pathSegments.Count > 0 && (pathSegments[0].Equals("ar", StringComparison.OrdinalIgnoreCase) ||
                                               pathSegments[0].Equals("en", StringComparison.OrdinalIgnoreCase)))
                {
                    startIdx = 1;
                }

                string pageTitle = GetCurrentPageTitle();
                bool isHomePage = string.IsNullOrEmpty(pageFileName) ||
                                  string.Equals(pageFileName, "default.aspx", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(pageFileName, "home.aspx", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(pageTitle, "الرئيسية", StringComparison.OrdinalIgnoreCase) ||
                                  pageTitle.EndsWith("- الرئيسية", StringComparison.OrdinalIgnoreCase) ||
                                  pageTitle.EndsWith("-الرئيسية", StringComparison.OrdinalIgnoreCase);

                List<string> crumbs = new List<string>();

                // 1. Root Home Crumb
                if (pathSegments.Count <= startIdx && isHomePage)
                {
                    crumbs.Add($"<li class='breadcrumb-item small active disabled' aria-current='page'><span class='text-muted'>{SPHttpUtility.HtmlEncode(homeTitle)}</span></li>");
                    litBreadcrumbs.Text = string.Join("", crumbs);
                    return;
                }

                crumbs.Add($"<li class='breadcrumb-item small'><a href='{SPHttpUtility.HtmlEncode(rootHomeUrl)}'>{SPHttpUtility.HtmlEncode(homeTitle)}</a></li>");

                // 2. Intermediate & Subsite Crumbs
                string accumulatedPath = rootHomeUrl.TrimEnd('/');

                for (int i = startIdx; i < pathSegments.Count; i++)
                {
                    string seg = pathSegments[i];
                    accumulatedPath += "/" + seg;
                    bool isLastSegment = (i == pathSegments.Count - 1);

                    // First try to resolve real Site Title dynamically from SharePoint
                    string segTitle = string.Empty;

                    // If it is the current web, take title directly from SPContext.Current.Web
                    if (isLastSegment && currentWeb != null && !string.IsNullOrWhiteSpace(currentWeb.Title))
                    {
                        segTitle = currentWeb.Title.Trim();
                    }
                    else
                    {
                        // Dynamically resolve parent web's title from SharePoint cache
                        segTitle = ResolveWebTitle(accumulatedPath);
                    }

                    // If not resolved from SPWeb, fall back to known section/subsite dictionary
                    if (string.IsNullOrEmpty(segTitle))
                    {
                        segTitle = GetSegmentTitleFallback(seg, isArabic);
                    }

                    if (isLastSegment && isHomePage)
                    {
                        // Active leaf for subsite home
                        crumbs.Add($"<li class='breadcrumb-item small active disabled' aria-current='page'><span class='text-muted'>{SPHttpUtility.HtmlEncode(segTitle)}</span></li>");
                    }
                    else
                    {
                        crumbs.Add($"<li class='breadcrumb-item small'><a href='{SPHttpUtility.HtmlEncode(accumulatedPath + "/")}'>{SPHttpUtility.HtmlEncode(segTitle)}</a></li>");
                    }
                }

                // 3. Current Page Leaf (only if NOT on a subsite home page)
                if (!isHomePage && !string.IsNullOrEmpty(pageTitle))
                {
                    string currentSubsiteTitle = currentWeb != null && !string.IsNullOrWhiteSpace(currentWeb.Title) ? currentWeb.Title.Trim() : "";
                    if (!string.Equals(pageTitle, currentSubsiteTitle, StringComparison.OrdinalIgnoreCase) &&
                        !pageTitle.Equals("الرئيسية", StringComparison.OrdinalIgnoreCase))
                    {
                        crumbs.Add($"<li class='breadcrumb-item small active disabled' aria-current='page'><span class='text-muted'>{SPHttpUtility.HtmlEncode(pageTitle)}</span></li>");
                    }
                }

                litBreadcrumbs.Text = string.Join("", crumbs);
            }
            catch
            {
                // Fallback gracefully without breaking the page or throwing 401
            }
        }

        private static string ResolveWebTitle(string serverRelativeUrl)
        {
            if (string.IsNullOrEmpty(serverRelativeUrl)) return string.Empty;

            string cached;
            if (_webTitleCache.TryGetValue(serverRelativeUrl, out cached))
            {
                return cached;
            }

            string resolvedTitle = string.Empty;
            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        try
                        {
                            using (SPSite site = new SPSite(siteId))
                            {
                                using (SPWeb web = site.OpenWeb(serverRelativeUrl))
                                {
                                    if (web.Exists && !string.IsNullOrWhiteSpace(web.Title))
                                    {
                                        resolvedTitle = web.Title.Trim();
                                    }
                                }
                            }
                        }
                        catch { }
                    });
                }
            }
            catch { }

            if (!string.IsNullOrEmpty(resolvedTitle))
            {
                _webTitleCache[serverRelativeUrl] = resolvedTitle;
                return resolvedTitle;
            }

            return string.Empty;
        }

        private static string GetSegmentTitleFallback(string segment, bool isArabic)
        {
            if (string.IsNullOrEmpty(segment)) return string.Empty;

            if (SectionNames.TryGetValue(segment, out var secVal))
            {
                return isArabic ? secVal.ar : secVal.en;
            }

            if (SubsiteNames.TryGetValue(segment, out var subVal))
            {
                return isArabic ? subVal.ar : subVal.en;
            }

            return HttpUtility.UrlDecode(segment);
        }

        private string GetCurrentPageTitle()
        {
            try
            {
                if (Page != null && !string.IsNullOrWhiteSpace(Page.Title))
                {
                    string t = Page.Title.Trim();
                    if (t.StartsWith("الصفحات - ", StringComparison.OrdinalIgnoreCase))
                    {
                        t = t.Substring("الصفحات - ".Length).Trim();
                    }
                    return t;
                }

                if (SPContext.Current != null && SPContext.Current.ListItem != null)
                {
                    object title = SPContext.Current.ListItem["Title"];
                    if (title != null && !string.IsNullOrWhiteSpace(title.ToString()))
                    {
                        string s = title.ToString().Trim();
                        if (s.StartsWith("الصفحات - ", StringComparison.OrdinalIgnoreCase))
                        {
                            s = s.Substring("الصفحات - ".Length).Trim();
                        }
                        return s;
                    }

                    if (!string.IsNullOrEmpty(SPContext.Current.ListItem.Name))
                    {
                        return System.IO.Path.GetFileNameWithoutExtension(SPContext.Current.ListItem.Name);
                    }
                }
            }
            catch { }

            return string.Empty;
        }
    }
}
