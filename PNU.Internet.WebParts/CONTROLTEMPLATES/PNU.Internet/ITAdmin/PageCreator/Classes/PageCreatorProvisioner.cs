using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    /// <summary>
    /// Idempotent provisioner for the Page Creator lists.
    /// Called from ucPageCreator.OnInit (authenticated users only).
    /// Pattern: EnsureList / EnsureField guards, out bool created flag – never re-seeds existing lists.
    /// </summary>
    public static class PageCreatorProvisioner
    {
        public static void EnsureAll()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    Guid webId = SPContext.Current.Web.ID;

                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            bool created;

                            // ---------------- PageTemplates ----------------
                            SPList templates = EnsureList(web, PageCreatorLists.PageTemplates,
                                "Page templates: layout + user control + default properties", out created);
                            EnsureNoteField(templates, "PageLayoutURL", "Page Layout URL");
                            EnsureNoteField(templates, "UserControlPath", "User Control Path");
                            EnsureNoteField(templates, "DefaultProperties", "Default Control Properties");
                            EnsureBoolField(templates, "IsActive", "Is Active", true);

                            // ---------------- PageLayoutsCatalog ----------------
                            SPList layouts = EnsureList(web, PageCreatorLists.PageLayouts,
                                "Catalog of available publishing page layouts", out created);
                            EnsureNoteField(layouts, "PageLayoutURL", "Page Layout URL");
                            EnsureBoolField(layouts, "IsActive", "Is Active", true);

                            // ---------------- PageCreatorAdmins ----------------
                            SPList admins = EnsureList(web, PageCreatorLists.PageCreatorAdmins,
                                "Users allowed to use the Page Creator tool", out created);
                            EnsureTextField(admins, "UserLogin", "User Login");

                            // Seed the deploying user as first admin so nobody is locked out.
                            if (created)
                            {
                                string login = SPContext.Current.Web.CurrentUser != null
                                    ? SPContext.Current.Web.CurrentUser.LoginName
                                    : string.Empty;
                                if (!string.IsNullOrEmpty(login))
                                {
                                    SPListItem item = admins.AddItem();
                                    item["Title"] = SPContext.Current.Web.CurrentUser.Name;
                                    item["UserLogin"] = login;
                                    item.Update();
                                }
                            }
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "PageCreatorProvisioner.EnsureAll", ex.Message);
            }
        }

        private static SPList EnsureList(SPWeb web, string listName, string description, out bool created)
        {
            created = false;
            SPList list = web.Lists.TryGetList(listName);
            if (list == null)
            {
                Guid id = web.Lists.Add(listName, description, SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
                created = true;
            }
            return list;
        }

        private static void EnsureTextField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return;
            string name = list.Fields.Add(internalName, SPFieldType.Text, false);
            SPField field = list.Fields.GetFieldByInternalName(name);
            field.Title = displayName;
            field.Update();
            list.Update();
        }

        /// <summary>
        /// Note field in Plain-text mode – project convention for URLs longer than 255 chars.
        /// </summary>
        private static void EnsureNoteField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return;
            string name = list.Fields.Add(internalName, SPFieldType.Note, false);
            SPFieldMultiLineText field = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(name);
            field.Title = displayName;
            field.RichText = false;
            field.NumberOfLines = 3;
            field.Update();
            list.Update();
        }

        private static void EnsureBoolField(SPList list, string internalName, string displayName, bool defaultValue)
        {
            if (list.Fields.ContainsField(internalName)) return;
            string name = list.Fields.Add(internalName, SPFieldType.Boolean, false);
            SPField field = list.Fields.GetFieldByInternalName(name);
            field.Title = displayName;
            field.DefaultValue = defaultValue ? "1" : "0";
            field.Update();
            list.Update();
        }
    }
}
