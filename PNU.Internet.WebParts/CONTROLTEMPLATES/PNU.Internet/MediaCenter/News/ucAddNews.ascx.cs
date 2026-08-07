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
    public partial class ucAddNews : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    divConfirmation.Visible = false;
                    if (CheckUserType() <= 0)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");

                        return;
                    }
                    else if (CheckUserType() == 2) // content editor
                    {
                        LoadDepts();
                        //dvIsHome.Visible = true;
                        dvDept.Visible = true;
                        rfvFaculties.Enabled = true;
                        divConfirmation.Visible = true;
                    }
                    else if (CheckUserType() == 1) // admin users
                    {
                        //dvIsHome.Visible = false;
                        dvDept.Visible = false;
                        rfvFaculties.Enabled = false;
                    }




                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected void CheckBoxRequired_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = chkConfirmation.Checked;
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
                                <Value Type='MultiChoice'>News</Value>
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



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if(CheckUserType() == 2)
                {
                    if (!chkConfirmation.Checked)
                    {
                        hConfirmation.Attributes.Add("class", "block");
                        return;
                    }
                }                
                var requesterName = SPContext.Current.Web.CurrentUser.Name;
                var requesterEmail = SPContext.Current.Web.CurrentUser.Email;


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                            {
                                SPList requestsList = web.Lists["RequestsList"];

                                SPListItem listItem = requestsList.Items.Add();

                                listItem["Title"] = txtTitle.Text;
                                listItem["Summary"] = txtSummary.Text;
                                listItem["MediaContent"] = txtDetails.Text;
                                listItem["MainCategory"] = "الأخبار الرئيسية";//ddlMediaCategory.SelectedItem.Text;
                                listItem["MainCategory_EN"] = "News highlights";//ddlMediaCategory.SelectedValue;
                                if (CheckUserType() == 2)
                                {
                                    //listItem["IsHome"] = Convert.ToBoolean(opt_IsHome.SelectedValue);
                                    listItem["IsHome"] = false;
                                    listItem["FacultyName"] = ddlFaculties.SelectedItem.Text;
                                    listItem["FacultyName_EN"] = ddlFaculties.SelectedValue;

                                    NewsWorkflowSteps NextStep = busclsNewsWorkflowSteps.GetFirstStep();
                                    if (NextStep != null)
                                        listItem["NextRequestStatus"] = NextStep.WorkflowStepName;
                                    else
                                        listItem["NextRequestStatus"] = "";



                                    //Confirmation

                                    #region Confirmation
                                    string AdditionalfileUrl = "";

                                    if (fileAdditionalUpload.HasFile)
                                    {
                                        SPList DocumentsList = web.Lists["المستندات"];

                                        web.AllowUnsafeUpdates = true;
                                        // Folder name will be the ID you provide
                                        string folderName = "NewsAdditionalFiles"; // Replace with the actual ID you provide

                                        // Check if the folder exists, if not, create it
                                        SPFolder folder = web.GetFolder(DocumentsList.RootFolder.ServerRelativeUrl + "/" + folderName);
                                        if (!folder.Exists)
                                        {
                                            // Create the new folder in the library
                                            SPListItem folderItem = DocumentsList.Folders.Add(DocumentsList.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder, folderName);

                                            folderItem.Update();
                                            folder = folderItem.Folder;


                                        }
                                        // Get the uploaded file stream
                                        Stream fileStream = fileAdditionalUpload.PostedFile.InputStream;

                                        // File name
                                        string fileName = Path.GetFileName(fileAdditionalUpload.PostedFile.FileName);

                                        // Combine the folder path with the file name
                                        AdditionalfileUrl = folder.ServerRelativeUrl + "/" + fileName;

                                        // Save the file in the folder
                                        SPFile uploadedFile = folder.Files.Add(AdditionalfileUrl, fileStream, true); // true to overwrite if it exists
                                        uploadedFile.Item.Update(); // Commit the changes to the list item
                                        web.AllowUnsafeUpdates = true;
                                        // Close the file stream
                                        fileStream.Close();
                                        fileStream.Dispose();

                                        web.AllowUnsafeUpdates = false;
                                        // Save file URL in the list item
                                        listItem["AdditionalFileURL"] = AdditionalfileUrl;

                                    }
                                    
                                    #endregion
                                    //End Confirmation

                                }
                                else if (CheckUserType() == 1)
                                {
                                    listItem["FacultyName"] = "المركز الإعلامي";
                                    listItem["FacultyName_EN"] = "Media Center";
                                    listItem["IsHome"] = true;
                                }

                                listItem["RequesterName"] = requesterName;
                                listItem["RequesterEmail"] = requesterEmail;

                                listItem["Title_EN"] = txtTitleEn.Text;
                                listItem["Summary_EN"] = txtSummaryEn.Text;
                                listItem["MediaContent_EN"] = txtDetailsEn.Text;
                               

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

                                string fileUrl = "";

                                // Now that we have the folder, we can upload the file
                                if (VideofileUPload.PostedFile != null && VideofileUPload.HasFile)
                                {
                                    SPList DocumentsList = web.Lists["المستندات"];

                                    web.AllowUnsafeUpdates = true;
                                    // Folder name will be the ID you provide
                                    string folderName = "News"; // Replace with the actual ID you provide

                                    // Check if the folder exists, if not, create it
                                    SPFolder folder = web.GetFolder(DocumentsList.RootFolder.ServerRelativeUrl + "/" + folderName);
                                    if (!folder.Exists)
                                    {
                                        // Create the new folder in the library
                                        SPListItem folderItem = DocumentsList.Folders.Add(DocumentsList.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder, folderName);

                                        folderItem.Update();
                                        folder = folderItem.Folder;


                                    }
                                    // Get the uploaded file stream
                                    Stream fileStream = VideofileUPload.PostedFile.InputStream;

                                    // File name
                                    string fileName = Path.GetFileName(VideofileUPload.PostedFile.FileName);

                                    // Combine the folder path with the file name
                                    fileUrl = folder.ServerRelativeUrl + "/" + fileName;

                                    // Save the file in the folder
                                    SPFile uploadedFile = folder.Files.Add(fileUrl, fileStream, true); // true to overwrite if it exists
                                    uploadedFile.Item.Update(); // Commit the changes to the list item

                                    // Close the file stream
                                    fileStream.Close();
                                    fileStream.Dispose();

                                    listItem["VideoURL"] = fileUrl;
                                    listItem["MediaTypes_EN"] = "Video";

                                    web.AllowUnsafeUpdates = false;
                                }


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;


                                string CommonTemplate = PortalHelper.GetEmailTemplate("CommonTemplate");
                                CommonTemplate = CommonTemplate.Replace("{Title}", "الأخبار - تم إضافة خبر جديد")
                                .Replace("{LinkTitle}", "رابط الخبر ")
                                .Replace("{LinkURL}", SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId=" + listItem.ID)
;
                                Hashtable values = new Hashtable();
                                values.Add("{requesterName}", requesterName);
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


                                var toMail =  requesterEmail != "" ? requesterEmail : "aesharaf@pnu.edu.sa";
                                var groupUsers = Helper.GetAdminUsers();
                                List<string> ccMail = new List<string>();
                                foreach (var u in groupUsers)
                                {
                                    ccMail.Add(u.UserEmail);
                                }


                                var subjectMail = $"المركز الإعلامي - إضافة خبر";
                                StringBuilder bodyMail = new StringBuilder();
                                bodyMail.Append("<html>");
                                bodyMail.Append("<body>");
                                bodyMail.Append($"<p>تم إضافة خبر باسم {txtTitle.Text} في انتظار الموافقة عليه  </p><br/>مرسل الطلب {requesterName} ");
                                bodyMail.Append($"<br/> <a href ={SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId="+listItem.ID} > رابط الخبر </a><br/>");
                                bodyMail.Append("مع الشكر");
                                bodyMail.Append("</body>");
                                bodyMail.Append("</html>");


                                //EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());

                                EmailUtility.SendEmail(toMail, ccMail, subjectMail, CommonTemplate);



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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return "";
            }
            
        }
        

       
    }

   
}
