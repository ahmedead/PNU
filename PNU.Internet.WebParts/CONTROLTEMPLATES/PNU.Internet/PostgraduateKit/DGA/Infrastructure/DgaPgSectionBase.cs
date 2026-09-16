using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    /// <summary>
    /// Base user control for DGA Postgraduate Academic Kit sections.
    /// Handles language resolution and safe execution.
    /// </summary>
    public abstract class DgaPgSectionBase : UserControl
    {
        public virtual string Phase { get; set; } = DgaPgPhases.Applicants;

        public bool IsArabic => DgaPgHelper.IsArabic;
        public string CurrentLangCode => DgaPgHelper.CurrentLangCode;

        protected SPWeb OpenTargetWeb(SPSite site)
        {
            return DgaPgTargetWeb.Open(site);
        }

        protected SPList GetList(SPWeb web, string listName)
        {
            return DgaPgListProvisioner.FindList(web, listName);
        }

        public string ResolveKitHubUrl()
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    string url = SPContext.Current.Web.ServerRelativeUrl.TrimEnd('/');
                    if (url.StartsWith("/ar", StringComparison.OrdinalIgnoreCase) ||
                        url.StartsWith("/en", StringComparison.OrdinalIgnoreCase) ||
                        url.Contains("/ar/") || url.Contains("/en/") ||
                        url.EndsWith("/ar", StringComparison.OrdinalIgnoreCase) ||
                        url.EndsWith("/en", StringComparison.OrdinalIgnoreCase))
                    {
                        return url + "/Pages/academic-kit.aspx";
                    }
                    return url + "/" + (IsArabic ? "ar" : "en") + "/Pages/academic-kit.aspx";
                }
            }
            catch { }
            return "academic-kit.aspx";
        }

        public string ResolveSiteHomeUrl()
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    string root = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/');
                    return (string.IsNullOrEmpty(root) ? "" : root) + "/" + (IsArabic ? "ar" : "en") + "/Pages/default.aspx";
                }
            }
            catch { }
            return "/" + (IsArabic ? "ar" : "en") + "/Pages/default.aspx";
        }

        public string ResolveKitUrl(string stage)
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    string url = SPContext.Current.Web.ServerRelativeUrl.TrimEnd('/');
                    if (url.StartsWith("/ar", StringComparison.OrdinalIgnoreCase) ||
                        url.StartsWith("/en", StringComparison.OrdinalIgnoreCase) ||
                        url.Contains("/ar/") || url.Contains("/en/") ||
                        url.EndsWith("/ar", StringComparison.OrdinalIgnoreCase) ||
                        url.EndsWith("/en", StringComparison.OrdinalIgnoreCase))
                    {
                        return url + "/Pages/academic-kit-" + stage + ".aspx";
                    }
                    return url + "/" + (IsArabic ? "ar" : "en") + "/Pages/academic-kit-" + stage + ".aspx";
                }
            }
            catch { }
            return "academic-kit-" + stage + ".aspx";
        }

        /// <summary>
        /// Creates and seeds the DgaPg* lists before the derived control's Page_Load reads
        /// them. Nothing else invokes the provisioner on the public pages, so without this
        /// the lists never come into existence and every station renders the reduced
        /// EnsureDefaults() content instead of the list data.
        /// EnsureAllListsExist is guarded per request and per app-domain, so this is a
        /// dictionary lookup on every call after the first.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ExecuteSafely("EnsureAllListsExist", DgaPgListProvisioner.EnsureAllListsExist);
        }

        protected void ExecuteSafely(string actionName, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                DgaPgLog.Write(GetType().Name + "." + actionName, ex);
            }
        }

        // ---------------------------------------------------------------
        // Sections (DgaPgSections), shared by every station control
        // ---------------------------------------------------------------

        /// <summary>Section rows for this control's Phase, keyed by SectionKey.</summary>
        protected readonly Dictionary<string, DgaPgSectionModel> Sections =
            new Dictionary<string, DgaPgSectionModel>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Fills <see cref="Sections"/> from the DgaPgSections list for the current Phase.</summary>
        protected void LoadSections(SPWeb web)
        {
            SPList list = GetList(web, DgaPgListNames.Sections);
            if (list == null) return;

            var query = new SPQuery
            {
                Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>" + Phase + "</Value></Eq></Where>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                string key = DgaPgHelper.SafeString(item, "SectionKey");
                if (string.IsNullOrEmpty(key)) continue;

                Sections[key] = new DgaPgSectionModel
                {
                    SectionKey = key,
                    Phase = Phase,
                    Title = DgaPgHelper.PickField(item, "Title"),
                    LeadText = DgaPgHelper.PickField(item, "LeadText"),
                    BadgeText = DgaPgHelper.PickField(item, "BadgeText"),
                    IconClass = DgaPgHelper.SafeString(item, "IconClass"),
                    NoteText = DgaPgHelper.PickField(item, "NoteText"),
                    ItemOrder = DgaPgHelper.SafeInt(item, "ItemOrder"),
                    IsVisible = DgaPgHelper.SafeBool(item, "IsVisible", true)
                };
            }
        }

        protected string GetSectionTitle(string sectionKey, string fallback)
        {
            DgaPgSectionModel section;
            if (Sections.TryGetValue(sectionKey, out section) && !string.IsNullOrEmpty(section.Title))
                return section.Title;
            return fallback;
        }

        protected string GetSectionLead(string sectionKey, string fallback)
        {
            DgaPgSectionModel section;
            if (Sections.TryGetValue(sectionKey, out section) && !string.IsNullOrEmpty(section.LeadText))
                return section.LeadText;
            return fallback;
        }

        protected string GetSectionNote(string sectionKey)
        {
            DgaPgSectionModel section;
            if (Sections.TryGetValue(sectionKey, out section)) return section.NoteText;
            return string.Empty;
        }

        /// <summary>An editor can hide a whole axis by clearing IsVisible on its section row.</summary>
        protected bool IsSectionVisible(string sectionKey)
        {
            DgaPgSectionModel section;
            if (Sections.TryGetValue(sectionKey, out section)) return section.IsVisible;
            return true;
        }

        protected string GetResource(string key, string ar, string en)
        {
            return DgaPgHelper.Pick(ar, en);
        }

        protected string ReferenceTitle
        {
            get
            {
                return GetSectionTitle(DgaPgSectionKeys.Reference,
                    DgaPgHelper.Pick("الأساس النظامي والمرجعي", "Statutory and Reference Basis"));
            }
        }

        protected string ReferenceLead
        {
            get
            {
                return GetSectionLead(DgaPgSectionKeys.Reference, DgaPgHelper.Pick(
                    "اللائحة المنظمة للدراسات العليا في الجامعات (قرار مجلس شؤون الجامعات ٢/٩/١٤٤٤) • الدليل الإرشادي والنماذج المعتمدة بموقع العمادة",
                    "The Regulation Governing Postgraduate Studies in Universities (Council of University Affairs resolution 2/9/1444) • The guide and approved forms on the Deanship website"));
            }
        }

        /// <summary>
        /// Fixed DOM anchor per section key. academic-kit-pages.css styles
        /// #applicant-section-N / #enrolled-section-N / #graduate-section-N individually,
        /// so anchors must never be derived from list position — reordering or hiding a
        /// section in SharePoint would otherwise move the styling onto the wrong content.
        /// </summary>
        protected virtual string SectionAnchor(string sectionKey)
        {
            return sectionKey;
        }

        /// <summary>Appends a TOC entry when the section is both visible and non-empty.</summary>
        protected void AddToc(List<DgaPgLinkItem> toc, string sectionKey, string title, bool hasContent)
        {
            if (!hasContent || string.IsNullOrEmpty(title)) return;
            if (!IsSectionVisible(sectionKey)) return;

            toc.Add(new DgaPgLinkItem { Title = title, Url = SectionAnchor(sectionKey) });
        }

        // ---------------------------------------------------------------
        // Markup helpers used by the .ascx templates
        // ---------------------------------------------------------------

        /// <summary>HTML-encodes list content before it reaches the markup.</summary>
        protected static string Enc(string value)
        {
            return HttpUtility.HtmlEncode(value ?? string.Empty);
        }

        /// <summary>Encodes a list-supplied URL for use in an href/src attribute.</summary>
        protected static string EncUrl(string value)
        {
            return HttpUtility.HtmlAttributeEncode(DgaPgHelper.ResolveUrl(value) ?? string.Empty);
        }

        /// <summary>
        /// Encodes each line of a multi-line list value and joins them with &lt;br /&gt;,
        /// so an editor can break a table cell without being able to inject markup.
        /// </summary>
        protected static string MultiLine(string value)
        {
            List<string> lines = DgaPgHelper.SplitLines(value);
            if (lines.Count == 0) return string.Empty;

            var encoded = new List<string>();
            foreach (string line in lines) encoded.Add(HttpUtility.HtmlEncode(line));

            return string.Join("<br />", encoded.ToArray());
        }

        /// <summary>Falls back to the design's default icon when the list leaves IconClass empty.</summary>
        protected static string IconOr(string value, string fallback)
        {
            return string.IsNullOrEmpty(value) ? fallback : value;
        }

        /// <summary>
        /// Parses the Header list's BreadcrumbText ("text|url;text|url;text|") into links.
        /// The final crumb is the current page and normally carries no URL.
        /// </summary>
        protected static List<DgaPgLinkItem> ParseBreadcrumbs(string raw)
        {
            var crumbs = new List<DgaPgLinkItem>();
            if (string.IsNullOrEmpty(raw)) return crumbs;

            foreach (string part in raw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string entry = part.Trim();
                if (entry.Length == 0) continue;

                string text = entry;
                string url = string.Empty;

                int pipe = entry.IndexOf('|');
                if (pipe >= 0)
                {
                    text = entry.Substring(0, pipe).Trim();
                    url = entry.Substring(pipe + 1).Trim();
                }

                if (text.Length > 0) crumbs.Add(new DgaPgLinkItem { Title = text, Url = url });
            }

            return crumbs;
        }

        // ---------------------------------------------------------------
        // Body CSS class (v4 design parity)
        // ---------------------------------------------------------------

        /// <summary>
        /// Body classes the static v4 design applies for this phase.
        /// academic-kit-pages.css scopes the great majority of its rules under
        /// .academic-kit-detail / .academic-kit-applicants / -enrolled / -graduates,
        /// so the station pages render unstyled unless these reach the body tag.
        /// </summary>
        protected virtual string BodyCssClass
        {
            get
            {
                switch (Phase)
                {
                    case DgaPgPhases.Applicants: return "academic-kit-detail academic-kit-applicants";
                    case DgaPgPhases.Enrolled: return "academic-kit-detail academic-kit-enrolled";
                    case DgaPgPhases.Graduates: return "academic-kit-detail academic-kit-graduates";
                    default: return "academic-kit-home";
                }
            }
        }

        private static readonly string[] KitBodyClasses = new string[]
        {
            "academic-kit-home",
            "academic-kit-detail",
            "academic-kit-applicants",
            "academic-kit-enrolled",
            "academic-kit-graduates"
        };

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Phase is assigned in the derived control's Page_Load, so this has to run later.
            ExecuteSafely("ApplyBodyCssClass", ApplyBodyCssClass);
        }

        private void ApplyBodyCssClass()
        {
            StampKitClasses(FindMasterControl("bodyTag"));

            // A handful of design rules are written as ".academic-kit-detail > .bg-primary-25"
            // (the title band is a direct child of <body> in the static v4 pages). Under
            // SharePoint the band sits inside MSO_ContentDiv, so the same classes are stamped
            // on that wrapper to keep those child selectors matching.
            StampKitClasses(FindMasterControl("MSO_ContentDiv"));
        }

        private void StampKitClasses(HtmlGenericControl target)
        {
            if (target == null) return;

            string current = target.Attributes["class"] ?? string.Empty;
            List<string> kept = new List<string>();

            foreach (string token in current.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Array.IndexOf(KitBodyClasses, token) < 0) kept.Add(token);
            }

            foreach (string token in BodyCssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!kept.Contains(token)) kept.Add(token);
            }

            target.Attributes["class"] = string.Join(" ", kept.ToArray());
        }

        private HtmlGenericControl FindMasterControl(string id)
        {
            if (Page == null) return null;

            HtmlGenericControl found = null;
            if (Page.Master != null) found = Page.Master.FindControl(id) as HtmlGenericControl;
            if (found == null) found = Page.FindControl(id) as HtmlGenericControl;
            if (found == null) found = SearchTree(Page.Master ?? (Control)Page, id);
            return found;
        }

        private static HtmlGenericControl SearchTree(Control root, string id)
        {
            if (root == null) return null;

            foreach (Control child in root.Controls)
            {
                HtmlGenericControl generic = child as HtmlGenericControl;
                if (generic != null && string.Equals(generic.ID, id, StringComparison.Ordinal)) return generic;

                HtmlGenericControl nested = SearchTree(child, id);
                if (nested != null) return nested;
            }

            return null;
        }
    }
}
