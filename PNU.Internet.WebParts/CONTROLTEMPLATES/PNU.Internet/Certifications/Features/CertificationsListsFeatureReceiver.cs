using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls
{
    /// <summary>
    /// Optional web-scoped feature receiver: provisions the certifications list at
    /// activation time - list, columns AND the 44 default certificates - so the content
    /// is in place before anyone opens the page.
    ///
    /// Activation is not mandatory: ucCertifications performs the same check on every
    /// page load and will provision anything that is missing.
    /// </summary>
    [Guid("8F3C6A57-91D4-4E62-B8A3-1C5D0E7F2B94")]
    public class CertificationsListsFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                if (web == null)
                {
                    SPSite parentSite = properties.Feature.Parent as SPSite;
                    if (parentSite != null) web = parentSite.RootWeb;
                }
                if (web == null) return;

                CertListProvisioner.ResetCache();

                // The list lives on one web (/ar/Agencies/AcademicAffairs), not on the web the
                // feature happens to be activated on.
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb target = CertTargetWeb.Open(site))
                {
                    if (target == null)
                    {
                        CertLog.Write("CertificationsListsFeatureReceiver.FeatureActivated",
                            "Target web '" + CertTargetWeb.ResolveUrl(site) + "' was not found - nothing provisioned.");
                        return;
                    }

                    CertListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                }

                // The shared AdminUsers list lives on its own web (/ar/ContentAdmin).
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb adminWeb = CertTargetWeb.OpenAdminWeb(site))
                {
                    if (adminWeb != null)
                        CertListProvisioner.EnsureListOnWeb(site.ID, adminWeb.ID,
                            CertListSchema.Get(CertListNames.AdminUsers));
                }
            }
            catch (Exception ex)
            {
                CertLog.Write("CertificationsListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            // The list and its content are intentionally left in place on deactivation.
            CertListProvisioner.ResetCache();
        }
    }
}
