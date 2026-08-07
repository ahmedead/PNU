using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public class NsFieldDef
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

        public NsFieldDef(string name, string ar, string en, SPFieldType type,
                          bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return NsHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class NsListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<NsFieldDef> Fields { get; set; }
        /// <summary>Rows inserted when the list is created.</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return NsHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for every Noura Students list: used by the provisioner to
    /// create the lists and by ucNsAdmin to render the CRUD form.
    /// Title always holds the Arabic title of the item (built-in field, re-labelled).
    /// </summary>
    public static class NsListSchema
    {
        /// <summary>
        /// Groups of the "تواريخ تهمك" section. The stored choice value is Arabic
        /// (that is what lives in the list), so the English label is kept beside it
        /// and resolved at render time - otherwise the English page shows Arabic
        /// column headings.
        /// </summary>
        public static readonly string[] DateCategories = new string[]
        {
            "مواعيد التسجيل", "الفصول الدراسية", "جدول الاختبارات"
        };

        private static readonly string[] DateCategoriesEn = new string[]
        {
            "Registration dates", "Semesters", "Exam schedule"
        };

        /// <summary>
        /// Localized heading for a stored category value. A value an editor added
        /// outside the schema is returned unchanged.
        /// </summary>
        public static string DateCategoryDisplay(string storedValue)
        {
            if (string.IsNullOrEmpty(storedValue)) return string.Empty;

            for (int i = 0; i < DateCategories.Length; i++)
            {
                if (string.Equals(DateCategories[i], storedValue, StringComparison.OrdinalIgnoreCase))
                    return NsHelper.Pick(DateCategories[i], DateCategoriesEn[i]);
            }

            return storedValue;
        }

        private static Dictionary<string, NsListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, NsListDef> All
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

        public static NsListDef Get(string listName)
        {
            NsListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        // ---- shared field builders -------------------------------------------------
        private static NsFieldDef TitleEn()   { return new NsFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static NsFieldDef DescAr()    { return new NsFieldDef("Description", "الوصف (عربي)", "Description (AR)", SPFieldType.Note, false, 3); }
        private static NsFieldDef DescEn()    { return new NsFieldDef("Description_EN", "الوصف (إنجليزي)", "Description (EN)", SPFieldType.Note, false, 3); }
        private static NsFieldDef Icon()      { return new NsFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, true, 0, "hgi-star, hgi-award-01, hgi-user-group ..."); }
        private static NsFieldDef Order()     { return new NsFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static NsFieldDef LinkUrl()   { return new NsFieldDef("LinkUrl", "الرابط", "Link URL", SPFieldType.URL); }
        private static NsFieldDef ImageUrl()  { return new NsFieldDef("ImageUrl", "رابط الصورة", "Image URL", SPFieldType.URL, false, 0, "/Style Library/DGA/images/..."); }
        private static NsFieldDef BtnAr()     { return new NsFieldDef("ButtonText", "نص الزر (عربي)", "Button text (AR)", SPFieldType.Text); }
        private static NsFieldDef BtnEn()     { return new NsFieldDef("ButtonText_EN", "نص الزر (إنجليزي)", "Button text (EN)", SPFieldType.Text); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, NsListDef> Build()
        {
            var map = new Dictionary<string, NsListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) طالبات نورة في أرقام
            map[NsListNames.Numbers] = new NsListDef
            {
                Name = NsListNames.Numbers,
                TitleAr = "طالبات نورة في أرقام", TitleEn = "Noura students in numbers",
                Description = "Statistic counters on the Noura Students page.",
                Fields = new List<NsFieldDef>
                {
                    TitleEn(),
                    new NsFieldDef("StatValue", "القيمة", "Value", SPFieldType.Text, true, 0, "900+, 7000+, 20+"),
                    Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الأنشطة الطلابية","Title_EN","Student activities","StatValue","900+","IconClass","hgi-user-group","ItemOrder","1"),
                    Row("Title","المبادرات الطلابية","Title_EN","Student initiatives","StatValue","7000+","IconClass","hgi-star","ItemOrder","2"),
                    Row("Title","المشروعات الطلابية","Title_EN","Student projects","StatValue","20+","IconClass","hgi-award-01","ItemOrder","3"),
                    Row("Title","الجوائز الطلابية","Title_EN","Student awards","StatValue","400+","IconClass","hgi-award-01","ItemOrder","4")
                }
            };

            // 2) لوحة الجوائز الطلابية
            map[NsListNames.Awards] = new NsListDef
            {
                Name = NsListNames.Awards,
                TitleAr = "لوحة الجوائز الطلابية", TitleEn = "Student awards board",
                Description = "Local, regional and international award figures.",
                Fields = new List<NsFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","محليًا","Title_EN","Locally","Description","310 جائزة محلية — تعزيزٌ على المستوى الوطني","Description_EN","310 local awards - recognition at national level","IconClass","hgi-award-01","ItemOrder","1"),
                    Row("Title","إقليميًا","Title_EN","Regionally","Description","35 جائزة إقليمية — ريادةٌ إقليمية متميزة","Description_EN","35 regional awards - distinguished regional leadership","IconClass","hgi-location-01","ItemOrder","2"),
                    Row("Title","دوليًا","Title_EN","Internationally","Description","60 جائزة دولية — إنجازات عالمية متميزة","Description_EN","60 international awards - outstanding global achievements","IconClass","hgi-global","ItemOrder","3")
                }
            };

            // 3) الخدمات الأكاديمية (swiper)
            map[NsListNames.AcademicServices] = new NsListDef
            {
                Name = NsListNames.AcademicServices,
                TitleAr = "الخدمات الأكاديمية", TitleEn = "Academic services",
                Description = "Academic service cards shown in the swiper.",
                Fields = new List<NsFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), Icon(),
                    new NsFieldDef("BadgeText", "الوسم (عربي)", "Badge (AR)", SPFieldType.Text, true),
                    new NsFieldDef("BadgeText_EN", "الوسم (إنجليزي)", "Badge (EN)", SPFieldType.Text),
                    LinkUrl(), BtnAr(), BtnEn(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الإرشاد الأكاديمي","Title_EN","Academic advising","Description","يساعدك في اختيار المسار الأكاديمي الأنسب.","Description_EN","Helps you choose the most suitable academic path.","IconClass","hgi-mentor","BadgeText","الطالبات","BadgeText_EN","Students","ButtonText","انتقال إلى المنصة","ButtonText_EN","Go to the platform","LinkUrl","#, الإرشاد الأكاديمي","ItemOrder","1"),
                    Row("Title","التسجيل والحذف والإضافة","Title_EN","Registration, add and drop","Description","إدارة خطتك الدراسية بسهولة.","Description_EN","Manage your study plan easily.","IconClass","hgi-file-edit","BadgeText","الطالبات","BadgeText_EN","Students","ButtonText","انتقال إلى المنصة","ButtonText_EN","Go to the platform","LinkUrl","#, التسجيل والحذف والإضافة","ItemOrder","2"),
                    Row("Title","الجداول والقاعات الدراسية","Title_EN","Timetables and classrooms","Description","متابعة مواعيد وأماكن محاضراتك.","Description_EN","Track the times and places of your lectures.","IconClass","hgi-calendar-02","BadgeText","الطالبات","BadgeText_EN","Students","ButtonText","انتقال إلى المنصة","ButtonText_EN","Go to the platform","LinkUrl","#, الجداول والقاعات الدراسية","ItemOrder","3"),
                    Row("Title","الاعتذار عن الفصل الدراسي","Title_EN","Semester withdrawal","Description","تقديم طلبات الاعتذار إلكترونيًا.","Description_EN","Submit withdrawal requests online.","IconClass","hgi-file-02","BadgeText","الطالبات","BadgeText_EN","Students","ButtonText","انتقال إلى المنصة","ButtonText_EN","Go to the platform","LinkUrl","#, الاعتذار عن الفصل الدراسي","ItemOrder","4"),
                    Row("Title","التأجيل والانقطاع","Title_EN","Postponement and interruption","Description","خدمات التأجيل وإعادة القيد.","Description_EN","Postponement and re-enrolment services.","IconClass","hgi-calendar-add-01","BadgeText","الطالبات","BadgeText_EN","Students","ButtonText","انتقال إلى المنصة","ButtonText_EN","Go to the platform","LinkUrl","#, التأجيل والانقطاع","ItemOrder","5"),
                    Row("Title","السجل الأكاديمي","Title_EN","Academic record","Description","الاطلاع على سجلك الأكاديمي الكامل.","Description_EN","View your full academic record.","IconClass","hgi-certificate-01","BadgeText","الطالبات","BadgeText_EN","Students","ButtonText","انتقال إلى المنصة","ButtonText_EN","Go to the platform","LinkUrl","#, السجل الأكاديمي","ItemOrder","6")
                }
            };

            // 4) روابط سريعة
            map[NsListNames.QuickLinks] = new NsListDef
            {
                Name = NsListNames.QuickLinks,
                TitleAr = "روابط سريعة", TitleEn = "Quick links",
                Description = "Quick navigation cards.",
                Fields = new List<NsFieldDef> { TitleEn(), Icon(), LinkUrl(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","بوابة الخدمات","Title_EN","Services portal","IconClass","hgi-school","LinkUrl","#, بوابة الخدمات","ItemOrder","1"),
                    Row("Title","نظام التعلم الإلكتروني","Title_EN","E-learning system","IconClass","hgi-book-open-01","LinkUrl","#, نظام التعلم الإلكتروني","ItemOrder","2"),
                    Row("Title","المكتبة السعودية الرقمية","Title_EN","Saudi Digital Library","IconClass","hgi-book-02","LinkUrl","#, المكتبة السعودية الرقمية","ItemOrder","3"),
                    Row("Title","منصة سجّلني","Title_EN","Sajjilni platform","IconClass","hgi-chart-line-data-01","LinkUrl","#, منصة سجّلني","ItemOrder","4"),
                    Row("Title","تطبيق طالبات نورة","Title_EN","Noura Students app","IconClass","hgi-smart-phone-01","LinkUrl","#, تطبيق طالبات نورة","ItemOrder","5"),
                    Row("Title","استبانة رضا الطالبات عن بيئة وخدمات الجامعة","Title_EN","Student satisfaction survey","IconClass","hgi-check-list","LinkUrl","#, استبانة رضا الطالبات","ItemOrder","6")
                }
            };

            // 5) الخدمات الطلابية (icon list beside the image)
            map[NsListNames.StudentServices] = new NsListDef
            {
                Name = NsListNames.StudentServices,
                TitleAr = "الخدمات الطلابية", TitleEn = "Student services",
                Description = "Student service items listed next to the section image.",
                Fields = new List<NsFieldDef> { TitleEn(), Icon(), LinkUrl(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","خدمات الدعم النفسي والاجتماعي","Title_EN","Psychological and social support","IconClass","hgi-favourite","ItemOrder","1"),
                    Row("Title","خدمات التغذية","Title_EN","Catering services","IconClass","hgi-restaurant-01","ItemOrder","2"),
                    Row("Title","خدمات ذوي الاحتياجات الخاصة","Title_EN","Special needs services","IconClass","hgi-accessibility","ItemOrder","3"),
                    Row("Title","النقل والمواصلات","Title_EN","Transport","IconClass","hgi-bus-01","ItemOrder","4"),
                    Row("Title","السكن الجامعي","Title_EN","University housing","IconClass","hgi-building-03","ItemOrder","5"),
                    Row("Title","خدمات صندوق الطالبات","Title_EN","Student fund services","IconClass","hgi-dollar-01","ItemOrder","6"),
                    Row("Title","وحدة المستفيدات","Title_EN","Beneficiaries unit","IconClass","hgi-user-group","ItemOrder","7"),
                    Row("Title","تشغيل الطالبات داخل الحرم الجامعي","Title_EN","On-campus student employment","IconClass","hgi-briefcase-01","ItemOrder","8")
                }
            };

            // 6) تواريخ تهمك
            map[NsListNames.ImportantDates] = new NsListDef
            {
                Name = NsListNames.ImportantDates,
                TitleAr = "تواريخ تهمك", TitleEn = "Dates that matter",
                Description = "Registration dates, semesters and exam schedule.",
                Fields = new List<NsFieldDef>
                {
                    TitleEn(),
                    new NsFieldDef("EventDate", "التاريخ", "Date", SPFieldType.DateTime, true),
                    new NsFieldDef("DateCategory", "المجموعة", "Group", SPFieldType.Choice, true, 0,
                                   "مواعيد التسجيل / الفصول الدراسية / جدول الاختبارات", DateCategories),
                    new NsFieldDef("SubTitle", "الوصف الفرعي (عربي)", "Subtitle (AR)", SPFieldType.Text),
                    new NsFieldDef("SubTitle_EN", "الوصف الفرعي (إنجليزي)", "Subtitle (EN)", SPFieldType.Text),
                    Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","التسجيل للفصل الدراسي الأول","Title_EN","First semester registration","EventDate","2026-06-24","DateCategory","مواعيد التسجيل","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","1"),
                    Row("Title","التسجيل للفصل الدراسي الثاني","Title_EN","Second semester registration","EventDate","2026-06-19","DateCategory","مواعيد التسجيل","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","2"),
                    Row("Title","التسجيل للفصل الصيفي","Title_EN","Summer term registration","EventDate","2026-06-07","DateCategory","مواعيد التسجيل","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","3"),
                    Row("Title","الفصل الدراسي الأول","Title_EN","First semester","EventDate","2026-09-01","DateCategory","الفصول الدراسية","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","1"),
                    Row("Title","الفصل الدراسي الثاني","Title_EN","Second semester","EventDate","2027-01-12","DateCategory","الفصول الدراسية","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","2"),
                    Row("Title","الفصل الصيفي","Title_EN","Summer term","EventDate","2027-06-08","DateCategory","الفصول الدراسية","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","3"),
                    Row("Title","اختبارات الفصل الأول","Title_EN","First semester exams","EventDate","2026-12-15","DateCategory","جدول الاختبارات","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","1"),
                    Row("Title","اختبارات الفصل الثاني","Title_EN","Second semester exams","EventDate","2027-05-18","DateCategory","جدول الاختبارات","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","2"),
                    Row("Title","اختبارات الفصل الصيفي","Title_EN","Summer term exams","EventDate","2027-07-20","DateCategory","جدول الاختبارات","SubTitle","التقويم الأكاديمي","SubTitle_EN","Academic calendar","IconClass","hgi-calendar-03","ItemOrder","3")
                }
            };

            // 7) الحياة الجامعية
            map[NsListNames.CampusLife] = new NsListDef
            {
                Name = NsListNames.CampusLife,
                TitleAr = "الحياة الجامعية", TitleEn = "Campus life",
                Description = "Campus life cards.",
                Fields = new List<NsFieldDef> { TitleEn(), Icon(), LinkUrl(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","المجلس الاستشاري الطلابي","Title_EN","Student advisory council","IconClass","hgi-user-group","ItemOrder","1"),
                    Row("Title","الأندية الطلابية","Title_EN","Student clubs","IconClass","hgi-user-group","ItemOrder","2"),
                    Row("Title","التطوع والكشافة","Title_EN","Volunteering and scouting","IconClass","hgi-favourite","ItemOrder","3"),
                    Row("Title","الرياضة في نورة","Title_EN","Sport at PNU","IconClass","hgi-bicycle","ItemOrder","4"),
                    Row("Title","الرحلات الطلابية","Title_EN","Student trips","IconClass","hgi-location-01","ItemOrder","5")
                }
            };

            // 8) التطوير المهني والوظيفي
            map[NsListNames.Career] = new NsListDef
            {
                Name = NsListNames.Career,
                TitleAr = "التطوير المهني والوظيفي", TitleEn = "Professional and career development",
                Description = "Career development cards.",
                Fields = new List<NsFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), LinkUrl(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","التدريب التعاوني والميداني","Title_EN","Cooperative and field training","IconClass","hgi-target-02","ItemOrder","1"),
                    Row("Title","الإرشاد المهني","Title_EN","Career guidance","IconClass","hgi-target-02","ItemOrder","2"),
                    Row("Title","برنامج سموق","Title_EN","Sumooq program","IconClass","hgi-target-02","ItemOrder","3"),
                    Row("Title","الخريجات","Title_EN","Alumnae","IconClass","hgi-target-02","ItemOrder","4")
                }
            };

            // 9) تجارب ملهمة من طالباتنا
            map[NsListNames.Experiences] = new NsListDef
            {
                Name = NsListNames.Experiences,
                TitleAr = "تجارب ملهمة من طالباتنا", TitleEn = "Inspiring student stories",
                Description = "Student testimonials.",
                Fields = new List<NsFieldDef>
                {
                    TitleEn(),
                    new NsFieldDef("RoleText", "الصفة (عربي)", "Role (AR)", SPFieldType.Text, true),
                    new NsFieldDef("RoleText_EN", "الصفة (إنجليزي)", "Role (EN)", SPFieldType.Text),
                    DescAr(), DescEn(), ImageUrl(), Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","سارة العنزي","Title_EN","Sarah Al-Anazi",
                        "RoleText","طالبة في جامعة الأميرة نورة بنت عبدالرحمن","RoleText_EN","Student at Princess Nourah bint Abdulrahman University",
                        "Description","جامعة نورة وفرت لي كل الإمكانات لتحقيق أحلامي الأكاديمية والمهنية، وقد ساعدتني بيئتها التعليمية المتكاملة على تطوير مهاراتي وبناء مساري بثقة.",
                        "Description_EN","PNU gave me everything I needed to reach my academic and professional goals; its integrated learning environment helped me build my path with confidence.",
                        "IconClass","hgi-star","ImageUrl","/Style Library/DGA/images/students/story-1.png, سارة العنزي","ItemOrder","1"),
                    Row("Title","نورة العتيبي","Title_EN","Noura Al-Otaibi",
                        "RoleText","طالبة في جامعة الأميرة نورة بنت عبدالرحمن","RoleText_EN","Student at Princess Nourah bint Abdulrahman University",
                        "Description","الدعم الأكاديمي والنفسي المستمر ساعدني على التفوق والإبداع، ومنحني الفرصة للمشاركة في الأنشطة والمبادرات التي أثرت تجربتي الجامعية.",
                        "Description_EN","Continuous academic and psychological support helped me excel and gave me the chance to join activities that enriched my university experience.",
                        "IconClass","hgi-star","ImageUrl","/Style Library/DGA/images/students/story-2.png, نورة العتيبي","ItemOrder","2")
                }
            };

            // 10) المنح والدعم المالي
            map[NsListNames.FinancialSupport] = new NsListDef
            {
                Name = NsListNames.FinancialSupport,
                TitleAr = "المنح والدعم المالي", TitleEn = "Scholarships and financial support",
                Description = "Scholarship and financial support cards.",
                Fields = new List<NsFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), LinkUrl(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","المنح الدراسية","Title_EN","Scholarships","Description","فرص دعم دراسية تساعدك في تحقيق أهدافك.","Description_EN","Study support opportunities that help you reach your goals.","IconClass","hgi-certificate-01","ItemOrder","1"),
                    Row("Title","المكافآت الطلابية","Title_EN","Student allowances","Description","الخدمات المالية والمكافآت المخصصة للطالبات.","Description_EN","Financial services and allowances for students.","IconClass","hgi-dollar-01","ItemOrder","2")
                }
            };

            // 11) التواصل والدعم
            map[NsListNames.Contact] = new NsListDef
            {
                Name = NsListNames.Contact,
                TitleAr = "التواصل والدعم", TitleEn = "Contact and support",
                Description = "Contact and support details.",
                Fields = new List<NsFieldDef>
                {
                    TitleEn(),
                    new NsFieldDef("ContactValue", "القيمة (عربي)", "Value (AR)", SPFieldType.Text, true),
                    new NsFieldDef("ContactValue_EN", "القيمة (إنجليزي)", "Value (EN)", SPFieldType.Text),
                    Icon(), LinkUrl(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","اتصل بنا","Title_EN","Call us","ContactValue","+966 11 822 0000","ContactValue_EN","+966 11 822 0000","IconClass","hgi-call","LinkUrl","tel:+966118220000, اتصل بنا","ItemOrder","1"),
                    Row("Title","الدعم الفني","Title_EN","Technical support","ContactValue","الدعم التقني لخدمات الطالبات","ContactValue_EN","Technical support for student services","IconClass","hgi-headset","ItemOrder","2")
                }
            };

            // Editors allowed to use ucNsAdmin. Provisioned like any other list so
            // the screen works on a fresh web; an empty list simply authorises nobody,
            // and ManageLists / site admin still gets you in.
            map[NsListNames.AdminUsers] = new NsListDef
            {
                Name = NsListNames.AdminUsers,
                TitleAr = "مسؤولو المحتوى", TitleEn = "Content administrators",
                Description = "Users allowed to manage the Noura Students content.",
                Fields = new List<NsFieldDef>
                {
                    new NsFieldDef("UserAccount", "حساب المستخدم", "User account", SPFieldType.User, true),
                    new NsFieldDef("Active", "مفعّل", "Active", SPFieldType.Boolean, true),
                    Order()
                },
                Seed = null
            };

            return map;
        }
    }
}
