using System;
using System.Web;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    /// <summary>
    /// Logging abstraction for the Herbarium controls.
    /// Routes all exception and info logging to Portal.Main.Helper.Publics.WriteToLog.
    /// </summary>
    public static class HerbariumLog
    {
        public static void Write(string context, Exception ex)
        {
            if (ex == null) return;
            Write(context, ex.Message);
        }

        public static void Write(string context, string message)
        {
            try
            {
                string url = HttpContext.Current != null && HttpContext.Current.Request != null
                    ? HttpContext.Current.Request.Url.ToString()
                    : "HerbariumControl";

                string controlName = "Herbarium:" + (context ?? "General");
                Publics.WriteToLog(url, controlName, message ?? string.Empty);
            }
            catch { }
        }
    }
}
