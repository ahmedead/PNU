<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgHubHero.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgHubHero" %>

<asp:PlaceHolder ID="phHero" runat="server">
<div id="pgHeroCarousel" data-bs-interval="false" class="carousel slide pg-kit" aria-roledescription="carousel"
     role="region" aria-label="<%= CarouselLabel %>" aria-live="off" data-bs-wrap="false">

    <div class="carousel-inner">
        <asp:Repeater ID="rptSlides" runat="server">
            <ItemTemplate>
                <div class='<%# Eval("IsFirst").Equals(true) ? "carousel-item active" : "carousel-item" %>'
                     role="group" aria-roledescription="slide" aria-label='<%# Eval("SlideLabel") %>'>
                    <picture class="carousel-media">
                        <source type="image/avif" media="(min-width: 1200px)" srcset='<%# Eval("LargeAvif") %>'>
                        <source type="image/avif" media="(min-width: 768px)" srcset='<%# Eval("MediumAvif") %>'>
                        <source type="image/avif" srcset='<%# Eval("SmallAvif") %>'>
                        <source type="image/webp" media="(min-width: 1200px)" srcset='<%# Eval("LargeWebp") %>'>
                        <source type="image/webp" media="(min-width: 768px)" srcset='<%# Eval("MediumWebp") %>'>
                        <source type="image/webp" srcset='<%# Eval("SmallWebp") %>'>
                        <img width="1920" height="492" class="carousel-image" alt='<%# Eval("ImageAlt") %>'
                             loading='<%# Eval("IsFirst").Equals(true) ? "eager" : "lazy" %>'
                             fetchpriority='<%# Eval("IsFirst").Equals(true) ? "high" : "low" %>'
                             decoding="async" src='<%# Eval("Fallback") %>'>
                    </picture>
                    <div class="carousel-caption w-100 text-white">
                        <div class="container">
                            <h2 class="text-white display-2 fw-semibold mb-4"><%# Eval("Title") %></h2>
                            <%# Eval("ParagraphsHtml") %>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <asp:PlaceHolder ID="phIndicators" runat="server">
        <div class="carousel-indicators">
            <asp:Repeater ID="rptIndicators" runat="server">
                <ItemTemplate>
                    <button type="button" data-bs-target="#pgHeroCarousel"
                            class='<%# Eval("IsFirst").Equals(true) ? "active" : "" %>'
                            data-bs-slide-to='<%# Eval("Index") %>'
                            aria-current='<%# Eval("IsFirst").Equals(true) ? "true" : "false" %>'
                            aria-label='<%# Eval("SlideLabel") %>'></button>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:PlaceHolder>
</div>
</asp:PlaceHolder>
