<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUniversityPresidents.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucUniversityPresidents" %>

<!-- ======================== DGA Design System Color Tokens & Styles ======================== -->
<style>
    .pnu-presidents-dga {
        --dga-primary: #006923;
        --dga-primary-dark: #004d1a;
        --dga-primary-25: #f4f9f6;
        --dga-primary-50: #e6f2eb;
        --dga-primary-subtle: #c7e3d2;
        --dga-dark: #1e293b;
        --dga-gray-800: #1e293b;
        --dga-gray-700: #334155;
        --dga-gray-600: #475569;
        --dga-gray-500: #64748b;
        --dga-warning: #b45309;
        --dga-warning-bg: #fef3c7;
    }

    .pnu-presidents-dga .bg-primary-25 {
        background-color: var(--dga-primary-25) !important;
    }

    .pnu-presidents-dga .text-primary {
        color: var(--dga-primary) !important;
    }

    .pnu-presidents-dga .bg-primary {
        background-color: var(--dga-primary) !important;
    }

    .pnu-presidents-dga .border-primary-subtle {
        border-color: var(--dga-primary-subtle) !important;
    }

    .pnu-presidents-dga .text-dga-dark {
        color: var(--dga-dark) !important;
    }

    .pnu-presidents-dga .text-dga-body {
        color: var(--dga-gray-700) !important;
    }

    .pnu-presidents-dga .text-dga-muted {
        color: var(--dga-gray-500) !important;
    }

    .pnu-presidents-dga .btn-outline-primary {
        color: var(--dga-primary) !important;
        border-color: var(--dga-primary) !important;
        background-color: #ffffff;
        transition: all 0.2s ease-in-out;
    }

    .pnu-presidents-dga .btn-outline-primary:hover,
    .pnu-presidents-dga .btn-outline-primary:focus {
        background-color: var(--dga-primary) !important;
        border-color: var(--dga-primary) !important;
        color: #ffffff !important;
    }

    .pnu-presidents-dga .nav-pills .nav-link {
        color: var(--dga-gray-700) !important;
        background-color: transparent;
        transition: all 0.2s ease-in-out;
    }

    .pnu-presidents-dga .nav-pills .nav-link:hover:not(.active) {
        background-color: var(--dga-primary-50) !important;
        color: var(--dga-primary) !important;
    }

    .pnu-presidents-dga .nav-pills .nav-link.active {
        background-color: var(--dga-primary) !important;
        color: #ffffff !important;
    }

    .pnu-presidents-dga .nav-pills .nav-link.active i {
        color: #ffffff !important;
    }

    .pnu-presidents-dga .icon-container-primary {
        background-color: #ffffff;
        color: var(--dga-primary);
    }

    .pnu-presidents-dga .icon-container-warning {
        background-color: var(--dga-warning-bg);
        color: var(--dga-warning);
    }

    .pnu-presidents-dga .contact-link {
        color: var(--dga-primary) !important;
        font-weight: 600;
        text-decoration: none;
    }

    .pnu-presidents-dga .contact-link:hover {
        color: var(--dga-primary-dark) !important;
        text-decoration: underline;
    }
</style>

<div class="pnu-presidents-dga">
    <!-- ======================== President Speech / Message Section ======================== -->
    <section id="president-speech-section" class="py-4 py-lg-5" aria-labelledby="president-speech-title">
        <div class="container">
            <asp:Repeater ID="rptSpeech" runat="server">
                <ItemTemplate>
                    <div class="row align-items-center g-4 g-lg-5 mb-4">
                        <div class="col-12 col-lg-7">
                            <div class="card border-0 bg-primary-25 rounded-4 p-4 p-md-5 h-100 shadow-sm">
                                <div class="d-flex flex-column gap-3">
                                    <span class="icon-container-primary rounded-3 p-2 shadow-xs d-inline-flex align-items-center justify-content-center" style="width: 44px; height: 44px;">
                                        <i class="hgi hgi-stroke hgi-quote-down fs-4" aria-hidden="true"></i>
                                    </span>
                                    <h2 id="president-speech-title" class="h3 fw-bold text-dga-dark mb-2">
                                        <%# Eval("ParagraphTitleAr") %>
                                    </h2>
                                    <p class="text-dga-body fs-5 mb-0 lh-lg text-justify">
                                        <%# Eval("ParagraphTextAr") %>
                                    </p>
                                </div>
                            </div>
                        </div>

                        <asp:PlaceHolder ID="phPresidentImage" runat="server"
                            Visible='<%# Eval("ImageUrl") != null && !string.IsNullOrEmpty(Eval("ImageUrl").ToString()) %>'>
                            <div class="col-12 col-lg-5 text-center">
                                <div class="position-relative d-inline-block">
                                    <img src="<%# Eval("ImageUrl") %>" class="img-fluid rounded-4 shadow-sm object-fit-cover" 
                                        style="max-height: 480px; width: auto;" alt="صورة رئيسة الجامعة" loading="lazy" />
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>

    <!-- ======================== Faculty Member / President Details Section ======================== -->
    <section id="president-details-section" class="pb-5 mb-5" aria-labelledby="president-details-heading">
        <div class="container">
            <div class="row g-4 g-xl-5">
                <!-- Left Column: Profile Card & Contacts -->
                <div class="col-12 col-lg-5 col-xl-4">
                    <div class="card border-0 bg-primary-25 rounded-4 shadow-sm h-100">
                        <div class="card-body p-4 p-xl-5 d-flex flex-column gap-4">
                            <!-- Profile Header -->
                            <asp:Repeater ID="rptProfile" runat="server">
                                <ItemTemplate>
                                    <div class="d-flex align-items-start gap-3">
                                        <span class="icon-container-primary rounded-circle p-3 shadow-xs flex-shrink-0 d-inline-flex align-items-center justify-content-center" style="width: 56px; height: 56px;">
                                            <i class="hgi hgi-stroke hgi-user-circle fs-2" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <h3 id="president-details-heading" class="h4 fw-bold text-dga-dark mb-1">
                                                <%# Eval("PresidentNameAr") %>
                                            </h3>
                                            <p class="text-primary fw-medium mb-1"><%# Eval("PresidentTitleAr") %></p>
                                            <span class="text-dga-muted small"><%# Eval("UniversityNameAr") %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <hr class="my-0 border-primary-subtle opacity-50" />

                            <!-- Office Contact Title -->
                            <asp:Repeater ID="rptProfile1" runat="server">
                                <ItemTemplate>
                                    <div class="d-flex align-items-center gap-2">
                                        <i class="hgi hgi-stroke hgi-call text-primary fs-5"></i>
                                        <h4 class="h6 fw-bold text-dga-dark mb-0">تواصل مع مكتب <%# Eval("PresidentTitleAr") %></h4>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <!-- Contact Details List -->
                            <div class="d-flex flex-column gap-3">
                                <asp:Repeater ID="rptContacts" runat="server" OnItemDataBound="rptContacts_ItemDataBound">
                                <ItemTemplate>
                                    <div class="d-flex align-items-center justify-content-between p-3 bg-white rounded-3 border shadow-xs">
                                        <span class="text-dga-body small fw-medium">
                                            <%# Eval("ContactLabelAr") %>
                                        </span>
                                        <div>
                                            <asp:HyperLink ID="lnkContact" runat="server" CssClass="contact-link small" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <!-- Tawasul Nourah Link -->
                            <div class="mt-auto pt-2">
                                <a href="https://tawasulnourah.pnu.edu.sa" target="_blank" rel="noopener noreferrer" 
                                   class="btn btn-outline-primary w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 shadow-xs text-decoration-none">
                                    <i class="hgi hgi-stroke hgi-sent fs-5"></i>
                                    <span class="fw-bold">بوابة تواصل نورة</span>
                                    <i class="hgi hgi-stroke hgi-arrow-left-01 small"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Right Column: Information Tabs -->
                <div class="col-12 col-lg-7 col-xl-8">
                    <div class="card border rounded-4 shadow-sm h-100 bg-white">
                        <!-- Tab Headers -->
                        <div class="p-3 p-md-4 border-bottom bg-light rounded-top-4">
                            <ul class="nav nav-pills nav-fill gap-2" id="presidentTab" role="tablist">
                                <li class="nav-item" role="presentation">
                                    <button class="nav-link active d-flex align-items-center justify-content-center gap-2 py-2 px-3 rounded-3 fw-semibold"
                                        id="bio-tab" data-bs-toggle="tab" data-bs-target="#bio-tab-pane" type="button"
                                        role="tab" aria-controls="bio-tab-pane" aria-selected="true">
                                        <i class="hgi hgi-stroke hgi-user-square fs-5" aria-hidden="true"></i>
                                        <span>السيرة الذاتية</span>
                                    </button>
                                </li>

                                <li class="nav-item" role="presentation">
                                    <button class="nav-link d-flex align-items-center justify-content-center gap-2 py-2 px-3 rounded-3 fw-semibold" 
                                        id="memberships-tab" data-bs-toggle="tab" data-bs-target="#memberships-tab-pane" type="button" 
                                        role="tab" aria-controls="memberships-tab-pane" aria-selected="false">
                                        <i class="hgi hgi-stroke hgi-id fs-5" aria-hidden="true"></i>
                                        <span>العضويات</span>
                                    </button>
                                </li>

                                <li class="nav-item" role="presentation">
                                    <button class="nav-link d-flex align-items-center justify-content-center gap-2 py-2 px-3 rounded-3 fw-semibold" 
                                        id="awards-tab" data-bs-toggle="tab" data-bs-target="#awards-tab-pane" type="button" 
                                        role="tab" aria-controls="awards-tab-pane" aria-selected="false">
                                        <i class="hgi hgi-stroke hgi-award-01 fs-5" aria-hidden="true"></i>
                                        <span>الأوسمة والجوائز</span>
                                    </button>
                                </li>

                                <li class="nav-item" role="presentation">
                                    <button class="nav-link d-flex align-items-center justify-content-center gap-2 py-2 px-3 rounded-3 fw-semibold"
                                        id="experiences-tab" data-bs-toggle="tab" data-bs-target="#experiences-tab-pane"
                                        type="button" role="tab" aria-controls="experiences-tab-pane" aria-selected="false">
                                        <i class="hgi hgi-stroke hgi-briefcase-01 fs-5" aria-hidden="true"></i>
                                        <span>الخبرات العملية</span>
                                    </button>
                                </li>
                            </ul>
                        </div>

                        <!-- Tab Content -->
                        <div class="card-body p-4 p-md-5">
                            <div class="tab-content" id="presidentTabContent">

                                <!-- Tab 1: Biography -->
                                <div class="tab-pane fade show active" id="bio-tab-pane" role="tabpanel" aria-labelledby="bio-tab" tabindex="0">
                                    <div class="d-flex align-items-center gap-2 mb-3">
                                        <span class="badge bg-primary text-white rounded-pill px-3 py-2">نبذة تعريفية</span>
                                    </div>
                                    <h3 class="h4 fw-bold text-dga-dark mb-4">
                                        <asp:Literal ID="litBiographyTitle" runat="server" />
                                    </h3>
                                    <div class="text-dga-body fs-5 lh-lg text-justify">
                                        <asp:Literal ID="litBiographyText" runat="server" />
                                    </div>
                                </div>

                                <!-- Tab 2: Memberships -->
                                <div class="tab-pane fade" id="memberships-tab-pane" role="tabpanel" aria-labelledby="memberships-tab" tabindex="0">
                                    <div class="d-flex align-items-center gap-2 mb-4">
                                        <i class="hgi hgi-stroke hgi-id text-primary fs-4" aria-hidden="true"></i>
                                        <h3 class="h4 fw-bold text-dga-dark mb-0">العضويات</h3>
                                    </div>

                                    <div class="d-flex flex-column gap-3">
                                        <asp:Repeater ID="rptMemberships" runat="server">
                                            <ItemTemplate>
                                                <div class="d-flex align-items-start gap-3 p-3 bg-light rounded-3 border">
                                                    <span class="icon-container-primary rounded-circle p-2 shadow-xs flex-shrink-0 d-inline-flex align-items-center justify-content-center" style="width: 32px; height: 32px;">
                                                        <i class="hgi hgi-stroke hgi-checkmark-circle-02 fs-5 text-primary" aria-hidden="true"></i>
                                                    </span>
                                                    <p class="text-dga-dark fw-medium fs-6 mb-0 align-self-center">
                                                        <%# Eval("MembershipTextAr") %>
                                                    </p>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>

                                <!-- Tab 3: Awards -->
                                <div class="tab-pane fade" id="awards-tab-pane" role="tabpanel" aria-labelledby="awards-tab" tabindex="0">
                                    <div class="d-flex align-items-center gap-2 mb-4">
                                        <i class="hgi hgi-stroke hgi-award-01 text-warning fs-4" aria-hidden="true" style="color: var(--dga-warning) !important;"></i>
                                        <h3 class="h4 fw-bold text-dga-dark mb-0">الأوسمة والجوائز</h3>
                                    </div>

                                    <div class="d-flex flex-column gap-3">
                                        <asp:Repeater ID="rptAwards" runat="server">
                                            <ItemTemplate>
                                                <div class="d-flex align-items-start gap-3 p-3 bg-light rounded-3 border">
                                                    <span class="icon-container-warning rounded-circle p-2 shadow-xs flex-shrink-0 d-inline-flex align-items-center justify-content-center" style="width: 32px; height: 32px;">
                                                        <i class="hgi hgi-stroke hgi-star fs-5" aria-hidden="true"></i>
                                                    </span>
                                                    <p class="text-dga-dark fw-medium fs-6 mb-0 align-self-center">
                                                        <%# Eval("AwardTextAr") %>
                                                    </p>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>

                                <!-- Tab 4: Experiences -->
                                <div class="tab-pane fade" id="experiences-tab-pane" role="tabpanel" aria-labelledby="experiences-tab" tabindex="0">
                                    <div class="d-flex align-items-center gap-2 mb-4">
                                        <i class="hgi hgi-stroke hgi-briefcase-01 text-primary fs-4" aria-hidden="true"></i>
                                        <h3 class="h4 fw-bold text-dga-dark mb-0">الخبرات العملية</h3>
                                    </div>

                                    <div class="d-flex flex-column gap-3">
                                        <asp:Repeater ID="rptExperiences" runat="server">
                                            <ItemTemplate>
                                                <div class="card border rounded-3 p-4 bg-white shadow-xs">
                                                    <div class="d-flex flex-column gap-2">
                                                        <div class="d-flex align-items-center gap-2">
                                                            <span class="rounded-2 p-2 d-inline-flex align-items-center justify-content-center bg-primary-25 text-primary" style="width: 36px; height: 36px;">
                                                                <i class="hgi hgi-stroke hgi-briefcase-01 fs-5" aria-hidden="true"></i>
                                                            </span>
                                                            <h4 class="h5 fw-bold text-dga-dark mb-0">
                                                                <%# Eval("JobTitleAr") %>
                                                            </h4>
                                                        </div>
                                                        <div class="d-flex flex-wrap align-items-center gap-3 text-dga-muted small mt-2">
                                                            <span class="d-inline-flex align-items-center gap-1">
                                                                <i class="hgi hgi-stroke hgi-building-03 text-primary" aria-hidden="true"></i>
                                                                <span><%# Eval("OrganizationAr") %></span>
                                                            </span>
                                                            <span class="text-muted">&bull;</span>
                                                            <span class="d-inline-flex align-items-center gap-1">
                                                                <i class="hgi hgi-stroke hgi-location-01 text-primary" aria-hidden="true"></i>
                                                                <span><%# Eval("LocationAr") %></span>
                                                            </span>
                                                            <span class="text-muted">&bull;</span>
                                                            <span class="d-inline-flex align-items-center gap-1">
                                                                <i class="hgi hgi-stroke hgi-calendar-03 text-primary" aria-hidden="true"></i>
                                                                <span><%# Eval("PeriodAr") %></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>

<!-- ======================== Script: Hide Speech Section If Empty ======================== -->
<script type="text/javascript">
    (function () {
        function checkPresidentSpeech() {
            var speechSection = document.getElementById('president-speech-section');
            if (!speechSection) return;

            var rows = speechSection.querySelectorAll('.row');
            if (!rows || rows.length === 0) {
                speechSection.style.display = 'none';
                return;
            }

            var hasContent = false;
            for (var i = 0; i < rows.length; i++) {
                var titleEl = rows[i].querySelector('h2');
                var textEl = rows[i].querySelector('p.text-justify');
                var imgEl = rows[i].querySelector('img');

                var titleText = titleEl ? titleEl.textContent.trim() : '';
                var bodyText = textEl ? textEl.textContent.trim() : '';
                var hasImg = imgEl && imgEl.getAttribute('src') && imgEl.getAttribute('src').trim() !== '';

                if (titleText !== '' || bodyText !== '' || hasImg) {
                    hasContent = true;
                    break;
                }
            }

            if (!hasContent) {
                speechSection.style.display = 'none';
            }
        }

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', checkPresidentSpeech);
        } else {
            checkPresidentSpeech();
        }

        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(checkPresidentSpeech);
        }
    })();
</script>
