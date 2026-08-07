using System;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Provisioning
{
    /// <summary>
    /// Creates the /SearchIndexing/ subsite under the root web and 7
    /// configuration / log lists used by the search indexer.
    /// </summary>
    public static class SearchIndexingProvisioner
    {
        public const string SUBSITE_URL  = "SearchIndexing";
        public const string SUBSITE_NAME = "Search Indexing";

        public const string LIST_CONTENT      = "ContentListsConfig";
        public const string LIST_MENU         = "MenuListsConfig";
        public const string LIST_EXC_WEB      = "ExcludedWebSites";
        public const string LIST_EXC_PAGE     = "ExcludedPages";
        public const string LIST_EXC_LIST     = "ExcludedLists";
        public const string LIST_INDEXED_WEBS = "IndexedWebs";
        public const string LIST_MANUAL_PAGES = "ManualPagesToIndex";

        // ----------------------------------------------------------------
        public static void EnsureAll()
        {
            if (SPContext.Current == null) return;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        EnsureAllInternal(site);
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning", "EnsureAll", ex.Message);
            }
        }

        public static void EnsureAllForSite(SPSite site)
        {
            if (site == null) return;
            try { EnsureAllInternal(site); }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning",
                    "EnsureAllForSite " + site.Url, ex.Message);
            }
        }

        private static void EnsureAllInternal(SPSite site)
        {
            SPWeb subsite = EnsureSubsite(site);
            if (subsite == null) return;

            try
            {
                subsite.AllowUnsafeUpdates = true;

                EnsureContentListsConfig(subsite);
                EnsureMenuListsConfig(subsite);
                EnsureExcludedWebSites(subsite);
                EnsureExcludedPages(subsite);
                EnsureExcludedLists(subsite);
                EnsureIndexedWebs(subsite);
                EnsureManualPagesToIndex(subsite);

                SeedDefaultContentLists(subsite);
                SeedDefaultMenuLists(subsite);
            }
            finally
            {
                subsite.AllowUnsafeUpdates = false;
                subsite.Dispose();
            }
        }

        // ----------------------------------------------------------------
        private static SPWeb EnsureSubsite(SPSite site)
        {
            SPWeb root = site.RootWeb;
            try
            {
                foreach (SPWeb sub in root.Webs)
                {
                    string segments = sub.ServerRelativeUrl.TrimEnd('/');
                    string lastSegment = segments.Substring(
                        segments.LastIndexOf('/') + 1);
                    bool match = string.Equals(lastSegment, SUBSITE_URL,
                        StringComparison.OrdinalIgnoreCase);
                    sub.Dispose();
                    if (match) return root.Webs[SUBSITE_URL];
                }

                SPWeb newWeb = root.Webs.Add(
                    SUBSITE_URL, SUBSITE_NAME,
                    "Configuration site for the custom search indexer.",
                    1033, "STS#0", false, false);
                return newWeb;
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning",
                    "EnsureSubsite", ex.Message);
                return null;
            }
        }

        // ================================================================
        private static void EnsureContentListsConfig(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_CONTENT) != null) return;

            Guid id = web.Lists.Add(LIST_CONTENT,
                "Lists that the search indexer should crawl as content.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            list.Fields["Title"].Title = "ListTitle";
            list.Fields["Title"].Update();

            list.Fields.Add("ListWebUrl",      SPFieldType.Text,    false);
            list.Fields.Add("CategoryAr",      SPFieldType.Text,    false);
            list.Fields.Add("CategoryEn",      SPFieldType.Text,    false);
            list.Fields.Add("TitleField",      SPFieldType.Text,    false);
            list.Fields.Add("TitleFieldEn",    SPFieldType.Text,    false);
            list.Fields.Add("ContentFields",   SPFieldType.Note,    false);
            list.Fields.Add("ContentFieldsEn", SPFieldType.Note,    false);
            list.Fields.Add("DateField",       SPFieldType.Text,    false);
            list.Fields.Add("UrlPattern",      SPFieldType.Text,    false);
            list.Fields.Add("VisibilityField", SPFieldType.Text,    false);
            list.Fields.Add("FilterField",     SPFieldType.Text,    false);
            list.Fields.Add("FilterValue",     SPFieldType.Text,    false);
            list.Fields.Add("FilterMode",      SPFieldType.Text,    false);
            list.Fields.Add("Active",          SPFieldType.Boolean, false);

            ((SPFieldBoolean)list.Fields["Active"]).DefaultValue = "1";
            list.Fields["Active"].Update();

            list.Update();
        }

        private static void EnsureMenuListsConfig(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_MENU) != null) return;

            Guid id = web.Lists.Add(LIST_MENU,
                "Menu lists whose URL fields the indexer should crawl.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            list.Fields["Title"].Title = "ListTitle";
            list.Fields["Title"].Update();

            list.Fields.Add("UrlField",   SPFieldType.Text,    false);
            list.Fields.Add("CategoryAr", SPFieldType.Text,    false);
            list.Fields.Add("CategoryEn", SPFieldType.Text,    false);
            list.Fields.Add("Active",     SPFieldType.Boolean, false);
            ((SPFieldBoolean)list.Fields["Active"]).DefaultValue = "1";
            list.Fields["Active"].Update();
            list.Update();
        }

        private static void EnsureExcludedWebSites(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_EXC_WEB) != null) return;
            Guid id = web.Lists.Add(LIST_EXC_WEB,
                "SPWeb URLs to exclude from indexing.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            list.Fields["Title"].Title = "WebUrl";
            list.Fields["Title"].Update();
            list.Fields.Add("Reason", SPFieldType.Note, false);
            list.Fields.Add("Active", SPFieldType.Boolean, false);
            ((SPFieldBoolean)list.Fields["Active"]).DefaultValue = "1";
            list.Fields["Active"].Update();
            list.Update();
        }

        private static void EnsureExcludedPages(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_EXC_PAGE) != null) return;
            Guid id = web.Lists.Add(LIST_EXC_PAGE,
                "Specific page URLs that should never be indexed.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            list.Fields["Title"].Title = "PageUrl";
            list.Fields["Title"].Update();
            list.Fields.Add("Reason", SPFieldType.Note, false);
            list.Fields.Add("Active", SPFieldType.Boolean, false);
            ((SPFieldBoolean)list.Fields["Active"]).DefaultValue = "1";
            list.Fields["Active"].Update();
            list.Update();
        }

        private static void EnsureExcludedLists(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_EXC_LIST) != null) return;
            Guid id = web.Lists.Add(LIST_EXC_LIST,
                "List titles that should be skipped wherever they appear.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            list.Fields["Title"].Title = "ListTitle";
            list.Fields["Title"].Update();
            list.Fields.Add("Reason", SPFieldType.Note, false);
            list.Fields.Add("Active", SPFieldType.Boolean, false);
            ((SPFieldBoolean)list.Fields["Active"]).DefaultValue = "1";
            list.Fields["Active"].Update();
            list.Update();
        }

        private static void EnsureIndexedWebs(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_INDEXED_WEBS) != null) return;
            Guid id = web.Lists.Add(LIST_INDEXED_WEBS,
                "Tracks completed indexing tasks per web. Skip-on-rerun.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            list.Fields["Title"].Title = "TaskName";
            list.Fields["Title"].Update();
            list.Fields.Add("WebUrl",      SPFieldType.Text,     false);
            list.Fields.Add("CompletedAt", SPFieldType.DateTime, false);
            list.Fields.Add("ItemsCount",  SPFieldType.Number,   false);
            list.Fields.Add("Errors",      SPFieldType.Note,     false);
            list.Update();
        }

        private static void EnsureManualPagesToIndex(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_MANUAL_PAGES) != null) return;
            Guid id = web.Lists.Add(LIST_MANUAL_PAGES,
                "Page URLs to index manually when auto-discovery misses them.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            list.Fields["Title"].Title = "PageUrl";
            list.Fields["Title"].Update();
            list.Fields.Add("PageTitleAr", SPFieldType.Text,    false);
            list.Fields.Add("PageTitleEn", SPFieldType.Text,    false);
            list.Fields.Add("CategoryAr",  SPFieldType.Text,    false);
            list.Fields.Add("CategoryEn",  SPFieldType.Text,    false);
            list.Fields.Add("Active",      SPFieldType.Boolean, false);
            ((SPFieldBoolean)list.Fields["Active"]).DefaultValue = "1";
            list.Fields["Active"].Update();
            list.Update();
        }

        // ================================================================
        private static void SeedDefaultContentLists(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(LIST_CONTENT);
            if (list == null || list.ItemCount > 0) return;

            // News (RequestsList where MainCategory = الأخبار الرئيسية)
            AddContent(list,
                listTitle:   "RequestsList",
                listWebUrl:  "/ar/MediaCenter/News",
                catAr:       "الأخبار", catEn: "News",
                fields:      "Description,Body",
                fieldsEn:    "Description_EN,Body_EN",
                dateField:   "DisplayDate",
                urlPattern:  "/ar/MediaCenter/News/Pages/NewsDetails.aspx?RequestID={ID}",
                visField:    "",
                filterField: "MainCategory",
                filterValue: "الأخبار الرئيسية",
                filterMode:  "Equals");

            // Digital Media (same list, opposite filter)
            AddContent(list,
                listTitle:   "RequestsList",
                listWebUrl:  "/ar/MediaCenter/News",
                catAr:       "الوسائط الرقمية", catEn: "Digital Media",
                fields:      "Description,Body",
                fieldsEn:    "Description_EN,Body_EN",
                dateField:   "DisplayDate",
                urlPattern:  "/ar/MediaCenter/News/Pages/DigitalMediaDetails.aspx?RequestID={ID}",
                visField:    "",
                filterField: "MainCategory",
                filterValue: "الأخبار الرئيسية",
                filterMode:  "NotEquals");

            // Events / AdvertisementsRequests
            AddContent(list,
                listTitle:   "AdvertisementsRequests",
                listWebUrl:  "/ar/MediaCenter/MediaCenterAdmin",
                catAr:       "الفعاليات والإعلانات", catEn: "Events & Announcements",
                fields:      "Description,Body",
                fieldsEn:    "Description_EN,Body_EN",
                dateField:   "MediaDate",
                urlPattern:  "/ar/MediaCenter/Pages/AdvertisementDetails.aspx?RequestID={ID}",
                visField:    "",
                filterField: "", filterValue: "", filterMode: "");

            // EServices
            AddContent(list,
                listTitle:   "EservicesList",
                listWebUrl:  "/",
                catAr:       "الخدمات الإلكترونية", catEn: "E-Services",
                fields:      "Description",
                fieldsEn:    "Description_EN",
                dateField:   "Modified",
                urlPattern:  "/ar/Pages/service-details.aspx?eti={ID}",
                visField:    "",
                filterField: "", filterValue: "", filterMode: "");

            // AllFaculties under /Admin
            AddContent(list,
                listTitle:   "AllFaculties",
                listWebUrl:  "/Admin",
                catAr:       "الكليات", catEn: "Faculties",
                fields:      "Description,Vision,Mission",
                fieldsEn:    "Description_EN,Vision_EN,Mission_EN",
                dateField:   "Modified",
                urlPattern:  "/ar/Faculties/Pages/FacultyDetails.aspx?ID={ID}",
                visField:    "",
                filterField: "", filterValue: "", filterMode: "");
        }

        private static void SeedDefaultMenuLists(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(LIST_MENU);
            if (list == null || list.ItemCount > 0) return;

            AddMenu(list, "TopMenuLevel1", "URL", "صفحات رئيسية", "Main Pages");
            AddMenu(list, "TopMenuLevel2", "URL", "صفحات فرعية",  "Sub Pages");
            AddMenu(list, "TopMenuLevel3", "URL", "صفحات داخلية", "Inner Pages");
        }

        private static void AddContent(SPList list,
            string listTitle, string listWebUrl,
            string catAr, string catEn,
            string fields, string fieldsEn,
            string dateField, string urlPattern, string visField,
            string filterField, string filterValue, string filterMode)
        {
            SPListItem item = list.Items.Add();
            item["Title"]           = listTitle;
            item["ListWebUrl"]      = listWebUrl;
            item["CategoryAr"]      = catAr;
            item["CategoryEn"]      = catEn;
            item["TitleField"]      = "Title";
            item["TitleFieldEn"]    = "Title_EN";
            item["ContentFields"]   = fields;
            item["ContentFieldsEn"] = fieldsEn;
            item["DateField"]       = dateField;
            item["UrlPattern"]      = urlPattern;
            item["VisibilityField"] = visField;
            item["FilterField"]     = filterField;
            item["FilterValue"]     = filterValue;
            item["FilterMode"]      = filterMode;
            item["Active"]          = true;
            item.Update();
        }

        private static void AddMenu(SPList list, string listTitle,
            string urlField, string catAr, string catEn)
        {
            SPListItem item = list.Items.Add();
            item["Title"]      = listTitle;
            item["UrlField"]   = urlField;
            item["CategoryAr"] = catAr;
            item["CategoryEn"] = catEn;
            item["Active"]     = true;
            item.Update();
        }
    }
}
