// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyAccessor.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   AssemblyAccessor
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Reflection
{
    using System.IO;
    using System.Reflection;

    /// <summary>
    ///     AssemblyAccessor
    /// </summary>
    public static class AssemblyAccessor
    {
        /// <summary>
        ///     Gets Assembly Title
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyTitle()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != string.Empty)
                {
                    return titleAttribute.Title;
                }
            }

            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }

        /// <summary>
        ///     Gets Assembly Version
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        ///     Gets Assembly Description
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyDescription()
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length == 0)
            {
                return string.Empty;
            }

            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }

        /// <summary>
        ///     Gets Assembly Product
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyProduct()
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                return string.Empty;
            }

            return ((AssemblyProductAttribute)attributes[0]).Product;
        }

        /// <summary>
        ///     Gets Assembly Copyright
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyCopyright()
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
                return string.Empty;
            }

            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        /// <summary>
        ///     Gets Assembly Company
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyCompany()
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
                return string.Empty;
            }

            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }

        /// <summary>
        ///     Gets Assembly FileVersion
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyFileVersion()
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (attributes.Length == 0)
            {
                return string.Empty;
            }

            return ((AssemblyFileVersionAttribute)attributes[0]).Version;
        }
    }
}