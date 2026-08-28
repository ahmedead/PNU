using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ContentAdmin
{
    /// <summary>
    /// Definition of an Admin Module registered in the Master Content Admin Hub.
    /// </summary>
    public class AdminModuleItem
    {
        public string Key { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
        public string ControlPath { get; set; }
        public string DefaultTargetWeb { get; set; }
        public string Description { get; set; }
        public string ProvisionerTypeName { get; set; }
        public Action ProvisionAction { get; set; }
    }

    /// <summary>
    /// Central catalog of all administrative modules across the PNU portal.
    /// Queries the dynamic ContentAdminModules SharePoint list with fallback to built-in definitions.
    /// </summary>
    public static class AdminModuleRegistry
    {
        /// <summary>
        /// Reads registered admin modules from the ContentAdminModules SharePoint list on the target admin web.
        /// </summary>
        public static List<AdminModuleItem> GetModules(string adminWebPath = ContentAdminListProvisioner.DEFAULT_ADMIN_WEB)
        {
            return ContentAdminListProvisioner.GetRegisteredModules(adminWebPath);
        }

        /// <summary>
        /// Built-in fallback catalog of all core portal admin controls.
        /// </summary>
        public static List<AdminModuleItem> GetDefaultBuiltInModules()
        {
            return new List<AdminModuleItem>
            {
                new AdminModuleItem
                {
                    Key = "AboutPnu",
                    TitleAr = "إدارة محتوى عن الجامعة",
                    TitleEn = "About PNU Content",
                    Category = "المحتوى الرئيسي",
                    Icon = "bi-bank",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/DGANewDesign/Admin/ucAboutPnuAdmin.ascx",
                    DefaultTargetWeb = "/ar/AboutUniversity",
                    Description = "إدارة الأقسام والنبذة والمجالس والبيانات الإحصائية الخاصة بصفحة عن الجامعة.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes.AboutPnuListProvisioner",
                    ProvisionAction = () =>
                    {
                        DGANewDesign.Classes.AboutPnuListProvisioner.EnsureAllListsExist();
                    }
                },
                new AdminModuleItem
                {
                    Key = "ProfessionalCertifications",
                    TitleAr = "الشهادات الاحترافية",
                    TitleEn = "Professional Certifications",
                    Category = "البرامج والشهادات",
                    Icon = "bi-award",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/ProfessionalCertifications/Admin/ucPcAdmin.ascx",
                    DefaultTargetWeb = "/ar/ProfessionalCertifications",
                    Description = "إدارة بيانات ومجالات وشروط ومسارات الشهادات الاحترافية.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls.PcListProvisioner",
                    ProvisionAction = () =>
                    {
                        ProfessionalCertifications.Controls.PcListProvisioner.EnsureAllListsExist();
                    }
                },
                new AdminModuleItem
                {
                    Key = "Tawasul",
                    TitleAr = "خدمة تواصل",
                    TitleEn = "Tawasul Service",
                    Category = "الخدمات والتواصل",
                    Icon = "bi-chat-dots",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/Tawasul/Admin/ucTwAdmin.ascx",
                    DefaultTargetWeb = "/ar/Tawasul",
                    Description = "إدارة قنوات التواصل ونماذج الاستفسارات والأسئلة الشائعة.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls.TwListProvisioner",
                    ProvisionAction = () =>
                    {
                        Tawasul.Controls.TwListProvisioner.EnsureAllListsExist();
                    }
                },
                new AdminModuleItem
                {
                    Key = "NouraStudents",
                    TitleAr = "طالبات نورة",
                    TitleEn = "Noura Students",
                    Category = "شؤون الطالبات",
                    Icon = "bi-person-hearts",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/NouraStudents/Admin/ucNsAdmin.ascx",
                    DefaultTargetWeb = "/ar/NouraStudents",
                    Description = "إدارة خدمات وأدلة وأنشطة طالبات جامعة الأميرة نورة.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls.NsListProvisioner",
                    ProvisionAction = () =>
                    {
                        NouraStudents.Controls.NsListProvisioner.EnsureAllListsExist();
                    }
                },
                new AdminModuleItem
                {
                    Key = "CentralLibrary",
                    TitleAr = "المكتبة المركزية",
                    TitleEn = "Central Library",
                    Category = "المرافق والخدمات",
                    Icon = "bi-book",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/CentralLibrary/Admin/ucClAdmin.ascx",
                    DefaultTargetWeb = "/ar/CentralLibrary",
                    Description = "إدارة خدمات وقواعد بيانات وأقسام المكتبة المركزية.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Infrastructure.ClListProvisioner",
                    ProvisionAction = () =>
                    {
                        ClListProvisioner.EnsureAllListsExist();
                    }
                },
                new AdminModuleItem
                {
                    Key = "AcademicCalendar",
                    TitleAr = "التقويم الأكاديمي",
                    TitleEn = "Academic Calendar",
                    Category = "المحتوى الرئيسي",
                    Icon = "bi-calendar-event",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/AcademicCalendar/Admin/ucAcAdmin.ascx",
                    DefaultTargetWeb = "/ar/AcademicCalendar",
                    Description = "إدارة الفصول الدراسية والمواعيد والفعاليات الأكاديمية.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Infrastructure.AcListProvisioner",
                    ProvisionAction = () =>
                    {
                        AcListProvisioner.EnsureAllListsExist();
                    }
                },
                new AdminModuleItem
                {
                    Key = "InternationalStudents",
                    TitleAr = "الطلبة الدوليين",
                    TitleEn = "International Students",
                    Category = "شؤون الطلاب",
                    Icon = "bi-globe-americas",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/International/Admin/ucIntlAdmin.ascx",
                    DefaultTargetWeb = "/ar/International",
                    Description = "إدارة خدمات وبرامج وإعلانات الطلبة الدوليين.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Infrastructure.InternationalListProvisioner",
                    ProvisionAction = () =>
                    {
                        InternationalListProvisioner.EnsureAllListsExist();
                    }
                },
                //new AdminModuleItem
                //{
                //    Key = "SideMenu",
                //    TitleAr = "إدارة القوائم الجانبية",
                //    TitleEn = "Side Menu Admin",
                //    Category = "إعدادات الهيكل",
                //    Icon = "bi-layout-sidebar",
                //    ControlPath = "~/_controltemplates/15/PNU.Internet/SideMenu/ucSideMenuListAdmin.ascx",
                //    DefaultTargetWeb = "/ar",
                //    Description = "إدارة مستويات القوائم الجانبية وروابط التنقل بالمواقع الفرعية.",
                //    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu.SideMenuListProvisioner",
                //    ProvisionAction = () =>
                //    {
                //        SideMenu.SideMenuListProvisioner.EnsureLists();
                //    }
                //},
                new AdminModuleItem
                {
                    Key = "FacultiesContent",
                    TitleAr = "إدارة محتوى الكليات",
                    TitleEn = "Faculties Content",
                    Category = "الكليات",
                    Icon = "bi-buildings",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/Faculties/DGA/Admin/ucContentAdmin.ascx",
                    DefaultTargetWeb = "/ar",
                    Description = "إدارة المحتوى المخصص لصفحات الكليات والأقسام التابعة لها.",
                    ProvisionerTypeName = "",
                    ProvisionAction = null
                },
                new AdminModuleItem
                {
                    Key = "Herbarium",
                    TitleAr = "إدارة محتوى المعشبة",
                    TitleEn = "Herbarium Content",
                    Category = "الكليات",
                    Icon = "bi-flower1",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/Faculties/DGA/Herbarium/Admin/ucHerbariumAdmin.ascx",
                    DefaultTargetWeb = "/ar",
                    Description = "إدارة عينات ومعلومات معشبة كلية العلوم.",
                    ProvisionerTypeName = "PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.Infrastructure.HerbariumListProvisioner",
                    ProvisionAction = () =>
                    {
                        HerbariumListProvisioner.EnsureAllListsExist();
                    }
                },
                new AdminModuleItem
                {
                    Key = "OrgStructure",
                    TitleAr = "الهيكل التنظيمي",
                    TitleEn = "Org Structure",
                    Category = "المحتوى الرئيسي",
                    Icon = "bi-diagram-2",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/About/OrgStructure/ucOrgStructureAdmin.ascx",
                    DefaultTargetWeb = "/ar/AboutUniversity",
                    Description = "إدارة بيانات قيادات ووحدات الهيكل التنظيمي.",
                    ProvisionerTypeName = "",
                    ProvisionAction = null
                },
                new AdminModuleItem
                {
                    Key = "PresidentOffice",
                    TitleAr = "مكتب رئيس الجامعة",
                    TitleEn = "President Office",
                    Category = "المحتوى الرئيسي",
                    Icon = "bi-person-badge",
                    ControlPath = "~/_controltemplates/15/PNU.Internet/AboutUniversity/UniversityPresidentOffice/Admin/ucUpoAdmin.ascx",
                    DefaultTargetWeb = "/ar/AboutUniversity",
                    Description = "إدارة محتوى وخدمات مكتب رئيس الجامعة.",
                    ProvisionerTypeName = "",
                    ProvisionAction = null
                }
            };
        }
    }
}
