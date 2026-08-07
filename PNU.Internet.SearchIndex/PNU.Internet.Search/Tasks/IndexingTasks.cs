using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Config;
using PNU.Internet.SearchIndex.Indexer;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Tasks
{
    public class TaskResult
    {
        public string TaskName    { get; set; }
        public int    ItemsCount  { get; set; }
        public int    WebsTouched { get; set; }
        public int    WebsSkipped { get; set; }
        public string Errors      { get; set; }

        public string Summary
        {
            get
            {
                return string.Format(
                    "Indexed {0} items across {1} web(s). " +
                    "{2} web(s) skipped (already done).",
                    ItemsCount, WebsTouched, WebsSkipped);
            }
        }
    }

    public static class IndexingTasks
    {
        public const string TASK_MENUS         = "Menus";
        public const string TASK_NEWS          = "News";
        public const string TASK_DIGITAL_MEDIA = "DigitalMedia";
        public const string TASK_EVENTS        = "Events";
        public const string TASK_ESERVICES     = "EServices";
        public const string TASK_FACULTIES     = "Faculties";
        public const string TASK_MANUAL_PAGES  = "ManualPages";

        // ================================================================
        public static TaskResult RunMenusTask(Guid siteId)
        {
            var result = new TaskResult { TaskName = TASK_MENUS };
            var errors = new StringBuilder();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb root = site.RootWeb)
                    {
                        SearchConfig cfg = SearchConfigLoader.Load(site);

                        string rootKey = root.ServerRelativeUrl.TrimEnd('/')
                            .ToLowerInvariant();
                        if (string.IsNullOrEmpty(rootKey)) rootKey = "/";

                        if (SearchConfigLoader.IsTaskCompletedForWeb(
                                site, TASK_MENUS, rootKey))
                        {
                            result.WebsSkipped++;
                            return;
                        }

                        int n = MenuPageCrawler.CrawlMenusForOneWeb(
                            root, cfg, errors);
                        result.ItemsCount = n;
                        result.WebsTouched = 1;

                        SearchConfigLoader.MarkTaskCompletedForWeb(
                            site, TASK_MENUS, rootKey, n, errors.ToString());
                    }
                });
            }
            catch (Exception ex)
            {
                errors.AppendLine("FATAL: " + ex.Message);
                SearchLogger.WriteToLog("Tasks",
                    "RunMenusTask " + siteId, ex.Message);
            }

            result.Errors = errors.ToString();
            return result;
        }

        // ================================================================
        public static TaskResult RunNewsTask(Guid siteId)
        {
            return RunSingleListTask(siteId, TASK_NEWS,
                "RequestsList", catFilter: "الأخبار");
        }

        public static TaskResult RunDigitalMediaTask(Guid siteId)
        {
            return RunSingleListTask(siteId, TASK_DIGITAL_MEDIA,
                "RequestsList", catFilter: "الوسائط الرقمية");
        }

        public static TaskResult RunEventsTask(Guid siteId)
        {
            return RunSingleListTask(siteId, TASK_EVENTS,
                "AdvertisementsRequests", catFilter: null);
        }

        public static TaskResult RunEServicesTask(Guid siteId)
        {
            return RunSingleListTask(siteId, TASK_ESERVICES,
                "EservicesList", catFilter: null);
        }

        // ================================================================
        public static TaskResult RunFacultiesTask(Guid siteId)
        {
            var result = new TaskResult { TaskName = TASK_FACULTIES };
            var errors = new StringBuilder();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        SearchConfig cfg = SearchConfigLoader.Load(site);

                        const string adminWebKey = "/admin";
                        if (SearchConfigLoader.IsTaskCompletedForWeb(
                                site, TASK_FACULTIES, adminWebKey))
                        {
                            result.WebsSkipped++;
                            return;
                        }

                        int n = FacultiesIndexer.CrawlAllFaculties(
                            site, cfg, errors);
                        result.ItemsCount = n;
                        result.WebsTouched = 1;

                        SearchConfigLoader.MarkTaskCompletedForWeb(
                            site, TASK_FACULTIES, adminWebKey, n,
                            errors.ToString());
                    }
                });
            }
            catch (Exception ex)
            {
                errors.AppendLine("FATAL: " + ex.Message);
                SearchLogger.WriteToLog("Tasks",
                    "RunFacultiesTask " + siteId, ex.Message);
            }

            result.Errors = errors.ToString();
            return result;
        }

        // ================================================================
        public static TaskResult RunManualPagesTask(Guid siteId)
        {
            var result = new TaskResult { TaskName = TASK_MANUAL_PAGES };
            var errors = new StringBuilder();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        SearchConfig cfg = SearchConfigLoader.Load(site);
                        if (cfg.ManualPages == null
                            || cfg.ManualPages.Count == 0) return;

                        foreach (var p in cfg.ManualPages)
                        {
                            try
                            {
                                string absoluteUrl = MakeAbsolute(p.PageUrl,
                                    site.RootWeb);
                                if (cfg.IsPageExcluded(absoluteUrl)) continue;

                                MenuPageCrawler.IndexManualPage(
                                    site.RootWeb, absoluteUrl, p, cfg, errors);
                                result.ItemsCount++;
                            }
                            catch (Exception ex)
                            {
                                errors.AppendLine("MANUAL " + p.PageUrl
                                    + ": " + ex.Message);
                            }
                        }
                        result.WebsTouched = 1;
                    }
                });
            }
            catch (Exception ex)
            {
                errors.AppendLine("FATAL: " + ex.Message);
                SearchLogger.WriteToLog("Tasks",
                    "RunManualPagesTask " + siteId, ex.Message);
            }

            result.Errors = errors.ToString();
            return result;
        }

        // ================================================================
        private static TaskResult RunSingleListTask(Guid siteId,
            string taskName, string listTitle, string catFilter)
        {
            var result = new TaskResult { TaskName = taskName };
            var errors = new StringBuilder();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        SearchConfig cfg = SearchConfigLoader.Load(site);

                        var maps = new List<ContentListMap>();
                        foreach (var m in cfg.ContentLists)
                        {
                            if (!string.Equals(m.ListTitle, listTitle,
                                    StringComparison.OrdinalIgnoreCase))
                                continue;
                            if (!string.IsNullOrEmpty(catFilter)
                                && !string.Equals(m.CategoryAr, catFilter,
                                    StringComparison.OrdinalIgnoreCase)
                                && !string.Equals(m.CategoryEn, catFilter,
                                    StringComparison.OrdinalIgnoreCase))
                                continue;
                            maps.Add(m);
                        }

                        if (maps.Count == 0)
                        {
                            errors.AppendLine("No active config row for "
                                + listTitle
                                + (catFilter != null ? " (" + catFilter + ")" : ""));
                            return;
                        }

                        foreach (var map in maps)
                        {
                            try
                            {
                                using (SPWeb listWeb = SearchIndexer.OpenWebSafe(
                                    site, map.ListWebUrl))
                                {
                                    if (listWeb == null)
                                    {
                                        errors.AppendLine("Cannot open web "
                                            + map.ListWebUrl);
                                        continue;
                                    }

                                    string webKey = listWeb.ServerRelativeUrl
                                        .TrimEnd('/').ToLowerInvariant();
                                    string skipKey = taskName + "::" + map.CategoryAr;

                                    if (SearchConfigLoader.IsTaskCompletedForWeb(
                                            site, skipKey, webKey))
                                    {
                                        result.WebsSkipped++;
                                        continue;
                                    }

                                    if (cfg.IsWebExcluded(listWeb)) continue;
                                    if (cfg.IsListExcluded(map.ListTitle)) continue;

                                    int n = ContentListIndexer.IndexList(
                                        listWeb, map, errors);
                                    result.ItemsCount += n;
                                    result.WebsTouched++;

                                    SearchConfigLoader.MarkTaskCompletedForWeb(
                                        site, skipKey, webKey, n,
                                        errors.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                errors.AppendLine("MAP " + map.ListTitle
                                    + " @ " + map.ListWebUrl + ": "
                                    + ex.Message);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                errors.AppendLine("FATAL: " + ex.Message);
                SearchLogger.WriteToLog("Tasks", taskName, ex.Message);
            }

            result.Errors = errors.ToString();
            return result;
        }

        // ----------------------------------------------------------------
        private static string MakeAbsolute(string url, SPWeb web)
        {
            if (string.IsNullOrEmpty(url)) return url;
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return url;
            return web.Site.Url.TrimEnd('/')
                 + (url.StartsWith("/") ? url : "/" + url);
        }
    }
}
