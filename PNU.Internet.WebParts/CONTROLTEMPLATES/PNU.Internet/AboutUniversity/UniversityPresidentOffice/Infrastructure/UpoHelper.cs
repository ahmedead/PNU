using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    /// <summary>
    /// Shared helpers for the University President Office controls.
    /// Delegates to NsHelper for bilingual support where available.
    /// </summary>
    public static class UpoHelper
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

        public static string Enc(string value)
        {
            return HttpUtility.HtmlEncode(value ?? string.Empty);
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

        public static DateTime? SafeDate(SPListItem item, string field)
        {
            try
            {
                if (item == null || !item.Fields.ContainsField(field)) return null;
                object raw = item[field];
                if (raw == null) return null;
                if (raw is DateTime) return (DateTime)raw;

                DateTime parsed;
                return DateTime.TryParse(Convert.ToString(raw), CultureInfo.InvariantCulture,
                                         DateTimeStyles.None, out parsed)
                    ? parsed : (DateTime?)null;
            }
            catch { return null; }
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
                var result = new List<SPListItem>();
                foreach (SPListItem item in list.GetItems(OrderedQuery()))
                    result.Add(item);
                return result;
            }
            catch (Exception ex)
            {
                UpoLog.Write("UpoHelper.GetItems:" + listName, ex);
                return new List<SPListItem>();
            }
        }

        /// <summary>Reads a label from the PNUres global resource file with hardcoded fallbacks.</summary>
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
            catch { /* fall through to hardcoded fallback */ }

            return IsArabic ? fallbackAr : fallbackEn;
        }
    }
}
