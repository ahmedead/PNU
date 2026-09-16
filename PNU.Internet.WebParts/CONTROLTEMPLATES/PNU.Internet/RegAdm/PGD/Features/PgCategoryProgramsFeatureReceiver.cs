using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Features
{
    /// <summary>
    /// Feature receiver that automatically provisions and seeds the Postgraduate Category Programs lists
    /// upon Feature Activation in SharePoint (no PowerShell script needed).
    /// </summary>
    [Guid("A71D5E92-3B4F-4C81-89D1-F5E24A790B12")]
    public class PgCategoryProgramsFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature != null ? properties.Feature.Parent as SPWeb : null;
                if (web == null && properties.Feature != null)
                {
                    SPSite site = properties.Feature.Parent as SPSite;
                    if (site != null) web = site.RootWeb;
                }

                if (web != null)
                {
                    PgCategoryProvisioner.EnsureLists(web);
                }
            }
            catch (Exception ex)
            {
                // Error handled gracefully
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            // Do not delete lists on deactivation to preserve user-managed data
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }
    }
}
