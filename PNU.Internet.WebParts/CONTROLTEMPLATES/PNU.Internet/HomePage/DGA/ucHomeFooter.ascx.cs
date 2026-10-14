using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.Helpers;
using Portal.Main.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeFooter : UserControl
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan FailureCacheDuration = TimeSpan.FromMinutes(1);

        // ------------------------------------------------------------------
        //  Properties consumed by the .ascx
        // ------------------------------------------------------------------

        public bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public string OverviewTitle { get; set; }
        public string OpportunitiesTitle { get; set; }
        public string UniversityLifeTitle { get; set; }
        public string RelatedLinksTitle { get; set; }

        protected string ContactTitle
        {
            get { return IsArabic ? "تواصل معنا" : "Contact Us"; }
        }

        protected string FollowTitle
        {
            get { return IsArabic ? "تابعنا على" : "Follow Us"; }
        }

        public string CopyrightText { get; set; }
        public string DevelopedByText { get; set; }

        public string SdaiaUrl { get; set; }
        public string VisionUrl { get; set; }
        public string PnuHomeUrl { get; set; }

        protected string Vision2030Alt
        {
            get { return IsArabic ? "شعار رؤية السعودية 2030" : "Saudi Vision 2030 logo"; }
        }

        protected string RaqmiAlt
        {
            get { return IsArabic ? "شعار ترخيص منصة رقمي" : "Raqmi platform license logo"; }
        }

        // Collections for this request (taken from the cached, bilingual footer data)
        private List<FooterLink> _overviewLinks = new List<FooterLink>();
        private List<FooterLink> _opportunitiesLinks = new List<FooterLink>();
        private List<FooterLink> _universityLifeLinks = new List<FooterLink>();
        private List<FooterLink> _usefulLinks = new List<FooterLink>();
        private List<FooterContact> _contacts = new List<FooterContact>();
        private List<FooterSocial> _socialLinks = new List<FooterSocial>();
        private List<FooterDownLink> _downLinks = new List<FooterDownLink>();

        // ------------------------------------------------------------------
        //  Page lifecycle
        // ------------------------------------------------------------------

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string path = Request.Url.AbsolutePath;

                bool isHome =
                    path.EndsWith("/ar/Pages/Home.aspx", StringComparison.OrdinalIgnoreCase) ||
                    path.EndsWith("/en/Pages/Home.aspx", StringComparison.OrdinalIgnoreCase);

                if (ucSurvey != null)
                {
                    ucSurvey.Visible = !isHome;
                }

                InitDefaults();

                // Guarded once per AppDomain inside the provisioner
                if (!Page.IsPostBack)
                {
                    FooterListProvisioner.EnsureFooterLists();
                }

                // Load and bind on every request (from cache) so postbacks keep the list values
                ApplyFooterData(GetFooterData());
                BindContactPanel();
                BindRepeaters();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void InitDefaults()
        {
            OverviewTitle = IsArabic ? "نظرة عامة" : "Overview";
            OpportunitiesTitle = IsArabic ? "فرصك في نورة" : "Your opportunities in Noura";
            UniversityLifeTitle = IsArabic ? "الحياة الجامعية" : "University life";
            RelatedLinksTitle = IsArabic ? "مواقع ذات صلة" : "Related Links";

            CopyrightText = IsArabic
                ? "جميع الحقوق محفوظة 2026 | جامعة الأميرة نورة بنت عبدالرحمن"
                : "All Rights Reserved 2026 | Princess Nourah bint Abdulrahman University";

            DevelopedByText = IsArabic
                ? " - تم تطويره وصيانته بواسطة إدارة التحول الرقمي"
                : " - Developed and maintained by the Digital Transformation Department";

            SdaiaUrl = IsArabic ? "https://sdaia.gov.sa/ar/default.aspx" : "https://sdaia.gov.sa/en/default.aspx";
            VisionUrl = IsArabic ? "https://www.vision2030.gov.sa/" : "https://www.vision2030.gov.sa/en/";
            PnuHomeUrl = IsArabic ? "/ar" : "/en";
        }

        // ------------------------------------------------------------------
        //  Cached data (bilingual; the language is applied per request)
        // ------------------------------------------------------------------

        private class FooterData
        {
            public bool Loaded;
            public bool NavLinksListExists;
            public readonly Dictionary<string, Pair> SectionTitles = new Dictionary<string, Pair>(StringComparer.OrdinalIgnoreCase);
            public readonly List<FooterLink> Overview = new List<FooterLink>();
            public readonly List<FooterLink> Opportunities = new List<FooterLink>();
            public readonly List<FooterLink> UniversityLife = new List<FooterLink>();
            public readonly List<FooterLink> Related = new List<FooterLink>();
            public readonly List<FooterContact> Contacts = new List<FooterContact>();
            public readonly List<FooterSocial> Social = new List<FooterSocial>();
            public readonly List<FooterDownLink> DownLinks = new List<FooterDownLink>();
            public Pair Copyright, DevelopedBy, Sdaia, Vision, PnuHome;
        }

        private class Pair
        {
            public string Ar;
            public string En;
            public Pair(string ar, string en) { Ar = ar ?? ""; En = en ?? ""; }
            public bool IsEmpty { get { return string.IsNullOrEmpty(Ar) && string.IsNullOrEmpty(En); } }
        }

        private static FooterData GetFooterData()
        {
            string key = FooterCache.CurrentKey();
            if (key == null) return new FooterData();

            FooterData cached = HttpRuntime.Cache[key] as FooterData;
            if (cached != null) return cached;

            FooterData data = LoadFooterData(SPContext.Current.Site.ID);
            HttpRuntime.Cache.Insert(key, data, null,
                DateTime.UtcNow.Add(data.Loaded ? CacheDuration : FailureCacheDuration),
                Cache.NoSlidingExpiration);
            return data;
        }

        /// <summary>
        /// Reads every footer list once (elevated: the data is public and shared by all visitors
        /// through the cache, so it must not depend on the first visitor's permissions).
        /// </summary>
        private static FooterData LoadFooterData(Guid siteId)
        {
            FooterData data = new FooterData();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        SPWeb rootWeb = site.RootWeb; // owned by SPSite: do not dispose
                        LoadSections(rootWeb, data);
                        LoadNavLinks(rootWeb, data);
                        LoadContacts(rootWeb, data);
                        LoadSocial(rootWeb, data);
                        LoadDownLinks(rootWeb, data);
                        LoadSettings(rootWeb, data);
                        data.Loaded = true;
                    }
                });
            }
            catch (Exception ex)
            {
                data.Loaded = false;
                Publics.WriteToLog(HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "ucHomeFooter",
                    "ucHomeFooter.LoadFooterData", ex.Message);
            }
            return data;
        }

        private static SPQuery VisibleOrderedQuery()
        {
            return new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };
        }

        private static void LoadSections(SPWeb web, FooterData data)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_NAVSECTIONS);
            if (list == null) return;

            foreach (SPListItem item in list.GetItems(VisibleOrderedQuery()))
            {
                string key = SafeString(item["Title"]);
                if (!string.IsNullOrEmpty(key) && !data.SectionTitles.ContainsKey(key))
                    data.SectionTitles[key] = new Pair(SafeString(item["TitleAr"]), SafeString(item["TitleEn"]));
            }
        }

        private static void LoadNavLinks(SPWeb web, FooterData data)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_NAVLINKS);
            data.NavLinksListExists = list != null;

            if (list == null)
            {
                // Older sites without FooterNavLinks: related links come from the legacy list
                LoadLegacyUsefulLinks(web, data);
                return;
            }

            foreach (SPListItem item in list.GetItems(VisibleOrderedQuery()))
            {
                SPFieldUrlValue urlAr = new SPFieldUrlValue(SafeString(item["URLAr"]));
                SPFieldUrlValue urlEn = new SPFieldUrlValue(SafeString(item["URLEn"]));
                string secKey = SafeString(item["SectionKey"]);

                FooterLink link = new FooterLink
                {
                    TitleAr = SafeString(item["Title"]),
                    TitleEn = SafeString(item["TitleEn"]),
                    UrlAr = string.IsNullOrEmpty(urlAr.Url) ? "#" : urlAr.Url,
                    UrlEn = string.IsNullOrEmpty(urlEn.Url) ? "#" : urlEn.Url,
                    OpenInNewTab = SafeBool(item["OpenInNewTab"]),
                    SectionKey = secKey
                };

                if (secKey.Equals("Overview", StringComparison.OrdinalIgnoreCase))
                    data.Overview.Add(link);
                else if (secKey.Equals("Opportunities", StringComparison.OrdinalIgnoreCase))
                    data.Opportunities.Add(link);
                else if (secKey.Equals("UniversityLife", StringComparison.OrdinalIgnoreCase))
                    data.UniversityLife.Add(link);
                else if (secKey.Equals("RelatedLinks", StringComparison.OrdinalIgnoreCase))
                    data.Related.Add(link);
            }
        }

        private static void LoadLegacyUsefulLinks(SPWeb web, FooterData data)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_USEFULLINKS);
            if (list == null) return;

            foreach (SPListItem item in list.GetItems(VisibleOrderedQuery()))
            {
                SPFieldUrlValue url = new SPFieldUrlValue(SafeString(item["URL"]));
                SPFieldUrlValue urlEn = new SPFieldUrlValue(SafeString(item["URLEn"]));

                string uAr = string.IsNullOrEmpty(url.Url) ? "#" : url.Url;
                string uEn = string.IsNullOrEmpty(urlEn.Url) ? uAr : urlEn.Url;

                data.Related.Add(new FooterLink
                {
                    TitleAr = SafeString(item["Title"]),
                    TitleEn = SafeString(item["TitleEn"]),
                    UrlAr = uAr,
                    UrlEn = uEn,
                    Url = uAr,
                    OpenInNewTab = SafeBool(item["OpenInNewTab"]),
                    SectionKey = "RelatedLinks"
                });
            }
        }

        private static void LoadContacts(SPWeb web, FooterData data)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_CONTACT);
            if (list == null) return;

            foreach (SPListItem item in list.GetItems(VisibleOrderedQuery()))
            {
                data.Contacts.Add(new FooterContact
                {
                    Key = SafeString(item["Title"]),
                    ContactType = SafeString(item["ContactType"]),
                    LabelAr = SafeString(item["LabelAr"]),
                    LabelEn = SafeString(item["LabelEn"]),
                    Value = SafeString(item["Value"])
                });
            }
        }

        private static void LoadSocial(SPWeb web, FooterData data)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_FOLLOWUS);
            if (list == null) return;

            List<FooterSocial> all = new List<FooterSocial>();
            foreach (SPListItem item in list.GetItems(VisibleOrderedQuery()))
            {
                SPFieldUrlValue url = new SPFieldUrlValue(SafeString(item["URL"]));
                all.Add(new FooterSocial
                {
                    Key = SafeString(item["Title"]),
                    IconClass = SafeString(item["IconClass"]),
                    Url = string.IsNullOrEmpty(url.Url) ? "" : url.Url,
                    AriaLabelAr = SafeString(item["AriaLabelAr"]),
                    AriaLabelEn = SafeString(item["AriaLabelEn"]),
                    ParentKey = SafeString(item["ParentKey"]),
                    Children = new List<FooterSocial>()
                });
            }

            foreach (FooterSocial row in all)
            {
                if (string.IsNullOrEmpty(row.ParentKey))
                {
                    data.Social.Add(row);
                    continue;
                }

                foreach (FooterSocial p in all)
                {
                    if (p.Key == row.ParentKey && string.IsNullOrEmpty(p.ParentKey))
                    {
                        p.Children.Add(row);
                        break;
                    }
                }
            }
        }

        private static void LoadDownLinks(SPWeb web, FooterData data)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_DOWNLINKS);
            if (list == null) return;

            foreach (SPListItem item in list.GetItems(VisibleOrderedQuery()))
            {
                SPFieldUrlValue urlAr = new SPFieldUrlValue(SafeString(item["URLAr"]));
                SPFieldUrlValue urlEn = new SPFieldUrlValue(SafeString(item["URLEn"]));
                data.DownLinks.Add(new FooterDownLink
                {
                    TitleAr = SafeString(item["Title"]),
                    TitleEn = SafeString(item["TitleEn"]),
                    UrlAr = string.IsNullOrEmpty(urlAr.Url) ? "#" : urlAr.Url,
                    UrlEn = string.IsNullOrEmpty(urlEn.Url) ? "#" : urlEn.Url,
                    OpenInNewTab = SafeBool(item["OpenInNewTab"])
                });
            }
        }

        private static void LoadSettings(SPWeb web, FooterData data)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_SETTINGS);
            if (list == null || list.ItemCount == 0) return;

            SPQuery q = new SPQuery { RowLimit = 1 };
            foreach (SPListItem item in list.GetItems(q))
            {
                data.Copyright = new Pair(SafeString(item["CopyrightAr"]), SafeString(item["CopyrightEn"]));
                data.DevelopedBy = new Pair(SafeString(item["DevelopedByAr"]), SafeString(item["DevelopedByEn"]));
                data.Sdaia = UrlPair(item, "SdaiaUrlAr", "SdaiaUrlEn");
                data.Vision = UrlPair(item, "VisionUrlAr", "VisionUrlEn");
                data.PnuHome = UrlPair(item, "PnuHomeUrlAr", "PnuHomeUrlEn");
                break;
            }
        }

        private static Pair UrlPair(SPListItem item, string arField, string enField)
        {
            return new Pair(new SPFieldUrlValue(SafeString(item[arField])).Url,
                            new SPFieldUrlValue(SafeString(item[enField])).Url);
        }

        // ------------------------------------------------------------------
        //  Per-request: apply the language and fall back to defaults
        // ------------------------------------------------------------------

        private void ApplyFooterData(FooterData data)
        {
            OverviewTitle = Pick(SectionTitle(data, "Overview"), OverviewTitle);
            OpportunitiesTitle = Pick(SectionTitle(data, "Opportunities"), OpportunitiesTitle);
            UniversityLifeTitle = Pick(SectionTitle(data, "UniversityLife"), UniversityLifeTitle);
            RelatedLinksTitle = Pick(SectionTitle(data, "RelatedLinks"), RelatedLinksTitle);

            CopyrightText = Pick(data.Copyright, CopyrightText);
            DevelopedByText = Pick(data.DevelopedBy, DevelopedByText);
            SdaiaUrl = Pick(data.Sdaia, SdaiaUrl);
            VisionUrl = Pick(data.Vision, VisionUrl);
            PnuHomeUrl = Pick(data.PnuHome, PnuHomeUrl);

            // Built-in links only when the links list itself is missing/unreadable. A list the
            // admin emptied (or whose links are all hidden) stays empty on the page.
            if (data.Loaded && data.NavLinksListExists)
            {
                _overviewLinks = data.Overview;
                _opportunitiesLinks = data.Opportunities;
                _universityLifeLinks = data.UniversityLife;
            }
            else
            {
                PopulateDefaultNavLinks();
            }

            _usefulLinks = data.Related;
            _contacts = data.Contacts;
            _socialLinks = data.Social;
            _downLinks = data.DownLinks;
        }

        private static Pair SectionTitle(FooterData data, string key)
        {
            Pair p;
            return data.SectionTitles.TryGetValue(key, out p) ? p : null;
        }

        /// <summary>Picks the current language's value, else keeps the default (other language is not mixed in).</summary>
        private string Pick(Pair value, string fallback)
        {
            if (value == null) return fallback;
            string v = IsArabic ? value.Ar : value.En;
            return !string.IsNullOrEmpty(v) ? v : fallback;
        }

        private void PopulateDefaultNavLinks()
        {
            _overviewLinks = new List<FooterLink>();
            _opportunitiesLinks = new List<FooterLink>();
            _universityLifeLinks = new List<FooterLink>();

            // Overview defaults
            _overviewLinks.Add(new FooterLink { TitleAr = "الأسئلة الشائعة", TitleEn = "FAQs", UrlAr = "/ar/FAQs/Pages/default.aspx", UrlEn = "/en/FAQs/Pages/default.aspx", OpenInNewTab = false });
            _overviewLinks.Add(new FooterLink { TitleAr = "تيك كير لتقنية المعلومات", TitleEn = "Tech Care for Information Technology", UrlAr = "https://techcare.pnu.edu.sa/", UrlEn = "https://techcare.pnu.edu.sa/en", OpenInNewTab = true });
            _overviewLinks.Add(new FooterLink { TitleAr = "المستشفى الجامعي", TitleEn = "KAAUH Hospital", UrlAr = "https://www.kaauh.edu.sa/ar", UrlEn = "https://www.kaauh.edu.sa/en", OpenInNewTab = true });
            _overviewLinks.Add(new FooterLink { TitleAr = "تواصل نورة", TitleEn = "Tawasul with Nourah", UrlAr = "https://tawasulnourah.pnu.edu.sa/", UrlEn = "https://tawasulnourah.pnu.edu.sa/en", OpenInNewTab = true });
            _overviewLinks.Add(new FooterLink { TitleAr = "بوابة الخدمات الإلكترونية", TitleEn = "e-services portal", UrlAr = "https://eservice.pnu.edu.sa/", UrlEn = "https://eservice.pnu.edu.sa/en", OpenInNewTab = true });
            _overviewLinks.Add(new FooterLink { TitleAr = "المساعدة والدعم", TitleEn = "Help and Support", UrlAr = "/ar/AboutUniversity/Pages/Support.aspx", UrlEn = "/en/AboutUniversity/Pages/Support.aspx", OpenInNewTab = true });
            _overviewLinks.Add(new FooterLink { TitleAr = "تواصل معنا", TitleEn = "Contact Us", UrlAr = "/ar/Pages/ContactUsForm.aspx", UrlEn = "/en/Pages/ContactUsForm.aspx", OpenInNewTab = true });

            // Opportunities defaults
            _opportunitiesLinks.Add(new FooterLink { TitleAr = "التوظيف", TitleEn = "Recruitment", UrlAr = "/ar/AboutUniversity/Pages/Careers.aspx", UrlEn = "/en/AboutUniversity/Pages/Careers.aspx", OpenInNewTab = false });
            _opportunitiesLinks.Add(new FooterLink { TitleAr = "استثمر في نورة", TitleEn = "Invest in Noura", UrlAr = "/ar/NourahOpportunities/Pages/Invest-in-noura.aspx", UrlEn = "/en/NourahOpportunities/Pages/Invest-in-noura.aspx", OpenInNewTab = false });
            _opportunitiesLinks.Add(new FooterLink { TitleAr = "التدريب", TitleEn = "Training", UrlAr = "/ar/NourahOpportunities/Pages/التدريب.aspx", UrlEn = "/en/NourahOpportunities/Pages/Training.aspx", OpenInNewTab = false });
            _opportunitiesLinks.Add(new FooterLink { TitleAr = "التطوع", TitleEn = "Volunteering", UrlAr = "https://norahattaa.pnu.edu.sa/ar/Pages/default.aspx", UrlEn = "https://norahattaa.pnu.edu.sa/en/Pages/default.aspx", OpenInNewTab = true });

            // University Life defaults
            _universityLifeLinks.Add(new FooterLink { TitleAr = "جولة افتراضية 360", TitleEn = "360 virtual tour", UrlAr = "/ar/UniversityLife/Pages/VirtualTour.aspx", UrlEn = "/en/UniversityLife/Pages/VirtualTour.aspx", OpenInNewTab = false });
            _universityLifeLinks.Add(new FooterLink { TitleAr = "مرافق الجامعة", TitleEn = "University Facilities", UrlAr = "/ar/UniversityLife/Pages/UniversityFacilities.aspx", UrlEn = "/en/UniversityLife/Pages/UniversityFacilities.aspx", OpenInNewTab = false });
        }

        // ------------------------------------------------------------------
        //  Bindings
        // ------------------------------------------------------------------

        private void BindContactPanel()
        {
            FooterContact phone = FindContact("Phone");
            FooterContact phoneIntl = FindContact("PhoneIntl");
            FooterContact email = FindContact("Email");

            if (phPhone != null)
            {
                phPhone.Visible = phone != null && !string.IsNullOrEmpty(phone.Value);
                if (phPhone.Visible)
                {
                    lnkPhone.HRef = "tel:" + phone.Value;
                    litPhone.Text = Server.HtmlEncode(IsArabic ? phone.LabelAr : phone.LabelEn);
                }
            }

            if (phPhoneIntl != null)
            {
                phPhoneIntl.Visible = true;
                if (phoneIntl != null && !string.IsNullOrEmpty(phoneIntl.Value))
                {
                    lnkPhoneIntl.HRef = "tel:" + phoneIntl.Value;
                    litPhoneIntl.Text = Server.HtmlEncode(IsArabic ? phoneIntl.LabelAr : phoneIntl.LabelEn);
                }
                else
                {
                    // Same number as the seeded PhoneIntl row (link and text now match)
                    lnkPhoneIntl.HRef = "tel:+966118220011";
                    litPhoneIntl.Text = "00966-11-8220011";
                }
            }

            if (phEmail != null)
            {
                phEmail.Visible = email != null && !string.IsNullOrEmpty(email.Value);
                if (phEmail.Visible)
                {
                    lnkEmail.HRef = "mailto:" + email.Value;
                    litEmail.Text = Server.HtmlEncode(IsArabic ? email.LabelAr : email.LabelEn);
                }
            }
        }

        private FooterContact FindContact(string key)
        {
            foreach (FooterContact c in _contacts)
            {
                if (c.Key.Equals(key, StringComparison.OrdinalIgnoreCase)) return c;
            }
            return null;
        }

        private void BindRepeaters()
        {
            if (rptOverviewLinks != null)
            {
                rptOverviewLinks.DataSource = _overviewLinks;
                rptOverviewLinks.DataBind();
            }

            if (rptOpportunitiesLinks != null)
            {
                rptOpportunitiesLinks.DataSource = _opportunitiesLinks;
                rptOpportunitiesLinks.DataBind();
            }

            if (rptUniversityLifeLinks != null)
            {
                rptUniversityLifeLinks.DataSource = _universityLifeLinks;
                rptUniversityLifeLinks.DataBind();
            }

            if (rptUsefulLinks != null)
            {
                rptUsefulLinks.DataSource = _usefulLinks;
                rptUsefulLinks.DataBind();
            }

            if (rptSocial != null)
            {
                rptSocial.DataSource = _socialLinks;
                rptSocial.DataBind();
            }

            if (rptDownLinks != null)
            {
                rptDownLinks.DataSource = _downLinks;
                rptDownLinks.DataBind();
            }
        }

        // ------------------------------------------------------------------
        //  Rendering helpers (encoded output for list content)
        // ------------------------------------------------------------------

        protected static string Text(object value) { return SafeHtml.Text(value); }
        protected static string Attr(object value) { return SafeHtml.Attr(value); }
        protected static string Href(object value) { return SafeHtml.Href(value); }

        // ------------------------------------------------------------------
        //  Utilities
        // ------------------------------------------------------------------

        private static string SafeString(object v)
        {
            return v == null ? "" : v.ToString();
        }

        private static bool SafeBool(object v)
        {
            if (v == null) return false;
            bool b;
            return bool.TryParse(v.ToString(), out b) && b;
        }

        // ------------------------------------------------------------------
        //  DTOs
        // ------------------------------------------------------------------

        public class FooterLink
        {
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string UrlAr { get; set; }
            public string UrlEn { get; set; }
            public string Url { get; set; }
            public bool OpenInNewTab { get; set; }
            public string SectionKey { get; set; }

            public string GetDisplayTitle(bool isArabic)
            {
                return isArabic ? (!string.IsNullOrEmpty(TitleAr) ? TitleAr : TitleEn) : (!string.IsNullOrEmpty(TitleEn) ? TitleEn : TitleAr);
            }

            public string GetDisplayUrl(bool isArabic)
            {
                string u = isArabic ? (!string.IsNullOrEmpty(UrlAr) ? UrlAr : Url) : (!string.IsNullOrEmpty(UrlEn) ? UrlEn : Url);
                return string.IsNullOrEmpty(u) ? "#" : u;
            }
        }

        public class FooterContact
        {
            public string Key { get; set; }
            public string ContactType { get; set; }
            public string LabelAr { get; set; }
            public string LabelEn { get; set; }
            public string Value { get; set; }
        }

        public class FooterSocial
        {
            public string Key { get; set; }
            public string IconClass { get; set; }
            public string Url { get; set; }
            public string AriaLabelAr { get; set; }
            public string AriaLabelEn { get; set; }
            public string ParentKey { get; set; }
            public List<FooterSocial> Children { get; set; }

            public bool HasChildren { get { return Children != null && Children.Count > 0; } }
            public bool HasUrl { get { return !string.IsNullOrEmpty(Url); } }
        }

        public class FooterDownLink
        {
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string UrlAr { get; set; }
            public string UrlEn { get; set; }
            public bool OpenInNewTab { get; set; }

            public string GetDisplayTitle(bool isArabic)
            {
                return isArabic ? (!string.IsNullOrEmpty(TitleAr) ? TitleAr : TitleEn) : (!string.IsNullOrEmpty(TitleEn) ? TitleEn : TitleAr);
            }

            public string GetDisplayUrl(bool isArabic)
            {
                string u = isArabic ? UrlAr : UrlEn;
                return string.IsNullOrEmpty(u) ? "#" : u;
            }
        }
    }
}
