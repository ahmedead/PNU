<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCenterContactDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA.ucCenterContactDga" %>

<h2 class="mb-4">
    <asp:Literal ID="litMainHeading" runat="server" />
</h2>

<!-- 1. Contact Information Cards -->
<section aria-labelledby="center-contact-location-title" class="mb-5">
    <h3 class="h5 mb-3" id="center-contact-location-title">
        <asp:Literal ID="litInfoTitle" runat="server" />
    </h3>
    <div class="row g-4">
        <!-- Email -->
        <asp:Panel ID="pnlEmailCard" runat="server" CssClass="col-12 col-md-6">
            <div class="card h-100">
                <div class="card-body d-flex flex-column gap-3">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-mail-01 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h4 class="h6"><asp:Literal ID="litEmailLabel" runat="server" /></h4>
                        <asp:Literal ID="litEmailValue" runat="server" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- Phone -->
        <asp:Panel ID="pnlPhoneCard" runat="server" CssClass="col-12 col-md-6">
            <div class="card h-100">
                <div class="card-body d-flex flex-column gap-3">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-call fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h4 class="h6"><asp:Literal ID="litPhoneLabel" runat="server" /></h4>
                        <asp:Literal ID="litPhoneValue" runat="server" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- Location -->
        <asp:Panel ID="pnlLocationCard" runat="server" CssClass="col-12">
            <div class="card h-100">
                <div class="card-body d-flex flex-column gap-3">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-location-01 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h4 class="h6"><asp:Literal ID="litLocationLabel" runat="server" /></h4>
                        <p class="mb-0"><asp:Literal ID="litLocationValue" runat="server" /></p>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</section>

<!-- 2. Contact Directory Table -->
<asp:Panel ID="pnlDirectory" runat="server" CssClass="mt-5 mb-5">
    <section aria-labelledby="center-contact-directory-title">
        <h3 class="h5 mb-3" id="center-contact-directory-title">
            <asp:Literal ID="litDirectoryTitle" runat="server" />
        </h3>
        <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
            <table class="table table-striped mb-0">
                <thead>
                    <tr>
                        <th scope="col"><asp:Literal ID="litThEntity" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="litThPhone" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="litThEmail" runat="server" /></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDirectory" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("EntityName") %></td>
                                <td dir="ltr"><%# Eval("Phone") %></td>
                                <td><%# Eval("EmailHtml") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </section>
</asp:Panel>

<!-- 3. Unified Contact Form -->
<section class="mt-5" aria-labelledby="center-unified-contact-title">
    <h3 class="h5 mb-3" id="center-unified-contact-title">
        <asp:Literal ID="litUnifiedTitle" runat="server" />
    </h3>
    <div class="row g-4">
        <div class="col-12 col-md-6">
            <div class="card nav-card h-100">
                <div class="d-flex card-body flex-column gap-4">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-message-01 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h4 class="card-title"><asp:Literal ID="litFormCardTitle" runat="server" /></h4>
                        <p class="card-text"><asp:Literal ID="litFormCardDesc" runat="server" /></p>
                    </div>
                    <div class="d-flex justify-content-end mt-auto">
                        <a id="lnkContactForm" runat="server" class="btn btn-secondary stretched-link" href="/ar/Tawasul/Pages/ContactUS.aspx" aria-label="فتح نموذج التواصل الموحد">
                            <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
