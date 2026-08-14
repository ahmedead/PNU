using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    /// <summary>
    /// Resolves the web that hosts the Herbarium lists.
    /// Lists live on /ar/Faculties/SC regardless of which web the page is rendered from.
    /// The caller ALWAYS disposes the returned SPWeb.
    /// </summary>
    public static class HerbariumTargetWeb
    {
        /// <summary>Path of the web holding the content lists.</summary>
        public const string DefaultPath = "/ar/Faculties/SC";

        /// <summary>Path of the web holding the shared AdminUsers list.</summary>
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
                !configured.StartsWith(siteRoot + "/", StringComparison.OrdinalIgnoreCase))
            {
                configured = siteRoot + configured;
            }

            return configured;
        }

        /// <summary>
        /// Opens the web holding the lists (/ar/Faculties/SC).
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

                HerbariumLog.Write("HerbariumTargetWeb.Open",
                    "Web '" + url + "' was not found."
                    + (allowFallback ? " Falling back to the current web." : " Access will be denied."));
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("HerbariumTargetWeb.Open:" + url, ex);
            }

            if (!allowFallback) return null;

            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                    return site.OpenWeb(SPContext.Current.Web.ID);
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("HerbariumTargetWeb.OpenFallback", ex);
            }

            return null;
        }
    }
}
