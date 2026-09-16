using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    /// <summary>
    /// Intelligent bilingual HTML and text translation service for PNU SharePoint Publishing Pages.
    /// Preserves HTML tags, CSS classes, attributes, scripts, and layouts using HtmlAgilityPack.
    /// </summary>
    public static class BilingualHtmlTranslator
    {
        // Cache of frequent academic and administrative dictionary terms
        private static readonly Dictionary<string, string> ArToEnDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "الرئيسية", "Home" },
            { "عن الكلية", "About College" },
            { "عن العمادة", "About Deanship" },
            { "عن الإدارة", "About Department" },
            { "عن المركز", "About Center" },
            { "عن المعهد", "About Institute" },
            { "الهيكل التنظيمي", "Organizational Structure" },
            { "الرؤية والرسالة والأهداف", "Vision, Mission and Objectives" },
            { "الرؤية والرسالة", "Vision and Mission" },
            { "كلمة العميد", "Dean's Message" },
            { "كلمة العميدة", "Dean's Message" },
            { "كلمة المشرف العام", "General Supervisor's Message" },
            { "مجلس الكلية", "College Council" },
            { "الوكالات", "Agencies" },
            { "وكالة الكلية", "College Vice Deanship" },
            { "وكالة الكلية للشؤون التعليمية", "Vice Deanship for Academic Affairs" },
            { "وكالة الكلية للدراسات العليا والبحث العلمي", "Vice Deanship for Graduate Studies & Scientific Research" },
            { "وكالة الكلية للتطوير والجودة", "Vice Deanship for Development & Quality" },
            { "الأقسام الأكاديمية", "Academic Departments" },
            { "الأقسام", "Departments" },
            { "الإدارات", "Departments" },
            { "المراكز", "Centers" },
            { "الوحدات", "Units" },
            { "البرامج والمبادرات", "Programs and Initiatives" },
            { "البرامج الأكاديمية", "Academic Programs" },
            { "برامج البكالوريوس", "Bachelor Programs" },
            { "برامج الدراسات العليا", "Graduate Studies Programs" },
            { "الخطة الدراسية", "Study Plan" },
            { "الخطط الدراسية", "Study Plans" },
            { "شروط القبول", "Admission Requirements" },
            { "الخدمات", "Services" },
            { "الخدمات الطلابية", "Student Services" },
            { "المستندات والنماذج والأدلة", "Documents, Forms and Guides" },
            { "اللوائح والأنظمة", "Rules and Regulations" },
            { "الأدلة والنماذج", "Guides and Forms" },
            { "البحث العلمي", "Scientific Research" },
            { "المشاريع البحثية", "Research Projects" },
            { "الأنشطة والفعاليات", "Activities and Events" },
            { "الأخبار", "News" },
            { "الإعلانات", "Advertisements" },
            { "تواصل معنا", "Contact Us" },
            { "منسوبي الكلية", "College Staff" },
            { "أعضاء هيئة التدريس", "Faculty Members" },
            { "الشهادات والجوائز", "Certificates and Awards" },
            { "الإنجازات", "Achievements" },
            { "الأندية الطلابية", "Student Clubs" },
            { "المرافق", "Facilities" },
            { "مسارات المستفيدين", "Beneficiary Pathways" },
            { "الأسئلة الشائعة", "Frequently Asked Questions" }
        };

        private static readonly Dictionary<string, string> EnToArDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static BilingualHtmlTranslator()
        {
            // Build reverse dictionary
            foreach (var kvp in ArToEnDictionary)
            {
                if (!EnToArDictionary.ContainsKey(kvp.Value))
                {
                    EnToArDictionary.Add(kvp.Value, kvp.Key);
                }
            }
        }

        /// <summary>
        /// Translates full HTML document or fragment while preserving markup, styling, and classes.
        /// Only text nodes are translated.
        /// </summary>
        public static string TranslateHtml(string html, string sourceLang, string targetLang)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;

            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Select text nodes that contain non-whitespace text
                var textNodes = doc.DocumentNode.SelectNodes("//text()[normalize-space(.) != '']");
                if (textNodes != null && textNodes.Count > 0)
                {
                    foreach (var node in textNodes)
                    {
                        // Skip scripts, styles, codes, pre
                        if (node.ParentNode != null)
                        {
                            string parentName = node.ParentNode.Name.ToLowerInvariant();
                            if (parentName == "script" || parentName == "style" || parentName == "code" || parentName == "noscript")
                                continue;
                        }

                        string originalText = node.InnerText.Trim();
                        if (string.IsNullOrEmpty(originalText))
                            continue;

                        // Check if text is only numbers or punctuation
                        if (Regex.IsMatch(originalText, @"^[\d\s\-_.,:;!@#$%^&*()+=/\\|<>?'""]+$"))
                            continue;

                        string translated = TranslateText(originalText, sourceLang, targetLang);
                        if (!string.IsNullOrEmpty(translated) && translated != originalText)
                        {
                            // Preserve leading and trailing whitespace
                            string raw = node.InnerHtml;
                            string leading = Regex.Match(raw, @"^\s*").Value;
                            string trailing = Regex.Match(raw, @"\s*$").Value;

                            node.InnerHtml = leading + WebUtility.HtmlEncode(translated) + trailing;
                        }
                    }
                }

                // Adjust direction attributes on root elements if present
                AdjustDirectionAttributes(doc, targetLang);

                return doc.DocumentNode.InnerHtml;
            }
            catch
            {
                // Fallback: return original html safely if parsing fails
                return html;
            }
        }

        /// <summary>
        /// Translates plain text (e.g. Page Title, headings) between Arabic and English.
        /// </summary>
        public static string TranslateText(string text, string sourceLang, string targetLang)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            string trimmed = text.Trim();

            // 1. Check university terminology dictionary first for exact match
            if (sourceLang.Equals("ar", StringComparison.OrdinalIgnoreCase) && ArToEnDictionary.TryGetValue(trimmed, out string enTerm))
            {
                return enTerm;
            }
            if (sourceLang.Equals("en", StringComparison.OrdinalIgnoreCase) && EnToArDictionary.TryGetValue(trimmed, out string arTerm))
            {
                return arTerm;
            }

            // 2. Query free translation endpoint
            try
            {
                string result = QueryGoogleTranslateEndpoint(trimmed, sourceLang, targetLang);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }
            catch
            {
                // Ignore web exception, fall back to dictionary or original
            }

            // 3. Fallback to dictionary substring replacement or original
            return trimmed;
        }

        private static string QueryGoogleTranslateEndpoint(string text, string sl, string tl)
        {
            // Limit single request length
            if (text.Length > 2000)
            {
                text = text.Substring(0, 2000);
            }

            string encodedText = Uri.EscapeDataString(text);
            string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                sl, tl, encodedText);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";
            request.Timeout = 7000;
            request.ReadWriteTimeout = 7000;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string json = reader.ReadToEnd();
                return ParseGoogleTranslateResponse(json);
            }
        }

        private static string ParseGoogleTranslateResponse(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            try
            {
                JArray root = JArray.Parse(json);
                if (root.Count > 0 && root[0] is JArray segments)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var seg in segments)
                    {
                        if (seg is JArray item && item.Count > 0)
                        {
                            sb.Append(item[0]?.ToString());
                        }
                    }
                    return sb.ToString();
                }
            }
            catch
            {
                // Failed to parse json
            }

            return string.Empty;
        }

        private static void AdjustDirectionAttributes(HtmlDocument doc, string targetLang)
        {
            bool isArabic = targetLang.Equals("ar", StringComparison.OrdinalIgnoreCase);
            string dir = isArabic ? "rtl" : "ltr";
            string lang = isArabic ? "ar-SA" : "en-US";

            var dirNodes = doc.DocumentNode.SelectNodes("//*[@dir]");
            if (dirNodes != null && dirNodes.Count > 0)
            {
                foreach (var n in dirNodes)
                {
                    n.SetAttributeValue("dir", dir);
                }
            }
            else
            {
                // If no dir attribute is present, ensure container has correct dir
                var firstContainer = doc.DocumentNode.SelectSingleNode("//div | //section | //article");
                if (firstContainer != null)
                {
                    firstContainer.SetAttributeValue("dir", dir);
                }
            }

            var langNodes = doc.DocumentNode.SelectNodes("//*[@lang]");
            if (langNodes != null && langNodes.Count > 0)
            {
                foreach (var n in langNodes)
                {
                    n.SetAttributeValue("lang", lang);
                }
            }
        }

        /// <summary>
        /// Determines if the provided URL or text belongs to an Arabic or English context.
        /// </summary>
        public static bool IsArabicContext(string url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            if (url.IndexOf("/en/", StringComparison.OrdinalIgnoreCase) >= 0)
                return false;

            return true;
        }
    }
}
