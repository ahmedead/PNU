using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebPartPages;

namespace PNU.Integration
{
    public class MissingPageSqlItem
    {
        public long Id { get; set; }
        public string PageFileName { get; set; }
        public string PageTitleAr { get; set; }
        public string PageTitleEn { get; set; }
        public string SourcePageUrl { get; set; }
        public string SourcePageFullUrl { get; set; }
        public string TargetPageUrl { get; set; }
        public string TargetPageFullUrl { get; set; }
        public string SourceWebUrl { get; set; }
        public string SourceWebFullUrl { get; set; }
        public string TargetWebUrl { get; set; }
        public string TargetWebFullUrl { get; set; }
        public string PageLayout { get; set; }
        public string PageLayoutFileName { get; set; }
        public bool HasControlLoader { get; set; }
        public string UserControlPath { get; set; }
        public string UserControlProperties { get; set; }
        public bool HasContentEditor { get; set; }
        public bool HasPageContent { get; set; }
        public string Direction { get; set; }
        public bool IsCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Batch job to create missing bilingual publishing pages from SQL database (dbo.MissingPagesOnEN)
    /// 50 pages at a time with full support for Page Layouts, Page Content, ControlLoaderWebPart,
    /// ContentEditorWebPart, Major CheckIn, Publish, and Moderation Approval.
    /// </summary>
    public static class MissingPagesJob
    {
        private const string WP_TYPE_NAME = "PNU.Internet.WebParts.ControlLoaderWebPart.ControlLoaderWebPart";
        private const string WP_ASSEMBLY_NAME = "PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3";
        private const string DEFAULT_ZONE_ID = "TopZone";
        private const int DEFAULT_BATCH_SIZE = 50;

        private static string _cachedConnectionString = null;

        public static string GetSqlConnectionString()
        {
            if (!string.IsNullOrEmpty(_cachedConnectionString))
                return _cachedConnectionString;

            // 1. Try App.config connection strings
            try
            {
                if (ConfigurationManager.ConnectionStrings["PNU_SearchIndex"] != null &&
                    !string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["PNU_SearchIndex"].ConnectionString))
                {
                    _cachedConnectionString = ConfigurationManager.ConnectionStrings["PNU_SearchIndex"].ConnectionString;
                    return _cachedConnectionString;
                }

                if (ConfigurationManager.ConnectionStrings["SharePointExportDb"] != null &&
                    !string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["SharePointExportDb"].ConnectionString))
                {
                    _cachedConnectionString = ConfigurationManager.ConnectionStrings["SharePointExportDb"].ConnectionString;
                    return _cachedConnectionString;
                }
            }
            catch { }

            // 2. Try fetching from SharePoint list (Admin web -> ConnectionStrings list)
            try
            {
                string rootUrl = Config.RootSiteUrl;
                if (string.IsNullOrEmpty(rootUrl)) rootUrl = "https://pnu.edu.sa";

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(rootUrl))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            if (web != null && web.Exists)
                            {
                                SPList list = web.Lists.TryGetList("ConnectionStrings");
                                if (list != null)
                                {
                                    SPQuery query = new SPQuery();
                                    query.Query = @"<Where>
                                                      <Eq>
                                                         <FieldRef Name='Title' />
                                                         <Value Type='Text'>PNU_SearchIndex</Value>
                                                      </Eq>
                                                   </Where>";

                                    SPListItemCollection coll = list.GetItems(query);
                                    if (coll != null && coll.Count > 0 && coll[0]["ConnectionString"] != null)
                                    {
                                        _cachedConnectionString = coll[0]["ConnectionString"].ToString();
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                OracleDBContext.LogToFile($"Warning: Could not retrieve connection string from SharePoint Admin list: {ex.Message}");
            }

            return _cachedConnectionString;
        }

        /// <summary>
        /// Main entry point for the missing pages creation job.
        /// </summary>
        /// <param name="batchSize">Number of pages to process per batch (default: 50)</param>
        /// <param name="maxPages">Optional limit on total pages to process (0 = all pending)</param>
        /// <param name="urlFilter">Optional filter for specific target or source web URL</param>
        public static void CreateMissingPagesFromSql(int batchSize = DEFAULT_BATCH_SIZE, int maxPages = 0, string urlFilter = null)
        {
            if (batchSize <= 0) batchSize = DEFAULT_BATCH_SIZE;

            string connStr = GetSqlConnectionString();
            if (string.IsNullOrEmpty(connStr))
            {
                OracleDBContext.LogToFile("ERROR: SQL Connection string for dbo.MissingPagesOnEN could not be determined.");
                return;
            }

            OracleDBContext.LogToFile("================================================================================");
            OracleDBContext.LogToFile($"=== START MISSING PAGES CREATION JOB (Batch Size: {batchSize}, Max Pages: {(maxPages > 0 ? maxPages.ToString() : "ALL")}, Filter: '{urlFilter ?? "None"}') ===");
            OracleDBContext.LogToFile("================================================================================");

            List<MissingPageSqlItem> pendingPages = LoadPendingMissingPagesFromSql(connStr, urlFilter, maxPages);

            if (pendingPages == null || pendingPages.Count == 0)
            {
                OracleDBContext.LogToFile("No pending missing pages found in database (dbo.MissingPagesOnEN WHERE IsCreated = 0).");
                OracleDBContext.LogToFile("=== MISSING PAGES CREATION JOB FINISHED ===");
                return;
            }

            int totalPages = pendingPages.Count;
            int totalBatches = (int)Math.Ceiling((double)totalPages / batchSize);

            OracleDBContext.LogToFile($"Found {totalPages} pending page(s) to create across {totalBatches} batch(es).");

            string rootSiteUrl = Config.RootSiteUrl;
            if (string.IsNullOrEmpty(rootSiteUrl)) rootSiteUrl = "https://pnu.edu.sa";

            int overallSuccess = 0;
            int overallFailed = 0;
            int overallSkipped = 0;

            for (int b = 0; b < totalBatches; b++)
            {
                int currentBatchNum = b + 1;
                var currentBatch = pendingPages.Skip(b * batchSize).Take(batchSize).ToList();

                OracleDBContext.LogToFile($"--------------------------------------------------------------------------------");
                OracleDBContext.LogToFile($"▶ BATCH {currentBatchNum}/{totalBatches} START: Processing {currentBatch.Count} page(s) (Items {b * batchSize + 1} to {b * batchSize + currentBatch.Count} of {totalPages})...");
                OracleDBContext.LogToFile($"--------------------------------------------------------------------------------");

                int batchSuccess = 0;
                int batchFailed = 0;
                int batchSkipped = 0;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(rootSiteUrl))
                    {
                        for (int i = 0; i < currentBatch.Count; i++)
                        {
                            var item = currentBatch[i];
                            int globalIndex = (b * batchSize) + i + 1;

                            try
                            {
                                var result = CreateSinglePage(site, item, connStr);

                                if (result == PageCreationResult.Success)
                                {
                                    batchSuccess++;
                                    overallSuccess++;
                                    OracleDBContext.LogToFile($"  [{globalIndex}/{totalPages}] [SUCCESS] Created '{item.PageFileName}' on '{item.TargetWebUrl}'");
                                }
                                else if (result == PageCreationResult.AlreadyExists)
                                {
                                    batchSkipped++;
                                    overallSkipped++;
                                    OracleDBContext.LogToFile($"  [{globalIndex}/{totalPages}] [EXISTS] Page '{item.PageFileName}' already exists on '{item.TargetWebUrl}'. Marked as Created.");
                                }
                                else
                                {
                                    batchFailed++;
                                    overallFailed++;
                                    OracleDBContext.LogToFile($"  [{globalIndex}/{totalPages}] [FAILED] Failed creating '{item.PageFileName}' on '{item.TargetWebUrl}'");
                                }
                            }
                            catch (Exception ex)
                            {
                                batchFailed++;
                                overallFailed++;
                                OracleDBContext.LogToFile($"  [{globalIndex}/{totalPages}] [EXCEPTION] '{item.PageFileName}': {ex.Message}");
                            }
                        }
                    }
                });

                OracleDBContext.LogToFile($"✔ BATCH {currentBatchNum}/{totalBatches} FINISHED -> Success: {batchSuccess}, Skipped: {batchSkipped}, Failed: {batchFailed}");

                // Small pause between batches to prevent SharePoint farm overload
                if (currentBatchNum < totalBatches)
                {
                    OracleDBContext.LogToFile($"Pausing 2 seconds before next batch...");
                    Thread.Sleep(2000);
                }
            }

            OracleDBContext.LogToFile("================================================================================");
            OracleDBContext.LogToFile($"=== MISSING PAGES CREATION SUMMARY ===");
            OracleDBContext.LogToFile($"  Total Pending Processed : {totalPages}");
            OracleDBContext.LogToFile($"  Successfully Created   : {overallSuccess}");
            OracleDBContext.LogToFile($"  Already Existed / Synced: {overallSkipped}");
            OracleDBContext.LogToFile($"  Failed                  : {overallFailed}");
            OracleDBContext.LogToFile("================================================================================");
        }

        private enum PageCreationResult
        {
            Success,
            AlreadyExists,
            Failed
        }

        private static PageCreationResult CreateSinglePage(SPSite site, MissingPageSqlItem item, string connStr)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.TargetWebUrl) || string.IsNullOrWhiteSpace(item.PageFileName))
                return PageCreationResult.Failed;

            using (SPWeb tgtWeb = site.OpenWeb(item.TargetWebUrl, false))
            {
                if (tgtWeb == null || !tgtWeb.Exists)
                {
                    OracleDBContext.LogToFile($"    Target web does not exist: {item.TargetWebUrl}");
                    return PageCreationResult.Failed;
                }

                if (!PublishingWeb.IsPublishingWeb(tgtWeb))
                {
                    OracleDBContext.LogToFile($"    Target web is not a Publishing Web: {item.TargetWebUrl}");
                    return PageCreationResult.Failed;
                }

                tgtWeb.AllowUnsafeUpdates = true;
                try
                {
                    PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(tgtWeb);
                    string pagesListName = GetPagesListName(tgtWeb);
                    string pageServerRelUrl = tgtWeb.ServerRelativeUrl.TrimEnd('/') + "/" + pagesListName + "/" + item.PageFileName.TrimStart('/');

                    SPFile existingFile = GetPageFileInWeb(tgtWeb, item.PageFileName) ?? tgtWeb.GetFile(pageServerRelUrl);

                    if (existingFile != null && existingFile.Exists)
                    {
                        // Ensure it's published and mark as created in SQL
                        MarkPageAsCreatedInSql(connStr, item.TargetPageUrl, item.TargetPageFullUrl ?? site.MakeFullUrl(pageServerRelUrl));
                        return PageCreationResult.AlreadyExists;
                    }

                    // 1. Inspect source page directly if available to get the freshest content and web parts
                    string sourceHtmlContent = null;
                    string contentEditorHtml = null;
                    string contentEditorLink = null;
                    string controlLoaderPath = item.UserControlPath;
                    string controlLoaderProps = item.UserControlProperties;
                    string pageLayoutUrl = item.PageLayout;
                    string resolvedTitle = !string.IsNullOrEmpty(item.PageTitleEn) 
                        ? item.PageTitleEn 
                        : (!string.IsNullOrEmpty(item.PageTitleAr) ? item.PageTitleAr : Path.GetFileNameWithoutExtension(item.PageFileName));

                    if (!string.IsNullOrEmpty(item.SourceWebUrl))
                    {
                        try
                        {
                            using (SPWeb srcWeb = site.OpenWeb(item.SourceWebUrl, false))
                            {
                                if (srcWeb != null && srcWeb.Exists)
                                {
                                    SPFile srcFile = GetPageFileInWeb(srcWeb, item.PageFileName);
                                    if (srcFile != null && srcFile.Exists)
                                    {
                                        PublishingPage srcPubPage = null;
                                        try { srcPubPage = PublishingPage.GetPublishingPage(srcFile.Item); } catch { }

                                        if (srcPubPage != null)
                                        {
                                            if (string.IsNullOrEmpty(resolvedTitle) || resolvedTitle == Path.GetFileNameWithoutExtension(item.PageFileName))
                                            {
                                                resolvedTitle = srcPubPage.Title;
                                            }

                                            if (string.IsNullOrEmpty(pageLayoutUrl) && srcPubPage.Layout != null)
                                            {
                                                pageLayoutUrl = srcPubPage.Layout.ServerRelativeUrl;
                                            }

                                            sourceHtmlContent = GetPageContentHtml(srcPubPage);
                                        }

                                        // Inspect Web Parts on source page
                                        try
                                        {
                                            using (SPLimitedWebPartManager wpManager = srcFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                                            {
                                                foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpManager.WebParts)
                                                {
                                                    string wpTypeName = wp.GetType().FullName;
                                                    if (wpTypeName.IndexOf("ControlLoaderWebPart", StringComparison.OrdinalIgnoreCase) >= 0)
                                                    {
                                                        item.HasControlLoader = true;
                                                        var pathProp = wp.GetType().GetProperty("UserControlPath");
                                                        if (pathProp != null)
                                                            controlLoaderPath = Convert.ToString(pathProp.GetValue(wp, null));

                                                        var propsProp = wp.GetType().GetProperty("UserControlProperties");
                                                        if (propsProp != null)
                                                            controlLoaderProps = Convert.ToString(propsProp.GetValue(wp, null));
                                                    }
                                                    else if (wp is ContentEditorWebPart cewp)
                                                    {
                                                        item.HasContentEditor = true;
                                                        if (cewp.Content != null)
                                                        {
                                                            string xml = cewp.Content.InnerXml;
                                                            contentEditorHtml = !string.IsNullOrWhiteSpace(xml) ? xml : cewp.Content.InnerText;
                                                        }
                                                        if (!string.IsNullOrEmpty(cewp.ContentLink))
                                                        {
                                                            contentEditorLink = cewp.ContentLink;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                        catch { }
                    }

                    // 2. Resolve Page Layout
                    PageLayout layout = ResolvePageLayout(site, pubWeb, pageLayoutUrl);
                    if (layout == null)
                    {
                        OracleDBContext.LogToFile($"    Page Layout could not be resolved: '{pageLayoutUrl}' for '{item.PageFileName}'");
                        return PageCreationResult.Failed;
                    }

                    // 3. Create Publishing Page
                    PublishingPage newPage = pubWeb.GetPublishingPages().Add(item.PageFileName, layout);
                    newPage.Title = resolvedTitle;

                    // 4. Set Page Content (PublishingPageContent / RichHtmlField)
                    if (!string.IsNullOrWhiteSpace(sourceHtmlContent))
                    {
                        SetPageContentHtml(newPage, sourceHtmlContent);
                    }

                    newPage.Update();

                    // 5. Add Web Parts
                    SPFile newPageFile = newPage.ListItem.File;
                    using (SPLimitedWebPartManager wpManager = newPageFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                    {
                        // ControlLoaderWebPart
                        if (item.HasControlLoader && !string.IsNullOrEmpty(controlLoaderPath))
                        {
                            AddControlLoaderWebPart(wpManager, controlLoaderPath, controlLoaderProps, DEFAULT_ZONE_ID);
                        }

                        // ContentEditorWebPart
                        if (item.HasContentEditor && (!string.IsNullOrEmpty(contentEditorHtml) || !string.IsNullOrEmpty(contentEditorLink)))
                        {
                            AddContentEditorWebPart(wpManager, contentEditorHtml, contentEditorLink, DEFAULT_ZONE_ID);
                        }
                    }

                    // 6. Check In, Publish, Approve
                    if (newPageFile.CheckOutType != SPFile.SPCheckOutType.None)
                    {
                        newPageFile.CheckIn("Created by MissingPages batch job", SPCheckinType.MajorCheckIn);
                    }

                    if (newPageFile.Item.ParentList.EnableMinorVersions)
                    {
                        newPageFile.Publish("Published automatically by MissingPages batch job");
                    }

                    if (newPageFile.Item.ParentList.EnableModeration)
                    {
                        newPageFile.Approve("Approved automatically by MissingPages batch job");
                    }

                    // 7. Update status in SQL (dbo.MissingPagesOnEN)
                    string fullTargetUrl = site.MakeFullUrl(pageServerRelUrl);
                    MarkPageAsCreatedInSql(connStr, item.TargetPageUrl, fullTargetUrl ?? item.TargetPageFullUrl);

                    return PageCreationResult.Success;
                }
                finally
                {
                    tgtWeb.AllowUnsafeUpdates = false;
                }
            }
        }

        #region Layout & WebPart Helpers

        private static PageLayout ResolvePageLayout(SPSite site, PublishingWeb pubWeb, string layoutUrl)
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

            // Fallback default
            try
            {
                foreach (PageLayout pl in pubWeb.GetAvailablePageLayouts())
                {
                    if (pl.Name.IndexOf("NewSideMenu", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        pl.Name.IndexOf("DGANewBlankWebPartPage", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return pl;
                    }
                }
            }
            catch { }

            return null;
        }

        private static void AddControlLoaderWebPart(SPLimitedWebPartManager wpManager, string userControlPath, string userControlProperties, string zoneId)
        {
            try
            {
                Assembly asm = Assembly.Load(WP_ASSEMBLY_NAME);
                Type wpType = asm.GetType(WP_TYPE_NAME);

                if (wpType == null)
                {
                    OracleDBContext.LogToFile($"    Warning: WebPart type not found: {WP_TYPE_NAME}");
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
                OracleDBContext.LogToFile($"    Warning: Could not add ControlLoaderWebPart: {ex.Message}");
            }
        }

        private static void AddContentEditorWebPart(SPLimitedWebPartManager wpManager, string htmlContent, string contentLink, string zoneId)
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
                    try
                    {
                        elem.InnerXml = htmlContent;
                    }
                    catch
                    {
                        elem.InnerText = htmlContent;
                    }
                    cewp.Content = elem;
                }

                string targetZone = string.IsNullOrEmpty(zoneId) ? DEFAULT_ZONE_ID : zoneId;
                wpManager.AddWebPart(cewp, targetZone, 1);
            }
            catch (Exception ex)
            {
                OracleDBContext.LogToFile($"    Warning: Could not add ContentEditorWebPart: {ex.Message}");
            }
        }

        private static string GetPageContentHtml(PublishingPage pubPage)
        {
            if (pubPage == null || pubPage.ListItem == null) return string.Empty;
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
                return Convert.ToString(pubPage.ListItem["PublishingPageContent"]);
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
                return Convert.ToString(pubPage.ListItem["PublishingPageContentHtml"]);
            if (pubPage.ListItem.Fields.ContainsField("PageContent"))
                return Convert.ToString(pubPage.ListItem["PageContent"]);
            return string.Empty;
        }

        private static void SetPageContentHtml(PublishingPage pubPage, string content)
        {
            if (pubPage == null || pubPage.ListItem == null || string.IsNullOrEmpty(content)) return;
            if (pubPage.ListItem.Fields.ContainsField("PublishingPageContent"))
                pubPage.ListItem["PublishingPageContent"] = content;
            else if (pubPage.ListItem.Fields.ContainsField("PublishingPageContentHtml"))
                pubPage.ListItem["PublishingPageContentHtml"] = content;
            else if (pubPage.ListItem.Fields.ContainsField("PageContent"))
                pubPage.ListItem["PageContent"] = content;
        }

        private static string GetPagesListName(SPWeb web)
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

        private static SPFile GetPageFileInWeb(SPWeb web, string fileName)
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
            }
            catch { }
            return null;
        }

        #endregion

        #region SQL Data Access

        private static List<MissingPageSqlItem> LoadPendingMissingPagesFromSql(string connStr, string urlFilter, int maxPages)
        {
            var list = new List<MissingPageSqlItem>();
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    var sb = new StringBuilder();
                    sb.Append("SELECT ");
                    if (maxPages > 0) sb.Append($"TOP {maxPages} ");
                    sb.Append("Id, PageFileName, PageTitleAr, PageTitleEn, SourcePageUrl, SourcePageFullUrl, " +
                              "TargetPageUrl, TargetPageFullUrl, SourceWebUrl, SourceWebFullUrl, TargetWebUrl, TargetWebFullUrl, " +
                              "PageLayout, PageLayoutFileName, HasControlLoader, UserControlPath, UserControlProperties, " +
                              "HasContentEditor, HasPageContent, Direction, IsCreated, CreatedDate, Status " +
                              "FROM dbo.MissingPagesOnEN WHERE IsCreated = 0 ");

                    if (!string.IsNullOrWhiteSpace(urlFilter))
                    {
                        sb.Append("AND (TargetWebUrl LIKE @UrlFilter OR TargetPageUrl LIKE @UrlFilter OR SourceWebUrl LIKE @UrlFilter) ");
                    }

                    sb.Append("ORDER BY TargetWebUrl, PageFileName, Id");

                    using (var cmd = new SqlCommand(sb.ToString(), conn))
                    {
                        cmd.CommandTimeout = 120;
                        if (!string.IsNullOrWhiteSpace(urlFilter))
                        {
                            cmd.Parameters.AddWithValue("@UrlFilter", "%" + urlFilter.Trim().TrimEnd('/') + "%");
                        }

                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                list.Add(new MissingPageSqlItem
                                {
                                    Id = Convert.ToInt64(rdr["Id"]),
                                    PageFileName = rdr["PageFileName"] as string,
                                    PageTitleAr = rdr["PageTitleAr"] as string,
                                    PageTitleEn = rdr["PageTitleEn"] as string,
                                    SourcePageUrl = rdr["SourcePageUrl"] as string,
                                    SourcePageFullUrl = rdr["SourcePageFullUrl"] as string,
                                    TargetPageUrl = rdr["TargetPageUrl"] as string,
                                    TargetPageFullUrl = rdr["TargetPageFullUrl"] as string,
                                    SourceWebUrl = rdr["SourceWebUrl"] as string,
                                    SourceWebFullUrl = rdr["SourceWebFullUrl"] as string,
                                    TargetWebUrl = rdr["TargetWebUrl"] as string,
                                    TargetWebFullUrl = rdr["TargetWebFullUrl"] as string,
                                    PageLayout = rdr["PageLayout"] as string,
                                    PageLayoutFileName = rdr["PageLayoutFileName"] as string,
                                    HasControlLoader = Convert.ToBoolean(rdr["HasControlLoader"]),
                                    UserControlPath = rdr["UserControlPath"] as string,
                                    UserControlProperties = rdr["UserControlProperties"] as string,
                                    HasContentEditor = Convert.ToBoolean(rdr["HasContentEditor"]),
                                    HasPageContent = Convert.ToBoolean(rdr["HasPageContent"]),
                                    Direction = rdr["Direction"] as string,
                                    IsCreated = Convert.ToBoolean(rdr["IsCreated"]),
                                    CreatedDate = rdr["CreatedDate"] != DBNull.Value ? (DateTime?)rdr["CreatedDate"] : null,
                                    Status = rdr["Status"] as string
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OracleDBContext.LogToFile($"ERROR loading missing pages from SQL: {ex.Message}");
            }

            return list;
        }

        private static void MarkPageAsCreatedInSql(string connStr, string targetPageUrl, string targetPageFullUrl)
        {
            if (string.IsNullOrEmpty(connStr)) return;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"UPDATE dbo.MissingPagesOnEN 
                                                   SET IsCreated = 1, 
                                                       Status = 'Created', 
                                                       CreatedDate = ISNULL(CreatedDate, GETDATE()), 
                                                       LastUpdatedDate = GETDATE() 
                                                   WHERE TargetPageUrl = @Url 
                                                      OR TargetPageFullUrl = @FullUrl 
                                                      OR (@Url IS NOT NULL AND TargetPageUrl LIKE '%' + @Url)", conn))
                {
                    cmd.CommandTimeout = 30;
                    cmd.Parameters.AddWithValue("@Url", (object)targetPageUrl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FullUrl", (object)targetPageFullUrl ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                OracleDBContext.LogToFile($"    Warning: Could not update status in SQL for '{targetPageUrl}': {ex.Message}");
            }
        }

        #endregion
    }
}
