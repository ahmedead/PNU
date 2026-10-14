// Provisioners/NavCardsProvisioner.cs
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies
{
    /// <summary>
    /// Idempotent provisioner for the NavigationCards list on /ar/SECURITYCYBER.
    /// Creates the list + columns if missing, seeds default cards bilingually,
    /// patches existing items to ensure English metadata and correct policy URLs,
    /// and grants anonymous read. Runs at most once per process unless forced, and
    /// only writes to SharePoint when something actually changed.
    /// </summary>
    public static class NavCardsProvisioner
    {
        public const string ListName = "NavigationCards";
        public const string TargetWebRelativeUrl = "/ar/SECURITYCYBER";

        // Set on the list's root folder once the three newer policy cards have been added to an
        // existing list. After that they are never re-added, so admins can delete them.
        private const string PolicyCardsAddedFlag = "PNU_NavCards_PolicyCardsV2";

        private static readonly object _lock = new object();
        private static volatile bool _provisionedInProcess = false;

        /// <summary>
        /// Opens /ar/SECURITYCYBER only (never a fallback web). Returns null when it does not exist.
        /// Caller is responsible for disposing the returned SPWeb.
        /// </summary>
        public static SPWeb OpenTargetWeb(SPSite site)
        {
            if (site == null) return null;

            string root = (site.ServerRelativeUrl ?? "/").TrimEnd('/');
            string serverRelative = root + TargetWebRelativeUrl;

            SPWeb web = null;
            try
            {
                web = site.OpenWeb(serverRelative, true);
                if (web != null && web.Exists &&
                    (web.ServerRelativeUrl ?? "").TrimEnd('/').EndsWith(TargetWebRelativeUrl, StringComparison.OrdinalIgnoreCase))
                {
                    return web;
                }
            }
            catch { }

            if (web != null) web.Dispose();
            return null;
        }

        /// <summary>Parameterless entry point for ContentAdminListProvisioner (reflection).</summary>
        public static void EnsureAllListsExist()
        {
            EnsureList();
        }

        /// <summary>Always runs, ignoring the once-per-process guard (admin "Provision" button).</summary>
        public static bool ForceProvision()
        {
            return Provision();
        }

        /// <summary>Cheap guarded call: provisions once per process.</summary>
        public static void EnsureList()
        {
            if (_provisionedInProcess) return;
            Provision();
        }

        private static bool Provision()
        {
            bool ok = false;
            try
            {
                if (SPContext.Current == null) return false;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    lock (_lock)
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb web = OpenTargetWeb(site))
                        {
                            if (web == null) return;
                            ok = EnsureListOnWeb(web) != null;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("NavCardsProvisioner.Provision", ListName, ex.Message);
            }

            if (ok) _provisionedInProcess = true;
            return ok;
        }

        /// <summary>
        /// Creates/patches the list on an already-open (elevated) web and returns it.
        /// </summary>
        public static SPList EnsureListOnWeb(SPWeb web)
        {
            bool allowUnsafe = web.AllowUnsafeUpdates;
            web.AllowUnsafeUpdates = true;
            try
            {
                bool created = false;
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
                    created = true;
                }

                EnsureColumns(list);

                if (created)
                {
                    SeedDefaults(list);
                    SetFlag(list, PolicyCardsAddedFlag);
                }
                else
                {
                    FixKnownDataIssues(list);
                }

                GrantAnonymousRead(list);
                return list;
            }
            finally
            {
                web.AllowUnsafeUpdates = allowUnsafe;
            }
        }

        private static void EnsureColumns(SPList list)
        {
            bool changed = false;

            // Title already exists on a GenericList.
            if (!list.Fields.ContainsField("TitleEn"))
            {
                list.Fields.Add("TitleEn", SPFieldType.Text, false);
                changed = true;
            }

            if (!list.Fields.ContainsField("Icon"))
            {
                list.Fields.Add("Icon", SPFieldType.Text, true);
                changed = true;
            }

            if (!list.Fields.ContainsField("Url"))
            {
                AddPlainNote(list, "Url", true);
                changed = true;
            }

            if (!list.Fields.ContainsField("UrlEn"))
            {
                AddPlainNote(list, "UrlEn", false);
                changed = true;
            }

            if (!list.Fields.ContainsField("ButtonText"))
            {
                list.Fields.Add("ButtonText", SPFieldType.Text, true);
                changed = true;
            }

            if (!list.Fields.ContainsField("ButtonTextEn"))
            {
                list.Fields.Add("ButtonTextEn", SPFieldType.Text, false);
                changed = true;
            }

            if (!list.Fields.ContainsField("Target"))
            {
                var choices = new StringCollection();
                choices.AddRange(new[] { "_self", "_blank" });
                string internalName = list.Fields.Add("Target", SPFieldType.Choice, false, false, choices);
                var f = (SPFieldChoice)list.Fields.GetFieldByInternalName(internalName);
                f.DefaultValue = "_self";
                f.EditFormat = SPChoiceFormatType.Dropdown;
                f.Update();
                changed = true;
            }

            if (!list.Fields.ContainsField("SortOrder"))
            {
                list.Fields.Add("SortOrder", SPFieldType.Number, true);
                changed = true;
            }

            // Surface columns on the default view
            SPView view = list.DefaultView;
            bool viewChanged = false;
            foreach (string f in new[] { "Icon", "TitleEn", "Url", "UrlEn", "ButtonText", "ButtonTextEn", "Target", "SortOrder" })
            {
                if (!view.ViewFields.Exists(f)) { view.ViewFields.Add(f); viewChanged = true; }
            }
            if (viewChanged) view.Update();

            if (changed) list.Update();
        }

        private static void AddPlainNote(SPList list, string name, bool required)
        {
            string internalName = list.Fields.Add(name, SPFieldType.Note, required);
            var f = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(internalName);
            f.RichText = false;
            f.NumberOfLines = 2;
            f.Update();
        }

        private static void SeedDefaults(SPList list)
        {
            // 1. Privacy Policy
            AddCard(list, "سياسة الخصوصية", "Privacy Policy",
                    "hgi hgi-stroke hgi-shield-01",
                    "/ar/SECURITYCYBER/Pages/DataPrivacyPolicy.aspx",
                    "/en/SECURITYCYBER/Pages/DataPrivacyPolicy.aspx",
                    "عرض التفاصيل", "View Details", "_self", 1);

            // 2. Data Governance Policies (points to DMPolicy.aspx, NOT home.aspx)
            AddCard(list, "سياسات حوكمة البيانات", "Data Governance Policies",
                    "hgi hgi-stroke hgi-database",
                    "/ar/Departments/DM/Pages/DMPolicy.aspx",
                    "/en/Departments/DM/Pages/DMPolicy.aspx",
                    "عرض التفاصيل", "View Details", "_blank", 2);

            AddAssetManagementCard(list);
            AddAcceptableUseCard(list);
            AddItGovernanceCard(list);

            // 6. Terms of Use
            AddCard(list, "اتفاقية الاستخدام", "Terms of Use",
                    "hgi hgi-stroke hgi-agreement-01",
                    "/ar/SECURITYCYBER/Documents/term1.pdf",
                    "/en/SECURITYCYBER/Documents/term1_en.pdf",
                    "عرض الملف", "View Document", "_blank", 6);

            // 7. Generative AI Use Policy
            AddCard(list, "سياسة استخدام الذكاء الاصطناعي التوليدي", "Generative AI Use Policy",
                    "hgi hgi-stroke hgi-ai-generative",
                    "/ar/SECURITYCYBER/Documents/Generative_AI_Use_Policy_Ar.pdf",
                    "/en/SECURITYCYBER/Documents/Generative_AI_Use_Policy_En.pdf",
                    "عرض الملف", "View Document", "_blank", 7);
        }

        private static void AddAssetManagementCard(SPList list)
        {
            AddCard(list, "سياسات إدارة أصول المعلومات", "Information Asset Management Policy",
                    "hgi hgi-stroke hgi-folder-security",
                    "/ar/SECURITYCYBER/Documents/Information_Asset_Management_Policy_Ar.pdf",
                    "/en/SECURITYCYBER/Documents/Information_Asset_Management_Policy_En.pdf",
                    "عرض الملف", "View Document", "_blank", 3);
        }

        private static void AddAcceptableUseCard(SPList list)
        {
            AddCard(list, "سياسة الاستخدام المقبول لأصول المعلومات", "Acceptable Use Policy of Information Assets",
                    "hgi hgi-stroke hgi-user-check-01",
                    "/ar/SECURITYCYBER/Documents/Acceptable_Use_Policy_Ar.pdf",
                    "/en/SECURITYCYBER/Documents/Acceptable_Use_Policy_En.pdf",
                    "عرض الملف", "View Document", "_blank", 4);
        }

        private static void AddItGovernanceCard(SPList list)
        {
            AddCard(list, "سياسات تقنية المعلومات وحوكمتها", "IT and Governance Policies",
                    "hgi hgi-stroke hgi-computer-programming-01",
                    "/ar/SECURITYCYBER/Documents/IT_Governance_Policy_Ar.pdf",
                    "/en/SECURITYCYBER/Documents/IT_Governance_Policy_En.pdf",
                    "عرض الملف", "View Document", "_blank", 5);
        }

        /// <summary>
        /// One-time patches for lists created by earlier versions: fills missing English fields,
        /// fixes the old Data Governance URL, and adds the three newer policy cards once.
        /// </summary>
        private static void FixKnownDataIssues(SPList list)
        {
            try
            {
                bool hasAssetMgmt = false;
                bool hasAcceptableUse = false;
                bool hasItGov = false;

                foreach (SPListItem item in list.Items)
                {
                    string title = Convert.ToString(item["Title"]) ?? "";
                    string currentUrl = Convert.ToString(item["Url"]) ?? "";
                    bool updated = false;

                    // 1. Data Governance URL fix (from DM home to DMPolicy.aspx)
                    if (title.Contains("حوكمة البيانات") || title.IndexOf("Data Governance", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (currentUrl.Contains("DT/DM/Pages/home.aspx") || currentUrl.EndsWith("/home.aspx", StringComparison.OrdinalIgnoreCase))
                        {
                            item["Url"] = "/ar/Departments/DM/Pages/DMPolicy.aspx";
                            updated = true;
                        }
                        updated |= FillIfEmpty(item, "TitleEn", "Data Governance Policies");
                        updated |= FillIfEmpty(item, "UrlEn", "/en/Departments/DM/Pages/DMPolicy.aspx");
                        updated |= FillIfEmpty(item, "ButtonTextEn", "View Details");
                    }

                    // 2. Privacy Policy English fields
                    if (title.Contains("الخصوصية") || title.IndexOf("Privacy Policy", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        updated |= FillIfEmpty(item, "TitleEn", "Privacy Policy");
                        updated |= FillIfEmpty(item, "UrlEn", "/en/SECURITYCYBER/Pages/DataPrivacyPolicy.aspx");
                        updated |= FillIfEmpty(item, "ButtonTextEn", "View Details");
                    }

                    // 3. Terms of Use English fields
                    if (title.Contains("اتفاقية الاستخدام") || title.IndexOf("Terms of Use", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        updated |= FillIfEmpty(item, "TitleEn", "Terms of Use");
                        updated |= FillIfEmpty(item, "UrlEn", "/en/SECURITYCYBER/Documents/term1_en.pdf");
                        updated |= FillIfEmpty(item, "ButtonTextEn", "View Document");
                    }

                    // 4. Generative AI Policy English fields
                    if (title.Contains("الذكاء الاصطناعي") || title.IndexOf("Generative AI", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        updated |= FillIfEmpty(item, "TitleEn", "Generative AI Use Policy");
                        updated |= FillIfEmpty(item, "UrlEn", "/en/SECURITYCYBER/Documents/Generative_AI_Use_Policy_En.pdf");
                        updated |= FillIfEmpty(item, "ButtonTextEn", "View Document");
                    }

                    // 5. Information Asset Management
                    if (title.Contains("أصول المعلومات") || title.IndexOf("Asset Management", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        hasAssetMgmt = true;
                        updated |= FillIfEmpty(item, "TitleEn", "Information Asset Management Policy");
                        updated |= FillIfEmpty(item, "UrlEn", currentUrl.Contains("_En.pdf") ? currentUrl : "/en/SECURITYCYBER/Documents/Information_Asset_Management_Policy_En.pdf");
                        updated |= FillIfEmpty(item, "ButtonTextEn", "View Document");
                    }

                    // 6. Acceptable Use Policy
                    if (title.Contains("الاستخدام المقبول") || title.IndexOf("Acceptable Use", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        hasAcceptableUse = true;
                        updated |= FillIfEmpty(item, "TitleEn", "Acceptable Use Policy of Information Assets");
                        updated |= FillIfEmpty(item, "UrlEn", currentUrl.Contains("_En.pdf") ? currentUrl : "/en/SECURITYCYBER/Documents/Acceptable_Use_Policy_En.pdf");
                        updated |= FillIfEmpty(item, "ButtonTextEn", "View Document");
                    }

                    // 7. IT Governance Policy
                    if (title.Contains("تقنية المعلومات") || title.IndexOf("IT Governance", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        hasItGov = true;
                        updated |= FillIfEmpty(item, "TitleEn", "IT and Governance Policies");
                        updated |= FillIfEmpty(item, "UrlEn", currentUrl.Contains("_En.pdf") ? currentUrl : "/en/SECURITYCYBER/Documents/IT_Governance_Policy_En.pdf");
                        updated |= FillIfEmpty(item, "ButtonTextEn", "View Document");
                    }

                    if (updated)
                    {
                        item.Update();
                    }
                }

                // Add the newer policy cards to an existing list ONCE. After the flag is set,
                // a card deleted by an admin stays deleted.
                if (!HasFlag(list, PolicyCardsAddedFlag))
                {
                    if (!hasAssetMgmt) AddAssetManagementCard(list);
                    if (!hasAcceptableUse) AddAcceptableUseCard(list);
                    if (!hasItGov) AddItGovernanceCard(list);
                    SetFlag(list, PolicyCardsAddedFlag);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("NavCardsProvisioner.FixKnownDataIssues", ListName, ex.Message);
            }
        }

        private static bool FillIfEmpty(SPListItem item, string field, string value)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(item[field]))) return false;
            item[field] = value;
            return true;
        }

        private static bool HasFlag(SPList list, string key)
        {
            return list.RootFolder.Properties.ContainsKey(key);
        }

        private static void SetFlag(SPList list, string key)
        {
            list.RootFolder.Properties[key] = "1";
            list.RootFolder.Update();
        }

        private static void AddCard(SPList list, string title, string titleEn, string icon,
                                    string url, string urlEn, string buttonText, string buttonTextEn,
                                    string target, double sortOrder)
        {
            SPListItem item = list.AddItem();
            item["Title"] = title;
            item["TitleEn"] = titleEn;
            item["Icon"] = icon;
            item["Url"] = url;
            item["UrlEn"] = urlEn;
            item["ButtonText"] = buttonText;
            item["ButtonTextEn"] = buttonTextEn;
            item["Target"] = target;
            item["SortOrder"] = sortOrder;
            item.Update();
        }

        private static void GrantAnonymousRead(SPList list)
        {
            try
            {
                SPBasePermissions wanted = SPBasePermissions.ViewListItems | SPBasePermissions.ViewPages;
                if (list.ReadSecurity == 1 && (list.AnonymousPermMask64 & wanted) == wanted) return;

                list.ReadSecurity = 1;
                list.AnonymousPermMask64 = list.AnonymousPermMask64 | wanted;
                list.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("NavCardsProvisioner.GrantAnonymousRead", ListName, ex.Message);
            }
        }

        #region Read helpers (shared by the public control and the admin)

        public static SPQuery BuildOrderedQuery()
        {
            return new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/><FieldRef Name='ID' Ascending='TRUE'/></OrderBy>",
                RowLimit = 500
            };
        }

        /// <summary>
        /// Maps list rows by hand. SPFactory.MapListItemsToClass only fills string/bool
        /// properties, which left ID and SortOrder at 0.
        /// </summary>
        public static List<NavCardsList> ReadCards(SPList list, bool isArabic)
        {
            var cards = new List<NavCardsList>();
            if (list == null) return cards;

            foreach (SPListItem row in list.GetItems(BuildOrderedQuery()))
            {
                cards.Add(MapItem(row, isArabic));
            }
            return cards;
        }

        public static NavCardsList MapItem(SPListItem row, bool isArabic)
        {
            double order;
            object rawOrder = row.Fields.ContainsField("SortOrder") ? row["SortOrder"] : null;
            if (rawOrder is double) order = (double)rawOrder;
            else if (!double.TryParse(Convert.ToString(rawOrder), NumberStyles.Any, CultureInfo.InvariantCulture, out order)) order = 0;

            return new NavCardsList
            {
                ID = row.ID,
                Title = Field(row, "Title"),
                TitleEn = Field(row, "TitleEn"),
                Icon = Field(row, "Icon"),
                Url = Field(row, "Url"),
                UrlEn = Field(row, "UrlEn"),
                ButtonText = Field(row, "ButtonText"),
                ButtonTextEn = Field(row, "ButtonTextEn"),
                Target = Field(row, "Target"),
                SortOrder = order,
                IsArabic = isArabic
            };
        }

        private static string Field(SPListItem row, string name)
        {
            if (!row.Fields.ContainsField(name)) return string.Empty;
            object v = row[name];
            return v != null ? v.ToString() : string.Empty;
        }

        #endregion
    }
}
