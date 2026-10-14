using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.Infrastructure
{
    /// <summary>
    /// Resolves and opens the SharePoint webs hosting:
    /// 1. The MediaFiles content list (/ar/MediaCenter)
    /// 2. The MediaFileAdminUsers security list (/ar/ContentAdmin)
    /// </summary>
    public static class MediaFilesTargetWeb
    {
        public const string DefaultContentPath = "/ar/MediaCenter";
        public const string DefaultAdminPath = "/ar/ContentAdmin";

        private static string _contentPath = DefaultContentPath;
        private static string _adminPath = DefaultAdminPath;

        public static string ContentPath
        {
            get { return string.IsNullOrEmpty(_contentPath) ? DefaultContentPath : _contentPath; }
            set { _contentPath = value; }
        }

        public static string AdminPath
        {
            get { return string.IsNullOrEmpty(_adminPath) ? DefaultAdminPath : _adminPath; }
            set { _adminPath = value; }
        }

        public static string ResolveUrl(SPSite site, string path)
        {
            string configured = (path ?? string.Empty).Replace('\\', '/').Trim();
            if (configured.Length == 0) configured = DefaultContentPath;

            configured = configured.TrimEnd('/');
            if (!configured.StartsWith("/", StringComparison.Ordinal))
                configured = "/" + configured;

            if (site == null) return configured;

            string siteRoot = (site.ServerRelativeUrl ?? "/").TrimEnd('/');
            if (siteRoot.Length > 0 &&
                !configured.StartsWith(siteRoot + "/", StringComparison.OrdinalIgnoreCase))
            {
                configured = siteRoot + configured;
            }

            return configured;
        }

        public static string ResolveContentUrl(SPSite site)
        {
            return ResolveUrl(site, ContentPath);
        }

        public static string ResolveAdminUrl(SPSite site)
        {
            return ResolveUrl(site, AdminPath);
        }

        /// <summary>
        /// Opens the web holding the MediaFiles list (/ar/MediaCenter) - the same web
        /// ucMediaFiles.ascx reads (res_MediaFileWeb). No fallback: writing to the current
        /// web (/ar/ContentAdmin) would create a list the public page never reads.
        /// Returns null when unavailable. CALLER DISPOSES.
        /// </summary>
        public static SPWeb OpenContentWeb(SPSite site)
        {
            if (site == null) return null;
            string url = ResolveContentUrl(site);

            try
            {
                SPWeb web = site.OpenWeb(url, true);
                if (web != null && web.Exists) return web;
                if (web != null) web.Dispose();
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("MediaFilesTargetWeb.OpenContentWeb:" + url, ex);
            }

            return null;
        }

        /// <summary>
        /// Opens the web holding the MediaFileAdminUsers list (/ar/ContentAdmin).
        /// Strictly no fallback: permissions must come ONLY from /ar/ContentAdmin.
        /// CALLER DISPOSES.
        /// </summary>
        public static SPWeb OpenAdminWeb(SPSite site)
        {
            if (site == null) return null;
            string url = ResolveAdminUrl(site);

            try
            {
                SPWeb web = site.OpenWeb(url, true);
                if (web != null && web.Exists) return web;
                if (web != null) web.Dispose();
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("MediaFilesTargetWeb.OpenAdminWeb:" + url, ex);
            }

            return null;
        }
    }
}
