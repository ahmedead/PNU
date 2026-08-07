using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>
    /// Flat DTO used by every Noura Students section control.
    /// Language selection AND HTML encoding happen at projection time
    /// (NsSectionBase.Project), so the ASCX only needs simple Eval() calls
    /// inside the Repeater ItemTemplate - no runat="server" controls, no code blocks.
    /// </summary>
    public class NsCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatValue { get; set; }
        public string ContactValue { get; set; }
        public string BadgeText { get; set; }
        public string RoleText { get; set; }
        public string SubTitle { get; set; }
        public string Category { get; set; }
        public string IconClass { get; set; }
        public string LinkUrl { get; set; }
        public string ButtonText { get; set; }
        public string ImageUrl { get; set; }
        public int ItemOrder { get; set; }

        /// <summary>Raw date, used for grouping and sorting.</summary>
        public DateTime? EventDate { get; set; }

        /// <summary>Pre-formatted date parts for the "تواريخ تهمك" badge.</summary>
        public string DateIso { get; set; }
        public string DateDay { get; set; }
        public string DateMonth { get; set; }

        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool HasImage { get { return !string.IsNullOrEmpty(ImageUrl); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
        public bool HasDate { get { return EventDate.HasValue; } }

        // ---- pre-rendered fragments (values are already HTML-encoded) ---------------

        public string BadgeHtml
        {
            get
            {
                if (string.IsNullOrEmpty(BadgeText)) return string.Empty;
                return "<div class=\"d-flex flex-wrap mt-auto gap-2\">"
                     + "<span class=\"badge badge-info\">" + BadgeText + "</span></div>";
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

        /// <summary>Primary CTA button, or empty string.</summary>
        public string ButtonHtml
        {
            get
            {
                if (!HasLink || string.IsNullOrEmpty(ButtonText)) return string.Empty;
                return "<div class=\"d-flex gap-3\">"
                     + "<a target=\"_blank\" rel=\"noopener\" class=\"btn btn-primary\" href=\"" + LinkUrl + "\">"
                     + ButtonText + "</a></div>";
            }
        }

        /// <summary>Arrow "read more" button used by nav cards, or empty string.</summary>
        public string ArrowLinkHtml
        {
            get
            {
                if (!HasLink) return string.Empty;
                return "<div class=\"d-flex justify-content-end mt-auto\">"
                     + "<a class=\"btn btn-secondary stretched-link\" href=\"" + LinkUrl + "\" aria-label=\"" + Title + "\">"
                     + "<i class=\"hgi hgi-stroke hgi-arrow-left-02 fs-4\" aria-hidden=\"true\"></i></a></div>";
            }
        }

        /// <summary>"nav-card" when the card is clickable, otherwise empty.</summary>
        public string NavCardCss { get { return HasLink ? "nav-card" : string.Empty; } }

        // ---- carousel state (set by the control before binding) --------------------

        /// <summary>Zero-based slide position, used by the carousel indicators.</summary>
        public int SlideIndex { get; set; }

        /// <summary>True for the slide that must carry the "active" class.</summary>
        public bool IsActiveSlide { get; set; }

        /// <summary>Total slide count, used to build "1 من 2".</summary>
        public int SlideCount { get; set; }

        /// <summary>Localized slide label, e.g. "1 من 2". Set by the control.</summary>
        public string SlideAriaLabel { get; set; }

        /// <summary>Localized indicator label, e.g. "القصة 1: سارة العنزي". Set by the control.</summary>
        public string IndicatorAriaLabel { get; set; }

        public string SlideCss { get { return IsActiveSlide ? "carousel-item active" : "carousel-item"; } }

        public string IndicatorCss { get { return IsActiveSlide ? "active" : string.Empty; } }

        /// <summary>Renders aria-current only on the active indicator.</summary>
        public string IndicatorAriaCurrent { get { return IsActiveSlide ? "aria-current=\"true\"" : string.Empty; } }

        /// <summary>Portrait image for a student story, or empty string.</summary>
        public string PortraitHtml
        {
            get
            {
                if (!HasImage) return string.Empty;
                return "<div class=\"rounded-3 reasons-carousel\">"
                     + "<picture class=\"carousel-media\">"
                     + "<img class=\"carousel-image w-100\" src=\"" + ImageUrl + "\" alt=\"" + Title + "\" "
                     + "loading=\"lazy\" decoding=\"async\" />"
                     + "</picture></div>";
            }
        }

        /// <summary>Contact value, wrapped in an anchor when a link is configured.</summary>
        public string ContactValueHtml
        {
            get
            {
                if (!HasLink) return ContactValue;
                return "<a href=\"" + LinkUrl + "\">" + ContactValue + "</a>";
            }
        }

        public string RoleHtml
        {
            get
            {
                if (string.IsNullOrEmpty(RoleText)) return string.Empty;
                return "<p class=\"text-primary fw-semibold\">" + RoleText + "</p>";
            }
        }
    }

    /// <summary>
    /// One column of the "تواريخ تهمك" section: a category heading plus its rows.
    /// Built in the code-behind before binding, so the markup stays declarative.
    /// </summary>
    public class NsDateGroup
    {
        public string Title { get; set; }
        public System.Collections.Generic.List<NsCard> Items { get; set; }

        public NsDateGroup() { Items = new System.Collections.Generic.List<NsCard>(); }
    }
}
