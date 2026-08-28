using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";

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

                ClListProvisioner.EnsureAllListsExist();
                FillListPicker();

                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAdmin.OnInit", ex);
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
                ClLog.Write("ucClAdmin.Page_Load", ex);
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
                        if (!string.IsNullOrEmpty(posted) && ClListSchema.Get(posted) != null)
                            return posted;
                    }
                }
                catch { }

                return CurrentListName;
            }
        }

        private void SetTitles()
        {
            ltPageTitle.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminTitle", "إدارة محتوى المكتبة المركزية", "Central Library Content Management"));
            lblListLabel.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminList", "القسم", "Section"));
            ltSeed.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminSeed", "إدراج البيانات الافتراضية", "Seed Default Data"));
            ltNew.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminNew", " إضافة عنصر", " Add item"));
            ltActions.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminActions", "إجراءات", "Actions"));
            ltEmpty.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminEmpty", "لا توجد عناصر في هذا القسم بعد.", "No items in this section yet."));
            ltDenied.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminDenied", "ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            btnSave.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminSave", "حفظ", "Save"));
            btnCancel.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminCancel", "إلغاء", "Cancel"));
        }

        private void FillListPicker()
        {
            ddlList.Items.Clear();

            if (!string.IsNullOrEmpty(FixedListName))
            {
                ClListDef only = ClListSchema.Get(FixedListName);
                if (only != null)
                {
                    ddlList.Items.Add(new ListItem(only.Display, only.Name));
                    ddlList.Visible = false;
                    lblListLabel.Visible = false;
                }
                return;
            }

            foreach (string name in ClListNames.All)
            {
                ClListDef def = ClListSchema.Get(name);
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

        private ClListDef CurrentDef { get { return ClListSchema.Get(CurrentListName); } }

        private bool IsAdmin()
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    return SPContext.Current.Web.CurrentUser.IsSiteAdmin ||
                           SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.ManageWeb) ||
                           SPContext.Current.Web.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                }
            }
            catch { }
            return true;
        }

        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();
                CloseForm();
                BuildForm(CurrentListName);
                BindGrid();
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAdmin.ddlList_SelectedIndexChanged", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnSeed_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();

                string listName = CurrentListName;
                if (string.IsNullOrEmpty(listName))
                {
                    ShowError(ClHelper.GetRes("Cl_AdminNoList", "الرجاء اختيار قسم أولاً.", "Please select a section first."));
                    return;
                }

                bool success = ClListProvisioner.ForceSeedDefaultData(listName);
                if (success)
                {
                    ShowMessage(ClHelper.GetRes("Cl_AdminSeeded", "تم إدراج البيانات الافتراضية بنجاح.", "Default data seeded successfully."));
                    BindGrid();
                }
                else
                {
                    ShowError(ClHelper.GetRes("Cl_AdminSeedFailed", "فشل إدراج البيانات الافتراضية.", "Failed to seed default data."));
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAdmin.btnSeed_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();
                BuildForm(CurrentListName);
                OpenFormNew();
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAdmin.btnNew_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            HideMessage();
            HideError();
            CloseForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();

                ClListDef def = CurrentDef;
                if (def == null) return;

                bool isEdit = string.Equals(hfMode.Value, ModeEdit, StringComparison.OrdinalIgnoreCase);
                int itemId = 0;
                int.TryParse(hfItemId.Value, out itemId);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = ClTargetWeb.Open(site))
                {
                    if (web == null) return;
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.Lists.TryGetList(def.Name);
                    if (list == null) return;

                    SPListItem item = isEdit ? list.GetItemById(itemId) : list.AddItem();

                    Control txtTitle = FindInput("Title");
                    if (txtTitle is TextBox) item["Title"] = ((TextBox)txtTitle).Text.Trim();

                    foreach (ClFieldDef f in def.Fields)
                    {
                        Control ctrl = FindInput(f.InternalName);
                        if (ctrl == null) continue;

                        if (ctrl is TextBox)
                        {
                            string val = ((TextBox)ctrl).Text.Trim();
                            if (f.Type == SPFieldType.Number)
                            {
                                int intVal;
                                item[f.InternalName] = int.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out intVal) ? intVal : 0;
                            }
                            else if (f.Type == SPFieldType.URL)
                            {
                                item[f.InternalName] = new SPFieldUrlValue(val);
                            }
                            else
                            {
                                item[f.InternalName] = val;
                            }
                        }
                        else if (ctrl is DropDownList)
                        {
                            item[f.InternalName] = ((DropDownList)ctrl).SelectedValue;
                        }
                    }

                    item.Update();
                    web.AllowUnsafeUpdates = false;
                }

                ShowMessage(ClHelper.GetRes("Cl_AdminSaved", "تم حفظ البيانات بنجاح.", "Data saved successfully."));
                CloseForm();
                BindGrid();
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAdmin.btnSave_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                HideMessage();
                HideError();

                int itemId = Convert.ToInt32(e.CommandArgument);
                ClListDef def = CurrentDef;
                if (def == null) return;

                if (e.CommandName == "EditItem")
                {
                    BuildForm(def.Name);
                    OpenFormEdit(itemId);
                }
                else if (e.CommandName == "DeleteItem")
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = ClTargetWeb.Open(site))
                    {
                        if (web == null) return;
                        web.AllowUnsafeUpdates = true;
                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list != null)
                        {
                            SPListItem item = list.GetItemById(itemId);
                            item.Delete();
                        }
                        web.AllowUnsafeUpdates = false;
                    }

                    ShowMessage(ClHelper.GetRes("Cl_AdminDeleted", "تم حذف العنصر بنجاح.", "Item deleted successfully."));
                    CloseForm();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAdmin.rptItems_ItemCommand", ex);
                ShowError(ex.Message);
            }
        }

        private void BuildForm(string listName)
        {
            phForm.Controls.Clear();
            _inputs.Clear();
            _builtListName = listName;

            ClListDef def = ClListSchema.Get(listName);
            if (def == null) return;

            AddFormField(new ClFieldDef("Title", "العنوان (عربي)", "Title (AR)", SPFieldType.Text, true));

            foreach (ClFieldDef f in def.Fields)
            {
                AddFormField(f);
            }
        }

        private void AddFormField(ClFieldDef f)
        {
            var col = new Panel { CssClass = f.Rows > 1 ? "col-12" : "col-12 col-md-6" };

            var label = new Label
            {
                Text = ClHelper.Enc(f.Display),
                CssClass = "form-label",
                ID = "lbl_" + f.InternalName
            };
            col.Controls.Add(label);

            Control input;
            if (f.Choices != null && f.Choices.Length > 0)
            {
                var ddl = new DropDownList { ID = "inp_" + f.InternalName, CssClass = "form-select" };
                foreach (string choice in f.Choices)
                {
                    ddl.Items.Add(new ListItem(choice, choice));
                }
                input = ddl;
            }
            else
            {
                var txt = new TextBox { ID = "inp_" + f.InternalName, CssClass = "form-control" };
                if (f.Rows > 1)
                {
                    txt.TextMode = TextBoxMode.MultiLine;
                    txt.Rows = f.Rows;
                }
                input = txt;
            }

            label.AssociatedControlID = input.ID;
            col.Controls.Add(input);

            if (!string.IsNullOrEmpty(f.Hint))
            {
                col.Controls.Add(new Literal { Text = "<div class=\"form-text\">" + ClHelper.Enc(f.Hint) + "</div>" });
            }

            phForm.Controls.Add(col);
            _inputs[f.InternalName] = input;
        }

        private Control FindInput(string internalName)
        {
            Control input;
            if (_inputs.TryGetValue(internalName, out input)) return input;

            return phForm.FindControl("inp_" + internalName);
        }

        private void OpenFormNew()
        {
            hfMode.Value = ModeNew;
            hfItemId.Value = "0";
            ltFormTitle.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminNewItem", "إضافة عنصر جديد", "Add new item"));
            pnlForm.Visible = true;
        }

        private void OpenFormEdit(int itemId)
        {
            ClListDef def = CurrentDef;
            if (def == null) return;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = ClTargetWeb.Open(site))
            {
                if (web == null) return;
                SPList list = web.Lists.TryGetList(def.Name);
                if (list == null) return;

                SPListItem item = list.GetItemById(itemId);

                Control txtTitle = FindInput("Title");
                if (txtTitle is TextBox) ((TextBox)txtTitle).Text = ClHelper.SafeString(item, "Title");

                foreach (ClFieldDef f in def.Fields)
                {
                    Control ctrl = FindInput(f.InternalName);
                    if (ctrl is TextBox)
                    {
                        ((TextBox)ctrl).Text = f.Type == SPFieldType.URL ? ClHelper.SafeUrl(item, f.InternalName) : ClHelper.SafeString(item, f.InternalName);
                    }
                    else if (ctrl is DropDownList)
                    {
                        string val = ClHelper.SafeString(item, f.InternalName);
                        DropDownList ddl = (DropDownList)ctrl;
                        if (ddl.Items.FindByValue(val) != null) ddl.SelectedValue = val;
                    }
                }
            }

            hfMode.Value = ModeEdit;
            hfItemId.Value = itemId.ToString(CultureInfo.InvariantCulture);
            ltFormTitle.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminEditItem", "تعديل العنصر", "Edit item"));
            pnlForm.Visible = true;
        }

        private void CloseForm()
        {
            pnlForm.Visible = false;
            hfMode.Value = string.Empty;
            hfItemId.Value = "0";
        }

        private void RestoreFormState()
        {
            if (string.Equals(hfMode.Value, ModeEdit, StringComparison.OrdinalIgnoreCase))
            {
                int itemId;
                if (int.TryParse(hfItemId.Value, out itemId) && itemId > 0)
                    ltFormTitle.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminEditItem", "تعديل العنصر", "Edit item"));
            }
            else if (string.Equals(hfMode.Value, ModeNew, StringComparison.OrdinalIgnoreCase))
            {
                ltFormTitle.Text = ClHelper.Enc(ClHelper.GetRes("Cl_AdminNewItem", "إضافة عنصر جديد", "Add new item"));
            }
        }

        private void BindGrid()
        {
            ClListDef def = CurrentDef;
            if (def == null) return;

            List<ClFieldDef> gridFields = def.Fields.Where(f => f.InGrid).ToList();

            var headers = new List<string> { ClHelper.Pick("العنوان", "Title") };
            headers.AddRange(gridFields.Select(f => f.Display));
            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = ClTargetWeb.Open(site))
            {
                if (web == null) return;

                List<SPListItem> items = ClHelper.GetItems(web, def.Name);
                pnlEmpty.Visible = (items.Count == 0);

                string editText = ClHelper.Enc(ClHelper.GetRes("Cl_AdminEdit", "تعديل", "Edit"));
                string deleteText = ClHelper.Enc(ClHelper.GetRes("Cl_AdminDelete", "حذف", "Delete"));
                string confirmText = "return confirm('" + ClHelper.GetRes("Cl_AdminConfirm", "هل أنت تأكد من الحذف؟", "Are you sure you want to delete this item?") + "');";

                var gridRows = items.Select(item => new
                {
                    Id = item.ID,
                    Cells = BuildGridCells(item, gridFields),
                    EditText = editText,
                    DeleteText = deleteText,
                    DeleteConfirm = confirmText
                }).ToList();

                rptItems.DataSource = gridRows;
                rptItems.DataBind();
            }
        }

        private List<string> BuildGridCells(SPListItem item, List<ClFieldDef> fields)
        {
            var cells = new List<string>
            {
                ClHelper.Enc(ClHelper.SafeString(item, "Title"))
            };

            foreach (ClFieldDef f in fields)
            {
                string val = f.Type == SPFieldType.URL ? ClHelper.SafeUrl(item, f.InternalName) : ClHelper.SafeString(item, f.InternalName);
                cells.Add(ClHelper.Enc(val));
            }

            return cells;
        }

        private void ShowMessage(string msg)
        {
            ltMessage.Text = ClHelper.Enc(msg);
            pnlMessage.Visible = true;
        }

        private void HideMessage()
        {
            pnlMessage.Visible = false;
        }

        private void ShowError(string err)
        {
            ltError.Text = ClHelper.Enc(err);
            pnlError.Visible = true;
        }

        private void HideError()
        {
            pnlError.Visible = false;
        }
    }
}
