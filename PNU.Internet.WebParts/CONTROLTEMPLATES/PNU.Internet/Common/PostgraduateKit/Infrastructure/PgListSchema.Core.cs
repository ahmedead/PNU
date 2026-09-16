using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Definitions and seed data for the structural lists: PgSections, PgHeader,
    /// PgIntro and PgJourney.
    /// </summary>
    public static partial class PgListSchema
    {
        private static void BuildCoreLists(Dictionary<string, PgListDef> map)
        {
            BuildSections(map);
            BuildHeader(map);
            BuildIntro(map);
            BuildJourney(map);
        }

        // 1) عناوين المحاور (Section headings and lead paragraphs)
        private static void BuildSections(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Sections] = new PgListDef
            {
                Name = PgListNames.Sections,
                TitleAr = "عناوين المحاور", TitleEn = "Section headings",
                Description = "Heading, lead paragraph and visibility of every axis, per audience.",
                Fields = new List<PgFieldDef>
                {
                    new PgFieldDef("SectionKey", "مفتاح المحور", "Section key", SPFieldType.Choice, true, 0,
                                   "يُطابق المفتاح المضبوط في عنصر التحكم.", PgSectionKeys.All),
                    TitleEn(),
                    new PgFieldDef("Lead", "الفقرة التمهيدية (عربي)", "Lead paragraph (AR)", SPFieldType.Note, false, 4),
                    new PgFieldDef("Lead_EN", "الفقرة التمهيدية (إنجليزي)", "Lead paragraph (EN)", SPFieldType.Note, false, 4),
                    YesNoField("Visible", "ظاهر", "Visible", true),
                    Phase(), Order()
                },
                Seed = SectionsSeed()
            };
        }

        private static Dictionary<string, string> Section(string phase, int order, string key,
                                                          string titleAr, string titleEn,
                                                          string leadAr = "", string leadEn = "")
        {
            return Row("SectionKey", key,
                       "Title", titleAr, "Title_EN", titleEn,
                       "Lead", leadAr, "Lead_EN", leadEn,
                       "Visible", "Yes",
                       "Phase", phase,
                       "ItemOrder", order.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        private static List<Dictionary<string, string>> SectionsSeed()
        {
            var seed = new List<Dictionary<string, string>>();

            // ---------- الصفحة الرئيسة للحقيبة / Hub ----------
            seed.Add(Section(PgPhases.Hub, 1, PgSectionKeys.Intro,
                "نبذة تعريفية", "Overview"));

            seed.Add(Section(PgPhases.Hub, 2, PgSectionKeys.Audiences,
                "اختر فئتك للوصول إلى ما يخصّك", "Choose your station to reach what concerns you",
                "تواكبك هذه الحقيبة في كل مرحلةٍ من مراحل رحلتك الأكاديمية",
                "This kit accompanies you at every stage of your academic journey"));

            // ---------- المتقدمون / Applicants ----------
            seed.Add(Section(PgPhases.Applicants, 1, PgSectionKeys.Intro,
                "النبذة التعريفية", "Overview"));

            seed.Add(Section(PgPhases.Applicants, 2, PgSectionKeys.Journey,
                "المحور الأول: خريطة رحلة القبول", "Axis One: The admission journey map"));

            seed.Add(Section(PgPhases.Applicants, 3, PgSectionKeys.Programs,
                "المحور الثاني: البرامج وشروط القبول والرسوم",
                "Axis Two: Programs, admission requirements and fees",
                "شروط القبول والرسوم تختلف بحسب البرنامج وتُحدَّث كل فصل؛ ولأن الرسوم مدرجةٌ في صفحة كل برنامج، ندلّكِ على المصادر الرسمية المحدّثة مباشرةً:",
                "Admission requirements and fees differ by program and are updated every term. Because the fees are published on each program page, the official up-to-date sources are linked directly below:"));

            seed.Add(Section(PgPhases.Applicants, 4, PgSectionKeys.EnglishCourse,
                "المحور الثالث: الدورة التأهيلية للغة الإنجليزية",
                "Axis Three: The English language preparatory course"));

            seed.Add(Section(PgPhases.Applicants, 5, PgSectionKeys.Faq,
                "المحور الرابع: الأسئلة الأكثر شيوعاً", "Axis Four: Frequently asked questions",
                "إجاباتٌ موجزةٌ عن أبرز الاستفسارات المتكرّرة لدى طلبة الدراسات العليا.",
                "Short answers to the questions postgraduate students ask most often."));

            seed.Add(Section(PgPhases.Applicants, 6, PgSectionKeys.Guides,
                "المحور الخامس: الحقوق والواجبات / اللوائح النظامية",
                "Axis Five: Rights, duties and official regulations"));

            seed.Add(Section(PgPhases.Applicants, 7, PgSectionKeys.Discounts,
                "المحور السادس: إعفاء وتخفيض الرسوم الدراسية لفئات محددة من طلبة الدراسات العليا",
                "Axis Six: Tuition exemptions and reductions for specific categories of postgraduate students",
                "الفئات التي يشملها الإعفاء والتخفيض:",
                "Categories covered by the exemption and the reduction:"));

            seed.Add(Section(PgPhases.Applicants, 8, PgSectionKeys.Services,
                "المحور السابع: الخدمات / المراكز", "Axis Seven: Services and centres",
                "جمعنا لكِ أبرز ما تحتاجينه من خدماتٍ ومراكزَ في الجامعة، موصولةً مباشرةً بصفحاتها الرسمية؛ لتصلي إليها بيسرٍ ووضوح.",
                "The services and centres you are most likely to need, linked straight to their official pages so you can reach them easily."));

            seed.Add(Section(PgPhases.Applicants, 9, PgSectionKeys.Housing,
                "المحور الثامن: السكن الجامعي", "Axis Eight: Student housing",
                "يوفّر السكن الجامعي بيئةً آمنةً ومهيّأةً لطالبات الدراسات العليا، وتُدار خدمة التسجيل فيه إلكترونياً عبر عمادة شؤون الطالبات. يمكنكِ الاطّلاع على الشروط والمستندات والرسوم والتقديم من خلال صفحة الخدمة الرسمية.",
                "Student housing offers a safe, well-equipped environment for postgraduate students, and registration is handled online by the Deanship of Student Affairs. Requirements, documents, fees and the application form are all on the official service page."));

            seed.Add(Section(PgPhases.Applicants, 10, PgSectionKeys.Contact,
                "المحور التاسع: تواصلي معنا", "Axis Nine: Contact us",
                "استفساراتكِ تهمّنا. إن احتجتِ أي مساعدةٍ أو إيضاح، تواصلي مع وحدة خدمة المستفيدين بعمادة الدراسات العليا عبر البريد الإلكتروني، وسيسعدنا خدمتكِ.",
                "Your questions matter to us. For any help or clarification, contact the Beneficiary Service Unit at the Deanship of Graduate Studies by email and we will be glad to assist you."));

            seed.Add(Section(PgPhases.Applicants, 11, PgSectionKeys.Footer,
                "الأساس النظامي والمرجعي", "Regulatory basis and references"));

            // ---------- المقيدون / Enrolled ----------
            seed.Add(Section(PgPhases.Enrolled, 1, PgSectionKeys.Intro,
                "النبذة التعريفية", "Overview"));

            seed.Add(Section(PgPhases.Enrolled, 2, PgSectionKeys.Journey,
                "المحور الأول: خريطة الرحلة الأكاديمية", "Axis One: The academic journey map",
                "خط سير طلبة الدراسات العليا المقيَّدة في خمس مراحلَ متتابعة، من تسجيل الجدول الدراسي حتى منح الدرجة العلمية.",
                "The path of an enrolled postgraduate student in five consecutive stages, from registering the study schedule to the award of the degree."));

            seed.Add(Section(PgPhases.Enrolled, 3, PgSectionKeys.Regulations,
                "المحور الثاني: الحقوق والواجبات واللوائح النظامية",
                "Axis Two: Rights, duties and official regulations",
                "حزمة اللوائح الرسمية المعتمدة بالجامعة التي تبيّن حقوق طالبة الدراسات العليا وواجباتها — بالضغط على اللائحة تُفتح مباشرةً.",
                "The university's approved regulations setting out the rights and duties of a postgraduate student. Select a document to open it directly."));

            seed.Add(Section(PgPhases.Enrolled, 4, PgSectionKeys.Procedures,
                "المحور الثالث: الإجراءات الأكاديمية", "Axis Three: Academic procedures",
                "خمسة عشر إجراءً مرتّبةً تصاعدياً وفق رقم المادة. بالضغط على الإجراء يتوسّع ليعرض شروطه ونصّ مادته والنماذج المرتبطة من موقع العمادة.",
                "Fifteen procedures ordered by article number. Select a procedure to expand it and see its requirements, the text of its article and the related forms from the deanship website."));

            seed.Add(Section(PgPhases.Enrolled, 5, PgSectionKeys.Faq,
                "المحور الرابع: الأسئلة الأكثر شيوعاً", "Axis Four: Frequently asked questions",
                "إجاباتٌ موجزةٌ عن أبرز الاستفسارات المتكرّرة لدى طلبة الدراسات العليا.",
                "Short answers to the questions postgraduate students ask most often."));

            seed.Add(Section(PgPhases.Enrolled, 6, PgSectionKeys.PublishIncentive,
                "المحور الخامس: حافز النشر العلمي", "Axis Five: The research publication incentive",
                "مبادرةٌ تحفيزيةٌ لدعم المسيرة البحثية لطلبة الدراسات العليا.",
                "An incentive initiative supporting the research path of postgraduate students."));

            seed.Add(Section(PgPhases.Enrolled, 7, PgSectionKeys.Discounts,
                "المحور السادس: إعفاء وتخفيض الرسوم الدراسية لفئات محددة من طلبة الدراسات العليا",
                "Axis Six: Tuition exemptions and reductions for specific categories of postgraduate students",
                "الفئات التي يشملها الإعفاء والتخفيض:",
                "Categories covered by the exemption and the reduction:"));

            seed.Add(Section(PgPhases.Enrolled, 8, PgSectionKeys.Guides,
                "المحور السابع: الأدلة الرسمية", "Axis Seven: Official guides",
                "أدلّةٌ رسميةٌ موصولةٌ مباشرةً بموقع العمادة — بالضغط على الدليل يُفتح مباشرةً.",
                "Official guides linked straight to the deanship website. Select a guide to open it directly."));

            seed.Add(Section(PgPhases.Enrolled, 9, PgSectionKeys.Services,
                "المحور الثامن: الخدمات / المراكز", "Axis Eight: Services and centres",
                "جمعنا لكِ أبرز ما تحتاجينه من خدماتٍ ومراكزَ في الجامعة، موصولةً مباشرةً بصفحاتها الرسمية؛ لتصلي إليها بيسرٍ ووضوح.",
                "The services and centres you are most likely to need, linked straight to their official pages so you can reach them easily."));

            seed.Add(Section(PgPhases.Enrolled, 10, PgSectionKeys.Housing,
                "المحور التاسع: السكن الجامعي", "Axis Nine: Student housing",
                "يوفّر السكن الجامعي بيئةً آمنةً ومهيّأةً لطالبات الدراسات العليا، وتُدار خدمة التسجيل فيه إلكترونياً عبر عمادة شؤون الطالبات. يمكنكِ الاطّلاع على الشروط والمستندات والرسوم والتقديم من خلال صفحة الخدمة الرسمية.",
                "Student housing offers a safe, well-equipped environment for postgraduate students, and registration is handled online by the Deanship of Student Affairs. Requirements, documents, fees and the application form are all on the official service page."));

            seed.Add(Section(PgPhases.Enrolled, 11, PgSectionKeys.Contact,
                "المحور العاشر: تواصلي معنا", "Axis Ten: Contact us",
                "استفساراتكِ تهمّنا. إن احتجتِ أي مساعدةٍ أو إيضاح، تواصلي مع وحدة خدمة المستفيدين بعمادة الدراسات العليا عبر البريد الإلكتروني، وسيسعدنا خدمتكِ.",
                "Your questions matter to us. For any help or clarification, contact the Beneficiary Service Unit at the Deanship of Graduate Studies by email and we will be glad to assist you."));

            seed.Add(Section(PgPhases.Enrolled, 12, PgSectionKeys.Footer,
                "الأساس النظامي والمرجعي", "Regulatory basis and references"));

            // ---------- الخريجون / Graduates ----------
            seed.Add(Section(PgPhases.Graduates, 1, PgSectionKeys.Intro,
                "النبذة التعريفية", "Overview"));

            seed.Add(Section(PgPhases.Graduates, 2, PgSectionKeys.Journey,
                "المحور الأول: رحلة التخرّج", "Axis One: The graduation journey"));

            seed.Add(Section(PgPhases.Graduates, 3, PgSectionKeys.GradRequirements,
                "المحور الثاني: متطلّبات التخرّج", "Axis Two: Graduation requirements",
                "تستكمل الخرِّيجة المتطلّبات التالية لإتمام تخرّجها واستلام وثيقتها:",
                "A graduating student completes the following requirements before receiving her graduation document:"));

            seed.Add(Section(PgPhases.Graduates, 4, PgSectionKeys.Procedures,
                "المحور الثالث: إجراءات التخرّج", "Axis Three: Graduation procedures"));

            seed.Add(Section(PgPhases.Graduates, 5, PgSectionKeys.Faq,
                "المحور الرابع: الأسئلة الأكثر شيوعاً", "Axis Four: Frequently asked questions"));

            seed.Add(Section(PgPhases.Graduates, 6, PgSectionKeys.Footer,
                "الأساس النظامي والمرجعي", "Regulatory basis and references"));

            return seed;
        }

        // 2) ترويسة الصفحة (Hero title and breadcrumb)
        private static void BuildHeader(Dictionary<string, PgListDef> map)
        {
            const string kitAr = "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا";
            const string kitEn = "The interactive academic kit for postgraduate students";
            const string kitUrlAr = "/ar/Deanship/postgraduate/Pages/academic-kit.aspx";
            const string kitUrlEn = "/en/Deanship/postgraduate/Pages/academic-kit.aspx";

            map[PgListNames.Header] = new PgListDef
            {
                Name = PgListNames.Header,
                TitleAr = "ترويسة الصفحة", TitleEn = "Page header",
                Description = "Hero title, description and breadcrumb of each audience page.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(),
                    new PgFieldDef("BreadcrumbText", "مسار التنقل (عربي)", "Breadcrumb (AR)", SPFieldType.Note, false, 2,
                                   "Title|Url;Title|Url;Title|"),
                    new PgFieldDef("BreadcrumbText_EN", "مسار التنقل (إنجليزي)", "Breadcrumb (EN)", SPFieldType.Note, false, 2),
                    Phase(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title", "المتقدمون", "Title_EN", "Applicants",
                        "BreadcrumbText", kitAr + "|" + kitUrlAr + ";المتقدمون|",
                        "BreadcrumbText_EN", kitEn + "|" + kitUrlEn + ";Applicants|",
                        "Phase", PgPhases.Applicants, "ItemOrder", "1"),

                    Row("Title", "المقيدون", "Title_EN", "Enrolled students",
                        "BreadcrumbText", kitAr + "|" + kitUrlAr + ";المقيدون|",
                        "BreadcrumbText_EN", kitEn + "|" + kitUrlEn + ";Enrolled students|",
                        "Phase", PgPhases.Enrolled, "ItemOrder", "1"),

                    Row("Title", "الخريجون", "Title_EN", "Graduates",
                        "BreadcrumbText", kitAr + "|" + kitUrlAr + ";الخريجون|",
                        "BreadcrumbText_EN", kitEn + "|" + kitUrlEn + ";Graduates|",
                        "Phase", PgPhases.Graduates, "ItemOrder", "1")
                }
            };
        }

        // 3) النبذة التعريفية (Overview)
        private static void BuildIntro(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Intro] = new PgListDef
            {
                Name = PgListNames.Intro,
                TitleAr = "النبذة التعريفية", TitleEn = "Overview",
                Description = "The introductory paragraph shown at the top of each audience page.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), DescAr(8), DescEn(8), Phase(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title", "نبذة تعريفية", "Title_EN", "Overview",
                        "Description", "تُعدّ الحقيبة الأكاديمية الذكية لطلبة الدراسات العليا إحدى مبادرات وكالة الشؤون الأكاديمية بعمادة الدراسات العليا في مسارها نحو التحوّل الرقمي وتجويد الخدمة المقدَّمة لطلبتها؛ إذ تجمع في مرجعيةٍ رقميةٍ واحدةٍ منظّمةٍ كل ما يحتاجه الطلبة من إجراءاتٍ أكاديميةٍ ولوائحَ منظّمةٍ وأدلّةٍ إرشاديةٍ ونماذجَ معتمدة، وتربطهم مباشرةً بالمصادر الرسمية في موقع العمادة لتظلّ محدّثةً على الدوام.\nوتنطلق هذه الحقيبة من إيمان العمادة بأنّ تيسير وصول الطلبة إلى المعلومة الصحيحة في حينها ركيزةٌ من ركائز جودة الحياة الأكاديمية؛ فجاءت لتختصر المسافة بين الطلبة وما يحتاجونه، وتوحّد مرجعيتهم، وترسّخ ثقافة الوضوح والشفافية في الإجراءات، مواكبةً لمستهدفات الجامعة ورؤية المملكة 2030 في تمكين بيئةٍ تعليميةٍ رقميةٍ متكاملة.\nوتنتظم الحقيبة في ثلاث محطّاتٍ تمثّل مراحل الرحلة الأكاديمية، تنتقل إليها الطلبة بحسب موضعها من مسيرتها:",
                        "Description_EN", "The smart academic kit for postgraduate students is one of the initiatives of the Vice-Deanship for Academic Affairs at the Deanship of Graduate Studies on its path towards digital transformation and better service for its students. It gathers into a single organised digital reference everything students need of academic procedures, governing regulations, guides and approved forms, and links them directly to the official sources on the deanship website so that it always stays current.\nThe kit proceeds from the deanship's belief that making the right information easy to reach at the right time is a pillar of academic quality of life. It was created to shorten the distance between students and what they need, to unify their point of reference, and to establish a culture of clarity and transparency in procedures, in step with the university's goals and Saudi Vision 2030 in enabling an integrated digital learning environment.\nThe kit is organised into three stations representing the stages of the academic journey, which students move between according to where they stand on their path:",
                        "Phase", PgPhases.Hub, "ItemOrder", "1"),

                    Row("Title", "النبذة التعريفية", "Title_EN", "Overview",
                        "Description", "تحرص عمادة الدراسات العليا على أن تبدأ رحلتك الأكاديمية من أساسٍ واضحٍ ومطمئن؛ ولذلك خُصّصت هذه المحطّة لتكون مرجعك الأول قبل الالتحاق، تجمع لك في موضعٍ واحدٍ منظّم كل ما تحتاجه للتقديم على برامج الدراسات العليا: من خريطةٍ موضّحةٍ لمراحل القبول خطوةً بخطوة، وروابطَ مباشرةٍ للبرامج المطروحة وشروط القبول والرسوم، والدورة التأهيلية للغة الإنجليزية، ودليل استخدام بوابة القبول الإلكترونية. وهي تسعى إلى تمكينك من اتخاذ قرارٍ مدروسٍ، والانطلاق في مسيرتك على بصيرةٍ ووضوحٍ منذ خطوتك الأولى.",
                        "Description_EN", "The Deanship of Graduate Studies wants your academic journey to begin on clear and reassuring ground. This station is therefore your first reference before you enrol, gathering in one organised place everything you need in order to apply to postgraduate programs: a step-by-step map of the admission stages, direct links to the programs on offer with their admission requirements and fees, the English language preparatory course, and the user guide for the electronic admission portal. Its aim is to help you make a well-considered decision and to start your path with insight and clarity from the very first step.",
                        "Phase", PgPhases.Applicants, "ItemOrder", "1"),

                    Row("Title", "النبذة التعريفية", "Title_EN", "Overview",
                        "Description", "بعد أن أصبحتِ من طلبة الدراسات العليا المقيَّدات، تأتي هذه المحطّة لتكون مرجعكِ اليومي في مسيرتكِ الأكاديمية؛ تجمع لكِ خريطة الرحلة من تسجيل الجدول الدراسي حتى منح الدرجة العلمية، واللوائح المنظِّمة لحقوقكِ وواجباتكِ، والإجراءات الأكاديمية بشروطها ونصوص موادّها ونماذجها المعتمدة، إضافةً إلى حافز النشر العلمي وتخفيضات الرسوم والخدمات والمراكز المساندة؛ لتمضي في دراستكِ بخطىً واثقةٍ ومعلومةٍ بيّنة.",
                        "Description_EN", "Now that you are an enrolled postgraduate student, this station is your day-to-day reference along the academic path. It brings together the journey map from registering your study schedule to the award of the degree, the regulations that define your rights and duties, and the academic procedures with their requirements, article texts and approved forms, alongside the research publication incentive, the tuition reductions and the supporting services and centres, so that you can move through your studies with confident and well-informed steps.",
                        "Phase", PgPhases.Enrolled, "ItemOrder", "1"),

                    Row("Title", "النبذة التعريفية", "Title_EN", "Overview",
                        "Description", "وأنتِ على أعتاب التخرّج بعد رحلةٍ من الجدّ والعطاء، تأتي هذه المحطّة لتكون دليلكِ في إتمام مسيرتكِ باستحقاقٍ ووضوح؛ حرصاً من العمادة على أن تُكلَّل جهودكِ بإجراءاتٍ ميسّرةٍ ومعلومةٍ بيّنة. وهي تبيّن لكِ خطوات التخرّج كاملةً من اكتمال المناقشة حتى استلام الوثيقة، ومتطلّبات التخرّج، وإجراءات منح الدرجة العلمية وإخلاء الطرف بنماذجها وجهاتها المعتمدة؛ لتغادري الجامعة وقد أتممتِ مسيرتكِ على أكمل وجهٍ وأيسر سبيل.",
                        "Description_EN", "As you stand at the threshold of graduation after a journey of diligence and dedication, this station is your guide to completing the path with clarity and merit, so that your effort is crowned by procedures that are simple and clearly explained. It sets out the full graduation steps from the completion of the thesis defence to the receipt of your document, the graduation requirements, and the procedures for the award of the degree and the clearance form with their approved entities and forms, so that you leave the university having completed your path in the best and easiest way.",
                        "Phase", PgPhases.Graduates, "ItemOrder", "1")
                }
            };
        }

        // 4) خريطة الرحلة (Journey steps)
        private static void BuildJourney(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Journey] = new PgListDef
            {
                Name = PgListNames.Journey,
                TitleAr = "خريطة الرحلة", TitleEn = "Journey map",
                Description = "The numbered steps of the admission, academic and graduation journeys.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), DescAr(4), DescEn(4),
                    new PgFieldDef("StepLabel", "مسمّى المرحلة (عربي)", "Stage label (AR)", SPFieldType.Text, true),
                    new PgFieldDef("StepLabel_EN", "مسمّى المرحلة (إنجليزي)", "Stage label (EN)", SPFieldType.Text),
                    Icon(), Link(), Phase(), Order()
                },
                Seed = JourneySeed()
            };
        }

        private static Dictionary<string, string> Step(string phase, int order, string icon,
                                                       string titleAr, string titleEn,
                                                       string descAr, string descEn,
                                                       string labelAr = "", string labelEn = "")
        {
            return Row("Title", titleAr, "Title_EN", titleEn,
                       "Description", descAr, "Description_EN", descEn,
                       "StepLabel", labelAr, "StepLabel_EN", labelEn,
                       "IconUrl", IconRoot + icon,
                       "Phase", phase,
                       "ItemOrder", order.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        private static List<Dictionary<string, string>> JourneySeed()
        {
            var seed = new List<Dictionary<string, string>>();

            // ---------- المتقدمون / Applicants: خريطة رحلة القبول ----------
            seed.Add(Step(PgPhases.Applicants, 1, "step01.png",
                "متابعة الإعلان", "Following the announcement",
                "متابعة إعلان فتح القبول لبرامج الدراسات العليا عبر موقع العمادة وقنواتها الرسمية، ومعرفة البرامج المطروحة لكل فصل.",
                "Follow the announcement of the opening of admission to postgraduate programs on the deanship website and its official channels, and find out which programs are offered each term."));

            seed.Add(Step(PgPhases.Applicants, 2, "step02.png",
                "اختيار البرنامج والتحقّق من شروطه", "Choosing a program and checking its requirements",
                "الاطّلاع على البرامج المطروحة وشروط القبول الخاصة بكل برنامج، والتأكّد من استيفائها قبل التقديم.",
                "Review the programs on offer and the admission requirements specific to each one, and make sure you meet them before applying."));

            seed.Add(Step(PgPhases.Applicants, 3, "step03.png",
                "تجهيز المستندات", "Preparing the documents",
                "إعداد المستندات المطلوبة وفق شروط البرنامج (المؤهّل، السجل الأكاديمي، التزكيات، الهوية، وغيرها)، ورفعها في البوابة.",
                "Prepare the documents required by the program (qualification, academic transcript, recommendations, identity document and others) and upload them to the portal."));

            seed.Add(Step(PgPhases.Applicants, 4, "step04.png",
                "التقديم عبر البوابة", "Applying through the portal",
                "تعبئة طلب القبول إلكترونياً عبر بوابة القبول للدراسات العليا، ورفع المستندات، ومتابعة حالة الطلب.",
                "Complete the admission application online through the postgraduate admission portal, upload the documents and follow the status of the application."));

            seed.Add(Step(PgPhases.Applicants, 5, "step05.png",
                "فترات التقديم وبوابة القبول الإلكتروني", "Application periods and the electronic admission portal",
                "معرفة مواعيد فتح القبول والدخول إلى بوابة التقديم الإلكترونية.",
                "Find out the admission opening dates and sign in to the electronic application portal."));

            // ---------- المقيدون / Enrolled: خريطة الرحلة الأكاديمية ----------
            seed.Add(Step(PgPhases.Enrolled, 1, "step001.png",
                "تسجيل الجدول الدراسي", "Registering the study schedule",
                "مرحلةٌ مشتركةٌ مع الإدارة التنفيذية: إدخال الجداول في النظام الأكاديمي وتسجيلها للطلبة.",
                "A stage shared with the executive administration: the schedules are entered into the academic system and registered for the students.",
                "المرحلة ١", "Stage 1"));

            seed.Add(Step(PgPhases.Enrolled, 2, "step002.png",
                "دراسة المقررات", "Studying the courses",
                "الالتزام بالجدول وحضور المقررات وتسليم متطلباتها، والنجاح بتقدير «جيد مرتفع» على الأقل.",
                "Keep to the schedule, attend the courses, submit their requirements and pass with a grade of at least \"high good\".",
                "المرحلة ٢", "Stage 2"));

            seed.Add(Step(PgPhases.Enrolled, 3, "step003.png",
                "تسجيل خطة البحث", "Registering the research plan",
                "مناقشة الفكرة مع المرشد/ـة الأكاديمي/ـة وتسجيل خطة الرسالة بعد إنجاز 50% من المقررات بمعدل لا يقل عن «جيد جداً».",
                "Discuss the idea with your academic advisor and register the thesis plan after completing 50% of the courses with a grade point average of no less than \"very good\".",
                "المرحلة ٣", "Stage 3"));

            seed.Add(Step(PgPhases.Enrolled, 4, "step004.png",
                "تشكيل لجنة المناقشة", "Forming the examination committee",
                "تقدّم المشرف/ـة بطلب تشكيل اللجنة، ورفع التوصيات والمسوّغات من عمادة الكلية.",
                "The supervisor submits the request to form the committee, and the college deanship forwards the recommendations and their justifications.",
                "المرحلة ٤", "Stage 4"));

            seed.Add(Step(PgPhases.Enrolled, 5, "step005.png",
                "منح الدرجة العلمية", "Award of the degree",
                "استكمال نماذج اللجنة والإقرار، وتسليم نسخ الرسالة المعتمدة، ورفع التوصية بمنح الدرجة.",
                "Complete the committee and acknowledgement forms, submit the approved copies of the thesis and forward the recommendation to award the degree.",
                "المرحلة ٥", "Stage 5"));

            // ---------- الخريجون / Graduates: رحلة التخرّج ----------
            seed.Add(Step(PgPhases.Graduates, 1, "step0001.png",
                "اكتمال المناقشة والتعديلات", "Completing the defence and the amendments",
                "استكمال مناقشة الرسالة وإجراء التعديلات المطلوبة والإقرار بها وفق توصية لجنة المناقشة.",
                "Complete the thesis defence, make the required amendments and acknowledge them in line with the examination committee's recommendation."));

            seed.Add(Step(PgPhases.Graduates, 2, "step0002.png",
                "تسليم الرسالة العلمية إلكترونياً", "Submitting the thesis electronically",
                "تسليم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ورفعها إلى عمادة الدراسات العليا بصورتها المعتمدة.",
                "Submit the final electronic version of the thesis to the department and the college, which forward the approved version to the Deanship of Graduate Studies."));

            seed.Add(Step(PgPhases.Graduates, 3, "step0003.png",
                "رفع التوصية بمنح الدرجة", "Forwarding the recommendation to award the degree",
                "يرفع القسم تقرير اللجنة إلى الكلية، وتضمّ وحدة الخريجين بالعمادة اسم الطالبة إلى مذكرة الخريجين تمهيداً لرفعها إلى مجلس الجامعة.",
                "The department forwards the committee report to the college, and the graduates unit at the deanship adds the student's name to the graduates memorandum before it goes to the university council."));

            seed.Add(Step(PgPhases.Graduates, 4, "step0004.png",
                "اعتماد رئيسة الجامعة المكلّفة", "Approval by the acting university president",
                "اعتماد مذكرة الخريجين ومنح الدرجة العلمية وفق الصلاحيات النظامية المقرّرة.",
                "The graduates memorandum and the award of the degree are approved under the established statutory powers."));

            seed.Add(Step(PgPhases.Graduates, 5, "step0005.png",
                "رفع خطاب لإصدار الوثيقة", "Requesting the issue of the document",
                "يُرفع خطابٌ إلى إدارة القبول والتسجيل لإصدار وثيقة التخرّج بعد اعتماد منح الدرجة.",
                "A letter is sent to the Admission and Registration Department to issue the graduation document once the award of the degree is approved."));

            seed.Add(Step(PgPhases.Graduates, 6, "step0006.png",
                "إخلاء الطرف", "Clearance",
                "استكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة الواردة فيه، تمهيداً لاستلام وثيقة التخرّج.",
                "Complete the clearance form with the approval of the entities listed on it, in preparation for receiving the graduation document."));

            return seed;
        }
    }
}
