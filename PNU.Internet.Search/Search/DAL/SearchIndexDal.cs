using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Microsoft.SharePoint;
using Portal.Main.Helper;

namespace PNU.Internet.Search.DAL
{
    /// <summary>
    /// Light wrapper around ADO.NET for the PNU_SearchIndex database.
    /// Connection string is read from web.config: PNU_SearchIndex
    /// </summary>
    public static class SearchIndexDal
    {
        public static string GetConnectionString(string ConnectionName)
        {
            string connection = "";
            try
            {

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

            }

            catch (Exception ex)
            {
                
            }
            return connection;

        }

        // ----- connection ------------------------------------------------
        private static string ConnectionString
        {
            get
            {
                string ConnectionString = GetConnectionString("PNU_SearchIndex");
                if (ConnectionString == null || string.IsNullOrEmpty(ConnectionString))
                {
                    throw new ConfigurationErrorsException(
                        "Connection string 'PNU_SearchIndex' is missing in List");
                }
                return ConnectionString;
            }
        }

        // ----- upsert ----------------------------------------------------
        public static void Upsert(SearchIndexItem item)
        {
            if (item == null) return;

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand("dbo.usp_UpsertSearchItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SourceType", item.SourceType ?? "");
                    cmd.Parameters.AddWithValue("@SourceKey", item.SourceKey ?? "");
                    cmd.Parameters.AddWithValue("@SiteId", item.SiteId);
                    cmd.Parameters.AddWithValue("@WebId", item.WebId);
                    cmd.Parameters.AddWithValue("@ListId",
                        (object)item.ListId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ListItemId",
                        (object)item.ListItemId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TitleAr",
                        (object)Truncate(item.TitleAr, 500) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TitleEn",
                        (object)Truncate(item.TitleEn, 500) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContentAr",
                        (object)item.ContentAr ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContentEn",
                        (object)item.ContentEn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Url", item.Url ?? "");
                    cmd.Parameters.AddWithValue("@Category",
                        (object)Truncate(item.Category, 200) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DisplayDate",
                        (object)item.DisplayDate ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        // ----- soft delete ----------------------------------------------
        public static void Deactivate(string sourceKey)
        {
            if (string.IsNullOrEmpty(sourceKey)) return;
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand("dbo.usp_DeactivateSearchItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SourceKey", sourceKey);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        // ----- search ---------------------------------------------------
        public static List<SearchIndexItem> Search(
            string keyword,
            bool isArabic,
            string categoriesCsv,
            string sortBy,
            int pageIndex,
            int pageSize,
            out int totalCount)
        {
            totalCount = 0;
            var list = new List<SearchIndexItem>();

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand("dbo.usp_SearchItems", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword ?? "");
                    cmd.Parameters.AddWithValue("@IsArabic", isArabic);
                    cmd.Parameters.AddWithValue("@Categories",
                        (object)categoriesCsv ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SortBy", sortBy ?? "rel");
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    var pTotal = new SqlParameter("@TotalCount", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(pTotal);

                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                            list.Add(MapReader(rdr));
                    }

                    if (pTotal.Value != DBNull.Value)
                        totalCount = Convert.ToInt32(pTotal.Value);
                }
            }
            catch (Exception ex)
            {
                
            }
            return list;
        }

        // ----- categories ------------------------------------------------
        public static List<KeyValuePair<string, int>> GetCategories()
        {
            var list = new List<KeyValuePair<string, int>>();
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand("dbo.usp_GetSearchCategories", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new KeyValuePair<string, int>(
                                rdr["Category"].ToString(),
                                Convert.ToInt32(rdr["Cnt"])));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return list;
        }

        // ----- crawl log -------------------------------------------------
        public static long StartCrawlLog(string crawlType)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(
                    "INSERT INTO dbo.SearchCrawlLog(CrawlType) " +
                    "VALUES(@t); SELECT SCOPE_IDENTITY();", conn))
                {
                    cmd.Parameters.AddWithValue("@t", crawlType ?? "Unknown");
                    conn.Open();
                    var o = cmd.ExecuteScalar();
                    return o == null ? 0 : Convert.ToInt64(o);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static void EndCrawlLog(long logId, int itemsCount, string errors)
        {
            if (logId <= 0) return;
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(
                    "UPDATE dbo.SearchCrawlLog " +
                    "   SET FinishedAt=GETDATE(), ItemsCount=@c, Errors=@e " +
                    " WHERE LogId=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@c", itemsCount);
                    cmd.Parameters.AddWithValue("@e",
                        (object)errors ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", logId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        // ----- helpers ---------------------------------------------------
        private static SearchIndexItem MapReader(IDataReader rdr)
        {
            return new SearchIndexItem
            {
                ItemId = Convert.ToInt64(rdr["ItemId"]),
                SourceType = rdr["SourceType"] as string,
                SourceKey = rdr["SourceKey"] as string,
                SiteId = (Guid)rdr["SiteId"],
                WebId = (Guid)rdr["WebId"],
                ListId = rdr["ListId"] == DBNull.Value
                                 ? (Guid?)null : (Guid)rdr["ListId"],
                ListItemId = rdr["ListItemId"] == DBNull.Value
                                 ? (int?)null : Convert.ToInt32(rdr["ListItemId"]),
                TitleAr = rdr["TitleAr"] as string,
                TitleEn = rdr["TitleEn"] as string,
                ContentAr = rdr["ContentAr"] as string,
                ContentEn = rdr["ContentEn"] as string,
                Url = rdr["Url"] as string,
                Category = rdr["Category"] as string,
                DisplayDate = rdr["DisplayDate"] == DBNull.Value
                                 ? (DateTime?)null : (DateTime)rdr["DisplayDate"],
                ModifiedDate = (DateTime)rdr["ModifiedDate"]
            };
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max);
        }
    }
}
