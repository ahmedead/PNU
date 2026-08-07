<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/Sections/Programs/ucPLANS_ELEC_U.ascx" TagPrefix="uc1" TagName="ucPLANS_ELEC_U" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/Sections/Programs/ucPLANS_ELEC_C.ascx" TagPrefix="uc1" TagName="ucPLANS_ELEC_C" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/Sections/Programs/ucPLANS_ELEC_P.ascx" TagPrefix="uc1" TagName="ucPLANS_ELEC_P" %>



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucStudyPlan.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs.ucStudyPlan" %>



<asp:Panel ID="pnlAll" runat="server">
<section class="my-5 py-5">
    <div class="container">
        <div class="d-flex justify-content-center ">
                    <h1
                        class="title text-dark fw-bold px-2  mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0 mt-5 h3 text-center">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, StudyPlan %>" />
                    </h1>
                    <img src="/Style Library/NewStyle/UI5/images/coiled-arrow.png" class="mb-4">
                </div>
        <div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">

            <div class="col-12 col-lg-auto tabbable">
                <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" id="news1-tab" data-bs-toggle="tab"
                            data-bs-target="#news1-tab-pane" type="button" role="tab"
                            aria-controls="news1-tab-pane" aria-selected="false" tabindex="0">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, UniversityRequisites %>" />
                        </button>
                    </li>

                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="news2-tab" data-bs-toggle="tab"
                            data-bs-target="#news2-tab-pane" type="button" role="tab"
                            aria-controls="news2-tab-pane" aria-selected="false" tabindex="1">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, CollegeRequisites %>" /> 
                        </button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="news3-tab" data-bs-toggle="tab"
                            data-bs-target="#news3-tab-pane" type="button" role="tab"
                            aria-controls="news3-tab-pane" aria-selected="false" tabindex="2">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, ProgramRequisites %>" />
                        </button>
                    </li>


                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="new4-tab" data-bs-toggle="tab"
                            data-bs-target="#news4-tab-pane" type="button" role="tab"
                            aria-controls="news4-tab-pane" aria-selected="true" tabindex="3">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, Levels %>" />
                        </button>
                    </li>

                </ul>
            </div>
        </div>

        <div class="tab-content">

            <div class="tab-pane show active" id="news1-tab-pane" role="tabpanel" aria-labelledby="news1-tab" tabindex="0">

                <uc1:ucPLANS_ELEC_U runat="server" id="ucPLANS_ELEC_U" />
                
            
        </div>


            <div class="tab-pane hide" id="news2-tab-pane" role="tabpanel" aria-labelledby="news2-tab" tabindex="1">
                
                <uc1:ucPLANS_ELEC_C runat="server" id="ucPLANS_ELEC_C" />

            </div>

            <div class="tab-pane hide" id="news3-tab-pane" role="tabpanel" aria-labelledby="news3-tab" tabindex="2">
                
                <uc1:ucPLANS_ELEC_P runat="server" id="ucPLANS_ELEC_P" />

            </div>

            <div class="tab-pane  hide" id="news4-tab-pane" role="tabpanel" aria-labelledby="news4-tab" tabindex="3">
                <div class="accordion-card">
    <asp:Repeater ID="masterRepeater" runat="server">
        <ItemTemplate>
            <div class="accordion accordion-flush " id="accordionFlushExample<%# Eval("LevelCode") %>">




                <div class="accordion-item  my-2 border-0">
                    <h2 class="accordion-header" id="flush-headingOne">
                        <button class="accordion-button collapsed border" type="button"
                            data-bs-toggle="collapse" data-bs-target="#flush-collapse<%# Eval("LevelCode") %>" aria-expanded="false"
                            aria-controls="flush-collapse<%# Eval("LevelCode") %>">
                            <%# Eval("LevelDesc") %>
                        </button>
                    </h2>

                    <div id="flush-collapse<%# Eval("LevelCode") %>" class="accordion-collapse collapse "
                        aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample">
                        <div class="accordion-body p-0">


                            <table class="table rounded-bottom-4 fs-5 text-nowrap mb-0  table-striped-secondary overflow-hidden">
                                <thead>
                                    <tr class="my-2 align-middle">
                                        <th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, SCRATTR_SUBJ_CODE %>" /> </th>
                                        <th class="text-start"><asp:Literal runat="server" Text="<%$ Resources: PNUres, COURSE_TITLE %>" /></th>
                                        <th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, CREDIT %>" /></th>
										<th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, PreRequisite %>" /></th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="rptCourses" runat="server" DataSource='<%# Eval("StudyPlan") %>'>
                                        <ItemTemplate>
                                            <tr>
                                                <td class=" text-primary text-center">
                                                    <button class="btn btn-link text-primary fs-5 p-0 text-decoration-none" type="button"
                                                        data-bs-toggle="collapse" data-bs-target="#collapseWidthExample0"
                                                        aria-expanded="false" aria-controls="   ">
                                                        <%# Eval("SUBJ_CODE") %> <%# Eval("CRSE_NUMB") %>
                                                    </button>
                                                </td>
                                                <td class="text-start"><%# Eval("COURSE_TITLE") %></td>
                                                <td class="text-center"><%# Eval("CREDIT") %></td>
												<td class="text-center"><%# Eval("S_COREQ1") %></td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <%--<tr class="collapse  bg-gray" id="collapseWidthExample0">
                                                <td colspan="5" class="table-active">

                                                    <div>
                                                        <p class="text-wrap">
                                                            وصف المقرر: <%# Eval("STVATTR_DESC") %>
                                                        </p>
                                                    </div>


                                                </td>

                                            </tr>--%>

                                        </ItemTemplate>
                                    </asp:Repeater>


                                </tbody>

                            </table>


                        </div>
                    </div>

                </div>
        </ItemTemplate>
    </asp:Repeater>




</div>
            </div>
       



    </div>

        </div>
</section>
</asp:Panel>



<asp:Panel ID="pnlData" runat="server">
<section class="my-5 py-5">
    <div class="container">

        <div class="d-flex align-items-start ">
            <h1
                class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0"><asp:Literal runat="server" Text="<%$ Resources: PNUres, StudyPlan %>" />
                       
                <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
            <img src="/Style Library/NewStyle/UI5/images/coiled-arrow.png" class="mb-4">
        </div>

        <div class="accordion-card">
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <div class="accordion accordion-flush " id="accordionFlushExample<%# Eval("LevelCode") %>">




                <div class="accordion-item  my-2 border-0">
                    <h2 class="accordion-header" id="flush-headingOne">
                        <button class="accordion-button collapsed border" type="button"
                            data-bs-toggle="collapse" data-bs-target="#flush-collapse<%# Eval("LevelCode") %>" aria-expanded="false"
                            aria-controls="flush-collapse<%# Eval("LevelCode") %>">
                            <%# Eval("LevelDesc") %>
                        </button>
                    </h2>

                    <div id="flush-collapse<%# Eval("LevelCode") %>" class="accordion-collapse collapse "
                        aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample">
                        <div class="accordion-body p-0">


                            <table class="table rounded-bottom-4 fs-5 text-nowrap mb-0  table-striped-secondary overflow-hidden">
                                <thead>
                                    <tr class="my-2 align-middle">
                                        <th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, SCRATTR_SUBJ_CODE %>" /> </th>
                                        <th class="text-start"><asp:Literal runat="server" Text="<%$ Resources: PNUres, COURSE_TITLE %>" /></th>
                                        <th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, CREDIT %>" /></th>
                                        <th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, PreRequisite %>" /></th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="rptCourses" runat="server" DataSource='<%# Eval("StudyPlan") %>'>
                                        <ItemTemplate>
                                            <tr>
                                                <td class=" text-primary text-center">
                                                    <button class="btn btn-link text-primary fs-5 p-0 text-decoration-none" type="button"
                                                        data-bs-toggle="collapse" data-bs-target="#collapseWidthExample0"
                                                        aria-expanded="false" aria-controls="   ">
                                                        <%# Eval("SUBJ_CODE") %> <%# Eval("CRSE_NUMB") %>
                                                    </button>
                                                </td>
                                                <td class="text-start"><%# Eval("COURSE_TITLE") %></td>
                                                <td class="text-center"><%# Eval("CREDIT") %></td>
												<td class="text-center"><%# Eval("S_COREQ1") %></td>
                                            </tr>
                                            <tr>
                                            </tr>


                                        </ItemTemplate>
                                    </asp:Repeater>


                                </tbody>

                            </table>


                        </div>
                    </div>

                </div>
        </ItemTemplate>
    </asp:Repeater>




</div>
            
        </div>
</section>
</asp:Panel>