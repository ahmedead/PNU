<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGallery.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.ucGallery" %>




<section class="exhibition py-5">
    <div class="container my-4 mt-5">
        <div class="d-flex justify-content-between align-items-baseline mb-5">
            <div>
                <h1 class="title text-dark fw-bold px-2 border-start border-primary    mt-md-0 mt-4"><asp:Literal ID="lit_Exhibition" runat="server" Text="<%$Resources:PnuInternetResources, res_Exhibition%>"></asp:Literal> 
              <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                </h1>
            </div>
            <div>
                <p class="mb-0 ">
                    <a runat="server" id="allExhibitionUrl" href="/ar/MediaCenter/Gallery/Pages/exhibition.aspx" class="btn btn-link fw-bolder text-primary text-decoration-none">عرض كل
                المعارض
                ›</a>
                </p>
            </div>

        </div>
        <div class="row">
            <asp:Repeater ID="rptGallery" runat="server">
                <ItemTemplate>
                    <div class="col-lg-4 col-md-6 col-12">
                        <div class="card mb-4 p-3 exhibition-item shadow-none">
                            <div class="row g-0">
                                <div class="col-md-12">
                                    <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" class="img-fluid rounded-start w-100" alt="...">
                                    <div class="card-body">
                                        <a style="text-decoration: none;" class="card-title mb-3 fw-bold" href="<%#DataBinder.Eval(Container.DataItem,"NavUrl") %>">
                                            <h5 class="card-title mb-3 fw-bold"><%#DataBinder.Eval(Container.DataItem,"Title") %> </h5>
                                        </a>
                                        <p class=" mb-3 card-text text-muted  d-flex-inline align-items-center  ">
                                            <svg class="bi   me-1 mb-1" width="20" height="20">
                                                <use xlink:href="#calendar"></use>
                                            </svg>
                                            <%#DataBinder.Eval(Container.DataItem,"FromDate", "{0:yyyy/MMMM/dd}") %>
                   
                                        </p>
                                        <p class="card-text">
                                            <%#DataBinder.Eval(Container.DataItem,"ShortDesc") %>
                   
                                        </p>
                                        <span class="d-block">
                                            <span class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#DataBinder.Eval(Container.DataItem,"Faculty") %></span>
                                            <span class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#DataBinder.Eval(Container.DataItem,"Tag") %></span>
                                        </span>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>

            </asp:Repeater>
        </div>
    </div>
</section>
