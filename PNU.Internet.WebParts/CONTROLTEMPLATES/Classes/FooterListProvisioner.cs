using Microsoft.SharePoint;
using Portal.Main.Helper.Utils;
using System;
using System.Web;

namespace PNU.Internet.WebParts.Helpers
{
    /// <summary>
    /// Provisions the 5 footer lists at the root web of the site collection if they do not already exist.
    /// Call <see cref="EnsureFooterLists"/> once from Page_Load; it is guarded so it runs at most once
    /// per AppDomain and is safe against concurrent first-hit requests.
    /// </summary>
    public static class FooterListProvisioner
    {
        // List internal names (Title fields may be renamed later; we look them up by URL-safe name).
        public const string LIST_ABOUT = "AboutFooter";
        public const string LIST_USEFULLINKS = "UsefulLinksUsFooter";
        public const string LIST_CONTACT = "ContactUsFooter";
        public const string LIST_FOLLOWUS = "FollowUsUsFooter";
        public const string LIST_DOWNLINKS = "DownLinksFooter";

        private static volatile bool _alreadyChecked = false;
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// Ensures all 5 footer lists exist at the root web. Safe to call on every Page_Load:
        /// the work runs only once per AppDomain, and subsequent calls are a fast boolean check.
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
                        // Always provision at the ROOT web of the site collection.
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb rootWeb = site.RootWeb)
                            {
                                rootWeb.AllowUnsafeUpdates = true;
                                try
                                {
                                    EnsureAboutFooter(rootWeb);
                                    EnsureUsefulLinksFooter(rootWeb);
                                    EnsureContactUsFooter(rootWeb);
                                    EnsureFollowUsFooter(rootWeb);
                                    EnsureDownLinksFooter(rootWeb);
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
                    // Don't flip the flag on failure — allow a retry on the next page load.
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
        //  Individual list provisioners
        // ------------------------------------------------------------------

        private static void EnsureAboutFooter(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_ABOUT) != null) return;

            Guid id = web.Lists.Add(LIST_ABOUT, "About section (description) shown in the footer", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list, "<Field Type='Note' DisplayName='DescriptionAr' Name='DescriptionAr' NumLines='6' RichText='FALSE' />");
            AddField(list, "<Field Type='Note' DisplayName='DescriptionEn' Name='DescriptionEn' NumLines='6' RichText='FALSE' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0'><Default>1</Default></Field>");

            HideFromUI(list);
            list.Update();

            // Seed a single default row
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

        private static void EnsureUsefulLinksFooter(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_USEFULLINKS) != null) return;

            Guid id = web.Lists.Add(LIST_USEFULLINKS, "Related/useful links shown in the footer", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />");
            AddField(list, "<Field Type='URL' DisplayName='URL' Name='URL' Format='Hyperlink' />");
            AddField(list, "<Field Type='Boolean' DisplayName='OpenInNewTab' Name='OpenInNewTab'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            // Seed default rows
            AddLinkRow(list, "وزارة التعليم", "Ministry of Education", "https://www.moe.gov.sa/ar/Pages/default.aspx", 1);
            AddLinkRow(list, "المنصة الوطنية", "National Government Portal", "https://my.gov.sa/ar", 2);
            AddLinkRow(list, "البوابة الوطنية للبيانات المفتوحة", "National Open Data Portal", "https://data.gov.sa/", 3);
            AddLinkRow(list, "منصة الاستشارات (استطلاع)", "Public Consultation (Istitlaa)", "https://istitlaa.ncc.gov.sa/", 4);
            AddLinkRow(list, "المنصة الوطنية للتوظيف (جدارات)", "National Employment Portal (Jadarat)", "https://jadarat.sa/", 5);
        }

        private static void AddLinkRow(SPList list, string titleAr, string titleEn, string url, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = titleAr;
            row["TitleEn"] = titleEn;
            row["URL"] = new SPFieldUrlValue { Url = url, Description = titleAr };
            row["OpenInNewTab"] = true;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        private static void EnsureContactUsFooter(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_CONTACT) != null) return;

            Guid id = web.Lists.Add(LIST_CONTACT, "Contact entries shown in the footer (phone, email, trademark text)", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list,
                "<Field Type='Choice' DisplayName='ContactType' Name='ContactType'>" +
                "<CHOICES><CHOICE>Phone</CHOICE><CHOICE>Email</CHOICE><CHOICE>Text</CHOICE></CHOICES>" +
                "<Default>Text</Default></Field>");
            AddField(list, "<Field Type='Text' DisplayName='LabelAr' Name='LabelAr' />");
            AddField(list, "<Field Type='Text' DisplayName='LabelEn' Name='LabelEn' />");
            AddField(list, "<Field Type='Text' DisplayName='Value' Name='Value' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            // Seed: phone + email (trademark block removed per new design)
            SPListItem r1 = list.Items.Add();
            r1["Title"] = "Phone";
            r1["ContactType"] = "Phone";
            // Use the Saudi unified number (9200 11114) as shown in the new DGA design.
            // NOTE: the string contains intentional zero-width spaces to match the design's formatting.
            r1["LabelAr"] = "9200​11114​";
            r1["LabelEn"] = "9200​11114​";
            r1["Value"] = "920011114";
            r1["Visibility"] = true;
            r1["ItemOrder"] = 1;
            r1.Update();

            SPListItem r2 = list.Items.Add();
            r2["Title"] = "Email";
            r2["ContactType"] = "Email";
            r2["LabelAr"] = "info@pnu.edu.sa";
            r2["LabelEn"] = "info@pnu.edu.sa";
            r2["Value"] = "info@pnu.edu.sa";
            r2["Visibility"] = true;
            r2["ItemOrder"] = 2;
            r2.Update();
        }

        private static void EnsureFollowUsFooter(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_FOLLOWUS) != null) return;

            Guid id = web.Lists.Add(LIST_FOLLOWUS, "Social media icons shown in the footer", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list, "<Field Type='Text' DisplayName='IconClass' Name='IconClass' />");
            AddField(list, "<Field Type='URL' DisplayName='URL' Name='URL' Format='Hyperlink' />");
            AddField(list, "<Field Type='Text' DisplayName='AriaLabelAr' Name='AriaLabelAr' />");
            AddField(list, "<Field Type='Text' DisplayName='AriaLabelEn' Name='AriaLabelEn' />");
            // ParentKey: leave empty for top-level icons; set to the Title of the parent row
            // for dropdown child items (e.g. the three Twitter sub-accounts all have ParentKey="Twitter").
            AddField(list, "<Field Type='Text' DisplayName='ParentKey' Name='ParentKey' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            // Seed: top-level icons in the order shown in the design.
            // Twitter is a dropdown PARENT (no URL); its three sub-accounts are child rows with ParentKey="Twitter".
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

        private static void EnsureDownLinksFooter(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_DOWNLINKS) != null) return;

            Guid id = web.Lists.Add(LIST_DOWNLINKS, "Bottom-bar links shown in the footer (policies, sitemap, etc.)", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />");
            AddField(list, "<Field Type='URL' DisplayName='URLAr' Name='URLAr' Format='Hyperlink' />");
            AddField(list, "<Field Type='URL' DisplayName='URLEn' Name='URLEn' Format='Hyperlink' />");
            AddField(list, "<Field Type='Boolean' DisplayName='OpenInNewTab' Name='OpenInNewTab'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            AddDownLinkRow(list, "سياسات الجامعة", "Policies", "/ar/SECURITYCYBER/Pages/Privacy-Policy.aspx", "/en/SECURITYCYBER/Pages/Privacy-Policy.aspx", 1);
            AddDownLinkRow(list, "خريطة الموقع", "Site Map", "/ar/Pages/SiteMap.aspx", "/en/Pages/SiteMap.aspx", 2);
            AddDownLinkRow(list, "المشاركة الإلكترونية", "Electronic Share", "https://eparticipation.my.gov.sa/", "https://eparticipation.my.gov.sa/", 3);
            AddDownLinkRow(list, "البيانات المفتوحة", "Open Data", "/ar/AboutUniversity/Pages/PNUinNumbers.aspx", "/en/AboutUniversity/Pages/PNUinNumbers.aspx", 4);
        }

        private static void AddDownLinkRow(SPList list, string titleAr, string titleEn, string urlAr, string urlEn, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = titleAr;
            row["TitleEn"] = titleEn;
            row["URLAr"] = new SPFieldUrlValue { Url = urlAr, Description = titleAr };
            row["URLEn"] = new SPFieldUrlValue { Url = urlEn, Description = titleEn };
            row["OpenInNewTab"] = true;
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
        /// Hides the list from Quick Launch and the All Site Content page, and disables search crawling.
        /// Keeps the site clean since these lists are data-only and not for direct user browsing.
        /// </summary>
        private static void HideFromUI(SPList list)
        {
            try
            {
                list.Hidden = false;          // leave visible so admins can find it in Site Contents
                list.OnQuickLaunch = false;   // but keep it off the quick launch bar
                list.NoCrawl = true;
            }
            catch { /* cosmetic - ignore failures */ }
        }
    }
}
