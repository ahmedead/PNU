using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebPartPages;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucWebsitePagesDashboard : UserControl
    {
        private string CurrentTab
        {
            get { return ViewState["CurrentTab"] as string ?? "Overview"; }
            set { ViewState["CurrentTab"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebsitePagesReportDal.EnsureTablesExist();
                PopulateLayoutsDropdown();
                LoadDashboardOverview();
                SetActiveTab(CurrentTab);
            }
        }

        #region Tab Switching

        protected void SwitchTab_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            if (btn != null)
            {
                string tab = btn.CommandArgument;
                SetActiveTab(tab);
            }
        }

        private void SetActiveTab(string tab)
        {
            CurrentTab = tab;

            // Reset tab CSS
            btnTabOverview.CssClass = "nav-tab-btn";
            btnTabLayouts.CssClass = "nav-tab-btn";
            btnTabMissingPages.CssClass = "nav-tab-btn";
            btnTabUserControls.CssClass = "nav-tab-btn";
            btnTabSubsites.CssClass = "nav-tab-btn";
            btnTabLiveScan.CssClass = "nav-tab-btn";

            // Reset panel visibility
            pnlTabOverview.Visible = false;
            pnlTabLayouts.Visible = false;
            pnlTabMissingPages.Visible = false;
            pnlTabUserControls.Visible = false;
            pnlTabSubsites.Visible = false;
            pnlTabLiveScan.Visible = false;

            pnlAlert.Visible = false;

            switch (tab)
            {
                case "Overview":
                    btnTabOverview.CssClass = "nav-tab-btn active";
                    pnlTabOverview.Visible = true;
                    LoadDashboardOverview();
                    break;

                case "Layouts":
                    btnTabLayouts.CssClass = "nav-tab-btn active";
                    pnlTabLayouts.Visible = true;
                    LoadPagesByLayoutReport();
                    break;

                case "MissingPages":
                    btnTabMissingPages.CssClass = "nav-tab-btn active";
                    pnlTabMissingPages.Visible = true;
                    LoadMissingPagesReport();
                    break;

                case "UserControls":
                    btnTabUserControls.CssClass = "nav-tab-btn active";
                    pnlTabUserControls.Visible = true;
                    LoadUserControlsReport();
                    break;

                case "Subsites":
                    btnTabSubsites.CssClass = "nav-tab-btn active";
                    pnlTabSubsites.Visible = true;
                    LoadSubsitesScorecard();
                    break;

                case "LiveScan":
                    btnTabLiveScan.CssClass = "nav-tab-btn active";
                    pnlTabLiveScan.Visible = true;
                    break;

                default:
                    btnTabOverview.CssClass = "nav-tab-btn active";
                    pnlTabOverview.Visible = true;
                    LoadDashboardOverview();
                    break;
            }
        }

        #endregion

        #region Data Loading & Binding

        protected void btnRefreshAll_Click(object sender, EventArgs e)
        {
            PopulateLayoutsDropdown();
            LoadDashboardOverview();
            LoadPagesByLayoutReport();
            LoadMissingPagesReport();
            LoadUserControlsReport();
            LoadSubsitesScorecard();

            ShowAlert("تم تحديث جميع بيانات التقارير ولوحة المؤشرات بنجاح.", "success");
        }

        private void LoadDashboardOverview()
        {
            try
            {
                var summary = WebsitePagesReportDal.GetDashboardSummary();

                litKpiTotalPages.Text = summary.TotalPages.ToString("N0");
                litKpiArPages.Text = summary.TotalArabicPages.ToString("N0");
                litKpiEnPages.Text = summary.TotalEnglishPages.ToString("N0");
                litKpiEnExists.Text = summary.TotalEnExistsPages.ToString("N0");
                litKpiParityPct.Text = summary.BilingualParityPercentage.ToString("0.0") + "%";
                divParityProgress.Style["width"] = Math.Min(100.0, summary.BilingualParityPercentage) + "%";

                litKpiMissingTotal.Text = summary.TotalMissingPagesTotal > 0
                    ? summary.TotalMissingPagesTotal.ToString("N0")
                    : summary.TotalMissingEnPages.ToString("N0");
                litKpiMissingEn.Text = summary.TotalMissingEnPages.ToString("N0");
                litKpiResolved.Text = summary.TotalResolvedPages.ToString("N0");

                litKpiLayoutsCount.Text = summary.TotalPageLayouts.ToString("N0");
                litKpiDgaCount.Text = summary.DgaCompliantPages.ToString("N0");
                litKpiDgaPct.Text = summary.DgaCompliancePercentage.ToString("0.0") + "%";
                divDgaProgress.Style["width"] = Math.Min(100.0, summary.DgaCompliancePercentage) + "%";

                litKpiUcCount.Text = summary.TotalUserControlsCount.ToString("N0");
                litKpiSubsites.Text = summary.TotalSubsitesCount.ToString("N0");

                litLastScanDate.Text = summary.LastScanDate != DateTime.MinValue
                    ? summary.LastScanDate.ToString("yyyy-MM-dd HH:mm")
                    : DateTime.Now.ToString("yyyy-MM-dd");

                // Top Layouts
                var layouts = WebsitePagesReportDal.GetPageLayoutsSummary();
                rptTopLayouts.DataSource = layouts;
                rptTopLayouts.DataBind();

                // Top Subsites
                var subsites = WebsitePagesReportDal.GetSubsitesSummary();
                rptTopSubsites.DataSource = subsites.Count > 6 ? subsites.GetRange(0, 6) : subsites;
                rptTopSubsites.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء تحميل لوحة المؤشرات: " + ex.Message, "danger");
            }
        }

        private void PopulateLayoutsDropdown()
        {
            try
            {
                ddlLayoutFilter.Items.Clear();
                ddlLayoutFilter.Items.Add(new ListItem("جميع التخطيطات (All Layouts)", "All"));

                var layouts = WebsitePagesReportDal.GetPageLayoutsSummary();
                foreach (var l in layouts)
                {
                    string text = string.Format("{0} ({1} صفحة - {2}%)", l.LayoutFileName, l.PageCount, l.Percentage);
                    ddlLayoutFilter.Items.Add(new ListItem(text, l.PageLayout));
                }
            }
            catch { }
        }

        private void LoadPagesByLayoutReport()
        {
            try
            {
                // Summary table
                var layouts = WebsitePagesReportDal.GetPageLayoutsSummary();
                gvLayoutsSummary.DataSource = layouts;
                gvLayoutsSummary.DataBind();

                // Detailed pages list
                string layout = ddlLayoutFilter.SelectedValue;
                string subsite = txtPagesSubsiteFilter.Text.Trim();
                string kw = txtPagesKeyword.Text.Trim();
                string enStatus = ddlPagesEnStatus.SelectedValue;

                var pages = WebsitePagesReportDal.GetPages(layout, subsite, kw, enStatus, 1000);
                litPagesResultsCount.Text = pages.Count.ToString("N0");

                gvPagesByLayout.DataSource = pages;
                gvPagesByLayout.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء تحميل تقرير الصفحات: " + ex.Message, "danger");
            }
        }

        protected void FilterPages_Changed(object sender, EventArgs e)
        {
            gvPagesByLayout.PageIndex = 0;
            LoadPagesByLayoutReport();
        }

        protected void btnFilterPages_Click(object sender, EventArgs e)
        {
            gvPagesByLayout.PageIndex = 0;
            LoadPagesByLayoutReport();
        }

        protected void btnResetPagesFilter_Click(object sender, EventArgs e)
        {
            ddlLayoutFilter.SelectedIndex = 0;
            txtPagesSubsiteFilter.Text = "";
            txtPagesKeyword.Text = "";
            ddlPagesEnStatus.SelectedIndex = 0;
            gvPagesByLayout.PageIndex = 0;
            LoadPagesByLayoutReport();
        }

        protected void gvPagesByLayout_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPagesByLayout.PageIndex = e.NewPageIndex;
            LoadPagesByLayoutReport();
        }

        private void LoadMissingPagesReport()
        {
            try
            {
                string status = ddlMissingStatusFilter.SelectedValue;
                string direction = ddlMissingDirectionFilter.SelectedValue;
                string subsite = txtMissingSubsiteFilter.Text.Trim();
                string kw = txtMissingKeyword.Text.Trim();

                var missing = WebsitePagesReportDal.GetMissingPagesReport(status, direction, subsite, kw, 1000);
                litMissingCount.Text = missing.Count.ToString("N0");

                gvMissingPages.DataSource = missing;
                gvMissingPages.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء تحميل تقرير الصفحات المفقودة: " + ex.Message, "danger");
            }
        }

        protected void btnFilterMissing_Click(object sender, EventArgs e)
        {
            gvMissingPages.PageIndex = 0;
            LoadMissingPagesReport();
        }

        protected void btnResetMissingFilter_Click(object sender, EventArgs e)
        {
            ddlMissingStatusFilter.SelectedValue = "Pending";
            ddlMissingDirectionFilter.SelectedValue = "All";
            txtMissingSubsiteFilter.Text = "";
            txtMissingKeyword.Text = "";
            gvMissingPages.PageIndex = 0;
            LoadMissingPagesReport();
        }

        protected void gvMissingPages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMissingPages.PageIndex = e.NewPageIndex;
            LoadMissingPagesReport();
        }

        private void LoadUserControlsReport()
        {
            try
            {
                var ucList = WebsitePagesReportDal.GetUserControlsSummary();
                gvUserControls.DataSource = ucList;
                gvUserControls.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء تحميل تقرير عناصر التحكم: " + ex.Message, "danger");
            }
        }

        private void LoadSubsitesScorecard()
        {
            try
            {
                var subsites = WebsitePagesReportDal.GetSubsitesSummary();
                gvSubsitesHealth.DataSource = subsites;
                gvSubsitesHealth.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء تحميل تقرير المواقع الفرعية: " + ex.Message, "danger");
            }
        }

        #endregion

        #region Feature: Change Page Layout (Single & Bulk)

        protected void btnBulkChangeLayout_Click(object sender, EventArgs e)
        {
            string targetLayoutUrl = ddlTargetLayout.SelectedValue;
            bool majorCheckIn = chkLayoutMajorCheckIn.Checked;
            bool autoPublish = chkLayoutAutoPublish.Checked;

            int successCount = 0;
            int failCount = 0;
            var errorDetails = new StringBuilder();

            foreach (GridViewRow row in gvPagesByLayout.Rows)
            {
                var chk = row.FindControl("chkSelectPage") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    var hfPageUrl = row.FindControl("hfPageUrl") as HiddenField;
                    string pageUrl = hfPageUrl != null ? hfPageUrl.Value : "";

                    if (!string.IsNullOrEmpty(pageUrl))
                    {
                        string err;
                        bool ok = ExecutePageLayoutChange(pageUrl, targetLayoutUrl, majorCheckIn, autoPublish, out err);
                        if (ok)
                        {
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                            errorDetails.AppendLine(string.Format("• {0}: {1}", pageUrl, err));
                        }
                    }
                }
            }

            if (successCount == 0 && failCount == 0)
            {
                ShowAlert("يرجى تحديد صفحة واحدة على الأقل من الجدول لتغيير تخطيطها.", "warning");
                return;
            }

            // Refresh data
            LoadPagesByLayoutReport();
            PopulateLayoutsDropdown();
            LoadDashboardOverview();

            string msg = string.Format("اكتملت عملية تغيير التخطيط! تم تحديث {0} صفحة بنجاح.", successCount);
            if (failCount > 0)
            {
                msg += string.Format(" وفشل {0} صفحة.<br/>تفاصيل الأخطاء:<br/>{1}", failCount, errorDetails.ToString().Replace("\n", "<br/>"));
                ShowAlert(msg, "warning");
            }
            else
            {
                ShowAlert(msg, "success");
            }
        }

        protected void gvPagesByLayout_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ChangeSingleLayout")
            {
                string pageUrl = Convert.ToString(e.CommandArgument);
                string targetLayoutUrl = ddlTargetLayout.SelectedValue;
                bool majorCheckIn = chkLayoutMajorCheckIn.Checked;
                bool autoPublish = chkLayoutAutoPublish.Checked;

                string err;
                bool ok = ExecutePageLayoutChange(pageUrl, targetLayoutUrl, majorCheckIn, autoPublish, out err);
                if (ok)
                {
                    ShowAlert(string.Format("تم تغيير تخطيط الصفحة ({0}) بنجاح إلى: {1}", pageUrl, targetLayoutUrl), "success");
                    LoadPagesByLayoutReport();
                    PopulateLayoutsDropdown();
                    LoadDashboardOverview();
                }
                else
                {
                    ShowAlert(string.Format("فشل تغيير تخطيط الصفحة ({0}): {1}", pageUrl, err), "danger");
                }
            }
        }

        private bool ExecutePageLayoutChange(string pageUrl, string targetLayoutUrl, bool majorCheckIn, bool autoPublish, out string errorMessage)
        {
            errorMessage = null;
            bool ok = false;
            string err = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string rootSiteUrl = SPContext.Current != null && SPContext.Current.Site != null
                        ? SPContext.Current.Site.Url
                        : "https://pnu.edu.sa";

                    using (SPSite site = new SPSite(rootSiteUrl))
                    {
                        using (SPWeb web = OpenTargetWeb(site, pageUrl))
                        {
                            if (web == null || !web.Exists)
                            {
                                err = "تعذر العثور على موقع شيربوينت الحاضن للصفحة.";
                                return;
                            }

                            string serverRelPage = pageUrl;
                            if (serverRelPage.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || serverRelPage.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                                serverRelPage = new Uri(serverRelPage).AbsolutePath;

                            SPFile file = web.GetFile(serverRelPage);
                            if (file == null || !file.Exists)
                            {
                                err = "ملف الصفحة غير موجود في شيربوينت: " + serverRelPage;
                                return;
                            }

                            if (!PublishingWeb.IsPublishingWeb(web))
                            {
                                err = "الموقع ليس Publishing Web.";
                                return;
                            }

                            PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                            PageLayout targetLayout = GetPageLayout(pubWeb, targetLayoutUrl);

                            if (targetLayout == null)
                            {
                                err = "تعذر العثور على PageLayout المستهدف: " + targetLayoutUrl;
                                return;
                            }

                            web.AllowUnsafeUpdates = true;

                            // Check out if needed
                            if (file.CheckOutType == SPFile.SPCheckOutType.None)
                            {
                                file.CheckOut();
                            }

                            PublishingPage pubPage = PublishingPage.GetPublishingPage(file.Item);
                            if (pubPage == null)
                            {
                                string fileName = file.Name;
                                try { pubPage = pubWeb.GetPublishingPages()[fileName]; } catch { }
                            }

                            if (pubPage != null)
                            {
                                pubPage.Layout = targetLayout;
                                pubPage.Update();
                            }
                            else
                            {
                                file.Item["PublishingPageLayout"] = string.Format("{0}, {1}", targetLayout.ServerRelativeUrl, targetLayout.Title);
                                file.Item.Update();
                            }

                            // Check In
                            if (file.CheckOutType != SPFile.SPCheckOutType.None)
                            {
                                file.CheckIn("تم تغيير تخطيط الصفحة آلياً", majorCheckIn ? SPCheckinType.MajorCheckIn : SPCheckinType.MinorCheckIn);
                            }

                            // Publish & Approve
                            if (autoPublish && file.Item.ParentList.EnableMinorVersions)
                            {
                                file.Publish("تم النشر التلقائي بعد تغيير التخطيط");
                                if (file.Item.ParentList.EnableModeration)
                                {
                                    file.Approve("تم الاعتماد التلقائي");
                                }
                            }

                            // Update Database Record
                            string layoutRel = targetLayout.ServerRelativeUrl;
                            string pageAbsoluteUrl = new Uri(new Uri(site.Url), serverRelPage).AbsoluteUri;
                            WebsitePagesReportDal.UpdatePageLayout(pageAbsoluteUrl, layoutRel);

                            ok = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            });

            errorMessage = err;
            return ok;
        }

        #endregion

        #region Feature: Create Missing Pages (Single & Bulk)

        protected void btnBulkCreateMissing_Click(object sender, EventArgs e)
        {
            var selectedIds = new List<long>();
            foreach (GridViewRow row in gvMissingPages.Rows)
            {
                var chk = row.FindControl("chkSelectMissing") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    var hfId = row.FindControl("hfMissingId") as HiddenField;
                    long id = 0;
                    if (hfId != null && long.TryParse(hfId.Value, out id))
                    {
                        selectedIds.Add(id);
                    }
                }
            }

            if (selectedIds.Count == 0)
            {
                ShowAlert("يرجى تحديد صفحة مفقودة واحدة على الأقل من الجدول لإنشائها.", "warning");
                return;
            }

            ProcessCreateMissingPages(selectedIds);
        }

        protected void btnCreateAllFilteredMissing_Click(object sender, EventArgs e)
        {
            string status = ddlMissingStatusFilter.SelectedValue;
            string direction = ddlMissingDirectionFilter.SelectedValue;
            string subsite = txtMissingSubsiteFilter.Text.Trim();
            string kw = txtMissingKeyword.Text.Trim();

            var missingList = WebsitePagesReportDal.GetMissingPagesReport(status, direction, subsite, kw, 1000);
            var pendingIds = new List<long>();
            foreach (var m in missingList)
            {
                if (!m.IsCreated) pendingIds.Add(m.Id);
            }

            if (pendingIds.Count == 0)
            {
                ShowAlert("لا توجد صفحات مفقودة معلقة في النتائج الحالية لإنشائها.", "info");
                return;
            }

            ProcessCreateMissingPages(pendingIds);
        }

        protected void gvMissingPages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CreateSingleMissing")
            {
                long id = 0;
                long.TryParse(Convert.ToString(e.CommandArgument), out id);
                if (id > 0)
                {
                    ProcessCreateMissingPages(new List<long> { id });
                }
            }
            else if (e.CommandName == "ResolvePage")
            {
                string arg = Convert.ToString(e.CommandArgument);
                if (!string.IsNullOrEmpty(arg))
                {
                    string[] parts = arg.Split('|');
                    long id = 0;
                    long.TryParse(parts[0], out id);
                    string targetUrl = parts.Length > 1 ? parts[1] : "";

                    bool ok = WebsitePagesReportDal.MarkMissingPageResolved(id, targetUrl);
                    if (ok)
                    {
                        ShowAlert("تم تحديث حالة الصفحة المفقودة كـ (تم إنشاؤها / محلولة) بنجاح.", "success");
                        LoadMissingPagesReport();
                        LoadDashboardOverview();
                    }
                    else
                    {
                        ShowAlert("تعذر تحديث حالة الصفحة المفقودة.", "danger");
                    }
                }
            }
        }

        private void ProcessCreateMissingPages(List<long> missingIds)
        {
            string targetLayoutOption = ddlMissingTargetLayout.SelectedValue;
            bool copyUserControls = chkCopyUserControls.Checked;
            bool copyContentHtml = chkCopyContentHtml.Checked;
            bool autoPublish = chkAutoPublishCreated.Checked;

            int successCount = 0;
            int failCount = 0;
            var errorDetails = new StringBuilder();

            var allMissing = WebsitePagesReportDal.GetMissingPagesReport("All", "All", null, null, 2000);
            var map = new Dictionary<long, MissingPageReportDto>();
            foreach (var m in allMissing) map[m.Id] = m;

            foreach (long id in missingIds)
            {
                if (!map.ContainsKey(id)) continue;
                var item = map[id];

                string err;
                bool ok = ExecutePageCreation(item, targetLayoutOption, copyUserControls, copyContentHtml, autoPublish, out err);
                if (ok)
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                    errorDetails.AppendLine(string.Format("• {0}: {1}", item.TargetPageUrl, err));
                }
            }

            // Refresh data
            LoadMissingPagesReport();
            LoadDashboardOverview();
            LoadPagesByLayoutReport();

            string msg = string.Format("اكتملت عملية إنشاء الصفحات المفقودة! تم إنشاء {0} صفحة بنجاح في شيربوينت.", successCount);
            if (failCount > 0)
            {
                msg += string.Format(" وفشل {0} صفحة.<br/>تفاصيل الأخطاء:<br/>{1}", failCount, errorDetails.ToString().Replace("\n", "<br/>"));
                ShowAlert(msg, "warning");
            }
            else
            {
                ShowAlert(msg, "success");
            }
        }

        private bool ExecutePageCreation(MissingPageReportDto item, string targetLayoutOption, bool copyUserControls, bool copyContentHtml, bool autoPublish, out string errorMessage)
        {
            errorMessage = null;
            if (item == null || string.IsNullOrEmpty(item.TargetPageUrl))
            {
                errorMessage = "بيانات الصفحة المفقودة غير صحيحة.";
                return false;
            }

            bool ok = false;
            string err = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string rootSiteUrl = SPContext.Current != null && SPContext.Current.Site != null
                        ? SPContext.Current.Site.Url
                        : "https://pnu.edu.sa";

                    using (SPSite site = new SPSite(rootSiteUrl))
                    {
                        string targetWebUrl = item.TargetWebUrl;
                        if (string.IsNullOrEmpty(targetWebUrl))
                        {
                            targetWebUrl = item.TargetPageUrl;
                            int lastSlash = targetWebUrl.LastIndexOf('/');
                            if (lastSlash > 0) targetWebUrl = targetWebUrl.Substring(0, lastSlash);
                            if (targetWebUrl.EndsWith("/Pages", StringComparison.OrdinalIgnoreCase))
                                targetWebUrl = targetWebUrl.Substring(0, targetWebUrl.Length - 6);
                        }

                        using (SPWeb tgtWeb = OpenTargetWeb(site, targetWebUrl))
                        {
                            if (tgtWeb == null || !tgtWeb.Exists)
                            {
                                err = "الموقع المستهدف غير موجود في شيربوينت: " + targetWebUrl;
                                return;
                            }

                            if (!PublishingWeb.IsPublishingWeb(tgtWeb))
                            {
                                err = "الموقع المستهدف ليس Publishing Web.";
                                return;
                            }

                            PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(tgtWeb);

                            // Determine Layout
                            string chosenLayoutPath = targetLayoutOption == "SourceLayout" || string.IsNullOrEmpty(targetLayoutOption)
                                ? item.PageLayout
                                : targetLayoutOption;

                            if (string.IsNullOrEmpty(chosenLayoutPath))
                                chosenLayoutPath = "/_catalogs/masterpage/DGANewBlankWebPartPage.aspx";

                            PageLayout layout = GetPageLayout(pubWeb, chosenLayoutPath);
                            if (layout == null)
                            {
                                layout = GetPageLayout(pubWeb, "/_catalogs/masterpage/DGANewBlankWebPartPage.aspx")
                                      ?? GetPageLayout(pubWeb, "BlankWebPartPage.aspx");
                            }

                            if (layout == null)
                            {
                                err = "تعذر العثور على PageLayout صالح لإنشاء الصفحة: " + chosenLayoutPath;
                                return;
                            }

                            tgtWeb.AllowUnsafeUpdates = true;

                            // Read source page content if needed
                            string sourceContentHtml = "";
                            if (copyContentHtml && !string.IsNullOrEmpty(item.SourcePageUrl))
                            {
                                try
                                {
                                    using (SPWeb srcWeb = OpenTargetWeb(site, item.SourcePageUrl))
                                    {
                                        if (srcWeb != null && srcWeb.Exists)
                                        {
                                            SPFile srcFile = srcWeb.GetFile(item.SourcePageUrl);
                                            if (srcFile != null && srcFile.Exists && srcFile.Item != null)
                                            {
                                                PublishingPage srcPubPage = PublishingPage.GetPublishingPage(srcFile.Item);
                                                if (srcPubPage != null)
                                                    sourceContentHtml = GetPageContentHtml(srcPubPage);
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }

                            // Check if page already exists
                            string pageFileName = item.PageFileName;
                            if (string.IsNullOrEmpty(pageFileName))
                            {
                                int sl = item.TargetPageUrl.LastIndexOf('/');
                                pageFileName = sl >= 0 ? item.TargetPageUrl.Substring(sl + 1) : "page.aspx";
                            }

                            SPFile existingFile = tgtWeb.GetFile(tgtWeb.ServerRelativeUrl.TrimEnd('/') + "/Pages/" + pageFileName);
                            PublishingPage newPage = null;

                            if (existingFile != null && existingFile.Exists)
                            {
                                newPage = PublishingPage.GetPublishingPage(existingFile.Item);
                                if (existingFile.CheckOutType == SPFile.SPCheckOutType.None)
                                    existingFile.CheckOut();
                            }
                            else
                            {
                                newPage = pubWeb.GetPublishingPages().Add(pageFileName, layout);
                            }

                            if (newPage != null)
                            {
                                string pageTitle = !string.IsNullOrEmpty(item.PageTitleEn)
                                    ? item.PageTitleEn
                                    : (!string.IsNullOrEmpty(item.PageTitleAr) ? item.PageTitleAr : pageFileName);

                                newPage.Title = pageTitle;

                                if (copyContentHtml && !string.IsNullOrEmpty(sourceContentHtml))
                                {
                                    SetPageContentHtml(newPage, sourceContentHtml);
                                }

                                newPage.Update();

                                // Add User Controls / Web Parts
                                if (copyUserControls && !string.IsNullOrEmpty(item.UserControlPath))
                                {
                                    try
                                    {
                                        SPFile newPageFile = newPage.ListItem.File;
                                        using (SPLimitedWebPartManager wpMgr = newPageFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                                        {
                                            var clWp = new global::PNU.Internet.WebParts.ControlLoaderWebPart.ControlLoaderWebPart
                                            {
                                                Title = "Control Loader WebPart",
                                                ChromeType = PartChromeType.None,
                                                UserControlPath = item.UserControlPath,
                                                UserControlProperties = item.UserControlProperties
                                            };
                                            wpMgr.AddWebPart(clWp, "Header", 0);
                                        }
                                    }
                                    catch { }
                                }

                                SPFile pageFile = newPage.ListItem.File;

                                // Check In
                                if (pageFile.CheckOutType != SPFile.SPCheckOutType.None)
                                {
                                    pageFile.CheckIn("تم إنشاء ومزامنة الصفحة آلياً", SPCheckinType.MajorCheckIn);
                                }

                                // Publish & Approve
                                if (autoPublish && pageFile.Item.ParentList.EnableMinorVersions)
                                {
                                    pageFile.Publish("تم النشر التلقائي بعد إنشاء الصفحة");
                                    if (pageFile.Item.ParentList.EnableModeration)
                                    {
                                        pageFile.Approve("تم الاعتماد التلقائي");
                                    }
                                }

                                // Mark as created in SQL database
                                WebsitePagesReportDal.MarkMissingPageResolved(item.Id, item.TargetPageUrl);

                                // Upsert to dbo.WebsitePages
                                WebsitePagesReportDal.UpsertPageRecord(new WebsitePageItemDto
                                {
                                    PageTitle = pageTitle,
                                    PageURL = new Uri(new Uri(site.Url), item.TargetPageUrl).AbsoluteUri,
                                    PageLayout = layout.ServerRelativeUrl,
                                    UserControlPath = item.UserControlPath,
                                    UserControlProperties = item.UserControlProperties,
                                    WebUrl = tgtWeb.Url,
                                    EnExists = true
                                });

                                ok = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            });

            errorMessage = err;
            return ok;
        }

        #endregion

        #region Live Subsite Scanner & Sync

        protected void btnStartLiveScan_Click(object sender, EventArgs e)
        {
            var log = new StringBuilder();
            log.AppendLine(string.Format("[{0:yyyy-MM-dd HH:mm:ss}] بدء المسح المباشر وتحديث الفهرس...", DateTime.Now));

            int scannedWebs = 0;
            int scannedPages = 0;
            int missingFound = 0;

            string targetUrl = txtLiveScanUrl.Text.Trim();
            bool currentWebOnly = chkCurrentSubsiteOnly.Checked;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string rootSiteUrl = SPContext.Current != null && SPContext.Current.Site != null
                        ? SPContext.Current.Site.Url
                        : "https://pnu.edu.sa";

                    using (SPSite site = new SPSite(rootSiteUrl))
                    {
                        string webToOpen = string.IsNullOrEmpty(targetUrl) ? site.ServerRelativeUrl : targetUrl;
                        if (!webToOpen.StartsWith("/")) webToOpen = "/" + webToOpen;

                        using (SPWeb targetWeb = OpenTargetWeb(site, webToOpen))
                        {
                            if (targetWeb == null || !targetWeb.Exists)
                            {
                                log.AppendLine(string.Format("[-] تعذر العثور على موقع شيربوينت في المسار: {0}", webToOpen));
                                return;
                            }

                            ScanWebRecursive(site, targetWeb, currentWebOnly, log, ref scannedWebs, ref scannedPages, ref missingFound);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.AppendLine(string.Format("[!] خطأ أثناء تنفيذ الفحص: {0}", ex.ToString()));
                }
            });

            log.AppendLine(string.Format("[{0:yyyy-MM-dd HH:mm:ss}] اكتمل المسح بنجاح! المواقع المفحوصة: {1} | الصفحات المفحوصة: {2} | الصفحات المفقودة المسجلة: {3}",
                DateTime.Now, scannedWebs, scannedPages, missingFound));

            litLiveScanLog.Text = log.ToString();

            // Refresh dashboards
            LoadDashboardOverview();
            PopulateLayoutsDropdown();
            ShowAlert(string.Format("اكتملت عملية الفحص والتحديث المباشر بنجاح! تم فحص {0} صفحة عبر {1} موقع فرعي.", scannedPages, scannedWebs), "success");
        }

        private void ScanWebRecursive(SPSite site, SPWeb web, bool currentWebOnly, StringBuilder log, ref int scannedWebs, ref int scannedPages, ref int missingFound)
        {
            if (web == null) return;
            scannedWebs++;
            log.AppendLine(string.Format("[+] فحص الموقع: {0} ({1})", web.Title, web.ServerRelativeUrl));

            try
            {
                if (PublishingWeb.IsPublishingWeb(web))
                {
                    PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                    SPList pagesList = pubWeb.PagesList;
                    if (pagesList != null)
                    {
                        var query = new SPQuery { ViewAttributes = "Scope=\"Recursive\"" };
                        foreach (SPListItem item in pagesList.GetItems(query))
                        {
                            try
                            {
                                PublishingPage pubPage = PublishingPage.GetPublishingPage(item);
                                if (pubPage == null) continue;

                                scannedPages++;
                                string pageServerRel = item.File != null ? item.File.ServerRelativeUrl : "";
                                string absoluteUrl = !string.IsNullOrEmpty(pageServerRel)
                                    ? new Uri(new Uri(site.Url), pageServerRel).AbsoluteUri
                                    : pubPage.Url;

                                string layoutRel = pubPage.Layout != null ? pubPage.Layout.ServerRelativeUrl : "";
                                string layoutFileName = layoutRel;
                                if (!string.IsNullOrEmpty(layoutRel))
                                {
                                    int sl = layoutRel.LastIndexOf('/');
                                    if (sl >= 0) layoutFileName = layoutRel.Substring(sl + 1);
                                }

                                // Collect Web Parts / User Controls
                                string ucPath = "";
                                string ucProps = "";
                                try
                                {
                                    using (SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(
                                        !string.IsNullOrEmpty(pageServerRel) ? pageServerRel : pubPage.Url,
                                        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
                                    {
                                        var ucList = new List<string>();
                                        foreach (WebPart wp in mgr.WebParts)
                                        {
                                            string p = GetWebPartUserControlPath(wp);
                                            if (!string.IsNullOrEmpty(p) && !ucList.Contains(p))
                                                ucList.Add(p);
                                        }
                                        ucPath = string.Join(Environment.NewLine, ucList);
                                    }
                                }
                                catch { }

                                // Check English mirror
                                bool enExists = false;
                                string enServerRel = "";
                                string enAbsoluteUrl = "";
                                string pageTitleEn = null;
                                string layoutEn = null;

                                if (pageServerRel.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase))
                                {
                                    enServerRel = "/en/" + pageServerRel.Substring(4);
                                    enAbsoluteUrl = new Uri(new Uri(site.Url), enServerRel).AbsoluteUri;

                                    try
                                    {
                                        using (SPWeb enWeb = OpenTargetWeb(site, enServerRel))
                                        {
                                            if (enWeb != null && enWeb.Exists)
                                            {
                                                SPFile enFile = enWeb.GetFile(enServerRel);
                                                if (enFile != null && enFile.Exists)
                                                {
                                                    enExists = true;
                                                    if (enFile.Item != null)
                                                    {
                                                        pageTitleEn = Convert.ToString(enFile.Item["Title"] ?? enFile.Name);
                                                        try
                                                        {
                                                            PublishingPage enPubPage = PublishingPage.GetPublishingPage(enFile.Item);
                                                            if (enPubPage != null && enPubPage.Layout != null)
                                                                layoutEn = enPubPage.Layout.ServerRelativeUrl;
                                                        }
                                                        catch { }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch { }

                                    // If missing, record in dbo.MissingPagesOnEN
                                    if (!enExists)
                                    {
                                        missingFound++;
                                        WebsitePagesReportDal.UpsertMissingPageRecord(new MissingPageReportDto
                                        {
                                            PageFileName = item.File != null ? item.File.Name : "page.aspx",
                                            PageTitleAr = pubPage.Title,
                                            PageTitleEn = null,
                                            SourcePageUrl = pageServerRel,
                                            SourcePageFullUrl = absoluteUrl,
                                            TargetPageUrl = enServerRel,
                                            TargetPageFullUrl = enAbsoluteUrl,
                                            SourceWebUrl = web.ServerRelativeUrl,
                                            SourceWebFullUrl = web.Url,
                                            TargetWebUrl = web.ServerRelativeUrl.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase) ? "/en/" + web.ServerRelativeUrl.Substring(4) : web.ServerRelativeUrl,
                                            PageLayout = layoutRel,
                                            PageLayoutFileName = layoutFileName,
                                            HasControlLoader = !string.IsNullOrEmpty(ucPath),
                                            UserControlPath = ucPath,
                                            UserControlProperties = ucProps,
                                            Direction = "ArToEn",
                                            IsCreated = false,
                                            Status = "Missing"
                                        });
                                    }
                                }

                                // Upsert record in dbo.WebsitePages
                                WebsitePagesReportDal.UpsertPageRecord(new WebsitePageItemDto
                                {
                                    PageTitle = pubPage.Title,
                                    PageURL = absoluteUrl,
                                    PageLayout = layoutRel,
                                    UserControlPath = ucPath,
                                    UserControlProperties = ucProps,
                                    WebUrl = web.Url,
                                    PageTitleEn = pageTitleEn,
                                    PageURLEn = !string.IsNullOrEmpty(enAbsoluteUrl) ? enAbsoluteUrl : null,
                                    PageLayoutEn = layoutEn,
                                    WebUrlEn = !string.IsNullOrEmpty(enServerRel) ? web.Url.Replace("/ar/", "/en/") : null,
                                    EnExists = enExists
                                });
                            }
                            catch (Exception pex)
                            {
                                log.AppendLine(string.Format("    [!] فشل قراءة صفحة: {0}", pex.Message));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.AppendLine(string.Format("    [!] خطأ في فحص صفحات الموقع {0}: {1}", web.ServerRelativeUrl, ex.Message));
            }

            if (!currentWebOnly)
            {
                foreach (SPWeb childWeb in web.Webs)
                {
                    try { ScanWebRecursive(site, childWeb, false, log, ref scannedWebs, ref scannedPages, ref missingFound); }
                    finally { childWeb.Dispose(); }
                }
            }
        }

        private static SPWeb OpenTargetWeb(SPSite site, string serverRelativeUrl)
        {
            if (site == null || string.IsNullOrEmpty(serverRelativeUrl)) return null;
            try
            {
                string path = serverRelativeUrl;
                if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    path = new Uri(path).AbsolutePath;

                if (!path.StartsWith("/")) path = "/" + path;
                path = path.TrimEnd('/');
                if (path.Length == 0) path = "/";

                try
                {
                    SPWeb w = site.OpenWeb(path, true);
                    if (w != null && w.Exists) return w;
                    if (w != null) w.Dispose();
                }
                catch { }

                // Walk up
                int idx = path.LastIndexOf('/');
                while (idx > 0)
                {
                    path = path.Substring(0, idx);
                    try
                    {
                        SPWeb w = site.OpenWeb(path, true);
                        if (w != null && w.Exists) return w;
                        if (w != null) w.Dispose();
                    }
                    catch { }
                    idx = path.LastIndexOf('/');
                }

                return site.OpenWeb(site.ServerRelativeUrl);
            }
            catch { return null; }
        }

        private static PageLayout GetPageLayout(PublishingWeb pubWeb, string layoutUrlOrName)
        {
            if (pubWeb == null || string.IsNullOrEmpty(layoutUrlOrName)) return null;

            string layoutFileName = layoutUrlOrName;
            int idx = layoutFileName.LastIndexOf('/');
            if (idx >= 0) layoutFileName = layoutFileName.Substring(idx + 1);

            try
            {
                PageLayout[] availableLayouts = pubWeb.GetAvailablePageLayouts();
                foreach (PageLayout layout in availableLayouts)
                {
                    if (layout.Name.Equals(layoutFileName, StringComparison.OrdinalIgnoreCase) ||
                        layout.ServerRelativeUrl.EndsWith(layoutFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return layout;
                    }
                }
            }
            catch { }

            try
            {
                SPList masterCatalog = pubWeb.Web.Site.GetCatalog(SPListTemplateType.MasterPageCatalog);
                if (masterCatalog != null)
                {
                    foreach (SPListItem item in masterCatalog.Items)
                    {
                        if (item.File != null && item.File.Name.Equals(layoutFileName, StringComparison.OrdinalIgnoreCase))
                        {
                            return new PageLayout(item);
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        private static string GetPageContentHtml(PublishingPage pubPage)
        {
            if (pubPage == null || pubPage.ListItem == null) return "";
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
                return Convert.ToString(pubPage.ListItem["PublishingPageContent"]);
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
                return Convert.ToString(pubPage.ListItem["PublishingPageContentHtml"]);
            return "";
        }

        private static void SetPageContentHtml(PublishingPage pubPage, string content)
        {
            if (pubPage == null || pubPage.ListItem == null) return;
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
                pubPage.ListItem["PublishingPageContent"] = content;
            else if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
                pubPage.ListItem["PublishingPageContentHtml"] = content;
        }

        private static string GetWebPartUserControlPath(WebPart wp)
        {
            if (wp == null) return "";
            try
            {
                var type = wp.GetType();
                var pi = type.GetProperty("UserControlPath") ??
                         type.GetProperty("UserControlSrc") ??
                         type.GetProperty("Path") ??
                         type.GetProperty("ControlPath") ??
                         type.GetProperty("Src");

                if (pi != null && pi.CanRead)
                {
                    object val = pi.GetValue(wp, null);
                    if (val != null && !string.IsNullOrEmpty(val.ToString()))
                        return val.ToString();
                }
            }
            catch { }
            return wp.GetType().FullName;
        }

        #endregion

        #region Export to Excel

        protected void btnExportCurrentTab_Click(object sender, EventArgs e)
        {
            string fileName = string.Format("PNU_Pages_Report_{0}_{1:yyyyMMdd_HHmm}.xls", CurrentTab, DateTime.Now);
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\" dir=\"rtl\">");
            sb.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Tahoma, Arial, sans-serif; }");
            sb.AppendLine("th { background-color: #0d5c46; color: #ffffff; font-weight: bold; border: 1px solid #083e2f; padding: 8px; text-align: center; }");
            sb.AppendLine("td { border: 1px solid #cbd5e1; padding: 6px 10px; font-size: 12px; }");
            sb.AppendLine(".dga { background-color: #dcfce7; color: #166534; font-weight: bold; }");
            sb.AppendLine(".missing { background-color: #fee2e2; color: #991b1b; font-weight: bold; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head><body>");

            if (CurrentTab == "MissingPages")
            {
                var list = WebsitePagesReportDal.GetMissingPagesReport(ddlMissingStatusFilter.SelectedValue, ddlMissingDirectionFilter.SelectedValue, txtMissingSubsiteFilter.Text.Trim(), txtMissingKeyword.Text.Trim(), 5000);
                sb.AppendLine("<h2>تقرير الصفحات المفقودة ثنائياً - جامعة الأميرة نورة بنت عبدالرحمن</h2>");
                sb.AppendLine("<table border=\"1\">");
                sb.AppendLine("<tr><th>#</th><th>اسم الملف</th><th>العنوان العربي</th><th>الرابط المستهدف المفقود</th><th>الرابط المصدر</th><th>الموقع الفرعي</th><th>تخطيط الصفحة</th><th>عنصر التحكم</th><th>الحالة</th><th>تاريخ الاكتشاف</th></tr>");
                int index = 1;
                foreach (var item in list)
                {
                    sb.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td class=\"missing\">{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9:yyyy-MM-dd}</td></tr>",
                        index++,
                        HttpUtility.HtmlEncode(item.PageFileName),
                        HttpUtility.HtmlEncode(item.PageTitleAr),
                        HttpUtility.HtmlEncode(item.TargetPageUrl),
                        HttpUtility.HtmlEncode(item.SourcePageUrl),
                        HttpUtility.HtmlEncode(item.TargetWebUrl),
                        HttpUtility.HtmlEncode(item.PageLayoutFileName),
                        HttpUtility.HtmlEncode(item.UserControlPath),
                        item.IsCreated ? "تم الإنشاء" : "مفقودة",
                        item.LastDiscoveredDate));
                }
                sb.AppendLine("</table>");
            }
            else if (CurrentTab == "UserControls")
            {
                var list = WebsitePagesReportDal.GetUserControlsSummary();
                sb.AppendLine("<h2>تقرير عناصر التحكم وقطع الويب - جامعة الأميرة نورة بنت عبدالرحمن</h2>");
                sb.AppendLine("<table border=\"1\">");
                sb.AppendLine("<tr><th>#</th><th>اسم الملف</th><th>مسار عنصر التحكم</th><th>إجمالي الصفحات</th><th>الصفحات العربية</th><th>الصفحات الإنجليزية</th><th>نموذج صفحة</th></tr>");
                int index = 1;
                foreach (var item in list)
                {
                    sb.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                        index++,
                        HttpUtility.HtmlEncode(item.ControlFileName),
                        HttpUtility.HtmlEncode(item.UserControlPath),
                        item.PageCount,
                        item.ArPagesCount,
                        item.EnPagesCount,
                        HttpUtility.HtmlEncode(item.SamplePageUrl)));
                }
                sb.AppendLine("</table>");
            }
            else if (CurrentTab == "Subsites")
            {
                var list = WebsitePagesReportDal.GetSubsitesSummary();
                sb.AppendLine("<h2>تقرير مؤشرات صحة المواقع الفرعية - جامعة الأميرة نورة بنت عبدالرحمن</h2>");
                sb.AppendLine("<table border=\"1\">");
                sb.AppendLine("<tr><th>#</th><th>رابط الموقع الفرعي</th><th>إجمالي الصفحات</th><th>متوفرة بالإنجليزية</th><th>مفقودة بالإنجليزية</th><th>نسبة التوافق %</th><th>نسبة DGA %</th></tr>");
                int index = 1;
                foreach (var item in list)
                {
                    sb.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}%</td><td>{6}%</td></tr>",
                        index++,
                        HttpUtility.HtmlEncode(item.WebUrl),
                        item.TotalPages,
                        item.HasEnglishCount,
                        item.MissingEnglishCount,
                        item.ParityPercentage,
                        item.DgaPercentage));
                }
                sb.AppendLine("</table>");
            }
            else
            {
                // Default: Pages by Layout
                var list = WebsitePagesReportDal.GetPages(ddlLayoutFilter.SelectedValue, txtPagesSubsiteFilter.Text.Trim(), txtPagesKeyword.Text.Trim(), ddlPagesEnStatus.SelectedValue, 5000);
                sb.AppendLine("<h2>تقرير صفحات الموقع وتخطيطاتها - جامعة الأميرة نورة بنت عبدالرحمن</h2>");
                sb.AppendLine("<table border=\"1\">");
                sb.AppendLine("<tr><th>#</th><th>عنوان الصفحة (AR)</th><th>الرابط (AR)</th><th>تخطيط الصفحة</th><th>معايير DGA</th><th>الرابط الإنجليزي</th><th>متوفرة بالإنجليزية؟</th><th>عنصر التحكم</th><th>الموقع الفرعي</th><th>تاريخ الفهرسة</th></tr>");
                int index = 1;
                foreach (var item in list)
                {
                    sb.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td class=\"{4}\">{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10:yyyy-MM-dd HH:mm}</td></tr>",
                        index++,
                        HttpUtility.HtmlEncode(item.PageTitle),
                        HttpUtility.HtmlEncode(item.PageURL),
                        HttpUtility.HtmlEncode(item.PageLayoutFileName),
                        item.IsDgaCompliant ? "dga" : "",
                        item.IsDgaCompliant ? "نعم (DGA)" : "لا (Legacy)",
                        HttpUtility.HtmlEncode(item.PageURLEn),
                        item.EnExists ? "نعم" : "لا",
                        HttpUtility.HtmlEncode(item.UserControlPath),
                        HttpUtility.HtmlEncode(item.WebUrl),
                        item.LastIndexed));
                }
                sb.AppendLine("</table>");
            }

            sb.AppendLine("</body></html>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "utf-8";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        #endregion

        #region Helpers

        private void ShowAlert(string message, string type = "info")
        {
            pnlAlert.Visible = true;
            string color = "#0d5c46";
            if (type == "danger") color = "#dc2626";
            else if (type == "warning") color = "#d97706";
            else if (type == "success") color = "#16a34a";

            pnlAlert.Style["border-right"] = "5px solid " + color;
            litAlertMessage.Text = string.Format("<div style='font-weight:600; color:{0}; font-size:14px;'>{1}</div>", color, message);
        }

        #endregion
    }
}
