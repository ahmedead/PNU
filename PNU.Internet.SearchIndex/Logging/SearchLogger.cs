using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;

namespace PNU.Internet.SearchIndex.Logging
{
    /// <summary>
    /// Writes to the SharePoint ULS log instead of a custom log list.
    /// Use:
    ///   SearchLogger.WriteToLog("Crawler", "key", "message");
    ///
    /// View entries with ULSViewer (filter Product = "PNU Custom Search")
    /// or PowerShell:
    ///   Get-SPLogEvent | Where-Object { $_.Area -eq "PNU Custom Search" }
    /// </summary>
    public static class SearchLogger
    {
        private static PNUDiagnosticsService Service
        {
            get { return PNUDiagnosticsService.Local; }
        }

        public static void WriteToLog(string category, string key, string message)
        {
            try
            {
                string formatted = string.Format("[{0}] [{1}] {2}",
                    category ?? "", key ?? "", message ?? "");

                Service.WriteTrace(0,
                    Service.GetCategory(category),
                    TraceSeverity.Unexpected,
                    formatted);
            }
            catch { /* never let logging break the caller */ }
        }

        public static void WriteInfo(string category, string key, string message)
        {
            try
            {
                string formatted = string.Format("[{0}] [{1}] {2}",
                    category ?? "", key ?? "", message ?? "");

                Service.WriteTrace(0,
                    Service.GetCategory(category),
                    TraceSeverity.Medium,
                    formatted);
            }
            catch { }
        }
    }

    /// <summary>
    /// Registers the "PNU Custom Search" diagnostic area + its categories
    /// with SharePoint's ULS infrastructure. Auto-created on first use.
    /// </summary>
    [System.Runtime.InteropServices.GuidAttribute(
        "9A6E5D2B-1C3F-4A7E-B2D4-A9F8C7E61234")]
    public class PNUDiagnosticsService : SPDiagnosticsServiceBase
    {
        public const string AREA_NAME = "PNU Custom Search";

        private static PNUDiagnosticsService _local;
        public static PNUDiagnosticsService Local
        {
            get
            {
                if (_local == null)
                {
                    _local = SPFarm.Local.Services.GetValue<PNUDiagnosticsService>();
                    if (_local == null)
                    {
                        _local = new PNUDiagnosticsService();
                        _local.Update();
                    }
                }
                return _local;
            }
        }

        public PNUDiagnosticsService()
            : base("PNU Custom Search Diagnostics", SPFarm.Local) { }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            yield return new SPDiagnosticsArea(AREA_NAME, new[]
            {
                new SPDiagnosticsCategory("General",       TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("Indexer",       TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("Crawler",       TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("EventReceiver", TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("TimerJob",      TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("Provisioning",  TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("DAL",           TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("Tasks",         TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("UI",            TraceSeverity.Medium, EventSeverity.Information)
            });
        }

        public SPDiagnosticsCategory GetCategory(string name)
        {
            try
            {
                var area = this.Areas[AREA_NAME];
                if (area != null && !string.IsNullOrEmpty(name))
                {
                    foreach (var cat in area.Categories)
                    {
                        if (string.Equals(cat.Name, name,
                                StringComparison.OrdinalIgnoreCase))
                            return cat;
                    }
                    // fallback to General
                    foreach (var cat in area.Categories)
                        if (cat.Name == "General") return cat;
                }
            }
            catch { }
            return null;
        }
    }
}
