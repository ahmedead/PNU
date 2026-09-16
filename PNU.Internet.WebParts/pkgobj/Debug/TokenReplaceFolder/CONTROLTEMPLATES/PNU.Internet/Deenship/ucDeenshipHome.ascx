<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDeenshipHome.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Deenship.ucDeenshipHome" %>



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

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class=" pt-5 MainData"></section>

<section  id="missionSectionContainer" class=" my-5 p-5 bg-semi-light position-relative">
    
</section>


<%--<section class="Mission  my-5 p-5 bg-semi-light position-relative">
    <div class="container  my-5 py-5">
        <div class="swiper-sections-navigation d-none d-lg-flex bottom-0">
                    <div class=" swiper-next me-2">
                        <svg width="50" height="30">
                            <use xlink:href="#arrowRight" />
                        </svg>
                    </div>
                    <div class="swiper-prev">
                        <svg width="50" height="30">
                            <use xlink:href="#arrowLeft" />
                        </svg>
                    </div>
                </div>
				<div class="d-lg-flex justify-content-between   ">

                <div class="d-flex align-items-start my-5">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, MainMissions %>" />
                        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
						
                    </h1>


                    

                    <img id="mgCoiledArrow" runat="server">
                </div>
				
				</div>
        <div>
            <div class="swiper swiper-container-13">
                <div class="swiper-wrapper">
                    
                   
                </div>
            </div>
        </div>
        
    </div>
</section>--%>



<!-- Swiper JS -->
<script src="https://unpkg.com/swiper/swiper-bundle.min.js"></script>
<script>
    var swiper = new Swiper('.swiper-container-13', {
        slidesPerView: 4,
        spaceBetween: 30,
        loop: true,
        autoplay: {
            delay: 2500,
            disableOnInteraction: false,
        },
        navigation: {
            nextEl: '.swiper-next',
            prevEl: '.swiper-prev',
        },
    });
</script>


