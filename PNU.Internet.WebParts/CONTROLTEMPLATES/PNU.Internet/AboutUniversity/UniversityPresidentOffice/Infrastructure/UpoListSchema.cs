using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    public class UpoFieldDef
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

        public UpoFieldDef(string name, string ar, string en, SPFieldType type,
                           bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return UpoHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class UpoListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<UpoFieldDef> Fields { get; set; }
        /// <summary>Rows inserted when the list is created.</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return UpoHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for every University President Office list: used by the
    /// provisioner to create the lists and by ucUpoAdmin to render the CRUD form.
    /// </summary>
    public static class UpoListSchema
    {
        private static Dictionary<string, UpoListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, UpoListDef> All
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

        public static UpoListDef Get(string listName)
        {
            UpoListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        // ---- shared field builders -------------------------------------------------
        private static UpoFieldDef TitleEn()   { return new UpoFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static UpoFieldDef DescAr()    { return new UpoFieldDef("Description", "المحتوى (عربي)", "Content (AR)", SPFieldType.Note, false, 6); }
        private static UpoFieldDef DescEn()    { return new UpoFieldDef("Description_EN", "المحتوى (إنجليزي)", "Content (EN)", SPFieldType.Note, false, 6); }
        private static UpoFieldDef Icon()      { return new UpoFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, false, 0, "hgi-quote-down, hgi-star ..."); }
        private static UpoFieldDef Order()     { return new UpoFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static UpoFieldDef ImageUrl()  { return new UpoFieldDef("ImageUrl", "رابط الصورة", "Image URL", SPFieldType.URL, false, 0, "/Style Library/DGA/images/..."); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, UpoListDef> Build()
        {
            var map = new Dictionary<string, UpoListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) أقسام المحتوى — Content sections
            map[UpoListNames.Sections] = new UpoListDef
            {
                Name = UpoListNames.Sections,
                TitleAr = "أقسام كلمة رئيسة الجامعة", TitleEn = "President Office sections",
                Description = "Content sections on the University President Office page.",
                Fields = new List<UpoFieldDef>
                {
                    TitleEn(),
                    DescAr(), DescEn(),
                    new UpoFieldDef("LeadText", "مقدمة القسم (عربي)", "Lead text (AR)", SPFieldType.Note, false, 3),
                    new UpoFieldDef("LeadText_EN", "مقدمة القسم (إنجليزي)", "Lead text (EN)", SPFieldType.Note, false, 3),
                    Icon(),
                    new UpoFieldDef("ShowLeadCard", "إظهار البطاقة التعريفية", "Show intro card", SPFieldType.Boolean, true),
                    ImageUrl(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الترحيب والرؤية","Title_EN","Welcome and Vision",
                        "Description","ترتكز الرسالة على جعل المعرفة أساسًا للتنمية والابتكار، وربط مسار الجامعة بمستهدفات رؤية السعودية 2030 التي تمنح التعليم والبحث دورًا محوريًا في تقدم المجتمع وتعزيز حضوره محليًا وعالميًا.",
                        "Description_EN","The message is based on making knowledge the foundation for development and innovation, linking the university's path with Saudi Vision 2030 targets.",
                        "LeadText","ترحب رئيسة الجامعة بزوار البوابة الرقمية، وتؤكد أن جامعة الأميرة نورة تعمل على تطوير بيئة أكاديمية وبحثية وابتكارية متجددة.",
                        "LeadText_EN","The university president welcomes visitors to the digital portal, affirming that PNU works on developing a renewed academic, research and innovation environment.",
                        "IconClass","hgi-quote-down","ShowLeadCard","1","ItemOrder","1"),

                    Row("Title","تمكين المرأة وريادتها","Title_EN","Women Empowerment and Leadership",
                        "Description","تضع الكلمة تمكين المرأة في صميم هوية الجامعة، بوصفها إحدى أبرز الجامعات النسائية عالميًا، وتربط هذا الدور بمنظومة ريادة المرأة داخل الجامعة.\r\nوتشير إلى أن التجربة الأكاديمية في الجامعة تستهدف إعداد قيادات نسائية قادرة على الجمع بين المعرفة والمهارة، والمشاركة الفاعلة في التنمية وخدمة المجتمع والمنافسة على المستوى العالمي.",
                        "Description_EN","The message places women's empowerment at the core of the university's identity, as one of the most prominent women's universities globally.",
                        "ShowLeadCard","0","ItemOrder","2"),

                    Row("Title","البحث والابتكار وصناعة المستقبل","Title_EN","Research, Innovation and Building the Future",
                        "Description","تؤكد الرسالة عناية الجامعة بالبحث العلمي والابتكار والتعلُّم المستمر، من خلال توظيف التقنية وتطوير البرامج والمراكز التي تدعم الإنتاج البحثي والمعرفي.\r\nكما تبرز توجه الجامعة إلى تحسين جودة المخرجات، والاستفادة من أدوات الذكاء الاصطناعي في التعليم والبحث، بما يعزز حضورها الأكاديمي ويدعم صناعة المستقبل.",
                        "Description_EN","The message affirms the university's care for scientific research, innovation and continuous learning through technology and development.",
                        "ShowLeadCard","0","ItemOrder","3"),

                    Row("Title","الالتزام والشراكات","Title_EN","Commitment and Partnerships",
                        "Description","تُعبر الكلمة عن تقدير الجامعة لدعم القيادة الرشيدة لقطاع التعليم في المملكة، وما يمثله ذلك من دافع لاستمرار التميز وتوسيع الشراكات داخل المملكة وخارجها.\r\nوتختم الرسالة بتأكيد المسؤولية المهنية والوطنية والإنسانية للجامعة، ودورها في إعداد الكفاءات الوطنية وتعزيز أثرها المعرفي في التنمية المستدامة.",
                        "Description_EN","The message expresses appreciation for the wise leadership's support of education and its motivation for continued excellence and expanding partnerships.",
                        "ShowLeadCard","0","ItemOrder","4")
                }
            };

            // 2) بيانات التواصل — Contact details
            map[UpoListNames.Contacts] = new UpoListDef
            {
                Name = UpoListNames.Contacts,
                TitleAr = "بيانات التواصل", TitleEn = "Contact details",
                Description = "Contact information for the President's office.",
                Fields = new List<UpoFieldDef>
                {
                    TitleEn(),
                    new UpoFieldDef("ContactValue", "قيمة التواصل", "Contact value", SPFieldType.Text, true),
                    new UpoFieldDef("ContactType", "نوع التواصل", "Contact type", SPFieldType.Choice, true, 0,
                                   "email / phone / link", new string[] { "email", "phone", "link" }),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","البريد الإلكتروني","Title_EN","Email","ContactValue","rector@pnu.edu.sa","ContactType","email","ItemOrder","1"),
                    Row("Title","تواصل نورة","Title_EN","Tawasul Noura","ContactValue","pnu-tawasul@pnu.edu.sa","ContactType","email","ItemOrder","2"),
                    Row("Title","الرقم المباشر","Title_EN","Direct number","ContactValue","0118249111","ContactType","phone","ItemOrder","3"),
                    Row("Title","الرقم المباشر","Title_EN","Direct number","ContactValue","0118242437","ContactType","phone","ItemOrder","4"),
                    Row("Title","الرقم المباشر","Title_EN","Direct number","ContactValue","0118241461","ContactType","phone","ItemOrder","5"),
                    Row("Title","الرقم المباشر","Title_EN","Direct number","ContactValue","0118243612","ContactType","phone","ItemOrder","6"),
                    Row("Title","الرقم المباشر","Title_EN","Direct number","ContactValue","0118241893","ContactType","phone","ItemOrder","7"),
                    Row("Title","الرقم المباشر باللغة الإنجليزية","Title_EN","Direct number (English)","ContactValue","0118242615","ContactType","phone","ItemOrder","8"),
                    Row("Title","نظام تواصل نورة","Title_EN","Noura Communication System","ContactValue","https://tawasulnourah.pnu.edu.sa/","ContactType","link","ItemOrder","9")
                }
            };

            // 3) بطاقة رئيسة الجامعة — President signature card
            map[UpoListNames.Signature] = new UpoListDef
            {
                Name = UpoListNames.Signature,
                TitleAr = "بطاقة رئيسة الجامعة", TitleEn = "President signature card",
                Description = "President name, title and portrait.",
                Fields = new List<UpoFieldDef>
                {
                    TitleEn(),
                    DescAr(), DescEn(),
                    ImageUrl(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الدكتورة فوزية بنت سليمان العمرو",
                        "Title_EN","Dr. Fawziah bint Sulaiman Al-Amro",
                        "Description","رئيسة جامعة الأميرة نورة بنت عبدالرحمن المُكلَّفة",
                        "Description_EN","Acting President of Princess Nourah bint Abdulrahman University",
                        "ImageUrl","public/images/placeholder-portrait.svg, رئيسة الجامعة",
                        "ItemOrder","1")
                }
            };

            return map;
        }
    }
}
