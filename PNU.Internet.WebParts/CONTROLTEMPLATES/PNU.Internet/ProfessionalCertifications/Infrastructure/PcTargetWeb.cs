using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    /// <summary>
    /// Resolves the web that holds the Professional Certifications SharePoint lists (/ar/Agencies/AcademicAffairs).
    /// </summary>
    public static class PcTargetWeb
    {
        public const string DefaultPath = "/ar/Agencies/AcademicAffairs";
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
            EnsureTargetWebExists(site);
            return OpenPath(site, Path, true);
        }

        public static void EnsureTargetWebExists(SPSite site)
        {
            if (site == null) return;
            string url = ResolveUrl(site, Path);

            try
            {
                using (SPWeb testWeb = site.OpenWeb(url, true))
                {
                    if (testWeb != null && testWeb.Exists) return;
                }

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite elevatedSite = new SPSite(site.ID))
                    {
                        string targetPath = Path.Trim('/');
                        try
                        {
                            elevatedSite.RootWeb.AllowUnsafeUpdates = true;
                            using (SPWeb newWeb = elevatedSite.RootWeb.Webs.Add(
                                targetPath,
                                "وكالة الشؤون الأكاديمية",
                                "Academic Affairs Agency Web for Professional Certifications",
                                1025,
                                "STS#0",
                                false,
                                false))
                            {
                                newWeb.AllowUnsafeUpdates = true;
                                newWeb.Update();
                            }
                            elevatedSite.RootWeb.AllowUnsafeUpdates = false;
                        }
                        catch (Exception ex)
                        {
                            PcLog.Write("PcTargetWeb.EnsureTargetWebExists", ex);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                PcLog.Write("PcTargetWeb.EnsureTargetWebExists.Outer", ex);
            }
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

                PcLog.Write("PcTargetWeb.Open",
                    "Web '" + url + "' was not found."
                    + (allowFallback ? " Falling back to the current web." : " Access will be denied."));
            }
            catch (Exception ex)
            {
                PcLog.Write("PcTargetWeb.Open:" + url, ex);
            }

            if (!allowFallback) return null;

            try
            {
                if (SPContext.Current != null)
                    return site.OpenWeb(SPContext.Current.Web.ID);
            }
            catch (Exception ex)
            {
                PcLog.Write("PcTargetWeb.OpenFallback", ex);
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
