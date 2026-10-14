using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    /// <summary>
    /// Output helpers for list content rendered into markup: HTML/attribute encoding and a
    /// link allow-list (site-relative, in-page anchors, http(s), and optionally tel:/mailto:).
    /// </summary>
    public static class SafeHtml
    {
        /// <summary>HTML-encodes text for element content.</summary>
        public static string Text(object value)
        {
            return HttpUtility.HtmlEncode(value != null ? value.ToString() : string.Empty);
        }

        /// <summary>Attribute-encodes a value (class, title, aria-label...).</summary>
        public static string Attr(object value)
        {
            return HttpUtility.HtmlAttributeEncode(value != null ? value.ToString() : string.Empty);
        }

        /// <summary>Attribute-encoded href; unsafe or empty links become "#".</summary>
        public static string Href(object value)
        {
            string url = value != null ? value.ToString().Trim() : string.Empty;
            return HttpUtility.HtmlAttributeEncode(IsSafeUrl(url) ? url : "#");
        }

        /// <summary>
        /// Allows "/path", "#anchor" and absolute http(s) URLs. Blocks javascript:, data:,
        /// protocol-relative "//host" and anything else. tel:/mailto: only when allowed.
        /// </summary>
        public static bool IsSafeUrl(string url, bool allowContactSchemes = false)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            string u = url.Trim();
            if (u.StartsWith("//", StringComparison.Ordinal)) return false;
            if (u.StartsWith("/", StringComparison.Ordinal) || u.StartsWith("#", StringComparison.Ordinal)) return true;
            if (u.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || u.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) return true;
            if (allowContactSchemes &&
                (u.StartsWith("tel:", StringComparison.OrdinalIgnoreCase) || u.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)))
                return true;
            return false;
        }

        /// <summary>Restricts a link target to _blank / _self.</summary>
        public static string Target(object value)
        {
            string t = value != null ? value.ToString().Trim() : string.Empty;
            return string.Equals(t, "_blank", StringComparison.OrdinalIgnoreCase) ? "_blank" : "_self";
        }
    }
}
