using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Fields;
using Microsoft.SharePoint.Upgrade;
using Microsoft.SharePoint.Utilities;
using PNU.Jobs.Classes;
using PNU.Jobs.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static PNU.Jobs.Classes.Helper;

namespace PNU.Jobs.webparts.JobsList
{
    public partial class JobsListUserControl : UserControl
    {

        public ArrayList qryParam {
            get
            {
                if (ViewState["searchFilter"] != null)
                    return (ArrayList)(ViewState["searchFilter"]);
                else
                    return new ArrayList();
            }
            set
            {
                ViewState["searchFilter"] = value;
            }
        }

        public bool Sorting
        {
            get
            {
                if (ViewState["Sorting"] != null)
                    return (bool)(ViewState["Sorting"]);
                else
                    return false;
            }
            set
            {
                ViewState["Sorting"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadLookups();
                LoadJobsList();
            }
        }


        private void LoadLookups()
        {
            var JobType = LookupManager.GetChoicesLookup("JobsList", "JobType");
            var JobCategory = LookupManager.GetChoicesLookup("JobsList", "JobCategory");

            ddlJobType.DataSource = JobType;
            ddlJobType.DataTextField = "Name";
            ddlJobType.DataValueField = "Value";
            ddlJobType.DataBind();

            ddlJobType.Items.Insert(0, new ListItem("--الكل--", "0"));

            ddlJobCategory.DataSource = JobCategory;
            ddlJobCategory.DataTextField = "Name";
            ddlJobCategory.DataValueField = "Value";
            ddlJobCategory.DataBind();

            ddlJobCategory.Items.Insert(0, new ListItem("--الكل--", "0"));

        }
        private void LoadJobsList()
        {
            
            var Jobs = GetSPListItemsPosition();
            if (Jobs != null)
            {

                PagedDataSource pgitems = new PagedDataSource();

                pgitems.DataSource = Jobs;
                pgitems.AllowPaging = true;
                pgitems.PageSize = Convert.ToInt32( JobsList._PageSize);
                pgitems.CurrentPageIndex = PageNumber;
                if (pgitems.PageCount > 1)
                {
                    rptPaging.Visible = true;
                    ArrayList pages = new ArrayList();
                    for (int i = 0; i < pgitems.PageCount; i++)
                        pages.Add((i + 1).ToString());
                    rptPaging.DataSource = pages;
                    rptPaging.DataBind();
                }
                else
                {
                    rptPaging.Visible = false;
                }

                rptgetAllData.DataSource = pgitems;
                rptgetAllData.DataBind();


            }
        }


        public List<JobDto> GetSPListItemsPosition()
        {
            List<JobDto> _jobsList = new List<JobDto>();
          
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite sPSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb sPWeb = sPSite.OpenWeb())
                    {
                       
                        SPQuery objQuery = new SPQuery();
                        if (qryParam .Count > 0)
                            objQuery = Helper.GenerateCAMLQuery(qryParam);
                        else
                            objQuery.Query = "<OrderBy><FieldRef Name='JobDate' Ascending=" + Sorting + " /></OrderBy>";

                        objQuery.RowLimit = JobsList._PageSize;
                       

                        do
                        {

                            SPListItemCollection collListItems = sPWeb.Lists[JobsList._ListName].GetItems(objQuery);

                            foreach (SPListItem item in collListItems)
                            {
                                var job = new JobDto()
                                {
                                    ID =Convert.ToInt32(item["ID"]),
                                    UniqueId = new Guid(Convert.ToString(item["UniqueId"])),
                                    JobTitle = Convert.ToString(item["Title"]),
                                    JobDescription = Convert.ToString(item["JobDescription"]),
                                    JobCategory = Convert.ToString(item["JobCategory"]),
                                    JobType = Convert.ToString(item["JobType"]),
                                    JobDate = Convert.ToDateTime(item["JobDate"].ToString())
                                };

                                _jobsList.Add(job);

                            }
                            objQuery.ListItemCollectionPosition =
                              collListItems.ListItemCollectionPosition;
                      

                        } while (objQuery.ListItemCollectionPosition != null);


                    }
                }
            });

            return _jobsList;
        }


        public int PageNumber
        {
            get
            {
                if (ViewState["PageNumber"] != null)
                    return Convert.ToInt32(ViewState["PageNumber"]);
                else
                    return 0;
            }
            set
            {
                ViewState["PageNumber"] = value;
            }
        }
        protected void rptPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            PageNumber = Convert.ToInt32(e.CommandArgument) - 1;
            LoadJobsList();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ArrayList arrList = new ArrayList();
            if(txtSearchInputs.Text!="")
                arrList.Add("<Contains> <FieldRef Name='Title'/><Value Type='Text'>"+txtSearchInputs.Text+"</Value> </Contains>");
            if(ddlJobType.SelectedIndex > 0)
                arrList.Add("<Eq><FieldRef Name='JobType'/><Value Type='Text'>" + ddlJobType.SelectedValue+ "</Value></Eq>");
            if(ddlJobCategory.SelectedIndex > 0)
                arrList.Add("<Eq><FieldRef Name='JobCategory'/><Value Type='Text'>" + ddlJobCategory.SelectedValue + "</Value></Eq>");
            if(txtDate.Text !="")
                arrList.Add("<Eq><FieldRef Name='JobDate' /><Value IncludeTimeValue='TRUE' Type='DateTime'>" + SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(txtDate.Text)) + "</Value></Eq>");


            qryParam = arrList;
            LoadJobsList();

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchInputs.Text = "";
            ddlJobCategory.SelectedIndex = 0;
            ddlJobType.SelectedIndex = 0;
            txtDate.Text = "";
            qryParam = new ArrayList();
            LoadJobsList();
        }

        protected void ddlSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlSorting.SelectedIndex > 0)
                Sorting = Convert.ToBoolean(ddlSorting.SelectedValue);
            LoadJobsList();
        }
    }

   
}


