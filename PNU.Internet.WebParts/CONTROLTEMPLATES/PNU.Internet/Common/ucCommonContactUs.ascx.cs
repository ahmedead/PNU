using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common
{
    public partial class ucCommonContactUs : UserControl
    {
        private const string LIST_NAME = "pContactUs";
        private const string CATEGORY_LIST_NAME = "pContactUsCategory";

        public bool IsArabic
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                    {
                        string url = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                        if (url.Contains("/en/") || url.EndsWith("/en")) return false;
                    }
                    if (SPContext.Current != null && SPContext.Current.Web != null)
                    {
                        return SPContext.Current.Web.Language == 1025;
                    }
                    return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower() != "en";
                }
                catch { return true; }
            }
        }

        public string Pick(string ar, string en)
        {
            return IsArabic ? ar : en;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnsureListsProvisioned();
                LoadCategories();
            }

            SetupLocalizedTexts();
        }

        private void SetupLocalizedTexts()
        {
            divWrapper.Attributes["dir"] = IsArabic ? "rtl" : "ltr";

            lblFullName.Text = Pick("الاسم الكامل <span class=\"text-danger\">*</span>", "Full Name <span class=\"text-danger\">*</span>");
            lblEmail.Text = Pick("البريد الإلكتروني <span class=\"text-danger\">*</span>", "Email Address <span class=\"text-danger\">*</span>");
            lblInquiryCategory.Text = Pick("تصنيف الاستفسار <span class=\"text-danger\">*</span>", "Inquiry Category <span class=\"text-danger\">*</span>");
            lblApplicationNumber.Text = Pick("رقم الطلب (اختياري)", "Application Number (Optional)");
            lblMessage.Text = Pick("كيف يمكننا مساعدتك؟ <span class=\"text-danger\">*</span>", "How can we help you? <span class=\"text-danger\">*</span>");

            txtFullName.Attributes["placeholder"] = Pick("اكتب الاسم الكامل", "Enter full name");
            txtEmail.Attributes["placeholder"] = "name@example.com";
            txtApplicationNumber.Attributes["placeholder"] = Pick("مثال: 123-456-789", "e.g. 123-456-789");
            txtMessage.Attributes["placeholder"] = Pick("اكتب رسالتك هنا", "Type your message here");

            rfvFullName.ErrorMessage = Pick("مطلوب", "Required");
            rfvEmail.ErrorMessage = Pick("مطلوب", "Required");
            revEmail.ErrorMessage = Pick("بريد إلكتروني غير صحيح", "Invalid email address");
            rfvInquiryCategory.ErrorMessage = Pick("مطلوب اختيار التصنيف", "Please select a category");
            rfvMessage.ErrorMessage = Pick("مطلوب", "Required");

            btnSubmit.Text = Pick("إرسال الطلب", "Submit Request");

            ltrSideTitle.Text = Pick("تواصل نورة", "Tawasul Nourah");
            ltrPlatformTitle.Text = Pick("رابط المنصة", "Platform Link");
            ltrTawasulPhoneTitle.Text = Pick("هاتف تواصل نورة", "Tawasul Nourah Phone");
            ltrTawasulEmailTitle.Text = Pick("بريد تواصل نورة", "Tawasul Nourah Email");
            ltrUnivPhoneTitle.Text = Pick("هاتف الجامعة", "University Phone");
            ltrUnivEmailTitle.Text = Pick("بريد الجامعة", "University Email");
            ltrServicesTitle.Text = Pick("الخدمات المتاحة", "Available Services");
            ltrServicesDesc.Text = Pick("حجز المواعيد، ورفع الاستفسارات والشكاوى والمقترحات، ومتابعة الطلبات مع الجهات المعنية في الجامعة.", "Booking appointments, submitting inquiries, complaints, and suggestions, and tracking requests with relevant university departments.");

            ltrSuccessTitle.Text = Pick("شكراً لك..", "Thank You..");
            ltrSuccessDesc.Text = Pick("تم تسجيل طلبك بنجاح", "Your request has been submitted successfully.");
        }

        #region List Provisioning & Category Loading

        /// <summary>
        /// Ensures both pContactUs and pContactUsCategory lists exist under RootWeb.
        /// </summary>
        private void EnsureListsProvisioned()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb rootWeb = site.RootWeb)
                        {
                            rootWeb.AllowUnsafeUpdates = true;

                            EnsureCategoryList(rootWeb);
                            EnsureContactUsList(rootWeb);

                            rootWeb.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                lblException.Text = "Provisioning warning: " + ex.Message;
            }
        }

        private void EnsureCategoryList(SPWeb rootWeb)
        {
            SPList list = rootWeb.Lists.Cast<SPList>().FirstOrDefault(l => l.Title == CATEGORY_LIST_NAME);

            if (list == null)
            {
                Guid listId = rootWeb.Lists.Add(CATEGORY_LIST_NAME, "تصنيفات تواصل معنا", SPListTemplateType.GenericList);
                list = rootWeb.Lists[listId];

                SPField titleField = list.Fields.GetFieldByInternalName("Title");
                titleField.Title = "التصنيف";
                titleField.Update();
            }

            AddTextField(list, "TitleEn", "Category (English)");

            if (list.ItemCount == 0)
            {
                var defaultCategories = new[]
                {
                    new { Ar = "استفسار عام", En = "General Inquiry" },
                    new { Ar = "الخدمات الإلكترونية", En = "E-Services" },
                    new { Ar = "القبول والتسجيل", En = "Admission & Registration" },
                    new { Ar = "أخرى", En = "Other" }
                };

                foreach (var cat in defaultCategories)
                {
                    SPListItem item = list.Items.Add();
                    item["Title"] = cat.Ar;
                    item["TitleEn"] = cat.En;
                    item.Update();
                }

                list.Update();
            }
        }

        private void EnsureContactUsList(SPWeb rootWeb)
        {
            SPList list = rootWeb.Lists.Cast<SPList>().FirstOrDefault(l => l.Title == LIST_NAME);

            if (list == null)
            {
                Guid listId = rootWeb.Lists.Add(LIST_NAME, "طلبات تواصل معنا", SPListTemplateType.GenericList);
                list = rootWeb.Lists[listId];

                SPField titleField = list.Fields.GetFieldByInternalName("Title");
                titleField.Title = "رقم الطلب";
                titleField.Update();

                AddTextField(list, "FullName", "الاسم الكامل");
                AddTextField(list, "Email", "البريد الإلكتروني");
                AddTextField(list, "InquiryCategory", "تصنيف الاستفسار");
                AddTextField(list, "ApplicationNumber", "رقم الطلب (إن وجد)");
                AddNoteField(list, "Message", "نص الرسالة");

                SPView view = list.DefaultView;
                string[] fields = new string[] { "FullName", "Email", "InquiryCategory", "ApplicationNumber", "Message" };
                foreach (string f in fields)
                {
                    if (!view.ViewFields.Exists(f))
                        view.ViewFields.Add(f);
                }
                view.Update();
            }
        }

        private void AddTextField(SPList list, string internalName, string displayName)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Text, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                f.Update();
            }
        }

        private void AddNoteField(SPList list, string internalName, string displayName)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Note, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                f.Update();
            }
        }

        private void LoadCategories()
        {
            try
            {
                ddlInquiryCategory.Items.Clear();
                string defaultText = Pick("اختر التصنيف", "Select Category");
                ddlInquiryCategory.Items.Add(new ListItem(defaultText, "0"));

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb rootWeb = site.RootWeb)
                        {
                            SPList categoryList = rootWeb.Lists.Cast<SPList>().FirstOrDefault(l => l.Title == CATEGORY_LIST_NAME);
                            if (categoryList != null)
                            {
                                bool hasTitleEn = categoryList.Fields.ContainsField("TitleEn");
                                SPListItemCollection items = categoryList.GetItems();
                                foreach (SPListItem item in items)
                                {
                                    string titleAr = Convert.ToString(item["Title"]);
                                    string titleEn = hasTitleEn ? Convert.ToString(item["TitleEn"]) : "";

                                    string displayTitle = IsArabic
                                        ? (!string.IsNullOrWhiteSpace(titleAr) ? titleAr : titleEn)
                                        : (!string.IsNullOrWhiteSpace(titleEn) ? titleEn : titleAr);

                                    string valueStr = !string.IsNullOrWhiteSpace(titleAr) ? titleAr : displayTitle;

                                    if (!string.IsNullOrWhiteSpace(displayTitle))
                                    {
                                        ddlInquiryCategory.Items.Add(new ListItem(displayTitle, valueStr));
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                lblException.Visible = true;
                lblException.Text = Pick("حدث خطأ أثناء تحميل التصنيفات: ", "An error occurred while loading categories: ") + ex.Message;
            }
        }

        #endregion

        #region Submit Event

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int newItemId = 0;
                string PrivateKey = "6Leg0vAsAAAAAKpQnA7rq6ZE97EkXRVhSiI45iLj";
                string EncodedResponse = Request.Form["g-Recaptcha-Response"];

                var client = new System.Net.WebClient();
                var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));
                var serializer = new JavaScriptSerializer();

                dynamic j = serializer.Deserialize<dynamic>(GoogleReply.ToString());
                var dictionary = j as Dictionary<string, object>;

                if (dictionary != null && dictionary.ContainsKey("success"))
                {
                    if (dictionary["success"].ToString().ToLower() == "true")
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                            {
                                using (SPWeb rootWeb = site.RootWeb)
                                {
                                    rootWeb.AllowUnsafeUpdates = true;

                                    SPList list = rootWeb.Lists[LIST_NAME];
                                    SPListItem item = list.Items.Add();

                                    item["Title"] = $"CCU-{DateTime.Now:yyyyMMddHHmmss}";
                                    item["FullName"] = txtFullName.Text.Trim();
                                    item["Email"] = txtEmail.Text.Trim();
                                    item["InquiryCategory"] = ddlInquiryCategory.SelectedValue;
                                    item["ApplicationNumber"] = txtApplicationNumber.Text.Trim();
                                    item["Message"] = txtMessage.Text.Trim();

                                    item.Update();
                                    newItemId = item.ID;

                                    list.Update();
                                    rootWeb.AllowUnsafeUpdates = false;
                                }
                            }
                        });

                        SendNotificationEmail(newItemId);

                        pnlData.Visible = false;
                        pnlSuccess.Visible = true;
                    }
                    else
                    {
                        lblException.Visible = true;
                        lblException.Text = Pick(
                            "رمز التحقق (CAPTCHA) غير صحيح، يرجى المحاولة مرة أخرى.",
                            "Invalid CAPTCHA verification, please try again.");
                    }
                }
                else
                {
                    lblException.Visible = true;
                    lblException.Text = Pick(
                        "فشل التحقق من رمز (CAPTCHA).",
                        "CAPTCHA verification failed.");
                }
            }
            catch (Exception ex)
            {
                lblException.Visible = true;
                lblException.Text = Pick("حدث خطأ أثناء حفظ الطلب: ", "An error occurred while saving your request: ") + ex.Message;
            }
        }

        #endregion

        #region Email Notification

        private void SendNotificationEmail(int requestId)
        {
            try
            {
                List<string> emailList = new List<string>();
                var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "ContactUsAdminEmail" });
                if (smtpSettings != null && smtpSettings.ContainsKey("ContactUsAdminEmail"))
                {
                    string rawEmails = Convert.ToString(smtpSettings["ContactUsAdminEmail"]);
                    if (!string.IsNullOrWhiteSpace(rawEmails))
                    {
                        emailList = rawEmails
                            .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(e => e.Trim())
                            .Where(e => !string.IsNullOrWhiteSpace(e))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();
                    }
                }

                if (emailList.Count == 0)
                {
                    emailList.Add("pnu-tawasul@pnu.edu.sa");
                    emailList.Add("info@pnu.edu.sa");
                }

                string subject = Pick($"طلب تواصل جديد - (#{requestId})", $"New Contact Request - (#{requestId})");
                string dir = IsArabic ? "rtl" : "ltr";
                string font = IsArabic ? "Tajawal, Tahoma, Arial, sans-serif" : "Roboto, Arial, sans-serif";

                StringBuilder body = new StringBuilder();
                body.Append($"<html lang='{(IsArabic ? "ar" : "en")}'><body style='direction: {dir}; font-family: {font};'>");
                body.Append($"<h3 style='color:#007a87;'>{Pick("تم استلام طلب تواصل جديد", "A new contact request has been received")}</h3>");
                body.Append($"<p><strong>{Pick("رقم الطلب", "Request ID")}:</strong> {requestId}</p>");
                body.Append("<hr/>");

                body.Append($"<h4 style='color:#007a87;'>{Pick("بيانات مقدم الطلب", "Applicant Information")}</h4>");
                body.Append($"<p><strong>{Pick("الاسم الكامل", "Full Name")}:</strong> {HttpUtility.HtmlEncode(txtFullName.Text)}</p>");
                body.Append($"<p><strong>{Pick("البريد الإلكتروني", "Email Address")}:</strong> {HttpUtility.HtmlEncode(txtEmail.Text)}</p>");
                body.Append($"<p><strong>{Pick("تصنيف الاستفسار", "Inquiry Category")}:</strong> {HttpUtility.HtmlEncode(ddlInquiryCategory.SelectedValue)}</p>");
                body.Append($"<p><strong>{Pick("رقم الطلب (اختياري)", "Application Number (Optional)")}:</strong> {HttpUtility.HtmlEncode(txtApplicationNumber.Text)}</p>");

                body.Append($"<h4 style='color:#007a87;'>{Pick("نص الرسالة", "Message Text")}</h4>");
                body.Append($"<p>{HttpUtility.HtmlEncode(txtMessage.Text).Replace(Environment.NewLine, "<br/>")}</p>");

                body.Append($"<br/><p>{Pick("مع التحية،", "Regards,")}</p>");
                body.Append($"<p style='color:#888;font-size:12px;'>{Pick("هذه رسالة آلية من نظام تواصل معنا", "This is an automated email from the Contact Us system.")}</p>");
                body.Append("</body></html>");

                EmailUtility.SendEmail(emailList, new List<string>(), subject, body.ToString(), null);
            }
            catch (Exception ex)
            {
                lblException.Text = Pick("ملاحظة: تم حفظ الطلب ولكن فشل إرسال الإشعار. ", "Note: Request saved, but failed to send email notification. ") + ex.Message;
                lblException.Visible = true;
            }
        }

        #endregion
    }
}
