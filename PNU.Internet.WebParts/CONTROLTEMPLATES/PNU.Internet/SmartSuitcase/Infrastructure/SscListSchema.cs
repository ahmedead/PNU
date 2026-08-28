using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public class SscFieldDef
    {
        public string InternalName { get; set; }
        public string DisplayAr { get; set; }
        public string DisplayEn { get; set; }
        public SPFieldType Type { get; set; }
        public bool InGrid { get; set; }
        public int Rows { get; set; }
        public string Hint { get; set; }

        public SscFieldDef(string name, string ar, string en, SPFieldType type,
                           bool inGrid = false, int rows = 0, string hint = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint;
        }

        public string Display { get { return SscHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class SscListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<SscFieldDef> Fields { get; set; }
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return SscHelper.Pick(TitleAr, TitleEn); } }
    }

    public static class SscListSchema
    {
        private static Dictionary<string, SscListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, SscListDef> All
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

        public static SscListDef Get(string listName)
        {
            SscListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        private static SscFieldDef TitleEn()   { return new SscFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true); }
        private static SscFieldDef DescAr()    { return new SscFieldDef("Description", "الوصف (عربي)", "Description (AR)", SPFieldType.Note, false, 3); }
        private static SscFieldDef DescEn()    { return new SscFieldDef("Description_EN", "الوصف (إنجليزي)", "Description (EN)", SPFieldType.Note, false, 3); }
        private static SscFieldDef Icon()      { return new SscFieldDef("IconClass", "أيقونة (hgi)", "Icon class (hgi)", SPFieldType.Text, true, 0, "hgi-mail-01, hgi-computer-video-call, hgi-cloud ..."); }
        private static SscFieldDef Order()     { return new SscFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true); }
        private static SscFieldDef LinkUrl()   { return new SscFieldDef("LinkUrl", "الرابط", "Link URL", SPFieldType.URL); }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, SscListDef> Build()
        {
            var map = new Dictionary<string, SscListDef>(StringComparer.OrdinalIgnoreCase);

            // 1) Hero Image Banner
            map[SscListNames.Hero] = new SscListDef
            {
                Name = SscListNames.Hero,
                TitleAr = "الصورة التعريفية للحقيبة الذكية", TitleEn = "Smart Suitcase Hero Banner",
                Description = "Hero promotional banner for Smart Suitcase page.",
                Fields = new List<SscFieldDef>
                {
                    new SscFieldDef("ImageUrl", "رابط الصورة", "Image URL", SPFieldType.URL, true),
                    new SscFieldDef("ImageAlt", "النص البديل (عربي)", "Alt Text (AR)", SPFieldType.Text, true),
                    new SscFieldDef("ImageAlt_EN", "النص البديل (إنجليزي)", "Alt Text (EN)", SPFieldType.Text),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title", "صورة تعريفية للحقيبة الذكية",
                        "ImageUrl", "https://cdn.cs.1worldsync.com/syndication/mediaserverredirect/ca2b962c678f2d76b6169f3e69e10176/original.png, تطبيقات وخدمات Microsoft 365 المتاحة ضمن الحقيبة الذكية",
                        "ImageAlt", "تطبيقات وخدمات Microsoft 365 المتاحة ضمن الحقيبة الذكية",
                        "ImageAlt_EN", "Microsoft 365 applications and services available in the Smart Suitcase",
                        "ItemOrder", "1")
                }
            };

            // 2) خدمات الحقيبة الذكية
            map[SscListNames.Services] = new SscListDef
            {
                Name = SscListNames.Services,
                TitleAr = "خدمات الحقيبة الذكية", TitleEn = "Smart Suitcase Services",
                Description = "Microsoft 365 service cards.",
                Fields = new List<SscFieldDef> { TitleEn(), DescAr(), DescEn(), Icon(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","البريد الإلكتروني الجامعي","Title_EN","University Email","Description","يوفر للطالبات بريدًا جامعيًا عبر سحابة مايكروسوفت بسعة تصل إلى 50 جيجابايت وفق الصلاحيات الممنوحة.","Description_EN","Provides students with a university email via Microsoft cloud with up to 50 GB storage according to granted permissions.","IconClass","hgi-mail-01","ItemOrder","1"),
                    Row("Title","Microsoft Teams","Title_EN","Microsoft Teams","Description","يجمع المحادثات والاجتماعات والمكالمات والتعاون في مساحة واحدة للتواصل بين الطالبات ومنسوبي الجامعة.","Description_EN","Brings together chats, meetings, calls, and collaboration in one place for communication between students and university staff.","IconClass","hgi-computer-video-call","ItemOrder","2"),
                    Row("Title","Microsoft 365 للويب","Title_EN","Microsoft 365 for Web","Description","يتيح استخدام Word وExcel وPowerPoint وغيرها من تطبيقات Microsoft 365 مباشرة من المتصفح، مع حفظ الملفات على OneDrive.","Description_EN","Allows using Word, Excel, PowerPoint, and other Microsoft 365 apps directly from the browser, saving files to OneDrive.","IconClass","hgi-presentation-online","ItemOrder","3"),
                    Row("Title","Microsoft Viva Engage","Title_EN","Microsoft Viva Engage","Description","الاسم الحديث لخدمة Yammer، ويوفر مجتمعات ومحادثات لمشاركة المعرفة والتفاعل حول الموضوعات والاهتمامات المشتركة.","Description_EN","The modern name for Yammer, providing communities and conversations for sharing knowledge and engaging around common topics.","IconClass","hgi-user-group","ItemOrder","4"),
                    Row("Title","Microsoft OneDrive","Title_EN","Microsoft OneDrive","Description","يوفر تخزين الملفات ومزامنتها والوصول إليها من أجهزة مختلفة، مع إمكانات المشاركة والتعاون حسب صلاحيات الحساب.","Description_EN","Provides file storage, syncing, and access from different devices, with sharing and collaboration capabilities based on account permissions.","IconClass","hgi-cloud","ItemOrder","5"),
                    Row("Title","Microsoft 365 Apps","Title_EN","Microsoft 365 Apps","Description","الاسم الحديث لخدمة Office ProPlus، وتتيح تنزيل تطبيقات Microsoft 365 وتفعيلها على الأجهزة الشخصية وفق الترخيص الجامعي.","Description_EN","The modern name for Office ProPlus, allowing download and activation of Microsoft 365 apps on personal devices under university license.","IconClass","hgi-office-365","ItemOrder","6")
                }
            };

            // 3) الدخول على الحقيبة
            map[SscListNames.Access] = new SscListDef
            {
                Name = SscListNames.Access,
                TitleAr = "الدخول على الحقيبة", TitleEn = "Access to Suitcase",
                Description = "Login portal and usage guide cards.",
                Fields = new List<SscFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), Icon(), LinkUrl(),
                    new SscFieldDef("ButtonText", "نص الزر (عربي)", "Button Text (AR)", SPFieldType.Text),
                    new SscFieldDef("ButtonText_EN", "نص الزر (إنجليزي)", "Button Text (EN)", SPFieldType.Text),
                    new SscFieldDef("Badge1", "شارة 1 (عربي)", "Badge 1 (AR)", SPFieldType.Text),
                    new SscFieldDef("Badge1_EN", "شارة 1 (إنجليزي)", "Badge 1 (EN)", SPFieldType.Text),
                    new SscFieldDef("Badge2", "شارة 2 (عربي)", "Badge 2 (AR)", SPFieldType.Text),
                    new SscFieldDef("Badge2_EN", "شارة 2 (إنجليزي)", "Badge 2 (EN)", SPFieldType.Text),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","الدخول إلى خدمات Microsoft 365","Title_EN","Access to Microsoft 365 Services",
                        "Description","استخدم حسابك الجامعي للوصول إلى خدمات الحقيبة الذكية وتطبيقاتها السحابية.",
                        "Description_EN","Use your university account to access Smart Suitcase services and cloud applications.",
                        "IconClass","hgi-login-01",
                        "LinkUrl","https://www.office.com/, الدخول إلى خدمات Microsoft 365",
                        "ButtonText","الدخول على الحقيبة","ButtonText_EN","Access Suitcase",
                        "Badge1","طالبات","Badge1_EN","Students",
                        "Badge2","منسوبو الجامعة","Badge2_EN","University Staff",
                        "ItemOrder","1"),
                    Row("Title","دليل الاستخدام","Title_EN","User Guide",
                        "Description","راجع الدليل الإرشادي للتعرف على خطوات استخدام الحقيبة الذكية وخدماتها.",
                        "Description_EN","Review the user guide to learn the steps for using the Smart Suitcase and its services.",
                        "IconClass","hgi-book-open-01",
                        "LinkUrl","https://www.pnu.edu.sa/ar/smart-suitcase/Pages/manual/index.html, دليل الاستخدام",
                        "ButtonText","دليل الاستخدام","ButtonText_EN","User Guide",
                        "ItemOrder","2")
                }
            };

            // 4) الأسئلة الشائعة
            map[SscListNames.Faq] = new SscListDef
            {
                Name = SscListNames.Faq,
                TitleAr = "الأسئلة الشائعة", TitleEn = "Frequently Asked Questions",
                Description = "FAQ questions and answers for Smart Suitcase.",
                Fields = new List<SscFieldDef> { TitleEn(), DescAr(), DescEn(), Order() },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","هل تُوفر خدمة الحقيبة الذكية لكل طالبة؟","Title_EN","Is the Smart Suitcase service provided to every student?",
                        "Description","نعم، تُمنح طالبات الجامعة المنتظمات صلاحيات استخدام الحقيبة الذكية وفق الأنظمة والصلاحيات المعتمدة.",
                        "Description_EN","Yes, regular university students are granted access rights to the Smart Suitcase in accordance with approved regulations and permissions.",
                        "ItemOrder","1"),
                    Row("Title","هل يتغير عنوان البريد الجامعي عند استخدام سحابة مايكروسوفت؟","Title_EN","Does the university email address change when using Microsoft cloud?",
                        "Description","لا، يظل البريد الجامعي للطالبة بصيغة الرقم الجامعي متبوعًا بالنطاق @pnu.edu.sa.",
                        "Description_EN","No, the student's university email remains in the format of student ID followed by @pnu.edu.sa.",
                        "ItemOrder","2"),
                    Row("Title","هل يمكن الدخول إلى البريد الجامعي من الأجهزة المحمولة؟","Title_EN","Can university email be accessed from mobile devices?",
                        "Description","نعم، يمكن استخدام البريد الجامعي وخدمات Microsoft 365 من الأجهزة المحمولة. راجع دليل الاستخدام لمعرفة خطوات الإعداد.",
                        "Description_EN","Yes, university email and Microsoft 365 services can be used from mobile devices. Refer to the user guide for setup steps.",
                        "ItemOrder","3"),
                    Row("Title","ماذا يحدث للحقيبة الذكية عند التخرج أو الانسحاب أو الانتقال إلى جامعة أخرى؟","Title_EN","What happens to the Smart Suitcase upon graduation, withdrawal, or transfer?",
                        "Description","تتوقف الخدمة عند انتهاء صفة الطالبة المنتظمة بسبب التخرج أو الانسحاب أو الانتقال إلى جامعة أخرى.",
                        "Description_EN","The service stops when regular student status ends due to graduation, withdrawal, or transfer to another university.",
                        "ItemOrder","4"),
                    Row("Title","ماذا يحدث للملفات المخزنة بعد انتهاء الاستفادة من الخدمة؟","Title_EN","What happens to stored files after service termination?",
                        "Description","تُحذف الملفات المخزنة تلقائيًا عند التخرج أو الانسحاب أو الانتقال إلى جامعة أخرى؛ لذلك يجب أخذ نسخة احتياطية من الملفات المهمة قبل انتهاء الخدمة.",
                        "Description_EN","Stored files are automatically deleted upon graduation, withdrawal, or transfer; therefore, important files must be backed up before service ends.",
                        "ItemOrder","5"),
                    Row("Title","هل يمكن تنزيل تطبيقات Microsoft 365 على الأجهزة الشخصية؟","Title_EN","Can Microsoft 365 applications be downloaded on personal devices?",
                        "Description","نعم، يمكن تنزيل التطبيقات وتفعيلها على الأجهزة المدعومة وفق الترخيص الجامعي المخصص للمستخدم.",
                        "Description_EN","Yes, applications can be downloaded and activated on supported devices according to the university license assigned to the user.",
                        "ItemOrder","6"),
                    Row("Title","متى تُحذف الرسائل الموجودة في مجلد المحذوفات أو البريد غير الهام؟","Title_EN","When are messages in Deleted Items or Junk Email deleted?",
                        "Description","بحسب المعلومات المنشورة في الصفحة الأصلية، تُحذف هذه الرسائل تلقائيًا بعد 30 يومًا من تاريخ الحذف.",
                        "Description_EN","According to information published on the original page, these messages are automatically deleted 30 days after deletion date.",
                        "ItemOrder","7"),
                    Row("Title","كيف يمكن تغيير كلمة المرور الخاصة بالحساب الجامعي؟","Title_EN","How can the university account password be changed?",
                        "Description","يمكن تغيير كلمة المرور من خلال بوابة تحديث كلمة المرور (https://sts.pnu.edu.sa/adfs/portal/updatepassword).",
                        "Description_EN","Password can be changed through the password update portal (https://sts.pnu.edu.sa/adfs/portal/updatepassword).",
                        "ItemOrder","8")
                }
            };

            // 5) الدعم الفني
            map[SscListNames.Support] = new SscListDef
            {
                Name = SscListNames.Support,
                TitleAr = "الدعم الفني", TitleEn = "Technical Support",
                Description = "Technical support contact channels.",
                Fields = new List<SscFieldDef>
                {
                    TitleEn(), DescAr(), DescEn(), Icon(), LinkUrl(),
                    new SscFieldDef("ContactValue", "البريد / التحويلة (عربي)", "Contact Value (AR)", SPFieldType.Text, true),
                    new SscFieldDef("ContactValue_EN", "البريد / التحويلة (إنجليزي)", "Contact Value (EN)", SPFieldType.Text),
                    new SscFieldDef("ButtonText", "نص الزر (عربي)", "Button Text (AR)", SPFieldType.Text),
                    new SscFieldDef("ButtonText_EN", "نص الزر (إنجليزي)", "Button Text (EN)", SPFieldType.Text),
                    Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title","تحتاج إلى مساعدة؟","Title_EN","Need help?",
                        "Description","في حال وجود أي استفسار بخصوص الحقيبة الذكية، تواصل مع الدعم الفني عبر البريد الإلكتروني أو التحويلة الداخلية.",
                        "Description_EN","If you have any questions regarding Smart Suitcase, contact technical support via email or internal extension.",
                        "IconClass","hgi-customer-support",
                        "ContactValue","ucc@pnu.edu.sa","ContactValue_EN","ucc@pnu.edu.sa",
                        "LinkUrl","mailto:ucc@pnu.edu.sa, مراسلة الدعم الفني للحقيبة الذكية",
                        "ButtonText","تواصل مع الدعم","ButtonText_EN","Contact Support",
                        "ItemOrder","1")
                }
            };

            // Shared AdminUsers list definition for permissions check
            map[SscListNames.AdminUsers] = new SscListDef
            {
                Name = SscListNames.AdminUsers,
                TitleAr = "مسؤولو المحتوى", TitleEn = "Content administrators",
                Description = "Users allowed to manage the Smart Suitcase content.",
                Fields = new List<SscFieldDef>
                {
                    new SscFieldDef("UserAccount", "حساب المستخدم", "User account", SPFieldType.User, true),
                    new SscFieldDef("Active", "مفعّل", "Active", SPFieldType.Boolean, true),
                    Order()
                },
                Seed = null
            };

            return map;
        }
    }
}
