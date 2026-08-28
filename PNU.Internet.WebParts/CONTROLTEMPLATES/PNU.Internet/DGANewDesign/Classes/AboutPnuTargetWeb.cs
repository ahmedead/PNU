using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes
{
    /// <summary>
    /// Resolves the web that hosts the About PNU lists.
    /// Lists live on /ar/AboutUniversity regardless of which web the page is rendered from.
    /// THE CALLER MUST ALWAYS DISPOSE the returned SPWeb.
    /// </summary>
    public static class AboutPnuTargetWeb
    {
        public const string DefaultPath = "/ar/AboutUniversity";
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
                !configured.StartsWith(siteRoot + "/", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(configured, siteRoot, StringComparison.OrdinalIgnoreCase))
            {
                configured = siteRoot + configured;
            }

            return configured;
        }

        /// <summary>
        /// Opens the web holding the lists (/ar/AboutUniversity).
        /// Falls back to the current context web if the target web does not exist.
        /// THE CALLER DISPOSES.
        /// </summary>
        public static SPWeb Open(SPSite site)
        {
            return OpenPath(site, Path, true);
        }

        /// <summary>
        /// Opens the web holding the AdminUsers list (/ar/ContentAdmin).
        /// THE CALLER DISPOSES.
        /// </summary>
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

                AboutPnuLog.Write("AboutPnuTargetWeb.Open",
                    "Web '" + url + "' was not found."
                    + (allowFallback ? " Falling back to the current web." : " Access will be denied."));
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("AboutPnuTargetWeb.Open:" + url, ex);
            }

            if (!allowFallback) return null;

            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                    return site.OpenWeb(SPContext.Current.Web.ID);
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("AboutPnuTargetWeb.OpenFallback", ex);
            }

            return null;
        }
    }
}
