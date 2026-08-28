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
    public partial class ucUpdatePageLayoutAndContent : UserControl
    {
        private readonly StringBuilder _logBuilder = new StringBuilder();
        private int _successCount = 0;
        private int _failCount = 0;
        private int _totalCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPredefinedList();
            }
        }

        private void BindPredefinedList()
        {
            var pages = ProgramPagesDgaData.GetPredefinedPgdPrograms();
            cblPredefinedPages.Items.Clear();

            for (int i = 0; i < pages.Count; i++)
            {
                var p = pages[i];
                string langBadge = p.Language == "ar" ? "[العربية]" : "[English]";
                string itemText = string.Format("{0} {1} — ({2})", langBadge, p.Title, p.Url);
                var listItem = new ListItem(itemText, i.ToString());
                listItem.Selected = true; // Selected by default
                cblPredefinedPages.Items.Add(listItem);
            }
        }

        #region Button Event Handlers

        protected void btnExecutePredefined_Click(object sender, EventArgs e)
        {
            var allPredefined = ProgramPagesDgaData.GetPredefinedPgdPrograms();
            var selectedItems = new List<PageMigrationItem>();

            foreach (ListItem item in cblPredefinedPages.Items)
            {
                if (item.Selected)
                {
                    int index;
                    if (int.TryParse(item.Value, out index) && index >= 0 && index < allPredefined.Count)
                    {
                        selectedItems.Add(allPredefined[index]);
                    }
                }
            }

            if (selectedItems.Count == 0)
            {
                ShowSummary(0, 0, 0, "لم يتم تحديد أي برامج للمعالجة.");
                return;
            }

            ProcessMigrationItems(selectedItems);
        }

        protected void btnExecuteBulk_Click(object sender, EventArgs e)
        {
            var items = new List<PageMigrationItem>();

            // 1. Check if server file path is provided
            string filePath = txtServerFilePath.Text.Trim();
            string bulkText = txtBulkData.Text.Trim();

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try
                {
                    string fileContent = File.ReadAllText(filePath, Encoding.UTF8);
                    items.AddRange(ParseBulkText(fileContent));
                }
                catch (Exception ex)
                {
                    LogMessage(string.Format("فشل قراءة الملف ({0}): {1}", filePath, ex.Message), "danger");
                }
            }

            // 2. Parse direct textarea input if provided
            if (!string.IsNullOrEmpty(bulkText))
            {
                items.AddRange(ParseBulkText(bulkText));
            }

            if (items.Count == 0)
            {
                ShowSummary(0, 0, 0, "لم يتم العثور على صفحات صالحة للمعالجة. يرجى التحقق من تنسيق الإدخال.");
                return;
            }

            ProcessMigrationItems(items);
        }

        protected void btnExecuteSingle_Click(object sender, EventArgs e)
        {
            string url = txtSingleUrl.Text.Trim();
            string layout = txtSingleLayout.Text.Trim();
            string title = txtSingleTitle.Text.Trim();
            string content = txtSingleContent.Text;

            if (string.IsNullOrEmpty(url))
            {
                ShowSummary(0, 0, 0, "يرجى إدخال رابط الصفحة المطلوب تحديثها.");
                return;
            }

            if (string.IsNullOrEmpty(layout))
            {
                layout = ProgramPagesDgaData.DefaultLayout;
            }

            var item = new PageMigrationItem
            {
                Url = url,
                Title = title,
                PageLayoutUrl = layout,
                ContentHtml = content
            };

            ProcessMigrationItems(new List<PageMigrationItem> { item });
        }

        #endregion

        #region Core Migration & SharePoint Logic

        private void ProcessMigrationItems(List<PageMigrationItem> items)
        {
            _logBuilder.Length = 0;
            _successCount = 0;
            _failCount = 0;
            _totalCount = items.Count;

            DateTime startTime = DateTime.Now;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                foreach (var item in items)
                {
                    ProcessSinglePage(item);
                }
            });

            TimeSpan elapsed = DateTime.Now - startTime;
            string extra = string.Format("استغرقت المعالجة: {0:N1} ثانية", elapsed.TotalSeconds);
            ShowSummary(_totalCount, _successCount, _failCount, extra);
        }

        private void ProcessSinglePage(PageMigrationItem item)
        {
            if (string.IsNullOrEmpty(item.Url))
            {
                LogMessage("رابط الصفحة فارغ، تم التخطي.", "warning");
                _failCount++;
                return;
            }

            try
            {
                string rawUrl = item.Url.Trim();
                string pageUrl = rawUrl;

                // Handle absolute vs relative URL
                if (pageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Uri uri = new Uri(pageUrl);
                        pageUrl = uri.AbsolutePath;
                    }
                    catch
                    {
                        // Keep raw if URI parse fails
                    }
                }

                if (!pageUrl.StartsWith("/"))
                {
                    pageUrl = "/" + pageUrl;
                }

                // Extract web URL by removing /Pages/... or the .aspx file part
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

                if (string.IsNullOrEmpty(webPath))
                {
                    webPath = "/";
                }

                // Resolve Site URL from current context or full URL
                string siteUrl = SPContext.Current != null && SPContext.Current.Site != null 
                    ? SPContext.Current.Site.Url 
                    : "https://pnu.edu.sa";

                using (SPSite site = new SPSite(siteUrl))
                {
                    // Open target web using calculated web path
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
                            // Get page file
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

                            if (file == null || !file.Exists)
                            {
                                LogMessage(string.Format("الصفحة غير موجودة: {0} (الموقع: {1})", pageUrl, web.ServerRelativeUrl), "danger");
                                _failCount++;
                                return;
                            }

                            // 1. Check out if required
                            if (chkAutoCheckOut.Checked && file.CheckOutType == SPFile.SPCheckOutType.None)
                            {
                                file.CheckOut();
                            }

                            // 2. Resolve Publishing Web & Page
                            PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                            PublishingPage pubPage = null;

                            try
                            {
                                pubPage = PublishingPage.GetPublishingPage(file.Item);
                            }
                            catch
                            {
                                // Fallback if direct getItem fails
                            }

                            if (pubPage == null)
                            {
                                LogMessage(string.Format("الصفحة ليست صفحة نشر (Publishing Page): {0}", pageUrl), "warning");
                                _failCount++;
                                return;
                            }

                            // 3. Update Page Layout if specified
                            string targetLayoutUrl = string.IsNullOrEmpty(item.PageLayoutUrl) 
                                ? ProgramPagesDgaData.DefaultLayout 
                                : item.PageLayoutUrl;

                            PageLayout targetLayout = ResolvePageLayout(site, pubWeb, targetLayoutUrl);
                            if (targetLayout != null)
                            {
                                pubPage.Layout = targetLayout;
                            }
                            else
                            {
                                LogMessage(string.Format("تعذر العثور على تخطيط الصفحة ({0})، تم الإبقاء على التخطيط الحالي.", targetLayoutUrl), "warning");
                            }

                            // 4. Update Page Title if provided
                            if (!string.IsNullOrEmpty(item.Title))
                            {
                                pubPage.Title = item.Title;
                            }

                            // 5. Update Page Content HTML
                            if (!string.IsNullOrEmpty(item.ContentHtml))
                            {
                                if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
                                {
                                    pubPage.ListItem["PublishingPageContent"] = item.ContentHtml;
                                }
                                else if (pubPage.ListItem.Fields.ContainsField("PageContent"))
                                {
                                    pubPage.ListItem["PageContent"] = item.ContentHtml;
                                }
                                else if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
                                {
                                    pubPage.ListItem["PublishingPageContentHtml"] = item.ContentHtml;
                                }
                            }

                            pubPage.Update();

                            // 6. Check In
                            if (file.CheckOutType != SPFile.SPCheckOutType.None)
                            {
                                var checkinType = chkMajorCheckIn.Checked 
                                    ? SPCheckinType.MajorCheckIn 
                                    : SPCheckinType.MinorCheckIn;
                                file.CheckIn("تم تحديث تخطيط ومحتوى الصفحة إلى DGA بنجاح", checkinType);
                            }

                            // 7. Publish
                            if (chkAutoPublish.Checked && file.Item.ParentList.EnableMinorVersions)
                            {
                                file.Publish("تم نشر النسخة المحدثة تلقائياً");
                            }

                            // 8. Approve
                            if (chkAutoApprove.Checked && file.Item.ParentList.EnableModeration)
                            {
                                file.Approve("تم اعتماد النسخة المحدثة تلقائياً");
                            }

                            _successCount++;
                            LogMessage(string.Format("نجح: تم تحديث ونشر الصفحة بنجاح: <a href='{0}' target='_blank' class='fw-bold text-decoration-none'>{1}</a> ({2})", 
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
                LogMessage(string.Format("خطأ أثناء معالجة ({0}): {1}", item.Url, ex.Message), "danger");
            }
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
                        if (pl.ServerRelativeUrl.Equals(layoutFile.ServerRelativeUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            return pl;
                        }
                    }

                    // Direct instantiation fallback
                    return new PageLayout(layoutFile.Item);
                }
            }
            catch
            {
                // Fallback attempt by searching by filename
                string layoutFileName = Path.GetFileName(layoutUrl);
                foreach (PageLayout pl in pubWeb.GetAvailablePageLayouts())
                {
                    if (pl.Name.Equals(layoutFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return pl;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Bulk Text Parser

        private List<PageMigrationItem> ParseBulkText(string text)
        {
            var list = new List<PageMigrationItem>();
            if (string.IsNullOrEmpty(text)) return list;

            // 1. Check for ===PAGE=== delimiter format
            if (text.Contains("===PAGE==="))
            {
                string[] blocks = text.Split(new[] { "===PAGE===" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string rawBlock in blocks)
                {
                    string block = rawBlock.Replace("===END===", "").Trim();
                    if (string.IsNullOrEmpty(block)) continue;

                    var item = new PageMigrationItem();
                    using (var reader = new StringReader(block))
                    {
                        string line;
                        var contentSb = new StringBuilder();
                        bool readingContent = false;

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (readingContent)
                            {
                                contentSb.AppendLine(line);
                            }
                            else if (line.StartsWith("URL:", StringComparison.OrdinalIgnoreCase))
                            {
                                item.Url = line.Substring(4).Trim();
                            }
                            else if (line.StartsWith("LAYOUT:", StringComparison.OrdinalIgnoreCase))
                            {
                                item.PageLayoutUrl = line.Substring(7).Trim();
                            }
                            else if (line.StartsWith("TITLE:", StringComparison.OrdinalIgnoreCase))
                            {
                                item.Title = line.Substring(6).Trim();
                            }
                            else if (line.StartsWith("CONTENT:", StringComparison.OrdinalIgnoreCase))
                            {
                                readingContent = true;
                                string inlineContent = line.Substring(8).Trim();
                                if (!string.IsNullOrEmpty(inlineContent))
                                {
                                    contentSb.AppendLine(inlineContent);
                                }
                            }
                        }

                        item.ContentHtml = contentSb.ToString().Trim();
                    }

                    if (!string.IsNullOrEmpty(item.Url))
                    {
                        list.Add(item);
                    }
                }
            }
            else
            {
                // 2. Simple line-by-line URL mode (if just URLs are pasted)
                using (var reader = new StringReader(text))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string trimmed = line.Trim();
                        if (trimmed.StartsWith("http", StringComparison.OrdinalIgnoreCase) || trimmed.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new PageMigrationItem
                            {
                                Url = trimmed,
                                PageLayoutUrl = ProgramPagesDgaData.DefaultLayout
                            });
                        }
                    }
                }
            }

            return list;
        }

        #endregion

        #region UI Logging Helpers

        private void LogMessage(string message, string levelCss)
        {
            string icon = "info-circle";
            if (levelCss == "success") icon = "checkmark-circle-02";
            else if (levelCss == "danger") icon = "cancel-circle";
            else if (levelCss == "warning") icon = "alert-02";

            _logBuilder.AppendFormat("<div class='py-1 border-bottom d-flex align-items-center gap-2 text-{0}'><i class='hgi hgi-stroke hgi-{1}'></i><span>{2}</span></div>",
                levelCss, icon, message);
        }

        private void ShowSummary(int total, int success, int failed, string extra)
        {
            pnlResults.Visible = true;

            string summaryHtml = string.Format(@"
                <div class='row g-3 mb-3 text-center'>
                    <div class='col-4'>
                        <div class='p-3 bg-primary-25 rounded-3 border'>
                            <span class='text-muted small d-block mb-1'>إجمالي العناصر</span>
                            <strong class='h4 text-primary mb-0 font-monospace'>{0}</strong>
                        </div>
                    </div>
                    <div class='col-4'>
                        <div class='p-3 bg-success-subtle rounded-3 border border-success-subtle'>
                            <span class='text-success-emphasis small d-block mb-1'>الناجحة</span>
                            <strong class='h4 text-success mb-0 font-monospace'>{1}</strong>
                        </div>
                    </div>
                    <div class='col-4'>
                        <div class='p-3 bg-danger-subtle rounded-3 border border-danger-subtle'>
                            <span class='text-danger-emphasis small d-block mb-1'>الفاشلة / المتخطاة</span>
                            <strong class='h4 text-danger mb-0 font-monospace'>{2}</strong>
                        </div>
                    </div>
                </div>
                <div class='alert alert-light border small text-muted mb-3'>{3}</div>",
                total, success, failed, extra);

            litSummaryCard.Text = summaryHtml;
            litDetailedLog.Text = _logBuilder.Length > 0 ? _logBuilder.ToString() : "<div class='text-muted'>لا توجد رسائل مسجلة.</div>";
        }

        #endregion
    }
}
