using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    /// <summary>
    /// Generic add / edit / delete control for every University President Office list.
    /// The form is generated from UpoListSchema, so adding a field to the schema
    /// provisions the column AND exposes it here automatically.
    /// </summary>
    public partial class ucUpoAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";
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

                UpoListProvisioner.EnsureAllListsExist();
                FillListPicker();

                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                UpoLog.Write("ucUpoAdmin.OnInit", ex);
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
                UpoLog.Write("ucUpoAdmin.Page_Load", ex);
                ShowError(ex.Message);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (_isAdmin) hfListBuilt.Value = _builtListName ?? string.Empty;
        }

        private string ListRenderedLastRequest
        {
            get
            {
                try
                {
                    if (Page != null && Page.IsPostBack)
                    {
                        string posted = Request.Form[hfListBuilt.UniqueID];
                        if (!string.IsNullOrEmpty(posted) && UpoListSchema.Get(posted) != null)
                            return posted;
                    }
                }
                catch { }

                return CurrentListName;
            }
        }

        // =====================================================================
        // labels
        // =====================================================================
        private void SetTitles()
        {
            ltPageTitle.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminTitle",
                "إدارة محتوى كلمة رئيسة الجامعة", "President Office content management"));
            lblListLabel.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminList", "القسم", "Section"));
            ltNew.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminNew", " إضافة عنصر", " Add item"));
            ltActions.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminActions", "إجراءات", "Actions"));
            ltEmpty.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminEmpty",
                "لا توجد عناصر في هذا القسم بعد.", "No items in this section yet."));
            ltDenied.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminDenied",
                "ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            btnSave.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminSave", "حفظ", "Save"));
            btnCancel.Text = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminCancel", "إلغاء", "Cancel"));
        }

        // =====================================================================
        // list picker
        // =====================================================================
        private void FillListPicker()
        {
            ddlList.Items.Clear();

            if (!string.IsNullOrEmpty(FixedListName))
            {
                UpoListDef only = UpoListSchema.Get(FixedListName);
                if (only != null)
                {
                    ddlList.Items.Add(new ListItem(only.Display, only.Name));
                    ddlList.Visible = false;
                    lblListLabel.Visible = false;
                }
                return;
            }

            foreach (string name in UpoListNames.AllLists)
            {
                UpoListDef def = UpoListSchema.Get(name);
                if (def != null) ddlList.Items.Add(new ListItem(def.Display, def.Name));
            }

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

        private UpoListDef CurrentDef { get { return UpoListSchema.Get(CurrentListName); } }

        // =====================================================================
        // dynamic form
        // =====================================================================
        private void BuildForm(string listName)
        {
            phForm.Controls.Clear();
            phForm.EnableViewState = false;
            _inputs.Clear();
            _builtListName = listName;

            UpoListDef def = UpoListSchema.Get(listName);
            if (def == null) return;

            AddInput("Title", UpoHelper.Pick("العنوان (عربي)", "Title (AR)"), SPFieldType.Text, 0, null, null);

            foreach (UpoFieldDef f in def.Fields)
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
                Text = UpoHelper.Enc(label),
                AssociatedControlID = "fld_" + internalName
            };
            col.Controls.Add(lbl);

            Control input;

            if (type == SPFieldType.Boolean)
            {
                var flag = new DropDownList { ID = "fld_" + internalName, CssClass = "form-select", EnableViewState = false };
                flag.Items.Add(new ListItem(UpoHelper.Pick("نعم", "Yes"), "1"));
                flag.Items.Add(new ListItem(UpoHelper.Pick("لا", "No"), "0"));
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
                hint = UpoHelper.Pick("الصيغة: 2026-06-24", "Format: 2026-06-24");

            if (!string.IsNullOrEmpty(hint))
                col.Controls.Add(new Literal { Text = "<div class=\"form-text\">" + UpoHelper.Enc(hint) + "</div>" });

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

            ltFormTitle.Text = UpoHelper.Enc(mode == ModeEdit
                ? UpoHelper.GetRes("Upo_AdminEdit", "تعديل عنصر", "Edit item")
                : UpoHelper.GetRes("Upo_AdminNew", "إضافة عنصر", "Add item"));
        }

        // =====================================================================
        // events
        // =====================================================================
        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                UpoListDef def = CurrentDef;
                if (def == null) return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = UpoTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = list.GetItemById(id);
                        SetValue("Title", UpoHelper.SafeString(item, "Title"));

                        foreach (UpoFieldDef f in def.Fields)
                        {
                            string value;

                            if (f.Type == SPFieldType.URL)
                            {
                                value = UpoHelper.SafeUrl(item, f.InternalName);
                            }
                            else if (f.Type == SPFieldType.DateTime)
                            {
                                DateTime? d = UpoHelper.SafeDate(item, f.InternalName);
                                value = d.HasValue
                                    ? d.Value.ToString(DateFormat, CultureInfo.InvariantCulture)
                                    : string.Empty;
                            }
                            else if (f.Type == SPFieldType.Boolean)
                            {
                                value = UpoHelper.SafeBool(item, f.InternalName) ? "1" : "0";
                            }
                            else
                            {
                                value = UpoHelper.SafeString(item, f.InternalName);
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
                UpoLog.Write("ucUpoAdmin.LoadItemIntoForm", ex);
                ShowError(ex.Message);
            }
        }

        private void SaveItem()
        {
            UpoListDef def = CurrentDef;
            if (def == null) return;

            string title = GetValue("Title");
            if (string.IsNullOrEmpty(title))
            {
                ShowError(UpoHelper.GetRes("Upo_AdminTitleRequired", "العنوان مطلوب.", "Title is required."));
                RestoreFormState();
                return;
            }

            int id;
            int.TryParse(hfItemId.Value, out id);
            bool isNew = hfMode.Value != ModeEdit || id <= 0;

            var values = new Dictionary<string, string>();
            foreach (UpoFieldDef f in def.Fields)
                values[f.InternalName] = GetValue(f.InternalName);

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = UpoTargetWeb.Open(site))
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

                            foreach (UpoFieldDef f in def.Fields)
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
                    ? UpoHelper.GetRes("Upo_AdminAdded", "تمت إضافة العنصر بنجاح.", "Item added successfully.")
                    : UpoHelper.GetRes("Upo_AdminUpdated", "تم تحديث العنصر بنجاح.", "Item updated successfully."));
            }
            catch (Exception ex)
            {
                UpoLog.Write("ucUpoAdmin.SaveItem", ex);
                ShowError(ex.Message);
                RestoreFormState();
            }
        }

        private void DeleteItem(int id)
        {
            UpoListDef def = CurrentDef;
            if (def == null) return;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = UpoTargetWeb.Open(site))
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
                ShowMessage(UpoHelper.GetRes("Upo_AdminDeleted", "تم حذف العنصر.", "Item deleted."));
            }
            catch (Exception ex)
            {
                UpoLog.Write("ucUpoAdmin.DeleteItem", ex);
                ShowError(ex.Message);
            }
        }

        // =====================================================================
        // grid
        // =====================================================================
        private void BindGrid()
        {
            UpoListDef def = CurrentDef;
            if (def == null) return;

            var headers = new List<string> { UpoHelper.Enc(UpoHelper.Pick("العنوان (عربي)", "Title (AR)")) };
            var gridFields = def.Fields.Where(f => f.InGrid).ToList();
            foreach (UpoFieldDef f in gridFields) headers.Add(UpoHelper.Enc(f.Display));

            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            string editText = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminEditBtn", "تعديل", "Edit"));
            string deleteText = UpoHelper.Enc(UpoHelper.GetRes("Upo_AdminDeleteBtn", "حذف", "Delete"));
            string confirmText = UpoHelper.GetRes("Upo_AdminConfirm",
                "هل أنت متأكد من حذف هذا العنصر؟", "Are you sure you want to delete this item?")
                .Replace("'", "\\'");

            var rows = new List<UpoAdminRow>();

            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = UpoTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (SPListItem item in UpoHelper.GetItems(web, def.Name))
                    {
                        var row = new UpoAdminRow
                        {
                            Id = item.ID,
                            EditText = editText,
                            DeleteText = deleteText,
                            DeleteConfirm = "return confirm('" + confirmText + "');"
                        };

                        row.Cells.Add(UpoHelper.Enc(UpoHelper.SafeString(item, "Title")));

                        foreach (UpoFieldDef f in gridFields)
                        {
                            string value;

                            if (f.Type == SPFieldType.URL)
                            {
                                value = UpoHelper.SafeUrl(item, f.InternalName);
                            }
                            else if (f.Type == SPFieldType.DateTime)
                            {
                                DateTime? d = UpoHelper.SafeDate(item, f.InternalName);
                                value = d.HasValue
                                    ? d.Value.ToString(DateFormat, CultureInfo.InvariantCulture)
                                    : string.Empty;
                            }
                            else
                            {
                                value = UpoHelper.SafeString(item, f.InternalName);
                            }

                            row.Cells.Add(UpoHelper.Enc(value));
                        }

                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                UpoLog.Write("ucUpoAdmin.BindGrid:" + def.Name, ex);
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
            ltMessage.Text = UpoHelper.Enc(text);
            pnlMessage.Visible = true;
        }

        private void ShowError(string text)
        {
            ltError.Text = UpoHelper.Enc(text);
            pnlError.Visible = true;
        }

        private bool IsAdmin()
        {
            return ContentAdm.IsAdmin();
        }
    }

    /// <summary>Row DTO for the admin grid — all values already HTML-encoded.</summary>
    public class UpoAdminRow
    {
        public int Id { get; set; }
        public List<string> Cells { get; set; }
        public string EditText { get; set; }
        public string DeleteText { get; set; }
        public string DeleteConfirm { get; set; }

        public UpoAdminRow() { Cells = new List<string>(); }
    }
}
