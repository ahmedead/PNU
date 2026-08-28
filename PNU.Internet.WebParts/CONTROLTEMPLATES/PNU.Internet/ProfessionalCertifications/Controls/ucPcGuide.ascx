<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPcGuide.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls.ucPcGuide" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">
        <dga-search-input>
            <form id="professionalCertificationsFilterForm" class="d-flex flex-column flex-md-row gap-3 mb-4 w-100" role="search" aria-label="ابحث عن شهادة احترافية" onsubmit="return false;">
                <div class="form-control-container has-icon flex-grow-1">
                    <label class="visually-hidden" for="professionalCertificationSearch">البحث في الشهادات الاحترافية</label>
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input class="form-control" type="search" id="professionalCertificationSearch" autocomplete="off" placeholder="ابحث باسم الشهادة أو الجهة المانحة أو القسم" oninput="applyCertificationFilters()">
                </div>
                <button class="btn btn-primary flex-shrink-0" type="button" onclick="applyCertificationFilters()" aria-label="بحث">بحث</button>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" id="professionalCertificationsFilterToggle" data-bs-toggle="dropdown" aria-expanded="false" data-bs-auto-close="outside" class="btn btn-dark gap-1">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span>تصفية</span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu px-2" style="width: 20rem;" aria-labelledby="professionalCertificationsFilterToggle">
                            <p class="fw-semibold">الكلية</p>
                            <div class="form-control-container has-icon">
                                <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                    <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                </span>
                                <label class="visually-hidden" for="professionalCertificationFilterSearch">البحث في الكليات</label>
                                <input type="text" autocomplete="off" class="form-control" id="professionalCertificationFilterSearch" placeholder="ابحث عن كلية" oninput="filterCollegeDropdownOptions()">
                            </div>
                            <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto" style="max-height: 12.5rem;" id="professionalCertificationCollegeFilters">
                                <asp:Repeater ID="rptColleges" runat="server">
                                    <ItemTemplate>
                                        <div class="form-check" data-filter-option="<%# Container.DataItem.ToString().ToLower() %>">
                                            <input type="checkbox" class="form-check-input" id="college-chk-<%# Container.ItemIndex %>" value="<%# Container.DataItem %>" onchange="applyCertificationFilters()" />
                                            <label class="form-check-label" for="college-chk-<%# Container.ItemIndex %>"><%# Container.DataItem %></label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <hr>
                            <div class="d-flex justify-content-between gap-2 pb-2 px-2">
                                <button type="button" class="btn btn-primary" onclick="applyCertificationFilters()">تطبيق الاختيارات</button>
                                <button type="button" class="btn btn-secondary" onclick="resetCertificationFilters()">إعادة تعيين</button>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </form>
        </dga-search-input>

        <div class="d-flex align-items-center justify-content-between gap-3 mb-4" aria-live="polite">
            <h2 class="mb-0" id="professional-certifications-results-title">الشهادات الاحترافية</h2>
            <span class="text-body-secondary" id="professionalCertificationsCount">
                <asp:Literal ID="litCountText" runat="server" />
            </span>
        </div>

        <div class="row g-4" id="professionalCertificationsGrid">
            <asp:Repeater ID="rptCertifications" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-6 col-lg-4 certification-card-item"
                         data-college='<%# Eval("college") %>'
                         data-department='<%# Eval("department") %>'
                         data-program='<%# Eval("Program") %>'
                         data-title-ar='<%# Eval("title_ar") %>'
                         data-title-en='<%# Eval("title_en") %>'
                         data-provider='<%# Eval("provider") %>'
                         data-description='<%# Eval("description") %>'
                         data-level='<%# Eval("level") %>'
                         data-language='<%# Eval("language") %>'
                         data-cost='<%# Eval("cost") %>'
                         data-mode='<%# Eval("mode") %>'
                         data-is-supported='<%# Eval("is_supported") %>'
                         data-topics='<%# Eval("topics") %>'
                         data-requirements='<%# Eval("requirements") %>'
                         data-registration-steps='<%# Eval("registration_steps") %>'
                         data-resources='<%# Eval("resources") %>'
                         data-validity='<%# Eval("validity") %>'
                         data-course1='<%# Eval("course_1") %>'
                         data-course2='<%# Eval("course_2") %>'
                         data-course3='<%# Eval("course_3") %>'
                         data-course4='<%# Eval("course_4") %>'
                         data-link='<%# Eval("link") %>'>
                        <article class="card h-100" id="professional-certification-card-<%# Container.ItemIndex + 1 %>">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <span class="d-inline-flex fs-3">
                                        <i class="hgi hgi-stroke hgi-certificate-01 fs-3" aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div>
                                    <h3 class="card-title"><%# string.IsNullOrEmpty(Convert.ToString(Eval("title_ar"))) ? Eval("title_en") : Eval("title_ar") %></h3>
                                    <p class="card-text line-clamp max-clamp-line-4"><%# string.IsNullOrEmpty(Convert.ToString(Eval("description"))) ? "لا يوجد وصف متاح." : Eval("description") %></p>
                                </div>
                                <div class="d-flex flex-column gap-2 small text-body-secondary">
                                    <div class="d-flex gap-2 align-items-start">
                                        <i class="hgi hgi-stroke hgi-award-01 fs-5 text-primary flex-shrink-0" aria-hidden="true"></i>
                                        <span><strong>الجهة المانحة: </strong><%# string.IsNullOrEmpty(Convert.ToString(Eval("provider"))) ? "غير محدد" : Eval("provider") %></span>
                                    </div>
                                    <div class="d-flex gap-2 align-items-start">
                                        <i class="hgi hgi-stroke hgi-building-01 fs-5 text-primary flex-shrink-0" aria-hidden="true"></i>
                                        <span><strong>الكلية والقسم: </strong><%# Eval("college") %> - <%# Eval("department") %></span>
                                    </div>
                                    <div class="d-flex gap-2 align-items-start">
                                        <i class="hgi hgi-stroke hgi-money-03 fs-5 text-primary flex-shrink-0" aria-hidden="true"></i>
                                        <span><strong>السعر: </strong><%# string.IsNullOrEmpty(Convert.ToString(Eval("cost"))) ? "غير محدد" : Eval("cost") %></span>
                                    </div>
                                    <div class="d-flex gap-2 align-items-start">
                                        <i class="hgi hgi-stroke hgi-globe-02 fs-5 text-primary flex-shrink-0" aria-hidden="true"></i>
                                        <span><strong>اللغة: </strong><%# string.IsNullOrEmpty(Convert.ToString(Eval("language"))) ? "غير محدد" : Eval("language") %></span>
                                    </div>
                                </div>
                                <div class="d-flex flex-wrap mt-auto gap-2">
                                    <span class="badge badge-info"><%# string.IsNullOrEmpty(Convert.ToString(Eval("level"))) ? "غير محدد" : Eval("level") %></span>
                                </div>
                                <div class="d-flex gap-3 flex-wrap">
                                    <button type="button" class="btn btn-primary btn-certification-details" onclick="openCertificationModal(this)">عرض التفاصيل</button>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="text-center py-5" id="professionalCertificationsEmptyState" hidden>
            <img src="/Style Library/DGA/public/images/empty.svg" width="180" height="135" alt="" aria-hidden="true" loading="lazy" decoding="async">
            <h3 class="h5 mt-4" id="professionalCertificationsEmptyTitle">لا توجد نتائج مطابقة</h3>
            <p class="mb-0 text-body-secondary" id="professionalCertificationsEmptyText">جرّب تغيير خيارات التصفية أو عبارة البحث.</p>
        </div>
    </div>
</main>

<div class="modal fade" id="professionalCertificationModal" tabindex="-1" aria-labelledby="professionalCertificationModalTitle" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header p-3">
                <div>
                    <h2 class="modal-title h4 mb-1" id="professionalCertificationModalTitle"></h2>
                    <p class="mb-0 text-body-secondary" id="professionalCertificationModalSubtitle" dir="ltr"></p>
                </div>
                <button type="button" class="btn btn-secondary icon-btn" data-bs-dismiss="modal" aria-label="إغلاق">
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
                                        <span class="small text-body-secondary d-block">الجهة المانحة</span>
                                        <strong class="text-dark d-block" data-certification-field="provider"></strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-chart-increase fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block">المستوى</span>
                                        <strong class="text-dark d-block" data-certification-field="level"></strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-money-03 fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block">التكلفة</span>
                                        <strong class="text-dark d-block" data-certification-field="cost"></strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-globe-02 fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block">اللغة</span>
                                        <strong class="text-dark d-block" data-certification-field="language"></strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-laptop fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block">آلية التنفيذ</span>
                                        <strong class="text-dark d-block" data-certification-field="mode"></strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100">
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class="hgi hgi-stroke hgi-award-01 fs-4 text-primary" aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <span class="small text-body-secondary d-block">دعم هدف</span>
                                        <strong class="text-dark d-block" data-certification-field="is_supported"></strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row g-4">
                    <div class="col-12 professional-certification-detail" data-detail-key="description"><h3 class="h5">نبذة عن الشهادة</h3><p class="mb-0 professional-certification-preserve-lines" data-detail-value></p></div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="requirements"><h3 class="h5">المتطلبات</h3><p class="mb-0 professional-certification-preserve-lines" data-detail-value></p></div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="topics"><h3 class="h5">الموضوعات</h3><ul class="mb-0" data-detail-value></ul></div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="validity"><h3 class="h5">صلاحية الشهادة</h3><p class="mb-0 professional-certification-preserve-lines" data-detail-value></p></div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="registration_steps"><h3 class="h5">إجراءات التقديم</h3><p class="mb-0 professional-certification-preserve-lines" data-detail-value></p></div>
                    <div class="col-12 col-lg-6 professional-certification-detail" data-detail-key="resources"><h3 class="h5">المصادر التعليمية</h3><p class="mb-0 professional-certification-preserve-lines" data-detail-value></p></div>
                    <div class="col-12 col-lg-6 professional-certification-detail" id="professionalCertificationCoursesBlock"><h3 class="h5">المقررات المرتبطة</h3><div class="d-flex flex-wrap gap-2" id="professionalCertificationCourses"></div></div>
                </div>
            </div>
            <div class="modal-footer p-3">
                <button type="button" class="btn btn-secondary gap-2" id="printSingleCertificationButton" onclick="printSingleCertification()">
                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-printer" aria-hidden="true"></i></span>
                    <span>تصدير تفاصيل الشهادة</span>
                </button>
                <a class="btn btn-primary" id="professionalCertificationOfficialLink" href="#" target="_blank" rel="noopener" hidden>
                    <span>زيارة الموقع الرسمي</span>
                    <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-link-square-01" aria-hidden="true"></i></span>
                </a>
            </div>
        </div>
    </div>
</div>

<div id="certificationsPrintList" class="container py-4" dir="rtl" style="display:none;">
    <div class="text-center border-bottom border-3 pb-3 mb-4">
        <h1 class="h3 mb-2">تقرير الشهادات الاحترافية</h1>
        <p class="mb-1">جامعة الأميرة نورة بنت عبدالرحمن</p>
        <p class="small mb-0" id="certificationsPrintSubtitle"></p>
    </div>
    <table class="table table-bordered">
        <thead><tr><th>الكلية</th><th>القسم</th><th>اسم الشهادة</th><th>الجهة المانحة</th><th>المستوى</th></tr></thead>
        <tbody id="certificationsPrintBody"></tbody>
    </table>
</div>

<div id="certificationPrintDetail" class="container py-4" dir="rtl" style="display:none;"></div>

<script type="text/javascript">
    var activeSelectedCard = null;

    function applyCertificationFilters() {
        var searchInput = document.getElementById('professionalCertificationSearch');
        var query = (searchInput ? searchInput.value : '').trim().toLowerCase();

        var checkedBoxes = document.querySelectorAll('#professionalCertificationCollegeFilters input[type="checkbox"]:checked');
        var selectedColleges = [];
        for (var i = 0; i < checkedBoxes.length; i++) {
            selectedColleges.push(checkedBoxes[i].value);
        }

        var cards = document.querySelectorAll('.certification-card-item');
        var visibleCount = 0;

        for (var j = 0; j < cards.length; j++) {
            var card = cards[j];
            var college = card.getAttribute('data-college') || '';
            var dept = card.getAttribute('data-department') || '';
            var titleAr = card.getAttribute('data-title-ar') || '';
            var titleEn = card.getAttribute('data-title-en') || '';
            var provider = card.getAttribute('data-provider') || '';
            var program = card.getAttribute('data-program') || '';

            var matchesCollege = (selectedColleges.length === 0) || (selectedColleges.indexOf(college) !== -1);
            var textContent = (titleAr + ' ' + titleEn + ' ' + provider + ' ' + dept + ' ' + college + ' ' + program).toLowerCase();
            var matchesSearch = !query || (textContent.indexOf(query) !== -1);

            if (matchesCollege && matchesSearch) {
                card.style.display = '';
                visibleCount++;
            } else {
                card.style.display = 'none';
            }
        }

        var countBadge = document.getElementById('professionalCertificationsCount');
        if (countBadge) countBadge.textContent = visibleCount + ' شهادة معروضة';

        var emptyState = document.getElementById('professionalCertificationsEmptyState');
        if (emptyState) emptyState.hidden = (visibleCount > 0);
    }

    function filterCollegeDropdownOptions() {
        var filterSearch = document.getElementById('professionalCertificationFilterSearch');
        var query = (filterSearch ? filterSearch.value : '').trim().toLowerCase();
        var options = document.querySelectorAll('#professionalCertificationCollegeFilters [data-filter-option]');

        for (var i = 0; i < options.length; i++) {
            var opt = options[i];
            var val = opt.getAttribute('data-filter-option') || '';
            opt.style.display = (!query || val.indexOf(query) !== -1) ? '' : 'none';
        }
    }

    function resetCertificationFilters() {
        var searchInput = document.getElementById('professionalCertificationSearch');
        if (searchInput) searchInput.value = '';

        var filterSearch = document.getElementById('professionalCertificationFilterSearch');
        if (filterSearch) filterSearch.value = '';

        var checkedBoxes = document.querySelectorAll('#professionalCertificationCollegeFilters input[type="checkbox"]');
        for (var i = 0; i < checkedBoxes.length; i++) {
            checkedBoxes[i].checked = false;
        }

        filterCollegeDropdownOptions();
        applyCertificationFilters();
    }

    function openCertificationModal(btn) {
        var card = btn.closest('.certification-card-item');
        if (!card) return;

        activeSelectedCard = card;

        var titleAr = card.getAttribute('data-title-ar') || '';
        var titleEn = card.getAttribute('data-title-en') || '';

        var titleElem = document.getElementById('professionalCertificationModalTitle');
        if (titleElem) titleElem.textContent = titleAr || titleEn;

        var subtitleElem = document.getElementById('professionalCertificationModalSubtitle');
        if (subtitleElem) subtitleElem.textContent = (titleEn && titleEn !== titleAr) ? titleEn : '';

        var setFact = function(key, attr) {
            var target = document.querySelector('#professionalCertificationModal [data-certification-field="' + key + '"]');
            if (target) target.textContent = card.getAttribute('data-' + attr) || 'غير محدد';
        };

        setFact('provider', 'provider');
        setFact('level', 'level');
        setFact('cost', 'cost');
        setFact('language', 'language');
        setFact('mode', 'mode');
        setFact('is_supported', 'is-supported');

        var setDetail = function(key, attr, fallback) {
            var block = document.querySelector('#professionalCertificationModal [data-detail-key="' + key + '"]');
            if (block) {
                var target = block.querySelector('[data-detail-value]');
                if (target) target.textContent = card.getAttribute('data-' + attr) || fallback || 'غير محدد';
            }
        };

        setDetail('description', 'description');
        setDetail('requirements', 'requirements', 'لا يوجد');
        setDetail('validity', 'validity');
        setDetail('registration_steps', 'registration-steps');
        setDetail('resources', 'resources');

        var topicsList = document.querySelector('#professionalCertificationModal [data-detail-key="topics"] [data-detail-value]');
        if (topicsList) {
            topicsList.innerHTML = '';
            var rawTopics = card.getAttribute('data-topics') || '';
            var topics = rawTopics.split(/\r?\n|•|\s+-\s+/).map(function(t) { return t.trim().replace(/^[\d٠-٩]+[./]\s*/, ''); }).filter(Boolean);
            (topics.length ? topics : ['غير محدد']).forEach(function(topic) {
                var li = document.createElement('li');
                li.textContent = topic;
                topicsList.appendChild(li);
            });
        }

        var coursesContainer = document.getElementById('professionalCertificationCourses');
        var coursesBlock = document.getElementById('professionalCertificationCoursesBlock');
        if (coursesContainer && coursesBlock) {
            coursesContainer.innerHTML = '';
            var c1 = card.getAttribute('data-course1') || '';
            var c2 = card.getAttribute('data-course2') || '';
            var c3 = card.getAttribute('data-course3') || '';
            var c4 = card.getAttribute('data-course4') || '';
            var courses = [c1, c2, c3, c4].filter(Boolean);
            coursesBlock.hidden = (courses.length === 0);
            courses.forEach(function(c) {
                var badge = document.createElement('span');
                badge.className = 'badge badge-neutral';
                badge.textContent = c;
                coursesContainer.appendChild(badge);
            });
        }

        var officialLink = document.getElementById('professionalCertificationOfficialLink');
        var linkVal = (card.getAttribute('data-link') || '').trim();
        if (officialLink) {
            if (linkVal && (linkVal.indexOf('http://') === 0 || linkVal.indexOf('https://') === 0)) {
                officialLink.hidden = false;
                officialLink.href = linkVal;
            } else {
                officialLink.hidden = true;
            }
        }

        var modalElem = document.getElementById('professionalCertificationModal');
        if (modalElem && typeof bootstrap !== 'undefined' && bootstrap.Modal) {
            bootstrap.Modal.getOrCreateInstance(modalElem).show();
        }
    }

    function printSingleCertification() {
        if (!activeSelectedCard) return;
        var card = activeSelectedCard;
        var printDetail = document.getElementById('certificationPrintDetail');
        if (!printDetail) return;

        printDetail.innerHTML = '';

        var titleAr = card.getAttribute('data-title-ar') || '';
        var titleEn = card.getAttribute('data-title-en') || '';
        var college = card.getAttribute('data-college') || '';
        var dept = card.getAttribute('data-department') || '';

        var header = document.createElement('div');
        header.className = 'text-center border-bottom border-3 pb-3 mb-4';
        header.innerHTML = '<h1 class="h3 mb-2">' + (titleAr || titleEn) + '</h1>' +
                           '<p class="mb-1" dir="ltr">' + titleEn + '</p>' +
                           '<p class="small mb-0">' + college + ' - ' + dept + '</p>';

        printDetail.appendChild(header);

        var modalElem = document.getElementById('professionalCertificationModal');
        if (modalElem && typeof bootstrap !== 'undefined' && bootstrap.Modal) {
            bootstrap.Modal.getOrCreateInstance(modalElem).hide();
        }

        printDetail.style.display = 'block';
        window.print();
        printDetail.style.display = 'none';
    }

    document.addEventListener('DOMContentLoaded', function() {
        var exportBtn = document.getElementById('printCertificationListButton');
        if (exportBtn) {
            exportBtn.addEventListener('click', function() {
                var visibleCards = document.querySelectorAll('.certification-card-item:not([style*="display: none"])');
                var tbody = document.getElementById('certificationsPrintBody');
                if (!tbody) return;

                tbody.innerHTML = '';
                for (var i = 0; i < visibleCards.length; i++) {
                    var card = visibleCards[i];
                    var tr = document.createElement('tr');
                    tr.innerHTML = '<td>' + (card.getAttribute('data-college') || '-') + '</td>' +
                                   '<td>' + (card.getAttribute('data-department') || '-') + '</td>' +
                                   '<td>' + (card.getAttribute('data-title-ar') || card.getAttribute('data-title-en') || '-') + '</td>' +
                                   '<td>' + (card.getAttribute('data-provider') || '-') + '</td>' +
                                   '<td>' + (card.getAttribute('data-level') || '-') + '</td>';
                    tbody.appendChild(tr);
                }

                var sub = document.getElementById('certificationsPrintSubtitle');
                if (sub) sub.textContent = 'عدد النتائج: ' + visibleCards.length;

                var printListElem = document.getElementById('certificationsPrintList');
                if (printListElem) printListElem.style.display = 'block';
                window.print();
                if (printListElem) printListElem.style.display = 'none';
            });
        }
    });
</script>
