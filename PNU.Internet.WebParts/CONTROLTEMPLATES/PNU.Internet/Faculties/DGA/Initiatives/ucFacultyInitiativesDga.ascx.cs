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
    /// Faculty initiatives (المبادرات) - DGA design.
    /// Markup lives in the .ascx; this only binds data.
    /// Lists: FacultyInitiatives (+Images) and, for قائمة العميد, the child lists
    /// FacultyInitiativeSections / FacultyInitiativeCriteria / FacultyDeansListNames,
    /// all matched by ItemKey. Body text is stored plain; HTML added here.
    /// </summary>
    public partial class ucFacultyInitiativesDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class SectionDto
        {
            public string Title { get; set; }
            public List<BodyBlock> Body { get; set; }
        }

        public class CriterionDto
        {
            public string Criterion { get; set; }
            public List<BodyBlock> Details { get; set; }
            public string MaxScore { get; set; }
        }

        public class NameDto
        {
            public string StudentName { get; set; }
            public string Track { get; set; }
        }

        public class NameGroupDto
        {
            public string Track { get; set; }
            public List<NameDto> Names { get; set; }
        }

        public class ItemDto
        {
            public string ItemKey { get; set; }
            public string Title { get; set; }
            public List<BodyBlock> Body { get; set; }
            public List<SectionDto> Sections { get; set; }
            public List<CriterionDto> Criteria { get; set; }
            public bool HasCriteria { get; set; }
            public List<NameGroupDto> NameGroups { get; set; }
            public bool HasNames { get; set; }
            public List<string> Images { get; set; }
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
                InitiativesProvisioner.EnsureLists();
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetTitles();
                if (!IsPostBack)
                    BindItems(SPContext.Current.Web);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void SetTitles()
        {
            //ltrHeading.Text = IsArabic ? "المبادرات" : "Initiatives";
            //ltrIntro.Text = IsArabic ? "تعرّف على مبادرات الكلية وبرامجها الداعمة للتميز الأكاديمي والمهني والمشاركة المجتمعية." : "Learn about the faculty's initiatives supporting academic and professional excellence.";
        }

        private void BindItems(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(InitiativesProvisioner.ItemsListName);
            if (list == null) { phItems.Visible = false; return; }

            // Preload children grouped by ItemKey.
            var imagesByKey = LoadImages(web);
            var sectionsByKey = LoadSections(web);
            var criteriaByKey = LoadCriteria(web);
            var namesByKey = LoadNames(web);

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            var dtos = new List<ItemDto>();
            int idx = 1;
            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "ItemKey");
                bool expanded = idx == 1;
                var criteria = criteriaByKey.ContainsKey(key) ? criteriaByKey[key] : new List<CriterionDto>();
                var nameGroups = namesByKey.ContainsKey(key) ? namesByKey[key] : new List<NameGroupDto>();
                dtos.Add(new ItemDto
                {
                    ItemKey = key,
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Body = FacultyProvisioningHelper.ParseBody(IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr"))),
                    Sections = sectionsByKey.ContainsKey(key) ? sectionsByKey[key] : new List<SectionDto>(),
                    Criteria = criteria,
                    HasCriteria = criteria.Count > 0,
                    NameGroups = nameGroups,
                    HasNames = nameGroups.Count > 0,
                    Images = imagesByKey.ContainsKey(key) ? imagesByKey[key] : new List<string>(),
                    HeadingId = "faculty-initiativesAccordionHeading" + idx,
                    CollapseId = "faculty-initiativesAccordionCollapse" + idx,
                    ButtonClass = expanded ? "accordion-button" : "accordion-button collapsed",
                    PanelClass = expanded ? "accordion-collapse collapse show" : "accordion-collapse collapse",
                    AriaExpanded = expanded ? "true" : "false"
                });
                idx++;
            }

            phItems.Visible = dtos.Count > 0;
            rptItems.DataSource = dtos;
            rptItems.DataBind();
        }

        private Dictionary<string, List<string>> LoadImages(SPWeb web)
        {
            var map = new Dictionary<string, List<string>>();
            SPList list = web.Lists.TryGetList(InitiativesProvisioner.ImagesListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "ItemKey");
                string url = SafeUrl(it, "ImageUrl");
                if (string.IsNullOrEmpty(url)) continue;
                if (!map.ContainsKey(key)) map[key] = new List<string>();
                map[key].Add(url);
            }
            return map;
        }

        private Dictionary<string, List<SectionDto>> LoadSections(SPWeb web)
        {
            var map = new Dictionary<string, List<SectionDto>>();
            SPList list = web.Lists.TryGetList(InitiativesProvisioner.SectionsListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "ItemKey");
                if (!map.ContainsKey(key)) map[key] = new List<SectionDto>();
                map[key].Add(new SectionDto
                {
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Body = FacultyProvisioningHelper.ParseBody(
                        IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr")))
                });
            }
            return map;
        }

        private Dictionary<string, List<CriterionDto>> LoadCriteria(SPWeb web)
        {
            var map = new Dictionary<string, List<CriterionDto>>();
            SPList list = web.Lists.TryGetList(InitiativesProvisioner.CriteriaListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "ItemKey");
                if (!map.ContainsKey(key)) map[key] = new List<CriterionDto>();
                map[key].Add(new CriterionDto
                {
                    Criterion = SafeString(it, "Title"),
                    Details = FacultyProvisioningHelper.ParseBody(
                        IsArabic ? SafeString(it, "Details") : Fallback(SafeString(it, "DetailsEn"), SafeString(it, "Details"))),
                    MaxScore = SafeString(it, "MaxScore")
                });
            }
            return map;
        }

        private Dictionary<string, List<NameGroupDto>> LoadNames(SPWeb web)
        {
            var map = new Dictionary<string, List<NameGroupDto>>();
            SPList list = web.Lists.TryGetList(InitiativesProvisioner.NamesListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            // group: ItemKey -> Track -> names (order preserved)
            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "ItemKey");
                string track = IsArabic ? SafeString(it, "Track") : Fallback(SafeString(it, "TrackEn"), SafeString(it, "Track"));
                string name = SafeString(it, "Title");
                if (string.IsNullOrEmpty(name)) continue;

                if (!map.ContainsKey(key)) map[key] = new List<NameGroupDto>();
                NameGroupDto grp = map[key].FirstOrDefault(g => g.Track == track);
                if (grp == null)
                {
                    grp = new NameGroupDto { Track = track, Names = new List<NameDto>() };
                    map[key].Add(grp);
                }
                grp.Names.Add(new NameDto { StudentName = name, Track = track });
            }
            return map;
        }

        /// <summary>Binds the parsed body blocks and the image list inside each accordion item.</summary>
        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            ItemDto row = e.Item.DataItem as ItemDto;
            if (row == null) return;

            Repeater rptBody = e.Item.FindControl("rptBody") as Repeater;
            if (rptBody != null) { rptBody.DataSource = row.Body; rptBody.DataBind(); }

            Repeater rptSections = e.Item.FindControl("rptSections") as Repeater;
            if (rptSections != null) { rptSections.DataSource = row.Sections; rptSections.DataBind(); }

            PlaceHolder phCriteria = e.Item.FindControl("phCriteria") as PlaceHolder;
            if (phCriteria != null) phCriteria.Visible = row.HasCriteria;
            Repeater rptCriteria = e.Item.FindControl("rptCriteria") as Repeater;
            if (rptCriteria != null) { rptCriteria.DataSource = row.Criteria; rptCriteria.DataBind(); }

            PlaceHolder phNames = e.Item.FindControl("phNames") as PlaceHolder;
            if (phNames != null) phNames.Visible = row.HasNames;
            Repeater rptNameGroups = e.Item.FindControl("rptNameGroups") as Repeater;
            if (rptNameGroups != null) { rptNameGroups.DataSource = row.NameGroups; rptNameGroups.DataBind(); }

            Repeater rptImages = e.Item.FindControl("rptImages") as Repeater;
            if (rptImages != null) { rptImages.DataSource = row.Images; rptImages.DataBind(); }
        }

        /// <summary>Binds the parsed detail blocks inside each scoring-criteria row.</summary>
        protected void rptCriteria_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
            CriterionDto row = e.Item.DataItem as CriterionDto;
            if (row == null) return;
            Repeater rptDetails = e.Item.FindControl("rptDetails") as Repeater;
            if (rptDetails != null) { rptDetails.DataSource = row.Details; rptDetails.DataBind(); }
        }

        /// <summary>Binds the student names inside each Dean's-List track group.</summary>
        protected void rptNameGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
            NameGroupDto row = e.Item.DataItem as NameGroupDto;
            if (row == null) return;
            Repeater rptNames = e.Item.FindControl("rptNames") as Repeater;
            if (rptNames != null) { rptNames.DataSource = row.Names; rptNames.DataBind(); }
        }

        // ---- helpers (same as Contact control) ----
        private string DisplayName(SPListItem item, string arField, string enField)
        {
            string ar = SafeString(item, arField);
            string en = SafeString(item, enField);
            return IsArabic
                ? (!string.IsNullOrEmpty(ar) ? ar : en)
                : (!string.IsNullOrEmpty(en) ? en : ar);
        }

        private static string Fallback(string primary, string secondary)
        {
            return string.IsNullOrEmpty(primary) ? secondary : primary;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try
            {
                return item.Fields.ContainsField(field) && item[field] != null
                    ? item[field].ToString() : string.Empty;
            }
            catch { return string.Empty; }
        }

        private static double SafeDouble(SPListItem item, string field)
        {
            try
            {
                double v;
                return item.Fields.ContainsField(field) && item[field] != null
                    && double.TryParse(item[field].ToString(), out v) ? v : 0;
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
