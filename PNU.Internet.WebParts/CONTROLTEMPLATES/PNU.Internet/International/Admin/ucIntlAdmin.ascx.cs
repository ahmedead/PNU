using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Generic add / edit / delete control for every International Students list.
    /// The form is generated from IntlListSchema, so adding a field to the schema
    /// automatically provisions the column AND exposes it in this screen.
    ///
    /// State is kept in HiddenFields and the form is rebuilt on every request,
    /// because ViewState is frequently disabled inside SharePoint web part zones.
    /// </summary>
    public partial class ucIntlAdmin : UserControl
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

                InternationalListProvisioner.EnsureAllListsExist();
                FillListPicker();

                // Rebuild the form for the list the PREVIOUS request rendered, so the
                // control tree LoadViewState walks is identical to the one that saved it.
                // Changing the picker rebuilds the form again in ddlList_SelectedIndexChanged,
                // which runs after ViewState has been loaded.
                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlAdmin.OnInit", ex);
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
                IntlLog.Write("ucIntlAdmin.Page_Load", ex);
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
                        if (!string.IsNullOrEmpty(posted) && IntlListSchema.Get(posted) != null)
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
            ltPageTitle.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminTitle",
                "إدارة محتوى الطلاب الدوليين", "International Students content management"));
            lblListLabel.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminList", "القسم", "Section"));
            ltNew.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminNew", " إضافة عنصر", " Add item"));
            ltActions.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminActions", "إجراءات", "Actions"));
            ltEmpty.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminEmpty",
                "لا توجد عناصر في هذا القسم بعد.", "No items in this section yet."));
            ltDenied.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminDenied",
                "ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            btnSave.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminSave", "حفظ", "Save"));
            btnCancel.Text = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminCancel", "إلغاء", "Cancel"));
        }

        // =====================================================================
        // list picker
        // =====================================================================
        private void FillListPicker()
        {
            ddlList.Items.Clear();

            if (!string.IsNullOrEmpty(FixedListName))
            {
                IntlListDef only = IntlListSchema.Get(FixedListName);
                if (only != null)
                {
                    ddlList.Items.Add(new ListItem(only.Display, only.Name));
                    ddlList.Visible = false;
                    lblListLabel.Visible = false;
                }
                return;
            }

            foreach (string name in IntlListNames.AllLists)
            {
                IntlListDef def = IntlListSchema.Get(name);
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

        private IntlListDef CurrentDef { get { return IntlListSchema.Get(CurrentListName); } }

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

            IntlListDef def = IntlListSchema.Get(listName);
            if (def == null) return;

            // Built-in Title column first.
            AddInput("Title", IntlHelper.Pick("العنوان (عربي)", "Title (AR)"), SPFieldType.Text, 0, null);

            foreach (IntlFieldDef f in def.Fields)
                AddInput(f.InternalName, f.Display, f.Type, f.Rows, f.Hint);
        }

        private void AddInput(string internalName, string label, SPFieldType type, int rows, string hint)
        {
            bool wide = type == SPFieldType.Note;

            var col = new Panel { CssClass = wide ? "col-12" : "col-12 col-md-6", EnableViewState = false };

            var lbl = new Label
            {
                CssClass = "form-label",
                Text = IntlHelper.Enc(label),
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
                    Text = "<div class=\"form-text\">" + IntlHelper.Enc(hint) + "</div>"
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

        /// <summary>Re-opens the form after a postback that left it open.</summary>
        private void RestoreFormState()
        {
            string mode = hfMode.Value;
            pnlForm.Visible = mode == ModeNew || mode == ModeEdit;

            ltFormTitle.Text = IntlHelper.Enc(mode == ModeEdit
                ? IntlHelper.GetRes("Intl_AdminEdit", "تعديل عنصر", "Edit item")
                : IntlHelper.GetRes("Intl_AdminNew", "إضافة عنصر", "Add item"));
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
                IntlListDef def = CurrentDef;
                if (def == null) return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = IntlTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = list.GetItemById(id);

                        SetBox("Title", IntlHelper.SafeString(item, "Title"));

                        foreach (IntlFieldDef f in def.Fields)
                        {
                            string value = f.Type == SPFieldType.URL
                                ? IntlHelper.SafeUrl(item, f.InternalName)
                                : IntlHelper.SafeString(item, f.InternalName);
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
                IntlLog.Write("ucIntlAdmin.LoadItemIntoForm", ex);
                ShowError(ex.Message);
            }
        }

        private void SaveItem()
        {
            IntlListDef def = CurrentDef;
            if (def == null) return;

            string title = GetBoxValue("Title");
            if (string.IsNullOrEmpty(title))
            {
                ShowError(IntlHelper.GetRes("Intl_AdminTitleRequired",
                    "العنوان مطلوب.", "Title is required."));
                RestoreFormState();
                return;
            }

            int id;
            int.TryParse(hfItemId.Value, out id);
            bool isNew = hfMode.Value != ModeEdit || id <= 0;

            // Snapshot the posted values before entering the elevated delegate.
            var values = new Dictionary<string, string>();
            foreach (IntlFieldDef f in def.Fields)
                values[f.InternalName] = GetBoxValue(f.InternalName);

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = IntlTargetWeb.Open(site))
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

                            foreach (IntlFieldDef f in def.Fields)
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
                    ? IntlHelper.GetRes("Intl_AdminAdded", "تمت إضافة العنصر بنجاح.", "Item added successfully.")
                    : IntlHelper.GetRes("Intl_AdminUpdated", "تم تحديث العنصر بنجاح.", "Item updated successfully."));
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlAdmin.SaveItem", ex);
                ShowError(ex.Message);
                RestoreFormState();
            }
        }

        private void DeleteItem(int id)
        {
            IntlListDef def = CurrentDef;
            if (def == null) return;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = IntlTargetWeb.Open(site))
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
                ShowMessage(IntlHelper.GetRes("Intl_AdminDeleted", "تم حذف العنصر.", "Item deleted."));
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlAdmin.DeleteItem", ex);
                ShowError(ex.Message);
            }
        }

        // =====================================================================
        // grid
        // =====================================================================
        private void BindGrid()
        {
            IntlListDef def = CurrentDef;
            if (def == null) return;

            var headers = new List<string>
            {
                IntlHelper.Enc(IntlHelper.Pick("العنوان (عربي)", "Title (AR)"))
            };
            var gridFields = def.Fields.Where(f => f.InGrid).ToList();
            foreach (IntlFieldDef f in gridFields) headers.Add(IntlHelper.Enc(f.Display));

            rptHeader.DataSource = headers;
            rptHeader.DataBind();

            string editText = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminEditBtn", "تعديل", "Edit"));
            string deleteText = IntlHelper.Enc(IntlHelper.GetRes("Intl_AdminDeleteBtn", "حذف", "Delete"));
            string confirmText = IntlHelper.GetRes("Intl_AdminConfirm",
                "هل أنت متأكد من حذف هذا العنصر؟", "Are you sure you want to delete this item?")
                .Replace("'", "\\'");

            var rows = new List<AdminRow>();

            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = IntlTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (SPListItem item in IntlHelper.GetItems(web, def.Name))
                    {
                        var row = new AdminRow
                        {
                            Id = item.ID,
                            EditText = editText,
                            DeleteText = deleteText,
                            DeleteConfirm = "return confirm('" + confirmText + "');"
                        };

                        row.Cells.Add(IntlHelper.Enc(IntlHelper.SafeString(item, "Title")));
                        foreach (IntlFieldDef f in gridFields)
                        {
                            string value = f.Type == SPFieldType.URL
                                ? IntlHelper.SafeUrl(item, f.InternalName)
                                : IntlHelper.SafeString(item, f.InternalName);
                            row.Cells.Add(IntlHelper.Enc(value));
                        }

                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlAdmin.BindGrid:" + def.Name, ex);
            }

            rptItems.DataSource = rows;
            rptItems.DataBind();
            pnlEmpty.Visible = rows.Count == 0;
        }

        // =====================================================================
        // helpers
        // =====================================================================
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
            ltMessage.Text = IntlHelper.Enc(text);
            pnlMessage.Visible = true;
        }

        private void ShowError(string text)
        {
            ltError.Text = IntlHelper.Enc(text);
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
            return ContentAdm.IsAdmin();
        }
    }

    /// <summary>Row DTO for the admin grid - all values already HTML-encoded.</summary>
    public class AdminRow
    {
        public int Id { get; set; }
        public List<string> Cells { get; set; }
        public string EditText { get; set; }
        public string DeleteText { get; set; }
        public string DeleteConfirm { get; set; }

        public AdminRow() { Cells = new List<string>(); }
    }
}
