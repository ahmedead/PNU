using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// Generic add / edit / delete control for every Agency list.
    /// The form is generated from AgListSchema, so adding a field to the schema
    /// provisions the column AND exposes it here automatically.
    ///
    /// State lives in HiddenFields and the form is rebuilt on every request, because
    /// ViewState is frequently disabled inside SharePoint web part zones.
    /// </summary>
    public partial class ucAgAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";
        private const string AdminUsersList = "AdminUsers";

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

                AgListProvisioner.EnsureAllListsExist();
                FillListPicker();

                // Rebuild the form for the list the PREVIOUS request rendered, so the
                // control tree LoadViewState walks is identical to the one that saved it.
                // Changing the picker rebuilds the form again in ddlList_SelectedIndexChanged,
                // which runs after ViewState has been loaded.
                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgAdmin.OnInit", ex);
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
                AgLog.Write("ucAgAdmin.Page_Load", ex);
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
                        if (!string.IsNullOrEmpty(posted) && AgListSchema.Get(posted) != null)
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
            ltPageTitle.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminTitle",
                "إدارة محتوى وكالة الجامعة", "University Agency content management"));
            lblListLabel.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminList", "القسم", "Section"));
            ltNew.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminNew", " إضافة عنصر", " Add item"));
            ltActions.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminActions", "إجراءات", "Actions"));
            ltEmpty.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminEmpty",
                "لا توجد عناصر في هذا القسم بعد.", "No items in this section yet."));
            ltDenied.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminDenied",
                "ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            btnSave.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminSave", "حفظ", "Save"));
            btnCancel.Text = AgHelper.Enc(AgHelper.GetRes("Ag_AdminCancel", "إلغاء", "Cancel"));
        }

        // =====================================================================
        // list picker
        // =====================================================================
        private void FillListPicker()
        {
            ddlList.Items.Clear();

            if (!string.IsNullOrEmpty(FixedListName))
            {
                AgListDef only = AgListSchema.Get(FixedListName);
                if (only != null)
                {
                    ddlList.Items.Add(new ListItem(only.Display, only.Name));
                    ddlList.Visible = false;
                    lblListLabel.Visible = false;
                }
                return;
            }

            foreach (string name in AgListNames.AllLists)
            {
                AgListDef def = AgListSchema.Get(name);
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

        private AgListDef CurrentDef { get { return AgListSchema.Get(CurrentListName); } }

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

            AgListDef def = AgListSchema.Get(listName);
            if (def == null) return;

            // Built-in Title column first.
            AddInput("Title", AgHelper.Pick("العنوان (عربي)", "Title (AR)"), SPFieldType.Text, 0, null, null);

            foreach (AgFieldDef f in def.Fields)
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
                Text = AgHelper.Enc(label),
                AssociatedControlID = "fld_" + internalName
            };
            col.Controls.Add(lbl);

            Control input;

            if (type == SPFieldType.Choice && choices != null && choices.Length > 0)
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

                col.Controls.Add(box);
                input = box;
            }

            if (!string.IsNullOrEmpty(hint))
                col.Controls.Add(new Literal { Text = "<div class=\"form-text\">" + AgHelper.Enc(hint) + "</div>" });

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

            ltFormTitle.Text = AgHelper.Enc(mode == ModeEdit
                ? AgHelper.GetRes("Ag_AdminEdit", "تعديل عنصر", "Edit item")
                : AgHelper.GetRes("Ag_AdminNew", "إضافة عنصر", "Add item"));
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
                AgListDef def = CurrentDef;
                if (def == null) return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = list.GetItemById(id);
                        SetValue("Title", AgHelper.SafeString(item, "Title"));

                        foreach (AgFieldDef f in def.Fields)
                        {
                            string value = f.Type == SPFieldType.URL
                                ? AgHelper.SafeUrl(item, f.InternalName)
                                : AgHelper.SafeString(item, f.InternalName);

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
                AgLog.Write("ucAgAdmin.LoadItemIntoForm", ex);
                ShowError(ex.Message);
            }
        }

        private void SaveItem()
        {
            AgListDef def = CurrentDef;
            if (def == null) return;

            string title = GetValue("Title");
            if (string.IsNullOrEmpty(title))
            {
                ShowError(AgHelper.GetRes("Ag_AdminTitleRequired", "العنوان مطلوب.", "Title is required."));
                RestoreFormState();
                return;
            }

            int id;
            int.TryParse(hfItemId.Value, out id);
            bool isNew = hfMode.Value != ModeEdit || id <= 0;

            // Snapshot the posted values before entering the elevated delegate.
            var values = new Dictionary<string, string>();
            foreach (AgFieldDef f in def.Fields)
                values[f.InternalName] = GetValue(f.InternalName);

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(def.Name);
                            if (list == null) return;

                            SPListItem item = isNew ? list.AddItem() : list.GetItemById(id);
                            item["Title"] = title;

                            foreach (AgFieldDef f in def.Fields)
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
                    ? AgHelper.GetRes("Ag_AdminAdded", "تمت إضافة العنصر بنجاح.", "Item added successfully.")
                    : AgHelper.GetRes("Ag_AdminUpdated", "تم تحديث العنصر بنجاح.", "Item updated successfully."));
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgAdmin.SaveItem", ex);
                ShowError(ex.Message);
                RestoreFormState();
            }
        }

        private void DeleteItem(int id)
        {
            AgListDef def = CurrentDef;
            if (def == null) return;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
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
                ShowMessage(AgHelper.GetRes("Ag_AdminDeleted", "تم حذف العنصر.", "Item deleted."));
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgAdmin.DeleteItem", ex);
                ShowError(ex.Message);
            }
        }

        // =====================================================================
        // grid
        // =====================================================================
        private void BindGrid()
        {
            AgListDef def = CurrentDef;
            if (def == null) return;

            var headers = new List<string> { AgHelper.Enc(AgHelper.Pick("العنوان (عربي)", "Title (AR)")) };
            var gridFields = def.Fields.Where(f => f.InGrid).ToList();
            foreach (AgFieldDef f in gridFields) headers.Add(AgHelper.Enc(f.Display));

            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            string editText = AgHelper.Enc(AgHelper.GetRes("Ag_AdminEditBtn", "تعديل", "Edit"));
            string deleteText = AgHelper.Enc(AgHelper.GetRes("Ag_AdminDeleteBtn", "حذف", "Delete"));
            string confirmText = AgHelper.GetRes("Ag_AdminConfirm",
                "هل أنت متأكد من حذف هذا العنصر؟", "Are you sure you want to delete this item?")
                .Replace("'", "\\'");

            var rows = new List<AgAdminRow>();

            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    foreach (SPListItem item in AgHelper.GetItems(web, def.Name))
                    {
                        var row = new AgAdminRow
                        {
                            Id = item.ID,
                            EditText = editText,
                            DeleteText = deleteText,
                            DeleteConfirm = "return confirm('" + confirmText + "');"
                        };

                        row.Cells.Add(AgHelper.Enc(AgHelper.SafeString(item, "Title")));

                        foreach (AgFieldDef f in gridFields)
                        {
                            string value = f.Type == SPFieldType.URL
                                ? AgHelper.SafeUrl(item, f.InternalName)
                                : AgHelper.SafeString(item, f.InternalName);

                            row.Cells.Add(AgHelper.Enc(value));
                        }

                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgAdmin.BindGrid:" + def.Name, ex);
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
            ltMessage.Text = AgHelper.Enc(text);
            pnlMessage.Visible = true;
        }

        private void ShowError(string text)
        {
            ltError.Text = AgHelper.Enc(text);
            pnlError.Visible = true;
        }

        /// <summary>
        /// Allowed for site collection administrators, users with ManageLists on the
        /// current web, or users listed in the AdminUsers list.
        /// </summary>
        private bool IsAdmin()
        {
            try
            {
                SPWeb web = SPContext.Current.Web;
                if (web.CurrentUser == null) return false;
                if (web.CurrentUser.IsSiteAdmin) return true;
                if (web.DoesUserHavePermissions(SPBasePermissions.ManageLists)) return true;

                SPList adminList = web.Lists.TryGetList(AdminUsersList);
                if (adminList == null && web.Site.RootWeb != null)
                    adminList = web.Site.RootWeb.Lists.TryGetList(AdminUsersList);
                if (adminList == null) return false;

                string login = web.CurrentUser.LoginName;
                var q = new SPQuery
                {
                    Query = "<Where><Eq><FieldRef Name='UserLogin'/>"
                          + "<Value Type='Text'>" + System.Security.SecurityElement.Escape(login) + "</Value></Eq></Where>",
                    RowLimit = 1
                };
                return adminList.GetItems(q).Count > 0;
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgAdmin.IsAdmin", ex);
                return false;
            }
        }
    }

    /// <summary>Row DTO for the admin grid - all values already HTML-encoded.</summary>
    public class AgAdminRow
    {
        public int Id { get; set; }
        public List<string> Cells { get; set; }
        public string EditText { get; set; }
        public string DeleteText { get; set; }
        public string DeleteConfirm { get; set; }

        public AgAdminRow() { Cells = new List<string>(); }
    }
}
