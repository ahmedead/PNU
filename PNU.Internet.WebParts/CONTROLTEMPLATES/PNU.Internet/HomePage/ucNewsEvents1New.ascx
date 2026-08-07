<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsEvents1New.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucNewsEvents1New" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="news-and-events my-1 py-1 py-md-5 my-md-5">
    <div class="container">
        <div class="row justify-content-between align-items-center mb-5">
            <div class="col-lg-4 col-6">
                <h2 class="title text-dark fw-bold px-2 border-start border-primary mb-0">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_NewsAndEvents %>" />
                </h2>
            </div>
            <div class="col-lg-2 col-6 text-end">
                <div class="dropdown">
                    <button class="btn btn-outline-primary dropdown-toggle px-3 py-2" type="button"
                        id="moreNewsEventsDropdown" data-bs-toggle="dropdown" aria-expanded="false">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_ShowMore %>" />
                    </button>
                    <ul class="dropdown-menu shadow border-0" aria-labelledby="moreNewsEventsDropdown">
                        <%-- Note: These links are pulled from the first item in the master repeater context --%>
                        <li><a class="dropdown-item" href='<%= SPFactory.GetSiteURL() %>MediaCenter/Pages/AllNews.aspx?Id=1'><asp:Literal runat="server" Text="<%$ Resources: PNUres, res_MoreAllNews %>" /></a></li>
                        <li><a class="dropdown-item" href='<%= SPFactory.GetSiteURL() %>MediaCenter/Pages/LatestAdvertisements.aspx'><asp:Literal runat="server" Text="<%$ Resources: PNUres, res_MoreAllAdvsLink %>" /></a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="row">
            <asp:Repeater ID="detailsRepeater" runat="server">
                <ItemTemplate>
                    <div class="col-xl-8 col-lg-7 news">
                        <asp:Repeater ID="rptNews" runat="server" DataSource='<%# Eval("Requests") %>'>
                            <ItemTemplate>
                                <div class="card mb-4 p-3 news-item border-0 shadow-sm">
                                    <a href='<%# Eval("DetailsURL") %>' class="text-decoration-none text-dark">
                                        <div class="row g-0">
                                            <div class="col-md-5 position-relative">
                                                <img loading="lazy"  runat="server" id="dv_newsImg" src='<%# Eval("AttachmentURL") %>' 
                                                     class="img-fluid rounded-3 w-100 object-fit-cover" height="250" 
                                                     style='<%# ((Eval("IsVideo").ToString() != "False") ? "display:none;": string.Empty) %>'
                                                     alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                                                <span class="badge rounded-pill text-bg-light">
                                                    <%# Eval("MediaDate") %>
                                                </span>
                                            </div>
                                            <div class="col-md-7">
                                                <div class="card-body">
                                                    <h3 class="card-title h5 mb-4 fw-bold">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                    </h3>
                                                    <p class="card-text text-muted">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <div class="col-xl-4 col-lg-5 events">
                        <div class="events-container px-lg-4">
                            <asp:Repeater ID="rptEvents" runat="server" DataSource='<%# Eval("Events") %>'>
                                <ItemTemplate>
                                    <div class="event-item py-4 border-bottom">
                                        <div class="row align-items-center">
                                            <div class="col-4">
                                                <div class="date px-1 py-2 d-flex flex-column text-center bg-primary text-white rounded-3">
                                                    <span class="h1 fw-bold mb-0"><%# Eval("day") %></span>
                                                    <small class="text-uppercase"><%# Eval("FromDate") %></small>
                                                </div>
                                            </div>
                                            <div class="col-8">
                                                <a href='<%# Eval("NavUrl") %>' class="text-decoration-none text-dark">
                                                    <h3 class="card-title h6 mb-2 fw-bold">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                    </h3>
                                                </a>
                                                
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>



<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#' + tabno + '-tab-pane"]').tab('show');
        });
    }

</script>

<style>
    img.img-fluid.rounded-start {
    height: 187px;
    width: 284px;
}
</style>