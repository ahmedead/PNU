<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllNews.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.MediaCenter.ucAllNews" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>


<main id="main-content" class="dga-main-body" tabindex="-1">

    <div class="container page-padding">

        <%-- Search + filter bar --%>
        <dga-search-input>
            <div class="d-flex gap-3 mb-4" role="search" aria-label="ابحث في الأخبار">
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input type="search" id="newsSearchInput" autocomplete="off" class="form-control"
                        placeholder='<%# SPFactory.GetLocalizedTitle("ابحث في الأخبار", "Search news") %>' value="" />
                </div>
                <button type="button" class="btn btn-primary" id="btnNewsSearch" aria-label="بحث">بحث</button>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                            data-bs-auto-close="outside" class="btn btn-dark gap-1" id="newsFilterToggle">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span>تصفية</span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu p-2" style="width: 20rem;">
                            <div class="m-1">

                                <%-- ===== Faculty filter section ===== --%>
                                <asp:Panel ID="pnlFacultyFilter" runat="server" Visible="false">
                                    <p class="fw-semibold mb-2">
                                        <%# SPFactory.GetLocalizedTitle("الكلية", "Faculty") %>
                                    </p>
                                    <div class="form-control-container has-icon">
                                        <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                            <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                        </span>
                                        <input type="text" autocomplete="off" class="form-control"
                                               id="newsFacultyBoxSearch" placeholder="بحث" />
                                    </div>
                                    <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto"
                                         style="max-height: 12.5rem;" id="newsFacultyBox">
                                        <asp:Repeater ID="rptFacultyFilter" runat="server">
                                            <ItemTemplate>
                                                <div class="form-check filter-row"
                                                     data-label='<%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>'>
                                                    <input type="checkbox" class="form-check-input news-faculty-filter"
                                                           id='<%# "news-fac-filter-" + Eval("ID") %>'
                                                           data-faculty='<%# Eval("FacultyName") %>' />
                                                    <label class="form-check-label" for='<%# "news-fac-filter-" + Eval("ID") %>'>
                                                        <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                                    </label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:Panel>

                                <hr />

                                <div class="d-flex justify-content-between align-content-stretch align-items-stretch gap-2">
                                    <button type="button" class="btn btn-primary" id="btnApplyNewsFilter">تطبيق الاختيارات</button>
                                    <button type="button" class="btn btn-secondary" id="btnResetNewsFilter">إعادة تعيين</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </dga-search-input>

        <%-- News cards grid --%>
        <div class="row g-4 paginated-content" id="newsGrid">
            <asp:Repeater ID="rptNews" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6 paginated-item news-item"
                         data-faculty='<%# Eval("FacultyName") %>'
                         data-title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                        <article class="card h-100 pnu-news-card">
                            <div class="card-body d-flex flex-column placeholder-glow h-100">
                                <img width="400" height="250" class="w-100 rounded-2"
                                     alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                     loading="lazy" fetchpriority="low"
                                     src='<%# Eval("AttachmentURL") %>'
                                     sizes="(min-width: 1200px) 360px, (min-width: 768px) 45vw, 90vw"
                                     decoding="async" />
                                <div class="flex-grow-1">
                                    <h3 class="card-title">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                    </h3>
                                    <p class="card-text line-clamp max-clamp-line-4">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                                    </p>
                                </div>
                                <div class="mt-auto d-flex flex-column gap-4">
                                    <small class="d-flex gap-2 align-items-center">
                                        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        <time datetime='<%# Eval("MediaDateISO") %>'><%# Eval("MediaDate") %></time>
                                    </small>
                                    <div class="d-flex flex-wrap mt-auto gap-2">
                                        <span class="badge badge-info">
                                            <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                        </span>
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <a class="btn btn-primary" href='<%# Eval("DetailsURL") %>'>
                                            <%# SPFactory.GetLocalizedTitle("قراءة المزيد", "Read more") %>
                                        </a>
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
</main>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        var itemsPerPage = 6;
        var grid = document.getElementById('newsGrid');
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

        // ---------- Apply filters ----------
        function applyFilter() {
            var searchInput = document.getElementById('newsSearchInput');
            var term = (searchInput && searchInput.value ? searchInput.value : '').trim().toLowerCase();

            var checkedFaculties = Array.prototype.slice.call(document.querySelectorAll('.news-faculty-filter:checked'))
                .map(function (c) { return c.getAttribute('data-faculty'); });

            filteredItems = allItems.filter(function (it) {
                var title = (it.getAttribute('data-title') || '').toLowerCase();
                var fac = it.getAttribute('data-faculty') || '';

                var matchesTerm = term === '' || title.indexOf(term) !== -1;
                var matchesFac = checkedFaculties.length === 0 || checkedFaculties.indexOf(fac) !== -1;

                return matchesTerm && matchesFac;
            });
            currentPage = 0;
            render();
        }

        function closeFilterDropdown() {
            var toggleBtn = document.getElementById('newsFilterToggle');
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
        wireBoxSearch('newsFacultyBoxSearch', 'newsFacultyBox');

        var btnApply = document.getElementById('btnApplyNewsFilter');
        if (btnApply) btnApply.addEventListener('click', function () {
            applyFilter();
            closeFilterDropdown();
        });

        var btnReset = document.getElementById('btnResetNewsFilter');
        if (btnReset) btnReset.addEventListener('click', function () {
            document.querySelectorAll('.news-faculty-filter').forEach(function (c) { c.checked = false; });
            var si = document.getElementById('newsSearchInput');
            if (si) si.value = '';
            var c1 = document.getElementById('newsFacultyBoxSearch'); if (c1) c1.value = '';
            document.querySelectorAll('.filter-row').forEach(function (row) { row.style.display = ''; });
            applyFilter();
            closeFilterDropdown();
        });

        var btnSearch = document.getElementById('btnNewsSearch');
        if (btnSearch) btnSearch.addEventListener('click', applyFilter);

        var searchInput = document.getElementById('newsSearchInput');
        if (searchInput) searchInput.addEventListener('keyup', function (e) {
            if (e.key === 'Enter') applyFilter();
        });

        render();
    });
</script>
