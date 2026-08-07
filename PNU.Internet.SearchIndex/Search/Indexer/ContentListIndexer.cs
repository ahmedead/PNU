using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Config;
using PNU.Internet.SearchIndex.DAL;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Indexer
{
    public static class ContentListIndexer
    {
        public static int IndexList(SPWeb web, ContentListMap map,
            StringBuilder errors)
        {
            int n = 0;
            SPList list = web.Lists.TryGetList(map.ListTitle);
            if (list == null)
            {
                errors.AppendLine("List not found: " + map.ListTitle
                    + " in web " + web.ServerRelativeUrl);
                return 0;
            }

            string caml = BuildCaml(map);
            var q = new SPQuery
            {
                Query = caml,
                ViewAttributes = "Scope=\"Recursive\""
            };

            foreach (SPListItem item in list.GetItems(q))
            {
                string itemUrl = "";
                try
                {
                    if (!PassesFilter(item, map)) continue;
                    var dto = MapItem(web, list, item, map);
                    if (dto == null) continue;
                    itemUrl = dto.Url;

                    SearchIndexDal.Upsert(dto);
                    IndexErrorLogger.MarkResolved(web.Site, itemUrl);
                    n++;
                }
                catch (Exception ex)
                {
                    string ctx = "ContentList:" + map.ListTitle + "#" + item.ID;
                    errors.AppendLine(ctx + ": " + ex.Message);
                    if (!string.IsNullOrEmpty(itemUrl))
                        IndexErrorLogger.Log(web.Site, itemUrl, ctx, ex.Message);
                }
            }
            return n;
        }

        public static SearchIndexItem MapItem(SPWeb web, SPList list,
            SPListItem item, ContentListMap map)
        {
            string titleAr = SafeStr(item, map.TitleField);
            string titleEn = SafeStr(item, map.TitleFieldEn);

            string contentAr = BuildContent(item, map.ContentFields, isEn: false);
            string contentEn = BuildContent(item, map.ContentFieldsEn, isEn: true);

            DateTime? displayDate = null;
            if (!string.IsNullOrEmpty(map.DateField)
                && item.Fields.ContainsField(map.DateField)
                && item[map.DateField] != null)
            {
                DateTime dt;
                if (DateTime.TryParse(item[map.DateField].ToString(), out dt))
                    displayDate = dt;
            }

            string url = ExpandUrl(map.UrlPattern, web, item);

            // Category-aware key so a single item that qualifies for two
            // configs (e.g. RequestsList → News + Digital Media) writes
            // two distinct rows.
            string sourceKey = "LI|" + web.ID.ToString("N") + "|"
                             + list.ID.ToString("N") + "|" + item.ID
                             + "|" + (map.CategoryAr ?? map.CategoryEn ?? "");

            return new SearchIndexItem
            {
                SourceType = "ListItem",
                SourceKey = sourceKey,
                SiteId = web.Site.ID,
                WebId = web.ID,
                ListId = list.ID,
                ListItemId = item.ID,
                TitleAr = titleAr,
                TitleEn = titleEn,
                ContentAr = MenuPageCrawler.RemoveBoilerplate(
                                SearchIndexer.StripHtml(contentAr)),
                ContentEn = MenuPageCrawler.RemoveBoilerplate(
                                SearchIndexer.StripHtml(contentEn)),
                Url = url,
                CategoryAr = map.CategoryAr,
                CategoryEn = map.CategoryEn,
                DisplayDate = displayDate
            };
        }

        // ----------------------------------------------------------------
        private static string BuildCaml(ContentListMap map)
        {
            string visClause = "";
            if (!string.IsNullOrEmpty(map.VisibilityField))
            {
                visClause = string.Format(
                    "<Eq><FieldRef Name='{0}'/>" +
                    "<Value Type='Boolean'>1</Value></Eq>",
                    map.VisibilityField);
            }

            string filterClause = "";
            if (!string.IsNullOrEmpty(map.FilterField)
                && !string.IsNullOrEmpty(map.FilterValue))
            {
                bool isNotEquals = string.Equals(map.FilterMode, "NotEquals",
                    StringComparison.OrdinalIgnoreCase);
                string op = isNotEquals ? "Neq" : "Eq";

                filterClause = string.Format(
                    "<{0}><FieldRef Name='{1}'/>" +
                    "<Value Type='Text'>{2}</Value></{0}>",
                    op, map.FilterField, EscapeCaml(map.FilterValue));
            }

            string clauses;
            if (visClause.Length > 0 && filterClause.Length > 0)
                clauses = "<And>" + visClause + filterClause + "</And>";
            else if (visClause.Length > 0)
                clauses = visClause;
            else if (filterClause.Length > 0)
                clauses = filterClause;
            else
                return "";

            return "<Where>" + clauses + "</Where>";
        }

        private static bool PassesFilter(SPListItem item, ContentListMap map)
        {
            if (string.IsNullOrEmpty(map.FilterField)) return true;
            if (!item.Fields.ContainsField(map.FilterField)) return true;

            string val = SafeStr(item, map.FilterField);
            bool eq = string.Equals(val, map.FilterValue,
                StringComparison.OrdinalIgnoreCase);

            return string.Equals(map.FilterMode, "NotEquals",
                StringComparison.OrdinalIgnoreCase) ? !eq : eq;
        }

        private static string BuildContent(SPListItem item,
            List<string> fields, bool isEn)
        {
            var sb = new StringBuilder();

            if (fields != null && fields.Count > 0)
            {
                foreach (var f in fields)
                {
                    string v = SafeStr(item, f);
                    if (!string.IsNullOrWhiteSpace(v))
                        sb.Append(v).Append(' ');
                }
                return sb.ToString().Trim();
            }

            foreach (SPField f in item.Fields)
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
            return sb.ToString().Trim();
        }

        private static string ExpandUrl(string pattern, SPWeb web, SPListItem item)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return web.Url.TrimEnd('/') + "/"
                     + item.ParentList.DefaultDisplayFormUrl.TrimStart('/')
                     + "?ID=" + item.ID;
            }

            string baseUrl = web.Site.Url.TrimEnd('/');
            string expanded = pattern
                .Replace("{WebUrl}", web.Url.TrimEnd('/'))
                .Replace("{ID}", item.ID.ToString())
                .Replace("{ListGuid}", item.ParentList.ID.ToString())
                .Replace("{Title}", System.Web.HttpUtility.UrlEncode(
                                            SafeStr(item, "Title") ?? ""));
            if (expanded.StartsWith("/"))
                expanded = baseUrl + expanded;
            return expanded;
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

        private static string EscapeCaml(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }
}
