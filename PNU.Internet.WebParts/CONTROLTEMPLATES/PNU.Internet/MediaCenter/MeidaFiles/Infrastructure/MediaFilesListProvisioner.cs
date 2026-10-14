using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.Infrastructure
{
    /// <summary>
    /// Idempotent SharePoint provisioner and authorization manager for Media Files.
    /// Manages:
    /// 1. 'MediaFileAdminUsers' on /ar/ContentAdmin (Dedicated admin authorization list)
    /// 2. 'MediaFiles' on /ar/MediaCenter (The media files content list)
    /// </summary>
    public static class MediaFilesListProvisioner
    {
        public const string AdminListName = "MediaFileAdminUsers";
        public const string ContentListName = "MediaFiles";

        private static readonly object _lock = new object();
        private static volatile bool _alreadyProvisionedInProcess = false;

        public static void EnsureAllListsExist()
        {
            if (DoneThisRequest("MediaFilesAllLists")) return;
            if (_alreadyProvisionedInProcess) return;

            try
            {
                if (SPContext.Current == null) return;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        EnsureAdminUsersListOnWeb(site);
                        EnsureContentListOnWeb(site);
                    }
                });

                _alreadyProvisionedInProcess = true;
                MarkDoneThisRequest("MediaFilesAllLists");
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("MediaFilesListProvisioner.EnsureAllListsExist", ex);
            }
        }

        public static void ForceProvision()
        {
            try
            {
                if (SPContext.Current == null) return;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        EnsureAdminUsersListOnWeb(site);
                        EnsureContentListOnWeb(site);
                    }
                });
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("MediaFilesListProvisioner.ForceProvision", ex);
            }
        }

        #region Admin Users Provisioning (/ar/ContentAdmin)

        /// <summary>
        /// Provisions the dedicated MediaFileAdminUsers list on /ar/ContentAdmin.
        /// </summary>
        public static void EnsureAdminUsersListOnWeb(SPSite site)
        {
            if (site == null) return;

            using (SPWeb adminWeb = MediaFilesTargetWeb.OpenAdminWeb(site))
            {
                if (adminWeb == null)
                {
                    MediaFilesLog.Write("EnsureAdminUsersListOnWeb", "Admin web /ar/ContentAdmin could not be opened.");
                    return;
                }

                bool prevUnsafe = adminWeb.AllowUnsafeUpdates;
                adminWeb.AllowUnsafeUpdates = true;
                try
                {
                    SPList list = adminWeb.Lists.TryGetList(AdminListName);
                    if (list == null)
                    {
                        Guid id = adminWeb.Lists.Add(AdminListName,
                            "مسؤولو إدارة ملفات المركز الإعلامي - Media Files Administrators",
                            SPListTemplateType.GenericList);
                        list = adminWeb.Lists[id];
                        list.OnQuickLaunch = false;
                        list.Update();
                    }

                    EnsureField(list, "UserAccount", SPFieldType.User, "حساب المشرف");
                    SPFieldUser userField = list.Fields["UserAccount"] as SPFieldUser;
                    if (userField != null)
                    {
                        userField.AllowMultipleValues = false;
                        userField.SelectionMode = SPFieldUserSelectionMode.PeopleOnly;
                        userField.Presence = false;
                        userField.Update();
                    }

                    EnsureBoolField(list, "Active", "مفعّل", "1");
                    EnsureNumberField(list, "ItemOrder", "الترتيب", 1);

                    AddViewFields(list, "UserAccount", "Active", "ItemOrder");
                    list.Update();
                }
                catch (Exception ex)
                {
                    MediaFilesLog.Write("EnsureAdminUsersListOnWeb", ex);
                }
                finally
                {
                    adminWeb.AllowUnsafeUpdates = prevUnsafe;
                }
            }
        }

        #endregion

        #region Content List Provisioning (/ar/MediaCenter)

        /// <summary>
        /// Provisions the MediaFiles list on /ar/MediaCenter.
        /// </summary>
        public static void EnsureContentListOnWeb(SPSite site)
        {
            if (site == null) return;

            using (SPWeb contentWeb = MediaFilesTargetWeb.OpenContentWeb(site))
            {
                if (contentWeb == null)
                {
                    MediaFilesLog.Write("EnsureContentListOnWeb", "Content web /ar/MediaCenter could not be opened.");
                    return;
                }

                bool prevUnsafe = contentWeb.AllowUnsafeUpdates;
                contentWeb.AllowUnsafeUpdates = true;
                try
                {
                    SPList list = contentWeb.Lists.TryGetList(ContentListName);
                    if (list == null)
                    {
                        Guid id = contentWeb.Lists.Add(ContentListName,
                            "ملفات المركز الإعلامي (أسبوعية وشهرية)",
                            SPListTemplateType.GenericList);
                        list = contentWeb.Lists[id];
                        list.OnQuickLaunch = false;
                        list.EnableAttachments = true;
                        list.Update();
                    }
                    else
                    {
                        if (!list.EnableAttachments)
                        {
                            list.EnableAttachments = true;
                            list.Update();
                        }
                    }

                    // Update Title field title
                    try
                    {
                        SPField title = list.Fields.GetFieldByInternalName("Title");
                        if (title != null && title.Title != "اسم الملف")
                        {
                            title.Title = "اسم الملف";
                            title.Update();
                        }
                    }
                    catch { }

                    // MediaType Choice field ("أسبوعي", "شهري")
                    EnsureChoiceField(list, "MediaType", "نوع الملف", new string[] { "أسبوعي", "شهري" }, "أسبوعي");

                    // ItemOrder Number field
                    EnsureNumberField(list, "ItemOrder", "الترتيب", 1);

                    AddViewFields(list, "MediaType", "ItemOrder", "Attachments");
                    list.Update();
                }
                catch (Exception ex)
                {
                    MediaFilesLog.Write("EnsureContentListOnWeb", ex);
                }
                finally
                {
                    contentWeb.AllowUnsafeUpdates = prevUnsafe;
                }
            }
        }

        #endregion

        #region Authorization Logic

        /// <summary>
        /// Checks if current user is active in MediaFileAdminUsers on /ar/ContentAdmin.
        /// If user is not authenticated, triggers SharePoint modal login popup.
        /// </summary>
        public static bool CheckAdminAccess(Page page)
        {
            if (!ContentAdm.IsAuthenticated())
            {
                ContentAdm.PromptLogin(page);
                return false;
            }

            return IsAdmin();
        }

        /// <summary>
        /// Elevated check: returns true if current user is an active admin in MediaFileAdminUsers list on /ar/ContentAdmin.
        /// </summary>
        public static bool IsAdmin()
        {
            try
            {
                if (SPContext.Current == null || SPContext.Current.Web == null) return false;
                SPUser user = SPContext.Current.Web.CurrentUser;
                if (user == null) return false;

                // Same rule as ContentAdm.IsAdmin: access comes ONLY from the admin list
                int userId = user.ID;
                string userName = user.Name;
                Guid siteId = SPContext.Current.Site.ID;
                bool allowed = false;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb adminWeb = MediaFilesTargetWeb.OpenAdminWeb(site))
                    {
                        if (adminWeb == null) return;
                        allowed = IsListedMediaFileAdmin(adminWeb, userId, userName);
                    }
                });

                return allowed;
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("MediaFilesListProvisioner.IsAdmin", ex);
                return false;
            }
        }

        private static bool IsListedMediaFileAdmin(SPWeb web, int userId, string userName)
        {
            if (web == null) return false;

            try
            {
                SPList list = web.Lists.TryGetList(AdminListName);
                if (list == null || list.ItemCount == 0) return false;

                string safeName = System.Security.SecurityElement.Escape(userName ?? string.Empty);
                string safeUserId = userId.ToString(CultureInfo.InvariantCulture);

                // Same matching as ContentAdm.IsListedAdmin: UserAccount by lookup id or display name.
                // Free-text Title is deliberately NOT matched - anyone could type a name there.
                var query = new SPQuery
                {
                    RowLimit = 10,
                    Query =
                        "<Where>" +
                          "<Or>" +
                            "<Eq>" +
                              "<FieldRef Name='UserAccount' LookupId='TRUE' />" +
                              "<Value Type='Integer'>" + safeUserId + "</Value>" +
                            "</Eq>" +
                            "<Eq>" +
                              "<FieldRef Name='UserAccount' />" +
                              "<Value Type='User'>" + safeName + "</Value>" +
                            "</Eq>" +
                          "</Or>" +
                        "</Where>"
                };

                SPListItemCollection matches = list.GetItems(query);
                if (matches == null || matches.Count == 0) return false;

                foreach (SPListItem item in matches)
                {
                    string active = MediaFilesHelper.SafeString(item, "Active").Trim();
                    if (string.IsNullOrEmpty(active)) return true; // column blank defaults to enabled
                    if (active == "1" || active == "-1") return true;

                    bool flag;
                    if (bool.TryParse(active, out flag) && flag) return true;
                }
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("IsListedMediaFileAdmin", ex);
            }

            return false;
        }

        #endregion

        #region Helpers

        private static void EnsureField(SPList list, string name, SPFieldType type, string displayName)
        {
            if (!list.Fields.ContainsField(name))
            {
                string internalName = list.Fields.Add(name, type, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                if (!string.IsNullOrEmpty(displayName))
                {
                    f.Title = displayName;
                    f.Update();
                }
            }
        }

        private static void EnsureBoolField(SPList list, string name, string displayName, string defaultVal)
        {
            if (!list.Fields.ContainsField(name))
            {
                string internalName = list.Fields.Add(name, SPFieldType.Boolean, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                if (!string.IsNullOrEmpty(displayName)) f.Title = displayName;
                f.DefaultValue = defaultVal;
                f.Update();
            }
        }

        private static void EnsureNumberField(SPList list, string name, string displayName, double defaultVal)
        {
            if (!list.Fields.ContainsField(name))
            {
                string internalName = list.Fields.Add(name, SPFieldType.Number, false);
                SPFieldNumber f = list.Fields.GetFieldByInternalName(internalName) as SPFieldNumber;
                if (f != null)
                {
                    if (!string.IsNullOrEmpty(displayName)) f.Title = displayName;
                    f.DefaultValue = defaultVal.ToString(CultureInfo.InvariantCulture);
                    f.DisplayFormat = SPNumberFormatTypes.NoDecimal;
                    f.Update();
                }
            }
        }

        private static void EnsureChoiceField(SPList list, string name, string displayName, string[] choices, string defaultVal)
        {
            if (!list.Fields.ContainsField(name))
            {
                string internalName = list.Fields.Add(name, SPFieldType.Choice, false);
                SPFieldChoice f = list.Fields.GetFieldByInternalName(internalName) as SPFieldChoice;
                if (f != null)
                {
                    if (!string.IsNullOrEmpty(displayName)) f.Title = displayName;
                    f.Choices.Clear();
                    foreach (string c in choices) f.Choices.Add(c);
                    f.DefaultValue = defaultVal;
                    f.EditFormat = SPChoiceFormatType.Dropdown;
                    f.Update();
                }
            }
        }

        private static void AddViewFields(SPList list, params string[] fields)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;
                bool changed = false;
                foreach (string f in fields)
                {
                    if (list.Fields.ContainsField(f) && !view.ViewFields.Exists(f))
                    {
                        view.ViewFields.Add(f);
                        changed = true;
                    }
                }
                if (changed) view.Update();
            }
            catch { }
        }

        private static bool DoneThisRequest(string key)
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                return ctx != null && ctx.Items.Contains("MFProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null) ctx.Items["MFProv:" + key] = true;
            }
            catch { }
        }

        #endregion
    }
}
