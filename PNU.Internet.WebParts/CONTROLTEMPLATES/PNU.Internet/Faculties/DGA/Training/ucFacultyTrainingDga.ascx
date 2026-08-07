<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFacultyTrainingDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.ucFacultyTrainingDga" %>


<section id="faculty-training" class="pnu-section-anchor">
    <h2 class="mb-2"><%= SPContext.Current.ListItem["Title"] %></h2>
<p><%= SPContext.Current.ListItem["Comments"] %></p>

    <div class="accordion accordion-flush mt-3" id="faculty-trainingAccordion">
        <div class="accordion-item">
            <p class="accordion-header" id="faculty-trainingAccordionHeading1">
                <button type="button" class="accordion-button"
                    data-bs-toggle="collapse"
                    data-bs-target="#faculty-trainingAccordionCollapse1"
                    aria-expanded="true" aria-controls="faculty-trainingAccordionCollapse1">
                    <asp:Literal ID="ltrItemTitle" runat="server" />
                </button>
            </p>
            <div id="faculty-trainingAccordionCollapse1" class="accordion-collapse collapse show"
                aria-labelledby="faculty-trainingAccordionHeading1"
                data-bs-parent="#faculty-trainingAccordion">
                <div class="accordion-body">

                    <asp:Repeater ID="rptBody" runat="server">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%# Eval("IsParagraph") %>'>
                                <p><%# Eval("Html") %></p>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && !(bool)Eval("IsOrdered") %>'>
                                <ul class="d-flex flex-column gap-2 mb-3">
                                    <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                        <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && (bool)Eval("IsOrdered") %>'>
                                <ol class="d-flex flex-column gap-2 mb-3">
                                    <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                        <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                    </asp:Repeater>
                                </ol>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:PlaceHolder ID="phCoords" runat="server">
                        <h3 class="h5 mb-3 mt-4"><asp:Literal ID="ltrCoordsTitle" runat="server" /></h3>
                        <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                            <table class="table table-striped mb-0">
                                <thead>
                                    <tr>
                                        <th scope="col"><asp:Literal ID="ltrThProgram" runat="server" /></th>
                                        <th scope="col"><asp:Literal ID="ltrThName" runat="server" /></th>
                                        <th scope="col"><asp:Literal ID="ltrThEmail" runat="server" /></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptCoords" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("Program") %></td>
                                                <td><%# Eval("Name") %></td>
                                                <td>
                                                    <asp:Panel runat="server" Visible='<%# Eval("HasEmail") %>'>
                                                        <a href='<%# "mailto:" + Eval("Email") %>' dir="ltr"><%# Eval("Email") %></a>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" Visible='<%# !(bool)Eval("HasEmail") %>'>-</asp:Panel>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </asp:PlaceHolder>

                </div>
            </div>
        </div>
    </div>
</section>
