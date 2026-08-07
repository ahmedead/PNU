<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ucSearchResults.ascx.cs"
    Inherits="PNU.Internet.Search.ControlTemplates.PNU.Internet.Search.ucSearchResults" %>

<div class="dga-search-results">

    <!-- Search bar at top of results -->
    <div class="dga-results-toolbar">
        <div class="dga-search-input">
            <span class="dga-search-icon">
                <i class="fa fa-search" aria-hidden="true"></i>
            </span>
            <asp:TextBox runat="server" ID="txtKeyword"
                         CssClass="dga-search-textbox"
                         placeholder="ابحث..." />
            <asp:Button runat="server" ID="btnSearch"
                        CssClass="dga-search-button"
                        Text="بحث"
                        OnClick="btnSearch_Click" />
        </div>
    </div>

    <!-- Filter row: category dropdown + sort dropdown -->
    <div class="dga-filter-row">
        <div class="dga-filter-dropdown">
            <label for="<%= ddlCategory.ClientID %>">التصنيف</label>
            <asp:DropDownList runat="server" ID="ddlCategory"
                              AutoPostBack="true"
                              CssClass="dga-dropdown"
                              OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" />
        </div>
        <div class="dga-filter-dropdown">
            <label for="<%= ddlSort.ClientID %>">الترتيب</label>
            <asp:DropDownList runat="server" ID="ddlSort"
                              AutoPostBack="true"
                              CssClass="dga-dropdown"
                              OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                <asp:ListItem Value="rel"    Text="الأكثر صلة" />
                <asp:ListItem Value="newest" Text="الأحدث" />
                <asp:ListItem Value="oldest" Text="الأقدم" />
            </asp:DropDownList>
        </div>
    </div>

    <!-- Summary line -->
    <asp:Panel runat="server" ID="pnlSummary" CssClass="dga-results-summary">
        <asp:Literal runat="server" ID="litSummary" />
    </asp:Panel>

    <!-- Results list -->
    <asp:Panel runat="server" ID="pnlResults" CssClass="dga-results-list">
        <asp:Repeater runat="server" ID="rptResults">
            <ItemTemplate>
                <div class="dga-result-item">
                    <a href='<%# Eval("Url") %>' class="dga-result-title">
                        <%# Server.HtmlEncode(Convert.ToString(Eval("DisplayTitleHtml"))) %>
                    </a>
                    <div class="dga-result-meta">
                        <span class="dga-result-cat">
                            <%# Server.HtmlEncode(Convert.ToString(Eval("Category"))) %>
                        </span>
                        <span class="dga-result-sep">|</span>
                        <span class="dga-result-date">
                            <%# Eval("DisplayDateString") %>
                        </span>
                    </div>
                    <p class="dga-result-snippet">
                        <%# Server.HtmlEncode(Convert.ToString(Eval("Snippet"))) %>
                    </p>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>

    <!-- Empty state -->
    <asp:Panel runat="server" ID="pnlEmpty" Visible="false"
               CssClass="dga-empty-state">
        <div class="dga-empty-icon">
            <i class="fa fa-search" aria-hidden="true"></i>
        </div>
        <h3>لا توجد نتائج</h3>
        <p>
            لم نعثر على نتائج تطابق "<asp:Literal runat="server"
                ID="litEmptyKeyword" />".
            جرّب كلمات أخرى أو تصنيفات مختلفة.
        </p>
    </asp:Panel>

    <!-- Paginator -->
    <asp:Panel runat="server" ID="pnlPaginator" CssClass="dga-paginator">
        <asp:LinkButton runat="server" ID="lnkPrev"
                        CssClass="dga-page-prev" Text="السابق"
                        OnClick="lnkPrev_Click" />
        <asp:Literal runat="server" ID="litPageInfo" />
        <asp:LinkButton runat="server" ID="lnkNext"
                        CssClass="dga-page-next" Text="التالي"
                        OnClick="lnkNext_Click" />
    </asp:Panel>

</div>

<style>
.dga-search-results { max-width:960px; margin:0 auto; padding:24px 16px;
                      direction:rtl; font-family:inherit; }

.dga-results-toolbar { margin-bottom:16px; }
.dga-search-input {
    display:flex; align-items:stretch;
    border:1px solid #d0d5dd; border-radius:8px;
    background:#fff; overflow:hidden;
}
.dga-search-icon { display:flex; align-items:center;
    justify-content:center; padding:0 14px; color:#667085; }
.dga-search-textbox { flex:1; border:0; padding:12px 8px;
    font-size:15px; outline:none; direction:rtl; }
.dga-search-button { border:0; padding:0 28px;
    background:#005C5C; color:#fff; font-weight:600; cursor:pointer; }
.dga-search-button:hover { background:#00484c; }

.dga-filter-row { display:flex; gap:18px; flex-wrap:wrap;
    margin-bottom:18px; }
.dga-filter-dropdown { display:flex; flex-direction:column; gap:4px; }
.dga-filter-dropdown label { font-size:13px; color:#475467; }
.dga-dropdown { padding:8px 12px; border:1px solid #d0d5dd;
    border-radius:6px; min-width:180px; background:#fff; }

.dga-results-summary { font-size:14px; color:#475467; margin-bottom:12px; }

.dga-results-list { display:flex; flex-direction:column; gap:18px; }
.dga-result-item { padding:18px 20px; background:#fff;
    border:1px solid #eaecf0; border-radius:10px;
    transition:box-shadow .15s ease; }
.dga-result-item:hover { box-shadow:0 2px 8px rgba(16,24,40,.06); }
.dga-result-title { font-weight:700; font-size:17px;
    color:#005C5C; text-decoration:none; }
.dga-result-title:hover { text-decoration:underline; }
.dga-result-meta { font-size:13px; color:#667085;
    margin:6px 0 8px; display:flex; gap:10px; flex-wrap:wrap; }
.dga-result-sep { opacity:.4; }
.dga-result-snippet { color:#344054; font-size:14px; line-height:1.7;
    margin:0; }

.dga-empty-state { text-align:center; padding:48px 16px; color:#667085; }
.dga-empty-icon { font-size:48px; margin-bottom:12px; opacity:.5; }

.dga-paginator { display:flex; gap:14px; justify-content:center;
    align-items:center; margin-top:24px; padding-top:18px;
    border-top:1px solid #eaecf0; }
.dga-page-prev, .dga-page-next { padding:8px 16px; border:1px solid #d0d5dd;
    border-radius:6px; color:#005C5C; text-decoration:none;
    font-weight:600; }
.dga-page-prev:hover, .dga-page-next:hover { background:#f6fafa; }
</style>
