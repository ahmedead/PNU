using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Resolves the web that hosts the Smart Suitcase lists (/ar/smart-suitcase).
    /// </summary>
    public static class SscTargetWeb
    {
        public const string DefaultPath = "/ar/smart-suitcase";
        public const string DefaultAdminPath = "/ar/ContentAdmin";

        private static string _path = DefaultPath;
        private static string _adminPath = DefaultAdminPath;

        public static string Path
        {
            get { return string.IsNullOrEmpty(_path) ? DefaultPath : _path; }
            set { _path = value; }
        }

        public static string AdminPath
        {
            get { return string.IsNullOrEmpty(_adminPath) ? DefaultAdminPath : _adminPath; }
            set { _adminPath = value; }
        }

        public static string ResolveUrl(SPSite site)
        {
            return ResolveUrl(site, Path);
        }

        public static string ResolveAdminUrl(SPSite site)
        {
            return ResolveUrl(site, AdminPath);
        }

        public static string ResolveUrl(SPSite site, string path)
        {
            string configured = (path ?? string.Empty).Replace('\\', '/').Trim();
            if (configured.Length == 0) configured = DefaultPath;

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

        public static SPWeb Open(SPSite site)
        {
            return OpenPath(site, Path, true);
        }

        public static SPWeb OpenAdminWeb(SPSite site)
        {
            return OpenPath(site, AdminPath, false);
        }

        private static SPWeb OpenPath(SPSite site, string path, bool allowFallback)
        {
            if (site == null) return null;

            string url = ResolveUrl(site, path);

            try
            {
                SPWeb web = site.OpenWeb(url, true);
                if (web != null && web.Exists) return web;
                if (web != null) web.Dispose();

                SscLog.Write("SscTargetWeb.Open",
                    "Web '" + url + "' was not found."
                    + (allowFallback ? " Falling back to the current web." : " Access will be denied."));
            }
            catch (Exception ex)
            {
                SscLog.Write("SscTargetWeb.Open:" + url, ex);
            }

            if (!allowFallback) return null;

            try
            {
                if (SPContext.Current != null)
                    return site.OpenWeb(SPContext.Current.Web.ID);
            }
            catch (Exception ex)
            {
                SscLog.Write("SscTargetWeb.OpenFallback", ex);
            }

            return null;
        }

        public static Guid ResolveWebId(SPSite site)
        {
            using (SPWeb web = Open(site))
            {
                return web == null ? Guid.Empty : web.ID;
            }
        }
    }
}
