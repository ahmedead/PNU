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
        private static readonly ConcurrentDictionary<string, WebInfo> _webInfoCache =
            new ConcurrentDictionary<string, WebInfo>(StringComparer.OrdinalIgnoreCase);

        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(30);

        // URL segments that mean we left the site hierarchy and entered a list/library.
        private static readonly HashSet<string> ContainerSegments =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Pages", "SitePages", "Lists", "Forms", "_layouts", "_catalogs"
        };

        // Separators the SEO page title uses between the page name and the site names.
        private static readonly string[] TitleSeparators = { " - ", " – ", " — ", " | " };

        private static readonly string[] KnownTitleSuffixes =
        {
            "جامعة الأميرة نورة بنت عبد الرحمن",
            "Princess Nourah bint Abdulrahman University"
        };

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

        private sealed class WebInfo
        {
            public bool Exists;
            public string Title;
            public DateTime CachedAtUtc;
        }

        private sealed class Crumb
        {
            public string Title;
            public string Url; // null => rendered as plain text, never a link
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Always rebuild: the breadcrumb depends only on the URL, and skipping it on
            // postback left the control empty after any in-page postback.
            GenerateBreadcrumbs();
        }

        private void GenerateBreadcrumbs()
        {
            try
            {
                string rawUrl = GetRawUrl();

                SPWeb currentWeb = null;
                try
                {
                    if (SPContext.Current != null)
                    {
                        currentWeb = SPContext.Current.Web;
                    }
                }
                catch { }

                string[] parts = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                bool isArabic = DetectArabic(parts, currentWeb);
                string homeTitle = isArabic ? "الرئيسية" : "Home";
                string rootHomeUrl = isArabic ? "/ar/" : "/en/";

                // Walk the URL down to the deepest subsite, keeping the real path of every
                // segment so a crumb can never link somewhere the segment does not live.
                int startIdx = 0;
                string runningRaw = string.Empty;
                string runningDecoded = string.Empty;
                if (parts.Length > 0 && IsLanguageSegment(parts[0]))
                {
                    startIdx = 1;
                    runningRaw = "/" + parts[0];
                    runningDecoded = runningRaw;
                }

                // Find the page file independently of the hierarchy walk below: the walk stops
                // at "Pages", so it would never reach the .aspx and every content page would
                // look like a site home page.
                string pageFileName = string.Empty;
                for (int i = parts.Length - 1; i >= 0; i--)
                {
                    if (parts[i].EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        pageFileName = parts[i];
                        break;
                    }
                }

                List<string> segments = new List<string>();
                List<string> segmentUrls = new List<string>();
                List<string> segmentPaths = new List<string>();

                for (int i = startIdx; i < parts.Length; i++)
                {
                    string part = parts[i];

                    if (part.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    // Anything past a list/library container belongs to the item, not to the
                    // site hierarchy, so stop walking here.
                    if (ContainerSegments.Contains(part))
                    {
                        break;
                    }

                    string decodedPart = HttpUtility.UrlDecode(part) ?? part;
                    runningRaw += "/" + part;
                    runningDecoded += "/" + decodedPart;

                    segments.Add(decodedPart);
                    segmentUrls.Add(runningRaw);
                    segmentPaths.Add(runningDecoded);
                }

                string pageTitle = GetCurrentPageTitle(currentWeb);
                bool isHomePage = IsHomePage(pageFileName, pageTitle);

                // Build the list first and render once, so "the last crumb is never a link"
                // is enforced in a single place instead of at every branch.
                List<Crumb> crumbs = new List<Crumb>();
                crumbs.Add(new Crumb { Title = homeTitle, Url = rootHomeUrl });

                string currentWebUrl = string.Empty;
                try
                {
                    if (currentWeb != null && !string.IsNullOrEmpty(currentWeb.ServerRelativeUrl))
                    {
                        currentWebUrl = currentWeb.ServerRelativeUrl.TrimEnd('/');
                    }
                }
                catch { }

                for (int i = 0; i < segments.Count; i++)
                {
                    string segPath = segmentPaths[i];
                    string segTitle;
                    bool isWeb;

                    if (!string.IsNullOrEmpty(currentWebUrl) &&
                        string.Equals(currentWebUrl, segPath, StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrWhiteSpace(currentWeb.Title))
                    {
                        segTitle = currentWeb.Title.Trim();
                        isWeb = true;
                    }
                    else
                    {
                        WebInfo info = ResolveWeb(segPath);
                        isWeb = info.Exists;
                        segTitle = info.Title;
                    }

                    if (string.IsNullOrEmpty(segTitle))
                    {
                        segTitle = GetSegmentTitleFallback(segments[i], isArabic);
                    }

                    if (string.IsNullOrEmpty(segTitle))
                    {
                        continue;
                    }

                    // A segment that is not a web has no landing page, so show it without a
                    // link rather than emitting a crumb that 404s.
                    crumbs.Add(new Crumb
                    {
                        Title = segTitle,
                        Url = isWeb ? segmentUrls[i] + "/" : null
                    });
                }

                // The page leaf, unless this is a site home page (the subsite crumb is
                // already the leaf) or the page title merely repeats the subsite name.
                if (!isHomePage && !string.IsNullOrEmpty(pageTitle))
                {
                    string lastTitle = crumbs[crumbs.Count - 1].Title;
                    if (!string.Equals(pageTitle, lastTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        crumbs.Add(new Crumb { Title = pageTitle, Url = null });
                    }
                }

                litBreadcrumbs.Text = Render(crumbs);
            }
            catch
            {
                // Never break the page over a breadcrumb.
            }
        }

        private static string Render(List<Crumb> crumbs)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < crumbs.Count; i++)
            {
                Crumb crumb = crumbs[i];
                bool isLast = (i == crumbs.Count - 1);
                string encodedTitle = SPHttpUtility.HtmlEncode(crumb.Title);

                if (isLast || string.IsNullOrEmpty(crumb.Url))
                {
                    sb.Append("<li class='breadcrumb-item small active disabled' aria-current='page'><span class='text-muted'>");
                    sb.Append(encodedTitle);
                    sb.Append("</span></li>");
                }
                else
                {
                    sb.Append("<li class='breadcrumb-item small'><a href='");
                    sb.Append(SPHttpUtility.HtmlEncode(crumb.Url));
                    sb.Append("'>");
                    sb.Append(encodedTitle);
                    sb.Append("</a></li>");
                }
            }

            return sb.ToString();
        }

        private string GetRawUrl()
        {
            try
            {
                if (Request != null && Request.Url != null)
                {
                    return Request.Url.AbsolutePath;
                }
            }
            catch { }

            try
            {
                if (Page != null && Page.Request != null)
                {
                    return Page.Request.ServerVariables["SCRIPT_NAME"] ?? Page.Request.Path ?? string.Empty;
                }
            }
            catch { }

            return string.Empty;
        }

        private static bool IsLanguageSegment(string segment)
        {
            return segment.Equals("ar", StringComparison.OrdinalIgnoreCase) ||
                   segment.Equals("en", StringComparison.OrdinalIgnoreCase);
        }

        private static bool DetectArabic(string[] parts, SPWeb currentWeb)
        {
            // Match the language segment exactly: a substring test on "/en" turns
            // /ar/faculties/engineering into an English page.
            if (parts.Length > 0)
            {
                if (parts[0].Equals("en", StringComparison.OrdinalIgnoreCase)) return false;
                if (parts[0].Equals("ar", StringComparison.OrdinalIgnoreCase)) return true;
            }

            try
            {
                if (currentWeb != null && currentWeb.Language != 1025 && currentWeb.Language != 0)
                {
                    return false;
                }
            }
            catch { }

            return true;
        }

        private static bool IsHomePage(string pageFileName, string pageTitle)
        {
            if (string.IsNullOrEmpty(pageFileName) ||
                string.Equals(pageFileName, "default.aspx", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(pageFileName, "home.aspx", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(pageFileName, "index.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.IsNullOrEmpty(pageTitle)) return false;

            return pageTitle.Equals("الرئيسية", StringComparison.OrdinalIgnoreCase) ||
                   pageTitle.Equals("Home", StringComparison.OrdinalIgnoreCase);
        }

        private static WebInfo ResolveWeb(string serverRelativeUrl)
        {
            if (string.IsNullOrEmpty(serverRelativeUrl))
            {
                return new WebInfo { Exists = false, Title = string.Empty, CachedAtUtc = DateTime.UtcNow };
            }

            WebInfo cached;
            if (_webInfoCache.TryGetValue(serverRelativeUrl, out cached) &&
                (DateTime.UtcNow - cached.CachedAtUtc) < CacheTtl)
            {
                return cached;
            }

            WebInfo result = new WebInfo { Exists = false, Title = string.Empty, CachedAtUtc = DateTime.UtcNow };
            bool lookupSucceeded = false;

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
                                // requireExactUrl: without it SharePoint silently returns the
                                // nearest ancestor web, so a plain folder inherits its parent's
                                // title and becomes a crumb pointing at the wrong place.
                                using (SPWeb web = site.OpenWeb(serverRelativeUrl, true))
                                {
                                    if (web != null && web.Exists)
                                    {
                                        result.Exists = true;
                                        result.Title = string.IsNullOrWhiteSpace(web.Title) ? string.Empty : web.Title.Trim();
                                        lookupSucceeded = true;
                                    }
                                }
                            }
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            // No web at this exact URL: a definitive "not a subsite".
                            result.Exists = false;
                            result.Title = string.Empty;
                            lookupSucceeded = true;
                        }
                        catch { }
                    });
                }
            }
            catch { }

            if (lookupSucceeded)
            {
                _webInfoCache[serverRelativeUrl] = result;
                return result;
            }

            // The lookup failed for some other reason (permissions, transient error). Don't
            // cache that, and assume the segment is a site so the chain keeps its links.
            return new WebInfo { Exists = true, Title = string.Empty, CachedAtUtc = DateTime.UtcNow };
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

            return segment;
        }

        private string GetCurrentPageTitle(SPWeb currentWeb)
        {
            // The page's own Title field first: Page.Title is the SEO title and carries the
            // subsite and university names, which is what leaked into the last crumb.
            try
            {
                if (SPContext.Current != null && SPContext.Current.ListItem != null)
                {
                    object title = SPContext.Current.ListItem["Title"];
                    if (title != null && !string.IsNullOrWhiteSpace(title.ToString()))
                    {
                        string fromItem = CleanTitle(title.ToString(), currentWeb);
                        if (!string.IsNullOrEmpty(fromItem)) return fromItem;
                    }
                }
            }
            catch { }

            try
            {
                if (Page != null && !string.IsNullOrWhiteSpace(Page.Title))
                {
                    string fromPage = CleanTitle(Page.Title, currentWeb);
                    if (!string.IsNullOrEmpty(fromPage)) return fromPage;
                }
            }
            catch { }

            try
            {
                if (SPContext.Current != null && SPContext.Current.ListItem != null &&
                    !string.IsNullOrEmpty(SPContext.Current.ListItem.Name))
                {
                    return System.IO.Path.GetFileNameWithoutExtension(SPContext.Current.ListItem.Name);
                }
            }
            catch { }

            return string.Empty;
        }

        private static string CleanTitle(string title, SPWeb currentWeb)
        {
            return StripTitleDecorations(title, BuildTitleSuffixes(currentWeb));
        }

        private static List<string> BuildTitleSuffixes(SPWeb currentWeb)
        {
            List<string> suffixes = new List<string>();
            try
            {
                if (currentWeb != null)
                {
                    if (!string.IsNullOrWhiteSpace(currentWeb.Title)) suffixes.Add(currentWeb.Title.Trim());
                    if (currentWeb.Site != null && currentWeb.Site.RootWeb != null &&
                        !string.IsNullOrWhiteSpace(currentWeb.Site.RootWeb.Title))
                    {
                        suffixes.Add(currentWeb.Site.RootWeb.Title.Trim());
                    }
                }
            }
            catch { }
            suffixes.AddRange(KnownTitleSuffixes);
            return suffixes;
        }

        private static string StripTitleDecorations(string title, List<string> suffixes)
        {
            if (string.IsNullOrWhiteSpace(title)) return string.Empty;

            string result = title.Trim();

            foreach (string prefix in new[] { "الصفحات - ", "Pages - " })
            {
                if (result.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Substring(prefix.Length).Trim();
                }
            }

            // "<page> - <college> - <university>" stacks several suffixes, so keep peeling
            // until nothing matches any more.
            bool trimmed = true;
            while (trimmed)
            {
                trimmed = false;
                foreach (string suffix in suffixes)
                {
                    if (string.IsNullOrEmpty(suffix)) continue;

                    foreach (string separator in TitleSeparators)
                    {
                        string tail = separator + suffix;
                        if (result.Length > tail.Length &&
                            result.EndsWith(tail, StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.Substring(0, result.Length - tail.Length).TrimEnd();
                            trimmed = true;
                        }
                    }
                }
            }

            return result.Trim();
        }
    }
}
