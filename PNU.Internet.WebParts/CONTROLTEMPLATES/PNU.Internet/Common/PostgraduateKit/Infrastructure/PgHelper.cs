using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Shared utility methods for Postgraduate Academic Kit user controls:
    /// Safe* item readers, language helpers, HTML encoding, multi-line parsing
    /// and phase-aware list queries.
    /// </summary>
    public static class PgHelper
    {
        public static bool IsArabic
        {
            get
            {
                try
                {
                    return SPContext.Current != null && SPContext.Current.Web != null
                        ? SPContext.Current.Web.Language == 1025
                        : true;
                }
                catch { return true; }
            }
        }

        public static string GetRes(string key, string fallbackAr, string fallbackEn)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && HttpContext.Current != null)
                {
                    object val = HttpContext.GetGlobalResourceObject("PNUres", key);
                    string s = val as string;
                    if (!string.IsNullOrEmpty(s)) return s;
                }
            }
            catch { /* fall through */ }

            return IsArabic ? fallbackAr : fallbackEn;
        }

        public static string Pick(string ar, string en)
        {
            if (IsArabic) return string.IsNullOrEmpty(ar) ? (en ?? string.Empty) : ar;
            return string.IsNullOrEmpty(en) ? (ar ?? string.Empty) : en;
        }

        /// <summary>Reads the Arabic field and its "_EN" twin, then picks by current language.</summary>
        public static string PickField(SPListItem item, string field)
        {
            return Pick(SafeString(item, field), SafeString(item, field + "_EN"));
        }

        public static string SafeString(SPListItem item, string field)
        {
            try
            {
                if (item == null) return string.Empty;
                if (!item.Fields.ContainsField(field)) return string.Empty;
                return Convert.ToString(item[field]) ?? string.Empty;
            }
            catch { return string.Empty; }
        }

        public static int SafeInt(SPListItem item, string field)
        {
            try
            {
                int v;
                return int.TryParse(SafeString(item, field), NumberStyles.Any,
                                    CultureInfo.InvariantCulture, out v) ? v : 0;
            }
            catch { return 0; }
        }

        public static bool SafeBool(SPListItem item, string field)
        {
            try
            {
                string raw = SafeString(item, field).Trim();
                if (raw.Length == 0) return false;
                if (raw == "1" || raw == "-1") return true;
                if (raw.Equals("Yes", StringComparison.OrdinalIgnoreCase)) return true;
                if (raw.Equals("نعم", StringComparison.Ordinal)) return true;

                bool value;
                return bool.TryParse(raw, out value) && value;
            }
            catch { return false; }
        }

        public static string SafeUrl(SPListItem item, string field)
        {
            try
            {
                string raw = SafeString(item, field);
                if (string.IsNullOrEmpty(raw)) return string.Empty;
                return new SPFieldUrlValue(raw).Url ?? string.Empty;
            }
            catch { return string.Empty; }
        }

        public static string Enc(string value)
        {
            return HttpUtility.HtmlEncode(value ?? string.Empty);
        }

        /// <summary>Splits a Note field into non-empty trimmed lines.</summary>
        public static List<string> Lines(string raw)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(raw)) return result;

            string text = raw.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n");
            foreach (string line in text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = line.Trim();
                if (trimmed.Length > 0) result.Add(trimmed);
            }

            return result;
        }

        /// <summary>Renders a Note field as an encoded list. Returns empty when there is nothing to show.</summary>
        public static string BulletHtml(string raw, string cssClass)
        {
            List<string> lines = Lines(raw);
            if (lines.Count == 0) return string.Empty;

            var sb = new StringBuilder();
            sb.Append("<ul class=\"").Append(Enc(cssClass ?? string.Empty)).Append("\">");
            foreach (string line in lines) sb.Append("<li>").Append(Enc(line)).Append("</li>");
            sb.Append("</ul>");
            return sb.ToString();
        }

        /// <summary>Renders a Note field as a sequence of encoded paragraphs.</summary>
        public static string ParagraphHtml(string raw, string cssClass)
        {
            List<string> lines = Lines(raw);
            if (lines.Count == 0) return string.Empty;

            var sb = new StringBuilder();
            foreach (string line in lines)
                sb.Append("<p class=\"").Append(Enc(cssClass ?? string.Empty)).Append("\">").Append(Enc(line)).Append("</p>");
            return sb.ToString();
        }

        /// <summary>Parses "Text|Url" lines (one pair per line) into link items.</summary>
        public static List<PgLinkItem> ParseLinks(string raw)
        {
            var result = new List<PgLinkItem>();

            foreach (string line in Lines(raw))
            {
                string[] pair = line.Split('|');
                string text = pair.Length > 0 ? pair[0].Trim() : string.Empty;
                string url = pair.Length > 1 ? pair[1].Trim() : string.Empty;

                if (text.Length == 0) continue;
                result.Add(new PgLinkItem { Text = Enc(text), Url = Enc(url.Length == 0 ? "#" : url) });
            }

            return result;
        }

        /// <summary>
        /// Parses "Text|IconFileName" lines into highlight rows. The icon name is resolved
        /// against the shared DGA icon folder unless it is already a path.
        /// </summary>
        public static List<PgLinkItem> ParseHighlights(string raw)
        {
            var result = new List<PgLinkItem>();

            foreach (string line in Lines(raw))
            {
                string[] pair = line.Split('|');
                string text = pair.Length > 0 ? pair[0].Trim() : string.Empty;
                string icon = pair.Length > 1 ? pair[1].Trim() : string.Empty;

                if (text.Length == 0) continue;
                if (icon.Length > 0 && icon.IndexOf('/') < 0) icon = PgListSchema.IconRoot + icon;

                result.Add(new PgLinkItem { Text = Enc(text), Icon = Enc(icon), Url = string.Empty });
            }

            return result;
        }

        /// <summary>Parses "Title|Url;Title|Url;Title|" breadcrumb strings.</summary>
        public static List<PgBreadcrumbItem> ParseBreadcrumb(string raw)
        {
            var list = new List<PgBreadcrumbItem>();
            if (string.IsNullOrEmpty(raw)) return list;

            string[] parts = raw.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                string[] pair = parts[i].Split('|');
                string title = pair.Length > 0 ? pair[0].Trim() : string.Empty;
                string url = pair.Length > 1 ? pair[1].Trim() : string.Empty;

                if (title.Length == 0) continue;

                list.Add(new PgBreadcrumbItem
                {
                    Title = Enc(title),
                    Url = Enc(url),
                    IsLast = (i == parts.Length - 1)
                });
            }

            return list;
        }

        public static SPQuery OrderedQuery()
        {
            return new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /><FieldRef Name='ID' Ascending='TRUE' /></OrderBy>",
                ViewAttributes = "Scope='Recursive'"
            };
        }

        public static List<SPListItem> GetItems(SPWeb web, string listName)
        {
            try
            {
                if (web == null) return new List<SPListItem>();
                SPList list = web.Lists.TryGetList(listName);
                if (list == null) return new List<SPListItem>();
                return list.GetItems(OrderedQuery()).Cast<SPListItem>().ToList();
            }
            catch (Exception ex)
            {
                PgLog.Write("PgHelper.GetItems:" + listName, ex);
                return new List<SPListItem>();
            }
        }

        /// <summary>
        /// Returns the items of a list restricted to one phase. Items whose Phase column is
        /// empty are shared and returned for every phase.
        /// </summary>
        public static List<SPListItem> GetItems(SPWeb web, string listName, string phase)
        {
            List<SPListItem> items = GetItems(web, listName);
            if (string.IsNullOrEmpty(phase)) return items;

            return items.Where(i =>
            {
                string value = SafeString(i, "Phase").Trim();
                return value.Length == 0 || value.Equals(phase, StringComparison.OrdinalIgnoreCase);
            }).ToList();
        }
    }
}
