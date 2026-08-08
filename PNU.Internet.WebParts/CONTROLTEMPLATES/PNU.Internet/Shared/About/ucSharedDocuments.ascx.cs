using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    /// <summary>
    /// "المستندات والنماذج والأدلة" tab - DGA design.
    /// Provisions the list named by LIST_NAME (default: CollegeDocuments)
    ///   Title/Title_EN, Category (Documents | Forms | Guides), FileUrl, ItemOrder, Visibility.
    /// Renders one accordion group per category with nav-cards linking to the files.
    /// </summary>
    public partial class ucSharedDocuments : UserControl
    {
        private string _listName;

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

        // =====================================================================
        // DTOs
        // =====================================================================
        public class DocCategoryGroup
        {
            public string Label { get; set; }
            public string HeadingId { get; set; }
            public string CollapseId { get; set; }
            public List<DocItem> Items { get; set; }
        }

        public class DocItem
        {
            public string Title { get; set; }
            public string FileUrl { get; set; }
        }

        // =====================================================================
        // lifecycle
        // =====================================================================
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureList();
                if (!IsPostBack)
                    BindDocuments();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // =====================================================================
        // binding
        // =====================================================================
        private void BindDocuments()
        {
            litHeading.Text = HttpUtility.HtmlEncode(
                IsArabic ? "المستندات والنماذج والأدلة" : "Documents, Forms and Guides");

            var docs = LoadDocs();
            var groups = BuildGroups(docs);

            if (groups.Count == 0)
            {
                secDocuments.Visible = false;
                return;
            }

            rptCategories.DataSource = groups;
            rptCategories.DataBind();
        }

        protected void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var group = (DocCategoryGroup)e.Item.DataItem;
            var inner = e.Item.FindControl("rptDocs") as Repeater;
            if (inner != null)
            {
                inner.DataSource = group.Items;
                inner.DataBind();
            }
        }

        // =====================================================================
        // data
        // =====================================================================
        private class DocEntry { public string Title, FileUrl, Category; public double Order; }

        private List<DocEntry> LoadDocs()
        {
            var docs = new List<DocEntry>();

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(LIST_NAME);
                if (list == null) return docs;

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
                            ? GetFirstFieldValue(item, "Title")
                            : GetFirstFieldValue(item, "Title_EN", "Title"),
                        FileUrl = GetFirstFieldValue(item, "FileUrl"),
                        Category = GetFirstFieldValue(item, "Category"),
                        Order = item.Fields.ContainsField("ItemOrder") && item["ItemOrder"] != null
                                ? Convert.ToDouble(item["ItemOrder"]) : 0
                    });
                }
            }
            return docs;
        }

        private List<DocCategoryGroup> BuildGroups(List<DocEntry> docs)
        {
            string[] categories = { "Documents", "Forms", "Guides" };
            string[] labelsAr   = { "المستندات", "النماذج", "الأدلة" };
            string[] labelsEn   = { "Documents", "Forms", "Guides" };

            var groups = new List<DocCategoryGroup>();

            for (int g = 0; g < categories.Length; g++)
            {
                var items = docs
                    .Where(d => string.Equals(d.Category, categories[g], StringComparison.OrdinalIgnoreCase))
                    .OrderBy(d => d.Order)
                    .Select(d => new DocItem
                    {
                        Title   = HttpUtility.HtmlEncode(d.Title),
                        FileUrl = string.IsNullOrEmpty(d.FileUrl) ? "#" : d.FileUrl
                    })
                    .ToList();

                if (items.Count == 0) continue;

                groups.Add(new DocCategoryGroup
                {
                    Label       = HttpUtility.HtmlEncode(IsArabic ? labelsAr[g] : labelsEn[g]),
                    HeadingId   = "faculty-documentsAccordionHeading" + (g + 1),
                    CollapseId  = "faculty-documentsAccordionCollapse" + (g + 1),
                    Items       = items
                });
            }

            return groups;
        }

        // =====================================================================
        // provisioning
        // =====================================================================
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
                            "Documents, forms and guides", SPListTemplateType.GenericList)];

                    AddTextField(list, "Title_EN");
                    AddTextField(list, "FileUrl");

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

                    AddNumberField(list, "ItemOrder");
                    AddBoolField(list, "Visibility", "1");
                    list.Update();

                    web.AllowUnsafeUpdates = false;
                }
            });
        }

        // =====================================================================
        // helpers
        // =====================================================================
        private static void AddTextField(SPList list, string fieldName)
        {
            if (!list.Fields.ContainsField(fieldName))
                list.Fields.Add(fieldName, SPFieldType.Text, false);
        }

        private static void AddNumberField(SPList list, string fieldName)
        {
            if (!list.Fields.ContainsField(fieldName))
                list.Fields.Add(fieldName, SPFieldType.Number, false);
        }

        private static void AddBoolField(SPList list, string fieldName, string defaultValue)
        {
            if (!list.Fields.ContainsField(fieldName))
            {
                string fld = list.Fields.Add(fieldName, SPFieldType.Boolean, false);
                SPFieldBoolean b = (SPFieldBoolean)list.Fields.GetFieldByInternalName(fld);
                b.DefaultValue = defaultValue;
                b.Update();
            }
        }

        private static string GetFirstFieldValue(SPListItem item, params string[] internalNames)
        {
            foreach (string name in internalNames)
            {
                if (item.Fields.ContainsField(name) && item[name] != null)
                {
                    string val = item[name].ToString().Trim();
                    if (!string.IsNullOrEmpty(val)) return val;
                }
            }
            return "";
        }
    }
}
