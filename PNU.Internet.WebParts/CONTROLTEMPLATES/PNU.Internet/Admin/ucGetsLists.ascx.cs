using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucGetsLists : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize the first dropdown with top-level subsites
                LoadSubsites(SPContext.Current.Site.Url, ddlSubsiteLevel1);
            }
        }

        protected void ddlSubsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList currentDropdown = (DropDownList)sender;

            // Determine which dropdown level was selected
            int currentLevel = int.Parse(currentDropdown.ID.Replace("ddlSubsiteLevel", ""));

            // Hide all subsequent dropdowns
            for (int i = currentLevel + 1; i <= 10; i++)
            {
                DropDownList ddlNextLevel = FindControl("ddlSubsiteLevel" + i) as DropDownList;
                ddlNextLevel.Visible = false;
                ddlNextLevel.Items.Clear();
            }

            // If a valid subsite is selected, load its child subsites into the next dropdown
            if (!string.IsNullOrEmpty(currentDropdown.SelectedValue))
            {
                string selectedSubsiteUrl = currentDropdown.SelectedValue;
                DropDownList nextDropdown = FindControl("ddlSubsiteLevel" + (currentLevel + 1)) as DropDownList;

                if (nextDropdown != null)
                {
                    LoadSubsites(selectedSubsiteUrl, nextDropdown);
                }
                
                LoadLists(selectedSubsiteUrl);
            }
        }

        protected void ddlLists_SelectedIndexChanged(object sender, EventArgs e)
        {
           

            // Call a method to get list data as JSON
             BindGrid();

            
        }

        private void BindGrid( )
        {
            string listId = ddlLists.SelectedValue;
            //List<Dictionary<string, string>> listItems = new List<Dictionary<string, string>>();

            try
            {
                // Assuming the subsite URL is stored or passed elsewhere in the code
                using (SPSite site = new SPSite(GetLastSelectedSubsiteUrl()))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists[new Guid(listId)];
                        SPListItemCollection listItems = list.Items;

                        DataTable dt = new DataTable();
                        dt = listItems.GetDataTable();

                        GridView1.DataSource = dt;
                        GridView1.DataBind();

                        //Required for jQuery DataTables to work.
                        GridView1.UseAccessibleHeader = true;
                        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log errors)
            }

            // Convert listItems to JSON
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }
        private void LoadSubsites(string siteUrl, DropDownList ddlSubsites)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPWebCollection subsites = web.Webs;

                        if (subsites.Count > 0)
                        {
                            ddlSubsites.DataSource = subsites;
                            ddlSubsites.DataTextField = "Title";
                            ddlSubsites.DataValueField = "Url";
                            ddlSubsites.DataBind();
                            ddlSubsites.Items.Insert(0, new ListItem("--Select a Subsite--", ""));

                            // Make the dropdown visible if it contains items
                            ddlSubsites.Visible = true;
                        }
                    }
                }
            });
        }

        


        
        private string GetLastSelectedSubsiteUrl()
        {
            for (int i = 10; i >= 1; i--)
            {
                DropDownList ddl = FindControl("ddlSubsiteLevel" + i) as DropDownList;
                if (ddl != null && !string.IsNullOrEmpty(ddl.SelectedValue))
                {
                    return ddl.SelectedValue;
                }
            }
            return null;
        }

        protected void LoadLists(string subsiteUrl)
        {
            // Clear existing items
            ddlLists.Items.Clear();

            try
            {
                // Connect to the subsite
                using (SPSite site = new SPSite(subsiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        // Get all lists in the subsite
                        SPListCollection lists = web.Lists;

                        foreach (SPList list in lists)
                        {
                            // Add list name to dropdown
                            ddlLists.Items.Add(new ListItem(list.Title, list.ID.ToString()));
                        }
                        ddlLists.Items.Insert(0, new ListItem("--Select a List--", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                // You may also want to display a user-friendly message
            }
        }
    }
}
