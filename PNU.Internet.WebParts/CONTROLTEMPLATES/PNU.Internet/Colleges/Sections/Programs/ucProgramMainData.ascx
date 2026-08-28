<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/Sections/Programs/ucStudyPlan.ascx" TagPrefix="uc1" TagName="ucStudyPlan" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/ucAcademicCredits.ascx" TagPrefix="uc1" TagName="ucAcademicCredits" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/Sections/Programs/ucObjectivesOutcomes.ascx" TagPrefix="uc1" TagName="ucObjectivesOutcomes" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProgramMainData.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs.ucProgramMainData" %>

<style>
    .wrap-text {
        white-space: normal;
        word-wrap: break-word;
        overflow-wrap: break-word;
    }
    .breadcrumbhide {
        display: none;
    }
</style>

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <asp:Repeater ID="rptMainData" runat="server">
        <ItemTemplate>
            <div class="bg-primary-25 py-5" id="program-main-hero" data-aos="fade-up">
                <div class="container">
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb mb-2">
                            <li class="breadcrumb-item small"><a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>"><asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a></li>
                            <li class="breadcrumb-item small"><a href="<%# String.Format("{0}Faculties/Pages/AllCollegesNew.aspx", SPFactory.GetSiteURL()) %>"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Colleges %>" /></a></li>
                            <li class="breadcrumb-item small"><a href="<%# String.Format("{0}Faculties/Pages/FacultyMain.aspx?Source={1}", SPFactory.GetSiteURL(),Eval("CollegeCode")) %>">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyTitle %>" /> 
                                <%# SPFactory.GetLocalizedTitle(Eval("CollegeName"), Eval("CollegeName_EN")) %>
                            </a></li>
                            <li class="breadcrumb-item small"><a href="<%# String.Format("{0}Faculties/Pages/Sections.aspx?SecCode={1}", SPFactory.GetSiteURL(),Eval("DepartmentCode")) %>">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, SectionTitle %>" /> 
                                <%# SPFactory.GetLocalizedTitle(Eval("DepartmentName"), Eval("DepartmentName_EN")) %>
                            </a></li>
                            <li class="breadcrumb-item small active" aria-current="page">
                                <span><%# SPFactory.GetLocalizedTitle(Eval("ProgramName"), Eval("ProgramName_EN")) %></span>
                            </li>
                        </ol>
                    </nav>
                    <div class="row g-4">
                        <div class="col-12 col-lg-9">
                            <h1 class="mb-0 fw-semibold h2"><%# SPFactory.GetLocalizedTitle(Eval("ProgramName"), Eval("ProgramName_EN")) %></h1>
                            <p class="mb-0 mt-3"><%# SPFactory.GetLocalizedTitle(Eval("ProgramDesc"), Eval("ProgramDesc_EN")) %></p>
                        </div>
                    </div>
                </div>
            </div>

            <section class="py-5" data-aos="fade-up" aria-labelledby="program-v2-overview-title">
                <div class="container">
                    <h2 id="program-v2-overview-title" class="mb-4"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AboutProgram %>" /></h2>
                    <div>
                        <h3 class="h4 mb-3"><asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgramNature %>" /></h3>
                        <p class="mb-0"><%# SPFactory.GetLocalizedTitle(Eval("ProgramNature"), Eval("ProgramNature_EN")) %></p>
                    </div>
                    <div class="card bg-primary-25 border-0 mt-4 mb-0">
                        <div class="card-body p-4 p-lg-5">
                            <div class="d-flex flex-column gap-3">
                                <span class="icon-container bg-white">
                                    <i class="hgi hgi-stroke hgi-briefcase-02 fs-3" aria-hidden="true"></i>
                                </span>
                                <h3 class="h4 mb-0"><asp:Literal runat="server" Text="<%$ Resources: PNUres, CareerOpportunities %>" /></h3>
                                <p class="mb-0"><%# SPFactory.GetLocalizedTitle(Eval("ProgramFields"), Eval("ProgramFields_EN")) %></p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section class="py-5" data-aos="fade-up" aria-labelledby="program-v2-information-title">
                <div class="container">
                    <h2 id="program-v2-information-title" class="mb-4"><asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgramDetails %>" /></h2>
                    <div class="card overflow-hidden" data-program-details-unit="">
                        <div class="row row-cols-1 row-cols-md-3 g-0">
                            <div class="col position-relative">
                                <div class="list-group-item border-0 rounded-0 p-4 h-100" data-program-detail-item="">
                                    <div class="d-flex gap-3 align-items-center">
                                        <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                            <i class="hgi hgi-stroke hgi-school fs-4 text-primary" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <strong class="text-dark d-block"><asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyTitle %>" /></strong>
                                            <small class="text-muted"><%# SPFactory.GetLocalizedTitle(Eval("CollegeName"), Eval("CollegeName_EN")) %></small>
                                        </div>
                                    </div>
                                </div>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-none d-md-block" aria-hidden="true"></span>
                            </div>
                            <div class="col position-relative">
                                <div class="list-group-item border-0 rounded-0 p-4 h-100" data-program-detail-item="">
                                    <div class="d-flex gap-3 align-items-center">
                                        <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                            <i class="hgi hgi-stroke hgi-building-02 fs-4 text-primary" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <strong class="text-dark d-block"><asp:Literal runat="server" Text="<%$ Resources: PNUres, SectionTitle %>" /></strong>
                                            <small class="text-muted"><%# SPFactory.GetLocalizedTitle(Eval("DepartmentName"), Eval("DepartmentName_EN")) %></small>
                                        </div>
                                    </div>
                                </div>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-none d-md-block" aria-hidden="true"></span>
                                <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                            </div>
                            <div class="col position-relative">
                                <div class="list-group-item border-0 rounded-0 p-4 h-100" data-program-detail-item="">
                                    <div class="d-flex gap-3 align-items-center">
                                        <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                            <i class="hgi hgi-stroke hgi-book-open-02 fs-4 text-primary" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <strong class="text-dark d-block"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Major %>" /></strong>
                                            <small class="text-muted"><%# SPFactory.GetLocalizedTitle(Eval("Major"), Eval("Major_EN")) %></small>
                                        </div>
                                    </div>
                                </div>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-none d-md-block" aria-hidden="true"></span>
                                <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                            </div>
                            <div class="col position-relative">
                                <div class="list-group-item border-0 rounded-0 p-4 h-100" data-program-detail-item="">
                                    <div class="d-flex gap-3 align-items-center">
                                        <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                            <i class="hgi hgi-stroke hgi-layers-01 fs-4 text-primary" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <strong class="text-dark d-block"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AcademicDegree %>" /></strong>
                                            <small class="text-muted"><%# SPFactory.GetLocalizedTitle(Eval("ProgramDegree"), Eval("ProgramDegree_EN")) %></small>
                                        </div>
                                    </div>
                                </div>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                            </div>
                            <div class="col position-relative">
                                <div class="list-group-item border-0 rounded-0 p-4 h-100" data-program-detail-item="">
                                    <div class="d-flex gap-3 align-items-center">
                                        <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                            <i class="hgi hgi-stroke hgi-language-skill fs-4 text-primary" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <strong class="text-dark d-block"><asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgLanguage %>" /></strong>
                                            <small class="text-muted"><%# Eval("ProgramLanguage") %></small>
                                        </div>
                                    </div>
                                </div>
                                <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                                <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                            </div>
                            <div class="col position-relative">
                                <div class="list-group-item border-0 rounded-0 p-4 h-100" data-program-detail-item="">
                                    <div class="d-flex gap-3 align-items-center">
                                        <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                            <i class="hgi hgi-stroke hgi-calendar-02 fs-4 text-primary" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <strong class="text-dark d-block"><asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgDuration %>" /></strong>
                                            <small class="text-muted"><%# Eval("ProgramYears") %> <asp:Literal runat="server" Text="<%$ Resources: PNUres, Years %>" /></small>
                                        </div>
                                    </div>
                                </div>
                                <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ItemTemplate>
    </asp:Repeater>

    <!-- تفاصيل البرنامج (Tabs Section) -->
    <section class="py-5" data-aos="fade-up" aria-labelledby="program-v2-details-title">
        <div class="container">
            <h2 id="program-v2-details-title" class="mb-4"><asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgramDetails %>" /></h2>
            
            <ul class="nav nav-tabs nav-underline nav-flush flex-nowrap overflow-x-auto overflow-y-hidden w-100 mb-4" id="academicProgramV2Tabs" role="tablist" aria-label="التنقل بين تفاصيل البرنامج">
                <li class="nav-item" role="presentation">
                    <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2 text-nowrap active" id="academic-program-v2-objectives-tab" data-bs-toggle="tab" data-bs-target="#academic-program-v2-objectives-pane" type="button" role="tab" aria-controls="academic-program-v2-objectives-pane" aria-selected="true" tabindex="0">
                        <i class="hgi hgi-stroke hgi-target-02 fs-5 fw-light" aria-hidden="true"></i>
                        <span>الأهداف</span>
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2 text-nowrap" id="academic-program-v2-outcomes-tab" data-bs-toggle="tab" data-bs-target="#academic-program-v2-outcomes-pane" type="button" role="tab" aria-controls="academic-program-v2-outcomes-pane" aria-selected="false" tabindex="-1">
                        <i class="hgi hgi-stroke hgi-checkmark-badge-01 fs-5 fw-light" aria-hidden="true"></i>
                        <span>مخرجات التعلم</span>
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2 text-nowrap" id="academic-program-v2-graduate-tab" data-bs-toggle="tab" data-bs-target="#academic-program-v2-graduate-pane" type="button" role="tab" aria-controls="academic-program-v2-graduate-pane" aria-selected="false" tabindex="-1">
                        <i class="hgi hgi-stroke hgi-user-check-01 fs-5 fw-light" aria-hidden="true"></i>
                        <span>مواصفات خريجة البرنامج</span>
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2 text-nowrap" id="academic-program-v2-study-plan-tab" data-bs-toggle="tab" data-bs-target="#academic-program-v2-study-plan-pane" type="button" role="tab" aria-controls="academic-program-v2-study-plan-pane" aria-selected="false" tabindex="-1">
                        <i class="hgi hgi-stroke hgi-book-open-02 fs-5 fw-light" aria-hidden="true"></i>
                        <span>الخطة الدراسية</span>
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2 text-nowrap" id="academic-program-v2-requirements-tab" data-bs-toggle="tab" data-bs-target="#academic-program-v2-requirements-pane" type="button" role="tab" aria-controls="academic-program-v2-requirements-pane" aria-selected="false" tabindex="-1">
                        <i class="hgi hgi-stroke hgi-file-02 fs-5 fw-light" aria-hidden="true"></i>
                        <span>متطلبات الخطة</span>
                    </button>
                </li>
            </ul>

            <div class="tab-content pt-2" id="academicProgramV2TabContent">
                <uc1:ucObjectivesOutcomes runat="server" id="ucObjectivesOutcomes" />
                <uc1:ucStudyPlan runat="server" id="ucStudyPlan" />
            </div>
        </div>
    </section>

    <uc1:ucAcademicCredits runat="server" id="ucAcademicCredits" />
</main>

<script>
    function overridePageBreadcrumbAndTitle() {
        try {
            const heroBanner = document.getElementById("program-main-hero");
            if (!heroBanner) return;

            // 1. Hide the department link if code is 0000
            const secLink = heroBanner.querySelector('a[href*="SecCode=0000"]');
            if (secLink && secLink.closest("li")) {
                secLink.closest("li").style.display = "none";
            }

            // 2. Find any outer breadcrumb or page hero outside our control
            const outerBreadcrumbs = Array.from(document.querySelectorAll('nav[aria-label="breadcrumb"], .breadcrumb'))
                .filter(el => !heroBanner.contains(el));

            const outerHeroes = Array.from(document.querySelectorAll('.bg-primary-25, .page-header, .page-header-container, .breadcrumb-container'))
                .filter(el => !heroBanner.contains(el) && el !== heroBanner);

            // 3. Find outer page titles containing "بيانات البرنامج"
            const outerTitles = Array.from(document.querySelectorAll('h1, h2, .h1, .h2, .page-title, #pageTitle'))
                .filter(el => !heroBanner.contains(el) && el.textContent.trim().includes("بيانات البرنامج"));

            // If an outer hero container exists (above the side-menu + main body), replace it with our hero banner
            if (outerHeroes.length > 0) {
                const targetOuterHero = outerHeroes[0];
                targetOuterHero.parentNode.replaceChild(heroBanner, targetOuterHero);
                heroBanner.style.display = "";
            } else if (outerBreadcrumbs.length > 0) {
                const outerBreadcrumb = outerBreadcrumbs[0];
                const outerWrapper = outerBreadcrumb.closest('.bg-primary-25') || outerBreadcrumb.closest('.container') || outerBreadcrumb.parentElement;
                if (outerWrapper && !heroBanner.contains(outerWrapper) && outerWrapper !== document.body) {
                    outerWrapper.parentNode.insertBefore(heroBanner, outerWrapper);
                    outerWrapper.style.display = "none";
                    heroBanner.style.display = "";
                }
            } else {
                // Check if main-body or container has an outer section before the 2-column layout
                const mainBody = document.querySelector('.dga-main-body, #main-content');
                if (mainBody && mainBody.parentElement && !mainBody.contains(heroBanner)) {
                    mainBody.parentElement.insertBefore(heroBanner, mainBody);
                    heroBanner.style.display = "";
                }
            }

            // 4. Hide any duplicate outer breadcrumbs and titles
            outerBreadcrumbs.forEach(el => el.style.display = "none");
            outerTitles.forEach(el => {
                el.style.display = "none";
                if (el.parentElement && el.parentElement.children.length <= 1) {
                    el.parentElement.style.display = "none";
                }
            });
        } catch (e) {
            console.warn("Error overriding page breadcrumb:", e);
        }
    }

    // Execute immediately and on DOMContentLoaded
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", overridePageBreadcrumbAndTitle);
    } else {
        overridePageBreadcrumbAndTitle();
    }
</script>