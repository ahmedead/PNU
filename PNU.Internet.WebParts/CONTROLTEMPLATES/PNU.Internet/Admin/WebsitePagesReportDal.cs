using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    #region DTOs

    [Serializable]
    public class DashboardSummaryDto
    {
        public int TotalPages { get; set; }
        public int TotalArabicPages { get; set; }
        public int TotalEnglishPages { get; set; }
        public int TotalEnExistsPages { get; set; }
        public int TotalMissingEnPages { get; set; }
        public int TotalMissingArPages { get; set; }
        public int TotalMissingPagesTotal { get; set; }
        public int TotalResolvedPages { get; set; }
        public int TotalPendingPages { get; set; }
        public int TotalPageLayouts { get; set; }
        public int DgaCompliantPages { get; set; }
        public double DgaCompliancePercentage { get; set; }
        public double BilingualParityPercentage { get; set; }
        public int TotalUserControlsCount { get; set; }
        public int TotalSubsitesCount { get; set; }
        public DateTime LastScanDate { get; set; }
    }

    [Serializable]
    public class PageLayoutSummaryDto
    {
        public string PageLayout { get; set; }
        public string LayoutFileName { get; set; }
        public int PageCount { get; set; }
        public double Percentage { get; set; }
        public bool IsDgaCompliant { get; set; }
        public int EnExistsCount { get; set; }
        public int MissingEnCount { get; set; }
        public string SamplePageUrl { get; set; }
    }

    [Serializable]
    public class WebsitePageItemDto
    {
        public long PageId { get; set; }
        public string PageTitle { get; set; }
        public string PageURL { get; set; }
        public string PageLayout { get; set; }
        public string PageLayoutFileName { get; set; }
        public string UserControlPath { get; set; }
        public string UserControlProperties { get; set; }
        public string WebUrl { get; set; }
        public string PageTitleEn { get; set; }
        public string PageURLEn { get; set; }
        public string PageLayoutEn { get; set; }
        public string UserControlPathEn { get; set; }
        public string UserControlPropertiesEn { get; set; }
        public string WebUrlEn { get; set; }
        public bool EnExists { get; set; }
        public bool IsDgaCompliant { get; set; }
        public DateTime LastIndexed { get; set; }
    }

    [Serializable]
    public class MissingPageReportDto
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
        public string Direction { get; set; } // "ArToEn" or "EnToAr"
        public bool IsCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Status { get; set; } // "Missing", "Created", "Pending"
        public DateTime LastDiscoveredDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    [Serializable]
    public class UserControlSummaryDto
    {
        public string UserControlPath { get; set; }
        public string ControlFileName { get; set; }
        public int PageCount { get; set; }
        public int ArPagesCount { get; set; }
        public int EnPagesCount { get; set; }
        public string SamplePageUrl { get; set; }
        public string SamplePageTitle { get; set; }
    }

    [Serializable]
    public class SubsiteHealthDto
    {
        public string WebUrl { get; set; }
        public string WebTitle { get; set; }
        public int TotalPages { get; set; }
        public int HasEnglishCount { get; set; }
        public int MissingEnglishCount { get; set; }
        public int DgaCompliantCount { get; set; }
        public double ParityPercentage { get; set; }
        public double DgaPercentage { get; set; }
    }

    #endregion

    /// <summary>
    /// Data Access Layer for Website Pages, Layouts, Missing Pages, and User Controls reports.
    /// Accesses dbo.WebsitePages and dbo.MissingPagesOnEN in the PNU_SearchIndex database.
    /// </summary>
    public static class WebsitePagesReportDal
    {
        public static bool IsDgaLayout(string layout)
        {
            if (string.IsNullOrEmpty(layout)) return false;
            if (layout.IndexOf("DGA", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (layout.IndexOf("NewSideMenu", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }

        public static string ConnectionString
        {
            get
            {
                string connectionName = "PNU_SearchIndex";
                string connection = "";

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    try
                    {
                        string siteUrl = SPContext.Current != null && SPContext.Current.Site != null
                            ? SPContext.Current.Site.Url
                            : "https://pnu.edu.sa";

                        using (SPSite site = new SPSite(siteUrl))
                        {
                            using (SPWeb web = site.OpenWeb("Admin"))
                            {
                                SPList list = web.Lists.TryGetList("ConnectionStrings");
                                if (list != null)
                                {
                                    SPQuery query = new SPQuery();
                                    query.Query = string.Concat(
                                        $@"<Where>
                                            <Eq>
                                                <FieldRef Name='Title' />
                                                <Value Type='Text'>{connectionName}</Value>
                                            </Eq>
                                        </Where>");

                                    SPListItemCollection coll = list.GetItems(query);
                                    if (coll != null && coll.Count > 0 && coll[0]["ConnectionString"] != null)
                                    {
                                        connection = coll[0]["ConnectionString"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    catch { }

                    if (string.IsNullOrEmpty(connection))
                    {
                        try
                        {
                            var cs = ConfigurationManager.ConnectionStrings["PNU_SearchIndex"];
                            if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
                            {
                                connection = cs.ConnectionString;
                            }
                        }
                        catch { }
                    }
                });

                return connection;
            }
        }

        public static void EnsureTablesExist()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    const string ddl = @"
IF OBJECT_ID('dbo.WebsitePages', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.WebsitePages
    (
        PageId                BIGINT IDENTITY(1,1) PRIMARY KEY,
        PageTitle             NVARCHAR(500)   NULL,
        PageURL               NVARCHAR(1000)  NOT NULL,
        PageLayout            NVARCHAR(500)   NULL,
        UserControlPath       NVARCHAR(MAX)   NULL,
        UserControlProperties NVARCHAR(MAX)   NULL,
        WebUrl                NVARCHAR(1000)  NULL,
        PageTitleEn           NVARCHAR(500)   NULL,
        PageURLEn             NVARCHAR(1000)  NULL,
        PageLayoutEn          NVARCHAR(500)   NULL,
        UserControlPathEn     NVARCHAR(MAX)   NULL,
        UserControlPropertiesEn NVARCHAR(MAX) NULL,
        WebUrlEn              NVARCHAR(1000)  NULL,
        EnExists              BIT             NOT NULL DEFAULT(0),
        LastIndexed           DATETIME        NOT NULL DEFAULT(GETDATE())
    );
    CREATE UNIQUE INDEX UX_WebsitePages_PageURL ON dbo.WebsitePages(PageURL);
END;

IF OBJECT_ID('dbo.MissingPagesOnEN', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MissingPagesOnEN
    (
        Id                     BIGINT IDENTITY(1,1) PRIMARY KEY,
        PageFileName           NVARCHAR(500)   NOT NULL,
        PageTitleAr            NVARCHAR(500)   NULL,
        PageTitleEn            NVARCHAR(500)   NULL,
        SourcePageUrl          NVARCHAR(1000)  NULL,
        SourcePageFullUrl      NVARCHAR(1000)  NULL,
        TargetPageUrl          NVARCHAR(1000)  NOT NULL,
        TargetPageFullUrl      NVARCHAR(1000)  NOT NULL,
        SourceWebUrl           NVARCHAR(1000)  NULL,
        SourceWebFullUrl       NVARCHAR(1000)  NULL,
        TargetWebUrl           NVARCHAR(1000)  NULL,
        TargetWebFullUrl       NVARCHAR(1000)  NULL,
        PageLayout             NVARCHAR(500)   NULL,
        PageLayoutFileName     NVARCHAR(500)   NULL,
        HasControlLoader       BIT             NOT NULL DEFAULT(0),
        UserControlPath        NVARCHAR(MAX)   NULL,
        UserControlProperties  NVARCHAR(MAX)   NULL,
        HasContentEditor       BIT             NOT NULL DEFAULT(0),
        HasPageContent         BIT             NOT NULL DEFAULT(0),
        Direction              NVARCHAR(50)    NOT NULL DEFAULT('ArToEn'),
        IsCreated              BIT             NOT NULL DEFAULT(0),
        CreatedDate            DATETIME        NULL,
        Status                 NVARCHAR(100)   NOT NULL DEFAULT('Missing'),
        LastDiscoveredDate     DATETIME        NOT NULL DEFAULT(GETDATE()),
        LastUpdatedDate        DATETIME        NOT NULL DEFAULT(GETDATE())
    );
    CREATE UNIQUE INDEX UX_MissingPagesOnEN_TargetPageUrl ON dbo.MissingPagesOnEN(TargetPageUrl);
END;";

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(ddl, conn))
                        {
                            cmd.CommandTimeout = 60;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch { }
            });
        }

        #region Dashboard Summary

        public static DashboardSummaryDto GetDashboardSummary()
        {
            var summary = new DashboardSummaryDto();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureTablesExist();
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        // 1. Website Pages metrics
                        const string pagesSql = @"
SELECT 
    COUNT(*) AS TotalPages,
    ISNULL(SUM(CASE WHEN PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%' THEN 1 ELSE 0 END), 0) AS ArabicPages,
    ISNULL(SUM(CASE WHEN PageURL LIKE '%/en/%' THEN 1 ELSE 0 END), 0) AS EnglishPages,
    ISNULL(SUM(CASE WHEN (PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%') AND EnExists = 1 THEN 1 ELSE 0 END), 0) AS EnExistsCount,
    ISNULL(SUM(CASE WHEN (PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%') AND EnExists = 0 THEN 1 ELSE 0 END), 0) AS MissingEnCount,
    ISNULL(SUM(CASE WHEN PageLayout LIKE '%DGA%' OR PageLayout LIKE '%NewSideMenu%' OR PageLayoutEn LIKE '%DGA%' OR PageLayoutEn LIKE '%NewSideMenu%' THEN 1 ELSE 0 END), 0) AS DgaCount,
    COUNT(DISTINCT NULLIF(PageLayout, '')) AS LayoutCount,
    COUNT(DISTINCT NULLIF(WebUrl, '')) AS SubsiteCount,
    ISNULL(MAX(LastIndexed), GETDATE()) AS LastScan
FROM dbo.WebsitePages";

                        using (var cmd = new SqlCommand(pagesSql, conn))
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                summary.TotalPages = Convert.ToInt32(rdr["TotalPages"]);
                                summary.TotalArabicPages = Convert.ToInt32(rdr["ArabicPages"]);
                                summary.TotalEnglishPages = Convert.ToInt32(rdr["EnglishPages"]);
                                summary.TotalEnExistsPages = Convert.ToInt32(rdr["EnExistsCount"]);
                                summary.TotalMissingEnPages = Convert.ToInt32(rdr["MissingEnCount"]);
                                summary.DgaCompliantPages = Convert.ToInt32(rdr["DgaCount"]);
                                summary.TotalPageLayouts = Convert.ToInt32(rdr["LayoutCount"]);
                                summary.TotalSubsitesCount = Convert.ToInt32(rdr["SubsiteCount"]);
                                summary.LastScanDate = Convert.ToDateTime(rdr["LastScan"]);
                            }
                        }

                        // 2. Missing Pages table metrics
                        const string missingSql = @"
SELECT 
    ISNULL(SUM(CASE WHEN Status = 'Missing' THEN 1 ELSE 0 END), 0) AS TotalMissing,
    ISNULL(SUM(CASE WHEN Status = 'Missing' AND Direction = 'ArToEn' THEN 1 ELSE 0 END), 0) AS MissingEn,
    ISNULL(SUM(CASE WHEN Status = 'Missing' AND Direction = 'EnToAr' THEN 1 ELSE 0 END), 0) AS MissingAr,
    ISNULL(SUM(CASE WHEN IsCreated = 1 OR Status = 'Created' OR Status = 'Resolved' THEN 1 ELSE 0 END), 0) AS ResolvedCount,
    ISNULL(SUM(CASE WHEN Status = 'Missing' THEN 1 ELSE 0 END), 0) AS PendingCount
FROM dbo.MissingPagesOnEN";

                        using (var cmd = new SqlCommand(missingSql, conn))
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                summary.TotalMissingPagesTotal = Convert.ToInt32(rdr["TotalMissing"]);
                                summary.TotalMissingEnPages = Convert.ToInt32(rdr["MissingEn"]);
                                summary.TotalMissingArPages = Convert.ToInt32(rdr["MissingAr"]);
                                summary.TotalResolvedPages = Convert.ToInt32(rdr["ResolvedCount"]);
                                summary.TotalPendingPages = Convert.ToInt32(rdr["PendingCount"]);
                            }
                        }

                        // 3. User Controls Count
                        const string ucSql = @"
SELECT COUNT(DISTINCT UserControlPath) AS UcCount 
FROM dbo.WebsitePages 
WHERE UserControlPath IS NOT NULL AND LEN(LTRIM(RTRIM(UserControlPath))) > 0";

                        using (var cmd = new SqlCommand(ucSql, conn))
                        {
                            object val = cmd.ExecuteScalar();
                            if (val != null && val != DBNull.Value)
                                summary.TotalUserControlsCount = Convert.ToInt32(val);
                        }
                    }
                }
                catch { }
            });

            if (summary.TotalArabicPages > 0)
            {
                summary.BilingualParityPercentage = Math.Min(100.0, Math.Round(((double)summary.TotalEnExistsPages / summary.TotalArabicPages) * 100.0, 1));
            }
            if (summary.TotalPages > 0)
            {
                summary.DgaCompliancePercentage = Math.Min(100.0, Math.Round(((double)summary.DgaCompliantPages / summary.TotalPages) * 100.0, 1));
            }

            return summary;
        }

        #endregion

        #region Page Layouts Report

        public static List<PageLayoutSummaryDto> GetPageLayoutsSummary()
        {
            var list = new List<PageLayoutSummaryDto>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureTablesExist();
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        const string sql = @"
WITH TotalCalc AS (SELECT COUNT(*) AS GrandTotal FROM dbo.WebsitePages)
SELECT 
    ISNULL(NULLIF(LTRIM(RTRIM(PageLayout)), ''), '[بدون تخطيط محدد / افتراضي]') AS PageLayout,
    COUNT(*) AS PageCount,
    CAST(COUNT(*) * 100.0 / (SELECT GrandTotal FROM TotalCalc) AS DECIMAL(5,2)) AS Percentage,
    SUM(CASE WHEN (PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%') AND EnExists = 1 THEN 1 ELSE 0 END) AS EnExistsCount,
    SUM(CASE WHEN (PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%') AND EnExists = 0 THEN 1 ELSE 0 END) AS MissingEnCount,
    MIN(PageURL) AS SampleUrl
FROM dbo.WebsitePages
GROUP BY ISNULL(NULLIF(LTRIM(RTRIM(PageLayout)), ''), '[بدون تخطيط محدد / افتراضي]')
ORDER BY PageCount DESC";

                        using (var cmd = new SqlCommand(sql, conn))
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                string layout = rdr["PageLayout"].ToString();
                                string fileName = layout;
                                int slash = layout.LastIndexOf('/');
                                if (slash >= 0) fileName = layout.Substring(slash + 1);

                                bool isDga = IsDgaLayout(layout);

                                list.Add(new PageLayoutSummaryDto
                                {
                                    PageLayout = layout,
                                    LayoutFileName = fileName,
                                    PageCount = Convert.ToInt32(rdr["PageCount"]),
                                    Percentage = Convert.ToDouble(rdr["Percentage"]),
                                    IsDgaCompliant = isDga,
                                    EnExistsCount = Convert.ToInt32(rdr["EnExistsCount"]),
                                    MissingEnCount = Convert.ToInt32(rdr["MissingEnCount"]),
                                    SamplePageUrl = rdr["SampleUrl"] as string
                                });
                            }
                        }
                    }
                }
                catch { }
            });

            return list;
        }

        public static List<WebsitePageItemDto> GetPages(string layoutFilter = null, string subsiteFilter = null, string keyword = null, string enStatusFilter = "All", int maxRows = 500)
        {
            var list = new List<WebsitePageItemDto>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureTablesExist();
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (var conn = new SqlConnection(connStr))
                    {
                        var sb = new StringBuilder();
                        sb.Append("SELECT TOP (@MaxRows) PageId, PageTitle, PageURL, PageLayout, UserControlPath, UserControlProperties, WebUrl, " +
                                  "PageTitleEn, PageURLEn, PageLayoutEn, UserControlPathEn, UserControlPropertiesEn, WebUrlEn, EnExists, LastIndexed " +
                                  "FROM dbo.WebsitePages WHERE 1=1 ");

                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Parameters.AddWithValue("@MaxRows", maxRows <= 0 ? 1000 : maxRows);

                            if (!string.IsNullOrWhiteSpace(layoutFilter) && layoutFilter != "All")
                            {
                                if (layoutFilter == "[بدون تخطيط محدد / افتراضي]")
                                {
                                    sb.Append("AND (PageLayout IS NULL OR LTRIM(RTRIM(PageLayout)) = '') ");
                                }
                                else
                                {
                                    sb.Append("AND PageLayout = @Layout ");
                                    cmd.Parameters.AddWithValue("@Layout", layoutFilter);
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(subsiteFilter))
                            {
                                sb.Append("AND (WebUrl LIKE @Subsite OR WebUrlEn LIKE @Subsite OR PageURL LIKE @Subsite) ");
                                cmd.Parameters.AddWithValue("@Subsite", "%" + subsiteFilter.Trim() + "%");
                            }

                            if (!string.IsNullOrWhiteSpace(keyword))
                            {
                                sb.Append("AND (PageTitle LIKE @Kw OR PageTitleEn LIKE @Kw OR PageURL LIKE @Kw OR PageURLEn LIKE @Kw OR UserControlPath LIKE @Kw) ");
                                cmd.Parameters.AddWithValue("@Kw", "%" + keyword.Trim() + "%");
                            }

                            if (enStatusFilter == "Exists")
                            {
                                sb.Append("AND EnExists = 1 ");
                            }
                            else if (enStatusFilter == "Missing")
                            {
                                sb.Append("AND EnExists = 0 ");
                            }

                            sb.Append("ORDER BY WebUrl, PageTitle");
                            cmd.CommandText = sb.ToString();

                            conn.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    string layout = rdr["PageLayout"] as string;
                                    string fileName = layout;
                                    if (!string.IsNullOrEmpty(layout))
                                    {
                                        int slash = layout.LastIndexOf('/');
                                        if (slash >= 0) fileName = layout.Substring(slash + 1);
                                    }

                                    bool isDga = IsDgaLayout(layout);

                                    list.Add(new WebsitePageItemDto
                                    {
                                        PageId = Convert.ToInt64(rdr["PageId"]),
                                        PageTitle = rdr["PageTitle"] as string,
                                        PageURL = rdr["PageURL"] as string,
                                        PageLayout = layout,
                                        PageLayoutFileName = fileName,
                                        UserControlPath = rdr["UserControlPath"] as string,
                                        UserControlProperties = rdr["UserControlProperties"] as string,
                                        WebUrl = rdr["WebUrl"] as string,
                                        PageTitleEn = rdr["PageTitleEn"] as string,
                                        PageURLEn = rdr["PageURLEn"] as string,
                                        PageLayoutEn = rdr["PageLayoutEn"] as string,
                                        UserControlPathEn = rdr["UserControlPathEn"] as string,
                                        UserControlPropertiesEn = rdr["UserControlPropertiesEn"] as string,
                                        WebUrlEn = rdr["WebUrlEn"] as string,
                                        EnExists = Convert.ToBoolean(rdr["EnExists"]),
                                        IsDgaCompliant = isDga,
                                        LastIndexed = Convert.ToDateTime(rdr["LastIndexed"])
                                    });
                                }
                            }
                        }
                    }
                }
                catch { }
            });

            return list;
        }

        #endregion

        #region Missing Pages Report

        public static List<MissingPageReportDto> GetMissingPagesReport(string statusFilter = "All", string directionFilter = "All", string subsiteFilter = null, string keyword = null, int maxRows = 500)
        {
            var list = new List<MissingPageReportDto>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureTablesExist();
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (var conn = new SqlConnection(connStr))
                    {
                        var sb = new StringBuilder();
                        sb.Append("SELECT TOP (@MaxRows) Id, PageFileName, PageTitleAr, PageTitleEn, SourcePageUrl, SourcePageFullUrl, " +
                                  "TargetPageUrl, TargetPageFullUrl, SourceWebUrl, SourceWebFullUrl, TargetWebUrl, TargetWebFullUrl, " +
                                  "PageLayout, PageLayoutFileName, HasControlLoader, UserControlPath, UserControlProperties, " +
                                  "HasContentEditor, HasPageContent, Direction, IsCreated, CreatedDate, Status, LastDiscoveredDate, LastUpdatedDate " +
                                  "FROM dbo.MissingPagesOnEN WHERE 1=1 ");

                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Parameters.AddWithValue("@MaxRows", maxRows <= 0 ? 1000 : maxRows);

                            if (statusFilter == "Pending" || statusFilter == "Missing")
                            {
                                sb.Append("AND Status = 'Missing' ");
                            }
                            else if (statusFilter == "Created")
                            {
                                sb.Append("AND (IsCreated = 1 OR Status = 'Created' OR Status = 'Resolved') ");
                            }

                            if (!string.IsNullOrWhiteSpace(directionFilter) && directionFilter != "All")
                            {
                                sb.Append("AND Direction = @Dir ");
                                cmd.Parameters.AddWithValue("@Dir", directionFilter);
                            }

                            if (!string.IsNullOrWhiteSpace(subsiteFilter))
                            {
                                sb.Append("AND (SourceWebUrl LIKE @Subsite OR TargetWebUrl LIKE @Subsite OR SourceWebFullUrl LIKE @Subsite OR TargetWebFullUrl LIKE @Subsite) ");
                                cmd.Parameters.AddWithValue("@Subsite", "%" + subsiteFilter.Trim() + "%");
                            }

                            if (!string.IsNullOrWhiteSpace(keyword))
                            {
                                sb.Append("AND (PageFileName LIKE @Kw OR PageTitleAr LIKE @Kw OR PageTitleEn LIKE @Kw OR TargetPageUrl LIKE @Kw OR SourcePageUrl LIKE @Kw OR UserControlPath LIKE @Kw) ");
                                cmd.Parameters.AddWithValue("@Kw", "%" + keyword.Trim() + "%");
                            }

                            sb.Append("ORDER BY TargetWebUrl, PageFileName");
                            cmd.CommandText = sb.ToString();

                            conn.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    list.Add(new MissingPageReportDto
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
                                        Status = rdr["Status"] as string,
                                        LastDiscoveredDate = Convert.ToDateTime(rdr["LastDiscoveredDate"]),
                                        LastUpdatedDate = Convert.ToDateTime(rdr["LastUpdatedDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                catch { }
            });

            return list;
        }

        public static bool MarkMissingPageResolved(long id, string targetUrl)
        {
            bool ok = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = new SqlCommand(
                        "UPDATE dbo.MissingPagesOnEN SET IsCreated = 1, Status = 'Created', CreatedDate = GETDATE(), LastUpdatedDate = GETDATE() " +
                        "WHERE Id = @Id OR TargetPageUrl = @Url OR TargetPageFullUrl = @Url", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Url", (object)targetUrl ?? DBNull.Value);
                        conn.Open();
                        ok = cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch { }
            });
            return ok;
        }

        #endregion

        #region User Controls Inventory Report

        public static List<UserControlSummaryDto> GetUserControlsSummary()
        {
            var list = new List<UserControlSummaryDto>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureTablesExist();
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        const string sql = @"
SELECT 
    UserControlPath,
    COUNT(*) AS PageCount,
    SUM(CASE WHEN PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%' THEN 1 ELSE 0 END) AS ArCount,
    SUM(CASE WHEN PageURL LIKE '%/en/%' THEN 1 ELSE 0 END) AS EnCount,
    MIN(PageURL) AS SampleUrl,
    MIN(PageTitle) AS SampleTitle
FROM dbo.WebsitePages
WHERE UserControlPath IS NOT NULL AND LEN(LTRIM(RTRIM(UserControlPath))) > 0
GROUP BY UserControlPath
ORDER BY PageCount DESC";

                        using (var cmd = new SqlCommand(sql, conn))
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                string uc = rdr["UserControlPath"].ToString();
                                string fileName = uc;
                                int slash = uc.LastIndexOf('/');
                                if (slash >= 0) fileName = uc.Substring(slash + 1);

                                list.Add(new UserControlSummaryDto
                                {
                                    UserControlPath = uc,
                                    ControlFileName = fileName,
                                    PageCount = Convert.ToInt32(rdr["PageCount"]),
                                    ArPagesCount = Convert.ToInt32(rdr["ArCount"]),
                                    EnPagesCount = Convert.ToInt32(rdr["EnCount"]),
                                    SamplePageUrl = rdr["SampleUrl"] as string,
                                    SamplePageTitle = rdr["SampleTitle"] as string
                                });
                            }
                        }
                    }
                }
                catch { }
            });

            return list;
        }

        #endregion

        #region Subsite Health Breakdown

        public static List<SubsiteHealthDto> GetSubsitesSummary()
        {
            var list = new List<SubsiteHealthDto>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureTablesExist();
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        const string sql = @"
SELECT 
    ISNULL(NULLIF(WebUrl, ''), '[Root Web]') AS WebUrl,
    COUNT(*) AS TotalPages,
    ISNULL(SUM(CASE WHEN PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%' THEN 1 ELSE 0 END), 0) AS ArabicPages,
    ISNULL(SUM(CASE WHEN (PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%') AND EnExists = 1 THEN 1 ELSE 0 END), 0) AS HasEnCount,
    ISNULL(SUM(CASE WHEN (PageURL LIKE '%/ar/%' OR PageURL NOT LIKE '%/en/%') AND EnExists = 0 THEN 1 ELSE 0 END), 0) AS MissingEnCount,
    ISNULL(SUM(CASE WHEN PageLayout LIKE '%DGA%' OR PageLayout LIKE '%NewSideMenu%' OR PageLayoutEn LIKE '%DGA%' OR PageLayoutEn LIKE '%NewSideMenu%' THEN 1 ELSE 0 END), 0) AS DgaCount
FROM dbo.WebsitePages
GROUP BY ISNULL(NULLIF(WebUrl, ''), '[Root Web]')
ORDER BY TotalPages DESC";

                        using (var cmd = new SqlCommand(sql, conn))
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int total = Convert.ToInt32(rdr["TotalPages"]);
                                int arPages = Convert.ToInt32(rdr["ArabicPages"]);
                                int hasEn = Convert.ToInt32(rdr["HasEnCount"]);
                                int dga = Convert.ToInt32(rdr["DgaCount"]);

                                double parity = arPages > 0 ? Math.Min(100.0, Math.Round((hasEn * 100.0) / arPages, 1)) : (total > 0 && hasEn > 0 ? 100.0 : 0);
                                double dgaPct = total > 0 ? Math.Min(100.0, Math.Round((dga * 100.0) / total, 1)) : 0;

                                list.Add(new SubsiteHealthDto
                                {
                                    WebUrl = rdr["WebUrl"].ToString(),
                                    TotalPages = total,
                                    HasEnglishCount = hasEn,
                                    MissingEnglishCount = Convert.ToInt32(rdr["MissingEnCount"]),
                                    DgaCompliantCount = dga,
                                    ParityPercentage = parity,
                                    DgaPercentage = dgaPct
                                });
                            }
                        }
                    }
                }
                catch { }
            });

            return list;
        }

        #endregion

        #region Live Crawling & Upsert

        public static void UpsertPageRecord(WebsitePageItemDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.PageURL)) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    const string sql = @"
MERGE dbo.WebsitePages AS T
USING (SELECT @PageURL AS PageURL) AS S ON T.PageURL = S.PageURL
WHEN MATCHED THEN
    UPDATE SET
        PageTitle               = @PageTitle,
        PageLayout              = @PageLayout,
        UserControlPath         = @UserControlPath,
        UserControlProperties   = @UserControlProperties,
        WebUrl                  = @WebUrl,
        PageTitleEn             = @PageTitleEn,
        PageURLEn               = @PageURLEn,
        PageLayoutEn            = @PageLayoutEn,
        UserControlPathEn       = @UserControlPathEn,
        UserControlPropertiesEn = @UserControlPropertiesEn,
        WebUrlEn                = @WebUrlEn,
        EnExists                = @EnExists,
        LastIndexed             = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (PageTitle, PageURL, PageLayout, UserControlPath,
            UserControlProperties, WebUrl,
            PageTitleEn, PageURLEn, PageLayoutEn, UserControlPathEn,
            UserControlPropertiesEn, WebUrlEn, EnExists, LastIndexed)
    VALUES (@PageTitle, @PageURL, @PageLayout, @UserControlPath,
            @UserControlProperties, @WebUrl,
            @PageTitleEn, @PageURLEn, @PageLayoutEn, @UserControlPathEn,
            @UserControlPropertiesEn, @WebUrlEn, @EnExists, GETDATE());";

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@PageTitle", (object)dto.PageTitle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageURL", dto.PageURL);
                        cmd.Parameters.AddWithValue("@PageLayout", (object)dto.PageLayout ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlPath", (object)dto.UserControlPath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlProperties", (object)dto.UserControlProperties ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@WebUrl", (object)dto.WebUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageTitleEn", (object)dto.PageTitleEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageURLEn", (object)dto.PageURLEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageLayoutEn", (object)dto.PageLayoutEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlPathEn", (object)dto.UserControlPathEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlPropertiesEn", (object)dto.UserControlPropertiesEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@WebUrlEn", (object)dto.WebUrlEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EnExists", dto.EnExists);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }
            });
        }

        public static void UpdatePageLayout(string pageUrl, string newLayout)
        {
            if (string.IsNullOrEmpty(pageUrl) || string.IsNullOrEmpty(newLayout)) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    bool isEn = pageUrl.IndexOf("/en/", StringComparison.OrdinalIgnoreCase) >= 0;

                    string sql = isEn
                        ? @"UPDATE dbo.WebsitePages 
                            SET PageLayoutEn = @NewLayout, LastIndexed = GETDATE() 
                            WHERE PageURLEn = @PageUrl OR PageURL = @PageUrl"
                        : @"UPDATE dbo.WebsitePages 
                            SET PageLayout = @NewLayout, LastIndexed = GETDATE() 
                            WHERE PageURL = @PageUrl OR PageURLEn = @PageUrl";

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@NewLayout", newLayout);
                        cmd.Parameters.AddWithValue("@PageUrl", pageUrl);
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();

                        if (rows == 0)
                        {
                            try
                            {
                                var uri = new Uri(pageUrl);
                                string path = uri.AbsolutePath;
                                string fallbackSql = isEn
                                    ? @"UPDATE dbo.WebsitePages 
                                        SET PageLayoutEn = @NewLayout, LastIndexed = GETDATE() 
                                        WHERE PageURLEn LIKE '%' + @Path OR PageURL LIKE '%' + @Path"
                                    : @"UPDATE dbo.WebsitePages 
                                        SET PageLayout = @NewLayout, LastIndexed = GETDATE() 
                                        WHERE PageURL LIKE '%' + @Path OR PageURLEn LIKE '%' + @Path";

                                using (var fbCmd = new SqlCommand(fallbackSql, conn))
                                {
                                    fbCmd.Parameters.AddWithValue("@NewLayout", newLayout);
                                    fbCmd.Parameters.AddWithValue("@Path", path);
                                    fbCmd.ExecuteNonQuery();
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            });
        }

        public static void UpsertMissingPageRecord(MissingPageReportDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.TargetPageUrl)) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    const string sql = @"
MERGE dbo.MissingPagesOnEN AS T
USING (SELECT @TargetPageUrl AS TargetPageUrl) AS S ON T.TargetPageUrl = S.TargetPageUrl
WHEN MATCHED THEN
    UPDATE SET
        PageFileName           = @PageFileName,
        PageTitleAr            = @PageTitleAr,
        PageTitleEn            = @PageTitleEn,
        SourcePageUrl          = @SourcePageUrl,
        SourcePageFullUrl      = @SourcePageFullUrl,
        TargetPageFullUrl      = @TargetPageFullUrl,
        SourceWebUrl           = @SourceWebUrl,
        SourceWebFullUrl       = @SourceWebFullUrl,
        TargetWebUrl           = @TargetWebUrl,
        TargetWebFullUrl       = @TargetWebFullUrl,
        PageLayout             = @PageLayout,
        PageLayoutFileName     = @PageLayoutFileName,
        HasControlLoader       = @HasControlLoader,
        UserControlPath        = @UserControlPath,
        UserControlProperties  = @UserControlProperties,
        HasContentEditor       = @HasContentEditor,
        HasPageContent         = @HasPageContent,
        Direction              = @Direction,
        IsCreated              = @IsCreated,
        CreatedDate            = CASE WHEN @IsCreated = 1 AND T.CreatedDate IS NULL THEN ISNULL(@CreatedDate, GETDATE()) ELSE T.CreatedDate END,
        Status                 = @Status,
        LastDiscoveredDate     = GETDATE(),
        LastUpdatedDate        = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (PageFileName, PageTitleAr, PageTitleEn, SourcePageUrl, SourcePageFullUrl,
            TargetPageUrl, TargetPageFullUrl, SourceWebUrl, SourceWebFullUrl,
            TargetWebUrl, TargetWebFullUrl, PageLayout, PageLayoutFileName,
            HasControlLoader, UserControlPath, UserControlProperties,
            HasContentEditor, HasPageContent, Direction, IsCreated, CreatedDate,
            Status, LastDiscoveredDate, LastUpdatedDate)
    VALUES (@PageFileName, @PageTitleAr, @PageTitleEn, @SourcePageUrl, @SourcePageFullUrl,
            @TargetPageUrl, @TargetPageFullUrl, @SourceWebUrl, @SourceWebFullUrl,
            @TargetWebUrl, @TargetWebFullUrl, @PageLayout, @PageLayoutFileName,
            @HasControlLoader, @UserControlPath, @UserControlProperties,
            @HasContentEditor, @HasPageContent, @Direction, @IsCreated, @CreatedDate,
            @Status, GETDATE(), GETDATE());";

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@PageFileName", (object)dto.PageFileName ?? "page.aspx");
                        cmd.Parameters.AddWithValue("@PageTitleAr", (object)dto.PageTitleAr ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageTitleEn", (object)dto.PageTitleEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SourcePageUrl", (object)dto.SourcePageUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SourcePageFullUrl", (object)dto.SourcePageFullUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TargetPageUrl", dto.TargetPageUrl);
                        cmd.Parameters.AddWithValue("@TargetPageFullUrl", (object)dto.TargetPageFullUrl ?? dto.TargetPageUrl);
                        cmd.Parameters.AddWithValue("@SourceWebUrl", (object)dto.SourceWebUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SourceWebFullUrl", (object)dto.SourceWebFullUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TargetWebUrl", (object)dto.TargetWebUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TargetWebFullUrl", (object)dto.TargetWebFullUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageLayout", (object)dto.PageLayout ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageLayoutFileName", (object)dto.PageLayoutFileName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HasControlLoader", dto.HasControlLoader);
                        cmd.Parameters.AddWithValue("@UserControlPath", (object)dto.UserControlPath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlProperties", (object)dto.UserControlProperties ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HasContentEditor", dto.HasContentEditor);
                        cmd.Parameters.AddWithValue("@HasPageContent", dto.HasPageContent);
                        cmd.Parameters.AddWithValue("@Direction", (object)dto.Direction ?? "ArToEn");
                        cmd.Parameters.AddWithValue("@IsCreated", dto.IsCreated);
                        cmd.Parameters.AddWithValue("@CreatedDate", (object)dto.CreatedDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", (object)dto.Status ?? "Missing");

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }
            });
        }

        #endregion
    }
}
