using PNU.Jobs.Dtos;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using PNU.Jobs.Classes;

namespace PNU.Jobs.webparts.JobDetails
{
    public partial class JobDetailsUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["JobId"] == null)
                return;
            if (!IsPostBack)
            {
                getJobDetails();
            }
        }

        private void getJobDetails()
        {

          
            ArrayList qryParam = new ArrayList();
            if (Page.Request.QueryString["JobId"] == null)
            {
                return;
            }

            

            var GuidJobId = new Guid(Convert.ToString(Page.Request.QueryString["JobId"]));

            SPSecurity.RunWithElevatedPrivileges(delegate () {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPQuery JobListQuery = new SPQuery() ;
                        SPQuery JobCondQuery = new SPQuery();
                        SPQuery JobDocQuery = new SPQuery();

                        SPList JobList = web.Lists.TryGetList("JobsList");
                        SPList JobCondList = web.Lists.TryGetList("JobsConditions");
                        SPList JobDocList = web.Lists.TryGetList("JobsRequiredDocuments");

                        JobListQuery.Query = "<Where><Eq><FieldRef Name='UniqueId'/><Value Type='Guid'>"+GuidJobId+"</Value></Eq></Where>";
                        var JobsListColl = JobList.GetItems(JobListQuery);
                        var item = JobsListColl[0];
                        lblJobTitle.Text = Convert.ToString(item["Title"]);
                        lblDueDate.Text = string.Format("{0:d}", Convert.ToDateTime(item["AppliedDueDate"]));
                        lblJobCategory.Text = Convert.ToString(item["JobCategory"]);
                        lblJobDescription.Text = Convert.ToString(item["JobDescription"]);
                        lblJobType.Text = Convert.ToString(item["JobType"]);
                        lblPublishingDate.Text = string.Format("{0:d}", Convert.ToDateTime(item["JobDate"]));
                        lblLocation.Text = Convert.ToString(item["JobLocation"]);

                        var JobId = Convert.ToInt32(item["ID"]);

                        JobCondQuery.Query = "<Where><Eq><FieldRef Name='JobId' LookupId='TRUE' /><Value Type='Lookup'>" + JobId + "</Value></Eq></Where>";
                        JobDocQuery.Query = "<Where><Eq><FieldRef Name='JobId' LookupId='TRUE' /><Value Type='Lookup'>" + JobId + "</Value></Eq></Where>";

                        var JobscondLColl = JobCondList.GetItems(JobCondQuery).GetDataTable();
                        var JobsDocColl = JobDocList.GetItems(JobDocQuery).GetDataTable();

                        repConditions.DataSource = JobscondLColl;
                        repConditions.DataBind();

                        repDocs.DataSource = JobsDocColl;
                        repDocs.DataBind();

                    }
                }
            });

            

        }
  
    }
}
