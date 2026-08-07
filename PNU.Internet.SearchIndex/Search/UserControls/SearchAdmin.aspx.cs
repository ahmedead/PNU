using System;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using PNU.Internet.Search.Config;
using PNU.Internet.Search.Provisioning;
using PNU.Internet.Search.Tasks;
using Portal.Main.Helper;

namespace PNU.Internet.Search.Layouts.PNU.Internet.Search
{
    public partial class SearchAdmin : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SPContext.Current.Web.UserIsSiteAdmin)
            {
                Response.StatusCode = 403;
                Response.End();
                return;
            }

            string configUrl = SPContext.Current.Site.RootWeb.Url.TrimEnd('/')
                             + "/" + SearchIndexingProvisioner.SUBSITE_URL;
            hlConfigSite.NavigateUrl = configUrl;
            hlConfigSite.Text = configUrl;

            if (!IsPostBack) ShowStatus();
        }

        // ================================================================
        // Task buttons
        // ================================================================
        protected void btnTaskMenus_Click(object sender, EventArgs e)
        {
            var r = IndexingTasks.RunMenusTask(SPContext.Current.Site.ID);
            ShowResult(r);
        }

        protected void btnTaskNews_Click(object sender, EventArgs e)
        {
            var r = IndexingTasks.RunNewsTask(SPContext.Current.Site.ID);
            ShowResult(r);
        }

        protected void btnTaskDigital_Click(object sender, EventArgs e)
        {
            var r = IndexingTasks.RunDigitalMediaTask(SPContext.Current.Site.ID);
            ShowResult(r);
        }

        protected void btnTaskEvents_Click(object sender, EventArgs e)
        {
            var r = IndexingTasks.RunEventsTask(SPContext.Current.Site.ID);
            ShowResult(r);
        }

        protected void btnTaskEServices_Click(object sender, EventArgs e)
        {
            var r = IndexingTasks.RunEServicesTask(SPContext.Current.Site.ID);
            ShowResult(r);
        }

        protected void btnTaskManual_Click(object sender, EventArgs e)
        {
            var r = IndexingTasks.RunManualPagesTask(SPContext.Current.Site.ID);
            ShowResult(r);
        }

        // ================================================================
        // Reset buttons - clear the IndexedWebs entries for one task only
        // ================================================================
        protected void btnResetMenus_Click(object sender, EventArgs e)
        {
            ResetTask(IndexingTasks.TASK_MENUS);
        }

        protected void btnResetNews_Click(object sender, EventArgs e)
        {
            ResetTask(IndexingTasks.TASK_NEWS + "::الأخبار");
        }

        protected void btnResetDigital_Click(object sender, EventArgs e)
        {
            ResetTask(IndexingTasks.TASK_DIGITAL_MEDIA + "::الوسائط الرقمية");
        }

        protected void btnResetEvents_Click(object sender, EventArgs e)
        {
            ResetTask(IndexingTasks.TASK_EVENTS + "::الفعاليات والإعلانات");
        }

        protected void btnResetEServices_Click(object sender, EventArgs e)
        {
            ResetTask(IndexingTasks.TASK_ESERVICES + "::الخدمات الإلكترونية");
        }

        private void ResetTask(string taskKey)
        {
            try
            {
                SearchConfigLoader.ClearTaskLog(SPContext.Current.Site, taskKey);
                litResult.Text =
                    "<div class='alert alert-info mt-3'>Cleared log entries for task <code>"
                    + Server.HtmlEncode(taskKey)
                    + "</code>. Next run will process all webs from scratch.</div>";
                ShowStatus();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("SearchAdmin.ResetTask", taskKey, ex.Message);
                litResult.Text = ErrorBlock(ex);
            }
        }

        // ================================================================
        // Configuration buttons
        // ================================================================
        protected void btnReloadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                SearchConfigLoader.InvalidateCache();
                SearchConfig cfg = SearchConfigLoader.Load(SPContext.Current.Site);
                litResult.Text = string.Format(
                    "<div class='alert alert-success mt-3'>" +
                    "Configuration reloaded. " +
                    "{0} content lists, {1} menu lists, {2} manual pages, " +
                    "{3} excluded webs, {4} excluded pages, {5} excluded lists." +
                    "</div>",
                    cfg.ContentLists.Count, cfg.MenuLists.Count,
                    cfg.ManualPages.Count, cfg.ExcludedWebs.Count,
                    cfg.ExcludedPages.Count, cfg.ExcludedLists.Count);
                ShowStatus();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("SearchAdmin.btnReloadConfig_Click",
                    "", ex.Message);
                litResult.Text = ErrorBlock(ex);
            }
        }

        protected void btnReprovision_Click(object sender, EventArgs e)
        {
            try
            {
                SearchIndexingProvisioner.EnsureAllForSite(SPContext.Current.Site);
                SearchConfigLoader.InvalidateCache();
                litResult.Text =
                    "<div class='alert alert-success mt-3'>" +
                    "Re-provisioning complete. Missing lists were created. " +
                    "Existing lists were left intact." +
                    "</div>";
                ShowStatus();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("SearchAdmin.btnReprovision_Click",
                    "", ex.Message);
                litResult.Text = ErrorBlock(ex);
            }
        }

        protected void btnClearAllLogs_Click(object sender, EventArgs e)
        {
            try
            {
                SearchConfigLoader.ClearTaskLog(SPContext.Current.Site, null);
                litResult.Text =
                    "<div class='alert alert-info mt-3'>" +
                    "All task logs cleared. Every task will re-run from scratch." +
                    "</div>";
                ShowStatus();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("SearchAdmin.btnClearAllLogs_Click",
                    "", ex.Message);
                litResult.Text = ErrorBlock(ex);
            }
        }

        // ================================================================
        // Helpers
        // ================================================================
        private void ShowResult(TaskResult r)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendFormat(
                    "<div class='alert alert-success mt-3'>" +
                    "<strong>Task: {0}</strong><br/>{1}",
                    Server.HtmlEncode(r.TaskName), r.Summary);

                if (!string.IsNullOrWhiteSpace(r.Errors))
                {
                    sb.Append("<details class='mt-2'><summary>Errors / details</summary>");
                    sb.AppendFormat("<pre style='white-space:pre-wrap'>{0}</pre>",
                        Server.HtmlEncode(r.Errors));
                    sb.Append("</details>");
                }
                sb.Append("</div>");
                litResult.Text = sb.ToString();
                ShowStatus();
            }
            catch (Exception ex)
            {
                litResult.Text = ErrorBlock(ex);
            }
        }

        private void ShowStatus()
        {
            try
            {
                SearchConfig cfg = SearchConfigLoader.Load(SPContext.Current.Site);
                var sb = new StringBuilder();
                sb.Append("<table class='table table-sm'>");
                sb.Append("<tr><th>Setting</th><th>Count</th></tr>");
                sb.AppendFormat("<tr><td>Content lists indexed</td><td><strong>{0}</strong></td></tr>",
                    cfg.ContentLists.Count);
                sb.AppendFormat("<tr><td>Menu lists crawled</td><td><strong>{0}</strong></td></tr>",
                    cfg.MenuLists.Count);
                sb.AppendFormat("<tr><td>Manual pages</td><td><strong>{0}</strong></td></tr>",
                    cfg.ManualPages.Count);
                sb.AppendFormat("<tr><td>Excluded sub-webs</td><td><strong>{0}</strong></td></tr>",
                    cfg.ExcludedWebs.Count);
                sb.AppendFormat("<tr><td>Excluded pages</td><td><strong>{0}</strong></td></tr>",
                    cfg.ExcludedPages.Count);
                sb.AppendFormat("<tr><td>Excluded lists</td><td><strong>{0}</strong></td></tr>",
                    cfg.ExcludedLists.Count);
                sb.Append("</table>");
                litStatus.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                litStatus.Text = "<em>Could not load status: "
                               + Server.HtmlEncode(ex.Message) + "</em>";
            }
        }

        private string ErrorBlock(Exception ex)
        {
            return "<div class='alert alert-danger mt-3'>Error: "
                 + Server.HtmlEncode(ex.Message) + "</div>";
        }
    }
}
