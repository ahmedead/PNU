using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu
{

    /// <summary>
    /// Idempotent provisioner for the two side-menu lists. Structure mirrors
    /// TopMenuLevel1 / TopMenuLevel2 (Title, Title_EN, URL, ItemOrder, Visibility,
    /// plus a Parent lookup on Level2 pointing back to Level1.Title).
    /// </summary>
    public static class SideMenuListProvisioner
    {
        public const string LIST_LEVEL1 = "SideMenuLevel1";
        public const string LIST_LEVEL2 = "SideMenuLevel2";

        // ------------------------------------------------------------------
        //  Internal field names on the "AllFacultyDepartments" list (Admin web).
        //  Each array is tried IN ORDER and the first non-empty value wins, so
        //  the exact convention doesn't need to be known up front. If none match,
        //  SeedDepartments logs the list's real internal field names via
        //  Publics.WriteToLog - add the correct one to the front of the array.
        // ------------------------------------------------------------------
        private static readonly string[] DEPT_FIELDS_TITLE_AR =
            { "Title", "DEPT_DSC", "DEPT_NAME_AR", "Dept_Title" };

        private static readonly string[] DEPT_FIELDS_TITLE_EN =
            { "Title_EN", "DEPT_DSC_EN", "DEPT_NAME_EN", "Dept_Title_EN" };

        private static readonly string[] DEPT_FIELDS_CODE =
            { "SecCode", "DEPT_CODE", "Code", "SEC_CODE" };

        // Page layout + controls for the generated pages
        private const string PAGE_LAYOUT_URL = "/_catalogs/masterpage/NewSideMenu.aspx";
        private const string CTRL_CONTACT = "PNU.Internet/Colleges/DGA/ucCollegeContactDga.ascx";
        private const string CTRL_DOCUMENTS = "PNU.Internet/Colleges/DGA/ucCollegeDocuments.ascx";
        private const string CTRL_PROGRAMS = "PNU.Internet/Colleges/DGA/ucCollegeProgramsDga.ascx";
        private const string CTRL_Advertisments = "PNU.Internet/MediaCenter/Advertisements/ucAllAdvertisementsDGA.ascx";
        private const string CTRL_Hierarchy = "PNU.Internet/Faculties/DGA/OrgStructure/ucFacultyOrgStructureDga.ascx";
        private const string CTRL_Achievements = "PNU.Internet/Colleges/Details/ucCollegeAchievements.ascx";
        private const string CTRL_Agancies = "PNU.Internet/Colleges/Details/ucCollegeAgancies.ascx";
        private const string CTRL_Facilities = "PNU.Internet/Colleges/Details/ucCollegeFacilities.ascx";
        private const string CTRL_Researches = "PNU.Internet/Faculties/DGA/Research/ucFacultyResearchDga.ascx";
        private const string CTRL_Students = "PNU.Internet/Colleges/Details/ucCollegeStudents.ascx";
        private const string CTRL_Services = "PNU.Internet/Colleges/Details/ucCollegeStudentServices.ascx";
        private const string CTRL_Clubs = "PNU.Internet/Colleges/Details/ucCollegeClubs.ascx";
        private const string CTRL_Initiatives = "PNU.Internet/Colleges/Details/ucCollegeInitiatives.ascx";
        private const string CTRL_Trainings = "PNU.Internet/Colleges/Details/ucCollegeTrainings.ascx";
        private const string CTRL_SharedCONTACT = "PNU.Internet/Shared/ucContactUsDGA.ascx";
        private const string CTRL_SharedAbout = "PNU.Internet/Shared/About/ucSharedAboutDga.ascx";
        private const string CTRL_DestDepartments = "PNU.Internet/GeneralDest/ucDestDepartments.ascx";
        private const string CTRL_DestCertificates = "PNU.Internet/GeneralDest/ucDestCertificates.ascx";
        private const string CTRL_DestSections = "PNU.Internet/GeneralDest/ucDestSections.ascx";
        private const string CTRL_EntitySection = "PNU.Internet/Shared/EntitySection/ucEntitySection.ascx";
        private const string CTRL_CTREntitySection = "PNU.Internet/Centers/DGA/ucCenterBeneficiariesDga.ascx";
        private const string CTRL_CTREPrograms = "PNU.Internet/Centers/DGA/ucCenterProgramsDga.ascx";
        private const string CTRL_CTREDepartments = "PNU.Internet/Centers/DGA/ucCenterDepartmentsDga.ascx";
        private const string CTRL_CTRERecord = "PNU.Internet/Centers/DGA/ucCenterRecordDga.ascx";
        private const string CTRL_CTREDigitalChannels = "PNU.Internet/Centers/DGA/ucCenterDigitalChannelsDga.ascx";


        private const string PAGE_CONTACT = "CollegeContacts.aspx";
        private const string PAGE_DOCUMENTS = "CollegeDocuments.aspx";
        private const string PAGE_PROGRAMS = "CollegePrograms.aspx";
        private const string PAGE_Hierarchy = "NewHierarchy.aspx";
        private const string PAGE_Achievements = "NewAchievements.aspx";
        private const string PAGE_Agancies = "NewAgancies.aspx";
        private const string PAGE_Facilities = "NewFacilities.aspx";
        private const string PAGE_Researches = "NewResearches.aspx";
        private const string PAGE_Students = "NewStudents.aspx";
        private const string PAGE_Services = "NewServices.aspx";
        private const string PAGE_Clubs = "NewClubs.aspx";
        private const string PAGE_Initiatives = "NewInitiatives.aspx";
        private const string PAGE_Trainings = "NewTrainings.aspx";
        

        private const string PAGE_SharedCONTACT = "SharedContactUs.aspx";
        private const string PAGE_SharedAbout = "SharedAbout.aspx";

        //private const string CTRL_SharedDOCUMENTS = "PNU.Internet/Colleges/DGA/ucCollegeDocuments.ascx";
        //private const string PAGE_SharedDOCUMENTS = "SharedDocuments.aspx";

        //private const string PAGE_SharedCONTACT = "SharedContactUs.aspx";

        // ControlLoaderWebPart - same type/assembly used by PageGenerator.
        // Loaded via reflection so this file needs no compile-time reference.
        private const string WP_TYPE_NAME = "PNU.Internet.WebParts.ControlLoaderWebPart.ControlLoaderWebPart";
        private const string WP_ASSEMBLY_NAME = "PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3";

        /// <summary>Web part zone on DGANewBlankWithSideMenu.aspx - adjust if the layout differs.</summary>
        private const string WP_ZONE_ID = "TopZone";

        /// <summary>
        /// Ensures both lists exist on the supplied web. Safe to call repeatedly.
        /// Caller is responsible for elevation (RunWithElevatedPrivileges) if needed.
        /// </summary>
        public static void EnsureLists(SPWeb web)
        {
            try
            {
                bool originalAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;

                SPList level1 = EnsureLevel1List(web);
                EnsureLevel2List(web, level1);

                web.AllowUnsafeUpdates = originalAllowUnsafeUpdates;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.EnsureLists", ex.Message);
            }
        }

        // ==================================================================
        //  SEEDING
        // ==================================================================

        /// <summary>
        /// Seeds the menu for a freshly-provisioned web. Only Faculties webs get
        /// the standard college menu; other webs are left empty for manual entry.
        /// No-op if Level1 already has items.
        /// </summary>
        public static void SeedMenu(SPWeb web)
        {
            try
            {
                if (web == null) return;

                // Only seed college subwebs
                if (web.ServerRelativeUrl.IndexOf("/Faculties/", StringComparison.OrdinalIgnoreCase) < 0)
                    return;

                SPList level1 = web.Lists.TryGetList(LIST_LEVEL1);
                SPList level2 = web.Lists.TryGetList(LIST_LEVEL2);
                if (level1 == null || level2 == null) return;

                if (level1.ItemCount > 0) return;   // already seeded

                bool originalAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;

                // Build "/ar/Faculties/IT/" (or "/en/...") from the web's own path.
                // ServerRelativeUrl normally already carries the language segment;
                // if it doesn't, prepend the one matching this web's language.
                string webPath = web.ServerRelativeUrl.TrimEnd('/') + "/";
                string langSegment = web.Language == 1025 ? "/ar/" : "/en/";

                if (!webPath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                    && !webPath.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                {
                    webPath = langSegment + webPath.TrimStart('/');
                }

                AddLevel1(level1, "الرئيسية", "Main", webPath, 1);
                int MoreAboutCollegeItemId = AddLevel1(level1, "المزيد عن الكلية", "More About College", "", 2);
                int departmentsItemId = AddLevel1(level1, "الأقسام", "Departments", "", 3);
                AddLevel1(level1, "البرامج", "Programs", webPath + "Pages/" + PAGE_PROGRAMS, 4);
                AddLevel1(level1, "الأخبار", "News", webPath + "News/Pages/default.aspx", 5);
                AddLevel1(level1, "الإعلانات", "Announcements", webPath + "Announcements/Pages/default.aspx", 6);
                AddLevel1(level1, "المستندات والنماذج والأدلة", "Documents, Forms and Guides",
                          webPath + "Pages/" + PAGE_DOCUMENTS, 7);
                AddLevel1(level1, "تواصل مع الكلية", "Contact the College",
                          webPath + "Pages/" + PAGE_CONTACT, 8);

                SeedDepartments(web, level2, webPath, departmentsItemId);

                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "الهيكل التنظيمي للكلية", "Hierarchy", webPath + "Pages/" + PAGE_Hierarchy, 1);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "إنجازات الكلية", "Achievements", webPath + "Pages/" + PAGE_Achievements, 2);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "وكالات الكلية", "Agancies", webPath + "Pages/" + PAGE_Agancies, 3);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "مرافق الكلية", "Facilities", webPath + "Pages/" + PAGE_Facilities, 4);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "البحث العلمي في الكلية", "Researches", webPath + "Pages/" + PAGE_Researches, 5);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "الطالبات في الكلية", "Students", webPath + "Pages/" + PAGE_Students, 6);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "الخدمات الطلابية في الكلية", "Services", webPath + "Pages/" + PAGE_Services, 7);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "الأندية الطلابية في الكلية", "Clubs", webPath + "Pages/" + PAGE_Clubs, 8);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "المبادرات", "Initiatives", webPath + "Pages/" + PAGE_Initiatives, 9);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الكلية", "التدريب في الكلية", "Trainings", webPath + "Pages/" + PAGE_Trainings, 10);

                web.AllowUnsafeUpdates = originalAllowUnsafeUpdates;

                // Create the pages the Level1 items point at
                EnsurePage(web, PAGE_PROGRAMS, "البرامج","Programs", CTRL_PROGRAMS);
                EnsurePage(web, PAGE_DOCUMENTS, "المستندات والنماذج والأدلة","Documents, Forms and Guides", CTRL_DOCUMENTS);
                EnsurePage(web, PAGE_CONTACT, "تواصل مع الكلية","Contact the College", CTRL_CONTACT);

                EnsurePage(web, PAGE_Hierarchy, "الهيكل التنظيمي للكلية", "Hierarchy", CTRL_Hierarchy);
                EnsurePage(web, PAGE_Achievements, "إنجازات الكلية", "Achievements", CTRL_Achievements);
                EnsurePage(web, PAGE_Agancies, "وكالات الكلية", "Agancies", CTRL_Agancies);
                EnsurePage(web, PAGE_Facilities, "مرافق الكلية", "Facilities", CTRL_Facilities);
                EnsurePage(web, PAGE_Researches, "البحث العلمي في الكلية", "Researches", CTRL_Researches);
                EnsurePage(web, PAGE_Students, "الطالبات في الكلية", "Students", CTRL_Students);
                EnsurePage(web, PAGE_Services, "الخدمات الطلابية في الكلية", "Services", CTRL_Services);
                EnsurePage(web, PAGE_Clubs, "الأندية الطلابية في الكلية", "Clubs", CTRL_Clubs);
                EnsurePage(web, PAGE_Initiatives, "المبادرات", "Initiatives", CTRL_Initiatives);
                EnsurePage(web, PAGE_Trainings, "التدريب في الكلية", "Trainings", CTRL_Trainings);



            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.SeedMenu", ex.Message);
            }
        }

        public static void SeedMenuForAgencies(SPWeb web)
        {
            try
            {
                if (web == null) return;

                // Only seed college subwebs
                if (web.ServerRelativeUrl.IndexOf("/Agencies/", StringComparison.OrdinalIgnoreCase) < 0)
                    return;

                SPList level1 = web.Lists.TryGetList(LIST_LEVEL1);
                SPList level2 = web.Lists.TryGetList(LIST_LEVEL2);
                if (level1 == null || level2 == null) return;

                if (level1.ItemCount > 0) return;   // already seeded

                bool originalAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;

                // Build "/ar/Faculties/IT/" (or "/en/...") from the web's own path.
                // ServerRelativeUrl normally already carries the language segment;
                // if it doesn't, prepend the one matching this web's language.
                string webPath = web.ServerRelativeUrl.TrimEnd('/') + "/";
                string langSegment = web.Language == 1025 ? "/ar/" : "/en/";

                if (!webPath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                    && !webPath.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                {
                    webPath = langSegment + webPath.TrimStart('/');
                }

                AddLevel1(level1, "الرئيسية", "Main", webPath + "Pages/" + PAGE_SharedAbout, 1);
                int MoreAboutCollegeItemId = AddLevel1(level1, "المزيد عن الوكالة", "More About Vice Rectorate", "", 2);
                int departmentsItemId = AddLevel1(level1, "الجهات التابعة", "Affiliated entities", "", 3);

                //AddLevel1(level1, "الأخبار", "News", webPath + "News/Pages/default.aspx", 5);
                //AddLevel1(level1, "الإعلانات", "Announcements", webPath + "Announcements/Pages/default.aspx", 6);
                AddLevel1(level1, "المستندات والنماذج والأدلة", "Documents, Forms and Guides",webPath + "Pages/" + PAGE_DOCUMENTS, 4);
                AddLevel1(level1, "تواصل معنا", "Contact Us",webPath + "Pages/" + PAGE_SharedCONTACT, 5);

                
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الوكالة", "الهيكل التنظيمي", "Hierarchy", webPath + "Pages/" + PAGE_Hierarchy, 1);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الوكالة", "الشهادات والجوائز", "AgencyAchievements", webPath + "Pages/" + "AgencyAchievements.aspx", 2);


                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "العمادات", "AgencyDeens", webPath + "Pages/" + "AgencyDeens.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "الإدارات", "AgencyDepartments", webPath + "Pages/" + "AgencyDepartments.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "المراكز", "AgencyCenters", webPath + "Pages/" + "AgencyCenters.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "الوحدات", "AgencyUnits", webPath + "Pages/" + "AgencyUnits.aspx", 2);

                web.AllowUnsafeUpdates = originalAllowUnsafeUpdates;

                // Create the pages the Level1 items point at
                EnsurePage(web, PAGE_SharedAbout, "الرئيسية ", "About", CTRL_SharedAbout);
                EnsurePage(web, PAGE_DOCUMENTS, "المستندات والنماذج والأدلة", "Documents, Forms and Guides", CTRL_DOCUMENTS);
                EnsurePage(web, PAGE_SharedCONTACT, "تواصل معنا ", "Contact Us", CTRL_SharedCONTACT);
                EnsurePage(web, PAGE_Hierarchy, "الهيكل التنظيمي", "Hierarchy", CTRL_Hierarchy);

                EnsurePage(web, "AgencyAchievements.aspx", "الشهادات والجوائز", "AgencyAchievements", CTRL_DestCertificates, "ListName#AgencyAchievements");
                EnsurePage(web, "AgencyDeens.aspx", "العمادات", "AgencyDeens", CTRL_DestDepartments, "ListName#AgencyDeens");
                EnsurePage(web, "AgencyDepartments.aspx", "الإدارات", "AgencyDepartments", CTRL_DestDepartments, "ListName#AgencyDepartments");
                EnsurePage(web, "AgencyCenters.aspx", "المراكز", "AgencyCenters", CTRL_DestDepartments, "ListName#AgencyCenters");
                EnsurePage(web, "AgencyUnits.aspx", "الوحدات", "AgencyUnits", CTRL_DestDepartments, "ListName#AgencyUnits");






            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.SeedMenu", ex.Message);
            }
        }
        public static void SeedMenuForDeenShips(SPWeb web)
        {
            try
            {
                if (web == null) return;

                // Only seed college subwebs
                if (web.ServerRelativeUrl.IndexOf("/Deanship/", StringComparison.OrdinalIgnoreCase) < 0)
                    return;

                SPList level1 = web.Lists.TryGetList(LIST_LEVEL1);
                SPList level2 = web.Lists.TryGetList(LIST_LEVEL2);
                if (level1 == null || level2 == null) return;

                if (level1.ItemCount > 0) return;   // already seeded

                bool originalAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;

                // Build "/ar/Faculties/IT/" (or "/en/...") from the web's own path.
                // ServerRelativeUrl normally already carries the language segment;
                // if it doesn't, prepend the one matching this web's language.
                string webPath = web.ServerRelativeUrl.TrimEnd('/') + "/";
                string langSegment = web.Language == 1025 ? "/ar/" : "/en/";

                if (!webPath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                    && !webPath.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                {
                    webPath = langSegment + webPath.TrimStart('/');
                }

                AddLevel1(level1, "الرئيسية", "Main", webPath + "Pages/" + PAGE_SharedAbout, 1);
                int MoreAboutCollegeItemId = AddLevel1(level1, "المزيد عن العمادة", "More About Vice Rectorate", "", 2);
                int departmentsItemId = AddLevel1(level1, "الجهات التابعة", "Affiliated entities", "", 3);

                AddLevel1(level1, "المستندات والنماذج والأدلة", "Documents, Forms and Guides", webPath + "Pages/DnDocuments.aspx" , 4);
                AddLevel1(level1, "الأخبار", "News", webPath + "News/Pages/default.aspx", 5);
                AddLevel1(level1, "الإعلانات", "Advertisements", webPath + "Pages/DnAdvertisements.aspx", 6);
                
                AddLevel1(level1, "تواصل معنا", "Contact Us", webPath + "Pages/" + PAGE_SharedCONTACT, 7);


                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن العمادة", "الهيكل التنظيمي", "Hierarchy", webPath + "Pages/" + PAGE_Hierarchy, 1);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن العمادة", "مسارات المستفيدين", "Beneficiary Pathways", webPath + "Pages/" + "DnBeneficiaryPathways.aspx", 2);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن العمادة", "الخدمات", "Services", webPath + "Pages/" + "DnServices.aspx", 2);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن العمادة", "البرامج والمبادرات", "Programs and Initiatives", webPath + "Pages/" + "DnInitiatives.aspx", 2);
                //AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن العمادة", "مسارات المستفيدين", "AgencyAchievements", webPath + "Pages/" + "AgencyAchievements.aspx", 2);

                //DnBeneficiaryPathways

                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "الوكالات", "Deenship Agencies", webPath + "Pages/" + "DnAgencies.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "الإدارات", "Deenship Departments", webPath + "Pages/" + "DnDepartments.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "المراكز", "Deenship Centers", webPath + "Pages/" + "DnCenters.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "الوحدات", "Deenship Units", webPath + "Pages/" + "DnUnits.aspx", 2);

                web.AllowUnsafeUpdates = originalAllowUnsafeUpdates;

                // Create the pages the Level1 items point at
                EnsurePage(web, PAGE_SharedAbout, "الرئيسية ", "About", CTRL_SharedAbout);
                EnsurePage(web, "DnDocuments.aspx", "المستندات والنماذج والأدلة", "Documents, Forms and Guides", CTRL_DOCUMENTS, "LIST_NAME#DnDocuments");
                EnsurePage(web, PAGE_SharedCONTACT, "تواصل معنا ", "Contact Us", CTRL_SharedCONTACT);
                EnsurePage(web, PAGE_Hierarchy, "الهيكل التنظيمي", "Hierarchy", CTRL_Hierarchy);

                EnsurePage(web, "DnAdvertisements.aspx", "الإعلانات", "Advertisements", CTRL_Advertisments);
                EnsurePage(web, "DnBeneficiaryPathways.aspx", "مسارات المستفيدين", "Beneficiary Pathways", CTRL_EntitySection, "TracksListName#DnBeneficiaryPathwaysTracks;BulletsListName#DnBeneficiaryPathwaysBullets");
                EnsurePage(web, "DnServices.aspx", "الخدمات", "Services", CTRL_EntitySection, "TracksListName#DnServicesTracks;BulletsListName#DnServicesBullets" );
                EnsurePage(web, "DnInitiatives.aspx", "البرامج والمبادرات", "Programs and Initiatives", CTRL_EntitySection, "TracksListName#DnInitiativesTracks;BulletsListName#DnInitiativesBullets");

                EnsurePage(web, "DnAgencies.aspx", "الوكالات", "Deenship Agencies", CTRL_DestDepartments, "ListName#DnAgencies");
                EnsurePage(web, "DnDepartments.aspx", "الإدارات", "Deenship Departments", CTRL_DestDepartments, "ListName#DnDepartments");
                EnsurePage(web, "DnCenters.aspx", "المراكز", "Deenship Centers", CTRL_DestDepartments, "ListName#DnCenters");
                EnsurePage(web, "DnUnits.aspx", "الوحدات", "Deenship Units", CTRL_DestDepartments, "ListName#DnUnits");

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.SeedMenu", ex.Message);
            }
        }

        public static void SeedMenuForDepartments(SPWeb web)
        {
            try
            {
                if (web == null) return;

                //// Only seed college subwebs
                //if (web.ServerRelativeUrl.IndexOf("/Departments/", StringComparison.OrdinalIgnoreCase) < 0)
                //    return;

                string _Prefix = "DEPT";
                string _PrefixEntity = "Department";
                string _PrefixEntity_Ar = "Department";

                SPList level1 = web.Lists.TryGetList(LIST_LEVEL1);
                SPList level2 = web.Lists.TryGetList(LIST_LEVEL2);
                if (level1 == null || level2 == null) return;

                if (level1.ItemCount > 0) return;   // already seeded

                bool originalAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;

                string webPath = web.ServerRelativeUrl.TrimEnd('/') + "/";
                string langSegment = web.Language == 1025 ? "/ar/" : "/en/";

                if (!webPath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                    && !webPath.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                {
                    webPath = langSegment + webPath.TrimStart('/');
                }

                AddLevel1(level1, "الرئيسية", "Main", webPath + "Pages/" + PAGE_SharedAbout, 1);
                int MoreAboutCollegeItemId = AddLevel1(level1, "المزيد عن الإدارة", "More About " + _PrefixEntity, "", 2);
                int departmentsItemId = AddLevel1(level1, "الجهات التابعة", "Affiliated entities", "", 3);

                AddLevel1(level1, "المستندات والنماذج والأدلة", "Documents, Forms and Guides", webPath + "Pages/"+ _Prefix + "Documents.aspx", 4);
                AddLevel1(level1, "تواصل معنا", "Contact Us", webPath + "Pages/" + PAGE_SharedCONTACT, 5);


                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن " + _PrefixEntity_Ar, "الهيكل التنظيمي", "Hierarchy", webPath + "Pages/" + _Prefix + "Hierarchy.aspx", 1);
                //AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن " + _PrefixEntity_Ar, "مسارات المستفيدين", "Beneficiary Pathways", webPath + "Pages/" + _Prefix + "BeneficiaryPathways.aspx", 2);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن " + _PrefixEntity_Ar, "الخدمات والبرامج", "Services", webPath + "Pages/"+ _Prefix + "Services.aspx", 2);
                AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن " + _PrefixEntity_Ar, "البرامج والمبادرات", "Programs and Initiatives", webPath + "Pages/"+ _Prefix + "Initiatives.aspx", 2);
                //AddLevel2MoreAbout(level2, MoreAboutCollegeItemId, "المزيد عن الإدارة", "مسارات المستفيدين", "AgencyAchievements", webPath + "Pages/" + "AgencyAchievements.aspx", 2);

                //DnBeneficiaryPathways

                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "الوكالات", _PrefixEntity +" Agencies", webPath + "Pages/"+ _Prefix + "Agencies.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "العمادات", _PrefixEntity + " Deenships", webPath + "Pages/"+ _Prefix + "Deenships.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "المراكز", _PrefixEntity + " Centers", webPath + "Pages/"+ _Prefix + "Centers.aspx", 2);
                AddLevel2MoreAbout(level2, departmentsItemId, "الجهات التابعة", "الوحدات", _PrefixEntity + " Units", webPath + "Pages/"+ _Prefix + "Units.aspx", 2);

                web.AllowUnsafeUpdates = originalAllowUnsafeUpdates;

                // Create the pages the Level1 items point at
                EnsurePage(web, PAGE_SharedAbout, "الرئيسية ", "About", CTRL_SharedAbout);
                EnsurePage(web,  _Prefix + "Documents.aspx", "المستندات والنماذج والأدلة", "Documents, Forms and Guides", CTRL_DOCUMENTS, "LIST_NAME#"+ _Prefix + "Documents");
                EnsurePage(web, PAGE_SharedCONTACT, "تواصل معنا ", "Contact Us", CTRL_SharedCONTACT);
                EnsurePage(web,  _Prefix + "Hierarchy.aspx", "الهيكل التنظيمي", "Hierarchy", CTRL_EntitySection, "TracksListName#"+ _Prefix + "HierarchyTracks;BulletsListName#"+ _Prefix + "HierarchyBullets");

                EnsurePage(web,  _Prefix + "Advertisements.aspx", "الإعلانات", "Advertisements", CTRL_Advertisments);
                //EnsurePage(web,  _Prefix + "BeneficiaryPathways.aspx", "مسارات المستفيدين", "Beneficiary Pathways", CTRL_EntitySection, "TracksListName#"+ _Prefix + "BeneficiaryPathwaysTracks;BulletsListName#"+ _Prefix + "BeneficiaryPathwaysBullets");
                EnsurePage(web,  _Prefix + "Services.aspx", "الخدمات والبرامج", "Services", CTRL_EntitySection, "TracksListName#"+ _Prefix + "ServicesTracks;BulletsListName#"+ _Prefix + "ServicesBullets");
                EnsurePage(web,  _Prefix + "Initiatives.aspx", "البرامج والمبادرات", "Programs and Initiatives", CTRL_EntitySection, "TracksListName#"+ _Prefix + "InitiativesTracks;BulletsListName#"+ _Prefix + "InitiativesBullets");

                EnsurePage(web,  _Prefix + "Agencies.aspx", "الوكالات", "Department Agencies", CTRL_DestDepartments, "ListName#"+ _Prefix + "Agencies");
                EnsurePage(web,  _Prefix + "Deenships.aspx", "العمادات", "Department Deenships", CTRL_DestDepartments, "ListName#"+ _Prefix + "Deenships");
                EnsurePage(web,  _Prefix + "Centers.aspx", "المراكز", "Department Centers", CTRL_DestDepartments, "ListName#"+ _Prefix + "Centers");
                EnsurePage(web,  _Prefix + "Units.aspx", "الوحدات", "Department Units", CTRL_DestDepartments, "ListName#"+ _Prefix + "Units");

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.SeedMenu", ex.Message);
            }
        }

        public static void SeedMenuForCenters(SPWeb web)
        {
            try
            {
                if (web == null) return;

                string _Prefix = "Center";
                string _PrefixEntity = "Center";
                string _PrefixEntity_Ar = "المركز";

                SPList level1 = web.Lists.TryGetList(LIST_LEVEL1);
                SPList level2 = web.Lists.TryGetList(LIST_LEVEL2);
                if (level1 == null || level2 == null) return;

                if (level1.ItemCount > 0) return;   // already seeded

                bool originalAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;

                string webPath = web.ServerRelativeUrl.TrimEnd('/') + "/";
                string langSegment = web.Language == 1025 ? "/ar/" : "/en/";

                if (!webPath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                    && !webPath.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                {
                    webPath = langSegment + webPath.TrimStart('/');
                }

                AddLevel1(level1, "الرئيسية", "Main", webPath + "Pages/" + PAGE_SharedAbout, 1);
                int MoreAboutCenterItemId = AddLevel1(level1, "المزيد عن المركز", "More About " + _PrefixEntity, "", 2);

                
                AddLevel2MoreAbout(level2, MoreAboutCenterItemId, "المزيد عن " + _PrefixEntity_Ar, "الفئات المستفيدة", "Beneficiaries", webPath + "Pages/" + _Prefix + "Beneficiaries.aspx", 1);
                AddLevel2MoreAbout(level2, MoreAboutCenterItemId, "المزيد عن " + _PrefixEntity_Ar, "سجل سموق المهاري", "Smoc Record", webPath + "Pages/" + _Prefix + "SmocRecord.aspx", 2);
                AddLevel2MoreAbout(level2, MoreAboutCenterItemId, "المزيد عن " + _PrefixEntity_Ar, "البرامج", "Programs", webPath + "Pages/" + _Prefix + "Programs.aspx", 3);
                AddLevel2MoreAbout(level2, MoreAboutCenterItemId, "المزيد عن " + _PrefixEntity_Ar, "المنصات والخدمات المرتبطة", "Linked Services", webPath + "Pages/" + _Prefix + "LinkedServices.aspx", 4);

                AddLevel1(level1, "الإدارات", _Prefix + "Departments", webPath + "Pages/" + "CenterDepartments.aspx", 5);
                AddLevel1(level1, "تواصل مع المركز", "Contact Us", webPath + "Pages/" + PAGE_SharedCONTACT, 5);



                web.AllowUnsafeUpdates = originalAllowUnsafeUpdates;

                // Create the pages the Level1 and Level2 items point at
                EnsurePage(web, PAGE_SharedAbout, "الرئيسية ", "About", CTRL_SharedAbout);
                
                EnsurePage(web, _Prefix + "Beneficiaries.aspx", "الفئات المستفيدة", "Beneficiaries", CTRL_CTREntitySection);
                EnsurePage(web, _Prefix + "SmocRecord.aspx", "سجل سموق المهاري", "Smoc Record", CTRL_CTRERecord);
                EnsurePage(web, _Prefix + "Programs.aspx", "البرامج", "Services", CTRL_CTREPrograms);
                EnsurePage(web, _Prefix + "LinkedServices.aspx", "المنصات والخدمات المرتبطة", "Linked Services", CTRL_CTREDigitalChannels);

                EnsurePage(web, _Prefix + "Departments.aspx", "الإدارات", "Center Departments", CTRL_CTREDepartments);
                EnsurePage(web, PAGE_SharedCONTACT, "تواصل مع المركز ", "Contact Us", CTRL_SharedCONTACT);

                // Ensure and seed all Center lists
                PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA.CenterProvisioner.EnsureAllLists(web);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.SeedMenuForCenters", ex.Message);
            }
        }

        public static void AddLevel2MoreAbout(SPList level2, int parentItemId, string ParentTitle, string titleAr, string titleEn, string url, int order, bool visibility = true)
        {
            try
            {
                try
                {
                    SPListItem newItem = level2.AddItem();
                    newItem["Title"] = titleAr;
                    newItem["Title_EN"] = titleEn;
                    if (!string.IsNullOrEmpty(url))
                        newItem["URL"] = new SPFieldUrlValue { Url = url, Description = titleAr };

                    // Lookup fields need an ID-backed value, NOT a display string -
                    // assigning the bare text throws "invalid data ... read only".
                    newItem["Parent"] = new SPFieldLookupValue(parentItemId, ParentTitle);

                    newItem["ItemOrder"] = (double)order;
                    newItem["Visibility"] = visibility;
                    newItem.Update();
                }
                catch (Exception itemEx)
                {
                    // Don't let one bad row abort the whole seed
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                        "SideMenuListProvisioner.AddLevel2MoreAbout",
                        string.Format("Failed to add AddLevel2MoreAbout '{0}': {1}",
                            titleAr, itemEx.Message));
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.AddLevel2MoreAbout", ex.Message);
            }
        }

        public static void AddLevel2(SPList level2, int parentItemId, string parentTitle, string titleAr, string titleEn, string url, int order, bool visibility = true)
        {
            AddLevel2MoreAbout(level2, parentItemId, parentTitle, titleAr, titleEn, url, order, visibility);
        }

        /// <summary>
        /// Adds one Level2 item per department of this college, under "الأقسام".
        /// College code comes from the web's own "AboutCollege" list; departments
        /// come from "AllFacultyDepartments" on the Admin web - the same pair of
        /// lookups ucCollegeSections uses.
        /// </summary>
        private static void SeedDepartments(SPWeb web, SPList level2, string webPath, int parentItemId)
        {
            try
            {
                string collegeCode = GetCollegeCode(web);
                if (string.IsNullOrEmpty(collegeCode))
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                        "SideMenuListProvisioner.SeedDepartments",
                        "College_Code not found in AboutCollege for web: " + web.Url);
                    return;
                }

                using (SPSite site = new SPSite(web.Url))
                using (SPWeb adminWeb = site.OpenWeb("Admin"))
                {
                    SPList deptList = adminWeb.Lists.TryGetList("AllFacultyDepartments");
                    if (deptList == null)
                    {
                        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                            "SideMenuListProvisioner.SeedDepartments",
                            "AllFacultyDepartments list not found on Admin web");
                        return;
                    }

                    SPQuery query = new SPQuery
                    {
                        Query = string.Format(
                            @"<Where><Eq><FieldRef Name='COLL_CODE'/>
                                <Value Type='Text'>{0}</Value></Eq></Where>",
                            System.Security.SecurityElement.Escape(collegeCode))
                    };

                    SPListItemCollection departments = deptList.GetItems(query);
                    if (departments == null || departments.Count == 0)
                    {
                        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                            "SideMenuListProvisioner.SeedDepartments",
                            "No departments found for COLL_CODE=" + collegeCode);
                        return;
                    }

                    int order = 1;
                    int skipped = 0;

                    foreach (SPListItem dept in departments)
                    {
                        string titleAr = FirstNonEmptyField(dept, DEPT_FIELDS_TITLE_AR);
                        string titleEn = FirstNonEmptyField(dept, DEPT_FIELDS_TITLE_EN);
                        string code = FirstNonEmptyField(dept, DEPT_FIELDS_CODE);

                        if (string.IsNullOrEmpty(titleAr) && string.IsNullOrEmpty(titleEn))
                        {
                            skipped++;
                            continue;
                        }

                        string url = webPath + "Pages/Sections.aspx?SecCode=" + code;

                        try
                        {
                            SPListItem newItem = level2.AddItem();
                            newItem["Title"] = titleAr;
                            newItem["Title_EN"] = titleEn;
                            newItem["URL"] = new SPFieldUrlValue { Url = url, Description = titleAr };

                            // Lookup fields need an ID-backed value, NOT a display string -
                            // assigning the bare text throws "invalid data ... read only".
                            newItem["Parent"] = new SPFieldLookupValue(parentItemId, "الأقسام");

                            newItem["ItemOrder"] = (double)order++;
                            newItem["Visibility"] = true;
                            newItem.Update();
                        }
                        catch (Exception itemEx)
                        {
                            // Don't let one bad row abort the whole seed
                            Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                                "SideMenuListProvisioner.SeedDepartments",
                                string.Format("Failed to add department '{0}': {1}",
                                    titleAr, itemEx.Message));
                        }
                    }

                    if (skipped > 0)
                    {
                        // Almost always means the title field names below are wrong
                        // for this farm - log the actual internal names to fix them.
                        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                            "SideMenuListProvisioner.SeedDepartments",
                            string.Format("Skipped {0} department(s) with no title. "
                                + "Available internal field names: {1}",
                                skipped, DescribeFields(departments[0])));
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.SeedDepartments", ex.Message);
            }
        }

        /// <summary>Returns the first non-empty value among the supplied candidate field names.</summary>
        private static string FirstNonEmptyField(SPListItem item, string[] candidateNames)
        {
            foreach (string name in candidateNames)
            {
                string value = SafeField(item, name);
                if (!string.IsNullOrEmpty(value))
                    return value.Trim();
            }
            return string.Empty;
        }

        /// <summary>Comma-separated list of the item's internal field names - for diagnostics.</summary>
        private static string DescribeFields(SPListItem item)
        {
            try
            {
                List<string> names = new List<string>();
                foreach (SPField field in item.Fields)
                {
                    if (!field.Hidden && !field.ReadOnlyField)
                        names.Add(field.InternalName);
                }
                return string.Join(", ", names.ToArray());
            }
            catch (Exception)
            {
                return "(unavailable)";
            }
        }

        private static string GetCollegeCode(SPWeb web)
        {
            try
            {
                SPList aboutList = web.Lists.TryGetList("AboutCollege");
                if (aboutList == null) return string.Empty;

                SPListItemCollection items = aboutList.GetItems();
                if (items == null || items.Count == 0) return string.Empty;

                return SafeField(items[0], "College_Code").Trim();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>Adds a Level1 item and returns its new list item ID.</summary>
        public static int AddLevel1(SPList list, string titleAr, string titleEn, string url, int order, bool visibility = true)
        {
            SPListItem item = list.AddItem();
            item["Title"] = titleAr;
            item["Title_EN"] = titleEn;

            if (!string.IsNullOrEmpty(url))
                item["URL"] = new SPFieldUrlValue { Url = url, Description = titleAr };

            item["ItemOrder"] = (double)order;
            item["Visibility"] = visibility;
            item.Update();

            return item.ID;
        }

        public static void UpdateLevel1(SPList list, int itemId, string titleAr, string titleEn, string url, int order, bool visibility)
        {
            SPListItem item = list.GetItemById(itemId);
            if (item == null) return;

            item["Title"] = titleAr;
            item["Title_EN"] = titleEn;
            if (!string.IsNullOrEmpty(url))
                item["URL"] = new SPFieldUrlValue { Url = url, Description = titleAr };
            else
                item["URL"] = null;

            item["ItemOrder"] = (double)order;
            item["Visibility"] = visibility;
            item.Update();
        }

        public static void UpdateLevel2(SPList list, int itemId, int parentItemId, string parentTitle, string titleAr, string titleEn, string url, int order, bool visibility)
        {
            SPListItem item = list.GetItemById(itemId);
            if (item == null) return;

            item["Title"] = titleAr;
            item["Title_EN"] = titleEn;
            if (!string.IsNullOrEmpty(url))
                item["URL"] = new SPFieldUrlValue { Url = url, Description = titleAr };
            else
                item["URL"] = null;

            item["Parent"] = new SPFieldLookupValue(parentItemId, parentTitle);
            item["ItemOrder"] = (double)order;
            item["Visibility"] = visibility;
            item.Update();
        }

        public static void DeleteItem(SPList list, int itemId)
        {
            SPListItem item = list.GetItemById(itemId);
            if (item != null)
            {
                item.Delete();
            }
        }

        public static bool CheckPageExists(SPWeb web, string pageName)
        {
            try
            {
                if (web == null || string.IsNullOrEmpty(pageName)) return false;
                if (!PublishingWeb.IsPublishingWeb(web)) return false;

                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                if (pubWeb == null) return false;

                string pageServerRelativeUrl = web.ServerRelativeUrl.TrimEnd('/') + "/" + pubWeb.PagesListName + "/" + pageName.TrimStart('/');
                SPFile existingFile = web.GetFile(pageServerRelativeUrl);
                return existingFile != null && existingFile.Exists;
            }
            catch
            {
                return false;
            }
        }

        // ==================================================================
        //  PAGE CREATION
        // ==================================================================

        /// <summary>
        /// Creates a publishing page from the DGANewBlankWithSideMenu layout and
        /// drops the supplied user control into its web part zone.
        /// No-op if the page already exists. Follows the same sequence as
        /// PageGenerator.CreatePublishingPage.
        /// </summary>
        public static void EnsurePage(SPWeb web, string pageName, string titleAr,
                                       string titleEn, string userControlPath, string UserControlProperties = "", string pageLayoutUrl = PAGE_LAYOUT_URL)
        {
            try
            {
                if (!PublishingWeb.IsPublishingWeb(web)) return;

                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                if (pubWeb == null) return;

                // Skip if the page file already exists
                string pageServerRelativeUrl = web.ServerRelativeUrl.TrimEnd('/')
                    + "/" + pubWeb.PagesListName + "/" + pageName;
                SPFile existingFile = web.GetFile(pageServerRelativeUrl);
                if (existingFile != null && existingFile.Exists) return;

                using (SPSite site = new SPSite(web.Site.ID))
                using (SPWeb rootWeb = site.RootWeb)
                {
                    string targetLayout = string.IsNullOrEmpty(pageLayoutUrl) ? PAGE_LAYOUT_URL : pageLayoutUrl;
                    SPFile layoutFile = rootWeb.GetFile(targetLayout);
                    if (layoutFile == null || !layoutFile.Exists)
                    {
                        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                            "SideMenuListProvisioner.EnsurePage:" + pageName,
                            "Page layout not found: " + targetLayout);
                        return;
                    }

                    // Prefer the layout instance the web already exposes
                    PageLayout layout = null;
                    foreach (PageLayout pl in pubWeb.GetAvailablePageLayouts())
                    {
                        if (pl.ServerRelativeUrl.Equals(layoutFile.ServerRelativeUrl,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            layout = pl;
                            break;
                        }
                    }

                    if (layout == null)
                        layout = new PageLayout(layoutFile.Item);

                    bool originalAllowUnsafeUpdates = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;

                    try
                    {
                        PublishingPage page = pubWeb.GetPublishingPages().Add(pageName, layout);
                        page.Title = web.Language == 1025 ? titleAr : titleEn;

                        //for test
                        page.Title = web.Title + " - " + page.Title;
                        page.Update();

                        AddUserControlToPage(web, page, userControlPath, UserControlProperties);

                        SPFile pageFile = page.ListItem.File;

                        if (pageFile.CheckOutType != SPFile.SPCheckOutType.None)
                            pageFile.CheckIn("Created automatically", SPCheckinType.MajorCheckIn);

                        if (pageFile.Item.ParentList.EnableModeration)
                            pageFile.Approve("Approved automatically");

                        if (pageFile.Item.ParentList.EnableMinorVersions)
                            pageFile.Publish("Published automatically");
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = originalAllowUnsafeUpdates;
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.EnsurePage:" + pageName, ex.Message);
            }
        }

        /// <summary>
        /// Adds the ControlLoaderWebPart to the page and points it at the .ascx.
        /// The web part type is resolved by reflection (same approach as
        /// PageGenerator) so this file needs no compile-time reference to it.
        /// </summary>
        private static void AddUserControlToPage(SPWeb web, PublishingPage page, string userControlPath,string UserControlProperties="")
        {
            try
            {
                using (Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager wpManager =
                       page.ListItem.File.GetLimitedWebPartManager(
                           System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
                {
                    System.Reflection.Assembly asm = System.Reflection.Assembly.Load(WP_ASSEMBLY_NAME);
                    Type wpType = asm.GetType(WP_TYPE_NAME);

                    if (wpType == null)
                    {
                        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                            "SideMenuListProvisioner.AddUserControlToPage",
                            "WebPart type not found: " + WP_TYPE_NAME);
                        return;
                    }

                    System.Web.UI.WebControls.WebParts.WebPart wp =
                        (System.Web.UI.WebControls.WebParts.WebPart)Activator.CreateInstance(wpType);

                    wp.Title = "Control Loader WebPart";
                    wp.ChromeType = System.Web.UI.WebControls.WebParts.PartChromeType.None;

                    System.Reflection.PropertyInfo prop = wpType.GetProperty("UserControlPath");
                    if (prop == null)
                    {
                        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                            "SideMenuListProvisioner.AddUserControlToPage",
                            "UserControlPath property not found on WebPart");
                        return;
                    }

                    prop.SetValue(wp, userControlPath, null);


                    if(UserControlProperties != "")
                    {
                        System.Reflection.PropertyInfo propSettings = wpType.GetProperty("UserControlProperties");
                        if (propSettings == null)
                        {
                            Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                                "SideMenuListProvisioner.AddUserControlToPage",
                                "UserControlPath property not found on WebPart");
                            return;
                        }

                        propSettings.SetValue(wp, UserControlProperties, null);

                    }
                    wpManager.AddWebPart(wp, WP_ZONE_ID, 0);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SideMenuListProvisioner.AddUserControlToPage", ex.Message);
            }
        }

        // ==================================================================
        //  LIST / FIELD CREATION
        // ==================================================================

        private static SPList EnsureLevel1List(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(LIST_LEVEL1);
            if (list != null) return list;

            Guid listId = web.Lists.Add(LIST_LEVEL1, "Side menu - Level 1 items", SPListTemplateType.GenericList);
            list = web.Lists[listId];

            EnsureCommonFields(list);

            list.OnQuickLaunch = false;
            list.Update();

            return list;
        }

        private static void EnsureDefaultViewFields(SPList list, params string[] fieldNames)
        {
            if (list == null || fieldNames == null || fieldNames.Length == 0) return;
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool updated = false;
                foreach (string name in fieldNames)
                {
                    if (list.Fields.ContainsField(name) && !view.ViewFields.Exists(name))
                    {
                        view.ViewFields.Add(name);
                        updated = true;
                    }
                }

                if (updated)
                {
                    view.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucHelpSupport - EnsureDefaultViewFields", ex.Message);
            }
        }


        private static void EnsureLevel2List(SPWeb web, SPList level1List)
        {
            SPList list = web.Lists.TryGetList(LIST_LEVEL2);
            if (list != null) return;

            Guid listId = web.Lists.Add(LIST_LEVEL2, "Side menu - Level 2 items", SPListTemplateType.GenericList);
            list = web.Lists[listId];

            EnsureCommonFields(list);

            if (!list.Fields.ContainsField("Parent"))
            {
                string fieldName = list.Fields.AddLookup("Parent", level1List.ID, false);
                SPFieldLookup lookup = (SPFieldLookup)list.Fields.GetField(fieldName);
                lookup.LookupField = "Title";
                lookup.Update();
            }

            EnsureDefaultViewFields(list, "Parent");

            list.OnQuickLaunch = false;
            list.Update();
        }

        private static void EnsureCommonFields(SPList list)
        {
            if (!list.Fields.ContainsField("Title_EN"))
                list.Fields.Add("Title_EN", SPFieldType.Text, false);

            if (!list.Fields.ContainsField("URL"))
                list.Fields.Add("URL", SPFieldType.URL, false);

            if (!list.Fields.ContainsField("ItemOrder"))
                list.Fields.Add("ItemOrder", SPFieldType.Number, false);

            if (!list.Fields.ContainsField("Visibility"))
            {
                string fieldName = list.Fields.Add("Visibility", SPFieldType.Boolean, false);
                SPFieldBoolean fld = (SPFieldBoolean)list.Fields.GetField(fieldName);
                fld.DefaultValue = "1";
                fld.Update();
            }

            EnsureDefaultViewFields(list, "Title_EN", "URL", "ItemOrder", "Visibility");
        }

        private static string SafeField(SPListItem item, string fieldName)
        {
            try
            {
                if (item.Fields.ContainsField(fieldName) && item[fieldName] != null)
                    return Convert.ToString(item[fieldName]);
            }
            catch (Exception)
            {
                // field missing on this item
            }
            return string.Empty;
        }
    }
}