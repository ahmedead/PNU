using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty achievements lists on the CURRENT web:
    ///   FacultyFacilities       - accordion items (Title + plain body + order)
    ///   FacultyFacilitiesImages - images per item (matched by ItemKey)
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// </summary>
    public static class FacilitiesProvisioner
    {
        public const string ItemsListName = "FacultyFacilities";
        public const string ImagesListName = "FacultyFacilitiesImages";
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

                            SPList items = EnsureList(web, ItemsListName, "Faculty facilities items");
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "ItemKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "ItemKey", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (items.ItemCount == 0) SeedItems(items);

                            SPList images = EnsureList(web, ImagesListName, "Faculty facilities images");
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
                    "FacilitiesProvisioner - EnsureLists", ex.Message);
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

        private static void AddItem(SPList list, string key, string ar, string en, string bodyAr, string bodyEn, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["ItemKey"] = key;
            item["BodyAr"] = bodyAr;
            item["BodyEn"] = bodyEn;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedItems(SPList list)
        {
            AddItem(list, "fac1", "مركز الذكاء الاصطناعي", "AI Center",
                "الرابط الرسمي لصفحة مركز الذكاء الاصطناعي.", "Official page of the AI Center.", 1);
            AddItem(list, "fac2", "معامل تخصصية", "Specialized Labs",
                "الرابط الرسمي لصفحة معامل تخصصية ضمن مرافق الكلية.", "Official page of the specialized labs.", 2);
            AddItem(list, "fac3", "مرافق الجامعة", "University Facilities",
                "الرابط الرسمي لصفحة مرافق الجامعة ضمن مرافق الكلية.", "Official page of the university facilities.", 3);
        }

        private static void SeedImages(SPList list) { }

    }
}
