using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts
{
    public static class EmailUtility
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="toEmail">To email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        /// 

        public static async Task<bool> SendEmail(
    List<string> toList,
    List<string> CCList,
    string subject,
    string body,
    List<Attachment> attachments)
        {
            bool retVal = false;
            try
            {
                var smtpSettings = PortalHelper.GetSPSettingList(new string[]
                {
            "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated",
            "SmtpPort", "SmtpEnableSSL", "SendUsingSP", "BCCEmail"
                });

                if (toList == null || toList.Count == 0) return false;

                toList = toList
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Select(e => e.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (toList.Count == 0) return false;

                if (smtpSettings != null)
                {
                    if (Convert.ToBoolean(smtpSettings["SendUsingSP"]) == false)
                    {
                        // ===== System.Net.Mail path - SUPPORTS attachments =====
                        var mailMsg = new MailMessage();
                        mailMsg.Subject = subject;
                        mailMsg.From = new MailAddress(smtpSettings["SmtpUserName"].ToString(), "PNU - InternetWebSite");
                        mailMsg.IsBodyHtml = true;
                        mailMsg.Body = body;

                        foreach (string toEmail in toList)
                            mailMsg.To.Add(new MailAddress(toEmail, string.Empty));

                        if (CCList != null)
                        {
                            foreach (string Mail in CCList)
                            {
                                if (!string.IsNullOrWhiteSpace(Mail))
                                    mailMsg.CC.Add(new MailAddress(Mail.Trim()));
                            }
                        }

                        if (!string.IsNullOrEmpty(smtpSettings["BCCEmail"].ToString()))
                        {
                            mailMsg.Bcc.Add(new MailAddress(smtpSettings["BCCEmail"].ToString()));
                        }

                        // Add attachments
                        if (attachments != null && attachments.Count > 0)
                        {
                            foreach (var att in attachments)
                            {
                                if (att != null)
                                    mailMsg.Attachments.Add(att);
                            }
                        }

                        bool sent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);

                        // Dispose attachments after sending (releases file handles)
                        if (attachments != null)
                        {
                            foreach (var att in attachments)
                            {
                                try { att.Dispose(); } catch { }
                            }
                        }

                        return sent;
                    }
                    else
                    {
                        // ===== SPUtility.SendEmail path - DOES NOT support attachments =====
                        // SPUtility.SendEmail is a simple text wrapper around SharePoint's mail service
                        // and cannot include attachments. Fall back to attaching link references instead.

                        var sender = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                        StringDictionary headers = new StringDictionary();
                        headers.Add("to", string.Join(",", toList));

                        if (CCList != null && CCList.Count > 0)
                        {
                            string cclist = string.Join(",", CCList.Where(c => !string.IsNullOrWhiteSpace(c)));
                            if (!string.IsNullOrWhiteSpace(cclist))
                                headers.Add("cc", cclist);
                        }

                        if (!string.IsNullOrEmpty(smtpSettings["BCCEmail"].ToString()))
                            headers.Add("bcc", smtpSettings["BCCEmail"].ToString());

                        headers.Add("from", sender);
                        headers.Add("subject", subject);
                        headers.Add("content-type", "text/html");

                        var tcs = new TaskCompletionSource<bool>();
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            try
                            {
                                SPUtility.SendEmail(SPContext.Current.Web, headers, body.ToString());
                                tcs.SetResult(true);
                            }
                            catch (Exception ex)
                            {
                                tcs.SetException(ex);
                            }
                        });

                        retVal = await tcs.Task;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        // Keep backward-compatible overload (no attachments)
        public static async Task<bool> SendEmail(List<string> toList, List<string> CCList, string subject, string body)
        {
            return await SendEmail(toList, CCList, subject, body, null);
        }

        public static async Task<bool> SendEmail(string to, List<string> CCList, string subject, string body)
        {
            bool retVal = false;
            try
            {

                var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL","SendUsingSP","BCCEmail" });

                bool sent = false;
                if (smtpSettings != null)
                {
                    if (Convert.ToBoolean(smtpSettings["SendUsingSP"]) == false)
                    {
                        // Forming the mail message
                        var mailMsg = new MailMessage();
                        mailMsg.Subject = subject;
                        mailMsg.From = new MailAddress(smtpSettings["SmtpUserName"].ToString(), "PNU - InternetWebSite");


                        mailMsg.To.Add(new MailAddress(to, string.Empty));
                        //mailMsg.To.Add(new MailAddress("asharaf@lcgpa.gov.sa", string.Empty));
                        if (CCList != null)
                        {

                            foreach (string Mail in CCList)
                            {
                                mailMsg.CC.Add(new MailAddress(Mail));
                            }
                        }
                        mailMsg.Body = body;
                        if (!string.IsNullOrEmpty(smtpSettings["BCCEmail"].ToString()))   
                        {
                            mailMsg.Bcc.Add(new MailAddress(smtpSettings["BCCEmail"].ToString()));

                        }
                        
                        sent = PortalHelper.instance.SendMail(mailMsg, smtpSettings); ;
                        return sent;

                    }
                    else
                    {
                        var sender = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                        StringDictionary headers = new StringDictionary();
                        if (to != string.Empty)
                        {
                            headers.Add("to", to);
                            if (CCList.Count > 0)
                            {
                                string cclist = string.Join(",", CCList);
                                headers.Add("cc", cclist);
                            }

                            if (!string.IsNullOrEmpty(smtpSettings["BCCEmail"].ToString()))
                            {
                                headers.Add("bcc", smtpSettings["BCCEmail"].ToString());

                            }

                            //headers.Add("bcc", "aesharaf@pnu.edu.sa");

                            headers.Add("from", sender);
                            headers.Add("subject", subject);
                            headers.Add("content-type", "text/html");



                            var tcs = new TaskCompletionSource<bool>();
                            SPSecurity.RunWithElevatedPrivileges(delegate ()
                            {
                                try
                                {
                                    // Send email synchronously within the elevated privileges delegate
                                    SPUtility.SendEmail(SPContext.Current.Web, headers, body.ToString());
                                    tcs.SetResult(true); // Set the result to true if email sent successfully
                                }
                                catch (Exception ex)
                                {
                                    tcs.SetException(ex); // Set the exception if email sending fails
                                }
                            });

                            // Await the Task created by TaskCompletionSource
                            retVal = await tcs.Task;
                        }
                    }
                }

                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }


        //public static async Task<bool> SendEmail(List<string> toList, List<string> CCList, string subject, string body)
        //{
        //    bool retVal = false;
        //    try
        //    {
        //        var smtpSettings = PortalHelper.GetSPSettingList(new string[]
        //        {
        //    "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated",
        //    "SmtpPort", "SmtpEnableSSL", "SendUsingSP", "BCCEmail"
        //        });

        //        bool sent = false;

        //        // Validate that we have at least one recipient
        //        if (toList == null || toList.Count == 0)
        //            return false;

        //        // Clean and deduplicate the To list
        //        toList = toList
        //            .Where(e => !string.IsNullOrWhiteSpace(e))
        //            .Select(e => e.Trim())
        //            .Distinct(StringComparer.OrdinalIgnoreCase)
        //            .ToList();

        //        if (toList.Count == 0)
        //            return false;

        //        if (smtpSettings != null)
        //        {
        //            if (Convert.ToBoolean(smtpSettings["SendUsingSP"]) == false)
        //            {
        //                // ===== Path 1: Using System.Net.Mail =====
        //                var mailMsg = new MailMessage();
        //                mailMsg.Subject = subject;
        //                mailMsg.From = new MailAddress(smtpSettings["SmtpUserName"].ToString(), "PNU - InternetWebSite");

        //                // Add all To recipients
        //                foreach (string toEmail in toList)
        //                {
        //                    mailMsg.To.Add(new MailAddress(toEmail, string.Empty));
        //                }

        //                if (CCList != null)
        //                {
        //                    foreach (string Mail in CCList)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(Mail))
        //                            mailMsg.CC.Add(new MailAddress(Mail.Trim()));
        //                    }
        //                }

        //                mailMsg.Body = body;

        //                if (!string.IsNullOrEmpty(smtpSettings["BCCEmail"].ToString()))
        //                {
        //                    mailMsg.Bcc.Add(new MailAddress(smtpSettings["BCCEmail"].ToString()));
        //                }

        //                sent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
        //                return sent;
        //            }
        //            else
        //            {
        //                // ===== Path 2: Using SPUtility.SendEmail =====
        //                var sender = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
        //                StringDictionary headers = new StringDictionary();

        //                // Join all To recipients with comma
        //                string toListString = string.Join(",", toList);
        //                headers.Add("to", toListString);

        //                if (CCList != null && CCList.Count > 0)
        //                {
        //                    string cclist = string.Join(",", CCList.Where(c => !string.IsNullOrWhiteSpace(c)));
        //                    if (!string.IsNullOrWhiteSpace(cclist))
        //                        headers.Add("cc", cclist);
        //                }

        //                if (!string.IsNullOrEmpty(smtpSettings["BCCEmail"].ToString()))
        //                {
        //                    headers.Add("bcc", smtpSettings["BCCEmail"].ToString());
        //                }

        //                headers.Add("from", sender);
        //                headers.Add("subject", subject);
        //                headers.Add("content-type", "text/html");

        //                var tcs = new TaskCompletionSource<bool>();
        //                SPSecurity.RunWithElevatedPrivileges(delegate ()
        //                {
        //                    try
        //                    {
        //                        SPUtility.SendEmail(SPContext.Current.Web, headers, body.ToString());
        //                        tcs.SetResult(true);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        tcs.SetException(ex);
        //                    }
        //                });

        //                retVal = await tcs.Task;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retVal;
        //}
        
        public static async Task<bool> SendEmailToGroupAsync(string groupName, string cc, string subject, string body)
        {
            try
            {
                bool success = false;
                var sender = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                MailMessage Message = new MailMessage();
                Message.Subject = subject;
                Message.Body = body;
                Message.From = new MailAddress(sender);

                if (cc != "")
                    Message.CC.Add(cc);
                StringDictionary headers = new StringDictionary();

                if (groupName != string.Empty)
                {
                    List<string> ToList = new List<string>();
                    

                    var tcs = new TaskCompletionSource<bool>();
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        try
                        {
                            SPGroup group = SPContext.Current.Site.RootWeb.Groups[groupName];
                            foreach (SPUser user in group.Users)
                            {
                                if (user.Email != "")
                                    ToList.Add(user.Email);
                            }
                            headers.Add("to", string.Join(",", ToList));
                            headers.Add("cc", cc);
                            //headers.Add("bcc", "");
                            headers.Add("from", sender);
                            headers.Add("subject", subject);
                            headers.Add("content-type", "text/html");
                            SPUtility.SendEmail(SPContext.Current.Web, headers, body.ToString());
                            tcs.SetResult(true); // Set the result to true if email sent successfully
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex); // Set the exception if email sending fails
                        }
                    });

                    // Await the Task created by TaskCompletionSource
                    success = await tcs.Task;

                }
                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get Emails from spfieldUserValue
        /// </summary>
        /// <param name="fieldUserValue"></param>
        /// <returns></returns>
        public static List<string> GetEmailsList(this SPFieldUserValue fieldUserValue)
        {
            try
            {
                List<string> lstEmails = new List<string>();
                if (fieldUserValue.User != null) // in case it is user
                {
                    lstEmails.Add(fieldUserValue.User.Email);
                }
                else
                {
                    SPGroup group = SPContext.Current.Web.Groups[fieldUserValue.LookupValue];
                    foreach (SPUser user in group.Users)
                    {
                        lstEmails.Add(user.Email);
                    }
                }
                return lstEmails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string ReadEmailTemplate(string Path, List<KeyValuePair<string, string>> keyValuePairs)
        {
            string html = string.Empty;
            string featurePath;
            featurePath = SPUtility.GetVersionedGenericSetupPath(Path, 15);
            StreamReader reader = new StreamReader(featurePath);
            html = reader.ReadToEnd();
            string HtmlReplace = html;
            foreach (var pair in keyValuePairs)
            {
                HtmlReplace = HtmlReplace.Replace("{" + pair.Key + "}", pair.Value);
            }
            reader.Close();
            reader = null;
            return HtmlReplace;
        }
    }

}
