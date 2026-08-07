using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>
    /// Main container for the Noura Students (طالبات نورة) page.
    /// Checks/provisions all supporting lists on every load, then hosts the eleven
    /// section controls. Each section can be switched off from the page layout, e.g.
    /// &lt;pnu:NouraStudents runat="server" ShowExperiences="False" /&gt;
    /// </summary>
    public partial class ucNouraStudents : UserControl
    {
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowNumbers { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowAwards { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowAcademicServices { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowQuickLinks { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowStudentServices { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowImportantDates { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowCampusLife { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowCareer { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowExperiences { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowFinancialSupport { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowContact { get; set; }

        /// <summary>Target of the "دليل الخدمات الأكاديمية" button.</summary>
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string AcademicGuideUrl { get; set; }

        /// <summary>Image shown in the Student Services block.</summary>
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string StudentServicesImageUrl { get; set; }

        /// <summary>Image shown beside the Campus Life cards.</summary>
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string CampusLifeImageUrl { get; set; }

        /// <summary>Set to "False" to skip the per-page-load list check once the site is live.</summary>
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string EnsureLists { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                // Runs on every page load: verifies all eleven lists exist on this web and
                // provisions any missing one (columns + default data) before binding.
                if (!IsFalse(EnsureLists))
                    NsListProvisioner.EnsureAllListsExist();

                // Set visibility in OnInit so hidden sections never query their lists.
                ucNumbers.Visible          = !IsFalse(ShowNumbers);
                ucAwards.Visible           = !IsFalse(ShowAwards);
                ucAcademicServices.Visible = !IsFalse(ShowAcademicServices);
                ucQuickLinks.Visible       = !IsFalse(ShowQuickLinks);
                ucStudentServices.Visible  = !IsFalse(ShowStudentServices);
                ucImportantDates.Visible   = !IsFalse(ShowImportantDates);
                ucCampusLife.Visible       = !IsFalse(ShowCampusLife);
                ucCareer.Visible           = !IsFalse(ShowCareer);
                ucExperiences.Visible      = !IsFalse(ShowExperiences);
                ucFinancialSupport.Visible = !IsFalse(ShowFinancialSupport);
                ucContact.Visible          = !IsFalse(ShowContact);

                ApplyChildProperties();
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNouraStudents.OnInit", ex);
            }
        }

        /// <summary>
        /// Visual Studio types Src-registered user controls as plain UserControl in the
        /// generated designer file, so cast before touching any custom property.
        /// </summary>
        private void ApplyChildProperties()
        {
            ucNsAcademicServices services = ucAcademicServices as ucNsAcademicServices;
            if (services != null && !string.IsNullOrEmpty(AcademicGuideUrl))
                services.GuideUrl = AcademicGuideUrl;

            ucNsStudentServices studentServices = ucStudentServices as ucNsStudentServices;
            if (studentServices != null && !string.IsNullOrEmpty(StudentServicesImageUrl))
                studentServices.ImageUrl = StudentServicesImageUrl;

            ucNsCampusLife campusLife = ucCampusLife as ucNsCampusLife;
            if (campusLife != null && !string.IsNullOrEmpty(CampusLifeImageUrl))
                campusLife.ImageUrl = CampusLifeImageUrl;
        }

        private static bool IsFalse(string value)
        {
            return "False".Equals(value, StringComparison.OrdinalIgnoreCase)
                || "0".Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
