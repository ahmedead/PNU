using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// Provisions the CollegeDocuments list on the CURRENT web and seeds defaults.
    /// Category groups the accordion: Documents | Forms | Guides.
    /// Safe to call from OnInit / Page_Load (idempotent, seeds only when empty).
    /// </summary>
    public static class CollegeDocumentsProvisioner
    {
        public const string ListName = "CollegeDocuments";
        private static readonly object _lock = new object();

        public static void EnsureList()
        {
            try
            {
                lock (_lock)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            web.AllowUnsafeUpdates = true;

                            SPList list = web.Lists.TryGetList(ListName);
                            if (list == null)
                            {
                                Guid id = web.Lists.Add(ListName, "College documents, forms and guides",
                                    SPListTemplateType.GenericList);
                                list = web.Lists[id];
                                list.OnQuickLaunch = false;
                                list.Update();
                            }

                            EnsureField(list, "TitleEn", SPFieldType.Text);
                            EnsureField(list, "Category", SPFieldType.Text);      // Documents | Forms | Guides
                            EnsureField(list, "LinkUrl", SPFieldType.URL);
                            EnsureField(list, "SortOrder", SPFieldType.Number);

                            SPView dirview = list.DefaultView;
                            dirview.ViewFields.Add("TitleEn");
                            dirview.ViewFields.Add("Category");
                            dirview.ViewFields.Add("LinkUrl");
                            dirview.ViewFields.Add("SortOrder");

                            dirview.Update();


                            //if (list.ItemCount == 0)
                            //    SeedDefaults(list);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "CollegeDocumentsProvisioner - EnsureList", ex.Message);
            }
        }

        private static void EnsureField(SPList list, string name, SPFieldType type)
        {
            if (!list.Fields.ContainsField(name))
                list.Fields.Add(name, type, false);
            SPView view = list.DefaultView;
            if (!view.ViewFields.Exists(name))
            {
                view.ViewFields.Add(name);
                view.Update();
            }
        }

        private static void SeedDefaults(SPList list)
        {
            // Documents
            AddItem(list, "المستند الأول", "First Document", "Documents", "#", 1);
            AddItem(list, "المستند الثاني", "Second Document", "Documents", "#", 2);
            AddItem(list, "المستند الثالث", "Third Document", "Documents", "#", 3);
            // Forms
            AddItem(list, "النموذج الأول", "First Form", "Forms", "#", 1);
            AddItem(list, "النموذج الثاني", "Second Form", "Forms", "#", 2);
            AddItem(list, "النموذج الثالث", "Third Form", "Forms", "#", 3);
            // Guides
            AddItem(list, "الدليل الأول", "First Guide", "Guides", "#", 1);
            AddItem(list, "الدليل الثاني", "Second Guide", "Guides", "#", 2);
            AddItem(list, "الدليل الثالث", "Third Guide", "Guides", "#", 3);
        }

        private static void AddItem(SPList list, string ar, string en, string category, string url, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["Category"] = category;
            item["LinkUrl"] = new SPFieldUrlValue { Url = url, Description = ar };
            item["SortOrder"] = sort;
            item.Update();
        }
    }
}
