using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using PNU.Internet.SearchIndex.Config;
using PNU.Internet.SearchIndex.DAL;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Indexer
{
    /// <summary>
    /// Recursively crawls the menu tree of a site collection. For each
    /// web visited it:
    ///   1. reads TopMenuLevel1/2/3 (any subset) and indexes the URLs
    ///   2. honours SiteRules:
    ///        - HasAllItems        → also index AllItems list
    ///        - HasAboutList       → also index home.aspx / default.aspx
    ///        - FacultiesContainer → don't recurse into children
    ///        - FacultyChild       → skip entirely (AllFaculties owns it)
    ///   3. recurses into every menu URL that targets another SPWeb,
    ///      and into every direct child SPWeb the menus didn't link to.
    ///
    /// Boilerplate (DGA gov banner, login link, registration number,
    /// header/footer/nav blocks) is stripped from indexed content.
    /// </summary>
    public static class MenuPageCrawler
    {
        private const int MAX_DEPTH = 8;

        // ================================================================
        // PUBLIC API
        // ================================================================
        public static int CrawlMenusForOneWeb(SPWeb startWeb,
            SearchConfig cfg, StringBuilder errors)
        {
            if (startWeb == null || cfg == null) return 0;
            if (errors == null) errors = new StringBuilder();

            var seenUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenWebs = new HashSet<Guid>();

            return CrawlWebRecursive(startWeb, cfg, seenUrls, seenWebs, errors, 0);
        }

        public static void IndexManualPage(SPWeb originWeb, string absoluteUrl,
            ManualPage manual, SearchConfig cfg, StringBuilder errors)
        {
            if (string.IsNullOrEmpty(absoluteUrl)) return;
            if (!IsIndexableUrl(absoluteUrl, originWeb.Site.Url)) return;

            bool isArabic = absoluteUrl.IndexOf("/ar/",
                StringComparison.OrdinalIgnoreCase) >= 0;

            string titleAr = manual != null ? manual.PageTitleAr : "";
            string titleEn = manual != null ? manual.PageTitleEn : "";
            string catAr = (manual != null && !string.IsNullOrEmpty(manual.CategoryAr))
                ? manual.CategoryAr : "صفحات يدوية";
            string catEn = (manual != null && !string.IsNullOrEmpty(manual.CategoryEn))
                ? manual.CategoryEn : "Manual Pages";

            IndexUrlAsPage(originWeb, absoluteUrl, titleAr, titleEn,
                isArabic ? catAr : catEn, cfg, errors);
        }

        // ================================================================
        // RECURSIVE WALK
        // ================================================================
        private static int CrawlWebRecursive(SPWeb web, SearchConfig cfg,
            HashSet<string> seenUrls, HashSet<Guid> seenWebs,
            StringBuilder errors, int depth)
        {
            if (web == null) return 0;
            if (depth > MAX_DEPTH) return 0;
            if (!seenWebs.Add(web.ID)) return 0;
            if (cfg.IsWebExcluded(web)) return 0;

            int count = 0;

            try
            {
                var kind = SiteRules.Classify(web);

                if (kind == SiteRules.WebKind.SkipEntirely
                    || kind == SiteRules.WebKind.FacultyChild)
                    return 0;

                bool recurseIntoChildren =
                    (kind != SiteRules.WebKind.FacultiesContainer);

                // --- 1. Crawl menus on this web ---
                var menuTargets = new List<string>();
                foreach (var menuMap in cfg.MenuLists)
                {
                    if (cfg.IsListExcluded(menuMap.ListTitle)) continue;
                    count += CrawlOneMenuList(web, menuMap, cfg,
                        seenUrls, menuTargets, errors);
                }

                // --- 2. AllItems for Agencies/Deanship/Departments/Centers
                if (kind == SiteRules.WebKind.HasAllItems)
                    count += IndexAllItemsList(web, cfg, errors);

                // --- 3. About-list-driven home page ---
                if (SiteRules.HasAboutList(web))
                {
                    string home = SiteRules.ResolveHomePageUrl(web);
                    if (!string.IsNullOrEmpty(home)
                        && IsIndexableUrl(home, web.Site.Url)
                        && !cfg.IsPageExcluded(home)
                        && seenUrls.Add(home))
                    {
                        bool ar = web.Language == 1025;
                        IndexUrlAsPage(web, home,
                            ar ? "الصفحة الرئيسية" : "Home",
                            ar ? "Home" : "Home",
                            ar ? "صفحات رئيسية فرعية" : "Sub-site Home",
                            cfg, errors);
                        count++;
                    }
                }

                // --- 4. Recurse ---
                if (recurseIntoChildren)
                {
                    foreach (string url in menuTargets)
                    {
                        SPWeb target = TryOpenWebForUrl(web.Site, url);
                        if (target == null) continue;
                        try
                        {
                            if (target.ID != web.ID)
                            {
                                count += CrawlWebRecursive(target, cfg,
                                    seenUrls, seenWebs, errors, depth + 1);
                            }
                        }
                        finally { target.Dispose(); }
                    }

                    foreach (SPWeb child in web.Webs)
                    {
                        try
                        {
                            count += CrawlWebRecursive(child, cfg,
                                seenUrls, seenWebs, errors, depth + 1);
                        }
                        finally { child.Dispose(); }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.AppendLine("WEB " + web.Url + ": " + ex.Message);
                SearchLogger.WriteToLog("Crawler", web.Url, ex.Message);
            }

            return count;
        }

        // ================================================================
        // Menu list crawler (one list, on one web)
        // ================================================================
        private static int CrawlOneMenuList(SPWeb web, MenuListMap menuMap,
            SearchConfig cfg, HashSet<string> seenUrls,
            List<string> outTargets, StringBuilder errors)
        {
            int count = 0;
            try
            {
                SPList list = web.Lists.TryGetList(menuMap.ListTitle);
                if (list == null) return 0;

                string caml = "";
                if (HasField(list, "Visibility"))
                {
                    caml = @"<Where><Eq><FieldRef Name='Visibility'/>
                              <Value Type='Boolean'>1</Value></Eq></Where>";
                }

                var q = new SPQuery
                {
                    Query = caml,
                    ViewAttributes = "Scope=\"Recursive\""
                };

                foreach (SPListItem menu in list.GetItems(q))
                {
                    try
                    {
                        string url = ExtractUrl(menu, menuMap.UrlField);
                        if (string.IsNullOrEmpty(url)) continue;

                        string absoluteUrl = MakeAbsolute(url, web);
                        if (!IsIndexableUrl(absoluteUrl, web.Site.Url)) continue;
                        if (cfg.IsPageExcluded(absoluteUrl)) continue;

                        outTargets.Add(absoluteUrl);

                        if (!seenUrls.Add(absoluteUrl)) continue;

                        string titleAr = SafeStr(menu, "Title");
                        string titleEn = SafeStr(menu, "TitleEn");
                        if (string.IsNullOrEmpty(titleEn))
                            titleEn = SafeStr(menu, "Title_EN");

                        bool isArabic = web.Language == 1025;
                        string category = isArabic
                            ? (menuMap.CategoryAr ?? "صفحات")
                            : (menuMap.CategoryEn ?? "Pages");

                        IndexUrlAsPage(web, absoluteUrl, titleAr, titleEn,
                            category, cfg, errors);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        errors.AppendLine("MENU ITEM " + menuMap.ListTitle
                            + "#" + menu.ID + ": " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.AppendLine("MENU LIST " + menuMap.ListTitle
                    + " in web " + web.Url + ": " + ex.Message);
            }
            return count;
        }

        // ================================================================
        // AllItems list (Agencies / Deanship / Departments / Centers)
        // ================================================================
        private static int IndexAllItemsList(SPWeb web, SearchConfig cfg,
            StringBuilder errors)
        {
            int n = 0;
            try
            {
                SPList list = web.Lists.TryGetList("AllItems");
                if (list == null) return 0;

                bool isArabic = web.Language == 1025;
                string category = isArabic ? "محتوى الجهة" : "Unit Content";

                foreach (SPListItem item in list.GetItems(new SPQuery
                {
                    ViewAttributes = "Scope=\"Recursive\""
                }))
                {
                    try
                    {
                        string titleAr = SafeStr(item, "Title");
                        string titleEn = SafeStr(item, "Title_EN");

                        var sbAr = new StringBuilder();
                        var sbEn = new StringBuilder();
                        foreach (SPField f in list.Fields)
                        {
                            if (f.Hidden || f.ReadOnlyField) continue;
                            if (f.Type != SPFieldType.Text &&
                                f.Type != SPFieldType.Note &&
                                f.Type != SPFieldType.Choice) continue;

                            string v = SafeStr(item, f.InternalName);
                            if (string.IsNullOrWhiteSpace(v)) continue;

                            if (f.InternalName.EndsWith("_EN",
                                    StringComparison.OrdinalIgnoreCase))
                                sbEn.Append(v).Append(' ');
                            else
                                sbAr.Append(v).Append(' ');
                        }

                        string url = SafeStr(item, "URL");
                        if (string.IsNullOrEmpty(url))
                            url = web.Url.TrimEnd('/') + "/"
                                + list.DefaultDisplayFormUrl.TrimStart('/')
                                + "?ID=" + item.ID;
                        else
                            url = MakeAbsolute(url, web);

                        if (cfg.IsPageExcluded(url)) continue;

                        var dto = new SearchIndexItem
                        {
                            SourceType = "ListItem",
                            SourceKey  = "ALLITEMS|" + web.ID.ToString("N")
                                       + "|" + list.ID.ToString("N")
                                       + "|" + item.ID,
                            SiteId     = web.Site.ID,
                            WebId      = web.ID,
                            ListId     = list.ID,
                            ListItemId = item.ID,
                            TitleAr    = titleAr,
                            TitleEn    = titleEn,
                            ContentAr  = RemoveBoilerplate(
                                            SearchIndexer.StripHtml(sbAr.ToString().Trim())),
                            ContentEn  = RemoveBoilerplate(
                                            SearchIndexer.StripHtml(sbEn.ToString().Trim())),
                            Url        = url,
                            Category   = category,
                            DisplayDate= item["Modified"] != null
                                          ? (DateTime?)item["Modified"] : null
                        };
                        SearchIndexDal.Upsert(dto);
                        n++;
                    }
                    catch (Exception ex)
                    {
                        errors.AppendLine("ALLITEMS " + web.Url + "#"
                            + item.ID + ": " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.AppendLine("ALLITEMS " + web.Url + ": " + ex.Message);
            }
            return n;
        }

        // ================================================================
        // Index one URL as a "page" - tries publishing field first,
        // falls back to HTTP fetch + main extraction.
        // ================================================================
        private static void IndexUrlAsPage(SPWeb originWeb, string absoluteUrl,
            string menuTitleAr, string menuTitleEn, string category,
            SearchConfig cfg, StringBuilder errors)
        {
            SPListItem pubItem = TryResolvePublishingPage(
                originWeb.Site, absoluteUrl, cfg);

            string contentAr = "", contentEn = "";
            string titleAr = menuTitleAr, titleEn = menuTitleEn;
            DateTime? displayDate = null;

            if (pubItem != null)
            {
                string pubTitleAr = SafeStr(pubItem, "Title");
                string pubTitleEn = SafeStr(pubItem, "Title_EN");
                if (!string.IsNullOrEmpty(pubTitleAr)) titleAr = pubTitleAr;
                if (!string.IsNullOrEmpty(pubTitleEn)) titleEn = pubTitleEn;

                contentAr = FirstNonEmpty(
                    SafeStr(pubItem, "PublishingPageContent"),
                    SafeStr(pubItem, "Content"),
                    SafeStr(pubItem, "Description"));
                contentEn = FirstNonEmpty(
                    SafeStr(pubItem, "PublishingPageContent_EN"),
                    SafeStr(pubItem, "Content_EN"),
                    SafeStr(pubItem, "Description_EN"));

                if (pubItem["Modified"] != null)
                    displayDate = (DateTime)pubItem["Modified"];
            }

            // Fallback: fetch rendered HTML and extract clean main body
            if (string.IsNullOrWhiteSpace(contentAr)
                && string.IsNullOrWhiteSpace(contentEn))
            {
                string rendered = TryFetchRenderedHtml(absoluteUrl);
                if (!string.IsNullOrEmpty(rendered))
                {
                    string mainText = ExtractMainContent(rendered);
                    if (absoluteUrl.IndexOf("/ar/", StringComparison.OrdinalIgnoreCase) >= 0)
                        contentAr = mainText;
                    else if (absoluteUrl.IndexOf("/en/", StringComparison.OrdinalIgnoreCase) >= 0)
                        contentEn = mainText;
                    else
                        contentAr = mainText;
                }
            }

            string sourceKey = "MENUPAGE|"
                + originWeb.Site.ID.ToString("N") + "|"
                + absoluteUrl.ToLowerInvariant();

            var dto = new SearchIndexItem
            {
                SourceType = "Page",
                SourceKey  = sourceKey,
                SiteId     = originWeb.Site.ID,
                WebId      = originWeb.ID,
                ListId     = pubItem != null ? (Guid?)pubItem.ParentList.ID : null,
                ListItemId = pubItem != null ? (int?)pubItem.ID : null,
                TitleAr    = titleAr,
                TitleEn    = titleEn,
                ContentAr  = RemoveBoilerplate(SearchIndexer.StripHtml(contentAr)),
                ContentEn  = RemoveBoilerplate(SearchIndexer.StripHtml(contentEn)),
                Url        = absoluteUrl,
                Category   = category,
                DisplayDate= displayDate
            };
            SearchIndexDal.Upsert(dto);
        }

        // ================================================================
        // HTML EXTRACTION + boilerplate filter
        // ================================================================
        private static readonly Regex MainBlockRegex = new Regex(
            @"<main[^>]*id\s*=\s*['""]main-content['""][^>]*>(?<body>.*?)</main>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly string[] BlockTagsToStrip =
        {
            "header", "footer", "nav", "script", "style", "noscript",
            "dga-header", "dga-footer", "dga-nav",
            "dga-breadcrumb", "dga-page-header"
        };

        private static readonly Regex BlockClassRegex = new Regex(
            @"<(?:div|section|aside)[^>]*class\s*=\s*['""][^'""]*" +
            @"(?:masthead|topbar|topnav|site-header|site-footer|" +
            @"main-header|main-footer|navbar|breadcrumb|pageheader|" +
            @"page-header|cookie|skip-link)[^'""]*['""][^>]*>" +
            @".*?</(?:div|section|aside)>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly string[] DgaContainerPatterns =
        {
            @"<dga-[a-z-]+\b[^>]*>.*?</dga-[a-z-]+>",
            @"<div[^>]*\bid\s*=\s*['""]?(?:dga|gov|topbar|loginbar)[^'""]*['""]?[^>]*>.*?</div>",
            @"<section[^>]*\bid\s*=\s*['""]?(?:dga|gov|topbar)[^'""]*['""]?[^>]*>.*?</section>",
            @"<(?:div|section|aside)[^>]*\bclass\s*=\s*['""][^'""]*" +
                @"(?:official-site|gov-banner|trust-banner|site-meta|verify-banner|" +
                @"page-header-meta|breadcrumbs?)[^'""]*['""][^>]*>.*?</(?:div|section|aside)>"
        };

        private static readonly string[] BoilerplatePhrases =
        {
            "تسجيل الدخول",
            "Sign in", "Sign In", "Log in", "Login",

            "موقع حكومي رسمي تابع لحكومة المملكة العربية السعودية",
            "كيف تتحقق",
            "روابط المواقع الالكترونية الرسمية السعودية تنتهي بـ edu.sa",
            "جميع روابط المواقع الرسمية التعليمية في المملكة العربية السعودية تنتهي بـ sch.sa أو edu.sa",
            "المواقع الالكترونية الحكومية تستخدم بروتوكول HTTPS للتشفير و الأمان",
            "المواقع الالكترونية الآمنة في المملكة العربية السعودية تستخدم بروتوكول HTTPS للتشفير",
            "مسجل لدى هيئة الحكومة الرقمية برقم",
            "An official government website of the Kingdom of Saudi Arabia",
            "How to verify",
            "Official Saudi government websites end with .gov.sa",
            "Secure Saudi government websites use HTTPS",
            "Registered with the Digital Government Authority",

            "محتوى الصفحة", "Page content",
            "القائمة الرئيسية", "Main menu",
            "تخطي إلى المحتوى", "Skip to content",
            "الصفحة الرئيسية", "Home page",
            "اتصل بنا", "Contact us",
            "خريطة الموقع", "Site map",
            "الأسئلة الشائعة", "FAQ",
            "تابعنا", "Follow us",
            "جميع الحقوق محفوظة", "All rights reserved",

            "20250417424",
            "مهام الإدارة"
        };

        public static string ExtractMainContent(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";

            Match m = MainBlockRegex.Match(html);
            string body = m.Success ? m.Groups["body"].Value : html;

            foreach (string tag in BlockTagsToStrip)
            {
                string pattern = "<" + tag + @"\b[^>]*>.*?</" + tag + ">";
                body = Regex.Replace(body, pattern, " ",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            body = BlockClassRegex.Replace(body, " ");

            foreach (string pattern in DgaContainerPatterns)
            {
                body = Regex.Replace(body, pattern, " ",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            body = Regex.Replace(body, "<!--.*?-->", " ",
                RegexOptions.Singleline);

            string text = SearchIndexer.StripHtml(body);
            return RemoveBoilerplate(text);
        }

        public static string RemoveBoilerplate(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            foreach (string phrase in BoilerplatePhrases)
            {
                if (string.IsNullOrEmpty(phrase)) continue;
                int idx;
                while ((idx = text.IndexOf(phrase,
                        StringComparison.OrdinalIgnoreCase)) >= 0)
                {
                    text = text.Remove(idx, phrase.Length);
                }
            }

            text = Regex.Replace(text, @"\s+", " ").Trim();
            if (text.Length < 20) return "";
            return text;
        }

        // Backwards-compat
        public static string ExtractMainText(string html)
        {
            return ExtractMainContent(html);
        }

        // ================================================================
        // HTTP fetch with elevated credentials (no double-hop issue)
        // ================================================================
        private static string TryFetchRenderedHtml(string url)
        {
            string result = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    var req = (HttpWebRequest)WebRequest.Create(url);
                    req.Credentials = CredentialCache.DefaultNetworkCredentials;
                    req.PreAuthenticate = true;
                    req.Timeout = 15000;
                    req.UserAgent = "PNUSearchCrawler/1.0";
                    using (var resp = (HttpWebResponse)req.GetResponse())
                    using (var s    = resp.GetResponseStream())
                    using (var rd   = new StreamReader(s, Encoding.UTF8))
                        result = rd.ReadToEnd();
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("Crawler",
                        "TryFetchRenderedHtml " + url, ex.Message);
                }
            });
            return result;
        }

        // ================================================================
        // Helpers
        // ================================================================
        private static SPWeb TryOpenWebForUrl(SPSite site, string absoluteUrl)
        {
            try
            {
                Uri uri = new Uri(absoluteUrl);
                string path = uri.AbsolutePath;
                int lastSlash = path.LastIndexOf('/');
                if (lastSlash > 0) path = path.Substring(0, lastSlash);

                while (!string.IsNullOrEmpty(path))
                {
                    try
                    {
                        SPWeb w = site.OpenWeb(path, true);
                        if (w != null && w.Exists) return w;
                        if (w != null) w.Dispose();
                    }
                    catch { }

                    int idx = path.LastIndexOf('/');
                    if (idx <= 0) break;
                    path = path.Substring(0, idx);
                }
            }
            catch { }
            return null;
        }

        private static SPListItem TryResolvePublishingPage(SPSite site,
            string absoluteUrl, SearchConfig cfg)
        {
            try
            {
                Uri uri = new Uri(absoluteUrl);
                using (SPWeb web = site.OpenWeb(uri.AbsolutePath))
                {
                    if (cfg.IsWebExcluded(web)) return null;
                    if (!PublishingWeb.IsPublishingWeb(web)) return null;

                    PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                    foreach (PublishingPage page in pubWeb.GetPublishingPages())
                    {
                        if (string.Equals(page.Url.TrimStart('/'),
                                uri.AbsolutePath.TrimStart('/'),
                                StringComparison.OrdinalIgnoreCase))
                            return page.ListItem;
                    }
                }
            }
            catch { }
            return null;
        }

        private static string ExtractUrl(SPListItem menuItem, string preferredField)
        {
            string val = SafeStr(menuItem, preferredField);
            if (!string.IsNullOrWhiteSpace(val))
            {
                try
                {
                    var u = new SPFieldUrlValue(val);
                    if (!string.IsNullOrEmpty(u.Url)) return u.Url;
                }
                catch { }
                return val;
            }

            string[] candidates = { "URL", "Url", "Link", "PageUrl", "NavigationUrl" };
            foreach (string field in candidates)
            {
                string v = SafeStr(menuItem, field);
                if (string.IsNullOrWhiteSpace(v)) continue;
                try
                {
                    var u = new SPFieldUrlValue(v);
                    if (!string.IsNullOrEmpty(u.Url)) return u.Url;
                }
                catch { }
                return v;
            }
            return null;
        }

        private static string MakeAbsolute(string url, SPWeb web)
        {
            if (string.IsNullOrEmpty(url)) return url;
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return url;
            string baseUrl = web.Site.Url.TrimEnd('/');
            return baseUrl + (url.StartsWith("/") ? url : "/" + url);
        }

        private static bool IsIndexableUrl(string url, string siteUrl)
        {
            if (string.IsNullOrEmpty(url)) return false;
            if (url.IndexOf(siteUrl, StringComparison.OrdinalIgnoreCase) < 0)
                return false;
            if (url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)) return false;
            if (url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase)) return false;
            if (url.StartsWith("#")) return false;

            string lower = url.ToLowerInvariant();
            string[] badExt = { ".pdf",".doc",".docx",".xls",".xlsx",".ppt",".pptx",
                                ".zip",".jpg",".jpeg",".png",".gif",".svg",".mp4",".mp3" };
            foreach (var ext in badExt)
                if (lower.EndsWith(ext)) return false;
            return true;
        }

        private static bool HasField(SPList list, string internalName)
        {
            if (list == null || string.IsNullOrEmpty(internalName)) return false;
            try
            {
                foreach (SPField f in list.Fields)
                {
                    if (string.Equals(f.InternalName, internalName,
                            StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            catch { }
            return false;
        }

        private static string SafeStr(SPListItem item, string field)
        {
            try
            {
                if (item == null || string.IsNullOrEmpty(field)) return "";
                if (!item.Fields.ContainsField(field)) return "";
                return Convert.ToString(item[field]) ?? "";
            }
            catch { return ""; }
        }

        private static string FirstNonEmpty(params string[] vals)
        {
            foreach (var v in vals)
                if (!string.IsNullOrWhiteSpace(v)) return v;
            return "";
        }
    }
}
