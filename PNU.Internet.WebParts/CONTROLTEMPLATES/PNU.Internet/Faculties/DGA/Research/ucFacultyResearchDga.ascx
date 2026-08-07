<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFacultyResearchDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.ucFacultyResearchDga" %>


<section id="faculty-research" class="pnu-section-anchor mb-5">
    <h2 class="mb-2"><%= SPContext.Current.ListItem["Title"] %></h2>
    <p><%= SPContext.Current.ListItem["Comments"] %></p>

    <asp:PlaceHolder ID="phItems" runat="server">
        <div class="accordion accordion-flush mt-3" id="faculty-researchAccordion">
            <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                <ItemTemplate>
                    <div class="accordion-item">
                        <p class="accordion-header" id='<%# Eval("HeadingId") %>'>
                            <button type="button" class='<%# Eval("ButtonClass") %>'
                                data-bs-toggle="collapse"
                                data-bs-target='<%# "#" + Eval("CollapseId") %>'
                                aria-expanded='<%# Eval("AriaExpanded") %>'
                                aria-controls='<%# Eval("CollapseId") %>'>
                                <%# Eval("Title") %>
                            </button>
                        </p>
                        <div id='<%# Eval("CollapseId") %>' class='<%# Eval("PanelClass") %>'
                            aria-labelledby='<%# Eval("HeadingId") %>'
                            data-bs-parent="#faculty-researchAccordion">
                            <div class="accordion-body">

                                <%-- item-level body --%>
                                <asp:Repeater ID="rptBody" runat="server">
                                    <ItemTemplate>
                                        <asp:Panel runat="server" Visible='<%# Eval("IsParagraph") %>'>
                                            <p><%# Eval("Html") %></p>
                                        </asp:Panel>
                                        <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && !(bool)Eval("IsOrdered") %>'>
                                            <ul class="d-flex flex-column gap-2 mb-4">
                                                <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                                    <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </asp:Panel>
                                        <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && (bool)Eval("IsOrdered") %>'>
                                            <ol class="d-flex flex-column gap-2 mb-4">
                                                <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                                    <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                                </asp:Repeater>
                                            </ol>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <%-- h3 sub-sections --%>
                                <asp:Repeater ID="rptSections" runat="server" OnItemDataBound="rptSections_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:Panel runat="server" Visible='<%# Eval("HasTitle") %>'>
                                            <h3 class="h5 mt-4 mb-3"><%# Eval("Title") %></h3>
                                        </asp:Panel>

                                        <asp:Repeater ID="rptSectionBody" runat="server">
                                            <ItemTemplate>
                                                <asp:Panel runat="server" Visible='<%# Eval("IsParagraph") %>'>
                                                    <p><%# Eval("Html") %></p>
                                                </asp:Panel>
                                                <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && !(bool)Eval("IsOrdered") %>'>
                                                    <ul class="d-flex flex-column gap-2 mb-4">
                                                        <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                                            <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </asp:Panel>
                                                <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && (bool)Eval("IsOrdered") %>'>
                                                    <ol class="d-flex flex-column gap-2 mb-4">
                                                        <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                                            <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                                        </asp:Repeater>
                                                    </ol>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <%-- h4 sub-items (units / services / labs) --%>
                                        <asp:Repeater ID="rptSubItems" runat="server" OnItemDataBound="rptSubItems_ItemDataBound">
                                            <ItemTemplate>
                                                <h4 class="h6"><%# Eval("Title") %></h4>
                                                <asp:Repeater ID="rptSubBody" runat="server">
                                                    <ItemTemplate>
                                                        <asp:Panel runat="server" Visible='<%# Eval("IsParagraph") %>'>
                                                            <p><%# Eval("Html") %></p>
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && !(bool)Eval("IsOrdered") %>'>
                                                            <ul class="d-flex flex-column gap-2 mb-4">
                                                                <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                                                    <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") && (bool)Eval("IsOrdered") %>'>
                                                            <ol class="d-flex flex-column gap-2 mb-4">
                                                                <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                                                    <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                                                </asp:Repeater>
                                                            </ol>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Panel runat="server" Visible='<%# Eval("HasLink") %>' CssClass="mb-4">
                                                    <a class="btn btn-secondary" href='<%# Eval("LinkUrl") %>'
                                                       target="_blank" rel="noopener noreferrer">
                                                        <%# Eval("LinkText") %>
                                                        <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                                                    </a>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <%-- optional table (e.g. أجهزة المركز) --%>
                                        <asp:PlaceHolder ID="phTable" runat="server">
                                            <div class="table-responsive border border-top-0 border-bottom-0 rounded-2 mb-4">
                                                <table class="table table-striped mb-0">
                                                    <thead>
                                                        <tr>
                                                            <asp:Repeater ID="rptTableHead" runat="server">
                                                                <ItemTemplate><th scope="col"><%# Container.DataItem %></th></ItemTemplate>
                                                            </asp:Repeater>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <%-- Cells are data-driven: the row renders exactly as many
                                                             columns as the sub-section declares in TableHeaders
                                                             (up to five), so the header and body always line up. --%>
                                                        <asp:Repeater ID="rptTableRows" runat="server" OnItemDataBound="rptTableRows_ItemDataBound">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <asp:Repeater ID="rptCells" runat="server">
                                                                        <ItemTemplate><td><%# Container.DataItem %></td></ItemTemplate>
                                                                    </asp:Repeater>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </asp:PlaceHolder>

                                        <%-- sub-section images, grid as per design --%>
                                        <asp:PlaceHolder ID="phSectionImages" runat="server">
                                            <div class="row g-3 mb-4">
                                                <asp:Repeater ID="rptSectionImages" runat="server">
                                                    <ItemTemplate>
                                                        <div class='<%# Eval("ColClass") %>'>
                                                            <img class="img-fluid rounded-3" src='<%# Eval("Url") %>'
                                                                 alt='<%# Eval("Alt") %>' loading="lazy" decoding="async" />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </asp:PlaceHolder>

                                        <asp:Panel runat="server" Visible='<%# Eval("HasLink") %>' CssClass="mb-4">
                                            <a class="btn btn-secondary" href='<%# Eval("LinkUrl") %>'
                                               target="_blank" rel="noopener noreferrer">
                                                <%# Eval("LinkText") %>
                                                <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                                            </a>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <%-- item-level images --%>
                                <asp:PlaceHolder ID="phItemImages" runat="server">
                                    <div class="row g-3 mt-2">
                                        <asp:Repeater ID="rptImages" runat="server">
                                            <ItemTemplate>
                                                <div class='<%# Eval("ColClass") %>'>
                                                    <img class="img-fluid rounded-3" src='<%# Eval("Url") %>'
                                                         alt='<%# Eval("Alt") %>' loading="lazy" decoding="async" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:PlaceHolder>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:PlaceHolder>
</section>
