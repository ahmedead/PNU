using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Authorization gate for the content-administration controls.
    ///
    /// WHY THIS EXISTS
    /// The editor writes to a subweb chosen at runtime, using elevated privileges
    /// (the app-pool identity). Without a gate, any authenticated visitor who
    /// reached the page could write content into any web in the site collection.
    /// Every write path in the editor MUST call IsAuthorized() first, and every
    /// target web MUST pass IsWebInScope().
    ///
    /// Two ways to be authorized:
    ///   1. Site collection administrator (always allowed - prevents lock-out).
    ///   2. An active row in the PortalAdmins list on the /admin/ web whose Title
    ///      matches the current user's login name.
    /// </summary>
    public static class AdminSecurity
    {
        private const string CacheKey = "PNU_ContentAdmin_IsAuthorized";

        /// <summary>
        /// True when the CURRENT user may use the content-administration controls.
        /// Result is cached per request (the check opens another web).
        /// </summary>
        public static bool IsAuthorized()
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null && ctx.Items.Contains(CacheKey))
                    return (bool)ctx.Items[CacheKey];

                bool allowed = Evaluate();

                if (ctx != null) ctx.Items[CacheKey] = allowed;
                return allowed;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("", "AdminSecurity - IsAuthorized", ex.Message);
                return false;   // fail closed
            }
        }

        private static bool Evaluate()
        {
            return ContentAdm.IsAdmin();
        }

        /// <summary>
        /// Guards the target web: it must belong to the CURRENT site collection.
        /// This stops a crafted URL from pointing the editor at another site.
        /// </summary>
        public static bool IsWebInScope(string serverRelativeUrl)
        {
            if (string.IsNullOrEmpty(serverRelativeUrl)) return false;
            try
            {
                string root = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                string target = serverRelativeUrl.TrimEnd('/');
                if (target.Length == 0) target = "/";

                if (root.Length == 0 || root == "/")
                    return target.StartsWith("/");

                return target.Equals(root, StringComparison.OrdinalIgnoreCase)
                    || target.StartsWith(root + "/", StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        /// <summary>All webs in the current site collection, for the subweb picker.</summary>
        public static List<KeyValuePair<string, string>> GetSiteWebs()
        {
            var result = new List<KeyValuePair<string, string>>();
            if (!IsAuthorized()) return result;      // don't enumerate for non-admins

            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        foreach (SPWeb w in site.AllWebs)
                        {
                            try
                            {
                                result.Add(new KeyValuePair<string, string>(
                                    w.ServerRelativeUrl,
                                    string.IsNullOrEmpty(w.Title) ? w.ServerRelativeUrl : w.Title + "  (" + w.ServerRelativeUrl + ")"));
                            }
                            finally { w.Dispose(); }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("", "AdminSecurity - GetSiteWebs", ex.Message);
            }
            return result;
        }
    }
}
