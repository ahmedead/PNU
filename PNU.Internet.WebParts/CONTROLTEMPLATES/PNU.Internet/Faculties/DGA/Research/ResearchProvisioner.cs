using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty research lists on the CURRENT web:
    ///   FacultyResearch         - accordion items
    ///   FacultyResearchSections - h3 sub-sections (ItemKey -> SectionKey)
    ///   FacultyResearchSubItems - h4 blocks inside a sub-section (SectionKey)
    ///   FacultyResearchTable    - table rows inside a sub-section (SectionKey)
    ///   FacultyResearchImages   - images for an item OR a sub-section (OwnerKey)
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// All body text is PLAIN TEXT ("- " = bullet, "1." = numbered, blank line = paragraph).
    /// </summary>
    public static class ResearchProvisioner
    {
        public const string ItemsListName = "FacultyResearch";
        public const string SectionsListName = "FacultyResearchSections";
        public const string SubItemsListName = "FacultyResearchSubItems";
        public const string TableListName = "FacultyResearchTable";
        public const string ImagesListName = "FacultyResearchImages";
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

                            bool itemsCreated;
                            SPList items = EnsureList(web, ItemsListName, "Faculty research accordion items", out itemsCreated);
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "ItemKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "ItemKey", "BodyAr", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (itemsCreated) SeedItems(items);   // seed only on first creation

                            bool sectionsCreated;
                            SPList sections = EnsureList(web, SectionsListName, "Faculty research sub-sections", out sectionsCreated);
                            EnsureField(sections, "TitleEn", SPFieldType.Text);
                            EnsureField(sections, "ItemKey", SPFieldType.Text);
                            EnsureField(sections, "SectionKey", SPFieldType.Text);
                            EnsureField(sections, "BodyAr", SPFieldType.Note);
                            EnsureField(sections, "BodyEn", SPFieldType.Note);
                            EnsureField(sections, "TableHeaders", SPFieldType.Text);
                            EnsureField(sections, "LinkUrl", SPFieldType.URL);
                            EnsureField(sections, "LinkText", SPFieldType.Text);
                            EnsureField(sections, "SortOrder", SPFieldType.Number);
                            AddViewFields(sections, "TitleEn", "ItemKey", "SectionKey", "BodyAr", "TableHeaders", "LinkUrl", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(sections);
                            if (sectionsCreated) SeedSections(sections);   // seed only on first creation

                            bool subsCreated;
                            SPList subs = EnsureList(web, SubItemsListName, "Faculty research sub-items", out subsCreated);
                            EnsureField(subs, "TitleEn", SPFieldType.Text);
                            EnsureField(subs, "SectionKey", SPFieldType.Text);
                            EnsureField(subs, "BodyAr", SPFieldType.Note);
                            EnsureField(subs, "BodyEn", SPFieldType.Note);
                            EnsureField(subs, "LinkUrl", SPFieldType.URL);
                            EnsureField(subs, "LinkText", SPFieldType.Text);
                            EnsureField(subs, "SortOrder", SPFieldType.Number);
                            AddViewFields(subs, "TitleEn", "SectionKey", "BodyAr", "LinkUrl", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(subs);
                            if (subsCreated) SeedSubItems(subs);   // seed only on first creation

                            bool tableCreated;
                            SPList table = EnsureList(web, TableListName, "Faculty research table rows", out tableCreated);
                            EnsureField(table, "SectionKey", SPFieldType.Text);
                            EnsureField(table, "Col2", SPFieldType.Note);
                            EnsureField(table, "Col3", SPFieldType.Text);
                            EnsureField(table, "Col4", SPFieldType.Text);
                            EnsureField(table, "Col5", SPFieldType.Text);
                            EnsureField(table, "SortOrder", SPFieldType.Number);
                            AddViewFields(table, "SectionKey", "Col2", "Col3", "Col4", "Col5", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(table);
                            if (tableCreated) SeedTable(table);   // seed only on first creation

                            bool imagesCreated;
                            SPList images = EnsureList(web, ImagesListName, "Faculty research images", out imagesCreated);
                            EnsureField(images, "OwnerKey", SPFieldType.Text);
                            EnsureField(images, "ImageUrl", SPFieldType.URL);
                            EnsureField(images, "AltText", SPFieldType.Text);
                            EnsureField(images, "SortOrder", SPFieldType.Number);
                            AddViewFields(images, "OwnerKey", "ImageUrl", "AltText", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(images);
                            if (imagesCreated) SeedImages(images);   // seed only on first creation

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ResearchProvisioner - EnsureLists", ex.Message);
            }
        }

        /// <summary>
        /// Returns the list, creating it when missing. <paramref name="created"/> is
        /// true ONLY when this call created it, which is what drives seeding: an
        /// existing list is never re-seeded, even if the content team emptied it.
        /// </summary>
        private static SPList EnsureList(SPWeb web, string name, string desc, out bool created)
        {
            created = false;
            SPList list = web.Lists.TryGetList(name);
            if (list == null)
            {
                created = true;
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

        // ---------- seeds ----------

        private static void AddItem(SPList list, string key, string ar, string en, string bodyAr, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en; it["ItemKey"] = key;
            it["BodyAr"] = bodyAr; it["SortOrder"] = sort; it.Update();
        }

        private static void SeedItems(SPList list)
        {
            AddItem(list, "res1", "وحدة البحث العلمي", "Scientific Research Unit", "", 1);
            AddItem(list, "res2", "مركز الابتكار", "Innovation Center", "", 2);
            AddItem(list, "res3", "معامل تخصصية", "Specialized Labs", "", 3);
        }

        private static void AddSection(SPList list, string itemKey, string sectionKey, string ar, string en,
            string bodyAr, string tableHeaders, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en;
            it["ItemKey"] = itemKey; it["SectionKey"] = sectionKey;
            it["BodyAr"] = bodyAr; it["TableHeaders"] = tableHeaders;
            it["SortOrder"] = sort; it.Update();
        }

        private static void SeedSections(SPList list)
        {
            // ---- res1: وحدة البحث العلمي ----
            AddSection(list, "res1", "res1-tasks", "مهام الوحدة", "Unit Tasks",
                "- إعداد الخطة السنوية للبحث العلمي والأولويات البحثية للكلية بالتعاون مع الأقسام، ومتابعة تنفيذها بعد اعتمادها من جهة الاختصاص في الجامعة.\n- تنمية ثقافة الابتكار في الكلية وتقديم الدعم اللوجستي والفني للمبتكرات والباحثات المتميزات بالتنسيق مع عمادة البحث العلمي والمكتبات.\n- تحفيز الأعضاء على التقديم على برامج البحث العلمي ومتابعة سير المشاريع البحثية المختلفة للمستفيدين.\n- متابعة المجموعات البحثية للمستفيدين وتعريفهم بخدمات التمويل المؤسسي للبحث والابتكار.\n- الاتصال والتنسيق مع المراكز داخل الجامعة وخارجها بشأن كل ما له علاقة بطبيعة البحوث وتسويقها.",
                "", 1);
            AddSection(list, "res1", "res1-lab", "المعمل البحثي", "Research Lab", "", "", 2);

            // ---- res2: مركز الابتكار ----
            AddSection(list, "res2", "res2-word", "كلمة مديرة المركز", "Director's Word",
                "انطلاقًا من رؤية الجامعة، وحرصها على تفوقها وتميزها في العديد من المجالات، ومع زيادة الاهتمام العالمي والمحلي بالابتكار وريادة الأعمال ودعم الأفكار الشبابية الطموحة والمشاريع الابتكارية؛ أنشأت كلية علوم الحاسب والمعلومات مركز الابتكار ليكون بوابة ورافدًا مهمًا لتحقيق هذه الأهداف.\n\nيعمل المركز على رفع مستوى الوعي بالتقنيات الناشئة بإقامة الدورات وورش العمل، وخلق بيئة تفاعلية مستدامة تساعد الطالبات على اكتساب المعرفة والمهارات والخبرات العلمية في تلك التقنيات عن طريق المعسكرات. كما يعمل على اكتشاف التحديات وتوليد الأفكار عن طريق الهاكثونات، ورعاية الأفكار والاختراعات والمواهب حتى تصبح واقعًا ملموسًا، وأن يكون مركز الابتكار خلية حية من طالبات الكلية الباحثات عن تحويل ابتكاراتهن إلى مشاريع فعلية.\n\nكما يُعنى المركز بتقديم الدورات وورش العمل والمعسكرات التدريبية والساعات الاستشارية ومساحات العمل الحرة على مصادر المركز من الروبوتات والأجهزة المختلفة، بما يسهم في إنتاج جيل من المبتكرات في مجال الحاسب الآلي قادرات على مواكبة سوق العمل والإبداع فيه وخدمة الوطن.\n\nمديرة مركز الابتكار بكلية علوم الحاسب والمعلومات",
                "", 1);
            AddSection(list, "res2", "res2-units", "وحدات المركز", "Center Units", "", "", 2);
            AddSection(list, "res2", "res2-devices", "أجهزة المركز", "Center Devices",
                "- روبوتان من نوع Pepper.\n- روبوت NAO.\n- روبوت VEX.\n- مجموعة أجهزة CLASS VR للواقع الافتراضي.\n- طاولة تفاعلية.\n- طابعات ثلاثية الأبعاد 3D Printer.\n- الأردوينو والراسبيري باي Arduino and Raspberry Pi.",
                "اسم الجهاز|استخداماته|نوعه", 3);
            AddSection(list, "res2", "res2-photos", "صور أجهزة مركز الابتكار", "Center Device Photos", "", "", 4);
            AddSection(list, "res2", "res2-services", "خدمات المركز", "Center Services", "", "", 5);

            // ---- res3: معامل تخصصية (one sub-section per lab) ----
            AddSection(list, "res3", "lab1", "معمل سيسكو (Cisco Laboratory)", "Cisco Laboratory",
                "معمل موجه للطالبات المتخصصات في مجال تقنية المعلومات، يتيح التعلم والتدريب على مهارات الشبكات والبرمجة والأمن السيبراني المؤهلة للحصول على شهادات سيسكو المهنية، ويتميز بتجهيزات حديثة ومحاكاة واقعية للبيئة التقنية.\n\n- أجهزة حاسب آلي مكتبي.\n- أجهزة توجيه Routers.\n- محولات شبكية Switches.",
                "", 1);
            AddSection(list, "res3", "lab2", "معمل الاتصالات (Signal and Communication Lab)", "Signal and Communication Lab",
                "معمل موجه للطالبات والباحثين المهتمين بأنظمة الاتصالات، ويوفر التدريب على تصميم وتحليل الأنظمة اللاسلكية والموجات الكهرومغناطيسية باستخدام تجهيزات متطورة وأدوات قياس متخصصة لفهم الإشارات والترددات المختلفة.\n\n- أجهزة حاسب آلي.\n- لوحات DIGITAL COMM 1.\n- لوحات FIBER OPTICS COMM.\n- لوحات ANALOG COMMUNICATION.\n- قواعد COMP BASE USB.\n- أجهزة MULTIMETER / FUNCT GEN.\n- أجهزة Digital Storage Oscilloscope.\n- لوحات Embedded Internet Training Solution.",
                "", 2);
            AddSection(list, "res3", "lab3", "مركز أبل للتدريب (Apple Training Centre)", "Apple Training Centre",
                "مركز معتمد من أبل يتيح للمتدربين الراغبين في التخصص في تطوير التطبيقات باستخدام نظام أبل التعرف على أنظمة التشغيل والتطبيقات الخاصة بأجهزة أبل. يضم أجهزة حاسب من شركة أبل وبرامج تطوير متخصصة مثل Xcode وSwift، ويهدف إلى تعزيز مهارات تطوير التطبيقات والبرامج وتوفير بيئة تعليمية لاستكشاف تقنيات أبل.",
                "", 3);
            AddSection(list, "res3", "lab4", "معمل أبل لمشاريع التخرج (Apple Lab for Graduation Projects)", "Apple Lab for Graduation Projects",
                "معمل مجهز بأحدث الأجهزة اللازمة لطالبات الكلية لتنفيذ مشاريعهن النهائية بالتعاون مع أكاديمية أبل. يتضمن برامج حديثة وأدوات لتطوير البرمجيات، ويساعد الطالبات على تطبيق المفاهيم والمهارات المكتسبة في إنجاز مشاريع تخرج تعتمد على أنظمة وبرامج أبل الموجهة لتطوير تطبيقات متخصصة.",
                "", 4);
            AddSection(list, "res3", "lab5", "معمل تحليل البيانات (Data Analytics Lab)", "Data Analytics Lab",
                "معمل موجه للمهتمين بفهم البيانات واستخدامها بفاعلية. تتدرب فيه الطالبات على استخراج البيانات وتنقيبها وتحليلها باستخدام الأدوات والوسائل التقنية المناسبة، ويضم برامج وأجهزة حديثة للتحليل الإحصائي والتعامل مع البيانات الضخمة.",
                "", 5);
            AddSection(list, "res3", "lab6", "معمل الدوائر الرقمية (Digital Logic Lab)", "Digital Logic Lab",
                "معمل للمهتمين بالإلكترونيات وتصميم الدوائر الرقمية، يتيح للطالبات تعلم وتطوير مهارات تصميم الدوائر الرقمية وتحليلها باستخدام أدوات ومكونات إلكترونية متنوعة لإنشاء الدوائر واختبارها.\n\n- أجهزة حاسب آلي محمولة.\n- DC Power Supply.\n- Two Channel Digital Storage Oscilloscope.\n- Arbitrary Function Generator.\n- Electronic Logic Trainer Kit.\n- Digital Trainer Kit.",
                "", 6);
            AddSection(list, "res3", "lab7", "معمل الروبوتات (Robotics Laboratory)", "Robotics Laboratory",
                "معمل موجه للطالبات المتخصصات في الروبوتات والذكاء الاصطناعي، يتيح تعلم وتطوير مهارات بناء الروبوتات وبرمجتها باستخدام أذرع آلية وأجهزة متنوعة لإنشاء مشاريع مبتكرة وحل المشكلات العملية.\n\n- أجهزة حاسب آلي محمولة.\n- DC Power Supply.\n- Two Channel Digital Storage Oscilloscope.\n- Arbitrary Function Generator.\n- Electronic Logic Trainer Kit.\n- Digital Trainer Kit.\n- أجهزة Six Axis Robot Adept Viper S 650.",
                "", 7);
            AddSection(list, "res3", "lab8", "معمل الأمن السيبراني (Cybersecurity Lab)", "Cybersecurity Lab",
                "معمل متطور لتدريب الطالبات على برامج وتقنيات حماية الأنظمة والشبكات من التهديدات الإلكترونية، وكيفية اكتشاف الاختراقات السيبرانية ومنعها وحماية البيانات الحساسة. يضم أجهزة متطورة وبرامج لتحليل الهجمات واختبار الأمان.",
                "", 8);
        }

        private static void AddSub(SPList list, string sectionKey, string ar, string en, string bodyAr, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en; it["SectionKey"] = sectionKey;
            it["BodyAr"] = bodyAr; it["SortOrder"] = sort; it.Update();
        }

        private static void SeedSubItems(SPList list)
        {
            // وحدات المركز
            AddSub(list, "res2-units", "وحدة المسابقات", "Competitions Unit",
                "تهتم الوحدة بدعم طالبات كلية علوم الحاسب والمعلومات للمشاركة في المسابقات والهاكثونات المحلية والدولية، وتحقيق نتائج متقدمة وحصد الجوائز وتعزيز كفاءة طالبات الجامعة.\n\n- بناء مجتمع محب للإبداع والابتكار من خلال المشاركة في المسابقات والهاكثونات المحلية والدولية.\n- توفير بيئة مناسبة للمشاركة في المسابقات المحلية والدولية، وخلق فرص تعليمية تنافسية من خلال مسابقات وهاكثونات تقام بالكلية وتحاكي المسابقات المحلية والدولية.",
                1);
            AddSub(list, "res2-units", "وحدة الروبوتات والأجهزة", "Robotics and Devices Unit",
                "تُعنى الوحدة بكل ما هو جديد في علم الروبوتات والأجهزة التقنية الحديثة، وتشمل أعمالها:\n\n- توفير أجهزة حديثة لطالبات الكلية للممارسة العملية لكل ما هو جديد في هذا المجال وكسر الحاجز بين التعليم النظري والعملي.\n- تدريب أكبر عدد ممكن من الطالبات على الروبوتات والأجهزة الحديثة، لضمان مخرجات تعليمية عالية المستوى قادرة على مواجهة تحديات سوق العمل ومتطلباته.",
                2);

            // خدمات المركز
            AddSub(list, "res2-services", "المعسكرات", "Bootcamps",
                "دورات تدريبية مكثفة في سلسلة من اللقاءات لتمكين منسوبي الكلية معرفيًا ومهاريًا من التقنيات الناشئة والابتكار وريادة الأعمال، وينتج عنها نماذج أولية للمشاريع.", 1);
            AddSub(list, "res2-services", "ورش العمل", "Workshops",
                "لقاءات قصيرة تهدف إلى نشر الوعي بالتقنيات الناشئة والابتكار وريادة الأعمال.", 2);
            AddSub(list, "res2-services", "مساحات العمل الحرة", "Open Workspaces",
                "تمكّن منسوبي الكلية من استكشاف مصادر المركز والتقنيات الناشئة من خلال مكاتب عمل بتنظيم مرن، وتوفر فرص التواصل والتعاون.", 3);
            AddSub(list, "res2-services", "الاستشارات", "Consultations",
                "جلسات استشارية وإرشادية في التقنيات الناشئة والابتكار وريادة الأعمال.", 4);
            AddSub(list, "res2-services", "الزيارات التوعوية", "Awareness Visits",
                "تتيح لأعضاء الكلية وطالباتها زيارة مركز الابتكار والتعرف على مصادره من أجهزة وخدمات.", 5);
        }

        /// <summary>
        /// Adds a table row. Columns 4 and 5 are optional: leave them empty when the
        /// sub-section's TableHeaders declares fewer than five columns.
        /// </summary>
        private static void AddRow(SPList list, string sectionKey, string c1, string c2, string c3, double sort,
            string c4 = "", string c5 = "")
        {
            SPListItem it = list.AddItem();
            it["Title"] = c1; it["SectionKey"] = sectionKey;
            it["Col2"] = c2; it["Col3"] = c3;
            it["Col4"] = c4; it["Col5"] = c5;
            it["SortOrder"] = sort; it.Update();
        }

        private static void SeedTable(SPList list)
        {
            AddRow(list, "res2-devices", "Pepper Robot", "روبوت بشري للتفاعل والمحادثات والتعرف على المشاعر وتوفير المعلومات وتنفيذ المهام البسيطة.", "روبوت", 1);
            AddRow(list, "res2-devices", "NAO Robot", "روبوت بشري للتعليم والبحث، يُستخدم لتدريس البرمجة والروبوتات والذكاء الاصطناعي.", "روبوت", 2);
            AddRow(list, "res2-devices", "VEX Robot", "روبوت تعليمي لبناء وبرمجة حلول للتحديات، وتنمية المهارات الهندسية وحل المشكلات والعمل الجماعي.", "روبوت", 3);
            AddRow(list, "res2-devices", "CLASS VR", "منصة تعليمية للواقع الافتراضي تقدم محاكاة تفاعلية ورحلات ميدانية افتراضية ومحتوى تعليميًا.", "جهاز محاكاة", 4);
            AddRow(list, "res2-devices", "Attractive Table", "طاولة ذكية تُستخدم في الشروحات التفاعلية للمجموعات.", "طاولة تفاعلية", 5);
            AddRow(list, "res2-devices", "3D Printer", "طابعات لإنشاء مجسمات ثلاثية الأبعاد بناءً على نماذج رقمية، للاستخدامات التعليمية والنماذج الأولية.", "طابعة ثلاثية الأبعاد", 6);
            AddRow(list, "res2-devices", "Arduino and Raspberry Pi", "وحدات تحكم لمشاريع الإلكترونيات والبرمجة والروبوتات وإنترنت الأشياء.", "حساسات وقطع إلكترونية", 7);
        }

        private static void AddImage(SPList list, string ownerKey, string url, string alt, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = alt; it["OwnerKey"] = ownerKey;
            it["ImageUrl"] = new SPFieldUrlValue { Url = url, Description = alt };
            it["AltText"] = alt; it["SortOrder"] = sort; it.Update();
        }

        private static void SeedImages(SPList list)
        {
            const string RC = "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/researchcenter/";
            const string IC = "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/innovationcenter/";
            const string PA = "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/projectsachievements1/";

            AddImage(list, "res1-lab", RC + "27-lab1.jpg", "المعمل البحثي في كلية علوم الحاسب والمعلومات", 1);
            AddImage(list, "res1-lab", RC + "27-lab2.png", "تجهيزات المعمل البحثي في كلية علوم الحاسب والمعلومات", 2);

            AddImage(list, "res2-photos", IC + "p1.jpg", "أجهزة مركز الابتكار", 1);
            AddImage(list, "res2-photos", IC + "p2.jpg", "روبوتات مركز الابتكار", 2);
            AddImage(list, "res2-photos", IC + "p3.jpg", "تقنيات مركز الابتكار", 3);
            AddImage(list, "res2-photos", IC + "p4.jpg", "مساحة مركز الابتكار", 4);

            AddImage(list, "lab1", PA + "Picture1.jpg", "معمل سيسكو", 1);
            AddImage(list, "lab2", PA + "Picture2.jpg", "معمل الاتصالات", 1);
            AddImage(list, "lab3", PA + "Picture3.jpg", "مركز أبل للتدريب", 1);
            AddImage(list, "lab4", PA + "Picture4.jpg", "معمل أبل لمشاريع التخرج", 1);
            AddImage(list, "lab5", PA + "Picture5.jpg", "معمل تحليل البيانات", 1);
            AddImage(list, "lab6", PA + "Picture6.jpg", "معمل الدوائر الرقمية", 1);
            AddImage(list, "lab7", PA + "Picture7.jpg", "معمل الروبوتات", 1);
            AddImage(list, "lab8", PA + "Picture8.jpg", "معمل الأمن السيبراني", 1);
        }
    }
}
