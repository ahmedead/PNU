using System;
using System.Runtime.Remoting.Metadata;
using System.Web;
using Microsoft.SharePoint;
using static PNU.Internet.WebParts.SPFactory;

namespace PNU.Internet.WebParts
{
    /// <summary>
    /// Model for the CollegeAboutSections list.
    /// Title       = Arabic section title (built-in Title field)
    /// SectionKey  = Vision | Mission | Goals | DeanWord (used by the control to place content)
    /// Goals content: one goal per line inside Description/DescriptionEn.
    /// </summary>
    public class CollegeAboutItem
    {
        public int ID { get; set; }
        public string Title { get; set; }                 // Arabic title
        public string TitleEn { get; set; }

        [SPField(SPFieldType.Note)]
        public string Description { get; set; }           // Arabic body

        [SPField(SPFieldType.Note)]
        public string DescriptionEn { get; set; }

        public string SectionKey { get; set; }
        public string PersonName { get; set; }            // DeanWord only
        public string PersonNameEn { get; set; }
        public string PersonPosition { get; set; }        // DeanWord only
        public string PersonPositionEn { get; set; }
        public double SortOrder { get; set; }
    }

    public static class CollegeAboutProvisioner
    {
        public const string ListName = "CollegeAboutSections";
        private static readonly object _lock = new object();

        /// <summary>
        /// Ensures the list exists on the CURRENT web with all fields and seeds
        /// default data when empty. Safe to call from OnInit / Page_Load.
        /// </summary>
        public static void EnsureList()
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

                            SPList list = web.Lists.TryGetList(ListName);
                            if (list == null)
                            {
                                Guid id = web.Lists.Add(ListName, "College about page sections",
                                    SPListTemplateType.GenericList);
                                list = web.Lists[id];
                                list.OnQuickLaunch = false;
                                list.Update();
                            }

                            // Fields + default view (idempotent)
                            SPFactory.MapListFieldsFromClass<CollegeAboutItem>(list);

                            // Seed defaults only once
                            if (list.ItemCount == 0)
                                SeedDefaults(list);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "CollegeAboutProvisioner - EnsureList", ex.Message);
            }
        }

        private static void SeedDefaults(SPList list)
        {
            AddItem(list, "الرؤية", "Vision", "Vision", 1,
                "منارة المرأة للتميز البحثي والعلمي في علوم الحاسب والمعلومات.",
                "A beacon for women's research and scientific excellence in computer and information sciences.");

            AddItem(list, "الرسالة", "Mission", "Mission", 2,
                "كلية متخصصة في التعليم وإعداد الأبحاث في مجال علوم الحاسب والمعلومات، تعنى بتأهيل كوادر نسائية متميزة علميًا وبحثيًا وتقنيًا في بيئة تعليمية تفاعلية تدار بكفاءات مؤهلة للمساهمة.",
                "A specialized college in education and research in computer and information sciences, dedicated to preparing distinguished female cadres in an interactive learning environment.");

            AddItem(list, "الأهداف", "Goals", "Goals", 3,
                "تطوير برامج أكاديمية لمواكبة التسارع في علوم الحاسب وتقنية المعلومات." + "\n" +
                "تنمية مهارات المستقبل ورفع القدرة التنافسية." + "\n" +
                "تعزيز فرص الاستثمار والشراكات المستدامة في الكلية." + "\n" +
                "تطوير الأنشطة الأكاديمية والبحثية لخدمة المجتمع." + "\n" +
                "تطوير بيئة معززة تقنيًا ومعرفيًا داعمة للصحة والرفاهية.",
                "Develop academic programs to keep pace with advances in computing and IT." + "\n" +
                "Develop future skills and raise competitiveness." + "\n" +
                "Promote investment opportunities and sustainable partnerships." + "\n" +
                "Develop academic and research activities to serve the community." + "\n" +
                "Develop a technically and cognitively enhanced environment supporting health and wellbeing.");

            AddItem(list, "كلمة العميدة", "Dean's Welcome", "DeanWord", 4,
                "يمثل قطاع علوم الحاسب والمعلومات أحد أهم القطاعات النشطة والمؤثرة في نهضة وتطور المجتمعات، وتعد كلية علوم الحاسب والمعلومات بجامعة الأميرة نورة بنت عبد الرحمن إحدى الكليات الرائدة للبنات على المستوى الوطني والإقليمي والدولي في هذا المجال...",
                "The computer and information sciences sector is one of the most active and influential sectors in the advancement of societies...",
                "د. منال بنت عبدالله العوهلي", "Dr. Manal bint Abdullah Al-Awhali",
                "عميدة كلية علوم الحاسب والمعلومات", "Dean of the College of Computer and Information Sciences");
        }

        private static void AddItem(SPList list, string titleAr, string titleEn, string key,
            double sort, string descAr, string descEn,
            string nameAr = "", string nameEn = "", string posAr = "", string posEn = "")
        {
            SPListItem item = list.AddItem();
            item["Title"] = titleAr;
            item["TitleEn"] = titleEn;
            item["Description"] = descAr;
            item["DescriptionEn"] = descEn;
            item["SectionKey"] = key;
            item["SortOrder"] = sort;
            item["PersonName"] = nameAr;
            item["PersonNameEn"] = nameEn;
            item["PersonPosition"] = posAr;
            item["PersonPositionEn"] = posEn;
            item.Update();
        }
    }
}
