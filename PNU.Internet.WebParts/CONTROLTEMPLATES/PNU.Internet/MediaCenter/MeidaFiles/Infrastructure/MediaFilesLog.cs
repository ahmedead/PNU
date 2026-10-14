using System;
using System.Web;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.Infrastructure
{
    public static class MediaFilesLog
    {
        public static void Write(string source, string message)
        {
            try
            {
                string url = string.Empty;
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                        url = HttpContext.Current.Request.Url.ToString();
                }
                catch { }

                Publics.WriteToLog(url, "MediaFiles." + source, message);
            }
            catch { }
        }

        public static void Write(string source, Exception ex)
        {
            if (ex == null) return;
            Write(source, ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : ""));
        }
    }
}
