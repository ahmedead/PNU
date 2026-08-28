using System;
using System.Web;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.EntitySection
{
    /// <summary>
    /// Provisions two SharePoint lists for the Entity Section (Beneficiary Tracks):
    /// 1. Tracks/Groups List - stores tracks (TitleAr, TitleEn, DescriptionAr, DescriptionEn, ListStyle, SortOrder)
    /// 2. Bullets List - stores bullet points (TextAr, TextEn, GroupId, SortOrder)
    /// Idempotent list creation with seed data.
    /// </summary>
    public static class EntitySectionProvisioner
    {
        public const string DefaultTracksList = "EntitySectionTracks";
        public const string DefaultBulletsList = "EntitySectionBullets";
        private static readonly object _lock = new object();

        public static void EnsureLists(SPWeb web, string tracksListName = null, string bulletsListName = null)
        {
            if (web == null) return;
            if (string.IsNullOrWhiteSpace(tracksListName)) tracksListName = DefaultTracksList;
            if (string.IsNullOrWhiteSpace(bulletsListName)) bulletsListName = DefaultBulletsList;

            lock (_lock)
            {
                bool oldAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                try
                {
                    web.AllowUnsafeUpdates = true;

                    bool tracksCreated;
                    bool bulletsCreated;
                    var tracks = EnsureTracksList(web, tracksListName, out tracksCreated);
                    var bullets = EnsureBulletsList(web, bulletsListName, out bulletsCreated);

                    if (tracksCreated && tracks != null)
                    {
                        Seed(tracks, bullets);
                    }
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "EntitySectionProvisioner.EnsureLists", ex.Message);
                }
                finally
                {
                    try { web.AllowUnsafeUpdates = oldAllowUnsafeUpdates; } catch { }
                }
            }
        }

        private static SPList EnsureTracksList(SPWeb web, string listName, out bool created)
        {
            created = false;
            var list = web.Lists.TryGetList(listName);
            if (list == null)
            {
                Guid id = web.Lists.Add(listName, "Accordion panels for Entity Section tracks.", SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;

                EnsureField(list, "TitleAr", SPFieldType.Text);
                EnsureField(list, "TitleEn", SPFieldType.Text);
                EnsureField(list, "DescriptionAr", SPFieldType.Note);
                EnsureField(list, "DescriptionEn", SPFieldType.Note);
                EnsureField(list, "ListStyle", SPFieldType.Text); // "bullet" or "number"
                EnsureField(list, "SortOrder", SPFieldType.Number);
                list.Update();
                created = true;
            }
            else
            {
                EnsureField(list, "TitleAr", SPFieldType.Text);
                EnsureField(list, "TitleEn", SPFieldType.Text);
                EnsureField(list, "DescriptionAr", SPFieldType.Note);
                EnsureField(list, "DescriptionEn", SPFieldType.Note);
                EnsureField(list, "ListStyle", SPFieldType.Text);
                EnsureField(list, "SortOrder", SPFieldType.Number);
                list.Update();
            }

            EnsureDefaultViewFields(list, "TitleAr", "TitleEn", "DescriptionAr", "DescriptionEn", "ListStyle", "SortOrder");
            GrantAnonymous(list);
            return list;
        }

        private static SPList EnsureBulletsList(SPWeb web, string listName, out bool created)
        {
            created = false;
            var list = web.Lists.TryGetList(listName);
            if (list == null)
            {
                Guid id = web.Lists.Add(listName, "Bullet items belonging to Entity Section tracks.", SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;

                EnsureField(list, "TextAr", SPFieldType.Note);
                EnsureField(list, "TextEn", SPFieldType.Note);
                EnsureField(list, "GroupId", SPFieldType.Number);
                EnsureField(list, "SortOrder", SPFieldType.Number);
                list.Update();
                created = true;
            }
            else
            {
                EnsureField(list, "TextAr", SPFieldType.Note);
                EnsureField(list, "TextEn", SPFieldType.Note);
                EnsureField(list, "GroupId", SPFieldType.Number);
                EnsureField(list, "SortOrder", SPFieldType.Number);
                list.Update();
            }

            EnsureDefaultViewFields(list, "TextAr", "TextEn", "GroupId", "SortOrder");
            GrantAnonymous(list);
            return list;
        }

        private static void EnsureField(SPList list, string name, SPFieldType type)
        {
            if (!list.Fields.ContainsField(name))
                list.Fields.Add(name, type, false);
        }

        private static void EnsureDefaultViewFields(SPList list, params string[] fieldNames)
        {
            if (list == null || fieldNames == null || fieldNames.Length == 0) return;
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool updated = false;
                foreach (string name in fieldNames)
                {
                    if (list.Fields.ContainsField(name) && !view.ViewFields.Exists(name))
                    {
                        view.ViewFields.Add(name);
                        updated = true;
                    }
                }

                if (updated)
                {
                    view.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "EntitySectionProvisioner.EnsureDefaultViewFields", ex.Message);
            }
        }

        private static void GrantAnonymous(SPList list)
        {
            try
            {
                if (list != null && list.ParentWeb != null)
                {
                    list.ParentWeb.AllowUnsafeUpdates = true;
                    list.BreakRoleInheritance(true, false);
                    list.AnonymousPermMask64 = SPBasePermissions.ViewListItems | SPBasePermissions.ViewPages;
                    list.Update();
                }
            }
            catch { }
        }

        private static void Seed(SPList tracks, SPList bullets)
        {
            // Track 1: مسار القبول
            AddTrack(tracks, bullets, 1,
                "مسار القبول",
                "Admission Track",
                "يبدأ التقديم للجامعة إلكترونيًا عبر المنصة الوطنية الموحدة للقبول، مع إتاحة معلومات البرامج ومعايير القبول والنسبة الموزونة بشفافية.",
                "Applications to the university begin electronically via the Unified National Admission Platform, providing transparent information on programs, admission criteria, and weighted ratios.",
                "bullet",
                new[]
                {
                    "قبول خريجات الثانوية العامة.",
                    "المنح الداخلية والمنح الخارجية.",
                    "قبول الطالبات الموهوبات.",
                    "الدراسة بنظام الزيارة من خارج الجامعة.",
                    "التحويل إلى جامعة الأميرة نورة بنت عبدالرحمن."
                },
                new[]
                {
                    "Admission of high school graduates.",
                    "Internal and external scholarships.",
                    "Admission of gifted female students.",
                    "Visiting student system from outside the university.",
                    "Transferring to Princess Nourah bint Abdulrahman University."
                });

            // Track 2: مسار الطالبات المستجدات
            AddTrack(tracks, bullets, 2,
                "مسار الطالبات المستجدات",
                "Freshmen Students Track",
                "",
                "",
                "bullet",
                new[]
                {
                    "إرشادات الطالبة المستجدة وخطوات بدء الدراسة.",
                    "التعريف بالأنظمة الجامعية والخدمات الإلكترونية.",
                    "إجراءات الانقطاع والانسحاب خلال المرحلة الأولى من الدراسة."
                },
                new[]
                {
                    "Freshman student instructions and steps to start study.",
                    "Introduction to university regulations and e-services.",
                    "Withdrawal and leave procedures during the first phase of study."
                });

            // Track 3: مسار الطالبات المستمرات
            AddTrack(tracks, bullets, 3,
                "مسار الطالبات المستمرات",
                "Continuing Students Track",
                "",
                "",
                "bullet",
                new[]
                {
                    "الحذف والإضافة وتعديل الجداول.",
                    "الاعتذار عن مقرر أو فصل، والتأجيل، والانقطاع، وإعادة القيد.",
                    "التحويل الداخلي وتغيير التخصص والدراسة بنظام الزيارة.",
                    "الاختبارات النهائية والنتائج والتسجيل المبكر والمكافآت والإرشاد الأكاديمي."
                },
                new[]
                {
                    "Course drop/add and schedule modifications.",
                    "Course/semester apology, postponement, interruption, and re-enrollment.",
                    "Internal transfer, change of major, and visiting student system.",
                    "Final exams, results, early registration, allowances, and academic counseling."
                });

            // Track 4: مسار الخريجات
            AddTrack(tracks, bullets, 4,
                "مسار الخريجات",
                "Graduates Track",
                "",
                "",
                "bullet",
                new[]
                {
                    "الإرشاد الأكاديمي للخريجات ودليل الخريجة.",
                    "استلام وثيقة التخرج وإعادة إصدارها.",
                    "طلب ترجمة وثيقة التخرج."
                },
                new[]
                {
                    "Academic counseling for graduates and graduate handbook.",
                    "Receiving and re-issuing graduation certificates.",
                    "Graduation certificate translation requests."
                });
        }

        private static void AddTrack(SPList tracks, SPList bullets, int order,
            string titleAr, string titleEn,
            string descAr, string descEn,
            string listStyle,
            string[] bulletsAr, string[] bulletsEn)
        {
            SPListItem t = tracks.AddItem();
            t["Title"] = "TRK-" + order;
            t["TitleAr"] = titleAr;
            t["TitleEn"] = titleEn;
            t["DescriptionAr"] = descAr;
            t["DescriptionEn"] = descEn;
            t["ListStyle"] = listStyle;
            t["SortOrder"] = order;
            t.Update();

            int trackId = t.ID;
            if (bullets == null || bulletsAr == null) return;

            for (int i = 0; i < bulletsAr.Length; i++)
            {
                SPListItem b = bullets.AddItem();
                b["Title"] = "TRK-" + order + "-B" + (i + 1);
                b["TextAr"] = bulletsAr[i];
                b["TextEn"] = (bulletsEn != null && i < bulletsEn.Length) ? bulletsEn[i] : bulletsAr[i];
                b["GroupId"] = trackId;
                b["SortOrder"] = i + 1;
                b.Update();
            }
        }
    }
}
