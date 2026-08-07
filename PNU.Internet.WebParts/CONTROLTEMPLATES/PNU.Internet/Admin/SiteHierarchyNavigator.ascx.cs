using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class SiteHierarchyNavigator : UserControl
    {
        #region Constants

        private const string ADMIN_SITE_URL = "/ar/ITAdmin";
        private const string PAGES_INVENTORY_LIST = "PagesInventory";

        // ViewState keys
        private const string VS_SELECTED_WEB_URL = "SelectedWebUrl";
        private const string VS_SELECTED_WEB_APP = "SelectedWebApp";
        private const string VS_SUBSITE_LEVELS = "SubsiteLevels";
        private const string VS_PAGES_DATA = "PagesData";

        private const string AR_SEGMENT = "/ar/";
        private const string EN_SEGMENT = "/en/";
        private const string AR_ROOT = "/ar";
        private const string EN_ROOT = "/en";

        #endregion

        #region Properties

        private string SelectedWebUrl
        {
            get { return ViewState[VS_SELECTED_WEB_URL] as string; }
            set { ViewState[VS_SELECTED_WEB_URL] = value; }
        }

        private string SelectedWebAppUrl
        {
            get { return ViewState[VS_SELECTED_WEB_APP] as string; }
            set { ViewState[VS_SELECTED_WEB_APP] = value; }
        }

        /// <summary>
        /// Tracks the chain of selected subsite URLs (one per dropdown level).
        /// </summary>
        private List<string> SubsiteLevels
        {
            get
            {
                var list = ViewState[VS_SUBSITE_LEVELS] as List<string>;
                if (list == null)
                {
                    list = new List<string>();
                    ViewState[VS_SUBSITE_LEVELS] = list;
                }
                return list;
            }
            set { ViewState[VS_SUBSITE_LEVELS] = value; }
        }

        private DataTable PagesData
        {
            get { return ViewState[VS_PAGES_DATA] as DataTable; }
            set { ViewState[VS_PAGES_DATA] = value; }
        }

        #endregion

        #region Page Lifecycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWebApplications();
            }
            else
            {
                // Rebuild dynamic dropdowns so events wire up after postback
                RebuildSubsiteDropdowns();
            }
        }

        #endregion

        #region Web Applications

        private void LoadWebApplications()
        {
            try
            {
                ddlWebApps.Items.Clear();
                ddlWebApps.Items.Add(new ListItem("-- اختر تطبيق ويب --", ""));

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    SPWebService service = SPWebService.ContentService;
                    foreach (SPWebApplication webApp in service.WebApplications)
                    {
                        // Skip central admin
                        if (webApp.IsAdministrationWebApplication) continue;

                        foreach (SPAlternateUrl url in webApp.AlternateUrls)
                        {
                            if (url.UrlZone == SPUrlZone.Default)
                            {
                                ddlWebApps.Items.Add(new ListItem(
                                    string.Format("{0} ({1})", webApp.Name, url.IncomingUrl),
                                    url.IncomingUrl));
                                break;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في تحميل تطبيقات الويب: " + ex.Message, "error");
            }
        }

        protected void ddlWebApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetBelow(0);
            string webAppUrl = ddlWebApps.SelectedValue;

            if (string.IsNullOrEmpty(webAppUrl))
            {
                pnlSiteCollections.Visible = false;
                return;
            }

            SelectedWebAppUrl = webAppUrl;
            LoadSiteCollections(webAppUrl);
            pnlSiteCollections.Visible = true;
        }

        #endregion

        #region Site Collections

        private void LoadSiteCollections(string webAppUrl)
        {
            try
            {
                ddlSiteCollections.Items.Clear();
                ddlSiteCollections.Items.Add(new ListItem("-- اختر مجموعة المواقع --", ""));

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    Uri webAppUri = new Uri(webAppUrl);
                    SPWebApplication webApp = SPWebApplication.Lookup(webAppUri);
                    if (webApp == null) return;

                    foreach (SPSite site in webApp.Sites)
                    {
                        try
                        {
                            string display = string.IsNullOrEmpty(site.RootWeb.Title)
                                ? site.Url
                                : string.Format("{0} ({1})", site.RootWeb.Title, site.ServerRelativeUrl);
                            ddlSiteCollections.Items.Add(new ListItem(display, site.Url));
                        }
                        finally
                        {
                            site.Dispose();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في تحميل مجموعات المواقع: " + ex.Message, "error");
            }
        }

        protected void ddlSiteCollections_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetBelow(0);
            string siteUrl = ddlSiteCollections.SelectedValue;
            if (string.IsNullOrEmpty(siteUrl))
            {
                pnlActions.Visible = false;
                return;
            }

            SelectedWebUrl = siteUrl;
            ShowSelectedSite(siteUrl);

            // Build the first subsite-level dropdown
            BuildNextSubsiteDropdown(siteUrl, 0);
        }

        #endregion

        #region Dynamic Subsite Dropdowns

        /// <summary>
        /// Builds a dropdown listing direct subsites of parentWebUrl, 
        /// placed at the given depth level.
        /// </summary>
        private void BuildNextSubsiteDropdown(string parentWebUrl, int level)
        {
            List<KeyValuePair<string, string>> subsites = GetDirectSubsites(parentWebUrl);

            if (subsites == null || subsites.Count == 0)
            {
                // Leaf reached — no more subsites
                return;
            }

            // Wrapper div for visual indentation
            HtmlGenericControl wrapper = new HtmlGenericControl("div");
            wrapper.Attributes["class"] = "form-row subsite-level";
            wrapper.Style["margin-right"] = (level * 15) + "px";

            Label lbl = new Label();
            lbl.Text = string.Format("المواقع الفرعية (المستوى {0}): ", level + 1);
            lbl.Font.Bold = true;
            wrapper.Controls.Add(lbl);

            DropDownList ddl = new DropDownList();
            ddl.ID = "ddlSubsite_" + level;
            ddl.AutoPostBack = true;
            ddl.Width = new Unit(450);
            ddl.CssClass = "ms-long";
            ddl.Attributes["data-level"] = level.ToString();
            ddl.Attributes["data-parent"] = parentWebUrl;
            ddl.SelectedIndexChanged += DynamicSubsiteDropdown_Changed;

            ddl.Items.Add(new ListItem("-- (اختياري) اختر موقعاً فرعياً --", ""));
            foreach (var kv in subsites)
            {
                ddl.Items.Add(new ListItem(kv.Key, kv.Value));
            }

            wrapper.Controls.Add(ddl);
            phSubsiteDropdowns.Controls.Add(wrapper);
        }

        /// <summary>
        /// Re-creates dynamic dropdowns from ViewState after a postback so that 
        /// their events still fire and selections persist.
        /// </summary>
        private void RebuildSubsiteDropdowns()
        {
            phSubsiteDropdowns.Controls.Clear();
            var chain = SubsiteLevels;

            if (string.IsNullOrEmpty(SelectedWebAppUrl)) return;
            if (string.IsNullOrEmpty(ddlSiteCollections.SelectedValue)) return;

            // Re-build level 0 (subsites of the site collection root)
            BuildNextSubsiteDropdown(ddlSiteCollections.SelectedValue, 0);
            RestoreDropdownSelection(0);

            // Re-build each subsequent level whose parent was previously selected
            for (int i = 0; i < chain.Count; i++)
            {
                string parentUrl = chain[i];
                if (string.IsNullOrEmpty(parentUrl)) break;
                BuildNextSubsiteDropdown(parentUrl, i + 1);
                RestoreDropdownSelection(i + 1);
            }
        }

        private void RestoreDropdownSelection(int level)
        {
            if (level >= SubsiteLevels.Count) return;
            DropDownList ddl = phSubsiteDropdowns.FindControl("ddlSubsite_" + level)
                as DropDownList;
            if (ddl == null) return;

            string selected = SubsiteLevels[level];
            ListItem item = ddl.Items.FindByValue(selected ?? "");
            if (item != null) ddl.SelectedValue = selected;
        }

        protected void DynamicSubsiteDropdown_Changed(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            if (ddl == null) return;

            int level = int.Parse(ddl.Attributes["data-level"]);
            string selectedUrl = ddl.SelectedValue;

            // Trim levels at and below this one
            var chain = SubsiteLevels;
            while (chain.Count > level) chain.RemoveAt(chain.Count - 1);

            if (string.IsNullOrEmpty(selectedUrl))
            {
                // User cleared selection — fall back to the parent at this level
                string parent = ddl.Attributes["data-parent"];
                SelectedWebUrl = parent;
                SubsiteLevels = chain;
                RemoveDropdownsBelow(level);
                ShowSelectedSite(parent);
                return;
            }

            chain.Add(selectedUrl);
            SubsiteLevels = chain;
            SelectedWebUrl = selectedUrl;
            ShowSelectedSite(selectedUrl);

            // Remove any deeper dropdowns then try to build the next level
            RemoveDropdownsBelow(level);
            BuildNextSubsiteDropdown(selectedUrl, level + 1);
        }

        private void RemoveDropdownsBelow(int keepLevel)
        {
            // Remove wrappers whose dropdown level > keepLevel
            var toRemove = new List<Control>();
            foreach (Control wrapper in phSubsiteDropdowns.Controls)
            {
                foreach (Control child in wrapper.Controls)
                {
                    DropDownList ddl = child as DropDownList;
                    if (ddl != null && ddl.Attributes["data-level"] != null)
                    {
                        int lvl = int.Parse(ddl.Attributes["data-level"]);
                        if (lvl > keepLevel) toRemove.Add(wrapper);
                    }
                }
            }
            foreach (var c in toRemove) phSubsiteDropdowns.Controls.Remove(c);
        }

        private void ResetBelow(int level)
        {
            phSubsiteDropdowns.Controls.Clear();
            SubsiteLevels = new List<string>();
            pnlActions.Visible = false;
            pnlResults.Visible = false;
            pnlMessage.Visible = false;
        }

        /// <summary>
        /// Returns direct subsites of parentWebUrl as (Title, ServerRelativeUrl) pairs.
        /// </summary>
        private List<KeyValuePair<string, string>> GetDirectSubsites(string parentWebUrl)
        {
            var result = new List<KeyValuePair<string, string>>();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                try
                {
                    using (SPSite site = new SPSite(parentWebUrl))
                    using (SPWeb web = site.OpenWeb(new Uri(parentWebUrl).AbsolutePath))
                    {
                        foreach (SPWeb sub in web.Webs)
                        {
                            try
                            {
                                string display = string.IsNullOrEmpty(sub.Title)
                                    ? sub.ServerRelativeUrl
                                    : string.Format("{0} ({1})", sub.Title, sub.ServerRelativeUrl);
                                result.Add(new KeyValuePair<string, string>(
                                    display,
                                    site.MakeFullUrl(sub.ServerRelativeUrl)));
                            }
                            finally
                            {
                                sub.Dispose();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            });

            return result;
        }

        private void ShowSelectedSite(string url)
        {
            lblSelectedSite.Text = "الموقع المختار: " + url;
            pnlActions.Visible = true;
        }

        #endregion

        #region List Pages

        protected void btnListPages_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedWebUrl))
            {
                ShowMessage("لم يتم اختيار موقع", "error");
                return;
            }

            try
            {
                DataTable dt = BuildPagesTable();
                PagesData = dt;

                gvPages.DataSource = dt;
                gvPages.DataBind();
                litCount.Text = dt.Rows.Count.ToString();
                pnlResults.Visible = true;
                pnlMessage.Visible = false;
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في جلب الصفحات: " + ex.Message, "error");
            }
        }

        private DataTable BuildPagesTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Url", typeof(string));        // server-relative AR URL
            dt.Columns.Add("FullUrl", typeof(string));
            dt.Columns.Add("PageLayout", typeof(string));
            dt.Columns.Add("UserControlPath", typeof(string));
            dt.Columns.Add("WebUrl", typeof(string));
            dt.Columns.Add("HasEnglishVersion", typeof(bool));
            dt.Columns.Add("ExpectedEnUrl", typeof(string));
            dt.Columns.Add("EnWebUrl", typeof(string));      // server-relative EN web URL
            dt.Columns.Add("PageFileName", typeof(string));  // e.g. MyPage.aspx

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(SelectedWebUrl))
                using (SPWeb web = site.OpenWeb(new Uri(SelectedWebUrl).AbsolutePath))
                {
                    if (!PublishingWeb.IsPublishingWeb(web))
                    {
                        ReadSitePagesLibrary(web, dt);
                        EnrichWithEnglishInfo(dt, site);
                        return;
                    }

                    PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                    PublishingPageCollection pages = pubWeb.GetPublishingPages();

                    foreach (PublishingPage page in pages)
                    {
                        // IMPORTANT: page.Url is RELATIVE to the publishing web, 
                        // not server-relative. For a web at /ar it returns 
                        // "Pages/home.aspx" — losing the /ar prefix. 
                        // Always use the file's ServerRelativeUrl instead.
                        string pageServerRelativeUrl = page.ListItem.File.ServerRelativeUrl;

                        DataRow row = dt.NewRow();
                        row["Title"] = page.Title ?? page.Name;
                        row["Url"] = pageServerRelativeUrl;
                        row["FullUrl"] = site.MakeFullUrl(pageServerRelativeUrl);
                        row["PageLayout"] = page.Layout != null
                            ? page.Layout.ServerRelativeUrl : "";
                        row["UserControlPath"] = ExtractControlLoaderPath(page.ListItem.File);
                        row["WebUrl"] = web.ServerRelativeUrl;
                        row["PageFileName"] = page.ListItem.File.Name;
                        row["HasEnglishVersion"] = false;
                        row["ExpectedEnUrl"] = "";
                        row["EnWebUrl"] = "";
                        dt.Rows.Add(row);
                    }

                    EnrichWithEnglishInfo(dt, site);
                }
            });

            return dt;
        }

        private void ReadSitePagesLibrary(SPWeb web, DataTable dt)
        {
            SPList sitePages = web.Lists.TryGetList("Site Pages")
                ?? web.Lists.TryGetList("SitePages");
            if (sitePages == null) return;

            foreach (SPListItem item in sitePages.Items)
            {
                if (item.File == null) continue;
                DataRow row = dt.NewRow();
                row["Title"] = item.Title ?? item.File.Name;
                row["Url"] = item.File.ServerRelativeUrl;
                row["FullUrl"] = web.Site.MakeFullUrl(item.File.ServerRelativeUrl);
                row["PageLayout"] = "";
                row["UserControlPath"] = ExtractControlLoaderPath(item.File);
                row["WebUrl"] = web.ServerRelativeUrl;
                row["PageFileName"] = item.File.Name;
                row["HasEnglishVersion"] = false;
                row["ExpectedEnUrl"] = "";
                row["EnWebUrl"] = "";
                dt.Rows.Add(row);
            }
        }

        #region English Version Detection

        /// <summary>
        /// For every row, compute the expected EN URL and check if it exists.
        /// </summary>
        private void EnrichWithEnglishInfo(DataTable dt, SPSite site)
        {
            int missing = 0;
            foreach (DataRow row in dt.Rows)
            {
                string arWebUrl = SafeStr(row["WebUrl"]);
                string arPageUrl = SafeStr(row["Url"]);

                string enWebUrl = MapArWebToEnWeb(arWebUrl);
                if (string.IsNullOrEmpty(enWebUrl))
                {
                    row["HasEnglishVersion"] = false;
                    row["ExpectedEnUrl"] = "(غير قابل للتعيين — الموقع ليس تحت /ar/)";
                    continue;
                }

                string expectedEnPageUrl = MapArPageToEnPage(arPageUrl);
                row["ExpectedEnUrl"] = expectedEnPageUrl;
                row["EnWebUrl"] = enWebUrl;

                bool exists = CheckEnPageExists(site, enWebUrl, expectedEnPageUrl);
                row["HasEnglishVersion"] = exists;
                if (!exists) missing++;
            }

            lblMissingCount.Text = string.Format(
                "عدد صفحات EN المفقودة: {0} من أصل {1}", missing, dt.Rows.Count);
        }

        /// <summary>
        /// Map an Arabic web URL to its English equivalent by swapping /ar/ → /en/ 
        /// or /ar (exact) → /en. Returns empty if not under the Arabic branch.
        /// </summary>
        private string MapArWebToEnWeb(string arWebUrl)
        {
            if (string.IsNullOrEmpty(arWebUrl)) return "";

            // Normalize: ensure trailing slash so /ar at root matches
            string normalized = arWebUrl;
            if (normalized.Equals(AR_ROOT, StringComparison.OrdinalIgnoreCase))
                return EN_ROOT;

            // Exact "/ar" prefix followed by "/" → swap
            if (normalized.StartsWith(AR_SEGMENT, StringComparison.OrdinalIgnoreCase))
                return EN_SEGMENT + normalized.Substring(AR_SEGMENT.Length);

            // Case: URL begins with "/ar" but next char is not slash → not actually under /ar
            return "";
        }

        /// <summary>
        /// Map an AR page URL to its EN counterpart by swapping /ar/ → /en/ in the path.
        /// </summary>
        private string MapArPageToEnPage(string arPageUrl)
        {
            if (string.IsNullOrEmpty(arPageUrl)) return "";

            if (arPageUrl.StartsWith(AR_SEGMENT, StringComparison.OrdinalIgnoreCase))
                return EN_SEGMENT + arPageUrl.Substring(AR_SEGMENT.Length);

            if (arPageUrl.Equals(AR_ROOT, StringComparison.OrdinalIgnoreCase))
                return EN_ROOT;

            return "";
        }

        /// <summary>
        /// Checks whether the expected EN page exists in the EN web's Pages library.
        /// </summary>
        private bool CheckEnPageExists(SPSite site, string enWebServerRelativeUrl,
            string enPageServerRelativeUrl)
        {
            if (string.IsNullOrEmpty(enWebServerRelativeUrl)) return false;
            if (string.IsNullOrEmpty(enPageServerRelativeUrl)) return false;

            try
            {
                // First confirm the EN web itself exists
                using (SPWeb enWeb = site.OpenWeb(enWebServerRelativeUrl))
                {
                    if (!enWeb.Exists) return false;

                    SPFile file = enWeb.GetFile(enPageServerRelativeUrl);
                    return file != null && file.Exists;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Create Missing EN Pages

        protected void btnCreateMissingEn_Click(object sender, EventArgs e)
        {
            DataTable dt = PagesData;
            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessage("لا توجد بيانات. اعرض الصفحات أولاً.", "error");
                return;
            }

            // CRITICAL: Validate the form digest ONCE at the start of the request.
            // Per Microsoft docs, this caches the validation result for the entire 
            // request, so all subsequent writes (including those inside 
            // RunWithElevatedPrivileges) pass digest validation automatically.
            // Without this, writes inside elevated mode throw 
            // "The security validation for this page is invalid."
            // Reference: https://learn.microsoft.com/.../SPWeb.ValidateFormDigest
            try
            {
                SPUtility.ValidateFormDigest();
            }
            catch
            {
                // If digest validation fails entirely, fall through and rely on 
                // AllowUnsafeUpdates as a secondary defense.
            }

            int created = 0, skipped = 0, failed = 0, websCreated = 0;
            StringBuilder details = new StringBuilder();
            // Track webs created in this run so we don't try to re-create them 
            // and to report them in the summary
            HashSet<string> createdWebs = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(SelectedWebUrl))
                    {
                        // CRITICAL: bypass form digest validation for the whole 
                        // elevated operation. Required for write operations 
                        // initiated from an HTTP request context.
                        site.AllowUnsafeUpdates = true;
                        try
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                bool hasEn = (bool)row["HasEnglishVersion"];
                                if (hasEn) { skipped++; continue; }

                                string enWebUrl = SafeStr(row["EnWebUrl"]);
                                string expectedEnPageUrl = SafeStr(row["ExpectedEnUrl"]);
                                string arPageUrl = SafeStr(row["Url"]);

                                if (string.IsNullOrEmpty(enWebUrl)
                                    || string.IsNullOrEmpty(expectedEnPageUrl)
                                    || expectedEnPageUrl.StartsWith("("))
                                {
                                    skipped++;
                                    details.AppendFormat(
                                        "<div class='warning'>تخطي (لا يمكن تعيين EN): {0}</div>",
                                        arPageUrl);
                                    continue;
                                }

                                try
                                {
                                    // STEP 1: Ensure the EN web (and any missing ancestors) exist
                                    List<string> newlyCreatedWebs = EnsureEnglishWebExists(
                                        site, enWebUrl);
                                    foreach (var w in newlyCreatedWebs)
                                    {
                                        if (createdWebs.Add(w))
                                        {
                                            websCreated++;
                                            details.AppendFormat(
                                                "<div class='success'>✓ تم إنشاء الموقع: {0}</div>",
                                                w);
                                        }
                                    }

                                    // STEP 2: Create the EN page
                                    CreateEnglishPage(site, arPageUrl, enWebUrl,
                                        expectedEnPageUrl, SafeStr(row["PageFileName"]));

                                    created++;
                                    details.AppendFormat(
                                        "<div class='success'>✓ تم إنشاء الصفحة: {0}</div>",
                                        expectedEnPageUrl);

                                    // Update the in-memory grid so refresh isn't required
                                    row["HasEnglishVersion"] = true;
                                }
                                catch (Exception ex)
                                {
                                    failed++;
                                    details.AppendFormat(
                                        "<div class='error'>✗ فشل {0}: {1}</div>",
                                        expectedEnPageUrl, ex.Message);

                                }
                            }
                        }
                        finally
                        {
                            site.AllowUnsafeUpdates = false;
                        }
                    }
                });

                // Persist updated grid state
                PagesData = dt;
                gvPages.DataSource = dt;
                gvPages.DataBind();

                string summary = string.Format(
                    "المواقع المُنشأة: {0} | الصفحات المُنشأة: {1} | تم التخطي: {2} | فشل: {3}<br/><br/>{4}",
                    websCreated, created, skipped, failed, details.ToString());
                ShowMessage(summary, failed > 0 ? "error" : "success");
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ عام: " + ex.Message, "error");

            }
        }

        /// <summary>
        /// Ensures the EN web at enWebServerRelativeUrl exists, creating it (and any 
        /// missing ancestor EN webs) by cloning the structure of the matching AR webs.
        /// Returns the list of newly-created EN web URLs in creation order.
        /// </summary>
        private List<string> EnsureEnglishWebExists(SPSite site,
            string enWebServerRelativeUrl)
        {
            var created = new List<string>();

            if (string.IsNullOrEmpty(enWebServerRelativeUrl)) return created;

            // Normalize: collapse double slashes, trim trailing
            string normalized = NormalizeServerRelativeUrl(enWebServerRelativeUrl);

            // Check if already exists
            if (WebExists(site, normalized)) return created;

            // Walk up the chain to find the first existing EN ancestor.
            // Build path segments from "/en" downward (excluding the root "/").
            // E.g. "/en/colleges/cs/students" → segments ["/en", "/en/colleges", 
            // "/en/colleges/cs", "/en/colleges/cs/students"]
            List<string> chain = BuildAncestorChain(normalized);

            // Find the deepest existing ancestor as the starting point
            string deepestExisting = null;
            int startIdx = 0;
            for (int i = chain.Count - 1; i >= 0; i--)
            {
                if (WebExists(site, chain[i]))
                {
                    deepestExisting = chain[i];
                    startIdx = i + 1;
                    break;
                }
            }

            if (deepestExisting == null)
            {
                // Even "/en" doesn't exist — we need the site collection root as parent
                deepestExisting = "/";
                startIdx = 0;
            }

            // Create each missing level in turn
            for (int i = startIdx; i < chain.Count; i++)
            {
                string targetEnUrl = chain[i];
                string parentEnUrl = (i == 0) ? deepestExisting : chain[i - 1];
                string matchingArUrl = MapEnWebToArWeb(targetEnUrl);

                CreateEnglishSubsite(site, parentEnUrl, targetEnUrl, matchingArUrl);
                created.Add(targetEnUrl);
            }

            return created;
        }

        /// <summary>
        /// Creates a single EN subsite as a child of parentEnUrl, named to match 
        /// targetEnUrl, cloning the template/language settings of matchingArUrl 
        /// (the AR sibling) so the EN site has the same structure.
        /// </summary>
        private void CreateEnglishSubsite(SPSite site, string parentEnUrl,
            string targetEnUrl, string matchingArUrl)
        {
            // Determine the leaf segment (the new web's URL name within its parent)
            string leafName = GetLastSegment(targetEnUrl);
            if (string.IsNullOrEmpty(leafName))
                throw new Exception("تعذر تحديد اسم الموقع الجديد: " + targetEnUrl);

            // Read template + title + description from the matching AR web 
            // (if it exists). Otherwise fall back to a sensible default.
            string templateName = "CMSPUBLISHING#0";  // default publishing site template
            string title = leafName;
            string description = "";
            uint lcid = 1033;  // English

            if (WebExists(site, matchingArUrl))
            {
                using (SPWeb arWeb = site.OpenWeb(matchingArUrl))
                {
                    if (arWeb.Exists)
                    {
                        // WebTemplate#Configuration is the format Add expects
                        templateName = string.Format("{0}#{1}",
                            arWeb.WebTemplate, arWeb.Configuration);
                        title = arWeb.Title;
                        description = arWeb.Description;
                    }
                }
            }

            using (SPWeb parentWeb = site.OpenWeb(parentEnUrl))
            {
                if (!parentWeb.Exists)
                    throw new Exception("الموقع الأب غير موجود: " + parentEnUrl);

                parentWeb.AllowUnsafeUpdates = true;
                try
                {
                    // SPWebCollection.Add signature:
                    // Add(strWebUrl, strTitle, strDescription, nLCID, strWebTemplate, 
                    //     useUniquePermissions, bConvertIfThere)
                    SPWeb newWeb = parentWeb.Webs.Add(
                        leafName,
                        title,
                        description,
                        lcid,
                        templateName,
                        false,   // inherit permissions from parent
                        false);  // don't convert if folder exists

                    try
                    {
                        // Ensure the publishing feature is active if AR sibling has it
                        if (WebExists(site, matchingArUrl))
                        {
                            using (SPWeb arSibling = site.OpenWeb(matchingArUrl))
                            {
                                if (arSibling.Exists && PublishingWeb.IsPublishingWeb(arSibling)
                                    && !PublishingWeb.IsPublishingWeb(newWeb))
                                {
                                    // Activate the publishing web feature
                                    Guid pubFeatureId = new Guid(
                                        "94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb");
                                    newWeb.Features.Add(pubFeatureId, true);
                                }
                            }
                        }
                    }
                    finally
                    {
                        newWeb.Dispose();
                    }
                }
                finally
                {
                    parentWeb.AllowUnsafeUpdates = false;
                }
            }
        }

        /// <summary>
        /// Maps an EN web URL back to its AR sibling. E.g. "/en/colleges/cs" → 
        /// "/ar/colleges/cs". Returns empty string if not under /en/.
        /// </summary>
        private string MapEnWebToArWeb(string enWebUrl)
        {
            if (string.IsNullOrEmpty(enWebUrl)) return "";

            if (enWebUrl.Equals(EN_ROOT, StringComparison.OrdinalIgnoreCase))
                return AR_ROOT;

            if (enWebUrl.StartsWith(EN_SEGMENT, StringComparison.OrdinalIgnoreCase))
                return AR_SEGMENT + enWebUrl.Substring(EN_SEGMENT.Length);

            return "";
        }

        /// <summary>
        /// Checks whether an SPWeb exists at the given server-relative URL.
        /// </summary>
        private bool WebExists(SPSite site, string webServerRelativeUrl)
        {
            if (string.IsNullOrEmpty(webServerRelativeUrl)) return false;
            try
            {
                using (SPWeb w = site.OpenWeb(webServerRelativeUrl))
                {
                    return w.Exists;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Builds the ancestor chain for a server-relative web URL.
        /// "/en/colleges/cs" → ["/en", "/en/colleges", "/en/colleges/cs"]
        /// </summary>
        private List<string> BuildAncestorChain(string webUrl)
        {
            var chain = new List<string>();
            if (string.IsNullOrEmpty(webUrl) || webUrl == "/") return chain;

            string[] segments = webUrl.Trim('/').Split(
                new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            string accumulated = "";
            foreach (string seg in segments)
            {
                accumulated += "/" + seg;
                chain.Add(accumulated);
            }
            return chain;
        }

        /// <summary>
        /// Returns the last URL segment of a server-relative URL.
        /// "/en/colleges/cs" → "cs"
        /// </summary>
        private string GetLastSegment(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            string[] parts = url.Trim('/').Split(
                new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 0 ? "" : parts[parts.Length - 1];
        }

        /// <summary>
        /// Normalizes a server-relative URL: ensures leading slash, removes 
        /// trailing slash, collapses doubled slashes.
        /// </summary>
        private string NormalizeServerRelativeUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return "/";
            string result = url.Trim();
            while (result.Contains("//")) result = result.Replace("//", "/");
            if (!result.StartsWith("/")) result = "/" + result;
            if (result.Length > 1 && result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            return result;
        }

        /// <summary>
        /// Clones an AR publishing page to its EN equivalent, copying field values 
        /// and the ControlLoader WebPart with its UserControlPath.
        /// </summary>
        private void CreateEnglishPage(SPSite site, string arPageServerRelativeUrl,
            string enWebServerRelativeUrl, string enPageServerRelativeUrl,
            string enPageFileName)
        {
            // 1) Discover the AR web that actually owns this page, and read 
            //    the source layout URL while we still hold that context.
            string arWebServerRelativeUrl = ExtractWebUrlFromPageUrl(arPageServerRelativeUrl);
            string sourceLayoutServerRelativeUrl = null;
            string sourceTitle = null;

            using (SPWeb arWeb = site.OpenWeb(arWebServerRelativeUrl))
            {
                if (!arWeb.Exists)
                    throw new Exception("الموقع العربي غير موجود: " + arWebServerRelativeUrl);

                SPFile arFileCheck = arWeb.GetFile(arPageServerRelativeUrl);
                if (!arFileCheck.Exists)
                    throw new Exception("الصفحة العربية الأصلية غير موجودة");

                PublishingPage arPage = PublishingPage.GetPublishingPage(arFileCheck.Item);
                if (arPage.Layout != null)
                    sourceLayoutServerRelativeUrl = arPage.Layout.ServerRelativeUrl;
                sourceTitle = arPage.Title;
            }

            if (string.IsNullOrEmpty(sourceLayoutServerRelativeUrl))
                throw new Exception("تعذر تحديد تخطيط الصفحة المصدر");

            // 2) Open target EN web AND keep the AR web open simultaneously, 
            //    so that field copying and webpart cloning work in clean contexts.
            using (SPWeb enWeb = site.OpenWeb(enWebServerRelativeUrl))
            {
                if (!enWeb.Exists)
                    throw new Exception("الموقع الإنجليزي المقابل غير موجود: "
                        + enWebServerRelativeUrl);

                if (!PublishingWeb.IsPublishingWeb(enWeb))
                    throw new Exception("الموقع الإنجليزي ليس موقع نشر");

                PublishingWeb enPubWeb = PublishingWeb.GetPublishingWeb(enWeb);

                // Check again in case state changed since last enrichment
                SPFile existing = enWeb.GetFile(enPageServerRelativeUrl);
                if (existing.Exists)
                    throw new Exception("الصفحة EN موجودة بالفعل (ربما أنشئت للتو)");

                enWeb.AllowUnsafeUpdates = true;
                try
                {
                    // 3) Resolve the page layout FRESH in the EN web's context.
                    //    The layout's SPListItem must come from the same SPSite 
                    //    instance we're using for the EN web, or "Add" throws 
                    //    "The specified item does not belong to a list."
                    PageLayout layout = ResolveLayoutInWebContext(
                        enPubWeb, sourceLayoutServerRelativeUrl);
                    if (layout == null)
                        throw new Exception(
                            "تخطيط الصفحة غير متاح في الموقع الإنجليزي: "
                            + sourceLayoutServerRelativeUrl);

                    // 4) Create the EN page using the freshly-bound layout
                    PublishingPage enPage = enPubWeb.GetPublishingPages()
                        .Add(enPageFileName, layout);
                    enPage.Title = sourceTitle;
                    enPage.Update();

                    // 5) Copy publishing field values (HTML, image, summary links, etc.) 
                    //    Re-open the AR web in this scope for fresh field reads.
                    using (SPWeb arWeb = site.OpenWeb(arWebServerRelativeUrl))
                    {
                        SPFile arFile = arWeb.GetFile(arPageServerRelativeUrl);
                        SPListItem arItem = arFile.Item;

                        CopyPublishingFields(arItem, enPage.ListItem);

                        // 6) Clone ControlLoader WebPart(s) preserving UserControlPath
                        CloneControlLoaderWebParts(arFile, enPage.ListItem.File);
                    }

                    // 7) Check in, publish, approve
                    SPFile enFile = enPage.ListItem.File;
                    if (enFile.CheckOutType != SPFile.SPCheckOutType.None)
                        enFile.CheckIn("نسخة EN منشأة تلقائياً من " + arPageServerRelativeUrl,
                            SPCheckinType.MajorCheckIn);

                    if (enFile.Item.ParentList.EnableMinorVersions)
                        enFile.Publish("تم النشر تلقائياً");

                    if (enFile.Item.ParentList.EnableModeration)
                        enFile.Approve("تمت الموافقة تلقائياً");
                }
                finally
                {
                    enWeb.AllowUnsafeUpdates = false;
                }
            }
        }

        /// <summary>
        /// Given a page server-relative URL like "/ar/Pages/home.aspx", returns 
        /// the owning web's server-relative URL ("/ar"). Strips "/Pages/file.aspx".
        /// </summary>
        private string ExtractWebUrlFromPageUrl(string pageServerRelativeUrl)
        {
            if (string.IsNullOrEmpty(pageServerRelativeUrl)) return "/";

            // Find "/Pages/" segment (case-insensitive)
            int pagesIdx = pageServerRelativeUrl.IndexOf(
                "/Pages/", StringComparison.OrdinalIgnoreCase);
            if (pagesIdx < 0)
            {
                // Site Pages library fallback
                int sitePagesIdx = pageServerRelativeUrl.IndexOf(
                    "/SitePages/", StringComparison.OrdinalIgnoreCase);
                if (sitePagesIdx < 0) return "/";
                pagesIdx = sitePagesIdx;
            }

            string webUrl = pageServerRelativeUrl.Substring(0, pagesIdx);
            return string.IsNullOrEmpty(webUrl) ? "/" : webUrl;
        }

        /// <summary>
        /// Loads a PageLayout whose SPListItem is bound to the SAME SPSite/SPWeb 
        /// context as targetPubWeb. This is mandatory — passing a PageLayout that 
        /// was loaded from a different context to PublishingPageCollection.Add 
        /// causes "The specified item does not belong to a list."
        /// </summary>
        private PageLayout ResolveLayoutInWebContext(PublishingWeb targetPubWeb,
            string layoutServerRelativeUrl)
        {
            if (string.IsNullOrEmpty(layoutServerRelativeUrl)) return null;

            // Try the available layouts first (they're already bound to this context)
            try
            {
                foreach (PageLayout pl in targetPubWeb.GetAvailablePageLayouts())
                {
                    if (pl.ServerRelativeUrl.Equals(layoutServerRelativeUrl,
                        StringComparison.OrdinalIgnoreCase))
                        return pl;
                }
            }
            catch { /* fall through to direct load */ }

            // Direct load: get the layout file via the SAME SPSite that owns 
            // targetPubWeb. This guarantees the SPListItem belongs to a list 
            // that's reachable from this context.
            SPSite contextSite = targetPubWeb.Web.Site;
            SPFile layoutFile = contextSite.RootWeb.GetFile(layoutServerRelativeUrl);
            if (!layoutFile.Exists) return null;

            // The layout file's parent list MUST be the Master Page Gallery on 
            // the same site collection. Validate by accessing ParentList.
            SPListItem layoutItem = layoutFile.Item;
            if (layoutItem == null || layoutItem.ParentList == null) return null;

            return new PageLayout(layoutItem);
        }

        /// <summary>
        /// Copies non-system field values from source list item to target. 
        /// Skips read-only, computed, hidden, and identity fields.
        /// </summary>
        private void CopyPublishingFields(SPListItem source, SPListItem target)
        {
            // Field internal names that must NOT be copied
            HashSet<string> skip = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "ID", "ContentTypeId", "ContentType", "FileLeafRef", "FileRef",
        "FileDirRef", "Modified", "Created", "Author", "Editor",
        "_ModerationStatus", "_ModerationComments", "_UIVersion",
        "_UIVersionString", "Attachments", "GUID", "WorkflowVersion",
        "_HasCopyDestinations", "_CopySource", "ParentVersionString",
        "ParentLeafName", "owshiddenversion", "PublishingPageLayout",
        "Title"  // Already set above
    };

            foreach (SPField field in source.Fields)
            {
                if (field.ReadOnlyField) continue;
                if (field.Hidden) continue;
                if (skip.Contains(field.InternalName)) continue;
                if (!target.Fields.ContainsField(field.InternalName)) continue;

                try
                {
                    object val = source[field.InternalName];
                    if (val == null) continue;

                    // Special handling: PublishingPageImage and HTML fields keep AR URLs 
                    // — optionally swap /ar/ → /en/ in image and link refs
                    string strVal = val as string;
                    if (strVal != null && field.Type == SPFieldType.Note)
                    {
                        strVal = RewriteArLinksToEn(strVal);
                        target[field.InternalName] = strVal;
                    }
                    else
                    {
                        target[field.InternalName] = val;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            target.SystemUpdate(false);
        }

        /// <summary>
        /// Rewrites /ar/ paths inside HTML field content to /en/ so that links and 
        /// embedded image src attributes point at the English branch. Only swaps 
        /// /ar/ as a path segment to avoid touching unrelated substrings.
        /// </summary>
        private string RewriteArLinksToEn(string html)
        {
            if (string.IsNullOrEmpty(html)) return html;

            // Swap /ar/ when it appears as a path segment in href/src attributes.
            // Conservative regex: matches "/ar/" preceded by " or ' or = or start.
            string result = System.Text.RegularExpressions.Regex.Replace(
                html,
                @"(?<=[""'=])\/ar\/",
                "/en/",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return result;
        }

        /// <summary>
        /// Reads ControlLoader WebParts from the source page and recreates them on 
        /// the target page, preserving zone, order, and UserControlPath.
        /// </summary>
        private void CloneControlLoaderWebParts(SPFile sourceFile, SPFile targetFile)
        {
            var clones = new List<WebPartCloneInfo>();

            using (SPLimitedWebPartManager srcMgr =
                sourceFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                foreach (System.Web.UI.WebControls.WebParts.WebPart wp in srcMgr.WebParts)
                {
                    Type t = wp.GetType();
                    if (t.Name.IndexOf("ControlLoader", StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    var prop = t.GetProperty("UserControlPath");
                    string ucPath = prop != null ? (prop.GetValue(wp, null) as string) : null;

                    clones.Add(new WebPartCloneInfo
                    {
                        TypeFullName = t.FullName,
                        AssemblyQualifiedName = t.AssemblyQualifiedName,
                        Title = wp.Title,
                        ZoneId = srcMgr.GetZoneID(wp),
                        ZoneIndex = wp.ZoneIndex,
                        UserControlPath = ucPath
                    });
                }
            }

            if (clones.Count == 0) return;

            using (SPLimitedWebPartManager tgtMgr =
                targetFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                foreach (var info in clones)
                {
                    try
                    {
                        Type wpType = Type.GetType(info.AssemblyQualifiedName);
                        if (wpType == null)
                        {

                            continue;
                        }

                        var wp = (System.Web.UI.WebControls.WebParts.WebPart)
                            Activator.CreateInstance(wpType);

                        wp.Title = info.Title;
                        wp.ChromeType =
                            System.Web.UI.WebControls.WebParts.PartChromeType.None;

                        if (!string.IsNullOrEmpty(info.UserControlPath))
                        {
                            var prop = wpType.GetProperty("UserControlPath");
                            if (prop != null && prop.CanWrite)
                                prop.SetValue(wp, info.UserControlPath, null);
                        }

                        tgtMgr.AddWebPart(wp, info.ZoneId, info.ZoneIndex);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private class WebPartCloneInfo
        {
            public string TypeFullName { get; set; }
            public string AssemblyQualifiedName { get; set; }
            public string Title { get; set; }
            public string ZoneId { get; set; }
            public int ZoneIndex { get; set; }
            public string UserControlPath { get; set; }
        }

        #endregion


        /// <summary>
        /// Opens the page's web part manager and looks for a ControlLoader WebPart, 
        /// reading its UserControlPath property via reflection.
        /// </summary>
        private string ExtractControlLoaderPath(SPFile pageFile)
        {
            try
            {
                using (SPLimitedWebPartManager wpm =
                    pageFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                {
                    var paths = new List<string>();
                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpm.WebParts)
                    {
                        Type t = wp.GetType();
                        // Match by type name to be loosely coupled
                        if (t.Name.IndexOf("ControlLoader", StringComparison.OrdinalIgnoreCase) < 0)
                            continue;

                        var prop = t.GetProperty("UserControlPath");
                        if (prop == null) continue;

                        object val = prop.GetValue(wp, null);
                        if (val != null && !string.IsNullOrEmpty(val.ToString()))
                            paths.Add(val.ToString());
                    }
                    return string.Join(" | ", paths.ToArray());
                }
            }
            catch (Exception ex)
            {

                return "";
            }
        }

        #endregion

        #region Save to Admin List

        protected void btnSaveToList_Click(object sender, EventArgs e)
        {
            DataTable dt = PagesData;
            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessage("لا توجد بيانات للحفظ. اعرض الصفحات أولاً.", "error");
                return;
            }

            // Validate form digest once for the request — needed because we 
            // perform writes inside RunWithElevatedPrivileges from a POST.
            try
            {
                SPUtility.ValidateFormDigest();
            }
            catch
            {
                // Fall back to AllowUnsafeUpdates already in place below
            }

            try
            {
                int saved = 0, skipped = 0;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    string adminWebFullUrl = SPContext.Current.Site.MakeFullUrl(ADMIN_SITE_URL);

                    using (SPSite site = new SPSite(adminWebFullUrl))
                    using (SPWeb adminWeb = site.OpenWeb(ADMIN_SITE_URL))
                    {
                        SPList list = EnsurePagesInventoryList(adminWeb);

                        adminWeb.AllowUnsafeUpdates = true;
                        try
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                string pageUrl = SafeStr(row["Url"]);
                                if (PageAlreadyExists(list, pageUrl))
                                {
                                    skipped++;
                                    continue;
                                }

                                SPListItem item = list.AddItem();
                                item["Title"] = SafeStr(row["Title"]);
                                item["PageUrl"] = SafeStr(row["Url"]);
                                item["PageLayout"] = SafeStr(row["PageLayout"]);
                                item["UserControlPath"] = SafeStr(row["UserControlPath"]);
                                item["WebUrl"] = SafeStr(row["WebUrl"]);
                                item["HasEnglishVersion"] = (bool)row["HasEnglishVersion"];
                                item["ExpectedEnUrl"] = SafeStr(row["ExpectedEnUrl"]);
                                item["InventoryDate"] = DateTime.Now;
                                item.Update();
                                saved++;
                            }
                        }
                        finally
                        {
                            adminWeb.AllowUnsafeUpdates = false;
                        }
                    }
                });

                ShowMessage(string.Format(
                    "تم الحفظ بنجاح. تمت إضافة {0} سجل، وتم تخطي {1} سجل موجود مسبقاً.",
                    saved, skipped), "success");
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في الحفظ: " + ex.Message, "error");

            }
        }

        private bool PageAlreadyExists(SPList list, string pageUrl)
        {
            SPQuery q = new SPQuery
            {
                Query = string.Format(
                    @"<Where><Eq><FieldRef Name='PageUrl'/>
                      <Value Type='Text'>{0}</Value></Eq></Where>",
                    System.Security.SecurityElement.Escape(pageUrl)),
                RowLimit = 1
            };
            return list.GetItems(q).Count > 0;
        }

        #endregion

        #region List Provisioning

        /// <summary>
        /// Ensures the PagesInventory list exists in the given web. 
        /// Creates it (with fields) if it doesn't.
        /// </summary>
        private SPList EnsurePagesInventoryList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(PAGES_INVENTORY_LIST);
            bool isNew = false;
            if (list == null)
            {
                web.AllowUnsafeUpdates = true;
                Guid listId = web.Lists.Add(
                    PAGES_INVENTORY_LIST,
                    "جرد صفحات المواقع تحت تطبيق الويب",
                    SPListTemplateType.GenericList);
                list = web.Lists[listId];
                isNew = true;
            }

            // Always ensure fields exist (handles upgrading older lists)
            AddTextField(list, "PageUrl", "Page URL", true);
            AddTextField(list, "PageLayout", "Page Layout", false);
            AddTextField(list, "UserControlPath", "User Control Path", false);
            AddTextField(list, "WebUrl", "Web URL", false);
            AddTextField(list, "ExpectedEnUrl", "Expected EN URL", false);

            EnsureBooleanField(list, "HasEnglishVersion", "Has English Version");

            if (!list.Fields.ContainsField("InventoryDate"))
            {
                string internalName = list.Fields.Add(
                    "InventoryDate", SPFieldType.DateTime, false);
                SPFieldDateTime dtField = (SPFieldDateTime)list.Fields.GetField(internalName);
                dtField.DisplayFormat = SPDateTimeFieldFormatType.DateTime;
                dtField.Title = "Inventory Date";
                dtField.Update();
            }

            if (isNew)
            {
                SPView view = list.DefaultView;
                AddToView(view, "PageUrl");
                AddToView(view, "PageLayout");
                AddToView(view, "UserControlPath");
                AddToView(view, "WebUrl");
                AddToView(view, "HasEnglishVersion");
                AddToView(view, "ExpectedEnUrl");
                AddToView(view, "InventoryDate");
                view.Update();
            }

            list.Update();
            return list;
        }

        private void EnsureBooleanField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return;
            string created = list.Fields.Add(internalName, SPFieldType.Boolean, false);
            SPField fld = list.Fields.GetField(created);
            fld.Title = displayName;
            fld.Update();
        }

        private void AddTextField(SPList list, string internalName,
            string displayName, bool required)
        {
            if (list.Fields.ContainsField(internalName)) return;
            string created = list.Fields.Add(internalName, SPFieldType.Text, required);
            SPField fld = list.Fields.GetField(created);
            fld.Title = displayName;
            fld.Update();
        }

        private void AddToView(SPView view, string fieldName)
        {
            if (!view.ViewFields.Exists(fieldName))
                view.ViewFields.Add(fieldName);
        }

        #endregion

        #region Helpers

        private string SafeStr(object val)
        {
            return val == null ? "" : val.ToString().Trim();
        }

        private void ShowMessage(string text, string cssClass)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "message-panel " + cssClass;
            litMessage.Text = Server.HtmlEncode(text);
        }

        #endregion
    }
}