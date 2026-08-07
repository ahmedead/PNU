<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeWhy.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeWhy" %>





 <%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="py-5" data-aos="fade-up" aria-labelledby="reasons-section-title">
  <div class="container">
    <div class="row g-5 align-items-center">
      <div class="col-12 col-lg-8">
        <asp:Repeater ID="rptWhyUsMain" runat="server">
          <ItemTemplate>
            <h2 id="reasons-section-title" class="mb-4">
              <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
            </h2>
            <p class="mb-4">
              <%# SPFactory.GetLocalizedTitle(Eval("Desc"), Eval("Desc_EN")) %>
            </p>
          </ItemTemplate>
        </asp:Repeater>

        <div class="pt-2 g-4 row">
          <asp:Repeater ID="rptWhyUsDetails" runat="server">
            <ItemTemplate>
              <div class="col-12 col-md-4 col-sm-6">
                <div class="card h-100">
                  <div class="card-body">
                    <div class="icon-container mb-3">
                      <i class="hgi hgi-stroke <%# Eval("IconClass") %> fs-3" aria-hidden="true"></i>
                    </div>
                    <h3 class="card-title h5">
                      <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                    </h3>
                  </div>
                </div>
              </div>
            </ItemTemplate>
          </asp:Repeater>
        </div>
      </div>

      <div class="col-12 col-lg-4">
        <div id="reasonsCarousel" data-bs-ride="carousel" class="carousel slide rounded-3" role="region" aria-roledescription="carousel" aria-label="صور من جامعة الأميرة نورة">
          <div class="carousel-inner rounded-3">
            <div class="carousel-item active" role="group" aria-roledescription="slide" aria-label="1 من 2">
              <img src="/Style Library/NewStyle/UI5/images/unsplash_l1nRZW-hjGY.png" class="d-block w-100" alt="حرم جامعة الأميرة نورة" />
            </div>
            <div class="carousel-item" role="group" aria-roledescription="slide" aria-label="2 من 2">
              <img src="/Style Library/NewStyle/UI5/images/unsplash_l1nRZW-hjGY.png" class="d-block w-100" alt="مشاهد من جامعة الأميرة نورة" />
            </div>
          </div>
          <div class="carousel-indicators">
            <button type="button" data-bs-target="#reasonsCarousel" class="active" data-bs-slide-to="0" aria-label="الشريحة 1"></button>
            <button type="button" data-bs-target="#reasonsCarousel" data-bs-slide-to="1" aria-label="الشريحة 2"></button>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>