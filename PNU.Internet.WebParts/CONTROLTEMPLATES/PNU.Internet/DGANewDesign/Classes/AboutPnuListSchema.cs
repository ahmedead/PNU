using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes
{
    public class AboutPnuFieldDef
    {
        public string InternalName { get; set; }
        public string DisplayAr { get; set; }
        public string DisplayEn { get; set; }
        public SPFieldType Type { get; set; }
        /// <summary>Show this column in the admin grid.</summary>
        public bool InGrid { get; set; }
        /// <summary>Rows for Note fields in the admin form.</summary>
        public int Rows { get; set; }
        public string Hint { get; set; }
        /// <summary>Options for Choice fields.</summary>
        public string[] Choices { get; set; }

        public AboutPnuFieldDef(string name, string ar, string en, SPFieldType type,
                                bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return AboutPnuHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class AboutPnuListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<AboutPnuFieldDef> Fields { get; set; }
        /// <summary>Rows inserted when the list is created or reseeded.</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return AboutPnuHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for About PNU lists: used by the provisioner
    /// to create lists and by ucAboutPnuAdmin to render dynamic CRUD forms.
    /// </summary>
    public static class AboutPnuListSchema
    {
        private static Dictionary<string, AboutPnuListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, AboutPnuListDef> All
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

        public static AboutPnuListDef Get(string listName)
        {
            AboutPnuListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        // Shared field helper builders
        private static AboutPnuFieldDef TitleField()      { return new AboutPnuFieldDef("Title", "العنوان (عربي)", "Title (AR)", SPFieldType.Text, true); }
        private static AboutPnuFieldDef TitleEnField()    { return new AboutPnuFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static AboutPnuFieldDef SubtitleField()   { return new AboutPnuFieldDef("Subtitle", "العنوان الفرعي (عربي)", "Subtitle (AR)", SPFieldType.Text, false); }
        private static AboutPnuFieldDef SubtitleEnField() { return new AboutPnuFieldDef("Subtitle_EN", "العنوان الفرعي (إنجليزي)", "Subtitle (EN)", SPFieldType.Text, false); }
        private static AboutPnuFieldDef DescAr(int r = 6) { return new AboutPnuFieldDef("Description", "الوصف / المحتوى (عربي)", "Description / Content (AR)", SPFieldType.Note, false, r); }
        private static AboutPnuFieldDef DescEn(int r = 6) { return new AboutPnuFieldDef("Description_EN", "الوصف / المحتوى (إنجليزي)", "Description / Content (EN)", SPFieldType.Note, false, r); }
        private static AboutPnuFieldDef Icon()            { return new AboutPnuFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, true, 0, "hgi-target-01, hgi-hierarchy, hgi-chart, hgi-globe, hgi-star..."); }
        private static AboutPnuFieldDef Order()           { return new AboutPnuFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static AboutPnuFieldDef Visibility()      { return new AboutPnuFieldDef("Visibility", "ظاهر في الصفحة", "Visible", SPFieldType.Boolean, true); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, AboutPnuListDef> Build()
        {
            var map = new Dictionary<string, AboutPnuListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) AboutPnuOverview (العناوين العامة ونصوص الأقسام والصورة الرئيسية)
            map[AboutPnuListNames.Overview] = new AboutPnuListDef
            {
                Name = AboutPnuListNames.Overview,
                TitleAr = "ملخص وعناوين صفحة عن الجامعة",
                TitleEn = "About PNU Overview & Headers",
                Description = "General section titles, history narrative, and campus facility image for About PNU.",
                Fields = new List<AboutPnuFieldDef>
                {
                    TitleField(),
                    TitleEnField(),
                    SubtitleField(),
                    SubtitleEnField(),
                    DescAr(10),
                    DescEn(10),
                    new AboutPnuFieldDef("ImageUrl", "رابط الصورة", "Image URL", SPFieldType.URL, false, 0, "/style%20library/dga/public/images/hero/hero-library-lg.avif"),
                    new AboutPnuFieldDef("ImageAlt", "النص البديل للصورة (عربي)", "Image Alt (AR)", SPFieldType.Text, false),
                    new AboutPnuFieldDef("ImageAlt_EN", "النص البديل للصورة (إنجليزي)", "Image Alt (EN)", SPFieldType.Text, false),
                    new AboutPnuFieldDef("ImageCaption", "تعليق الصورة (عربي)", "Image Caption (AR)", SPFieldType.Text, false),
                    new AboutPnuFieldDef("ImageCaption_EN", "تعليق الصورة (إنجليزي)", "Image Caption (EN)", SPFieldType.Text, false),
                    Order(),
                    Visibility()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row(
                        "Title", "تاريخ الجامعة",
                        "Title_EN", "University History",
                        "Subtitle", "",
                        "Subtitle_EN", "",
                        "Description", "شهد تعليم المرأة في المملكة العربية السعودية اهتمامًا كبيرًا مكّنها من تحقيق إنجازات مميزة محليًا وعالميًا، حيث برزت نماذج نسائية رائدة في مختلف مجالات العلم والمعرفة.\n\nوتُعد جامعة الأميرة نورة بنت عبد الرحمن من أبرز ثمار هذا الاهتمام؛ إذ بدأت مسيرة تعليم المرأة مبكرًا بإنشاء أول كلية تربوية للبنات عام 1970م، ثم توسع التعليم ليشمل عشرات الكليات في مختلف مناطق المملكة. وفي عام 1427هـ صدر الأمر الملكي بإنشاء أول جامعة متكاملة للبنات في الرياض، وتم تفعيلها عام 1428هـ.\n\nوفي عام 1429هـ، وُضع حجر الأساس للمدينة الجامعية، ليُطلق عليها لاحقًا اسم \"جامعة الأميرة نورة بنت عبد الرحمن\"، تخليدًا لاسم شقيقة الملك عبد العزيز -رحمه الله-، لتصبح اليوم صرحًا علميًا رائدًا يعكس تمكين المرأة ودورها في التنمية.",
                        "Description_EN", "Women's education in Saudi Arabia has received tremendous attention, enabling women to achieve distinguished achievements locally and globally, bringing forth leading female models across fields of science and knowledge.\n\nPrincess Nourah bint Abdulrahman University stands as one of the prominent fruits of this attention. The journey began early with the establishment of the first women's educational college in 1970, expanding to dozens of colleges across the Kingdom. In 1427 AH, a royal decree was issued establishing the first comprehensive university for women in Riyadh, activated in 1428 AH.\n\nIn 1429 AH, the cornerstone was laid for the university campus, later named Princess Nourah bint Abdulrahman University, commemorating the sister of King Abdulaziz, to become a leading academic monument reflecting women's empowerment and role in development.",
                        "ImageUrl", "/style%20library/dga/public/images/hero/hero-library-lg.avif",
                        "ImageAlt", "واجهة من مرافق جامعة الأميرة نورة بنت عبد الرحمن",
                        "ImageAlt_EN", "Princess Nourah University campus facilities facade",
                        "ImageCaption", "مشهد من مرافق الجامعة يعكس اتساع الحرم الجامعي والطابع المعماري للمباني الرئيسة.",
                        "ImageCaption_EN", "A scene of university facilities reflecting the vast campus and main architectural design.",
                        "ItemOrder", "1",
                        "Visibility", "1"
                    ),
                    Row(
                        "Title", "مرتكزات الجامعة",
                        "Title_EN", "University Pillars",
                        "Subtitle", "بطاقات مختصرة تلخص المحاور الأساسية التي تتكرر في الصفحات المرجعية المرتبطة بالجامعة.",
                        "Subtitle_EN", "Brief cards summarizing core pillars recurring across university reference pages.",
                        "Description", "",
                        "Description_EN", "",
                        "ImageUrl", "",
                        "ImageAlt", "",
                        "ImageAlt_EN", "",
                        "ImageCaption", "",
                        "ImageCaption_EN", "",
                        "ItemOrder", "2",
                        "Visibility", "1"
                    )
                }
            };

            // 2) AboutPnuMilestones (محطات وتاريخ الجامعة)
            map[AboutPnuListNames.Milestones] = new AboutPnuListDef
            {
                Name = AboutPnuListNames.Milestones,
                TitleAr = "محطات تاريخ الجامعة",
                TitleEn = "University History Milestones",
                Description = "Milestone cards showing the historical highlights of PNU.",
                Fields = new List<AboutPnuFieldDef>
                {
                    TitleField(),
                    TitleEnField(),
                    DescAr(4),
                    DescEn(4),
                    new AboutPnuFieldDef("Period", "الفترة / التاريخ المعروض (عربي)", "Period / Date Display (AR)", SPFieldType.Text, true, 0, "1390 هـ / 1970 م"),
                    new AboutPnuFieldDef("Period_EN", "الفترة / التاريخ المعروض (إنجليزي)", "Period / Date Display (EN)", SPFieldType.Text, true, 0, "1390 AH / 1970 AD"),
                    new AboutPnuFieldDef("DateAttribute", "قيمة التاريخ البرمجية (datetime)", "Date Attribute (datetime)", SPFieldType.Text, false, 0, "1970"),
                    Icon(),
                    Order(),
                    Visibility()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row(
                        "Title", "البداية الأكاديمية",
                        "Title_EN", "Academic Inception",
                        "Description", "إنشاء أول كلية تربوية للبنات إيذانًا بانطلاقة المسار الأكاديمي المنظم لتعليم المرأة.",
                        "Description_EN", "Establishment of the first educational college for women, marking the launch of an organized academic path for women's education.",
                        "Period", "1390 هـ / 1970 م",
                        "Period_EN", "1390 AH / 1970 AD",
                        "DateAttribute", "1970",
                        "IconClass", "hgi hgi-stroke hgi-target-01",
                        "ItemOrder", "1",
                        "Visibility", "1"
                    ),
                    Row(
                        "Title", "تأسيس الجامعة",
                        "Title_EN", "University Establishment",
                        "Description", "صدور الأمر الملكي بإنشاء أول جامعة للبنات بالرياض تحت إشراف وزارة التعليم العالي.",
                        "Description_EN", "Issuance of the Royal Decree establishing the first women's university in Riyadh under the Ministry of Higher Education.",
                        "Period", "1427 هـ",
                        "Period_EN", "1427 AH",
                        "DateAttribute", "2006",
                        "IconClass", "hgi hgi-stroke hgi-hierarchy",
                        "ItemOrder", "2",
                        "Visibility", "1"
                    ),
                    Row(
                        "Title", "التفعيل والمدينة الجامعية",
                        "Title_EN", "Activation & University City",
                        "Description", "تفعيل الجامعة، ثم وضع حجر الأساس للمدينة الجامعية واعتماد اسمها الحالي.",
                        "Description_EN", "Activating the university, laying the foundation stone of the university city, and approving its current name.",
                        "Period", "1428 - 1429 هـ",
                        "Period_EN", "1428 - 1429 AH",
                        "DateAttribute", "2007/2008",
                        "IconClass", "hgi hgi-stroke hgi-chart",
                        "ItemOrder", "3",
                        "Visibility", "1"
                    )
                }
            };

            // 3) AboutPnuPillars (مرتكزات الجامعة: الرؤية، الرسالة، القيم...)
            map[AboutPnuListNames.Pillars] = new AboutPnuListDef
            {
                Name = AboutPnuListNames.Pillars,
                TitleAr = "مرتكزات الجامعة",
                TitleEn = "University Pillars",
                Description = "Core pillars of the university (Vision, Mission, Values).",
                Fields = new List<AboutPnuFieldDef>
                {
                    TitleField(),
                    TitleEnField(),
                    DescAr(4),
                    DescEn(4),
                    Icon(),
                    Order(),
                    Visibility()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row(
                        "Title", "الرؤية",
                        "Title_EN", "Vision",
                        "Description", "هوية أكاديمية ومعرفية تقود إلى أثر مؤسسي ومجتمعي أوسع.",
                        "Description_EN", "An academic and knowledge identity driving broader institutional and societal impact.",
                        "IconClass", "hgi hgi-stroke hgi-target-01",
                        "ItemOrder", "1",
                        "Visibility", "1"
                    ),
                    Row(
                        "Title", "الرسالة",
                        "Title_EN", "Mission",
                        "Description", "تجربة جامعية موثوقة تدعم التعليم والبحث والخدمة المجتمعية.",
                        "Description_EN", "A trusted university experience supporting education, research, and community service.",
                        "IconClass", "hgi hgi-stroke hgi-globe",
                        "ItemOrder", "2",
                        "Visibility", "1"
                    ),
                    Row(
                        "Title", "القيم",
                        "Title_EN", "Values",
                        "Description", "التميز والاعتزاز بالهوية والمسؤولية والتعاون والشفافية.",
                        "Description_EN", "Excellence, pride in identity, responsibility, collaboration, and transparency.",
                        "IconClass", "hgi hgi-stroke hgi-star",
                        "ItemOrder", "3",
                        "Visibility", "1"
                    )
                }
            };

            return map;
        }
    }
}
