using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    /// <summary>
    /// Central registry + reader for section titles used by the About/Agency controls.
    /// One list ("AboutSharedTitles") holds every title/label so editors change them in one place.
    /// Each row: TitleKey (unique code), TitleAr, TitleEn.
    /// </summary>
    public static class SharedTitles
    {
        public const string ListName = "AboutSharedTitles";

        // -------- Title keys (stable codes; do NOT rename after go-live) --------
        public const string OverviewHeading      = "Overview.Heading";
        public const string OverviewIntro        = "Overview.Intro";
        public const string VisionTitle          = "Overview.Vision.Title";
        public const string VisionBody           = "Overview.Vision.Body";
        public const string MissionTitle         = "Overview.Mission.Title";
        public const string MissionBody          = "Overview.Mission.Body";
        public const string ObjectivesTitle      = "Overview.Objectives.Title";

        public const string DeputyHeading        = "Deputy.Heading";
        public const string DeputyBody           = "Deputy.Body";
        public const string DeputyRoleTitle      = "Deputy.Role.Title";
        public const string DeputyRoleSubtitle   = "Deputy.Role.Subtitle";

        public const string TasksHeading         = "Tasks.Heading";

        private static readonly object _lock = new object();

        /// <summary>
        /// Ensures the shared titles list exists on the CURRENT web, seeds defaults once,
        /// and grants anonymous read. Idempotent. Call from OnInit (authenticated users only).
        /// </summary>
        public static void EnsureList(SPWeb web)
        {
            if (web == null && SPContext.Current != null) web = SPContext.Current.Web;
            if (web == null) return;
            lock (_lock)
            {
                try
                {
                    Guid siteId = web.Site.ID;
                    Guid webId = web.ID;

                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        using (var site = new SPSite(siteId))
                        using (var elevatedWeb = site.OpenWeb(webId))
                        {
                            elevatedWeb.AllowUnsafeUpdates = true;

                            var list = elevatedWeb.Lists.TryGetList(ListName);
                            if (list == null)
                            {
                                Guid id = elevatedWeb.Lists.Add(ListName, "Shared titles/labels for About & Agency controls.",
                                                        SPListTemplateType.GenericList);
                                list = elevatedWeb.Lists[id];
                                list.OnQuickLaunch = false;

                                EnsureField(list, "TitleKey", SPFieldType.Text);
                                EnsureField(list, "TitleAr",  SPFieldType.Note);
                                EnsureField(list, "TitleEn",  SPFieldType.Note);

                                var view = list.DefaultView;
                                var cols = view.ViewFields;
                                if (!cols.Exists("TitleKey")) cols.Add("TitleKey");
                                if (!cols.Exists("TitleAr"))  cols.Add("TitleAr");
                                if (!cols.Exists("TitleEn"))  cols.Add("TitleEn");
                                view.Update();

                                list.Update();
                            }
                            else
                            {
                                EnsureField(list, "TitleKey", SPFieldType.Text);
                                EnsureField(list, "TitleAr",  SPFieldType.Note);
                                EnsureField(list, "TitleEn",  SPFieldType.Note);
                            }

                            // Anonymous read
                            if (elevatedWeb.HasUniqueRoleAssignments == false)
                                list.BreakRoleInheritance(true, false);
                            list.AnonymousPermMask64 = SPBasePermissions.ViewListItems | SPBasePermissions.ViewPages;
                            list.Update();

                            if (list.ItemCount == 0)
                                Seed(list);

                            elevatedWeb.AllowUnsafeUpdates = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "SharedTitles.EnsureList", ex.Message);
                }
            }
        }

        private static void EnsureField(SPList list, string name, SPFieldType type)
        {
            if (!list.Fields.ContainsField(name))
                list.Fields.Add(name, type, false);
        }

        private static void Seed(SPList list)
        {
            Add(list, OverviewHeading,    "نظرة عامة عن وكالة الجامعة", "University Agency Overview");
            Add(list, OverviewIntro,
                "تتولى وكالة الجامعة الإشراف على الشؤون الإدارية والمالية والوحدات المرتبطة بها، والتنسيق بينها بما يضمن التكامل والاستفادة من الإمكانيات المتاحة.",
                "The University Agency oversees administrative and financial affairs and related units, coordinating among them to ensure integration and optimal use of available resources.");
            Add(list, VisionTitle, "الرؤية", "Vision");
            Add(list, VisionBody,
                "الريادة في تقديم منظومة إدارية ومالية مؤسسية كفؤة ومستدامة تدعم تميز الجامعة.",
                "Leadership in providing an efficient and sustainable institutional administrative and financial system that supports the university's excellence.");
            Add(list, MissionTitle, "الرسالة", "Mission");
            Add(list, MissionBody,
                "تطوير وإدارة الموارد والخدمات الإدارية والمالية للجامعة وفق أفضل الممارسات، من خلال حوكمة فاعلة وكفاءات مؤهلة وتقنيات حديثة؛ بما يسهم في رفع جودة الأداء وتحقيق أهداف الجامعة.",
                "Developing and managing the university's administrative and financial resources and services per best practices, through effective governance, qualified competencies, and modern technologies, contributing to raising performance quality and achieving the university's goals.");
            Add(list, ObjectivesTitle, "الأهداف", "Objectives");

            Add(list, DeputyHeading, "كلمة الوكيلة", "Deputy's Word");
            Add(list, DeputyBody,
                "تؤدي وكالة الجامعة دورًا محوريًا في دعم مسيرة الجامعة وتحقيق أهدافها الاستراتيجية، من خلال الإشراف على منظومة متكاملة من الأعمال الإدارية والمالية والخدمات المساندة. ونسعى إلى تطوير الإجراءات، ورفع كفاءة استخدام الموارد، وتعزيز الحوكمة والشفافية بما يضمن جودة الأداء واستدامته. كما نعمل بالشراكة مع قطاعات الجامعة المختلفة على بناء بيئة عمل مرنة ومحفزة، وتقديم خدمات موثوقة تلبي احتياجات المستفيدين وتسهم في الارتقاء بالتجربة الجامعية. ونؤمن بأن التكامل بين الكفاءات البشرية والتقنيات الحديثة هو الأساس لتحقيق التميز المؤسسي ودعم تطلعات الجامعة المستقبلية.",
                "The University Agency plays a pivotal role in supporting the university's journey and achieving its strategic goals through overseeing an integrated system of administrative, financial, and support services.");
            Add(list, DeputyRoleTitle,    "وكيلة الجامعة", "University Deputy");
            Add(list, DeputyRoleSubtitle, "جامعة الأميرة نورة بنت عبدالرحمن", "Princess Nourah bint Abdulrahman University");

            Add(list, TasksHeading, "المهام", "Main Tasks");
        }

        private static void Add(SPList list, string key, string ar, string en)
        {
            SPListItem item = list.AddItem();
            item["Title"]    = key;
            item["TitleKey"] = key;
            item["TitleAr"]  = ar;
            item["TitleEn"]  = en;
            item.Update();
        }
    }
}
