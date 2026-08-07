<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUnivHistory.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucUnivHistory" %>


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

<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>
        <section>
            <div class="container pt-5 my-5">
                <div class="row">
                    <div class="col-lg-6 order-lg-0 order-1  ">
                        <div class="ps-lg-4 mt-5 ">
                            <div class="d-flex align-items-start  mb-4 mt-5">
                                <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-5 me-md-4 flex-shrink-0">
                                    <span class="px-2 position-absolute mt-1 h2 text-primary"><font color="#000c0d"><span style="font-size: 40px;"><b><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>  </b></span></font>•</span> </h1>
                            </div>
                            <p class="fs-5 mb-5 text-muted" style="text-align: justify;">
                                <%# SPFactory.GetLocalizedTitle(Eval("Overview"), Eval("Overview_EN")) %>
                                <br>
                            </p>
                        </div>
                    </div>
                    <div class="col-12 col-lg-6 mt-0 pb-md-5 pb-2   mb-md-5 mb-2 text-end">
                        <div class=" position-relative ms-4 border-behind-img">
                            <img src='<%# Eval("PublishingRollupImage") %>' class="d-block w-100 rounded-4 object-fit-cover img-fluid ms-auto d-block mx-5" alt="...">
                        </div>

                    </div>
                </div>
            </div>
        </section>


        <section>
            <div class="container py-5 my-5 " style="display:none">
                <div class="row university-president-speech">
                    <div class="col-12 col-lg-5  order-lg-0 order-1  p-5">
                        <div class="p-3">
                            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 lh-1"><%# SPFactory.GetLocalizedTitle(Eval("PresidentMessagTitle"), Eval("PresidentMessagTitle_EN")) %> <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1>
                            <p class="text-muted h5 mb-3 lh-base" style="text-align: justify;"><%# SPFactory.GetLocalizedTitle(Eval("PresidentMessage"), Eval("PresidentMessage_EN")) %>  </p>
                        </div>
                    </div>
                    <div class="col-12 col-lg-7 mt-5 py-5  mb-5  text-start">
                        <div class="border-behind-img position-relative object-fit-cover">
                            <img src='<%# Eval("PublishingPageImage") %>' class=" img-fluid ms-auto d-block mx-5 w-75" alt="...">
                        </div>

                    </div>
                </div>
            </div>
        </section>
        <section class="container my-5">
            <div class="row mt-5 pt-5 gy-5">
                <div class="col-12 col-lg-6  order-lg-0 order-1  ">
                    <div class="p-5 breadcrumb  rounded-3 h-100">
                        <div class="p-2">
                            <h1 class="title text-dark fw-bold px-2 border-start border-primary  mb-5"><%# SPFactory.GetLocalizedTitle(Eval("VisionTitle"), Eval("VisionTitle_EN")) %> <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1>
                            <p class="text-muted h5 mb-3 lh-base"><%# SPFactory.GetLocalizedTitle(Eval("VisionText"), Eval("VisionText_EN")) %>  </p>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-6  order-lg-0 order-1  ">
                    <div class="p-5 breadcrumb  rounded-3 h-100">
                        <div class="p-2">
                            <h1 class="title text-dark fw-bold px-2 border-start border-primary  mb-5"><%# SPFactory.GetLocalizedTitle(Eval("MissionTitle"), Eval("MissionTitle_EN")) %>
                                <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1>
                            <p class="text-muted h5 mb-3 lh-base"><%# SPFactory.GetLocalizedTitle(Eval("MissionText"), Eval("MissionText_EN")) %>  </p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

    </ItemTemplate>
</asp:Repeater>


<section class="degrees my-1 py-1  py-md-5 my-md-5">
    <div class="container">
        <div class="row">
            <div class="d-flex justify-content-center">
                <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, OurValues %>" />
                    <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1>
            </div>
        </div>
        <div class="row border rounded-4">
            <asp:Repeater ID="rptOurValues" runat="server">
                <ItemTemplate>
                    <div class="col-lg-3 mb-lg-0 mb-4">
                        <div class="degree-item card text-center pt-5 p-4 d-flex flex-column align-items-center border-0">
                            <div class="icon-container rounded-circle bg-resonant-blue-100">
                                <h1 class="text-primary"><%# SPFactory.GetLocalizedTitle(Eval("CircleText"), Eval("CircleText_EN")) %> </h1>
                            </div>
                            <h2 class="my-4 fw-bolder text-dark"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h2>
                            <p class="h6 mb-3 px-3 text-dark"><%# SPFactory.GetLocalizedTitle(Eval("Desc"), Eval("Desc_EN")) %> </p>
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>
</section>


<section class=" my-5 p-5 bg-semi-light position-relative">
    <div class="container ">
        <div class="swiper-sections-navigation d-none d-lg-flex bottom-0">
            <div class="swiper-next me-2" role="button" aria-label="Next slide" aria-controls="swiper-wrapper-e8d8ececc53d3416" aria-disabled="false">
                <svg xmlns="http://www.w3.org/2000/svg" width="50" height="30">
                    <use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="#arrowRight" />
                </svg>
                &#160;
            </div>
            <div class="swiper-prev swiper-button-disabled" tabindex="-1" role="button" aria-label="Previous slide" aria-controls="swiper-wrapper-e8d8ececc53d3416" aria-disabled="true">
                <svg xmlns="http://www.w3.org/2000/svg" width="50" height="30">
                    <use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="#arrowLeft" />
                </svg>
                &#160;
            </div>
            &#160;
        </div>
        <div class="d-flex align-items-start my-5">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, StrategicPlan %>" />
                <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1>
            <img src="/Style%20Library/NewStyle/UI5/images/coiled-arrow.png" />
        </div>
        <div>
            <div class="swiper swiper-container-13 swiper-initialized swiper-horizontal swiper-pointer-events swiper-rtl swiper-backface-hidden">
                <div class="swiper-wrapper" id="swiper-wrapper-e8d8ececc53d3416" aria-live="polite" style="transition-duration: 0ms; transform: translate3d(0px, 0px, 0px);">


                    <asp:Repeater ID="rptStrategicPlan" runat="server">
                        <ItemTemplate>
                            <div class="swiper-slide bg-transparent swiper-slide-active" role="group" aria-label="<%# String.Format("{0} / 7",SPFactory.GetLocalizedTitle(Eval("DisplayNo"), Eval("DisplayNo_EN"))) %>"  style="width: 301.5px; margin-left: 30px;">
                                <div class="d-flex align-items-center flex-column justify-content-center">
                                    <div class=" rounded-circle bg-turquoise-100 w-52px h-52px  text-center d-flex flex-column d-flex align-items-center flex-column justify-content-center z-3 ">
                                        <span class="text-turquoise-600 fs-4 mb-0 lh-sm mt-1 fw-bold"> <%# Eval("DisplayNo") %></span>
                                    </div>
                                    <div class="electronic-sys-item card text-center py-5  px-3 d-flex flex-column align-items-center rounded-bottom-0 " style="margin-top: -27px;">
                                        <div class="py-4">
                                            <h4 class="mb-4 fw-bolder text-dark text-start"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>    </h4>
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
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span><span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
                <span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>
            </div>
        </div>
        <div class="d-flex justify-content-center    py-lg-4  mt-5">&#160;<br />
        </div>
    </div>

</section>






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
