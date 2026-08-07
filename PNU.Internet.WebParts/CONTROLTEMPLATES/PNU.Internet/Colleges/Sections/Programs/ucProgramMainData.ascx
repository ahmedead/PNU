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
        white-space: normal; /* Allows text to wrap to the next line */
        word-wrap: break-word; /* Allows long words to be broken and wrap onto the next line */
        overflow-wrap: break-word; /* Similar to word-wrap for better compatibility */
    }
</style>



<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>
        <section class="breadcrumb">
            <div class="container py-5 my-3">
                <h1 class="display-6 fw-bold mb-3">  <%# SPFactory.GetLocalizedTitle(Eval("ProgramName"), Eval("ProgramName_EN")) %>  </h1>
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb h6">
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>" class="text-decoration-none fw-bold"><asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a></li>
                <li class="breadcrumb-item"><a href="<%# String.Format("{0}Faculties/Pages/AllCollegesNew.aspx", SPFactory.GetSiteURL()) %>" class="text-decoration-none fw-bold"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Colleges %>" /></a></li>
                <li class="breadcrumb-item"><a href="<%# String.Format("{0}Faculties/Pages/FacultyMain.aspx?Source={1}", SPFactory.GetSiteURL(),Eval("CollegeCode")) %>" class="text-decoration-none fw-bold">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyTitle %>" /> 
                    <%# SPFactory.GetLocalizedTitle(Eval("CollegeName"), Eval("CollegeName_EN")) %>
                </a></li>
                <li class="breadcrumb-item"><a href="<%# String.Format("{0}Faculties/Pages/Sections.aspx?SecCode={1}", SPFactory.GetSiteURL(),Eval("DepartmentCode")) %>" class="text-decoration-none fw-bold">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, SectionTitle %>" /> 
                    <%# SPFactory.GetLocalizedTitle(Eval("DepartmentName"), Eval("DepartmentName_EN")) %>
                </a></li>

                <li class="breadcrumb-item active fw-bold text-turquoise-600" aria-current="page">
                    <%# SPFactory.GetLocalizedTitle(Eval("ProgramName"), Eval("ProgramName_EN")) %> 
                </li>
                    </ol>
                </nav>
            </div>
        </section>

        <section>
            <div class="container pt-5 my-5">
                <div class="d-flex align-items-start  mb-0 mt-3">
                    <h1
                        class="title text-dark fw-bold px-2 border-start border-primary mb-4 lb-lo mt-md-0 mt-5 me-md-4 flex-shrink-0 h3">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AboutProgram %>" /> 
                    </h1>
                </div>
                <p class="fs-5 mb-5 text-muted">
                   <%# SPFactory.GetLocalizedTitle(Eval("ProgramDesc"), Eval("ProgramDesc_EN")) %> 
                </p>
            </div>
        </section>

        



        <section>
            <div class="container pt-5 my-5">
                <div class="row">        
                    <div class="col-md-12">
                        <div class="the-message p-2">
                            <div class="row my-2 justify-content-around gy-3 py-3 px-3">
        
                                <div class="col-lg-4 col-md-4">
                                    <div class="d-flex align-items-center justify-content-center mb-4">
                                        <svg class="bi me-3" width="50" height="50">
                                            <use xlink:href="#progBook"></use>
                                        </svg>
                                        <div>
                                        <p class="text-muted h5"> <asp:Literal runat="server" Text="<%$ Resources: PNUres, Major %>" /></p>
                                            <p class="text-dark fw-bold h5 mb-0">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Major"), Eval("Major_EN")) %> 
                                            </p>
                                        </div>
                                    </div>
                                </div>
            
                                <div class="col-lg-4 col-md-4">
                                    <div class="d-flex align-items-center justify-content-center mb-4">
                                        <svg class="bi me-3" width="50" height="50">
                                            <use xlink:href="#progLang"></use>
                                        </svg>
                                        <div>
                                        <p class="text-muted h5"> <asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgLanguage %>" /></p>
                                        <p class="text-dark fw-bold h5 mb-0"> <%# Eval("ProgramLanguage") %> </p>
                                        </div>
                                    </div>
                                </div>
            
                                <div class="col-lg-4 col-md-4">
                                    <div class="d-flex align-items-center justify-content-center mb-4">
                                        <svg class="bi me-3" width="50" height="50">
                                            <use xlink:href="#progCalendar"></use>
                                        </svg>
                                        <div>
                                            <p class="text-muted h5"> <asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgDuration %>" /></p>
                                            <p class="text-dark fw-bold h5 mb-0">
                                                <%# Eval("ProgramYears") %> <asp:Literal runat="server" Text="<%$ Resources: PNUres, Years %>" />
                                            </p>
                                        </div>
                                    </div>
                                </div>
            
            
                            </div>
                            <div class="mt-3 px-md-5 px-0">
                                <div class="d-block">
                                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AcademicDegree %>" />
                                    </h1>
                                </div>
                                <div class="d-block text-start">
                                    <p class=" text-muted lh-base fs-5">
                                        <%# SPFactory.GetLocalizedTitle(Eval("ProgramDegree"), Eval("ProgramDegree_EN")) %> 
                                    </p>
                                </div>
                            </div>
                            <div class="mt-5 px-md-5 px-0">
                                <div class="d-block">
                                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, CareerOpportunities %>" />
                                    </h1>
                                </div>
                                <div class="d-block text-start mb-5">
                                    <p class="h4 text-muted lh-base fs-5">
                                        <%# SPFactory.GetLocalizedTitle(Eval("ProgramFields"), Eval("ProgramFields_EN")) %> 
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

      

            </div>


        </section>

        <section>
            <div class="container pt-5 my-5">
                <div class="d-flex align-items-start  mb-0 mt-3">
                    <h1
                        class="title text-dark fw-bold px-2 border-start border-primary mb-4 lb-lo mt-md-0 mt-5 me-md-4 flex-shrink-0 h3">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgramNature %>" />
                    </h1>
                </div>
                <p class="fs-5 mb-5 text-muted">
                    <%# SPFactory.GetLocalizedTitle(Eval("ProgramNature"), Eval("ProgramNature_EN")) %> 
                </p>
            </div>
        </section>



    </ItemTemplate>
</asp:Repeater>



<uc1:ucObjectivesOutcomes runat="server" id="ucObjectivesOutcomes" />

<uc1:ucStudyPlan runat="server" id="ucStudyPlan" />


<uc1:ucAcademicCredits runat="server" id="ucAcademicCredits" />




<style>
.breadcrumbhide {display: none;}
</style>


<script>
    document.addEventListener("DOMContentLoaded", function() {
    const secLink = document.querySelector('a[href*="SecCode=0000"]');
    if (secLink) {
        secLink.closest("li").style.display = "none";
    }
});

</script>