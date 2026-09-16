namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls
{
    /// <summary>Central list-name constants for the professional certifications guide.</summary>
    public static class CertListNames
    {
        /// <summary>دليل الشهادات الاحترافية - the certificates themselves.</summary>
        public const string Certifications = "Certifications";

        /// <summary>Editors allowed to manage the content. Lives on /ar/ContentAdmin.</summary>
        public const string AdminUsers = "AdminUsers";

        /// <summary>Content lists - these are what ucCertAdmin offers in its picker.</summary>
        public static readonly string[] AllLists = new string[]
        {
            Certifications
        };

        // AdminUsers is NOT part of AllLists: it lives on a different web
        // (/ar/ContentAdmin) and is provisioned by CertListProvisioner.EnsureAdminUsersList().
    }
}
