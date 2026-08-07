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
using PNU.Internet.WebParts;
using System.Collections;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
    public partial class usAddAdvertisements : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    if (CheckUserType() <= 0)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");

                        return;
                    }
                    else if (CheckUserType() == 2) // content editor
                    {
                        LoadDepts();
                        dvDept.Visible = true;
                        rfvFaculties.Enabled = true;
                        dvIsHome.Visible = true;
                    }
                    else if (CheckUserType() == 1) // admin users
                    {
                        dvDept.Visible = false;
                        rfvFaculties.Enabled = false;
                        dvIsHome.Visible = false;
                    }

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        protected void LoadDepts()
        {
            var currentUser = SPContext.Current.Web.CurrentUser.LoginName;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                    {
                        SPList list = web.Lists["ContentAdminUsers"];
                        SPQuery query = new SPQuery();
                        query.Query = @"
                        <Where>
                            <Contains>
                                <FieldRef Name='MediaTypes' />
                                <Value Type='MultiChoice'>Ads</Value>
                            </Contains>
                        </Where>";

                        SPListItemCollection items = list.GetItems(query);

                        foreach (SPListItem item in items)
                        {
                            SPFieldUserValueCollection userFieldValues = (SPFieldUserValueCollection)item["DeptUsers"];

                            foreach (SPFieldUserValue userFieldValue in userFieldValues)
                            {
                                if (userFieldValue.User.LoginName.Equals(currentUser, StringComparison.OrdinalIgnoreCase))
                                {
                                    ddlFaculties.Items.Insert(0, new ListItem(Convert.ToString(item["Title"]), Convert.ToString(item["DeptCode"])));
                                }
                            }
                        }
                    }
                }
            });
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

        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var requesterName = SPContext.Current.Web.CurrentUser.Name;
                var requesterEmail = SPContext.Current.Web.CurrentUser.Email;


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList requestsList = web.Lists["AdvertisementsRequests"];

                                SPListItem listItem = requestsList.Items.Add();

                                listItem["Title"] = txtTitle.Text;
                                listItem["MediaContent"] = txtDetails.Text;
                                if (CheckUserType() == 2)
                                {
                                    listItem["FacultyName"] = ddlFaculties.SelectedItem.Text;
                                    listItem["FacultyName_EN"] = ddlFaculties.SelectedValue;
                                    if (opt_IsHome.SelectedValue == "True")
                                        listItem["IsHome"] = 1;
                                    else
                                        listItem["IsHome"] = 0;

                                    //listItem["IsHome"] = Convert.ToBoolean(opt_IsHome.SelectedValue);

                                }
                                else if (CheckUserType() == 1)
                                {
                                    listItem["FacultyName"] = "Main";
                                    listItem["FacultyName_EN"] = "Main";
                                    listItem["IsHome"] = true;
                                }

                                listItem["RequesterName"] = requesterName;
                                listItem["RequesterEmail"] = requesterEmail;
                                listItem["Title_EN"] = txtTitleEn.Text;
                                listItem["MediaContent_EN"] = txtDetailsEn.Text;
                               

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
                                var groupUsers = Helper.GetAdsAdminUsers();
                                List<string> ccMail = new List<string>();
                                foreach (var u in groupUsers)
                                {
                                    ccMail.Add(u.UserEmail);
                                }

                                string Subject = "";
                                if (rfvFaculties.Enabled == false)
                                    Subject = "إضافة إعلان";
                                else
                                    Subject= $"{ddlFaculties.SelectedItem.Text} - إضافة إعلان";

                                var subjectMail = Subject;
                                StringBuilder bodyMail = new StringBuilder();
                                bodyMail.Append("<html>");
                                bodyMail.Append("<body>");
                                bodyMail.Append($"<p>تم إضافة إعلان باسم {txtTitle.Text} في انتظار الموافقة عليه  </p><br/>مرسل الطلب {requesterName} ");
                                bodyMail.Append($"<br/> <a href ={SPContext.Current.Web.Url + "/Pages/AdsRequestDetails.aspx?RequestId=" + listItem.ID} > رابط الإعلان </a><br/>");
                                bodyMail.Append("مع الشكر");
                                bodyMail.Append("</body>");
                                bodyMail.Append("</html>");


                                EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());

                                if (CheckUserType() == 2)
                                {
                                    dvForm.Attributes.Add("class", "d-none");
                                    hSuccess.Attributes.Add("class", "block");
                                }
                                else
                                    Response.Redirect("/ar/MediaCenter/MediaCenterAdmin/Pages/AdsRequestlist.aspx");
                                //Response.Redirect("AdsRequestlist.aspx", false);

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
