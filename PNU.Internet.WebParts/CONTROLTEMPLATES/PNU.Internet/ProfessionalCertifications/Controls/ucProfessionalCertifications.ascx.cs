using System;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public partial class ucProfessionalCertifications : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PcListProvisioner.EnsureAllListsExist();
            }
            catch (Exception ex)
            {
                PcLog.Write("ucProfessionalCertifications.Page_Load", ex);
            }
        }
    }
}
