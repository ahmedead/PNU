using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Provisions the faculty org-structure lists on the CURRENT web and seeds defaults:
    ///   FacultyOrgStructure       - accordion items (Title + plain-text body + order)
    ///   FacultyOrgStructureImages - images belonging to an item (matched by ItemKey)
    /// Idempotent; seeds only when each list is empty. Grants anonymous read.
    /// Body text is stored as PLAIN TEXT (Note); the .ascx adds the HTML on display.
    /// </summary>
    public static class OrgStructureProvisioner
    {
        public const string ItemsListName = "FacultyOrgStructure";
        public const string ImagesListName = "FacultyOrgStructureImages";
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

                            SPList items = EnsureList(web, ItemsListName, "Faculty org-structure accordion items");
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "ItemKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "ItemKey", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (items.ItemCount == 0) SeedItems(items);

                            SPList images = EnsureList(web, ImagesListName, "Faculty org-structure images");
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
                    "OrgStructureProvisioner - EnsureLists", ex.Message);
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
                    if (f != null) { f.RichText = false; f.NumberOfLines = 8; f.Update(); }
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

        private static void SeedItems(SPList list)
        {
            SPListItem item = list.AddItem();
            item["Title"] = "الهيكل التنظيمي";
            item["TitleEn"] = "Organizational Structure";
            item["ItemKey"] = "org";
            item["BodyAr"] = "يعرض الهيكل التنظيمي موقع الوحدات والارتباطات الإدارية داخل الكلية.";
            item["BodyEn"] = "The organizational structure shows the units and administrative relationships within the faculty.";
            item["SortOrder"] = 1;
            item.Update();
        }

        private static void SeedImages(SPList list)
        {
            SPListItem item = list.AddItem();
            item["Title"] = "الهيكل التنظيمي";
            item["ItemKey"] = "org";
            item["ImageUrl"] = new SPFieldUrlValue
            {
                Url = "https://pnu.edu.sa/ar/Faculties/IT/PublishingImages/Pages/Orgs/azibrahim3324.png",
                Description = "الهيكل التنظيمي"
            };
            item["SortOrder"] = 1;
            item.Update();
        }
    }
}
