using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class DynamicMenuLoader : UserControl
    {
        #region Public Properties (set from hosting page / web part)

        /// <summary>
        /// Display name (Title) of the SharePoint list that holds menu items.
        /// </summary>
        public string MenuListName { get; set; }

        /// <summary>
        /// Server-relative URL of the web that contains the menu list.
        /// Example: "/ar/admin/" or "/sites/portal/ar/".
        /// </summary>
        public string WebUrl { get; set; }

        /// <summary>
        /// Querystring key used to track the currently selected menu item.
        /// </summary>
        public string SelectionQueryKey { get; set; } = "mid";

        #endregion

        #region Internal State

        protected bool IsRtl
        {
            get
            {
                try
                {
                    return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
                }
                catch { return false; }
            }
        }

        private List<MenuItemRow> _menuItems = new List<MenuItemRow>();
        private MenuItemRow _selectedItem;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadMenuItems();
                BindMenu();
                LoadSelectedUserControl();
            }
            catch (Exception ex)
            {
                ShowError("Error loading dynamic menu: " + HttpUtility.HtmlEncode(ex.Message));
            }
        }

        #region Data Loading

        private void LoadMenuItems()
        {
            if (string.IsNullOrWhiteSpace(MenuListName))
                throw new InvalidOperationException("MenuListName property is not set.");

            // Resolve web URL - default to current web if not provided
            string targetWebUrl = string.IsNullOrWhiteSpace(WebUrl)
                ? SPContext.Current.Web.ServerRelativeUrl
                : WebUrl;

            // Build absolute URL safely
            string absoluteUrl = ResolveAbsoluteWebUrl(targetWebUrl);

            using (SPSite site = new SPSite(absoluteUrl))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(MenuListName);
                if (list == null)
                {
                    // Auto-provision the list (with required fields) if it doesn't exist
                    EnsureList(site.ID, web.ID);

                    // Re-open to pick up the freshly provisioned list
                    list = web.Lists.TryGetList(MenuListName);
                    if (list == null)
                        throw new InvalidOperationException(
                            string.Format("Failed to provision list '{0}' in web '{1}'.",
                                MenuListName, web.Url));
                }

                SPQuery query = new SPQuery
                {
                    Query = @"<Where>
                                <Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq>
                              </Where>
                              <OrderBy>
                                <FieldRef Name='ItemOrder' Ascending='TRUE'/>
                              </OrderBy>",
                    ViewFields = @"<FieldRef Name='ID'/>
                                   <FieldRef Name='TitleAr'/>
                                   <FieldRef Name='TitleEn'/>
                                   <FieldRef Name='userControlPath'/>
                                   <FieldRef Name='ItemOrder'/>
                                   <FieldRef Name='Visibility'/>
                                   <FieldRef Name='Setting'/>",
                    ViewFieldsOnly = true,
                    RowLimit = 200
                };

                SPListItemCollection items = list.GetItems(query);
                foreach (SPListItem item in items)
                {
                    _menuItems.Add(new MenuItemRow
                    {
                        Id = item.ID,
                        TitleAr = SafeString(item, "TitleAr"),
                        TitleEn = SafeString(item, "TitleEn"),
                        UserControlPath = "~/_controltemplates/15/" +  SafeString(item, "userControlPath"),
                        ItemOrder = SafeDouble(item, "ItemOrder"),
                        Visibility = SafeBool(item, "Visibility"),
                        Setting = SafeString(item, "Setting")
                    });
                }
            }
        }

        private string ResolveAbsoluteWebUrl(string serverRelativeOrAbsolute)
        {
            if (Uri.IsWellFormedUriString(serverRelativeOrAbsolute, UriKind.Absolute))
                return serverRelativeOrAbsolute;

            Uri currentUri = HttpContext.Current.Request.Url;
            string baseUrl = currentUri.Scheme + "://" + currentUri.Authority;
            string rel = serverRelativeOrAbsolute.StartsWith("/") ? serverRelativeOrAbsolute : "/" + serverRelativeOrAbsolute;
            return baseUrl + rel.TrimEnd('/');
        }

        /// <summary>
        /// Creates the menu list and required fields if they don't already exist.
        /// Runs with elevated privileges so non-admin visitors don't get blocked.
        /// </summary>
        private void EnsureList(Guid siteId, Guid webId)
        {
            string listName = MenuListName;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite elevatedSite = new SPSite(siteId))
                using (SPWeb elevatedWeb = elevatedSite.OpenWeb(webId))
                {
                    bool prevAllowUnsafe = elevatedWeb.AllowUnsafeUpdates;
                    elevatedWeb.AllowUnsafeUpdates = true;
                    try
                    {
                        SPList list = elevatedWeb.Lists.TryGetList(listName);
                        if (list == null)
                        {
                            Guid listId = elevatedWeb.Lists.Add(
                                listName,
                                "Dynamic side-menu items for DynamicMenuLoader control.",
                                SPListTemplateType.GenericList);

                            list = elevatedWeb.Lists[listId];
                            list.OnQuickLaunch = false;
                            list.Update();
                        }

                        EnsureField(list, "TitleAr", SPFieldType.Text,
                            "<Field Type='Text' DisplayName='TitleAr' Name='TitleAr' StaticName='TitleAr' Required='FALSE' />");

                        EnsureField(list, "TitleEn", SPFieldType.Text,
                            "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' StaticName='TitleEn' Required='FALSE' />");

                        EnsureField(list, "userControlPath", SPFieldType.Text,
                            "<Field Type='Text' DisplayName='userControlPath' Name='userControlPath' StaticName='userControlPath' Required='TRUE' />");

                        EnsureField(list, "ItemOrder", SPFieldType.Number,
                            "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' StaticName='ItemOrder' Decimals='0' Required='FALSE' />");

                        EnsureField(list, "Visibility", SPFieldType.Boolean,
                            "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility' StaticName='Visibility'><Default>1</Default></Field>");

                        EnsureField(list, "Setting", SPFieldType.Note,
                            "<Field Type='Note' DisplayName='Setting' Name='Setting' StaticName='Setting' NumLines='3' RichText='FALSE' Required='FALSE' />");

                        // Surface the new fields on the default view so admins can edit them easily
                        EnsureFieldsOnDefaultView(list,
                            "TitleAr", "TitleEn", "userControlPath", "ItemOrder", "Visibility", "Setting");
                    }
                    finally
                    {
                        elevatedWeb.AllowUnsafeUpdates = prevAllowUnsafe;
                    }
                }
            });
        }

        private static void EnsureField(SPList list, string internalName, SPFieldType type, string schemaXml)
        {
            if (list.Fields.ContainsField(internalName))
                return;

            // AddFieldAsXml respects the StaticName/Name in the schema, avoiding mangled internal names
            list.Fields.AddFieldAsXml(schemaXml, true, SPAddFieldOptions.AddFieldInternalNameHint);
            list.Update();
        }

        private static void EnsureFieldsOnDefaultView(SPList list, params string[] internalNames)
        {
            SPView view = list.DefaultView;
            if (view == null) return;

            bool changed = false;
            foreach (string n in internalNames)
            {
                if (!list.Fields.ContainsField(n)) continue;
                if (!view.ViewFields.Exists(n))
                {
                    view.ViewFields.Add(n);
                    changed = true;
                }
            }
            if (changed) view.Update();
        }

        #endregion

        #region Menu Binding

        private void BindMenu()
        {
            // Determine selected item from query string, or default to first
            int selectedId = 0;
            int.TryParse(Request.QueryString[SelectionQueryKey], out selectedId);

            if (selectedId > 0)
                _selectedItem = _menuItems.FirstOrDefault(m => m.Id == selectedId);

            if (_selectedItem == null && _menuItems.Count > 0)
                _selectedItem = _menuItems.First();

            rptMenu.DataSource = _menuItems;
            rptMenu.DataBind();

            if (_menuItems.Count == 0)
                litEmpty.Visible = true;
        }

        protected void rptMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            MenuItemRow row = (MenuItemRow)e.Item.DataItem;
            HyperLink hl = (HyperLink)e.Item.FindControl("hlItem");
            if (hl == null) return;

            hl.Text = HttpUtility.HtmlEncode(IsRtl
                ? (string.IsNullOrEmpty(row.TitleAr) ? row.TitleEn : row.TitleAr)
                : (string.IsNullOrEmpty(row.TitleEn) ? row.TitleAr : row.TitleEn));

            // Build URL preserving existing query string, replacing the selection key
            hl.NavigateUrl = BuildSelectionUrl(row.Id);

            if (_selectedItem != null && row.Id == _selectedItem.Id)
                hl.CssClass = "active";
        }

        private string BuildSelectionUrl(int id)
        {
            var qs = HttpUtility.ParseQueryString(Request.Url.Query);
            qs[SelectionQueryKey] = id.ToString(CultureInfo.InvariantCulture);
            string path = Request.Url.AbsolutePath;
            return path + "?" + qs.ToString();
        }

        #endregion

        #region User Control Loading

        private void LoadSelectedUserControl()
        {
            if (_selectedItem == null || string.IsNullOrWhiteSpace(_selectedItem.UserControlPath))
                return;

            try
            {
                Control loaded = Page.LoadControl(_selectedItem.UserControlPath);
                if (loaded == null)
                {
                    ShowError("Failed to load user control: " +
                        HttpUtility.HtmlEncode(_selectedItem.UserControlPath));
                    return;
                }

                // Apply settings from "Setting" column (Key:Value pairs separated by comma)
                ApplySettings(loaded, _selectedItem.Setting);

                phContent.Controls.Add(loaded);
            }
            catch (Exception ex)
            {
                ShowError(string.Format("Error loading control '{0}': {1}",
                    HttpUtility.HtmlEncode(_selectedItem.UserControlPath),
                    HttpUtility.HtmlEncode(ex.Message)));
            }
        }

        /// <summary>
        /// Parses "Key1:Value1,Key2:Value2" format and assigns each value to a public
        /// property of matching name on the loaded control via reflection.
        /// </summary>
        private void ApplySettings(Control control, string settingString)
        {
            if (control == null || string.IsNullOrWhiteSpace(settingString))
                return;

            Dictionary<string, string> settings = ParseSettings(settingString);
            Type controlType = control.GetType();

            foreach (var kvp in settings)
            {
                PropertyInfo prop = controlType.GetProperty(
                    kvp.Key,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (prop == null || !prop.CanWrite)
                    continue;

                try
                {
                    object converted = ConvertValue(kvp.Value, prop.PropertyType);
                    prop.SetValue(control, converted, null);
                }
                catch
                {
                    // Skip properties that fail to convert; don't break the whole load
                }
            }
        }

        private Dictionary<string, string> ParseSettings(string raw)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(raw)) return dict;

            // Split on commas - each pair is "Key:Value"
            string[] pairs = raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pair in pairs)
            {
                int idx = pair.IndexOf(':');
                if (idx <= 0 || idx >= pair.Length - 1) continue;

                string key = pair.Substring(0, idx).Trim();
                string value = pair.Substring(idx + 1).Trim();

                if (!string.IsNullOrEmpty(key) && !dict.ContainsKey(key))
                    dict[key] = value;
            }
            return dict;
        }

        private object ConvertValue(string raw, Type targetType)
        {
            if (targetType == typeof(string)) return raw;
            if (targetType.IsEnum) return Enum.Parse(targetType, raw, true);

            Type underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
            return Convert.ChangeType(raw, underlying, CultureInfo.InvariantCulture);
        }

        #endregion

        #region Helpers

        private void ShowError(string message)
        {
            litErrors.Visible = true;
            litErrors.Text = "<div class=\"pnu-dml-error\">" + message + "</div>";
        }

        private static string SafeString(SPListItem item, string field)
        {
            if (item == null || !item.Fields.ContainsField(field)) return string.Empty;
            object v = item[field];
            return v == null ? string.Empty : v.ToString();
        }

        private static double SafeDouble(SPListItem item, string field)
        {
            if (item == null || !item.Fields.ContainsField(field)) return 0;
            object v = item[field];
            if (v == null) return 0;
            double d;
            return double.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0;
        }

        private static bool SafeBool(SPListItem item, string field)
        {
            if (item == null || !item.Fields.ContainsField(field)) return false;
            object v = item[field];
            if (v == null) return false;
            bool b;
            return bool.TryParse(v.ToString(), out b) && b;
        }

        #endregion

        #region Row Model

        private class MenuItemRow
        {
            public int Id { get; set; }
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string UserControlPath { get; set; }
            public double ItemOrder { get; set; }
            public bool Visibility { get; set; }
            public string Setting { get; set; }
        }

        #endregion
    }
}
