using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Central list-name constants for the Smart Suitcase section.
    /// </summary>
    public static class SscListNames
    {
        public const string Hero       = "SscHero";
        public const string Services   = "SscServices";
        public const string Access     = "SscAccess";
        public const string Faq        = "SscFaq";
        public const string Support    = "SscSupport";

        /// <summary>Editors allowed to manage the content. Lives on /ar/ContentAdmin.</summary>
        public const string AdminUsers = "AdminUsers";

        /// <summary>Content lists - these are what ucSscAdmin offers in its picker.</summary>
        public static readonly string[] AllLists = new string[]
        {
            Hero, Services, Access, Faq, Support
        };
    }
}
