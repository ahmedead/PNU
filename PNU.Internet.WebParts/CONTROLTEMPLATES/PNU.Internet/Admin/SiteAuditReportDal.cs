using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    #region DTO Models

    [Serializable]
    public class AuditScanSummaryDto
    {
        public long ScanId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TotalPagesScanned { get; set; }
        public int TotalIssuesFound { get; set; }
        public int CriticalCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public int PassedCount { get; set; }
        public double HealthScorePercentage { get; set; }
        public string Status { get; set; } // Running, Completed, Failed, Cancelled
        public string ScannedBy { get; set; }
        public string TargetScope { get; set; } // "All", "Specific Subsite", etc.
    }

    [Serializable]
    public class AuditIssueDto
    {
        public long IssueId { get; set; }
        public long ScanId { get; set; }
        public int IssueCode { get; set; } // 1 to 74
        public string IssueCategory { get; set; } // SEO, Security, Performance, Accessibility, URL & 404, Structure
        public string IssueName { get; set; }
        public string Severity { get; set; } // Critical, Warning, Info
        public string PageUrl { get; set; }
        public string SubsiteUrl { get; set; }
        public string PageTitle { get; set; }
        public string Details { get; set; }
        public string RecommendedFix { get; set; }
        public string ElementSnippet { get; set; }
        public DateTime DiscoveredDate { get; set; }
        public string Status { get; set; } // Open, Resolved, Ignored
    }

    [Serializable]
    public class CategoryIssueCountDto
    {
        public string Category { get; set; }
        public int CriticalCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public int TotalCount { get; set; }
        public double HealthPercentage { get; set; }
    }

    #endregion

    public static class SiteAuditReportDal
    {
        private static string _connectionString = null;
        private static readonly object _lockObj = new object();

        /// <summary>
        /// Last error raised by a DAL call. Kept for diagnostics; the calls themselves
        /// rethrow so the UI can surface the real reason instead of showing empty data.
        /// </summary>
        public static string LastError { get; private set; }

        internal const string NoConnectionMessage =
            "تعذر تحديد سلسلة الاتصال بقاعدة البيانات. أضف عنصراً باسم 'PNU_SearchIndex' أو 'New_PNU_Portal_Cust_DB' في قائمة ConnectionStrings داخل موقع Admin، أو عرّف ConnectionString بنفس الاسم في web.config.";

        /// <summary>
        /// Connection names probed, in order, before falling back to web.config.
        /// </summary>
        private static readonly string[] CandidateConnectionNames =
        {
            "PNU_SearchIndex",
            "New_PNU_Portal_Cust_DB",
            "Portal_Admin",
            "MainPortal"
        };

        public static string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_connectionString))
                    return _connectionString;

                lock (_lockObj)
                {
                    if (!string.IsNullOrEmpty(_connectionString))
                        return _connectionString;

                    string connection = string.Empty;

                    // 1. Try SQLFactory standard helper (reads the Admin > ConnectionStrings list)
                    foreach (string name in CandidateConnectionNames)
                    {
                        try
                        {
                            connection = SQLFactory.GetConnectionString(name);
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(connection)) break;
                    }

                    if (string.IsNullOrEmpty(connection))
                    {
                        try
                        {
                            connection = SQLFactory.connectionString;
                        }
                        catch { }
                    }

                    // 2. Fallback to web.config connectionStrings
                    if (string.IsNullOrEmpty(connection))
                    {
                        try
                        {
                            foreach (string name in CandidateConnectionNames)
                            {
                                var cs = ConfigurationManager.ConnectionStrings[name];
                                if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
                                {
                                    connection = cs.ConnectionString;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(connection))
                            {
                                var csConn = ConfigurationManager.ConnectionStrings["ConnString"];
                                if (csConn != null && !string.IsNullOrEmpty(csConn.ConnectionString))
                                    connection = csConn.ConnectionString;
                            }
                        }
                        catch { }
                    }

                    _connectionString = connection;
                    return _connectionString;
                }
            }
        }

        public static void EnsureTablesExist()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    const string ddl = @"
IF OBJECT_ID('dbo.SiteAuditScans', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SiteAuditScans
    (
        ScanId                 BIGINT IDENTITY(1,1) PRIMARY KEY,
        StartTime              DATETIME        NOT NULL DEFAULT(GETDATE()),
        EndTime                DATETIME        NULL,
        TotalPagesScanned      INT             NOT NULL DEFAULT(0),
        TotalIssuesFound       INT             NOT NULL DEFAULT(0),
        CriticalCount          INT             NOT NULL DEFAULT(0),
        WarningCount           INT             NOT NULL DEFAULT(0),
        InfoCount              INT             NOT NULL DEFAULT(0),
        PassedCount            INT             NOT NULL DEFAULT(0),
        HealthScorePercentage  FLOAT           NOT NULL DEFAULT(100.0),
        Status                 NVARCHAR(50)    NOT NULL DEFAULT('Running'),
        ScannedBy              NVARCHAR(200)   NULL,
        TargetScope            NVARCHAR(500)   NULL
    );
END;

IF OBJECT_ID('dbo.SiteAuditIssues', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SiteAuditIssues
    (
        IssueId                BIGINT IDENTITY(1,1) PRIMARY KEY,
        ScanId                 BIGINT          NOT NULL,
        IssueCode              INT             NOT NULL,
        IssueCategory          NVARCHAR(100)   NOT NULL,
        IssueName              NVARCHAR(250)   NOT NULL,
        Severity               NVARCHAR(50)    NOT NULL,
        PageUrl                NVARCHAR(1000)  NOT NULL,
        SubsiteUrl             NVARCHAR(1000)  NULL,
        PageTitle              NVARCHAR(500)   NULL,
        Details                NVARCHAR(MAX)   NULL,
        RecommendedFix         NVARCHAR(MAX)   NULL,
        ElementSnippet         NVARCHAR(MAX)   NULL,
        DiscoveredDate         DATETIME        NOT NULL DEFAULT(GETDATE()),
        Status                 NVARCHAR(50)    NOT NULL DEFAULT('Open')
    );
    CREATE INDEX IX_SiteAuditIssues_ScanId ON dbo.SiteAuditIssues(ScanId);
    CREATE INDEX IX_SiteAuditIssues_IssueCode ON dbo.SiteAuditIssues(IssueCode);
    CREATE INDEX IX_SiteAuditIssues_Severity ON dbo.SiteAuditIssues(Severity);
    CREATE INDEX IX_SiteAuditIssues_Category ON dbo.SiteAuditIssues(IssueCategory);
    CREATE INDEX IX_SiteAuditIssues_PageUrl ON dbo.SiteAuditIssues(PageUrl);
END;

IF OBJECT_ID('dbo.SiteAuditPageSnapshots', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SiteAuditPageSnapshots
    (
        SnapshotId             BIGINT IDENTITY(1,1) PRIMARY KEY,
        ScanId                 BIGINT          NOT NULL,
        PageUrl                NVARCHAR(1000)  NOT NULL,
        SubsiteUrl             NVARCHAR(1000)  NULL,
        PageTitle              NVARCHAR(500)   NULL,
        PageLayout             NVARCHAR(500)   NULL,
        ResponseCode           INT             NOT NULL DEFAULT(200),
        CanonicalUrl           NVARCHAR(1000)  NULL,
        Description            NVARCHAR(MAX)   NULL,
        H1Tag                  NVARCHAR(500)   NULL,
        HreflangCount          INT             NOT NULL DEFAULT(0),
        InsecureLinksCount     INT             NOT NULL DEFAULT(0),
        MissingAltImagesCount  INT             NOT NULL DEFAULT(0),
        TotalIssuesOnPage      INT             NOT NULL DEFAULT(0),
        LastAudited            DATETIME        NOT NULL DEFAULT(GETDATE())
    );
    CREATE INDEX IX_SiteAuditSnapshots_ScanId ON dbo.SiteAuditPageSnapshots(ScanId);
END;
";
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(ddl, conn) { CommandTimeout = 120 })
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });
        }

        public static long CreateScanSession(string scannedBy, string scope)
        {
            long scanId = 0;
            EnsureTablesExist();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string sql = @"
INSERT INTO dbo.SiteAuditScans (StartTime, Status, ScannedBy, TargetScope)
VALUES (GETDATE(), 'Running', @ScannedBy, @TargetScope);
SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ScannedBy", (object)scannedBy ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@TargetScope", (object)scope ?? "All");
                            object res = cmd.ExecuteScalar();
                            if (res != null && res != DBNull.Value)
                            {
                                scanId = Convert.ToInt64(res);
                            }
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });

            return scanId;
        }

        public static void CompleteScanSession(long scanId, int totalPages, int critical, int warnings, int info, int passed)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    int totalIssues = critical + warnings + info;
                    int totalChecks = totalIssues + passed;
                    double healthScore = totalChecks > 0
                        ? Math.Round(((double)passed / (double)totalChecks) * 100.0, 1)
                        : 100.0;

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string sql = @"
UPDATE dbo.SiteAuditScans
SET EndTime = GETDATE(),
    TotalPagesScanned = @TotalPages,
    TotalIssuesFound = @TotalIssues,
    CriticalCount = @Critical,
    WarningCount = @Warnings,
    InfoCount = @Info,
    PassedCount = @Passed,
    HealthScorePercentage = @HealthScore,
    Status = 'Completed'
WHERE ScanId = @ScanId;";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ScanId", scanId);
                            cmd.Parameters.AddWithValue("@TotalPages", totalPages);
                            cmd.Parameters.AddWithValue("@TotalIssues", totalIssues);
                            cmd.Parameters.AddWithValue("@Critical", critical);
                            cmd.Parameters.AddWithValue("@Warnings", warnings);
                            cmd.Parameters.AddWithValue("@Info", info);
                            cmd.Parameters.AddWithValue("@Passed", passed);
                            cmd.Parameters.AddWithValue("@HealthScore", healthScore);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });
        }

        public static void BatchInsertIssues(long scanId, List<AuditIssueDto> issues)
        {
            if (issues == null || issues.Count == 0) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ScanId", typeof(long));
                    dt.Columns.Add("IssueCode", typeof(int));
                    dt.Columns.Add("IssueCategory", typeof(string));
                    dt.Columns.Add("IssueName", typeof(string));
                    dt.Columns.Add("Severity", typeof(string));
                    dt.Columns.Add("PageUrl", typeof(string));
                    dt.Columns.Add("SubsiteUrl", typeof(string));
                    dt.Columns.Add("PageTitle", typeof(string));
                    dt.Columns.Add("Details", typeof(string));
                    dt.Columns.Add("RecommendedFix", typeof(string));
                    dt.Columns.Add("ElementSnippet", typeof(string));
                    dt.Columns.Add("DiscoveredDate", typeof(DateTime));
                    dt.Columns.Add("Status", typeof(string));

                    DateTime now = DateTime.Now;
                    foreach (var issue in issues)
                    {
                        dt.Rows.Add(
                            scanId,
                            issue.IssueCode,
                            issue.IssueCategory ?? "General",
                            issue.IssueName ?? string.Empty,
                            issue.Severity ?? "Warning",
                            issue.PageUrl ?? string.Empty,
                            issue.SubsiteUrl ?? string.Empty,
                            issue.PageTitle ?? string.Empty,
                            issue.Details ?? string.Empty,
                            issue.RecommendedFix ?? string.Empty,
                            issue.ElementSnippet ?? string.Empty,
                            now,
                            "Open"
                        );
                    }

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        using (SqlBulkCopy bulk = new SqlBulkCopy(conn) { BulkCopyTimeout = 300 })
                        {
                            bulk.DestinationTableName = "dbo.SiteAuditIssues";
                            bulk.ColumnMappings.Add("ScanId", "ScanId");
                            bulk.ColumnMappings.Add("IssueCode", "IssueCode");
                            bulk.ColumnMappings.Add("IssueCategory", "IssueCategory");
                            bulk.ColumnMappings.Add("IssueName", "IssueName");
                            bulk.ColumnMappings.Add("Severity", "Severity");
                            bulk.ColumnMappings.Add("PageUrl", "PageUrl");
                            bulk.ColumnMappings.Add("SubsiteUrl", "SubsiteUrl");
                            bulk.ColumnMappings.Add("PageTitle", "PageTitle");
                            bulk.ColumnMappings.Add("Details", "Details");
                            bulk.ColumnMappings.Add("RecommendedFix", "RecommendedFix");
                            bulk.ColumnMappings.Add("ElementSnippet", "ElementSnippet");
                            bulk.ColumnMappings.Add("DiscoveredDate", "DiscoveredDate");
                            bulk.ColumnMappings.Add("Status", "Status");

                            bulk.WriteToServer(dt);
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });
        }

        public static AuditScanSummaryDto GetLatestScanSummary()
        {
            return GetScanSummary(0);
        }

        /// <summary>
        /// Returns the summary for <paramref name="scanId"/>, or the most recent scan when it is 0.
        /// </summary>
        public static AuditScanSummaryDto GetScanSummary(long scanId)
        {
            AuditScanSummaryDto summary = null;
            EnsureTablesExist();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string sql = @"
SELECT TOP 1 ScanId, StartTime, EndTime, TotalPagesScanned, TotalIssuesFound,
             CriticalCount, WarningCount, InfoCount, PassedCount, HealthScorePercentage,
             Status, ScannedBy, TargetScope
FROM dbo.SiteAuditScans
WHERE (@ScanId = 0 OR ScanId = @ScanId)
ORDER BY ScanId DESC;";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ScanId", scanId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    summary = new AuditScanSummaryDto
                                    {
                                        ScanId = reader.GetInt64(0),
                                        StartTime = reader.GetDateTime(1),
                                        EndTime = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                                        TotalPagesScanned = reader.GetInt32(3),
                                        TotalIssuesFound = reader.GetInt32(4),
                                        CriticalCount = reader.GetInt32(5),
                                        WarningCount = reader.GetInt32(6),
                                        InfoCount = reader.GetInt32(7),
                                        PassedCount = reader.GetInt32(8),
                                        HealthScorePercentage = reader.GetDouble(9),
                                        Status = reader.GetString(10),
                                        ScannedBy = reader.IsDBNull(11) ? "" : reader.GetString(11),
                                        TargetScope = reader.IsDBNull(12) ? "All" : reader.GetString(12)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });

            return summary;
        }

        public static List<CategoryIssueCountDto> GetCategoryBreakdown(long scanId)
        {
            List<CategoryIssueCountDto> list = new List<CategoryIssueCountDto>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string sql = @"
SELECT IssueCategory,
       SUM(CASE WHEN Severity = 'Critical' THEN 1 ELSE 0 END) AS CriticalCount,
       SUM(CASE WHEN Severity = 'Warning' THEN 1 ELSE 0 END) AS WarningCount,
       SUM(CASE WHEN Severity = 'Info' THEN 1 ELSE 0 END) AS InfoCount,
       COUNT(*) AS TotalCount
FROM dbo.SiteAuditIssues
WHERE ScanId = @ScanId
GROUP BY IssueCategory
ORDER BY TotalCount DESC;";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ScanId", scanId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    list.Add(new CategoryIssueCountDto
                                    {
                                        Category = reader.GetString(0),
                                        CriticalCount = reader.GetInt32(1),
                                        WarningCount = reader.GetInt32(2),
                                        InfoCount = reader.GetInt32(3),
                                        TotalCount = reader.GetInt32(4)
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });

            return list;
        }

        public static List<AuditIssueDto> GetFilteredIssues(long scanId, string category, string severity, int? issueCode, string searchTerm, int pageIndex, int pageSize, out int totalCount)
        {
            List<AuditIssueDto> list = new List<AuditIssueDto>();
            int count = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        StringBuilder whereClause = new StringBuilder(" WHERE ScanId = @ScanId");
                        if (!string.IsNullOrEmpty(category) && !category.Equals("All", StringComparison.OrdinalIgnoreCase))
                        {
                            whereClause.Append(" AND IssueCategory = @Category");
                        }
                        if (!string.IsNullOrEmpty(severity) && !severity.Equals("All", StringComparison.OrdinalIgnoreCase))
                        {
                            whereClause.Append(" AND Severity = @Severity");
                        }
                        if (issueCode.HasValue && issueCode.Value > 0)
                        {
                            whereClause.Append(" AND IssueCode = @IssueCode");
                        }
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            whereClause.Append(" AND (PageUrl LIKE @Search OR PageTitle LIKE @Search OR IssueName LIKE @Search OR Details LIKE @Search)");
                        }

                        // Count query
                        string countSql = "SELECT COUNT(*) FROM dbo.SiteAuditIssues" + whereClause.ToString();
                        using (SqlCommand cmdCount = new SqlCommand(countSql, conn))
                        {
                            cmdCount.Parameters.AddWithValue("@ScanId", scanId);
                            if (whereClause.ToString().Contains("@Category")) cmdCount.Parameters.AddWithValue("@Category", category);
                            if (whereClause.ToString().Contains("@Severity")) cmdCount.Parameters.AddWithValue("@Severity", severity);
                            if (whereClause.ToString().Contains("@IssueCode")) cmdCount.Parameters.AddWithValue("@IssueCode", issueCode.Value);
                            if (whereClause.ToString().Contains("@Search")) cmdCount.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");
                            count = Convert.ToInt32(cmdCount.ExecuteScalar());
                        }

                        // Paged query
                        int offset = pageIndex * pageSize;
                        string pagedSql = $@"
SELECT IssueId, ScanId, IssueCode, IssueCategory, IssueName, Severity,
       PageUrl, SubsiteUrl, PageTitle, Details, RecommendedFix, ElementSnippet, DiscoveredDate, Status
FROM dbo.SiteAuditIssues
{whereClause}
ORDER BY 
    CASE Severity WHEN 'Critical' THEN 1 WHEN 'Warning' THEN 2 ELSE 3 END,
    IssueCode ASC,
    IssueId ASC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

                        using (SqlCommand cmd = new SqlCommand(pagedSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ScanId", scanId);
                            cmd.Parameters.AddWithValue("@Offset", offset);
                            cmd.Parameters.AddWithValue("@PageSize", pageSize);
                            if (whereClause.ToString().Contains("@Category")) cmd.Parameters.AddWithValue("@Category", category);
                            if (whereClause.ToString().Contains("@Severity")) cmd.Parameters.AddWithValue("@Severity", severity);
                            if (whereClause.ToString().Contains("@IssueCode")) cmd.Parameters.AddWithValue("@IssueCode", issueCode.Value);
                            if (whereClause.ToString().Contains("@Search")) cmd.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    list.Add(new AuditIssueDto
                                    {
                                        IssueId = reader.GetInt64(0),
                                        ScanId = reader.GetInt64(1),
                                        IssueCode = reader.GetInt32(2),
                                        IssueCategory = reader.GetString(3),
                                        IssueName = reader.GetString(4),
                                        Severity = reader.GetString(5),
                                        PageUrl = reader.GetString(6),
                                        SubsiteUrl = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                        PageTitle = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                        Details = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                        RecommendedFix = reader.IsDBNull(10) ? "" : reader.GetString(10),
                                        ElementSnippet = reader.IsDBNull(11) ? "" : reader.GetString(11),
                                        DiscoveredDate = reader.GetDateTime(12),
                                        Status = reader.GetString(13)
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });

            totalCount = count;
            return list;
        }

        public static DataTable ExportIssuesToDataTable(long scanId)
        {
            DataTable dt = new DataTable("SiteAuditIssues");
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string sql = @"
SELECT IssueId AS [Issue #],
       IssueCode AS [Code],
       IssueCategory AS [Category],
       Severity,
       IssueName AS [Issue Name],
       PageTitle AS [Page Title],
       PageUrl AS [Page URL],
       SubsiteUrl AS [Subsite],
       Details,
       RecommendedFix AS [Recommended Action],
       Status,
       DiscoveredDate AS [Discovered Date]
FROM dbo.SiteAuditIssues
WHERE ScanId = @ScanId
ORDER BY CASE Severity WHEN 'Critical' THEN 1 WHEN 'Warning' THEN 2 ELSE 3 END, IssueCode ASC;";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ScanId", scanId);
                            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(dt);
                            }
                        }
                    }
                }
                catch (Exception ex) { LastError = ex.Message; throw; }
            });

            return dt;
        }
    }
}
