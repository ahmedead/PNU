using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty student services lists on the CURRENT web:
    ///   FacultyResearch         - accordion items
    ///   FacultyStudentServicesSections - h3 sub-sections (ItemKey -> SectionKey)
    ///   FacultyStudentServicesSubItems - h4 blocks inside a sub-section (SectionKey)
    ///   FacultyStudentServicesTable    - table rows inside a sub-section (SectionKey)
    ///   FacultyStudentServicesImages   - images for an item OR a sub-section (OwnerKey)
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// All body text is PLAIN TEXT ("- " = bullet, "1." = numbered, blank line = paragraph).
    /// </summary>
    public static class StudentServicesProvisioner
    {
        public const string ItemsListName = "FacultyStudentServices";
        public const string SectionsListName = "FacultyStudentServicesSections";
        public const string SubItemsListName = "FacultyStudentServicesSubItems";
        public const string TableListName = "FacultyStudentServicesTable";
        public const string ImagesListName = "FacultyStudentServicesImages";
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

                            SPList items = EnsureList(web, ItemsListName, "Faculty student services accordion items");
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "ItemKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "ItemKey", "BodyAr", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (items.ItemCount == 0) SeedItems(items);

                            SPList sections = EnsureList(web, SectionsListName, "Faculty student services sub-sections");
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
                            if (sections.ItemCount == 0) SeedSections(sections);

                            SPList subs = EnsureList(web, SubItemsListName, "Faculty student services sub-items");
                            EnsureField(subs, "TitleEn", SPFieldType.Text);
                            EnsureField(subs, "SectionKey", SPFieldType.Text);
                            EnsureField(subs, "BodyAr", SPFieldType.Note);
                            EnsureField(subs, "BodyEn", SPFieldType.Note);
                            EnsureField(subs, "LinkUrl", SPFieldType.URL);
                            EnsureField(subs, "LinkText", SPFieldType.Text);
                            EnsureField(subs, "SortOrder", SPFieldType.Number);
                            AddViewFields(subs, "TitleEn", "SectionKey", "BodyAr", "LinkUrl", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(subs);
                            if (subs.ItemCount == 0) SeedSubItems(subs);

                            SPList table = EnsureList(web, TableListName, "Faculty student services table rows");
                            EnsureField(table, "SectionKey", SPFieldType.Text);
                            EnsureField(table, "Col2", SPFieldType.Note);
                            EnsureField(table, "Col3", SPFieldType.Text);
                            EnsureField(table, "SortOrder", SPFieldType.Number);
                            AddViewFields(table, "SectionKey", "Col2", "Col3", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(table);
                            if (table.ItemCount == 0) SeedTable(table);

                            SPList images = EnsureList(web, ImagesListName, "Faculty student services images");
                            EnsureField(images, "OwnerKey", SPFieldType.Text);
                            EnsureField(images, "ImageUrl", SPFieldType.URL);
                            EnsureField(images, "AltText", SPFieldType.Text);
                            EnsureField(images, "SortOrder", SPFieldType.Number);
                            AddViewFields(images, "OwnerKey", "ImageUrl", "AltText", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(images);
                            if (images.ItemCount == 0) SeedImages(images);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "StudentServicesProvisioner - EnsureLists", ex.Message);
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

        // ---------- seeds ----------

        private static void AddItem(SPList list, string key, string ar, string en, string bodyAr, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en; it["ItemKey"] = key;
            it["BodyAr"] = bodyAr; it["SortOrder"] = sort; it.Update();
        }

        private static void SeedItems(SPList list)
        {
            AddItem(list, "ss1", "الإرشاد الأكاديمي", "Academic Advising", "", 1);
            AddItem(list, "ss2", "الإرشاد النفسي والاجتماعي", "Psychological and Social Counselling",
                "تقدم وحدة الإرشاد النفسي والاجتماعي خدمات الدعم النفسي والاجتماعي للطالبات، وتهدف إلى تعزيز صحتهن النفسية وضمان استقرارهن الأكاديمي والاجتماعي من خلال المهام التالية:\n\n- تقديم خدمة الاستشارة الإرشادية الفردية.\n- دراسة حالات الطالبات المتقدمات لخدمة الإعانة والسكن الجامعي.\n- المساهمة في ملاحظة ورصد مشكلات الطالبات الأكاديمية والاجتماعية داخل الكلية.\n- تحويل الطالبة إلى مكتب ملاذ عند الحاجة إلى خدمات الدعم النفسي.\n- التعاون مع الأخصائية النفسية في تنفيذ خطط تأهيلية علاجية.\n- متابعة جميع الطالبات المسجلات في نظام بنر لضمان استقرار أوضاعهن الأكاديمية.\n- التنسيق مع وحدة مساندة الطالبات ذوات الإعاقة لتقديم الدعم اللازم.\n- إعداد تقارير فصلية عن الحالات الواردة للمكتب.\n- التخطيط للأنشطة الوقائية والعلاجية وفقًا لاحتياجات الطالبات.\n- تقديم برامج تعريفية للطالبات المستجدات للتعريف بخدمات الوحدة.",
                2);
            AddItem(list, "ss3", "الإرشاد المهني", "Career Counselling",
                "تنظم وحدة الإرشاد المهني عددًا من البرامج التدريبية المهنية المتخصصة، تشمل:\n\n- جلسات الإرشاد المهني الفردية.\n- جلسات الإرشاد المهني الجماعية.\n- برنامج صورتي المهنية.\n- برنامج التدريب التعاوني.\n- برامج الشهادات المهنية.\n- فعاليات ومنتديات الإرشاد المهني.",
                3);
            AddItem(list, "ss4", "إدارة الخدمات الطلابية", "Student Services Administration",
                "تُعنى إدارة الخدمات الطلابية بتنمية مهارات الطالبات، وتشجيع مشاركتهن في الفعاليات والمؤتمرات والمسابقات المحلية والدولية، وتنظيم الرحلات الطلابية، وتفعيل دورهن في الأندية الرياضية والطلابية ومختلف الأنشطة. كما تقدم الدعم المهني وتنمية المهارات للطالبات والخريجات، وتعمل على تسهيل اندماج الخريجات في المجتمع وتطوير مهاراتهن المهنية من خلال العمل التطوعي.",
                4);
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

        private static void AddSectionLink(SPList list, string itemKey, string sectionKey, string ar, string en,
            string bodyAr, string linkUrl, string linkText, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en;
            it["ItemKey"] = itemKey; it["SectionKey"] = sectionKey;
            it["BodyAr"] = bodyAr; it["SortOrder"] = sort;
            if (!string.IsNullOrEmpty(linkUrl))
            {
                it["LinkUrl"] = new SPFieldUrlValue { Url = linkUrl, Description = linkText };
                it["LinkText"] = linkText;
            }
            it.Update();
        }

        private static void SeedSections(SPList list)
        {
            // ---- ss1: الإرشاد الأكاديمي ----
            AddSection(list, "ss1", "ss1-advisor", "مهام المرشدة الأكاديمية", "Academic Advisor Duties",
                "1. مراجعة السجل الأكاديمي للطالبة، بما في ذلك المقررات المكتملة والخطة الدراسية، لضمان التقدم وفق الخطة والتخرج في الوقت المحدد.\n2. بناء علاقة جيدة مع الطالبة وجميع الأطراف المعنية بعملية الإرشاد الأكاديمي والحفاظ عليها.\n3. تشجيع الطالبات على استخدام موقع الجامعة للاطلاع على الأدلة واللوائح والقواعد ونماذج الإرشاد، ومتابعة الأخبار والفعاليات والإعلانات.\n4. توجيه الطالبات للحصول على الخدمات الأكاديمية وفق التقويم الجامعي، مثل التسجيل وإضافة أو حذف المقررات والانسحاب والتأجيل والتكليف والتحويل ومعادلة المقررات.\n5. الإجابة على استفسارات الطالبات في نطاق عملية الإرشاد الأكاديمي.\n6. تحديد المشكلات الأكاديمية وغير الأكاديمية التي تؤثر على الطالبات وإبلاغ منسقة الإرشاد الأكاديمي بالقسم بها.\n7. مساعدة الطالبات على فهم أنفسهن والتعرف على المشكلات المؤثرة في تحصيلهن الدراسي وإيجاد حلول لها.\n8. مساعدة الطالبات على المشاركة في الأنشطة والاجتماعات والبرامج التدريبية.\n9. متابعة التقدم الأكاديمي للطالبات لضمان استيفاء متطلبات التخرج في الوقت المحدد.\n10. دعم الطالبات في تنمية شخصياتهن بشكل شامل.\n11. مساعدة الطالبات في التخطيط لمستقبلهن الأكاديمي والمهني.\n12. تحديد ساعات الإرشاد الأكاديمي وإبلاغ الطالبات بها.",
                "", 1);
            AddSection(list, "ss1", "ss1-coord", "مهام منسقة الإرشاد الأكاديمي", "Advising Coordinator Duties",
                "1. إعداد قوائم الإرشاد الأكاديمي.\n2. توزيع الطالبات الجدد على المرشدات الأكاديميات.\n3. تزويد وكيلة الكلية للشؤون الأكاديمية ورئيسة وحدة الإرشاد الأكاديمي بقوائم المرشدات والطالبات.\n4. تعليق أسماء الطالبات على أبواب مكاتب المرشدات الأكاديميات.\n5. التأكد من ربط جميع الطالبات بمرشداتهن في النظام.\n6. تخصيص أماكن مناسبة للإرشاد داخل كل قسم، وتوفير كتيبات تعريفية عن الإرشاد الأكاديمي والمعلومات الخاصة بالقسم.\n7. الإعلان عن مكان ومواعيد الإرشاد عبر لوحات إعلانات القسم والكلية.\n8. اختيار مجموعة من الطالبات المتفوقات في القسم ليكن «صديقات الإرشاد» وتدريبهن على مسؤولياتهن.\n9. تفعيل دور صديقات الإرشاد والتأكد من تسجيل مساهماتهن في سجل مهاراتهن.\n10. تزويد المرشدة الأكاديمية بأدوات الإرشاد التالية: الخطة الدراسية، ومعلومات معادلة المقررات، وتقويم الإجراءات الأكاديمية، والجداول الدراسية والأرقام المرجعية لكل شعبة.\n11. متابعة عملية الإرشاد الأكاديمي في القسم والتأكد من سيرها على النحو الأمثل، وإبلاغ رئيسة القسم بأي مشكلات أو تقصير.\n12. حصر المشكلات الأكاديمية التي تواجهها الطالبات في القسم.\n13. تنظيم ورش عمل للمرشدات الجديدات في القسم للتدريب على مهام وأدوات الإرشاد الأكاديمي.\n14. الاستعداد للفصل الدراسي القادم من خلال طلب قوائم الخريجات والطالبات المتعثرات وأعضاء هيئة التدريس.\n15. ما يُسند من مهام أخرى في مجال التخصص.",
                "", 2);

            // ---- ss2: التواصل ----
            AddSection(list, "ss2", "ss2-contact", "التواصل", "Contact",
                "- 0118238463 – 0118238665\n- 0118241959 – 0118238353\n- Ccis-cg@pnu.edu.sa\n- محطة A3، الدور الأول، مكتب 0.511.",
                "", 1);

            // ---- ss3: التواصل + زر الخدمة ----
            AddSectionLink(list, "ss3", "ss3-contact", "التواصل", "Contact",
                "- كلية علوم الحاسب والمعلومات، مبنى 170، الدور الأرضي، مكتب 0.100.\n- ccis-ccu@pnu.edu.sa\n- 8222154",
                "https://pnu.edu.sa/ar/Pages/Eservice.aspx", "الحصول على خدمة الإرشاد المهني", 1);

            // ---- ss4: مهام + تواصل + وحدات ----
            AddSection(list, "ss4", "ss4-tasks", "مهام إدارة الخدمات الطلابية", "Administration Duties",
                "- الإشراف على أنشطة الأندية الطلابية والرياضية والفعاليات والمسابقات والمسرح بالكلية بالتنسيق مع عمادة شؤون الطالبات.\n- الإشراف على العمل التطوعي الطلابي بالتنسيق مع عمادة شؤون الطالبات.\n- تقديم الخدمات المساندة، مثل الإعانات والقروض والحقوق الطالبية والسكن والتعاون الطلابي والخزائن والتغذية.\n- تقديم خدمات الإرشاد النفسي والاجتماعي والخدمات المقدمة للطالبات ذوات الإعاقة.\n- تقديم خدمات الخريجات وتعزيز استمرار العلاقة الإيجابية مع الكلية بالتنسيق مع إدارة الخريجات في الجامعة.\n- تقديم الدعم المهاري والإرشاد المهني للطالبات والخريجات بالتنسيق مع مركز الدعم الطلابي والمهني بالجامعة.\n- تفعيل دور مراقب الطلبة بالتنسيق مع عمادة شؤون الطالبات.\n- التنسيق مع إدارة الشراكات والمسؤولية المجتمعية في الكلية فيما يخص التطوع الطلابي.",
                "", 1);
            AddSection(list, "ss4", "ss4-contact", "التواصل مع الإدارة", "Contact the Administration",
                "كلية علوم الحاسب والمعلومات، مبنى 170، الدور الأرضي، مكتب 0.100.06 – 0.100.\n\nCcis-sa@pnu.edu.sa",
                "", 2);
            AddSection(list, "ss4", "ss4-units", "الوحدات التابعة لإدارة الخدمات الطلابية", "Units", "", "", 3);
        }

        private static void AddSub(SPList list, string sectionKey, string ar, string en, string bodyAr, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en; it["SectionKey"] = sectionKey;
            it["BodyAr"] = bodyAr; it["SortOrder"] = sort; it.Update();
        }

        private static void AddSubLink(SPList list, string sectionKey, string ar, string en, string bodyAr,
            string linkUrl, string linkText, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en; it["SectionKey"] = sectionKey;
            it["BodyAr"] = bodyAr; it["SortOrder"] = sort;
            if (!string.IsNullOrEmpty(linkUrl))
            {
                it["LinkUrl"] = new SPFieldUrlValue { Url = linkUrl, Description = linkText };
                it["LinkText"] = linkText;
            }
            it.Update();
        }

        private static void SeedSubItems(SPList list)
        {
            AddSub(list, "ss4-units", "وحدة الخريجات", "Graduates Unit",
                "تهدف إلى الحفاظ على التواصل مع الخريجات، والإعلان عن ورش العمل والدورات التي تسهم في تطويرهن المهني، واستطلاع آرائهن والاستفادة من مقترحاتهن في الفعاليات.\n\nمبنى 170، الدور الأرضي، مكتب 0.100.\n\nccis-g@pnu.edu.sa", 1);
            AddSub(list, "ss4-units", "وحدة الأندية والأنشطة الطلابية", "Clubs and Student Activities Unit",
                "تعمل على تفعيل الأنشطة الثقافية والصحية والوطنية والعالمية للطالبات.\n\nمبنى 170، الدور الأرضي، مكتب 0.100.\n\nccis-active@pnu.edu.sa", 2);
            AddSub(list, "ss4-units", "وحدة الخزائن", "Lockers Unit",
                "تتولى مسؤولية تأجير الخزائن لطالبات كلية علوم الحاسب والمعلومات.\n\nمبنى 170، الدور الأرضي، مكتب 0.100.\n\nccis-lou@pnu.edu.sa", 3);
            AddSub(list, "ss4-units", "وحدة الإرشاد المهني", "Career Counselling Unit",
                "تنظم جلسات الإرشاد المهني الفردية والجماعية، وبرنامج صورتي المهنية، والتدريب التعاوني، وبرامج الشهادات المهنية، وفعاليات ومنتديات الإرشاد المهني.\n\nمبنى 170، الدور الأرضي، مكتب 0.100.\n\nccis-ccu@pnu.edu.sa", 4);
            AddSub(list, "ss4-units", "النادي الرياضي", "Sports Club",
                "مسؤول عن إدارة النادي الرياضي في الكلية، ويخدم جميع منسوبات الكلية من طالبات وعضوات هيئة تدريس وموظفات.\n\nمبنى 170، الدور الثاني، 2.106.\n\nccis-scl@pnu.edu.sa", 5);
            AddSub(list, "ss4-units", "المجلس الطلابي", "Student Council",
                "جهة طلابية منتخبة تمثل الكلية، وتعمل حلقة وصل بين الجامعة والطالبات، وتشارك في عمليات صنع القرار.\n\nمبنى 170، الدور الأرضي، مكتب 0.100.", 6);
            AddSubLink(list, "ss4-units", "وحدة حقوق الطالبات", "Student Rights Unit",
                "تقدم الاستفسارات والدعم المتعلق بحقوق الطالبات.\n\nمبنى 170، الدور الأرضي، مكتب 0.100.",
                "https://pnu.edu.sa/ar/Faculties/IT/Documents/13-1-2026/Female%20Students%E2%80%99%20Rights%20Protection%20Unit%20Arabic.pdf",
                "وحدة حماية حقوق الطالبات", 7);
        }

        private static void SeedTable(SPList list) { }

        private static void SeedImages(SPList list) { }
    }
}
