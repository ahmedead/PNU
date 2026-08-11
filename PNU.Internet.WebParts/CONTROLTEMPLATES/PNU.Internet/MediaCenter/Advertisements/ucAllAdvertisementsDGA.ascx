<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllAdvertisementsDGA.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements.ucAllAdvertisementsDGA" %>

<div class="tab-pane fade active show" id="agency-details-tabs-pane-5" role="tabpanel" aria-labelledby="agency-details-tabs-tab-5" tabindex="0">
    <h2 class="mb-4">الإعلانات</h2>
    
    <div class="accordion accordion-flush paginated-content" id="deanship-announcements-accordion">
        <asp:Repeater ID="rptAdvertisements" runat="server">
            <ItemTemplate>
                <div class="accordion-item paginated-item">
                    <h3 class="accordion-header" id='<%# "deanship-announcements-accordion-heading-" + (Container.ItemIndex + 1) %>'>
                        <button class="accordion-button collapsed" type="button" 
                                data-bs-toggle="collapse" 
                                data-bs-target='<%# "#deanship-announcements-accordion-collapse-" + (Container.ItemIndex + 1) %>' 
                                aria-expanded="false" 
                                aria-controls='<%# "deanship-announcements-accordion-collapse-" + (Container.ItemIndex + 1) %>'>
                            <%# DataBinder.Eval(Container.DataItem, "Title") %>
                        </button>
                    </h3>
                    <div class="accordion-collapse collapse" 
                         id='<%# "deanship-announcements-accordion-collapse-" + (Container.ItemIndex + 1) %>' 
                         aria-labelledby='<%# "deanship-announcements-accordion-heading-" + (Container.ItemIndex + 1) %>' 
                         data-bs-parent="#deanship-announcements-accordion">
                        <div class="accordion-body">
                            <p class="mb-0">
                                <%# !string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "Comments"))) ? DataBinder.Eval(Container.DataItem, "Comments") : DataBinder.Eval(Container.DataItem, "Title") %>
                                <%# !string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "Link"))) ? "<br/><a href='" + DataBinder.Eval(Container.DataItem, "Link") + "' class='text-decoration-none mt-2 d-inline-block fw-bold'>عرض التفاصيل</a>" : "" %>
                            </p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Pagination Controls -->
    <nav aria-label="Page navigation" class="pagination-container mt-4">
        <ul class="pagination justify-content-center gap-2"></ul>
    </nav>
</div>

<script>
    document.addEventListener('DOMContentLoaded', () => {
        const itemsPerPage = 6; // Number of items per page
        const paginatedContent = document.querySelector('#deanship-announcements-accordion.paginated-content') || document.querySelector('.paginated-content');
        if (!paginatedContent) return;

        const paginatedItems = paginatedContent.querySelectorAll('.paginated-item');
        const totalItems = paginatedItems.length;
        const totalPages = Math.ceil(totalItems / itemsPerPage);

        const paginationContainer = document.querySelector('.pagination-container');
        if (!paginationContainer) return;

        const paginationList = paginationContainer.querySelector('.pagination');
        if (!paginationList) return;

        paginationList.innerHTML = ''; // Clear existing pagination

        if (totalPages > 1) {
            paginationContainer.style.display = 'block';

            // Create Previous Button
            const prevItem = createPaginationItem('السابق', false);
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
            const nextItem = createPaginationItem('التالي', false);
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
                        ? ''
                        : 'none';
            });

            // Update active page in pagination
            const pageItems = paginationList.querySelectorAll('.page-item');
            pageItems.forEach((pageItem, index) => {
                pageItem.classList.toggle('active', index === pageIndex + 1); // +1 to account for "السابق"
            });
        }

        function getActivePageIndex() {
            const pageItems = paginationList.querySelectorAll('.page-item');
            return Array.from(pageItems).findIndex((pageItem) =>
                pageItem.classList.contains('active')
            ) - 1; // -1 to ignore "السابق"
        }
    });
</script>
