using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Provisioning;

namespace PNU.Internet.SearchIndex.Logging
{
    /// <summary>
    /// Records indexing errors per page/item and supports retry.
    ///
    /// Workflow:
    ///   - Crawler fails on a page → call Log(...) once. If it's the
    ///     first failure a new IndexErrors row is created. Subsequent
    ///     failures bump Attempts and update LastSeenAt.
    ///   - The user fixes the source (permissions, broken page, etc.)
    ///     and ticks Retry = Yes on the IndexErrors row.
    ///   - The next time the crawler runs the relevant task it calls
    ///     GetUrlsMarkedForRetry() and re-attempts those URLs.
    ///   - On success the crawler calls MarkResolved(url) which sets
    ///     Resolved = Yes (we keep the row for history).
    /// </summary>
    public static class IndexErrorLogger
    {
        // ----- write -----------------------------------------------------
        public static void Log(SPSite site, string pageUrl, string context,
            string errorMessage)
        {
            if (site == null || string.IsNullOrEmpty(pageUrl)) return;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    {
                        string subUrl = s.RootWeb.ServerRelativeUrl.TrimEnd('/')
                            + "/" + SearchIndexingProvisioner.SUBSITE_URL;
                        using (SPWeb sub = s.OpenWeb(subUrl))
                        {
                            if (sub == null || !sub.Exists) return;
                            sub.AllowUnsafeUpdates = true;
                            try
                            {
                                SPList list = sub.Lists.TryGetList(
                                    SearchIndexingProvisioner.LIST_INDEX_ERRORS);
                                if (list == null) return;

                                string caml = string.Format(
                                    @"<Where><Eq><FieldRef Name='Title'/>
                                      <Value Type='Text'>{0}</Value></Eq></Where>",
                                    Escape(pageUrl));
                                SPListItemCollection existing = list.GetItems(
                                    new SPQuery { Query = caml, RowLimit = 1 });

                                if (existing.Count > 0)
                                {
                                    SPListItem it = existing[0];
                                    int attempts = 0;
                                    if (it["Attempts"] != null)
                                        int.TryParse(it["Attempts"].ToString(),
                                            out attempts);
                                    it["Attempts"]     = attempts + 1;
                                    it["LastSeenAt"]   = DateTime.Now;
                                    it["ErrorMessage"] = Truncate(errorMessage, 4000);
                                    it["ErrorContext"] = Truncate(context, 250);
                                    it["Retry"]        = false;
                                    it["Resolved"]     = false;
                                    it.Update();
                                }
                                else
                                {
                                    SPListItem it = list.Items.Add();
                                    it["Title"]        = pageUrl;
                                    it["ErrorMessage"] = Truncate(errorMessage, 4000);
                                    it["ErrorContext"] = Truncate(context, 250);
                                    it["FirstSeenAt"]  = DateTime.Now;
                                    it["LastSeenAt"]   = DateTime.Now;
                                    it["Attempts"]     = 1;
                                    it["Retry"]        = false;
                                    it["Resolved"]     = false;
                                    it.Update();
                                }
                            }
                            finally { sub.AllowUnsafeUpdates = false; }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("IndexErrors",
                    pageUrl, ex.Message);
            }
        }

        // ----- read ------------------------------------------------------
        /// <summary>
        /// Returns URLs of IndexErrors rows where Retry=Yes and
        /// Resolved=No. The crawler should call MarkResolved on each
        /// after a successful retry.
        /// </summary>
        public static List<string> GetUrlsMarkedForRetry(SPSite site)
        {
            var urls = new List<string>();
            if (site == null) return urls;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    {
                        string subUrl = s.RootWeb.ServerRelativeUrl.TrimEnd('/')
                            + "/" + SearchIndexingProvisioner.SUBSITE_URL;
                        using (SPWeb sub = s.OpenWeb(subUrl))
                        {
                            if (sub == null || !sub.Exists) return;
                            SPList list = sub.Lists.TryGetList(
                                SearchIndexingProvisioner.LIST_INDEX_ERRORS);
                            if (list == null) return;

                            var q = new SPQuery
                            {
                                Query = @"<Where><And>
                                    <Eq><FieldRef Name='Retry'/>
                                        <Value Type='Boolean'>1</Value></Eq>
                                    <Neq><FieldRef Name='Resolved'/>
                                         <Value Type='Boolean'>1</Value></Neq>
                                  </And></Where>"
                            };

                            foreach (SPListItem it in list.GetItems(q))
                            {
                                string u = Convert.ToString(it["Title"]);
                                if (!string.IsNullOrEmpty(u)) urls.Add(u);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("IndexErrors",
                    "GetUrlsMarkedForRetry", ex.Message);
            }
            return urls;
        }

        public static void MarkResolved(SPSite site, string pageUrl)
        {
            if (site == null || string.IsNullOrEmpty(pageUrl)) return;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    {
                        string subUrl = s.RootWeb.ServerRelativeUrl.TrimEnd('/')
                            + "/" + SearchIndexingProvisioner.SUBSITE_URL;
                        using (SPWeb sub = s.OpenWeb(subUrl))
                        {
                            if (sub == null || !sub.Exists) return;
                            sub.AllowUnsafeUpdates = true;
                            try
                            {
                                SPList list = sub.Lists.TryGetList(
                                    SearchIndexingProvisioner.LIST_INDEX_ERRORS);
                                if (list == null) return;

                                string caml = string.Format(
                                    @"<Where><Eq><FieldRef Name='Title'/>
                                      <Value Type='Text'>{0}</Value></Eq></Where>",
                                    Escape(pageUrl));
                                SPListItemCollection existing = list.GetItems(
                                    new SPQuery { Query = caml, RowLimit = 1 });
                                if (existing.Count == 0) return;

                                SPListItem it = existing[0];
                                it["Resolved"]   = true;
                                it["Retry"]      = false;
                                it["LastSeenAt"] = DateTime.Now;
                                it.Update();
                            }
                            finally { sub.AllowUnsafeUpdates = false; }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("IndexErrors",
                    "MarkResolved " + pageUrl, ex.Message);
            }
        }

        // ----- helpers ---------------------------------------------------
        private static string Escape(string s)
        {
            return (s ?? "").Replace("&","&amp;").Replace("<","&lt;")
                            .Replace(">","&gt;").Replace("'","&apos;");
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max);
        }
    }
}
