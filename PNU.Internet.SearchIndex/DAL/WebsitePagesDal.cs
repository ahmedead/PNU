using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.DAL
{
    public class WebsitePageDto
    {
        public long PageId { get; set; }
        public string PageTitle { get; set; }
        public string PageURL { get; set; }
        public string PageLayout { get; set; }
        public string UserControlPath { get; set; }
        public string UserControlProperties { get; set; }
        public string WebUrl { get; set; }

        // English mirror
        public string PageTitleEn { get; set; }
        public string PageURLEn { get; set; }
        public string PageLayoutEn { get; set; }
        public string UserControlPathEn { get; set; }
        public string UserControlPropertiesEn { get; set; }
        public string WebUrlEn { get; set; }
        public bool EnExists { get; set; }

        public DateTime LastIndexed { get; set; }
    }

    /// <summary>
    /// Data access for the page catalog table (dbo.WebsitePages).
    /// This is independent of the search index; it's only used by the
    /// PageIndexAdmin.aspx layouts page for auditing publishing pages
    /// and their embedded web-part properties.
    /// </summary>
    public static class WebsitePagesDal
    {
        public static string ConnectionString
        {
            get
            {
                string ConnectionName = "PNU_SearchIndex";
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
                                             <Value Type='Text'>{ConnectionName}</Value>
                                          </Eq>
                                       </Where>");

                                    SPListItemCollection coll = list.GetItems(query);
                                    if (coll != null && coll.Count > 0 && coll[0]["ConnectionString"] != null)
                                        connection = coll[0]["ConnectionString"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SearchLogger.WriteToLog("WebsitePagesDal", "ConnectionString", ex.Message);
                    }
                });

                return connection;
            }
        }


        // ----- schema bootstrap -----------------------------------------
        /// <summary>
        /// Creates dbo.WebsitePages and dbo.usp_UpsertWebsitePage if
        /// missing. Safe to call every request; a no-op once objects
        /// exist. Prevents "table not found" errors if the operator
        /// deployed the DLL without re-running the .sql script.
        /// </summary>
        public static void EnsureWebsitePagesTableExists()
        {
            string ignored;
            EnsureWebsitePagesTableExists(out ignored);
        }

        /// <summary>
        /// Same as the parameterless overload but reports why schema
        /// setup failed (bad connection string, no CREATE rights, etc.)
        /// instead of silently continuing to a run that writes nothing.
        /// </summary>
        public static void EnsureWebsitePagesTableExists(out string errorMessage)
        {
            string err = null;
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
        PageTitleEn             NVARCHAR(500)  NULL,
        PageURLEn               NVARCHAR(1000) NULL,
        PageLayoutEn            NVARCHAR(500)  NULL,
        UserControlPathEn       NVARCHAR(MAX)  NULL,
        UserControlPropertiesEn NVARCHAR(MAX)  NULL,
        WebUrlEn                NVARCHAR(1000) NULL,
        EnExists                BIT NOT NULL DEFAULT(0),
        LastIndexed           DATETIME        NOT NULL DEFAULT(GETDATE())
    );
    CREATE UNIQUE INDEX UX_WebsitePages_PageURL
        ON dbo.WebsitePages(PageURL);
END
ELSE
BEGIN
    IF COL_LENGTH('dbo.WebsitePages','PageURLEn')               IS NULL
        ALTER TABLE dbo.WebsitePages ADD PageURLEn               NVARCHAR(1000) NULL;
    IF COL_LENGTH('dbo.WebsitePages','PageTitleEn')             IS NULL
        ALTER TABLE dbo.WebsitePages ADD PageTitleEn             NVARCHAR(500)  NULL;
    IF COL_LENGTH('dbo.WebsitePages','PageLayoutEn')            IS NULL
        ALTER TABLE dbo.WebsitePages ADD PageLayoutEn            NVARCHAR(500)  NULL;
    IF COL_LENGTH('dbo.WebsitePages','UserControlPathEn')       IS NULL
        ALTER TABLE dbo.WebsitePages ADD UserControlPathEn       NVARCHAR(MAX)  NULL;
    IF COL_LENGTH('dbo.WebsitePages','UserControlPropertiesEn') IS NULL
        ALTER TABLE dbo.WebsitePages ADD UserControlPropertiesEn NVARCHAR(MAX)  NULL;
    IF COL_LENGTH('dbo.WebsitePages','WebUrlEn')                IS NULL
        ALTER TABLE dbo.WebsitePages ADD WebUrlEn                NVARCHAR(1000) NULL;
    IF COL_LENGTH('dbo.WebsitePages','EnExists')                IS NULL
        ALTER TABLE dbo.WebsitePages ADD EnExists                BIT NOT NULL DEFAULT(0);
END";

            const string procDrop = @"
IF OBJECT_ID('dbo.usp_UpsertWebsitePage','P') IS NOT NULL
    DROP PROCEDURE dbo.usp_UpsertWebsitePage;";

            const string procCreate = @"
CREATE PROCEDURE dbo.usp_UpsertWebsitePage
    @PageTitle               NVARCHAR(500)  = NULL,
    @PageURL                 NVARCHAR(1000),
    @PageLayout              NVARCHAR(500)  = NULL,
    @UserControlPath         NVARCHAR(MAX)  = NULL,
    @UserControlProperties   NVARCHAR(MAX)  = NULL,
    @WebUrl                  NVARCHAR(1000) = NULL,
    @PageTitleEn             NVARCHAR(500)  = NULL,
    @PageURLEn               NVARCHAR(1000) = NULL,
    @PageLayoutEn            NVARCHAR(500)  = NULL,
    @UserControlPathEn       NVARCHAR(MAX)  = NULL,
    @UserControlPropertiesEn NVARCHAR(MAX)  = NULL,
    @WebUrlEn                NVARCHAR(1000) = NULL,
    @EnExists                BIT            = 0
AS
BEGIN
    SET NOCOUNT ON;
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
                @UserControlPropertiesEn, @WebUrlEn, @EnExists, GETDATE());
END";

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        Exec(conn, ddl);

                        // Always recreate: the signature changed when the
                        // bilingual columns were added, so an existing v1
                        // proc must be replaced.
                        Exec(conn, procDrop);
                        Exec(conn, procCreate);
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    SearchLogger.WriteToLog("WebsitePagesDal",
                        "EnsureTable", ex.ToString());
                }
            });

            errorMessage = err;
        }

        /// <summary>
        /// Round-trips a trivial query so the UI can prove the app pool
        /// can actually reach PNU_SearchIndex. Returns null on success,
        /// otherwise the failure message.
        /// </summary>
        public static string TestConnection()
        {
            string err = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM dbo.WebsitePages", conn))
                    {
                        conn.Open();
                        cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex) { err = ex.Message; }
            });
            return err;
        }

        /// <summary>Row count, or -1 when the query fails.</summary>
        public static int GetRowCount()
        {
            int n = -1;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM dbo.WebsitePages", conn))
                    {
                        conn.Open();
                        n = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("WebsitePagesDal",
                        "GetRowCount", ex.Message);
                }
            });
            return n;
        }

        // ----- upsert ----------------------------------------------------
        public static bool UpsertWebsitePage(string title, string url,
            string layout, string ucPath, string ucProps, string webUrl,
            string titleEn = null, string urlEn = null, string layoutEn = null,
            string ucPathEn = null, string ucPropsEn = null,
            string webUrlEn = null, bool enExists = false)
        {
            string ignored;
            return UpsertWebsitePage(title, url, layout, ucPath, ucProps,
                webUrl, titleEn, urlEn, layoutEn, ucPathEn, ucPropsEn,
                webUrlEn, enExists, out ignored);
        }

        /// <summary>
        /// Upserts one page row. Returns false and sets
        /// <paramref name="errorMessage"/> when the write fails, so the
        /// caller can report a real success count instead of assuming
        /// every row landed.
        /// </summary>
        public static bool UpsertWebsitePage(string title, string url,
            string layout, string ucPath, string ucProps, string webUrl,
            string titleEn, string urlEn, string layoutEn,
            string ucPathEn, string ucPropsEn, string webUrlEn,
            bool enExists, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(url))
            {
                errorMessage = "Empty page URL.";
                return false;
            }

            bool ok = false;
            string err = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    using (var cmd = new SqlCommand("dbo.usp_UpsertWebsitePage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageTitle",
                            (object)Truncate(title, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageURL",
                            Truncate(url, 1000));
                        cmd.Parameters.AddWithValue("@PageLayout",
                            (object)Truncate(layout, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlPath",
                            (object)ucPath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlProperties",
                            (object)ucProps ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@WebUrl",
                            (object)Truncate(webUrl, 1000) ?? DBNull.Value);

                        cmd.Parameters.AddWithValue("@PageTitleEn",
                            (object)Truncate(titleEn, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageURLEn",
                            (object)Truncate(urlEn, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageLayoutEn",
                            (object)Truncate(layoutEn, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlPathEn",
                            (object)ucPathEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlPropertiesEn",
                            (object)ucPropsEn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@WebUrlEn",
                            (object)Truncate(webUrlEn, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EnExists", enExists);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ok = true;
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    SearchLogger.WriteToLog("WebsitePagesDal",
                        "Upsert " + url, ex.ToString());
                }
            });

            errorMessage = err;
            return ok;
        }

        // ----- read ------------------------------------------------------
        /// <summary>
        /// Returns a lookup of PageURL → WebsitePageDto for fast status
        /// checks when rendering the grid.
        /// </summary>
        public static Dictionary<string, WebsitePageDto> GetIndexedWebsitePagesMap()
        {
            var map = new Dictionary<string, WebsitePageDto>(
                StringComparer.OrdinalIgnoreCase);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    using (var cmd = new SqlCommand(
                        "SELECT PageId, PageTitle, PageURL, PageLayout, " +
                        "UserControlPath, UserControlProperties, WebUrl, " +
                        "PageTitleEn, PageURLEn, PageLayoutEn, " +
                        "UserControlPathEn, UserControlPropertiesEn, " +
                        "WebUrlEn, EnExists, " +
                        "LastIndexed FROM dbo.WebsitePages", conn))
                    {
                        cmd.CommandTimeout = 60;
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var dto = MapReader(rdr);
                                if (!string.IsNullOrEmpty(dto.PageURL))
                                    map[dto.PageURL] = dto;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("WebsitePagesDal",
                        "GetIndexedWebsitePagesMap", ex.Message);
                }
            });
            return map;
        }

        public static List<WebsitePageDto> GetAll()
        {
            var list = new List<WebsitePageDto>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    using (var cmd = new SqlCommand(
                        "SELECT PageId, PageTitle, PageURL, PageLayout, " +
                        "UserControlPath, UserControlProperties, WebUrl, " +
                        "PageTitleEn, PageURLEn, PageLayoutEn, " +
                        "UserControlPathEn, UserControlPropertiesEn, " +
                        "WebUrlEn, EnExists, " +
                        "LastIndexed FROM dbo.WebsitePages " +
                        "ORDER BY WebUrl, PageURL", conn))
                    {
                        cmd.CommandTimeout = 60;
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                            while (rdr.Read()) list.Add(MapReader(rdr));
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("WebsitePagesDal",
                        "GetAll", ex.Message);
                }
            });
            return list;
        }

        // ----- helpers ---------------------------------------------------
        private static WebsitePageDto MapReader(IDataReader rdr)
        {
            return new WebsitePageDto
            {
                PageId = Convert.ToInt64(rdr["PageId"]),
                PageTitle = rdr["PageTitle"] as string,
                PageURL = rdr["PageURL"] as string,
                PageLayout = rdr["PageLayout"] as string,
                UserControlPath = rdr["UserControlPath"] as string,
                UserControlProperties = rdr["UserControlProperties"] as string,
                WebUrl = rdr["WebUrl"] as string,
                PageTitleEn = SafeCol(rdr, "PageTitleEn"),
                PageURLEn = SafeCol(rdr, "PageURLEn"),
                PageLayoutEn = SafeCol(rdr, "PageLayoutEn"),
                UserControlPathEn = SafeCol(rdr, "UserControlPathEn"),
                UserControlPropertiesEn = SafeCol(rdr, "UserControlPropertiesEn"),
                WebUrlEn = SafeCol(rdr, "WebUrlEn"),
                EnExists = SafeBool(rdr, "EnExists"),
                LastIndexed = (DateTime)rdr["LastIndexed"]
            };
        }

        private static string SafeCol(IDataReader rdr, string name)
        {
            try
            {
                int i = rdr.GetOrdinal(name);
                return rdr.IsDBNull(i) ? null : rdr.GetValue(i) as string;
            }
            catch { return null; }
        }

        private static bool SafeBool(IDataReader rdr, string name)
        {
            try
            {
                int i = rdr.GetOrdinal(name);
                return !rdr.IsDBNull(i) && Convert.ToBoolean(rdr.GetValue(i));
            }
            catch { return false; }
        }

        private static void Exec(SqlConnection conn, string sql)
        {
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 60;
                cmd.ExecuteNonQuery();
            }
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max);
        }
    }

    [Serializable]
    public class MissingPageOnEnDto
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
        public string Status { get; set; } // "Missing", "Created", "Updated"
        public DateTime LastDiscoveredDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    /// <summary>
    /// Data Access Layer for managing missing pages on English sites (dbo.MissingPagesOnEN).
    /// Stores both relative and full URLs, page layout, web part details, and sync/creation status.
    /// </summary>
    public static class MissingPagesOnEnDal
    {
        public static void EnsureMissingPagesOnEnTableExists()
        {
            string ignored;
            EnsureMissingPagesOnEnTableExists(out ignored);
        }

        public static void EnsureMissingPagesOnEnTableExists(out string errorMessage)
        {
            string err = null;
            const string ddl = @"
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
    CREATE UNIQUE INDEX UX_MissingPagesOnEN_TargetPageUrl
        ON dbo.MissingPagesOnEN(TargetPageUrl);
END
ELSE
BEGIN
    IF COL_LENGTH('dbo.MissingPagesOnEN','SourcePageFullUrl') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD SourcePageFullUrl NVARCHAR(1000) NULL;
    IF COL_LENGTH('dbo.MissingPagesOnEN','TargetPageFullUrl') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD TargetPageFullUrl NVARCHAR(1000) NULL;
    IF COL_LENGTH('dbo.MissingPagesOnEN','SourceWebFullUrl') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD SourceWebFullUrl NVARCHAR(1000) NULL;
    IF COL_LENGTH('dbo.MissingPagesOnEN','TargetWebFullUrl') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD TargetWebFullUrl NVARCHAR(1000) NULL;
    IF COL_LENGTH('dbo.MissingPagesOnEN','PageLayoutFileName') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD PageLayoutFileName NVARCHAR(500) NULL;
    IF COL_LENGTH('dbo.MissingPagesOnEN','IsCreated') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD IsCreated BIT NOT NULL DEFAULT(0);
    IF COL_LENGTH('dbo.MissingPagesOnEN','CreatedDate') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD CreatedDate DATETIME NULL;
    IF COL_LENGTH('dbo.MissingPagesOnEN','Status') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD Status NVARCHAR(100) NOT NULL DEFAULT('Missing');
    IF COL_LENGTH('dbo.MissingPagesOnEN','LastDiscoveredDate') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD LastDiscoveredDate DATETIME NOT NULL DEFAULT(GETDATE());
    IF COL_LENGTH('dbo.MissingPagesOnEN','LastUpdatedDate') IS NULL
        ALTER TABLE dbo.MissingPagesOnEN ADD LastUpdatedDate DATETIME NOT NULL DEFAULT(GETDATE());
END";

            const string procDrop = @"
IF OBJECT_ID('dbo.usp_UpsertMissingPageOnEN','P') IS NOT NULL
    DROP PROCEDURE dbo.usp_UpsertMissingPageOnEN;
IF OBJECT_ID('dbo.usp_MarkMissingPageCreated','P') IS NOT NULL
    DROP PROCEDURE dbo.usp_MarkMissingPageCreated;";

            const string procCreate = @"
CREATE PROCEDURE dbo.usp_UpsertMissingPageOnEN
    @PageFileName           NVARCHAR(500),
    @PageTitleAr            NVARCHAR(500)  = NULL,
    @PageTitleEn            NVARCHAR(500)  = NULL,
    @SourcePageUrl          NVARCHAR(1000) = NULL,
    @SourcePageFullUrl      NVARCHAR(1000) = NULL,
    @TargetPageUrl          NVARCHAR(1000),
    @TargetPageFullUrl      NVARCHAR(1000),
    @SourceWebUrl           NVARCHAR(1000) = NULL,
    @SourceWebFullUrl       NVARCHAR(1000) = NULL,
    @TargetWebUrl           NVARCHAR(1000) = NULL,
    @TargetWebFullUrl       NVARCHAR(1000) = NULL,
    @PageLayout             NVARCHAR(500)  = NULL,
    @PageLayoutFileName     NVARCHAR(500)  = NULL,
    @HasControlLoader       BIT            = 0,
    @UserControlPath        NVARCHAR(MAX)  = NULL,
    @UserControlProperties  NVARCHAR(MAX)  = NULL,
    @HasContentEditor       BIT            = 0,
    @HasPageContent         BIT            = 0,
    @Direction              NVARCHAR(50)   = 'ArToEn',
    @IsCreated              BIT            = 0,
    @CreatedDate            DATETIME       = NULL,
    @Status                 NVARCHAR(100)  = 'Missing'
AS
BEGIN
    SET NOCOUNT ON;
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
                @Status, GETDATE(), GETDATE());
END;";

            const string procMarkCreated = @"
CREATE PROCEDURE dbo.usp_MarkMissingPageCreated
    @TargetPageUrl NVARCHAR(1000),
    @CreatedDate   DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.MissingPagesOnEN
    SET IsCreated       = 1,
        Status          = 'Created',
        CreatedDate     = ISNULL(@CreatedDate, GETDATE()),
        LastUpdatedDate = GETDATE()
    WHERE TargetPageUrl = @TargetPageUrl
       OR TargetPageFullUrl = @TargetPageUrl;
END;";

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(WebsitePagesDal.ConnectionString))
                    {
                        conn.Open();
                        Exec(conn, ddl);
                        Exec(conn, procDrop);
                        Exec(conn, procCreate);
                        Exec(conn, procMarkCreated);
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    SearchLogger.WriteToLog("MissingPagesOnEnDal", "EnsureMissingPagesOnEnTableExists", ex.ToString());
                }
            });

            errorMessage = err;
        }

        public static bool UpsertMissingPage(MissingPageOnEnDto dto, out string errorMessage)
        {
            errorMessage = null;
            if (dto == null || string.IsNullOrEmpty(dto.TargetPageUrl))
            {
                errorMessage = "Invalid missing page data.";
                return false;
            }

            bool ok = false;
            string err = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureMissingPagesOnEnTableExists(out err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        return;
                    }

                    using (var conn = new SqlConnection(WebsitePagesDal.ConnectionString))
                    using (var cmd = new SqlCommand("dbo.usp_UpsertMissingPageOnEN", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageFileName", Truncate(dto.PageFileName, 500));
                        cmd.Parameters.AddWithValue("@PageTitleAr", (object)Truncate(dto.PageTitleAr, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageTitleEn", (object)Truncate(dto.PageTitleEn, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SourcePageUrl", (object)Truncate(dto.SourcePageUrl, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SourcePageFullUrl", (object)Truncate(dto.SourcePageFullUrl, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TargetPageUrl", Truncate(dto.TargetPageUrl, 1000));
                        cmd.Parameters.AddWithValue("@TargetPageFullUrl", Truncate(dto.TargetPageFullUrl ?? dto.TargetPageUrl, 1000));
                        cmd.Parameters.AddWithValue("@SourceWebUrl", (object)Truncate(dto.SourceWebUrl, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SourceWebFullUrl", (object)Truncate(dto.SourceWebFullUrl, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TargetWebUrl", (object)Truncate(dto.TargetWebUrl, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TargetWebFullUrl", (object)Truncate(dto.TargetWebFullUrl, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageLayout", (object)Truncate(dto.PageLayout, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageLayoutFileName", (object)Truncate(dto.PageLayoutFileName, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HasControlLoader", dto.HasControlLoader);
                        cmd.Parameters.AddWithValue("@UserControlPath", (object)dto.UserControlPath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserControlProperties", (object)dto.UserControlProperties ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HasContentEditor", dto.HasContentEditor);
                        cmd.Parameters.AddWithValue("@HasPageContent", dto.HasPageContent);
                        cmd.Parameters.AddWithValue("@Direction", Truncate(dto.Direction ?? "ArToEn", 50));
                        cmd.Parameters.AddWithValue("@IsCreated", dto.IsCreated);
                        cmd.Parameters.AddWithValue("@CreatedDate", (object)dto.CreatedDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", Truncate(dto.Status ?? (dto.IsCreated ? "Created" : "Missing"), 100));

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ok = true;
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    SearchLogger.WriteToLog("MissingPagesOnEnDal", "UpsertMissingPage " + dto.TargetPageUrl, ex.ToString());
                }
            });

            errorMessage = err;
            return ok;
        }

        public static int BulkUpsertMissingPages(IEnumerable<MissingPageOnEnDto> dtos, out string errorMessage)
        {
            errorMessage = null;
            if (dtos == null) return 0;

            int count = 0;
            string lastErr = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureMissingPagesOnEnTableExists(out lastErr);
                    if (!string.IsNullOrEmpty(lastErr))
                    {
                        return;
                    }

                    using (var conn = new SqlConnection(WebsitePagesDal.ConnectionString))
                    {
                        conn.Open();
                        foreach (var dto in dtos)
                        {
                            try
                            {
                                using (var cmd = new SqlCommand("dbo.usp_UpsertMissingPageOnEN", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@PageFileName", Truncate(dto.PageFileName, 500));
                                    cmd.Parameters.AddWithValue("@PageTitleAr", (object)Truncate(dto.PageTitleAr, 500) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@PageTitleEn", (object)Truncate(dto.PageTitleEn, 500) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@SourcePageUrl", (object)Truncate(dto.SourcePageUrl, 1000) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@SourcePageFullUrl", (object)Truncate(dto.SourcePageFullUrl, 1000) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@TargetPageUrl", Truncate(dto.TargetPageUrl, 1000));
                                    cmd.Parameters.AddWithValue("@TargetPageFullUrl", Truncate(dto.TargetPageFullUrl ?? dto.TargetPageUrl, 1000));
                                    cmd.Parameters.AddWithValue("@SourceWebUrl", (object)Truncate(dto.SourceWebUrl, 1000) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@SourceWebFullUrl", (object)Truncate(dto.SourceWebFullUrl, 1000) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@TargetWebUrl", (object)Truncate(dto.TargetWebUrl, 1000) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@TargetWebFullUrl", (object)Truncate(dto.TargetWebFullUrl, 1000) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@PageLayout", (object)Truncate(dto.PageLayout, 500) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@PageLayoutFileName", (object)Truncate(dto.PageLayoutFileName, 500) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@HasControlLoader", dto.HasControlLoader);
                                    cmd.Parameters.AddWithValue("@UserControlPath", (object)dto.UserControlPath ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@UserControlProperties", (object)dto.UserControlProperties ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@HasContentEditor", dto.HasContentEditor);
                                    cmd.Parameters.AddWithValue("@HasPageContent", dto.HasPageContent);
                                    cmd.Parameters.AddWithValue("@Direction", Truncate(dto.Direction ?? "ArToEn", 50));
                                    cmd.Parameters.AddWithValue("@IsCreated", dto.IsCreated);
                                    cmd.Parameters.AddWithValue("@CreatedDate", (object)dto.CreatedDate ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Status", Truncate(dto.Status ?? (dto.IsCreated ? "Created" : "Missing"), 100));

                                    cmd.ExecuteNonQuery();
                                    count++;
                                }
                            }
                            catch (Exception ex)
                            {
                                lastErr = ex.Message;
                                SearchLogger.WriteToLog("MissingPagesOnEnDal", "BulkUpsert row " + dto.TargetPageUrl, ex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lastErr = ex.Message;
                    SearchLogger.WriteToLog("MissingPagesOnEnDal", "BulkUpsertMissingPages", ex.ToString());
                }
            });

            errorMessage = lastErr;
            return count;
        }

        public static bool MarkPageAsCreated(string targetPageUrlOrFullUrl, DateTime? createdDate, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(targetPageUrlOrFullUrl))
            {
                errorMessage = "Invalid page URL.";
                return false;
            }

            bool ok = false;
            string err = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureMissingPagesOnEnTableExists(out err);
                    using (var conn = new SqlConnection(WebsitePagesDal.ConnectionString))
                    using (var cmd = new SqlCommand("dbo.usp_MarkMissingPageCreated", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TargetPageUrl", targetPageUrlOrFullUrl);
                        cmd.Parameters.AddWithValue("@CreatedDate", (object)createdDate ?? DateTime.Now);

                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        ok = rows > 0;
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    SearchLogger.WriteToLog("MissingPagesOnEnDal", "MarkPageAsCreated " + targetPageUrlOrFullUrl, ex.ToString());
                }
            });

            errorMessage = err;
            return ok;
        }

        public static List<MissingPageOnEnDto> GetAllMissingPages(bool onlyNotCreated = false)
        {
            return SearchMissingPages(null, null, onlyNotCreated ? "Pending" : "All");
        }

        public static List<MissingPageOnEnDto> SearchMissingPages(string webUrlFilter = null, string keyword = null, string statusFilter = "All")
        {
            var list = new List<MissingPageOnEnDto>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureMissingPagesOnEnTableExists();
                    using (var conn = new SqlConnection(WebsitePagesDal.ConnectionString))
                    {
                        var sb = new StringBuilder();
                        sb.Append("SELECT Id, PageFileName, PageTitleAr, PageTitleEn, SourcePageUrl, SourcePageFullUrl, " +
                                  "TargetPageUrl, TargetPageFullUrl, SourceWebUrl, SourceWebFullUrl, TargetWebUrl, TargetWebFullUrl, " +
                                  "PageLayout, PageLayoutFileName, HasControlLoader, UserControlPath, UserControlProperties, " +
                                  "HasContentEditor, HasPageContent, Direction, IsCreated, CreatedDate, Status, LastDiscoveredDate, LastUpdatedDate " +
                                  "FROM dbo.MissingPagesOnEN WHERE 1=1 ");

                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandTimeout = 60;

                            bool hasUrl = !string.IsNullOrWhiteSpace(webUrlFilter);
                            bool hasKw = !string.IsNullOrWhiteSpace(keyword);

                            // If both are passed with the same query string (unified search), search OR across all columns
                            if (hasUrl && hasKw && string.Equals(webUrlFilter.Trim(), keyword.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                string q = webUrlFilter.Trim().TrimEnd('/');
                                string altQ = null;
                                if (q.IndexOf("/ar/", StringComparison.OrdinalIgnoreCase) >= 0)
                                    altQ = q.Replace("/ar/", "/en/").Replace("/AR/", "/en/");
                                else if (q.IndexOf("/en/", StringComparison.OrdinalIgnoreCase) >= 0)
                                    altQ = q.Replace("/en/", "/ar/").Replace("/EN/", "/ar/");

                                sb.Append("AND (SourceWebUrl LIKE @Q OR TargetWebUrl LIKE @Q OR SourcePageUrl LIKE @Q OR TargetPageUrl LIKE @Q " +
                                          "OR SourceWebFullUrl LIKE @Q OR TargetWebFullUrl LIKE @Q OR SourcePageFullUrl LIKE @Q OR TargetPageFullUrl LIKE @Q " +
                                          "OR PageFileName LIKE @Q OR PageTitleAr LIKE @Q OR PageTitleEn LIKE @Q OR UserControlPath LIKE @Q " +
                                          "OR PageLayout LIKE @Q OR PageLayoutFileName LIKE @Q ");

                                if (!string.IsNullOrEmpty(altQ))
                                {
                                    sb.Append("OR SourceWebUrl LIKE @AltQ OR TargetWebUrl LIKE @AltQ OR SourcePageUrl LIKE @AltQ OR TargetPageUrl LIKE @AltQ " +
                                              "OR SourceWebFullUrl LIKE @AltQ OR TargetWebFullUrl LIKE @AltQ OR SourcePageFullUrl LIKE @AltQ OR TargetPageFullUrl LIKE @AltQ ");
                                    cmd.Parameters.AddWithValue("@AltQ", "%" + altQ + "%");
                                }

                                sb.Append(") ");
                                cmd.Parameters.AddWithValue("@Q", "%" + q + "%");
                            }
                            else
                            {
                                if (hasUrl)
                                {
                                    string cleanUrl = webUrlFilter.Trim().TrimEnd('/');
                                    string altUrl = null;
                                    if (cleanUrl.IndexOf("/ar/", StringComparison.OrdinalIgnoreCase) >= 0)
                                        altUrl = cleanUrl.Replace("/ar/", "/en/").Replace("/AR/", "/en/");
                                    else if (cleanUrl.IndexOf("/en/", StringComparison.OrdinalIgnoreCase) >= 0)
                                        altUrl = cleanUrl.Replace("/en/", "/ar/").Replace("/EN/", "/ar/");

                                    sb.Append("AND (SourceWebUrl LIKE @Url OR TargetWebUrl LIKE @Url OR SourcePageUrl LIKE @Url OR TargetPageUrl LIKE @Url " +
                                              "OR SourceWebFullUrl LIKE @Url OR TargetWebFullUrl LIKE @Url OR SourcePageFullUrl LIKE @Url OR TargetPageFullUrl LIKE @Url ");
                                    if (!string.IsNullOrEmpty(altUrl))
                                    {
                                        sb.Append("OR SourceWebUrl LIKE @AltUrl OR TargetWebUrl LIKE @AltUrl OR SourcePageUrl LIKE @AltUrl OR TargetPageUrl LIKE @AltUrl " +
                                                  "OR SourceWebFullUrl LIKE @AltUrl OR TargetWebFullUrl LIKE @AltUrl OR SourcePageFullUrl LIKE @AltUrl OR TargetPageFullUrl LIKE @AltUrl ");
                                        cmd.Parameters.AddWithValue("@AltUrl", "%" + altUrl + "%");
                                    }
                                    sb.Append(") ");
                                    cmd.Parameters.AddWithValue("@Url", "%" + cleanUrl + "%");
                                }

                                if (hasKw)
                                {
                                    sb.Append("AND (PageFileName LIKE @Kw OR PageTitleAr LIKE @Kw OR PageTitleEn LIKE @Kw OR UserControlPath LIKE @Kw OR PageLayout LIKE @Kw OR PageLayoutFileName LIKE @Kw) ");
                                    cmd.Parameters.AddWithValue("@Kw", "%" + keyword.Trim() + "%");
                                }
                            }

                            if (string.Equals(statusFilter, "Pending", StringComparison.OrdinalIgnoreCase) || string.Equals(statusFilter, "Missing", StringComparison.OrdinalIgnoreCase))
                            {
                                sb.Append("AND IsCreated = 0 ");
                            }
                            else if (string.Equals(statusFilter, "Created", StringComparison.OrdinalIgnoreCase))
                            {
                                sb.Append("AND IsCreated = 1 ");
                            }

                            sb.Append("ORDER BY TargetWebUrl, PageFileName");
                            cmd.CommandText = sb.ToString();

                            conn.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    list.Add(MapMissingPageReader(rdr));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("MissingPagesOnEnDal", "SearchMissingPages", ex.Message);
                }
            });
            return list;
        }

        public static void GetMissingPagesCount(out int totalCount, out int pendingCount, out int createdCount)
        {
            int total = 0;
            int pending = 0;
            int created = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    EnsureMissingPagesOnEnTableExists();
                    using (var conn = new SqlConnection(WebsitePagesDal.ConnectionString))
                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(*) AS Total, " +
                        "ISNULL(SUM(CASE WHEN IsCreated = 0 THEN 1 ELSE 0 END), 0) AS Pending, " +
                        "ISNULL(SUM(CASE WHEN IsCreated = 1 THEN 1 ELSE 0 END), 0) AS Created " +
                        "FROM dbo.MissingPagesOnEN", conn))
                    {
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                total = Convert.ToInt32(rdr["Total"]);
                                pending = Convert.ToInt32(rdr["Pending"]);
                                created = Convert.ToInt32(rdr["Created"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("MissingPagesOnEnDal", "GetMissingPagesCount", ex.Message);
                }
            });

            totalCount = total;
            pendingCount = pending;
            createdCount = created;
        }

        private static MissingPageOnEnDto MapMissingPageReader(IDataReader rdr)
        {
            return new MissingPageOnEnDto
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
                LastDiscoveredDate = (DateTime)rdr["LastDiscoveredDate"],
                LastUpdatedDate = (DateTime)rdr["LastUpdatedDate"]
            };
        }

        private static void Exec(SqlConnection conn, string sql)
        {
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 60;
                cmd.ExecuteNonQuery();
            }
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max);
        }
    }
}
