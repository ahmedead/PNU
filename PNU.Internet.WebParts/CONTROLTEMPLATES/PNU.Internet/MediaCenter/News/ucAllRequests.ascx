<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllRequests.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucAllRequests" %>


<style>


.fcc-btn {
  
  color: white !important;
  padding: 15px 25px 15px !important;
}

</style>


<section class="breadcrumb">
    <div class="container py-5 my-3">
        <h1 class="display-6 fw-bold mb-3">قائمة الطلبات </h1>
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb h6">
                <li class="breadcrumb-item"><a href="#" class="text-decoration-none fw-bold">الرئيسية</a></li>
                <li class="breadcrumb-item"><a href="#" class="text-decoration-none fw-bold">الأخبار</a></li>
                <li class="breadcrumb-item active fw-bold text-turquoise-600" aria-current="page">قائمة الطلبات
                </li>
            </ol>
        </nav>

    </div>
</section>

<section>
    <div class="container pt-5 my-5">
        
        <div  id="dvAddNew"  runat="server" visible="false" >
            <a  class="btn btn-primary fcc-btn"  href="addnew.aspx">إضافة خبر جديد</a>
        </div>
        <h2 id="hMsg" runat="server"  style="color:red" class="d-none">ليس لديك صلاحية الوصول لهذه الشاشة
        </h2>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="table-responsive" id="dvForm" runat="server">
                    <table class="table table-striped-secondary table-borderless fs-5">
  <thead>
    <tr>
      <th scope="col">العنوان</th>
      <th scope="col">الجهة الطالبة</th>
      <th scope="col">مقدم الطلب</th>
      <th scope="col">تاريخ الطلب</th>
      <th scope="col">حالة الطلب</th>
      <th scope="col">حالة الطلب في المركز الإعلامي</th>
      <th scope="col">عرض</th>
      <th scope="col">حذف</th>
      <th scope="col">تعديل الطلب</th>
    </tr>
  </thead>

  <tbody id="dataBody">
    <asp:Repeater ID="rep" runat="server" OnItemCommand="rep_ItemCommand">
      <ItemTemplate>
        <tr class="paginated-item" style="height:25px;">
          <td><%# Eval("Title") %></td>
          <td><%# Eval("FacultyName") %></td>
          <td class="text-nowrap"><%# Eval("RequesterNameDisplay") %></td>
          <td class="text-nowrap"><%# Eval("Created") %></td>
          <td class="text-nowrap"><%# Eval("RequestStatus") %></td>
          <td class="text-nowrap"><%# Eval("NextRequestStatus") %></td>
          <td><a href='RequestDetails.aspx?RequestId=<%# Eval("ID") %>'>عرض</a></td>
          <td>
            <asp:LinkButton ID="btnDelete" runat="server"
              CommandName="Delete" CommandArgument='<%# Eval("ID") %>'
              Text="حذف الطلب"
              OnClientClick="return confirm('Are you sure you want to delete this record?');">
            </asp:LinkButton>
          </td>
          <td>
            <asp:LinkButton ID="btnEdit" runat="server"
              CommandName="Edit" CommandArgument='<%# Eval("ID") %>'
              Text="تعديل الطلب">
            </asp:LinkButton>
          </td>
        </tr>
      </ItemTemplate>
    </asp:Repeater>
  </tbody>
</table>

<nav aria-label="Page navigation" class="pagination-container">
  <ul class="pagination justify-content-center"></ul>
</nav>

				</div>
            </ContentTemplate>

        </asp:UpdatePanel>
    </div>

</section>

<script>
(function () {
  const itemsPerPage = 6;

  function initPagination() {
    const tbody = document.getElementById('dataBody');
    if (!tbody) return;

    const rows = Array.from(tbody.querySelectorAll('tr.paginated-item'));
    const totalItems = rows.length;
    const totalPages = Math.ceil(totalItems / itemsPerPage);

    const paginationContainer = document.querySelector('.pagination-container');
    const paginationList = document.querySelector('.pagination');

    if (!paginationList || totalPages <= 1) {
      if (paginationContainer) paginationContainer.style.display = 'none';
      // Show all rows if no pagination
      rows.forEach(r => r.style.display = '');
      return;
    } else {
      paginationContainer.style.display = '';
    }

    function render(pageIndex) {
      // clamp page
      pageIndex = Math.max(0, Math.min(pageIndex, totalPages - 1));

      rows.forEach((row, i) => {
        const show = i >= pageIndex * itemsPerPage && i < (pageIndex + 1) * itemsPerPage;
        row.style.display = show ? '' : 'none';
      });

      renderControls(pageIndex);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    function renderControls(currentPage) {
      paginationList.innerHTML = '';

      const addItem = (label, page, enabled = true, active = false) => {
        const li = document.createElement('li');
        li.className = 'page-item' + (active ? ' active' : '') + (enabled ? '' : ' disabled');

        const a = document.createElement('a');
        a.className = 'page-link';
        a.href = '#';
        a.textContent = label;

        a.addEventListener('click', e => {
          e.preventDefault();
          if (!enabled) return;
          render(page);
        });

        li.appendChild(a);
        paginationList.appendChild(li);
      };

      // Prev
      addItem('<', currentPage - 1, currentPage > 0);

      // First
      addItem('1', 0, true, currentPage === 0);

      if (currentPage > 2) addItem('...', currentPage - 2, false);

      // Middle neighbors
      for (let i = Math.max(1, currentPage - 1); i <= Math.min(currentPage + 1, totalPages - 2); i++) {
        addItem(String(i + 1), i, true, currentPage === i);
      }

      if (currentPage < totalPages - 3) addItem('...', currentPage + 2, false);

      // Last
      if (totalPages > 1) addItem(String(totalPages), totalPages - 1, true, currentPage === totalPages - 1);

      // Next
      addItem('>', currentPage + 1, currentPage < totalPages - 1);
    }

    render(0);
  }

  // Initial load
  document.addEventListener('DOMContentLoaded', initPagination);

  // Re-init after UpdatePanel partial postbacks
  if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    const prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(initPagination);
  }
})();
</script>
