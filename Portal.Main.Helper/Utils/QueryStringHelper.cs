// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStringHelper.cs" company="SURE International Technology">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The query string helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Portal.Main.Helper.Utils
{
    using System.Text;
    using System.Web;

    /// <summary>
    /// The query string helper.
    /// </summary>
    public class QueryStringHelper
    {
        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetValue(string key)
        {
            return HttpContext.Current.Request[key];
        }

        /// <summary>
        /// Get all parameters from query string
        /// </summary>
        /// <param name="exceptedKeys">
        /// The excepted Keys.
        /// </param>
        /// <returns>
        /// String value
        /// </returns>
        public static string GetQueryString(params string[] exceptedKeys)
        {
            var sb = new StringBuilder();
            string tmpKey;
            var found = false;

            foreach (string key in HttpContext.Current.Request.QueryString.Keys)
            {
                found = false;
                if (key == null)
                {
                    continue;
                }

                tmpKey = key.ToLower();

                for (var i = 0; i < exceptedKeys.Length; i++)
                {
                    if (exceptedKeys[i].ToLower() == tmpKey)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    continue;
                }

                // add to the query string
                sb.AppendFormat("{0}={1}&", key, HttpContext.Current.Request[key]);
            }

            return sb.ToString();
        }
    }
}