using Microsoft.SharePoint.Administration;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Error logging for the International Students controls.
    ///
    /// Writes to ULS through SPDiagnosticsService, so it has no dependency on any
    /// other project helper. If you want these entries to go to the portal's own
    /// log instead, set <see cref="ExternalLogger"/> once at start-up, e.g.:
    ///
    ///     IntlLog.ExternalLogger = (source, ex) =&gt; Publics.WriteToLog(source, ex);
    ///
    /// Logging must never break a page, so every path here is exception-safe.
    /// </summary>
    public static class IntlLog
    {
        private const string CategoryName = "PNU International Students";

        /// <summary>Optional hook to forward entries to the portal logger.</summary>
        public static Action<string, Exception> ExternalLogger { get; set; }

        public static void Write(string source, Exception ex)
        {
            try
            {
                if (ExternalLogger != null)
                {
                    ExternalLogger(source, ex);
                    return;
                }

                WriteToUls(source, ex == null ? "(no exception)" : ex.ToString());
            }
            catch { /* logging must never throw */ }
        }

        public static void Write(string source, string message)
        {
            try
            {
                if (ExternalLogger != null)
                {
                    ExternalLogger(source, new Exception(message));
                    return;
                }

                WriteToUls(source, message);
            }
            catch { /* logging must never throw */ }
        }

        private static void WriteToUls(string source, string message)
        {
            SPDiagnosticsService service = SPDiagnosticsService.Local;
            if (service == null) return;

            var category = new SPDiagnosticsCategory(CategoryName, TraceSeverity.Unexpected, EventSeverity.Error);

            service.WriteTrace(0, category, TraceSeverity.Unexpected,
                               "{0} :: {1}", new object[] { source, message });
        }
    }
}
