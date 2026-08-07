<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllDigitalMediaRequestsBeta.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta.ucAllDigitalMediaRequestsBeta" %>



<style>
    .fcc-btn {
        color: white !important;
        padding: 15px 25px 15px !important;
    }
</style>

<section>
    <div class="container pt-5 my-5">

        <div id="dvAddNew" runat="server" visible="false">
            <a class="btn btn-primary fcc-btn" href="/ar/MediaCenter/NewsBeta/Pages/AddDigitalMedia.aspx">إضافة وسائط رقمية</a>
        </div>
        <h2 id="hMsg" runat="server" style="color: red" class="d-none">ليس لديك صلاحية الوصول لهذه الشاشة</h2>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="table-responsive" id="dvForm" runat="server">
                    <table class="table table-striped-secondary table-borderless fs-5" id="myTable">
                        <thead>
                            <tr>
                                <th scope="col">العنوان</th>
                                <th scope="col">الجهة الطالبة</th>
                                <th scope="col">مقدم الطلب</th>
                                <th scope="col">تاريخ الطلب</th>
                                <th scope="col">حالة الطلب</th>
                                <th scope="col">تعديل</th>
                                <th scope="col">حذف</th>
                            </tr>
                        </thead>

                        <tbody id="newsTableBody">
                            <asp:Repeater ID="rep" runat="server" OnItemCommand="rep_ItemCommand">
                                <ItemTemplate>
                                    <tr class="data-row">
                                        <td><%#Eval("Title")%></td>
                                        <td><%#Eval("FacultyName")%></td>
                                        <td class="text-nowrap"><%#Eval("RequesterNameDisplay")%></td>
                                        <td class="text-nowrap"><%#Eval("Created")%></td>
                                        <td class="text-nowrap"><%#Eval("RequestStatus")%></td>
                                        <td>
                                            <a href="/ar/MediaCenter/News/Pages/EditDigitalMedia.aspx?RequestId=<%#Eval("ID").ToString()%>">تعديل</a>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <!-- Pagination Controls -->
                    <div id="paginationControls" class="pagination"></div>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</section>


<script>
    $(document).ready(function () {
             var itemsPerPage = 10; // Number of items per page
             var currentPage = 1;   // Track current page
             var $tableRows = $('#newsTableBody tr'); // All rows in the table
             var totalItems = $tableRows.length; // Total number of rows (items)
             var totalPages = Math.ceil(totalItems / itemsPerPage); // Total number of pages

             // Function to show only a subset of rows per page
             function showPage(page) {
                 // Hide all rows
                 $tableRows.hide();

                 // Calculate start and end indexes for the current page
                 var start = (page - 1) * itemsPerPage;
                 var end = start + itemsPerPage;

                 // Show only the rows for the current page
                 $tableRows.slice(start, end).show();
             }

             // Generate pagination buttons with arrows
             function generatePagination() {
                 var paginationHtml = '';

                 // "<< Previous" button
                 if (currentPage > 1) {
                     paginationHtml += `<button class="pagination-btn prev">&laquo;</button>`;
                 }

                 // Page number buttons (show limited pages like 5 at a time)
                 var maxPagesToShow = 5;
                 var startPage = Math.max(1, currentPage - 2);
                 var endPage = Math.min(totalPages, currentPage + 2);

                 if (startPage > 1) {
                     paginationHtml += `<span>...</span>`;
                 }

                 for (var i = endPage; i >= startPage; i--) {
                     paginationHtml += `<button class="pagination-btn page-num" data-page="${i}" ${i === currentPage ? 'disabled' : ''}>${i}</button>`;
                 }

                 if (endPage < totalPages) {
                     paginationHtml += `<span>...</span>`;
                 }

                 // "Next >>" button
                 if (currentPage < totalPages) {
                     paginationHtml += `<button class="pagination-btn next">&raquo;</button>`;
                 }

                 // Add the pagination controls to the DOM
                 $('#paginationControls').html(paginationHtml);
             }

             // Initial page display
             showPage(currentPage);
             generatePagination();

             // Handle pagination button clicks
             $(document).on('click', '.pagination-btn', function () {
                 if ($(this).hasClass('prev')) {
                     currentPage--;
                 } else if ($(this).hasClass('next')) {
                     currentPage++;
                 } else {
                     currentPage = parseInt($(this).data('page'));
                 }

                 // Update page display and regenerate pagination controls
                 showPage(currentPage);
                 generatePagination();
             });
         });
</script>
<style>
    .pagination {
        margin-top: 20px;
        text-align: center;
        padding: 20px;
        padding-right: 170px;
    }

    .pagination-btn {
        display: inline-block;
        padding: 10px 15px;
        margin: 0 5px;
        background-color: #1a778d;
        color: #fff;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        font-size: 18px;
    }

        .pagination-btn:hover {
            background-color: #24b8a9;
        }

        .pagination-btn[disabled] {
            background-color: #f5f5f5;
            color: #fff;
            cursor: not-allowed;
        }

        .pagination-btn.prev, .pagination-btn.next {
            background-color: #024d4c;
            color: #fff;
            font-size: 20px;
        }

            .pagination-btn.prev:hover, .pagination-btn.next:hover {
                background-color: #003f3a;
            }

    .pagination span {
        display: inline-block;
        padding: 10px 15px;
        color: #333;
        font-size: 14px;
    }

    span.othersInfo {
        font-size: 20px;
    }
</style>
