<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 

<%@ Register Src="~/_controltemplates/15/PNU.Internet/Shared/ucDgaRatingFeedback.ascx" TagPrefix="PNU" TagName="DgaRatingFeedback" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Shared/ucDgaSurveyFeedback.ascx" TagPrefix="PNU" TagName="DgaSurveyFeedback" %>

<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDGAServiceDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.ucDGAServiceDetails" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>

<main id="main-content" class="dga-main-body">
    <asp:Repeater ID="rptAllData" runat="server" OnItemDataBound="rptAllData_ItemDataBound">
        <ItemTemplate>
            <div class="d-flex flex-colum gap-4 py-5" style="background-color: var(--dga-primary-25);" data-aos="fade-up">
                <div class="container">
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item">
                                <a id="lnkHome" runat="server"></a>
                            </li>
                            <li class="breadcrumb-item">
                                <a id="lnkEServices" runat="server"></a>
                            </li>
                            <li aria-current="page" class="breadcrumb-item active">
                                <%# SPFactory.GetLocalizedTitle(Eval("ARServiceName"), Eval("ENServiceName")) %>
                            </li>
                        </ol>
                    </nav>
                    <div class="row g-4">
                        <div class="col-12 col-lg-8" data-aos="fade-up">
                            <div class="d-flex flex-column flex-lg-row align-items-start align-items-lg-center justify-content-between gap-3">
                                <h1 class="mb-0 fw-semibold h2">
                                    <%# SPFactory.GetLocalizedTitle(Eval("ARServiceName"), Eval("ENServiceName")) %>
                                </h1>
                                <a id="lnkStart" runat="server" target="_blank" rel="noopener" class="btn btn-primary"
                                    href='<%# Eval("URL") %>'></a>
                            </div>
                            <div class="mt-3 d-flex flex-wrap gap-2">
                                <span id="badge1" runat="server" class="badge badge-info">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Badge1"), Eval("Badge1_EN")) %>
                                </span>
                                <span id="badge2" runat="server" class="badge badge-success">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Badge2"), Eval("Badge2_EN")) %>
                                </span>
                                <span id="badge3" runat="server" class="badge badge-warning">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Badge3"), Eval("Badge3_EN")) %>
                                </span>
                            </div>
                            <p class="mb-0 mt-3">
                                <%# SPFactory.GetLocalizedTitle(Eval("Desc"), Eval("Desc_EN")) %>
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container">
                <div class="g-4 row align-items-start e-service-detail-layout">
                    <%-- ============ MAIN COLUMN ============ --%>
                    <div class="col-12 col-lg-8">
                        <a id="slaLink" runat="server" class="d-flex gap-2 align-items-center text-decoration-none my-4"
                            href='<%# Eval("ServiceAgreement") %>' target="_blank" rel="noopener">
                            <span><asp:Literal ID="litSla" runat="server"></asp:Literal></span>
                            <i class="hgi hgi-stroke hgi-link-04"></i>
                        </a>

                        <div class="mt-4">
                            <ul class="nav nav-flush nav-underline" id="serviceTabs" role="tablist">
                                <li class="nav-item" role="presentation">
                                    <a class="nav-link active" href="#" aria-controls="tab1Content" aria-selected="true"
                                        data-bs-target="#tab1Content" data-bs-toggle="tab" id="tab1Tab" role="tab"><asp:Literal ID="litTabSteps" runat="server"></asp:Literal></a>
                                </li>
                                <li class="nav-item" role="presentation">
                                    <a class="nav-link" href="#" aria-controls="tab2Content" aria-selected="false"
                                        data-bs-target="#tab2Content" data-bs-toggle="tab" id="tab2Tab" role="tab" tabindex="-1"><asp:Literal ID="litTabReq" runat="server"></asp:Literal></a>
                                </li>
                                <li class="nav-item" role="presentation">
                                    <a class="nav-link" href="#" aria-controls="tab3Content" aria-selected="false"
                                        data-bs-target="#tab3Content" data-bs-toggle="tab" id="tab3Tab" role="tab" tabindex="-1"><asp:Literal ID="litTabDocs" runat="server"></asp:Literal></a>
                                </li>
                            </ul>
                            <div class="mt-4 tab-content service-tab-content" id="serviceTabContent">
                                <div class="fade tab-pane active show" id="tab1Content" aria-labelledby="tab1Tab" role="tabpanel">
                                    <asp:Literal ID="litSteps" runat="server" Mode="PassThrough"></asp:Literal>
                                </div>
                                <div class="fade tab-pane" id="tab2Content" aria-labelledby="tab2Tab" role="tabpanel">
                                    <ul>
                                        <li id="req1" runat="server">
                                            <%# SPFactory.GetLocalizedTitle(Eval("_x0052_eq1"), Eval("Req1_EN")) %>
                                        </li>
                                        <li id="req2" runat="server">
                                            <%# SPFactory.GetLocalizedTitle(Eval("_x0052_eq2"), Eval("Req2_EN")) %>
                                        </li>
                                        <li id="req3" runat="server">
                                            <%# SPFactory.GetLocalizedTitle(Eval("_x0052_eq3"), Eval("Req3_EN")) %>
                                        </li>
                                    </ul>
                                </div>
                                <div class="fade tab-pane" id="tab3Content" aria-labelledby="tab3Tab" role="tabpanel">
                                    <asp:Literal ID="litRequiredDocs" runat="server" Mode="PassThrough"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-- ============ SIDE CARD ============ --%>
                    <div class="col-12 col-lg-4">
                        <div class="card e-service-side-nav">
                            <div class="card-body p-5">
                                <div class="d-flex flex-column gap-3">

                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-user-group"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litTargetAudience" runat="server"></asp:Literal></h4>
                                            <p class="mb-0">
                                                <%# SPFactory.GetLocalizedTitle(Eval("DP_TargetGroup"), Eval("DP_TargetGroup_EN")) %>
                                            </p>
                                        </div>
                                    </div>

                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-clock-01"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litDuration" runat="server"></asp:Literal></h4>
                                            <p class="mb-0">
                                                <%# SPFactory.GetLocalizedTitle(Eval("TimeDuration"), Eval("TimeDuration_EN")) %>
                                            </p>
                                        </div>
                                    </div>

                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-computer-phone-sync"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litChannels" runat="server"></asp:Literal></h4>
                                            <p class="mb-0">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Channels"), Eval("Channels_EN")) %>
                                            </p>
                                        </div>
                                    </div>

                                    <div id="langRow" runat="server" class="d-flex gap-2 align-items-start">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 22 22" fill="none" class="flex-shrink-0" aria-hidden="true">
                                            <path fill-rule="evenodd" clip-rule="evenodd" d="M10.75 21.5C4.81294 21.5 0 16.6871 0 10.75C0 4.81294 4.81294 0 10.75 0C16.6871 0 21.5 4.81294 21.5 10.75C21.5 16.6871 16.6871 21.5 10.75 21.5ZM6.79401 19.1138C3.7851 17.6881 1.66834 14.6865 1.50958 11.1749C2.17856 11.5235 2.96341 11.7682 3.83879 11.7682C4.47252 11.7682 4.93837 11.7991 5.28488 11.864C5.63029 11.9287 5.80382 12.0178 5.89832 12.0952C6.05799 12.2259 6.20842 12.5052 6.20842 13.5018L6.20841 13.5761C6.20836 14.4989 6.20833 15.0472 6.28166 15.5375C6.35716 16.0424 6.50891 16.4772 6.75023 17.1686L6.77022 17.2259C7.0524 18.0347 7.09168 18.6264 6.79401 19.1138ZM10.75 20C9.86972 20 9.01816 19.877 8.21156 19.6474C8.695 18.6543 8.48898 17.5987 8.18649 16.7318C7.9195 15.9666 7.81758 15.6662 7.76516 15.3157C7.71045 14.9498 7.70842 14.5208 7.70842 13.5018C7.70842 12.5115 7.59887 11.549 6.84865 10.9346C6.4882 10.6395 6.04431 10.4802 5.56106 10.3897C5.07893 10.2994 4.5049 10.2682 3.83879 10.2682C2.98884 10.2682 2.21886 9.9231 1.58905 9.46133C2.08809 5.8811 4.63643 2.957 8.0108 1.9123C7.98881 2.19333 8.00012 2.47416 8.04742 2.74689C8.18481 3.53913 8.63585 4.28555 9.46268 4.68688C9.77793 4.8399 9.90463 4.99438 9.97437 5.1234C10.0581 5.27825 10.0993 5.4658 10.1456 5.77719C10.1506 5.81062 10.1556 5.8453 10.1607 5.88113C10.2458 6.47705 10.3758 7.38711 11.3819 8.03826C12.0614 8.47801 12.9118 8.63579 13.813 8.37062C14.6971 8.11045 15.5687 7.46329 16.3814 6.41131C16.8824 5.76286 17.5613 5.43053 18.2129 5.28382C19.3364 6.81518 20 8.7051 20 10.75C20 10.8656 19.9979 10.9807 19.9937 11.0953C19.1363 10.8916 18.1591 10.9818 17.1857 11.6815C16.2847 12.329 15.5506 12.3846 14.8761 12.4154L14.7837 12.4194C14.4773 12.4323 14.0868 12.4488 13.7392 12.5584C13.2861 12.7013 12.9079 12.992 12.647 13.492C12.2815 14.1925 12.3416 14.8104 12.5394 15.3507C12.6269 15.5894 12.7433 15.8194 12.8417 16.0138L12.8481 16.0265C12.954 16.2357 13.0469 16.42 13.1245 16.6168C13.2707 16.9877 13.3589 17.3961 13.2659 17.9412C13.1848 18.4162 12.9593 19.0333 12.4515 19.8438C11.9 19.9464 11.3312 20 10.75 20ZM14.4488 19.2309C17.1709 18.042 19.2102 15.5809 19.8151 12.5991C19.2575 12.4207 18.6691 12.4625 18.0611 12.8995C16.8063 13.8014 15.7194 13.8785 14.9445 13.9139C14.5376 13.9324 14.3411 13.9414 14.1904 13.9889C14.1093 14.0145 14.0506 14.0444 13.9768 14.1859C13.8496 14.4297 13.8598 14.5942 13.948 14.8348C14.0016 14.9813 14.0788 15.1365 14.1865 15.3492L14.1944 15.3647C14.2943 15.5622 14.4156 15.8018 14.5201 16.0669C14.7436 16.6341 14.8931 17.3226 14.7445 18.1935C14.6887 18.5204 14.5924 18.8648 14.4488 19.2309ZM9.59109 1.5719C9.49747 1.89221 9.47707 2.21215 9.52536 2.49057C9.59508 2.89261 9.79751 3.18204 10.1177 3.33744C10.6824 3.61153 11.0581 3.97385 11.2939 4.41013C11.5158 4.82057 11.5832 5.24679 11.6293 5.55644L11.6332 5.58284C11.7275 6.21687 11.77 6.50266 12.1969 6.77898C12.5171 6.98617 12.9162 7.0709 13.3895 6.93162C13.8799 6.78734 14.5077 6.38311 15.1944 5.49425C15.7526 4.77173 16.4434 4.32206 17.1296 4.05198C15.4699 2.47073 13.2233 1.5 10.75 1.5C10.3575 1.5 9.97073 1.52445 9.59109 1.5719Z" fill="#1B8354"></path>
                                        </svg>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litLanguage" runat="server"></asp:Literal></h4>
                                            <p class="mb-0">
                                                <%# SPFactory.GetLocalizedTitle(Eval("ServiceLanguage"), Eval("ServiceLanguage_EN")) %>
                                            </p>
                                        </div>
                                    </div>

                                    <div id="costRow" runat="server" class="d-flex gap-2 align-items-start">
                                        <svg class="flex-shrink-0" fill="none" height="24" viewBox="0 0 22 22" width="24" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
                                            <path d="M11 21.75C16.9371 21.75 21.75 16.9369 21.75 11C21.75 5.06292 16.9371 0.25 11 0.25C5.06292 0.25 0.25 5.06292 0.25 11C0.25 16.9369 5.06291 21.75 11 21.75ZM11 20.25C5.89136 20.25 1.75 16.1085 1.75 11C1.75 5.89135 5.89135 1.75 11 1.75C16.1087 1.75 20.25 5.89135 20.25 11C20.25 16.1085 16.1086 20.25 11 20.25ZM9.33594 14.3135C9.60752 14.2588 9.8414 14.1035 9.99316 13.8896L10.6045 13.0127V13.0117C10.668 12.9209 10.7051 12.8113 10.7051 12.6934V11.4023L11.8818 11.1611V13.4883L15.6582 12.7109V12.7119C15.8366 12.3292 15.9548 11.9132 16 11.4775L13.0586 12.083V10.9189L15.6582 10.3838C15.8365 10.0012 15.9548 9.5859 16 9.15039L13.0586 9.75488L13.0586 5.56934C12.6079 5.81421 12.2072 6.13996 11.8818 6.52441L11.8818 9.99707L10.7051 10.2393L10.7051 5C10.2546 5.24472 9.85458 5.57078 9.5293 5.95508L9.5293 10.4805L6.89648 11.0225C6.71818 11.4051 6.59999 11.8204 6.55469 12.2559L9.5293 11.6445V13.1104L6.3418 13.7656C6.16335 14.1484 6.04519 14.5642 6 15L9.33594 14.3135ZM15.668 15.2275C15.8413 14.8468 15.956 14.4334 16 14L12.332 14.7725C12.1587 15.1533 12.0439 15.5666 12 16L15.668 15.2275Z" fill="#1B8354"></path>
                                        </svg>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litCost" runat="server"></asp:Literal></h4>
                                            <p class="mb-0">
                                                <%# SPFactory.GetLocalizedTitle(Eval("ServiceCost"), Eval("ServiceCost_EN")) %>
                                            </p>
                                        </div>
                                    </div>

                                    <hr class="my-2" />

                                    <%-- ====== STATIC SECTION (same for all services, bilingual via PNUres) ====== --%>
                                    <div>
                                        <h4 class="fw-bold mb-1 h5"><asp:Literal ID="litFaqTitle" runat="server"></asp:Literal></h4>
                                        <a id="lnkFaq" runat="server" class="d-flex gap-2 align-items-center text-decoration-none">
                                            <span><asp:Literal ID="litFaqPage" runat="server"></asp:Literal></span>
                                            <i class="hgi hgi-stroke hgi-link-square-02"></i>
                                        </a>
                                    </div>

                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-call"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litTelephone" runat="server"></asp:Literal></h4>
                                            <a class="d-flex gap-2 align-items-center text-decoration-none" href="tel:0118241111">
                                                <span dir="ltr">0118241111</span>
                                                <i class="hgi hgi-stroke hgi-link-square-02"></i>
                                            </a>
                                        </div>
                                    </div>

                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-mail-01"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litEmail" runat="server"></asp:Literal></h4>
                                            <a class="d-flex gap-2 align-items-center text-decoration-none" href="mailto:info@pnu.edu.sa">
                                                <span>info@pnu.edu.sa</span>
                                                <i class="hgi hgi-stroke hgi-link-square-02"></i>
                                            </a>
                                        </div>
                                    </div>

                                    <%-- ====== SHARE ====== --%>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-share-08"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="litSharePage" runat="server"></asp:Literal></h4>
                                            <div class="d-flex gap-2 mt-2">
                                                <a id="lnkShareWa" runat="server" target="_blank" rel="noopener noreferrer" class="btn btn-outline-secondary icon-btn" aria-label="Whatsapp">
                                                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-whatsapp"></i></span>
                                                </a>
                                                <a id="lnkShareLi" runat="server" target="_blank" rel="noopener noreferrer" class="btn btn-outline-secondary icon-btn" aria-label="Linkedin">
                                                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-linkedin-02"></i></span>
                                                </a>
                                                <a id="lnkShareEmail" runat="server" class="btn btn-outline-secondary icon-btn" aria-label="Email">
                                                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-mail-01"></i></span>
                                                </a>
                                                <a id="lnkShareX" runat="server" target="_blank" rel="noopener noreferrer" class="btn btn-outline-secondary icon-btn" aria-label="X">
                                                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-new-twitter"></i></span>
                                                </a>
                                                <a id="lnkShareFb" runat="server" target="_blank" rel="noopener noreferrer" class="btn btn-outline-secondary icon-btn" aria-label="Facebook">
                                                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-facebook-02"></i></span>
                                                </a>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="UserManualLink" runat="server">
                                        <a class="btn btn-secondary solid" href='<%# Eval("UserManualURL") %>' target="_blank" rel="noopener"><asp:Literal ID="litUserManual" runat="server"></asp:Literal></a>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</main>

<div class="py-3">
    <div class="container">
        <span class="small"><asp:Literal ID="litLastModified" runat="server"></asp:Literal></span>
    </div>
</div>

<%-- Reusable DGA feedback controls --%>
<PNU:DgaRatingFeedback ID="ucRating" runat="server" />
<PNU:DgaSurveyFeedback ID="ucSurvey" runat="server" />
