using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// Shared helpers for the Agency controls.
    /// Manual Safe* readers - no [SharePointField] attribute mapping.
    /// </summary>
    public static class AgHelper
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

        /// <summary>
        /// Reads a label from the PNUres global resource file, with hardcoded bilingual
        /// fallbacks so the control never fails while resource keys are being rolled out.
        /// </summary>
        public static string GetRes(string key, string fallbackAr, string fallbackEn)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && HttpContext.Current != null)
                {
                    // GetGlobalResourceObject is static on HttpContext - do not call it
                    // through HttpContext.Current (CS0176).
                    object val = HttpContext.GetGlobalResourceObject("PNUres", key);
                    string s = val as string;
                    if (!string.IsNullOrEmpty(s)) return s;
                }
            }
            catch { /* fall through to hardcoded fallback */ }

            return IsArabic ? fallbackAr : fallbackEn;
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
                int v;
                return int.TryParse(SafeString(item, field), NumberStyles.Any,
                                    CultureInfo.InvariantCulture, out v) ? v : 0;
            }
            catch { return 0; }
        }

        /// <summary>Reads a URL field and returns only the Url part.</summary>
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

        public static string Icon(string iconClass, string fallback)
        {
            return string.IsNullOrEmpty(iconClass) ? fallback : iconClass;
        }

        /// <summary>Splits a multi-line value into trimmed, non-empty lines.</summary>
        public static List<string> Lines(string value)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(value)) return result;

            foreach (string raw in value.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n'))
            {
                string line = raw.Trim();
                if (line.Length > 0) result.Add(line);
            }
            return result;
        }

        /// <summary>Swaps a file extension, used to derive the .avif variant of a hero image.</summary>
        public static string SwapExtension(string url, string newExtension)
        {
            if (string.IsNullOrEmpty(url)) return url;
            int dot = url.LastIndexOf('.');
            int slash = url.LastIndexOf('/');
            return dot > slash && dot >= 0 ? url.Substring(0, dot) + newExtension : url + newExtension;
        }

        /// <summary>Standard ordered query used by every Agency list.</summary>
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
                AgLog.Write("AgHelper.GetItems:" + listName, ex);
                return new List<SPListItem>();
            }
        }
    }
}
