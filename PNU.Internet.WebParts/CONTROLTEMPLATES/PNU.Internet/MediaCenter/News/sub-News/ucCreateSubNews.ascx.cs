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

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.sub_News
{
    public partial class ucCreateSubNews : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    var user = Helper.IsContentEditorUser();

                    if (user.Id <= 0)
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

                    ddlFaculties.Items.Insert(0, new ListItem(user.DeptTitle,user.DeptTitle));

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
                var requesterName = SPContext.Current.Web.CurrentUser.Name;
                var requesterEmail = SPContext.Current.Web.CurrentUser.Email;


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/Faculties/"))
                            {
                                SPList requestsList = web.Lists["FacultiesRequestsList"];

                                SPListItem listItem = requestsList.Items.Add();

                                listItem["Title"] = txtTitle.Text;
                                listItem["Summary"] = txtSummary.Text;
                                listItem["MediaContent"] = txtDetails.Text;
                                listItem["FacultyName"] = ddlFaculties.SelectedItem.Text;

                                listItem["RequesterName"] = requesterName;
                                listItem["RequesterEmail"] = requesterEmail;

                                listItem["Title_EN"] = txtTitleEn.Text;
                                listItem["Summary_EN"] = txtSummaryEn.Text;
                                listItem["MediaContent_EN"] = txtDetailsEn.Text;
                                listItem["FacultyName_EN"] = ddlFaculties.SelectedValue;

                                if (txtVideoURL.Text.Length > 0)
                                {
                                    listItem["VideoURL"] = txtVideoURL.Text;
                                }

                               

                                listItem["MediaDate"] = publishingDate.SelectedDate;
                                listItem["ApprovalGroupName"] = "PNU-WF";
                                listItem["RequestStatus"] = "Pending";


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


                                var toMail = requesterEmail != "" ? requesterEmail : "mhelrefaie@pnu.edu.sa";
                                var groupUsers = Helper.GetAdminUsers();
                                List<string> ccMail = new List<string>();
                                foreach (var u in groupUsers)
                                {
                                    ccMail.Add(u.UserEmail);
                                }


                                var subjectMail = $"{ddlFaculties.SelectedItem.Text} - إضافة خبر";
                                StringBuilder bodyMail = new StringBuilder();
                                bodyMail.Append("<html>");
                                bodyMail.Append("<body>");
                                bodyMail.Append($"<p>تم إضافة خبر باسم {txtTitle.Text} في انتظار الموافقة عليه  </p><br/>مرسل الطلب {requesterName} ");
                                bodyMail.Append($"<br/> <a href ={SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId=" + listItem.ID} > رابط الخبر </a><br/>");
                                bodyMail.Append("مع الشكر");
                                bodyMail.Append("</body>");
                                bodyMail.Append("</html>");


                                EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());


                                Response.Redirect("requestslist.aspx",false);

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

      
    }
}
