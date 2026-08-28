using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    /// <summary>
    /// Flat DTOs for the University President Office controls.
    /// </summary>
    public class UpoSection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LeadText { get; set; }
        public string IconClass { get; set; }
        public bool ShowLeadCard { get; set; }
        public string ImageUrl { get; set; }
        public int ItemOrder { get; set; }

        public bool HasLeadCard { get { return ShowLeadCard && !string.IsNullOrEmpty(LeadText); } }
        public bool HasImage { get { return !string.IsNullOrEmpty(ImageUrl); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }

        public System.Collections.Generic.List<string> Paragraphs
        {
            get
            {
                if (string.IsNullOrEmpty(Description)) return new System.Collections.Generic.List<string>();
                return new System.Collections.Generic.List<string>(
                    Description.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => p.Trim())
                               .Where(p => p.Length > 0));
            }
        }
    }

    public class UpoContact
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContactValue { get; set; }
        public string ContactType { get; set; }
        public int ItemOrder { get; set; }

        /// <summary>Returns the href value based on ContactType.</summary>
        public string ContactHref
        {
            get
            {
                switch ((ContactType ?? "").ToLower())
                {
                    case "email": return "mailto:" + ContactValue;
                    case "phone": return "tel:" + ContactValue;
                    case "link":  return ContactValue;
                    default:      return "#";
                }
            }
        }

        public bool IsExternal { get { return (ContactType ?? "").ToLower() == "link"; } }
    }

    public class UpoSignatureCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int ItemOrder { get; set; }

        public bool HasImage { get { return !string.IsNullOrEmpty(ImageUrl); } }
    }
}
