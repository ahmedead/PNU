using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Provisions the PortalAdmins list on the /admin/ web.
    /// This list controls WHO may open the content-administration pages and write
    /// content into faculty subwebs.
    ///
    ///   Title       = user login name (e.g. PNU\ahmed.sharaf) - the match key
    ///   DisplayName = friendly name, for the administrator's own reference
    ///   IsActive    = Yes/No; set to No to revoke access without deleting the row
    ///   Notes       = free text (department, reason, review date)
    ///
    /// The list itself is NOT anonymously readable and does NOT break inheritance -
    /// it stays locked to whatever permissions the /admin/ web already has, so only
    /// existing site administrators can edit who is an admin.
    /// </summary>
    public static class AdminUsersProvisioner
    {
        public const string ListName = "PortalAdmins";
        public const string AdminWebName = "admin";
        private static readonly object _lock = new object();

        public static void EnsureList()
        {
            try
            {
                lock (_lock)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            SPWeb adminWeb = OpenAdminWeb(site);
                            if (adminWeb == null) return;

                            try
                            {
                                adminWeb.AllowUnsafeUpdates = true;

                                SPList list = adminWeb.Lists.TryGetList(ListName);
                                if (list == null)
                                {
                                    Guid id = adminWeb.Lists.Add(ListName,
                                        "Users allowed to open the portal content administration pages",
                                        SPListTemplateType.GenericList);
                                    list = adminWeb.Lists[id];
                                    list.OnQuickLaunch = false;
                                    list.Update();
                                }

                                EnsureField(list, "DisplayName", SPFieldType.Text);
                                EnsureBool(list, "IsActive");
                                EnsureField(list, "Notes", SPFieldType.Note);
                                AddViewFields(list, "DisplayName", "IsActive", "Notes");

                                // Deliberately NO anonymous access and NO broken inheritance here.

                                adminWeb.AllowUnsafeUpdates = false;
                            }
                            finally
                            {
                                adminWeb.Dispose();
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "",
                    "AdminUsersProvisioner - EnsureList", ex.Message);
            }
        }

        /// <summary>
        /// Opens the /admin/ web. SPWeb.OpenWeb() resolves relative to the site
        /// collection root, so the path is built explicitly and checked with Exists.
        /// Caller is responsible for disposing the returned web.
        /// </summary>
        public static SPWeb OpenAdminWeb(SPSite site)
        {
            try
            {
                string rootUrl = site.RootWeb.ServerRelativeUrl.TrimEnd('/');
                string path = rootUrl + "/" + AdminWebName;
                SPWeb web = site.OpenWeb(path);
                if (web == null) return null;
                if (!web.Exists) { web.Dispose(); return null; }
                return web;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("", "AdminUsersProvisioner - OpenAdminWeb", ex.Message);
                return null;
            }
        }

        private static void EnsureField(SPList list, string name, SPFieldType type)
        {
            if (!list.Fields.ContainsField(name))
            {
                string internalName = list.Fields.Add(name, type, false);
                if (type == SPFieldType.Note)
                {
                    var f = list.Fields.GetFieldByInternalName(internalName) as SPFieldMultiLineText;
                    if (f != null) { f.RichText = false; f.NumberOfLines = 3; f.Update(); }
                }
            }
        }

        private static void EnsureBool(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                string internalName = list.Fields.Add(name, SPFieldType.Boolean, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                f.DefaultValue = "1";
                f.Update();
            }
        }

        private static void AddViewFields(SPList list, params string[] fields)
        {
            SPView view = list.DefaultView;
            bool changed = false;
            foreach (string f in fields)
                if (!view.ViewFields.Exists(f)) { view.ViewFields.Add(f); changed = true; }
            if (changed) view.Update();
        }
    }
}
