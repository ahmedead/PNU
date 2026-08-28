using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public static class PcHelper
    {
        public static bool IsArabic
        {
            get
            {
                try
                {
                    if (SPContext.Current != null && SPContext.Current.Web != null)
                        return SPContext.Current.Web.Language == 1025;
                }
                catch { }
                return true;
            }
        }

        public static string Pick(string ar, string en)
        {
            if (IsArabic) return string.IsNullOrEmpty(ar) ? en ?? string.Empty : ar;
            return string.IsNullOrEmpty(en) ? ar ?? string.Empty : en;
        }

        public static string Enc(string input)
        {
            return HttpUtility.HtmlEncode(input ?? string.Empty);
        }

        public static string GetRes(string key, string defaultAr, string defaultEn)
        {
            return Pick(defaultAr, defaultEn);
        }

        public static string SafeString(SPListItem item, string fieldName)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return string.Empty;
            try
            {
                if (item.Fields.ContainsField(fieldName))
                {
                    SPField field = item.Fields.GetFieldByInternalName(fieldName);
                    object val = item[field.InternalName];
                    return val != null ? val.ToString() : string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }

        public static string SafeUrl(SPListItem item, string fieldName)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return string.Empty;
            try
            {
                if (item.Fields.ContainsField(fieldName))
                {
                    SPField field = item.Fields.GetFieldByInternalName(fieldName);
                    object val = item[field.InternalName];
                    if (val is SPFieldUrlValue)
                        return ((SPFieldUrlValue)val).Url;
                    if (val != null)
                        return val.ToString();
                }
            }
            catch { }
            return string.Empty;
        }

        public static int SafeInt(SPListItem item, string fieldName, int fallback = 0)
        {
            string s = SafeString(item, fieldName);
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : fallback;
        }

        public static List<SPListItem> GetItems(SPWeb web, string listName)
        {
            var list = new List<SPListItem>();
            if (web == null || string.IsNullOrEmpty(listName)) return list;

            try
            {
                SPList spList = web.Lists.TryGetList(listName);
                if (spList == null) return list;

                try
                {
                    SPQuery query = new SPQuery
                    {
                        Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/><FieldRef Name='ID' Ascending='TRUE'/></OrderBy>",
                        ViewAttributes = "Scope='Recursive'",
                        RowLimit = 2000
                    };

                    SPListItemCollection items = spList.GetItems(query);
                    foreach (SPListItem item in items)
                    {
                        list.Add(item);
                    }
                }
                catch
                {
                    foreach (SPListItem item in spList.Items)
                    {
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("PcHelper.GetItems:" + listName, ex);
            }

            return list;
        }
    }
}
