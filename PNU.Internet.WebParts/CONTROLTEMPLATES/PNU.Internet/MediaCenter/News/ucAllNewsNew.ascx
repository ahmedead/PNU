<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllNewsNew.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucAllNewsNew" %>




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
                                                            <img runat="server" id="dv_newsImg" src='<%# Eval("AttachmentURL") %>' class="object-fit-cover rounded-3 w-100" height="250" style='<%# ((Eval("IsVideo").ToString() != "False") ? "display:none;": string.Empty) %>'>
                                                            <%--<div class="video-wrapper video-container" id="divVideo" runat="server" visible='<%# Eval("IsVideo") %>'>
                                                                <video class="object-fit-cover for-thumbnail rounded-3 w-100" style="height: 250px; max-width: 100%; pointer-events: none;">
                                                                    <source src="<%# Eval("VideoURL") %>" />
                                                                </video>
                                                            </div> --%>
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

                <%--<div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">
                    <div class="py-3 d-flex flex-wrap d-md-block">
                        <a id="AllMCNews" runat="server" class="btn btn-lg btn-outline-primary px-5 me-3 mb-3 mb-md-5 flex-fill" href="">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>"></asp:Literal>
                        </a>
                    </div>
                </div>--%>
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

            if (totalPages > 1) {
                paginationContainer.style.display = 'block';
                for (let i = 0; i < totalPages; i++) {
                    const pageItem = document.createElement('li');
                    pageItem.className = 'page-item';
                    const pageLink = document.createElement('a');
                    pageLink.className = 'page-link';
                    pageLink.href = '#';
                    pageLink.innerText = i + 1;
                    pageLink.addEventListener('click', (event) => {
                        event.preventDefault();
                        showPage(tabPane, i, itemsPerPage);
                    });

                    pageItem.appendChild(pageLink);
                    paginationList.appendChild(pageItem);
                }
            } else {
                paginationContainer.style.display = 'none';
            }

            showPage(tabPane, 0, itemsPerPage);
        });

        function showPage(tabPane, pageIndex, itemsPerPage) {
            const paginatedItems = tabPane.querySelectorAll('.paginated-item');
            const paginationList = tabPane.querySelector('.pagination');

            paginatedItems.forEach((item, index) => {
                item.style.display = (index >= pageIndex * itemsPerPage && index < (pageIndex + 1) * itemsPerPage) ? 'block' : 'none';
            });

            const pageItems = paginationList.querySelectorAll('.page-item');
            pageItems.forEach((pageItem, index) => {
                pageItem.classList.toggle('active', index === pageIndex);
            });

            // Scroll to the top of the page
            window.scrollTo({
                top: 0,
                behavior: 'smooth' // Optional: smooth scrolling
            });
        }
    });
</script>






<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#new' + tabno + '-tab-pane"]').tab('show');
        });
    }

</script>
