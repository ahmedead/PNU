using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucRequestDetailsOld1 : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;
            if (!IsPostBack)
            {
                NewsUsers newsUsers = new NewsUsers();
                newsUsers = busclsNewsUsers.GetCurrentUser();

                if (newsUsers == null)
                {
                    dvForm.Attributes.Add("class", "d-none");
                    hMsg.Attributes.Add("class", "block");

                    return;
                }
                dv_userAction.Visible = false;
                RequestsList obj = GetRequestsById();
                if (obj.RequestStatus == "Pending")
                {
                    if (obj.NextRequestStatus != "")
                    {
                        if (newsUsers.IsAdmin)
                            dv_userAction.Visible = true;
                        else
                        {
                            if (newsUsers.WorkFlowSteps.Contains(obj.NextRequestStatus))
                                dv_userAction.Visible = true;

                        }
                    }
                }
                else if (obj.RequestStatus == "Rejected")
                {
                    dv_userAction.Visible = true;
                    divSubmit.Visible = false;
                    if (obj.UserComments != null && obj.UserComments.ToString() != "")
                        txtComments.Text = obj.UserComments.ToString();
                }


            }
        }
        private RequestsList GetRequestsById()
        {

            RequestsList obj = new RequestsList();  
            if (Page.Request.QueryString["RequestId"] == null)
                return null;

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
                            
                            obj = SPFactory.MapListItemsToClass<RequestsList>(item);
                            opt_IsHome.SelectedValue = "False";
                            if (item["IsHome"] != null)
                            {
                                if (item["IsHome"].ToString() == "1")
                                {
                                    opt_IsHome.SelectedValue = "True";
                                }

                            }

                            if (item["FacultyName"] != null)
                                lblFacultyName.Text = Convert.ToString(item["FacultyName"]);

                            txtDate.Text = Convert.ToString(item["MediaDate"]);
                            txtTitle.Text = Convert.ToString(item["Title"]);
                            txtSummary.Text = Convert.ToString(item["Summary"]);
                            txtDetails.Text = Convert.ToString(item["MediaContent"]);

                            txtTitleEn.Text = Convert.ToString(item["Title_EN"]);
                            txtSummaryEn.Text = Convert.ToString(item["Summary_EN"]);
                            txtDetailsEn.Text = Convert.ToString(item["MediaContent_EN"]);

                        

                            if (item["VideoURL"] != null)
                                txtVideoURL.Text = item["VideoURL"].ToString();

                            if (item.Attachments.Count > 0)
                            {
                                SPAttachmentCollection attachments = item.Attachments;

                                SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                Image1.ImageUrl = file.ServerRelativeUrl;
                            }

                            if (item["AdditionalFileURL"] != null)
                            {
                                divAdditionalFileURL.Visible = true;
                                AdditionalFileURLLink.NavigateUrl = SPContext.Current.Site.Url + item["AdditionalFileURL"].ToString();
                                AdditionalFileURLLink.Text = "رابط الملف";
                            }

                        }
                    }
                }
            });



            return obj;



        }
        private bool CompleteTask(string userAction)
        {
            bool retVal = false;

            if (Page.Request.QueryString["RequestId"] == null)
                return false;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            string requesterUser = "";
            string newTitle = "";

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList requestsList = web.Lists["RequestsList"];

                            SPListItem item = requestsList.GetItemById(RequestId);
                            
                            if (item == null)
                                return;

                            newTitle = Convert.ToString(item["Title"]);
                            if (item["RequesterEmail"] != null)
                                requesterUser = Convert.ToString(item["RequesterEmail"]);
                            if(userAction == "Approved")
                            {
                                if (item["NextRequestStatus"] != null && item["NextRequestStatus"].ToString() != "")
                                {
                                    NewsWorkflowSteps NextStep = busclsNewsWorkflowSteps.GetNextStep(item["NextRequestStatus"].ToString());
                                    if (NextStep != null)
                                        item["NextRequestStatus"] = NextStep.WorkflowStepName;
                                    else
                                    {
                                        item["NextRequestStatus"] = "";
                                        item["RequestStatus"] = userAction;
                                    }
                                }
                                else
                                    item["RequestStatus"] = userAction;

                            }
                            else
                                item["RequestStatus"] = userAction;

                            item["UserComments"] = txtComments.Text;
                            //item["IsHome"] = Convert.ToInt32(opt_IsHome.SelectedValue);

                            if (opt_IsHome.SelectedValue == "True")
                                item["IsHome"] = 1;
                            else
                                item["IsHome"] = 0;


                            web.AllowUnsafeUpdates = true;
                            item.Update();
                            web.AllowUnsafeUpdates = false;
                            retVal = true;
                        }
                    }
                }
            });


            var toMail = requesterUser != "" ? requesterUser : "mhelrefaie@pnu.edu.sa";
            var groupUsers = Helper.GetAdminUsers();
            List<string> ccMail = new List<string>();
            foreach (var u in groupUsers)
            {
                ccMail.Add(u.UserEmail);
            }

            string action = "";
            if (userAction == "Approved")
            {
                action = "الموافقة";
            }
            else if (userAction == "Rejected")
            {
                action = "الرفض";
            }
            else
            {
                action = "إعادة";
            }
            var subjectMail = $"المركز الإعلامي - تم {action} علي نشر الخبر ";
            StringBuilder bodyMail = new StringBuilder();
            bodyMail.Append("<html>");
            bodyMail.Append($"<body><p>تم {action} علي نشر الخبر </p></body>");
            bodyMail.Append("<br/>");
            bodyMail.Append($"عنوان الخبر {newTitle}");
            bodyMail.Append("<br/>");
            bodyMail.Append("مع تحيات المركز الإعلامي");
            bodyMail.Append("</html>");

            
            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());
            return retVal;
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            var result = CompleteTask("Approved");
            if(result)
                dv_userAction.Visible = false;
            else
                dv_userAction.Visible = true;

            Response.Redirect("requestlist.aspx",false);
            //if (result)
            //    createPage();
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            // Enable validation for txtComments
            rfvFaculties.Enabled = true;
            rfvFaculties.ValidationGroup = "RejectRequest";

            // Trigger validation
            Page.Validate("RejectRequest");

            if (Page.IsValid)
            {
                var result = CompleteTask("Rejected");
                if (result)
                    dv_userAction.Visible = false;
                else
                    dv_userAction.Visible = true;


                Response.Redirect("requestlist.aspx", false);
            }


            

        }

        protected void brnEdit_Click(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            Response.Redirect("EditNews.aspx?RequestId=" + RequestId);
        }
    }


    

}
