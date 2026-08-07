using System;
using System.Web;

// NOTE: this is the ONLY file that references the portal logger.
// If Publics does not resolve, add the correct using here (e.g. Portal.Main.Helper)
// and every control in this folder is fixed at once.
// using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>
    /// Error logging for the Noura Students controls.
    /// Wraps the portal logger call:
    ///     Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), UserControlName, ex.Message);
    /// Logging must never break a page, so every path here is exception-safe.
    /// </summary>
    public static class NsLog
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
            catch { /* no request context (feature receiver, timer job) */ }

            return string.Empty;
        }
    }
}
