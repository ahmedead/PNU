using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Main.Helper
{
    public class PortalHelper
    {

        /// <summary>    
        /// method to encrypt the string    
        /// </summary>    
        /// <param name="clearText">simple string to encrypt</param>    
        /// <returns>encrypted string</returns>    
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream oMemoryStream = new MemoryStream())
                {
                    using (CryptoStream oCryptoStream = new CryptoStream(oMemoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        oCryptoStream.Write(clearBytes, 0, clearBytes.Length);
                        oCryptoStream.Close();
                    }
                    clearText = Convert.ToBase64String(oMemoryStream.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }




        #region Singleton

        public static readonly PortalHelper instance = new PortalHelper();

        /// <summary>
        /// Singleton instance .
        /// </summary>        
        public static PortalHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// enumration message types
        /// </summary>
        public enum MessageType
        {
            Success,
            Error,
            Information,
            Warning
        }

        public enum CurrentCulture
        {
            Arabic,
            English
        }

        #endregion

        #region Properties

        /// <summary>
        /// SHC portal apps database connection string
        /// </summary>
        public static string MainPortalConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["MainPortalEntities"] == null)
                {
                    throw new Exception("MainPortalEntities connection string is not found");
                }

                return ConfigurationManager.ConnectionStrings["MainPortalEntities"].ConnectionString;
            }
        }

        /// <summary>
        /// get the application Id
        /// </summary>
        public static string ApplicationId { get { return "MainPortalAppID"; } }

        /// <summary>
        /// Rerturn the current page URL
        /// </summary>
        public string CurrnetPageURL
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsoluteUri;
            }
        }

        /// <summary>
        /// get the application Id
        /// </summary>
        public static string CommonGlobalResources { get { return "CommonGlobalResources"; } }

        /// <summary>
        /// Returns the current user name
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? string.Empty : HttpContext.Current.User.Identity.Name.Split('\\')[1];
            }
        }

        /// <summary>
        /// Get the current culture as enum
        /// </summary>
        public static bool IsArabic
        {
            get
            {
                return SPContext.Current.Web.Language == 1025;
            }
        }


        public static string ParentLangSite
        {
            get
            {
                return SPContext.Current.Web.Language == 1025 ? "/ar/" : "/en/"; 
            }
        }

        /// <summary>
        /// Get string from global resource
        /// </summary>
        /// <param name="key">
        /// Key name
        /// </param>
        /// <returns>
        /// string value from global resource
        /// </returns>
        public static string GetGlobalResourceString(string key)
        {
            var obj = HttpContext.GetGlobalResourceObject(CommonGlobalResources, key);

            return obj != null ? obj.ToString() : string.Empty;
        }

        #endregion

        #region Logging

        public static NLog.Logger GetCurrentClassLogger()
        {
            try
            {
                int skipFrames = 2;
                Type declaringType;
                string name;
                do
                {
                    MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                    declaringType = method.DeclaringType;
                    if (declaringType == (Type)null)
                    {
                        name = method.Name;
                        break;
                    }
                    else
                    {
                        ++skipFrames;
                        name = declaringType.FullName;
                    }
                }
                while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));
                return NLog.LogManager.GetLogger(name);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static readonly NLog.Logger Logger = GetCurrentClassLogger();

        /// <summary>
        /// Log exception details
        /// </summary>
        /// <param name="ex">ex</param>
        public static void LogException(Exception ex)
        {
            Logger.Fatal("Unexpected Exception: " + ex.ToString());
        }

        /// <summary>
        /// Log string information
        /// </summary>
        /// <param name="info">info</param>
        public static void LogInformation(string info)
        {
            Logger.Info(info);
        }


        #endregion

        #region Settings and Notifications

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="appID">The application identifier.</param>
        /// <returns></returns>
        public string GetSPSetting(string key, string appID)
        {
            string result = string.Empty;
            string siteURL = string.Format("{0}{1}", SPContext.Current.Site.Url, "/admin");
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.GetList($"{siteURL}/Lists/Settings");
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = $"<Where><And><Eq><FieldRef Name='Key' /><Value Type='Text'>{key}</Value></Eq><Eq><FieldRef Name='AppID' /><Value Type='Text'>{appID}</Value></Eq></And></Where>";
                            SPListItemCollection items = list.GetItems(query);
                            if (items.Count > 0)
                                result = items[0]["Value"].ToString();
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Gets the email template.
        /// </summary>
        /// <param name="key">The key.</param>       
        /// <returns></returns>
        public static string GetEmailTemplate(string key)
        {
            string result = string.Empty;
            string siteURL = string.Format("{0}{1}", SPContext.Current.Site.Url, "/admin");
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.GetList($"{siteURL}/Lists/EmailTemplate");
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = $"<Where><Eq><FieldRef Name='Key' /><Value Type='Text'>{key}</Value></Eq></Where>";
                            SPListItemCollection items = list.GetItems(query);
                            if (items.Count > 0)
                                result = items[0]["Value"].ToString();
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static Hashtable GetSPSettingList(string[] keys)
        {
            Hashtable result = new Hashtable();
            string siteURL = string.Format("{0}{1}", SPContext.Current.Site.Url, "/admin");
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.GetList($"{siteURL}/Lists/Settings");
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            var keyListValues = string.Empty;
                            foreach (var item in keys)
                            {
                                keyListValues += string.Format("<Value Type='Text'>{0}</Value>", item);
                            }
                            query.Query = $"<Where><In><FieldRef Name='Key' /><Values>{keyListValues}</Values></In></Where>";
                            SPListItemCollection items = list.GetItems(query);
                            if (items != null && items.Count > 0)
                            {
                                foreach (DataRow dataRow in items.GetDataTable().Rows)
                                {
                                    result.Add(dataRow["Key"].ToString(), dataRow["Value"].ToString());
                                }
                            }
                        }
                    }
                }
            });

            return result;
        }


        public static bool SendMailSP(string template, Hashtable values, string Subject, string To, string from = "skeltaadmin@sure.com.sa")
        {
            bool mailSent = false;

            // set common email template

            string body = template;
            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonTemplate" : "CommonTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
            {
                commonDesignTemplate = designTemplate.Replace("{Table}", body);

            }

            StringDictionary headers = new StringDictionary();

            headers.Add("to", To);
            headers.Add("from", from);
            headers.Add("subject", Subject);
            headers.Add("content-type", "text/html");

            mailSent = SPUtility.SendEmail(SPContext.Current.Web, headers, commonDesignTemplate);

            return mailSent;
        }

        public static bool SendMailLCGPAVisits(string template, Hashtable values, string Subject, string toMail, string fromMail, string fromName = "")
        {
            bool mailSent = false;

            // set common email template

            string body = template;
            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonIDPDesignTemplate" : "CommonIDPDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            if (fromName == "")
                fromName = "Intranet Portal";
            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, fromName);
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));

            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }

        public static bool SendMailIDP(string template, Hashtable values, string Subject, string toMail, string fromMail, string fromName = "")
        {
            bool mailSent = false;

            // set common email template

            string body = template;
            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonIDPDesignTemplate" : "CommonIDPDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "Intranet Portal");
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));

            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }
        public static bool SendMailJDP(string template, Hashtable values, string Subject, string toMail, string fromMail, string fromName = "")
        {
            bool mailSent = false;

            // set common email template

            string body = template;
            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonIDPDesignTemplate" : "CommonIDPDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "تحديث الأوصاف الوظيفية");
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));
            mailMsg.Bcc.Add(new MailAddress("aalrashudi@LCGPA.gov.sa"));

            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }

        public static bool SendMailEmployeeVoting(string template, Hashtable values, string Subject, string toMail, string fromMail, string fromName = "")
        {
            bool mailSent = false;
            string body = template;
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "EmployeeVotinDesignTemplate" : "EmployeeVotinDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);
            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "جدير الهيئة");
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));
            
            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }

        public static bool SendMailNewTemplatewithcc(string template, Hashtable values, string Subject, string toMail, string ccemail, string fromMail, string fromName = "")
        {
            bool mailSent = false;

            // set common email template

            string body = template;
            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "NewCommonDesignTemplate" : "NewCommonDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("@Body", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "Intranet Portal");
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            if(ccemail != null)
            {
                if(ccemail.Contains(";"))
                {
                    string[] CCId = ccemail.Split(';');
                    foreach (string CCEmail in CCId)
                    {
                        //mailMsg.CC.Add(new MailAddress(CCEmail));
                        mailMsg.CC.Add(new MailAddress(CCEmail.Trim(), string.Empty)); //Adding Multiple CC email Id  
                    }

                }
                else if(ccemail.Contains(","))
                {
                    string[] CCId = ccemail.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        //mailMsg.CC.Add(new MailAddress(CCEmail));
                        mailMsg.CC.Add(new MailAddress(CCEmail.Trim(), string.Empty)); //Adding Multiple CC email Id  
                    }

                }
                else
                    mailMsg.CC.Add(new MailAddress(ccemail.Trim(), string.Empty));
            }
            

            //mailMsg.CC.Add(new MailAddress(ccemail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));

            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }

        public static bool SendMailNewTemplate(string template, Hashtable values, string Subject, string toMail, string fromMail, string fromName = "")
        {
            bool mailSent = false;

            // set common email template

            string body = template;
            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "NewCommonDesignTemplate" : "NewCommonDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("@Body", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "Intranet Portal");
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));

            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }
        public static bool SendMail(string template, Hashtable values, string Subject, string toMail, string fromMail, string fromName = "")
        {
            bool mailSent = false;

            // set common email template

            string body = template;
            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }

            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonDesignTemplate" : "CommonDesignTemplateEn");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, fromName);
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            //mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));

            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }
        public static bool SendMailJobDescription(string Subject, string toMail, string BodyText, Hashtable values)
        {
            bool mailSent = false;

            // set common email template
            string fromMail = PortalHelper.Instance.GetSPSetting("ContactUsFromEmail", PortalHelper.ApplicationId);
            string template = PortalHelper.GetEmailTemplate(PortalHelper.IsArabic ? "SendMailJobDescription" : "SendMailJobDescription");
            string body = template;

            //toMail = "asharaf@lcgpa.gov.sa";



            body = body.Replace("@BodyText", BodyText);
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }
            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonIDPDesignTemplate" : "CommonIDPDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "تحديث الأوصاف الوظيفية");
            //mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("aalrashudi@LCGPA.gov.sa"));
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));
            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }

        public static bool SendMailTransactions(string Subject, string toMail, string BodyText, string ccMail="" )
        {
            bool mailSent = false;

            // set common email template
            string fromMail = PortalHelper.Instance.GetSPSetting("ContactUsFromEmail", PortalHelper.ApplicationId);
            string template = PortalHelper.GetEmailTemplate(PortalHelper.IsArabic ? "SendMailTransactions" : "SendMailTransactions");
            string body = template;


            body = body.Replace("@BodyText", BodyText);
            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonIDPDesignTemplate" : "CommonIDPDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "أتمتة الإجراءات");


            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            //mailMsg.To.Add(new MailAddress("asharaf@lcgpa.gov.sa", string.Empty));
            if (ccMail != "")
            {
                string[] Mails = ccMail.Split(',');
                foreach (string Mail in Mails)
                {
                    mailMsg.CC.Add(new MailAddress(Mail));
                }
            }
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));
            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }


        public static bool SendMailJobDescriptionToManager(string Subject, string toMail, string BodyText, Hashtable values)
        {
            bool mailSent = false;

            // set common email template
            string fromMail = PortalHelper.Instance.GetSPSetting("ContactUsFromEmail", PortalHelper.ApplicationId);
            string template = PortalHelper.GetEmailTemplate(PortalHelper.IsArabic ? "SendMailJobDescriptionToManager" : "SendMailJobDescriptionToManager");
            string body = template;

            //toMail = "asharaf@lcgpa.gov.sa";



            body = body.Replace("@BodyText", BodyText);
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }
            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonIDPDesignTemplate" : "CommonIDPDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "تحديث الأوصاف الوظيفية");
            //mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            mailMsg.Bcc.Add(new MailAddress("aalrashudi@LCGPA.gov.sa"));
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));
            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }


        public static bool SendMailVisitorToUser(string Subject, string toMail, string BodyText, Hashtable values)
        {
            bool mailSent = false;

            // set common email template
            string fromMail = PortalHelper.Instance.GetSPSetting("ContactUsFromEmail", PortalHelper.ApplicationId);
            string template = PortalHelper.GetEmailTemplate(PortalHelper.IsArabic ? "SendMailToUser" : "SendMailToUser");
            string body = template;





            body = body.Replace("@BodyText", BodyText);
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    body = body.Replace(key, values[key].ToString());
                }
            }
            string commonDesignTemplate = body;
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonIDPDesignTemplate" : "CommonIDPDesignTemplate");
            if (!string.IsNullOrEmpty(designTemplate))
                commonDesignTemplate = designTemplate.Replace("{EmailBody}", body);


            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(fromMail, "استقبال الهيئة");
            //mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = commonDesignTemplate;
            //mailMsg.CC.Add(new MailAddress("asharaf@lcgpa.gov.sa"));
            mailMsg.Bcc.Add(new MailAddress("asharaf@lcgpa.gov.sa"));
            mailSent = PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            return mailSent;
        }

        /// <summary>
        /// Send mail using SMTP server
        /// </summary>
        /// <param name="msg">MailMessage object</param>
        /// <param name="settings">
        /// Hashtable contains SMTP settings
        /// (   Host:SmtpServer, 
        ///     Port:SmtpPort,
        ///     EnableSsl:SmtpEnableSSL(True or False),
        ///     IsSmtpAuthenticated(True or False),
        ///     NetworkCredential(UserName:SmtpUserName, password:SmtpPassword)
        ///  )
        /// </param>
        public bool SendMail(MailMessage msg, Hashtable settings)
        {
            SmtpClient smtp = new SmtpClient();

            // make message encoding to arabic
            msg.BodyEncoding = Encoding.GetEncoding("UTF-8");
            msg.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            msg.IsBodyHtml = true;

            // SMTP Server
            if (settings["SmtpServer"].ToString() != "")
            {
                smtp.Host = settings["SmtpServer"].ToString();
            }

            // check port
            if (settings["SmtpPort"].ToString() != "")
            {
                smtp.Port = Convert.ToInt32(settings["SmtpPort"]);
            }

            // check SSL
            smtp.EnableSsl = Convert.ToBoolean(settings["SmtpEnableSSL"]);

            // check if it must be authenticate
            if (Convert.ToBoolean(settings["IsSmtpAuthenticated"]))
            {
                NetworkCredential cren = new NetworkCredential(settings["SmtpUserName"].ToString(), settings["SmtpPassword"].ToString());
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = cren;
            }
            else
            {
                smtp.UseDefaultCredentials = true;
            }
            NEVER_EAT_POISON_Disable_CertificateValidation();
            smtp.Send(msg);
            return true;
        }

        static void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
        }
        #endregion

        public async Task<bool> SendMailNewJoiners(string template, Hashtable values, string Subject, string toMail, string fromName = "")
        {
            bool mailSent = false;

            // set common email template
            string designTemplate = GetEmailTemplate(PortalHelper.IsArabic ? "CommonDesignTemplate" : "CommonDesignTemplate");

            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    designTemplate = designTemplate.Replace(key, values[key].ToString());
                }
            }

            var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "SmtpServer", "SmtpUserName", "SmtpPassword", "IsSmtpAuthenticated", "SmtpPort", "SmtpEnableSSL" });

            // Forming the mail message
            var mailMsg = new MailMessage();
            mailMsg.Subject = Subject;
            mailMsg.From = new MailAddress(smtpSettings["SmtpUserName"].ToString(), "Princess Nora University");
            mailMsg.To.Add(new MailAddress(toMail, string.Empty));
            mailMsg.Body = designTemplate;
            mailMsg.Bcc.Add(new MailAddress("aesharaf@pnu.edu.sa"));

            string IsSMTP = GetSPSetting("IsSMTP", "MainPortalAppID");
            string from = "Princess Nora University";
            if (IsSMTP == "1")
                PortalHelper.instance.SendMail(mailMsg, smtpSettings);
            else
            {
                StringDictionary headers = new StringDictionary();

                headers.Add("to", toMail);
                headers.Add("from", from);
                headers.Add("subject", Subject);
                headers.Add("content-type", "text/html");

                mailSent = SPUtility.SendEmail(SPContext.Current.Web, headers, designTemplate);
            }

            return mailSent;
        }

    }
}
