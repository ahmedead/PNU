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
    /// Generic DGA content-section control.
    /// One control serves ALL "More about the college" tabs (org structure, achievements,
    /// agencies, facilities, research, students, services, clubs, initiatives, training).
    ///
    /// The side menu passes the section via the ControlPath:
    ///   ucCollegeContentSection.ascx?SectionKey=faculty-achievements
    ///
    /// Data lives in two lists on the college web (provisioned from the page, idempotent):
    ///   CollegeContentSections : SectionKey, Title/Title_EN, Intro/Intro_EN
    ///   CollegeContentItems    : SectionKey, Title/Title_EN, Body/Body_EN (rich HTML), ItemOrder
    /// Rendering: h2 + intro + Bootstrap accordion (DGA accordion-flush pattern).
    /// </summary>
    public partial class ucCollegeContentSection : UserControl
    {
        public const string LIST_SECTIONS = "CollegeContentSections";
        public const string LIST_ITEMS = "CollegeContentItems";

        /// <summary>Set by ucSideMenu from the ControlPath query (?SectionKey=...).</summary>
        public string SectionKey { get; set; }

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureLists();
                if (!IsPostBack || ltrSection.Text.Length == 0)
                    Render();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  Render
        // ------------------------------------------------------------------

        private void Render()
        {
            if (string.IsNullOrEmpty(SectionKey)) return;

            string sectionTitle = "", sectionIntro = "";
            var items = new List<KeyValuePair<string, string>>();   // title -> body html

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                // ---- section header ----
                SPList secList = web.Lists.TryGetList(LIST_SECTIONS);
                if (secList != null)
                {
                    SPQuery q = new SPQuery
                    {
                        Query = @"<Where><Eq><FieldRef Name='SectionKey'/><Value Type='Text'>" +
                                SectionKey + @"</Value></Eq></Where>",
                        RowLimit = 1
                    };
                    foreach (SPListItem it in secList.GetItems(q))
                    {
                        sectionTitle = Localize(it, "Title", "Title_EN");
                        sectionIntro = Localize(it, "Intro", "Intro_EN");
                    }
                }

                // ---- accordion items ----
                SPList itemList = web.Lists.TryGetList(LIST_ITEMS);
                if (itemList != null)
                {
                    SPQuery q = new SPQuery
                    {
                        Query = @"<Where>
                                    <And>
                                      <Eq><FieldRef Name='SectionKey'/><Value Type='Text'>" + SectionKey + @"</Value></Eq>
                                      <Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq>
                                    </And>
                                  </Where>
                                  <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                    };
                    foreach (SPListItem it in itemList.GetItems(q))
                    {
                        string title = Localize(it, "Title", "Title_EN");
                        string body = Localize(it, "Body", "Body_EN");
                        if (!string.IsNullOrEmpty(title))
                            items.Add(new KeyValuePair<string, string>(title, body));
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            string accId = SectionKey.Replace("-", "") + "Accordion";

            sb.AppendFormat("<section id=\"{0}\" class=\"pnu-section-anchor mb-5\">", SectionKey);
            if (!string.IsNullOrEmpty(sectionTitle))
                sb.AppendFormat("<h2>{0}</h2>", HttpUtility.HtmlEncode(sectionTitle));
            if (!string.IsNullOrEmpty(sectionIntro))
                sb.AppendFormat("<p>{0}</p>", HttpUtility.HtmlEncode(sectionIntro));

            if (items.Count > 0)
            {
                sb.AppendFormat("<div class=\"accordion accordion-flush mt-3\" id=\"{0}\">", accId);
                for (int i = 0; i < items.Count; i++)
                {
                    string hId = accId + "Heading" + (i + 1);
                    string cId = accId + "Collapse" + (i + 1);
                    bool open = (i == 0);

                    sb.Append("<div class=\"accordion-item\">");
                    sb.AppendFormat(
                        "<h3 class=\"accordion-header\" id=\"{0}\">" +
                        "<button aria-controls=\"{1}\" aria-expanded=\"{2}\" class=\"accordion-button{3}\" " +
                        "data-bs-target=\"#{1}\" data-bs-toggle=\"collapse\" type=\"button\">{4}</button></h3>",
                        hId, cId, open ? "true" : "false", open ? "" : " collapsed",
                        HttpUtility.HtmlEncode(items[i].Key));
                    sb.AppendFormat(
                        "<div class=\"accordion-collapse collapse{0}\" id=\"{1}\" aria-labelledby=\"{2}\" data-bs-parent=\"#{3}\">" +
                        "<div class=\"accordion-body\">{4}</div></div>",
                        open ? " show" : "", cId, hId, accId,
                        items[i].Value);   // rich HTML body from the list - rendered as-is
                    sb.Append("</div>");
                }
                sb.Append("</div>");
            }

            sb.Append("</section>");
            ltrSection.Text = sb.ToString();
        }

        private string Localize(SPListItem item, string fieldAr, string fieldEn)
        {
            string ar = item.Fields.ContainsField(fieldAr) && item[fieldAr] != null ? item[fieldAr].ToString() : "";
            string en = item.Fields.ContainsField(fieldEn) && item[fieldEn] != null ? item[fieldEn].ToString() : "";
            if (IsArabic) return !string.IsNullOrEmpty(ar) ? ar : en;
            return !string.IsNullOrEmpty(en) ? en : ar;
        }

        // ------------------------------------------------------------------
        //  Provisioning (from the page - idempotent)
        // ------------------------------------------------------------------

        private void EnsureLists()
        {
            bool needsWork;
            using (SPSite s = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb w = s.OpenWeb())
            {
                SPList sec = w.Lists.TryGetList(LIST_SECTIONS);
                SPList itm = w.Lists.TryGetList(LIST_ITEMS);
                needsWork = (sec == null || itm == null || sec.ItemCount == 0);
            }
            if (!needsWork) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    // ---- CollegeContentSections ----
                    SPList secList = web.Lists.TryGetList(LIST_SECTIONS);
                    if (secList == null)
                        secList = web.Lists[web.Lists.Add(LIST_SECTIONS,
                            "Section headers of the More-about-college tabs", SPListTemplateType.GenericList)];

                    AddTextField(secList, "SectionKey");
                    AddTextField(secList, "Title_EN");
                    AddNoteField(secList, "Intro");
                    AddNoteField(secList, "Intro_EN");
                    AddNumberField(secList, "ItemOrder");
                    secList.Update();

                    // ---- CollegeContentItems ----
                    SPList itemList = web.Lists.TryGetList(LIST_ITEMS);
                    if (itemList == null)
                        itemList = web.Lists[web.Lists.Add(LIST_ITEMS,
                            "Accordion items of the More-about-college tabs (Body = rich HTML)",
                            SPListTemplateType.GenericList)];

                    AddTextField(itemList, "SectionKey");
                    AddTextField(itemList, "Title_EN");
                    AddRichField(itemList, "Body");
                    AddRichField(itemList, "Body_EN");
                    AddNumberField(itemList, "ItemOrder");
                    AddBoolField(itemList, "Visibility", "1");
                    itemList.Update();

                    // ---- seed section headers once ----
                    if (secList.ItemCount == 0)
                    {
                        SeedSection(secList, "faculty-org-structure", "الهيكل التنظيمي للكلية", "Organizational Structure",
                            "يعرض الهيكل التنظيمي موقع الوحدات والارتباطات الإدارية داخل الكلية.",
                            "The organizational structure of the college and its administrative units.", 1);
                        SeedSection(secList, "faculty-achievements", "إنجازات الكلية", "College Achievements",
                            "مساحة مخصصة لإبراز منجزات الكلية ومشاركاتها النوعية.",
                            "Highlights of the college achievements and distinguished participations.", 2);
                        SeedSection(secList, "faculty-agencies", "وكالات الكلية", "College Vice Deanships",
                            "تعرض الوكالات المنشورة حاليًا ضمن هيكل الكلية.",
                            "The vice deanships currently published within the college structure.", 3);
                        SeedSection(secList, "faculty-facilities", "مرافق الكلية", "College Facilities",
                            "تشمل مرافق الكلية المساحات التعليمية والبحثية والمعامل التخصصية.",
                            "Educational and research spaces and specialized laboratories.", 4);
                        SeedSection(secList, "faculty-research", "البحث العلمي في الكلية", "Scientific Research",
                            "وحدات ومراكز البحث والابتكار في الكلية.",
                            "Research and innovation units and centers in the college.", 5);
                        SeedSection(secList, "faculty-students", "الطالبات في الكلية", "Students",
                            "روابط الإرشاد والخدمات والأنشطة والتدريب الموجهة للطالبات.",
                            "Advising, services, activities and training for students.", 6);
                        SeedSection(secList, "faculty-student-services", "الخدمات الطلابية في الكلية", "Student Services",
                            "خدمات الإرشاد الأكاديمي والنفسي والاجتماعي والمهني وإدارة الخدمات الطلابية.",
                            "Academic, psychological, social and career advising and student services.", 7);
                        SeedSection(secList, "faculty-clubs", "الأندية الطلابية في الكلية", "Student Clubs",
                            "تعرّف على الأندية الطلابية التعليمية والنادي الرياضي في الكلية.",
                            "Educational student clubs and the sports club of the college.", 8);
                        SeedSection(secList, "faculty-initiatives", "المبادرات", "Initiatives",
                            "مبادرات الكلية وبرامجها الداعمة للتميز الأكاديمي والمهني.",
                            "College initiatives supporting academic and professional excellence.", 9);
                        SeedSection(secList, "faculty-training", "التدريب في الكلية", "Training",
                            "معلومات التدريب التعاوني ولجنة التدريب ومنسقي التدريب في الأقسام.",
                            "Cooperative training information, committee and coordinators.", 10);
                    }

                    web.AllowUnsafeUpdates = false;
                }
            });
        }

        private static void SeedSection(SPList list, string key, string titleAr, string titleEn,
                                        string introAr, string introEn, double order)
        {
            SPListItem item = list.AddItem();
            item["Title"] = titleAr;
            item["Title_EN"] = titleEn;
            item["SectionKey"] = key;
            item["Intro"] = introAr;
            item["Intro_EN"] = introEn;
            item["ItemOrder"] = order;
            item.Update();
        }

        internal static void AddTextField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name)) list.Fields.Add(name, SPFieldType.Text, false);
        }

        internal static void AddNoteField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name)) list.Fields.Add(name, SPFieldType.Note, false);
        }

        internal static void AddRichField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                string fld = list.Fields.Add(name, SPFieldType.Note, false);
                SPFieldMultiLineText f = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(fld);
                f.RichText = true;
                f.RichTextMode = SPRichTextMode.FullHtml;
                f.Update();
            }
        }

        internal static void AddNumberField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name)) list.Fields.Add(name, SPFieldType.Number, false);
        }

        internal static void AddBoolField(SPList list, string name, string defaultValue)
        {
            if (!list.Fields.ContainsField(name))
            {
                string fld = list.Fields.Add(name, SPFieldType.Boolean, false);
                SPField f = list.Fields.GetFieldByInternalName(fld);
                f.DefaultValue = defaultValue;
                f.Update();
            }
        }
    }

}
