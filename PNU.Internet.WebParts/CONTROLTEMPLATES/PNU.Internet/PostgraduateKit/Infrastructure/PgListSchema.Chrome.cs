using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Definitions and seed data for the page chrome that the kit's own master page renders:
    /// PgTopBar (the strip above the main navigation) and PgFooterLinks (the kit footer).
    /// </summary>
    public static partial class PgListSchema
    {
        /// <summary>Kinds of top bar entry.</summary>
        public static class PgTopBarKinds
        {
            public const string Info = "Info";
            public const string Button = "Button";

            public static readonly string[] All = new string[] { Info, Button };
        }

        /// <summary>Values the browser fills in for a top bar entry.</summary>
        public static class PgLiveValues
        {
            public const string None = "None";
            public const string Date = "Date";
            public const string Time = "Time";

            public static readonly string[] All = new string[] { None, Date, Time };
        }

        /// <summary>Where a footer entry belongs.</summary>
        public static class PgFooterAreas
        {
            public const string Column = "Column";
            public const string BottomBar = "BottomBar";

            public static readonly string[] All = new string[] { Column, BottomBar };
        }

        private static void BuildChromeLists(Dictionary<string, PgListDef> map)
        {
            BuildTopBar(map);
            BuildFooterLinks(map);
        }

        // 13) الشريط العلوي (The strip above the main navigation)
        private static void BuildTopBar(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.TopBar] = new PgListDef
            {
                Name = PgListNames.TopBar,
                TitleAr = "الشريط العلوي", TitleEn = "Top bar",
                Description = "The information items and shortcut buttons of the strip above the main navigation.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), Icon(), Link(),
                    new PgFieldDef("Kind", "النوع", "Kind", SPFieldType.Choice, true, 0,
                                   "Info = عنصر معلومات، Button = زر مختصر.", PgTopBarKinds.All),
                    new PgFieldDef("LiveValue", "قيمة متغيرة", "Live value", SPFieldType.Choice, true, 0,
                                   "Date أو Time يملأها المتصفح بتوقيت الرياض ويتجاهل العنوان.", PgLiveValues.All),
                    new PgFieldDef("AriaLabel", "وصف الزر (عربي)", "Button label (AR)", SPFieldType.Text),
                    new PgFieldDef("AriaLabel_EN", "وصف الزر (إنجليزي)", "Button label (EN)", SPFieldType.Text),
                    Phase(), Order()
                },
                Seed = new List<Dictionary<string, string>>
                {
                    Row("Title", "غائم", "Title_EN", "Cloudy",
                        "IconUrl", IconRoot + "cloud.svg",
                        "Kind", PgTopBarKinds.Info, "LiveValue", PgLiveValues.None, "ItemOrder", "1"),

                    // The browser fills these two in with the Riyadh date and time, so the
                    // titles are only there to identify the rows in the admin grid.
                    Row("Title", "التاريخ", "Title_EN", "Date",
                        "IconUrl", IconRoot + "calendar-04.svg",
                        "Kind", PgTopBarKinds.Info, "LiveValue", PgLiveValues.Date, "ItemOrder", "2"),

                    Row("Title", "الوقت", "Title_EN", "Time",
                        "IconUrl", IconRoot + "time-02.svg",
                        "Kind", PgTopBarKinds.Info, "LiveValue", PgLiveValues.Time, "ItemOrder", "3"),

                    Row("Title", "الرياض", "Title_EN", "Riyadh",
                        "IconUrl", IconRoot + "location-01.svg",
                        "Kind", PgTopBarKinds.Info, "LiveValue", PgLiveValues.None, "ItemOrder", "4"),

                    Row("Title", "المتقدمون", "Title_EN", "Applicants",
                        "IconUrl", IconRoot + "Button01.png",
                        "LinkUrl", "/ar/Deanship/postgraduate/Pages/applicants.aspx",
                        "AriaLabel", "محطة المتقدمين", "AriaLabel_EN", "Applicants station",
                        "Kind", PgTopBarKinds.Button, "LiveValue", PgLiveValues.None, "ItemOrder", "5"),

                    Row("Title", "المقيدون", "Title_EN", "Enrolled students",
                        "IconUrl", IconRoot + "Button02.png",
                        "LinkUrl", "/ar/Deanship/postgraduate/Pages/enrolled.aspx",
                        "AriaLabel", "محطة المقيدين", "AriaLabel_EN", "Enrolled students station",
                        "Kind", PgTopBarKinds.Button, "LiveValue", PgLiveValues.None, "ItemOrder", "6"),

                    Row("Title", "الخريجون", "Title_EN", "Graduates",
                        "IconUrl", IconRoot + "Button03.png",
                        "LinkUrl", "/ar/Deanship/postgraduate/Pages/graduates.aspx",
                        "AriaLabel", "محطة الخريجين", "AriaLabel_EN", "Graduates station",
                        "Kind", PgTopBarKinds.Button, "LiveValue", PgLiveValues.None, "ItemOrder", "7"),

                    Row("Title", "عمادة الدراسات العليا", "Title_EN", "Deanship of Graduate Studies",
                        "IconUrl", IconRoot + "Button04.png",
                        "LinkUrl", "/ar/Deanship/postgraduate/Pages/home.aspx",
                        "AriaLabel", "موقع عمادة الدراسات العليا", "AriaLabel_EN", "Deanship of Graduate Studies website",
                        "Kind", PgTopBarKinds.Button, "LiveValue", PgLiveValues.None, "ItemOrder", "8")
                }
            };
        }

        // 14) تذييل الحقيبة (The kit footer)
        private static void BuildFooterLinks(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.FooterLinks] = new PgListDef
            {
                Name = PgListNames.FooterLinks,
                TitleAr = "تذييل الحقيبة", TitleEn = "Kit footer",
                Description = "The link columns and the bottom bar links of the kit footer.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), Link(),
                    new PgFieldDef("GroupName", "عنوان العمود (عربي)", "Column heading (AR)", SPFieldType.Text, true),
                    new PgFieldDef("GroupName_EN", "عنوان العمود (إنجليزي)", "Column heading (EN)", SPFieldType.Text),
                    new PgFieldDef("Area", "الموضع", "Area", SPFieldType.Choice, true, 0,
                                   "Column = عمود روابط، BottomBar = الشريط السفلي.", PgFooterAreas.All),
                    YesNoField("NewTab", "فتح في نافذة جديدة", "Open in a new tab"),
                    Phase(), Order()
                },
                Seed = FooterLinksSeed()
            };
        }

        private static Dictionary<string, string> FooterLink(int order, string groupAr, string groupEn,
                                                             string titleAr, string titleEn,
                                                             string url, bool newTab)
        {
            return Row("Title", titleAr, "Title_EN", titleEn,
                       "LinkUrl", url,
                       "GroupName", groupAr, "GroupName_EN", groupEn,
                       "Area", PgFooterAreas.Column,
                       "NewTab", newTab ? "Yes" : "No",
                       "ItemOrder", N(order));
        }

        private static Dictionary<string, string> FooterBottom(int order, string titleAr, string titleEn, string url)
        {
            return Row("Title", titleAr, "Title_EN", titleEn,
                       "LinkUrl", url,
                       "GroupName", "", "GroupName_EN", "",
                       "Area", PgFooterAreas.BottomBar,
                       "NewTab", "Yes",
                       "ItemOrder", N(order));
        }

        private static List<Dictionary<string, string>> FooterLinksSeed()
        {
            const string summaryAr = "ملخص";
            const string summaryEn = "Summary";
            const string relatedAr = "روابط مهمة";
            const string relatedEn = "Important links";
            const string contactAr = "الاتصال والدعم";
            const string contactEn = "Contact and support";

            return new List<Dictionary<string, string>>
            {
                FooterLink(1, summaryAr, summaryEn,
                    "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا",
                    "The interactive academic kit for postgraduate students",
                    "/ar/Deanship/postgraduate/Pages/academic-kit.aspx", false),

                FooterLink(2, relatedAr, relatedEn,
                    "عمادة الدراسات العليا", "Deanship of Graduate Studies",
                    "/ar/Deanship/postgraduate/Pages/home.aspx", false),

                FooterLink(3, contactAr, contactEn,
                    "عمادة الدراسات العليا dgs-pd@pnu.edu.sa",
                    "Deanship of Graduate Studies dgs-pd@pnu.edu.sa",
                    "mailto:dgs-pd@pnu.edu.sa", false),

                FooterLink(4, contactAr, contactEn,
                    "خدمات المستفيدين dga-aa@pnu.edu.sa",
                    "Beneficiary services dga-aa@pnu.edu.sa",
                    "mailto:dga-aa@pnu.edu.sa", false),

                FooterBottom(5, "سياسات الجامعة", "University policies",
                    "/ar/SECURITYCYBER/Pages/Privacy-Policy.aspx"),
                FooterBottom(6, "خريطة الموقع", "Site map",
                    "/ar/Pages/SiteMap.aspx"),
                FooterBottom(7, "المشاركة الإلكترونية", "Electronic participation",
                    "https://eparticipation.my.gov.sa/"),
                FooterBottom(8, "البيانات المفتوحة", "Open data",
                    "/ar/AboutUniversity/Pages/PNUinNumbers.aspx")
            };
        }
    }
}
