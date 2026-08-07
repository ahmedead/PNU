using System;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// Container for the "نظرة عامة عن وكالة الجامعة" tab pane.
    /// Owns the grid structure only; every section control reads and provisions
    /// its own list, so this control has no data access of its own.
    /// </summary>
    public partial class ucAgencyDetails : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Safety net: makes sure every Agency list exists on the current web,
                // even if a section control is later removed from this container.
                AgListProvisioner.EnsureAllListsExist();
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgencyDetails.Page_Load", ex);
            }
        }
    }
}
