using System;
using System.Web;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    /// <summary>
    /// Reads AboutSharedTitles into a key->localized-value dictionary, honoring the current language.
    /// Reads are elevated so anonymous users get titles too.
    /// </summary>
    public static class SharedTitleReader
    {
        public static Dictionary<string, string> Load(bool isArabic)
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId  = SPContext.Current.Web.ID;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var web  = site.OpenWeb(webId))
                    {
                        var list = web.Lists.TryGetList(SharedTitles.ListName);
                        if (list == null) return;

                        foreach (SPListItem item in list.Items)
                        {
                            string key = SafeString(item, "TitleKey");
                            if (string.IsNullOrEmpty(key)) continue;
                            string val = isArabic ? SafeString(item, "TitleAr") : SafeString(item, "TitleEn");
                            if (string.IsNullOrEmpty(val)) val = SafeString(item, "TitleAr");
                            map[key] = val;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "SharedTitleReader.Load", ex.Message);
            }
            return map;
        }

        public static string Get(Dictionary<string, string> map, string key, string fallback = "")
        {
            if (map != null && map.TryGetValue(key, out string v) && !string.IsNullOrEmpty(v)) return v;
            return fallback;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try { return item[field]?.ToString() ?? string.Empty; }
            catch { return string.Empty; }
        }
    }
}
