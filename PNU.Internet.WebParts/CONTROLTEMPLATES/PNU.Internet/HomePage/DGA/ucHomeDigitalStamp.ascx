<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeDigitalStamp.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeDigitalStamp" %>

   <dga-digital-signature>
      <div class="dga-digital-signature border-bottom">
        <div class="container">
          <div class="d-flex justify-content-between py-1">
            <div class="d-flex flex-column flex-lg-row align-items-start align-items-lg-center gap-lg-2">
              <div class="d-flex align-items-center gap-2">
                <img src="/Style%20Library/DGA/public/images/saudi-flag.svg" width="20" height="14" alt="" aria-hidden="true"
                  class="flex-shrink-0" loading="eager" decoding="async">
                <p class="mb-0 small fw-medium"> موقع حكومي رسمي تابع لحكومة المملكة العربية السعودية </p>
              </div>
              <a type="button" data-bs-toggle="collapse" data-bs-target="#digitalContentCollapse" aria-expanded="false"
                aria-controls="digitalContentCollapse"
                class="small d-flex align-items-center px-0 verify-btn collapsed">
                <span>كيف تتحقق</span>
                <span class="d-inline-flex fs-6">
                  <i class="verify-icon hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i>
                </span>
              </a>
            </div>
          </div>
          <div id="digitalContentCollapse" class="py-4 collapse">
            <div class="d-flex flex-column gap-4 flex-lg-row justify-content-lg-between">
              <div class="d-flex gap-3 align-items-start">
                <div class="digital-content-icon">
                  <i class="hgi-stroke hgi-rounded hgi-link-04 text-primary fs-5" aria-hidden="true"></i>
                </div>
                <div>
                  <h3 class="fw-semibold h5"> روابط المواقع الالكترونية الرسمية السعودية تنتهي بـ <span
                      class="text-primary">.edu.sa</span>
                  </h3>
                  <p class="mb-0"> جميع روابط المواقع الرسمية التعليمية في المملكة العربية السعودية تنتهي بـ sch.sa أو
                    edu.sa

                  </p>
                </div>
              </div>
              <div class="d-flex gap-3 align-items-start">
                <div class="digital-content-icon">
                  <i class="hgi-stroke hgi-rounded hgi-square-lock-password text-primary fs-5" aria-hidden="true"></i>
                </div>
                <div>
                  <h3 class="fw-semibold h5"> المواقع الالكترونية الحكومية تستخدم بروتوكول <span
                      class="text-primary">HTTPS</span> للتشفير و الأمان. </h3>
                  <p class="mb-0"> المواقع الالكترونية الآمنة في المملكة العربية السعودية تستخدم بروتوكول HTTPS للتشفير.
                  </p>
                </div>
              </div>
            </div>
            <div class="bg-white p-2 mt-4 rounded d-flex align-items-center gap-3">
              <img alt="شعار هيئة الحكومة الرقمية" width="32" height="32" loading="lazy" fetchpriority="auto"
                src="/Style%20Library/DGA/public/images/dga-logo.svg" decoding="async">
              <div class="flex-grow-1 d-flex flex-column flex-lg-row align-items-lg-center gap-2">
                <p class="mb-0">مسجل لدى هيئة الحكومة الرقمية برقم:</p>
                <a class="external-link"
                  href="https://raqmi.dga.gov.sa/platforms/platforms/82920d00-7973-42b5-e1c6-08dd66df7e11/platform-license"
                  target="_blank" rel="external noopener noreferrer">20250417424</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </dga-digital-signature>
    