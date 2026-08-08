<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAgencyOverviewDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About.ucAgencyOverviewDga" %>

<section id="secOverview" runat="server" class="mb-5" aria-labelledby="agency-overview-title">
    <div id="pnlOverviewHeader" runat="server" class="row g-5 align-items-center mb-5">
        <div id="pnlOverviewText" runat="server" class="col-12 col-md-6 col-lg-8">
            <h2 id="hOverviewHeading" runat="server" class="mb-4">
                <asp:Literal ID="litOverviewHeading" runat="server" /></h2>
            <p id="pOverviewIntro" runat="server" class="mb-4 text-justify">
                <asp:Literal ID="litOverviewIntro" runat="server" /></p>
        </div>
        <div id="pnlOverviewImage" runat="server" class="col-12 col-md-6 col-lg-4">
            <div>
                <picture class="rounded-3">
                    <img id="imgOverview" runat="server" class="carousel-image w-100 rounded-3"
                         loading="eager" fetchpriority="high" decoding="async" />
                </picture>
            </div>
        </div>
    </div>

    <div id="pnlCardsRow" runat="server" class="row g-5">
        <div id="pnlVision" runat="server" class="col-12 col-md-6 col-sm-12">
            <div class="card h-100">
                <div class="card-body">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-target-02 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h3 id="hVisionTitle" runat="server" class="card-title h5"><asp:Literal ID="litVisionTitle" runat="server" /></h3>
                        <p id="pVisionBody" runat="server" class="card-text"><asp:Literal ID="litVisionBody" runat="server" /></p>
                    </div>
                </div>
            </div>
        </div>

        <div id="pnlMission" runat="server" class="col-12 col-md-6 col-sm-12">
            <div class="card h-100">
                <div class="card-body">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-message-01 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h3 id="hMissionTitle" runat="server" class="card-title h5"><asp:Literal ID="litMissionTitle" runat="server" /></h3>
                        <p id="pMissionBody" runat="server" class="card-text"><asp:Literal ID="litMissionBody" runat="server" /></p>
                    </div>
                </div>
            </div>
        </div>

        <div id="pnlObjectives" runat="server" class="col-12">
            <div class="card h-100">
                <div class="card-body">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-target-02 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h3 id="hObjectivesTitle" runat="server" class="card-title h5"><asp:Literal ID="litObjectivesTitle" runat="server" /></h3>
                        <ul id="ulObjectives" runat="server" class="card-text mb-0">
                            <asp:Repeater ID="rptObjectives" runat="server">
                                <ItemTemplate>
                                    <li><%# Eval("Text") %></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
