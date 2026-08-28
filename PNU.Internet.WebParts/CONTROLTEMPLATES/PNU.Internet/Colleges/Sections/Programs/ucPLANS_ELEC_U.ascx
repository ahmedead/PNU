<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPLANS_ELEC_U.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs.ucPLANS_ELEC_U" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Repeater ID="masterRepeaterU" runat="server">
    <ItemTemplate>
        <section class="mt-4" aria-labelledby='<%# "program-u-req-" + Eval("LevelCode") %>'>
            <h3 id='<%# "program-u-req-" + Eval("LevelCode") %>' class="h4 mb-3"><%# Eval("LevelDesc") %></h3>
            <div class="table-responsive border border-top-0 border-bottom-0 rounded-2 mb-4">
                <table class="table table-striped align-middle mb-0">
                    <thead>
                        <tr>
                            <th scope="col" class="text-center text-nowrap"><asp:Literal runat="server" Text="<%$ Resources: PNUres, SCRATTR_SUBJ_CODE %>" /></th>
                            <th scope="col" class="text-start"><asp:Literal runat="server" Text="<%$ Resources: PNUres, COURSE_TITLE %>" /></th>
                            <th scope="col" class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, CREDIT %>" /></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptCoursesU" runat="server" DataSource='<%# Eval("StudyPlan") %>'>
                            <ItemTemplate>
                                <tr data-plan-requirement-row="">
                                    <td class="text-center text-nowrap" dir="auto"><%# Eval("SCRATTR_SUBJ_CODE") %> <%# Eval("SCRATTR_CRSE_NUMB") %></td>
                                    <td class="text-start"><%# Eval("COURSE_TITLE") %></td>
                                    <td class="text-center"><%# Eval("CREDIT") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </section>
    </ItemTemplate>
</asp:Repeater>
