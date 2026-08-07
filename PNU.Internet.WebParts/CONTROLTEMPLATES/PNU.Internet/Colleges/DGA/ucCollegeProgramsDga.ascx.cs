using Microsoft.SharePoint;
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
    /// Programs (البرامج) tab - DGA design.
    /// Degree groups come from the EXISTING lists: NewStudyPlan (Arabic) /
    /// NewStudyPlan_EN (English) filtered by COLLEGE_CODE, and the program record
    /// from AllDepartmentPrograms by Code = PROGRAM.
    ///
    /// A final "الأدلة" (Guides) group is appended from the ProgramsDocuments list,
    /// which is provisioned + seeded on the current web at page load.
    ///
    /// Markup lives in the .ascx; this only binds data. All groups render collapsed.
    /// </summary>
    public partial class ucCollegeProgramsDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        private class ProgramEntry
        {
            public string Name;
            public string Code;
            public string Degree;   // grouping key
        }

        /// <summary>Bound to the outer repeater.</summary>
        public class ProgramGroupDto
        {
            public string GroupTitle { get; set; }
            public string HeadingId { get; set; }
            public string CollapseId { get; set; }
            public List<ProgramItemDto> Items { get; set; }
        }

        /// <summary>Bound to the inner repeater.</summary>
        public class ProgramItemDto
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string IconClass { get; set; }
            public string LinkTarget { get; set; }
            public string LinkRel { get; set; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Only authenticated users trigger provisioning. Anonymous page views
            // should never cause schema changes - by the time the site is public
            // the list already exists, and LoadGuides reads it elevated anyway.
            if (HttpContext.Current != null
                && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ProgramsDocumentsProvisioner.EnsureList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ltrHeading.Text = IsArabic ? "البرامج" : "Programs";

                if (!IsPostBack)
                    BindPrograms();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void BindPrograms()
        {
            var groups = new List<ProgramGroupDto>();
            int g = 0;

            // ---- degree groups from the existing study-plan lists ----
            foreach (var group in LoadPrograms().GroupBy(p => p.Degree))
            {
                g++;
                groups.Add(new ProgramGroupDto
                {
                    GroupTitle = group.Key,
                    HeadingId = "faculty-programsAccordionHeading" + g,
                    CollapseId = "faculty-programsAccordionCollapse" + g,
                    Items = group.Select(p => new ProgramItemDto
                    {
                        Name = p.Name,
                        Url = "ProgramDetails.aspx?ProgramCode=" + p.Code,
                        IconClass = "hgi-diploma",
                        LinkTarget = "_self",
                        LinkRel = ""
                    }).ToList()
                });
            }

            // ---- guides group from ProgramsDocuments ----
            var guides = LoadGuides();
            if (guides.Count > 0)
            {
                g++;
                groups.Add(new ProgramGroupDto
                {
                    GroupTitle = IsArabic ? "الأدلة" : "Guides",
                    HeadingId = "faculty-programsAccordionHeading" + g,
                    CollapseId = "faculty-programsAccordionCollapse" + g,
                    Items = guides
                });
            }

            phPrograms.Visible = groups.Count > 0;
            rptGroups.DataSource = groups;
            rptGroups.DataBind();
        }

        private List<ProgramEntry> LoadPrograms()
        {
            List<ProgramEntry> programs = new List<ProgramEntry>();

            string collegeCode = GetCollegeCode();
            if (string.IsNullOrEmpty(collegeCode)) return programs;

            // Elevated: anonymous users have no rights on the Admin web, and
            // SPWeb.Lists is permission-trimmed, so an unelevated read returns
            // nothing for them. Reopen site/web by ID INSIDE the delegate -
            // reusing the outer objects would keep the original user token.
            Guid siteId = SPContext.Current.Site.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = site.OpenWeb("Admin"))
                {
                    string planListName = IsArabic ? "NewStudyPlan" : "NewStudyPlan_EN";
                    SPList planList = web.Lists.TryGetList(planListName) ?? web.Lists.TryGetList("NewStudyPlan");
                    SPList progList = web.Lists.TryGetList("AllDepartmentPrograms");
                    if (planList == null || progList == null) return;

                    SPQuery query = new SPQuery
                    {
                        Query = @"<Where>
                                 <Eq>
                                    <FieldRef Name='COLLEGE_CODE' />
                                    <Value Type='Text'>" + collegeCode + @"</Value>
                                 </Eq>
                              </Where>"
                    };

                    HashSet<string> seen = new HashSet<string>();
                    foreach (SPListItem plan in planList.GetItems(query))
                    {
                        string programCode = ucSideMenu.GetFirstFieldValue(plan, "PROGRAM", "PROGRAM_CODE");
                        if (string.IsNullOrEmpty(programCode) || !seen.Add(programCode)) continue;

                        SPQuery pq = new SPQuery
                        {
                            Query = @"<Where>
                                     <Eq>
                                        <FieldRef Name='Code' />
                                        <Value Type='Text'>" + programCode + @"</Value>
                                     </Eq>
                                  </Where>",
                            RowLimit = 1
                        };

                        foreach (SPListItem prog in progList.GetItems(pq))
                        {
                            string nameAr = ucSideMenu.GetFirstFieldValue(prog, "PROG_NAME", "NAME", "Title");
                            string nameEn = ucSideMenu.GetFirstFieldValue(prog, "PROG_NAME_EN", "ENG_NAME", "NAME_EN", "Title_EN");
                            string name = IsArabic
                                ? (!string.IsNullOrEmpty(nameAr) ? nameAr : nameEn)
                                : (!string.IsNullOrEmpty(nameEn) ? nameEn : nameAr);
                            if (string.IsNullOrEmpty(name)) continue;

                            string degree = ucSideMenu.GetFirstFieldValue(prog, "DEGREE", "DEG_NAME", "DegreeType");

                            programs.Add(new ProgramEntry
                            {
                                Name = name,
                                Code = programCode,
                                Degree = ResolveDegreeGroup(degree, programCode)
                            });
                        }
                    }
                }
            });

            return programs;
        }

        private List<ProgramItemDto> LoadGuides()
        {
            var guides = new List<ProgramItemDto>();

            try
            {
                // Elevated: SPWeb.Lists is permission-trimmed, so anonymous users
                // get null back from TryGetList even when the list exists.
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId = SPContext.Current.Web.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite eSite = new SPSite(siteId))
                    using (SPWeb web = eSite.OpenWeb(webId))
                    {
                        SPList list = web.Lists.TryGetList(ProgramsDocumentsProvisioner.ListName);
                        if (list == null) return;

                        var items = new List<SPListItem>();
                        foreach (SPListItem it in list.Items) items.Add(it);

                        foreach (SPListItem it in items.OrderBy(x => SafeDouble(x, "SortOrder")))
                        {
                            string nameAr = SafeString(it, "Title");
                            string nameEn = SafeString(it, "TitleEn");
                            string name = IsArabic
                                ? (!string.IsNullOrEmpty(nameAr) ? nameAr : nameEn)
                                : (!string.IsNullOrEmpty(nameEn) ? nameEn : nameAr);
                            if (string.IsNullOrEmpty(name)) continue;

                            string url = SafeUrl(it, "LinkUrl");
                            if (string.IsNullOrEmpty(url)) url = "#";

                            guides.Add(new ProgramItemDto
                            {
                                Name = name,
                                Url = url,
                                IconClass = "hgi-diploma",
                                LinkTarget = "_blank",
                                LinkRel = "noopener noreferrer"
                            });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucCollegeProgramsDga.LoadGuides", ex.Message);
            }

            return guides;
        }

        /// <summary>Binds the inner card repeater for each accordion group.</summary>
        protected void rptGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            ProgramGroupDto group = e.Item.DataItem as ProgramGroupDto;
            if (group == null) return;

            Repeater rptItems = e.Item.FindControl("rptItems") as Repeater;
            if (rptItems == null) return;

            rptItems.DataSource = group.Items;
            rptItems.DataBind();
        }

        /// <summary>
        /// Maps the degree field (or the program code convention xx-BS-xxxx / xx-MS-xxxx)
        /// to the DGA group heading.
        /// </summary>
        private string ResolveDegreeGroup(string degree, string programCode)
        {
            string d = (degree ?? "").ToUpperInvariant();
            string c = (programCode ?? "").ToUpperInvariant();

            bool master = d.Contains("MASTER") || d.Contains("ماجستير") || c.Contains("-MS-");
            bool bachelor = d.Contains("BACH") || d.Contains("بكالوريوس") || c.Contains("-BS-");

            if (master) return IsArabic ? "برامج الماجستير" : "Master Programs";
            if (bachelor) return IsArabic ? "برامج البكالوريوس" : "Bachelor Programs";
            return !string.IsNullOrEmpty(degree) ? degree : (IsArabic ? "برامج أخرى" : "Other Programs");
        }

        private string GetCollegeCode()
        {
            try
            {
                if (Page.Request.QueryString["Source"] != null)
                    return Page.Request.QueryString["Source"].ToString().Trim();

                // Elevated - AboutCollege is not readable by anonymous users, and
                // returning "" here would make LoadPrograms bail out silently.
                string code = "";
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId = SPContext.Current.Web.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        SPList list = web.Lists.TryGetList("AboutCollege");
                        if (list != null)
                        {
                            SPListItemCollection collitem = list.GetItems();
                            if (collitem != null && collitem.Count > 0 && collitem[0]["College_Code"] != null)
                                code = collitem[0]["College_Code"].ToString().Trim();
                        }
                    }
                });

                return code;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucCollegeProgramsDga.GetCollegeCode", ex.Message);
            }
            return "";
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