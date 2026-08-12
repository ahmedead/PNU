using Microsoft.SharePoint;
using System;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    /// <summary>
    /// Centralized administrator authorization service for Content Administration.
    /// Access is granted ONLY by the AdminUsers list on /ar/ContentAdmin.
    /// Site collection administrators, ManageLists holders and authenticated users get
    /// nothing from those roles alone - the list is the single source of truth.
    /// </summary>
    public static class ContentAdm
    {
        public const string AdminUsersList = "AdminUsers";
        public const string DefaultAdminPath = "/ar/ContentAdmin";

        /// <summary>
        /// True when the current user is listed as an active administrator in /ar/ContentAdmin's AdminUsers list.
        /// </summary>
        public static bool IsAdmin()
        {
            try
            {
                if (SPContext.Current == null || SPContext.Current.Web == null) return false;

                SPUser user = SPContext.Current.Web.CurrentUser;
                if (user == null) return false;                        // anonymous

                // Read the identity BEFORE elevating - inside the delegate the current
                // user is the app pool account. The id is site-collection wide, so it is
                // valid on the ContentAdmin web even if the user never visited it.
                int userId = user.ID;
                string userName = user.Name;

                Guid siteId = SPContext.Current.Site.ID;
                bool allowed = false;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb adminWeb = OpenAdminWeb(site))
                    {
                        allowed = IsListedAdmin(adminWeb, userId, userName);
                    }
                });

                return allowed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Opens the web holding the AdminUsers list (/ar/ContentAdmin).
        /// Returns null when unavailable. CALLER DISPOSES.
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
            catch { }

            return null;
        }

        public static string ResolveAdminUrl(SPSite site)
        {
            string configured = DefaultAdminPath.Replace('\\', '/').Trim().TrimEnd('/');
            if (!configured.StartsWith("/", StringComparison.Ordinal))
                configured = "/" + configured;

            if (site == null) return configured;

            string siteRoot = (site.ServerRelativeUrl ?? "/").TrimEnd('/');
            if (siteRoot.Length > 0 && !configured.StartsWith(siteRoot + "/", StringComparison.OrdinalIgnoreCase))
            {
                configured = siteRoot + configured;
            }

            return configured;
        }

        /// <summary>
        /// True when the user has an enabled row in the AdminUsers list on this web.
        /// UserAccount is a Person column, so it is matched two ways in one query:
        /// by lookup id (exact, survives a rename) and by display name (the form an
        /// editor sees). A missing web or list means "no match" - which denies access.
        /// An empty Active column counts as enabled.
        /// </summary>
        public static bool IsListedAdmin(SPWeb web, int userId, string userName)
        {
            if (web == null) return false;

            try
            {
                SPList list = web.Lists.TryGetList(AdminUsersList);
                if (list == null || list.ItemCount == 0) return false;

                string safeName = System.Security.SecurityElement.Escape(userName ?? string.Empty);

                var query = new SPQuery
                {
                    RowLimit = 10,
                    Query =
                        "<Where>" +
                          "<Or>" +
                            "<Eq>" +
                              "<FieldRef Name='UserAccount' LookupId='TRUE' />" +
                              "<Value Type='Integer'>" + userId.ToString(CultureInfo.InvariantCulture) + "</Value>" +
                            "</Eq>" +
                            "<Eq>" +
                              "<FieldRef Name='UserAccount' />" +
                              "<Value Type='User'>" + safeName + "</Value>" +
                            "</Eq>" +
                          "</Or>" +
                        "</Where>"
                };

                SPListItemCollection matches = list.GetItems(query);
                if (matches.Count == 0) return false;

                foreach (SPListItem item in matches)
                {
                    string active = SafeString(item, "Active").Trim();
                    if (active.Length == 0) return true;                // column missing or blank
                    if (active == "1" || active == "-1") return true;

                    bool flag;
                    if (bool.TryParse(active, out flag) && flag) return true;
                }
            }
            catch { }

            return false;
        }

        private static string SafeString(SPListItem item, string fieldInternalName)
        {
            if (item == null || string.IsNullOrEmpty(fieldInternalName)) return string.Empty;
            if (!item.Fields.ContainsField(fieldInternalName)) return string.Empty;
            object val = item[fieldInternalName];
            return val == null ? string.Empty : val.ToString();
        }
    }
}
