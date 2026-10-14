using Microsoft.SharePoint;
using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.Infrastructure
{
    public static class MediaFilesHelper
    {
        public static string Enc(string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : HttpUtility.HtmlEncode(text);
        }

        public static string SafeString(SPListItem item, string fieldName)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return string.Empty;
            if (!item.Fields.ContainsField(fieldName)) return string.Empty;
            object val = item[fieldName];
            return val == null ? string.Empty : val.ToString();
        }

        public static int SafeInt(SPListItem item, string fieldName, int defaultVal = 0)
        {
            string s = SafeString(item, fieldName);
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : defaultVal;
        }

        public static bool SafeBool(SPListItem item, string fieldName, bool defaultVal = true)
        {
            string s = SafeString(item, fieldName).Trim();
            if (string.IsNullOrEmpty(s)) return defaultVal;
            if (s == "1" || s == "-1" || string.Equals(s, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(s, "yes", StringComparison.OrdinalIgnoreCase))
                return true;
            if (s == "0" || string.Equals(s, "false", StringComparison.OrdinalIgnoreCase) || string.Equals(s, "no", StringComparison.OrdinalIgnoreCase))
                return false;
            return defaultVal;
        }

        /// <summary>
        /// Gets the server-relative URL and filename of the primary attachment for a list item.
        /// </summary>
        public static string GetAttachmentUrl(SPListItem item)
        {
            if (item == null || item.Attachments == null || item.Attachments.Count == 0)
                return string.Empty;

            try
            {
                SPAttachmentCollection attachments = item.Attachments;
                return attachments.UrlPrefix + attachments[0];
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetAttachmentFileName(SPListItem item)
        {
            if (item == null || item.Attachments == null || item.Attachments.Count == 0)
                return string.Empty;

            try
            {
                return item.Attachments[0];
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
