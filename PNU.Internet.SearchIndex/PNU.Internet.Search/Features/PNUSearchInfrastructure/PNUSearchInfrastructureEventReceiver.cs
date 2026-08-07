using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using PNU.Internet.SearchIndex.Logging;
using PNU.Internet.SearchIndex.Provisioning;
using PNU.Internet.SearchIndex.Receivers;
using PNU.Internet.SearchIndex.TimerJobs;

namespace PNU.Internet.SearchIndex.Features
{
    /// <summary>
    /// Web-Application-scoped feature receiver. On activation:
    ///   1. provisions /SearchIndexing/ subsite + 7 lists
    ///   2. installs the nightly timer job
    ///   3. attaches SearchListEventReceiver to every existing list
    ///      (skipping galleries, system lists, and the SearchIndexing
    ///      lists themselves).
    /// </summary>
    public class PNUSearchInfrastructureEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp == null) return;

                SearchCrawlerTimerJob.Install(webApp);

                foreach (SPSite site in webApp.Sites)
                {
                    try
                    {
                        ProvisionSearchIndexing(site);

                        foreach (SPWeb web in site.AllWebs)
                        {
                            try { AttachReceiverToWebLists(web); }
                            finally { web.Dispose(); }
                        }
                    }
                    finally { site.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning",
                    "FeatureActivated", ex.Message);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp == null) return;

                SearchCrawlerTimerJob.Uninstall(webApp);

                foreach (SPSite site in webApp.Sites)
                {
                    try
                    {
                        foreach (SPWeb web in site.AllWebs)
                        {
                            try
                            {
                                foreach (SPList list in web.Lists)
                                {
                                    if (list.Hidden) continue;
                                    SearchEventReceiverInstaller.Uninstall(list);
                                }
                            }
                            finally { web.Dispose(); }
                        }
                    }
                    finally { site.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning",
                    "FeatureDeactivating", ex.Message);
            }
        }

        // ----------------------------------------------------------------
        private static void ProvisionSearchIndexing(SPSite site)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite s = new SPSite(site.ID))
                    using (SPWeb root = s.RootWeb)
                    {
                        root.AllowUnsafeUpdates = true;
                        SearchIndexingProvisioner.EnsureAllForSite(s);
                    }
                });
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("Provisioning",
                    "ProvisionSearchIndexing " + site.Url, ex.Message);
            }
        }

        private static void AttachReceiverToWebLists(SPWeb web)
        {
            foreach (SPList list in web.Lists)
            {
                if (list.Hidden) continue;
                if (list.Title.StartsWith("_") ||
                    list.Title.Contains("Gallery") ||
                    list.Title == "Workflow History" ||
                    list.Title == "Workflow Tasks" ||
                    list.Title == "User Information List" ||
                    list.Title == "TaxonomyHiddenList")
                    continue;

                if (list.Title == SearchIndexingProvisioner.LIST_CONTENT  ||
                    list.Title == SearchIndexingProvisioner.LIST_MENU     ||
                    list.Title == SearchIndexingProvisioner.LIST_EXC_WEB  ||
                    list.Title == SearchIndexingProvisioner.LIST_EXC_PAGE ||
                    list.Title == SearchIndexingProvisioner.LIST_EXC_LIST ||
                    list.Title == SearchIndexingProvisioner.LIST_INDEXED_WEBS ||
                    list.Title == SearchIndexingProvisioner.LIST_MANUAL_PAGES)
                    continue;

                SearchEventReceiverInstaller.Install(list);
            }
        }
    }
}
