using System;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// Provisions the ProgramsDocuments list on the CURRENT web and seeds defaults.
    /// Feeds the "الأدلة" (Guides) accordion group inside ucCollegeProgramsDga.
    /// Idempotent; seeds only when the list is empty.
    /// </summary>
    public static class ProgramsDocumentsProvisioner
    {
        public const string ListName = "ProgramsDocuments";
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
                                Guid id = web.Lists.Add(ListName,
                                    "Program guides shown in the Programs tab",
                                    SPListTemplateType.GenericList);
                                list = web.Lists[id];
                                list.OnQuickLaunch = false;
                                list.Update();
                            }

                            EnsureField(list, "TitleEn", SPFieldType.Text);
                            EnsureField(list, "LinkUrl", SPFieldType.URL);
                            EnsureField(list, "SortOrder", SPFieldType.Number);

                            SPView dirview = list.DefaultView;
                            dirview.ViewFields.Add("TitleEn");
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
                    "ProgramsDocumentsProvisioner - EnsureList", ex.Message);
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
            AddItem(list, "برنامج نظم المعلومات", "Information Systems Program",
                "https://pnu.edu.sa/ar/Faculties/IT/Pages/IS-Guides.aspx", 1);
            AddItem(list, "برنامج علوم الحاسبات", "Computer Science Program",
                "https://pnu.edu.sa/ar/Faculties/IT/Pages/CS-Guides.aspx", 2);
            AddItem(list, "برنامج تقنية المعلومات", "Information Technology Program",
                "https://pnu.edu.sa/ar/Faculties/IT/Pages/IT-Guides.aspx", 3);
            AddItem(list, "برنامج الذكاء الاصطناعي", "Artificial Intelligence Program",
                "https://pnu.edu.sa/ar/Faculties/IT/Pages/AIS.aspx", 4);
        }

        private static void AddItem(SPList list, string ar, string en, string url, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["LinkUrl"] = new SPFieldUrlValue { Url = url, Description = ar };
            item["SortOrder"] = sort;
            item.Update();
        }
    }
}
