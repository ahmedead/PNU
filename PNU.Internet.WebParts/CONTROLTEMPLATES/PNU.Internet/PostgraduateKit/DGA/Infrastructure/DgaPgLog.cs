using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    /// <summary>
    /// Isolated logger for DGA Postgraduate Kit.
    /// </summary>
    public static class DgaPgLog
    {
        public static void Write(string context, Exception ex)
        {
            if (ex == null) return;
            string url = string.Empty;
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    url = HttpContext.Current.Request.Url != null ? HttpContext.Current.Request.Url.ToString() : string.Empty;
                }
            }
            catch { }

            try
            {
                Publics.WriteToLog(url, "DgaPostgraduateKit:" + context, ex.Message + " | " + ex.StackTrace);
            }
            catch
            {
                try
                {
                    System.Diagnostics.Trace.TraceError("[DgaPostgraduateKit] {0}: {1}", context, ex);
                }
                catch { }
            }
        }
    }
}
