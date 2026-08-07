<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MoreRecentReleases-WPUserControl.ascx.cs" Inherits="PNU_SP.webparts.MoreRecentReleases_WP.MoreRecentReleases_WPUserControl" %>

<section class="exhibition py-5">
    <div class="container my-4 mt-5">
        <div class="d-flex justify-content-between align-items-baseline mb-5">
            <div>
                <h1 class="title text-dark fw-bold px-2 border-start border-primary    mt-md-0 mt-4">الإصدارات الأخيرة
             
                    <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                </h1>
            </div>
            <div>
                <p class="mb-0 ">
                    <a class="btn btn-link fw-bolder text-primary text-decoration-none" href="/ar/MediaCenter/RecentReleases/Pages/default.aspx">عرض كل
               </a>
                </p>
            </div>

        </div>
        <div class="row">
            <asp:Repeater ID="rep" runat="server" OnItemCommand="rep_ItemCommand">
                <ItemTemplate>
                    <div class="col-lg-4 col-md-6 col-12">
                        <div class="card mb-4 p-3 exhibition-item shadow-none">
                            <div class="row g-0">
                                <div class="col-md-12">
                                    <asp:Image runat="server" ID="Image1" ImageUrl='<%#Eval("ImageUrl")%>' class="img-fluid rounded-start w-100" alt="..." />
                                    <span class="badge rounded-pill text-bg-secondary text-white px-3">
                                        <asp:Label runat="server" ID="SubCategory1"></asp:Label>
                                    </span>
                                    <div class="card-body">
                                         <asp:HyperLink Target="_blank" runat="server"  ID="url1" href='<%# Eval("ID","ReleaseDetails.aspx?itemid={0}") %>' class="stretched-link text-decoration-none text-body">

                                            <h5 class="card-title mb-3 fw-bold">
                                                <asp:Label runat="server" ID="lblTitle" Text='<%#Eval("Title") %>'></asp:Label></h5>
                                        </asp:HyperLink>
                                        <p class=" mb-3 card-text text-muted  d-flex-inline align-items-center ">
                                            <asp:Label runat="server" ID="lblDate" Text='<%# Convert.ToString(Eval("Date")) %>'></asp:Label>
                                            <svg class="bi   me-1 mb-1" width="20" height="20">
                                                <use xlink:href="#calendar" />

                                            </svg>

                                        </p>
                                        <p class="card-text">
                                            <asp:Label runat="server" ID="lblDec" Text='<%#Eval("Desc") %>'></asp:Label>
                                        </p>
                                        <span class="d-block">
                                            <span
                                                class="badge rounded-pill position-static text-bg-resonant-blue-100 text-secondary me-1">
                                                <asp:Label runat="server" ID="lblTag1" Text='<%#Eval("_x0054_ag1") %>'></asp:Label>
                                            </span>
                                            <span class="badge rounded-pill position-static text-bg-resonant-blue-100  text-secondary  me-1 ">
                                                <asp:Label runat="server" ID="lblTag2" Text='<%#Eval("Tags2") %>'></asp:Label>
                                            </span>
                                        </span>
                                        <span class="d-block">
                                       
                                            <asp:Button ID="btnDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("ID") %>' />

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
