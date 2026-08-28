<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContactUsDGA.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.ucContactUsDGA" %>
<%@ Import Namespace="Portal.Main.Helper" %>



<%--<h2 class="mb-4"><asp:Literal ID="ltrHeading" runat="server" /></h2>--%>
<h2 class="mb-4"><%= SPContext.Current.ListItem["Title"] %></h2>

<%-- ---------- location / contact cards ---------- --%>
<asp:PlaceHolder ID="phInfo" runat="server">
    <section aria-labelledby="faculty-contact-location-title">
        <h3 class="h5 mb-3" id="faculty-contact-location-title">
            <asp:Literal ID="ltrInfoTitle" runat="server" /></h3>
        <div class="row g-4">
            <asp:Repeater ID="rptInfo" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-6 col-xl-3">
                        <div class="card h-100">
                            <div class="card-body d-flex flex-column gap-3">
                                <div class="icon-container">
                                    <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                </div>
                                <div>
                                    <h4 class="h6"><%# Eval("Label") %></h4>

                                    <%-- link when a URL exists, plain value otherwise --%>
                                    <asp:Panel ID="pnlLink" runat="server" Visible='<%# Eval("HasLink") %>' CssClass="">
                                        <a href='<%# Eval("Url") %>' target="_blank" rel="noopener noreferrer"><%# Eval("Value") %></a>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlText" runat="server" Visible='<%# !(bool)Eval("HasLink") %>'>
                                        <p class="mb-0" dir="ltr"><%# Eval("Value") %></p>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:PlaceHolder>

<%-- ---------- offices directory ---------- --%>
<asp:PlaceHolder ID="phDirectory" runat="server">
    <section class="mt-5" aria-labelledby="faculty-contact-directory-title">
        <h3 class="h5 mb-3" id="faculty-contact-directory-title">
            <asp:Literal ID="ltrDirectoryTitle" runat="server" /></h3>
        <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
            <table class="table table-striped mb-0">
                <thead>
                    <tr>
                        <th scope="col"><asp:Literal ID="ltrThOffice" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="ltrThExtension" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="ltrThRoom" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="ltrThEmail" runat="server" /></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDirectory" runat="server" OnItemDataBound="rptDirectory_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Office") %></td>
                                <td dir="ltr">
                                    <asp:Repeater ID="rptExtensions" runat="server">
                                        <ItemTemplate>
                                            <span class="d-block"><%# Container.DataItem %></span>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                                <td dir="ltr"><%# Eval("Room") %></td>
                                <td>
                                    <asp:Panel ID="pnlEmail" runat="server" Visible='<%# Eval("HasEmail") %>'>
                                        <a href='<%# "mailto:" + Eval("Email") %>' dir="ltr"><%# Eval("Email") %></a>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlNoEmail" runat="server" Visible='<%# !(bool)Eval("HasEmail") %>'>-</asp:Panel>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </section>
</asp:PlaceHolder>

<%-- ---------- beneficiary hours ---------- --%>
<asp:PlaceHolder ID="phHours" runat="server">
    <section class="mt-5" aria-labelledby="faculty-contact-hours-title">
        <h3 class="h5 mb-3" id="faculty-contact-hours-title">
            <asp:Literal ID="ltrHoursTitle" runat="server" /></h3>
        <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
            <table class="table table-striped mb-0">
                <thead>
                    <tr>
                        <th scope="col"><asp:Literal ID="ltrThUnit" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="ltrThDay" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="ltrThTime" runat="server" /></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptHours" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Unit") %></td>
                                <td><%# Eval("Day") %></td>
                                <td><%# Eval("Time") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </section>
</asp:PlaceHolder>

<%-- ---------- unified contact form ---------- --%>
<section class="mt-5" aria-labelledby="faculty-unified-contact-title">
    <h3 class="h5 mb-3" id="faculty-unified-contact-title">
        <asp:Literal ID="ltrUnifiedTitle" runat="server" /></h3>
    <div class="row g-4">
        <div class="col-12 col-md-6">
            <div class="card nav-card h-100">
                <div class="d-flex card-body flex-column gap-4">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-message-01 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h4 class="card-title"><asp:Literal ID="ltrFormTitle" runat="server" /></h4>
                        <p class="card-text"><asp:Literal ID="ltrFormText" runat="server" /></p>
                    </div>
                    <div class="d-flex justify-content-end mt-auto">
                        <a class="btn btn-secondary stretched-link"
                           href="/<%= PortalHelper.IsArabic ? "ar" : "en" %>/Pages/ContactUsForm.aspx"
                           aria-label='<%= PortalHelper.IsArabic ? "فتح نموذج التواصل الموحد" : "Open the unified contact form" %>'>
                            <i class="hgi hgi-stroke <%= PortalHelper.IsArabic ? "hgi-arrow-left-02" : "hgi-arrow-right-02" %> fs-4" aria-hidden="true"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
