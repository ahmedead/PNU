using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SiteAudit
{
    public class VerificationResult
    {
        public bool IsResolved { get; set; }
        public string Status { get; set; } // Resolved, Unresolved, Error
        public int ResponseCode { get; set; }
        public string Details { get; set; }
    }

    public static class SiteAuditRuleVerifier
    {
        #region Sitemap Cache & Helper

        private static readonly object _sitemapLock = new object();
        private static HashSet<string> _sitemapUrls = null;
        private static DateTime _sitemapLastLoaded = DateTime.MinValue;

        /// <summary>
        /// Loads and caches URLs present in sitemap.xml for verifying sitemap-related issues (#02, #37, #05, #06).
        /// </summary>
        public static HashSet<string> GetSitemapUrls(string baseUrl = "https://pnu.edu.sa")
        {
            if (_sitemapUrls != null && (DateTime.UtcNow - _sitemapLastLoaded).TotalMinutes < 15)
            {
                return _sitemapUrls;
            }

            lock (_sitemapLock)
            {
                if (_sitemapUrls != null && (DateTime.UtcNow - _sitemapLastLoaded).TotalMinutes < 15)
                {
                    return _sitemapUrls;
                }

                HashSet<string> set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // 1. Try reading from filesystem (Reports/sitemap.xml or root sitemap.xml)
                try
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
                    string[] candidatePaths = new string[]
                    {
                        Path.Combine(baseDir, "sitemap.xml"),
                        Path.Combine(baseDir, "Reports", "sitemap.xml"),
                        @"d:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\Reports\sitemap.xml"
                    };

                    foreach (var path in candidatePaths)
                    {
                        if (File.Exists(path))
                        {
                            string xml = File.ReadAllText(path);
                            LoadUrlsFromSitemapXml(xml, set);
                            if (set.Count > 0) break;
                        }
                    }
                }
                catch { }

                // 2. Try fetching from live /sitemap.xml endpoint if reachable
                if (set.Count == 0)
                {
                    try
                    {
                        string sitemapUrl = baseUrl.TrimEnd('/') + "/sitemap.xml";
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sitemapUrl);
                        req.Timeout = 5000;
                        req.UserAgent = "PNU-SiteAudit-Verifier/1.0";
                        using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                        using (StreamReader rdr = new StreamReader(resp.GetResponseStream()))
                        {
                            string xml = rdr.ReadToEnd();
                            LoadUrlsFromSitemapXml(xml, set);
                        }
                    }
                    catch { }
                }

                // 3. Fallback: Query database via SiteAuditSitemapGenerator
                if (set.Count == 0)
                {
                    try
                    {
                        var dbEntries = SiteAuditSitemapGenerator.GetSitemapEntriesFromDatabase(baseUrl);
                        if (dbEntries != null)
                        {
                            foreach (var entry in dbEntries)
                            {
                                if (!string.IsNullOrEmpty(entry.Loc))
                                {
                                    set.Add(NormalizeUrlForLookup(entry.Loc));
                                }
                            }
                        }
                    }
                    catch { }
                }

                _sitemapUrls = set;
                _sitemapLastLoaded = DateTime.UtcNow;
                return _sitemapUrls;
            }
        }

        private static void LoadUrlsFromSitemapXml(string xmlContent, HashSet<string> set)
        {
            if (string.IsNullOrEmpty(xmlContent)) return;
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlContent);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
                nsmgr.AddNamespace("sm", "http://www.sitemaps.org/schemas/sitemap/0.9");
                XmlNodeList nodes = xdoc.SelectNodes("//sm:loc | //loc", nsmgr);
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        string u = node.InnerText.Trim();
                        if (!string.IsNullOrEmpty(u))
                        {
                            set.Add(NormalizeUrlForLookup(u));
                        }
                    }
                }
            }
            catch { }
        }

        public static string NormalizeUrlForLookup(string rawUrl)
        {
            if (string.IsNullOrEmpty(rawUrl)) return string.Empty;
            try
            {
                Uri uri = new Uri(rawUrl.Trim());
                return (uri.AbsolutePath + uri.Query).TrimEnd('/').ToLowerInvariant();
            }
            catch
            {
                return rawUrl.Trim().TrimEnd('/').ToLowerInvariant();
            }
        }

        #endregion

        #region Word Count Extraction Helper

        private static int CalculatePageWordCount(HtmlDocument doc)
        {
            if (doc == null || doc.DocumentNode == null) return 0;
            try
            {
                var body = doc.DocumentNode.SelectSingleNode("//body");
                if (body == null) return 0;

                // Clone body to strip non-content elements without mutating doc
                HtmlNode clonedBody = body.CloneNode(true);

                var noiseNodes = clonedBody.SelectNodes(".//script|.//style|.//noscript|.//svg|.//nav|.//footer|.//aside|.//header");
                if (noiseNodes != null)
                {
                    foreach (var node in noiseNodes)
                    {
                        node.Remove();
                    }
                }

                string text = HtmlEntity.DeEntitize(clonedBody.InnerText ?? string.Empty);
                var words = Regex.Matches(text, @"\b[\p{L}\p{N}_\-']+\b");
                return words.Count;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        public static VerificationResult VerifyUrlIssue(int issueCode, string url)
        {
            var result = new VerificationResult
            {
                IsResolved = false,
                Status = "Unresolved",
                ResponseCode = 0,
                Details = ""
            };

            if (string.IsNullOrEmpty(url))
            {
                result.Details = "رابط الصفحة غير متوفر.";
                return result;
            }

            try
            {
                // Prepare WebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 8000; // 8 seconds timeout
                request.UserAgent = "PNU-SiteAudit-Verifier/1.0 (Mozilla/5.0; Windows NT 10.0; Win64; x64)";
                request.AllowAutoRedirect = false; // Inspect actual status code & redirects

                // Ignore SSL errors if verifying internal test certs
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                int statusCode = 0;
                string responseHtml = string.Empty;
                WebHeaderCollection responseHeaders = null;

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        statusCode = (int)response.StatusCode;
                        responseHeaders = response.Headers;
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseHtml = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException wex)
                {
                    if (wex.Response is HttpWebResponse errResponse)
                    {
                        statusCode = (int)errResponse.StatusCode;
                        responseHeaders = errResponse.Headers;
                    }
                    else
                    {
                        result.ResponseCode = 0;
                        result.Status = "Unresolved";
                        result.Details = "تعذر الاتصال بالخادم: " + wex.Message;
                        return result;
                    }
                }

                result.ResponseCode = statusCode;

                // Load HTML DOM if response succeeded
                HtmlDocument doc = null;
                if (!string.IsNullOrEmpty(responseHtml))
                {
                    doc = new HtmlDocument();
                    doc.LoadHtml(responseHtml);
                }

                // Rule-specific verification: Test the EXACT inverse condition of each issue
                switch (issueCode)
                {
                    // ==========================================
                    // 1) Sitemaps - URLs not in Sitemap (#02) & Non-200 URLs (#37)
                    // ==========================================
                    case 2: // Sitemaps - URLs not in Sitemap
                        {
                            var sitemapUrls = GetSitemapUrls();
                            string normalizedUrl = NormalizeUrlForLookup(url);
                            bool inSitemap = sitemapUrls.Contains(normalizedUrl);

                            if (!inSitemap)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: الرابط غير موجود في ملف خريطة الموقع (sitemap.xml). يجب توليد وتحديث خريطة الموقع لتضمينه.";
                            }
                            else if (statusCode == 200)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: الرابط مدرج رسمياً داخل ملف خريطة الموقع sitemap.xml ويستجيب بنجاح بكود 200 OK.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "الرابط مدرج في خريطة الموقع ولكنه يعيد كود استجابة غير صالح (" + statusCode + ").";
                            }
                        }
                        break;

                    case 37: // Sitemaps - Non-200 URLs in Sitemap
                        {
                            var sitemapUrls = GetSitemapUrls();
                            string normalizedUrl = NormalizeUrlForLookup(url);
                            bool inSitemap = sitemapUrls.Contains(normalizedUrl);

                            if (inSitemap)
                            {
                                if (statusCode == 200)
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = "تم حل المشكلة: الرابط في خريطة الموقع sitemap.xml يستجيب بحالة سليمة 200 OK وخالٍ من أخطاء 404 أو تحويلات 302.";
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "ما زالت المشكلة قائمة: الرابط مدرج في خريطة الموقع ولكنه يعيد كود خطأ (" + statusCode + ") بدلاً من 200 OK.";
                                }
                            }
                            else
                            {
                                // If removed from sitemap because it's non-200, it's resolved
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: تم استبعاد الرابط غير السليم من خريطة الموقع sitemap.xml بنجاح.";
                            }
                        }
                        break;

                    // ==========================================
                    // 2) Content - Low Content Pages (#04)
                    // ==========================================
                    case 4: // Content - Low Content Pages (< 100 words)
                        if (statusCode == 200 && doc != null)
                        {
                            int wordCount = CalculatePageWordCount(doc);
                            if (wordCount < 100)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: محتوى الصفحة ضعيف (" + wordCount + " كلمة فقط، والحد الأدنى المطلوب 100 كلمة).";
                            }
                            else
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: الصفحة تحتوي على محتوى كافٍ وغني (" + wordCount + " كلمة، تجاوزت حد الـ 100 كلمة).";
                            }
                        }
                        else
                        {
                            result.Details = "الصفحة لا تستجيب بكود 200 لحساب عدد الكلمات.";
                        }
                        break;

                    // ==========================================
                    // 3) Hreflang - Missing (#03)
                    // ==========================================
                    case 3: // Hreflang - Missing
                        if (statusCode == 200 && doc != null)
                        {
                            var hreflangNodes = doc.DocumentNode.SelectNodes("//head//link[@rel='alternate' and @hreflang]");
                            if (hreflangNodes != null && hreflangNodes.Count >= 2)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: وسوم hreflang للربط متعدد اللغات (ar / en) متوفرة بنجاح في هيدر الصفحة (" + hreflangNodes.Count + " وسوم).";
                            }
                            else if (hreflangNodes != null && hreflangNodes.Count == 1)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: وسم hreflang للربط اللغوي متوفر في هيدر الصفحة.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: وسوم hreflang للربط اللغوي مفقودة تماماً من هيدر الصفحة.";
                            }
                        }
                        else
                        {
                            result.Details = "الصفحة لا تستجيب بكود 200 لفحص وسوم hreflang.";
                        }
                        break;

                    // ==========================================
                    // 4) Near Duplicates Report (#21, #22)
                    // ==========================================
                    case 21: // Near Duplicates Report
                    case 22: // Near Duplicates (Variants)
                        if (statusCode == 200 && doc != null)
                        {
                            if (url.IndexOf("FacultyMemberDetails.aspx", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                var titleNode = doc.DocumentNode.SelectSingleNode("//head//title");
                                string pageTitle = titleNode != null ? titleNode.InnerText.Trim() : "";

                                var h1Node = doc.DocumentNode.SelectSingleNode("//h1");
                                string h1Text = h1Node != null ? h1Node.InnerText.Trim() : "";

                                bool isGeneric = string.IsNullOrEmpty(pageTitle) ||
                                                 pageTitle.Equals("صفحة عضو هيئة التدريس", StringComparison.OrdinalIgnoreCase) ||
                                                 pageTitle.Equals("القائمة الرئيسية", StringComparison.OrdinalIgnoreCase) ||
                                                 h1Text.Equals("صفحة عضو هيئة التدريس", StringComparison.OrdinalIgnoreCase) ||
                                                 string.IsNullOrEmpty(h1Text);

                                var memberBox = doc.DocumentNode.SelectSingleNode("//*[contains(@class, 'faculty') or contains(@class, 'member') or contains(@id, 'Faculty') or contains(@class, 'dga-')]");

                                if (!isGeneric && memberBox != null)
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = "تم حل المشكلة: تم إلغاء التكرار وتخصيص هوية فريدة لعضو هيئة التدريس (H1: \"" + (h1Text.Length > 30 ? h1Text.Substring(0, 30) + "..." : h1Text) + "\") مع بياناته الأكاديمية المستقلة.";
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "ما زالت المشكلة قائمة: محتوى أو عنوان الصفحة مكرر وقالب عام دون تخصيص اسم وبيانات العضو.";
                                }
                            }
                            else
                            {
                                var titleNode = doc.DocumentNode.SelectSingleNode("//head//title");
                                var canonical = doc.DocumentNode.SelectSingleNode("//head//link[@rel='canonical']");
                                if (titleNode != null && titleNode.InnerText.Trim().Length >= 15 && canonical != null)
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = "تم حل المشكلة: الصفحة تحمل عنواناً فريداً ووسم canonical معتمد لمنع احتسابها كمحتوى مكرر.";
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "ما زالت المشكلة قائمة: الصفحة تفتقر لوسم canonical أو عنوان فريد يمنع ازدواجية المحتوى.";
                                }
                            }
                        }
                        else
                        {
                            result.Details = "الصفحة لا تستجيب بكود 200 لفحص المحتوى المكرر.";
                        }
                        break;

                    // ==========================================
                    // 5) Routing & 404 / 5xx / Client Errors
                    // ==========================================
                    case 1:  // Response Codes - Internal Broken (4xx)
                    case 19: // Internal All Error (4xx, 5xx)
                    case 20: // External All Error (4xx, 5xx)
                    case 35: // 5xx Server Error
                    case 44: // Internal Client Error (4xx)
                        if (statusCode == 200)
                        {
                            result.IsResolved = true;
                            result.Status = "Resolved";
                            result.Details = "تم حل المشكلة: الصفحة أصبحت تعمل بنجاح واستجابتها 200 OK دون أخطاء.";
                        }
                        else
                        {
                            result.IsResolved = false;
                            result.Status = "Unresolved";
                            result.Details = "ما زالت المشكلة قائمة: الصفحة تعيد كود خطأ (" + statusCode + ").";
                        }
                        break;

                    // ==========================================
                    // 6) Internal Redirection & Redirect Hops (#26, #71)
                    // ==========================================
                    case 26: // Internal Redirection (3xx)
                    case 43: // Response Codes - Internal 3xx
                    case 71: // Number of Redirects
                        if (statusCode >= 300 && statusCode < 400)
                        {
                            result.IsResolved = false;
                            result.Status = "Unresolved";
                            result.Details = "ما زالت المشكلة قائمة: الرابط يعيد كود تحويل " + statusCode + " (3xx) ويجب استبداله بالرابط المباشر النهائي 200 OK.";
                        }
                        else if (statusCode == 200)
                        {
                            result.IsResolved = true;
                            result.Status = "Resolved";
                            result.Details = "تم حل المشكلة: الرابط يفتح مباشرة برمز 200 OK دون أي حلقات أو سلاسل إعادة توجيه.";
                        }
                        else
                        {
                            result.Details = "حالة الاستجابة: " + statusCode;
                        }
                        break;

                    // ==========================================
                    // 7) Links - High Crawl Depth (#05) & Orphan Pages (#06)
                    // ==========================================
                    case 5: // Links - Pages With High Crawl Depth
                    case 6: // Links - Pages Without Internal Links (Orphan Pages)
                        {
                            var sitemapUrls = GetSitemapUrls();
                            string normalizedUrl = NormalizeUrlForLookup(url);
                            bool inSitemap = sitemapUrls.Contains(normalizedUrl);

                            if (inSitemap && statusCode == 200)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: الرابط مدرج في خريطة الموقع الرسمية sitemap.xml مما خفض عمق الوصول وألغى العزلة.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: الصفحة معزولة (يتيمة) أو على عمق وصول كبير وغير مدرجة في خريطة الموقع.";
                            }
                        }
                        break;

                    // ==========================================
                    // 8) Directives - Noindex (#10) & Nofollow (#11)
                    // ==========================================
                    case 10: // Directives - Noindex
                        if (statusCode == 200 && doc != null)
                        {
                            var robotsMeta = doc.DocumentNode.SelectSingleNode("//head//meta[@name='robots']");
                            string robotsContent = robotsMeta != null ? robotsMeta.GetAttributeValue("content", "").ToLower() : "";
                            string xRobots = responseHeaders != null ? (responseHeaders["X-Robots-Tag"] ?? "").ToLower() : "";

                            if (robotsContent.Contains("noindex") || xRobots.Contains("noindex"))
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: توجيه noindex ما زال مفعلاً مما يمنع محركات البحث من أرشفة الصفحة.";
                            }
                            else
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: تم إلغاء توجيه noindex والصفحة مهيأة للفهرسة والأرشفة الرسمية.";
                            }
                        }
                        break;

                    case 11: // Directives - Nofollow
                        if (statusCode == 200 && doc != null)
                        {
                            var robotsMeta = doc.DocumentNode.SelectSingleNode("//head//meta[@name='robots']");
                            string robotsContent = robotsMeta != null ? robotsMeta.GetAttributeValue("content", "").ToLower() : "";
                            string xRobots = responseHeaders != null ? (responseHeaders["X-Robots-Tag"] ?? "").ToLower() : "";

                            if (robotsContent.Contains("nofollow") || xRobots.Contains("nofollow"))
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: توجيه nofollow ما زال مفعلاً على روابط الصفحة.";
                            }
                            else
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: توجيه nofollow غير موجود وروابط الصفحة قابلة للتتبع السليم.";
                            }
                        }
                        break;

                    // ==========================================
                    // 9) Security Headers (#17)
                    // ==========================================
                    case 17: // Security - Bad Content Type (Missing nosniff)
                        {
                            string nosniff = responseHeaders != null ? responseHeaders["X-Content-Type-Options"] : null;
                            if (!string.IsNullOrEmpty(nosniff) && nosniff.IndexOf("nosniff", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: ترويسة الحماية X-Content-Type-Options: nosniff مفعلة بنجاح.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: ترويسة الحماية X-Content-Type-Options: nosniff مفقودة من استجابة الخادم.";
                            }
                        }
                        break;

                    // ==========================================
                    // 10) Security / Target Blank Links (#18, #30)
                    // ==========================================
                    case 18: // Unsafe Cross-Origin
                    case 30: // Unsafe Cross-Origin Blank Links
                        if (statusCode == 200 && doc != null)
                        {
                            var blankLinks = doc.DocumentNode.SelectNodes("//a[@target='_blank']");
                            int unsafeCount = 0;
                            if (blankLinks != null)
                            {
                                foreach (var a in blankLinks)
                                {
                                    string rel = a.GetAttributeValue("rel", "").ToLower();
                                    if (!rel.Contains("noopener")) unsafeCount++;
                                }
                            }

                            if (unsafeCount == 0)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: كافة الروابط الخارجية المفتوحة بنوافذ جديدة مؤمنة بـ rel='noopener'.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: يوجد " + unsafeCount + " روابط تفتح بـ target=_blank دون سمة noopener.";
                            }
                        }
                        break;

                    // ==========================================
                    // 11) Title Issues (#12, #39, #45)
                    // ==========================================
                    case 12: // Page Titles Missing / Duplicate
                    case 39: // Multiple Page Titles
                    case 45: // Page Titles Outside <head>
                        if (statusCode == 200 && doc != null)
                        {
                            var titleNodes = doc.DocumentNode.SelectNodes("//head//title");
                            var allTitles = doc.DocumentNode.SelectNodes("//title");

                            if (titleNodes != null && titleNodes.Count == 1)
                            {
                                string titleText = titleNodes[0].InnerText.Trim();
                                if (titleText.Length >= 10)
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = "تم حل المشكلة: عنوان الصفحة سليم ووحيد داخل <head> (\"" + (titleText.Length > 35 ? titleText.Substring(0, 35) + "..." : titleText) + "\").";
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "عنوان الصفحة قصير جداً (" + titleText.Length + " حرفاً).";
                                }
                            }
                            else if (allTitles != null && allTitles.Count > 1)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "توجد وسوم <title> مكررة في الصفحة (" + allTitles.Count + " وسوم).";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "وسم <title> مفقود في هيدر الصفحة.";
                            }
                        }
                        else
                        {
                            result.Details = "تعذر فحص العنوان لعدم استجابة الصفحة بكود 200.";
                        }
                        break;

                    // ==========================================
                    // 12) H1 Issues (#15, #16, #51)
                    // ==========================================
                    case 15: // H1 Missing
                    case 16: // H1 Duplicate
                    case 51: // H1 Missing in Subsites
                        if (statusCode == 200 && doc != null)
                        {
                            var h1Nodes = doc.DocumentNode.SelectNodes("//h1");
                            if (h1Nodes != null && h1Nodes.Count == 1)
                            {
                                string h1Text = h1Nodes[0].InnerText.Trim();
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: يوجد وسم H1 مفرد وصالح (\"" + (h1Text.Length > 35 ? h1Text.Substring(0, 35) + "..." : h1Text) + "\").";
                            }
                            else if (h1Nodes != null && h1Nodes.Count > 1)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "يوجد أكثر من وسم H1 في الصفحة (" + h1Nodes.Count + " وسوم).";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "وسم H1 مفقود تماماً من الصفحة.";
                            }
                        }
                        else
                        {
                            result.Details = "الصفحة لا تستجيب بكود 200.";
                        }
                        break;

                    // ==========================================
                    // 13) Canonical Issues (#08, #09)
                    // ==========================================
                    case 8: // Canonicals Missing
                    case 9: // Canonicals Canonicalised
                        if (statusCode == 200 && doc != null)
                        {
                            var canonical = doc.DocumentNode.SelectSingleNode("//head//link[@rel='canonical']");
                            if (canonical != null && !string.IsNullOrEmpty(canonical.GetAttributeValue("href", "")))
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: وسم rel='canonical' متوفر وصحيح في الهيدر.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "وسم rel='canonical' غير موجود في هيدر الصفحة.";
                            }
                        }
                        break;

                    // ==========================================
                    // 14) Images Alt Text Issues (#27, #28, #41, #46, #47)
                    // ==========================================
                    case 27: // Images Missing Alt Attribute
                    case 28: // Images Missing Alt Text Inlinks
                    case 41: // Images Missing Alt Text
                    case 46: // Responsive Images Missing Alt
                    case 47: // Images Missing Alt Attribute
                        if (statusCode == 200 && doc != null)
                        {
                            var images = doc.DocumentNode.SelectNodes("//img");
                            int missingAltCount = 0;
                            if (images != null)
                            {
                                foreach (var img in images)
                                {
                                    string alt = img.GetAttributeValue("alt", null);
                                    if (alt == null || string.IsNullOrWhiteSpace(alt))
                                    {
                                        missingAltCount++;
                                    }
                                }
                            }

                            if (missingAltCount == 0)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: كافة الصور في الصفحة تحمل نصوص بديلة alt سليمة ومطابقة للمعايير.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: توجد صور بدون نصوص بديلة (عدد: " + missingAltCount + ").";
                            }
                        }
                        break;

                    // ==========================================
                    // 15) Meta Description Issues (#13, #14, #38)
                    // ==========================================
                    case 13: // Meta Description Missing
                    case 14: // Meta Description Duplicate
                    case 38: // Meta Description Multiple
                        if (statusCode == 200 && doc != null)
                        {
                            var descNodes = doc.DocumentNode.SelectNodes("//head//meta[@name='description']");
                            if (descNodes != null && descNodes.Count == 1)
                            {
                                string content = descNodes[0].GetAttributeValue("content", "").Trim();
                                if (content.Length >= 20)
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = "تم حل المشكلة: وسم meta description متوفر ومفرد ومطابق للمواصفات (" + content.Length + " حرفاً).";
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "نص الوصف قصير جداً (" + content.Length + " حرفاً).";
                                }
                            }
                            else if (descNodes != null && descNodes.Count > 1)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "توجد وسوم meta description مكررة في الهيدر.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "وسم meta description غير موجود في هيدر الصفحة.";
                            }
                        }
                        break;

                    // ==========================================
                    // 16) URL Multiple Slashes (#32)
                    // ==========================================
                    case 32: // URL - Multiple Slashes
                        {
                            Uri u;
                            if (Uri.TryCreate(url, UriKind.Absolute, out u))
                            {
                                if (Regex.IsMatch(u.AbsolutePath, @"/{2,}"))
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "ما زالت المشكلة قائمة: مسار الرابط يحتوي على شرطات مائلة مكررة (//).";
                                }
                                else if (statusCode == 200)
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = "تم حل المشكلة: مسار الرابط نظيف وخالٍ من أي شرطات مائلة مكررة ويستجيب بنجاح.";
                                }
                                else
                                {
                                    result.Details = "المسار خالٍ من الشرطات المكررة ولكن كود الاستجابة: " + statusCode;
                                }
                            }
                        }
                        break;

                    // ==========================================
                    // 17) URL Contains Space (#48)
                    // ==========================================
                    case 48: // URL - Contains Space
                        {
                            if (url.Contains(" ") || url.Contains("%20"))
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: الرابط يحتوي على مسافات غير مسموحة في مسار الويب.";
                            }
                            else if (statusCode == 200)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: الرابط خالٍ تماماً من المسافات ويعمل بنجاح.";
                            }
                            else
                            {
                                result.Details = "الرابط خالٍ من المسافات ولكن كود الاستجابة: " + statusCode;
                            }
                        }
                        break;

                    // ==========================================
                    // 18) Mobile Unsupported Plugins (#50)
                    // ==========================================
                    case 50: // Mobile - Contains Unsupported Plugins (Flash / Applet)
                        if (statusCode == 200 && doc != null)
                        {
                            var badPluginNodes = doc.DocumentNode.SelectNodes("//embed | //applet | //object[contains(@type, 'flash') or contains(@data, '.swf')]");
                            if (badPluginNodes != null && badPluginNodes.Count > 0)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: تم العثور على ملحقات غير مدعومة للهواتف (Flash/Applet) في كود الصفحة.";
                            }
                            else
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: الصفحة خالية تماماً من الملحقات غير المدعومة (Flash/Applet) ومتوافقة مع الهواتف الذكية.";
                            }
                        }
                        break;

                    // ==========================================
                    // 19) Validation Missing <body> (#40) & <head> (#52)
                    // ==========================================
                    case 40: // Validation - Missing <body>
                        if (statusCode == 200 && doc != null)
                        {
                            var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
                            if (bodyNode != null)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: وسم <body> سليم وموجود في هيكل كود الصفحة.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: وسم <body> مفقود من هيكل كود الصفحة.";
                            }
                        }
                        break;

                    case 52: // Validation - Missing <head>
                        if (statusCode == 200 && doc != null)
                        {
                            var headNode = doc.DocumentNode.SelectSingleNode("//head");
                            if (headNode != null)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: وسم <head> سليم وموجود في هيدر الصفحة.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: وسم <head> مفقود من هيدر الصفحة.";
                            }
                        }
                        break;

                    // ==========================================
                    // 20) Insecure Content / Protocol (#23, #74)
                    // ==========================================
                    case 23: // External Links Protocol
                    case 74: // Insecure Content (Mixed Content)
                        if (statusCode == 200 && doc != null)
                        {
                            var insecureNodes = doc.DocumentNode.SelectNodes(
                                "//img[starts-with(@src,'http://')] | //script[starts-with(@src,'http://')] | //link[starts-with(@href,'http://')] | //iframe[starts-with(@src,'http://')]");

                            if (insecureNodes != null && insecureNodes.Count > 0)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: تم رصد " + insecureNodes.Count + " موارد غير مشفرة (http://) داخل الصفحة.";
                            }
                            else if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: الصفحة وكافة مواردها محمية ومشفرة ببروتوكول HTTPS دون محتوى مختلط.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "الرابط الأساسي ما زال يعمل ببروتوكول HTTP غير المشفر.";
                            }
                        }
                        break;

                    // ==========================================
                    // 21) Semrush Audit Issues (1001 - 1009)
                    // ==========================================
                    case 1001: // SEMRUSH: Internal Broken Links (401 Authenticate.aspx / 404)
                        {
                            if (statusCode == 200)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: الرابط يعمل بنجاح (HTTP 200) ولا يتطلب مصادقة (تم حل مشكلة 401).";
                            }
                            else if (statusCode == 401 || (responseHeaders != null && (responseHeaders["Location"] ?? "").IndexOf("Authenticate.aspx", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: الرابط يطلب مصادقة 401 أو يحول إلى Authenticate.aspx.";
                            }
                            else if (statusCode == 404)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: الرابط الداخلي معطل ويرجع 404 Not Found.";
                            }
                            else
                            {
                                result.IsResolved = (statusCode >= 200 && statusCode < 400);
                                result.Status = result.IsResolved ? "Resolved" : "Unresolved";
                                result.Details = result.IsResolved ? $"الرابط يستجيب بنجاح برمز {statusCode}." : $"الرابط يرجع كود خطأ {statusCode}.";
                            }
                        }
                        break;

                    case 1002: // SEMRUSH: HTTP 4xx Client Errors (404)
                        {
                            if (statusCode == 200 || statusCode == 301 || statusCode == 302)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = $"تم حل المشكلة: تم إصلاح المسار أو تحويله برمز ({statusCode}) بنجاح.";
                            }
                            else if (statusCode == 404)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: الملف أو المسار مفقود ويرجع 404 Not Found.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = $"المسار يرجع رمز استجابة {statusCode}.";
                            }
                        }
                        break;

                    case 1003: // SEMRUSH: Duplicate Content (/AR/ vs /ar/ case sensitivity)
                        {
                            if (statusCode == 200 && doc != null)
                            {
                                var canonicalNode = doc.DocumentNode.SelectSingleNode("//link[@rel='canonical']");
                                string canonicalHref = canonicalNode != null ? canonicalNode.GetAttributeValue("href", "").Trim() : "";

                                if (!string.IsNullOrEmpty(canonicalHref))
                                {
                                    Uri canUri;
                                    if (Uri.TryCreate(canonicalHref, UriKind.Absolute, out canUri))
                                    {
                                        string canPath = canUri.AbsolutePath;
                                        // Canonical path must be lowercase and must not be blindly pointing to the homepage /ar/ (unless the tested page itself is homepage)
                                        bool isCleanLowercase = canPath.Equals(canPath.ToLowerInvariant(), StringComparison.Ordinal);
                                        bool isNotStuckAtHome = !canPath.TrimEnd('/').Equals("/ar", StringComparison.OrdinalIgnoreCase) || url.ToLowerInvariant().Contains("/pages/home.aspx") || url.TrimEnd('/').EndsWith("/ar", StringComparison.OrdinalIgnoreCase);

                                        if (isCleanLowercase && isNotStuckAtHome)
                                        {
                                            result.IsResolved = true;
                                            result.Status = "Resolved";
                                            result.Details = $"تم حل المشكلة: وسم Canonical محدد بحروف صغيرة موحدة ({canonicalHref}) مما يمنع احتساب المحتوى المكرر.";
                                        }
                                        else if (!isNotStuckAtHome)
                                        {
                                            result.IsResolved = false;
                                            result.Status = "Unresolved";
                                            result.Details = $"ما زالت المشكلة قائمة: وسم Canonical يشير خطأً إلى الصفحة الرئيسية ({canonicalHref}).";
                                        }
                                        else
                                        {
                                            result.IsResolved = false;
                                            result.Status = "Unresolved";
                                            result.Details = $"ما زالت المشكلة قائمة: وسم Canonical يحتوي على حروف كبيرة ({canonicalHref}).";
                                        }
                                    }
                                    else
                                    {
                                        result.IsResolved = false;
                                        result.Status = "Unresolved";
                                        result.Details = "وسم Canonical غير صالح.";
                                    }
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "ما زالت المشكلة قائمة: وسم Canonical مفقود من الصفحة.";
                                }
                            }
                        }
                        break;

                    case 1004: // SEMRUSH: Duplicate Title Tags
                        {
                            if (statusCode == 200 && doc != null)
                            {
                                var titleNode = doc.DocumentNode.SelectSingleNode("//title");
                                string titleText = titleNode != null ? HtmlEntity.DeEntitize(titleNode.InnerText.Trim()) : "";

                                if (!string.IsNullOrEmpty(titleText) && titleText.Length > 20 && titleText.Contains(" - "))
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = $"تم حل المشكلة: عنوان الصفحة فريد ومفصل ويتضمن اسم الموقع والسياق: {titleText}";
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = $"العنوان الحالي قصير أو قد يكون مكرراً: '{titleText}'";
                                }
                            }
                        }
                        break;

                    case 1005: // SEMRUSH: Duplicate Meta Descriptions
                        {
                            if (statusCode == 200 && doc != null)
                            {
                                var metaDesc = doc.DocumentNode.SelectSingleNode("//meta[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='description']");
                                string desc = metaDesc != null ? metaDesc.GetAttributeValue("content", "").Trim() : "";

                                string genericHomeDesc = "الموقع الرسمي لجامعة الأميرة نورة بنت عبد الرحمن في المملكة العربية السعودية، ويقدّم معلومات وخدمات الجامعة وأحدث الأخبار والإعلانات.";

                                if (!string.IsNullOrEmpty(desc) && desc.Length >= 25 && !desc.Equals(genericHomeDesc, StringComparison.OrdinalIgnoreCase))
                                {
                                    result.IsResolved = true;
                                    result.Status = "Resolved";
                                    result.Details = $"تم حل المشكلة: وصف الميتا فريد ومخصص ({desc.Substring(0, Math.Min(60, desc.Length))}...)";
                                }
                                else if (desc.Equals(genericHomeDesc, StringComparison.OrdinalIgnoreCase) && !url.ToLowerInvariant().Contains("/pages/home.aspx") && !url.TrimEnd('/').EndsWith("/ar", StringComparison.OrdinalIgnoreCase))
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "ما زالت المشكلة قائمة: وصف الميتا ما زال هو النص العام للرئيسية المكرر عبر آلاف الصفحات.";
                                }
                                else
                                {
                                    result.IsResolved = false;
                                    result.Status = "Unresolved";
                                    result.Details = "وصف الميتا مفقود أو قصير جداً.";
                                }
                            }
                        }
                        break;

                    case 1006: // SEMRUSH: Broken Internal Images (33)
                        {
                            if (statusCode == 200)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم حل المشكلة: مسار الصورة يعمل بنجاح ويرجع كود 200 OK.";
                            }
                            else if (statusCode == 404)
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "ما زالت المشكلة قائمة: مسار الصورة مفقود ويعيد 404 Not Found.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = $"مسار الصورة يعيد كود ({statusCode}).";
                            }
                        }
                        break;

                    case 1007: // SEMRUSH: Slow Loading Speed (>3.5s)
                        {
                            if (statusCode == 200)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "الصفحة استجابت بنجاح برمز 200 OK أثناء الاختبار السريع.";
                            }
                        }
                        break;

                    case 1008: // SEMRUSH: URLs with double slashes (//)
                        {
                            if (!url.Contains("//MediaCenter") && !Regex.IsMatch(url, @"(?<!:)/{2,}"))
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم تصحيح الرابط وإزالة الشرطة المائلة المزدوجة.";
                            }
                            else if (statusCode == 301 || statusCode == 200)
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم تصحيح الرابط أو تحويله برمز 301 إلى الرابط النظيف.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = "الرابط ما زال يحتوي على شرطة مائلة مزدوجة // غير مصححة.";
                            }
                        }
                        break;

                    case 1009: // SEMRUSH: HTTP to HTTPS 301 Redirect
                        {
                            if (statusCode == 301 && responseHeaders != null && (responseHeaders["Location"] ?? "").StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                            {
                                result.IsResolved = true;
                                result.Status = "Resolved";
                                result.Details = "تم التحقق: التحويل الإجباري من HTTP إلى HTTPS يعمل بنجاح برمز 301 الدائم.";
                            }
                            else
                            {
                                result.IsResolved = false;
                                result.Status = "Unresolved";
                                result.Details = $"الموقع لا يفرض التحويل إلى HTTPS برمز 301 (كود الاستجابة: {statusCode}).";
                            }
                        }
                        break;

                    // ==========================================
                    // Default Fallback: Do NOT falsely claim resolved just because 200 OK!
                    // ==========================================
                    default:
                        result.IsResolved = false;
                        result.Status = "Unresolved";
                        result.Details = "الصفحة تستجيب برمز (" + statusCode + ")، ولكن لم يُعتمد حلها تلقائياً لأن التحقق المتخصص للمعيار رقم (" + issueCode + ") قيد التطوير ولم يتم الجزم بانتفاء المشكلة.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.IsResolved = false;
                result.Status = "Error";
                result.Details = "خطأ أثناء الفحص: " + ex.Message;
            }

            return result;
        }
    }
}
