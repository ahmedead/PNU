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
    /// Scientific research in the faculty (البحث العلمي في الكلية) - DGA design.
    /// Markup lives in the .ascx; this only binds data.
    ///
    /// Lists (all on the CURRENT web, all keyed so the content team just fills rows):
    ///   FacultyResearch          - accordion items
    ///   FacultyResearchSections  - h3 sub-sections inside an item (ItemKey)
    ///   FacultyResearchSubItems  - h4 blocks inside a sub-section (SectionKey)
    ///   FacultyResearchTable     - table rows inside a sub-section (SectionKey)
    ///   FacultyResearchImages    - images for an item OR a sub-section (OwnerKey)
    /// Body text is stored plain; HTML is added here / in the markup.
    /// </summary>
    public partial class ucFacultyResearchDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class ImageDto
        {
            public string Url { get; set; }
            public string Alt { get; set; }
            /// <summary>col-12 for a lone image, col-12 col-md-6 for a gallery (as per design).</summary>
            public string ColClass { get; set; }
        }

        /// <summary>
        /// One table row. Cells are held as a list rather than named properties so
        /// the table renders whatever number of columns the sub-section declares in
        /// TableHeaders (up to five), and stays valid HTML for narrower tables.
        /// </summary>
        public class TableRowDto
        {
            public List<string> Cells { get; set; }
        }

        public class SubItemDto
        {
            public string Title { get; set; }
            public List<BodyBlock> Body { get; set; }
            public string LinkUrl { get; set; }
            public string LinkText { get; set; }
            public bool HasLink { get; set; }
        }

        public class SectionDto
        {
            public string SectionKey { get; set; }
            public string Title { get; set; }
            public bool HasTitle { get; set; }
            public List<BodyBlock> Body { get; set; }
            public List<SubItemDto> SubItems { get; set; }

            public List<string> TableHeaders { get; set; }
            public List<TableRowDto> TableRows { get; set; }
            public bool HasTable { get; set; }

            public List<ImageDto> Images { get; set; }
            public bool HasImages { get; set; }

            public string LinkUrl { get; set; }
            public string LinkText { get; set; }
            public bool HasLink { get; set; }
        }

        public class ItemDto
        {
            public string ItemKey { get; set; }
            public string Title { get; set; }
            public List<BodyBlock> Body { get; set; }
            public List<SectionDto> Sections { get; set; }
            public List<ImageDto> Images { get; set; }
            public bool HasImages { get; set; }
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
                ResearchProvisioner.EnsureLists();
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
            //ltrHeading.Text = IsArabic ? "البحث العلمي في الكلية" : "Scientific Research in the Faculty";
            //ltrIntro.Text = IsArabic ? "تضم الكلية وحدات ومراكز بحث وابتكار في مجالات الحوسبة والذكاء الاصطناعي والبيانات." : "The faculty includes research and innovation units and centers in computing, AI and data.";
        }

        private void BindItems(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ResearchProvisioner.ItemsListName);
            if (list == null) { phItems.Visible = false; return; }

            var imagesByKey = LoadImages(web);
            var subItemsByKey = LoadSubItems(web);
            var tableRowsByKey = LoadTableRows(web);
            var sectionsByItem = LoadSections(web, subItemsByKey, tableRowsByKey, imagesByKey);

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            var dtos = new List<ItemDto>();
            int idx = 1;
            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "ItemKey");
                bool expanded = idx == 1;
                var images = imagesByKey.ContainsKey(key) ? imagesByKey[key] : new List<ImageDto>();
                dtos.Add(new ItemDto
                {
                    ItemKey = key,
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Body = FacultyProvisioningHelper.ParseBody(IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr"))),
                    Sections = sectionsByItem.ContainsKey(key) ? sectionsByItem[key] : new List<SectionDto>(),
                    Images = images,
                    HasImages = images.Count > 0,
                    HeadingId = "faculty-researchAccordionHeading" + idx,
                    CollapseId = "faculty-researchAccordionCollapse" + idx,
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

        private Dictionary<string, List<SectionDto>> LoadSections(SPWeb web,
            Dictionary<string, List<SubItemDto>> subItemsByKey,
            Dictionary<string, List<TableRowDto>> tableRowsByKey,
            Dictionary<string, List<ImageDto>> imagesByKey)
        {
            var map = new Dictionary<string, List<SectionDto>>();
            SPList list = web.Lists.TryGetList(ResearchProvisioner.SectionsListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string itemKey = SafeString(it, "ItemKey");
                string sectionKey = SafeString(it, "SectionKey");
                string title = DisplayName(it, "Title", "TitleEn");

                var headers = new List<string>();
                string rawHeaders = SafeString(it, "TableHeaders");
                if (!string.IsNullOrEmpty(rawHeaders))
                    foreach (string h in rawHeaders.Split('|'))
                        if (!string.IsNullOrWhiteSpace(h)) headers.Add(h.Trim());

                string secUrl = SafeUrl(it, "LinkUrl");
                string secLinkText = SafeString(it, "LinkText");
                var rows = tableRowsByKey.ContainsKey(sectionKey) ? tableRowsByKey[sectionKey] : new List<TableRowDto>();
                var images = imagesByKey.ContainsKey(sectionKey) ? imagesByKey[sectionKey] : new List<ImageDto>();

                // Rows always carry five cells; keep only as many as this table
                // declares headers for, so a 3-column table stays 3 columns and the
                // cell count always matches the header count.
                if (headers.Count > 0)
                {
                    foreach (TableRowDto r in rows)
                    {
                        if (r.Cells == null) { r.Cells = new List<string>(); }
                        while (r.Cells.Count < headers.Count) r.Cells.Add("");
                        if (r.Cells.Count > headers.Count)
                            r.Cells = r.Cells.GetRange(0, headers.Count);
                    }
                }

                if (!map.ContainsKey(itemKey)) map[itemKey] = new List<SectionDto>();
                map[itemKey].Add(new SectionDto
                {
                    SectionKey = sectionKey,
                    Title = title,
                    HasTitle = !string.IsNullOrEmpty(title),
                    Body = FacultyProvisioningHelper.ParseBody(
                        IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr"))),
                    SubItems = subItemsByKey.ContainsKey(sectionKey) ? subItemsByKey[sectionKey] : new List<SubItemDto>(),
                    TableHeaders = headers,
                    TableRows = rows,
                    HasTable = headers.Count > 0 && rows.Count > 0,
                    Images = images,
                    HasImages = images.Count > 0,
                    LinkUrl = secUrl,
                    LinkText = string.IsNullOrEmpty(secLinkText) ? title : secLinkText,
                    HasLink = !string.IsNullOrEmpty(secUrl)
                });
            }
            return map;
        }

        private Dictionary<string, List<SubItemDto>> LoadSubItems(SPWeb web)
        {
            var map = new Dictionary<string, List<SubItemDto>>();
            SPList list = web.Lists.TryGetList(ResearchProvisioner.SubItemsListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "SectionKey");
                string url = SafeUrl(it, "LinkUrl");
                string linkText = SafeString(it, "LinkText");
                if (!map.ContainsKey(key)) map[key] = new List<SubItemDto>();
                map[key].Add(new SubItemDto
                {
                    Title = DisplayName(it, "Title", "TitleEn"),
                    Body = FacultyProvisioningHelper.ParseBody(
                        IsArabic ? SafeString(it, "BodyAr") : Fallback(SafeString(it, "BodyEn"), SafeString(it, "BodyAr"))),
                    LinkUrl = url,
                    LinkText = string.IsNullOrEmpty(linkText) ? DisplayName(it, "Title", "TitleEn") : linkText,
                    HasLink = !string.IsNullOrEmpty(url)
                });
            }
            return map;
        }

        private Dictionary<string, List<TableRowDto>> LoadTableRows(SPWeb web)
        {
            var map = new Dictionary<string, List<TableRowDto>>();
            SPList list = web.Lists.TryGetList(ResearchProvisioner.TableListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "SectionKey");
                if (!map.ContainsKey(key)) map[key] = new List<TableRowDto>();
                map[key].Add(new TableRowDto
                {
                    Cells = new List<string>
                    {
                        SafeString(it, "Title"),    // column 1
                        SafeString(it, "Col2"),
                        SafeString(it, "Col3"),
                        SafeString(it, "Col4"),
                        SafeString(it, "Col5")
                    }
                });
            }
            return map;
        }

        private Dictionary<string, List<ImageDto>> LoadImages(SPWeb web)
        {
            var map = new Dictionary<string, List<ImageDto>>();
            SPList list = web.Lists.TryGetList(ResearchProvisioner.ImagesListName);
            if (list == null) return map;

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string key = SafeString(it, "OwnerKey");
                string url = SafeUrl(it, "ImageUrl");
                if (string.IsNullOrEmpty(url)) continue;
                if (!map.ContainsKey(key)) map[key] = new List<ImageDto>();
                map[key].Add(new ImageDto { Url = url, Alt = SafeString(it, "AltText") });
            }

            // A lone image spans the row; a gallery goes two-up, matching the design.
            foreach (KeyValuePair<string, List<ImageDto>> pair in map)
            {
                string cls = pair.Value.Count > 1 ? "col-12 col-md-6" : "col-12";
                foreach (ImageDto img in pair.Value) img.ColClass = cls;
            }
            return map;
        }

        /// <summary>Binds body blocks, sub-sections and item images inside each accordion item.</summary>
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

            PlaceHolder phItemImages = e.Item.FindControl("phItemImages") as PlaceHolder;
            if (phItemImages != null) phItemImages.Visible = row.HasImages;
            Repeater rptImages = e.Item.FindControl("rptImages") as Repeater;
            if (rptImages != null) { rptImages.DataSource = row.Images; rptImages.DataBind(); }
        }

        /// <summary>Binds a sub-section: its body, h4 sub-items, table and images.</summary>
        protected void rptSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            SectionDto row = e.Item.DataItem as SectionDto;
            if (row == null) return;

            Repeater rptSectionBody = e.Item.FindControl("rptSectionBody") as Repeater;
            if (rptSectionBody != null) { rptSectionBody.DataSource = row.Body; rptSectionBody.DataBind(); }

            Repeater rptSubItems = e.Item.FindControl("rptSubItems") as Repeater;
            if (rptSubItems != null) { rptSubItems.DataSource = row.SubItems; rptSubItems.DataBind(); }

            PlaceHolder phTable = e.Item.FindControl("phTable") as PlaceHolder;
            if (phTable != null) phTable.Visible = row.HasTable;
            if (row.HasTable)
            {
                Repeater rptTableHead = e.Item.FindControl("rptTableHead") as Repeater;
                if (rptTableHead != null) { rptTableHead.DataSource = row.TableHeaders; rptTableHead.DataBind(); }
                Repeater rptTableRows = e.Item.FindControl("rptTableRows") as Repeater;
                if (rptTableRows != null) { rptTableRows.DataSource = row.TableRows; rptTableRows.DataBind(); }
            }

            PlaceHolder phSectionImages = e.Item.FindControl("phSectionImages") as PlaceHolder;
            if (phSectionImages != null) phSectionImages.Visible = row.HasImages;
            Repeater rptSectionImages = e.Item.FindControl("rptSectionImages") as Repeater;
            if (rptSectionImages != null) { rptSectionImages.DataSource = row.Images; rptSectionImages.DataBind(); }
        }

        /// <summary>Binds the cells of one table row (column count is data-driven).</summary>
        protected void rptTableRows_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            TableRowDto row = e.Item.DataItem as TableRowDto;
            if (row == null) return;

            Repeater rptCells = e.Item.FindControl("rptCells") as Repeater;
            if (rptCells != null) { rptCells.DataSource = row.Cells; rptCells.DataBind(); }
        }

        /// <summary>Binds the body blocks inside each h4 sub-item.</summary>
        protected void rptSubItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            SubItemDto row = e.Item.DataItem as SubItemDto;
            if (row == null) return;

            Repeater rptSubBody = e.Item.FindControl("rptSubBody") as Repeater;
            if (rptSubBody != null) { rptSubBody.DataSource = row.Body; rptSubBody.DataBind(); }
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
