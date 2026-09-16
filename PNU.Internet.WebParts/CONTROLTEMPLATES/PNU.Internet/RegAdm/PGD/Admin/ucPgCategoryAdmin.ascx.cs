using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Admin
{
    public partial class ucPgCategoryAdmin : UserControl
    {
        private PgCategoryRepository _repository;

        protected void Page_Load(object sender, EventArgs e)
        {
            _repository = new PgCategoryRepository(SPContext.Current != null ? SPContext.Current.Web : null);

            // Verify permissions
            if (!CheckAdminPermissions())
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "alert alert-danger";
                litAlertMsg.Text = "عذراً، لا تملك الصلاحيات الكافية للوصول إلى لوحة إدارة البرامج.";
                this.Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                // Ensure lists exist
                try
                {
                    if (SPContext.Current != null && SPContext.Current.Web != null)
                    {
                        PgCategoryProvisioner.EnsureLists(SPContext.Current.Web);
                    }
                }
                catch { }

                BindGrid();
            }
        }

        private bool CheckAdminPermissions()
        {
            try
            {
                if (SPContext.Current == null || SPContext.Current.Web == null) return true;
                SPWeb web = SPContext.Current.Web;
                return web.UserIsSiteAdmin || web.DoesUserHavePermissions(SPBasePermissions.EditListItems);
            }
            catch
            {
                return true;
            }
        }

        private void ShowAlert(string message, bool isSuccess)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = isSuccess ? "alert alert-success alert-dismissible fade show mb-4" : "alert alert-danger alert-dismissible fade show mb-4";
            litAlertMsg.Text = string.Format("<i class='hgi hgi-stroke {0} me-2'></i>{1}", isSuccess ? "hgi-checkmark-circle-02" : "hgi-alert-circle", message);
        }

        private void BindGrid()
        {
            PgProgramCategory selectedCat = PgCategoryConstants.ParseCategory(ddlCategoryFilter.SelectedValue);
            var programs = _repository.GetPrograms(selectedCat, false);

            string keyword = txtSearchKeyword.Text.Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(keyword))
            {
                programs = programs.Where(p =>
                    (p.TitleAr != null && p.TitleAr.ToLowerInvariant().Contains(keyword)) ||
                    (p.TitleEn != null && p.TitleEn.ToLowerInvariant().Contains(keyword)) ||
                    (p.CollegeAr != null && p.CollegeAr.ToLowerInvariant().Contains(keyword)) ||
                    (p.DepartmentAr != null && p.DepartmentAr.ToLowerInvariant().Contains(keyword)) ||
                    (p.DegreeGroupTitleAr != null && p.DegreeGroupTitleAr.ToLowerInvariant().Contains(keyword)) ||
                    (p.DetailsUrl != null && p.DetailsUrl.ToLowerInvariant().Contains(keyword))
                ).ToList();
            }

            litProgramCount.Text = programs.Count.ToString();
            rptAdminPrograms.DataSource = programs;
            rptAdminPrograms.DataBind();
        }

        protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void txtSearchKeyword_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        #region Program Quick Form Events

        protected void btnOpenAddProgram_Click(object sender, EventArgs e)
        {
            pnlProgramForm.Visible = true;
            pnlProgramDetailsForm.Visible = false;
            pnlOverviewForm.Visible = false;
            litFormTitle.Text = "إضافة برنامج جديد";

            hfProgramId.Value = "0";
            txtFormTitleAr.Text = "";
            txtFormTitleEn.Text = "";
            txtFormCollegeAr.Text = "";
            txtFormCollegeEn.Text = "";
            txtFormDepartmentAr.Text = "";
            txtFormDepartmentEn.Text = "";
            txtFormDetailsUrl.Text = "";
            txtFormApplyUrl.Text = PgCategorySeedData.DefaultBannerUrl;
            txtFormItemOrder.Text = (rptAdminPrograms.Items.Count + 1).ToString();
            chkFormIsActive.Checked = true;

            if (ddlCategoryFilter.SelectedValue != "All")
            {
                ddlFormCategory.SelectedValue = ddlCategoryFilter.SelectedValue;
            }
        }

        protected void btnCancelProgramForm_Click(object sender, EventArgs e)
        {
            pnlProgramForm.Visible = false;
        }

        protected void btnSaveProgram_Click(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(hfProgramId.Value, out id);

            if (string.IsNullOrEmpty(txtFormTitleAr.Text.Trim()))
            {
                ShowAlert("يرجى إدخال اسم البرنامج بالعربي.", false);
                return;
            }

            int order = 1;
            int.TryParse(txtFormItemOrder.Text.Trim(), out order);

            string degreeGroupTitleAr = txtFormDegreeGroupTitleAr.Text.Trim();
            if (string.IsNullOrEmpty(degreeGroupTitleAr))
            {
                degreeGroupTitleAr = ddlFormDegreeGroupKey.SelectedItem.Text;
            }

            var model = new PgProgramItemModel
            {
                Id = id,
                TitleAr = txtFormTitleAr.Text.Trim(),
                TitleEn = txtFormTitleEn.Text.Trim(),
                CategoryKey = ddlFormCategory.SelectedValue,
                DegreeGroupKey = ddlFormDegreeGroupKey.SelectedValue,
                DegreeGroupTitleAr = degreeGroupTitleAr,
                DegreeGroupTitleEn = ddlFormDegreeGroupKey.SelectedValue,
                CollegeAr = txtFormCollegeAr.Text.Trim(),
                CollegeEn = txtFormCollegeEn.Text.Trim(),
                DepartmentAr = txtFormDepartmentAr.Text.Trim(),
                DepartmentEn = txtFormDepartmentEn.Text.Trim(),
                ScopeAr = ddlFormScope.SelectedValue,
                ScopeEn = ddlFormScope.SelectedValue.Contains("عالمي") ? "International" : "Local",
                DetailsUrl = txtFormDetailsUrl.Text.Trim(),
                ApplyUrl = txtFormApplyUrl.Text.Trim(),
                ItemOrder = order,
                IsActive = chkFormIsActive.Checked
            };

            bool success = _repository.SaveProgram(model);
            if (success)
            {
                pnlProgramForm.Visible = false;
                ShowAlert(id > 0 ? "تم تحديث البرنامج بنجاح." : "تمت إضافة البرنامج الجديد بنجاح.", true);
                BindGrid();
            }
            else
            {
                ShowAlert("حدث خطأ أثناء حفظ البرنامج. يرجى التأكد من الصلاحيات ووجود القائمة.", false);
            }
        }

        protected void rptAdminPrograms_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EditProgram")
            {
                int id = 0;
                int.TryParse(e.CommandArgument.ToString(), out id);
                if (id > 0)
                {
                    var prog = _repository.GetProgramById(id);
                    if (prog != null)
                    {
                        pnlProgramForm.Visible = true;
                        pnlProgramDetailsForm.Visible = false;
                        pnlOverviewForm.Visible = false;
                        litFormTitle.Text = "تعديل البرنامج: " + prog.TitleAr;

                        hfProgramId.Value = prog.Id.ToString();
                        txtFormTitleAr.Text = prog.TitleAr;
                        txtFormTitleEn.Text = prog.TitleEn;
                        txtFormCollegeAr.Text = prog.CollegeAr;
                        txtFormCollegeEn.Text = prog.CollegeEn;
                        txtFormDepartmentAr.Text = prog.DepartmentAr;
                        txtFormDepartmentEn.Text = prog.DepartmentEn;
                        txtFormDegreeGroupTitleAr.Text = prog.DegreeGroupTitleAr;
                        txtFormDetailsUrl.Text = prog.DetailsUrl;
                        txtFormApplyUrl.Text = prog.ApplyUrl;
                        txtFormItemOrder.Text = prog.ItemOrder.ToString();
                        chkFormIsActive.Checked = prog.IsActive;

                        if (ddlFormCategory.Items.FindByValue(prog.CategoryKey) != null)
                            ddlFormCategory.SelectedValue = prog.CategoryKey;

                        if (ddlFormDegreeGroupKey.Items.FindByValue(prog.DegreeGroupKey) != null)
                            ddlFormDegreeGroupKey.SelectedValue = prog.DegreeGroupKey;

                        if (ddlFormScope.Items.FindByValue(prog.ScopeAr) != null)
                            ddlFormScope.SelectedValue = prog.ScopeAr;
                    }
                }
            }
            else if (e.CommandName == "EditDetails")
            {
                string arg = e.CommandArgument.ToString();
                string[] parts = arg.Split('|');
                string detailsUrl = parts.Length > 0 ? parts[0] : "";
                string titleAr = parts.Length > 1 ? parts[1] : "";

                string progCode = PgProgramDetailsSeedData.ExtractCodeFromUrl(detailsUrl);
                if (string.IsNullOrEmpty(progCode)) progCode = "pro55";

                LoadProgramDetailsForm(progCode, titleAr, detailsUrl);
            }
            else if (e.CommandName == "DeleteProgram")
            {
                int id = 0;
                int.TryParse(e.CommandArgument.ToString(), out id);
                if (id > 0)
                {
                    bool deleted = _repository.DeleteProgram(id);
                    if (deleted)
                    {
                        ShowAlert("تم حذف البرنامج بنجاح.", true);
                        BindGrid();
                    }
                    else
                    {
                        ShowAlert("تعذر حذف البرنامج.", false);
                    }
                }
            }
        }

        #endregion

        #region Program Full Details Form Events

        private void LoadProgramDetailsForm(string progCode, string fallbackTitleAr, string fallbackUrl)
        {
            pnlProgramDetailsForm.Visible = true;
            pnlProgramForm.Visible = false;
            pnlOverviewForm.Visible = false;

            litDetailsFormTitle.Text = "تعديل تفاصيل البرنامج: " + (!string.IsNullOrEmpty(fallbackTitleAr) ? fallbackTitleAr : progCode);

            // Look up existing details
            var details = _repository.GetProgramDetails(progCode);
            if (details == null)
            {
                // Initialize default model
                details = new PgProgramDetailsModel
                {
                    ProgramCode = progCode,
                    TitleAr = fallbackTitleAr,
                    ApplyUrl = PgCategorySeedData.DefaultBannerUrl,
                    IsActive = true
                };
            }

            hfDetailsId.Value = details.Id.ToString();
            txtDetProgramCode.Text = details.ProgramCode;
            txtDetTitleAr.Text = details.TitleAr;
            txtDetTitleEn.Text = details.TitleEn;
            txtDetDegreeAr.Text = details.DegreeAr;
            txtDetDegreeEn.Text = details.DegreeEn;
            txtDetCollegeAr.Text = details.CollegeAr;
            txtDetCollegeEn.Text = details.CollegeEn;
            txtDetDepartmentAr.Text = details.DepartmentAr;
            txtDetDepartmentEn.Text = details.DepartmentEn;

            if (ddlDetCategory.Items.FindByValue(details.CategoryKey) != null)
                ddlDetCategory.SelectedValue = details.CategoryKey;

            if (ddlDetScope.Items.FindByValue(details.ScopeAr) != null)
                ddlDetScope.SelectedValue = details.ScopeAr;

            // Stats
            txtDetCreditHoursAr.Text = details.CreditHoursAr;
            txtDetCreditHoursEn.Text = details.CreditHoursEn;
            txtDetDurationAr.Text = details.DurationAr;
            txtDetDurationEn.Text = details.DurationEn;
            txtDetLanguageAr.Text = details.LanguageAr;
            txtDetLanguageEn.Text = details.LanguageEn;
            txtDetStudySystemAr.Text = details.StudySystemAr;
            txtDetStudySystemEn.Text = details.StudySystemEn;
            txtDetTuitionFeesAr.Text = details.TuitionFeesAr;
            txtDetTuitionFeesEn.Text = details.TuitionFeesEn;

            // Rich Content
            txtDetOverviewAr.Text = details.OverviewAr;
            txtDetOverviewEn.Text = details.OverviewEn;
            txtDetMissionAr.Text = details.MissionAr;
            txtDetMissionEn.Text = details.MissionEn;
            txtDetObjectivesAr.Text = details.ObjectivesAr;
            txtDetObjectivesEn.Text = details.ObjectivesEn;
            txtDetAdmissionRequirementsAr.Text = details.AdmissionRequirementsAr;
            txtDetAdmissionRequirementsEn.Text = details.AdmissionRequirementsEn;
            txtDetStudyPlanAr.Text = details.StudyPlanAr;
            txtDetStudyPlanEn.Text = details.StudyPlanEn;
            txtDetStudyPlanPdfUrl.Text = details.StudyPlanPdfUrl;
            txtDetCareerOpportunitiesAr.Text = details.CareerOpportunitiesAr;
            txtDetCareerOpportunitiesEn.Text = details.CareerOpportunitiesEn;

            // Links & Partner
            txtDetApplyUrl.Text = !string.IsNullOrEmpty(details.ApplyUrl) ? details.ApplyUrl : PgCategorySeedData.DefaultBannerUrl;
            txtDetContactEmail.Text = details.ContactEmail;
            txtDetPartnerUnivAr.Text = details.PartnerUniversityAr;
            txtDetPartnerUnivEn.Text = details.PartnerUniversityEn;
            txtDetPartnerUnivUrl.Text = details.PartnerUniversityUrl;
            chkDetIsActive.Checked = details.IsActive;
        }

        protected void btnCancelProgramDetails_Click(object sender, EventArgs e)
        {
            pnlProgramDetailsForm.Visible = false;
        }

        protected void btnSaveProgramDetails_Click(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(hfDetailsId.Value, out id);

            string progCode = txtDetProgramCode.Text.Trim();
            if (string.IsNullOrEmpty(progCode))
            {
                ShowAlert("يرجى إدخال رمز البرنامج (Program Code).", false);
                return;
            }

            if (string.IsNullOrEmpty(txtDetTitleAr.Text.Trim()))
            {
                ShowAlert("يرجى إدخال اسم البرنامج بالعربي.", false);
                return;
            }

            var model = new PgProgramDetailsModel
            {
                Id = id,
                ProgramCode = progCode,
                TitleAr = txtDetTitleAr.Text.Trim(),
                TitleEn = txtDetTitleEn.Text.Trim(),
                CategoryKey = ddlDetCategory.SelectedValue,
                DegreeAr = txtDetDegreeAr.Text.Trim(),
                DegreeEn = txtDetDegreeEn.Text.Trim(),
                CollegeAr = txtDetCollegeAr.Text.Trim(),
                CollegeEn = txtDetCollegeEn.Text.Trim(),
                DepartmentAr = txtDetDepartmentAr.Text.Trim(),
                DepartmentEn = txtDetDepartmentEn.Text.Trim(),
                ScopeAr = ddlDetScope.SelectedValue,
                ScopeEn = ddlDetScope.SelectedValue.Contains("عالمي") ? "International" : "Local",

                CreditHoursAr = txtDetCreditHoursAr.Text.Trim(),
                CreditHoursEn = txtDetCreditHoursEn.Text.Trim(),
                DurationAr = txtDetDurationAr.Text.Trim(),
                DurationEn = txtDetDurationEn.Text.Trim(),
                LanguageAr = txtDetLanguageAr.Text.Trim(),
                LanguageEn = txtDetLanguageEn.Text.Trim(),
                StudySystemAr = txtDetStudySystemAr.Text.Trim(),
                StudySystemEn = txtDetStudySystemEn.Text.Trim(),
                TuitionFeesAr = txtDetTuitionFeesAr.Text.Trim(),
                TuitionFeesEn = txtDetTuitionFeesEn.Text.Trim(),

                OverviewAr = txtDetOverviewAr.Text.Trim(),
                OverviewEn = txtDetOverviewEn.Text.Trim(),
                MissionAr = txtDetMissionAr.Text.Trim(),
                MissionEn = txtDetMissionEn.Text.Trim(),
                ObjectivesAr = txtDetObjectivesAr.Text.Trim(),
                ObjectivesEn = txtDetObjectivesEn.Text.Trim(),
                AdmissionRequirementsAr = txtDetAdmissionRequirementsAr.Text.Trim(),
                AdmissionRequirementsEn = txtDetAdmissionRequirementsEn.Text.Trim(),
                StudyPlanAr = txtDetStudyPlanAr.Text.Trim(),
                StudyPlanEn = txtDetStudyPlanEn.Text.Trim(),
                StudyPlanPdfUrl = txtDetStudyPlanPdfUrl.Text.Trim(),
                CareerOpportunitiesAr = txtDetCareerOpportunitiesAr.Text.Trim(),
                CareerOpportunitiesEn = txtDetCareerOpportunitiesEn.Text.Trim(),

                ApplyUrl = txtDetApplyUrl.Text.Trim(),
                ContactEmail = txtDetContactEmail.Text.Trim(),
                PartnerUniversityAr = txtDetPartnerUnivAr.Text.Trim(),
                PartnerUniversityEn = txtDetPartnerUnivEn.Text.Trim(),
                PartnerUniversityUrl = txtDetPartnerUnivUrl.Text.Trim(),
                IsActive = chkDetIsActive.Checked
            };

            bool success = _repository.SaveProgramDetails(model);
            if (success)
            {
                pnlProgramDetailsForm.Visible = false;
                ShowAlert("تم حفظ وتحديث تفاصيل البرنامج بنجاح.", true);
                BindGrid();
            }
            else
            {
                ShowAlert("حدث خطأ أثناء حفظ تفاصيل البرنامج. يرجى مراجعة الصلاحيات.", false);
            }
        }

        #endregion

        #region Category Overview Form Events

        protected void btnOpenEditOverview_Click(object sender, EventArgs e)
        {
            pnlOverviewForm.Visible = true;
            pnlProgramForm.Visible = false;
            pnlProgramDetailsForm.Visible = false;

            string selectedCat = ddlCategoryFilter.SelectedValue == "All" ? "NouraStaff" : ddlCategoryFilter.SelectedValue;
            if (ddlOverviewCategory.Items.FindByValue(selectedCat) != null)
            {
                ddlOverviewCategory.SelectedValue = selectedCat;
            }

            LoadOverviewFormForCategory(PgCategoryConstants.ParseCategory(ddlOverviewCategory.SelectedValue));
        }

        protected void ddlOverviewCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOverviewFormForCategory(PgCategoryConstants.ParseCategory(ddlOverviewCategory.SelectedValue));
        }

        private void LoadOverviewFormForCategory(PgProgramCategory cat)
        {
            var ov = _repository.GetOverview(cat);
            if (ov != null)
            {
                hfOverviewId.Value = ov.Id.ToString();
                txtHeroTagAr.Text = ov.HeroTagAr;
                txtHeroTagEn.Text = ov.HeroTagEn;
                txtHeroTitleAr.Text = ov.HeroTitleAr;
                txtHeroTitleEn.Text = ov.HeroTitleEn;
                txtHeroDescAr.Text = ov.HeroDescriptionAr;
                txtHeroDescEn.Text = ov.HeroDescriptionEn;

                txtBannerTitleAr.Text = ov.BannerTitleAr;
                txtBannerDescAr.Text = ov.BannerDescriptionAr;
                txtBannerBtnAr.Text = ov.BannerButtonTextAr;
                txtBannerUrl.Text = ov.BannerUrl;

                txtCard1TitleAr.Text = ov.Card1TitleAr;
                txtCard1DescAr.Text = ov.Card1DescAr;
                txtCard1Url.Text = ov.Card1Url;

                txtCard2TitleAr.Text = ov.Card2TitleAr;
                txtCard2DescAr.Text = ov.Card2DescAr;
                txtCard2Url.Text = ov.Card2Url;
            }
        }

        protected void btnCancelOverviewForm_Click(object sender, EventArgs e)
        {
            pnlOverviewForm.Visible = false;
        }

        protected void btnSaveOverview_Click(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(hfOverviewId.Value, out id);

            var model = new PgCategoryOverviewModel
            {
                Id = id,
                CategoryKey = ddlOverviewCategory.SelectedValue,
                HeroTagAr = txtHeroTagAr.Text.Trim(),
                HeroTagEn = txtHeroTagEn.Text.Trim(),
                HeroSubTagAr = "الدراسات العليا",
                HeroSubTagEn = "Postgraduate Studies",
                HeroTitleAr = txtHeroTitleAr.Text.Trim(),
                HeroTitleEn = txtHeroTitleEn.Text.Trim(),
                HeroDescriptionAr = txtHeroDescAr.Text.Trim(),
                HeroDescriptionEn = txtHeroDescEn.Text.Trim(),

                BannerTitleAr = txtBannerTitleAr.Text.Trim(),
                BannerTitleEn = txtBannerTitleAr.Text.Trim(),
                BannerDescriptionAr = txtBannerDescAr.Text.Trim(),
                BannerDescriptionEn = txtBannerDescAr.Text.Trim(),
                BannerButtonTextAr = txtBannerBtnAr.Text.Trim(),
                BannerButtonTextEn = "Apply via Admission Portal",
                BannerUrl = txtBannerUrl.Text.Trim(),

                Card1TitleAr = txtCard1TitleAr.Text.Trim(),
                Card1TitleEn = "Admission Requirements",
                Card1DescAr = txtCard1DescAr.Text.Trim(),
                Card1DescEn = "Admission criteria and requirements",
                Card1Url = txtCard1Url.Text.Trim(),

                Card2TitleAr = txtCard2TitleAr.Text.Trim(),
                Card2TitleEn = "Admission Schedule",
                Card2DescAr = txtCard2DescAr.Text.Trim(),
                Card2DescEn = "Admission dates and timeline",
                Card2Url = txtCard2Url.Text.Trim(),

                IsActive = true
            };

            bool success = _repository.SaveOverview(model);
            if (success)
            {
                pnlOverviewForm.Visible = false;
                ShowAlert("تم حفظ إعدادات ترويسة الفئة والبانر بنجاح.", true);
            }
            else
            {
                ShowAlert("حدث خطأ أثناء حفظ ترويسة الفئة.", false);
            }
        }

        #endregion

        #region Reseed Actions

        protected void btnReseedCategory_Click(object sender, EventArgs e)
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    PgProgramCategory cat = PgCategoryConstants.ParseCategory(ddlCategoryFilter.SelectedValue);
                    PgCategoryProvisioner.ReseedCategory(SPContext.Current.Web, cat);
                    ShowAlert(string.Format("تمت إعادة تعبئة بيانات وتفاصيل فئة ({0}) بنجاح.", PgCategoryConstants.GetCategoryDisplayNameAr(cat)), true);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء إعادة التعبئة: " + ex.Message, false);
            }
        }

        protected void btnReseedAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    PgCategoryProvisioner.ReseedCategory(SPContext.Current.Web, PgProgramCategory.All);
                    ShowAlert("تمت إعادة تعبئة جميع الفئات والبرامج وتفاصيلها الكاملة بنجاح وفقاً لأحدث البيانات.", true);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("حدث خطأ أثناء إعادة التعبئة: " + ex.Message, false);
            }
        }

        #endregion
    }
}
