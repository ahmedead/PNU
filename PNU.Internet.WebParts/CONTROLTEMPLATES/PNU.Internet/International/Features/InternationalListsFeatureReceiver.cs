using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Optional web-scoped feature receiver: provisions the International Students
    /// lists at activation time - list, columns AND default data - so the content is
    /// in place before anyone opens the page.
    ///
    /// Activation is not mandatory: the controls perform the same check on every
    /// page load and will provision anything that is missing.
    /// </summary>
    [Guid("6F1B2C3D-4E5A-4B6C-8D9E-0A1B2C3D4E5F")]
    public class InternationalListsFeatureReceiver : SPFeatureReceiver
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

                // Same code path as the page-load provisioning, so lists, columns and
                // seed rows are always created identically.
                InternationalListProvisioner.ResetCache();

                // The lists live on one web (/ar/International), not on the web the
                // feature happens to be activated on.
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb target = IntlTargetWeb.Open(site))
                {
                    if (target == null)
                    {
                        IntlLog.Write("InternationalListsFeatureReceiver.FeatureActivated",
                            "Target web '" + IntlTargetWeb.ResolveUrl(site) + "' was not found - nothing provisioned.");
                        return;
                    }

                    InternationalListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                }

                // The shared AdminUsers list lives on its own web (/ar/ContentAdmin).
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb adminWeb = IntlTargetWeb.OpenAdminWeb(site))
                {
                    if (adminWeb != null)
                        InternationalListProvisioner.EnsureListOnWeb(site.ID, adminWeb.ID,
                            IntlListSchema.Get(IntlListNames.AdminUsers));
                }
            }
            catch (Exception ex)
            {
                IntlLog.Write("InternationalListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            // Lists and their content are intentionally left in place on deactivation.
            InternationalListProvisioner.ResetCache();
        }
    }
}
