<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSectionMembers.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.ucSectionMembers" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="container page-padding pt-0">
    <h3 class="h5 mt-5"><asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyMembers %>" /></h3>

    <div class="accordion accordion-flush mt-3" id="sectionMembersAccordion">

        <%-- الأساتذة --%>
        <div class="accordion-item" id="tab1" runat="server">
            <p class="accordion-header" id="sectionMembersHeading1">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#sectionMembersCollapse1" aria-expanded="false" aria-controls="sectionMembersCollapse1">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, Professors %>" />
                </button>
            </p>
            <div class="accordion-collapse collapse" id="sectionMembersCollapse1" aria-labelledby="sectionMembersHeading1" data-bs-parent="#sectionMembersAccordion">
                <div class="accordion-body">
                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                        <table class="table table-striped mb-0">
                            <thead>
                                <tr>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Name %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, specialization %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, minor %>" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="<%# String.Format("/ar/Faculties/Pages/FacultyMemberDetails.aspx?view={0}", Eval("Email_address")) %>"><%# SPFactory.GetLocalizedTitle(Eval("FULL_NAME"), Eval("ENGLISH_NAME")) %></a></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIALIZATION"), Eval("SPECIALIZATION_EN")) %></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIAL_CPECIALIZATION"), Eval("SPECIAL_CPECIALIZATION_EN")) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <%-- الأساتذة المشاركون --%>
        <div class="accordion-item" id="tab2" runat="server">
            <p class="accordion-header" id="sectionMembersHeading2">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#sectionMembersCollapse2" aria-expanded="false" aria-controls="sectionMembersCollapse2">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, AssociateProfessors %>" />
                </button>
            </p>
            <div class="accordion-collapse collapse" id="sectionMembersCollapse2" aria-labelledby="sectionMembersHeading2" data-bs-parent="#sectionMembersAccordion">
                <div class="accordion-body">
                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                        <table class="table table-striped mb-0">
                            <thead>
                                <tr>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Name %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, specialization %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, minor %>" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater2" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="<%# String.Format("/ar/Faculties/Pages/FacultyMemberDetails.aspx?view={0}", Eval("Email_address")) %>"><%# SPFactory.GetLocalizedTitle(Eval("FULL_NAME"), Eval("ENGLISH_NAME")) %></a></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIALIZATION"), Eval("SPECIALIZATION_EN")) %></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIAL_CPECIALIZATION"), Eval("SPECIAL_CPECIALIZATION_EN")) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <%-- الأساتذة المساعدون --%>
        <div class="accordion-item" id="tab3" runat="server">
            <p class="accordion-header" id="sectionMembersHeading3">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#sectionMembersCollapse3" aria-expanded="false" aria-controls="sectionMembersCollapse3">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, AssistantProfessors %>" />
                </button>
            </p>
            <div class="accordion-collapse collapse" id="sectionMembersCollapse3" aria-labelledby="sectionMembersHeading3" data-bs-parent="#sectionMembersAccordion">
                <div class="accordion-body">
                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                        <table class="table table-striped mb-0">
                            <thead>
                                <tr>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Name %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, specialization %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, minor %>" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater3" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="<%# String.Format("/ar/Faculties/Pages/FacultyMemberDetails.aspx?view={0}", Eval("Email_address")) %>"><%# SPFactory.GetLocalizedTitle(Eval("FULL_NAME"), Eval("ENGLISH_NAME")) %></a></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIALIZATION"), Eval("SPECIALIZATION_EN")) %></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIAL_CPECIALIZATION"), Eval("SPECIAL_CPECIALIZATION_EN")) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <%-- المحاضرون --%>
        <div class="accordion-item" id="tab4" runat="server">
            <p class="accordion-header" id="sectionMembersHeading4">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#sectionMembersCollapse4" aria-expanded="false" aria-controls="sectionMembersCollapse4">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, Lecturers %>" />
                </button>
            </p>
            <div class="accordion-collapse collapse" id="sectionMembersCollapse4" aria-labelledby="sectionMembersHeading4" data-bs-parent="#sectionMembersAccordion">
                <div class="accordion-body">
                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                        <table class="table table-striped mb-0">
                            <thead>
                                <tr>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Name %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, specialization %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, minor %>" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater4" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="<%# String.Format("/ar/Faculties/Pages/FacultyMemberDetails.aspx?view={0}", Eval("Email_address")) %>"><%# SPFactory.GetLocalizedTitle(Eval("FULL_NAME"), Eval("ENGLISH_NAME")) %></a></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIALIZATION"), Eval("SPECIALIZATION_EN")) %></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIAL_CPECIALIZATION"), Eval("SPECIAL_CPECIALIZATION_EN")) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <%-- المعيدون --%>
        <div class="accordion-item" id="tab5" runat="server">
            <p class="accordion-header" id="sectionMembersHeading5">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#sectionMembersCollapse5" aria-expanded="false" aria-controls="sectionMembersCollapse5">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, TeachingAssistants %>" />
                </button>
            </p>
            <div class="accordion-collapse collapse" id="sectionMembersCollapse5" aria-labelledby="sectionMembersHeading5" data-bs-parent="#sectionMembersAccordion">
                <div class="accordion-body">
                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                        <table class="table table-striped mb-0">
                            <thead>
                                <tr>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Name %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, specialization %>" /></th>
                                    <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: PNUres, minor %>" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater5" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="<%# String.Format("/ar/Faculties/Pages/FacultyMemberDetails.aspx?view={0}", Eval("Email_address")) %>"><%# SPFactory.GetLocalizedTitle(Eval("FULL_NAME"), Eval("ENGLISH_NAME")) %></a></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIALIZATION"), Eval("SPECIALIZATION_EN")) %></td>
                                            <td><%# SPFactory.GetLocalizedTitle(Eval("SPECIAL_CPECIALIZATION"), Eval("SPECIAL_CPECIALIZATION_EN")) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

    </div>
</section>        