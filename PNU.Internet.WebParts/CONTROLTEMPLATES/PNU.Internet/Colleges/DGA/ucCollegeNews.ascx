<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeNews.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA.ucCollegeNews" %>





<%@ Import Namespace="PNU.Internet.WebParts" %>

<main id="main-content" class="dga-main-body" tabindex="-1">

    <div class="container page-padding">


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

        
    });
</script>




