<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAchievements.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucAchievements" %>



 <%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="news-and-events my-1 py-1 py-md-5 my-md-5">
    <div class="container">
        <div class="d-flex justify-content-between align-items-baseline mb-5">
            <div>
                <h2 class="title text-dark fw-bold px-2 border-start border-primary mt-md-0 mt-4">
                    <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,AchievementsAndAwards%>" EncodeMethod="HtmlEncode"/>
                </h2>
            </div>
            <div id="divViewMore" runat="server">
                <a href="AchievementsArchive.aspx" class="btn btn-outline-primary px-3 py-2">
                    <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,ViewMore%>" EncodeMethod="HtmlEncode"/>
                </a>
            </div>
        </div>

        <div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">
            <div class="col-lg-6 tabbable">
                <ul class="nav nav-tabs nav-fill nav-pills bg-sa-50 border-0 shadow-none p-1 rounded-2" id="achievementsTab" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" id="news1-tab" data-bs-toggle="tab" data-bs-target="#news1-tab-pane" type="button" role="tab" aria-controls="news1-tab-pane" aria-selected="true">
                            <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,Achievements%>" EncodeMethod="HtmlEncode"/>
                        </button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="news2-tab" data-bs-toggle="tab" data-bs-target="#news2-tab-pane" type="button" role="tab" aria-controls="news2-tab-pane" aria-selected="false">
                            <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,Awards%>" EncodeMethod="HtmlEncode"/>
                        </button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="news3-tab" data-bs-toggle="tab" data-bs-target="#news3-tab-pane" type="button" role="tab" aria-controls="news3-tab-pane" aria-selected="false">
                            <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,Rankings%>" EncodeMethod="HtmlEncode"/>
                        </button>
                    </li>
                </ul>
            </div>
        </div>

        <div class="tab-content" id="newsTabsContent">
            
            <div class="tab-pane fade show active" id="news1-tab-pane" role="tabpanel" aria-labelledby="news1-tab" tabindex="0">
                <div class="row">
                    <asp:Repeater ID="rptAchievements" runat="server">
                        <ItemTemplate>
                            <div class="col-lg-4 col-md-6 col-12">
                                <div class="card mb-4 p-3 exhibition-item shadow-none rounded-4 border-0">
                                    <div class="row g-0">
                                        <div class="col-md-12">
                                            <img loading="lazy" src='<%# Eval("ImageUrl") %>' class="img-fluid rounded-4 w-100" alt='<%# Eval("Title") %>'>
                                            <div class="card-body mt-2">
                                                <p class="mb-3 card-text text-primary fw-bolder">
                                                    
                                                     <%# SPFactory.GetLocalizedTitle(Eval("DisplayDate"), Eval("DisplayDate_EN")) %>
                                                </p>
                                                <h3 class="card-title mb-3 fw-bold h5 text-dark">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                </h3>
                                                <p class="card-text mb-4 text-muted">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:PlaceHolder ID="pnlNoAchievements" runat="server" Visible="false">
    <div class="col-12 text-center py-5">
        <p class="text-muted h6"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,NoDataAvailable%>" EncodeMethod="HtmlEncode"/></p>
    </div>
</asp:PlaceHolder>
                </div>
            </div>

            <div class="tab-pane fade" id="news2-tab-pane" role="tabpanel" aria-labelledby="news2-tab" tabindex="0">
                <div class="row">
                    <asp:Repeater ID="rptAwards" runat="server">
                        <ItemTemplate>
                            <div class="col-lg-4 col-md-6 col-12">
                                <div class="card mb-4 p-3 exhibition-item shadow-none rounded-4 border-0">
                                    <div class="row g-0">
                                        <div class="col-md-12">
                                            <img loading="lazy"  src='<%# Eval("ImageUrl") %>' class="img-fluid rounded-4 w-100" alt='<%# Eval("Title") %>'>
                                            <div class="card-body mt-2">
                                                <p class="mb-3 card-text text-primary fw-bolder">
                                                   <%# SPFactory.GetLocalizedTitle(Eval("DisplayDate"), Eval("DisplayDate_EN")) %>
                                                </p>
                                                <h3 class="card-title mb-3 fw-bold h5 text-dark">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                </h3>
                                                <p class="card-text mb-4 text-muted">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:PlaceHolder ID="phNoAwards" runat="server" Visible="false">
                        <div class="col-12 text-center py-5">
                            <p class="text-muted h6"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,NoDataAvailable%>" EncodeMethod="HtmlEncode"/></p>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>

            <div class="tab-pane fade" id="news3-tab-pane" role="tabpanel" aria-labelledby="news3-tab" tabindex="0">
                <div class="row">
                    <asp:Repeater ID="rptRankings" runat="server">
                        <ItemTemplate>
                            <div class="col-lg-4 col-md-6 col-12">
                                <div class="card mb-4 p-3 exhibition-item shadow-none rounded-4 border-0">
                                    <div class="row g-0">
                                        <div class="col-md-12">
                                            <img loading="lazy"  src='<%# Eval("ImageUrl") %>' class="img-fluid rounded-4 w-100" alt='<%# Eval("Title") %>'>
                                            <div class="card-body mt-2">
                                                <p class="mb-3 card-text text-primary fw-bolder">
                                                   <%# SPFactory.GetLocalizedTitle(Eval("DisplayDate"), Eval("DisplayDate_EN")) %>
                                                </p>
                                                <h3 class="card-title mb-3 fw-bold h5 text-dark">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                </h3>
                                                <p class="card-text mb-4 text-muted">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:PlaceHolder ID="pnlNoRankings" runat="server" Visible="false">
    <div class="col-12 text-center py-5">
        <p class="text-muted h6"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,NoDataAvailable%>" EncodeMethod="HtmlEncode"/></p>
    </div>
</asp:PlaceHolder>
                </div>
            </div>

        </div>
    </div>
</section>