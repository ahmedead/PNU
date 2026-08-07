using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Org.BouncyCastle.Ocsp;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Controls.Common;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter
{
    public partial class ucAIContactUS : UserControl
    {

        
        protected void Page_Load(object sender, EventArgs e)
        {
           //btnSend.Visible = false;
        }

        protected void btnSend_Click(object sender, EventArgs e)
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
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["ContactUs"];
                            if (list != null)
                            {



                                // Check if a similar record exists
                                SPQuery query = new SPQuery();
                                query.Query = "<Where>" +
                                              "<And>" +
                                              "<And>" +
                                              "<Eq><FieldRef Name='FirstName' /><Value Type='Text'>" + txtFirstName.Text + "</Value></Eq>" +
                                              "<Eq><FieldRef Name='LastName' /><Value Type='Text'>" + txtLastName.Text + "</Value></Eq>" +
                                              "</And>" +
                                              "<And>" +
                                              "<Eq><FieldRef Name='Email' /><Value Type='Text'>" + txtEmail.Text + "</Value></Eq>" +
                                              "<Eq><FieldRef Name='PhoneNo' /><Value Type='Text'>" + txtMobileNo.Text + "</Value></Eq>" +
                                              "</And>" +
                                              "</And>" +
                                              "</Where>";


                                SPListItemCollection existingItems = list.GetItems(query);

                                if (existingItems.Count > 0)
                                {
                                    lblException.Text = "This data has already been submitted.";
                                    lblException.Visible = true;

                                    lblSuccessMessage.Visible = false;  // Make the success message visible
                                    btnSend.Visible = false;
                                    return;
                                }
                                AIContactUS contactUS = new AIContactUS();
                                contactUS.FirstName = txtFirstName.Text;
                                contactUS.LastName = txtLastName.Text;
                                contactUS.Email = txtEmail.Text;
                                contactUS.PhoneNo = txtMobileNo.Text;
                                contactUS.Messege = txtMessege.Text;
                                SPListItem listItem = list.Items.Add();
                                listItem = SPFactory.MapClassToSPListItem(listItem, contactUS, true);



                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;


                                var toMail = "ccis-aic@pnu.edu.sa";
                                //var toMail = "aesharaf@pnu.edu.sa";

                                List<string> ccMail = new List<string>();
                                //ccMail.Add("aesharaf@pnu.edu.sa");


                                var subjectMail = $"مركز الذكاء الإصطناعي - اتصل بنا";
                                StringBuilder bodyMail = new StringBuilder();
                                bodyMail.Append("<html>");
                                bodyMail.Append("<body>");
                                bodyMail.Append($"<p> تم استلام طلب جديد </p><br/>مرسل الطلب {contactUS.FirstName + " " + contactUS.LastName} ");
                                bodyMail.Append($"<br/> الإيميل : {contactUS.Email} <br/>");
                                bodyMail.Append($"<br/>رقم الجوال :  {contactUS.PhoneNo} <br/>");
                                bodyMail.Append($"<br/>الرسالة :  {contactUS.Messege} <br/>");
                                bodyMail.Append($"<br/> <br/>");

                                bodyMail.Append("مع الشكر");
                                bodyMail.Append("</body>");
                                bodyMail.Append("</html>");


                                EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());

                            }
                        }
                    }


                });

                lblSuccessMessage.Text = SPFactory.GetPNUresResource("SavedSuccessfully");
                lblSuccessMessage.Visible = true;  // Make the success message visible
                lblException.Visible = false;
                btnSend.Visible = false;
                txtFirstName.Text = "";
                txtEmail.Text = "";
                txtLastName.Text = "";
                txtMessege.Text = "";
                txtMobileNo.Text = "";
                

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

    }

    public class AIContactUS
    {
        public string Title { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Messege { get; set; }


    }

}
