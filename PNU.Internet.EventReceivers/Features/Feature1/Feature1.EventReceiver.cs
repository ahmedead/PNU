using Microsoft.SharePoint;
using PNU.Internet.EventReceivers.PagesLibraryEventReceiver;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace PNU.Internet.EventReceivers.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("545c01db-6cdc-4657-9fba-d638ac0a6f97")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {


        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            string rootSiteUrl = "https://newportal.pnu.edu.sa/ar"; // Replace with actual URL

            using (SPSite siteCollection = new SPSite(rootSiteUrl))
            {
                using (SPWeb rootWeb = siteCollection.OpenWeb())
                {
                    RegisterEventReceiverOnSubsites(rootWeb);
                }
            }
        }

        private void RegisterEventReceiverOnSubsites(SPWeb web)
        {
            SPList pagesLibrary = web.Lists.TryGetList("Pages");

            if (pagesLibrary != null)
            {
                bool isReceiverRegistered = pagesLibrary.EventReceivers
                    .Cast<SPEventReceiverDefinition>()
                    .Any(r =>
                    {
                        return r.Class == typeof(PNU.Internet.EventReceivers.PagesLibraryEventReceiver.PagesLibraryEventReceiver).FullName && r.Type == SPEventReceiverType.ItemAdded;
                    });

                if (!isReceiverRegistered)
                {
                    pagesLibrary.EventReceivers.Add(SPEventReceiverType.ItemAdded,
                        Assembly.GetExecutingAssembly().FullName,
                        typeof(PNU.Internet.EventReceivers.PagesLibraryEventReceiver.PagesLibraryEventReceiver).FullName);
                    pagesLibrary.Update();
                }
            }

            foreach (SPWeb subWeb in web.Webs)
            {
                using (subWeb)
                {
                    RegisterEventReceiverOnSubsites(subWeb);
                }
            }
        }




    }
}
