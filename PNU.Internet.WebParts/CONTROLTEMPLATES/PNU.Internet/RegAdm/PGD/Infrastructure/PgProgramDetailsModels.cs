using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure
{
    public class PgProgramDetailsModel
    {
        public int Id { get; set; }
        public string ProgramCode { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string CategoryKey { get; set; }
        public string DegreeAr { get; set; }
        public string DegreeEn { get; set; }
        public string CollegeAr { get; set; }
        public string CollegeEn { get; set; }
        public string DepartmentAr { get; set; }
        public string DepartmentEn { get; set; }
        public string ScopeAr { get; set; }
        public string ScopeEn { get; set; }

        // Quick Meta Stats
        public string DurationAr { get; set; }
        public string DurationEn { get; set; }
        public string CreditHoursAr { get; set; }
        public string CreditHoursEn { get; set; }
        public string LanguageAr { get; set; }
        public string LanguageEn { get; set; }
        public string StudySystemAr { get; set; }
        public string StudySystemEn { get; set; }
        public string TuitionFeesAr { get; set; }
        public string TuitionFeesEn { get; set; }

        // Rich Sections
        public string OverviewAr { get; set; }
        public string OverviewEn { get; set; }
        public string MissionAr { get; set; }
        public string MissionEn { get; set; }
        public string ObjectivesAr { get; set; }
        public string ObjectivesEn { get; set; }
        public string AdmissionRequirementsAr { get; set; }
        public string AdmissionRequirementsEn { get; set; }
        public string StudyPlanAr { get; set; }
        public string StudyPlanEn { get; set; }
        public string StudyPlanPdfUrl { get; set; }
        public string CareerOpportunitiesAr { get; set; }
        public string CareerOpportunitiesEn { get; set; }

        // Links & Partner Info
        public string ApplyUrl { get; set; }
        public string PartnerUniversityAr { get; set; }
        public string PartnerUniversityEn { get; set; }
        public string PartnerUniversityUrl { get; set; }
        public string ContactEmail { get; set; }

        public bool IsActive { get; set; }

        public string GetTitle(bool isAr) { return isAr ? TitleAr : (string.IsNullOrEmpty(TitleEn) ? TitleAr : TitleEn); }
        public string GetDegree(bool isAr) { return isAr ? DegreeAr : (string.IsNullOrEmpty(DegreeEn) ? DegreeAr : DegreeEn); }
        public string GetCollege(bool isAr) { return isAr ? CollegeAr : (string.IsNullOrEmpty(CollegeEn) ? CollegeAr : CollegeEn); }
        public string GetDepartment(bool isAr) { return isAr ? DepartmentAr : (string.IsNullOrEmpty(DepartmentEn) ? DepartmentAr : DepartmentEn); }
        public string GetScope(bool isAr) { return isAr ? ScopeAr : (string.IsNullOrEmpty(ScopeEn) ? ScopeAr : ScopeEn); }
        public string GetDuration(bool isAr) { return isAr ? DurationAr : (string.IsNullOrEmpty(DurationEn) ? DurationAr : DurationEn); }
        public string GetCreditHours(bool isAr) { return isAr ? CreditHoursAr : (string.IsNullOrEmpty(CreditHoursEn) ? CreditHoursAr : CreditHoursEn); }
        public string GetLanguage(bool isAr) { return isAr ? LanguageAr : (string.IsNullOrEmpty(LanguageEn) ? LanguageAr : LanguageEn); }
        public string GetStudySystem(bool isAr) { return isAr ? StudySystemAr : (string.IsNullOrEmpty(StudySystemEn) ? StudySystemAr : StudySystemEn); }
        public string GetTuitionFees(bool isAr) { return isAr ? TuitionFeesAr : (string.IsNullOrEmpty(TuitionFeesEn) ? TuitionFeesAr : TuitionFeesEn); }
        public string GetOverview(bool isAr) { return isAr ? OverviewAr : (string.IsNullOrEmpty(OverviewEn) ? OverviewAr : OverviewEn); }
        public string GetMission(bool isAr) { return isAr ? MissionAr : (string.IsNullOrEmpty(MissionEn) ? MissionAr : MissionEn); }
        public string GetObjectives(bool isAr) { return isAr ? ObjectivesAr : (string.IsNullOrEmpty(ObjectivesEn) ? ObjectivesAr : ObjectivesEn); }
        public string GetAdmissionRequirements(bool isAr) { return isAr ? AdmissionRequirementsAr : (string.IsNullOrEmpty(AdmissionRequirementsEn) ? AdmissionRequirementsAr : AdmissionRequirementsEn); }
        public string GetStudyPlan(bool isAr) { return isAr ? StudyPlanAr : (string.IsNullOrEmpty(StudyPlanEn) ? StudyPlanAr : StudyPlanEn); }
        public string GetCareerOpportunities(bool isAr) { return isAr ? CareerOpportunitiesAr : (string.IsNullOrEmpty(CareerOpportunitiesEn) ? CareerOpportunitiesAr : CareerOpportunitiesEn); }
        public string GetPartnerUniversity(bool isAr) { return isAr ? PartnerUniversityAr : (string.IsNullOrEmpty(PartnerUniversityEn) ? PartnerUniversityAr : PartnerUniversityEn); }

        public bool IsInternational
        {
            get
            {
                if (string.IsNullOrEmpty(ScopeAr) && string.IsNullOrEmpty(ScopeEn)) return false;
                string s = (ScopeAr + " " + ScopeEn).ToLowerInvariant();
                return s.Contains("عالمي") || s.Contains("international") || s.Contains("شراكة");
            }
        }
    }
}
