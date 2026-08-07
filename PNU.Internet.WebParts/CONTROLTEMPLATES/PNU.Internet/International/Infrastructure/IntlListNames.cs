using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Central list-name constants for the International Students section.
    /// </summary>
    public static class IntlListNames
    {
        public const string Numbers       = "IntlNumbers";
        public const string Features      = "IntlFeatures";
        public const string Admission     = "IntlAdmission";
        public const string BeforeArrival = "IntlBeforeArrival";
        public const string OnArrival     = "IntlOnArrival";
        public const string Office        = "IntlOffice";
        public const string Activities    = "IntlActivities";
        public const string Exchange      = "IntlExchange";
        public const string Documents     = "IntlDocuments";
        public const string Contact       = "IntlContact";

        /// <summary>Editors allowed to manage the content. Lives on /ar/ContentAdmin.</summary>
        public const string AdminUsers    = "AdminUsers";

        /// <summary>Content lists - these are what ucIntlAdmin offers in its picker.</summary>
        public static readonly string[] AllLists = new string[]
        {
            Numbers, Features, Admission, BeforeArrival, OnArrival,
            Office, Activities, Exchange, Documents, Contact
        };

        // AdminUsers is NOT part of AllLists: it lives on a different web
        // (/ar/ContentAdmin) and is provisioned by InternationalListProvisioner.EnsureAdminUsersList().
    }
}
