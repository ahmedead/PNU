<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAgencyOverviewDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About.ucAgencyOverviewDga" %>

<section class="mb-5" aria-labelledby="agency-overview-title">
    <div class="row g-5 align-items-center mb-5">
        <div class="col-12 col-md-6 col-lg-8">
            <h2 id="agency-overview-title" class="mb-4">
                <asp:Literal ID="litOverviewHeading" runat="server" /></h2>
            <p class="mb-4 text-justify">
                <asp:Literal ID="litOverviewIntro" runat="server" /></p>
        </div>
        <div class="col-12 col-md-6 col-lg-4">
            <div>
                <picture class="rounded-3">
                    <img id="imgOverview" runat="server" class="carousel-image w-100 rounded-3"
                         loading="eager" fetchpriority="high" decoding="async" />
                </picture>
            </div>
        </div>
    </div>

    <div class="row g-5">
        <div class="col-12 col-md-6 col-sm-12">
            <div class="card h-100">
                <div class="card-body">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-target-02 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h3 class="card-title h5"><asp:Literal ID="litVisionTitle" runat="server" /></h3>
                        <p class="card-text"><asp:Literal ID="litVisionBody" runat="server" /></p>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-12 col-md-6 col-sm-12">
            <div class="card h-100">
                <div class="card-body">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-message-01 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h3 class="card-title h5"><asp:Literal ID="litMissionTitle" runat="server" /></h3>
                        <p class="card-text"><asp:Literal ID="litMissionBody" runat="server" /></p>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-12">
            <div class="card h-100">
                <div class="card-body">
                    <div class="icon-container">
                        <i class="hgi hgi-stroke hgi-target-02 fs-3" aria-hidden="true"></i>
                    </div>
                    <div>
                        <h3 class="card-title h5"><asp:Literal ID="litObjectivesTitle" runat="server" /></h3>
                        <ul class="card-text mb-0">
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
