using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    /// <summary>
    /// Feature receiver to provision Central Library lists upon feature activation.
    /// Resets the memory cache so deleted lists are automatically re-created and re-seeded.
    /// </summary>
    [Guid("C9D7E2B5-3A48-4E60-AB1C-8E7F6D5C4B32")]
    public class CentralLibraryListsFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                ClListProvisioner.ResetCache();

                SPWeb web = properties.Feature != null ? properties.Feature.Parent as SPWeb : null;
                if (web == null && properties.Feature != null)
                {
                    SPSite site = properties.Feature.Parent as SPSite;
                    if (site != null) web = site.RootWeb;
                }

                if (web != null)
                {
                    using (SPSite site = new SPSite(web.Site.ID))
                    using (SPWeb target = ClTargetWeb.Open(site))
                    {
                        if (target != null)
                        {
                            ClListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                        }
                    }
                }
                else
                {
                    ClListProvisioner.EnsureAllListsExist();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("CentralLibraryListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            ClListProvisioner.ResetCache();
        }
    }
}
