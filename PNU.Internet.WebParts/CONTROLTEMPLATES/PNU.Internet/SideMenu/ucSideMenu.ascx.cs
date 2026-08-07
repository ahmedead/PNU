using System;
using System.Collections.Generic;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using Portal.Main.Helper.Utils;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu
{
    public partial class ucSideMenu : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadSideMenu();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        /// <summary>
        /// Walks: current site -> parent -> parent's parent -> ... -> top site,
        /// looking for a web that has BOTH SideMenuLevel1 and SideMenuLevel2.
        /// The first web where both are found wins. If nothing is found all the
        /// way to the top site, both lists are provisioned on the CURRENT site.
        /// </summary>
        private void LoadSideMenu()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    SPList list1 = null;
                    SPList list2 = null;
                    bool reachedTopWithoutMatch = false;
                    string currentWebRelativeUrl;

                    using (SPWeb web = site.OpenWeb())
                    {
                        currentWebRelativeUrl = web.Url + "/";
                        if (currentWebRelativeUrl.Contains(site.Url))
                            currentWebRelativeUrl = currentWebRelativeUrl.Replace(site.Url, "");

                        string relativeWeb = currentWebRelativeUrl;

                        while (true)
                        {
                            bool foundBoth = false;

                            try
                            {
                                using (SPWeb pWeb = site.OpenWeb(relativeWeb))
                                {
                                    list1 = pWeb.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                                    list2 = pWeb.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);

                                    if (list1 != null && list2 != null)
                                    {
                                        BindMenu(pWeb, list1, list2);
                                        foundBoth = true;
                                    }
                                    else if (pWeb.ParentWeb == null)
                                    {
                                        reachedTopWithoutMatch = true;
                                    }
                                    else
                                    {
                                        relativeWeb = pWeb.ParentWeb.Url + "/";
                                        if (relativeWeb.Contains(site.Url))
                                            relativeWeb = relativeWeb.Replace(site.Url, "");
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                reachedTopWithoutMatch = true;
                            }

                            if (foundBoth || reachedTopWithoutMatch)
                                break;
                        }
                    }

                    if (reachedTopWithoutMatch)
                    {
                        ProvisionAndBindAtCurrentSite(site, currentWebRelativeUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        /// <summary>
        /// Neither list exists anywhere in the ancestor chain. Provision both on
        /// the CURRENT site, seed them (Faculties webs get the standard college
        /// menu), then bind.
        /// </summary>
        private void ProvisionAndBindAtCurrentSite(SPSite site, string currentWebRelativeUrl)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite eSite = new SPSite(site.ID))
                    using (SPWeb eWeb = eSite.OpenWeb(currentWebRelativeUrl))
                    {
                        SideMenuListProvisioner.EnsureLists(eWeb);
                        SideMenuListProvisioner.SeedMenu(eWeb);
                        SideMenuListProvisioner.SeedMenuForAgencies(eWeb);


                    }
                });

                using (SPWeb currentWeb = site.OpenWeb(currentWebRelativeUrl))
                {
                    SPList list1 = currentWeb.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                    SPList list2 = currentWeb.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);

                    if (list1 != null && list2 != null)
                        BindMenu(currentWeb, list1, list2);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  Binding
        // ------------------------------------------------------------------

        private void BindMenu(SPWeb web, SPList list1, SPList list2)
        {
            try
            {
                SPQuery level1Query = new SPQuery
                {
                    Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                              <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                };

                List<SideMenuLevel1> level1Items = MapLevel1(list1.GetItems(level1Query));

                if (level1Items == null || level1Items.Count == 0)
                {
                    pnlSideMenu.Visible = false;
                    return;
                }

                string currentPageUrl = NormalizeUrlForComparison(GetCurrentPageUrl());
                string activeTitle = null;
                int groupCounter = 0;

                foreach (SideMenuLevel1 parent in level1Items)
                {
                    SPQuery level2Query = new SPQuery
                    {
                        Query = string.Format(
                            @"<Where><And>
                                <Eq><FieldRef Name='Parent'/><Value Type='Lookup'>{0}</Value></Eq>
                                <Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq>
                              </And></Where>
                              <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>",
                            EscapeCamlValue(parent.Title))
                    };

                    parent.LVL2 = MapLevel2(list2.GetItems(level2Query));

                    // --- presentation properties, computed once, before binding ---
                    parent.DisplayTitle = GetLocalizedTitle(parent.Title, parent.Title_EN);
                    parent.ResolvedUrl = GetItemUrl(parent.URL);
                    parent.HasChildren = parent.LVL2 != null && parent.LVL2.Count > 0;
                    parent.ChildCount = parent.HasChildren ? parent.LVL2.Count : 0;
                    parent.GroupId = string.Format("sideMenuGroup{0}", ++groupCounter);

                    if (parent.HasChildren)
                    {
                        foreach (SideMenuLevel2 child in parent.LVL2)
                        {
                            child.DisplayTitle = GetLocalizedTitle(child.Title, child.Title_EN);
                            child.ResolvedUrl = GetItemUrl(child.URL);
                            child.IsActive = !string.IsNullOrEmpty(currentPageUrl)
                                && NormalizeUrlForComparison(child.ResolvedUrl) == currentPageUrl;

                            if (child.IsActive)
                            {
                                parent.IsActive = true;   // expand the group
                                if (activeTitle == null) activeTitle = child.DisplayTitle;
                            }
                        }
                    }
                    else
                    {
                        parent.IsActive = !string.IsNullOrEmpty(currentPageUrl)
                            && NormalizeUrlForComparison(parent.ResolvedUrl) == currentPageUrl;

                        if (parent.IsActive && activeTitle == null)
                            activeTitle = parent.DisplayTitle;
                    }
                }

                litCurrentTitle.Text = Server.HtmlEncode(activeTitle ?? level1Items[0].DisplayTitle);

                rptLevel1.DataSource = level1Items;
                rptLevel1.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        /// <summary>
        /// Shows either the leaf or the group placeholder for each Level1 row,
        /// and binds the nested Level2 repeater when it's a group.
        /// </summary>
        protected void rptLevel1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            SideMenuLevel1 data = e.Item.DataItem as SideMenuLevel1;
            if (data == null) return;

            PlaceHolder phLeaf = (PlaceHolder)e.Item.FindControl("phLeaf");
            PlaceHolder phGroup = (PlaceHolder)e.Item.FindControl("phGroup");

            phLeaf.Visible = !data.HasChildren;
            phGroup.Visible = data.HasChildren;

            if (data.HasChildren)
            {
                Repeater rptLevel2 = (Repeater)e.Item.FindControl("rptLevel2");
                if (rptLevel2 != null)
                {
                    rptLevel2.DataSource = data.LVL2;
                    rptLevel2.DataBind();
                }
            }
        }

        // ------------------------------------------------------------------
        //  Field mapping
        // ------------------------------------------------------------------

        private List<SideMenuLevel1> MapLevel1(SPListItemCollection items)
        {
            List<SideMenuLevel1> results = new List<SideMenuLevel1>();
            if (items == null) return results;

            foreach (SPListItem item in items)
            {
                results.Add(new SideMenuLevel1
                {
                    Id = item.ID,
                    Title = SafeString(item["Title"]),
                    Title_EN = SafeString(item["Title_EN"]),
                    URL = SafeUrl(item["URL"]),
                    ItemOrder = SafeInt(item["ItemOrder"]),
                    Visibility = SafeBool(item["Visibility"])
                });
            }

            return results;
        }

        private List<SideMenuLevel2> MapLevel2(SPListItemCollection items)
        {
            List<SideMenuLevel2> results = new List<SideMenuLevel2>();
            if (items == null) return results;

            foreach (SPListItem item in items)
            {
                results.Add(new SideMenuLevel2
                {
                    Id = item.ID,
                    Title = SafeString(item["Title"]),
                    Title_EN = SafeString(item["Title_EN"]),
                    URL = SafeUrl(item["URL"]),
                    Parent = SafeString(item["Parent"]),
                    ItemOrder = SafeInt(item["ItemOrder"]),
                    Visibility = SafeBool(item["Visibility"])
                });
            }

            return results;
        }

        // ------------------------------------------------------------------
        //  Helpers
        // ------------------------------------------------------------------

        private string GetLocalizedTitle(string ar, string en)
        {
            if (PortalHelper.IsArabic)
                return string.IsNullOrWhiteSpace(ar) ? en : ar;

            return string.IsNullOrWhiteSpace(en) ? ar : en;
        }

        private string GetItemUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return "#";

            if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return url;

            // Stored values may or may not have a leading slash - normalize to one.
            return "/" + url.TrimStart('/');
        }

        /// <summary>The URL of the page currently being rendered, used to find the active menu item.</summary>
        private string GetCurrentPageUrl()
        {
            string query = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                query = HttpContext.Current.Request.Url.Query;

            try
            {
                if (SPContext.Current != null && SPContext.Current.File != null
                    && SPContext.Current.File.Exists)
                {
                    return SPContext.Current.File.ServerRelativeUrl + query;
                }
            }
            catch (Exception)
            {
                // fall through to the request URL below
            }

            return HttpContext.Current != null && HttpContext.Current.Request != null
                ? HttpContext.Current.Request.Url.AbsoluteUri
                : string.Empty;
        }

        /// <summary>
        /// Normalizes a URL for comparison: strips protocol/host and fragment,
        /// trims a trailing slash, decodes and lower-cases the path, and
        /// canonicalizes the query string (parameters sorted by name).
        /// </summary>
        private string NormalizeUrlForComparison(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            string result = url.Trim();

            int fragmentIndex = result.IndexOf('#');
            if (fragmentIndex >= 0)
                result = result.Substring(0, fragmentIndex);

            string path = result;
            string query = string.Empty;
            int queryIndex = result.IndexOf('?');
            if (queryIndex >= 0)
            {
                path = result.Substring(0, queryIndex);
                query = result.Substring(queryIndex + 1);
            }

            Uri parsed;
            if (Uri.TryCreate(path, UriKind.Absolute, out parsed))
                path = parsed.AbsolutePath;

            path = HttpUtility.UrlDecode(path.TrimEnd('/')).ToLowerInvariant();

            if (string.IsNullOrEmpty(query))
                return path;

            string[] pairs = query.Split('&');
            List<string> normalizedPairs = new List<string>();
            foreach (string pair in pairs)
            {
                if (!string.IsNullOrEmpty(pair))
                    normalizedPairs.Add(HttpUtility.UrlDecode(pair).ToLowerInvariant());
            }
            normalizedPairs.Sort(StringComparer.Ordinal);

            return path + "?" + string.Join("&", normalizedPairs.ToArray());
        }

        private string EscapeCamlValue(string value)
        {
            return string.IsNullOrEmpty(value) ? value : SecurityElement.Escape(value);
        }

        private string SafeString(object value)
        {
            return value == null ? string.Empty : Convert.ToString(value);
        }

        private int SafeInt(object value)
        {
            int result;
            return int.TryParse(SafeString(value), out result) ? result : 0;
        }

        private bool SafeBool(object value)
        {
            if (value == null) return false;
            bool result;
            return bool.TryParse(SafeString(value), out result) && result;
        }

        /// <summary>URL fields are stored as "url, description" - unpack via SPFieldUrlValue.</summary>
        private string SafeUrl(object value)
        {
            if (value == null) return string.Empty;
            SPFieldUrlValue urlValue = new SPFieldUrlValue(Convert.ToString(value));
            return urlValue.Url ?? string.Empty;
        }
    }
}
