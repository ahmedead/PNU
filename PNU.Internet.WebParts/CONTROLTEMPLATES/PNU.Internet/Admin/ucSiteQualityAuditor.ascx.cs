using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucSiteQualityAuditor : UserControl
    {
        private const int PageSize = 25;

        private int CurrentPageIndex
        {
            get
            {
                object o = ViewState["CurrentPageIndex"];
                return (o != null) ? (int)o : 0;
            }
            set
            {
                ViewState["CurrentPageIndex"] = value;
            }
        }

        private long CurrentScanId
        {
            get
            {
                object o = ViewState["CurrentScanId"];
                return (o != null) ? (long)o : 0;
            }
            set
            {
                ViewState["CurrentScanId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        SiteAuditReportDal.EnsureTablesExist();
                    });

                    // BindDashboard also fills lblLastScan, so there is no separate
                    // LoadLatestScanSession round-trip here.
                    BindDashboard();
                    BindIssuesGrid();
                }
                catch (Exception ex)
                {
                    lblScanStatus.Text = "تنبيه أثناء الاتصال بقاعدة البيانات: " + ex.Message;
                    lblScanStatus.ForeColor = System.Drawing.Color.Crimson;
                }
            }
        }

        private void BindDashboard()
        {
            try
            {
                AuditScanSummaryDto summary = SiteAuditReportDal.GetLatestScanSummary();
                if (summary != null)
                {
                    CurrentScanId = summary.ScanId;
                    litHealthScore.Text = string.Format("{0:0.0}%", summary.HealthScorePercentage);
                    litTotalPages.Text = summary.TotalPagesScanned.ToString("N0");
                    litCriticalCount.Text = summary.CriticalCount.ToString("N0");
                    litWarningCount.Text = summary.WarningCount.ToString("N0");
                    litInfoCount.Text = summary.InfoCount.ToString("N0");
                    litPassedCount.Text = summary.PassedCount.ToString("N0");

                    lblLastScan.Text = string.Format("آخر فحص: {0:yyyy-MM-dd HH:mm} (الحالة: {1})", summary.StartTime, summary.Status);
                }
                else
                {
                    litHealthScore.Text = "100%";
                    litTotalPages.Text = "0";
                    litCriticalCount.Text = "0";
                    litWarningCount.Text = "0";
                    litInfoCount.Text = "0";
                    litPassedCount.Text = "0";
                    lblLastScan.Text = "لم يتم إجراء أي فحص مسبق بعد.";
                }
            }
            catch (Exception ex)
            {
                lblScanStatus.Text = "خطأ في عرض إحصائيات المؤشرات: " + ex.Message;
                lblScanStatus.ForeColor = System.Drawing.Color.Crimson;
            }
        }

        private void BindIssuesGrid()
        {
            try
            {
                string category = ddlCategory.SelectedValue == "All" ? null : ddlCategory.SelectedValue;
                string severity = ddlSeverity.SelectedValue == "All" ? null : ddlSeverity.SelectedValue;
                string search = string.IsNullOrEmpty(txtSearch.Text) ? null : txtSearch.Text.Trim();

                int totalCount = 0;
                List<AuditIssueDto> issues = SiteAuditReportDal.GetFilteredIssues(CurrentScanId, category, severity, null, search, CurrentPageIndex, PageSize, out totalCount);

                if (issues != null && issues.Count > 0)
                {
                    rptIssues.DataSource = issues;
                    rptIssues.DataBind();
                    rptIssues.Visible = true;
                    pnlNoIssues.Visible = false;
                }
                else
                {
                    rptIssues.Visible = false;
                    pnlNoIssues.Visible = true;
                }

                int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
                if (totalPages == 0) totalPages = 1;

                lblRecordCount.Text = string.Format("إجمالي الملاحظات: {0:N0} (الصفحة {1} من {2})", totalCount, CurrentPageIndex + 1, totalPages);
                lblCurrentPage.Text = string.Format("{0} / {1}", CurrentPageIndex + 1, totalPages);

                btnPrevPage.Enabled = (CurrentPageIndex > 0);
                btnNextPage.Enabled = (CurrentPageIndex + 1 < totalPages);
            }
            catch (Exception ex)
            {
                lblScanStatus.Text = "خطأ أثناء استعلام جدول الملاحظات: " + ex.Message;
                lblScanStatus.ForeColor = System.Drawing.Color.Crimson;
            }
        }

        protected void btnStartScan_Click(object sender, EventArgs e)
        {
            try
            {
                // Walking every publishing page in the site collection takes far longer
                // than the default 110-second request timeout.
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Server.ScriptTimeout = 3600;
                }

                string scope = ddlScanScope.SelectedValue;
                string currentUser = SPContext.Current != null && SPContext.Current.Web != null && SPContext.Current.Web.CurrentUser != null
                    ? SPContext.Current.Web.CurrentUser.LoginName
                    : "System";

                AuditScanSummaryDto result = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    result = SiteAuditScannerEngine.RunAuditScan(scope, currentUser, null);
                });

                if (result == null)
                {
                    lblScanStatus.Text = "❌ انتهى الفحص دون تسجيل أي نتائج في قاعدة البيانات. تحقق من إعدادات الاتصال بقاعدة البيانات.";
                    lblScanStatus.ForeColor = System.Drawing.Color.Crimson;
                    return;
                }

                CurrentScanId = result.ScanId;

                if (result.TotalPagesScanned == 0)
                {
                    lblScanStatus.Text = "⚠️ اكتمل الفحص لكن لم يتم العثور على أي صفحة نشر ضمن النطاق المحدد.";
                    lblScanStatus.ForeColor = System.Drawing.Color.DarkOrange;
                }
                else
                {
                    lblScanStatus.Text = string.Format(
                        "✅ اكتمل الفحص: {0:N0} صفحة، {1:N0} ملاحظة مسجلة في قاعدة البيانات.",
                        result.TotalPagesScanned, result.TotalIssuesFound);
                    lblScanStatus.ForeColor = System.Drawing.Color.FromArgb(5, 150, 105);
                }

                CurrentPageIndex = 0;
                BindDashboard();
                BindIssuesGrid();
            }
            catch (Exception ex)
            {
                lblScanStatus.Text = "❌ حدث خطأ أثناء الفحص: " + ex.Message;
                lblScanStatus.ForeColor = System.Drawing.Color.Crimson;
            }
        }

        protected void FilterChanged(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            BindIssuesGrid();
        }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 0)
            {
                CurrentPageIndex--;
                BindIssuesGrid();
            }
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            CurrentPageIndex++;
            BindIssuesGrid();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = SiteAuditReportDal.ExportIssuesToDataTable(CurrentScanId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    lblScanStatus.Text = "لا توجد بيانات للتصدير.";
                    return;
                }

                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Buffer = true;
                response.Charset = "utf-8";
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("content-disposition", string.Format("attachment;filename=PNU_Site_Quality_Audit_{0:yyyyMMdd_HHmm}.xls", DateTime.Now));

                // Write UTF-8 BOM
                response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        // Write HTML Table for Excel
                        htw.WriteLine("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\" dir=\"rtl\">");
                        htw.WriteLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                        htw.WriteLine("<style>");
                        htw.WriteLine("th { background-color: #10753a; color: #ffffff; font-weight: bold; border: 1px solid #0d5c2e; padding: 8px; }");
                        htw.WriteLine("td { border: 1px solid #e2e8f0; padding: 6px; font-family: Segoe UI, Tahoma, sans-serif; font-size: 11pt; }");
                        htw.WriteLine(".critical { background-color: #fee2e2; color: #991b1b; }");
                        htw.WriteLine(".warning { background-color: #fef3c7; color: #92400e; }");
                        htw.WriteLine(".info { background-color: #dbeafe; color: #1e40af; }");
                        htw.WriteLine("</style></head><body>");

                        htw.WriteLine("<h2>تقرير تدقيق جودة الموقع والـ SEO - جامعة الأميرة نورة بنت عبدالرحمن</h2>");
                        htw.WriteLine("<table border=\"1\">");
                        htw.WriteLine("<thead><tr>");
                        foreach (DataColumn col in dt.Columns)
                        {
                            htw.WriteLine("<th>" + HttpUtility.HtmlEncode(col.ColumnName) + "</th>");
                        }
                        htw.WriteLine("</tr></thead><tbody>");

                        foreach (DataRow row in dt.Rows)
                        {
                            htw.WriteLine("<tr>");
                            foreach (DataColumn col in dt.Columns)
                            {
                                string val = row[col] != null ? row[col].ToString() : string.Empty;
                                string cssClass = string.Empty;
                                if (col.ColumnName == "Severity")
                                {
                                    if (val == "Critical") cssClass = " class=\"critical\"";
                                    else if (val == "Warning") cssClass = " class=\"warning\"";
                                    else if (val == "Info") cssClass = " class=\"info\"";
                                }
                                htw.WriteLine("<td" + cssClass + ">" + HttpUtility.HtmlEncode(val) + "</td>");
                            }
                            htw.WriteLine("</tr>");
                        }

                        htw.WriteLine("</tbody></table></body></html>");
                    }
                    response.Write(sw.ToString());
                }

                response.Flush();
                response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Expected when calling Response.End()
            }
            catch (Exception ex)
            {
                lblScanStatus.Text = "خطأ أثناء تصدير ملف Excel: " + ex.Message;
                lblScanStatus.ForeColor = System.Drawing.Color.Crimson;
            }
        }

        protected void btnGenerateSitemap_Click(object sender, EventArgs e)
        {
            try
            {
                string baseUrl = "https://pnu.edu.sa";
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    baseUrl = SPContext.Current.Site.Url;
                }

                string sitemapXml = SiteAuditSitemapGenerator.GenerateSitemapXml(baseUrl);

                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Buffer = true;
                response.Charset = "utf-8";
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = "text/xml; charset=utf-8";
                response.AddHeader("content-disposition", "attachment;filename=sitemap.xml");

                response.Write(sitemapXml);
                response.Flush();
                response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Expected when calling Response.End()
            }
            catch (Exception ex)
            {
                lblScanStatus.Text = "خطأ أثناء توليد خريطة الموقع: " + ex.Message;
                lblScanStatus.ForeColor = System.Drawing.Color.Crimson;
            }
        }

        public string GetSeverityBadgeClass(string severity)
        {
            if (string.IsNullOrEmpty(severity)) return "pnu-badge-code";
            switch (severity.Trim().ToLower())
            {
                case "critical": return "pnu-badge-critical";
                case "warning": return "pnu-badge-warning";
                case "info": return "pnu-badge-info";
                default: return "pnu-badge-code";
            }
        }
    }
}
