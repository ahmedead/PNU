using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public class ClFieldDef
    {
        public string InternalName { get; set; }
        public string DisplayAr { get; set; }
        public string DisplayEn { get; set; }
        public SPFieldType Type { get; set; }
        public bool InGrid { get; set; }
        public int Rows { get; set; }
        public string Hint { get; set; }
        public string[] Choices { get; set; }

        public ClFieldDef(string name, string ar, string en, SPFieldType type,
                          bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return ClHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class ClListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<ClFieldDef> Fields { get; set; }
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return ClHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for all Central Library lists, custom fields, and seed data.
    /// All image paths are rooted at /Style Library/DGA/public/images/.
    /// </summary>
    public static class ClListSchema
    {
        private static Dictionary<string, ClListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, ClListDef> All
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

        public static ClListDef Get(string listName)
        {
            ClListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        private static ClFieldDef TitleEn()   { return new ClFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static ClFieldDef DescAr()    { return new ClFieldDef("Description", "الوصف (عربي)", "Description (AR)", SPFieldType.Note, false, 3); }
        private static ClFieldDef DescEn()    { return new ClFieldDef("Description_EN", "الوصف (إنجليزي)", "Description (EN)", SPFieldType.Note, false, 3); }
        private static ClFieldDef SubTitleAr(){ return new ClFieldDef("SubTitle", "العنوان الفرعي (عربي)", "SubTitle (AR)", SPFieldType.Text, true); }
        private static ClFieldDef SubTitleEn(){ return new ClFieldDef("SubTitle_EN", "العنوان الفرعي (إنجليزي)", "SubTitle (EN)", SPFieldType.Text); }
        private static ClFieldDef Icon()      { return new ClFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, true); }
        private static ClFieldDef Order()     { return new ClFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static ClFieldDef LinkUrl()   { return new ClFieldDef("LinkUrl", "الرابط", "Link URL", SPFieldType.URL); }
        private static ClFieldDef ImageUrl()  { return new ClFieldDef("ImageUrl", "رابط الصورة", "Image URL", SPFieldType.URL); }
        private static ClFieldDef BtnAr()     { return new ClFieldDef("ButtonText", "نص الزر (عربي)", "Button text (AR)", SPFieldType.Text); }
        private static ClFieldDef BtnEn()     { return new ClFieldDef("ButtonText_EN", "نص الزر (إنجليزي)", "Button text (EN)", SPFieldType.Text); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, ClListDef> Build()
        {
            var map = new Dictionary<string, ClListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) ترويسة المكتبة المركزية (Header & Breadcrumb)
            map[ClListNames.Header] = new ClListDef
            {
                Name = ClListNames.Header,
                TitleAr = "ترويسة المكتبة المركزية", TitleEn = "Central Library Header",
                Description = "Hero header, title, subtitle, buttons and breadcrumb items.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(),
                    new ClFieldDef("BreadcrumbText", "مسار التنقل (عربي)", "Breadcrumb (AR)", SPFieldType.Note, false, 2, "Title|Url;Title|Url;Title|"),
                    new ClFieldDef("BreadcrumbText_EN", "مسار التنقل (إنجليزي)", "Breadcrumb (EN)", SPFieldType.Note, false, 2),
                    new ClFieldDef("Button1Text", "نص الزر الأول (عربي)", "Button 1 Text (AR)", SPFieldType.Text),
                    new ClFieldDef("Button1Text_EN", "نص الزر الأول (إنجليزي)", "Button 1 Text (EN)", SPFieldType.Text),
                    new ClFieldDef("Button1Url", "رابط الزر الأول", "Button 1 URL", SPFieldType.URL),
                    new ClFieldDef("Button2Text", "نص الزر الثاني (عربي)", "Button 2 Text (AR)", SPFieldType.Text),
                    new ClFieldDef("Button2Text_EN", "نص الزر الثاني (إنجليزي)", "Button 2 Text (EN)", SPFieldType.Text),
                    new ClFieldDef("Button2Url", "رابط الزر الثاني", "Button 2 URL", SPFieldType.URL),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","المكتبة المركزية","Title_EN","Central Library",
                        "Description","بوابتك للمعرفة والتميز الأكاديمي، وتضم مجموعات وخدمات ومرافق تدعم التعلم والبحث العلمي في الجامعة.",
                        "Description_EN","Your gateway to knowledge and academic excellence, featuring collections, services and facilities that support learning and research at the university.",
                        "BreadcrumbText","الرئيسية|/ar/Pages/default.aspx;مرافق الجامعة|/ar/UniversityLife/Pages/UniversityFacilities.aspx;المكتبة المركزية|",
                        "BreadcrumbText_EN","Home|/en/Pages/default.aspx;University Facilities|/en/UniversityLife/Pages/UniversityFacilities.aspx;Central Library|",
                        "Button1Text","البحث في الفهرس","Button1Text_EN","Catalog Search","Button1Url","https://library.pnu.edu.sa/",
                        "Button2Text","قواعد البيانات","Button2Text_EN","Databases","Button2Url","https://sdl.edu.sa/",
                        "ItemOrder","1")
                }
            };

            // 2) عن المكتبة المركزية (About Section)
            map[ClListNames.About] = new ClListDef
            {
                Name = ClListNames.About,
                TitleAr = "عن المكتبة المركزية", TitleEn = "About Central Library",
                Description = "About section content, paragraphs, banner images and numbers section title.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(),
                    new ClFieldDef("Paragraph1", "الفقرة الأولى (عربي)", "Paragraph 1 (AR)", SPFieldType.Note, false, 4),
                    new ClFieldDef("Paragraph1_EN", "الفقرة الأولى (إنجليزي)", "Paragraph 1 (EN)", SPFieldType.Note, false, 4),
                    new ClFieldDef("Paragraph2", "الفقرة الثانية (عربي)", "Paragraph 2 (AR)", SPFieldType.Note, false, 4),
                    new ClFieldDef("Paragraph2_EN", "الفقرة الثانية (إنجليزي)", "Paragraph 2 (EN)", SPFieldType.Note, false, 4),
                    new ClFieldDef("ImageUrlSm", "الصورة (صغيرة)", "Image (Small)", SPFieldType.URL),
                    new ClFieldDef("ImageUrlMd", "الصورة (متوسطة)", "Image (Medium)", SPFieldType.URL),
                    new ClFieldDef("ImageUrlLg", "الصورة (كبيرة)", "Image (Large)", SPFieldType.URL),
                    new ClFieldDef("ImageAlt", "النص البديل للصورة (عربي)", "Image Alt (AR)", SPFieldType.Text),
                    new ClFieldDef("ImageAlt_EN", "النص البديل للصورة (إنجليزي)", "Image Alt (EN)", SPFieldType.Text),
                    new ClFieldDef("NumbersTitle", "عنوان قسم الأرقام (عربي)", "Numbers Section Title (AR)", SPFieldType.Text),
                    new ClFieldDef("NumbersTitle_EN", "عنوان قسم الأرقام (إنجليزي)", "Numbers Section Title (EN)", SPFieldType.Text),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","عن المكتبة المركزية","Title_EN","About Central Library",
                        "Paragraph1","تُعدّ المكتبة المركزية منارة الثقافة في جامعة الأميرة نورة بنت عبدالرحمن، وأحد أبرز معالمها، إضافة إلى كونها إحدى الوجهات السياحية في المملكة العربية السعودية. يمتد مبنى المكتبة على مساحة تتجاوز ثمانية وثلاثين ألف متر مربع، وقد حصل على المرتبة الذهبية وفق نظام الريادة في تصميمات الطاقة والبيئة (LEED) للمباني الخضراء والمعتمد من المجلس الأمريكي للأبنية الخضراء.",
                        "Paragraph1_EN","The Central Library is a cultural beacon at Princess Nourah bint Abdulrahman University and one of its most prominent landmarks, as well as a tourist destination in Saudi Arabia. Spanning over 38,000 square meters, the building achieved LEED Gold certification from the U.S. Green Building Council.",
                        "Paragraph2","وتؤدي المكتبة دورًا محوريًا في دعم البرامج الأكاديمية وخدمة البحث العلمي، كما تمتاز بتصميم معماري عربي مستوحى من التراث الأندلسي المزين بالأحرف العربية. وتضم نظام التخزين والاسترجاع الآلي (الذراع الآلي) الأول من نوعه في الشرق الأوسط والثالث على مستوى العالم.",
                        "Paragraph2_EN","The library plays a pivotal role in supporting academic programs and scientific research, featuring an Andalusian-inspired Arabic architectural design. It houses an automated storage and retrieval system (automated arm), the first of its kind in the Middle East and third globally.",
                        "ImageUrlSm","/Style Library/DGA/public/images/hero/hero-library-sm.webp",
                        "ImageUrlMd","/Style Library/DGA/public/images/hero/hero-library-md.webp",
                        "ImageUrlLg","/Style Library/DGA/public/images/hero/hero-library-lg.webp",
                        "ImageAlt","مبنى المكتبة المركزية في جامعة الأميرة نورة","ImageAlt_EN","Central Library Building at Princess Nourah University",
                        "NumbersTitle","المكتبة في أرقام","NumbersTitle_EN","Library in numbers",
                        "ItemOrder","1")
                }
            };

            // 3) المكتبة في أرقام (Numbers)
            map[ClListNames.Numbers] = new ClListDef
            {
                Name = ClListNames.Numbers,
                TitleAr = "المكتبة في أرقام", TitleEn = "Library in Numbers",
                Description = "Stat counter cards for the Central Library.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(),
                    new ClFieldDef("StatValue", "القيمة", "Value", SPFieldType.Text, true),
                    Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","استيعاب الذراع للكتب والمراجع","Title_EN","Arm capacity for books & references","StatValue","5,000,000","IconClass","hgi-bookshelf-03","ItemOrder","1"),
                    Row("Title","مقعد للقراءة","Title_EN","Reading seats","StatValue","2,000","IconClass","hgi-chair-02","ItemOrder","2"),
                    Row("Title","خدمة إعارة","Title_EN","Borrowing services","StatValue","33,857","IconClass","hgi-book-upload","ItemOrder","3"),
                    Row("Title","زائر لفهرس مكتبات الجامعة","Title_EN","University library catalog visitors","StatValue","256,138","IconClass","hgi-catalogue","ItemOrder","4"),
                    Row("Title","زائر للمكتبة المركزية","Title_EN","Central Library visitors","StatValue","152,435","IconClass","hgi-user-group","ItemOrder","5"),
                    Row("Title","مستخدم للمكتبة السعودية الرقمية","Title_EN","Saudi Digital Library users","StatValue","77,924","IconClass","hgi-computer-cloud","ItemOrder","6"),
                    Row("Title","مستفيد من حجوزات القاعات","Title_EN","Hall reservation beneficiaries","StatValue","10,474","IconClass","hgi-calendar-check-in-01","ItemOrder","7"),
                    Row("Title","مستفيد من البرامج المعرفية","Title_EN","Knowledge program beneficiaries","StatValue","1,355","IconClass","hgi-school","ItemOrder","8")
                }
            };

            // 4) جوائز المكتبة المركزية (Awards)
            map[ClListNames.Awards] = new ClListDef
            {
                Name = ClListNames.Awards,
                TitleAr = "جوائز المكتبة المركزية", TitleEn = "Central Library Awards",
                Description = "Awards received by the Central Library.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), SubTitleAr(), SubTitleEn(),
                    new ClFieldDef("LogoUrl", "رابط الشعار", "Logo URL", SPFieldType.URL),
                    Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الفهرس العربي الموحد","Title_EN","Unified Arabic Catalog","Description","جائزة أفضل استخدام للفهرس العربي الموحد على مستوى العالم العربي من الفهرس العربي الموحد.","Description_EN","Award for best use of the Unified Arabic Catalog across the Arab world.","SubTitle","عام 1439هـ","SubTitle_EN","Year 1439 AH","LogoUrl","/Style Library/DGA/public/images/pnu-logo-ar-h.svg","ItemOrder","1"),
                    Row("Title","مبادرة «أعلم»","Title_EN","'A'lam' Initiative","Description","درع وشهادة تفوق عن المشاركة في مبادرة «أعلم» لتكريم المكتبات المتفاعلة مع أزمة كورونا على مستوى العالم العربي من الاتحاد العربي للمكتبات والمعلومات.","Description_EN","Shield and certificate of excellence for participating in the 'A'lam' initiative honoring libraries during COVID-19.","SubTitle","عام 1441هـ","SubTitle_EN","Year 1441 AH","LogoUrl","/Style Library/DGA/public/images/pnu-logo-ar-h.svg","ItemOrder","2"),
                    Row("Title","التميز المؤسسي","Title_EN","Institutional Excellence","Description","شهادة من أكاديمية نسيج في مجال التميز المؤسسي.","Description_EN","Certificate from Naseej Academy in institutional excellence.","SubTitle","عام 1446هـ","SubTitle_EN","Year 1446 AH","LogoUrl","/Style Library/DGA/public/images/pnu-logo-ar-h.svg","ItemOrder","3"),
                    Row("Title","جمعية المكتبات والمعلومات السعودية","Title_EN","Saudi Library & Information Association","Description","جائزة أفضل مكتبة من جمعية المكتبات والمعلومات السعودية.","Description_EN","Best Library Award from the Saudi Library and Information Association.","SubTitle","عام 1446هـ / 2024م","SubTitle_EN","Year 1446 AH / 2024 AD","LogoUrl","/Style Library/DGA/public/images/pnu-logo-ar-h.svg","ItemOrder","4")
                }
            };

            // 5) خدمات المكتبة المركزية (Services)
            map[ClListNames.Services] = new ClListDef
            {
                Name = ClListNames.Services,
                TitleAr = "خدمات المكتبة المركزية", TitleEn = "Central Library Services",
                Description = "Central Library service cards in slider.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), Icon(),
                    new ClFieldDef("BadgeText", "الوسم (عربي)", "Badge (AR)", SPFieldType.Text, true),
                    new ClFieldDef("BadgeText_EN", "الوسم (إنجليزي)", "Badge (EN)", SPFieldType.Text),
                    LinkUrl(), BtnAr(), BtnEn(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","المكتبة الرقمية السعودية","Title_EN","Saudi Digital Library","Description","أكثر من 680 ألف كتاب إلكتروني، وأكثر من 200 ألف رسالة علمية، وملايين المقالات والدوريات وقواعد البيانات المتخصصة.","Description_EN","Over 680k e-books, 200k dissertations, and millions of specialized articles and databases.","BadgeText","رقمية","BadgeText_EN","Digital","IconClass","hgi-computer-cloud","LinkUrl","https://sdl.edu.sa/, المكتبة الرقمية السعودية","ButtonText","انتقال إلى المنصة","ButtonText_EN","Go to platform","ItemOrder","1"),
                    Row("Title","حجز خدمات المكتبة المركزية","Title_EN","Central Library Booking Services","Description","حجز الخلوات البحثية والقاعات التدريبية والدراسية وقاعات الاجتماع والجولات التعريفية وطلبات الزيارة.","Description_EN","Book research retreats, training/study rooms, meeting halls, and guided tours.","BadgeText","حجز","BadgeText_EN","Booking","IconClass","hgi-calendar-check-in-01","LinkUrl","mailto:dla-us@pnu.edu.sa, طلب الخدمة","ButtonText","طلب الخدمة","ButtonText_EN","Request service","ItemOrder","2"),
                    Row("Title","خدمة إصدار بطاقة العضوية","Title_EN","Membership Card Service","Description","خدمة مجتمعية للمستفيدين من خارج الجامعة تمنح بطاقة عضوية تتيح الاستعارة وفق ضوابط طلب العضوية.","Description_EN","Community service offering membership cards for external visitors to borrow books.","BadgeText","خدمة مجتمعية","BadgeText_EN","Community service","IconClass","hgi-user-id-verification","LinkUrl","mailto:dla-us@pnu.edu.sa, طلب العضوية","ButtonText","طلب العضوية","ButtonText_EN","Apply for membership","ItemOrder","3"),
                    Row("Title","خدمات الإعارة","Title_EN","Borrowing Services","Description","تشمل إخلاء الطرف وحجز الكتب والاستعلام عن الكتب المستعارة.","Description_EN","Includes clearance, book reservation, and checking borrowed books status.","BadgeText","إعارة","BadgeText_EN","Borrowing","IconClass","hgi-book-upload","LinkUrl","https://library.pnu.edu.sa/, خدمات الإعارة","ButtonText","انتقال إلى الفهرس","ButtonText_EN","Go to catalog","ItemOrder","4"),
                    Row("Title","خدمة الفهرس الآلي لمكتبات الجامعة","Title_EN","Automated Catalog Service","Description","تتيح الاطلاع على الكتب التي وصلت حديثًا واقتراح كتاب والاستفادة من خدمة البث الانتقائي.","Description_EN","Browse newly arrived books, suggest new titles, and benefit from selective dissemination of information.","BadgeText","فهرس","BadgeText_EN","Catalog","IconClass","hgi-catalogue","LinkUrl","https://library.pnu.edu.sa/, خدمة الفهرس الآلي","ButtonText","فتح الفهرس","ButtonText_EN","Open catalog","ItemOrder","5"),
                    Row("Title","خدمة الإهداء","Title_EN","Gift & Donation Service","Description","تستقبل طلبات إهداء الكتب والرسائل الجامعية وفق الإجراءات المعتمدة.","Description_EN","Receives book and thesis donation requests following approved procedures.","BadgeText","إهداء","BadgeText_EN","Donation","IconClass","hgi-gift","LinkUrl","mailto:dla-taa@pnu.edu.sa, تواصل معنا","ButtonText","تواصل معنا","ButtonText_EN","Contact us","ItemOrder","6")
                }
            };

            // 6) المجموعات والمصادر (Collections)
            map[ClListNames.Collections] = new ClListDef
            {
                Name = ClListNames.Collections,
                TitleAr = "المجموعات والمصادر", TitleEn = "Collections & Resources",
                Description = "Library collections and shelf locations.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), SubTitleAr(), SubTitleEn(),
                    new ClFieldDef("LocationLabel", "نص كلمة الموقع (عربي)", "Location Label (AR)", SPFieldType.Text),
                    new ClFieldDef("LocationLabel_EN", "نص كلمة الموقع (إنجليزي)", "Location Label (EN)", SPFieldType.Text),
                    Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","المخطوطات والكتب النادرة","Title_EN","Manuscripts & Rare Books","Description","تحتوي على 93 مخطوطة مختلفة الموضوعات ما بين القرآن الكريم والأحاديث الشريفة والتفسير والفقه والبلاغة","Description_EN","Contains 93 rare manuscripts covering Quran, Hadith, Tafsir, Fiqh, and Rhetoric.","SubTitle","0.402","SubTitle_EN","0.402","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","1"),
                    Row("Title","الأدب واللغات (عربي)","Title_EN","Literature & Languages (Arabic)","Description","كتب اللغات والأدب باللغة العربية","Description_EN","Books on language and literature in Arabic.","SubTitle","2.101","SubTitle_EN","2.101","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","2"),
                    Row("Title","الأدب واللغات (أجنبي)","Title_EN","Literature & Languages (Foreign)","Description","كتب اللغات وكتب الأدب باللغات الأجنبية","Description_EN","Books on language and literature in foreign languages.","SubTitle","2.101","SubTitle_EN","2.101","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","3"),
                    Row("Title","المعارف العامة والفلسفة وعلم النفس","Title_EN","General Knowledge, Philosophy & Psychology","Description","كتب في موضوعات المعارف العامة والفلسفة وعلم النفس وفروعها","Description_EN","Books on general knowledge, philosophy, psychology and related disciplines.","SubTitle","3.300","SubTitle_EN","3.300","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","4"),
                    Row("Title","الديانات (عربي وأجنبي)","Title_EN","Religions (Arabic & Foreign)","Description","كتب الديانات باللغتين العربية والإنجليزية","Description_EN","Religious books in Arabic and English.","SubTitle","4.101","SubTitle_EN","4.101","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","5"),
                    Row("Title","العلوم البحتة والتطبيقية (عربي)","Title_EN","Pure & Applied Sciences (Arabic)","Description","الفيزياء والكيمياء والرياضيات والأحياء والعلوم التطبيقية","Description_EN","Physics, chemistry, mathematics, biology, and applied sciences.","SubTitle","5.201","SubTitle_EN","5.201","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","6"),
                    Row("Title","العلوم البحتة والتطبيقية (أجنبي)","Title_EN","Pure & Applied Sciences (Foreign)","Description","الفيزياء والكيمياء والرياضيات والأحياء والعلوم التطبيقية","Description_EN","Physics, chemistry, mathematics, biology, and applied sciences in foreign languages.","SubTitle","5.101","SubTitle_EN","5.101","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","7"),
                    Row("Title","العلوم الاجتماعية (عربي وأجنبي)","Title_EN","Social Sciences (Arabic & Foreign)","Description","التربية والقانون والإدارة العامة","Description_EN","Education, law, and public administration.","SubTitle","5.300","SubTitle_EN","5.300","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","8"),
                    Row("Title","الفنون (عربي وأجنبي)","Title_EN","Arts (Arabic & Foreign)","Description","الفنون والتصاميم والعمارة والتصوير والرسم","Description_EN","Arts, design, architecture, photography, and drawing.","SubTitle","6.101","SubTitle_EN","6.101","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","9"),
                    Row("Title","التاريخ والجغرافيا (عربي وأجنبي)","Title_EN","History & Geography (Arabic & Foreign)","Description","كتب في مجال التاريخ والجغرافيا وتراجم السير","Description_EN","Books on history, geography, and biographies.","SubTitle","6.300","SubTitle_EN","6.300","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","10"),
                    Row("Title","الرسائل الجامعية","Title_EN","Academic Theses","Description","أطروحات الماجستير والدكتوراه باللغة العربية واللغات الأجنبية","Description_EN","Master's and PhD dissertations in Arabic and foreign languages.","SubTitle","6.402","SubTitle_EN","6.402","LocationLabel","الموقع","LocationLabel_EN","Location","IconClass","hgi-books-02","ItemOrder","11")
                }
            };

            // 7) المرافق والخدمات (Facilities)
            map[ClListNames.Facilities] = new ClListDef
            {
                Name = ClListNames.Facilities,
                TitleAr = "المرافق والخدمات", TitleEn = "Facilities & Services",
                Description = "Library facility cards with images under /Style Library/DGA/public/images/ and feature tags.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), ImageUrl(),
                    new ClFieldDef("BadgeText", "الوسوم مفصولة بفواصل (عربي)", "Badges separated by commas (AR)", SPFieldType.Text, true),
                    new ClFieldDef("BadgeText_EN", "الوسوم مفصولة بفواصل (إنجليزي)", "Badges separated by commas (EN)", SPFieldType.Text),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","قاعات القراءة","Title_EN","Reading Halls","Description","مساحات هادئة ومريحة للقراءة والدراسة.","Description_EN","Quiet and comfortable spaces for reading and studying.","BadgeText","إضاءة مريحة, تكييف مركزي, مقاعد مريحة","BadgeText_EN","Comfortable lighting, Central AC, Comfortable seating","ImageUrl","/Style Library/DGA/public/images/hero/hero-library-lg.webp","ItemOrder","1"),
                    Row("Title","قاعات تدريبية","Title_EN","Training Halls","Description","غرف مجهزة لاستضافة الورش والبرامج التدريبية.","Description_EN","Fully equipped rooms to host workshops and training programs.","BadgeText","شاشات عرض, سبورات ذكية, حجز مسبق","BadgeText_EN","Display screens, Smart boards, Advance booking","ImageUrl","/Style Library/DGA/public/images/hero/library.png","ItemOrder","2"),
                    Row("Title","معمل الحاسب","Title_EN","Computer Lab","Description","أجهزة حاسب مهيأة لدعم البحث والوصول إلى المصادر الرقمية.","Description_EN","Computers equipped to support research and digital access.","BadgeText","إنترنت مجاني, مقاعد مريحة, دعم فني","BadgeText_EN","Free Wi-Fi, Comfortable seating, Tech support","ImageUrl","/Style Library/DGA/public/images/hero/hero-library-md.webp","ItemOrder","3"),
                    Row("Title","منطقة الاستراحة","Title_EN","Lounge Area","Description","مساحة مخصصة للراحة والاسترخاء داخل المكتبة.","Description_EN","Dedicated area for relaxation inside the library.","BadgeText","مشروبات ساخنة, وجبات خفيفة, جلسات مريحة","BadgeText_EN","Hot drinks, Snacks, Comfortable seating","ImageUrl","/Style Library/DGA/public/images/hero/hero-library-sm.webp","ItemOrder","4"),
                    Row("Title","خدمة النسخ الضوئي","Title_EN","Photocopying Service","Description","معدات للنسخ والطباعة والمسح الضوئي.","Description_EN","Equipment for copying, printing, and scanning.","BadgeText","مسح ضوئي, نسخ, طباعة","BadgeText_EN","Scanning, Copying, Printing","ImageUrl","/Style Library/DGA/public/images/hero/hero-library-lg.jpg","ItemOrder","5")
                }
            };

            // 8) الأسئلة الشائعة (FAQ)
            map[ClListNames.Faq] = new ClListDef
            {
                Name = ClListNames.Faq,
                TitleAr = "الأسئلة الشائعة", TitleEn = "Frequently Asked Questions",
                Description = "FAQ accordion list for Central Library.",
                Fields = new List<ClFieldDef> { TitleEn(), DescAr(), DescEn(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","ما هي أوقات عمل المكتبة؟","Title_EN","What are the library opening hours?","Description","تقدم المكتبة المركزية خدماتها من الأحد إلى الخميس من الساعة 08:00 صباحًا حتى الساعة 08:00 مساءً للنساء، بينما تكون متاحة للنساء والرجال يوم السبت من الساعة 10:00 صباحًا حتى الساعة 04:00 مساءً. وتقدم المكتبات الفرعية خدماتها من الأحد إلى الخميس من الساعة 08:00 صباحًا حتى الساعة 02:00 مساءً.","Description_EN","Central Library is open Sun-Thu 8am-8pm for women, Sat 10am-4pm for men and women. Branch libraries open Sun-Thu 8am-2pm.","ItemOrder","1"),
                    Row("Title","كيف يمكن الوصول إلى المكتبة المركزية؟","Title_EN","How to reach the Central Library?","Description","تقع المكتبة المركزية في جامعة الأميرة نورة بنت عبدالرحمن مقابل محطة A9.","Description_EN","Located at PNU opposite station A9.","ItemOrder","2"),
                    Row("Title","هل يسمح بالدخول إلى المكتبة المركزية من غير منسوبي الجامعة؟","Title_EN","Can non-PNU members enter the Central Library?","Description","نعم، مع إبراز الهوية أو الإقامة.","Description_EN","Yes, upon presenting national ID or Iqama.","ItemOrder","3"),
                    Row("Title","ما سياسة استعارة الكتب؟","Title_EN","What is the book borrowing policy?","Description","تسمح سياسة الإعارة لكل مستفيدة باستعارة ما يصل إلى عشرة كتب، ومدة الإعارة شهر للطالبات وفصل دراسي واحد للأعضاء. يجب إرجاع الكتب في الوقت المحدد، وفي حال التأخر تطبق غرامة مالية.","Description_EN","Up to 10 books. Duration is 1 month for students and 1 semester for faculty.","ItemOrder","4"),
                    Row("Title","هل يمكنني تجديد استعارة الكتاب؟","Title_EN","Can I renew borrowed books?","Description","نعم، يمكن تجديد استعارة الكتاب مرة واحدة فقط.","Description_EN","Yes, books can be renewed once.","ItemOrder","5"),
                    Row("Title","ما الكتب التي لا يمكن استعارتها؟","Title_EN","Which books cannot be borrowed?","Description","نسخة الكتاب الموجودة على الرف (c1) مخصصة للاطلاع فقط، وتوجد نسخ منها في الذراع الآلي يمكن استعارتها، كما لا تعار المعاجم والموسوعات.","Description_EN","Shelf copy (c1) is for reference only; extra copies are available via the automated arm. Dictionaries & encyclopedias cannot be borrowed.","ItemOrder","6"),
                    Row("Title","كيف أبحث عن كتاب أو موضوع معين؟","Title_EN","How do I search for a book?","Description","يمكن البحث من خلال فهرس مكتبات الجامعة الإلكتروني على library.pnu.edu.sa.","Description_EN","You can search through the university library catalog at library.pnu.edu.sa.","ItemOrder","7"),
                    Row("Title","كيف تحتسب الغرامات؟","Title_EN","How are late fines calculated?","Description","تحتسب الغرامة بواقع ريال واحد عن كل يوم تأخير لمدة ثلاثة أشهر، وبعد مضيها يعد الكتاب مفقودًا. عند إعادة الكتاب تدفع غرامة أربعة أشهر وقيمة تجليد الكتاب، وفي حالة فقدانه تضاف قيمة الكتاب و35 ريالًا للباركود والمغنطة.","Description_EN","1 SAR per day for up to 3 months. After that the book is considered lost.","ItemOrder","8"),
                    Row("Title","أين يمكن تسديد غرامات المكتبة؟","Title_EN","Where can library fines be paid?","Description","يمكن تسديد الغرامات في قاعات القراءة بالمكتبة المركزية أو في المكتبات الفرعية. وللاستفسار يمكن التواصل مع وحدة خدمات المستفيدين عبر dla-cl@pnu.edu.sa.","Description_EN","Fines can be paid in reading halls or branch libraries, or email dla-cl@pnu.edu.sa.","ItemOrder","9"),
                    Row("Title","ما إجراءات الحصول على عضوية في المكتبة؟","Title_EN","What are the procedures to get a library membership?","Description","يمكن للمستفيدين من خارج الجامعة الاستفادة من خدمات المكتبة المركزية عبر تعبئة النموذج المطلوب. رسوم إصدار البطاقة 50 ريالًا سعوديًا، مع تأمين بقيمة 500 ريال سعودي. للاستفسار: dla-us@pnu.edu.sa.","Description_EN","External visitors can apply for membership. Card fee: 50 SAR + 500 SAR deposit. Contact dla-us@pnu.edu.sa.","ItemOrder","10"),
                    Row("Title","هل توفر المكتبة جولات تعريفية؟","Title_EN","Does the library offer guided tours?","Description","نعم، ويمكن طلبها عبر البريد الإلكتروني dsrl@pnu.edu.sa وتعبئة النموذج المطلوب.","Description_EN","Yes, guided tours can be requested via email dsrl@pnu.edu.sa.","ItemOrder","11"),
                    Row("Title","كيف يمكنني حجز خلوة بحثية في المكتبة؟","Title_EN","How can I book a research retreat room?","Description","يمكن تقديم طلب الحجز عبر dla-us@pnu.edu.sa وتعبئة النموذج المطلوب. يبلغ تأمين الخدمة 200 ريال سعودي، ومدة الاستفادة شهر واحد مع إمكانية التمديد لشهر إضافي.","Description_EN","Submit booking request via dla-us@pnu.edu.sa. Deposit: 200 SAR, duration 1 month extendable for another month.","ItemOrder","12")
                }
            };

            // 9) معلومات الزيارة والتواصل (Contact)
            map[ClListNames.Contact] = new ClListDef
            {
                Name = ClListNames.Contact,
                TitleAr = "معلومات الزيارة والتواصل", TitleEn = "Visit Info & Contact",
                Description = "Working hours and contact information.",
                Fields = new List<ClFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), Icon(),
                    new ClFieldDef("Row1Label", "السطر 1 - العنوان (عربي)", "Row 1 Label (AR)", SPFieldType.Text),
                    new ClFieldDef("Row1Label_EN", "السطر 1 - العنوان (إنجليزي)", "Row 1 Label (EN)", SPFieldType.Text),
                    new ClFieldDef("Row1Value", "السطر 1 - القيمة (عربي)", "Row 1 Value (AR)", SPFieldType.Text),
                    new ClFieldDef("Row1Value_EN", "السطر 1 - القيمة (إنجليزي)", "Row 1 Value (EN)", SPFieldType.Text),
                    new ClFieldDef("Row2Label", "السطر 2 - العنوان (عربي)", "Row 2 Label (AR)", SPFieldType.Text),
                    new ClFieldDef("Row2Label_EN", "السطر 2 - العنوان (إنجليزي)", "Row 2 Label (EN)", SPFieldType.Text),
                    new ClFieldDef("Row2Value", "السطر 2 - القيمة (عربي)", "Row 2 Value (AR)", SPFieldType.Text),
                    new ClFieldDef("Row2Value_EN", "السطر 2 - القيمة (إنجليزي)", "Row 2 Value (EN)", SPFieldType.Text),
                    new ClFieldDef("Row3Label", "السطر 3 - العنوان (عربي)", "Row 3 Label (AR)", SPFieldType.Text),
                    new ClFieldDef("Row3Label_EN", "السطر 3 - العنوان (إنجليزي)", "Row 3 Label (EN)", SPFieldType.Text),
                    new ClFieldDef("Row3Value", "السطر 3 - القيمة (عربي)", "Row 3 Value (AR)", SPFieldType.Text),
                    new ClFieldDef("Row3Value_EN", "السطر 3 - القيمة (إنجليزي)", "Row 3 Value (EN)", SPFieldType.Text),
                    new ClFieldDef("Email", "البريد الإلكتروني", "Email", SPFieldType.Text),
                    new ClFieldDef("Phone1", "الهاتف الأول", "Phone 1", SPFieldType.Text),
                    new ClFieldDef("Phone1Tel", "رابط الهاتف الأول (tel:)", "Phone 1 Tel", SPFieldType.Text),
                    new ClFieldDef("Phone2", "الهاتف الثاني", "Phone 2", SPFieldType.Text),
                    new ClFieldDef("Phone2Tel", "رابط الهاتف الثاني (tel:)", "Phone 2 Tel", SPFieldType.Text),
                    new ClFieldDef("LocationText", "ملاحظة الموقع (عربي)", "Location Note (AR)", SPFieldType.Text),
                    new ClFieldDef("LocationText_EN", "ملاحظة الموقع (إنجليزي)", "Location Note (EN)", SPFieldType.Text),
                    LinkUrl(), BtnAr(), BtnEn(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","أوقات العمل","Title_EN","Working Hours",
                        "IconClass","hgi-clock-01",
                        "Row1Label","الأحد - الخميس","Row1Label_EN","Sunday - Thursday","Row1Value","8:00 ص - 8:00 م","Row1Value_EN","8:00 AM - 8:00 PM",
                        "Row2Label","الجمعة","Row2Label_EN","Friday","Row2Value","مغلق","Row2Value_EN","Closed",
                        "Row3Label","السبت","Row3Label_EN","Saturday","Row3Value","10:00 ص - 4:00 م (رجال ونساء)","Row3Value_EN","10:00 AM - 4:00 PM (Men & Women)",
                        "ItemOrder","1"),
                    Row("Title","اتصل بنا","Title_EN","Contact Us",
                        "IconClass","hgi-customer-support",
                        "Email","dla-taa@pnu.edu.sa",
                        "Phone1","011 824 4430","Phone1Tel","+966118244430",
                        "Phone2","011 824 4424","Phone2Tel","+966118244424",
                        "LocationText","مبنى المكتبة المركزية، الدوران الأول والثاني، مقابل محطة A9",
                        "LocationText_EN","Central Library Building, 1st & 2nd turn, opposite Station A9",
                        "ButtonText","راسل المكتبة","ButtonText_EN","Email the library",
                        "LinkUrl","mailto:dla-taa@pnu.edu.sa",
                        "ItemOrder","2")
                }
            };

            return map;
        }
    }
}
