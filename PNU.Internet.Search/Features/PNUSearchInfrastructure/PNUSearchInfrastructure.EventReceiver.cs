using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using PNU.Internet.Search.Receivers;
using PNU.Internet.Search.TimerJobs;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace PNU.Internet.Search.Features.PNUSearchInfrastructure
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("9f3afbe4-5687-4a7c-b57d-ca4ee16eedbc")]
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
                        foreach (SPWeb web in site.AllWebs)
                        {
                            try
                            {
                                AttachReceiverToWebLists(web);
                            }
                            finally { web.Dispose(); }
                        }
                    }
                    finally { site.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                
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
                
            }
        }

        // ----------------------------------------------------------------
        private static void AttachReceiverToWebLists(SPWeb web)
        {
            foreach (SPList list in web.Lists)
            {
                if (list.Hidden) continue;

                // skip pure system libraries
                if (list.Title.StartsWith("_") ||
                    list.Title.Contains("Gallery") ||
                    list.Title == "Workflow History" ||
                    list.Title == "Workflow Tasks" ||
                    list.Title == "User Information List" ||
                    list.Title == "TaxonomyHiddenList")
                    continue;

                SearchEventReceiverInstaller.Install(list);
            }
        }
    }

}
