<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearchFacultyMembers.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucSearchFacultyMembers" %>



<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>



<div class="container page-padding">

    <%-- ============ DGA Search Input + Filter ============ --%>
    <dga-search-input>
        <div class="d-flex flex-column flex-md-row gap-3 mb-4" role="search">
            <div class="form-control-container has-icon flex-grow-1">
                <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                    <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                </span>
                <input type="text" autocomplete="off" class="form-control" id="txtSearch" runat="server" />
            </div>

            <button type="submit" class="btn btn-primary" id="btnSearch" runat="server">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, Search %>" />
            </button>

            <dga-filter-dropdown class="flex-shrink-0">
                <div class="dropdown flex-shrink-0" id="divFilterDropdown">
                    <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                        data-bs-auto-close="outside" class="btn btn-dark gap-1">
                        <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                        <span><%= FilterText %></span>
                        <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                    </button>
                    <div class="dropdown-menu p-2" style="width: 20rem;">
                        <div class="m-1">
                            <p class="fw-semibold mb-2"><%= CollegeText %></p>
                            <asp:DropDownList ID="ddlColleges" runat="server" CssClass="form-select mb-3"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlColleges_SelectedIndexChanged" />

                            <p class="fw-semibold mb-2"><%= SectionText %></p>
                            <asp:DropDownList ID="ddlSections" runat="server" CssClass="form-select" />

                            <hr />
                            <div class="d-flex justify-content-between align-content-stretch align-items-stretch gap-2">
                                <button type="submit" class="btn btn-primary" id="btnApplyFilter" runat="server"><%= ApplyText %></button>
                                <button type="submit" class="btn btn-secondary" id="btnClear" runat="server"><%= ResetText %></button>
                            </div>
                        </div>
                    </div>
                </div>
            </dga-filter-dropdown>
        </div>
    </dga-search-input>

    <asp:HiddenField ID="hfCurrentPage" runat="server" />
    <asp:HiddenField ID="hfFilterOpen" runat="server" Value="0" />

    <%-- ============ Results Cards ============ --%>
    <div class="row g-4">
        <asp:Repeater ID="rptData" runat="server">
            <ItemTemplate>
                <div class="col-12 col-lg-4 col-md-6">
                    <article class="card h-100 pnu-news-card">
                        <div class="card-body d-flex flex-column h-100">
                            <div class="icon-container">
                                <i class="hgi hgi-stroke hgi-user-circle fs-3" aria-hidden="true"></i>
                            </div>
                            <div class="flex-grow-1">
                                <h3 class="card-title"><%# SPFactory.GetLocalizedTitle(Eval("FULL_NAME"), Eval("ENGLISH_NAME")) %></h3>
                                <p class="card-text"><%# SPFactory.GetLocalizedTitle(Eval("PROFESSION"), Eval("PROFESSION_EN")) %></p>
                            </div>
                            <div class="mt-auto d-flex flex-column gap-4">
                                <div class="d-flex flex-wrap mt-auto gap-2">
                                    <span class="badge badge-info"><%# SPFactory.GetLocalizedTitle(Eval("COLLEGE"), Eval("COLLEGE_EN")) %></span>
                                    <span class="badge badge-light"><%# SPFactory.GetLocalizedTitle(Eval("SECTION"), Eval("SECTION_EN")) %></span>
                                </div>
                                <div class="d-flex justify-content-start">
                                    <a class="btn btn-primary"
                                        href='<%# String.Format("{1}Faculties/Pages/FacultyMemberDetails.aspx?view={0}", Eval("EMAIL_ADDRESS"), SPFactory.GetSiteURL()) %>'>
                                        <%# ViewProfileText %>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </article>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <%-- ============ Empty State ============ --%>
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
        <div class="d-flex flex-column align-items-center py-5">
            <img src="/Style Library/PNU/images/empty.svg" alt="" />
            <div class="text-center d-flex flex-column align-items-center mt-5 mb-2 py-4">
                <h3 class="text-black mb-3"><%= EmptyTitleText %></h3>
                <p class="mb-0"><%= EmptyHintText %></p>
            </div>
        </div>
    </asp:Panel>

    <%-- ============ DGA Paginator ============ --%>
    <asp:Panel ID="pnlPaginator" runat="server">
        <dga-paginator>
            <div class="mt-5 p-2 d-flex justify-content-center">
                <nav aria-label="pagination">
                    <ul class="pagination">
                        <li class="page-item">
                            <asp:LinkButton ID="lbPrevious" runat="server" EnableViewState="false"
                                CssClass="page-link btn btn-secondary icon-btn navigation-link" OnClick="lbPrevious_Click">
                                <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i></span>
                            </asp:LinkButton>
                        </li>
                        <asp:Repeater ID="rptPaging" runat="server" OnItemCommand="rptPaging_ItemCommand">
                            <ItemTemplate>
                                <li class="page-item">
                                    <asp:LinkButton ID="lbPaging" runat="server"
                                        Text='<%# Eval("Text") %>'
                                        CommandArgument='<%# Eval("Value") %>'
                                        CssClass='<%# GetPageClass(Eval("Value")) %>' />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <li class="page-item">
                            <asp:LinkButton ID="lbNext" runat="server" EnableViewState="false"
                                CssClass="page-link btn btn-secondary icon-btn navigation-link" OnClick="lbNext_Click">
                                <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i></span>
                            </asp:LinkButton>
                        </li>
                    </ul>
                </nav>
            </div>
        </dga-paginator>
    </asp:Panel>

</div>

<script type="text/javascript">
    (function () {
        // Re-open the filter dropdown after an AutoPostBack triggered from inside it
        var flag = document.getElementById('<%= hfFilterOpen.ClientID %>');
        var dd = document.querySelector('#divFilterDropdown [data-bs-toggle="dropdown"]');
        if (!flag || !dd) return;

        // Mark postbacks that originate from inside the dropdown
        var menu = document.querySelector('#divFilterDropdown .dropdown-menu');
        if (menu) {
            menu.addEventListener('change', function () { flag.value = '1'; });
        }

        if (flag.value === '1' && window.bootstrap) {
            flag.value = '0';
            new bootstrap.Dropdown(dd).show();
        }
    })();
</script>
