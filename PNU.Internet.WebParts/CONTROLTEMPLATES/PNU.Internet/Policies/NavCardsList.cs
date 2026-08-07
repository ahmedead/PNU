
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

        public string Icon { get; set; }

        public string Url { get; set; }

        public string ButtonText { get; set; }

        public string Target { get; set; }

        public double SortOrder { get; set; }

        // ---- display-ready values (computed, so markup stays Eval-only) -----

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

        /// <summary>aria-label, e.g. "عرض سياسة الخصوصية".</summary>
        public string AriaLabel
        {
            get { return "عرض " + (Title ?? ""); }
        }
    }
}
