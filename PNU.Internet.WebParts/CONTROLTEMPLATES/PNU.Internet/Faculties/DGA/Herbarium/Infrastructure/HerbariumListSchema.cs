using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public class HerbariumFieldDef
    {
        public string InternalName { get; set; }
        public string DisplayAr { get; set; }
        public string DisplayEn { get; set; }
        public SPFieldType Type { get; set; }
        public bool InGrid { get; set; }
        public int Rows { get; set; }
        public string Hint { get; set; }
        public string[] Choices { get; set; }

        public HerbariumFieldDef(string name, string ar, string en, SPFieldType type,
                                 bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return HerbariumHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class HerbariumListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<HerbariumFieldDef> Fields { get; set; }
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return HerbariumHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for every Herbarium list definition and initial seed data.
    /// Used by the provisioner and ucHerbariumAdmin CRUD control.
    /// </summary>
    public static class HerbariumListSchema
    {
        private static Dictionary<string, HerbariumListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, HerbariumListDef> All
        {
            get
            {
                if (_all == null)
                {
                    lock (_lock)
                    {
                        if (_all == null) _all = Build();
                    }
                }
                return _all;
            }
        }

        public static HerbariumListDef Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            HerbariumListDef def;
            return All.TryGetValue(name, out def) ? def : null;
        }

        private static Dictionary<string, HerbariumListDef> Build()
        {
            var dict = new Dictionary<string, HerbariumListDef>(StringComparer.OrdinalIgnoreCase);

            // 1. HerbariumHeader
            dict[HerbariumListNames.HerbariumHeader] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumHeader,
                TitleAr = "المعشبة - الهيدر والصورة",
                TitleEn = "Herbarium - Header & Image",
                Description = "عنوان ووصف المعشبة والصورة الرئيسية",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("SubTitle", "الرمز / SubTitle", "SubTitle (Arabic)", SPFieldType.Text, true),
                    new HerbariumFieldDef("SubTitle_EN", "SubTitle (إنجليزي)", "SubTitle (English)", SPFieldType.Text),
                    new HerbariumFieldDef("Description", "الوصف بالعربية", "Description (Arabic)", SPFieldType.Note, true, 3),
                    new HerbariumFieldDef("Description_EN", "الوصف بالإنجليزي", "Description (English)", SPFieldType.Note, false, 3),
                    new HerbariumFieldDef("ImageUrl", "رابط الصورة", "Image URL", SPFieldType.URL, true),
                    new HerbariumFieldDef("ImageAlt", "النص البديل للصورة", "Image Alt Text (Arabic)", SPFieldType.Text),
                    new HerbariumFieldDef("ImageAlt_EN", "النص البديل (إنجليزي)", "Image Alt Text (English)", SPFieldType.Text),
                    new HerbariumFieldDef("ImageCaption", "تعليق الصورة", "Image Caption (Arabic)", SPFieldType.Text, true),
                    new HerbariumFieldDef("ImageCaption_EN", "تعليق الصورة (إنجليزي)", "Image Caption (English)", SPFieldType.Text),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "Title", "المعشبة النباتية" },
                        { "Title_EN", "Plant Herbarium" },
                        { "SubTitle", "(PNUH)" },
                        { "SubTitle_EN", "(PNUH)" },
                        { "Description", "تُعد معشبة جامعة الأميرة نورة بنت عبدالرحمن (PNUH) مرجعًا علميًا لتوثيق النباتات المحلية وحفظ العينات النباتية ودعم البحث العلمي والتوعية البيئية." },
                        { "Description_EN", "Princess Nourah Bint Abdulrahman University Herbarium (PNUH) serves as a scientific reference for documenting local flora, preserving plant specimens, supporting scientific research, and promoting environmental awareness." },
                        { "ImageUrl", "https://taffy-modify-59445061.figma.site/_components/v2/6cc9e692147db66a673f8f1e07b75c437d197f83/image-9.5a9231f5.png" },
                        { "ImageAlt", "مقتنيات وخزائن حفظ العينات في معشبة جامعة الأميرة نورة" },
                        { "ImageAlt_EN", "Specimen cabinets and collections in PNU Herbarium" },
                        { "ImageCaption", "معشبة جامعة الأميرة نورة بنت عبدالرحمن — كلية العلوم." },
                        { "ImageCaption_EN", "Princess Nourah bint Abdulrahman University Herbarium — College of Science." },
                        { "ItemOrder", "1" }
                    }
                }
            };

            // 2. HerbariumOverview
            dict[HerbariumListNames.HerbariumOverview] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumOverview,
                TitleAr = "المعشبة - نبذة عن المعشبة",
                TitleEn = "Herbarium - Overview",
                Description = "بطاقة نبذة عن المعشبة النباتية",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("Description", "النص (عربي)", "Description (Arabic)", SPFieldType.Note, true, 4),
                    new HerbariumFieldDef("Description_EN", "النص (إنجليزي)", "Description (English)", SPFieldType.Note, false, 4),
                    new HerbariumFieldDef("IconClass", "فئة الأيقونة", "Icon Class", SPFieldType.Text, true),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "Title", "نبذة عن المعشبة" },
                        { "Title_EN", "About the Herbarium" },
                        { "Description", "تضم المعشبة عينات نباتية مجففة ومضغوطة وموثقة ببياناتها العلمية، لتكون سجلًا مرجعيًا يساعد الباحثات والطالبات في التعرف على النباتات وتصنيفها ودراسة التنوع النباتي في المملكة." },
                        { "Description_EN", "The herbarium contains dried, pressed, and scientifically documented plant specimens, serving as a reference record that helps researchers and students identify, classify, and study plant diversity in the Kingdom." },
                        { "IconClass", "hgi hgi-stroke hgi-leaf-01 fs-3" },
                        { "ItemOrder", "1" }
                    }
                }
            };

            // 3. HerbariumStats
            dict[HerbariumListNames.HerbariumStats] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumStats,
                TitleAr = "المعشبة - الإحصائيات والأرقام",
                TitleEn = "Herbarium - Statistics",
                Description = "أرقام وإحصائيات المعشبة النباتية",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("StatValue", "القيمة / الرقم", "Stat Value", SPFieldType.Text, true),
                    new HerbariumFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Title", "عينة نباتية" }, { "Title_EN", "Plant Specimen" }, { "StatValue", "+7,000" }, { "ItemOrder", "1" } },
                    new Dictionary<string, string> { { "Title", "نوعًا نباتيًا" }, { "Title_EN", "Plant Species" }, { "StatValue", "318" }, { "ItemOrder", "2" } },
                    new Dictionary<string, string> { { "Title", "جنسًا نباتيًا" }, { "Title_EN", "Plant Genera" }, { "StatValue", "198" }, { "ItemOrder", "3" } },
                    new Dictionary<string, string> { { "Title", "فصيلة نباتية" }, { "Title_EN", "Plant Families" }, { "StatValue", "63" }, { "ItemOrder", "4" } }
                }
            };

            // 4. HerbariumMission
            dict[HerbariumListNames.HerbariumMission] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumMission,
                TitleAr = "المعشبة - الرسالة",
                TitleEn = "Herbarium - Mission",
                Description = "بطاقة الرسالة للمعشبة النباتية",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("Description", "نص الرسالة (عربي)", "Description (Arabic)", SPFieldType.Note, true, 3),
                    new HerbariumFieldDef("Description_EN", "نص الرسالة (إنجليزي)", "Description (English)", SPFieldType.Note, false, 3),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "Title", "الرسالة" },
                        { "Title_EN", "Mission" },
                        { "Description", "جمع وحفظ وتوثيق المجاميع النباتية في الفلورا السعودية، وتصنيفها علميًا بما يخدم البيئة والمجتمع والبحث العلمي." },
                        { "Description_EN", "Collecting, preserving, and documenting plant collections in the Saudi flora, and classifying them scientifically to serve the environment, society, and scientific research." },
                        { "ItemOrder", "1" }
                    }
                }
            };

            // 5. HerbariumObjectives
            dict[HerbariumListNames.HerbariumObjectives] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumObjectives,
                TitleAr = "المعشبة - الأهداف",
                TitleEn = "Herbarium - Objectives",
                Description = "أهداف المعشبة (قائمة الأكورديون)",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("Title_EN", "عنوان الهدف (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("Description", "تفاصيل الهدف (عربي)", "Description (Arabic)", SPFieldType.Note, true, 3),
                    new HerbariumFieldDef("Description_EN", "تفاصيل الهدف (إنجليزي)", "Description (English)", SPFieldType.Note, false, 3),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "Title", "حفظ وتوثيق النباتات" },
                        { "Title_EN", "Preservation and Documentation" },
                        { "Description", "توثيق الأنواع النباتية البرية والطبية والمزروعة وحفظ بياناتها العلمية بصورة منظمة." },
                        { "Description_EN", "Documenting wild, medicinal, and cultivated plant species and systematically preserving their scientific data." },
                        { "ItemOrder", "1" }
                    },
                    new Dictionary<string, string>
                    {
                        { "Title", "دعم البحث العلمي" },
                        { "Title_EN", "Supporting Scientific Research" },
                        { "Description", "إتاحة العينات والمعلومات المصنفة للباحثات والدارسات ودعم الدراسات البيئية والتصنيفية." },
                        { "Description_EN", "Providing classified specimens and data to researchers and students, supporting environmental and taxonomic studies." },
                        { "ItemOrder", "2" }
                    },
                    new Dictionary<string, string>
                    {
                        { "Title", "التدريب والتوعية" },
                        { "Title_EN", "Training and Awareness" },
                        { "Description", "تنمية الوعي بالتنوع النباتي من خلال الزيارات والدورات والأنشطة العلمية والمجتمعية." },
                        { "Description_EN", "Promoting awareness of plant biodiversity through visits, workshops, scientific, and community activities." },
                        { "ItemOrder", "3" }
                    },
                    new Dictionary<string, string>
                    {
                        { "Title", "الشراكات وقاعدة البيانات" },
                        { "Title_EN", "Partnerships and Database" },
                        { "Description", "تعزيز التعاون مع المؤسسات العلمية وتطوير قاعدة بيانات تدعم التوثيق والنشر العلمي." },
                        { "Description_EN", "Enhancing collaboration with scientific institutions and developing a database to support documentation and publishing." },
                        { "ItemOrder", "4" }
                    }
                }
            };

            // 6. HerbariumServices
            dict[HerbariumListNames.HerbariumServices] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumServices,
                TitleAr = "المعشبة - ما تقدمه المعشبة",
                TitleEn = "Herbarium - Services",
                Description = "خدمات وما تقدمه المعشبة النباتية",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("Title_EN", "عنوان الخدمة (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("Description", "وصف الخدمة (عربي)", "Description (Arabic)", SPFieldType.Note, true, 3),
                    new HerbariumFieldDef("Description_EN", "وصف الخدمة (إنجليزي)", "Description (English)", SPFieldType.Note, false, 3),
                    new HerbariumFieldDef("IconClass", "فئة الأيقونة", "Icon Class", SPFieldType.Text, true),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "Title", "التعريف والتصنيف" },
                        { "Title_EN", "Identification and Classification" },
                        { "Description", "المساعدة في التعرف العلمي على العينات النباتية وتصنيفها." },
                        { "Description_EN", "Assisting in the scientific identification and classification of plant specimens." },
                        { "IconClass", "hgi hgi-stroke hgi-plant-01 fs-3" },
                        { "ItemOrder", "1" }
                    },
                    new Dictionary<string, string>
                    {
                        { "Title", "خدمة الباحثات" },
                        { "Title_EN", "Researcher Support" },
                        { "Description", "دعم المشروعات والدراسات المرتبطة بالنباتات والبيئة والتنوع الحيوي." },
                        { "Description_EN", "Supporting projects and studies related to plants, environment, and biodiversity." },
                        { "IconClass", "hgi hgi-stroke hgi-book-open-01 fs-3" },
                        { "ItemOrder", "2" }
                    },
                    new Dictionary<string, string>
                    {
                        { "Title", "الزيارات والتدريب" },
                        { "Title_EN", "Visits and Training" },
                        { "Description", "جولات تعريفية ودورات وفرص تطوعية لطالبات الجامعة والمهتمات." },
                        { "Description_EN", "Guided tours, training sessions, and volunteer opportunities for university students and interested individuals." },
                        { "IconClass", "hgi hgi-stroke hgi-user-group fs-3" },
                        { "ItemOrder", "3" }
                    }
                }
            };

            // 7. HerbariumMilestones
            dict[HerbariumListNames.HerbariumMilestones] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumMilestones,
                TitleAr = "المعشبة - محطات بارزة",
                TitleEn = "Herbarium - Milestones",
                Description = "محطات بارزة في تاريخ المعشبة",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("Description", "الوصف (عربي)", "Description (Arabic)", SPFieldType.Note, true, 3),
                    new HerbariumFieldDef("Description_EN", "الوصف (إنجليزي)", "Description (English)", SPFieldType.Note, false, 3),
                    new HerbariumFieldDef("BadgeText", "نص الشارة (2013 / PNUH)", "Badge Text", SPFieldType.Text, true),
                    new HerbariumFieldDef("BadgeClass", "نوع الشارة (badge-info / badge-success)", "Badge Class", SPFieldType.Text, true),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "Title", "الانطلاقة" },
                        { "Title_EN", "The Launch" },
                        { "Description", "بدأت المعشبة مبادرة تطوعية في قسم الأحياء بكلية العلوم." },
                        { "Description_EN", "The herbarium started as a volunteer initiative in the Biology Department at the College of Science." },
                        { "BadgeText", "2013" },
                        { "BadgeClass", "badge-info" },
                        { "ItemOrder", "1" }
                    },
                    new Dictionary<string, string>
                    {
                        { "Title", "الافتتاح الرسمي" },
                        { "Title_EN", "Official Opening" },
                        { "Description", "افتُتح مقر المعشبة رسميًا لخدمة الجامعة والباحثات والمجتمع." },
                        { "Description_EN", "The herbarium headquarters was officially inaugurated to serve the university, researchers, and society." },
                        { "BadgeText", "2018" },
                        { "BadgeClass", "badge-info" },
                        { "ItemOrder", "2" }
                    },
                    new Dictionary<string, string>
                    {
                        { "Title", "التسجيل الدولي" },
                        { "Title_EN", "International Registration" },
                        { "Description", "سُجلت المعشبة دوليًا ضمن قوائم المعاشب العالمية بالرمز PNUH." },
                        { "Description_EN", "The herbarium was internationally registered in Index Herbariorum under the code PNUH." },
                        { "BadgeText", "PNUH" },
                        { "BadgeClass", "badge-success" },
                        { "ItemOrder", "3" }
                    }
                }
            };

            // 8. HerbariumContact
            dict[HerbariumListNames.HerbariumContact] = new HerbariumListDef
            {
                Name = HerbariumListNames.HerbariumContact,
                TitleAr = "المعشبة - التواصل",
                TitleEn = "Herbarium - Contact Info",
                Description = "معلومات التواصل والبريد الإلكتروني والهاتف",
                Fields = new List<HerbariumFieldDef>
                {
                    new HerbariumFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (English)", SPFieldType.Text, true),
                    new HerbariumFieldDef("Description", "الوصف (عربي)", "Description (Arabic)", SPFieldType.Note, true, 3),
                    new HerbariumFieldDef("Description_EN", "الوصف (إنجليزي)", "Description (English)", SPFieldType.Note, false, 3),
                    new HerbariumFieldDef("Email", "البريد الإلكتروني", "Email", SPFieldType.Text, true),
                    new HerbariumFieldDef("Phone", "رقم الهاتف", "Phone", SPFieldType.Text, true),
                    new HerbariumFieldDef("IconClass", "فئة الأيقونة", "Icon Class", SPFieldType.Text),
                    new HerbariumFieldDef("ButtonText", "نص الزر (عربي)", "Button Text (Arabic)", SPFieldType.Text, true),
                    new HerbariumFieldDef("ButtonText_EN", "نص الزر (إنجليزي)", "Button Text (English)", SPFieldType.Text),
                    new HerbariumFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "Title", "للتعاون والزيارات العلمية" },
                        { "Title_EN", "For Collaboration and Scientific Visits" },
                        { "Description", "يمكن التواصل مع معشبة كلية العلوم للاستفسار عن العينات أو الزيارات أو فرص التعاون العلمي." },
                        { "Description_EN", "You can contact the College of Science Herbarium for inquiries regarding specimens, visits, or scientific collaboration opportunities." },
                        { "Email", "cs-herbarium@pnu.edu.sa" },
                        { "Phone", "0118235980" },
                        { "IconClass", "hgi hgi-stroke hgi-leaf-02 fs-3" },
                        { "ButtonText", "تواصل معنا" },
                        { "ButtonText_EN", "Contact Us" },
                        { "ItemOrder", "1" }
                    }
                }
            };

            return dict;
        }
    }
}
