using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    /// <summary>
    /// Generic Intelligent Transformer to convert raw SharePoint publishing page content into DGA compliant HTML.
    /// </summary>
    public static class DgaAutoTransformer
    {
        public const string DefaultLayout = "/_catalogs/masterpage/DGANewBlankWebPartPage.aspx";

        public static string TransformContent(string pageUrl, string pageTitle, string rawHtml)
        {
            if (string.IsNullOrWhiteSpace(rawHtml))
            {
                rawHtml = "<p>No content available.</p>";
            }

            // 1. Clean encodings and problematic SharePoint artifacts
            string cleanedHtml = CleanSharePointArtifacts(rawHtml);

            // If already fully wrapped in DGA structure, return cleaned
            if (cleanedHtml.Contains("class=\"d-flex flex-column gap-5 text-start\"") ||
                cleanedHtml.Contains("bg-primary-25"))
            {
                return cleanedHtml;
            }

            bool isArabic = IsArabicPage(pageUrl, cleanedHtml);
            string dir = isArabic ? "rtl" : "ltr";
            string lang = isArabic ? "ar-SA" : "en-US";
            string entityName = DetectEntityName(pageUrl, isArabic);

            // Parse HTML with HtmlAgilityPack
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(cleanedHtml);

            // Clean inline styles & SharePoint classes from all nodes
            CleanNodes(doc.DocumentNode);

            // Transform tables
            TransformTables(doc);

            // Transform download links
            TransformDownloadLinks(doc, isArabic);

            // Transform Headings and Subsections
            TransformHeadingsAndLists(doc, isArabic);

            // Format Contact Info blocks
            TransformContactInfo(doc, isArabic);

            // Extract lead paragraph
            string leadText = ExtractLeadText(doc, pageTitle, entityName, isArabic);

            // Build DGA HTML
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(@"<div dir=""{0}"" lang=""{1}"" class=""d-flex flex-column gap-5 text-start"">", dir, lang));

            // Hero Header Card
            sb.AppendLine(@"   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">");
            sb.AppendLine(@"      <div class=""d-flex align-items-center gap-2 mb-2"">");
            sb.AppendLine(string.Format(@"         <span class=""badge bg-primary text-white"">{0}</span>", entityName));
            sb.AppendLine(string.Format(@"         <span class=""badge bg-light text-primary border border-primary"">{0}</span>", isArabic ? "معلومات معتمدة" : "Official Information"));
            sb.AppendLine(@"      </div>");
            sb.AppendLine(@"      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">");
            sb.AppendLine(string.Format(@"         <i class=""hgi hgi-stroke hgi-document-text text-primary""></i><span>{0}</span>", pageTitle));
            sb.AppendLine(@"      </h1>");
            if (!string.IsNullOrEmpty(leadText))
            {
                sb.AppendLine(string.Format(@"      <p class=""text-body-secondary leading-relaxed mb-0"">{0}</p>", leadText));
            }
            sb.AppendLine(@"   </div>");

            // Main Content Body Card
            sb.AppendLine(@"   <section class=""d-flex flex-column gap-4"">");
            sb.AppendLine(@"      <div class=""card p-4 border rounded-3 bg-light"">");
            sb.AppendLine(doc.DocumentNode.InnerHtml.Trim());
            sb.AppendLine(@"      </div>");
            sb.AppendLine(@"   </section>");
            sb.AppendLine(@"</div>");

            return sb.ToString();
        }

        private static void TransformHeadingsAndLists(HtmlDocument doc, bool isArabic)
        {
            string arrowIcon = isArabic ? "hgi-arrow-left-01" : "hgi-arrow-right-01";

            // 1. Transform H1, H2, H3, H4 into styled DGA Section Headers or body paragraphs
            var headings = doc.DocumentNode.SelectNodes("//h1 | //h2 | //h3 | //h4");
            if (headings != null)
            {
                foreach (var h in headings)
                {
                    string text = StripTags(h.InnerText);
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        h.Remove();
                        continue;
                    }

                    // In SharePoint RTE, body paragraphs frequently end up inside H1/H2/H3/H4 tags
                    // when authors press Enter without resetting the style ribbon.
                    // If text is long (> 85 chars) or ends with a sentence period '.' (and not a colon ':'),
                    // it is genuine BODY CONTENT, NOT A SECTION HEADER!
                    if (text.Length > 85 || (text.EndsWith(".") && !text.EndsWith(":") && text.Length > 30))
                    {
                        h.Name = "p";
                        h.SetAttributeValue("class", "text-body-secondary leading-relaxed mb-3");
                        h.InnerHtml = CleanEncodings(h.InnerHtml);
                        continue;
                    }

                    // Otherwise, it is a valid Section Header!
                    h.Name = "h2";
                    h.SetAttributeValue("class", "h5 fw-bold text-dark mt-4 mb-3 d-flex align-items-center gap-2 border-bottom pb-2");
                    h.InnerHtml = string.Format(@"<i class=""hgi hgi-stroke {0} text-primary fs-5""></i><span>{1}</span>", arrowIcon, text);
                }
            }

            // 2. Transform pseudo bullet points and subtitle paragraphs/divs
            var pNodes = doc.DocumentNode.SelectNodes("//p | //div[not(.//div) and not(.//table) and not(.//h2)]");
            if (pNodes != null)
            {
                foreach (var p in pNodes)
                {
                    string text = p.InnerText.Trim();
                    if (text.StartsWith("·") || text.StartsWith("•") || text.StartsWith("o ") || text.StartsWith("- "))
                    {
                        string cleanText = text.TrimStart('·', '•', 'o', '-', ' ', '\t');
                        if (cleanText.EndsWith(":"))
                        {
                            // Subhead bullet
                            p.Name = "h3";
                            p.SetAttributeValue("class", "h6 fw-bold text-primary mt-3 mb-2 d-flex align-items-center gap-2");
                            p.InnerHtml = string.Format(@"<i class=""hgi hgi-stroke hgi-tick-02 fs-6 text-primary""></i><span>{0}</span>", cleanText);
                        }
                        else
                        {
                            // Bullet item
                            p.SetAttributeValue("class", "d-flex align-items-start gap-2 mb-2 text-body-secondary");
                            p.InnerHtml = string.Format(@"<i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-6 mt-1 flex-shrink-0""></i><span>{0}</span>", cleanText);
                        }
                    }
                    else if (p.Name == "p" && text.EndsWith(":") && text.Length < 60 && !text.Contains("<br"))
                    {
                        // Paragraph that is a bold/normal subtitle ending with colon (like "Overview:", "Tasks:", "المهام:")
                        p.Name = "h3";
                        p.SetAttributeValue("class", "h6 fw-bold text-dark mt-3 mb-2 d-flex align-items-center gap-2");
                        p.InnerHtml = string.Format(@"<i class=""hgi hgi-stroke {0} text-primary fs-6""></i><span>{1}</span>", arrowIcon, text);
                    }
                }
            }

            // 3. Remove underline formatting spans that look ugly
            var underlines = doc.DocumentNode.SelectNodes("//span[contains(@style, 'underline')] | //u");
            if (underlines != null)
            {
                foreach (var u in underlines)
                {
                    u.Attributes.Remove("style");
                }
            }
        }

        private static void TransformContactInfo(HtmlDocument doc, bool isArabic)
        {
            var pNodes = doc.DocumentNode.SelectNodes("//p[contains(text(), '@') or contains(text(), 'WhatsApp') or contains(text(), 'واتساب') or contains(text(), '01182') or contains(text(), 'الهاتف')] | //div[not(.//div) and (contains(text(), '@') or contains(text(), 'WhatsApp') or contains(text(), 'واتساب') or contains(text(), '01182') or contains(text(), 'الهاتف'))]");
            if (pNodes != null)
            {
                foreach (var p in pNodes)
                {
                    string text = p.InnerText;

                    // Match WhatsApp
                    Match mWa = Regex.Match(text, @"(?:WhatsApp|واتساب)[\s/:]*(\d{9,12})", RegexOptions.IgnoreCase);
                    if (mWa.Success)
                    {
                        string waNum = mWa.Groups[1].Value.Trim();
                        string intlNum = waNum.StartsWith("0") ? "966" + waNum.Substring(1) : waNum;
                        p.InnerHtml = Regex.Replace(p.InnerHtml, @"(?:WhatsApp|واتساب)[\s/:]*\d{9,12}",
                            string.Format(@"<a href=""https://wa.me/{0}"" target=""_blank"" class=""btn btn-outline-success btn-sm d-inline-flex align-items-center gap-1 my-1""><i class=""hgi hgi-stroke hgi-whatsapp""></i><span>واتساب: {1}</span></a>", intlNum, waNum), RegexOptions.IgnoreCase);
                    }

                    // Match Email
                    Match mMail = Regex.Match(text, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
                    if (mMail.Success && !p.InnerHtml.Contains("<a "))
                    {
                        string email = mMail.Value;
                        p.InnerHtml = p.InnerHtml.Replace(email,
                            string.Format(@"<a href=""mailto:{0}"" class=""btn btn-outline-primary btn-sm d-inline-flex align-items-center gap-1 my-1""><i class=""hgi hgi-stroke hgi-mail-01""></i><span>{0}</span></a>", email));
                    }

                    // Match Phone
                    Match mPhone = Regex.Match(text, @"(?:الهاتف|Phone)[\s/:]*(\d{7,10})", RegexOptions.IgnoreCase);
                    if (mPhone.Success)
                    {
                        string phone = mPhone.Groups[1].Value;
                        p.InnerHtml = Regex.Replace(p.InnerHtml, @"(?:الهاتف|Phone)[\s/:]*\d{7,10}",
                            string.Format(@"<a href=""tel:{0}"" class=""btn btn-outline-secondary btn-sm d-inline-flex align-items-center gap-1 my-1""><i class=""hgi hgi-stroke hgi-call""></i><span>{0}</span></a>", phone), RegexOptions.IgnoreCase);
                    }
                }
            }
        }

        private static void CleanNodes(HtmlNode node)
        {
            if (node == null) return;

            if (node.NodeType == HtmlNodeType.Element)
            {
                // Remove inline style attributes that conflict with DGA
                if (node.Attributes.Contains("style"))
                {
                    string style = node.Attributes["style"].Value;
                    style = Regex.Replace(style, @"(font-family|font-size|color|text-indent|background-color):[^;""']*;?", "", RegexOptions.IgnoreCase);
                    if (string.IsNullOrWhiteSpace(style))
                    {
                        node.Attributes.Remove("style");
                    }
                    else
                    {
                        node.Attributes["style"].Value = style.Trim();
                    }
                }

                // Preserve SharePoint RTE Heading classes before stripping them
                if (node.Attributes.Contains("class"))
                {
                    string cls = node.Attributes["class"].Value;
                    if (cls.IndexOf("ms-rteElement-H", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string txt = StripTags(node.InnerText);
                        if (!string.IsNullOrWhiteSpace(txt) && txt.Length <= 85 && (!txt.EndsWith(".") || txt.EndsWith(":")))
                        {
                            node.Name = "h2";
                        }
                    }

                    cls = Regex.Replace(cls, @"ms-rte\S*", "", RegexOptions.IgnoreCase);
                    cls = Regex.Replace(cls, @"MsoNormal\S*", "", RegexOptions.IgnoreCase);
                    if (string.IsNullOrWhiteSpace(cls))
                    {
                        node.Attributes.Remove("class");
                    }
                    else
                    {
                        node.Attributes["class"].Value = cls.Trim();
                    }
                }

                // Remove font tags
                if (node.Name.Equals("font", StringComparison.OrdinalIgnoreCase))
                {
                    node.Name = "span";
                    node.Attributes.RemoveAll();
                }
            }

            foreach (var child in node.ChildNodes)
            {
                CleanNodes(child);
            }
        }

        private static void TransformTables(HtmlDocument doc)
        {
            var tables = doc.DocumentNode.SelectNodes("//table");
            if (tables == null) return;

            foreach (var tbl in tables)
            {
                tbl.SetAttributeValue("class", "table table-hover align-middle mb-0");
                tbl.Attributes.Remove("cellspacing");
                tbl.Attributes.Remove("cellpadding");
                tbl.Attributes.Remove("border");
                tbl.Attributes.Remove("width");

                // Wrap in table-responsive div if not already wrapped
                if (tbl.ParentNode == null || !tbl.ParentNode.GetAttributeValue("class", "").Contains("table-responsive"))
                {
                    var wrapper = doc.CreateElement("div");
                    wrapper.SetAttributeValue("class", "table-responsive border rounded-3 my-3");
                    tbl.ParentNode.ReplaceChild(wrapper, tbl);
                    wrapper.AppendChild(tbl);
                }
            }
        }

        private static void TransformDownloadLinks(HtmlDocument doc, bool isArabic)
        {
            var links = doc.DocumentNode.SelectNodes("//a[@href]");
            if (links == null) return;

            foreach (var link in links)
            {
                string href = link.GetAttributeValue("href", "");
                if (href.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ||
                    href.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) ||
                    href.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    link.SetAttributeValue("target", "_blank");
                    link.SetAttributeValue("rel", "noopener noreferrer");
                    link.SetAttributeValue("class", "btn btn-outline-primary btn-sm d-inline-flex align-items-center gap-1 my-1");
                }
            }
        }

        private static string ExtractLeadText(HtmlDocument doc, string pageTitle, string entityName, bool isArabic)
        {
            var pNodes = doc.DocumentNode.SelectNodes("//p | //div[not(.//div) and not(.//table) and not(.//h2)]");
            if (pNodes != null)
            {
                foreach (var p in pNodes)
                {
                    string txt = p.InnerText.Trim();
                    txt = CleanEncodings(txt);
                    if (txt.Length > 25 && !txt.Contains("Click here") && !txt.Contains("اضغط هنا") && !txt.Contains("Download") && !txt.StartsWith("·") && !txt.StartsWith("•") && !txt.EndsWith(":"))
                    {
                        return txt;
                    }
                }
            }

            return isArabic
                ? string.Format("الصفحة الرسمية المعتمدة لـ {0} بجامعة الأميرة نورة بنت عبد الرحمن.", pageTitle)
                : string.Format("Official portal page for {0} at Princess Nourah bint Abdulrahman University.", pageTitle);
        }

        public static bool IsArabicPage(string url, string text)
        {
            if (url.IndexOf("/ar/", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (url.IndexOf("/en/", StringComparison.OrdinalIgnoreCase) >= 0) return false;

            int arabicChars = 0;
            foreach (char c in text)
            {
                if (c >= 0x0600 && c <= 0x06FF) arabicChars++;
            }
            return arabicChars > 20;
        }

        public static string DetectEntityName(string url, bool isArabic)
        {
            string u = url.ToLowerInvariant();
            if (u.Contains("/devandskilldean/")) return isArabic ? "عمادة التطوير والجودة" : "Deanship of Development and Quality";
            if (u.Contains("/faculties/ad/")) return isArabic ? "كلية التصاميم والفنون" : "College of Design and Arts";
            if (u.Contains("/faculties/ei/")) return isArabic ? "معهد اللغة الإنجليزية" : "English Language Institute";
            if (u.Contains("/faculties/lw/")) return isArabic ? "كلية الحقوق" : "College of Law";
            if (u.Contains("/faculties/ph/")) return isArabic ? "كلية الصيدلة" : "College of Pharmacy";
            if (u.Contains("/faculties/md/")) return isArabic ? "كلية الطب البشري" : "College of Medicine";
            if (u.Contains("/faculties/cs/")) return isArabic ? "كلية العلوم" : "College of Science";
            if (u.Contains("/faculties/ccis/")) return isArabic ? "كلية علوم الحاسب والمعلومات" : "College of Computer and Information Sciences";
            if (u.Contains("/faculties/cba/")) return isArabic ? "كلية إدارة الأعمال" : "College of Business Administration";
            if (u.Contains("/faculties/cen/")) return isArabic ? "كلية الهندسة" : "College of Engineering";
            if (u.Contains("/regadm/regbsc/")) return isArabic ? "عمادة القبول والتسجيل" : "Deanship of Admission and Registration";
            if (u.Contains("/regadm/pgd/") || u.Contains("/deanship/postgraduate/")) return isArabic ? "عمادة الدراسات العليا" : "Deanship of Postgraduate Studies";
            if (u.Contains("/deanship/research/") || u.Contains("/sr/")) return isArabic ? "عمادة البحث العلمي والمكتبات" : "Deanship of Scientific Research and Libraries";

            return isArabic ? "جامعة الأميرة نورة بنت عبد الرحمن" : "Princess Nourah bint Abdulrahman University";
        }

        public static string CleanSharePointArtifacts(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            return html.Replace("&#58;", ":")
                       .Replace("&#160;", " ")
                       .Replace("&nbsp;", " ")
                       .Replace("&quot;", "\"")
                       .Replace("&amp;", "&")
                       .Replace("â€‹", "")
                       .Replace("â€¢", "•")
                       .Replace("â€“", "–")
                       .Replace("â€”", "—")
                       .Replace("Â·", "•")
                       .Replace("Ã¼", "ü")
                       .Replace("Ã´", "ô")
                       .Replace("â€œ", "\"")
                       .Replace("â€\u009d", "\"")
                       .Replace("â€™", "'");
        }

        public static string StripTags(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string s = Regex.Replace(input, @"<[^>]+>", " ");
            s = CleanEncodings(s);
            return Regex.Replace(s, @"\s+", " ").Trim();
        }

        public static string CleanEncodings(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string s = CleanSharePointArtifacts(text);
            return Regex.Replace(s, @"\s+", " ").Trim();
        }
    }
}
