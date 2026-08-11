using System;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    /// <summary>
    /// Container that renders the three shared About sections in order:
    /// Overview, Deputy Welcome, Tasks. Provisions all required child control lists on initialization.
    /// </summary>
    public partial class ucSharedAboutDga : UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current != null &&
                    HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity.IsAuthenticated &&
                    SPContext.Current != null &&
                    SPContext.Current.Web != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    SharedTitles.EnsureList(SPContext.Current.Web);
                    AgencyOverviewProvisioner.EnsureList(SPContext.Current.Web);
                    AgencyTasksProvisioner.EnsureLists(SPContext.Current.Web);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucSharedAboutDga.Page_Init", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

