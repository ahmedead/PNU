using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.Admin
{
    public partial class ucEservicesAdmin : UserControl
    {
        private const string ListName = "EservicesList";
        private const string DefaultSlaEnUrl = "https://pnu.edu.sa/en/Pages/SLA.aspx";
        private const string ModeNew = "New";
        private const string ModeEdit = "Edit";
        private const string ListNotFoundMessage = "تعذر العثور على قائمة الخدمات الإلكترونية EservicesList.";

        private bool _isAdmin;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                _isAdmin = ContentAdm.IsAdmin();
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

                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    ServiceDetailsDgaProvisioner.EnsureAll(SPContext.Current.Site.Url);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.OnInit", ex.Message);
                ShowError("حدث خطأ أثناء فحص الصلاحيات أو تهيئة قائمة الخدمات: " + ex.Message);
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
                    LoadFilterTargetGroups();
                    BindStats();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.Page_Load", ex.Message);
                ShowError(ex.Message);
            }
        }

        #region Helper: Locate EservicesList

        /// <summary>
        /// Runs <paramref name="action"/> elevated against EservicesList on the root web —
        /// the same list ucDGAServiceDetails, ucEserviceBrow and ServiceDetailsDgaProvisioner use.
        /// Returns false when there is no context or the list does not exist.
        /// </summary>
        private static bool WithEservicesList(bool forWrite, Action<SPList> action)
        {
            if (SPContext.Current == null || SPContext.Current.Site == null) return false;

            Guid siteId = SPContext.Current.Site.ID;
            bool found = false;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    // RootWeb belongs to the SPSite and is disposed with it
                    SPWeb web = site.RootWeb;
                    SPList list = web.Lists.TryGetList(ListName);
                    if (list == null) return;
                    found = true;

                    if (forWrite) web.AllowUnsafeUpdates = true;
                    try
                    {
                        action(list);
                    }
                    finally
                    {
                        if (forWrite) web.AllowUnsafeUpdates = false;
                    }
                }
            });

            return found;
        }

        private static SPListItemCollection GetAllItems(SPList list)
        {
            return list.GetItems(new SPQuery());
        }

        #endregion

        #region Statistics & Filtering

        private void BindStats()
        {
            try
            {
                WithEservicesList(false, list =>
                {
                    SPListItemCollection allItems = GetAllItems(list);
                    int total = allItems.Count;
                    int active = 0;
                    int home = 0;
                    int sla = 0;

                    foreach (SPListItem itm in allItems)
                    {
                        if (GetBool(itm, "Active")) active++;
                        if (GetBool(itm, "IsHome")) home++;

                        string slaAr = SafeStr(itm, "ServiceAgreement");
                        if (string.IsNullOrEmpty(slaAr)) slaAr = SafeStr(itm, "ServiceAgreementURL");
                        string slaEn = SafeStr(itm, "ServiceAgreementURL_EN");

                        if (!string.IsNullOrEmpty(slaAr) || !string.IsNullOrEmpty(slaEn))
                            sla++;
                    }

                    ltTotalCount.Text = total.ToString(CultureInfo.InvariantCulture);
                    ltActiveCount.Text = active.ToString(CultureInfo.InvariantCulture);
                    ltHomeCount.Text = home.ToString(CultureInfo.InvariantCulture);
                    ltSlaCount.Text = sla.ToString(CultureInfo.InvariantCulture);
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.BindStats", ex.Message);
            }
        }

        private void LoadFilterTargetGroups()
        {
            try
            {
                ddlFilterTargetGroup.Items.Clear();
                ddlFilterTargetGroup.Items.Add(new ListItem("جميع الفئات المستهدفة", ""));

                WithEservicesList(false, list =>
                {
                    HashSet<string> groups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (SPListItem itm in GetAllItems(list))
                    {
                        string tg = SafeStr(itm, "TargetGroup");
                        if (!string.IsNullOrWhiteSpace(tg))
                        {
                            groups.Add(tg.Trim());
                        }
                    }

                    foreach (string g in groups.OrderBy(x => x))
                    {
                        ddlFilterTargetGroup.Items.Add(new ListItem(g, g));
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.LoadFilterTargetGroups", ex.Message);
            }
        }

        private void BindGrid()
        {
            try
            {
                var dataList = new List<EServiceAdminViewModel>();

                string search = (txtSearch.Text ?? "").Trim();
                string filterTg = ddlFilterTargetGroup.SelectedValue;
                string filterActive = ddlFilterActive.SelectedValue;
                string filterHome = ddlFilterHome.SelectedValue;

                bool found = WithEservicesList(false, list =>
                {
                    foreach (SPListItem itm in GetAllItems(list))
                    {
                        string arName = SafeStr(itm, "ARServiceName");
                        string enName = SafeStr(itm, "ENServiceName");
                        string title = SafeStr(itm, "Title");
                        string desc = SafeStr(itm, "Desc");
                        string targetGroup = SafeStr(itm, "TargetGroup");
                        bool active = GetBool(itm, "Active");
                        bool isHome = GetBool(itm, "IsHome");
                        bool nafath = GetBool(itm, "NafathLogin");
                        int order = GetInt(itm, "ItemOrder", 9999);

                        // Filter: Search
                        if (!string.IsNullOrEmpty(search))
                        {
                            bool matches = (arName != null && arName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                                        || (enName != null && enName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                                        || (title != null && title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                                        || (desc != null && desc.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                            if (!matches) continue;
                        }

                        // Filter: TargetGroup
                        if (!string.IsNullOrEmpty(filterTg))
                        {
                            if (!string.Equals(targetGroup, filterTg, StringComparison.OrdinalIgnoreCase))
                                continue;
                        }

                        // Filter: Active
                        if (!string.IsNullOrEmpty(filterActive))
                        {
                            bool reqActive = filterActive == "1";
                            if (active != reqActive) continue;
                        }

                        // Filter: Home
                        if (!string.IsNullOrEmpty(filterHome))
                        {
                            bool reqHome = filterHome == "1";
                            if (isHome != reqHome) continue;
                        }

                        string slaAr = SafeStr(itm, "ServiceAgreement");
                        if (string.IsNullOrEmpty(slaAr)) slaAr = SafeStr(itm, "ServiceAgreementURL");
                        string slaEn = SafeStr(itm, "ServiceAgreementURL_EN");

                        dataList.Add(new EServiceAdminViewModel
                        {
                            ID = itm.ID,
                            ARServiceName = !string.IsNullOrEmpty(arName) ? arName : title,
                            ENServiceName = enName,
                            TargetGroup = targetGroup,
                            Active = active,
                            IsHome = isHome,
                            NafathLogin = nafath,
                            ItemOrder = order,
                            URL = SafeStr(itm, "URL"),
                            URL_EN = SafeStr(itm, "URL_EN"),
                            ServiceAgreement = slaAr,
                            ServiceAgreementURL_EN = slaEn
                        });
                    }
                });

                if (!found)
                {
                    ShowError(ListNotFoundMessage);
                }

                var sorted = dataList.OrderBy(x => x.ItemOrder)
                                     .ThenBy(x => x.ARServiceName)
                                     .ToList();

                ltFilteredCount.Text = sorted.Count.ToString(CultureInfo.InvariantCulture);
                rptServices.DataSource = sorted;
                rptServices.DataBind();

                pnlEmpty.Visible = (sorted.Count == 0);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.BindGrid", ex.Message);
                ShowError("حدث خطأ أثناء تحميل بيانات الخدمات: " + ex.Message);
            }
        }

        #endregion

        #region Toolbar & Filter Actions

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            BindGrid();
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            txtSearch.Text = string.Empty;
            ddlFilterTargetGroup.SelectedIndex = 0;
            ddlFilterActive.SelectedIndex = 0;
            ddlFilterHome.SelectedIndex = 0;
            BindGrid();
        }

        protected void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            BindGrid();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            BindStats();
            BindGrid();
        }

        protected void btnProvision_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            try
            {
                ServiceDetailsDgaProvisioner.ForceProvision();
                BindStats();
                LoadFilterTargetGroups();
                BindGrid();
                ShowMessage("تم فحص وتحديث حقول قائمة الخدمات الإلكترونية بنجاح بما فيها حقل اتفاقية مستوى الخدمة بالإنجليزية (ServiceAgreementURL_EN).");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.btnProvision_Click", ex.Message);
                ShowError("تعذر فحص وتهيئة الحقول: " + ex.Message);
            }
        }

        #endregion

        #region Form Actions (New, Edit, Save, Delete)

        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            OpenFormNew();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            CloseForm();
        }

        private void OpenFormNew()
        {
            hfMode.Value = ModeNew;
            hfEditingId.Value = string.Empty;
            ltFormTitle.Text = "إضافة خدمة إلكترونية جديدة";

            // Reset basic fields
            txtARServiceName.Text = string.Empty;
            txtENServiceName.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtItemOrder.Text = "1";
            chkActive.Checked = true;
            chkIsHome.Checked = false;
            chkNafathLogin.Checked = false;
            txtDesc.Text = string.Empty;
            txtDesc_EN.Text = string.Empty;

            // URLs & SLA
            txtURL.Text = string.Empty;
            txtURL_EN.Text = string.Empty;
            txtServiceAgreement.Text = string.Empty;
            txtServiceAgreementURL_EN.Text = DefaultSlaEnUrl; // new services only — existing ones keep their value
            txtUserManualURL.Text = string.Empty;
            txtRulesAndRegulationsURL.Text = string.Empty;

            // Details & Specs
            txtTargetGroup.Text = string.Empty;
            txtTargetGroup_EN.Text = string.Empty;
            txtDP_TargetGroup.Text = string.Empty;
            txtDP_TargetGroup_EN.Text = string.Empty;
            txtResponsibleParty.Text = string.Empty;
            txtResponsibleParty_EN.Text = string.Empty;
            txtTimeDuration.Text = string.Empty;
            txtTimeDuration_EN.Text = string.Empty;
            txtChannels.Text = string.Empty;
            txtChannels_EN.Text = string.Empty;
            txtServiceLanguage.Text = string.Empty;
            txtServiceLanguage_EN.Text = string.Empty;
            txtServiceCost.Text = string.Empty;
            txtServiceCost_EN.Text = string.Empty;

            // Badges & Requirements
            txtBadge1.Text = string.Empty;
            txtBadge1_EN.Text = string.Empty;
            txtBadge2.Text = string.Empty;
            txtBadge2_EN.Text = string.Empty;
            txtBadge3.Text = string.Empty;
            txtBadge3_EN.Text = string.Empty;

            txtReq1.Text = string.Empty;
            txtReq1_EN.Text = string.Empty;
            txtReq2.Text = string.Empty;
            txtReq2_EN.Text = string.Empty;
            txtReq3.Text = string.Empty;
            txtReq3_EN.Text = string.Empty;

            txtSteps.Text = string.Empty;
            txtSteps_EN.Text = string.Empty;
            txtRequiredDocs.Text = string.Empty;
            txtRequiredDocs_EN.Text = string.Empty;

            pnlForm.Visible = true;
        }

        private void CloseForm()
        {
            pnlForm.Visible = false;
            hfMode.Value = ModeNew;
            hfEditingId.Value = string.Empty;
        }

        protected void rptServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            int itemId;
            if (!int.TryParse(e.CommandArgument?.ToString(), out itemId) || itemId <= 0)
                return;

            if (string.Equals(e.CommandName, "EditItem", StringComparison.OrdinalIgnoreCase))
            {
                LoadForEdit(itemId);
            }
            else if (string.Equals(e.CommandName, "DeleteItem", StringComparison.OrdinalIgnoreCase))
            {
                DeleteItem(itemId);
            }
        }

        private void LoadForEdit(int itemId)
        {
            try
            {
                bool itemFound = false;
                bool listFound = WithEservicesList(false, list =>
                {
                    SPListItem itm = FindItem(list, itemId);
                    if (itm == null) return;
                    itemFound = true;

                    hfMode.Value = ModeEdit;
                    hfEditingId.Value = itemId.ToString(CultureInfo.InvariantCulture);
                    ltFormTitle.Text = HttpUtility.HtmlEncode("تعديل الخدمة: " + (SafeStr(itm, "ARServiceName") ?? SafeStr(itm, "Title")));

                    // Basic
                    txtARServiceName.Text = SafeStr(itm, "ARServiceName");
                    txtENServiceName.Text = SafeStr(itm, "ENServiceName");
                    txtTitle.Text = SafeStr(itm, "Title");
                    txtItemOrder.Text = GetInt(itm, "ItemOrder", 1).ToString(CultureInfo.InvariantCulture);
                    chkActive.Checked = GetBool(itm, "Active");
                    chkIsHome.Checked = GetBool(itm, "IsHome");
                    chkNafathLogin.Checked = GetBool(itm, "NafathLogin");
                    txtDesc.Text = SafeStr(itm, "Desc");
                    txtDesc_EN.Text = SafeStr(itm, "Desc_EN");

                    // URLs & SLA
                    txtURL.Text = SafeStr(itm, "URL");
                    txtURL_EN.Text = SafeStr(itm, "URL_EN");

                    string slaAr = SafeStr(itm, "ServiceAgreement");
                    if (string.IsNullOrEmpty(slaAr)) slaAr = SafeStr(itm, "ServiceAgreementURL");
                    txtServiceAgreement.Text = slaAr;

                    // Left empty when empty: an empty EN link falls back to the AR link on the EN page,
                    // so pre-filling the default here would overwrite that on every save.
                    txtServiceAgreementURL_EN.Text = SafeStr(itm, "ServiceAgreementURL_EN");

                    txtUserManualURL.Text = SafeStr(itm, "UserManualURL");
                    txtRulesAndRegulationsURL.Text = SafeStr(itm, "RulesAndRegulationsURL");

                    // Details & Specs
                    txtTargetGroup.Text = SafeStr(itm, "TargetGroup");
                    txtTargetGroup_EN.Text = SafeStr(itm, "TargetGroup_EN");
                    txtDP_TargetGroup.Text = SafeStr(itm, "DP_TargetGroup");
                    txtDP_TargetGroup_EN.Text = SafeStr(itm, "DP_TargetGroup_EN");
                    txtResponsibleParty.Text = SafeStr(itm, "ResponsibleParty");
                    txtResponsibleParty_EN.Text = SafeStr(itm, "ResponsibleParty_EN");
                    txtTimeDuration.Text = SafeStr(itm, "TimeDuration");
                    txtTimeDuration_EN.Text = SafeStr(itm, "TimeDuration_EN");
                    txtChannels.Text = SafeStr(itm, "Channels");
                    txtChannels_EN.Text = SafeStr(itm, "Channels_EN");
                    txtServiceLanguage.Text = SafeStr(itm, "ServiceLanguage");
                    txtServiceLanguage_EN.Text = SafeStr(itm, "ServiceLanguage_EN");
                    txtServiceCost.Text = SafeStr(itm, "ServiceCost");
                    txtServiceCost_EN.Text = SafeStr(itm, "ServiceCost_EN");

                    // Badges & Requirements
                    txtBadge1.Text = SafeStr(itm, "Badge1");
                    txtBadge1_EN.Text = SafeStr(itm, "Badge1_EN");
                    txtBadge2.Text = SafeStr(itm, "Badge2");
                    txtBadge2_EN.Text = SafeStr(itm, "Badge2_EN");
                    txtBadge3.Text = SafeStr(itm, "Badge3");
                    txtBadge3_EN.Text = SafeStr(itm, "Badge3_EN");

                    txtReq1.Text = SafeStr(itm, "_x0052_eq1");
                    txtReq1_EN.Text = SafeStr(itm, "Req1_EN");
                    txtReq2.Text = SafeStr(itm, "_x0052_eq2");
                    txtReq2_EN.Text = SafeStr(itm, "Req2_EN");
                    txtReq3.Text = SafeStr(itm, "_x0052_eq3");
                    txtReq3_EN.Text = SafeStr(itm, "Req3_EN");

                    txtSteps.Text = SafeStr(itm, "Steps");
                    txtSteps_EN.Text = SafeStr(itm, "Steps_EN");
                    txtRequiredDocs.Text = SafeStr(itm, "RequiredDocs");
                    txtRequiredDocs_EN.Text = SafeStr(itm, "RequiredDocs_EN");

                    pnlForm.Visible = true;
                });

                if (!listFound)
                    ShowError(ListNotFoundMessage);
                else if (!itemFound)
                    ShowError("تعذر العثور على الخدمة برقم: " + itemId);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.LoadForEdit", ex.Message);
                ShowError("تعذر تحميل بيانات الخدمة: " + ex.Message);
            }
        }

        private void DeleteItem(int itemId)
        {
            try
            {
                bool deleted = false;
                bool listFound = WithEservicesList(true, list =>
                {
                    SPListItem itm = FindItem(list, itemId);
                    if (itm == null) return;
                    itm.Delete();
                    deleted = true;
                });

                if (!listFound)
                    ShowError(ListNotFoundMessage);
                else if (!deleted)
                    ShowError("تعذر العثور على الخدمة برقم: " + itemId);
                else
                    ShowMessage("تم حذف الخدمة بنجاح.");

                BindStats();
                BindGrid();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.DeleteItem", ex.Message);
                ShowError("حدث خطأ أثناء حذف الخدمة: " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();

            string arName = (txtARServiceName.Text ?? "").Trim();
            if (string.IsNullOrEmpty(arName))
            {
                ShowError("يرجى إدخال اسم الخدمة بالعربية.");
                return;
            }

            int itemOrder = 1;
            int.TryParse(txtItemOrder.Text.Trim(), out itemOrder);

            bool isNew = string.Equals(hfMode.Value, ModeNew, StringComparison.OrdinalIgnoreCase);
            int itemId = 0;
            int.TryParse(hfEditingId.Value, out itemId);

            try
            {
                bool saved = false;
                bool listFound = WithEservicesList(true, list =>
                {
                    SPListItem itm = isNew ? list.Items.Add() : FindItem(list, itemId);
                    if (itm == null) return;

                    string codeTitle = (txtTitle.Text ?? "").Trim();
                    if (string.IsNullOrEmpty(codeTitle))
                        codeTitle = arName;

                    // Title & Basic Info
                    SetField(list, itm, "Title", codeTitle);
                    SetField(list, itm, "ARServiceName", arName);
                    SetField(list, itm, "ENServiceName", (txtENServiceName.Text ?? "").Trim());
                    SetField(list, itm, "ItemOrder", itemOrder);
                    SetField(list, itm, "Order", itemOrder.ToString());
                    SetField(list, itm, "Active", chkActive.Checked);
                    SetField(list, itm, "IsHome", chkIsHome.Checked);
                    SetField(list, itm, "NafathLogin", chkNafathLogin.Checked);
                    SetField(list, itm, "Desc", (txtDesc.Text ?? "").Trim());
                    SetField(list, itm, "Desc_EN", (txtDesc_EN.Text ?? "").Trim());

                    // URLs & SLA
                    SetField(list, itm, "URL", (txtURL.Text ?? "").Trim());
                    SetField(list, itm, "URL_EN", (txtURL_EN.Text ?? "").Trim());

                    string slaAr = (txtServiceAgreement.Text ?? "").Trim();
                    SetField(list, itm, "ServiceAgreement", slaAr);
                    SetField(list, itm, "ServiceAgreementURL", slaAr);

                    string slaEn = (txtServiceAgreementURL_EN.Text ?? "").Trim();
                    SetField(list, itm, "ServiceAgreementURL_EN", slaEn);

                    SetField(list, itm, "UserManualURL", (txtUserManualURL.Text ?? "").Trim());
                    SetField(list, itm, "RulesAndRegulationsURL", (txtRulesAndRegulationsURL.Text ?? "").Trim());

                    // Details & Specifications
                    string tg = (txtTargetGroup.Text ?? "").Trim();
                    SetField(list, itm, "TargetGroup", tg);
                    SetField(list, itm, "TargetGroup_EN", (txtTargetGroup_EN.Text ?? "").Trim());
                    SetField(list, itm, "DP_TargetGroup", (txtDP_TargetGroup.Text ?? "").Trim());
                    SetField(list, itm, "DP_TargetGroup_EN", (txtDP_TargetGroup_EN.Text ?? "").Trim());

                    SetField(list, itm, "ResponsibleParty", (txtResponsibleParty.Text ?? "").Trim());
                    SetField(list, itm, "ResponsibleParty_EN", (txtResponsibleParty_EN.Text ?? "").Trim());
                    SetField(list, itm, "TimeDuration", (txtTimeDuration.Text ?? "").Trim());
                    SetField(list, itm, "TimeDuration_EN", (txtTimeDuration_EN.Text ?? "").Trim());
                    SetField(list, itm, "Channels", (txtChannels.Text ?? "").Trim());
                    SetField(list, itm, "Channels_EN", (txtChannels_EN.Text ?? "").Trim());
                    SetField(list, itm, "ServiceLanguage", (txtServiceLanguage.Text ?? "").Trim());
                    SetField(list, itm, "ServiceLanguage_EN", (txtServiceLanguage_EN.Text ?? "").Trim());
                    SetField(list, itm, "ServiceCost", (txtServiceCost.Text ?? "").Trim());
                    SetField(list, itm, "ServiceCost_EN", (txtServiceCost_EN.Text ?? "").Trim());

                    // Badges & Requirements
                    SetField(list, itm, "Badge1", (txtBadge1.Text ?? "").Trim());
                    SetField(list, itm, "Badge1_EN", (txtBadge1_EN.Text ?? "").Trim());
                    SetField(list, itm, "Badge2", (txtBadge2.Text ?? "").Trim());
                    SetField(list, itm, "Badge2_EN", (txtBadge2_EN.Text ?? "").Trim());
                    SetField(list, itm, "Badge3", (txtBadge3.Text ?? "").Trim());
                    SetField(list, itm, "Badge3_EN", (txtBadge3_EN.Text ?? "").Trim());

                    SetField(list, itm, "_x0052_eq1", (txtReq1.Text ?? "").Trim());
                    SetField(list, itm, "Req1_EN", (txtReq1_EN.Text ?? "").Trim());
                    SetField(list, itm, "_x0052_eq2", (txtReq2.Text ?? "").Trim());
                    SetField(list, itm, "Req2_EN", (txtReq2_EN.Text ?? "").Trim());
                    SetField(list, itm, "_x0052_eq3", (txtReq3.Text ?? "").Trim());
                    SetField(list, itm, "Req3_EN", (txtReq3_EN.Text ?? "").Trim());

                    // Steps & Documents
                    SetField(list, itm, "Steps", (txtSteps.Text ?? "").Trim());
                    SetField(list, itm, "Steps_EN", (txtSteps_EN.Text ?? "").Trim());
                    SetField(list, itm, "RequiredDocs", (txtRequiredDocs.Text ?? "").Trim());
                    SetField(list, itm, "RequiredDocs_EN", (txtRequiredDocs_EN.Text ?? "").Trim());

                    itm.Update();
                    saved = true;
                });

                if (!listFound)
                {
                    ShowError(ListNotFoundMessage);
                    return;
                }
                if (!saved)
                {
                    ShowError("تعذر العثور على عنصر الخدمة المراد حفظه.");
                    return;
                }

                ShowMessage(isNew ? "تمت إضافة الخدمة الإلكترونية بنجاح." : "تم تحديث بيانات الخدمة بنجاح.");
                CloseForm();
                BindStats();
                LoadFilterTargetGroups();
                BindGrid();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucEservicesAdmin.btnSave_Click", ex.Message);
                ShowError("حدث خطأ أثناء حفظ بيانات الخدمة: " + ex.Message);
            }
        }

        #endregion

        #region Safe Field Helpers

        /// <summary>GetItemById throws for a missing ID; this returns null instead.</summary>
        private static SPListItem FindItem(SPList list, int itemId)
        {
            if (itemId <= 0) return null;
            try
            {
                return list.GetItemById(itemId);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private static void SetField(SPList list, SPListItem itm, string fieldName, object value)
        {
            if (list == null || itm == null || string.IsNullOrEmpty(fieldName)) return;
            if (!list.Fields.ContainsField(fieldName)) return;

            try
            {
                itm[fieldName] = value;
            }
            catch { }
        }

        private static string SafeStr(SPListItem itm, string fieldName)
        {
            if (itm == null || string.IsNullOrEmpty(fieldName)) return null;
            try
            {
                if (itm.Fields.ContainsField(fieldName) && itm[fieldName] != null)
                {
                    return itm[fieldName].ToString();
                }
            }
            catch { }
            return null;
        }

        private static bool GetBool(SPListItem itm, string fieldName)
        {
            if (itm == null || string.IsNullOrEmpty(fieldName)) return false;
            try
            {
                if (itm.Fields.ContainsField(fieldName) && itm[fieldName] != null)
                {
                    string s = itm[fieldName].ToString().Trim();
                    return string.Equals(s, "1", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(s, "true", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(s, "yes", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch { }
            return false;
        }

        private static int GetInt(SPListItem itm, string fieldName, int defaultValue = 0)
        {
            if (itm == null || string.IsNullOrEmpty(fieldName)) return defaultValue;
            try
            {
                if (itm.Fields.ContainsField(fieldName) && itm[fieldName] != null)
                {
                    int val;
                    if (int.TryParse(itm[fieldName].ToString(), out val))
                        return val;
                }
            }
            catch { }
            return defaultValue;
        }

        #endregion

        #region Grid Rendering Helpers

        /// <summary>HTML-encodes a bound value for the grid.</summary>
        protected string Enc(object value)
        {
            return HttpUtility.HtmlEncode(value == null ? "" : value.ToString());
        }

        /// <summary>
        /// Builds an encoded &lt;a target="_blank"&gt; for a list URL. Only http(s) and site-relative
        /// URLs become links, so a "javascript:" value saved in the list cannot run in the admin's browser.
        /// <paramref name="innerHtml"/> is trusted markup; when null the encoded URL is shown.
        /// </summary>
        protected string LinkHtml(object url, string cssClass, string innerHtml, string emptyHtml, string style = null)
        {
            string href = url == null ? "" : url.ToString().Trim();
            if (string.IsNullOrEmpty(href)) return emptyHtml ?? "";

            bool safe = href.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                     || href.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                     || (href.StartsWith("/") && !href.StartsWith("//"));
            if (!safe) return HttpUtility.HtmlEncode(href);

            return "<a href=\"" + HttpUtility.HtmlAttributeEncode(href) + "\" target=\"_blank\" rel=\"noopener\""
                 + " class=\"" + cssClass + "\""
                 + (string.IsNullOrEmpty(style) ? "" : " style=\"" + style + "\"")
                 + ">" + (innerHtml ?? HttpUtility.HtmlEncode(href)) + "</a>";
        }

        #endregion

        #region UI Message Banners

        private void ShowMessage(string message)
        {
            ltMessage.Text = HttpUtility.HtmlEncode(message);
            pnlMessage.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string error)
        {
            ltError.Text = HttpUtility.HtmlEncode(error);
            pnlError.Visible = true;
            pnlMessage.Visible = false;
        }

        private void ClearAlerts()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
            ltMessage.Text = string.Empty;
            ltError.Text = string.Empty;
        }

        private string CurrentUrl()
        {
            return HttpContext.Current != null && HttpContext.Current.Request != null
                ? HttpContext.Current.Request.Url.ToString()
                : string.Empty;
        }

        #endregion
    }

    public class EServiceAdminViewModel
    {
        public int ID { get; set; }
        public string ARServiceName { get; set; }
        public string ENServiceName { get; set; }
        public string TargetGroup { get; set; }
        public bool Active { get; set; }
        public bool IsHome { get; set; }
        public bool NafathLogin { get; set; }
        public int ItemOrder { get; set; }
        public string URL { get; set; }
        public string URL_EN { get; set; }
        public string ServiceAgreement { get; set; }
        public string ServiceAgreementURL_EN { get; set; }
    }
}
