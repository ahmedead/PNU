using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.Admin
{
    public partial class ucMediaFilesAdmin : UserControl
    {
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";
        private const string TargetWebMissing = "تعذر الوصول إلى موقع المركز الإعلامي /ar/MediaCenter.";

        private bool _isAdmin;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                _isAdmin = MediaFilesListProvisioner.CheckAdminAccess(Page);

                pnlDenied.Visible = !_isAdmin;
                pnlMain.Visible = _isAdmin;

                if (!_isAdmin)
                {
                    if (!ContentAdm.IsAuthenticated())
                    {
                        ltDenied.Text = "يجب تسجيل الدخول أولاً للوصول إلى لوحة التحكم. <a href=\"javascript:void(0)\" onclick=\"openAdminLoginPopup(); return false;\" class=\"btn btn-sm btn-primary ms-2 me-2\">تسجيل الدخول / Login</a>";
                    }
                    return;
                }

                MediaFilesListProvisioner.EnsureAllListsExist();
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("ucMediaFilesAdmin.OnInit", ex);
                ShowError("حدث خطأ أثناء فحص الصلاحيات أو تهيئة القوائم: " + ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetNoServerCaching();

            if (!_isAdmin) return;

            try
            {
                if (!IsPostBack)
                {
                    BindStats();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("ucMediaFilesAdmin.Page_Load", ex);
                ShowError(ex.Message);
            }
        }

        #region Actions & Commands

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearAlerts();
            OpenFormNew();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAlerts();
            CloseForm();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearAlerts();
            BindStats();
            BindGrid();
        }

        protected void ddlFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAlerts();
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ClearAlerts();
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();

            string title = txtTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                ShowError("يرجى إدخال اسم أو عنوان الملف.");
                return;
            }

            string mediaType = ddlMediaType.SelectedValue;
            int itemOrder;
            if (!int.TryParse(txtItemOrder.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out itemOrder))
                itemOrder = 1;

            bool isNew = string.Equals(hfMode.Value, ModeNew, StringComparison.OrdinalIgnoreCase);
            int itemId = 0;
            int.TryParse(hfItemId.Value, out itemId);

            bool hasFile = fuAttachment.HasFile && fuAttachment.PostedFile != null;

            // ucMediaFiles.ascx renders every item as a download card, so a new item needs a file
            if (isNew && !hasFile)
            {
                ShowError("يرجى إرفاق الملف قبل الحفظ.");
                return;
            }

            byte[] fileBytes = hasFile ? fuAttachment.FileBytes : null;
            string fileName = hasFile ? Path.GetFileName(fuAttachment.PostedFile.FileName) : null;

            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = MediaFilesTargetWeb.OpenContentWeb(site))
                    {
                        if (web == null)
                        {
                            throw new InvalidOperationException(TargetWebMissing);
                        }

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(MediaFilesListProvisioner.ContentListName);
                            if (list == null)
                            {
                                throw new InvalidOperationException("قائمة MediaFiles غير موجودة على الموقع.");
                            }

                            SPListItem item = isNew ? list.AddItem() : list.GetItemById(itemId);

                            item["Title"] = title;
                            item["MediaType"] = mediaType;
                            item["ItemOrder"] = itemOrder;

                            // A new upload replaces the current attachment (the public page shows only the first one)
                            if (hasFile)
                            {
                                if (!isNew)
                                {
                                    while (item.Attachments.Count > 0)
                                    {
                                        item.Attachments.Delete(item.Attachments[0]);
                                    }
                                }

                                item.Attachments.Add(fileName, fileBytes);
                            }

                            item.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = prevUnsafe;
                        }
                    }
                });

                ShowMessage(isNew ? "تم إضافة الملف الإعلامي بنجاح." : "تم تحديث بيانات الملف الإعلامي بنجاح.");
                CloseForm();
                BindStats();
                BindGrid();
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("ucMediaFilesAdmin.btnSave_Click", ex);
                ShowError("فشل حفظ الملف: " + ex.Message);
            }
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();

            int itemId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out itemId)) return;

            if (e.CommandName == "EditItem")
            {
                OpenFormEdit(itemId);
            }
            else if (e.CommandName == "DeleteItem")
            {
                DeleteItem(itemId);
            }
        }

        #endregion

        #region Form Helpers

        private void OpenFormNew()
        {
            hfMode.Value = ModeNew;
            hfItemId.Value = "0";
            ltFormTitle.Text = "إضافة ملف جديد";
            txtTitle.Text = string.Empty;
            ddlMediaType.SelectedIndex = 0;
            txtItemOrder.Text = "1";
            pnlCurrentAttachment.Visible = false;
            pnlForm.Visible = true;
            txtTitle.Focus();
        }

        private void OpenFormEdit(int itemId)
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = MediaFilesTargetWeb.OpenContentWeb(site))
                    {
                        if (web == null) return;
                        SPList list = web.Lists.TryGetList(MediaFilesListProvisioner.ContentListName);
                        if (list == null) return;

                        SPListItem item = list.GetItemById(itemId);
                        if (item == null) return;

                        hfMode.Value = ModeEdit;
                        hfItemId.Value = itemId.ToString();
                        ltFormTitle.Text = "تعديل الملف: " + MediaFilesHelper.Enc(MediaFilesHelper.SafeString(item, "Title"));

                        txtTitle.Text = MediaFilesHelper.SafeString(item, "Title");

                        string currentType = MediaFilesHelper.SafeString(item, "MediaType");
                        if (ddlMediaType.Items.FindByValue(currentType) != null)
                            ddlMediaType.SelectedValue = currentType;

                        txtItemOrder.Text = MediaFilesHelper.SafeInt(item, "ItemOrder", 1).ToString();

                        string attUrl = MediaFilesHelper.GetAttachmentUrl(item);
                        string attName = MediaFilesHelper.GetAttachmentFileName(item);

                        if (!string.IsNullOrEmpty(attUrl))
                        {
                            pnlCurrentAttachment.Visible = true;
                            hlCurrentAttachment.NavigateUrl = attUrl;
                            ltCurrentAttachmentName.Text = MediaFilesHelper.Enc(attName);
                        }
                        else
                        {
                            pnlCurrentAttachment.Visible = false;
                        }

                        pnlForm.Visible = true;
                        txtTitle.Focus();
                    }
                });
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("ucMediaFilesAdmin.OpenFormEdit", ex);
                ShowError("تعذر تحميل بيانات الملف للتعديل: " + ex.Message);
            }
        }

        private void DeleteItem(int itemId)
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = MediaFilesTargetWeb.OpenContentWeb(site))
                    {
                        if (web == null) return;
                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(MediaFilesListProvisioner.ContentListName);
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

                ShowMessage("تم حذف الملف بنجاح.");
                CloseForm();
                BindStats();
                BindGrid();
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("ucMediaFilesAdmin.DeleteItem", ex);
                ShowError("تعذر حذف الملف: " + ex.Message);
            }
        }

        private void CloseForm()
        {
            hfMode.Value = ModeNew;
            hfItemId.Value = "0";
            pnlForm.Visible = false;
            txtTitle.Text = string.Empty;
            ddlMediaType.SelectedIndex = 0;
            txtItemOrder.Text = "1";
            pnlCurrentAttachment.Visible = false;
        }

        #endregion

        #region Data Binding & Stats

        private void BindStats()
        {
            try
            {
                int total = 0;
                int weekly = 0;
                int monthly = 0;

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = MediaFilesTargetWeb.OpenContentWeb(site))
                    {
                        if (web == null) return;
                        SPList list = web.Lists.TryGetList(MediaFilesListProvisioner.ContentListName);
                        if (list == null) return;

                        total = list.ItemCount;

                        SPQuery qWeekly = new SPQuery
                        {
                            Query = "<Where><Eq><FieldRef Name='MediaType' /><Value Type='Choice'>أسبوعي</Value></Eq></Where>",
                            RowLimit = 2000
                        };
                        SPListItemCollection itemsWeekly = list.GetItems(qWeekly);
                        weekly = itemsWeekly != null ? itemsWeekly.Count : 0;

                        SPQuery qMonthly = new SPQuery
                        {
                            Query = "<Where><Eq><FieldRef Name='MediaType' /><Value Type='Choice'>شهري</Value></Eq></Where>",
                            RowLimit = 2000
                        };
                        SPListItemCollection itemsMonthly = list.GetItems(qMonthly);
                        monthly = itemsMonthly != null ? itemsMonthly.Count : 0;
                    }
                });

                ltTotalCount.Text = total.ToString();
                ltWeeklyCount.Text = weekly.ToString();
                ltMonthlyCount.Text = monthly.ToString();
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("ucMediaFilesAdmin.BindStats", ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                string selectedFilter = ddlFilterType.SelectedValue;
                string keyword = txtSearch.Text.Trim();

                List<MediaFileAdminDto> dtoList = new List<MediaFileAdminDto>();
                bool webFound = false;

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = MediaFilesTargetWeb.OpenContentWeb(site))
                    {
                        if (web == null) return;
                        webFound = true;
                        SPList list = web.Lists.TryGetList(MediaFilesListProvisioner.ContentListName);
                        if (list == null || list.ItemCount == 0) return;

                        // Build CAML query
                        string whereCaml = string.Empty;
                        List<string> conditions = new List<string>();

                        if (!string.IsNullOrEmpty(selectedFilter))
                        {
                            conditions.Add(string.Format(
                                "<Eq><FieldRef Name='MediaType' /><Value Type='Choice'>{0}</Value></Eq>",
                                System.Security.SecurityElement.Escape(selectedFilter)));
                        }

                        if (!string.IsNullOrEmpty(keyword))
                        {
                            conditions.Add(string.Format(
                                "<Contains><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Contains>",
                                System.Security.SecurityElement.Escape(keyword)));
                        }

                        if (conditions.Count == 1)
                        {
                            whereCaml = "<Where>" + conditions[0] + "</Where>";
                        }
                        else if (conditions.Count > 1)
                        {
                            whereCaml = "<Where><And>" + conditions[0] + conditions[1] + "</And></Where>";
                        }

                        SPQuery query = new SPQuery
                        {
                            Query = whereCaml + "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /><FieldRef Name='Created' Ascending='FALSE' /></OrderBy>",
                            RowLimit = 500
                        };

                        SPListItemCollection items = list.GetItems(query);
                        if (items != null)
                        {
                            foreach (SPListItem itm in items)
                            {
                                DateTime createdDate = itm.Fields.ContainsField("Created") && itm["Created"] != null 
                                    ? Convert.ToDateTime(itm["Created"]) 
                                    : DateTime.MinValue;

                                dtoList.Add(new MediaFileAdminDto
                                {
                                    ID = itm.ID,
                                    Title = MediaFilesHelper.SafeString(itm, "Title"),
                                    MediaType = MediaFilesHelper.SafeString(itm, "MediaType"),
                                    ItemOrder = MediaFilesHelper.SafeInt(itm, "ItemOrder", 1),
                                    AttachmentUrl = MediaFilesHelper.GetAttachmentUrl(itm),
                                    AttachmentFileName = MediaFilesHelper.GetAttachmentFileName(itm),
                                    CreatedDate = createdDate != DateTime.MinValue ? createdDate.ToString("yyyy-MM-dd") : string.Empty
                                });
                            }
                        }
                    }
                });

                if (!webFound) ShowError(TargetWebMissing);

                rptItems.DataSource = dtoList;
                rptItems.DataBind();

                pnlEmpty.Visible = (dtoList.Count == 0);
            }
            catch (Exception ex)
            {
                MediaFilesLog.Write("ucMediaFilesAdmin.BindGrid", ex);
                ShowError("تعذر استرجاع ملفات المركز الإعلامي: " + ex.Message);
            }
        }

        #endregion

        #region Alerts

        private void ShowMessage(string msg)
        {
            ltMessage.Text = MediaFilesHelper.Enc(msg);
            pnlMessage.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string err)
        {
            ltError.Text = MediaFilesHelper.Enc(err);
            pnlError.Visible = true;
            pnlMessage.Visible = false;
        }

        private void ClearAlerts()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
        }

        #endregion
    }

    public class MediaFileAdminDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string MediaType { get; set; }
        public int ItemOrder { get; set; }
        public string AttachmentUrl { get; set; }
        public string AttachmentFileName { get; set; }
        public string CreatedDate { get; set; }
    }
}
