using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.sub_News
{
    public partial class ucSubRequestDetails : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;
            if (!IsPostBack)
            {
                var user = Helper.IsContentEditorUser();
                if (user==null)
                {
                    dvForm.Attributes.Add("class", "d-none");
                    hMsg.Attributes.Add("class", "block");
                    return;
                }
                if (user.Id <= 0)
                {
                    dvForm.Attributes.Add("class", "d-none");
                    hMsg.Attributes.Add("class", "block");
                    return;
                }


                EnableUserActions();



                GetRequestsById();
            }
        }

        private void EnableUserActions()
        {
            var requestStatus = IsPendingRequestStatus();
            if (Helper.IsAdminUser() && requestStatus)
            {
                dv_userAction.Visible = true;
            }
            else
            {
                dv_userAction.Visible = false;
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
                        using (SPWeb web = site.OpenWeb("/ar/Faculties/"))
                        {
                            SPList requestsList = web.Lists["FacultiesRequestsList"];
                            SPListItem item = requestsList.GetItemById(RequestId);
                            

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

                        }
                    }
                }
            });






        }
        private bool IsPendingRequestStatus()
        {
            bool retVal = false;
            if (Page.Request.QueryString["RequestId"] == null)
                return retVal;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/Faculties/"))
                        {
                            SPList requestsList = web.Lists["FacultiesRequestsList"];

                            SPListItem item = requestsList.GetItemById(RequestId);
                            if (item != null && Convert.ToString(item["RequestStatus"]) == "Pending")
                                retVal = true;
                        }
                    }
                }
            });

            return retVal;
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
                        using (SPWeb web = site.OpenWeb("/ar/Faculties/"))
                        {
                            SPList requestsList = web.Lists["FacultiesRequestsList"];

                            SPListItem item = requestsList.GetItemById(RequestId);

                            if (item == null)
                                return;

                            newTitle = Convert.ToString(item["Title"]);
                            if (item["RequesterEmail"] != null)
                                requesterUser = Convert.ToString(item["RequesterEmail"]);
                            item["RequestStatus"] = userAction;
                            item["UserComments"] = txtComments.Text;


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
            if (result)
                dv_userAction.Visible = false;
            else
                dv_userAction.Visible = true;

            Response.Redirect("requestslist.aspx",false);
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            var result = CompleteTask("Rejected");
            if (result)
                dv_userAction.Visible = false;
            else
                dv_userAction.Visible = true;


            Response.Redirect("requestslist.aspx", false);
        }
        protected void brnEdit_Click(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            Response.Redirect("EditFacultyNews.aspx?RequestId=" + RequestId);
        }
    }
}
