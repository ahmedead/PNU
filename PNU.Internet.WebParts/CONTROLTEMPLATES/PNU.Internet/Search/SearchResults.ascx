<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="SearchResults.ascx.cs"
    Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search.SearchResults" %>

<%@ Import Namespace="PNU.Internet.WebParts" %>
<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">

        <%-- ===== Search bar ===== --%>
        <dga-search-input>
            <div class="d-flex gap-3" role="search"
                 aria-label='<%= SPFactory.GetLocalizedTitle("بحث" ,"Search" )%>'>
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control"
                                 placeholder='<%# SPFactory.GetLocalizedTitle("بحث" ,"Search") %>'
                                 ClientIDMode="Static" />
                </div>
                <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-primary"
                            Text='<%# SPFactory.GetLocalizedTitle("بحث" ,"Search") %>'
                            OnClick="btnSearch_Click" />
            </div>
        </dga-search-input>

        <%-- ===== Header / sort / filter ===== --%>
        <div class="d-flex flex-column gap-2 flex-wrap flex-lg-row align-items-lg-start justify-content-lg-between mt-5">
            <div>
                <h2 class="mb-0 pb-0 fw-bold">
                    <%= SPFactory.GetLocalizedTitle("نتيجة البحث عن" ,"Results for") %>
                    &nbsp;&ldquo;<asp:Literal runat="server" ID="litKeyword" />&rdquo;
                </h2>
                <asp:Panel runat="server" ID="pnlCount" Visible="false">
                    <span class="text-body-secondary d-block mt-2">
                        <asp:Literal runat="server" ID="litTotal" />
                        <%= SPFactory.GetLocalizedTitle(" نتائج وجدت" ," results found") %>
                    </span>
                </asp:Panel>
            </div>

            <div class="d-flex gap-2">
                <%-- sort --%>
                <div class="dropdown flex-shrink-0">
                    <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                            class="btn btn-outline-secondary gap-1">
                        <span><%= SPFactory.GetLocalizedTitle("ترتيب حسب" ,"Sort by") %></span>
                        <span class="d-inline-flex fs-5">
                            <i class="hgi hgi-stroke hgi-sort-by-down-02" aria-hidden="true"></i>
                        </span>
                    </button>
                    <ul class="dropdown-menu px-0">
                        <li><asp:LinkButton runat="server" ID="lnkSortRel"    CssClass="dropdown-item"
                                            CommandArgument="rel"
                                            OnClick="Sort_Click">
                                <%= SPFactory.GetLocalizedTitle("الأكثر صلة" ,"Most relevant") %>
                            </asp:LinkButton></li>
                        <li><asp:LinkButton runat="server" ID="lnkSortNewest" CssClass="dropdown-item"
                                            CommandArgument="newest"
                                            OnClick="Sort_Click">
                                <%= SPFactory.GetLocalizedTitle("الأحدث أولاً" ,"Newest first") %>
                            </asp:LinkButton></li>
                        <li><asp:LinkButton runat="server" ID="lnkSortOldest" CssClass="dropdown-item"
                                            CommandArgument="oldest"
                                            OnClick="Sort_Click">
                                <%= SPFactory.GetLocalizedTitle("الأقدم أولاً" ,"Oldest first") %>
                            </asp:LinkButton></li>
                    </ul>
                </div>

                <%-- filter --%>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                                data-bs-auto-close="outside" class="btn btn-dark gap-1">
                            <span class="d-inline-flex fs-5">
                                <i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i>
                            </span>
                            <span><%= SPFactory.GetLocalizedTitle("تصفية" ,"Filter") %></span>
                            <span class="d-inline-flex fs-5">
                                <i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i>
                            </span>
                        </button>
                        <div class="dropdown-menu p-2" style="width: 20rem;">
                            <div class="m-1">
                                <p class="fw-semibold mb-2">
                                    <%= SPFactory.GetLocalizedTitle("الفئة" ,"Category") %>
                                </p>
                                <div class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto"
                                     style="max-height: 12.5rem;">
                                    <asp:CheckBoxList runat="server" ID="chkCategories"
                                                      CssClass="form-check"
                                                      RepeatLayout="Flow" />
                                </div>
                                <hr/>
                                <div class="d-flex justify-content-between align-content-stretch align-items-stretch gap-2">
                                    <asp:Button runat="server" ID="btnApplyFilter" CssClass="btn btn-primary"
                                                Text='<%# SPFactory.GetLocalizedTitle("تطبيق الاختيارات" ,"Apply") %>'
                                                OnClick="btnApplyFilter_Click" />
                                    <asp:Button runat="server" ID="btnResetFilter" CssClass="btn btn-secondary"
                                                Text='<%# SPFactory.GetLocalizedTitle("إعادة تعيين" ,"Reset") %>'
                                                OnClick="btnResetFilter_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </div>

        <%-- ===== Results list ===== --%>
        <asp:Panel runat="server" ID="pnlResults" CssClass="mt-5" Visible="false">
            <asp:Repeater runat="server" ID="rptResults">
                <ItemTemplate>
                    <div class="d-flex flex-column gap-2 my-4 py-2">
                        <div class="d-flex flex-wrap gap-2">
                            <span class="badge badge-info"><%# Eval("Category") %></span>
                            <span class="badge badge-success"><%# Eval("SourceType") %></span>
                        </div>
                        <div>
                            <a href='<%# Eval("Url") %>'>
                                <%# Server.HtmlEncode(((PNU.Internet.SearchIndex.DAL.SearchIndexItem)Container.DataItem).DisplayTitle(SPFactory.IsArabic)) %>
                            </a>
                        </div>
                        <p class="mb-0 line-clamp max-clamp-line-2">
                            <%# HighlightSnippet(((PNU.Internet.SearchIndex.DAL.SearchIndexItem)Container.DataItem).DisplayContent(SPFactory.IsArabic), Keyword) %>
                        </p>
                        <div>
                            <span class="small text-body-secondary">
                                <%# ((PNU.Internet.SearchIndex.DAL.SearchIndexItem)Container.DataItem).DisplayDateString %>
                            </span>
                        </div>
                    </div>
                    <hr class="my-0" />
                </ItemTemplate>
            </asp:Repeater>

            <%-- ===== Pagination ===== --%>
            <dga-paginator>
                <div class="mt-5 p-2 d-flex justify-content-center">
                    <nav aria-label='<%= SPFactory.GetLocalizedTitle("قائمة التنقل في الصفحات" ,"Pagination" %>'>
                        <ul class="pagination">
                            <li class="page-item">
                                <asp:LinkButton runat="server" ID="lnkPrev"
                                                CssClass="page-link btn btn-secondary icon-btn navigation-link"
                                                CommandArgument="prev" OnClick="Page_Click">
                                    <span class="d-inline-flex fs-5">
                                        <i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i>
                                    </span>
                                </asp:LinkButton>
                            </li>
                            <asp:Repeater runat="server" ID="rptPages">
                                <ItemTemplate>
                                    <li class="page-item">
                                        <asp:LinkButton runat="server"
                                            CssClass='<%# "page-link btn btn-secondary " + (Convert.ToInt32(Eval("PageNumber")) == CurrentPage ? "active" ,"") %>'
                                            CommandArgument='<%# Eval("PageNumber") %>'
                                            OnClick="Page_Click">
                                            <%# Eval("PageNumber") %>
                                        </asp:LinkButton>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                            <li class="page-item">
                                <asp:LinkButton runat="server" ID="lnkNext"
                                                CssClass="page-link btn btn-secondary icon-btn navigation-link"
                                                CommandArgument="next" OnClick="Page_Click">
                                    <span class="d-inline-flex fs-5">
                                        <i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i>
                                    </span>
                                </asp:LinkButton>
                            </li>
                        </ul>
                    </nav>
                </div>
            </dga-paginator>
        </asp:Panel>

        <%-- ===== Empty state (search-empty.html) ===== --%>
        <asp:Panel runat="server" ID="pnlEmpty" Visible="false" CssClass="error-page text-center py-5">
            <div class="error-page__image d-flex justify-content-center">
                <img src='<%# ResolveUrl("~/_layouts/15/PNU/Internet/Search/empty.svg") %>' alt="empty" />
            </div>
            <div class="erorr-page__text text-center d-flex flex-column align-items-center mt-5 mb-2 py-4">
                <h3 class="text-black mb-3">
                    <%= SPFactory.GetLocalizedTitle("لا توجد نتائج مطابقة لبحثك" ,"No results match your search" %>
                </h3>
                <p class="mb-0">
                    <%= IsArabic
                        ? "يمكنك تجربة التأكد من كتابة كلمة البحث بشكل صحيح، أو استخدام كلمات مختلفة أو مرادفات أخرى، أو البحث بكلمات أكثر عمومية، أو تقليل عدد كلمات البحث."
                        : "Try checking your spelling, using different words or synonyms, broader keywords, or fewer terms." %>
                </p>
            </div>
            <a href='<%# ResolveUrl("~/") %>' class="btn btn-primary">
                <%= SPFactory.GetLocalizedTitle("العودة للصفحة الرئيسية" ,"Back to home" %>
            </a>
        </asp:Panel>
    </div>
</main>
