<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllAdvs.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.MediaCenter.ucAllAdvs" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>


<main id="main-content" class="dga-main-body" tabindex="-1">

    <div class="container page-padding">

        <%-- Search + filter bar --%>
        <dga-search-input>
            <div class="d-flex gap-3 mb-4" role="search" aria-label='<%# SPFactory.GetLocalizedTitle("ابحث في الإعلانات", "Search advertisements") %>'>
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input type="search" id="advsSearchInput" autocomplete="off" class="form-control"
                        placeholder='<%# SPFactory.GetLocalizedTitle("ابحث في الإعلانات", "Search advertisements") %>' value="" />
                </div>
                <button type="button" class="btn btn-primary" id="btnAdvsSearch" aria-label='<%# SPFactory.GetLocalizedTitle("بحث", "Search") %>'><%# SPFactory.GetLocalizedTitle("بحث", "Search") %></button>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                            data-bs-auto-close="outside" class="btn btn-dark gap-1" id="advsFilterToggle">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span><%# SPFactory.GetLocalizedTitle("تصفية", "Filter") %></span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu p-2" style="width: 20rem;">
                            <div class="m-1">

                                <%-- ===== SearchCategory filter section ===== --%>
                                <asp:Panel ID="pnlSearchCategoryFilter" runat="server" Visible="false">
                                    <p class="fw-semibold mb-2"><%# SPFactory.GetLocalizedTitle("الفئة", "Category") %></p>
                                    <div class="form-control-container has-icon">
                                        <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                            <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                        </span>
                                        <input type="text" autocomplete="off" class="form-control"
                                               id="advsSearchCatBoxSearch" placeholder='<%# SPFactory.GetLocalizedTitle("بحث", "Search") %>' />
                                    </div>
                                    <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto"
                                         style="max-height: 12.5rem;" id="advsSearchCatBox">
                                        <asp:Repeater ID="rptSearchCategoryFilter" runat="server">
                                            <ItemTemplate>
                                                <div class="form-check filter-row"
                                                     data-label='<%# SPFactory.GetLocalizedTitle(Eval("SearchCategory"), Eval("SearchCategory_EN")) %>'>
                                                    <input type="checkbox" class="form-check-input advs-searchcategory-filter"
                                                           id='<%# "advs-scat-filter-" + Eval("ID") %>'
                                                           data-searchcategory='<%# Eval("SearchCategory") %>' />
                                                    <label class="form-check-label" for='<%# "advs-scat-filter-" + Eval("ID") %>'>
                                                        <%# SPFactory.GetLocalizedTitle(Eval("SearchCategory"), Eval("SearchCategory_EN")) %>
                                                    </label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:Panel>

                                <hr />

                                <div class="d-flex justify-content-between align-content-stretch align-items-stretch gap-2">
                                    <button type="button" class="btn btn-primary" id="btnApplyAdvsFilter"><%# SPFactory.GetLocalizedTitle("تطبيق الاختيارات", "Apply filters") %></button>
                                    <button type="button" class="btn btn-secondary" id="btnResetAdvsFilter"><%# SPFactory.GetLocalizedTitle("إعادة تعيين", "Reset") %></button>
                                </div>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </dga-search-input>

        <%-- Advertisement cards grid --%>
        <div class="row g-4 paginated-content" id="advsGrid">
            <asp:Repeater ID="rptAdvs" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6 paginated-item advs-item"
                         data-searchcategory='<%# Eval("SearchCategory") %>'
                         data-title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                        <div class="card h-100 pnu-event-card">
                            <div class="card-body d-flex flex-column gap-3">
                                <div class="mt-0 d-flex flex-column gap-3">
                                    <div>
                                        <span class="px-3 py-1 icon-container fw-bold text-primary small"
                                              style="width: fit-content; height: fit-content;">
                                            <%# Eval("DayName") %>
                                        </span>
                                    </div>
                                    <small class="d-flex gap-2 align-items-center">
                                        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        <time datetime='<%# Eval("MediaDateISO") %>'><%# Eval("MediaDate") %></time>
                                    </small>
                                    <h3 class="card-title">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                    </h3>
                                </div>
                                <div class="mt-auto d-flex flex-column gap-4">
                                    <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(SPFactory.GetLocalizedTitle(Eval("SearchCategory"), Eval("SearchCategory_EN")) as string) %>'>
                                        <div class="d-flex flex-wrap mt-auto gap-2">
                                            <span class="badge badge-info">
                                                <%# SPFactory.GetLocalizedTitle(Eval("SearchCategory"), Eval("SearchCategory_EN")) %>
                                            </span>
                                        </div>
                                    </asp:PlaceHolder>
                                    <div class="d-flex justify-content-start">
                                        <a class="btn btn-primary" href='<%# Eval("DetailsURL") %>'>
                                            <%# SPFactory.GetLocalizedTitle("عرض التفاصيل", "View details") %>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%-- Pagination --%>
        <dga-paginator>
            <div class="mt-5 p-2 d-flex justify-content-center pagination-container">
                <nav aria-label='<%# SPFactory.GetLocalizedTitle("قائمة التنقل في الصفحات", "Pagination Navigation") %>'>
                    <ul class="pagination"></ul>
                </nav>
            </div>
        </dga-paginator>

    </div>
</main>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        var itemsPerPage = 9;
        var grid = document.getElementById('advsGrid');
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
            var searchInput = document.getElementById('advsSearchInput');
            var term = (searchInput && searchInput.value ? searchInput.value : '').trim().toLowerCase();

            var checkedSearchCats = Array.prototype.slice.call(document.querySelectorAll('.advs-searchcategory-filter:checked'))
                .map(function (c) { return c.getAttribute('data-searchcategory'); });

            filteredItems = allItems.filter(function (it) {
                var title = (it.getAttribute('data-title') || '').toLowerCase();
                var scat = it.getAttribute('data-searchcategory') || '';

                var matchesTerm = term === '' || title.indexOf(term) !== -1;
                var matchesSearchCat = checkedSearchCats.length === 0 || checkedSearchCats.indexOf(scat) !== -1;

                return matchesTerm && matchesSearchCat;
            });
            currentPage = 0;
            render();
        }

        function closeFilterDropdown() {
            var toggleBtn = document.getElementById('advsFilterToggle');
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
        wireBoxSearch('advsSearchCatBoxSearch', 'advsSearchCatBox');

        var btnApply = document.getElementById('btnApplyAdvsFilter');
        if (btnApply) btnApply.addEventListener('click', function () {
            applyFilter();
            closeFilterDropdown();
        });

        var btnReset = document.getElementById('btnResetAdvsFilter');
        if (btnReset) btnReset.addEventListener('click', function () {
            document.querySelectorAll('.advs-searchcategory-filter').forEach(function (c) { c.checked = false; });
            var si = document.getElementById('advsSearchInput');
            if (si) si.value = '';
            var c1 = document.getElementById('advsSearchCatBoxSearch'); if (c1) c1.value = '';
            document.querySelectorAll('.filter-row').forEach(function (row) { row.style.display = ''; });
            applyFilter();
            closeFilterDropdown();
        });

        var btnSearch = document.getElementById('btnAdvsSearch');
        if (btnSearch) btnSearch.addEventListener('click', applyFilter);

        var searchInput = document.getElementById('advsSearchInput');
        if (searchInput) searchInput.addEventListener('keyup', function (e) {
            if (e.key === 'Enter') applyFilter();
        });

        render();
    });
</script>
