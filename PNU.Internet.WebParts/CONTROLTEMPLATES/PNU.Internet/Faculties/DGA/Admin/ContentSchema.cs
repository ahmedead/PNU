using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    public enum FieldKind { Text, Note, Number, Url }

    /// <summary>One editable column in a content list.</summary>
    public class FieldDef
    {
        public string Name { get; set; }
        public string Label { get; set; }        // Arabic label shown to the editor
        public FieldKind Kind { get; set; }
        public bool ShowInGrid { get; set; }
        public string Hint { get; set; }

        public FieldDef(string name, string label, FieldKind kind, bool showInGrid, string hint)
        {
            Name = name; Label = label; Kind = kind; ShowInGrid = showInGrid; Hint = hint;
        }
    }

    /// <summary>One content list and its editable columns.</summary>
    public class ListDef
    {
        public string ListName { get; set; }
        public string Display { get; set; }      // Arabic label
        public List<FieldDef> Fields { get; set; }
    }

    /// <summary>A page section, grouping the lists that feed it.</summary>
    public class SectionDef
    {
        public string Key { get; set; }
        public string Display { get; set; }
        public List<ListDef> Lists { get; set; }
    }

    /// <summary>
    /// The single source of truth for the content-administration UI.
    ///
    /// It also acts as a WHITELIST: the editor will only ever open a list that
    /// appears here, so a tampered form value cannot point the editor at an
    /// arbitrary list (for example a permissions or configuration list).
    /// </summary>
    public static class ContentSchema
    {
        private static List<SectionDef> _sections;

        public static List<SectionDef> Sections
        {
            get { if (_sections == null) _sections = Build(); return _sections; }
        }

        public static SectionDef FindSection(string key)
        {
            foreach (SectionDef s in Sections)
                if (string.Equals(s.Key, key, StringComparison.OrdinalIgnoreCase)) return s;
            return null;
        }

        /// <summary>Returns the list definition, or null when the name is not whitelisted.</summary>
        public static ListDef FindList(string listName)
        {
            if (string.IsNullOrEmpty(listName)) return null;
            foreach (SectionDef s in Sections)
                foreach (ListDef l in s.Lists)
                    if (string.Equals(l.ListName, listName, StringComparison.OrdinalIgnoreCase)) return l;
            return null;
        }

        // ---------- shorthand builders ----------
        private static FieldDef T(string n, string l, bool grid = false, string hint = "") { return new FieldDef(n, l, FieldKind.Text, grid, hint); }
        private static FieldDef N(string n, string l, string hint = "") { return new FieldDef(n, l, FieldKind.Note, false, hint); }
        private static FieldDef U(string n, string l, bool grid = false) { return new FieldDef(n, l, FieldKind.Url, grid, ""); }
        private static FieldDef S() { return new FieldDef("SortOrder", "الترتيب", FieldKind.Number, true, "رقم يحدد ترتيب الظهور"); }

        private static ListDef L(string name, string display, params FieldDef[] fields)
        {
            return new ListDef { ListName = name, Display = display, Fields = new List<FieldDef>(fields) };
        }

        private static SectionDef Sec(string key, string display, params ListDef[] lists)
        {
            return new SectionDef { Key = key, Display = display, Lists = new List<ListDef>(lists) };
        }

        // Common field sets
        private static FieldDef Title(string label = "العنوان (عربي)") { return T("Title", label, true); }
        private static FieldDef TitleEn() { return T("TitleEn", "العنوان (إنجليزي)"); }
        private static FieldDef ItemKey() { return T("ItemKey", "مفتاح العنصر", true, "حروف إنجليزية صغيرة بلا مسافات، مثل: ach1"); }
        private static FieldDef BodyAr() { return N("BodyAr", "النص (عربي)", "سطر فارغ = فقرة جديدة، \"- \" = نقطة، \"1.\" = ترقيم"); }
        private static FieldDef BodyEn() { return N("BodyEn", "النص (إنجليزي)"); }

        private static ListDef ImagesList(string name, string keyField, string keyLabel)
        {
            return L(name, "الصور",
                T("Title", "وصف مختصر", true),
                T(keyField, keyLabel, true, "يجب أن يطابق مفتاح العنصر الذي تتبعه الصورة"),
                U("ImageUrl", "رابط الصورة", true),
                S());
        }

        // A "rich" section uses the same five lists; only the names differ.
        private static ListDef RichSections(string name)
        {
            return L(name, "الأقسام الفرعية",
                Title("عنوان القسم الفرعي"), TitleEn(),
                ItemKey(),
                T("SectionKey", "مفتاح القسم الفرعي", true, "مفتاح فريد، مثل: res2-devices"),
                BodyAr(), BodyEn(),
                T("TableHeaders", "عناوين أعمدة الجدول", false, "افصلي بينها بعلامة | مثل: اسم الجهاز|استخداماته|نوعه"),
                U("LinkUrl", "رابط زر (اختياري)"),
                T("LinkText", "نص الزر"),
                S());
        }

        private static ListDef RichSubItems(string name)
        {
            return L(name, "العناصر الفرعية",
                Title("عنوان العنصر الفرعي"), TitleEn(),
                T("SectionKey", "مفتاح القسم الفرعي", true, "يطابق مفتاح القسم الذي يتبعه"),
                BodyAr(), BodyEn(),
                U("LinkUrl", "رابط زر (اختياري)"),
                T("LinkText", "نص الزر"),
                S());
        }

        private static ListDef RichTable(string name)
        {
            return L(name, "صفوف الجداول",
                T("SectionKey", "مفتاح القسم الفرعي", true),
                Title("العمود الأول"),
                N("Col2", "العمود الثاني"),
                T("Col3", "العمود الثالث", true),
                S());
        }

        private static ListDef RichImages(string name)
        {
            return L(name, "الصور",
                T("Title", "وصف مختصر", true),
                T("OwnerKey", "مفتاح المالك", true, "ضعي مفتاح العنصر أو مفتاح القسم الفرعي"),
                U("ImageUrl", "رابط الصورة", true),
                T("AltText", "النص البديل للصورة", false, "مطلوب لمعايير الوصول الرقمي"),
                S());
        }

        private static ListDef RichItems(string name)
        {
            return L(name, "العناصر الرئيسية",
                Title(), TitleEn(), ItemKey(), BodyAr(), BodyEn(), S());
        }

        private static SectionDef RichSection(string key, string display, string prefix)
        {
            return Sec(key, display,
                RichItems(prefix),
                RichSections(prefix + "Sections"),
                RichSubItems(prefix + "SubItems"),
                RichTable(prefix + "Table"),
                RichImages(prefix + "Images"));
        }

        private static SectionDef SimpleSection(string key, string display, string prefix)
        {
            return Sec(key, display,
                L(prefix, "العناصر", Title(), TitleEn(), ItemKey(), BodyAr(), BodyEn(), S()),
                ImagesList(prefix + "Images", "ItemKey", "مفتاح العنصر"));
        }

        // ---------- the registry ----------
        private static List<SectionDef> Build()
        {
            var list = new List<SectionDef>();

            list.Add(SimpleSection("org", "الهيكل التنظيمي", "FacultyOrgStructure"));
            list.Add(SimpleSection("achievements", "إنجازات الكلية", "FacultyAchievements"));
            list.Add(SimpleSection("facilities", "مرافق الكلية", "FacultyFacilities"));

            list.Add(Sec("agencies", "وكالات الكلية",
                L("FacultyAgencies", "الوكالات",
                    Title(), TitleEn(),
                    T("AgencyKey", "مفتاح الوكالة", true, "مثل: academic"),
                    BodyAr(), BodyEn(), S()),
                L("FacultyAgencyIntros", "نبذة الوكالة / كلمة الوكيلة",
                    Title(), TitleEn(),
                    T("AgencyKey", "مفتاح الوكالة", true),
                    BodyAr(), BodyEn(), S()),
                L("FacultyAgencyUnits", "الإدارات والوحدات التابعة",
                    Title(), TitleEn(),
                    T("AgencyKey", "مفتاح الوكالة", true),
                    U("LinkUrl", "رابط الصفحة", true), S())));

            list.Add(RichSection("research", "البحث العلمي", "FacultyResearch"));
            list.Add(RichSection("studentservices", "الخدمات الطلابية", "FacultyStudentServices"));
            list.Add(RichSection("clubs", "الأندية الطلابية", "FacultyClubs"));

            list.Add(Sec("initiatives", "المبادرات",
                L("FacultyInitiatives", "المبادرات",
                    Title(), TitleEn(), ItemKey(), BodyAr(), BodyEn(), S()),
                L("FacultyInitiativeSections", "الأقسام النصية",
                    Title("عنوان القسم"), TitleEn(), ItemKey(), BodyAr(), BodyEn(), S()),
                L("FacultyInitiativeCriteria", "جدول معايير المفاضلة",
                    Title("اسم المعيار"), ItemKey(),
                    N("Details", "التفاصيل"), N("DetailsEn", "التفاصيل (إنجليزي)"),
                    T("MaxScore", "الدرجة القصوى", true), S()),
                L("FacultyDeansListNames", "أسماء قائمة العميد",
                    Title("اسم الطالبة"), ItemKey(),
                    T("Track", "المسار", true), T("TrackEn", "المسار (إنجليزي)"), S()),
                ImagesList("FacultyInitiativesImages", "ItemKey", "مفتاح المبادرة")));

            list.Add(Sec("training", "التدريب",
                L("FacultyTraining", "نبذة التدريب",
                    Title(), TitleEn(), BodyAr(), BodyEn(), S()),
                L("FacultyTrainingCoords", "منسقو التدريب",
                    Title("البرنامج"), T("ProgramEn", "البرنامج (إنجليزي)"),
                    T("CoordName", "اسم المنسق/ة", true),
                    T("Email", "البريد الإلكتروني", true), S())));

            return list;
        }
    }
}
