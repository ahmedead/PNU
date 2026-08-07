// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonHelper.cs" company="SURE International Technology">
//   Copyright © 2015 All Right Reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    /// <summary>
    ///     The common helper.
    /// </summary>
    public class CommonHelper
    {
        #region Public Properties

        /// <summary>
        /// The is arabic.
        /// </summary>
        public static bool IsArabic => Thread.CurrentThread.CurrentCulture.LCID == 1025;

        /// <summary>
        ///     The culture name.
        /// </summary>
        public enum CultureName
        {
            /// <summary>
            ///     The arabic.
            /// </summary>
            Arabic, 

            /// <summary>
            ///     The english.
            /// </summary>
            English
        }

        /// <summary>
        ///     Generate random code to be used as activation code by email or SMS
        /// </summary>
        /// <returns>Code</returns>
        public static string GenerateRandomCode()
        {
            var random = new Random();
            return random.Next(111111, 999999).ToString();
        }

        /// <summary>
        ///     Generate random password to be used as activation code by email or SMS
        /// </summary>
        /// <returns>Code</returns>
        public static string GenerateRandomPassword()
        {
            var random = new Random();
            return random.Next(111111, int.MaxValue).ToString();
        }

        /// <summary>
        ///     Get the current culture as enum
        /// </summary>
        public static CultureName CurrentCulture
        {
            get
            {
                if (Thread.CurrentThread.CurrentCulture.LCID == 1033)
                {
                    return CultureName.English;
                }

                return CultureName.Arabic;
            }
        }

      
        ///// <summary>
        /////     Get current application ID
        ///// </summary>
        // public static string ApplicationID
        // {
        // get
        // {
        // return WebConfigHelper.GetApplicationSetting<string>("ApplicationID");
        // }
        // }

        /// <summary>
        ///     Get current page URL
        /// </summary>
        public static string PageURL
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.Url.AbsoluteUri;
                }

                return null;
            }
        }

        /// <summary>
        ///     Get the username for current logged in user
        /// </summary>
        public static string CurrentUserName
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                return HttpContext.Current.User.Identity.Name;
            }
        }

        /// <summary>
        ///     Get the username for current logged in user
        /// </summary>
        public static string CurrentUserNameWithoutDomain
        {
            get
            {
                if (HttpContext.Current.User.Identity.Name == null)
                {
                    return string.Empty;
                }

                if (CurrentUserName.Contains("\\"))
                {
                    var user = HttpContext.Current.User.Identity.Name.Split("\\".ToCharArray());
                    return user[1];
                }

                if (CurrentUserName.Contains("|"))
                {
                    return CurrentUserName.Split('|')[2];
                }

                return CurrentUserName;
            }
        }

        /// <summary>
        ///     Get current web site URL without page path just to /
        ///     e.g: http://www.domain.com or http://www.domain.com/site
        /// </summary>
        public static string CurrentSiteURL
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion

        #region Public Methodes

        /// <summary>
        /// Check if mobile argument is valid mobile number
        /// </summary>
        /// <param name="mobile">
        /// Mobile number stats with 05
        /// </param>
        /// <returns>
        /// True if mobile argument is valid Saudi mobile number otherwise false
        /// </returns>
        public static bool CheckSaudiMobileFormat(string mobile)
        {
            mobile = mobile.Trim();
            if (string.IsNullOrEmpty(mobile))
            {
                return false;
            }

            var pattern = @"05[0-9]{8,8}$";
            return Regex.IsMatch(mobile, pattern);
        }

        /// <summary>
        /// Check if the passed name is Arabic name
        /// </summary>
        /// <param name="name">
        /// Name, it is a required
        /// </param>
        /// <returns>
        /// true if the name contains only Arabic characters
        /// </returns>
        public static bool IsArabicName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            var pattern = @"^[ ءةآأ-ي\s]{2,}$";
            return Regex.IsMatch(name, pattern);
        }

        /// <summary>
        /// Check if the passed string contains only numbers
        /// </summary>
        /// <param name="number">
        /// Name, it is a required
        /// </param>
        /// <returns>
        /// true if the name contains only numbers
        /// </returns>
        public static bool IsNumber(string number)
        {
            number = number.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }

            var pattern = @"^[0-9]{1,50}$";
            return Regex.IsMatch(number, pattern);
        }

        /// <summary>
        /// Check if email argument is valid email
        /// </summary>
        /// <param name="email">
        /// Email address
        /// </param>
        /// <returns>
        /// true if email argument is valid email otherwise false
        /// </returns>
        public static bool IsValidEmail(string email)
        {
            email = email.Trim();
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Convert temp degree from fahrenheit to celsius
        /// </summary>
        /// <param name="temperatureCelsius">
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public double FahrenheitToCelsius(string temperatureCelsius)
        {
            var fahrenheit = double.Parse(temperatureCelsius);
            return Math.Round((fahrenheit - 32) * 5 / 9);
        }

        /// <summary>
        /// Redirect to destination page
        /// </summary>
        /// <param name="destPage">
        /// Destination page
        /// </param>
        public static void Redirect(string destPage)
        {
            var control = new Control();
            destPage = control.ResolveClientUrl(destPage);
            HttpContext.Current.Response.RedirectLocation = destPage;
            HttpContext.Current.Response.StatusCode = 301;
        }

        /// <summary>
        /// Convert numbers to arabic numeral
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// Arabic integer numeral
        /// </returns>
        public static string ConvertToArabicNumeral(int number)
        {
            var tmp = number.ToString();
            string[] arabicNums = { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            var sb = new StringBuilder(string.Empty);
            for (var i = 0; i < tmp.Length; i++)
            {
                sb.AppendFormat("{0}", arabicNums[Convert.ToInt32(tmp[i].ToString())]);
            }

            return sb.ToString();
        }

        // convert from arabic numeral to english numeral
        /// <summary>
        /// The convert to english numeral.
        /// </summary>
        /// <param name="arabicNumebr">
        /// The arabic numebr.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ConvertToEnglishNumeral(string arabicNumebr)
        {
            var englishNumber = arabicNumebr;

            try
            {
                foreach (var arn in arabicNumebr)
                {
                    switch (arn)
                    {
                        case '٠':
                            englishNumber = englishNumber.Replace('٠', '0');
                            break;
                        case '١':
                            englishNumber = englishNumber.Replace('١', '1');
                            break;
                        case '٢':
                            englishNumber = englishNumber.Replace('٢', '2');
                            break;
                        case '٣':
                            englishNumber = englishNumber.Replace('٣', '3');
                            break;
                        case '٤':
                            englishNumber = englishNumber.Replace('٤', '4');
                            break;
                        case '٥':
                            englishNumber = englishNumber.Replace('٥', '5');
                            break;
                        case '٦':
                            englishNumber = englishNumber.Replace('٦', '6');
                            break;
                        case '٧':
                            englishNumber = englishNumber.Replace('٧', '7');
                            break;
                        case '٨':
                            englishNumber = englishNumber.Replace('٨', '8');
                            break;
                        case '٩':
                            englishNumber = englishNumber.Replace('٩', '9');
                            break;
                    }
                }

                return englishNumber;
            }
            catch
            {
                return arabicNumebr;
            }
        }

        /// <summary>
        /// Generate regular expression to validate allowed extensions
        /// </summary>
        /// <param name="ext">
        /// allowed extensions separtaed with comma
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GenerateAllowedExtensionsExpression(string ext)
        {
            if (ext.Trim() == string.Empty)
            {
                return string.Empty;
            }

            var temp = ext.Trim().ToLower().Split(",".ToCharArray());
            var sb = new StringBuilder(@"^.+\.(");
            var tmp = string.Empty;

            // loop for each extension
            for (var i = 0; i < temp.Length; i++)
            {
                // get "i" extension
                tmp = temp[i];

                if (i == 0)
                {
                    sb.Append("(");
                }
                else
                {
                    sb.Append("|(");
                }

                // loop for each char
                for (var j = 0; j < tmp.Length; j++)
                {
                    sb.AppendFormat("[{0}{1}]", tmp[j], tmp[j].ToString().ToUpper());
                }

                sb.Append(")");
            }

            sb.Append(")$");

            return sb.ToString();
        }

        /// <summary>
        ///     Refresh current page
        /// </summary>
        public static void Refresh()
        {
            Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        /// <summary>
        /// Remove all HTML tags from "input string"
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// Plain text
        /// </returns>
        public static string RemoveTag(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // remove HTML tags
            var r = new Regex("\\<[^\\<]*\\>");
            var strOutput = r.Replace(input, string.Empty);

            // remove space
            r = new Regex("&nbsp;");
            strOutput = r.Replace(strOutput, string.Empty);

            return strOutput;
        }

        /// <summary>
        /// Replace plain new line with "<br/>" tag for HTML
        /// </summary>
        /// <param name="str">
        /// some string
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string InitHTMLSpace(string str)
        {
            return Regex.Replace(str, "\r\n", "<br />");
        }

        /// <summary>
        /// Check if str is integer (Int32) value
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// If str is integer value return true else false
        /// </returns>
        public static bool IsInteger(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var result = 0;
            return int.TryParse(s, out result);
        }

        /// <summary>
        /// Init DropDownList web control by its value
        /// </summary>
        /// <param name="value">
        /// DropDownList value
        /// </param>
        /// <param name="ddl">
        /// DropDownList web control
        /// </param>
        public static void InitDropDownByValue(string value, DropDownList ddl)
        {
            ddl.ClearSelection();
            if (ddl.Items.FindByValue(value) != null)
            {
                ddl.Items.FindByValue(value).Selected = true;
            }
            else
            {
                ddl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Add first item to drop down list control
        /// </summary>
        /// <param name="ddl">
        /// </param>
        /// <param name="title">
        /// </param>
        public static void AddFirstItemToDropDownList(DropDownList ddl, string title)
        {
            ddl.Items.Insert(0, new ListItem(title, string.Empty));
        }

        /// <summary>
        ///     Get Full Virtual Path For Application
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string getFullVirtualPathForApplication()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)
                   + HttpContext.Current.Request.ApplicationPath;
        }

        /// <summary>
        /// Convert List of Object To XML String
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="root">
        /// </param>
        /// <param name="list">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ConvertListOfObjectToXMl<T>(string root, List<T> list)
        {
            var xDoc = new XmlDocument();

            XmlNode rootNode = xDoc.CreateElement(root);

            foreach (var elem in list)
            {
                XmlNode classNode = xDoc.CreateElement(elem.GetType().Name);

                var properties = elem.GetType().GetProperties();

                foreach (var property in properties)
                {
                    XmlNode p = xDoc.CreateElement(property.Name);

                    p.InnerText = property.GetValue(elem, null).ToString();

                    classNode.AppendChild(p);
                }

                rootNode.AppendChild(classNode);
            }

            xDoc.AppendChild(rootNode);

            return xDoc.InnerXml;
        }

        /// <summary>
        /// Convert From Object To XML String
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="t">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ConvertFromObjectToXML<T>(T t)
        {
            var xDoc = new XmlDocument();

            XmlNode classNode = xDoc.CreateElement(t.GetType().Name);

            var properties = t.GetType().GetProperties();

            foreach (var property in properties)
            {
                XmlNode p = xDoc.CreateElement(property.Name);

                p.InnerText = property.GetValue(t, null) == null ? string.Empty : property.GetValue(t, null).ToString();

                classNode.AppendChild(p);
            }

            xDoc.AppendChild(classNode);

            return xDoc.InnerXml;
        }

        /// <summary>
        /// Check if This Exression is GUID
        /// </summary>
        /// <param name="expression">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsGUID(string expression)
        {
            if (expression != null)
            {
                var guidRegEx =
                    new Regex(
                        @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return guidRegEx.IsMatch(expression);
            }

            return false;
        }

        /// <summary>
        /// Check national/Iqama number
        /// </summary>
        /// <param name="nationalNumber">
        /// national/Iqama number
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNationalNumber(string nationalNumber)
        {
            if (nationalNumber.Length != 10)
            {
                return false;
            }

            var charArray = nationalNumber.ToCharArray();
            var numArray = new int[10];
            for (var i = 0; i < charArray.Length; i++)
            {
                numArray[i] = (int)char.GetNumericValue(charArray[i]);
            }

            var sum = 0;
            for (var i = 0; i < numArray.Length - 1; i++)
            {
                if (i % 2 != 0)
                {
                    sum += numArray[i];
                }
                else
                {
                    var oddByTwo = numArray[i] * 2;
                    var oddByTwoString = Convert.ToString(oddByTwo);
                    var oddByTwoArray = new int[oddByTwoString.Length];
                    var oddByTwoSum = 0;
                    for (var j = 0; j < oddByTwoArray.Length; j++)
                    {
                        oddByTwoArray[j] = (int)char.GetNumericValue(oddByTwoString[j]);
                        oddByTwoSum += oddByTwoArray[j];
                    }

                    sum += oddByTwoSum;
                }
            }

            var sumString = Convert.ToString(sum);
            var unit = (int)char.GetNumericValue(sumString[sumString.Length - 1]);
            if (unit == 0 && numArray[9] == 0)
            {
                return true;
            }

            if ((10 - unit) == numArray[9])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check national number
        /// </summary>
        /// <param name="nationalNumber">
        /// Saudi Identity number
        /// </param>
        /// <returns>
        /// True if it is Suadi ID
        /// </returns>
        public static bool IsSaudiNumber(string nationalNumber)
        {
            nationalNumber = nationalNumber.Trim();
            if (nationalNumber.Length != 10)
            {
                return false;
            }

            if (!nationalNumber.StartsWith("1"))
            {
                return false;
            }

            var charArray = nationalNumber.ToCharArray();
            var numArray = new int[10];
            for (var i = 0; i < charArray.Length; i++)
            {
                numArray[i] = (int)char.GetNumericValue(charArray[i]);
            }

            var sum = 0;
            for (var i = 0; i < numArray.Length - 1; i++)
            {
                if (i % 2 != 0)
                {
                    sum += numArray[i];
                }
                else
                {
                    var oddByTwo = numArray[i] * 2;
                    var oddByTwoString = Convert.ToString(oddByTwo);
                    var oddByTwoArray = new int[oddByTwoString.Length];
                    var oddByTwoSum = 0;
                    for (var j = 0; j < oddByTwoArray.Length; j++)
                    {
                        oddByTwoArray[j] = (int)char.GetNumericValue(oddByTwoString[j]);
                        oddByTwoSum += oddByTwoArray[j];
                    }

                    sum += oddByTwoSum;
                }
            }

            var sumString = Convert.ToString(sum);
            var unit = (int)char.GetNumericValue(sumString[sumString.Length - 1]);
            if (unit == 0 && numArray[9] == 0)
            {
                return true;
            }

            if ((10 - unit) == numArray[9])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Convert string to Unicode encoding
        /// </summary>
        /// <param name="s">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ConvertStringToUnicode(string s)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < s.Length; i++)
            {
                sb.Append(Convert.ToString(s[i], 16).PadLeft(4, '0'));
            }

            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// Convert string to Unicode encoding with \u as prefix
        /// </summary>
        /// <param name="s">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ConvertStringToUnicodeWithU(string s)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < s.Length; i++)
            {
                sb.Append(string.Format(@"\u{0}", Convert.ToString(s[i], 16).PadLeft(4, '0')));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     The get url without query string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetUrlWithoutQueryString()
        {
            var fullPageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            var questionMarkIndex = fullPageUrl.IndexOf("?");

            var pageUrlWithoutQuerystring = fullPageUrl;
            if (questionMarkIndex >= 0)
            {
                pageUrlWithoutQuerystring = fullPageUrl.Substring(0, questionMarkIndex);
            }

            return pageUrlWithoutQuerystring; // .Replace("http:", "https:");
        }

        #endregion
    }
}