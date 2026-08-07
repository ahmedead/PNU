using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls
{
    /// <summary>
    /// Resolves the web that hosts the Academic Calendar list.
    ///
    /// The content list lives on ONE web - /ar/AcademicCalendar - regardless of which
    /// web the page is rendered from, so the Arabic and English pages share a single
    /// set of bilingual items and editors have one place to manage them.
    ///
    /// The caller ALWAYS disposes the returned SPWeb.
    /// </summary>
    public static class AcTargetWeb
    {
        /// <summary>Path of the web holding the content list.</summary>
        public const string DefaultPath = "/ar/AcademicCalendar";

        /// <summary>Path of the web holding the shared AdminUsers list.</summary>
        public const string DefaultAdminPath = "/ar/ContentAdmin";

        private static string _path = DefaultPath;
        private static string _adminPath = DefaultAdminPath;

        /// <summary>
        /// Farm-wide override of <see cref="DefaultPath"/>. Leave alone unless the list
        /// is moved; a leading "/" is treated as relative to the site collection root.
        /// </summary>
        public static string Path
        {
            get { return string.IsNullOrEmpty(_path) ? DefaultPath : _path; }
            set { _path = value; }
        }

        /// <summary>Farm-wide override of <see cref="DefaultAdminPath"/>.</summary>
        public static string AdminPath
        {
            get { return string.IsNullOrEmpty(_adminPath) ? DefaultAdminPath : _adminPath; }
            set { _adminPath = value; }
        }

        /// <summary>
        /// Builds the server-relative URL of the target web for this site collection.
        /// SPSite.OpenWeb resolves relative to the site collection root, so a site at a
        /// managed path (e.g. /sites/portal) gets its prefix added here.
        /// </summary>
        public static string ResolveUrl(SPSite site)
        {
            return ResolveUrl(site, Path);
        }

        /// <summary>Server-relative URL of the web holding the AdminUsers list.</summary>
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

        /// <summary>
        /// Opens the web holding the list. Falls back to the current context web when
        /// the configured web does not exist, so a page never dies over a missing subweb.
        /// Returns null when nothing can be opened. THE CALLER DISPOSES.
        /// </summary>
        public static SPWeb Open(SPSite site)
        {
            return OpenPath(site, Path, true);
        }

        /// <summary>
        /// Opens the web holding the AdminUsers list. There is deliberately NO fallback:
        /// permission comes from that one list, so a missing web must deny access rather
        /// than silently look somewhere else. THE CALLER DISPOSES.
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

                AcLog.Write("AcTargetWeb.Open",
                    "Web '" + url + "' was not found."
                    + (allowFallback ? " Falling back to the current web." : " Access will be denied."));
            }
            catch (Exception ex)
            {
                AcLog.Write("AcTargetWeb.Open:" + url, ex);
            }

            if (!allowFallback) return null;

            try
            {
                if (SPContext.Current != null)
                    return site.OpenWeb(SPContext.Current.Web.ID);
            }
            catch (Exception ex)
            {
                AcLog.Write("AcTargetWeb.OpenFallback", ex);
            }

            return null;
        }

        /// <summary>Id of the web holding the list, or Guid.Empty when unavailable.</summary>
        public static Guid ResolveWebId(SPSite site)
        {
            using (SPWeb web = Open(site))
            {
                return web == null ? Guid.Empty : web.ID;
            }
        }
    }
}
