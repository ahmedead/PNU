// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeHelper.cs" company="SURE International Technology">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   Group of methods help you to manipulate with DateTime
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Portal.Main.Helper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    ///     Group of methods help you to manipulate with DateTime
    /// </summary>
    public static class DateTimeHelper
    {
        #region From String to Date

        /// <summary>
        /// Convert string Hijri date to UmAlQura date
        /// </summary>
        /// <param name="hijriStr">
        /// Hijry date
        /// </param>
        /// <returns>
        /// UnAlQura DateTime format
        /// </returns>
        public static DateTime FromHijriStringToUmAlQuraDate(string hijriStr)
        {
            var hijriCal = new HijriCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var hijriDate = StringToDate(hijriStr, hijriCal);

            var umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            var umAlQuraDate = StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        /// <summary>
        /// Convert Hijri string date to Miladi (US date time format) date
        /// </summary>
        /// <param name="hijriStr">
        /// Hijry date
        /// </param>
        /// <returns>
        /// Gregorian DateTime format
        /// </returns>
        public static DateTime FromHijriStringToMiladiDate(string hijriStr)
        {
            var hijriCal = new HijriCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var hijriDate = StringToDate(hijriStr, hijriCal);

            var miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);
            var miladiDate = StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        /// <summary>
        /// Convert string UmAlQUra date to Hijri date
        /// </summary>
        /// <param name="umAlQuraStr">
        /// Hijry date
        /// </param>
        /// <returns>
        /// Hijri DateTime format
        /// </returns>
        public static DateTime FromUmAlQuraStringToHijriDate(string umAlQuraStr)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, umAlQuraCal);

            var hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);
            var hijriDate = StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        /// <summary>
        /// Convert UmAlQura string date to Miladi (US date time format) date
        /// </summary>
        /// <param name="umAlQuraStr">
        /// The um Al Qura Str.
        /// </param>
        /// <returns>
        /// Gregorian DateTime format
        /// </returns>
        public static DateTime FromUmAlQuraStringToMiladiDate(string umAlQuraStr)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, umAlQuraCal);

            var miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);
            var miladiDate = StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        /// <summary>
        /// Convert Miladi string date to UmAlQura date
        /// </summary>
        /// <param name="miladiStr">
        /// Miladi string date
        /// </param>
        /// <returns>
        /// UmAlQura DateTime format
        /// </returns>
        public static DateTime FromMiladiStringToUmAlQuraDate(string miladiStr)
        {
            var miladiCal = new GregorianCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var miladiDate = StringToDate(miladiStr, miladiCal);

            var umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            var umAlQuraDate = StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        /// <summary>
        /// Convert Miladi string date to Hijri date
        /// </summary>
        /// <param name="miladiStr">
        /// Miladi string date
        /// </param>
        /// <returns>
        /// Hijri DateTime format
        /// </returns>
        public static DateTime FromMiladiStringToHijriDate(string miladiStr)
        {
            var miladiCal = new GregorianCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var miladiDate = StringToDate(miladiStr, miladiCal);

            var hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);
            var hijriDate = StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        #endregion

        #region Date To String

        /// <summary>
        /// Convert date to UmAlqura format whatever is
        /// </summary>
        /// <param name="date">
        /// Instance of DateTime
        /// </param>
        /// <param name="format">
        /// Output format
        /// </param>
        /// <returns>
        /// UmAlqura date
        /// </returns>
        public static string ConvertDateToUmAlqura(DateTime date, string format)
        {
            // check if the InDate is UmAlqura date or not
            var isUmAlqura = IsUmAlquraDate(date.Day.ToString(), date.Month.ToString(), date.Year.ToString());

            // check if the InDate is UmAlqura date or not
            var isGreg = IsGregDate(date.Day.ToString(), date.Month.ToString(), date.Year.ToString());

            var inDateString = string.Format("{0}/{1}/{2}", date.Day, date.Month, date.Year);

            if (isUmAlqura)
            {
                return FromUmAlQuraStringToUmAlQuraString(inDateString, format);
            }

            if (isGreg)
            {
                return FromMiladiStringToUmAlQuraString(inDateString, format);
            }

            return FromHijriStringToUmAlQuraString(inDateString, format);
        }

        /// <summary>
        /// The from hijri date to um al qura string.
        /// </summary>
        /// <param name="hijriDate">
        /// The hijri date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromHijriDateToUmAlQuraString(DateTime hijriDate)
        {
            var hijriCal = new HijriCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// The from hijri date to miladi string.
        /// </summary>
        /// <param name="hijriDate">
        /// The hijri date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromHijriDateToMiladiString(DateTime hijriDate)
        {
            var hijriCal = new HijriCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from um al qura date to hijri string.
        /// </summary>
        /// <param name="umAlQuraDate">
        /// The um al qura date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraDateToHijriString(DateTime umAlQuraDate)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        /// <summary>
        /// The from um al qura date to um al qura string.
        /// </summary>
        /// <param name="umAlQuraDate">
        /// The um al qura date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraDateToUmAlQuraString(DateTime umAlQuraDate)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var umAlQuraCulture = new CultureInfo("ar-SA");

            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            return umAlQuraDate.ToString("dd/MM/yyyy", umAlQuraCulture);
        }

        /// <summary>
        /// The from miladi date to miladi string.
        /// </summary>
        /// <param name="gregDate">
        /// The greg date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiDateToMiladiString(DateTime gregDate)
        {
            var greg = new GregorianCalendar();
            var enCulture = new CultureInfo("en-US");

            enCulture.DateTimeFormat.Calendar = greg;

            return gregDate.ToString("dd/MM/yyyy", enCulture);
        }

        /// <summary>
        /// The from um al qura date to miladi string.
        /// </summary>
        /// <param name="umAlQuraDate">
        /// The um al qura date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraDateToMiladiString(DateTime umAlQuraDate)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from miladi date to um al qura string.
        /// </summary>
        /// <param name="miladiDate">
        /// The miladi date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiDateToUmAlQuraString(DateTime miladiDate)
        {
            var miladiCal = new GregorianCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiCulture = new CultureInfo("ar-EG");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// Convert from Gregorean date to UmAlQura date
        /// </summary>
        /// <param name="miladiDate">
        /// Gregorean Date
        /// </param>
        /// <param name="dateFormat">
        /// String format (The format must be true format or exception will rised)
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiDateToUmAlQuraString(DateTime miladiDate, string dateFormat)
        {
            var miladiCal = new GregorianCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraString = miladiDate.ToString(dateFormat, umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// The from miladi date to hijri string.
        /// </summary>
        /// <param name="miladiDate">
        /// The miladi date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiDateToHijriString(DateTime miladiDate)
        {
            var miladiCal = new GregorianCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        #endregion

        #region From String to String

        /// <summary>
        /// The from hijri string to um al qura string.
        /// </summary>
        /// <param name="hijriStr">
        /// The hijri str.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromHijriStringToUmAlQuraString(string hijriStr, string format)
        {
            var hijriCal = new HijriCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var hijriDate = StringToDate(hijriStr, hijriCal);

            var umAlQuraString = hijriDate.ToString(format, umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// The from hijri string to um al qura string.
        /// </summary>
        /// <param name="hijriStr">
        /// The hijri str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromHijriStringToUmAlQuraString(string hijriStr)
        {
            var hijriCal = new HijriCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var hijriDate = StringToDate(hijriStr, hijriCal);

            var umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// The from hijri string to miladi string.
        /// </summary>
        /// <param name="hijriStr">
        /// The hijri str.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromHijriStringToMiladiString(string hijriStr, string format, string culture)
        {
            var hijriCal = new HijriCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo(culture);
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var hijriDate = StringToDate(hijriStr, hijriCal);

            var miladiString = hijriDate.ToString(format, miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from hijri string to miladi string.
        /// </summary>
        /// <param name="hijriStr">
        /// The hijri str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromHijriStringToMiladiString(string hijriStr)
        {
            var hijriCal = new HijriCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var hijriDate = StringToDate(hijriStr, hijriCal);

            var miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from um al qura string to hijri string.
        /// </summary>
        /// <param name="umAlQuraStr">
        /// The um al qura str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraStringToHijriString(string umAlQuraStr)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, umAlQuraCal);

            var hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        /// <summary>
        /// The from um al qura string to miladi string.
        /// </summary>
        /// <param name="umAlQuraStr">
        /// The um al qura str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraStringToMiladiString(string umAlQuraStr)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, umAlQuraCal);

            var miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from um al qura string to um al qura string.
        /// </summary>
        /// <param name="umAlQuraStr">
        /// The um al qura str.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraStringToUmAlQuraString(string umAlQuraStr, string format)
        {
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, umAlQuraCal);

            var miladiString = umAlQuraDate.ToString(format, umAlQuraCulture);
            return miladiString;
        }

        /// <summary>
        /// The from um al qura string to miladi string.
        /// </summary>
        /// <param name="umAlQuraStr">
        /// The um al qura str.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraStringToMiladiString(string umAlQuraStr, string format)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("ar-EG");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, umAlQuraCal);

            var miladiString = umAlQuraDate.ToString(format, miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from miladi string to miladi string.
        /// </summary>
        /// <param name="umAlQuraStr">
        /// The um al qura str.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiStringToMiladiString(string umAlQuraStr, string format, string culture)
        {
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo(culture);
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, miladiCal);

            var miladiString = umAlQuraDate.ToString(format, miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from um al qura string to miladi string.
        /// </summary>
        /// <param name="umAlQuraStr">
        /// The um al qura str.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromUmAlQuraStringToMiladiString(string umAlQuraStr, string format, string culture)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo(culture);
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var umAlQuraDate = StringToDate(umAlQuraStr, umAlQuraCal);

            var miladiString = umAlQuraDate.ToString(format, miladiCulture);
            return miladiString;
        }

        /// <summary>
        /// The from miladi string to um al qura string.
        /// </summary>
        /// <param name="miladiStr">
        /// The miladi str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiStringToUmAlQuraString(string miladiStr)
        {
            var miladiCal = new GregorianCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var miladiDate = StringToDate(miladiStr, miladiCal);

            var umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// The from miladi string to um al qura string.
        /// </summary>
        /// <param name="miladiStr">
        /// The miladi str.
        /// </param>
        /// <param name="dateFormat">
        /// The date format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiStringToUmAlQuraString(string miladiStr, string dateFormat)
        {
            var miladiCal = new GregorianCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiCulture = new CultureInfo("ar-EG");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var miladiDate = StringToDate(miladiStr, miladiCal);

            var umAlQuraString = miladiDate.ToString(dateFormat, umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// The from miladi string to hijri string.
        /// </summary>
        /// <param name="miladiStr">
        /// The miladi str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FromMiladiStringToHijriString(string miladiStr)
        {
            var miladiCal = new GregorianCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var miladiDate = StringToDate(miladiStr, miladiCal);

            var hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        #endregion

        #region From Date To Date

        /// <summary>
        /// The from hijri date to um al qura date.
        /// </summary>
        /// <param name="hijriDate">
        /// The hijri date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FromHijriDateToUmAlQuraDate(DateTime hijriDate)
        {
            var hijriCal = new HijriCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);

            var umAlQuraDate = StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        /// <summary>
        /// The from hijri date to miladi date.
        /// </summary>
        /// <param name="hijriDate">
        /// The hijri date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FromHijriDateToMiladiDate(DateTime hijriDate)
        {
            var hijriCal = new HijriCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);

            var miladiDate = StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        /// <summary>
        /// The from um al qura date to hijri date.
        /// </summary>
        /// <param name="umAlQuraDate">
        /// The um al qura date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FromUmAlQuraDateToHijriDate(DateTime umAlQuraDate)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);

            var hijriDate = StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        /// <summary>
        /// The from um al qura date to miladi date.
        /// </summary>
        /// <param name="umAlQuraDate">
        /// The um al qura date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FromUmAlQuraDateToMiladiDate(DateTime umAlQuraDate)
        {
            var umAlQuraCal = new UmAlQuraCalendar();
            var miladiCal = new GregorianCalendar();

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);

            var miladiDate = StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        /// <summary>
        /// The from miladi date to um al qura date.
        /// </summary>
        /// <param name="miladiDate">
        /// The miladi date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FromMiladiDateToUmAlQuraDate(DateTime miladiDate)
        {
            var miladiCal = new GregorianCalendar();
            var umAlQuraCal = new UmAlQuraCalendar();

            var umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);

            var umAlQuraDate = StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        /// <summary>
        /// The from miladi date to hijri date.
        /// </summary>
        /// <param name="miladiDate">
        /// The miladi date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FromMiladiDateToHijriDate(DateTime miladiDate)
        {
            var miladiCal = new GregorianCalendar();
            var hijriCal = new HijriCalendar();

            var hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            var miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            var hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);

            var hijriDate = StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        #endregion

        #region Misc

        /// <summary>
        /// The get month names.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <param name="Culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetMonthNames(Calendar calendar, CultureInfo Culture)
        {
            if ((calendar is UmAlQuraCalendar) && (Culture.Name == "en-US"))
            {
                return new[]
                           {
                               "Muharram", "Safar", "Rabi' al-awwal", "Rabi' al-thani", "Jumada al-awwal", 
                               "Jumada al-thani", "Rajab", "Sha'aban", "Ramadan", "Shawwal", "Dhu al-Qi'dah", 
                               "Dhu al-Hijjah"
                           };
            }

            var info = Culture.DateTimeFormat;
            info.Calendar = calendar;
            var MonthNames = info.MonthNames;
            Array.Resize(ref MonthNames, 12);
            return MonthNames;
        }

        /// <summary>
        /// The get month names.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetMonthNames(Calendar calendar)
        {
            return GetMonthNames(calendar, new CultureInfo(CultureInfo.CurrentCulture.ToString(), false));
        }

        /// <summary>
        /// The get current um alqura date.
        /// </summary>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetCurrentUmAlquraDate()
        {
            var strDate = FromMiladiDateToUmAlQuraString(DateTime.Now);
            return GetDateParts(strDate);
        }

        /// <summary>
        /// The get current greg date.
        /// </summary>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetCurrentGregDate()
        {
            var strDate = FromMiladiDateToMiladiString(DateTime.Now);
            return GetDateParts(strDate);
        }

        /// <summary>
        /// return date' parts as string array
        /// </summary>
        /// <param name="d">
        /// </param>
        /// <returns>
        /// day, month, year
        /// </returns>
        public static string[] GetDateParts(string d)
        {
            return d.Split('/');
        }

        /// <summary>
        /// return array of 31 days
        /// </summary>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetDays()
        {
            string[] days =
                {
                    "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", 
                    "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", 
                    "31"
                };
            return days;
        }

        /// <summary>
        /// return array of 30 days for Hijri dates
        /// </summary>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetHijriDays()
        {
            string[] days =
                {
                    "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", 
                    "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30"
                };
            return days;
        }

        /// <summary>
        /// return array of 12 months
        /// </summary>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetMonths()
        {
            string[] months = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            return months;
        }

        /// <summary>
        /// return array of min-max years
        /// </summary>
        /// <param name="min">
        /// </param>
        /// <param name="max">
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<string> GetYears(int min, int max)
        {
            if (min > max)
            {
                throw new Exception("The min parameter must be less than max parameter");
            }

            var years = new List<string>();
            for (var i = min; i <= max; i++)
            {
                years.Add(i.ToString());
            }

            return years;
        }

        /// <summary>
        /// The string to date.
        /// </summary>
        /// <param name="dateStr">
        /// The date str.
        /// </param>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime StringToDate(string dateStr, Calendar calendar)
        {
            var dateParts = dateStr.Split('/');
            var day = int.Parse(dateParts[0]);
            var month = int.Parse(dateParts[1]);
            var year = int.Parse(dateParts[2]);

            if ((!DateTime.IsLeapYear(year)) && (month == 2) && (day == 29))
            {
                month++;
                day = 1;
            }

            var date = new DateTime(year, month, day, calendar);

            return date;
        }

        /// <summary>
        /// The get gregorean date.
        /// </summary>
        /// <param name="dateFormat">
        /// The date format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetGregoreanDate(string dateFormat)
        {
            dateFormat = string.IsNullOrEmpty(dateFormat) ? "dd/MM/yyyy" : dateFormat;
            var egCal = new GregorianCalendar();

            var egCulture = new CultureInfo("ar-EG");
            egCulture.DateTimeFormat.Calendar = egCal;

            return DateTime.Now.ToString(dateFormat, egCulture);
        }

        /// <summary>
        /// The get gregorean date.
        /// </summary>
        /// <param name="dateFormat">
        /// The date format.
        /// </param>
        /// <param name="cultureName">
        /// The culture name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetGregoreanDate(string dateFormat, string cultureName)
        {
            dateFormat = string.IsNullOrEmpty(dateFormat) ? "dd/MM/yyyy" : dateFormat;
            var egCal = new GregorianCalendar();

            var egCulture = new CultureInfo(cultureName);
            egCulture.DateTimeFormat.Calendar = egCal;

            return DateTime.Now.ToString(dateFormat, egCulture);
        }

        /// <summary>
        /// The get um al qura date.
        /// </summary>
        /// <param name="dateFormat">
        /// The date format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetUmAlQuraDate(string dateFormat)
        {
            dateFormat = string.IsNullOrEmpty(dateFormat) ? "dd/MM/yyyy" : dateFormat;
            var arCal = new UmAlQuraCalendar();

            var arCulture = new CultureInfo("ar-SA");
            arCulture.DateTimeFormat.Calendar = arCal;

            return DateTime.Now.ToString(dateFormat, arCulture);
        }

        /// <summary>
        /// Check if day, month, and year present correct date
        /// </summary>
        /// <param name="day">
        /// </param>
        /// <param name="month">
        /// </param>
        /// <param name="year">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsDate(string day, string month, string year)
        {
            try
            {
                var dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if day, month, and year present correct UnAlQura date
        /// </summary>
        /// <param name="day">
        /// </param>
        /// <param name="month">
        /// </param>
        /// <param name="year">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsUmAlquraDate(string day, string month, string year)
        {
            try
            {
                var dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), new UmAlQuraCalendar());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if day, month, and year present correct Greg date
        /// </summary>
        /// <param name="day">
        /// </param>
        /// <param name="month">
        /// </param>
        /// <param name="year">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsGregDate(string day, string month, string year)
        {
            try
            {
                var dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), new GregorianCalendar());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if day, month, and year present correct Greg date
        /// </summary>
        /// <param name="day">
        /// </param>
        /// <param name="month">
        /// </param>
        /// <param name="year">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsHijry(string day, string month, string year)
        {
            try
            {
                var dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), new HijriCalendar());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Replace Hijry month name with its english name
        /// </summary>
        /// <param name="date">
        /// String contains Arabic Hijry month name
        /// </param>
        /// <param name="month">
        /// index of month (start from 1)
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReplaceHijryMonth(string date, int month)
        {
            var arabicMonthNames = GetMonthNames(new UmAlQuraCalendar(), new CultureInfo("ar-SA"));
            var englishMonthNames = GetMonthNames(new UmAlQuraCalendar(), new CultureInfo("en-US"));
            return date.Replace(arabicMonthNames[month - 1], englishMonthNames[month - 1]);
        }

        #endregion
    }
}