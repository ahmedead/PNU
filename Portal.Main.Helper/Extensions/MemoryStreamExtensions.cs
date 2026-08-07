// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamExtensions.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   MemoryStream Extension Methods that provide conversions to and from strings
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System.IO;
    using System.Text;

    /// <summary>
    ///     MemoryStream Extension Methods that provide conversions to and from strings
    /// </summary>
    public static class MemoryStreamExtensions
    {
        /// <summary>
        /// Returns the content of the stream as a string
        /// </summary>
        /// <param name="ms">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AsString(this MemoryStream ms, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            return encoding.GetString(ms.ToArray());
        }

        /// <summary>
        /// Writes the specified string into the memory stream
        /// </summary>
        /// <param name="ms">
        /// </param>
        /// <param name="inputString">
        /// </param>
        /// <param name="encoding">
        /// </param>
        public static void FromString(this MemoryStream ms, string inputString, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            var buffer = encoding.GetBytes(inputString);
            ms.Write(buffer, 0, buffer.Length);
        }
    }
}