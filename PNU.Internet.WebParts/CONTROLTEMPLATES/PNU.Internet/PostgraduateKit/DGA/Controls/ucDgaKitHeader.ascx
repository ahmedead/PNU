<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaKitHeader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls.ucDgaKitHeader" %>

<header class="dga-kit-header-wrapper" role="banner">
  <a class="visually-hidden-focusable" href="#main-content"><%= GetResource("SkipToMain", "تخطي إلى المحتوى الرئيسي", "Skip to main content") %></a>

  <%-- Saudi Digital Signature --%>
  <div class="dga-digital-signature border-bottom">
    <div class="container">
      <div class="d-flex justify-content-between py-1">
        <div class="d-flex flex-column flex-lg-row align-items-start align-items-lg-center gap-lg-2">
          <div class="d-flex align-items-center gap-2">
            <img src="/Style%20Library/DGA/PostgraduateKit/public/images/saudi-flag.svg" width="20" height="14" alt="" aria-hidden="true"
                 class="flex-shrink-0" loading="eager" decoding="async">
            <p class="mb-0 small fw-medium"> <%= GetResource("GovStamp", "موقع حكومي رسمي تابع لحكومة المملكة العربية السعودية", "An official government website of Saudi Arabia") %> </p>
          </div>
          <a type="button" data-bs-toggle="collapse" data-bs-target="#digitalContentCollapse" aria-expanded="false"
             aria-controls="digitalContentCollapse"
             class="small d-flex align-items-center px-0 verify-btn collapsed">
            <span><%= GetResource("HowToVerify", "كيف تتحقق", "How to verify") %></span>
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
              <h3 class="fw-semibold h5"> <%= GetResource("DomainTipTitle", "روابط المواقع الالكترونية الرسمية السعودية تنتهي بـ", "Official Saudi website links end in") %> <span class="text-primary">.edu.sa</span></h3>
              <p class="mb-0"> <%= GetResource("DomainTipText", "جميع روابط المواقع الرسمية التعليمية في المملكة العربية السعودية تنتهي بـ sch.sa أو edu.sa", "All official educational website links in Saudi Arabia end in sch.sa or edu.sa") %></p>
            </div>
          </div>
          <div class="d-flex gap-3 align-items-start">
            <div class="digital-content-icon">
              <i class="hgi-stroke hgi-rounded hgi-square-lock-password text-primary fs-5" aria-hidden="true"></i>
            </div>
            <div>
              <h3 class="fw-semibold h5"> <%= GetResource("HttpsTipTitle", "المواقع الالكترونية الحكومية تستخدم بروتوكول", "Government websites use") %> <span class="text-primary">HTTPS</span> <%= GetResource("HttpsTipSuffix", "للتشفير و الأمان.", "for encryption and security.") %> </h3>
              <p class="mb-0"> <%= GetResource("HttpsTipText", "المواقع الالكترونية الآمنة في المملكة العربية السعودية تستخدم بروتوكول HTTPS للتشفير.", "Secure websites in Saudi Arabia use the HTTPS protocol for encryption.") %></p>
            </div>
          </div>
        </div>
        <div class="bg-white p-2 mt-4 rounded d-flex align-items-center gap-3">
          <img alt="شعار هيئة الحكومة الرقمية" width="32" height="32" loading="lazy" fetchpriority="auto"
               src="/Style%20Library/DGA/PostgraduateKit/public/images/dga-logo.svg" decoding="async">
          <div class="flex-grow-1 d-flex flex-column flex-lg-row align-items-lg-center gap-2">
            <p class="mb-0"><%= GetResource("DgaRegNo", "مسجل لدى هيئة الحكومة الرقمية برقم:", "Registered with Digital Government Authority No:") %></p>
            <a class="external-link"
               href="https://raqmi.dga.gov.sa/platforms/platforms/82920d00-7973-42b5-e1c6-08dd66df7e11/platform-license"
               target="_blank" rel="external noopener noreferrer">20250417424</a>
          </div>
        </div>
      </div>
    </div>
  </div>

  <%-- Main Navbar --%>
  <div class="sticky-top border-bottom dga-header" role="none">
    <nav class="navbar navbar-expand-lg bg-white h-100 py-0" aria-label="<%= GetResource("MainNav", "التنقل الرئيسي", "Main Navigation") %>">
      <div class="container h-100">
        <div class="d-flex align-items-center gap-2 d-lg-none">
          <button type="button" data-bs-toggle="offcanvas" data-bs-target="#mobileSideBar"
                  aria-controls="mobileSideBar" aria-expanded="false" aria-label="فتح القائمة"
                  class="btn icon-btn btn-secondary no-hover-bg border-0">
            <span class="d-inline-flex fs-5">
              <i class="hgi hgi-stroke hgi-menu-01" aria-hidden="true"></i>
            </span>
          </button>
        </div>

        <%-- Logo --%>
        <a class="outline-none" href="<%= ResolveHomeUrl() %>" aria-label="<%= GetResource("Home", "الانتقال إلى الصفحة الرئيسية", "Go to homepage") %>">
          <picture>
            <source media="(min-width: 992px)"
                    srcset="/Style%20Library/DGA/PostgraduateKit/public/images/pnu-logo-ar-h.svg, /Style%20Library/DGA/PostgraduateKit/public/images/pnu-logo-ar-h.svg 2x">
            <img height="50" title="<%= GetResource("UnivName", "جامعة الأميرة نورة", "Princess Nourah University") %>"
                 alt="<%= GetResource("UnivName", "شعار جامعة الأميرة نورة", "Princess Nourah University") %>"
                 loading="eager" fetchpriority="high"
                 src="/Style%20Library/DGA/PostgraduateKit/public/images/pnu-logo-icon-only.svg" decoding="async">
          </picture>
        </a>

        <%-- Mobile Search Button --%>
        <div class="d-flex align-items-center gap-2 d-lg-none">
          <div class="dropdown header-dropdown position-static">
            <a id="SearchDropdownBtnMobile" href="#" role="button" data-bs-toggle="dropdown"
               data-bs-auto-close="outside" aria-expanded="false" aria-controls="searchDropdownMenuMobile"
               class="btn icon-btn btn-secondary text-decoration-none no-hover-bg border-0" aria-label="فتح البحث">
              <span class="d-inline-flex fs-5">
                <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
              </span>
            </a>
            <div id="searchDropdownMenuMobile" aria-labelledby="SearchDropdownBtnMobile" class="dropdown-menu start-0 end-0">
              <div class="container">
                <div class="d-flex justify-content-end">
                  <button id="searchDropdownCloseMobile" type="button" class="btn btn-secondary icon-btn" aria-label="إغلاق البحث">
                    <span class="d-inline-flex fs-5">
                      <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
                    </span>
                  </button>
                </div>
                <div class="py-4">
                  <div class="d-flex gap-3" role="search" aria-label="بحث في الموقع">
                    <div class="form-control-container has-icon flex-grow-1">
                      <input id="globalSearchInputMobile" type="search" name="q" autocomplete="off" class="form-control" placeholder="<%= GetResource("SearchPlaceholder", "بحث", "Search") %>" />
                    </div>
                    <button type="button" class="btn btn-primary" aria-label="بحث"><%= GetResource("SearchBtn", "بحث", "Search") %></button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div id="expanded-navbar-nav" class="collapse navbar-collapse h-100">
        </div>

        <%-- Desktop Search --%>
        <div class="d-none d-lg-block dropdown header-dropdown h-100 position-static">
          <a id="SearchDropdownBtn" href="#" role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside"
             aria-expanded="false" aria-controls="searchDropdownMenu"
             class="nav-link position-relative p-0 px-3 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium">
            <i class="hgi-stroke hgi-search-01" aria-hidden="true"></i>
            <span class="ps-2"><%= GetResource("SearchBtn", "بحث", "Search") %></span>
          </a>
          <div id="searchDropdownMenu" aria-labelledby="SearchDropdownBtn" class="dropdown-menu start-0 end-0">
            <div class="container">
              <div class="d-flex justify-content-end">
                <button id="searchDropdownClose" type="button" class="btn btn-secondary icon-btn" aria-label="إغلاق البحث">
                  <span class="d-inline-flex fs-5">
                    <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
                  </span>
                </button>
              </div>
              <div class="py-4">
                <div class="d-flex gap-3" role="search" aria-label="بحث في الموقع">
                  <div class="form-control-container has-icon flex-grow-1">
                    <input id="globalSearchInput" type="search" name="q" autocomplete="off" class="form-control" placeholder="<%= GetResource("SearchPlaceholder", "بحث", "Search") %>" />
                  </div>
                  <button type="button" class="btn btn-primary" aria-label="بحث"><%= GetResource("SearchBtn", "بحث", "Search") %></button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <%-- Desktop Language Selector --%>
        <div class="d-none d-lg-block dropdown header-dropdown h-100">
          <a id="LanguagesDropdownBtn" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false"
             aria-controls="languagesDropdownMenu"
             class="nav-link position-relative p-0 px-3 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium">
            <i class="hgi-stroke hgi-translation" aria-hidden="true"></i>
            <span class="ps-2"><%= IsArabic ? "العربية" : "English" %></span>
          </a>
          <ul id="languagesDropdownMenu" aria-labelledby="LanguagesDropdownBtn" class="dropdown-menu p-3 gap-1">
            <li>
              <a href="/ar/Deanship/postgraduate/Pages/academic-kit.aspx" role="button" lang="ar" aria-label="التبديل إلى اللغة العربية"
                 class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1 my-2">
                <img height="20" width="32" src="/Style%20Library/DGA/PostgraduateKit/public/images/flags/sa.svg" alt="العربية" decoding="async"> العربية
              </a>
            </li>
            <li>
              <a href="/en/Deanship/postgraduate/Pages/academic-kit.aspx" role="button" lang="en" aria-label="Switch to English"
                 class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1 my-2">
                <img height="20" width="32" src="/Style%20Library/DGA/PostgraduateKit/public/images/flags/gb.svg" alt="English" decoding="async"> English
              </a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </div>

  <%-- Mobile Offcanvas Sidebar --%>
  <aside class="header-offcanvas d-lg-none">
    <div tabindex="-1" id="mobileSideBar" class="offcanvas offcanvas-start" role="dialog" aria-modal="true" aria-labelledby="mobileSideBarLabel">
      <div class="offcanvas-header align-items-center px-4 justify-content-between">
        <h2 id="mobileSideBarLabel" class="visually-hidden"><%= GetResource("MainMenu", "القائمة الرئيسية", "Main Menu") %></h2>
        <div>
          <a href="<%= ResolveHomeUrl() %>" class="d-inline-block">
            <img width="140" height="50" title="<%= GetResource("UnivName", "جامعة الأميرة نورة", "Princess Nourah University") %>"
                 alt="<%= GetResource("UnivName", "شعار جامعة الأميرة نورة", "Princess Nourah University") %>" loading="lazy" fetchpriority="low"
                 src="/Style%20Library/DGA/PostgraduateKit/public/images/pnu-logo-ar-h.png" srcset="/Style%20Library/DGA/PostgraduateKit/public/images/pnu-logo-ar-h-2x.png 2x" decoding="async">
          </a>
        </div>
        <button type="button" class="btn p-0" data-bs-dismiss="offcanvas" aria-label="إغلاق القائمة">
          <span class="d-inline-flex fs-2">
            <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
          </span>
        </button>
      </div>
      <div class="offcanvas-body px-4">
        <nav aria-label="اختيار اللغة" class="mb-3">
          <div class="fw-semibold mb-2"><%= GetResource("Lang", "اللغة", "Language") %></div>
          <ul class="list-unstyled d-flex flex-column gap-2 mb-0">
            <li>
              <a href="/ar/Deanship/postgraduate/Pages/academic-kit.aspx" role="button" lang="ar" aria-label="التبديل إلى اللغة العربية"
                 class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1">
                <img height="20" width="32" src="/Style%20Library/DGA/PostgraduateKit/public/images/flags/sa.svg" alt="العربية" decoding="async"> العربية
              </a>
            </li>
            <li>
              <a href="/en/Deanship/postgraduate/Pages/academic-kit.aspx" role="button" lang="en" aria-label="Switch to English"
                 class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1">
                <img height="20" width="32" src="/Style%20Library/DGA/PostgraduateKit/public/images/flags/gb.svg" alt="English" decoding="async"> English
              </a>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  </aside>
</header>
