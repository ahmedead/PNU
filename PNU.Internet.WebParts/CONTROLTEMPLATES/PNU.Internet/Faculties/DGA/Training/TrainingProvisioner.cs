using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty training on the CURRENT web:
    ///   FacultyTraining        - intro + committee tasks (Title + plain body)
    ///   FacultyTrainingCoords  - coordinator rows (Program / Name / Email)
    /// Idempotent; seeds only when empty; anonymous read granted.
    /// </summary>
    public static class TrainingProvisioner
    {
        public const string IntroListName = "FacultyTraining";
        public const string CoordsListName = "FacultyTrainingCoords";
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

                            SPList intro = EnsureList(web, IntroListName, "Faculty training intro/body");
                            EnsureField(intro, "TitleEn", SPFieldType.Text);
                            EnsureField(intro, "BodyAr", SPFieldType.Note);
                            EnsureField(intro, "BodyEn", SPFieldType.Note);
                            EnsureField(intro, "SortOrder", SPFieldType.Number);
                            AddViewFields(intro, "TitleEn", "BodyAr", "BodyEn", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(intro);
                            if (intro.ItemCount == 0) SeedIntro(intro);

                            SPList coords = EnsureList(web, CoordsListName, "Faculty training coordinators");
                            EnsureField(coords, "ProgramEn", SPFieldType.Text);
                            EnsureField(coords, "CoordName", SPFieldType.Text);
                            EnsureField(coords, "Email", SPFieldType.Text);
                            EnsureField(coords, "SortOrder", SPFieldType.Number);
                            AddViewFields(coords, "ProgramEn", "CoordName", "Email", "SortOrder");
                            FacultyProvisioningHelper.GrantAnonymousRead(coords);
                            if (coords.ItemCount == 0) SeedCoords(coords);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "TrainingProvisioner - EnsureLists", ex.Message);
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

        private static void SeedIntro(SPList list)
        {
            SPListItem item = list.AddItem();
            item["Title"] = "التدريب التعاوني";
            item["TitleEn"] = "Cooperative Training";
            item["BodyAr"] = "تقوم طالبات كلية علوم الحاسب والمعلومات في جميع أقسامها بتدريب تعاوني يطبقن خلاله ما تعلمنه في إحدى المنظمات أو الشركات أو الهيئات الحكومية أو الخاصة. ويساعد التدريب الطالبات على خوض بيئة العمل وممارسة المعرفة عمليًا واكتساب الخبرة.\n\nمهام لجنة التدريب:\n\n- الإشراف على التدريب التعاوني بالكلية ومخاطبة الجهات رسميًا.\n- متابعة قبول الطالبات ومباشرتهن في الجهات التدريبية.\n- المتابعة مع المشرفين الأكاديميين والتأكد من وضع الطالبة في الجهات التدريبية.\n- تحديث الأدلة والنماذج الخاصة بالتدريب التعاوني وفق المستجدات.\n- اعتماد الخطة التدريبية للطالبة والتأكد من ملاءمتها مع الجهة التدريبية.\n\nملاحظة: للحصول على النماذج الخاصة بالطالبة أو الأدلة أو للاستفسار، يرجى الرجوع إلى منسق التدريب في القسم.";
            item["SortOrder"] = 1;
            item.Update();
        }

        private static void AddCoord(SPList list, string programAr, string programEn, string name, string email, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = programAr;
            item["ProgramEn"] = programEn;
            item["CoordName"] = name;
            item["Email"] = email;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedCoords(SPList list)
        {
            AddCoord(list, "تقنية المعلومات", "Information Technology", "د. محمد حسان", "HAAMohamed@pnu.edu.sa", 1);
            AddCoord(list, "الأمن السيبراني", "Cybersecurity", "د. فتحي عبدالحميد", "Feabdelhamid@pnu.edu.sa", 2);
            AddCoord(list, "علوم الحاسبات", "Computer Science", "أ. رزان العقيل", "Rialaqeel@pnu.edu.sa", 3);
            AddCoord(list, "علوم الحاسبات", "Computer Science", "أ. وجدان الطويرقي", "Wfaltowairqi@pnu.edu.sa", 4);
            AddCoord(list, "الذكاء الاصطناعي", "Artificial Intelligence", "أ. رهف العلياني", "RHAlalyani@pnu.edu.sa", 5);
            AddCoord(list, "نظم المعلومات", "Information Systems", "د. أميمة السعيداني", "ocsaidani@pnu.edu.sa", 6);
            AddCoord(list, "برنامج علم البيانات وتحليلها", "Data Science", "د. فاطمة الكمأه", "Fsalkomah@pnu.edu.sa", 7);
        }
    }
}
