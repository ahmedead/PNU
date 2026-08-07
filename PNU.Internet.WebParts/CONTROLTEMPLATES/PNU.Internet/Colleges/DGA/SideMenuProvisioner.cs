using Microsoft.SharePoint;
using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// Provisions the "CollegeSideMenu" list on the current college web.
    /// Called from ucSideMenu.OnInit (page provisioning - no event receiver, no PowerShell).
    /// Idempotent: fields guarded by ContainsField, seed only when the list is empty.
    ///
    /// Structure: ParentID = 0 for root items; children carry the ID of their parent.
    /// The "Sections" root is added ONCE - its level-2 entries come from code.
    /// </summary>
    public static class SideMenuProvisioner
    {
        public const string LIST_NAME = "CollegeSideMenu";
        private const string CTRL = "/_controltemplates/15/PNU.Internet/Colleges/DGA/";
        private const string CTRL_NEWS = "/_controltemplates/15/PNU.Internet/MediaCenter/News/";

        public static void EnsureList()
        {
            try
            {
                // fast check as current user; only elevate when work is actually needed
                bool needsWork;
                using (SPSite s = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb w = s.OpenWeb())
                {
                    SPList l = w.Lists.TryGetList(LIST_NAME);
                    needsWork = (l == null || l.ItemCount == 0);
                }
                if (!needsWork) return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    using (SPWeb web = site.OpenWeb())
                    {
                        EnsureListOnWeb(web);
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(
                    HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "SideMenuProvisioner",
                    "SideMenuProvisioner.EnsureList", ex.Message);
            }
        }

        public static void EnsureListOnWeb(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;

            SPList list = web.Lists.TryGetList(LIST_NAME);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_NAME, "College side menu items (DGA entity-details menu)",
                                        SPListTemplateType.GenericList);
                list = web.Lists[id];
            }

            if (!list.Fields.ContainsField("Title_EN"))
                list.Fields.Add("Title_EN", SPFieldType.Text, false);

            if (!list.Fields.ContainsField("ControlPath"))
            {
                string fld = list.Fields.Add("ControlPath", SPFieldType.Text, false);
                SPField f = list.Fields.GetFieldByInternalName(fld);
                f.Description = "User control path. May pass properties: " +
                                CTRL + "ucCollegeContentSection.ascx?SectionKey=faculty-achievements";
                f.Update();
            }

            if (!list.Fields.ContainsField("ItemType"))
            {
                string fld = list.Fields.Add("ItemType", SPFieldType.Choice, false);
                SPFieldChoice choice = (SPFieldChoice)list.Fields.GetFieldByInternalName(fld);
                choice.Choices.Add("Control");
                choice.Choices.Add("Sections");
                choice.Choices.Add("Programs");
                choice.Choices.Add("News");
                choice.Choices.Add("Advertisements");
                choice.DefaultValue = "Control";
                choice.Update();
            }

            if (!list.Fields.ContainsField("ParentID"))
            {
                string fld = list.Fields.Add("ParentID", SPFieldType.Number, false);
                SPField f = list.Fields.GetFieldByInternalName(fld);
                f.DefaultValue = "0";
                f.Description = "0 = root item. Otherwise: ID of the parent root item (2-level menu).";
                f.Update();
            }

            if (!list.Fields.ContainsField("ItemOrder"))
                list.Fields.Add("ItemOrder", SPFieldType.Number, false);

            if (!list.Fields.ContainsField("Visibility"))
            {
                string fld = list.Fields.Add("Visibility", SPFieldType.Boolean, false);
                SPField f = list.Fields.GetFieldByInternalName(fld);
                f.DefaultValue = "1";
                f.Update();
            }

            list.Update();

            SPView view = list.DefaultView;
            EnsureViewField(view, "Title_EN");
            EnsureViewField(view, "ControlPath");
            EnsureViewField(view, "ItemType");
            EnsureViewField(view, "ParentID");
            EnsureViewField(view, "ItemOrder");
            EnsureViewField(view, "Visibility");
            view.Update();

            if (list.ItemCount == 0)
            {
                // ---------------- Level 1 ----------------
                Seed(list, "الرئيسية", "Home",
                     CTRL + "ucCollegeHomeDga.ascx", "Control", 0, 1);

                int moreId = Seed(list, "المزيد عن الكلية", "More About the College",
                     "", "Control", 0, 2);

                Seed(list, "الأقسام", "Departments",
                     CTRL + "ucCollegeSectionDetails.ascx", "Sections", 0, 3);   // added ONCE; one pane per department, SectionKey=DEPT_CODE appended by ucSideMenu

                Seed(list, "البرامج", "Programs",
                     CTRL + "ucCollegeProgramsDga.ascx", "Programs", 0, 4);

                Seed(list, "الأخبار", "News",
                     CTRL + "ucCollegeNews.ascx", "News", 0, 5);

                Seed(list, "الإعلانات", "Announcements",
                     CTRL + "ucAllCollegeAds.ascx", "Advertisements", 0, 6);

                Seed(list, "المستندات والنماذج والأدلة", "Documents, Forms and Guides",
                     CTRL + "ucCollegeDocuments.ascx", "Control", 0, 7);

                Seed(list, "تواصل مع الكلية", "Contact the College",
                     CTRL + "ucCollegeContact.ascx", "Control", 0, 8);

                // ---------------- Level 2 (under "More About the College") ----------------
                string generic = CTRL + "ucCollegeContentSection.ascx?SectionKey=";

                Seed(list, "الهيكل التنظيمي للكلية", "Organizational Structure", generic + "faculty-org-structure", "Control", moreId, 1);
                Seed(list, "إنجازات الكلية", "College Achievements", generic + "faculty-achievements", "Control", moreId, 2);
                Seed(list, "وكالات الكلية", "College Vice Deanships", generic + "faculty-agencies", "Control", moreId, 3);
                Seed(list, "مرافق الكلية", "College Facilities", generic + "faculty-facilities", "Control", moreId, 4);
                Seed(list, "البحث العلمي في الكلية", "Scientific Research", generic + "faculty-research", "Control", moreId, 5);
                Seed(list, "الطالبات في الكلية", "Students", generic + "faculty-students", "Control", moreId, 6);
                Seed(list, "الخدمات الطلابية في الكلية", "Student Services", generic + "faculty-student-services", "Control", moreId, 7);
                Seed(list, "الأندية الطلابية في الكلية", "Student Clubs", generic + "faculty-clubs", "Control", moreId, 8);
                Seed(list, "المبادرات", "Initiatives", generic + "faculty-initiatives", "Control", moreId, 9);
                Seed(list, "التدريب في الكلية", "Training", generic + "faculty-training", "Control", moreId, 10);
            }

            web.AllowUnsafeUpdates = false;
        }

        private static int Seed(SPList list, string titleAr, string titleEn,
                                string controlPath, string itemType, int parentId, double order)
        {
            SPListItem item = list.AddItem();
            item["Title"] = titleAr;
            item["Title_EN"] = titleEn;
            item["ControlPath"] = controlPath;
            item["ItemType"] = itemType;
            item["ParentID"] = parentId;
            item["ItemOrder"] = order;
            item["Visibility"] = true;
            item.Update();
            return item.ID;
        }

        private static void EnsureViewField(SPView view, string fieldName)
        {
            if (!view.ViewFields.Exists(fieldName))
                view.ViewFields.Add(fieldName);
        }
    }



}
