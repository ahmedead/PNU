using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>Central list-name constants for the Noura Students (طالبات نورة) page.</summary>
    public static class NsListNames
    {
        public const string Numbers          = "NsNumbers";
        public const string Awards           = "NsAwards";
        public const string AcademicServices = "NsAcademicServices";
        public const string QuickLinks       = "NsQuickLinks";
        public const string StudentServices  = "NsStudentServices";
        public const string ImportantDates   = "NsImportantDates";
        public const string CampusLife       = "NsCampusLife";
        public const string Career           = "NsCareer";
        public const string Experiences      = "NsExperiences";
        public const string FinancialSupport = "NsFinancialSupport";
        public const string Contact          = "NsContact";

        /// <summary>Editors allowed to manage the content. Lives on /ar/ContentAdmin.</summary>
        public const string AdminUsers       = "AdminUsers";

        /// <summary>Content lists - these are what ucNsAdmin offers in its picker.</summary>
        public static readonly string[] AllLists = new string[]
        {
            Numbers, Awards, AcademicServices, QuickLinks, StudentServices,
            ImportantDates, CampusLife, Career, Experiences, FinancialSupport, Contact
        };

        // AdminUsers is NOT part of AllLists: it lives on a different web
        // (/ar/ContentAdmin) and is provisioned by NsListProvisioner.EnsureAdminUsersList().
    }
}
