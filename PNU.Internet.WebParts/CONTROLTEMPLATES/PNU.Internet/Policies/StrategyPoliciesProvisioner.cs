using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies
{
    /// <summary>
    /// Idempotent provisioner for Strategy, Policies and Regulations lists stored on /ar/SECURITYCYBER
    /// (rendered on /ar/AboutUniversity/Pages/StrategyPoliciesRegulations.aspx).
    /// Manages:
    /// 1. StrategyPoliciesSections: Section titles and descriptions (Strategies, Policies, Regulations)
    /// 2. StrategyPoliciesCards: Individual cards with badges, icons, links and targets.
    /// </summary>
    public static class StrategyPoliciesProvisioner
    {
        public const string ListSections = "StrategyPoliciesSections";
        public const string ListCards = "StrategyPoliciesCards";
        public const string TargetWebRelativeUrl = "/ar/SECURITYCYBER";

        private static readonly object _lock = new object();
        private static volatile bool _provisionedInProcess = false;

        /// <summary>
        /// Opens /ar/SECURITYCYBER. Returns null (and logs) when the web cannot be opened —
        /// no fallback, so lists are never created or read on an unintended web.
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
            catch (Exception ex)
            {
                Publics.WriteToLog("StrategyPoliciesProvisioner.OpenTargetWeb", serverRelative, ex.Message);
            }

            if (web != null) web.Dispose();
            Publics.WriteToLog("StrategyPoliciesProvisioner.OpenTargetWeb", serverRelative, "Target web not found.");
            return null;
        }

        public static void EnsureAllListsExist()
        {
            EnsureLists();
        }

        public static bool ForceProvision()
        {
            return Provision();
        }

        public static void EnsureLists()
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
                            ok = EnsureListsOnWeb(web);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("StrategyPoliciesProvisioner.Provision", ListCards, ex.Message);
            }

            if (ok) _provisionedInProcess = true;
            return ok;
        }

        public static bool EnsureListsOnWeb(SPWeb web)
        {
            bool allowUnsafe = web.AllowUnsafeUpdates;
            web.AllowUnsafeUpdates = true;
            try
            {
                SPList sectionsList = EnsureSectionsList(web);
                SPList cardsList = EnsureCardsList(web);

                GrantAnonymousRead(sectionsList);
                GrantAnonymousRead(cardsList);

                return sectionsList != null && cardsList != null;
            }
            finally
            {
                web.AllowUnsafeUpdates = allowUnsafe;
            }
        }

        #region Sections List Provisioning

        private static SPList EnsureSectionsList(SPWeb web)
        {
            bool created = false;
            SPList list = web.Lists.TryGetList(ListSections);
            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ListSections,
                    "أقسام وعناوين الاستراتيجيات والسياسات والأنظمة واللوائح",
                    SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
                created = true;
            }

            bool changed = false;

            if (!list.Fields.ContainsField("SectionKey"))
            {
                list.Fields.Add("SectionKey", SPFieldType.Text, true);
                changed = true;
            }

            if (!list.Fields.ContainsField("TitleEn"))
            {
                list.Fields.Add("TitleEn", SPFieldType.Text, false);
                changed = true;
            }

            if (!list.Fields.ContainsField("Description"))
            {
                AddPlainNote(list, "Description", false);
                changed = true;
            }

            if (!list.Fields.ContainsField("DescriptionEn"))
            {
                AddPlainNote(list, "DescriptionEn", false);
                changed = true;
            }

            if (!list.Fields.ContainsField("ItemOrder"))
            {
                list.Fields.Add("ItemOrder", SPFieldType.Number, true);
                changed = true;
            }

            if (!list.Fields.ContainsField("Visibility"))
            {
                list.Fields.Add("Visibility", SPFieldType.Boolean, false);
                changed = true;
            }

            // Default view fields
            SPView view = list.DefaultView;
            bool viewChanged = false;
            foreach (string f in new[] { "SectionKey", "TitleEn", "Description", "DescriptionEn", "ItemOrder", "Visibility" })
            {
                if (!view.ViewFields.Exists(f)) { view.ViewFields.Add(f); viewChanged = true; }
            }
            if (viewChanged) view.Update();
            if (changed) list.Update();

            // Seed only on first creation so admin deletions are never undone
            if (created)
            {
                SeedSections(list);
            }

            return list;
        }

        private static void SeedSections(SPList list)
        {
            AddSectionRow(list, "strategies", "الاستراتيجيات", "Strategies",
                "قائمة بروابط الاستراتيجيات التي تتبناها الجامعة، مع تمييز الاستراتيجيات السابقة بعلامة مؤرشفة.",
                "List of strategies adopted by the university, highlighting past strategies with an archived badge.",
                1);

            AddSectionRow(list, "policies", "السياسات", "Policies",
                "قائمة بروابط نصوص السياسات والوثائق ذات العلاقة بعمل الجامعة وخدماتها الرقمية.",
                "List of links to policies and documents related to the university's work and digital services.",
                2);

            AddSectionRow(list, "regulations", "الأنظمة واللوائح", "Regulations & Bylaws",
                "روابط تقود إلى صفحات الأنظمة واللوائح والوثائق التنظيمية ذات العلاقة.",
                "Links to regulations, bylaws, and related regulatory documents.",
                3);
        }

        private static void AddSectionRow(SPList list, string key, string titleAr, string titleEn, string descAr, string descEn, double order)
        {
            SPListItem item = list.AddItem();
            item["Title"] = titleAr;
            item["SectionKey"] = key;
            item["TitleEn"] = titleEn;
            item["Description"] = descAr;
            item["DescriptionEn"] = descEn;
            item["ItemOrder"] = order;
            item["Visibility"] = true;
            item.Update();
        }

        #endregion

        #region Cards List Provisioning

        private static SPList EnsureCardsList(SPWeb web)
        {
            bool created = false;
            SPList list = web.Lists.TryGetList(ListCards);
            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ListCards,
                    "بطاقات وروابط الاستراتيجيات والسياسات والأنظمة واللوائح",
                    SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
                created = true;
            }

            bool changed = false;

            if (!list.Fields.ContainsField("SectionKey"))
            {
                list.Fields.Add("SectionKey", SPFieldType.Text, true);
                changed = true;
            }

            if (!list.Fields.ContainsField("TitleEn"))
            {
                list.Fields.Add("TitleEn", SPFieldType.Text, false);
                changed = true;
            }

            if (!list.Fields.ContainsField("Badge"))
            {
                list.Fields.Add("Badge", SPFieldType.Text, false);
                changed = true;
            }

            if (!list.Fields.ContainsField("BadgeEn"))
            {
                list.Fields.Add("BadgeEn", SPFieldType.Text, false);
                changed = true;
            }

            if (!list.Fields.ContainsField("BadgeClass"))
            {
                list.Fields.Add("BadgeClass", SPFieldType.Text, false);
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

            if (!list.Fields.ContainsField("Visibility"))
            {
                list.Fields.Add("Visibility", SPFieldType.Boolean, false);
                changed = true;
            }

            // Default view fields
            SPView view = list.DefaultView;
            bool viewChanged = false;
            foreach (string f in new[] { "SectionKey", "TitleEn", "Badge", "BadgeEn", "BadgeClass", "Icon", "Url", "UrlEn", "ButtonText", "ButtonTextEn", "Target", "SortOrder", "Visibility" })
            {
                if (!view.ViewFields.Exists(f)) { view.ViewFields.Add(f); viewChanged = true; }
            }
            if (viewChanged) view.Update();
            if (changed) list.Update();

            // Seed only on first creation so admin deletions are never undone
            if (created)
            {
                SeedCards(list);
            }

            return list;
        }

        private static void SeedCards(SPList list)
        {
            // === 1. Strategies (الاستراتيجيات) ===
            AddCardRow(list, "strategies",
                "الخطة الاستراتيجية للجامعة PNU 2021 - 2025",
                "University Strategic Plan PNU 2021 - 2025",
                "مؤرشفة", "Archived", "badge-warning",
                "hgi hgi-stroke hgi-file-02",
                "https://pnu.edu.sa/ar/Departments/NouraGateForTransformation/PD/Lists/Guidelines/Attachments/4/Strategic%20plan%20document%20AR.pdf",
                "https://pnu.edu.sa/en/Departments/NouraGateForTransformation/PD/Lists/Guidelines/Attachments/4/Strategic%20plan%20document%20EN.pdf",
                "عرض الملف", "View Document",
                "_blank", 1);

            AddCardRow(list, "strategies",
                "خبر اعتماد الخطة الاستراتيجية لجامعة الأميرة نورة 2025",
                "Approval Announcement for Princess Nourah University Strategic Plan 2025",
                "مؤرشفة", "Archived", "badge-warning",
                "hgi hgi-stroke hgi-file-02",
                "https://pnu.edu.sa/ar/NewsActivities/Pages/news2861.aspx",
                "https://pnu.edu.sa/en/NewsActivities/Pages/news2861.aspx",
                "عرض التفاصيل", "View Details",
                "_blank", 2);

            // === 2. Policies (السياسات) ===
            AddCardRow(list, "policies",
                "سياسات الجهة ضمن صلب عملها",
                "Entity Policies Within Core Mandate",
                "", "", "",
                "hgi hgi-stroke hgi-agreement-01",
                "/ar/SECURITYCYBER/Pages/Privacy-Policy.aspx",
                "/en/SECURITYCYBER/Pages/Privacy-Policy.aspx",
                "عرض السياسات", "View Policies",
                "_self", 1);

            AddCardRow(list, "policies",
                "اتفاقية مستوى الخدمة",
                "Service Level Agreement",
                "", "", "",
                "hgi hgi-stroke hgi-agreement-01",
                "https://pnu.edu.sa/ar/Pages/SLA.aspx",
                "https://pnu.edu.sa/en/Pages/SLA.aspx",
                "عرض الخدمات", "View Services",
                "_self", 2);

            AddCardRow(list, "policies",
                "سياسة الخصوصية",
                "Privacy Policy",
                "", "", "",
                "hgi hgi-stroke hgi-shield-01",
                "https://pnu.edu.sa/ar/SECURITYCYBER/Pages/DataPrivacyPolicy.aspx",
                "https://pnu.edu.sa/en/SECURITYCYBER/Pages/DataPrivacyPolicy.aspx",
                "عرض السياسة", "View Policy",
                "_blank", 3);

            AddCardRow(list, "policies",
                "سياسة حرية المعلومات",
                "Freedom of Information Policy",
                "", "", "",
                "hgi hgi-stroke hgi-file-02",
                "/ar/Departments/DM/Pages/FOIP.aspx",
                "/en/Departments/DM/Pages/FOIP.aspx",
                "عرض السياسة", "View Policy",
                "_blank", 4);

            AddCardRow(list, "policies",
                "سياسة مشاركة البيانات",
                "Data Sharing Policy",
                "", "", "",
                "hgi hgi-stroke hgi-database",
                "https://pnu.edu.sa/ar/Departments/DM/Pages/DSP.aspx",
                "https://pnu.edu.sa/en/Departments/DM/Pages/DSP.aspx",
                "عرض السياسة", "View Policy",
                "_blank", 5);

            AddCardRow(list, "policies",
                "سياسة البيانات المفتوحة",
                "Open Data Policy",
                "", "", "",
                "hgi hgi-stroke hgi-database",
                "https://pnu.edu.sa/ar/Departments/DT/DM/Pages/ODP.aspx",
                "https://pnu.edu.sa/en/Departments/DT/DM/Pages/ODP.aspx",
                "عرض السياسة", "View Policy",
                "_blank", 6);

            AddCardRow(list, "policies",
                "سياسة المشاركة الإلكترونية",
                "E-Participation Policy",
                "", "", "",
                "hgi hgi-stroke hgi-agreement-01",
                "https://eparticipation.my.gov.sa/",
                "https://eparticipation.my.gov.sa/",
                "عرض المنصة", "View Platform",
                "_blank", 7);

            AddCardRow(list, "policies",
                "التنمية المستدامة",
                "Sustainable Development",
                "", "", "",
                "hgi hgi-stroke hgi-file-02",
                "/ar/AboutUniversity/Pages/SustainableDevelopment.aspx",
                "/en/AboutUniversity/Pages/SustainableDevelopment.aspx",
                "عرض التفاصيل", "View Details",
                "_blank", 8);

            AddCardRow(list, "policies",
                "الدليل الإجرائي للسياسات الاتصالية",
                "Communication Policies Procedural Guide",
                "", "", "",
                "hgi hgi-stroke hgi-file-02",
                "/ar/AboutUniversity/Documents/%D8%A7%D9%84%D8%AF%D9%84%D9%8A%D9%84%20%D8%A7%D9%84%D8%A5%D8%AC%D8%B1%D8%A7%D8%A6%D9%8A%20%D9%84%D9%84%D8%B3%D9%8A%D8%A7%D8%B3%D8%A7%D8%AA%20%D8%A7%D9%84%D8%A7%D8%AA%D8%B5%D8%A7%D9%84%D9%8A%D8%A9.pdf",
                "/ar/AboutUniversity/Documents/%D8%A7%D9%84%D8%AF%D9%84%D9%8A%D9%84%20%D8%A7%D9%84%D8%A5%D8%AC%D8%B1%D8%A7%D8%A6%D9%8A%20%D9%84%D9%84%D8%B3%D9%8A%D8%A7%D8%B3%D8%A7%D8%AA%20%D8%A7%D9%84%D8%A7%D8%AA%D8%B5%D8%A7%D9%84%D9%8A%D8%A9.pdf",
                "عرض الدليل", "View Guide",
                "_blank", 9);

            // === 3. Regulations & Bylaws (الأنظمة واللوائح) ===
            AddCardRow(list, "regulations",
                "صفحة السياسات واللوائح",
                "Policies & Regulations Page",
                "", "", "",
                "hgi hgi-stroke hgi-file-02",
                "/ar/SECURITYCYBER/Pages/Privacy-Policy.aspx",
                "/en/SECURITYCYBER/Pages/Privacy-Policy.aspx",
                "عرض الصفحة", "View Page",
                "_self", 1);

            AddCardRow(list, "regulations",
                "الأنظمة واللوائح التعليمية",
                "Academic Regulations and Bylaws",
                "", "", "",
                "hgi hgi-stroke hgi-file-02",
                "https://pnu.edu.sa/ar/Agencies/AcademicAffairs/Pages/home.aspx",
                "https://pnu.edu.sa/en/Agencies/AcademicAffairs/Pages/home.aspx",
                "عرض الصفحة", "View Page",
                "_blank", 2);

            AddCardRow(list, "regulations",
                "قواعد السلوك والانضباط الطلابي",
                "Student Conduct and Discipline Rules",
                "", "", "",
                "hgi hgi-stroke hgi-file-02",
                "https://pnu.edu.sa/ar/Faculties/Ap/SiteAssets/Pages/Evidence-and-models/%D9%82%D9%88%D8%A7%D8%B9%D8%AF%20%D8%A7%D9%84%D8%B3%D9%84%D9%88%D9%83%20%D9%88%D8%A7%D9%84%D8%A7%D9%86%D8%B6%D8%A8%D8%A7%D8%B7%20%D8%A7%D9%84%D8%B7%D9%84%D8%A7%D8%A8%D9%8A.pdf",
                "https://pnu.edu.sa/ar/Faculties/Ap/SiteAssets/Pages/Evidence-and-models/%D9%82%D9%88%D8%A7%D8%B9%D8%AF%20%D8%A7%D9%84%D8%B3%D9%84%D9%88%D9%83%20%D9%88%D8%A7%D9%84%D8%A7%D9%86%D8%B6%D8%A8%D8%A7%D8%B7%20%D8%A7%D9%84%D8%B7%D9%84%D8%A7%D8%A8%D9%8A.pdf",
                "عرض الملف", "View Document",
                "_blank", 3);
        }

        private static void AddCardRow(SPList list, string sectionKey, string titleAr, string titleEn,
                                       string badgeAr, string badgeEn, string badgeClass,
                                       string icon, string urlAr, string urlEn,
                                       string buttonAr, string buttonEn, string target, double sortOrder)
        {
            SPListItem item = list.AddItem();
            item["Title"] = titleAr;
            item["TitleEn"] = titleEn;
            item["SectionKey"] = sectionKey;
            item["Badge"] = badgeAr;
            item["BadgeEn"] = badgeEn;
            item["BadgeClass"] = badgeClass;
            item["Icon"] = icon;
            item["Url"] = urlAr;
            item["UrlEn"] = urlEn;
            item["ButtonText"] = buttonAr;
            item["ButtonTextEn"] = buttonEn;
            item["Target"] = target;
            item["SortOrder"] = sortOrder;
            item["Visibility"] = true;
            item.Update();
        }

        private static void AddPlainNote(SPList list, string name, bool required)
        {
            string internalName = list.Fields.Add(name, SPFieldType.Note, required);
            var f = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(internalName);
            f.RichText = false;
            f.NumberOfLines = 2;
            f.Update();
        }

        private static void GrantAnonymousRead(SPList list)
        {
            if (list == null) return;
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
                Publics.WriteToLog("StrategyPoliciesProvisioner.GrantAnonymousRead", list.Title, ex.Message);
            }
        }

        #endregion

        #region Read Helpers

        public static List<StrategyPoliciesSection> ReadSectionsWithCards(SPWeb web, bool isArabic)
        {
            var sections = new List<StrategyPoliciesSection>();
            if (web == null) return sections;

            SPList secList = web.Lists.TryGetList(ListSections);
            SPList cardList = web.Lists.TryGetList(ListCards);

            if (secList == null || cardList == null) return sections;

            sections = ReadSections(secList, isArabic, onlyVisible: true);
            List<StrategyPoliciesCard> allCards = ReadCards(cardList, isArabic, onlyVisible: true);

            foreach (var sec in sections)
            {
                sec.Cards = allCards.FindAll(c => string.Equals(c.SectionKey, sec.SectionKey, StringComparison.OrdinalIgnoreCase));
            }

            return sections;
        }

        public static List<StrategyPoliciesSection> ReadSections(SPList list, bool isArabic, bool onlyVisible = false)
        {
            var result = new List<StrategyPoliciesSection>();
            if (list == null) return result;

            SPQuery query = new SPQuery
            {
                Query = onlyVisible
                    ? "<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/><FieldRef Name='ID' Ascending='TRUE'/></OrderBy>"
                    : "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/><FieldRef Name='ID' Ascending='TRUE'/></OrderBy>",
                RowLimit = 100
            };

            foreach (SPListItem row in list.GetItems(query))
            {
                result.Add(MapSectionItem(row, isArabic));
            }
            return result;
        }

        public static StrategyPoliciesSection MapSectionItem(SPListItem row, bool isArabic)
        {
            double order = 0;
            object rawOrder = row.Fields.ContainsField("ItemOrder") ? row["ItemOrder"] : null;
            if (rawOrder is double) order = (double)rawOrder;
            else if (!double.TryParse(Convert.ToString(rawOrder), NumberStyles.Any, CultureInfo.InvariantCulture, out order)) order = 0;

            bool visible = true;
            if (row.Fields.ContainsField("Visibility") && row["Visibility"] != null)
                visible = Convert.ToBoolean(row["Visibility"]);

            return new StrategyPoliciesSection
            {
                ID = row.ID,
                SectionKey = Field(row, "SectionKey"),
                Title = Field(row, "Title"),
                TitleEn = Field(row, "TitleEn"),
                Description = Field(row, "Description"),
                DescriptionEn = Field(row, "DescriptionEn"),
                ItemOrder = order,
                Visibility = visible,
                IsArabic = isArabic
            };
        }

        public static List<StrategyPoliciesCard> ReadCards(SPList list, bool isArabic, string sectionKey = null, bool onlyVisible = false)
        {
            var result = new List<StrategyPoliciesCard>();
            if (list == null) return result;

            string whereClause = "";
            if (onlyVisible && !string.IsNullOrEmpty(sectionKey))
            {
                whereClause = "<Where><And><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='SectionKey'/><Value Type='Text'>" + SecurityElement.Escape(sectionKey) + "</Value></Eq></And></Where>";
            }
            else if (onlyVisible)
            {
                whereClause = "<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>";
            }
            else if (!string.IsNullOrEmpty(sectionKey))
            {
                whereClause = "<Where><Eq><FieldRef Name='SectionKey'/><Value Type='Text'>" + SecurityElement.Escape(sectionKey) + "</Value></Eq></Where>";
            }

            SPQuery query = new SPQuery
            {
                Query = whereClause + "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/><FieldRef Name='ID' Ascending='TRUE'/></OrderBy>",
                RowLimit = 500
            };

            foreach (SPListItem row in list.GetItems(query))
            {
                result.Add(MapCardItem(row, isArabic));
            }
            return result;
        }

        public static StrategyPoliciesCard MapCardItem(SPListItem row, bool isArabic)
        {
            double order = 0;
            object rawOrder = row.Fields.ContainsField("SortOrder") ? row["SortOrder"] : null;
            if (rawOrder is double) order = (double)rawOrder;
            else if (!double.TryParse(Convert.ToString(rawOrder), NumberStyles.Any, CultureInfo.InvariantCulture, out order)) order = 0;

            bool visible = true;
            if (row.Fields.ContainsField("Visibility") && row["Visibility"] != null)
                visible = Convert.ToBoolean(row["Visibility"]);

            return new StrategyPoliciesCard
            {
                ID = row.ID,
                Title = Field(row, "Title"),
                TitleEn = Field(row, "TitleEn"),
                SectionKey = Field(row, "SectionKey"),
                Badge = Field(row, "Badge"),
                BadgeEn = Field(row, "BadgeEn"),
                BadgeClass = Field(row, "BadgeClass"),
                Icon = Field(row, "Icon"),
                Url = Field(row, "Url"),
                UrlEn = Field(row, "UrlEn"),
                ButtonText = Field(row, "ButtonText"),
                ButtonTextEn = Field(row, "ButtonTextEn"),
                Target = Field(row, "Target"),
                SortOrder = order,
                Visibility = visible,
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
