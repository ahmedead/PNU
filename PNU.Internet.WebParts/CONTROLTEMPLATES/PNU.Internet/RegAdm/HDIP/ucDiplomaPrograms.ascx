<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDiplomaPrograms.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.HDIP.ucDiplomaPrograms" %>


<main id="main-content" class="dga-main-body" tabindex="-1">

    <!-- Overview -->
    <section class="py-5" data-aos="fade-up" aria-labelledby="diploma-overview-title">
        <div class="container">
            <div class="row g-5 align-items-center">
                <div class="col-12 col-md-12 col-lg-12">
                    <h2 id="diploma-overview-title" class="mb-4"><asp:Literal ID="litOverviewTitle" runat="server" /></h2>
                    <p class="mb-4 text-justify"><asp:Literal ID="litOverviewDesc" runat="server" /></p>
                    <ul>
                        <asp:Repeater ID="rptBullets" runat="server">
                            <ItemTemplate><li><%# Container.DataItem %></li></ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="col-12 col-lg-12" id="divImage" runat="server">
                    <figure class="figure w-100 mb-0">
                        <div class="overflow-hidden rounded-3 shadow-sm">
                            <picture>
                                <img class="w-100 object-fit-cover" alt="" loading="eager" fetchpriority="high"
                                     decoding="async" id="imgOverview" runat="server" />
                            </picture>
                        </div>
                        <figcaption class="figure-caption mt-3 mb-0"><asp:Literal ID="litImageCaption" runat="server" /></figcaption>
                    </figure>
                </div>
            </div>
        </div>
    </section>

    <!-- Nav Cards -->
    <section class="py-5" data-aos="fade-up">
        <div class="container">
            <div class="mb-4">
                <h2 class="mb-0"><asp:Literal ID="litLinksTitle" runat="server" /></h2>
            </div>
            <div class="row g-4">
                <asp:Repeater ID="rptLinks" runat="server">
                    <ItemTemplate>
                        <div class="col-12 col-md-6 col-lg-4">
                            <div class="card nav-card h-100">
                                <div class="d-flex card-body flex-column gap-4">
                                    <div class="icon-container">
                                        <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                    </div>
                                    <div>
                                        <h3 class="card-title"><%# Eval("Title") %></h3>
                                        <p class="card-text"><%# Eval("Description") %></p>
                                    </div>
                                    <div class="d-flex justify-content-end mt-auto">
                                        <a class="btn btn-secondary stretched-link" href='<%# Eval("LinkUrl") %>'
                                           target="_blank" rel="noopener noreferrer" aria-label='<%# Eval("Title") %>'>
                                            <i class="hgi hgi-stroke <%# Eval("ArrowClass") %> fs-4" aria-hidden="true"></i>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</main>