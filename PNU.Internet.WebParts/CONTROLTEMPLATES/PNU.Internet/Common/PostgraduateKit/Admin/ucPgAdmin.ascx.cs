using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Content management for every Postgraduate Academic Kit list: create, edit, delete
    /// items, filter them by audience, and re-seed a list (or all of them) from the schema.
    /// </summary>
    public partial class ucPgAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";
        private const string AllPhases = "*";

        /// <summary>Restricts the control to a single list; the list picker is then hidden.</summary>
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

                PgListProvisioner.EnsureAllListsExist();
                FillListPicker();
                FillPhasePicker();

                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgAdmin.OnInit", ex);
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
                PgLog.Write("ucPgAdmin.Page_Load", ex);
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
                        if (!string.IsNullOrEmpty(posted) && PgListSchema.Get(posted) != null)
                            return posted;
                    }
                }
                catch { }

                return CurrentListName;
            }
        }

        private void SetTitles()
        {
            ltPageTitle.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminTitle",
                "إدارة محتوى الحقيبة الأكاديمية لطلبة الدراسات العليا",
                "Postgraduate academic kit content management"));

            ltWebPath.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminWeb",
                "تُحفظ البيانات في القوائم الموجودة تحت: " + PgTargetWeb.Path,
                "The data is stored in the lists under: " + PgTargetWeb.Path));

            lblListLabel.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminList", "القائمة", "List"));
            lblPhaseLabel.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminPhase", "الفئة", "Audience"));
            ltSeed.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminSeed", " إعادة تعبئة هذه القائمة", " Re-seed this list"));
            ltSeedAll.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminSeedAll", " إعادة تعبئة جميع القوائم", " Re-seed every list"));
            ltNew.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminNew", " إضافة عنصر", " Add item"));
            ltActions.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminActions", "إجراءات", "Actions"));
            ltEmpty.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminEmpty", "لا توجد عناصر في هذه القائمة بعد.", "No items in this list yet."));
            ltDenied.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminDenied", "ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            ltSeedHint.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminSeedHint",
                "إعادة التعبئة تحذف محتوى القائمة الحالي وتستبدله بالبيانات الافتراضية بالعربية والإنجليزية.",
                "Re-seeding deletes the current content of the list and replaces it with the default Arabic and English data."));

            btnSave.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminSave", "حفظ", "Save"));
            btnCancel.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminCancel", "إلغاء", "Cancel"));

            string confirmSeed = PgHelper.GetRes("Pg_AdminSeedConfirm",
                "سيتم حذف محتوى القائمة الحالي واستبداله بالبيانات الافتراضية. هل تريد المتابعة؟",
                "The current content of the list will be deleted and replaced with the default data. Continue?");
            btnSeed.OnClientClick = "return confirm('" + confirmSeed.Replace("'", "\\'") + "');";

            string confirmSeedAll = PgHelper.GetRes("Pg_AdminSeedAllConfirm",
                "سيتم حذف محتوى جميع قوائم الحقيبة واستبداله بالبيانات الافتراضية. هل تريد المتابعة؟",
                "The content of every kit list will be deleted and replaced with the default data. Continue?");
            btnSeedAll.OnClientClick = "return confirm('" + confirmSeedAll.Replace("'", "\\'") + "');";
        }

        private void FillListPicker()
        {
            ddlList.Items.Clear();

            if (!string.IsNullOrEmpty(FixedListName))
            {
                PgListDef only = PgListSchema.Get(FixedListName);
                if (only != null)
                {
                    ddlList.Items.Add(new ListItem(only.Display, only.Name));
                    ddlList.Visible = false;
                    lblListLabel.Visible = false;
                }
                return;
            }

            foreach (string name in PgListNames.All)
            {
                PgListDef def = PgListSchema.Get(name);
                if (def != null) ddlList.Items.Add(new ListItem(def.Display, def.Name));
            }

            string posted = Request.Form[ddlList.UniqueID];
            if (string.IsNullOrEmpty(posted)) posted = Request.QueryString["list"];

            if (!string.IsNullOrEmpty(posted) && ddlList.Items.FindByValue(posted) != null)
                ddlList.SelectedValue = posted;
        }

        private void FillPhasePicker()
        {
            ddlPhase.Items.Clear();
            ddlPhase.Items.Add(new ListItem(PgHelper.Pick("جميع الفئات", "All audiences"), AllPhases));

            foreach (string phase in PgPhases.All)
                ddlPhase.Items.Add(new ListItem(PgPhases.Display(phase), phase));

            string posted = Request.Form[ddlPhase.UniqueID];
            if (string.IsNullOrEmpty(posted)) posted = Request.QueryString["phase"];

            if (!string.IsNullOrEmpty(posted) && ddlPhase.Items.FindByValue(posted) != null)
                ddlPhase.SelectedValue = posted;
        }

        private string CurrentListName
        {
            get
            {
                if (!string.IsNullOrEmpty(FixedListName)) return FixedListName;
                return ddlList.SelectedValue;
            }
        }

        private string CurrentPhase
        {
            get
            {
                string value = ddlPhase.SelectedValue;
                return string.IsNullOrEmpty(value) || value == AllPhases ? null : value;
            }
        }

        private PgListDef CurrentDef { get { return PgListSchema.Get(CurrentListName); } }

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
                PgLog.Write("ucPgAdmin.ddlList_SelectedIndexChanged", ex);
                ShowError(ex.Message);
            }
        }

        protected void ddlPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();
                CloseForm();
                BindGrid();
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgAdmin.ddlPhase_SelectedIndexChanged", ex);
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
                    ShowError(PgHelper.GetRes("Pg_AdminNoList", "الرجاء اختيار قائمة أولاً.", "Please select a list first."));
                    return;
                }

                if (PgListProvisioner.ForceSeedDefaultData(listName))
                {
                    ShowMessage(PgHelper.GetRes("Pg_AdminSeeded", "تم إدراج البيانات الافتراضية بنجاح.", "Default data seeded successfully."));
                    BindGrid();
                }
                else
                {
                    ShowError(PgHelper.GetRes("Pg_AdminSeedFailed", "فشل إدراج البيانات الافتراضية.", "Failed to seed the default data."));
                }
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgAdmin.btnSeed_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnSeedAll_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();

                if (PgListProvisioner.ForceSeedAll())
                {
                    ShowMessage(PgHelper.GetRes("Pg_AdminSeededAll",
                        "تم إدراج البيانات الافتراضية في جميع القوائم بنجاح.",
                        "Default data seeded into every list successfully."));
                }
                else
                {
                    ShowError(PgHelper.GetRes("Pg_AdminSeedAllFailed",
                        "تعذّر إدراج البيانات الافتراضية في بعض القوائم، راجع سجل الأخطاء.",
                        "The default data could not be seeded into some lists; check the error log."));
                }

                BindGrid();
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgAdmin.btnSeedAll_Click", ex);
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
                PgLog.Write("ucPgAdmin.btnNew_Click", ex);
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

                PgListDef def = CurrentDef;
                if (def == null) return;

                bool isEdit = string.Equals(hfMode.Value, ModeEdit, StringComparison.OrdinalIgnoreCase);
                int itemId;
                int.TryParse(hfItemId.Value, out itemId);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = PgTargetWeb.Open(site))
                {
                    if (web == null) return;

                    bool prevUnsafe = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = isEdit ? list.GetItemById(itemId) : list.AddItem();

                        Control txtTitle = FindInput("Title");
                        if (txtTitle is TextBox) item["Title"] = ((TextBox)txtTitle).Text.Trim();

                        foreach (PgFieldDef f in def.Fields)
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
                                string val = ((DropDownList)ctrl).SelectedValue;
                                item[f.InternalName] = string.IsNullOrEmpty(val) ? null : val;
                            }
                        }

                        item.Update();
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = prevUnsafe;
                    }
                }

                ShowMessage(PgHelper.GetRes("Pg_AdminSaved", "تم حفظ البيانات بنجاح.", "Data saved successfully."));
                CloseForm();
                BindGrid();
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgAdmin.btnSave_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                HideMessage();
                HideError();

                int itemId = Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture);
                PgListDef def = CurrentDef;
                if (def == null) return;

                if (e.CommandName == "EditItem")
                {
                    BuildForm(def.Name);
                    OpenFormEdit(itemId);
                }
                else if (e.CommandName == "DeleteItem")
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = PgTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(def.Name);
                            if (list != null) list.GetItemById(itemId).Delete();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = prevUnsafe;
                        }
                    }

                    ShowMessage(PgHelper.GetRes("Pg_AdminDeleted", "تم حذف العنصر بنجاح.", "Item deleted successfully."));
                    CloseForm();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgAdmin.rptItems_ItemCommand", ex);
                ShowError(ex.Message);
            }
        }

        private void BuildForm(string listName)
        {
            phForm.Controls.Clear();
            _inputs.Clear();
            _builtListName = listName;

            PgListDef def = PgListSchema.Get(listName);
            if (def == null) return;

            AddFormField(new PgFieldDef("Title", "العنوان (عربي)", "Title (AR)", SPFieldType.Text, true));

            foreach (PgFieldDef f in def.Fields)
                AddFormField(f);
        }

        private void AddFormField(PgFieldDef f)
        {
            var col = new Panel { CssClass = f.Rows > 1 ? "col-12" : "col-12 col-md-6" };

            var label = new Label
            {
                Text = PgHelper.Enc(f.Display),
                CssClass = "form-label",
                ID = "lbl_" + f.InternalName
            };
            col.Controls.Add(label);

            Control input;
            if (f.Choices != null && f.Choices.Length > 0)
            {
                var ddl = new DropDownList { ID = "inp_" + f.InternalName, CssClass = "form-select" };
                ddl.Items.Add(new ListItem(PgHelper.Pick("(غير محدد)", "(not set)"), string.Empty));
                foreach (string choice in f.Choices)
                    ddl.Items.Add(new ListItem(choice, choice));
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
                col.Controls.Add(new Literal { Text = "<div class=\"form-text\">" + PgHelper.Enc(f.Hint) + "</div>" });

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
            ltFormTitle.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminNewItem", "إضافة عنصر جديد", "Add new item"));

            // pre-select the audience currently being filtered
            Control phase = FindInput("Phase");
            var ddl = phase as DropDownList;
            if (ddl != null && CurrentPhase != null && ddl.Items.FindByValue(CurrentPhase) != null)
                ddl.SelectedValue = CurrentPhase;

            pnlForm.Visible = true;
        }

        private void OpenFormEdit(int itemId)
        {
            PgListDef def = CurrentDef;
            if (def == null) return;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = PgTargetWeb.Open(site))
            {
                if (web == null) return;

                SPList list = web.Lists.TryGetList(def.Name);
                if (list == null) return;

                SPListItem item = list.GetItemById(itemId);

                Control txtTitle = FindInput("Title");
                if (txtTitle is TextBox) ((TextBox)txtTitle).Text = PgHelper.SafeString(item, "Title");

                foreach (PgFieldDef f in def.Fields)
                {
                    Control ctrl = FindInput(f.InternalName);

                    if (ctrl is TextBox)
                    {
                        ((TextBox)ctrl).Text = f.Type == SPFieldType.URL
                            ? PgHelper.SafeUrl(item, f.InternalName)
                            : PgHelper.SafeString(item, f.InternalName);
                    }
                    else if (ctrl is DropDownList)
                    {
                        string val = PgHelper.SafeString(item, f.InternalName);
                        var ddl = (DropDownList)ctrl;
                        ddl.SelectedValue = ddl.Items.FindByValue(val) != null ? val : string.Empty;
                    }
                }
            }

            hfMode.Value = ModeEdit;
            hfItemId.Value = itemId.ToString(CultureInfo.InvariantCulture);
            ltFormTitle.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminEditItem", "تعديل العنصر", "Edit item"));
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
                    ltFormTitle.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminEditItem", "تعديل العنصر", "Edit item"));
            }
            else if (string.Equals(hfMode.Value, ModeNew, StringComparison.OrdinalIgnoreCase))
            {
                ltFormTitle.Text = PgHelper.Enc(PgHelper.GetRes("Pg_AdminNewItem", "إضافة عنصر جديد", "Add new item"));
            }
        }

        private void BindGrid()
        {
            PgListDef def = CurrentDef;
            if (def == null) return;

            List<PgFieldDef> gridFields = def.Fields.Where(f => f.InGrid).ToList();

            var headers = new List<string> { PgHelper.Pick("العنوان", "Title") };
            headers.AddRange(gridFields.Select(f => f.Display));
            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = PgTargetWeb.Open(site))
            {
                if (web == null) return;

                List<SPListItem> items = CurrentPhase == null
                    ? PgHelper.GetItems(web, def.Name)
                    : PgHelper.GetItems(web, def.Name, CurrentPhase);

                pnlEmpty.Visible = (items.Count == 0);

                string editText = PgHelper.Enc(PgHelper.GetRes("Pg_AdminEdit", "تعديل", "Edit"));
                string deleteText = PgHelper.Enc(PgHelper.GetRes("Pg_AdminDelete", "حذف", "Delete"));
                string confirmText = "return confirm('"
                    + PgHelper.GetRes("Pg_AdminConfirm", "هل أنت متأكد من الحذف؟", "Are you sure you want to delete this item?").Replace("'", "\\'")
                    + "');";

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

        private List<string> BuildGridCells(SPListItem item, List<PgFieldDef> fields)
        {
            var cells = new List<string> { PgHelper.Enc(Shorten(PgHelper.SafeString(item, "Title"))) };

            foreach (PgFieldDef f in fields)
            {
                string val = f.Type == SPFieldType.URL
                    ? PgHelper.SafeUrl(item, f.InternalName)
                    : PgHelper.SafeString(item, f.InternalName);

                cells.Add(PgHelper.Enc(Shorten(val)));
            }

            return cells;
        }

        private static string Shorten(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = value.Replace('\n', ' ').Replace('\r', ' ').Trim();
            return value.Length <= 90 ? value : value.Substring(0, 90) + "…";
        }

        private void ShowMessage(string msg)
        {
            ltMessage.Text = PgHelper.Enc(msg);
            pnlMessage.Visible = true;
        }

        private void HideMessage()
        {
            pnlMessage.Visible = false;
        }

        private void ShowError(string err)
        {
            ltError.Text = PgHelper.Enc(err);
            pnlError.Visible = true;
        }

        private void HideError()
        {
            pnlError.Visible = false;
        }
    }
}
