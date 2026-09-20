using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SiteAudit
{
    public partial class ucExcelAuditVerifier : UserControl
    {
        public string ActiveSource
        {
            get { return (ViewState["ActiveSource"] as string) ?? "Excel74"; }
            set { ViewState["ActiveSource"] = value; }
        }

        public int CurrentPageIndex
        {
            get { return (int)(ViewState["CurrentPageIndex"] ?? 1); }
            set { ViewState["CurrentPageIndex"] = value; }
        }

        public int PageSize
        {
            get { return 25; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentPageIndex = 1;
                ActiveSource = "Excel74";
                UpdateTabUi();
                BindIssueGroups();
                LoadKpisAndIssues();
            }
        }

        protected void btnTab74_Click(object sender, EventArgs e)
        {
            ActiveSource = "Excel74";
            CurrentPageIndex = 1;
            pnlAlert.Visible = false;
            UpdateTabUi();
            BindIssueGroups();
            LoadKpisAndIssues();
        }

        protected void btnTabSemrush_Click(object sender, EventArgs e)
        {
            ActiveSource = "Semrush_20260919";
            CurrentPageIndex = 1;
            pnlAlert.Visible = false;
            UpdateTabUi();
            BindIssueGroups();
            LoadKpisAndIssues();
        }

        private void UpdateTabUi()
        {
            if (ActiveSource == "Semrush_20260919")
            {
                btnTabSemrush.CssClass = "tab-btn active";
                btnTab74.CssClass = "tab-btn";
            }
            else
            {
                btnTab74.CssClass = "tab-btn active";
                btnTabSemrush.CssClass = "tab-btn";
            }
        }

        private void BindIssueGroups()
        {
            try
            {
                var groups = ExcelAuditDal.GetGroupsSummary(ActiveSource);
                ddlIssueGroups.Items.Clear();

                if (groups != null && groups.Count > 0)
                {
                    foreach (var g in groups)
                    {
                        string text = string.Format("#{0:D2} - {1} ({2} رابط)", g.IssueCode, g.IssueName, g.TotalUrls);
                        ddlIssueGroups.Items.Add(new ListItem(text, g.IssueCode.ToString()));
                    }
                }
                else
                {
                    if (ActiveSource == "Semrush_20260919")
                        ddlIssueGroups.Items.Add(new ListItem("لا توجد بيانات مستوردة لتقرير سيمرش بعد (قم بتشغيل سكريبت الاستيراد Import-SemrushAuditIssues.ps1)", "0"));
                    else
                        ddlIssueGroups.Items.Add(new ListItem("لا توجد بيانات مستوردة بعد (قم باستيراد ملف الإكسل أولاً)", "0"));
                }
            }
            catch (Exception ex)
            {
                ShowAlert("تعذر جلب معايير الفحص: " + ex.Message, false);
            }
        }

        private void LoadKpisAndIssues()
        {
            try
            {
                int issueCode = 0;
                int.TryParse(ddlIssueGroups.SelectedValue, out issueCode);

                if (issueCode <= 0)
                {
                    litKpiTotal.Text = "0";
                    litKpiResolved.Text = "0";
                    litKpiUnresolved.Text = "0";
                    litKpiPending.Text = "0";
                    rptIssues.DataSource = new List<ExcelAuditIssueItemDto>();
                    rptIssues.DataBind();
                    litPaginationInfo.Text = "لا توجد بيانات.";
                    btnPrevPage.Enabled = false;
                    btnNextPage.Enabled = false;
                    return;
                }

                // Update KPIs for the selected issue
                var groups = ExcelAuditDal.GetGroupsSummary(ActiveSource);
                var currentGroup = groups.Find(g => g.IssueCode == issueCode);
                if (currentGroup != null)
                {
                    litKpiTotal.Text = currentGroup.TotalUrls.ToString();
                    litKpiResolved.Text = currentGroup.ResolvedCount.ToString();
                    litKpiUnresolved.Text = currentGroup.UnresolvedCount.ToString();
                    litKpiPending.Text = currentGroup.PendingCount.ToString();
                }

                // Load paged issues
                string status = ddlStatusFilter.SelectedValue;
                string search = txtSearch.Text.Trim();
                int totalCount = 0;

                var items = ExcelAuditDal.GetIssues(issueCode, status, search, CurrentPageIndex, PageSize, out totalCount, ActiveSource);

                rptIssues.DataSource = items;
                rptIssues.DataBind();

                // Pagination
                int maxPages = (int)Math.Ceiling((double)totalCount / PageSize);
                if (maxPages == 0) maxPages = 1;

                btnPrevPage.Enabled = (CurrentPageIndex > 1);
                btnNextPage.Enabled = (CurrentPageIndex < maxPages);

                int startRow = ((CurrentPageIndex - 1) * PageSize) + (items.Count > 0 ? 1 : 0);
                int endRow = ((CurrentPageIndex - 1) * PageSize) + items.Count;
                litPaginationInfo.Text = string.Format("عرض {0} إلى {1} من إجمالي {2} رابط (صفحة {3} من {4})", startRow, endRow, totalCount, CurrentPageIndex, maxPages);
            }
            catch (Exception ex)
            {
                ShowAlert("خطأ أثناء استرجاع الروابط: " + ex.Message, false);
            }
        }

        protected void ddlIssueGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            pnlAlert.Visible = false;
            LoadKpisAndIssues();
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            LoadKpisAndIssues();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            LoadKpisAndIssues();
        }

        protected void btnRefreshData_Click(object sender, EventArgs e)
        {
            BindIssueGroups();
            LoadKpisAndIssues();
            ShowAlert("تم تحديث البيانات من قاعدة البيانات بنجاح.", true);
        }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                LoadKpisAndIssues();
            }
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            CurrentPageIndex++;
            LoadKpisAndIssues();
        }

        protected void btnVerifySelected_Click(object sender, EventArgs e)
        {
            int checkedCount = 0;
            int resolvedCount = 0;
            int unresolvedCount = 0;

            try
            {
                foreach (RepeaterItem item in rptIssues.Items)
                {
                    CheckBox chk = item.FindControl("chkRowSelect") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        HiddenField hfId = item.FindControl("hfRowId") as HiddenField;
                        HiddenField hfUrl = item.FindControl("hfRowUrl") as HiddenField;
                        HiddenField hfCode = item.FindControl("hfRowIssueCode") as HiddenField;

                        if (hfId != null && hfUrl != null && hfCode != null)
                        {
                            long id = Convert.ToInt64(hfId.Value);
                            int issueCode = Convert.ToInt32(hfCode.Value);
                            string url = hfUrl.Value;

                            // Run Verification Rule
                            var checkResult = SiteAuditRuleVerifier.VerifyUrlIssue(issueCode, url);

                            // Update Database
                            ExcelAuditDal.UpdateVerificationStatus(id, checkResult.Status, checkResult.ResponseCode, checkResult.Details);

                            checkedCount++;
                            if (checkResult.IsResolved) resolvedCount++;
                            else unresolvedCount++;
                        }
                    }
                }

                if (checkedCount > 0)
                {
                    LoadKpisAndIssues();
                    ShowAlert(string.Format("تم الانتهاء من فحص {0} روابط مختارة: ({1} تم معالجتها بنجاح ✅، {2} ما زالت بحاجة لمعالجة ❌).", checkedCount, resolvedCount, unresolvedCount), true);
                }
                else
                {
                    ShowAlert("لم يتم تحديد أي رابط. يرجى تحديد الروابط المطلوب فحصها عبر مربعات الاختيار.", false);
                }
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء فحص الروابط المحددة: " + ex.Message, false);
            }
        }

        protected void btnVerifyPendingOnly_Click(object sender, EventArgs e)
        {
            try
            {
                int issueCode = 0;
                int.TryParse(ddlIssueGroups.SelectedValue, out issueCode);
                if (issueCode <= 0) return;

                int totalCount = 0;
                var pendingItems = ExcelAuditDal.GetIssues(issueCode, "Pending", "", 1, 25, out totalCount, ActiveSource);

                if (pendingItems == null || pendingItems.Count == 0)
                {
                    ShowAlert("لا توجد روابط بحالة 'معلقة' في هذا المعيار.", false);
                    return;
                }

                int resolvedCount = 0;
                int unresolvedCount = 0;

                foreach (var item in pendingItems)
                {
                    var res = SiteAuditRuleVerifier.VerifyUrlIssue(item.IssueCode, item.PageUrl);
                    ExcelAuditDal.UpdateVerificationStatus(item.Id, res.Status, res.ResponseCode, res.Details);
                    if (res.IsResolved) resolvedCount++;
                    else unresolvedCount++;
                }

                LoadKpisAndIssues();
                ShowAlert(string.Format("تم فحص {0} روابط معلقة بنجاح: ({1} تم حلها ✅، {2} لم تحل ❌).", pendingItems.Count, resolvedCount, unresolvedCount), true);
            }
            catch (Exception ex)
            {
                ShowAlert("خطأ أثناء فحص الروابط المعلقة: " + ex.Message, false);
            }
        }

        protected void rptIssues_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "VerifySingle")
            {
                try
                {
                    string[] parts = e.CommandArgument.ToString().Split('|');
                    if (parts.Length >= 3)
                    {
                        long id = Convert.ToInt64(parts[0]);
                        int issueCode = Convert.ToInt32(parts[1]);
                        string url = parts[2];

                        var res = SiteAuditRuleVerifier.VerifyUrlIssue(issueCode, url);
                        ExcelAuditDal.UpdateVerificationStatus(id, res.Status, res.ResponseCode, res.Details);

                        LoadKpisAndIssues();
                        ShowAlert(string.Format("تم فحص الرابط بنجاح: {0} ({1})", res.Details, res.IsResolved ? "✅ تم الحل" : "❌ لم يُحل بعد"), res.IsResolved);
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert("خطأ أثناء فحص الرابط: " + ex.Message, false);
                }
            }
        }

        public string GetStatusBadge(string status)
        {
            if (string.Equals(status, "Resolved", StringComparison.OrdinalIgnoreCase))
            {
                return "<span class=\"badge-status badge-resolved\">✅ تم الحل بنجاح</span>";
            }
            if (string.Equals(status, "Unresolved", StringComparison.OrdinalIgnoreCase))
            {
                return "<span class=\"badge-status badge-unresolved\">❌ لم يتم الحل بعد</span>";
            }
            return "<span class=\"badge-status badge-pending\">⏳ بانتظار الفحص</span>";
        }

        private void ShowAlert(string msg, bool isSuccess)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = isSuccess ? "alert-banner alert-success" : "alert-banner alert-info";
            litAlertMsg.Text = (isSuccess ? "✅ " : "⚠️ ") + msg;
        }
    }
}
