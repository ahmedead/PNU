<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="SearchInput.ascx.cs"
    Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search.SearchInput" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>


<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">
        <dga-search-input>
            <div class="d-flex gap-3" role="search"
                 aria-label='<%= SPFactory.GetLocalizedTitle("ابحث في موقع الجامعة" , "Search the university website") %>'>
                <div class="form-control-container has-icon">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control"
                                 placeholder='<%# SPFactory.GetLocalizedTitle("ابحث في موقع الجامعة" , "Search the university website") %>'
                                 ClientIDMode="Static" />
                </div>
                <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-primary"
                            Text='<%# SPFactory.GetLocalizedTitle("بحث" , "Search") %>'
                            OnClick="btnSearch_Click" />
            </div>
        </dga-search-input>

        <div class="mt-5">
            <div class="fw-semibold text-primary fs-5">
                <%= SPFactory.GetLocalizedTitle("اقتراحات" , "Suggestions") %>
            </div>

            <div class="d-flex gap-2 mt-3">
                <asp:Repeater runat="server" ID="rptSuggestions">
                    <ItemTemplate>
                        <a href='<%# "SearchResults.aspx" + "?q=" + System.Web.HttpUtility.UrlEncode(Eval("Category").ToString()) %>'
                           class="btn btn-sm btn-light">
                            <%# Eval("Category") %>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</main>
