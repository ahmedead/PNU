<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllCollegeNews.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucAllCollegeNews" %>





<%@ Import Namespace="PNU.Internet.WebParts" %>

<main id="main-content" class="dga-main-body" tabindex="-1">

    <div class="container page-padding">

        <%-- Search + filter bar (client-side only, no code-behind changes) --%>
        <dga-search-input>
            <div class="d-flex gap-3 mb-4" role="search" aria-label="ابحث في الوسائط">
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input type="text" id="mediaSearchInput" autocomplete="off" class="form-control"
                        placeholder='<%# SPFactory.GetLocalizedTitle("ابحث في الوسائط", "Search media") %>' value="" />
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
                                <p class="fw-semibold mb-2">
                                    <%# SPFactory.GetLocalizedTitle("نوع الوسائط", "Media type") %>
                                </p>
                                <div class="d-flex flex-column gap-2 px-2 py-2" id="mediaTypeBox">
                                    <div class="form-check">
                                        <input type="checkbox" class="form-check-input media-type-filter"
                                               id="media-filter-image" data-isvideo="False" />
                                        <label class="form-check-label" for="media-filter-image">
                                            <%# SPFactory.GetLocalizedTitle("صورة", "Image") %>
                                        </label>
                                    </div>
                                    <div class="form-check">
                                        <input type="checkbox" class="form-check-input media-type-filter"
                                               id="media-filter-video" data-isvideo="True" />
                                        <label class="form-check-label" for="media-filter-video">
                                            <%# SPFactory.GetLocalizedTitle("فيديو", "Video") %>
                                        </label>
                                    </div>
                                </div>
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

        <%-- Media cards grid --%>
        <div class="row g-4 paginated-content" id="mediaGrid">
            <asp:Repeater ID="rptNews" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6 paginated-item media-item"
                         data-isvideo='<%# Eval("IsVideo") %>'
                         data-title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                        <article class="card h-100 pnu-news-card">
                            <div class="card-body d-flex flex-column placeholder-glow h-100">
                                <div class="media-card__cover">
                                    <img runat="server" id="dv_newsImg" width="400" height="250"
                                         class="w-100 rounded-2"
                                         alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                         loading="lazy" fetchpriority="low"
                                         src='<%# Eval("AttachmentURL") %>'
                                         sizes="(min-width: 1200px) 360px, (min-width: 768px) 45vw, 90vw"
                                         decoding="async"
                                         style='<%# ((Eval("IsVideo").ToString() != "False") ? "display:none;" : string.Empty) %>' />
                                    <%--<div class="video-wrapper video-container" id="divVideo" runat="server" visible='<%# Eval("IsVideo") %>'>
                                        <video class="object-fit-cover for-thumbnail rounded-2 w-100" style="height: 250px; max-width: 100%; pointer-events: none;">
                                            <source src="<%# Eval("VideoURL") %>" />
                                        </video>
                                    </div>--%>
                                </div>
                                <div class="flex-grow-1">
                                    <h3 class="card-title">
                                        <a href='<%# Eval("DetailsURL") %>' class="stretched-link text-decoration-none text-body">
                                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                        </a>
                                    </h3>
                                    <p class="card-text line-clamp max-clamp-line-4">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                                    </p>
                                </div>
                                <div class="mt-auto d-flex flex-column gap-4">
                                    <small class="d-flex gap-2 align-items-center">
                                        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        <time><%# Eval("MediaDate") %></time>
                                    </small>
                                    <div class="d-flex flex-wrap mt-auto gap-2">
                                        <span class='<%# ((Eval("IsVideo").ToString() != "False") ? "badge badge-success" : "badge badge-info") %>'>
                                            <%# Eval("MediaTypes") %>
                                        </span>
                                        <span class="badge badge-info">
                                            <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                        </span>
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <a class="btn btn-primary" href='<%# Eval("DetailsURL") %>'>
                                            <%# SPFactory.GetLocalizedTitle("عرض", "View") %>
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

        // ---------- Apply filters ----------
        function applyFilter() {
            var searchInput = document.getElementById('mediaSearchInput');
            var term = (searchInput && searchInput.value ? searchInput.value : '').trim().toLowerCase();

            var checkedTypes = Array.prototype.slice.call(document.querySelectorAll('.media-type-filter:checked'))
                .map(function (c) { return c.getAttribute('data-isvideo'); });

            filteredItems = allItems.filter(function (it) {
                var title = (it.getAttribute('data-title') || '').toLowerCase();
                var isVideo = it.getAttribute('data-isvideo') || 'False';

                var matchesTerm = term === '' || title.indexOf(term) !== -1;
                var matchesType = checkedTypes.length === 0 || checkedTypes.indexOf(isVideo) !== -1;

                return matchesTerm && matchesType;
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

        var btnApply = document.getElementById('btnApplyMediaFilter');
        if (btnApply) btnApply.addEventListener('click', function () {
            applyFilter();
            closeFilterDropdown();
        });

        var btnReset = document.getElementById('btnResetMediaFilter');
        if (btnReset) btnReset.addEventListener('click', function () {
            document.querySelectorAll('.media-type-filter').forEach(function (c) { c.checked = false; });
            var si = document.getElementById('mediaSearchInput');
            if (si) si.value = '';
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
    });
</script>




