<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/HomePage/DGA/ucHomeDigitalStamp.ascx" TagPrefix="uc1" TagName="ucHomeDigitalStamp" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeHeader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeHeader" %>

<%@ Import Namespace="PNU.Internet.WebParts" %>

<script type="text/javascript">
    (function(c,l,a,r,i,t,y){
        c[a]=c[a]||function(){(c[a].q=c[a].q||[]).push(arguments)};
        t=l.createElement(r);t.async=1;t.src="https://www.clarity.ms/tag/"+i;
        y=l.getElementsByTagName(r)[0];y.parentNode.insertBefore(t,y);
    })(window, document, "clarity", "script", "wsyx3gfl6p");
</script>
<dga-header role="banner">
    <a class="visually-hidden-focusable" href="#main-content">تخطي إلى المحتوى الرئيسي</a>

    <uc1:ucHomeDigitalStamp runat="server" id="ucHomeDigitalStamp" />

    <header class="sticky-top border-bottom dga-header" role="none">
        <nav class="navbar navbar-expand-lg bg-white h-100 py-0" aria-label="التنقل الرئيسي">
            <div class="container h-100">

                <%-- MOBILE: hamburger button on the LEFT (before logo) --%>
                <div class="d-flex align-items-center gap-2 d-lg-none">
                    <button type="button" data-bs-toggle="offcanvas" data-bs-target="#mobileSideBar"
                        aria-controls="mobileSideBar" aria-expanded="false" aria-label="فتح القائمة"
                        class="btn icon-btn btn-secondary no-hover-bg border-0">
                        <span class="d-inline-flex fs-5">
                            <i class="hgi hgi-stroke hgi-menu-01" aria-hidden="true"></i>
                        </span>
                    </button>
                </div>

                <%-- LOGO --%>
                <a id="aLinkHome" runat="server" class="outline-none" href="/ar" aria-label="الانتقال إلى الصفحة الرئيسية">
                    <picture>
                        <source media="(min-width: 992px)"
                            srcset="/Style Library/DGA/public/images/pnu-logo-ar-h.png, /Style Library/DGA/public/images/pnu-logo-ar-h-2x.svg 2x">
                        <img id="img2" runat="server" height="50" title="جامعة الأميرة نورة" alt="شعار جامعة الأميرة نورة"
                            loading="eager" fetchpriority="high"
                            src="/Style%20Library/DGA/public/images/pnu-logo-icon-only.svg" decoding="async">
                    </picture>
                </a>

                <%-- MOBILE: search button on the RIGHT (after logo) --%>
                <div class="d-flex align-items-center gap-2 d-lg-none">
                    <div class="dropdown header-dropdown position-static">
                        <a id="SearchDropdownBtnMobile" href="#" role="button" data-bs-toggle="dropdown"
                            data-bs-auto-close="outside" aria-expanded="false" aria-controls="searchDropdownMenuMobile"
                            class="btn icon-btn btn-secondary text-decoration-none no-hover-bg border-0" aria-label="فتح البحث">
                            <span class="d-inline-flex fs-5">
                                <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                            </span>
                        </a>
                        <div id="searchDropdownMenuMobile" aria-labelledby="SearchDropdownBtnMobile"
                            class="dropdown-menu start-0 end-0">
                            <div class="container">
                                <div class="d-flex justify-content-end">
                                    <button id="searchDropdownCloseMobile" type="button" class="btn btn-secondary icon-btn"
                                        aria-label="إغلاق البحث">
                                        <span class="d-inline-flex fs-5">
                                            <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
                                        </span>
                                    </button>
                                </div>
                                <div class="py-4">
                                    <dga-search-input>
                                        <div class="d-flex gap-3" role="search" aria-label="<%# IsArabic ? "ابحث في موقع الجامعة" : "Search the university website" %>">
                                            <div class="form-control-container has-icon">
                                                <label for="globalSearchInputMobile" class="visually-hidden"><%# IsArabic ? "ابحث في موقع الجامعة" : "Search the university website" %></label>
                                                <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                                    <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                                </span>
                                                <asp:TextBox runat="server" ID="txtKeyword"
    CssClass="form-control" autocomplete="off"
    ClientIDMode="Static"
    onkeydown="if(event.keyCode===13||event.key==='Enter'){event.preventDefault();document.getElementById('btnSearch').click();}" />
                                            </div>
                                                   <!-- The ASP.NET Button (Hidden or Visible) -->
<asp:Button runat="server" ID="btnSearch"
     ClientIDMode="Static"
     CssClass="btn btn-primary"
     OnClick="btnSearch_Click"
     style="display:none;" /> <!-- Optional: hide it if you only want the custom button visible -->

<!-- The HTML Button -->
<button type="button" 
        class="btn btn-primary" 
        aria-label="بحث" 
        onclick="document.getElementById('btnSearch').click();">
    <%= IsArabic ? "بحث" : "Search" %>
</button>
                                        </div>
                                    </dga-search-input>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%-- DESKTOP DYNAMIC MENU (rendered from SharePoint lists in code-behind) --%>
                <div id="expanded-navbar-nav" class="collapse navbar-collapse h-100">
                    <ul class="navbar-nav p-0 ps-3 h-100">
                        <asp:Literal ID="litMenu" runat="server" />
                    </ul>
                </div>

                <%-- DESKTOP: Search --%>
                <div class="d-none d-lg-block dropdown header-dropdown h-100 position-static">
                    <a id="SearchDropdownBtn" href="#" role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside"
                        aria-expanded="false" aria-controls="searchDropdownMenu"
                        class="nav-link position-relative p-0 px-3 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium">
                        <i class="hgi-stroke hgi-search-01" aria-hidden="true"></i>
                        <span class="ps-2">بحث</span>
                    </a>
                    <div id="searchDropdownMenu" aria-labelledby="SearchDropdownBtn" class="dropdown-menu start-0 end-0">
                        <div class="container">
                            <div class="d-flex justify-content-end">
                                <button id="searchDropdownClose" type="button" class="btn btn-secondary icon-btn"
                                    aria-label="إغلاق البحث">
                                    <span class="d-inline-flex fs-5">
                                        <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
                                    </span>
                                </button>
                            </div>
                            <div class="py-4">
                                <dga-search-input>
                                    <div class="d-flex gap-3" role="search" aria-label="بحث في الموقع">
                                        <div class="form-control-container has-icon">
                                            <label for="globalSearchInput" class="visually-hidden">بحث في الموقع</label>
                                            <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                                <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                            </span>
                                                                                        <asp:TextBox runat="server" ID="txtKeywordDesktop"
CssClass="form-control" autocomplete="off"
ClientIDMode="Static"
onkeydown="if(event.keyCode===13||event.key==='Enter'){event.preventDefault();document.getElementById('btnSearchDesktop').click();}" />
                                        </div>
                                        <asp:Button runat="server" ID="btnSearchDesktop"
     ClientIDMode="Static"
     CssClass="btn btn-primary"
     OnClick="btnSearchDesktop_Click"
     style="display:none;" /> <!-- Optional: hide it if you only want the custom button visible -->

<!-- The HTML Button -->
<button type="button" 
        class="btn btn-primary" 
        aria-label="بحث" 
        onclick="document.getElementById('btnSearchDesktop').click();">
    <%= IsArabic ? "بحث" : "Search" %>
</button>
                                    </div>
                                </dga-search-input>
                            </div>
                        </div>
                    </div>
                </div>

                <%-- DESKTOP: Languages --%>
                <div class="d-none d-lg-block dropdown header-dropdown h-100">
                    <a id="LanguagesDropdownBtn" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false"
                        aria-controls="languagesDropdownMenu"
                        class="nav-link position-relative p-0 px-3 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium">
                        <i class="hgi-stroke hgi-translation" aria-hidden="true"></i>
                        <span class="ps-2">العربية</span>
                    </a>
                    <ul id="languagesDropdownMenu" aria-labelledby="LanguagesDropdownBtn" class="dropdown-menu p-3 gap-1 ">
                        <li>
                            <a id="ArabicLink" runat="server" href="#" role="button" lang="ar" aria-label="التبديل إلى اللغة العربية"
                                class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1 my-2">
                                <img height="20" width="32" src="/Style%20Library/DGA/public/images/flags/sa.svg" alt="العربية" decoding="async"> العربية </a>
                        </li>
                        <li>
                            <a id="EnglishLink" runat="server" href="#" role="button" lang="en" aria-label="Switch to English"
                                class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1 my-2">
                                <img height="20" width="32" src="/Style%20Library/DGA/public/images/flags/gb.svg" alt="English" decoding="async"> English </a>
                        </li>
                        
                    </ul>
                </div>

            </div>
        </nav>
    </header>

    <aside class="header-offcanvas d-lg-none">
        <div tabindex="-1" id="mobileSideBar" class="offcanvas offcanvas-start" role="dialog" aria-modal="true"
            aria-labelledby="mobileSideBarLabel">
            <div class="offcanvas-header align-items-center px-4 justify-content-between">
                <h2 id="mobileSideBarLabel" class="visually-hidden">القائمة الرئيسية</h2>
                <div>
                    <a href="/ar" class="d-inline-block">
                        <img width="140" height="50" title="جامعة الأميرة نورة" alt="شعار جامعة الأميرة نورة"
                            loading="lazy" fetchpriority="low" src="/Style%20Library/DGA/public/images/pnu-logo-ar-h.png"
                            srcset="/Style Library/DGA/public/images/pnu-logo-ar-h-2x.png 2x" decoding="async">
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
                    <div class="fw-semibold mb-2">اللغة</div>
                    <ul class="list-unstyled d-flex flex-column gap-2 mb-0">
                        <li>
                            <a href="#" role="button" lang="ar" aria-label="التبديل إلى اللغة العربية"
                                class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1">
                                <img height="20" width="32" src="/Style%20Library/DGA/public/images/flags/sa.svg" alt="العربية" decoding="async"> العربية </a>
                        </li>
                        <li>
                            <a href="#" role="button" lang="en" aria-label="Switch to English"
                                class="dropdown-item d-flex align-items-center gap-2 fw-medium rounded-1">
                                <img height="20" width="32" src="/Style%20Library/DGA/public/images/flags/gb.svg" alt="English" decoding="async"> English </a>
                        </li>
                        
                    </ul>
                </nav>
                <nav aria-label="القائمة الجانبية">
                    <%-- MOBILE DYNAMIC MENU (rendered from SharePoint lists in code-behind) --%>
                    <ul class="navbar-nav flex-column" id="nav_accordion">
                        <asp:Literal ID="litMenuMobile" runat="server" />
                    </ul>
                </nav>
            </div>
        </div>
    </aside>
</dga-header>


<%--<script>
    document.querySelectorAll('#languagesDropdownMenu a[lang]').forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();

            const targetLang = this.getAttribute('lang'); // 'ar' or 'en'
            const currentUrl = window.location.href;
            let newUrl;

            if (targetLang === 'ar') {
                // Replace /en/ with /ar/
                newUrl = currentUrl.replace(/\/en(\/|$)/, '/ar$1');
            } else if (targetLang === 'en') {
                // Replace /ar/ with /en/
                newUrl = currentUrl.replace(/\/ar(\/|$)/, '/en$1');
            }

            // Only redirect if the URL actually changed to prevent loops
            if (newUrl && newUrl !== currentUrl) {
                window.location.href = newUrl;
            }
        });
    });
</script>--%>