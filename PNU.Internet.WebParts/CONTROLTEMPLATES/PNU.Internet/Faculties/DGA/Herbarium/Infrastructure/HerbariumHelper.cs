using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    /// <summary>
    /// Shared helpers for the Herbarium controls.
    /// Manual Safe* readers - no [SharePointField] attribute mapping.
    /// </summary>
    public static class HerbariumHelper
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
        /// Reads a label from PNUres global resource file, with hardcoded fallbacks.
        /// </summary>
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
            catch { }

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

        public static bool SafeBool(SPListItem item, string field)
        {
            try
            {
                string raw = SafeString(item, field).Trim();
                if (raw.Length == 0) return false;
                if (raw == "1" || raw == "-1") return true;

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

        public static string Icon(string iconClass, string fallback)
        {
            return string.IsNullOrEmpty(iconClass) ? fallback : iconClass;
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
                HerbariumLog.Write("HerbariumHelper.GetItems:" + listName, ex);
                return new List<SPListItem>();
            }
        }
    }
}
