using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// Flat DTO used by every Agency section control.
    /// Language selection AND HTML encoding happen at projection time
    /// (AgSectionBase.Project), so the ASCX only needs simple Eval() calls
    /// inside the Repeater ItemTemplate - no runat="server" controls, no code blocks.
    /// </summary>
    public class AgCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RoleText { get; set; }
        public string SubTitle { get; set; }
        /// <summary>TaskGroup choice value - kept RAW (not encoded) so grouping can match it.</summary>
        public string Category { get; set; }
        public string IconClass { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlMd { get; set; }
        public string ColumnClass { get; set; }
        public int ItemOrder { get; set; }

        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool HasImage { get { return !string.IsNullOrEmpty(ImageUrl); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }

        // ---- pre-rendered fragments (values are already HTML-encoded) ---------------

        /// <summary>Bootstrap column classes, with a sensible default for a half-width card.</summary>
        public string ColumnCss
        {
            get
            {
                return string.IsNullOrEmpty(ColumnClass)
                    ? "col-12 col-md-6 col-sm-12"
                    : ColumnClass;
            }
        }

        public string DescriptionHtml
        {
            get
            {
                if (!HasDescription) return string.Empty;
                return "<p class=\"card-text\">" + Description + "</p>";
            }
        }

        public string RoleHtml
        {
            get
            {
                if (string.IsNullOrEmpty(RoleText)) return string.Empty;
                return "<h3 class=\"card-title\">" + RoleText + "</h3>";
            }
        }

        public string SubTitleHtml
        {
            get
            {
                if (string.IsNullOrEmpty(SubTitle)) return string.Empty;
                return "<p class=\"card-text mb-0\">" + SubTitle + "</p>";
            }
        }

        /// <summary>
        /// Responsive hero &lt;picture&gt; for the overview section, or empty string.
        /// AVIF variants are derived by swapping the extension; the browser falls back to
        /// the WebP source and finally to the &lt;img src&gt; when they are not published.
        /// </summary>
        public string PictureHtml
        {
            get
            {
                if (!HasImage) return string.Empty;

                string sm = ImageUrl;
                string md = string.IsNullOrEmpty(ImageUrlMd) ? sm : ImageUrlMd;
                string smAvif = AgHelper.Enc(AgHelper.SwapExtension(RawSm, ".avif"));
                string mdAvif = AgHelper.Enc(AgHelper.SwapExtension(RawMd, ".avif"));

                return "<picture class=\"rounded-3\">"
                     + "<source class=\"rounded-3\" type=\"image/avif\" media=\"(min-width: 1200px)\" srcset=\"" + mdAvif + "\" />"
                     + "<source class=\"rounded-3\" type=\"image/avif\" media=\"(min-width: 768px)\" srcset=\"" + mdAvif + "\" />"
                     + "<source class=\"rounded-3\" type=\"image/avif\" srcset=\"" + smAvif + "\" />"
                     + "<source class=\"rounded-3\" type=\"image/webp\" media=\"(min-width: 1200px)\" srcset=\"" + md + "\" />"
                     + "<source class=\"rounded-3\" type=\"image/webp\" media=\"(min-width: 768px)\" srcset=\"" + md + "\" />"
                     + "<source type=\"image/webp\" srcset=\"" + sm + "\" />"
                     + "<img class=\"carousel-image w-100 rounded-3\" src=\"" + sm + "\" alt=\"" + Title + "\" "
                     + "loading=\"eager\" fetchpriority=\"high\" decoding=\"async\" />"
                     + "</picture>";
            }
        }

        /// <summary>Un-encoded copies of the image URLs, used only to derive the .avif names.</summary>
        internal string RawSm { get; set; }
        internal string RawMd { get; set; }
    }

    /// <summary>
    /// One accordion item of the "المهام" section: a group heading plus its rows.
    /// Built in the code-behind before binding, so the markup stays declarative.
    /// </summary>
    public class AgTaskGroup
    {
        public string Title { get; set; }
        public int Index { get; set; }
        public List<AgCard> Items { get; set; }

        public AgTaskGroup() { Items = new List<AgCard>(); }

        public string HeadingId { get { return "agency-main-tasks-heading-" + (Index + 1); } }
        public string CollapseId { get { return "agency-main-tasks-collapse-" + (Index + 1); } }
        public string CollapseTarget { get { return "#" + CollapseId; } }
        public bool IsFirst { get { return Index == 0; } }

        public string ButtonCss { get { return IsFirst ? "accordion-button" : "accordion-button collapsed"; } }
        public string CollapseCss { get { return IsFirst ? "accordion-collapse collapse show" : "accordion-collapse collapse"; } }
        public string AriaExpanded { get { return IsFirst ? "true" : "false"; } }

        /// <summary>
        /// A single row renders as a paragraph; two or more render as a bullet list.
        /// Values are already HTML-encoded at projection time.
        /// </summary>
        public string BodyHtml
        {
            get
            {
                if (Items == null || Items.Count == 0) return string.Empty;
                if (Items.Count == 1) return "<p class=\"mb-0\">" + Items[0].Title + "</p>";

                var sb = new System.Text.StringBuilder();
                sb.Append("<ul class=\"mb-0\">");
                foreach (AgCard row in Items) sb.Append("<li>").Append(row.Title).Append("</li>");
                sb.Append("</ul>");
                return sb.ToString();
            }
        }
    }
}
