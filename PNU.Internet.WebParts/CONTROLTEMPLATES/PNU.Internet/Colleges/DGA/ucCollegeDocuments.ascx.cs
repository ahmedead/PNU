using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// "المستندات والنماذج والأدلة" tab - DGA design.
    /// New list (provisioned from the page): CollegeDocuments
    ///   Title/Title_EN, Category (Documents | Forms | Guides), FileUrl, ItemOrder, Visibility.
    /// Renders one accordion group per category with nav-cards linking to the files.
    /// </summary>
    public partial class ucCollegeDocuments : UserControl
    {
        private string _listName;

        [WebBrowsable(true),
         Category("List Settings"),
         DefaultValue("CollegeDocuments"),
         Description("Name of the SharePoint list containing college documents.")]
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LIST_NAME
        {
            get
            {
                if (string.IsNullOrEmpty(_listName)) return "CollegeDocuments";
                return _listName;
            }
            set
            {
                _listName = value;
            }
        }

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        private class DocEntry { public string Title, FileUrl, Category; public double Order; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureList();
                if (!IsPostBack || ltrDocuments.Text.Length == 0)
                    RenderDocuments();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void RenderDocuments()
        {
            List<DocEntry> docs = new List<DocEntry>();

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(LIST_NAME);
                if (list == null) return;

                SPQuery q = new SPQuery
                {
                    Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                              <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                };
                foreach (SPListItem item in list.GetItems(q))
                {
                    docs.Add(new DocEntry
                    {
                        Title = IsArabic
                            ? ucSideMenu.GetFirstFieldValue(item, "Title")
                            : ucSideMenu.GetFirstFieldValue(item, "Title_EN", "Title"),
                        FileUrl = ucSideMenu.GetFirstFieldValue(item, "FileUrl"),
                        Category = ucSideMenu.GetFirstFieldValue(item, "Category"),
                        Order = item.Fields.ContainsField("ItemOrder") && item["ItemOrder"] != null
                                ? Convert.ToDouble(item["ItemOrder"]) : 0
                    });
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<h2 class=\"mb-4\">{0}</h2>",
                IsArabic ? "المستندات والنماذج والأدلة" : "Documents, Forms and Guides");
            sb.Append("<div class=\"accordion accordion-flush\" id=\"faculty-documentsAccordion\">");

            string[] categories = { "Documents", "Forms", "Guides" };
            string[] labelsAr = { "المستندات", "النماذج", "الأدلة" };
            string[] labelsEn = { "Documents", "Forms", "Guides" };

            for (int g = 0; g < categories.Length; g++)
            {
                List<DocEntry> group = docs.Where(d =>
                    string.Equals(d.Category, categories[g], StringComparison.OrdinalIgnoreCase))
                    .OrderBy(d => d.Order).ToList();
                if (group.Count == 0) continue;

                string hId = "faculty-documentsAccordionHeading" + (g + 1);
                string cId = "faculty-documentsAccordionCollapse" + (g + 1);

                sb.Append("<div class=\"accordion-item\">");
                sb.AppendFormat(
                    "<p class=\"accordion-header\" id=\"{0}\">" +
                    "<button class=\"accordion-button collapsed\" type=\"button\" data-bs-toggle=\"collapse\" " +
                    "data-bs-target=\"#{1}\" aria-expanded=\"false\" aria-controls=\"{1}\">{2}</button></p>",
                    hId, cId, IsArabic ? labelsAr[g] : labelsEn[g]);

                sb.AppendFormat(
                    "<div class=\"accordion-collapse collapse\" id=\"{0}\" aria-labelledby=\"{1}\" " +
                    "data-bs-parent=\"#faculty-documentsAccordion\"><div class=\"accordion-body\"><div class=\"row g-4\">",
                    cId, hId);

                foreach (DocEntry doc in group)
                {
                    sb.Append("<div class=\"col-12 col-md-6\">");
                    sb.Append("<div class=\"card nav-card h-100\"><div class=\"d-flex card-body flex-column gap-4\">");
                    sb.Append("<div class=\"icon-container\"><i class=\"hgi hgi-stroke hgi-file-02 fs-3\" aria-hidden=\"true\"></i></div>");
                    sb.AppendFormat("<div><h3 class=\"card-title\">{0}</h3></div>", HttpUtility.HtmlEncode(doc.Title));
                    sb.Append("<div class=\"d-flex justify-content-end mt-auto\">");
                    sb.AppendFormat(
                        "<a class=\"btn btn-secondary stretched-link\" href=\"{0}\" target=\"_blank\" " +
                        "rel=\"noopener noreferrer\" aria-label=\"{1}\">" +
                        "<i class=\"hgi hgi-stroke hgi-arrow-left-02 fs-4\" aria-hidden=\"true\"></i></a>",
                        string.IsNullOrEmpty(doc.FileUrl) ? "#" : doc.FileUrl,
                        HttpUtility.HtmlEncode(doc.Title));
                    sb.Append("</div></div></div></div>");
                }

                sb.Append("</div></div></div></div>");
            }

            sb.Append("</div>");
            ltrDocuments.Text = sb.ToString();
        }

        private void EnsureList()
        {
            bool needsWork;
            using (SPSite s = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb w = s.OpenWeb())
            {
                needsWork = (w.Lists.TryGetList(LIST_NAME) == null);
            }
            if (!needsWork) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.Lists.TryGetList(LIST_NAME);
                    if (list == null)
                        list = web.Lists[web.Lists.Add(LIST_NAME,
                            "College documents, forms and guides", SPListTemplateType.GenericList)];

                    ucCollegeContentSection.AddTextField(list, "Title_EN");
                    ucCollegeContentSection.AddTextField(list, "FileUrl");

                    if (!list.Fields.ContainsField("Category"))
                    {
                        string fld = list.Fields.Add("Category", SPFieldType.Choice, false);
                        SPFieldChoice choice = (SPFieldChoice)list.Fields.GetFieldByInternalName(fld);
                        choice.Choices.Add("Documents");
                        choice.Choices.Add("Forms");
                        choice.Choices.Add("Guides");
                        choice.DefaultValue = "Documents";
                        choice.Update();
                    }

                    ucCollegeContentSection.AddNumberField(list, "ItemOrder");
                    ucCollegeContentSection.AddBoolField(list, "Visibility", "1");
                    list.Update();

                    web.AllowUnsafeUpdates = false;
                }
            });
        }
    }

}
