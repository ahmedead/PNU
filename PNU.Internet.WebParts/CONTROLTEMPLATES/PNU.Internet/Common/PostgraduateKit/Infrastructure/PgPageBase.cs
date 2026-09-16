using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;

namespace PNU.Internet.WebParts
{
    /// <summary>
    /// Base class for the postgraduate kit page layouts. It forces the kit's own master page,
    /// which carries the v3.0 stylesheet and the kit header and footer, the same way
    /// DGAPageBase forces DGA_Internal.master.
    /// </summary>
    public class PostgraduateKitPageBase : PublishingLayoutPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            try
            {
                string rootUrl = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                this.MasterPageFile = rootUrl + "/_catalogs/masterpage/PostgraduateKit.master";
            }
            catch
            {
                // Fall back to whatever master the site resolves by default.
            }
        }
    }
}
