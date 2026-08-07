using System;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Provisioning;

namespace PNU.Internet.SearchIndex.Logging
{
    /// <summary>
    /// Records pages found in one language but missing in the other.
    /// Used by the bilingual page indexer.
    /// </summary>
    public static class MissedPagesLogger
    {
        public static void Log(SPSite site, string foundUrl, string foundLanguage,
            string missedUrl, string missedLanguage)
        {
            if (site == null) return;
            if (string.IsNullOrEmpty(missedUrl)) return;

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
                                    SearchIndexingProvisioner.LIST_MISSED_PAGES);
                                if (list == null) return;

                                string caml = string.Format(
                                    @"<Where><Eq><FieldRef Name='MissedUrl'/>
                                      <Value Type='Text'>{0}</Value></Eq></Where>",
                                    Escape(missedUrl));
                                if (list.GetItems(new SPQuery {
                                        Query = caml, RowLimit = 1
                                    }).Count > 0) return;

                                SPListItem it = list.Items.Add();
                                it["Title"]          = foundUrl ?? "";
                                it["MissedUrl"]      = missedUrl;
                                it["FoundLanguage"]  = foundLanguage  ?? "";
                                it["MissedLanguage"] = missedLanguage ?? "";
                                it["DetectedAt"]     = DateTime.Now;
                                it.Update();
                            }
                            finally { sub.AllowUnsafeUpdates = false; }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("MissedPages",
                    missedUrl, ex.Message);
            }
        }

        private static string Escape(string s)
        {
            return (s ?? "").Replace("&", "&amp;").Replace("<", "&lt;")
                            .Replace(">", "&gt;").Replace("'", "&apos;");
        }
    }
}
