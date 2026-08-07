using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    public class TwFieldDef
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

        public TwFieldDef(string name, string ar, string en, SPFieldType type,
                          bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return TwHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class TwListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<TwFieldDef> Fields { get; set; }
        /// <summary>Rows inserted when the list is created.</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return TwHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for every Tawasul list: used by the provisioner to create
    /// the lists and by ucTwAdmin to render the CRUD form.
    /// Title always holds the Arabic title of the item (built-in field, re-labelled).
    /// </summary>
    public static class TwListSchema
    {
        /// <summary>Stored (Arabic) value that groups a channel row under the University card.</summary>
        public const string GroupUniversity = "التواصل مع الجامعة";

        /// <summary>Stored (Arabic) value that groups a channel row under the Tawasul Nourah card.</summary>
        public const string GroupTawasul = "تواصل نورة";

        /// <summary>Section keys used by the TwSectionTitles list.</summary>
        public const string KeyUniversity = "UniversityContact";
        public const string KeyTawasul    = "TawasulNourah";
        public const string KeyEntities   = "Entities";
        public const string KeyLocation   = "Location";
        public const string KeyPlatforms  = "DigitalPlatforms";

        private static readonly string[] ContactGroups = new string[] { GroupUniversity, GroupTawasul };

        private static Dictionary<string, TwListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, TwListDef> All
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

        public static TwListDef Get(string listName)
        {
            TwListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        // ---- shared field builders -------------------------------------------------
        private static TwFieldDef TitleEn()  { return new TwFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static TwFieldDef Icon()     { return new TwFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, true, 0, "hgi-call, hgi-mail-01, hgi-location-01 ..."); }
        private static TwFieldDef Order()    { return new TwFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static TwFieldDef LinkUrl()  { return new TwFieldDef("LinkUrl", "الرابط", "Link URL", SPFieldType.URL); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, TwListDef> Build()
        {
            var map = new Dictionary<string, TwListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) عناوين الأقسام - every section heading + subtitle lives here.
            map[TwListNames.SectionTitles] = new TwListDef
            {
                Name = TwListNames.SectionTitles,
                TitleAr = "عناوين الأقسام", TitleEn = "Section titles",
                Description = "Headings and subtitles for every section of the Contact page.",
                Fields = new List<TwFieldDef>
                {
                    TitleEn(),
                    new TwFieldDef("SectionKey", "مفتاح القسم", "Section key", SPFieldType.Text, true, 0,
                                   "UniversityContact / TawasulNourah / Entities / Location / DigitalPlatforms"),
                    new TwFieldDef("Subtitle", "النص الفرعي (عربي)", "Subtitle (AR)", SPFieldType.Note, false, 3),
                    new TwFieldDef("Subtitle_EN", "النص الفرعي (إنجليزي)", "Subtitle (EN)", SPFieldType.Note, false, 3),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","التواصل مع الجامعة","Title_EN","Contact the University","SectionKey",KeyUniversity,
                        "Subtitle","بيانات الاتصال الرسمية لجامعة الأميرة نورة بنت عبدالرحمن.",
                        "Subtitle_EN","Official contact details for Princess Nourah bint Abdulrahman University.","ItemOrder","1"),
                    Row("Title","تواصل نورة","Title_EN","Tawasul Nourah","SectionKey",KeyTawasul,
                        "Subtitle","قناة موحدة للتواصل مع مسؤولي ومنسوبي الجامعة ورفع الطلبات ومتابعتها.",
                        "Subtitle_EN","A unified channel to reach the University's officials and staff and to submit and follow up on requests.","ItemOrder","2"),
                    Row("Title","الجهات داخل الجامعة","Title_EN","University entities","SectionKey",KeyEntities,
                        "Subtitle","بيانات مختصرة لأبرز القنوات الداخلية التي يحتاجها المستفيد.",
                        "Subtitle_EN","Brief details of the main internal channels a beneficiary may need.","ItemOrder","3"),
                    Row("Title","الموقع","Title_EN","Location","SectionKey",KeyLocation,
                        "Subtitle","","Subtitle_EN","","ItemOrder","4"),
                    Row("Title","منصاتنا الرقمية","Title_EN","Our digital platforms","SectionKey",KeyPlatforms,
                        "Subtitle","","Subtitle_EN","","ItemOrder","5")
                }
            };

            // 2) قنوات الاتصال للبطاقتين العلويتين، مجمّعة حسب ContactGroup.
            map[TwListNames.ContactChannels] = new TwListDef
            {
                Name = TwListNames.ContactChannels,
                TitleAr = "قنوات الاتصال", TitleEn = "Contact channels",
                Description = "Phone / e-mail / link rows of the two top cards.",
                Fields = new List<TwFieldDef>
                {
                    TitleEn(),
                    new TwFieldDef("ContactGroup", "البطاقة", "Card", SPFieldType.Choice, true, 0,
                                   "التواصل مع الجامعة / تواصل نورة", ContactGroups),
                    new TwFieldDef("ContactValue", "القيمة (عربي)", "Value (AR)", SPFieldType.Text, true),
                    new TwFieldDef("ContactValue_EN", "القيمة (إنجليزي)", "Value (EN)", SPFieldType.Text),
                    new TwFieldDef("Description", "نص (عربي)", "Text (AR)", SPFieldType.Note, false, 3, "للصفوف النصية مثل \"الخدمات المتاحة\""),
                    new TwFieldDef("Description_EN", "نص (إنجليزي)", "Text (EN)", SPFieldType.Note, false, 3),
                    Icon(), LinkUrl(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","هاتف الجامعة","Title_EN","University phone","ContactGroup",GroupUniversity,
                        "ContactValue","+966 11 822 0000","ContactValue_EN","+966 11 822 0000",
                        "IconClass","hgi-call","LinkUrl","tel:+966118220000, هاتف الجامعة","ItemOrder","1"),
                    Row("Title","بريد الجامعة","Title_EN","University e-mail","ContactGroup",GroupUniversity,
                        "ContactValue","info@pnu.edu.sa","ContactValue_EN","info@pnu.edu.sa",
                        "IconClass","hgi-mail-01","LinkUrl","mailto:info@pnu.edu.sa, بريد الجامعة","ItemOrder","2"),

                    Row("Title","رابط الخدمة","Title_EN","Service link","ContactGroup",GroupTawasul,
                        "ContactValue","tawasulnourah.pnu.edu.sa","ContactValue_EN","tawasulnourah.pnu.edu.sa",
                        "IconClass","hgi-link-square-02","LinkUrl","https://tawasulnourah.pnu.edu.sa/, رابط الخدمة","ItemOrder","1"),
                    Row("Title","هاتف تواصل نورة","Title_EN","Tawasul Nourah phone","ContactGroup",GroupTawasul,
                        "ContactValue","0118220000","ContactValue_EN","0118220000",
                        "IconClass","hgi-call","LinkUrl","tel:0118220000, هاتف تواصل نورة","ItemOrder","2"),
                    Row("Title","بريد تواصل نورة","Title_EN","Tawasul Nourah e-mail","ContactGroup",GroupTawasul,
                        "ContactValue","pnu-tawasul@pnu.edu.sa","ContactValue_EN","pnu-tawasul@pnu.edu.sa",
                        "IconClass","hgi-mail-01","LinkUrl","mailto:pnu-tawasul@pnu.edu.sa, بريد تواصل نورة","ItemOrder","3"),
                    Row("Title","الخدمات المتاحة","Title_EN","Available services","ContactGroup",GroupTawasul,
                        "Description","حجز المواعيد، ورفع الاستفسارات والشكاوى والمقترحات، ومتابعة الطلبات مع الجهات المعنية في الجامعة.",
                        "Description_EN","Appointment booking, submitting enquiries, complaints and suggestions, and following up on requests with the relevant University entities.",
                        "IconClass","hgi-customer-service-01","ItemOrder","4")
                }
            };

            // 3) منصاتنا الرقمية
            map[TwListNames.SocialLinks] = new TwListDef
            {
                Name = TwListNames.SocialLinks,
                TitleAr = "المنصات الرقمية", TitleEn = "Digital platforms",
                Description = "Social / digital platform links shown in the University card.",
                Fields = new List<TwFieldDef> { TitleEn(), LinkUrl(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","منصة إكس","Title_EN","X platform","LinkUrl","https://twitter.com/_PNU_KSA","IconClass","hgi-new-twitter","ItemOrder","1"),
                    Row("Title","إنستغرام","Title_EN","Instagram","LinkUrl","https://www.instagram.com/_PNU_KSA/","IconClass","hgi-instagram","ItemOrder","2"),
                    Row("Title","لينكدإن","Title_EN","LinkedIn","LinkUrl","https://www.linkedin.com/school/princess-nourah-bint-abdulrahman-university/","IconClass","hgi-linkedin-02","ItemOrder","3"),
                    Row("Title","يوتيوب","Title_EN","YouTube","LinkUrl","https://www.youtube.com/channel/UC-0BTuZ46ApPXt6dWONTyGg","IconClass","hgi-youtube","ItemOrder","4")
                }
            };

            // 4) الجهات داخل الجامعة
            map[TwListNames.Entities] = new TwListDef
            {
                Name = TwListNames.Entities,
                TitleAr = "الجهات داخل الجامعة", TitleEn = "University entities",
                Description = "Internal university entities with their phone, e-mail and link.",
                Fields = new List<TwFieldDef>
                {
                    TitleEn(),
                    new TwFieldDef("Phone", "الهاتف", "Phone", SPFieldType.Text, true, 0, "41881 - 41944 - 41924"),
                    new TwFieldDef("PhoneLink", "رابط الهاتف", "Phone link (tel:)", SPFieldType.URL, false, 0, "tel:0118220000 (اتركه فارغًا للتحويلات الداخلية)"),
                    new TwFieldDef("Email", "البريد", "E-mail", SPFieldType.Text, true),
                    new TwFieldDef("ContactValue", "نص الرابط", "Link text", SPFieldType.Text, false, 0, "tawasulnourah.pnu.edu.sa"),
                    LinkUrl(),
                    new TwFieldDef("Description", "ملاحظة (عربي)", "Note (AR)", SPFieldType.Note, false, 2),
                    new TwFieldDef("Description_EN", "ملاحظة (إنجليزي)", "Note (EN)", SPFieldType.Note, false, 2),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","تواصل نورة","Title_EN","Tawasul Nourah","Phone","0118220000","PhoneLink","tel:0118220000",
                        "Email","pnu-tawasul@pnu.edu.sa","ContactValue","tawasulnourah.pnu.edu.sa",
                        "LinkUrl","https://tawasulnourah.pnu.edu.sa/, تواصل نورة","ItemOrder","1"),
                    Row("Title","وكالة الجامعة للشؤون الأكاديمية","Title_EN","University Vice-Presidency for Academic Affairs",
                        "Phone","41881 - 41944 - 41924","Email","ea-office@pnu.edu.sa","ItemOrder","2"),
                    Row("Title","عمادة القبول والتسجيل","Title_EN","Deanship of Admission and Registration",
                        "Phone","43467 - 43490","Email","dar@pnu.edu.sa","ItemOrder","3"),
                    Row("Title","عمادة شؤون الطالبات","Title_EN","Deanship of Student Affairs",
                        "Phone","42299","Email","Dsa_doff@pnu.edu.sa","ItemOrder","4"),
                    Row("Title","عمادة الدراسات العليا","Title_EN","Deanship of Graduate Studies",
                        "Phone","42049 - 42131","Email","dgs@pnu.edu.sa","ItemOrder","5"),
                    Row("Title","كلية الإدارة والأعمال","Title_EN","College of Business Administration",
                        "Phone","44437 - 22930","Email","CBA@PNU.EDU.SA","ItemOrder","6"),
                    Row("Title","كلية اللغات","Title_EN","College of Languages",
                        "Phone","22814 - 22652","Email","clt.vice_dean@pnu.edu.sa","ItemOrder","7"),
                    Row("Title","الإدارة العامة للموارد البشرية","Title_EN","General Administration of Human Resources",
                        "Phone","43068 - 43133","Email","Hrd-gic@pnu.edu.sa","ItemOrder","8"),
                    Row("Title","الكلية التطبيقية","Title_EN","Applied College",
                        "Phone","36861","Email","COC-GA@PNU.EDU.SA","ItemOrder","9"),
                    Row("Title","مركز الدعم الطلابي والمهني","Title_EN","Student and Career Support Center",
                        "Email","CDC@pnu.edu.sa","Description","ضمن قنوات عمادة شؤون الطالبات",
                        "Description_EN","Within the Deanship of Student Affairs channels","ItemOrder","10")
                }
            };

            // 5) الموقع - عنصر واحد، يُعاد استخدامه في بطاقة الجامعة وقسم الموقع.
            map[TwListNames.Location] = new TwListDef
            {
                Name = TwListNames.Location,
                TitleAr = "الموقع", TitleEn = "Location",
                Description = "National address, short code and embedded map. One item.",
                Fields = new List<TwFieldDef>
                {
                    TitleEn(),
                    new TwFieldDef("Description", "العنوان (عربي)", "Address (AR)", SPFieldType.Note, false, 3),
                    new TwFieldDef("Description_EN", "العنوان (إنجليزي)", "Address (EN)", SPFieldType.Note, false, 3),
                    new TwFieldDef("ShortCode", "العنوان الوطني المختصر", "National short address", SPFieldType.Text, true, 0, "RUKA7808"),
                    new TwFieldDef("MapUrl", "رابط الخريطة (embed)", "Map URL (embed)", SPFieldType.URL, false, 0, "https://www.google.com/maps?q=...&output=embed"),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","عنوان الجامعة الوطني","Title_EN","National address",
                        "Description","رقم المبنى 7808، مطار الملك خالد الدولي، رقم الوحدة 1، الرياض 13412-3230، المملكة العربية السعودية.",
                        "Description_EN","Building No. 7808, King Khalid International Airport, Unit No. 1, Riyadh 13412-3230, Kingdom of Saudi Arabia.",
                        "ShortCode","RUKA7808",
                        "MapUrl","https://www.google.com/maps?q=Princess%20Nourah%20bint%20Abdulrahman%20University%20Riyadh&output=embed",
                        "ItemOrder","1")
                }
            };

            // Editors allowed to use ucTwAdmin. Same shared list used by the other pages.
            map[TwListNames.AdminUsers] = new TwListDef
            {
                Name = TwListNames.AdminUsers,
                TitleAr = "مسؤولو المحتوى", TitleEn = "Content administrators",
                Description = "Users allowed to manage the Tawasul content.",
                Fields = new List<TwFieldDef>
                {
                    new TwFieldDef("UserAccount", "حساب المستخدم", "User account", SPFieldType.User, true),
                    new TwFieldDef("Active", "مفعّل", "Active", SPFieldType.Boolean, true),
                    Order()
                },
                Seed = null
            };

            return map;
        }
    }
}
