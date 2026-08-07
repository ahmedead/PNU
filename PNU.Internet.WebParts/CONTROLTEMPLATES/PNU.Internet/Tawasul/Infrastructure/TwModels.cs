using System;
using System.Text;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// Flat DTO used by every Tawasul section control.
    /// Language selection AND HTML encoding happen at projection time
    /// (TwSectionBase.Project), so the ASCX only needs simple Eval() calls inside the
    /// Repeater ItemTemplate - no runat="server" controls, no inline code blocks.
    /// Every string property below is ALREADY HTML-encoded.
    /// </summary>
    public class TwCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubTitle { get; set; }
        public string ContactValue { get; set; }
        /// <summary>Raw grouping / section key (NOT encoded) so filtering can match it.</summary>
        public string Category { get; set; }
        public string IconClass { get; set; }
        public string LinkUrl { get; set; }
        public string Phone { get; set; }
        public string PhoneLink { get; set; }
        public string Email { get; set; }
        public string ShortCode { get; set; }
        public string MapUrl { get; set; }
        public int ItemOrder { get; set; }

        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool HasPhone { get { return !string.IsNullOrEmpty(Phone); } }
        public bool HasEmail { get { return !string.IsNullOrEmpty(Email); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
        public bool HasShortCode { get { return !string.IsNullOrEmpty(ShortCode); } }
        public bool HasMap { get { return !string.IsNullOrEmpty(MapUrl); } }
        public bool HasSubTitle { get { return !string.IsNullOrEmpty(SubTitle); } }

        // ---- pre-rendered fragments (values are already HTML-encoded) ---------------

        /// <summary>
        /// Body of a top-card contact row: a link, a paragraph of prose, or a plain value.
        /// </summary>
        public string ChannelBodyHtml
        {
            get
            {
                if (HasLink)
                    return "<a class=\"external-link\" href=\"" + LinkUrl + "\"><span>"
                         + (string.IsNullOrEmpty(ContactValue) ? LinkUrl : ContactValue) + "</span></a>";

                if (HasDescription)
                    return "<p class=\"mb-0\">" + Description + "</p>";

                return "<span>" + ContactValue + "</span>";
            }
        }

        /// <summary>Contact rows of one internal-entity card (phone, e-mail, link, note).</summary>
        public string EntityContactsHtml
        {
            get
            {
                var sb = new StringBuilder();

                if (HasPhone)
                {
                    sb.Append("<div class=\"d-flex gap-2 align-items-center\">")
                      .Append("<i class=\"hgi hgi-stroke hgi-call text-primary fs-4\" aria-hidden=\"true\"></i>");
                    if (!string.IsNullOrEmpty(PhoneLink))
                        sb.Append("<a class=\"external-link fw-medium text-primary justify-content-end\" href=\"")
                          .Append(PhoneLink).Append("\" dir=\"ltr\">").Append(Phone).Append("</a>");
                    else
                        sb.Append("<span class=\"fw-medium\" dir=\"ltr\">").Append(Phone).Append("</span>");
                    sb.Append("</div>");
                }

                if (HasEmail)
                {
                    sb.Append("<div class=\"d-flex gap-2 align-items-center\">")
                      .Append("<i class=\"hgi hgi-stroke hgi-mail-01 text-primary fs-4\" aria-hidden=\"true\"></i>")
                      .Append("<a class=\"external-link\" href=\"mailto:").Append(Email).Append("\"><span>")
                      .Append(Email).Append("</span></a></div>");
                }

                if (HasLink)
                {
                    string text = string.IsNullOrEmpty(ContactValue) ? LinkUrl : ContactValue;
                    sb.Append("<div class=\"d-flex gap-2 align-items-center\">")
                      .Append("<i class=\"hgi hgi-stroke hgi-link-square-02 text-primary fs-4\" aria-hidden=\"true\"></i>")
                      .Append("<a class=\"external-link\" href=\"").Append(LinkUrl)
                      .Append("\" target=\"_blank\" rel=\"noopener noreferrer\"><span>")
                      .Append(text).Append("</span></a></div>");
                }

                if (HasDescription)
                {
                    sb.Append("<div class=\"d-flex gap-2 align-items-center\">")
                      .Append("<i class=\"hgi hgi-stroke hgi-university text-primary fs-4\" aria-hidden=\"true\"></i>")
                      .Append("<span>").Append(Description).Append("</span></div>");
                }

                return sb.ToString();
            }
        }

        /// <summary>The national-address block (icon, heading, address, short code).</summary>
        public string AddressHtml
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("<div class=\"d-flex gap-2 align-items-start\">")
                  .Append("<span class=\"flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary\">")
                  .Append("<i class=\"hgi hgi-stroke hgi-location-01\" aria-hidden=\"true\"></i></span><div>")
                  .Append("<h3 class=\"fw-bold mb-1 h6\">").Append(Title).Append("</h3>")
                  .Append("<p class=\"mb-0\">").Append(Description).Append("</p>");

                if (HasShortCode)
                {
                    string label = TwHelper.Enc(TwHelper.Pick("العنوان الوطني المختصر:", "National short address:"));
                    sb.Append("<p class=\"mb-0 mt-2\"><span class=\"fw-bold\">").Append(label)
                      .Append("</span> <span dir=\"ltr\">").Append(ShortCode).Append("</span></p>");
                }

                sb.Append("</div></div>");
                return sb.ToString();
            }
        }

        /// <summary>Responsive map iframe wrapped by the caller in a .ratio container.</summary>
        public string MapHtml
        {
            get
            {
                if (!HasMap) return string.Empty;
                string title = TwHelper.Enc(TwHelper.Pick(
                    "جامعة الأميرة نورة بنت عبدالرحمن - الموقع على الخريطة",
                    "Princess Nourah bint Abdulrahman University - location on the map"));
                return "<iframe title=\"" + title + "\" src=\"" + MapUrl + "\" "
                     + "loading=\"lazy\" referrerpolicy=\"no-referrer-when-downgrade\"></iframe>";
            }
        }
    }
}
