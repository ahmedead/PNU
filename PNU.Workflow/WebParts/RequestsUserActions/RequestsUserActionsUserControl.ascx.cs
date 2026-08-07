using Microsoft.SharePoint;
using PNU.Workflow.Classes;
using PNU.Workflow.Entities;
using PNU.Workflow.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Fields;
using PNU.Workflow.Dtos;

namespace PNU.Workflow.WebParts.RequestsUserActions
{
    public partial class RequestsUserActionsUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;

            if (!CheckRequestStatus())
            {
                // dv_userAction.Style.Add("display", "none");
                dv_userAction.Visible = false;
            }
            else
            {
                dv_userAction.Visible = true;
            }


        }
        private bool CheckRequestStatus()
        {
            bool retVal = false;
            if (Page.Request.QueryString["RequestId"] == null)
                return false;

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
                            if(item != null && Convert.ToString(item["RequestStatus"]) =="Pending")    
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


                            if(item["RequesterEmail"] !=null)
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


            var toMail = requesterUser !="" ? requesterUser :  "mhelrefaie@pnuds.edu.sa";
            var groupUsers = Helper.GetUsersInGroup("PNU-WF");
            List<string> ccMail = new List<string>();
            foreach (PnuUser u in groupUsers)
            {
                ccMail.Add(u.Email);
            }
            
            string action = "";
            if (userAction == "Approved")
            {
                action = "الموفقة";
            }
            else if (userAction == "Rejected")
            {
                action = "الرفض";
            }
            else
            {
                action = "إعادة";
            }
            var subjectMail = $"تم {action} علي الطلب";
            StringBuilder bodyMail = new StringBuilder();
            bodyMail.Append("<html>");
            bodyMail.Append($"<body><p>تم {action} الطلب بنجاح</p></body>");
            bodyMail.Append("</html>");


            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());
            return retVal;
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            var result = CompleteTask("Approved");
            //if (result)
            //    createPage();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            var result = CompleteTask("Rejected");
        }

        protected void brnReturn_Click(object sender, EventArgs e)
        {
            var result = CompleteTask("Returned");
        }


        protected void ShowMessage(string Message, MessageType type)
        {
           // dvMain.Visible = false;
           // dvRequestsList.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);

        }

        protected void createPage()
        {
            try
            {
                
                string strPageURL = GetPageName() + ".aspx";
                string strPageTitle = "dynamic page test4";// txtTitle.Text.Trim();
                string strCreatedPageURL = string.Empty;
                string layoutName = RequestsUserActions._PageLayout;//GetLayoutName(ddlContentType.SelectedValue);

                var requestData = GetRequestDetails();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite oSite = new SPSite(SPContext.Current.Site.Url))
                    {
                    using (SPWeb web = oSite.OpenWeb(RequestsUserActions._NewsSite))
                        {

                            PublishingSite oPublishingSite = new PublishingSite(oSite);

                            //SPWeb web = SPContext.Current.Site.OpenWeb("http://epm:2019/media/subsite1/");
                            web.AllowUnsafeUpdates = true;
                            Guid PagesID = web.Lists[RequestsUserActions._PageLibrary].ID;

                            web.AllProperties["__PagesListId"] = PagesID.ToString();
                            //web.Update();
                            //web.AllProperties["__PublishingFeatureActivated"] = "True";
                            //web.Update();

                            PublishingWeb oPublishingWeb = PublishingWeb.GetPublishingWeb(web);

                            PageLayoutCollection layoutCollection = oPublishingSite.GetPageLayouts(false);

                            PageLayout layout = layoutCollection[layoutName];

                            PublishingPage page = oPublishingWeb.GetPublishingPage(web.Url + "/Pages/" + strPageURL);

                            if (page == null)
                            {
                                page = oPublishingWeb.AddPublishingPage(strPageURL, layout);
                            }
                            else
                            {
                                throw new Exception("A page with the given URL already exists. Please try a different URL");
                            }

                            if (page.ListItem.File.CheckedOutByUser == null)
                            {
                                page.CheckOut();
                            }


                            page.ListItem["NewsCategory"] = requestData.MainCategory;
                            page.ListItem["Tags"] = requestData.FacultyName;

                            List<string> selectedTypes =requestData.MediaTypes;
                            SPFieldMultiChoiceValue itemValue = new SPFieldMultiChoiceValue();
                            foreach (var i in selectedTypes)
                            {
                                itemValue.Add(i);
                            }
                            page.ListItem["NewsTags2"] = itemValue;  //media types
                            page.ListItem["ShortDescription"] = requestData.Summary;
                            //page.ListItem["SubCategory"] = ""; //deleted
                            page.ListItem["Summary"] = requestData.Summary;
                            page.ListItem["visible"] = true;
                            page.ListItem["ArticleStartDate"] = requestData.MediaDate;
                            page.ListItem["Title"] = requestData.Title;
                            page.ListItem["PublishingPageContent"] = requestData.MediaContent;



                            ImageFieldValue _pageimage = new ImageFieldValue();

                            _pageimage.ImageUrl = requestData.MediaImage;

                            ImageFieldValue _rollupimage = new ImageFieldValue();

                            _rollupimage.ImageUrl = requestData.MediaImage;

                            page.ListItem["PublishingPageImage"] = _pageimage;
                            page.ListItem["PublishingRollupImage"] = _rollupimage;



                            page.Update();
                            page.CheckIn("Page Created");

                            page.ListItem.File.Publish("Page Published");

                            web.AllowUnsafeUpdates = false;
                            strCreatedPageURL = web.Url + "/Pages/" + strPageURL;

                            Response.Redirect(strCreatedPageURL);
                        }
                    }
                });


            }
            catch (Exception ex)
            {

            }
        }
        private static string GetPageName()
        {
            Random rnd = new Random();
            var r = rnd.Next(1, 100);
            var curDate = DateTime.Now.ToString("ddMMyyyy");
            return "news" + r.ToString() + curDate.ToString();
        }
        private RequestDetailsModel GetRequestDetails()
        {

            if (Page.Request.QueryString["RequestId"] == null)
                return null;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            ArrayList qryParam = new ArrayList();
            qryParam.Add("<Eq><FieldRef Name='ID'/><Value Type='Number'>" + RequestId + "</Value></Eq>");

            SPListItemCollection listItemColl = Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, "RequestsList", qryParam)
;           if (listItemColl == null || listItemColl.Count <= 0)
                return null;

            SPListItem item = listItemColl[0];

            SPFieldMultiChoiceValue itemValue = new SPFieldMultiChoiceValue(item["MediaTypes"].ToString());
            List<string> choices = new List<string>();
            for (int i = 0; i < itemValue.Count; i++)
            {
                choices.Add(itemValue[i]);
            }

            SPAttachmentCollection attachments = item.Attachments;

            SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
            
            var requestDetails = new RequestDetailsModel
            {
                Title = Convert.ToString(item["Title"]),
                FacultyName = Convert.ToString(item["FacultyName"]),
                MainCategory = Convert.ToString(item["MainCategory"]),
                MediaContent = Convert.ToString(item["MediaContent"]),
                MediaDate = Convert.ToDateTime(item["MediaDate"]),
                Summary = Convert.ToString(item["Summary"]),
                MediaTypes = choices,
                MediaImage = file.ServerRelativeUrl

            };
           

            return requestDetails;

        }

    }
   
}
