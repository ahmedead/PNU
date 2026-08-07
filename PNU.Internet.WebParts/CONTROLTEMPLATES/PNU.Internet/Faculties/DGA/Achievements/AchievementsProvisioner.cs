using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty achievements lists on the CURRENT web:
    ///   FacultyAchievements       - accordion items (Title + plain body + order)
    ///   FacultyAchievementsImages - images per item (matched by ItemKey)
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// </summary>
    public static class AchievementsProvisioner
    {
        public const string ItemsListName = "FacultyAchievements";
        public const string ImagesListName = "FacultyAchievementsImages";
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

                            SPList items = EnsureList(web, ItemsListName, "Faculty achievements accordion items");
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "ItemKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "ItemKey", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (items.ItemCount == 0) SeedItems(items);

                            SPList images = EnsureList(web, ImagesListName, "Faculty achievements images");
                            EnsureField(images, "ItemKey", SPFieldType.Text);
                            EnsureField(images, "ImageUrl", SPFieldType.URL);
                            EnsureField(images, "SortOrder", SPFieldType.Number);
                            AddViewFields(images, "ItemKey", "ImageUrl", "SortOrder");
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
                    "AchievementsProvisioner - EnsureLists", ex.Message);
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

        // ---------- seeds (PLAIN TEXT: blank line = new paragraph) ----------

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
            AddItem(list, "ach1",
                "الفوز بالمركز الثاني في هاكاثون التوعية بالثورة الصناعية الرابعة - مسار الطاقة",
                "Second place - Fourth Industrial Revolution Hackathon (Energy track)",
                "حققت خريجات قسم الهندسة الكهربائية لعام 1445 هـ، المهندسة سارة السلمان والمهندسة ريما التويجري، المركز الثاني في هاكاثون التوعية بالثورة الصناعية الرابعة في مسار الطاقة، الذي نظمته وزارة الاتصالات وتقنية المعلومات، ووزارة الصناعة، وبرنامج تطوير الصناعة الوطنية والخدمات اللوجستية.\n\nالتاريخ: يوليو 2024 - أغسطس 2024",
                1);

            AddItem(list, "ach2",
                "الفوز بالمركز الأول والمركز الثاني في حفل تخرج برنامج التدريب الصيفي",
                "First and second place - Summer Training Program graduation",
                "شاركت طالبات قسم الهندسة الكهربائية في حفل تخرج برنامج التدريب الصيفي الذي نظمته شركة نوكيا بالتعاون مع هيئة الأمم المتحدة للمرأة وبالتنسيق مع وزارة الاتصالات وتقنية المعلومات.\n\nوقد تمكنت الطالبات من تحقيق المركزين الأول والثاني، حيث حصل على المركز الأول الطالبات منار القحطاني، الجوزاء الدوسري، وريهام المزروع، بينما حققت المركز الثاني الطالبات غادة المنيع، سما العرفج، وشهد الحديثي.\n\nالتاريخ: فترة صيف 1445 هـ من يوليو 2024 إلى أغسطس 2024",
                2);

            AddItem(list, "ach3",
                "الفوز بالمركز الأول في هاكاثون التوعية بالثورة الصناعية الرابعة - مسار الطاقة",
                "First place - Fourth Industrial Revolution Hackathon (Energy track)",
                "حققت طالبات قسم الهندسة الصناعية والنظم، ترف بن غالي، دارين السحيباني، المركز الأول في هاكاثون التوعية بالثورة الصناعية الرابعة في مسار الطاقة، الذي نظمته وزارة الاتصالات وتقنية المعلومات، ووزارة الصناعة، وبرنامج تطوير الصناعة الوطنية والخدمات اللوجستية.\n\nالفريق مكون من 7 طالبات، 3 من جامعة الأميرة نورة بنت عبدالرحمن.\n\nالتاريخ: يوليو 2024 - أغسطس 2024",
                3);

            AddItem(list, "ach4",
                "فوز مجموعة من طالبات كلية الهندسة بجائزة الحكام في الدورة الثالثة للألعاب السعودية",
                "Referees' award - 3rd Saudi Games",
                "أقيمت مسابقة الألعاب السعودية بدورتها الثالثة عن فئة الروبوت، وذلك يومي الأربعاء والخميس الموافق 16 - 17 أكتوبر 2024م في البوليفارد سيتي.\n\nتم ترشيح عدد من طالبات كلية الهندسة من قبل إدارة الشؤون الرياضية بجامعة الأميرة نورة بنت عبدالرحمن، وفزن بجائزة الحكام بالمركز العاشر.\n\nالطالبات الفائزات هن: أسيل البريدي، ليان السلطان، نجود الشلوي، طيف الرماح، بثينة الدوسري، عبير العمار، نوره الصعب، داليا مساوي، راما الزايدي، وجود الزامل.\n\nتميزت الطالبات بأدائهن الرائع وأظهرن قدرات عالية في التخطيط والاستراتيجية، مما لفت انتباه لجنة التحكيم.",
                4);

            AddItem(list, "ach5",
                "فوز طالبة كلية الهندسة في مسابقة أفضل مشروع استشاري في برنامج جسور القادة",
                "Best consulting project - Jusoor Leaders program",
                "حققت الطالبة ريم بنت مساعد آل ضايم من كلية الهندسة بجامعة الأميرة نورة بنت عبدالرحمن إنجازًا بحصولها على جائزة أفضل مشروع استشاري ضمن برنامج جسور القادة، الذي يُعد أحد أبرز برامج مبادرة طويق لتأهيل القيادات.\n\nتميز مشروعها الاستشاري Global Guidance Consultancy بقدرته على تقديم حلول مبتكرة وفعالة في مجال الاستشارات العالمية، مما أهّلها للحصول على المركز الأول في المسابقة.",
                5);

            AddItem(list, "ach6",
                "إقامة ندوة عن تقنية وأنظمة الروبوتات بجامعة الأميرة نورة",
                "Robotics technology and systems seminar",
                "أقام نادي تحكم للروبوت والرياضات اللاسلكية في كلية الهندسة أولى ندواته تحت سلسلة تقنية بعنوان \"تقنية وأنظمة الروبوتات\"، وذلك يوم الأربعاء 20 نوفمبر 2024، في مركز الأبحاث بالجامعة، قدمها مهندس الميكاترونكس وخبير الروبوتات محمد التلا.\n\nتناولت الندوة أحدث التطورات في تقنيات الروبوتات، بما في ذلك أنظمة الذكاء الاصطناعي، التطبيقات الصناعية، والتحديات التقنية.\n\nكما تضمنت الندوة ورش عمل تفاعلية حول تصميم وبرمجة الروبوتات.",
                6);

            AddItem(list, "ach7",
                "فوز مجموعة من طالبات كلية الهندسة في المسابقة البحثية العالمية",
                "First place - International research competition",
                "حققت خمس طالبات من كلية الهندسة، قسم الهندسة الكهربائية، بإشراف سعادة الدكتور يزن علاوي، المركز الأول في المسابقة الدولية البحثية العالمية ضمن فعاليات المنتدى الثاني الدولي للشبكات غير الأرضية.\n\nنظمت المسابقة منظومة الاتصالات وتقنية المعلومات ممثلة بهيئة الاتصالات والتقنية والفضاء، بالتعاون مع الاتحاد الدولي للاتصالات.\n\nشهدت المسابقة منافسة قوية بمشاركة 90 فريقًا بحثيًا من 30 دولة.",
                7);
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
            AddImage(list, "ach4", "https://pnu.edu.sa/ar/Faculties/EN/PublishingImages/Pages/Achievements/game1.webp", 1);
            AddImage(list, "ach4", "https://pnu.edu.sa/ar/Faculties/EN/PublishingImages/Pages/Achievements/game2.webp", 2);
            AddImage(list, "ach5", "https://pnu.edu.sa/ar/Faculties/EN/PublishingImages/Pages/Achievements/27-j.webp", 1);
            AddImage(list, "ach6", "https://pnu.edu.sa/ar/Faculties/EN/PublishingImages/Pages/Achievements/ropots-27.webp", 1);
            AddImage(list, "ach7", "https://pnu.edu.sa/ar/Faculties/EN/PublishingImages/Pages/Achievements/Achievement-5-1-2025a.webp", 1);
            AddImage(list, "ach7", "https://pnu.edu.sa/ar/Faculties/EN/PublishingImages/Pages/Achievements/Achievement-5-1-2025a2.webp", 2);
        }
    }
}
