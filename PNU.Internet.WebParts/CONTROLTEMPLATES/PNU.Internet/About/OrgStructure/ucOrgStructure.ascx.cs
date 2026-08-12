using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About
{
    /// <summary>
    /// Renders the PNU organizational-structure chart. The SVG skeleton (boxes +
    /// connectors, no text) lives in the .ascx; every label is bound here into an
    /// overlay Repeater from the OrgStructureUnits list (on the ListWebUrl web),
    /// positioned over its box, localized (AR/EN), with a Hugeicons glyph.
    /// </summary>
    public partial class ucOrgStructure : UserControl
    {
        /// <summary>Server-relative URL of the web that holds the list.</summary>
        [Browsable(true)]
        public string ListWebUrl { get; set; }

        /// <summary>URL of the Hugeicons stylesheet to inject (so box icons render as glyphs).</summary>
        [Browsable(true)]
        public string IconCssUrl { get; set; }

        private string EffectiveWebUrl
        {
            get { return string.IsNullOrWhiteSpace(ListWebUrl) ? "/ar/AboutUniversity/" : ListWebUrl.Trim(); }
        }

        private string EffectiveIconCss
        {
            get
            {
                return string.IsNullOrWhiteSpace(IconCssUrl)
                    ? "/_layouts/15/PNU.Internet/vendor/hugeicons/hgi-stroke-rounded.css"
                    : IconCssUrl.Trim();
            }
        }

        /// <summary>Bind-only view model for one overlay label (plain string props for Eval()).</summary>
        public class OrgLabelVM
        {
            public string Style { get; set; }
            public string Label { get; set; }
            public string Title { get; set; }
            public string Desc { get; set; }
            public string Icon { get; set; }
            public string IconClass { get; set; }
            public string Href { get; set; }
            public string HasLink { get; set; }
            public string LinkClass { get; set; }
            public string AriaLabel { get; set; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    OrgStructureProvisioner.EnsureListAt(SPContext.Current.Site.ID, EffectiveWebUrl);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "ucOrgStructure.OnInit", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData(); // ViewState off in SP zones — rebind every load.
        }

        private bool IsArabic
        {
            get
            {
                try { return PortalHelper.IsArabic; }
                catch
                {
                    return SPContext.Current != null && SPContext.Current.Web != null &&
                           SPContext.Current.Web.Language == 1025;
                }
            }
        }

        private void BindData()
        {
            bool ar = IsArabic;
            pnlStage.Attributes["dir"] = ar ? "rtl" : "ltr";
            // Language class drives per-language alignment (Arabic leads right, English left)
            // independent of dir inheritance from the master page.
            pnlStage.CssClass = "org-structure-stage " + (ar ? "org-rtl" : "org-ltr");

            // Inject the Hugeicons stylesheet so box icons render as glyphs.
            string css = EffectiveIconCss;
            litIconCss.Text = string.IsNullOrEmpty(css)
                ? string.Empty
                : "<link rel=\"stylesheet\" href=\"" + HttpUtility.HtmlAttributeEncode(css) + "\" />";

            try
            {
                var repo = new OrgStructureRepository(SPContext.Current.Web, EffectiveWebUrl);
                List<OrgStructureUnit> units = repo.GetActiveUnits();

                var vms = new List<OrgLabelVM>(units.Count);
                string sep = ar ? "، " : ", ";
                foreach (OrgStructureUnit u in units)
                {
                    string title = u.LocalizedTitle(ar) ?? string.Empty;
                    string desc = u.LocalizedDescription(ar) ?? string.Empty;
                    string badge = u.LocalizedBadge(ar) ?? string.Empty;
                    bool hasLink = !string.IsNullOrWhiteSpace(u.LinkUrl);
                    string icon = (u.Icon ?? string.Empty).Trim();

                    vms.Add(new OrgLabelVM
                    {
                        Style = u.OverlayStyle(ar),                    // server-generated, safe
                        Label = Enc(title),
                        Title = Enc(title),
                        Desc = Enc(desc),
                        Icon = Enc(icon),
                        IconClass = string.IsNullOrEmpty(icon) ? string.Empty : "hgi hgi-stroke " + Enc(icon),
                        Href = hasLink ? Enc(u.LinkUrl) : string.Empty,
                        HasLink = hasLink ? "true" : "false",
                        LinkClass = hasLink ? "has-link" : string.Empty,
                        AriaLabel = Enc(title + sep + badge + (string.IsNullOrEmpty(desc) ? "" : ". " + desc))
                    });
                }

                rptLabels.DataSource = vms;
                rptLabels.DataBind();

                rptLegend.DataSource = BuildLegend(ar);
                rptLegend.DataBind();

                litConfig.Text = BuildConfig(ar);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "ucOrgStructure.BindData", ex.Message);
                rptLabels.DataSource = new List<OrgLabelVM>();
                rptLabels.DataBind();
            }
        }

        // Legend (color key) entries — swatch center Y in SVG coords + bilingual text.
        private List<OrgLabelVM> BuildLegend(bool ar)
        {
            var rows = new[]
            {
                new { Cy = 20.2,  Ar = "المجالس",                                En = "Councils" },
                new { Cy = 48.0,  Ar = "الوكالات",                               En = "Vice-Presidencies" },
                new { Cy = 75.8,  Ar = "الإدارات العامة المرتبطة برئاسة الجامعة", En = "General departments linked to the University Presidency" },
                new { Cy = 103.5, Ar = "الكليات والمعاهد",                        En = "Colleges and Institutes" },
                new { Cy = 131.2, Ar = "العمادات",                               En = "Deanships" },
                new { Cy = 159.0, Ar = "المراكز",                                En = "Centers" },
                new { Cy = 186.8, Ar = "الكيانات المرتبطة برئاسة الجامعة",        En = "Entities linked to the University Presidency" },
                new { Cy = 214.5, Ar = "كيانات أخرى",                            En = "Other entities" }
            };

            var ci = CultureInfo.InvariantCulture;
            const double leftPx = 6.0, rightPx = 150.0, rowH = 26.0;
            double left = leftPx / OrgStructureUnit.ViewBoxW * 100.0;
            double width = (rightPx - leftPx) / OrgStructureUnit.ViewBoxW * 100.0;
            double height = rowH / OrgStructureUnit.ViewBoxH * 100.0;

            var list = new List<OrgLabelVM>(rows.Length);
            foreach (var r in rows)
            {
                double top = (r.Cy - rowH / 2.0) / OrgStructureUnit.ViewBoxH * 100.0;
                string style = string.Format(ci,
                    "left:{0:0.###}%;top:{1:0.###}%;width:{2:0.###}%;height:{3:0.###}%;",
                    left, top, width, height);
                list.Add(new OrgLabelVM { Style = style, Label = Enc(ar ? r.Ar : r.En) });
            }
            return list;
        }

        private static string BuildConfig(bool ar)
        {
            string fallback = ar ? "وحدة تنظيمية" : "Organizational unit";
            string noDetails = ar ? "لا توجد صفحة تفاصيل" : "No details page";
            return "<script>window.__pnuOrgConfig={fallbackTitle:" + JsStr(fallback) +
                   ",noDetails:" + JsStr(noDetails) + "};</script>";
        }

        private static string Enc(string s)
        {
            return HttpUtility.HtmlEncode(s ?? string.Empty);
        }

        private static string JsStr(string s)
        {
            var sb = new StringBuilder(s.Length + 2);
            sb.Append('"');
            foreach (char c in s)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '<': sb.Append("\\u003c"); break;
                    case '>': sb.Append("\\u003e"); break;
                    case '&': sb.Append("\\u0026"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    default:
                        if (c < ' ') sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
            return sb.ToString();
        }

        private static string GetUrl()
        {
            try
            {
                return HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : string.Empty;
            }
            catch { return string.Empty; }
        }
    }
}
