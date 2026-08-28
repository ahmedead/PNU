using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public class ProfessionalCertificationsListsFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                if (web != null)
                {
                    PcListProvisioner.ResetCache();
                    PcListProvisioner.EnsureAllListsOnWeb(web.Site.ID, web.ID);
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("ProfessionalCertificationsListsFeatureReceiver.FeatureActivated", ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties) { }
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties) { }
        public override void FeatureInstalling(SPFeatureReceiverProperties properties) { }
    }
}
