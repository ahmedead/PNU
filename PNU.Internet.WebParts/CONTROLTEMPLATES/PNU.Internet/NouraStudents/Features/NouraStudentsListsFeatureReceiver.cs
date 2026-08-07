using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>
    /// Optional web-scoped feature receiver: provisions the Noura Students lists at
    /// activation time - list, columns AND default data - so the content is in place
    /// before anyone opens the page.
    ///
    /// Activation is not mandatory: the controls perform the same check on every page
    /// load and will provision anything that is missing.
    /// </summary>
    [Guid("B4E8D1A6-2C37-4F59-9A0B-7D6E5C4B3A21")]
    public class NouraStudentsListsFeatureReceiver : SPFeatureReceiver
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

                NsListProvisioner.ResetCache();

                // The lists live on one web (/ar/NouraStudents), not on the web the
                // feature happens to be activated on.
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb target = NsTargetWeb.Open(site))
                {
                    if (target == null)
                    {
                        NsLog.Write("NouraStudentsListsFeatureReceiver.FeatureActivated",
                            "Target web '" + NsTargetWeb.ResolveUrl(site) + "' was not found - nothing provisioned.");
                        return;
                    }

                    NsListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                }

                // The shared AdminUsers list lives on its own web (/ar/ContentAdmin).
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb adminWeb = NsTargetWeb.OpenAdminWeb(site))
                {
                    if (adminWeb != null)
                        NsListProvisioner.EnsureListOnWeb(site.ID, adminWeb.ID,
                            NsListSchema.Get(NsListNames.AdminUsers));
                }
            }
            catch (Exception ex)
            {
                NsLog.Write("NouraStudentsListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            // Lists and their content are intentionally left in place on deactivation.
            NsListProvisioner.ResetCache();
        }
    }
}
