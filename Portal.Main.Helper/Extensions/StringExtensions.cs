// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The string extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        #region Constants

        /// <summary>
        ///     The newline.
        /// </summary>
        private const string Newline = "\r\n";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parses a string into an array of lines broken
        ///     by \r\n or \n
        /// </summary>
        /// <param name="s">
        /// String to check for lines
        /// </param>
        /// <returns>
        /// array of strings, or null if the string passed was a null
        /// </returns>
        public static string[] GetLines(this string s)
        {
            if (s == null)
            {
                return null;
            }

            s = s.Replace("\r\n", "\n");
            return s.Split('\n');
        }

        /// <summary>
        /// Returns a line count for a string
        /// </summary>
        /// <param name="s">
        /// string to count lines for
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int CountLines(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            return s.Split('\n').Length;
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        ///     breaking words.
        /// </summary>
        /// <param name="text">
        /// String that contains line of text
        /// </param>
        /// <param name="pos">
        /// Index where line of text starts
        /// </param>
        /// <param name="max">
        /// Maximum line length
        /// </param>
        /// <returns>
        /// The modified line length
        /// </returns>
        public static int BreakLine(this string text, int pos, int max)
        {
            // Find last whitespace in line
            var i = max - 1;

            while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            if (i < 0)
            {
                return max; // No whitespace found; break at maximum length
            }

            // Find start of whitespace
            while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            // Return length of text before whitespace
            return i + 1;
        }

        /// <summary>
        /// Returns part of a string up to the specified number of characters, while maintaining full words
        /// </summary>
        /// <param name="s">
        /// </param>
        /// <param name="length">
        /// Maximum characters to be returned
        /// </param>
        /// <returns>
        /// String
        /// </returns>
        public static string Chop(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException(s);
            }

            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var word in words.Where(word => (sb.ToString().Length + word.Length) <= length))
            {
                sb.Append(word + " ");
            }

            return sb.ToString().TrimEnd(' ') + "...";
        }

        /// <summary>
        /// Convert the string to camel case.
        /// </summary>
        /// <param name="str">
        /// the string to turn into Camel case
        /// </param>
        /// <returns>
        /// a string formatted as Camel case
        /// </returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (!char.IsUpper(str[0]))
            {
                return str;
            }

            var camelCase = char.ToLower(str[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
            if (str.Length > 1)
            {
                camelCase += str.Substring(1);
            }

            return camelCase;
        }

        /// <summary>
        /// Converts a string into bytes for storage in any byte[] types
        ///     buffer or stream format (like MemoryStream).
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <param name="encoding">
        /// The character encoding to use. Defaults to Unicode
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] ToByteArray(this string text, Encoding encoding = null)
        {
            if (text == null)
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Convert the string to Pascal case.
        /// </summary>
        /// <param name="theString">
        /// the string to turn into Pascal case
        /// </param>
        /// <returns>
        /// a string formatted as Pascal case
        /// </returns>
        public static string ToPascalCase(this string theString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (theString == null)
            {
                return null;
            }

            if (theString.Length < 2)
            {
                return theString.ToUpper();
            }

            // Split the string into words.
            var words = theString.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            return words.Aggregate(
                string.Empty, 
                (current, word) => current + (word.Substring(0, 1).ToUpper() + word.Substring(1)));
        }

        /// <summary>
        /// Capitalize the first character and add a space before
        ///     each capitalized letter (except the first character).
        /// </summary>
        /// <param name="theString">
        /// the string to turn into Proper case
        /// </param>
        /// <returns>
        /// a string formatted as Proper case
        /// </returns>
        public static string ToProperCase(this string theString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (theString == null)
            {
                return null;
            }

            if (theString.Length < 2)
            {
                return theString.ToUpper();
            }

            // Start with the first character.
            var result = theString.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (var i = 1; i < theString.Length; i++)
            {
                if (char.IsUpper(theString[i]))
                {
                    result += " ";
                }

                result += theString[i];
            }

            return result;
        }

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">
        /// Text to be word wrapped
        /// </param>
        /// <param name="width">
        /// Width, in characters, to which the text
        ///     should be word wrapped
        /// </param>
        /// <returns>
        /// The modified text
        /// </returns>
        /// <see cref="http://www.softcircuits.com/Blog/post/2010/01/10/Implementing-Word-Wrap-in-C.aspx"/>
        public static string WordWrap(this string text, int width)
        {
            int pos, next;
            var sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
            {
                return text;
            }

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf(Newline, pos, StringComparison.Ordinal);

                if (eol == -1)
                {
                    next = eol = text.Length;
                }
                else
                {
                    next = eol + Newline.Length;
                }

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        var len = eol - pos;

                        if (len > width)
                        {
                            len = BreakLine(text, pos, width);
                        }

                        sb.Append(text, pos, len);
                        sb.Append(Newline);

                        // Trim whitespace following break
                        pos += len;

                        while (pos < eol && char.IsWhiteSpace(text[pos]))
                        {
                            pos++;
                        }
                    }
                    while (eol > pos);
                }
                else
                {
                    sb.Append(Newline); // Empty line
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     The random.
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Creates a new random string of upper, lower case letters and digits.
        ///     Very useful for generating random data for storage in test data.
        /// </summary>
        /// <param name="size">
        /// The number of characters of the string to generate
        /// </param>
        /// <param name="includeNumbers">
        /// if set to <c>true</c> [include numbers].
        /// </param>
        /// <returns>
        /// randomized string
        /// </returns>
        public static string RandomString(int size, bool includeNumbers = false)
        {
            var builder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                var num =
                    Convert.ToInt32(
                        includeNumbers ? Math.Floor(62 * Random.NextDouble()) : Math.Floor(52 * Random.NextDouble()));

                char ch;
                if (num < 26)
                {
                    ch = Convert.ToChar(num + 65);
                }

                // lower case
                else if (num > 25 && num < 52)
                {
                    ch = Convert.ToChar(num - 26 + 97);
                }

                // numbers
                else
                {
                    ch = Convert.ToChar(num - 52 + 48);
                }

                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// The as unicode string.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AsUnicodeString(this string s)
        {
            var stringBuilder = new StringBuilder();
            foreach (var t in s)
            {
                stringBuilder.Append($"\\u{Convert.ToString(t, 16).PadLeft(4, '0')}");
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}