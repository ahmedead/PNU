using System;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Provisioning
{
    /// <summary>
    /// Creates the /SearchIndexing/ subsite under the root web and 9
    /// configuration / log lists used by the search indexer.
    /// </summary>
    public static class SearchIndexingProvisioner
    {
        public const string SUBSITE_URL  = "SearchIndexing";
        public const string SUBSITE_NAME = "Search Indexing";

        public const string LIST_CONTENT       = "ContentListsConfig";
        public const string LIST_MENU          = "MenuListsConfig";
        public const string LIST_EXC_WEB       = "ExcludedWebSites";
        public const string LIST_EXC_PAGE      = "ExcludedPages";
        public const string LIST_EXC_LIST      = "ExcludedLists";
        public const string LIST_INDEXED_WEBS  = "IndexedWebs";
        public const string LIST_MANUAL_PAGES  = "ManualPagesToIndex";
        public const string LIST_MISSED_PAGES  = "MissedPages";
        public const string LIST_INDEX_ERRORS  = "IndexErrors";

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
                EnsureMissedPages(subsite);
                EnsureIndexErrors(subsite);

                SeedDefaultContentLists(subsite);
                SeedDefaultMenuLists(subsite);
            }
            finally
            {
                subsite.AllowUnsafeUpdates = false;
                subsite.Dispose();
            }
        }

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

                return root.Webs.Add(SUBSITE_URL, SUBSITE_NAME,
                    "Configuration site for the custom search indexer.",
                    1033, "STS#0", false, false);
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning",
                    "EnsureSubsite", ex.Message);
                return null;
            }
        }

        // ============================================================
        private static void EnsureContentListsConfig(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_CONTENT) != null) return;
            Guid id = web.Lists.Add(LIST_CONTENT,
                "Lists the search indexer crawls as content.",
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
                "Menu lists the indexer should crawl as URLs.",
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
                "SPWeb URLs to skip.",
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
                "Specific page URLs to skip.",
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
                "List titles to skip wherever they appear.",
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
                "Per-task progress log. Skip-on-rerun.",
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
                "Page URLs to index manually.",
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

        private static void EnsureMissedPages(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_MISSED_PAGES) != null) return;
            Guid id = web.Lists.Add(LIST_MISSED_PAGES,
                "Pages found in one language but missing in the other.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            list.Fields["Title"].Title = "FoundUrl";
            list.Fields["Title"].Update();
            list.Fields.Add("MissedUrl",      SPFieldType.Text,     false);
            list.Fields.Add("FoundLanguage",  SPFieldType.Text,     false);
            list.Fields.Add("MissedLanguage", SPFieldType.Text,     false);
            list.Fields.Add("DetectedAt",     SPFieldType.DateTime, false);
            list.Update();
        }

        private static void EnsureIndexErrors(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_INDEX_ERRORS) != null) return;
            Guid id = web.Lists.Add(LIST_INDEX_ERRORS,
                "Pages or items the indexer failed to process. " +
                "Set 'Retry' to Yes after fixing the source, then run " +
                "the matching task again to retry these.",
                SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            list.Fields["Title"].Title = "PageUrl";
            list.Fields["Title"].Update();
            list.Fields.Add("ErrorMessage", SPFieldType.Note,     false);
            list.Fields.Add("ErrorContext", SPFieldType.Text,     false);
            list.Fields.Add("FirstSeenAt",  SPFieldType.DateTime, false);
            list.Fields.Add("LastSeenAt",   SPFieldType.DateTime, false);
            list.Fields.Add("Attempts",     SPFieldType.Number,   false);
            list.Fields.Add("Retry",        SPFieldType.Boolean,  false);
            list.Fields.Add("Resolved",     SPFieldType.Boolean,  false);
            ((SPFieldBoolean)list.Fields["Retry"]).DefaultValue   = "0";
            list.Fields["Retry"].Update();
            ((SPFieldBoolean)list.Fields["Resolved"]).DefaultValue= "0";
            list.Fields["Resolved"].Update();
            list.Update();
        }

        // ============================================================
        private static void SeedDefaultContentLists(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(LIST_CONTENT);
            if (list == null || list.ItemCount > 0) return;

            AddContent(list, "RequestsList", "/ar/MediaCenter/News",
                "الأخبار", "News",
                "Description,Body", "Description_EN,Body_EN",
                "DisplayDate",
                "/ar/MediaCenter/News/Pages/NewsDetails.aspx?RequestID={ID}",
                "", "MainCategory", "الأخبار الرئيسية", "Equals");

            AddContent(list, "RequestsList", "/ar/MediaCenter/News",
                "الوسائط الرقمية", "Digital Media",
                "Description,Body", "Description_EN,Body_EN",
                "DisplayDate",
                "/ar/MediaCenter/News/Pages/DigitalMediaDetails.aspx?RequestID={ID}",
                "", "MainCategory", "الأخبار الرئيسية", "NotEquals");

            AddContent(list, "AdvertisementsRequests",
                "/ar/MediaCenter/MediaCenterAdmin",
                "الفعاليات والإعلانات", "Events & Announcements",
                "Description,Body", "Description_EN,Body_EN",
                "MediaDate",
                "/ar/MediaCenter/Pages/AdvertisementDetails.aspx?RequestID={ID}",
                "", "", "", "");

            AddContent(list, "EservicesList", "/",
                "الخدمات الإلكترونية", "E-Services",
                "Description", "Description_EN", "Modified",
                "/ar/Pages/service-details.aspx?eti={ID}",
                "", "", "", "");

            AddContent(list, "AllFaculties", "/Admin",
                "الكليات", "Faculties",
                "Description,Vision,Mission",
                "Description_EN,Vision_EN,Mission_EN",
                "Modified",
                "/ar/Faculties/Pages/FacultyDetails.aspx?ID={ID}",
                "", "", "", "");
        }

        private static void SeedDefaultMenuLists(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(LIST_MENU);
            if (list == null) return;

            // Per-row existence check (not ItemCount==0) so re-running
            // the provisioner adds newly-introduced menu lists (e.g. the
            // side menus) to existing deployments without duplicating.
            AddMenuIfMissing(list, "TopMenuLevel1",  "URL", "صفحات رئيسية", "Main Pages");
            AddMenuIfMissing(list, "TopMenuLevel2",  "URL", "صفحات فرعية",  "Sub Pages");
            AddMenuIfMissing(list, "TopMenuLevel3",  "URL", "صفحات داخلية", "Inner Pages");
            AddMenuIfMissing(list, "SideMenuLevel1", "URL", "صفحات جانبية", "Side Pages");
            AddMenuIfMissing(list, "SideMenuLevel2", "URL", "صفحات جانبية فرعية", "Side Sub Pages");
        }

        private static void AddContent(SPList list,
            string listTitle, string listWebUrl, string catAr, string catEn,
            string fields, string fieldsEn, string dateField,
            string urlPattern, string visField,
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

        private static void AddMenuIfMissing(SPList list, string listTitle,
            string urlField, string catAr, string catEn)
        {
            string caml = string.Format(
                @"<Where><Eq><FieldRef Name='Title'/>
                  <Value Type='Text'>{0}</Value></Eq></Where>", listTitle);
            if (list.GetItems(new SPQuery { Query = caml, RowLimit = 1 })
                    .Count > 0) return;
            AddMenu(list, listTitle, urlField, catAr, catEn);
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
