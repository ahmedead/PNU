using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ContentAdmin
{
    /// <summary>
    /// Idempotent C# provisioner for ContentAdmin metadata & configuration SharePoint lists.
    /// Lists created:
    ///   1. "ContentAdminModules": Stores dynamically registered admin user controls (title, path, icon, provisioner).
    ///   2. "ContentAdminConfigs": Key-value configuration store for the Admin Hub.
    /// </summary>
    public static class ContentAdminListProvisioner
    {
        public const string LIST_MODULES = "ContentAdminModules";
        public const string LIST_CONFIGS = "ContentAdminConfigs";
        public const string DEFAULT_ADMIN_WEB = "/ar/ContentAdmin";

        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        #region Public Entry Points

        /// <summary>
        /// Ensures that ContentAdminModules and ContentAdminConfigs exist on the specified or current admin web.
        /// </summary>
        public static void EnsureAllListsExist(string adminWebPath = null)
        {
            string resolvedPath = ResolveAdminWebPath(adminWebPath);
            if (DoneThisRequest("ContentAdminAllLists:" + resolvedPath)) return;

            try
            {
                if (SPContext.Current == null) return;

                Guid siteId = SPContext.Current.Site.ID;
                string currentWebUrl = SPContext.Current.Web != null ? SPContext.Current.Web.ServerRelativeUrl : DEFAULT_ADMIN_WEB;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = OpenWebByPath(site, resolvedPath) ?? OpenWebByPath(site, currentWebUrl))
                        {
                            if (web == null || !web.Exists) return;

                            EnsureModulesList(web);
                            EnsureConfigsList(web);
                        }
                    }
                });

                MarkDoneThisRequest("ContentAdminAllLists:" + resolvedPath);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request.Url.ToString() ?? "", "ContentAdminListProvisioner.EnsureAllListsExist", ex.Message);
            }
        }

        #endregion

        #region List Creation & Schema Definition

        private static void EnsureModulesList(SPWeb web)
        {
            string key = CacheKey(web.ID, LIST_MODULES);
            lock (_lock)
            {
                if (_provisioned.Contains(key)) return;
            }

            try
            {
                SPList list = web.Lists.TryGetList(LIST_MODULES);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(LIST_MODULES, "سجل وحدات التحكم الإدارية للبوابة (Admin User Controls Registry)", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                    list.OnQuickLaunch = true;
                    list.Update();
                }

                bool listChanged = false;

                // Ensure Fields
                listChanged |= EnsureField(list, "Title_EN", "Title (EN)", SPFieldType.Text, false);
                listChanged |= EnsureField(list, "ModuleKey", "Module Key", SPFieldType.Text, true);
                listChanged |= EnsureField(list, "ControlPath", "Control Virtual Path", SPFieldType.Text, true);
                listChanged |= EnsureField(list, "Category", "Category (AR)", SPFieldType.Text, false);
                listChanged |= EnsureField(list, "Category_EN", "Category (EN)", SPFieldType.Text, false);
                listChanged |= EnsureField(list, "Icon", "Bootstrap Icon Class", SPFieldType.Text, false);
                listChanged |= EnsureField(list, "DefaultTargetWeb", "Default Target Web", SPFieldType.Text, false);
                listChanged |= EnsureField(list, "ProvisionerType", "Provisioner Type / Action", SPFieldType.Text, false);
                listChanged |= EnsureField(list, "Description", "Description (AR)", SPFieldType.Note, false);
                listChanged |= EnsureField(list, "Description_EN", "Description (EN)", SPFieldType.Note, false);
                listChanged |= EnsureField(list, "ItemOrder", "Item Order", SPFieldType.Number, false);
                listChanged |= EnsureField(list, "IsActive", "Is Active", SPFieldType.Boolean, false);

                if (listChanged)
                {
                    list.Update();
                }

                // Pre-seed default built-in modules if empty
                if (list.ItemCount == 0)
                {
                    SeedDefaultModules(list);
                }

                lock (_lock)
                {
                    _provisioned.Add(key);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request.Url.ToString() ?? "", "ContentAdminListProvisioner.EnsureModulesList", ex.Message);
            }
        }

        private static void EnsureConfigsList(SPWeb web)
        {
            string key = CacheKey(web.ID, LIST_CONFIGS);
            lock (_lock)
            {
                if (_provisioned.Contains(key)) return;
            }

            try
            {
                SPList list = web.Lists.TryGetList(LIST_CONFIGS);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(LIST_CONFIGS, "إعدادات لوحة إدارة المحتوى (Content Admin Configurations)", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                    list.OnQuickLaunch = true;
                    list.Update();
                }

                bool listChanged = false;

                // Ensure Fields
                listChanged |= EnsureField(list, "ConfigValue", "Config Value", SPFieldType.Note, false);
                listChanged |= EnsureField(list, "Description", "Description", SPFieldType.Note, false);
                listChanged |= EnsureField(list, "IsActive", "Is Active", SPFieldType.Boolean, false);

                if (listChanged)
                {
                    list.Update();
                }

                // Pre-seed default configurations if empty
                if (list.ItemCount == 0)
                {
                    SeedDefaultConfigs(list);
                }

                lock (_lock)
                {
                    _provisioned.Add(key);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request.Url.ToString() ?? "", "ContentAdminListProvisioner.EnsureConfigsList", ex.Message);
            }
        }

        private static bool EnsureField(SPList list, string internalName, string displayName, SPFieldType fieldType, bool required)
        {
            if (list == null || list.Fields.ContainsField(internalName)) return false;

            try
            {
                string fieldName = list.Fields.Add(internalName, fieldType, required);
                SPField field = list.Fields.GetFieldByInternalName(fieldName);
                if (field != null)
                {
                    field.Title = displayName;
                    field.Update();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Seeding Default Data

        private static void SeedDefaultModules(SPList list)
        {
            var seedData = AdminModuleRegistry.GetDefaultBuiltInModules();

            int order = 10;
            foreach (var item in seedData)
            {
                SPListItem row = list.Items.Add();
                row["Title"] = item.TitleAr;
                row["Title_EN"] = !string.IsNullOrEmpty(item.TitleEn) ? item.TitleEn : item.Key;
                row["ModuleKey"] = item.Key;
                row["ControlPath"] = item.ControlPath;
                row["Category"] = item.Category;
                row["Category_EN"] = item.Category;
                row["Icon"] = item.Icon;
                row["DefaultTargetWeb"] = item.DefaultTargetWeb;
                row["ProvisionerType"] = item.ProvisionerTypeName ?? string.Empty;
                row["Description"] = item.Description;
                row["ItemOrder"] = order;
                row["IsActive"] = true;
                row.Update();
                order += 10;
            }
        }

        private static void SeedDefaultConfigs(SPList list)
        {
            var defaultConfigs = new Dictionary<string, Tuple<string, string>>
            {
                { "DefaultTemplate", Tuple.Create("SidebarWorkspace", "Default layout template: SidebarWorkspace or CardsDashboard") },
                { "DefaultAdminListName", Tuple.Create("AdminUsers", "Name of the admin user list granting portal permissions") },
                { "DefaultAdminWebUrl", Tuple.Create("/ar/ContentAdmin", "Server-relative path of the web hosting the admin user list") },
                { "DefaultTargetWeb", Tuple.Create("/ar/ContentAdmin", "Default server-relative target web for content administration") },
                { "AutoProvisionOnLoad", Tuple.Create("true", "Automatically ensure lists exist when a module is opened") }
            };

            foreach (var kvp in defaultConfigs)
            {
                SPListItem row = list.Items.Add();
                row["Title"] = kvp.Key;
                row["ConfigValue"] = kvp.Value.Item1;
                row["Description"] = kvp.Value.Item2;
                row["IsActive"] = true;
                row.Update();
            }
        }

        #endregion

        #region Data Retrieval & Execution

        /// <summary>
        /// Reads all active modules from the ContentAdminModules SharePoint list with fallback to built-in catalog.
        /// </summary>
        public static List<AdminModuleItem> GetRegisteredModules(string adminWebPath = null)
        {
            var result = new List<AdminModuleItem>();
            string resolvedPath = ResolveAdminWebPath(adminWebPath);

            try
            {
                if (SPContext.Current == null) return AdminModuleRegistry.GetDefaultBuiltInModules();

                Guid siteId = SPContext.Current.Site.ID;
                string currentWebUrl = SPContext.Current.Web != null ? SPContext.Current.Web.ServerRelativeUrl : DEFAULT_ADMIN_WEB;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = OpenWebByPath(site, resolvedPath) ?? OpenWebByPath(site, currentWebUrl))
                        {
                            if (web == null || !web.Exists) return;

                            SPList list = web.Lists.TryGetList(LIST_MODULES);
                            if (list == null || list.ItemCount == 0) return;

                            SPQuery query = new SPQuery
                            {
                                Query = "<Where><Eq><FieldRef Name='IsActive' /><Value Type='Boolean'>1</Value></Eq></Where>" +
                                        "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /><FieldRef Name='Title' Ascending='TRUE' /></OrderBy>"
                            };

                            SPListItemCollection items = list.GetItems(query);
                            foreach (SPListItem item in items)
                            {
                                string key = SafeString(item, "ModuleKey");
                                if (string.IsNullOrEmpty(key)) key = SafeString(item, "Title");

                                string provTypeName = SafeString(item, "ProvisionerType");

                                var mod = new AdminModuleItem
                                {
                                    Key = key,
                                    TitleAr = SafeString(item, "Title"),
                                    TitleEn = SafeString(item, "Title_EN"),
                                    Category = SafeString(item, "Category"),
                                    Icon = SafeString(item, "Icon", "bi-grid"),
                                    ControlPath = SafeString(item, "ControlPath"),
                                    DefaultTargetWeb = SafeString(item, "DefaultTargetWeb", "/ar"),
                                    Description = SafeString(item, "Description"),
                                    ProvisionerTypeName = provTypeName,
                                    ProvisionAction = CreateProvisionAction(provTypeName)
                                };

                                if (!string.IsNullOrEmpty(mod.ControlPath))
                                {
                                    result.Add(mod);
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request.Url.ToString() ?? "", "ContentAdminListProvisioner.GetRegisteredModules", ex.Message);
            }

            // Fallback to built-in catalog if list was not created/populated yet
            if (result.Count == 0)
            {
                result = AdminModuleRegistry.GetDefaultBuiltInModules();
            }

            return result;
        }

        /// <summary>
        /// Reads a configuration value from ContentAdminConfigs.
        /// </summary>
        public static string GetConfigValue(string configKey, string defaultValue = "", string adminWebPath = null)
        {
            if (string.IsNullOrEmpty(configKey)) return defaultValue;
            string resolvedPath = ResolveAdminWebPath(adminWebPath);

            try
            {
                if (SPContext.Current == null) return defaultValue;

                string val = null;
                Guid siteId = SPContext.Current.Site.ID;
                string currentWebUrl = SPContext.Current.Web != null ? SPContext.Current.Web.ServerRelativeUrl : DEFAULT_ADMIN_WEB;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = OpenWebByPath(site, resolvedPath) ?? OpenWebByPath(site, currentWebUrl))
                        {
                            if (web == null || !web.Exists) return;

                            SPList list = web.Lists.TryGetList(LIST_CONFIGS);
                            if (list == null || list.ItemCount == 0) return;

                            string safeKey = System.Security.SecurityElement.Escape(configKey);
                            SPQuery query = new SPQuery
                            {
                                RowLimit = 1,
                                Query = string.Format(
                                    "<Where><And><Eq><FieldRef Name='IsActive' /><Value Type='Boolean'>1</Value></Eq>" +
                                    "<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></And></Where>", safeKey)
                            };

                            SPListItemCollection items = list.GetItems(query);
                            if (items != null && items.Count > 0)
                            {
                                val = SafeString(items[0], "ConfigValue");
                            }
                        }
                    }
                });

                return val ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Dynamically creates an executable Action to invoke the module's ListProvisioner.
        /// Supports class type names or method calls.
        /// </summary>
        public static Action CreateProvisionAction(string provisionerTypeName)
        {
            if (string.IsNullOrEmpty(provisionerTypeName)) return null;

            return () =>
            {
                try
                {
                    Type type = Type.GetType(provisionerTypeName);
                    if (type == null)
                    {
                        // Look up in current executing assemblies
                        foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            type = asm.GetType(provisionerTypeName);
                            if (type != null) break;
                        }
                    }

                    if (type != null)
                    {
                        MethodInfo method = type.GetMethod("EnsureAllListsExist", BindingFlags.Public | BindingFlags.Static)
                                         ?? type.GetMethod("EnsureLists", BindingFlags.Public | BindingFlags.Static);

                        if (method != null)
                        {
                            method.Invoke(null, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current?.Request.Url.ToString() ?? "", "ContentAdminListProvisioner.InvokeProvisioner", ex.Message);
                }
            };
        }

        #endregion

        #region Helpers

        private static string ResolveAdminWebPath(string customPath)
        {
            if (!string.IsNullOrEmpty(customPath)) return customPath;
            if (SPContext.Current != null && SPContext.Current.Web != null)
                return SPContext.Current.Web.ServerRelativeUrl;
            return DEFAULT_ADMIN_WEB;
        }

        private static SPWeb OpenWebByPath(SPSite site, string path)
        {
            if (site == null || string.IsNullOrEmpty(path)) return null;

            string serverRelative = path.Replace('\\', '/').Trim();
            if (!serverRelative.StartsWith("/", StringComparison.Ordinal))
                serverRelative = "/" + serverRelative;

            string root = (site.ServerRelativeUrl ?? "/").TrimEnd('/');
            if (root.Length > 0 && !serverRelative.StartsWith(root + "/", StringComparison.OrdinalIgnoreCase) && !serverRelative.Equals(root, StringComparison.OrdinalIgnoreCase))
            {
                serverRelative = root + serverRelative;
            }

            try
            {
                SPWeb web = site.OpenWeb(serverRelative, true);
                if (web != null && web.Exists) return web;
                if (web != null) web.Dispose();
            }
            catch { }

            return null;
        }

        private static string SafeString(SPListItem item, string fieldInternalName, string fallback = "")
        {
            if (item == null || string.IsNullOrEmpty(fieldInternalName)) return fallback;
            if (!item.Fields.ContainsField(fieldInternalName)) return fallback;
            object val = item[fieldInternalName];
            return val != null ? val.ToString() : fallback;
        }

        private static bool DoneThisRequest(string key)
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                return ctx != null && ctx.Items.Contains("ContentAdminProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null) ctx.Items["ContentAdminProv:" + key] = true;
            }
            catch { }
        }

        private static string CacheKey(Guid webId, string listName)
        {
            return webId.ToString("N") + ":" + listName.ToLowerInvariant();
        }

        #endregion
    }
}
