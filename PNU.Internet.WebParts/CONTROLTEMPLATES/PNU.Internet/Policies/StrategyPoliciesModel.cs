using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies
{
    /// <summary>
    /// Section model representing one major section (Strategies, Policies, Regulations).
    /// Saved in the StrategyPoliciesSections list.
    /// </summary>
    public class StrategyPoliciesSection
    {
        public int ID { get; set; }

        public string SectionKey { get; set; }

        public string Title { get; set; }

        public string TitleEn { get; set; }

        public string Description { get; set; }

        public string DescriptionEn { get; set; }

        public double ItemOrder { get; set; }

        public bool Visibility { get; set; } = true;

        public bool IsArabic { get; set; } = true;

        public List<StrategyPoliciesCard> Cards { get; set; } = new List<StrategyPoliciesCard>();

        public string DisplayTitle
        {
            get
            {
                if (!IsArabic && !string.IsNullOrWhiteSpace(TitleEn))
                    return TitleEn;
                return Title ?? "";
            }
        }

        public string DisplayDescription
        {
            get
            {
                if (!IsArabic && !string.IsNullOrWhiteSpace(DescriptionEn))
                    return DescriptionEn;
                return Description ?? "";
            }
        }
    }

    /// <summary>
    /// Card model representing a single strategy, policy, or regulation card.
    /// Saved in the StrategyPoliciesCards list.
    /// </summary>
    public class StrategyPoliciesCard
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string TitleEn { get; set; }

        public string SectionKey { get; set; }

        public string Badge { get; set; }

        public string BadgeEn { get; set; }

        public string BadgeClass { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public string UrlEn { get; set; }

        public string ButtonText { get; set; }

        public string ButtonTextEn { get; set; }

        public string Target { get; set; } = "_self";

        public double SortOrder { get; set; }

        public bool Visibility { get; set; } = true;

        public bool IsArabic { get; set; } = true;

        public string DisplayTitle
        {
            get
            {
                if (!IsArabic && !string.IsNullOrWhiteSpace(TitleEn))
                    return TitleEn;
                return Title ?? "";
            }
        }

        public string DisplayBadge
        {
            get
            {
                if (!IsArabic && !string.IsNullOrWhiteSpace(BadgeEn))
                    return BadgeEn;
                return Badge ?? "";
            }
        }

        public string DisplayUrl
        {
            get
            {
                if (!IsArabic && !string.IsNullOrWhiteSpace(UrlEn))
                    return UrlEn;
                return Url ?? "";
            }
        }

        public string DisplayButtonText
        {
            get
            {
                if (!IsArabic && !string.IsNullOrWhiteSpace(ButtonTextEn))
                    return ButtonTextEn;
                if (!string.IsNullOrWhiteSpace(ButtonText))
                    return ButtonText;
                return IsArabic ? "عرض التفاصيل" : "View Details";
            }
        }

        public string Rel
        {
            get
            {
                return (!string.IsNullOrEmpty(Target) &&
                        Target.Trim().Equals("_blank", StringComparison.OrdinalIgnoreCase))
                        ? "noopener noreferrer" : "";
            }
        }

        public string AriaLabel
        {
            get
            {
                string btn = DisplayButtonText;
                string title = DisplayTitle;
                return !string.IsNullOrEmpty(btn) && !string.IsNullOrEmpty(title)
                    ? btn + ": " + title
                    : title;
            }
        }
    }
}
