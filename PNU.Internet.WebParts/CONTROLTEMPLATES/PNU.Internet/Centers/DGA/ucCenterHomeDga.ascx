<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCenterHomeDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA.ucCenterHomeDga" %>

<div class="d-flex flex-column gap-4">
    <!-- Overview Section -->
    <asp:Panel ID="pnlOverview" runat="server" CssClass="mb-5">
        <div class="row g-5 align-items-center mb-5">
            <div id="divOverviewText" runat="server" class="col-12 col-md-6 col-lg-8">
                <h2 id="center-overview-title" class="mb-4">
                    <asp:Literal ID="litOverviewTitle" runat="server" />
                </h2>
                <div class="mb-3 text-justify">
                    <asp:Literal ID="litOverviewText" runat="server" />
                </div>
            </div>
            <asp:Panel ID="pnlOverviewImage" runat="server" CssClass="col-12 col-md-6 col-lg-4">
                <picture class="rounded-3">
                    <img id="imgOverview" runat="server" class="carousel-image w-100 rounded-3" alt="" loading="eager" fetchpriority="high" decoding="async" />
                </picture>
            </asp:Panel>
        </div>

        <div class="row g-5">
            <!-- Vision Card -->
            <asp:Panel ID="pnlVision" runat="server" CssClass="col-12 col-md-6">
                <div class="card h-100">
                    <div class="card-body">
                        <div class="icon-container">
                            <i class="hgi hgi-stroke hgi-target-02 fs-3" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h3 class="card-title h5"><asp:Literal ID="litVisionTitle" runat="server" /></h3>
                            <p class="card-text"><asp:Literal ID="litVisionText" runat="server" /></p>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Mission Card -->
            <asp:Panel ID="pnlMission" runat="server" CssClass="col-12 col-md-6">
                <div class="card h-100">
                    <div class="card-body">
                        <div class="icon-container">
                            <i class="hgi hgi-stroke hgi-message-01 fs-3" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h3 class="card-title h5"><asp:Literal ID="litMissionTitle" runat="server" /></h3>
                            <p class="card-text"><asp:Literal ID="litMissionText" runat="server" /></p>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Objectives Card -->
            <asp:Panel ID="pnlObjectives" runat="server" CssClass="col-12">
                <div class="card h-100">
                    <div class="card-body">
                        <div class="icon-container">
                            <i class="hgi hgi-stroke hgi-target-02 fs-3" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h3 class="card-title h5"><asp:Literal ID="litObjectivesTitle" runat="server" /></h3>
                            <ul class="mb-0" id="ulObjectives" runat="server">
                                <asp:Repeater ID="rptObjectives" runat="server">
                                    <ItemTemplate>
                                        <li><%# Container.DataItem %></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Center Director's Word -->
            <asp:Panel ID="pnlDirectorWord" runat="server" CssClass="col-12">
                <section aria-labelledby="center-director-welcome-title">
                    <div class="card mb-4 bg-primary-25 border-0">
                        <div class="card-body p-4 p-lg-5">
                            <div class="d-flex flex-column gap-3">
                                <span class="icon-container bg-white">
                                    <i class="hgi hgi-stroke hgi-quote-down fs-4" aria-hidden="true"></i>
                                </span>
                                <h2 id="center-director-welcome-title" class="mb-0">
                                    <asp:Literal ID="litDirectorTitle" runat="server" />
                                </h2>
                                <p class="lead mb-0 text-justify">
                                    <asp:Literal ID="litDirectorWord" runat="server" />
                                </p>
                            </div>
                            <div class="card-body px-0 pb-0">
                                <h3 class="card-title">
                                    <asp:Literal ID="litDirectorName" runat="server" />
                                </h3>
                            </div>
                        </div>
                    </div>
                </section>
            </asp:Panel>
        </div>
    </asp:Panel>

    <!-- Center Tasks Section -->
    <asp:Panel ID="pnlTasks" runat="server" CssClass="pb-5">
        <h2 id="center-main-tasks-title" class="mb-4">
            <asp:Literal ID="litTasksHeader" runat="server" />
        </h2>
        <div class="accordion accordion-flush" id="center-main-tasks-accordion">
            <asp:Repeater ID="rptTasks" runat="server">
                <ItemTemplate>
                    <div class="accordion-item">
                        <h3 class="accordion-header" id='<%# "center-main-tasks-heading-" + Container.ItemIndex %>'>
                            <button class='<%# "accordion-button" + (Container.ItemIndex == 0 ? "" : " collapsed") %>' 
                                    type="button" data-bs-toggle="collapse"
                                    data-bs-target='<%# "#center-main-tasks-collapse-" + Container.ItemIndex %>' 
                                    aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>' 
                                    aria-controls='<%# "center-main-tasks-collapse-" + Container.ItemIndex %>'>
                                <%# Eval("Title") %>
                            </button>
                        </h3>
                        <div id='<%# "center-main-tasks-collapse-" + Container.ItemIndex %>' 
                             class='<%# "accordion-collapse collapse" + (Container.ItemIndex == 0 ? " show" : "") %>'
                             aria-labelledby='<%# "center-main-tasks-heading-" + Container.ItemIndex %>' 
                             data-bs-parent="#center-main-tasks-accordion">
                            <div class="accordion-body">
                                <p class="mb-0"><%# Eval("Description") %></p>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
</div>
