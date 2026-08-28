using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using PNU.Internet.SearchIndex.DAL;
using PNU.Internet.SearchIndex.Indexer;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;


using System.Text;

using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.WebParts.Layouts.PNU.Internet.Search
{
    public partial class PageIndexAdmin : LayoutsPageBase
    {
        // --- transient state kept across postbacks in ViewState ---------
        private const string VS_PAGES = "PageIndexAdmin.LoadedPages";

        private List<PageRow> LoadedPages
        {
            get
            {
                return ViewState[VS_PAGES] as List<PageRow>
                       ?? new List<PageRow>();
            }
            set { ViewState[VS_PAGES] = value; }
        }

        // ----------------------------------------------------------------
        [Serializable]
        public class PageRow
        {
            public string PageTitle { get; set; }
            public string PageURL { get; set; }
            public string PageLayout { get; set; }
            public string UserControlPath { get; set; }
            public string UserControlProperties { get; set; }
            public string WebUrl { get; set; }

            // English mirror
            public string PageTitleEn { get; set; }
            public string PageURLEn { get; set; }
            public string PageLayoutEn { get; set; }
            public string UserControlPathEn { get; set; }
            public string UserControlPropertiesEn { get; set; }
            public string WebUrlEn { get; set; }
            public bool EnExists { get; set; }
            public string EnStatus { get; set; }

            public string Status { get; set; }  // Indexed / Not Indexed
            public string LastIndexedDisplay { get; set; }
        }

        // ================================================================
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 1800;

            try
            {
                string schemaError;
                WebsitePagesDal.EnsureWebsitePagesTableExists(out schemaError);

                if (!string.IsNullOrEmpty(schemaError))
                {
                    litStatus.Text = Error2(
                        "Could not create or upgrade <code>dbo.WebsitePages</code>. "
                        + "Indexing will not save anything until this is fixed."
                        + "<br/>Error: <code>"
                        + Server.HtmlEncode(schemaError) + "</code>"
                        + "<br/>Check that the <code>PNU_SearchIndex</code> "
                        + "connection string exists in this web application's "
                        + "web.config and that the application pool account has "
                        + "db_owner (or at least CREATE TABLE / CREATE PROCEDURE) "
                        + "on the database.");
                    return;
                }
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("Ensuring WebsitePages table", ex);
                SearchLogger.WriteToLog("UI",
                    "PageIndexAdmin.EnsureTable", ex.Message);
                return;
            }

            if (!IsPostBack)
                litStatus.Text = Info(
                    "Enter a target web (or leave blank for site root) and " +
                    "click <strong>Load &amp; Check Pages</strong>.");
        }

        // ================================================================
        protected void btnLoadPages_Click(object sender, EventArgs e)
        {
            try
            {
                bool webNotFound, webExcluded;
                string resolvedWebUrl;
                LoadedPages = LoadPagesFromSharePoint(
                    out webNotFound, out webExcluded, out resolvedWebUrl);
                BindGrid();

                if (webExcluded)
                {
                    litStatus.Text = Warn(
                        "That web is on the catalog exclusion list and was "
                        + "skipped. Excluded: /ar/Announcements, "
                        + "/ar/VirtualTour, /en/VirtualTour, /ar/NewStudents, "
                        + "/ar/NewsActivities, /ar/ITAdmin, /ar/ContentAdmin, "
                        + "/en/NewsActivities.");
                    return;
                }

                if (webNotFound)
                {
                    litStatus.Text = Warn(
                        "No SharePoint web was found at that URL. "
                        + "The path may be a folder inside the root web's "
                        + "<code>Pages</code> library, not a separate subsite. "
                        + "Try the parent web URL (e.g. <code>/</code>) or "
                        + "the exact server-relative URL of the subsite "
                        + "you want to scan.");
                    return;
                }

                string msg = LoadedPages.Count + " page(s) discovered";
                if (!string.IsNullOrEmpty(resolvedWebUrl))
                    msg += " under <code>" + Server.HtmlEncode(resolvedWebUrl)
                         + "</code>";
                msg += ".";
                litStatus.Text = Ok(msg);
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("Load pages", ex);
                SearchLogger.WriteToLog("UI",
                    "PageIndexAdmin.Load", ex.Message);
            }
        }

        // ================================================================
        protected void btnIndexPages_Click(object sender, EventArgs e)
        {
            try
            {
                var pages = LoadedPages;
                bool webNotFound = false, webExcluded = false;
                string resolvedWebUrl = null;
                if (pages.Count == 0)
                {
                    pages = LoadPagesFromSharePoint(
                        out webNotFound, out webExcluded, out resolvedWebUrl);
                }

                if (webExcluded)
                {
                    litStatus.Text = Warn("That web is excluded from the "
                        + "catalog. Nothing was indexed.");
                    return;
                }

                if (webExcluded)
                {
                    litStatus.Text = Warn(
                        "That web is on the catalog exclusion list and was "
                        + "skipped. Excluded: /ar/Announcements, "
                        + "/ar/VirtualTour, /en/VirtualTour, /ar/NewStudents, "
                        + "/ar/NewsActivities, /ar/ITAdmin, /ar/ContentAdmin, "
                        + "/en/NewsActivities.");
                    return;
                }

                if (webNotFound)
                {
                    litStatus.Text = Warn(
                        "No SharePoint web was found at that URL. "
                        + "Nothing was indexed.");
                    return;
                }

                int ok = 0, failed = 0;
                string firstError = null;

                foreach (var p in pages)
                {
                    string err;
                    bool written = WebsitePagesDal.UpsertWebsitePage(
                        p.PageTitle, p.PageURL, p.PageLayout,
                        p.UserControlPath, p.UserControlProperties,
                        p.WebUrl,
                        p.PageTitleEn, p.PageURLEn, p.PageLayoutEn,
                        p.UserControlPathEn, p.UserControlPropertiesEn,
                        p.WebUrlEn, p.EnExists, out err);

                    if (written) ok++;
                    else
                    {
                        failed++;
                        if (firstError == null) firstError = err;
                    }
                }

                // Refresh status column from DB after upsert
                LoadedPages = MergeWithIndexStatus(pages);
                BindGrid();

                int rowCount = WebsitePagesDal.GetRowCount();

                if (failed == 0)
                {
                    litStatus.Text = Ok(ok + " page(s) written to "
                        + "<code>dbo.WebsitePages</code>. Table now holds "
                        + rowCount + " row(s).");
                }
                else
                {
                    litStatus.Text = Error2(
                        ok + " succeeded, <strong>" + failed
                        + " failed</strong>. Table holds " + rowCount
                        + " row(s).<br/>First error: <code>"
                        + Server.HtmlEncode(firstError ?? "(none)")
                        + "</code>");
                }
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("Run indexing", ex);
                SearchLogger.WriteToLog("UI",
                    "PageIndexAdmin.Index", ex.Message);
            }
        }

        // ================================================================
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Prefer live loaded rows; fall back to whatever is in the DB
                var pages = LoadedPages;
                if (pages == null || pages.Count == 0)
                {
                    var db = WebsitePagesDal.GetAll();
                    pages = new List<PageRow>();
                    foreach (var d in db)
                    {
                        pages.Add(new PageRow
                        {
                            PageTitle = d.PageTitle,
                            PageURL = d.PageURL,
                            PageLayout = d.PageLayout,
                            UserControlPath = d.UserControlPath,
                            UserControlProperties = d.UserControlProperties,
                            WebUrl = d.WebUrl,
                            PageTitleEn = d.PageTitleEn,
                            PageURLEn = d.PageURLEn,
                            PageLayoutEn = d.PageLayoutEn,
                            UserControlPathEn = d.UserControlPathEn,
                            UserControlPropertiesEn = d.UserControlPropertiesEn,
                            WebUrlEn = d.WebUrlEn,
                            EnExists = d.EnExists,
                            EnStatus = d.EnExists
                                                        ? "EN found" : "EN missing",
                            Status = "Indexed",
                            LastIndexedDisplay = d.LastIndexed
                                .ToString("yyyy-MM-dd HH:mm")
                        });
                    }
                }

                string html = BuildExcelHtml(pages);
                string fileName = "PageCatalog_"
                    + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xls";

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "utf-8";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AddHeader("Content-Disposition",
                    "attachment; filename=" + fileName);
                Response.Write("\uFEFF"); // BOM so Excel picks up UTF-8
                Response.Write(html);
                Response.Flush();
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("Export to Excel", ex);
                SearchLogger.WriteToLog("UI",
                    "PageIndexAdmin.Export", ex.Message);
            }
        }

        // ================================================================
        protected void btnTestDb_Click(object sender, EventArgs e)
        {
            string err = WebsitePagesDal.TestConnection();
            if (string.IsNullOrEmpty(err))
            {
                int n = WebsitePagesDal.GetRowCount();
                litStatus.Text = Ok("Database reachable. "
                    + "<code>dbo.WebsitePages</code> currently holds "
                    + n + " row(s).");
            }
            else
            {
                litStatus.Text = Error2(
                    "Cannot reach <code>dbo.WebsitePages</code>.<br/>Error: "
                    + "<code>" + Server.HtmlEncode(err) + "</code>");
            }
        }

        // ================================================================
        protected void gvPages_RowDataBound(object sender,
            GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var lbl = e.Row.FindControl("lblStatus") as Label;
            if (lbl == null) return;

            if (string.Equals(lbl.Text, "Indexed",
                    StringComparison.OrdinalIgnoreCase))
                lbl.CssClass = "badge bg-success";
            else
                lbl.CssClass = "badge bg-warning text-dark";

            var lblEn = e.Row.FindControl("lblEnStatus") as Label;
            if (lblEn != null)
            {
                lblEn.CssClass = lblEn.Text.IndexOf("found",
                    StringComparison.OrdinalIgnoreCase) >= 0
                    ? "badge bg-success"
                    : "badge bg-secondary";
            }
        }

        // ================================================================
        private List<PageRow> LoadPagesFromSharePoint(
            out bool webNotFound, out bool webExcluded,
            out string resolvedWebUrl)
        {
            string targetUrl = (txtSiteUrl.Text ?? "").Trim();
            bool currentOnly = chkCurrentSiteOnly.Checked;

            PageInspector.InspectionResult inspection = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    inspection = PageInspector.InspectPages(
                        site, targetUrl, currentOnly);
                }
            });

            if (inspection == null)
                inspection = new PageInspector.InspectionResult();

            webNotFound = inspection.WebNotFound;
            webExcluded = inspection.WebExcluded;
            resolvedWebUrl = inspection.ResolvedWebUrl;

            var raw = new List<PageRow>();
            foreach (var ip in inspection.Pages)
            {
                raw.Add(new PageRow
                {
                    PageTitle = ip.PageTitle,
                    PageURL = ip.PageURL,
                    PageLayout = ip.PageLayout,
                    UserControlPath = ip.UserControlPath,
                    UserControlProperties = ip.UserControlProperties,
                    WebUrl = ip.WebUrl,
                    PageTitleEn = ip.PageTitleEn,
                    PageURLEn = ip.PageURLEn,
                    PageLayoutEn = ip.PageLayoutEn,
                    UserControlPathEn = ip.UserControlPathEn,
                    UserControlPropertiesEn = ip.UserControlPropertiesEn,
                    WebUrlEn = ip.WebUrlEn,
                    EnExists = ip.EnExists,
                    EnStatus = ip.EnExists ? "EN found" : "EN missing",
                    Status = "Not Indexed",
                    LastIndexedDisplay = ""
                });
            }
            return MergeWithIndexStatus(raw);
        }

        private static List<PageRow> MergeWithIndexStatus(List<PageRow> pages)
        {
            if (pages == null || pages.Count == 0)
                return pages ?? new List<PageRow>();

            var indexedMap = WebsitePagesDal.GetIndexedWebsitePagesMap();
            foreach (var p in pages)
            {
                WebsitePageDto hit;
                if (!string.IsNullOrEmpty(p.PageURL)
                    && indexedMap.TryGetValue(p.PageURL, out hit))
                {
                    p.Status = "Indexed";
                    p.LastIndexedDisplay = hit.LastIndexed
                        .ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    p.Status = "Not Indexed";
                    p.LastIndexedDisplay = "";
                }
            }
            return pages;
        }

        private void BindGrid()
        {
            gvPages.DataSource = LoadedPages;
            gvPages.DataBind();
        }

        // ================================================================
        // Formatted Excel via HTML table + application/vnd.ms-excel MIME.
        // Excel honours the inline CSS below.
        // ================================================================
        private static string BuildExcelHtml(List<PageRow> pages)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sb.AppendLine("<head><meta charset=\"utf-8\" /><style>");
            sb.AppendLine("  body { font-family: Segoe UI, Arial, sans-serif; font-size: 11pt; }");
            sb.AppendLine("  h2   { color: #005c39; }");
            sb.AppendLine("  table { border-collapse: collapse; width: 100%; }");
            sb.AppendLine("  th { background: #005c39; color: #fff; " +
                          "padding: 6px 8px; text-align: left; border: 1px solid #002a1a; }");
            sb.AppendLine("  td { padding: 6px 8px; border: 1px solid #cfd6df; " +
                          "vertical-align: top; }");
            sb.AppendLine("  tr:nth-child(even) td { background: #f5f7fa; }");
            sb.AppendLine("  td.status-indexed    { color: #0a6c2a; font-weight: bold; }");
            sb.AppendLine("  td.status-notindexed { color: #a15c00; font-weight: bold; }");
            sb.AppendLine("  pre { margin: 0; font-family: Consolas, monospace; " +
                          "font-size: 10pt; white-space: pre-wrap; }");
            sb.AppendLine("</style></head><body>");

            sb.AppendLine("<h2>PNU - Page Catalog Export</h2>");
            sb.AppendLine("<p>Generated: "
                + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | "
                + pages.Count + " page(s)</p>");

            sb.AppendLine("<table>");
            sb.AppendLine("<thead><tr>" +
                "<th>Title (AR)</th><th>Page URL (AR)</th><th>Layout (AR)</th>" +
                "<th>User Controls (AR)</th><th>Properties (AR)</th>" +
                "<th>Web (AR)</th>" +
                "<th>Title (EN)</th><th>Page URL (EN)</th><th>Layout (EN)</th>" +
                "<th>User Controls (EN)</th><th>Properties (EN)</th>" +
                "<th>Web (EN)</th><th>EN status</th>" +
                "<th>Status</th><th>Last indexed</th>" +
                "</tr></thead><tbody>");

            foreach (var p in pages)
            {
                bool indexed = string.Equals(p.Status, "Indexed",
                    StringComparison.OrdinalIgnoreCase);
                string statusClass = indexed
                    ? "status-indexed" : "status-notindexed";

                sb.AppendLine("<tr>");
                sb.Append("<td>").Append(H(p.PageTitle)).AppendLine("</td>");
                sb.Append("<td><a href=\"").Append(H(p.PageURL))
                  .Append("\">").Append(H(p.PageURL)).AppendLine("</a></td>");
                sb.Append("<td>").Append(H(p.PageLayout)).AppendLine("</td>");
                sb.Append("<td><pre>").Append(H(p.UserControlPath))
                  .AppendLine("</pre></td>");
                sb.Append("<td><pre>").Append(H(p.UserControlProperties))
                  .AppendLine("</pre></td>");
                sb.Append("<td>").Append(H(p.WebUrl)).AppendLine("</td>");

                sb.Append("<td>").Append(H(p.PageTitleEn)).AppendLine("</td>");
                sb.Append("<td><a href=\"").Append(H(p.PageURLEn))
                  .Append("\">").Append(H(p.PageURLEn)).AppendLine("</a></td>");
                sb.Append("<td>").Append(H(p.PageLayoutEn)).AppendLine("</td>");
                sb.Append("<td><pre>").Append(H(p.UserControlPathEn))
                  .AppendLine("</pre></td>");
                sb.Append("<td><pre>").Append(H(p.UserControlPropertiesEn))
                  .AppendLine("</pre></td>");
                sb.Append("<td>").Append(H(p.WebUrlEn)).AppendLine("</td>");
                sb.Append("<td class=\"")
                  .Append(p.EnExists ? "status-indexed" : "status-notindexed")
                  .Append("\">").Append(H(p.EnStatus)).AppendLine("</td>");

                sb.Append("<td class=\"").Append(statusClass).Append("\">")
                  .Append(H(p.Status)).AppendLine("</td>");
                sb.Append("<td>").Append(H(p.LastIndexedDisplay))
                  .AppendLine("</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table></body></html>");
            return sb.ToString();
        }

        private static string H(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return System.Web.HttpUtility.HtmlEncode(s);
        }

        // ================================================================
        private static string Ok(string msg)
        {
            return "<div class='alert alert-success'>" + msg + "</div>";
        }

        private static string Info(string msg)
        {
            return "<div class='alert alert-info'>" + msg + "</div>";
        }

        private static string Warn(string msg)
        {
            return "<div class='alert alert-warning'>" + msg + "</div>";
        }

        private static string Error2(string html)
        {
            return "<div class='alert alert-danger'>" + html + "</div>";
        }

        private static string Error(string label, Exception ex)
        {
            return "<div class='alert alert-danger'><strong>" + label
                + "</strong><br/>" + System.Web.HttpUtility.HtmlEncode(ex.Message)
                + "</div>";
        }
    }

}
