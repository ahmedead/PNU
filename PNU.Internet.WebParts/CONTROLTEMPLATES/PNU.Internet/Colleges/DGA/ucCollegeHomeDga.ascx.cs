using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// College Home tab (DGA design):
    ///   Overview + image | Vision / Mission / Goals cards | Dean's word |
    ///   International partners logo strip | Accreditations cards.
    ///
    /// Lists (provisioned from the page, idempotent, on the college web):
    ///   CollegeHomeContent   : ContentKey + Body/Body_EN (rich HTML).
    ///        Keys: Overview, OverviewTitle, OverviewImage, Vision, Mission, Goals,
    ///              DeanWord, DeanName, DeanTitle
    ///   CollegePartners      : Title/Title_EN, LogoUrl, NavUrl, ItemOrder, Visibility
    ///   CollegeAccreditations: Title/Title_EN, Body/Body_EN, ImageUrl, DisplayDate, ItemOrder, Visibility
    /// </summary>
    public partial class ucCollegeHomeDga : UserControl
    {
        public const string LIST_CONTENT = "CollegeHomeContent";
        public const string LIST_PARTNERS = "CollegePartners";
        public const string LIST_ACCRED = "CollegeAccreditations";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        private class Partner { public string Title, LogoUrl, NavUrl; }
        private class Accreditation { public string Title, Body, ImageUrl, DateText, DateIso; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureLists();
                if (!IsPostBack || ltrHome.Text.Length == 0)
                    RenderHome();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  Render
        // ------------------------------------------------------------------

        private void RenderHome()
        {
            Dictionary<string, string> content = LoadContent();
            List<Partner> partners = LoadPartners();
            List<Accreditation> accreds = LoadAccreditations();

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"d-flex flex-column gap-4\">");

            // ---------------- Overview + Vision/Mission/Goals + Dean ----------------
            sb.Append("<section class=\"mb-5\" aria-labelledby=\"faculty-overview-title\">");

            sb.Append("<div class=\"row g-5 align-items-center mb-5\">");
            sb.Append("<div class=\"col-12 col-md-6 col-lg-8\">");
            sb.AppendFormat("<h2 id=\"faculty-overview-title\" class=\"mb-4\">{0}</h2>",
                Get(content, "OverviewTitle"));
            sb.AppendFormat("<div class=\"mb-4 text-justify\">{0}</div>", Get(content, "Overview"));
            sb.Append("</div>");
            string img = Get(content, "OverviewImage");
            if (!string.IsNullOrEmpty(img))
            {
                sb.Append("<div class=\"col-12 col-md-6 col-lg-4\"><div>");
                sb.AppendFormat("<img class=\"carousel-image w-100 rounded-3\" src=\"{0}\" alt=\"\" " +
                                "loading=\"eager\" decoding=\"async\" />", img);
                sb.Append("</div></div>");
            }
            sb.Append("</div>");

            sb.Append("<div class=\"row g-5\">");
            AppendVmgCard(sb, "col-12 col-md-6 col-sm-12", "hgi-target-02",
                IsArabic ? "الرؤية" : "Vision", Get(content, "Vision"));
            AppendVmgCard(sb, "col-12 col-md-6 col-sm-12", "hgi-message-01",
                IsArabic ? "الرسالة" : "Mission", Get(content, "Mission"));
            AppendVmgCard(sb, "col-12", "hgi-target-02",
                IsArabic ? "الأهداف" : "Goals", Get(content, "Goals"));

            // Dean's word
            string deanWord = Get(content, "DeanWord");
            if (!string.IsNullOrEmpty(deanWord))
            {
                sb.Append("<section id=\"president-welcome\" aria-labelledby=\"president-welcome-title\">");
                sb.Append("<div class=\"card mb-4 bg-primary-25 border-0\"><div class=\"card-body p-4 p-lg-5\">");
                sb.Append("<div class=\"d-flex flex-column gap-3\">");
                sb.Append("<span class=\"icon-container bg-white\"><i class=\"hgi hgi-stroke hgi-quote-down fs-4\" aria-hidden=\"true\"></i></span>");
                sb.AppendFormat("<h2 id=\"president-welcome-title\" class=\"mb-0\">{0}</h2>",
                    IsArabic ? "كلمة العميدة" : "Dean's Word");
                sb.AppendFormat("<div class=\"lead mb-0 text-justify\">{0}</div>", deanWord);
                sb.Append("</div>");
                sb.Append("<div class=\"card-body\"><div>");
                sb.AppendFormat("<h3 class=\"card-title\">{0}</h3>", Get(content, "DeanName"));
                sb.AppendFormat("<p class=\"card-text mb-0\">{0}</p>", Get(content, "DeanTitle"));
                sb.Append("</div></div>");
                sb.Append("</div></div></section>");
            }

            sb.Append("</div></section>");

            // ---------------- Partners ----------------
            if (partners.Count > 0)
            {
                sb.Append("<section class=\"pb-5\" aria-labelledby=\"international-partners-title\"><div class=\"container\">");
                sb.AppendFormat("<div><h2 id=\"international-partners-title\" class=\"mb-4\">{0}</h2></div>",
                    IsArabic ? "شركاء الكلية" : "College Partners");
                sb.Append("<div class=\"d-flex flex-wrap gap-3\">");
                foreach (Partner p in partners)
                {
                    sb.Append("<div class=\"card text-center related-entity-card\"><div class=\"card-body placeholder-glow p-3\">");
                    sb.AppendFormat(
                        "<a target=\"_blank\" rel=\"noopener noreferrer\" class=\"d-flex align-items-center justify-content-center h-100\" " +
                        "href=\"{0}\" aria-label=\"{1}\">" +
                        "<img class=\"img-fluid mx-auto d-block\" src=\"{2}\" alt=\"{1}\" loading=\"lazy\" decoding=\"async\" /></a>",
                        string.IsNullOrEmpty(p.NavUrl) ? "#" : p.NavUrl,
                        HttpUtility.HtmlEncode(p.Title), p.LogoUrl);
                    sb.Append("</div></div>");
                }
                sb.Append("</div></div></section>");
            }

            // ---------------- Accreditations ----------------
            if (accreds.Count > 0)
            {
                sb.Append("<section class=\"pb-5\" aria-labelledby=\"faculty-accreditations-title\"><div class=\"container\">");
                sb.AppendFormat("<div><h2 id=\"faculty-accreditations-title\" class=\"mb-4\">{0}</h2></div>",
                    IsArabic ? "الاعتمادات" : "Accreditations");
                sb.Append("<div class=\"row g-4\">");
                foreach (Accreditation a in accreds)
                {
                    sb.Append("<div class=\"col-12 col-lg-5 col-md-5\">");
                    sb.Append("<article class=\"card h-100 pnu-news-card\"><div class=\"card-body d-flex flex-column placeholder-glow h-100\">");
                    if (!string.IsNullOrEmpty(a.ImageUrl))
                        sb.AppendFormat("<img width=\"400\" height=\"250\" class=\"rounded-2\" alt=\"{0}\" loading=\"lazy\" src=\"{1}\" decoding=\"async\" />",
                            HttpUtility.HtmlEncode(a.Title), a.ImageUrl);
                    sb.Append("<div class=\"flex-grow-1\">");
                    sb.AppendFormat("<h3 class=\"card-title\">{0}</h3>", HttpUtility.HtmlEncode(a.Title));
                    sb.AppendFormat("<div class=\"card-text line-clamp max-clamp-line-4\">{0}</div>", a.Body);
                    sb.Append("</div>");
                    if (!string.IsNullOrEmpty(a.DateText))
                    {
                        sb.Append("<div class=\"mt-auto d-flex flex-column gap-3\">");
                        sb.AppendFormat("<small class=\"d-flex gap-2 align-items-center\">" +
                            "<i class=\"hgi hgi-stroke hgi-calendar-03\" aria-hidden=\"true\"></i>" +
                            "<time datetime=\"{0}\">{1}</time></small>", a.DateIso, a.DateText);
                        sb.Append("</div>");
                    }
                    sb.Append("</div></article></div>");
                }
                sb.Append("</div></div></section>");
            }

            sb.Append("</div>");
            ltrHome.Text = sb.ToString();
        }

        private void AppendVmgCard(StringBuilder sb, string colClass, string icon, string title, string body)
        {
            if (string.IsNullOrEmpty(body)) return;
            sb.AppendFormat("<div class=\"{0}\"><div class=\"card h-100\"><div class=\"card-body\">", colClass);
            sb.AppendFormat("<div class=\"icon-container\"><i class=\"hgi hgi-stroke {0} fs-3\" aria-hidden=\"true\"></i></div>", icon);
            sb.AppendFormat("<div><h3 class=\"card-title h5\">{0}</h3><div class=\"card-text\">{1}</div></div>",
                HttpUtility.HtmlEncode(title), body);
            sb.Append("</div></div></div>");
        }

        // ------------------------------------------------------------------
        //  Data
        // ------------------------------------------------------------------

        private Dictionary<string, string> LoadContent()
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(LIST_CONTENT);
                if (list == null) return result;

                foreach (SPListItem item in list.GetItems())
                {
                    string key = item.Fields.ContainsField("ContentKey") && item["ContentKey"] != null
                                 ? item["ContentKey"].ToString() : "";
                    if (string.IsNullOrEmpty(key)) continue;

                    string ar = item.Fields.ContainsField("Body") && item["Body"] != null ? item["Body"].ToString() : "";
                    string en = item.Fields.ContainsField("Body_EN") && item["Body_EN"] != null ? item["Body_EN"].ToString() : "";
                    result[key] = IsArabic
                        ? (!string.IsNullOrEmpty(ar) ? ar : en)
                        : (!string.IsNullOrEmpty(en) ? en : ar);
                }
            }
            return result;
        }

        private List<Partner> LoadPartners()
        {
            var result = new List<Partner>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList(LIST_PARTNERS);
                    if (list == null) return;

                    SPQuery q = new SPQuery
                    {
                        Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                                  <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                    };
                    foreach (SPListItem item in list.GetItems(q))
                    {
                        string logo = ucSideMenu.GetFirstFieldValue(item, "LogoUrl");
                        if (string.IsNullOrEmpty(logo)) continue;
                        result.Add(new Partner
                        {
                            Title = IsArabic
                                ? ucSideMenu.GetFirstFieldValue(item, "Title")
                                : ucSideMenu.GetFirstFieldValue(item, "Title_EN", "Title"),
                            LogoUrl = logo,
                            NavUrl = ucSideMenu.GetFirstFieldValue(item, "NavUrl")
                        });
                    }
                }
            

            });
            return result;
        }

        private List<Accreditation> LoadAccreditations()
        {
            var result = new List<Accreditation>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList(LIST_ACCRED);
                    if (list == null)
                    {
                        return;
                    }

                    SPQuery q = new SPQuery
                    {
                        Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                                  <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                    };
                    foreach (SPListItem item in list.GetItems(q))
                    {
                        var acc = new Accreditation
                        {
                            Title = IsArabic
                                ? ucSideMenu.GetFirstFieldValue(item, "Title")
                                : ucSideMenu.GetFirstFieldValue(item, "Title_EN", "Title"),
                            Body = IsArabic
                                ? ucSideMenu.GetFirstFieldValue(item, "Body")
                                : ucSideMenu.GetFirstFieldValue(item, "Body_EN", "Body"),
                            ImageUrl = ucSideMenu.GetFirstFieldValue(item, "ImageUrl")
                        };

                        if (item.Fields.ContainsField("DisplayDate") && item["DisplayDate"] != null)
                        {
                            // InvariantCulture prevents Hijri rendering (established PNU rule)
                            DateTime dt = Convert.ToDateTime(item["DisplayDate"].ToString(),
                                                             System.Globalization.CultureInfo.InvariantCulture);
                            acc.DateIso = dt.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            acc.DateText = IsArabic
                                ? dt.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"))
                                : dt.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));
                        }
                        result.Add(acc);
                    }
                }
            
            });

            
            return result;
        }

        // ------------------------------------------------------------------
        //  Provisioning (from the page - idempotent)
        // ------------------------------------------------------------------

        private void EnsureLists()
        {
            bool needsWork = false; ;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite s = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb w = s.OpenWeb())
                {
                    SPList c = w.Lists.TryGetList(LIST_CONTENT);
                    needsWork = (c == null || c.ItemCount == 0 ||
                                 w.Lists.TryGetList(LIST_PARTNERS) == null ||
                                 w.Lists.TryGetList(LIST_ACCRED) == null);
                }

            });

            
            if (!needsWork) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    // CollegeHomeContent
                    SPList content = web.Lists.TryGetList(LIST_CONTENT);
                    if (content == null)
                        content = web.Lists[web.Lists.Add(LIST_CONTENT,
                            "Home tab content blocks (key/value, rich HTML)", SPListTemplateType.GenericList)];
                    ucCollegeContentSection.AddTextField(content, "ContentKey");
                    ucCollegeContentSection.AddRichField(content, "Body");
                    ucCollegeContentSection.AddRichField(content, "Body_EN");
                    content.Update();

                    if (content.ItemCount == 0)
                    {
                        SeedContent(content, "OverviewTitle", "نظرة عامة عن الكلية", "College Overview");
                        SeedContent(content, "Overview", "<p>نبذة عن الكلية...</p>", "<p>About the college...</p>");
                        SeedContent(content, "OverviewImage", "", "");
                        SeedContent(content, "Vision", "رؤية الكلية...", "College vision...");
                        SeedContent(content, "Mission", "رسالة الكلية...", "College mission...");
                        SeedContent(content, "Goals", "<ul class=\"card-text mb-0\"><li>الهدف الأول</li><li>الهدف الثاني</li></ul>",
                                                      "<ul class=\"card-text mb-0\"><li>Goal 1</li><li>Goal 2</li></ul>");
                        SeedContent(content, "DeanWord", "<p>كلمة العميدة...</p>", "<p>Dean's word...</p>");
                        SeedContent(content, "DeanName", "د. ...", "Dr. ...");
                        SeedContent(content, "DeanTitle", "عميدة الكلية", "Dean of the College");
                    }

                    // CollegePartners
                    SPList partners = web.Lists.TryGetList(LIST_PARTNERS);
                    if (partners == null)
                        partners = web.Lists[web.Lists.Add(LIST_PARTNERS,
                            "College partner logos", SPListTemplateType.GenericList)];
                    ucCollegeContentSection.AddTextField(partners, "Title_EN");
                    ucCollegeContentSection.AddTextField(partners, "LogoUrl");
                    ucCollegeContentSection.AddTextField(partners, "NavUrl");
                    ucCollegeContentSection.AddNumberField(partners, "ItemOrder");
                    ucCollegeContentSection.AddBoolField(partners, "Visibility", "1");
                    partners.Update();

                    // CollegeAccreditations
                    SPList accreds = web.Lists.TryGetList(LIST_ACCRED);
                    if (accreds == null)
                        accreds = web.Lists[web.Lists.Add(LIST_ACCRED,
                            "College accreditations cards", SPListTemplateType.GenericList)];
                    ucCollegeContentSection.AddTextField(accreds, "Title_EN");
                    ucCollegeContentSection.AddRichField(accreds, "Body");
                    ucCollegeContentSection.AddRichField(accreds, "Body_EN");
                    ucCollegeContentSection.AddTextField(accreds, "ImageUrl");
                    if (!accreds.Fields.ContainsField("DisplayDate"))
                        accreds.Fields.Add("DisplayDate", SPFieldType.DateTime, false);
                    ucCollegeContentSection.AddNumberField(accreds, "ItemOrder");
                    ucCollegeContentSection.AddBoolField(accreds, "Visibility", "1");
                    accreds.Update();

                    web.AllowUnsafeUpdates = false;
                }
            });
        }

        private static void SeedContent(SPList list, string key, string bodyAr, string bodyEn)
        {
            SPListItem item = list.AddItem();
            item["Title"] = key;
            item["ContentKey"] = key;
            item["Body"] = bodyAr;
            item["Body_EN"] = bodyEn;
            item.Update();
        }

        private static string Get(Dictionary<string, string> dict, string key)
        {
            return dict.ContainsKey(key) ? dict[key] : "";
        }
    }

}
