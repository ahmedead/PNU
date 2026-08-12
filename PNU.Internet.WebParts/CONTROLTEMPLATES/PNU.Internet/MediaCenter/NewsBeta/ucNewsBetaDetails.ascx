<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsBetaDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta.ucNewsBetaDetails" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fancybox/3.5.7/jquery.fancybox.min.css" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/fancybox/3.5.7/jquery.fancybox.min.js"></script>


<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>
        <section class="breadcrumb">
            <div class="container py-5 my-3">
                <h1 class="display-6 fw-bold mb-3"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h1>
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb h6">
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>" class="text-decoration-none fw-bold">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a></li>
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}/{1}/", SPFactory.GetSiteURL(),"MediaCenter") %>" class="text-decoration-none fw-bold">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, MediaCenter %>" /></a></li>

                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}{1}", SPFactory.GetSiteURL(),"/MediaCenter/Pages/AllNews.aspx") %>" class="text-decoration-none fw-bold">

                            <asp:Literal runat="server" Text='<%# String.Format("{0}", Eval("DisplayCatName")) %> ' /></a></li>
                        </li>
                    </ol>
                </nav>
        </section>


        <section class="researcher-and-contributor ">
            <div class="container py-5 my-5">
                <div class="row">

                    <div class="col-12  mt-3    mb-md-5 mb-2 d-flex justify-content-center" id="divImage" runat="server" style='<%# "display:" +  Eval("ImgVisible") %>'>
                        <div class="mx-4 pattern-border d-block video-container ">
                            <img src='<%# Eval("AttachmentURL") %>' class=" rounded-3 " alt="..." style='<%# "display:" +  Eval("ImgVisible") %>'>
                        </div>
                    </div>

                    <div class="col-12 order-lg-0 order-1 " id="divTitle" runat="server" style='<%# "display:" +  Eval("TitleVisible") %>'>
                        <div class=" mb-3 mt-md-0 mt-4">
                            <h1 class="title text-dark fw-bold m-0">
                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                            </h1>
                            <a href="#" class="d-block mt-3 text-decoration-none mb-3 mb-md-0">
                                <svg width=" 18" height="16">
                                    <use xlink:href="#share" />
                                </svg>
                                <span class="ms-2 text-primary">مشاركة</span>
                            </a>
                        </div>

                        <div class="my-3">
                            <svg width="19" height="20">
                                <use xlink:href="#calendar" />
                            </svg>
                            <span class="ms-2 text-muted"><%# Eval("MediaDate") %></span>
                        </div>
                        <p class="text-muted h5 mb-3 lh-base text-justify">
                            <%# SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) %>
                            <br>
                            <br>
                        </p>



                    </div>



                    <div class="video-wrapper video-container" id="div1" runat="server" visible='<%# Convert.ToBoolean(Eval("IsVideo")) && Convert.ToBoolean(Eval("IsYouTubeVideo")) %>'>
                        
                        <a href="<%# Eval("VideoURL") %>" data-fancybox="video1" class="video__item" data-cat="1" style="display:none">
                            <div class="video__cover position-relative">

                                <!-- الصورة المصغرة للفيديو -->
                                <img src='https://img.youtube.com/vi/<%# Eval("VideoID") %>/0.jpg'
                                    border="0"
                                    class="img-fluid"
                                    alt="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>">

                                <!-- أيقونة YouTube SVG في المنتصف -->
                                <div class="youtube-icon position-absolute d-flex justify-content-center align-items-center"
                                    style="top: 0; left: 0; width: 100%; height: 100%; z-index: 2;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" viewBox="0 0 48 48">
                                        <circle cx="24" cy="24" r="24" fill="rgba(0,0,0,0.6)"></circle>
                                        <path d="M43.6,14.6c-0.5-2-2-3.5-4-4c-3.5-0.9-17.6-0.9-17.6-0.9s-14.1,0-17.6,0.9c-2,0.5-3.5,2-4,4
                             c-0.9,3.5-0.9,10.9-0.9,10.9s0,7.4,0.9,10.9c0.5,2,2,3.5,4,4c3.5,0.9,17.6,0.9,17.6,0.9s14.1,0,17.6-0.9
                             c2-0.5,3.5-2,4-4c0.9-3.5,0.9-10.9,0.9-10.9S44.5,18.1,43.6,14.6z"
                                            fill="#FF0000">
                                        </path>
                                        <polygon fill="#FFFFFF" points="19,31 31,24 19,17"></polygon>
                                    </svg>
                                </div>

                            </div>
                        </a>
                        <div class="col-12  mt-3 mb-md-5 mb-2 d-flex  justify-content-center">
                            <div class="mx-4 pattern-border d-block vplyr ">
                                <!-- <div class=" rounded-3 " id="player2" data-plyr-provider="vimeo" data-plyr-embed-id="76979871"></div> -->



                                <div id="player-container" class="plyr__video-embed" data-plyr-provider="youtube"
                                    data-plyr-embed-id="<%# Eval("VideoID") %>">
                                </div>

                                <!-- YouTube IFrame API -->
                                <script src="https://www.youtube.com/iframe_api"></script>

                                <script src="https://cdn.plyr.io/3.8.3/plyr.polyfilled.js"></script>
                                <link rel="stylesheet" href="https://cdn.plyr.io/3.8.3/plyr.css" />
                                <script>
                                    let plyrInstance;

                                    // الدالة التي يجب أن تكون في النطاق العام ليتم استدعاؤها من قبل YouTube IFrame API
                                    function onYouTubePlayerAPIReady() {
                                        // تأكد من تهيئة Plyr مرة واحدة فقط
                                        if (!plyrInstance) {
                                            plyrInstance = new Plyr('#player-container', {
                                                // الخيارات التي تم إضافتها في الإجابة السابقة
                                                fullscreen: {
                                                    enabled: false,
                                                },
                                                youtube: {
                                                    modestbranding: 1,
                                                    rel: 0
                                                }
                                            });

                                            // ربط الأحداث
                                            plyrInstance.on('ready', onPlayerReady);
                                            plyrInstance.on('ended', onPlayerEnded);
                                        }
                                    }

                                    // تعيين الدالة إلى النطاق العام للتوافق مع YouTube API
                                    window.onYouTubePlayerAPIReady = onYouTubePlayerAPIReady;

                                    // عند الانتهاء من تحميل الـ DOM، يتم تنفيذ الدالة للتأكد في حالة كان الـ API قد حمل بالفعل
                                    document.addEventListener('DOMContentLoaded', () => {
                                        // إذا كان كائن YT موجوداً، فهذا يعني أن API قد تم تحميله بالفعل ويجب استدعاء الدالة يدوياً
                                        if (window.YT && window.YT.Player) {
                                            onYouTubePlayerAPIReady();
                                        }
                                    });


                                    // ************************
                                    // الدوال الخاصة بك
                                    // ************************

                                    // هذه الدالة سيتم استدعاؤها عندما يكون المشغل جاهزاً
                                    function onPlayerReady(event) {
                                        // لا تحتاج إلى استدعاء cueVideoById هنا لأن Plyr يقوم بذلك تلقائياً
                                    }

                                    // هذه الدالة تتعامل مع إنهاء الفيديو
                                    function onPlayerEnded(event) {
                                        // يتم إعادة تشغيل الفيديو
                                        if (plyrInstance) {
                                            plyrInstance.stop();
                                            plyrInstance.source = {
                                                type: 'video',
                                                sources: [{
                                                    src: '<%# Eval("VideoID") %>',
                                                    provider: 'youtube',
                                                }],
                                            };
                                        }
                                    }

                                </script>

                                <style>
                                    /* Adjust iframe size within Plyr container */
                                    .plyr__video-embed iframe {
                                        top: 0;
                                        height: 100%;
                                    }

                                    /* Custom CSS for player state and poster */
                                    .plyr--paused {
                                        /* Force poster to render on pause */
                                        background: #000;
                                        /* or use a custom color or image */
                                    }

                                    .plyr__poster {
                                        /* Force poster to cover the video */
                                        background-size: cover;
                                        z-index: 2;
                                    }

                                    /* Additional custom styling for iframe */
                                    .plyr__video-embed iframe {
                                        top: -50%;
                                        height: 200%;
                                    }
                                    button.plyr__control.plyr__control--overlaid {
                                            min-width:unset  !important;
                                    }

                                    button.plyr__controls__item.plyr__control {
                                        min-width:unset  !important;
                                    }
                                </style>
                            </div>
                        </div>

                    </div>




                    <div id="divVideoYouTube" runat="server" style='<%# "display:" +  Eval("MP4Visible") %>'>



                        <div class="video-wrapper" id="divVideo" style='<%# "display:" +  Eval("VideoVisiable") %>'>

                            <iframe class="responsive-iframe" src='<%# Eval("VideoURL") %>'></iframe>
                        </div>

                    </div>
                    <div class="col-12  mt-3 mb-md-5 mb-2 d-flex  justify-content-center">
                        <div class="mx-4 pattern-border d-block vplyr ">
                            <div class=" rounded-3 " id="player2" data-plyr-provider="vimeo" data-plyr-embed-id="76979871"></div>
                        </div>
                    </div>





                </div>
                <br>


                <script>
                    $(document).ready(function () {
                        $("[data-fancybox]").fancybox({
                            // Options
                        });
                    });
                </script>

                <script>
                    $(document).ready(function () {
                        $("[data-fancybox]").fancybox({
                            // Options
                            width: 800, // Width of the fancybox
                            height: 450, // Height of the fancybox
                            iframe: {
                                preload: false // Disable iframe preloading
                            },
                            animationEffect: "fade", // Opening animation effect
                            transitionEffect: "slide" // Transition effect between slides
                        });
                    });
                </script>

                <style>
                    .video-wrapper {
                        position: relative;
                        padding-bottom: 56.25%; /* 16:9 Aspect Ratio */
                        height: 0;
                        overflow: hidden;
                    }

                    .responsive-iframe {
                        position: absolute;
                        top: 0;
                        left: 0;
                        width: 100%;
                        height: 100%;
                        border: 0;
                    }

                    img.rounded-3 {
                        max-width: 700px;
                        max-height: 700px;
                    }
                    button.plyr__control.plyr__control--overlaid {
                            min-width:unset  !important;
                    }

                    button.plyr__controls__item.plyr__control {
                        min-width:unset  !important;
                    }
                </style>


            </div>
        </section>


    </ItemTemplate>
</asp:Repeater>




<div class="row">
    <asp:Repeater ID="rptNews" runat="server">
        <ItemTemplate>
            <div class="col-md-6 col-lg-4 mb-4">
                <div class="card border rounded-4 h-100">
                    <div class="card-body p-2">
                        <div class="position-absolute top-0 start-0 m-4">
                            <span class="badge rounded-pill bg-tertiary text-white fs-6"><%# Eval("MediaTypes") %>
                            </span>
                        </div>
                        <div class="video-wrapper video-container" id="divVideo" runat="server"
                            visible='<%# Convert.ToBoolean(Eval("IsVideo")) && Convert.ToBoolean(Eval("IsYouTubeVideo")) %>'>
                            <div class="video__cover">
                                <img src='https://img.youtube.com/vi/<%# Eval("VideoID") %>/0.jpg'
                                    border="0"
                                    class="img-fluid"
                                    alt="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>">
                                <div class="play--btn">
                                    <svg width="35" height="35" viewBox="0 0 45 45"></svg>
                                </div>
                            </div>
                        </div>

                        
                        <div style='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("AttachmentURL"))) ? "": "display:none;" %>'>
                            <img src='<%# Eval("AttachmentURL") %>'
                                class="object-fit-cover rounded-3 w-100"
                                height="250"
                                alt="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>">
                        </div>
                        
                        <div class="my-2 fs-4 fw-bold">
                            <a href='<%#  Eval("DetailsURL")  %>' class="stretched-link text-decoration-none text-body strong">
                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> 
                            </a>
                        </div>
                        <div class="d-flex mb-3">
                            <svg width="19" height="20">
                                <use xlink:href="#calendar" />
                            </svg>
                            <span class="ms-2 text-muted"><%# Eval("MediaDate") %></span>
                        </div>


                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
           




<div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">
    <div class="py-3 d-flex flex-wrap d-md-block">
        <a id="AllMCNews" runat="server" class="btn btn-lg btn-outline-primary  px-5  me-3 mb-3  mb-md-5 flex-fill" href="">
            <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />

        </a>
    </div>
</div>
