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
    /// Faculty organizational structure (الهيكل التنظيمي للكلية) - DGA design.
    /// Markup lives in the .ascx; this only binds data.
    /// Lists: FacultyOrgStructure / FacultyOrgStructureImages, provisioned + seeded
    /// on the current web at page load. Body text is stored plain; HTML added here.
    /// </summary>
    public partial class ucFacultyAchievementsDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class ItemDto
        {
            public string ItemKey { get; set; }
            public string Title { get; set; }
            public List<BodyBlock> Body { get; set; }
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
                AchievementsProvisioner.EnsureLists();
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
            //ltrHeading.Text = IsArabic ? "إنجازات الكلية" : "Faculty Achievements";
            //ltrIntro.Text = IsArabic ? "مساحة مخصصة لإبراز منجزات الكلية ومشاركاتها النوعية ضمن أخبار وإنجازات الجامعة." : "A space to highlight the faculty's achievements and distinguished contributions.";
        }

        private void BindItems(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(AchievementsProvisioner.ItemsListName);
            if (list == null) { phItems.Visible = false; return; }

            // Preload images grouped by ItemKey.
            var imagesByKey = LoadImages(web);

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            var dtos = new List<ItemDto>();
            int idx = 1;
            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "ItemKey");
                bool expanded = idx == 1;
                dtos.Add(new ItemDto
                {
                    ItemKey = key,
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Body = FacultyProvisioningHelper.ParseBody(IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr"))),
                    Images = imagesByKey.ContainsKey(key) ? imagesByKey[key] : new List<string>(),
                    HeadingId = "faculty-achievementsAccordionHeading" + idx,
                    CollapseId = "faculty-achievementsAccordionCollapse" + idx,
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
            SPList list = web.Lists.TryGetList(AchievementsProvisioner.ImagesListName);
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

        /// <summary>Binds the parsed body blocks and the image list inside each accordion item.</summary>
        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            ItemDto row = e.Item.DataItem as ItemDto;
            if (row == null) return;

            Repeater rptBody = e.Item.FindControl("rptBody") as Repeater;
            if (rptBody != null) { rptBody.DataSource = row.Body; rptBody.DataBind(); }

            Repeater rptImages = e.Item.FindControl("rptImages") as Repeater;
            if (rptImages != null) { rptImages.DataSource = row.Images; rptImages.DataBind(); }
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
