<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PNUPartners.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.PNUPartners" %>


<main id="main-content" class="dga-main-body" tabindex="-1">

 
<%-- ================== LOCAL PARTNERS ================== --%>
<section class="py-5" data-aos="fade-up" aria-labelledby="local-partners-title">
    <div class="container">
        <div>
            <h2 id="local-partners-title" class="mb-4">
                <asp:Literal ID="litLocalHeading" runat="server" />
            </h2>
            <p class="mb-4">
                <asp:Literal ID="litLocalDescription" runat="server" />
            </p>
        </div>
        <div class="d-flex flex-wrap gap-3">
            <asp:Repeater ID="rptLocalPartners" runat="server" OnItemDataBound="rptPartners_ItemDataBound">
                <ItemTemplate>
                    <div class="card text-center related-entity-card">
                        <div class="card-body placeholder-glow p-3">
                            <a runat="server" id="lnkPartner" target="_blank" rel="noopener noreferrer"
                               class="d-flex align-items-center justify-content-center h-100">
                                <img runat="server" id="imgLogo" class="img-fluid mx-auto d-block" loading="lazy" decoding="async" />
                            </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>


<%-- ================== INTERNATIONAL PARTNERS ================== --%>
<section class="pb-5" data-aos="fade-up" aria-labelledby="international-partners-title">
    <div class="container">
        <div>
            <h2 id="international-partners-title" class="mb-4">
                <asp:Literal ID="litInternationalHeading" runat="server" />
            </h2>
            <p class="mb-4">
                <asp:Literal ID="litInternationalDescription" runat="server" />
            </p>
        </div>
        <div class="d-flex flex-wrap gap-3">
            <asp:Repeater ID="rptInternationalPartners" runat="server" OnItemDataBound="rptPartners_ItemDataBound">
                <ItemTemplate>
                    <div class="card text-center related-entity-card">
                        <div class="card-body placeholder-glow p-3">
                            <a runat="server" id="lnkPartner" target="_blank" rel="noopener noreferrer"
                               class="d-flex align-items-center justify-content-center h-100">
                                <img runat="server" id="imgLogo" class="img-fluid mx-auto d-block" loading="lazy" decoding="async" />
                            </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>

    </main>


