using Microsoft.SharePoint;
using Portal.Main.Helper;
using QRCoder;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EventsWithQRCode
{
    public partial class ucRegister : UserControl
    {
        public string ListName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ListName)
                || string.IsNullOrEmpty(ListName.Trim()))
            {
                ListName = "CareerCarnival";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate {
                try
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            if (web.AllowUnsafeUpdates == false)
                                web.AllowUnsafeUpdates = true;

                            SPList requestsList = web.Lists.TryGetList(ListName);

                            if (requestsList == null)
                            {
                                // إنشاء القائمة إذا لم تكن موجودة
                                Guid listId = web.Lists.Add(ListName, "قائمة تسجيل كرنفال المهنة", SPListTemplateType.GenericList);
                                requestsList = web.Lists[listId];

                                // 2. إضافة الأعمدة المطلوبة
                                string[] fields = { "FullName", "StudentID", "Email", "Mobile", "College", "Major", "AcademicStatus" };
                                foreach (string field in fields)
                                {
                                    requestsList.Fields.Add(field, SPFieldType.Text, false);
                                }

                                // إضافة حقول الحالة (Boolean)
                                requestsList.Fields.Add("EmailSent", SPFieldType.Boolean, false);
                                requestsList.Fields.Add("QRGenerated", SPFieldType.Boolean, false);

                                // 3. إضافة الأعمدة للعرض الافتراضي (Default View)
                                SPView defaultView = requestsList.DefaultView;
                                foreach (string field in fields)
                                {
                                    if (!defaultView.ViewFields.Exists(field))
                                        defaultView.ViewFields.Add(field);
                                }
                                defaultView.ViewFields.Add("EmailSent");
                                defaultView.ViewFields.Add("QRGenerated");
                                defaultView.Update();

                                requestsList.Update();
                            }
                            SPListItem item = requestsList.Items.Add();

                            // تحديث القيم من عناصر ASP.NET الجديدة
                            item["FullName"] = this.txtFullName.Text.Trim();
                            item["Title"] = this.txtFullName.Text.Trim();
                            item["StudentID"] = this.txtIDNumber.Text.Trim();
                            item["Email"] = this.txtEmail.Text.Trim().ToLower();
                            item["Mobile"] = this.txtPhone.Text.Trim();

                            // الحقول الإضافية التي تم إنشاؤها في الواجهة
                            item["College"] = this.txtCollege.Text.Trim();
                            item["Major"] = this.txtMajor.Text.Trim();
                            item["AcademicStatus"] = this.ddlStatus.SelectedValue;

                            // تحديث الواجهة لإظهار رسالة النجاح
                            this.lblName.Text = this.txtFullName.Text;
                            this.DivSucWrap.Visible = true;
                            this.divSuccess.Visible = true;
                            this.DivRegister.Visible = false;

                            item.Update();

                            // توليد الـ QR Code
                            string LinkURL = SPContext.Current.Web.Url + "/Pages/AttendStudent.aspx?ItemId=" + item.ID.ToString();
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(LinkURL, QRCodeGenerator.ECCLevel.Q);
                            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);

                            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                            {
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    byte[] ImgBytes = stream.ToArray();

                                    // إضافة الـ QR كمرفق للقائمة
                                    SPAttachmentCollection attachments = item.Attachments;
                                    attachments.Add("QRCode.PNG", ImgBytes);
                                    item.Update();

                                    // الحصول على رابط الصورة لعرضها في الواجهة
                                    SPFile file = web.GetFile(attachments.UrlPrefix + attachments[0]);
                                    this.ImageGeneratedBarcode.Visible = true;
                                    this.ImageGeneratedBarcode.ImageUrl = web.Url + "/" + file.Url;

                                    // إرسال البريد الإلكتروني
                                    try
                                    {
                                        Hashtable values = new Hashtable();
                                        values.Add("{ImageBase64}", web.Url + "/" + file.Url);

                                        PortalHelper obj = new PortalHelper();
                                        var toMail = this.txtEmail.Text;

                                        bool x = obj.SendMailNewJoiners("CommonDesignTemplate", values, "حفل التميز 1446 هـ", toMail, "").Result;

                                        item["EmailSent"] = true;
                                        item["QRGenerated"] = true;
                                        item.Update();
                                    }
                                    catch (Exception ex)
                                    {
                                        this.divAlerts.Visible = true;
                                        this.lblerr.Text = "خطأ في إرسال البريد: " + ex.Message;
                                    }
                                }
                            }

                            if (web.AllowUnsafeUpdates == true)
                                web.AllowUnsafeUpdates = false;

                            this.lblID.Text = item.ID.ToString();
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.divAlerts.Visible = true;
                    this.lblerr.Text = exception.Message;
                    this.DivSucWrap.Visible = false;
                    this.divSuccess.Visible = false;
                }
            });

        }
    }
}
