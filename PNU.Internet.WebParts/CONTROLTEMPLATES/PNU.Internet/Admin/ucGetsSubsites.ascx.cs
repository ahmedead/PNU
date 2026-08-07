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
    public partial class ucGetsSubsites : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize the first dropdown with top-level subsites
                LoadSubsites(SPContext.Current.Site.Url, ddlSubsiteLevel1);
                LoadSubSiteTemplates();
            }
        }

        private void LoadSubSiteTemplates()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb oWeb = site.OpenWeb())
                    {
                        try
                        {
                            SPList reqList = oWeb.GetList(Settings.SubSiteTemplates );

                            
                            SPListItemCollection items = reqList.Items;

                            if (items != null)
                            {
                                ddlSubSiteTemplates.DataSource = items.GetDataTable();
                                ddlSubSiteTemplates.DataValueField = "ID";
                                ddlSubSiteTemplates.DataTextField = "Title";
                                ddlSubSiteTemplates.DataBind();
                            }






                        }
                        catch (Exception ex)
                        {
                            
                        }
                        finally
                        {
                            
                        }
                        
                    }
                }
            });

        }

        protected void btnCreateSiteFromTemplate_Click(object sender, EventArgs e)
        {
            string lastSelectedSubsiteUrl = GetLastSelectedSubsiteUrl();

            if (!string.IsNullOrEmpty(lastSelectedSubsiteUrl))
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb oWeb = site.OpenWeb())
                    {



                        SPUtility.ValidateFormDigest();
                        try
                        {
                            SPList reqList = oWeb.GetList(Settings.SubSiteTemplates);


                            SPListItem item = reqList.Items.GetItemById(Convert.ToInt32(ddlSubSiteTemplates.SelectedValue));

                            if (item != null)
                            {
                                CreateSubsite(lastSelectedSubsiteUrl, txtSiteName.Text, txtSiteTitle.Text, item["TemplateName"].ToString(),Convert.ToUInt32(item["LCID"].ToString()));

                            }


                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {

                        }

                    }
                }


                
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
                txtDeptTitle.Text = currentDropdown.SelectedItem.Text;
            }
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
                            ddlSubsites.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select a Subsite--", ""));

                            // Make the dropdown visible if it contains items
                            ddlSubsites.Visible = true;
                        }
                    }
                }
            });
        }

        protected void btnCreateSubsite_Click(object sender, EventArgs e)
        {
            string lastSelectedSubsiteUrl = GetLastSelectedSubsiteUrl();

            if (!string.IsNullOrEmpty(lastSelectedSubsiteUrl))
            {
                CreateSubsite(lastSelectedSubsiteUrl,"News", "الأخبار", "DepartmentNews",1025);
            }
        }


        protected void btnCreateAnnouncement_Click(object sender, EventArgs e)
        {
            string lastSelectedSubsiteUrl = GetLastSelectedSubsiteUrl();

            if (!string.IsNullOrEmpty(lastSelectedSubsiteUrl))
            {
                CreateSubsite(lastSelectedSubsiteUrl, "Announcements", "الإعلانات", "DepartmentAnnouncements",1025);
            }
        }

        protected void btnCreateSubsite_EN_Click(object sender, EventArgs e)
        {
            string lastSelectedSubsiteUrl = GetLastSelectedSubsiteUrl();

            if (!string.IsNullOrEmpty(lastSelectedSubsiteUrl))
            {
                CreateSubsite(lastSelectedSubsiteUrl, "News", "News", "DepartmentNews_EN",1033);
            }
        }

        protected void btnCreateAnnouncement_EN_Click(object sender, EventArgs e)
        {
            string lastSelectedSubsiteUrl = GetLastSelectedSubsiteUrl();

            if (!string.IsNullOrEmpty(lastSelectedSubsiteUrl))
            {
                CreateSubsite(lastSelectedSubsiteUrl, "Announcements", "Announcements", "DepartmentAnnouncements_EN", 1033);
            }
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

        private void CreateSubsite(string parentSiteUrl,string subsiteURL, string subsiteName, string templateName, uint LCID)
        {
            string RedirectURL = "";
            //uint LCID = 1033;
            //if (parentSiteUrl.Contains(@"\ar\"))
            //    LCID = 1025;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb parentWeb = site.OpenWeb(parentSiteUrl.Replace(site.Url,"") + "/"))
                    {
                        
                        bool originalAllowUnsafeUpdates = parentWeb.AllowUnsafeUpdates;

                        
                        //string templateId = "eb07f9cc-0685-4147-85ee-48668a78521b";// "DepartmentNews";
                        SPWebTemplateCollection coll = site.GetWebTemplates(LCID);

                        SPUtility.ValidateFormDigest();
                        try
                        {
                            



                            SPWeb newSubsite = null;

                            foreach (SPWebTemplate template in coll)
                            {
                                if (template.Title.Equals(templateName, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    site.AllowUnsafeUpdates = true;
                                    parentWeb.AllowUnsafeUpdates = true;
                                    // Create the subsite
                                    newSubsite = parentWeb.Webs.Add(subsiteURL, subsiteName, "", LCID, template, false, false);

                                    // Set any additional properties or configurations for the new subsite here
                                    newSubsite.Update();
                                    break;
                                }
                            }

                            // If the subsite was created successfully, change the master page
                            if (newSubsite != null)
                            {
                                // Create a custom list in the subsite
                                CreateCustomList(newSubsite, txtDeptTitle.Text, txtDeptCode.Text);


                                string masterPageUrl = "/_catalogs/masterpage/Portal_Internal.master";

                                // Set the master page and the custom master page
                                //newSubsite.MasterUrl = masterPageUrl;
                                newSubsite.CustomMasterUrl = masterPageUrl;

                                // Allow unsafe updates again to save the master page changes
                                newSubsite.AllowUnsafeUpdates = true;
                                newSubsite.Update();

                                // Revert AllowUnsafeUpdates to its original state
                                newSubsite.AllowUnsafeUpdates = false;

                                // Register client-side script to open the new subsite in a new tab
                                RedirectURL = newSubsite.Url;
                            }


                        }
                        catch (Exception ex)
                        {
                            // Log or handle the exception as needed
                            throw new InvalidOperationException($"Error creating subsite: {ex.Message}", ex);
                        }
                        finally
                        {
                            // Restore original security validation setting

                            site.AllowUnsafeUpdates |= false;
                            parentWeb.AllowUnsafeUpdates = false;
                        }
                        Response.Redirect(RedirectURL);
                        //string script = $"window.open('{RedirectURL}', '_blank');";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenNewSubsite", script, true);
                    }
                }
            });
        
        }


        private void CreateCustomList(SPWeb subsite, string title, string deptCode)
        {
            try
            {
                subsite.AllowUnsafeUpdates = true;

                // Create a custom list named "DepartmentDetails"
                Guid listId = subsite.Lists.Add("DepartmentDetails", "", SPListTemplateType.GenericList);

                // Get the newly created list
                SPList list = subsite.Lists[listId];

                // Add fields to the list
                list.Fields.Add("Title", SPFieldType.Text, false);
                list.Fields.Add("DeptCode", SPFieldType.Text, false);

                // Set additional properties if needed

                // Update the list
                list.Update();

                SPView defaultView = list.DefaultView;

                // Add the fields to the default view
                defaultView.ViewFields.Add("Title"); // Title is usually added by default, but explicitly adding it for clarity
                defaultView.ViewFields.Add("DeptCode");

                // Update the view
                defaultView.Update();

                // Add an item to the list with Title and DeptCode
                SPListItem newItem = list.AddItem();
                newItem["Title"] = title;
                newItem["DeptCode"] = deptCode;
                newItem.Update();

                // Commit changes
                subsite.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new InvalidOperationException($"Error creating custom list: {ex.Message}", ex);
            }
        }

        
    }
}
