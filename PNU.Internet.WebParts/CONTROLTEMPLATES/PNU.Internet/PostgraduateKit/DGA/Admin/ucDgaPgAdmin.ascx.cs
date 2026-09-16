using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Admin
{
    public partial class ucDgaPgAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";

        private readonly Dictionary<string, Control> _inputs = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
        private bool _isAdmin;
        private string _builtListName;

        public class DgaPgAdminRow
        {
            public int Id { get; set; }
            public List<string> Cells { get; set; } = new List<string>();
            public string DeleteConfirm { get; set; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                _isAdmin = CheckAdminPermission();

                pnlDenied.Visible = !_isAdmin;
                pnlMain.Visible = _isAdmin;
                if (!_isAdmin) return;

                DgaPgListProvisioner.EnsureAllListsExist();
                PopulateListsDropdown();

                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("ucDgaPgAdmin.OnInit", ex);
                ShowError("حدث خطأ أثناء تهيئة لوحة التحكم: " + ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            try
            {
                RestoreFormState();
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("ucDgaPgAdmin.Page_Load", ex);
                ShowError("حدث خطأ أثناء تحميل البيانات: " + ex.Message);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (_isAdmin)
            {
                hfListBuilt.Value = _builtListName ?? string.Empty;
            }
        }

        private bool CheckAdminPermission()
        {
            try
            {
                if (SPContext.Current == null || SPContext.Current.Web == null) return false;

                SPWeb web = SPContext.Current.Web;
                if (web.CurrentUser == null) return false;
                if (web.CurrentUser.IsSiteAdmin) return true;

                return web.DoesUserHavePermissions(SPBasePermissions.ManageWeb) ||
                       web.DoesUserHavePermissions(SPBasePermissions.ManageLists) ||
                       web.DoesUserHavePermissions(SPBasePermissions.EditListItems);
            }
            catch
            {
                return false;
            }
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
                        if (!string.IsNullOrEmpty(posted) && DgaPgListSchema.Get(posted) != null)
                            return posted;
                    }
                }
                catch { }

                return CurrentListName;
            }
        }

        private string CurrentListName
        {
            get
            {
                if (ddlLists.SelectedItem != null && !string.IsNullOrEmpty(ddlLists.SelectedValue))
                    return ddlLists.SelectedValue;

                var first = DgaPgListSchema.All.FirstOrDefault();
                return first != null ? first.Name : DgaPgListNames.Sections;
            }
        }

        private DgaPgListDef CurrentDef => DgaPgListSchema.Get(CurrentListName);

        private void PopulateListsDropdown()
        {
            if (ddlLists.Items.Count > 0) return;

            ddlLists.Items.Clear();
            foreach (var listDef in DgaPgListSchema.All)
            {
                ddlLists.Items.Add(new ListItem($"{listDef.DisplayTitle} ({listDef.Name})", listDef.Name));
            }

            string posted = Request.Form[ddlLists.UniqueID];
            if (string.IsNullOrEmpty(posted)) posted = Request.QueryString["list"];

            if (!string.IsNullOrEmpty(posted) && ddlLists.Items.FindByValue(posted) != null)
                ddlLists.SelectedValue = posted;
        }

        // =====================================================================
        // Dynamic Form Builder
        // =====================================================================
        private void BuildForm(string listName)
        {
            phForm.Controls.Clear();
            phForm.EnableViewState = false;
            _inputs.Clear();
            _builtListName = listName;

            DgaPgListDef def = DgaPgListSchema.Get(listName);
            if (def == null) return;

            // 1. Primary Title (AR)
            AddTextInput("Title", "العنوان الرئيسي (عربي) *", SPFieldType.Text, 0, true, "العنوان الأساسي للعنصر");

            // 2. Schema Defined Fields
            foreach (DgaPgFieldDef f in def.Fields)
            {
                if (f.Type == SPFieldType.Choice)
                {
                    AddChoiceInput(f.Name, f.DisplayName, f.Choices, f.Required, f.DefaultValue);
                }
                else if (f.Type == SPFieldType.Boolean)
                {
                    AddBooleanInput(f.Name, f.DisplayName, f.DefaultValue);
                }
                else if (f.Type == SPFieldType.Note)
                {
                    AddTextInput(f.Name, f.DisplayName, f.Type, 4, f.Required, null);
                }
                else
                {
                    AddTextInput(f.Name, f.DisplayName, f.Type, 0, f.Required, null);
                }
            }
        }

        private void AddTextInput(string internalName, string label, SPFieldType type, int rows, bool required, string hint)
        {
            bool wide = type == SPFieldType.Note;
            var col = new Panel { CssClass = wide ? "col-12" : "col-12 col-md-6", EnableViewState = false };

            var lbl = new Label
            {
                CssClass = "form-label fw-semibold",
                Text = HttpUtility.HtmlEncode(label),
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
                    Text = "<div class=\"form-text\">" + HttpUtility.HtmlEncode(hint) + "</div>"
                });
            }

            phForm.Controls.Add(col);
            _inputs[internalName] = box;
        }

        private void AddChoiceInput(string internalName, string label, string[] choices, bool required, string defaultValue)
        {
            var col = new Panel { CssClass = "col-12 col-md-6", EnableViewState = false };

            var lbl = new Label
            {
                CssClass = "form-label fw-semibold",
                Text = HttpUtility.HtmlEncode(label),
                AssociatedControlID = "fld_" + internalName
            };

            var ddl = new DropDownList
            {
                ID = "fld_" + internalName,
                CssClass = "form-select",
                ClientIDMode = ClientIDMode.AutoID,
                EnableViewState = false
            };

            if (!required)
            {
                ddl.Items.Add(new ListItem("-- اختر --", ""));
            }

            if (choices != null)
            {
                foreach (string choice in choices)
                {
                    ddl.Items.Add(new ListItem(choice, choice));
                }
            }

            if (!string.IsNullOrEmpty(defaultValue) && ddl.Items.FindByValue(defaultValue) != null)
            {
                ddl.SelectedValue = defaultValue;
            }

            col.Controls.Add(lbl);
            col.Controls.Add(ddl);

            phForm.Controls.Add(col);
            _inputs[internalName] = ddl;
        }

        private void AddBooleanInput(string internalName, string label, string defaultValue)
        {
            var col = new Panel { CssClass = "col-12 col-md-6", EnableViewState = false };

            var lbl = new Label
            {
                CssClass = "form-label fw-semibold",
                Text = HttpUtility.HtmlEncode(label),
                AssociatedControlID = "fld_" + internalName
            };

            var ddl = new DropDownList
            {
                ID = "fld_" + internalName,
                CssClass = "form-select",
                ClientIDMode = ClientIDMode.AutoID,
                EnableViewState = false
            };

            ddl.Items.Add(new ListItem("نعم (مفعّل)", "1"));
            ddl.Items.Add(new ListItem("لا (معطّل)", "0"));

            if (defaultValue == "0" || defaultValue == "false")
            {
                ddl.SelectedValue = "0";
            }
            else
            {
                ddl.SelectedValue = "1";
            }

            col.Controls.Add(lbl);
            col.Controls.Add(ddl);

            phForm.Controls.Add(col);
            _inputs[internalName] = ddl;
        }

        private string GetControlValue(string internalName)
        {
            if (!_inputs.TryGetValue(internalName, out Control c) || c == null)
                return string.Empty;

            if (c is TextBox box) return box.Text.Trim();
            if (c is DropDownList ddl) return ddl.SelectedValue;

            return string.Empty;
        }

        private void SetControlValue(string internalName, string value)
        {
            if (!_inputs.TryGetValue(internalName, out Control c) || c == null)
                return;

            if (c is TextBox box)
            {
                box.Text = value ?? string.Empty;
                return;
            }

            if (c is DropDownList ddl)
            {
                if (!string.IsNullOrEmpty(value) && ddl.Items.FindByValue(value) != null)
                {
                    ddl.SelectedValue = value;
                }
                else if (ddl.Items.Count > 0)
                {
                    ddl.SelectedIndex = 0;
                }
            }
        }

        private void ClearForm()
        {
            foreach (var kvp in _inputs)
            {
                if (kvp.Value is TextBox box) box.Text = string.Empty;
                else if (kvp.Value is DropDownList ddl && ddl.Items.Count > 0) ddl.SelectedIndex = 0;
            }
        }

        private void RestoreFormState()
        {
            string mode = hfMode.Value;
            bool isOpen = mode == ModeNew || mode == ModeEdit;
            pnlForm.Visible = isOpen;

            if (isOpen)
            {
                ltFormTitle.Text = mode == ModeEdit
                    ? $"تعديل عنصر (ID #{hfItemId.Value})"
                    : "إضافة عنصر جديد";
            }
        }

        private void CloseForm()
        {
            hfMode.Value = string.Empty;
            hfItemId.Value = "0";
            pnlForm.Visible = false;
        }

        // =====================================================================
        // Event Handlers
        // =====================================================================
        protected void ddlLists_SelectedIndexChanged(object sender, EventArgs e)
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
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out int id)) return;

            if (e.CommandName == "EditItem")
            {
                LoadItemIntoForm(id);
            }
            else if (e.CommandName == "DeleteItem")
            {
                DeleteItem(id);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        // =====================================================================
        // CRUD Operations
        // =====================================================================
        private void LoadItemIntoForm(int id)
        {
            try
            {
                DgaPgListDef def = CurrentDef;
                if (def == null) return;

                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = DgaPgTargetWeb.Open(site))
                {
                    if (web == null) return;

                    SPList list = DgaPgListProvisioner.FindList(web, def.Name);
                    if (list == null) return;

                    SPListItem item = list.GetItemById(id);
                    if (item == null) return;

                    SetControlValue("Title", DgaPgHelper.SafeString(item, "Title"));

                    foreach (DgaPgFieldDef f in def.Fields)
                    {
                        string val;
                        if (f.Type == SPFieldType.Boolean)
                        {
                            val = DgaPgHelper.SafeBool(item, f.Name) ? "1" : "0";
                        }
                        else if (f.Type == SPFieldType.URL)
                        {
                            val = DgaPgHelper.SafeUrl(item, f.Name);
                        }
                        else
                        {
                            val = DgaPgHelper.SafeString(item, f.Name);
                        }

                        SetControlValue(f.Name, val);
                    }
                }

                hfMode.Value = ModeEdit;
                hfItemId.Value = id.ToString(CultureInfo.InvariantCulture);
                RestoreFormState();
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("ucDgaPgAdmin.LoadItemIntoForm", ex);
                ShowError("فشل تحميل بيانات العنصر: " + ex.Message);
            }
        }

        private void SaveItem()
        {
            DgaPgListDef def = CurrentDef;
            if (def == null) return;

            string title = GetControlValue("Title");
            if (string.IsNullOrEmpty(title))
            {
                ShowError("حقل العنوان الرئيسي (عربي) مطلوب.");
                RestoreFormState();
                return;
            }

            int.TryParse(hfItemId.Value, out int id);
            bool isNew = hfMode.Value != ModeEdit || id <= 0;

            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (DgaPgFieldDef f in def.Fields)
            {
                values[f.Name] = GetControlValue(f.Name);
            }

            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    if (SPContext.Current == null) return;

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = DgaPgTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = DgaPgListProvisioner.FindList(web, def.Name);
                            if (list == null) return;

                            SPListItem item = isNew ? list.AddItem() : list.GetItemById(id);
                            string cleanTitle = title.Length > 255 ? title.Substring(0, 255) : title;
                            item["Title"] = cleanTitle;

                            foreach (DgaPgFieldDef f in def.Fields)
                            {
                                if (!list.Fields.ContainsField(f.Name)) continue;
                                string raw = values.ContainsKey(f.Name) ? values[f.Name] : string.Empty;

                                if (f.Type == SPFieldType.Number)
                                {
                                    if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                                        item[f.Name] = num;
                                    else
                                        item[f.Name] = null;
                                }
                                else if (f.Type == SPFieldType.Boolean)
                                {
                                    item[f.Name] = (raw == "1" || raw.Equals("true", StringComparison.OrdinalIgnoreCase));
                                }
                                else if (f.Type == SPFieldType.URL)
                                {
                                    if (!string.IsNullOrEmpty(raw))
                                        item[f.Name] = new SPFieldUrlValue { Url = raw, Description = cleanTitle };
                                    else
                                        item[f.Name] = null;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(raw))
                                    {
                                        item[f.Name] = null;
                                    }
                                    else
                                    {
                                        SPField spField = DgaPgListProvisioner.GetFieldSafely(list, f.Name);
                                        if (spField != null && spField.Type == SPFieldType.Text)
                                        {
                                            string clean = Uri.UnescapeDataString(raw);
                                            if (clean.Length > 255) clean = clean.Substring(0, 255);
                                            item[f.Name] = clean;
                                        }
                                        else
                                        {
                                            item[f.Name] = raw;
                                        }
                                    }
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
                ShowMessage(isNew ? "تمت إضافة العنصر بنجاح." : "تم حفظ التعديلات بنجاح.", true);
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("ucDgaPgAdmin.SaveItem", ex);
                ShowError("حدث خطأ أثناء حفظ العنصر: " + ex.Message);
                RestoreFormState();
            }
        }

        private void DeleteItem(int id)
        {
            DgaPgListDef def = CurrentDef;
            if (def == null) return;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    if (SPContext.Current == null) return;

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = DgaPgTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = DgaPgListProvisioner.FindList(web, def.Name);
                            if (list == null) return;

                            SPListItem item = list.GetItemById(id);
                            if (item != null) item.Delete();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = prevUnsafe;
                        }
                    }
                });

                CloseForm();
                BindGrid();
                ShowMessage("تم حذف العنصر بنجاح.", true);
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("ucDgaPgAdmin.DeleteItem", ex);
                ShowError("فشل حذف العنصر: " + ex.Message);
            }
        }

        // =====================================================================
        // Grid & Actions
        // =====================================================================
        private void BindGrid()
        {
            DgaPgListDef def = CurrentDef;
            if (def == null) return;

            // Build dynamic headers
            var headers = new List<string> { "العنوان الرئيسي (Title AR)" };

            // Select display fields (exclude heavy note fields from table columns, keeping key metadata)
            var displayFields = def.Fields.Where(f => f.Type != SPFieldType.Note && f.Name != "Title_EN").Take(4).ToList();
            if (def.Fields.Any(f => f.Name == "Title_EN"))
            {
                headers.Add("Title (EN)");
            }
            foreach (var f in displayFields)
            {
                headers.Add(f.DisplayName);
            }

            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            var rows = new List<DgaPgAdminRow>();

            try
            {
                if (SPContext.Current != null)
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = DgaPgTargetWeb.Open(site))
                    {
                        if (web != null)
                        {
                            SPList list = DgaPgListProvisioner.FindList(web, def.Name);
                            if (list != null)
                            {
                                SPQuery q = new SPQuery { RowLimit = 500 };
                                SPListItemCollection items = list.GetItems(q);

                                foreach (SPListItem item in items)
                                {
                                    var row = new DgaPgAdminRow
                                    {
                                        Id = item.ID,
                                        DeleteConfirm = "return confirm('هل أنت متأكد من حذف هذا العنصر نهائياً؟');"
                                    };

                                    row.Cells.Add(DgaPgHelper.SafeString(item, "Title"));

                                    if (def.Fields.Any(f => f.Name == "Title_EN"))
                                    {
                                        row.Cells.Add(DgaPgHelper.SafeString(item, "Title_EN"));
                                    }

                                    foreach (var f in displayFields)
                                    {
                                        string val;
                                        if (f.Type == SPFieldType.Boolean)
                                        {
                                            val = DgaPgHelper.SafeBool(item, f.Name) ? "نعم" : "لا";
                                        }
                                        else if (f.Type == SPFieldType.URL)
                                        {
                                            val = DgaPgHelper.SafeUrl(item, f.Name);
                                        }
                                        else
                                        {
                                            val = DgaPgHelper.SafeString(item, f.Name);
                                        }

                                        row.Cells.Add(val);
                                    }

                                    rows.Add(row);
                                }
                            }
                        }
                    }
                }

                rptItems.DataSource = rows;
                rptItems.DataBind();

                pnlEmpty.Visible = rows.Count == 0;
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("ucDgaPgAdmin.BindGrid", ex);
                ShowError("فشل تحميل عناصر الجدول: " + ex.Message);
            }
        }

        protected void btnReSeedSelected_Click(object sender, EventArgs e)
        {
            string listName = CurrentListName;
            if (string.IsNullOrEmpty(listName)) return;

            int count = DgaPgListProvisioner.ReSeedList(listName);
            CloseForm();
            BindGrid();
            ShowMessage($"تمت إعادة تعبئة القائمة ({listName}) بنجاح بعدد {count} سجلاً افتراضياً.", true);
        }

        protected void btnReSeedAll_Click(object sender, EventArgs e)
        {
            int total = 0;
            foreach (var listDef in DgaPgListSchema.All)
            {
                total += DgaPgListProvisioner.ReSeedList(listDef.Name);
            }
            CloseForm();
            BindGrid();
            ShowMessage($"تمت إعادة تعبئة جميع القوائم بنجاح بإجمالي {total} سجلاً افتراضياً.", true);
        }

        private void ShowMessage(string msg, bool success)
        {
            pnlMessage.Visible = success;
            pnlError.Visible = !success;

            if (success) litMessage.Text = HttpUtility.HtmlEncode(msg);
            else litError.Text = HttpUtility.HtmlEncode(msg);
        }

        private void ShowError(string msg)
        {
            ShowMessage(msg, false);
        }
    }
}
