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
        public long     PageId                { get; set; }
        public string   PageTitle             { get; set; }
        public string   PageURL               { get; set; }
        public string   PageLayout            { get; set; }
        public string   UserControlPath       { get; set; }
        public string   UserControlProperties { get; set; }
        public string   WebUrl                { get; set; }
        public DateTime LastIndexed           { get; set; }
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
        LastIndexed           DATETIME        NOT NULL DEFAULT(GETDATE())
    );
    CREATE UNIQUE INDEX UX_WebsitePages_PageURL
        ON dbo.WebsitePages(PageURL);
END";

            const string procDrop = @"
IF OBJECT_ID('dbo.usp_UpsertWebsitePage','P') IS NOT NULL
    DROP PROCEDURE dbo.usp_UpsertWebsitePage;";

            const string procCreate = @"
CREATE PROCEDURE dbo.usp_UpsertWebsitePage
    @PageTitle             NVARCHAR(500) = NULL,
    @PageURL               NVARCHAR(1000),
    @PageLayout            NVARCHAR(500) = NULL,
    @UserControlPath       NVARCHAR(MAX) = NULL,
    @UserControlProperties NVARCHAR(MAX) = NULL,
    @WebUrl                NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    MERGE dbo.WebsitePages AS T
    USING (SELECT @PageURL AS PageURL) AS S ON T.PageURL = S.PageURL
    WHEN MATCHED THEN
        UPDATE SET
            PageTitle             = @PageTitle,
            PageLayout            = @PageLayout,
            UserControlPath       = @UserControlPath,
            UserControlProperties = @UserControlProperties,
            WebUrl                = @WebUrl,
            LastIndexed           = GETDATE()
    WHEN NOT MATCHED THEN
        INSERT (PageTitle, PageURL, PageLayout, UserControlPath,
                UserControlProperties, WebUrl, LastIndexed)
        VALUES (@PageTitle, @PageURL, @PageLayout, @UserControlPath,
                @UserControlProperties, @WebUrl, GETDATE());
END";

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        Exec(conn, ddl);

                        bool procExists = false;
                        using (var cmd = new SqlCommand(
                            "SELECT CASE WHEN OBJECT_ID('dbo.usp_UpsertWebsitePage','P') " +
                            "IS NULL THEN 0 ELSE 1 END", conn))
                        {
                            procExists = Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                        }
                        if (!procExists)
                        {
                            Exec(conn, procDrop);
                            Exec(conn, procCreate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("WebsitePagesDal",
                        "EnsureTable", ex.Message);
                }
            });
        }

        // ----- upsert ----------------------------------------------------
        public static void UpsertWebsitePage(string title, string url,
            string layout, string ucPath, string ucProps, string webUrl)
        {
            if (string.IsNullOrEmpty(url)) return;
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
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("WebsitePagesDal",
                        "Upsert " + url, ex.Message);
                }
            });
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
                    using (var cmd  = new SqlCommand(
                        "SELECT PageId, PageTitle, PageURL, PageLayout, " +
                        "UserControlPath, UserControlProperties, WebUrl, " +
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
                    using (var cmd  = new SqlCommand(
                        "SELECT PageId, PageTitle, PageURL, PageLayout, " +
                        "UserControlPath, UserControlProperties, WebUrl, " +
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
                PageId                = Convert.ToInt64(rdr["PageId"]),
                PageTitle             = rdr["PageTitle"]             as string,
                PageURL               = rdr["PageURL"]               as string,
                PageLayout            = rdr["PageLayout"]            as string,
                UserControlPath       = rdr["UserControlPath"]       as string,
                UserControlProperties = rdr["UserControlProperties"] as string,
                WebUrl                = rdr["WebUrl"]                as string,
                LastIndexed           = (DateTime)rdr["LastIndexed"]
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
