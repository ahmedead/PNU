<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>




<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeHome.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucCollegeHome" %>




<style>
    .row.justify-content-between {
        padding-top: 70px;
    }
</style>

 
   <section class="faculty-video-section position-relative MainData"></section>
   <section class="bg-semi-light FacultyStatistics"></section>
   <section class="researcher-and-contributor DeanSpeech"></section>

<section class="our-sections position-relative">
    <div class="container">
        <div class="d-flex justify-content-center">
            <div>
                <h1 class="title text-white fw-bold px-2 border-start border-primary mb-5">أقسامنا
            <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                </h1>
            </div>

        </div>
        <div class="position-relative also-know bg-transparent">
            <div class="swiperbox-bg-1 d-none d-lg-block"></div>
            <div dir="ltr" class="swiper swiper-container-5 me-5">
                <div class="swiper-wrapper FacultySections">
                </div>

            </div>
        </div>
        <div class="swiper-sections-navigation d-none d-lg-flex">
            <div class=" swiper-next me-2">
                <svg width="50" height="30">
                    <use xlink:href="#arrowRight" />
                </svg>
            </div>
            <div class="swiper-prev">
                <svg width="50" height="30">
                    <use xlink:href="#arrowLeft" />
                </svg>
            </div>
        </div>
        <div class="swiperbox-bg-2 d-none d-lg-block"></div>
    </div>

</section>

   
   
<section class="faculty-members-section position-relative bg-semi-light">


    <div class="swiperbox-faculty-members-1 d-none d-lg-block"></div>
    <div class="swiperbox-faculty-members-2 d-none d-lg-block"></div>
</section>





<section class="academic-knowledge-section position-relative my-5 py-5">
    <div class="container">
        <div class="d-flex">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4">معرفاتنا
            الأكاديمية
            <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                <img class="d-none d-md-inline-block" src="/Style Library/NewStyle/UI5/images/coiled-arrow.png">
            </h1>
        </div>
        <div dir="rtl" class="swiper swiper-container-7">
            <div class="swiper-wrapper py-5 AcademicKnowledge">
            </div>
        </div>
        <div class="d-none d-lg-flex justify-content-center">
            <div class="swiper-prev-2 me-2">
                <svg width="50" height="30">
                    <use xlink:href="#arrowRight" />
                </svg>
            </div>
            <div class=" swiper-next-2">
                <svg width="50" height="30">
                    <use xlink:href="#arrowLeft" />
                </svg>
            </div>
        </div>
    </div>
</section>












