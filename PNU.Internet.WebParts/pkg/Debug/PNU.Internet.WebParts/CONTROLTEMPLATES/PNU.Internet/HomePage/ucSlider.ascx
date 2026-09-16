<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSlider.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucSlider" %>


<section class="slider-carousel">
    <div id="myCarousel" class="carousel slide" data-bs-ride="false">
        <div class="background-overlay  bg-sa-950 bg-opacity-75"></div>
        <div class="carousel-inner">
            <asp:Repeater ID="rptSlider" runat="server">
                <ItemTemplate>
                    <a href='<%# Eval("BtnURL") %>'>
                        <div class='<%# Container.ItemIndex == 0 ? "carousel-item active" : "carousel-item" %>'>
                            <%# Eval("VideoHTML") %>
                            <div class="h-100">
                                  
                               <%-- <picture>
                              <!-- mobile -->
                              <source loading="lazy"  class="w-100 h-100 object-fit-cover" media="(max-width:768px)" id="mobPic"
                                srcset="<%# Eval("MobileImageURL") %>">
                              <!-- Larger than mobile -->
                              <img loading="lazy"  src='<%# Eval("PublishingRollupImage") %>' class="h-100 object-fit-cover h-100 w-100" alt="...">
                                class="w-100 h-100 object-fit-cover" alt="صورة غلاف اليوم الوطني السعودي 95">
                              <!-- mobile -->
                            </picture>--%>

                                <picture>
                                  <!-- mobile -->
                                  <source media="(max-width: 767.98px)" srcset="<%# Eval("MobileImageURL") %>" loading="lazy">
                                  <!-- tablet -->
                                  <source media="<%# Eval("TabletImageURL") %>" srcset="<%# Eval("TabletImageURL") %>" loading="lazy">
                                  <!-- desktop -->
                                  <img src="<%# Eval("PublishingRollupImage") %>" class="w-100 h-100 object-fit-cover" alt="<%# Eval("Desc") %>" loading="lazy">
                                </picture>


                                <div class="carousel-caption text-start">
                                    <div class="container">
                                        <div class="row justify-content-between">
                                            <div class="col-12 col-md-6">
                                                <h1 class="lh-base mt-4 text-light"><%# Eval("Desc") %></h1>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>



                           



                        </div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
		
		
		<div class="position-absolute bottom-0 w-100 z-11 mb-3">
          <div class="container">
            <div class="d-flex justify-content-center align-items-center">
              <div class="carousel-indicators d-block position-relative m-0">
                <asp:Repeater ID="rptIndicators" runat="server">
                            <ItemTemplate>
                                <button type="button" data-bs-target="#myCarousel" data-bs-slide-to='<%# Container.ItemIndex %>' class='<%# Container.ItemIndex == 0 ? "active" : "" %>' aria-current='<%# Container.ItemIndex == 0 ? "true" : "false" %>' aria-label='Slide <%# Container.ItemIndex + 1 %>'></button>
                            </ItemTemplate>
                        </asp:Repeater>
              </div>

            </div>
          </div>
        </div>
		
		
        
    </div>
</section>






<script>
    document.addEventListener('DOMContentLoaded', function () {
        const myCarousel = document.getElementById('myCarousel');
        const carousel = new bootstrap.Carousel(myCarousel, {
            interval: false // Disable default interval
        });
        let carouselTimeout;
        let currentVideo = null;
        function startCarouselInterval() {
            clearTimeout(carouselTimeout);
            carouselTimeout = setTimeout(() => {
                carousel.next();
            }, 5000); // Normal slide duration after video
        }
        function handleSlideContent(activeItem) {
            // Pause and reset any previous video
            if (currentVideo) {
                currentVideo.pause();
                currentVideo.currentTime = 0;
                currentVideo = null;
            }
            const video = activeItem.querySelector('video');
            if (video) {
                // Configure video properties
                video.muted = true;
                video.loop = false;
                video.playsInline = true;
                // Play video and handle end
                const playPromise = video.play();
                if (playPromise !== undefined) {
                    playPromise.then(() => {
                        currentVideo = video;
                        video.addEventListener('ended', () => {
                            startCarouselInterval();
                        }, { once: true });
                    }).catch(error => {
                        // Autoplay was prevented, start interval anyway
                        startCarouselInterval();
                    });
                }
            } else {
                startCarouselInterval();
            }
        }
        // Handle initial slide
        const initialActiveItem = document.querySelector('.carousel-item.active');
        handleSlideContent(initialActiveItem);
        // Handle slide changes
        myCarousel.addEventListener('slid.bs.carousel', function (event) {
            const activeItem = event.relatedTarget;
            handleSlideContent(activeItem);
        });
        // Handle manual controls
        document.querySelectorAll('[data-bs-slide]').forEach(control => {
            control.addEventListener('click', () => {
                clearTimeout(carouselTimeout);
                if (currentVideo) {
                    currentVideo.pause();
                    currentVideo.currentTime = 0;
                    currentVideo = null;
                }
            });
        });
    });
</script>
