using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    /// <summary>
    /// Provisions the Overview control's data on the current web:
    ///   SharedObjectives       – bullet items (TextAr/TextEn/SortOrder)
    ///   SharedOverviewSettings – single-row settings incl. the overview image (ImageUrl/ImageAlt)
    /// Idempotent; seeds once; grants anonymous read.
    /// </summary>
    public static class AgencyOverviewProvisioner
    {
        public const string ObjectivesList = "SharedObjectives";
        public const string SettingsList   = "SharedOverviewSettings";

        // Default image shipped with the solution (used to seed the settings row).
        private const string DefaultImageUrl = "/_layouts/15/PNU.Internet/images/hero/hero-campus-sm.webp";
        private const string DefaultImageAlt = "حرم جامعة الأميرة نورة";

        private static readonly object _lock = new object();

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
                            EnsureObjectives(elevatedWeb);
                            EnsureSettings(elevatedWeb);
                            elevatedWeb.AllowUnsafeUpdates = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "AgencyOverviewProvisioner.EnsureList", ex.Message);
                }
            }
        }

        private static void EnsureObjectives(SPWeb web)
        {
            var list = web.Lists.TryGetList(ObjectivesList);
            if (list == null)
            {
                Guid id = web.Lists.Add(ObjectivesList, "Objective bullet items for the shared overview.",
                                        SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;

                EnsureField(list, "TextAr",    SPFieldType.Note);
                EnsureField(list, "TextEn",    SPFieldType.Note);
                EnsureField(list, "SortOrder", SPFieldType.Number);

                var cols = list.DefaultView.ViewFields;
                if (!cols.Exists("TextAr"))    cols.Add("TextAr");
                if (!cols.Exists("TextEn"))    cols.Add("TextEn");
                if (!cols.Exists("SortOrder")) cols.Add("SortOrder");
                list.DefaultView.Update();
                list.Update();
            }
            else
            {
                EnsureField(list, "TextAr",    SPFieldType.Note);
                EnsureField(list, "TextEn",    SPFieldType.Note);
                EnsureField(list, "SortOrder", SPFieldType.Number);
            }

            GrantAnonymous(list);

            if (list.ItemCount == 0)
                SeedObjectives(list);
        }

        private static void EnsureSettings(SPWeb web)
        {
            var list = web.Lists.TryGetList(SettingsList);
            if (list == null)
            {
                Guid id = web.Lists.Add(SettingsList, "Single-row settings for the shared overview (image, etc.).",
                                        SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;

                EnsureField(list, "ImageUrl", SPFieldType.URL);
                EnsureField(list, "ImageAlt", SPFieldType.Text);

                var cols = list.DefaultView.ViewFields;
                if (!cols.Exists("ImageUrl")) cols.Add("ImageUrl");
                if (!cols.Exists("ImageAlt")) cols.Add("ImageAlt");
                list.DefaultView.Update();
                list.Update();
            }
            else
            {
                EnsureField(list, "ImageUrl", SPFieldType.URL);
                EnsureField(list, "ImageAlt", SPFieldType.Text);
            }

            GrantAnonymous(list);

            if (list.ItemCount == 0)
                SeedSettings(list);
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

        private static void SeedSettings(SPList list)
        {
            SPListItem item = list.AddItem();
            item["Title"] = "Overview";
            // SPFieldType.URL stores "url, description"
            item["ImageUrl"] = new SPFieldUrlValue { Url = DefaultImageUrl, Description = DefaultImageAlt };
            item["ImageAlt"] = DefaultImageAlt;
            item.Update();
        }

        private static void SeedObjectives(SPList list)
        {
            Add(list, 1, "رفع كفاءة التخطيط وإدارة الموارد المالية والإدارية.",
                         "Raising the efficiency of planning and managing financial and administrative resources.");
            Add(list, 2, "تعزيز الحوكمة والالتزام بالأنظمة واللوائح ورفع مستوى الشفافية.",
                         "Enhancing governance, compliance with regulations, and raising the level of transparency.");
            Add(list, 3, "تطوير جودة الخدمات المقدمة للقطاعات الجامعية والمستفيدين.",
                         "Improving the quality of services provided to university sectors and beneficiaries.");
            Add(list, 4, "دعم التحول الرقمي وتبسيط الإجراءات ورفع الكفاءة التشغيلية.",
                         "Supporting digital transformation, simplifying procedures, and raising operational efficiency.");
            Add(list, 5, "تنمية قدرات الكوادر وتعزيز بيئة عمل محفزة ومتكاملة.",
                         "Developing staff capabilities and fostering a motivating, integrated work environment.");
        }

        private static void Add(SPList list, int order, string ar, string en)
        {
            SPListItem item = list.AddItem();
            item["Title"]     = "OBJ-" + order;
            item["TextAr"]    = ar;
            item["TextEn"]    = en;
            item["SortOrder"] = order;
            item.Update();
        }
    }
}
