using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty agencies on the CURRENT web:
    ///   FacultyAgencies      - agency accordion items (Title + plain body + AgencyKey)
    ///   FacultyAgencyUnits   - department/unit cards (linked by AgencyKey)
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// </summary>
    public static class AgenciesProvisioner
    {
        public const string ItemsListName = "FacultyAgencies";
        public const string UnitsListName = "FacultyAgencyUnits";
        public const string IntrosListName = "FacultyAgencyIntros";
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

                            SPList items = EnsureList(web, ItemsListName, "Faculty agencies accordion items");
                            EnsureField(items, "TitleEn", SPFieldType.Text);
                            EnsureField(items, "AgencyKey", SPFieldType.Text);
                            EnsureField(items, "BodyAr", SPFieldType.Note);
                            EnsureField(items, "BodyEn", SPFieldType.Note);
                            EnsureField(items, "SortOrder", SPFieldType.Number);
                            AddViewFields(items, "TitleEn", "AgencyKey", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(items);
                            if (items.ItemCount == 0) SeedItems(items);

                            SPList intros = EnsureList(web, IntrosListName, "Faculty agency intro blocks (نبذة / كلمة الوكيلة)");
                            EnsureField(intros, "TitleEn", SPFieldType.Text);
                            EnsureField(intros, "AgencyKey", SPFieldType.Text);
                            EnsureField(intros, "BodyAr", SPFieldType.Note);
                            EnsureField(intros, "BodyEn", SPFieldType.Note);
                            EnsureField(intros, "SortOrder", SPFieldType.Number);
                            AddViewFields(intros, "TitleEn", "AgencyKey", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(intros);
                            if (intros.ItemCount == 0) SeedIntros(intros);

                            SPList units = EnsureList(web, UnitsListName, "Faculty agency unit cards");
                            EnsureField(units, "TitleEn", SPFieldType.Text);
                            EnsureField(units, "AgencyKey", SPFieldType.Text);
                            EnsureField(units, "LinkUrl", SPFieldType.URL);
                            EnsureField(units, "SortOrder", SPFieldType.Number);
                            AddViewFields(units, "TitleEn", "AgencyKey", "LinkUrl", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(units);
                            if (units.ItemCount == 0) SeedUnits(units);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "AgenciesProvisioner - EnsureLists", ex.Message);
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

        private static void SeedItems(SPList list)
        {
            SPListItem a1 = list.AddItem();
            a1["Title"] = "وكالة الشؤون الأكاديمية";
            a1["TitleEn"] = "Vice-Deanship for Academic Affairs";
            a1["AgencyKey"] = "academic";
            a1["BodyAr"] = "أهم مهام الوكالة:\n\n1. الإشراف على إعداد الخطة السنوية لوكالة الكلية للشؤون الأكاديمية ومتابعة تنفيذها.\n2. الإشراف على تنفيذ اللوائح والقواعد المنظمة للائحة الدراسة والاختبارات للمرحلة الجامعية.\n3. الإشراف على البرامج الأكاديمية في الكلية.\n4. الإشراف على سير العملية التعليمية وتهيئة البيئة التعليمية الممكنة لتعليم متميز.\n5. الإشراف على متابعة تطبيق نظام إدارة الجودة والالتزام بمعايير الاعتماد الأكاديمي.";
            a1["SortOrder"] = 1;
            a1.Update();

            SPListItem a2 = list.AddItem();
            a2["Title"] = "وكالة البحث والابتكار والأعمال";
            a2["TitleEn"] = "Vice-Deanship for Research, Innovation and Business";
            a2["AgencyKey"] = "research";
            a2["BodyAr"] = "مهام الوكالة:\n\n1. الإشراف على الخطة السنوية للبحث العلمي للكلية المعتمدة من جهة الاختصاص.\n2. الإشراف على برامج الدراسات العليا في الكلية واختباراتها ولجانها.\n3. الإشراف على الجمعيات العلمية والمجلات العلمية بالكلية.\n4. الإشراف على تنمية الشراكات وعلاقات الكلية مع القطاعات الخارجية.\n5. إيجاد بيئة داعمة للابتكار وريادة الأعمال.";
            a2["SortOrder"] = 2;
            a2.Update();
        }

        private static void AddIntro(SPList list, string agencyKey, string ar, string en, string bodyAr, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["AgencyKey"] = agencyKey;
            item["BodyAr"] = bodyAr;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedIntros(SPList list)
        {
            // Academic-affairs agency: نبذة عن الوكالة
            AddIntro(list, "academic", "نبذة عن الوكالة", "About the Vice-Deanship",
                "تُعد وكالة الكلية للشؤون الأكاديمية من أهم الركائز التي تقوم عليها كلية علوم الحاسب والمعلومات، حيث تقوم إداراتها ووحداتها المختلفة بالإشراف على سير العملية التعليمية، والتحقق من جودة برامجها بما يسهم في تحقيق رؤية ورسالة الكلية في تأهيل كوادر نسائية متميزة علميًا وبحثيًا وتقنيًا في بيئة تعليمية تفاعلية.\n\nكما تعنى الوكالة بتهيئة البيئة التعليمية المناسبة من قاعات دراسية ومعامل تخصصية، والإشراف عليها إداريًا وتقنيًا لتقديم المحاضرات النظرية والتدريب العملي وفق أعلى المعايير الأكاديمية.",
                1);

            // Research agency: كلمة الوكيلة
            AddIntro(list, "research", "كلمة الوكيلة", "Vice-Dean's Word",
                "انطلاقًا من رؤية الكلية ورسالتها، فإن مهام وكالة الكلية تهدف إلى تعزيز دور الكلية في الريادة العلمية، وتحقيق التنمية المستدامة في مختلف مجالاتها. نعمل بروح الفريق لتحقيق أهداف الكلية، ونسعى دائمًا إلى التميز في كل ما نقدمه.",
                1);
        }

        private static void AddUnit(SPList list, string agencyKey, string ar, string en, string url, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["AgencyKey"] = agencyKey;
            if (!string.IsNullOrEmpty(url))
                item["LinkUrl"] = new SPFieldUrlValue { Url = url, Description = ar };
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedUnits(SPList list)
        {
            AddUnit(list, "academic", "إدارة الشؤون التعليمية", "Educational Affairs", "https://pnu.edu.sa/ar/Faculties/IT/Pages/EA-Department.aspx", 1);
            AddUnit(list, "academic", "إدارة جودة التعليم والتعلم", "Education Quality", "https://pnu.edu.sa/ar/Faculties/IT/Pages/Department-TLQ.aspx", 2);
            AddUnit(list, "academic", "إدارة المعامل ومصادر التعلم", "Labs and Learning Resources", "https://pnu.edu.sa/ar/Faculties/IT/Pages/Department-LLR.aspx", 3);
            AddUnit(list, "academic", "وحدة التعلم الإلكتروني", "E-Learning Unit", "https://pnu.edu.sa/ar/Faculties/IT/Pages/E-Learning-Unit.aspx", 4);

            AddUnit(list, "research", "وحدة البحث العلمي", "Scientific Research Unit", "https://pnu.edu.sa/ar/Faculties/IT/Pages/researchcenter.aspx", 1);
            AddUnit(list, "research", "مركز الابتكار", "Innovation Center", "https://pnu.edu.sa/ar/Faculties/IT/Pages/innovationcenter.aspx", 2);
            AddUnit(list, "research", "مركز الذكاء الاصطناعي", "AI Center", "https://pnu.edu.sa/ar/Faculties/IT/AICentre/Pages/default.aspx", 3);
            AddUnit(list, "research", "إدارة برامج الدراسات العليا", "Graduate Programs", "https://pnu.edu.sa/ar/Faculties/IT/Pages/APS.aspx", 4);
            AddUnit(list, "research", "إدارة الشراكات والمسؤولية المجتمعية", "Partnerships & Community", "https://pnu.edu.sa/ar/Faculties/IT/Pages/Partnerships-and-cru.aspx", 5);
            AddUnit(list, "research", "مكتب الأعمال وإدارة المشاريع", "Business & Project Mgmt", "https://pnu.edu.sa/ar/Faculties/IT/Pages/Business-Project-Management.aspx", 6);
            AddUnit(list, "research", "وحدة الشؤون العلمية", "Scientific Affairs Unit", "", 7);
        }
    }
}
