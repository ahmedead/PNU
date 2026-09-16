using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Definitions and seed data for the content lists: PgResources, PgContentBlocks,
    /// PgDiscounts, PgDiscountNotes, PgServices and PgQuickLinks.
    /// </summary>
    public static partial class PgListSchema
    {
        // Official documents published on the deanship site, referenced by several sections.
        internal const string DocStudentGuide =
            "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf";

        internal const string DocProceduresGuide =
            "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d9%8a%d9%94%d9%8a%20%d9%84%d9%85%d8%b3%d8%a7%d8%b1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d8%a1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%94%d9%83%d8%a7%d8%af%d9%8a%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf";

        internal const string DocResearchPlanGuide =
            "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%b9%d8%af%d8%a7%d8%af%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%88%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7.pdf";

        internal const string UrlPrograms = "https://pnu.edu.sa/ar/RegAdm/PGD/Pages/home.aspx";
        internal const string UrlEnglishCondition = "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/English.aspx";
        internal const string UrlPeriods = "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/Periods.aspx";
        internal const string UrlStudentAffairs = "https://pnu.edu.sa/ar/Deanship/studaffairs/Pages/default.aspx";
        internal const string UrlHousingService = "https://pnu.edu.sa/ar/Deanship/studaffairs/Pages/sfdsre.aspx";
        internal const string DeanshipEmail = "dgs-aa@pnu.edu.sa";

        private static readonly string[] ResourceGroups = new string[]
        {
            PgSectionKeys.Programs,
            PgSectionKeys.Regulations,
            PgSectionKeys.Guides,
            PgSectionKeys.GradRequirements
        };

        private static void BuildContentLists(Dictionary<string, PgListDef> map)
        {
            BuildResources(map);
            BuildContentBlocks(map);
            BuildDiscounts(map);
            BuildDiscountNotes(map);
            BuildServices(map);
            BuildQuickLinks(map);
        }

        private static string N(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        // 5) البطاقات والوثائق (Resource / document cards)
        private static void BuildResources(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Resources] = new PgListDef
            {
                Name = PgListNames.Resources,
                TitleAr = "البطاقات والوثائق", TitleEn = "Resource and document cards",
                Description = "Link cards for programs, regulations, official guides and graduation requirements.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), DescAr(4), DescEn(4), Link(), Icon(),
                    new PgFieldDef("GroupKey", "المجموعة", "Group", SPFieldType.Choice, true, 0,
                                   "Programs / Regulations / Guides / GradRequirements", ResourceGroups),
                    YesNoField("ShowDownload", "إظهار أيقونة التحميل", "Show download icon"),
                    Phase(), Order()
                },
                Seed = ResourcesSeed()
            };
        }

        private static Dictionary<string, string> Resource(string phase, string group, int order, string icon,
                                                           string titleAr, string titleEn,
                                                           string descAr, string descEn,
                                                           string url, bool showDownload)
        {
            return Row("Title", titleAr, "Title_EN", titleEn,
                       "Description", descAr, "Description_EN", descEn,
                       "LinkUrl", url,
                       "IconUrl", IconRoot + icon,
                       "GroupKey", group,
                       "ShowDownload", showDownload ? "Yes" : "No",
                       "Phase", phase,
                       "ItemOrder", N(order));
        }

        private static List<Dictionary<string, string>> ResourcesSeed()
        {
            var seed = new List<Dictionary<string, string>>();

            // ---------- المتقدمون: البرامج وشروط القبول والرسوم ----------
            seed.Add(Resource(PgPhases.Applicants, PgSectionKeys.Programs, 1, "books.png",
                "برامج الدراسات العليا والرسوم الدراسية", "Postgraduate programs and tuition fees",
                "تصفّح البرامج المتاحة (دبلوم عالٍ، ماجستير، دكتوراه) وتخصّصاتها ورسومها الدراسية — محدّثةٌ دائماً من المصدر.",
                "Browse the available programs (higher diploma, master's, doctorate), their specialisations and their tuition fees, always up to date from the source.",
                UrlPrograms, false));

            seed.Add(Resource(PgPhases.Applicants, PgSectionKeys.Programs, 2, "step02.png",
                "شروط القبول وشرط اللغة الإنجليزية", "Admission requirements and the English language requirement",
                "الاطّلاع على شروط القبول العامة والخاصة بكل برنامج، وشرط اللغة الإنجليزية المعتمد.",
                "Review the general admission requirements, those specific to each program, and the approved English language requirement.",
                UrlEnglishCondition, false));

            seed.Add(Resource(PgPhases.Applicants, PgSectionKeys.Programs, 3, "step03.png",
                "فترات التقديم وبوابة القبول الإلكتروني", "Application periods and the electronic admission portal",
                "معرفة مواعيد فتح القبول والدخول إلى بوابة التقديم الإلكترونية.",
                "Find out the admission opening dates and sign in to the electronic application portal.",
                UrlPeriods, false));

            // ---------- المتقدمون: الحقوق والواجبات / اللوائح النظامية ----------
            seed.Add(Resource(PgPhases.Applicants, PgSectionKeys.Guides, 1, "document.png",
                "دليل المستخدم لبوابة القبول الإلكترونية", "User guide for the electronic admission portal",
                "مرجعٌ يوضّح خطوات التسجيل والتقديم عبر البوابة.",
                "A reference explaining the registration and application steps in the portal.",
                DocStudentGuide, true));

            seed.Add(Resource(PgPhases.Applicants, PgSectionKeys.Guides, 2, "document.png",
                "الدليل الإرشادي لطلبة الدراسات العليا", "The postgraduate students guide",
                "مرجعٌ تعريفيٌّ عامٌّ بنظام الدراسة والإجراءات والخدمات.",
                "A general reference to the study system, the procedures and the services.",
                DocStudentGuide, true));

            seed.Add(Resource(PgPhases.Applicants, PgSectionKeys.Guides, 3, "document.png",
                "الدليل الإجرائي لمسار الإجراءات الأكاديمية", "The procedural guide to the academic procedures",
                "تفصيلٌ لمسار كل إجراءٍ أكاديميٍّ خطوةً بخطوة.",
                "A step-by-step account of the path of every academic procedure.",
                DocProceduresGuide, true));

            seed.Add(Resource(PgPhases.Applicants, PgSectionKeys.Guides, 4, "document.png",
                "دليل إعداد خطة البحث والرسالة العلمية", "Guide to preparing the research plan and the thesis",
                "إرشاداتٌ لإعداد الخطة البحثية وكتابة الرسالة العلمية.",
                "Guidance on preparing the research plan and writing the thesis.",
                DocResearchPlanGuide, true));

            // ---------- المقيدون: الحقوق والواجبات واللوائح النظامية ----------
            seed.Add(Resource(PgPhases.Enrolled, PgSectionKeys.Regulations, 1, "document.png",
                "اللائحة المنظمة للدراسات العليا في الجامعات وقواعدها التنفيذية",
                "The regulation governing postgraduate studies in universities and its executive rules",
                "المصدر الأساس لحقوق طالبة الدراسات العليا وواجباتها الأكاديمية (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤).",
                "The primary source of a postgraduate student's academic rights and duties (Council of University Affairs resolution 2/9/1444).",
                DocStudentGuide, true));

            seed.Add(Resource(PgPhases.Enrolled, PgSectionKeys.Regulations, 2, "document.png",
                "وثيقة حقوق والتزامات الطالبات بالجامعة", "The document of students' rights and obligations",
                "وثيقةٌ معتمدةٌ من عمادة شؤون الطالبات تبيّن حقوق الطالب/ـة والتزاماتها في البيئة الجامعية.",
                "A document approved by the Deanship of Student Affairs setting out a student's rights and obligations within the university environment.",
                UrlStudentAffairs, true));

            seed.Add(Resource(PgPhases.Enrolled, PgSectionKeys.Regulations, 3, "document.png",
                "لائحة إسكان الطالبات بالجامعة", "The student housing regulation",
                "تنظّم ضوابط السكن الجامعي وحقوق الطالب/ـة المقيمة والتزاماتها.",
                "It governs the rules of student housing and the rights and obligations of a resident student.",
                "#", true));

            seed.Add(Resource(PgPhases.Enrolled, PgSectionKeys.Regulations, 4, "document.png",
                "ميثاق طالبة جامعة الأميرة نورة", "The Princess Nourah University student charter",
                "يبيّن القيم والسمات المؤسسية المتوقّعة من الطالب/ـة والخريجة، ضمن ملاحق الدليل الإرشادي.",
                "It sets out the institutional values and attributes expected of a student and a graduate, within the appendices of the students guide.",
                DocStudentGuide, true));

            // ---------- المقيدون: الأدلة الرسمية ----------
            seed.Add(Resource(PgPhases.Enrolled, PgSectionKeys.Guides, 1, "document.png",
                "الدليل الإرشادي لطلبة الدراسات العليا", "The postgraduate students guide",
                "مرجعٌ تعريفيٌّ عامٌّ بنظام الدراسة والإجراءات والخدمات.",
                "A general reference to the study system, the procedures and the services.",
                DocStudentGuide, true));

            seed.Add(Resource(PgPhases.Enrolled, PgSectionKeys.Guides, 2, "document.png",
                "الدليل الإجرائي لمسار الإجراءات الأكاديمية", "The procedural guide to the academic procedures",
                "تفصيلٌ لمسار كل إجراءٍ أكاديميٍّ خطوةً بخطوة.",
                "A step-by-step account of the path of every academic procedure.",
                DocProceduresGuide, true));

            seed.Add(Resource(PgPhases.Enrolled, PgSectionKeys.Guides, 3, "document.png",
                "دليل إعداد خطة البحث والرسالة العلمية", "Guide to preparing the research plan and the thesis",
                "إرشاداتٌ لإعداد الخطة البحثية وكتابة الرسالة العلمية.",
                "Guidance on preparing the research plan and writing the thesis.",
                DocResearchPlanGuide, true));

            // ---------- الخريجون: متطلّبات التخرّج ----------
            seed.Add(Resource(PgPhases.Graduates, PgSectionKeys.GradRequirements, 1, "step10.png",
                "تسليم الرسالة العلمية إلكترونياً", "Submitting the thesis electronically",
                "تسليم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ليتولّيا رفعها إلى عمادة الدراسات العليا بصورتها المعتمدة.",
                "Submit the final electronic version of the thesis to the department and the college so they may forward the approved version to the Deanship of Graduate Studies.",
                "", false));

            seed.Add(Resource(PgPhases.Graduates, PgSectionKeys.GradRequirements, 2, "step20.png",
                "تسليم نسختين ورقيتين للعمادة", "Delivering two printed copies to the deanship",
                "تسليم نسختين ورقيتين من الرسالة العلمية إلى عمادة الدراسات العليا.",
                "Deliver two printed copies of the thesis to the Deanship of Graduate Studies.",
                "", false));

            seed.Add(Resource(PgPhases.Graduates, PgSectionKeys.GradRequirements, 3, "step0006.png",
                "إخلاء الطرف", "Clearance",
                "استكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة: مكتبة الجامعة، وسكن الطالبات، ووحدة الشؤون المالية بالعمادة، ووكالة الشؤون الأكاديمية للدراسات العليا؛ تمهيداً لاستلام وثيقة التخرّج من إدارة القبول والتسجيل.",
                "Complete the clearance form with the approval of the relevant entities: the university library, student housing, the financial affairs unit at the deanship and the vice-deanship for postgraduate academic affairs, in preparation for receiving the graduation document from the Admission and Registration Department.",
                "", false));

            return seed;
        }

        // 6) البطاقات النصية (Rich content blocks)
        private static readonly string[] BlockKeys = new string[]
        {
            PgSectionKeys.EnglishCourse,
            PgSectionKeys.GraduateIdentity,
            PgSectionKeys.FinancialRules,
            PgSectionKeys.PublishIncentive,
            PgSectionKeys.Footer
        };

        private static void BuildContentBlocks(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.ContentBlocks] = new PgListDef
            {
                Name = PgListNames.ContentBlocks,
                TitleAr = "البطاقات النصية", TitleEn = "Rich content blocks",
                Description = "Free-form cards: the English course, the graduate identity, the financial rules, the publication incentive and the reference footer.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(),
                    new PgFieldDef("SubTitle", "العنوان الفرعي (عربي)", "Subtitle (AR)", SPFieldType.Text),
                    new PgFieldDef("SubTitle_EN", "العنوان الفرعي (إنجليزي)", "Subtitle (EN)", SPFieldType.Text),
                    new PgFieldDef("Badge", "الشارة (عربي)", "Badge (AR)", SPFieldType.Text, true),
                    new PgFieldDef("Badge_EN", "الشارة (إنجليزي)", "Badge (EN)", SPFieldType.Text),
                    new PgFieldDef("StatValue", "القيمة البارزة", "Highlighted value", SPFieldType.Text),
                    Icon(),
                    new PgFieldDef("Paragraphs", "الفقرات (عربي)", "Paragraphs (AR)", SPFieldType.Note, false, 8, "فقرة في كل سطر."),
                    new PgFieldDef("Paragraphs_EN", "الفقرات (إنجليزي)", "Paragraphs (EN)", SPFieldType.Note, false, 8),
                    new PgFieldDef("Bullets", "النقاط (عربي)", "Bullets (AR)", SPFieldType.Note, false, 8, "نقطة في كل سطر."),
                    new PgFieldDef("Bullets_EN", "النقاط (إنجليزي)", "Bullets (EN)", SPFieldType.Note, false, 8),
                    new PgFieldDef("Highlights", "الملاحظات المميّزة (عربي)", "Highlights (AR)", SPFieldType.Note, false, 4, "النص|اسم ملف الأيقونة"),
                    new PgFieldDef("Highlights_EN", "الملاحظات المميّزة (إنجليزي)", "Highlights (EN)", SPFieldType.Note, false, 4),
                    new PgFieldDef("FooterTitle", "عنوان الخاتمة (عربي)", "Closing heading (AR)", SPFieldType.Text),
                    new PgFieldDef("FooterTitle_EN", "عنوان الخاتمة (إنجليزي)", "Closing heading (EN)", SPFieldType.Text),
                    new PgFieldDef("FooterParagraphs", "فقرات الخاتمة (عربي)", "Closing paragraphs (AR)", SPFieldType.Note, false, 4),
                    new PgFieldDef("FooterParagraphs_EN", "فقرات الخاتمة (إنجليزي)", "Closing paragraphs (EN)", SPFieldType.Note, false, 4),
                    new PgFieldDef("BlockKey", "نوع البطاقة", "Block key", SPFieldType.Choice, true, 0, null, BlockKeys),
                    YesNoField("Collapsible", "قابلة للطي", "Collapsible"),
                    Phase(), Order()
                },
                Seed = ContentBlocksSeed()
            };
        }

        private static List<Dictionary<string, string>> ContentBlocksSeed()
        {
            var seed = new List<Dictionary<string, string>>();

            // الدورة التأهيلية للغة الإنجليزية — المتقدمون
            seed.Add(Row(
                "Title", "الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا",
                "Title_EN", "The English language preparatory course for postgraduate students",
                "Paragraphs", "يقدّم معهد اللغة الإنجليزية بالجامعة دورةً تأهيليةً تُعِدّ الطلبة بالمهارات اللغوية الأكاديمية اللازمة للالتحاق ببرامج الدراسات العليا، وتُعادل الدرجة المطلوبة في اختبار اللغة الإنجليزية المعتمد للقبول.",
                "Paragraphs_EN", "The university's English Language Institute offers a preparatory course that equips students with the academic language skills needed to join postgraduate programs, and that is treated as equivalent to the score required in the approved English language test for admission.",
                "Bullets", "تركّز على المهارات الأكاديمية الأربع: الكتابة والقراءة والاستماع والمحادثة.\nتتضمّن مسارين تدريبيين بحسب المستوى، تجمع بين الحضور المباشر والتعلّم الذاتي.\nتُمنح شهادة اجتياز عند تحقيق نسبة 70% فأعلى في التقييمات.\nيستفيد المتدرّبون من مركزي التحدّث والكتابة بالمعهد.",
                "Bullets_EN", "It focuses on the four academic skills: writing, reading, listening and speaking.\nIt offers two training tracks according to level, combining in-person attendance with self-directed learning.\nA certificate of completion is awarded on achieving 70% or above in the assessments.\nParticipants benefit from the institute's speaking and writing centres.",
                "BlockKey", PgSectionKeys.EnglishCourse,
                "Collapsible", "No",
                "Phase", PgPhases.Applicants, "ItemOrder", "1"));

            // وثيقة هوية الخريجة — المقيدون
            seed.Add(Row(
                "Title", "وثيقة هوية خريجة جامعة الأميرة نورة",
                "Title_EN", "The Princess Nourah University graduate identity document",
                "Badge", "٩ خصائص", "Badge_EN", "9 attributes",
                "Paragraphs", "الخصائص التسع التي تتطلّع الجامعة إلى تمكين خرّيجيها منها، وفق وثيقة هوية الخريجة الصادرة عن وكالة الجامعة للشؤون التعليمية.",
                "Paragraphs_EN", "The nine attributes the university seeks to develop in its graduates, according to the graduate identity document issued by the Vice-Presidency for Educational Affairs.",
                "Bullets", "التحليل والإبداع: توظيف المعرفة في البحث والاستقصاء، التفكير الإبداعي والتحليل الناقد.\nالخدمة المجتمعية: المسؤولية المجتمعية، التعاون، الإيثار.\nريادة الأعمال: إدارة الأولويات والأهداف، التخطيط، البحث والابتكار، الوعي المالي والإنتاجية.\nالقيم: الاعتزاز بالقيم والهوية الوطنية، الإيجابية، احترام الثقافات الأخرى، الانتماء الوظيفي، الإتقان، التسامح والوسطية.\nالوعي والمعرفة: اتساع وعمق المعرفة بالنظريات والأسس المهنية، تطبيق المعرفة والانضباط، التجديد.\nتطوير الذات: التعلم المستمر في ضوء المتغيرات، المرونة والتكيف مع المتغيرات، الصحة واللياقة البدنية.\nالكفاءة المعلوماتية: الاستخدام الفعال للتقنية الحديثة والتكنولوجيا، استخراج البيانات وتقييمها.\nالقيادة: القيادة والتأثير، تحمل المسؤولية، حل النزاعات، صنع القرار، المبادرة، إدارة الوقت.\nالتواصل: التواصل الفعّال باللغتين العربية والإنجليزية في السياق الاجتماعي والمهني، مهارات الذكاء العاطفي، العمل الجماعي.",
                "Bullets_EN", "Analysis and creativity: applying knowledge in research and inquiry, creative thinking and critical analysis.\nCommunity service: social responsibility, cooperation, altruism.\nEntrepreneurship: managing priorities and goals, planning, research and innovation, financial awareness and productivity.\nValues: pride in national values and identity, positivity, respect for other cultures, professional belonging, mastery, tolerance and moderation.\nAwareness and knowledge: breadth and depth of knowledge of professional theories and foundations, applying knowledge with discipline, renewal.\nSelf-development: continuous learning in the light of change, flexibility and adaptability, health and physical fitness.\nInformation competence: effective use of modern technology, extracting and evaluating data.\nLeadership: leadership and influence, taking responsibility, conflict resolution, decision making, initiative, time management.\nCommunication: effective communication in Arabic and English in social and professional settings, emotional intelligence skills, teamwork.",
                "IconUrl", IconRoot + "prize01.png",
                "BlockKey", PgSectionKeys.GraduateIdentity,
                "Collapsible", "Yes",
                "Phase", PgPhases.Enrolled, "ItemOrder", "1"));

            // التنظيمات المالية للإجراءات الأكاديمية — المقيدون
            seed.Add(Row(
                "Title", "التنظيمات المالية للإجراءات الأكاديمية",
                "Title_EN", "Financial rules for the academic procedures",
                "Badge", "٧ حالات", "Badge_EN", "7 cases",
                "Paragraphs", "تبيّن التنظيمات المالية المعتمدة بالعمادة آثار الإجراءات الأكاديمية على الرسوم الدراسية، وفق ما يلي:",
                "Paragraphs_EN", "The financial rules approved by the deanship set out the effect of the academic procedures on tuition fees, as follows:",
                "Bullets", "١/ الانسحاب من البرنامج أو الاعتذار عن فصل دراسي: تُسترد الرسوم وفق توقيت الطلب من بدء الدراسة: 75% خلال الأسبوع الأول، و50% خلال الثاني، و25% خلال الثالث، ولا يُسترد شيءٌ بعد الأسبوع الثالث. ويُعتمد تاريخ الانسحاب بناءً على تاريخ إرسال النموذج المعتمد.\n٢/ حذف مقرر أو عددٍ من مقررات الفصل: لا تُسترد أي رسومٍ عند حذف بعض مقررات الفصل، ولا يُطالَب الطالب/ـة برسومها عند إعادة تسجيلها لاحقاً؛ أمّا الانسحاب من البرنامج لاحقاً فلا يترتّب عليه استرداد.\n٣/ تأجيل القبول: يفقد الطالب/ـة حقّ الاسترداد عند الانسحاب بعد تأجيل القبول. وفي حال إلغاء البرنامج أو تأجيل طرحه، يحقّ استرداد كامل الرسوم أو تمديد التأجيل لحين إعادة الطرح.\n٤/ تأجيل الدراسة: لا تُسدَّد رسومٌ عن الفصل المؤجَّل، ويُقدَّم الطلب قبل بداية الفصل بأسبوعين على الأقل، وينطبق على من اجتاز فصلاً أو أكثر أو أنجز قدراً مناسباً من الرسالة.\n٥/ إعادة دراسة مقرر أو دراسة مقررات إضافية: تُحتسب رسومٌ إضافيةٌ على كل مقرر عند عدم اجتيازه أو انخفاض المعدل التراكمي عن «جيد جداً» في فصلين متتاليين ولزوم إعادته أو دراسة مقررات إضافية.\n٦/ تسجيل فصلٍ إضافيٍّ للرسالة: تُحتسب رسومٌ عن كل فصلٍ إضافيٍّ يتجاوز عاماً دراسياً واحداً بعد المدة المحددة للبرنامج.\n٧/ الحصول على شهادةٍ عند نقطة خروج: عند الخروج بدرجةٍ أدنى — إن أتاحت خطة البرنامج ذلك — يُستكمل سداد الرسوم المستحقة بحسب الوحدات المطلوبة للدرجة الأدنى، ولا تُسترد رسوم الفصول السابقة للبرنامج الأساسي.",
                "Bullets_EN", "1/ Withdrawing from the program or apologising for a term: fees are refunded according to when the request is made from the start of study: 75% during the first week, 50% during the second and 25% during the third; nothing is refunded after the third week. The withdrawal date is taken from the date the approved form was sent.\n2/ Dropping one or more courses in a term: no fees are refunded when some courses in the term are dropped, and the student is not charged again when she re-registers them later; a later withdrawal from the program carries no refund.\n3/ Deferring admission: the student loses the right to a refund if she withdraws after deferring admission. If the program is cancelled or its offering postponed, she is entitled to a full refund or to extend the deferral until it is offered again.\n4/ Deferring study: no fees are paid for the deferred term. The request is submitted at least two weeks before the term begins, and it applies to a student who has passed one term or more or completed a suitable portion of the thesis.\n5/ Repeating a course or taking additional courses: an additional fee is charged for each course when it is not passed, or when the grade point average falls below \"very good\" for two consecutive terms and the course must be repeated or additional courses taken.\n6/ Registering an additional term for the thesis: a fee is charged for every additional term beyond one academic year after the period set for the program.\n7/ Obtaining a certificate at an exit point: on exiting with a lower degree, where the program plan allows it, the fees due are settled according to the units required for the lower degree, and the fees of previous terms of the main program are not refunded.",
                "FooterParagraphs", "ملاحظة: تخضع جميع الإجراءات للّائحة المنظمة للدراسات العليا وقواعدها التنفيذية بالجامعة، وللتقويم الجامعي المعلن، وتخضع البرامج القائمة على شراكاتٍ خارجيةٍ لضوابطَ خاصة.",
                "FooterParagraphs_EN", "Note: all procedures are subject to the regulation governing postgraduate studies and its executive rules at the university and to the published academic calendar; programs based on external partnerships are subject to their own rules.",
                "IconUrl", IconRoot + "percent.png",
                "BlockKey", PgSectionKeys.FinancialRules,
                "Collapsible", "Yes",
                "Phase", PgPhases.Enrolled, "ItemOrder", "2"));

            // حافز النشر العلمي — المقيدون
            seed.Add(Row(
                "Title", "حافزٌ بحثيٌّ لطلبة الدراسات العليا",
                "Title_EN", "A research incentive for postgraduate students",
                "SubTitle", "النشر العلمي… طريقك إلى خصم 10% على الرسوم الدراسية",
                "SubTitle_EN", "Research publication: your route to a 10% tuition discount",
                "StatValue", "10%",
                "IconUrl", IconRoot + "percent.png",
                "Paragraphs", "إشارةً إلى قرار مجلس جامعة الأميرة نورة بنت عبد الرحمن رقم (١٠-٦-١٤٤٧هـ) المتضمّن الموافقة على إعفاء وتخفيض الرسوم الدراسية لبعض فئات طلبة الدراسات العليا، ومن ضمنهم الطلبة الحاصلون على النشر العلمي أثناء فترة الدراسة وفق ضوابط وآلية النشر العلمي بالجامعة؛ تقرّر تطبيق نسبة خصمٍ مقدارها 10% على المقبولين ابتداءً من العام الجامعي ١٤٤٨هـ، دعماً لمسيرتهم البحثية وتحفيزاً لهم على الاستمرار في تحقيق منجزاتٍ علميةٍ متميزة.",
                "Paragraphs_EN", "With reference to Princess Nourah bint Abdulrahman University Council resolution no. (10-6-1447 AH) approving the exemption from and reduction of tuition fees for certain categories of postgraduate students, among them students who publish research during their period of study in line with the university's research publication rules and mechanism, a discount of 10% has been approved for students admitted from the academic year 1448 AH onwards, in support of their research path and to encourage them to continue producing distinguished scholarly work.",
                "Highlights", "للاطّلاع على ضوابط النشر المعتمدة وآليته، يُنصح بالتواصل المبكر مع المشرف/ـة الأكاديمي/ـة لاختيار الوعاء العلمي المناسب وتوقيت النشر.|member-icon.png\nليكن البحث العلمي بدايةَ إنجازٍ يواكب طموح الجامعة ورؤية المملكة 2030.|star-icon.png",
                "Highlights_EN", "To review the approved publication rules and mechanism, contact your academic supervisor early to choose the right publication venue and timing.|member-icon.png\nLet research be the beginning of an achievement that matches the university's ambition and Saudi Vision 2030.|star-icon.png",
                "FooterTitle", "المشاركة في المؤتمرات العلمية",
                "FooterTitle_EN", "Taking part in academic conferences",
                "FooterParagraphs", "إلى جانب النشر، تمثّل المشاركة في المؤتمرات والندوات العلمية رافداً مهماً لإثراء التجربة البحثية وبناء الحضور الأكاديمي وتوسيع شبكة العلاقات العلمية. وهي مسارٌ تطويريٌّ مستقلٌّ لا يترتّب عليه خصم الرسوم، ويُوصى بمناقشة فرص المشاركة مع المشرف/ـة الأكاديمي/ـة.",
                "FooterParagraphs_EN", "Alongside publication, taking part in academic conferences and seminars is an important way to enrich the research experience, build an academic presence and widen the network of scholarly contacts. It is an independent development path that carries no fee discount, and opportunities to take part are best discussed with your academic supervisor.",
                "BlockKey", PgSectionKeys.PublishIncentive,
                "Collapsible", "No",
                "Phase", PgPhases.Enrolled, "ItemOrder", "1"));

            // الأساس النظامي والمرجعي — الفئات الثلاث
            const string footerAr = "اللائحة المنظمة للدراسات العليا في الجامعات (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤)\nالدليل الإرشادي والنماذج المعتمدة بموقع العمادة";
            const string footerEn = "The regulation governing postgraduate studies in universities (Council of University Affairs resolution 2/9/1444)\nThe students guide and the approved forms on the deanship website";

            // Stations only: the landing page carries no regulatory footnote.
            foreach (string phase in PgPhases.Stations)
            {
                seed.Add(Row(
                    "Title", "الأساس النظامي والمرجعي", "Title_EN", "Regulatory basis and references",
                    "Paragraphs", footerAr, "Paragraphs_EN", footerEn,
                    "BlockKey", PgSectionKeys.Footer,
                    "Collapsible", "No",
                    "Phase", phase, "ItemOrder", "1"));
            }

            return seed;
        }

        // 7) جدول الإعفاء والتخفيض (Fee exemption table)
        private static void BuildDiscounts(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Discounts] = new PgListDef
            {
                Name = PgListNames.Discounts,
                TitleAr = "جدول الإعفاء والتخفيض", TitleEn = "Exemption and reduction table",
                Description = "Rows of the tuition exemption and reduction table.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(),
                    new PgFieldDef("DiscountValue", "نسبة الخصم (عربي)", "Discount value (AR)", SPFieldType.Text, true),
                    new PgFieldDef("DiscountValue_EN", "نسبة الخصم (إنجليزي)", "Discount value (EN)", SPFieldType.Text),
                    Phase(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title", "أبناء شهداء الواجب أو المصابين في أداء الواجب الوطني",
                        "Title_EN", "Children of martyrs of duty or of those injured in the performance of national duty",
                        "DiscountValue", "إعفاء كامل", "DiscountValue_EN", "Full exemption", "ItemOrder", "1"),

                    Row("Title", "ذوي الإعاقة (حسب طبيعة البرنامج)",
                        "Title_EN", "Persons with disabilities (according to the nature of the program)",
                        "DiscountValue", "٪٢٥", "DiscountValue_EN", "25%", "ItemOrder", "2"),

                    Row("Title", "السداد المبكر لإجمالي رسوم البرنامج - طلبة الدراسات العليا",
                        "Title_EN", "Early payment of the total program fees - postgraduate students",
                        "DiscountValue", "٪١٠", "DiscountValue_EN", "10%", "ItemOrder", "3"),

                    Row("Title", "الأخوة طلبة الدراسات العليا",
                        "Title_EN", "Siblings enrolled as postgraduate students",
                        "DiscountValue", "٪١٠", "DiscountValue_EN", "10%", "ItemOrder", "4"),

                    Row("Title", "جهات الإيفاد - الجهات الحكومية والمؤسسات التعليمية والجهات الخاصة",
                        "Title_EN", "Sponsoring bodies - government entities, educational institutions and private organisations",
                        "DiscountValue", "٪١٠ إذا كان عدد الموفدين أقل من خمسة، و١٥٪ إذا كان عدد الموفدين خمسة فأكثر",
                        "DiscountValue_EN", "10% for fewer than five sponsored students, and 15% for five or more",
                        "ItemOrder", "5"),

                    Row("Title", "المستفيدون من الضمان الاجتماعي والجمعيات الخيرية والمتبرعون بالأعضاء",
                        "Title_EN", "Beneficiaries of social security and charitable societies, and organ donors",
                        "DiscountValue", "٪٢٠", "DiscountValue_EN", "20%", "ItemOrder", "6"),

                    Row("Title", "النشر العلمي أثناء الدراسة (وفق ضوابط وآلية النشر العلمي بجامعة الأميرة نورة بنت عبدالرحمن)",
                        "Title_EN", "Research publication during study (under the research publication rules and mechanism of Princess Nourah bint Abdulrahman University)",
                        "DiscountValue", "٪١٠", "DiscountValue_EN", "10%", "ItemOrder", "7")
                }
            };
        }

        // 8) ملاحظات الإعفاء والتخفيض (Exemption notes)
        private static void BuildDiscountNotes(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.DiscountNotes] = new PgListDef
            {
                Name = PgListNames.DiscountNotes,
                TitleAr = "ملاحظات الإعفاء والتخفيض", TitleEn = "Exemption and reduction notes",
                Description = "The conditions printed under the tuition exemption and reduction table.",
                Fields = new List<PgFieldDef> { TitleEn(), Phase(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title", "يطبق التخفيض الأعلى في حال استحقاق المستفيد لأكثر من تخفيض",
                        "Title_EN", "The highest reduction applies when a beneficiary qualifies for more than one",
                        "ItemOrder", "1"),
                    Row("Title", "لا يطبق الخصم على الحاصلين على منح التميز",
                        "Title_EN", "The discount does not apply to holders of excellence scholarships",
                        "ItemOrder", "2"),
                    Row("Title", "لا يطبق التخفيض على رسوم المقررات التكميلية",
                        "Title_EN", "The reduction does not apply to the fees of supplementary courses",
                        "ItemOrder", "3"),
                    Row("Title", "يطبق الإعفاء والتخفيض على البرامج المحلية فقط",
                        "Title_EN", "The exemption and the reduction apply to local programs only",
                        "ItemOrder", "4"),
                    Row("Title", "لا يطبق التخفيض على المستفيدين من برنامج تقسيط التعليم في حال السداد المبكر لإجمالي الرسوم الدراسية (المقابل المالي)",
                        "Title_EN", "The reduction does not apply to beneficiaries of the tuition instalment program in the case of early payment of the total tuition fees",
                        "ItemOrder", "5"),
                    Row("Title", "لا يطبق الإعفاء والتخفيض على بعض البرامج القائمة بالشراكات",
                        "Title_EN", "The exemption and the reduction do not apply to certain partnership-based programs",
                        "ItemOrder", "6"),
                    Row("Title", "يجب تقديم المستندات التي تثبت الحالة الصحية أو بطاقة الإعاقة، لجميع الفئات",
                        "Title_EN", "Documents proving the medical condition, or the disability card, must be submitted for all categories",
                        "ItemOrder", "7"),
                    Row("Title", "يجب تقديم شهادة أو خطاب من المركز السعودي لزراعة الأعضاء أو من المستشفى يفيد بأنه تم التبرع",
                        "Title_EN", "A certificate or letter from the Saudi Center for Organ Transplantation or from the hospital confirming the donation must be submitted",
                        "ItemOrder", "8"),
                    Row("Title", "حسب ضريبة القيمة المضافة لغير السعوديين على الرسوم الدراسية بعد الخصم",
                        "Title_EN", "Value added tax for non-Saudis is calculated on the tuition fees after the discount",
                        "ItemOrder", "9")
                }
            };
        }

        // 9) الخدمات والمراكز (Services and centres)
        private static void BuildServices(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Services] = new PgListDef
            {
                Name = PgListNames.Services,
                TitleAr = "الخدمات والمراكز", TitleEn = "Services and centres",
                Description = "Grouped links to university services and centres.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), Link(), Icon(),
                    new PgFieldDef("GroupName", "المجموعة (عربي)", "Group (AR)", SPFieldType.Text, true),
                    new PgFieldDef("GroupName_EN", "المجموعة (إنجليزي)", "Group (EN)", SPFieldType.Text),
                    Phase(), Order()
                },
                Seed = ServicesSeed()
            };
        }

        private static Dictionary<string, string> Service(string groupAr, string groupEn, int order,
                                                          string titleAr, string titleEn, string url)
        {
            return Row("Title", titleAr, "Title_EN", titleEn,
                       "LinkUrl", url,
                       "IconUrl", IconRoot + "arrow-right.png",
                       "GroupName", groupAr, "GroupName_EN", groupEn,
                       "ItemOrder", N(order));
        }

        private static List<Dictionary<string, string>> ServicesSeed()
        {
            const string research = "الخدمات البحثية";
            const string researchEn = "Research services";
            const string career = "الخدمات التأهيلية لسوق العمل";
            const string careerEn = "Employability services";
            const string student = "الخدمات الطلابية";
            const string studentEn = "Student services";

            return new List<Dictionary<string, string>>
            {
                Service(research, researchEn, 1, "برنامج الباحثات الناشئات", "Junior Researchers Program",
                        "https://pnu.edu.sa/ar/SR/Pages/Junior-Researchers-Program.aspx"),
                Service(research, researchEn, 2, "برنامج نافع", "Nafie Program",
                        "https://pnu.edu.sa/ar/SR/Pages/Nafie-Program.aspx"),
                Service(research, researchEn, 3, "برنامج المجموعات البحثية", "Research Groups Program",
                        "https://pnu.edu.sa/ar/SR/Pages/Research-Groups-Program.aspx"),
                Service(research, researchEn, 4, "أخلاقيات البحث العلمي", "Research ethics",
                        "https://pnu.edu.sa/ar/SR/Pages/SREthics_.aspx"),
                Service(research, researchEn, 5, "المكتبة المركزية", "Central Library",
                        "https://pnu.edu.sa/ar/Deanship/Research/Pages/LibraryServices.aspx"),
                Service(research, researchEn, 6, "مركز الخدمات البحثية", "Research Services Center",
                        "https://pnu.edu.sa/ar/Centers/ResearchServicesCenter/Pages/home.aspx"),

                Service(career, careerEn, 7, "إضافة السير الذاتية", "Submitting your CV",
                        "https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=223"),
                Service(career, careerEn, 8, "مركز الدعم الطلابي والمهني", "Student and Career Support Center",
                        "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/home.aspx"),
                Service(career, careerEn, 9, "إدارة الدعم المهاري", "Skills Support Department",
                        "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/55.aspx"),
                Service(career, careerEn, 10, "الأنشطة اللامنهجية", "Extracurricular activities",
                        "https://graduates.pnu.edu.sa/ar/Pages/NonApproachActivities.aspx"),
                Service(career, careerEn, 11, "برنامج سموق", "Sumouq Program",
                        "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/33.aspx"),
                Service(career, careerEn, 12, "بوابة الخريجات", "Graduates portal",
                        "https://graduates.pnu.edu.sa/ar/pages/default.aspx#about"),

                Service(student, studentEn, 13, "وكالة الخدمات الطلابية", "Vice-Deanship for Student Services",
                        "https://pnu.edu.sa/ar/Deanship/studaffairs/AgencyStudentServices/Pages/home.aspx"),
                Service(student, studentEn, 14, "وكالة الحياة الطلابية", "Vice-Deanship for Student Life",
                        "https://pnu.edu.sa/ar/Deanship/studaffairs/Student%20Life%20Agency/Pages/home.aspx"),
                Service(student, studentEn, 15, "حماية حقوق الطالبات", "Protection of student rights",
                        "https://pnu.edu.sa/ar/Faculties/Ap/Pages/Student-Rights-Protection.aspx"),
                Service(student, studentEn, 16, "وحدة مساندة الطالبات ذوي الإعاقة", "Support Unit for Students with Disabilities",
                        "https://pnu.edu.sa/ar/Deanship/studaffairs/AgencyStudentServices/Pages/StudentswithDisabilitiesUnit.aspx"),
                Service(student, studentEn, 17, "دليل تمكين", "Tamkeen guide",
                        "https://drive.google.com/file/d/1C3BgZxt_eMMyCqXJ5G46-h5JhQp08NIJ/view?usp=drive_link"),
                Service(student, studentEn, 18, "المراكز الترفيهية", "Recreation centres",
                        "https://pnu.edu.sa/ar/Deanship/studaffairs/Student%20Life%20Agency/Pages/RecreationCenter.aspx")
            };
        }

        // 10) روابط السكن والتواصل (Housing and contact links)
        private static void BuildQuickLinks(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.QuickLinks] = new PgListDef
            {
                Name = PgListNames.QuickLinks,
                TitleAr = "روابط السكن والتواصل", TitleEn = "Housing and contact links",
                Description = "Single link cards for the student housing and the contact sections.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), Link(), Icon(),
                    new PgFieldDef("SectionKey", "القسم", "Section", SPFieldType.Choice, true, 0,
                                   "Housing / Contact",
                                   new string[] { PgSectionKeys.Housing, PgSectionKeys.Contact }),
                    Phase(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title", "التسجيل في سكن الطالبات", "Title_EN", "Registering for student housing",
                        "LinkUrl", UrlHousingService,
                        "IconUrl", IconRoot + "arrow-right.png",
                        "SectionKey", PgSectionKeys.Housing, "ItemOrder", "1"),

                    Row("Title", DeanshipEmail, "Title_EN", DeanshipEmail,
                        "LinkUrl", "mailto:" + DeanshipEmail,
                        "IconUrl", IconRoot + "arrow-right.png",
                        "SectionKey", PgSectionKeys.Contact, "ItemOrder", "1")
                }
            };
        }
    }
}
