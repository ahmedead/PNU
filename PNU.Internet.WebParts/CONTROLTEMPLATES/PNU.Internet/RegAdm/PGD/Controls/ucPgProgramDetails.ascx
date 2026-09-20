<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgProgramDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Controls.ucPgProgramDetails" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <section class="py-5" data-aos="fade-up" aria-labelledby="program-details-title">
        <div class="container">
            <div dir="<asp:Literal ID="litDir" runat="server" />" lang="<asp:Literal ID="litLang" runat="server" />" class="dga-content-wrapper d-flex flex-column gap-4 text-start">
                
                <!-- 1. Hero Overview Section -->
                <div class="card border-0 rounded-4 p-4 p-lg-5" style="border: 1px solid #d1e2db !important; background-color: #ffffff;">
                    <div class="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3">
                        <div class="d-flex align-items-center flex-wrap gap-2">
                            <span class="dga-tag px-3 py-1 rounded-pill small fw-medium" style="background-color: #e6f4ea; color: #006848; border: 1px solid #b7e1cd;">
                                <i class="hgi hgi-stroke hgi-graduation-scroll me-1"></i>
                                <asp:Literal ID="litCategoryBadge" runat="server" />
                            </span>
                            <span class="dga-tag px-3 py-1 rounded-pill small fw-medium" style="background-color: #f3f4f6; color: #374151; border: 1px solid #e5e7eb;">
                                <asp:Literal ID="litCollegeBadge" runat="server" />
                            </span>
                            <span class="dga-tag px-3 py-1 rounded-pill small fw-medium" style="background-color: #f3f4f6; color: #374151; border: 1px solid #e5e7eb;">
                                <asp:Literal ID="litDepartmentBadge" runat="server" />
                            </span>
                            <asp:Panel ID="pnlScopeBadge" runat="server" CssClass="d-inline">
                                <span class='dga-tag px-3 py-1 rounded-pill small fw-medium' style='<asp:Literal ID="litScopeStyle" runat="server" />'>
                                    <asp:Literal ID="litScopeBadge" runat="server" />
                                </span>
                            </asp:Panel>
                        </div>
                        <div class="d-flex align-items-center gap-2 text-secondary small">
                            <i class="hgi hgi-stroke hgi-building-03 text-success"></i>
                            <span><asp:Literal ID="litUnivName" runat="server" /></span>
                        </div>
                    </div>

                    <h1 id="program-details-title" class="h2 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                        <i class="hgi hgi-stroke hgi-mortarboard-02 flex-shrink-0" style="color: #c59b27;"></i>
                        <span><asp:Literal ID="litProgramTitle" runat="server" /></span>
                    </h1>

                    <p class="lead mb-4" style="color: #4b5563; font-size: 1.1rem; line-height: 1.8;">
                        <asp:Literal ID="litProgramOverviewLead" runat="server" />
                    </p>

                    <!-- Quick Meta Highlights -->
                    <div class="row g-3 pt-3 border-top">
                        <div class="col-6 col-md-4 col-lg-2">
                            <div class="p-3 bg-light rounded-3 text-start h-100">
                                <span class="d-block text-muted small mb-1"><i class="hgi hgi-stroke hgi-award-02 text-success me-1"></i><asp:Literal ID="litLblDegree" runat="server" Text="الدرجة" /></span>
                                <strong class="d-block text-dark small"><asp:Literal ID="litDegree" runat="server" /></strong>
                            </div>
                        </div>
                        <div class="col-6 col-md-4 col-lg-2">
                            <div class="p-3 bg-light rounded-3 text-start h-100">
                                <span class="d-block text-muted small mb-1"><i class="hgi hgi-stroke hgi-calendar-03 text-success me-1"></i><asp:Literal ID="litLblDuration" runat="server" Text="المدة" /></span>
                                <strong class="d-block text-dark small"><asp:Literal ID="litDuration" runat="server" /></strong>
                            </div>
                        </div>
                        <div class="col-6 col-md-4 col-lg-2">
                            <div class="p-3 bg-light rounded-3 text-start h-100">
                                <span class="d-block text-muted small mb-1"><i class="hgi hgi-stroke hgi-clock-01 text-success me-1"></i><asp:Literal ID="litLblCredits" runat="server" Text="الساعات" /></span>
                                <strong class="d-block text-dark small"><asp:Literal ID="litCreditHours" runat="server" /></strong>
                            </div>
                        </div>
                        <div class="col-6 col-md-4 col-lg-2">
                            <div class="p-3 bg-light rounded-3 text-start h-100">
                                <span class="d-block text-muted small mb-1"><i class="hgi hgi-stroke hgi-globe-02 text-success me-1"></i><asp:Literal ID="litLblLanguage" runat="server" Text="لغة الدراسة" /></span>
                                <strong class="d-block text-dark small"><asp:Literal ID="litLanguage" runat="server" /></strong>
                            </div>
                        </div>
                        <div class="col-6 col-md-4 col-lg-2">
                            <div class="p-3 bg-light rounded-3 text-start h-100">
                                <span class="d-block text-muted small mb-1"><i class="hgi hgi-stroke hgi-book-open-01 text-success me-1"></i><asp:Literal ID="litLblSystem" runat="server" Text="نظام الدراسة" /></span>
                                <strong class="d-block text-dark small"><asp:Literal ID="litStudySystem" runat="server" /></strong>
                            </div>
                        </div>
                        <div class="col-6 col-md-4 col-lg-2">
                            <div class="p-3 bg-light rounded-3 text-start h-100">
                                <span class="d-block text-muted small mb-1"><i class="hgi hgi-stroke hgi-money-bag-02 text-success me-1"></i><asp:Literal ID="litLblFees" runat="server" Text="الرسوم" /></span>
                                <strong class="d-block text-dark small"><asp:Literal ID="litTuitionFees" runat="server" /></strong>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- 2. Electronic Admission Banner -->
                <div class="card border rounded-3 p-4 bg-white shadow-sm" style="border-right: 4px solid #006848 !important;">
                    <div class="d-flex flex-column flex-lg-row justify-content-between align-items-start align-items-lg-center gap-3">
                        <div class="d-flex align-items-start gap-3">
                            <div class="p-3 rounded-3 flex-shrink-0 d-flex align-items-center justify-content-center" style="background-color: #e6f4ea; color: #006848; width: 52px; height: 52px;">
                                <i class="hgi hgi-stroke hgi-login-01 fs-3"></i>
                            </div>
                            <div>
                                <h2 class="h5 fw-bold text-dark mb-1"><asp:Literal ID="litBannerTitle" runat="server" Text="التقديم على البرنامج عبر بوابة القبول الموحدة (Banner SIS)" /></h2>
                                <p class="text-muted small mb-0"><asp:Literal ID="litBannerDesc" runat="server" Text="يمكنك التقديم مباشرة ومتابعة حالة طلب القبول إلكترونياً." /></p>
                            </div>
                        </div>
                        <a id="btnApply" runat="server" class="btn px-4 py-2 text-white fw-medium d-inline-flex align-items-center gap-2 flex-shrink-0 shadow-sm" target="_blank" rel="noopener noreferrer" style="background-color: #006848; border-color: #006848; border-radius: 8px; min-height: 44px;">
                            <span><asp:Literal ID="litBtnApplyText" runat="server" Text="التقديم عبر بوابة القبول" /></span>
                            <i class="hgi hgi-stroke hgi-arrow-up-right-01"></i>
                        </a>
                    </div>
                </div>

                <!-- 3. International Partnership Card (If applicable) -->
                <asp:Panel ID="pnlPartner" runat="server" Visible="false">
                    <div class="card border-0 rounded-4 p-4 text-white" style="background: linear-gradient(135deg, #0f4b31 0%, #006848 100%);">
                        <div class="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3">
                            <div class="d-flex align-items-center gap-3">
                                <div class="p-3 rounded-circle bg-white text-success d-flex align-items-center justify-content-center flex-shrink-0" style="width: 54px; height: 54px;">
                                    <i class="hgi hgi-stroke hgi-agreement-01 fs-3"></i>
                                </div>
                                <div>
                                    <span class="badge bg-light text-success fw-bold mb-1"><asp:Literal ID="litPartnerBadge" runat="server" Text="شراكة دولية معتمدة" /></span>
                                    <h3 class="h5 fw-bold mb-0 text-white"><asp:Literal ID="litPartnerTitle" runat="server" /></h3>
                                </div>
                            </div>
                            <a id="lnkPartner" runat="server" class="btn btn-light px-3 py-2 fw-medium d-inline-flex align-items-center gap-2" target="_blank" rel="noopener noreferrer" style="border-radius: 8px;">
                                <span><asp:Literal ID="litPartnerBtnText" runat="server" Text="زيارة موقع الجامعة الشريكة" /></span>
                                <i class="hgi hgi-stroke hgi-arrow-up-right-01"></i>
                            </a>
                        </div>
                    </div>
                </asp:Panel>

                <!-- 4. Mission & Objectives -->
                <div class="row g-4">
                    <!-- Mission -->
                    <div class="col-12 col-lg-5" id="divMission" runat="server">
                        <div class="card h-100 border rounded-4 p-4 bg-white shadow-sm">
                            <div class="d-flex align-items-center gap-2 mb-3 pb-2 border-bottom">
                                <i class="hgi hgi-stroke hgi-compass-02 fs-4" style="color: #006848;"></i>
                                <h2 class="h5 fw-bold text-dark mb-0"><asp:Literal ID="litLblMission" runat="server" Text="رسالة البرنامج" /></h2>
                            </div>
                            <div class="p-3 rounded-3 mb-0" style="background-color: #edf8f3; border-right: 3px solid #006848; color: #0d121c; line-height: 1.8;">
                                <asp:Literal ID="litMission" runat="server" />
                            </div>
                        </div>
                    </div>

                    <!-- Objectives -->
                    <div class="col-12 col-lg-7" id="divObjectives" runat="server">
                        <div class="card h-100 border rounded-4 p-4 bg-white shadow-sm">
                            <div class="d-flex align-items-center gap-2 mb-3 pb-2 border-bottom">
                                <i class="hgi hgi-stroke hgi-target-02 fs-4" style="color: #006848;"></i>
                                <h2 class="h5 fw-bold text-dark mb-0"><asp:Literal ID="litLblObjectives" runat="server" Text="أهداف البرنامج" /></h2>
                            </div>
                            <div class="program-objectives-content text-muted" style="line-height: 1.8;">
                                <asp:Literal ID="litObjectives" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- 5. Admission Requirements -->
                <div class="card border rounded-4 p-4 bg-white shadow-sm" id="divAdmission" runat="server">
                    <div class="d-flex align-items-center gap-2 mb-3 pb-2 border-bottom">
                        <i class="hgi hgi-stroke hgi-clipboard-check fs-4" style="color: #006848;"></i>
                        <h2 class="h5 fw-bold text-dark mb-0"><asp:Literal ID="litLblAdmission" runat="server" Text="شروط ومتطلبات القبول" /></h2>
                    </div>
                    <div class="program-requirements-content" style="line-height: 1.8;">
                        <asp:Literal ID="litAdmissionRequirements" runat="server" />
                    </div>
                </div>

                <!-- 6. Study Plan & Curriculum -->
                <div class="card border rounded-4 p-4 bg-white shadow-sm" id="divStudyPlan" runat="server">
                    <div class="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3 pb-2 border-bottom">
                        <div class="d-flex align-items-center gap-2">
                            <i class="hgi hgi-stroke hgi-book-open-02 fs-4" style="color: #006848;"></i>
                            <h2 class="h5 fw-bold text-dark mb-0"><asp:Literal ID="litLblStudyPlan" runat="server" Text="الخطة الدراسية" /></h2>
                        </div>
                        <a id="btnDownloadPlan" runat="server" visible="false" class="btn btn-sm btn-outline-success d-inline-flex align-items-center gap-2" target="_blank" style="border-radius: 6px;" rel="noopener noreferrer">
                            <i class="hgi hgi-stroke hgi-file-download"></i>
                            <span><asp:Literal ID="litBtnDownloadText" runat="server" Text="تحميل الخطة الدراسية (PDF)" /></span>
                        </a>
                    </div>
                    <div class="program-studyplan-content" style="line-height: 1.8;">
                        <asp:Literal ID="litStudyPlan" runat="server" />
                    </div>
                </div>

                <!-- 7. Career Opportunities -->
                <div class="card border rounded-4 p-4 bg-white shadow-sm" id="divCareers" runat="server">
                    <div class="d-flex align-items-center gap-2 mb-3 pb-2 border-bottom">
                        <i class="hgi hgi-stroke hgi-briefcase-02 fs-4" style="color: #006848;"></i>
                        <h2 class="h5 fw-bold text-dark mb-0"><asp:Literal ID="litLblCareers" runat="server" Text="مجالات العمل والفرص المهنية للخريجات" /></h2>
                    </div>
                    <div class="program-careers-content" style="line-height: 1.8;">
                        <asp:Literal ID="litCareerOpportunities" runat="server" />
                    </div>
                </div>

                <!-- 8. Back Navigation Link -->
                <div class="d-flex justify-content-between align-items-center pt-3 border-top">
                    <a id="lnkBack" runat="server" class="btn btn-outline-secondary d-inline-flex align-items-center gap-2" style="border-radius: 8px;">
                        <i class='hgi hgi-stroke <asp:Literal ID="litBackArrow" runat="server" />'></i>
                        <span><asp:Literal ID="litBackText" runat="server" Text="العودة إلى دليل البرامج" /></span>
                    </a>
                    <a href="mailto:dps-admissions@pnu.edu.sa" class="btn btn-light text-success border d-inline-flex align-items-center gap-2" style="border-radius: 8px;">
                        <i class="hgi hgi-stroke hgi-mail-01"></i>
                        <span><asp:Literal ID="litContactText" runat="server" Text="تواصل مع عمادة الدراسات العليا" /></span>
                    </a>
                </div>

            </div>
        </div>
    </section>
</main>
