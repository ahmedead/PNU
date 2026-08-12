<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllAdvertisements.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements.ucAllAdvertisements" %>




<section class="py-5 mb-5">
  <div class="container">
<div class="row paginated-content">
  <asp:Repeater ID="rptNews" runat="server">
    <ItemTemplate>
      <div class="col-md-6 col-lg-4 mb-4 paginated-item">
        <div class="card border rounded-4 h-100">
          <div class="card-body p-2">
            <img src='<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>' 
                 class="object-fit-cover rounded-3 w-100" 
                 height="250">
            <div class="my-2 fs-4 fw-bold">
              <asp:HyperLink runat="server" ID="link2" Target="_blank"  style="display:none">
                <h5 class="card-title mb-3 fw-bold">
                  <asp:Label ID="lblNewsTitle2" runat="server" 
                             Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                </h5>
              </asp:HyperLink>
              <a href='<%# DataBinder.Eval(Container.DataItem, "Link") %>' 
                 class="stretched-link text-decoration-none text-body strong">
                <%# DataBinder.Eval(Container.DataItem, "Title") %>
              </a>
            </div>
          </div>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>
</div>

<!-- Pagination Controls -->
<nav aria-label="Page navigation" class="pagination-container mt-3">
  <ul class="pagination justify-content-center"></ul>
</nav>


    <nav class="mt-5">
      <ul class="pagination justify-content-center gap-3">
        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" >
          <ItemTemplate>
            <li class="page-item">
                <asp:LinkButton ID="lnkPage"     CssClass="page-link" CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server" Font-Bold="True">
                  <%# Container.DataItem %>
                </asp:LinkButton>
            </li>
          </ItemTemplate>
        </asp:Repeater>
      </ul>
    </nav>
  </div>
</section>



<script>
    document.addEventListener('DOMContentLoaded', () => {
        const itemsPerPage = 6; // Number of items per page
        const paginatedContent = document.querySelector('.paginated-content');
        const paginatedItems = paginatedContent.querySelectorAll('.paginated-item');
        const totalItems = paginatedItems.length;
        const totalPages = Math.ceil(totalItems / itemsPerPage);

        const paginationContainer = document.querySelector('.pagination-container');
        const paginationList = paginationContainer.querySelector('.pagination');
        paginationList.innerHTML = ''; // Clear existing pagination

        if (totalPages > 1) {
            paginationContainer.style.display = 'block';

            // Create Previous Button
            const prevItem = createPaginationItem('Previous', false);
            paginationList.appendChild(prevItem);

            // Create Page Numbers
            for (let i = 0; i < totalPages; i++) {
                const pageItem = createPaginationItem(i + 1, i === 0); // First page active by default
                pageItem.addEventListener('click', (event) => {
                    event.preventDefault();
                    showPage(i);
                });
                paginationList.appendChild(pageItem);
            }

            // Create Next Button
            const nextItem = createPaginationItem('Next', false);
            paginationList.appendChild(nextItem);

            // Handle Previous and Next Button Clicks
            prevItem.addEventListener('click', (event) => {
                event.preventDefault();
                const currentPage = getActivePageIndex();
                if (currentPage > 0) {
                    showPage(currentPage - 1);
                }
            });

            nextItem.addEventListener('click', (event) => {
                event.preventDefault();
                const currentPage = getActivePageIndex();
                if (currentPage < totalPages - 1) {
                    showPage(currentPage + 1);
                }
            });
        } else {
            paginationContainer.style.display = 'none'; // Hide pagination if there's only one page
        }

        // Show the first page by default
        showPage(0);

        function createPaginationItem(label, isActive) {
            const pageItem = document.createElement('li');
            pageItem.className = `page-item ${isActive ? 'active' : ''}`;
            const pageLink = document.createElement('a');
            pageLink.className = 'page-link';
            pageLink.href = '#';
            pageLink.innerText = label;
            pageItem.appendChild(pageLink);
            return pageItem;
        }

        function showPage(pageIndex) {
            paginatedItems.forEach((item, index) => {
                item.style.display =
                    index >= pageIndex * itemsPerPage && index < (pageIndex + 1) * itemsPerPage
                        ? 'block'
                        : 'none';
            });

            // Update active page in pagination
            const pageItems = paginationList.querySelectorAll('.page-item');
            pageItems.forEach((pageItem, index) => {
                pageItem.classList.toggle('active', index === pageIndex + 1); // +1 to account for "Previous"
            });

            // Smooth scroll to the top of the list
            window.scrollTo({
                top: paginatedContent.offsetTop,
                behavior: 'smooth',
            });
        }

        function getActivePageIndex() {
            const pageItems = paginationList.querySelectorAll('.page-item');
            return Array.from(pageItems).findIndex((pageItem) =>
                pageItem.classList.contains('active')
            ) - 1; // -1 to ignore "Previous"
        }
    });

</script>