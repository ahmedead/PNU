<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllDigitalMedia.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.MediaCenter.ucAllDigitalMedia" %>

<%@ Import Namespace="PNU.Internet.WebParts" %>


<main id="main-content" class="dga-main-body" tabindex="-1">

    <div class="container page-padding">

        <%-- Search + filter bar --%>
        <dga-search-input>
            <div class="d-flex gap-3 mb-4" role="search" aria-label="ابحث في الوسائط الرقمية">
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input type="search" id="mediaSearchInput" autocomplete="off" class="form-control"
                        placeholder='<%# SPFactory.GetLocalizedTitle("ابحث في الوسائط الرقمية", "Search digital media") %>' value="" />
                </div>
                <button type="button" class="btn btn-primary" id="btnMediaSearch" aria-label="بحث">بحث</button>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                            data-bs-auto-close="outside" class="btn btn-dark gap-1" id="mediaFilterToggle">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span>تصفية</span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu p-2" style="width: 20rem;">
                            <div class="m-1">

                                <%-- ===== Media Type filter section ===== --%>
                                <p class="fw-semibold mb-2">نوع الوسيط</p>
                                <div class="form-control-container has-icon">
                                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                    </span>
                                    <input type="text" autocomplete="off" class="form-control"
                                           id="mediaTypeBoxSearch" placeholder="بحث" />
                                </div>
                                <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto"
                                     style="max-height: 12.5rem;" id="mediaTypeBox">
                                    <div class="form-check filter-row" data-label="صورة">
                                        <input type="checkbox" class="form-check-input media-type-filter" id="media-filter-image" data-type="image" />
                                        <label class="form-check-label" for="media-filter-image">صورة</label>
                                    </div>
                                    <div class="form-check filter-row" data-label="فيديو">
                                        <input type="checkbox" class="form-check-input media-type-filter" id="media-filter-video" data-type="video" />
                                        <label class="form-check-label" for="media-filter-video">فيديو</label>
                                    </div>
                                </div>

                                <%-- ===== Faculty filter section ===== --%>
                                <asp:Panel ID="pnlFacultyFilter" runat="server" Visible="false">
                                    <hr />
                                    <p class="fw-semibold mb-2">
                                        <%# SPFactory.GetLocalizedTitle("الكلية", "Faculty") %>
                                    </p>
                                    <div class="form-control-container has-icon">
                                        <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                            <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                        </span>
                                        <input type="text" autocomplete="off" class="form-control"
                                               id="mediaFacultyBoxSearch" placeholder="بحث" />
                                    </div>
                                    <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto"
                                         style="max-height: 12.5rem;" id="mediaFacultyBox">
                                        <asp:Repeater ID="rptFacultyFilter" runat="server">
                                            <ItemTemplate>
                                                <div class="form-check filter-row"
                                                     data-label='<%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>'>
                                                    <input type="checkbox" class="form-check-input media-faculty-filter"
                                                           id='<%# "media-fac-filter-" + Eval("ID") %>'
                                                           data-faculty='<%# Eval("FacultyName") %>' />
                                                    <label class="form-check-label" for='<%# "media-fac-filter-" + Eval("ID") %>'>
                                                        <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                                    </label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:Panel>

                                <hr />

                                <div class="d-flex justify-content-between align-content-stretch align-items-stretch gap-2">
                                    <button type="button" class="btn btn-primary" id="btnApplyMediaFilter">تطبيق الاختيارات</button>
                                    <button type="button" class="btn btn-secondary" id="btnResetMediaFilter">إعادة تعيين</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </dga-search-input>

        <%-- Digital Media cards --%>
        <div class="row g-4 paginated-content" id="mediaGrid">
            <asp:Repeater ID="rptMedia" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6 paginated-item media-item"
                         data-type='<%# (bool)Eval("IsVideo") ? "video" : "image" %>'
                         data-faculty='<%# Eval("FacultyName") %>'
                         data-title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                        <article class="card h-100 pnu-news-card">
                            <div class="card-body d-flex flex-column placeholder-glow h-100">
                                <div class="media-card__cover">
                                    <img width="400" height="250" class="w-100 rounded-2"
                                         alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                         loading="lazy" fetchpriority="low"
                                         src='<%# Eval("AttachmentURL") %>'
                                         sizes="(min-width: 1200px) 360px, (min-width: 768px) 45vw, 90vw"
                                         decoding="async" />
                                </div>
                                <div class="flex-grow-1">
                                    <h3 class="card-title mb-0">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                    </h3>
                                </div>
                                <div class="mt-auto d-flex flex-column gap-4">
                                    <small class="d-flex gap-2 align-items-center">
                                        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        <time datetime='<%# Eval("MediaDateISO") %>'><%# Eval("MediaDate") %></time>
                                    </small>
                                    <div class="d-flex flex-wrap mt-auto gap-2">
                                        <span class='<%# "badge " + Eval("MediaBadgeClass") %>'>
                                            <%# Eval("MediaTypeLabel") %>
                                        </span>
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("IsVideo") %>'>
                                            <button class="btn btn-primary video-preview-trigger" type="button"
                                                data-bs-toggle="modal" data-bs-target="#videoPreviewModal"
                                                data-video-title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                                data-video-src='<%# Eval("VideoURL") %>'>عرض</button>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" Visible='<%# !(bool)Eval("IsVideo") %>'>
                                            <button class="btn btn-primary media-preview-trigger" type="button"
                                                data-bs-toggle="modal" data-bs-target="#mediaPreviewModal"
                                                data-media-title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                                data-media-src='<%# Eval("AttachmentURL") %>'>عرض</button>
                                        </asp:PlaceHolder>
                                    </div>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%-- Pagination --%>
        <dga-paginator>
            <div class="mt-5 p-2 d-flex justify-content-center pagination-container">
                <nav aria-label="قائمة التنقل في الصفحات">
                    <ul class="pagination"></ul>
                </nav>
            </div>
        </dga-paginator>

    </div>

    <%-- Image preview modal --%>
    <div class="modal fade" id="mediaPreviewModal" tabindex="-1" aria-labelledby="mediaPreviewModalTitle" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header p-3">
                    <h3 class="modal-title h5 m-0 p-0" id="mediaPreviewModalTitle">الوسائط الرقمية</h3>
                    <button type="button" class="btn btn-secondary icon-btn" data-bs-dismiss="modal" aria-label="إغلاق">
                        <span class="d-inline-flex fs-5">
                            <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
                        </span>
                    </button>
                </div>
                <div class="modal-body px-3 py-0 pb-3">
                    <img id="mediaPreviewImage" class="img-fluid w-100 rounded-3" src="" alt="" />
                </div>
            </div>
        </div>
    </div>

    <%-- Video preview modal --%>
    <div class="modal fade" id="videoPreviewModal" tabindex="-1" aria-labelledby="videoPreviewModalTitle" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header p-3">
                    <h3 class="modal-title h5 m-0 p-0" id="videoPreviewModalTitle">فيديو</h3>
                    <button type="button" class="btn btn-secondary icon-btn" data-bs-dismiss="modal" aria-label="إغلاق">
                        <span class="d-inline-flex fs-5">
                            <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
                        </span>
                    </button>
                </div>
                <div class="modal-body px-3 py-0 pb-3">
                    <div class="ratio ratio-16x9 overflow-hidden rounded-3 bg-dark">
                        <iframe id="videoPreviewFrame" title="فيديو" src="" loading="lazy"
                            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                            allowfullscreen referrerpolicy="strict-origin-when-cross-origin"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </div>
</main>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        var itemsPerPage = 6;
        var grid = document.getElementById('mediaGrid');
        if (!grid) return;

        var allItems = Array.prototype.slice.call(grid.querySelectorAll('.paginated-item'));
        var filteredItems = allItems.slice();
        var paginationList = document.querySelector('.pagination-container .pagination');
        var currentPage = 0;

        // ---------- Compact pagination ----------
        function render() {
            var totalPages = Math.ceil(filteredItems.length / itemsPerPage);
            paginationList.innerHTML = '';

            allItems.forEach(function (it) { it.style.display = 'none'; });
            for (var i = currentPage * itemsPerPage; i < Math.min(filteredItems.length, (currentPage + 1) * itemsPerPage); i++) {
                filteredItems[i].style.display = '';
            }

            if (totalPages <= 1) return;

            addPageItem('<', currentPage - 1, currentPage > 0, false);
            var pages = buildPageList(currentPage, totalPages);
            pages.forEach(function (p) {
                if (p === '...') { addDots(); }
                else { addPageItem((p + 1).toString(), p, true, p === currentPage); }
            });
            addPageItem('>', currentPage + 1, currentPage < totalPages - 1, false);
        }

        function buildPageList(current, total) {
            var windowSize = 1;
            var pages = [];
            var first = 0;
            var last = total - 1;
            var start = Math.max(current - windowSize, first + 1);
            var end = Math.min(current + windowSize, last - 1);

            pages.push(first);
            if (start > first + 1) pages.push('...');
            for (var i = start; i <= end; i++) pages.push(i);
            if (end < last - 1) pages.push('...');
            if (last > first) pages.push(last);
            return pages;
        }

        function addPageItem(text, pageIndex, enabled, isActive) {
            var li = document.createElement('li');
            li.className = 'page-item' + (isActive ? ' active' : '') + (!enabled ? ' disabled' : '');
            var a = document.createElement('a');
            a.className = 'page-link btn btn-secondary' + (isActive ? ' active' : '');
            a.href = '#';
            a.innerText = text;
            a.addEventListener('click', function (e) {
                e.preventDefault();
                if (!enabled) return;
                currentPage = pageIndex;
                render();
                grid.scrollIntoView({ behavior: 'smooth', block: 'start' });
            });
            li.appendChild(a);
            paginationList.appendChild(li);
        }

        function addDots() {
            var li = document.createElement('li');
            li.className = 'page-item disabled';
            var span = document.createElement('span');
            span.className = 'page-link btn btn-secondary';
            span.innerText = '...';
            li.appendChild(span);
            paginationList.appendChild(li);
        }

        // ---------- Filter ----------
        function applyFilter() {
            var searchInput = document.getElementById('mediaSearchInput');
            var term = (searchInput && searchInput.value ? searchInput.value : '').trim().toLowerCase();

            var types = Array.prototype.slice.call(document.querySelectorAll('.media-type-filter:checked'))
                .map(function (c) { return c.getAttribute('data-type'); });

            var checkedFaculties = Array.prototype.slice.call(document.querySelectorAll('.media-faculty-filter:checked'))
                .map(function (c) { return c.getAttribute('data-faculty'); });

            filteredItems = allItems.filter(function (it) {
                var title = (it.getAttribute('data-title') || '').toLowerCase();
                var type = it.getAttribute('data-type') || '';
                var fac = it.getAttribute('data-faculty') || '';

                var matchesTerm = term === '' || title.indexOf(term) !== -1;
                var matchesType = types.length === 0 || types.indexOf(type) !== -1;
                var matchesFac = checkedFaculties.length === 0 || checkedFaculties.indexOf(fac) !== -1;

                return matchesTerm && matchesType && matchesFac;
            });
            currentPage = 0;
            render();
        }

        function closeFilterDropdown() {
            var toggleBtn = document.getElementById('mediaFilterToggle');
            if (!toggleBtn) return;
            if (window.bootstrap && window.bootstrap.Dropdown) {
                var inst = window.bootstrap.Dropdown.getInstance(toggleBtn) || new window.bootstrap.Dropdown(toggleBtn);
                inst.hide();
                return;
            }
            if (window.jQuery) {
                window.jQuery(toggleBtn).dropdown('hide');
                return;
            }
            toggleBtn.setAttribute('aria-expanded', 'false');
            toggleBtn.classList.remove('show');
            var menu = toggleBtn.parentElement.querySelector('.dropdown-menu');
            if (menu) menu.classList.remove('show');
        }

        function wireBoxSearch(inputId, boxId) {
            var inp = document.getElementById(inputId);
            var box = document.getElementById(boxId);
            if (!inp || !box) return;
            inp.addEventListener('keyup', function () {
                var q = inp.value.trim().toLowerCase();
                var rows = box.querySelectorAll('.filter-row');
                rows.forEach(function (row) {
                    var label = (row.getAttribute('data-label') || '').toLowerCase();
                    row.style.display = (q === '' || label.indexOf(q) !== -1) ? '' : 'none';
                });
            });
            inp.addEventListener('click', function (e) { e.stopPropagation(); });
        }
        wireBoxSearch('mediaTypeBoxSearch', 'mediaTypeBox');
        wireBoxSearch('mediaFacultyBoxSearch', 'mediaFacultyBox');

        var btnApply = document.getElementById('btnApplyMediaFilter');
        if (btnApply) btnApply.addEventListener('click', function () {
            applyFilter();
            closeFilterDropdown();
        });

        var btnReset = document.getElementById('btnResetMediaFilter');
        if (btnReset) btnReset.addEventListener('click', function () {
            document.querySelectorAll('.media-type-filter').forEach(function (c) { c.checked = false; });
            document.querySelectorAll('.media-faculty-filter').forEach(function (c) { c.checked = false; });
            var si = document.getElementById('mediaSearchInput');
            if (si) si.value = '';
            var c1 = document.getElementById('mediaTypeBoxSearch'); if (c1) c1.value = '';
            var c2 = document.getElementById('mediaFacultyBoxSearch'); if (c2) c2.value = '';
            document.querySelectorAll('.filter-row').forEach(function (row) { row.style.display = ''; });
            applyFilter();
            closeFilterDropdown();
        });

        var btnSearch = document.getElementById('btnMediaSearch');
        if (btnSearch) btnSearch.addEventListener('click', applyFilter);

        var searchInput = document.getElementById('mediaSearchInput');
        if (searchInput) searchInput.addEventListener('keyup', function (e) {
            if (e.key === 'Enter') applyFilter();
        });

        render();

        // ---------- Modal wiring ----------
        var imgModal = document.getElementById('mediaPreviewModal');
        if (imgModal) {
            imgModal.addEventListener('show.bs.modal', function (event) {
                var btn = event.relatedTarget;
                if (!btn) return;
                var src = btn.getAttribute('data-media-src');
                var title = btn.getAttribute('data-media-title');
                var imgEl = document.getElementById('mediaPreviewImage');
                var titleEl = document.getElementById('mediaPreviewModalTitle');
                if (imgEl) { imgEl.src = src || ''; imgEl.alt = title || ''; }
                if (titleEl && title) titleEl.textContent = title;
            });
        }

        var vidModal = document.getElementById('videoPreviewModal');
        if (vidModal) {
            vidModal.addEventListener('show.bs.modal', function (event) {
                var btn = event.relatedTarget;
                if (!btn) return;
                var src = btn.getAttribute('data-video-src');
                var title = btn.getAttribute('data-video-title');
                var frameEl = document.getElementById('videoPreviewFrame');
                var titleEl = document.getElementById('videoPreviewModalTitle');
                if (frameEl) frameEl.src = src || '';
                if (titleEl && title) titleEl.textContent = title;
            });
            vidModal.addEventListener('hidden.bs.modal', function () {
                var frameEl = document.getElementById('videoPreviewFrame');
                if (frameEl) frameEl.src = '';
            });
        }
    });
</script>
