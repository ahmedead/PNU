using System;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    /// <summary>
    /// Container that renders the three shared About sections in order:
    /// Overview, Deputy Welcome, Tasks. Each child control owns its own
    /// provisioning and data binding, so this control has no logic of its own.
    /// </summary>
    public partial class ucSharedAboutDga : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No-op: child controls self-provision (Page_Init) and self-bind (Page_Load).
        }
    }
}
