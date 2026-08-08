using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public static class SscHelper
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

        public static string Pick(string ar, string en)
        {
            if (IsArabic) return string.IsNullOrEmpty(ar) ? (en ?? string.Empty) : ar;
            return string.IsNullOrEmpty(en) ? (ar ?? string.Empty) : en;
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
                string s = SafeString(item, field);
                int v;
                return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
            }
            catch { return 0; }
        }

        public static bool SafeBool(SPListItem item, string field)
        {
            try
            {
                string s = SafeString(item, field);
                if (string.IsNullOrEmpty(s)) return false;
                if (s == "1" || s == "-1") return true;
                bool v;
                return bool.TryParse(s, out v) && v;
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

        public static List<string> SplitLines(string value)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(value)) return result;

            string cleaned = value
                .Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n")
                .Replace("</div>", "\n").Replace("</p>", "\n").Replace("</li>", "\n");

            cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, "<[^>]+>", string.Empty);
            cleaned = HttpUtility.HtmlDecode(cleaned);

            foreach (string line in cleaned.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = line.Trim().Trim(' ');
                if (!string.IsNullOrEmpty(t)) result.Add(t);
            }
            return result;
        }

        public static string Enc(string value)
        {
            return HttpUtility.HtmlEncode(value ?? string.Empty);
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
                SscLog.Write("SscHelper.GetItems:" + listName, ex);
                return new List<SPListItem>();
            }
        }
    }
}
