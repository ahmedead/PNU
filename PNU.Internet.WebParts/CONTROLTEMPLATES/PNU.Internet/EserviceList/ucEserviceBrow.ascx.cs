using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucEserviceBrow : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int pageNumber = 1;
                if (Request.QueryString["page"] != null)
                {
                    pageNumber = int.Parse(Request.QueryString["page"]);
                    BindServices(pageNumber);
                }
                else
                {
                    BindServices(pageNumber);
                    FillDropDownList();
                    LoadCollegeServices();
                }
            }

            else
            {
                ddlFilterSelectedValue.Value = string.Empty;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindServices(1); // Reset to the first page when searching
        }

        protected void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterServices(1); // Reset to the first page when filtering
        }

        private void BindServices(int pageNumber)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();

                            // Build the query based on search and filters
                            string searchQuery = txtSearch.Value.Trim();
                            
                                
                            if (!string.IsNullOrEmpty(searchQuery))
                            {
                                query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Active' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                                 <Contains>
                                                    <FieldRef Name='ARServiceName' />
                                                    <Value Type='Text'>{searchQuery}</Value>
                                                 </Contains>
                                              </And>
                                           </Where>
                                        </Query>";

                            }
                            else
                            {
                                string Filterquery = ddlFilterSelectedValue.Value;
                                

                                string TargetGroup = PortalHelper.IsArabic ? "TargetGroup_EN" : "TargetGroup";


                                if (!string.IsNullOrEmpty(Filterquery))
                                {
                                    query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Active' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                                 <Contains>
                                                    <FieldRef Name='{TargetGroup}' />
                                                    <Value Type='Text'>{Filterquery}</Value>
                                                 </Contains>
                                              </And>
                                           </Where>
                                        </Query>";

                                }
                                else
                                {
                                    query.Query = $@"
                                                <Where>
                                                        <Eq>
                                                            <FieldRef Name='Active' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                </Where>";
                                }

                            }

                            // Fetch all items matching the query
                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null)
                            {
                                // Pagination logic
                                int pageSize = 6; // Number of items per page
                                int totalItems = collitem.Count;
                                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                                // Calculate the items to display for the current page
                                int skip = (pageNumber - 1) * pageSize;
                                var pagedItems = collitem.Cast<SPListItem>().Skip(skip).Take(pageSize).ToList();

                                // Map items to your custom class
                                List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(pagedItems);

                                // Bind data to the repeater
                                rptAllData.DataSource = eserviceList;
                                rptAllData.DataBind();

                                // Generate pagination links
                                GeneratePagination(totalPages, pageNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void FilterServices(int pageNumber)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();

                            // Build the query based on search and filters
                            string Filterquery = string.Empty;
                            if (ddlFilter.SelectedValue == "All")
                            {
                                Filterquery = string.Empty;
                            }
                            else 
                            {
                                Filterquery = ddlFilter.SelectedValue;
                            }
                            ddlFilterSelectedValue.Value = Filterquery;

                            string TargetGroup = PortalHelper.IsArabic ? "TargetGroup_EN" : "TargetGroup";
                            

                            if (!string.IsNullOrEmpty(Filterquery))
                            {
                                query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Active' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                                 <Contains>
                                                    <FieldRef Name='{TargetGroup}' />
                                                    <Value Type='Text'>{Filterquery}</Value>
                                                 </Contains>
                                              </And>
                                           </Where>
                                        </Query>";

                            }
                            else
                            {
                                query.Query = $@"
                                                <Where>
                                                        <Eq>
                                                            <FieldRef Name='Active' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                </Where>";
                            }

                            // Fetch all items matching the query
                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null)
                            {
                                // Pagination logic
                                int pageSize = 6; // Number of items per page
                                int totalItems = collitem.Count;
                                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                                // Calculate the items to display for the current page
                                int skip = (pageNumber - 1) * pageSize;
                                var pagedItems = collitem.Cast<SPListItem>().Skip(skip).Take(pageSize).ToList();

                                // Map items to your custom class
                                List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(pagedItems);

                                // Bind data to the repeater
                                rptAllData.DataSource = eserviceList;
                                rptAllData.DataBind();

                                // Generate pagination links
                                GeneratePagination(totalPages, pageNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void FillDropDownList()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();

                            query.Query = $@"
                                                <Where>
                                                        <Eq>
                                                            <FieldRef Name='Active' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                </Where>";



                            // Fetch all items matching the query
                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null)
                            {
                                
                                List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(collitem);

                                var distinctTargetGroups = eserviceList
                                    .GroupBy(service => service.TargetGroup)  // Group by TargetGroup
                                    .Select(group => group.First())           // Select the first item from each group
                                    .ToList();

                                for (int i = 0; i < distinctTargetGroups.Count; i++)
                                {
                                    distinctTargetGroups[i].TargetGroup = SPFactory.GetLocalizedTitle(distinctTargetGroups[i].TargetGroup_EN, distinctTargetGroups[i].TargetGroup);
                                }
                                ddlFilter.DataSource = distinctTargetGroups;
                                ddlFilter.DataTextField = "TargetGroup" ;
                                ddlFilter.DataValueField = "TargetGroup";
                                ddlFilter.DataBind();
                                if(PortalHelper.IsArabic)
                                    ddlFilter.Items.Insert(0, new ListItem("الكل", "All"));
                                else
                                    ddlFilter.Items.Insert(0, new ListItem("All", "All"));

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void GeneratePagination(int totalPages, int currentPage)
        {
            // Clear existing pagination
            pagination.Controls.Clear();

            // Add "Previous" button
            var prevButton = new HtmlGenericControl("li");
            prevButton.Attributes["class"] = "page-item" + (currentPage == 1 ? " disabled" : "");
            prevButton.InnerHtml = $"<a class='page-link' href='?page={currentPage - 1}' tabindex='-1' aria-disabled='true'>&laquo;</a>";
            pagination.Controls.Add(prevButton);

            // Add page numbers
            for (int i = 1; i <= totalPages; i++)
            {
                var pageItem = new HtmlGenericControl("li");
                pageItem.Attributes["class"] = "page-item" + (i == currentPage ? " active" : "");
                pageItem.InnerHtml = $"<a class='page-link' href='?page={i}'>{i}</a>";
                pagination.Controls.Add(pageItem);
            }

            // Add "Next" button
            var nextButton = new HtmlGenericControl("li");
            nextButton.Attributes["class"] = "page-item" + (currentPage == totalPages ? " disabled" : "");
            nextButton.InnerHtml = $"<a class='page-link' href='?page={currentPage + 1}'>&raquo;</a>";
            pagination.Controls.Add(nextButton);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Value = "";
            BindServices(1);
            FillDropDownList();
            LoadCollegeServices();
        }


        private void LoadCollegeServices()
        {
            try
            {


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {

                            SPList list = web.Lists["ESystems"];
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = @"<Where>
                                                   <Eq>
                                                      <FieldRef Name='Visibility' />
                                                      <Value Type='Boolean'>1</Value>
                                                   </Eq>
                                                </Where>
                                                <OrderBy>
                                                   <FieldRef Name='ItemOrder' Ascending='True' />
                                                </OrderBy>
                                             </Query>";
                                query.RowLimit = 6;
                                SPListItemCollection collitem = list.GetItems(query);

                                if (collitem != null && collitem.Count > 0)
                                {

                                    List<ESystems> _AllData = new List<ESystems>();
                                    _AllData = SPFactory.MapListItemsToClass<ESystems>(collitem);


                                    rptSystems.DataSource = _AllData;
                                    rptSystems.DataBind();

                                }


                            }

                        }
                    }
                });




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!Page.IsPostBack)
        //        {
        //            int pageNumber = 1;
        //            if (Request.QueryString["page"] != null)
        //            {
        //                pageNumber = int.Parse(Request.QueryString["page"]);
        //            }
        //            BindServices(pageNumber);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
        //    }
        //}

        //private void BindServices(int pageNumber)
        //{
        //    try
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
        //        {
        //            using (SPWeb web = site.OpenWeb())
        //            {
        //                SPList list = web.Lists["EservicesList"];
        //                if (list != null)
        //                {
        //                    SPQuery query = new SPQuery();
        //                    query.Query = "<Where><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq></Where>";
        //                    //query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='TargetGroup' /><Value Type='Choice'>Student</Value></Eq></And></Where>";

        //                    // Fetch all items
        //                    SPListItemCollection collitem = list.GetItems(query);

        //                    if (collitem != null)
        //                    {
        //                        // Pagination logic
        //                        int pageSize = 6; // Number of items per page
        //                        int totalItems = collitem.Count;
        //                        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //                        // Calculate the items to display for the current page
        //                        int skip = (pageNumber - 1) * pageSize;
        //                        var pagedItems = collitem.Cast<SPListItem>().Skip(skip).Take(pageSize).ToList();

        //                        // Map items to your custom class
        //                        List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(pagedItems);

        //                        // Bind data to the repeater
        //                        rptAllData.DataSource = eserviceList;
        //                        rptAllData.DataBind();

        //                        // Generate pagination links
        //                        GeneratePagination(totalPages, pageNumber);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
        //    }
        //}

        //private void GeneratePagination(int totalPages, int currentPage)
        //{
        //    // Clear existing pagination
        //    pagination.Controls.Clear();

        //    // Add "Previous" button
        //    var prevButton = new HtmlGenericControl("li");
        //    prevButton.Attributes["class"] = "page-item" + (currentPage == 1 ? " disabled" : "");
        //    prevButton.InnerHtml = $"<a class='page-link' href='?page={currentPage - 1}' tabindex='-1' aria-disabled='true'>&laquo;</a>";
        //    pagination.Controls.Add(prevButton);

        //    // Add page numbers
        //    for (int i = 1; i <= totalPages; i++)
        //    {
        //        var pageItem = new HtmlGenericControl("li");
        //        pageItem.Attributes["class"] = "page-item" + (i == currentPage ? " active" : "");
        //        pageItem.InnerHtml = $"<a class='page-link' href='?page={i}'>{i}</a>";
        //        pagination.Controls.Add(pageItem);
        //    }

        //    // Add "Next" button
        //    var nextButton = new HtmlGenericControl("li");
        //    nextButton.Attributes["class"] = "page-item" + (currentPage == totalPages ? " disabled" : "");
        //    nextButton.InnerHtml = $"<a class='page-link' href='?page={currentPage + 1}'>&raquo;</a>";
        //    pagination.Controls.Add(nextButton);
        //}


    }
}
