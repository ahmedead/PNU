using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    /// <summary>
    /// Base page layout class for DGA Postgraduate Academic Kit publishing pages.
    /// Enforces the dga-PostgraduateKit.master master page in OnPreInit.
    /// </summary>
    public class DgaPostgraduateKitPageBase : PublishingLayoutPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    string rootUrl = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                    this.MasterPageFile = $"{rootUrl}/_catalogs/masterpage/dga-PostgraduateKit.master";
                }
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("DgaPostgraduateKitPageBase.OnPreInit", ex);
            }
        }
    }
}

namespace PNU.Internet.WebParts
{
    /// <summary>
    /// Direct root namespace alias for DGA Postgraduate Academic Kit publishing page layouts.
    /// </summary>
    public class DgaPostgraduateKitPageBase : CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure.DgaPostgraduateKitPageBase
    {
    }
}
