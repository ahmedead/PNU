using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared
{
    public static class ContactUsProvisioner
    {
        public const string InfoListName = "ContactUsInfo";
        public const string DirectoryListName = "ContactUsDirectory";
        public const string HoursListName = "ContactUsHours";
        public const string TitlesListName = "ContactUsTitles";
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

                            SPList info = EnsureList(web, InfoListName, "Contact Us info cards");
                            EnsureField(info, "TitleEn", SPFieldType.Text);
                            EnsureField(info, "Value", SPFieldType.Text);
                            EnsureField(info, "IconClass", SPFieldType.Text);
                            EnsureField(info, "LinkUrl", SPFieldType.URL);
                            EnsureField(info, "SortOrder", SPFieldType.Number);

                            SPView infoview = info.DefaultView;
                            AddViewField(infoview, "TitleEn");
                            AddViewField(infoview, "Value");
                            AddViewField(infoview, "IconClass");
                            AddViewField(infoview, "LinkUrl");
                            AddViewField(infoview, "SortOrder");
                            infoview.Update();

                            if (info.ItemCount == 0) SeedInfo(info);
                            GrantAnonymousRead(info);

                            SPList dir = EnsureList(web, DirectoryListName, "Offices Directory");
                            EnsureField(dir, "OfficeEn", SPFieldType.Text);
                            EnsureField(dir, "Extension", SPFieldType.Note);   // multiple lines
                            EnsureField(dir, "RoomNumber", SPFieldType.Text);
                            EnsureField(dir, "Email", SPFieldType.Text);
                            EnsureField(dir, "SortOrder", SPFieldType.Number);

                            SPView dirview = dir.DefaultView;
                            AddViewField(dirview, "OfficeEn");
                            AddViewField(dirview, "Extension");
                            AddViewField(dirview, "RoomNumber");
                            AddViewField(dirview, "Email");
                            AddViewField(dirview, "SortOrder");
                            dirview.Update();

                            if (dir.ItemCount == 0) SeedDirectory(dir);
                            GrantAnonymousRead(dir);

                            SPList hours = EnsureList(web, HoursListName, "beneficiary hours");
                            EnsureField(hours, "UnitEn", SPFieldType.Text);
                            EnsureField(hours, "Day", SPFieldType.Text);
                            EnsureField(hours, "DayEn", SPFieldType.Text);
                            EnsureField(hours, "Time", SPFieldType.Text);
                            EnsureField(hours, "SortOrder", SPFieldType.Number);

                            SPView hoursview = hours.DefaultView;
                            AddViewField(hoursview, "UnitEn");
                            AddViewField(hoursview, "Day");
                            AddViewField(hoursview, "DayEn");
                            AddViewField(hoursview, "Time");
                            AddViewField(hoursview, "SortOrder");
                            hoursview.Update();

                            if (hours.ItemCount == 0) SeedHours(hours);
                            GrantAnonymousRead(hours);

                            SPList titles = EnsureList(web, TitlesListName, "Contact Us bilingual labels");
                            EnsureField(titles, "TitleKey", SPFieldType.Text);
                            EnsureField(titles, "ValueAr", SPFieldType.Note);
                            EnsureField(titles, "ValueEn", SPFieldType.Note);

                            SPView titlesview = titles.DefaultView;
                            AddViewField(titlesview, "TitleKey");
                            AddViewField(titlesview, "ValueAr");
                            AddViewField(titlesview, "ValueEn");
                            titlesview.Update();

                            if (titles.ItemCount == 0) SeedTitles(titles);
                            GrantAnonymousRead(titles);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ContactUsProvisioner - EnsureLists", ex.Message);
            }
        }

        /// <summary>
        /// Reads the titles list into a case-insensitive key -> (Ar, En) map.
        /// Call inside RunWithElevatedPrivileges for anonymous users.
        /// </summary>
        public static Dictionary<string, KeyValuePair<string, string>> LoadTitles(SPWeb web)
        {
            var map = new Dictionary<string, KeyValuePair<string, string>>(StringComparer.OrdinalIgnoreCase);
            SPList list = web.Lists.TryGetList(TitlesListName);
            if (list == null) return map;

            foreach (SPListItem item in list.GetItems())
            {
                string key = item["TitleKey"] as string;
                if (string.IsNullOrEmpty(key)) continue;
                map[key] = new KeyValuePair<string, string>(
                    item["ValueAr"] as string ?? "",
                    item["ValueEn"] as string ?? "");
            }
            return map;
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
                    if (f != null) { f.RichText = false; f.NumberOfLines = 4; f.Update(); }
                }
            }
        }

        private static void AddViewField(SPView view, string name)
        {
            if (!view.ViewFields.Exists(name))
                view.ViewFields.Add(name);
        }

        private static void GrantAnonymousRead(SPList list)
        {
            if (!list.HasUniqueRoleAssignments)
                list.BreakRoleInheritance(true, false);

            list.AnonymousPermMask64 =
                SPBasePermissions.ViewListItems | SPBasePermissions.ViewVersions |
                SPBasePermissions.ViewPages | SPBasePermissions.Open;
            list.Update();
        }

        // ---------- seeds ----------

        private static void SeedInfo(SPList list)
        {
            AddInfo(list, "رقم المبنى", "Building Number", "170", "hgi-location-01", "", 1);
            AddInfo(list, "محطة القطار", "Train Station", "A3", "hgi-location-01", "", 2);
            AddInfo(list, "رقم الهاتف", "Phone Number", "0118201182", "hgi-call", "", 3);
            AddInfo(list, "موقع الكلية", " Location", "كلية علوم الحاسب والمعلومات", "hgi-location-01",
                "https://www.google.com/maps/place/24%C2%B051%2711.4%22N+46%C2%B043%2706.6%22E/@24.8531685,46.7159119,17z", 4);
        }

        private static void AddInfo(SPList list, string ar, string en, string value, string icon, string url, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["Value"] = value;
            item["IconClass"] = icon;
            if (!string.IsNullOrEmpty(url))
                item["LinkUrl"] = new SPFieldUrlValue { Url = url, Description = ar };
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedDirectory(SPList list)
        {
            AddDir(list, "مكتب العميدة", "Dean's Office", "35432\n38477\n38634", "2.100.01", "ccis@pnu.edu.sa", 1);
            AddDir(list, "وكالة الكلية للشؤون الأكاديمية", "Vice-Deanship for Academic Affairs", "38420", "2.100.08", "ccis-ea@pnu.edu.sa", 2);
            AddDir(list, "إدارة المعامل ومصادر التعلم", "Labs and Learning Resources", "38466", "0.702", "ccis-lu@pnu.edu.sa", 3);
            AddDir(list, "وحدة التعلم الإلكتروني", "E-Learning Unit", "-", "2.100.14", "ccis-qd-edl@pnu.edu.sa", 4);
            AddDir(list, "وكالة البحث والابتكار والأعمال", "Vice-Deanship for Research", "38424", "1.100.08", "ccis-gs@pnu.edu.sa", 5);
            AddDir(list, "إدارة الخدمات الطلابية", "Student Services", "38368", "0.100.06", "ccis-sa@pnu.edu.sa", 6);
            AddDir(list, "إدارة جودة التعليم والتعلم", "Education Quality", "38579", "1.100.D", "ccis-qt@pnu.edu.sa", 7);
            AddDir(list, "رئيسة قسم علوم الحاسبات", "Head of Computer Science Dept.", "38418", "2.505.35", "ccis-cs@pnu.edu.sa", 8);
            AddDir(list, "رئيسة قسم نظم المعلومات", "Head of Information Systems Dept.", "38419", "1.501.37", "ccis-is@pnu.edu.sa", 9);
            AddDir(list, "رئيسة قسم تقنية المعلومات", "Head of Information Technology Dept.", "38425", "0.701.05", "ccis-nw@pnu.edu.sa", 10);
            AddDir(list, "مكتب العلاقات العامة", "Public Relations Office", "38510", "2.100.15", "ccis-pr@pnu.edu.sa", 11);
            AddDir(list, "وحدة الإعلام", "Media Unit", "38295", "2.100.15", "ccis-mu@pnu.edu.sa", 12);
            AddDir(list, "منسق التدريب/ قسم علوم الحاسبات", "Training Coordinator / CS", "38405", "2.505.41", "raalsmari@pnu.edu.sa", 13);
            AddDir(list, "منسق التدريب/ قسم نظم المعلومات", "Training Coordinator / IS", "43923", "1.501.11", "ocsaidani@pnu.edu.sa", 14);
            AddDir(list, "منسق التدريب/قسم تقنية المعلومات", "Training Coordinator / IT", "43923", "1.201.02", "ccis-nw-ct@pnu.edu.sa", 15);
            AddDir(list, "منسقة الإرشاد الأكاديمي في قسم علوم الحاسبات", "Academic Advising / CS", "38402", "2.505.23", "raalsalih@pnu.edu.sa", 16);
            AddDir(list, "منسقة الإرشاد الأكاديمي في قسم نظم المعلومات", "Academic Advising / IS", "38461", "1.505.38", "haalmansur@pnu.edu.sa", 17);
            AddDir(list, "منسقة الإرشاد الأكاديمي في قسم تقنية المعلومات", "Academic Advising / IT", "-", "0.701.33", "ccis-nw-aau@pnu.edu.sa", 18);
        }

        private static void AddDir(SPList list, string ar, string en, string ext, string room, string email, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["OfficeEn"] = en;
            item["Extension"] = ext;
            item["RoomNumber"] = room;
            item["Email"] = email;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedHours(SPList list)
        {
            AddHours(list, "مكتب العميدة", "Dean's Office", "الاثنين", "Monday", "12:00 م", 1);
            AddHours(list, "وكالة الكلية للشؤون الأكاديمية", "Academic Affairs", "الاثنين", "Monday", "10:00 ص", 2);
            AddHours(list, "وكالة الكلية للبحث والابتكار والأعمال", "Research & Innovation", "الأحد", "Sunday", "1:00 م", 3);
            AddHours(list, "قسم علوم الحاسبات", "Computer Science Dept.", "الاثنين", "Monday", "1:00 م", 4);
            AddHours(list, "قسم نظم المعلومات", "Information Systems Dept.", "الأربعاء", "Wednesday", "12:00 م", 5);
            AddHours(list, "قسم تقنية المعلومات", "Information Technology Dept.", "الأحد", "Sunday", "11:00 ص", 6);
        }

        private static void AddHours(SPList list, string ar, string en, string dayAr, string dayEn, string time, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["UnitEn"] = en;
            item["Day"] = dayAr;
            item["DayEn"] = dayEn;
            item["Time"] = time;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedTitles(SPList list)
        {
            AddTitle(list, "InfoTitle", "بيانات الموقع والتواصل", "Location and Contact Details");
            AddTitle(list, "DirectoryTitle", "دليل التواصل مع مكاتب الكلية", "College Offices Directory");
            AddTitle(list, "ThOffice", "المكتب", "Office");
            AddTitle(list, "ThExtension", "التحويلة", "Extension");
            AddTitle(list, "ThRoom", "رقم المكتب", "Room");
            AddTitle(list, "ThEmail", "البريد الرسمي", "Email");
            AddTitle(list, "HoursTitle", "ساعات المستفيدين", "Beneficiary Hours");
            AddTitle(list, "ThUnit", "الوكالة / القسم", "Unit / Department");
            AddTitle(list, "ThDay", "اليوم", "Day");
            AddTitle(list, "ThTime", "الساعة", "Time");
            AddTitle(list, "UnifiedTitle", "أو عن طريق نموذج التواصل الموحد", "Or via the unified contact form");
            AddTitle(list, "FormTitle", "نموذج التواصل الموحد", "Unified Contact Form");
            AddTitle(list, "FormText",
                "أرسل استفسارك أو مقترحك إلى الكلية من خلال نموذج التواصل.",
                "Send your inquiry or suggestion to the college through the contact form.");
            AddTitle(list, "FormAriaLabel", "فتح نموذج التواصل الموحد", "Open the unified contact form");
        }

        private static void AddTitle(SPList list, string key, string valueAr, string valueEn)
        {
            SPListItem item = list.AddItem();
            item["Title"] = key;
            item["TitleKey"] = key;
            item["ValueAr"] = valueAr;
            item["ValueEn"] = valueEn;
            item.Update();
        }
    }
}