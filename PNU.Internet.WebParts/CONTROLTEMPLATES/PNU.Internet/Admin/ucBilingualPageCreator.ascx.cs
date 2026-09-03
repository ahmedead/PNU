using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebPartPages;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucBilingualPageCreator : UserControl
    {
        private const string WP_TYPE_NAME = "PNU.Internet.WebParts.ControlLoaderWebPart.ControlLoaderWebPart";
        private const string WP_ASSEMBLY_NAME = "PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3";
        private const string DEFAULT_ZONE_ID = "TopZone";

        private readonly StringBuilder _logBuilder = new StringBuilder();
        private readonly List<string> _createdUrls = new List<string>();
        private int _successCount = 0;
        private int _failCount = 0;

        [Serializable]
        public class MissingPageItem
        {
            public string SourceWebUrl { get; set; }
            public string TargetWebUrl { get; set; }
            public string PageFileName { get; set; }
            public string SourcePageTitle { get; set; }
            public string TargetPageTitle { get; set; }
            public string SourcePageUrl { get; set; }
            public string TargetPageUrl { get; set; }
            public string PageLayoutUrl { get; set; }
            public string PageLayoutFileName { get; set; }

            public bool HasControlLoader { get; set; }
            public string UserControlPath { get; set; }
            public string UserControlProperties { get; set; }
            public string ControlLoaderZoneId { get; set; }
            public int ControlLoaderZoneIndex { get; set; }

            public bool HasContentEditor { get; set; }
            public string ContentEditorHtml { get; set; }
            public string ContentEditorLink { get; set; }
            public string ContentEditorZoneId { get; set; }
            public int ContentEditorZoneIndex { get; set; }

            public bool HasPageContent { get; set; }
            public string PublishingPageContentHtml { get; set; }

            public string Direction { get; set; } // "ArToEn" or "EnToAr"
        }

        private List<MissingPageItem> CurrentMissingPages
        {
            get
            {
                return ViewState["CurrentMissingPages"] as List<MissingPageItem> ?? new List<MissingPageItem>();
            }
            set
            {
                ViewState["CurrentMissingPages"] = value;
            }
        }

        [Serializable]
        public class ExistingPageComparisonItem
        {
            public string PageFileName { get; set; }
            public string SourceWebUrl { get; set; }
            public string TargetWebUrl { get; set; }

            public string ArPageTitle { get; set; }
            public string ArPageUrl { get; set; }
            public string ArPageLayoutUrl { get; set; }
            public string ArPageLayoutFileName { get; set; }
            public bool ArIsDgaLayout { get; set; }
            public bool ArHasPageContent { get; set; }
            public int ArContentLength { get; set; }
            public bool ArHasControlLoader { get; set; }
            public string ArUserControlPath { get; set; }
            public bool ArHasContentEditor { get; set; }

            public string EnPageTitle { get; set; }
            public string EnPageUrl { get; set; }
            public string EnPageLayoutUrl { get; set; }
            public string EnPageLayoutFileName { get; set; }
            public bool EnIsDgaLayout { get; set; }
            public bool EnHasPageContent { get; set; }
            public int EnContentLength { get; set; }
            public bool EnHasControlLoader { get; set; }
            public string EnUserControlPath { get; set; }
            public bool EnHasContentEditor { get; set; }

            public bool LayoutIsDifferent { get; set; }
            public bool ContentIsDifferent { get; set; }

            public string StatusBadgeText { get; set; }
            public string StatusBadgeClass { get; set; }
        }

        private class PageDetailsSnapshot
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public string LayoutUrl { get; set; }
            public string LayoutFileName { get; set; }
            public bool IsDgaLayout { get; set; }
            public bool HasPageContent { get; set; }
            public int ContentLength { get; set; }
            public string ContentHtml { get; set; }
            public bool HasControlLoader { get; set; }
            public string ControlLoaderPath { get; set; }
            public bool HasContentEditor { get; set; }
            public int ContentEditorLength { get; set; }
        }

        private List<ExistingPageComparisonItem> CurrentExistingPages
        {
            get
            {
                return ViewState["CurrentExistingPages"] as List<ExistingPageComparisonItem> ?? new List<ExistingPageComparisonItem>();
            }
            set
            {
                ViewState["CurrentExistingPages"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateSubsites();
            }
        }

        #region Subsite Navigation & Dropdown

        private void PopulateSubsites()
        {
            try
            {
                ddlSubsites.Items.Clear();
                ddlSubsites.Items.Add(new ListItem("-- اختر موقعاً فرعياً من القائمة --", ""));

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                        ? SPContext.Current.Site.Url
                        : "https://pnu.edu.sa";

                    using (SPSite site = new SPSite(siteUrl))
                    using (SPWeb rootWeb = site.RootWeb)
                    {
                        // Prioritize /ar/ web as the master content site
                        SPWeb arRoot = null;
                        try { arRoot = site.OpenWeb("/ar", false); } catch { }

                        if (arRoot != null && arRoot.Exists)
                        {
                            ddlSubsites.Items.Add(new ListItem("--- [المواقع العربية /ar/ - المصدر الأساسي للمحتوى] ---", ""));
                            AddWebAndChildrenToDropdown(arRoot, 0);

                            // Optional English root list below
                            SPWeb enRoot = null;
                            try { enRoot = site.OpenWeb("/en", false); } catch { }
                            if (enRoot != null && enRoot.Exists)
                            {
                                ddlSubsites.Items.Add(new ListItem("--- [المواقع الإنجليزية /en/] ---", ""));
                                AddWebAndChildrenToDropdown(enRoot, 0);
                            }
                        }
                        else
                        {
                            foreach (SPWeb sub in rootWeb.Webs)
                            {
                                AddWebAndChildrenToDropdown(sub, 0);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                LogMessage("فشل تحميل قائمة المواقع الفرعية: " + ex.Message, "danger");
                RenderExecutionResults(0, 0, 0);
            }
        }

        private void AddWebAndChildrenToDropdown(SPWeb web, int depth)
        {
            try
            {
                string indent = new string('-', depth * 2);
                string text = string.Format("{0} {1} ({2})", indent, web.Title, web.ServerRelativeUrl);
                ddlSubsites.Items.Add(new ListItem(text, web.ServerRelativeUrl));

                foreach (SPWeb child in web.Webs)
                {
                    AddWebAndChildrenToDropdown(child, depth + 1);
                }
            }
            catch { }
        }

        #endregion

        #region Scanning & Detection Methods

        protected void btnScanDropdownSite_Click(object sender, EventArgs e)
        {
            string selectedUrl = ddlSubsites.SelectedValue;
            if (string.IsNullOrWhiteSpace(selectedUrl))
            {
                LogMessage("يرجى اختيار موقع فرعي من القائمة أولاً.", "warning");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            ScanAndDisplayMissingPages(selectedUrl);
        }

        protected void btnScanUrlSite_Click(object sender, EventArgs e)
        {
            string rawUrl = txtSiteUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                LogMessage("يرجى إدخال رابط الموقع المراد فحصه.", "warning");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            ScanAndDisplayMissingPages(rawUrl);
        }

        protected void btnCompareExistingDropdown_Click(object sender, EventArgs e)
        {
            string selectedUrl = ddlSubsites.SelectedValue;
            if (string.IsNullOrWhiteSpace(selectedUrl))
            {
                LogMessage("يرجى اختيار موقع فرعي من القائمة أولاً.", "warning");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            CompareExistingPages(selectedUrl);
        }

        protected void btnCompareExistingUrl_Click(object sender, EventArgs e)
        {
            string rawUrl = txtSiteUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                LogMessage("يرجى إدخال رابط الموقع المراد مقارنة صفحاته.", "warning");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            CompareExistingPages(rawUrl);
        }

        protected void btnProcessDirectPages_Click(object sender, EventArgs e)
        {
            string rawText = txtDirectPages.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawText))
            {
                LogMessage("يرجى إدخال روابط الصفحات في الحقل المخصص.", "warning");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            ProcessDirectPastedPages(rawText);
        }

        private void ScanAndDisplayMissingPages(string inputUrl)
        {
            ResetUi();
            string normalizedWebUrl = NormalizeWebUrl(inputUrl);
            string pairedWebUrl = GetPairedWebUrl(normalizedWebUrl);

            if (string.IsNullOrEmpty(pairedWebUrl))
            {
                LogMessage(string.Format("تعذر اشتقاق الرابط المقابل (/ar/ أو /en/) للرابط المدخل: {0}", inputUrl), "danger");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            string arWebUrl = normalizedWebUrl.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase) ? normalizedWebUrl : pairedWebUrl;
            string enWebUrl = normalizedWebUrl.StartsWith("/en/", StringComparison.OrdinalIgnoreCase) ? normalizedWebUrl : pairedWebUrl;

            bool onlyArToEn = ddlSyncDirection.SelectedValue == "ArToEn";

            LogMessage(string.Format("بدء فحص الصفحات المفقودة (المصدر الأساسي: الموقع العربي <code>{0}</code> ➔ المستهدف: <code>{1}</code>)...", arWebUrl, enWebUrl), "info");

            List<MissingPageItem> missingList = new List<MissingPageItem>();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb arWeb = site.OpenWeb(arWebUrl, false))
                    using (SPWeb enWeb = site.OpenWeb(enWebUrl, false))
                    {
                        if (arWeb == null || !arWeb.Exists)
                        {
                            LogMessage(string.Format("الموقع العربي غير موجود: {0}", arWebUrl), "danger");
                            return;
                        }
                        if (enWeb == null || !enWeb.Exists)
                        {
                            LogMessage(string.Format("الموقع الإنجليزي غير موجود: {0}", enWebUrl), "danger");
                            return;
                        }

                        // 1. Pages existing in AR (Master Content Source) but missing in EN
                        var arPages = GetPagesDict(arWeb);
                        var enPages = GetPagesDict(enWeb);

                        LogMessage(string.Format("إحصائيات صفحات النشر المكتشفة:<br />&bull; الموقع العربي (<code>{0}</code>): تم العثور على <b>{1}</b> صفحة نشر.<br />&bull; الموقع الإنجليزي (<code>{2}</code>): تم العثور على <b>{3}</b> صفحة نشر.",
                            arWebUrl, arPages.Count, enWebUrl, enPages.Count), "info");

                        foreach (var kvp in arPages)
                        {
                            string fileName = kvp.Key;
                            if (!enPages.ContainsKey(fileName))
                            {
                                var item = InspectSourcePage(site, arWeb, enWeb, kvp.Value, fileName, "ArToEn");
                                if (item != null) missingList.Add(item);
                            }
                        }

                        // 2. Pages existing in EN but missing in AR (only if bidirectional selected)
                        if (!onlyArToEn)
                        {
                            foreach (var kvp in enPages)
                            {
                                string fileName = kvp.Key;
                                if (!arPages.ContainsKey(fileName))
                                {
                                    var item = InspectSourcePage(site, enWeb, arWeb, kvp.Value, fileName, "EnToAr");
                                    if (item != null) missingList.Add(item);
                                }
                            }
                        }
                    }
                }
            });

            BindMissingPagesList(missingList);
        }

        private void ProcessDirectPastedPages(string text)
        {
            ResetUi();
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var missingList = new List<MissingPageItem>();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    foreach (string l in lines)
                    {
                        string line = l.Trim();
                        if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;

                        string pageUrl = NormalizePageUrl(line);
                        string webPath = ExtractWebPath(pageUrl);
                        string fileName = Path.GetFileName(pageUrl);

                        if (string.IsNullOrEmpty(fileName)) continue;

                        // Since Arabic site is the master content source:
                        // Ensure source is read from the Arabic web, and target is English web
                        string arWebPath = webPath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase) 
                            ? webPath 
                            : GetPairedWebUrl(webPath);
                        string enWebPath = webPath.StartsWith("/en/", StringComparison.OrdinalIgnoreCase) 
                            ? webPath 
                            : GetPairedWebUrl(webPath);

                        if (string.IsNullOrEmpty(arWebPath) || string.IsNullOrEmpty(enWebPath))
                        {
                            LogMessage(string.Format("تعذر تحديد مسار الموقع العربي أو الإنجليزي للصفحة: {0}", pageUrl), "warning");
                            continue;
                        }

                        using (SPWeb arWeb = site.OpenWeb(arWebPath, false))
                        using (SPWeb enWeb = site.OpenWeb(enWebPath, false))
                        {
                            if (arWeb == null || !arWeb.Exists)
                            {
                                LogMessage(string.Format("الموقع العربي المصدر غير موجود: {0}", arWebPath), "warning");
                                continue;
                            }
                            if (enWeb == null || !enWeb.Exists)
                            {
                                LogMessage(string.Format("الموقع الإنجليزي المستهدف غير موجود: {0}", enWebPath), "warning");
                                continue;
                            }

                            // Check Arabic page file as primary master content source
                            SPFile arFile = GetPageFileInWeb(arWeb, fileName);
                            if (arFile == null || !arFile.Exists)
                            {
                                arFile = ResolvePageFile(site, arWeb, arWeb.ServerRelativeUrl.TrimEnd('/') + "/Pages/" + fileName);
                            }
                            if (arFile == null || !arFile.Exists)
                            {
                                arFile = ResolvePageFile(site, arWeb, pageUrl);
                            }

                            if (arFile == null || !arFile.Exists)
                            {
                                LogMessage(string.Format("لم يتم العثور على الصفحة في الموقع العربي الأساسي: {0} ({1})", fileName, arWebPath), "warning");
                                continue;
                            }

                            // Check if target page already exists on English web
                            SPFile enFile = GetPageFileInWeb(enWeb, fileName);

                            if (enFile != null && enFile.Exists && !chkOverwrite.Checked)
                            {
                                LogMessage(string.Format("الصفحة الإنجليزية موجودة بالفعل وتم تخطيها: {0}", enFile.ServerRelativeUrl), "info");
                                continue;
                            }

                            var item = InspectSourcePage(site, arWeb, enWeb, arFile, fileName, "ArToEn");
                            if (item != null) missingList.Add(item);
                        }
                    }
                }
            });

            BindMissingPagesList(missingList);
        }

        private MissingPageItem InspectSourcePage(SPSite site, SPWeb srcWeb, SPWeb tgtWeb, SPFile srcFile, string fileName, string direction)
        {
            try
            {
                if (srcFile == null || !srcFile.Exists)
                    return null;

                PublishingPage srcPubPage = null;
                try
                {
                    if (srcFile.Item != null)
                    {
                        srcPubPage = PublishingPage.GetPublishingPage(srcFile.Item);
                    }
                }
                catch { }

                if (srcPubPage == null)
                {
                    try
                    {
                        PublishingWeb srcPubWeb = PublishingWeb.GetPublishingWeb(srcWeb);
                        if (srcPubWeb != null)
                        {
                            srcPubPage = srcPubWeb.GetPublishingPages()[fileName];
                        }
                    }
                    catch { }
                }

                string targetPagesListName = GetPagesListName(tgtWeb);
                string pageTitle = srcPubPage != null && !string.IsNullOrEmpty(srcPubPage.Title)
                    ? srcPubPage.Title
                    : (srcFile.Title ?? Path.GetFileNameWithoutExtension(fileName));

                var item = new MissingPageItem
                {
                    SourceWebUrl = srcWeb.ServerRelativeUrl,
                    TargetWebUrl = tgtWeb.ServerRelativeUrl,
                    PageFileName = fileName,
                    SourcePageTitle = pageTitle,
                    SourcePageUrl = srcFile.ServerRelativeUrl,
                    TargetPageUrl = tgtWeb.ServerRelativeUrl.TrimEnd('/') + "/" + targetPagesListName + "/" + fileName,
                    Direction = direction
                };

                // Page Layout
                string layoutUrl = null;
                try
                {
                    string layoutFieldVal = GetItemFieldValue(srcFile.Item, "PublishingPageLayout");
                    if (!string.IsNullOrEmpty(layoutFieldVal))
                    {
                        layoutUrl = ExtractUrlFromLayoutField(layoutFieldVal);
                    }
                }
                catch { }

                if (string.IsNullOrEmpty(layoutUrl))
                {
                    try
                    {
                        if (srcPubPage != null && srcPubPage.Layout != null)
                        {
                            layoutUrl = srcPubPage.Layout.ServerRelativeUrl;
                        }
                    }
                    catch { }
                }

                if (string.IsNullOrEmpty(layoutUrl))
                {
                    layoutUrl = "/_catalogs/masterpage/NewSideMenu.aspx";
                }

                item.PageLayoutUrl = layoutUrl;
                item.PageLayoutFileName = Path.GetFileName(layoutUrl);

                // Page Content
                string pageContent = srcPubPage != null ? GetPageContentHtml(srcPubPage) : GetItemFieldValue(srcFile.Item, "PublishingPageContent");
                if (!string.IsNullOrWhiteSpace(pageContent))
                {
                    item.HasPageContent = true;
                    item.PublishingPageContentHtml = pageContent;
                }

                // Web Parts Inspection
                try
                {
                    using (SPLimitedWebPartManager wpManager = srcFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                    {
                        foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpManager.WebParts)
                        {
                            string wpTypeName = wp.GetType().FullName;

                            // 1. ControlLoaderWebPart
                            if (wpTypeName.IndexOf("ControlLoaderWebPart", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                item.HasControlLoader = true;
                                item.ControlLoaderZoneId = wpManager.GetZoneID(wp);
                                if (string.IsNullOrEmpty(item.ControlLoaderZoneId)) item.ControlLoaderZoneId = DEFAULT_ZONE_ID;
                                item.ControlLoaderZoneIndex = wp.ZoneIndex;

                                var pathProp = wp.GetType().GetProperty("UserControlPath");
                                if (pathProp != null)
                                {
                                    item.UserControlPath = Convert.ToString(pathProp.GetValue(wp, null));
                                }

                                var propsProp = wp.GetType().GetProperty("UserControlProperties");
                                if (propsProp != null)
                                {
                                    item.UserControlProperties = Convert.ToString(propsProp.GetValue(wp, null));
                                }
                            }

                            // 2. ContentEditorWebPart
                            if (wp is ContentEditorWebPart cewp)
                            {
                                item.HasContentEditor = true;
                                item.ContentEditorZoneId = wpManager.GetZoneID(cewp);
                                if (string.IsNullOrEmpty(item.ContentEditorZoneId)) item.ContentEditorZoneId = DEFAULT_ZONE_ID;
                                item.ContentEditorZoneIndex = cewp.ZoneIndex;

                                if (cewp.Content != null)
                                {
                                    string xml = cewp.Content.InnerXml;
                                    item.ContentEditorHtml = !string.IsNullOrWhiteSpace(xml) ? xml : cewp.Content.InnerText;
                                }
                                if (!string.IsNullOrEmpty(cewp.ContentLink))
                                {
                                    item.ContentEditorLink = cewp.ContentLink;
                                }
                            }
                        }
                    }
                }
                catch { }

                item.TargetPageTitle = item.SourcePageTitle;
                return item;
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("تحذير عند فحص ({0}): {1}", fileName, ex.Message), "warning");
                return null;
            }
        }

        private void BindMissingPagesList(List<MissingPageItem> missingList)
        {
            CurrentMissingPages = missingList;
            litMissingCount.Text = missingList.Count.ToString();

            if (missingList.Count == 0)
            {
                pnlMissingPages.Visible = false;
                LogMessage("ممتاز! لا توجد صفحات مفقودة بين اللغتين في الموقع المحدد.", "success");
            }
            else
            {
                pnlMissingPages.Visible = true;
                rptMissingPages.DataSource = missingList;
                rptMissingPages.DataBind();
                LogMessage(string.Format("تم العثور على ({0}) صفحة مفقودة جاهزة للإنشاء والمزامنة.", missingList.Count), "info");
            }

            RenderExecutionResults(missingList.Count, 0, 0);
        }

        #endregion

        #region Existing Pages Comparison & Inspection

        public static bool IsDgaLayout(string layoutName)
        {
            if (string.IsNullOrEmpty(layoutName)) return false;
            string l = layoutName.ToLowerInvariant();
            return l.Contains("dganewblankwebpartpage") || l.Contains("newsidemenu") || l.Contains("dga");
        }

        private PageDetailsSnapshot InspectSinglePageSnapshot(SPWeb web, SPFile file, string fileName)
        {
            var snap = new PageDetailsSnapshot
            {
                FileName = fileName,
                Url = file != null ? file.ServerRelativeUrl : string.Empty,
                Title = file != null && !string.IsNullOrEmpty(file.Title) ? file.Title : Path.GetFileNameWithoutExtension(fileName)
            };

            if (file == null || !file.Exists) return snap;

            PublishingPage pubPage = null;
            try
            {
                if (file.Item != null)
                {
                    pubPage = PublishingPage.GetPublishingPage(file.Item);
                }
            }
            catch { }

            if (pubPage == null)
            {
                try
                {
                    PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                    if (pubWeb != null) pubPage = pubWeb.GetPublishingPages()[fileName];
                }
                catch { }
            }

            if (pubPage != null && !string.IsNullOrEmpty(pubPage.Title))
            {
                snap.Title = pubPage.Title;
            }

            // Layout
            string layoutUrl = null;
            try
            {
                string layoutFieldVal = GetItemFieldValue(file.Item, "PublishingPageLayout");
                if (!string.IsNullOrEmpty(layoutFieldVal))
                {
                    layoutUrl = ExtractUrlFromLayoutField(layoutFieldVal);
                }
            }
            catch { }

            if (string.IsNullOrEmpty(layoutUrl))
            {
                try
                {
                    if (pubPage != null && pubPage.Layout != null)
                    {
                        layoutUrl = pubPage.Layout.ServerRelativeUrl;
                    }
                }
                catch { }
            }

            if (string.IsNullOrEmpty(layoutUrl))
            {
                layoutUrl = "/_catalogs/masterpage/NewSideMenu.aspx";
            }

            snap.LayoutUrl = layoutUrl;
            snap.LayoutFileName = Path.GetFileName(layoutUrl);
            snap.IsDgaLayout = IsDgaLayout(snap.LayoutFileName);

            // Content
            string pageContent = pubPage != null ? GetPageContentHtml(pubPage) : GetItemFieldValue(file.Item, "PublishingPageContent");
            if (!string.IsNullOrWhiteSpace(pageContent))
            {
                snap.HasPageContent = true;
                snap.ContentLength = pageContent.Length;
                snap.ContentHtml = pageContent;
            }

            // Web Parts
            try
            {
                using (SPLimitedWebPartManager wpManager = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
                {
                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpManager.WebParts)
                    {
                        string wpTypeName = wp.GetType().FullName;
                        if (wpTypeName.IndexOf("ControlLoaderWebPart", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            snap.HasControlLoader = true;
                            var pathProp = wp.GetType().GetProperty("UserControlPath");
                            if (pathProp != null)
                            {
                                snap.ControlLoaderPath = Convert.ToString(pathProp.GetValue(wp, null));
                            }
                        }
                        else if (wp is ContentEditorWebPart cewp)
                        {
                            snap.HasContentEditor = true;
                            if (cewp.Content != null)
                            {
                                string xml = cewp.Content.InnerXml;
                                string html = !string.IsNullOrWhiteSpace(xml) ? xml : cewp.Content.InnerText;
                                snap.ContentEditorLength = html != null ? html.Length : 0;
                            }
                        }
                    }
                }
            }
            catch { }

            return snap;
        }

        private void CompareExistingPages(string inputUrl)
        {
            ResetUi();
            pnlMissingPages.Visible = false;

            string normalizedWebUrl = NormalizeWebUrl(inputUrl);
            string pairedWebUrl = GetPairedWebUrl(normalizedWebUrl);

            if (string.IsNullOrEmpty(pairedWebUrl))
            {
                LogMessage(string.Format("تعذر اشتقاق الرابط المقابل (/ar/ أو /en/) للرابط المدخل: {0}", inputUrl), "danger");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            string arWebUrl = normalizedWebUrl.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase) ? normalizedWebUrl : pairedWebUrl;
            string enWebUrl = normalizedWebUrl.StartsWith("/en/", StringComparison.OrdinalIgnoreCase) ? normalizedWebUrl : pairedWebUrl;

            LogMessage(string.Format("بدء مقارنة الصفحات الموجودة في كلا الموقعين العربي (<code>{0}</code>) والإنجليزي (<code>{1}</code>)...", arWebUrl, enWebUrl), "info");

            List<ExistingPageComparisonItem> comparisonList = new List<ExistingPageComparisonItem>();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                using (SPWeb arWeb = site.OpenWeb(arWebUrl, false))
                using (SPWeb enWeb = site.OpenWeb(enWebUrl, false))
                {
                    if (arWeb == null || !arWeb.Exists)
                    {
                        LogMessage(string.Format("الموقع العربي غير موجود: {0}", arWebUrl), "danger");
                        return;
                    }
                    if (enWeb == null || !enWeb.Exists)
                    {
                        LogMessage(string.Format("الموقع الإنجليزي غير موجود: {0}", enWebUrl), "danger");
                        return;
                    }

                    var arPages = GetPagesDict(arWeb);
                    var enPages = GetPagesDict(enWeb);

                    LogMessage(string.Format("تم فحص الصفحات المشتركة: تم العثور على <b>{0}</b> صفحة في العربي و <b>{1}</b> صفحة في الإنجليزي.",
                        arPages.Count, enPages.Count), "info");

                    foreach (var kvp in arPages)
                    {
                        try
                        {
                            string fileName = kvp.Key;
                            if (enPages.ContainsKey(fileName))
                            {
                                SPFile arFile = kvp.Value;
                                SPFile enFile = enPages[fileName];

                                var arSnap = InspectSinglePageSnapshot(arWeb, arFile, fileName);
                                var enSnap = InspectSinglePageSnapshot(enWeb, enFile, fileName);

                                bool layoutDiff = !string.Equals(arSnap.LayoutFileName, enSnap.LayoutFileName, StringComparison.OrdinalIgnoreCase);

                                bool contentDiff = false;
                                if (arSnap.HasPageContent && (!enSnap.HasPageContent || enSnap.ContentLength < 15))
                                    contentDiff = true;
                                else if (arSnap.HasContentEditor && (!enSnap.HasContentEditor || enSnap.ContentEditorLength < 15))
                                    contentDiff = true;
                                else if (arSnap.HasControlLoader != enSnap.HasControlLoader)
                                    contentDiff = true;
                                else if (arSnap.HasControlLoader && enSnap.HasControlLoader && !string.Equals(arSnap.ControlLoaderPath, enSnap.ControlLoaderPath, StringComparison.OrdinalIgnoreCase))
                                    contentDiff = true;

                                string badgeText = "متطابق تماماً";
                                string badgeClass = "bg-success text-white";

                                if (layoutDiff && contentDiff)
                                {
                                    badgeText = "اختلاف في التخطيط والمحتوى";
                                    badgeClass = "bg-danger text-white";
                                }
                                else if (layoutDiff)
                                {
                                    badgeText = "اختلاف في التخطيط (Mismatch)";
                                    badgeClass = "bg-danger text-white";
                                }
                                else if (contentDiff)
                                {
                                    badgeText = "اختلاف أو نقص في المحتوى";
                                    badgeClass = "bg-warning text-dark";
                                }

                                var compItem = new ExistingPageComparisonItem
                                {
                                    PageFileName = fileName,
                                    SourceWebUrl = arWeb.ServerRelativeUrl,
                                    TargetWebUrl = enWeb.ServerRelativeUrl,

                                    ArPageTitle = arSnap.Title,
                                    ArPageUrl = arSnap.Url,
                                    ArPageLayoutUrl = arSnap.LayoutUrl,
                                    ArPageLayoutFileName = arSnap.LayoutFileName,
                                    ArIsDgaLayout = arSnap.IsDgaLayout,
                                    ArHasPageContent = arSnap.HasPageContent,
                                    ArContentLength = arSnap.ContentLength,
                                    ArHasControlLoader = arSnap.HasControlLoader,
                                    ArUserControlPath = arSnap.ControlLoaderPath,
                                    ArHasContentEditor = arSnap.HasContentEditor,

                                    EnPageTitle = enSnap.Title,
                                    EnPageUrl = enSnap.Url,
                                    EnPageLayoutUrl = enSnap.LayoutUrl,
                                    EnPageLayoutFileName = enSnap.LayoutFileName,
                                    EnIsDgaLayout = enSnap.IsDgaLayout,
                                    EnHasPageContent = enSnap.HasPageContent,
                                    EnContentLength = enSnap.ContentLength,
                                    EnHasControlLoader = enSnap.HasControlLoader,
                                    EnUserControlPath = enSnap.ControlLoaderPath,
                                    EnHasContentEditor = enSnap.HasContentEditor,

                                    LayoutIsDifferent = layoutDiff,
                                    ContentIsDifferent = contentDiff,
                                    StatusBadgeText = badgeText,
                                    StatusBadgeClass = badgeClass
                                };

                                comparisonList.Add(compItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMessage(string.Format("تحذير عند مقارنة الصفحة ({0}): {1}", kvp.Key, ex.Message), "warning");
                        }
                    }
                }
            });

            CurrentExistingPages = comparisonList;
            BindExistingPagesList(comparisonList);
            RenderExecutionResults(comparisonList.Count, comparisonList.Count(x => !x.LayoutIsDifferent && !x.ContentIsDifferent), comparisonList.Count(x => x.LayoutIsDifferent || x.ContentIsDifferent));
        }

        private void BindExistingPagesList(List<ExistingPageComparisonItem> list)
        {
            if (list == null) list = new List<ExistingPageComparisonItem>();

            int total = list.Count;
            int matching = list.Count(x => !x.LayoutIsDifferent);
            int layoutDiff = list.Count(x => x.LayoutIsDifferent);
            int contentDiff = list.Count(x => x.ContentIsDifferent);

            litTotalExisting.Text = total.ToString();
            litMatchingLayouts.Text = matching.ToString();
            litDifferentLayouts.Text = layoutDiff.ToString();
            litContentDifferences.Text = contentDiff.ToString();
            litExistingCount.Text = total.ToString();

            // Apply filter
            string filter = ddlExistingFilter != null ? ddlExistingFilter.SelectedValue : "All";
            IEnumerable<ExistingPageComparisonItem> filtered = list;

            if (filter == "DifferentLayouts")
                filtered = list.Where(x => x.LayoutIsDifferent);
            else if (filter == "DifferentContent")
                filtered = list.Where(x => x.ContentIsDifferent);
            else if (filter == "LegacyLayouts")
                filtered = list.Where(x => !x.ArIsDgaLayout || !x.EnIsDgaLayout);
            else if (filter == "MatchingAll")
                filtered = list.Where(x => !x.LayoutIsDifferent && !x.ContentIsDifferent);

            rptExistingPages.DataSource = filtered.ToList();
            rptExistingPages.DataBind();

            pnlExistingPages.Visible = true;
        }

        protected void ddlExistingFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindExistingPagesList(CurrentExistingPages);
        }

        protected void btnExportExistingPages_Click(object sender, EventArgs e)
        {
            var pages = CurrentExistingPages;
            if (pages == null || pages.Count == 0) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sb.AppendLine("<head><meta charset=\"utf-8\"><style>");
            sb.AppendLine("body { font-family: Segoe UI, Tahoma, sans-serif; direction: rtl; }");
            sb.AppendLine("th { background-color: #007848; color: white; padding: 8px; border: 1px solid #ccc; }");
            sb.AppendLine("td { padding: 6px; border: 1px solid #ccc; vertical-align: top; }");
            sb.AppendLine(".diff { background-color: #ffe6e6; color: #cc0000; font-weight: bold; }");
            sb.AppendLine(".match { background-color: #e6f7ed; color: #007848; }");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine("<h2>تقرير مقارنة تخطيط ومحتوى الصفحات الموجودة (/ar/ & /en/)</h2>");
            sb.AppendLine("<p>تاريخ الاستخراج: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | إجمالي الصفحات: " + pages.Count + "</p>");
            sb.AppendLine("<table>");
            sb.AppendLine("<thead><tr>");
            sb.AppendLine("<th>#</th>");
            sb.AppendLine("<th>اسم الملف</th>");
            sb.AppendLine("<th>عنوان الصفحة العربي</th>");
            sb.AppendLine("<th>رابط الصفحة العربي</th>");
            sb.AppendLine("<th>تخطيط الصفحة العربي</th>");
            sb.AppendLine("<th>نوع التخطيط العربي</th>");
            sb.AppendLine("<th>محتوى العربي</th>");
            sb.AppendLine("<th>عنوان الصفحة الإنجليزي</th>");
            sb.AppendLine("<th>رابط الصفحة الإنجليزي</th>");
            sb.AppendLine("<th>تخطيط الصفحة الإنجليزي</th>");
            sb.AppendLine("<th>نوع التخطيط الإنجليزي</th>");
            sb.AppendLine("<th>محتوى الإنجليزي</th>");
            sb.AppendLine("<th>حالة التخطيط</th>");
            sb.AppendLine("<th>حالة المحتوى</th>");
            sb.AppendLine("</tr></thead><tbody>");

            int idx = 1;
            foreach (var p in pages)
            {
                sb.AppendLine("<tr>");
                sb.AppendFormat("<td>{0}</td>", idx++);
                sb.AppendFormat("<td>{0}</td>", p.PageFileName);
                sb.AppendFormat("<td>{0}</td>", Server.HtmlEncode(p.ArPageTitle));
                sb.AppendFormat("<td>{0}</td>", p.ArPageUrl);
                sb.AppendFormat("<td>{0}</td>", p.ArPageLayoutFileName);
                sb.AppendFormat("<td>{0}</td>", p.ArIsDgaLayout ? "DGA" : "Legacy");
                sb.AppendFormat("<td>{0}</td>", p.ArHasPageContent ? string.Format("محتوى ({0} حرف)", p.ArContentLength) : "فارغ");
                sb.AppendFormat("<td>{0}</td>", Server.HtmlEncode(p.EnPageTitle));
                sb.AppendFormat("<td>{0}</td>", p.EnPageUrl);
                sb.AppendFormat("<td>{0}</td>", p.EnPageLayoutFileName);
                sb.AppendFormat("<td>{0}</td>", p.EnIsDgaLayout ? "DGA" : "Legacy");
                sb.AppendFormat("<td>{0}</td>", p.EnHasPageContent ? string.Format("محتوى ({0} حرف)", p.EnContentLength) : "فارغ");
                sb.AppendFormat("<td class='{0}'>{1}</td>", p.LayoutIsDifferent ? "diff" : "match", p.LayoutIsDifferent ? "تخطيط مختلف (Mismatch)" : "تخطيط مطابق");
                sb.AppendFormat("<td class='{0}'>{1}</td>", p.ContentIsDifferent ? "diff" : "match", p.ContentIsDifferent ? "محتوى مختلف أو ناقص" : "محتوى مطابق");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table></body></html>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment;filename=Existing_Pages_Comparison_{0}.xls", DateTime.Now.ToString("yyyyMMdd_HHmm")));
            Response.Charset = "utf-8";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btnSyncSelectedLayouts_Click(object sender, EventArgs e)
        {
            var pages = CurrentExistingPages;
            if (pages == null || pages.Count == 0) return;

            var selectedItems = new List<ExistingPageComparisonItem>();
            foreach (RepeaterItem ri in rptExistingPages.Items)
            {
                var chk = ri.FindControl("chkSelectExistingPage") as CheckBox;
                var hf = ri.FindControl("hfExistingIndex") as HiddenField;
                if (chk != null && chk.Checked && hf != null)
                {
                    int index;
                    if (int.TryParse(hf.Value, out index) && index >= 0 && index < pages.Count)
                    {
                        selectedItems.Add(pages[index]);
                    }
                }
            }

            if (selectedItems.Count == 0)
            {
                LogMessage("يرجى تحديد صفحة واحدة على الأقل للمزامنة.", "warning");
                RenderExecutionResults(0, 0, 0);
                return;
            }

            ResetUi();
            int success = 0;
            int failed = 0;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    foreach (var item in selectedItems)
                    {
                        try
                        {
                            using (SPWeb enWeb = site.OpenWeb(item.TargetWebUrl, false))
                            {
                                if (enWeb == null || !enWeb.Exists)
                                {
                                    failed++;
                                    LogMessage(string.Format("تعذر فتح الموقع الإنجليزي: {0}", item.TargetWebUrl), "danger");
                                    continue;
                                }

                                enWeb.AllowUnsafeUpdates = true;
                                try
                                {
                                    PublishingWeb enPubWeb = PublishingWeb.GetPublishingWeb(enWeb);
                                    if (enPubWeb == null)
                                    {
                                        failed++;
                                        LogMessage(string.Format("الموقع ليس Publishing Web: {0}", item.TargetWebUrl), "danger");
                                        continue;
                                    }

                                    PublishingPage enPubPage = null;
                                    try { enPubPage = enPubWeb.GetPublishingPages()[item.PageFileName]; } catch { }

                                    if (enPubPage == null)
                                    {
                                        SPFile enFile = GetPageFileInWeb(enWeb, item.PageFileName);
                                        if (enFile != null) enPubPage = PublishingPage.GetPublishingPage(enFile.Item);
                                    }

                                    if (enPubPage == null)
                                    {
                                        failed++;
                                        LogMessage(string.Format("الصفحة غير موجودة في الموقع الإنجليزي: {0}", item.PageFileName), "danger");
                                        continue;
                                    }

                                    SPFile enPageFile = enPubPage.ListItem.File;
                                    if (enPageFile.CheckOutType == SPFile.SPCheckOutType.None)
                                    {
                                        enPageFile.CheckOut();
                                    }

                                    // Resolve target layout (matches Arabic layout)
                                    PageLayout targetLayout = ResolvePageLayout(site, enPubWeb, item.ArPageLayoutUrl);
                                    if (targetLayout != null)
                                    {
                                        enPubPage.Layout = targetLayout;
                                    }

                                    enPubPage.Update();

                                    if (enPageFile.CheckOutType != SPFile.SPCheckOutType.None)
                                    {
                                        enPageFile.CheckIn("تمت مزامنة تخطيط الصفحة مع النسخة العربية عبر أداة الصفحات ثنائية اللغة", SPCheckinType.MajorCheckIn);
                                    }

                                    if (enPageFile.Item.ParentList.EnableMinorVersions)
                                    {
                                        enPageFile.Publish("تم النشر التلقائي لمزامنة التخطيط");
                                    }

                                    if (enPageFile.Item.ParentList.EnableModeration)
                                    {
                                        enPageFile.Approve("تم الاعتماد التلقائي لمزامنة التخطيط");
                                    }

                                    success++;
                                    LogMessage(string.Format("نجاح: تم تحديث تخطيط الصفحة الإنجليزية ({0}) إلى ({1})", item.PageFileName, item.ArPageLayoutFileName), "success");
                                }
                                finally
                                {
                                    enWeb.AllowUnsafeUpdates = false;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            LogMessage(string.Format("خطأ أثناء تحديث ({0}): {1}", item.PageFileName, ex.Message), "danger");
                        }
                    }
                }
            });

            RenderExecutionResults(selectedItems.Count, success, failed);

            // Re-compare to refresh UI
            if (selectedItems.Count > 0)
            {
                CompareExistingPages(selectedItems[0].SourceWebUrl);
            }
        }

        #endregion

        #region Page Creation Execution

        protected void btnExportMissingPages_Click(object sender, EventArgs e)
        {
            var list = CurrentMissingPages;
            if (list == null || list.Count == 0)
            {
                LogMessage("لا توجد صفحات مفقودة لتصديرها.", "warning");
                return;
            }

            try
            {
                string siteName = "PNU";
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    siteName = SPContext.Current.Web.Title;
                }

                string fileName = string.Format("MissingPages_{0}_{1}.csv",
                    System.Text.RegularExpressions.Regex.Replace(siteName, @"[^\w\-]", "_"),
                    DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                StringBuilder sb = new StringBuilder();
                // UTF-8 BOM for Arabic support in Excel
                sb.Append("\uFEFF");
                sb.AppendLine("م,اسم الملف,عنوان الصفحة,الموقع المصدر,رابط الصفحة المصدر,الموقع المستهدف,رابط الصفحة المستهدفة,تخطيط الصفحة,الاتجاه,يحتوي ControlLoader,مسار ControlLoader,يحتوي ContentEditor,يحتوي محتوى صفحة");

                int idx = 1;
                foreach (var item in list)
                {
                    sb.AppendLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\"",
                        idx++,
                        EscapeCsv(item.PageFileName),
                        EscapeCsv(item.SourcePageTitle),
                        EscapeCsv(item.SourceWebUrl),
                        EscapeCsv(item.SourcePageUrl),
                        EscapeCsv(item.TargetWebUrl),
                        EscapeCsv(item.TargetPageUrl),
                        EscapeCsv(item.PageLayoutFileName),
                        item.Direction == "ArToEn" ? "عربي إلى إنجليزي" : "إنجليزي إلى عربي",
                        item.HasControlLoader ? "نعم" : "لا",
                        EscapeCsv(item.UserControlPath),
                        item.HasContentEditor ? "نعم" : "لا",
                        item.HasPageContent ? "نعم" : "لا"
                    ));
                }

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "text/csv; charset=utf-8";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Normal Response.End completion
            }
            catch (Exception ex)
            {
                LogMessage("خطأ أثناء تصدير الصفحات: " + ex.Message, "danger");
            }
        }

        private string EscapeCsv(string val)
        {
            if (string.IsNullOrEmpty(val)) return string.Empty;
            return val.Replace("\"", "\"\"");
        }

        protected void btnCreateSelectedPages_Click(object sender, EventArgs e)
        {
            var allItems = CurrentMissingPages;
            if (allItems == null || allItems.Count == 0)
            {
                LogMessage("لا توجد صفحات مفقودة مسجلة للإنشاء.", "warning");
                return;
            }

            List<MissingPageItem> selectedItems = new List<MissingPageItem>();

            foreach (RepeaterItem ri in rptMissingPages.Items)
            {
                CheckBox chk = ri.FindControl("chkSelectPage") as CheckBox;
                HiddenField hf = ri.FindControl("hfItemIndex") as HiddenField;

                if (chk != null && chk.Checked && hf != null)
                {
                    int idx;
                    if (int.TryParse(hf.Value, out idx) && idx >= 0 && idx < allItems.Count)
                    {
                        selectedItems.Add(allItems[idx]);
                    }
                }
            }

            if (selectedItems.Count == 0)
            {
                LogMessage("يرجى تحديد صفحة واحدة على الأقل لإنشائها.", "warning");
                RenderExecutionResults(allItems.Count, 0, 0);
                return;
            }

            _logBuilder.Clear();
            _createdUrls.Clear();
            _successCount = 0;
            _failCount = 0;

            LogMessage(string.Format("بدء إنشاء ومزامنة عدد ({0}) صفحة...", selectedItems.Count), "info");

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    foreach (var item in selectedItems)
                    {
                        CreateSingleMissingPage(site, item);
                    }
                }
            });

            RenderExecutionResults(selectedItems.Count, _successCount, _failCount);

            if (_createdUrls.Count > 0)
            {
                pnlCreatedUrls.Visible = true;
                litCreatedUrlsCount.Text = _createdUrls.Count.ToString();
                txtCreatedPagesUrls.Text = string.Join(Environment.NewLine, _createdUrls);
            }
            else
            {
                pnlCreatedUrls.Visible = false;
                txtCreatedPagesUrls.Text = string.Empty;
            }
        }

        private void CreateSingleMissingPage(SPSite site, MissingPageItem item)
        {
            try
            {
                using (SPWeb tgtWeb = site.OpenWeb(item.TargetWebUrl, false))
                {
                    if (tgtWeb == null || !tgtWeb.Exists)
                    {
                        LogMessage(string.Format("فشل: الموقع المستهدف غير موجود: {0}", item.TargetWebUrl), "danger");
                        _failCount++;
                        return;
                    }

                    if (!PublishingWeb.IsPublishingWeb(tgtWeb))
                    {
                        LogMessage(string.Format("فشل: الموقع المستهدف ليس موقع نشر (Publishing Web): {0}", item.TargetWebUrl), "danger");
                        _failCount++;
                        return;
                    }

                    tgtWeb.AllowUnsafeUpdates = true;

                    try
                    {
                        PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(tgtWeb);
                        string pagesListName = GetPagesListName(tgtWeb);
                        string pageServerRelUrl = tgtWeb.ServerRelativeUrl.TrimEnd('/') + "/" + pagesListName + "/" + item.PageFileName;
                        SPFile existingFile = GetPageFileInWeb(tgtWeb, item.PageFileName) ?? tgtWeb.GetFile(pageServerRelUrl);

                        if (existingFile != null && existingFile.Exists)
                        {
                            if (!chkOverwrite.Checked)
                            {
                                LogMessage(string.Format("الصفحة موجودة بالفعل، تم التخطي: {0}", pageServerRelUrl), "warning");
                                _failCount++;
                                return;
                            }

                            // Overwrite: remove checkout or delete old
                            if (existingFile.CheckOutType != SPFile.SPCheckOutType.None)
                                existingFile.UndoCheckOut();
                            existingFile.Delete();
                        }

                        // 1. Resolve Page Layout
                        PageLayout layout = ResolvePageLayout(site, pubWeb, item.PageLayoutUrl);
                        if (layout == null)
                        {
                            LogMessage(string.Format("تخطيط الصفحة غير موجود ({0})، تعذر إنشاء: {1}", item.PageLayoutUrl, item.PageFileName), "danger");
                            _failCount++;
                            return;
                        }

                        // 2. Determine Title (copied directly from source)
                        string pageTitle = item.SourcePageTitle;

                        // 3. Create Publishing Page
                        PublishingPage newPage = pubWeb.GetPublishingPages().Add(item.PageFileName, layout);
                        newPage.Title = pageTitle;

                        // 4. Copy Page Content (PublishingPageContent / RichHtmlField) directly from source
                        if (chkCopyContentEditor.Checked && item.HasPageContent && !string.IsNullOrWhiteSpace(item.PublishingPageContentHtml))
                        {
                            SetPageContentHtml(newPage, item.PublishingPageContentHtml);
                        }

                        newPage.Update();

                        // 5. Add Web Parts
                        SPFile newPageFile = newPage.ListItem.File;
                        using (SPLimitedWebPartManager wpManager = newPageFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                        {
                            // A) ControlLoaderWebPart
                            if (chkCopyControlLoader.Checked && item.HasControlLoader && !string.IsNullOrEmpty(item.UserControlPath))
                            {
                                AddControlLoaderWebPart(wpManager, item.UserControlPath, item.UserControlProperties, item.ControlLoaderZoneId);
                            }

                            // B) ContentEditorWebPart (copied directly from source)
                            if (chkCopyContentEditor.Checked && item.HasContentEditor)
                            {
                                AddContentEditorWebPart(wpManager, item.ContentEditorHtml, item.ContentEditorLink, item.ContentEditorZoneId);
                            }
                        }

                        // 6. Check In, Publish, Approve
                        if (newPageFile.CheckOutType != SPFile.SPCheckOutType.None)
                        {
                            var checkinType = chkMajorCheckIn.Checked ? SPCheckinType.MajorCheckIn : SPCheckinType.MinorCheckIn;
                            newPageFile.CheckIn("تم الإنشاء والمزامنة آلياً عبر أداة الصفحات ثنائية اللغة", checkinType);
                        }

                        if (chkAutoPublish.Checked && newPageFile.Item.ParentList.EnableMinorVersions)
                        {
                            newPageFile.Publish("تم النشر التلقائي عبر أداة الصفحات ثنائية اللغة");
                        }

                        if (chkAutoApprove.Checked && newPageFile.Item.ParentList.EnableModeration)
                        {
                            newPageFile.Approve("تم الاعتماد التلقائي عبر أداة الصفحات ثنائية اللغة");
                        }

                        _successCount++;
                        string fullCreatedUrl = site.MakeFullUrl(pageServerRelUrl);
                        _createdUrls.Add(fullCreatedUrl);

                        LogMessage(string.Format("نجاح: تم إنشاء الصفحة بنجاح: <a href='{0}' target='_blank' class='fw-bold text-success text-decoration-none'>{1}</a> ({2})",
                            fullCreatedUrl, newPage.Title, pageServerRelUrl), "success");
                    }
                    finally
                    {
                        tgtWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _failCount++;
                LogMessage(string.Format("خطأ أثناء إنشاء ({0}): {1}", item.PageFileName, ex.Message), "danger");
            }
        }

        private void AddControlLoaderWebPart(SPLimitedWebPartManager wpManager, string userControlPath, string userControlProperties, string zoneId)
        {
            try
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.Load(WP_ASSEMBLY_NAME);
                Type wpType = asm.GetType(WP_TYPE_NAME);

                if (wpType == null)
                {
                    LogMessage("لم يتم العثور على فئة WebPart: " + WP_TYPE_NAME, "warning");
                    return;
                }

                System.Web.UI.WebControls.WebParts.WebPart wp =
                    (System.Web.UI.WebControls.WebParts.WebPart)Activator.CreateInstance(wpType);

                wp.Title = "Control Loader WebPart";
                wp.ChromeType = PartChromeType.None;

                var propPath = wpType.GetProperty("UserControlPath");
                if (propPath != null)
                {
                    propPath.SetValue(wp, userControlPath, null);
                }

                if (!string.IsNullOrEmpty(userControlProperties))
                {
                    var propSettings = wpType.GetProperty("UserControlProperties");
                    if (propSettings != null)
                    {
                        propSettings.SetValue(wp, userControlProperties, null);
                    }
                }

                string targetZone = string.IsNullOrEmpty(zoneId) ? DEFAULT_ZONE_ID : zoneId;
                wpManager.AddWebPart(wp, targetZone, 0);
            }
            catch (Exception ex)
            {
                LogMessage("تحذير عند إضافة ControlLoaderWebPart: " + ex.Message, "warning");
            }
        }

        private void AddContentEditorWebPart(SPLimitedWebPartManager wpManager, string htmlContent, string contentLink, string zoneId)
        {
            try
            {
                ContentEditorWebPart cewp = new ContentEditorWebPart
                {
                    Title = "Content Editor",
                    ChromeType = PartChromeType.None
                };

                if (!string.IsNullOrEmpty(contentLink))
                {
                    cewp.ContentLink = contentLink;
                }

                if (!string.IsNullOrEmpty(htmlContent))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlElement elem = xmlDoc.CreateElement("div");
                    elem.InnerXml = htmlContent;
                    cewp.Content = elem;
                }

                string targetZone = string.IsNullOrEmpty(zoneId) ? DEFAULT_ZONE_ID : zoneId;
                wpManager.AddWebPart(cewp, targetZone, 1);
            }
            catch
            {
                // Fallback: simple text element if inner xml is not strictly valid XML
                try
                {
                    ContentEditorWebPart cewp = new ContentEditorWebPart
                    {
                        Title = "Content Editor",
                        ChromeType = PartChromeType.None
                    };

                    XmlDocument xmlDoc = new XmlDocument();
                    XmlElement elem = xmlDoc.CreateElement("div");
                    elem.InnerText = htmlContent;
                    cewp.Content = elem;

                    string targetZone = string.IsNullOrEmpty(zoneId) ? DEFAULT_ZONE_ID : zoneId;
                    wpManager.AddWebPart(cewp, targetZone, 1);
                }
                catch (Exception ex)
                {
                    LogMessage("تحذير عند إضافة ContentEditorWebPart: " + ex.Message, "warning");
                }
            }
        }

        #endregion

        #region Helpers & Layout Resolvers

        private PageLayout ResolvePageLayout(SPSite site, PublishingWeb pubWeb, string layoutUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(layoutUrl))
                    layoutUrl = "/_catalogs/masterpage/NewSideMenu.aspx";

                SPFile layoutFile = site.RootWeb.GetFile(layoutUrl);
                if (layoutFile != null && layoutFile.Exists)
                {
                    foreach (PageLayout pl in pubWeb.GetAvailablePageLayouts())
                    {
                        if (pl.ServerRelativeUrl.Equals(layoutFile.ServerRelativeUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            return pl;
                        }
                    }
                    return new PageLayout(layoutFile.Item);
                }
            }
            catch { }

            // Fallback by layout filename
            try
            {
                string fileName = Path.GetFileName(layoutUrl);
                foreach (PageLayout pl in pubWeb.GetAvailablePageLayouts())
                {
                    if (pl.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return pl;
                    }
                }
            }
            catch { }

            return null;
        }

        private Dictionary<string, SPFile> GetPagesDict(SPWeb web)
        {
            var dict = new Dictionary<string, SPFile>(StringComparer.OrdinalIgnoreCase);
            if (web == null || !web.Exists) return dict;

            try
            {
                // Strategy 1: Using PublishingWeb.GetPublishingPages()
                if (PublishingWeb.IsPublishingWeb(web))
                {
                    try
                    {
                        PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                        if (pubWeb != null)
                        {
                            try
                            {
                                PublishingPageCollection pages = pubWeb.GetPublishingPages();
                                if (pages != null)
                                {
                                    foreach (PublishingPage p in pages)
                                    {
                                        if (p.ListItem != null && p.ListItem.File != null && p.ListItem.File.Exists && p.Name.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                                        {
                                            dict[p.Name] = p.ListItem.File;
                                        }
                                    }
                                }
                            }
                            catch { }

                            // Also try pubWeb.PagesList directly
                            try
                            {
                                SPList pList = pubWeb.PagesList;
                                if (pList != null)
                                {
                                    PopulateDictFromList(dict, pList);
                                }
                            }
                            catch { }

                            // Try by pubWeb.PagesListName
                            if (!string.IsNullOrEmpty(pubWeb.PagesListName))
                            {
                                try
                                {
                                    SPList l = web.Lists.TryGetList(pubWeb.PagesListName);
                                    if (l != null) PopulateDictFromList(dict, l);
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }

                // Strategy 2: Check by list names ("Pages", "الصفحات", "صفحات")
                if (dict.Count == 0)
                {
                    string[] candidateNames = new[] { "Pages", "الصفحات", "صفحات", "SitePages", "مستندات" };
                    foreach (var name in candidateNames)
                    {
                        try
                        {
                            SPList pagesList = web.Lists.TryGetList(name);
                            if (pagesList != null)
                            {
                                PopulateDictFromList(dict, pagesList);
                                if (dict.Count > 0) break;
                            }
                        }
                        catch { }
                    }
                }

                // Strategy 3: Check web.Lists for document library with URL ending in /Pages or /الصفحات
                if (dict.Count == 0)
                {
                    foreach (SPList list in web.Lists)
                    {
                        if (list.BaseType == SPBaseType.DocumentLibrary)
                        {
                            string rootUrl = list.RootFolder != null ? list.RootFolder.Url : "";
                            if (rootUrl.EndsWith("Pages", StringComparison.OrdinalIgnoreCase) ||
                                rootUrl.EndsWith("الصفحات", StringComparison.OrdinalIgnoreCase))
                            {
                                PopulateDictFromList(dict, list);
                                if (dict.Count > 0) break;
                            }
                        }
                    }
                }

                // Strategy 4: Direct SPFolder check
                if (dict.Count == 0)
                {
                    string[] folderCandidates = new[] {
                        "Pages", "الصفحات",
                        web.ServerRelativeUrl.TrimEnd('/') + "/Pages",
                        web.ServerRelativeUrl.TrimEnd('/') + "/الصفحات"
                    };

                    foreach (var fc in folderCandidates)
                    {
                        try
                        {
                            SPFolder folder = web.GetFolder(fc);
                            if (folder != null && folder.Exists)
                            {
                                foreach (SPFile f in folder.Files)
                                {
                                    if (f.Exists && f.Name.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                                    {
                                        dict[f.Name] = f;
                                    }
                                }
                                if (dict.Count > 0) break;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("تنبيه عند فحص صفحات ({0}): {1}", web.ServerRelativeUrl, ex.Message), "warning");
            }

            return dict;
        }

        private void PopulateDictFromList(Dictionary<string, SPFile> dict, SPList list)
        {
            if (list == null) return;
            try
            {
                SPQuery query = new SPQuery
                {
                    ViewAttributes = "Scope='RecursiveAll'",
                    RowLimit = 2000
                };
                SPListItemCollection items = list.GetItems(query);
                foreach (SPListItem item in items)
                {
                    if (item.File != null && item.File.Exists && item.File.Name.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        dict[item.File.Name] = item.File;
                    }
                }
            }
            catch
            {
                try
                {
                    foreach (SPListItem item in list.Items)
                    {
                        if (item.File != null && item.File.Exists && item.File.Name.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                        {
                            dict[item.File.Name] = item.File;
                        }
                    }
                }
                catch { }
            }
        }

        private string GetPagesListName(SPWeb web)
        {
            try
            {
                if (PublishingWeb.IsPublishingWeb(web))
                {
                    PublishingWeb pw = PublishingWeb.GetPublishingWeb(web);
                    if (pw != null && !string.IsNullOrEmpty(pw.PagesListName))
                        return pw.PagesListName;
                    if (pw != null && pw.PagesList != null && pw.PagesList.RootFolder != null)
                        return pw.PagesList.RootFolder.Name;
                }
            }
            catch { }

            return web.Language == 1025 ? "الصفحات" : "Pages";
        }

        private SPFile GetPageFileInWeb(SPWeb web, string fileName)
        {
            try
            {
                string pName = GetPagesListName(web);
                string relUrl = web.ServerRelativeUrl.TrimEnd('/') + "/" + pName + "/" + fileName.TrimStart('/');
                SPFile f = web.GetFile(relUrl);
                if (f != null && f.Exists) return f;

                string altName = pName.Equals("Pages", StringComparison.OrdinalIgnoreCase) ? "الصفحات" : "Pages";
                string altUrl = web.ServerRelativeUrl.TrimEnd('/') + "/" + altName + "/" + fileName.TrimStart('/');
                SPFile altFile = web.GetFile(altUrl);
                if (altFile != null && altFile.Exists) return altFile;

                // Check in dictionary
                var dict = GetPagesDict(web);
                if (dict.TryGetValue(fileName, out SPFile dictFile))
                {
                    return dictFile;
                }
            }
            catch { }
            return null;
        }

        private string GetItemFieldValue(SPListItem item, string fieldName)
        {
            try
            {
                if (item == null || !item.Fields.ContainsField(fieldName)) return string.Empty;
                return Convert.ToString(item[fieldName]);
            }
            catch { return string.Empty; }
        }

        private string ExtractUrlFromLayoutField(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue)) return string.Empty;
            int commaIdx = fieldValue.IndexOf(',');
            if (commaIdx >= 0) return fieldValue.Substring(0, commaIdx).Trim();
            return fieldValue.Trim();
        }

        private string GetPageContentHtml(PublishingPage pubPage)
        {
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
                return Convert.ToString(pubPage.ListItem["PublishingPageContent"]);
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
                return Convert.ToString(pubPage.ListItem["PublishingPageContentHtml"]);
            if (pubPage.ListItem.Fields.ContainsField("PageContent"))
                return Convert.ToString(pubPage.ListItem["PageContent"]);
            return string.Empty;
        }

        private void SetPageContentHtml(PublishingPage pubPage, string content)
        {
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
                pubPage.ListItem["PublishingPageContent"] = content;
            else if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
                pubPage.ListItem["PublishingPageContentHtml"] = content;
            else if (pubPage.ListItem.Fields.ContainsField("PageContent"))
                pubPage.ListItem["PageContent"] = content;
        }

        private string NormalizeWebUrl(string rawUrl)
        {
            if (string.IsNullOrEmpty(rawUrl)) return string.Empty;
            string url = rawUrl.Trim();
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                try { url = new Uri(url).AbsolutePath; } catch { return string.Empty; }
            }
            if (!url.StartsWith("/")) url = "/" + url;
            url = url.TrimEnd('/');

            int pagesIdx = url.IndexOf("/Pages", StringComparison.OrdinalIgnoreCase);
            if (pagesIdx > 0) url = url.Substring(0, pagesIdx);

            int arPagesIdx = url.IndexOf("/الصفحات", StringComparison.OrdinalIgnoreCase);
            if (arPagesIdx > 0) url = url.Substring(0, arPagesIdx);

            return url;
        }

        private string NormalizePageUrl(string rawUrl)
        {
            string url = rawUrl.Trim();

            // Unpack requestUrl from 404 handler if present
            int reqIdx = url.IndexOf("requestUrl=", StringComparison.OrdinalIgnoreCase);
            if (reqIdx >= 0)
            {
                url = url.Substring(reqIdx + "requestUrl=".Length);
                int ampIdx = url.IndexOf('&');
                if (ampIdx >= 0) url = url.Substring(0, ampIdx);
                url = System.Web.HttpUtility.UrlDecode(url);
            }

            // Strip queries and hashes
            int qIdx = url.IndexOf('?');
            if (qIdx >= 0) url = url.Substring(0, qIdx);

            int hIdx = url.IndexOf('#');
            if (hIdx >= 0) url = url.Substring(0, hIdx);

            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                try { url = new Uri(url).AbsolutePath; } catch { }
            }
            if (!url.StartsWith("/")) url = "/" + url;
            return url;
        }

        private string ExtractWebPath(string pageUrl)
        {
            string webPath = pageUrl;
            int pagesIdx = webPath.IndexOf("/Pages/", StringComparison.OrdinalIgnoreCase);
            if (pagesIdx >= 0)
            {
                webPath = webPath.Substring(0, pagesIdx);
            }
            else
            {
                int arPagesIdx = webPath.IndexOf("/الصفحات/", StringComparison.OrdinalIgnoreCase);
                if (arPagesIdx >= 0)
                {
                    webPath = webPath.Substring(0, arPagesIdx);
                }
                else
                {
                    int lastSlash = webPath.LastIndexOf('/');
                    if (lastSlash > 0 && webPath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        webPath = webPath.Substring(0, lastSlash);
                    }
                }
            }
            return string.IsNullOrEmpty(webPath) ? "/" : webPath;
        }

        private string GetPairedWebUrl(string serverRelUrl)
        {
            if (string.IsNullOrEmpty(serverRelUrl)) return string.Empty;

            string url = serverRelUrl.EndsWith("/") ? serverRelUrl : serverRelUrl + "/";

            if (url.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase))
                return ("/en/" + url.Substring(4)).TrimEnd('/');
            if (url.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                return ("/ar/" + url.Substring(4)).TrimEnd('/');

            return string.Empty;
        }

        private SPFile ResolvePageFile(SPSite site, SPWeb web, string pageUrl)
        {
            SPFile file = null;
            try { file = web.GetFile(pageUrl); } catch { }

            if (file == null || !file.Exists)
            {
                try { file = site.RootWeb.GetFile(pageUrl); } catch { }
            }

            if (file == null || !file.Exists)
            {
                string fileName = Path.GetFileName(pageUrl);
                if (!string.IsNullOrEmpty(fileName))
                {
                    file = GetPageFileInWeb(web, fileName);
                }
            }

            return file;
        }

        private void ResetUi()
        {
            _logBuilder.Clear();
            _createdUrls.Clear();
            _successCount = 0;
            _failCount = 0;
            pnlResults.Visible = false;
            pnlCreatedUrls.Visible = false;
            pnlMissingPages.Visible = false;
            pnlExistingPages.Visible = false;
            txtCreatedPagesUrls.Text = string.Empty;
        }

        private void LogMessage(string message, string levelCss)
        {
            string icon = "info-circle";
            if (levelCss == "success") icon = "checkmark-circle-02";
            else if (levelCss == "danger") icon = "cancel-circle";
            else if (levelCss == "warning") icon = "alert-02";

            _logBuilder.AppendFormat("<div class='py-1 border-bottom d-flex align-items-center gap-2 text-{0}'><i class='hgi hgi-stroke hgi-{1}'></i><span>{2}</span></div>",
                levelCss, icon, message);
        }

        private void RenderExecutionResults(int total, int success, int failed)
        {
            pnlResults.Visible = true;

            string summaryHtml = string.Format(@"
                <div class='row g-3 mb-3 text-center'>
                    <div class='col-4'>
                        <div class='p-3 bg-primary-25 rounded-3 border'>
                            <span class='text-muted small d-block mb-1'>إجمالي الصفحات المكتشفة</span>
                            <strong class='h4 text-primary mb-0 font-monospace'>{0}</strong>
                        </div>
                    </div>
                    <div class='col-4'>
                        <div class='p-3 bg-success-subtle rounded-3 border border-success-subtle'>
                            <span class='text-success-emphasis small d-block mb-1'>تم إنشاؤها بنجاح</span>
                            <strong class='h4 text-success mb-0 font-monospace'>{1}</strong>
                        </div>
                    </div>
                    <div class='col-4'>
                        <div class='p-3 bg-danger-subtle rounded-3 border border-danger-subtle'>
                            <span class='text-danger-emphasis small d-block mb-1'>الفاشلة / المتخطاة</span>
                            <strong class='h4 text-danger mb-0 font-monospace'>{2}</strong>
                        </div>
                    </div>
                </div>",
                total, success, failed);

            litSummaryCard.Text = summaryHtml;
            litDetailedLog.Text = _logBuilder.Length > 0 ? _logBuilder.ToString() : "<div class='text-muted'>لا توجد رسائل مسجلة.</div>";
        }

        #endregion
    }
}
