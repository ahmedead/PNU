using Microsoft.SharePoint;
using PNU.Internet.WebParts.Helpers;
using Portal.Main.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeFooter : UserControl
    {
        // ------------------------------------------------------------------
        //  Properties consumed by the .ascx
        //  NOTE: IsArabic is PUBLIC so Repeater data-binding expressions
        //  (<%# IsArabic ? ... %>) can resolve it at runtime.
        // ------------------------------------------------------------------

        public bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected string AboutTitle
        {
            get { return IsArabic ? "عن الجامعة" : "About the University"; }
        }

        protected string RelatedLinksTitle
        {
            get { return IsArabic ? "مواقع ذات صلة" : "Related Links"; }
        }

        protected string ContactTitle
        {
            get { return IsArabic ? "تواصل معنا" : "Contact Us"; }
        }

        protected string FollowTitle
        {
            get { return IsArabic ? "تابعنا على" : "Follow Us"; }
        }

        protected string CopyrightText
        {
            get
            {
                return IsArabic
                    ? "جميع الحقوق محفوظة 2026 | جامعة الأميرة نورة بنت عبدالرحمن"
                    : "All Rights Reserved 2026 | Princess Nourah bint Abdulrahman University";
            }
        }

        protected string Vision2030Alt
        {
            get { return IsArabic ? "شعار رؤية السعودية 2030" : "Saudi Vision 2030 logo"; }
        }

        protected string RaqmiAlt
        {
            get { return IsArabic ? "شعار ترخيص منصة رقمي" : "Raqmi platform license logo"; }
        }

        protected string AboutDescription = "";

        // In-memory caches for this request
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
                if (!Page.IsPostBack)
                {
                    // Step 1: ensure the 5 footer lists exist (provisioning runs once per AppDomain)
                    FooterListProvisioner.EnsureFooterLists();

                    // Step 2: load data from those lists
                    LoadFooterData();

                    // Step 3: bind repeaters and the contact panel
                    BindContactPanel();
                    BindRepeaters();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  Data loading
        // ------------------------------------------------------------------

        private void LoadFooterData()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb rootWeb = site.RootWeb)
                    {
                        LoadAbout(rootWeb);
                        LoadUsefulLinks(rootWeb);
                        LoadContacts(rootWeb);
                        LoadSocial(rootWeb);
                        LoadDownLinks(rootWeb);
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void LoadAbout(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_ABOUT);
            if (list == null) return;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>",
                RowLimit = 1
            };
            foreach (SPListItem item in list.GetItems(q))
            {
                AboutDescription = IsArabic
                    ? SafeString(item["DescriptionAr"])
                    : SafeString(item["DescriptionEn"]);
                break;
            }
        }

        private void LoadUsefulLinks(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_USEFULLINKS);
            if (list == null) return;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };
            foreach (SPListItem item in list.GetItems(q))
            {
                SPFieldUrlValue url = new SPFieldUrlValue(SafeString(item["URL"]));
                _usefulLinks.Add(new FooterLink
                {
                    TitleAr = SafeString(item["Title"]),
                    TitleEn = SafeString(item["TitleEn"]),
                    Url = string.IsNullOrEmpty(url.Url) ? "#" : url.Url,
                    OpenInNewTab = SafeBool(item["OpenInNewTab"])
                });
            }
        }

        private void LoadContacts(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_CONTACT);
            if (list == null) return;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };
            foreach (SPListItem item in list.GetItems(q))
            {
                _contacts.Add(new FooterContact
                {
                    Key = SafeString(item["Title"]),
                    ContactType = SafeString(item["ContactType"]),
                    LabelAr = SafeString(item["LabelAr"]),
                    LabelEn = SafeString(item["LabelEn"]),
                    Value = SafeString(item["Value"])
                });
            }
        }

        private void LoadSocial(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_FOLLOWUS);
            if (list == null) return;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };

            // Read all rows into a flat list first
            List<FooterSocial> all = new List<FooterSocial>();
            foreach (SPListItem item in list.GetItems(q))
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

            // Two-pass grouping: children get attached to their parent by ParentKey == parent.Key.
            // Only rows without a ParentKey are added to _socialLinks (the Repeater source).
            foreach (FooterSocial row in all)
            {
                if (string.IsNullOrEmpty(row.ParentKey))
                {
                    _socialLinks.Add(row);
                }
                else
                {
                    FooterSocial parent = null;
                    foreach (FooterSocial p in all)
                    {
                        if (p.Key == row.ParentKey && string.IsNullOrEmpty(p.ParentKey))
                        {
                            parent = p;
                            break;
                        }
                    }
                    if (parent != null)
                    {
                        parent.Children.Add(row);
                    }
                    // If the parent is missing, the orphan is silently dropped (or you can log it).
                }
            }
        }

        private void LoadDownLinks(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(FooterListProvisioner.LIST_DOWNLINKS);
            if (list == null) return;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };
            foreach (SPListItem item in list.GetItems(q))
            {
                SPFieldUrlValue urlAr = new SPFieldUrlValue(SafeString(item["URLAr"]));
                SPFieldUrlValue urlEn = new SPFieldUrlValue(SafeString(item["URLEn"]));
                _downLinks.Add(new FooterDownLink
                {
                    TitleAr = SafeString(item["Title"]),
                    TitleEn = SafeString(item["TitleEn"]),
                    UrlAr = string.IsNullOrEmpty(urlAr.Url) ? "#" : urlAr.Url,
                    UrlEn = string.IsNullOrEmpty(urlEn.Url) ? "#" : urlEn.Url,
                    OpenInNewTab = SafeBool(item["OpenInNewTab"])
                });
            }
        }

        // ------------------------------------------------------------------
        //  Bindings
        // ------------------------------------------------------------------

        /// <summary>
        /// Populates the contact placeholders/labels based on the "Title" key of each row:
        /// "Phone", "Email". (TradeMark/TradeDept removed per the new DGA design.)
        /// </summary>
        private void BindContactPanel()
        {
            FooterContact phone = FindContact("Phone");
            FooterContact email = FindContact("Email");

            if (phone != null)
            {
                phPhone.Visible = true;
                lnkPhone.HRef = "tel:" + phone.Value;
                litPhone.Text = Server.HtmlEncode(IsArabic ? phone.LabelAr : phone.LabelEn);
            }

            if (email != null)
            {
                phEmail.Visible = true;
                lnkEmail.HRef = "mailto:" + email.Value;
                litEmail.Text = Server.HtmlEncode(IsArabic ? email.LabelAr : email.LabelEn);
            }
        }

        private FooterContact FindContact(string key)
        {
            foreach (FooterContact c in _contacts)
            {
                if (c.Key == key) return c;
            }
            return null;
        }

        private void BindRepeaters()
        {
            rptUsefulLinks.DataSource = _usefulLinks;
            rptUsefulLinks.DataBind();

            rptSocial.DataSource = _socialLinks;
            rptSocial.DataBind();

            rptDownLinks.DataSource = _downLinks;
            rptDownLinks.DataBind();
        }

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
        //  DTOs (public so Eval() in the repeater data-binding can access them)
        // ------------------------------------------------------------------

        public class FooterLink
        {
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string Url { get; set; }
            public bool OpenInNewTab { get; set; }
        }

        public class FooterContact
        {
            public string Key { get; set; }           // "Phone" | "Email"
            public string ContactType { get; set; }   // "Phone" | "Email" | "Text"
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

            // For dropdown support: ParentKey is the Key of the parent row (empty for top-level items).
            // Children is populated at load time for top-level items that have sub-rows.
            public string ParentKey { get; set; }
            public List<FooterSocial> Children { get; set; }

            // Convenience flags used by the data-binding expressions in the .ascx
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
        }
    }
}
