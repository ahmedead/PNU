using Portal.Main.Helper.Utils;
using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes
{
    /// <summary>
    /// Logging helper for About PNU.
    /// </summary>
    public static class AboutPnuLog
    {
        public static void Write(string location, Exception ex)
        {
            try
            {
                string url = GetCurrentUrl();
                string message = ex != null ? (ex.Message + " | " + ex.StackTrace) : "Unknown exception";
                Publics.WriteToLog(url, location ?? "AboutPNU", message);
            }
            catch
            {
                // swallow logging failures
            }
        }

        public static void Write(string location, string message)
        {
            try
            {
                string url = GetCurrentUrl();
                Publics.WriteToLog(url, location ?? "AboutPNU", message ?? string.Empty);
            }
            catch
            {
                // swallow logging failures
            }
        }

        private static string GetCurrentUrl()
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                {
                    return HttpContext.Current.Request.Url.ToString();
                }
            }
            catch { }
            return "AboutPNU";
        }
    }
}
