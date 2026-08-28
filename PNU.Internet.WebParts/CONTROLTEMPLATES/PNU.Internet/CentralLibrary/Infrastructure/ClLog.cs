using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    /// <summary>
    /// Centralised error logging for Central Library user controls.
    /// Delegates to Publics.WriteToLog safely without throwing exceptions.
    /// </summary>
    public static class ClLog
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
