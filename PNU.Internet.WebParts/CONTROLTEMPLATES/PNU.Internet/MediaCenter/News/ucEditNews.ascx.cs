using Microsoft.SharePoint;
using Newtonsoft.Json.Linq;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucEditNews : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.QueryString["RequestId"] == null)
                    return;

                if (!Page.IsPostBack)
                {
                    var user =Helper.IsAllowedUser();
                    RequestsList obj = GetRequestsById(); 
                    if (user == null)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");
                        return;
                    }
                    if(!user.CanEdit)
                    {
                        if (!user.IsAdmin)
                        {
                            if(obj.RequestStatus != "Pending")
                            {
                                dvForm.Attributes.Add("class", "d-none");
                                hMsg.Attributes.Add("class", "block");
                                return;
                            }
                        }
                    }
                    

                    //else if (user.Id <= 0)
                    //{
                    //    dvForm.Attributes.Add("class", "d-none");
                    //    hMsg.Attributes.Add("class", "block");
                    //    return;
                    //}

                   

                    
                  
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
     


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // var requesterName = SPContext.Current.Web.CurrentUser.Name;
                // var requesterEmail = SPContext.Current.Web.CurrentUser.Email;

                if (Page.Request.QueryString["RequestId"] == null)
                {
                    hMsg.Visible = true;
                    hMsg.InnerHtml = "الطلب الذي تريد تعديله غير صحيح";
                    dvForm.Visible = false;
                    return;
                }
                   

                var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                            {
                                SPList requestsList = web.Lists["RequestsList"];

                                //SPListItem listItem = requestsList.Items.Add();
                                SPListItem listItem = requestsList.GetItemById(RequestId);

                                string RequestStatus = "";
                                if (listItem["RequestStatus"] != null)
                                {
                                    if (listItem["RequestStatus"].ToString() != null)
                                        RequestStatus = listItem["RequestStatus"].ToString();
                                }
                                listItem["Title"] = txtTitle.Text;
                                listItem["Summary"] = txtSummary.Text;
                                listItem["MediaContent"] = txtDetails.Text;
                                // listItem["MainCategory"] = ddlMediaCategory.SelectedItem.Text;
                                // listItem["FacultyName"] = txtRequester.Text;


                                //listItem["IsHome"] =Convert.ToBoolean(opt_IsHome.SelectedValue);
                                //listItem["IsHome"] = false;

                                listItem["Title_EN"] = txtTitleEn.Text;
                                listItem["Summary_EN"] = txtSummaryEn.Text;
                                listItem["MediaContent_EN"] = txtDetailsEn.Text;
                               // listItem["MainCategory_EN"] = ddlMediaCategory.SelectedValue;
                                //listItem["FacultyName_EN"] = ddlFaculty.SelectedValue;

                                if (txtVideoURL.Text.Length > 0)
                                {
                                    listItem["VideoURL"] = txtVideoURL.Text;
                                }

                                listItem["MediaDate"] = publishingDate.SelectedDate;

                                if (listItem["NextRequestStatus"] != null && listItem["NextRequestStatus"].ToString() != "")
                                {
                                    NewsWorkflowSteps NextStep = busclsNewsWorkflowSteps.GetNextStep(listItem["NextRequestStatus"].ToString());
                                    if (NextStep != null)
                                        listItem["NextRequestStatus"] = NextStep.WorkflowStepName;
                                    else
                                    {
                                        listItem["NextRequestStatus"] = "";
                                        listItem["RequestStatus"] = "Approved";
                                    }
                                }
                                else
                                    listItem["RequestStatus"] = "Approved";
                                //listItem["RequestStatus"] = "Approved";



                                //if (fileUPload.PostedFile != null && fileUPload.HasFile)
                                //{
                                //    Stream fStream = fileUPload.PostedFile.InputStream;
                                //    byte[] contents = new byte[fStream.Length];
                                //    fStream.Read(contents, 0, (int)fStream.Length);
                                //    fStream.Close();
                                //    fStream.Dispose();
                                //    SPAttachmentCollection attachments = listItem.Attachments;
                                //    string fileName = Path.GetFileName(fileUPload.PostedFile.FileName);
                                //    attachments.Add(fileName, contents);
                                //}


                                if (fileUPload.PostedFile != null && fileUPload.HasFile)
                                {
                                    // Get the file stream and contents
                                    Stream fStream = fileUPload.PostedFile.InputStream;
                                    byte[] contents = new byte[fStream.Length];
                                    fStream.Read(contents, 0, (int)fStream.Length);
                                    fStream.Close();
                                    fStream.Dispose();

                                    // Get the SPAttachmentCollection
                                    SPAttachmentCollection attachments = listItem.Attachments;

                                    // Remove all existing attachments
                                    try
                                    {
                                        foreach (string attachment in attachments)
                                        {
                                            attachments.Delete(attachment);

                                        }
                                    }
                                    catch { }

                                    // Add the new attachment
                                    string fileName = Path.GetFileName(fileUPload.PostedFile.FileName);
                                    attachments.Add(fileName, contents);
                                }


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;


                                var toMail = !string.IsNullOrEmpty(Convert.ToString(listItem["RequesterEmail"])) ? Convert.ToString(listItem["RequesterEmail"]) : "mhelrefaie@pnu.edu.sa";
                                var groupUsers = Helper.GetAdminUsers();
                                List<string> ccMail = new List<string>();
                                foreach (var u in groupUsers)
                                {
                                    ccMail.Add(u.UserEmail);
                                }

                                if(RequestStatus != "Approved")
                                {
                                    string action = "الموافقة";

                                    var subjectMail = $"المركز الإعلامي - تم {action} علي نشر الخبر ";
                                    StringBuilder bodyMail = new StringBuilder();
                                    bodyMail.Append("<html>");
                                    bodyMail.Append($"<body><p>تم {action} علي نشر الخبر </p></body>");
                                    bodyMail.Append("<br/>");
                                    bodyMail.Append($"عنوان الخبر {Convert.ToString(listItem["Title"])}");
                                    bodyMail.Append("<br/>");
                                    bodyMail.Append("مع تحيات المركز الإعلامي");
                                    bodyMail.Append("</html>");


                                    EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());
                                }
                                


                                Response.Redirect("/ar/MediaCenter/News/Pages/requestlist.aspx");

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
        protected String GetAttachmentLink(Object item)
        {
            try
            {
                SPListItem listItem = (SPListItem)item;
                if (listItem.Attachments != null && listItem.Attachments.Count > 0)
                    return listItem.Attachments.UrlPrefix + "/" + listItem.Attachments[0];
                else
                    return "";
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return "";
            }

        }

   

        private RequestsList GetRequestsById()
        {

            if (Page.Request.QueryString["RequestId"] == null)
                return null;
            RequestsList obj = new RequestsList();
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

                            if (item["FacultyName"] != null)
                                txtRequester.Text = Convert.ToString(item["FacultyName"]);

                            publishingDate.SelectedDate = Convert.ToDateTime(item["MediaDate"].ToString());
                            txtTitle.Text = Convert.ToString(item["Title"]);
                            txtSummary.Text = Convert.ToString(item["Summary"]);
                            txtDetails.Text = Convert.ToString(item["MediaContent"]);

                            txtTitleEn.Text = Convert.ToString(item["Title_EN"]);
                            txtSummaryEn.Text = Convert.ToString(item["Summary_EN"]);
                            txtDetailsEn.Text = Convert.ToString(item["MediaContent_EN"]);


                            //opt_IsHome.SelectedValue = Convert.ToString(item["IsHome"]);
                            opt_IsHome.SelectedValue = "False";
                            if (item["IsHome"] != null)
                            {
                                if (item["IsHome"].ToString() == "1")
                                {
                                    opt_IsHome.SelectedValue = "True";
                                }

                            }


                            if (item["VideoURL"] != null)
                                txtVideoURL.Text = item["VideoURL"].ToString();

                            if (item.Attachments.Count > 0)
                            {
                                SPAttachmentCollection attachments = item.Attachments;

                                SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                // Image1.ImageUrl = file.ServerRelativeUrl;
                                imgUrl.NavigateUrl = SPContext.Current.Site.Url + file.ServerRelativeUrl;
                                imgUrl.Text =  "رابط صورة الخبر";
                            }
                            else
                                imgUrl.Text = string.Empty;

                        }
                    }
                }
            });


            return obj;



        }
    
    }
}
