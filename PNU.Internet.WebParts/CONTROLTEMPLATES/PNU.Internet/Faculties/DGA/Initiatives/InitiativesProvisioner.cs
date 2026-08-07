using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty initiatives lists on the CURRENT web:
    ///   FacultyInitiatives         - accordion items (Title + plain body + order)
    ///   FacultyInitiativesImages   - images per item (matched by ItemKey)
    ///   FacultyInitiativeSections  - prose sub-sections per item (Title + Body)
    ///   FacultyInitiativeCriteria  - scoring-criteria table rows (Dean's List)
    ///   FacultyDeansListNames      - Dean's List student roster
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// </summary>
    public static class InitiativesProvisioner
    {
        public const string ItemsListName = "FacultyInitiatives";
        public const string ImagesListName = "FacultyInitiativesImages";
        public const string SectionsListName = "FacultyInitiativeSections";
        public const string CriteriaListName = "FacultyInitiativeCriteria";
        public const string NamesListName = "FacultyDeansListNames";
        private static readonly object _lock = new object();

        public static void EnsureLists()
        {
            try
            {
                lock (_lock)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            web.AllowUnsafeUpdates = true;

                            SPList items = EnsureList(web, ItemsListName, "Faculty initiatives items");
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "ItemKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "ItemKey", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (items.ItemCount == 0) SeedItems(items);

                            SPList images = EnsureList(web, ImagesListName, "Faculty initiatives images");
                            EnsureField(images, "ItemKey", SPFieldType.Text);
                            EnsureField(images, "ImageUrl", SPFieldType.URL);
                            EnsureField(images, "SortOrder", SPFieldType.Number);
                            AddViewFields(images, "ItemKey", "ImageUrl", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(images);
                            if (images.ItemCount == 0) SeedImages(images);

                            SPList sections = EnsureList(web, SectionsListName, "Faculty initiative prose sub-sections");
                            EnsureField(sections, "TitleEn", SPFieldType.Text);
                            EnsureField(sections, "ItemKey", SPFieldType.Text);
                            EnsureField(sections, "BodyAr", SPFieldType.Note);
                            EnsureField(sections, "BodyEn", SPFieldType.Note);
                            EnsureField(sections, "SortOrder", SPFieldType.Number);
                            AddViewFields(sections, "TitleEn", "ItemKey", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(sections);
                            if (sections.ItemCount == 0) SeedSections(sections);

                            SPList criteria = EnsureList(web, CriteriaListName, "Faculty initiative scoring criteria");
                            EnsureField(criteria, "ItemKey", SPFieldType.Text);
                            EnsureField(criteria, "Details", SPFieldType.Note);
                            EnsureField(criteria, "DetailsEn", SPFieldType.Note);
                            EnsureField(criteria, "MaxScore", SPFieldType.Text);
                            EnsureField(criteria, "SortOrder", SPFieldType.Number);
                            AddViewFields(criteria, "ItemKey", "Details", "MaxScore", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(criteria);
                            if (criteria.ItemCount == 0) SeedCriteria(criteria);

                            SPList names = EnsureList(web, NamesListName, "Faculty Dean's List student names");
                            EnsureField(names, "ItemKey", SPFieldType.Text);
                            EnsureField(names, "Track", SPFieldType.Text);
                            EnsureField(names, "TrackEn", SPFieldType.Text);
                            EnsureField(names, "SortOrder", SPFieldType.Number);
                            AddViewFields(names, "ItemKey", "Track", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(names);
                            if (names.ItemCount == 0) SeedNames(names);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "InitiativesProvisioner - EnsureLists", ex.Message);
            }
        }

        private static SPList EnsureList(SPWeb web, string name, string desc)
        {
            SPList list = web.Lists.TryGetList(name);
            if (list == null)
            {
                Guid id = web.Lists.Add(name, desc, SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
            }
            return list;
        }

        private static void EnsureField(SPList list, string name, SPFieldType type)
        {
            if (!list.Fields.ContainsField(name))
            {
                string internalName = list.Fields.Add(name, type, false);
                if (type == SPFieldType.Note)
                {
                    var f = list.Fields.GetFieldByInternalName(internalName) as SPFieldMultiLineText;
                    if (f != null) { f.RichText = false; f.NumberOfLines = 10; f.Update(); }
                }
            }
        }

        private static void AddViewFields(SPList list, params string[] fields)
        {
            SPView view = list.DefaultView;
            bool changed = false;
            foreach (string f in fields)
                if (!view.ViewFields.Exists(f)) { view.ViewFields.Add(f); changed = true; }
            if (changed) view.Update();
        }

        // ---------- seeds (PLAIN TEXT) ----------

        private static void AddItem(SPList list, string key, string ar, string en, string bodyAr, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["ItemKey"] = key;
            item["BodyAr"] = bodyAr;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedItems(SPList list)
        {
            AddItem(list, "init1", "قائمة العميد", "Dean's List",
                "قائمة شرفية للطالبات المتميزات أكاديميًا أو في الأنشطة اللامنهجية، تقديرًا لإنجازاتهن وتحفيزًا لهن على مواصلة التميز.",
                1);
            AddItem(list, "init2", "مبادرة ولاء", "Walaa Initiative",
                "تحرص الكلية على استمرار علاقتها بخريجاتها، وتدعوهن إلى الانضمام لفريق مبادرة ولاء ليسهمن في عملية العطاء من خلال أنشطة الكلية المختلفة، ونقل معارفهن وخبراتهن المكتسبة من سوق العمل إلى طالبات الكلية.",
                2);
            AddItem(list, "init3", "مبادرة أجيالنا الواعدة", "Our Promising Generations",
                "مبادرة تُعنى بإرشاد المستجدات في كلية علوم الحاسب والمعلومات، لتقديم العون والمعرفة للأجيال القادمة.\n\nالرؤية: أن نكون مبادرة ذات عطاء يمتد بظلاله وجذوره ليحتوي جميع مستجدات تخصصات الحاسب والمعلومات.\n\nالرسالة: أن تكون المبادرة مرجعًا ينير مدارك المستجدات ويعرّفهن على لوائح الجامعة ومرافقها.",
                3);
            AddItem(list, "init4", "مبادرة تواصل", "Tawasul Initiative",
                "تحرص الكلية على إنشاء قاعدة بيانات لخريجات الكلية تشمل تخصصاتهن ووسائل التواصل معهن.\n\nالأهداف:\n\n- أن تكون المبادرة حلقة وصل بين الخريجات وسوق العمل عند توفر فرص مناسبة.\n- دعوة الخريجات إلى فعاليات الكلية، ومنها الدورات والخدمات المجتمعية والندوات.\n- تقديم الدعم المهني للخريجات.",
                4);
            AddItem(list, "init5", "مبادرة جاهزيتك مسؤوليتك", "Your Readiness Initiative",
                "مبادرة خاصة بتهيئة الطالبات الخريجات لاختبار الجاهزية المقدم من هيئة تقويم التعليم والتدريب.",
                5);
            AddItem(list, "init6", "مبادرة الشهادات المهنية", "Professional Certifications",
                "الشهادات المهنية أوراق اعتماد يحصل عليها الفرد بعد اجتياز برامج تدريبية أو اختبارات معيارية، وتصدرها هيئات مهنية محلية أو عالمية لإثبات مستوى المعرفة والمهارات المطلوبة في سوق العمل.",
                6);
            AddItem(list, "init7", "مبادرة She Codes", "She Codes",
                "معرض لمشاريع تخرج طالبات البكالوريوس في التخصصات الحاسوبية والتقنية بكليات الحاسب في الجامعات السعودية، يعرض جهود الطالبات وابتكاراتهن من خلال الملصقات والعروض التقديمية.\n\nيتضمن المعرض مسابقات لأكثر المشاريع إبداعًا وأفضل الملصقات.",
                7);
        }

        private static void AddSection(SPList list, string key, string ar, string en, string bodyAr, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["ItemKey"] = key;
            item["BodyAr"] = bodyAr;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedSections(SPList list)
        {
            // قائمة العميد — prose sub-sections
            AddSection(list, "init1", "الأهداف", "Objectives",
                "- تعزيز التميز الأكاديمي وتحفيز الطالبات على تحقيق أفضل النتائج.\n- رفع جودة المخرجات التعليمية وتنمية المهارات التقنية والبحثية والمشاركة في الأنشطة اللامنهجية.\n- تقدير إنجازات الطالبات وتكريمهن بوصفهن نماذج ملهمة.\n- تعزيز الانتماء المؤسسي للكلية والجامعة.\n- إيجاد بيئة تنافسية إيجابية بين الطالبات.\n- تحفيز روح المبادرة وتنمية المهارات الاجتماعية ومهارات التواصل.",
                1);
            AddSection(list, "init1", "مزايا الانضمام", "Membership Benefits",
                "- تكريم الطالبات في الحفل الختامي للكلية.\n- نشر أسماء الطالبات في موقع الكلية خلال الأسبوع السادس عشر من الفصل الدراسي الثاني.",
                2);
            AddSection(list, "init1", "مسارات القائمة", "List Tracks",
                "مسار التميز الأكاديمي: ترشّح الكلية آليًا الطالبات اللاتي يبلغ معدلهن التراكمي 4.99 فأعلى، مع اجتياز 50 ساعة دراسية فأكثر.\n\nمسار التميز في الأنشطة اللامنهجية: يُتاح للطالبات المستوفيات للشروط التقديم وفق معايير المفاضلة المعتمدة.",
                3);
            AddSection(list, "init1", "شروط التقديم لمسار الأنشطة اللامنهجية", "Extracurricular Track Conditions",
                "- ألا يقل المعدل التراكمي عن 3.5 من 5، مع إرفاق سجل أكاديمي معتمد.\n- ألا يكون لدى الطالبة رسوب أو حرمان في خطتها الدراسية.\n- ألا تكون قد صدرت بحق الطالبة عقوبة تأديبية.",
                4);
            AddSection(list, "init1", "آلية التقديم", "Application Mechanism",
                "تُرسل المستندات المطلوبة إلى البريد الإلكتروني CCIS-SA-DL@PNU.EDU.SA. يبدأ التقديم في الفصل الدراسي الثاني من الأسبوع الخامس وحتى الأسبوع العاشر.",
                5);
        }

        private static void AddCriteria(SPList list, string key, string ar, string details, string maxScore, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["ItemKey"] = key;
            item["Details"] = details;
            item["MaxScore"] = maxScore;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedCriteria(SPList list)
        {
            // قائمة العميد — معايير المفاضلة (scoring table)
            AddCriteria(list, "init1", "المعدل التراكمي", "(المعدل التراكمي × 30) ÷ 5", "30", 1);
            AddCriteria(list, "init1", "الأنشطة والقيادة",
                "- رئيسة أو نائبة نادٍ طلابي: 8 درجات.\n- رئيسة أو نائبة لجنة أو فريق: 8 درجات.\n- عضو فعال: درجتان لكل مشاركة، بحد أقصى 6 درجات.\n- تمثيل الكلية في أنشطة داخل الجامعة: 3 درجات لكل مشاركة، بحد أقصى 6 درجات.\n- تمثيل الكلية خارج الجامعة: 4 درجات لكل مشاركة، بحد أقصى 8 درجات.",
                "10", 2);
            AddCriteria(list, "init1", "المبادرات والشهادات المهنية",
                "- قيادة مبادرة: 10 درجات.\n- شهادة مهنية من خارج الخطة الدراسية: 5 درجات لكل شهادة.",
                "10", 3);
            AddCriteria(list, "init1", "البحث العلمي والمسابقات",
                "- بحث منشور أو مقبول للنشر: 10 درجات.\n- ملصق أو ورقة علمية مقبولة في مؤتمر: 8 درجات.\n- باحثة مشاركة في بحث قائم: 3 درجات لكل بحث، بحد أقصى 6 درجات.\n- المشاركة في بحث علمي: 3 درجات لكل مشاركة، بحد أقصى 6 درجات.\n- المشاركة في المسابقات: درجتان لكل مشاركة، بحد أقصى 6 درجات.",
                "20", 4);
            AddCriteria(list, "init1", "الجوائز والابتكار",
                "- جائزة على مستوى الكلية: درجتان لكل جائزة، بحد أقصى 6 درجات.\n- جائزة تميز أكاديمي: درجتان لكل جائزة، بحد أقصى 4 درجات.\n- جائزة رياضية أو ثقافية أو علمية: درجتان لكل جائزة، بحد أقصى 6 درجات.\n- براءة اختراع أو ابتكار: 10 درجات.",
                "10", 5);
            AddCriteria(list, "init1", "التطوع",
                "درجتان لكل مشاركة في الأعمال التطوعية أو الأنشطة الاجتماعية والتوعوية.",
                "20", 6);
            AddCriteria(list, "init1", "المجموع", "", "100", 7);
        }

        private static void AddName(SPList list, string key, string name, string track, string trackEn, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = name;
            item["ItemKey"] = key;
            item["Track"] = track;
            item["TrackEn"] = trackEn;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedNames(SPList list)
        {
            string tAcademic = "مسار التميز الأكاديمي";
            string tExtra = "مسار التميز في الأنشطة اللامنهجية";
            double s = 1;
            string[] academic = {
                "الاء زكي علي بايعشوت","البندري محمد بندر السعدون","جود عبدالله سعيد الدوسري","جوري محمد علي الحصيني",
                "ديما خالد مضحي العتيبي","رانيا عبدالله محمد فرحان","رزان يوسف ابن محمد القحطاني","رشا عبدالحكيم احمد الخطيب",
                "رنا زيد عبدالله آل عسكر","رناد خالد عبدالرحمن المحيسني","رند عبدالله محمد الزومان","رندا علي بن صالح الخضير",
                "رنده نائف عايض العتيبي","رنيم فيصل محمد الهزاع","رهف عبدالله فرحان المالكي","ريما عبدالله صالح السحيباني",
                "ريوف احمد عبدالله بن نفيسه","ساره أسامة حسن الزعبي","سعاد اسعد بن عبدالله الدريهم","غيداء عائش سعد العلياني",
                "فريده حسام رشدي وشاح","لينا خليل ابراهيم الضويحي","مايه محمد علي السيوفي","مها فهد صالح بن مجلي",
                "ميسان محمد انس العطار","نهى علي محمد القحطاني","نيار عبدالرحمن صالح الفريح","وجدان علي محمد آل بزيع",
                "يارا فهد عبدالكريم الشدوخي"
            };
            foreach (string n in academic) AddName(list, "init1", n, tAcademic, "Academic Excellence", s++);

            string[] extra = {
                "أثير ايمن عبدالرحمن العياضي","نوره خالد عبدالرحمن المحيسني","دانة بنت عبدالكريم بن عويض الحمدي",
                "جمانه فرج بن فراج الحربي","ميس صالح محمد العيدان","ريوف خالد محمد الخزيم",
                "ساره عبدالله راشد آل زنان","رناد عبدالله بن عبدالعزيز الجريوي"
            };
            foreach (string n in extra) AddName(list, "init1", n, tExtra, "Extracurricular Excellence", s++);
        }

        private static void AddImage(SPList list, string key, string url, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = key;
            item["ItemKey"] = key;
            item["ImageUrl"] = new SPFieldUrlValue { Url = url, Description = key };
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedImages(SPList list)
        {
            AddImage(list, "init2", "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/Walaa-Initiative/Walaa-Initiative.png", 1);
            AddImage(list, "init4", "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/Tawasul-Initiative/Tawasul-Initiative.png", 1);
            AddImage(list, "init7", "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/She-Codes/she-code.jpg", 1);
        }

    }
}
