using ImageMagick;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class ucBreadCrumb : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateBreadcrumbs();

            }
        }

        private void GenerateBreadcrumbs()
        {
            List<string> crumbs = new List<string>();
            string currentWebTitle = string.Empty;

            SPWeb currentWeb = SPContext.Current.Web;
            Guid rootWebId = currentWeb.Site.RootWeb.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite elevatedSite = new SPSite(currentWeb.Url))
                using (SPWeb elevatedWeb = elevatedSite.OpenWeb(currentWeb.ID))
                {
                    SPWeb tempWeb = elevatedWeb;
                    bool isFirstIteration = true;

                    while (tempWeb != null)
                    {
                        // Skip the root web - don't add it to breadcrumbs
                        if (tempWeb.ID == rootWebId)
                        {
                            if (!isFirstIteration) tempWeb.Dispose();
                            break;
                        }

                        if (isFirstIteration) currentWebTitle = tempWeb.Title;

                        // All webs (including current) render as links
                        crumbs.Add($"<li class='breadcrumb-item small'><a href='{SPHttpUtility.HtmlEncode(tempWeb.Url)}'>{SPHttpUtility.HtmlEncode(tempWeb.Title)}</a></li>");

                        SPWeb parent = tempWeb.ParentWeb;

                        if (!isFirstIteration)
                        {
                            tempWeb.Dispose();
                        }

                        tempWeb = parent;
                        isFirstIteration = false;
                    }
                }
            });

            crumbs.Reverse();

            // The current page is the last crumb: disabled style, rendered as text (not a link)
            string pageTitle = GetCurrentPageTitle();
            if (!string.IsNullOrEmpty(pageTitle) &&
                !string.Equals(pageTitle, currentWebTitle, StringComparison.OrdinalIgnoreCase))
            {
                crumbs.Add($"<li class='breadcrumb-item small active disabled' aria-current='page'><span class='text-muted'>{SPHttpUtility.HtmlEncode(pageTitle)}</span></li>");
            }

            litBreadcrumbs.Text = string.Join("", crumbs);
        }

        private string GetCurrentPageTitle()
        {
            try
            {
                SPListItem item = SPContext.Current.ListItem;

                if (item != null)
                {
                    object title = item["Title"];

                    if (title != null && !string.IsNullOrWhiteSpace(title.ToString()))
                    {
                        return title.ToString().Trim();
                    }

                    // No Title set - fall back to the file name without its extension
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        return System.IO.Path.GetFileNameWithoutExtension(item.Name);
                    }
                }
            }
            catch
            {
                // No list item in context (application page, etc.) - fall back below
            }

            return (Page != null && !string.IsNullOrWhiteSpace(Page.Title)) ? Page.Title.Trim() : string.Empty;
        }

    }
}
