using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.sub_News
{
    public partial class ucEditFacultyNews : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.QueryString["RequestId"] == null)
                    return;

                if (!Page.IsPostBack)
                {
                    var user = Helper.IsContentEditorUser();
                    if (user==null)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");
                    }
                    if (user.Id <= 0)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");
                    }

                    //

                    ddlFaculties.Items.Insert(0, new ListItem(user.DeptTitle, user.DeptTitle));

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
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/Faculties/"))
                            {
                                SPList requestsList = web.Lists["FacultiesRequestsList"];

                                //SPListItem listItem = requestsList.Items.Add();
                                SPListItem listItem = requestsList.GetItemById(RequestId);

                                listItem["Title"] = txtTitle.Text;
                                listItem["Summary"] = txtSummary.Text;
                                listItem["MediaContent"] = txtDetails.Text;
                                
                                listItem["FacultyName"] = ddlFaculties.SelectedItem.Text;
                                listItem["FacultyName_EN"] = ddlFaculties.SelectedValue;


                                listItem["Title_EN"] = txtTitleEn.Text;
                                listItem["Summary_EN"] = txtSummaryEn.Text;
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


                                //var toMail = "hmserhan@pnudstst.edu.sa";
                                //var groupUsers = Helper.GetUsersInGroup("PNU-WF");
                                //List<string> ccMail = new List<string>();
                                //foreach (PnuUser u in groupUsers)
                                //{
                                //    ccMail.Add(u.Email);
                                //}

                                //var subjectMail = "طلب موافقة علي نشر محتوى";
                                //StringBuilder bodyMail = new StringBuilder();
                                //bodyMail.Append("<html>");
                                //bodyMail.Append("<body><p>تم تقديم الطلب بنجاح</p></body>");
                                //bodyMail.Append("</html>");


                                //EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());


                                Response.Redirect("/ar/Faculties/Pages/requestslist.aspx");

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
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/Faculties/"))
                        {
                            SPList requestsList = web.Lists["FacultiesRequestsList"];
                            SPListItem item = requestsList.GetItemById(RequestId);


                            if (item["FacultyName"] != null && item["FacultyName_EN"] != null)
                                ddlFaculties.SelectedValue = Convert.ToString(item["FacultyName_EN"]);

                            publishingDate.SelectedDate = Convert.ToDateTime(item["MediaDate"].ToString());
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
                                // Image1.ImageUrl = file.ServerRelativeUrl;
                                imgUrl.NavigateUrl = SPContext.Current.Site.Url + file.ServerRelativeUrl;
                                imgUrl.Text = "رابط صورة الخبر";
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
