using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Utilities.Win32;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucGetUnMatchedPages : UserControl
    {
        // To store missing pages between /ar/ and /en/
        private List<PageInfo> missingPages = new List<PageInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize the tree view with top-level subsites under /ar/
                LoadSubsites(SPContext.Current.Site.Url);// + "/ar/");
            }
        }

        // This method loads subsites and pages
        private void LoadSubsites(string siteUrl, TreeNode parentNode = null)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPWebCollection subsites = web.Webs;

                        foreach (SPWeb subsite in subsites)
                        {
                            // Create a node for each subsite
                            TreeNode node = new TreeNode
                            {
                                Text = subsite.Title,
                                Value = subsite.Url
                            };

                            if (parentNode == null)
                            {
                                // If there is no parent node, add to the root of the tree
                                tvSubsites.Nodes.Add(node);
                            }
                            else
                            {
                                // Otherwise, add as a child node
                                parentNode.ChildNodes.Add(node);
                            }

                            // Recursively load child subsites
                            LoadSubsites(subsite.Url, node);

                            // Compare pages for this subsite between /ar/ and /en/
                            //ComparePages(subsite.Url);
                        }
                    }
                }
            });
        }

        // This method compares pages between /ar/ and /en/ sites
        private void ComparePages(string subsiteUrl)
        {
            // Example URLs of /ar/ and /en/ subsites
            string arSiteUrl = subsiteUrl + "/";
            string enSiteUrl = arSiteUrl.Replace("/ar/", "/en/");

            List<string> arPages = GetPagesFromSite(arSiteUrl);
            List<string> enPages = GetPagesFromSite(enSiteUrl);

            // Compare pages and get missing ones
            foreach (var page in arPages)
            {
                if (!enPages.Contains(page))
                {
                    missingPages.Add(new PageInfo
                    {
                        PageTitle = page,
                        PageURL = arSiteUrl + "Pages/" + page
                    });
                }
            }

            // Bind missing pages to the Repeater control
            PagesRepeater.DataSource = missingPages;
            PagesRepeater.DataBind();
        }

        // This method fetches page names from the /Pages/ library in SharePoint
        private List<string> GetPagesFromSite(string siteUrl)
        {
            List<string> pages = new List<string>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        string PagesListName = web.Language == 1025 ? "الصفحات" : "Pages";
                        
                            SPList list = web.Lists[PagesListName]; // Use the Pages library
                                                                    //SPQuery query = new SPQuery
                                                                    //{
                                                                    //    Query = "<Where><Eq><FieldRef Name='ContentType' /><Value Type='Text'>Page</Value></Eq></Where>"
                                                                    //};

                        SPListItemCollection items = list.GetItems();//query);
                            foreach (SPListItem item in items)
                            {
                                string pageName = item["FileLeafRef"].ToString(); // The file name of the page (e.g., page.aspx)
                                pages.Add(pageName);
                            }
                        
                        
                    }
                }
            });
            }
            catch
            {

                return null;
            }

            return pages;
        }

        // Handle the selected node changed event in TreeView
        protected void tvSubsites_SelectedNodeChanged(object sender, EventArgs e)
        {
            // Handle the logic when a node is selected (optional)
            string selectedNodeValue = tvSubsites.SelectedNode.Value;
            // You can perform further actions based on selected node
            ComparePages(selectedNodeValue);
        }
    }

    // Class to store page info for binding to the Repeater

}
public class PageInfo
{
    public string PageTitle { get; set; }
    public string PageURL { get; set; }
}



