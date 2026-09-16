using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls
{
    public partial class ucDgaAcademicKitApplicants : DgaPgSectionBase
    {
        public DgaPgHeaderModel HeaderModel { get; private set; } = new DgaPgHeaderModel();
        public DgaPgIntroModel IntroModel { get; private set; } = new DgaPgIntroModel();
        public List<DgaPgJourneyStepModel> JourneySteps { get; private set; } = new List<DgaPgJourneyStepModel>();
        public List<DgaPgResourceCardModel> ProgramCards { get; private set; } = new List<DgaPgResourceCardModel>();
        public DgaPgContentBlockModel EnglishCourse { get; private set; }
        public List<DgaPgFaqItemModel> FaqItems { get; private set; } = new List<DgaPgFaqItemModel>();
        public List<DgaPgResourceCardModel> Regulations { get; private set; } = new List<DgaPgResourceCardModel>();
        public List<DgaPgDiscountRowModel> DiscountRows { get; private set; } = new List<DgaPgDiscountRowModel>();
        public List<string> DiscountNotes { get; private set; } = new List<string>();
        public List<DgaPgServiceGroupModel> ServiceGroups { get; private set; } = new List<DgaPgServiceGroupModel>();
        public DgaPgQuickLinkModel HousingInfo { get; private set; }
        public DgaPgQuickLinkModel ContactInfo { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Phase = DgaPgPhases.Applicants;
            ExecuteSafely("LoadData", LoadData);
        }

        private void LoadData()
        {
            if (SPContext.Current != null)
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = OpenTargetWeb(site))
                {
                    if (web != null)
                    {
                        // 0. Sections
                        LoadSections(web);

                        // 1. Header
                        SPList headerList = GetList(web, DgaPgListNames.Header);
                        if (headerList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Applicants</Value></Eq></Where>" };
                            SPListItemCollection items = headerList.GetItems(q);
                            if (items.Count > 0)
                            {
                                var item = items[0];
                                HeaderModel.Title = DgaPgHelper.PickField(item, "Title");
                                HeaderModel.Description = DgaPgHelper.PickField(item, "Description");
                                HeaderModel.Breadcrumbs = ParseBreadcrumbs(DgaPgHelper.PickField(item, "BreadcrumbText"));
                            }
                        }

                        // 2. Intro
                        SPList introList = GetList(web, DgaPgListNames.Intro);
                        if (introList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Applicants</Value></Eq></Where>" };
                            SPListItemCollection items = introList.GetItems(q);
                            if (items.Count > 0)
                            {
                                var item = items[0];
                                IntroModel.Title = DgaPgHelper.PickField(item, "Title");
                                IntroModel.SubTitle = DgaPgHelper.PickField(item, "SubTitle");
                                IntroModel.LeadText = DgaPgHelper.PickField(item, "LeadText");
                                string rawP = DgaPgHelper.PickField(item, "Paragraphs");
                                IntroModel.Paragraphs = DgaPgHelper.SplitLines(rawP);
                                string rawB = DgaPgHelper.PickField(item, "Badges");
                                IntroModel.Badges = DgaPgHelper.SplitLines(rawB);
                                IntroModel.ReturnText = DgaPgHelper.PickField(item, "ReturnText");
                                IntroModel.ReturnUrl = DgaPgHelper.SafeString(item, "ReturnUrl");
                            }
                        }

                        // 3. Journey
                        SPList journeyList = GetList(web, DgaPgListNames.Journey);
                        if (journeyList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Applicants</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            SPListItemCollection items = journeyList.GetItems(q);
                            foreach (SPListItem item in items)
                            {
                                JourneySteps.Add(new DgaPgJourneyStepModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-route-03"),
                                    StepNumber = DgaPgHelper.SafeInt(item, "StepNumber")
                                });
                            }
                        }

                        // 4. Resources: Programs & Regulations
                        SPList resList = GetList(web, DgaPgListNames.Resources);
                        if (resList != null)
                        {
                            SPQuery qProg = new SPQuery { Query = "<Where><Eq><FieldRef Name='GroupKey' /><Value Type='Choice'>Programs</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            foreach (SPListItem item in resList.GetItems(qProg))
                            {
                                ProgramCards.Add(new DgaPgResourceCardModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-link-square-02")
                                });
                            }

                            SPQuery qReg = new SPQuery { Query = "<Where><And><Eq><FieldRef Name='GroupKey' /><Value Type='Choice'>Regulations</Value></Eq><Or><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Applicants</Value></Eq><IsNull><FieldRef Name='Phase' /></IsNull></Or></And></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            foreach (SPListItem item in resList.GetItems(qReg))
                            {
                                Regulations.Add(new DgaPgResourceCardModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                                    LinkText = DgaPgHelper.PickField(item, "LinkText"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-book-open-01")
                                });
                            }
                        }

                        // 5. English Course
                        SPList blocksList = GetList(web, DgaPgListNames.ContentBlocks);
                        if (blocksList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='BlockKey' /><Value Type='Choice'>EnglishCourse</Value></Eq></Where>" };
                            SPListItemCollection items = blocksList.GetItems(q);
                            if (items.Count > 0)
                            {
                                var item = items[0];
                                EnglishCourse = new DgaPgContentBlockModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    Bullets = DgaPgHelper.SplitLines(DgaPgHelper.PickField(item, "Bullets")),
                                    LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                                    LinkText = DgaPgHelper.PickField(item, "LinkText"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-book-open-01")
                                };
                            }
                        }

                        // 6. FAQs
                        SPList faqList = GetList(web, DgaPgListNames.Faq);
                        if (faqList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Applicants</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            foreach (SPListItem item in faqList.GetItems(q))
                            {
                                FaqItems.Add(new DgaPgFaqItemModel
                                {
                                    Question = DgaPgHelper.PickField(item, "Title"),
                                    Answer = DgaPgHelper.PickField(item, "Answer")
                                });
                            }
                        }

                        // 7. Discounts & Notes
                        SPList discList = GetList(web, DgaPgListNames.Discounts);
                        if (discList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            foreach (SPListItem item in discList.GetItems(q))
                            {
                                DiscountRows.Add(new DgaPgDiscountRowModel
                                {
                                    Category = DgaPgHelper.PickField(item, "Title"),
                                    Percentage = DgaPgHelper.SafeString(item, "Percentage")
                                });
                            }
                        }

                        SPList notesList = GetList(web, DgaPgListNames.DiscountNotes);
                        if (notesList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            foreach (SPListItem item in notesList.GetItems(q))
                            {
                                DiscountNotes.Add(DgaPgHelper.PickField(item, "Title"));
                            }
                        }

                        // 8. Services
                        SPList srvList = GetList(web, DgaPgListNames.Services);
                        if (srvList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            var allSrv = new List<DgaPgServiceModel>();
                            foreach (SPListItem item in srvList.GetItems(q))
                            {
                                allSrv.Add(new DgaPgServiceModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Category = DgaPgHelper.SafeString(item, "Category"),
                                    LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-building-03")
                                });
                            }
                            OrganizeServices(allSrv);
                        }

                        // 9. QuickLinks: Housing & Contact
                        SPList qlList = GetList(web, DgaPgListNames.QuickLinks);
                        if (qlList != null)
                        {
                            SPQuery qH = new SPQuery { Query = "<Where><Eq><FieldRef Name='SectionKey' /><Value Type='Choice'>Housing</Value></Eq></Where>" };
                            SPListItemCollection hItems = qlList.GetItems(qH);
                            if (hItems.Count > 0)
                            {
                                var item = hItems[0];
                                HousingInfo = new DgaPgQuickLinkModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-building-03")
                                };
                            }

                            SPQuery qC = new SPQuery { Query = "<Where><Eq><FieldRef Name='SectionKey' /><Value Type='Choice'>Contact</Value></Eq></Where>" };
                            SPListItemCollection cItems = qlList.GetItems(qC);
                            if (cItems.Count > 0)
                            {
                                var item = cItems[0];
                                ContactInfo = new DgaPgQuickLinkModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-building-03")
                                };
                            }
                        }
                    }
                }
            }

            EnsureDefaults();
        }

        private void OrganizeServices(List<DgaPgServiceModel> all)
        {
            var grpDict = new Dictionary<string, DgaPgServiceGroupModel>();
            foreach (var s in all)
            {
                string grpName = string.IsNullOrEmpty(s.Category) ? GetResource("OtherServices", "خدمات أخرى", "Other Services") : s.Category;
                if (!grpDict.ContainsKey(grpName))
                {
                    grpDict[grpName] = new DgaPgServiceGroupModel { GroupName = grpName };
                    ServiceGroups.Add(grpDict[grpName]);
                }
                grpDict[grpName].Services.Add(s);
            }
        }

        private void EnsureDefaults()
        {
            if (string.IsNullOrEmpty(HeaderModel.Title))
            {
                HeaderModel.Title = GetResource("Applicants", "المتقدمون", "Applicants");
                HeaderModel.Description = GetResource("ApplicantsHeaderDesc", "شروط القبول والبرامج والمستندات والدورة التأهيلية للغة الإنجليزية.", "Admission criteria, programs, documents and English qualifying course.");
            }

            if (HeaderModel.Breadcrumbs.Count == 0)
            {
                HeaderModel.Breadcrumbs.Add(new DgaPgLinkItem { Title = GetResource("Home", "الرئيسية", "Home"), Url = ResolveSiteHomeUrl() });
                HeaderModel.Breadcrumbs.Add(new DgaPgLinkItem { Title = GetResource("KitTitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا", "Interactive Academic Kit for Postgraduate Students"), Url = ResolveKitHubUrl() });
                HeaderModel.Breadcrumbs.Add(new DgaPgLinkItem { Title = HeaderModel.Title, Url = string.Empty });
            }

            if (string.IsNullOrEmpty(IntroModel.Title))
            {
                IntroModel.Title = GetResource("Intro", "نبذة تعريفية", "Overview");
                IntroModel.SubTitle = GetResource("KitSubtitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا", "Interactive Academic Kit for Postgraduate Students");
                IntroModel.LeadText = GetResource("IntroLead", "تحرص عمادة الدراسات العليا على أن تبدأ رحلتكِ الأكاديمية من أساسٍ واضحٍ ومطمئن؛ ولذلك خُصّصت هذه المحطّة لتكون مرجعكِ الأول قبل الالتحاق، تجمع لكِ في موضعٍ واحدٍ منظّم كل ما تحتاجينه للتقديم على برامج الدراسات العليا: من خريطةٍ موضّحةٍ لمراحل القبول خطوةً بخطوة، وروابطَ مباشرةٍ للبرامج المطروحة وشروط القبول والرسوم، والدورة التأهيلية للغة الإنجليزية، ودليل استخدام بوابة القبول الإلكترونية. وهي تسعى إلى تمكينكِ من اتخاذ قرارٍ مدروسٍ، والانطلاق في مسيرتكِ على بصيرةٍ ووضوحٍ منذ خطوتكِ الأولى.", "The Deanship of Graduate Studies is dedicated to ensuring your academic journey begins on a clear and confident foundation. This station gathers everything you need before admission into one organized place.");
                IntroModel.Paragraphs = new List<string> {
                    GetResource("IntroP1", "رفيقٌ رقميٌّ موحّدٌ يواكب الطالبة من القبول حتى منح الدرجة العلمية — بالضغط على أي إجراءٍ تظهر شروطه ونصّ مادته ونماذجه.", "A unified digital companion accompanying students from admission to graduation — clicking any procedure reveals its conditions, rules, and forms.")
                };
                IntroModel.Badges = new List<string> {
                    GetResource("BadgeDeanship", "جامعة الأميرة نورة بنت عبد الرحمن • عمادة الدراسات العليا • وكالة الشؤون الأكاديمية", "Princess Nourah bint Abdulrahman University • Deanship of Graduate Studies"),
                    GetResource("BadgeYear", "١٤٤٧ - ١٤٤٨هـ", "1447 - 1448 AH")
                };
                IntroModel.ReturnText = GetResource("BackToHome", "⟶ العودة لاختيار الفئة", "⟶ Back to Kit Home");
            }

            if (JourneySteps.Count == 0)
            {
                JourneySteps.Add(new DgaPgJourneyStepModel { Title = GetResource("Step1Title", "متابعة الإعلان", "Announcements Tracking"), Description = GetResource("Step1Desc", "المرحلة 1 — متابعة إعلان فتح القبول لبرامج الدراسات العليا عبر موقع العمادة وقنواتها الرسمية، ومعرفة البرامج المطروحة لكل فصل.", "Stage 1 — Follow the admission announcement via the official Deanship portal and discover available programs each semester."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { Title = GetResource("Step2Title", "اختيار البرنامج والتحقّق من شروطه", "Program Selection & Requirements"), Description = GetResource("Step2Desc", "المرحلة 2 — الاطّلاع على البرامج المطروحة وشروط القبول الخاصة بكل برنامج، والتأكّد من استيفائها قبل التقديم.", "Stage 2 — Review available programs and specific admission criteria, ensuring all prerequisites are met prior to applying."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { Title = GetResource("Step3Title", "تجهيز المستندات", "Document Preparation"), Description = GetResource("Step3Desc", "المرحلة 3 — إعداد المستندات المطلوبة وفق شروط البرنامج (المؤهّل، السجل الأكاديمي، التزكيات، الهوية، وغيرها)، ورفعها في البوابة.", "Stage 3 — Prepare required documents according to program specifications (degree, transcript, recommendations, ID) and upload them to portal."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { Title = GetResource("Step4Title", "التقديم عبر البوابة", "Portal Application Submission"), Description = GetResource("Step4Desc", "المرحلة 4 — تعبئة طلب القبول إلكترونياً عبر بوابة القبول للدراسات العليا، ورفع المستندات، ومتابعة حالة الطلب.", "Stage 4 — Complete online application via the postgraduate admissions portal, submit documents, and track application status."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { Title = GetResource("Step5Title", "المفاضلة والنتيجة", "Evaluation & Final Admission"), Description = GetResource("Step5Desc", "المرحلة 5 — تتمّ المفاضلة وفق معايير كل برنامج، ثم تُعلَن النتائج عبر البوابة؛ وعلى المقبولة استكمال إجراءات التسجيل.", "Stage 5 — Competitive evaluation according to program criteria, result declaration on portal, and completing registration procedures."), IconClass = "hgi-route-03" });
            }

            if (ProgramCards.Count == 0)
            {
                ProgramCards.Add(new DgaPgResourceCardModel { Title = GetResource("Prog1Title", "برامج الدراسات العليا والرسوم الدراسية", "Postgraduate Programs & Tuition Fees"), Description = GetResource("Prog1Desc", "تصفّح البرامج المتاحة (دبلوم عالٍ، ماجستير، دكتوراه) وتخصّصاتها ورسومها الدراسية — محدّثةٌ دائماً من المصدر.", "Browse available programs (Higher Diploma, Master's, PhD) and tuition fees — always updated from official source."), LinkUrl = "https://pnu.edu.sa/ar/RegAdm/PGD/Pages/home.aspx", IconClass = "hgi-link-square-02" });
                ProgramCards.Add(new DgaPgResourceCardModel { Title = GetResource("Prog2Title", "شروط القبول وشرط اللغة الإنجليزية", "Admission Criteria & English Requirements"), Description = GetResource("Prog2Desc", "الاطّلاع على شروط القبول العامة والخاصة بكل برنامج، وشرط اللغة الإنجليزية المعتمد.", "Review general and specific admission requirements and approved English proficiency scores."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/English.aspx", IconClass = "hgi-link-square-02" });
                ProgramCards.Add(new DgaPgResourceCardModel { Title = GetResource("Prog3Title", "فترات التقديم وبوابة القبول الإلكتروني", "Application Dates & Online Admission Portal"), Description = GetResource("Prog3Desc", "معرفة مواعيد فتح القبول والدخول إلى بوابة التقديم الإلكترونية.", "Check admission opening schedules and access the online application portal."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/Periods.aspx", IconClass = "hgi-link-square-02" });
            }

            if (EnglishCourse == null)
            {
                EnglishCourse = new DgaPgContentBlockModel
                {
                    Title = GetResource("EngTitle", "الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا", "English Qualifying Course for Postgraduate Students"),
                    Description = GetResource("EngDesc", "يقدّم معهد اللغة الإنجليزية بالجامعة دورةً تأهيليةً تُعِدّ الطالبة بالمهارات اللغوية الأكاديمية اللازمة للالتحاق ببرامج الدراسات العليا، وتُعادل الدرجة المطلوبة في اختبار اللغة الإنجليزية المعتمد للقبول.", "The English Language Institute offers a qualifying course equipping students with academic English skills equivalent to the required test score."),
                    Bullets = new List<string> {
                        GetResource("EngB1", "تركّز على المهارات الأكاديمية الأربع: الكتابة والقراءة والاستماع والمحادثة.", "Focuses on the four academic language skills: Writing, Reading, Listening, and Speaking."),
                        GetResource("EngB2", "تتضمّن مسارين تدريبيين بحسب المستوى، تجمع بين الحضور المباشر والتعلّم الذاتي.", "Includes two training tracks according to proficiency level, combining in-person sessions and self-paced learning."),
                        GetResource("EngB3", "تُمنح شهادة اجتياز عند تحقيق نسبة 70% فأعلى في التقييمات.", "A completion certificate is awarded upon scoring 70% or higher in assessments."),
                        GetResource("EngB4", "تستفيد المتدرّبة من مركزي التحدّث والكتابة بالمعهد.", "Enrolled trainees benefit from the Institute's Speaking and Writing Centers.")
                    },
                    LinkUrl = "https://pnu.edu.sa/ar/Faculties/EI/Pages/GraduateStudentsCourses.aspx",
                    LinkText = GetResource("EngBtn", "الإعلان والتسجيل في الدورة", "Announcement & Course Registration"),
                    IconClass = "hgi-book-open-01"
                };
            }

            if (Regulations.Count == 0)
            {
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("Reg1Title", "دليل المستخدم لبوابة القبول الإلكترونية", "Online Admission Portal User Guide"), Description = GetResource("Reg1Desc", "مرجعٌ يوضّح خطوات التسجيل والتقديم عبر البوابة.", "A guide explaining registration and application steps on the portal."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf", IconClass = "hgi-book-open-01" });
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("Reg2Title", "الدليل الإرشادي لطلبة الدراسات العليا", "Postgraduate Student Handbook"), Description = GetResource("Reg2Desc", "مرجعٌ تعريفيٌّ عامٌّ بنظام الدراسة والإجراءات والخدمات.", "A comprehensive guide to academic study systems, procedures and services."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf", IconClass = "hgi-book-open-01" });
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("Reg3Title", "الدليل الإجرائي لمسار الإجراءات الأكاديمية", "Academic Procedures Operational Manual"), Description = GetResource("Reg3Desc", "تفصيلٌ لمسار كل إجراءٍ أكاديميٍّ خطوةً بخطوة.", "Step-by-step detail of every academic operational procedure."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d9%8a%d9%94%d9%8a%20%d9%84%d9%85%d8%b3%d8%a7%d8%b1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d8%a1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%94%d9%83%d8%a7%d8%af%d9%8a%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf", IconClass = "hgi-book-open-01" });
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("Reg4Title", "دليل إعداد خطة البحث والرسالة العلمية", "Thesis & Proposal Preparation Manual"), Description = GetResource("Reg4Desc", "إرشاداتٌ لإعداد الخطة البحثية وكتابة الرسالة العلمية.", "Guidelines for preparing the research proposal and writing the academic thesis."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%b9%d8%af%d8%a7%d8%af%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%88%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d8%44%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7.pdf", IconClass = "hgi-book-open-01" });
            }

            if (DiscountRows.Count == 0)
            {
                DiscountRows.Add(new DgaPgDiscountRowModel { Category = GetResource("D1Cat", "أبناء شهداء الواجب أو المصابين في أداء الواجب الوطني", "Children of Duty Martyrs or Those Injured in National Duty"), Percentage = GetResource("D1Pct", "إعفاء كامل", "Full Exemption") });
                DiscountRows.Add(new DgaPgDiscountRowModel { Category = GetResource("D2Cat", "ذوو الإعاقة (حسب طبيعة البرنامج)", "Persons with Disabilities (subject to program nature)"), Percentage = "٢٥٪" });
                DiscountRows.Add(new DgaPgDiscountRowModel { Category = GetResource("D3Cat", "السداد المبكر لإجمالي رسوم البرنامج - طلبة الدراسات العليا", "Early Full Program Tuition Payment - Postgraduate Students"), Percentage = "١٠٪" });
                DiscountRows.Add(new DgaPgDiscountRowModel { Category = GetResource("D4Cat", "الإخوة طلبة الدراسات العليا", "Siblings Enrolled in Postgraduate Studies"), Percentage = "١٠٪" });
                DiscountRows.Add(new DgaPgDiscountRowModel { Category = GetResource("D5Cat", "جهات الإيفاد - الجهات الحكومية والمؤسسات التعليمية والجهات الخاصة", "Sponsoring Entities - Government, Educational & Private Sectors"), Percentage = GetResource("D5Pct", "١٠٪ إذا كان عدد الموفدين أقل من خمسة / ١٥٪ إذا كان عدد الموفدين خمسة فأكثر", "10% for fewer than 5 candidates / 15% for 5 or more candidates") });
                DiscountRows.Add(new DgaPgDiscountRowModel { Category = GetResource("D6Cat", "المستفيدون من الضمان الاجتماعي والجمعيات الخيرية والمتبرعون بالأعضاء", "Social Security Beneficiaries, Charities & Organ Donors"), Percentage = "٢٠٪" });
                DiscountRows.Add(new DgaPgDiscountRowModel { Category = GetResource("D7Cat", "النشر العلمي أثناء الدراسة (وفق ضوابط وآلية النشر العلمي بجامعة الأميرة نورة بنت عبدالرحمن)", "Scientific Publication during Studies (pursuant to PNU Publishing Rules)"), Percentage = "١٠٪" });
            }

            if (DiscountNotes.Count == 0)
            {
                DiscountNotes.Add(GetResource("N1", "يطبق التخفيض الأعلى في حال استحقاق المستفيد لأكثر من تخفيض.", "The highest discount rate applies when eligible for multiple discounts."));
                DiscountNotes.Add(GetResource("N2", "يطبق الإعفاء والتخفيض على السعوديين وغير السعوديين.", "Exemptions and discounts apply to both Saudi and international students."));
                DiscountNotes.Add(GetResource("N3", "لا يطبق الخصم على الحاصلين على منح التميز.", "Discounts do not apply to excellence scholarship recipients."));
                DiscountNotes.Add(GetResource("N4", "لا يطبق التخفيض على رسوم المقررات التكميلية.", "Discounts do not apply to supplementary / prerequisite course fees."));
                DiscountNotes.Add(GetResource("N5", "يطبق الإعفاء والتخفيض على البرامج المحلية فقط.", "Exemptions and discounts apply strictly to local programs."));
                DiscountNotes.Add(GetResource("N6", "لا يطبق التخفيض على المستفيدين من برنامج تقسيط التعليم في حال السداد المبكر لإجمالي الرسوم الدراسية.", "Discounts do not apply to tuition installment beneficiaries in case of early payment."));
                DiscountNotes.Add(GetResource("N7", "لا يطبق الإعفاء والتخفيض على بعض البرامج القائمة بالشراكات.", "Exemptions and discounts do not apply to certain partnership programs."));
                DiscountNotes.Add(GetResource("N8", "يجب تقديم المستندات التي تثبت الحالة الصحية أو بطاقة الإعاقة لجميع الفئات.", "Official disability ID or certified health documentation must be provided."));
                DiscountNotes.Add(GetResource("N9", "يجب تقديم شهادة أو خطاب من المركز السعودي لزراعة الأعضاء أو من المستشفى يفيد بأنه تم التبرع.", "Official letter from Saudi Center for Organ Transplantation or hospital is required for donors."));
                DiscountNotes.Add(GetResource("N10", "تُحسب ضريبة القيمة المضافة لغير السعوديين على الرسوم الدراسية بعد الخصم.", "VAT for non-Saudi students is calculated on tuition fees after applying discounts."));
            }

            if (ServiceGroups.Count == 0)
            {
                var g1 = new DgaPgServiceGroupModel { GroupName = GetResource("SrvG1", "الخدمات البحثية", "Research Services") };
                g1.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv1_1", "برنامج الباحثات الناشئات", "Junior Researchers Program"), Category = GetResource("SrvG1", "الخدمات البحثية", "Research Services"), LinkUrl = "https://pnu.edu.sa/ar/SR/Pages/Junior-Researchers-Program.aspx", IconClass = "hgi-building-03" });
                g1.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv1_2", "برنامج نافع", "Nafie Program"), Category = GetResource("SrvG1", "الخدمات البحثية", "Research Services"), LinkUrl = "https://pnu.edu.sa/ar/SR/Pages/Nafie-Program.aspx", IconClass = "hgi-building-03" });
                g1.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv1_3", "برنامج المجموعات البحثية", "Research Groups Program"), Category = GetResource("SrvG1", "الخدمات البحثية", "Research Services"), LinkUrl = "https://pnu.edu.sa/ar/SR/Pages/Research-Groups-Program.aspx", IconClass = "hgi-building-03" });
                g1.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv1_4", "أخلاقيات البحث العلمي", "Scientific Research Ethics"), Category = GetResource("SrvG1", "الخدمات البحثية", "Research Services"), LinkUrl = "https://pnu.edu.sa/ar/SR/Pages/SREthics_.aspx", IconClass = "hgi-building-03" });
                g1.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv1_5", "المكتبة المركزية", "Central Library"), Category = GetResource("SrvG1", "الخدمات البحثية", "Research Services"), LinkUrl = "https://pnu.edu.sa/ar/Deanship/Research/Pages/LibraryServices.aspx", IconClass = "hgi-building-03" });
                g1.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv1_6", "مركز الخدمات البحثية", "Research Services Center"), Category = GetResource("SrvG1", "الخدمات البحثية", "Research Services"), LinkUrl = "https://pnu.edu.sa/ar/Centers/ResearchServicesCenter/Pages/home.aspx", IconClass = "hgi-building-03" });
                ServiceGroups.Add(g1);

                var g2 = new DgaPgServiceGroupModel { GroupName = GetResource("SrvG2", "الخدمات التأهيلية لسوق العمل", "Career & Market Readiness Services") };
                g2.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv2_1", "إضافة السير الذاتية", "CV Upload Service"), Category = GetResource("SrvG2", "الخدمات التأهيلية لسوق العمل", "Career Services"), LinkUrl = "https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=223", IconClass = "hgi-building-03" });
                g2.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv2_2", "مركز الدعم الطلابي والمهني", "Student & Career Support Center"), Category = GetResource("SrvG2", "الخدمات التأهيلية لسوق العمل", "Career Services"), LinkUrl = "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/home.aspx", IconClass = "hgi-building-03" });
                g2.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv2_3", "إدارة الدعم المهاري", "Skills Support Administration"), Category = GetResource("SrvG2", "الخدمات التأهيلية لسوق العمل", "Career Services"), LinkUrl = "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/55.aspx", IconClass = "hgi-building-03" });
                g2.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv2_4", "الأنشطة اللامنهجية", "Extracurricular Activities"), Category = GetResource("SrvG2", "الخدمات التأهيلية لسوق العمل", "Career Services"), LinkUrl = "https://graduates.pnu.edu.sa/ar/Pages/NonApproachActivities.aspx", IconClass = "hgi-building-03" });
                g2.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv2_5", "برنامج سموق", "Sumouq Program"), Category = GetResource("SrvG2", "الخدمات التأهيلية لسوق العمل", "Career Services"), LinkUrl = "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/33.aspx", IconClass = "hgi-building-03" });
                g2.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv2_6", "بوابة الخريجات", "Alumnae Portal"), Category = GetResource("SrvG2", "الخدمات التأهيلية لسوق العمل", "Career Services"), LinkUrl = "https://graduates.pnu.edu.sa/ar/pages/default.aspx#about", IconClass = "hgi-building-03" });
                ServiceGroups.Add(g2);

                var g3 = new DgaPgServiceGroupModel { GroupName = GetResource("SrvG3", "الخدمات الطلابية", "Student Affairs Services") };
                g3.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv3_1", "وكالة الخدمات الطلابية", "Student Services Agency"), Category = GetResource("SrvG3", "الخدمات الطلابية", "Student Services"), LinkUrl = "https://pnu.edu.sa/ar/Deanship/studaffairs/AgencyStudentServices/Pages/home.aspx", IconClass = "hgi-building-03" });
                g3.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv3_2", "وكالة الحياة الطلابية", "Student Life Agency"), Category = GetResource("SrvG3", "الخدمات الطلابية", "Student Services"), LinkUrl = "https://pnu.edu.sa/ar/Deanship/studaffairs/Student%20Life%20Agency/Pages/home.aspx", IconClass = "hgi-building-03" });
                g3.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv3_3", "حماية حقوق الطالبات", "Student Rights Protection"), Category = GetResource("SrvG3", "الخدمات الطلابية", "Student Services"), LinkUrl = "https://pnu.edu.sa/ar/Faculties/Ap/Pages/Student-Rights-Protection.aspx", IconClass = "hgi-building-03" });
                g3.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv3_4", "وحدة مساندة الطالبات ذوي الإعاقة", "Support Unit for Students with Disabilities"), Category = GetResource("SrvG3", "الخدمات الطلابية", "Student Services"), LinkUrl = "https://pnu.edu.sa/ar/Deanship/studaffairs/AgencyStudentServices/Pages/StudentswithDisabilitiesUnit.aspx", IconClass = "hgi-building-03" });
                g3.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv3_5", "دليل تمكين", "Tamkeen Guide"), Category = GetResource("SrvG3", "الخدمات الطلابية", "Student Services"), LinkUrl = "https://drive.google.com/file/d/1C3BgZxt_eMMyCqXJ5G46-h5JhQp08NIJ/view?usp=drive_link", IconClass = "hgi-building-03" });
                g3.Services.Add(new DgaPgServiceModel { Title = GetResource("Srv3_6", "المراكز الترفيهية", "Recreation Centers"), Category = GetResource("SrvG3", "الخدمات الطلابية", "Student Services"), LinkUrl = "https://pnu.edu.sa/ar/Deanship/studaffairs/Student%20Life%20Agency/Pages/RecreationCenter.aspx", IconClass = "hgi-building-03" });
                ServiceGroups.Add(g3);
            }

            if (HousingInfo == null)
            {
                HousingInfo = new DgaPgQuickLinkModel
                {
                    Title = GetResource("HousingTitle", "التسجيل في سكن الطالبات", "Student Housing Registration"),
                    Description = GetResource("HousingDesc", "يوفّر السكن الجامعي بيئةً آمنةً ومهيّأةً لطالبات الدراسات العليا، وتُدار خدمة التسجيل فيه إلكترونياً عبر عمادة شؤون الطالبات. يمكنكِ الاطّلاع على الشروط والمستندات والرسوم والتقديم من خلال صفحة الخدمة الرسمية.", "University housing provides a secure, well-equipped environment for postgraduate students. Registration is managed online through the Deanship of Student Affairs."),
                    LinkUrl = "https://pnu.edu.sa/ar/Deanship/studaffairs/Pages/sfdsre.aspx",
                    LinkText = GetResource("ApplyHousing", "التسجيل في سكن الطالبات", "Apply for Student Housing"),
                    IconClass = "hgi-building-03"
                };
            }

            if (ContactInfo == null)
            {
                ContactInfo = new DgaPgQuickLinkModel
                {
                    Title = "dgs-aa@pnu.edu.sa",
                    Description = GetResource("ContactDesc", "استفساراتكِ تهمّنا. إن احتجتِ أي مساعدةٍ أو إيضاح، تواصلي مع وحدة خدمة المستفيدين بعمادة الدراسات العليا عبر البريد الإلكتروني، وسيسعدنا خدمتكِ.", "Your inquiries matter to us. If you need any assistance, contact the Beneficiary Services Unit at the Deanship of Graduate Studies via email."),
                    LinkUrl = "mailto:dgs-aa@pnu.edu.sa",
                    LinkText = "dgs-aa@pnu.edu.sa",
                    IconClass = "hgi-building-03"
                };
            }

            if (FaqItems.Count == 0)
            {
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ1", "متى يُفتح باب القبول لبرامج الدراسات العليا؟", "When does admission open for postgraduate programs?"), Answer = GetResource("FaqA1", "يُعلَن عن فتح القبول عبر موقع عمادة الدراسات العليا وقنواتها الرسمية في مواعيد محدّدة لكل عام؛ ويمكن متابعة المواعيد عبر صفحة مواعيد وبوابة القبول.", "Admission openings are announced on the Deanship website and official channels on scheduled dates each year.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ2", "ما شروط القبول؟", "What are the admission requirements?"), Answer = GetResource("FaqA2", "تختلف الشروط بحسب البرنامج والمرحلة، وتشمل شروطاً عامةً (كالمؤهّل الجامعي والتقدير) وأخرى خاصةً بكل برنامج. ولأنها قد تتغيّر، يُرجى الرجوع إلى صفحة شروط القبول الرسمية للاطّلاع على المحدّث منها.", "Requirements vary by program and stage, comprising general criteria and program-specific conditions.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ3", "ما المستندات المطلوبة للتقديم؟", "What documents are required for application?"), Answer = GetResource("FaqA3", "تُحدَّد المستندات وفق شروط كل برنامج، وتشمل غالباً المؤهّل الجامعي والسجل الأكاديمي والتزكيات والهوية؛ وتُرفع إلكترونياً عبر بوابة القبول.", "Documents include graduation certificate, academic transcript, recommendation letters, and national ID/passport.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ4", "لم أحقّق شرط اللغة الإنجليزية، ما الحلّ؟", "I didn't meet the English requirement, what is the solution?"), Answer = GetResource("FaqA4", "يمكنكِ الالتحاق بالدورة التأهيلية للغة الإنجليزية بمعهد اللغة الإنجليزية، فهي تُعادل الدرجة المطلوبة في اختبار اللغة المعتمد للقبول.", "You may enroll in the English Qualifying Course offered by the English Language Institute, which satisfies the requirement.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ5", "كيف تتمّ المفاضلة بين المتقدّمات؟", "How is candidate evaluation conducted?"), Answer = GetResource("FaqA5", "تتمّ المفاضلة وفق معايير كل برنامج (كالتقدير الجامعي والاختبارات والمقابلات إن وُجدت)، وتُعلَن النتائج عبر البوابة.", "Evaluation is based on program criteria (GPA, standardized tests, interviews) and results are published via portal.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ6", "هل يمكنني التقديم على أكثر من برنامج؟", "Can I apply for more than one program?"), Answer = GetResource("FaqA6", "تتيح بوابة القبول التقديم وفق ضوابط كل فصل؛ ويُنصح بمراجعة دليل المستخدم وصفحة البرامج لمعرفة الخيارات المتاحة وآليّة الترشيح.", "The admission portal specifies options per term; refer to the user guide for allowable choices.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ7", "هل الدراسة في برامج الدراسات العليا بمقابلٍ مادي؟", "Are postgraduate programs tuition-based?"), Answer = GetResource("FaqA7", "تتفاوت البرامج بين مجانيةٍ ومدفوعة، وتُوضَّح الرسوم في صفحة كل برنامج؛ لذا يُرجى الرجوع إلى صفحة البرامج والرسوم للاطّلاع على المحدّث.", "Programs vary between funded and tuition-based; tuition details are specified on each program's page.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ8", "ما الدورة التأهيلية للغة الإنجليزية، ولمن تُوجَّه؟", "What is the English Qualifying Course and who is it for?"), Answer = GetResource("FaqA8", "هي دورةٌ يقدّمها معهد اللغة الإنجليزية للطالبات الحاصلات على قبولٍ مشروط، تُعِدّهنّ بالمهارات اللغوية اللازمة، وتُعادل الدرجة المطلوبة في اختبار اللغة المعتمد للقبول.", "It is a course offered by the English Language Institute for conditionally admitted students, equipping them with required linguistic skills.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("FaqQ9", "أين أتابع حالة طلب قبولي؟", "Where do I track my application status?"), Answer = GetResource("FaqA9", "يمكنكِ متابعة حالة طلبكِ عبر بوابة القبول الإلكترونية التي قدّمتِ من خلالها، ومتابعة إعلانات النتائج عبر قنوات العمادة الرسمية.", "Track your application status directly via the online admissions portal.") });
            }
        }

        // ---------------------------------------------------------------
        // Section anchors and table of contents
        // ---------------------------------------------------------------

        protected override string SectionAnchor(string sectionKey)
        {
            switch (sectionKey)
            {
                case DgaPgSectionKeys.Intro:         return "applicant-intro";
                case DgaPgSectionKeys.Journey:       return "applicant-section-1";
                case DgaPgSectionKeys.Programs:      return "applicant-section-2";
                case DgaPgSectionKeys.EnglishCourse: return "applicant-section-3";
                case DgaPgSectionKeys.Faq:           return "applicant-section-4";
                case DgaPgSectionKeys.Regulations:   return "applicant-section-5";
                case DgaPgSectionKeys.Discounts:     return "applicant-section-6";
                case DgaPgSectionKeys.Services:      return "applicant-section-7";
                case DgaPgSectionKeys.Housing:       return "applicant-section-8";
                case DgaPgSectionKeys.Contact:       return "applicant-section-9";
                case DgaPgSectionKeys.Reference:     return "applicant-reference";
                default:                             return "applicant-" + (sectionKey ?? string.Empty).ToLowerInvariant();
            }
        }

        public List<DgaPgLinkItem> TocItems
        {
            get
            {
                var toc = new List<DgaPgLinkItem>();

                AddToc(toc, DgaPgSectionKeys.Intro, IntroModel.Title, true);
                AddToc(toc, DgaPgSectionKeys.Journey, SectionTitle(DgaPgSectionKeys.Journey), JourneySteps.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Programs, SectionTitle(DgaPgSectionKeys.Programs), ProgramCards.Count > 0);
                AddToc(toc, DgaPgSectionKeys.EnglishCourse, SectionTitle(DgaPgSectionKeys.EnglishCourse), EnglishCourse != null);
                AddToc(toc, DgaPgSectionKeys.Faq, SectionTitle(DgaPgSectionKeys.Faq), FaqItems.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Regulations, SectionTitle(DgaPgSectionKeys.Regulations), Regulations.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Discounts, SectionTitle(DgaPgSectionKeys.Discounts), DiscountRows.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Services, SectionTitle(DgaPgSectionKeys.Services), ServiceGroups.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Housing, SectionTitle(DgaPgSectionKeys.Housing), HousingInfo != null);
                AddToc(toc, DgaPgSectionKeys.Contact, SectionTitle(DgaPgSectionKeys.Contact), ContactInfo != null);
                AddToc(toc, DgaPgSectionKeys.Reference, ReferenceTitle, !string.IsNullOrEmpty(ReferenceLead));

                return toc;
            }
        }

        /// <summary>Section heading, falling back to the v4 design wording per axis.</summary>
        public string SectionTitle(string sectionKey)
        {
            switch (sectionKey)
            {
                case DgaPgSectionKeys.Journey:       return GetSectionTitle(sectionKey, GetResource("Axis1", "المحور الأول: خريطة رحلة القبول", "First Axis: Admission Journey Map"));
                case DgaPgSectionKeys.Programs:      return GetSectionTitle(sectionKey, GetResource("Axis2", "المحور الثاني: البرامج وشروط القبول والرسوم", "Second Axis: Programs, Admission Requirements and Fees"));
                case DgaPgSectionKeys.EnglishCourse: return GetSectionTitle(sectionKey, GetResource("Axis3", "المحور الثالث: الدورة التأهيلية للغة الإنجليزية", "Third Axis: English Language Qualifying Course"));
                case DgaPgSectionKeys.Faq:           return GetSectionTitle(sectionKey, GetResource("Axis4", "المحور الرابع: الأسئلة الأكثر شيوعاً", "Fourth Axis: Frequently Asked Questions"));
                case DgaPgSectionKeys.Regulations:   return GetSectionTitle(sectionKey, GetResource("Axis5", "المحور الخامس: الحقوق والواجبات / اللوائح النظامية", "Fifth Axis: Rights and Duties / Statutory Regulations"));
                case DgaPgSectionKeys.Discounts:     return GetSectionTitle(sectionKey, GetResource("Axis6", "المحور السادس: إعفاء وتخفيض الرسوم الدراسية لفئات محددة من طلبة الدراسات العليا", "Sixth Axis: Tuition Exemptions and Reductions for Specific Categories"));
                case DgaPgSectionKeys.Services:      return GetSectionTitle(sectionKey, GetResource("Axis7", "المحور السابع: الخدمات / المراكز", "Seventh Axis: Services and Centers"));
                case DgaPgSectionKeys.Housing:       return GetSectionTitle(sectionKey, GetResource("Axis8", "المحور الثامن: السكن الجامعي", "Eighth Axis: University Housing"));
                case DgaPgSectionKeys.Contact:       return GetSectionTitle(sectionKey, GetResource("Axis9", "المحور التاسع: تواصلي معنا", "Ninth Axis: Contact Us"));
                default:                             return GetSectionTitle(sectionKey, string.Empty);
            }
        }

        protected string ResolveHomeUrl()
        {
            return IsArabic ? "/ar/Pages/default.aspx" : "/en/Pages/default.aspx";
        }
    }
}
