using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Controls
{
    public partial class ucPgProgramDetails : UserControl
    {
        private PgCategoryRepository _repository;

        /// <summary>
        /// Explicit program code/key to display. If left blank, it will resolve dynamically from QueryString or URL.
        /// </summary>
        public string ProgramCode { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _repository = new PgCategoryRepository(SPContext.Current != null ? SPContext.Current.Web : null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderProgramDetails();
            }
        }

        private void RenderProgramDetails()
        {
            bool isAr = _repository != null ? _repository.IsArabic : true;

            // Set Direction & Lang
            litDir.Text = isAr ? "rtl" : "ltr";
            litLang.Text = isAr ? "ar" : "en";
            litBackArrow.Text = isAr ? "hgi-arrow-right-01" : "hgi-arrow-left-01";

            // Localized Static Labels
            litLblDegree.Text = isAr ? "الدرجة العلمية" : "Degree";
            litLblDuration.Text = isAr ? "المدة الزمنية" : "Duration";
            litLblCredits.Text = isAr ? "الساعات المعتمدة" : "Credit Hours";
            litLblLanguage.Text = isAr ? "لغة التدريس" : "Language";
            litLblSystem.Text = isAr ? "نظام الدراسة" : "Study System";
            litLblFees.Text = isAr ? "الرسوم الدراسية" : "Tuition Fees";

            litBannerTitle.Text = isAr ? "التقديم على البرنامج عبر بوابة القبول الموحدة (Banner SIS)" : "Apply via the Unified Admission Portal (Banner SIS)";
            litBannerDesc.Text = isAr ? "يمكنك التقديم مباشرة ومتابعة حالة طلب القبول إلكترونياً." : "Submit your application online and track your admission status directly.";
            litBtnApplyText.Text = isAr ? "التقديم عبر بوابة القبول" : "Apply Online";

            litLblMission.Text = isAr ? "رسالة البرنامج" : "Program Mission";
            litLblObjectives.Text = isAr ? "أهداف البرنامج" : "Program Objectives";
            litLblAdmission.Text = isAr ? "شروط ومتطلبات القبول" : "Admission Requirements";
            litLblStudyPlan.Text = isAr ? "الخطة الدراسية" : "Study Plan & Curriculum";
            litBtnDownloadText.Text = isAr ? "تحميل الخطة الدراسية (PDF)" : "Download Study Plan (PDF)";
            litLblCareers.Text = isAr ? "مجالات العمل والفرص المهنية" : "Career Opportunities";
            litBackText.Text = isAr ? "العودة إلى دليل البرامج" : "Back to Programs Directory";
            litContactText.Text = isAr ? "تواصل مع عمادة الدراسات العليا" : "Contact Deanship of Graduate Studies";

            litUnivName.Text = isAr ? "جامعة الأميرة نورة بنت عبدالرحمن" : "Princess Nourah bint Abdulrahman University";

            // Resolve target Program Code
            string targetCode = ResolveProgramCode();

            // Fetch Program Details Entity
            PgProgramDetailsModel details = _repository != null ? _repository.GetProgramDetails(targetCode) : null;

            if (details == null)
            {
                // Fallback attempt with seed data
                var seeds = PgProgramDetailsSeedData.GetSeedDetails();
                details = seeds.Find(s => s.ProgramCode.Equals(targetCode, StringComparison.OrdinalIgnoreCase)) ?? seeds[0];
            }

            // Populate Badges & Headers
            litCategoryBadge.Text = GetCategoryDisplayName(details.CategoryKey, isAr);
            litCollegeBadge.Text = HttpUtility.HtmlEncode(details.GetCollege(isAr));
            litDepartmentBadge.Text = HttpUtility.HtmlEncode(details.GetDepartment(isAr));
            
            string scopeText = details.GetScope(isAr);
            if (!string.IsNullOrEmpty(scopeText))
            {
                pnlScopeBadge.Visible = true;
                litScopeBadge.Text = HttpUtility.HtmlEncode(scopeText);
                if (details.IsInternational)
                {
                    litScopeStyle.Text = "background-color: #fef3c7; color: #92400e; border: 1px solid #fde68a;";
                }
                else
                {
                    litScopeStyle.Text = "background-color: #f3f4f6; color: #374151; border: 1px solid #e5e7eb;";
                }
            }
            else
            {
                pnlScopeBadge.Visible = false;
            }

            // Main Title & Overview
            litProgramTitle.Text = HttpUtility.HtmlEncode(details.GetTitle(isAr));
            string overview = details.GetOverview(isAr);
            litProgramOverviewLead.Text = !string.IsNullOrEmpty(overview) ? overview : (isAr ? "برنامج دراسات عليا متميز تقدمه جامعة الأميرة نورة بنت عبدالرحمن لإعداد كفاءات علمية ومهنية متخصصة." : "A specialized graduate program offered by Princess Nourah bint Abdulrahman University.");

            // Quick Stats
            litDegree.Text = HttpUtility.HtmlEncode(details.GetDegree(isAr));
            litDuration.Text = HttpUtility.HtmlEncode(!string.IsNullOrEmpty(details.GetDuration(isAr)) ? details.GetDuration(isAr) : (isAr ? "سنتان (4 فصول)" : "2 Years (4 Semesters)"));
            litCreditHours.Text = HttpUtility.HtmlEncode(!string.IsNullOrEmpty(details.GetCreditHours(isAr)) ? details.GetCreditHours(isAr) : (isAr ? "36-42 ساعة" : "36-42 Credits"));
            litLanguage.Text = HttpUtility.HtmlEncode(!string.IsNullOrEmpty(details.GetLanguage(isAr)) ? details.GetLanguage(isAr) : (isAr ? "العربية" : "Arabic"));
            litStudySystem.Text = HttpUtility.HtmlEncode(!string.IsNullOrEmpty(details.GetStudySystem(isAr)) ? details.GetStudySystem(isAr) : (isAr ? "مقررات ورسالة" : "Courses & Thesis"));
            litTuitionFees.Text = HttpUtility.HtmlEncode(!string.IsNullOrEmpty(details.GetTuitionFees(isAr)) ? details.GetTuitionFees(isAr) : (isAr ? "مجاني / مدفوع" : "Standard Tuition"));

            // Apply Button Link
            string applyUrl = !string.IsNullOrEmpty(details.ApplyUrl) ? details.ApplyUrl : "https://banner-ssb.pnu.edu.sa/PROD_ar/bwskalog.P_DispLoginNon";
            btnApply.HRef = applyUrl;

            // International Partnership Section
            if (details.IsInternational || !string.IsNullOrEmpty(details.GetPartnerUniversity(isAr)))
            {
                pnlPartner.Visible = true;
                litPartnerBadge.Text = isAr ? "شراكة دولية معتمدة" : "Accredited International Partnership";
                litPartnerTitle.Text = details.GetPartnerUniversity(isAr);
                litPartnerBtnText.Text = isAr ? "زيارة موقع الجامعة الشريكة" : "Visit Partner University Website";
                lnkPartner.HRef = !string.IsNullOrEmpty(details.PartnerUniversityUrl) ? details.PartnerUniversityUrl : "#";
            }
            else
            {
                pnlPartner.Visible = false;
            }

            // Mission Section
            string mission = details.GetMission(isAr);
            if (!string.IsNullOrEmpty(mission))
            {
                divMission.Visible = true;
                litMission.Text = FormatRichContent(mission);
            }
            else
            {
                divMission.Visible = false;
            }

            // Objectives Section
            string objectives = details.GetObjectives(isAr);
            if (!string.IsNullOrEmpty(objectives))
            {
                divObjectives.Visible = true;
                litObjectives.Text = FormatRichContent(objectives);
            }
            else
            {
                divObjectives.Visible = false;
            }

            // Admission Requirements Section
            string requirements = details.GetAdmissionRequirements(isAr);
            if (!string.IsNullOrEmpty(requirements))
            {
                divAdmission.Visible = true;
                litAdmissionRequirements.Text = FormatRichContent(requirements);
            }
            else
            {
                divAdmission.Visible = false;
            }

            // Study Plan Section
            string studyPlan = details.GetStudyPlan(isAr);
            if (!string.IsNullOrEmpty(studyPlan))
            {
                divStudyPlan.Visible = true;
                litStudyPlan.Text = FormatRichContent(studyPlan);
            }
            else
            {
                divStudyPlan.Visible = false;
            }

            // Study Plan PDF Download Link
            if (!string.IsNullOrEmpty(details.StudyPlanPdfUrl))
            {
                btnDownloadPlan.Visible = true;
                btnDownloadPlan.HRef = details.StudyPlanPdfUrl;
            }
            else
            {
                btnDownloadPlan.Visible = false;
            }

            // Career Opportunities Section
            string careers = details.GetCareerOpportunities(isAr);
            if (!string.IsNullOrEmpty(careers))
            {
                divCareers.Visible = true;
                litCareerOpportunities.Text = FormatRichContent(careers);
            }
            else
            {
                divCareers.Visible = false;
            }

            // Back Link
            lnkBack.HRef = GetCategoryListingUrl(details.CategoryKey, isAr);
        }

        private string FormatRichContent(string content)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;

            string trimmed = content.Trim();
            // If it already contains structured HTML tags, return as-is
            if (trimmed.Contains("<") && trimmed.Contains(">"))
            {
                return trimmed;
            }

            // Convert plain text with newlines or bullets into clean structured cards
            string[] lines = trimmed.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 1)
            {
                return string.Format("<p class='text-muted mb-0' style='line-height: 1.8;'>{0}</p>", HttpUtility.HtmlEncode(trimmed));
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<div class='d-flex flex-column gap-2'>");
            foreach (string line in lines)
            {
                string l = line.Trim();
                if (string.IsNullOrEmpty(l)) continue;

                if (l.StartsWith("-") || l.StartsWith("•") || l.StartsWith("*"))
                {
                    l = l.Substring(1).Trim();
                }

                sb.Append("<div class='p-3 bg-light rounded-3 border d-flex align-items-start gap-2'>");
                sb.Append("<i class='hgi hgi-stroke hgi-checkmark-circle-02 text-success mt-1 flex-shrink-0'></i>");
                sb.AppendFormat("<span class='text-dark small fw-medium'>{0}</span>", HttpUtility.HtmlEncode(l));
                sb.Append("</div>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        private string ResolveProgramCode()
        {
            if (!string.IsNullOrEmpty(ProgramCode))
            {
                return PgProgramDetailsSeedData.ExtractCodeFromUrl(ProgramCode);
            }

            // QueryString checks
            string[] queryKeys = new string[] { "ProgramCode", "ProgramKey", "ProgramId", "Prog", "Code", "p", "Program", "id", "ID" };
            foreach (string qk in queryKeys)
            {
                string val = Request.QueryString[qk];
                if (!string.IsNullOrEmpty(val))
                {
                    return PgProgramDetailsSeedData.ExtractCodeFromUrl(val.Trim());
                }
            }

            // Current URL filename check (e.g. /pro55.aspx -> pro55)
            try
            {
                string urlPath = Request.Url.AbsolutePath;
                string fileName = Path.GetFileNameWithoutExtension(urlPath);
                if (!string.IsNullOrEmpty(fileName))
                {
                    string clean = PgProgramDetailsSeedData.ExtractCodeFromUrl(fileName);
                    string[] genericPages = new string[] { "pnusliderwithoutimage", "default", "home", "index", "page", "pages", "pgdetails", "programdetails", "program-details", "details" };
                    if (!System.Array.Exists(genericPages, g => g.Equals(clean, StringComparison.OrdinalIgnoreCase)))
                    {
                        return clean;
                    }
                }
            }
            catch { }

            return "pro55";
        }

        private string GetCategoryDisplayName(string categoryKey, bool isAr)
        {
            if (string.IsNullOrEmpty(categoryKey)) return isAr ? "الدراسات العليا" : "Postgraduate Programs";

            PgProgramCategory cat = PgCategoryConstants.ParseCategory(categoryKey);
            return isAr ? PgCategoryConstants.GetCategoryDisplayNameAr(cat) : PgCategoryConstants.GetCategoryDisplayNameEn(cat);
        }

        private string GetCategoryListingUrl(string categoryKey, bool isAr)
        {
            string langPrefix = isAr ? "/ar" : "/en";
            string baseFolder = langPrefix + "/RegAdm/PGD/Pages/";

            if (string.IsNullOrEmpty(categoryKey)) return baseFolder + "pnur.aspx";

            PgProgramCategory cat = PgCategoryConstants.ParseCategory(categoryKey);
            switch (cat)
            {
                case PgProgramCategory.NouraStaff:
                    return baseFolder + "pnur.aspx";
                case PgProgramCategory.Humanitarian:
                    return baseFolder + "HumanitarianPror.aspx";
                case PgProgramCategory.Interdisciplinary:
                    return baseFolder + "Interfaceprogr.aspx";
                case PgProgramCategory.Scientific:
                    return baseFolder + "SciencesProgr.aspx";
                case PgProgramCategory.Health:
                    return baseFolder + "healthprogr1.aspx";
                default:
                    return baseFolder + "pnur.aspx";
            }
        }
    }
}
