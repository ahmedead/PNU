using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty agencies (وكالات الكلية) - DGA design.
    /// Parent accordion (FacultyAgencies); each item renders a grid of unit cards
    /// (FacultyAgencyUnits) matched by AgencyKey. Body text stored plain; HTML added here.
    /// </summary>
    public partial class ucFacultyAgenciesDga : UserControl
    {
        private bool IsArabic { get { return SPContext.Current.Web.Language == 1025; } }

        public class UnitDto
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public bool HasLink { get; set; }
        }

        public class IntroDto
        {
            public string Title { get; set; }
            public List<BodyBlock> Body { get; set; }
        }

        public class AgencyDto
        {
            public string AgencyKey { get; set; }
            public string Title { get; set; }
            public List<IntroDto> Intros { get; set; }
            public List<BodyBlock> Body { get; set; }
            public List<UnitDto> Units { get; set; }
            public string HeadingId { get; set; }
            public string CollapseId { get; set; }
            public string ButtonClass { get; set; }
            public string PanelClass { get; set; }
            public string AriaExpanded { get; set; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current != null
                && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                AgenciesProvisioner.EnsureLists();
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetTitles();
                if (!IsPostBack)
                    BindAgencies(SPContext.Current.Web);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void SetTitles()
        {
            //ltrHeading.Text = IsArabic ? "وكالات الكلية" : "Faculty Agencies";
            //ltrIntro.Text = IsArabic
            //    ? "تعرض الوكالات المنشورة حاليًا ضمن هيكل الكلية."
            //    : "The agencies currently published within the faculty structure.";
        }

        private void BindAgencies(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(AgenciesProvisioner.ItemsListName);
            if (list == null) { phAgencies.Visible = false; return; }

            var introsByKey = LoadIntros(web);
            var unitsByKey = LoadUnits(web);

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            var dtos = new List<AgencyDto>();
            int idx = 1;
            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "AgencyKey");
                bool expanded = idx == 1;
                dtos.Add(new AgencyDto
                {
                    AgencyKey = key,
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Intros = introsByKey.ContainsKey(key) ? introsByKey[key] : new List<IntroDto>(),
                    Body = FacultyProvisioningHelper.ParseBody(IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr"))),
                    Units = unitsByKey.ContainsKey(key) ? unitsByKey[key] : new List<UnitDto>(),
                    HeadingId = "faculty-agenciesAccordionHeading" + idx,
                    CollapseId = "faculty-agenciesAccordionCollapse" + idx,
                    ButtonClass = expanded ? "accordion-button" : "accordion-button collapsed",
                    PanelClass = expanded ? "accordion-collapse collapse show" : "accordion-collapse collapse",
                    AriaExpanded = expanded ? "true" : "false"
                });
                idx++;
            }

            phAgencies.Visible = dtos.Count > 0;
            rptAgencies.DataSource = dtos;
            rptAgencies.DataBind();
        }

        private Dictionary<string, List<IntroDto>> LoadIntros(SPWeb web)
        {
            var map = new Dictionary<string, List<IntroDto>>();
            SPList list = web.Lists.TryGetList(AgenciesProvisioner.IntrosListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "AgencyKey");
                if (!map.ContainsKey(key)) map[key] = new List<IntroDto>();
                map[key].Add(new IntroDto
                {
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Body = FacultyProvisioningHelper.ParseBody(
                        IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr")))
                });
            }
            return map;
        }

        private Dictionary<string, List<UnitDto>> LoadUnits(SPWeb web)
        {
            var map = new Dictionary<string, List<UnitDto>>();
            SPList list = web.Lists.TryGetList(AgenciesProvisioner.UnitsListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "AgencyKey");
                string url = SafeUrl(it, "LinkUrl");
                if (!map.ContainsKey(key)) map[key] = new List<UnitDto>();
                map[key].Add(new UnitDto
                {
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Url = url,
                    HasLink = !string.IsNullOrEmpty(url)
                });
            }
            return map;
        }

        /// <summary>Binds the parsed body blocks and the unit cards inside each agency.</summary>
        protected void rptAgencies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            AgencyDto row = e.Item.DataItem as AgencyDto;
            if (row == null) return;

            Repeater rptIntros = e.Item.FindControl("rptIntros") as Repeater;
            if (rptIntros != null) { rptIntros.DataSource = row.Intros; rptIntros.DataBind(); }

            Repeater rptBody = e.Item.FindControl("rptBody") as Repeater;
            if (rptBody != null) { rptBody.DataSource = row.Body; rptBody.DataBind(); }

            Repeater rptUnits = e.Item.FindControl("rptUnits") as Repeater;
            if (rptUnits != null) { rptUnits.DataSource = row.Units; rptUnits.DataBind(); }
        }

        /// <summary>Binds the parsed body blocks inside each intro block (نبذة / كلمة الوكيلة).</summary>
        protected void rptIntros_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            IntroDto intro = e.Item.DataItem as IntroDto;
            if (intro == null) return;

            Repeater rptIntroBody = e.Item.FindControl("rptIntroBody") as Repeater;
            if (rptIntroBody != null) { rptIntroBody.DataSource = intro.Body; rptIntroBody.DataBind(); }
        }

        // ---- helpers ----
        private string DisplayName(SPListItem item, string arField, string enField)
        {
            string ar = SafeString(item, arField);
            string en = SafeString(item, enField);
            return IsArabic ? (!string.IsNullOrEmpty(ar) ? ar : en) : (!string.IsNullOrEmpty(en) ? en : ar);
        }

        private static string Fallback(string primary, string secondary)
        {
            return string.IsNullOrEmpty(primary) ? secondary : primary;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try { return item.Fields.ContainsField(field) && item[field] != null ? item[field].ToString() : string.Empty; }
            catch { return string.Empty; }
        }

        private static double SafeDouble(SPListItem item, string field)
        {
            try
            {
                double v;
                return item.Fields.ContainsField(field) && item[field] != null && double.TryParse(item[field].ToString(), out v) ? v : 0;
            }
            catch { return 0; }
        }

        private static string SafeUrl(SPListItem item, string field)
        {
            try
            {
                if (item.Fields.ContainsField(field) && item[field] != null)
                {
                    var v = new SPFieldUrlValue(item[field].ToString());
                    return v.Url ?? string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }
    }
}
