using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Admin
{
    /// <summary>
    /// Generic CRUD admin control for all About PNU lists on /ar/AboutUniversity.
    /// Forms and grids are driven by AboutPnuListSchema.
    /// </summary>
    public partial class ucAboutPnuAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string FixedListName { get; set; }

        private readonly Dictionary<string, Control> _inputs = new Dictionary<string, Control>();
        private bool _isAdmin;
        private string _builtListName;

        private string CurrentListName
        {
            get
            {
                if (!string.IsNullOrEmpty(FixedListName)) return FixedListName;
                return ddlList != null ? ddlList.SelectedValue : AboutPnuListNames.Overview;
            }
        }

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

                AboutPnuListProvisioner.EnsureAllListsExist();
                FillListPicker();

                BuildForm(ListRenderedLastRequest);
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPnuAdmin.OnInit", ex);
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
                AboutPnuLog.Write("ucAboutPnuAdmin.Page_Load", ex);
                ShowError(ex.Message);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (_isAdmin && hfListBuilt != null)
                hfListBuilt.Value = _builtListName ?? string.Empty;
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
                        if (!string.IsNullOrEmpty(posted) && AboutPnuListSchema.Get(posted) != null)
                            return posted;
                    }
                }
                catch { }

                return CurrentListName;
            }
        }

        private void SetTitles()
        {
            ltPageTitle.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminTitle",
                "إدارة محتوى صفحة عن الجامعة", "About PNU Content Management"));
            lblListLabel.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminList", "القسم / القائمة", "Section / List"));
            ltNew.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminNew", "إضافة عنصر", "Add item"));
            ltSeed.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminSeed", "إعادة تعيين البيانات الافتراضية", "Re-seed Default Data"));
            ltActions.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminActions", "إجراءات", "Actions"));
            ltEmpty.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminEmpty",
                "لا توجد عناصر في هذه القائمة بعد.", "No items in this list yet."));
            ltDenied.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminDenied",
                "ليس لديك صلاحية لإدارة هذا المحتوى.", "You do not have permission to manage this content."));

            ltSaveBtnText.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminSave", "حفظ", "Save"));
            ltCancelBtnText.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_AdminCancel", "إلغاء", "Cancel"));
        }

        private bool IsAdmin()
        {
            try
            {
                if (ContentAdm.IsAdmin()) return true;

                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    if (SPContext.Current.Web.UserIsSiteAdmin) return true;
                }
            }
            catch { }

            return false;
        }

        private void FillListPicker()
        {
            if (ddlList == null || ddlList.Items.Count > 0) return;

            foreach (string name in AboutPnuListNames.All)
            {
                AboutPnuListDef def = AboutPnuListSchema.Get(name);
                string text = def != null ? string.Format("{0} ({1})", def.Display, def.Name) : name;
                ddlList.Items.Add(new ListItem(text, name));
            }

            if (!string.IsNullOrEmpty(FixedListName))
            {
                ListItem match = ddlList.Items.FindByValue(FixedListName);
                if (match != null)
                {
                    ddlList.SelectedValue = FixedListName;
                    ddlList.Enabled = false;
                }
            }
        }

        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideMessages();
                CloseForm();
                BuildForm(CurrentListName);
                BindGrid();
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPnuAdmin.ddlList_SelectedIndexChanged", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessages();
                hfEditingId.Value = string.Empty;
                ltFormTitle.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_NewTitle", "إضافة عنصر جديد", "Add new item"));
                ResetFormValues();
                pnlForm.Visible = true;
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPnuAdmin.btnNew_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnSeed_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessages();
                string listName = CurrentListName;
                bool ok = AboutPnuListProvisioner.ForceSeedDefaultData(listName);
                if (ok)
                {
                    ShowSuccess(AboutPnuHelper.GetRes("AboutPnu_SeedSuccess", "تمت إعادة تعيين البيانات الافتراضية بنجاح.", "Default data re-seeded successfully."));
                    CloseForm();
                    BindGrid();
                }
                else
                {
                    ShowError(AboutPnuHelper.GetRes("AboutPnu_SeedFail", "حدث خطأ أثناء إعادة تعيين البيانات.", "Failed to re-seed default data."));
                }
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPnuAdmin.btnSeed_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            HideMessages();
            CloseForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessages();

                string listName = CurrentListName;
                AboutPnuListDef def = AboutPnuListSchema.Get(listName);
                if (def == null) return;

                int itemId = 0;
                int.TryParse(hfEditingId.Value, out itemId);
                bool isNew = itemId <= 0;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = AboutPnuTargetWeb.Open(site))
                {
                    if (web == null)
                    {
                        ShowError("Target subsite /ar/AboutUniversity could not be opened.");
                        return;
                    }

                    SPList list = web.Lists.TryGetList(def.Name);
                    if (list == null)
                    {
                        AboutPnuListProvisioner.EnsureListOnWeb(site.ID, web.ID, def);
                        list = web.Lists.TryGetList(def.Name);
                    }

                    if (list == null)
                    {
                        ShowError("List '" + def.Name + "' does not exist.");
                        return;
                    }

                    bool prevUnsafe = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;

                    try
                    {
                        SPListItem item = isNew ? list.AddItem() : list.GetItemById(itemId);
                        if (item == null)
                        {
                            ShowError("Item not found.");
                            return;
                        }

                        foreach (AboutPnuFieldDef f in def.Fields)
                        {
                            Control c;
                            if (!_inputs.TryGetValue(f.InternalName, out c)) continue;

                            if (f.Type == SPFieldType.Boolean)
                            {
                                CheckBox cb = c as CheckBox;
                                if (cb != null) item[f.InternalName] = cb.Checked;
                            }
                            else if (f.Type == SPFieldType.Number)
                            {
                                TextBox tb = c as TextBox;
                                if (tb != null)
                                {
                                    double num;
                                    if (double.TryParse(tb.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out num))
                                        item[f.InternalName] = num;
                                    else if (string.IsNullOrEmpty(tb.Text.Trim()))
                                        item[f.InternalName] = null;
                                }
                            }
                            else if (f.Type == SPFieldType.URL)
                            {
                                TextBox tb = c as TextBox;
                                if (tb != null)
                                {
                                    string val = tb.Text.Trim();
                                    if (string.IsNullOrEmpty(val))
                                    {
                                        item[f.InternalName] = null;
                                    }
                                    else
                                    {
                                        string url = val;
                                        string desc = string.Empty;
                                        int comma = val.IndexOf(',');
                                        if (comma > -1)
                                        {
                                            desc = val.Substring(comma + 1).Trim();
                                            url = val.Substring(0, comma).Trim();
                                        }
                                        item[f.InternalName] = new SPFieldUrlValue { Url = url, Description = desc };
                                    }
                                }
                            }
                            else if (f.Choices != null && f.Choices.Length > 0)
                            {
                                DropDownList ddl = c as DropDownList;
                                if (ddl != null) item[f.InternalName] = ddl.SelectedValue;
                            }
                            else
                            {
                                TextBox tb = c as TextBox;
                                if (tb != null)
                                {
                                    if (string.Equals(f.InternalName, "Title", StringComparison.OrdinalIgnoreCase))
                                        item["Title"] = tb.Text.Trim();
                                    else
                                        item[f.InternalName] = tb.Text;
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

                ShowSuccess(AboutPnuHelper.GetRes("AboutPnu_SaveSuccess", "تم حفظ العنصر بنجاح.", "Item saved successfully."));
                CloseForm();
                BindGrid();
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPnuAdmin.btnSave_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                HideMessages();
                int itemId;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out itemId)) return;

                if (string.Equals(e.CommandName, "EditItem", StringComparison.OrdinalIgnoreCase))
                {
                    OpenForEdit(itemId);
                }
                else if (string.Equals(e.CommandName, "DeleteItem", StringComparison.OrdinalIgnoreCase))
                {
                    DeleteItem(itemId);
                }
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPnuAdmin.rptItems_ItemCommand", ex);
                ShowError(ex.Message);
            }
        }

        private void OpenForEdit(int itemId)
        {
            string listName = CurrentListName;
            AboutPnuListDef def = AboutPnuListSchema.Get(listName);
            if (def == null) return;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = AboutPnuTargetWeb.Open(site))
            {
                if (web == null) return;
                SPList list = web.Lists.TryGetList(def.Name);
                if (list == null) return;

                SPListItem item = list.GetItemById(itemId);
                if (item == null) return;

                hfEditingId.Value = itemId.ToString(CultureInfo.InvariantCulture);
                ltFormTitle.Text = AboutPnuHelper.Enc(AboutPnuHelper.GetRes("AboutPnu_EditTitle", "تعديل عنصر", "Edit item") + " #" + itemId);

                foreach (AboutPnuFieldDef f in def.Fields)
                {
                    Control c;
                    if (!_inputs.TryGetValue(f.InternalName, out c)) continue;

                    if (f.Type == SPFieldType.Boolean)
                    {
                        CheckBox cb = c as CheckBox;
                        if (cb != null) cb.Checked = AboutPnuHelper.SafeBool(item, f.InternalName);
                    }
                    else if (f.Type == SPFieldType.URL)
                    {
                        TextBox tb = c as TextBox;
                        if (tb != null) tb.Text = AboutPnuHelper.SafeUrl(item, f.InternalName);
                    }
                    else if (f.Choices != null && f.Choices.Length > 0)
                    {
                        DropDownList ddl = c as DropDownList;
                        if (ddl != null)
                        {
                            string val = AboutPnuHelper.SafeString(item, f.InternalName);
                            ListItem li = ddl.Items.FindByValue(val);
                            if (li != null) ddl.SelectedValue = val;
                        }
                    }
                    else
                    {
                        TextBox tb = c as TextBox;
                        if (tb != null) tb.Text = AboutPnuHelper.SafeString(item, f.InternalName);
                    }
                }

                pnlForm.Visible = true;
            }
        }

        private void DeleteItem(int itemId)
        {
            string listName = CurrentListName;
            AboutPnuListDef def = AboutPnuListSchema.Get(listName);
            if (def == null) return;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = AboutPnuTargetWeb.Open(site))
            {
                if (web == null) return;
                SPList list = web.Lists.TryGetList(def.Name);
                if (list == null) return;

                bool prevUnsafe = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;
                try
                {
                    SPListItem item = list.GetItemById(itemId);
                    if (item != null)
                    {
                        item.Delete();
                        ShowSuccess(AboutPnuHelper.GetRes("AboutPnu_DeleteSuccess", "تم حذف العنصر بنجاح.", "Item deleted successfully."));
                    }
                }
                finally
                {
                    web.AllowUnsafeUpdates = prevUnsafe;
                }
            }

            CloseForm();
            BindGrid();
        }

        private void BuildForm(string listName)
        {
            if (phForm == null) return;
            phForm.Controls.Clear();
            _inputs.Clear();

            AboutPnuListDef def = AboutPnuListSchema.Get(listName);
            if (def == null) return;

            _builtListName = def.Name;

            foreach (AboutPnuFieldDef f in def.Fields)
            {
                Panel col = new Panel();
                col.CssClass = DetermineColClass(f);

                if (f.Type == SPFieldType.Boolean)
                {
                    CheckBox cb = new CheckBox
                    {
                        ID = "ctrl_" + f.InternalName,
                        Text = " " + f.Display,
                        CssClass = "form-check-input ms-1"
                    };
                    Panel wrapper = new Panel { CssClass = "form-check mt-4 pt-2" };
                    wrapper.Controls.Add(cb);
                    col.Controls.Add(wrapper);
                    _inputs[f.InternalName] = cb;
                }
                else
                {
                    Label lbl = new Label
                    {
                        Text = f.Display,
                        CssClass = "form-label fw-bold",
                        AssociatedControlID = "ctrl_" + f.InternalName
                    };
                    col.Controls.Add(lbl);

                    if (f.Choices != null && f.Choices.Length > 0)
                    {
                        DropDownList ddl = new DropDownList
                        {
                            ID = "ctrl_" + f.InternalName,
                            CssClass = "form-select"
                        };
                        foreach (string ch in f.Choices)
                        {
                            ddl.Items.Add(new ListItem(ch, ch));
                        }
                        col.Controls.Add(ddl);
                        _inputs[f.InternalName] = ddl;
                    }
                    else
                    {
                        TextBox tb = new TextBox
                        {
                            ID = "ctrl_" + f.InternalName,
                            CssClass = "form-control"
                        };

                        if (f.Type == SPFieldType.Note)
                        {
                            tb.TextMode = TextBoxMode.MultiLine;
                            tb.Rows = f.Rows > 0 ? f.Rows : 4;
                        }
                        else if (f.Type == SPFieldType.Number)
                        {
                            tb.TextMode = TextBoxMode.SingleLine;
                        }

                        if (f.InternalName.EndsWith("_EN", StringComparison.OrdinalIgnoreCase) ||
                            f.InternalName.Contains("Url") ||
                            f.InternalName.Contains("Icon") ||
                            f.InternalName.Contains("DateAttribute"))
                        {
                            tb.Attributes["dir"] = "ltr";
                        }

                        if (!string.IsNullOrEmpty(f.Hint))
                        {
                            tb.Attributes["placeholder"] = f.Hint;
                        }

                        col.Controls.Add(tb);
                        _inputs[f.InternalName] = tb;
                    }
                }

                phForm.Controls.Add(col);
            }
        }

        private string DetermineColClass(AboutPnuFieldDef f)
        {
            if (f.Type == SPFieldType.Note) return "col-12";
            if (f.InternalName.StartsWith("Title", StringComparison.OrdinalIgnoreCase) ||
                f.InternalName.StartsWith("Subtitle", StringComparison.OrdinalIgnoreCase) ||
                f.InternalName.StartsWith("ImageCaption", StringComparison.OrdinalIgnoreCase))
                return "col-12 col-md-6";
            if (f.InternalName.StartsWith("ImageAlt", StringComparison.OrdinalIgnoreCase) ||
                f.InternalName.StartsWith("Period", StringComparison.OrdinalIgnoreCase))
                return "col-12 col-md-6";
            if (f.Type == SPFieldType.URL) return "col-12 col-md-8";
            if (f.Type == SPFieldType.Number || f.Type == SPFieldType.Boolean) return "col-12 col-md-3";
            return "col-12 col-md-4";
        }

        private void RestoreFormState()
        {
            // Inputs preserve state via ViewState/PostBack automatically
        }

        private void ResetFormValues()
        {
            foreach (KeyValuePair<string, Control> kv in _inputs)
            {
                CheckBox cb = kv.Value as CheckBox;
                if (cb != null)
                {
                    cb.Checked = kv.Key.Equals("Visibility", StringComparison.OrdinalIgnoreCase);
                    continue;
                }
                TextBox tb = kv.Value as TextBox;
                if (tb != null) { tb.Text = string.Empty; continue; }
                DropDownList ddl = kv.Value as DropDownList;
                if (ddl != null && ddl.Items.Count > 0) ddl.SelectedIndex = 0;
            }
        }

        private void CloseForm()
        {
            pnlForm.Visible = false;
            hfEditingId.Value = string.Empty;
        }

        private void BindGrid()
        {
            try
            {
                string listName = CurrentListName;
                AboutPnuListDef def = AboutPnuListSchema.Get(listName);
                if (def == null) return;

                List<AboutPnuFieldDef> gridFields = def.Fields.Where(f => f.InGrid).ToList();

                rptHeader.DataSource = gridFields.Select(f => f.Display).ToList();
                rptHeader.DataBind();

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = AboutPnuTargetWeb.Open(site))
                {
                    if (web == null)
                    {
                        pnlEmpty.Visible = true;
                        rptItems.DataSource = null;
                        rptItems.DataBind();
                        return;
                    }

                    List<SPListItem> items = AboutPnuHelper.GetItems(web, def.Name);
                    if (items == null || items.Count == 0)
                    {
                        pnlEmpty.Visible = true;
                        rptItems.DataSource = null;
                        rptItems.DataBind();
                        return;
                    }

                    pnlEmpty.Visible = false;

                    var rows = items.Select(item => new
                    {
                        Id = item.ID,
                        Cells = gridFields.Select(f => FormatGridCell(item, f)).ToList()
                    }).ToList();

                    rptItems.DataSource = rows;
                    rptItems.DataBind();
                }
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPnuAdmin.BindGrid", ex);
                ShowError(ex.Message);
            }
        }

        private string FormatGridCell(SPListItem item, AboutPnuFieldDef field)
        {
            try
            {
                if (field.Type == SPFieldType.Boolean)
                {
                    bool b = AboutPnuHelper.SafeBool(item, field.InternalName);
                    return b
                        ? "<span class='badge bg-success text-white'>" + (AboutPnuHelper.IsArabic ? "نعم" : "Yes") + "</span>"
                        : "<span class='badge bg-secondary text-white'>" + (AboutPnuHelper.IsArabic ? "لا" : "No") + "</span>";
                }

                if (field.Type == SPFieldType.URL)
                {
                    string url = AboutPnuHelper.SafeUrl(item, field.InternalName);
                    if (string.IsNullOrEmpty(url)) return "-";
                    return "<a href='" + url + "' target='_blank' class='text-truncate d-inline-block' style='max-width:200px;'>" + url + "</a>";
                }

                string raw = AboutPnuHelper.SafeString(item, field.InternalName);
                if (string.IsNullOrEmpty(raw)) return "-";

                if (raw.Length > 60) raw = raw.Substring(0, 57) + "...";
                return AboutPnuHelper.Enc(raw);
            }
            catch
            {
                return "-";
            }
        }

        private void ShowSuccess(string msg)
        {
            ltMessage.Text = AboutPnuHelper.Enc(msg);
            pnlMessage.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string msg)
        {
            ltError.Text = AboutPnuHelper.Enc(msg);
            pnlError.Visible = true;
            pnlMessage.Visible = false;
        }

        private void HideMessages()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
        }
    }
}
