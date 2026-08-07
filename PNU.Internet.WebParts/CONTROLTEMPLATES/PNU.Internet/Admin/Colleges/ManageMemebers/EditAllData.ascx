<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Admin/Colleges/ManageMemebers/ManageResume.ascx" TagPrefix="uc1" TagName="ManageResume" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Admin/Colleges/ManageMemebers/ManageLibraryHours.ascx" TagPrefix="uc1" TagName="ManageLibraryHours" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Admin/Colleges/ManageMemebers/ManageAdvertisements.ascx" TagPrefix="uc1" TagName="ManageAdvertisements" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Admin/Colleges/ManageMemebers/ManagePosts.ascx" TagPrefix="uc1" TagName="ManagePosts" %>




<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAllData.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers.EditAllData" %>




<%@ Import Namespace="PNU.Internet.WebParts" %>



<asp:Panel ID="pnlData" runat="server">
    <main id="main-content" class="dga-main-body" tabindex="-1">
        <section class="py-5" aria-labelledby="edit-member-title">
            <div class="container">

                <%-- User guide download --%>
                <div class="d-flex flex-wrap gap-3 align-items-center justify-content-between mb-4">
                    <a role="button"
                        href="/ar/Documents/خدمة%20موقع%20جامعة%20الأميرة%20نورة%20(بيانات%20أعضاء%20هيئة%20التدريس)-نسخة%20محدثة.pdf"
                        download="دليل المستخدم لصفحة عضو هيئة التدريس"
                        class="btn btn-secondary">
                        <i class="hgi hgi-stroke hgi-file-download me-1" aria-hidden="true"></i>
                        دليل المستخدم لصفحة عضو هيئة التدريس
                    </a>
                </div>

                <div class="row g-4">

                    <%-- Member profile card --%>
                    <div class="col-12 col-lg-4">
                        <asp:Repeater ID="rptMainData" runat="server">
                            <ItemTemplate>
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column h-100">
                                        <div class="icon-container">
                                            <i class="hgi hgi-stroke hgi-user-circle fs-3" aria-hidden="true"></i>
                                        </div>
                                        <div class="flex-grow-1">
                                            <h3 class="card-title" id="edit-member-title"><%# SPFactory.GetLocalizedTitle(Eval("full_name"), Eval("english_name")) %></h3>
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
                                                <span id="memberEmail"><%# DataBinder.Eval(Container.DataItem, "Email_address") %></span>
                                            </p>
                                            <p class="mb-0">
                                                <strong class="text-primary"><asp:Literal runat="server" Text="<%$ Resources: PNUres, ExtensionNo %>" />:</strong>
                                                <span><%# DataBinder.Eval(Container.DataItem, "extention_number") %></span>
                                            </p>
                                        </div>
                                    </div>
                                </article>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <%-- Edit forms --%>
                    <div class="col-12 col-lg-8">

                        <ul class="nav nav-tabs nav-underline nav-flush w-100 mb-4" id="edit-member-tabs" role="tablist">
                            <li class="nav-item" role="presentation" id="tabPersonal">
                                <button class="nav-link border-top-0 border-end-0 border-start-0 active d-inline-flex align-items-center gap-2 bg-transparent px-2" id="home-tab" data-bs-toggle="tab" data-bs-target="#z1-tab-pane" type="button" role="tab" aria-controls="z1-tab-pane" aria-selected="true">
                                    <i class="hgi hgi-stroke hgi-user-square fs-5 fw-light" aria-hidden="true"></i>
                                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, CV %>" /></span>
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college4-tab" data-bs-toggle="tab" data-bs-target="#z4-tab-pane" type="button" role="tab" aria-controls="z4-tab-pane" aria-selected="false">
                                    <i class="hgi hgi-stroke hgi-time-02 fs-5 fw-light" aria-hidden="true"></i>
                                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, LibraryHours %>" /></span>
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college5-tab" data-bs-toggle="tab" data-bs-target="#z5-tab-pane" type="button" role="tab" aria-controls="z5-tab-pane" aria-selected="false">
                                    <i class="hgi hgi-stroke hgi-megaphone-01 fs-5 fw-light" aria-hidden="true"></i>
                                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, Announcements %>" /></span>
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" id="college6-tab" data-bs-toggle="tab" data-bs-target="#z6-tab-pane" type="button" role="tab" aria-controls="z6-tab-pane" aria-selected="false">
                                    <i class="hgi hgi-stroke hgi-news fs-5 fw-light" aria-hidden="true"></i>
                                    <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, Tweets %>" /></span>
                                </button>
                            </li>
                        </ul>

                        <div class="tab-content pt-2" id="myTabContent">
                            <div class="tab-pane fade show active" id="z1-tab-pane" role="tabpanel" aria-labelledby="home-tab" tabindex="0">
                                <uc1:ManageResume runat="server" ID="ManageResume" />
                            </div>
                            <div class="tab-pane fade" id="z4-tab-pane" role="tabpanel" aria-labelledby="college4-tab" tabindex="0">
                                <uc1:ManageLibraryHours runat="server" ID="ManageLibraryHours" />
                            </div>
                            <div class="tab-pane fade" id="z5-tab-pane" role="tabpanel" aria-labelledby="college5-tab" tabindex="0">
                                <uc1:ManageAdvertisements runat="server" ID="ManageAdvertisements" />
                            </div>
                            <div class="tab-pane fade" id="z6-tab-pane" role="tabpanel" aria-labelledby="college6-tab" tabindex="0">
                                <uc1:ManagePosts runat="server" ID="ManagePosts" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </section>
    </main>
</asp:Panel>

<style>
    /* DGA-styled server-side pagination (shared by manage controls) */
    .dga-pagination .page-link,
    #main-content .pagination .page-link {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-width: 40px;
        height: 40px;
        border-radius: 8px;
        border: 1px solid var(--dga-border-default, #D1D5DB);
        color: var(--dga-text-primary, #111827);
        text-decoration: none;
        background: var(--dga-bg-surface, #FFFFFF);
    }
</style>

<script type="text/javascript">
    // ── Tab persistence across server postbacks (save buttons inside panes) ──
    (function () {
        var KEY = 'pnuEditDataActiveTab:' + window.location.pathname + window.location.search;

        document.addEventListener('click', function (e) {
            var btn = e.target.closest ? e.target.closest('#edit-member-tabs button[data-bs-target]') : null;
            if (btn) {
                try { sessionStorage.setItem(KEY, btn.getAttribute('data-bs-target')); } catch (ex) { }
            }
        }, true);

        function restoreTab() {
            var target;
            try { target = sessionStorage.getItem(KEY); } catch (ex) { return; }
            if (!target) return;

            var btn = document.querySelector('#edit-member-tabs button[data-bs-target="' + target + '"]');
            var pane = document.querySelector(target);
            if (!btn || !pane) return;

            document.querySelectorAll('#edit-member-tabs .nav-link').forEach(function (b) {
                b.classList.remove('active');
                b.setAttribute('aria-selected', 'false');
            });
            document.querySelectorAll('#myTabContent > .tab-pane').forEach(function (p) {
                p.classList.remove('active', 'show');
            });

            btn.classList.add('active');
            btn.setAttribute('aria-selected', 'true');
            pane.classList.add('active', 'show');
        }

        if (document.readyState === 'loading')
            document.addEventListener('DOMContentLoaded', restoreTab);
        else
            restoreTab();
    })();
</script>
