<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFacultyInitiativesDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.ucFacultyInitiativesDga" %>


<section id="faculty-initiatives" class="pnu-section-anchor mb-5">
    <h2 class="mb-2"><%= SPContext.Current.ListItem["Title"] %></h2>
<p><%= SPContext.Current.ListItem["Comments"] %></p>

    <asp:PlaceHolder ID="phItems" runat="server">
        <div class="accordion accordion-flush mt-3" id="faculty-initiativesAccordion">
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
                            data-bs-parent="#faculty-initiativesAccordion">
                            <div class="accordion-body">

                                <%-- body blocks: paragraph OR bulleted OR numbered list --%>
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

                                <%-- prose sub-sections (الأهداف / المزايا / المسارات / الشروط / آلية التقديم) --%>
                                <asp:Repeater ID="rptSections" runat="server">
                                    <ItemTemplate>
                                        <h3 class="h5 mt-4 mb-3"><%# Eval("Title") %></h3>
                                        <asp:Repeater runat="server" DataSource='<%# Eval("Body") %>'>
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
                                    </ItemTemplate>
                                </asp:Repeater>

                                <%-- معايير المفاضلة (scoring criteria table) --%>
                                <asp:PlaceHolder ID="phCriteria" runat="server">
                                    <h3 class="h5 mt-4 mb-3"><asp:Literal ID="ltrCriteriaTitle" runat="server" Text="معايير المفاضلة" /></h3>
                                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                                        <table class="table table-striped mb-0">
                                            <thead>
                                                <tr>
                                                    <th scope="col">المعيار</th>
                                                    <th scope="col">التفاصيل</th>
                                                    <th scope="col">الدرجة القصوى</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptCriteria" runat="server" OnItemDataBound="rptCriteria_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <th scope="row"><%# Eval("Criterion") %></th>
                                                            <td>
                                                                <asp:Repeater ID="rptDetails" runat="server">
                                                                    <ItemTemplate>
                                                                        <asp:Panel runat="server" Visible='<%# Eval("IsParagraph") %>'>
                                                                            <p class="mb-0"><%# Eval("Html") %></p>
                                                                        </asp:Panel>
                                                                        <asp:Panel runat="server" Visible='<%# (bool)Eval("IsList") %>'>
                                                                            <ul class="mb-0">
                                                                                <asp:Repeater runat="server" DataSource='<%# Eval("ItemsHtml") %>'>
                                                                                    <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </ul>
                                                                        </asp:Panel>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                            <td><%# Eval("MaxScore") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </asp:PlaceHolder>

                                <%-- طالبات قائمة العميد (roster grouped by track) --%>
                                <asp:PlaceHolder ID="phNames" runat="server">
                                    <h3 class="h5 mt-4 mb-3"><asp:Literal ID="ltrNamesTitle" runat="server" Text="طالبات قائمة العميد" /></h3>
                                    <div class="row g-4">
                                        <asp:Repeater ID="rptNameGroups" runat="server" OnItemDataBound="rptNameGroups_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="col-lg-6">
                                                    <h4 class="h6"><%# Eval("Track") %></h4>
                                                    <ul class="mb-0">
                                                        <asp:Repeater ID="rptNames" runat="server">
                                                            <ItemTemplate><li><%# Eval("StudentName") %></li></ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:PlaceHolder>

                                <%-- images belonging to this item --%>
                                <asp:Repeater ID="rptImages" runat="server">
                                    <ItemTemplate>
                                        <img class="img-fluid rounded-3 mt-3" src='<%# Container.DataItem %>' alt="" loading="lazy" decoding="async" />
                                    </ItemTemplate>
                                </asp:Repeater>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:PlaceHolder>
</section>
