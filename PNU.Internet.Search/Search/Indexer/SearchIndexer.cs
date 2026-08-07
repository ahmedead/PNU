using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using PNU.Internet.Search.DAL;
using Portal.Main.Helper;

namespace PNU.Internet.Search.Indexer
{
    /// <summary>
    /// Crawls the whole site collection (pages + lists) and feeds the
    /// custom search index. Use FullCrawl() from a timer job, and
    /// IndexSinglePage / IndexSingleListItem from event receivers.
    /// </summary>
    public static class SearchIndexer
    {
        // ----------------------------------------------------------------
        //  Lists / libraries to skip during a crawl. Add to this set if
        //  you want to exclude system or back-office lists.
        // ----------------------------------------------------------------
        private static readonly HashSet<string> ExcludedLists = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "Master Page Gallery", "Style Library", "Site Assets",
            "Form Templates", "Theme Gallery", "Solution Gallery",
            "Web Part Gallery", "List Template Gallery",
            "User Information List", "Workflow History",
            "Workflow Tasks", "TaxonomyHiddenList", "Reusable Content",
            "_catalogs", "Composed Looks"
        };

        // Lists you DO want indexed when discovered (matches your project)
        // - if your list isn't here it will still be indexed unless excluded above,
        //   this is just a list of fields-of-interest hints per known list.
        private static readonly HashSet<string> KnownContentLists = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "EservicesList", "AdvertisementsRequests",
            "AchievementsAndAwards", "News", "Events",
            "Faculties", "Policies", "FAQs", "Partners"
        };

        // ----------------------------------------------------------------
        //  PUBLIC API
        // ----------------------------------------------------------------

        /// <summary>Full re-crawl of a site collection.</summary>
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
                    {
                        foreach (SPWeb web in site.AllWebs)
                        {
                            try
                            {
                                count += IndexWeb(web, errors);
                            }
                            catch (Exception exWeb)
                            {
                                errors.AppendLine(
                                    "WEB " + web.Url + ": " + exWeb.Message);
                            }
                            finally
                            {
                                web.Dispose();
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                SearchIndexDal.EndCrawlLog(logId, count, errors.ToString());
            }
            return count;
        }

        /// <summary>Index a single web (pages + lists in that web only).</summary>
        public static int IndexWeb(SPWeb web, StringBuilder errors = null)
        {
            if (web == null) return 0;
            if (errors == null) errors = new StringBuilder();
            int count = 0;

            count += IndexPagesLibrary(web, errors);

            foreach (SPList list in web.Lists)
            {
                if (list.Hidden) continue;
                if (ExcludedLists.Contains(list.Title)) continue;
                if (list.BaseTemplate == SPListTemplateType.DocumentLibrary
                    && !IsPublishingPagesLibrary(list))
                    continue;       // skip plain document libs (handled above for pages)

                try
                {
                    count += IndexList(web, list, errors);
                }
                catch (Exception ex)
                {
                    errors.AppendLine("LIST " + list.Title + ": " + ex.Message);
                }
            }
            return count;
        }

        /// <summary>Index (or re-index) one list item.</summary>
        public static void IndexSingleListItem(SPListItem item)
        {
            if (item == null) return;
            try
            {
                var dto = MapListItem(item);
                if (dto != null) SearchIndexDal.Upsert(dto);
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>Index (or re-index) a single publishing page.</summary>
        public static void IndexSinglePage(SPListItem pageItem)
        {
            if (pageItem == null) return;
            try
            {
                var dto = MapPublishingPage(pageItem);
                if (dto != null) SearchIndexDal.Upsert(dto);
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>Mark an item as deleted (soft).</summary>
        public static void RemoveListItem(Guid webId, Guid listId, int itemId)
        {
            string key = BuildListItemKey(webId, listId, itemId);
            SearchIndexDal.Deactivate(key);
        }

        public static void RemovePage(Guid webId, int itemId)
        {
            string key = BuildPageKey(webId, itemId);
            SearchIndexDal.Deactivate(key);
        }

        // ----------------------------------------------------------------
        //  PRIVATE - library/list crawl
        // ----------------------------------------------------------------

        private static int IndexPagesLibrary(SPWeb web, StringBuilder errors)
        {
            int n = 0;
            try
            {
                if (!PublishingWeb.IsPublishingWeb(web)) return 0;

                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                PublishingPageCollection pages = pubWeb.GetPublishingPages();

                foreach (PublishingPage page in pages)
                {
                    try
                    {
                        var dto = MapPublishingPage(page.ListItem);
                        if (dto != null)
                        {
                            SearchIndexDal.Upsert(dto);
                            n++;
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.AppendLine(
                            "PAGE " + page.Url + ": " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.AppendLine("PAGES " + web.Url + ": " + ex.Message);
            }
            return n;
        }

        private static int IndexList(SPWeb web, SPList list, StringBuilder errors)
        {
            int n = 0;
            SPQuery q = new SPQuery { ViewAttributes = "Scope=\"Recursive\"" };
            SPListItemCollection items = list.GetItems(q);

            foreach (SPListItem item in items)
            {
                try
                {
                    var dto = MapListItem(item);
                    if (dto != null)
                    {
                        SearchIndexDal.Upsert(dto);
                        n++;
                    }
                }
                catch (Exception ex)
                {
                    errors.AppendLine(
                        "ITEM " + list.Title + "#" + item.ID + ": " + ex.Message);
                }
            }
            return n;
        }

        // ----------------------------------------------------------------
        //  PRIVATE - mapping
        // ----------------------------------------------------------------

        private static SearchIndexItem MapPublishingPage(SPListItem item)
        {
            if (item == null) return null;
            SPWeb web = item.Web;

            string titleAr = SafeStr(item, "Title");
            string titleEn = SafeStr(item, "Title_EN");

            // Try common publishing fields for body content
            string contentAr =
                FirstNonEmpty(SafeStr(item, "PublishingPageContent"),
                              SafeStr(item, "Content"),
                              SafeStr(item, "Description"));
            string contentEn =
                FirstNonEmpty(SafeStr(item, "PublishingPageContent_EN"),
                              SafeStr(item, "Content_EN"),
                              SafeStr(item, "Description_EN"));

            contentAr = StripHtml(contentAr);
            contentEn = StripHtml(contentEn);

            string url = web.Url.TrimEnd('/') + "/"
                       + item.Url.TrimStart('/');

            DateTime? displayDate = null;
            if (item["Modified"] != null)
                displayDate = (DateTime)item["Modified"];

            return new SearchIndexItem
            {
                SourceType = "Page",
                SourceKey  = BuildPageKey(web.ID, item.ID),
                SiteId     = web.Site.ID,
                WebId      = web.ID,
                ListId     = item.ParentList.ID,
                ListItemId = item.ID,
                TitleAr    = titleAr,
                TitleEn    = titleEn,
                ContentAr  = contentAr,
                ContentEn  = contentEn,
                Url        = url,
                Category   = "صفحات",                  // "Pages"
                DisplayDate= displayDate
            };
        }

        private static SearchIndexItem MapListItem(SPListItem item)
        {
            if (item == null) return null;
            SPWeb  web  = item.Web;
            SPList list = item.ParentList;

            string titleAr = SafeStr(item, "Title");
            string titleEn = SafeStr(item, "Title_EN");

            // Concatenate every plain-text/note field – that becomes "content"
            var sbAr = new StringBuilder();
            var sbEn = new StringBuilder();

            foreach (SPField f in list.Fields)
            {
                if (f.Hidden || f.ReadOnlyField) continue;
                if (f.Type != SPFieldType.Text &&
                    f.Type != SPFieldType.Note &&
                    f.Type != SPFieldType.Choice)
                    continue;

                string v = SafeStr(item, f.InternalName);
                if (string.IsNullOrWhiteSpace(v)) continue;

                if (f.InternalName.EndsWith("_EN", StringComparison.OrdinalIgnoreCase))
                    sbEn.Append(StripHtml(v)).Append(' ');
                else
                    sbAr.Append(StripHtml(v)).Append(' ');
            }

            // best-guess display URL
            string url = BuildItemDisplayUrl(web, list, item);

            DateTime? displayDate = null;
            string[] dateFieldsToTry =
                { "DisplayDate", "EventDate", "MediaDate", "Modified", "Created" };
            foreach (string df in dateFieldsToTry)
            {
                if (item.Fields.ContainsField(df) && item[df] != null)
                {
                    DateTime dt;
                    if (DateTime.TryParse(item[df].ToString(), out dt))
                    {
                        displayDate = dt;
                        break;
                    }
                }
            }

            return new SearchIndexItem
            {
                SourceType = "ListItem",
                SourceKey  = BuildListItemKey(web.ID, list.ID, item.ID),
                SiteId     = web.Site.ID,
                WebId      = web.ID,
                ListId     = list.ID,
                ListItemId = item.ID,
                TitleAr    = titleAr,
                TitleEn    = titleEn,
                ContentAr  = sbAr.ToString().Trim(),
                ContentEn  = sbEn.ToString().Trim(),
                Url        = url,
                Category   = list.Title,
                DisplayDate= displayDate
            };
        }

        // ----------------------------------------------------------------
        //  PRIVATE - helpers
        // ----------------------------------------------------------------

        private static string BuildItemDisplayUrl(SPWeb web, SPList list, SPListItem item)
        {
            // Special-case the well-known content lists in the project
            switch (list.Title)
            {
                case "AdvertisementsRequests":
                    return web.Url.TrimEnd('/')
                         + "/Pages/AdvertisementDetails.aspx?RequestID=" + item.ID;
                case "News":
                    return web.Url.TrimEnd('/')
                         + "/Pages/NewsDetails.aspx?ID=" + item.ID;
                case "Events":
                    return web.Url.TrimEnd('/')
                         + "/Pages/EventDetails.aspx?ID=" + item.ID;
                case "EservicesList":
                    return web.Url.TrimEnd('/')
                         + "/Pages/EServiceDetails.aspx?ID=" + item.ID;
                case "Policies":
                    return web.Url.TrimEnd('/')
                         + "/Pages/PolicyDetails.aspx?ID=" + item.ID;
            }

            // Fallback: SP default DispForm
            return web.Url.TrimEnd('/') + "/"
                 + list.DefaultDisplayFormUrl.TrimStart('/')
                 + "?ID=" + item.ID;
        }

        public static string BuildPageKey(Guid webId, int itemId)
        {
            return "PAGE|" + webId.ToString("N") + "|" + itemId;
        }

        public static string BuildListItemKey(Guid webId, Guid listId, int itemId)
        {
            return "LI|" + webId.ToString("N") + "|"
                        + listId.ToString("N") + "|" + itemId;
        }

        private static bool IsPublishingPagesLibrary(SPList list)
        {
            return list != null && list.Title.Equals("Pages",
                StringComparison.OrdinalIgnoreCase);
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
    }
}
