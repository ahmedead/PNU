using System;
using System.Web;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    /// <summary>
    /// Provisions two lists for the Tasks accordion:
    ///   SharedTaskGroups  – one row per accordion panel (TitleAr/TitleEn/SortOrder)
    ///   SharedTaskPoints  – bullet points, linked to a group via numeric GroupId + SortOrder
    /// Idempotent; seeds once; grants anonymous read.
    /// </summary>
    public static class AgencyTasksProvisioner
    {
        public const string GroupsList = "SharedTaskGroups";
        public const string PointsList = "SharedTaskPoints";
        private const string ProvisionedKey = "PNU_SharedTasks_Provisioned";
        private static readonly object _lock = new object();

        public static void EnsureLists(SPWeb web)
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
                            // If already provisioned, do not recreate deleted lists or re-seed deleted items
                            if (elevatedWeb.AllProperties.ContainsKey(ProvisionedKey))
                                return;

                            // If lists already exist, mark as provisioned so deleted items won't re-seed
                            if (elevatedWeb.Lists.TryGetList(GroupsList) != null ||
                                elevatedWeb.Lists.TryGetList(PointsList) != null)
                            {
                                elevatedWeb.AllowUnsafeUpdates = true;
                                elevatedWeb.AllProperties[ProvisionedKey] = "1";
                                elevatedWeb.Update();
                                elevatedWeb.AllowUnsafeUpdates = false;
                                return;
                            }

                            elevatedWeb.AllowUnsafeUpdates = true;

                            var groups = EnsureGroups(elevatedWeb);
                            var points = EnsurePoints(elevatedWeb);

                            if (groups != null)
                                Seed(groups, points);

                            elevatedWeb.AllProperties[ProvisionedKey] = "1";
                            elevatedWeb.Update();

                            elevatedWeb.AllowUnsafeUpdates = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "AgencyTasksProvisioner.EnsureLists", ex.Message);
                }
            }
        }

        private static SPList EnsureGroups(SPWeb web)
        {
            var list = web.Lists.TryGetList(GroupsList);
            if (list == null)
            {
                Guid id = web.Lists.Add(GroupsList, "Accordion panels for the Agency tasks section.",
                                        SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;

                EnsureField(list, "TitleAr",   SPFieldType.Text);
                EnsureField(list, "TitleEn",   SPFieldType.Text);
                EnsureField(list, "SortOrder", SPFieldType.Number);

                var cols = list.DefaultView.ViewFields;
                if (!cols.Exists("TitleAr"))   cols.Add("TitleAr");
                if (!cols.Exists("TitleEn"))   cols.Add("TitleEn");
                if (!cols.Exists("SortOrder")) cols.Add("SortOrder");
                list.DefaultView.Update();
                list.Update();
            }
            GrantAnonymous(list);
            return list;
        }

        private static SPList EnsurePoints(SPWeb web)
        {
            var list = web.Lists.TryGetList(PointsList);
            if (list == null)
            {
                Guid id = web.Lists.Add(PointsList, "Bullet points belonging to Agency task panels.",
                                        SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;

                EnsureField(list, "TextAr",    SPFieldType.Note);
                EnsureField(list, "TextEn",    SPFieldType.Note);
                EnsureField(list, "GroupId",   SPFieldType.Number);
                EnsureField(list, "SortOrder", SPFieldType.Number);

                var cols = list.DefaultView.ViewFields;
                if (!cols.Exists("TextAr"))    cols.Add("TextAr");
                if (!cols.Exists("TextEn"))    cols.Add("TextEn");
                if (!cols.Exists("GroupId"))   cols.Add("GroupId");
                if (!cols.Exists("SortOrder")) cols.Add("SortOrder");
                list.DefaultView.Update();
                list.Update();
            }
            GrantAnonymous(list);
            return list;
        }

        private static void EnsureField(SPList list, string name, SPFieldType type)
        {
            if (!list.Fields.ContainsField(name))
                list.Fields.Add(name, type, false);
        }

        private static void GrantAnonymous(SPList list)
        {
            list.BreakRoleInheritance(true, false);
            list.AnonymousPermMask64 = SPBasePermissions.ViewListItems | SPBasePermissions.ViewPages;
            list.Update();
        }

        private static void Seed(SPList groups, SPList points)
        {
            AddGroup(groups, points, 1, "التخطيط", "Planning", new[]
            {
                "إعداد الخطط السنوية والخمسية لنشاطات الإدارات ومتابعة تنفيذها بعد اعتمادها.",
                "إعداد البرامج للوحدات الإدارية المرتبطة بالوكالة ومتابعة تنفيذها بعد اعتمادها."
            }, new[]
            {
                "Preparing annual and five-year plans for departmental activities and following up on their implementation after approval.",
                "Preparing programs for the administrative units affiliated with the agency and following up on their implementation after approval."
            });

            AddGroup(groups, points, 2, "الرقابة", "Oversight", new[]
            {
                "الإشراف على تطبيق الأنظمة واللوائح والتعليمات المعتمدة في مجال الشؤون الإدارية والمالية في الجامعة.",
                "الإشراف على الوحدات الإدارية المرتبطة بالوكالة."
            }, new[]
            {
                "Supervising the application of approved regulations, bylaws, and instructions in administrative and financial affairs.",
                "Supervising the administrative units affiliated with the agency."
            });

            AddGroup(groups, points, 3, "التنسيق", "Coordination", new[]
            {
                "العمل على التنسيق بين الوحدات الإدارية المرتبطة بالوكالة بما يضمن التكامل بينها والاستفادة القصوى من الإمكانيات المتاحة لها.",
                "العمل على التنسيق مع وكالات الجامعة في كل ما يتعلق بالخدمات الإدارية والمالية."
            }, new[]
            {
                "Coordinating among affiliated administrative units to ensure integration and maximum use of available resources.",
                "Coordinating with the university's agencies on all administrative and financial services."
            });

            AddGroup(groups, points, 4, "تحديد المتطلبات", "Requirements Definition", new[]
            {
                "حصر احتياجات الوكالة من القوى العاملة والأجهزة والمواد ومتابعة توفيرها.",
                "حصر الاحتياجات التدريبية لموظفي وموظفات الوكالة والتنسيق مع عمادة التطوير والجودة بشأن الترشيح للبرامج التي تلبي تلك الاحتياجات."
            }, new[]
            {
                "Identifying the agency's needs for workforce, equipment, and materials and following up on providing them.",
                "Identifying training needs for agency staff and coordinating with the Deanship of Development and Quality on nominations for suitable programs."
            });

            AddGroup(groups, points, 5, "إعداد التقارير", "Reporting", new[]
            {
                "الاشتراك في اللجان المتعلقة بنشاطات الوكالة وإعداد تقارير دورية عن نشاطات الوكالة وإنجازاتها ومقترحات تطويرها ورفعها لمديرة الجامعة."
            }, new[]
            {
                "Participating in committees related to the agency's activities and preparing periodic reports on its activities, achievements, and development proposals, and submitting them to the university president."
            });
        }

        private static void AddGroup(SPList groups, SPList points, int order,
                                     string titleAr, string titleEn, string[] pointsAr, string[] pointsEn)
        {
            SPListItem g = groups.AddItem();
            g["Title"]     = "GRP-" + order;
            g["TitleAr"]   = titleAr;
            g["TitleEn"]   = titleEn;
            g["SortOrder"] = order;
            g.Update();

            int groupId = g.ID;
            if (points == null) return;

            for (int i = 0; i < pointsAr.Length; i++)
            {
                SPListItem p = points.AddItem();
                p["Title"]     = "GRP-" + order + "-P" + (i + 1);
                p["TextAr"]    = pointsAr[i];
                p["TextEn"]    = (pointsEn != null && i < pointsEn.Length) ? pointsEn[i] : pointsAr[i];
                p["GroupId"]   = groupId;
                p["SortOrder"] = i + 1;
                p.Update();
            }
        }
    }
}
