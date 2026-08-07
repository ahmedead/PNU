using System;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using PNU.Internet.SearchIndex.Config;
using PNU.Internet.SearchIndex.Logging;
using PNU.Internet.SearchIndex.Provisioning;
using PNU.Internet.SearchIndex.Tasks;

namespace PNU.Internet.WebParts.Layouts.PNU.Internet.Search
{
    public partial class SearchAdmin : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Long-running crawls need more than the IIS default
                Server.ScriptTimeout = 1800; // 30 minutes

                if (SPContext.Current == null) return;

                string configUrl = SPContext.Current.Site.Url.TrimEnd('/')
                    + "/" + SearchIndexingProvisioner.SUBSITE_URL;
                hlConfigSite.NavigateUrl = configUrl;
                hlConfigSite.Text = configUrl;

                if (!IsPostBack)
                    litStatus.Text = "<div class='alert alert-info'>Ready.</div>";
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("Page_Load", ex);
                SearchLogger.WriteToLog("UI", "SearchAdmin.Page_Load", ex.Message);
            }
        }

        // ================================================================
        protected void btnTaskMenus_Click(object sender, EventArgs e)
        { RunTask("Menus", () => IndexingTasks.RunMenusTask(SPContext.Current.Site.ID)); }

        protected void btnTaskNews_Click(object sender, EventArgs e)
        { RunTask("News", () => IndexingTasks.RunNewsTask(SPContext.Current.Site.ID)); }

        protected void btnTaskDigital_Click(object sender, EventArgs e)
        { RunTask("Digital Media", () => IndexingTasks.RunDigitalMediaTask(SPContext.Current.Site.ID)); }

        protected void btnTaskEvents_Click(object sender, EventArgs e)
        { RunTask("Events", () => IndexingTasks.RunEventsTask(SPContext.Current.Site.ID)); }

        protected void btnTaskEServices_Click(object sender, EventArgs e)
        { RunTask("E-Services", () => IndexingTasks.RunEServicesTask(SPContext.Current.Site.ID)); }

        protected void btnTaskFaculties_Click(object sender, EventArgs e)
        { RunTask("Faculties", () => IndexingTasks.RunFacultiesTask(SPContext.Current.Site.ID)); }

        protected void btnTaskManual_Click(object sender, EventArgs e)
        { RunTask("Manual Pages", () => IndexingTasks.RunManualPagesTask(SPContext.Current.Site.ID)); }

        protected void btnTaskRetry_Click(object sender, EventArgs e)
        { RunTask("Retry Failed", () => IndexingTasks.RunRetryFailedTask(SPContext.Current.Site.ID)); }

        // ================================================================
        protected void btnResetMenus_Click(object sender, EventArgs e)
        { ResetTask(IndexingTasks.TASK_MENUS); }

        protected void btnResetNews_Click(object sender, EventArgs e)
        { ResetTaskByPrefix(IndexingTasks.TASK_NEWS); }

        protected void btnResetDigital_Click(object sender, EventArgs e)
        { ResetTaskByPrefix(IndexingTasks.TASK_DIGITAL_MEDIA); }

        protected void btnResetEvents_Click(object sender, EventArgs e)
        { ResetTaskByPrefix(IndexingTasks.TASK_EVENTS); }

        protected void btnResetEServices_Click(object sender, EventArgs e)
        { ResetTaskByPrefix(IndexingTasks.TASK_ESERVICES); }

        protected void btnResetFaculties_Click(object sender, EventArgs e)
        { ResetTask(IndexingTasks.TASK_FACULTIES); }

        // ================================================================
        protected void btnReloadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                SearchConfigLoader.InvalidateCache();
                SearchConfigLoader.Load(SPContext.Current.Site);
                litStatus.Text = Ok("Configuration reloaded.");
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("ReloadConfig", ex);
                SearchLogger.WriteToLog("UI", "ReloadConfig", ex.Message);
            }
        }

        protected void btnReprovision_Click(object sender, EventArgs e)
        {
            try
            {
                SearchIndexingProvisioner.EnsureAll();
                SearchConfigLoader.InvalidateCache();
                litStatus.Text = Ok("Provisioner ran. Lists checked / created.");
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("Reprovision", ex);
                SearchLogger.WriteToLog("UI", "Reprovision", ex.Message);
            }
        }

        protected void btnClearAllLogs_Click(object sender, EventArgs e)
        {
            try
            {
                SearchConfigLoader.ClearTaskLog(SPContext.Current.Site, null);
                litStatus.Text = Ok("All task logs cleared.");
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("ClearAllLogs", ex);
                SearchLogger.WriteToLog("UI", "ClearAllLogs", ex.Message);
            }
        }

        // ================================================================
        private void RunTask(string label, Func<TaskResult> action)
        {
            try
            {
                TaskResult r = action();
                string body = "<strong>" + label + "</strong><br/>" + r.Summary;
                if (!string.IsNullOrEmpty(r.Errors))
                    body += "<br/><pre style='max-height:300px;"
                          + "overflow:auto;background:#f6f6f6;padding:8px;'>"
                          + Server.HtmlEncode(r.Errors) + "</pre>";

                litResult.Text = "<div class='alert alert-success'>"
                    + body + "</div>";
                litStatus.Text = Ok(label + " completed.");
            }
            catch (Exception ex)
            {
                litResult.Text = Error(label, ex);
                SearchLogger.WriteToLog("UI", "RunTask " + label, ex.Message);
            }
        }

        private void ResetTask(string taskName)
        {
            try
            {
                SearchConfigLoader.ClearTaskLog(SPContext.Current.Site, taskName);
                litStatus.Text = Ok("Reset log for task '" + taskName + "'.");
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("Reset " + taskName, ex);
                SearchLogger.WriteToLog("UI", "Reset " + taskName, ex.Message);
            }
        }

        private void ResetTaskByPrefix(string prefix)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        string url = site.RootWeb.ServerRelativeUrl.TrimEnd('/')
                            + "/" + SearchIndexingProvisioner.SUBSITE_URL;
                        using (SPWeb sub = site.OpenWeb(url))
                        {
                            if (sub == null) return;
                            sub.AllowUnsafeUpdates = true;
                            try
                            {
                                SPList list = sub.Lists.TryGetList(
                                    SearchIndexingProvisioner.LIST_INDEXED_WEBS);
                                if (list == null) return;

                                var ids = new System.Collections.Generic.List<int>();
                                foreach (SPListItem it in list.Items)
                                {
                                    string t = Convert.ToString(it["Title"]) ?? "";
                                    if (t.StartsWith(prefix,
                                            StringComparison.OrdinalIgnoreCase))
                                        ids.Add(it.ID);
                                }
                                foreach (int id in ids)
                                    list.Items.DeleteItemById(id);
                            }
                            finally { sub.AllowUnsafeUpdates = false; }
                        }
                    }
                });
                litStatus.Text = Ok("Reset all log entries that start with '"
                    + prefix + "'.");
            }
            catch (Exception ex)
            {
                litStatus.Text = Error("ResetByPrefix " + prefix, ex);
                SearchLogger.WriteToLog("UI",
                    "ResetByPrefix " + prefix, ex.Message);
            }
        }

        private static string Ok(string msg)
        {
            return "<div class='alert alert-info'>" + msg + "</div>";
        }

        private static string Error(string label, Exception ex)
        {
            return "<div class='alert alert-danger'><strong>" + label
                + "</strong><br/>" + ex.Message + "</div>";
        }
    }
}
