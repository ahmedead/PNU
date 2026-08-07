<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucORCIDData.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucORCIDData" %>

<%-- ORCID profile card (matches sample pane-5) --%>
<asp:Repeater ID="rptProfile" runat="server">
    <ItemTemplate>
        <article class="card nav-card mb-4" dir="auto">
            <div class="card-body">
                <div class="icon-container">
                    <i class="hgi hgi-stroke hgi-link-04 fs-3" aria-hidden="true"></i>
                </div>
                <div>
                    <h3 class="card-title">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, ORCID %>" />
                    </h3>
                    <p class="card-text mb-0"><%# DataBinder.Eval(Container.DataItem, "Name") %></p>
                </div>
                <div class="d-flex justify-content-start mt-auto">
                    <a class="btn btn-secondary external-link" href="https://orcid.org/" target="_blank" rel="noopener noreferrer">ORCID</a>
                </div>
            </div>
        </article>
    </ItemTemplate>
</asp:Repeater>

<%-- ORCID works list --%>
<div dir="auto">
    <asp:Repeater ID="rptData1" runat="server">
        <ItemTemplate>
            <div class="d-flex flex-column gap-2 my-4 py-2 data-item">
                <div class="d-flex flex-wrap gap-2">
                    <span class="badge badge-info"><%# DataBinder.Eval(Container.DataItem, "Type") %></span>
                    <span class="badge badge-success"><%# DataBinder.Eval(Container.DataItem, "Source") %></span>
                </div>
                <div><a><%# DataBinder.Eval(Container.DataItem, "Title") %></a></div>
                <p class="mb-0 line-clamp max-clamp-line-2"><%# DataBinder.Eval(Container.DataItem, "journaltitle") %></p>
                <p class="mb-0 small"><%# DataBinder.Eval(Container.DataItem, "ExternalIds") %></p>
                <div class="d-flex flex-wrap gap-3">
                    <span class="small text-body-secondary">
                        <i class="hgi hgi-stroke hgi-calendar-01 me-1" aria-hidden="true"></i>
                        <%# DataBinder.Eval(Container.DataItem, "PublicationYear") %>-<%# DataBinder.Eval(Container.DataItem, "PublicationMonth") %>-<%# DataBinder.Eval(Container.DataItem, "PublicationDay") %>
                    </span>
                    <span class="small text-body-secondary">Added: <%# DataBinder.Eval(Container.DataItem, "Added") %></span>
                    <span class="small text-body-secondary">Last Modified: <%# DataBinder.Eval(Container.DataItem, "LastModified") %></span>
                </div>
            </div>
            <hr class="my-0">
        </ItemTemplate>
    </asp:Repeater>

    <%-- Client-side pagination container --%>
    <nav class="mt-5" aria-label="ORCID pagination">
        <div id="pagination-controls" class="pagination justify-content-center align-items-center gap-2 dga-pagination"></div>
    </nav>
</div>

<script>
    (function () {
        function initOrcidPaging() {
            var itemsPerPage = 5;
            var items = Array.prototype.slice.call(document.querySelectorAll('.data-item'));
            if (!items.length) return;
            var numPages = Math.ceil(items.length / itemsPerPage);
            var container = document.getElementById('pagination-controls');
            if (!container) return;
            container.innerHTML = '';

            function showPage(page) {
                items.forEach(function (el, i) {
                    var visible = i >= (page - 1) * itemsPerPage && i < page * itemsPerPage;
                    el.style.display = visible ? '' : 'none';
                    if (el.nextElementSibling && el.nextElementSibling.tagName === 'HR')
                        el.nextElementSibling.style.display = visible ? '' : 'none';
                });
                Array.prototype.forEach.call(container.querySelectorAll('.page-link'), function (a) {
                    a.classList.toggle('active', a.getAttribute('data-page') == page);
                });
            }

            for (var i = 1; i <= numPages; i++) {
                var a = document.createElement('a');
                a.href = '#';
                a.className = 'page-link';
                a.setAttribute('data-page', i);
                a.textContent = i;
                a.addEventListener('click', function (e) {
                    e.preventDefault();
                    showPage(parseInt(this.getAttribute('data-page'), 10));
                });
                container.appendChild(a);
            }
            showPage(1);
        }
        if (document.readyState === 'loading')
            document.addEventListener('DOMContentLoaded', initOrcidPaging);
        else
            initOrcidPaging();
    })();
</script>
