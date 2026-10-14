using Microsoft.SharePoint;
using Portal.Main.Helper.Utils;
using System;
using System.Web;

namespace PNU.Internet.WebParts.Helpers
{
    /// <summary>
    /// Provisions and maintains the footer lists at the root web of the site collection.
    /// Call <see cref="EnsureFooterLists"/> once from Page_Load; it is guarded so it runs at most once
    /// per AppDomain and is safe against concurrent first-hit requests.
    /// </summary>
    public static class FooterListProvisioner
    {
        // List internal names
        public const string LIST_ABOUT = "AboutFooter";
        public const string LIST_USEFULLINKS = "UsefulLinksUsFooter";
        public const string LIST_CONTACT = "ContactUsFooter";
        public const string LIST_FOLLOWUS = "FollowUsUsFooter";
        public const string LIST_DOWNLINKS = "DownLinksFooter";
        public const string LIST_NAVSECTIONS = "FooterNavSections";
        public const string LIST_NAVLINKS = "FooterNavLinks";
        public const string LIST_SETTINGS = "FooterSettings";

        private static volatile bool _alreadyChecked = false;
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// Forces provisioning to run regardless of the in-memory cache flag.
        /// Useful when called from the Admin page or setup routines.
        /// </summary>
        public static void ForceProvision()
        {
            _alreadyChecked = false;
            EnsureFooterLists();
            FooterCache.Invalidate();
        }

        /// <summary>Parameterless entry point looked up by ContentAdminListProvisioner (reflection).</summary>
        public static void EnsureAllListsExist()
        {
            EnsureFooterLists();
        }

        /// <summary>
        /// Ensures all footer lists and columns exist at the root web.
        /// </summary>
        public static void EnsureFooterLists()
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
                            SPWeb rootWeb = site.RootWeb; // owned by SPSite: do not dispose
                            {
                                rootWeb.AllowUnsafeUpdates = true;
                                try
                                {
                                    EnsureAboutFooter(rootWeb);
                                    EnsureUsefulLinksFooter(rootWeb);
                                    EnsureContactUsFooter(rootWeb);
                                    EnsureFollowUsFooter(rootWeb);
                                    EnsureDownLinksFooter(rootWeb);
                                    EnsureNavSections(rootWeb);
                                    EnsureNavLinks(rootWeb);
                                    EnsureSettings(rootWeb);
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
                        string urlStr = "FooterListProvisioner";
                        try
                        {
                            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                                urlStr = HttpContext.Current.Request.Url.ToString();
                        }
                        catch { /* ignore */ }

                        Publics.WriteToLog(
                            urlStr,
                            "FooterListProvisioner.EnsureFooterLists",
                            ex.Message + " | " + ex.StackTrace);
                    }
                    catch { /* swallow logging errors */ }
                }
            }
        }

        // ------------------------------------------------------------------
        //  1. Footer Navigation Sections (Columns 1, 2, 3)
        // ------------------------------------------------------------------

        private static void EnsureNavSections(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_NAVSECTIONS);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_NAVSECTIONS, "Sections shown in the footer navigation columns", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list, "<Field Type='Text' DisplayName='TitleAr' Name='TitleAr' />", "TitleAr");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />", "TitleEn");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ColumnNumber' Name='ColumnNumber' Decimals='0'><Default>1</Default></Field>", "ColumnNumber");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0'><Default>1</Default></Field>", "ItemOrder");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>", "Visibility");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                AddSectionRow(list, "Overview", "نظرة عامة", "Overview", 1, 1);
                AddSectionRow(list, "Opportunities", "فرصك في نورة", "Your opportunities in Noura", 2, 2);
                AddSectionRow(list, "UniversityLife", "الحياة الجامعية", "University life", 2, 3);
                AddSectionRow(list, "RelatedLinks", "مواقع ذات صلة", "Related Links", 3, 4);
            }
        }

        private static void AddSectionRow(SPList list, string key, string titleAr, string titleEn, int columnNumber, int order)
        {
            SPListItem item = list.Items.Add();
            item["Title"] = key;
            item["TitleAr"] = titleAr;
            item["TitleEn"] = titleEn;
            item["ColumnNumber"] = columnNumber;
            item["ItemOrder"] = order;
            item["Visibility"] = true;
            item.Update();
        }

        // ------------------------------------------------------------------
        //  2. Footer Navigation Links
        // ------------------------------------------------------------------

        private static void EnsureNavLinks(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_NAVLINKS);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_NAVLINKS, "Navigation links shown under footer sections", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />", "TitleEn");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='SectionKey' Name='SectionKey' />", "SectionKey");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='URLAr' Name='URLAr' Format='Hyperlink' />", "URLAr");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='URLEn' Name='URLEn' Format='Hyperlink' />", "URLEn");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='OpenInNewTab' Name='OpenInNewTab'><Default>0</Default></Field>", "OpenInNewTab");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>", "Visibility");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />", "ItemOrder");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                // Overview Section
                AddNavLinkRow(list, "الأسئلة الشائعة", "FAQs", "Overview", "/ar/FAQs/Pages/default.aspx", "/en/FAQs/Pages/default.aspx", false, 1);
                AddNavLinkRow(list, "تيك كير لتقنية المعلومات", "Tech Care for Information Technology", "Overview", "https://techcare.pnu.edu.sa/", "https://techcare.pnu.edu.sa/en", true, 2);
                AddNavLinkRow(list, "المستشفى الجامعي", "KAAUH Hospital", "Overview", "https://www.kaauh.edu.sa/ar", "https://www.kaauh.edu.sa/en", true, 3);
                AddNavLinkRow(list, "تواصل نورة", "Tawasul with Nourah", "Overview", "https://tawasulnourah.pnu.edu.sa/", "https://tawasulnourah.pnu.edu.sa/en", true, 4);
                AddNavLinkRow(list, "بوابة الخدمات الإلكترونية", "e-services portal", "Overview", "https://eservice.pnu.edu.sa/", "https://eservice.pnu.edu.sa/en", true, 5);
                AddNavLinkRow(list, "المساعدة والدعم", "Help and Support", "Overview", "/ar/AboutUniversity/Pages/Support.aspx", "/en/AboutUniversity/Pages/Support.aspx", true, 6);
                AddNavLinkRow(list, "تواصل معنا", "Contact Us", "Overview", "/ar/Pages/ContactUsForm.aspx", "/en/Pages/ContactUsForm.aspx", true, 7);

                // Opportunities Section
                AddNavLinkRow(list, "التوظيف", "Recruitment", "Opportunities", "/ar/AboutUniversity/Pages/Careers.aspx", "/en/AboutUniversity/Pages/Careers.aspx", false, 1);
                AddNavLinkRow(list, "استثمر في نورة", "Invest in Noura", "Opportunities", "/ar/NourahOpportunities/Pages/Invest-in-noura.aspx", "/en/NourahOpportunities/Pages/Invest-in-noura.aspx", false, 2);
                AddNavLinkRow(list, "التدريب", "Training", "Opportunities", "/ar/NourahOpportunities/Pages/التدريب.aspx", "/en/NourahOpportunities/Pages/Training.aspx", false, 3);
                AddNavLinkRow(list, "التطوع", "Volunteering", "Opportunities", "https://norahattaa.pnu.edu.sa/ar/Pages/default.aspx", "https://norahattaa.pnu.edu.sa/en/Pages/default.aspx", true, 4);

                // University Life Section
                AddNavLinkRow(list, "جولة افتراضية 360", "360 virtual tour", "UniversityLife", "/ar/UniversityLife/Pages/VirtualTour.aspx", "/en/UniversityLife/Pages/VirtualTour.aspx", false, 1);
                AddNavLinkRow(list, "مرافق الجامعة", "University Facilities", "UniversityLife", "/ar/UniversityLife/Pages/UniversityFacilities.aspx", "/en/UniversityLife/Pages/UniversityFacilities.aspx", false, 2);

                // Related Links Section
                AddNavLinkRow(list, "وزارة التعليم", "Ministry of Education", "RelatedLinks", "https://www.moe.gov.sa/ar/Pages/default.aspx", "https://www.moe.gov.sa/en/Pages/default.aspx", true, 1);
                AddNavLinkRow(list, "المنصة الوطنية", "National Government Portal", "RelatedLinks", "https://my.gov.sa/ar", "https://my.gov.sa/en", true, 2);
                AddNavLinkRow(list, "البوابة الوطنية للبيانات المفتوحة", "National Open Data Portal", "RelatedLinks", "https://data.gov.sa/", "https://data.gov.sa/en/", true, 3);
                AddNavLinkRow(list, "منصة الاستشارات (استطلاع)", "Public Consultation (Istitlaa)", "RelatedLinks", "https://istitlaa.ncc.gov.sa/", "https://istitlaa.ncc.gov.sa/en/", true, 4);
                AddNavLinkRow(list, "المنصة الوطنية للتوظيف (جدارات)", "National Employment Portal (Jadarat)", "RelatedLinks", "https://jadarat.sa/", "https://jadarat.sa/en/", true, 5);
            }
        }

        public static void AddNavLinkRow(SPList list, string titleAr, string titleEn, string sectionKey, string urlAr, string urlEn, bool openInNewTab, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = titleAr;
            row["TitleEn"] = titleEn;
            row["SectionKey"] = sectionKey;
            row["URLAr"] = new SPFieldUrlValue { Url = urlAr, Description = titleAr };
            row["URLEn"] = new SPFieldUrlValue { Url = urlEn, Description = titleEn };
            row["OpenInNewTab"] = openInNewTab;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  3. Footer Settings & Logos
        // ------------------------------------------------------------------

        private static void EnsureSettings(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_SETTINGS);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_SETTINGS, "General settings, copyright and logo URLs for the footer", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list, "<Field Type='Text' DisplayName='CopyrightAr' Name='CopyrightAr' />", "CopyrightAr");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='CopyrightEn' Name='CopyrightEn' />", "CopyrightEn");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='DevelopedByAr' Name='DevelopedByAr' />", "DevelopedByAr");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='DevelopedByEn' Name='DevelopedByEn' />", "DevelopedByEn");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='SdaiaUrlAr' Name='SdaiaUrlAr' Format='Hyperlink' />", "SdaiaUrlAr");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='SdaiaUrlEn' Name='SdaiaUrlEn' Format='Hyperlink' />", "SdaiaUrlEn");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='VisionUrlAr' Name='VisionUrlAr' Format='Hyperlink' />", "VisionUrlAr");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='VisionUrlEn' Name='VisionUrlEn' Format='Hyperlink' />", "VisionUrlEn");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='PnuHomeUrlAr' Name='PnuHomeUrlAr' Format='Hyperlink' />", "PnuHomeUrlAr");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='PnuHomeUrlEn' Name='PnuHomeUrlEn' Format='Hyperlink' />", "PnuHomeUrlEn");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                SPListItem row = list.Items.Add();
                row["Title"] = "General";
                row["CopyrightAr"] = "جميع الحقوق محفوظة 2026 | جامعة الأميرة نورة بنت عبدالرحمن";
                row["CopyrightEn"] = "All Rights Reserved 2026 | Princess Nourah bint Abdulrahman University";
                row["DevelopedByAr"] = " - تم تطويره وصيانته بواسطة إدارة التحول الرقمي";
                row["DevelopedByEn"] = " - Developed and maintained by the Digital Transformation Department";
                row["SdaiaUrlAr"] = new SPFieldUrlValue { Url = "https://sdaia.gov.sa/ar/default.aspx", Description = "SDAIA AR" };
                row["SdaiaUrlEn"] = new SPFieldUrlValue { Url = "https://sdaia.gov.sa/en/default.aspx", Description = "SDAIA EN" };
                row["VisionUrlAr"] = new SPFieldUrlValue { Url = "https://www.vision2030.gov.sa/", Description = "Vision 2030 AR" };
                row["VisionUrlEn"] = new SPFieldUrlValue { Url = "https://www.vision2030.gov.sa/en/", Description = "Vision 2030 EN" };
                row["PnuHomeUrlAr"] = new SPFieldUrlValue { Url = "/ar", Description = "PNU AR" };
                row["PnuHomeUrlEn"] = new SPFieldUrlValue { Url = "/en", Description = "PNU EN" };
                row.Update();
            }
        }

        // ------------------------------------------------------------------
        //  4. About Section
        // ------------------------------------------------------------------

        private static void EnsureAboutFooter(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_ABOUT);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_ABOUT, "About section (description) shown in the footer", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list, "<Field Type='Note' DisplayName='DescriptionAr' Name='DescriptionAr' NumLines='6' RichText='FALSE' />", "DescriptionAr");
            changed |= EnsureField(list, "<Field Type='Note' DisplayName='DescriptionEn' Name='DescriptionEn' NumLines='6' RichText='FALSE' />", "DescriptionEn");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>", "Visibility");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0'><Default>1</Default></Field>", "ItemOrder");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                SPListItem item = list.Items.Add();
                item["Title"] = "Main";
                item["DescriptionAr"] =
                    "جامعة الأميرة نورة بنت عبد الرحمن هي أكبر جامعة نسائية في العالم وإحدى أبرز المؤسسات التعليمية في المملكة العربية السعودية، " +
                    "توفّر بيئة أكاديمية متكاملة تجمع بين التميّز العلمي والبحثي والتطوير المهني، " +
                    "وتقدّم برامج دراسية متنوعة في مختلف التخصصات مع مرافق حديثة وخدمات طلابية متكاملة.";
                item["DescriptionEn"] =
                    "Princess Nourah bint Abdulrahman University is the largest women's university in the world and one of the leading educational institutions in Saudi Arabia, " +
                    "offering an integrated academic environment that combines scientific excellence, research, and professional development.";
                item["Visibility"] = true;
                item["ItemOrder"] = 1;
                item.Update();
            }
        }

        // ------------------------------------------------------------------
        //  5. Useful / Related Links (Backwards compatibility)
        // ------------------------------------------------------------------

        private static void EnsureUsefulLinksFooter(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_USEFULLINKS);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_USEFULLINKS, "Related/useful links shown in the footer", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />", "TitleEn");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='URL' Name='URL' Format='Hyperlink' />", "URL");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='URLEn' Name='URLEn' Format='Hyperlink' />", "URLEn");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='OpenInNewTab' Name='OpenInNewTab'><Default>1</Default></Field>", "OpenInNewTab");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>", "Visibility");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />", "ItemOrder");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                AddLinkRow(list, "وزارة التعليم", "Ministry of Education", "https://www.moe.gov.sa/ar/Pages/default.aspx", "https://www.moe.gov.sa/en/Pages/default.aspx", 1);
                AddLinkRow(list, "المنصة الوطنية", "National Government Portal", "https://my.gov.sa/ar", "https://my.gov.sa/en", 2);
                AddLinkRow(list, "البوابة الوطنية للبيانات المفتوحة", "National Open Data Portal", "https://data.gov.sa/", "https://data.gov.sa/en/", 3);
                AddLinkRow(list, "منصة الاستشارات (استطلاع)", "Public Consultation (Istitlaa)", "https://istitlaa.ncc.gov.sa/", "https://istitlaa.ncc.gov.sa/en/", 4);
                AddLinkRow(list, "المنصة الوطنية للتوظيف (جدارات)", "National Employment Portal (Jadarat)", "https://jadarat.sa/", "https://jadarat.sa/en/", 5);
            }
        }

        private static void AddLinkRow(SPList list, string titleAr, string titleEn, string urlAr, string urlEn, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = titleAr;
            row["TitleEn"] = titleEn;
            row["URL"] = new SPFieldUrlValue { Url = urlAr, Description = titleAr };
            row["URLEn"] = new SPFieldUrlValue { Url = urlEn, Description = titleEn };
            row["OpenInNewTab"] = true;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  6. Contact Info
        // ------------------------------------------------------------------

        private static void EnsureContactUsFooter(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_CONTACT);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_CONTACT, "Contact entries shown in the footer (phone, email)", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list,
                "<Field Type='Choice' DisplayName='ContactType' Name='ContactType'>" +
                "<CHOICES><CHOICE>Phone</CHOICE><CHOICE>Email</CHOICE><CHOICE>Text</CHOICE></CHOICES>" +
                "<Default>Text</Default></Field>", "ContactType");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='LabelAr' Name='LabelAr' />", "LabelAr");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='LabelEn' Name='LabelEn' />", "LabelEn");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='Value' Name='Value' />", "Value");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>", "Visibility");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />", "ItemOrder");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                // Unified phone
                SPListItem r1 = list.Items.Add();
                r1["Title"] = "Phone";
                r1["ContactType"] = "Phone";
                r1["LabelAr"] = "920011114";
                r1["LabelEn"] = "920011114";
                r1["Value"] = "920011114";
                r1["Visibility"] = true;
                r1["ItemOrder"] = 1;
                r1.Update();

                // International phone
                SPListItem rPhoneIntl = list.Items.Add();
                rPhoneIntl["Title"] = "PhoneIntl";
                rPhoneIntl["ContactType"] = "Phone";
                rPhoneIntl["LabelAr"] = "00966-11-8220011";
                rPhoneIntl["LabelEn"] = "00966-11-8220011";
                rPhoneIntl["Value"] = "+966118220011";
                rPhoneIntl["Visibility"] = true;
                rPhoneIntl["ItemOrder"] = 2;
                rPhoneIntl.Update();

                // Email
                SPListItem r2 = list.Items.Add();
                r2["Title"] = "Email";
                r2["ContactType"] = "Email";
                r2["LabelAr"] = "info@pnu.edu.sa";
                r2["LabelEn"] = "info@pnu.edu.sa";
                r2["Value"] = "info@pnu.edu.sa";
                r2["Visibility"] = true;
                r2["ItemOrder"] = 3;
                r2.Update();
            }
        }

        // ------------------------------------------------------------------
        //  7. Follow Us (Social Media)
        // ------------------------------------------------------------------

        private static void EnsureFollowUsFooter(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_FOLLOWUS);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_FOLLOWUS, "Social media icons shown in the footer", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list, "<Field Type='Text' DisplayName='IconClass' Name='IconClass' />", "IconClass");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='URL' Name='URL' Format='Hyperlink' />", "URL");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='AriaLabelAr' Name='AriaLabelAr' />", "AriaLabelAr");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='AriaLabelEn' Name='AriaLabelEn' />", "AriaLabelEn");
            changed |= EnsureField(list, "<Field Type='Text' DisplayName='ParentKey' Name='ParentKey' />", "ParentKey");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>", "Visibility");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />", "ItemOrder");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                AddSocialRow(list, "Facebook", "hgi hgi-stroke hgi-facebook-02", "https://www.facebook.com/PNUKSA0/", "حساب الجامعة في منصة فيسبوك", "PNU on Facebook", "", 1);
                AddSocialRow(list, "Twitter", "hgi hgi-stroke hgi-new-twitter", "", "حسابات الجامعة في منصة اكس", "PNU on X", "", 2);
                AddSocialRow(list, "Twitter_PNU_KSA", "", "https://twitter.com/_PNU_KSA", "جامعة الأميرة نورة", "PNU KSA", "Twitter", 3);
                AddSocialRow(list, "Twitter_Live", "", "https://twitter.com/pnu_live", "مباشر PNU", "PNU Live", "Twitter", 4);
                AddSocialRow(list, "Twitter_Events", "", "https://twitter.com/pnu_events", "فعاليات PNU", "PNU Events", "Twitter", 5);
                AddSocialRow(list, "LinkedIn", "hgi hgi-stroke hgi-linkedin-02", "https://www.linkedin.com/school/princess-nourah-bint-abdulrahman-university/", "حساب الجامعة في منصة لينكدان", "PNU on LinkedIn", "", 6);
                AddSocialRow(list, "Snapchat", "hgi hgi-stroke hgi-snapchat", "https://www.snapchat.com/add/pnu_ksa", "حساب الجامعة في منصة سناب شات", "PNU on Snapchat", "", 7);
                AddSocialRow(list, "TikTok", "hgi hgi-stroke hgi-tiktok", "https://www.tiktok.com/@_pnu_ksa", "حساب الجامعة في منصة تيك توك", "PNU on TikTok", "", 8);
                AddSocialRow(list, "Medium", "hgi hgi-stroke hgi-medium", "https://medium.com/@pnu-ksa", "حساب الجامعة في منصة ميديم", "PNU on Medium", "", 9);
                AddSocialRow(list, "Instagram", "hgi hgi-stroke hgi-instagram", "https://www.instagram.com/_PNU_KSA/", "حساب الجامعة في منصة انستاقرام", "PNU on Instagram", "", 10);
                AddSocialRow(list, "YouTube", "hgi hgi-stroke hgi-youtube", "https://www.youtube.com/channel/UC-0BTuZ46ApPXt6dWONTyGg", "حساب الجامعة في منصة يوتيوب", "PNU on YouTube", "", 11);
                AddSocialRow(list, "WhatsApp", "hgi hgi-stroke hgi-whatsapp", "https://whatsapp.com/channel/0029VabX2wDKGGGN07X1c80f", "قناة الجامعة في واتساب", "PNU on WhatsApp", "", 12);
            }
        }

        private static void AddSocialRow(SPList list, string title, string iconClass, string url, string ariaAr, string ariaEn, string parentKey, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = title;
            row["IconClass"] = iconClass;
            if (!string.IsNullOrEmpty(url))
            {
                row["URL"] = new SPFieldUrlValue { Url = url, Description = title };
            }
            row["AriaLabelAr"] = ariaAr;
            row["AriaLabelEn"] = ariaEn;
            row["ParentKey"] = parentKey;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  8. Bottom-Bar Links
        // ------------------------------------------------------------------

        private static void EnsureDownLinksFooter(SPWeb web)
        {
            bool created = false, changed = false;
            SPList list = web.Lists.TryGetList(LIST_DOWNLINKS);
            if (list == null)
            {
                Guid id = web.Lists.Add(LIST_DOWNLINKS, "Bottom-bar links shown in the footer (policies, sitemap, etc.)", SPListTemplateType.GenericList);
                list = web.Lists[id];
                created = true;
            }

            changed |= EnsureField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />", "TitleEn");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='URLAr' Name='URLAr' Format='Hyperlink' />", "URLAr");
            changed |= EnsureField(list, "<Field Type='URL' DisplayName='URLEn' Name='URLEn' Format='Hyperlink' />", "URLEn");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='OpenInNewTab' Name='OpenInNewTab'><Default>0</Default></Field>", "OpenInNewTab");
            changed |= EnsureField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>", "Visibility");
            changed |= EnsureField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />", "ItemOrder");

            changed |= HideFromUI(list);
            if (changed || created) list.Update();

            // Seed only a list created just now: an admin who empties a list keeps it empty
            if (created)
            {
                AddDownLinkRow(list, "سياسات الجامعة", "Policies", "/ar/SECURITYCYBER/Pages/Privacy-Policy.aspx", "/en/SECURITYCYBER/Pages/Privacy-Policy.aspx", false, 1);
                AddDownLinkRow(list, "خريطة الموقع", "Site Map", "/ar/Pages/SiteMap.aspx", "/en/Pages/SiteMap.aspx", false, 2);
                AddDownLinkRow(list, "المشاركة الإلكترونية", "Electronic Share", "https://eparticipation.my.gov.sa/", "https://eparticipation.my.gov.sa/", true, 3);
                AddDownLinkRow(list, "البيانات المفتوحة", "Open Data", "/ar/AboutUniversity/Pages/PNUinNumbers.aspx", "/en/AboutUniversity/Pages/PNUinNumbers.aspx", false, 4);
            }
        }

        private static void AddDownLinkRow(SPList list, string titleAr, string titleEn, string urlAr, string urlEn, bool openInNewTab, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = titleAr;
            row["TitleEn"] = titleEn;
            row["URLAr"] = new SPFieldUrlValue { Url = urlAr, Description = titleAr };
            row["URLEn"] = new SPFieldUrlValue { Url = urlEn, Description = titleEn };
            row["OpenInNewTab"] = openInNewTab;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  Utilities
        // ------------------------------------------------------------------

        public static bool EnsureField(SPList list, string fieldXml, string internalName)
        {
            try
            {
                if (!list.Fields.ContainsField(internalName))
                {
                    list.Fields.AddFieldAsXml(fieldXml, true, SPAddFieldOptions.AddFieldInternalNameHint);
                    return true;
                }
            }
            catch { /* field may already exist */ }
            return false;
        }

        /// <summary>Applies the list display settings; returns true only when something changed.</summary>
        private static bool HideFromUI(SPList list)
        {
            try
            {
                if (list.Hidden || list.OnQuickLaunch || !list.NoCrawl)
                {
                    list.Hidden = false;
                    list.OnQuickLaunch = false;
                    list.NoCrawl = true;
                    return true;
                }
            }
            catch { /* cosmetic */ }
            return false;
        }
    }

    /// <summary>
    /// Shared cache key for the footer data (filled by ucHomeFooter). Admin saves and forced
    /// provisioning call Invalidate() so changes show immediately on this server; other
    /// front-ends pick them up when their cache entry expires.
    /// </summary>
    public static class FooterCache
    {
        public const string KeyPrefix = "PNU.HomeFooter.Data.";

        public static string CurrentKey()
        {
            return SPContext.Current != null ? KeyPrefix + SPContext.Current.Site.ID : null;
        }

        public static void Invalidate()
        {
            try
            {
                string key = CurrentKey();
                if (key != null) HttpRuntime.Cache.Remove(key);
            }
            catch { }
        }
    }
}
