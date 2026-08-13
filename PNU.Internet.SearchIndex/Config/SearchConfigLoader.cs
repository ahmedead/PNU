using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Logging;
using PNU.Internet.SearchIndex.Provisioning;

namespace PNU.Internet.SearchIndex.Config
{
    public class ContentListMap
    {
        public string ListTitle      { get; set; }
        public string ListWebUrl     { get; set; }
        public string CategoryAr     { get; set; }
        public string CategoryEn     { get; set; }
        public string TitleField     { get; set; } = "Title";
        public string TitleFieldEn   { get; set; } = "Title_EN";
        public List<string> ContentFields   { get; set; } = new List<string>();
        public List<string> ContentFieldsEn { get; set; } = new List<string>();
        public string DateField       { get; set; } = "Modified";
        public string UrlPattern      { get; set; }
        public string VisibilityField { get; set; }
        public string FilterField     { get; set; }
        public string FilterValue     { get; set; }
        public string FilterMode      { get; set; } = "Equals";
    }

    public class MenuListMap
    {
        public string ListTitle  { get; set; }
        public string UrlField   { get; set; } = "URL";
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }
    }

    public class ManualPage
    {
        public string PageUrl     { get; set; }
        public string PageTitleAr { get; set; }
        public string PageTitleEn { get; set; }
        public string CategoryAr  { get; set; }
        public string CategoryEn  { get; set; }
    }

    public class SearchConfig
    {
        public List<ContentListMap> ContentLists  { get; set; } = new List<ContentListMap>();
        public List<MenuListMap>    MenuLists     { get; set; } = new List<MenuListMap>();
        public List<ManualPage>     ManualPages   { get; set; } = new List<ManualPage>();
        public HashSet<string>      ExcludedWebs  { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string>      ExcludedPages { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string>      ExcludedLists { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public ContentListMap GetContentMap(string listTitle)
        {
            return ContentLists.FirstOrDefault(c => string.Equals(
                c.ListTitle, listTitle, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsListExcluded(string listTitle)
        {
            return !string.IsNullOrEmpty(listTitle)
                   && ExcludedLists.Contains(listTitle);
        }

        public bool IsWebExcluded(SPWeb web)
        {
            if (web == null) return false;
            return ExcludedWebs.Contains(web.ServerRelativeUrl
                       .TrimEnd('/').ToLowerInvariant())
                || ExcludedWebs.Contains(web.Url
                       .TrimEnd('/').ToLowerInvariant());
        }

        public bool IsPageExcluded(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            string norm = url.TrimEnd('/').ToLowerInvariant();
            return ExcludedPages.Contains(norm)
                || ExcludedPages.Any(p =>
                       norm.EndsWith(p.TrimEnd('/').ToLowerInvariant()));
        }
    }

    public static class SearchConfigLoader
    {
        private static SearchConfig _cached;
        private static DateTime     _cachedAt;
        private static readonly object _lock = new object();
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

        public static void InvalidateCache()
        {
            lock (_lock) { _cached = null; }
        }

        public static SearchConfig Load(SPSite site)
        {
            lock (_lock)
            {
                if (_cached != null
                    && (DateTime.UtcNow - _cachedAt) < CacheTtl)
                    return _cached;

                _cached = LoadFresh(site);
                _cachedAt = DateTime.UtcNow;
                return _cached;
            }
        }

        // ================================================================
        // IndexedWebs - per-task progress log
        // ================================================================
        public static bool IsTaskCompletedForWeb(SPSite site,
            string taskName, string webUrl)
        {
            try
            {
                bool found = false;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    {
                        SPWeb sub = TryOpenSubsite(s, s.RootWeb);
                        if (sub == null) return;
                        try
                        {
                            SPList list = sub.Lists.TryGetList(
                                SearchIndexingProvisioner.LIST_INDEXED_WEBS);
                            if (list == null) return;

                            string norm = (webUrl ?? "").TrimEnd('/').ToLowerInvariant();
                            string caml = string.Format(
                                @"<Where><And>
                                    <Eq><FieldRef Name='Title'/>
                                        <Value Type='Text'>{0}</Value></Eq>
                                    <Eq><FieldRef Name='WebUrl'/>
                                        <Value Type='Text'>{1}</Value></Eq>
                                  </And></Where>",
                                Escape(taskName), Escape(norm));
                            var q = new SPQuery { Query = caml, RowLimit = 1 };
                            found = list.GetItems(q).Count > 0;
                        }
                        finally { sub.Dispose(); }
                    }
                });
                return found;
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Tasks",
                    "IsTaskCompletedForWeb " + taskName + "|" + webUrl,
                    ex.Message);
                return false;
            }
        }

        public static void MarkTaskCompletedForWeb(SPSite site,
            string taskName, string webUrl, int itemsCount, string errors)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    {
                        SPWeb sub = TryOpenSubsite(s, s.RootWeb);
                        if (sub == null) return;
                        try
                        {
                            sub.AllowUnsafeUpdates = true;
                            SPList list = sub.Lists.TryGetList(
                                SearchIndexingProvisioner.LIST_INDEXED_WEBS);
                            if (list == null) return;

                            SPListItem it = list.Items.Add();
                            it["Title"]       = taskName;
                            it["WebUrl"]      = (webUrl ?? "").TrimEnd('/').ToLowerInvariant();
                            it["CompletedAt"] = DateTime.Now;
                            it["ItemsCount"]  = itemsCount;
                            if (!string.IsNullOrEmpty(errors))
                                it["Errors"] = errors.Length > 4000
                                    ? errors.Substring(0, 4000) : errors;
                            it.Update();
                        }
                        finally
                        {
                            sub.AllowUnsafeUpdates = false;
                            sub.Dispose();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Tasks",
                    "MarkTaskCompletedForWeb " + taskName + "|" + webUrl,
                    ex.Message);
            }
        }

        public static void ClearTaskLog(SPSite site, string taskName = null)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    {
                        SPWeb sub = TryOpenSubsite(s, s.RootWeb);
                        if (sub == null) return;
                        try
                        {
                            sub.AllowUnsafeUpdates = true;
                            SPList list = sub.Lists.TryGetList(
                                SearchIndexingProvisioner.LIST_INDEXED_WEBS);
                            if (list == null) return;

                            string caml = string.IsNullOrEmpty(taskName)
                                ? ""
                                : string.Format(
                                    @"<Where><Eq><FieldRef Name='Title'/>
                                      <Value Type='Text'>{0}</Value></Eq></Where>",
                                    Escape(taskName));
                            var q = new SPQuery { Query = caml };

                            var ids = new List<int>();
                            foreach (SPListItem it in list.GetItems(q))
                                ids.Add(it.ID);
                            foreach (int id in ids)
                                list.Items.DeleteItemById(id);
                        }
                        finally
                        {
                            sub.AllowUnsafeUpdates = false;
                            sub.Dispose();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Tasks",
                    "ClearTaskLog " + (taskName ?? "*"), ex.Message);
            }
        }

        // ----------------------------------------------------------------
        private static SearchConfig LoadFresh(SPSite site)
        {
            var cfg = new SearchConfig();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    using (SPWeb root = s.RootWeb)
                    {
                        SPWeb sub = TryOpenSubsite(s, root);
                        if (sub == null)
                        {
                            SearchIndexingProvisioner.EnsureAllForSite(s);
                            sub = TryOpenSubsite(s, root);
                            if (sub == null) return;
                        }

                        try
                        {
                            cfg.ContentLists  = LoadContentLists(sub);
                            cfg.MenuLists     = LoadMenuLists(sub);
                            cfg.ManualPages   = LoadManualPages(sub);
                            cfg.ExcludedWebs  = LoadHashSet(sub,
                                SearchIndexingProvisioner.LIST_EXC_WEB,  "Title");
                            cfg.ExcludedPages = LoadHashSet(sub,
                                SearchIndexingProvisioner.LIST_EXC_PAGE, "Title");
                            cfg.ExcludedLists = LoadHashSet(sub,
                                SearchIndexingProvisioner.LIST_EXC_LIST, "Title");
                        }
                        finally { sub.Dispose(); }
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning",
                    "LoadFresh", ex.Message);
            }
            return cfg;
        }

        private static SPWeb TryOpenSubsite(SPSite site, SPWeb root)
        {
            try
            {
                string url = root.ServerRelativeUrl.TrimEnd('/')
                           + "/" + SearchIndexingProvisioner.SUBSITE_URL;
                return site.OpenWeb(url);
            }
            catch { return null; }
        }

        private static List<ContentListMap> LoadContentLists(SPWeb web)
        {
            var result = new List<ContentListMap>();
            SPList list = web.Lists.TryGetList(
                SearchIndexingProvisioner.LIST_CONTENT);
            if (list == null) return result;

            var q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Active'/>
                          <Value Type='Boolean'>1</Value></Eq></Where>"
            };

            foreach (SPListItem it in list.GetItems(q))
            {
                var map = new ContentListMap
                {
                    ListTitle       = SafeStr(it, "Title"),
                    ListWebUrl      = NonEmpty(SafeStr(it, "ListWebUrl"), "/"),
                    CategoryAr      = SafeStr(it, "CategoryAr"),
                    CategoryEn      = SafeStr(it, "CategoryEn"),
                    TitleField      = NonEmpty(SafeStr(it, "TitleField"),     "Title"),
                    TitleFieldEn    = NonEmpty(SafeStr(it, "TitleFieldEn"),   "Title_EN"),
                    ContentFields   = SplitCsv(SafeStr(it, "ContentFields")),
                    ContentFieldsEn = SplitCsv(SafeStr(it, "ContentFieldsEn")),
                    DateField       = NonEmpty(SafeStr(it, "DateField"),      "Modified"),
                    UrlPattern      = SafeStr(it, "UrlPattern"),
                    VisibilityField = SafeStr(it, "VisibilityField"),
                    FilterField     = SafeStr(it, "FilterField"),
                    FilterValue     = SafeStr(it, "FilterValue"),
                    FilterMode      = NonEmpty(SafeStr(it, "FilterMode"), "Equals")
                };
                if (!string.IsNullOrEmpty(map.ListTitle))
                    result.Add(map);
            }
            return result;
        }

        private static List<MenuListMap> LoadMenuLists(SPWeb web)
        {
            var result = new List<MenuListMap>();
            SPList list = web.Lists.TryGetList(
                SearchIndexingProvisioner.LIST_MENU);
            if (list == null) return result;

            var q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Active'/>
                          <Value Type='Boolean'>1</Value></Eq></Where>"
            };
            foreach (SPListItem it in list.GetItems(q))
            {
                var map = new MenuListMap
                {
                    ListTitle  = SafeStr(it, "Title"),
                    UrlField   = NonEmpty(SafeStr(it, "UrlField"), "URL"),
                    CategoryAr = SafeStr(it, "CategoryAr"),
                    CategoryEn = SafeStr(it, "CategoryEn")
                };
                if (!string.IsNullOrEmpty(map.ListTitle))
                    result.Add(map);
            }
            return result;
        }

        private static List<ManualPage> LoadManualPages(SPWeb web)
        {
            var result = new List<ManualPage>();
            SPList list = web.Lists.TryGetList(
                SearchIndexingProvisioner.LIST_MANUAL_PAGES);
            if (list == null) return result;

            var q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Active'/>
                          <Value Type='Boolean'>1</Value></Eq></Where>"
            };
            foreach (SPListItem it in list.GetItems(q))
            {
                string url = SafeStr(it, "Title");
                if (string.IsNullOrEmpty(url)) continue;
                result.Add(new ManualPage
                {
                    PageUrl     = url,
                    PageTitleAr = SafeStr(it, "PageTitleAr"),
                    PageTitleEn = SafeStr(it, "PageTitleEn"),
                    CategoryAr  = SafeStr(it, "CategoryAr"),
                    CategoryEn  = SafeStr(it, "CategoryEn")
                });
            }
            return result;
        }

        private static HashSet<string> LoadHashSet(SPWeb web,
            string listTitle, string fieldName)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            SPList list = web.Lists.TryGetList(listTitle);
            if (list == null) return set;

            var q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Active'/>
                          <Value Type='Boolean'>1</Value></Eq></Where>"
            };
            foreach (SPListItem it in list.GetItems(q))
            {
                string v = SafeStr(it, fieldName);
                if (!string.IsNullOrEmpty(v))
                    set.Add(v.Trim().TrimEnd('/').ToLowerInvariant());
            }
            return set;
        }

        // ----------------------------------------------------------------
        private static string SafeStr(SPListItem item, string field)
        {
            try
            {
                if (item == null) return "";
                if (!item.Fields.ContainsField(field)) return "";
                return Convert.ToString(item[field]) ?? "";
            }
            catch { return ""; }
        }

        private static string NonEmpty(string v, string fallback)
        {
            return string.IsNullOrWhiteSpace(v) ? fallback : v;
        }

        private static List<string> SplitCsv(string csv)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(csv)) return result;
            foreach (var part in csv.Split(new[] { ',', ';', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                string t = part.Trim();
                if (!string.IsNullOrEmpty(t)) result.Add(t);
            }
            return result;
        }

        private static string Escape(string s)
        {
            return (s ?? "").Replace("&", "&amp;")
                            .Replace("<", "&lt;")
                            .Replace(">", "&gt;")
                            .Replace("'", "&apos;");
        }
    }
}
