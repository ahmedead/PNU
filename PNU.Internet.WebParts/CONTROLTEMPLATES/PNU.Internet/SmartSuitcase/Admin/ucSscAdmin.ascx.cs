using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public partial class ucSscAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";
        private const string AdminUsersList = "AdminUsers";

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string FixedListName { get; set; }

        private readonly Dictionary<string, Control> _inputs = new Dictionary<string, Control>();
        private bool _isAdmin;
        private string _builtListName;

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

                SmartSuitcaseListProvisioner.EnsureAllListsExist();
                FillListPicker();

                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSscAdmin.OnInit", ex);
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
                SscLog.Write("ucSscAdmin.Page_Load", ex);
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
                        if (!string.IsNullOrEmpty(posted) && SscListSchema.Get(posted) != null)
                            return posted;
                    }
                }
                catch { }

                return CurrentListName;
            }
        }

        private void SetTitles()
        {
            ltPageTitle.Text = SscHelper.Enc(SscHelper.Pick("إدارة محتوى الحقيبة الذكية", "Smart Suitcase Content Management"));
            lblListLabel.Text = SscHelper.Enc(SscHelper.Pick("القسم", "Section"));
            ltNew.Text = SscHelper.Enc(SscHelper.Pick(" إضافة عنصر", " Add Item"));
            ltActions.Text = SscHelper.Enc(SscHelper.Pick("إجراءات", "Actions"));
            ltEmpty.Text = SscHelper.Enc(SscHelper.Pick("لا توجد عناصر في هذا القسم بعد.", "No items in this section yet."));
            ltDenied.Text = SscHelper.Enc(SscHelper.Pick("ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            btnSave.Text = SscHelper.Enc(SscHelper.Pick("حفظ", "Save"));
            btnCancel.Text = SscHelper.Enc(SscHelper.Pick("إلغاء", "Cancel"));
        }

        private void FillListPicker()
        {
            ddlList.Items.Clear();

            if (!string.IsNullOrEmpty(FixedListName))
            {
                SscListDef only = SscListSchema.Get(FixedListName);
                if (only != null)
                {
                    ddlList.Items.Add(new ListItem(only.Display, only.Name));
                    ddlList.Visible = false;
                    lblListLabel.Visible = false;
                }
                return;
            }

            foreach (string name in SscListNames.AllLists)
            {
                SscListDef def = SscListSchema.Get(name);
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

        private SscListDef CurrentDef { get { return SscListSchema.Get(CurrentListName); } }

        private void BuildForm(string listName)
        {
            phForm.Controls.Clear();
            phForm.EnableViewState = false;
            _inputs.Clear();
            _builtListName = listName;

            SscListDef def = SscListSchema.Get(listName);
            if (def == null) return;

            AddInput("Title", SscHelper.Pick("العنوان (عربي)", "Title (AR)"), SPFieldType.Text, 0, null);

            foreach (SscFieldDef f in def.Fields)
                AddInput(f.InternalName, f.Display, f.Type, f.Rows, f.Hint);
        }

        private void AddInput(string internalName, string label, SPFieldType type, int rows, string hint)
        {
            bool wide = type == SPFieldType.Note;

            var col = new Panel { CssClass = wide ? "col-12" : "col-12 col-md-6", EnableViewState = false };

            var lbl = new Label
            {
                CssClass = "form-label",
                Text = SscHelper.Enc(label),
                AssociatedControlID = "fld_" + internalName
            };

            var box = new TextBox
            {
                ID = "fld_" + internalName,
                CssClass = "form-control",
                ClientIDMode = ClientIDMode.AutoID,
                EnableViewState = false
            };

            if (type == SPFieldType.Note)
            {
                box.TextMode = TextBoxMode.MultiLine;
                box.Rows = rows > 0 ? rows : 4;
            }

            col.Controls.Add(lbl);
            col.Controls.Add(box);

            if (!string.IsNullOrEmpty(hint))
            {
                col.Controls.Add(new Literal
                {
                    Text = "<div class=\"form-text\">" + SscHelper.Enc(hint) + "</div>"
                });
            }

            phForm.Controls.Add(col);
            _inputs[internalName] = box;
        }

        private TextBox Box(string internalName)
        {
            Control c;
            return _inputs.TryGetValue(internalName, out c) ? c as TextBox : null;
        }

        private void RestoreFormState()
        {
            string mode = hfMode.Value;
            pnlForm.Visible = mode == ModeNew || mode == ModeEdit;

            ltFormTitle.Text = SscHelper.Enc(mode == ModeEdit
                ? SscHelper.Pick("تعديل عنصر", "Edit Item")
                : SscHelper.Pick("إضافة عنصر", "Add Item"));
        }

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

        private void LoadItemIntoForm(int id)
        {
            try
            {
                SscListDef def = CurrentDef;
                if (def == null) return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = SscTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = list.GetItemById(id);

                        SetBox("Title", SscHelper.SafeString(item, "Title"));

                        foreach (SscFieldDef f in def.Fields)
                        {
                            string value = f.Type == SPFieldType.URL
                                ? SscHelper.SafeUrl(item, f.InternalName)
                                : SscHelper.SafeString(item, f.InternalName);
                            SetBox(f.InternalName, value);
                        }
                    }
                });

                hfMode.Value = ModeEdit;
                hfItemId.Value = id.ToString(CultureInfo.InvariantCulture);
                RestoreFormState();
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSscAdmin.LoadItemIntoForm", ex);
                ShowError(ex.Message);
            }
        }

        private void SaveItem()
        {
            SscListDef def = CurrentDef;
            if (def == null) return;

            string title = GetBoxValue("Title");
            if (string.IsNullOrEmpty(title))
            {
                ShowError(SscHelper.Pick("العنوان مطلوب.", "Title is required."));
                RestoreFormState();
                return;
            }

            int id;
            int.TryParse(hfItemId.Value, out id);
            bool isNew = hfMode.Value != ModeEdit || id <= 0;

            var values = new Dictionary<string, string>();
            foreach (SscFieldDef f in def.Fields)
                values[f.InternalName] = GetBoxValue(f.InternalName);

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = SscTargetWeb.Open(site))
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

                            foreach (SscFieldDef f in def.Fields)
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
                    ? SscHelper.Pick("تمت إضافة العنصر بنجاح.", "Item added successfully.")
                    : SscHelper.Pick("تم تحديث العنصر بنجاح.", "Item updated successfully."));
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSscAdmin.SaveItem", ex);
                ShowError(ex.Message);
                RestoreFormState();
            }
        }

        private void DeleteItem(int id)
        {
            SscListDef def = CurrentDef;
            if (def == null) return;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = SscTargetWeb.Open(site))
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
                ShowMessage(SscHelper.Pick("تم حذف العنصر.", "Item deleted."));
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSscAdmin.DeleteItem", ex);
                ShowError(ex.Message);
            }
        }

        private void BindGrid()
        {
            SscListDef def = CurrentDef;
            if (def == null) return;

            var headers = new List<string>
            {
                SscHelper.Enc(SscHelper.Pick("العنوان (عربي)", "Title (AR)"))
            };
            var gridFields = def.Fields.Where(f => f.InGrid).ToList();
            foreach (SscFieldDef f in gridFields) headers.Add(SscHelper.Enc(f.Display));

            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            string editText = SscHelper.Enc(SscHelper.Pick("تعديل", "Edit"));
            string deleteText = SscHelper.Enc(SscHelper.Pick("حذف", "Delete"));
            string confirmText = SscHelper.Pick("هل أنت متأكد من حذف هذا العنصر؟", "Are you sure you want to delete this item?")
                .Replace("'", "\\'");

            var rows = new List<AdminRow>();

            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = SscTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (SPListItem item in SscHelper.GetItems(web, def.Name))
                    {
                        var row = new AdminRow
                        {
                            Id = item.ID,
                            EditText = editText,
                            DeleteText = deleteText,
                            DeleteConfirm = "return confirm('" + confirmText + "');"
                        };

                        row.Cells.Add(SscHelper.Enc(SscHelper.SafeString(item, "Title")));
                        foreach (SscFieldDef f in gridFields)
                        {
                            string value = f.Type == SPFieldType.URL
                                ? SscHelper.SafeUrl(item, f.InternalName)
                                : SscHelper.SafeString(item, f.InternalName);
                            row.Cells.Add(SscHelper.Enc(value));
                        }

                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSscAdmin.BindGrid:" + def.Name, ex);
            }

            rptItems.DataSource = rows;
            rptItems.DataBind();
            pnlEmpty.Visible = rows.Count == 0;
        }

        private void SetBox(string internalName, string value)
        {
            TextBox box = Box(internalName);
            if (box != null) box.Text = value ?? string.Empty;
        }

        private string GetBoxValue(string internalName)
        {
            TextBox box = Box(internalName);
            return box == null ? string.Empty : (box.Text ?? string.Empty).Trim();
        }

        private void ClearForm()
        {
            foreach (Control c in _inputs.Values)
            {
                TextBox box = c as TextBox;
                if (box != null) box.Text = string.Empty;
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
            ltMessage.Text = SscHelper.Enc(text);
            pnlMessage.Visible = true;
        }

        private void ShowError(string text)
        {
            ltError.Text = SscHelper.Enc(text);
            pnlError.Visible = true;
        }

        private bool IsAdmin()
        {
            return ContentAdm.IsAdmin();
        }
    }
}
