using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.Helpers;
using Portal.Main.Helper.Utils;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.Admin
{
    public partial class ucHomeFooterAdmin : UserControl
    {
        private const string InvalidUrlMessage = "{0} غير صالح. يجب أن يبدأ بـ / أو http:// أو https://";

        private bool _isAdmin;
        private string _initError; // already HTML-safe

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Bootstrap switch styling for checkboxes
            chkNavLinkNewTab.InputAttributes["class"] = "form-check-input";
            chkNavLinkVisible.InputAttributes["class"] = "form-check-input";
            chkSocialVisible.InputAttributes["class"] = "form-check-input";
            chkDownLinkNewTab.InputAttributes["class"] = "form-check-input";
            chkDownLinkVisible.InputAttributes["class"] = "form-check-input";

            try
            {
                _isAdmin = ContentAdm.IsAdmin();
                pnlDenied.Visible = !_isAdmin;
                pnlMain.Visible = _isAdmin;
                phHeaderActions.Visible = _isAdmin;

                if (!_isAdmin)
                {
                    if (!ContentAdm.IsAuthenticated())
                    {
                        ltDenied.Text = "يجب تسجيل الدخول أولاً للوصول إلى لوحة التحكم. <a href=\"javascript:void(0)\" onclick=\"openAdminLoginPopup(); return false;\" class=\"btn btn-sm btn-primary ms-2 me-2\">تسجيل الدخول / Login</a>";
                    }
                    return;
                }

                if (!Page.IsPostBack)
                {
                    FooterListProvisioner.EnsureFooterLists();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucHomeFooterAdmin.OnInit", ex.Message);
                _initError = Enc(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();

            if (!IsPostBack)
            {
                LoadAll();
            }

            if (!string.IsNullOrEmpty(_initError))
            {
                ShowError(_initError);
            }
        }

        private void LoadAll()
        {
            LoadSections();
            BindNavLinksGrid();
            LoadContacts();
            BindSocialGrid();
            LoadSettings();
            BindDownLinksGrid();
        }

        #region Tab Navigation

        protected void tabBtnNavLinks_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            SetActiveTab(tabBtnNavLinks, pnlTabNavLinks);
        }

        protected void tabBtnContact_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            SetActiveTab(tabBtnContact, pnlTabContact);
        }

        protected void tabBtnSocial_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            SetActiveTab(tabBtnSocial, pnlTabSocial);
        }

        protected void tabBtnBottom_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            SetActiveTab(tabBtnBottom, pnlTabBottom);
        }

        private void SetActiveTab(LinkButton activeBtn, Panel activePnl)
        {
            tabBtnNavLinks.CssClass = "nav-link rounded-3 fw-semibold";
            tabBtnContact.CssClass = "nav-link rounded-3 fw-semibold";
            tabBtnSocial.CssClass = "nav-link rounded-3 fw-semibold";
            tabBtnBottom.CssClass = "nav-link rounded-3 fw-semibold";

            activeBtn.CssClass = "nav-link rounded-3 fw-semibold active";

            pnlTabNavLinks.Visible = false;
            pnlTabContact.Visible = false;
            pnlTabSocial.Visible = false;
            pnlTabBottom.Visible = false;

            activePnl.Visible = true;
        }

        #endregion

        #region Actions Header

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            FooterCache.Invalidate();
            LoadAll();
            ShowMessage("تم تحديث البيانات بنجاح.");
        }

        protected void btnProvision_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            try
            {
                FooterListProvisioner.ForceProvision();
                LoadAll();
                ShowMessage("تم التحقق من قوائم الفوتر وإنشاء القوائم والحقول الناقصة. لا تُعبأ البيانات الافتراضية إلا في القوائم المنشأة حديثًا.");
            }
            catch (Exception ex)
            {
                ShowError("فشل أثناء تهيئة القوائم: " + Enc(ex.Message));
            }
        }

        #endregion

        #region Tab 1: Navigation Sections & Links

        private void LoadSections()
        {
            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = rootWeb.Lists.TryGetList(FooterListProvisioner.LIST_NAVSECTIONS);
                    if (list == null) return;

                    foreach (SPListItem item in list.Items)
                    {
                        string key = SafeString(item["Title"]);
                        string tAr = SafeString(item["TitleAr"]);
                        string tEn = SafeString(item["TitleEn"]);

                        if (key.Equals("Overview", StringComparison.OrdinalIgnoreCase))
                        {
                            txtSecOverviewAr.Text = tAr;
                            txtSecOverviewEn.Text = tEn;
                        }
                        else if (key.Equals("Opportunities", StringComparison.OrdinalIgnoreCase))
                        {
                            txtSecOppAr.Text = tAr;
                            txtSecOppEn.Text = tEn;
                        }
                        else if (key.Equals("UniversityLife", StringComparison.OrdinalIgnoreCase))
                        {
                            txtSecLifeAr.Text = tAr;
                            txtSecLifeEn.Text = tEn;
                        }
                        else if (key.Equals("RelatedLinks", StringComparison.OrdinalIgnoreCase))
                        {
                            txtSecRelAr.Text = tAr;
                            txtSecRelEn.Text = tEn;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل عناوين الأقسام: " + Enc(ex.Message));
            }
        }

        protected void btnSaveSections_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_NAVSECTIONS);
                    WithUnsafeUpdates(rootWeb, () =>
                    {
                        SaveSectionItem(list, "Overview", txtSecOverviewAr.Text.Trim(), txtSecOverviewEn.Text.Trim(), 1, 1);
                        SaveSectionItem(list, "Opportunities", txtSecOppAr.Text.Trim(), txtSecOppEn.Text.Trim(), 2, 2);
                        SaveSectionItem(list, "UniversityLife", txtSecLifeAr.Text.Trim(), txtSecLifeEn.Text.Trim(), 2, 3);
                        SaveSectionItem(list, "RelatedLinks", txtSecRelAr.Text.Trim(), txtSecRelEn.Text.Trim(), 3, 4);
                    });
                });

                FooterCache.Invalidate();
                ShowMessage("تم حفظ عناوين أقسام الفوتر بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء حفظ عناوين الأقسام: " + Enc(ex.Message));
            }
        }

        private void SaveSectionItem(SPList list, string key, string tAr, string tEn, int col, int order)
        {
            SPListItem target = null;
            foreach (SPListItem item in list.Items)
            {
                if (SafeString(item["Title"]).Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    target = item;
                    break;
                }
            }

            if (target == null)
            {
                target = list.Items.Add();
                target["Title"] = key;
            }

            target["TitleAr"] = tAr;
            target["TitleEn"] = tEn;
            target["ColumnNumber"] = col;
            target["ItemOrder"] = order;
            target["Visibility"] = true;
            target.Update();
        }

        private void BindNavLinksGrid()
        {
            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = rootWeb.Lists.TryGetList(FooterListProvisioner.LIST_NAVLINKS);
                    if (list == null) return;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Title");
                    dt.Columns.Add("TitleEn");
                    dt.Columns.Add("SectionKey");
                    dt.Columns.Add("UrlAr");
                    dt.Columns.Add("UrlEn");
                    dt.Columns.Add("OpenInNewTab", typeof(bool));
                    dt.Columns.Add("Visibility", typeof(bool));
                    dt.Columns.Add("ItemOrder", typeof(int));

                    string selectedSec = ddlFilterSection.SelectedValue;
                    SPQuery q = new SPQuery
                    {
                        Query = "<OrderBy><FieldRef Name='SectionKey'/><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                    };

                    foreach (SPListItem item in list.GetItems(q))
                    {
                        string secKey = SafeString(item["SectionKey"]);
                        if (!string.IsNullOrEmpty(selectedSec) && !secKey.Equals(selectedSec, StringComparison.OrdinalIgnoreCase))
                            continue;

                        SPFieldUrlValue uAr = new SPFieldUrlValue(SafeString(item["URLAr"]));
                        SPFieldUrlValue uEn = new SPFieldUrlValue(SafeString(item["URLEn"]));

                        dt.Rows.Add(
                            item.ID,
                            SafeString(item["Title"]),
                            SafeString(item["TitleEn"]),
                            secKey,
                            uAr.Url ?? "",
                            uEn.Url ?? "",
                            SafeBool(item["OpenInNewTab"]),
                            SafeBool(item["Visibility"]),
                            ParseOrder(SafeString(item["ItemOrder"]), 1)
                        );
                    }

                    gvNavLinks.DataSource = dt;
                    gvNavLinks.DataBind();

                    lblNavLinkCount.Text = dt.Rows.Count + " رابط";
                });
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل روابط الفوتر: " + Enc(ex.Message));
            }
        }

        protected void ddlFilterSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            BindNavLinksGrid();
        }

        protected void btnNewNavLink_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            hfNavLinkId.Value = "0";
            ltNavLinkFormTitle.Text = "إضافة رابط جديد";
            txtNavLinkTitleAr.Text = "";
            txtNavLinkTitleEn.Text = "";
            txtNavLinkUrlAr.Text = "";
            txtNavLinkUrlEn.Text = "";
            txtNavLinkOrder.Text = "1";
            chkNavLinkNewTab.Checked = false;
            chkNavLinkVisible.Checked = true;
            if (!string.IsNullOrEmpty(ddlFilterSection.SelectedValue))
                ddlNavSection.SelectedValue = ddlFilterSection.SelectedValue;

            pnlNavLinkForm.Visible = true;
        }

        protected void btnCancelNavLink_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            pnlNavLinkForm.Visible = false;
            hfNavLinkId.Value = "0";
        }

        protected void btnSaveNavLink_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            string tAr = txtNavLinkTitleAr.Text.Trim();
            string tEn = txtNavLinkTitleEn.Text.Trim();
            string uAr = txtNavLinkUrlAr.Text.Trim();
            string uEn = txtNavLinkUrlEn.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(tAr))
                    throw new ValidationException("الرجاء إدخال عنوان الرابط بالعربية.");
                if (string.IsNullOrEmpty(uAr))
                    throw new ValidationException("الرجاء إدخال الرابط العربي.");
                RequireSafeUrl(uAr, "الرابط العربي");
                RequireSafeUrl(uEn, "الرابط الإنجليزي");

                int id = ParseId(hfNavLinkId.Value);
                int order = ParseOrder(txtNavLinkOrder.Text, 1);

                RunElevated(rootWeb =>
                {
                    SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_NAVLINKS);
                    WithUnsafeUpdates(rootWeb, () =>
                    {
                        SPListItem item = id > 0 ? list.GetItemById(id) : list.Items.Add();

                        item["Title"] = tAr;
                        item["TitleEn"] = tEn;
                        item["SectionKey"] = ddlNavSection.SelectedValue;
                        item["URLAr"] = new SPFieldUrlValue { Url = uAr, Description = tAr };
                        item["URLEn"] = new SPFieldUrlValue { Url = string.IsNullOrEmpty(uEn) ? uAr : uEn, Description = string.IsNullOrEmpty(tEn) ? tAr : tEn };
                        item["OpenInNewTab"] = chkNavLinkNewTab.Checked;
                        item["Visibility"] = chkNavLinkVisible.Checked;
                        item["ItemOrder"] = order;
                        item.Update();
                    });
                });

                FooterCache.Invalidate();
                pnlNavLinkForm.Visible = false;
                hfNavLinkId.Value = "0";
                BindNavLinksGrid();
                ShowMessage("تم حفظ الرابط بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء حفظ الرابط: " + Enc(ex.Message));
            }
        }

        protected void gvNavLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!_isAdmin) return;
            int id = ParseId(Convert.ToString(e.CommandArgument));
            if (id <= 0) return;

            try
            {
                if (e.CommandName == "EditItem")
                {
                    RunElevated(rootWeb =>
                    {
                        SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_NAVLINKS);
                        SPListItem item = list.GetItemById(id);

                        hfNavLinkId.Value = id.ToString(CultureInfo.InvariantCulture);
                        ltNavLinkFormTitle.Text = "تعديل الرابط: " + Enc(SafeString(item["Title"]));
                        txtNavLinkTitleAr.Text = SafeString(item["Title"]);
                        txtNavLinkTitleEn.Text = SafeString(item["TitleEn"]);

                        string secKey = SafeString(item["SectionKey"]);
                        if (ddlNavSection.Items.FindByValue(secKey) != null)
                            ddlNavSection.SelectedValue = secKey;

                        txtNavLinkUrlAr.Text = new SPFieldUrlValue(SafeString(item["URLAr"])).Url;
                        txtNavLinkUrlEn.Text = new SPFieldUrlValue(SafeString(item["URLEn"])).Url;

                        txtNavLinkOrder.Text = ParseOrder(SafeString(item["ItemOrder"]), 1).ToString(CultureInfo.InvariantCulture);
                        chkNavLinkNewTab.Checked = SafeBool(item["OpenInNewTab"]);
                        chkNavLinkVisible.Checked = SafeBool(item["Visibility"]);

                        pnlNavLinkForm.Visible = true;
                    });
                }
                else if (e.CommandName == "ToggleVisibility")
                {
                    ToggleVisibility(FooterListProvisioner.LIST_NAVLINKS, id);
                    BindNavLinksGrid();
                    ShowMessage("تم تغيير حالة الرابط بنجاح.");
                }
                else if (e.CommandName == "DeleteItem")
                {
                    DeleteItem(FooterListProvisioner.LIST_NAVLINKS, id);
                    if (hfNavLinkId.Value == id.ToString(CultureInfo.InvariantCulture))
                    {
                        pnlNavLinkForm.Visible = false;
                        hfNavLinkId.Value = "0";
                    }
                    BindNavLinksGrid();
                    ShowMessage("تم حذف الرابط بنجاح.");
                }
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("تعذر تنفيذ العملية على الرابط: " + Enc(ex.Message));
            }
        }

        protected void gvNavLinks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;
                bool isVisible = Convert.ToBoolean(row["Visibility"]);
                bool newTab = Convert.ToBoolean(row["OpenInNewTab"]);

                Label lblVis = (Label)e.Row.FindControl("lblVisible");
                if (lblVis != null)
                {
                    lblVis.Text = isVisible
                        ? "<span class='badge bg-success-subtle text-success'>مفعل</span>"
                        : "<span class='badge bg-danger-subtle text-danger'>معطل</span>";
                }

                Label lblTab = (Label)e.Row.FindControl("lblNewTab");
                if (lblTab != null)
                {
                    lblTab.Text = newTab
                        ? "<span class='badge bg-info-subtle text-info'>_blank</span>"
                        : "<span class='badge bg-light text-muted border'>_self</span>";
                }
            }
        }

        #endregion

        #region Tab 2: Contact Info

        private void LoadContacts()
        {
            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = rootWeb.Lists.TryGetList(FooterListProvisioner.LIST_CONTACT);
                    if (list == null) return;

                    foreach (SPListItem item in list.Items)
                    {
                        string key = SafeString(item["Title"]);
                        string val = SafeString(item["Value"]);
                        string lbl = SafeString(item["LabelAr"]);

                        if (key.Equals("Phone", StringComparison.OrdinalIgnoreCase))
                        {
                            txtContactPhone.Text = !string.IsNullOrEmpty(lbl) ? lbl : val;
                        }
                        else if (key.Equals("PhoneIntl", StringComparison.OrdinalIgnoreCase))
                        {
                            txtContactPhoneIntl.Text = !string.IsNullOrEmpty(lbl) ? lbl : val;
                        }
                        else if (key.Equals("Email", StringComparison.OrdinalIgnoreCase))
                        {
                            txtContactEmail.Text = val;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل بيانات التواصل: " + Enc(ex.Message));
            }
        }

        protected void btnSaveContact_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            string phone = txtContactPhone.Text.Trim();
            string phoneIntl = txtContactPhoneIntl.Text.Trim();
            string email = txtContactEmail.Text.Trim();

            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_CONTACT);
                    WithUnsafeUpdates(rootWeb, () =>
                    {
                        SaveContactRow(list, "Phone", "Phone", phone, phone, phone.Replace(" ", "").Replace("-", ""), 1);
                        SaveContactRow(list, "PhoneIntl", "Phone", phoneIntl, phoneIntl, phoneIntl.Replace(" ", "").Replace("-", ""), 2);
                        SaveContactRow(list, "Email", "Email", email, email, email, 3);
                    });
                });

                FooterCache.Invalidate();
                ShowMessage("تم حفظ معلومات التواصل بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء حفظ بيانات التواصل: " + Enc(ex.Message));
            }
        }

        private void SaveContactRow(SPList list, string key, string type, string lblAr, string lblEn, string val, int order)
        {
            SPListItem target = null;
            foreach (SPListItem item in list.Items)
            {
                if (SafeString(item["Title"]).Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    target = item;
                    break;
                }
            }

            if (target == null)
            {
                target = list.Items.Add();
                target["Title"] = key;
            }

            target["ContactType"] = type;
            target["LabelAr"] = lblAr;
            target["LabelEn"] = lblEn;
            target["Value"] = val;
            // An emptied field hides that contact row on the footer
            target["Visibility"] = !string.IsNullOrEmpty(val);
            target["ItemOrder"] = order;
            target.Update();
        }

        #endregion

        #region Tab 3: Social Media

        private void BindSocialGrid()
        {
            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = rootWeb.Lists.TryGetList(FooterListProvisioner.LIST_FOLLOWUS);
                    if (list == null) return;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Title");
                    dt.Columns.Add("IconClass");
                    dt.Columns.Add("Url");
                    dt.Columns.Add("AriaLabelAr");
                    dt.Columns.Add("AriaLabelEn");
                    dt.Columns.Add("ParentKey");
                    dt.Columns.Add("Visibility", typeof(bool));
                    dt.Columns.Add("ItemOrder", typeof(int));

                    SPQuery q = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>" };
                    foreach (SPListItem item in list.GetItems(q))
                    {
                        SPFieldUrlValue u = new SPFieldUrlValue(SafeString(item["URL"]));

                        dt.Rows.Add(
                            item.ID,
                            SafeString(item["Title"]),
                            SafeString(item["IconClass"]),
                            u.Url ?? "",
                            SafeString(item["AriaLabelAr"]),
                            SafeString(item["AriaLabelEn"]),
                            SafeString(item["ParentKey"]),
                            SafeBool(item["Visibility"]),
                            ParseOrder(SafeString(item["ItemOrder"]), 1)
                        );
                    }

                    gvSocial.DataSource = dt;
                    gvSocial.DataBind();
                    lblSocialCount.Text = dt.Rows.Count + " حساب";
                });
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل قنوات التواصل الاجتماعي: " + Enc(ex.Message));
            }
        }

        protected void btnNewSocial_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            hfSocialId.Value = "0";
            ltSocialFormTitle.Text = "إضافة منصة تواصل جديدة";
            txtSocialTitle.Text = "";
            txtSocialIconClass.Text = "";
            txtSocialParentKey.Text = "";
            txtSocialUrl.Text = "";
            txtSocialAriaAr.Text = "";
            txtSocialAriaEn.Text = "";
            txtSocialOrder.Text = "1";
            chkSocialVisible.Checked = true;
            pnlSocialForm.Visible = true;
        }

        protected void btnCancelSocial_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            pnlSocialForm.Visible = false;
            hfSocialId.Value = "0";
        }

        protected void btnSaveSocial_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            string key = txtSocialTitle.Text.Trim();
            string url = txtSocialUrl.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(key))
                    throw new ValidationException("الرجاء إدخال اسم المنصة أو المفتاح.");
                RequireSafeUrl(url, "رابط المنصة");

                int id = ParseId(hfSocialId.Value);
                int order = ParseOrder(txtSocialOrder.Text, 1);

                RunElevated(rootWeb =>
                {
                    SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_FOLLOWUS);
                    WithUnsafeUpdates(rootWeb, () =>
                    {
                        SPListItem item = id > 0 ? list.GetItemById(id) : list.Items.Add();

                        item["Title"] = key;
                        item["IconClass"] = txtSocialIconClass.Text.Trim();
                        if (!string.IsNullOrEmpty(url))
                            item["URL"] = new SPFieldUrlValue { Url = url, Description = key };
                        else
                            item["URL"] = null;

                        item["AriaLabelAr"] = txtSocialAriaAr.Text.Trim();
                        item["AriaLabelEn"] = txtSocialAriaEn.Text.Trim();
                        item["ParentKey"] = txtSocialParentKey.Text.Trim();
                        item["Visibility"] = chkSocialVisible.Checked;
                        item["ItemOrder"] = order;
                        item.Update();
                    });
                });

                FooterCache.Invalidate();
                pnlSocialForm.Visible = false;
                hfSocialId.Value = "0";
                BindSocialGrid();
                ShowMessage("تم حفظ منصة التواصل بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء حفظ منصة التواصل: " + Enc(ex.Message));
            }
        }

        protected void gvSocial_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!_isAdmin) return;
            int id = ParseId(Convert.ToString(e.CommandArgument));
            if (id <= 0) return;

            try
            {
                if (e.CommandName == "EditItem")
                {
                    RunElevated(rootWeb =>
                    {
                        SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_FOLLOWUS);
                        SPListItem item = list.GetItemById(id);

                        hfSocialId.Value = id.ToString(CultureInfo.InvariantCulture);
                        ltSocialFormTitle.Text = "تعديل حساب: " + Enc(SafeString(item["Title"]));
                        txtSocialTitle.Text = SafeString(item["Title"]);
                        txtSocialIconClass.Text = SafeString(item["IconClass"]);
                        txtSocialUrl.Text = new SPFieldUrlValue(SafeString(item["URL"])).Url;
                        txtSocialAriaAr.Text = SafeString(item["AriaLabelAr"]);
                        txtSocialAriaEn.Text = SafeString(item["AriaLabelEn"]);
                        txtSocialParentKey.Text = SafeString(item["ParentKey"]);
                        txtSocialOrder.Text = ParseOrder(SafeString(item["ItemOrder"]), 1).ToString(CultureInfo.InvariantCulture);
                        chkSocialVisible.Checked = SafeBool(item["Visibility"]);

                        pnlSocialForm.Visible = true;
                    });
                }
                else if (e.CommandName == "ToggleVisibility")
                {
                    ToggleVisibility(FooterListProvisioner.LIST_FOLLOWUS, id);
                    BindSocialGrid();
                    ShowMessage("تم تغيير حالة المنصة بنجاح.");
                }
                else if (e.CommandName == "DeleteItem")
                {
                    DeleteItem(FooterListProvisioner.LIST_FOLLOWUS, id);
                    if (hfSocialId.Value == id.ToString(CultureInfo.InvariantCulture))
                    {
                        pnlSocialForm.Visible = false;
                        hfSocialId.Value = "0";
                    }
                    BindSocialGrid();
                    ShowMessage("تم حذف المنصة بنجاح.");
                }
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("تعذر تنفيذ العملية على المنصة: " + Enc(ex.Message));
            }
        }

        protected void gvSocial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;
                bool isVisible = Convert.ToBoolean(row["Visibility"]);

                Label lblVis = (Label)e.Row.FindControl("lblSocialVisible");
                if (lblVis != null)
                {
                    lblVis.Text = isVisible
                        ? "<span class='badge bg-success-subtle text-success'>مفعل</span>"
                        : "<span class='badge bg-danger-subtle text-danger'>معطل</span>";
                }
            }
        }

        #endregion

        #region Tab 4: Bottom Links & Logos

        private void LoadSettings()
        {
            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = rootWeb.Lists.TryGetList(FooterListProvisioner.LIST_SETTINGS);
                    if (list == null || list.ItemCount == 0) return;

                    SPQuery q = new SPQuery { RowLimit = 1 };
                    foreach (SPListItem item in list.GetItems(q))
                    {
                        txtSdaiaUrlAr.Text = new SPFieldUrlValue(SafeString(item["SdaiaUrlAr"])).Url;
                        txtSdaiaUrlEn.Text = new SPFieldUrlValue(SafeString(item["SdaiaUrlEn"])).Url;
                        txtVisionUrlAr.Text = new SPFieldUrlValue(SafeString(item["VisionUrlAr"])).Url;
                        txtVisionUrlEn.Text = new SPFieldUrlValue(SafeString(item["VisionUrlEn"])).Url;
                        txtPnuUrlAr.Text = new SPFieldUrlValue(SafeString(item["PnuHomeUrlAr"])).Url;
                        txtPnuUrlEn.Text = new SPFieldUrlValue(SafeString(item["PnuHomeUrlEn"])).Url;

                        txtCopyrightAr.Text = SafeString(item["CopyrightAr"]);
                        txtCopyrightEn.Text = SafeString(item["CopyrightEn"]);
                        txtDevelopedAr.Text = SafeString(item["DevelopedByAr"]);
                        txtDevelopedEn.Text = SafeString(item["DevelopedByEn"]);
                        break;
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل إعدادات الشعارات: " + Enc(ex.Message));
            }
        }

        protected void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            string sdaiaAr = txtSdaiaUrlAr.Text.Trim(), sdaiaEn = txtSdaiaUrlEn.Text.Trim();
            string visionAr = txtVisionUrlAr.Text.Trim(), visionEn = txtVisionUrlEn.Text.Trim();
            string pnuAr = txtPnuUrlAr.Text.Trim(), pnuEn = txtPnuUrlEn.Text.Trim();

            try
            {
                RequireSafeUrl(sdaiaAr, "رابط سدايا (عربي)");
                RequireSafeUrl(sdaiaEn, "رابط سدايا (إنجليزي)");
                RequireSafeUrl(visionAr, "رابط رؤية 2030 (عربي)");
                RequireSafeUrl(visionEn, "رابط رؤية 2030 (إنجليزي)");
                RequireSafeUrl(pnuAr, "رابط الجامعة (عربي)");
                RequireSafeUrl(pnuEn, "رابط الجامعة (إنجليزي)");

                RunElevated(rootWeb =>
                {
                    SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_SETTINGS);
                    WithUnsafeUpdates(rootWeb, () =>
                    {
                        SPQuery q = new SPQuery { RowLimit = 1 };
                        SPListItemCollection rows = list.GetItems(q);
                        SPListItem item = rows.Count > 0 ? rows[0] : list.Items.Add();

                        item["Title"] = "General";
                        item["SdaiaUrlAr"] = UrlValue(sdaiaAr, "SDAIA AR");
                        item["SdaiaUrlEn"] = UrlValue(sdaiaEn, "SDAIA EN");
                        item["VisionUrlAr"] = UrlValue(visionAr, "Vision 2030 AR");
                        item["VisionUrlEn"] = UrlValue(visionEn, "Vision 2030 EN");
                        item["PnuHomeUrlAr"] = UrlValue(pnuAr, "PNU AR");
                        item["PnuHomeUrlEn"] = UrlValue(pnuEn, "PNU EN");
                        item["CopyrightAr"] = txtCopyrightAr.Text.Trim();
                        item["CopyrightEn"] = txtCopyrightEn.Text.Trim();
                        item["DevelopedByAr"] = txtDevelopedAr.Text.Trim();
                        item["DevelopedByEn"] = txtDevelopedEn.Text.Trim();
                        item.Update();
                    });
                });

                FooterCache.Invalidate();
                ShowMessage("تم حفظ إعدادات الشعارات وحقوق النشر بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء حفظ إعدادات الشعارات: " + Enc(ex.Message));
            }
        }

        private static SPFieldUrlValue UrlValue(string url, string description)
        {
            return string.IsNullOrEmpty(url) ? null : new SPFieldUrlValue { Url = url, Description = description };
        }

        private void BindDownLinksGrid()
        {
            try
            {
                RunElevated(rootWeb =>
                {
                    SPList list = rootWeb.Lists.TryGetList(FooterListProvisioner.LIST_DOWNLINKS);
                    if (list == null) return;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Title");
                    dt.Columns.Add("TitleEn");
                    dt.Columns.Add("UrlAr");
                    dt.Columns.Add("UrlEn");
                    dt.Columns.Add("OpenInNewTab", typeof(bool));
                    dt.Columns.Add("Visibility", typeof(bool));
                    dt.Columns.Add("ItemOrder", typeof(int));

                    SPQuery q = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>" };
                    foreach (SPListItem item in list.GetItems(q))
                    {
                        SPFieldUrlValue uAr = new SPFieldUrlValue(SafeString(item["URLAr"]));
                        SPFieldUrlValue uEn = new SPFieldUrlValue(SafeString(item["URLEn"]));

                        dt.Rows.Add(
                            item.ID,
                            SafeString(item["Title"]),
                            SafeString(item["TitleEn"]),
                            uAr.Url ?? "",
                            uEn.Url ?? "",
                            SafeBool(item["OpenInNewTab"]),
                            SafeBool(item["Visibility"]),
                            ParseOrder(SafeString(item["ItemOrder"]), 1)
                        );
                    }

                    gvDownLinks.DataSource = dt;
                    gvDownLinks.DataBind();
                    lblDownLinkCount.Text = dt.Rows.Count + " رابط";
                });
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل الروابط السفلية: " + Enc(ex.Message));
            }
        }

        protected void btnNewDownLink_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            hfDownLinkId.Value = "0";
            ltDownLinkFormTitle.Text = "إضافة رابط سفلي جديد";
            txtDownLinkTitleAr.Text = "";
            txtDownLinkTitleEn.Text = "";
            txtDownLinkUrlAr.Text = "";
            txtDownLinkUrlEn.Text = "";
            txtDownLinkOrder.Text = "1";
            chkDownLinkNewTab.Checked = false;
            chkDownLinkVisible.Checked = true;
            pnlDownLinkForm.Visible = true;
        }

        protected void btnCancelDownLink_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            pnlDownLinkForm.Visible = false;
            hfDownLinkId.Value = "0";
        }

        protected void btnSaveDownLink_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            string tAr = txtDownLinkTitleAr.Text.Trim();
            string tEn = txtDownLinkTitleEn.Text.Trim();
            string uAr = txtDownLinkUrlAr.Text.Trim();
            string uEn = txtDownLinkUrlEn.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(tAr))
                    throw new ValidationException("الرجاء إدخال عنوان الرابط بالعربية.");
                if (string.IsNullOrEmpty(uAr))
                    throw new ValidationException("الرجاء إدخال الرابط العربي.");
                RequireSafeUrl(uAr, "الرابط العربي");
                RequireSafeUrl(uEn, "الرابط الإنجليزي");

                int id = ParseId(hfDownLinkId.Value);
                int order = ParseOrder(txtDownLinkOrder.Text, 1);

                RunElevated(rootWeb =>
                {
                    SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_DOWNLINKS);
                    WithUnsafeUpdates(rootWeb, () =>
                    {
                        SPListItem item = id > 0 ? list.GetItemById(id) : list.Items.Add();

                        item["Title"] = tAr;
                        item["TitleEn"] = tEn;
                        item["URLAr"] = new SPFieldUrlValue { Url = uAr, Description = tAr };
                        item["URLEn"] = new SPFieldUrlValue { Url = string.IsNullOrEmpty(uEn) ? uAr : uEn, Description = string.IsNullOrEmpty(tEn) ? tAr : tEn };
                        item["OpenInNewTab"] = chkDownLinkNewTab.Checked;
                        item["Visibility"] = chkDownLinkVisible.Checked;
                        item["ItemOrder"] = order;
                        item.Update();
                    });
                });

                FooterCache.Invalidate();
                pnlDownLinkForm.Visible = false;
                hfDownLinkId.Value = "0";
                BindDownLinksGrid();
                ShowMessage("تم حفظ الرابط السفلي بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء حفظ الرابط السفلي: " + Enc(ex.Message));
            }
        }

        protected void gvDownLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!_isAdmin) return;
            int id = ParseId(Convert.ToString(e.CommandArgument));
            if (id <= 0) return;

            try
            {
                if (e.CommandName == "EditItem")
                {
                    RunElevated(rootWeb =>
                    {
                        SPList list = GetListOrThrow(rootWeb, FooterListProvisioner.LIST_DOWNLINKS);
                        SPListItem item = list.GetItemById(id);

                        hfDownLinkId.Value = id.ToString(CultureInfo.InvariantCulture);
                        ltDownLinkFormTitle.Text = "تعديل الرابط: " + Enc(SafeString(item["Title"]));
                        txtDownLinkTitleAr.Text = SafeString(item["Title"]);
                        txtDownLinkTitleEn.Text = SafeString(item["TitleEn"]);
                        txtDownLinkUrlAr.Text = new SPFieldUrlValue(SafeString(item["URLAr"])).Url;
                        txtDownLinkUrlEn.Text = new SPFieldUrlValue(SafeString(item["URLEn"])).Url;
                        txtDownLinkOrder.Text = ParseOrder(SafeString(item["ItemOrder"]), 1).ToString(CultureInfo.InvariantCulture);
                        chkDownLinkNewTab.Checked = SafeBool(item["OpenInNewTab"]);
                        chkDownLinkVisible.Checked = SafeBool(item["Visibility"]);

                        pnlDownLinkForm.Visible = true;
                    });
                }
                else if (e.CommandName == "ToggleVisibility")
                {
                    ToggleVisibility(FooterListProvisioner.LIST_DOWNLINKS, id);
                    BindDownLinksGrid();
                    ShowMessage("تم تغيير حالة الرابط بنجاح.");
                }
                else if (e.CommandName == "DeleteItem")
                {
                    DeleteItem(FooterListProvisioner.LIST_DOWNLINKS, id);
                    if (hfDownLinkId.Value == id.ToString(CultureInfo.InvariantCulture))
                    {
                        pnlDownLinkForm.Visible = false;
                        hfDownLinkId.Value = "0";
                    }
                    BindDownLinksGrid();
                    ShowMessage("تم حذف الرابط بنجاح.");
                }
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("تعذر تنفيذ العملية على الرابط: " + Enc(ex.Message));
            }
        }

        protected void gvDownLinks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;
                bool isVisible = Convert.ToBoolean(row["Visibility"]);

                Label lblVis = (Label)e.Row.FindControl("lblDownVisible");
                if (lblVis != null)
                {
                    lblVis.Text = isVisible
                        ? "<span class='badge bg-success-subtle text-success'>مفعل</span>"
                        : "<span class='badge bg-danger-subtle text-danger'>معطل</span>";
                }
            }
        }

        #endregion

        #region Shared row actions

        private void ToggleVisibility(string listName, int id)
        {
            RunElevated(rootWeb =>
            {
                SPList list = GetListOrThrow(rootWeb, listName);
                SPListItem item = list.GetItemById(id);
                WithUnsafeUpdates(rootWeb, () =>
                {
                    item["Visibility"] = !SafeBool(item["Visibility"]);
                    item.Update();
                });
            });
            FooterCache.Invalidate();
        }

        private void DeleteItem(string listName, int id)
        {
            RunElevated(rootWeb =>
            {
                SPList list = GetListOrThrow(rootWeb, listName);
                SPListItem item = list.GetItemById(id);
                WithUnsafeUpdates(rootWeb, () => item.Delete());
            });
            FooterCache.Invalidate();
        }

        #endregion

        #region Helpers

        private static void RunElevated(Action<SPWeb> action)
        {
            Guid siteId = SPContext.Current.Site.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(siteId))
                {
                    action(site.RootWeb); // RootWeb is owned by SPSite: do not dispose
                }
            });
        }

        private static void WithUnsafeUpdates(SPWeb web, Action action)
        {
            bool previous = web.AllowUnsafeUpdates;
            web.AllowUnsafeUpdates = true;
            try
            {
                action();
            }
            finally
            {
                web.AllowUnsafeUpdates = previous;
            }
        }

        private static SPList GetListOrThrow(SPWeb web, string listName)
        {
            SPList list = web.Lists.TryGetList(listName);
            if (list == null)
                throw new ValidationException("القائمة " + listName + " غير موجودة. استخدم زر تهيئة القوائم أولاً.");
            return list;
        }

        private static void RequireSafeUrl(string url, string label)
        {
            if (!string.IsNullOrEmpty(url) && !SafeHtml.IsSafeUrl(url))
                throw new ValidationException(string.Format(InvalidUrlMessage, label));
        }

        // Messages are rendered as HTML — callers encode any list/user/exception text they include
        private void ShowMessage(string msg)
        {
            ltMessage.Text = msg;
            pnlMessage.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string err)
        {
            ltError.Text = err;
            pnlError.Visible = true;
            pnlMessage.Visible = false;
        }

        private void ClearAlerts()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
        }

        private static string SafeString(object v)
        {
            return v == null ? "" : v.ToString();
        }

        private static bool SafeBool(object v)
        {
            if (v == null) return false;
            if (v is bool) return (bool)v;
            bool b;
            return bool.TryParse(v.ToString(), out b) && b;
        }

        private static int ParseId(string value)
        {
            int id;
            return int.TryParse(value, out id) ? id : 0;
        }

        private static int ParseOrder(string value, int fallback)
        {
            double d;
            string s = (value ?? "").Trim();
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out d)) return (int)d;
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out d)) return (int)d;
            return fallback;
        }

        private static string Enc(string s)
        {
            return string.IsNullOrEmpty(s) ? "" : HttpUtility.HtmlEncode(s);
        }

        // Markup helpers (grid output)
        public static string Text(object value) { return SafeHtml.Text(value); }
        public static string Attr(object value) { return SafeHtml.Attr(value); }
        public static string Href(object value) { return SafeHtml.Href(value); }

        private static string CurrentUrl()
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                    return HttpContext.Current.Request.Url.ToString();
            }
            catch { }
            return "ucHomeFooterAdmin";
        }

        /// <summary>User-facing validation error (shown as-is, not logged).</summary>
        private class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }

        #endregion
    }
}
