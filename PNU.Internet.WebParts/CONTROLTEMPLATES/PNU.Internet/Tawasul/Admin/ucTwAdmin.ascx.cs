using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// Generic add / edit / delete control for every Tawasul list.
    /// The form is generated from TwListSchema, so adding a field to the schema
    /// provisions the column AND exposes it here automatically.
    ///
    /// State lives in HiddenFields and the form is rebuilt on every request, because
    /// ViewState is frequently disabled inside SharePoint web part zones.
    /// </summary>
    public partial class ucTwAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";
        private const string AdminUsersList = "AdminUsers";
        private const string DateFormat = "yyyy-MM-dd";

        /// <summary>Restrict the screen to a single list (hides the list picker).</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string FixedListName { get; set; }

        private readonly Dictionary<string, Control> _inputs = new Dictionary<string, Control>();
        private bool _isAdmin;
        private string _builtListName;

        // =====================================================================
        // lifecycle
        // =====================================================================
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                SetTitles();
                _isAdmin = IsAdmin();

                pnlDenied.Visible = !_isAdmin;
                pnlMain.Visible = _isAdmin;
                if (!_isAdmin) return;

                TwListProvisioner.EnsureAllListsExist();
                FillListPicker();

                // Rebuild the form for the list the PREVIOUS request rendered, so the
                // control tree LoadViewState walks is identical to the one that saved it.
                // Changing the picker rebuilds the form again in ddlList_SelectedIndexChanged,
                // which runs after ViewState has been loaded.
                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.OnInit", ex);
                ShowError(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            try
            {
                RestoreFormState();
                BindGrid();
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.Page_Load", ex);
                ShowError(ex.Message);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Record which list the form was actually rendered for, so the next
            // postback can rebuild the same control tree before ViewState is loaded.
            if (_isAdmin) hfListBuilt.Value = _builtListName ?? string.Empty;
        }

        /// <summary>
        /// The list the form was built for on the previous request. Read straight from
        /// the posted form because HiddenField values are not available yet in OnInit.
        /// </summary>
        private string ListRenderedLastRequest
        {
            get
            {
                try
                {
                    if (Page != null && Page.IsPostBack)
                    {
                        string posted = Request.Form[hfListBuilt.UniqueID];
                        if (!string.IsNullOrEmpty(posted) && TwListSchema.Get(posted) != null)
                            return posted;
                    }
                }
                catch { /* fall through to the current selection */ }

                return CurrentListName;
            }
        }

        // =====================================================================
        // labels
        // =====================================================================
        private void SetTitles()
        {
            ltPageTitle.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminTitle",
                "إدارة محتوى صفحة التواصل", "Tawasul (Contact) content management"));
            lblListLabel.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminList", "القسم", "Section"));
            ltNew.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminNew", " إضافة عنصر", " Add item"));
            ltActions.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminActions", "إجراءات", "Actions"));
            ltEmpty.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminEmpty",
                "لا توجد عناصر في هذا القسم بعد.", "No items in this section yet."));
            ltDenied.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminDenied",
                "ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            btnSave.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminSave", "حفظ", "Save"));
            btnCancel.Text = TwHelper.Enc(TwHelper.GetRes("Tw_AdminCancel", "إلغاء", "Cancel"));
        }

        // =====================================================================
        // list picker
        // =====================================================================
        private void FillListPicker()
        {
            ddlList.Items.Clear();

            if (!string.IsNullOrEmpty(FixedListName))
            {
                TwListDef only = TwListSchema.Get(FixedListName);
                if (only != null)
                {
                    ddlList.Items.Add(new ListItem(only.Display, only.Name));
                    ddlList.Visible = false;
                    lblListLabel.Visible = false;
                }
                return;
            }

            foreach (string name in TwListNames.AllLists)
            {
                TwListDef def = TwListSchema.Get(name);
                if (def != null) ddlList.Items.Add(new ListItem(def.Display, def.Name));
            }

            // Restore the posted selection before ViewState is applied.
            string posted = Request.Form[ddlList.UniqueID];
            if (string.IsNullOrEmpty(posted)) posted = Request.QueryString["list"];

            if (!string.IsNullOrEmpty(posted) && ddlList.Items.FindByValue(posted) != null)
                ddlList.SelectedValue = posted;
        }

        private string CurrentListName
        {
            get
            {
                if (!string.IsNullOrEmpty(FixedListName)) return FixedListName;
                return ddlList.SelectedValue;
            }
        }

        private TwListDef CurrentDef { get { return TwListSchema.Get(CurrentListName); } }

        // =====================================================================
        // dynamic form
        // =====================================================================
        /// <summary>
        /// Rebuilds the dynamic form for one list.
        /// ViewState is switched OFF for the generated controls: they are recreated on
        /// every request and their values arrive through the posted form, so there is
        /// nothing to persist - and nothing that can mismatch when the list changes.
        /// </summary>
        private void BuildForm(string listName)
        {
            phForm.Controls.Clear();
            phForm.EnableViewState = false;
            _inputs.Clear();
            _builtListName = listName;

            TwListDef def = TwListSchema.Get(listName);
            if (def == null) return;

            // Built-in Title column first.
            AddInput("Title", TwHelper.Pick("العنوان (عربي)", "Title (AR)"), SPFieldType.Text, 0, null, null);

            foreach (TwFieldDef f in def.Fields)
                AddInput(f.InternalName, f.Display, f.Type, f.Rows, f.Hint, f.Choices);
        }

        private void AddInput(string internalName, string label, SPFieldType type,
                              int rows, string hint, string[] choices)
        {
            bool wide = type == SPFieldType.Note;
            var col = new Panel { CssClass = wide ? "col-12" : "col-12 col-md-6", EnableViewState = false };

            var lbl = new Label
            {
                CssClass = "form-label",
                Text = TwHelper.Enc(label),
                AssociatedControlID = "fld_" + internalName
            };
            col.Controls.Add(lbl);

            Control input;

            if (type == SPFieldType.Boolean)
            {
                var flag = new DropDownList { ID = "fld_" + internalName, CssClass = "form-select", EnableViewState = false };
                flag.Items.Add(new ListItem(TwHelper.Pick("نعم", "Yes"), "1"));
                flag.Items.Add(new ListItem(TwHelper.Pick("لا", "No"), "0"));
                col.Controls.Add(flag);
                input = flag;
            }
            else if (type == SPFieldType.Choice && choices != null && choices.Length > 0)
            {
                var ddl = new DropDownList { ID = "fld_" + internalName, CssClass = "form-select", EnableViewState = false };
                foreach (string option in choices) ddl.Items.Add(new ListItem(option, option));
                col.Controls.Add(ddl);
                input = ddl;
            }
            else
            {
                var box = new TextBox { ID = "fld_" + internalName, CssClass = "form-control", EnableViewState = false };

                if (type == SPFieldType.Note)
                {
                    box.TextMode = TextBoxMode.MultiLine;
                    box.Rows = rows > 0 ? rows : 4;
                }
                else if (type == SPFieldType.DateTime)
                {
                    box.Attributes["placeholder"] = DateFormat;
                }

                col.Controls.Add(box);
                input = box;
            }

            if (type == SPFieldType.DateTime && string.IsNullOrEmpty(hint))
                hint = TwHelper.Pick("الصيغة: 2026-06-24", "Format: 2026-06-24");

            if (!string.IsNullOrEmpty(hint))
                col.Controls.Add(new Literal { Text = "<div class=\"form-text\">" + TwHelper.Enc(hint) + "</div>" });

            phForm.Controls.Add(col);
            _inputs[internalName] = input;
        }

        private string GetValue(string internalName)
        {
            Control c;
            if (!_inputs.TryGetValue(internalName, out c)) return string.Empty;

            TextBox box = c as TextBox;
            if (box != null) return (box.Text ?? string.Empty).Trim();

            DropDownList ddl = c as DropDownList;
            if (ddl != null) return ddl.SelectedValue ?? string.Empty;

            return string.Empty;
        }

        private void SetValue(string internalName, string value)
        {
            Control c;
            if (!_inputs.TryGetValue(internalName, out c)) return;

            TextBox box = c as TextBox;
            if (box != null) { box.Text = value ?? string.Empty; return; }

            DropDownList ddl = c as DropDownList;
            if (ddl != null && !string.IsNullOrEmpty(value) && ddl.Items.FindByValue(value) != null)
                ddl.SelectedValue = value;
        }

        private void RestoreFormState()
        {
            string mode = hfMode.Value;
            pnlForm.Visible = mode == ModeNew || mode == ModeEdit;

            ltFormTitle.Text = TwHelper.Enc(mode == ModeEdit
                ? TwHelper.GetRes("Tw_AdminEdit", "تعديل عنصر", "Edit item")
                : TwHelper.GetRes("Tw_AdminNew", "إضافة عنصر", "Add item"));
        }

        // =====================================================================
        // events
        // =====================================================================
        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Runs after ViewState has been loaded against the previous tree, so it is
            // safe to swap the form over to the newly selected list here.
            CloseForm();
            BuildForm(CurrentListName);
            BindGrid();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            hfMode.Value = ModeNew;
            hfItemId.Value = "0";
            RestoreFormState();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;

            if (e.CommandName == "EditItem") LoadItemIntoForm(id);
            else if (e.CommandName == "DeleteItem") DeleteItem(id);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        // =====================================================================
        // CRUD
        // =====================================================================
        private void LoadItemIntoForm(int id)
        {
            try
            {
                TwListDef def = CurrentDef;
                if (def == null) return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = TwTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = list.GetItemById(id);
                        SetValue("Title", TwHelper.SafeString(item, "Title"));

                        foreach (TwFieldDef f in def.Fields)
                        {
                            string value;

                            if (f.Type == SPFieldType.URL)
                            {
                                value = TwHelper.SafeUrl(item, f.InternalName);
                            }
                            else if (f.Type == SPFieldType.DateTime)
                            {
                                DateTime? d = TwHelper.SafeDate(item, f.InternalName);
                                value = d.HasValue
                                    ? d.Value.ToString(DateFormat, CultureInfo.InvariantCulture)
                                    : string.Empty;
                            }
                            else if (f.Type == SPFieldType.Boolean)
                            {
                                value = TwHelper.SafeBool(item, f.InternalName) ? "1" : "0";
                            }
                            else
                            {
                                value = TwHelper.SafeString(item, f.InternalName);
                            }

                            SetValue(f.InternalName, value);
                        }
                    }
                });

                hfMode.Value = ModeEdit;
                hfItemId.Value = id.ToString(CultureInfo.InvariantCulture);
                RestoreFormState();
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.LoadItemIntoForm", ex);
                ShowError(ex.Message);
            }
        }

        private void SaveItem()
        {
            TwListDef def = CurrentDef;
            if (def == null) return;

            string title = GetValue("Title");
            if (string.IsNullOrEmpty(title))
            {
                ShowError(TwHelper.GetRes("Tw_AdminTitleRequired", "العنوان مطلوب.", "Title is required."));
                RestoreFormState();
                return;
            }

            int id;
            int.TryParse(hfItemId.Value, out id);
            bool isNew = hfMode.Value != ModeEdit || id <= 0;

            // Snapshot the posted values before entering the elevated delegate.
            var values = new Dictionary<string, string>();
            foreach (TwFieldDef f in def.Fields)
                values[f.InternalName] = GetValue(f.InternalName);

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = TwTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(def.Name);
                            if (list == null) return;

                            SPListItem item = isNew ? list.AddItem() : list.GetItemById(id);
                            item["Title"] = title;

                            foreach (TwFieldDef f in def.Fields)
                            {
                                if (!list.Fields.ContainsField(f.InternalName)) continue;
                                string raw = values[f.InternalName];

                                if (f.Type == SPFieldType.URL)
                                {
                                    item[f.InternalName] = string.IsNullOrEmpty(raw)
                                        ? null
                                        : new SPFieldUrlValue { Url = raw, Description = title };
                                }
                                else if (f.Type == SPFieldType.Number)
                                {
                                    double d;
                                    item[f.InternalName] =
                                        double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out d)
                                            ? (object)d : null;
                                }
                                else if (f.Type == SPFieldType.DateTime)
                                {
                                    DateTime dt;
                                    item[f.InternalName] =
                                        DateTime.TryParse(raw, CultureInfo.InvariantCulture,
                                                          DateTimeStyles.None, out dt)
                                            ? (object)dt : null;
                                }
                                else if (f.Type == SPFieldType.Boolean)
                                {
                                    item[f.InternalName] = raw == "1";
                                }
                                else
                                {
                                    item[f.InternalName] = raw;
                                }
                            }

                            item.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = prevUnsafe;
                        }
                    }
                });

                CloseForm();
                BindGrid();
                ShowMessage(isNew
                    ? TwHelper.GetRes("Tw_AdminAdded", "تمت إضافة العنصر بنجاح.", "Item added successfully.")
                    : TwHelper.GetRes("Tw_AdminUpdated", "تم تحديث العنصر بنجاح.", "Item updated successfully."));
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.SaveItem", ex);
                ShowError(ex.Message);
                RestoreFormState();
            }
        }

        private void DeleteItem(int id)
        {
            TwListDef def = CurrentDef;
            if (def == null) return;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = TwTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(def.Name);
                            if (list == null) return;
                            list.GetItemById(id).Delete();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = prevUnsafe;
                        }
                    }
                });

                CloseForm();
                BindGrid();
                ShowMessage(TwHelper.GetRes("Tw_AdminDeleted", "تم حذف العنصر.", "Item deleted."));
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.DeleteItem", ex);
                ShowError(ex.Message);
            }
        }

        // =====================================================================
        // grid
        // =====================================================================
        private void BindGrid()
        {
            TwListDef def = CurrentDef;
            if (def == null) return;

            var headers = new List<string> { TwHelper.Enc(TwHelper.Pick("العنوان (عربي)", "Title (AR)")) };
            var gridFields = def.Fields.Where(f => f.InGrid).ToList();
            foreach (TwFieldDef f in gridFields) headers.Add(TwHelper.Enc(f.Display));

            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            string editText = TwHelper.Enc(TwHelper.GetRes("Tw_AdminEditBtn", "تعديل", "Edit"));
            string deleteText = TwHelper.Enc(TwHelper.GetRes("Tw_AdminDeleteBtn", "حذف", "Delete"));
            string confirmText = TwHelper.GetRes("Tw_AdminConfirm",
                "هل أنت متأكد من حذف هذا العنصر؟", "Are you sure you want to delete this item?")
                .Replace("'", "\\'");

            var rows = new List<TwAdminRow>();

            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = TwTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (SPListItem item in TwHelper.GetItems(web, def.Name))
                    {
                        var row = new TwAdminRow
                        {
                            Id = item.ID,
                            EditText = editText,
                            DeleteText = deleteText,
                            DeleteConfirm = "return confirm('" + confirmText + "');"
                        };

                        row.Cells.Add(TwHelper.Enc(TwHelper.SafeString(item, "Title")));

                        foreach (TwFieldDef f in gridFields)
                        {
                            string value;

                            if (f.Type == SPFieldType.URL)
                            {
                                value = TwHelper.SafeUrl(item, f.InternalName);
                            }
                            else if (f.Type == SPFieldType.DateTime)
                            {
                                DateTime? d = TwHelper.SafeDate(item, f.InternalName);
                                value = d.HasValue
                                    ? d.Value.ToString(DateFormat, CultureInfo.InvariantCulture)
                                    : string.Empty;
                            }
                            else
                            {
                                value = TwHelper.SafeString(item, f.InternalName);
                            }

                            row.Cells.Add(TwHelper.Enc(value));
                        }

                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.BindGrid:" + def.Name, ex);
            }

            rptItems.DataSource = rows;
            rptItems.DataBind();
            pnlEmpty.Visible = rows.Count == 0;
        }

        // =====================================================================
        // helpers
        // =====================================================================
        private void ClearForm()
        {
            foreach (Control c in _inputs.Values)
            {
                TextBox box = c as TextBox;
                if (box != null) { box.Text = string.Empty; continue; }

                DropDownList ddl = c as DropDownList;
                if (ddl != null && ddl.Items.Count > 0) ddl.SelectedIndex = 0;
            }
        }

        private void CloseForm()
        {
            hfMode.Value = string.Empty;
            hfItemId.Value = "0";
            pnlForm.Visible = false;
            ClearForm();
        }

        private void ShowMessage(string text)
        {
            ltMessage.Text = TwHelper.Enc(text);
            pnlMessage.Visible = true;
        }

        private void ShowError(string text)
        {
            ltError.Text = TwHelper.Enc(text);
            pnlError.Visible = true;
        }

        /// <summary>
        /// Access is granted ONLY by the AdminUsers list on /ar/ContentAdmin.
        /// Site collection administrators, ManageLists holders and authenticated users get
        /// nothing from those roles alone - the list is the single source of truth.
        ///
        /// The lookup runs elevated because editors are not expected to have read access
        /// to that web.
        /// </summary>
        private bool IsAdmin()
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
                    using (SPWeb adminWeb = TwTargetWeb.OpenAdminWeb(site))
                    {
                        allowed = IsListedAdmin(adminWeb, userId, userName);
                    }
                });

                return allowed;
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.IsAdmin", ex);
                return false;
            }
        }

        /// <summary>
        /// True when the user has an enabled row in the AdminUsers list on this web.
        /// UserAccount is a Person column, so it is matched two ways in one query:
        /// by lookup id (exact, survives a rename) and by display name (the form an
        /// editor sees). A missing web or list means "no match" - which denies access.
        /// An empty Active column counts as enabled.
        /// </summary>
        private bool IsListedAdmin(SPWeb web, int userId, string userName)
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
                    string active = TwHelper.SafeString(item, "Active").Trim();
                    if (active.Length == 0) return true;                // column missing or blank
                    if (active == "1" || active == "-1") return true;

                    bool flag;
                    if (bool.TryParse(active, out flag) && flag) return true;
                }
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwAdmin.IsListedAdmin:" + web.Url, ex);
            }

            return false;
        }
    }

    /// <summary>Row DTO for the admin grid - all values already HTML-encoded.</summary>
    public class TwAdminRow
    {
        public int Id { get; set; }
        public List<string> Cells { get; set; }
        public string EditText { get; set; }
        public string DeleteText { get; set; }
        public string DeleteConfirm { get; set; }

        public TwAdminRow() { Cells = new List<string>(); }
    }
}
