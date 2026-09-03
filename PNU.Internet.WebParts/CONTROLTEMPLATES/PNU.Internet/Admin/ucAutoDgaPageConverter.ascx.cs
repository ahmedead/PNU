using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucAutoDgaPageConverter : UserControl
    {
        private int _successCount = 0;
        private int _failCount = 0;
        private StringBuilder _logBuilder = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initial state
            }
        }

        #region Mode 1: Bulk URLs Auto-Converter

        protected void btnConvertBulkUrls_Click(object sender, EventArgs e)
        {
            ResetLogs();

            string bulkText = txtBulkUrls.Text;
            if (string.IsNullOrWhiteSpace(bulkText))
            {
                LogMessage("يرجى إدخال رابط صفحة واحد على الأقل في الحقل المخصص.", "warning");
                RenderLogs();
                return;
            }

            string[] lines = bulkText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var distinctUrls = new List<string>();
            foreach (string l in lines)
            {
                string u = l.Trim();
                if (!string.IsNullOrEmpty(u) && !u.StartsWith("#") && !distinctUrls.Contains(u))
                {
                    distinctUrls.Add(u);
                }
            }

            if (distinctUrls.Count == 0)
            {
                LogMessage("لم يتم العثور على روابط صالحة للمعالجة.", "warning");
                RenderLogs();
                return;
            }

            LogMessage(string.Format("بدء المعالجة والتحويل التلقائي لعدد ({0}) صفحة...", distinctUrls.Count), "info");

            foreach (string url in distinctUrls)
            {
                AutoConvertSinglePage(url);
            }

            RenderLogs();
        }

        #endregion

        #region Mode 2: Single URL Preview & Converter

        protected void btnInspectSingleUrl_Click(object sender, EventArgs e)
        {
            pnlPreview.Visible = false;
            string rawUrl = txtSingleUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                LogMessage("يرجى إدخال رابط الصفحة المراد فحصها.", "warning");
                RenderLogs();
                return;
            }

            try
            {
                string pageUrl = NormalizePageUrl(rawUrl);
                string webPath = ExtractWebPath(pageUrl);

                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb(webPath, false))
                    {
                        if (web == null || !web.Exists)
                        {
                            LogMessage(string.Format("الموقع الفرعي غير موجود للمسار ({0}) للرابط: {1}", webPath, rawUrl), "danger");
                            RenderLogs();
                            return;
                        }

                        SPFile file = ResolvePageFile(site, web, pageUrl);
                        if (file == null || !file.Exists)
                        {
                            LogMessage(string.Format("ملف الصفحة غير موجود في المسار: {0}", pageUrl), "danger");
                            RenderLogs();
                            return;
                        }

                        PublishingPage pubPage = null;
                        try
                        {
                            pubPage = PublishingPage.GetPublishingPage(file.Item);
                        }
                        catch { }

                        if (pubPage == null)
                        {
                            LogMessage(string.Format("الصفحة ليست صفحة نشر (Publishing Page): {0}", pageUrl), "warning");
                            RenderLogs();
                            return;
                        }

                        string title = pubPage.Title ?? file.Name;
                        string originalContent = GetPageContent(pubPage, file);
                        string dgaTransformed = DgaAutoTransformer.TransformContent(pageUrl, title, originalContent);

                        litOriginalContent.Text = originalContent;
                        litDgaPreview.Text = dgaTransformed;
                        pnlPreview.Visible = true;

                        LogMessage(string.Format("تم فحص الصفحة بنجاح: <strong>{0}</strong> ({1})", title, pageUrl), "info");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("خطأ أثناء فحص الصفحة: {0}", ex.Message), "danger");
            }

            RenderLogs();
        }

        protected void btnApplySingleUrl_Click(object sender, EventArgs e)
        {
            ResetLogs();
            string rawUrl = txtSingleUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                LogMessage("يرجى إدخال رابط الصفحة.", "warning");
                RenderLogs();
                return;
            }

            AutoConvertSinglePage(rawUrl);
            RenderLogs();
        }

        #endregion

        #region Mode 3: Subsite Scanner

        protected void btnScanSubsite_Click(object sender, EventArgs e)
        {
            pnlSubsiteResults.Visible = false;
            cblSubsitePages.Items.Clear();

            string rawUrl = txtSubsiteUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                LogMessage("يرجى إدخال مسار الموقع الفرعي (مثال: /en/Faculties/AD)", "warning");
                RenderLogs();
                return;
            }

            string subsiteUrl = rawUrl;
            if (subsiteUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                try { subsiteUrl = new Uri(subsiteUrl).AbsolutePath; } catch { }
            }

            try
            {
                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb(subsiteUrl, false))
                    {
                        if (web == null || !web.Exists)
                        {
                            LogMessage(string.Format("الموقع الفرعي غير موجود للمسار: {0}", subsiteUrl), "danger");
                            RenderLogs();
                            return;
                        }

                        if (!PublishingWeb.IsPublishingWeb(web))
                        {
                            LogMessage(string.Format("الموقع الفرعي ({0}) ليس Publishing Web.", subsiteUrl), "warning");
                            RenderLogs();
                            return;
                        }

                        PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                        PublishingPageCollection pages = pubWeb.GetPublishingPages();

                        int count = 0;
                        foreach (PublishingPage page in pages)
                        {
                            string text = string.Format("{0} ({1})", page.Title ?? page.Name, page.Url);
                            ListItem item = new ListItem(text, page.Uri.AbsolutePath);
                            item.Selected = true;
                            cblSubsitePages.Items.Add(item);
                            count++;
                        }

                        pnlSubsiteResults.Visible = true;
                        LogMessage(string.Format("تم العثور على ({0}) صفحة في الموقع الفرعي ({1}).", count, subsiteUrl), "info");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("خطأ أثناء مسح صفحات الموقع الفرعي: {0}", ex.Message), "danger");
            }

            RenderLogs();
        }

        protected void btnConvertScannedPages_Click(object sender, EventArgs e)
        {
            ResetLogs();
            var selectedUrls = new List<string>();
            foreach (ListItem item in cblSubsitePages.Items)
            {
                if (item.Selected)
                {
                    selectedUrls.Add(item.Value);
                }
            }

            if (selectedUrls.Count == 0)
            {
                LogMessage("لم يتم اختيار أي صفحات من القائمة.", "warning");
                RenderLogs();
                return;
            }

            LogMessage(string.Format("بدء تحويل ({0}) صفحة محددة من الموقع الفرعي...", selectedUrls.Count), "info");
            foreach (string u in selectedUrls)
            {
                AutoConvertSinglePage(u);
            }
            RenderLogs();
        }

        #endregion

        #region Core Auto-Conversion Processing

        private void AutoConvertSinglePage(string rawUrl)
        {
            try
            {
                string pageUrl = NormalizePageUrl(rawUrl);
                string webPath = ExtractWebPath(pageUrl);

                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                    ? SPContext.Current.Site.Url
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb(webPath, false))
                    {
                        if (web == null || !web.Exists)
                        {
                            LogMessage(string.Format("الموقع الفرعي غير موجود للمسار ({0}) للرابط: {1}", webPath, rawUrl), "danger");
                            _failCount++;
                            return;
                        }

                        web.AllowUnsafeUpdates = true;

                        try
                        {
                            SPFile file = ResolvePageFile(site, web, pageUrl);
                            if (file == null || !file.Exists)
                            {
                                LogMessage(string.Format("ملف الصفحة غير موجود: {0}", pageUrl), "danger");
                                _failCount++;
                                return;
                            }

                            if (!PublishingWeb.IsPublishingWeb(web))
                            {
                                LogMessage(string.Format("الموقع ({0}) ليس Publishing Web: {1}", web.ServerRelativeUrl, pageUrl), "warning");
                                _failCount++;
                                return;
                            }

                            PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                            PublishingPage pubPage = null;
                            try
                            {
                                pubPage = PublishingPage.GetPublishingPage(file.Item);
                            }
                            catch { }

                            if (pubPage == null)
                            {
                                LogMessage(string.Format("الصفحة ليست صفحة نشر (Publishing Page): {0}", pageUrl), "warning");
                                _failCount++;
                                return;
                            }

                            // 1. Check Out
                            if (file.CheckOutType == SPFile.SPCheckOutType.None)
                            {
                                file.CheckOut();
                            }

                            // 2. Read Current Title & Content
                            string title = pubPage.Title ?? file.Name;
                            string originalRawContent = GetPageContent(pubPage, file);

                            // 3. Auto-Transform Content to DGA Standards
                            string dgaContent = DgaAutoTransformer.TransformContent(pageUrl, title, originalRawContent);

                            // 4. Change Page Layout if requested
                            if (chkChangeLayout.Checked)
                            {
                                string targetLayoutUrl = string.IsNullOrEmpty(txtTargetLayout.Text.Trim())
                                    ? DgaAutoTransformer.DefaultLayout
                                    : txtTargetLayout.Text.Trim();

                                PageLayout targetLayout = ResolvePageLayout(site, pubWeb, targetLayoutUrl);
                                if (targetLayout != null)
                                {
                                    pubPage.Layout = targetLayout;
                                }
                                else
                                {
                                    LogMessage(string.Format("تنبيه: تعذر العثور على التخطيط ({0})، تم الإبقاء على التخطيط الحالي.", targetLayoutUrl), "warning");
                                }
                            }

                            // 5. Update Content
                            SetPageContent(pubPage, dgaContent, !DgaAutoTransformer.IsArabicPage(pageUrl, dgaContent));
                            pubPage.Update();

                            // 6. Check In
                            if (file.CheckOutType != SPFile.SPCheckOutType.None)
                            {
                                var checkinType = chkMajorCheckIn.Checked
                                    ? SPCheckinType.MajorCheckIn
                                    : SPCheckinType.MinorCheckIn;
                                file.CheckIn("تم تحويل وتحديث محتوى وتخطيط الصفحة آلياً إلى هوية DGA", checkinType);
                            }

                            // 7. Publish
                            if (chkAutoPublish.Checked && file.Item.ParentList.EnableMinorVersions)
                            {
                                file.Publish("تم النشر التلقائي عبر محول DGA");
                            }

                            // 8. Approve
                            if (chkAutoApprove.Checked && file.Item.ParentList.EnableModeration)
                            {
                                file.Approve("تم الاعتماد التلقائي عبر محول DGA");
                            }

                            _successCount++;
                            LogMessage(string.Format("نجح: تم تحويل ونشر الصفحة بنجاح: <a href='{0}' target='_blank' class='fw-bold text-decoration-none text-success'>{1}</a> ({2})",
                                pageUrl, pubPage.Title ?? file.Name, pageUrl), "success");
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _failCount++;
                LogMessage(string.Format("خطأ أثناء تحويل ({0}): {1}", rawUrl, ex.Message), "danger");
            }
        }

        #endregion

        #region Helpers

        private string NormalizePageUrl(string rawUrl)
        {
            string url = rawUrl.Trim();
            if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                try { url = new Uri(url).AbsolutePath; } catch { }
            }
            if (!url.StartsWith("/"))
            {
                url = "/" + url;
            }
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
                int lastSlash = webPath.LastIndexOf('/');
                if (lastSlash > 0 && webPath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                {
                    webPath = webPath.Substring(0, lastSlash);
                }
            }
            return string.IsNullOrEmpty(webPath) ? "/" : webPath;
        }

        private SPFile ResolvePageFile(SPSite site, SPWeb web, string pageUrl)
        {
            SPFile file = null;
            try
            {
                file = web.GetFile(pageUrl);
            }
            catch { }

            if (file == null || !file.Exists)
            {
                try
                {
                    file = site.RootWeb.GetFile(pageUrl);
                }
                catch { }
            }
            return file;
        }

        private PageLayout ResolvePageLayout(SPSite site, PublishingWeb pubWeb, string layoutUrl)
        {
            try
            {
                SPFile layoutFile = site.RootWeb.GetFile(layoutUrl);
                if (layoutFile.Exists)
                {
                    foreach (PageLayout pl in pubWeb.GetAvailablePageLayouts())
                    {
                        if (pl.ServerRelativeUrl.Equals(layoutFile.ServerRelativeUrl, StringComparison.OrdinalIgnoreCase) ||
                            pl.Name.Equals(layoutFile.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            return pl;
                        }
                    }
                    return new PageLayout(layoutFile.Item);
                }
            }
            catch { }
            return null;
        }

        private string GetPageContent(PublishingPage pubPage, SPFile file = null)
        {
            string content = "";
            string[] fieldCandidates = new[]
            {
                "PublishingPageContent",
                "PublishingPageContent_EN",
                "PageContent",
                "PublishingPageContentHtml",
                "Content",
                "Content_EN",
                "Description",
                "Description_EN"
            };

            foreach (var fieldName in fieldCandidates)
            {
                if (pubPage.ListItem.Fields.ContainsField(fieldName))
                {
                    string val = Convert.ToString(pubPage.ListItem[fieldName]);
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        content = val;
                        break;
                    }
                }
            }

            // Fallback: check ContentEditorWebPart in the page
            if (string.IsNullOrWhiteSpace(content) && file != null)
            {
                try
                {
                    using (SPLimitedWebPartManager wpManager = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
                    {
                        foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpManager.WebParts)
                        {
                            if (wp is ContentEditorWebPart cewp && cewp.Content != null)
                            {
                                string cewpHtml = cewp.Content.InnerXml;
                                if (!string.IsNullOrWhiteSpace(cewpHtml))
                                {
                                    content = cewpHtml;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch { }
            }

            return content ?? "";
        }

        private void SetPageContent(PublishingPage pubPage, string content, bool isEnglish = false)
        {
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
            {
                pubPage.ListItem["PublishingPageContent"] = content;
            }
            if (pubPage.ListItem.Fields.ContainsField("PageContent"))
            {
                pubPage.ListItem["PageContent"] = content;
            }
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
            {
                pubPage.ListItem["PublishingPageContentHtml"] = content;
            }
            if (isEnglish && pubPage.ListItem.Fields.ContainsField("PublishingPageContent_EN"))
            {
                pubPage.ListItem["PublishingPageContent_EN"] = content;
            }
        }

        private void ResetLogs()
        {
            _successCount = 0;
            _failCount = 0;
            _logBuilder.Clear();
            pnlResults.Visible = false;
        }

        private void LogMessage(string message, string type)
        {
            string badgeClass = "bg-secondary";
            string iconClass = "hgi-information-circle";
            if (type == "success") { badgeClass = "bg-success"; iconClass = "hgi-checkmark-circle-02"; }
            else if (type == "danger") { badgeClass = "bg-danger"; iconClass = "hgi-alert-circle"; }
            else if (type == "warning") { badgeClass = "bg-warning text-dark"; iconClass = "hgi-alert-02"; }
            else if (type == "info") { badgeClass = "bg-info text-dark"; iconClass = "hgi-information-circle"; }

            _logBuilder.AppendFormat(@"
                <div class='d-flex align-items-center justify-content-between p-2 border-bottom text-start' dir='ltr'>
                    <div class='d-flex align-items-center gap-2'>
                        <span class='badge {0}'><i class='hgi hgi-stroke {1}'></i></span>
                        <span>{2}</span>
                    </div>
                    <span class='text-muted small'>{3:HH:mm:ss}</span>
                </div>", badgeClass, iconClass, message, DateTime.Now);
        }

        private void RenderLogs()
        {
            lblSuccessCount.Text = string.Format("الناجحة: {0}", _successCount);
            lblFailCount.Text = string.Format("الفاشلة: {0}", _failCount);
            litLogs.Text = _logBuilder.ToString();
            pnlResults.Visible = true;
        }

        #endregion
    }
}
