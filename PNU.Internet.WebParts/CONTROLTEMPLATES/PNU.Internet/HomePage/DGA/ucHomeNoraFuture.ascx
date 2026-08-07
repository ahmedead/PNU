<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeNoraFuture.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeNoraFuture" %>


<section class="gray colored-section py-5" data-aos="fade-up" aria-labelledby="future-section-title">
  <div class="container">
    <div class="mb-4">
      <div class="d-flex justify-content-between align-items-start gap-2">
        <h2 id="future-section-title" class="mb-0">
          <SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,StudyAtPNU%>" EncodeMethod="HtmlEncode"/>
        </h2>
      </div>
    </div>
    
    <dga-swiper class="swiper dga-eservices-swiper pt-2" role="region" aria-label="برامج ومستقبل نورة">
      <swiper-container class="swiper-wrapper">
        
        <swiper-slide class="swiper-slide" data-aos="fade-up" data-aos-delay="0">
          <div class="card nav-card h-100">
            <div class="d-flex card-body flex-column gap-4">
              <div class="icon-container">
                 <i class="hgi hgi-stroke fs-3 hgi-book-01" aria-hidden="true"></i>
              </div>
              <div>
                <h3 class="card-title"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,UndergraduatePrograms%>" EncodeMethod="HtmlEncode"/></h3>
                <p class="card-text"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,UndergraduateProgramsCount%>" EncodeMethod="HtmlEncode"/></p>
              </div>
              <div class="d-flex justify-content-end mt-auto">
                <a class="btn btn-secondary stretched-link" href='<SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,UndergraduateLink%>" EncodeMethod="HtmlEncode"/>'>
                  <i class="hgi hgi-stroke fs-4 hgi-arrow-left-02" aria-hidden="true"></i>
                </a>
              </div>
            </div>
          </div>
        </swiper-slide>

        <swiper-slide class="swiper-slide" data-aos="fade-up" data-aos-delay="100">
          <div class="card nav-card h-100">
            <div class="d-flex card-body flex-column gap-4">
              <div class="icon-container">
                 <i class="hgi hgi-stroke fs-3 hgi-book-02" aria-hidden="true"></i>
              </div>
              <div>
                <h3 class="card-title"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,PostgraduatePrograms%>" EncodeMethod="HtmlEncode"/></h3>
                <p class="card-text"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,PostgraduateProgramsCount%>" EncodeMethod="HtmlEncode"/></p>
              </div>
              <div class="d-flex justify-content-end mt-auto">
                <a class="btn btn-secondary stretched-link" href='<SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,PostgraduateLink%>" EncodeMethod="HtmlEncode"/>'>
                  <i class="hgi hgi-stroke fs-4 hgi-arrow-left-02" aria-hidden="true"></i>
                </a>
              </div>
            </div>
          </div>
        </swiper-slide>

        <swiper-slide class="swiper-slide" data-aos="fade-up" data-aos-delay="200">
          <div class="card nav-card h-100">
            <div class="d-flex card-body flex-column gap-4">
              <div class="icon-container">
                 <i class="hgi hgi-stroke fs-3 hgi-diploma" aria-hidden="true"></i>
              </div>
              <div>
                <h3 class="card-title"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,DiplomaPrograms%>" EncodeMethod="HtmlEncode"/></h3>
                <p class="card-text"><SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,DiplomaProgramsCount%>" EncodeMethod="HtmlEncode"/></p>
              </div>
              <div class="d-flex justify-content-end mt-auto">
                <a class="btn btn-secondary stretched-link" href='<SharePoint:EncodedLiteral runat="server" Text="<%$Resources:CommonGlobalResources,DiplomaLink%>" EncodeMethod="HtmlEncode"/>'>
                  <i class="hgi hgi-stroke fs-4 hgi-arrow-left-02" aria-hidden="true"></i>
                </a>
              </div>
            </div>
          </div>
        </swiper-slide>

      </swiper-container>
      
      <div class="d-flex justify-content-between align-items-center mt-3">
        <div class="d-flex gap-1">
          <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-prev" aria-label="Previous"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip"></i></button>
          <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-next" aria-label="Next"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip"></i></button>
        </div>
        <div class="dga-swiper-pagination"></div>
      </div>
    </dga-swiper>
  </div>
</section>

