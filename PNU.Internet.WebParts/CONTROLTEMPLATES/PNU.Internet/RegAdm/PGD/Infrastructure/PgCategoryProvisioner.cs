using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure
{
    public static class PgCategoryProvisioner
    {
        public static void EnsureLists(SPWeb web = null)
        {
            Guid siteId = Guid.Empty;
            try
            {
                if (web != null && web.Site != null) siteId = web.Site.ID;
                else if (SPContext.Current != null && SPContext.Current.Site != null) siteId = SPContext.Current.Site.ID;
                else if (SPContext.Current != null && SPContext.Current.Web != null && SPContext.Current.Web.Site != null) siteId = SPContext.Current.Web.Site.ID;
            }
            catch { }

            if (siteId == Guid.Empty) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb elevatedWeb = PgCategoryRepository.OpenTargetWeb(site))
                {
                    if (elevatedWeb == null) return;
                    bool originalAllowUnsafe = elevatedWeb.AllowUnsafeUpdates;
                    try
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;
                        EnsureOverviewList(elevatedWeb);
                        EnsureProgramsList(elevatedWeb);
                        EnsureProgramDetailsList(elevatedWeb);
                    }
                    finally
                    {
                        elevatedWeb.AllowUnsafeUpdates = originalAllowUnsafe;
                    }
                }
            });
        }

        public static void EnsureListsOnCurrentWeb()
        {
            EnsureLists(SPContext.Current != null ? SPContext.Current.Web : null);
        }

        #region Overview List Provisioning

        private static SPList EnsureOverviewList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(PgCategoryConstants.ListCategoryOverview);
            if (list == null)
            {
                Guid id = web.Lists.Add(PgCategoryConstants.ListCategoryOverview, "Postgraduate Program Category Overview and Headers", SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
            }

            AddTextField(list, "CategoryKey");
            AddTextField(list, "HeroTagAr");
            AddTextField(list, "HeroTagEn");
            AddTextField(list, "HeroSubTagAr");
            AddTextField(list, "HeroSubTagEn");
            AddTextField(list, "HeroTitleAr");
            AddTextField(list, "HeroTitleEn");
            AddNoteField(list, "HeroDescriptionAr");
            AddNoteField(list, "HeroDescriptionEn");

            AddTextField(list, "BannerTitleAr");
            AddTextField(list, "BannerTitleEn");
            AddNoteField(list, "BannerDescriptionAr");
            AddNoteField(list, "BannerDescriptionEn");
            AddTextField(list, "BannerButtonTextAr");
            AddTextField(list, "BannerButtonTextEn");
            AddTextField(list, "BannerUrl");

            AddTextField(list, "Card1TitleAr");
            AddTextField(list, "Card1TitleEn");
            AddTextField(list, "Card1DescAr");
            AddTextField(list, "Card1DescEn");
            AddTextField(list, "Card1Url");

            AddTextField(list, "Card2TitleAr");
            AddTextField(list, "Card2TitleEn");
            AddTextField(list, "Card2DescAr");
            AddTextField(list, "Card2DescEn");
            AddTextField(list, "Card2Url");

            AddBoolField(list, "IsActive");

            if (list.ItemCount == 0)
            {
                SeedOverviews(list, false);
            }

            return list;
        }

        public static void SeedOverviews(SPList list, bool clearExisting)
        {
            if (list == null) return;

            if (clearExisting && list.ItemCount > 0)
            {
                var items = list.Items;
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    items[i].Delete();
                }
            }

            var overviews = PgCategorySeedData.GetOverviews();
            foreach (var ov in overviews)
            {
                // check if already exists
                if (!clearExisting)
                {
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><Eq><FieldRef Name='CategoryKey'/><Value Type='Text'>{0}</Value></Eq></Where>", ov.CategoryKey);
                    if (list.GetItems(query).Count > 0) continue;
                }

                SPListItem item = list.AddItem();
                item["Title"] = ov.CategoryKey;
                item["CategoryKey"] = ov.CategoryKey;
                item["HeroTagAr"] = ov.HeroTagAr;
                item["HeroTagEn"] = ov.HeroTagEn;
                item["HeroSubTagAr"] = ov.HeroSubTagAr;
                item["HeroSubTagEn"] = ov.HeroSubTagEn;
                item["HeroTitleAr"] = ov.HeroTitleAr;
                item["HeroTitleEn"] = ov.HeroTitleEn;
                item["HeroDescriptionAr"] = ov.HeroDescriptionAr;
                item["HeroDescriptionEn"] = ov.HeroDescriptionEn;

                item["BannerTitleAr"] = ov.BannerTitleAr;
                item["BannerTitleEn"] = ov.BannerTitleEn;
                item["BannerDescriptionAr"] = ov.BannerDescriptionAr;
                item["BannerDescriptionEn"] = ov.BannerDescriptionEn;
                item["BannerButtonTextAr"] = ov.BannerButtonTextAr;
                item["BannerButtonTextEn"] = ov.BannerButtonTextEn;
                item["BannerUrl"] = ov.BannerUrl;

                item["Card1TitleAr"] = ov.Card1TitleAr;
                item["Card1TitleEn"] = ov.Card1TitleEn;
                item["Card1DescAr"] = ov.Card1DescAr;
                item["Card1DescEn"] = ov.Card1DescEn;
                item["Card1Url"] = ov.Card1Url;

                item["Card2TitleAr"] = ov.Card2TitleAr;
                item["Card2TitleEn"] = ov.Card2TitleEn;
                item["Card2DescAr"] = ov.Card2DescAr;
                item["Card2DescEn"] = ov.Card2DescEn;
                item["Card2Url"] = ov.Card2Url;

                item["IsActive"] = ov.IsActive;
                item.Update();
            }
        }

        #endregion

        #region Programs List Provisioning

        private static SPList EnsureProgramsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(PgCategoryConstants.ListCategoryPrograms);
            if (list == null)
            {
                Guid id = web.Lists.Add(PgCategoryConstants.ListCategoryPrograms, "Postgraduate Category Programs Directory", SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
            }

            AddTextField(list, "TitleEn");
            AddTextField(list, "CategoryKey");
            AddTextField(list, "DegreeGroupKey");
            AddTextField(list, "DegreeGroupTitleAr");
            AddTextField(list, "DegreeGroupTitleEn");
            AddTextField(list, "CollegeAr");
            AddTextField(list, "CollegeEn");
            AddTextField(list, "DepartmentAr");
            AddTextField(list, "DepartmentEn");
            AddTextField(list, "ScopeAr");
            AddTextField(list, "ScopeEn");
            AddTextField(list, "DetailsUrl");
            AddTextField(list, "ApplyUrl");
            AddNumberField(list, "ItemOrder");
            AddBoolField(list, "IsActive");

            if (list.ItemCount == 0)
            {
                SeedPrograms(list, false, null);
            }

            return list;
        }

        public static void SeedPrograms(SPList list, bool clearExisting, string categoryKey = null)
        {
            if (list == null) return;

            if (clearExisting)
            {
                if (string.IsNullOrEmpty(categoryKey) || categoryKey.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    var items = list.Items;
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        items[i].Delete();
                    }
                }
                else
                {
                    SPQuery q = new SPQuery();
                    q.Query = string.Format("<Where><Eq><FieldRef Name='CategoryKey'/><Value Type='Text'>{0}</Value></Eq></Where>", categoryKey);
                    var items = list.GetItems(q);
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        items[i].Delete();
                    }
                }
            }

            var allPrograms = PgCategorySeedData.GetPrograms();
            var targetPrograms = string.IsNullOrEmpty(categoryKey) || categoryKey.Equals("All", StringComparison.OrdinalIgnoreCase)
                ? allPrograms
                : allPrograms.Where(p => p.CategoryKey.Equals(categoryKey, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var prog in targetPrograms)
            {
                if (!clearExisting)
                {
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><And><Eq><FieldRef Name='Title'/><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='CategoryKey'/><Value Type='Text'>{1}</Value></Eq></And></Where>",
                        prog.TitleAr, prog.CategoryKey);
                    if (list.GetItems(query).Count > 0) continue;
                }

                SPListItem item = list.AddItem();
                item["Title"] = prog.TitleAr;
                item["TitleEn"] = prog.TitleEn;
                item["CategoryKey"] = prog.CategoryKey;
                item["DegreeGroupKey"] = prog.DegreeGroupKey;
                item["DegreeGroupTitleAr"] = prog.DegreeGroupTitleAr;
                item["DegreeGroupTitleEn"] = prog.DegreeGroupTitleEn;
                item["CollegeAr"] = prog.CollegeAr;
                item["CollegeEn"] = prog.CollegeEn;
                item["DepartmentAr"] = prog.DepartmentAr;
                item["DepartmentEn"] = prog.DepartmentEn;
                item["ScopeAr"] = prog.ScopeAr;
                item["ScopeEn"] = prog.ScopeEn;
                item["DetailsUrl"] = prog.DetailsUrl;
                item["ApplyUrl"] = prog.ApplyUrl;
                item["ItemOrder"] = prog.ItemOrder;
                item["IsActive"] = prog.IsActive;
                item.Update();
            }
        }

        public static void ReseedCategory(SPWeb web, PgProgramCategory category)
        {
            Guid siteId = Guid.Empty;
            try
            {
                if (web != null && web.Site != null) siteId = web.Site.ID;
                else if (SPContext.Current != null && SPContext.Current.Site != null) siteId = SPContext.Current.Site.ID;
                else if (SPContext.Current != null && SPContext.Current.Web != null && SPContext.Current.Web.Site != null) siteId = SPContext.Current.Web.Site.ID;
            }
            catch { }

            if (siteId == Guid.Empty) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb elevatedWeb = PgCategoryRepository.OpenTargetWeb(site))
                {
                    if (elevatedWeb == null) return;
                    bool original = elevatedWeb.AllowUnsafeUpdates;
                    try
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;
                        SPList overviewList = EnsureOverviewList(elevatedWeb);
                        SPList progList = EnsureProgramsList(elevatedWeb);
                        SPList detailsList = EnsureProgramDetailsList(elevatedWeb);

                        string catKey = category == PgProgramCategory.All ? "All" : category.ToString();
                        SeedPrograms(progList, true, catKey);

                        if (category == PgProgramCategory.All)
                        {
                            SeedOverviews(overviewList, true);
                            SeedProgramDetails(detailsList, true);
                        }
                    }
                    finally
                    {
                        elevatedWeb.AllowUnsafeUpdates = original;
                    }
                }
            });
        }

        #endregion

        #region Program Details List Provisioning

        private static SPList EnsureProgramDetailsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(PgCategoryConstants.ListProgramDetails);
            if (list == null)
            {
                Guid id = web.Lists.Add(PgCategoryConstants.ListProgramDetails, "Postgraduate Program Details Directory", SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
            }

            AddTextField(list, "ProgramCode");
            AddTextField(list, "TitleEn");
            AddTextField(list, "CategoryKey");
            AddTextField(list, "DegreeAr");
            AddTextField(list, "DegreeEn");
            AddTextField(list, "CollegeAr");
            AddTextField(list, "CollegeEn");
            AddTextField(list, "DepartmentAr");
            AddTextField(list, "DepartmentEn");
            AddTextField(list, "ScopeAr");
            AddTextField(list, "ScopeEn");

            AddTextField(list, "DurationAr");
            AddTextField(list, "DurationEn");
            AddTextField(list, "CreditHoursAr");
            AddTextField(list, "CreditHoursEn");
            AddTextField(list, "LanguageAr");
            AddTextField(list, "LanguageEn");
            AddTextField(list, "StudySystemAr");
            AddTextField(list, "StudySystemEn");
            AddTextField(list, "TuitionFeesAr");
            AddTextField(list, "TuitionFeesEn");

            AddNoteField(list, "OverviewAr");
            AddNoteField(list, "OverviewEn");
            AddNoteField(list, "MissionAr");
            AddNoteField(list, "MissionEn");
            AddNoteField(list, "ObjectivesAr");
            AddNoteField(list, "ObjectivesEn");
            AddNoteField(list, "AdmissionRequirementsAr");
            AddNoteField(list, "AdmissionRequirementsEn");
            AddNoteField(list, "StudyPlanAr");
            AddNoteField(list, "StudyPlanEn");
            AddTextField(list, "StudyPlanPdfUrl");
            AddNoteField(list, "CareerOpportunitiesAr");
            AddNoteField(list, "CareerOpportunitiesEn");

            AddTextField(list, "ApplyUrl");
            AddTextField(list, "PartnerUniversityAr");
            AddTextField(list, "PartnerUniversityEn");
            AddTextField(list, "PartnerUniversityUrl");
            AddTextField(list, "ContactEmail");
            AddBoolField(list, "IsActive");

            if (list.ItemCount == 0)
            {
                SeedProgramDetails(list, false);
            }

            return list;
        }

        public static void SeedProgramDetails(SPList list, bool clearExisting)
        {
            if (list == null) return;

            if (clearExisting && list.ItemCount > 0)
            {
                var items = list.Items;
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    items[i].Delete();
                }
            }

            var detailsList = PgProgramDetailsSeedData.GetSeedDetails();
            foreach (var d in detailsList)
            {
                if (!clearExisting)
                {
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><Eq><FieldRef Name='ProgramCode'/><Value Type='Text'>{0}</Value></Eq></Where>", d.ProgramCode);
                    if (list.GetItems(query).Count > 0) continue;
                }

                SPListItem item = list.AddItem();
                item["Title"] = d.TitleAr;
                item["TitleEn"] = d.TitleEn;
                item["ProgramCode"] = d.ProgramCode;
                item["CategoryKey"] = d.CategoryKey;
                item["DegreeAr"] = d.DegreeAr;
                item["DegreeEn"] = d.DegreeEn;
                item["CollegeAr"] = d.CollegeAr;
                item["CollegeEn"] = d.CollegeEn;
                item["DepartmentAr"] = d.DepartmentAr;
                item["DepartmentEn"] = d.DepartmentEn;
                item["ScopeAr"] = d.ScopeAr;
                item["ScopeEn"] = d.ScopeEn;

                item["DurationAr"] = d.DurationAr;
                item["DurationEn"] = d.DurationEn;
                item["CreditHoursAr"] = d.CreditHoursAr;
                item["CreditHoursEn"] = d.CreditHoursEn;
                item["LanguageAr"] = d.LanguageAr;
                item["LanguageEn"] = d.LanguageEn;
                item["StudySystemAr"] = d.StudySystemAr;
                item["StudySystemEn"] = d.StudySystemEn;
                item["TuitionFeesAr"] = d.TuitionFeesAr;
                item["TuitionFeesEn"] = d.TuitionFeesEn;

                item["OverviewAr"] = d.OverviewAr;
                item["OverviewEn"] = d.OverviewEn;
                item["MissionAr"] = d.MissionAr;
                item["MissionEn"] = d.MissionEn;
                item["ObjectivesAr"] = d.ObjectivesAr;
                item["ObjectivesEn"] = d.ObjectivesEn;
                item["AdmissionRequirementsAr"] = d.AdmissionRequirementsAr;
                item["AdmissionRequirementsEn"] = d.AdmissionRequirementsEn;
                item["StudyPlanAr"] = d.StudyPlanAr;
                item["StudyPlanEn"] = d.StudyPlanEn;
                item["StudyPlanPdfUrl"] = d.StudyPlanPdfUrl;
                item["CareerOpportunitiesAr"] = d.CareerOpportunitiesAr;
                item["CareerOpportunitiesEn"] = d.CareerOpportunitiesEn;

                item["ApplyUrl"] = d.ApplyUrl;
                item["PartnerUniversityAr"] = d.PartnerUniversityAr;
                item["PartnerUniversityEn"] = d.PartnerUniversityEn;
                item["PartnerUniversityUrl"] = d.PartnerUniversityUrl;
                item["ContactEmail"] = d.ContactEmail;
                item["IsActive"] = d.IsActive;

                item.Update();
            }
        }

        #endregion

        #region Field Helpers

        private static void AddTextField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Text, false);
                list.Update();
            }
        }

        private static void AddNoteField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Note, false);
                SPFieldMultiLineText f = (SPFieldMultiLineText)list.Fields[name];
                f.RichText = false;
                f.Update();
                list.Update();
            }
        }

        private static void AddNumberField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Number, false);
                list.Update();
            }
        }

        private static void AddBoolField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Boolean, false);
                list.Update();
            }
        }

        #endregion
    }
}
