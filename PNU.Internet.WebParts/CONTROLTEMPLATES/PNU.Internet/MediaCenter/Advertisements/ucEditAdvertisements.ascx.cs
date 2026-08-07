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

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
    public partial class ucEditAdvertisements : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.QueryString["RequestId"] == null)
                    return;

                if (!Page.IsPostBack)
                {
                    var user = Helper.IsAdsAllowedUser();
                    if (user == null)
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



                    GetRequestsById();

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
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList requestsList = web.Lists["AdvertisementsRequests"];

                                //SPListItem listItem = requestsList.Items.Add();
                                SPListItem listItem = requestsList.GetItemById(RequestId);

                                listItem["Title"] = txtTitle.Text;
                              
                                listItem["MediaContent"] = txtDetails.Text;
                              
                                listItem["IsHome"] = Convert.ToBoolean(opt_IsHome.SelectedValue);

                                listItem["Title_EN"] = txtTitleEn.Text;
                               
                                listItem["MediaContent_EN"] = txtDetailsEn.Text;
                              
                                if (txtVideoURL.Text.Length > 0)
                                {
                                    listItem["VideoURL"] = txtVideoURL.Text;
                                }

                                listItem["MediaDate"] = publishingDate.SelectedDate;

                                listItem["RequestStatus"] = "Approved";


                                if (fileUPload.PostedFile != null && fileUPload.HasFile)
                                {
                                    Stream fStream = fileUPload.PostedFile.InputStream;
                                    byte[] contents = new byte[fStream.Length];
                                    fStream.Read(contents, 0, (int)fStream.Length);
                                    fStream.Close();
                                    fStream.Dispose();
                                    SPAttachmentCollection attachments = listItem.Attachments;
                                    string fileName = Path.GetFileName(fileUPload.PostedFile.FileName);
                                    attachments.Add(fileName, contents);
                                }

                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;


                                var toMail = !string.IsNullOrEmpty(Convert.ToString(listItem["RequesterEmail"])) ? Convert.ToString(listItem["RequesterEmail"]) : "mhelrefaie@pnu.edu.sa";
                                var groupUsers = Helper.GetAdsAdminUsers();
                                List<string> ccMail = new List<string>();
                                foreach (var u in groupUsers)
                                {
                                    ccMail.Add(u.UserEmail);
                                }

                                string action = "الموافقة";

                                var subjectMail = $"المركز الإعلامي - تم {action} علي نشر الإعلان ";
                                StringBuilder bodyMail = new StringBuilder();
                                bodyMail.Append("<html>");
                                bodyMail.Append($"<body><p>تم {action} علي نشر الخبر </p></body>");
                                bodyMail.Append("<br/>");
                                bodyMail.Append($"عنوان الإعلان {Convert.ToString(listItem["Title"])}");
                                bodyMail.Append("<br/>");
                                bodyMail.Append("مع تحيات المركز الإعلامي");
                                bodyMail.Append("</html>");


                                EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());
   
                                dvForm.Attributes.Add("class", "d-none"); 
                                hSuccess.Attributes.Add("class", "block");
                                
                                //Response.Redirect("AdsRequestlist.aspx");

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



                            if (item["FacultyName"] != null)
                                txtRequester.Text = Convert.ToString(item["FacultyName"]);

                            publishingDate.SelectedDate = Convert.ToDateTime(item["MediaDate"].ToString());
                            txtTitle.Text = Convert.ToString(item["Title"]);
                          
                            txtDetails.Text = Convert.ToString(item["MediaContent"]);

                            txtTitleEn.Text = Convert.ToString(item["Title_EN"]);
                          
                            txtDetailsEn.Text = Convert.ToString(item["MediaContent_EN"]);

                            opt_IsHome.SelectedValue = "False";
                            if (item["IsHome"] != null)
                            {
                                if (item["IsHome"].ToString() == "1")
                                {
                                    opt_IsHome.SelectedValue = "True";
                                }

                            }
                            //opt_IsHome.SelectedValue = Convert.ToString(item["IsHome"]);



                            if (item["VideoURL"] != null)
                                txtVideoURL.Text = item["VideoURL"].ToString();

                            if (item.Attachments.Count > 0)
                            {
                                SPAttachmentCollection attachments = item.Attachments;

                                SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                // Image1.ImageUrl = file.ServerRelativeUrl;
                                imgUrl.NavigateUrl = SPContext.Current.Site.Url + file.ServerRelativeUrl;
                                imgUrl.Text = "رابط صورة الإعلان";
                            }
                            else
                                imgUrl.Text = string.Empty;

                        }
                    }
                }
            });






        }
    }
}
