using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Config;
using PNU.Internet.SearchIndex.DAL;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Indexer
{
    public static class FacultiesIndexer
    {
        private const string DEFAULT_ADMIN_WEB = "/Admin";
        private const string LIST_TITLE       = "AllFaculties";

        public static int CrawlAllFaculties(SPSite site,
            SearchConfig cfg, StringBuilder errors)
        {
            if (site == null) return 0;
            if (errors == null) errors = new StringBuilder();
            int count = 0;

            ContentListMap map = cfg.GetContentMap(LIST_TITLE);
            string adminWebUrl = (map != null && !string.IsNullOrEmpty(map.ListWebUrl))
                ? map.ListWebUrl : DEFAULT_ADMIN_WEB;

            try
            {
                using (SPWeb adminWeb = site.OpenWeb(adminWebUrl))
                {
                    if (adminWeb == null || !adminWeb.Exists)
                    {
                        errors.AppendLine("Admin web not found at " + adminWebUrl);
                        return 0;
                    }

                    SPList list = adminWeb.Lists.TryGetList(LIST_TITLE);
                    if (list == null)
                    {
                        errors.AppendLine(LIST_TITLE
                            + " list not found in " + adminWebUrl);
                        return 0;
                    }

                    foreach (SPListItem item in list.GetItems(new SPQuery
                    {
                        ViewAttributes = "Scope=\"Recursive\""
                    }))
                    {
                        string url = "";
                        try
                        {
                            var dto = MapFacultyItem(adminWeb, list, item, map);
                            if (dto == null) continue;
                            url = dto.Url;

                            SearchIndexDal.Upsert(dto);
                            IndexErrorLogger.MarkResolved(site, url);
                            count++;
                        }
                        catch (Exception ex)
                        {
                            string ctx = "Faculty#" + item.ID;
                            errors.AppendLine(ctx + ": " + ex.Message);
                            if (!string.IsNullOrEmpty(url))
                                IndexErrorLogger.Log(site, url, ctx, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.AppendLine("AllFaculties: " + ex.Message);
                SearchLogger.WriteToLog("Indexer",
                    "FacultiesIndexer", ex.Message);
            }
            return count;
        }

        private static SearchIndexItem MapFacultyItem(SPWeb adminWeb,
            SPList list, SPListItem item, ContentListMap map)
        {
            string titleAr = SafeStr(item, "Title");
            string titleEn = SafeStr(item, "Title_EN");

            var sbAr = new StringBuilder();
            var sbEn = new StringBuilder();

            List<string> arFields = (map != null) ? map.ContentFields   : null;
            List<string> enFields = (map != null) ? map.ContentFieldsEn : null;

            if (arFields != null && arFields.Count > 0)
            {
                foreach (var f in arFields)
                {
                    string v = SafeStr(item, f);
                    if (!string.IsNullOrWhiteSpace(v))
                        sbAr.Append(v).Append(' ');
                }
            }
            else AutoCollect(item, list, sbAr, isEn: false);

            if (enFields != null && enFields.Count > 0)
            {
                foreach (var f in enFields)
                {
                    string v = SafeStr(item, f);
                    if (!string.IsNullOrWhiteSpace(v))
                        sbEn.Append(v).Append(' ');
                }
            }
            else AutoCollect(item, list, sbEn, isEn: true);

            string url = "";
            if (map != null && !string.IsNullOrEmpty(map.UrlPattern))
            {
                url = map.UrlPattern
                    .Replace("{ID}",     item.ID.ToString())
                    .Replace("{Title}",  System.Web.HttpUtility.UrlEncode(titleAr ?? ""))
                    .Replace("{WebUrl}", adminWeb.Site.Url.TrimEnd('/'));
                if (url.StartsWith("/"))
                    url = adminWeb.Site.Url.TrimEnd('/') + url;
            }
            else
            {
                string[] urlFields = { "FacultyUrl", "URL", "Url", "PageUrl" };
                foreach (string f in urlFields)
                {
                    string v = SafeStr(item, f);
                    if (string.IsNullOrEmpty(v)) continue;
                    try
                    {
                        var u = new SPFieldUrlValue(v);
                        if (!string.IsNullOrEmpty(u.Url)) { url = u.Url; break; }
                    }
                    catch { url = v; break; }
                }
                if (string.IsNullOrEmpty(url))
                    url = adminWeb.Site.Url.TrimEnd('/')
                        + "/ar/Faculties/Pages/FacultyDetails.aspx?ID=" + item.ID;
            }

            DateTime? displayDate = null;
            if (item["Modified"] != null)
                displayDate = (DateTime)item["Modified"];

            string catAr = (map != null && !string.IsNullOrEmpty(map.CategoryAr))
                ? map.CategoryAr : "الكليات";
            string catEn = (map != null && !string.IsNullOrEmpty(map.CategoryEn))
                ? map.CategoryEn : "Faculties";

            return new SearchIndexItem
            {
                SourceType = "ListItem",
                SourceKey  = "FACULTY|" + adminWeb.ID.ToString("N")
                           + "|" + list.ID.ToString("N") + "|" + item.ID,
                SiteId     = adminWeb.Site.ID,
                WebId      = adminWeb.ID,
                ListId     = list.ID,
                ListItemId = item.ID,
                TitleAr    = titleAr,
                TitleEn    = titleEn,
                ContentAr  = MenuPageCrawler.RemoveBoilerplate(
                                SearchIndexer.StripHtml(sbAr.ToString().Trim())),
                ContentEn  = MenuPageCrawler.RemoveBoilerplate(
                                SearchIndexer.StripHtml(sbEn.ToString().Trim())),
                Url        = url,
                CategoryAr = catAr,
                CategoryEn = catEn,
                DisplayDate= displayDate
            };
        }

        private static void AutoCollect(SPListItem item, SPList list,
            StringBuilder sb, bool isEn)
        {
            foreach (SPField f in list.Fields)
            {
                if (f.Hidden || f.ReadOnlyField) continue;
                if (f.Type != SPFieldType.Text &&
                    f.Type != SPFieldType.Note &&
                    f.Type != SPFieldType.Choice) continue;

                bool isEnField = f.InternalName.EndsWith("_EN",
                    StringComparison.OrdinalIgnoreCase);
                if (isEn != isEnField) continue;

                string v = SafeStr(item, f.InternalName);
                if (!string.IsNullOrWhiteSpace(v))
                    sb.Append(v).Append(' ');
            }
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
    }
}
