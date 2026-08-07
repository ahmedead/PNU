using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>Central list-name constants for the Contact / تواصل (Tawasul) page.</summary>
    public static class TwListNames
    {
        /// <summary>Section headings and subtitles for every section of the page.</summary>
        public const string SectionTitles   = "TwSectionTitles";

        /// <summary>Phone / e-mail / link rows of the two top cards, grouped by ContactGroup.</summary>
        public const string ContactChannels = "TwContactChannels";

        /// <summary>Digital platform links (X, Instagram, LinkedIn, YouTube).</summary>
        public const string SocialLinks     = "TwSocialLinks";

        /// <summary>Internal university entities grid (الجهات داخل الجامعة).</summary>
        public const string Entities        = "TwEntities";

        /// <summary>National address + short code + map. One item, reused by two sections.</summary>
        public const string Location        = "TwLocation";

        /// <summary>Editors allowed to manage the content. Lives on /ar/ContentAdmin.</summary>
        public const string AdminUsers      = "AdminUsers";

        /// <summary>Content lists - these are what ucTwAdmin offers in its picker.</summary>
        public static readonly string[] AllLists = new string[]
        {
            SectionTitles, ContactChannels, SocialLinks, Entities, Location
        };

        // AdminUsers is NOT part of AllLists: it lives on a different web
        // (/ar/ContentAdmin) and is provisioned by TwListProvisioner.EnsureAdminUsersList().
    }
}
