<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllCollegesNew.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.ucAllCollegesNew" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>


<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">

        <div class="mt-4">
            <%-- Tabs Navigation --%>
            <ul class="nav nav-tabs nav-underline nav-flush w-100 mb-4" id="faculties-tabs" role="tablist">
                <asp:Repeater ID="masterRepeater" runat="server">
                    <ItemTemplate>
                        <li class="nav-item" role="presentation">
                            <button
                                class='<%# "nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2" + (Eval("ID").ToString() == "0" ? " active" : "") %>'
                                id='<%# "faculties-tabs-tab-" + Eval("ID") %>'
                                data-bs-toggle="tab"
                                data-bs-target='<%# "#faculties-tabs-pane-" + Eval("ID") %>'
                                type="button"
                                role="tab"
                                aria-controls='<%# "faculties-tabs-pane-" + Eval("ID") %>'
                                aria-selected='<%# Eval("ID").ToString() == "0" ? "true" : "false" %>'>
                                <i class="hgi hgi-stroke hgi-book-open-02 fs-5 fw-light" aria-hidden="true"></i>
                                <span><%# SPFactory.GetLocalizedTitle(Eval("COLL_CLASS_AR"), Eval("COLL_CLASS_EN")) %></span>
                            </button>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>

            <%-- Tabs Content --%>
            <div class="tab-content pt-2" id="faculties-tabs-content">
                <asp:Repeater ID="masterRepeater1" runat="server">
                    <ItemTemplate>
                        <div class='<%# "tab-pane fade" + (Eval("ID").ToString() == "0" ? " show active paginated-pane" : "") %>'
                             id='<%# "faculties-tabs-pane-" + Eval("ID") %>'
                             role="tabpanel"
                             aria-labelledby='<%# "faculties-tabs-tab-" + Eval("ID") %>'
                             tabindex="0"
                             data-tab-id='<%# Eval("ID") %>'>

                            <div class="row g-4 colleges-grid">
                                <asp:Repeater ID="rptCourses" runat="server" DataSource='<%# Eval("Colleges") %>'>
                                    <ItemTemplate>
                                        <div class="col-12 col-lg-4 col-md-6 college-item">
                                            <div class="card nav-card h-100">
                                                <div class="d-flex card-body flex-column gap-4">
                                                    <div class="icon-container">
                                                        <i class='<%# Eval("ClassName") %>' aria-hidden="true"></i>
                                                    </div>
                                                    <div>
                                                        <h3 class="card-title">
                                                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                        </h3>
                                                        <p class="card-text" id='<%# "paragraph_" + Container.ItemIndex %>'>
                                                            <%# SPFactory.GetLocalizedTitle(Eval("DescriptionDisplay"), Eval("DescriptionDisplay_EN")) %>
                                                        </p>
                                                    </div>
                                                    <div class="d-flex justify-content-end mt-auto">
                                                        <a class="btn btn-secondary stretched-link"
                                                           href='<%# String.Format("{1}Faculties/{0}/Pages/speech.aspx", Eval("Code"), SPFactory.GetSiteURL()) %>'
                                                           aria-label='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                                                            <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <%-- Pagination only for "All Colleges" tab (ID = 0) --%>
                            <asp:Panel runat="server" Visible='<%# Eval("ID").ToString() == "0" %>'>
                                <dga-paginator>
                                    <div class="mt-5 p-2 d-flex justify-content-center">
                                        <nav aria-label="قائمة التنقل في الصفحات">
                                            <ul class="pagination" id="colleges-pagination">
                                                <%-- Built dynamically by JS --%>
                                            </ul>
                                        </nav>
                                    </div>
                                </dga-paginator>
                            </asp:Panel>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

    </div>
</main>


<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#faculties-tabs button[data-bs-target="#faculties-tabs-pane-' + tabno + '"]').tab('show');
        });
    }

</script>

<script>
    document.addEventListener('DOMContentLoaded', function () {

        // ---------- 1) Truncate long descriptions ----------
        var paragraphs = document.querySelectorAll('p[id^="paragraph_"]');
        paragraphs.forEach(function (paragraph) {
            var text = paragraph.textContent.trim();
            var words = text.split(' ');
            if (words.length > 20) {
                paragraph.textContent = words.slice(0, 20).join(' ') + '...';
            }
        });

        // ---------- 2) Pagination for the "All Colleges" tab ----------
        var itemsPerPage = 9;
        var paginatedPane = document.querySelector('.paginated-pane');
        if (!paginatedPane) return;

        var items = paginatedPane.querySelectorAll('.college-item');
        var paginationContainer = document.getElementById('colleges-pagination');
        if (!items.length || !paginationContainer) return;

        var totalPages = Math.ceil(items.length / itemsPerPage);
        var currentPage = 1;

        function showPage(page) {
            currentPage = page;
            var start = (page - 1) * itemsPerPage;
            var end = start + itemsPerPage;

            items.forEach(function (item, idx) {
                item.style.display = (idx >= start && idx < end) ? '' : 'none';
            });

            buildPagination();
        }

        function buildPagination() {
            paginationContainer.innerHTML = '';

            // Previous button
            var prevLi = document.createElement('li');
            prevLi.className = 'page-item';
            var prevLink = document.createElement('a');
            prevLink.href = '#';
            prevLink.className = 'page-link btn btn-secondary icon-btn navigation-link' + (currentPage === 1 ? ' disabled' : '');
            prevLink.setAttribute('aria-label', 'previous');
            prevLink.innerHTML = '<span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i></span>';
            prevLink.addEventListener('click', function (e) {
                e.preventDefault();
                if (currentPage > 1) showPage(currentPage - 1);
            });
            prevLi.appendChild(prevLink);
            paginationContainer.appendChild(prevLi);

            // Page numbers
            for (var i = 1; i <= totalPages; i++) {
                (function (pageNum) {
                    var li = document.createElement('li');
                    li.className = 'page-item';
                    var link = document.createElement('a');
                    link.href = '#';
                    link.className = 'page-link btn btn-secondary' + (pageNum === currentPage ? ' active' : '');
                    link.textContent = pageNum;
                    link.addEventListener('click', function (e) {
                        e.preventDefault();
                        showPage(pageNum);
                    });
                    li.appendChild(link);
                    paginationContainer.appendChild(li);
                })(i);
            }

            // Next button
            var nextLi = document.createElement('li');
            nextLi.className = 'page-item';
            var nextLink = document.createElement('a');
            nextLink.href = '#';
            nextLink.className = 'page-link btn btn-secondary icon-btn navigation-link' + (currentPage === totalPages ? ' disabled' : '');
            nextLink.setAttribute('aria-label', 'next');
            nextLink.innerHTML = '<span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i></span>';
            nextLink.addEventListener('click', function (e) {
                e.preventDefault();
                if (currentPage < totalPages) showPage(currentPage + 1);
            });
            nextLi.appendChild(nextLink);
            paginationContainer.appendChild(nextLi);
        }

        if (totalPages > 1) {
            showPage(1);
        }
    });
</script>
