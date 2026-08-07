using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

using PNU.Workflow.Classes;
using PNU.Workflow.Entities;
using PNU.Workflow.Utilities;
using static PNU.Workflow.Classes.Helper;

namespace PNU.Workflow.WebParts.RequestForm
{
    public partial class RequestFormUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var currentUser = SPContext.Current.Web.CurrentUser.LoginName;
                lblFacultyName.Text = GetUserDepartment("WorkFlowRequestersList", currentUser);
                LoadLookups();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList requestsList = web.Lists["RequestsList"];

                            SPListItem listItem = requestsList.Items.Add();

                            listItem["Title"] = txtTitle.Text;
                            listItem["Summary"] = txtSummary.Text;
                            listItem["MediaContent"] = txtDetails.Text;
                            listItem["MainCategory"] = ddlCategory.SelectedValue;
                            listItem["FacultyName"] = lblFacultyName.Text;
                            List<ListItem> selectedTypes = chkTypes.Items.Cast<ListItem>()
                                                        .Where(li => li.Selected)
                                                        .ToList();

                            SPFieldMultiChoiceValue itemValue = new SPFieldMultiChoiceValue();
                            foreach (var i in selectedTypes)
                            {
                                itemValue.Add(i.Value);
                            }

                            listItem["MediaTypes"] = itemValue;
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


                           // ShowMessage("your request has been sent successfully", MessageType.Success);

                            

                            //var to = SPContext.Current.Web.EnsureUser(Helper.ExtractLoginName(SPContext.Current.Web.CurrentUser.LoginName));
                            
                            var toMail = "hmserhan@pnudstst.edu.sa";
                            var groupUsers = Helper.GetUsersInGroup("PNU-WF");
                            List<string> ccMail = new List<string>();
                            foreach (PnuUser u in groupUsers)
                            {
                                ccMail.Add(u.Email);
                            }
                            
                            var subjectMail = "طلب موافقة علي نشر محتوى";
                            StringBuilder bodyMail = new StringBuilder();
                            bodyMail.Append("<html>");
                            bodyMail.Append("<body><p>تم تقديم الطلب بنجاح</p></body>");
                            bodyMail.Append("</html>");


                            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());


                            Response.Redirect("requestlist.aspx");

                        }
                    }
                }
            });

        }
        protected String GetAttachmentLink(Object item)
        {
            SPListItem listItem = (SPListItem)item;
            if (listItem.Attachments != null && listItem.Attachments.Count > 0)
                return listItem.Attachments.UrlPrefix + "/" + listItem.Attachments[0];
            else
                return "";
        }
        protected void LoadLookups()
        {
            var Categories =LookupManager.GetChoicesLookup("RequestsList", "MainCategory");
            var MediaTypes = LookupManager.GetMultipleChoicesLookup("RequestsList", "MediaTypes");

            ddlCategory.DataSource=Categories;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "Value";
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(0, new ListItem("--اختر--", "0"));

            chkTypes.DataSource = MediaTypes;
            chkTypes.DataTextField = "Name";
            chkTypes.DataValueField = "Value";
            chkTypes.DataBind();

        }
        protected void ShowMessage(string Message, MessageType type)
        {
            dvMain.Visible = false;
            dvRequestsList.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);

        }
        protected string GetUserDepartment(string listName, string userName)
        {
            string departmentName = string.Empty;
            List<string> ListGroups = new List<string>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("/ar/MediaCenter/"))
                    {
                        SPList list = web.Lists[listName];
                        if (list == null)
                            return;
                        SPListItemCollection itemColl = list.GetItems();
                        if (itemColl == null || itemColl.Count == 0)
                            return;
                        foreach (SPListItem item in itemColl)
                        {
                            var user = GetUsersInGroup(Convert.ToString(item["GroupName"])).FirstOrDefault(a => a.LoginName == userName);
                           
                            if (user != null)
                            {
                                departmentName = Convert.ToString(item["TitleEn"]);
                                break;
                            }
                        }


                    }

                }
            });

            return departmentName;

        }
    
    
    }

   
}
