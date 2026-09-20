using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SiteAudit
{
    #region Models

    [Serializable]
    public class ExcelAuditGroupDto
    {
        public int IssueCode { get; set; }
        public string IssueName { get; set; }
        public string Category { get; set; }
        public string Severity { get; set; }
        public string Source { get; set; }
        public int TotalUrls { get; set; }
        public int PendingCount { get; set; }
        public int ResolvedCount { get; set; }
        public int UnresolvedCount { get; set; }
    }

    [Serializable]
    public class ExcelAuditIssueItemDto
    {
        public long Id { get; set; }
        public int IssueCode { get; set; }
        public string IssueName { get; set; }
        public string Category { get; set; }
        public string Severity { get; set; }
        public string Source { get; set; }
        public string PageUrl { get; set; }
        public string InitialStatus { get; set; }
        public string VerificationStatus { get; set; } // Pending, Resolved, Unresolved, Error
        public DateTime? LastCheckedDate { get; set; }
        public string CheckResultDetails { get; set; }
        public int? ResponseCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    #endregion

    public static class ExcelAuditDal
    {
        private static string _connectionString = null;
        private static readonly object _lockObj = new object();

        /// <summary>
        /// Connection string name stored in the SharePoint 'ConnectionStrings' list in the Admin subsite.
        /// </summary>
        private const string PrimaryConnectionName = "PNU_SearchIndex";

        /// <summary>
        /// Shown when no connection string can be resolved. Without this the calls used to
        /// return empty results, so a configuration problem looked like "no data".
        /// </summary>
        internal const string NoConnectionMessage =
            "تعذر تحديد سلسلة الاتصال بقاعدة البيانات. أضف عنصراً باسم '" + PrimaryConnectionName +
            "' في قائمة ConnectionStrings داخل موقع Admin، أو عرّف ConnectionString بنفس الاسم في web.config.";

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

                    // 1. Fetch 'PNU_SearchIndex' from SharePoint 'ConnectionStrings' list via SQLFactory
                    try
                    {
                        connection = SQLFactory.GetConnectionString(PrimaryConnectionName);
                    }
                    catch { }

                    // 2. Fallback to web.config for 'PNU_SearchIndex'
                    if (string.IsNullOrEmpty(connection))
                    {
                        try
                        {
                            var cs = ConfigurationManager.ConnectionStrings[PrimaryConnectionName];
                            if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
                                connection = cs.ConnectionString;
                        }
                        catch { }
                    }

                    // 3. Fallback to SQLFactory default property if available
                    if (string.IsNullOrEmpty(connection))
                    {
                        try { connection = SQLFactory.connectionString; } catch { }
                    }

                    // 4. Fallback to generic ConnString in web.config
                    if (string.IsNullOrEmpty(connection))
                    {
                        try
                        {
                            var csConn = ConfigurationManager.ConnectionStrings["ConnString"];
                            if (csConn != null && !string.IsNullOrEmpty(csConn.ConnectionString))
                                connection = csConn.ConnectionString;
                        }
                        catch { }
                    }

                    _connectionString = connection;
                    return _connectionString;
                }
            }
            set
            {
                _connectionString = value;
            }
        }

        public static void EnsureTableExists()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                string connStr = ConnectionString;
                if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                const string ddl = @"
IF OBJECT_ID('dbo.ExcelAuditIssues', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ExcelAuditIssues
    (
        Id                   BIGINT IDENTITY(1,1) PRIMARY KEY,
        IssueCode            INT             NOT NULL,
        IssueName            NVARCHAR(250)   NOT NULL,
        Category             NVARCHAR(100)   NOT NULL,
        Severity             NVARCHAR(50)    NOT NULL,
        [Source]             NVARCHAR(50)    NOT NULL DEFAULT('Excel74'),
        PageUrl              NVARCHAR(1000)  NOT NULL,
        InitialStatus        NVARCHAR(250)   NULL,
        VerificationStatus   NVARCHAR(50)    NOT NULL DEFAULT('Pending'),
        LastCheckedDate      DATETIME        NULL,
        CheckResultDetails   NVARCHAR(MAX)   NULL,
        ResponseCode         INT             NULL,
        CreatedDate          DATETIME        NOT NULL DEFAULT(GETDATE())
    );

    CREATE INDEX IX_ExcelAuditIssues_IssueCode ON dbo.ExcelAuditIssues(IssueCode);
    CREATE INDEX IX_ExcelAuditIssues_Status ON dbo.ExcelAuditIssues(VerificationStatus);
    CREATE INDEX IX_ExcelAuditIssues_PageUrl ON dbo.ExcelAuditIssues(PageUrl);
    CREATE INDEX IX_ExcelAuditIssues_Source ON dbo.ExcelAuditIssues([Source]);
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ExcelAuditIssues') AND name = 'Source')
    BEGIN
        ALTER TABLE dbo.ExcelAuditIssues ADD [Source] NVARCHAR(50) NOT NULL CONSTRAINT DF_ExcelAuditIssues_Source DEFAULT('Excel74');
        CREATE INDEX IX_ExcelAuditIssues_Source ON dbo.ExcelAuditIssues([Source]);
    END
END;";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(ddl, conn) { CommandTimeout = 60 })
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        public static List<ExcelAuditGroupDto> GetGroupsSummary(string source = null)
        {
            var list = new List<ExcelAuditGroupDto>();
            EnsureTableExists();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                string connStr = ConnectionString;
                if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                string whereSql = string.IsNullOrEmpty(source) ? "" : "WHERE [Source] = @Source";
                string sql = @"
SELECT 
    IssueCode,
    MAX(IssueName) AS IssueName,
    MAX(Category) AS Category,
    MAX(Severity) AS Severity,
    MAX([Source]) AS [Source],
    COUNT(*) AS TotalUrls,
    SUM(CASE WHEN VerificationStatus = 'Pending' THEN 1 ELSE 0 END) AS PendingCount,
    SUM(CASE WHEN VerificationStatus = 'Resolved' THEN 1 ELSE 0 END) AS ResolvedCount,
    SUM(CASE WHEN VerificationStatus NOT IN ('Pending', 'Resolved') THEN 1 ELSE 0 END) AS UnresolvedCount
FROM dbo.ExcelAuditIssues
" + whereSql + @"
GROUP BY IssueCode
ORDER BY IssueCode ASC;";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
                    {
                        if (!string.IsNullOrEmpty(source))
                        {
                            cmd.Parameters.AddWithValue("@Source", source);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new ExcelAuditGroupDto
                                {
                                    IssueCode = Convert.ToInt32(reader["IssueCode"]),
                                    IssueName = reader["IssueName"]?.ToString() ?? "",
                                    Category = reader["Category"]?.ToString() ?? "",
                                    Severity = reader["Severity"]?.ToString() ?? "",
                                    Source = reader["Source"]?.ToString() ?? "Excel74",
                                    TotalUrls = Convert.ToInt32(reader["TotalUrls"]),
                                    PendingCount = Convert.ToInt32(reader["PendingCount"]),
                                    ResolvedCount = Convert.ToInt32(reader["ResolvedCount"]),
                                    UnresolvedCount = Convert.ToInt32(reader["UnresolvedCount"])
                                });
                            }
                        }
                    }
                }
            });

            return list;
        }

        public static List<ExcelAuditIssueItemDto> GetIssues(int issueCode, string statusFilter, string search, int pageIndex, int pageSize, out int totalCount, string source = null)
        {
            var list = new List<ExcelAuditIssueItemDto>();
            int count = 0;
            EnsureTableExists();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                string connStr = ConnectionString;
                if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                string whereClause = "WHERE IssueCode = @IssueCode";
                if (!string.IsNullOrEmpty(source))
                {
                    whereClause += " AND [Source] = @Source";
                }
                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    whereClause += " AND VerificationStatus = @Status";
                }
                if (!string.IsNullOrEmpty(search))
                {
                    whereClause += " AND (PageUrl LIKE @Search OR CheckResultDetails LIKE @Search)";
                }

                string countSql = "SELECT COUNT(*) FROM dbo.ExcelAuditIssues " + whereClause + ";";

                int offset = (pageIndex - 1) * pageSize;
                string querySql = @"
SELECT Id, IssueCode, IssueName, Category, Severity, [Source], PageUrl, InitialStatus, VerificationStatus, LastCheckedDate, CheckResultDetails, ResponseCode, CreatedDate
FROM dbo.ExcelAuditIssues
" + whereClause + @"
ORDER BY 
    CASE WHEN VerificationStatus = 'Pending' THEN 1 WHEN VerificationStatus = 'Unresolved' THEN 2 ELSE 3 END,
    Id ASC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Count
                    using (SqlCommand cmd = new SqlCommand(countSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IssueCode", issueCode);
                        if (!string.IsNullOrEmpty(source)) cmd.Parameters.AddWithValue("@Source", source);
                        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All") cmd.Parameters.AddWithValue("@Status", statusFilter);
                        if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Paged Results
                    using (SqlCommand cmd = new SqlCommand(querySql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IssueCode", issueCode);
                        cmd.Parameters.AddWithValue("@Offset", offset);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        if (!string.IsNullOrEmpty(source)) cmd.Parameters.AddWithValue("@Source", source);
                        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All") cmd.Parameters.AddWithValue("@Status", statusFilter);
                        if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new ExcelAuditIssueItemDto
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    IssueCode = Convert.ToInt32(reader["IssueCode"]),
                                    IssueName = reader["IssueName"]?.ToString(),
                                    Category = reader["Category"]?.ToString(),
                                    Severity = reader["Severity"]?.ToString(),
                                    Source = reader["Source"]?.ToString() ?? "Excel74",
                                    PageUrl = reader["PageUrl"]?.ToString(),
                                    InitialStatus = reader["InitialStatus"]?.ToString(),
                                    VerificationStatus = reader["VerificationStatus"]?.ToString(),
                                    LastCheckedDate = reader["LastCheckedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["LastCheckedDate"]) : null,
                                    CheckResultDetails = reader["CheckResultDetails"]?.ToString(),
                                    ResponseCode = reader["ResponseCode"] != DBNull.Value ? (int?)Convert.ToInt32(reader["ResponseCode"]) : null,
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                                });
                            }
                        }
                    }
                }
            });

            totalCount = count;
            return list;
        }

        public static void UpdateVerificationStatus(long id, string status, int? responseCode, string details)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                string connStr = ConnectionString;
                if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                const string sql = @"
UPDATE dbo.ExcelAuditIssues
SET 
    VerificationStatus = @Status,
    ResponseCode = @ResponseCode,
    CheckResultDetails = @Details,
    LastCheckedDate = GETDATE()
WHERE Id = @Id;";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@ResponseCode", (object)responseCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Details", (object)details ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        public static void BulkInsertIssues(DataTable dataTable)
        {
            EnsureTableExists();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                string connStr = ConnectionString;
                if (string.IsNullOrEmpty(connStr)) throw new InvalidOperationException(NoConnectionMessage);

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName = "dbo.ExcelAuditIssues";
                        bulkCopy.BatchSize = 5000;
                        bulkCopy.BulkCopyTimeout = 300;

                        bulkCopy.ColumnMappings.Add("IssueCode", "IssueCode");
                        bulkCopy.ColumnMappings.Add("IssueName", "IssueName");
                        bulkCopy.ColumnMappings.Add("Category", "Category");
                        bulkCopy.ColumnMappings.Add("Severity", "Severity");
                        if (dataTable.Columns.Contains("Source"))
                        {
                            bulkCopy.ColumnMappings.Add("Source", "Source");
                        }
                        bulkCopy.ColumnMappings.Add("PageUrl", "PageUrl");
                        bulkCopy.ColumnMappings.Add("InitialStatus", "InitialStatus");
                        bulkCopy.ColumnMappings.Add("VerificationStatus", "VerificationStatus");

                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            });
        }
    }
}
