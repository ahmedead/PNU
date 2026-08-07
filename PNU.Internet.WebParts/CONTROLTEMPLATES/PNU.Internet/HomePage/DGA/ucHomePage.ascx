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
    
    
    <ucHomeBanner:ucHomeBanner runat="server" id="ucHomeBanner" />
    

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