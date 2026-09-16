namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    /// <summary>
    /// Internal names for the DGA Postgraduate Academic Kit SharePoint lists.
    /// Prefixed with DgaPg to remain completely isolated from existing lists.
    /// </summary>
    public static class DgaPgListNames
    {
        public const string Sections      = "DgaPgSections";
        public const string Header        = "DgaPgHeader";
        public const string Hub           = "DgaPgHub";
        public const string Intro         = "DgaPgIntro";
        public const string Journey       = "DgaPgJourney";
        public const string Resources     = "DgaPgResources";
        public const string ContentBlocks = "DgaPgContentBlocks";
        public const string Procedures    = "DgaPgProcedures";
        public const string Faq           = "DgaPgFaq";
        public const string Discounts     = "DgaPgDiscounts";
        public const string DiscountNotes = "DgaPgDiscountNotes";
        public const string Services      = "DgaPgServices";
        public const string QuickLinks    = "DgaPgQuickLinks";
        public const string FooterLinks   = "DgaPgFooterLinks";

        public static readonly string[] All = new string[]
        {
            Sections,
            Header,
            Hub,
            Intro,
            Journey,
            Resources,
            ContentBlocks,
            Procedures,
            Faq,
            Discounts,
            DiscountNotes,
            Services,
            QuickLinks,
            FooterLinks
        };
    }

    /// <summary>
    /// The phases / audiences of the kit.
    /// </summary>
    public static class DgaPgPhases
    {
        public const string Hub        = "Hub";
        public const string Applicants = "Applicants";
        public const string Enrolled   = "Enrolled";
        public const string Graduates  = "Graduates";

        public static readonly string[] All = new string[] { Hub, Applicants, Enrolled, Graduates };
        public static readonly string[] Stations = new string[] { Applicants, Enrolled, Graduates };

        public static string Display(string phase)
        {
            switch (phase)
            {
                case Hub:       return DgaPgHelper.Pick("الصفحة الرئيسة للحقيبة", "Academic Kit Home");
                case Enrolled:  return DgaPgHelper.Pick("المقيدون", "Enrolled Students");
                case Graduates: return DgaPgHelper.Pick("الخريجون", "Graduates");
                default:        return DgaPgHelper.Pick("المتقدمون", "Applicants");
            }
        }
    }

    /// <summary>
    /// Keys used by the SectionKey / GroupKey / BlockKey choice columns.
    /// </summary>
    public static class DgaPgSectionKeys
    {
        public const string Intro             = "Intro";
        public const string Journey           = "Journey";
        public const string Programs          = "Programs";
        public const string Regulations       = "Regulations";
        public const string GraduateIdentity  = "GraduateIdentity";
        public const string FinancialRules    = "FinancialRules";
        public const string Procedures        = "Procedures";
        public const string EnglishCourse     = "EnglishCourse";
        public const string Faq               = "Faq";
        public const string PublishIncentive  = "PublishIncentive";
        public const string Discounts         = "Discounts";
        public const string Guides            = "Guides";
        public const string Services          = "Services";
        public const string Housing           = "Housing";
        public const string Contact           = "Contact";
        public const string GradRequirements  = "GradRequirements";
        public const string Reference         = "Reference";
        public const string Footer            = "Footer";
        public const string Audiences         = "Audiences";

        /// <summary>
        /// Allowed values for the SectionKey / BlockKey choice columns.
        /// </summary>
        public static readonly string[] All = new string[]
        {
            Intro, Journey, Programs, Regulations, GraduateIdentity, FinancialRules,
            Procedures, EnglishCourse, Faq, PublishIncentive, Discounts, Guides,
            Services, Housing, Contact, GradRequirements, Reference, Footer, Audiences
        };
    }
}
