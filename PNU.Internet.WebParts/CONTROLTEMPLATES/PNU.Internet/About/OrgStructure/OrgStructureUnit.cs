using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About.OrgStructure
{
    /// <summary>
    /// DTO for a single organizational unit rendered on the org-structure chart.
    /// The <see cref="Selector"/> is the join key that pairs a list item with a
    /// fixed &lt;rect&gt; element inside the SVG (matches the CSS selector used by
    /// the client script). Content columns are bilingual (Arabic + English).
    /// </summary>
    public class OrgStructureUnit
    {
        public int Id { get; set; }

        /// <summary>Sort order on the chart / in the admin grid.</summary>
        public int Order { get; set; }

        /// <summary>CSS selector matching the SVG rect, e.g. rect[x="482.5"][y="79.5"][width="280"][height="56"].</summary>
        public string Selector { get; set; }

        public string Title { get; set; }
        public string TitleEn { get; set; }

        public string Description { get; set; }
        public string DescriptionEn { get; set; }

        public string Badge { get; set; }
        public string BadgeEn { get; set; }

        public string Meta { get; set; }
        public string MetaEn { get; set; }

        /// <summary>hgi icon class, e.g. hgi-hierarchy.</summary>
        public string Icon { get; set; }

        /// <summary>Visual theme token used by the client (primary, sa, saSoft, saMuted).</summary>
        public string Theme { get; set; }

        /// <summary>Optional details page URL. Empty means "no details page".</summary>
        public string LinkUrl { get; set; }

        public bool Active { get; set; }

        /// <summary>Localized title resolved from the current UI language.</summary>
        public string LocalizedTitle(bool isArabic)
        {
            return isArabic ? Title : (string.IsNullOrWhiteSpace(TitleEn) ? Title : TitleEn);
        }

        public string LocalizedDescription(bool isArabic)
        {
            return isArabic ? Description : (string.IsNullOrWhiteSpace(DescriptionEn) ? Description : DescriptionEn);
        }

        public string LocalizedBadge(bool isArabic)
        {
            return isArabic ? Badge : (string.IsNullOrWhiteSpace(BadgeEn) ? Badge : BadgeEn);
        }

        public string LocalizedMeta(bool isArabic)
        {
            return isArabic ? Meta : (string.IsNullOrWhiteSpace(MetaEn) ? Meta : MetaEn);
        }
    }
}
