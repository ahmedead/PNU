<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHeader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucHeader" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>



<style>
    .ms-rtestate-field img {max-width: 100% !important;height: auto !important;}
a.nav-link.px-2.text-white {
    font-size: 18px;
}


/* Custom CSS for nested dropdowns */
.dropdown-submenu {
  position: relative;
}

.dropdown-submenu > .dropdown-menu {
  display: none;
  position: absolute;
  top: 0;
  left: 100%;
  margin-top: -1px;
}

.dropdown-submenu:hover > .dropdown-menu {
  display: block;
}

@media (max-width: 767px) {
  .dropdown-submenu > .dropdown-menu {
    position: relative;
    left: 0;
    top: 0;
    margin-top: 0;
  }
}

</style>

<header>
    <div class=" py-1 bg-turquoise-800 text-white">
        <div class="container">
            <div class="d-flex flex-wrap align-items-center justify-content-between ">

                <ul class="nav col-12 col-lg-auto mb-2 justify-content-center mb-md-0 d-none d-sm-flex">
                    
                    <li><a href="<%=  GetDynamicURL("AcademicCalendar/Pages/default.aspx") %>" class="nav-link px-2 text-white"><asp:Literal runat="server" Text="<%$ Resources: PNUres, UniversityCalender %>" /></a></li>
                    <%--<li><a href="<%= GetDynamicURL("Pages/AllServices.aspx") %>" class="nav-link px-2 text-white"><asp:Literal runat="server" Text="<%$ Resources: PNUres, EServices %>" /></a></li>--%>
					<li><a href="https://graduates.pnu.edu.sa/ar/Pages/default.aspx" class="nav-link px-2 text-white"  target="_blank"><asp:Literal runat="server" Text="<%$ Resources: CommonGlobalResources, GradutesLink %>" /></a></li>
                </ul>
                <div class=" col-12 col-lg-auto d-flex justify-content-sm-center justify-content-between" id="leftSideHeader">
                    <ul class="nav justify-content-center align-items-center  text-small  px-sm-3 px-0 lh-base d-sm-flex">

                        <li><a href="<%= GetDynamicURL("Faculties/Pages/FacultyMemberSearch.aspx") %>" class="nav-link px-2 text-white">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyMemberSearch %>" />
                            </a></li>

						<li class="header__search" >
                                 <a href="#" class="search__toggler" title="بحث">
                                    <span class="input-group-btn h-100 d-flex align-items-center">
										<svg class="bi d-block m-1 " width="20" height="20">
											<use xlink:href="#search" />
										</svg>
									</span>
                                 </a>
                                 <div class="search__container">
                                    
                                    
                                          <input type="search" class="search__input" placeholder="بحث">
                                       
                                    <button type="button" class="search__button" onclick="searchSite(false)">
                                       <svg width="20" height="20" viewBox="0 0 20 20">
                                          <path d="M14.7,12.9c2.7-3.6,1.9-8.7-1.8-11.4S4.2-0.3,1.6,3.3S-0.3,12,3.3,14.7c2.9,2.1,6.7,2.1,9.6,0l5.3,5.3l1.8-1.8L14.7,12.9z M8.2,13.8c-3.1,0-5.6-2.5-5.6-5.6s2.5-5.6,5.6-5.6c3.1,0,5.6,2.5,5.6,5.6c0,0,0,0,0,0C13.8,11.3,11.3,13.8,8.2,13.8L8.2,13.8z"></path>
                                       </svg>
                                    </button>
                                 </div>
                              </li>

						<li class="rounded-circle bg-white  bg-opacity-10  justify-content-center flex-column mx-1">
                            <a onclick="document.getElementById('muneer-trigger-button').click()" class="nav-link text-white p-2 ">
                                <svg class="bi d-block m-1" width="20" height="20">
                                    <use xlink:href="#accessibility" />
                                </svg>
                            </a>
                        </li>
                        <li class="rounded-circle bg-white  bg-opacity-10  justify-content-center flex-column mx-1">
                            <a href="#" class="nav-link text-white p-2 language__switcher">
                                <svg class="bi d-block m-1" width="20" height="20">
                                    <use xlink:href="#global" />
                                </svg>

                            </a>
                        </li>
                        <%--<li class="rounded-circle bg-white  bg-opacity-10  justify-content-center flex-column mx-1 dark-btn">
                            <a class="nav-link text-white p-2 position-relative theme-btn" href="#" data-bs-theme="dark">
                                <svg class="bi d-block m-1" width="19" height="19">
                                    <use xlink:href="#dark" />
                                </svg>
                                <span class="badge bg-danger position-absolute top-0 left-0">Beta</span>
                            </a>
                        </li>
                        <li class="rounded-circle bg-white  bg-opacity-10  justify-content-center flex-column mx-1 light-btn">
                            <a class="nav-link text-white p-2 position-relative theme-btn" href="#" data-bs-theme="light">
                                <svg class="bi d-block m-1" width="20" height="20">
                                    <use xlink:href="#light" />
                                </svg>
                                <span class="badge bg-danger position-absolute top-0 left-0">Beta</span>
                            </a>
                        </li>--%>
                    </ul>
                    <div class="text-white divider d-none d-sm-flex">|</div>

                    <div id="divSignIn" runat="server">
        <a id="authLink" href="/_layouts/15/Authenticate.aspx" style="min-width: 9.6rem;"
            class="nav-link link-white text-white p-sm-3 p-0 d-inline-flex align-items-center justify-content-end">
            <svg class="bi d-block" width="24" height="24">
                <use xlink:href="#login" />
            </svg>
            <asp:Literal runat="server" Text="<%$ Resources: PNUres, Login %>" />
        </a>
    </div>

    
	

                    
                    <div id="divSignOut" runat="server">
                        <a href="/ar/_layouts/15/SignOut.aspx" target="_blank" style="min-width: 9.6rem;"
                            class="nav-link link-white text-white  p-sm-3 p-0  d-inline-flex align-items-center justify-content-end">
                            <svg class="bi d-block " width="24" height="24">
                                <use xlink:href="#login" />
                            </svg>
                            <%--<p class="mx-3  mb-0">تسجيل خروج </p>--%>


                            <asp:LinkButton  ID="lnkbtn_signout" visible="false" CssClass="mx-3  mb-0" CausesValidation="false" runat="server" Text=" <%$ Resources: PNUres, Logout %>" OnClick="lnkbtn_signout_Click1"></asp:LinkButton>

                        </a>
                    </div>
                </div>




            </div>
        </div>
    </div>
    <div class="container">
        <div class="d-flex flex-wrap align-items-center  justify-content-between py-3 ">
            <a id="aLinkHome" runat="server" class="d-flex align-items-center col-md-auto mb-2 mb-md-0 text-dark text-decoration-none">
                
                <img class="mx-md-0 mx-3" height="64" id="img2" runat="server" alt="pnu-logo" />
            </a>


            <%--<ul class="nav navbar col-12 d-none d-lg-flex col-xl-7 col-lg-8 col-md-7 mb-2 d-flex justify-content-between mb-md-0 menu__list">

            </ul>--%>

            <ul id="menuList" class="nav navbar col-12 d-none d-lg-flex col-xl-7 col-lg-8 col-md-7 mb-2 d-flex justify-content-between mb-md-0" runat="server">

                


            </ul>


            <div class="col-2  col-lg-1 text-end nav navbar d-flex justify-content-end">
                <!-- 
          <button type="button" class="btn btn-link text-dark me-md-0 me-4  d-md-none ">
            <svg class="bi d-block me-1" width="20" height="20">
              <use xlink:href="#menu" />
            </svg>
          </button> -->
                <!-- <li class="nav-item dropdown hide-dropdown-arrow d-none d-lg-block">
                    <a href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false"
                        class="nav-link dropdown-toggle px-sm-3 px-0  link-dark">
                        <svg class="bi d-block me-3" width="20" height="20">
                            <use xlink:href="#menu" />
                        </svg>

                    </a>
                    <ul class="dropdown-menu py-0">
                        <%--<li><a class="dropdown-item" href="community-service.html">خدمة المجتمع</a></li>--%>
                        <li><a class="dropdown-item" href="/ar/FAQs/Pages/FAQ.aspx"><asp:Literal runat="server" Text="<%$ Resources: PNUres, FAQ %>" /></a></li>
                    </ul>
                </li>-->
                <li class="nav-item dropdown hide-dropdown-arrow d-flex d-lg-none">
                    <a href="#" role="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar"
                        aria-controls="offcanvasNavbar" class="nav-link dropdown-toggle px-sm-3 px-0  link-dark">
                        <svg class="bi d-block me-3" width="20" height="20">
                            <use xlink:href="#menu" />
                        </svg>
                    </a>
                </li>
                <!-- <button type="button" class="btn btn-outline-white me-2 px-3 py-2">تسجيل دخول</button> -->
            </div>
        </div>
    </div>
    <div class="offcanvas offcanvas-end sidebar" tabindex="-1" id="offcanvasNavbar"
        aria-labelledby="offcanvasNavbarLabel">
        <div class="offcanvas-header align-items-center px-4">
            <a href="/ar/" class="d-flex align-items-center col-md-auto text-dark text-decoration-none">
                <img class="mx-md-0 mx-3" height="64" id="imgLogo" runat="server" alt="pnu-logo" />
            </a>
            <button type="button" class="btn p-0" data-bs-dismiss="offcanvas" aria-label="Close">
                <svg class="bi d-block m-1 " width="15" height="15">
                    <use xlink:href="#close" />
                </svg>
            </button>
        </div>
       <%--<div class="offcanvas-body px-4 menu__list_mobile">

        </div>--%>


        <div id="divMenuMobile" runat="server" class="offcanvas-body px-4">

        </div>
		
		
		
		
    </div>
</header>

<script src='/Style%20Library/Portal/js/dialog.min.js'></script>
<script>
    document.addEventListener('DOMContentLoaded', function () {
        var authLink = document.getElementById('authLink');
        var currentUrl = window.location.href;
        if (authLink != null) {
            authLink.href += '?Source=' + encodeURIComponent(currentUrl);

        }

    });


</script>


<script>
    window.onload = function () {
        // Select all image elements on the page
        const images = document.querySelectorAll('img');

        // Loop through each image
        images.forEach(image => {
            if (!image.hasAttribute('alt')) {
                const altText = image.src.split('/').pop().split('.')[0]; // Use the image filename as alt text
                image.setAttribute('alt', altText);
            }
        });

    };

</script>


<script type="text/javascript">

    document.addEventListener("DOMContentLoaded", function () {
        // Prevent closing from click inside dropdown
        document.querySelectorAll('.dropdown-menu').forEach(function (element) {
            element.addEventListener('click', function (e) {
                e.stopPropagation();
            });
        });

        // Make it as accordion for smaller screens
        if (window.innerWidth < 992) {
            // Close all inner dropdowns when parent is closed
            document.querySelectorAll('.navbar .dropdown').forEach(function (everydropdown) {
                everydropdown.addEventListener('hidden.bs.dropdown', function () {
                    // After dropdown is hidden, then find all submenus
                    this.querySelectorAll('.submenu').forEach(function (everysubmenu) {
                        // Hide every submenu as well
                        everysubmenu.style.display = 'none';
                    });
                });
            });

            document.querySelectorAll('.dropdown-menu a').forEach(function (element) {
                element.addEventListener('click', function (e) {
                    let nextEl = this.nextElementSibling;
                    if (nextEl && nextEl.classList.contains('submenu')) {
                        // Prevent opening link if link needs to open dropdown
                        e.preventDefault();
                        if (nextEl.style.display == 'block') {
                            nextEl.style.display = 'none';
                        } else {
                            // Close other open submenus
                            document.querySelectorAll('.submenu').forEach(function (submenu) {
                                submenu.style.display = 'none';
                            });
                            nextEl.style.display = 'block';
                        }
                    }
                });
            });
        }

        // Close submenus on outside click
        document.addEventListener('click', function (e) {
            // Check if the click is outside any dropdown menu or submenu
            if (!e.target.closest('.dropdown-menu')) {
                // Close all submenus
                document.querySelectorAll('.submenu').forEach(function (submenu) {
                    submenu.style.display = 'none';
                });
            }
        });
    });



</script>