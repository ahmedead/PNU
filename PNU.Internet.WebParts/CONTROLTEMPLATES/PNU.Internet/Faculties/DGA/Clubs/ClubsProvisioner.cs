using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty clubs lists on the CURRENT web:
    ///   FacultyResearch         - accordion items
    ///   FacultyClubsSections - h3 sub-sections (ItemKey -> SectionKey)
    ///   FacultyClubsSubItems - h4 blocks inside a sub-section (SectionKey)
    ///   FacultyClubsTable    - table rows inside a sub-section (SectionKey)
    ///   FacultyClubsImages   - images for an item OR a sub-section (OwnerKey)
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// All body text is PLAIN TEXT ("- " = bullet, "1." = numbered, blank line = paragraph).
    /// </summary>
    public static class ClubsProvisioner
    {
        public const string ItemsListName = "FacultyClubs";
        public const string SectionsListName = "FacultyClubsSections";
        public const string SubItemsListName = "FacultyClubsSubItems";
        public const string TableListName = "FacultyClubsTable";
        public const string ImagesListName = "FacultyClubsImages";
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

                            SPList items = EnsureList(web, ItemsListName, "Faculty clubs accordion items");
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "ItemKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "ItemKey", "BodyAr", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (items.ItemCount == 0) SeedItems(items);

                            SPList sections = EnsureList(web, SectionsListName, "Faculty clubs sub-sections");
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

                            SPList subs = EnsureList(web, SubItemsListName, "Faculty clubs sub-items");
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

                            SPList table = EnsureList(web, TableListName, "Faculty clubs table rows");
                            EnsureField(table, "SectionKey", SPFieldType.Text);
                            EnsureField(table, "Col2", SPFieldType.Note);
                            EnsureField(table, "Col3", SPFieldType.Text);
                            EnsureField(table, "SortOrder", SPFieldType.Number);
                            AddViewFields(table, "SectionKey", "Col2", "Col3", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(table);
                            if (table.ItemCount == 0) SeedTable(table);

                            SPList images = EnsureList(web, ImagesListName, "Faculty clubs images");
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
                    "ClubsProvisioner - EnsureLists", ex.Message);
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
            AddItem(list, "club1", "الأندية الطلابية التعليمية", "Educational Student Clubs", "", 1);
            AddItem(list, "club2", "النادي الرياضي", "Sports Club",
                "تسعى كلية علوم الحاسب والمعلومات إلى دعم مستهدفات رؤية المملكة في زيادة عدد ممارسي النشاط البدني من خلال النادي الرياضي الذي يخدم الطالبات وأعضاء الهيئتين التعليمية والإدارية. افتُتح النادي عام 1442هـ، ويهدف إلى تهيئة بيئة مناسبة لممارسة الأنشطة الرياضية، واستقطاب المواهب للمشاركة في فعاليات الجامعة، وتعزيز جودة الحياة والوصول إلى مجتمع صحي نشيط.\n\nالموقع: كلية علوم الحاسب والمعلومات، مبنى 170، الدور الثاني، 2.106.",
                2);
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
            AddSection(list, "club1", "club1-tracks", "مسارات نادي كلية علوم الحاسب والمعلومات",
                "Club Tracks", "", "", 1);
            AddSection(list, "club1", "club1-reg", "آلية التسجيل في الأندية الطلابية",
                "Registration Process",
                "تُطرح فرص الانضمام مرة واحدة في بداية كل عام دراسي وفق الآلية التالية:\n\n1. إعلان فتح التسجيل من الأسبوع الأول إلى الأسبوع الثالث عبر تعبئة النموذج الخاص بكل نادٍ.\n2. رفع أسماء الطالبات المرشحات إلى رئيسات الأندية.\n3. إجراء المفاضلة بين المرشحات وتصفية النتائج.\n4. التواصل مع المرشحات المقبولات عن طريق مشرفة النادي.",
                "", 2);
        }

        private static void AddSub(SPList list, string sectionKey, string ar, string en, string bodyAr, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = ar; it["TitleEn"] = en; it["SectionKey"] = sectionKey;
            it["BodyAr"] = bodyAr; it["SortOrder"] = sort; it.Update();
        }

        private static void SeedSubItems(SPList list)
        {
            AddSub(list, "club1-tracks", "مسار الأمن السيبراني", "Cybersecurity Track",
                "أحد فروع نادي الحاسب الآلي التابع لقسم تقنية المعلومات، يهتم بنشر الوعي بأهمية الأمن السيبراني والمهارات المطلوبة للتميز فيه، ويقدم أنشطة وفعاليات متخصصة.", 1);
            AddSub(list, "club1-tracks", "مسار إكسير هب", "Exir Hub Track",
                "أحد مسارات نادي الحاسب التابع لقسم علوم الحاسبات، ويهتم بإثراء الحصيلة المعرفية للطالبات في مجال التقنية، وتنمية الابتكار والإبداع والعمل الجماعي والمهارات الإدارية، والاستعداد لسوق العمل عبر مشاريع واقعية.", 2);
            AddSub(list, "club1-tracks", "مسار عقول التقنية", "Tech Minds Track",
                "أحد فروع نادي الحاسب الآلي التابع لقسم نظم المعلومات، يهتم بالتطورات التقنية ولغات البرمجة وتهيئة الطالبات لمتطلبات سوق العمل من خلال الدورات وورش العمل والحملات.", 3);
            AddSub(list, "club1-tracks", "نادي روبوتكس", "Robotics Club",
                "مبادرة ابتكارية تحت مركز الابتكار تُعنى بالروبوتات والتقنية الريادية وتحويل الابتكار إلى إنجازات تقنية.", 4);
            AddSub(list, "club1-tracks", "AI Hub – مجتمع الذكاء الاصطناعي", "AI Hub",
                "تجمع تقني لطالبات الجامعة في مجالات الذكاء الاصطناعي، يهدف إلى تطوير المهارات التقنية عبر التدريب والتطبيق، وتعزيز الريادة والأثر المجتمعي من خلال ورش تخصصية ومشاريع تطبيقية وفعاليات تفاعلية.", 5);
            AddSub(list, "club1-tracks", "مجتمع مطوري قوقل", "Google Developer Student Club",
                "أحد برامج Google العالمية المخصصة للطلاب، ويهدف إلى بناء مجتمع تقني طلابي يتيح تعلم تقنيات Google وتطبيقها عبر ورش العمل والجلسات والبرامج التدريبية والمشروعات الواقعية، ويرحب بالطالبات من مختلف التخصصات والمستويات التقنية.", 6);
        }

        private static void SeedTable(SPList list) { }

        private static void AddImage(SPList list, string ownerKey, string url, string alt, double sort)
        {
            SPListItem it = list.AddItem();
            it["Title"] = alt; it["OwnerKey"] = ownerKey;
            it["ImageUrl"] = new SPFieldUrlValue { Url = url, Description = alt };
            it["AltText"] = alt; it["SortOrder"] = sort; it.Update();
        }

        private static void SeedImages(SPList list)
        {
            const string SC = "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/Sports-Club/";
            AddImage(list, "club2", SC + "Sports-Club1.jpg", "النادي الرياضي في كلية علوم الحاسب والمعلومات", 1);
            AddImage(list, "club2", SC + "Sports-Club2.jpg", "تجهيزات النادي الرياضي", 2);
        }
    }
}
