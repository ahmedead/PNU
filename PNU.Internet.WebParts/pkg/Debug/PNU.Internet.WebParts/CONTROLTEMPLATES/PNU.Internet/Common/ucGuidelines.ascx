<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGuidelines.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common.ucGuidelines" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Panel ID="pnlData" runat="server">

    <section class="my-5 py-5">
        <div class="container">
            <div class="d-lg-flex justify-content-between   ">

                <div class="d-flex align-items-start ">

                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0">
                        <asp:Label ID="lblWPTitle" runat="server" Visible="false"></asp:Label>
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, GuidelinesTitle %>" />
                        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                    </h1>
                    <img id="imgCoiledArrow" runat="server">
                </div>
            </div>



            <!-- -->
            <div class="position-relative">

                <div dir="rtl" class="swiper swiper-container-14 col-lg-10 swiper-initialized swiper-horizontal swiper-pointer-events swiper-rtl swiper-backface-hidden">
                    <div class="swiper-wrapper py-4 align-items-center" id="swiper-wrapper-1b64988c6cb141ab" aria-live="off" style="height: 120px; transform: translate3d(0px, 0px, 0px); transition-duration: 0ms;">

                        <asp:Repeater ID="rep" runat="server">
                            <ItemTemplate>

                                <div class="swiper-slide d-flex justify-content-center align-items-start swiper-slide-active" role="group" aria-label="1 / 6" style="height: fit-content; width: 228px; margin-left: 60px;">
                                    <div class="card border rounded-4 h-100 w-100">
                                        <div class="card-body p-2 px-4">
                                            <div class="d-flex justify-content-between align-items-center">
                                                <div class="my-3 fs-4 fw-bold text-start d-flex flex-column justify-content-start">
                                                    <strong><%# Eval("Title") %></strong>
                                                    <span class="badge rounded-pill bg-turquoise-100 bg-opacity-75 text-turquoise-900 fs-6 w-auto d-none"
                                                        style="width: fit-content !important;">

                                                        <%# Eval("Extension") %> <%# Eval("FileSize") %>
                                                    </span>
                                                </div>
                                                <a role="button" href="<%# Eval("URL") %>" class="btn px-4 btn-outline-primary mb-2" target="_blank">
                                                    <div class="d-flex justify-content-center p">
                                                        <svg width="20" height="20">
                                                            <!-- You can add SVG content here -->
                                                        </svg>
                                                        <strong class="ms-2">
                                                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:PnuInternetResources, res_View%>"></asp:Literal>
                                                            
                                                        </strong>
                                                    </div>
                                                </a>

                                                <a role="button" href="<%# Eval("URL") %>" download="<%# Eval("Title") %>" class="btn px-4 btn-outline-primary mb-2 ">
                                                    <div class="d-flex justify-content-center p">
                                                        <svg width=" 20" height="20">
                                                        </svg>
                                                        <strong class="ms-2">
                                                            <asp:Literal ID="lit_download" runat="server" Text="<%$Resources:PnuInternetResources, res_Download%>"></asp:Literal>
                                                        </strong>
                                                    </div>
                                                </a>
                                            </div>


                                        </div>
                                    </div>
                                </div>

                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="mt-5">
                        <div class="swiper-pagination swiper-pagination-bullets swiper-pagination-horizontal">
                            <span class="swiper-pagination-bullet swiper-pagination-bullet-active" aria-current="true"></span>
                            <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                            <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                        </div>
                    </div>

                </div>
            </div>


        </div>

    </section>


</asp:Panel>
