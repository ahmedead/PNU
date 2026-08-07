using Microsoft.SharePoint;
using PNU.Workflow.Classes;
using PNU.Workflow.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static PNU.Workflow.Classes.Helper;

namespace PNU.Workflow.WebParts.RequestDetails
{
    public partial class RequestDetailsUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;
            if (!IsPostBack)
            {
                GetRequestsById();
            }
        }

        private void GetRequestsById()
        {

            if (Page.Request.QueryString["RequestId"] == null)
                return;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList requestsList = web.Lists["RequestsList"];
                            SPListItem item = requestsList.GetItemById(RequestId);
                            txtCategory.Text = Convert.ToString(item["MainCategory"]);

                            if(item["FacultyName"] !=null)
                                lblFacultyName.Text = Convert.ToString(item["FacultyName"]);

                            txtDate.Text = Convert.ToString(item["MediaDate"]);
                            txtTitle.Text = Convert.ToString(item["Title"]);
                            txtSummary.Text = Convert.ToString(item["Summary"]);
                            txtDetails.Text = Convert.ToString(item["MediaContent"]);

                            txtTitleEn.Text = Convert.ToString(item["Title"]);
                            txtSummaryEn.Text = Convert.ToString(item["Summary"]);
                            txtDetailsEn.Text = Convert.ToString(item["MediaContent"]);

                            if (item["MediaTypes"] != null)
                                txtMediaTypes.Text = item["MediaTypes"].ToString();

                            if (item["VideoURL"] != null)
                                txtVideoURL.Text = item["VideoURL"].ToString();

                            if (item.Attachments.Count > 0)
                            {
                                SPAttachmentCollection attachments = item.Attachments;

                                SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                Image1.ImageUrl = file.ServerRelativeUrl;
                            }

                        }
                    }
                }
            });


            

            

        }

        
    }
}
