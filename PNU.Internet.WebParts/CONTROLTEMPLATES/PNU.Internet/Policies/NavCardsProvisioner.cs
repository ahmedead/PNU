// Provisioners/NavCardsProvisioner.cs
using System;
using System.Collections.Specialized;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies
{
    /// <summary>
    /// Idempotent provisioner for the NavigationCards list on the CURRENT web.
    /// Creates the list + columns if missing, seeds the four default cards when
    /// empty, and grants anonymous read. Elevated + reopened by ID inside the
    /// delegate, per the portal's anonymous-access pattern.
    /// </summary>
    public static class NavCardsProvisioner
    {
        public const string ListName = "NavigationCards";
        private static readonly object _lock = new object();

        /// <summary>
        /// Call from the control's OnInit (gated to authenticated users).
        /// </summary>
        public static void EnsureList()
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId = SPContext.Current.Web.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    lock (_lock)
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb web = site.OpenWeb(webId))
                        {
                            bool allowUnsafe = web.AllowUnsafeUpdates;
                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                SPList list = web.Lists.TryGetList(ListName);
                                if (list == null)
                                {
                                    Guid id = web.Lists.Add(
                                        ListName,
                                        "بطاقات التنقل - السياسات وحوكمة البيانات",
                                        SPListTemplateType.GenericList);
                                    list = web.Lists[id];
                                    list.OnQuickLaunch = false;
                                    list.Update();
                                }

                                EnsureColumns(list);
                                SeedIfEmpty(web, list);
                                GrantAnonymousRead(list);
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = allowUnsafe;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("NavCardsProvisioner.EnsureList", ListName, ex.Message);
            }
        }

        private static void EnsureColumns(SPList list)
        {
            // Title already exists on a GenericList.
            if (!list.Fields.ContainsField("Icon"))
                list.Fields.Add("Icon", SPFieldType.Text, true);

            if (!list.Fields.ContainsField("Url"))
            {
                string internalName = list.Fields.Add("Url", SPFieldType.Note, true);
                var f = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(internalName);
                f.RichText = false;
                f.NumberOfLines = 2;
                f.Update();
            }

            if (!list.Fields.ContainsField("ButtonText"))
                list.Fields.Add("ButtonText", SPFieldType.Text, true);

            if (!list.Fields.ContainsField("Target"))
            {
                var choices = new StringCollection();
                choices.AddRange(new[] { "_self", "_blank" });
                string internalName = list.Fields.Add("Target", SPFieldType.Choice, false, false, choices);
                var f = (SPFieldChoice)list.Fields.GetFieldByInternalName(internalName);
                f.DefaultValue = "_self";
                f.EditFormat = SPChoiceFormatType.Dropdown;
                f.Update();
            }

            if (!list.Fields.ContainsField("SortOrder"))
                list.Fields.Add("SortOrder", SPFieldType.Number, true);

            // Surface columns on the default view (Exists is a METHOD).
            SPView view = list.DefaultView;
            bool changed = false;
            foreach (string f in new[] { "Icon", "Url", "ButtonText", "Target", "SortOrder" })
            {
                if (!view.ViewFields.Exists(f)) { view.ViewFields.Add(f); changed = true; }
            }
            if (changed) view.Update();

            list.Update();
        }

        private static void SeedIfEmpty(SPWeb web, SPList list)
        {
            if (list.ItemCount > 0) return;

            AddCard(list, "سياسة الخصوصية", "hgi hgi-stroke hgi-shield-01",
                    "policy-details.html", "عرض التفاصيل", "_self", 1);

            AddCard(list, "سياسات حوكمة البيانات", "hgi hgi-stroke hgi-database",
                    "https://pnu.edu.sa/ar/Departments/DT/DM/Pages/home.aspx", "عرض التفاصيل", "_blank", 2);

            AddCard(list, "اتفاقية الاستخدام", "hgi hgi-stroke hgi-agreement-01",
                    "https://pnu.edu.sa/ar/SECURITYCYBER/Documents/term1.pdf", "عرض الملف", "_blank", 3);

            AddCard(list, "سياسة استخدام الذكاء الاصطناعي التوليدي", "hgi hgi-stroke hgi-ai-generative",
                    "https://pnu.edu.sa/ar/SECURITYCYBER/Documents/Generative_AI_Use_Policy_Ar.pdf", "عرض الملف", "_blank", 4);
        }

        private static void AddCard(SPList list, string title, string icon,
                                    string url, string buttonText, string target, double sortOrder)
        {
            SPListItem item = list.AddItem();
            item["Title"] = title;
            item["Icon"] = icon;
            item["Url"] = url;
            item["ButtonText"] = buttonText;
            item["Target"] = target;
            item["SortOrder"] = sortOrder;
            item.Update();
        }

        private static void GrantAnonymousRead(SPList list)
        {
            try
            {
                if (list.ReadSecurity != 1) list.ReadSecurity = 1;
                list.AnonymousPermMask64 = SPBasePermissions.ViewListItems | SPBasePermissions.ViewPages;
                list.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("NavCardsProvisioner.GrantAnonymousRead", ListName, ex.Message);
            }
        }
    }
}
