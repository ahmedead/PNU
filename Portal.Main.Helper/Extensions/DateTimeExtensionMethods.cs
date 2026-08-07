// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensionMethods.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The date time extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System;
    using System.Text;

    /// <summary>
    ///     The date time extension methods.
    /// </summary>
    public static class DateTimeExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a nicely formatted duration
        ///     eg. 3 seconds ago, 4 hours ago etc.
        /// </summary>
        /// <param name="dateTime">
        /// The datetime value
        /// </param>
        /// <returns>
        /// A nicely formatted duration
        /// </returns>
        /// <see cref="http://samscode.com/index.php/2009/12/timespan-or-datetime-to-friendly-duration-text-e-g-3-days-ago/"/>
        public static string TimeAgoString(this DateTime dateTime)
        {
            var sb = new StringBuilder();
            var timespan = DateTime.Now - dateTime;

            // A year or more?  Do "[Y] years and [M] months ago"
            if ((int)timespan.TotalDays >= 365)
            {
                // Years
                var nYears = (int)timespan.TotalDays / 365;
                sb.Append(nYears);
                sb.Append(nYears > 1 ? " years" : " year");

                // Months
                var remainingDays = (int)timespan.TotalDays - (nYears * 365);
                var nMonths = remainingDays / 30;
                if (nMonths == 1)
                {
                    sb.Append(" and ").Append(nMonths).Append(" month");
                }
                else if (nMonths > 1)
                {
                    sb.Append(" and ").Append(nMonths).Append(" months");
                }
            }

            // More than 60 days? (appx 2 months or 8 weeks)
            else if ((int)timespan.TotalDays >= 60)
            {
                // Do months
                var nMonths = (int)timespan.TotalDays / 30;
                sb.Append(nMonths).Append(" months");
            }

            // Weeks? (7 days or more)
            else if ((int)timespan.TotalDays >= 7)
            {
                var nWeeks = (int)timespan.TotalDays / 7;
                sb.Append(nWeeks);
                sb.Append(nWeeks == 1 ? " week" : " weeks");
            }

            // Days? (1 or more)
            else if ((int)timespan.TotalDays >= 1)
            {
                var nDays = (int)timespan.TotalDays;
                sb.Append(nDays);
                sb.Append(nDays == 1 ? " day" : " days");
            }

            // Hours?
            else if ((int)timespan.TotalHours >= 1)
            {
                var nHours = (int)timespan.TotalHours;
                sb.Append(nHours);
                sb.Append(nHours == 1 ? " hour" : " hours");
            }

            // Minutes?
            else if ((int)timespan.TotalMinutes >= 1)
            {
                var nMinutes = (int)timespan.TotalMinutes;
                sb.Append(nMinutes);
                sb.Append(nMinutes == 1 ? " minute" : " minutes");
            }

            // Seconds?
            else if ((int)timespan.TotalSeconds >= 1)
            {
                var nSeconds = (int)timespan.TotalSeconds;
                sb.Append(nSeconds);
                sb.Append(nSeconds == 1 ? " second" : " seconds");
            }

            // Just say "1 second" as the smallest unit of time
            else
            {
                sb.Append("1 second");
            }

            sb.Append(" ago");

            // For anything more than 6 months back, put " ([Month] [Year])" at the end, for better reference
            if ((int)timespan.TotalDays >= 30 * 6)
            {
                sb.Append(" (" + dateTime.ToString("MMMM") + " " + dateTime.Year + ")");
            }

            return sb.ToString();
        }

        /// <summary>
        /// The to w 3 c date.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToW3CDate(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("s") + "Z";
        }

        /// <summary>
        /// Returns true if the date is between or equal to one of the two values.
        /// </summary>
        /// <param name="date">
        /// DateTime Base, from where the calculation will be preformed.
        /// </param>
        /// <param name="startDate">
        /// Start date to check for
        /// </param>
        /// <param name="endDate">
        /// End date to check for
        /// </param>
        /// <returns>
        /// boolean value indicating if the date is between or equal to one of the two values
        /// </returns>
        public static bool Between(this DateTime date, DateTime startDate, DateTime endDate)
        {
            var ticks = date.Ticks;
            return ticks >= startDate.Ticks && ticks <= endDate.Ticks;
        }

        /// <summary>
        /// Returns 12:59:59pm time for the date passed.
        ///     Useful for date only search ranges end value
        /// </summary>
        /// <param name="date">
        /// Date to convert
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Returns 12:00am time for the date passed.
        ///     Useful for date only search ranges start value
        /// </summary>
        /// <param name="date">
        /// Date to convert
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime BeginningOfDay(this DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Returns the very end of the given month (the last millisecond of the last hour for the given date)
        /// </summary>
        /// <param name="obj">
        /// DateTime Base, from where the calculation will be preformed.
        /// </param>
        /// <returns>
        /// Returns the very end of the given month (the last millisecond of the last hour for the given date)
        /// </returns>
        public static DateTime EndOfMonth(this DateTime obj)
        {
            return new DateTime(obj.Year, obj.Month, DateTime.DaysInMonth(obj.Year, obj.Month), 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the Start of the given month (the fist millisecond of the given date)
        /// </summary>
        /// <param name="obj">
        /// DateTime Base, from where the calculation will be preformed.
        /// </param>
        /// <returns>
        /// Returns the Start of the given month (the fist millisecond of the given date)
        /// </returns>
        public static DateTime BeginningOfMonth(this DateTime obj)
        {
            return new DateTime(obj.Year, obj.Month, 1, 0, 0, 0, 0);
        }

        #endregion
    }
}