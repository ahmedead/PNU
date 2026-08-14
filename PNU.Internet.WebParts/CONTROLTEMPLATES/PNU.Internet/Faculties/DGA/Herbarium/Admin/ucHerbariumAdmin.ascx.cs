using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbariumAdmin : UserControl
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

                HerbariumListProvisioner.EnsureAllListsExist();

                FillListPicker();
                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbariumAdmin.OnInit", ex);
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
                HerbariumLog.Write("ucHerbariumAdmin.Page_Load", ex);
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
                        if (!string.IsNullOrEmpty(posted) && HerbariumListSchema.All.ContainsKey(posted))
                            return posted;
                    }
                }
                catch { }

                return CurrentListName;
            }
        }

        private void SetTitles()
        {
            ltPageTitle.Text = HerbariumHelper.GetRes("HerbariumAdmin_Title", "إدارة المعشبة النباتية", "Herbarium Administration");
            ltDenied.Text = HerbariumHelper.GetRes("HerbariumAdmin_Denied", "عفوًا، ليس لديك صلاحية الوصول لهذه الصفحة.", "Access Denied.");
            lblListLabel.Text = HerbariumHelper.GetRes("HerbariumAdmin_SelectList", "اختر القائمة", "Select List");
            ltNew.Text = HerbariumHelper.GetRes("HerbariumAdmin_NewItem", "إضافة عنصر جديد", "Add New Item");
            btnCancel.Text = HerbariumHelper.GetRes("HerbariumAdmin_Cancel", "إلغاء", "Cancel");
            btnSave.Text = HerbariumHelper.GetRes("HerbariumAdmin_Save", "حفظ", "Save");
        }

        private string CurrentListName
        {
            get
            {
                if (!string.IsNullOrEmpty(FixedListName) && HerbariumListSchema.All.ContainsKey(FixedListName))
                    return FixedListName;

                if (ddlList != null && ddlList.SelectedItem != null && !string.IsNullOrEmpty(ddlList.SelectedValue))
                    return ddlList.SelectedValue;

                return HerbariumListNames.HerbariumHeader;
            }
        }

        private HerbariumListDef CurrentSchema
        {
            get
            {
                HerbariumListDef schema;
                return HerbariumListSchema.All.TryGetValue(CurrentListName, out schema) ? schema : null;
            }
        }

        private void FillListPicker()
        {
            if (ddlList == null) return;

            if (!string.IsNullOrEmpty(FixedListName))
            {
                ddlList.Visible = false;
                if (lblListLabel != null) lblListLabel.Visible = false;
                return;
            }

            if (ddlList.Items.Count > 0) return;

            foreach (var kvp in HerbariumListSchema.All)
            {
                ddlList.Items.Add(new ListItem(kvp.Value.Display, kvp.Key));
            }
        }

        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideMessages();
                pnlForm.Visible = false;
                BuildForm(CurrentListName);
                BindGrid();
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbariumAdmin.ddlList_SelectedIndexChanged", ex);
                ShowError(ex.Message);
            }
        }

        private void BuildForm(string listName)
        {
            if (pnlDynamicFields == null) return;
            pnlDynamicFields.Controls.Clear();
            _inputs.Clear();
            _builtListName = listName;

            HerbariumListDef schema;
            if (!HerbariumListSchema.All.TryGetValue(listName, out schema) || schema == null) return;

            // Always add Title field (built-in)
            AddFieldControl(new HerbariumFieldDef("Title", "العنوان بالعربية", "Title (Arabic)", SPFieldType.Text, true));

            foreach (var field in schema.Fields)
            {
                if (field.InternalName.Equals("Title", StringComparison.OrdinalIgnoreCase)) continue;
                AddFieldControl(field);
            }
        }

        private void AddFieldControl(HerbariumFieldDef field)
        {
            Panel wrapper = new Panel { CssClass = field.Rows > 1 || field.Type == SPFieldType.Note ? "col-12" : "col-12 col-md-6" };
            Label label = new Label { CssClass = "form-label fw-bold", Text = HerbariumHelper.Enc(field.Display) };
            wrapper.Controls.Add(label);

            Control inputCtrl = null;

            if (field.Type == SPFieldType.Choice && field.Choices != null && field.Choices.Length > 0)
            {
                DropDownList ddl = new DropDownList { ID = "inp_" + field.InternalName, CssClass = "form-select" };
                foreach (var c in field.Choices) ddl.Items.Add(new ListItem(c, c));
                inputCtrl = ddl;
            }
            else if (field.Type == SPFieldType.Note || field.Rows > 1)
            {
                TextBox tb = new TextBox
                {
                    ID = "inp_" + field.InternalName,
                    CssClass = "form-control",
                    TextMode = TextBoxMode.MultiLine,
                    Rows = field.Rows > 0 ? field.Rows : 3
                };
                inputCtrl = tb;
            }
            else
            {
                TextBox tb = new TextBox { ID = "inp_" + field.InternalName, CssClass = "form-control" };
                inputCtrl = tb;
            }

            if (inputCtrl != null)
            {
                wrapper.Controls.Add(inputCtrl);
                pnlDynamicFields.Controls.Add(wrapper);
                _inputs[field.InternalName] = inputCtrl;
            }
        }

        private void RestoreFormState()
        {
            if (pnlForm == null || !pnlForm.Visible) return;
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessages();
                hfMode.Value = ModeNew;
                hfItemId.Value = string.Empty;
                ltFormTitle.Text = HerbariumHelper.GetRes("HerbariumAdmin_AddHeading", "إضافة عنصر جديد", "Add New Item");
                ClearInputs();
                pnlForm.Visible = true;
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbariumAdmin.btnNew_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            HideMessages();
            pnlForm.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessages();
                HerbariumListDef schema = CurrentSchema;
                if (schema == null) return;

                string title = GetInputValue("Title");
                if (string.IsNullOrEmpty(title))
                {
                    ShowError(HerbariumHelper.GetRes("HerbariumAdmin_TitleRequired", "العنوان مطلوب.", "Title is required."));
                    return;
                }

                var values = new Dictionary<string, string>();
                values["Title"] = title;
                foreach (var fld in schema.Fields)
                {
                    if (fld.InternalName.Equals("Title", StringComparison.OrdinalIgnoreCase)) continue;
                    values[fld.InternalName] = GetInputValue(fld.InternalName);
                }

                string mode = hfMode.Value;
                int id = 0;
                int.TryParse(hfItemId.Value, out id);

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    if (SPContext.Current == null) return;
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = HerbariumTargetWeb.Open(site))
                    {
                        if (web == null) return;
                        SPList list = web.Lists.TryGetList(schema.Name);
                        if (list == null) return;

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem item;
                            if (mode == ModeEdit && id > 0)
                            {
                                item = list.GetItemById(id);
                            }
                            else
                            {
                                item = list.Items.Add();
                            }

                            item["Title"] = values["Title"];

                            foreach (var fld in schema.Fields)
                            {
                                if (fld.InternalName.Equals("Title", StringComparison.OrdinalIgnoreCase)) continue;
                                string val = values.ContainsKey(fld.InternalName) ? values[fld.InternalName] : string.Empty;

                                if (fld.Type == SPFieldType.Number)
                                {
                                    int num;
                                    if (int.TryParse(val, out num)) item[fld.InternalName] = num;
                                    else item[fld.InternalName] = null;
                                }
                                else if (fld.Type == SPFieldType.URL)
                                {
                                    item[fld.InternalName] = new SPFieldUrlValue { Url = val, Description = val };
                                }
                                else
                                {
                                    item[fld.InternalName] = val;
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

                pnlForm.Visible = false;
                ShowSuccess(HerbariumHelper.GetRes("HerbariumAdmin_SaveSuccess", "تم حفظ البيانات بنجاح.", "Data saved successfully."));
                BindGrid();
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbariumAdmin.btnSave_Click", ex);
                ShowError(ex.Message);
            }
        }

        private string GetInputValue(string fieldName)
        {
            Control ctrl;
            if (!_inputs.TryGetValue(fieldName, out ctrl) || ctrl == null) return string.Empty;

            if (ctrl is TextBox) return ((TextBox)ctrl).Text.Trim();
            if (ctrl is DropDownList) return ((DropDownList)ctrl).SelectedValue;
            return string.Empty;
        }

        private void SetInputValue(string fieldName, string val)
        {
            Control ctrl;
            if (!_inputs.TryGetValue(fieldName, out ctrl) || ctrl == null) return;

            if (ctrl is TextBox) ((TextBox)ctrl).Text = val ?? string.Empty;
            else if (ctrl is DropDownList)
            {
                DropDownList ddl = (DropDownList)ctrl;
                ListItem li = ddl.Items.FindByValue(val);
                if (li != null)
                {
                    ddl.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        private void ClearInputs()
        {
            foreach (var kvp in _inputs)
            {
                if (kvp.Value is TextBox) ((TextBox)kvp.Value).Text = string.Empty;
                else if (kvp.Value is DropDownList) ((DropDownList)kvp.Value).SelectedIndex = 0;
            }
        }

        private void BindGrid()
        {
            if (gvItems == null) return;

            HerbariumListDef schema = CurrentSchema;
            if (schema == null) return;

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Title", typeof(string));

            foreach (var fld in schema.Fields)
            {
                if (fld.InGrid && !fld.InternalName.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    if (!dt.Columns.Contains(fld.InternalName))
                    {
                        dt.Columns.Add(fld.InternalName, typeof(string));
                    }
                }
            }

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    if (SPContext.Current == null) return;
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = HerbariumTargetWeb.Open(site))
                    {
                        if (web == null) return;
                        List<SPListItem> items = HerbariumHelper.GetItems(web, schema.Name);

                        foreach (SPListItem item in items)
                        {
                            DataRow dr = dt.NewRow();
                            dr["ID"] = item.ID;
                            dr["Title"] = HerbariumHelper.SafeString(item, "Title");

                            foreach (var fld in schema.Fields)
                            {
                                if (fld.InGrid && !fld.InternalName.Equals("Title", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (fld.Type == SPFieldType.URL)
                                        dr[fld.InternalName] = HerbariumHelper.SafeUrl(item, fld.InternalName);
                                    else
                                        dr[fld.InternalName] = HerbariumHelper.SafeString(item, fld.InternalName);
                                }
                            }

                            dt.Rows.Add(dr);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbariumAdmin.BindGrid", ex);
            }

            gvItems.Columns.Clear();

            // ID column
            BoundField bfId = new BoundField { DataField = "ID", HeaderText = "#" };
            gvItems.Columns.Add(bfId);

            // Title column
            BoundField bfTitle = new BoundField { DataField = "Title", HeaderText = HerbariumHelper.GetRes("HerbariumAdmin_ColTitle", "العنوان", "Title") };
            gvItems.Columns.Add(bfTitle);

            // Grid columns from schema
            foreach (var fld in schema.Fields)
            {
                if (fld.InGrid && !fld.InternalName.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    BoundField bf = new BoundField
                    {
                        DataField = fld.InternalName,
                        HeaderText = HerbariumHelper.Enc(fld.Display)
                    };
                    gvItems.Columns.Add(bf);
                }
            }

            // Actions column (Edit / Delete)
            TemplateField tfActions = new TemplateField { HeaderText = HerbariumHelper.GetRes("HerbariumAdmin_ColActions", "إجراءات", "Actions") };
            gvItems.Columns.Add(tfActions);

            gvItems.DataSource = dt;
            gvItems.DataBind();
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv == null) return;

                int id = Convert.ToInt32(drv["ID"]);
                int actionsCellIndex = e.Row.Cells.Count - 1;
                TableCell cell = e.Row.Cells[actionsCellIndex];

                LinkButton btnEdit = new LinkButton
                {
                    CommandName = "EditItem",
                    CommandArgument = id.ToString(),
                    CssClass = "btn btn-sm btn-outline-primary me-1",
                    Text = "<i class='hgi hgi-stroke hgi-pencil-edit-01'></i> " + HerbariumHelper.GetRes("HerbariumAdmin_Edit", "تعديل", "Edit")
                };

                LinkButton btnDelete = new LinkButton
                {
                    CommandName = "DeleteItem",
                    CommandArgument = id.ToString(),
                    CssClass = "btn btn-sm btn-outline-danger",
                    Text = "<i class='hgi hgi-stroke hgi-delete-02'></i> " + HerbariumHelper.GetRes("HerbariumAdmin_Delete", "حذف", "Delete"),
                    OnClientClick = "return confirm('" + HerbariumHelper.GetRes("HerbariumAdmin_ConfirmDelete", "هل أنت تأكد من حذف هذا العنصر؟", "Are you sure you want to delete this item?") + "');"
                };

                cell.Controls.Add(btnEdit);
                cell.Controls.Add(btnDelete);
            }
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                HideMessages();
                int id;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;

                HerbariumListDef schema = CurrentSchema;
                if (schema == null) return;

                if (e.CommandName == "EditItem")
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        if (SPContext.Current == null) return;
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb web = HerbariumTargetWeb.Open(site))
                        {
                            if (web == null) return;
                            SPList list = web.Lists.TryGetList(schema.Name);
                            if (list == null) return;
                            SPListItem item = list.GetItemById(id);
                            if (item == null) return;

                            hfMode.Value = ModeEdit;
                            hfItemId.Value = id.ToString(CultureInfo.InvariantCulture);
                            ltFormTitle.Text = HerbariumHelper.GetRes("HerbariumAdmin_EditHeading", "تعديل العنصر", "Edit Item") + " #" + id;

                            SetInputValue("Title", HerbariumHelper.SafeString(item, "Title"));

                            foreach (var fld in schema.Fields)
                            {
                                if (fld.InternalName.Equals("Title", StringComparison.OrdinalIgnoreCase)) continue;
                                if (fld.Type == SPFieldType.URL)
                                    SetInputValue(fld.InternalName, HerbariumHelper.SafeUrl(item, fld.InternalName));
                                else
                                    SetInputValue(fld.InternalName, HerbariumHelper.SafeString(item, fld.InternalName));
                            }

                            pnlForm.Visible = true;
                        }
                    });
                }
                else if (e.CommandName == "DeleteItem")
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        if (SPContext.Current == null) return;
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb web = HerbariumTargetWeb.Open(site))
                        {
                            if (web == null) return;
                            SPList list = web.Lists.TryGetList(schema.Name);
                            if (list == null) return;
                            SPListItem item = list.GetItemById(id);
                            if (item == null) return;

                            bool prevUnsafe = web.AllowUnsafeUpdates;
                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                item.Delete();
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = prevUnsafe;
                            }
                        }
                    });

                    ShowSuccess(HerbariumHelper.GetRes("HerbariumAdmin_DeleteSuccess", "تم حذف العنصر بنجاح.", "Item deleted successfully."));
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbariumAdmin.gvItems_RowCommand", ex);
                ShowError(ex.Message);
            }
        }

        private bool IsAdmin()
        {
            try
            {
                if (SPContext.Current == null || SPContext.Current.Web == null) return true;
                if (SPContext.Current.Web.CurrentUser == null) return false;
                if (SPContext.Current.Web.CurrentUser.IsSiteAdmin) return true;

                using (SPWeb adminWeb = HerbariumTargetWeb.OpenAdminWeb(SPContext.Current.Site))
                {
                    if (adminWeb == null) return false;
                    SPList adminList = adminWeb.Lists.TryGetList(AdminUsersList);
                    if (adminList == null) return false;

                    string currentAccount = SPContext.Current.Web.CurrentUser.LoginName;
                    foreach (SPListItem item in adminList.Items)
                    {
                        string userVal = Convert.ToString(item["Title"]) ?? string.Empty;
                        if (userVal.Equals(currentAccount, StringComparison.OrdinalIgnoreCase)) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbariumAdmin.IsAdmin", ex);
            }

            return false;
        }

        private void ShowSuccess(string msg)
        {
            pnlMessage.Visible = true;
            ltMessage.Text = HerbariumHelper.Enc(msg);
        }

        private void ShowError(string msg)
        {
            pnlError.Visible = true;
            ltError.Text = HerbariumHelper.Enc(msg);
        }

        private void HideMessages()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
        }
    }
}
