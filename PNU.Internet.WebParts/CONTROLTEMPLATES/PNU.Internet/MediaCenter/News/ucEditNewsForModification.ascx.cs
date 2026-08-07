using Microsoft.SharePoint;
using Newtonsoft.Json.Linq;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using Portal.Main.Helper;
using System;
using System.Collections;
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
    public partial class ucEditNewsForModification : UserControl
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
                    if (CheckUserType() <= 0 || obj.RequesterEmail == null)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");

                        return;
                    }
                    else
                    {
                       if(obj.RequesterEmail.ToLower() != Helper.GetUserEmail())
                        {
                            dvForm.Attributes.Add("class", "d-none");
                            hMsg.Attributes.Add("class", "block");

                            return;
                        }

                    }



                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        protected int CheckUserType()
        {
            var mediaAdminUser = Helper.IsAllowedUser();
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
                                
                                listItem["RequestStatus"] = "Pending";
                                listItem["Title"] = txtTitle.Text;
                                listItem["Summary"] = txtSummary.Text;
                                listItem["MediaContent"] = txtDetails.Text;
                                // listItem["MainCategory"] = ddlMediaCategory.SelectedItem.Text;
                                // listItem["FacultyName"] = txtRequester.Text;

                                //listItem["IsHome"] =Convert.ToBoolean(opt_IsHome.SelectedValue);
                                listItem["IsHome"] = false;

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

                                


                                var subjectMail = $"المركز الإعلامي - تم تعديل خبر";
                                //StringBuilder bodyMail = new StringBuilder();
                                //bodyMail.Append("<html>");
                                //bodyMail.Append("<body>");
                                //bodyMail.Append($"<p>تم تعديل  خبر باسم {txtTitle.Text} في انتظار الموافقة عليه  </p><br/>مرسل الطلب {listItem["RequesterName"].ToString()} ");
                                //bodyMail.Append($"<br/> <a href ={SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId=" + listItem.ID} > رابط الخبر </a><br/>");
                                //bodyMail.Append("مع الشكر");
                                //bodyMail.Append("</body>");
                                //bodyMail.Append("</html>");



                                string CommonTemplate = PortalHelper.GetEmailTemplate("CommonTemplate");
                                CommonTemplate = CommonTemplate.Replace("{Title}", subjectMail)
                                .Replace("{LinkTitle}", "رابط الخبر ")
                                .Replace("{LinkURL}", SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId=" + listItem.ID)
;
                                Hashtable values = new Hashtable();
                                values.Add("{requesterName}", listItem["RequesterName"].ToString());
                                values.Add("{txtTitle}", txtTitle.Text);
                                values.Add("{MediaContent}", txtDetails.Text);

                                string AddNewTemplate = PortalHelper.GetEmailTemplate("AddNewTemplate");

                                // replace values in design template
                                foreach (string key in values.Keys)
                                {
                                    if (values[key] != null)
                                    {
                                        // replace variable in template design
                                        AddNewTemplate = AddNewTemplate.Replace(key, values[key].ToString());
                                    }
                                }
                                CommonTemplate = CommonTemplate.Replace("{Table}", AddNewTemplate);



                                EmailUtility.SendEmail(toMail, ccMail, subjectMail, CommonTemplate.ToString());

                                if (CheckUserType() == 2)
                                {
                                    dvForm.Attributes.Add("class", "d-none");
                                    hSuccess.Attributes.Add("class", "block");
                                }
                                else

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

                            
                            if(item["UserComments"] != null)
                            {
                                txtComments.Text = item["UserComments"].ToString();
                            }

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
