// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionExtensions.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The exception extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Web;

    /// <summary>
    ///     The exception extensions.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The to formatted string.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToFormattedString(this Exception exception)
        {
            if (HttpContext.Current != null)
            {
                exception.Data["Page Url"] = HttpContext.Current.Request.Url.ToString();

                exception.Data["Browser"] =
                    $"{HttpContext.Current.Request.Browser.Browser} {HttpContext.Current.Request.Browser.MajorVersion}";

                exception.Data["Current User"] = HttpContext.Current.User != null
                                                     ? HttpContext.Current.User.Identity.Name
                                                     : "anonymous";
            }

            var exceptionString = new StringBuilder();
            exceptionString.AppendFormat("{0}\n", exception.Message);
            exceptionString.Append(exception);
            exceptionString.Append("\nData:\n");

            foreach (DictionaryEntry dictionaryEntry in exception.Data)
            {
                exceptionString.AppendFormat("\t{0}: {1}\n", dictionaryEntry.Key, dictionaryEntry.Value);
            }

            exceptionString.Append("--------- End of the exception ---------");

            return exceptionString.ToString();
        }

        #endregion
    }
}