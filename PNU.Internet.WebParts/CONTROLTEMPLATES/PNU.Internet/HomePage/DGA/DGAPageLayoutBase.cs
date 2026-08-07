using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

namespace PNU.Internet.WebParts
{
    public class DGAPageBase : PublishingLayoutPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            // Run the base logic first
            base.OnPreInit(e);

            try
            {
                // Force the master page path
                string rootUrl = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                this.MasterPageFile = $"{rootUrl}/_catalogs/masterpage/DGA_Internal.master";
            }
            catch
            {
                // Fallback to default if something goes wrong
            }
        }
    }

    public class DGANewBodyPageBase : PublishingLayoutPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            // Run the base logic first
            base.OnPreInit(e);

            try
            {
                // Force the master page path
                string rootUrl = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                this.MasterPageFile = $"{rootUrl}/_catalogs/masterpage/DGA_ASide.master";
            }
            catch
            {
                // Fallback to default if something goes wrong
            }
        }
    }

    public class DGANewBodyWithoutBreadCrumbPageBase : PublishingLayoutPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            // Run the base logic first
            base.OnPreInit(e);

            try
            {
                // Force the master page path
                string rootUrl = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                this.MasterPageFile = $"{rootUrl}/_catalogs/masterpage/DGA_Internal_without_breadcrump.master";
            }
            catch
            {
                // Fallback to default if something goes wrong
            }
        }
    }

    public class DGAWithSideMenuPageBase : PublishingLayoutPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            // Run the base logic first
            base.OnPreInit(e);

            try
            {
                // Force the master page path
                string rootUrl = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                this.MasterPageFile = $"{rootUrl}/_catalogs/masterpage/DGA_Internal_WithSideMenu.master";
            }
            catch
            {
                // Fallback to default if something goes wrong
            }
        }
    }



}