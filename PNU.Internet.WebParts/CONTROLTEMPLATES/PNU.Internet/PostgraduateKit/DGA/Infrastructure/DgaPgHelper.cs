using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    /// <summary>
    /// Isolated helper utilities for DGA Postgraduate Kit data extraction and formatting.
    /// </summary>
    public static class DgaPgHelper
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

                try
                {
                    return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("ar", StringComparison.OrdinalIgnoreCase);
                }
                catch { return true; }
            }
        }

        public static string CurrentLangCode => IsArabic ? "ar" : "en";

        public static string Pick(string ar, string en)
        {
            if (IsArabic)
                return !string.IsNullOrEmpty(ar) ? ar : (en ?? string.Empty);
            return !string.IsNullOrEmpty(en) ? en : (ar ?? string.Empty);
        }

        public static string PickField(SPListItem item, string arFieldName, string enFieldName = null)
        {
            if (item == null) return string.Empty;
            if (string.IsNullOrEmpty(enFieldName)) enFieldName = arFieldName + "_EN";

            string arVal = SafeString(item, arFieldName);
            string enVal = SafeString(item, enFieldName);

            return Pick(arVal, enVal);
        }

        public static string SafeString(SPListItem item, string fieldName, string defaultValue = "")
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return defaultValue ?? string.Empty;
            try
            {
                if (!item.Fields.ContainsField(fieldName)) return defaultValue ?? string.Empty;
                object v = item[fieldName];
                if (v == null) return defaultValue ?? string.Empty;
                string s = v.ToString().Trim();
                return !string.IsNullOrEmpty(s) ? s : (defaultValue ?? string.Empty);
            }
            catch { return defaultValue ?? string.Empty; }
        }

        public static int SafeInt(SPListItem item, string fieldName, int defaultValue = 0)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return defaultValue;
            try
            {
                if (!item.Fields.ContainsField(fieldName)) return defaultValue;
                object v = item[fieldName];
                if (v == null) return defaultValue;
                if (int.TryParse(v.ToString(), out int parsed)) return parsed;
                if (double.TryParse(v.ToString(), out double d)) return (int)d;
                return defaultValue;
            }
            catch { return defaultValue; }
        }

        public static bool SafeBool(SPListItem item, string fieldName, bool defaultValue = false)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return defaultValue;
            try
            {
                if (!item.Fields.ContainsField(fieldName)) return defaultValue;
                object v = item[fieldName];
                if (v == null) return defaultValue;
                if (bool.TryParse(v.ToString(), out bool parsed)) return parsed;
                string s = v.ToString().Trim();
                if (s == "1" || s.Equals("yes", StringComparison.OrdinalIgnoreCase) || s.Equals("نعم", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (s == "0" || s.Equals("no", StringComparison.OrdinalIgnoreCase) || s.Equals("لا", StringComparison.OrdinalIgnoreCase))
                    return false;
                return defaultValue;
            }
            catch { return defaultValue; }
        }

        public static string SafeUrl(SPListItem item, string fieldName, string defaultValue = "")
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return defaultValue ?? string.Empty;
            try
            {
                if (!item.Fields.ContainsField(fieldName)) return defaultValue ?? string.Empty;
                object v = item[fieldName];
                if (v == null) return defaultValue ?? string.Empty;
                if (v is SPFieldUrlValue urlVal) return !string.IsNullOrEmpty(urlVal.Url) ? urlVal.Url : (defaultValue ?? string.Empty);
                string s = v.ToString().Trim();
                if (s.Contains(","))
                {
                    var parts = s.Split(new[] { ',' }, 2);
                    string u = parts[0].Trim();
                    return !string.IsNullOrEmpty(u) ? u : (defaultValue ?? string.Empty);
                }
                return !string.IsNullOrEmpty(s) ? s : (defaultValue ?? string.Empty);
            }
            catch { return defaultValue ?? string.Empty; }
        }

        public static List<string> SplitLines(string text)
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(text)) return list;

            using (var reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (!string.IsNullOrEmpty(line))
                    {
                        list.Add(line);
                    }
                }
            }
            return list;
        }

        public static List<DgaPgLinkItem> ParseLinks(string text)
        {
            var list = new List<DgaPgLinkItem>();
            if (string.IsNullOrEmpty(text)) return list;

            var lines = SplitLines(text);
            foreach (var line in lines)
            {
                if (line.Contains("|"))
                {
                    var parts = line.Split(new[] { '|' }, 2);
                    list.Add(new DgaPgLinkItem { Title = parts[0].Trim(), Url = parts[1].Trim() });
                }
                else
                {
                    list.Add(new DgaPgLinkItem { Title = line, Url = "#" });
                }
            }
            return list;
        }

        public static List<DgaPgLinkItem> ParseForms(string text)
        {
            return ParseLinks(text);
        }

        public static List<DgaPgArticleBlock> ParseArticles(string text)
        {
            var list = new List<DgaPgArticleBlock>();
            if (string.IsNullOrEmpty(text)) return list;

            var lines = SplitLines(text);
            foreach (var line in lines)
            {
                if (line.Contains("|"))
                {
                    var parts = line.Split(new[] { '|' }, 2);
                    list.Add(new DgaPgArticleBlock { Heading = parts[0].Trim(), Body = parts[1].Trim() });
                }
                else
                {
                    list.Add(new DgaPgArticleBlock { Heading = Pick("نص المادة", "Article Text"), Body = line });
                }
            }
            return list;
        }

        public static string ResolveUrl(string rawUrl)
        {
            if (string.IsNullOrEmpty(rawUrl)) return "#";
            if (rawUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                rawUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                rawUrl.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
                rawUrl.StartsWith("tel:", StringComparison.OrdinalIgnoreCase) ||
                rawUrl.StartsWith("#"))
            {
                return rawUrl;
            }

            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    string root = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                    if (rawUrl.StartsWith("/"))
                        return rawUrl;
                    return root + "/" + rawUrl.TrimStart('/');
                }
            }
            catch { }

            return rawUrl;
        }
    }
}
