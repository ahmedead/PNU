<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Import Namespace="PNU.Internet.WebParts" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeNoraFuture.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeNoraFuture" %>

<asp:PlaceHolder ID="phSection" runat="server">
<section class="gray colored-section py-5" data-aos="fade-up" aria-labelledby="future-section-title">
  <div class="container">
    <div class="mb-4">
      <div class="d-flex justify-content-between align-items-start gap-2">
        <h2 id="future-section-title" class="mb-0">
          <asp:Literal ID="litSectionTitle" runat="server" EnableViewState="false" />
        </h2>
      </div>
    </div>
    
    <dga-swiper class="swiper dga-eservices-swiper pt-2" role="region" aria-label='<%= GetAriaLabel() %>'>
      <swiper-container class="swiper-wrapper">
        <asp:Repeater ID="rptNoraFuture" runat="server" EnableViewState="false">
          <ItemTemplate>
            <swiper-slide class="swiper-slide" data-aos="fade-up" data-aos-delay='<%# (Container.ItemIndex * 100) %>'>
              <div class="card nav-card h-100">
                <div class="d-flex card-body flex-column gap-4">
                  <div class="icon-container">
                     <i class='<%# FormatIconClass(Eval("IconClass")) %>' aria-hidden="true"></i>
                  </div>
                  <div>
                    <h3 class="card-title"><%# GetLocalizedText(Eval("Title"), Eval("Title_EN")) %></h3>
                    <p class="card-text"><%# GetLocalizedText(Eval("Description"), Eval("Description_EN")) %></p>
                  </div>
                  <div class="d-flex justify-content-end mt-auto">
                    <a class="btn btn-secondary stretched-link" href='<%# GetItemUrl(Eval("LinkUrl"), Eval("LinkUrl_EN")) %>' aria-label='<%# GetLocalizedText(Eval("Title"), Eval("Title_EN")) %>'>
                      <i class='<%# SPFactory.IsArabic ? "hgi hgi-stroke fs-4 hgi-arrow-left-02" : "hgi hgi-stroke fs-4 hgi-arrow-right-02" %>' aria-hidden="true"></i>
                    </a>
                  </div>
                </div>
              </div>
            </swiper-slide>
          </ItemTemplate>
        </asp:Repeater>
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
</asp:PlaceHolder>
