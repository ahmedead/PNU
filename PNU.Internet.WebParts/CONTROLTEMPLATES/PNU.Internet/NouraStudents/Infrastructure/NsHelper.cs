using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>
    /// Shared helpers for the Noura Students controls.
    /// Manual Safe* readers - no [SharePointField] attribute mapping.
    /// </summary>
    public static class NsHelper
    {
        private static readonly string[] ArabicMonths = new string[]
        {
            "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
            "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
        };

        private static readonly string[] EnglishMonths = new string[]
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

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

        /// <summary>Day number as it appears in the date badge, e.g. "07".</summary>
        public static string DayNumber(DateTime value)
        {
            return value.Day.ToString("00", CultureInfo.InvariantCulture);
        }

        /// <summary>Gregorian month name - never Hijri, regardless of thread culture.</summary>
        public static string MonthName(DateTime value)
        {
            int index = value.Month - 1;
            if (index < 0 || index > 11) return string.Empty;
            return IsArabic ? ArabicMonths[index] : EnglishMonths[index];
        }

        /// <summary>ISO date for the &lt;time datetime="..."&gt; attribute.</summary>
        public static string IsoDate(DateTime value)
        {
            return value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string Enc(string value)
        {
            return HttpUtility.HtmlEncode(value ?? string.Empty);
        }

        public static string Icon(string iconClass, string fallback)
        {
            return string.IsNullOrEmpty(iconClass) ? fallback : iconClass;
        }

        /// <summary>Standard ordered query used by every Noura Students list.</summary>
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
                NsLog.Write("NsHelper.GetItems:" + listName, ex);
                return new List<SPListItem>();
            }
        }
    }
}
