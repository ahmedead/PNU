using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Controls.Common;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms
{
    public partial class ucOpenDataRequest : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnSend.Visible = false;
            if (IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('2');", true);
            }
        }

        protected void cvRole_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Check if at least one checkbox is selected
            args.IsValid = chkRole.Items.Cast<ListItem>().Any(item => item.Selected);

            // If "Other" is selected, ensure the textbox is not empty
            if (chkRole.Items.Cast<ListItem>().Any(item => item.Selected && item.Value == "Other"))
            {
                if (string.IsNullOrEmpty(txtOther.Text))
                {
                    args.IsValid = false;
                    lblErrorMessage.Text = "Please specify 'Other'.";
                }
            }
        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    Captcha CaptchaControl = FindControl("CaptchaControl") as Captcha;
                    // First validate the CAPTCHA
                    if (!CaptchaControl.ValidateCaptcha())
                    {
                        lblException.Text = "Invalid CAPTCHA code. Please try again.";
                        lblException.Visible = true;
                        lblSuccessMessage.Visible = false;
                        return;
                    }

                    if (!ValidchkRole())
                    {
                        return;
                    }
                    string RoleString = "";
                    if (chkRole.Items.Cast<ListItem>().Any(item => item.Selected))
                    {
                        // Get all selected items from the CheckBoxList
                        var selectedItems = chkRole.Items.Cast<ListItem>()
                                                        .Where(item => item.Selected)
                                                        .Select(item => item.Value)
                                                        .ToList();

                        // Check if "Other" is selected
                        if (selectedItems.Contains("Other"))
                        {
                            if (string.IsNullOrEmpty(txtOther.Text))
                            {
                                lblErrorMessage.Text = "you must specify value for other";
                                return;
                            }

                            // Replace "Other" with the value from txtOther
                            selectedItems[selectedItems.IndexOf("Other")] = txtOther.Text;
                        }

                        // Join the selected values into a single string separated by "-"
                        RoleString = string.Join(" - ", selectedItems);
                    }
                    SPSecurity.RunWithElevatedPrivileges(delegate ()

                    {
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList list = web.Lists["OpenDataRequests"];
                                if (list != null)
                                {

                                    // Check if a similar record exists
                                    SPQuery query = new SPQuery();
                                    query.Query = "<Where>" +
                                                  "<And>" +
                                                  "<Eq><FieldRef Name='Title' /><Value Type='Text'>" + txtName.Text + "</Value></Eq>" +
                                                  "<Eq><FieldRef Name='Email' /><Value Type='Text'>" + txtEmail.Text + "</Value></Eq>" +
                                                  "</And>" +
                                                  "</Where>";

                                    SPListItemCollection existingItems = list.GetItems(query);

                                    if (existingItems.Count > 0)
                                    {
                                        lblException.Text = SPFactory.GetPNUresResource("SavedBefore");
                                        lblException.Visible = true;
                                        lblSuccessMessage.Visible = false;
                                        pnlData.Visible = false;
                                        btnSend.Visible = false;
                                        return;
                                    }



                                    OpenDataRequests contactUS = new OpenDataRequests();
                                    contactUS.Title = txtName.Text;
                                    contactUS.Name = txtName.Text;
                                    contactUS.Email = txtEmail.Text;


                                    contactUS.Role = string.Join(" - ", RoleString);



                                    // Check if any items are selected

                                    if (chkDatasets.Items.Cast<ListItem>().Any(item => item.Selected))

                                    {
                                        // Get all selected items from the ListBox
                                        var selectedItems = chkDatasets.Items.Cast<ListItem>()
                                                                             .Where(item => item.Selected)
                                                                             .Select(item => item.Value);

                                        // Join the selected values into a single string separated by "-"
                                        contactUS.Datasets = string.Join(" - ", selectedItems);



                                    }

                                    if (!string.IsNullOrEmpty(txtOpenDataRequest.Text))
                                        contactUS.ExtraOpenDataRequest = txtOpenDataRequest.Text;
                                    if (!string.IsNullOrEmpty(txtSuggest.Text))
                                        contactUS.Suggests = txtSuggest.Text;




                                    SPListItem listItem = list.Items.Add();
                                    listItem = SPFactory.MapClassToSPListItem(listItem, contactUS, true);



                                    web.AllowUnsafeUpdates = true;
                                    listItem.Update();
                                    web.AllowUnsafeUpdates = false;


                                    var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "OpenDataFromEmail" });


                                    if (smtpSettings != null)
                                    {
                                        if (Convert.ToString(smtpSettings["OpenDataFromEmail"]) != "")
                                        {
                                            var toMail = Convert.ToString(smtpSettings["OpenDataFromEmail"]);
                                            //var toMail = "aesharaf@pnu.edu.sa";

                                            List<string> ccMail = new List<string>();
                                            //ccMail.Add("aesharaf@pnu.edu.sa");


                                            var subjectMail = $"طلب بيانات مفتوحة";
                                            StringBuilder bodyMail = new StringBuilder();
                                            bodyMail.Append("<html>");
                                            bodyMail.Append("<body>");
                                            bodyMail.Append($"<p> تم استلام طلب جديد </p><br/>مرسل الطلب {contactUS.Name} ");
                                            bodyMail.Append($"<br/> الإيميل : {contactUS.Email} <br/>");
                                            bodyMail.Append($"<br/>شرحة المستفيدين من البيانات المفتوحة :  {contactUS.Role} <br/>");
                                            bodyMail.Append($"<br/>مجموعة البيانات المطلوبة :  {contactUS.Datasets} <br/>");
                                            bodyMail.Append($"<br/>طلب بيانات مفتوحة إضافية :  {contactUS.ExtraOpenDataRequest} <br/>");
                                            bodyMail.Append($"<br/>مقترحات :  {contactUS.Suggests} <br/>");
                                            bodyMail.Append($"<br/> <br/>");

                                            bodyMail.Append("مع الشكر");
                                            bodyMail.Append("</body>");
                                            bodyMail.Append("</html>");


                                            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());

                                        }


                                    }




                                    pnlData.Visible = false;
                                    lblSuccessMessage.Text = SPFactory.GetPNUresResource("SavedSuccessfully");
                                    lblSuccessMessage.Visible = true;  // Make the success message visible
                                    lblException.Visible = false;
                                    btnSend.Visible = false;

                                }
                            }
                        }


                    });


                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected void chkRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if at least one checkbox is selected
            bool isValid = false;
            
            foreach (ListItem item in chkRole.Items)
            {
                if (item.Selected)
                {
                    isValid = true;
                    break;
                }
            }

            // Show error message if no checkbox is selected
            if (!isValid)
            {
                lblErrorMessage.Text = "Please select at least one role.";
                lblErrorMessage.Visible = true;  // Display error message
            }
            else
            {
                lblErrorMessage.Visible = false;  // Hide error message if validation passes
            }


            foreach (ListItem item in chkRole.Items)
            {
                if (item.Selected)
                {
                    if(item.Value == "Other")
                    {
                        if(txtOther.Text != "")
                        {
                            isValid = true;
                            break;
                        }
                        else
                        {
                            lblErrorMessage.Text = "Please Specify value for Other";
                            lblErrorMessage.Visible = true;  // Display error message
                            isValid = false;
                        }
                    }
                        
                }
            }

            

        }

        public bool ValidchkRole()
        {
            // Check if at least one checkbox is selected
            bool isValid = false;

            foreach (ListItem item in chkRole.Items)
            {
                if (item.Selected)
                {
                    isValid = true;
                    break;
                }
            }

            // Show error message if no checkbox is selected
            if (!isValid)
            {
                lblErrorMessage.Text = "Please select at least one role.";
                lblErrorMessage.Visible = true;  // Display error message
                return false;
            }
            else
            {
                lblErrorMessage.Visible = false;  
            }


            foreach (ListItem item in chkRole.Items)
            {
                if (item.Selected)
                {
                    if (item.Value == "Other")
                    {
                        if (txtOther.Text != "")
                        {
                            isValid = true;
                            break;
                        }
                        else
                        {
                            lblErrorMessage.Text = "Please Specify value for Other";
                            lblErrorMessage.Visible = true;  // Display error message
                            isValid = false;
                        }
                    }

                }
            }

            return isValid;


        }


    }


    public class OpenDataRequests
    {
        public string Title { get; set; }

        public string Name { get; set; }
        public string Suggests { get; set; }
        public string Email { get; set; }
        public string ExtraOpenDataRequest { get; set; }
        public string Role { get; set; }
        public string Datasets { get; set; }


    }
}
