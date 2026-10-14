
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies
{
    /// <summary>
    /// One navigation card. Field internal names below match the columns
    /// created by NavCardsProvisioner.EnsureList().
    /// Mapped from SPListItemCollection via SPFactory.MapListItemsToClass&lt;T&gt;().
    /// </summary>
    public class NavCardsList
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string TitleEn { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public string UrlEn { get; set; }

        public string ButtonText { get; set; }

        public string ButtonTextEn { get; set; }

        public string Target { get; set; }

        public double SortOrder { get; set; }

        /// <summary>Set by ucNavCards based on current page language context.</summary>
        public bool IsArabic { get; set; } = true;

        // ---- display-ready values (computed, so markup stays Eval-only) -----

        public string DisplayTitle
        {
            get
            {
                if (!IsArabic && !string.IsNullOrWhiteSpace(TitleEn))
                    return TitleEn;
                return Title ?? "";
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

        /// <summary>Empty for internal links; "noopener noreferrer" for _blank.</summary>
        public string Rel
        {
            get
            {
                return (!string.IsNullOrEmpty(Target) &&
                        Target.Trim().Equals("_blank", System.StringComparison.OrdinalIgnoreCase))
                        ? "noopener noreferrer" : "";
            }
        }

        /// <summary>aria-label for accessibility.</summary>
        public string AriaLabel
        {
            get
            {
                string action = IsArabic ? "عرض " : "View ";
                return action + (DisplayTitle ?? "");
            }
        }
    }
}
