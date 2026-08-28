<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAboutPNU.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.ucAboutPNU" %>

<main id="main-content" class="dga-main-body" tabindex="-1">

    <section class="py-5" data-aos="fade-up" aria-labelledby="reasons-section-title">
        <div class="container">
            <div class="row g-4 g-xl-5 align-items-center">
                <div class="col-12 col-lg-12">
                    <h2 id="reasons-section-title" class="mb-4">
                        <asp:Literal ID="litHistoryTitle" runat="server" />
                    </h2>
                    <div class="mb-4 about-history-narrative">
                        <asp:Literal ID="litHistoryDescription" runat="server" />
                    </div>

                    <div class="row g-3">
                        <asp:Repeater ID="rptMilestones" runat="server" OnItemDataBound="rptMilestones_ItemDataBound">
                            <ItemTemplate>
                                <div class="col-12 col-md-4">
                                    <article class="card h-100 pnu-news-card">
                                        <div class="card-body d-flex flex-column placeholder-glow h-100">
                                            <div class="icon-container">
                                                <span class="d-inline-flex fs-3">
                                                    <i id="iconEl" runat="server" class="hgi hgi-stroke hgi-target-01" aria-hidden="true"></i>
                                                </span>
                                            </div>
                                            <div class="flex-grow-1">
                                                <h3 class="card-title"><asp:Literal ID="litTitle" runat="server" /></h3>
                                                <p class="card-text"><asp:Literal ID="litDesc" runat="server" /></p>
                                            </div>
                                            <div class="mt-auto d-flex flex-column gap-3">
                                                <small class="d-flex gap-2 align-items-center">
                                                    <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                    <time id="timeEl" runat="server"><asp:Literal ID="litPeriod" runat="server" /></time>
                                                </small>
                                            </div>
                                        </div>
                                    </article>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <asp:Panel ID="pnlFacility" runat="server" CssClass="col-12 col-lg-12 d-none d-lg-block">
                    <figure class="figure w-100 mb-0">
                        <div class="overflow-hidden rounded-3 shadow-sm">
                            <picture>
                                <asp:Image ID="imgFacility" runat="server" CssClass="w-100 object-fit-cover"
                                           loading="eager" fetchpriority="high" decoding="async" />
                            </picture>
                        </div>
                        <figcaption class="figure-caption mt-3 mb-0">
                            <asp:Literal ID="litImageCaption" runat="server" />
                        </figcaption>
                    </figure>
                </asp:Panel>
            </div>
        </div>
    </section>

    <section class="gray colored-section py-5" data-aos="fade-up">
        <div class="container">
            <div class="mb-4">
                <div class="d-flex justify-content-between align-items-start gap-2">
                    <h2 class="mb-0"><asp:Literal ID="litPillarsTitle" runat="server" /></h2>
                </div>
                <p class="mb-0 mt-3"><asp:Literal ID="litPillarsSubtitle" runat="server" /></p>
            </div>
            <div class="row g-4">
                <asp:Repeater ID="rptPillars" runat="server" OnItemDataBound="rptPillars_ItemDataBound">
                    <ItemTemplate>
                        <div class="col-12 col-lg-4 col-md-6">
                            <article class="card h-100">
                                <div class="card-body d-flex flex-column gap-4">
                                    <div class="icon-container">
                                        <span class="d-inline-flex fs-3">
                                            <i id="iconEl" runat="server" class="hgi hgi-stroke hgi-target-01" aria-hidden="true"></i>
                                        </span>
                                    </div>
                                    <div>
                                        <h3 class="card-title"><asp:Literal ID="litTitle" runat="server" /></h3>
                                        <p class="card-text"><asp:Literal ID="litDesc" runat="server" /></p>
                                    </div>
                                </div>
                            </article>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
    
</main>