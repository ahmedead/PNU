using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using PNU.Internet.SearchIndex.DAL;
using PNU.Internet.SearchIndex.Logging;
using PNU.Internet.SearchIndex.Tasks;

namespace PNU.Internet.WebParts.Layouts.PNU.Internet.Search
{
    public partial class PageIndexAdmin : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.ScriptTimeout = 1800; // 30 minutes for large crawl

                if (SPContext.Current == null) return;

                if (!IsPostBack)
                {
                    litStatus.Text = "<div class='alert alert-info'>Ready. Enter target site URL (or leave blank for root) and click action.</div>";
                }
            }
            catch (Exception ex)
            {
                litStatus.Text = ErrorBlock("Page_Load", ex);
                SearchLogger.WriteToLog("UI", "PageIndexAdmin.Page_Load", ex.Message);
            }
        }

        protected void btnLoadPages_Click(object sender, EventArgs e)
        {
            try
            {
                string siteUrl = txtSiteUrl.Text.Trim();
                bool singleSiteOnly = chkCurrentSiteOnly.Checked;

                var pages = IndexingTasks.LoadWebPagesInfo(SPContext.Current.Site.ID, siteUrl, singleSiteOnly);

                gvPages.DataSource = pages;
                gvPages.DataBind();

                lblPageCount.Text = pages.Count.ToString();
                litStatus.Text = OkBlock("Loaded " + pages.Count + " pages for inspection. (Single site only: " + singleSiteOnly + ")");
            }
            catch (Exception ex)
            {
                litStatus.Text = ErrorBlock("LoadPages", ex);
                SearchLogger.WriteToLog("UI", "PageIndexAdmin.btnLoadPages_Click", ex.Message);
            }
        }

        protected void btnIndexPages_Click(object sender, EventArgs e)
        {
            try
            {
                string siteUrl = txtSiteUrl.Text.Trim();
                bool singleSiteOnly = chkCurrentSiteOnly.Checked;

                TaskResult r = IndexingTasks.RunWebsitePagesTask(SPContext.Current.Site.ID, siteUrl, singleSiteOnly);

                // Reload list to update status in grid
                btnLoadPages_Click(sender, e);

                string body = "<strong>Website Pages Indexing Completed</strong><br/>" + r.Summary;
                if (!string.IsNullOrEmpty(r.Errors))
                {
                    body += "<br/><pre style='max-height:300px;overflow:auto;background:#f6f6f6;padding:8px;'>"
                          + Server.HtmlEncode(r.Errors) + "</pre>";
                }

                litStatus.Text = "<div class='alert alert-success'>" + body + "</div>";
            }
            catch (Exception ex)
            {
                litStatus.Text = ErrorBlock("IndexPages", ex);
                SearchLogger.WriteToLog("UI", "PageIndexAdmin.btnIndexPages_Click", ex.Message);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string siteUrl = txtSiteUrl.Text.Trim();
                bool singleSiteOnly = chkCurrentSiteOnly.Checked;

                var pages = IndexingTasks.LoadWebPagesInfo(SPContext.Current.Site.ID, siteUrl, singleSiteOnly);

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment; filename=Website_Pages_Index_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                Response.Charset = "utf-8";
                Response.ContentEncoding = Encoding.UTF8;

                var sb = new StringBuilder();
                sb.AppendLine("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\">");
                sb.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                sb.AppendLine("<style>");
                sb.AppendLine("th { background-color: #0056b3; color: #ffffff; font-weight: bold; border: 1px solid #cccccc; text-align: center; padding: 8px; }");
                sb.AppendLine("td { border: 1px solid #cccccc; vertical-align: top; padding: 6px; }");
                sb.AppendLine(".indexed { background-color: #d4edda; color: #155724; font-weight: bold; }");
                sb.AppendLine(".not-indexed { background-color: #e2e3e5; color: #383d41; }");
                sb.AppendLine("</style></head><body>");

                sb.AppendLine("<h3>PNU Website Pages Index Report</h3>");
                sb.AppendLine("<p>Target Site: " + Server.HtmlEncode(string.IsNullOrEmpty(siteUrl) ? "Root Web" : siteUrl) + " | Single Site Only: " + singleSiteOnly + " | Export Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "</p>");

                sb.AppendLine("<table border='1'>");
                sb.AppendLine("<thead><tr>");
                sb.AppendLine("<th>#</th>");
                sb.AppendLine("<th>Status</th>");
                sb.AppendLine("<th>Page Title</th>");
                sb.AppendLine("<th>Page URL</th>");
                sb.AppendLine("<th>Page Layout</th>");
                sb.AppendLine("<th>User Control Path</th>");
                sb.AppendLine("<th>User Control Properties</th>");
                sb.AppendLine("<th>Web URL</th>");
                sb.AppendLine("<th>Last Indexed</th>");
                sb.AppendLine("</tr></thead><tbody>");

                int idx = 1;
                foreach (var p in pages)
                {
                    string statusClass = p.IsIndexed ? "indexed" : "not-indexed";
                    string statusText = p.IsIndexed ? "Indexed" : "Not Indexed";
                    string lastIndexedStr = p.LastIndexed > DateTime.MinValue ? p.LastIndexed.ToString("yyyy-MM-dd HH:mm") : "—";

                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td>" + idx++ + "</td>");
                    sb.AppendLine("<td class='" + statusClass + "'>" + statusText + "</td>");
                    sb.AppendLine("<td>" + Server.HtmlEncode(p.PageTitle ?? "") + "</td>");
                    sb.AppendLine("<td><a href='" + Server.HtmlEncode(p.PageURL ?? "") + "'>" + Server.HtmlEncode(p.PageURL ?? "") + "</a></td>");
                    sb.AppendLine("<td>" + Server.HtmlEncode(p.PageLayout ?? "") + "</td>");
                    sb.AppendLine("<td>" + Server.HtmlEncode(p.UserControlPath ?? "") + "</td>");
                    sb.AppendLine("<td>" + Server.HtmlEncode(p.UserControlProperties ?? "") + "</td>");
                    sb.AppendLine("<td>" + Server.HtmlEncode(p.WebUrl ?? "") + "</td>");
                    sb.AppendLine("<td>" + lastIndexedStr + "</td>");
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</tbody></table></body></html>");

                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                litStatus.Text = ErrorBlock("ExportExcel", ex);
                SearchLogger.WriteToLog("UI", "PageIndexAdmin.btnExportExcel_Click", ex.Message);
            }
        }

        protected void gvPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dto = e.Row.DataItem as WebsitePageDto;
                var lblBadge = e.Row.FindControl("lblStatusBadge") as Label;
                if (lblBadge != null && dto != null)
                {
                    if (dto.IsIndexed)
                    {
                        lblBadge.Text = "<span class='badge badge-success' style='background:#28a745;color:#fff;padding:4px 8px;border-radius:4px;'>Indexed</span>";
                    }
                    else
                    {
                        lblBadge.Text = "<span class='badge badge-secondary' style='background:#6c757d;color:#fff;padding:4px 8px;border-radius:4px;'>Not Indexed</span>";
                    }
                }
            }
        }

        private static string OkBlock(string msg)
        {
            return "<div class='alert alert-info'>" + msg + "</div>";
        }

        private static string ErrorBlock(string label, Exception ex)
        {
            return "<div class='alert alert-danger'><strong>" + label + "</strong><br/>" + ex.Message + "</div>";
        }
    }
}
