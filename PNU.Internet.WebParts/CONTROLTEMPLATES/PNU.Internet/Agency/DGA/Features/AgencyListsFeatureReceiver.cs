using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// Optional web-scoped feature receiver: provisions the Agency lists at
    /// activation time - list, columns AND default data - so the content is in place
    /// before anyone opens the page.
    ///
    /// Activation is not mandatory: the controls perform the same check on every page
    /// load and will provision anything that is missing.
    /// </summary>
    [Guid("7F2C9B41-58AD-4E63-9C10-2A6B8D5E4C37")]
    public class AgencyListsFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                if (web == null)
                {
                    SPSite site = properties.Feature.Parent as SPSite;
                    if (site != null) web = site.RootWeb;
                }
                if (web == null) return;

                AgListProvisioner.ResetCache();
                AgListProvisioner.EnsureAllListsOnWeb(web.Site.ID, web.ID);
            }
            catch (Exception ex)
            {
                AgLog.Write("AgencyListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            // Lists and their content are intentionally left in place on deactivation.
            AgListProvisioner.ResetCache();
        }
    }
}
