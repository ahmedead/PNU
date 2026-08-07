using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    public class AgFieldDef
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

        public AgFieldDef(string name, string ar, string en, SPFieldType type,
                          bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return AgHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class AgListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<AgFieldDef> Fields { get; set; }
        /// <summary>Rows inserted when the list is created.</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return AgHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for every Agency list: used by the provisioner to
    /// create the lists and by ucAgAdmin to render the CRUD form.
    /// Title always holds the Arabic title of the item (built-in field, re-labelled).
    /// </summary>
    public static class AgListSchema
    {
        /// <summary>Accordion groups of the "المهام" section, in display order.</summary>
        public static readonly string[] TaskGroups = new string[]
        {
            "التخطيط", "الرقابة", "التنسيق", "تحديد المتطلبات", "إعداد التقارير"
        };

        private static readonly string[] TaskGroupsEn = new string[]
        {
            "Planning", "Oversight", "Coordination", "Requirements identification", "Reporting"
        };

        /// <summary>Bilingual display name of a task group (falls back to the raw value).</summary>
        public static string TaskGroupDisplay(string arabicKey)
        {
            if (string.IsNullOrEmpty(arabicKey)) return string.Empty;
            for (int i = 0; i < TaskGroups.Length; i++)
            {
                if (string.Equals(TaskGroups[i], arabicKey, StringComparison.OrdinalIgnoreCase))
                    return AgHelper.Pick(TaskGroups[i], TaskGroupsEn[i]);
            }
            return arabicKey;
        }

        private static Dictionary<string, AgListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, AgListDef> All
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

        public static AgListDef Get(string listName)
        {
            AgListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        // ---- shared field builders -------------------------------------------------
        private static AgFieldDef TitleEn()  { return new AgFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static AgFieldDef DescAr(int rows = 6) { return new AgFieldDef("Description", "النص (عربي)", "Body (AR)", SPFieldType.Note, false, rows); }
        private static AgFieldDef DescEn(int rows = 6) { return new AgFieldDef("Description_EN", "النص (إنجليزي)", "Body (EN)", SPFieldType.Note, false, rows); }
        private static AgFieldDef Icon()     { return new AgFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, true, 0, "hgi-target-02, hgi-message-01, hgi-quote-down ..."); }
        private static AgFieldDef Order()    { return new AgFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static AgFieldDef LinkUrl()  { return new AgFieldDef("LinkUrl", "الرابط", "Link URL", SPFieldType.URL); }
        private static AgFieldDef Column()   { return new AgFieldDef("ColumnClass", "عرض البطاقة (Bootstrap)", "Column class (Bootstrap)", SPFieldType.Text, false, 0, "col-12 col-md-6 col-sm-12 / col-12"); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, AgListDef> Build()
        {
            var map = new Dictionary<string, AgListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) نظرة عامة عن وكالة الجامعة  (عنصر واحد)
            map[AgListNames.Overview] = new AgListDef
            {
                Name = AgListNames.Overview,
                TitleAr = "نظرة عامة عن وكالة الجامعة", TitleEn = "Agency overview",
                Description = "Intro paragraph and hero image of the University Agency page. One item.",
                Fields = new List<AgFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(),
                    new AgFieldDef("ImageUrl", "صورة (شاشة صغيرة)", "Image (small)", SPFieldType.URL, false, 0, "/Style Library/DGA/images/hero/hero-campus-sm.webp"),
                    new AgFieldDef("ImageUrlMd", "صورة (شاشة كبيرة)", "Image (large)", SPFieldType.URL, false, 0, "/Style Library/DGA/images/hero/hero-campus-md.webp"),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","نظرة عامة عن وكالة الجامعة",
                        "Title_EN","About the University Agency",
                        "Description","تتولى وكالة الجامعة الإشراف على الشؤون الإدارية والمالية والوحدات المرتبطة بها، والتنسيق بينها بما يضمن التكامل والاستفادة من الإمكانيات المتاحة.",
                        "Description_EN","The University Agency oversees the administrative and financial affairs and their affiliated units, coordinating between them to ensure integration and the best use of available resources.",
                        "ImageUrl","/Style Library/DGA/images/hero/hero-campus-sm.webp, حرم جامعة الأميرة نورة",
                        "ImageUrlMd","/Style Library/DGA/images/hero/hero-campus-md.webp, حرم جامعة الأميرة نورة",
                        "ItemOrder","1")
                }
            };

            // 2) الرؤية والرسالة
            map[AgListNames.Cards] = new AgListDef
            {
                Name = AgListNames.Cards,
                TitleAr = "الرؤية والرسالة", TitleEn = "Vision and mission",
                Description = "Vision and mission cards of the University Agency.",
                Fields = new List<AgFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), Column(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الرؤية","Title_EN","Vision",
                        "Description","الريادة في تقديم منظومة إدارية ومالية مؤسسية كفؤة ومستدامة تدعم تميز الجامعة.",
                        "Description_EN","Leadership in delivering an efficient and sustainable institutional administrative and financial system that supports the University's excellence.",
                        "IconClass","hgi-target-02","ColumnClass","col-12 col-md-6 col-sm-12","ItemOrder","1"),
                    Row("Title","الرسالة","Title_EN","Mission",
                        "Description","تطوير وإدارة الموارد والخدمات الإدارية والمالية للجامعة وفق أفضل الممارسات، من خلال حوكمة فاعلة وكفاءات مؤهلة وتقنيات حديثة؛ بما يسهم في رفع جودة الأداء وتحقيق أهداف الجامعة.",
                        "Description_EN","Developing and managing the University's administrative and financial resources and services in line with best practices, through effective governance, qualified talent and modern technology, contributing to higher performance quality and the achievement of the University's goals.",
                        "IconClass","hgi-message-01","ColumnClass","col-12 col-md-6 col-sm-12","ItemOrder","2")
                }
            };

            // 3) الأهداف
            map[AgListNames.Objectives] = new AgListDef
            {
                Name = AgListNames.Objectives,
                TitleAr = "الأهداف", TitleEn = "Objectives",
                Description = "Bullet points of the objectives card.",
                Fields = new List<AgFieldDef> { TitleEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","رفع كفاءة التخطيط وإدارة الموارد المالية والإدارية.",
                        "Title_EN","Improving planning efficiency and the management of financial and administrative resources.",
                        "IconClass","hgi-target-02","ItemOrder","1"),
                    Row("Title","تعزيز الحوكمة والالتزام بالأنظمة واللوائح ورفع مستوى الشفافية.",
                        "Title_EN","Strengthening governance, regulatory compliance and transparency.",
                        "IconClass","hgi-target-02","ItemOrder","2"),
                    Row("Title","تطوير جودة الخدمات المقدمة للقطاعات الجامعية والمستفيدين.",
                        "Title_EN","Enhancing the quality of services provided to university sectors and beneficiaries.",
                        "IconClass","hgi-target-02","ItemOrder","3"),
                    Row("Title","دعم التحول الرقمي وتبسيط الإجراءات ورفع الكفاءة التشغيلية.",
                        "Title_EN","Supporting digital transformation, simplifying procedures and raising operational efficiency.",
                        "IconClass","hgi-target-02","ItemOrder","4"),
                    Row("Title","تنمية قدرات الكوادر وتعزيز بيئة عمل محفزة ومتكاملة.",
                        "Title_EN","Developing staff capabilities and fostering a motivating, integrated work environment.",
                        "IconClass","hgi-target-02","ItemOrder","5")
                }
            };

            // 4) كلمة الوكيلة  (عنصر واحد)
            map[AgListNames.DeputyWord] = new AgListDef
            {
                Name = AgListNames.DeputyWord,
                TitleAr = "كلمة الوكيلة", TitleEn = "Message from the Vice President",
                Description = "The Vice President's word. One item.",
                Fields = new List<AgFieldDef>
                {
                    TitleEn(), DescAr(10), DescEn(10),
                    new AgFieldDef("RoleText", "الصفة (عربي)", "Role (AR)", SPFieldType.Text, true),
                    new AgFieldDef("RoleText_EN", "الصفة (إنجليزي)", "Role (EN)", SPFieldType.Text),
                    new AgFieldDef("SubTitle", "الجهة (عربي)", "Organization (AR)", SPFieldType.Text),
                    new AgFieldDef("SubTitle_EN", "الجهة (إنجليزي)", "Organization (EN)", SPFieldType.Text),
                    Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","كلمة الوكيلة","Title_EN","Message from the Vice President",
                        "Description","تؤدي وكالة الجامعة دورًا محوريًا في دعم مسيرة الجامعة وتحقيق أهدافها الاستراتيجية، من خلال الإشراف على منظومة متكاملة من الأعمال الإدارية والمالية والخدمات المساندة. ونسعى إلى تطوير الإجراءات، ورفع كفاءة استخدام الموارد، وتعزيز الحوكمة والشفافية بما يضمن جودة الأداء واستدامته. كما نعمل بالشراكة مع قطاعات الجامعة المختلفة على بناء بيئة عمل مرنة ومحفزة، وتقديم خدمات موثوقة تلبي احتياجات المستفيدين وتسهم في الارتقاء بالتجربة الجامعية. ونؤمن بأن التكامل بين الكفاءات البشرية والتقنيات الحديثة هو الأساس لتحقيق التميز المؤسسي ودعم تطلعات الجامعة المستقبلية.",
                        "Description_EN","The University Agency plays a pivotal role in supporting the University's journey and achieving its strategic objectives by overseeing an integrated system of administrative, financial and support services. We work to develop procedures, improve resource efficiency and strengthen governance and transparency so that performance quality is sustained. In partnership with the University's sectors we build a flexible, motivating work environment and deliver reliable services that meet beneficiaries' needs and enrich the university experience. We believe that integrating human capability with modern technology is the foundation of institutional excellence.",
                        "RoleText","وكيلة الجامعة","RoleText_EN","Vice President of the University",
                        "SubTitle","جامعة الأميرة نورة بنت عبدالرحمن","SubTitle_EN","Princess Nourah bint Abdulrahman University",
                        "IconClass","hgi-quote-down","ItemOrder","1")
                }
            };

            // 5) المهام  (أكورديون مجمّع حسب TaskGroup)
            map[AgListNames.MainTasks] = new AgListDef
            {
                Name = AgListNames.MainTasks,
                TitleAr = "المهام", TitleEn = "Main tasks",
                Description = "Task bullets grouped into the accordion sections of the Agency page.",
                Fields = new List<AgFieldDef>
                {
                    TitleEn(),
                    new AgFieldDef("TaskGroup", "المجموعة", "Group", SPFieldType.Choice, true, 0,
                                   "التخطيط / الرقابة / التنسيق / تحديد المتطلبات / إعداد التقارير", TaskGroups),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    // التخطيط
                    Row("Title","إعداد الخطط السنوية والخمسية لنشاطات الإدارات ومتابعة تنفيذها بعد اعتمادها.",
                        "Title_EN","Preparing annual and five-year plans for departmental activities and following up on their implementation after approval.",
                        "TaskGroup","التخطيط","ItemOrder","1"),
                    Row("Title","إعداد البرامج للوحدات الإدارية المرتبطة بالوكالة ومتابعة تنفيذها بعد اعتمادها.",
                        "Title_EN","Preparing programs for the administrative units affiliated with the Agency and following up on their implementation after approval.",
                        "TaskGroup","التخطيط","ItemOrder","2"),

                    // الرقابة
                    Row("Title","الإشراف على تطبيق الأنظمة واللوائح والتعليمات المعتمدة في مجال الشؤون الإدارية والمالية في الجامعة.",
                        "Title_EN","Supervising the application of approved regulations, bylaws and instructions in the University's administrative and financial affairs.",
                        "TaskGroup","الرقابة","ItemOrder","1"),
                    Row("Title","الإشراف على الوحدات الإدارية المرتبطة بالوكالة.",
                        "Title_EN","Supervising the administrative units affiliated with the Agency.",
                        "TaskGroup","الرقابة","ItemOrder","2"),

                    // التنسيق
                    Row("Title","العمل على التنسيق بين الوحدات الإدارية المرتبطة بالوكالة بما يضمن التكامل بينها والاستفادة القصوى من الإمكانيات المتاحة لها.",
                        "Title_EN","Coordinating among the affiliated administrative units to ensure integration and the maximum use of their available resources.",
                        "TaskGroup","التنسيق","ItemOrder","1"),
                    Row("Title","العمل على التنسيق مع وكالات الجامعة في كل ما يتعلق بالخدمات الإدارية والمالية.",
                        "Title_EN","Coordinating with the University's other agencies on all matters relating to administrative and financial services.",
                        "TaskGroup","التنسيق","ItemOrder","2"),

                    // تحديد المتطلبات
                    Row("Title","حصر احتياجات الوكالة من القوى العاملة والأجهزة والمواد ومتابعة توفيرها.",
                        "Title_EN","Identifying the Agency's needs for manpower, equipment and materials and following up on their provision.",
                        "TaskGroup","تحديد المتطلبات","ItemOrder","1"),
                    Row("Title","حصر الاحتياجات التدريبية لموظفي وموظفات الوكالة والتنسيق مع عمادة التطوير والجودة بشأن الترشيح للبرامج التي تلبي تلك الاحتياجات.",
                        "Title_EN","Identifying the training needs of the Agency's staff and coordinating with the Deanship of Development and Quality on nominations for programs that meet those needs.",
                        "TaskGroup","تحديد المتطلبات","ItemOrder","2"),

                    // إعداد التقارير
                    Row("Title","الاشتراك في اللجان المتعلقة بنشاطات الوكالة وإعداد تقارير دورية عن نشاطات الوكالة وإنجازاتها ومقترحات تطويرها ورفعها لمديرة الجامعة.",
                        "Title_EN","Participating in committees related to the Agency's activities and preparing periodic reports on its activities, achievements and development proposals, and submitting them to the University President.",
                        "TaskGroup","إعداد التقارير","ItemOrder","1")
                }
            };

            return map;
        }
    }
}
