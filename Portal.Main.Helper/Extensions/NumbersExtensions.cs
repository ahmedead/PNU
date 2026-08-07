// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumbersExtensions.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The numbers extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System;
    using System.Text;

    using Portal.Main.Helper.Utils;

    /// <summary>
    ///     The numbers extensions.
    /// </summary>
    public static class NumbersExtensions
    {
        /// <summary>
        /// The to arabic digits string.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToArabicDigitsString(this int number)
        {
            var str = number.ToString();
            string[] arabicNumsArr = { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            var sb = new StringBuilder();
            foreach (var t in str)
            {
                sb.AppendFormat("{0}", arabicNumsArr[int.Parse(t.ToString())]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// The to english words.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToEnglishWords(this int number)
        {
            if (number == 0)
            {
                return "zero";
            }

            if (number < 0)
            {
                return "minus " + ToEnglishWords(Math.Abs(number));
            }

            var words = string.Empty;

            if ((number / 1000000) > 0)
            {
                words += ToEnglishWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += ToEnglishWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ToEnglishWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != string.Empty)
                {
                    words += "and ";
                }

                var unitsMap = new[]
                                   {
                                       "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", 
                                       "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", 
                                       "seventeen", "eighteen", "nineteen"
                                   };
                var tensMap = new[]
                                  {
                                      "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", 
                                      "ninety"
                                  };

                if (number < 20)
                {
                    words += unitsMap[number];
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += "-" + unitsMap[number % 10];
                    }
                }
            }

            return words;
        }

        /// <summary>
        /// The to arabic words.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToArabicWords(this decimal number)
        {
            var toWord = new NumberToWord(number, null);
            return toWord.ConvertToArabic();
        }

        /// <summary>
        /// The to english words.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToEnglishWords(this decimal number)
        {
            var toWord = new NumberToWord(number, null);
            return toWord.ConvertToEnglish();
        }
    }
}