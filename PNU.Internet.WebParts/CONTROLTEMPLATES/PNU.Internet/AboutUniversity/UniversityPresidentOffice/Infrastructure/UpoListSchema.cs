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
        private static UpoFieldDef DescAr()    { return new UpoFieldDef("Description", "المحتوى (عربي)", "Content (AR)", SPFieldType.Note, false, 10); }
        private static UpoFieldDef DescEn()    { return new UpoFieldDef("Description_EN", "المحتوى (إنجليزي)", "Content (EN)", SPFieldType.Note, false, 10); }
        private static UpoFieldDef Icon()      { return new UpoFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, false, 0, "hgi-quote-down, hgi-star ..."); }
        private static UpoFieldDef Order()     { return new UpoFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static UpoFieldDef ImageUrl()  { return new UpoFieldDef("ImageUrl", "رابط الصورة", "Image URL", SPFieldType.URL, false, 0, "public/images/pnu-president-photo.webp"); }

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
                    Row("Title","كلمة رئيسة الجامعة","Title_EN","University President's Speech",
                        "Description","بسم الله الرحمن الرحيم، والصلاة والسلام على نبينا محمد، وعلى آله وصحبه أجمعين.\r\n\r\nتواصل جامعة الأميرة نورة بنت عبدالرحمن مسيرتها العلمية والتنموية بوصفها صرحًا وطنيًا متفردًا في التعليم الجامعي، وشاهدًا على ما حققته المرأة السعودية من حضور مؤثر وإسهام فاعل في نهضة الوطن.\r\n\r\nوقد رسخت الجامعة عبر مسيرتها مكانة تستند إلى إرث عريق، وإمكانات علمية وبحثية ومؤسسية، ومجتمع جامعي يجمعه الطموح والشعور بالمسؤولية، وننطلق من هذه المكانة نحو مرحلة تتطلب منا تعزيز مكتسباتنا، وتجديد ممارساتنا، والارتقاء بأثرنا؛ بما يرسخ حضور الجامعة مؤسسة وطنية رائدة، تنتج المعرفة، وتعمل بفاعلية في خدمة المجتمع، وتحقيق أولويات الوطن ومستهدفات رؤيته الطموحة.\r\n\r\nوتولي الجامعة جودة التعليم ومخرجاته أولوية رئيسة، من خلال تطوير البرامج الأكاديمية، وتعزيز ارتباطها بالاحتياجات التنموية والمهنية، وتنمية المعارف والمهارات التي تمكّن طالباتنا من المنافسة والقيادة، وتأهيلهن للإسهام بفاعلية في تحقيق تطلعات الوطن.\r\n\r\nومع ما يشهده العالم من تحولات معرفية وتقنية واقتصادية متسارعة، نواصل تطوير التجربة التعليمية، وربط المعرفة بالتطبيق، وتوسيع فرص التعلم والممارسة؛ بما يعزز جاهزية خريجاتنا، ويدعم قدرتهن على مواصلة التعلم والتكيف وصناعة الفرص.\r\n\r\nكما تولي الجامعة البحث والابتكار اهتمامًا محوريًا، انطلاقًا من مسؤوليتهما الوطنية تجاه الإنسان والمجتمع. ومن هنا، نوجّه قدراتنا البحثية نحو الأولويات الوطنية، ونعتز بما بنته الجامعة من شراكات فاعلة مع القطاعات الوطنية والمؤسسات العلمية العالمية، وما أتاحته من تكامل للخبرات وتبادل للمعرفة، ونتطلع إلى توسيع هذه الشراكات وتعميق أثرها؛ بما يسهم في تحويل الأفكار إلى حلول مبتكرة، ويعزز إسهام الجامعة في التنمية محليًا ودوليًا.\r\n\r\nوتستند قدرة الجامعة على أداء رسالتها إلى بنية تحتية متكاملة، تضم مرافق تعليمية وبحثية وصحية وثقافية ورياضية، تدعمها منظومات تقنية وخدمات متقدمة، وتوظف الجامعة هذه الإمكانات في تطوير التجربة التعليمية والبحثية، وتعزيز جودة الحياة الجامعية، وتوفير بيئة محفزة تمكّن أفراد مجتمعها من التعلم والعمل والإبداع بكفاءة. ويظل الإنسان محور اهتمامنا وأساس رسالتنا؛ لذلك نحرص على بناء بيئة جامعية متكاملة تدعم صحة أفراد مجتمع الجامعة ورفاههم، وتعزز الانتماء والثقة والتكامل، وتتيح لكل فرد أن ينمي قدراته، ويؤدي دوره، ويسهم بفاعلية في تحقيق رسالة الجامعة، ضمن بيئة آمنة ومحفزة تقدر التنوع وتدعم الطموح.\r\n\r\nإن مسؤوليتنا لا تقتصر على مواكبة المستقبل، بل تمتد إلى المشاركة في صناعته، وبجهود مجتمعنا الجامعي وشركائنا، ستواصل جامعة الأميرة نورة بنت عبدالرحمن بناء القدرات بالمعرفة، وتوسيع الفرص، وصناعة أثر يعبّر عن طموح الجامعة، ويواكب تطلعات الوطن.\r\n\r\nنسأل الله أن يبارك مسيرة الجامعة، وأن يوفقنا لأداء رسالتنا وخدمة وطننا، ومواصلة الإسهام في نهضته، وأن يحفظ وطننا وقيادته، ويديم عليه التقدم والازدهار.\r\n\r\nوالله ولي التوفيق.",
                        "Description_EN","In the name of Allah, the Most Gracious, the Most Merciful, and peace and blessings be upon our Prophet Muhammad, and upon his family and companions.\r\n\r\nPrincess Nourah bint Abdulrahman University continues its scientific and developmental journey as a unique national landmark in higher education, and a witness to the impactful presence and active contribution of Saudi women in the nation's renaissance.",
                        "IconClass","hgi-quote-down","ShowLeadCard","1","ItemOrder","1")
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
                    Row("Title","تواصل نورة","Title_EN","Tawasul Nourah","ContactValue","pnu-tawasul@pnu.edu.sa","ContactType","email","ItemOrder","2"),
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
                    Row("Title","د نجلاء بنت عبدالله التويجري",
                        "Title_EN","Dr. Najla bint Abdullah Al-Tuwaijri",
                        "Description","رئيسة جامعة الأميرة نورة بنت عبدالرحمن",
                        "Description_EN","President of Princess Nourah bint Abdulrahman University",
                        "ImageUrl","public/images/pnu-president-photo.webp, د نجلاء بنت عبدالله التويجري، رئيسة جامعة الأميرة نورة بنت عبدالرحمن",
                        "ItemOrder","1")
                }
            };

            return map;
        }
    }
}
