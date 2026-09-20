using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public static class SiteAuditSitemapGenerator
    {
        public class SitemapUrlEntry
        {
            public string Loc { get; set; }
            public DateTime LastMod { get; set; }
            public string ChangeFreq { get; set; } = "weekly";
            public string Priority { get; set; } = "0.8";
            public string AlternateAr { get; set; }
            public string AlternateEn { get; set; }
        }

        public static string GenerateSitemapXml(string baseUrl = "https://pnu.edu.sa")
        {
            List<SitemapUrlEntry> entries = GetSitemapEntriesFromDatabase(baseUrl);

            // Fallback: If database is empty or not yet indexed, fetch directly from SharePoint Publishing Webs
            if (entries.Count == 0)
            {
                entries = GetSitemapEntriesFromSharePoint(baseUrl);
            }

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = false
            };

            using (StringWriter sw = new StringWriter(sb))
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                writer.WriteStartDocument();
                writer.WriteComment(" Princess Nourah bint Abdulrahman University (PNU) - Dynamic XML Sitemap ");
                writer.WriteComment(string.Format(" Generated Automatically on {0:yyyy-MM-dd HH:mm:ss UTC} | Total URLs: {1} ", DateTime.UtcNow, entries.Count));

                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

                foreach (var entry in entries)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", entry.Loc);
                    writer.WriteElementString("lastmod", entry.LastMod.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    writer.WriteElementString("changefreq", entry.ChangeFreq);
                    writer.WriteElementString("priority", entry.Priority);

                    // Multilingual Alternates (Hreflang)
                    if (!string.IsNullOrEmpty(entry.AlternateAr))
                    {
                        writer.WriteStartElement("link", "http://www.w3.org/1999/xhtml");
                        writer.WriteAttributeString("rel", "alternate");
                        writer.WriteAttributeString("hreflang", "ar");
                        writer.WriteAttributeString("href", entry.AlternateAr);
                        writer.WriteEndElement();
                    }

                    if (!string.IsNullOrEmpty(entry.AlternateEn))
                    {
                        writer.WriteStartElement("link", "http://www.w3.org/1999/xhtml");
                        writer.WriteAttributeString("rel", "alternate");
                        writer.WriteAttributeString("hreflang", "en");
                        writer.WriteAttributeString("href", entry.AlternateEn);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement(); // </url>
                }

                writer.WriteEndElement(); // </urlset>
                writer.WriteEndDocument();
            }

            return sb.ToString();
        }

        public static List<SitemapUrlEntry> GetSitemapEntriesFromDatabase(string baseUrl)
        {
            List<SitemapUrlEntry> list = new List<SitemapUrlEntry>();
            HashSet<string> seenUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string connStr = SiteAuditReportDal.ConnectionString;
                    if (string.IsNullOrEmpty(connStr)) return;

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        // Query dbo.WebsitePages
                        string sql = @"
IF OBJECT_ID('dbo.WebsitePages', 'U') IS NOT NULL
BEGIN
    SELECT PageURL, PageURLEn, EnExists, LastIndexed
    FROM dbo.WebsitePages
    WHERE PageURL IS NOT NULL AND LTRIM(RTRIM(PageURL)) <> ''
    ORDER BY PageId ASC;
END";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string arUrl = reader.IsDBNull(0) ? string.Empty : reader.GetString(0).Trim();
                                string enUrl = reader.IsDBNull(1) ? string.Empty : reader.GetString(1).Trim();
                                bool enExists = !reader.IsDBNull(2) && reader.GetBoolean(2);
                                DateTime lastMod = reader.IsDBNull(3) ? DateTime.UtcNow : reader.GetDateTime(3);

                                if (IsValidPublishingUrl(arUrl))
                                {
                                    string fullArUrl = NormalizeFullUrl(arUrl, baseUrl);
                                    string fullEnUrl = (enExists && IsValidPublishingUrl(enUrl)) ? NormalizeFullUrl(enUrl, baseUrl) : null;

                                    if (seenUrls.Add(fullArUrl))
                                    {
                                        list.Add(new SitemapUrlEntry
                                        {
                                            Loc = fullArUrl,
                                            LastMod = lastMod,
                                            Priority = CalculatePriority(fullArUrl),
                                            ChangeFreq = CalculateChangeFreq(fullArUrl),
                                            AlternateAr = fullArUrl,
                                            AlternateEn = fullEnUrl
                                        });
                                    }

                                    // Also add English page as standalone indexable entry if exists
                                    if (!string.IsNullOrEmpty(fullEnUrl) && seenUrls.Add(fullEnUrl))
                                    {
                                        list.Add(new SitemapUrlEntry
                                        {
                                            Loc = fullEnUrl,
                                            LastMod = lastMod,
                                            Priority = CalculatePriority(fullEnUrl),
                                            ChangeFreq = CalculateChangeFreq(fullEnUrl),
                                            AlternateAr = fullArUrl,
                                            AlternateEn = fullEnUrl
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            });

            return list;
        }

        public static List<SitemapUrlEntry> GetSitemapEntriesFromSharePoint(string baseUrl)
        {
            List<SitemapUrlEntry> list = new List<SitemapUrlEntry>();
            HashSet<string> seenUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string rootUrl = SPContext.Current != null && SPContext.Current.Site != null
                        ? SPContext.Current.Site.Url
                        : baseUrl;

                    using (SPSite site = new SPSite(rootUrl))
                    {
                        foreach (SPWeb web in site.AllWebs)
                        {
                            try
                            {
                                if (!PublishingWeb.IsPublishingWeb(web)) continue;

                                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                                PublishingPageCollection pages = pubWeb.GetPublishingPages();

                                foreach (PublishingPage page in pages)
                                {
                                    if (page == null) continue;
                                    string pageName = page.Name ?? string.Empty;

                                    if (!pageName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) ||
                                        pageName.Equals("VariationRoot.aspx", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    string rawUrl = page.Uri != null ? page.Uri.ToString() : (web.Url + "/" + page.Name);
                                    string fullUrl = NormalizeFullUrl(rawUrl, baseUrl);

                                    if (seenUrls.Add(fullUrl))
                                    {
                                        DateTime lastMod = page.ListItem != null && page.ListItem["Modified"] != null
                                            ? Convert.ToDateTime(page.ListItem["Modified"])
                                            : DateTime.UtcNow;

                                        list.Add(new SitemapUrlEntry
                                        {
                                            Loc = fullUrl,
                                            LastMod = lastMod,
                                            Priority = CalculatePriority(fullUrl),
                                            ChangeFreq = CalculateChangeFreq(fullUrl)
                                        });
                                    }
                                }
                            }
                            catch { }
                            finally
                            {
                                web.Dispose();
                            }
                        }
                    }
                }
                catch { }
            });

            return list;
        }

        public static bool SaveSitemapToFile(string filePath, string xmlContent)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.WriteAllText(filePath, xmlContent, Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPublishingUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            if (!url.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)) return false;
            if (url.IndexOf("VariationRoot.aspx", StringComparison.OrdinalIgnoreCase) >= 0) return false;
            if (url.IndexOf("/_layouts/", StringComparison.OrdinalIgnoreCase) >= 0) return false;
            if (url.IndexOf("/_catalogs/", StringComparison.OrdinalIgnoreCase) >= 0) return false;
            if (url.IndexOf("/Forms/", StringComparison.OrdinalIgnoreCase) >= 0) return false;

            // Exclude administrative and internal portals
            if (url.IndexOf("/ITAdmin/", StringComparison.OrdinalIgnoreCase) >= 0) return false;
            if (url.IndexOf("/ContentAdmin/", StringComparison.OrdinalIgnoreCase) >= 0) return false;

            // Exclude archived news and announcements
            if (url.IndexOf("/Announcements/", StringComparison.OrdinalIgnoreCase) >= 0) return false;
            if (url.IndexOf("/NewsActivities/", StringComparison.OrdinalIgnoreCase) >= 0) return false;

            return true;
        }

        private static string NormalizeFullUrl(string url, string baseUrl)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                // Force HTTPS and consistent domain
                Uri uri = new Uri(url);
                return "https://" + (new Uri(baseUrl)).Host + uri.PathAndQuery;
            }

            if (!url.StartsWith("/")) url = "/" + url;
            return baseUrl.TrimEnd('/') + url;
        }

        private static string CalculatePriority(string url)
        {
            string lower = url.ToLowerInvariant();
            if (lower.EndsWith("/pages/home.aspx") || lower.EndsWith("/pages/default.aspx"))
            {
                if (lower.Contains("/ar/pages/") || lower.Contains("/en/pages/")) return "1.0";
                return "0.9";
            }
            if (lower.Contains("/faculties/") || lower.Contains("/deanship/") || lower.Contains("/agencies/"))
            {
                return "0.8";
            }
            if (lower.Contains("/newsactivities/") || lower.Contains("/announcements/"))
            {
                return "0.6";
            }
            return "0.7";
        }

        private static string CalculateChangeFreq(string url)
        {
            string lower = url.ToLowerInvariant();
            if (lower.Contains("/newsactivities/") || lower.Contains("/announcements/")) return "daily";
            if (lower.Contains("/pages/home.aspx")) return "daily";
            return "weekly";
        }
    }
}
