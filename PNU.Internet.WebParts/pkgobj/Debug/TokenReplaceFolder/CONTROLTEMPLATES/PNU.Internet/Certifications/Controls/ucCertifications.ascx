<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCertifications.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls.ucCertifications, PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>


<style>
    #certificationsPrintList,
    #certificationPrintDetail { display: none; }

    .professional-certification-preserve-lines { white-space: pre-line; }

    @media print {
        @page { margin: 14mm; }

        /* The two print containers are MOVED to be direct children of <body> just
           before window.print() (see movePrintHostToBody in the script below): inside
           the SharePoint form they are nested many levels deep, and the "hide every
           other child of body" rule below would hide their own ancestors with them -
           which is what produced a blank print page. */
        body.printing-certification-list,
        body.printing-certification-detail {
            background: #fff !important;
            height: auto !important;
            overflow: visible !important;
            position: static !important;
            -webkit-print-color-adjust: exact;
            print-color-adjust: exact;
        }

        body.printing-certification-list > :not(#certificationsPrintList),
        body.printing-certification-detail > :not(#certificationPrintDetail) {
            display: none !important;
        }

        body.printing-certification-list #certificationsPrintList,
        body.printing-certification-detail #certificationPrintDetail {
            display: block !important;
            visibility: visible !important;
            position: static !important;
            height: auto !important;
            overflow: visible !important;
        }

        /* SharePoint chrome: the scrolling workspace clips anything that prints. */
        body.printing-certification-list #s4-workspace,
        body.printing-certification-detail #s4-workspace,
        body.printing-certification-list #s4-bodyContainer,
        body.printing-certification-detail #s4-bodyContainer {
            height: auto !important;
            overflow: visible !important;
            position: static !important;
        }

        .professional-certification-print-table {
            border-collapse: collapse;
            table-layout: fixed;
            width: 100%;
        }

        .professional-certification-print-table th,
        .professional-certification-print-table td {
            border: 1px solid #6c757d;
            font-size: 10pt;
            padding: 8px;
            text-align: right;
            vertical-align: top;
            word-break: break-word;
        }

        .professional-certification-print-table th { background: #f1f3f5 !important; }
        .professional-certification-print-section { break-inside: avoid; }
    }
</style>

<%-- ============================ page header ============================ --%>
<asp:Panel ID="pnlHeader" runat="server" CssClass="bg-primary-25 py-5">
    <div class="container">
        <asp:Panel ID="pnlBreadcrumb" runat="server">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-2">
                    <li class="breadcrumb-item small"><a href='<%# HomeUrl %>'><%# HomeLabel %></a></li>
                    <li class="breadcrumb-item small active" aria-current="page"><span><%# PageHeading %></span></li>
                </ol>
            </nav>
        </asp:Panel>

        <div class="d-flex flex-column flex-lg-row align-items-start justify-content-between gap-3">
            <h1 class="mb-0 fw-semibold h2"><%# PageHeading %></h1>
            <button class="btn btn-outline-secondary flex-shrink-0 gap-2" type="button" id="printCertificationListButton">
                <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-printer" aria-hidden="true"></i></span>
                <span><%# ExportListLabel %></span>
            </button>
        </div>

        <div class="col-12 col-lg-10 mt-4">
            <p><%# IntroLine1 %></p>
            <p class="mb-0"><%# IntroLine2 %></p>
        </div>
    </div>
</asp:Panel>

<%-- ============================== guide =============================== --%>
<section id="secCertifications" runat="server" class="dga-main-body" aria-labelledby="professional-certifications-results-title">
    <div class="container page-padding">

        <dga-search-input>
            <%-- a <div>, not a <form>: the whole page already lives inside the
                 SharePoint server-side form and browsers drop nested forms. --%>
            <div id="professionalCertificationsFilterForm" class="d-flex gap-3 mb-4" role="search" aria-label='<%# SearchAriaLabel %>'>
                <div class="form-control-container has-icon">
                    <label class="visually-hidden" for="professionalCertificationSearch"><%# SearchLabel %></label>
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input class="form-control" type="search" id="professionalCertificationSearch" autocomplete="off" placeholder='<%# SearchPlaceholder %>'>
                </div>
                <button class="btn btn-primary" type="button" id="professionalCertificationSearchButton" aria-label='<%# SearchButtonLabel %>'><%# SearchButtonLabel %></button>

                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" id="professionalCertificationsFilterToggle" data-bs-toggle="dropdown" aria-expanded="false" data-bs-auto-close="outside" class="btn btn-dark gap-1">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span><%# FilterLabel %></span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu px-2" style="width: 20rem;" aria-labelledby="professionalCertificationsFilterToggle">
                            <p class="fw-semibold"><%# CollegeLabel %></p>
                            <div class="form-control-container has-icon">
                                <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                    <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                </span>
                                <label class="visually-hidden" for="professionalCertificationFilterSearch"><%# CollegeSearchLabel %></label>
                                <input type="text" autocomplete="off" class="form-control" id="professionalCertificationFilterSearch" placeholder='<%# CollegeSearchPlaceholder %>'>
                            </div>
                            <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto" style="max-height: 12.5rem;" id="professionalCertificationCollegeFilters"></div>
                            <hr>
                            <div class="d-flex justify-content-between gap-2 pb-2 px-2">
                                <button type="button" class="btn btn-primary" id="applyProfessionalCertificationFilters"><%# ApplyLabel %></button>
                                <button type="button" class="btn btn-secondary" id="resetProfessionalCertifications"><%# ResetLabel %></button>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </dga-search-input>

        <div class="d-flex align-items-center justify-content-between gap-3 mb-4" aria-live="polite">
            <h2 class="mb-0" id="professional-certifications-results-title"><%# ResultsHeading %></h2>
            <span class="text-body-secondary" id="professionalCertificationsCount"></span>
        </div>

        <div class="row g-4" id="professionalCertificationsGrid"></div>

        <div class="text-center py-5" id="professionalCertificationsEmptyState" hidden>
            <img src='<%# EmptyImageUrl %>' width="180" height="135" alt="" aria-hidden="true" loading="lazy" decoding="async">
            <h3 class="h5 mt-4" id="professionalCertificationsEmptyTitle"><%# EmptyTitle %></h3>
            <p class="mb-0 text-body-secondary" id="professionalCertificationsEmptyText"><%# EmptyText %></p>
        </div>
    </div>
</section>

<%-- ============================== modal =============================== --%>
<div class="modal fade" id="professionalCertificationModal" tabindex="-1" aria-labelledby="professionalCertificationModalTitle" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header p-3">
                <div>
                    <h2 class="modal-title h4 mb-1" id="professionalCertificationModalTitle"></h2>
                    <p class="mb-0 text-body-secondary" id="professionalCertificationModalSubtitle" dir="ltr"></p>
                </div>
                <button type="button" class="btn btn-secondary icon-btn" data-bs-dismiss="modal" aria-label='<%# CloseLabel %>'>
                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i></span>
                </button>
            </div>

            <div class="modal-body p-4">
                <div class="card overflow-hidden mb-4">
                    <div class="row row-cols-1 row-cols-md-3 g-0" id="professionalCertificationFacts">

                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-building-03 fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block"><%# ProviderLabel %></span>
                                        <strong class="text-dark d-block" data-certification-field="provider"></strong>
                                    </div>
                                </div>
                            </div>
                            <span class="position-absolute start-0 end-0 bottom-0 border-bottom" aria-hidden="true"></span>
                        </div>

                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-chart-increase fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block"><%# LevelLabel %></span>
                                        <strong class="text-dark d-block" data-certification-field="level"></strong>
                                    </div>
                                </div>
                            </div>
                            <span class="position-absolute start-0 end-0 bottom-0 border-bottom" aria-hidden="true"></span>
                            <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                        </div>

                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-money-03 fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block"><%# CostLabel %></span>
                                        <strong class="text-dark d-block" data-certification-field="cost"></strong>
                                    </div>
                                </div>
                            </div>
                            <span class="position-absolute start-0 end-0 bottom-0 border-bottom" aria-hidden="true"></span>
                            <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                        </div>

                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-globe-02 fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block"><%# LanguageLabel %></span>
                                        <strong class="text-dark d-block" data-certification-field="language"></strong>
                                    </div>
                                </div>
                            </div>
                            <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                        </div>

                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-laptop fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block"><%# ModeLabel %></span>
                                        <strong class="text-dark d-block" data-certification-field="mode"></strong>
                                    </div>
                                </div>
                            </div>
                            <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                            <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                        </div>

                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-award-01 fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block"><%# SupportLabel %></span>
                                        <strong class="text-dark d-block" data-certification-field="is_supported"></strong>
                                    </div>
                                </div>
                            </div>
                            <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                        </div>
                    </div>
                </div>

                <div class="row g-4">
                    <div class="col-12 professional-certification-detail" data-detail-key="description">
                        <h3 class="h5"><%# DescriptionHeading %></h3>
                        <p class="mb-0 professional-certification-preserve-lines" data-detail-value></p>
                    </div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="requirements">
                        <h3 class="h5"><%# RequirementsHeading %></h3>
                        <p class="mb-0 professional-certification-preserve-lines" data-detail-value></p>
                    </div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="topics">
                        <h3 class="h5"><%# TopicsHeading %></h3>
                        <ul class="mb-0" data-detail-value></ul>
                    </div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="validity">
                        <h3 class="h5"><%# ValidityHeading %></h3>
                        <p class="mb-0 professional-certification-preserve-lines" data-detail-value></p>
                    </div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="registration_steps">
                        <h3 class="h5"><%# RegistrationHeading %></h3>
                        <p class="mb-0 professional-certification-preserve-lines" data-detail-value></p>
                    </div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="resources">
                        <h3 class="h5"><%# ResourcesHeading %></h3>
                        <p class="mb-0 professional-certification-preserve-lines" data-detail-value></p>
                    </div>
                    <div class="col-12 col-lg-6 professional-certification-detail" id="professionalCertificationCoursesBlock">
                        <h3 class="h5"><%# CoursesHeading %></h3>
                        <div class="d-flex flex-wrap gap-2" id="professionalCertificationCourses"></div>
                    </div>
                </div>
            </div>

            <div class="modal-footer p-3">
                <button type="button" class="btn btn-secondary gap-2" id="printSingleCertificationButton">
                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-printer" aria-hidden="true"></i></span>
                    <span><%# ExportDetailLabel %></span>
                </button>
                <a class="btn btn-primary" id="professionalCertificationOfficialLink" href="#" target="_blank" rel="noopener" hidden>
                    <span><%# OfficialSiteLabel %></span>
                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-link-square-01" aria-hidden="true"></i></span>
                </a>
            </div>
        </div>
    </div>
</div>

<%-- =========================== print views =========================== --%>
<div id="certificationsPrintList" class="container py-4" dir='<%# TextDirection %>'>
    <div class="text-center border-bottom border-3 pb-3 mb-4">
        <h1 class="h3 mb-2"><%# PrintReportTitle %></h1>
        <p class="mb-1"><%# UniversityName %></p>
        <p class="small mb-0" id="certificationsPrintSubtitle"></p>
    </div>
    <table class="professional-certification-print-table">
        <thead>
            <tr>
                <th><%# CollegeLabel %></th>
                <th><%# DepartmentLabel %></th>
                <th><%# CertificateNameLabel %></th>
                <th><%# ProviderLabel %></th>
                <th><%# LevelLabel %></th>
            </tr>
        </thead>
        <tbody id="certificationsPrintBody"></tbody>
    </table>
</div>

<div id="certificationPrintDetail" class="container py-4" dir='<%# TextDirection %>'></div>

<%-- ===================== data + labels payloads ====================== --%>
<script type="application/json" id="certificationsData"><asp:Literal ID="ltData" runat="server" /></script>
<script type="application/json" id="certificationsLabels"><asp:Literal ID="ltLabels" runat="server" /></script>

<script>
    (function () {
        'use strict';

        var readJson = function (id, fallback) {
            try {
                var node = document.getElementById(id);
                if (!node) return fallback;
                var raw = (node.textContent || node.innerText || '').trim();
                return raw ? JSON.parse(raw) : fallback;
            } catch (error) {
                if (window.console && console.error) console.error('certifications: ' + id, error);
                return fallback;
            }
        };

        var certifications = readJson('certificationsData', []);
        var labels = readJson('certificationsLabels', {});

        if (!Array.isArray(certifications)) certifications = [];

        var label = function (key, fallback) {
            return labels && labels[key] ? labels[key] : fallback;
        };

        var currentData = certifications.slice();
        var selectedCertification = null;

        var elements = {
            form: document.getElementById('professionalCertificationsFilterForm'),
            search: document.getElementById('professionalCertificationSearch'),
            searchButton: document.getElementById('professionalCertificationSearchButton'),
            filterToggle: document.getElementById('professionalCertificationsFilterToggle'),
            filterSearch: document.getElementById('professionalCertificationFilterSearch'),
            collegeFilters: document.getElementById('professionalCertificationCollegeFilters'),
            applyFilters: document.getElementById('applyProfessionalCertificationFilters'),
            reset: document.getElementById('resetProfessionalCertifications'),
            printList: document.getElementById('printCertificationListButton'),
            grid: document.getElementById('professionalCertificationsGrid'),
            count: document.getElementById('professionalCertificationsCount'),
            empty: document.getElementById('professionalCertificationsEmptyState'),
            emptyTitle: document.getElementById('professionalCertificationsEmptyTitle'),
            emptyText: document.getElementById('professionalCertificationsEmptyText'),
            modal: document.getElementById('professionalCertificationModal'),
            modalTitle: document.getElementById('professionalCertificationModalTitle'),
            modalSubtitle: document.getElementById('professionalCertificationModalSubtitle'),
            courses: document.getElementById('professionalCertificationCourses'),
            coursesBlock: document.getElementById('professionalCertificationCoursesBlock'),
            officialLink: document.getElementById('professionalCertificationOfficialLink'),
            printSingle: document.getElementById('printSingleCertificationButton'),
            printListHost: document.getElementById('certificationsPrintList'),
            printListBody: document.getElementById('certificationsPrintBody'),
            printListSubtitle: document.getElementById('certificationsPrintSubtitle'),
            printDetail: document.getElementById('certificationPrintDetail')
        };

        if (!elements.grid) return;

        var locale = label('locale', 'ar');

        var cleanText = function (value, fallback) {
            if (fallback === undefined) fallback = label('notSpecified', 'غير محدد');
            var normalized = String(value === null || value === undefined ? '' : value)
                .split('_x000D_').join('').trim();
            return normalized ? normalized : fallback;
        };

        var isWebLink = function (value) {
            var raw = String(value || '').trim().toLowerCase();
            return raw.indexOf('http://') === 0 || raw.indexOf('https://') === 0;
        };

        var lower = function (value) {
            return String(value === null || value === undefined ? '' : value).toLocaleLowerCase(locale);
        };

        var unique = function (values) {
            var seen = {};
            var list = [];
            for (var i = 0; i < values.length; i++) {
                var value = values[i];
                if (!value || seen[value]) continue;
                seen[value] = true;
                list.push(value);
            }
            return list.sort(function (first, second) { return first.localeCompare(second, locale); });
        };

        var selectedFilterValues = function (container) {
            var checked = container.querySelectorAll('input[type="checkbox"]:checked');
            var values = [];
            for (var i = 0; i < checked.length; i++) values.push(checked[i].value);
            return values;
        };

        var renderFilterOptions = function (container, values, prefix) {
            container.innerHTML = '';
            for (var index = 0; index < values.length; index++) {
                var value = values[index];

                var wrapper = document.createElement('div');
                wrapper.className = 'form-check';
                wrapper.setAttribute('data-filter-option', lower(value));

                var checkbox = document.createElement('input');
                checkbox.type = 'checkbox';
                checkbox.className = 'form-check-input';
                checkbox.id = prefix + '-' + index;
                checkbox.value = value;

                var text = document.createElement('label');
                text.className = 'form-check-label';
                text.htmlFor = checkbox.id;
                text.textContent = value;

                wrapper.appendChild(checkbox);
                wrapper.appendChild(text);
                container.appendChild(wrapper);
            }
        };

        var createMetaRow = function (icon, text, value) {
            var row = document.createElement('div');
            row.className = 'd-flex gap-2 align-items-start';

            var iconElement = document.createElement('i');
            iconElement.className = 'hgi hgi-stroke hgi-' + icon + ' fs-5 text-primary flex-shrink-0';
            iconElement.setAttribute('aria-hidden', 'true');

            var body = document.createElement('span');
            var strong = document.createElement('strong');
            strong.textContent = text + ': ';
            body.appendChild(strong);
            body.appendChild(document.createTextNode(cleanText(value)));

            row.appendChild(iconElement);
            row.appendChild(body);
            return row;
        };

        var renderCards = function (data) {
            elements.grid.innerHTML = '';
            elements.count.textContent = data.length + ' ' + label('countSuffix', 'شهادة معروضة');

            if (data.length === 0) {
                elements.grid.hidden = true;
                elements.empty.hidden = false;
                return;
            }

            elements.empty.hidden = true;
            elements.grid.hidden = false;

            var fragment = document.createDocumentFragment();

            for (var index = 0; index < data.length; index++) {
                (function (item, position) {
                    var column = document.createElement('div');
                    column.className = 'col-12 col-md-6 col-lg-4';

                    var article = document.createElement('article');
                    article.className = 'card h-100';
                    article.id = 'professional-certification-card-' + (position + 1);

                    var body = document.createElement('div');
                    body.className = 'card-body d-flex flex-column gap-4';

                    var iconContainer = document.createElement('div');
                    iconContainer.className = 'icon-container';
                    var iconWrapper = document.createElement('span');
                    iconWrapper.className = 'd-inline-flex fs-3';
                    var certificationIcon = document.createElement('i');
                    certificationIcon.className = 'hgi hgi-stroke hgi-certificate-01 fs-3';
                    certificationIcon.setAttribute('aria-hidden', 'true');
                    iconWrapper.appendChild(certificationIcon);
                    iconContainer.appendChild(iconWrapper);

                    var headingBlock = document.createElement('div');
                    var title = document.createElement('h3');
                    title.className = 'card-title';
                    title.textContent = cleanText(item.title_ar || item.title_en);

                    var description = document.createElement('p');
                    description.className = 'card-text line-clamp max-clamp-line-4';
                    description.textContent = cleanText(item.description, label('noDescription', 'لا يوجد وصف متاح.'));

                    headingBlock.appendChild(title);
                    headingBlock.appendChild(description);

                    var metadata = document.createElement('div');
                    metadata.className = 'd-flex flex-column gap-2 small text-body-secondary';
                    metadata.appendChild(createMetaRow('award-01', label('provider', 'الجهة المانحة'), item.provider));
                    metadata.appendChild(createMetaRow('building-01', label('collegeAndDepartment', 'الكلية والقسم'),
                        cleanText(item.college) + ' - ' + cleanText(item.department)));
                    metadata.appendChild(createMetaRow('money-03', label('cost', 'التكلفة'), item.cost));
                    metadata.appendChild(createMetaRow('globe-02', label('language', 'اللغة'), item.language));

                    var badges = document.createElement('div');
                    badges.className = 'd-flex flex-wrap mt-auto gap-2';
                    var levelBadge = document.createElement('span');
                    levelBadge.className = 'badge badge-info';
                    levelBadge.textContent = cleanText(item.level);
                    badges.appendChild(levelBadge);

                    var actions = document.createElement('div');
                    actions.className = 'd-flex gap-3 flex-wrap';
                    var detailsButton = document.createElement('button');
                    detailsButton.type = 'button';
                    detailsButton.className = 'btn btn-primary';
                    detailsButton.textContent = label('details', 'عرض التفاصيل');
                    detailsButton.onclick = function () { openModal(item); };
                    actions.appendChild(detailsButton);

                    body.appendChild(iconContainer);
                    body.appendChild(headingBlock);
                    body.appendChild(metadata);
                    body.appendChild(badges);
                    body.appendChild(actions);
                    article.appendChild(body);
                    column.appendChild(article);
                    fragment.appendChild(column);
                })(data[index], index);
            }

            elements.grid.appendChild(fragment);
        };

        var applyFilters = function () {
            var query = lower(elements.search.value.trim());
            var colleges = selectedFilterValues(elements.collegeFilters);

            currentData = certifications.filter(function (item) {
                if (colleges.length && colleges.indexOf(item.college) === -1) return false;
                if (!query) return true;

                var haystack = [item.title_ar, item.title_en, item.provider,
                item.department, item.college, item.Program];

                for (var i = 0; i < haystack.length; i++) {
                    if (lower(haystack[i]).indexOf(query) !== -1) return true;
                }
                return false;
            });

            renderCards(currentData);
        };

        var filterDropdownOptions = function () {
            var query = lower(elements.filterSearch.value.trim());
            var options = elements.collegeFilters.querySelectorAll('[data-filter-option]');
            for (var i = 0; i < options.length; i++) {
                var value = options[i].getAttribute('data-filter-option') || '';
                options[i].hidden = Boolean(query) && value.indexOf(query) === -1;
            }
        };

        var setDetailText = function (key, value, fallback) {
            var block = elements.modal.querySelector('[data-detail-key="' + key + '"]');
            if (!block) return;
            var target = block.querySelector('[data-detail-value]');
            if (target) target.textContent = cleanText(value, fallback);
        };

        var splitTopics = function (value) {
            return String(value || '')
                .split(/\r?\n|•|\s+-\s+/)
                .map(function (line) { return line.trim().replace(/^[\d٠-٩]+[./]\s*/, ''); })
                .filter(function (line) { return line.length > 0; });
        };

        var openModal = function (item) {
            selectedCertification = item;
            elements.modalTitle.textContent = cleanText(item.title_ar || item.title_en);
            elements.modalSubtitle.textContent =
                item.title_en && item.title_en !== item.title_ar ? item.title_en : '';

            var facts = elements.modal.querySelectorAll('[data-certification-field]');
            for (var i = 0; i < facts.length; i++) {
                facts[i].textContent = cleanText(item[facts[i].getAttribute('data-certification-field')]);
            }

            setDetailText('description', item.description);
            setDetailText('requirements', item.requirements, label('none', 'لا يوجد'));
            setDetailText('validity', item.validity);
            setDetailText('registration_steps', item.registration_steps);
            setDetailText('resources', item.resources);

            var topicsList = elements.modal.querySelector('[data-detail-key="topics"] [data-detail-value]');
            topicsList.innerHTML = '';
            var topics = splitTopics(item.topics);
            if (topics.length === 0) topics = [label('notSpecified', 'غير محدد')];
            for (var t = 0; t < topics.length; t++) {
                var listItem = document.createElement('li');
                listItem.textContent = topics[t];
                topicsList.appendChild(listItem);
            }

            elements.courses.innerHTML = '';
            var courses = [item.course_1, item.course_2, item.course_3, item.course_4]
                .filter(function (course) { return Boolean(course); });
            elements.coursesBlock.hidden = courses.length === 0;
            for (var c = 0; c < courses.length; c++) {
                var badge = document.createElement('span');
                badge.className = 'badge badge-neutral';
                badge.textContent = courses[c];
                elements.courses.appendChild(badge);
            }

            var link = String(item.link || '').trim();
            elements.officialLink.hidden = !isWebLink(link);
            if (isWebLink(link)) elements.officialLink.href = link;

            if (window.bootstrap && bootstrap.Modal) {
                bootstrap.Modal.getOrCreateInstance(elements.modal).show();
            }
        };

        var createPrintRow = function (text, value) {
            var row = document.createElement('p');
            row.className = 'mb-2';
            var strong = document.createElement('strong');
            strong.textContent = text + ': ';
            row.appendChild(strong);
            row.appendChild(document.createTextNode(cleanText(value)));
            return row;
        };

        var PRINT_MODES = ['printing-certification-list', 'printing-certification-detail'];

        var setBodyClass = function (name, on) {
            // classList.add/remove take ONE argument in IE11 - never pass a list.
            if (document.body.classList) {
                if (on) document.body.classList.add(name);
                else document.body.classList.remove(name);
                return;
            }

            var current = ' ' + (document.body.className || '') + ' ';
            var cleaned = current.split(' ' + name + ' ').join(' ');
            document.body.className = (on ? cleaned + name : cleaned).replace(/\s+/g, ' ').trim();
        };

        var clearPrintClasses = function () {
            for (var i = 0; i < PRINT_MODES.length; i++) setBodyClass(PRINT_MODES[i], false);
        };

        /**
         * Re-parents a print container to <body>.
         * The print stylesheet hides every child of <body> except the container, so the
         * container has to BE a child of body - inside the SharePoint form it is nested
         * under the ribbon, #s4-workspace and the web part zone, and all of those would
         * be hidden along with it. That is what printed a blank page.
         * Moving it is harmless: it stays display:none on screen, and a postback
         * re-renders the whole page anyway.
         */
        var movePrintHostToBody = function (node) {
            if (node && node.parentNode !== document.body) document.body.appendChild(node);
        };

        var preparePrint = function (node, mode) {
            movePrintHostToBody(node);
            clearPrintClasses();
            setBodyClass(mode, true);

            // Let the browser apply the class before it snapshots the page.
            window.setTimeout(function () {
                try { window.print(); }
                catch (error) { if (window.console && console.error) console.error(error); }

                // Safety net for browsers that never fire afterprint.
                window.setTimeout(clearPrintClasses, 1000);
            }, 50);
        };

        var printList = function () {
            if (currentData.length === 0) return;

            elements.printListBody.innerHTML = '';
            for (var i = 0; i < currentData.length; i++) {
                var item = currentData[i];
                var row = document.createElement('tr');
                var cells = [item.college, item.department, item.title_ar || item.title_en,
                item.provider, item.level];

                for (var c = 0; c < cells.length; c++) {
                    var cell = document.createElement('td');
                    cell.textContent = cleanText(cells[c], '-');
                    row.appendChild(cell);
                }
                elements.printListBody.appendChild(row);
            }

            elements.printListSubtitle.textContent =
                label('resultsCount', 'عدد النتائج: ') + currentData.length;
            preparePrint(elements.printListHost, 'printing-certification-list');
        };

        var printSingle = function () {
            if (!selectedCertification) return;
            var item = selectedCertification;
            elements.printDetail.innerHTML = '';

            var header = document.createElement('div');
            header.className = 'text-center border-bottom border-3 pb-3 mb-4';

            var title = document.createElement('h1');
            title.className = 'h3 mb-2';
            title.textContent = cleanText(item.title_ar || item.title_en);

            var subtitle = document.createElement('p');
            subtitle.className = 'mb-1';
            subtitle.dir = 'ltr';
            subtitle.textContent = item.title_en || '';

            var context = document.createElement('p');
            context.className = 'small mb-0';
            context.textContent = cleanText(item.college) + ' - ' + cleanText(item.department);

            header.appendChild(title);
            header.appendChild(subtitle);
            header.appendChild(context);

            var facts = document.createElement('div');
            facts.className = 'row row-cols-1 row-cols-2 g-3 mb-4';

            var factRows = [
                [label('provider', 'الجهة المانحة'), item.provider],
                [label('level', 'المستوى'), item.level],
                [label('language', 'اللغة'), item.language],
                [label('cost', 'التكلفة'), item.cost],
                [label('mode', 'آلية التنفيذ'), item.mode],
                [label('support', 'دعم هدف'), item.is_supported]
            ];

            for (var f = 0; f < factRows.length; f++) {
                var column = document.createElement('div');
                column.className = 'col';
                column.appendChild(createPrintRow(factRows[f][0], factRows[f][1]));
                facts.appendChild(column);
            }

            elements.printDetail.appendChild(header);
            elements.printDetail.appendChild(facts);

            var sections = [
                [label('descriptionHeading', 'نبذة عن الشهادة'), item.description],
                [label('topicsHeading', 'الموضوعات'), item.topics],
                [label('requirementsHeading', 'المتطلبات'), item.requirements],
                [label('validityHeading', 'صلاحية الشهادة'), item.validity],
                [label('registrationHeading', 'إجراءات التقديم'), item.registration_steps],
                [label('resourcesHeading', 'المصادر التعليمية'), item.resources],
                [label('coursesHeading', 'المقررات المرتبطة'),
                [item.course_1, item.course_2, item.course_3, item.course_4]
                    .filter(function (course) { return Boolean(course); }).join('، ')]
            ];

            for (var s = 0; s < sections.length; s++) {
                var section = document.createElement('section');
                section.className = 'professional-certification-print-section mb-4';

                var sectionTitle = document.createElement('h2');
                sectionTitle.className = 'h5 border-bottom pb-2';
                sectionTitle.textContent = sections[s][0];

                var paragraph = document.createElement('p');
                paragraph.className = 'mb-0 professional-certification-preserve-lines';
                paragraph.textContent = cleanText(sections[s][1]);

                section.appendChild(sectionTitle);
                section.appendChild(paragraph);
                elements.printDetail.appendChild(section);
            }

            if (window.bootstrap && bootstrap.Modal) {
                bootstrap.Modal.getOrCreateInstance(elements.modal).hide();
            }

            // Wait for the modal to finish closing: while it is open Bootstrap puts
            // .modal-open on <body> and a backdrop in front of everything.
            window.setTimeout(function () {
                preparePrint(elements.printDetail, 'printing-certification-detail');
            }, 300);
        };

        renderFilterOptions(
            elements.collegeFilters,
            unique(certifications.map(function (item) { return item.college; })),
            'professional-certification-college-filter');

        elements.searchButton.onclick = applyFilters;

        // Enter inside the box filters instead of posting the SharePoint form back.
        elements.search.onkeydown = function (event) {
            if ((event.key || '') !== 'Enter' && event.keyCode !== 13) return;
            if (event.preventDefault) event.preventDefault();
            applyFilters();
        };

        elements.filterSearch.oninput = filterDropdownOptions;

        elements.applyFilters.onclick = function () {
            applyFilters();
            if (window.bootstrap && bootstrap.Dropdown) {
                bootstrap.Dropdown.getOrCreateInstance(elements.filterToggle).hide();
            }
        };

        elements.reset.onclick = function () {
            elements.search.value = '';
            elements.filterSearch.value = '';
            var boxes = elements.collegeFilters.querySelectorAll('input[type="checkbox"]');
            for (var i = 0; i < boxes.length; i++) boxes[i].checked = false;
            currentData = certifications.slice();
            renderCards(currentData);
            filterDropdownOptions();
        };

        if (elements.printList) elements.printList.onclick = printList;
        if (elements.printSingle) elements.printSingle.onclick = printSingle;

        if (window.addEventListener) window.addEventListener('afterprint', clearPrintClasses);
        else if (window.attachEvent) window.attachEvent('onafterprint', clearPrintClasses);

        if (window.matchMedia) {
            var printQuery = window.matchMedia('print');
            var onQueryChange = function (query) { if (!query.matches) clearPrintClasses(); };

            if (printQuery.addEventListener) printQuery.addEventListener('change', onQueryChange);
            else if (printQuery.addListener) printQuery.addListener(onQueryChange);
        }

        renderCards(currentData);
    })();
</script>
