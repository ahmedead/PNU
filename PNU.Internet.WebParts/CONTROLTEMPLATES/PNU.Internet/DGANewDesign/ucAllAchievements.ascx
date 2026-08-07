<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllAchievements.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.ucAllAchievements" %>




 <%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="py-5" data-aos="fade-up" aria-labelledby="achievements-section-title">
    <div class="container">

        <ul class="nav nav-tabs nav-underline nav-flush w-100 mb-4" id="achievementsTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 active d-inline-flex align-items-center gap-2 bg-transparent px-2"
                    id="achievements-tab" data-bs-toggle="tab" data-bs-target="#achievements-pane" type="button"
                    role="tab" aria-controls="achievements-pane" aria-selected="true">
                    <i class="hgi hgi-stroke hgi-elearning-exchange fs-5 fw-light" aria-hidden="true"></i>
                    <span>
                        <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,Achievements%>" EncodeMethod="HtmlEncode" />
                    </span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2"
                    id="awards-tab" data-bs-toggle="tab" data-bs-target="#awards-pane" type="button" role="tab"
                    aria-controls="awards-pane" aria-selected="false">
                    <i class="hgi hgi-stroke hgi-certificate-02 fs-5 fw-light" aria-hidden="true"></i>
                    <span>
                        <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,Awards%>" EncodeMethod="HtmlEncode" />
                    </span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2"
                    id="rankings-tab" data-bs-toggle="tab" data-bs-target="#rankings-pane" type="button" role="tab"
                    aria-controls="rankings-pane" aria-selected="false">
                    <i class="hgi hgi-stroke hgi-books-02 fs-5 fw-light" aria-hidden="true"></i>
                    <span>
                        <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,Rankings%>" EncodeMethod="HtmlEncode" />
                    </span>
                </button>
            </li>
        </ul>

        <div class="tab-content pt-2">

            <%-- Achievements Tab --%>
            <div class="tab-pane fade show active" id="achievements-pane" role="tabpanel" aria-labelledby="achievements-tab">
                <div class="row g-4">
                    <asp:Repeater ID="rptAchievements" runat="server">
                        <ItemTemplate>
                            <div class="col-12 col-lg-4 col-md-6">
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column placeholder-glow h-100">
                                        <img width="400" height="250" class="rounded-2 js-medium-zoom"
                                            alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                            loading="lazy"
                                            src='<%# Eval("ImageUrl") %>'
                                            decoding="async" />
                                        <div class="flex-grow-1">
                                            <h3 class="card-title">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                            </h3>
                                            <p class="card-text line-clamp max-clamp-line-4">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                            </p>
                                        </div>
                                        <div class="mt-auto d-flex flex-column gap-3">
                                            <small class="d-flex gap-2 align-items-center">
                                                <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                <time datetime='<%# Eval("DisplayDate") %>'>
                                                    <%# SPFactory.GetLocalizedTitle(Eval("DisplayDate"), Eval("DisplayDate_EN")) %>
                                                </time>
                                            </small>
                                        </div>
                                    </div>
                                </article>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:PlaceHolder ID="pnlNoAchievements" runat="server" Visible="false">
                        <div class="col-12 text-center py-5">
                            <p class="text-muted h6">
                                <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,NoDataAvailable%>" EncodeMethod="HtmlEncode" />
                            </p>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>

            <%-- Awards Tab --%>
            <div class="tab-pane fade" id="awards-pane" role="tabpanel" aria-labelledby="awards-tab">
                <div class="row g-4">
                    <asp:Repeater ID="rptAwards" runat="server">
                        <ItemTemplate>
                            <div class="col-12 col-lg-4 col-md-6">
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column placeholder-glow h-100">
                                        <img width="400" height="250" class="rounded-2 js-medium-zoom"
                                            alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                            loading="lazy"
                                            src='<%# Eval("ImageUrl") %>'
                                            decoding="async" />
                                        <div class="flex-grow-1">
                                            <h3 class="card-title">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                            </h3>
                                            <p class="card-text line-clamp max-clamp-line-4">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                            </p>
                                        </div>
                                        <div class="mt-auto d-flex flex-column gap-3">
                                            <small class="d-flex gap-2 align-items-center">
                                                <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                <time datetime='<%# Eval("DisplayDate") %>'>
                                                    <%# SPFactory.GetLocalizedTitle(Eval("DisplayDate"), Eval("DisplayDate_EN")) %>
                                                </time>
                                            </small>
                                        </div>
                                    </div>
                                </article>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:PlaceHolder ID="phNoAwards" runat="server" Visible="false">
                        <div class="col-12 text-center py-5">
                            <p class="text-muted h6">
                                <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,NoDataAvailable%>" EncodeMethod="HtmlEncode" />
                            </p>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>

            <%-- Rankings Tab --%>
            <div class="tab-pane fade" id="rankings-pane" role="tabpanel" aria-labelledby="rankings-tab">
                <div class="row g-4">
                    <asp:Repeater ID="rptRankings" runat="server">
                        <ItemTemplate>
                            <div class="col-12 col-lg-4 col-md-6">
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column placeholder-glow h-100">
                                        <img width="400" height="250" class="rounded-2 js-medium-zoom"
                                            alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                            loading="lazy"
                                            src='<%# Eval("ImageUrl") %>'
                                            decoding="async" />
                                        <div class="flex-grow-1">
                                            <h3 class="card-title">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                            </h3>
                                            <p class="card-text line-clamp max-clamp-line-4">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                            </p>
                                        </div>
                                        <div class="mt-auto d-flex flex-column gap-3">
                                            <small class="d-flex gap-2 align-items-center">
                                                <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                <time datetime='<%# Eval("DisplayDate") %>'>
                                                    <%# SPFactory.GetLocalizedTitle(Eval("DisplayDate"), Eval("DisplayDate_EN")) %>
                                                </time>
                                            </small>
                                        </div>
                                    </div>
                                </article>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:PlaceHolder ID="pnlNoRankings" runat="server" Visible="false">
                        <div class="col-12 text-center py-5">
                            <p class="text-muted h6">
                                <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,NoDataAvailable%>" EncodeMethod="HtmlEncode" />
                            </p>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>

        </div>
    </div>
</section>