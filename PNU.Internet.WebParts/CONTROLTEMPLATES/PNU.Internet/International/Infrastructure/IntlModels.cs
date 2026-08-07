using System.Collections.Generic;
using System.Text;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Flat DTO used by every International section control.
    /// All language selection AND HTML encoding happen at projection time
    /// (IntlSectionBase.Project), so the ASCX only needs simple Eval() calls
    /// inside the Repeater ItemTemplate - no runat="server" controls, no code blocks.
    /// </summary>
    public class IntlCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatValue { get; set; }
        public string ContactValue { get; set; }
        public string IconClass { get; set; }
        public string LinkUrl { get; set; }
        public string ButtonText { get; set; }
        public int ItemOrder { get; set; }

        public List<string> Bullets { get; set; }

        public IntlCard() { Bullets = new List<string>(); }

        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool HasBullets { get { return Bullets != null && Bullets.Count > 0; } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }

        // ---- pre-rendered fragments (values are already HTML-encoded) ---------------

        /// <summary>"nav-card" when the card is clickable, otherwise empty.</summary>
        public string NavCardCss { get { return HasLink ? "nav-card" : string.Empty; } }

        /// <summary>&lt;ul&gt; of bullets, or empty string.</summary>
        public string BulletsHtml
        {
            get
            {
                if (!HasBullets) return string.Empty;
                var sb = new StringBuilder("<ul class=\"mb-0 ps-3\">");
                foreach (string b in Bullets) sb.Append("<li>").Append(b).Append("</li>");
                return sb.Append("</ul>").ToString();
            }
        }

        /// <summary>Bullets when present, otherwise the description paragraph.</summary>
        public string BodyHtml
        {
            get
            {
                if (HasBullets) return BulletsHtml;
                if (HasDescription) return "<p class=\"card-text\">" + Description + "</p>";
                return string.Empty;
            }
        }

        /// <summary>Arrow "read more" button for nav cards, or empty string.</summary>
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

        /// <summary>Primary CTA button for document cards, or empty string.</summary>
        public string ButtonHtml
        {
            get
            {
                if (!HasLink || string.IsNullOrEmpty(ButtonText)) return string.Empty;
                return "<div class=\"d-flex gap-3 flex-wrap mt-auto\">"
                     + "<a class=\"btn btn-primary\" href=\"" + LinkUrl + "\">" + ButtonText + "</a></div>";
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
    }
}
