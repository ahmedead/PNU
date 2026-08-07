<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucGoogleScolar.ascx" TagPrefix="uc1" TagName="ucGoogleScolar" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucMemberResume.ascx" TagPrefix="uc1" TagName="ucMemberResume" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucMemberDegrees.ascx" TagPrefix="uc1" TagName="ucMemberDegrees" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucMemberAds.ascx" TagPrefix="uc1" TagName="ucMemberAds" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucMemberTweets.ascx" TagPrefix="uc1" TagName="ucMemberTweets" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucMemberLibrary.ascx" TagPrefix="uc1" TagName="ucMemberLibrary" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucORCIDData.ascx" TagPrefix="uc1" TagName="ucORCIDData" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/FacultyMembers/ucMemberCourses.ascx" TagPrefix="uc1" TagName="ucMemberCourses" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucViewFacultyMembers.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucViewFacultyMembers" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<%-- Edit panel (visible for the member only) --%>
<div id="divEdit" runat="server">
    <div class="container pt-4">
        <div class="d-flex flex-wrap gap-3 align-items-center justify-content-between">
            <a role="button"
                href="/ar/Documents/خدمة%20موقع%20جامعة%20الأميرة%20نورة%20(بيانات%20أعضاء%20هيئة%20التدريس)-نسخة%20محدثة.pdf"
                download="دليل المستخدم لصفحة عضو هيئة التدريس"
                class="btn btn-secondary">
                <i class="hgi hgi-stroke hgi-file-download me-1" aria-hidden="true"></i>
                دليل المستخدم لصفحة عضو هيئة التدريس
            </a>
            <a class="btn btn-primary px-4" id="edit-link" href="/ar/Faculties/Pages/EditData.aspx" data-ur1313m3t="true">
                <asp:Literal ID="lit_summary" runat="server" Text="<%$Resources:PnuInternetResources, res_MemberUpdateData%>"></asp:Literal>
            </a>
        </div>
    </div>
</div>

<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>

        <%-- Page header + breadcrumb --%>
        <div class="bg-primary-25 py-5">
            <div class="container">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-2">
                        <li class="breadcrumb-item small">
                            <a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a>
                        </li>
                        <li class="breadcrumb-item small active" aria-current="page">
                            <span><%# SPFactory.GetLocalizedTitle(Eval("full_name"), Eval("english_name")) %></span>
                        </li>
                    </ol>
                </nav>
                <div class="content">
                    <h2 class="mb-0"><%# SPFactory.GetLocalizedTitle(Eval("full_name"), Eval("english_name")) %></h2>
                    <div class="text mt-4"><%# DataBinder.Eval(Container.DataItem, "profession") %></div>
                </div>
            </div>
        </div>

        <main id="main-content" class="dga-main-body" tabindex="-1">
            <section class="py-5" data-aos="fade-up" aria-labelledby="faculty-member-title">
                <div class="container">
                    <div class="row g-4 mb-5">

                        <%-- Member profile card --%>
                        <div class="col-12 col-lg-4">
                            <article class="card h-100 pnu-news-card">
                                <div class="card-body d-flex flex-column placeholder-glow h-100">
                                    <div class="icon-container">
                                        <i class="hgi hgi-stroke hgi-user-circle fs-3" aria-hidden="true"></i>
                                    </div>
                                    <div class="flex-grow-1">
                                        <h3 class="card-title" id="faculty-member-title"><%# SPFactory.GetLocalizedTitle(Eval("full_name"), Eval("english_name")) %></h3>
                                        <p class="card-text"><%# DataBinder.Eval(Container.DataItem, "profession") %></p>
                                    </div>
                                    <div class="d-flex flex-column gap-3 card-text">
                                        <p class="mb-0">
                                            <strong class="text-primary"><asp:Literal runat="server" Text="<%$ Resources: PNUres, section %>" />:</strong>
                                            <span><%# DataBinder.Eval(Container.DataItem, "section") %></span>
                                        </p>
                                        <p class="mb-0">
                                            <strong class="text-primary"><asp:Literal runat="server" Text="<%$ Resources: PNUres, specialization %>" />:</strong>
                                            <span><%# DataBinder.Eval(Container.DataItem, "specialization") %></span>
                                        </p>
                                        <p class="mb-0">
                                            <strong class="text-primary"><asp:Literal runat="server" Text="<%$ Resources: PNUres, minor %>" />:</strong>
                                            <span><%# DataBinder.Eval(Container.DataItem, "SPECIAL_CPECIALIZATION") %></span>
                                        </p>
                                        <p class="mb-0">
                                            <strong class="text-primary"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Email %>" />:</strong>
                                            <a href="mailto:<%# DataBinder.Eval(Container.DataItem, "Email_address") %>"><%# DataBinder.Eval(Container.DataItem, "Email_address") %></a>
                                        </p>
                                        <p class="mb-0">
                                            <strong class="text-primary"><asp:Literal runat="server" Text="<%$ Resources: PNUres, ExtensionNo %>" />:</strong>
                                            <span><%# DataBinder.Eval(Container.DataItem, "extention_number") %></span>
                                        </p>
                                    </div>
                                </div>
                            </article>
                        </div>

                        <%-- Right column: current position + research interests + brief (from MembersResumes) --%>
                        <div class="col-12 col-lg-8">
                            <uc1:ucMemberResume runat="server" ID="ucMemberResume" />
                        </div>

                    </div>
                </div>
            </section>
        </main>

    </ItemTemplate>
</asp:Repeater>

<section class="py-5 pt-0">
    <div class="container">

        <ul class="nav nav-tabs nav-underline nav-flush w-100 mb-4" id="faculty-detail-tabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 active d-inline-flex align-items-center gap-2 bg-transparent px-2" id="home-tab" data-bs-toggle="tab" data-bs-target="#z1-tab-pane" type="button" role="tab" aria-controls="z1-tab-pane" aria-selected="true">
                    <i class="hgi hgi-stroke hgi-user-square fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, CV %>" /></span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college22tab" data-bs-toggle="tab" data-bs-target="#z2-tab-pane" type="button" role="tab" aria-controls="z2-tab-pane" aria-selected="false" runat="server" onserverclick="college2tab_Click">
                    <i class="hgi hgi-stroke hgi-book-open-01 fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, Courses %>" /></span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college33tab" data-bs-toggle="tab" data-bs-target="#z3-tab-pane" type="button" role="tab" aria-controls="z3-tab-pane" aria-selected="false" runat="server" onserverclick="college3tab_Click">
                    <i class="hgi hgi-stroke hgi-search-visual fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, GoogleScholar %>" /></span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college44tab" data-bs-toggle="tab" data-bs-target="#z4-tab-pane" type="button" role="tab" aria-controls="z4-tab-pane" aria-selected="false" runat="server" onserverclick="college4tab_Click">
                    <i class="hgi hgi-stroke hgi-time-02 fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, LibraryHours %>" /></span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college66tab" data-bs-toggle="tab" data-bs-target="#z6-tab-pane" type="button" role="tab" aria-controls="z6-tab-pane" aria-selected="false" runat="server" onserverclick="college6tab_Click">
                    <i class="hgi hgi-stroke hgi-news fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, Tweets %>" /></span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college77tab" data-bs-toggle="tab" data-bs-target="#z7-tab-pane" type="button" role="tab" aria-controls="z7-tab-pane" aria-selected="false" runat="server" onserverclick="college7tab_Click">
                    <i class="hgi hgi-stroke hgi-link-04 fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, ORCID %>" /></span>
                </button>
            </li>
        </ul>

        <div class="tab-content pt-2" id="myTabContent">
            <%-- CV: degrees --%>
            <div class="tab-pane fade show active" id="z1-tab-pane" role="tabpanel" aria-labelledby="home-tab" tabindex="0">
                <uc1:ucMemberDegrees runat="server" ID="ucMemberDegrees" />
            </div>

            <div class="tab-pane fade" id="z2-tab-pane" role="tabpanel" aria-labelledby="college22tab" tabindex="0">
                <uc1:ucMemberCourses runat="server" ID="ucMemberCourses" />
            </div>

            <div class="tab-pane fade" id="z3-tab-pane" role="tabpanel" aria-labelledby="college33tab" tabindex="0">
                <uc1:ucGoogleScolar runat="server" ID="ucGoogleScolar" />
            </div>

            <div class="tab-pane fade" id="z4-tab-pane" role="tabpanel" aria-labelledby="college44tab" tabindex="0">
                <uc1:ucMemberLibrary runat="server" ID="ucMemberLibrary" />
            </div>

            <div class="tab-pane fade" id="z6-tab-pane" role="tabpanel" aria-labelledby="college66tab" tabindex="0">
                <uc1:ucMemberTweets runat="server" ID="ucMemberTweets" />
            </div>

            <div class="tab-pane fade" id="z7-tab-pane" role="tabpanel" aria-labelledby="college77tab" tabindex="0">
                <uc1:ucORCIDData runat="server" ID="ucORCIDData" />
            </div>
        </div>

    </div>
</section>

<style>
    /* DGA-styled server-side pagination (shared by tab controls) */
    .dga-pagination .page-link {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 40px;
        height: 40px;
        border-radius: 8px;
        border: 1px solid var(--dga-border-default, #D1D5DB);
        color: var(--dga-text-primary, #111827);
        text-decoration: none;
        background: var(--dga-bg-surface, #FFFFFF);
    }
    .dga-pagination .page-link:hover {
        background: var(--dga-primary-50, #EDF8F3);
        color: var(--dga-primary-700, #006B3D);
    }
    .dga-pagination .page-link.active,
    .dga-pagination .page-link[disabled] {
        background: var(--dga-primary-700, #006B3D);
        border-color: var(--dga-primary-700, #006B3D);
        color: #fff;
    }
    .dga-pagination .dga-paging-list td { padding-inline: 4px; }
</style>

<script type="text/javascript">
    // ── Tab persistence across server postbacks ──────────────────────
    (function () {
        var KEY = 'pnuFacultyActiveTab:' + window.location.pathname + window.location.search;

        // 1. Save the clicked tab BEFORE the postback fires
        document.addEventListener('click', function (e) {
            var btn = e.target.closest ? e.target.closest('#faculty-detail-tabs button[data-bs-target]') : null;
            if (btn) {
                try { sessionStorage.setItem(KEY, btn.getAttribute('data-bs-target')); } catch (ex) { }
            }
        }, true);

        // 2. After the page returns from the server, re-activate that tab
        function restoreTab() {
            var target;
            try { target = sessionStorage.getItem(KEY); } catch (ex) { return; }
            if (!target) return;

            var btn = document.querySelector('#faculty-detail-tabs button[data-bs-target="' + target + '"]');
            var pane = document.querySelector(target);
            if (!btn || !pane) return;

            // Deactivate ALL tabs and panes first (clears the hard-coded 'show active' on the CV pane)
            document.querySelectorAll('#faculty-detail-tabs .nav-link').forEach(function (b) {
                b.classList.remove('active');
                b.setAttribute('aria-selected', 'false');
            });
            document.querySelectorAll('#myTabContent > .tab-pane').forEach(function (p) {
                p.classList.remove('active', 'show');
            });

            // Activate only the stored tab
            btn.classList.add('active');
            btn.setAttribute('aria-selected', 'true');
            pane.classList.add('active', 'show');
        }

        if (document.readyState === 'loading')
            document.addEventListener('DOMContentLoaded', restoreTab);
        else
            restoreTab();
    })();

    // Kept for code-behind use, e.g.:
    // Page.ClientScript.RegisterStartupScript(GetType(), "activeTab", "activeTab('z2-tab-pane');", true);
    function activeTab(tabno) {
        function doShow() {
            var btn = document.querySelector('#faculty-detail-tabs button[data-bs-target="#' + tabno + '"]');
            if (btn && window.bootstrap) {
                bootstrap.Tab.getOrCreateInstance(btn).show();
            }
        }
        if (document.readyState === 'loading')
            document.addEventListener('DOMContentLoaded', doShow);
        else
            doShow();
    }

    // Points the Edit button at the AR or EN edit page based on the URL.
    (function () {
        var linkElement = document.getElementById('edit-link');
        if (linkElement) {
            linkElement.href = window.location.pathname.indexOf('/ar/') !== -1
                ? "/ar/Faculties/Pages/EditData.aspx"
                : "/en/Faculties/Pages/EditData.aspx";
        }
    })();
</script>
