using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    public static partial class DgaPgListSchema
    {
        private static void RegisterContentLists()
        {
            // 1. DgaPgResources
            var resources = new DgaPgListDef(DgaPgListNames.Resources, "المصادر واللوائح والأدلة (DGA)", "البرامج واللوائح والأدلة الرسمية");
            resources.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            resources.AddField("Phase", "Phase", SPFieldType.Choice, false, DgaPgPhases.All);
            resources.AddField("GroupKey", "Group Key", SPFieldType.Choice, true, new[] { "Programs", "Regulations", "Guides", "GradRequirements" });
            resources.AddField("Description", "Description (AR)", SPFieldType.Note, false);
            resources.AddField("Description_EN", "Description (EN)", SPFieldType.Note, false);
            resources.AddField("LinkUrl", "Link URL", SPFieldType.Text, false);
            resources.AddField("LinkText", "Link Text (AR)", SPFieldType.Text, false);
            resources.AddField("LinkText_EN", "Link Text (EN)", SPFieldType.Text, false);
            resources.AddField("BadgeText", "Badge Text (AR)", SPFieldType.Text, false);
            resources.AddField("BadgeText_EN", "Badge Text (EN)", SPFieldType.Text, false);
            resources.AddField("Bullets", "Bullets (AR)", SPFieldType.Note, false);
            resources.AddField("Bullets_EN", "Bullets (EN)", SPFieldType.Note, false);
            resources.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            resources.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            resources.SeedDataFactory = GetResourcesSeed;
            Register(resources);

            // 2. DgaPgContentBlocks
            var contentBlocks = new DgaPgListDef(DgaPgListNames.ContentBlocks, "البطاقات والمحتوى المتخصص (DGA)", "الدورة التأهيلية، حافز النشر، هوية الخريج");
            contentBlocks.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            contentBlocks.AddField("Phase", "Phase", SPFieldType.Choice, false, DgaPgPhases.All);
            contentBlocks.AddField("BlockKey", "Block Key", SPFieldType.Choice, true, new[] { "EnglishCourse", "PublishIncentive", "GraduateIdentity", "FinancialRules", "Footer" });
            contentBlocks.AddField("Description", "Description (AR)", SPFieldType.Note, false);
            contentBlocks.AddField("Description_EN", "Description (EN)", SPFieldType.Note, false);
            contentBlocks.AddField("Paragraphs", "Paragraphs (AR)", SPFieldType.Note, false);
            contentBlocks.AddField("Paragraphs_EN", "Paragraphs (EN)", SPFieldType.Note, false);
            contentBlocks.AddField("Bullets", "Bullets (AR)", SPFieldType.Note, false);
            contentBlocks.AddField("Bullets_EN", "Bullets (EN)", SPFieldType.Note, false);
            contentBlocks.AddField("LinkUrl", "Link URL", SPFieldType.Text, false);
            contentBlocks.AddField("LinkText", "Link Text (AR)", SPFieldType.Text, false);
            contentBlocks.AddField("LinkText_EN", "Link Text (EN)", SPFieldType.Text, false);
            contentBlocks.AddField("BadgeText", "Badge Text (AR)", SPFieldType.Text, false);
            contentBlocks.AddField("BadgeText_EN", "Badge Text (EN)", SPFieldType.Text, false);
            contentBlocks.AddField("Groups", "Headed Sub-blocks (AR)", SPFieldType.Note, false);
            contentBlocks.AddField("Groups_EN", "Headed Sub-blocks (EN)", SPFieldType.Note, false);
            contentBlocks.AddField("NoteText", "Closing Note (AR)", SPFieldType.Note, false);
            contentBlocks.AddField("NoteText_EN", "Closing Note (EN)", SPFieldType.Note, false);
            contentBlocks.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            contentBlocks.AddField("IconUrl", "Illustration URL", SPFieldType.Text, false);
            contentBlocks.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            contentBlocks.SeedDataFactory = GetContentBlocksSeed;
            Register(contentBlocks);

            // 3. DgaPgDiscounts
            var discounts = new DgaPgListDef(DgaPgListNames.Discounts, "جدول الإعفاء والتخفيض (DGA)", "فئات ونسب تخفيض الرسوم الدراسية");
            discounts.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            discounts.AddField("Percentage", "Discount Percentage", SPFieldType.Text, true);
            discounts.AddField("Conditions", "Conditions (AR)", SPFieldType.Note, false);
            discounts.AddField("Conditions_EN", "Conditions (EN)", SPFieldType.Note, false);
            discounts.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            discounts.SeedDataFactory = GetDiscountsSeed;
            Register(discounts);

            // 4. DgaPgDiscountNotes
            var discountNotes = new DgaPgListDef(DgaPgListNames.DiscountNotes, "ملاحظات الإعفاء والتخفيض (DGA)", "الضوابط والشروط العامة لتخفيض الرسوم");
            discountNotes.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            discountNotes.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            discountNotes.SeedDataFactory = GetDiscountNotesSeed;
            Register(discountNotes);

            // 5. DgaPgServices
            var services = new DgaPgListDef(DgaPgListNames.Services, "الخدمات والمراكز (DGA)", "مراكز وخدمات الجامعة المساندة");
            services.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            services.AddField("Phase", "Phase", SPFieldType.Choice, false, DgaPgPhases.All);
            services.AddField("Category", "Group / Category (AR)", SPFieldType.Text, false);
            services.AddField("Category_EN", "Group / Category (EN)", SPFieldType.Text, false);
            services.AddField("Description", "Description (AR)", SPFieldType.Note, false);
            services.AddField("Description_EN", "Description (EN)", SPFieldType.Note, false);
            services.AddField("LinkUrl", "Link URL", SPFieldType.Text, false);
            services.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            services.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            services.SeedDataFactory = GetServicesSeed;
            Register(services);

            // 6. DgaPgQuickLinks
            var quickLinks = new DgaPgListDef(DgaPgListNames.QuickLinks, "الروابط السريعة والتواصل (DGA)", "السكن الجامعي، تواصلي معنا، القنوات المباشرة");
            quickLinks.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            quickLinks.AddField("Phase", "Phase", SPFieldType.Choice, false, DgaPgPhases.All);
            quickLinks.AddField("SectionKey", "Section Key", SPFieldType.Choice, true, new[] { "Housing", "Contact" });
            quickLinks.AddField("Description", "Description (AR)", SPFieldType.Note, false);
            quickLinks.AddField("Description_EN", "Description (EN)", SPFieldType.Note, false);
            quickLinks.AddField("LinkUrl", "Link URL", SPFieldType.Text, false);
            quickLinks.AddField("LinkText", "Link Text (AR)", SPFieldType.Text, false);
            quickLinks.AddField("LinkText_EN", "Link Text (EN)", SPFieldType.Text, false);
            quickLinks.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            quickLinks.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            quickLinks.SeedDataFactory = GetQuickLinksSeed;
            Register(quickLinks);
        }

        private static List<Dictionary<string, object>> GetResourcesSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Programs & Fees (Applicants)
            list.Add(new Dictionary<string, object> {
                { "Title", "برامج الدراسات العليا والرسوم الدراسية" },
                { "Title_EN", "Postgraduate Programs & Tuition Fees" },
                { "Phase", DgaPgPhases.Applicants },
                { "GroupKey", "Programs" },
                { "Description", "تصفّح البرامج المتاحة (دبلوم عالٍ، ماجستير، دكتوراه) وتخصّصاتها ورسومها الدراسية — محدّثةٌ دائماً من المصدر." },
                { "Description_EN", "Browse available diploma, master's and PhD programs with specialties and tuition fees." },
                { "LinkUrl", "https://pnu.edu.sa/ar/RegAdm/PGD/Pages/home.aspx" },
                { "LinkText", "" }, { "LinkText_EN", "" },
                { "IconClass", "hgi-link-square-02" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "شروط القبول وشرط اللغة الإنجليزية" },
                { "Title_EN", "Admission Requirements & English Condition" },
                { "Phase", DgaPgPhases.Applicants },
                { "GroupKey", "Programs" },
                { "Description", "الاطّلاع على شروط القبول العامة والخاصة بكل برنامج، وشرط اللغة الإنجليزية المعتمد." },
                { "Description_EN", "Review general and program-specific admission criteria and English language requirements." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/English.aspx" },
                { "LinkText", "" }, { "LinkText_EN", "" },
                { "IconClass", "hgi-link-square-02" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "فترات التقديم وبوابة القبول الإلكتروني" },
                { "Title_EN", "Application Periods & Admission Portal" },
                { "Phase", DgaPgPhases.Applicants },
                { "GroupKey", "Programs" },
                { "Description", "معرفة مواعيد فتح القبول والدخول إلى بوابة التقديم الإلكترونية." },
                { "Description_EN", "Learn admission schedules and access the online application portal." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Pages/Periods.aspx" },
                { "LinkText", "" }, { "LinkText_EN", "" },
                { "IconClass", "hgi-link-square-02" },
                { "ItemOrder", 3 }
            });

            // Regulations / Guides (Applicants - Axis 5)
            list.Add(new Dictionary<string, object> {
                { "Title", "دليل المستخدم لبوابة القبول الإلكترونية" },
                { "Title_EN", "Admission Portal User Guide" },
                { "Phase", DgaPgPhases.Applicants },
                { "GroupKey", "Regulations" },
                { "Description", "مرجعٌ يوضّح خطوات التسجيل والتقديم عبر البوابة." },
                { "Description_EN", "A reference illustrating registration and application steps via the portal." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الدليل الإرشادي لطلبة الدراسات العليا" },
                { "Title_EN", "Postgraduate Student Guide" },
                { "Phase", DgaPgPhases.Applicants },
                { "GroupKey", "Regulations" },
                { "Description", "مرجعٌ تعريفيٌّ عامٌّ بنظام الدراسة والإجراءات والخدمات." },
                { "Description_EN", "General introductory guide to study systems, procedures, and services." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الدليل الإجرائي لمسار الإجراءات الأكاديمية" },
                { "Title_EN", "Procedural Guide for Academic Workflows" },
                { "Phase", DgaPgPhases.Applicants },
                { "GroupKey", "Regulations" },
                { "Description", "تفصيلٌ لمسار كل إجراءٍ أكاديميٍّ خطوةً بخطوة." },
                { "Description_EN", "Step-by-step detail for each academic procedure workflow." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d9%8a%d9%94%d9%8a%20%d9%84%d9%85%d8%b3%d8%a7%d8%b1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d8%a1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%94%d9%83%d8%a7%d8%af%d9%8a%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "دليل إعداد خطة البحث والرسالة العلمية" },
                { "Title_EN", "Research Plan and Thesis Preparation Guide" },
                { "Phase", DgaPgPhases.Applicants },
                { "GroupKey", "Regulations" },
                { "Description", "إرشاداتٌ لإعداد الخطة البحثية وكتابة الرسالة العلمية." },
                { "Description_EN", "Guidelines for preparing research proposals and writing academic theses." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%b9%d8%af%d8%a7%d8%af%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%88%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d8%44%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 4 }
            });

            // Regulations (Enrolled - Axis 2)
            list.Add(new Dictionary<string, object> {
                { "Title", "اللائحة المنظمة للدراسات العليا في الجامعات وقواعدها التنفيذية" },
                { "Title_EN", "Unified Regulations for Postgraduate Studies and Executive Rules" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Regulations" },
                { "Description", "المصدر الأساس لحقوق طالبة الدراسات العليا وواجباتها الأكاديمية (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤)." },
                { "Description_EN", "Primary source for student rights and academic duties (Council of Universities' Affairs Resolution 2/9/1444)." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Lists/Guidelines/Attachments/4/%D8%A7%D9%84%D9%84%D8%A7%D9%8A%D9%94%D8%AD%D8%A9%20%D8%A7%D9%84%D9%85%D9%86%D8%B8%D9%85%D8%A9%20%D9%84%D9%84%D8%AF%D8%B1%D8%A7%D8%B3%D8%A7%D8%AA%20%D8%A7%D9%84%D8%B9%D9%84%D9%8a%D8%A7%20%D9%81%D9%8A%20%D8%A7%D9%84%D8%AC%D8%A7%D9%85%D8%B9%D8%A7%D8%AA.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "وثيقة حقوق والتزامات الطالبات بالجامعة" },
                { "Title_EN", "PNU Student Rights & Obligations Charter" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Regulations" },
                { "Description", "وثيقةٌ معتمدةٌ من عمادة شؤون الطالبات تبيّن حقوق الطالب/ـة والتزاماتها في البيئة الجامعية." },
                { "Description_EN", "Approved charter from Student Affairs outlining student rights and obligations." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/studaffairs/Pages/default.aspx" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "لائحة إسكان الطالبات بالجامعة" },
                { "Title_EN", "PNU Student Housing Regulations" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Regulations" },
                { "Description", "تنظّم ضوابط السكن الجامعي وحقوق الطالب/ـة المقيمة والتزاماتها." },
                { "Description_EN", "Regulates university housing criteria, resident student rights and obligations." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/studaffairs/Lists/Guidelines/Attachments/6/%D9%84%D8%A7%D8%A6%D8%AD%D8%A9%20%D8%A7%D8%B3%D9%83%D8%A7%D9%86%20%D8%A7%D8%B7%D8%A7%D9%84%D8%A8%D8%A7%D8%AA.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ميثاق طالبة جامعة الأميرة نورة" },
                { "Title_EN", "Princess Nourah University Student Charter" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Regulations" },
                { "Description", "يبيّن القيم والسمات المؤسسية المتوقّعة من الطالب/ـة والخريجة، ضمن ملاحق الدليل الإرشادي." },
                { "Description_EN", "Outlines institutional values and attributes expected from students and graduates." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 4 }
            });

            // Guides (Enrolled - Axis 7)
            list.Add(new Dictionary<string, object> {
                { "Title", "الدليل الإرشادي لطلبة الدراسات العليا" }, { "Title_EN", "Postgraduate Student Guide" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Guides" },
                { "Description", "مرجعٌ تعريفيٌّ عامٌّ بنظام الدراسة والإجراءات والخدمات." }, { "Description_EN", "General introductory guide to study systems, procedures, and services." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%20%d8%a7%d9%84%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%b1%d8%b4%d8%a7%d8%af%d9%8a%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الدليل الإجرائي لمسار الإجراءات الأكاديمية" }, { "Title_EN", "Procedural Guide for Academic Workflows" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Guides" },
                { "Description", "تفصيلٌ لمسار كل إجراءٍ أكاديميٍّ خطوةً بخطوة." }, { "Description_EN", "Step-by-step detail for each academic procedure workflow." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%ac%d8%b1%d8%a7%d8%a6%d9%94%d9%8a%20%d9%84%d9%85%d8%b3%d8%a7%d8%b1%d8%a7%d8%aa%20%d8%a7%d9%84%d8%a7%d9%94%d9%83%d8%a7%d8%af%d9%8a%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7%20%20.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "دليل إعداد خطة البحث والرسالة العلمية" }, { "Title_EN", "Research Plan and Thesis Preparation Guide" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Guides" },
                { "Description", "إرشاداتٌ لإعداد الخطة البحثية وكتابة الرسالة العلمية." }, { "Description_EN", "Guidelines for preparing research proposals and writing academic theses." },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "دليل إعداد خطة البحث والرسالة العلمية" }, { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "GroupKey", "Guides" },
                { "Description", "إرشاداتٌ لإعداد الخطة البحثية وكتابة الرسالة العلمية." }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%af%d9%84%d9%8a%d9%84%20%d8%a7%d9%95%d8%b9%d8%af%d8%a7%d8%af%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%88%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9%20%d9%84%d8%b7%d9%84%d8%a8%d8%a9%20%d8%a7%d9%84%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%a7%d9%84%d8%b9%d9%84%d9%8a%d8%a7.pdf" },
                { "LinkText", "استعراض" }, { "LinkText_EN", "View" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 3 }
            });

            // Graduation Requirements (Graduates)
            list.Add(new Dictionary<string, object> {
                { "Title", "تسليم الرسالة العلمية إلكترونياً" },
                { "Title_EN", "Electronic Thesis Submission" },
                { "Phase", DgaPgPhases.Graduates },
                { "GroupKey", "GradRequirements" },
                { "Description", "تسليم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ليتولّيا رفعها إلى عمادة الدراسات العليا بصورتها المعتمدة." },
                { "Description_EN", "Deliver the final electronic thesis to department and college for submission to deanship." },
                { "BadgeText", "1" }, { "BadgeText_EN", "1" },
                { "IconClass", "hgi-check-list" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تسليم نسختين ورقيتين للعمادة" },
                { "Title_EN", "Submit Two Hard Copies to Deanship" },
                { "Phase", DgaPgPhases.Graduates },
                { "GroupKey", "GradRequirements" },
                { "Description", "تسليم نسختين ورقيتين من الرسالة العلمية إلى عمادة الدراسات العليا." },
                { "Description_EN", "Submit two printed bound copies of the approved thesis to the Deanship." },
                { "BadgeText", "2" }, { "BadgeText_EN", "2" },
                { "IconClass", "hgi-check-list" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إخلاء الطرف" },
                { "Title_EN", "University Clearance" },
                { "Phase", DgaPgPhases.Graduates },
                { "GroupKey", "GradRequirements" },
                { "Description", "استكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة: مكتبة الجامعة، وسكن الطالبات، ووحدة الشؤون المالية بالعمادة، ووكالة الشؤون الأكاديمية للدراسات العليا؛ تمهيداً لاستلام وثيقة التخرّج من إدارة القبول والتسجيل." },
                { "Description_EN", "Complete clearance approvals from Central Library, Housing, Financial Affairs, and Academic Affairs." },
                { "BadgeText", "3" }, { "BadgeText_EN", "3" },
                { "IconClass", "hgi-check-list" },
                { "ItemOrder", 3 }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetContentBlocksSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // English Qualifying Course (Applicants)
            list.Add(new Dictionary<string, object> {
                { "Title", "الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا" },
                { "Title_EN", "English Qualifying Course for Postgraduate Students" },
                { "Phase", DgaPgPhases.Applicants },
                { "BlockKey", "EnglishCourse" },
                { "Description", "يقدّم معهد اللغة الإنجليزية بالجامعة دورةً تأهيليةً تُعِدّ الطالبة بالمهارات اللغوية الأكاديمية اللازمة للالتحاق ببرامج الدراسات العليا، وتُعادل الدرجة المطلوبة في اختبار اللغة الإنجليزية المعتمد للقبول." },
                { "Description_EN", "The English Language Institute offers a qualifying course equipping students with academic English skills equivalent to the required test score." },
                { "Bullets", "تركّز على المهارات الأكاديمية الأربع: الكتابة والقراءة والاستماع والمحادثة.\nتتضمّن مسارين تدريبيين بحسب المستوى، تجمع بين الحضور المباشر والتعلّم الذاتي.\nتُمنح شهادة اجتياز عند تحقيق نسبة 70% فأعلى في التقييمات.\nتستفيد المتدرّبة من مركزي التحدّث والكتابة بالمعهد." },
                { "Bullets_EN", "Focuses on the four academic skills: Writing, Reading, Listening, Speaking.\nOffers two tracks combining in-person sessions and self-paced learning.\nCertificate awarded upon achieving 70% or higher.\nAccess to Speaking and Writing Centers at the Institute." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Faculties/EI/Pages/GraduateStudentsCourses.aspx" },
                { "LinkText", "الإعلان والتسجيل في الدورة" }, { "LinkText_EN", "Announcement and Course Registration" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 1 }
            });

            // Publication Incentive (Enrolled)
            list.Add(new Dictionary<string, object> {
                { "Title", "النشر العلمي… طريقك إلى خصم 10% على الرسوم الدراسية" },
                { "Title_EN", "Scientific Publication: your route to a 10% tuition discount" },
                { "Phase", DgaPgPhases.Enrolled },
                { "BlockKey", "PublishIncentive" },
                { "Description", "حافزٌ بحثيٌّ لطلبة الدراسات العليا" },
                { "Description_EN", "A research incentive for postgraduate students" },
                { "BadgeText", "10%" }, { "BadgeText_EN", "10%" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-financial-rules.png" },
                { "Paragraphs", "إشارةً إلى قرار مجلس جامعة الأميرة نورة بنت عبد الرحمن رقم (١٠-٦-١٤٤٧هـ) المتضمّن الموافقة على إعفاء وتخفيض الرسوم الدراسية لبعض فئات طلبة الدراسات العليا، ومن ضمنهم الطلبة الحاصلون على النشر العلمي أثناء فترة الدراسة وفق ضوابط وآلية النشر العلمي بالجامعة؛ تقرّر تطبيق نسبة خصمٍ مقدارها 10% على المقبولين ابتداءً من العام الجامعي ١٤٤٨هـ، دعماً لمسيرتهم البحثية وتحفيزاً لهم على الاستمرار في تحقيق منجزاتٍ علميةٍ متميزة." },
                { "Paragraphs_EN", "" },
                { "Bullets", "للاطلاع على ضوابط النشر المعتمدة وآليته، يُنصح بالتواصل المبكر مع المشرف/ـة الأكاديمي/ـة لاختيار الوعاء العلمي المناسب وتوقيت النشر.\nليكن البحث العلمي بدايةَ إنجازٍ يواكب طموح الجامعة ورؤية المملكة 2030." },
                { "Bullets_EN", "" },
                { "Groups", "المشاركة في المؤتمرات العلمية|إلى جانب النشر، تمثّل المشاركة في المؤتمرات والندوات العلمية رافداً مهماً لإثراء التجربة البحثية وبناء الحضور الأكاديمي وتوسيع شبكة العلاقات العلمية. وهي مسارٌ تطويريٌّ مستقلّ لا يترتّب عليه خصم الرسوم، ويُوصى بمناقشة فرص المشاركة مع المشرف/ـة الأكاديمي/ـة." },
                { "Groups_EN", "" },
                { "NoteText", "" }, { "NoteText_EN", "" },
                { "LinkUrl", "" },
                { "LinkText", "" }, { "LinkText_EN", "" },
                { "IconClass", "hgi-award-02" },
                { "ItemOrder", 1 }
            });


            // Graduate identity charter (Enrolled, axis 2 accordion) - 9 traits from the v4 design
            list.Add(new Dictionary<string, object> {
                { "Title", "وثيقة هوية خريجة جامعة الأميرة نورة" },
                { "Title_EN", "Princess Nourah University Graduate Identity Charter" },
                { "Phase", DgaPgPhases.Enrolled },
                { "BlockKey", "GraduateIdentity" },
                { "BadgeText", "٩ خصائص" }, { "BadgeText_EN", "9 attributes" },
                { "Description", "الخصائص التسع التي تتطلع الجامعة إلى تمكين خريجيها منها، وفق وثيقة هوية الخريجة الصادرة عن وكالة الجامعة للشؤون التعليمية." },
                { "Description_EN", "" },
                { "Groups", "التحليل والإبداع|توظيف المعرفة في البحث والاستقصاء; التفكير الإبداعي والتحليل الناقد\nالخدمة المجتمعية|المسؤولية المجتمعية; التعاون; الإيثار\nريادة الأعمال|إدارة الأولويات والأهداف; التخطيط; البحث والابتكار; الوعي المالي والإنتاجية\nالقيم|الاعتزاز بالقيم والهوية الوطنية; الإيجابية; احترام الثقافات الأخرى; الانتماء الوظيفي; الإتقان; التسامح والوسطية\nالوعي والمعرفة|اتساع وعمق المعرفة بالنظريات والأسس المهنية; تطبيق المعرفة والانضباط; التجديد\nتطوير الذات|التعلم المستمر في ضوء المتغيرات; المرونة والتكيف مع المتغيرات; الصحة واللياقة البدنية\nالكفاءة المعلوماتية|الاستخدام الفعال للتقنية الحديثة والتكنولوجيا; استخراج البيانات وتقييمها\nالقيادة|القيادة والتأثير; تحمل المسؤولية; حل النزاعات; صنع القرار; المبادرة; إدارة الوقت\nالتواصل الفعّال|التواصل الفعّال باللغتين العربية والإنجليزية في السياق الاجتماعي والمهني; مهارات الذكاء العاطفي; العمل الجماعي" },
                { "Groups_EN", "" },
                { "NoteText", "" }, { "NoteText_EN", "" },
                { "IconClass", "hgi-checkmark-badge-01" },
                { "ItemOrder", 1 }
            });

            // Financial rules for academic procedures (Enrolled, axis 2 accordion) - 7 cases
            list.Add(new Dictionary<string, object> {
                { "Title", "التنظيمات المالية للإجراءات الأكاديمية" },
                { "Title_EN", "Financial Rules for Academic Procedures" },
                { "Phase", DgaPgPhases.Enrolled },
                { "BlockKey", "FinancialRules" },
                { "BadgeText", "٧ حالات" }, { "BadgeText_EN", "7 cases" },
                { "Description", "تبيّن التنظيمات المالية المعتمدة بالعمادة آثار الإجراءات الأكاديمية على الرسوم الدراسية، وفق ما يلي:" },
                { "Description_EN", "" },
                { "Groups", "١/ الانسحاب من البرنامج أو الاعتذار عن فصل دراسي|تُسترد الرسوم وفق توقيت الطلب من بدء الدراسة: 75% خلال الأسبوع الأول، و50% خلال الثاني، و25% خلال الثالث، ولا يُسترد شيءٌ بعد الأسبوع الثالث. ويُعتمد تاريخ الانسحاب بناءً على تاريخ إرسال النموذج المعتمد.\n٢/ حذف مقرر أو عدد من مقررات الفصل|لا تُسترد أي رسوم عند حذف بعض مقررات الفصل، ولا يُطالب الطالب/ـة برسومها عند إعادة تسجيلها لاحقًا؛ أمّا الانسحاب من البرنامج لاحقًا فلا يترتب عليه استرداد.\n٣/ تأجيل القبول|يفقد الطالب/ـة حق الاسترداد عند الانسحاب بعد تأجيل القبول. وفي حال إلغاء البرنامج أو تأجيل طرحه، يحق استرداد كامل الرسوم أو تمديد التأجيل لحين إعادة الطرح.\n٤/ تأجيل الدراسة|لا تُسدّد رسوم عن الفصل المؤجّل، ويُقدّم الطلب قبل بداية الفصل بأسبوعين على الأقل، وينطبق على من اجتاز فصلًا أو أكثر أو أنجز قدرًا مناسبًا من الرسالة.\n٥/ إعادة دراسة مقرر أو دراسة مقررات إضافية|تُحتسب رسوم إضافية على كل مقرر عند عدم اجتيازه أو انخفاض المعدل التراكمي عن «جيد جدًا» في فصلين متتاليين ولزوم إعادته أو دراسة مقررات إضافية.\n٦/ تسجيل فصل إضافي للرسالة|تُحتسب رسوم عن كل فصل إضافي يتجاوز عامًا دراسيًا واحدًا بعد المدة المحددة للبرنامج.\n٧/ الحصول على شهادة عند نقطة خروج|عند الخروج بدرجة أدنى — إن أتاحت خطة البرنامج ذلك — يُستكمل سداد الرسوم المستحقة بحسب الوحدات المطلوبة للدرجة الأدنى، ولا تُسترد رسوم الفصول السابقة للبرنامج الأساسي." },
                { "Groups_EN", "" },
                { "NoteText", "ملاحظة: تخضع جميع الإجراءات للائحة المنظمة للدراسات العليا وقواعدها التنفيذية بالجامعة، وللتقويم الجامعي المعلن، وتخضع البرامج القائمة على شراكات خارجية لضوابط خاصة." }, { "NoteText_EN", "" },
                { "IconClass", "hgi-chart" },
                { "ItemOrder", 2 }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetDiscountsSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // The 7 exemption / reduction tiers of the v4 design.
            list.Add(new Dictionary<string, object> {
                { "Title", "أبناء شهداء الواجب أو المصابين في أداء الواجب الوطني" }, { "Title_EN", "" },
                { "Percentage", "إعفاء كامل" },
                { "Conditions", "" }, { "Conditions_EN", "" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ذوو الإعاقة (حسب طبيعة البرنامج)" }, { "Title_EN", "" },
                { "Percentage", "٢٥٪" },
                { "Conditions", "" }, { "Conditions_EN", "" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "السداد المبكر لإجمالي رسوم البرنامج - طلبة الدراسات العليا" }, { "Title_EN", "" },
                { "Percentage", "١٠٪" },
                { "Conditions", "" }, { "Conditions_EN", "" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الإخوة طلبة الدراسات العليا" }, { "Title_EN", "" },
                { "Percentage", "١٠٪" },
                { "Conditions", "" }, { "Conditions_EN", "" },
                { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "جهات الإيفاد - الجهات الحكومية والمؤسسات التعليمية والجهات الخاصة" }, { "Title_EN", "" },
                { "Percentage", "١٠٪ إذا كان عدد الموفدين أقل من خمسة ١٥٪ إذا كان عدد الموفدين خمسة فأكثر" },
                { "Conditions", "" }, { "Conditions_EN", "" },
                { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المستفيدون من الضمان الاجتماعي والجمعيات الخيرية والمتبرعون بالأعضاء" }, { "Title_EN", "" },
                { "Percentage", "٢٠٪" },
                { "Conditions", "" }, { "Conditions_EN", "" },
                { "ItemOrder", 6 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "النشر العلمي أثناء الدراسة (وفق ضوابط وآلية النشر العلمي بجامعة الأميرة نورة بنت عبدالرحمن)" }, { "Title_EN", "" },
                { "Percentage", "١٠٪" },
                { "Conditions", "" }, { "Conditions_EN", "" },
                { "ItemOrder", 7 }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetDiscountNotesSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // The 10 exemption notes of the v4 design.
            list.Add(new Dictionary<string, object> {
                { "Title", "يطبق التخفيض الأعلى في حال استحقاق المستفيد لأكثر من تخفيض." },
                { "Title_EN", "" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "يطبق الإعفاء والتخفيض على السعوديين وغير السعوديين." },
                { "Title_EN", "" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "لا يطبق الخصم على الحاصلين على منح التميز." },
                { "Title_EN", "" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "لا يطبق التخفيض على رسوم المقررات التكميلية." },
                { "Title_EN", "" },
                { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "يطبق الإعفاء والتخفيض على البرامج المحلية فقط." },
                { "Title_EN", "" },
                { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "لا يطبق التخفيض على المستفيدين من برنامج تقسيط التعليم في حال السداد المبكر لإجمالي الرسوم الدراسية." },
                { "Title_EN", "" },
                { "ItemOrder", 6 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "لا يطبق الإعفاء والتخفيض على بعض البرامج القائمة بالشراكات." },
                { "Title_EN", "" },
                { "ItemOrder", 7 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "يجب تقديم المستندات التي تثبت الحالة الصحية أو بطاقة الإعاقة لجميع الفئات." },
                { "Title_EN", "" },
                { "ItemOrder", 8 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "يجب تقديم شهادة أو خطاب من المركز السعودي لزراعة الأعضاء أو من المستشفى يفيد بأنه تم التبرع." },
                { "Title_EN", "" },
                { "ItemOrder", 9 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تُحسب ضريبة القيمة المضافة لغير السعوديين على الرسوم الدراسية بعد الخصم." },
                { "Title_EN", "" },
                { "ItemOrder", 10 }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetServicesSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // The 18 services and centers of the v4 design, grouped by Category.
            // Phase is left blank: applicants and enrolled students share the same set.
            list.Add(new Dictionary<string, object> {
                { "Title", "برنامج الباحثات الناشئات" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات البحثية" }, { "Category_EN", "" },
                { "Description", "الخدمات البحثية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/SR/Pages/Junior-Researchers-Program.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "برنامج نافع" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات البحثية" }, { "Category_EN", "" },
                { "Description", "الخدمات البحثية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/SR/Pages/Nafie-Program.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "برنامج المجموعات البحثية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات البحثية" }, { "Category_EN", "" },
                { "Description", "الخدمات البحثية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/SR/Pages/Research-Groups-Program.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "أخلاقيات البحث العلمي" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات البحثية" }, { "Category_EN", "" },
                { "Description", "الخدمات البحثية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/SR/Pages/SREthics_.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المكتبة المركزية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات البحثية" }, { "Category_EN", "" },
                { "Description", "الخدمات البحثية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/Research/Pages/LibraryServices.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "مركز الخدمات البحثية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات البحثية" }, { "Category_EN", "" },
                { "Description", "الخدمات البحثية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Centers/ResearchServicesCenter/Pages/home.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 6 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إضافة السير الذاتية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات التأهيلية لسوق العمل" }, { "Category_EN", "" },
                { "Description", "الخدمات التأهيلية لسوق العمل" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=223" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 7 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "مركز الدعم الطلابي والمهني" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات التأهيلية لسوق العمل" }, { "Category_EN", "" },
                { "Description", "الخدمات التأهيلية لسوق العمل" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/home.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 8 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إدارة الدعم المهاري" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات التأهيلية لسوق العمل" }, { "Category_EN", "" },
                { "Description", "الخدمات التأهيلية لسوق العمل" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/55.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 9 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الأنشطة اللامنهجية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات التأهيلية لسوق العمل" }, { "Category_EN", "" },
                { "Description", "الخدمات التأهيلية لسوق العمل" }, { "Description_EN", "" },
                { "LinkUrl", "https://graduates.pnu.edu.sa/ar/Pages/NonApproachActivities.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 10 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "برنامج سموق" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات التأهيلية لسوق العمل" }, { "Category_EN", "" },
                { "Description", "الخدمات التأهيلية لسوق العمل" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Centers/StudentAndCareerSupportCenter/Pages/33.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 11 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "بوابة الخريجات" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات التأهيلية لسوق العمل" }, { "Category_EN", "" },
                { "Description", "الخدمات التأهيلية لسوق العمل" }, { "Description_EN", "" },
                { "LinkUrl", "https://graduates.pnu.edu.sa/ar/pages/default.aspx#about" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 12 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "وكالة الخدمات الطلابية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات الطلابية" }, { "Category_EN", "" },
                { "Description", "الخدمات الطلابية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/studaffairs/AgencyStudentServices/Pages/home.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 13 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "وكالة الحياة الطلابية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات الطلابية" }, { "Category_EN", "" },
                { "Description", "الخدمات الطلابية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/studaffairs/Student%20Life%20Agency/Pages/home.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 14 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "حماية حقوق الطالبات" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات الطلابية" }, { "Category_EN", "" },
                { "Description", "الخدمات الطلابية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Faculties/Ap/Pages/Student-Rights-Protection.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 15 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "وحدة مساندة الطالبات ذوي الإعاقة" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات الطلابية" }, { "Category_EN", "" },
                { "Description", "الخدمات الطلابية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/studaffairs/AgencyStudentServices/Pages/StudentswithDisabilitiesUnit.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 16 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "دليل تمكين" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات الطلابية" }, { "Category_EN", "" },
                { "Description", "الخدمات الطلابية" }, { "Description_EN", "" },
                { "LinkUrl", "https://drive.google.com/file/d/1C3BgZxt_eMMyCqXJ5G46-h5JhQp08NIJ/view?usp=drive_link" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 17 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "المراكز الترفيهية" }, { "Title_EN", "" },
                { "Phase", "" },
                { "Category", "الخدمات الطلابية" }, { "Category_EN", "" },
                { "Description", "الخدمات الطلابية" }, { "Description_EN", "" },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/studaffairs/Student%20Life%20Agency/Pages/RecreationCenter.aspx" },
                { "IconClass", "hgi-building-03" },
                { "ItemOrder", 18 }
            });

            return list;
        }

        private static List<Dictionary<string, object>> GetQuickLinksSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Housing
            list.Add(new Dictionary<string, object> {
                { "Title", "إسكان طالبات جامعة الأميرة نورة" },
                { "Title_EN", "PNU Female Student Housing" },
                { "Phase", "" },
                { "SectionKey", "Housing" },
                { "Description", "يوفّر السكن الجامعي بيئةً سكنيةً متكاملةً وآمنةً لطالبات الدراسات العليا القادمات من خارج مدينة الرياض أو المبتعثات، مجهّزةً بالخدمات المعيشية والترفيهية ووسائل النقل داخل الحرم الجامعي." },
                { "Description_EN", "Student housing provides a secure, fully equipped living environment with recreational facilities and campus transport." },
                { "LinkUrl", "https://pnu.edu.sa/ar/Deanship/StudentAffairs/Housing/Pages/home.aspx" },
                { "LinkText", "عرض تفاصيل السكن وشروط التقديم" },
                { "LinkText_EN", "View Housing Details & Application Criteria" },
                { "IconClass", "hgi-home-01" },
                { "ItemOrder", 1 }
            });

            // Contact
            list.Add(new Dictionary<string, object> {
                { "Title", "وكالة الشؤون الأكاديمية — عمادة الدراسات العليا" },
                { "Title_EN", "Academic Affairs Agency — Deanship of Graduate Studies" },
                { "Phase", "" },
                { "SectionKey", "Contact" },
                { "Description", "البريد الإلكتروني المعتمد للاستفسارات الأكاديمية والقبول ومتابعة الطلبات: dgs-aa@pnu.edu.sa" },
                { "Description_EN", "Official email for academic inquiries, admissions and application follow-up: dgs-aa@pnu.edu.sa" },
                { "LinkUrl", "mailto:dgs-aa@pnu.edu.sa" },
                { "LinkText", "إرسال بريد إلكتروني" },
                { "LinkText_EN", "Send Email" },
                { "IconClass", "hgi-mail-01" },
                { "ItemOrder", 1 }
            });

            return list;
        }
    }
}
