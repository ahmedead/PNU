using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    public static partial class DgaPgListSchema
    {
        private static void RegisterCoreLists()
        {
            // 1. DgaPgSections
            var sections = new DgaPgListDef(DgaPgListNames.Sections, "محاور الحقيبة الأكاديمية (DGA)", "تخزين المحاور والعناوين الرئيسية للحقيبة");
            sections.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            sections.AddField("SectionKey", "Section Key", SPFieldType.Choice, true, DgaPgSectionKeys.All);
            sections.AddField("Phase", "Phase / Audience", SPFieldType.Choice, false, DgaPgPhases.All);
            sections.AddField("LeadText", "Lead Text (AR)", SPFieldType.Note, false);
            sections.AddField("LeadText_EN", "Lead Text (EN)", SPFieldType.Note, false);
            sections.AddField("BadgeText", "Badge Text (AR)", SPFieldType.Text, false);
            sections.AddField("BadgeText_EN", "Badge Text (EN)", SPFieldType.Text, false);
            sections.AddField("NoteText", "Footnote (AR)", SPFieldType.Note, false);
            sections.AddField("NoteText_EN", "Footnote (EN)", SPFieldType.Note, false);
            sections.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            sections.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            sections.AddField("IsVisible", "Is Visible", SPFieldType.Boolean, false, null, "1");
            sections.SeedDataFactory = GetSectionsSeed;
            Register(sections);

            // 2. DgaPgHeader
            var header = new DgaPgListDef(DgaPgListNames.Header, "ترويسة الصفحات (DGA)", "عناوين الترويسة ومسار التنقل");
            header.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            header.AddField("Phase", "Phase", SPFieldType.Choice, true, DgaPgPhases.All);
            header.AddField("Description", "Description (AR)", SPFieldType.Note, false);
            header.AddField("Description_EN", "Description (EN)", SPFieldType.Note, false);
            header.AddField("BreadcrumbText", "Breadcrumbs (AR)", SPFieldType.Note, false);
            header.AddField("BreadcrumbText_EN", "Breadcrumbs (EN)", SPFieldType.Note, false);
            header.SeedDataFactory = GetHeaderSeed;
            Register(header);

            // 3. DgaPgIntro
            var intro = new DgaPgListDef(DgaPgListNames.Intro, "النبذة التعريفية (DGA)", "بطاقات النبذة التعريفية للحقيبة");
            intro.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            intro.AddField("Phase", "Phase", SPFieldType.Choice, true, DgaPgPhases.All);
            intro.AddField("SubTitle", "SubTitle (AR)", SPFieldType.Text, false);
            intro.AddField("SubTitle_EN", "SubTitle (EN)", SPFieldType.Text, false);
            intro.AddField("LeadText", "Lead Text (AR)", SPFieldType.Note, false);
            intro.AddField("LeadText_EN", "Lead Text (EN)", SPFieldType.Note, false);
            intro.AddField("Paragraphs", "Paragraphs (AR)", SPFieldType.Note, false);
            intro.AddField("Paragraphs_EN", "Paragraphs (EN)", SPFieldType.Note, false);
            intro.AddField("Badges", "Badges (AR)", SPFieldType.Note, false);
            intro.AddField("Badges_EN", "Badges (EN)", SPFieldType.Note, false);
            intro.AddField("ReturnText", "Return Button Text (AR)", SPFieldType.Text, false);
            intro.AddField("ReturnText_EN", "Return Button Text (EN)", SPFieldType.Text, false);
            intro.AddField("ReturnUrl", "Return Button URL", SPFieldType.Text, false);
            intro.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            intro.SeedDataFactory = GetIntroSeed;
            Register(intro);

            // 4. DgaPgJourney
            var journey = new DgaPgListDef(DgaPgListNames.Journey, "خريطة الرحلة (DGA)", "مراحل وخطوات الرحلة الأكاديمية");
            journey.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            journey.AddField("Phase", "Phase", SPFieldType.Choice, true, DgaPgPhases.Stations);
            journey.AddField("StepNumber", "Step Number", SPFieldType.Number, true);
            journey.AddField("Description", "Description (AR)", SPFieldType.Note, false);
            journey.AddField("Description_EN", "Description (EN)", SPFieldType.Note, false);
            journey.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            journey.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            journey.SeedDataFactory = GetJourneySeed;
            Register(journey);
        }

        private static List<Dictionary<string, object>> GetSectionsSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Applicants sections - headings and leads taken from the v4 design
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الأول: خريطة رحلة القبول" }, { "Title_EN", "First Axis: Admission Journey Map" },
                { "SectionKey", DgaPgSectionKeys.Journey }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "" }, { "LeadText_EN", "" },
                { "ItemOrder", 1 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثاني: البرامج وشروط القبول والرسوم" }, { "Title_EN", "Second Axis: Programs, Admission Requirements and Fees" },
                { "SectionKey", DgaPgSectionKeys.Programs }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "شروط القبول والرسوم تختلف بحسب البرنامج وتُحدَّث كل فصل؛ ولأن الرسوم مدرجةٌ في صفحة كل برنامج، ندلّكِ على المصادر الرسمية المحدّثة مباشرةً:" }, { "LeadText_EN", "" },
                { "ItemOrder", 2 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثالث: الدورة التأهيلية للغة الإنجليزية" }, { "Title_EN", "Third Axis: English Language Qualifying Course" },
                { "SectionKey", DgaPgSectionKeys.EnglishCourse }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "" }, { "LeadText_EN", "" },
                { "ItemOrder", 3 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الرابع: الأسئلة الأكثر شيوعاً" }, { "Title_EN", "Fourth Axis: Frequently Asked Questions" },
                { "SectionKey", DgaPgSectionKeys.Faq }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "إجاباتٌ موجزةٌ عن أبرز الاستفسارات المتكرّرة لدى طلبة الدراسات العليا." }, { "LeadText_EN", "" },
                { "ItemOrder", 4 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الخامس: الحقوق والواجبات / اللوائح النظامية" }, { "Title_EN", "Fifth Axis: Rights and Duties / Statutory Regulations" },
                { "SectionKey", DgaPgSectionKeys.Regulations }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "حزمة اللوائح الرسمية المعتمدة بالجامعة التي تبيّن حقوق طالبة الدراسات العليا وواجباتها — بالضغط على اللائحة تُفتح مباشرةً." }, { "LeadText_EN", "" },
                { "ItemOrder", 5 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور السادس: إعفاء وتخفيض الرسوم الدراسية لفئات محددة من طلبة الدراسات العليا" }, { "Title_EN", "Sixth Axis: Tuition Exemptions and Reductions for Specific Categories" },
                { "SectionKey", DgaPgSectionKeys.Discounts }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "الفئات التي يشملها الإعفاء والتخفيض:" }, { "LeadText_EN", "" },
                { "ItemOrder", 6 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور السابع: الخدمات / المراكز" }, { "Title_EN", "Seventh Axis: Services and Centers" },
                { "SectionKey", DgaPgSectionKeys.Services }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "جمعنا لكِ أبرز ما تحتاجينه من خدماتٍ ومراكزَ في الجامعة، موصولةً مباشرةً بصفحاتها الرسمية؛ لتصلي إليها بيسرٍ ووضوح." }, { "LeadText_EN", "" },
                { "ItemOrder", 7 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثامن: السكن الجامعي" }, { "Title_EN", "Eighth Axis: University Housing" },
                { "SectionKey", DgaPgSectionKeys.Housing }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "يوفّر السكن الجامعي بيئةً آمنةً ومهيّأةً لطالبات الدراسات العليا، وتُدار خدمة التسجيل فيه إلكترونياً عبر عمادة شؤون الطالبات. يمكنكِ الاطّلاع على الشروط والمستندات والرسوم والتقديم من خلال صفحة الخدمة الرسمية." }, { "LeadText_EN", "" },
                { "ItemOrder", 8 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور التاسع: تواصلي معنا" }, { "Title_EN", "Ninth Axis: Contact Us" },
                { "SectionKey", DgaPgSectionKeys.Contact }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "استفساراتكِ تهمّنا. إن احتجتِ أي مساعدةٍ أو إيضاح، تواصلي مع وحدة خدمة المستفيدين بعمادة الدراسات العليا عبر البريد الإلكتروني، وسيسعدنا خدمتكِ." }, { "LeadText_EN", "" },
                { "ItemOrder", 9 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الأساس النظامي والمرجعي" }, { "Title_EN", "Statutory and Reference Basis" },
                { "SectionKey", DgaPgSectionKeys.Reference }, { "Phase", DgaPgPhases.Applicants },
                { "LeadText", "اللائحة المنظمة للدراسات العليا في الجامعات (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤) • الدليل الإرشادي والنماذج المعتمدة بموقع العمادة" }, { "LeadText_EN", "" },
                { "ItemOrder", 10 }, { "IsVisible", true }
            });

            // Enrolled sections - headings and leads taken from the v4 design
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الأول: خريطة الرحلة الأكاديمية" }, { "Title_EN", "First Axis: Academic Journey Map" },
                { "SectionKey", DgaPgSectionKeys.Journey }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "خط سير طالبة الدراسات العليا المقيَّدة في خمس مراحلَ متتابعة، من تسجيل الجدول الدراسي حتى منح الدرجة العلمية." }, { "LeadText_EN", "" },
                { "NoteText", "مستندةٌ إلى «خط سير طالبـ/ـة الدراسات العليا» في الدليل الإرشادي لطلبة الدراسات العليا الصادر عن العمادة." },
                { "NoteText_EN", "Based on the postgraduate student pathway in the Deanship's guide for postgraduate students." },
                { "ItemOrder", 1 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثاني: الحقوق والواجبات / اللوائح النظامية" }, { "Title_EN", "Second Axis: Rights and Duties / Statutory Regulations" },
                { "SectionKey", DgaPgSectionKeys.Regulations }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "حزمة اللوائح الرسمية المعتمدة بالجامعة التي تبيّن حقوق طالبة الدراسات العليا وواجباتها — بالضغط على اللائحة تُفتح مباشرةً." }, { "LeadText_EN", "" },
                { "ItemOrder", 2 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثالث: الإجراءات الأكاديمية" }, { "Title_EN", "Third Axis: Academic Procedures" },
                { "SectionKey", DgaPgSectionKeys.Procedures }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "خمسة عشر إجراءً مرتّبةً تصاعدياً وفق رقم المادة. بالضغط على الإجراء يتوسّع ليعرض شروطه ونصّ مادته والنماذج المرتبطة من موقع العمادة." }, { "LeadText_EN", "" },
                { "ItemOrder", 3 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الرابع: الأسئلة الأكثر شيوعاً" }, { "Title_EN", "Fourth Axis: Frequently Asked Questions" },
                { "SectionKey", DgaPgSectionKeys.Faq }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "إجاباتٌ موجزةٌ عن أبرز الاستفسارات المتكرّرة لدى طلبة الدراسات العليا." }, { "LeadText_EN", "" },
                { "NoteText", "يمكنكم التواصل مع وحدة خدمة المستفيدين بعمادة الدراسات العليا عبر البريد الإلكتروني|mailto:dgs-aa@pnu.edu.sa|dgs-aa@pnu.edu.sa" },
                { "NoteText_EN", "You may contact the Beneficiary Service Unit at the Deanship of Postgraduate Studies by email|mailto:dgs-aa@pnu.edu.sa|dgs-aa@pnu.edu.sa" },
                { "ItemOrder", 4 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الخامس: حافز النشر العلمي" }, { "Title_EN", "Fifth Axis: Research Publication Incentive" },
                { "SectionKey", DgaPgSectionKeys.PublishIncentive }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "مبادرةٌ تحفيزيةٌ لدعم المسيرة البحثية لطلبة الدراسات العليا." }, { "LeadText_EN", "" },
                { "ItemOrder", 5 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور السادس: إعفاء وتخفيض الرسوم الدراسية لفئات محددة من طلبة الدراسات العليا" }, { "Title_EN", "Sixth Axis: Tuition Exemptions and Reductions for Specific Categories" },
                { "SectionKey", DgaPgSectionKeys.Discounts }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "الفئات التي يشملها الإعفاء والتخفيض:" }, { "LeadText_EN", "" },
                { "ItemOrder", 6 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور السابع: الأدلة الإرشادية" }, { "Title_EN", "Seventh Axis: Guides" },
                { "SectionKey", DgaPgSectionKeys.Guides }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "أدلّةٌ رسميةٌ موصولةٌ مباشرةً بموقع العمادة — بالضغط على الدليل يُفتح مباشرةً." }, { "LeadText_EN", "" },
                { "ItemOrder", 7 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثامن: الخدمات / المراكز" }, { "Title_EN", "Eighth Axis: Services and Centers" },
                { "SectionKey", DgaPgSectionKeys.Services }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "جمعنا لكِ أبرز ما تحتاجينه من خدماتٍ ومراكزَ في الجامعة، موصولةً مباشرةً بصفحاتها الرسمية؛ لتصلي إليها بيسرٍ ووضوح." }, { "LeadText_EN", "" },
                { "ItemOrder", 8 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور التاسع: السكن الجامعي" }, { "Title_EN", "Ninth Axis: University Housing" },
                { "SectionKey", DgaPgSectionKeys.Housing }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "يوفّر السكن الجامعي بيئةً آمنةً ومهيّأةً لطالبات الدراسات العليا، وتُدار خدمة التسجيل فيه إلكترونياً عبر عمادة شؤون الطالبات. يمكنكِ الاطّلاع على الشروط والمستندات والرسوم والتقديم من خلال صفحة الخدمة الرسمية." }, { "LeadText_EN", "" },
                { "ItemOrder", 9 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور العاشر: تواصلي معنا" }, { "Title_EN", "Tenth Axis: Contact Us" },
                { "SectionKey", DgaPgSectionKeys.Contact }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "استفساراتكِ تهمّنا. إن احتجتِ أي مساعدةٍ أو إيضاح، تواصلي مع وحدة خدمة المستفيدين بعمادة الدراسات العليا عبر البريد الإلكتروني، وسيسعدنا خدمتكِ." }, { "LeadText_EN", "" },
                { "ItemOrder", 10 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الأساس النظامي والمرجعي" }, { "Title_EN", "Statutory and Reference Basis" },
                { "SectionKey", DgaPgSectionKeys.Reference }, { "Phase", DgaPgPhases.Enrolled },
                { "LeadText", "اللائحة المنظمة للدراسات العليا في الجامعات (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤) • الدليل الإرشادي والنماذج المعتمدة بموقع العمادة" }, { "LeadText_EN", "" },
                { "ItemOrder", 11 }, { "IsVisible", true }
            });

            // Graduates sections - headings and leads taken from the v4 design
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الأول: رحلة التخرّج" }, { "Title_EN", "First Axis: Graduation Journey" },
                { "SectionKey", DgaPgSectionKeys.Journey }, { "Phase", DgaPgPhases.Graduates },
                { "LeadText", "" }, { "LeadText_EN", "" },
                { "ItemOrder", 1 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثاني: متطلّبات التخرّج" }, { "Title_EN", "Second Axis: Graduation Requirements" },
                { "SectionKey", DgaPgSectionKeys.GradRequirements }, { "Phase", DgaPgPhases.Graduates },
                { "LeadText", "تستكمل الخرِّيجة المتطلّبات التالية لإتمام تخرّجها واستلام وثيقتها:" }, { "LeadText_EN", "" },
                { "ItemOrder", 2 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الثالث: إجراءات التخرّج" }, { "Title_EN", "Third Axis: Graduation Procedures" },
                { "SectionKey", DgaPgSectionKeys.Procedures }, { "Phase", DgaPgPhases.Graduates },
                { "LeadText", "الإجراءات الأكاديمية الخاصة بمرحلة التخرّج — بالضغط على كلٍّ منها تظهر شروطه ونماذجه ومساره:" }, { "LeadText_EN", "" },
                { "ItemOrder", 3 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المحور الرابع: الأسئلة الأكثر شيوعاً" }, { "Title_EN", "Fourth Axis: Frequently Asked Questions" },
                { "SectionKey", DgaPgSectionKeys.Faq }, { "Phase", DgaPgPhases.Graduates },
                { "LeadText", "" }, { "LeadText_EN", "" },
                { "ItemOrder", 4 }, { "IsVisible", true }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الأساس النظامي والمرجعي" }, { "Title_EN", "Statutory and Reference Basis" },
                { "SectionKey", DgaPgSectionKeys.Reference }, { "Phase", DgaPgPhases.Graduates },
                { "LeadText", "اللائحة المنظمة للدراسات العليا في الجامعات (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤) • الدليل الإرشادي والنماذج المعتمدة بموقع العمادة" }, { "LeadText_EN", "" },
                { "ItemOrder", 5 }, { "IsVisible", true }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetHeaderSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Hub
            list.Add(new Dictionary<string, object> {
                { "Title", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا" },
                { "Title_EN", "Interactive Academic Kit for Postgraduate Students" },
                { "Phase", DgaPgPhases.Hub },
                { "Description", "مرجع رقمي موحّد يواكب طالبة الدراسات العليا من القبول حتى منح الدرجة العلمية." },
                { "Description_EN", "A unified digital reference accompanying postgraduate students from admission to graduation." },
                { "BreadcrumbText", "الرئيسية|/ar/Pages/default.aspx;الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا|" },
                { "BreadcrumbText_EN", "Home|/en/Pages/default.aspx;Interactive Academic Kit for Postgraduate Students|" }
            });

            // Applicants
            list.Add(new Dictionary<string, object> {
                { "Title", "المتقدمون" },
                { "Title_EN", "Applicants" },
                { "Phase", DgaPgPhases.Applicants },
                { "Description", "شروط القبول والبرامج والمستندات والدورة التأهيلية للغة الإنجليزية." },
                { "Description_EN", "Admission requirements, programs, documents and English qualifying course." },
                { "BreadcrumbText", "الرئيسية|/ar/Pages/default.aspx;الحقيبة الأكاديمية|/ar/Deanship/postgraduate/Pages/academic-kit.aspx;المتقدمون|" },
                { "BreadcrumbText_EN", "Home|/en/Pages/default.aspx;Academic Kit|/en/Deanship/postgraduate/Pages/academic-kit.aspx;Applicants|" }
            });

            // Enrolled
            list.Add(new Dictionary<string, object> {
                { "Title", "المقيدون" },
                { "Title_EN", "Enrolled Students" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Description", "الرحلة الأكاديمية والحقوق واللوائح والإجراءات والخدمات المساندة." },
                { "Description_EN", "Academic journey, rights, regulations, procedures and support services." },
                { "BreadcrumbText", "الرئيسية|/ar/Pages/default.aspx;الحقيبة الأكاديمية|/ar/Deanship/postgraduate/Pages/academic-kit.aspx;المقيدون|" },
                { "BreadcrumbText_EN", "Home|/en/Pages/default.aspx;Academic Kit|/en/Deanship/postgraduate/Pages/academic-kit.aspx;Enrolled Students|" }
            });

            // Graduates
            list.Add(new Dictionary<string, object> {
                { "Title", "الخريجون" },
                { "Title_EN", "Graduates" },
                { "Phase", DgaPgPhases.Graduates },
                { "Description", "رحلة التخرّج ومتطلباتها وإجراءات منح الدرجة وإخلاء الطرف." },
                { "Description_EN", "Graduation journey, requirements, degree conferral and clearance procedures." },
                { "BreadcrumbText", "الرئيسية|/ar/Pages/default.aspx;الحقيبة الأكاديمية|/ar/Deanship/postgraduate/Pages/academic-kit.aspx;الخريجون|" },
                { "BreadcrumbText_EN", "Home|/en/Pages/default.aspx;Academic Kit|/en/Deanship/postgraduate/Pages/academic-kit.aspx;Graduates|" }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetIntroSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Hub Intro
            list.Add(new Dictionary<string, object> {
                { "Title", "نبذة تعريفية" }, { "Title_EN", "About the Kit" },
                { "Phase", DgaPgPhases.Hub },
                { "SubTitle", "" }, { "SubTitle_EN", "" },
                { "LeadText", "" }, { "LeadText_EN", "" },
                { "Paragraphs", "تُعدّ «الحقيبة الأكاديمية الذكية لطلبة الدراسات العليا» إحدى مبادرات وكالة الشؤون الأكاديمية بعمادة الدراسات العليا في مسارها نحو التحوّل الرقمي وتجويد الخدمة المقدَّمة لطلبتها؛ إذ تجمع في مرجعيةٍ رقميةٍ واحدةٍ منظّمةٍ كل ما تحتاجه الطالبة من إجراءاتٍ أكاديميةٍ ولوائحَ منظّمةٍ وأدلّةٍ إرشاديةٍ ونماذجَ معتمدة، وتربطها مباشرةً بمصادرها الرسمية في موقع العمادة لتظلّ محدّثةً على الدوام.\n\nوتنطلق هذه الحقيبة من إيمان العمادة بأنّ تيسير وصول الطالبة إلى المعلومة الصحيحة في حينها ركيزةٌ من ركائز جودة الحياة الأكاديمية؛ فجاءت لتختصر المسافة بين الطالبة وما تحتاجه، وتوحّد مرجعيتها، وترسّخ ثقافة الوضوح والشفافية في الإجراءات، مواكبةً لمستهدفات الجامعة ورؤية المملكة 2030 في تمكين بيئةٍ تعليميةٍ رقميةٍ متكاملة.\n\nوتنتظم الحقيبة في ثلاث محطّاتٍ تمثّل مراحل الرحلة الأكاديمية، تنتقل إليها الطالبة بحسب موضعها من مسيرتها:" },
                { "Paragraphs_EN", "The Smart Academic Kit for Postgraduate Students is an initiative of the Academic Affairs Agency at the Deanship of Graduate Studies towards digital transformation and service excellence.\n\nIt organizes all academic procedures, regulations, guides and approved forms into one single digital source." },
                { "Badges", "جامعة الأميرة نورة بنت عبد الرحمن • عمادة الدراسات العليا • وكالة الشؤون الأكاديمية\n١٤٤٧ - ١٤٤٨هـ" },
                { "Badges_EN", "Princess Nourah bint Abdulrahman University • Deanship of Graduate Studies\n1447 - 1448 AH" },
                { "ReturnText", "" }, { "ReturnText_EN", "" }, { "ReturnUrl", "" },
                { "IconClass", "hgi-book-open-01" }
            });

            // Applicants Intro
            list.Add(new Dictionary<string, object> {
                { "Title", "نبذة تعريفية" }, { "Title_EN", "Overview" },
                { "Phase", DgaPgPhases.Applicants },
                { "SubTitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا" },
                { "SubTitle_EN", "Interactive Academic Kit for Postgraduate Students" },
                { "LeadText", "تحرص عمادة الدراسات العليا على أن تبدأ رحلتكِ الأكاديمية من أساسٍ واضحٍ ومطمئن؛ ولذلك خُصّصت هذه المحطّة لتكون مرجعكِ الأول قبل الالتحاق، تجمع لكِ في موضعٍ واحدٍ منظّم كل ما تحتاجينه للتقديم على برامج الدراسات العليا: من خريطةٍ موضّحةٍ لمراحل القبول خطوةً بخطوة، وروابطَ مباشرةٍ للبرامج المطروحة وشروط القبول والرسوم، والدورة التأهيلية للغة الإنجليزية، ودليل استخدام بوابة القبول الإلكترونية. وهي تسعى إلى تمكينكِ من اتخاذ قرارٍ مدروسٍ، والانطلاق في مسيرتكِ على بصيرةٍ ووضوحٍ منذ خطوتكِ الأولى." },
                { "LeadText_EN", "The Deanship of Graduate Studies is dedicated to ensuring your academic journey begins on a clear and confident foundation. This station is designed to be your primary reference before admission." },
                { "Paragraphs", "رفيقٌ رقميٌّ موحّدٌ يواكب الطالبة من القبول حتى منح الدرجة العلمية — بالضغط على أي إجراءٍ تظهر شروطه ونصّ مادته ونماذجه." },
                { "Paragraphs_EN", "A unified digital companion accompanying students from admission to graduation — clicking any procedure reveals its conditions, rules, and forms." },
                { "Badges", "جامعة الأميرة نورة بنت عبد الرحمن • عمادة الدراسات العليا • وكالة الشؤون الأكاديمية\n١٤٤٧ - ١٤٤٨هـ" },
                { "Badges_EN", "Princess Nourah bint Abdulrahman University • Deanship of Graduate Studies\n1447 - 1448 AH" },
                { "ReturnText", "⟶ العودة لاختيار الفئة" }, { "ReturnText_EN", "⟶ Back to Kit Home" },
                { "ReturnUrl", "/ar/Deanship/postgraduate/Pages/academic-kit.aspx" },
                { "IconClass", "hgi-book-open-01" }
            });

            // Enrolled Intro
            list.Add(new Dictionary<string, object> {
                { "Title", "نبذة تعريفية" }, { "Title_EN", "Overview" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubTitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا" },
                { "SubTitle_EN", "Interactive Academic Kit for Postgraduate Students" },
                { "LeadText", "بوصفكِ طالبةً مقيَّدةً في الدراسات العليا، تمثّل هذه المحطّة رفيقكِ الأكاديمي طوال مسيرتكِ؛ إذ تحرص العمادة على أن تكون كل إجراءاتكِ وحقوقكِ ولوائحكِ في متناول يدكِ، واضحةً مفهومةً يسيرةَ الوصول. وقد جُمعت لكِ فيها خريطة رحلتكِ الأكاديمية، وحزمة اللوائح المنظّمة لحقوقكِ وواجباتكِ، والإجراءات الأكاديمية كاملةً بشروطها ونماذجها ومساراتها، إلى جانب حافز النشر العلمي والأدلة الإرشادية؛ لتكون مرجعكِ الموحّد الذي يرافقكِ من بدء الدراسة حتى إتمام متطلّباتها بثقةٍ واطمئنان." },
                { "LeadText_EN", "As an enrolled postgraduate student, this station represents your academic companion throughout your journey. All your procedures, rights and regulations are readily accessible." },
                { "Paragraphs", "رفيقٌ رقميٌّ موحّدٌ يواكب الطالبة من القبول حتى منح الدرجة العلمية — بالضغط على أي إجراءٍ تظهر شروطه ونصّ مادته ونماذجه." },
                { "Paragraphs_EN", "A unified digital companion accompanying students from admission to graduation — clicking any procedure reveals its conditions, rules, and forms." },
                { "Badges", "جامعة الأميرة نورة بنت عبد الرحمن • عمادة الدراسات العليا • وكالة الشؤون الأكاديمية\n١٤٤٧ - ١٤٤٨هـ" },
                { "Badges_EN", "Princess Nourah bint Abdulrahman University • Deanship of Graduate Studies\n1447 - 1448 AH" },
                { "ReturnText", "⟶ العودة لاختيار الفئة" }, { "ReturnText_EN", "⟶ Back to Kit Home" },
                { "ReturnUrl", "/ar/Deanship/postgraduate/Pages/academic-kit.aspx" },
                { "IconClass", "hgi-book-open-01" }
            });

            // Graduates Intro
            list.Add(new Dictionary<string, object> {
                { "Title", "نبذة تعريفية" }, { "Title_EN", "Overview" },
                { "Phase", DgaPgPhases.Graduates },
                { "SubTitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا" },
                { "SubTitle_EN", "Interactive Academic Kit for Postgraduate Students" },
                { "LeadText", "وأنتِ على أعتاب التخرّج بعد رحلةٍ من الجدّ والعطاء، تأتي هذه المحطّة لتكون دليلكِ في إتمام مسيرتكِ باستحقاقٍ ووضوح؛ حرصاً من العمادة على أن تُكلَّل جهودكِ بإجراءاتٍ ميسّرةٍ ومعلومةٍ بيّنة. وهي تبيّن لكِ خطوات التخرّج كاملةً من اكتمال المناقشة حتى استلام الوثيقة، ومتطلّبات التخرّج، وإجراءات منح الدرجة العلمية وإخلاء الطرف بنماذجها وجهاتها المعتمدة؛ لتغادري الجامعة وقد أتممتِ مسيرتكِ على أكمل وجهٍ وأيسر سبيل." },
                { "LeadText_EN", "As you approach graduation, this station guides you through completing all requirements with clarity, from defense completion to diploma receipt and clearance." },
                { "Paragraphs", "رفيقٌ رقميٌّ موحّدٌ يواكب الطالبة من القبول حتى منح الدرجة العلمية — بالضغط على أي إجراءٍ تظهر شروطه ونصّ مادته ونماذجه." },
                { "Paragraphs_EN", "A unified digital companion accompanying students from admission to graduation — clicking any procedure reveals its conditions, rules, and forms." },
                { "Badges", "جامعة الأميرة نورة بنت عبد الرحمن • عمادة الدراسات العليا • وكالة الشؤون الأكاديمية\n١٤٤٧ - ١٤٤٨هـ" },
                { "Badges_EN", "Princess Nourah bint Abdulrahman University • Deanship of Graduate Studies\n1447 - 1448 AH" },
                { "ReturnText", "⟶ العودة لاختيار الفئة" }, { "ReturnText_EN", "⟶ Back to Kit Home" },
                { "ReturnUrl", "/ar/Deanship/postgraduate/Pages/academic-kit.aspx" },
                { "IconClass", "hgi-book-open-01" }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetJourneySeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Applicants Journey Steps (5 steps)
            list.Add(new Dictionary<string, object> {
                { "Title", "متابعة الإعلان" }, { "Title_EN", "Follow Announcements" },
                { "Phase", DgaPgPhases.Applicants }, { "StepNumber", 1 },
                { "Description", "المرحلة 1 — متابعة إعلان فتح القبول لبرامج الدراسات العليا عبر موقع العمادة وقنواتها الرسمية، ومعرفة البرامج المطروحة لكل فصل." },
                { "Description_EN", "Stage 1 — Follow admission opening announcements on the deanship website and official channels." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "اختيار البرنامج والتحقّق من شروطه" }, { "Title_EN", "Select Program & Check Conditions" },
                { "Phase", DgaPgPhases.Applicants }, { "StepNumber", 2 },
                { "Description", "المرحلة 2 — الاطّلاع على البرامج المطروحة وشروط القبول الخاصة بكل برنامج، والتأكّد من استيفائها قبل التقديم." },
                { "Description_EN", "Stage 2 — Explore available programs and review specific admission requirements." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تجهيز المستندات" }, { "Title_EN", "Prepare Documents" },
                { "Phase", DgaPgPhases.Applicants }, { "StepNumber", 3 },
                { "Description", "المرحلة 3 — إعداد المستندات المطلوبة وفق شروط البرنامج (المؤهّل، السجل الأكاديمي، التزكيات، الهوية، وغيرها)، ورفعها في البوابة." },
                { "Description_EN", "Stage 3 — Prepare required documents according to program criteria." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "التقديم عبر البوابة" }, { "Title_EN", "Apply via Portal" },
                { "Phase", DgaPgPhases.Applicants }, { "StepNumber", 4 },
                { "Description", "المرحلة 4 — تعبئة طلب القبول إلكترونياً عبر بوابة القبول للدراسات العليا، ورفع المستندات، ومتابعة حالة الطلب." },
                { "Description_EN", "Stage 4 — Fill out online application via postgraduate portal and upload documents." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المفاضلة والنتيجة" }, { "Title_EN", "Evaluation & Results" },
                { "Phase", DgaPgPhases.Applicants }, { "StepNumber", 5 },
                { "Description", "المرحلة 5 — تتمّ المفاضلة وفق معايير كل برنامج، ثم تُعلَن النتائج عبر البوابة؛ وعلى المقبولة استكمال إجراءات التسجيل." },
                { "Description_EN", "Stage 5 — Merit-based evaluation is conducted and results are announced via portal." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 5 }
            });

            // Enrolled Journey Steps (5 steps)
            list.Add(new Dictionary<string, object> {
                { "Title", "تسجيل الجدول الدراسي" }, { "Title_EN", "Schedule Registration" },
                { "Phase", DgaPgPhases.Enrolled }, { "StepNumber", 1 },
                { "Description", "مرحلةٌ مشتركةٌ مع الإدارة التنفيذية: إدخال الجداول في النظام الأكاديمي وتسجيلها للطلبة." },
                { "Description_EN", "Stage 1 — Joint phase with executive administration: entering schedules into the academic system and registering them for students." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "دراسة المقرّرات" }, { "Title_EN", "Coursework Study" },
                { "Phase", DgaPgPhases.Enrolled }, { "StepNumber", 2 },
                { "Description", "الالتزام بالجدول وحضور المقررات وتسليم متطلباتها، والنجاح بتقدير «جيد مرتفع» على الأقل." },
                { "Description_EN", "Stage 2 — Commitment to the schedule, attending courses, submitting requirements, and passing with at least a 'High Good' grade." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تسجيل خطة البحث" }, { "Title_EN", "Research Proposal Registration" },
                { "Phase", DgaPgPhases.Enrolled }, { "StepNumber", 3 },
                { "Description", "مناقشة الفكرة مع المرشد/ـة الأكاديمي/ـة وتسجيل خطة الرسالة بعد إنجاز 50% من المقررات بمعدل لا يقل عن «جيد جداً»." },
                { "Description_EN", "Stage 3 — Discussing the idea with the academic advisor and registering the thesis proposal after completing 50% of coursework with a GPA of at least 'Very Good'." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تشكيل لجنة المناقشة" }, { "Title_EN", "Defense Committee Formation" },
                { "Phase", DgaPgPhases.Enrolled }, { "StepNumber", 4 },
                { "Description", "تقدّم المشرف/ـة بطلب تشكيل اللجنة، ورفع التوصيات والمسوّغات من عمادة الكلية." },
                { "Description_EN", "Stage 4 — The supervisor requests the committee formation, and recommendations/justifications are submitted by the college deanship." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "منح الدرجة العلمية" }, { "Title_EN", "Degree Conferral" },
                { "Phase", DgaPgPhases.Enrolled }, { "StepNumber", 5 },
                { "Description", "استكمال نماذج اللجنة والإقرار، وتسليم نسخ الرسالة المعتمدة، ورفع التوصية بمنح الدرجة." },
                { "Description_EN", "Stage 5 — Completing committee forms and acknowledgments, submitting approved thesis copies, and submitting recommendation for degree conferral." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 5 }
            });

            // Graduates Journey Steps (6 steps)
            list.Add(new Dictionary<string, object> {
                { "Title", "اكتمال المناقشة والتعديلات" }, { "Title_EN", "Defense Completion & Revisions" },
                { "Phase", DgaPgPhases.Graduates }, { "StepNumber", 1 },
                { "Description", "المرحلة 1 — استكمال مناقشة الرسالة وإجراء التعديلات المطلوبة والإقرار بها وفق توصية لجنة المناقشة." },
                { "Description_EN", "Stage 1 — Complete thesis defense, make required modifications, submit revision acknowledgment." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تسليم الرسالة العلمية إلكترونياً" }, { "Title_EN", "Electronic Thesis Submission" },
                { "Phase", DgaPgPhases.Graduates }, { "StepNumber", 2 },
                { "Description", "المرحلة 2 — تسليم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ورفعها إلى عمادة الدراسات العليا بصورتها المعتمدة." },
                { "Description_EN", "Stage 2 — Submit final electronic version of thesis to department, college, and deanship." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "رفع التوصية بمنح الدرجة" }, { "Title_EN", "Degree Conferral Recommendation" },
                { "Phase", DgaPgPhases.Graduates }, { "StepNumber", 3 },
                { "Description", "المرحلة 3 — يرفع القسم تقرير اللجنة إلى الكلية، وتضمّ وحدة الخريجين بالعمادة اسم الطالبة إلى مذكرة الخريجين تمهيداً لرفعها إلى مجلس الجامعة." },
                { "Description_EN", "Stage 3 — Department submits committee report to college; graduates unit adds name to graduation memo." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "اعتماد رئيسة الجامعة المكلّفة" }, { "Title_EN", "President Approval" },
                { "Phase", DgaPgPhases.Graduates }, { "StepNumber", 4 },
                { "Description", "المرحلة 4 — اعتماد مذكرة الخريجين ومنح الدرجة العلمية وفق الصلاحيات النظامية المقرّرة." },
                { "Description_EN", "Stage 4 — University President approves graduation memo and confers scientific degree." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "رفع خطاب لإصدار الوثيقة" }, { "Title_EN", "Diploma Issuance Request" },
                { "Phase", DgaPgPhases.Graduates }, { "StepNumber", 5 },
                { "Description", "المرحلة 5 — يُرفع خطابٌ إلى إدارة القبول والتسجيل لإصدار وثيقة التخرّج بعد اعتماد منح الدرجة." },
                { "Description_EN", "Stage 5 — Deanship sends official letter to Admissions & Registration for diploma issuance." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إخلاء الطرف" }, { "Title_EN", "University Clearance" },
                { "Phase", DgaPgPhases.Graduates }, { "StepNumber", 6 },
                { "Description", "المرحلة 6 — استكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة الواردة فيه، تمهيداً لاستلام وثيقة التخرّج." },
                { "Description_EN", "Stage 6 — Complete university clearance form across all required departments to receive diploma." },
                { "IconClass", "hgi-route-03" }, { "ItemOrder", 6 }
            });

            return list;
        }
    }
}
