using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls
{
    public partial class ucDgaAcademicKitEnrolled : DgaPgSectionBase
    {
        public DgaPgHeaderModel HeaderModel { get; private set; } = new DgaPgHeaderModel();
        public DgaPgIntroModel IntroModel { get; private set; } = new DgaPgIntroModel();
        public List<DgaPgJourneyStepModel> JourneySteps { get; private set; } = new List<DgaPgJourneyStepModel>();
        public List<DgaPgResourceCardModel> Regulations { get; private set; } = new List<DgaPgResourceCardModel>();
        public List<DgaPgProcedureModel> Procedures { get; private set; } = new List<DgaPgProcedureModel>();
        public List<DgaPgFaqItemModel> FaqItems { get; private set; } = new List<DgaPgFaqItemModel>();
        public DgaPgContentBlockModel PublicationIncentive { get; private set; }
        public DgaPgContentBlockModel GraduateIdentity { get; private set; }
        public DgaPgContentBlockModel FinancialRules { get; private set; }
        public List<DgaPgDiscountRowModel> DiscountRows { get; private set; } = new List<DgaPgDiscountRowModel>();
        public List<string> DiscountNotes { get; private set; } = new List<string>();
        public List<DgaPgResourceCardModel> Guides { get; private set; } = new List<DgaPgResourceCardModel>();
        public List<DgaPgServiceGroupModel> ServiceGroups { get; private set; } = new List<DgaPgServiceGroupModel>();
        public DgaPgQuickLinkModel HousingInfo { get; private set; }
        public DgaPgQuickLinkModel ContactInfo { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Phase = DgaPgPhases.Enrolled;
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
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Enrolled</Value></Eq></Where>" };
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
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Enrolled</Value></Eq></Where>" };
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

                        // 3. Journey Steps
                        SPList journeyList = GetList(web, DgaPgListNames.Journey);
                        if (journeyList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Enrolled</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
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

                        // 4. Resources: Regulations & Guides
                        SPList resList = GetList(web, DgaPgListNames.Resources);
                        if (resList != null)
                        {
                            SPQuery qReg = new SPQuery { Query = "<Where><And><Eq><FieldRef Name='GroupKey' /><Value Type='Choice'>Regulations</Value></Eq><Or><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Enrolled</Value></Eq><IsNull><FieldRef Name='Phase' /></IsNull></Or></And></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
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

                            SPQuery qGuides = new SPQuery { Query = "<Where><Eq><FieldRef Name='GroupKey' /><Value Type='Choice'>Guides</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            foreach (SPListItem item in resList.GetItems(qGuides))
                            {
                                Guides.Add(new DgaPgResourceCardModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                                    LinkText = DgaPgHelper.PickField(item, "LinkText"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-book-open-01")
                                });
                            }
                        }

                        // 5. Interactive Procedures
                        SPList procList = GetList(web, DgaPgListNames.Procedures);
                        if (procList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Enrolled</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            SPListItemCollection items = procList.GetItems(q);
                            foreach (SPListItem item in items)
                            {
                                var p = new DgaPgProcedureModel
                                {
                                    Id = item.ID,
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    ShortDescription = DgaPgHelper.PickField(item, "ShortDescription"),
                                    FlowText = DgaPgHelper.PickField(item, "FlowText"),
                                    IconUrl = DgaPgHelper.SafeString(item, "IconUrl"),
                                    BadgeText = DgaPgHelper.PickField(item, "BadgeText"),
                                    SubGroupKey = DgaPgHelper.SafeString(item, "SubGroupKey", "College"),
                                    DirectServiceUrl = DgaPgHelper.SafeString(item, "DirectServiceUrl")
                                };
                                string rawReq = DgaPgHelper.PickField(item, "Requirements");
                                p.Requirements = DgaPgHelper.SplitLines(rawReq);
                                string rawArt = DgaPgHelper.PickField(item, "ArticleText");
                                p.Articles = DgaPgHelper.ParseArticles(rawArt);
                                string rawForms = DgaPgHelper.PickField(item, "Forms");
                                p.Forms = DgaPgHelper.ParseForms(rawForms);
                                Procedures.Add(p);
                            }
                        }

                        // 6. Publication Incentive
                        SPList blocksList = GetList(web, DgaPgListNames.ContentBlocks);
                        if (blocksList != null)
                        {
                            PublicationIncentive = ReadContentBlock(blocksList, "PublishIncentive", "hgi-award-02");
                            GraduateIdentity = ReadContentBlock(blocksList, "GraduateIdentity", "hgi-checkmark-badge-01");
                            FinancialRules = ReadContentBlock(blocksList, "FinancialRules", "hgi-chart");
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

                        // 9. QuickLinks
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

                        // 10. FAQs
                        SPList faqList = GetList(web, DgaPgListNames.Faq);
                        if (faqList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Enrolled</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            foreach (SPListItem item in faqList.GetItems(q))
                            {
                                FaqItems.Add(new DgaPgFaqItemModel
                                {
                                    Question = DgaPgHelper.PickField(item, "Title"),
                                    Answer = DgaPgHelper.PickField(item, "Answer")
                                });
                            }
                        }
                    }
                }
            }

            EnsureDefaults();
        }

        private DgaPgContentBlockModel ReadContentBlock(SPList blocksList, string blockKey, string defaultIcon)
        {
            var query = new SPQuery
            {
                Query = "<Where><Eq><FieldRef Name='BlockKey' /><Value Type='Choice'>" + blockKey + "</Value></Eq></Where>"
            };

            SPListItemCollection items = blocksList.GetItems(query);
            if (items.Count == 0) return null;

            SPListItem item = items[0];
            return new DgaPgContentBlockModel
            {
                BlockKey = blockKey,
                Title = DgaPgHelper.PickField(item, "Title"),
                Description = DgaPgHelper.PickField(item, "Description"),
                Paragraphs = DgaPgHelper.SplitLines(DgaPgHelper.PickField(item, "Paragraphs")),
                Bullets = DgaPgHelper.SplitLines(DgaPgHelper.PickField(item, "Bullets")),
                Groups = DgaPgHelper.ParseArticles(DgaPgHelper.PickField(item, "Groups")),
                NoteText = DgaPgHelper.PickField(item, "NoteText"),
                BadgeText = DgaPgHelper.PickField(item, "BadgeText"),
                LinkUrl = DgaPgHelper.SafeString(item, "LinkUrl"),
                LinkText = DgaPgHelper.PickField(item, "LinkText"),
                IconUrl = DgaPgHelper.SafeString(item, "IconUrl"),
                IconClass = DgaPgHelper.SafeString(item, "IconClass", defaultIcon)
            };
        }

        /// <summary>Procedures belonging to one of the design's two axis-3 sub-groups.</summary>
        public List<DgaPgProcedureModel> ProceduresIn(string subGroupKey)
        {
            var subset = new List<DgaPgProcedureModel>();
            foreach (DgaPgProcedureModel p in Procedures)
            {
                string key = string.IsNullOrEmpty(p.SubGroupKey) ? "College" : p.SubGroupKey;
                if (string.Equals(key, subGroupKey, StringComparison.OrdinalIgnoreCase)) subset.Add(p);
            }
            return subset;
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

        public string ResolveProcIcon(string title)
        {
            if (string.IsNullOrEmpty(title)) return "enrolled-procedure-generic.png";
            if (title.Contains("معادلة") || title.ToLower().Contains("equivalen")) return "enrolled-procedure-equivalency.png";
            if (title.Contains("خطة") || title.Contains("بحث") || title.ToLower().Contains("research")) return "enrolled-procedure-research-plan.png";
            if (title.Contains("إلغاء") || title.Contains("قيد") || title.ToLower().Contains("cancel")) return "enrolled-procedure-cancel.png";
            if (title.Contains("إنذار") || title.ToLower().Contains("warning")) return "enrolled-procedure-warning.png";
            if (title.Contains("إعادة") || title.ToLower().Contains("reinstate")) return "enrolled-procedure-re-enrollment.png";
            if (title.Contains("تأجيل") || title.ToLower().Contains("postpone")) return "enrolled-procedure-postponement.png";
            if (title.Contains("اعتذار") || title.ToLower().Contains("apology")) return "enrolled-procedure-apology.png";
            if (title.Contains("حذف") || title.Contains("إضافة") || title.ToLower().Contains("drop")) return "enrolled-procedure-add-drop.png";
            if (title.Contains("تفرغ") || title.ToLower().Contains("leave")) return "enrolled-procedure-full-time.png";
            if (title.Contains("فرصة") || title.Contains("إضافية") || title.ToLower().Contains("chance")) return "enrolled-procedure-extra-chance.png";
            if (title.Contains("بديل") || title.ToLower().Contains("substitute")) return "enrolled-procedure-substitute.png";
            if (title.Contains("تحويل") || title.ToLower().Contains("transfer")) return "enrolled-procedure-transfer.png";
            if (title.Contains("لجنة") || title.Contains("مناقشة") || title.ToLower().Contains("defense")) return "enrolled-procedure-defense-committee.png";
            if (title.Contains("تعديل") || title.ToLower().Contains("amendment")) return "enrolled-procedure-amendments.png";
            if (title.Contains("منح") || title.Contains("درجة") || title.ToLower().Contains("award")) return "enrolled-procedure-degree-grant.png";
            return "enrolled-procedure-generic.png";
        }

        private void EnsureDefaults()
        {
            if (string.IsNullOrEmpty(HeaderModel.Title))
            {
                HeaderModel.Title = GetResource("Enrolled", "المقيدون", "Enrolled Students");
                HeaderModel.Description = GetResource("EnrolledHeaderDesc", "الرحلة الأكاديمية والحقوق واللوائح والإجراءات والخدمات المساندة.", "Academic journey, rights, regulations, procedures and support services.");
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
                IntroModel.LeadText = GetResource("EnrolledIntroLead", "بوصفكِ طالبةً مقيَّدةً في الدراسات العليا، تمثّل هذه المحطّة رفيقكِ الأكاديمي طوال مسيرتكِ؛ إذ تحرص العمادة على أن تكون كل إجراءاتكِ وحقوقكِ ولوائحكِ في متناول يدكِ، واضحةً مفهومةً يسيرةَ الوصول. وقد جُمعت لكِ فيها خريطة رحلتكِ الأكاديمية، وحزمة اللوائح المنظّمة لحقوقكِ وواجباتكِ، والإجراءات الأكاديمية كاملةً بشروطها ونماذجها ومساراتها، إلى جانب حافز النشر العلمي والأدلة الإرشادية؛ لتكون مرجعكِ الموحّد الذي يرافقكِ من بدء الدراسة حتى إتمام متطلّباتها بثقةٍ واطمئنان.", "As an enrolled postgraduate student, this station represents your comprehensive academic companion throughout your journey.");
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
                JourneySteps.Add(new DgaPgJourneyStepModel { StepNumber = 1, Title = GetResource("EStep1Title", "تسجيل الجدول الدراسي", "Course Schedule Registration"), Description = GetResource("EStep1Desc", "مرحلةٌ مشتركةٌ مع الإدارة التنفيذية: إدخال الجداول في النظام الأكاديمي وتسجيلها للطلبة.", "Joint stage with executive administration: entering schedules in academic system and registering courses for students."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { StepNumber = 2, Title = GetResource("EStep2Title", "دراسة المقرّرات", "Coursework Study"), Description = GetResource("EStep2Desc", "الالتزام بالجدول وحضور المقررات وتسليم متطلباتها، والنجاح بتقدير «جيد مرتفع» على الأقل.", "Attending lectures, submitting requirements, and passing courses with at least 'High Good' grade."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { StepNumber = 3, Title = GetResource("EStep3Title", "تسجيل خطة البحث", "Thesis Proposal Registration"), Description = GetResource("EStep3Desc", "مناقشة الفكرة مع المرشد/ـة الأكاديمي/ـة وتسجيل خطة الرسالة بعد إنجاز 50% من المقررات بمعدل لا يقل عن «جيد جداً».", "Discussing proposal with advisor and registering thesis plan after completing 50% of coursework with GPA not less than 'Very Good'."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { StepNumber = 4, Title = GetResource("EStep4Title", "تشكيل لجنة المناقشة", "Defense Committee Formation"), Description = GetResource("EStep4Desc", "تقدّم المشرف/ـة بطلب تشكيل اللجنة، ورفع التوصيات والمسوّغات من عمادة الكلية.", "Supervisor submits committee formation request with recommendations from college deanship."), IconClass = "hgi-route-03" });
                JourneySteps.Add(new DgaPgJourneyStepModel { StepNumber = 5, Title = GetResource("EStep5Title", "منح الدرجة العلمية", "Degree Conferral"), Description = GetResource("EStep5Desc", "استكمال نماذج اللجنة والإقرار، وتسليم نسخ الرسالة المعتمدة، ورفع التوصية بمنح الدرجة.", "Completing committee reports, submitting approved thesis copies, and recommending degree award."), IconClass = "hgi-route-03" });
            }

            if (Regulations.Count == 0)
            {
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("EReg1", "اللائحة المنظمة للدراسات العليا في الجامعات وقواعدها التنفيذية", "Unified Postgraduate Regulations & Executive Rules"), Description = GetResource("EReg1Desc", "المصدر الأساس لحقوق طالبة الدراسات العليا وواجباتها الأكاديمية (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤).", "Primary foundation of student academic rights and obligations (Council Decision 2/9/1444)."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Lists/Guidelines/Attachments/4/%D8%A7%D9%84%D9%84%D8%A7%D9%8A%D9%94%D8%AD%D8%A9%20%D8%A7%D9%84%D9%85%D9%86%D8%B8%D9%85%D8%A9%20%D9%84%D9%84%D8%AF%D8%B1%D8%A7%D8%B3%D8%A7%D8%AA%20%D8%A7%D9%84%D8%B9%D9%84%D9%8A%D8%A7%20%D9%81%D9%8A%20%D8%A7%D9%84%D8%AC%D8%A7%D9%85%D8%B9%D8%A7%D8%AA.pdf", IconClass = "hgi-book-open-01" });
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("EReg2", "وثيقة حقوق والتزامات الطالبات بالجامعة", "Student Rights & Obligations Charter"), Description = GetResource("EReg2Desc", "وثيقةٌ معتمدةٌ من عمادة شؤون الطالبات تبيّن حقوق الطالب/ـة والتزاماتها في البيئة الجامعية.", "Approved charter defining student rights and obligations in the university environment."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/studaffairs/Pages/default.aspx", IconClass = "hgi-book-open-01" });
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("EReg3", "لائحة إسكان الطالبات بالجامعة", "Student Housing Bylaws"), Description = GetResource("EReg3Desc", "تنظّم ضوابط السكن الجامعي وحقوق الطالب/ـة المقيمة والتزاماتها.", "Regulates campus accommodation rules and resident student obligations."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/studaffairs/Lists/Guidelines/Attachments/6/%D9%84%D8%A7%D8%A6%D8%AD%D8%A9%20%D8%A7%D8%B3%D9%83%D8%A7%D9%86%20%D8%A7%D9%84%D8%B7%D8%A7%D9%84%D8%A8%D8%A7%D8%AA.pdf", IconClass = "hgi-book-open-01" });
                Regulations.Add(new DgaPgResourceCardModel { Title = GetResource("EReg4", "ميثاق طالبة جامعة الأميرة نورة", "PNU Student Code of Ethics"), Description = GetResource("EReg4Desc", "يبيّن القيم والسمات المؤسسية المتوقّعة من الطالب/ـة والخريجة، ضمن ملاحق الدليل الإرشادي.", "Outlines institutional values and behavioral attributes expected of students and alumnae."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf", IconClass = "hgi-book-open-01" });
            }

            EnsureDefaultProcedures();

            if (PublicationIncentive == null)
            {
                PublicationIncentive = new DgaPgContentBlockModel
                {
                    Title = GetResource("PubTitle", "حافز النشر العلمي لطالبات الدراسات العليا", "Scientific Publication Incentive for Postgraduate Students"),
                    Description = GetResource("PubDesc", "مبادرة تهدف إلى تشجيع طالبات الدراسات العليا على نشر أبحاثهن العلمية المستلّة من الرسائل في أوعية نشرٍ عالميةٍ مصنّفة ومحكّمة.", "An initiative aiming to motivate postgraduate students to publish high-impact research derived from theses in indexed international journals."),
                    Bullets = new List<string> {
                        GetResource("PubB1", "أن تكون الورقة العلمية مستلّة من رسالة الطالبة ومسجلة باسم جامعة الأميرة نورة.", "Paper must be derived from the thesis and affiliated with Princess Nourah University."),
                        GetResource("PubB2", "أن يكون النشر في مجلات مصنفة ضمن قواعد بيانات Web of Science (WOS) أو Scopus في الفئات المعتمدة.", "Published in journals indexed in Web of Science (WOS) or Scopus within approved quartiles."),
                        GetResource("PubB3", "تقديم طلب الصرف عبر وكالة الدراسات العليا والبحث العلمي وفق النماذج المعتمدة.", "Submit application via Graduate Studies & Scientific Research Agency.")
                    },
                    LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/PublicationIncentive.aspx",
                    LinkText = GetResource("PubBtn", "الإعلان والشروط والتقديم", "Announcement, Rules & Application"),
                    IconClass = "hgi-award-02"
                };
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

            if (Guides.Count == 0)
            {
                Guides.Add(new DgaPgResourceCardModel { Title = GetResource("G1Title", "دليل إعداد خطة البحث والرسالة العلمية", "Thesis & Proposal Preparation Manual"), Description = GetResource("G1Desc", "إرشاداتٌ لإعداد الخطة البحثية وكتابة الرسالة العلمية وتنسيقها.", "Guidelines for preparing the research proposal and formatting the thesis."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%b9%d8%af%d8%a7%d8%af%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%88%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7.pdf", IconClass = "hgi-book-open-01" });
                Guides.Add(new DgaPgResourceCardModel { Title = GetResource("G2Title", "دليل كتابة الرسائل العلمية للدراسات العليا", "Thesis Writing Manual for Postgraduate Studies"), Description = GetResource("G2Desc", "الدليل الإرشادي المعتمد لكتابة الرسائل العلمية وتوثيق المراجع.", "Approved manual for structuring, formatting and referencing theses."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/ThesisGuide.aspx", IconClass = "hgi-book-open-01" });
                Guides.Add(new DgaPgResourceCardModel { Title = GetResource("G3Title", "دليل استخدام بوابة القبول الإلكتروني", "Online Portal User Guide"), Description = GetResource("G3Desc", "خطوات استخدام بوابة القبول والأنظمة الإلكترونية المصاحبة.", "Instructions for navigating admission and academic portals."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/PortalGuide.aspx", IconClass = "hgi-book-open-01" });
                Guides.Add(new DgaPgResourceCardModel { Title = GetResource("G4Title", "الدليل الإجرائي لمسار الإجراءات الأكاديمية", "Academic Operational Procedures Manual"), Description = GetResource("G4Desc", "تفصيلٌ لمسار كل إجراءٍ أكاديميٍّ خطوةً بخطوة.", "Step-by-step detail of every academic operational procedure."), LinkUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d9%8a%d9%94%d9%8a%20%d9%84%d9%85%d8%b3%d8%a7%d8%b1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d8%a1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%94%d9%83%d8%a7%d8%af%d9%8a%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf", IconClass = "hgi-book-open-01" });
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
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("EFaqQ1", "أين أجد جدولي الدراسي؟", "Where do I find my course schedule?"), Answer = GetResource("EFaqA1", "تجدين جدولكِ الدراسي في نظام إدارة التعلم الإلكتروني «Blackboard» بعد الدخول إلى صفحتكِ باسم المستخدم وكلمة المرور الجامعية.", "Your schedule is accessible on Blackboard using your official university credentials.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("EFaqQ2", "كيف أدخل على البلاك بورد؟", "How do I log into Blackboard?"), Answer = GetResource("EFaqA2", "تدخلين إلى نظام إدارة التعلم الإلكتروني «Blackboard» باستخدام حسابكِ الجامعي، عبر اسم المستخدم وكلمة المرور المعتمدة للأنظمة الجامعية.", "Access Blackboard LMS using your active university username and password.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("EFaqQ3", "ماذا أفعل إذا لم يظهر جدولي الدراسي؟", "What should I do if my schedule does not appear?"), Answer = GetResource("EFaqA3", "راجعي القسم أو البرنامج الأكاديمي أولاً للتأكد من حالة تسجيلكِ والمقررات المطروحة، ثم تابعي مع الجهة المختصة بحسب الإجراء المعتمد.", "Contact your department or program coordinator to verify registration status and offered courses.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("EFaqQ4", "ما الأنظمة الإلكترونية التي أحتاج استخدامها خلال دراستي؟", "What electronic systems do I need during my studies?"), Answer = GetResource("EFaqA4", "تحتاجين إلى متابعة البريد الجامعي، ونظام إدارة التعلم الإلكتروني «Blackboard»، والنظام الأكاديمي، وبوابة الخدمات الإلكترونية.", "You need to monitor university email, Blackboard LMS, the Student Information System, and the E-Services portal.") });
                FaqItems.Add(new DgaPgFaqItemModel { Question = GetResource("EFaqQ5", "ما الحد الأدنى للمعدل التراكمي للاستمرار في البرنامج؟", "What is the minimum GPA required to continue?"), Answer = GetResource("EFaqA5", "يُشترط ألا يقل المعدل التراكمي للطالبة عن تقدير «جيد جداً» (3.75 من 5.00) طوال فترة دراستها في برامج الدراسات العليا.", "Postgraduate students must maintain a cumulative GPA of at least 'Very Good' (3.75/5.00).") });
            }
        }

        private void EnsureDefaultProcedures()
        {
            var defaults = BuildDefaultProcedures();
            if (Procedures.Count == 0)
            {
                Procedures.AddRange(defaults);
                return;
            }

            // If SharePoint list has partial data (e.g. only 4 procedures), merge the remaining defaults
            foreach (var def in defaults)
            {
                bool exists = Procedures.Exists(p => 
                    string.Equals(p.Title, def.Title, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(p.BadgeText) && string.Equals(p.BadgeText, def.BadgeText, StringComparison.OrdinalIgnoreCase)));
                if (!exists)
                {
                    Procedures.Add(def);
                }
            }

            // Ensure SubGroupKey is properly defaulted for all items
            foreach (var p in Procedures)
            {
                if (string.IsNullOrEmpty(p.SubGroupKey))
                {
                    var match = defaults.Find(d => string.Equals(d.Title, p.Title, StringComparison.OrdinalIgnoreCase));
                    p.SubGroupKey = match != null ? match.SubGroupKey : "College";
                }
            }
        }

        private List<DgaPgProcedureModel> BuildDefaultProcedures()
        {
            var list = new List<DgaPgProcedureModel>();

            // 3.1 إجراءات أكاديمية تبدأ من الكلية
            var p1 = new DgaPgProcedureModel { Id = 1, SubGroupKey = "College", Title = GetResource("P1Title", "معادلة المقررات الدراسية", "Course Equivalency / Credit Transfer"), BadgeText = GetResource("P1Badge", "المادة التاسعة عشر", "Article 19"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-equivalency.png" };
            p1.ShortDescription = GetResource("P1Desc", "معادلة مقرراتٍ دراسيةٍ سبق للطالب/ـة اجتيازُها في جامعةٍ أو مؤسسةٍ تعليميةٍ مرخّصةٍ من جهة الاختصاص، وإثباتها في السجل الأكاديمي.", "Transfer of study courses previously passed at an accredited university or institution.");
            p1.Requirements.Add(GetResource("P1R1", "ألا تقلّ المقررات المُعادَلة عمّا تسمح به الخطة الدراسية للبرنامج.", "Transferred courses must comply with program plan limits."));
            p1.Requirements.Add(GetResource("P1R2", "أن تكون الجهة المانحة للمقرر مرخّصةً من جهة الاختصاص في بلد الدراسة.", "Issuing institution must be accredited by authorized entity."));
            p1.Requirements.Add(GetResource("P1R3", "أن يصدر بالمعادلة قرارٌ من مجلس الكلية بناءً على توصية القسم، وتُثبَت في السجل الأكاديمي.", "Equivalency approved by College Council upon Department recommendation."));
            p1.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art19Head", "المادة التاسعة عشرة", "Article 19"), Body = GetResource("Art19Body", "يجوز معادلة مقررات دراسية سبق للطالب اجتيازها في جامعة أو مؤسسة تعليمية داخل المملكة، أو من خارج المملكة على أن تكون مرخصة من جهة الاختصاص، بناءً على توصية مجلس القسم، وموافقة من مجلس الكلية...", "Courses passed at accredited institutions inside or outside the Kingdom may be transferred upon Department and College recommendation...") });
            p1.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormEquiv", "نموذج استمارة معادلة المقررات الدراسية", "Course Equivalency Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a7%d8%b3%d8%aa%d9%85%d8%a7%d8%b1%d8%a9%20%d9%85%d8%b9%d8%a7%d8%af%d9%84%d8%a9%20%d9%85%d9%82%d8%b1%d8%b1%d8%a7%d8%aa%20%d8%af%d8%b1%d8%a7%d8%b3%d9%8a%d8%a9.docx" });
            p1.FlowText = GetResource("P1Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم ← موافقة مجلس الكلية ← إثبات المقررات في السجل الأكاديمي.", "Procedure workflow: Student -> Department Council -> College Council -> SIS Recording.");
            list.Add(p1);

            var p2 = new DgaPgProcedureModel { Id = 2, SubGroupKey = "College", Title = GetResource("P2Title", "تسجيل خطة البحث للرسالة العلمية", "Thesis Proposal Registration"), BadgeText = GetResource("P2Badge", "المادة السادسة والثلاثون", "Article 36"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-research-plan.png" };
            p2.ShortDescription = GetResource("P2Desc", "اعتماد موضوع الرسالة العلمية وخطتها البحثية، وتحديد المشرف عليها، وفق القواعد المنظِّمة التي يقرّها مجلس الجامعة.", "Approval of thesis topic and proposal and supervisor assignment.");
            p2.Requirements.Add(GetResource("P2R1", "ألا يقلّ المنجَز من المقررات الدراسية عن 50% قبل التقدّم بتسجيل الخطة.", "At least 50% of coursework must be completed before registration."));
            p2.Requirements.Add(GetResource("P2R2", "ألا يقلّ المعدل التراكمي عن تقدير «جيد جداً».", "Cumulative GPA must not be lower than 'Very Good'."));
            p2.Requirements.Add(GetResource("P2R3", "سلامة الخطة فكرياً وخلوّها من المؤلفات المحظورة، وتقدّمها المرشدة الأكاديمية للقسم المختص.", "Proposal intellectual integrity and compliance with academic standards."));
            p2.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art36Head", "المادة السادسة والثلاثون", "Article 36"), Body = GetResource("Art36Body", "يضع مجلس الجامعة القواعد المنظمة لتسجيل مشروع بحث التخرج، والرسالة العلمية لطالب الدراسات العليا — إن وجدت — وآلية تحديد المشرف...", "The University Council establishes the rules governing thesis and graduation research project registration...") });
            p2.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormProp", "نموذج تسجيل خطة البحث للرسالة العلمية", "Thesis Proposal Registration Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d8%b3%d8%ac%d9%8a%d9%84%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%84%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx" });
            p2.FlowText = GetResource("P2Flow", "مسار الإجراء الأكاديمي: الطالب/ـة والمشرف ← توصية مجلس القسم ← مجلس الكلية ← اعتماد الخطة وتحديد المشرف.", "Procedure workflow: Student & Advisor -> Department Council -> College Council -> Approval.");
            list.Add(p2);

            var p3 = new DgaPgProcedureModel { Id = 3, SubGroupKey = "College", Title = GetResource("P3Title", "إلغاء القيد", "Enrollment Cancellation"), BadgeText = GetResource("P3Badge", "المادة السادسة والعشرون", "Article 26"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-cancel.png" };
            p3.ShortDescription = GetResource("P3Desc", "إنهاء الجامعة علاقتَها بالطالب/ـة قبل إكمال برنامجها الدراسي لأسبابٍ محدّدةٍ نصّت عليها اللائحة.", "Termination of university relationship prior to completion due to statutory reasons.");
            p3.Requirements.Add(GetResource("P3R1", "يقع إلغاء القيد عند انطباق إحدى الحالات النظامية (الانقطاع، أو انخفاض المعدل عن «جيد جداً» في فصلين متتاليين).", "Cancellation occurs upon applicable statutory cases."));
            p3.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art26Head", "المادة السادسة والعشرون", "Article 26"), Body = GetResource("Art26Body", "يُلغى قيد الطالب في الحالات الآتية: إذا انقطع عن الدراسة؛ إذا لم يجتز المقررات التكميلية؛ إذا انخفض معدله التراكمي عن (جيد جداً) في فصلين متتاليين...", "Student enrollment is cancelled in statutory cases including discontinuation or low GPA...") });
            p3.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormCancel", "نموذج إلغاء قيد", "Enrollment Cancellation Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d9%84%d8%ba%d8%a7%d8%a1%20%d9%82%d9%8a%d8%af.docx" });
            p3.FlowText = GetResource("P3Flow", "مسار الإجراء الأكاديمي: توصية مجلس القسم ← تأييد مجلس الكلية ← قرار اللجنة الدائمة ← الإدارة التنفيذية للتنفيذ.", "Procedure workflow: Department -> College -> Standing Committee -> Execution.");
            list.Add(p3);

            var p4 = new DgaPgProcedureModel { Id = 4, SubGroupKey = "College", Title = GetResource("P4Title", "الإنذار الأكاديمي", "Academic Warning"), BadgeText = GetResource("P4Badge", "المادة الرابعة والأربعون", "Article 44"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-warning.png" };
            p4.ShortDescription = GetResource("P4Desc", "إشعارٌ نظاميٌّ يوجَّه إلى الطالب/ـة عند تعثّر مسارها الأكاديمي؛ سواء بانخفاض المعدل أو بثبوت عدم الجدّية في مرحلة الرسالة.", "Official notice issued upon academic stumbling or lack of thesis progress.");
            p4.Requirements.Add(GetResource("P4R1", "يُوجَّه الإنذار عند ثبوت عدم الجدية في مرحلة الرسالة بناءً على تقرير المشرف، أو عند انخفاض المعدل.", "Warning issued upon lack of progress based on advisor report or GPA decline."));
            p4.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art44Head", "المادة الرابعة والأربعون", "Article 44"), Body = GetResource("Art44Body", "إذا ثبت عدم جدية الطالب في مرحلة الرسالة يُوجَّه له إنذار، وإذا أُنذر مرتين ولم يتلافَ أسباب الإنذار جاز لمجلس الجامعة إلغاء قيده...", "If thesis progress is unsatisfactory, a warning is issued; two unaddressed warnings may lead to cancellation...") });
            p4.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormWarn", "نموذج إنذار طالبة", "Academic Warning Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%86%d8%b0%d8%a7%d8%b1.docx" });
            p4.FlowText = GetResource("P4Flow", "مسار الإجراء الأكاديمي: القسم المختص ← مجلس الكلية ← إشعار الطالب/ـة.", "Procedure workflow: Department -> College -> Warning Issued & Student Notified.");
            list.Add(p4);

            var p5 = new DgaPgProcedureModel { Id = 5, SubGroupKey = "College", Title = GetResource("P5Title", "إعادة القيد", "Re-Enrollment"), BadgeText = GetResource("P5Badge", "المادة السابعة والعشرون", "Article 27"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-reinstate.png" };
            p5.ShortDescription = GetResource("P5Desc", "إعادة الجامعة علاقتَها بالطالب/ـة التي أُلغي قيدها، عند توافر ظروفٍ قهريةٍ حالت دون مواصلة الدراسة.", "Reinstatement of cancelled student upon acceptable extenuating circumstances.");
            p5.Requirements.Add(GetResource("P5R1", "أن يكون الحائل دون مواصلة الدراسة ظرفاً قهرياً يقبله مجلسا القسم والكلية.", "Acceptable extenuating circumstances verified by councils."));
            p5.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art27Head", "المادة السابعة والعشرون", "Article 27"), Body = GetResource("Art27Body", "يجوز إعادة قيد الطالب الذي أُلغي قيده إذا كان الحائل دون مواصلة دراسته ظروفاً يقبلها مجلسا القسم والكلية، وتكون بقرار من اللجنة الدائمة...", "Reinstatement may be granted upon valid circumstances approved by councils...") });
            p5.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormReinstate", "نموذج إعادة قيد", "Reinstatement Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%b9%d8%a7%d8%af%d8%a9%20%d9%82%d9%8a%d8%af.docx" });
            p5.FlowText = GetResource("P5Flow", "مسار الإجراء الأكاديمي: قبول مجلسي القسم والكلية للظروف ← قرار اللجنة الدائمة ← الإدارة التنفيذية للتنفيذ.", "Workflow: Councils Acceptance -> Committee Decision -> Execution.");
            list.Add(p5);

            var p6 = new DgaPgProcedureModel { Id = 6, SubGroupKey = "College", Title = GetResource("P6Title", "تشكيل لجنة مناقشة للرسالة العلمية", "Thesis Defense Committee Formation"), BadgeText = GetResource("P6Badge", "المادة الثامنة والأربعون", "Article 48"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-committee.png" };
            p6.ShortDescription = GetResource("P6Desc", "تكوين لجنةٍ لمناقشة الرسالة العلمية بعد تقديم المشرف تقريراً باكتمالها إلى رئيس القسم.", "Forming a defense committee after supervisor submits completion report.");
            p6.Requirements.Add(GetResource("P6R1", "أن يتقدّم المشرف بتقرير اكتمال الرسالة إلى رئيس القسم، مرفقاً به نسخةٌ منها.", "Supervisor submits completion report with thesis copy."));
            p6.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art48Head", "المادة الثامنة والأربعون", "Article 48"), Body = GetResource("Art48Body", "تُكوَّن لجنة لمناقشة الرسالة العلمية بقرار من مجلس الكلية، بناءً على توصية مجلس القسم...", "Defense committee is formed by College Council upon Department recommendation...") });
            p6.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormComm1", "نموذج ترشيح لجنة مناقشة الرسالة العلمية", "Defense Committee Nomination Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d8%b1%d8%b4%d9%8a%d8%ad%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx" });
            p6.FlowText = GetResource("P6Flow", "مسار الإجراء الأكاديمي: تقرير المشرف باكتمال الرسالة ← توصية مجلس القسم ← قرار مجلس الكلية بتكوين اللجنة ← المناقشة.", "Workflow: Advisor Report -> Department -> College Decision -> Defense.");
            list.Add(p6);

            var p7 = new DgaPgProcedureModel { Id = 7, SubGroupKey = "College", Title = GetResource("P7Title", "الفرصة الإستثنائية لرفع المعدل", "Extra Chance for GPA Improvement"), BadgeText = GetResource("P7Badge", "المادة الثامنة والعشرون", "Article 28"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-gpa.png" };
            p7.ShortDescription = GetResource("P7Desc", "منح الطالب/ـة فرصةً استثنائيةً واحدةً لا تزيد على عامٍ دراسيٍّ كحدٍّ أعلى؛ لرفع معدلها التراكمي.", "Granting single extraordinary chance up to one year to raise cumulative GPA.");
            p7.Requirements.Add(GetResource("P7R1", "أن تكون فرصةً استثنائيةً واحدةً لا تزيد على عامٍ دراسيٍّ كحدٍّ أعلى.", "Single exceptional chance not exceeding one academic year."));
            p7.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art28Head", "المادة الثامنة والعشرون", "Article 28"), Body = GetResource("Art28Body", "لمجلس الكلية، استثناءً من اللائحة، منح الطالب فرصة استثنائية واحدة لا تزيد على عام دراسي كحد أعلى، بناءً على توصية مجلس القسم.", "College Council may grant an exceptional chance up to one year upon Department recommendation.") });
            p7.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormGpa", "نموذج طلب فرصة استثنائية لرفع المعدل", "GPA Extra Chance Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%b7%d9%84%d8%a8%20%d9%81%d8%b1%d8%b5%d8%a9%20%d8%a7%d8%b3%d8%aa%d8%ab%d9%86%d8%a7%d9%8a%d9%94%d9%8a%d8%a9%20%d9%84%d8%b1%d9%81%d8%b9%20%d8%a7%d9%84%d9%85%d8%b9%d8%af%d9%84%20%d8%a7%d9%84%d8%aa%d8%b1%d8%a7%d9%83%d9%85%d9%8a.docx" });
            p7.FlowText = GetResource("P7Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم المختص ← قرار مجلس الكلية بالاستثناء.", "Workflow: Student -> Department -> College Council Approval.");
            list.Add(p7);

            var p8 = new DgaPgProcedureModel { Id = 8, SubGroupKey = "College", Title = GetResource("P8Title", "منح الدرجة العلمية", "Conferral of Academic Degree"), BadgeText = GetResource("P8Badge", "المواد ٥٠ و ٥٣ و ٥٦", "Articles 50, 53 & 56"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-degree.png" };
            p8.ShortDescription = GetResource("P8Desc", "تخرّج الطالب/ـة بعد إنهاء متطلبات البرنامج بنجاح، ورفع التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.", "Graduation upon fulfilling requirements and recommending degree conferral.");
            p8.Requirements.Add(GetResource("P8R1", "اكتمال متطلبات التخرج بنجاح حسب خطة البرنامج، وألا يقلّ المعدل التراكمي عن «جيد جداً».", "Successful completion of graduation plan with minimum GPA of Very Good."));
            p8.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art56Head", "المادة السادسة والخمسون", "Article 56"), Body = GetResource("Art56Body", "يرفع النائب أو الوكيل المختص التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.", "Vice Rector submits degree conferral recommendation to University Council.") });
            p8.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormDegree", "نموذج تقرير لجنة مناقشة الرسالة العلمية", "Defense Committee Report"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d9%82%d8%b1%d9%8a%d8%b1%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx" });
            p8.FlowText = GetResource("P8Flow", "مسار الإجراء الأكاديمي: تقرير لجنة المناقشة ← رئيس القسم ← عميد الكلية ← الإدارة التنفيذية ← مجلس الجامعة.", "Workflow: Committee Report -> Department -> Dean -> Council.");
            list.Add(p8);

            var p9 = new DgaPgProcedureModel { Id = 9, SubGroupKey = "College", Title = GetResource("P9Title", "الفرصة الاستثنائية للحصول على الدرجة", "Extra Chance for Degree Completion"), BadgeText = GetResource("P9Badge", "المادة التاسعة والعشرون", "Article 29"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-degree-opportunity.png" };
            p9.ShortDescription = GetResource("P9Desc", "منح الطالب/ـة فرصةً استثنائيةً لا تزيد على عامٍ دراسي؛ لاستكمال متطلبات الحصول على الدرجة.", "Granting extraordinary chance up to one year to complete degree requirements.");
            p9.Requirements.Add(GetResource("P9R1", "أن تُقدَّم قبل نهاية المدة النظامية للبرنامج وتصدر بقرار اللجنة الدائمة.", "Submitted prior to maximum duration limit and approved by Standing Committee."));
            p9.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art29Head", "المادة التاسعة والعشرون", "Article 29"), Body = GetResource("Art29Body", "للجنة الدائمة، استثناءً من اللائحة، منح الطالب فرصة استثنائية لا تزيد على عام دراسي، بناءً على تقرير من المشرف وتوصية مجلسي القسم والكلية.", "Standing Committee may grant exceptional chance up to one year based on advisor and council reports.") });
            p9.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormDegreeChance", "نموذج طلب فرصة استثنائية للحصول على الدرجة", "Degree Extra Chance Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%b7%d9%84%d8%a8%20%d9%81%d8%b1%d8%b5%d8%a9%20%d8%a7%d8%b3%d8%aa%d8%ab%d9%86%d8%a7%d9%8a%d9%94%d9%8a%d8%a9%20%d9%84%d9%84%d8%ad%d8%b5%d9%88%d9%84%20%d8%b9%d9%84%d9%89%20%d8%a7%d9%84%d8%af%d8%b1%d8%ac%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9%20%281%29.docx" });
            p9.FlowText = GetResource("P9Flow", "مسار الإجراء الأكاديمي: تقرير المشرف ← توصية مجلسي القسم والكلية ← قرار اللجنة الدائمة بالاستثناء.", "Workflow: Advisor Report -> Councils -> Committee Approval.");
            list.Add(p9);

            var p10 = new DgaPgProcedureModel { Id = 10, SubGroupKey = "College", Title = GetResource("P10Title", "تعديل النتيجة", "Grade Amendment"), BadgeText = GetResource("P10Badge", "قواعد تنفيذية", "Executive Rules"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-result-edit.png" };
            p10.ShortDescription = GetResource("P10Desc", "تصحيح نتيجة مقررٍ بعد رصدها وفق الضوابط المقرّة وبالمسوّغات المعتمدة.", "Grade correction post-recording according to approved justifications.");
            p10.Requirements.Add(GetResource("P10R1", "أن يكون التعديل بمسوّغٍ معتمدٍ ووفق القواعد التنفيذية لرصد الدرجات.", "Must have certified justification under grading executive rules."));
            p10.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("ArtResHead", "الأساس النظامي", "Statutory Basis"), Body = GetResource("ArtResBody", "يستند تعديل النتيجة إلى القواعد التنفيذية المعتمدة لرصد الدرجات وتعديلها وفقاً لما تقرّه الجامعة.", "Grade amendments follow university-approved grading executive rules.") });
            p10.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormGradeEdit", "نموذج تعديل نتيجة طالبة", "Grade Amendment Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d8%b9%d8%af%d9%8a%d9%84%20%d9%86%d8%aa%d9%8a%d8%ac%d8%a9%20%d8%b7%d8%a7%d9%84%d8%a8%d8%a9.docx" });
            p10.FlowText = GetResource("P10Flow", "مسار الإجراء الأكاديمي: عضو هيئة التدريس ← مجلس القسم ← مجلس الكلية ← الإدارة التنفيذية للتنفيذ.", "Workflow: Faculty Member -> Department -> College -> SIS Execution.");
            list.Add(p10);

            var p11 = new DgaPgProcedureModel { Id = 11, SubGroupKey = "College", Title = GetResource("P11Title", "إخلاء الطرف", "Clearance"), BadgeText = GetResource("P11Badge", "إجراءات معتمدة", "Approved Procedures"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-clearance.png" };
            p11.ShortDescription = GetResource("P11Desc", "استكمال الطالب/ـة براءةَ ذمّتها تجاه وحدات الجامعة عند الانسحاب أو التخرج أو إلغاء القيد.", "Student clearance across university departments upon graduation or withdrawal.");
            p11.Requirements.Add(GetResource("P11R1", "استكمال براءة الذمة تجاه جميع وحدات الجامعة ذات العلاقة.", "Obtaining approvals from all relevant university departments."));
            p11.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("ArtClearHead", "الأساس الإجرائي", "Procedural Basis"), Body = GetResource("ArtClearBody", "يُستكمل إخلاء الطرف وفق الإجراءات المعتمدة بالعمادة كخطوة ختامية.", "Clearance is processed under approved protocols as a prerequisite.") });
            p11.Forms.Add(new DgaPgLinkItem { Title = GetResource("FormClear", "نموذج إخلاء طرف", "Clearance Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%ae%d9%84%d8%a7%d8%a1%20%d8%b7%d8%b1%d9%81.docx" });
            p11.FlowText = GetResource("P11Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← الوحدات ذات العلاقة ← اعتماد إخلاء الطرف.", "Workflow: Student -> Departments -> Clearance Approval.");
            list.Add(p11);

            // 3.2 إجراءات أكاديمية تبدأ من الطالبة
            var p12 = new DgaPgProcedureModel { Id = 12, SubGroupKey = "Student", Title = GetResource("P12Title", "تأجيل القبول", "Admission Postponement"), BadgeText = GetResource("P12Badge", "المادة العشرون", "Article 20"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-admission-delay.png", DirectServiceUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/نموذج تأجيل القبول.docx" };
            p12.ShortDescription = GetResource("P12Desc", "إرجاء تاريخ بدء دراسة الطالب/ـة بعد صدور رقمها الجامعي وقبل تسجيل أي مقرر دراسي، ويكون مرةً واحدة، ولا يُقبل بعد بدء الدراسة.", "Postponing admission start date prior to course registration for a single time.");
            p12.Requirements.Add(GetResource("P12R1", "أن يكون التأجيل قبل تسجيل أي مقرر دراسي، ولمرّةٍ واحدةٍ فقط.", "Must be requested before registering any courses, once only."));
            p12.Requirements.Add(GetResource("P12R2", "صدور موافقة مجلس القسم المختص وعميد الكلية.", "Approved by Department Council and College Dean."));
            p12.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art20Head", "المادة العشرون", "Article 20"), Body = GetResource("Art20Body", "يجوز بموافقة مجلس القسم المختص وعميد الكلية تأجيل الطالب قبوله، ويكون لمرة واحدة، ولا يمكن قبول التأجيل بعد بدء الطالب دراسته...", "Student may postpone admission once prior to studies start upon council approval...") });
            p12.FlowText = GetResource("P12Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← موافقة مجلس القسم المختص ← موافقة عميد الكلية ← الإدارة التنفيذية للتنفيذ.", "Workflow: Student -> Department Council -> Dean -> Execution.");
            list.Add(p12);

            var p13 = new DgaPgProcedureModel { Id = 13, SubGroupKey = "Student", Title = GetResource("P13Title", "الاعتذار عن دراسة مقرر أو فصل", "Course / Semester Apology (Withdrawal)"), BadgeText = GetResource("P13Badge", "المادة الثالثة والعشرون", "Article 23"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-course-apology.png", DirectServiceUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/نموذج الاعتذار  عن الاستمرار  في دراسة مقرر أو أكثر أو جميع مقررات الفصل الدراسي.docx" };
            p13.ShortDescription = GetResource("P13Desc", "إنهاء الطالب/ـة دراستَها لمقرر أو أكثر، أو لجميع مقررات الفصل الدراسي، بعد انتهاء فترة التسجيل.", "Discontinuing one or more courses or entire semester after registration period.");
            p13.Requirements.Add(GetResource("P13R1", "أن يكون الاعتذار بعد انتهاء فترة التسجيل وضمن المدة المحددة في التقويم الأكاديمي.", "Must be submitted within deadline set in academic calendar."));
            p13.Requirements.Add(GetResource("P13R2", "صدور توصية مجلس القسم المختص وموافقة عميد الكلية.", "Approved by Department Council and College Dean."));
            p13.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art23Head", "المادة الثالثة والعشرون", "Article 23"), Body = GetResource("Art23Body", "يجوز للطالب الاعتذار عن الاستمرار في دراسة مقرر أو أكثر، أو جميع مقررات الفصل الدراسي، بعد انتهاء فترة التسجيل بتوصية مجلس القسم وموافقة العميد...", "Student may apologize from courses or semester upon Department and Dean approval...") });
            p13.FlowText = GetResource("P13Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم المختص ← موافقة عميد الكلية ← الإدارة التنفيذية للتنفيذ.", "Workflow: Student -> Department -> Dean -> Execution.");
            list.Add(p13);

            var p14 = new DgaPgProcedureModel { Id = 14, SubGroupKey = "Student", Title = GetResource("P14Title", "تأجيل الدراسة", "Postponement of Studies"), BadgeText = GetResource("P14Badge", "المادة الثانية والعشرون", "Article 22"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-study-delay.png", DirectServiceUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/نموذج تأجيل الدراسة.docx" };
            p14.ShortDescription = GetResource("P14Desc", "إيقاف الطالب/ـة سريانَ المدة المقررة للحصول على الدرجة العلمية بعد بدء دراستها.", "Temporarily suspending academic duration progression after starting studies.");
            p14.Requirements.Add(GetResource("P14R1", "أن يصل الطلب مكتملاً للإدارة التنفيذية في موعدٍ أقصاه نهاية الأسبوع الثالث من بدء الفصل.", "Must be submitted by end of 3rd week of the semester."));
            p14.Requirements.Add(GetResource("P14R2", "ألا يتجاوز التأجيل فصلين دراسيين متتاليين، وألا يتجاوز مجموع مدة التأجيل نصف المدة المقررة للبرنامج.", "Max 2 consecutive semesters and not exceeding half the program duration."));
            p14.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art22Head", "المادة الثانية والعشرون", "Article 22"), Body = GetResource("Art22Body", "يجوز للطالب تأجيل الدراسة بتوصية من مجلس القسم المختص وموافقة مجلس الكلية وفق ضوابط يقرها مجلس الجامعة...", "Student may postpone studies upon Department and College recommendation...") });
            p14.FlowText = GetResource("P14Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم المختص ← موافقة مجلس الكلية ← الإدارة التنفيذية للتنفيذ.", "Workflow: Student -> Department -> College Council -> Execution.");
            list.Add(p14);

            var p15 = new DgaPgProcedureModel { Id = 15, SubGroupKey = "Student", Title = GetResource("P15Title", "الانسحاب", "Withdrawal"), BadgeText = GetResource("P15Badge", "المادة الرابعة والعشرون", "Article 24"), IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-withdrawal.png", DirectServiceUrl = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/2نموذج أنسحاب.docx" };
            p15.ShortDescription = GetResource("P15Desc", "إنهاء الطالب/ـة علاقتَها بالجامعة المقيَّدة بها بشكلٍ نهائي، قبل إكمال برنامجها الدراسي.", "Final termination of student relationship with the university prior to completion.");
            p15.Requirements.Add(GetResource("P15R1", "أن يكون الانسحاب قبل إكمال البرنامج الدراسي واستكمال إجراءات إخلاء الطرف.", "Must be before program completion along with full university clearance."));
            p15.Articles.Add(new DgaPgArticleBlock { Heading = GetResource("Art24Head", "المادة الرابعة والعشرون", "Article 24"), Body = GetResource("Art24Body", "إذا انسحب طالب الدراسات العليا من الجامعة ثم أراد الالتحاق بها مجدداً يجوز للجامعة أن تطبق عليه شروط القبول وقت التقديم الجديد.", "If a student withdraws and wishes to re-apply later, admission rules at time of re-application apply.") });
            p15.FlowText = GetResource("P15Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← القسم المختص ← الكلية ← الإدارة التنفيذية، مع إخلاء الطرف.", "Workflow: Student -> Department -> College -> Execution with Clearance.");
            list.Add(p15);

            return list;
        }

        // ---------------------------------------------------------------
        // Section anchors and table of contents
        // ---------------------------------------------------------------

        protected override string SectionAnchor(string sectionKey)
        {
            switch (sectionKey)
            {
                case DgaPgSectionKeys.Intro:            return "enrolled-intro";
                case DgaPgSectionKeys.Journey:          return "enrolled-section-1";
                case DgaPgSectionKeys.Regulations:      return "enrolled-section-2";
                case DgaPgSectionKeys.Procedures:       return "enrolled-section-3";
                case DgaPgSectionKeys.Faq:              return "enrolled-section-4";
                case DgaPgSectionKeys.PublishIncentive: return "enrolled-section-5";
                case DgaPgSectionKeys.Discounts:        return "enrolled-section-6";
                case DgaPgSectionKeys.Guides:           return "enrolled-section-7";
                case DgaPgSectionKeys.Services:         return "enrolled-section-8";
                case DgaPgSectionKeys.Housing:          return "enrolled-section-9";
                case DgaPgSectionKeys.Contact:          return "enrolled-section-10";
                case DgaPgSectionKeys.Reference:        return "enrolled-reference";
                default:                                return "enrolled-" + (sectionKey ?? string.Empty).ToLowerInvariant();
            }
        }

        public List<DgaPgLinkItem> TocItems
        {
            get
            {
                var toc = new List<DgaPgLinkItem>();

                AddToc(toc, DgaPgSectionKeys.Intro, IntroModel.Title, true);
                AddToc(toc, DgaPgSectionKeys.Journey, SectionTitle(DgaPgSectionKeys.Journey), JourneySteps.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Regulations, SectionTitle(DgaPgSectionKeys.Regulations), Regulations.Count > 0 || GraduateIdentity != null || FinancialRules != null);
                AddToc(toc, DgaPgSectionKeys.Procedures, SectionTitle(DgaPgSectionKeys.Procedures), Procedures.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Faq, SectionTitle(DgaPgSectionKeys.Faq), FaqItems.Count > 0);
                AddToc(toc, DgaPgSectionKeys.PublishIncentive, SectionTitle(DgaPgSectionKeys.PublishIncentive), PublicationIncentive != null);
                AddToc(toc, DgaPgSectionKeys.Discounts, SectionTitle(DgaPgSectionKeys.Discounts), DiscountRows.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Guides, SectionTitle(DgaPgSectionKeys.Guides), Guides.Count > 0);
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
                case DgaPgSectionKeys.Journey:          return GetSectionTitle(sectionKey, GetResource("Axis1", "المحور الأول: خريطة الرحلة الأكاديمية", "First Axis: Academic Journey Map"));
                case DgaPgSectionKeys.Regulations:      return GetSectionTitle(sectionKey, GetResource("Axis2", "المحور الثاني: الحقوق والواجبات / اللوائح النظامية", "Second Axis: Rights and Duties / Statutory Regulations"));
                case DgaPgSectionKeys.Procedures:       return GetSectionTitle(sectionKey, GetResource("Axis3", "المحور الثالث: الإجراءات الأكاديمية", "Third Axis: Academic Procedures"));
                case DgaPgSectionKeys.Faq:              return GetSectionTitle(sectionKey, GetResource("Axis4", "المحور الرابع: الأسئلة الأكثر شيوعاً", "Fourth Axis: Frequently Asked Questions"));
                case DgaPgSectionKeys.PublishIncentive: return GetSectionTitle(sectionKey, GetResource("Axis5", "المحور الخامس: حافز النشر العلمي", "Fifth Axis: Research Publication Incentive"));
                case DgaPgSectionKeys.Discounts:        return GetSectionTitle(sectionKey, GetResource("Axis6", "المحور السادس: إعفاء وتخفيض الرسوم الدراسية لفئات محددة من طلبة الدراسات العليا", "Sixth Axis: Tuition Exemptions and Reductions for Specific Categories"));
                case DgaPgSectionKeys.Guides:           return GetSectionTitle(sectionKey, GetResource("Axis7", "المحور السابع: الأدلة الإرشادية", "Seventh Axis: Guides"));
                case DgaPgSectionKeys.Services:         return GetSectionTitle(sectionKey, GetResource("Axis8", "المحور الثامن: الخدمات / المراكز", "Eighth Axis: Services and Centers"));
                case DgaPgSectionKeys.Housing:          return GetSectionTitle(sectionKey, GetResource("Axis9", "المحور التاسع: السكن الجامعي", "Ninth Axis: University Housing"));
                case DgaPgSectionKeys.Contact:          return GetSectionTitle(sectionKey, GetResource("Axis10", "المحور العاشر: تواصلي معنا", "Tenth Axis: Contact Us"));
                default:                                return GetSectionTitle(sectionKey, string.Empty);
            }
        }

        /// <summary>Heading for one of the two axis-3 procedure sub-groups.</summary>
        public string SubGroupTitle(string subGroupKey)
        {
            if (string.Equals(subGroupKey, "Student", StringComparison.OrdinalIgnoreCase))
                return GetResource("SubGroup32", "3.2 إجراءات أكاديمية تبدأ من الطالبة:", "3.2 Academic procedures initiated by the student:");
            return GetResource("SubGroup31", "3.1 إجراءات أكاديمية تبدأ من الكلية:", "3.1 Academic procedures initiated by the college:");
        }

        protected string ResolveHomeUrl()
        {
            return IsArabic ? "/ar/Pages/default.aspx" : "/en/Pages/default.aspx";
        }
    }
}
