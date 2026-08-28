using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    /// <summary>
    /// Internal names for the Central Library SharePoint lists.
    /// </summary>
    public static class ClListNames
    {
        public const string Header      = "ClHeader";
        public const string About       = "ClAbout";
        public const string Numbers     = "ClNumbers";
        public const string Awards      = "ClAwards";
        public const string Services    = "ClServices";
        public const string Collections = "ClCollections";
        public const string Facilities  = "ClFacilities";
        public const string Faq         = "ClFaq";
        public const string Contact     = "ClContact";

        public static readonly string[] All = new string[]
        {
            Header,
            About,
            Numbers,
            Awards,
            Services,
            Collections,
            Facilities,
            Faq,
            Contact
        };
    }
}
