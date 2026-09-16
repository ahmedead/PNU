using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls
{
    public class CertFieldDef
    {
        public string InternalName { get; set; }
        public string DisplayAr { get; set; }
        public string DisplayEn { get; set; }
        public SPFieldType Type { get; set; }

        /// <summary>
        /// Key this column takes in the JSON payload handed to the client script.
        /// Empty = the column is not published to the page.
        /// </summary>
        public string JsonKey { get; set; }

        /// <summary>Show this column in the admin grid and in the list default view.</summary>
        public bool InGrid { get; set; }

        /// <summary>Rows for Note fields in the admin form.</summary>
        public int Rows { get; set; }

        public string Hint { get; set; }

        /// <summary>Options for Choice fields.</summary>
        public string[] Choices { get; set; }

        public CertFieldDef(string name, string ar, string en, SPFieldType type, string jsonKey,
                            bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; JsonKey = jsonKey; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return CertHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class CertListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<CertFieldDef> Fields { get; set; }

        /// <summary>Rows inserted when the list is created.</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return CertHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for the certifications list: used by the provisioner to
    /// create the list, by ucCertAdmin to render the CRUD form and by ucCertifications
    /// to build the JSON payload.
    ///
    /// Title always holds the Arabic name of the certificate (built-in field,
    /// re-labelled) and is published as "title_ar".
    /// </summary>
    public static class CertListSchema
    {
        /// <summary>JSON key carrying the built-in Title column.</summary>
        public const string TitleJsonKey = "title_ar";

        private static Dictionary<string, CertListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, CertListDef> All
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

        public static CertListDef Get(string listName)
        {
            CertListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        private static Dictionary<string, CertListDef> Build()
        {
            var map = new Dictionary<string, CertListDef>(StringComparer.OrdinalIgnoreCase);

            // ---- دليل الشهادات الاحترافية -----------------------------------------
            map[CertListNames.Certifications] = new CertListDef
            {
                Name = CertListNames.Certifications,
                TitleAr = "دليل الشهادات الاحترافية", TitleEn = "Professional certifications guide",
                Description = "Professional certificates offered to PNU students, by college and department.",
                Fields = new List<CertFieldDef>
                {
                    new CertFieldDef("Title_EN", "اسم الشهادة (إنجليزي)", "Certificate name (EN)", SPFieldType.Text, "title_en", true),
                    new CertFieldDef("College", "الكلية", "College", SPFieldType.Text, "college", true, 0,
                        "يُستخدم في قائمة التصفية - اكتبه بنفس الصيغة في كل الصفوف."),
                    new CertFieldDef("Department", "القسم", "Department", SPFieldType.Text, "department", true),
                    new CertFieldDef("ProgramName", "البرنامج", "Program", SPFieldType.Text, "Program"),
                    new CertFieldDef("Description", "نبذة عن الشهادة", "Description", SPFieldType.Note, "description", false, 5),
                    new CertFieldDef("CertLevel", "المستوى", "Level", SPFieldType.Text, "level", true, 0,
                        "مبتدئ (لا تتطلب خبرات عالية ) / متوسط / محترف / مناسب للجميع"),
                    new CertFieldDef("CertLanguage", "اللغة", "Language", SPFieldType.Text, "language"),
                    new CertFieldDef("Cost", "التكلفة", "Cost", SPFieldType.Text, "cost", false, 0, "950 SR / مجانية"),
                    new CertFieldDef("DeliveryMode", "آلية التنفيذ", "Delivery mode", SPFieldType.Text, "mode", false, 0,
                        "حضوريًا / عن بعد / حضوريًا أو عن بعد"),
                    new CertFieldDef("IsSupported", "دعم هدف", "HRDF support", SPFieldType.Text, "is_supported", false, 0,
                        "نعم / لا / مدعومة من هدف / غير مدعومة"),
                    new CertFieldDef("Topics", "الموضوعات", "Topics", SPFieldType.Note, "topics", false, 10,
                        "موضوع في كل سطر - الصفحة تعرضها كقائمة نقطية."),
                    new CertFieldDef("Requirements", "المتطلبات", "Requirements", SPFieldType.Note, "requirements", false, 5),
                    new CertFieldDef("RegistrationSteps", "إجراءات التقديم", "Registration steps", SPFieldType.Note, "registration_steps", false, 4),
                    new CertFieldDef("Provider", "الجهة المانحة", "Provider", SPFieldType.Note, "provider", true, 3),
                    new CertFieldDef("LearningResources", "المصادر التعليمية", "Learning resources", SPFieldType.Note, "resources", false, 5),
                    new CertFieldDef("Validity", "صلاحية الشهادة", "Validity", SPFieldType.Text, "validity", false, 0, "دائمة / سنتان / 3 سنوات"),
                    new CertFieldDef("ExamDuration", "مدة الاختبار", "Exam duration", SPFieldType.Text, "exam_duration"),
                    new CertFieldDef("QuestionsCount", "عدد الأسئلة", "Questions count", SPFieldType.Text, "questions_count"),
                    new CertFieldDef("QuestionsType", "نوع الأسئلة", "Questions type", SPFieldType.Note, "questions_type", false, 3),
                    new CertFieldDef("ContactOfficer", "اسم ضابط الاتصال", "Contact officer", SPFieldType.Text, "contact_officer"),
                    new CertFieldDef("DataApproved", "اعتماد ضابط الاتصال للبيانات", "Data approved", SPFieldType.Text, "data_approved"),
                    new CertFieldDef("CoursesCount", "عدد المقررات المرتبطة", "Linked courses count", SPFieldType.Text, "courses_count"),
                    new CertFieldDef("Course1", "المقرر 1", "Course 1", SPFieldType.Text, "course_1"),
                    new CertFieldDef("Course2", "المقرر 2", "Course 2", SPFieldType.Text, "course_2"),
                    new CertFieldDef("Course3", "المقرر 3", "Course 3", SPFieldType.Text, "course_3"),
                    new CertFieldDef("Course4", "المقرر 4", "Course 4", SPFieldType.Text, "course_4"),

                    // Note, not URL: several official links are longer than the 255
                    // characters a SharePoint URL column accepts.
                    new CertFieldDef("OfficialLink", "رابط الموقع الرسمي", "Official link", SPFieldType.Note, "link", false, 2,
                        "https://... - يظهر كزر \"زيارة الموقع الرسمي\"."),

                    new CertFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, string.Empty, true)
                },
                Seed = CertSeedData.Build()
            };

            // ---- مسؤولو المحتوى (shared, lives on /ar/ContentAdmin) -----------------
            map[CertListNames.AdminUsers] = new CertListDef
            {
                Name = CertListNames.AdminUsers,
                TitleAr = "مسؤولو المحتوى", TitleEn = "Content administrators",
                Description = "Users allowed to manage the portal content.",
                Fields = new List<CertFieldDef>
                {
                    new CertFieldDef("UserAccount", "حساب المستخدم", "User account", SPFieldType.User, string.Empty, true),
                    new CertFieldDef("Active", "مفعّل", "Active", SPFieldType.Boolean, string.Empty, true),
                    new CertFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, string.Empty, true)
                },
                Seed = null
            };

            return map;
        }
    }
}
