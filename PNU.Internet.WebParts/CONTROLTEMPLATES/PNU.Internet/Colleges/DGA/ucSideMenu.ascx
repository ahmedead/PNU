<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSideMenu.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA.ucSideMenu" %>



<div class="container py-5 entity-details-container">
    <div class="entity-details-layout d-flex flex-column gap-4 flex-lg-row gap-lg-3 align-items-start">

        <aside class="entity-details-sidemenu bg-body" aria-label="College menu">
            <button type="button"
                class="entity-details-sidemenu-toggle btn btn-link w-100 align-items-center gap-3 px-3 text-decoration-none has-rotatable-icon collapsed border-bottom"
                data-bs-toggle="collapse" data-bs-target="#entity-details-sidemenu-drawer" aria-expanded="false"
                aria-controls="entity-details-sidemenu-drawer">
                <i class="hgi hgi-stroke hgi-menu-02 fs-4 text-primary" aria-hidden="true"></i>
                <span id="spnCurrentItem" runat="server" clientidmode="Static"
                    class="entity-details-sidemenu-current flex-grow-1 text-start fw-semibold"></span>
                <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
            </button>

            <div class="entity-details-sidemenu-drawer collapse d-lg-block" id="entity-details-sidemenu-drawer">
                <nav class="entity-details-tabs nav flex-column align-items-stretch" id="faculty-details-tabs"
                    role="tablist">
                    <asp:Literal ID="ltrNav" runat="server" />
                </nav>
            </div>
        </aside>

        <div class="flex-grow-1 w-100">
            <div class="tab-content" id="faculty-details-tabs-content">
                <asp:PlaceHolder ID="phPanes" runat="server" />
            </div>
        </div>

    </div>
</div>

<script type="text/javascript">
    (function () {
        var TAB_KEY = 'pnuSideMenuActiveTab_' + window.location.pathname;

        function allTriggers() {
            return document.querySelectorAll('#faculty-details-tabs [data-bs-toggle="tab"]');
        }

        function activate(paneSelector) {
            var trigger = document.querySelector(
                '#faculty-details-tabs [data-bs-toggle="tab"][data-bs-target="' + paneSelector + '"]');
            if (!trigger || !window.bootstrap) return false;

            // Bootstrap only deactivates siblings inside the same nav level,
            // so clear everything manually first (2-level menu).
            allTriggers().forEach(function (b) {
                b.classList.remove('active');
                b.setAttribute('aria-selected', 'false');
            });
            document.querySelectorAll('#faculty-details-tabs-content > .tab-pane').forEach(function (p) {
                p.classList.remove('show', 'active');
            });

            trigger.classList.add('active');
            trigger.setAttribute('aria-selected', 'true');
            var pane = document.querySelector(paneSelector);
            if (pane) pane.classList.add('show', 'active');

            var cur = document.getElementById('spnCurrentItem');
            if (cur) {
                var label = trigger.querySelector('span');
                cur.textContent = (label ? label.textContent : trigger.textContent).trim();
            }
            return true;
        }

        document.addEventListener('DOMContentLoaded', function () {

            // Tab clicks (both root buttons and level-2 toc items are tab triggers)
            allTriggers().forEach(function (btn) {
                btn.addEventListener('click', function (evt) {
                    evt.preventDefault();
                    var paneSel = btn.getAttribute('data-bs-target');
                    activate(paneSel);
                    sessionStorage.setItem(TAB_KEY, paneSel);

                    // Sections sub-items: scroll to the department anchor inside the pane
                    var anchorId = btn.getAttribute('data-faculty-section-target');
                    if (anchorId) {
                        window.setTimeout(function () {
                            var el = document.getElementById(anchorId);
                            if (el) el.scrollIntoView({ behavior: 'smooth', block: 'start' });
                        }, 150);
                    }

                    // Close mobile drawer after selection
                    var drawer = document.getElementById('entity-details-sidemenu-drawer');
                    if (drawer && window.bootstrap && window.getComputedStyle(drawer).position !== 'static') {
                        var c = bootstrap.Collapse.getInstance(drawer);
                        if (c) c.hide();
                    }
                });
            });

            // Group headers: expand/collapse their sub-menu
            document.querySelectorAll('[data-entity-menu-target]').forEach(function (btn) {
                btn.addEventListener('click', function () {
                    var target = document.querySelector(btn.getAttribute('data-entity-menu-target'));
                    if (target && window.bootstrap) {
                        bootstrap.Collapse.getOrCreateInstance(target, { toggle: false }).toggle();
                        btn.classList.toggle('collapsed');
                        btn.setAttribute('aria-expanded',
                            btn.getAttribute('aria-expanded') === 'true' ? 'false' : 'true');
                    }
                });
            });

            // Restore the active tab after a postback / reload
            var saved = sessionStorage.getItem(TAB_KEY);
            if (saved && document.querySelector(saved)) {
                if (activate(saved)) {
                    // also expand the parent group of the saved item, if any
                    var trg = document.querySelector(
                        '#faculty-details-tabs [data-bs-toggle="tab"][data-bs-target="' + saved + '"]');
                    var sub = trg ? trg.closest('.toc-sub') : null;
                    if (sub && window.bootstrap) {
                        bootstrap.Collapse.getOrCreateInstance(sub, { toggle: false }).show();
                        var head = document.querySelector('[data-entity-menu-target="#' + sub.id + '"]');
                        if (head) { head.classList.remove('collapsed'); head.setAttribute('aria-expanded', 'true'); }
                    }
                }
            }
        });
    })();
</script>
