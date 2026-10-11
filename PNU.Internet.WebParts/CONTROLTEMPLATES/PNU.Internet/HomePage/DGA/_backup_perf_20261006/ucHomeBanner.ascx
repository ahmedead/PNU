<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeBanner.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeBanner" %>




    <div class="hero-container">
      <div id="heroCarousel" data-bs-interval="false" class="carousel slide" aria-roledescription="carousel"
        role="region" aria-label="أبرز المحتوى" aria-live="off" data-bs-wrap="false">
        <div class="carousel-inner">
          <asp:Repeater ID="rptSlider" runat="server">
    <ItemTemplate>
    <div class='<%# (Container.ItemIndex == 0 ? "carousel-item active" : "carousel-item") + (string.IsNullOrWhiteSpace(Convert.ToString(Eval("Title"))) ? " no-overlay" : "") %>' role="group" aria-roledescription="slide" aria-label="<%# Eval("ID") %> من 3">

        <%# string.IsNullOrWhiteSpace(Convert.ToString(Eval("Title"))) && !string.IsNullOrWhiteSpace(Convert.ToString(Eval("BtnURL"))) 
            ? "<a href=\"" + Server.HtmlEncode(Convert.ToString(Eval("BtnURL"))) + "\" class=\"carousel-image-link\" aria-label=\"" + Server.HtmlEncode(Convert.ToString(Eval("Desc"))) + "\">" 
            : "" %>

        <picture class="carousel-media">
            <source type="image/avif" media="(min-width: 1200px)" srcset="/ar/SliderImages/<%# Eval("ID") %>/hero-lg.avif">
            <source type="image/avif" media="(min-width: 768px)" srcset="/ar/SliderImages/<%# Eval("ID") %>/hero-md.avif">
            <source type="image/avif" srcset="/ar/SliderImages/<%# Eval("ID") %>/hero-sm.avif">
            <source type="image/webp" media="(min-width: 1200px)" srcset="/ar/SliderImages/<%# Eval("ID") %>/hero-lg.webp">
            <source type="image/webp" media="(min-width: 768px)" srcset="/ar/SliderImages/<%# Eval("ID") %>/hero-md.webp">
            <source type="image/webp" srcset="/ar/SliderImages/<%# Eval("ID") %>/hero-sm.webp">
            <img width="1920" height="492" class="carousel-image" alt="الخدمات الرقمية في جامعة الأميرة نورة"
                loading="eager" fetchpriority="high" decoding="async" src='<%# Eval("ResizedImageUrl") %>'>
        </picture>

        <%# string.IsNullOrWhiteSpace(Convert.ToString(Eval("Title"))) && !string.IsNullOrWhiteSpace(Convert.ToString(Eval("BtnURL"))) 
            ? "</a>" 
            : "" %>

        <asp:PlaceHolder runat="server" Visible='<%# !string.IsNullOrWhiteSpace(Convert.ToString(Eval("Title"))) %>'>
            <div class="carousel-caption w-100 text-white">
                <div class="container">
                    <h2 class="text-white display-2 fw-semibold mb-4"><%# Eval("Title") %></h2>
                    <p class="pnu-hero-text fs-4 mb-2"><%# Eval("Desc") %></p>
                    <a class="btn btn-on-color-primary px-4 mt-4" href='<%# Eval("BtnURL") %>'>المزيد</a>
                </div>
            </div>
        </asp:PlaceHolder>

    </div>
</ItemTemplate>
</asp:Repeater>

        </div>
        <div class="carousel-indicators">
            <asp:Repeater ID="rptIndicators" runat="server">
            <ItemTemplate>
                
                <button type="button" data-bs-target="#heroCarousel"  data-bs-slide-to="<%# Container.ItemIndex %>"  class='<%# Container.ItemIndex == 0 ? "active" : "" %>' aria-current='<%# Container.ItemIndex == 0 ? "true" : "false" %>' 
                     aria-label="الشريحة <%# Eval("ID") %>"></button>
            </ItemTemplate>
        </asp:Repeater>

        </div>
      </div>
    </div>



<style>

.hero-container .carousel-image-link {
    display: block;
    line-height: 0;
}
.hero-container .carousel-image-link:focus-visible {
    outline: 3px solid #c9a96e; /* or your DGA focus color */
    outline-offset: -3px;
}
		/* Hero carousel — cap height at 492px, cover behavior */
.hero-container {
    width: 100%;
    max-height: 492px;
    overflow: hidden;
    position: relative;
}

.hero-container .carousel,
.hero-container .carousel-inner,
.hero-container .carousel-item {
    max-height: 492px;
}

.hero-container .carousel-media {
    display: block;
    width: 100%;
    max-height: 492px;
    overflow: hidden;
}

.hero-container .carousel-image {
    width: 100%;
    height: 492px;
    max-height: 492px;
    object-fit: cover;       /* fill width, crop top/bottom as needed */
    object-position: center; /* keep the center of the image visible */
    display: block;
}

/* ============================================================
   No-overlay: slides without a title show the clean image only
   ============================================================ */
.hero-container .carousel-item.no-overlay::before,
.hero-container .carousel-item.no-overlay::after {
    display: none !important;
    content: none !important;
    background: none !important;
}

.hero-container .carousel-item.no-overlay .carousel-media::before,
.hero-container .carousel-item.no-overlay .carousel-media::after {
    display: none !important;
    content: none !important;
    background: none !important;
}

.hero-container .carousel-item.no-overlay .carousel-image {
    filter: none !important;
}

/* If the DGA theme uses a dedicated overlay element */
.hero-container .carousel-item.no-overlay .overlay,
.hero-container .carousel-item.no-overlay .carousel-overlay,
.hero-container .carousel-item.no-overlay .hero-overlay {
    display: none !important;
}
	</style>
