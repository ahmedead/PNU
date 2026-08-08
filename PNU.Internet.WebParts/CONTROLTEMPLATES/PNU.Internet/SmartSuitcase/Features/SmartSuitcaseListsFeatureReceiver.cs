using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Optional web-scoped feature receiver: provisions the Smart Suitcase lists
    /// at activation time - list, columns AND default data.
    /// </summary>
    [Guid("8A2B3C4D-5E6F-7A8B-9C0D-1E2F3A4B5C6D")]
    public class SmartSuitcaseListsFeatureReceiver : SPFeatureReceiver
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

                SmartSuitcaseListProvisioner.ResetCache();

                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb target = SscTargetWeb.Open(site))
                {
                    if (target == null)
                    {
                        SscLog.Write("SmartSuitcaseListsFeatureReceiver.FeatureActivated",
                            "Target web '" + SscTargetWeb.ResolveUrl(site) + "' was not found - nothing provisioned.");
                        return;
                    }

                    SmartSuitcaseListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                }

                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb adminWeb = SscTargetWeb.OpenAdminWeb(site))
                {
                    if (adminWeb != null)
                        SmartSuitcaseListProvisioner.EnsureListOnWeb(site.ID, adminWeb.ID,
                            SscListSchema.Get(SscListNames.AdminUsers));
                }
            }
            catch (Exception ex)
            {
                SscLog.Write("SmartSuitcaseListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SmartSuitcaseListProvisioner.ResetCache();
        }
    }
}
