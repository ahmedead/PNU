<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearchResults.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search.ucSearchResults" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">
        <dga-search-input>
            <div class="d-flex gap-3" role="search"
                aria-label='<%# IsArabic ? "بحث" : "Search" %>'>
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <asp:TextBox runat="server" ID="txtKeyword"
                        CssClass="form-control" autocomplete="off"
                        ClientIDMode="Static" />
                </div>
                <asp:Button runat="server" ID="btnSearch"
                    CssClass="btn btn-primary"
                    OnClick="btnSearch_Click" Style="display: none;" />


                <!-- The HTML Button -->
                <button type="button"
                    class="btn btn-primary"
                    aria-label="بحث"
                    onclick="document.getElementById('<%= btnSearch.ClientID %>').click();">
                    بحث
                </button>

            </div>
        </dga-search-input>

        <div class="d-flex flex-column gap-2 flex-wrap flex-lg-row align-items-lg-start justify-content-lg-between mt-5">
            <div>
                <h2 class="mb-0 pb-0 fw-bold">
                    <asp:Literal runat="server" ID="litResultsTitle" />
                </h2>
                <asp:Literal runat="server" ID="litResultsCount" />
            </div>

            <div class="d-flex gap-2">

                <%-- Sort dropdown --%>
                <div class="dropdown flex-shrink-0">
                    <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                        class="btn btn-outline-secondary gap-1">
                        <span>
                            <asp:Literal runat="server" ID="litSortLabel" /></span>
                        <span class="d-inline-flex fs-5">
                            <i class="hgi hgi-stroke hgi-sort-by-down-02" aria-hidden="true"></i>
                        </span>
                    </button>
                    <ul class="dropdown-menu px-0">
                        <li><a role="button"
                            class='<%# "dropdown-item " + (SortBy == "rel"    ? "active" : "") %>'
                            href='<%# BuildSortUrl("rel") %>'>
                            <%# IsArabic ? "الأكثر صلة" : "Most relevant" %></a></li>
                        <li><a role="button"
                            class='<%# "dropdown-item " + (SortBy == "newest" ? "active" : "") %>'
                            href='<%# BuildSortUrl("newest") %>'>
                            <%# IsArabic ? "الأحدث أولاً" : "Newest first" %></a></li>
                        <li><a role="button"
                            class='<%# "dropdown-item " + (SortBy == "oldest" ? "active" : "") %>'
                            href='<%# BuildSortUrl("oldest") %>'>
                            <%# IsArabic ? "الأقدم أولاً" : "Oldest first" %></a></li>
                    </ul>
                </div>

                <%-- Filter dropdown (categories, multi-select) --%>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                            data-bs-auto-close="outside" class="btn btn-dark gap-1">
                            <span class="d-inline-flex fs-5">
                                <i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i>
                            </span>
                            <span>
                                <asp:Literal runat="server" ID="litFilterLabel" /></span>
                            <span class="d-inline-flex fs-5">
                                <i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i>
                            </span>
                        </button>
                        <div class="dropdown-menu p-2" style="width: 20rem;">
                            <div class="m-1">
                                <p class="fw-semibold mb-2">
                                    <%# IsArabic ? "الفئة" : "Category" %>
                                </p>
                                <div class="form-control-container has-icon">
                                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                                    </span>
                                    <input type="text" autocomplete="off" class="form-control"
                                        placeholder='<%# IsArabic ? "بحث" : "Search" %>'
                                        id="<%= ClientID %>_catFilterBox" />
                                </div>
                                <div id="<%= ClientID %>_catList"
                                    class="d-flex flex-column gap-2 mt-4 px-2 py-2 overflow-auto"
                                    style="max-height: 12.5rem;">
                                    <asp:Repeater runat="server" ID="rptCategories">
                                        <ItemTemplate>
                                            <div class="form-check">
                                                <input type="checkbox" class="form-check-input dga-cat-cb"
                                                    id='<%# ClientID + "_cat_" + Eval("Index") %>'
                                                    value='<%# Eval("Value") %>'
                                                    <%# Convert.ToBoolean(Eval("Checked")) ? "checked='checked'" : "" %> />
                                                <label class="form-check-label"
                                                    for='<%# ClientID + "_cat_" + Eval("Index") %>'>
                                                    <%# Server.HtmlEncode(Convert.ToString(Eval("Display"))) %>
                                                </label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <hr />
                                <div class="d-flex justify-content-between align-content-stretch align-items-stretch gap-2">
                                    <button type="button" class="btn btn-primary"
                                        onclick='dgaApplyFilter_<%= ClientID %>()'>
                                        <%# IsArabic ? "تطبيق الاختيارات" : "Apply" %>
                                    </button>
                                    <button type="button" class="btn btn-secondary"
                                        onclick='dgaResetFilter_<%= ClientID %>()'>
                                        <%# IsArabic ? "إعادة تعيين" : "Reset" %>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>

            </div>
        </div>

        <%-- ===== RESULTS ===== --%>
        <asp:PlaceHolder runat="server" ID="phResults" Visible="false">
            <div class="mt-5">
                <asp:Repeater runat="server" ID="rptResults">
                    <ItemTemplate>
                        <div class="d-flex flex-column gap-2 my-4 py-2">
                            <asp:PlaceHolder runat="server" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Category"))) %>'>
                                <div class="d-flex flex-wrap gap-2">
                                    <span class="badge badge-info">
                                        <%# Server.HtmlEncode(Convert.ToString(Eval("Category"))) %>
                                    </span>
                                </div>
                            </asp:PlaceHolder>
                            <div>
                                <a href='<%# Eval("Url") %>'>
                                    <%# Server.HtmlEncode(Convert.ToString(Eval("Title"))) %>
                                </a>
                            </div>
                            <p class="mb-0 line-clamp max-clamp-line-2">
                                <%# HighlightSnippet(
                                Convert.ToString(Eval("Snippet")),
                                Convert.ToString(Eval("Keyword"))) %>
                            </p>
                            <div>
                                <span class="small text-body-secondary">
                                    <%# Eval("DateString") %>
                                </span>
                            </div>
                        </div>
                        <hr class="my-0" />
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <%-- ===== PAGINATOR (DGA template) ===== --%>
            <dga-paginator>
                <div class="mt-5 p-2 d-flex justify-content-center">
                    <nav aria-label='<%# IsArabic ? "قائمة التنقل في الصفحات" : "Pagination" %>'>
                        <ul class="pagination">
                            <%-- Prev --%>
                            <li class="page-item">
                                <asp:HyperLink runat="server" ID="lnkPrev"
                                    CssClass="page-link btn btn-secondary icon-btn navigation-link">
                            <span class="d-inline-flex fs-5">
                                <i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip"
                                   aria-hidden="true"></i>
                            </span>
                                </asp:HyperLink>
                            </li>
                            <%-- Numbered pages --%>
                            <asp:Repeater runat="server" ID="rptPager">
                                <ItemTemplate>
                                    <li class="page-item">
                                        <a href='<%# Eval("Url") %>'
                                            class='<%# "page-link btn btn-secondary " + (Convert.ToBoolean(Eval("IsCurrent")) ? "active" : "") %>'>
                                            <%# Eval("Text") %>
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%-- Next --%>
                            <li class="page-item">
                                <asp:HyperLink runat="server" ID="lnkNext"
                                    CssClass="page-link btn btn-secondary icon-btn navigation-link">
                            <span class="d-inline-flex fs-5">
                                <i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip"
                                   aria-hidden="true"></i>
                            </span>
                                </asp:HyperLink>
                            </li>
                        </ul>
                    </nav>
                </div>
            </dga-paginator>
        </asp:PlaceHolder>

        <%-- ===== EMPTY STATE ===== --%>
        <asp:PlaceHolder runat="server" ID="phEmpty" Visible="false">
            <div class="error-page text-center py-5">
                <div class="error-page__image d-flex justify-content-center">
                    <img src='/Style%20Library/DGA/public/images/empty.svg' alt='<%# IsArabic ? "لا توجد نتائج" : "No results" %>' />
                </div>
                <div class="erorr-page__text text-center d-flex flex-column align-items-center mt-5 mb-2 py-4">
                    <h3 class="text-black mb-3">
                        <%# IsArabic
                    ? "لا توجد نتائج مطابقة لبحثك"
                    : "No results match your search" %>
                    </h3>
                    <p class="mb-0">
                        <%# IsArabic
                    ? "يمكنك تجربة التأكد من كتابة كلمة البحث بشكل صحيح، أو استخدام كلمات مختلفة أو مرادفات أخرى، أو البحث بكلمات أكثر عمومية، أو تقليل عدد كلمات البحث."
                    : "Check the spelling of your keywords, try different or more general words, or reduce the number of words." %>
                    </p>
                </div>
                <a href='<%# IsArabic ? "/" : "/en/" %>' class="btn btn-primary">
                    <%# IsArabic ? "العودة للصفحة الرئيسية" : "Back to home" %>
                </a>
            </div>
        </asp:PlaceHolder>


    </div>
</main>
<%-- ===== keyword highlight helper class ===== --%>
<style>
    mark { background:#fff3a3; padding:0 2px; border-radius:2px; }
</style>

<script type="text/javascript">
    (function () {
        var clientId = '<%= ClientID %>';
        var input    = document.getElementById('<%= txtKeyword.ClientID %>');
        var btn      = document.getElementById('<%= btnSearch.ClientID %>');

        // Submit on Enter
        if (input && btn) {
            input.addEventListener('keydown', function (e) {
                if (e.key === 'Enter' || e.keyCode === 13) {
                    e.preventDefault();
                    btn.click();
                }
            });
        }

        // Filter dropdown: live category list filter
        var box = document.getElementById(clientId + '_catFilterBox');
        if (box) {
            box.addEventListener('input', function () {
                var q = (this.value || '').toLowerCase();
                var list = document.getElementById(clientId + '_catList');
                if (!list) return;
                var rows = list.querySelectorAll('.form-check');
                for (var i = 0; i < rows.length; i++) {
                    var lbl = rows[i].querySelector('label');
                    var t = (lbl ? lbl.textContent || '' : '').toLowerCase();
                    rows[i].style.display = (t.indexOf(q) >= 0) ? '' : 'none';
                }
            });
        }

        // Apply / Reset → navigate with ?cat=csv preserved on the URL
        window['dgaApplyFilter_' + clientId] = function () {
            var list = document.getElementById(clientId + '_catList');
            if (!list) return;
            var checked = list.querySelectorAll('input.dga-cat-cb:checked');
            var vals = [];
            for (var i = 0; i < checked.length; i++)
                vals.push(checked[i].value);
            var qs = new URL(window.location.href);
            qs.searchParams.set('cat', vals.join(','));
            qs.searchParams.set('p', '1');
            window.location.href = qs.toString();
        };

        window['dgaResetFilter_' + clientId] = function () {
            var qs = new URL(window.location.href);
            qs.searchParams.delete('cat');
            qs.searchParams.set('p', '1');
            window.location.href = qs.toString();
        };
    })();
</script>
