using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// DGA entity-details side menu (2 levels).
    ///
    /// Menu items live in the "CollegeSideMenu" list on the current college web:
    ///   - ParentID = 0  -> root item
    ///   - ParentID = ID of a root item -> level-2 item (rendered inside the collapsible group)
    /// Every item (root or child) may carry a ControlPath; each one gets its own tab pane
    /// and its user control is loaded inside it.
    ///
    /// ControlPath supports passing properties to the loaded control:
    ///   /_controltemplates/15/PNU.Internet/Colleges/ucCollegeContentSection.ascx?SectionKey=faculty-achievements
    /// Everything after '?' is applied to matching public properties via reflection.
    ///
    /// ItemType = "Sections": a single list entry. Its level-2 entries (the department
    /// names) are generated from code (AllFacultyDepartments on the Admin web) and all
    /// point to the same Sections pane, scrolling to anchors faculty-department-{i}.
    ///
    /// The list is provisioned from the page itself (idempotent) - no event receiver needed.
    /// </summary>
    public partial class ucSideMenu : UserControl
    {
        private const string TABS_ID = "faculty-details-tabs";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        #region Models

        private class SideMenuItem
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
            public string ControlPath { get; set; }
            public string ItemType { get; set; }
            public double ItemOrder { get; set; }
            public List<SideMenuItem> Children = new List<SideMenuItem>();
        }

        private class SectionEntry
        {
            public string Name { get; set; }
            public string Code { get; set; }      // DEPT_CODE -> passed as SectionKey
            public string AnchorId { get; set; }
        }

        #endregion

        // Dynamic controls MUST be re-created on every request (postbacks included).
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                if (HttpContext.Current != null
                && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    SideMenuProvisioner.EnsureList();   // idempotent, runs from the page
                }
                
                BuildMenu();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // ------------------------------------------------------------------
        //  Build
        // ------------------------------------------------------------------

        private void BuildMenu()
        {
            List<SideMenuItem> all = LoadMenuItems();
            if (all.Count == 0) return;

            // build tree
            List<SideMenuItem> roots = all.Where(x => x.ParentId == 0).OrderBy(x => x.ItemOrder).ToList();
            foreach (SideMenuItem root in roots)
                root.Children = all.Where(x => x.ParentId == root.Id).OrderBy(x => x.ItemOrder).ToList();

            string collegeCode = GetCollegeCode();

            StringBuilder nav = new StringBuilder();
            bool activePlaced = false;

            foreach (SideMenuItem root in roots)
            {
                string display = GetDisplayTitle(root);
                bool isSections = string.Equals(root.ItemType, "Sections", StringComparison.OrdinalIgnoreCase);

                if (isSections)
                {
                    List<SectionEntry> sections = LoadSections(collegeCode);

                    // Group header is a collapse toggler only - the departments themselves are the tabs.
                    RenderGroupHeader(nav, root, display, sections.Count, active: false, hasOwnPane: false);
                    nav.AppendFormat("<div class=\"toc-sub collapse\" id=\"menu-sub-{0}\">", root.Id);

                    int s = 0;
                    foreach (SectionEntry sec in sections)
                    {
                        bool secActive = !activePlaced && s == 0 && root == roots[0];
                        string paneKey = root.Id + "s" + s;   // faculty-pane-{Id}s{index}

                        // Each department is a REAL tab with its OWN pane; the pane loads
                        // root.ControlPath (ucCollegeSectionDetails.ascx) with SectionKey=DEPT_CODE.
                        nav.AppendFormat(
                            "<button type=\"button\" class=\"toc-item text-start border-0 bg-transparent{0}\" " +
                            "data-bs-toggle=\"tab\" data-bs-target=\"#faculty-pane-{1}\" role=\"tab\" " +
                            "aria-selected=\"{2}\"><span>{3}</span></button>",
                            secActive ? " active" : "", paneKey,
                            secActive ? "true" : "false", HttpUtility.HtmlEncode(sec.Name));

                        string path = AppendQuery(root.ControlPath, "SectionKey=" + HttpUtility.UrlEncode(sec.Code));
                        AddTabPane(paneKey, path, secActive);
                        if (secActive) { SetCurrentLabel(sec.Name); activePlaced = true; }
                        s++;
                    }
                    nav.Append("</div></div>");
                }
                else if (root.Children.Count > 0)
                {
                    bool active = !activePlaced && !string.IsNullOrEmpty(root.ControlPath);

                    RenderGroupHeader(nav, root, display, root.Children.Count, active,
                                      hasOwnPane: !string.IsNullOrEmpty(root.ControlPath));

                    nav.AppendFormat("<div class=\"toc-sub collapse\" id=\"menu-sub-{0}\">", root.Id);
                    foreach (SideMenuItem child in root.Children)
                    {
                        bool childActive = !activePlaced && string.IsNullOrEmpty(root.ControlPath) &&
                                           child == root.Children[0] && root == roots[0];

                        // Each level-2 item is a REAL tab trigger with its OWN pane
                        nav.AppendFormat(
                            "<button type=\"button\" class=\"toc-item text-start border-0 bg-transparent{0}\" " +
                            "data-bs-toggle=\"tab\" data-bs-target=\"#faculty-pane-{1}\" role=\"tab\" " +
                            "aria-selected=\"{2}\"><span>{3}</span></button>",
                            childActive ? " active" : "", child.Id,
                            childActive ? "true" : "false", HttpUtility.HtmlEncode(GetDisplayTitle(child)));

                        AddTabPane(child, childActive);
                        if (childActive) { SetCurrentLabel(GetDisplayTitle(child)); activePlaced = true; }
                    }
                    nav.Append("</div></div>");

                    if (!string.IsNullOrEmpty(root.ControlPath))
                    {
                        AddTabPane(root, active);
                        if (active) { SetCurrentLabel(display); activePlaced = true; }
                    }
                }
                else
                {
                    bool active = !activePlaced;
                    nav.AppendFormat(
                        "<button class=\"nav-link{0} text-start text-nowrap\" id=\"faculty-tab-{1}\" " +
                        "data-bs-toggle=\"tab\" data-bs-target=\"#faculty-pane-{1}\" type=\"button\" role=\"tab\" " +
                        "aria-controls=\"faculty-pane-{1}\" aria-selected=\"{2}\">{3}</button>",
                        active ? " active" : "", root.Id,
                        active ? "true" : "false", HttpUtility.HtmlEncode(display));

                    AddTabPane(root, active);
                    if (active) { SetCurrentLabel(display); activePlaced = true; }
                }
            }

            ltrNav.Text = nav.ToString();
        }

        private void RenderGroupHeader(StringBuilder nav, SideMenuItem root, string display,
                                       int badgeCount, bool active, bool hasOwnPane)
        {
            nav.Append("<div class=\"entity-details-sidemenu-group d-flex flex-column align-items-stretch\">");

            string tabAttrs = hasOwnPane
                ? string.Format("data-bs-toggle=\"tab\" data-bs-target=\"#faculty-pane-{0}\" role=\"tab\" aria-selected=\"{1}\"",
                                root.Id, active ? "true" : "false")
                : "";

            nav.AppendFormat(
                "<button class=\"nav-link{0} text-start has-rotatable-icon collapsed\" id=\"faculty-tab-{1}\" " +
                "type=\"button\" {2} aria-expanded=\"false\" data-entity-menu-target=\"#menu-sub-{1}\">" +
                "<span class=\"flex-grow-1\">{3}</span>" +
                "<span class=\"badge text-bg-light rounded-pill\">{4}</span>" +
                "<i class=\"hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition\" aria-hidden=\"true\"></i>" +
                "</button>",
                active ? " active" : "", root.Id, tabAttrs, HttpUtility.HtmlEncode(display), badgeCount);
        }

        /// <summary>
        /// Creates the tab pane and loads the item's user control inside it.
        /// ClientIDMode.Static is CRITICAL: without it ASP.NET prefixes the rendered id
        /// (ctl00_..._faculty-pane-x) and it no longer matches data-bs-target, which is
        /// exactly why panes were not switching to "show active".
        /// </summary>
        private void AddTabPane(SideMenuItem item, bool active)
        {
            AddTabPane(item.Id.ToString(), item.ControlPath, active);
        }

        private void AddTabPane(string paneKey, string controlPath, bool active)
        {
            if (string.IsNullOrEmpty(controlPath)) return;

            HtmlGenericControl pane = new HtmlGenericControl("div");
            pane.ID = "faculty-pane-" + paneKey;
            pane.ClientIDMode = ClientIDMode.Static;
            pane.Attributes["class"] = "tab-pane fade" + (active ? " show active" : "");
            pane.Attributes["role"] = "tabpanel";
            pane.Attributes["aria-labelledby"] = "faculty-tab-" + paneKey;
            pane.Attributes["tabindex"] = "0";

            try
            {
                string path = controlPath;
                string query = "";
                int q = path.IndexOf('?');
                if (q >= 0)
                {
                    query = path.Substring(q + 1);
                    path = path.Substring(0, q);
                }

                Control child = Page.LoadControl(path);
                ApplyProperties(child, query);
                pane.Controls.Add(child);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucSideMenu.LoadControl: " + controlPath, ex.Message);
            }

            phPanes.Controls.Add(pane);
        }

        /// <summary>Appends a query pair to a ControlPath ("path?a=1" + "b=2" -> "path?a=1&amp;b=2").</summary>
        private static string AppendQuery(string controlPath, string pair)
        {
            if (string.IsNullOrEmpty(controlPath)) return controlPath;
            return controlPath + (controlPath.IndexOf('?') >= 0 ? "&" : "?") + pair;
        }

        /// <summary>Applies "Prop=Value&amp;Prop2=Value2" pairs to public properties of the control.</summary>
        private static void ApplyProperties(Control target, string query)
        {
            if (string.IsNullOrEmpty(query)) return;

            foreach (string pair in query.Split('&'))
            {
                string[] kv = pair.Split('=');
                if (kv.Length != 2) continue;

                PropertyInfo prop = target.GetType().GetProperty(kv[0],
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null || !prop.CanWrite) continue;

                object value = HttpUtility.UrlDecode(kv[1]);
                if (prop.PropertyType == typeof(int)) value = Convert.ToInt32(value);
                else if (prop.PropertyType == typeof(bool)) value = Convert.ToBoolean(value);
                prop.SetValue(target, value, null);
            }
        }

        private void SetCurrentLabel(string text)
        {
            spnCurrentItem.InnerText = text;
        }

        // ------------------------------------------------------------------
        //  Data
        // ------------------------------------------------------------------

        private List<SideMenuItem> LoadMenuItems()
        {
            List<SideMenuItem> result = new List<SideMenuItem>();

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(SideMenuProvisioner.LIST_NAME);
                if (list == null) return result;

                SPQuery query = new SPQuery
                {
                    Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                              <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                };

                foreach (SPListItem item in list.GetItems(query))
                {
                    result.Add(new SideMenuItem
                    {
                        Id = item.ID,
                        ParentId = item.Fields.ContainsField("ParentID") && item["ParentID"] != null
                                   ? Convert.ToInt32(Convert.ToDouble(item["ParentID"])) : 0,
                        Title = Convert.ToString(item["Title"]),
                        TitleEn = item.Fields.ContainsField("Title_EN") ? Convert.ToString(item["Title_EN"]) : "",
                        ControlPath = item.Fields.ContainsField("ControlPath")
                                   ? Convert.ToString(item["ControlPath"]).Trim() : "",
                        ItemType = item.Fields.ContainsField("ItemType") ? Convert.ToString(item["ItemType"]) : "Control",
                        ItemOrder = item.Fields.ContainsField("ItemOrder") && item["ItemOrder"] != null
                                   ? Convert.ToDouble(item["ItemOrder"]) : 0
                    });
                }
            }

            return result;
        }

        private string GetCollegeCode()
        {
            try
            {
                if (Page.Request.QueryString["Source"] != null)
                    return Page.Request.QueryString["Source"].ToString().Trim();

                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList("AboutCollege");
                    if (list != null)
                    {
                        SPListItemCollection collitem = list.GetItems();
                        if (collitem != null && collitem.Count > 0 && collitem[0]["College_Code"] != null)
                            return collitem[0]["College_Code"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucSideMenu.GetCollegeCode", ex.Message);
            }
            return "";
        }

        private List<SectionEntry> LoadSections(string collegeCode)
        {
            List<SectionEntry> result = new List<SectionEntry>();
            if (string.IsNullOrEmpty(collegeCode)) return result;

            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb("Admin"))
                {
                    SPList deptList = web.Lists.TryGetList("AllFacultyDepartments");
                    if (deptList == null) return result;

                    SPQuery query = new SPQuery
                    {
                        Query = @"<Where>
                                     <Eq>
                                        <FieldRef Name='COLL_CODE' />
                                        <Value Type='Text'>" + collegeCode + @"</Value>
                                     </Eq>
                                  </Where>"
                    };

                    int index = 0;
                    foreach (SPListItem dept in deptList.GetItems(query))
                    {
                        string nameAr = GetFirstFieldValue(dept, "DEPT_NAME", "NAME", "Title");
                        string nameEn = GetFirstFieldValue(dept, "DEPT_NAME_EN", "ENG_NAME", "NAME_EN", "Title_EN");

                        string display = IsArabic
                            ? (!string.IsNullOrEmpty(nameAr) ? nameAr : nameEn)
                            : (!string.IsNullOrEmpty(nameEn) ? nameEn : nameAr);

                        if (string.IsNullOrEmpty(display)) continue;

                        string deptCode = GetFirstFieldValue(dept, "DEPT_CODE", "Code");

                        result.Add(new SectionEntry
                        {
                            Name = display,
                            Code = deptCode,
                            AnchorId = "faculty-department-" + index
                        });
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucSideMenu.LoadSections", ex.Message);
            }

            return result;
        }

        internal static string GetFirstFieldValue(SPListItem item, params string[] internalNames)
        {
            foreach (string name in internalNames)
            {
                if (item.Fields.ContainsField(name) && item[name] != null)
                {
                    string val = item[name].ToString().Trim();
                    if (!string.IsNullOrEmpty(val)) return val;
                }
            }
            return "";
        }

        private string GetDisplayTitle(SideMenuItem item)
        {
            if (IsArabic) return item.Title;
            return string.IsNullOrEmpty(item.TitleEn) ? item.Title : item.TitleEn;
        }
    }


}
