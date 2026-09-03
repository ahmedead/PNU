using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterDocumentsDga : UserControl
    {
        [WebBrowsable(true),
         Category("List Settings"),
         DefaultValue("CenterDocuments"),
         Description("Name of the SharePoint list containing Center Documents.")]
        public string ListName { get; set; }

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class DocGroupDto
        {
            public string GroupTitle { get; set; }
            public string HeadingId { get; set; }
            public string CollapseId { get; set; }
            public List<DocItemDto> Items { get; set; }
        }

        public class DocItemDto
        {
            public string Name { get; set; }
            public string Url { get; set; }
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
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterDocumentsDga.Page_Load", ex.Message);
            }
        }

        private void BindDocuments()
        {
            string targetListName = string.IsNullOrWhiteSpace(ListName) ? "CenterDocuments" : ListName.Trim();
            SPWeb web = SPContext.Current.Web;
            CenterProvisioner.EnsureDocumentsList(web, targetListName);

            var groups = new List<DocGroupDto>();

            try
            {
                Guid siteId = web.Site.ID;
                Guid webId = web.ID;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var elevatedWeb = site.OpenWeb(webId))
                    {
                        SPList list = elevatedWeb.Lists.TryGetList(targetListName);
                        if (list != null && list.ItemCount > 0)
                        {
                            var grouped = new Dictionary<string, List<DocItemDto>>(StringComparer.OrdinalIgnoreCase);
                            foreach (SPListItem item in list.Items)
                            {
                                string nameAr = Convert.ToString(item["Title"] ?? "");
                                string nameEn = Convert.ToString(item["Title_EN"] ?? item["TitleEn"] ?? nameAr);
                                string name = IsArabic ? nameAr : nameEn;
                                if (string.IsNullOrEmpty(name)) continue;

                                string category = Convert.ToString(item["Category"] ?? (IsArabic ? "أدلة ونماذج عامة" : "General Guides and Forms"));
                                string url = "#";
                                if (item["URL"] != null || item["LinkUrl"] != null)
                                {
                                    var raw = item["URL"] ?? item["LinkUrl"];
                                    try
                                    {
                                        var uv = new SPFieldUrlValue(Convert.ToString(raw));
                                        url = uv.Url;
                                    }
                                    catch
                                    {
                                        url = Convert.ToString(raw);
                                    }
                                }

                                if (!grouped.ContainsKey(category))
                                    grouped[category] = new List<DocItemDto>();

                                grouped[category].Add(new DocItemDto { Name = name, Url = url });
                            }

                            int idx = 0;
                            foreach (var kvp in grouped)
                            {
                                groups.Add(new DocGroupDto
                                {
                                    GroupTitle = kvp.Key,
                                    HeadingId = "center-doc-group-heading-" + idx,
                                    CollapseId = "center-doc-group-collapse-" + idx,
                                    Items = kvp.Value
                                });
                                idx++;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterDocumentsDga.BindDocuments", ex.Message);
            }

            if (groups.Count == 0)
            {
                if (IsArabic)
                {
                    groups.Add(new DocGroupDto
                    {
                        GroupTitle = "الأدلة واللوائح التنظيمية",
                        HeadingId = "center-doc-group-heading-0",
                        CollapseId = "center-doc-group-collapse-0",
                        Items = new List<DocItemDto>
                        {
                            new DocItemDto { Name = "دليل الطالبة للخدمات والبرامج المهنية", Url = "#" },
                            new DocItemDto { Name = "لائحة الدعم الطلابي والمهني", Url = "#" }
                        }
                    });
                    groups.Add(new DocGroupDto
                    {
                        GroupTitle = "النماذج والاستمارات",
                        HeadingId = "center-doc-group-heading-1",
                        CollapseId = "center-doc-group-collapse-1",
                        Items = new List<DocItemDto>
                        {
                            new DocItemDto { Name = "نموذج التسجيل في برنامج التلمذة المهنية", Url = "#" },
                            new DocItemDto { Name = "استمارة طلب توثيق المهارات في سجل سموق", Url = "#" }
                        }
                    });
                }
                else
                {
                    groups.Add(new DocGroupDto
                    {
                        GroupTitle = "Regulations and Guides",
                        HeadingId = "center-doc-group-heading-0",
                        CollapseId = "center-doc-group-collapse-0",
                        Items = new List<DocItemDto>
                        {
                            new DocItemDto { Name = "Student Career Services Guide", Url = "#" }
                        }
                    });
                }
            }

            rptGroups.DataSource = groups;
            rptGroups.DataBind();
        }

        protected void rptGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DocGroupDto group = e.Item.DataItem as DocGroupDto;
                Repeater rptItems = e.Item.FindControl("rptItems") as Repeater;
                if (group != null && rptItems != null)
                {
                    rptItems.DataSource = group.Items;
                    rptItems.DataBind();
                }
            }
        }
    }
}
