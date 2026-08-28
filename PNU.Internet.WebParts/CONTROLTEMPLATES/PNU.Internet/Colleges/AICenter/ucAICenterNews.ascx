<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAICenterNews.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAICenterNews" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<div class="d-flex flex-column gap-4">
    <h2 class="mb-4 fw-semibold">
        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIRecentNews %>" />
    </h2>

    <div class="row g-4" id="aiCenterNewsContainer">
        <asp:Repeater ID="rptNews" runat="server">
            <ItemTemplate>
                <div class="col-12 col-md-6 col-xl-4 paginated-news-item">
                    <article class="card h-100 border rounded-3 overflow-hidden shadow-sm pnu-news-card position-relative open-news-card" style="cursor: pointer;">
                        <div class="card-body p-3 d-flex flex-column gap-3">
                            <div class="position-relative overflow-hidden rounded-2" style="height: 200px;">
                                <img src='<%# Eval("AttachmentURL") %>' class="w-100 h-100 object-fit-cover"
                                     style='<%# ((Eval("IsVideo").ToString() != "False") ? "display:none;": string.Empty) %>'
                                     alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>' loading="lazy" decoding="async">
                                <div class="position-absolute top-0 start-0 m-2">
                                    <span class="badge bg-primary text-white"><%# Eval("MediaTypes") %></span>
                                </div>
                            </div>

                            <div>
                                <h3 class="card-title h6 fw-bold mb-2">
                                    <a href="javascript:void(0)" class="stretched-link text-decoration-none text-body">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                    </a>
                                </h3>
                                <p class="card-text text-muted small line-clamp max-clamp-line-3 mb-0">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                                </p>
                            </div>

                            <div class="mt-auto pt-3 border-top d-flex justify-content-between align-items-center">
                                <small class="text-muted d-flex align-items-center gap-1">
                                    <i class="hgi hgi-stroke hgi-calendar-03 fs-6" aria-hidden="true"></i>
                                    <span><%# Eval("MediaDate") %></span>
                                </small>
                                <span class="badge text-bg-light border text-truncate" style="max-width: 140px;">
                                    <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                </span>
                            </div>
                        </div>
                    </article>

                    <!-- Template for Modal Details -->
                    <template class="details-template">
                        <div class="d-flex flex-column gap-3">
                            <div class="rounded-3 overflow-hidden" id="divImage" runat="server" style='<%# "display:" + Eval("ImgVisible") %>'>
                                <img src='<%# Eval("AttachmentURL") %>' class="w-100 rounded-3 object-fit-cover" style="max-height: 400px;" alt="...">
                            </div>

                            <div id="divTitle" runat="server" style='<%# "display:" + Eval("TitleVisible") %>'>
                                <h3 class="fw-bold mb-2">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                </h3>
                                <div class="d-flex align-items-center gap-2 text-muted mb-3 small">
                                    <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                    <span><%# Eval("MediaDate") %></span>
                                </div>
                                <div class="text-muted lh-base text-justify fs-6">
                                    <%# SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) %>
                                </div>
                            </div>

                            <div id="divVideoYouTube" runat="server" style='<%# "display:" + Eval("MP4Visible") %>'>
                                <div class="ratio ratio-16x9 rounded-3 overflow-hidden" id="divVideo" style='<%# "display:" + Eval("VideoVisiable") %>'>
                                    <iframe src='<%# Eval("VideoURL") %>' allowfullscreen></iframe>
                                </div>
                            </div>
                        </div>
                    </template>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Pagination -->
    <nav aria-label="Page navigation" class="mt-4" id="aiNewsPaginationContainer">
        <ul class="pagination justify-content-center" id="aiNewsPagination"></ul>
    </nav>
</div>

<!-- Modal -->
<div class="modal fade" id="newsModal" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable">
    <div class="modal-content rounded-4 border-0 shadow">
      <div class="modal-header border-bottom">
        <h5 class="modal-title fw-semibold"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AIRecentNews %>" /></h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body p-4" id="newsModalBody">
      </div>
      <div class="modal-footer border-top">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal"><%= SPFactory.IsArabic ? "إغلاق" : "Close" %></button>
      </div>
    </div>
  </div>
</div>

<script type="text/javascript">
    document.addEventListener('DOMContentLoaded', function () {
        // Modal Trigger Handler
        document.addEventListener('click', function (e) {
            var card = e.target.closest('.open-news-card');
            if (!card) return;

            var container = card.closest('.paginated-news-item');
            if (!container) return;

            var tpl = container.querySelector('template.details-template');
            if (!tpl) return;

            var modalBody = document.getElementById('newsModalBody');
            if (modalBody) {
                modalBody.innerHTML = '';
                modalBody.appendChild(tpl.content.cloneNode(true));
            }

            var modalEl = document.getElementById('newsModal');
            if (modalEl && window.bootstrap) {
                bootstrap.Modal.getOrCreateInstance(modalEl).show();
            }
            e.preventDefault();
        });

        // Pagination Handler
        var itemsPerPage = 6;
        var items = document.querySelectorAll('#aiCenterNewsContainer .paginated-news-item');
        var totalItems = items.length;
        var totalPages = Math.ceil(totalItems / itemsPerPage);
        var pagContainer = document.getElementById('aiNewsPaginationContainer');
        var pagList = document.getElementById('aiNewsPagination');

        if (pagList && totalPages > 1) {
            pagList.innerHTML = '';
            for (var i = 0; i < totalPages; i++) {
                (function (pageIdx) {
                    var li = document.createElement('li');
                    li.className = 'page-item' + (pageIdx === 0 ? ' active' : '');
                    var a = document.createElement('a');
                    a.className = 'page-link';
                    a.href = 'javascript:void(0)';
                    a.textContent = (pageIdx + 1);
                    a.addEventListener('click', function (evt) {
                        evt.preventDefault();
                        showPage(pageIdx);
                    });
                    li.appendChild(a);
                    pagList.appendChild(li);
                })(i);
            }
            showPage(0);
        } else if (pagContainer && totalPages <= 1) {
            pagContainer.style.display = 'none';
        }

        function showPage(pageIdx) {
            items.forEach(function (item, idx) {
                item.style.display = (idx >= pageIdx * itemsPerPage && idx < (pageIdx + 1) * itemsPerPage) ? 'block' : 'none';
            });
            if (pagList) {
                var pageItems = pagList.querySelectorAll('.page-item');
                pageItems.forEach(function (pi, idx) {
                    pi.classList.toggle('active', idx === pageIdx);
                });
            }
        }
    });
</script>
