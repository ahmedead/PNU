using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Security.Cryptography;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Microsoft.SharePoint.Publishing.Internal.WebControls;
using Org.BouncyCastle.Asn1.Ocsp;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System.Web.Script;

namespace PNU.Internet.WebParts
{
    public class tblLog
    {
        public string ID { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string UserControlName { get; set; }
        public string URL { get; set; }
        
        

    }

    public static class Publics
    {

        public static string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo DTFormat;
                DateLangCulture = DateLangCulture.ToLower();
                /// We can't have the hijri date writen in English. We will get a runtime error - LAITH - 11/13/2005 1:01:45 PM -

                if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
                {
                    DateLangCulture = "ar-sa";
                }

                /// Set the date time format to the given culture - LAITH - 11/13/2005 1:04:22 PM -
                DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;

                /// Set the calendar property of the date time format to the given calendar - LAITH - 11/13/2005 1:04:52 PM -
                switch (Calendar)
                {
                    case "Hijri":
                        DTFormat.Calendar = new System.Globalization.HijriCalendar();
                        break;

                    case "Gregorian":
                        DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                        break;

                    default:
                        return "";
                }

                /// We format the date structure to whatever we want - LAITH - 11/13/2005 1:05:39 PM -
                DTFormat.ShortDatePattern = "dd/MM/yyyy";
                return (DateConv.Date.ToString("f", DTFormat));

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ConvertDateCalendar", ex.Message);
                return "";
            }

            }


        
        public static bool IsArabic
        {
            get
            {
                return SPContext.Current.Web.Language == 1025;
            }
        }
        /// <summary>    
        /// method to encrypt the string    
        /// </summary>    
        /// <param name="clearText">simple string to encrypt</param>    
        /// <returns>encrypted string</returns>    
        public static string Encrypt(string clearText)
        {
            try
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

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - Encrypt", ex.Message);
                return "";
            }
            
        }

        public static string Decrypt(string cipherText)
        {
            try
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

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - Decrypt", ex.Message);
                return "";
            }
            
        }


        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ToDataTable", ex.Message);
                return null;
            }
            
        }

        public static SPListItem GetListItemByID(string ListURL, int ReqID)
        {
            try
            {
                SPListItem currItem = null;
                SPSite app = new SPSite(SPContext.Current.Site.ID);
                SPWeb web = app.OpenWeb();
                SPList reqList = web.GetList(ListURL);
                currItem = reqList.GetItemById(ReqID);


                return currItem;
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ConvertDateCalendar", ex.Message);
                return null;
            }
            
        }


        public static string CurrentUserName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? string.Empty : HttpContext.Current.User.Identity.Name.Split('\\')[1];
            }
        }



        public static void WriteToLog(string URL,string Page,string Message)
        {
            try
            {
                tblLog _log = new tblLog();
                _log.Source = Page;
                _log.UserControlName = Page;
                _log.Message = Message;
                _log.URL = URL;

                //var sqldb = SQLFactory.GetConnectionString("New_PNU_Portal_Cust_DB");
                //SQLFactory.SetConn(sqldb);
                //SQLFactory.InsertIntoTable(_log);


                
            }

            catch (Exception ex)
            {
                //Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ConvertDateCalendar", ex.Message);
            }
            
            
        }
        public static void AddVisitorsCount(string URL, string Page)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists.TryGetList("VisitorsCount");

                            SPQuery query = new SPQuery();
                            query.Query = $@"<Where>
                                  <Eq>
                                     <FieldRef Name='URL' />
                                     <Value Type='Text'>{URL}</Value>
                                  </Eq>
                               </Where>";
                            if (list == null)
                                return;
                            SPListItemCollection coll = list.GetItems(query);
                            int Count = 0;
                            SPListItem Item = null;
                            if (coll == null || coll.Count == 0)
                            {
                                Item = list.AddItem();
                            }
                            else if (coll.Count > 0)
                            {
                                Item = coll[0];
                                Count =Convert.ToInt32( Item["COUNT"].ToString());
                            }
                            Item["Title"] = Page;
                            Item["COUNT"] = (Count + 1 ).ToString();

                            Item["URL"] = URL;
                            web.AllowUnsafeUpdates = true;
                            Item.Update();
                            list.Update();
                            web.Update();
                            web.AllowUnsafeUpdates = false;






                        }
                    }
                });




            }

            catch (Exception ex)
            {
                
            }


        }
        public static void AddHomePageVisitorsCount()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists.TryGetList("VisitorsCount");

                            
                            int Count = 0;
                            SPListItem Item = list.GetItemById(1);
  
                            Count = Convert.ToInt32(Item["COUNT"].ToString());
                            //Item["Title"] = Page;
                            Item["COUNT"] = (Count + 1).ToString();

                            //Item["URL"] = URL;
                            web.AllowUnsafeUpdates = true;
                            Item.Update();
                            list.Update();
                            web.Update();
                            web.AllowUnsafeUpdates = false;






                        }
                    }
                });




            }

            catch (Exception ex)
            {

            }


        }

        static string GetInGlobalResourceString(string resourceKey)
        {
            try
            {
                return PortalHelper.GetGlobalResourceString(resourceKey);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ConvertDateCalendar", ex.Message);
                return "";
            }
            
        }
        public static string GetAllRequestsHtml()
        {
            try
            {
                StringBuilder NewsDivAll = new StringBuilder();

                NewsDivAll.AppendLine("<div class=\"col-md-6 col-sm-12\">");
                NewsDivAll.AppendLine("<div class=\"comp-wp\">");
                NewsDivAll.AppendLine("<div class=\"comp-dta\"><small>CreatedDate</small></div>");
                NewsDivAll.AppendLine("<h5 class=\"comp-hdr\"><a href = \"#\"><a href=\"LinkToItem\">RequestTitle</a></a> </h5>");
                NewsDivAll.AppendLine("<div class=\"comp-prg\">");
                NewsDivAll.AppendLine("<p>ReqDesc</p>");
                NewsDivAll.AppendLine("</div>");
                NewsDivAll.AppendLine("<div class=\"comp-dtls d-flex align-items-end justify-content-between\">");
                NewsDivAll.AppendLine("<div><div class=\"comp-to\"><span>ServiceType</span></div>");
                NewsDivAll.AppendLine("<div class=\"comp-stts\"><span class=\"mcbr-primary-c\">Status </span></div></div>");
                NewsDivAll.AppendLine("<div><button type = \"button\" onclick=\"window.location.href = 'LinkToItem' \" class=\"btn btn-primary\" >" + GetInGlobalResourceString("Details") + "</button></div>");
                NewsDivAll.AppendLine("</div>");
                NewsDivAll.AppendLine("</div>");
                NewsDivAll.AppendLine("</div>");

                return NewsDivAll.ToString();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ConvertDateCalendar", ex.Message);
                return "";
            }
            
        }
        static SPList GetList(string SiteURL, string ListName)
        {
            try
            {
                SPSite site = SPContext.Current.Site;
                SPWeb web = site.OpenWeb(site.Url);
                SPList list = web.GetList(ListName);
                return list;
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ConvertDateCalendar", ex.Message);
                return null;
            }
            
        }


        
        


        public static string GetFileNameFromURL(string URL)
        {
            try
            {
                if (URL == null || URL == "")
                    return "";
                //Uri uri = new Uri(URL);
                string filename = URL.Substring(URL.LastIndexOf('/') + 1);
                filename = filename.Remove(filename.IndexOf('.'));
                return filename;
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"Publics - ConvertDateCalendar", ex.Message);
                return "";
            }
            

        }

        public static string TruncateText(string text, int maxLength)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text)) return string.Empty;

                // strip any HTML tags (MediaContent may contain rich text)
                string plain = System.Text.RegularExpressions.Regex
                    .Replace(text, "<.*?>", " ");

                // collapse whitespace / &nbsp;
                plain = System.Web.HttpUtility.HtmlDecode(plain);
                plain = System.Text.RegularExpressions.Regex
                    .Replace(plain, @"\s+", " ").Trim();

                if (plain.Length <= maxLength) return plain;

                // cut at the last space before the limit so we don't split a word
                string cut = plain.Substring(0, maxLength);
                int lastSpace = cut.LastIndexOf(' ');
                if (lastSpace > 0) cut = cut.Substring(0, lastSpace);

                return cut + "…";
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "Publics.TruncateText", ex.Message);
                return string.Empty;
            }
        }
    }

}
