using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public class IntlFieldDef
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

        public IntlFieldDef(string name, string ar, string en, SPFieldType type,
                            bool inGrid = false, int rows = 0, string hint = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint;
        }

        public string Display { get { return IntlHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class IntlListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<IntlFieldDef> Fields { get; set; }
        /// <summary>Rows seeded on first provision (field-name => value maps).</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return IntlHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for every International Students list: used by the
    /// provisioner to create the lists and by ucIntlAdmin to render the CRUD form.
    /// Title is always the Arabic title of the item (built-in field, re-labelled).
    /// </summary>
    public static class IntlListSchema
    {
        private static Dictionary<string, IntlListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, IntlListDef> All
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

        public static IntlListDef Get(string listName)
        {
            IntlListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        // ---- shared field builders -------------------------------------------------
        private static IntlFieldDef TitleEn()  { return new IntlFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static IntlFieldDef DescAr()   { return new IntlFieldDef("Description", "الوصف (عربي)", "Description (AR)", SPFieldType.Note, false, 3); }
        private static IntlFieldDef DescEn()   { return new IntlFieldDef("Description_EN", "الوصف (إنجليزي)", "Description (EN)", SPFieldType.Note, false, 3); }
        private static IntlFieldDef Icon()     { return new IntlFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, true, 0, "hgi-global, hgi-school, hgi-airplane-01 ..."); }
        private static IntlFieldDef Order()    { return new IntlFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static IntlFieldDef LinkUrl()  { return new IntlFieldDef("LinkUrl", "الرابط", "Link URL", SPFieldType.URL); }
        private static IntlFieldDef BulletsAr(){ return new IntlFieldDef("Bullets", "النقاط (عربي - سطر لكل نقطة)", "Bullets (AR - one per line)", SPFieldType.Note, false, 5); }
        private static IntlFieldDef BulletsEn(){ return new IntlFieldDef("Bullets_EN", "النقاط (إنجليزي - سطر لكل نقطة)", "Bullets (EN - one per line)", SPFieldType.Note, false, 5); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, IntlListDef> Build()
        {
            var map = new Dictionary<string, IntlListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) الطلاب الدوليون في أرقام
            map[IntlListNames.Numbers] = new IntlListDef
            {
                Name = IntlListNames.Numbers,
                TitleAr = "الطلاب الدوليون في أرقام", TitleEn = "International students in numbers",
                Description = "Statistic counters shown on the International Students page.",
                Fields = new List<IntlFieldDef>
                {
                    TitleEn(),
                    new IntlFieldDef("StatValue", "القيمة", "Value", SPFieldType.Text, true, 0, "70+, 906, 50+"),
                    Icon(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","دولة ممثلة","Title_EN","Countries represented","StatValue","70+","IconClass","hgi-global","ItemOrder","1"),
                    Row("Title","طالبًا وطالبة","Title_EN","Students","StatValue","906","IconClass","hgi-user-group","ItemOrder","2"),
                    Row("Title","برنامجًا أكاديميًا","Title_EN","Academic programs","StatValue","50+","IconClass","hgi-book-02","ItemOrder","3")
                }
            };

            // 2) ما يميز الدراسة في جامعة نورة
            map[IntlListNames.Features] = new IntlListDef
            {
                Name = IntlListNames.Features,
                TitleAr = "ما يميز الدراسة في جامعة نورة", TitleEn = "Why study at PNU",
                Description = "Feature cards on the International Students page.",
                Fields = new List<IntlFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","بيئة تعليمية متميزة","Title_EN","Outstanding learning environment","Description","برامج تعليمية عالية الجودة ومعتمدة دوليًا.","Description_EN","High quality, internationally accredited programs.","IconClass","hgi-school","ItemOrder","1"),
                    Row("Title","تنوع ثقافي ثري","Title_EN","Rich cultural diversity","Description","مجتمع طلابي من أكثر من 70 دولة حول العالم.","Description_EN","A student community from more than 70 countries.","IconClass","hgi-language-skill","ItemOrder","2"),
                    Row("Title","دعم شامل ومستمر","Title_EN","Comprehensive ongoing support","Description","خدمات دعم أكاديمي ونفسي واجتماعي طوال الرحلة.","Description_EN","Academic, psychological and social support throughout the journey.","IconClass","hgi-favourite","ItemOrder","3"),
                    Row("Title","مرافق حديثة ومتطورة","Title_EN","Modern facilities","Description","حرم جامعي مجهز بأحدث التقنيات والمرافق.","Description_EN","A campus equipped with the latest technologies and facilities.","IconClass","hgi-building-03","ItemOrder","4"),
                    Row("Title","فرص للتبادل الدولي","Title_EN","International exchange opportunities","Description","برامج تبادل طلابي مع جامعات عالمية.","Description_EN","Student exchange programs with global universities.","IconClass","hgi-airplane-01","ItemOrder","5"),
                    Row("Title","موقع استراتيجي","Title_EN","Strategic location","Description","في قلب العاصمة الرياض بالمملكة العربية السعودية.","Description_EN","In the heart of Riyadh, Saudi Arabia.","IconClass","hgi-location-01","ItemOrder","6")
                }
            };

            // 3) التقديم والقبول  (bullet cards + optional nav card)
            map[IntlListNames.Admission] = new IntlListDef
            {
                Name = IntlListNames.Admission,
                TitleAr = "التقديم والقبول", TitleEn = "Admission",
                Description = "Admission requirements, documents and key dates.",
                Fields = new List<IntlFieldDef> { TitleEn(), DescAr(), DescEn(), BulletsAr(), BulletsEn(), Icon(), LinkUrl(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","متطلبات القبول","Title_EN","Admission requirements","IconClass","hgi-file-02","ItemOrder","1",
                        "Bullets","شهادة ثانوية عامة أو ما يعادلها\nتحقيق شروط البرنامج المختار\nاجتياز المقابلة أو الاختبارات المطلوبة",
                        "Bullets_EN","High school certificate or equivalent\nMeeting the selected program requirements\nPassing the interview or required tests"),
                    Row("Title","المستندات المطلوبة","Title_EN","Required documents","IconClass","hgi-certificate-01","ItemOrder","2",
                        "Bullets","نسخة من جواز السفر\nالشهادات الأكاديمية\nالسجل الأكاديمي\nإثبات إجادة اللغة",
                        "Bullets_EN","Copy of passport\nAcademic certificates\nAcademic transcript\nProof of language proficiency"),
                    Row("Title","المواعيد المهمة","Title_EN","Key dates","IconClass","hgi-calendar-02","ItemOrder","3",
                        "Description","تابع مواعيد فتح بوابة القبول والإعلانات الرسمية.",
                        "Description_EN","Follow the admission portal opening dates and official announcements.",
                        "LinkUrl","#, المواعيد المهمة")
                }
            };

            // 4) قبل الوصول إلى المملكة (nav cards)
            map[IntlListNames.BeforeArrival] = new IntlListDef
            {
                Name = IntlListNames.BeforeArrival,
                TitleAr = "قبل الوصول إلى المملكة", TitleEn = "Before arriving in the Kingdom",
                Description = "Navigation cards shown before arrival.",
                Fields = new List<IntlFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), LinkUrl(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","دليل التأشيرة والإقامة","Title_EN","Visa and residency guide","Description","إرشادات استخراج التأشيرة والإقامة والوثائق اللازمة.","Description_EN","Guidance on visas, residency and required documents.","IconClass","hgi-file-02","LinkUrl","#, دليل التأشيرة والإقامة","ItemOrder","1"),
                    Row("Title","السكن الجامعي","Title_EN","University housing","Description","معلومات شاملة عن السكن والخدمات المتوفرة.","Description_EN","Complete information about housing and available services.","IconClass","hgi-building-03","LinkUrl","#, السكن الجامعي","ItemOrder","2"),
                    Row("Title","إجراءات ما قبل السفر","Title_EN","Pre-travel checklist","Description","قائمة مرجعية تساعدك على الاستعداد قبل الوصول.","Description_EN","A checklist to help you prepare before arrival.","IconClass","hgi-airplane-01","LinkUrl","#, إجراءات ما قبل السفر","ItemOrder","3")
                }
            };

            // 5) عند الوصول للجامعة
            map[IntlListNames.OnArrival] = new IntlListDef
            {
                Name = IntlListNames.OnArrival,
                TitleAr = "عند الوصول للجامعة", TitleEn = "On arrival at the university",
                Description = "Steps completed after arriving at the university.",
                Fields = new List<IntlFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","استقبال من المطار","Title_EN","Airport reception","Description","خدمة استقبال وتنظيم الوصول للجامعة.","Description_EN","Reception service and arrival arrangements.","IconClass","hgi-airplane-01","ItemOrder","1"),
                    Row("Title","التسجيل الأكاديمي","Title_EN","Academic registration","Description","إتمام إجراءات التسجيل وبداية المسار الأكاديمي.","Description_EN","Completing registration and starting the academic path.","IconClass","hgi-school","ItemOrder","2"),
                    Row("Title","لقاء التعريف","Title_EN","Orientation meeting","Description","برنامج تعريفي بالجامعة وخدماتها.","Description_EN","An orientation program about the university and its services.","IconClass","hgi-user-group","ItemOrder","3"),
                    Row("Title","فتح حساب بنكي","Title_EN","Opening a bank account","Description","المساعدة في فتح حساب بنكي محلي.","Description_EN","Assistance opening a local bank account.","IconClass","hgi-dollar-01","ItemOrder","4"),
                    Row("Title","بطاقة الطالبة","Title_EN","Student ID card","Description","إصدار البطاقة الجامعية.","Description_EN","Issuing the university ID card.","IconClass","hgi-certificate-01","ItemOrder","5"),
                    Row("Title","جولة في الحرم الجامعي","Title_EN","Campus tour","Description","جولة تعريفية بأهم المواقع والمرافق.","Description_EN","A guided tour of key locations and facilities.","IconClass","hgi-location-01","ItemOrder","6")
                }
            };

            // 6) مكتب الطلاب الدوليين (bullet cards)
            map[IntlListNames.Office] = new IntlListDef
            {
                Name = IntlListNames.Office,
                TitleAr = "مكتب الطلاب الدوليين", TitleEn = "International Students Office",
                Description = "Support services offered by the International Students Office.",
                Fields = new List<IntlFieldDef> { TitleEn(), BulletsAr(), BulletsEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الدعم الأكاديمي","Title_EN","Academic support","IconClass","hgi-book-open-01","ItemOrder","1",
                        "Bullets","الإرشاد الأكاديمي المتخصص\nالمساعدة في اختيار المقررات\nدعم التعلم والتكيف الأكاديمي",
                        "Bullets_EN","Specialised academic advising\nHelp choosing courses\nLearning and academic adaptation support"),
                    Row("Title","الدعم الإداري","Title_EN","Administrative support","IconClass","hgi-building-03","ItemOrder","2",
                        "Bullets","تجديد التأشيرات والإقامات\nإنهاء الإجراءات الجامعية\nالمساعدة في الخدمات العامة",
                        "Bullets_EN","Visa and residency renewal\nCompleting university procedures\nHelp with general services"),
                    Row("Title","الدعم النفسي والاجتماعي","Title_EN","Psychological and social support","IconClass","hgi-favourite","ItemOrder","3",
                        "Bullets","برامج الصحة النفسية\nالاستشارات الفردية\nأنشطة التكيف والاندماج",
                        "Bullets_EN","Mental health programs\nIndividual counselling\nAdaptation and integration activities")
                }
            };

            // 7) الأنشطة والفعاليات
            map[IntlListNames.Activities] = new IntlListDef
            {
                Name = IntlListNames.Activities,
                TitleAr = "الأنشطة والفعاليات", TitleEn = "Activities and events",
                Description = "Activities and events for international students.",
                Fields = new List<IntlFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الأنشطة الثقافية","Title_EN","Cultural activities","Description","فعاليات ثقافية متنوعة تعكس التبادل بين الثقافات.","Description_EN","Diverse cultural events reflecting cross-cultural exchange.","IconClass","hgi-airplane-01","ItemOrder","1"),
                    Row("Title","زيارات ميدانية","Title_EN","Field visits","Description","رحلات تعليمية وثقافية داخل المملكة.","Description_EN","Educational and cultural trips inside the Kingdom.","IconClass","hgi-location-01","ItemOrder","2"),
                    Row("Title","برامج التأقلم","Title_EN","Adaptation programs","Description","برامج تساعدك على التكيف مع البيئة الجامعية.","Description_EN","Programs that help you adapt to university life.","IconClass","hgi-global","ItemOrder","3"),
                    Row("Title","الأنشطة الرياضية","Title_EN","Sports activities","Description","مسابقات وأنشطة رياضية متنوعة.","Description_EN","A variety of sports competitions and activities.","IconClass","hgi-chart-line-data-01","ItemOrder","4"),
                    Row("Title","ورش العمل","Title_EN","Workshops","Description","ورش تطويرية ومهارية متخصصة.","Description_EN","Specialised development and skills workshops.","IconClass","hgi-target-02","ItemOrder","5"),
                    Row("Title","الاحتفالات الخاصة","Title_EN","Special celebrations","Description","احتفالات بالمناسبات الوطنية والثقافية.","Description_EN","Celebrations of national and cultural occasions.","IconClass","hgi-star","ItemOrder","6")
                }
            };

            // 8) إدارة التبادل الطلابي (4-up grid)
            map[IntlListNames.Exchange] = new IntlListDef
            {
                Name = IntlListNames.Exchange,
                TitleAr = "إدارة التبادل الطلابي", TitleEn = "Student Exchange Department",
                Description = "Student exchange programs and partner universities.",
                Fields = new List<IntlFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","برامج التبادل الطلابي","Title_EN","Student exchange programs","Description","فرص دراسية في جامعات شريكة حول العالم.","Description_EN","Study opportunities at partner universities worldwide.","IconClass","hgi-global","ItemOrder","1"),
                    Row("Title","الجامعات الشريكة","Title_EN","Partner universities","Description","شبكة واسعة من الجامعات في أكثر من 70 دولة.","Description_EN","A wide network of universities in more than 70 countries.","IconClass","hgi-school","ItemOrder","2"),
                    Row("Title","شروط المشاركة","Title_EN","Participation conditions","Description","معايير القبول والمتطلبات الأكاديمية للمشاركة.","Description_EN","Acceptance criteria and academic requirements.","IconClass","hgi-check-list","ItemOrder","3"),
                    Row("Title","فرص التمويل","Title_EN","Funding opportunities","Description","منح ومساعدات مالية لدعم برامج التبادل.","Description_EN","Grants and financial aid supporting exchange programs.","IconClass","hgi-dollar-01","ItemOrder","4")
                }
            };

            // 9) البرامج والمستندات (download/CTA cards)
            map[IntlListNames.Documents] = new IntlListDef
            {
                Name = IntlListNames.Documents,
                TitleAr = "البرامج والمستندات", TitleEn = "Programs and documents",
                Description = "Downloadable forms and reference documents.",
                Fields = new List<IntlFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), Icon(), LinkUrl(),
                    new IntlFieldDef("ButtonText", "نص الزر (عربي)", "Button text (AR)", SPFieldType.Text),
                    new IntlFieldDef("ButtonText_EN", "نص الزر (إنجليزي)", "Button text (EN)", SPFieldType.Text),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","نموذج طلب السكن","Title_EN","Housing application form","Description","تحميل نموذج طلب السكن الجامعي.","Description_EN","Download the university housing application form.","IconClass","hgi-download-01","LinkUrl","#, نموذج طلب السكن","ButtonText","فتح","ButtonText_EN","Open","ItemOrder","1"),
                    Row("Title","قائمة الجامعات الشريكة","Title_EN","Partner universities list","Description","استعراض قائمة الجامعات المشاركة في التبادل.","Description_EN","Browse the list of partner exchange universities.","IconClass","hgi-global","LinkUrl","#, قائمة الجامعات الشريكة","ButtonText","استعراض","ButtonText_EN","Browse","ItemOrder","2")
                }
            };

            // 10) تواصل معنا
            map[IntlListNames.Contact] = new IntlListDef
            {
                Name = IntlListNames.Contact,
                TitleAr = "تواصل معنا", TitleEn = "Contact us",
                Description = "Contact details for the International Students Office.",
                Fields = new List<IntlFieldDef>
                {
                    TitleEn(),
                    new IntlFieldDef("ContactValue", "القيمة (عربي)", "Value (AR)", SPFieldType.Text, true),
                    new IntlFieldDef("ContactValue_EN", "القيمة (إنجليزي)", "Value (EN)", SPFieldType.Text),
                    Icon(), LinkUrl(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","البريد الإلكتروني","Title_EN","Email","ContactValue","international@pnu.edu.sa","ContactValue_EN","international@pnu.edu.sa","IconClass","hgi-mail-01","LinkUrl","mailto:international@pnu.edu.sa, البريد الإلكتروني","ItemOrder","1"),
                    Row("Title","الهاتف","Title_EN","Phone","ContactValue","+966 11 824 0000","ContactValue_EN","+966 11 824 0000","IconClass","hgi-call","LinkUrl","tel:+966118240000, الهاتف","ItemOrder","2"),
                    Row("Title","الموقع","Title_EN","Location","ContactValue","مبنى الخدمات الطلابية، الحرم الجامعي","ContactValue_EN","Student Services Building, Main Campus","IconClass","hgi-location-01","ItemOrder","3"),
                    Row("Title","ساعات العمل","Title_EN","Working hours","ContactValue","الأحد إلى الخميس، 8:00 ص – 3:00 م","ContactValue_EN","Sunday to Thursday, 8:00 AM – 3:00 PM","IconClass","hgi-clock-01","ItemOrder","4")
                }
            };

            // Editors allowed to use ucIntlAdmin. Provisioned like any other list so
            // the screen works on a fresh web; an empty list simply authorises nobody,
            // and ManageLists / site admin still gets you in.
            map[IntlListNames.AdminUsers] = new IntlListDef
            {
                Name = IntlListNames.AdminUsers,
                TitleAr = "مسؤولو المحتوى", TitleEn = "Content administrators",
                Description = "Users allowed to manage the International Students content.",
                Fields = new List<IntlFieldDef>
                {
                    new IntlFieldDef("UserAccount", "حساب المستخدم", "User account", SPFieldType.User, true),
                    new IntlFieldDef("Active", "مفعّل", "Active", SPFieldType.Boolean, true),
                    Order()
                },
                Seed = null
            };

            return map;
        }
    }
}
