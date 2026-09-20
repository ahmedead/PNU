<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomePage.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomePage" %>




<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeBanner.ascx" TagPrefix="ucHomeBanner" TagName="ucHomeBanner" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeNews.ascx" TagPrefix="ucHomeNews" TagName="ucHomeNews" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeAds.ascx" TagPrefix="ucHomeBanner" TagName="ucHomeAds" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeStatistics.ascx" TagPrefix="ucHomeBanner" TagName="ucHomeStatistics" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeEServices.ascx" TagPrefix="ucHomeBanner" TagName="ucHomeEServices" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeWhy.ascx" TagPrefix="ucHomeBanner" TagName="ucHomeWhy" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeNoraFuture.ascx" TagPrefix="ucHomeBanner" TagName="ucHomeNoraFuture" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeAchievements.ascx" TagPrefix="ucHomeBanner" TagName="ucHomeAchievements" %>




<main id="main-content" class="dga-main-body" tabindex="-1">
    <h1 class="visually-hidden">جامعة الأميرة نورة بنت عبد الرحمن - الموقع الرسمي</h1>
    
    
     <!-- National Day Hero Slider (سلايدر اليوم الوطني التفاعلي) -->
    <section class="nd96-hero nd96-hero--heritage" id="nd96Hero" aria-label="سلايدر اليوم الوطني">
      <div class="nd96-hero__deck">
        <button type="button" class="nd96-hero__slide nd96-hero__slide--heritage is-active is-near" style="--nd96-x:0px;--nd96-rel:0" aria-label="عِزّنا بأصالتنا" aria-pressed="true"></button>
        <button type="button" class="nd96-hero__slide nd96-hero__slide--courage is-near" style="--nd96-x:491px;--nd96-rel:1" aria-label="عِزّنا بشجاعتنا" aria-pressed="false"></button>
        <button type="button" class="nd96-hero__slide nd96-hero__slide--ambition" style="--nd96-x:536.8px;--nd96-rel:2" aria-label="عِزّنا بهمّتنا" aria-pressed="false"></button>
        <button type="button" class="nd96-hero__slide nd96-hero__slide--generosity" style="--nd96-x:582.6px;--nd96-rel:-3" aria-label="عِزّنا بجودنا" aria-pressed="false"></button>
        <button type="button" class="nd96-hero__slide nd96-hero__slide--kindness" style="--nd96-x:628.4px;--nd96-rel:-2" aria-label="عِزّنا بكرمنا" aria-pressed="false"></button>
        <button type="button" class="nd96-hero__slide nd96-hero__slide--vision is-near" style="--nd96-x:674.2px;--nd96-rel:-1" aria-label="عِزّنا برؤيتنا" aria-pressed="false"></button>
      </div>
      <div class="container">
        <div class="nd96-hero__content">
          <div class="nd96-hero__lockup" aria-hidden="true"></div>
          <h2 class="nd96-hero__title" aria-label="عِزّنا بأصالتنا">
            <span aria-hidden="true" class="nd96-hero__lead">عِزّنا</span>
            <span class="nd96-hero__title-typed" aria-hidden="true">
              <span class="nd96-hero__typed-text">بأصالتنا</span><i class="nd96-hero__caret"></i>
            </span>
          </h2>
          <p class="nd96-hero__body nd96-hero__body--wide is-in">الأصالة في المجتمع السعودي تعكس ارتباط الناس بجذورهم وتاريخهم، واعتزازهم بموروثهم. فهي تظهر في التمسك بالعادات والتقاليد، واستمرار القيم التي تتوارثها الأجيال.</p>
          <p class="nd96-hero__body nd96-hero__body--stacked is-in">الأصالة في المجتمع السعودي تعكس ارتباط الناس بجذورهم واعتزازهم بإرثهم.</p>
          <div class="nd96-hero__controls">
            <button type="button" class="nd96-hero__arrow nd96-hero__arrow--next" aria-label="الشريحة السابقة">
              <svg viewBox="0 0 24 24" aria-hidden="true" focusable="false">
                <path d="M15 18l-6-6 6-6" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
              </svg>
            </button>
            <div class="nd96-hero__bullets">
              <button type="button" class="nd96-hero__bullet is-active" aria-label="الانتقال إلى الشريحة 1" aria-pressed="true"></button>
              <button type="button" class="nd96-hero__bullet" aria-label="الانتقال إلى الشريحة 2" aria-pressed="false"></button>
              <button type="button" class="nd96-hero__bullet" aria-label="الانتقال إلى الشريحة 3" aria-pressed="false"></button>
              <button type="button" class="nd96-hero__bullet" aria-label="الانتقال إلى الشريحة 4" aria-pressed="false"></button>
              <button type="button" class="nd96-hero__bullet" aria-label="الانتقال إلى الشريحة 5" aria-pressed="false"></button>
              <button type="button" class="nd96-hero__bullet" aria-label="الانتقال إلى الشريحة 6" aria-pressed="false"></button>
            </div>
            <button type="button" class="nd96-hero__arrow nd96-hero__arrow--prev" aria-label="الشريحة التالية">
              <svg viewBox="0 0 24 24" aria-hidden="true" focusable="false">
                <path d="M15 18l-6-6 6-6" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
              </svg>
            </button>
          </div>
        </div>
      </div>
    </section>
    <%--<ucHomeBanner:ucHomeBanner runat="server" id="ucHomeBanner" />--%>
    

    <ucHomeBanner:ucHomeEServices runat="server" id="ucHomeEServices" RowsCount="5"  />
    

    <ucHomeNews:ucHomeNews runat="server" id="ucHomeNews" RowsCount="5" />


    <ucHomeBanner:ucHomeAds runat="server" id="ucHomeAds"  RowsCount="5" />


    <ucHomeBanner:ucHomeStatistics runat="server" id="ucHomeStatistics" />


    <ucHomeBanner:ucHomeNoraFuture runat="server" id="ucHomeNoraFuture" />
    

    <ucHomeBanner:ucHomeWhy runat="server" id="ucHomeWhy" />

    
    <ucHomeBanner:ucHomeAchievements runat="server" id="ucHomeAchievements" IsHome = "TRUE"/>


  </main>

 
  
  <script src="/Style%20Library/DGA/public/vendor/js/bootstrap.bundle.min.js"></script>
  <script src="/Style%20Library/DGA/public/vendor/js/swiper-bundle.min.js"></script>
  <script src="/Style%20Library/DGA/public/js/app.js"></script>
  <script>
      window.addEventListener("load", () => {
          const iconLink = document.createElement("link");
          iconLink.rel = "stylesheet";
          iconLink.href = "/Style%20Library/DGA/public/vendor/hugeicons/hgi-stroke-rounded.css";
          document.head.appendChild(iconLink);
      });
  </script>