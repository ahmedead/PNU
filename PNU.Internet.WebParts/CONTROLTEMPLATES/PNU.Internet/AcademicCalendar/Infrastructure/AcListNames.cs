using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls
{
    /// <summary>Central list-name constants for the Academic Calendar (التقويم الأكاديمي) page.</summary>
    public static class AcListNames
    {
        /// <summary>
        /// Single content list holding every academic-calendar row (Arabic + English).
        /// The three accordion tables (first / second / summer semester) are the values
        /// of the Semester choice column on this one list - mirroring how NsImportantDates
        /// groups one list by DateCategory.
        /// </summary>
        public const string Calendar = "AcCalendar";

        /// <summary>
        /// Section header text (title / intro / external link) in Arabic AND English.
        /// One row drives the H2 heading, the intro paragraph and the MOE button.
        /// </summary>
        public const string Headers = "AcCalendarHeaders";

        /// <summary>Editors allowed to manage the content. Lives on /ar/ContentAdmin.</summary>
        public const string AdminUsers = "AdminUsers";

        /// <summary>Content lists - these are what ucAcAdmin offers in its picker.</summary>
        public static readonly string[] AllLists = new string[]
        {
            Calendar, Headers
        };

        // AdminUsers is NOT part of AllLists: it lives on a different web
        // (/ar/ContentAdmin) and is provisioned by AcListProvisioner.EnsureAdminUsersList().
    }
}
