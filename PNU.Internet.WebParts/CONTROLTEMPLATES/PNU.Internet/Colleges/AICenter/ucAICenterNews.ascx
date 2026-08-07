<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAICenterNews.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAICenterNews" %>




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

        <div class="the-message p-2">
            <div class="mt-3 px-md-5 px-0">
                <div class="d-block">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIRecentNews %>" />
                    </h1>
                </div>
            </div>

            <div class="row g-3">
                <div class="row paginated-content">
                    <asp:Repeater ID="rptNews" runat="server">
                        <ItemTemplate>
                            <div class="col-md-6 col-lg-4 mb-4 paginated-item">
                                <div class="card border rounded-4 h-100 position-relative open-news" style="cursor: pointer;">
                                    <div class="card-body p-2">
                                        <div class="position-absolute top-0 start-0 m-4">
                                            <span class="badge rounded-pill bg-tertiary text-white fs-6"><%# Eval("MediaTypes") %></span>
                                        </div>

                                        <img runat="server" id="dv_newsImg" src='<%# Eval("AttachmentURL") %>' class="object-fit-cover rounded-3 w-100" height="250"
                                            style='<%# ((Eval("IsVideo").ToString() != "False") ? "display:none;": string.Empty) %>'>

                                        <div class="my-2 fs-4 fw-bold">
                                            <a href="javascript:void(0)" class="stretched-link text-decoration-none text-body strong">
                                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                            </a>
                                        </div>

                                        <div class="d-flex mb-3">
                                            <svg width="19" height="20">
                                                <use xlink:href="#calendar"></use></svg>
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

                                <!-- 🔒 Hidden, inert: browser won’t render it, so IDs here won’t clash -->
                                <template class="details-template">
                                    <section class="researcher-and-contributor">
                                        <div class="container py-5 my-5">
                                            <div class="row">

                                                <div class="col-12 mt-3 mb-md-5 mb-2 d-flex justify-content-center" id="divImage" runat="server" style='<%# "display:" + Eval("ImgVisible") %>'>
                                                    <div class="mx-4 pattern-border d-block video-container">
                                                        <img src='<%# Eval("AttachmentURL") %>' class="rounded-3" alt="..." style='<%# "display:" + Eval("ImgVisible") %>'>
                                                    </div>
                                                </div>

                                                <div class="col-12 order-lg-0 order-1" id="divTitle" runat="server" style='<%# "display:" + Eval("TitleVisible") %>'>
                                                    <div class="mb-3 mt-md-0 mt-4">
                                                        <h1 class="title text-dark fw-bold m-0">
                                                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                        </h1>
                                                        <a href="#" class="d-block mt-3 text-decoration-none mb-3 mb-md-0">
                                                            <svg width="18" height="16">
                                                                <use xlink:href="#share" />
                                                            </svg>
                                                            <span class="ms-2 text-primary">مشاركة</span>
                                                        </a>
                                                    </div>

                                                    <div class="my-3">
                                                        <svg width="19" height="20">
                                                            <use xlink:href="#calendar" />
                                                        </svg>
                                                        <span class="ms-2 text-muted"><%# Eval("MediaDate") %></span>
                                                    </div>

                                                    <p class="text-muted h5 mb-3 lh-base text-justify">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) %>
                                                        <br>
                                                        <br>
                                                    </p>
                                                </div>

                                                <div id="divVideoYouTube" runat="server" style='<%# "display:" + Eval("MP4Visible") %>'>
                                                    <div class="video-wrapper" id="divVideo" style='<%# "display:" + Eval("VideoVisiable") %>'>
                                                        <iframe class="responsive-iframe" src='<%# Eval("VideoURL") %>'></iframe>
                                                    </div>
                                                </div>

                                                <div class="col-12 mt-3 mb-md-5 mb-2 d-flex justify-content-center">
                                                    <div class="mx-4 pattern-border d-block vplyr">
                                                        <div class="rounded-3" id="player2" data-plyr-provider="vimeo" data-plyr-embed-id="76979871"></div>
                                                    </div>
                                                </div>

                                            </div>
                                            <br>
                                        </div>
                                    </section>
                                </template>
                            </div>
                        </ItemTemplate>

                    </asp:Repeater>
                </div>
                <nav aria-label="Page navigation" class="pagination-container">
                    <ul class="pagination justify-content-center"></ul>
                </nav>
            </div>


        </div>





    </div>
</section>
   
<div class="modal fade" id="newsModal" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
    <div class="modal-content rounded-4">
      <div class="modal-header">
        <h5 class="modal-title"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AIRecentNews %>" /></h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body" id="newsModalBody">
        <!-- content injected here -->
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">إغلاق</button>
      </div>
    </div>
  </div>
</div>

<script>
    document.addEventListener('click', function (e) {
        const card = e.target.closest('.open-news');
        if (!card) return;

        // خذ أقرب template مربوط بهذا الكارت
        const tpl = card.parentElement.querySelector('template.details-template')
            || card.querySelector('template.details-template');
        if (!tpl) return;

        // انسخ المحتوى إلى الـModal
        const modalBody = document.getElementById('newsModalBody');
        modalBody.innerHTML = '';
        modalBody.appendChild(tpl.content.cloneNode(true));

        // لو تستخدم Plyr أو أي مشغل، فعِّله هنا بعد حقن المحتوى
        // مثال (اختياري):
        // if (window.Plyr) { new Plyr('#player2'); }

        // اعرض الـModal
        const modalEl = document.getElementById('newsModal');
        bootstrap.Modal.getOrCreateInstance(modalEl).show();

        // امنع أي تنقل افتراضي
        e.preventDefault();
    }, false);
</script>



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









     


