<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsEvents.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucNewsEvents" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="news-and-events my-1 py-1  py-md-5 my-md-5 ">
    <div class="container">
        <div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">
            <div class="col-12 col-lg-auto tabbable">
                <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab" role="tablist">

                    <asp:Repeater ID="masterRepeater" runat="server">
                        <ItemTemplate>
                            <li class='<%# "nav-item" + Eval("ClassActive") %>' role="presentation">
                                <button class='<%# "nav-link" + Eval("ClassActive") %>' id='<%# "news" + Eval("ID") + "-tab" %>' data-bs-toggle="tab" data-bs-target='<%# "#news" + Eval("ID") + "-tab-pane" %>'
                                    type="button" role="tab" aria-controls='<%# "#news" + Eval("ID") + "-tab-pane" %>' aria-selected="false">
                                     <%# SPFactory.GetLocalizedTitle(Eval("MainCategory"), Eval("MainCategory_EN")) %>
                                </button>
                            </li>

                            <%--<li class='<%# "nav-item" + Eval("ClassActive") %>' role="presentation">
    <button class='<%# "nav-link" + Eval("ClassActive") %>' id='<%# "news" + Eval("ID") + "-tab" %>'  data-bs-target='<%# "#news" + Eval("ID") + "-tab-pane" %>'
        type="button" role="tab" aria-controls='<%# "#news" + Eval("ID") + "-tab-pane" %>' aria-selected="false">
         <%# SPFactory.GetLocalizedTitle(Eval("MainCategory"), Eval("MainCategory_EN")) %>
    </button>
</li>--%>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>

        <div class="row">
            <div class="tab-content" id="myTabContent">
                <asp:Repeater ID="detailsRepeater" runat="server">
                    <ItemTemplate>
                        <div class='<%# "tab-pane fade" + Eval("ClassActiveShow") %>' id='<%# "news" + Eval("ID") + "-tab-pane" %>' role="tabpanel" aria-labelledby='<%# "news" + Eval("ID") + "-tab" %>' tabindex="0">
                            <div class="row">


                                <div class="col-xl-8 col-lg-7 news">


                                    <h1 class="title text-dark   fw-bold px-2 border-start border-primary  mb-5  mt-md-0 mt-4">
                                        <%# SPFactory.GetLocalizedTitle(Eval("MainCategory"), Eval("MainCategory_EN")) %>
                                        <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                                    </h1>

                                    <div>
                                        <asp:Repeater ID="rptNews" runat="server" DataSource='<%# Eval("Requests") %>'>
                                            <ItemTemplate>
                                                <div class="card mb-4 p-3 news-item">
                                                    <a href='<%#  Eval("DetailsURL")  %>'>
                                                        <div class="row g-0">
                                                            <div class="col-md-5">
                                                                
                                                                <img loading="lazy"  runat="server" id="dv_newsImg" src='<%# Eval("AttachmentURL") %>' class="object-fit-cover rounded-3 w-100" height="250" style='<%# ((Eval("IsVideo").ToString() != "False") ? "display:none;": string.Empty) %>'>
<%--<div class="video-wrapper video-container" id="divVideo" runat="server" visible='<%# Eval("IsVideo") %>'>
    <video class="object-fit-cover for-thumbnail rounded-3 w-100" style="height: 250px; max-width: 100%; pointer-events: none;">
        <source src="<%# Eval("VideoURL") %>" />
    </video>
</div> --%>
                                                                <span class="badge rounded-pill text-bg-light "><%# Eval("MediaDate") %></span>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <div class="card-body">
                                                                    <h5 class="card-title mb-4 fw-bold"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                                    </h5>
                                                                    <p class="card-text"><%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %></p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <div>
                                            <p class="mb-0 text-end">
                                                <a href='<%# Eval("AllNewsLink") %>' runat="server" class="btn btn-link fw-bolder text-primary text-decoration-none">
                                                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />

                                                </a>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-lg-5 events">
                                    <h1 class="title text-dark   fw-bold px-2 border-start border-primary  mb-5  mt-md-0 mt-4">
                                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, LatestEvents %>" />
                                        <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                                    </h1>
                                    <div class="events-container px-4">

                                        <asp:Repeater ID="rptEvents" runat="server" DataSource='<%# Eval("Events") %>'>
                                            <ItemTemplate>

                                                <div class="event-item py-4 border-bottom">
                                                    <div class="row">
                                                        <div class="col-md-4  ">
                                                            <div class=" date px-1 py-2 d-flex flex-column text-center bg-primary text-white ">
                                                                <span class="h1 fw-bold"><%#DataBinder.Eval(Container.DataItem,"day") %></span><h6 class=" "><%#DataBinder.Eval(Container.DataItem,"FromDate") %></h6>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-8 mt-md-0 mt-4">
                                                            <a href="<%#DataBinder.Eval(Container.DataItem,"NavUrl") %>">
                                                                <h5 class="card-title mb-4 fw-bold"><%#DataBinder.Eval(Container.DataItem,"Title") %>
                                    
                                                                </h5>
                                                            </a>
                                                            <p class="card-text text-muted d-flex">
                                                                <svg class="bi mx-2" width="20" height="20">
                                                                    <use xlink:href="#time"></use>
                                                                </svg>
                                                                <%#DataBinder.Eval(Container.DataItem,"From") %> إلى <%#DataBinder.Eval(Container.DataItem,"To") %>
                                                            </p>
                                                        </div>

                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <div>
                                            <p class="mb-0 text-end">
                                                <a href='<%# Eval("AllEventsLink") %>' runat="server" class="btn btn-link fw-bolder text-primary text-decoration-none">
                                                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllEvents %>" />

                                                </a>
                                            </p>
                                        </div>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </ItemTemplate>
                </asp:Repeater>



            </div>


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
