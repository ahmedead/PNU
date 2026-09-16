using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls
{
    /// <summary>
    /// "دليل الشهادات الاحترافية" - the professional certifications guide.
    ///
    /// The list is checked (and provisioned + seeded when missing) on every page load,
    /// read into a flat dictionary per certificate and published as a JSON payload that
    /// the client script turns into cards, the details modal and the two print views.
    /// Search, college filter, modal and print therefore need no postback.
    ///
    /// Every label is bilingual (PNUres key + hardcoded fallback) and every value is
    /// HTML-encoded at projection time, so the markup only carries &lt;%# %&gt; expressions.
    /// </summary>
    public partial class ucCertifications : UserControl
    {
        // =====================================================================
        // public properties
        // =====================================================================
        /// <summary>Set to "False" to hide the blue page header (title + intro).</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ShowHeader { get; set; }

        /// <summary>Set to "False" to hide the breadcrumb inside the header.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ShowBreadcrumb { get; set; }

        /// <summary>Target of the first breadcrumb link.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string HomeUrl { get; set; }

        /// <summary>Overrides the page heading coming from the schema.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string PageTitleText { get; set; }

        /// <summary>Overrides the first intro paragraph.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string IntroText1 { get; set; }

        /// <summary>Overrides the second intro paragraph.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string IntroText2 { get; set; }

        /// <summary>Illustration shown when nothing matches the filters.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string EmptyImageUrl { get; set; }

        /// <summary>0 = publish every certificate.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int MaxItems { get; set; }

        /// <summary>Read a different list than the schema default.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListNameOverride { get; set; }

        public ucCertifications()
        {
            ShowHeader = "True";
            ShowBreadcrumb = "True";
            HomeUrl = "/";
            EmptyImageUrl = "/Style Library/DGA/images/empty.svg";
        }

        protected string EffectiveListName
        {
            get
            {
                return string.IsNullOrEmpty(ListNameOverride)
                    ? CertListNames.Certifications
                    : ListNameOverride;
            }
        }

        // =====================================================================
        // labels consumed by the <%# %> expressions
        // =====================================================================
        protected string HomeLabel { get; private set; }
        protected string PageHeading { get; private set; }
        protected string ExportListLabel { get; private set; }
        protected string ExportDetailLabel { get; private set; }
        protected string IntroLine1 { get; private set; }
        protected string IntroLine2 { get; private set; }
        protected string SearchAriaLabel { get; private set; }
        protected string SearchLabel { get; private set; }
        protected string SearchPlaceholder { get; private set; }
        protected string SearchButtonLabel { get; private set; }
        protected string FilterLabel { get; private set; }
        protected string CollegeLabel { get; private set; }
        protected string DepartmentLabel { get; private set; }
        protected string CertificateNameLabel { get; private set; }
        protected string CollegeSearchLabel { get; private set; }
        protected string CollegeSearchPlaceholder { get; private set; }
        protected string ApplyLabel { get; private set; }
        protected string ResetLabel { get; private set; }
        protected string ResultsHeading { get; private set; }
        protected string EmptyTitle { get; private set; }
        protected string EmptyText { get; private set; }
        protected string CloseLabel { get; private set; }
        protected string ProviderLabel { get; private set; }
        protected string LevelLabel { get; private set; }
        protected string CostLabel { get; private set; }
        protected string LanguageLabel { get; private set; }
        protected string ModeLabel { get; private set; }
        protected string SupportLabel { get; private set; }
        protected string DescriptionHeading { get; private set; }
        protected string RequirementsHeading { get; private set; }
        protected string TopicsHeading { get; private set; }
        protected string ValidityHeading { get; private set; }
        protected string RegistrationHeading { get; private set; }
        protected string ResourcesHeading { get; private set; }
        protected string CoursesHeading { get; private set; }
        protected string OfficialSiteLabel { get; private set; }
        protected string PrintReportTitle { get; private set; }
        protected string UniversityName { get; private set; }
        protected string TextDirection { get; private set; }

        // =====================================================================
        // lifecycle
        // =====================================================================
        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                SetLabels();

                pnlHeader.Visible = !"False".Equals(ShowHeader, StringComparison.OrdinalIgnoreCase);
                pnlBreadcrumb.Visible = !"False".Equals(ShowBreadcrumb, StringComparison.OrdinalIgnoreCase);

                List<Dictionary<string, string>> payload = LoadCertifications();

                ltData.Text = CertJson.Encode(payload);
                ltLabels.Text = CertJson.Encode(ScriptLabels());

                // this.DataBind() - not a repeater - so every <%# %> expression in the
                // static markup is evaluated as well.
                this.DataBind();

                secCertifications.Visible = true;
            }
            catch (Exception ex)
            {
                CertLog.Write("ucCertifications.Bind", ex);

                ltData.Text = "[]";
                ltLabels.Text = "{}";
                secCertifications.Visible = false;
            }
        }

        // =====================================================================
        // data
        // =====================================================================
        /// <summary>
        /// Checks/provisions the list on the target web (/ar/Agencies/AcademicAffairs) and projects
        /// every item onto the JSON keys the client script expects.
        /// Called on EVERY Page_Load - ViewState is often disabled in SharePoint zones.
        /// </summary>
        private List<Dictionary<string, string>> LoadCertifications()
        {
            var payload = new List<Dictionary<string, string>>();

            try
            {
                // Page-load safety net: verifies the list still exists on the target web
                // and re-provisions it (columns + the 44 default rows) if it is missing.
                CertListProvisioner.EnsureListExists(EffectiveListName);

                CertListDef def = CertListSchema.Get(EffectiveListName);
                if (def == null) return payload;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = CertTargetWeb.Open(site))
                {
                    if (web == null) return payload;

                    foreach (SPListItem item in CertHelper.GetItems(web, EffectiveListName))
                    {
                        payload.Add(Project(item, def));

                        if (MaxItems > 0 && payload.Count >= MaxItems) break;
                    }
                }
            }
            catch (Exception ex)
            {
                CertLog.Write("ucCertifications.LoadCertifications:" + EffectiveListName, ex);
            }

            return payload;
        }

        /// <summary>
        /// Maps one list item onto the JSON keys declared in the schema. Values are NOT
        /// HTML-encoded here: the script writes them through textContent, never innerHTML,
        /// and the payload itself is escaped by CertJson.Encode.
        /// </summary>
        private Dictionary<string, string> Project(SPListItem item, CertListDef def)
        {
            var row = new Dictionary<string, string>();

            row[CertListSchema.TitleJsonKey] = CertHelper.SafeString(item, "Title");

            foreach (CertFieldDef f in def.Fields)
            {
                if (string.IsNullOrEmpty(f.JsonKey)) continue;

                row[f.JsonKey] = f.Type == SPFieldType.URL
                    ? CertHelper.SafeUrl(item, f.InternalName)
                    : CertHelper.SafeString(item, f.InternalName);
            }

            return row;
        }

        // =====================================================================
        // labels
        // =====================================================================
        private void SetLabels()
        {
            CertListDef def = CertListSchema.Get(EffectiveListName);
            string schemaTitle = def != null ? def.Display : string.Empty;

            TextDirection = CertHelper.IsArabic ? "rtl" : "ltr";

            HomeLabel = Enc(CertHelper.GetRes("Cert_Home", "الرئيسية", "Home"));

            PageHeading = Enc(string.IsNullOrEmpty(PageTitleText) ? schemaTitle : PageTitleText);

            ExportListLabel = Enc(CertHelper.GetRes("Cert_ExportList", "تصدير القائمة", "Export list"));
            ExportDetailLabel = Enc(CertHelper.GetRes("Cert_ExportDetail",
                "تصدير تفاصيل الشهادة", "Export certificate details"));

            IntroLine1 = Enc(string.IsNullOrEmpty(IntroText1)
                ? CertHelper.GetRes("Cert_Intro1",
                    "يهدف هذا الدليل إلى تسليط الضوء على الشهادات الاحترافية المهمة في مختلف التخصصات، مع التركيز على كيفية استفادة طالبات جامعة الأميرة نورة بنت عبد الرحمن من هذه الشهادات لدعم رحلتهن الأكاديمية والمهنية.",
                    "This guide highlights the professional certificates that matter across disciplines, and how PNU students can use them to support their academic and professional journey.")
                : IntroText1);

            IntroLine2 = Enc(string.IsNullOrEmpty(IntroText2)
                ? CertHelper.GetRes("Cert_Intro2",
                    "تم إعداد هذا الدليل لتقديم قائمة شاملة من الشهادات الاحترافية في تخصصات متنوعة، كإدارة الأعمال، وتقنية المعلومات، والهندسة. ويهدف إلى دعم الطالبات في اختيار الشهادات الأنسب لتعزيز مهاراتهن المهنية.",
                    "It offers a comprehensive list of professional certificates across fields such as business administration, information technology and engineering, to help students pick the ones that best strengthen their professional skills.")
                : IntroText2);

            SearchAriaLabel = Enc(CertHelper.GetRes("Cert_SearchAria",
                "ابحث عن شهادة احترافية", "Search for a professional certificate"));
            SearchLabel = Enc(CertHelper.GetRes("Cert_SearchLabel",
                "البحث في الشهادات الاحترافية", "Search the professional certificates"));
            SearchPlaceholder = Enc(CertHelper.GetRes("Cert_SearchPlaceholder",
                "ابحث باسم الشهادة أو الجهة المانحة أو القسم",
                "Search by certificate name, provider or department"));
            SearchButtonLabel = Enc(CertHelper.GetRes("Cert_Search", "بحث", "Search"));

            FilterLabel = Enc(CertHelper.GetRes("Cert_Filter", "تصفية", "Filter"));
            CollegeLabel = Enc(CertHelper.GetRes("Cert_College", "الكلية", "College"));
            DepartmentLabel = Enc(CertHelper.GetRes("Cert_Department", "القسم", "Department"));
            CertificateNameLabel = Enc(CertHelper.GetRes("Cert_Name", "اسم الشهادة", "Certificate name"));
            CollegeSearchLabel = Enc(CertHelper.GetRes("Cert_CollegeSearch", "البحث في الكليات", "Search colleges"));
            CollegeSearchPlaceholder = Enc(CertHelper.GetRes("Cert_CollegeSearchPlaceholder",
                "ابحث عن كلية", "Search for a college"));
            ApplyLabel = Enc(CertHelper.GetRes("Cert_Apply", "تطبيق الاختيارات", "Apply selection"));
            ResetLabel = Enc(CertHelper.GetRes("Cert_Reset", "إعادة تعيين", "Reset"));

            ResultsHeading = Enc(CertHelper.GetRes("Cert_Results", "الشهادات الاحترافية", "Professional certificates"));
            EmptyTitle = Enc(CertHelper.GetRes("Cert_EmptyTitle", "لا توجد نتائج مطابقة", "No matching results"));
            EmptyText = Enc(CertHelper.GetRes("Cert_EmptyText",
                "جرّب تغيير الكلية أو عبارة البحث.", "Try a different college or search term."));

            CloseLabel = Enc(CertHelper.GetRes("Cert_Close", "إغلاق", "Close"));
            ProviderLabel = Enc(CertHelper.GetRes("Cert_Provider", "الجهة المانحة", "Provider"));
            LevelLabel = Enc(CertHelper.GetRes("Cert_Level", "المستوى", "Level"));
            CostLabel = Enc(CertHelper.GetRes("Cert_Cost", "التكلفة", "Cost"));
            LanguageLabel = Enc(CertHelper.GetRes("Cert_Language", "اللغة", "Language"));
            ModeLabel = Enc(CertHelper.GetRes("Cert_Mode", "آلية التنفيذ", "Delivery"));
            SupportLabel = Enc(CertHelper.GetRes("Cert_Support", "دعم هدف", "HRDF support"));

            DescriptionHeading = Enc(CertHelper.GetRes("Cert_DescriptionHeading", "نبذة عن الشهادة", "About the certificate"));
            RequirementsHeading = Enc(CertHelper.GetRes("Cert_RequirementsHeading", "المتطلبات", "Requirements"));
            TopicsHeading = Enc(CertHelper.GetRes("Cert_TopicsHeading", "الموضوعات", "Topics"));
            ValidityHeading = Enc(CertHelper.GetRes("Cert_ValidityHeading", "صلاحية الشهادة", "Validity"));
            RegistrationHeading = Enc(CertHelper.GetRes("Cert_RegistrationHeading", "إجراءات التقديم", "How to apply"));
            ResourcesHeading = Enc(CertHelper.GetRes("Cert_ResourcesHeading", "المصادر التعليمية", "Learning resources"));
            CoursesHeading = Enc(CertHelper.GetRes("Cert_CoursesHeading", "المقررات المرتبطة", "Linked courses"));
            OfficialSiteLabel = Enc(CertHelper.GetRes("Cert_OfficialSite", "زيارة الموقع الرسمي", "Visit the official site"));

            PrintReportTitle = Enc(CertHelper.GetRes("Cert_PrintTitle",
                "تقرير الشهادات الاحترافية", "Professional certificates report"));
            UniversityName = Enc(CertHelper.GetRes("Cert_University",
                "جامعة الأميرة نورة بنت عبدالرحمن", "Princess Nourah bint Abdulrahman University"));
        }

        /// <summary>
        /// The strings the client script needs. Plain text - the script writes them with
        /// textContent, so they must NOT be HTML-encoded here.
        /// </summary>
        private Dictionary<string, string> ScriptLabels()
        {
            var labels = new Dictionary<string, string>();

            labels["locale"] = CertHelper.IsArabic ? "ar" : "en";
            labels["notSpecified"] = CertHelper.GetRes("Cert_NotSpecified", "غير محدد", "Not specified");
            labels["none"] = CertHelper.GetRes("Cert_None", "لا يوجد", "None");
            labels["noDescription"] = CertHelper.GetRes("Cert_NoDescription",
                "لا يوجد وصف متاح.", "No description available.");
            labels["countSuffix"] = CertHelper.GetRes("Cert_CountSuffix", "شهادة معروضة", "certificates shown");
            labels["resultsCount"] = CertHelper.GetRes("Cert_ResultsCount", "عدد النتائج: ", "Results: ");
            labels["details"] = CertHelper.GetRes("Cert_Details", "عرض التفاصيل", "View details");
            labels["collegeAndDepartment"] = CertHelper.GetRes("Cert_CollegeAndDepartment",
                "الكلية والقسم", "College and department");

            labels["provider"] = CertHelper.GetRes("Cert_Provider", "الجهة المانحة", "Provider");
            labels["level"] = CertHelper.GetRes("Cert_Level", "المستوى", "Level");
            labels["cost"] = CertHelper.GetRes("Cert_Cost", "التكلفة", "Cost");
            labels["language"] = CertHelper.GetRes("Cert_Language", "اللغة", "Language");
            labels["mode"] = CertHelper.GetRes("Cert_Mode", "آلية التنفيذ", "Delivery");
            labels["support"] = CertHelper.GetRes("Cert_Support", "دعم هدف", "HRDF support");

            labels["descriptionHeading"] = CertHelper.GetRes("Cert_DescriptionHeading", "نبذة عن الشهادة", "About the certificate");
            labels["topicsHeading"] = CertHelper.GetRes("Cert_TopicsHeading", "الموضوعات", "Topics");
            labels["requirementsHeading"] = CertHelper.GetRes("Cert_RequirementsHeading", "المتطلبات", "Requirements");
            labels["validityHeading"] = CertHelper.GetRes("Cert_ValidityHeading", "صلاحية الشهادة", "Validity");
            labels["registrationHeading"] = CertHelper.GetRes("Cert_RegistrationHeading", "إجراءات التقديم", "How to apply");
            labels["resourcesHeading"] = CertHelper.GetRes("Cert_ResourcesHeading", "المصادر التعليمية", "Learning resources");
            labels["coursesHeading"] = CertHelper.GetRes("Cert_CoursesHeading", "المقررات المرتبطة", "Linked courses");

            return labels;
        }

        private static string Enc(string value)
        {
            return CertHelper.Enc(value);
        }
    }
}
