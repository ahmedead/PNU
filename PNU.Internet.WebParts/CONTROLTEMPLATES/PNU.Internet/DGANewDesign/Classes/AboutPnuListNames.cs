using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes
{
    /// <summary>
    /// Central list-name constants for the About PNU (عن الجامعة) page.
    /// Lists are hosted under /ar/AboutUniversity/
    /// </summary>
    public static class AboutPnuListNames
    {
        public const string Overview = "AboutPnuOverview";
        public const string Milestones = "AboutPnuMilestones";
        public const string Pillars = "AboutPnuPillars";

        /// <summary>All lists managed by About PNU.</summary>
        public static readonly string[] All = new string[]
        {
            Overview,
            Milestones,
            Pillars
        };
    }
}
