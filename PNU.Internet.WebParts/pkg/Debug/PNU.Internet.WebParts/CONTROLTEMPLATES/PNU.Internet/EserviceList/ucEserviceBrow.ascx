<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEserviceBrow.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.ucEserviceBrow" %>

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<%
    // Check the current site language once
    var isAr = SPContext.Current.Web.Language == 1025;
    string lblSearchService = isAr ? "ابحث عن خدمة" : "Search for a service";
    string lblFilter = isAr ? "تصفية" : "Filter";
    string lblFilterSearch = isAr ? "بحث" : "Search";
    string lblApply = isAr ? "تطبيق الاختيارات" : "Apply Filters";
    string lblReset = isAr ? "إعادة تعيين" : "Reset";
%>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">

        <asp:HiddenField ID="ddlFilterSelectedValue" runat="server" />

        <dga-search-input>
            <div class="d-flex gap-3 mb-4" role="search" aria-label="<%= lblSearchService %>">
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input type="text" id="txtSearch" runat="server" class="form-control" aria-describedby="input-icon" />
                </div>

                <asp:Button ID="btnSearch" runat="server" Text="<%$Resources:PNUres, Search%>" CssClass="btn btn-primary" OnClick="btnSearch_Click" />

                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false" data-bs-auto-close="outside" class="btn btn-dark gap-1">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span><%= lblFilter %></span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu px-2" style="width: 20rem;">
                            <p class="fw-semibold"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Classification %>" /></p>
                            <div class="form-control-container has-icon">
                                <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                    <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                </span>
                                <input type="text" autocomplete="off" class="form-control" placeholder="<%= lblFilterSearch %>">
                            </div>
                            <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto" style="max-height: 12.5rem;">
                                <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <hr>
                            <div class="d-flex justify-content-between gap-2 pb-2 px-2">
                                <button type="button" class="btn btn-primary"><%= lblApply %></button>
                                <button type="button" class="btn btn-secondary"><%= lblReset %></button>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </dga-search-input>

        <div class="row g-4">

            <asp:Repeater ID="rptAllData" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <span class="d-inline-flex fs-3">
                                        <i class="hgi hgi-stroke hgi-diploma fs-3" aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div>
                                    <h3 class="card-title"><%# SPFactory.GetLocalizedTitle(Eval("ARServiceName"), Eval("ENServiceName")) %></h3>
                                    <p class="card-text"><%# SPFactory.GetLocalizedTitle(Eval("Desc"), Eval("Desc_EN")) %></p>
                                </div>

                                <div class="d-flex flex-wrap mt-auto gap-2">
                                    <span class="badge badge-info"><%# SPFactory.GetLocalizedTitle(Eval("DP_TargetGroup"), Eval("DP_TargetGroup_EN")) %></span>
                                </div>

                                <div class="d-flex gap-3 flex-wrap">
                                    <a class="btn btn-outline-secondary" href="service-details.aspx?eti=<%# DataBinder.Eval(Container.DataItem, "ID") %>"><%# SPContext.Current.Web.Language == 1025 ? "تفاصيل الخدمة" : "Service Details" %></a>
                                    <a class="btn btn-primary" href="<%# DataBinder.Eval(Container.DataItem, "URL") %>" target="_blank" rel="noopener"><%# SPContext.Current.Web.Language == 1025 ? "انتقال إلى المنصة" : "Go to Platform" %></a>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>

        <nav class="mt-5" aria-label="Services navigation">
            <ul id="pagination" class="pagination justify-content-center gap-3" runat="server">
            </ul>
        </nav>

    </div>

    <div class="tab-content" id="myTabContent">
        <div class="tab-pane fade show active" style="display: none" id="all-tab-pane" role="tabpanel" aria-labelledby="all-tab" tabindex="0">
            <div class="d-flex justify-content-between align-items-baseline mb-5">
                <div>
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mt-md-0 mt-4">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, ImportantSystems %>" />
                        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                    </h1>
                </div>
                <a href="ESystems.aspx" class="btn btn-primary px-3 py-2">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, AllSystems %>" />
                </a>
            </div>

            <div class="mt-5">
                <div>
                    <div class="swiper swiper-container-eservices">
                        <div class="swiper-wrapper">
                            <asp:Repeater ID="rptSystems" runat="server">
                                <ItemTemplate>
                                    <div class="swiper-slide bg-transparent">
                                        <div class="electronic-sys-item card text-center py-5 px-3 d-flex flex-column align-items-center rounded-bottom-0">
                                            <div class="py-3">
                                                <a href="<%# String.Format("{0}", Eval("URL")) %>">
                                                    <h4 class="mb-4 fw-bolder text-dark">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                    </h4>
                                                    <p class="h6 mb-0 px-3 text-muted">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                                    </p>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="mt-5">
                            <div class="swiper-pagination mt-5"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</main>
