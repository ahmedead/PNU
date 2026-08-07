<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeFooter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeFooter" %>





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
                <a href="/<%= IsArabic  ? "ar" : "en" %>/FAQs/Pages/default.aspx" target="_self" class="text-white small">الأسئلة الشائعة</a>
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

