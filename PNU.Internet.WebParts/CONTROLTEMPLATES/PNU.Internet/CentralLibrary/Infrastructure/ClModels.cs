using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public class ClBreadcrumbItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsLast { get; set; }
    }

    public class ClHeaderModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Button1Text { get; set; }
        public string Button1Url { get; set; }
        public string Button2Text { get; set; }
        public string Button2Url { get; set; }
        public List<ClBreadcrumbItem> BreadcrumbItems { get; set; }

        public ClHeaderModel()
        {
            BreadcrumbItems = new List<ClBreadcrumbItem>();
        }
    }

    public class ClAboutModel
    {
        public string Title { get; set; }
        public string Paragraph1 { get; set; }
        public string Paragraph2 { get; set; }
        public string ImageUrlSm { get; set; }
        public string ImageUrlMd { get; set; }
        public string ImageUrlLg { get; set; }
        public string ImageAlt { get; set; }
        public string NumbersTitle { get; set; }
    }

    public class ClCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatValue { get; set; }
        public string SubTitle { get; set; }
        public string BadgeText { get; set; }
        public string IconClass { get; set; }
        public string LinkUrl { get; set; }
        public string ButtonText { get; set; }
        public string ImageUrl { get; set; }
        public string LogoUrl { get; set; }
        public int ItemOrder { get; set; }

        // Contact specific fields
        public string Row1Label { get; set; }
        public string Row1Value { get; set; }
        public string Row2Label { get; set; }
        public string Row2Value { get; set; }
        public string Row3Label { get; set; }
        public string Row3Value { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone1Tel { get; set; }
        public string Phone2 { get; set; }
        public string Phone2Tel { get; set; }
        public string LocationText { get; set; }

        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool HasImage { get { return !string.IsNullOrEmpty(ImageUrl); } }
        public bool HasLogo { get { return !string.IsNullOrEmpty(LogoUrl); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
        public bool HasBadge { get { return !string.IsNullOrEmpty(BadgeText); } }

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

        public List<string> BadgesList
        {
            get
            {
                if (string.IsNullOrEmpty(BadgeText)) return new List<string>();
                return BadgeText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(b => b.Trim())
                                .Where(b => b.Length > 0)
                                .ToList();
            }
        }
    }
}
