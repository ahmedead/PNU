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
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>
<%@ Import Namespace="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections" %>

<div class="tab-pane fade" id="academic-program-v2-study-plan-pane" role="tabpanel" aria-labelledby="academic-program-v2-study-plan-tab" tabindex="0">
    <asp:Panel ID="pnlAll" runat="server">
        <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
            <table class="table table-striped align-middle mb-0">
                <thead>
                    <tr>
                        <th scope="col" class="text-center text-nowrap"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Levels %>" /></th>
                        <th scope="col" class="text-center text-nowrap"><asp:Literal runat="server" Text="<%$ Resources: PNUres, SCRATTR_SUBJ_CODE %>" /></th>
                        <th scope="col" class="text-start"><asp:Literal runat="server" Text="<%$ Resources: PNUres, COURSE_TITLE %>" /></th>
                        <th scope="col" class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, CREDIT %>" /></th>
                        <th scope="col" class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, PreRequisite %>" /></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="masterRepeater" runat="server">
                        <ItemTemplate>
                            <asp:Repeater ID="rptCourses" runat="server" DataSource='<%# Eval("StudyPlan") %>'>
                                <ItemTemplate>
                                    <tr data-study-plan-row="">
                                        <td class="text-center text-nowrap"><%# ((AllProgramsMain)((RepeaterItem)Container.Parent.Parent).DataItem).LevelDesc %></td>
                                        <td class="text-center text-nowrap" dir="auto"><%# Eval("SUBJ_CODE") %> <%# Eval("CRSE_NUMB") %></td>
                                        <td class="text-start"><%# Eval("COURSE_TITLE") %></td>
                                        <td class="text-center"><%# Eval("CREDIT") %></td>
                                        <td class="text-center" dir="auto"><%# String.IsNullOrEmpty(Eval("S_COREQ1") as string) ? "—" : Eval("S_COREQ1") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlData" runat="server">
        <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
            <table class="table table-striped align-middle mb-0">
                <thead>
                    <tr>
                        <th scope="col" class="text-center text-nowrap"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Levels %>" /></th>
                        <th scope="col" class="text-center text-nowrap"><asp:Literal runat="server" Text="<%$ Resources: PNUres, SCRATTR_SUBJ_CODE %>" /></th>
                        <th scope="col" class="text-start"><asp:Literal runat="server" Text="<%$ Resources: PNUres, COURSE_TITLE %>" /></th>
                        <th scope="col" class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, CREDIT %>" /></th>
                        <th scope="col" class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, PreRequisite %>" /></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <asp:Repeater ID="rptCourses" runat="server" DataSource='<%# Eval("StudyPlan") %>'>
                                <ItemTemplate>
                                    <tr data-study-plan-row="">
                                        <td class="text-center text-nowrap"><%# ((AllProgramsMain)((RepeaterItem)Container.Parent.Parent).DataItem).LevelDesc %></td>
                                        <td class="text-center text-nowrap" dir="auto"><%# Eval("SUBJ_CODE") %> <%# Eval("CRSE_NUMB") %></td>
                                        <td class="text-start"><%# Eval("COURSE_TITLE") %></td>
                                        <td class="text-center"><%# Eval("CREDIT") %></td>
                                        <td class="text-center" dir="auto"><%# String.IsNullOrEmpty(Eval("S_COREQ1") as string) ? "—" : Eval("S_COREQ1") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </asp:Panel>
</div>

<div class="tab-pane fade" id="academic-program-v2-requirements-pane" role="tabpanel" aria-labelledby="academic-program-v2-requirements-tab" tabindex="0">
    <uc1:ucPLANS_ELEC_U runat="server" id="ucPLANS_ELEC_U" />
    <uc1:ucPLANS_ELEC_C runat="server" id="ucPLANS_ELEC_C" />
    <uc1:ucPLANS_ELEC_P runat="server" id="ucPLANS_ELEC_P" />
</div>