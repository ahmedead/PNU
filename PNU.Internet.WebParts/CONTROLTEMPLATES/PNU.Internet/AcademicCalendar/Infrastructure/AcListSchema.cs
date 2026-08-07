using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls
{
    public class AcFieldDef
    {
        public string InternalName { get; set; }
        public string DisplayAr { get; set; }
        public string DisplayEn { get; set; }
        public SPFieldType Type { get; set; }
        /// <summary>Show this column in the admin grid.</summary>
        public bool InGrid { get; set; }
        /// <summary>Rows for Note fields in the admin form.</summary>
        public int Rows { get; set; }
        public string Hint { get; set; }
        /// <summary>Options for Choice fields.</summary>
        public string[] Choices { get; set; }

        public AcFieldDef(string name, string ar, string en, SPFieldType type,
                          bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return AcHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class AcListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }

        /// <summary>Label applied to the built-in Title column (the item's primary field).</summary>
        public string ItemTitleAr { get; set; }
        public string ItemTitleEn { get; set; }

        public List<AcFieldDef> Fields { get; set; }
        /// <summary>Rows inserted when the list is created.</summary>
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return AcHelper.Pick(TitleAr, TitleEn); } }

        /// <summary>Localized label for the Title column, defaulting to a generic title.</summary>
        public string ItemTitleDisplay
        {
            get
            {
                string ar = string.IsNullOrEmpty(ItemTitleAr) ? "العنوان (عربي)" : ItemTitleAr;
                string en = string.IsNullOrEmpty(ItemTitleEn) ? "Title (AR)" : ItemTitleEn;
                return AcHelper.Pick(ar, en);
            }
        }
    }

    /// <summary>
    /// Single source of truth for the Academic Calendar lists: used by the provisioner to
    /// create the lists and by ucAcAdmin to render the CRUD form.
    /// Every row carries an Arabic AND an English value; the display control picks the
    /// right language for the page.
    /// </summary>
    public static class AcListSchema
    {
        /// <summary>
        /// Semesters of the calendar. The stored choice value is Arabic (that is what
        /// lives in the list), so the English label is kept beside it and resolved at
        /// render time - otherwise the English page shows Arabic accordion headings.
        /// </summary>
        public static readonly string[] Semesters = new string[]
        {
            "الفصل الدراسي الأول", "الفصل الدراسي الثاني", "الفصل الصيفي"
        };

        private static readonly string[] SemestersEn = new string[]
        {
            "First semester", "Second semester", "Summer term"
        };

        /// <summary>
        /// Localized heading for a stored semester value. A value an editor added
        /// outside the schema is returned unchanged.
        /// </summary>
        public static string SemesterDisplay(string storedValue)
        {
            if (string.IsNullOrEmpty(storedValue)) return string.Empty;

            for (int i = 0; i < Semesters.Length; i++)
            {
                if (string.Equals(Semesters[i], storedValue, StringComparison.OrdinalIgnoreCase))
                    return AcHelper.Pick(Semesters[i], SemestersEn[i]);
            }

            return storedValue;
        }

        private static Dictionary<string, AcListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, AcListDef> All
        {
            get
            {
                if (_all == null)
                {
                    lock (_lock)
                    {
                        if (_all == null) _all = Build();
                    }
                }
                return _all;
            }
        }

        public static AcListDef Get(string listName)
        {
            AcListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        private static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        // Semester choice values used in the seed rows.
        private const string S1 = "الفصل الدراسي الأول";
        private const string S2 = "الفصل الدراسي الثاني";
        private const string S3 = "الفصل الصيفي";

        /// <summary>Bilingual calendar seed row helper (positional, keeps the table readable).</summary>
        private static Dictionary<string, string> Cal(
            int order, string procAr, string procEn, string sem,
            string weekAr, string weekEn, string dayAr, string dayEn,
            string hijAr, string hijEn, string gregAr, string gregEn)
        {
            return Row(
                "Title", procAr, "Title_EN", procEn, "Semester", sem,
                "Week", weekAr, "Week_EN", weekEn,
                "DayPeriod", dayAr, "DayPeriod_EN", dayEn,
                "HijriDate", hijAr, "HijriDate_EN", hijEn,
                "GregorianDate", gregAr, "GregorianDate_EN", gregEn,
                "ItemOrder", order.ToString());
        }

        private static Dictionary<string, AcListDef> Build()
        {
            var map = new Dictionary<string, AcListDef>(StringComparer.OrdinalIgnoreCase);

            // ---- تقويم الإجراءات الأكاديمية (bilingual) --------------------------------
            map[AcListNames.Calendar] = new AcListDef
            {
                Name = AcListNames.Calendar,
                TitleAr = "تقويم الإجراءات الأكاديمية", TitleEn = "Academic procedures calendar",
                Description = "Academic procedure rows (Arabic + English) grouped by semester.",
                ItemTitleAr = "الإجراء (عربي)", ItemTitleEn = "Procedure (AR)",
                Fields = new List<AcFieldDef>
                {
                    new AcFieldDef("Title_EN", "الإجراء (إنجليزي)", "Procedure (EN)", SPFieldType.Text),
                    new AcFieldDef("Semester", "الفصل الدراسي", "Semester", SPFieldType.Choice, true, 0,
                                   "الفصل الدراسي الأول / الفصل الدراسي الثاني / الفصل الصيفي", Semesters),
                    new AcFieldDef("Week", "الأسبوع (عربي)", "Week (AR)", SPFieldType.Text, true),
                    new AcFieldDef("Week_EN", "الأسبوع (إنجليزي)", "Week (EN)", SPFieldType.Text),
                    new AcFieldDef("DayPeriod", "اليوم/الفترة (عربي)", "Day / period (AR)", SPFieldType.Text, true),
                    new AcFieldDef("DayPeriod_EN", "اليوم/الفترة (إنجليزي)", "Day / period (EN)", SPFieldType.Text),
                    new AcFieldDef("HijriDate", "التاريخ الهجري (عربي)", "Hijri date (AR)", SPFieldType.Text, true),
                    new AcFieldDef("HijriDate_EN", "التاريخ الهجري (إنجليزي)", "Hijri date (EN)", SPFieldType.Text),
                    new AcFieldDef("GregorianDate", "التاريخ الميلادي (عربي)", "Gregorian date (AR)", SPFieldType.Text, true),
                    new AcFieldDef("GregorianDate_EN", "التاريخ الميلادي (إنجليزي)", "Gregorian date (EN)", SPFieldType.Text),
                    new AcFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = new List<Dictionary<string, string>>
                {
                    // ===== الفصل الدراسي الأول / First semester =====
                    Cal(1,  "عودة أعضاء الهيئة التعليمية", "Return of teaching staff", S1, "-", "-", "الأحد", "Sunday", "3/ربيع أول/1448هـ", "3 Rabi I 1448H", "16/أغسطس/2026م", "16 August 2026"),
                    Cal(2,  "بداية الدراسة", "Start of study", S1, "الأول", "First", "الأحد", "Sunday", "10/ربيع أول/1448هـ", "10 Rabi I 1448H", "23/أغسطس/2026م", "23 August 2026"),
                    Cal(3,  "طلب تأجيل الفصل الدراسي", "Request to postpone the semester", S1, "- إلى الثاني", "- to second", "من الأحد إلى الخميس", "Sunday to Thursday", "3 - 21/ربيع أول/1448هـ", "3 - 21 Rabi I 1448H", "16/أغسطس - 3/سبتمبر/2026م", "16 August - 3 September 2026"),
                    Cal(4,  "الحذف والإضافة", "Add and drop", S1, "-", "-", "من الإثنين إلى الأربعاء", "Monday to Wednesday", "4 - 6/ربيع أول/1448هـ", "4 - 6 Rabi I 1448H", "17 - 19/أغسطس/2026م", "17 - 19 August 2026"),
                    Cal(5,  "طلب تعديل الجدول إلكترونياً", "Request to modify the schedule electronically", S1, "الأول", "First", "من الأحد إلى الإثنين", "Sunday to Monday", "10 - 11/ربيع أول/1448هـ", "10 - 11 Rabi I 1448H", "23 - 24/أغسطس/2026م", "23 - 24 August 2026"),
                    Cal(6,  "طلب الدراسة بنظام الزيارة", "Request to study as a visiting student", S1, "-", "-", "من الأحد إلى الثلاثاء", "Sunday to Tuesday", "19/صفر - 5/ربيع أول/1448هـ", "19 Safar - 5 Rabi I 1448H", "2 - 18/أغسطس/2026م", "2 - 18 August 2026"),
                    Cal(7,  "طلب الاعتذار عن دراسة الفصل الدراسي، وطلب الاعتذار عن مقرر، وطلب الانسحاب من الجامعة", "Request to withdraw from the semester, withdraw from a course, and withdraw from the university", S1, "الثالث إلى الثالث عشر", "Third to thirteenth", "من الأحد إلى الخميس", "Sunday to Thursday", "24/ربيع أول - 9/جمادى الآخرة/1448هـ", "24 Rabi I - 9 Jumada II 1448H", "6/سبتمبر - 19/نوفمبر/2026م", "6 September - 19 November 2026"),
                    Cal(8,  "إجازة اليوم الوطني", "National Day holiday", S1, "الخامس", "Fifth", "من الأربعاء إلى الخميس", "Wednesday to Thursday", "12 - 13/ربيع الآخر/1448هـ", "12 - 13 Rabi II 1448H", "23 - 24/سبتمبر/2026م", "23 - 24 September 2026"),
                    Cal(9,  "طلب مدة استثنائية للمتجاوزات للمدة النظامية", "Request for an exceptional period for students exceeding the standard duration", S1, "السادس إلى السابع", "Sixth to seventh", "من الأحد إلى الخميس", "Sunday to Thursday", "16 - 27/ربيع الآخر/1448هـ", "16 - 27 Rabi II 1448H", "27/سبتمبر - 8/أكتوبر/2026م", "27 September - 8 October 2026"),
                    Cal(10, "طلب إعادة قيد للطالبات المنقطعات", "Re-enrolment request for discontinued students", S1, "السادس إلى السابع", "Sixth to seventh", "من الأحد إلى الخميس", "Sunday to Thursday", "16 - 27/ربيع الآخر/1448هـ", "16 - 27 Rabi II 1448H", "27/سبتمبر - 8/أكتوبر/2026م", "27 September - 8 October 2026"),
                    Cal(11, "طلب التحويل الداخلي (تغيير التخصص)", "Internal transfer request (change of major)", S1, "الحادي عشر إلى الثاني عشر", "Eleventh to twelfth", "من الأحد إلى الخميس", "Sunday to Thursday", "21/جمادى الأولى - 2/جمادى الآخرة/1448هـ", "21 Jumada I - 2 Jumada II 1448H", "1 - 12/نوفمبر/2026م", "1 - 12 November 2026"),
                    Cal(12, "اختيار رغبات التخصيص للكليات: التأسيسية الصحية، التصاميم والفنون، الهندسة، والعلوم الإنسانية والاجتماعية", "Selection of specialization preferences for the colleges: Health Foundation, Design and Arts, Engineering, and Humanities and Social Sciences", S1, "الثالث عشر إلى الرابع عشر", "Thirteenth to fourteenth", "من الأحد إلى الخميس", "Sunday to Thursday", "5 - 23/جمادى الآخرة/1448هـ", "5 - 23 Jumada II 1448H", "15/نوفمبر - 3/ديسمبر/2026م", "15 November - 3 December 2026"),
                    Cal(13, "إجازة الخريف", "Autumn break", S1, "-", "-", "نهاية دوام يوم الخميس", "End of Thursday's working day", "9/جمادى الآخرة/1448هـ", "9 Jumada II 1448H", "19/نوفمبر/2026م", "19 November 2026"),
                    Cal(14, "بداية الدراسة بعد إجازة الخريف", "Start of study after the autumn break", S1, "الرابع عشر", "Fourteenth", "الأحد", "Sunday", "19/جمادى الآخرة/1448هـ", "19 Jumada II 1448H", "29/نوفمبر/2026م", "29 November 2026"),
                    Cal(15, "التسجيل والإرشاد الأكاديمي المبكر", "Early registration and academic advising", S1, "الرابع عشر", "Fourteenth", "من الإثنين إلى الخميس", "Monday to Thursday", "20 - 23/جمادى الآخرة/1448هـ", "20 - 23 Jumada II 1448H", "30/نوفمبر - 3/ديسمبر/2026م", "30 November - 3 December 2026"),
                    Cal(16, "الاختبارات النهائية", "Final exams", S1, "السادس عشر إلى الثامن عشر", "Sixteenth to eighteenth", "من الأحد إلى الخميس", "Sunday to Thursday", "4 - 22/رجب/1448هـ", "4 - 22 Rajab 1448H", "13 - 31/ديسمبر/2026م", "13 - 31 December 2026"),
                    Cal(17, "بداية إجازة منتصف العام الدراسي", "Start of the mid-year break", S1, "التاسع عشر", "Nineteenth", "نهاية دوام يوم الخميس", "End of Thursday's working day", "29/رجب/1448هـ", "29 Rajab 1448H", "7/يناير/2027م", "7 January 2027"),

                    // ===== الفصل الدراسي الثاني / Second semester =====
                    Cal(1,  "بداية الدراسة", "Start of study", S2, "الأول", "First", "الأحد", "Sunday", "9/شعبان/1448هـ", "9 Sha'ban 1448H", "17/يناير/2027م", "17 January 2027"),
                    Cal(2,  "طلب تأجيل الفصل الدراسي", "Request to postpone the semester", S2, "- إلى الثاني", "- to second", "من الأحد إلى الخميس", "Sunday to Thursday", "2 - 20/شعبان/1448هـ", "2 - 20 Sha'ban 1448H", "10 - 28/يناير/2027م", "10 - 28 January 2027"),
                    Cal(3,  "الحذف والإضافة", "Add and drop", S2, "-", "-", "من الإثنين إلى الأربعاء", "Monday to Wednesday", "3 - 5/شعبان/1448هـ", "3 - 5 Sha'ban 1448H", "11 - 13/يناير/2027م", "11 - 13 January 2027"),
                    Cal(4,  "طلب تعديل الجدول إلكترونياً", "Request to modify the schedule electronically", S2, "الأول", "First", "من الأحد إلى الإثنين", "Sunday to Monday", "9 - 10/شعبان/1448هـ", "9 - 10 Sha'ban 1448H", "17 - 18/يناير/2027م", "17 - 18 January 2027"),
                    Cal(5,  "طلب الدراسة بنظام الزيارة", "Request to study as a visiting student", S2, "-", "-", "من الأحد إلى الثلاثاء", "Sunday to Tuesday", "18/رجب - 11/شعبان/1448هـ", "18 Rajab - 11 Sha'ban 1448H", "27/ديسمبر/2026م - 19/يناير/2027م", "27 December 2026 - 19 January 2027"),
                    Cal(6,  "طلب الاعتذار عن دراسة الفصل الدراسي، وطلب الاعتذار عن مقرر، وطلب الانسحاب من الجامعة", "Request to withdraw from the semester, withdraw from a course, and withdraw from the university", S2, "الثالث إلى الثالث عشر", "Third to thirteenth", "من الأحد إلى الخميس", "Sunday to Thursday", "23/شعبان - 22/ذو القعدة/1448هـ", "23 Sha'ban - 22 Dhu al-Qi'dah 1448H", "31/يناير - 29/أبريل/2027م", "31 January - 29 April 2027"),
                    Cal(7,  "طلب التحويل من خارج جامعة الأميرة نورة بنت عبدالرحمن", "Transfer request from outside Princess Nourah bint Abdulrahman University", S2, "الرابع إلى الخامس", "Fourth to fifth", "من الأحد إلى الخميس", "Sunday to Thursday", "30/شعبان - 11/رمضان/1448هـ", "30 Sha'ban - 11 Ramadan 1448H", "7 - 18/فبراير/2027م", "7 - 18 February 2027"),
                    Cal(8,  "إجازة يوم التأسيس", "Founding Day holiday", S2, "السادس", "Sixth", "من الأحد إلى الإثنين", "Sunday to Monday", "14 - 15/رمضان/1448هـ", "14 - 15 Ramadan 1448H", "21 - 22/فبراير/2027م", "21 - 22 February 2027"),
                    Cal(9,  "بداية إجازة عيد الفطر", "Start of Eid al-Fitr break", S2, "-", "-", "نهاية دوام يوم الخميس", "End of Thursday's working day", "18/رمضان/1448هـ", "18 Ramadan 1448H", "25/فبراير/2027م", "25 February 2027"),
                    Cal(10, "بداية الدراسة بعد إجازة عيد الفطر", "Start of study after Eid al-Fitr break", S2, "السابع", "Seventh", "الأحد", "Sunday", "6/شوال/1448هـ", "6 Shawwal 1448H", "14/مارس/2027م", "14 March 2027"),
                    Cal(11, "طلب مدة استثنائية للمتجاوزات للمدة النظامية", "Request for an exceptional period for students exceeding the standard duration", S2, "السابع إلى الثامن", "Seventh to eighth", "من الأحد إلى الخميس", "Sunday to Thursday", "6 - 17/شوال/1448هـ", "6 - 17 Shawwal 1448H", "14 - 25/مارس/2027م", "14 - 25 March 2027"),
                    Cal(12, "طلب إعادة قيد للطالبات المنقطعات", "Re-enrolment request for discontinued students", S2, "السابع إلى الثامن", "Seventh to eighth", "من الأحد إلى الخميس", "Sunday to Thursday", "6 - 17/شوال/1448هـ", "6 - 17 Shawwal 1448H", "14 - 25/مارس/2027م", "14 - 25 March 2027"),
                    Cal(13, "طلب التحويل الداخلي (تغيير التخصص)", "Internal transfer request (change of major)", S2, "الحادي عشر إلى الثاني عشر", "Eleventh to twelfth", "من الأحد إلى الخميس", "Sunday to Thursday", "4 - 15/ذو القعدة/1448هـ", "4 - 15 Dhu al-Qi'dah 1448H", "11 - 22/أبريل/2027م", "11 - 22 April 2027"),
                    Cal(14, "اختيار رغبات التخصيص لطالبات الكليات: التأسيسية الصحية، التصاميم والفنون، الهندسة، العلوم الإنسانية والاجتماعية", "Selection of specialization preferences for students of the colleges: Health Foundation, Design and Arts, Engineering, Humanities and Social Sciences", S2, "الثالث عشر إلى الرابع عشر", "Thirteenth to fourteenth", "من الأحد إلى الخميس", "Sunday to Thursday", "18 - 29/ذو القعدة/1448هـ", "18 - 29 Dhu al-Qi'dah 1448H", "25/أبريل - 6/مايو/2027م", "25 April - 6 May 2027"),
                    Cal(15, "التسجيل والإرشاد الأكاديمي المبكر", "Early registration and academic advising", S2, "الرابع عشر", "Fourteenth", "من الإثنين إلى الخميس", "Monday to Thursday", "26 - 29/ذو القعدة/1448هـ", "26 - 29 Dhu al-Qi'dah 1448H", "3 - 6/مايو/2027م", "3 - 6 May 2027"),
                    Cal(16, "بداية إجازة عيد الأضحى المبارك", "Start of Eid al-Adha break", S2, "-", "-", "نهاية دوام يوم الخميس", "End of Thursday's working day", "29/ذو القعدة/1448هـ", "29 Dhu al-Qi'dah 1448H", "6/مايو/2027م", "6 May 2027"),
                    Cal(17, "بداية الدراسة بعد إجازة عيد الأضحى المبارك", "Start of study after Eid al-Adha break", S2, "الخامس عشر", "Fifteenth", "الأحد", "Sunday", "17/ذو الحجة/1448هـ", "17 Dhu al-Hijjah 1448H", "23/مايو/2027م", "23 May 2027"),
                    Cal(18, "الاختبارات النهائية", "Final exams", S2, "السادس عشر إلى الثامن عشر", "Sixteenth to eighteenth", "من الأحد إلى الخميس", "Sunday to Thursday", "24/ذو الحجة/1448هـ - 12/محرم/1449هـ", "24 Dhu al-Hijjah 1448H - 12 Muharram 1449H", "30/مايو - 17/يونيو/2027م", "30 May - 17 June 2027"),
                    Cal(19, "بداية إجازة نهاية العام للطالبات", "Start of the end-of-year break for students", S2, "-", "-", "نهاية دوام يوم الخميس", "End of Thursday's working day", "12/محرم/1449هـ", "12 Muharram 1449H", "17/يونيو/2027م", "17 June 2027"),
                    Cal(20, "بداية العام الدراسي 1449هـ", "Start of academic year 1449H", S2, "-", "-", "الأحد", "Sunday", "20/ربيع أول/1449هـ", "20 Rabi I 1449H", "22/أغسطس/2027م", "22 August 2027"),

                    // ===== الفصل الصيفي / Summer term =====
                    Cal(1, "الحذف والإضافة", "Add and drop", S3, "-", "-", "الخميس", "Thursday", "19/محرم/1449هـ", "19 Muharram 1449H", "24/يونيو/2027م", "24 June 2027"),
                    Cal(2, "بداية الدراسة", "Start of study", S3, "الأول", "First", "الأحد", "Sunday", "22/محرم/1449هـ", "22 Muharram 1449H", "27/يونيو/2027م", "27 June 2027"),
                    Cal(3, "طلب الاعتذار عن دراسة الفصل الصيفي والاعتذار عن مقرر", "Request to withdraw from the summer term and withdraw from a course", S3, "الثاني إلى الرابع", "Second to fourth", "من الأحد إلى الخميس", "Sunday to Thursday", "29/محرم - 18/صفر/1449هـ", "29 Muharram - 18 Safar 1449H", "4 - 22/يوليو/2027م", "4 - 22 July 2027"),
                    Cal(4, "الاختبارات النهائية", "Final exams", S3, "الثامن", "Eighth", "من الإثنين إلى الأربعاء", "Monday to Wednesday", "14 - 16/ربيع أول/1449هـ", "14 - 16 Rabi I 1449H", "16 - 18/أغسطس/2027م", "16 - 18 August 2027")
                }
            };

            // ---- ترويسة التقويم (bilingual header / intro / external link) --------------
            map[AcListNames.Headers] = new AcListDef
            {
                Name = AcListNames.Headers,
                TitleAr = "ترويسة التقويم الأكاديمي", TitleEn = "Academic calendar header",
                Description = "Section title, intro paragraph and external link (Arabic + English). One row.",
                ItemTitleAr = "العنوان (عربي)", ItemTitleEn = "Title (AR)",
                Fields = new List<AcFieldDef>
                {
                    new AcFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true),
                    new AcFieldDef("Intro", "المقدمة (عربي)", "Intro (AR)", SPFieldType.Note, false, 4),
                    new AcFieldDef("Intro_EN", "المقدمة (إنجليزي)", "Intro (EN)", SPFieldType.Note, false, 4),
                    new AcFieldDef("MoeLinkText", "نص الرابط (عربي)", "Link text (AR)", SPFieldType.Text),
                    new AcFieldDef("MoeLinkText_EN", "نص الرابط (إنجليزي)", "Link text (EN)", SPFieldType.Text),
                    new AcFieldDef("MoeLinkUrl", "رابط وزارة التعليم", "Ministry link URL", SPFieldType.Text, false, 0,
                                   "https://moe.gov.sa/...")
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row(
                        "Title", "تقويم الإجراءات الأكاديمية للعام الجامعي 1448هـ (2026م - 2027م)",
                        "Title_EN", "Academic procedures calendar for the year 1448H (2026-2027)",
                        "Intro", "يعرض هذا القسم أبرز الإجراءات الأكاديمية المنشورة في التقويم الرسمي لجامعة الأميرة نورة بنت عبدالرحمن، مرتبة حسب الفصل الدراسي لتسهيل المتابعة والرجوع السريع للتواريخ المهمة.",
                        "Intro_EN", "This section lists the main academic procedures published in the official calendar of Princess Nourah bint Abdulrahman University, arranged by semester for easy follow-up and quick reference to important dates.",
                        "MoeLinkText", "عرض التقويم الدراسي في وزارة التعليم",
                        "MoeLinkText_EN", "View the academic calendar at the Ministry of Education",
                        "MoeLinkUrl", "https://moe.gov.sa/ar/education/generaleducation/Pages/academicCalendar.aspx")
                }
            };

            // Editors allowed to use ucAcAdmin. Provisioned on /ar/ContentAdmin - the SAME
            // shared list the other sections (NouraStudents / International) use, so one
            // AdminUsers list governs every content-admin screen. An empty list authorises
            // nobody; membership in it is the entire permission model.
            map[AcListNames.AdminUsers] = new AcListDef
            {
                Name = AcListNames.AdminUsers,
                TitleAr = "مسؤولو المحتوى", TitleEn = "Content administrators",
                Description = "Users allowed to manage the portal content sections.",
                ItemTitleAr = "الاسم", ItemTitleEn = "Name",
                Fields = new List<AcFieldDef>
                {
                    new AcFieldDef("UserAccount", "حساب المستخدم", "User account", SPFieldType.User, true),
                    new AcFieldDef("Active", "مفعّل", "Active", SPFieldType.Boolean, true),
                    new AcFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true)
                },
                Seed = null
            };

            return map;
        }
    }
}
