<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgSiteFooter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgSiteFooter" %>

<dga-footer>
    <footer class="dga-footer pt-4 pg-kit" role="contentinfo">
        <div class="container">
            <div class="justify-content-between row">

                <asp:Repeater ID="rptColumns" runat="server">
                    <ItemTemplate>
                        <div class="col-12 col-lg-3">
                            <div class="flex-grow-1">
                                <h3 class="text-white mb-0 pb-2 d-lg-block d-none h6"
                                    style="border-bottom:1px solid rgba(255,255,255,.3)"><%# Eval("Heading") %></h3>

                                <button type="button"
                                        class="btn btn-on-color-link collapsed d-lg-none has-rotatable-icon justify-content-between px-0 rounded-0 w-100 border-top-0 border-start-0 border-end-0"
                                        style="border-bottom:1px solid rgba(255,255,255,.3)"
                                        data-bs-toggle="collapse" data-bs-target='<%# Eval("CollapseHref") %>'
                                        aria-controls='<%# Eval("CollapseId") %>' aria-expanded="false">
                                    <%# Eval("Heading") %>
                                    <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
                                </button>

                                <div class="collapse d-lg-block" id='<%# Eval("CollapseId") %>'>
                                    <div class="d-flex flex-column gap-2 mt-2">
                                        <asp:Repeater runat="server" DataSource='<%# Eval("Links") %>'>
                                            <ItemTemplate>
                                                <a class="text-white small" href='<%# Eval("Url") %>'
                                                   target='<%# Eval("Target") %>' rel='<%# Eval("Rel") %>'><%# Eval("Text") %></a>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <div class="col-12 col-lg-3">
                    <div class="d-flex gap-4 flex-row flex-column mt-5 mt-lg-0">
                        <div class="flex-grow-1">
                            <h3 class="text-white mb-0 pb-2 h6" style="border-bottom:1px solid rgba(255,255,255,.3)"><%= SocialHeading %></h3>
                            <div class="d-flex gap-2 mt-2 flex-wrap">

                                <a href="https://www.facebook.com/PNUKSA0/" target="_blank" rel="noopener noreferrer"
                                   class="btn btn-outline-on-color-secondary icon-btn" aria-label="حساب الجامعة في منصة فيسبوك">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-facebook-02" aria-hidden="true"></i></span>
                                </a>

                                <div class="dropdown dropup">
                                    <button type="button" class="btn btn-outline-on-color-secondary icon-btn" data-bs-toggle="dropdown"
                                            aria-expanded="false" aria-label="حسابات الجامعة في منصة اكس">
                                        <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-new-twitter" aria-hidden="true"></i></span>
                                    </button>
                                    <ul class="dropdown-menu py-0">
                                        <li><a class="dropdown-item" href="https://twitter.com/_PNU_KSA" target="_blank" rel="noopener noreferrer">جامعة الأميرة نورة</a></li>
                                        <li><a class="dropdown-item" href="https://twitter.com/pnu_live" target="_blank" rel="noopener noreferrer">مباشر PNU</a></li>
                                        <li><a class="dropdown-item" href="https://twitter.com/pnu_events" target="_blank" rel="noopener noreferrer">فعاليات PNU</a></li>
                                    </ul>
                                </div>

                                <a href="https://www.linkedin.com/school/princess-nourah-bint-abdulrahman-university/" target="_blank"
                                   rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn"
                                   aria-label="حساب الجامعة في منصة لينكدان">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-linkedin-02" aria-hidden="true"></i></span>
                                </a>

                                <a href="https://www.snapchat.com/add/pnu_ksa" target="_blank" rel="noopener noreferrer"
                                   class="btn btn-outline-on-color-secondary icon-btn" aria-label="حساب الجامعة في منصة سناب شات">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-snapchat" aria-hidden="true"></i></span>
                                </a>

                                <a href="https://www.tiktok.com/@_pnu_ksa" target="_blank" rel="noopener noreferrer"
                                   class="btn btn-outline-on-color-secondary icon-btn" aria-label="حساب الجامعة في منصة تيك توك">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-tiktok" aria-hidden="true"></i></span>
                                </a>

                                <a href="https://medium.com/@pnu-ksa" target="_blank" rel="noopener noreferrer"
                                   class="btn btn-outline-on-color-secondary icon-btn" aria-label="حساب الجامعة في منصة ميديم">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-medium" aria-hidden="true"></i></span>
                                </a>

                                <a href="https://www.instagram.com/_PNU_KSA/" target="_blank" rel="noopener noreferrer"
                                   class="btn btn-outline-on-color-secondary icon-btn" aria-label="حساب الجامعة في منصة انستاقرام">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-instagram" aria-hidden="true"></i></span>
                                </a>

                                <a href="https://www.youtube.com/channel/UC-0BTuZ46ApPXt6dWONTyGg" target="_blank"
                                   rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn"
                                   aria-label="حساب الجامعة في منصة يوتيوب">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-youtube" aria-hidden="true"></i></span>
                                </a>

                                <a href="https://whatsapp.com/channel/0029VabX2wDKGGGN07X1c80f" target="_blank"
                                   rel="noopener noreferrer" class="btn btn-outline-on-color-secondary icon-btn"
                                   aria-label="قناة الجامعة في واتساب">
                                    <span class="d-inline-flex fs-6"><i class="hgi hgi-stroke hgi-whatsapp" aria-hidden="true"></i></span>
                                </a>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-12">
                    <div class="mt-5 mb-3 py-3">
                        <div class="d-flex flex-column flex-lg-row gap-4 align-items-center align-items-lg-center justify-content-lg-between">

                            <div class="flex-grow-1">
                                <p class="text-white small text-center fw-semibold mb-3 mt-4 text-lg-start"><%= Copyright %></p>

                                <div class="d-flex gap-4 flex-wrap">
                                    <asp:Repeater ID="rptBottomLinks" runat="server">
                                        <ItemTemplate>
                                            <a class="text-white small text-center" href='<%# Eval("Url") %>'
                                               target='<%# Eval("Target") %>' rel='<%# Eval("Rel") %>'><%# Eval("Text") %></a>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>

                            <div class="d-flex align-self-stretch gap-3 justify-content-between align-items-end">
                                <a href="https://vision2030.gov.sa/" target="_blank" rel="noopener noreferrer">
                                    <img alt="شعار رؤية السعودية 2030" src='<%= ImageRoot %>partners/vision-2030.png'
                                         width="60" height="40" title="رؤية 2030" loading="lazy" decoding="async" data-themekey="#">
                                </a>
                                <a href="https://raqmi.dga.gov.sa/platforms/platforms/82920d00-7973-42b5-e1c6-08dd66df7e11/platform-license"
                                   target="_blank" rel="noopener noreferrer">
                                    <img src='<%= ImageRoot %>partners/raqmi-license.svg' width="106" height="40"
                                         data-themekey="#" alt="شعار ترخيص منصة رقمي" loading="lazy" decoding="async">
                                </a>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </footer>
</dga-footer>
