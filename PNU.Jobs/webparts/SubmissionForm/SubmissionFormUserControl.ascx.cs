using Microsoft.SharePoint;
using PNU.Jobs.Classes;
using PNU.Jobs.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Jobs.webparts.SubmissionForm
{
    public partial class SubmissionFormUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["JobId"] == null)
                return;
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Save();
        }

        void Save()
        {

            if (Page.Request.QueryString["JobId"] == null)
                return;
            
            var JobId= new Guid(Convert.ToString(Page.Request.QueryString["JobId"]));

            var jobData = getJobDetails();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList requestsList = web.Lists["JobsApplicants"];

                            SPListItem listItem = requestsList.Items.Add();

                            listItem["Title"] = jobData.Title;
                            listItem["JobId"] = jobData.Id;
                            listItem["FirstName"] = txtFName.Text;
                            listItem["LastName"] = txtLName.Text;
                            listItem["ApplicantEmail"] = txtEmail.Text;
                            listItem["ApplicantMobile"] = txtPhone.Text;
                            listItem["ApplicantNationality"] = ddlNationality.SelectedValue;
                            listItem["ApplicantCountry"] = ddlCountry.SelectedValue;
                            listItem["ApplicantCity"] = txtCity.Text;


                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;

                            var toMail = txtEmail.Text;
                            List<string> ccMail = new List<string> { "ma1746@pnudstst.edu.sa" };
                           

                            var subjectMail = "نموذج تقديم لوظيفة";
                            StringBuilder bodyMail = new StringBuilder();
                            bodyMail.Append("<html>");
                            bodyMail.Append("<body><p>تم تقديم الطلب بنجاح</p></body>");
                            bodyMail.Append("</html>");


                            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());


                            Response.Redirect("default.aspx");
                        }
                    }
                }
            });

        }
        private JobTitle getJobDetails()
        {

            ArrayList qryParam = new ArrayList();
           

            if (Page.Request.QueryString["JobId"] == null)
                return null;

            var JobId = new Guid(Convert.ToString(Page.Request.QueryString["JobId"]));
            qryParam.Add("<Eq><FieldRef Name='UniqueId'/><Value Type='Guid'>" + JobId + "</Value></Eq>");

            var JobItem = Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, "JobsList", qryParam)[0];

            return new JobTitle()
            {
                Title = Convert.ToString(JobItem["Title"]),
                Id =Convert.ToInt32(JobItem["ID"])

            };

        }
    }

    public class JobTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
