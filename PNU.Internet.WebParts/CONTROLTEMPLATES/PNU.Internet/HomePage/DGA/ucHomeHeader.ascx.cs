using Microsoft.IdentityModel.Web;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using Portal.Main.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeHeader : UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            FixCanonicalLink();
            InjectMetaDescription();
        }
        private void InjectMetaDescription()
        {
            try
            {
                if (Page.Header == null) return;

                string desc = GetPageDescription();
                if (string.IsNullOrWhiteSpace(desc)) return;

                Page.Header.Controls.Add(new LiteralControl(string.Format(
                    "<meta name=\"description\" content=\"{0}\" />",
                    HttpUtility.HtmlEncode(desc))));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeHeader.InjectMetaDescription", ex.Message);
            }
        }

        private string GetPageDescription()
        {
            try
            {
                var item = SPContext.Current.ListItem;
                if (item != null)
                {
                    // 1. SEO Properties field (Page tab → Edit SEO Properties → Meta Description)
                    string seo = GetFieldValue(item, "SeoMetaDescription");
                    if (!string.IsNullOrWhiteSpace(seo))
                        return Publics.TruncateText(seo, 160);

                    // 2. Publishing page Description field
                    string comments = GetFieldValue(item, "Comments");
                    if (!string.IsNullOrWhiteSpace(comments))
                        return Publics.TruncateText(comments, 160);

                    //// 3. Fallback: page title + site-wide sentence
                    //string title = GetFieldValue(item, "Title");
                    //if (!string.IsNullOrWhiteSpace(title))
                    //    return Publics.TruncateText(
                    //        title + " - " + GetDefaultDescription(), 160);

                    // 3. Fallback: current browser title + site-wide sentence
                    string title = GetCurrentBrowserTitle();
                    if (!string.IsNullOrWhiteSpace(title))
                        return Publics.TruncateText(title + " - " + GetDefaultDescription(), 160);
                }

                // 4. Non-publishing pages: site-wide default
                return GetDefaultDescription();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeHeader.GetPageDescription", ex.Message);
                return string.Empty;
            }
        }

        private string GetFieldValue(SPListItem item, string internalName)
        {
            return item.Fields.ContainsField(internalName) && item[internalName] != null
                ? item[internalName].ToString().Trim()
                : string.Empty;
        }

        private string GetDefaultDescription()
        {
            // the current site-wide text from the master, AR/EN aware
            return SPContext.Current.Web.Locale.LCID == 1025
                ? "الموقع الرسمي لجامعة الأميرة نورة بنت عبد الرحمن في المملكة العربية السعودية، ويقدّم معلومات وخدمات الجامعة وأحدث الأخبار والإعلانات."
                : "The official website of Princess Nourah bint Abdulrahman University in Saudi Arabia, providing university information, services, news and announcements.";
        }

        private string GetCurrentBrowserTitle()
        {
            try
            {
                // 1. Dynamic title set by details controls this request
                var dynamic = HttpContext.Current.Items["PNU_BrowserTitle"] as string;
                if (!string.IsNullOrWhiteSpace(dynamic)) return dynamic;

                // 2. Anything set via Page.Title
                if (!string.IsNullOrWhiteSpace(Page.Title)) return Page.Title;

                // 3. Static publishing page title
                var item = SPContext.Current.ListItem;
                if (item != null) return GetFieldValue(item, "Title");

                return string.Empty;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeHeader.GetCurrentBrowserTitle", ex.Message);
                return string.Empty;
            }
        }

        private static bool DetectArabic()
        {
            try
            {
                int lcid = Thread.CurrentThread.CurrentUICulture.LCID;
                if (lcid == 1025) return true;
                if (lcid == 1033) return false;
            }
            catch { }
            return true;
        }
        protected bool IsArabic { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsArabic = DetectArabic();
            try
            {
                if (!Page.IsPostBack)
                {
                    string currentPath = Request.Url.PathAndQuery;

                    // Generate the new URLs
                    string arabicUrl = currentPath.Replace("/en/", "/ar/");
                    string englishUrl = currentPath.Replace("/ar/", "/en/");

                    // Assign to your buttons (add ID to your tags in HTML)
                    ArabicLink.HRef = arabicUrl;
                    EnglishLink.HRef = englishUrl;


                    img2.Attributes["src"] = SPFactory.GetPNUresResource("LogoOnly");
                    aLinkHome.Attributes.Add("href", SPFactory.GetSiteURL());

                    LoadMenu();
                }

                Publics.AddVisitorsCount(HttpContext.Current.Request.Url.ToString(), this.Page.Title);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void LoadMenu()
        {
            try
            {
                string MenuLevel1 = "TopMenuLevel1";
                string MenuLevel2 = "TopMenuLevel2";
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList mainLlist = null;
                        string Parentweb = web.Url + "/";
                        if (Parentweb.Contains(site.Url))
                        {
                            Parentweb = Parentweb.Replace(site.Url, "");
                        }

                        while (mainLlist == null)
                        {
                            try
                            {
                                using (SPWeb Pweb = site.OpenWeb(Parentweb))
                                {
                                    mainLlist = Pweb.Lists.TryGetList(MenuLevel1);
                                    if (mainLlist != null)
                                    {
                                        SPQuery query = new SPQuery()
                                        {
                                            Query = $@"<Where><Eq><FieldRef Name='Visibility' /><Value Type='Boolean'>1</Value></Eq></Where>
                                                      <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /></OrderBy>"
                                        };
                                        SPListItemCollection sPListItemCollection = mainLlist.GetItems(query);
                                        List<TopMenuLevel1> _Level1 = new List<TopMenuLevel1>();
                                        List<TopMenuLevel2> _Level2 = new List<TopMenuLevel2>();
                                        List<TopMenuLevel3> _Level3 = new List<TopMenuLevel3>();
                                        _Level1 = SPFactory.MapListItemsToClass<TopMenuLevel1>(sPListItemCollection);

                                        for (int i = 0; i < _Level1.Count; i++)
                                        {
                                            _Level1[i].LVL2 = new List<TopMenuLevel2>();
                                            query = new SPQuery()
                                            {
                                                Query = $@"<Where>
                                                              <And>
                                                                 <Eq>
                                                                    <FieldRef Name='Parent' />
                                                                    <Value Type='Lookup'>{_Level1[i].Title}</Value>
                                                                 </Eq>
                                                                 <Eq>
                                                                    <FieldRef Name='Visibility' />
                                                                    <Value Type='Boolean'>1</Value>
                                                                 </Eq>
                                                              </And>
                                                           </Where>
                                                           <OrderBy>
                                                              <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                                           </OrderBy>"
                                            };
                                            SPList Sublist = Pweb.Lists.TryGetList(MenuLevel2);
                                            if (Sublist == null) continue;

                                            SPListItemCollection sPListItemCollectionLVL2 = Sublist.GetItems(query);
                                            _Level2 = SPFactory.MapListItemsToClass<TopMenuLevel2>(sPListItemCollectionLVL2);
                                            _Level1[i].LVL2 = new List<TopMenuLevel2>();

                                            if (_Level2 != null && _Level2.Count > 0)
                                            {
                                                _Level1[i].LVL2 = _Level2;

                                                for (int j = 0; j < _Level2.Count; j++)
                                                {
                                                    SPQuery Subquery = new SPQuery()
                                                    {
                                                        Query = $@"<Where>
                                                              <And>
                                                                 <Eq>
                                                                    <FieldRef Name='Parent' />
                                                                    <Value Type='Lookup'>{_Level2[j].Title}</Value>
                                                                 </Eq>
                                                                 <Eq>
                                                                    <FieldRef Name='Visibility' />
                                                                    <Value Type='Boolean'>1</Value>
                                                                 </Eq>
                                                              </And>
                                                           </Where>
                                                           <OrderBy>
                                                              <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                                           </OrderBy>"
                                                    };
                                                    string MenuLevel3 = "TopMenuLevel3";
                                                    SPList SubSublist = Pweb.Lists.TryGetList(MenuLevel3);
                                                    if (SubSublist != null)
                                                    {
                                                        SPListItemCollection sPListItemCollectionLVL3 = SubSublist.GetItems(Subquery);
                                                        _Level3 = SPFactory.MapListItemsToClass<TopMenuLevel3>(sPListItemCollectionLVL3);
                                                        _Level2[j].LVL3 = new List<TopMenuLevel3>();
                                                        _Level2[j].LVL3 = _Level3;
                                                    }
                                                }
                                            }

                                            _Level1[i].LVL2 = _Level2;
                                        }

                                        // Build flattened model (same pattern as old ucHeader)
                                        List<MenuLevel1> menuList = new List<MenuLevel1>();
                                        foreach (var level1 in _Level1)
                                        {
                                            MenuLevel1 menu1 = new MenuLevel1
                                            {
                                                Title = level1.Title,
                                                URL = level1.URL,
                                                LVL2 = new List<MenuLevel2>(),
                                                LVL3 = new List<MenuLevel3>()
                                            };
                                            if (level1.LVL2 != null && level1.LVL2.Count > 0)
                                            {
                                                foreach (var level2 in level1.LVL2)
                                                {
                                                    MenuLevel2 menu2 = new MenuLevel2
                                                    {
                                                        Title = level2.Title,
                                                        URL = level2.URL
                                                    };
                                                    menu1.LVL2.Add(menu2);

                                                    if (level2.LVL3 != null && level2.LVL3.Count > 0)
                                                    {
                                                        foreach (var level3 in level2.LVL3)
                                                        {
                                                            MenuLevel3 menu3 = new MenuLevel3
                                                            {
                                                                Title = level3.Title,
                                                                URL = level3.URL,
                                                                ParentTitle = level2.Title
                                                            };
                                                            menu1.LVL3.Add(menu3);
                                                        }
                                                    }
                                                }
                                            }
                                            menuList.Add(menu1);
                                        }

                                        // Render desktop + mobile using the new DGA design
                                        Get_MenuHTML_NewDesign(menuList);
                                        Get_MenuHTML(_Level1);
                                    }

                                    Parentweb = Pweb.ParentWeb.Url + "/";
                                    if (Parentweb.Contains(site.Url))
                                    {
                                        Parentweb = Parentweb.Replace(site.Url, "");
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }

                        if (mainLlist == null)
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        public string GetDynamicURL1(string URL)
        {
            try
            {
                return String.Format("{0}{1}", SPFactory.GetSiteURL(), URL);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Builds the DESKTOP navigation using the new DGA design classes.
        /// Output shape (per Level 1 with children):
        ///   <li class="nav-item dropdown">
        ///     <a ...>Title <i hgi-arrow-down-01/></a>
        ///     <ul class="dropdown-menu">
        ///       <li role="presentation">
        ///         <div class="container">
        ///           <ul class="list-unstyled w-100 d-flex m-auto flex-wrap">
        ///             [per Level 2] <li class="flex-fill list-unstyled"> ... <ul class="list-unstyled text-start"> [Level 3] </ul> </li>
        ///           </ul>
        ///         </div>
        ///       </li>
        ///     </ul>
        ///   </li>
        /// </summary>
        public void Get_MenuHTML_NewDesign(List<MenuLevel1> _AllData)
        {
            string HTML = "";

            if (_AllData != null && _AllData.Count > 0)
            {
                int lvl1Counter = 0;
                foreach (MenuLevel1 item in _AllData)
                {
                    lvl1Counter++;
                    string dropdownId = $"navDropdown{lvl1Counter}";
                    string menuId = $"navMenu{lvl1Counter}";

                    if (item.LVL2 != null && item.LVL2.Count > 0)
                    {
                        // Has Level 3 under Level 2 -> grouped submenu (column-per-group layout)
                        if (item.LVL3 != null && item.LVL3.Count > 0)
                        {
                            string LVL1 = $@"<li class=""nav-item dropdown"">
                                <a id=""{dropdownId}"" role=""button"" data-bs-toggle=""dropdown"" data-bs-auto-close=""outside""
                                   class=""nav-link position-relative p-0 px-2 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium""
                                   aria-expanded=""false"" aria-haspopup=""true"" aria-controls=""{menuId}"" href=""#"">
                                    <span class=""px-2 nav-link-text text-nowrap"">{item.Title}</span>
                                    <span class=""d-inline-flex fs-5"">
                                        <i class=""nav-link-icon rotation-transition hgi hgi-stroke hgi-arrow-down-01"" aria-hidden=""true""></i>
                                    </span>
                                </a>
                                <ul class=""dropdown-menu"" id=""{menuId}"" aria-labelledby=""{dropdownId}"">
                                    <li role=""presentation"">
                                        <div class=""container"">
                                            <ul class=""list-unstyled w-100 d-flex m-auto flex-wrap"">
                                                @LVL2
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                              </li>";

                            string LVL2 = "";
                            foreach (MenuLevel2 item2 in item.LVL2)
                            {
                                List<MenuLevel3> itemsLVL3 = item.LVL3
                                    .Where(e => e != null && e.ParentTitle == item2.Title)
                                    .ToList();

                                if (itemsLVL3 != null && itemsLVL3.Count > 0)
                                {
                                    LVL2 += $@"<li class=""flex-fill list-unstyled"">
                                                <div>
                                                    <span class=""dga-submenutitle text-decoration-none fw-semibold pe-none text-start mb-2"">
                                                        <div class=""menu-item-title fs-6 px-2"">{item2.Title}</div>
                                                    </span>
                                                    <div class=""tree-menu"">
                                                        <ul class=""list-unstyled text-start"">
                                                            @LVL3
                                                        </ul>
                                                    </div>
                                                </div>
                                              </li>";

                                    string LVL3 = "";
                                    foreach (MenuLevel3 item3 in itemsLVL3)
                                    {
                                        LVL3 += $@"<li><a class=""p-2"" href=""{item3.URL}"">{item3.Title}</a></li>";
                                    }
                                    LVL2 = LVL2.Replace("@LVL3", LVL3);
                                }
                                else
                                {
                                    // Level 2 without Level 3 -> clickable heading
                                    LVL2 += $@"<li class=""flex-fill list-unstyled"">
                                                <div>
                                                    <a class=""dga-submenutitle text-decoration-none fw-semibold text-start mb-2"" href=""{item2.URL}"">
                                                        <div class=""menu-item-title fs-6 px-2"">{item2.Title}</div>
                                                    </a>
                                                </div>
                                              </li>";
                                }
                            }

                            LVL1 = LVL1.Replace("@LVL2", LVL2);
                            HTML += LVL1;
                        }
                        else
                        {
                            // Level 1 with only Level 2 children -> 3-column grid of links
                            string LVL1 = $@"<li class=""nav-item dropdown"">
                                <a id=""{dropdownId}"" role=""button"" data-bs-toggle=""dropdown"" data-bs-auto-close=""outside""
                                   class=""nav-link position-relative p-0 px-2 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium""
                                   aria-expanded=""false"" aria-haspopup=""true"" aria-controls=""{menuId}"" href=""#"">
                                    <span class=""px-2 nav-link-text text-nowrap"">{item.Title}</span>
                                    <span class=""d-inline-flex fs-5"">
                                        <i class=""nav-link-icon rotation-transition hgi hgi-stroke hgi-arrow-down-01"" aria-hidden=""true""></i>
                                    </span>
                                </a>
                                <ul class=""dropdown-menu"" id=""{menuId}"" aria-labelledby=""{dropdownId}"">
                                    <li role=""presentation"">
                                        <div class=""container"">
                                            <ul class=""list-unstyled w-100 d-flex m-auto flex-wrap"">
                                                <li class=""flex-fill list-unstyled"">
                                                    <div class=""tree-menu"">
                                                        <ul class=""list-unstyled text-start row gx-3"">
                                                            @LVL2
                                                        </ul>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                              </li>";

                            string LVL2 = "";
                            foreach (MenuLevel2 item2 in item.LVL2)
                            {
                                LVL2 += $@"<li class=""col-4""><a class=""p-2"" href=""{item2.URL}"">{item2.Title}</a></li>";
                            }

                            LVL1 = LVL1.Replace("@LVL2", LVL2);
                            HTML += LVL1;
                        }
                    }
                    else
                    {
                        // Level 1 as direct link (no dropdown)
                        HTML += $@"<li class=""nav-item"">
                                    <a class=""nav-link position-relative p-0 px-2 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium"" href=""{item.URL}"">
                                        <span class=""px-2 nav-link-text text-nowrap"">{item.Title}</span>
                                    </a>
                                   </li>";
                    }
                }
            }

            litMenu.Text = HTML;
        }

        /// <summary>
        /// Builds the MOBILE offcanvas navigation using the new DGA accordion classes.
        /// Output shape:
        ///   <li class="nav-item mb-1">
        ///     <a data-bs-toggle="collapse" data-bs-target="#menu_itemN">Title <i hgi-arrow-down-01/></a>
        ///     <ul id="menu_itemN" class="list-unstyled collapse text-muted" data-bs-parent="#nav_accordion">
        ///       [if LVL3] <li><a data-bs-toggle="collapse" href="#menu_itemN_subM">Title</a><ul id="menu_itemN_subM"> [LVL3] </ul></li>
        ///       [else]    <li><a href="url">Title</a></li>
        ///     </ul>
        ///   </li>
        /// </summary>
        public void Get_MenuHTML(List<TopMenuLevel1> _AllData)
        {
            StringBuilder sb = new StringBuilder();

            if (_AllData != null && _AllData.Count > 0)
            {
                int lvl1Counter = 0;
                foreach (TopMenuLevel1 item in _AllData)
                {
                    lvl1Counter++;
                    string menuId = $"menu_item{lvl1Counter}";

                    if (item.LVL2 != null && item.LVL2.Count > 0)
                    {
                        sb.Append($@"
                <li class=""nav-item mb-1"">
                    <a class=""nav-link d-inline-flex w-100 justify-content-between collapsed fs-5 text-dark""
                       data-bs-toggle=""collapse"" data-bs-target=""#{menuId}"" aria-expanded=""false""
                       aria-controls=""{menuId}"">
                        {item.Title}
                        <i class=""verify-icon hgi hgi-stroke hgi-arrow-down-01 rotation-transition"" aria-hidden=""true""></i>
                    </a>
                    <ul id=""{menuId}"" class=""list-unstyled collapse text-muted"" data-bs-parent=""#nav_accordion"">");

                        int lvl2Counter = 0;
                        foreach (TopMenuLevel2 item2 in item.LVL2)
                        {
                            lvl2Counter++;
                            string subMenuId = $"{menuId}_sub{lvl2Counter}";

                            if (item2.LVL3 != null && item2.LVL3.Count > 0)
                            {
                                sb.Append($@"
                        <li class=""mt-2"">
                            <a class=""nav-link d-inline-flex w-100 justify-content-between collapsed fs-6 fw-semibold text-sa-600""
                               href=""#{subMenuId}"" role=""button"" data-bs-toggle=""collapse""
                               aria-expanded=""false"" aria-controls=""{subMenuId}"">
                                {item2.Title}
                                <i class=""verify-icon hgi hgi-stroke hgi-arrow-down-01 rotation-transition"" aria-hidden=""true""></i>
                            </a>
                            <ul id=""{subMenuId}"" class=""list-unstyled collapse ps-2 bg-light rounded-2 px-2 py-1 mt-1""
                                data-bs-parent=""#{menuId}"">");

                                foreach (TopMenuLevel3 item3 in item2.LVL3)
                                {
                                    sb.Append($@"<li><a class=""nav-link fs-6"" href=""{item3.URL}"">{item3.Title}</a></li>");
                                }

                                sb.Append("</ul></li>");
                            }
                            else
                            {
                                sb.Append($@"<li><a class=""nav-link fs-6"" href=""{item2.URL}"">{item2.Title}</a></li>");
                            }
                        }

                        sb.Append("</ul></li>");
                    }
                    else
                    {
                        sb.Append($@"<li class=""nav-item mb-1""><a class=""nav-link fs-5 text-dark"" href=""{item.URL}"">{item.Title}</a></li>");
                    }
                }
            }

            // The outer <ul id="nav_accordion"> is in the markup; we only emit its <li> children.
            litMenuMobile.Text = sb.ToString();
        }

        protected void lnkbtn_signout_Click1(object sender, EventArgs e)
        {
            try
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                FormsAuthentication.SignOut();
                if (HttpContext.Current == null)
                    return;
                HttpContext.Current.Response.Redirect(PortalHelper.ParentLangSite, false);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
        }
        public string ResultsPageUrl { get; set; }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string q = txtKeyword.Text == null ? "" : txtKeyword.Text.Trim();
            if (string.IsNullOrEmpty(q) && txtKeywordDesktop != null && !string.IsNullOrEmpty(txtKeywordDesktop.Text))
            {
                q = txtKeywordDesktop.Text.Trim();
            }
            DoSearch(q);
        }

        protected void btnSearchDesktop_Click(object sender, EventArgs e)
        {
            string q = txtKeywordDesktop.Text == null ? "" : txtKeywordDesktop.Text.Trim();
            if (string.IsNullOrEmpty(q) && txtKeyword != null && !string.IsNullOrEmpty(txtKeyword.Text))
            {
                q = txtKeyword.Text.Trim();
            }
            DoSearch(q);
        }

        private void DoSearch(string q)
        {
            string target = !string.IsNullOrEmpty(ResultsPageUrl)
                ? ResultsPageUrl
                : (IsArabic
                    ? "/ar/Search/Pages/SearchResults.aspx"
                    : "/en/Search/Pages/SearchResults.aspx");

            string url = target + "?q=" + HttpUtility.UrlEncode(q);
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }



        private void FixCanonicalLink()
        {
            try
            {
                if (SPContext.Current == null || Page.Header == null) return;

                // 1. Hide SharePoint's built-in SeoCanonicalLink (renders :443)
                HideCanonicalControls(Page.Header);

                // 2. Build clean canonical from AAM public URL (no port)
                var request = HttpContext.Current.Request;
                string authority = new Uri(SPContext.Current.Site.Url)
                                       .GetLeftPart(UriPartial.Authority);
                string canonical = authority + request.Url.AbsolutePath;

                if (request.Url.AbsolutePath.EndsWith("MediaDetails.aspx",
                        StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrEmpty(request.QueryString["Id"]))
                {
                    canonical += "?Id=" + HttpUtility.UrlEncode(request.QueryString["Id"]);
                }

                Page.Header.Controls.Add(new LiteralControl(
                    string.Format("<link rel=\"canonical\" href=\"{0}\" />", canonical)));
            }
            catch (Exception ex)
            {
                //Publics.WriteToLog("ucHomeHeader.FixCanonicalLink", ex);
            }
        }

        private void HideCanonicalControls(Control root)
        {
            if (root == null) return;
            if (root.GetType().Name.IndexOf("Canonical",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                root.Visible = false;
                return;
            }
            foreach (Control child in root.Controls)
                HideCanonicalControls(child);
        }

    }
}
