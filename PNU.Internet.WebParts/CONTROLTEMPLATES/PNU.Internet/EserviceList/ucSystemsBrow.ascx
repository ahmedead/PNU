<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSystemsBrow.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.ucSystemsBrow" %>




<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


<%-- ===== Page hero + breadcrumb ===== --%>
<div class="bg-primary-25 py-5">
    <div class="container">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2">
                <li class="breadcrumb-item small">
                    <asp:HyperLink ID="hlHome" runat="server"></asp:HyperLink>
                </li>
                <li class="breadcrumb-item small active" aria-current="page">
                    <span><asp:Literal ID="litBreadcrumbTitle" runat="server"></asp:Literal></span>
                </li>
            </ol>
        </nav>
        <div class="content">
            <h2 class="mb-0"><asp:Literal ID="litPageTitle" runat="server"></asp:Literal></h2>
            <div class="text mt-4"><asp:Literal ID="litPageDesc" runat="server"></asp:Literal></div>
        </div>
    </div>
</div>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">

        <%-- ===== Search + filter ===== --%>
        <dga-search-input>
            <div class="d-flex gap-3 mb-4" role="search">
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                </div>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click" ></asp:LinkButton>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false" data-bs-auto-close="outside" class="btn btn-dark gap-1">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span><asp:Literal ID="litFilterBtn" runat="server"></asp:Literal></span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu px-2" style="width: 20rem;">
                            <p class="fw-semibold"><asp:Literal ID="litFilterTitle" runat="server"></asp:Literal></p>
                            <div class="form-control-container has-icon">
                                <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                    <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                </span>
                                <input type="text" autocomplete="off" class="form-control" id="txtFilterSearch" runat="server" onkeyup="pnuFilterAudienceSearch(this)" />
                            </div>
                            <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto" id="pnuAudienceList" style="max-height: 12.5rem;">
                                <asp:Repeater ID="rptAudienceFilter" runat="server">
                                    <ItemTemplate>
                                        <div class="form-check">
                                            <input type="checkbox" runat="server" id="chkAudience" class="form-check-input" />
                                            <asp:Label ID="lblAudience" runat="server" AssociatedControlID="chkAudience"
                                                CssClass="form-check-label" Text='<%# Container.DataItem %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <hr />
                            <div class="d-flex justify-content-between gap-2 pb-2 px-2">
                                <asp:LinkButton ID="btnApplyFilter" runat="server" CssClass="btn btn-primary" OnClick="btnApplyFilter_Click"></asp:LinkButton>
                                <asp:LinkButton ID="btnResetFilter" runat="server" CssClass="btn btn-secondary" OnClick="btnResetFilter_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </dga-search-input>

        <%-- ===== Cards ===== --%>
        <div class="row g-4">
            <asp:Repeater ID="rptServices" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <span class="d-inline-flex fs-3">
                                        <i class='<%# String.Format("hgi hgi-stroke {0} fs-3", Eval("SafeIconClass")) %>' aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div>
                                    <h3 class="card-title"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h3>
                                    <p class="card-text"><%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %></p>
                                </div>
                                <div class="d-flex flex-wrap mt-auto gap-2">
                                    <asp:Label ID="lblCategory" runat="server" CssClass="badge badge-info"
                                        Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("DisplayCategory"))) %>'
                                        Text='<%# Eval("DisplayCategory") %>'></asp:Label>
                                    <asp:Repeater ID="rptAudiences" runat="server" DataSource='<%# Eval("DisplayAudiences") %>'>
                                        <ItemTemplate>
                                            <span class="badge badge-success"><%# Container.DataItem %></span>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="d-flex gap-3 flex-wrap">
                                    <a class="btn btn-primary" href='<%# Eval("Url") %>' target="_blank" rel="noopener">
                                        <asp:Literal ID="litOpenSystem" runat="server" Text='<%# OpenSystemLabel %>'></asp:Literal>
                                    </a>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%-- ===== Empty state ===== --%>
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="text-center py-5">
            <span class="d-inline-flex fs-1 text-body-secondary mb-3">
                <i class="hgi hgi-stroke hgi-search-remove" aria-hidden="true"></i>
            </span>
            <h3 class="mb-2"><asp:Literal ID="litEmptyTitle" runat="server"></asp:Literal></h3>
            <p class="text-body-secondary mb-0"><asp:Literal ID="litEmptyDesc" runat="server"></asp:Literal></p>
        </asp:Panel>

        <%-- ===== Pagination ===== --%>
        <dga-paginator>
            <asp:Panel ID="pnlPager" runat="server" CssClass="mt-5 p-2 d-flex justify-content-center">
                <nav>
                    <ul class="pagination">
                        <li class="page-item">
                            <asp:LinkButton ID="lnkPrev" runat="server" CssClass="page-link btn btn-secondary icon-btn navigation-link"
                                CommandName="PrevPage" OnCommand="Pager_Command" aria-label="previous">
                                <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i></span>
                            </asp:LinkButton>
                        </li>
                        <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand" OnItemDataBound="rptPager_ItemDataBound">
                            <ItemTemplate>
                                <li class="page-item">
                                    <asp:LinkButton ID="lnkPage" runat="server" CommandName="GoPage"
                                        CommandArgument='<%# Container.DataItem %>' Text='<%# Container.DataItem %>'></asp:LinkButton>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <li class="page-item">
                            <asp:LinkButton ID="lnkNext" runat="server" CssClass="page-link btn btn-secondary icon-btn navigation-link"
                                CommandName="NextPage" OnCommand="Pager_Command" aria-label="next">
                                <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i></span>
                            </asp:LinkButton>
                        </li>
                    </ul>
                </nav>
            </asp:Panel>
        </dga-paginator>

    </div>
</main>

<script type="text/javascript">
    // Search within the audience filter checkboxes (client-side only)
    function pnuFilterAudienceSearch(input) {
        var term = input.value.trim().toLowerCase();
        var items = document.querySelectorAll('#pnuAudienceList .form-check');
        items.forEach(function (it) {
            var txt = it.textContent.trim().toLowerCase();
            it.style.display = (term === '' || txt.indexOf(term) !== -1) ? '' : 'none';
        });
    }
</script>
