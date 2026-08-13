<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageIndexAdmin.aspx.cs" Inherits="PNU.Internet.WebParts.Layouts.PNU.Internet.Search.PageIndexAdmin" DynamicMasterPageFile="~masterurl/default.master" %>


<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="container py-4">
        <h2>PNU Search - page catalog</h2>
        <p class="text-muted">
            Scan publishing pages under a target web, capture their
            user-control paths and web-part properties, and export the
            result to Excel. Data is stored in the
            <code>dbo.WebsitePages</code> table.
        </p>

        <asp:Panel runat="server" ID="pnlOptions"
                   CssClass="p-3 border rounded bg-light mb-3">
            <div class="row g-2 align-items-end">
                <div class="col-md-8">
                    <label for='<%= txtSiteUrl.ClientID %>' class="form-label">
                        Target web (server-relative URL or absolute)
                    </label>
                    <asp:TextBox runat="server" ID="txtSiteUrl"
                        CssClass="form-control"
                        placeholder="/ar/Faculties/CBA (leave blank for site root)" />
                </div>
                <div class="col-md-4">
                    <div class="form-check mt-4">
                        <asp:CheckBox runat="server" ID="chkCurrentSiteOnly"
                            CssClass="form-check-input" />
                        <label class="form-check-label"
                               for='<%= chkCurrentSiteOnly.ClientID %>'>
                            Only index this web (skip subsites)
                        </label>
                    </div>
                </div>
            </div>

            <div class="mt-3 d-flex gap-2 flex-wrap">
                <asp:Button runat="server" ID="btnLoadPages"
                    CssClass="btn btn-secondary"
                    Text="Load &amp; Check Pages"
                    OnClick="btnLoadPages_Click" />
                <asp:Button runat="server" ID="btnIndexPages"
                    CssClass="btn btn-primary"
                    Text="Run Indexing"
                    OnClick="btnIndexPages_Click" />
                <asp:Button runat="server" ID="btnExportExcel"
                    CssClass="btn btn-success"
                    Text="Export to Excel"
                    OnClick="btnExportExcel_Click" />
                <a href="SearchAdmin.aspx" class="btn btn-outline-secondary ms-auto">
                    &laquo; Back to Search Admin
                </a>
            </div>
        </asp:Panel>

        <asp:Literal runat="server" ID="litStatus" />

        <asp:GridView runat="server" ID="gvPages" AutoGenerateColumns="false"
            CssClass="table table-striped table-hover align-middle"
            EmptyDataText="No pages loaded yet - click Load &amp; Check Pages."
            GridLines="None" HeaderStyle-CssClass="table-dark"
            OnRowDataBound="gvPages_RowDataBound">
            <Columns>
                <asp:BoundField DataField="PageTitle" HeaderText="Title"
                    ItemStyle-Width="180px" />
                <asp:TemplateField HeaderText="Page URL" ItemStyle-Width="260px">
                    <ItemTemplate>
                        <a href='<%# Eval("PageURL") %>' target="_blank">
                            <%# Server.HtmlEncode(Convert.ToString(Eval("PageURL"))) %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PageLayout" HeaderText="Layout"
                    ItemStyle-Width="140px" />
                <asp:TemplateField HeaderText="User Controls">
                    <ItemTemplate>
                        <pre class="small mb-0" style="white-space:pre-wrap;
                             max-height:100px; overflow:auto;"><%#
                            Server.HtmlEncode(Convert.ToString(Eval("UserControlPath")))
                        %></pre>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Properties">
                    <ItemTemplate>
                        <pre class="small mb-0" style="white-space:pre-wrap;
                             max-height:120px; overflow:auto; max-width:340px;"><%#
                            Server.HtmlEncode(Convert.ToString(Eval("UserControlProperties")))
                        %></pre>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="WebUrl" HeaderText="Web"
                    ItemStyle-Width="180px" />
                <asp:TemplateField HeaderText="Status" ItemStyle-Width="90px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatus"
                            Text='<%# Eval("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="LastIndexedDisplay"
                    HeaderText="Last indexed" ItemStyle-Width="140px" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Page catalog
</asp:Content>
