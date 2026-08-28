using System;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public static class PcLog
    {
        public static void Write(string context, Exception ex)
        {
            try
            {
                string msg = string.Format("[ProfessionalCertifications] {0}: {1}", context, ex != null ? ex.ToString() : "Unknown error");
                Publics.WriteToLog(msg);
            }
            catch { }
        }

        public static void Write(string context, string message)
        {
            try
            {
                string msg = string.Format("[ProfessionalCertifications] {0}: {1}", context, message);
                Publics.WriteToLog(msg);
            }
            catch { }
        }
    }
}
