<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearchInput.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search.ucSearchInput" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>


    <main id="main-content" class="dga-main-body" tabindex="-1">
        <div class="container page-padding">
            
<dga-search-input>
    <div class="d-flex gap-3" role="search"
         aria-label='<%# IsArabic ? "ابحث في موقع الجامعة" : "Search the university website" %>'>
        <div class="form-control-container has-icon">
            <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
            </span>
            <asp:TextBox runat="server" ID="txtKeyword"
                CssClass="form-control" autocomplete="off"
                ClientIDMode="Static" />
        </div>
       <!-- The ASP.NET Button (Hidden or Visible) -->
<asp:Button runat="server" ID="btnSearch"
     CssClass="btn btn-primary"
     OnClick="btnSearch_Click" 
     style="display:none;" /> <!-- Optional: hide it if you only want the custom button visible -->

<!-- The HTML Button -->
<button type="button" 
        class="btn btn-primary" 
        aria-label="بحث" 
        onclick="document.getElementById('<%= btnSearch.ClientID %>').click();">
    بحث
</button>

    </div>
</dga-search-input>

<asp:Panel runat="server" ID="pnlSuggestions" CssClass="mt-5"
           Visible="false">
    <div class="fw-semibold text-primary fs-5">
        <asp:Literal runat="server" ID="litSuggestionsLabel" />
    </div>
    <div class="d-flex gap-2 mt-3 flex-wrap">
        <asp:Repeater runat="server" ID="rptSuggestions">
            <ItemTemplate>
                <a href='<%# Eval("Url") %>'
                   class="btn btn-sm btn-light text-decoration-none">
                    <%# Server.HtmlEncode(Convert.ToString(Eval("Text"))) %>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>

        </div>

    </main>

<script type="text/javascript">
    (function () {
        var input = document.getElementById('<%= txtKeyword.ClientID %>');
        var btn   = document.getElementById('<%= btnSearch.ClientID %>');
        if (input && btn) {
            input.addEventListener('keydown', function (e) {
                if (e.key === 'Enter' || e.keyCode === 13) {
                    e.preventDefault();
                    btn.click();
                }
            });
        }
    })();
</script>
