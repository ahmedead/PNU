using System.Collections.Generic;
using System.Text;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Flat DTO used by every Smart Suitcase section control.
    /// Language selection AND HTML encoding happen at projection time (SscSectionBase.Project).
    /// </summary>
    public class SscCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public string LinkUrl { get; set; }
        public string ButtonText { get; set; }
        public string ContactValue { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string Badge1 { get; set; }
        public string Badge2 { get; set; }
        public int ItemOrder { get; set; }

        public List<string> Bullets { get; set; }

        public SscCard()
        {
            Bullets = new List<string>();
        }

        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
        public bool HasImageUrl { get { return !string.IsNullOrEmpty(ImageUrl); } }

        public string NavCardCss { get { return HasLink ? "nav-card" : string.Empty; } }

        public string ArrowLinkHtml
        {
            get
            {
                if (!HasLink) return string.Empty;
                string arrowIcon = SscHelper.IsArabic ? "hgi-arrow-left-02" : "hgi-arrow-right-02";
                return "<div class=\"d-flex justify-content-end mt-auto\">"
                     + "<a class=\"btn btn-secondary stretched-link\" href=\"" + LinkUrl + "\" target=\"_blank\" rel=\"external noopener noreferrer\" aria-label=\"" + Title + "\">"
                     + "<span>" + (string.IsNullOrEmpty(ButtonText) ? Title : ButtonText) + "</span> "
                     + "<i class=\"hgi hgi-stroke " + arrowIcon + " fs-4\" aria-hidden=\"true\"></i></a></div>";
            }
        }
    }
}
