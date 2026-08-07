using Microsoft.SharePoint;
using Portal.Main.Helper.Utils;
using System;
using System.Web;

namespace PNU.Internet.WebParts.Helpers
{
    /// <summary>
    /// Provisions the 3 partner lists at the root web of the site collection if they do not already exist.
    /// Call <see cref="EnsurePartnersLists"/> once from Page_Load; it is guarded so it runs at most once
    /// per AppDomain and is safe against concurrent first-hit requests.
    ///
    /// Lists provisioned:
    ///   - LocalPartnersHomePage         (partner cards, Local section)
    ///   - InternationalPartnersHomePage (partner cards, International section)
    ///   - PartnersHomePageSections      (editable section headings + descriptions)
    /// </summary>
    public static class PartnersListProvisioner
    {
        public const string LIST_LOCAL         = "LocalPartnersHomePage";
        public const string LIST_INTERNATIONAL = "InternationalPartnersHomePage";
        public const string LIST_SECTIONS      = "PartnersHomePageSections";

        // Standard path where partner logos live in the style library.
        // Code-behind and seed data both use this prefix.
        public const string LOGO_PATH_PREFIX = "/Style%20Library/DGA/public/images/partners/partners-logos/";

        // Fallback URL used by a few partner cards in the original design.
        private const string FALLBACK_URL = "https://pnu.edu.sa/ar/AboutUniversity/Pages/allPartners.aspx";

        private static volatile bool _alreadyChecked = false;
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// Ensures all 3 partner lists exist at the root web. Safe to call on every Page_Load:
        /// the work runs only once per AppDomain, and subsequent calls are a fast boolean check.
        /// </summary>
        public static void EnsurePartnersLists()
        {
            if (_alreadyChecked) return;

            lock (_syncRoot)
            {
                if (_alreadyChecked) return;

                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb rootWeb = site.RootWeb)
                            {
                                rootWeb.AllowUnsafeUpdates = true;
                                try
                                {
                                    EnsureSectionsList(rootWeb);
                                    EnsureLocalPartnersList(rootWeb);
                                    EnsureInternationalPartnersList(rootWeb);
                                }
                                finally
                                {
                                    rootWeb.AllowUnsafeUpdates = false;
                                }
                            }
                        }
                    });

                    _alreadyChecked = true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        string urlStr = "PartnersListProvisioner";
                        try
                        {
                            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                                urlStr = HttpContext.Current.Request.Url.ToString();
                        }
                        catch { /* ignore */ }

                        Publics.WriteToLog(
                            urlStr,
                            "PartnersListProvisioner.EnsurePartnersLists",
                            ex.Message + " | " + ex.StackTrace);
                    }
                    catch { /* swallow logging errors */ }
                }
            }
        }

        // ------------------------------------------------------------------
        //  Sections list (headings + descriptions for Local & International)
        // ------------------------------------------------------------------

        private static void EnsureSectionsList(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_SECTIONS) != null) return;

            Guid id = web.Lists.Add(LIST_SECTIONS, "Section headings and descriptions for the Home page partners sections", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            // Title is reused as the section key ("Local" / "International").
            AddField(list, "<Field Type='Text' DisplayName='HeadingAr' Name='HeadingAr' />");
            AddField(list, "<Field Type='Text' DisplayName='HeadingEn' Name='HeadingEn' />");
            AddField(list, "<Field Type='Note' DisplayName='DescriptionAr' Name='DescriptionAr' NumLines='4' RichText='FALSE' />");
            AddField(list, "<Field Type='Note' DisplayName='DescriptionEn' Name='DescriptionEn' NumLines='4' RichText='FALSE' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            // Seed rows: one per section. Title acts as the section key.
            AddSectionRow(list,
                "Local",
                "الشركاء المحليون",
                "Local Partners",
                "جهات وطنية ومؤسسات تعليمية وتنموية تدعم تعاون الجامعة داخل المملكة.",
                "National entities and educational/development institutions supporting the university's partnerships within the Kingdom.",
                1);

            AddSectionRow(list,
                "International",
                "الشركاء الدوليون",
                "International Partners",
                "جامعات ومؤسسات دولية تعزز تبادل الخبرات والتعاون الأكاديمي والبحثي.",
                "International universities and institutions enhancing the exchange of expertise and academic/research collaboration.",
                2);
        }

        private static void AddSectionRow(SPList list, string key, string headingAr, string headingEn, string descAr, string descEn, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = key;
            row["HeadingAr"] = headingAr;
            row["HeadingEn"] = headingEn;
            row["DescriptionAr"] = descAr;
            row["DescriptionEn"] = descEn;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  Local Partners
        // ------------------------------------------------------------------

        private static void EnsureLocalPartnersList(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_LOCAL) != null) return;

            Guid id = web.Lists.Add(LIST_LOCAL, "Local partner cards shown on the Home page", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            AddPartnerSchema(list);
            HideFromUI(list);
            list.Update();

            // Seed: 18 local partners in the exact order from the original design.
            AddPartnerRow(list, "الهيئة العامة للمنافسة",                 "الهيئة العامة للمنافسة",                 "https://gac.gov.sa/",          "partner-logo-01.png",  1);
            AddPartnerRow(list, "الهيئة الملكية لمدينة الرياض",           "الهيئة الملكية لمدينة الرياض",           "https://www.rcrc.gov.sa/",     "partner-logo-02.jpg",  2);
            AddPartnerRow(list, "جامعة شقراء",                             "جامعة شقراء",                             "https://www.su.edu.sa/",       "partner-logo-03.png",  3);
            AddPartnerRow(list, "سابك",                                     "سابك",                                     "https://www.sabic.com/",       "partner-logo-04.jpg",  4);
            AddPartnerRow(list, "الاتحاد السعودي لكرة القدم",              "الاتحاد السعودي لكرة القدم",              "https://www.saff.com.sa/",     "partner-logo-05.jpg",  5);
            AddPartnerRow(list, "المرصد الوطني للعمل",                     "المرصد الوطني للعمل",                     "https://nlo.gov.sa/",          "partner-logo-06.jpg",  6);
            AddPartnerRow(list, "الهيئة السعودية للمعارض والمؤتمرات",     "الهيئة السعودية للمعارض والمؤتمرات",     "https://scega.gov.sa/",        "partner-logo-07.png",  7);
            AddPartnerRow(list, "هيئة الزكاة والضريبة والجمارك",           "هيئة الزكاة والضريبة والجمارك",           "https://zatca.gov.sa/",        "partner-logo-08.png",  8);
            AddPartnerRow(list, "مؤسسة محمد بن سلمان مسك",                 "مؤسسة محمد بن سلمان مسك",                 "https://misk.org.sa/",         "partner-logo-09.png",  9);
            AddPartnerRow(list, "مجلس الضمان الصحي",                       "مجلس الضمان الصحي",                       "https://chi.gov.sa/",          "partner-logo-10.jpg",  10);
            AddPartnerRow(list, "المؤسسة العامة للتدريب التقني والمهني",  "المؤسسة العامة للتدريب التقني والمهني",  "https://tvtc.gov.sa/",         "partner-logo-11.jpeg", 11);
            AddPartnerRow(list, "محمية الملك عبدالعزيز الملكية",           "محمية الملك عبدالعزيز الملكية",           FALLBACK_URL,                   "partner-logo-12.png",  12);
            AddPartnerRow(list, "جامعة دار الحكمة",                        "جامعة دار الحكمة",                        "https://www.dah.edu.sa/",      "partner-logo-13.png",  13);
            AddPartnerRow(list, "جامعة الملك عبدالعزيز",                   "جامعة الملك عبدالعزيز",                   "https://www.kau.edu.sa/",      "partner-logo-14.png",  14);
            AddPartnerRow(list, "جامعة الملك عبدالله للعلوم والتقنية",    "جامعة الملك عبدالله للعلوم والتقنية",    "https://www.kaust.edu.sa/",    "partner-logo-15.png",  15);
            AddPartnerRow(list, "وزارة الصحة",                              "وزارة الصحة",                              "https://www.moh.gov.sa/",      "partner-logo-16.png",  16);
            AddPartnerRow(list, "جامعة الفيصل",                             "جامعة الفيصل",                             "https://www.alfaisal.edu/",    "partner-logo-17.jpg",  17);
            AddPartnerRow(list, "لوريال",                                   "لوريال",                                   "https://www.loreal.com/",      "partner-logo-18.jpg",  18);
        }

        // ------------------------------------------------------------------
        //  International Partners
        // ------------------------------------------------------------------

        private static void EnsureInternationalPartnersList(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_INTERNATIONAL) != null) return;

            Guid id = web.Lists.Add(LIST_INTERNATIONAL, "International partner cards shown on the Home page", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            AddPartnerSchema(list);
            HideFromUI(list);
            list.Update();

            // Seed: 19 international partners in the exact order from the original design.
            AddPartnerRow(list, "جامعة دبلن سيتي",              "جامعة دبلن سيتي",              "https://www.dcu.ie/",          "partner-logo-19.png",  1);
            AddPartnerRow(list, "جامعة إكستر",                   "جامعة إكستر",                   "https://www.exeter.ac.uk/",    "partner-logo-20.png",  2);
            AddPartnerRow(list, "جامعة إسكس",                    "جامعة إسكس",                    "https://www.essex.ac.uk/",     "partner-logo-21.jpeg", 3);
            AddPartnerRow(list, "إنترناشونال هاوس",              "إنترناشونال هاوس",              "https://ihworld.com/",         "partner-logo-22.jpg",  4);
            AddPartnerRow(list, "جامعة نيوكاسل",                 "جامعة نيوكاسل",                 "https://www.ncl.ac.uk/",       "partner-logo-23.png",  5);
            AddPartnerRow(list, "جامعة غلاسكو",                  "جامعة غلاسكو",                  "https://www.gla.ac.uk/",       "partner-logo-24.png",  6);
            AddPartnerRow(list, "جامعة دندي",                    "جامعة دندي",                    "https://www.dundee.ac.uk/",    "partner-logo-25.jpg",  7);
            AddPartnerRow(list, "جامعة ستراثكلايد",              "جامعة ستراثكلايد",              "https://www.strath.ac.uk/",    "partner-logo-26.jpg",  8);
            AddPartnerRow(list, "جامعة ساوثهامبتون",             "جامعة ساوثهامبتون",             "https://www.southampton.ac.uk/","partner-logo-27.png", 9);
            AddPartnerRow(list, "شريك دولي",                      "شريك دولي",                      FALLBACK_URL,                   "partner-logo-28.jpg",  10);
            AddPartnerRow(list, "GRC Seoul",                       "GRC Seoul",                       FALLBACK_URL,                   "partner-logo-29.png",  11);
            AddPartnerRow(list, "إنسياد",                         "إنسياد",                         "https://www.insead.edu/",      "partner-logo-30.jpg",  12);
            AddPartnerRow(list, "جامعة زايد",                    "جامعة زايد",                    "https://www.zu.ac.ae/",        "partner-logo-31.png",  13);
            AddPartnerRow(list, "First Info Tech",                 "First Info Tech",                 FALLBACK_URL,                   "partner-logo-32.jpg",  14);
            AddPartnerRow(list, "أكاديمية باريس سان جيرمان",     "أكاديمية باريس سان جيرمان",     "https://www.psgacademy.com/",  "partner-logo-33.png",  15);
            AddPartnerRow(list, "جامعة بكين للغة والثقافة",      "جامعة بكين للغة والثقافة",      "https://www.blcu.edu.cn/",     "partner-logo-34.png",  16);
            AddPartnerRow(list, "SKEMA Business School",           "SKEMA Business School",           "https://www.skema.edu/",       "partner-logo-35.png",  17);
            AddPartnerRow(list, "جامعة أبردين",                  "جامعة أبردين",                  "https://www.abdn.ac.uk/",      "partner-logo-36.jpg",  18);
            AddPartnerRow(list, "جامعة دورهام",                  "جامعة دورهام",                  "https://www.durham.ac.uk/",    "partner-logo-37.png",  19);
        }

        // ------------------------------------------------------------------
        //  Shared schema for both partner lists
        // ------------------------------------------------------------------

        private static void AddPartnerSchema(SPList list)
        {
            // Title column already exists (Arabic partner name goes here).
            AddField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />");
            AddField(list, "<Field Type='URL' DisplayName='URL' Name='URL' Format='Hyperlink' />");
            // LogoFileName: just the filename (e.g. "partner-logo-01.png"). Code-behind prepends the Style Library prefix.
            AddField(list, "<Field Type='Text' DisplayName='LogoFileName' Name='LogoFileName' />");
            AddField(list, "<Field Type='Text' DisplayName='AriaLabelAr' Name='AriaLabelAr' />");
            AddField(list, "<Field Type='Text' DisplayName='AriaLabelEn' Name='AriaLabelEn' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");
        }

        private static void AddPartnerRow(SPList list, string nameAr, string nameEn, string url, string logoFileName, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = nameAr;
            row["TitleEn"] = nameEn;
            if (!string.IsNullOrEmpty(url))
            {
                row["URL"] = new SPFieldUrlValue { Url = url, Description = nameAr };
            }
            row["LogoFileName"] = logoFileName;
            // AriaLabelAr/AriaLabelEn intentionally left empty; code-behind falls back to Title/TitleEn.
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  Utilities
        // ------------------------------------------------------------------

        private static void AddField(SPList list, string fieldXml)
        {
            list.Fields.AddFieldAsXml(fieldXml, true, SPAddFieldOptions.AddFieldInternalNameHint);
        }

        /// <summary>
        /// Hides the list from Quick Launch and disables search crawling. Stays visible in Site Contents.
        /// </summary>
        private static void HideFromUI(SPList list)
        {
            try
            {
                list.Hidden = false;
                list.OnQuickLaunch = false;
                list.NoCrawl = true;
            }
            catch { /* cosmetic - ignore failures */ }
        }
    }
}
