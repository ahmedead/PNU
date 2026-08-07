using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// Optional web-scoped feature receiver: provisions the Tawasul lists at activation
    /// time - list, columns AND default data - so the content is in place before anyone
    /// opens the page.
    ///
    /// Activation is not mandatory: the controls perform the same check on every page
    /// load and will provision anything that is missing.
    /// </summary>
    [Guid("3F6C2A9E-8B41-4D57-A2E0-9C7B5D3E1A48")]
    public class TawasulListsFeatureReceiver : SPFeatureReceiver
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

                TwListProvisioner.ResetCache();

                // The content lists live on one web (/ar/Tawasul), not on the web the
                // feature happens to be activated on.
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb target = TwTargetWeb.Open(site))
                {
                    if (target == null)
                    {
                        TwLog.Write("TawasulListsFeatureReceiver.FeatureActivated",
                            "Target web '" + TwTargetWeb.ResolveUrl(site) + "' was not found - nothing provisioned.");
                        return;
                    }

                    TwListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                }

                // The shared AdminUsers list lives on its own web (/ar/ContentAdmin).
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb adminWeb = TwTargetWeb.OpenAdminWeb(site))
                {
                    if (adminWeb != null)
                        TwListProvisioner.EnsureListOnWeb(site.ID, adminWeb.ID,
                            TwListSchema.Get(TwListNames.AdminUsers));
                }
            }
            catch (Exception ex)
            {
                TwLog.Write("TawasulListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            // Lists and their content are intentionally left in place on deactivation.
            TwListProvisioner.ResetCache();
        }
    }
}
