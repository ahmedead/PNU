using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure
{
    public class PgCategoryRepository
    {
        public const string TargetWebRelativePath = "ar/RegAdm/PGD";

        private SPWeb _web;

        public PgCategoryRepository(SPWeb web = null)
        {
            _web = web ?? (SPContext.Current != null ? SPContext.Current.Web : null);
        }

        public static SPWeb OpenTargetWeb(SPSite site)
        {
            if (site == null) return null;

            string siteRel = (site.ServerRelativeUrl ?? string.Empty).TrimEnd('/');
            string targetPathWithRoot = string.IsNullOrEmpty(siteRel) ? "/" + TargetWebRelativePath : siteRel + "/" + TargetWebRelativePath;

            string[] candidatePaths = new string[]
            {
                targetPathWithRoot,
                TargetWebRelativePath,
                "/" + TargetWebRelativePath,
                "ar/RegAdm/PGD",
                "/ar/RegAdm/PGD",
                "ar/regadm/pgd",
                "/ar/regadm/pgd"
            };

            foreach (string path in candidatePaths)
            {
                try
                {
                    SPWeb web = site.OpenWeb(path);
                    if (web != null && web.Exists)
                    {
                        return web;
                    }
                }
                catch { }
            }

            // Fallback if target subweb not found
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    return site.OpenWeb(SPContext.Current.Web.ID);
                }
            }
            catch { }

            try
            {
                return site.RootWeb;
            }
            catch
            {
                return null;
            }
        }

        private Guid GetSiteId()
        {
            try
            {
                if (_web != null && _web.Site != null) return _web.Site.ID;
                if (SPContext.Current != null && SPContext.Current.Site != null) return SPContext.Current.Site.ID;
                if (SPContext.Current != null && SPContext.Current.Web != null && SPContext.Current.Web.Site != null) return SPContext.Current.Web.Site.ID;
            }
            catch { }
            return Guid.Empty;
        }

        public bool IsArabic
        {
            get
            {
                try
                {
                    return _web != null ? _web.Language == 1025 : true;
                }
                catch { return true; }
            }
        }

        #region Overview Operations

        public PgCategoryOverviewModel GetOverview(PgProgramCategory category)
        {
            string catKey = category.ToString();
            Guid siteId = GetSiteId();
            PgCategoryOverviewModel found = null;

            if (siteId != Guid.Empty)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb elevatedWeb = OpenTargetWeb(site))
                        {
                            if (elevatedWeb == null) return;
                            SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryOverview);
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = string.Format("<Where><Eq><FieldRef Name='CategoryKey'/><Value Type='Text'>{0}</Value></Eq></Where>", catKey);
                                query.RowLimit = 1;

                                SPListItemCollection items = list.GetItems(query);
                                if (items != null && items.Count > 0)
                                {
                                    found = MapOverview(items[0]);
                                }
                            }
                        }
                    });
                    if (found != null) return found;
                }
                catch { }
            }

            // Fallback to in-memory seed data
            var seedOverviews = PgCategorySeedData.GetOverviews();
            var matched = seedOverviews.FirstOrDefault(o => o.CategoryKey.Equals(catKey, StringComparison.OrdinalIgnoreCase));
            return matched ?? seedOverviews.FirstOrDefault();
        }

        public bool SaveOverview(PgCategoryOverviewModel model)
        {
            if (model == null) return false;
            Guid siteId = GetSiteId();
            if (siteId == Guid.Empty) return false;

            bool success = false;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb elevatedWeb = OpenTargetWeb(site))
                {
                    if (elevatedWeb == null) return;
                    bool orig = elevatedWeb.AllowUnsafeUpdates;
                    try
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;
                        SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryOverview);
                        if (list == null)
                        {
                            PgCategoryProvisioner.EnsureLists(elevatedWeb);
                            list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryOverview);
                        }

                        if (list != null)
                        {
                            SPListItem item = null;
                            if (model.Id > 0)
                            {
                                try { item = list.GetItemById(model.Id); } catch { }
                            }

                            if (item == null)
                            {
                                SPQuery q = new SPQuery();
                                q.Query = string.Format("<Where><Eq><FieldRef Name='CategoryKey'/><Value Type='Text'>{0}</Value></Eq></Where>", model.CategoryKey);
                                q.RowLimit = 1;
                                var items = list.GetItems(q);
                                if (items.Count > 0) item = items[0];
                                else item = list.AddItem();
                            }

                            item["Title"] = model.CategoryKey;
                            item["CategoryKey"] = model.CategoryKey;
                            item["HeroTagAr"] = model.HeroTagAr ?? string.Empty;
                            item["HeroTagEn"] = model.HeroTagEn ?? string.Empty;
                            item["HeroSubTagAr"] = model.HeroSubTagAr ?? string.Empty;
                            item["HeroSubTagEn"] = model.HeroSubTagEn ?? string.Empty;
                            item["HeroTitleAr"] = model.HeroTitleAr ?? string.Empty;
                            item["HeroTitleEn"] = model.HeroTitleEn ?? string.Empty;
                            item["HeroDescriptionAr"] = model.HeroDescriptionAr ?? string.Empty;
                            item["HeroDescriptionEn"] = model.HeroDescriptionEn ?? string.Empty;

                            item["BannerTitleAr"] = model.BannerTitleAr ?? string.Empty;
                            item["BannerTitleEn"] = model.BannerTitleEn ?? string.Empty;
                            item["BannerDescriptionAr"] = model.BannerDescriptionAr ?? string.Empty;
                            item["BannerDescriptionEn"] = model.BannerDescriptionEn ?? string.Empty;
                            item["BannerButtonTextAr"] = model.BannerButtonTextAr ?? string.Empty;
                            item["BannerButtonTextEn"] = model.BannerButtonTextEn ?? string.Empty;
                            item["BannerUrl"] = model.BannerUrl ?? string.Empty;

                            item["Card1TitleAr"] = model.Card1TitleAr ?? string.Empty;
                            item["Card1TitleEn"] = model.Card1TitleEn ?? string.Empty;
                            item["Card1DescAr"] = model.Card1DescAr ?? string.Empty;
                            item["Card1DescEn"] = model.Card1DescEn ?? string.Empty;
                            item["Card1Url"] = model.Card1Url ?? string.Empty;

                            item["Card2TitleAr"] = model.Card2TitleAr ?? string.Empty;
                            item["Card2TitleEn"] = model.Card2TitleEn ?? string.Empty;
                            item["Card2DescAr"] = model.Card2DescAr ?? string.Empty;
                            item["Card2DescEn"] = model.Card2DescEn ?? string.Empty;
                            item["Card2Url"] = model.Card2Url ?? string.Empty;

                            item["IsActive"] = model.IsActive;
                            item.Update();
                            success = true;
                        }
                    }
                    catch
                    {
                        success = false;
                    }
                    finally
                    {
                        elevatedWeb.AllowUnsafeUpdates = orig;
                    }
                }
            });

            return success;
        }

        #endregion

        #region Programs Operations

        public List<PgProgramItemModel> GetPrograms(PgProgramCategory category, bool activeOnly = true)
        {
            string catKey = category == PgProgramCategory.All ? "All" : category.ToString();
            List<PgProgramItemModel> list = new List<PgProgramItemModel>();
            Guid siteId = GetSiteId();

            if (siteId != Guid.Empty)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb elevatedWeb = OpenTargetWeb(site))
                        {
                            if (elevatedWeb == null) return;
                            SPList spList = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryPrograms);
                            if (spList != null)
                            {
                                SPQuery query = new SPQuery();
                                string where = "";

                                if (category != PgProgramCategory.All)
                                {
                                    where = string.Format("<Eq><FieldRef Name='CategoryKey'/><Value Type='Text'>{0}</Value></Eq>", catKey);
                                }

                                if (activeOnly)
                                {
                                    string activeFilter = "<Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq>";
                                    where = string.IsNullOrEmpty(where) ? activeFilter : string.Format("<And>{0}{1}</And>", where, activeFilter);
                                }

                                query.Query = string.Format("<Where>{0}</Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True'/><FieldRef Name='Title' Ascending='True'/></OrderBy>",
                                    string.IsNullOrEmpty(where) ? "<IsNotNull><FieldRef Name='Title'/></IsNotNull>" : where);

                                SPListItemCollection items = spList.GetItems(query);
                                foreach (SPListItem it in items)
                                {
                                    list.Add(MapProgram(it));
                                }
                            }
                        }
                    });

                    if (list.Count > 0) return list;
                }
                catch { }
            }

            // Fallback
            var seedPrograms = PgCategorySeedData.GetPrograms();
            if (category == PgProgramCategory.All) return seedPrograms;

            return seedPrograms
                .Where(p => p.CategoryKey.Equals(catKey, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.ItemOrder)
                .ToList();
        }

        public List<PgDegreeGroupViewModel> GetGroupedPrograms(PgProgramCategory category, bool activeOnly = true)
        {
            var programs = GetPrograms(category, activeOnly);
            var grouped = new List<PgDegreeGroupViewModel>();

            var distinctGroups = programs
                .Select(p => new { Key = p.DegreeGroupKey, TitleAr = p.DegreeGroupTitleAr, TitleEn = p.DegreeGroupTitleEn })
                .Distinct()
                .ToList();

            foreach (var g in distinctGroups)
            {
                var groupPrograms = programs.Where(p => p.DegreeGroupKey == g.Key).ToList();
                string icon = "hgi-briefcase-02";
                if (g.Key.StartsWith("PhD", StringComparison.OrdinalIgnoreCase)) icon = "hgi-award-02";
                else if (g.Key.StartsWith("HigherDiploma", StringComparison.OrdinalIgnoreCase)) icon = "hgi-diploma";
                else if (g.Key.StartsWith("AcademicMaster", StringComparison.OrdinalIgnoreCase)) icon = "hgi-mortarboard-02";

                grouped.Add(new PgDegreeGroupViewModel
                {
                    GroupKey = g.Key,
                    GroupTitle = IsArabic ? g.TitleAr : (string.IsNullOrEmpty(g.TitleEn) ? g.TitleAr : g.TitleEn),
                    IconClass = icon,
                    Programs = groupPrograms
                });
            }

            return grouped;
        }

        public PgProgramItemModel GetProgramById(int id)
        {
            if (id <= 0) return null;
            Guid siteId = GetSiteId();
            if (siteId == Guid.Empty) return null;

            PgProgramItemModel result = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb elevatedWeb = OpenTargetWeb(site))
                    {
                        if (elevatedWeb == null) return;
                        SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryPrograms);
                        if (list != null)
                        {
                            SPListItem it = list.GetItemById(id);
                            if (it != null) result = MapProgram(it);
                        }
                    }
                });
            }
            catch { }

            return result;
        }

        public bool SaveProgram(PgProgramItemModel model)
        {
            if (model == null) return false;
            Guid siteId = GetSiteId();
            if (siteId == Guid.Empty) return false;

            bool success = false;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb elevatedWeb = OpenTargetWeb(site))
                {
                    if (elevatedWeb == null) return;
                    bool orig = elevatedWeb.AllowUnsafeUpdates;
                    try
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;
                        SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryPrograms);
                        if (list == null)
                        {
                            PgCategoryProvisioner.EnsureLists(elevatedWeb);
                            list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryPrograms);
                        }

                        if (list != null)
                        {
                            SPListItem item = null;
                            if (model.Id > 0)
                            {
                                try { item = list.GetItemById(model.Id); } catch { }
                            }
                            if (item == null) item = list.AddItem();

                            item["Title"] = model.TitleAr ?? string.Empty;
                            item["TitleEn"] = model.TitleEn ?? string.Empty;
                            item["CategoryKey"] = model.CategoryKey ?? string.Empty;
                            item["DegreeGroupKey"] = model.DegreeGroupKey ?? string.Empty;
                            item["DegreeGroupTitleAr"] = model.DegreeGroupTitleAr ?? string.Empty;
                            item["DegreeGroupTitleEn"] = model.DegreeGroupTitleEn ?? string.Empty;
                            item["CollegeAr"] = model.CollegeAr ?? string.Empty;
                            item["CollegeEn"] = model.CollegeEn ?? string.Empty;
                            item["DepartmentAr"] = model.DepartmentAr ?? string.Empty;
                            item["DepartmentEn"] = model.DepartmentEn ?? string.Empty;
                            item["ScopeAr"] = model.ScopeAr ?? "محلي";
                            item["ScopeEn"] = model.ScopeEn ?? "Local";
                            item["DetailsUrl"] = model.DetailsUrl ?? string.Empty;
                            item["ApplyUrl"] = model.ApplyUrl ?? string.Empty;
                            item["ItemOrder"] = model.ItemOrder;
                            item["IsActive"] = model.IsActive;

                            item.Update();
                            success = true;
                        }
                    }
                    catch
                    {
                        success = false;
                    }
                    finally
                    {
                        elevatedWeb.AllowUnsafeUpdates = orig;
                    }
                }
            });

            return success;
        }

        public bool DeleteProgram(int id)
        {
            if (id <= 0) return false;
            Guid siteId = GetSiteId();
            if (siteId == Guid.Empty) return false;

            bool success = false;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb elevatedWeb = OpenTargetWeb(site))
                {
                    if (elevatedWeb == null) return;
                    bool orig = elevatedWeb.AllowUnsafeUpdates;
                    try
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;
                        SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListCategoryPrograms);
                        if (list != null)
                        {
                            SPListItem item = list.GetItemById(id);
                            if (item != null)
                            {
                                item.Delete();
                                success = true;
                            }
                        }
                    }
                    catch
                    {
                        success = false;
                    }
                    finally
                    {
                        elevatedWeb.AllowUnsafeUpdates = orig;
                    }
                }
            });

            return success;
        }

        #endregion

        #region Program Details Operations

        public PgProgramDetailsModel GetProgramDetails(string programCode)
        {
            if (string.IsNullOrEmpty(programCode)) return null;
            string cleanCode = PgProgramDetailsSeedData.ExtractCodeFromUrl(programCode);
            if (string.IsNullOrEmpty(cleanCode)) cleanCode = programCode.Trim();

            Guid siteId = GetSiteId();
            if (siteId != Guid.Empty)
            {
                try
                {
                    PgProgramDetailsModel found = null;

                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb elevatedWeb = OpenTargetWeb(site))
                        {
                            if (elevatedWeb == null) return;
                            SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListProgramDetails);
                            if (list != null)
                            {
                                // 0. If cleanCode is an integer ID
                                int idVal;
                                if (int.TryParse(cleanCode, out idVal) && idVal > 0)
                                {
                                    try
                                    {
                                        SPListItem itById = list.GetItemById(idVal);
                                        if (itById != null)
                                        {
                                            found = MapProgramDetails(itById);
                                            return;
                                        }
                                    }
                                    catch { }
                                }

                                // 1. Try matching by exact ProgramCode or ProgramCode with extension
                                SPQuery query = new SPQuery();
                                query.Query = string.Format("<Where><Or><Eq><FieldRef Name='ProgramCode'/><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='ProgramCode'/><Value Type='Text'>{0}.aspx</Value></Eq></Or></Where>", cleanCode);
                                query.RowLimit = 1;

                                SPListItemCollection items = list.GetItems(query);
                                if (items != null && items.Count > 0)
                                {
                                    found = MapProgramDetails(items[0]);
                                    return;
                                }

                                // 2. Try matching by Title (e.g. Arabic Name or English Name)
                                SPQuery titleQuery = new SPQuery();
                                titleQuery.Query = string.Format("<Where><Or><Eq><FieldRef Name='Title'/><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='TitleEn'/><Value Type='Text'>{0}</Value></Eq></Or></Where>", programCode.Trim());
                                titleQuery.RowLimit = 1;
                                var titleItems = list.GetItems(titleQuery);
                                if (titleItems != null && titleItems.Count > 0)
                                {
                                    found = MapProgramDetails(titleItems[0]);
                                    return;
                                }

                                // 3. Case-insensitive scan in list items
                                foreach (SPListItem it in list.Items)
                                {
                                    string codeVal = SafeString(it, "ProgramCode");
                                    string titleVal = SafeString(it, "Title");
                                    string titleEnVal = SafeString(it, "TitleEn");
                                    if (codeVal.Equals(cleanCode, StringComparison.OrdinalIgnoreCase) ||
                                        PgProgramDetailsSeedData.ExtractCodeFromUrl(codeVal).Equals(cleanCode, StringComparison.OrdinalIgnoreCase) ||
                                        titleVal.Equals(programCode.Trim(), StringComparison.OrdinalIgnoreCase) ||
                                        titleEnVal.Equals(programCode.Trim(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        found = MapProgramDetails(it);
                                        return;
                                    }
                                }

                                // 4. If list has items, return the first item from the list instead of hardcoded seed
                                if (list.ItemCount > 0)
                                {
                                    // Prefer matching 'pro55' item if available
                                    foreach (SPListItem it in list.Items)
                                    {
                                        string codeVal = SafeString(it, "ProgramCode");
                                        if (codeVal.Equals("pro55", StringComparison.OrdinalIgnoreCase))
                                        {
                                            found = MapProgramDetails(it);
                                            return;
                                        }
                                    }
                                    found = MapProgramDetails(list.Items[0]);
                                    return;
                                }
                            }
                        }
                    });

                    if (found != null) return found;
                }
                catch { }
            }

            // Fallback to in-memory seed details
            var seedDetails = PgProgramDetailsSeedData.GetSeedDetails();
            var matched = seedDetails.FirstOrDefault(d => d.ProgramCode.Equals(cleanCode, StringComparison.OrdinalIgnoreCase) ||
                                                          PgProgramDetailsSeedData.ExtractCodeFromUrl(d.ProgramCode).Equals(cleanCode, StringComparison.OrdinalIgnoreCase));
            if (matched != null) return matched;

            return seedDetails.FirstOrDefault();
        }

        public PgProgramDetailsModel GetProgramDetailsById(int id)
        {
            if (id <= 0) return null;
            Guid siteId = GetSiteId();
            if (siteId == Guid.Empty) return null;

            PgProgramDetailsModel result = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb elevatedWeb = OpenTargetWeb(site))
                    {
                        if (elevatedWeb == null) return;
                        SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListProgramDetails);
                        if (list != null)
                        {
                            SPListItem it = list.GetItemById(id);
                            if (it != null) result = MapProgramDetails(it);
                        }
                    }
                });
            }
            catch { }

            return result;
        }

        public bool SaveProgramDetails(PgProgramDetailsModel model)
        {
            if (model == null) return false;
            string cleanCode = PgProgramDetailsSeedData.ExtractCodeFromUrl(model.ProgramCode);
            if (string.IsNullOrEmpty(cleanCode)) cleanCode = (model.ProgramCode ?? string.Empty).Trim();
            model.ProgramCode = cleanCode;

            Guid siteId = GetSiteId();
            if (siteId == Guid.Empty) return false;

            bool success = false;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb elevatedWeb = OpenTargetWeb(site))
                {
                    if (elevatedWeb == null) return;
                    bool orig = elevatedWeb.AllowUnsafeUpdates;
                    try
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;
                        SPList list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListProgramDetails);
                        if (list == null)
                        {
                            PgCategoryProvisioner.EnsureLists(elevatedWeb);
                            list = elevatedWeb.Lists.TryGetList(PgCategoryConstants.ListProgramDetails);
                        }

                        if (list != null)
                        {
                            SPListItem item = null;
                            if (model.Id > 0)
                            {
                                try { item = list.GetItemById(model.Id); } catch { }
                            }

                            if (item == null)
                            {
                                // Search by ProgramCode or Title
                                SPQuery q = new SPQuery();
                                q.Query = string.Format("<Where><Or><Eq><FieldRef Name='ProgramCode'/><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='ProgramCode'/><Value Type='Text'>{0}.aspx</Value></Eq></Or></Where>", cleanCode);
                                q.RowLimit = 1;
                                var items = list.GetItems(q);
                                if (items.Count > 0)
                                {
                                    item = items[0];
                                }
                                else
                                {
                                    // Also check items scan
                                    foreach (SPListItem it in list.Items)
                                    {
                                        string codeVal = SafeString(it, "ProgramCode");
                                        if (codeVal.Equals(cleanCode, StringComparison.OrdinalIgnoreCase) ||
                                            PgProgramDetailsSeedData.ExtractCodeFromUrl(codeVal).Equals(cleanCode, StringComparison.OrdinalIgnoreCase))
                                        {
                                            item = it;
                                            break;
                                        }
                                    }

                                    if (item == null) item = list.AddItem();
                                }
                            }

                            item["Title"] = model.TitleAr ?? string.Empty;
                            item["TitleEn"] = model.TitleEn ?? string.Empty;
                            item["ProgramCode"] = cleanCode;
                            item["CategoryKey"] = model.CategoryKey ?? string.Empty;
                            item["DegreeAr"] = model.DegreeAr ?? string.Empty;
                            item["DegreeEn"] = model.DegreeEn ?? string.Empty;
                            item["CollegeAr"] = model.CollegeAr ?? string.Empty;
                            item["CollegeEn"] = model.CollegeEn ?? string.Empty;
                            item["DepartmentAr"] = model.DepartmentAr ?? string.Empty;
                            item["DepartmentEn"] = model.DepartmentEn ?? string.Empty;
                            item["ScopeAr"] = model.ScopeAr ?? "محلي";
                            item["ScopeEn"] = model.ScopeEn ?? "Local";

                            item["DurationAr"] = model.DurationAr ?? string.Empty;
                            item["DurationEn"] = model.DurationEn ?? string.Empty;
                            item["CreditHoursAr"] = model.CreditHoursAr ?? string.Empty;
                            item["CreditHoursEn"] = model.CreditHoursEn ?? string.Empty;
                            item["LanguageAr"] = model.LanguageAr ?? string.Empty;
                            item["LanguageEn"] = model.LanguageEn ?? string.Empty;
                            item["StudySystemAr"] = model.StudySystemAr ?? string.Empty;
                            item["StudySystemEn"] = model.StudySystemEn ?? string.Empty;
                            item["TuitionFeesAr"] = model.TuitionFeesAr ?? string.Empty;
                            item["TuitionFeesEn"] = model.TuitionFeesEn ?? string.Empty;

                            item["OverviewAr"] = model.OverviewAr ?? string.Empty;
                            item["OverviewEn"] = model.OverviewEn ?? string.Empty;
                            item["MissionAr"] = model.MissionAr ?? string.Empty;
                            item["MissionEn"] = model.MissionEn ?? string.Empty;
                            item["ObjectivesAr"] = model.ObjectivesAr ?? string.Empty;
                            item["ObjectivesEn"] = model.ObjectivesEn ?? string.Empty;
                            item["AdmissionRequirementsAr"] = model.AdmissionRequirementsAr ?? string.Empty;
                            item["AdmissionRequirementsEn"] = model.AdmissionRequirementsEn ?? string.Empty;
                            item["StudyPlanAr"] = model.StudyPlanAr ?? string.Empty;
                            item["StudyPlanEn"] = model.StudyPlanEn ?? string.Empty;
                            item["StudyPlanPdfUrl"] = model.StudyPlanPdfUrl ?? string.Empty;
                            item["CareerOpportunitiesAr"] = model.CareerOpportunitiesAr ?? string.Empty;
                            item["CareerOpportunitiesEn"] = model.CareerOpportunitiesEn ?? string.Empty;

                            item["ApplyUrl"] = model.ApplyUrl ?? string.Empty;
                            item["PartnerUniversityAr"] = model.PartnerUniversityAr ?? string.Empty;
                            item["PartnerUniversityEn"] = model.PartnerUniversityEn ?? string.Empty;
                            item["PartnerUniversityUrl"] = model.PartnerUniversityUrl ?? string.Empty;
                            item["ContactEmail"] = model.ContactEmail ?? string.Empty;
                            item["IsActive"] = model.IsActive;

                            item.Update();
                            success = true;
                        }
                    }
                    catch
                    {
                        success = false;
                    }
                    finally
                    {
                        elevatedWeb.AllowUnsafeUpdates = orig;
                    }
                }
            });

            return success;
        }

        #endregion

        #region Mappers

        private PgCategoryOverviewModel MapOverview(SPListItem it)
        {
            return new PgCategoryOverviewModel
            {
                Id = it.ID,
                CategoryKey = SafeString(it, "CategoryKey"),
                HeroTagAr = SafeString(it, "HeroTagAr"),
                HeroTagEn = SafeString(it, "HeroTagEn"),
                HeroSubTagAr = SafeString(it, "HeroSubTagAr"),
                HeroSubTagEn = SafeString(it, "HeroSubTagEn"),
                HeroTitleAr = SafeString(it, "HeroTitleAr"),
                HeroTitleEn = SafeString(it, "HeroTitleEn"),
                HeroDescriptionAr = SafeString(it, "HeroDescriptionAr"),
                HeroDescriptionEn = SafeString(it, "HeroDescriptionEn"),

                BannerTitleAr = SafeString(it, "BannerTitleAr"),
                BannerTitleEn = SafeString(it, "BannerTitleEn"),
                BannerDescriptionAr = SafeString(it, "BannerDescriptionAr"),
                BannerDescriptionEn = SafeString(it, "BannerDescriptionEn"),
                BannerButtonTextAr = SafeString(it, "BannerButtonTextAr"),
                BannerButtonTextEn = SafeString(it, "BannerButtonTextEn"),
                BannerUrl = SafeString(it, "BannerUrl"),

                Card1TitleAr = SafeString(it, "Card1TitleAr"),
                Card1TitleEn = SafeString(it, "Card1TitleEn"),
                Card1DescAr = SafeString(it, "Card1DescAr"),
                Card1DescEn = SafeString(it, "Card1DescEn"),
                Card1Url = SafeString(it, "Card1Url"),

                Card2TitleAr = SafeString(it, "Card2TitleAr"),
                Card2TitleEn = SafeString(it, "Card2TitleEn"),
                Card2DescAr = SafeString(it, "Card2DescAr"),
                Card2DescEn = SafeString(it, "Card2DescEn"),
                Card2Url = SafeString(it, "Card2Url"),

                IsActive = SafeBool(it, "IsActive", true)
            };
        }

        private PgProgramItemModel MapProgram(SPListItem it)
        {
            return new PgProgramItemModel
            {
                Id = it.ID,
                TitleAr = SafeString(it, "Title"),
                TitleEn = SafeString(it, "TitleEn"),
                CategoryKey = SafeString(it, "CategoryKey"),
                DegreeGroupKey = SafeString(it, "DegreeGroupKey"),
                DegreeGroupTitleAr = SafeString(it, "DegreeGroupTitleAr"),
                DegreeGroupTitleEn = SafeString(it, "DegreeGroupTitleEn"),
                CollegeAr = SafeString(it, "CollegeAr"),
                CollegeEn = SafeString(it, "CollegeEn"),
                DepartmentAr = SafeString(it, "DepartmentAr"),
                DepartmentEn = SafeString(it, "DepartmentEn"),
                ScopeAr = SafeString(it, "ScopeAr"),
                ScopeEn = SafeString(it, "ScopeEn"),
                DetailsUrl = SafeString(it, "DetailsUrl"),
                ApplyUrl = SafeString(it, "ApplyUrl"),
                ItemOrder = SafeInt(it, "ItemOrder"),
                IsActive = SafeBool(it, "IsActive", true)
            };
        }

        private PgProgramDetailsModel MapProgramDetails(SPListItem it)
        {
            return new PgProgramDetailsModel
            {
                Id = it.ID,
                TitleAr = SafeString(it, "Title"),
                TitleEn = SafeString(it, "TitleEn"),
                ProgramCode = SafeString(it, "ProgramCode"),
                CategoryKey = SafeString(it, "CategoryKey"),
                DegreeAr = SafeString(it, "DegreeAr"),
                DegreeEn = SafeString(it, "DegreeEn"),
                CollegeAr = SafeString(it, "CollegeAr"),
                CollegeEn = SafeString(it, "CollegeEn"),
                DepartmentAr = SafeString(it, "DepartmentAr"),
                DepartmentEn = SafeString(it, "DepartmentEn"),
                ScopeAr = SafeString(it, "ScopeAr"),
                ScopeEn = SafeString(it, "ScopeEn"),

                DurationAr = SafeString(it, "DurationAr"),
                DurationEn = SafeString(it, "DurationEn"),
                CreditHoursAr = SafeString(it, "CreditHoursAr"),
                CreditHoursEn = SafeString(it, "CreditHoursEn"),
                LanguageAr = SafeString(it, "LanguageAr"),
                LanguageEn = SafeString(it, "LanguageEn"),
                StudySystemAr = SafeString(it, "StudySystemAr"),
                StudySystemEn = SafeString(it, "StudySystemEn"),
                TuitionFeesAr = SafeString(it, "TuitionFeesAr"),
                TuitionFeesEn = SafeString(it, "TuitionFeesEn"),

                OverviewAr = SafeString(it, "OverviewAr"),
                OverviewEn = SafeString(it, "OverviewEn"),
                MissionAr = SafeString(it, "MissionAr"),
                MissionEn = SafeString(it, "MissionEn"),
                ObjectivesAr = SafeString(it, "ObjectivesAr"),
                ObjectivesEn = SafeString(it, "ObjectivesEn"),
                AdmissionRequirementsAr = SafeString(it, "AdmissionRequirementsAr"),
                AdmissionRequirementsEn = SafeString(it, "AdmissionRequirementsEn"),
                StudyPlanAr = SafeString(it, "StudyPlanAr"),
                StudyPlanEn = SafeString(it, "StudyPlanEn"),
                StudyPlanPdfUrl = SafeString(it, "StudyPlanPdfUrl"),
                CareerOpportunitiesAr = SafeString(it, "CareerOpportunitiesAr"),
                CareerOpportunitiesEn = SafeString(it, "CareerOpportunitiesEn"),

                ApplyUrl = SafeString(it, "ApplyUrl"),
                PartnerUniversityAr = SafeString(it, "PartnerUniversityAr"),
                PartnerUniversityEn = SafeString(it, "PartnerUniversityEn"),
                PartnerUniversityUrl = SafeString(it, "PartnerUniversityUrl"),
                ContactEmail = SafeString(it, "ContactEmail"),
                IsActive = SafeBool(it, "IsActive", true)
            };
        }

        private static string SafeString(SPListItem it, string field)
        {
            try
            {
                if (it != null && it.Fields.ContainsField(field) && it[field] != null)
                    return it[field].ToString();
            }
            catch { }
            return string.Empty;
        }

        private static int SafeInt(SPListItem it, string field)
        {
            try
            {
                if (it != null && it.Fields.ContainsField(field) && it[field] != null)
                {
                    int val;
                    if (int.TryParse(it[field].ToString(), out val)) return val;
                }
            }
            catch { }
            return 0;
        }

        private static bool SafeBool(SPListItem it, string field, bool defaultValue = false)
        {
            try
            {
                if (it != null && it.Fields.ContainsField(field) && it[field] != null)
                {
                    string s = it[field].ToString().Trim();
                    if (s == "1" || s.Equals("Yes", StringComparison.OrdinalIgnoreCase) || s.Equals("True", StringComparison.OrdinalIgnoreCase) || s == "نعم")
                        return true;
                    if (s == "0" || s.Equals("No", StringComparison.OrdinalIgnoreCase) || s.Equals("False", StringComparison.OrdinalIgnoreCase) || s == "لا")
                        return false;
                }
            }
            catch { }
            return defaultValue;
        }

        #endregion
    }
}
