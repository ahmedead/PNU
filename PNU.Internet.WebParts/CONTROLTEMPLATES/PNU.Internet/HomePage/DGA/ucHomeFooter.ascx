<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeFooter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeFooter" %>

<%@ Register Src="~/_controltemplates/15/PNU.Internet/Shared/ucDgaSurveyFeedback.ascx" TagPrefix="PNU" TagName="DgaSurveyFeedback" %>


<PNU:DgaSurveyFeedback ID="ucSurvey" runat="server" />

<!-- English Content (LCID 1033) -->
<SharePoint:LanguageSpecificContent runat="server" Languages="1033">
    <ContentTemplate>
        <div class="py-3">
            <div class="container">
                <span class="small">Last page update: 
                    <time class="js-current-date-en" datetime=""></time> KSA Time
                </span>
            </div>
        </div>
    </ContentTemplate>
</SharePoint:LanguageSpecificContent>

<!-- Arabic Content (LCID 1025) -->
<SharePoint:LanguageSpecificContent runat="server" Languages="1025">
    <ContentTemplate>
        <div class="py-3">
            <div class="container">
                <span class="small">تاريخ آخر تعديل: 
                    <time class="js-current-date-ar" datetime=""></time> بتوقيت السعودية
                </span>
            </div>
        </div>
    </ContentTemplate>
</SharePoint:LanguageSpecificContent>

<script>
    (function () {
        const now = new Date();
        const options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false,
            timeZone: 'Asia/Riyadh'
        };

        // ISO string for the datetime attribute
        const isoString = now.toISOString();

        // Update English Time element
        const enTime = document.querySelector('.js-current-date-en');
        if (enTime) {
            const enDate = new Intl.DateTimeFormat('en-GB', options).format(now).replace(',', '');
            enTime.innerText = enDate;
            enTime.setAttribute('datetime', isoString);
        }

        // Update Arabic Time element
        const arTime = document.querySelector('.js-current-date-ar');
        if (arTime) {
            const arDate = new Intl.DateTimeFormat('ar-SA', options).format(now);
            arTime.innerText = arDate;
            arTime.setAttribute('datetime', isoString);
        }
    })();
</script>




<dga-footer>
<nav class="position-fixed bottom-0 start-0 z-3 m-4 d-flex flex-column align-items-start gap-2"
    aria-label="روابط سريعة">
	<%-- Muneer accessibility widget --%>
    <script src="https://muneer.cx/v1/js/muneer.min.js?cid=57d4d324-783d-448f-bd8f-3bd71237a092"></script>
    <div class="muneer-trigger-button-box bottom-right entrance-fade hover-none" data-nosnippet="">
        <button id="muneer-trigger-button" class="icon-position-before" aria-label="Open Accessibility Panel" data-muneer-trigger="">
            <span class="muneer-trigger-button-icon">
                <svg height="32" viewBox="0 0 512 512" width="32" xmlns="http://www.w3.org/2000/svg">
                    <path d="m256 112c30.9 0 56-25.1 56-56s-25.1-56-56-56-56 25.1-56 56 25.1 56 56 56z"></path>
                    <path d="m432 112.8-.5.1-.4.1c-1 .3-2 .6-3 .9-18.6 5.5-108.9 30.9-172.6 30.9-59.1 0-141.3-22-167.6-29.5-2.6-1-5.3-1.9-8-2.6-19-5-32 14.3-32 31.9 0 17.5 15.7 25.8 31.5 31.8v.3l95.2 29.7c9.7 3.7 12.3 7.5 13.6 10.8 4.1 10.6.8 31.6-.3 38.9l-5.8 45-32.1 176.3c-.1.5-.2 1-.3 1.5l-.2 1.3c-2.3 16.1 9.5 31.8 32 31.8 19.6 0 28.3-13.5 32-31.9 0 0 28-157.6 42-157.6s42.8 157.6 42.8 157.6c3.8 18.4 12.4 31.9 32 31.9 22.5 0 34.4-15.7 32-31.9-.2-1.4-.5-2.7-.8-4.1l-32.5-174.7-5.8-45c-4.2-26.2-.8-34.9.3-36.9 0 0 .1-.1.1-.2 1.1-2 6-6.5 17.5-10.8l89.3-31.2c.5-.1 1.1-.3 1.6-.5 16-6 32-14.3 32-31.9s-13-37-32-32z"></path>
                </svg>
            </span>
        </button>
    </div>

    <style>
        div#muneer-popup-footer { display: none; }
        .article-content {
            text-align: justify;
            font-family: var(--bs-body-font-family) !important;
        }
    </style>

    <a href="/<%= IsArabic  ? "ar" : "en" %>/AcademicCalendar/Pages/default.aspx" class="btn btn-primary icon-btn p-3 rounded-circle shadow"
      aria-label="التقويم الأكاديمي" title="التقويم الأكاديمي">
      <span class="d-inline-flex fs-4">
        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
      </span>
    </a>
    <a href="/<%= IsArabic  ? "ar" : "en" %>/Pages/ContactUsForm.aspx" class="btn btn-primary rounded-pill  px-3  shadow d-inline-flex align-items-center gap-2">
      <span class="d-inline-flex fs-4 py-1">
        <i class="hgi hgi-stroke hgi-customer-service-02" aria-hidden="true"></i>
      </span>
      <span>تواصل معنا</span>
    </a>
  </nav>
  
  
    <footer class="dga-footer pt-5" role="contentinfo">
      <div class="container">
        <div class="row g-4 pt-3" id="footerNavContainer">


          <div class="col-12 col-lg-3">
            <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6"
              style="border-bottom:1px solid rgba(255,255,255,.3)">
              <%= IsArabic  ? " نظرة عامة" : "Overview" %></h3>
            <button aria-controls="footerNav1" aria-expanded="false"
              class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
              data-bs-target="#footerNav1" data-bs-toggle="collapse"
              style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
              <span class="mb-0 h6"><%= IsArabic  ? " نظرة عامة" : "Overview" %>  </span>
              <span class="d-inline-flex fs-5">
                <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition"
                  aria-hidden="true"></i>
              </span>
            </button>
            <div class="collapse d-lg-block" id="footerNav1" data-bs-parent="#footerNavContainer">
              <div class="d-flex flex-column gap-2 mt-2">
                <a href="/<%= IsArabic  ? "ar" : "en" %>/FAQs/Pages/default.aspx" target="_self" class="text-white small"><%= IsArabic  ? "الأسئلة الشائعة" : "FAQs" %></a>
                <a href="https://techcare.pnu.edu.sa/" target="_blank" rel="noopener noreferrer"
                  class="text-white small external-link">
                  <%= IsArabic  ? " تيك كير لتقنية المعلومات" : "Tech Care for for Information Technology " %> 
                  </a>
                <a href="https://www.kaauh.edu.sa/" target="_blank" rel="noopener noreferrer"
                  class="text-white small external-link">
                  <%= IsArabic  ? " المستشفى الجامعي" : "kaauh Hosptial" %> 
                  </a>
                <%--<a href="https://nourahprize.pnu.edu.sa/" target="_blank" rel="nofollow noopener noreferrer"
                  class="text-white small external-link">
                  <%= IsArabic  ? " جائزة الأميرة نورة" : "Nourah Prize" %> 
                  </a>--%>
                <a href="https://tawasulnourah.pnu.edu.sa/" target="_blank" rel="noopener noreferrer"
                  class="text-white small external-link">
                    <%= IsArabic  ? " تواصل نورة" : "Tawasul with Nourah" %>                  
                </a>
                <a href="https://eservice.pnu.edu.sa/" target="_blank" rel="noopener noreferrer"
                  class="text-white small external-link">
                  <%= IsArabic  ? " بوابة الخدمات الإلكترونية" : "e-services portal" %>
                  
                </a>
                <%--<a href="https://graduates.pnu.edu.sa/ar/Pages/default.aspx" target="_blank"
                  rel="noopener noreferrer" class="text-white small external-link">
                    <%= IsArabic  ? " منصة الخريجات" : "Graduates platform" %>
                </a>--%>

                  <a href="/<%= IsArabic  ? "ar" : "en" %>/AboutUniversity/Pages/Support.aspx" target="_blank" rel="noopener noreferrer"
  class="text-white small ">
  <%= IsArabic  ? "المساعدة والدعم" : "Help and Support" %>
  
</a>

                                    <a href="/<%= IsArabic  ? "ar" : "en" %>/Pages/ContactUsForm.aspx" target="_blank" rel="noopener noreferrer"
  class="text-white small">
  <%= IsArabic  ? "تواصل معنا" : "Contact Us" %>
  
</a>
              </div>
            </div>
          </div>
          <div class="col-12 col-lg-3">
            <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6"
              style="border-bottom:1px solid rgba(255,255,255,.3)">
                <%= IsArabic  ? " فرصك في نورة" : "Your opportunities in Noura" %>

              </h3>
            <button aria-controls="footerNav2" aria-expanded="false"
              class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
              data-bs-target="#footerNav2" data-bs-toggle="collapse"
              style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
              <span class="mb-0 h6"><%= IsArabic  ? " فرصك في نورة" : "Your opportunities in Noura" %></span>
              <span class="d-inline-flex fs-5">
                <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition"
                  aria-hidden="true"></i>
              </span>
            </button>
            <div class="collapse d-lg-block" id="footerNav2" data-bs-parent="#footerNavContainer">
              <div class="d-flex flex-column gap-2 mt-2">
                  <a href="/<%= IsArabic  ? "ar" : "en" %>/AboutUniversity/Pages/Careers.aspx" target="_self"
  class="text-white small"> <%= IsArabic  ? " التوظيف" : "Recruitment" %> </a>

                <a href="/<%= IsArabic  ? "ar" : "en" %>/NourahOpportunities/Pages/Invest-in-noura.aspx" target="_self"
                  class="text-white small"> <%= IsArabic  ? " استثمر في نورة" : "Invest in Noura" %> </a>
                <a href="/<%= IsArabic  ? "ar" : "en" %>/NourahOpportunities/Pages/التدريب.aspx" target="_self"
                  class="text-white small"><%= IsArabic  ? "التدريب" : "Training" %></a>
                <a href="https://norahattaa.pnu.edu.sa/ar/Pages/default.aspx" target="_blank"
                  rel="nofollow noopener noreferrer" class="text-white small external-link">
                  <%= IsArabic  ? " التطوع" : "Volunteering" %>
                </a>
              </div>
            </div>
            <div class="mt-4">
              <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6"
                style="border-bottom:1px solid rgba(255,255,255,.3)">
                <%= IsArabic  ?  " الحياة الجامعية" : "University life" %>
              </h3>
              <button aria-controls="footerNav5" aria-expanded="false"
                class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
                data-bs-target="#footerNav5" data-bs-toggle="collapse"
                style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
                <span class="mb-0 h6">
                  <%= IsArabic  ? " الحياة الجامعية" : "University life" %>
                  
                </span>
                <span class="d-inline-flex fs-5">
                  <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition"
                    aria-hidden="true"></i>
                </span>
              </button>
              <div class="collapse d-lg-block" id="footerNav5" data-bs-parent="#footerNavContainer">
                <div class="d-flex flex-column gap-2 mt-2">
                  <a href="/<%= IsArabic  ? "ar" : "en" %>/UniversityLife/Pages/VirtualTour.aspx" target="_self"
                    class="text-white small">
                    <%= IsArabic  ? "جولة افتراضية 360" : "360 virtual tour" %>
                    
                  </a>
                  <a href="/<%= IsArabic  ? "ar" : "en" %>/UniversityLife/Pages/UniversityFacilities.aspx" target="_self"
                    class="text-white small">
                    <%= IsArabic  ? " مرافق الجامعة" : "University Facilities" %>
                  </a>
                </div>
              </div>
            </div>
          </div>

          <div class="col-12 col-lg-3">
            <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6"
              style="border-bottom:1px solid rgba(255,255,255,.3)">
              <%= RelatedLinksTitle %></h3>
            <button aria-controls="footerNav4" aria-expanded="false"
              class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
              data-bs-target="#footerNav4" data-bs-toggle="collapse"
              style="border-bottom:1px solid rgba(255,255,255,.3)" type="button">
              <span class="mb-0 h6"><%= RelatedLinksTitle %></span>
              <span class="d-inline-flex fs-5">
                <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition"
                  aria-hidden="true"></i>
              </span>
            </button>
            <div class="collapse d-lg-block" id="footerNav4" data-bs-parent="#footerNavContainer">
              <div class="d-flex flex-column gap-2 mt-2">

                  <asp:Repeater ID="rptUsefulLinks" runat="server">
                    <ItemTemplate>
                      <a href='<%# Eval("Url") %>'
                         target='<%# (bool)Eval("OpenInNewTab") ? "_blank" : "_self" %>'
                         rel="noopener noreferrer"
                         class="text-white small external-link"><%# Server.HtmlEncode(IsArabic ? (string)Eval("TitleAr") : (string)Eval("TitleEn")) %></a>
                    </ItemTemplate>
                  </asp:Repeater>

              </div>
            </div>
          </div>

          <div class="col-12 col-lg-3">
            <div class="d-flex gap-4 flex-row flex-column mt-5 mt-lg-0">

              <%-- Contact info (ContactUsFooter list: phone + email) --%>
              <div class="flex-grow-1">
                <h3 class="text-white mb-0 pb-2 h6" style="border-bottom:1px solid rgba(255,255,255,.3)">
                  <%= ContactTitle %>
                </h3>

                <asp:PlaceHolder ID="phPhone" runat="server" Visible="false">
                  <div class="mt-2 text-ltr">
                    <a id="lnkPhone" runat="server" class="text-white fw-bold">
                      <asp:Literal ID="litPhone" runat="server" />
                    </a>
                  </div>
                </asp:PlaceHolder>
				<div class="mt-2 text-ltr">
                    <a href="tel:+966-11-8220000" id="ctl00_ucHomeFooter_lnkPhone" class="text-white fw-bold">
                      00966-11-8220011
                    </a>
                  </div>

                <asp:PlaceHolder ID="phEmail" runat="server" Visible="false">
                  <div class="mt-2 text-ltr">
                    <a id="lnkEmail" runat="server" class="text-white fw-bold">
                      <asp:Literal ID="litEmail" runat="server" />
                    </a>
                  </div>
                </asp:PlaceHolder>
              </div>

              <%-- Social media icons (FollowUsUsFooter list): top-level rows render either as
                  a single icon-button OR as a dropdown toggle when they have children. --%>
              <div>
                <h3 class="text-white mb-0 pb-2 border-top-0 border-start-0 border-end-0 h6"
                    style="border-bottom:1px solid rgba(255,255,255,.3)">
                  <%= FollowTitle %>
                </h3>
                <div class="d-flex gap-2 mt-2 flex-wrap">
                  <asp:Repeater ID="rptSocial" runat="server">
                    <ItemTemplate>

                      <%-- CASE 1: item has NO children -> plain icon button with URL --%>
                      <asp:PlaceHolder runat="server" Visible='<%# !(bool)Eval("HasChildren") %>'>
                        <a href='<%# Eval("Url") %>' target="_blank" rel="noopener noreferrer"
                          class="btn btn-outline-on-color-secondary icon-btn"
                          aria-label='<%# Server.HtmlEncode(IsArabic ? (string)Eval("AriaLabelAr") : (string)Eval("AriaLabelEn")) %>'>
                          <span class="d-inline-flex fs-6">
                            <i class='<%# Eval("IconClass") %>' aria-hidden="true"></i>
                          </span>
                        </a>
                      </asp:PlaceHolder>

                      <%-- CASE 2: item HAS children -> dropdown (Bootstrap dropup) --%>
                      <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasChildren") %>'>
                        <div class="dropdown dropup">
                          <button type="button" class="btn btn-outline-on-color-secondary icon-btn"
                                  data-bs-toggle="dropdown" aria-expanded="false"
                                  aria-label='<%# Server.HtmlEncode(IsArabic ? (string)Eval("AriaLabelAr") : (string)Eval("AriaLabelEn")) %>'>
                            <span class="d-inline-flex fs-6">
                              <i class='<%# Eval("IconClass") %>' aria-hidden="true"></i>
                            </span>
                          </button>
                          <ul class="dropdown-menu py-0">
                            <asp:Repeater ID="rptSocialChildren" runat="server" DataSource='<%# Eval("Children") %>'>
                              <ItemTemplate>
                                <li>
                                  <a class="dropdown-item" href='<%# Eval("Url") %>' target="_blank" rel="noopener noreferrer">
                                    <%# Server.HtmlEncode(IsArabic ? (string)Eval("AriaLabelAr") : (string)Eval("AriaLabelEn")) %>
                                  </a>
                                </li>
                              </ItemTemplate>
                            </asp:Repeater>
                          </ul>
                        </div>
                      </asp:PlaceHolder>

                    </ItemTemplate>
                  </asp:Repeater>
                </div>
              </div>

            </div>
          </div>


          <div class="col-12">
            <div class="mt-3 py-5">
              <div
                class="mt-5 pt-1 d-flex flex-column flex-lg-row gap-4 align-items-center align-items-lg-center justify-content-lg-between">

                <div class="flex-grow-1">
                  <div class="d-flex gap-3 mb-4">
                    
                  <asp:Repeater ID="rptDownLinks" runat="server">
                    <ItemTemplate>
                      <a href='<%# IsArabic ? (string)Eval("UrlAr") : (string)Eval("UrlEn") %>'
                         target='<%# (bool)Eval("OpenInNewTab") ? "_blank" : "_self" %>'
                         rel="noopener noreferrer"
                         class="text-white small text-center">
                        <%# Server.HtmlEncode(IsArabic ? (string)Eval("TitleAr") : (string)Eval("TitleEn")) %>
                      </a>
                    </ItemTemplate>
                  </asp:Repeater>

                  </div>
                  <p class="text-white small text-center fw-semibold mb-3 mt-4 text-lg-start">
                    <%= CopyrightText %>
                  </p>

                    <p class="text-white small text-center fw-lighter text-lg-start mb-0">
                      <%= IsArabic ? " - تم تطويره وصيانته بواسطة إدارة التحول الرقمي" : " - Developed and maintained by the Digital Transformation Department" %>
                    </p>


                </div>
                <div class="d-flex align-self-stretch gap-3 justify-content-lg-between justify-content-center align-items-end">

                    <a href="https://sdaia.gov.sa/ar/default.aspx" target="_blank" rel="noopener noreferrer"><svg width="125"
                        height="42" viewBox="0 0 125 42" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path
                          d="M122.115 41.9943V24.2544H118.417V41.9943H122.115ZM125 4.49241V1.21215C123.022 1.26959 121.082 1.78662 119.35 2.76323L114.915 5.2737V0H111.318V5.26795L106.884 2.75749C105.151 1.78088 103.218 1.26385 101.233 1.2064V4.48666C102.55 4.54411 103.835 4.89454 104.988 5.5437L110.455 8.63439L104.988 11.7251C103.835 12.3742 102.55 12.7247 101.233 12.7821V16.0624C103.211 16.0049 105.151 15.4879 106.884 14.5113L111.318 12.0008V15.62C111.318 17.3262 110.644 18.998 109.328 20.2101L109.296 20.2388L104.597 24.5244C102.43 26.5006 101.233 29.1317 101.233 31.9294V41.9828H104.837V35.4682H111.318V41.9828H114.915V12.0123L119.35 14.5228C121.076 15.4994 123.016 16.0164 125 16.0739V12.7936C123.684 12.7362 122.398 12.3857 121.246 11.7366L115.778 8.64588L121.246 5.55519C122.398 4.90603 123.684 4.54986 125 4.49815V4.49241ZM111.312 32.1994H104.83V31.9467C104.83 30.0279 105.649 28.224 107.136 26.8625L111.312 23.0538V32.1994ZM117.68 20.3595L120.194 22.6516L122.707 20.3595L120.194 18.0673L117.68 20.3595Z"
                          fill="white" />
                        <path d="M92.1059 11.8516H90.2162V21.9796H92.1059V11.8516Z" fill="white" />
                        <path d="M54.4205 11.8517L53.0843 13.0703L54.4205 14.289L55.7568 13.0703L54.4205 11.8517Z"
                          fill="white" />
                        <path
                          d="M79.1046 33.8351V27.1481H74.8211C73.2149 27.1481 71.6968 27.7169 70.5629 28.7567L68.0937 31.0086C68.0433 31.0546 67.9866 31.1005 67.9299 31.1465V27.1539H66.0402V32.0312C65.7567 32.0886 65.4669 32.1174 65.1709 32.1174H62.267V27.1539H57.4733C55.8671 27.1539 54.349 27.7226 53.2151 28.7624L51.0923 30.6984V23.707H49.2026V31.8991C48.7616 32.0427 48.3018 32.1174 47.8231 32.1174H45.4294V27.1539H43.5396V32.1174H39.7602V23.707H37.8704V33.8351H47.8168C49.423 33.8351 50.9411 33.2663 52.075 32.2265L52.1758 32.1346V33.8351H65.1646C66.7709 33.8351 68.2889 33.2663 69.4228 32.2265L69.5173 32.1404V33.8351H79.0983H79.1046ZM54.5505 29.9746C55.3316 29.2622 56.371 28.8716 57.4733 28.8716H60.3772V32.1116H52.2073L54.5505 29.9746ZM71.8983 29.9746C72.6794 29.2622 73.7188 28.8716 74.8211 28.8716H77.2148V32.1116H69.5551L71.8983 29.9746Z"
                          fill="white" />
                        <path d="M92.1059 23.707H90.2162V33.8351H92.1059V23.707Z" fill="white" />
                        <path d="M44.4846 23.711L43.1484 24.9297L44.4846 26.1483L45.8209 24.9297L44.4846 23.711Z"
                          fill="white" />
                        <path d="M16.0347 31.4024L14.6984 32.6211L16.0347 33.8397L17.3709 32.6211L16.0347 31.4024Z"
                          fill="white" />
                        <path d="M18.7106 31.4024L17.3744 32.6211L18.7106 33.8397L20.0469 32.6211L18.7106 31.4024Z"
                          fill="white" />
                        <path
                          d="M18.9163 29.9749L21.3855 32.2268C22.5257 33.2666 24.0375 33.8354 25.6438 33.8354H35.9869V32.1119H25.9272L28.2705 29.9749C29.0516 29.2625 30.091 28.8719 31.1933 28.8719H35.9869V27.1484H31.1933C29.587 27.1484 28.0689 27.7172 26.9351 28.757L24.4658 31.0089C24.2013 31.2502 23.9115 31.4513 23.5965 31.6179C23.2816 31.4513 22.9855 31.2502 22.721 31.0089L20.2517 28.757C19.1116 27.7172 17.5998 27.1484 15.9935 27.1484H11.7101V32.1119H2.89759L3.35743 31.6925C4.13222 30.9859 5.18418 30.5896 6.28022 30.5896H9.82034V28.8661H6.28022C4.68024 28.8661 3.14955 29.4463 2.02201 30.4747L0.00628662 32.313V33.8354H1.00785L1.07084 33.7779V33.8354H13.5935V28.8719H15.9872C17.0895 28.8719 18.1289 29.2625 18.91 29.9749H18.9163Z"
                          fill="white" />
                        <path
                          d="M88.3139 33.8369C88.2257 32.4984 87.6147 31.246 86.5627 30.2866L85.9958 29.7696L86.5627 29.2526C87.7028 28.2128 88.3265 26.834 88.3265 25.3691V23.7031H86.4367V25.3691C86.4367 26.3744 86.0084 27.3223 85.2273 28.0347L84.6604 28.5517L84.0934 28.0347C83.3123 27.3223 82.884 26.3744 82.884 25.3691V23.7031H80.9943V25.3691C80.9943 26.834 81.6179 28.2185 82.758 29.2526L83.3249 29.7696L82.758 30.2866C81.7061 31.246 81.095 32.4926 81.0069 33.8369H88.3139ZM83.5643 32.1135C83.7218 31.8952 83.8919 31.6941 84.0934 31.5045L84.6604 30.9875L85.2273 31.5045C85.4288 31.6884 85.6052 31.8952 85.7564 32.1135H83.5643Z"
                          fill="white" />
                        <path
                          d="M14.6077 8.51794L14.7148 8.42028V10.1265H24.2832V3.44531H19.9997C18.3935 3.44531 16.8754 4.01404 15.7415 5.05385L13.2723 7.3058C12.4912 8.01815 11.4518 8.40879 10.3495 8.40879H0.00628662V10.1322H10.3495C11.9557 10.1322 13.4738 9.56349 14.6077 8.52369V8.51794ZM17.0769 6.27174C17.858 5.55939 18.8974 5.16874 19.9997 5.16874H22.3934V8.40879H14.7337L17.0769 6.27174Z"
                          fill="white" />
                        <path
                          d="M87.3123 5.16455H92.106V3.44112H87.3123C85.706 3.44112 84.1879 4.00985 83.0541 5.04965L80.5848 7.3016C80.4652 7.41075 80.3455 7.50267 80.2195 7.59458C80.1817 7.62331 80.1439 7.64629 80.0998 7.67501C80.0053 7.7382 79.9045 7.8014 79.8037 7.8531C79.7596 7.87608 79.7219 7.89906 79.6778 7.92204C79.5455 7.99097 79.4132 8.04842 79.2746 8.10587C79.262 8.10587 79.2557 8.11736 79.2431 8.11736C79.0982 8.17481 78.9408 8.22076 78.7896 8.26098C78.4242 8.35289 78.0463 8.4046 77.6557 8.4046H28.0627V0H26.1729V10.128H92.106V8.4046H82.0462L84.3895 6.26754C85.1706 5.55519 86.21 5.16455 87.3123 5.16455Z"
                          fill="white" />
                        <path
                          d="M53.0891 21.9835H88.3328V11.8555H86.443V20.2658H67.533C67.1425 20.2658 66.7645 20.2141 66.3992 20.1222C66.248 20.082 66.0905 20.036 65.9457 19.9786C65.9331 19.9786 65.9268 19.9671 65.9142 19.9671C65.7756 19.9154 65.6433 19.8522 65.511 19.7832C65.4669 19.7603 65.4291 19.7373 65.385 19.7143C65.2842 19.6569 65.1835 19.5994 65.089 19.5362C65.0512 19.5075 65.0134 19.4845 64.9693 19.4558C64.8433 19.3639 64.7173 19.2662 64.6039 19.1628L62.1347 16.9109C60.9945 15.8711 59.4827 15.3023 57.8765 15.3023H53.0828V17.0258H57.8765C58.9788 17.0258 60.0182 17.4164 60.7993 18.1288L63.1425 20.2658H53.0828V21.9892L53.0891 21.9835Z"
                          fill="white" />
                        <path
                          d="M12.1195 20.2604H6.69598L9.03926 18.1233C9.82035 17.411 10.8597 17.0203 11.9621 17.0203H12.1195V15.2969H11.9621C10.3558 15.2969 8.83769 15.8656 7.70384 16.9054L5.23458 19.1574C4.71805 19.6284 3.89287 20.2604 3.16217 20.2604H0V21.9838H12.1132V20.2604H12.1195Z"
                          fill="white" />
                        <path
                          d="M47.8419 19.8425L48.3018 20.2619H15.899V11.8516H14.0093V21.9796H50.1852H51.1868V20.4572L49.1711 18.6189C48.0435 17.5906 46.5065 17.0104 44.9128 17.0104H27.0359L27.7099 16.3957C28.491 15.6833 29.5303 15.2927 30.6327 15.2927H49.1962V13.5692H30.6327C29.0264 13.5692 27.5083 14.138 26.3745 15.1778L24.3651 17.0104V18.7338H44.9128C46.0089 18.7338 47.0608 19.1302 47.8356 19.8368L47.8419 19.8425Z"
                          fill="white" />
                        <path d="M2.34327 39.5524H1.49918V41.948H0.850371V39.5524H0.00628662V39.0469H2.34327V39.5524Z"
                          fill="white" />
                        <path
                          d="M4.68653 41.948V40.7243H3.4267V41.948H2.77789V39.0469H3.4267V40.2188H4.68653V39.0469H5.33534V41.948H4.68653Z"
                          fill="white" />
                        <path
                          d="M7.97469 41.4424V41.948H5.99677V39.0469H7.97469V39.5524H6.64558V40.2246H7.77942V40.7301H6.64558V41.4424H7.97469Z"
                          fill="white" />
                        <path
                          d="M10.6014 41.948H9.95259V40.7875C9.43606 40.1097 9.08331 39.5294 8.90063 39.0469H9.56834C9.73842 39.4605 9.97779 39.8799 10.2801 40.305C10.5762 39.8971 10.8093 39.4777 10.9856 39.0469H11.6534C11.4518 39.5467 11.099 40.1269 10.6014 40.7875V41.948Z"
                          fill="white" />
                        <path
                          d="M13.9841 41.4424V41.948H12.0062V39.0469H13.9841V39.5524H12.655V40.2246H13.7888V40.7301H12.655V41.4424H13.9841Z"
                          fill="white" />
                        <path
                          d="M16.4785 41.948L16.2896 41.3103H15.1179C15.0549 41.5056 14.9919 41.7182 14.9289 41.948H14.2612C14.6266 40.7358 14.9919 39.7707 15.3636 39.0469H16.0376C16.4218 39.788 16.7935 40.7588 17.1462 41.948H16.4785ZM15.6974 39.6788C15.5903 39.8914 15.4455 40.2705 15.2691 40.8048H16.1195C15.9368 40.2475 15.7982 39.8741 15.6974 39.6788Z"
                          fill="white" />
                        <path
                          d="M18.3872 40.8659H18.1919V41.9402H17.5368V39.0391H18.4754C18.8659 39.0391 19.1746 39.1137 19.395 39.2631C19.6155 39.4125 19.7289 39.625 19.7289 39.9008C19.7289 40.3201 19.521 40.5787 19.099 40.6763C19.2376 40.728 19.3636 40.8544 19.4769 41.0612L19.9431 41.9402H19.2439L18.8092 41.1187C18.7588 41.0268 18.7021 40.9636 18.6392 40.9233C18.5762 40.8831 18.4943 40.8659 18.3935 40.8659H18.3872ZM18.4439 39.5503H18.1919V40.3718H18.4439C18.8533 40.3718 19.0612 40.234 19.0612 39.9582C19.0612 39.8089 19.0108 39.7055 18.9037 39.6423C18.8029 39.5791 18.6455 39.5503 18.4376 39.5503H18.4439Z"
                          fill="white" />
                        <path
                          d="M23.7981 39.4058C24.0501 39.6816 24.1824 40.0435 24.1824 40.4916C24.1824 40.9397 24.0564 41.3016 23.7981 41.5773C23.5461 41.8531 23.1934 41.991 22.7462 41.991C22.2989 41.991 21.9462 41.8531 21.6879 41.5773C21.4296 41.3016 21.3036 40.9397 21.3036 40.4916C21.3036 40.0435 21.4296 39.6816 21.6879 39.4058C21.9399 39.1301 22.2926 38.9922 22.7462 38.9922C23.1997 38.9922 23.5461 39.1301 23.7981 39.4058ZM22.1666 39.7792C22.0343 39.9516 21.9714 40.1928 21.9714 40.4973C21.9714 40.8018 22.0343 41.0373 22.1666 41.2154C22.2989 41.3878 22.4879 41.4739 22.7399 41.4739C22.9918 41.4739 23.1745 41.3878 23.3068 41.2154C23.4391 41.0431 23.502 40.8018 23.502 40.4973C23.502 40.1928 23.4391 39.9573 23.3068 39.7792C23.1745 39.6069 22.9855 39.5207 22.7399 39.5207C22.4942 39.5207 22.2989 39.6069 22.1666 39.7792Z"
                          fill="white" />
                        <path
                          d="M25.3477 41.948H24.6989V39.0469H26.6327V39.5524H25.3477V40.3394H26.4312V40.845H25.3477V41.948Z"
                          fill="white" />
                        <path
                          d="M30.1539 41.948L29.965 41.3103H28.7933C28.7303 41.5056 28.6673 41.7182 28.6044 41.948H27.9366C28.302 40.7358 28.6673 39.7707 29.039 39.0469H29.713C30.0972 39.788 30.4689 40.7588 30.8216 41.948H30.1539ZM29.3791 39.6788C29.2721 39.8914 29.1272 40.2705 28.9508 40.8048H29.8012C29.6185 40.2475 29.4799 39.8741 29.3791 39.6788Z"
                          fill="white" />
                        <path
                          d="M32.0689 40.8659H31.8736V41.9402H31.2185V39.0391H32.1571C32.5476 39.0391 32.8563 39.1137 33.0768 39.2631C33.2972 39.4125 33.4106 39.625 33.4106 39.9008C33.4106 40.3201 33.2027 40.5787 32.7807 40.6763C32.9193 40.728 33.0453 40.8544 33.1586 41.0612L33.6248 41.9402H32.9256L32.4909 41.1187C32.4405 41.0268 32.3838 40.9636 32.3209 40.9233C32.2579 40.8831 32.176 40.8659 32.0752 40.8659H32.0689ZM32.1193 39.5503H31.8673V40.3718H32.1193C32.5287 40.3718 32.7366 40.234 32.7366 39.9582C32.7366 39.8089 32.6862 39.7055 32.5791 39.6423C32.4783 39.5791 32.3209 39.5503 32.113 39.5503H32.1193Z"
                          fill="white" />
                        <path d="M35.8925 39.5524H35.0484V41.948H34.3996V39.5524H33.5555V39.0469H35.8925V39.5524Z"
                          fill="white" />
                        <path d="M36.3208 41.948V39.0469H36.9696V41.948H36.3208Z" fill="white" />
                        <path
                          d="M38.2735 41.948H37.6247V39.0469H39.5585V39.5524H38.2735V40.3394H39.357V40.845H38.2735V41.948Z"
                          fill="white" />
                        <path d="M40.0625 41.948V39.0469H40.7113V41.948H40.0625Z" fill="white" />
                        <path
                          d="M42.727 41.9928C42.2546 41.9928 41.8829 41.8607 41.6247 41.5907C41.3664 41.3264 41.2341 40.9645 41.2341 40.5106C41.2341 40.0568 41.3664 39.6834 41.6247 39.4019C41.8892 39.1262 42.2609 38.9883 42.7522 38.9883C43.0105 38.9883 43.2625 39.04 43.5207 39.1376L43.3758 39.6374C43.105 39.5513 42.9034 39.5111 42.7585 39.5111C42.1916 39.5111 41.9018 39.8443 41.9018 40.5049C41.9018 41.1655 42.1853 41.4643 42.7522 41.4643C42.9664 41.4643 43.1869 41.4126 43.4199 41.3034L43.5459 41.8147C43.3065 41.9296 43.0357 41.987 42.7207 41.987L42.727 41.9928Z"
                          fill="white" />
                        <path d="M44.0247 41.948V39.0469H44.6735V41.948H44.0247Z" fill="white" />
                        <path
                          d="M47.2876 41.948L47.0986 41.3103H45.927C45.864 41.5056 45.801 41.7182 45.738 41.948H45.0703C45.4357 40.7358 45.801 39.7707 46.1727 39.0469H46.8467C47.2309 39.788 47.6026 40.7588 47.9553 41.948H47.2876ZM46.5128 39.6788C46.4057 39.8914 46.2608 40.2705 46.0845 40.8048H46.9349C46.7522 40.2475 46.6136 39.8741 46.5128 39.6788Z"
                          fill="white" />
                        <path d="M48.3522 41.948V39.0469H49.001V41.4424H50.286V41.948H48.3522Z" fill="white" />
                        <path d="M51.8293 41.948V39.0469H52.4781V41.948H51.8293Z" fill="white" />
                        <path
                          d="M53.7316 41.948H53.1332V39.0469H53.7631C54.1536 39.7305 54.6135 40.4199 55.1489 41.1207C55.1363 41.0231 55.13 40.9024 55.13 40.7588V39.0469H55.7284V41.948H55.0985C54.7962 41.5803 54.5442 41.2529 54.3363 40.9771C54.1284 40.6956 53.9269 40.3854 53.719 40.0465C53.7316 40.1901 53.7379 40.3567 53.7379 40.5405V41.9537L53.7316 41.948Z"
                          fill="white" />
                        <path d="M58.4874 39.5524H57.6433V41.948H56.9945V39.5524H56.1505V39.0469H58.4874V39.5524Z"
                          fill="white" />
                        <path
                          d="M60.9 41.4424V41.948H58.9221V39.0469H60.9V39.5524H59.5709V40.2246H60.7048V40.7301H59.5709V41.4424H60.9Z"
                          fill="white" />
                        <path d="M61.4354 41.948V39.0469H62.0842V41.4424H63.3693V41.948H61.4354Z" fill="white" />
                        <path d="M63.7409 41.948V39.0469H64.3897V41.4424H65.6747V41.948H63.7409Z" fill="white" />
                        <path d="M66.1598 41.948V39.0469H66.8086V41.948H66.1598Z" fill="white" />
                        <path
                          d="M69.9015 41.7669C69.5802 41.9163 69.2401 41.9967 68.8999 41.9967C68.4086 41.9967 68.0243 41.8646 67.7472 41.5946C67.47 41.3303 67.3314 40.9741 67.3314 40.526C67.3314 40.078 67.4763 39.6988 67.7598 39.4173C68.0432 39.1358 68.4338 38.9922 68.9251 38.9922C69.2464 38.9922 69.5361 39.0439 69.7881 39.1416L69.6432 39.6413C69.4227 39.5609 69.1834 39.515 68.9377 39.515C68.629 39.515 68.4023 39.6069 68.2448 39.785C68.0873 39.9631 68.0054 40.2101 68.0054 40.526C68.0054 40.8248 68.0873 41.0546 68.2448 41.2212C68.4023 41.3878 68.629 41.4682 68.9314 41.4682C69.0574 41.4682 69.1897 41.4567 69.3156 41.428V40.7846H68.7928V40.2963H69.9078V41.7612L69.9015 41.7669Z"
                          fill="white" />
                        <path
                          d="M72.4527 41.4424V41.948H70.4747V39.0469H72.4527V39.5524H71.1235V40.2246H72.2574V40.7301H71.1235V41.4424H72.4527Z"
                          fill="white" />
                        <path
                          d="M73.5865 41.948H72.988V39.0469H73.618C74.0085 39.7305 74.4683 40.4199 75.0038 41.1207C74.9912 41.0231 74.9849 40.9024 74.9849 40.7588V39.0469H75.5833V41.948H74.9534C74.651 41.5803 74.399 41.2529 74.1912 40.9771C73.9833 40.6956 73.7817 40.3854 73.5739 40.0465C73.5865 40.1901 73.5928 40.3567 73.5928 40.5405V41.9537L73.5865 41.948Z"
                          fill="white" />
                        <path
                          d="M77.5927 41.9928C77.1203 41.9928 76.7487 41.8607 76.4904 41.5907C76.2321 41.3264 76.0999 40.9645 76.0999 40.5106C76.0999 40.0568 76.2321 39.6834 76.4904 39.4019C76.755 39.1262 77.1266 38.9883 77.6179 38.9883C77.8762 38.9883 78.1282 39.04 78.3864 39.1376L78.2416 39.6374C77.9707 39.5513 77.7691 39.5111 77.6242 39.5111C77.0573 39.5111 76.7676 39.8443 76.7676 40.5049C76.7676 41.1655 77.051 41.4643 77.6179 41.4643C77.8321 41.4643 78.0526 41.4126 78.2856 41.3034L78.4116 41.8147C78.1723 41.9296 77.9014 41.987 77.5864 41.987L77.5927 41.9928Z"
                          fill="white" />
                        <path
                          d="M80.8683 41.4424V41.948H78.8904V39.0469H80.8683V39.5524H79.5392V40.2246H80.673V40.7301H79.5392V41.4424H80.8683Z"
                          fill="white" />
                        <path
                          d="M84.3517 41.9475H82.2919V41.465L82.7706 41.0513C83.0541 40.8101 83.2556 40.609 83.3753 40.4596C83.495 40.3103 83.5517 40.1552 83.5517 39.9943C83.5517 39.7415 83.4131 39.6151 83.1297 39.6151C82.947 39.6151 82.7454 39.6841 82.5375 39.822L82.2604 39.4141C82.5249 39.2188 82.8399 39.1211 83.2115 39.1211C83.5265 39.1211 83.7722 39.1958 83.9485 39.3394C84.1249 39.483 84.2068 39.6841 84.2068 39.9426C84.2068 40.2011 84.1186 40.4309 83.9359 40.6664C83.7533 40.902 83.5013 41.1433 83.1863 41.4018L83.1171 41.4592H84.3643V41.9533L84.3517 41.9475Z"
                          fill="white" />
                        <path
                          d="M85.9328 41.9912C85.7186 41.9912 85.536 41.951 85.3785 41.8763C85.221 41.8016 85.095 41.6982 85.0068 41.5603C84.9124 41.4282 84.8431 41.2731 84.799 41.1065C84.7549 40.9399 84.7297 40.7561 84.7297 40.555C84.7297 40.3539 84.7549 40.1701 84.799 39.9977C84.8431 39.8311 84.9124 39.676 85.0068 39.5382C85.1013 39.4003 85.2273 39.2969 85.3785 39.2165C85.536 39.1418 85.7186 39.1016 85.9328 39.1016C86.21 39.1016 86.443 39.1705 86.6257 39.3026C86.8084 39.4405 86.9407 39.6071 87.01 39.8197C87.0855 40.0265 87.1233 40.2677 87.1233 40.5492C87.1233 40.7561 87.1044 40.9399 87.0604 41.1065C87.0163 41.2731 86.947 41.4224 86.8588 41.5603C86.7706 41.6982 86.6446 41.7959 86.4871 41.8705C86.3297 41.9452 86.147 41.9797 85.9391 41.9797L85.9328 41.9912ZM85.9328 41.4971C86.2919 41.4971 86.4682 41.1869 86.4682 40.5607C86.4682 39.9346 86.2919 39.6128 85.9328 39.6128C85.5738 39.6128 85.3848 39.9288 85.3848 40.5607C85.3848 41.1927 85.5675 41.4971 85.9328 41.4971Z"
                          fill="white" />
                        <path
                          d="M89.454 41.9475H87.3942V41.465L87.8729 41.0513C88.1564 40.8101 88.3579 40.609 88.4776 40.4596C88.5973 40.3103 88.654 40.1552 88.654 39.9943C88.654 39.7415 88.5154 39.6151 88.232 39.6151C88.0493 39.6151 87.8477 39.6841 87.6398 39.822L87.3627 39.4141C87.6272 39.2188 87.9422 39.1211 88.3138 39.1211C88.6288 39.1211 88.8745 39.1958 89.0508 39.3394C89.2272 39.483 89.3091 39.6841 89.3091 39.9426C89.3091 40.2011 89.2209 40.4309 89.0382 40.6664C88.8556 40.902 88.6036 41.1433 88.2886 41.4018L88.2194 41.4592H89.4666V41.9533L89.454 41.9475Z"
                          fill="white" />
                        <path
                          d="M92.1059 41.0091C92.1059 41.3135 92.0115 41.5548 91.8162 41.7272C91.6209 41.9052 91.3627 41.9914 91.0225 41.9914C90.6508 41.9914 90.3611 41.8708 90.1532 41.6237C89.939 41.3767 89.8383 41.0665 89.8383 40.6931C89.8383 40.4805 89.8635 40.2795 89.9201 40.0899C89.9768 39.9061 90.0587 39.7337 90.1658 39.5901C90.2729 39.4407 90.4178 39.3258 90.6068 39.2397C90.7894 39.1535 90.9973 39.1133 91.2367 39.1133C91.476 39.1133 91.6524 39.1363 91.8099 39.1822L91.6902 39.6475C91.5453 39.6188 91.413 39.6016 91.2934 39.6016C90.8461 39.6016 90.5816 39.8429 90.506 40.3197C90.7075 40.2048 90.9343 40.1473 91.18 40.1473C91.4697 40.1473 91.6965 40.2278 91.8603 40.3829C92.0241 40.5437 92.1122 40.7505 92.1122 41.0033L92.1059 41.0091ZM91.0288 41.4974C91.1674 41.4974 91.2745 41.4571 91.35 41.3767C91.4256 41.2963 91.4634 41.1871 91.4634 41.055C91.4634 40.785 91.2997 40.6529 90.9784 40.6529C90.8272 40.6529 90.6634 40.6931 90.4745 40.7793C90.4871 41.0033 90.5375 41.1814 90.6319 41.3078C90.7264 41.4342 90.8587 41.5031 91.0288 41.5031V41.4974Z"
                          fill="white" />
                      </svg>
                    </a>
                    <a href="https://www.vision2030.gov.sa/" target="_blank" rel="noopener noreferrer">
                    <svg height="42" viewBox="0 0 373 250" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path d="M223.58 4.52002H219.39V8.70002H223.58V4.52002Z" fill="white"></path>
                      <path d="M310.67 56.49H306.48V60.67H310.67V56.49Z" fill="white"></path>
                      <path d="M357.39 52.01C358.42 51.52 359.25 50.82 359.88 49.89C360.51 48.97 360.95 47.85 361.21 46.55C361.47 45.24 361.6 43.77 361.6 42.14V12.88H357.41V42.14C357.41 43.32 357.35 44.32 357.23 45.14C357.11 45.96 356.9 46.62 356.6 47.13C356.3 47.63 355.9 48 355.4 48.23C354.89 48.46 354.24 48.57 353.44 48.57C353.06 48.57 352.72 48.55 352.45 48.52C352.17 48.49 351.79 48.4 351.3 48.26L350.2 52.28C351.35 52.6 352.49 52.75 353.6 52.75C355.09 52.74 356.36 52.5 357.39 52.01Z" fill="white"></path>
                      <path d="M342.99 5.47H337.24V4.5C337.24 3.97 337.35 3.56 337.58 3.27C337.77 3.03 338.22 2.91 338.87 2.91H341.75V0H338.8C337.2 0 335.95 0.42 335.11 1.25C334.26 2.08 333.83 3.44 333.83 5.27V8.68H343V5.47H342.99Z" fill="white"></path>
                      <path d="M302.29 56.49H298.1V60.67H302.29V56.49Z" fill="white"></path>
                      <path d="M231.96 4.52002H227.77V8.70002H231.96V4.52002Z" fill="white"></path>
                      <path d="M324.12 37.93C325.71 39.06 327.55 39.9 329.64 40.44C331.73 40.98 333.95 41.25 336.29 41.25H337.4L338.81 38.3V42.45C338.81 44.64 338.18 46.21 336.93 47.15C335.67 48.09 333.91 48.56 331.64 48.56C330.35 48.56 329.19 48.44 328.16 48.19C327.13 47.95 325.95 47.53 324.63 46.94L323.32 50.96C324.92 51.55 326.35 52 327.59 52.29C328.83 52.59 330.08 52.73 331.33 52.73C335.41 52.73 338.38 51.76 340.23 49.83C342.08 47.89 343 45.29 343 42.02V12.88H336.3C333.72 12.88 331.36 13.25 329.23 13.98C327.1 14.71 325.28 15.71 323.76 16.98C322.24 18.25 321.06 19.77 320.23 21.53C319.39 23.29 318.97 25.2 318.97 27.25C318.97 29.72 319.43 31.84 320.36 33.6C321.27 35.35 322.53 36.8 324.12 37.93ZM324.12 23.12C324.76 21.87 325.62 20.8 326.68 19.91C327.74 19.02 328.96 18.34 330.32 17.85C331.64 17.38 333.01 17.14 334.43 17.13V17.12H341.2L338.81 18.31V37.08H334.57C333 37.08 331.53 36.86 330.15 36.42C328.77 35.99 327.56 35.35 326.51 34.51C325.46 33.67 324.64 32.65 324.05 31.43C323.46 30.21 323.16 28.82 323.16 27.25C323.15 25.75 323.47 24.37 324.12 23.12Z" fill="white"></path>
                      <path d="M220.53 52.16H229.23V49.03C229.31 49.18 229.4 49.33 229.49 49.46C230.12 50.38 230.95 51.09 231.98 51.58C233.01 52.07 234.27 52.31 235.77 52.31H312.12V12.88H307.93V48.14H303.58H235.92C235.12 48.14 234.46 48.03 233.96 47.8C233.46 47.57 233.05 47.21 232.76 46.7C232.47 46.19 232.26 45.53 232.13 44.71C232.01 43.89 231.95 42.89 231.95 41.71V12.88H220.63C209.05 12.88 201.17 22.05 201.17 32.52V32.63C201.18 43.1 208.95 52.16 220.53 52.16ZM205.64 32.41C205.64 23.78 211.84 16.84 220.53 16.84H230.33L227.76 18.12V41.71C227.76 43.35 227.89 44.82 228.15 46.12C228.3 46.88 228.52 47.57 228.8 48.2H220.63C211.93 48.2 205.63 41.15 205.63 32.52V32.41H205.64Z" fill="white"></path>
                      <path d="M180.23 41.78L182.06 46.6L155.46 12.83H151.31V52.08H155.63V22.5L153.17 16.78L181.02 52.08H184.56V12.83H180.23V41.78Z" fill="white"></path>
                      <path d="M239.87 114.78L264.01 91.77V75.99H199.49V93.96H232.57L244.84 86.08L214.21 116.83L217.49 130.17H226.22C237.95 130.17 244.91 134.53 244.91 142.29V142.56C244.91 149.64 239.32 154.13 231.41 154.13C221.73 154.13 214.91 150.05 208.36 142.69L193.9 156.44C202.08 166.11 213.95 172.91 231.14 172.91C252.42 172.91 265.51 159.7 265.51 141.73V141.46C265.52 123.77 252.83 116.96 239.87 114.78Z" fill="white"></path>
                      <path d="M124.05 12.16C112.09 12.16 103.94 21.63 103.94 32.46V32.57C103.94 43.39 111.97 52.75 123.94 52.75C135.91 52.75 144.05 43.28 144.05 32.46V32.35C144.04 21.52 136.01 12.16 124.05 12.16ZM139.44 32.57C139.44 41.48 133.04 48.66 124.05 48.66C115.06 48.66 108.55 41.37 108.55 32.46V32.35C108.55 23.44 114.96 16.26 123.94 16.26C132.93 16.26 139.44 23.55 139.44 32.46V32.57Z" fill="white"></path>
                      <path d="M321.81 74.35C292.35 74.35 270.93 96.54 270.93 123.63V123.9C270.93 150.99 292.07 172.91 321.53 172.91C350.99 172.91 372.41 150.72 372.41 123.63V123.36C372.42 96.27 351.27 74.35 321.81 74.35ZM350.46 123.9C350.46 140.24 338.73 153.58 321.81 153.58C304.9 153.58 292.89 139.97 292.89 123.63V123.36C292.89 107.02 304.62 93.68 321.53 93.68C338.44 93.68 350.45 107.29 350.45 123.63V123.9H350.46Z" fill="white"></path>
                      <path d="M96.2798 12.83H91.8398V52.07H96.2798V12.83Z" fill="white"></path>
                      <path d="M20.5596 160.39L24.6096 157.27L45.4796 141.19C62.3896 128.12 70.5797 120.23 70.5797 104.3V104.03C70.5797 86.19 57.4897 74.62 37.7097 74.62C20.6597 74.62 11.5196 81.56 2.10965 94.36L16.8396 106.2C23.9296 97.49 28.9797 93.41 36.3497 93.41C43.8497 93.41 49.1696 97.77 49.1696 105.8C49.1696 113.69 44.5296 118.87 32.6696 128.67L0.889648 154.67V171.28H71.5497V153.17H35.1396L20.5596 160.39Z" fill="white"></path>
                      <path d="M20.8498 52.36L37.7498 12.83H32.9798L19.6198 45L20.0398 49.32L18.9398 46.64L7.36977 18.68L4.94976 12.83H0.00976562L16.9198 52.36H20.8498Z" fill="white"></path>
                      <path d="M48.2198 12.83H43.7798V52.07H48.2198V12.83Z" fill="white"></path>
                      <path d="M71.7196 30.32C63.0696 28.47 61.0496 26.4 61.0496 22.7V22.59C61.0496 19.06 64.3096 16.26 69.4196 16.26C73.4596 16.26 77.1196 17.55 80.7696 20.52L83.3496 17.1C79.3596 13.9 75.2596 12.28 69.5296 12.28C62.0596 12.28 56.6096 16.82 56.6096 22.99V23.1C56.6096 29.55 60.7696 32.46 69.8096 34.43C78.0696 36.17 80.0296 38.24 80.0296 41.89V42C80.0296 45.87 76.5496 48.67 71.2696 48.67C65.8196 48.67 61.8896 46.82 57.7896 43.12L55.0396 46.37C59.7596 50.58 64.8696 52.65 71.0996 52.65C78.9096 52.65 84.5196 48.28 84.5196 41.55V41.44C84.5196 35.43 80.4796 32.23 71.7196 30.32Z" fill="white"></path>
                      <path d="M128.6 68.08L133.44 72.71L128.8 77.54L123.96 72.91L128.6 68.08Z" fill="white"></path>
                      <path d="M139.67 72.91L144.5 77.54L149.14 72.71L144.31 68.08L139.67 72.91Z" fill="white"></path>
                      <path d="M132.86 78.99L136.63 82.6L140.24 78.84L136.47 75.23L132.86 78.99Z" fill="white"></path>
                      <path d="M146.67 81.05L150.43 84.66L154.04 80.9001L150.28 77.3L146.67 81.05Z" fill="white"></path>
                      <path d="M126.7 84.73L130.47 88.34L134.08 84.58L130.32 80.98L126.7 84.73Z" fill="white"></path>
                      <path d="M139.34 85.17L143.1 88.78L146.71 85.02L142.95 81.41L139.34 85.17Z" fill="white"></path>
                      <path d="M134.4 89.49L137.04 92.03L139.59 89.39L136.94 86.85L134.4 89.49Z" fill="white"></path>
                      <path d="M144.39 91.49L147.03 94.02L149.58 91.38L146.93 88.84L144.39 91.49Z" fill="white"></path>
                      <path d="M174.417 69.8743L166.204 71.3798L167.708 79.583L175.921 78.0775L174.417 69.8743Z" fill="white"></path>
                      <path d="M162.039 73.5985L155.449 74.8052L156.654 81.3855L163.244 80.1788L162.039 73.5985Z" fill="white"></path>
                      <path d="M174.814 82.7817L168.224 83.9904L169.431 90.5706L176.021 89.362L174.814 82.7817Z" fill="white"></path>
                      <path d="M164.249 83.9998L159.114 84.9404L160.053 90.065L165.187 89.1243L164.249 83.9998Z" fill="white"></path>
                      <path d="M174.238 93.7294L169.104 94.67L170.043 99.7946L175.177 98.854L174.238 93.7294Z" fill="white"></path>
                      <path d="M156.559 86.0899L151.424 87.0305L152.363 92.1551L157.498 91.2144L156.559 86.0899Z" fill="white"></path>
                      <path d="M165.879 92.7997L160.744 93.7404L161.683 98.8649L166.817 97.9243L165.879 92.7997Z" fill="white"></path>
                      <path d="M157.817 93.7088L154.208 94.3705L154.867 97.9705L158.477 97.3088L157.817 93.7088Z" fill="white"></path>
                      <path d="M164.771 101.159L161.161 101.819L161.819 105.419L165.429 104.759L164.771 101.159Z" fill="white"></path>
                      <path d="M190.64 98.24L187.05 105.77L194.59 109.35L198.18 101.83L190.64 98.24Z" fill="white"></path>
                      <path d="M179.93 94.76L177.05 100.8L183.1 103.68L185.98 97.64L179.93 94.76Z" fill="white"></path>
                      <path d="M184.88 109.64L182 115.68L188.06 118.55L190.94 112.52L184.88 109.64Z" fill="white"></path>
                      <path d="M176.95 105.11L174.71 109.82L179.42 112.05L181.67 107.35L176.95 105.11Z" fill="white"></path>
                      <path d="M179.35 118.84L177.1 123.54L181.81 125.78L184.06 121.07L179.35 118.84Z" fill="white"></path>
                      <path d="M169.49 102.31L167.25 107.02L171.96 109.25L174.21 104.55L169.49 102.31Z" fill="white"></path>
                      <path d="M173.12 113.19L170.87 117.9L175.59 120.13L177.83 115.43L173.12 113.19Z" fill="white"></path>
                      <path d="M167.45 109.88L165.87 113.19L169.19 114.76L170.76 111.45L167.45 109.88Z" fill="white"></path>
                      <path d="M168.719 119.954L167.146 123.259L170.46 124.836L172.033 121.531L168.719 119.954Z" fill="white"></path>
                      <path d="M194.7 134.3L187.38 138.3L191.4 145.61L198.72 141.61L194.7 134.3Z" fill="white"></path>
                      <path d="M188.06 125.22L182.18 128.44L185.41 134.3L191.28 131.08L188.06 125.22Z" fill="white"></path>
                      <path d="M183.35 140.18L177.48 143.4L180.7 149.26L186.58 146.04L183.35 140.18Z" fill="white"></path>
                      <path d="M179.58 131.87L175 134.38L177.51 138.94L182.09 136.44L179.58 131.87Z" fill="white"></path>
                      <path d="M173.47 144.4L168.9 146.91L171.41 151.47L175.98 148.97L173.47 144.4Z" fill="white"></path>
                      <path d="M175.17 125.25L170.6 127.75L173.1 132.32L177.68 129.81L175.17 125.25Z" fill="white"></path>
                      <path d="M171.73 136.19L167.15 138.69L169.66 143.26L174.24 140.75L171.73 136.19Z" fill="white"></path>
                      <path d="M169.08 130.19L165.86 131.95L167.63 135.16L170.84 133.4L169.08 130.19Z" fill="white"></path>
                      <path d="M164.18 139.11L160.96 140.87L162.73 144.08L165.95 142.32L164.18 139.11Z" fill="white"></path>
                      <path d="M168.582 164.837L167.559 173.114L175.846 174.138L176.869 165.86L168.582 164.837Z" fill="white"></path>
                      <path d="M170.15 153.849L169.327 160.488L175.976 161.312L176.799 154.673L170.15 153.849Z" fill="white"></path>
                      <path d="M157.572 163.24L156.749 169.879L163.398 170.703L164.221 164.064L157.572 163.24Z" fill="white"></path>
                      <path d="M160.832 154.51L160.193 159.68L165.373 160.321L166.012 155.15L160.832 154.51Z" fill="white"></path>
                      <path d="M148.537 161.099L147.897 166.27L153.078 166.91L153.717 161.74L148.537 161.099Z" fill="white"></path>
                      <path d="M161.155 146.541L160.515 151.711L165.695 152.353L166.336 147.183L161.155 146.541Z" fill="white"></path>
                      <path d="M151.953 153.393L151.312 158.564L156.493 159.206L157.134 154.035L151.953 153.393Z" fill="white"></path>
                      <path d="M154.843 147.185L154.394 150.818L158.036 151.268L158.485 147.636L154.843 147.185Z" fill="white"></path>
                      <path d="M145.631 151.564L145.182 155.196L148.824 155.646L149.273 152.014L145.631 151.564Z" fill="white"></path>
                      <path d="M143.87 181.12L137.75 175.45L132.06 181.56L138.18 187.23L143.87 181.12Z" fill="white"></path>
                      <path d="M142.74 64.34L136.62 58.67L130.93 64.78L137.06 70.4501L142.74 64.34Z" fill="white"></path>
                      <path d="M150.4 171.97L145.49 167.42L140.93 172.32L145.84 176.87L150.4 171.97Z" fill="white"></path>
                      <path d="M134.7 172.23L129.78 167.68L125.22 172.58L130.14 177.13L134.7 172.23Z" fill="white"></path>
                      <path d="M141.4 166.04L137.58 162.49L134.03 166.31L137.85 169.85L141.4 166.04Z" fill="white"></path>
                      <path d="M127.56 164.2L123.74 160.66L120.19 164.47L124.02 168.02L127.56 164.2Z" fill="white"></path>
                      <path d="M146.31 159.77L142.49 156.23L138.94 160.04L142.76 163.59L146.31 159.77Z" fill="white"></path>
                      <path d="M134.82 159.96L131 156.42L127.45 160.23L131.27 163.78L134.82 159.96Z" fill="white"></path>
                      <path d="M139.69 155.56L137 153.06L134.5 155.75L137.19 158.24L139.69 155.56Z" fill="white"></path>
                      <path d="M129.67 153.73L126.98 151.24L124.48 153.92L127.17 156.41L129.67 153.73Z" fill="white"></path>
                      <path d="M106.548 166.02L98.3608 167.661L99.9996 175.838L108.187 174.197L106.548 166.02Z" fill="white"></path>
                      <path d="M117.531 164.013L110.961 165.328L112.275 171.888L118.845 170.572L117.531 164.013Z" fill="white"></path>
                      <path d="M104.677 155.071L98.1074 156.389L99.4233 162.949L105.993 161.631L104.677 155.071Z" fill="white"></path>
                      <path d="M113.992 155.405L108.874 156.43L109.897 161.539L115.015 160.513L113.992 155.405Z" fill="white"></path>
                      <path d="M121.65 153.185L116.532 154.211L117.555 159.319L122.674 158.294L121.65 153.185Z" fill="white"></path>
                      <path d="M112.23 146.634L107.112 147.659L108.135 152.767L113.253 151.742L112.23 146.634Z" fill="white"></path>
                      <path d="M119.076 147.462L115.477 148.182L116.195 151.771L119.794 151.051L119.076 147.462Z" fill="white"></path>
                      <path d="M112.033 140.109L108.435 140.831L109.155 144.42L112.753 143.698L112.033 140.109Z" fill="white"></path>
                      <path d="M83.31 147.74L86.78 140.15L79.17 136.7L75.71 144.28L83.31 147.74Z" fill="white"></path>
                      <path d="M94.08 151.04L96.85 144.96L90.76 142.18L87.98 148.27L94.08 151.04Z" fill="white"></path>
                      <path d="M88.8798 136.25L91.6598 130.16L85.5598 127.39L82.7798 133.48L88.8798 136.25Z" fill="white"></path>
                      <path d="M94.2793 133.731L92.1177 138.472L96.8674 140.638L99.029 135.897L94.2793 133.731Z" fill="white"></path>
                      <path d="M103.24 152.5L105.4 147.76L100.65 145.6L98.4902 150.34L103.24 152.5Z" fill="white"></path>
                      <path d="M91.6934 120.056L89.5308 124.796L94.2799 126.963L96.4426 122.223L91.6934 120.056Z" fill="white"></path>
                      <path d="M101.823 136.424L99.6602 141.164L104.409 143.33L106.572 138.59L101.823 136.424Z" fill="white"></path>
                      <path d="M97.9958 125.624L95.8315 130.363L100.58 132.531L102.744 127.792L97.9958 125.624Z" fill="white"></path>
                      <path d="M106.3 135.72L107.83 132.39L104.49 130.87L102.96 134.21L106.3 135.72Z" fill="white"></path>
                      <path d="M104.88 125.66L106.4 122.32L103.06 120.8L101.54 124.14L104.88 125.66Z" fill="white"></path>
                      <path d="M78.6498 111.76L85.9098 107.63L81.7698 100.39L74.5098 104.52L78.6498 111.76Z" fill="white"></path>
                      <path d="M85.4401 120.72L91.2701 117.41L87.9401 111.6L82.1201 114.91L85.4401 120.72Z" fill="white"></path>
                      <path d="M89.9001 105.69L95.7201 102.38L92.4001 96.5699L86.5801 99.8799L89.9001 105.69Z" fill="white"></path>
                      <path d="M93.81 113.93L98.34 111.35L95.76 106.83L91.23 109.41L93.81 113.93Z" fill="white"></path>
                      <path d="M99.7101 101.31L104.24 98.7299L101.66 94.2L97.1201 96.78L99.7101 101.31Z" fill="white"></path>
                      <path d="M98.3302 120.48L102.86 117.9L100.28 113.38L95.7402 115.96L98.3302 120.48Z" fill="white"></path>
                      <path d="M101.59 109.49L106.12 106.91L103.54 102.38L99 104.97L101.59 109.49Z" fill="white"></path>
                      <path d="M104.34 115.44L107.53 113.63L105.71 110.45L102.52 112.26L104.34 115.44Z" fill="white"></path>
                      <path d="M109.09 106.45L112.28 104.63L110.46 101.45L107.27 103.26L109.09 106.45Z" fill="white"></path>
                      <path d="M96.8377 71.584L95.9512 79.8765L104.254 80.7641L105.14 72.4716L96.8377 71.584Z" fill="white"></path>
                      <path d="M96.9169 84.448L96.2051 91.0999L102.867 91.8127L103.579 85.1609L96.9169 84.448Z" fill="white"></path>
                      <path d="M109.353 74.8651L108.641 81.517L115.303 82.2298L116.015 75.578L109.353 74.8651Z" fill="white"></path>
                      <path d="M107.538 85.3066L106.983 90.4869L112.174 91.0429L112.729 85.8626L107.538 85.3066Z" fill="white"></path>
                      <path d="M119.733 78.4671L119.18 83.6479L124.371 84.2018L124.924 79.021L119.733 78.4671Z" fill="white"></path>
                      <path d="M107.362 93.2533L106.807 98.4336L111.997 98.9895L112.552 93.8092L107.362 93.2533Z" fill="white"></path>
                      <path d="M116.45 86.2426L115.896 91.4229L121.086 91.9788L121.641 86.7985L116.45 86.2426Z" fill="white"></path>
                      <path d="M115.025 94.2202L114.635 97.8594L118.284 98.2499L118.674 94.6107L115.025 94.2202Z" fill="white"></path>
                      <path d="M124.15 89.6875L123.763 93.327L127.413 93.7153L127.8 90.0758L124.15 89.6875Z" fill="white"></path>
                      <path d="M163.18 128.07C162.56 127.72 161.43 127.13 159.87 126.64C157.62 125.92 154.48 125.38 150.76 125.97C149.51 126.17 148.19 126.5 146.82 126.99C146.69 127.04 146.56 127.08 146.43 127.13C145.73 127.39 145.04 127.71 144.34 128.06C143.74 128.36 143.15 128.69 142.56 129.05C141.21 129.86 139.88 130.78 138.59 131.78C137.12 122.89 137.84 114.64 138.18 110.59C138.18 110.57 138.21 110.56 138.23 110.57C141.38 112.55 140.5 118.08 139.83 119.67C139.82 119.7 139.86 119.72 139.89 119.7C143.81 115.07 141.9 109.95 141.09 108.66C141.07 108.63 141.11 108.6 141.14 108.62C141.71 109.08 142.17 109.57 142.56 110.07C143.77 111.62 144.22 113.29 144.34 114.77C144.4 115.55 144.38 116.27 144.34 116.89C144.33 117.06 144.32 117.21 144.3 117.36C144.3 117.38 144.32 117.39 144.34 117.38C144.35 117.38 144.36 117.38 144.36 117.37C145.42 115.29 145.93 112.15 144.34 109.1C143.91 108.27 143.33 107.45 142.56 106.66C142.34 106.43 142.1 106.21 141.85 105.98C141.83 105.96 141.85 105.92 141.88 105.93C142.12 105.99 142.34 106.06 142.56 106.13C143.23 106.36 143.82 106.68 144.34 107.07C145.53 107.95 146.34 109.15 146.82 110.41C147.22 111.45 147.4 112.52 147.4 113.47C147.4 113.51 147.45 113.51 147.46 113.48C148.2 110.77 147.75 108.72 146.82 107.23C146.16 106.16 145.25 105.38 144.34 104.83C143.72 104.46 143.1 104.2 142.56 104.04C142.24 103.94 141.96 103.88 141.71 103.85C141.67 103.85 141.68 103.79 141.72 103.79C142.01 103.76 142.29 103.75 142.56 103.74C143.22 103.73 143.81 103.79 144.34 103.88C145.39 104.08 146.21 104.44 146.82 104.86C147.82 105.53 148.32 106.32 148.49 106.64C148.5 106.67 148.55 106.65 148.55 106.62C148.41 105.19 147.75 104.07 146.83 103.27C146.12 102.65 145.25 102.22 144.35 101.96C143.76 101.8 143.16 101.71 142.57 101.7C141.77 101.69 141 101.82 140.35 102.1C140.32 102.11 140.3 102.08 140.31 102.05C140.89 101.22 141.71 100.72 142.57 100.45C143.17 100.27 143.78 100.2 144.35 100.24C144.9 100.27 145.4 100.39 145.81 100.58C145.84 100.59 145.86 100.56 145.84 100.53C145.39 100.02 144.88 99.62 144.35 99.32C143.78 99 143.18 98.81 142.57 98.74C140.98 98.56 139.36 99.24 138.28 100.87C138.27 100.89 138.24 100.89 138.23 100.87C138.02 100.5 137.82 100.09 137.64 99.66C137.21 98.66 136.86 97.54 136.67 96.42C136.66 96.38 136.62 96.38 136.62 96.42C136.43 97.54 136.07 98.67 135.64 99.67C135.45 100.1 135.26 100.5 135.05 100.86C135.04 100.88 135.01 100.88 135 100.86C133.12 98.01 129.59 98.07 127.44 100.53C127.42 100.55 127.44 100.59 127.47 100.58C128.9 99.9 131.61 100.1 132.97 102.05C132.99 102.08 132.96 102.11 132.93 102.1C130.84 101.22 127.6 101.79 125.87 103.85C125.25 104.58 124.83 105.51 124.72 106.62C124.72 106.65 124.76 106.67 124.78 106.64C124.91 106.39 125.25 105.85 125.87 105.3C126.88 104.41 128.64 103.51 131.55 103.78C131.59 103.79 131.59 103.84 131.56 103.84C130.07 104.02 127 105.35 125.87 108.39C125.37 109.73 125.24 111.41 125.8 113.47C125.81 113.5 125.86 113.5 125.86 113.46C125.86 113.41 125.86 113.37 125.87 113.32C125.9 110.56 127.48 106.86 131.39 105.92C131.42 105.91 131.44 105.95 131.42 105.97C127.05 109.81 127.48 114.55 128.91 117.35C128.93 117.38 128.97 117.37 128.97 117.34C128.78 115.22 128.63 111.45 132.13 108.6C132.16 108.58 132.2 108.61 132.18 108.64C131.37 109.94 129.46 115.05 133.38 119.68C133.4 119.71 133.45 119.69 133.44 119.65C132.78 118.07 131.9 112.53 135.04 110.55C135.06 110.54 135.09 110.55 135.09 110.57C135.43 114.62 136.15 122.87 134.68 131.76C132.15 129.82 129.48 128.12 126.84 127.12C126.51 127 126.19 126.89 125.88 126.78C123.15 125.88 120.65 125.63 118.47 125.73C114.97 125.88 112.29 126.93 110.82 127.66C110.52 127.81 110.27 127.95 110.08 128.06C110 128.11 110.06 128.22 110.14 128.19C110.37 128.12 110.59 128.05 110.82 127.99C113.51 127.23 116.06 127.03 118.47 127.24C121.1 127.47 123.57 128.19 125.88 129.23C129.12 130.69 132.05 132.77 134.7 134.97C130.96 138.23 127.8 141.4 125.88 142.51C125.78 142.56 125.69 142.62 125.6 142.67C125.14 142.04 124.34 141.68 123.47 141.87C122.71 142.04 122.09 142.65 121.92 143.4C121.77 144.07 121.94 144.8 122.32 145.18C123.34 146.2 124.52 146.27 125.88 145.7C128.47 144.61 131.69 141.19 135.64 137.57C135.97 137.27 136.3 136.96 136.64 136.66C136.98 136.96 137.31 137.26 137.64 137.56C139.43 139.2 141.07 140.8 142.57 142.16C143.19 142.72 143.78 143.24 144.35 143.7C145.23 144.42 146.06 145.01 146.83 145.42C148.34 146.22 149.65 146.32 150.77 145.33C150.83 145.28 150.89 145.23 150.95 145.17C151.21 144.91 151.51 144.06 151.35 143.39C151.26 143 151.05 142.65 150.76 142.38C150.5 142.13 150.16 141.94 149.8 141.86C148.93 141.67 148.13 142.03 147.67 142.66C147.41 142.53 147.13 142.35 146.82 142.14C146.11 141.65 145.28 140.96 144.34 140.14C143.78 139.65 143.19 139.11 142.56 138.54C141.34 137.43 140 136.21 138.58 134.96C139.84 133.91 141.17 132.89 142.56 131.95C143.14 131.56 143.73 131.18 144.34 130.82C145.15 130.34 145.97 129.9 146.82 129.49C148.09 128.88 149.4 128.37 150.76 127.98C153.58 127.17 156.61 126.9 159.87 127.42C160.93 127.59 162.01 127.84 163.12 128.18C163.2 128.23 163.26 128.12 163.18 128.07Z" fill="white"></path>
                      <path d="M21.8599 218.88H19.3999V221.34H21.8599V218.88Z" fill="white"></path>
                      <path d="M185.03 216.04V199.8H182.63V213.64H173.58V199.8H171.18V213.64H161.65V199.8H157.88C156.38 199.8 155.02 200 153.8 200.41C152.58 200.82 151.54 201.39 150.68 202.11C149.82 202.84 149.16 203.7 148.69 204.7C148.22 205.7 147.99 206.77 147.99 207.93C147.99 209.35 148.26 210.56 148.8 211.58C149.34 212.6 150.06 213.43 150.96 214.09C151.86 214.75 152.91 215.24 154.11 215.55C155.31 215.87 156.57 216.03 157.89 216.03L185.03 216.04ZM154.36 213.26C153.57 213 152.88 212.63 152.29 212.14C151.7 211.65 151.24 211.05 150.9 210.35C150.56 209.64 150.39 208.84 150.39 207.94C150.39 207.08 150.58 206.3 150.95 205.59C151.32 204.88 151.81 204.28 152.42 203.77C153.03 203.26 153.72 202.87 154.5 202.6C155.28 202.33 156.08 202.2 156.9 202.2H159.27V213.65H156.9C156 213.65 155.15 213.52 154.36 213.26Z" fill="white"></path>
                      <path d="M136.16 194.58H133.76V216.04H136.16V194.58Z" fill="white"></path>
                      <path d="M161.65 194.58H159.19V197.04H161.65V194.58Z" fill="white"></path>
                      <path d="M185.06 218.85H182.6V221.31H185.06V218.85Z" fill="white"></path>
                      <path d="M26.0601 218.88H23.6001V221.34H26.0601V218.88Z" fill="white"></path>
                      <path d="M124.38 213.65H120.46C119.06 213.65 117.96 213.45 117.16 213.05C116.36 212.65 115.73 212.05 115.27 211.26C115.41 210.64 115.48 209.89 115.48 209.02V201.3H113.08V209.4C113.08 210.78 112.71 211.88 111.97 212.71C111.23 213.54 110.11 213.95 108.61 213.95C107.91 213.95 107.27 213.84 106.69 213.62C106.11 213.4 105.61 213.1 105.19 212.72C104.77 212.34 104.44 211.9 104.2 211.39C103.96 210.88 103.83 210.34 103.81 209.76V201.3H101.41V209.58C101.41 210.32 101.3 210.94 101.08 211.45C100.86 211.96 100.56 212.38 100.18 212.71C99.8001 213.04 99.3701 213.28 98.8901 213.43C98.4101 213.58 97.9001 213.65 97.3601 213.65H92.7201V201.3H90.3201V213.65H77.7601L81.9501 203.63C81.3101 203.07 80.6301 202.55 79.9101 202.06C79.1901 201.57 78.4501 201.14 77.6901 200.77C76.9301 200.4 76.1601 200.11 75.3801 199.9C74.6001 199.69 73.8301 199.59 73.0701 199.59C72.2901 199.59 71.5101 199.7 70.7301 199.9C69.9501 200.11 69.1801 200.39 68.4101 200.75C67.6401 201.11 66.9001 201.54 66.1901 202.04C65.4801 202.54 64.8201 203.06 64.2001 203.62L68.4201 213.64H60.1201V199.8H56.2901C54.8101 199.8 53.4601 200.01 52.2401 200.43C51.0201 200.85 49.9801 201.42 49.1101 202.15C48.2401 202.88 47.5701 203.75 47.0901 204.75C46.6101 205.76 46.3701 206.85 46.3701 208.03C46.3701 209.45 46.6301 210.66 47.1601 211.66C47.6901 212.67 48.4101 213.5 49.3201 214.14C50.2301 214.79 51.2801 215.27 52.4801 215.57C53.6801 215.88 54.9501 216.03 56.2901 216.03H57.7301V216.72C57.7301 217.98 57.3701 218.87 56.6501 219.41C55.9301 219.95 54.9201 220.22 53.6201 220.22C52.8801 220.22 52.2201 220.15 51.6301 220.01C51.0401 219.87 50.3701 219.63 49.6101 219.29L48.8601 221.59C49.7801 221.93 50.5901 222.18 51.3001 222.35C52.0101 222.52 52.7201 222.61 53.4401 222.61C55.7801 222.61 57.4701 222.06 58.5301 220.95C59.5901 219.84 60.1201 218.35 60.1201 216.48V216.03H97.3601C98.5201 216.03 99.5501 215.78 100.46 215.28C101.37 214.78 102.1 214.11 102.66 213.28C103.24 214.24 104.03 214.99 105.03 215.52C106.03 216.06 107.22 216.33 108.6 216.33C110.04 216.33 111.26 216.08 112.27 215.58C113.28 215.08 114.05 214.32 114.59 213.28C114.79 213.62 115.03 213.95 115.32 214.28C115.61 214.61 115.98 214.9 116.44 215.16C116.9 215.42 117.46 215.63 118.1 215.79C118.59 215.91 119.18 215.97 119.82 216V216.03H126.74V194.57H124.34V213.65H124.38ZM57.7401 213.65H55.3101C54.4101 213.65 53.5701 213.53 52.7801 213.28C51.9901 213.03 51.3001 212.67 50.7001 212.19C50.1001 211.71 49.6301 211.12 49.2901 210.43C48.9501 209.73 48.7801 208.93 48.7801 208.04C48.7801 207.18 48.9601 206.4 49.3301 205.68C49.7001 204.96 50.1901 204.35 50.8001 203.84C51.4101 203.33 52.1001 202.94 52.8801 202.66C53.6601 202.38 54.4701 202.24 55.3101 202.24H57.7401V213.65ZM70.9601 213.65L67.2201 204.47C68.1401 203.69 69.0801 203.12 70.0501 202.77C71.0201 202.41 72.0301 202.23 73.0901 202.23C74.1701 202.23 75.2401 202.44 76.3001 202.87C77.3601 203.3 78.2601 203.83 79.0001 204.47L75.2601 213.65H70.9601Z" fill="white"></path>
                      <path d="M13.6602 194.58H11.2002V197.04H13.6602V194.58Z" fill="white"></path>
                      <path d="M256.09 194.58H253.63V197.04H256.09V194.58Z" fill="white"></path>
                      <path d="M234.8 194.58H232.4V216.04H234.8V194.58Z" fill="white"></path>
                      <path d="M157.45 194.58H154.99V197.04H157.45V194.58Z" fill="white"></path>
                      <path d="M42.5802 206.63C42.5802 205.69 42.4602 204.82 42.2002 204C41.9502 203.18 41.5702 202.49 41.0502 201.91C40.4102 201.21 39.7002 200.69 38.9102 200.34C38.1202 199.99 37.1602 199.82 36.0202 199.82H34.2502V202.21H36.3502C36.9302 202.21 37.4802 202.34 38.0002 202.58C38.5202 202.83 38.9502 203.15 39.2902 203.55C39.6302 203.97 39.8602 204.43 39.9902 204.93C40.1202 205.43 40.1902 206.03 40.1902 206.72V213.66H28.9502V216.05H42.5802V206.63Z" fill="white"></path>
                      <path d="M361.6 194.58H359.2V216.04H361.6V194.58Z" fill="white"></path>
                      <path d="M260.29 194.58H257.83V197.04H260.29V194.58Z" fill="white"></path>
                      <path d="M352.22 194.58H349.82V213.65H341.58C342.08 212.55 342.33 211.38 342.33 210.12C342.33 208.89 342.1 207.73 341.66 206.67C341.21 205.6 340.6 204.68 339.82 203.91C339.04 203.13 338.12 202.52 337.05 202.07C335.98 201.62 334.83 201.4 333.59 201.4C332.39 201.4 331.27 201.63 330.23 202.09C329.19 202.55 328.29 203.17 327.52 203.96C326.75 204.75 326.15 205.67 325.71 206.73C325.27 207.79 325.05 208.92 325.05 210.14C325.05 210.78 325.12 211.39 325.25 211.98C325.38 212.57 325.55 213.13 325.77 213.67H318.34C318.84 212.57 319.09 211.4 319.09 210.14C319.09 208.91 318.86 207.75 318.42 206.69C317.97 205.62 317.36 204.7 316.58 203.93C315.8 203.15 314.88 202.54 313.81 202.09C312.74 201.64 311.59 201.42 310.35 201.42C309.15 201.42 308.04 201.65 307 202.11C305.96 202.57 305.06 203.19 304.29 203.98C303.52 204.77 302.92 205.69 302.48 206.75C302.04 207.81 301.82 208.94 301.82 210.16C301.82 210.8 301.89 211.41 302.02 212C302.15 212.59 302.32 213.15 302.54 213.69H294.93V194.62H292.53V213.69H284.89V210.03C284.89 209.11 284.77 208.24 284.53 207.41C284.29 206.58 283.91 205.88 283.39 205.3C282.77 204.6 282.07 204.08 281.29 203.74C280.51 203.4 279.52 203.23 278.32 203.23H267.71L272.22 194.61H269.61L265.01 203.2C265.09 203.72 265.21 204.17 265.39 204.55C265.56 204.93 265.79 205.29 266.09 205.63H278.31C279.07 205.63 279.72 205.74 280.27 205.98C280.82 206.21 281.26 206.52 281.6 206.92C281.94 207.32 282.17 207.78 282.29 208.3C282.41 208.82 282.47 209.4 282.47 210.03V213.69H260.3V199.85H256.52C255.02 199.85 253.66 200.05 252.45 200.46C251.23 200.87 250.19 201.44 249.33 202.16C248.47 202.89 247.81 203.75 247.34 204.75C246.87 205.75 246.64 206.82 246.64 207.98C246.64 209.4 246.91 210.61 247.45 211.63C247.99 212.65 248.71 213.48 249.61 214.14C250.51 214.8 251.56 215.29 252.76 215.6C253.96 215.92 255.22 216.08 256.53 216.08H304.08C304.86 216.92 305.78 217.58 306.85 218.07C307.92 218.56 309.08 218.8 310.34 218.8C311.62 218.8 312.81 218.56 313.92 218.08C315.03 217.6 315.98 216.93 316.78 216.08H327.33C328.11 216.92 329.03 217.58 330.1 218.07C331.17 218.56 332.33 218.8 333.59 218.8C334.87 218.8 336.06 218.56 337.17 218.08C338.28 217.6 339.23 216.93 340.03 216.08H352.22V194.58ZM257.9 213.65H255.53C254.63 213.65 253.79 213.52 253 213.26C252.21 213 251.52 212.63 250.93 212.14C250.34 211.65 249.88 211.05 249.54 210.35C249.2 209.64 249.03 208.84 249.03 207.94C249.03 207.08 249.21 206.3 249.58 205.59C249.95 204.88 250.44 204.28 251.05 203.77C251.66 203.26 252.35 202.87 253.13 202.6C253.91 202.33 254.71 202.2 255.53 202.2H257.9V213.65ZM316.19 212.59C315.86 213.36 315.41 214.02 314.84 214.58C314.27 215.14 313.6 215.57 312.83 215.88C312.06 216.19 311.23 216.34 310.33 216.34C309.45 216.34 308.64 216.17 307.89 215.85C307.14 215.52 306.49 215.07 305.94 214.5C305.39 213.93 304.96 213.27 304.65 212.51C304.34 211.75 304.19 210.94 304.19 210.09C304.19 209.23 304.35 208.42 304.65 207.65C304.96 206.88 305.39 206.22 305.93 205.65C306.47 205.08 307.11 204.63 307.86 204.29C308.61 203.95 309.43 203.78 310.33 203.78C311.25 203.78 312.09 203.94 312.86 204.26C313.63 204.58 314.3 205.02 314.87 205.58C315.44 206.14 315.88 206.8 316.2 207.57C316.52 208.34 316.68 209.18 316.68 210.1C316.69 210.99 316.52 211.82 316.19 212.59ZM339.44 212.59C339.11 213.36 338.66 214.02 338.09 214.58C337.52 215.14 336.85 215.57 336.08 215.88C335.31 216.19 334.48 216.34 333.58 216.34C332.7 216.34 331.89 216.17 331.14 215.85C330.39 215.52 329.74 215.07 329.19 214.5C328.64 213.93 328.21 213.27 327.9 212.51C327.59 211.75 327.43 210.94 327.43 210.09C327.43 209.23 327.59 208.42 327.9 207.65C328.21 206.88 328.63 206.22 329.17 205.65C329.71 205.08 330.35 204.63 331.1 204.29C331.85 203.95 332.67 203.78 333.57 203.78C334.49 203.78 335.33 203.94 336.1 204.26C336.87 204.58 337.54 205.02 338.11 205.58C338.68 206.14 339.12 206.8 339.44 207.57C339.76 208.34 339.92 209.18 339.92 210.1C339.94 210.99 339.77 211.82 339.44 212.59Z" fill="white"></path>
                      <path d="M6.12 215.56C7.32 215.88 8.58 216.04 9.89 216.04H25.59V199.8H23.19V213.64H13.66V199.8H9.88C8.38 199.8 7.03 200 5.81 200.41C4.59 200.82 3.55 201.39 2.69 202.11C1.83 202.84 1.17 203.7 0.699997 204.7C0.229997 205.7 0 206.77 0 207.93C0 209.35 0.269998 210.56 0.809998 211.58C1.35 212.6 2.07 213.43 2.97 214.09C3.87 214.76 4.92 215.25 6.12 215.56ZM2.4 207.94C2.4 207.08 2.59 206.3 2.96 205.59C3.33 204.88 3.82 204.28 4.43 203.77C5.04 203.26 5.73 202.87 6.51 202.6C7.29 202.33 8.09 202.2 8.91 202.2H11.28V213.65H8.91C8.01 213.65 7.17 213.52 6.38 213.26C5.59 213 4.9 212.63 4.31 212.14C3.72 211.65 3.26 211.05 2.92 210.35C2.57 209.64 2.4 208.84 2.4 207.94Z" fill="white"></path>
                      <path d="M176.3 218.88H173.84V221.34H176.3V218.88Z" fill="white"></path>
                      <path d="M172.1 218.88H169.64V221.34H172.1V218.88Z" fill="white"></path>
                      <path d="M194.36 219.08C194.51 218.33 194.59 217.49 194.59 216.55V216.04H225.42V194.58H223.02V213.65H212.83L217.02 203.63C216.38 203.07 215.7 202.55 214.98 202.06C214.26 201.57 213.52 201.14 212.76 200.77C212 200.4 211.23 200.11 210.45 199.9C209.67 199.69 208.9 199.59 208.14 199.59C207.36 199.59 206.58 199.7 205.8 199.9C205.02 200.11 204.25 200.39 203.48 200.75C202.71 201.11 201.97 201.54 201.26 202.04C200.55 202.54 199.89 203.06 199.27 203.62L203.49 213.64H194.56V199.8H192.16V216.55C192.16 217.23 192.13 217.8 192.06 218.27C191.99 218.74 191.87 219.12 191.7 219.41C191.53 219.7 191.3 219.91 191.01 220.04C190.72 220.17 190.35 220.24 189.89 220.24C189.67 220.24 189.48 220.23 189.32 220.21C189.16 220.19 188.94 220.14 188.66 220.06L188.03 222.36C188.69 222.54 189.34 222.63 189.98 222.63C190.84 222.63 191.56 222.49 192.15 222.21C192.74 221.93 193.21 221.53 193.57 221C193.96 220.47 194.21 219.83 194.36 219.08ZM202.29 204.47C203.21 203.69 204.15 203.12 205.12 202.77C206.09 202.41 207.1 202.23 208.16 202.23C209.24 202.23 210.31 202.44 211.37 202.87C212.43 203.3 213.33 203.83 214.07 204.47L210.33 213.65H206.05L202.29 204.47Z" fill="white"></path>
                      <path d="M9.46 194.58H7V197.04H9.46V194.58Z" fill="white"></path>
                      <path d="M17.2001 231.76H13.5101L5.3601 240.36V231.76H2.3501V248.85H5.3601V243.9L8.0301 241.16L13.8701 248.85H17.5201L10.0801 239.11L17.2001 231.76Z" fill="white"></path>
                      <path d="M23.2698 231.76H20.2598V248.85H23.2698V231.76Z" fill="white"></path>
                      <path d="M39.8599 243.58L30.6899 231.76H27.8999V248.85H30.8599V236.69L40.2999 248.85H42.8199V231.76H39.8599V243.58Z" fill="white"></path>
                      <path d="M55.2798 241.92H59.6598V245.12C58.6098 245.9 57.1398 246.41 55.5298 246.41C52.0298 246.41 49.7098 243.82 49.7098 240.31V240.26C49.7098 236.99 52.0998 234.23 55.2598 234.23C57.4398 234.23 58.7298 234.94 60.0498 236.06L61.9598 233.79C60.1998 232.3 58.3598 231.47 55.3798 231.47C50.2198 231.47 46.5498 235.52 46.5498 240.31V240.36C46.5498 245.34 50.0698 249.15 55.4298 249.15C58.4598 249.15 60.8898 247.93 62.5698 246.49V239.31H55.2798V241.92Z" fill="white"></path>
                      <path d="M73.1002 231.76H66.7202V248.85H73.1002C78.4802 248.85 82.2002 245.12 82.2002 240.3V240.25C82.2002 235.45 78.4802 231.76 73.1002 231.76ZM79.0502 240.36C79.0502 243.7 76.7002 246.12 73.1102 246.12H69.7402V234.5H73.1102C76.7102 234.5 79.0502 236.97 79.0502 240.31V240.36Z" fill="white"></path>
                      <path d="M93.9998 231.47C88.7398 231.47 85.0298 235.5 85.0298 240.31V240.36C85.0298 245.17 88.6998 249.15 93.9598 249.15C99.2198 249.15 102.94 245.12 102.94 240.31V240.26C102.92 235.45 99.2498 231.47 93.9998 231.47ZM99.7698 240.36C99.7698 243.68 97.3998 246.39 93.9998 246.39C90.5998 246.39 88.1798 243.63 88.1798 240.31V240.26C88.1798 236.94 90.5498 234.23 93.9498 234.23C97.3498 234.23 99.7698 236.99 99.7698 240.31V240.36Z" fill="white"></path>
                      <path d="M115.07 239.84L109.86 231.76H106.66V248.85H109.62V236.65L114.98 244.65H115.07L120.48 236.6V248.85H123.49V231.76H120.28L115.07 239.84Z" fill="white"></path>
                      <path d="M143.45 231.47C138.19 231.47 134.48 235.5 134.48 240.31V240.36C134.48 245.17 138.15 249.15 143.41 249.15C148.67 249.15 152.39 245.12 152.39 240.31V240.26C152.38 235.45 148.71 231.47 143.45 231.47ZM149.22 240.36C149.22 243.68 146.85 246.39 143.45 246.39C140.05 246.39 137.63 243.63 137.63 240.31V240.26C137.63 236.94 140 234.23 143.4 234.23C146.8 234.23 149.22 236.99 149.22 240.31V240.36Z" fill="white"></path>
                      <path d="M156.12 248.85H159.13V241.89H167.76V239.16H159.13V234.5H168.86V231.76H156.12V248.85Z" fill="white"></path>
                      <path d="M186.11 238.94C182.83 238.16 182.05 237.57 182.05 236.26V236.21C182.05 235.09 183.08 234.18 184.84 234.18C186.41 234.18 187.95 234.79 189.49 235.94L191.1 233.67C189.36 232.28 187.38 231.52 184.89 231.52C181.49 231.52 179.04 233.55 179.04 236.47V236.52C179.04 239.67 181.1 240.74 184.74 241.62C187.92 242.35 188.6 243.01 188.6 244.23V244.29C188.6 245.59 187.43 246.44 185.57 246.44C183.44 246.44 181.8 245.63 180.16 244.22L178.35 246.37C180.41 248.2 182.87 249.1 185.49 249.1C189.09 249.1 191.6 247.2 191.6 244V243.95C191.61 241.11 189.73 239.82 186.11 238.94Z" fill="white"></path>
                      <path d="M200.83 231.64L193.3 248.85H196.38L198.14 244.72H206.24L207.98 248.85H211.16L203.63 231.64H200.83ZM199.24 242.07L202.18 235.23L205.14 242.07H199.24Z" fill="white"></path>
                      <path d="M224.56 241.6C224.56 244.75 222.92 246.36 220.25 246.36C217.56 246.36 215.92 244.65 215.92 241.48V231.76H212.91V241.6C212.91 246.55 215.75 249.12 220.2 249.12C224.68 249.12 227.56 246.56 227.56 241.46V231.77H224.55V241.6H224.56Z" fill="white"></path>
                      <path d="M238.36 231.76H231.98V248.85H238.36C243.74 248.85 247.46 245.12 247.46 240.3V240.25C247.46 235.45 243.74 231.76 238.36 231.76ZM244.3 240.36C244.3 243.7 241.95 246.12 238.36 246.12H234.99V234.5H238.36C241.96 234.5 244.3 236.97 244.3 240.31V240.36Z" fill="white"></path>
                      <path d="M254.28 231.76H251.27V248.85H254.28V231.76Z" fill="white"></path>
                      <path d="M272.25 231.64L264.72 248.85H267.8L269.56 244.72H277.66L279.4 248.85H282.58L275.05 231.64H272.25ZM270.66 242.07L273.6 235.23L276.56 242.07H270.66Z" fill="white"></path>
                      <path d="M299.41 237.21V237.16C299.41 235.67 298.9 234.4 297.99 233.47C296.89 232.4 295.2 231.76 293.05 231.76H285.42V248.85H288.43V242.89H292.15H292.2L296.41 248.85H299.96L295.34 242.38C297.72 241.7 299.41 240.01 299.41 237.21ZM288.43 240.23V234.49H292.81C295.04 234.49 296.36 235.49 296.36 237.32V237.37C296.36 239.1 294.99 240.23 292.84 240.23H288.43Z" fill="white"></path>
                      <path d="M309.69 231.64L302.16 248.85H305.24L307 244.72H315.1L316.84 248.85H320.02L312.49 231.64H309.69ZM308.1 242.07L311.04 235.23L314 242.07H308.1Z" fill="white"></path>
                      <path d="M333.71 239.97C334.98 239.31 336.16 238.21 336.16 236.16V236.11C336.16 234.96 335.77 234.04 335.01 233.28C334.03 232.3 332.49 231.77 330.53 231.77H322.85V248.86H330.77C334.59 248.86 337.1 247.23 337.1 244.17V244.12C337.11 241.8 335.72 240.65 333.71 239.97ZM325.81 234.42H330.14C332.07 234.42 333.15 235.25 333.15 236.59V236.64C333.15 238.2 331.85 238.96 329.92 238.96H325.81V234.42ZM334.1 243.85C334.1 245.39 332.85 246.19 330.8 246.19H325.81V241.5H330.58C332.95 241.5 334.1 242.36 334.1 243.8V243.85Z" fill="white"></path>
                      <path d="M343.85 231.76H340.84V248.85H343.85V231.76Z" fill="white"></path>
                      <path d="M357.04 231.64H354.25L346.72 248.85H349.8L351.56 244.72H359.66L361.4 248.85H364.58L357.04 231.64ZM352.66 242.07L355.6 235.23L358.56 242.07H352.66Z" fill="white"></path>
                    </svg>
                  </a>
                    <a href="/ar">
                      <svg xmlns="http://www.w3.org/2000/svg" class="text-white" height="42" viewBox="0 0 1104 377"
                        fill="none">
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M143.898 146.2C140.232 142.467 138.398 138.067 138.398 133V127.5C138.198 122.433 139.932 118.067 143.598 114.4C147.265 110.733 151.732 108.8 156.998 108.6L177.298 108.4V151.3L156.998 151.6C151.865 151.733 147.498 149.933 143.898 146.2ZM147.198 133C147.198 135.867 148.198 138.367 150.198 140.5C152.198 142.633 154.532 143.7 157.198 143.7H168.398L168.198 116.8L157.198 117C154.332 117 151.965 117.967 150.098 119.9C148.165 121.967 147.198 124.5 147.198 127.5V133ZM151.698 98.9V90.8C151.898 90.6 152.065 90.5 152.198 90.5H160.298C160.498 90.5 160.665 90.6 160.798 90.8V99.8H152.198C151.865 99.8 151.698 99.5 151.698 98.9Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M164.602 98.9V90.8C164.735 90.6 164.902 90.5 165.102 90.5H173.402L173.702 90.8V99.8H165.102C164.768 99.8 164.602 99.5 164.602 98.9ZM249.802 169.2C249.468 169.2 249.302 168.9 249.302 168.3V160.2C249.502 160 249.668 159.9 249.802 159.9H258.202L258.402 160.2V169.2H249.802Z"
                          fill="currentColor" />
                        <path
                          d="M245 169.202H245.5V168.302V160.202C245.367 160.002 245.2 159.902 245 159.902H236.9C236.767 159.902 236.633 160.002 236.5 160.202V168.302C236.5 168.902 236.633 169.202 236.9 169.202H245Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M314 118.401C314.133 123.001 314.2 129.934 314.2 139.201C314.2 142.867 313.267 145.801 311.4 148.001C309.467 150.201 306.733 151.301 303.2 151.301H288C284.8 151.301 282.167 150.267 280.1 148.201C277.1 150.267 273.833 151.301 270.3 151.301H200C200 158.301 198.333 163.567 195 167.101C191.667 170.567 186.967 172.301 180.9 172.301C177.767 172.301 175.3 172.067 173.5 171.601L176.6 164.201H178.3C186.9 164.201 191.2 159.834 191.2 151.101V115.301C191.2 114.234 191.767 113.301 192.9 112.501L200 110.101V143.501H243.8L243.6 114.601C243.6 113.334 244.333 112.367 245.8 111.701L253.2 108.601V143.501H270.3C272.033 143.501 273.3 143.301 274.1 142.901C274.9 142.501 275.867 141.567 277 140.101V139.201L276.8 128.201C276.6 123.134 278.3 118.767 281.9 115.101C285.5 111.434 289.833 109.601 294.9 109.601H304.7C307.233 109.601 309.367 110.434 311.1 112.101C312.833 113.767 313.8 115.867 314 118.401ZM306.1 139.201L305.6 119.401C305.333 118.267 304.533 117.701 303.2 117.701H295.4C292.533 117.701 290.067 118.734 288 120.801C285.933 122.867 284.9 125.334 284.9 128.201V139.201C284.9 142.067 285.833 143.501 287.7 143.501H303.2C305.133 143.501 306.1 142.067 306.1 139.201ZM356.8 151.301H323.4V143.701H344.1L332.2 92.9008H341.7L353.7 143.701H355.8C362 143.701 365.1 140.134 365.1 133.001V92.3008C365.1 90.8341 365.667 89.8675 366.8 89.4008L374.4 86.3008V134.201C374.4 139.401 372.733 143.601 369.4 146.801C366.067 149.801 361.867 151.301 356.8 151.301Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M328.898 83.4008L332.398 82.2008C330.198 80.4008 329.098 78.3341 329.098 76.0008C329.098 73.9341 329.965 72.2008 331.698 70.8008C333.498 69.4674 335.798 68.8008 338.598 68.8008C339.932 68.8008 340.998 68.8675 341.798 69.0008L341.498 74.3008C340.898 74.1674 340.032 74.1008 338.898 74.1008C336.032 74.1008 334.598 74.8008 334.598 76.2008C334.598 77.8008 335.865 79.1674 338.398 80.3008L344.598 78.1008L345.998 83.4008L330.798 88.1008L328.898 83.4008ZM392.198 151.401H382.898V92.3008C382.898 91.0341 383.598 90.0675 384.998 89.4008L392.198 86.3008V151.401ZM429.298 98.9008V90.8008C429.432 90.6674 429.598 90.6008 429.798 90.6008H437.898C438.032 90.6008 438.198 90.6674 438.398 90.8008V99.9008H429.798C429.465 99.9008 429.298 99.5675 429.298 98.9008ZM442.198 98.9008V90.8008C442.332 90.6674 442.498 90.6008 442.698 90.6008H450.998L451.298 90.8008V99.9008H442.698C442.365 99.9008 442.198 99.5675 442.198 98.9008Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M562.001 151.303C558.801 151.303 556.167 150.27 554.101 148.203C551.101 150.27 547.834 151.303 544.301 151.303H456.501L436.201 151.603C431.134 151.736 426.767 149.936 423.101 146.203C419.434 142.47 417.601 138.07 417.601 133.003V127.503C417.467 122.436 419.234 118.07 422.901 114.403C426.501 110.736 430.934 108.803 436.201 108.603L456.501 108.403V143.503H495.801L488.401 120.103L491.301 110.803L501.601 108.403C505.867 107.27 509.834 106.703 513.501 106.703C519.234 106.703 523.367 108.37 525.901 111.703C527.967 114.436 529.001 118.603 529.001 124.203C529.001 129.603 527.801 136.036 525.401 143.503H544.301C546.101 143.503 547.401 143.303 548.201 142.903C548.934 142.503 549.867 141.57 551.001 140.103V139.203L550.801 128.203C550.601 123.136 552.301 118.77 555.901 115.103C559.501 111.436 563.834 109.603 568.901 109.603H578.701C581.234 109.603 583.367 110.436 585.101 112.103C586.901 113.77 587.867 115.87 588.001 118.403C588.134 123.07 588.201 130.003 588.201 139.203C588.201 142.87 587.267 145.803 585.401 148.003C583.467 150.203 580.767 151.303 577.301 151.303H562.001ZM520.401 124.203C520.401 120.803 519.767 118.47 518.501 117.203C516.901 115.67 514.601 114.87 511.601 114.803C510.134 114.803 508.301 115.07 506.101 115.603C504.967 115.603 503.634 115.803 502.101 116.203C500.634 116.603 499.334 117.036 498.201 117.503L497.001 120.603L504.601 143.503H510.801C513.067 143.17 514.701 142.47 515.701 141.403C516.767 140.403 517.767 138.236 518.701 134.903C519.834 131.103 520.401 127.536 520.401 124.203ZM447.601 143.503L447.401 116.803L436.401 117.003C433.534 117.003 431.167 117.97 429.301 119.903C427.367 121.97 426.401 124.503 426.401 127.503V133.003C426.401 135.87 427.401 138.37 429.401 140.503C431.401 142.636 433.734 143.703 436.401 143.703H445.001V143.503H447.601ZM561.701 143.503H577.201C579.134 143.503 580.101 142.07 580.101 139.203L579.601 119.403C579.267 118.27 578.467 117.703 577.201 117.703H569.301C566.434 117.703 563.967 118.736 561.901 120.803C559.834 122.87 558.801 125.336 558.801 128.203V139.203C558.801 142.07 559.767 143.503 561.701 143.503ZM661.801 157.303C661.934 157.303 662.001 157.37 662.001 157.503V165.903C662.001 166.236 661.834 166.403 661.501 166.403H653.401C652.867 166.403 652.601 166.236 652.601 165.903V157.503C652.601 157.37 652.867 157.303 653.401 157.303H661.801Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M600.805 151.303V92.2031C600.805 90.9365 601.505 89.9698 602.905 89.3031L610.105 86.2031V143.503H661.805L661.505 126.803C661.505 123.603 660.638 121.036 658.905 119.103C657.171 117.236 654.871 116.303 652.005 116.303C649.138 116.303 645.005 116.936 639.605 118.203L638.205 110.803C640.271 110.136 642.671 109.536 645.405 109.003C648.205 108.47 650.338 108.203 651.805 108.203C657.005 108.203 661.271 109.936 664.605 113.403C667.938 116.936 669.705 121.403 669.905 126.803V143.503H673.005L673.905 151.303H600.805ZM29.1047 208.103C29.1714 208.103 29.2047 208.17 29.2047 208.303V216.803C29.2047 217.136 29.038 217.303 28.7047 217.303H20.5047C20.038 217.303 19.8047 217.136 19.8047 216.803V208.303C19.8047 208.17 20.038 208.103 20.5047 208.103H29.1047Z"
                          fill="currentColor" />
                        <path
                          d="M159.802 244.002H156.602V227.002C156.468 221.535 154.702 217.002 151.302 213.402C147.902 209.868 143.535 208.102 138.202 208.102C136.735 208.102 134.602 208.368 131.802 208.902C128.935 209.502 126.468 210.135 124.402 210.802L125.902 218.302C131.368 216.968 135.568 216.302 138.502 216.302C141.368 216.302 143.702 217.268 145.502 219.202C147.235 221.135 148.102 223.735 148.102 227.002L148.402 244.002H123.702H123.202H116.902C115.102 244.002 113.802 243.768 113.002 243.302C112.202 242.902 111.302 242.068 110.302 240.802V239.602C110.302 230.202 110.235 223.168 110.102 218.502C109.902 215.902 108.935 213.768 107.202 212.102C105.402 210.435 103.202 209.602 100.602 209.602H90.7016C85.5682 209.602 81.1682 211.435 77.5016 215.102C73.9016 218.835 72.1682 223.268 72.3015 228.402L72.5016 239.602V240.602C71.3682 242.002 70.4016 242.902 69.6016 243.302C68.8016 243.768 67.5349 244.002 65.8016 244.002H63.1016H59.0016H52.4016V234.302V223.802L45.1016 227.002C43.5016 227.468 42.7016 228.435 42.7016 229.902L43.0016 239.402V249.502C43.0016 254.235 42.8016 257.768 42.4016 260.102C42.0016 262.435 41.0682 264.335 39.6016 265.802C38.1349 267.202 36.1016 268.268 33.5016 269.002C30.9016 269.735 28.1682 270.102 25.3016 270.102C22.3682 270.102 19.6682 269.702 17.2016 268.902C14.6682 268.102 12.9349 266.968 12.0016 265.502C9.86822 262.902 8.80156 258.802 8.80156 253.202C8.80156 246.402 10.1682 239.268 12.9016 231.802L4.20155 231.602C1.46822 238.868 0.101562 245.968 0.101562 252.902C0.101562 260.835 1.86822 266.902 5.40155 271.102C7.33488 273.368 10.1349 275.102 13.8016 276.302C17.4016 277.502 21.4016 278.102 25.8016 278.102C30.0016 278.102 33.9349 277.535 37.6016 276.402C41.3349 275.268 44.1682 273.668 46.1016 271.602C48.2349 269.668 49.7349 267.202 50.6016 264.202C51.4682 261.202 52.0682 257.135 52.4016 252.002H59.0016H63.1016H65.8016C69.3349 252.002 72.6349 250.935 75.7016 248.802C77.7682 250.935 80.4349 252.002 83.7016 252.002H99.2016C102.402 252.002 105.068 250.935 107.202 248.802C109.935 250.935 113.168 252.002 116.902 252.002H122.702H123.702H160.702L159.802 244.002ZM99.3015 244.002H83.5016C81.5682 244.002 80.6016 242.535 80.6016 239.602V228.402C80.6016 225.535 81.6682 223.035 83.8015 220.902C85.8682 218.835 88.3682 217.802 91.3015 217.802H99.3015C100.568 217.802 101.368 218.368 101.702 219.502L102.202 239.602C102.202 242.535 101.235 244.002 99.3015 244.002Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M198.905 189.001L206.205 185.801V252.001H181.705C181.705 259.067 180.005 264.367 176.605 267.901C173.205 271.501 168.438 273.301 162.305 273.301C159.105 273.301 156.605 273.034 154.805 272.501L157.905 265.001H159.605C168.338 265.001 172.705 260.567 172.705 251.701V215.401C172.705 214.267 173.271 213.301 174.405 212.501L181.705 210.001V244.001H196.705V191.901C196.705 190.567 197.438 189.601 198.905 189.001ZM217.205 189.001L224.405 185.801V252.001H215.005V191.901C215.005 190.567 215.738 189.601 217.205 189.001ZM300.005 260.701C300.071 260.701 300.105 260.767 300.105 260.901V269.401C300.105 269.734 299.971 269.901 299.705 269.901H291.405C290.938 269.901 290.705 269.734 290.705 269.401V260.901C290.705 260.767 290.938 260.701 291.405 260.701H300.005Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M337.605 243.702C339.938 243.902 344.805 244.002 352.205 244.002V252.002H233.805V243.702H265.205L264.805 228.702C264.605 224.836 263.505 221.802 261.505 219.602C259.505 217.402 256.805 216.302 253.405 216.302C249.338 216.302 244.905 217.769 240.105 220.702L237.905 213.202C243.505 209.669 249.071 207.902 254.605 207.902C260.271 207.902 264.905 209.869 268.505 213.802C272.171 217.736 274.071 223.036 274.205 229.702V244.002H291.705L291.405 214.602C291.405 213.336 292.138 212.369 293.605 211.702L301.105 208.602V244.002H321.205C318.938 235.736 317.805 229.002 317.805 223.802C317.805 218.536 319.071 214.602 321.605 212.002C324.138 209.402 328.038 208.102 333.305 208.102C337.505 208.102 342.538 208.836 348.405 210.302L346.905 218.802C341.571 217.336 337.371 216.602 334.305 216.602C328.971 216.602 326.305 218.936 326.305 223.602C326.305 226.336 327.205 230.369 329.005 235.702C329.938 238.769 331.038 240.836 332.305 241.902C333.505 242.969 335.271 243.569 337.605 243.702ZM417.305 200.802H408.805C408.671 200.802 408.605 200.569 408.605 200.102V191.502C408.605 191.436 408.671 191.402 408.805 191.402H417.305C417.638 191.402 417.805 191.569 417.805 191.902V200.102C417.805 200.569 417.638 200.802 417.305 200.802Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M395.703 200.802C395.37 200.802 395.203 200.569 395.203 200.102V191.902C395.203 191.569 395.37 191.402 395.703 191.402H404.203C404.336 191.402 404.403 191.436 404.403 191.502V200.102C404.403 200.569 404.336 200.802 404.203 200.802H395.703ZM461.103 201.302C460.636 201.302 460.403 201.136 460.403 200.802V192.302C460.403 192.169 460.636 192.102 461.103 192.102H469.703C469.77 192.102 469.803 192.169 469.803 192.302V200.802C469.803 201.136 469.67 201.302 469.403 201.302H461.103ZM496.303 260.202C496.37 260.202 496.403 260.269 496.403 260.402V268.902C496.403 269.236 496.27 269.402 496.003 269.402H487.703C487.236 269.402 487.003 269.236 487.003 268.902V260.402C487.003 260.269 487.236 260.202 487.703 260.202H496.303Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M494.1 211.701L501.6 208.601V252.001H392.8C379.267 252.001 372.5 245.767 372.5 233.301C372.5 229.434 373.067 225.234 374.2 220.701H381.9C381.433 224.234 381.2 227.467 381.2 230.401C381.2 235.267 382.167 238.734 384.1 240.801C386.033 242.934 389.267 244.001 393.8 244.001H431.1L430.9 214.601C430.9 213.334 431.6 212.367 433 211.701L440.5 208.601V244.001H461.6L461.4 214.601C461.4 213.334 462.133 212.367 463.6 211.701L471.1 208.601V244.001H492.2L491.9 214.601C491.9 213.334 492.633 212.367 494.1 211.701ZM563.1 252.001L542.5 252.201C537.367 252.334 532.933 250.501 529.2 246.701C525.467 242.967 523.6 238.501 523.6 233.301V227.701C523.467 222.567 525.267 218.134 529 214.401C532.667 210.667 537.167 208.734 542.5 208.601L563.1 208.301V252.001ZM554.2 244.301L553.9 216.901L542.8 217.101C539.867 217.101 537.433 218.101 535.5 220.101C533.567 222.167 532.6 224.734 532.6 227.801V233.401C532.6 236.267 533.6 238.801 535.6 241.001C537.667 243.201 540.067 244.301 542.8 244.301H554.2ZM550.8 190.301H559.3L559.5 190.501V199.701H550.8C550.467 199.701 550.3 199.367 550.3 198.701V190.501C550.433 190.367 550.6 190.301 550.8 190.301Z"
                          fill="currentColor" />
                        <path fill-rule="evenodd" clip-rule="evenodd"
                          d="M537.203 190.403V198.603C537.203 199.27 537.37 199.603 537.703 199.603H546.403V190.403C546.27 190.27 546.103 190.203 545.903 190.203H537.703C537.57 190.203 537.403 190.27 537.203 190.403ZM577.303 215.403V251.703C577.303 260.57 572.936 265.003 564.203 265.003H562.503L559.403 272.503C561.136 273.036 563.636 273.303 566.903 273.303C573.036 273.303 577.803 271.503 581.203 267.903C584.603 264.37 586.303 259.07 586.303 252.003V210.003L579.003 212.503C577.87 213.303 577.303 214.27 577.303 215.403ZM662.003 214.603L662.303 244.003H633.403V209.103L615.403 208.803C609.47 208.803 604.636 211.07 600.903 215.603C597.636 219.67 596.003 224.67 596.003 230.603C596.003 236.47 597.636 241.403 600.903 245.403C604.303 249.803 609.136 252.003 615.403 252.003H624.403C624.403 256.003 623.336 259.17 621.203 261.503C619.136 263.836 615.836 265.003 611.303 265.003H609.603L606.503 272.503C608.236 273.036 610.736 273.303 614.003 273.303C620.136 273.303 624.903 271.503 628.303 267.903C631.703 264.37 633.403 259.136 633.403 252.203V252.003H671.703V208.603L664.203 211.703C662.736 212.37 662.003 213.336 662.003 214.603ZM624.403 244.003H615.403C612.203 244.003 609.636 242.703 607.703 240.103C605.77 237.503 604.803 234.336 604.803 230.603C604.803 227.536 605.77 224.47 607.703 221.403C609.436 218.67 612.003 217.303 615.403 217.303H624.403V244.003Z"
                          fill="currentColor" />
                        <path
                          d="M660.302 192.102C659.835 192.102 659.602 192.168 659.602 192.302V200.802C659.602 201.135 659.835 201.302 660.302 201.302H668.602C668.935 201.302 669.102 201.135 669.102 200.802V192.302C669.102 192.168 669.035 192.102 668.902 192.102H668.602H660.302Z"
                          fill="currentColor" />
                        <path
                          d="M916.002 0.601562C812.302 0.601562 728.102 84.7016 728.102 188.502C728.102 292.202 812.302 376.402 916.002 376.402C1019.8 376.402 1103.9 292.202 1103.9 188.502C1103.9 84.7016 1019.8 0.601562 916.002 0.601562ZM1040.8 261.302H1011V215.002C1011 182.902 990.102 155.502 960.702 144.902C960.702 144.902 960.668 144.902 960.602 144.902C960.268 144.768 959.935 144.635 959.602 144.502C959.268 144.368 958.935 144.268 958.602 144.202C955.068 143.002 951.468 142.068 947.802 141.402C959.602 122.802 974.802 107.102 989.202 94.7016C991.402 97.6349 993.502 100.568 995.502 103.502C1002.8 114.302 1009.4 125.602 1015.1 137.302C1020.4 148.202 1024.9 159.502 1028.5 171.102C1032.3 182.702 1035.1 194.702 1037.1 206.702C1039.2 218.902 1040.4 231.302 1040.7 243.702C1040.8 245.202 1040.8 252.502 1040.8 261.302ZM1040.6 299.702V301.802C1034.4 308.502 1027.7 314.802 1020.6 320.502V295.902H979.902V225.502C979.902 197.902 957.502 175.502 929.902 175.502H900.602C873.002 175.502 850.602 197.902 850.602 225.502V295.902H810.302V319.502C803.502 314.102 797.202 308.002 791.302 301.602C791.402 297.502 791.502 285.702 791.702 272.302H831.902L831.802 261.302L831.502 214.902C831.502 179.702 861.302 151.002 898.002 151.002H933.602C970.202 151.002 1000.1 179.702 1000.1 215.002V272.302H1040.7C1040.7 284.602 1040.6 296.902 1040.6 299.702ZM970.102 316.002C969.602 317.602 969.002 319.502 968.402 321.502C967.735 323.568 967.035 325.768 966.302 328.102C965.635 330.435 964.935 332.868 964.202 335.402H960.302C959.568 333.068 958.868 330.868 958.202 328.802C957.468 326.735 956.768 324.768 956.102 322.902C955.368 320.968 954.602 319.102 953.802 317.302C953.068 315.502 952.268 313.668 951.402 311.802L956.002 310.302C956.468 311.368 956.968 312.668 957.502 314.202C958.102 315.702 958.602 317.202 959.202 318.802C959.735 320.468 960.302 322.135 960.902 323.802C961.435 325.468 961.902 327.002 962.302 328.402C963.035 325.268 963.835 322.202 964.702 319.202C965.502 316.302 966.302 313.302 967.202 310.302L971.602 311.802C971.135 312.935 970.635 314.335 970.102 316.002ZM939.702 217.402C940.302 216.402 940.902 215.402 941.802 214.602C942.702 213.802 943.202 213.502 944.202 213.702C945.702 214.102 946.102 215.402 946.602 217.002C946.902 218.002 946.702 219.102 946.302 220.102C945.902 220.902 945.402 221.802 944.802 222.502C944.102 223.102 943.302 223.602 942.402 223.602C941.802 223.602 941.302 223.402 940.702 223.102C940.002 222.802 939.302 222.302 938.902 221.502C938.102 220.202 938.902 218.702 939.702 217.402ZM946.502 250.802C946.102 251.502 946.002 253.302 945.202 255.802C944.702 257.602 943.702 260.502 942.802 262.402C941.402 265.302 937.802 269.602 932.802 271.702C928.402 273.502 923.002 273.602 919.702 273.602C918.402 273.602 914.702 273.702 914.502 271.702C914.402 269.502 915.302 268.302 917.102 267.802C918.702 267.302 921.002 267.202 922.402 267.002C924.202 266.902 925.502 266.802 927.302 266.502C929.302 266.202 930.702 266.102 932.602 265.202C937.802 262.802 940.102 257.302 939.302 254.102C939.202 253.902 938.902 253.002 938.602 252.802C936.102 251.902 933.202 249.102 932.902 244.902C932.702 242.502 936.002 236.902 943.302 240.602C945.402 241.702 946.002 243.302 946.102 243.302C947.002 243.802 951.502 242.502 951.402 241.402C951.402 239.902 951.602 238.702 951.602 237.202C951.702 236.602 951.602 236.002 951.702 235.202C951.702 233.302 951.502 229.102 951.702 228.602C952.402 227.002 955.302 224.102 958.002 225.402C961.502 227.102 959.602 231.602 959.102 234.802C959.002 235.202 958.702 236.402 958.602 236.902C958.402 238.502 958.102 239.002 957.702 241.002C957.502 242.402 956.602 244.402 955.702 245.602C954.202 247.602 947.302 249.702 946.502 250.802ZM947.602 315.402C946.735 315.268 945.868 315.202 945.002 315.202C944.135 315.135 943.268 315.102 942.402 315.102C940.702 315.102 939.102 315.202 937.702 315.302C937.968 316.502 938.235 317.768 938.502 319.102C938.768 320.435 938.968 321.768 939.102 323.102C939.302 324.435 939.435 325.768 939.502 327.102C939.635 328.368 939.702 329.602 939.702 330.802C939.702 332.468 939.602 334.002 939.402 335.402H935.802C935.702 331.502 935.402 327.602 934.802 323.602C934.135 319.602 933.268 315.868 932.202 312.402L935.102 310.802C936.235 310.602 937.368 310.468 938.502 310.402C939.635 310.335 940.802 310.302 942.002 310.302C943.002 310.302 944.035 310.335 945.102 310.402C946.168 310.468 947.202 310.568 948.202 310.702L947.602 315.402ZM924.902 335.302C923.968 335.435 923.035 335.535 922.102 335.602C921.168 335.735 920.268 335.768 919.402 335.702C916.468 335.702 914.235 335.202 912.702 334.202C911.235 333.135 910.502 331.602 910.502 329.602C910.502 328.402 910.835 327.235 911.502 326.102C912.168 324.968 913.102 324.068 914.302 323.402C912.968 323.068 911.935 322.502 911.202 321.702C910.468 320.902 910.102 319.935 910.102 318.802C910.102 317.802 910.402 316.802 911.102 315.902C911.768 314.968 912.668 314.102 913.802 313.302C914.935 312.568 916.268 311.935 917.802 311.402C919.335 310.868 921.002 310.502 922.802 310.302L921.802 314.802C920.735 315.002 919.735 315.235 918.802 315.502C917.868 315.768 917.068 316.102 916.402 316.502C915.702 316.902 915.102 317.202 914.702 317.602C914.302 318.102 914.102 318.402 914.102 318.902C914.102 319.502 914.568 320.002 915.502 320.402C916.435 320.735 917.768 320.902 919.502 320.902C920.035 320.902 920.502 320.902 920.902 320.902C921.302 320.902 921.602 320.868 921.802 320.802L921.402 324.702C919.202 324.968 917.502 325.468 916.302 326.202C915.102 326.868 914.502 327.735 914.502 328.802C914.502 329.535 915.002 330.102 916.002 330.502C916.935 330.902 918.335 331.102 920.202 331.102C921.068 331.102 922.035 331.068 923.102 331.002C924.102 330.935 925.002 330.802 925.802 330.602L924.902 335.302ZM895.902 268.502C893.802 270.702 891.602 272.902 889.402 275.102C888.302 276.202 887.102 277.302 885.602 277.802C884.102 278.302 882.202 278.002 881.402 276.702C880.602 275.602 880.802 274.102 881.502 272.902C882.102 271.702 883.202 270.902 884.202 270.002C893.502 262.002 901.902 252.902 909.502 243.302C911.102 241.402 912.602 239.402 914.402 237.702C916.202 236.102 918.402 234.702 920.802 234.302C922.002 234.102 923.602 234.202 924.202 235.402C924.902 236.702 923.702 238.102 922.902 239.102C915.602 247.802 907.802 256.102 899.902 264.302C898.568 265.702 897.235 267.102 895.902 268.502ZM898.602 335.502H895.002C894.902 333.502 894.802 331.502 894.602 329.502C894.402 327.502 894.202 325.502 893.902 323.502C893.568 321.502 893.168 319.535 892.702 317.602C892.235 315.668 891.735 313.835 891.202 312.102L895.802 310.702C896.268 312.035 896.668 313.535 897.002 315.202C897.402 316.868 897.735 318.568 898.002 320.302C898.268 322.102 898.468 323.902 898.602 325.702C898.735 327.502 898.802 329.235 898.802 330.902C898.802 332.568 898.735 334.102 898.602 335.502ZM878.402 244.802C878.602 245.102 879.302 246.602 880.902 247.002C881.902 247.302 882.802 247.102 883.702 246.802C884.402 246.502 885.002 246.102 885.502 245.602C886.202 244.802 887.502 243.602 887.602 242.402C888.002 239.002 888.202 235.002 891.302 232.502C893.302 230.902 895.202 230.102 896.802 231.002C897.502 231.402 898.102 232.202 898.402 233.102C899.502 236.502 895.902 240.402 894.002 242.702C890.902 246.102 887.602 249.802 883.702 252.102C881.202 253.502 877.802 253.002 875.302 252.002C874.102 251.602 872.902 250.902 872.102 249.902C868.402 245.502 871.402 238.102 873.702 233.802C876.702 228.602 885.202 221.402 890.702 218.902C891.802 218.402 893.902 218.102 894.202 218.702C894.502 219.302 893.102 220.602 893.102 220.602C888.002 224.802 879.002 236.002 878.002 241.202C877.902 242.002 877.702 243.402 878.402 244.802ZM882.202 331.602C881.735 332.468 881.168 333.202 880.502 333.802C879.768 334.335 878.968 334.768 878.102 335.102C877.168 335.368 876.235 335.502 875.302 335.502C874.168 335.502 873.068 335.335 872.002 335.002C870.935 334.735 869.902 334.368 868.902 333.902C867.635 334.368 866.335 334.735 865.002 335.002C863.535 335.335 862.202 335.502 861.002 335.502H857.702L857.102 335.102V331.102L857.702 330.702H862.802C863.335 330.702 863.835 330.668 864.302 330.602C863.835 330.002 863.502 329.368 863.302 328.702C863.102 328.002 863.002 327.202 863.002 326.402C863.002 325.468 863.168 324.602 863.502 323.802C863.768 322.935 864.168 322.202 864.702 321.602C865.235 320.935 865.868 320.435 866.602 320.102C867.335 319.702 868.168 319.502 869.102 319.502C869.968 319.502 870.768 319.702 871.502 320.102C872.235 320.435 872.868 320.935 873.402 321.602C873.935 322.202 874.335 322.935 874.602 323.802C874.935 324.602 875.102 325.468 875.102 326.402C875.102 327.302 875.002 328.002 874.702 328.702C874.502 329.435 874.168 330.102 873.702 330.702C873.902 330.702 874.135 330.702 874.402 330.702H875.102C875.902 330.702 876.602 330.635 877.202 330.502C877.735 330.368 878.168 330.202 878.502 330.002C878.802 329.902 879.002 329.702 879.102 329.502C879.235 329.235 879.302 329.035 879.302 328.902C879.302 328.102 879.135 327.168 878.802 326.102C878.468 325.035 877.902 323.968 877.102 322.902C876.302 321.835 875.268 320.802 874.002 319.802C872.802 318.802 871.202 318.002 869.302 317.302L871.102 313.302C873.035 314.102 874.735 315.035 876.202 316.102C877.668 317.235 878.902 318.468 879.902 319.802C880.835 321.135 881.568 322.535 882.102 324.002C882.568 325.468 882.802 326.935 882.802 328.402C882.802 329.668 882.602 330.735 882.202 331.602ZM784.602 216.102C783.468 224.902 782.735 233.768 782.402 242.702C782.402 244.102 782.368 245.502 782.302 246.902C782.302 250.602 782.502 286.202 782.402 286.902V291.002C760.602 262.602 747.602 227.102 747.602 188.602C747.602 139.602 768.502 95.5016 801.902 64.7016C807.702 68.0016 821.402 76.2016 837.002 88.6016C834.535 91.8016 832.168 95.0349 829.902 98.3015C825.168 105.168 820.768 112.202 816.702 119.402C812.635 126.735 808.868 134.235 805.402 141.902C802.002 149.635 798.968 157.535 796.302 165.602C793.568 173.735 791.235 181.968 789.302 190.302C787.302 198.835 785.735 207.435 784.602 216.102ZM915.902 20.1016C914.502 20.9016 909.602 23.7016 907.402 25.1016C899.802 29.7016 892.502 34.6682 885.502 40.0016C878.435 45.4016 871.668 51.1682 865.202 57.3015C858.668 63.4349 852.468 69.9016 846.602 76.7016C845.268 78.2349 843.935 79.8016 842.602 81.4016C841.268 80.3349 839.968 79.3349 838.702 78.4016C826.302 68.8015 815.502 62.1016 809.102 58.4016C838.202 34.5016 875.302 20.2016 915.902 20.1016ZM821.002 261.402H791.802C792.102 243.202 792.502 225.002 793.102 220.602C794.102 211.935 795.535 203.302 797.402 194.702C799.268 186.235 801.568 177.868 804.302 169.602C806.968 161.402 810.068 153.335 813.602 145.402C817.068 137.535 820.968 129.835 825.302 122.302C829.568 114.835 834.202 107.602 839.202 100.602C840.735 98.4682 842.302 96.3682 843.902 94.3015C858.302 106.602 873.702 122.402 885.602 141.202C867.202 144.102 851.102 153.102 839.402 166.102C827.602 179.302 820.502 196.402 820.502 215.102L821.002 261.402ZM916.402 30.1016C932.002 39.1016 946.502 49.9016 959.702 62.2016C968.202 70.1016 976.202 78.6016 983.502 87.6016C968.302 100.702 950.802 118.602 937.802 140.402C936.668 140.402 935.502 140.402 934.302 140.402H896.202C896.068 140.402 895.902 140.402 895.702 140.402C882.602 118.302 864.902 100.302 849.602 87.2016C866.002 67.1016 885.502 49.6016 907.402 35.6016C908.402 34.9016 915.602 29.6016 916.402 30.1016ZM1023.5 59.0016C1017.1 62.7016 1006.7 69.3015 994.802 78.4016C993.402 79.5349 991.935 80.7016 990.402 81.9016C980.702 70.0016 969.902 59.0016 958.202 48.9016C951.335 43.0349 944.202 37.5682 936.802 32.5016C933.068 29.9016 929.268 27.4349 925.402 25.1016C923.202 23.8016 918.402 21.0016 917.002 20.2016C957.502 20.4016 994.602 34.9016 1023.5 59.0016ZM1047.9 219.102C1046.5 207.102 1044.3 195.102 1041.3 183.402C1038.3 171.902 1034.6 160.602 1030.2 149.602C1025.8 138.702 1020.7 128.102 1014.9 118.002C1012.03 112.935 1009 107.968 1005.8 103.102C1002.73 98.4349 999.502 93.8349 996.102 89.3015C996.102 89.3015 996.068 89.2682 996.002 89.2016C1011.1 77.1016 1024.4 68.9015 1030.8 65.3015C1063.8 96.1015 1084.5 139.902 1084.5 188.602C1084.5 227.202 1071.5 262.702 1049.6 291.202C1049.7 277.002 1049.7 257.402 1049.7 256.402C1050 243.902 1049.4 231.502 1047.9 219.102Z"
                          fill="currentColor" />
                        <path
                          d="M869.098 323.5C867.898 323.5 866.898 324.8 866.898 326.3C866.898 327.7 867.898 328.9 869.098 328.9C870.298 328.9 871.198 327.7 871.198 326.3C871.198 324.8 870.298 323.5 869.098 323.5Z"
                          fill="currentColor" />


                      </svg>
                    </a>

                </div>

              </div>
              <div class="w-100 mt-3  text-lg-start text-center">
                
								
<!-- English Content (LCID 1033) -->
<SharePoint:LanguageSpecificContent runat="server" Languages="1033">
    <ContentTemplate>
                <span class="small text-white">Last page update: 
                    <time class="js-current-date-en" datetime=""></time> KSA Time
                </span>
    </ContentTemplate>
</SharePoint:LanguageSpecificContent>

<!-- Arabic Content (LCID 1025) -->
<SharePoint:LanguageSpecificContent runat="server" Languages="1025">
    <ContentTemplate>
                <span class="small text-white">تاريخ آخر تعديل: 
                    <time class="js-current-date-ar" datetime=""></time> بتوقيت السعودية
                </span>
    </ContentTemplate>
</SharePoint:LanguageSpecificContent>

				
              </div>
            </div>
          </div>
        </div>
      </div>
	  
	  
    </footer>

</dga-footer>


<script>

    document.querySelectorAll('a[href="https://wlc.org.sa/ar"]').forEach(function (link) {
        link.classList.add('external-link');
    });

    document.querySelectorAll('a[href="https://now.org.sa/"]').forEach(function (link) {
        link.classList.add('external-link');
    });

    (function () {
        const now = new Date();
        const options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false,
            timeZone: 'Asia/Riyadh'
        };

        const isoString = now.toISOString();

        // Update ALL English Time elements
        const enDate = new Intl.DateTimeFormat('en-GB', options).format(now).replace(',', '');
        document.querySelectorAll('.js-current-date-en').forEach(function (el) {
            el.innerText = enDate;
            el.setAttribute('datetime', isoString);
        });

        // Update ALL Arabic Time elements
        const arDate = new Intl.DateTimeFormat('ar-SA', options).format(now);
        document.querySelectorAll('.js-current-date-ar').forEach(function (el) {
            el.innerText = arDate;
            el.setAttribute('datetime', isoString);
        });
    })();
</script>

<style>
.more-calender-btn {
    z-index: 9;
    border-radius: 999px;
    width: 56px;
    height: 56px;
    bottom: 75px;
    padding: 0!important;
    background-color: var(--dga-green-700);
    color: #fff;

}

.more-calender-btn:focus:after {
    border-color: 0px;
    border-radius: 999px
}

[dir=rtl] .more-calender-btn {
    right: 12px
}

[dir=ltr] .more-calender-btn {
    left: 12px
}
    dialog#privacy-dialog {
        width: 100% !important;
        max-width: 100%;
        padding: 2rem !important;
        margin: 0;
        margin-top: auto;
        border: none;
        border-top: 0 !important;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.01);
        background-color: #fff;
        font: inherit;
        position: fixed;
        bottom: 0;
        right: 0;
        z-index: 1000;
        overflow: hidden;
    }

    .dialog-accept-btn {
        padding: 10px 0.75rem;
        background-color: #1b8354 !important;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        font: inherit;
        font-size: 1em;
        width: 100%;
    }

    .dialog-accept-btn:hover {
        background-color: #166a45 !important;
    }

    .dialog-accept-btn.active, .dialog-accept-btn:active {
        color: #104631 !important;
    }

    .dialog-title {
        font-size: 1.125rem !important;
        font-weight: 600 !important;
        margin-bottom: 1rem !important;
        color: #000 !important;
    }
</style>
<script src="/Style%20Library/Portal/js/dialog.min.js"></script>


