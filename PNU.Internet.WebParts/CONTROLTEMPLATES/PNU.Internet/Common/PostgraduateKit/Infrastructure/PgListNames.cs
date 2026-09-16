namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Internal names for the Postgraduate Academic Kit SharePoint lists.
    /// </summary>
    public static class PgListNames
    {
        public const string Sections      = "PgSections";
        public const string Header        = "PgHeader";
        public const string Intro         = "PgIntro";
        public const string Journey       = "PgJourney";
        public const string Resources     = "PgResources";
        public const string ContentBlocks = "PgContentBlocks";
        public const string Procedures    = "PgProcedures";
        public const string Faq           = "PgFaq";
        public const string Discounts     = "PgDiscounts";
        public const string DiscountNotes = "PgDiscountNotes";
        public const string Services      = "PgServices";
        public const string QuickLinks    = "PgQuickLinks";
        public const string TopBar        = "PgTopBar";
        public const string FooterLinks   = "PgFooterLinks";
        public const string Hub           = "PgHub";

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
            TopBar,
            FooterLinks
        };
    }

    /// <summary>
    /// The three audiences of the kit. Stored in the "Phase" choice column of every list.
    /// </summary>
    public static class PgPhases
    {
        /// <summary>The landing page that introduces the kit and links to the three stations.</summary>
        public const string Hub        = "Hub";

        public const string Applicants = "Applicants";
        public const string Enrolled   = "Enrolled";
        public const string Graduates  = "Graduates";

        public static readonly string[] All = new string[] { Hub, Applicants, Enrolled, Graduates };

        /// <summary>The three audience stations, without the hub.</summary>
        public static readonly string[] Stations = new string[] { Applicants, Enrolled, Graduates };

        public static string Display(string phase)
        {
            switch (phase)
            {
                case Hub:       return PgHelper.Pick("الصفحة الرئيسة للحقيبة", "Kit landing page");
                case Enrolled:  return PgHelper.Pick("المقيدون", "Enrolled students");
                case Graduates: return PgHelper.Pick("الخريجون", "Graduates");
                default:        return PgHelper.Pick("المتقدمون", "Applicants");
            }
        }
    }

    /// <summary>
    /// Keys used by the "SectionKey" / "GroupKey" / "BlockKey" choice columns.
    /// </summary>
    public static class PgSectionKeys
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
        public const string Footer            = "Footer";

        /// <summary>The "choose your station" section of the hub page.</summary>
        public const string Audiences         = "Audiences";

        public static readonly string[] All = new string[]
        {
            Intro, Journey, Programs, Regulations, GraduateIdentity, FinancialRules,
            Procedures, EnglishCourse, Faq, PublishIncentive, Discounts, Guides,
            Services, Housing, Contact, GradRequirements, Audiences, Footer
        };
    }
}
