<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaKitFooter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls.ucDgaKitFooter" %>

<footer class="dga-footer pt-5" role="contentinfo">
  <div class="container">
    <div class="row g-4 pt-3" id="footerNavContainer">
      <%-- Col 1: Overview --%>
      <div class="col-12 col-lg-3">
        <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6" style="border-bottom:1px solid rgba(255,255,255,.3)">
          <%= GetResource("Overview", "نظرة عامة", "Overview") %></h3>
        <button aria-controls="footerNav1" aria-expanded="false"
          class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
          data-bs-target="#footerNav1" data-bs-toggle="collapse"
          style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
          <span class="mb-0 h6"><%= GetResource("Overview", "نظرة عامة", "Overview") %></span>
          <span class="d-inline-flex fs-5">
            <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
          </span>
        </button>
        <div class="collapse d-lg-block" id="footerNav1" data-bs-parent="#footerNavContainer">
          <div class="d-flex flex-column gap-2 mt-2">
            <a href="<%= ResolveUrl("/ar/Pages/faqs.aspx") %>" target="_self" class="text-white small"><%= GetResource("Faqs", "الأسئلة الشائعة", "FAQs") %></a>
            <a href="https://techcare.pnu.edu.sa/" target="_blank" rel="noopener noreferrer" class="text-white small external-link"><%= GetResource("Techcare", "تيك كير لتقنية المعلومات", "TechCare IT") %></a>
            <a href="https://www.kaauh.edu.sa/" target="_blank" rel="noopener noreferrer" class="text-white small external-link"><%= GetResource("Hospital", "المستشفى الجامعي", "University Hospital") %></a>
            <a href="https://nourahprize.pnu.edu.sa/" target="_blank" rel="nofollow noopener noreferrer" class="text-white small external-link"><%= GetResource("Prize", "جائزة الأميرة نورة", "Princess Nourah Prize") %></a>
            <a href="https://tawasulnourah.pnu.edu.sa/" target="_blank" rel="noopener noreferrer" class="text-white small external-link"><%= GetResource("Tawasul", "تواصل نورة", "Tawasul Nourah") %></a>
            <a href="https://eservice.pnu.edu.sa/" target="_blank" rel="noopener noreferrer" class="text-white small external-link"><%= GetResource("EServices", "بوابة الخدمات الإلكترونية", "E-Services Portal") %></a>
            <a href="https://graduates.pnu.edu.sa/ar/Pages/default.aspx" target="_blank" rel="noopener noreferrer" class="text-white small external-link"><%= GetResource("GradPortal", "منصة الخريجات", "Graduates Platform") %></a>
          </div>
        </div>
      </div>

      <%-- Col 2: Opportunities & Life --%>
      <div class="col-12 col-lg-3">
        <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6" style="border-bottom:1px solid rgba(255,255,255,.3)">
          <%= GetResource("Opportunities", "فرصك في نورة", "Opportunities at Nourah") %></h3>
        <button aria-controls="footerNav2" aria-expanded="false"
          class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
          data-bs-target="#footerNav2" data-bs-toggle="collapse"
          style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
          <span class="mb-0 h6"><%= GetResource("Opportunities", "فرصك في نورة", "Opportunities at Nourah") %></span>
          <span class="d-inline-flex fs-5">
            <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
          </span>
        </button>
        <div class="collapse d-lg-block" id="footerNav2" data-bs-parent="#footerNavContainer">
          <div class="d-flex flex-column gap-2 mt-2">
            <a href="/ar/NourahOpportunities/Pages/Invest-in-noura.aspx" target="_self" class="text-white small"><%= GetResource("Invest", "استثمر في نورة", "Invest in Nourah") %></a>
            <a href="/ar/NourahOpportunities/Pages/التدريب.aspx" target="_self" class="text-white small"><%= GetResource("Training", "التدريب", "Training") %></a>
            <a href="https://norahattaa.pnu.edu.sa/ar/Pages/default.aspx" target="_blank" rel="nofollow noopener noreferrer" class="text-white small external-link"><%= GetResource("Volunteer", "التطوع", "Volunteering") %></a>
          </div>
        </div>
        <div class="mt-4">
          <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6" style="border-bottom:1px solid rgba(255,255,255,.3)">
            <%= GetResource("UniLife", "الحياة الجامعية", "University Life") %></h3>
          <button aria-controls="footerNav5" aria-expanded="false"
            class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
            data-bs-target="#footerNav5" data-bs-toggle="collapse"
            style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
            <span class="mb-0 h6"><%= GetResource("UniLife", "الحياة الجامعية", "University Life") %></span>
            <span class="d-inline-flex fs-5">
              <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
            </span>
          </button>
          <div class="collapse d-lg-block" id="footerNav5" data-bs-parent="#footerNavContainer">
            <div class="d-flex flex-column gap-2 mt-2">
              <a href="/ar/VirtualTour/Pages/VT/index.html" target="_self" class="text-white small"><%= GetResource("VT", "جولة افتراضية 360", "360 Virtual Tour") %></a>
              <a href="/ar/UniversityLife/Pages/UniversityFacilities.aspx" target="_self" class="text-white small"><%= GetResource("Facilities", "مرافق الجامعة", "Facilities") %></a>
            </div>
          </div>
        </div>
      </div>

      <%-- Col 3: Related Sites --%>
      <div class="col-12 col-lg-3">
        <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6" style="border-bottom:1px solid rgba(255,255,255,.3)">
          <%= GetResource("RelatedSites", "مواقع ذات صلة", "Related Sites") %></h3>
        <button aria-controls="footerNav4" aria-expanded="false"
          class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
          data-bs-target="#footerNav4" data-bs-toggle="collapse"
          style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
          <span class="mb-0 h6"><%= GetResource("RelatedSites", "مواقع ذات صلة", "Related Sites") %></span>
          <span class="d-inline-flex fs-5">
            <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
          </span>
        </button>
        <div class="collapse d-lg-block" id="footerNav4" data-bs-parent="#footerNavContainer">
          <div class="d-flex flex-column gap-2 mt-2">
            <a href="https://www.vision2030.gov.sa/ar" target="_blank" rel="noopener noreferrer" class="text-white small external-link">رؤية المملكة 2030</a>
            <a href="https://www.moe.gov.sa/" target="_blank" rel="noopener noreferrer" class="text-white small external-link">وزارة التعليم</a>
            <a href="https://sdaia.gov.sa/ar/default.aspx" target="_blank" rel="noopener noreferrer" class="text-white small external-link">الهيئة السعودية للبيانات والذكاء الاصطناعي</a>
            <a href="https://nca.gov.sa/ar/" target="_blank" rel="noopener noreferrer" class="text-white small external-link">الهيئة الوطنية للأمن السيبراني</a>
            <a href="https://istitlaa.ncc.gov.sa/ar/Pages/default.aspx" target="_blank" rel="noopener noreferrer" class="text-white small external-link">منصة الاستشارات (استطلاع)</a>
            <a href="https://jadarat.sa/" target="_blank" rel="noopener noreferrer" class="text-white small external-link">المنصة الوطنية للتوظيف (جدارات)</a>
            <a href="https://uap.sa/" target="_blank" rel="noopener noreferrer" class="text-white small external-link">المنصة الوطنية للقبول الموحد (قبول)</a>
          </div>
        </div>
      </div>

      <%-- Col 4: Contact & Social --%>
      <div class="col-12 col-lg-3">
        <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6" style="border-bottom:1px solid rgba(255,255,255,.3)">
          <%= GetResource("ContactUs", "تواصل معنا", "Contact Us") %></h3>
        <button aria-controls="footerNav3" aria-expanded="false"
          class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
          data-bs-target="#footerNav3" data-bs-toggle="collapse"
          style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
          <span class="mb-0 h6"><%= GetResource("ContactUs", "تواصل معنا", "Contact Us") %></span>
          <span class="d-inline-flex fs-5">
            <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
          </span>
        </button>
        <div class="collapse d-lg-block" id="footerNav3" data-bs-parent="#footerNavContainer">
          <div class="d-flex flex-column gap-2 mt-2">
            <div class="text-ltr">
              <a href="tel:920011114" class="text-white small external-link">920011114</a>
            </div>
            <div class="text-ltr">
              <a href="mailto:info@pnu.edu.sa" class="text-white small external-link">info@pnu.edu.sa</a>
            </div>
          </div>
        </div>
        <div class="mt-4">
          <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6" style="border-bottom:1px solid rgba(255,255,255,.3)">
            <%= GetResource("FollowUs", "تابعنا على", "Follow Us") %></h3>
          <div class="d-flex gap-2 mt-2 flex-wrap">
            <a href="https://www.facebook.com/PNUKSA0/" target="_blank" rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn" aria-label="Facebook">
              <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-facebook-02" aria-hidden="true"></i></span>
            </a>
            <a href="https://twitter.com/_PNU_KSA" target="_blank" rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn" aria-label="X">
              <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-new-twitter" aria-hidden="true"></i></span>
            </a>
            <a href="https://www.linkedin.com/school/princess-nourah-bint-abdulrahman-university/" target="_blank" rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn" aria-label="LinkedIn">
              <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-linkedin-02" aria-hidden="true"></i></span>
            </a>
            <a href="https://www.instagram.com/_PNU_KSA/" target="_blank" rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn" aria-label="Instagram">
              <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-instagram" aria-hidden="true"></i></span>
            </a>
            <a href="https://www.youtube.com/channel/UC-0BTuZ46ApPXt6dWONTyGg" target="_blank" rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn" aria-label="YouTube">
              <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-youtube" aria-hidden="true"></i></span>
            </a>
          </div>
        </div>
      </div>

      <%-- Bottom Bar --%>
      <div class="col-12">
        <div class="mt-3 py-4 border-top border-white-50 d-flex flex-column flex-lg-row gap-4 align-items-center justify-content-between">
          <div>
            <div class="d-flex gap-3 mb-2 flex-wrap">
              <a href="/ar/SECURITYCYBER/Pages/Privacy-Policy.aspx" target="_blank" rel="noopener noreferrer" class="text-white small"><%= GetResource("Policies", "سياسات الجامعة", "Policies") %></a>
              <a href="/ar/Pages/SiteMap.aspx" target="_blank" rel="noopener noreferrer" class="text-white small"><%= GetResource("SiteMap", "خريطة الموقع", "Site Map") %></a>
              <a href="https://eparticipation.my.gov.sa/" target="_blank" rel="noopener noreferrer" class="text-white small"><%= GetResource("Participation", "المشاركة الإلكترونية", "E-Participation") %></a>
            </div>
            <p class="text-white small mb-0 opacity-75">
              <%= GetResource("AllRights", "جميع الحقوق محفوظة لجامعة الأميرة نورة بنت عبدالرحمن ©", "All rights reserved Princess Nourah University ©") %> <%= DateTime.Now.Year %>
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</footer>
