using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public partial class ucPcAdmin : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string FixedListName { get; set; }

        private string CurrentListName
        {
            get
            {
                if (!string.IsNullOrEmpty(FixedListName)) return FixedListName;
                return ddlList.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PcListProvisioner.EnsureAllListsExist();

                if (!IsPostBack)
                {
                    PopulateListsDropdown();
                    BindGrid();
                }
                else
                {
                    if (pnlForm.Visible)
                    {
                        BuildForm(CurrentListName);
                    }
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcAdmin.Page_Load", ex);
                ShowError(ex.Message);
            }
        }

        private void PopulateListsDropdown()
        {
            ddlList.Items.Clear();
            foreach (string name in PcListNames.All)
            {
                PcListDef def = PcListSchema.Get(name);
                string text = def != null ? string.Format("{0} ({1})", def.Display, def.Name) : name;
                ddlList.Items.Add(new ListItem(text, name));
            }

            if (!string.IsNullOrEmpty(FixedListName))
            {
                ListItem item = ddlList.Items.FindByValue(FixedListName);
                if (item != null)
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
                HideMessage();
                HideError();
                CloseForm();
                BindGrid();
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcAdmin.ddlList_SelectedIndexChanged", ex);
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
                    ShowError(PcHelper.GetRes("Pc_AdminNoList", "الرجاء اختيار قسم أولاً.", "Please select a section first."));
                    return;
                }

                bool success = PcListProvisioner.ForceSeedDefaultData(listName);
                if (success)
                {
                    ShowMessage(PcHelper.GetRes("Pc_AdminSeeded", "تم إدراج البيانات الافتراضية بنجاح.", "Default data seeded successfully."));
                    BindGrid();
                }
                else
                {
                    ShowError(PcHelper.GetRes("Pc_AdminSeedFailed", "فشل إدراج البيانات الافتراضية.", "Failed to seed default data."));
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcAdmin.btnSeed_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();
                hfEditItemId.Value = "0";
                litFormTitle.Text = "إضافة عنصر جديد في " + GetListDisplayName(CurrentListName);
                BuildForm(CurrentListName);
                pnlForm.Visible = true;
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcAdmin.btnNew_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                HideError();

                int itemId = 0;
                int.TryParse(hfEditItemId.Value, out itemId);

                bool success = SaveItem(CurrentListName, itemId);
                if (success)
                {
                    ShowMessage("تم حفظ البيانات بنجاح.");
                    CloseForm();
                    BindGrid();
                }
                else
                {
                    ShowError("فشل حفظ البيانات.");
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcAdmin.btnSave_Click", ex);
                ShowError(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "EditItem")
                {
                    HideMessage();
                    HideError();
                    hfEditItemId.Value = itemId.ToString();
                    litFormTitle.Text = "تعديل عنصر #" + itemId;
                    BuildForm(CurrentListName);
                    PopulateForm(CurrentListName, itemId);
                    pnlForm.Visible = true;
                }
                else if (e.CommandName == "DeleteItem")
                {
                    DeleteItem(CurrentListName, itemId);
                    ShowMessage("تم حذف العنصر بنجاح.");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcAdmin.gvData_RowCommand", ex);
                ShowError(ex.Message);
            }
        }

        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        private void BindGrid()
        {
            try
            {
                string listName = CurrentListName;
                PcListDef def = PcListSchema.Get(listName);
                if (def == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = PcTargetWeb.Open(site))
                {
                    if (web == null) return;

                    List<SPListItem> items = PcHelper.GetItems(web, listName);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Title", typeof(string));
                    dt.Columns.Add("ItemOrder", typeof(int));

                    foreach (SPListItem item in items)
                    {
                        DataRow row = dt.NewRow();
                        row["ID"] = item.ID;
                        row["Title"] = PcHelper.SafeString(item, "Title");
                        row["ItemOrder"] = PcHelper.SafeInt(item, "ItemOrder");
                        dt.Rows.Add(row);
                    }

                    gvData.DataSource = dt;
                    gvData.DataBind();
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcAdmin.BindGrid", ex);
                ShowError(ex.Message);
            }
        }

        private void BuildForm(string listName)
        {
            divFormFields.Controls.Clear();
            PcListDef def = PcListSchema.Get(listName);
            if (def == null) return;

            foreach (PcFieldDef field in def.Fields)
            {
                HtmlGenericControl col = new HtmlGenericControl("div");
                col.Attributes["class"] = field.Rows > 1 ? "col-12" : "col-md-6";

                HtmlGenericControl label = new HtmlGenericControl("label");
                label.Attributes["class"] = "form-label fw-semibold";
                label.InnerText = field.Display;
                col.Controls.Add(label);

                string ctrlId = "txt_" + field.InternalName;

                if (field.Type == SPFieldType.Note)
                {
                    TextBox txt = new TextBox();
                    txt.ID = ctrlId;
                    txt.CssClass = "form-control";
                    txt.TextMode = TextBoxMode.MultiLine;
                    txt.Rows = field.Rows > 0 ? field.Rows : 4;
                    col.Controls.Add(txt);
                }
                else
                {
                    TextBox txt = new TextBox();
                    txt.ID = ctrlId;
                    txt.CssClass = "form-control";
                    col.Controls.Add(txt);
                }

                divFormFields.Controls.Add(col);
            }
        }

        private void PopulateForm(string listName, int itemId)
        {
            PcListDef def = PcListSchema.Get(listName);
            if (def == null) return;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = PcTargetWeb.Open(site))
            {
                if (web == null) return;

                SPList list = web.Lists.TryGetList(def.Name);
                if (list == null) return;

                SPListItem item = list.GetItemById(itemId);
                if (item == null) return;

                foreach (PcFieldDef field in def.Fields)
                {
                    string ctrlId = "txt_" + field.InternalName;
                    Control ctrl = divFormFields.FindControl(ctrlId);
                    if (ctrl is TextBox)
                    {
                        TextBox txt = (TextBox)ctrl;
                        if (field.Type == SPFieldType.URL)
                            txt.Text = PcHelper.SafeUrl(item, field.InternalName);
                        else
                            txt.Text = PcHelper.SafeString(item, field.InternalName);
                    }
                }
            }
        }

        private bool SaveItem(string listName, int itemId)
        {
            PcListDef def = PcListSchema.Get(listName);
            if (def == null) return false;

            bool success = false;
            Guid siteId = SPContext.Current.Site.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = PcTargetWeb.Open(site))
                {
                    if (web == null) return;

                    bool prevUnsafe = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = (itemId > 0) ? list.GetItemById(itemId) : list.AddItem();

                        foreach (PcFieldDef field in def.Fields)
                        {
                            string ctrlId = "txt_" + field.InternalName;
                            Control ctrl = divFormFields.FindControl(ctrlId);
                            if (ctrl is TextBox)
                            {
                                TextBox txt = (TextBox)ctrl;
                                string val = txt.Text.Trim();

                                if (string.Equals(field.InternalName, "Title", StringComparison.OrdinalIgnoreCase))
                                {
                                    item["Title"] = val;
                                }
                                else if (list.Fields.ContainsField(field.InternalName))
                                {
                                    SPField spField = list.Fields.GetFieldByInternalName(field.InternalName);
                                    if (spField.Type == SPFieldType.Number)
                                    {
                                        double dVal;
                                        item[spField.InternalName] = double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out dVal) ? dVal : 0;
                                    }
                                    else if (spField.Type == SPFieldType.URL)
                                    {
                                        item[spField.InternalName] = new SPFieldUrlValue { Url = val, Description = val };
                                    }
                                    else
                                    {
                                        item[spField.InternalName] = val;
                                    }
                                }
                            }
                        }

                        item.Update();
                        success = true;
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = prevUnsafe;
                    }
                }
            });

            return success;
        }

        private void DeleteItem(string listName, int itemId)
        {
            PcListDef def = PcListSchema.Get(listName);
            if (def == null) return;

            Guid siteId = SPContext.Current.Site.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = PcTargetWeb.Open(site))
                {
                    if (web == null) return;

                    bool prevUnsafe = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        SPList list = web.Lists.TryGetList(def.Name);
                        if (list == null) return;

                        SPListItem item = list.GetItemById(itemId);
                        if (item != null)
                        {
                            item.Delete();
                        }
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = prevUnsafe;
                    }
                }
            });
        }

        private void CloseForm()
        {
            pnlForm.Visible = false;
            hfEditItemId.Value = "0";
            divFormFields.Controls.Clear();
        }

        private string GetListDisplayName(string listName)
        {
            PcListDef def = PcListSchema.Get(listName);
            return def != null ? def.Display : listName;
        }

        private void ShowMessage(string msg)
        {
            pnlAlert.Visible = true;
            litAlert.Text = PcHelper.Enc(msg);
        }

        private void HideMessage()
        {
            pnlAlert.Visible = false;
            litAlert.Text = string.Empty;
        }

        private void ShowError(string msg)
        {
            pnlError.Visible = true;
            litError.Text = PcHelper.Enc(msg);
        }

        private void HideError()
        {
            pnlError.Visible = false;
            litError.Text = string.Empty;
        }
    }
}
