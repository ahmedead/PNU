<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMissions.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Departments.ucMissions" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>




    <link rel="stylesheet" href="https://unpkg.com/swiper/swiper-bundle.min.css">
    <style>
        .swiper-container-13 {
            width: 100%;
            height: 100%;
        }
        .swiper-slide {
            display: flex;
            justify-content: center;
            align-items: center;
            text-align: center;
            font-size: 18px;
            background: #fff;
            width: auto;
        }
        .swiper-slide .electronic-sys-item {
            width: 100%;
        }
    </style>
<asp:Panel ID="pnlData" runat="server">
<section class="my-5 p-5 bg-semi-light position-relative">
    <div class="container">
        <div class="swiper-sections-navigation d-none d-lg-flex bottom-0">
            <div class="swiper-next me-2" role="button" aria-label="Next slide">
                <svg xmlns="http://www.w3.org/2000/svg" width="50" height="30">
                    <use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="#arrowRight"></use>
                </svg>
            </div>
            <div class="swiper-prev" role="button" aria-label="Previous slide">
                <svg xmlns="http://www.w3.org/2000/svg" width="50" height="30">
                    <use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="#arrowLeft"></use>
                </svg>
            </div>
        </div>
        <div class="d-flex align-items-start my-5">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0">
                <asp:Literal runat="server" ID="literalDepartmentName"  />
                <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> 
            </h1>
            <img src="/Style%20Library/NewStyle/UI5/images/coiled-arrow.png">
        </div>
        <div>
            <div class="swiper swiper-container-13">
                <div class="swiper-wrapper">
                    <asp:Repeater ID="rptStrategicPlan" runat="server">
                        <ItemTemplate>
                            <div class="swiper-slide bg-transparent">
                                <div class="d-flex align-items-center flex-column justify-content-center">
                                    <div class="rounded-circle bg-turquoise-100 w-52px h-52px text-center d-flex flex-column align-items-center justify-content-center z-3">
                                        <span class="text-turquoise-600 fs-4 mb-0 lh-sm mt-1 fw-bold"><%# Eval("DisplayNo") %></span>
                                    </div>
                                    <div class="electronic-sys-item card text-center py-5 px-3 d-flex flex-column align-items-center rounded-bottom-0" style="margin-top: -27px;">
                                        <div class="py-4">
                                            <h4 class="mb-4 fw-bolder text-dark text-start"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title")) %></h4>
                                            <div>
                                                <%# Eval("Desc") %>
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
    </div>
</section>

    </asp:Panel>
<!-- Swiper JS -->
<script src="https://unpkg.com/swiper/swiper-bundle.min.js"></script>
<script>
    var swiper = new Swiper('.swiper-container-13', {
        slidesPerView: 4,
        spaceBetween: 30,

        navigation: {
            nextEl: '.swiper-prev',
            prevEl: '.swiper-next',
        },
    });
</script>
