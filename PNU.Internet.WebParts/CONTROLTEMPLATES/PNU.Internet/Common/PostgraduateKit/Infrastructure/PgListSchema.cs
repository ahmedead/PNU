using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    public class PgFieldDef
    {
        public string InternalName { get; set; }
        public string DisplayAr { get; set; }
        public string DisplayEn { get; set; }
        public SPFieldType Type { get; set; }
        public bool InGrid { get; set; }
        public int Rows { get; set; }
        public string Hint { get; set; }
        public string[] Choices { get; set; }

        public PgFieldDef(string name, string ar, string en, SPFieldType type,
                          bool inGrid = false, int rows = 0, string hint = null, string[] choices = null)
        {
            InternalName = name; DisplayAr = ar; DisplayEn = en;
            Type = type; InGrid = inGrid; Rows = rows; Hint = hint; Choices = choices;
        }

        public string Display { get { return PgHelper.Pick(DisplayAr, DisplayEn); } }
    }

    public class PgListDef
    {
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public List<PgFieldDef> Fields { get; set; }
        public List<Dictionary<string, string>> Seed { get; set; }

        public string Display { get { return PgHelper.Pick(TitleAr, TitleEn); } }
    }

    /// <summary>
    /// Single source of truth for every Postgraduate Academic Kit list, its custom fields
    /// and its bilingual seed data. The definitions are split across partial files:
    /// PgListSchema.Core.cs, PgListSchema.Content.cs, PgListSchema.Procedures.cs and
    /// PgListSchema.Faq.cs.
    /// All icon paths are rooted at /Style Library/DGA/public/images/icons/.
    /// </summary>
    public static partial class PgListSchema
    {
        /// <summary>
        /// Root of the kit's own asset folder. The v3.0 design ships a newer build of
        /// style.min.css than the one deployed under /Style Library/DGA/, so the kit keeps
        /// its assets in a separate folder and no other page is affected.
        /// </summary>
        public const string AssetRoot = "/Style Library/PostgraduateKit/public/";

        public const string IconRoot = AssetRoot + "images/icons/";
        public const string ImageRoot = AssetRoot + "images/";

        private static Dictionary<string, PgListDef> _all;
        private static readonly object _lock = new object();

        public static Dictionary<string, PgListDef> All
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

        public static PgListDef Get(string listName)
        {
            PgListDef def;
            return All.TryGetValue(listName, out def) ? def : null;
        }

        // ---- shared field builders -------------------------------------------------

        private static readonly string[] YesNo = new string[] { "Yes", "No" };

        internal static PgFieldDef TitleEn()
        {
            return new PgFieldDef("Title_EN", "العنوان (إنجليزي)", "Title (EN)", SPFieldType.Text, true);
        }

        internal static PgFieldDef DescAr(int rows = 3)
        {
            return new PgFieldDef("Description", "الوصف (عربي)", "Description (AR)", SPFieldType.Note, false, rows);
        }

        internal static PgFieldDef DescEn(int rows = 3)
        {
            return new PgFieldDef("Description_EN", "الوصف (إنجليزي)", "Description (EN)", SPFieldType.Note, false, rows);
        }

        internal static PgFieldDef Icon()
        {
            return new PgFieldDef("IconUrl", "رابط الأيقونة", "Icon URL", SPFieldType.URL, false, 0,
                                  IconRoot + "step01.png");
        }

        internal static PgFieldDef Link()
        {
            return new PgFieldDef("LinkUrl", "الرابط", "Link URL", SPFieldType.URL);
        }

        internal static PgFieldDef Order()
        {
            return new PgFieldDef("ItemOrder", "الترتيب", "Order", SPFieldType.Number, true);
        }

        internal static PgFieldDef Phase()
        {
            return new PgFieldDef("Phase", "الفئة", "Audience", SPFieldType.Choice, true, 0,
                                  "Applicants = المتقدمون، Enrolled = المقيدون، Graduates = الخريجون. اتركه فارغاً ليظهر العنصر في الصفحات الثلاث.",
                                  PgPhases.All);
        }

        internal static PgFieldDef YesNoField(string name, string ar, string en, bool inGrid = false)
        {
            return new PgFieldDef(name, ar, en, SPFieldType.Choice, inGrid, 0, null, YesNo);
        }

        internal static Dictionary<string, string> Row(params string[] kv)
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i + 1 < kv.Length; i += 2) d[kv[i]] = kv[i + 1];
            return d;
        }

        private static Dictionary<string, PgListDef> Build()
        {
            var map = new Dictionary<string, PgListDef>(StringComparer.OrdinalIgnoreCase);

            BuildCoreLists(map);
            BuildContentLists(map);
            BuildProcedureList(map);
            BuildFaqList(map);
            BuildChromeLists(map);
            BuildHubList(map);

            return map;
        }
    }
}
