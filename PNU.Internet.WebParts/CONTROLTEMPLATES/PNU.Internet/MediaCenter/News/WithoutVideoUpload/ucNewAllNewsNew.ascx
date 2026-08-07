<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewAllNewsNew.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.WithoutVideoUpload.ucNewAllNewsNew" %>




<%@ Import Namespace="PNU.Internet.WebParts" %>


    <style>
        .pagination-container {
            margin-top: 20px;
        }
        .page-item.active .page-link {
            background-color: #007580;
            border-color: #007580;
            color: white;
        }
		
		li.page-item {
    padding: 10px;
}
    </style>

        <section class="py-5 mb-5">
            <div class="container">
                <div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">
                    <div class="col-12 col-lg-auto tabbable">
                        <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab" role="tablist">
                            <asp:Repeater ID="masterRepeater" runat="server">
                                <ItemTemplate>
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link" id='<%# "new" + Eval("ID") + "-tab" %>' data-bs-toggle="tab" data-bs-target='<%# "#new" + Eval("ID") + "-tab-pane" %>'
                                            type="button" role="tab" aria-controls='<%# "#new" + Eval("ID") + "-tab-pane" %>' aria-selected="false">
                                            <%# SPFactory.GetLocalizedTitle(Eval("MainCategory"), Eval("MainCategory_EN")) %>
                                        </button>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>

                <div class="row">
                    <div class="tab-content" id="myTabContent">
                        <asp:Repeater ID="detailsRepeater" runat="server">
                            <ItemTemplate>
                                <div class="tab-pane fade" id='<%# "new" + Eval("ID") + "-tab-pane" %>' role="tabpanel" aria-labelledby='<%# "new" + Eval("ID") + "-tab" %>' tabindex="0">
                                    <div class="row paginated-content">
                                        <asp:Repeater ID="rptNews" runat="server" DataSource='<%# Eval("Requests") %>'>
                                            <ItemTemplate>
                                                <div class="col-md-6 col-lg-4 mb-4 paginated-item">
                                                    <div class="card border rounded-4 h-100">
                                                        <div class="card-body p-2">
                                                            <div class="position-absolute top-0 start-0 m-4">
                                                                <span class="badge rounded-pill bg-tertiary text-white fs-6"><%# Eval("MediaTypes") %></span>
                                                            </div>
                                                            <img runat="server" id="dv_newsImg" src='<%# Eval("AttachmentURL") %>' class="object-fit-cover rounded-3 w-100" height="250" style='<%# ((Eval("IsVideo").ToString() == "True") ? "display:none": "display:block") %>'>
<!-- إذا كان IsVideo = true && IsYouTubeVideo = true -->
                                                            <div class="video-wrapper video-container"
                                                                id="divVideo"
                                                                runat="server"
                                                                visible='<%# Convert.ToBoolean(Eval("IsVideo")) && Convert.ToBoolean(Eval("IsYouTubeVideo")) %>'>
                                                                <a href="<%# Eval("VideoURL") %>" data-fancybox="video1" class="video__item" data-cat="1">
                                                                    <div class="video__cover">
                                                                        <img src='https://img.youtube.com/vi/<%# Eval("VideoID") %>/0.jpg'
                                                                            border="0"
                                                                            class="img-fluid"
                                                                            alt="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>">
                                                                        <div class="play--btn">
                                                                            <svg width="35" height="35" viewBox="0 0 45 45"></svg>
                                                                        </div>
                                                                    </div>
                                                                </a>
                                                            </div>

<!-- إذا كان IsVideo = true && IsYouTubeVideo = false -->
                                                            <div class="video-wrapper video-container"
                                                                id="div1"
                                                                runat="server"
                                                                visible='<%# Convert.ToBoolean(Eval("IsVideo")) && !Convert.ToBoolean(Eval("IsYouTubeVideo")) %>'>
                                                                <video class="object-fit-cover for-thumbnail rounded-3 w-100"
                                                                    style="height: 250px; max-width: 100%; pointer-events: none;">
                                                                    <source src="<%# Eval("VideoURL") %>" />
                                                                </video>
                                                            </div>




                                                            <div class="my-2 fs-4 fw-bold">
                                                                <a href='<%# Eval("DetailsURL") %>' class="stretched-link text-decoration-none text-body strong">
                                                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                                </a>
                                                            </div>
                                                            <div class="d-flex mb-3">

                                                                <svg width="19" height="20">
                                                                    <use xlink:href="#calendar"></use>
                                                                </svg>
                                                                <span class="ms-2 text-muted"><%# Eval("MediaDate") %></span>
                                                            </div>
                                                            <p class="text-muted">
                                                                <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                                                            </p>
                                                            <div class="mb-2">
                                                                <span class="badge rounded-pill bg-resonant-blue-100 text-secondary">
                                                                    <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <nav aria-label="Page navigation" class="pagination-container">
                                        <ul class="pagination justify-content-center"></ul>
                                    </nav>


                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                    </div>
                </div>

<div class="py-3 d-flex flex-wrap d-md-block">
                                        
                                    </div>
               <div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">
                    <div class="py-3 d-flex flex-wrap d-md-block">
                        <a href="/ar/NewsActivities/Pages/all-news.aspx" id="NewsActivities" class="btn btn-lg btn-outline-primary  px-5  me-3 mb-3  mb-md-5 flex-fill">أرشيف الأخبار </a>
                    </div>
                </div>
            </div>
        </section>
   


    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
<script>

    document.addEventListener('DOMContentLoaded', (event) => {
        const itemsPerPage = 6; // Number of items per page

        const tabPanes = document.querySelectorAll('.tab-pane');

        tabPanes.forEach(tabPane => {
            const paginatedItems = tabPane.querySelectorAll('.paginated-item');
            const totalItems = paginatedItems.length;
            const totalPages = Math.ceil(totalItems / itemsPerPage);

            const paginationContainer = tabPane.querySelector('.pagination-container');
            const paginationList = tabPane.querySelector('.pagination');
            paginationList.innerHTML = '';

            if (totalPages <= 1) {
                paginationContainer.style.display = 'none';
                return;
            }

            let currentPage = 0; // Default current page is 0 (1st page)

            // First render of pagination buttons
            renderPagination(currentPage, totalPages);

            showPage(tabPane, currentPage, itemsPerPage);

            function renderPagination(currentPage, totalPages) {
                paginationList.innerHTML = ''; // Clear the existing pagination

                // Previous button
                addPageItem('<', currentPage - 1, currentPage > 0);

                // First page
                addPageItem('1', 0, true);

                if (currentPage > 2) {
                    addPageDots();
                }

                // Middle pages around the current page
                for (let i = Math.max(1, currentPage - 1); i <= Math.min(currentPage + 1, totalPages - 2); i++) {
                    addPageItem((i + 1).toString(), i, true);
                }

                if (currentPage < totalPages - 3) {
                    addPageDots();
                }

                // Last page
                if (totalPages > 1) {
                    addPageItem(totalPages.toString(), totalPages - 1, true);
                }

                // Next button
                addPageItem('>', currentPage + 1, currentPage < totalPages - 1);
            }

            function addPageItem(text, pageIndex, isEnabled) {
                const pageItem = document.createElement('li');
                pageItem.className = 'page-item ' + (pageIndex === currentPage ? 'active' : '');
                if (!isEnabled) {
                    pageItem.classList.add('disabled');
                }

                const pageLink = document.createElement('a');
                pageLink.className = 'page-link';
                pageLink.href = '#';
                pageLink.innerText = text;

                pageLink.addEventListener('click', (event) => {
                    event.preventDefault();
                    if (!isEnabled) return;

                    currentPage = pageIndex; // Set the current page to the clicked page
                    renderPagination(currentPage, totalPages); // Re-render the pagination with the new page selected
                    showPage(tabPane, currentPage, itemsPerPage); // Show the corresponding page items
                });

                pageItem.appendChild(pageLink);
                paginationList.appendChild(pageItem);
            }

            function addPageDots() {
                const dotsItem = document.createElement('li');
                dotsItem.className = 'page-item disabled';
                const dotsLink = document.createElement('a');
                dotsLink.className = 'page-link';
                dotsLink.href = '#';
                dotsLink.innerText = '...';
                dotsItem.appendChild(dotsLink);
                paginationList.appendChild(dotsItem);
            }

            function showPage(tabPane, pageIndex, itemsPerPage) {
                const paginatedItems = tabPane.querySelectorAll('.paginated-item');

                paginatedItems.forEach((item, index) => {
                    item.style.display = (index >= pageIndex * itemsPerPage && index < (pageIndex + 1) * itemsPerPage) ? 'block' : 'none';
                });

                // Scroll to the top of the page when a new page is shown
                window.scrollTo({
                    top: 0,
                    behavior: 'smooth' // Optional: smooth scrolling
                });
            }


        });






    });

</script>






<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#new' + tabno + '-tab-pane"]').tab('show');
            const newsActivitiesButton = document.getElementById('NewsActivities');

            if (tabno === '2') {
                newsActivitiesButton.style = "display:none";
            } else {
                newsActivitiesButton.style = "display:block";
            }



        });
    }

</script>
