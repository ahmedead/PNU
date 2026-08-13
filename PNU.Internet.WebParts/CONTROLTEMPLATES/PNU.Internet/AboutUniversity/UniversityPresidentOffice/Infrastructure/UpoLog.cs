using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    /// <summary>
    /// Error logging for the University President Office controls.
    /// Wraps the portal logger call. Logging must never break a page,
    /// so every path here is exception-safe.
    /// </summary>
    public static class UpoLog
    {
        public static void Write(string userControlName, Exception ex)
        {
            Write(userControlName, ex == null ? "(no exception)" : ex.Message);
        }

        public static void Write(string userControlName, string message)
        {
            try
            {
                Publics.WriteToLog(CurrentUrl(), userControlName, message);
            }
            catch { /* logging must never throw */ }
        }

        private static string CurrentUrl()
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null && ctx.Request != null && ctx.Request.Url != null)
                    return ctx.Request.Url.ToString();
            }
            catch { /* no request context */ }

            return string.Empty;
        }
    }
}
