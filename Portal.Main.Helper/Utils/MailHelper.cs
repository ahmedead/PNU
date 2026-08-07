// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailHelper.cs" company="SURE International Technology">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   Defines the MailHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Utils
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    /// <summary>
    /// The mail helper.
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// Send mail using SMTP server
        /// </summary>
        /// <param name="msg">
        /// MailMessage object
        /// </param>
        /// <param name="settings">
        /// Hashtable contains SMTP settings
        ///     (   Host:SmtpServer,
        ///     Port:SmtpPort,
        ///     EnableSsl:SmtpEnableSSL(True or False),
        ///     IsSmtpAuthenticated(True or False),
        ///     NetworkCredential(UserName:SmtpUserName, password:SmtpPassword)
        ///     )
        /// </param>
        public static void SendMail(MailMessage msg, Hashtable settings)
        {
            var smtp = new SmtpClient();

            // make message encoding to arabic
            msg.BodyEncoding = Encoding.GetEncoding("UTF-8");
            msg.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            msg.IsBodyHtml = true;

            // SMTP Server
            if (settings["SmtpServer"].ToString() != string.Empty)
            {
                smtp.Host = settings["SmtpServer"].ToString();
            }

            // check port
            if (settings["SmtpPort"].ToString() != string.Empty)
            {
                smtp.Port = Convert.ToInt32(settings["SmtpPort"]);
            }

            // check SSL
            smtp.EnableSsl = Convert.ToBoolean(settings["SmtpEnableSSL"]);

            // check if it must be authenticate
            if (Convert.ToBoolean(settings["IsSmtpAuthenticated"]))
            {
                var cren = new NetworkCredential(
                    settings["SmtpUserName"].ToString(), 
                    settings["SmtpPassword"].ToString());
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = cren;
            }
            else
            {
                smtp.UseDefaultCredentials = true;
            }

            smtp.Send(msg);
        }
    }
}