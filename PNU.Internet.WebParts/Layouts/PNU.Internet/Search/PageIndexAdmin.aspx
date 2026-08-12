<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageIndexAdmin.aspx.cs" Inherits="PNU.Internet.WebParts.Layouts.PNU.Internet.Search.PageIndexAdmin" 
    DynamicMasterPageFile="~masterurl/DGA_Internal.master" Async="false"%>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="container-fluid py-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>PNU Website Pages Indexer & Crawler</h2>
            <a href="SearchAdmin.aspx" class="btn btn-outline-secondary">&larr; Back to Search Admin</a>
        </div>

        <div class="card mb-4" style="background:#f8f9fa; border:1px solid #dee2e6; border-radius:4px; padding:15px;">
            <div class="card-body">
                <h5 class="card-title">Page Crawler Options</h5>
                <div class="row align-items-center mb-3">
                    <div class="col-md-6 mb-2">
                        <label for="<%= txtSiteUrl.ClientID %>" style="font-weight:bold;">Site / Sub-site URL:</label>
                        <asp:TextBox runat="server" ID="txtSiteUrl" CssClass="form-control"
                            Placeholder="Leave empty to index from Root Web, or enter specific site URL (e.g. https://site.pnu.edu.sa/subsite)"
                            Width="100%" />
                        <small class="form-text text-muted">Optional: Enter a target site/web URL.</small>
                    </div>
                    <div class="col-md-6 mb-2">
                        <div class="form-check" style="margin-top:24px;">
                            <asp:CheckBox runat="server" ID="chkCurrentSiteOnly" CssClass="form-check-input" />
                            <label class="form-check-label" for="<%= chkCurrentSiteOnly.ClientID %>" style="font-weight:bold;">
                                Only index this site (do not traverse sub-sites under it)
                            </label>
                        </div>
                    </div>
                </div>
                <div class="d-flex flex-wrap gap-2">
                    <asp:Button runat="server" ID="btnLoadPages" CssClass="btn btn-info" style="margin-right:8px;"
                        Text="Load & Check Site Pages" OnClick="btnLoadPages_Click" />
                    <asp:Button runat="server" ID="btnIndexPages" CssClass="btn btn-primary" style="margin-right:8px;"
                        Text="Run Indexing & Update DB" OnClick="btnIndexPages_Click" />
                    <asp:Button runat="server" ID="btnExportExcel" CssClass="btn btn-success"
                        Text="Export to Excel" OnClick="btnExportExcel_Click" />
                </div>
            </div>
        </div>

        <h4 class="mt-4">Status & Logs</h4>
        <asp:Literal runat="server" ID="litStatus" />

        <div class="d-flex justify-content-between align-items-center mt-4 mb-2">
            <h4>Discovered Pages (<asp:Label runat="server" ID="lblPageCount" Text="0" />)</h4>
        </div>

        <div class="table-responsive">
            <asp:GridView runat="server" ID="gvPages" AutoGenerateColumns="false"
                CssClass="table table-striped table-bordered table-hover"
                EmptyDataText="No pages loaded yet. Click 'Load & Check Site Pages' or 'Run Indexing & Update DB'."
                OnRowDataBound="gvPages_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblStatusBadge" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PageTitle" HeaderText="Page Title" />
                    <asp:TemplateField HeaderText="Page URL">
                        <ItemTemplate>
                            <a href='<%# Eval("PageURL") %>' target="_blank"><%# Eval("PageURL") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PageLayout" HeaderText="Page Layout" />
                    <asp:BoundField DataField="UserControlPath" HeaderText="User Control Path" />
                    <asp:BoundField DataField="UserControlProperties" HeaderText="User Control Properties" />
                    <asp:BoundField DataField="WebUrl" HeaderText="Web URL" />
                    <asp:TemplateField HeaderText="Last Indexed">
                        <ItemTemplate>
                            <%# ((DateTime)Eval("LastIndexed")) > DateTime.MinValue ? ((DateTime)Eval("LastIndexed")).ToString("yyyy-MM-dd HH:mm") : "—" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Website Pages Indexer & Crawler
</asp:Content>
