using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About
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

        /// <summary>
        /// CSS color for the box label text (e.g. "var(--dga-primary-950)").
        /// Reproduces the original glyph color so the overlay label matches the design.
        /// </summary>
        public string TextColor { get; set; }

        /// <summary>Optional details page URL. Empty means "no details page".</summary>
        public string LinkUrl { get; set; }

        public bool Active { get; set; }

        // ---- Geometry parsed from Selector (rect[x="..."][y="..."][width="..."][height="..."]) ----

        private static double ParseSelectorPart(string selector, string part)
        {
            if (string.IsNullOrEmpty(selector)) return 0;
            var m = System.Text.RegularExpressions.Regex.Match(
                selector, "\\[" + part + "=\"([0-9.]+)\"\\]");
            double v;
            return m.Success && double.TryParse(m.Groups[1].Value,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        public double BoxX { get { return ParseSelectorPart(Selector, "x"); } }
        public double BoxY { get { return ParseSelectorPart(Selector, "y"); } }
        public double BoxW { get { return ParseSelectorPart(Selector, "width"); } }
        public double BoxH { get { return ParseSelectorPart(Selector, "height"); } }

        /// <summary>SVG viewBox dimensions the coordinates are expressed in.</summary>
        public const double ViewBoxW = 1237.0;
        public const double ViewBoxH = 1650.0;

        /// <summary>Absolute-position style (percentages) for the overlay label, matching the box.</summary>
        public string OverlayStyle(bool isArabic)
        {
            var ci = System.Globalization.CultureInfo.InvariantCulture;
            double left = BoxX / ViewBoxW * 100.0;
            double top = BoxY / ViewBoxH * 100.0;
            double w = BoxW / ViewBoxW * 100.0;
            double h = BoxH / ViewBoxH * 100.0;
            string color = string.IsNullOrEmpty(TextColor) ? "var(--dga-primary-950)" : TextColor;
            return string.Format(ci,
                "left:{0:0.###}%;top:{1:0.###}%;width:{2:0.###}%;height:{3:0.###}%;color:{4};",
                left, top, w, h, color);
        }

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
