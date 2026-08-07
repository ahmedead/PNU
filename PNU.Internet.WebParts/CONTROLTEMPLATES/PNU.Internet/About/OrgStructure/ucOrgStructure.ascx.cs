using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About.OrgStructure
{
    /// <summary>
    /// Renders the PNU organizational-structure chart. Reads unit content from the
    /// OrgStructureUnits list and emits it as JSON (window.__pnuOrgUnits) plus a
    /// bilingual fallback config (window.__pnuOrgConfig) consumed by the client script.
    /// The SVG geometry lives in the .ascx and is never touched here.
    /// </summary>
    public partial class ucOrgStructure : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                // Provision the list/fields/seed only for authenticated users.
                if (SPContext.Current != null &&
                    SPContext.Current.Web != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    OrgStructureProvisioner.EnsureList(SPContext.Current.Web);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "ucOrgStructure.OnInit", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // ViewState is disabled in SharePoint page zones — rebind every load.
            BindData();
        }

        private bool IsArabic
        {
            get
            {
                try { return PortalHelper.IsArabic; }
                catch
                {
                    return SPContext.Current != null &&
                           SPContext.Current.Web != null &&
                           SPContext.Current.Web.Language == 1025;
                }
            }
        }

        private void BindData()
        {
            try
            {
                bool ar = IsArabic;
                var repo = new OrgStructureRepository(SPContext.Current.Web);
                List<OrgStructureUnit> units = repo.GetActiveUnits();

                var sb = new StringBuilder();
                sb.Append("<script>");
                sb.Append("window.__pnuOrgUnits=");
                sb.Append(BuildUnitsJson(units, ar));
                sb.Append(";window.__pnuOrgConfig=");
                sb.Append(BuildConfigJson(ar));
                sb.Append(";</script>");

                litUnitsJson.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "ucOrgStructure.BindData", ex.Message);
                litUnitsJson.Text = "<script>window.__pnuOrgUnits=[];</script>";
            }
        }

        private static string BuildUnitsJson(List<OrgStructureUnit> units, bool ar)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < units.Count; i++)
            {
                OrgStructureUnit u = units[i];
                if (i > 0) sb.Append(",");
                sb.Append("{");
                sb.Append("\"selector\":").Append(J(u.Selector)).Append(",");
                sb.Append("\"title\":").Append(J(u.LocalizedTitle(ar))).Append(",");
                sb.Append("\"desc\":").Append(J(u.LocalizedDescription(ar))).Append(",");
                sb.Append("\"badge\":").Append(J(u.LocalizedBadge(ar))).Append(",");
                sb.Append("\"meta\":").Append(J(u.LocalizedMeta(ar))).Append(",");
                sb.Append("\"icon\":").Append(J(u.Icon)).Append(",");
                sb.Append("\"theme\":").Append(J(u.Theme)).Append(",");
                sb.Append("\"href\":").Append(J(u.LinkUrl));
                sb.Append("}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static string BuildConfigJson(bool ar)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"fallbackTitle\":").Append(J(ar ? "وحدة تنظيمية" : "Organizational unit")).Append(",");
            sb.Append("\"noDetails\":").Append(J(ar ? "لا توجد صفحة تفاصيل" : "No details page")).Append(",");
            sb.Append("\"generic\":{");
            sb.Append("\"defaultBadge\":").Append(J(ar ? "ضمن الهيكل التنظيمي" : "Part of the org structure")).Append(",");
            sb.Append("\"defaultDesc\":").Append(J(ar
                ? "هذه الجهة موضحة ضمن الهيكل التنظيمي للجامعة، ولا تتوفر لها صفحة تفاصيل مستقلة حاليًا."
                : "This entity appears in the university org structure and currently has no standalone details page.")).Append(",");
            sb.Append("\"defaultMeta\":").Append(J(ar ? "الهيكل التنظيمي" : "Org structure")).Append(",");
            sb.Append("\"councilTitle\":").Append(J(ar ? "مجلس أو لجنة" : "Council or committee")).Append(",");
            sb.Append("\"councilBadge\":").Append(J(ar ? "جهة إشرافية" : "Supervisory body")).Append(",");
            sb.Append("\"agencyTitle\":").Append(J(ar ? "وكالة" : "Agency")).Append(",");
            sb.Append("\"agencyBadge\":").Append(J(ar ? "وكالة رئيسية" : "Main agency")).Append(",");
            sb.Append("\"deanshipTitle\":").Append(J(ar ? "عمادة" : "Deanship")).Append(",");
            sb.Append("\"deanshipBadge\":").Append(J(ar ? "عمادة مساندة" : "Supporting deanship")).Append(",");
            sb.Append("\"deptTitle\":").Append(J(ar ? "إدارة عامة" : "General department")).Append(",");
            sb.Append("\"deptBadge\":").Append(J(ar ? "جهة مرتبطة برئاسة الجامعة" : "Reports to the presidency")).Append(",");
            sb.Append("\"centerTitle\":").Append(J(ar ? "مركز" : "Center")).Append(",");
            sb.Append("\"centerBadge\":").Append(J(ar ? "مركز تخصصي" : "Specialized center")).Append(",");
            sb.Append("\"collegeTitle\":").Append(J(ar ? "كلية أو معهد" : "College or institute")).Append(",");
            sb.Append("\"collegeBadge\":").Append(J(ar ? "جهة أكاديمية" : "Academic body")).Append(",");
            sb.Append("\"entityTitle\":").Append(J(ar ? "جهة تنظيمية" : "Organizational entity")).Append(",");
            sb.Append("\"entityBadge\":").Append(J(ar ? "جهة مرتبطة" : "Affiliated body"));
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>Minimal, safe JSON string encoder for values injected into a &lt;script&gt; block.</summary>
        private static string J(string s)
        {
            if (s == null) return "\"\"";
            var sb = new StringBuilder(s.Length + 2);
            sb.Append('"');
            foreach (char c in s)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '<': sb.Append("\\u003c"); break; // prevent </script> breakout
                    case '>': sb.Append("\\u003e"); break;
                    case '&': sb.Append("\\u0026"); break;
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
                return System.Web.HttpContext.Current != null
                    ? System.Web.HttpContext.Current.Request.Url.ToString()
                    : string.Empty;
            }
            catch { return string.Empty; }
        }
    }
}
