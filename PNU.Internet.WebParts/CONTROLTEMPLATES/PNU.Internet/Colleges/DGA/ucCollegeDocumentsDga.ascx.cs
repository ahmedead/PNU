using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// Documents / Forms / Guides (المستندات والنماذج والأدلة) tab - DGA design.
    /// Markup lives in the .ascx; this only binds data.
    /// Data from the CollegeDocuments list (provisioned + seeded on the current
    /// web at page load). All accordion items render collapsed.
    /// </summary>
    public partial class ucCollegeDocumentsDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        /// <summary>Bound to the outer repeater.</summary>
        public class DocGroupDto
        {
            public string GroupTitle { get; set; }
            public string HeadingId { get; set; }
            public string CollapseId { get; set; }
            public List<DocItemDto> Items { get; set; }
        }

        /// <summary>Bound to the inner repeater.</summary>
        public class DocItemDto
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                SPContext.Current != null &&
                SPContext.Current.Web.CurrentUser != null)
            {
                CollegeDocumentsProvisioner.EnsureList();
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ltrHeading.Text = IsArabic
                    ? "المستندات والنماذج والأدلة"
                    : "Documents, Forms and Guides";

                if (!IsPostBack)
                    BindDocuments();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void BindDocuments()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(CollegeDocumentsProvisioner.ListName);
            if (list == null) { phDocuments.Visible = false; return; }

            // Flatten the list into a working set first
            var rows = new List<Tuple<string, string, string, double>>(); // category, name, url, sort
            foreach (SPListItem item in list.Items)
            {
                string nameAr = SafeString(item, "Title");
                string nameEn = SafeString(item, "TitleEn");
                string name = IsArabic
                    ? (!string.IsNullOrEmpty(nameAr) ? nameAr : nameEn)
                    : (!string.IsNullOrEmpty(nameEn) ? nameEn : nameAr);
                if (string.IsNullOrEmpty(name)) continue;

                string url = SafeUrl(item, "LinkUrl");
                if (string.IsNullOrEmpty(url)) url = "#";

                rows.Add(Tuple.Create(
                    SafeString(item, "Category"),
                    name,
                    url,
                    SafeDouble(item, "SortOrder")));
            }

            // Fixed group order matching the DGA template
            var order = new[]
            {
                new { Key = "Documents", Ar = "المستندات", En = "Documents" },
                new { Key = "Forms",     Ar = "النماذج",   En = "Forms" },
                new { Key = "Guides",    Ar = "الأدلة",     En = "Guides" }
            };

            var groups = new List<DocGroupDto>();
            int g = 0;
            foreach (var grp in order)
            {
                var items = rows
                    .Where(r => string.Equals(r.Item1, grp.Key, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(r => r.Item4)
                    .Select(r => new DocItemDto { Name = r.Item2, Url = r.Item3 })
                    .ToList();
                if (items.Count == 0) continue;

                g++;
                groups.Add(new DocGroupDto
                {
                    GroupTitle = IsArabic ? grp.Ar : grp.En,
                    HeadingId = "faculty-documentsAccordionHeading" + g,
                    CollapseId = "faculty-documentsAccordionCollapse" + g,
                    Items = items
                });
            }

            phDocuments.Visible = groups.Count > 0;
            rptGroups.DataSource = groups;
            rptGroups.DataBind();
        }

        /// <summary>Binds the inner card repeater for each accordion group.</summary>
        protected void rptGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DocGroupDto group = e.Item.DataItem as DocGroupDto;
            if (group == null) return;

            Repeater rptItems = e.Item.FindControl("rptItems") as Repeater;
            if (rptItems == null) return;

            rptItems.DataSource = group.Items;
            rptItems.DataBind();
        }

        // ---- helpers ----
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
