using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts
{
    public static class DateTimeExtensions
    {
        public static DateTime AddWorkdays(this DateTime originalDate, int workDays)
        {
            try
            {
                DateTime tmpDate = originalDate;
                while (workDays > 0)
                {
                    tmpDate = tmpDate.AddDays(1);
                    if (tmpDate.DayOfWeek != DayOfWeek.Saturday &&
                        tmpDate.DayOfWeek != DayOfWeek.Friday &&
                        !tmpDate.IsHoliday())
                        workDays--;
                }
                return tmpDate;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"DateTimeExtensions - AddWorkdays", ex.Message);
            }
            return originalDate;
        }

        public static bool IsHoliday(this DateTime originalDate)
        {
            // INSERT YOUR HOlIDAY-CODE HERE!
            return false;
        }
    }
}
