using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls
{
    /// <summary>
    /// Optional web-scoped feature receiver: provisions the Academic Calendar list at
    /// activation time - list, columns AND default data - so the content is in place
    /// before anyone opens the page.
    ///
    /// Activation is not mandatory: the controls perform the same check on every page
    /// load and will provision anything that is missing.
    /// </summary>
    [Guid("6F3B9E42-8A17-4D5C-B9E0-1C7A4F2D6B83")]
    public class AcademicCalendarListsFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                if (web == null)
                {
                    SPSite s = properties.Feature.Parent as SPSite;
                    if (s != null) web = s.RootWeb;
                }
                if (web == null) return;

                AcListProvisioner.ResetCache();

                // The list lives on one web (/ar/AcademicCalendar), not on the web the
                // feature happens to be activated on.
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb target = AcTargetWeb.Open(site))
                {
                    if (target == null)
                    {
                        AcLog.Write("AcademicCalendarListsFeatureReceiver.FeatureActivated",
                            "Target web '" + AcTargetWeb.ResolveUrl(site) + "' was not found - nothing provisioned.");
                        return;
                    }

                    AcListProvisioner.EnsureAllListsOnWeb(site.ID, target.ID);
                }

                // The shared AdminUsers list lives on its own web (/ar/ContentAdmin).
                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb adminWeb = AcTargetWeb.OpenAdminWeb(site))
                {
                    if (adminWeb != null)
                        AcListProvisioner.EnsureListOnWeb(site.ID, adminWeb.ID,
                            AcListSchema.Get(AcListNames.AdminUsers));
                }
            }
            catch (Exception ex)
            {
                AcLog.Write("AcademicCalendarListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            // Lists and their content are intentionally left in place on deactivation.
            AcListProvisioner.ResetCache();
        }
    }
}
