using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Assembles one audience page of the postgraduate academic kit.
    /// Set <see cref="Phase"/> to Applicants, Enrolled or Graduates; the sections that do not
    /// belong to that audience are switched off, and the rest render in the order of the design.
    /// Individual sections can also be switched off from the PgSections list.
    /// </summary>
    public partial class ucPostgraduateKit : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string Phase { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowHeaderSection { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowStyles { get; set; }

        public ucPostgraduateKit()
        {
            Phase = PgPhases.Applicants;
            ShowHeaderSection = true;
            ShowStyles = true;
        }

        private string EffectivePhase
        {
            get { return string.IsNullOrEmpty(Phase) ? PgPhases.Applicants : Phase; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                PgListProvisioner.EnsureAllListsExist();

                string phase = EffectivePhase;
                bool applicants = phase.Equals(PgPhases.Applicants, StringComparison.OrdinalIgnoreCase);
                bool enrolled = phase.Equals(PgPhases.Enrolled, StringComparison.OrdinalIgnoreCase);
                bool graduates = phase.Equals(PgPhases.Graduates, StringComparison.OrdinalIgnoreCase);

                ApplyPhase(ucHeader, phase);
                ApplyPhase(ucIntro, phase);
                ApplyPhase(ucJourney, phase);
                ApplyPhase(ucPrograms, phase);
                ApplyPhase(ucRegulations, phase);
                ApplyPhase(ucGraduateIdentity, phase);
                ApplyPhase(ucFinancialRules, phase);
                ApplyPhase(ucGradRequirements, phase);
                ApplyPhase(ucEnglishCourse, phase);
                ApplyPhase(ucProcedures, phase);
                ApplyPhase(ucFaq, phase);
                ApplyPhase(ucPublishIncentive, phase);
                ApplyPhase(ucGuidesEarly, phase);
                ApplyPhase(ucDiscounts, phase);
                ApplyPhase(ucGuidesLate, phase);
                ApplyPhase(ucServices, phase);
                ApplyPhase(ucHousing, phase);
                ApplyPhase(ucContact, phase);
                ApplyPhase(ucFooterNote, phase);

                // The guides axis sits before the fee table for applicants and after it for
                // enrolled students, so the design uses two slots and only one is ever shown.
                Show(phGuidesEarly, applicants);
                Show(phGuidesLate, enrolled);

                Show(phHeader, ShowHeaderSection);
                Show(phIntro, true);
                Show(phJourney, true);
                Show(phFaq, true);
                Show(phFooterNote, true);

                Show(phPrograms, applicants);
                Show(phEnglishCourse, applicants);

                Show(phRegulations, enrolled);
                Show(phGraduateIdentity, enrolled);
                Show(phFinancialRules, enrolled);
                Show(phPublishIncentive, enrolled);

                Show(phGradRequirements, graduates);

                Show(phProcedures, enrolled || graduates);
                Show(phDiscounts, applicants || enrolled);
                Show(phServices, applicants || enrolled);
                Show(phHousing, applicants || enrolled);
                Show(phContact, applicants || enrolled);

                if (ucStyles != null) ucStyles.Visible = ShowStyles;
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPostgraduateKit.OnInit", ex);
            }
        }

        private static void ApplyPhase(PgSectionBase section, string phase)
        {
            if (section != null) section.Phase = phase;
        }

        private static void Show(Control placeHolder, bool visible)
        {
            if (placeHolder != null) placeHolder.Visible = visible;
        }
    }
}
