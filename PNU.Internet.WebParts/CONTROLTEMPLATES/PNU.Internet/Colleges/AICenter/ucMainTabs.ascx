<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAboutCenter.ascx" TagPrefix="uc1" TagName="ucAboutCenter" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucCenterMembers.ascx" TagPrefix="uc1" TagName="ucCenterMembers" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucResearchPaths.ascx" TagPrefix="uc1" TagName="ucResearchPaths" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucNewsLetterFromList.ascx" TagPrefix="uc1" TagName="ucNewsLetter" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAIContactUS.ascx" TagPrefix="uc1" TagName="ucAIContactUS" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAICalenderNew.ascx" TagPrefix="uc2" TagName="ucAICalender" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAICenterNews.ascx" TagPrefix="uc1" TagName="ucAICenterNews" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMainTabs.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucMainTabs" %>

<asp:Panel ID="pnlAll" runat="server">
    <div class="container py-5 entity-details-container">
        <div class="entity-details-layout d-flex flex-column gap-4 flex-lg-row gap-lg-4 align-items-start">

            <aside class="entity-details-sidemenu bg-body" aria-label="قائمة المركز">
                <button type="button"
                    class="entity-details-sidemenu-toggle btn btn-link w-100 align-items-center gap-3 px-3 text-decoration-none has-rotatable-icon collapsed border-bottom"
                    data-bs-toggle="collapse" data-bs-target="#center-details-sidemenu-drawer"
                    aria-expanded="false" aria-controls="center-details-sidemenu-drawer">
                    <i class="hgi hgi-stroke hgi-menu-02 fs-4 text-primary" aria-hidden="true"></i>
                    <span id="spnCurrentCenterTab" class="entity-details-sidemenu-current flex-grow-1 text-start fw-semibold"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AboutCenter %>" /></span>
                    <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
                </button>

                <div class="entity-details-sidemenu-drawer collapse d-lg-block" id="center-details-sidemenu-drawer">
                    <nav class="entity-details-tabs nav flex-column align-items-stretch" id="center-details-tabs" role="tablist" aria-label="أقسام مركز الذكاء الاصطناعي">
                        
                        <button class="nav-link active text-start text-nowrap" id="news1-tab" data-bs-toggle="tab"
                            data-bs-target="#news1-tab-pane" type="button" role="tab"
                            aria-controls="news1-tab-pane" aria-selected="true" tabindex="0">
                            <i class="hgi hgi-stroke hgi-home-02 fs-5 me-2" aria-hidden="true"></i>
                            <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, AboutCenter %>" /></span>
                        </button>

                        <button class="nav-link text-start text-nowrap" id="college22tab" data-bs-toggle="tab"
                            data-bs-target="#news2-tab-pane" type="button" role="tab" runat="server" onserverclick="college2tab_Click"
                            aria-controls="news2-tab-pane" aria-selected="false" tabindex="1">
                            <i class="hgi hgi-stroke hgi-atomic-power fs-5 me-2" aria-hidden="true"></i>
                            <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, ResearchPaths %>" /></span>
                        </button>

                        <button class="nav-link text-start text-nowrap" id="new7-tab" data-bs-toggle="tab"
                            data-bs-target="#news7-tab-pane" type="button" role="tab"
                            aria-controls="news7-tab-pane" aria-selected="false" tabindex="2">
                            <i class="hgi hgi-stroke hgi-news fs-5 me-2" aria-hidden="true"></i>
                            <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, AIRecentNews %>" /></span>
                        </button>

                        <button class="nav-link text-start text-nowrap" id="news3-tab" data-bs-toggle="tab"
                            data-bs-target="#news3-tab-pane" type="button" role="tab"
                            aria-controls="news3-tab-pane" aria-selected="false" tabindex="3">
                            <i class="hgi hgi-stroke hgi-file-02 fs-5 me-2" aria-hidden="true"></i>
                            <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, NewsArticles %>" /></span>
                        </button>

                        <button class="nav-link text-start text-nowrap" id="new4-tab" data-bs-toggle="tab"
                            data-bs-target="#news4-tab-pane" type="button" role="tab"
                            aria-controls="news4-tab-pane" aria-selected="false" tabindex="4">
                            <i class="hgi hgi-stroke hgi-calendar-03 fs-5 me-2" aria-hidden="true"></i>
                            <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, AICalender %>" /></span>
                        </button>

                        <button class="nav-link text-start text-nowrap" id="new5-tab" data-bs-toggle="tab"
                            data-bs-target="#news5-tab-pane" type="button" role="tab"
                            aria-controls="news5-tab-pane" aria-selected="false" tabindex="5">
                            <i class="hgi hgi-stroke hgi-user-multiple-02 fs-5 me-2" aria-hidden="true"></i>
                            <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, AIMembers %>" /></span>
                        </button>

                        <button class="nav-link text-start text-nowrap" id="new6-tab" data-bs-toggle="tab"
                            data-bs-target="#news6-tab-pane" type="button" role="tab"
                            aria-controls="news6-tab-pane" aria-selected="false" tabindex="6">
                            <i class="hgi hgi-stroke hgi-call fs-5 me-2" aria-hidden="true"></i>
                            <span><asp:Literal runat="server" Text="<%$ Resources: PNUres, AIContactUS %>" /></span>
                        </button>

                    </nav>
                </div>
            </aside>

            <div class="flex-grow-1 w-100 min-w-0">
                <div class="tab-content" id="center-details-tabs-content">

                    <div class="tab-pane fade show active" id="news1-tab-pane" role="tabpanel" aria-labelledby="news1-tab" tabindex="0">
                        <uc1:ucAboutCenter runat="server" id="ucAboutCenter" />
                    </div>

                    <div class="tab-pane fade" id="news2-tab-pane" role="tabpanel" aria-labelledby="college22tab" tabindex="0">
                        <uc1:ucResearchPaths runat="server" id="ucResearchPaths" />
                    </div>

                    <div class="tab-pane fade" id="news7-tab-pane" role="tabpanel" aria-labelledby="new7-tab" tabindex="0">
                        <uc1:ucAICenterNews runat="server" id="ucAICenterNews" />
                    </div>

                    <div class="tab-pane fade" id="news3-tab-pane" role="tabpanel" aria-labelledby="news3-tab" tabindex="0">
                        <uc1:ucNewsLetter runat="server" id="ucNewsLetter" ListName="Newsletters"/>
                    </div>

                    <div class="tab-pane fade" id="news4-tab-pane" role="tabpanel" aria-labelledby="new4-tab" tabindex="0">
                        <uc2:ucAICalender runat="server" id="ucAICalender" />
                    </div>

                    <div class="tab-pane fade" id="news5-tab-pane" role="tabpanel" aria-labelledby="new5-tab" tabindex="0">
                        <uc1:ucCenterMembers runat="server" id="ucCenterMembers" />
                    </div>

                    <div class="tab-pane fade" id="news6-tab-pane" role="tabpanel" aria-labelledby="new6-tab" tabindex="0">
                        <uc1:ucAIContactUS runat="server" id="ucAIContactUS" />
                    </div>

                </div>
            </div>

        </div>
    </div>
</asp:Panel>

<script type="text/javascript">
    function activeTab(tabno) {
        var targetSelector = '#news' + tabno + '-tab-pane';
        var trigger = document.querySelector('#center-details-tabs [data-bs-target="' + targetSelector + '"]');
        if (trigger) {
            activateCenterTab(trigger);
        }
    }

    function activateCenterTab(triggerBtn) {
        if (!triggerBtn) return;
        var allTriggers = document.querySelectorAll('#center-details-tabs [data-bs-toggle="tab"]');
        allTriggers.forEach(function (btn) {
            btn.classList.remove('active');
            btn.setAttribute('aria-selected', 'false');
        });

        var targetSelector = triggerBtn.getAttribute('data-bs-target');
        var allPanes = document.querySelectorAll('#center-details-tabs-content > .tab-pane');
        allPanes.forEach(function (pane) {
            pane.classList.remove('show', 'active');
        });

        triggerBtn.classList.add('active');
        triggerBtn.setAttribute('aria-selected', 'true');
        var targetPane = document.querySelector(targetSelector);
        if (targetPane) {
            targetPane.classList.add('show', 'active');
        }

        var labelSpan = triggerBtn.querySelector('span');
        var currentLabel = document.getElementById('spnCurrentCenterTab');
        if (currentLabel && labelSpan) {
            currentLabel.textContent = labelSpan.textContent.trim();
        }
    }

    document.addEventListener('DOMContentLoaded', function () {
        var triggers = document.querySelectorAll('#center-details-tabs [data-bs-toggle="tab"]');
        triggers.forEach(function (btn) {
            btn.addEventListener('click', function (e) {
                if (!btn.getAttribute('onserverclick')) {
                    activateCenterTab(btn);
                }

                // Close mobile drawer after click
                var drawer = document.getElementById('center-details-sidemenu-drawer');
                if (drawer && window.bootstrap) {
                    var bsCollapse = bootstrap.Collapse.getInstance(drawer);
                    if (bsCollapse && window.innerWidth < 992) {
                        bsCollapse.hide();
                    }
                }
            });
        });
    });
</script>
