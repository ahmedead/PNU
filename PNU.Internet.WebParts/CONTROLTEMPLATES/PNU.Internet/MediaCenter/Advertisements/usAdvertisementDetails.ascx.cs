using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
    public partial class usAdvertisementDetails : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;
            if (!IsPostBack)
            {
               

                if (CheckUserType() <= 0)
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
        protected int CheckUserType()
        {
            var mediaAdminUser = Helper.IsAdsAllowedUser();
            if (mediaAdminUser?.Id > 0)
            {
                return (int)UserTypeEnum.MediaAdminUser;
            }

            var contentEditorUser = Helper.IsContentEditorUser();
            if (contentEditorUser?.Id > 0)
            {
                return (int)UserTypeEnum.ContentEditorUser;
            }

            return 0;
        }

        private void GetRequestsById()
        {

            if (Page.Request.QueryString["RequestId"] == null)
                return;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList requestsList = web.Lists["AdvertisementsRequests"];

                            SPListItem item = requestsList.GetItemById(RequestId);

                            opt_IsHome.SelectedValue = "False";
                            if (item["IsHome"] != null)
                            {
                                if(item["IsHome"].ToString() == "1")
                                {
                                    opt_IsHome.SelectedValue = "True";
                                }
                                
                            }
                            //opt_IsHome.SelectedValue = Convert.ToString(item["IsHome"]);

                            if (item["FacultyName"] != null)
                            {
                                lblFacultyName.Text = Convert.ToString(item["FacultyName"]);
                                if (item["FacultyName"].ToString() != "Main")
                                {
                                    btnAskForEdit.Visible = true;
                                }

                            }
                            

                            txtDate.Text = Convert.ToString(item["MediaDate"]);
                            txtTitle.Text = Convert.ToString(item["Title"]);
                           
                            txtDetails.Text = Convert.ToString(item["MediaContent"]);

                            txtTitleEn.Text = Convert.ToString(item["Title_EN"]);
                           
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
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList requestsList = web.Lists["AdvertisementsRequests"];

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
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList requestsList = web.Lists["AdvertisementsRequests"];


                            SPListItem item = requestsList.GetItemById(RequestId);

                            if (item == null)
                                return;

                            newTitle = Convert.ToString(item["Title"]);
                            if (item["RequesterEmail"] != null)
                                requesterUser = Convert.ToString(item["RequesterEmail"]);
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
            var groupUsers = Helper.GetAdsAdminUsers();
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
            else if (userAction == "ModificationRequest")
            {
                action = "طلب تعديل";
            }
            else
            {
                action = "إعادة";
            }
            var subjectMail = $"المركز الإعلامي - تم {action} علي نشر الإعلان ";
            StringBuilder bodyMail = new StringBuilder();
            bodyMail.Append("<html>");
            bodyMail.Append($"<body><p>تم {action} علي نشر الخبر </p></body>");
            bodyMail.Append("<br/>");
            bodyMail.Append($"عنوان الإعلان {newTitle}");
            bodyMail.Append("<br/>");
            if(userAction == "ModificationRequest")
            {
                bodyMail.Append($"<br/> <a href ={SPContext.Current.Web.Url + "/Pages/EditAdvertisement.aspx?RequestId=" + RequestId} > رابط الإعلان </a><br/>");
                bodyMail.Append($"<br/>سبب طلب التعديل : {txtComments.Text}<br/>");
            }
            else if (userAction == "Rejected")
            {
                bodyMail.Append($"<br/> <a href ={SPContext.Current.Web.Url + "/Pages/EditAdvertisement.aspx?RequestId=" + RequestId} > رابط الإعلان </a><br/>");
                bodyMail.Append($"<br/>سبب الرفض  : {txtComments.Text}<br/>");
            }
            bodyMail.Append("مع تحيات المركز الإعلامي");
            bodyMail.Append("</html>");

            
            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());

            return retVal;
        }
        protected void btnAskForEdit_Click(object sender, EventArgs e)
        {

            // Enable validation for txtComments
            rfvFaculties.Enabled = true;
            rfvFaculties.ValidationGroup = "EditRequest";

            // Trigger validation
            Page.Validate("EditRequest");

            if (Page.IsValid)
            {
                var result = CompleteTask("ModificationRequest");
                if (result)
                    dv_userAction.Visible = false;
                else
                    dv_userAction.Visible = true;

                Response.Redirect("AdsRequestlist.aspx", false);
            }


            
            //if (result)
            //    createPage();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            // Disable validation for txtComments
            rfvFaculties.Enabled = false;
            var result = CompleteTask("Approved");
            if (result)
                dv_userAction.Visible = false;
            else
                dv_userAction.Visible = true;

            Response.Redirect("AdsRequestlist.aspx", false);
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


                Response.Redirect("AdsRequestlist.aspx", false);
            }


            

        }

        protected void brnEdit_Click(object sender, EventArgs e)
        {
            // Disable validation for txtComments
            rfvFaculties.Enabled = false;
            if (Page.Request.QueryString["RequestId"] == null)
                return;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            Response.Redirect("EditAds.aspx?RequestId=" + RequestId);
        }
    }
}
