using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms
{
    public partial class ucDataLeakReport : UserControl
    {
        private const string LIST_NAME = "DataLeakReports";
        private const string ADMIN_LIST_NAME = "DataAdminUsers";

        // Max email attachment total size — files exceeding this won't be attached to the email
        // (they're always saved to SharePoint regardless)
        private const long MAX_EMAIL_ATTACHMENT_BYTES = 10 * 1024 * 1024; // 10 MB

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //cvDate.ValueToCompare = DateTime.Today.ToString("yyyy-MM-dd");
                txtDiscoveryDate.Attributes["max"] = DateTime.Today.ToString("yyyy-MM-dd");
                // Provision both lists on first load if missing
                EnsureListsProvisioned();
            }
        }

        #region List Provisioning

        /// <summary>
        /// Ensures both DataLeakReports and DataAdminUsers lists exist with proper fields and view config.
        /// </summary>
        private void EnsureListsProvisioned()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            EnsureReportsList(web);
                            EnsureAdminUsersList(web);

                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Don't break the page if provisioning fails — log silently
                lblException.Text = "Provisioning warning: " + ex.Message;
            }
        }

        private void EnsureReportsList(SPWeb web)
        {
            SPList list = web.Lists.Cast<SPList>().FirstOrDefault(l => l.Title == LIST_NAME);

            if (list == null)
            {
                Guid listId = web.Lists.Add(LIST_NAME, "بلاغات حوادث تسريب البيانات", SPListTemplateType.GenericList);
                list = web.Lists[listId];

                // Enable attachments (already enabled by default for GenericList, but be explicit)
                list.EnableAttachments = true;

                // Rename Title to "Reference No"
                SPField titleField = list.Fields.GetFieldByInternalName("Title");
                titleField.Title = "رقم البلاغ";
                titleField.Update();

                // Reporter info
                AddTextField(list, "FullName", "الاسم الكامل");
                AddTextField(list, "Mobile", "رقم الجوال");
                AddTextField(list, "Email", "البريد الإلكتروني");
                AddChoiceField(list, "Capacity", "الصفة",
                    new[] { "طالب", "موظف", "موظف سابق", "جهة خارجية", "متعاقد / مزود خدمة", "أخرى" });
                AddTextField(list, "CapacityOther", "الصفة - أخرى");

                // Incident details
                //AddDateField(list, "DiscoveryDate", "تاريخ اكتشاف التسريب");
                AddTextField(list, "DiscoveryDate", "تاريخ اكتشاف التسريب");

                AddChoiceField(list, "StillActive", "هل التسريب ما زال قائماً",
                    new[] { "نعم", "لا", "غير متأكد" });

                // Multi-select stored as semicolon-joined Note
                AddNoteField(list, "SourceType", "نوع مصدر التسريب");
                AddTextField(list, "SourceTypeOther", "نوع المصدر - أخرى");
                AddTextField(list, "SourceName", "اسم مصدر التسريب");

                AddNoteField(list, "DataType", "نوع البيانات المتأثرة");
                AddTextField(list, "DataTypeOther", "نوع البيانات - أخرى");

                AddNoteField(list, "DataFormat", "صيغة البيانات");
                AddTextField(list, "DataFormatOther", "صيغة البيانات - أخرى");

                AddChoiceField(list, "AccessMethod", "طريقة الوصول",
                    new[] { "متاحة للعامة", "محمية بكلمة مرور", "أُرسلت بالخطأ", "تم الحصول عليها دون تصريح", "غير معروف" });

                AddTextField(list, "AffectedCount", "عدد المتأثرين");
                AddNoteField(list, "DataBelongsTo", "البيانات تخص");

                AddNoteField(list, "IncidentDescription", "وصف الحادث");

                // Configure default view
                SPView view = list.DefaultView;
                string[] fields = new[]
                {
                    "FullName", "Email", "Mobile", "Capacity", "DiscoveryDate",
                    "StillActive", "SourceType", "SourceName", "DataType", "DataFormat",
                    "AccessMethod", "AffectedCount", "DataBelongsTo", "IncidentDescription"
                };

                foreach (string f in fields)
                {
                    if (!view.ViewFields.Exists(f))
                        view.ViewFields.Add(f);
                }
                view.Update();
            }
        }

        private void EnsureAdminUsersList(SPWeb web)
        {
            SPList adminList = web.Lists.Cast<SPList>().FirstOrDefault(l => l.Title == ADMIN_LIST_NAME);

            if (adminList == null)
            {
                Guid listId = web.Lists.Add(ADMIN_LIST_NAME, "المستخدمون المصرح لهم بعرض بلاغات تسريب البيانات", SPListTemplateType.GenericList);
                adminList = web.Lists[listId];

                // Rename Title to act as the user login
                SPField titleField = adminList.Fields.GetFieldByInternalName("Title");
                titleField.Title = "اسم المستخدم (Login)";
                titleField.Update();

                // Add a User field for proper picker
                adminList.Fields.Add("AdminUser", SPFieldType.User, true);
                AddTextField(adminList, "DisplayName", "الاسم");
                AddTextField(adminList, "AdminEmail", "البريد الإلكتروني");

                SPView view = adminList.DefaultView;
                if (!view.ViewFields.Exists("AdminUser")) view.ViewFields.Add("AdminUser");
                if (!view.ViewFields.Exists("DisplayName")) view.ViewFields.Add("DisplayName");
                if (!view.ViewFields.Exists("AdminEmail")) view.ViewFields.Add("AdminEmail");
                view.Update();
            }
        }

        #region Field helpers

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

        private void AddDateField(SPList list, string internalName, string displayName)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.DateTime, false);
                SPFieldDateTime f = (SPFieldDateTime)list.Fields.GetFieldByInternalName(internalName);
                f.DisplayFormat = SPDateTimeFieldFormatType.DateOnly;
                f.Title = displayName;
                f.Update();
            }
        }

        private void AddChoiceField(SPList list, string internalName, string displayName, string[] choices)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                StringCollection sc = new StringCollection();
                sc.AddRange(choices);
                list.Fields.Add(internalName, SPFieldType.Choice, false, false, sc);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                f.Update();
            }
        }

        #endregion

        #endregion

        #region Submit

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // Hold the uploaded files in memory once — used for both SharePoint attachments
            // and email attachments. Keeps us from re-reading the same files twice.
            List<UploadedFileBuffer> uploadedFiles = ReadUploadedFiles();

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

                if (dictionary.ContainsKey("success"))
                {
                    if (dictionary["success"].ToString().ToLower() == "true")
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {


                                    web.AllowUnsafeUpdates = true;

                                    SPList list = web.Lists[LIST_NAME];
                                    SPListItem item = list.Items.Add();

                                    // Reporter
                                    item["Title"] = $"DLR-{DateTime.Now:yyyyMMddHHmmss}";
                                    item["FullName"] = txtFullName.Text.Trim();
                                    item["Mobile"] = txtMobile.Text.Trim();
                                    item["Email"] = txtEmail.Text.Trim();
                                    item["Capacity"] = rblCapacity.SelectedValue;
                                    item["CapacityOther"] = rblCapacity.SelectedValue == "أخرى" ? txtCapacityOther.Text.Trim() : "";


                                    //DateTime discoveryDate;
                                    //if (DateTime.TryParseExact(
                                    //        txtDiscoveryDate.Text,
                                    //        new[] { "yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                                    //        System.Globalization.CultureInfo.InvariantCulture,
                                    //        System.Globalization.DateTimeStyles.None,
                                    //        out discoveryDate))
                                    //{
                                    //    item["DiscoveryDate"] = discoveryDate;
                                    //}
                                    //else
                                    //{
                                    //    // Fallback: try invariant culture parse as last resort
                                    //    if (DateTime.TryParse(
                                    //            txtDiscoveryDate.Text,
                                    //            System.Globalization.CultureInfo.InvariantCulture,
                                    //            System.Globalization.DateTimeStyles.None,
                                    //            out discoveryDate))
                                    //    {
                                    //        item["DiscoveryDate"] = discoveryDate;
                                    //    }
                                    //}

                                    item["DiscoveryDate"] = txtDiscoveryDate.Text;

                                    item["StillActive"] = rblStillActive.SelectedValue;

                                    item["SourceType"] = JoinSelected(cblSourceType);
                                    item["SourceTypeOther"] = IsSelected(cblSourceType, "أخرى") ? txtSourceTypeOther.Text.Trim() : "";
                                    item["SourceName"] = txtSourceName.Text.Trim();

                                    item["DataType"] = JoinSelected(cblDataType);
                                    item["DataTypeOther"] = IsSelected(cblDataType, "أخرى") ? txtDataTypeOther.Text.Trim() : "";

                                    item["DataFormat"] = JoinSelected(cblDataFormat);
                                    item["DataFormatOther"] = IsSelected(cblDataFormat, "أخرى") ? txtDataFormatOther.Text.Trim() : "";

                                    item["AccessMethod"] = rblAccessMethod.SelectedValue;
                                    item["AffectedCount"] = txtAffectedCount.Text.Trim();
                                    item["DataBelongsTo"] = JoinSelected(cblDataBelongsTo);
                                    item["IncidentDescription"] = txtIncidentDescription.Text.Trim();

                                    // Attach files to SharePoint item using the pre-read buffers
                                    foreach (var f in uploadedFiles)
                                    {
                                        try
                                        {
                                            item.Attachments.Add(f.FileName, f.Data);
                                        }
                                        catch { /* skip bad file, keep going */ }
                                    }

                                    item.Update();
                                    newItemId = item.ID;

                                    // Force SharePoint to commit and invalidate any list-level cache
                                    list.Update();

                                    web.AllowUnsafeUpdates = false;

                                }
                            }
                        });

                        // Send admin notification email with attachments
                        SendNotificationEmail(newItemId, uploadedFiles);

                        // Switch UI to success state
                        pnlData.Visible = false;
                        pnlSuccess.Visible = true;
                        // lblSuccessMessage.Text = "شكراً لك.. تم تسجيل بلاغك بنجاح";

                    }
                }

                
            }
            catch (Exception ex)
            {
                lblException.Visible = true;
                lblException.Text = "حدث خطأ أثناء حفظ البلاغ: " + ex.Message;
            }
        }

        /// <summary>
        /// Reads all uploaded files into memory once so they can be reused for both
        /// SharePoint attachments and email attachments without re-reading the upload streams.
        /// </summary>
        private List<UploadedFileBuffer> ReadUploadedFiles()
        {
            var result = new List<UploadedFileBuffer>();

            if (!fuAttachment.HasFiles) return result;

            foreach (HttpPostedFile pf in fuAttachment.PostedFiles)
            {
                if (pf == null || pf.ContentLength <= 0) continue;

                try
                {
                    byte[] buffer;
                    using (BinaryReader br = new BinaryReader(pf.InputStream))
                    {
                        buffer = br.ReadBytes(pf.ContentLength);
                    }
                    string safeName = Path.GetFileName(pf.FileName);
                    string contentType = string.IsNullOrWhiteSpace(pf.ContentType) ? "application/octet-stream" : pf.ContentType;

                    result.Add(new UploadedFileBuffer
                    {
                        FileName = safeName,
                        Data = buffer,
                        ContentType = contentType
                    });
                }
                catch { /* skip bad file */ }
            }

            return result;
        }

        private string JoinSelected(CheckBoxList cbl)
        {
            return string.Join("؛ ", cbl.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text));
        }

        private bool IsSelected(CheckBoxList cbl, string text)
        {
            return cbl.Items.Cast<ListItem>().Any(i => i.Selected && i.Text == text);
        }

        #endregion

        #region Email

        private void SendNotificationEmail(int reportId, List<UploadedFileBuffer> uploadedFiles)
        {
            // Email attachments need to stay alive until SendEmail finishes — collect for disposal in finally
            List<Attachment> emailAttachments = null;

            try
            {
                var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "DataLeakAdminEmail" });
                if (smtpSettings == null) return;

                string rawEmails = Convert.ToString(smtpSettings["DataLeakAdminEmail"]);
                if (string.IsNullOrWhiteSpace(rawEmails)) return;

                // Split by ; or , and clean up
                List<string> emailList = rawEmails
                    .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => e.Trim())
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (emailList.Count == 0) return;


                // First recipient goes as "To", rest go as "CC"
                //string toMail = emailList[0];
                //List<string> ccMail = emailList.Skip(1).ToList();



                string subject = $"بلاغ جديد - حوادث تسريب البيانات (#{reportId})";

                StringBuilder body = new StringBuilder();
                body.Append("<html lang='ar'><body style='direction: rtl; font-family: Tajawal, Tahoma, Arial, sans-serif;'>");
                body.Append("<h3 style='color:#007a87;'>تم استلام بلاغ جديد بشأن حوادث تسريب البيانات الشخصية</h3>");
                body.Append($"<p><strong>رقم البلاغ:</strong> {reportId}</p>");
                body.Append("<hr/>");

                body.Append("<h4 style='color:#007a87;'>بيانات مُقدم البلاغ</h4>");
                body.Append($"<p><strong>الاسم الكامل:</strong> {txtFullName.Text}</p>");
                body.Append($"<p><strong>البريد الإلكتروني:</strong> {txtEmail.Text}</p>");
                body.Append($"<p><strong>رقم الجوال:</strong> {txtMobile.Text}</p>");
                string capacity = rblCapacity.SelectedValue == "أخرى"
                    ? $"أخرى: {txtCapacityOther.Text}"
                    : rblCapacity.SelectedValue;
                body.Append($"<p><strong>الصفة:</strong> {capacity}</p>");

                body.Append("<h4 style='color:#007a87;'>تفاصيل الحادثة</h4>");
                body.Append($"<p><strong>تاريخ اكتشاف التسريب:</strong> {txtDiscoveryDate.Text}</p>");
                body.Append($"<p><strong>هل التسريب ما زال قائماً:</strong> {rblStillActive.SelectedValue}</p>");

                string sourceType = JoinSelected(cblSourceType);
                if (IsSelected(cblSourceType, "أخرى") && !string.IsNullOrWhiteSpace(txtSourceTypeOther.Text))
                    sourceType += $" - {txtSourceTypeOther.Text}";
                body.Append($"<p><strong>نوع مصدر التسريب:</strong> {sourceType}</p>");
                body.Append($"<p><strong>اسم المصدر / الرابط:</strong> {txtSourceName.Text}</p>");

                string dataType = JoinSelected(cblDataType);
                if (IsSelected(cblDataType, "أخرى") && !string.IsNullOrWhiteSpace(txtDataTypeOther.Text))
                    dataType += $" - {txtDataTypeOther.Text}";
                body.Append($"<p><strong>نوع البيانات المتأثرة:</strong> {dataType}</p>");

                string dataFormat = JoinSelected(cblDataFormat);
                if (IsSelected(cblDataFormat, "أخرى") && !string.IsNullOrWhiteSpace(txtDataFormatOther.Text))
                    dataFormat += $" - {txtDataFormatOther.Text}";
                body.Append($"<p><strong>صيغة البيانات:</strong> {dataFormat}</p>");

                body.Append($"<p><strong>طريقة الوصول:</strong> {rblAccessMethod.SelectedValue}</p>");
                body.Append($"<p><strong>عدد المتأثرين:</strong> {txtAffectedCount.Text}</p>");
                body.Append($"<p><strong>البيانات تخص:</strong> {JoinSelected(cblDataBelongsTo)}</p>");

                body.Append("<h4 style='color:#007a87;'>وصف الحادث</h4>");
                body.Append($"<p>{txtIncidentDescription.Text.Replace(Environment.NewLine, "<br/>")}</p>");

                // ===== Attachments handling =====
                bool sizeExceeded;
                long totalSize;
                emailAttachments = BuildEmailAttachments(uploadedFiles, out sizeExceeded, out totalSize);

                if (uploadedFiles != null && uploadedFiles.Count > 0)
                {
                    body.Append("<h4 style='color:#007a87;'>المرفقات</h4>");

                    if (sizeExceeded)
                    {
                        // Files too large to attach — show file list with note
                        body.Append("<p style='color:#d9534f;'><em>");
                        body.Append($"الحجم الإجمالي للمرفقات ({FormatFileSize(totalSize)}) يتجاوز الحد المسموح به للبريد. ");
                        body.Append("يمكن مراجعة المرفقات من خلال النظام.");
                        body.Append("</em></p>");
                    }

                    body.Append("<ul>");
                    foreach (var f in uploadedFiles)
                    {
                        body.Append($"<li>{HttpUtility.HtmlEncode(f.FileName)} ({FormatFileSize(f.Data.LongLength)})</li>");
                    }
                    body.Append("</ul>");
                }

                body.Append("<br/><p>مع التحية،</p>");
                body.Append("<p style='color:#888;font-size:12px;'>هذه رسالة آلية من نظام الإبلاغ عن حوادث تسريب البيانات</p>");
                body.Append("</body></html>");

                //List<string> ccMail = new List<string>();
                //EmailUtility.SendEmail(toMail, ccMail, subject, body.ToString());

                // Send email — attachments parameter is null if size exceeded or no files
                EmailUtility.SendEmail(emailList, new List<string>(), subject, body.ToString(), emailAttachments);

            }
            catch (Exception ex)
            {
                // Don't fail the form submission if email fails — but log it
                lblException.Text = "ملاحظة: تم حفظ البلاغ ولكن فشل إرسال الإشعار. " + ex.Message;
                lblException.Visible = true;
            }
            finally
            {
                // Dispose attachment streams after sending
                if (emailAttachments != null)
                {
                    foreach (var att in emailAttachments)
                    {
                        try { if (att != null) att.Dispose(); } catch { }
                    }
                }
            }
        }

        /// <summary>
        /// Builds a list of System.Net.Mail.Attachment objects from the in-memory file buffers.
        /// Returns null if total size exceeds the email limit or no files were uploaded.
        /// </summary>
        private List<Attachment> BuildEmailAttachments(
            List<UploadedFileBuffer> uploadedFiles,
            out bool sizeExceeded,
            out long totalSize)
        {
            sizeExceeded = false;
            totalSize = 0;

            if (uploadedFiles == null || uploadedFiles.Count == 0)
                return null;

            totalSize = uploadedFiles.Sum(f => f.Data != null ? f.Data.LongLength : 0L);

            if (totalSize > MAX_EMAIL_ATTACHMENT_BYTES)
            {
                sizeExceeded = true;
                return null;
            }

            var attachments = new List<Attachment>();
            foreach (var f in uploadedFiles)
            {
                try
                {
                    // MemoryStream stays alive until the Attachment is disposed (in finally block of caller)
                    var ms = new MemoryStream(f.Data);
                    var contentType = new System.Net.Mime.ContentType(string.IsNullOrWhiteSpace(f.ContentType) ? "application/octet-stream" : f.ContentType);
                    var att = new Attachment(ms, contentType)
                    {
                        Name = f.FileName,
                        NameEncoding = Encoding.UTF8
                    };
                    attachments.Add(att);
                }
                catch { /* skip bad file */ }
            }

            return attachments;
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:0.#} KB";
            return $"{bytes / (1024.0 * 1024.0):0.##} MB";
        }

        #endregion

        #region Helper Class

        /// <summary>
        /// In-memory representation of an uploaded file — used to pass file data to both
        /// SharePoint attachments and email attachments without re-reading the upload stream.
        /// </summary>
        private class UploadedFileBuffer
        {
            public string FileName { get; set; }
            public byte[] Data { get; set; }
            public string ContentType { get; set; }
        }

        #endregion
    }
}