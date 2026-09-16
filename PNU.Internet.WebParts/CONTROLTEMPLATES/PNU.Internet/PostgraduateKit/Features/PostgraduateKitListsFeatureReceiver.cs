using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Provisions the Postgraduate Academic Kit lists on feature activation.
    /// Resets the in-memory cache so deleted lists are re-created and re-seeded.
    /// </summary>
    [Guid("4F1C87A2-9D3E-4B05-8C61-2A7E5D9B3416")]
    public class PostgraduateKitListsFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                PgListProvisioner.ResetCache();

                SPWeb web = properties.Feature != null ? properties.Feature.Parent as SPWeb : null;
                if (web == null && properties.Feature != null)
                {
                    SPSite parentSite = properties.Feature.Parent as SPSite;
                    if (parentSite != null) web = parentSite.RootWeb;
                }

                if (web != null)
                {
                    using (SPSite site = new SPSite(web.Site.ID))
                    using (SPWeb target = PgTargetWeb.Open(site))
                    {
                        if (target != null)
                            PgListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                    }
                }
                else
                {
                    PgListProvisioner.EnsureAllListsExist();
                }
            }
            catch (Exception ex)
            {
                PgLog.Write("PostgraduateKitListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            PgListProvisioner.ResetCache();
        }
    }
}
