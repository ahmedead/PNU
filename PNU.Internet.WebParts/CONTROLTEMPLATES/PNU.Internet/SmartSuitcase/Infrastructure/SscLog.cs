using Microsoft.SharePoint.Administration;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Error logging for the Smart Suitcase controls.
    /// Writes to ULS through SPDiagnosticsService.
    /// </summary>
    public static class SscLog
    {
        private const string CategoryName = "PNU Smart Suitcase";

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
