using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucAllSubsites : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize the tree view with top-level subsites
                LoadSubsites(SPContext.Current.Site.Url, null);
            }
        }

        private void LoadSubsites(string siteUrl, TreeNode parentNode)
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
                        }
                    }
                }
            });
        }

        protected void tvSubsites_SelectedNodeChanged(object sender, EventArgs e)
        {
            // Get the selected node URL and set the Title and URL fields
            string selectedSubsiteUrl = tvSubsites.SelectedNode.Value;
            txtDeptTitle.Text = tvSubsites.SelectedNode.Text;

            // Load any additional data based on the selected subsite
            // (This depends on your specific requirements)
        }
    }
}
