using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Workflow.Utilities
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
       
        public static void SendEmail(string to, List<string> CCList, string subject, string body)
        {
            try
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
                    //headers.Add("bcc", "");
                    headers.Add("from", sender);
                    headers.Add("subject", subject);
                    headers.Add("content-type", "text/html");
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        SPUtility.SendEmail(SPContext.Current.Web, headers, body.ToString());
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool SendEmailToGroup(string groupName, string cc, string subject, string body)
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
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
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
                        success = SPUtility.SendEmail(SPContext.Current.Web, headers, body.ToString());
                    });
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
