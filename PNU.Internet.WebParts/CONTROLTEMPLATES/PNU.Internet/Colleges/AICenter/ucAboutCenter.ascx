<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAboutCenter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAboutCenter" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<div class="d-flex flex-column gap-4">
    <asp:Repeater ID="rptMainData" runat="server">
        <ItemTemplate>
            <!-- About Section -->
            <section class="mb-4" aria-labelledby="ai-center-about-title">
                <h2 id="ai-center-about-title" class="mb-4">
                    <%# SPFactory.GetLocalizedTitle(Eval("AboutTitle"), Eval("AboutTitle")) %>
                </h2>
                <div class="lead mb-4 text-justify text-muted lh-base">
                    <%# SPFactory.GetLocalizedTitle(Eval("AboutText"), Eval("AboutText")) %>
                </div>
            </section>

            <!-- Vision & Mission Cards -->
            <div class="row g-4 mb-4">
                <div class="col-12 col-md-6">
                    <div class="card h-100 border-0 bg-primary-25 rounded-3">
                        <div class="card-body p-4 d-flex flex-column gap-3">
                            <div class="icon-container bg-white">
                                <i class="hgi hgi-stroke hgi-target-02 fs-3 text-primary" aria-hidden="true"></i>
                            </div>
                            <div>
                                <h3 class="card-title h5 mb-2 fw-semibold">
                                    <%# SPFactory.GetLocalizedTitle(Eval("VisionTitle"), Eval("VisionTitle")) %>
                                </h3>
                                <p class="card-text text-muted mb-0 lh-base">
                                    <%# SPFactory.GetLocalizedTitle(Eval("VisionText"), Eval("VisionText")) %>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-12 col-md-6">
                    <div class="card h-100 border-0 bg-primary-25 rounded-3">
                        <div class="card-body p-4 d-flex flex-column gap-3">
                            <div class="icon-container bg-white">
                                <i class="hgi hgi-stroke hgi-flag-02 fs-3 text-primary" aria-hidden="true"></i>
                            </div>
                            <div>
                                <h3 class="card-title h5 mb-2 fw-semibold">
                                    <%# SPFactory.GetLocalizedTitle(Eval("MessageTitle"), Eval("MessageTitle")) %>
                                </h3>
                                <p class="card-text text-muted mb-0 lh-base">
                                    <%# SPFactory.GetLocalizedTitle(Eval("MessageText"), Eval("MessageText")) %>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <!-- Strategic Goals Card -->
    <div class="card h-100 border rounded-3 mb-4">
        <div class="card-body p-4">
            <div class="d-flex align-items-center gap-3 mb-4">
                <div class="icon-container">
                    <i class="hgi hgi-stroke hgi-checkmark-circle-02 fs-3 text-primary" aria-hidden="true"></i>
                </div>
                <div>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <h3 class="card-title h5 mb-0 fw-semibold">
                                <%# SPFactory.GetLocalizedTitle(Eval("StrategicGoalsTitle"), Eval("StrategicGoalsTitle")) %>
                            </h3>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <ul class="mb-0 list-unstyled d-flex flex-column gap-3">
                <asp:Repeater ID="rptStrategicPlan" runat="server">
                    <ItemTemplate>
                        <li class="d-flex align-items-start gap-2">
                            <i class="hgi hgi-stroke hgi-arrow-left-01 text-primary mt-1 flex-shrink-0" aria-hidden="true"></i>
                            <span class="text-muted lh-base">
                                <strong><%# SPFactory.GetLocalizedTitle(Eval("DisplayNo"), Eval("DisplayNo_EN")) %> - </strong>
                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                            </span>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
</div>
