using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        private static string ConnectionString
        {
            get
            {
                string ConnectionName = "PNU_SearchIndex";
                string connection = "";
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists.TryGetList("ConnectionStrings");

                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             $@"<Where>
                                  <Eq>
                                     <FieldRef Name='Title' />
                                     <Value Type='Text'>{ConnectionName}</Value>
                                  </Eq>
                               </Where>");

                            SPListItemCollection coll = list.GetItems(query);
                            if (coll != null && coll.Count > 0)
                                connection = coll[0]["ConnectionString"].ToString();
                        }
                    }
                });

                return connection;

                //var cs = ConfigurationManager.ConnectionStrings["PNU_SearchIndex"];
                //if (cs == null || string.IsNullOrEmpty(cs.ConnectionString))
                //    throw new ConfigurationErrorsException(
                //        "Connection string 'PNU_SearchIndex' is missing in web.config.");
                //return cs.ConnectionString;
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
}
