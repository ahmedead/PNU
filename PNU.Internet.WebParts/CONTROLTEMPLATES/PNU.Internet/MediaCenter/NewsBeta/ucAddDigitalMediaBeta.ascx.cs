using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta
{
    public partial class ucAddDigitalMediaBeta : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string WebUrl { get; set; } = "/ar/MediaCenter/NewsBeta/";

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListName { get; set; } = "DigitalMedia";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    var user = Helper.IsAllowedUser();
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

                    BindMediaCategory();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }


        private void BindMediaCategory()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb(WebUrl))
                            {
                                SPList requestsList = web.Lists["MainCategory"];
                                SPQuery qry = new SPQuery();
                                qry.Query = @"<Where> <Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>0</Value> </Eq> </Where>";
                                //List<clsLookUP> MainCategory = SPFactory.MapListItemsToClass<clsLookUP>(requestsList.Items);
                                SPListItemCollection itelColl = requestsList.GetItems(qry);
                                if (itelColl != null && itelColl.Count > 0)
                                {
                                    ddlMediaCategory.DataSource = itelColl.GetDataTable();
                                    ddlMediaCategory.DataTextField = "Title";
                                    ddlMediaCategory.DataValueField = "TitleEn";
                                    ddlMediaCategory.DataBind();



                                }
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
                            using (SPWeb web = site.OpenWeb(WebUrl))
                            {
                                SPList requestsList = web.Lists[ListName];

                                SPListItem listItem = requestsList.Items.Add();

                                listItem["Title"] = string.IsNullOrEmpty(txtTitle.Text) ? "" : txtTitle.Text;
                                listItem["Summary"] = string.IsNullOrEmpty(txtSummary.Text) ? "" : txtSummary.Text;
                                listItem["MediaContent"] = string.IsNullOrEmpty(txtDetails.Text) ? "" : txtDetails.Text;
                                listItem["MainCategory"] = ddlMediaCategory.SelectedItem.Text;
                                listItem["FacultyName"] = "";

                                listItem["RequesterName"] = requesterName;
                                listItem["RequesterEmail"] = requesterEmail;

                                listItem["Title_EN"] = string.IsNullOrEmpty(txtTitleEn.Text) ? "" : txtTitleEn.Text;
                                listItem["Summary_EN"] = string.IsNullOrEmpty(txtSummaryEn.Text) ? "" : txtSummaryEn.Text;
                                listItem["MediaContent_EN"] = string.IsNullOrEmpty(txtDetailsEn.Text) ? "" : txtDetailsEn.Text;
                                listItem["MainCategory_EN"] = ddlMediaCategory.SelectedValue;

                                

                                listItem["MediaDate"] = publishingDate.SelectedDate;
                                listItem["ApprovalGroupName"] = "PNU-WF";
                                listItem["RequestStatus"] = "Approved";

                                if (txtPublisherName.Text.Length > 0)
                                    listItem["PublisherName"] = txtPublisherName.Text;
                                if (txtPublisherPosition.Text.Length > 0)
                                    listItem["PublisherPosition"] = txtPublisherPosition.Text;




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
                                if (txtVideoURL.Text.Trim() != "" && txtVideoURL.Text.Trim() != null)
                                {


                                    listItem["VideoURL"] = txtVideoURL.Text;
                                    listItem["MediaTypes_EN"] = "Video";
                                }
                                //VideofileUPload

                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;
                                Response.Redirect("/ar/MediaCenter/NewsBeta/Pages/DigitalMediaRequestList.aspx");

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

        protected void ShowMessage(string Message, MessageType type)
        {
            try
            {            //dvMain.Visible = false;
                         //dvRequestsList.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }
        protected string GetUserDepartment(string listName, string userName)
        {
            try
            {
                string departmentName = string.Empty;
                List<string> ListGroups = new List<string>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists.TryGetList(listName);
                            if (list == null)
                                return;
                            SPListItemCollection itemColl = list.GetItems();
                            if (itemColl == null)
                                return;
                            foreach (SPListItem item in itemColl)
                            {
                                var user = busclsRequestsList.GetUsersInGroup(Convert.ToString(item["GroupName"])).FirstOrDefault(a => a.LoginName == userName);

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

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return "";
            }


        }


        private void FindCheckBoxes(ControlCollection controls, List<string> selectedCheckboxes)
        {
            try
            {
                foreach (Control control in controls)
                {
                    //HtmlInputCheckBox obj = (System.Web.UI.HtmlControls.HtmlInputCheckBox)control;
                    //if (obj != null)
                    //{
                    //    string value = obj.Value; // You can use other properties like checkbox.ID, checkbox.ToolTip, etc.
                    //    selectedCheckboxes.Add(value);

                    //}
                    //else if (control.HasControls())
                    //{
                    //    FindCheckBoxes(control.Controls, selectedCheckboxes);
                    //}


                    if (control is HtmlInputCheckBox checkbox && control.ID == "MediaTypescheckRadio")
                    {
                        if (checkbox.Checked)
                        {
                            string value = checkbox.Value; // You can use other properties like checkbox.ID, checkbox.ToolTip, etc.
                            selectedCheckboxes.Add(value);
                        }
                    }
                    else if (control.HasControls())
                    {
                        FindCheckBoxes(control.Controls, selectedCheckboxes);
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }



        public static NewsUserEditor IsAllowedUser()
        {
            try
            {
                var user = SPContext.Current.Web.CurrentUser;
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='UserAccount' LookupId='TRUE' />
                                 <Value Type='Lookup'>" + user.ID + @"</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList reqList = web.Lists["NewsUsers"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return new NewsUserEditor();
                if (objNew.Count == 0)
                    return new NewsUserEditor();

                return new NewsUserEditor { Id = Convert.ToInt16(objNew[0]["ID"]), IsAdmin = Convert.ToBoolean(objNew[0]["IsAdmin"]) };



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
            }


            return null;



        }

    }
}
