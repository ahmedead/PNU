using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;

namespace PNU.Internet.WebParts
{
    /// <summary>
    /// Base page layout for the Postgraduate Academic Kit pages.
    /// Forces the PostgraduateKit.master master page in OnPreInit, exactly like DGAPageBase.
    /// </summary>
    public class PostgraduateKitPageBase : PublishingLayoutPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            try
            {
                string rootUrl = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                this.MasterPageFile = $"{rootUrl}/_catalogs/masterpage/PostgraduateKit.master";
            }
            catch
            {
                // Fallback to default if SPContext is not available
            }
        }
    }
}
