using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Config;
using PNU.Internet.SearchIndex.DAL;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Indexer
{
    public static class SearchIndexer
    {
        // ================================================================
        public static int FullCrawl(Guid siteId)
        {
            int count = 0;
            long logId = SearchIndexDal.StartCrawlLog("Full");
            var errors = new StringBuilder();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb root = site.RootWeb)
                    {
                        SearchConfig cfg = SearchConfigLoader.Load(site);

                        count += MenuPageCrawler.CrawlMenusForOneWeb(
                            root, cfg, errors);

                        count += FacultiesIndexer.CrawlAllFaculties(
                            site, cfg, errors);

                        foreach (var map in cfg.ContentLists)
                        {
                            if (string.Equals(map.ListTitle, "AllFaculties",
                                    StringComparison.OrdinalIgnoreCase))
                                continue;
                            try
                            {
                                using (SPWeb listWeb = OpenWebSafe(site, map.ListWebUrl))
                                {
                                    if (listWeb == null) continue;
                                    if (cfg.IsWebExcluded(listWeb)) continue;
                                    if (cfg.IsListExcluded(map.ListTitle)) continue;

                                    count += ContentListIndexer.IndexList(
                                        listWeb, map, errors);
                                }
                            }
                            catch (Exception ex)
                            {
                                errors.AppendLine("FullCrawl map "
                                    + map.ListTitle + ": " + ex.Message);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                errors.AppendLine("FATAL: " + ex.Message);
                SearchLogger.WriteToLog("Indexer",
                    "FullCrawl " + siteId, ex.Message);
            }
            finally
            {
                SearchIndexDal.EndCrawlLog(logId, count, errors.ToString());
            }
            return count;
        }

        // ================================================================
        public static void IndexSingleListItem(SPListItem item)
        {
            if (item == null) return;
            try
            {
                SearchConfig cfg = SearchConfigLoader.Load(item.Web.Site);
                if (cfg.IsListExcluded(item.ParentList.Title)) return;
                if (cfg.IsWebExcluded(item.Web)) return;

                ContentListMap map = cfg.GetContentMap(item.ParentList.Title);
                if (map == null) return;

                var dto = ContentListIndexer.MapItem(item.Web,
                    item.ParentList, item, map);
                if (dto != null) SearchIndexDal.Upsert(dto);
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Indexer",
                    "IndexSingleListItem " + item.ID, ex.Message);
            }
        }

        public static void IndexSinglePage(SPListItem pageItem)
        {
            if (pageItem == null) return;
            try
            {
                SearchConfig cfg = SearchConfigLoader.Load(pageItem.Web.Site);
                if (cfg.IsWebExcluded(pageItem.Web)) return;

                SPWeb web = pageItem.Web;
                string url = web.Url.TrimEnd('/') + "/"
                           + pageItem.Url.TrimStart('/');
                if (cfg.IsPageExcluded(url)) return;

                string titleAr = SafeStr(pageItem, "Title");
                string titleEn = SafeStr(pageItem, "Title_EN");

                string contentAr = FirstNonEmpty(
                    SafeStr(pageItem, "PublishingPageContent"),
                    SafeStr(pageItem, "Content"),
                    SafeStr(pageItem, "Description"));
                string contentEn = FirstNonEmpty(
                    SafeStr(pageItem, "PublishingPageContent_EN"),
                    SafeStr(pageItem, "Content_EN"),
                    SafeStr(pageItem, "Description_EN"));

                var dto = new SearchIndexItem
                {
                    SourceType = "Page",
                    SourceKey = "MENUPAGE|" + web.Site.ID.ToString("N")
                                 + "|" + url.ToLowerInvariant(),
                    SiteId = web.Site.ID,
                    WebId = web.ID,
                    ListId = pageItem.ParentList.ID,
                    ListItemId = pageItem.ID,
                    TitleAr = titleAr,
                    TitleEn = titleEn,
                    ContentAr = MenuPageCrawler.RemoveBoilerplate(StripHtml(contentAr)),
                    ContentEn = MenuPageCrawler.RemoveBoilerplate(StripHtml(contentEn)),
                    Url = url,
                    CategoryAr = "صفحات",
                    CategoryEn = "Pages",
                    DisplayDate = pageItem["Modified"] != null
                                  ? (DateTime?)pageItem["Modified"] : null
                };
                SearchIndexDal.Upsert(dto);
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Indexer",
                    "IndexSinglePage " + pageItem.ID, ex.Message);
            }
        }

        public static void RemoveListItem(Guid webId, Guid listId, int itemId)
        {
            SearchIndexDal.Deactivate(BuildListItemKey(webId, listId, itemId));
        }

        public static void RemovePage(Guid webId, int itemId)
        {
            SearchIndexDal.Deactivate(BuildPageKey(webId, itemId));
        }

        // ================================================================
        // RETRY: re-index every URL the user marked Retry=Yes in the
        // IndexErrors list. Successful URLs are auto-resolved.
        // ================================================================
        public static int ProcessRetries(Guid siteId, StringBuilder errors)
        {
            if (errors == null) errors = new StringBuilder();
            int count = 0;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        var urls = IndexErrorLogger.GetUrlsMarkedForRetry(site);
                        if (urls == null || urls.Count == 0) return;

                        SearchConfig cfg = SearchConfigLoader.Load(site);
                        using (SPWeb root = site.RootWeb)
                        {
                            foreach (string url in urls)
                            {
                                try
                                {
                                    MenuPageCrawler.IndexManualPage(
                                        root, url, null, cfg, errors);
                                    count++;
                                }
                                catch (Exception ex)
                                {
                                    errors.AppendLine("RETRY " + url
                                        + ": " + ex.Message);
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                errors.AppendLine("ProcessRetries: " + ex.Message);
                SearchLogger.WriteToLog("Indexer", "ProcessRetries",
                    ex.Message);
            }
            return count;
        }

        // ================================================================
        public static string BuildPageKey(Guid webId, int itemId)
        {
            return "PAGE|" + webId.ToString("N") + "|" + itemId;
        }

        public static string BuildListItemKey(Guid webId, Guid listId, int itemId)
        {
            return "LI|" + webId.ToString("N") + "|"
                        + listId.ToString("N") + "|" + itemId;
        }

        private static readonly Regex HtmlTagRegex =
            new Regex("<[^>]+>", RegexOptions.Compiled);
        private static readonly Regex WhitespaceRegex =
            new Regex(@"\s+", RegexOptions.Compiled);

        public static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            string txt = HtmlTagRegex.Replace(html, " ");
            txt = HttpUtility.HtmlDecode(txt);
            txt = WhitespaceRegex.Replace(txt, " ");
            return txt.Trim();
        }

        public static SPWeb OpenWebSafe(SPSite site, string serverRelativeUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(serverRelativeUrl)
                    || serverRelativeUrl == "/")
                    return site.OpenWeb(site.ServerRelativeUrl);
                return site.OpenWeb(serverRelativeUrl);
            }
            catch { return null; }
        }

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

        private static string FirstNonEmpty(params string[] vals)
        {
            foreach (var v in vals)
                if (!string.IsNullOrWhiteSpace(v)) return v;
            return "";
        }
    }
}
