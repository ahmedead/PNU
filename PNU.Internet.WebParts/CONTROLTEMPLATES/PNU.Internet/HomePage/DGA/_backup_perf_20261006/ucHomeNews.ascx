<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeNews.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeNews" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="py-5" data-aos="fade-up" aria-labelledby="news-section-title">
      <div class="container">
        <div class="mb-4">
          <div class="d-flex justify-content-between align-items-center gap-2">
            <h2 id="news-section-title" class="mb-0"><asp:Literal runat="server" Text="<%$ Resources: PNUres, News %>" /></h2>
            <a class="btn btn-outline-secondary fw-semibold" href="<%= SPFactory.GetSiteURL() %>MediaCenter/Pages/AllNews.aspx">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_MoreAllNews %>" />
            </a>
          </div>
        </div>

        <dga-swiper class="swiper dga-news-swiper pt-2" role="region" aria-label="أحدث الأخبار">
          <swiper-container class="swiper-wrapper">
      <asp:Repeater ID="NewsRepeater" runat="server">
<ItemTemplate>
            <swiper-slide class="swiper-slide flex-grow-1" data-aos="fade-up" data-aos-delay="0">
              <article class="card h-100 pnu-news-card">
                <div class="card-body d-flex flex-column placeholder-glow h-100">
                  <img width="400" height="250" class="w-100 rounded-2"
                    alt="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>" loading="lazy" fetchpriority="low"
                    src="<%# Eval("AttachmentURL") %>"
                    srcset="<%# Eval("AttachmentURL") %> 2x"
                    sizes="(min-width: 1200px) 360px, (min-width: 768px) 45vw, 90vw" decoding="async">
                  <div class="flex-grow-1">
                    <h3 class="card-title"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h3>
                    <p class="card-text line-clamp max-clamp-line-4">
                       <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                    </p>
                  </div>
                  <div class="mt-auto d-flex flex-column gap-3">
                    <small class="d-flex gap-2 align-items-center">
                      <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                      <time datetime="2026-02-02"><%# Eval("MediaDate") %></time>
                    </small>
                    <div class="d-flex justify-content-start">
                      <a class="btn btn-primary" href="<%# Eval("DetailsURL") %>">
                          <asp:Literal runat="server" Text="<%$ Resources: PNUres, ReadMore %>" />
                      </a>
                    </div>
                  </div>
                </div>
              </article>
            </swiper-slide>
        
            </ItemTemplate>
</asp:Repeater>
          </swiper-container>
          <div class="d-flex justify-content-between align-items-center mt-3">
            <div class="d-flex gap-1">
              <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-prev" aria-label="السابق">
                <span class="d-inline-flex fs-4 lh-1">
                  <i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i>
                </span>
              </button>
              <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-next" aria-label="التالي">
                <span class="d-inline-flex fs-4 lh-1">
                  <i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i>
                </span>
              </button>
            </div>
            <div class="d-flex justify-content-end dga-swiper-pagination"></div>
          </div>
        </dga-swiper>
      </div>
    </section>

