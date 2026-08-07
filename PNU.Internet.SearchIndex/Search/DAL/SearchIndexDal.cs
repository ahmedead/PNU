using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.DAL
{
    /// <summary>
    /// ADO.NET wrapper around PNU_SearchIndex. Every method runs inside
    /// SPSecurity.RunWithElevatedPrivileges so SQL is connected to under
    /// the application pool identity (avoids ANONYMOUS LOGON double-hop).
    /// </summary>
    public static class SearchIndexDal
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


        // ----- upsert ----------------------------------------------------
        public static void Upsert(SearchIndexItem item)
        {
            if (item == null) return;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
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
                        cmd.Parameters.AddWithValue("@CategoryAr",
                            (object)Truncate(item.CategoryAr, 200) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CategoryEn",
                            (object)Truncate(item.CategoryEn, 200) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DisplayDate",
                            (object)item.DisplayDate ?? DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("DAL",
                        item.SourceKey ?? "", ex.Message);
                }
            });
        }

        public static void Deactivate(string sourceKey)
        {
            if (string.IsNullOrEmpty(sourceKey)) return;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
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
                    SearchLogger.WriteToLog("DAL",
                        "Deactivate " + sourceKey, ex.Message);
                }
            });
        }

        public static List<SearchIndexItem> Search(
            string keyword, bool isArabic, string categoriesCsv,
            string sortBy, int pageIndex, int pageSize, out int totalCount)
        {
            int total = 0;
            var list = new List<SearchIndexItem>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    using (var cmd = new SqlCommand("dbo.usp_SearchItems", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60;
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
                            total = Convert.ToInt32(pTotal.Value);
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("DAL", "Search", ex.Message);
                }
            });

            totalCount = total;
            return list;
        }

        public static List<CategoryRow> GetCategories()
        {
            var list = new List<CategoryRow>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
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
                                list.Add(new CategoryRow
                                {
                                    CategoryAr = rdr["CategoryAr"] as string ?? "",
                                    CategoryEn = rdr["CategoryEn"] as string ?? "",
                                    Count = Convert.ToInt32(rdr["Cnt"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("DAL", "GetCategories", ex.Message);
                }
            });
            return list;
        }

        public static long StartCrawlLog(string crawlType)
        {
            long id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
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
                        if (o != null) id = Convert.ToInt64(o);
                    }
                }
                catch (Exception ex)
                {
                    SearchLogger.WriteToLog("DAL",
                        "StartCrawlLog " + crawlType, ex.Message);
                }
            });
            return id;
        }

        public static void EndCrawlLog(long logId, int itemsCount, string errors)
        {
            if (logId <= 0) return;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
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
                    SearchLogger.WriteToLog("DAL",
                        "EndCrawlLog " + logId, ex.Message);
                }
            });
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
                CategoryAr = rdr["CategoryAr"] as string,
                CategoryEn = rdr["CategoryEn"] as string,
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
