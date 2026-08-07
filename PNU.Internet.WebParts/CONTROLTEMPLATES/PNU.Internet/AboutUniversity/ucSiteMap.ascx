<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSiteMap.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucSiteMap" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">

        <%--<h1 class="display-6 fw-bold mb-4">
            <%= SPFactory.GetLocalizedTitle("خريطة الموقع", "Sitemap") %>
        </h1>--%>

        <ul class="text-primary sitemap-list">
            <asp:Repeater ID="rptLevel1" runat="server" OnItemDataBound="rptLevel1_ItemDataBound">
                <ItemTemplate>
                    <li class="mt-3">
                        <%-- Level 1: if it has children, render as a span (group header).
                             If it has no children, render as a clickable link. --%>
                        <asp:PlaceHolder runat="server" Visible='<%# HasChildren(Container.DataItem) %>'>
                            <asp:PlaceHolder runat="server" Visible='<%# HasURL(Container.DataItem) %>'>
                                <a href='<%# Eval("URL") %>' class="fw-bold">
                                    <%# Eval("Title") %>
                                </a>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" Visible='<%# !HasURL(Container.DataItem) %>'>
                                <span class="fw-bold"><%# Eval("Title") %></span>
                            </asp:PlaceHolder>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" Visible='<%# !HasChildren(Container.DataItem) %>'>
                            <a href='<%# Eval("URL") %>' class="fw-bold">
                                <%# Eval("Title") %>
                            </a>
                        </asp:PlaceHolder>

                        <%-- Level 2 --%>
                        <asp:Repeater ID="rptLevel2" runat="server" OnItemDataBound="rptLevel2_ItemDataBound">
                            <HeaderTemplate><ul></HeaderTemplate>
                            <ItemTemplate>
                                <li class="mt-3">
                                    <asp:PlaceHolder runat="server" Visible='<%# HasL3Children(Container.DataItem) %>'>
                                        <asp:PlaceHolder runat="server" Visible='<%# HasURL(Container.DataItem) %>'>
                                            <a href='<%# Eval("URL") %>'>
                                                <%# Eval("Title") %>
                                            </a>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" Visible='<%# !HasURL(Container.DataItem) %>'>
                                            <span><%# Eval("Title") %></span>
                                        </asp:PlaceHolder>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder runat="server" Visible='<%# !HasL3Children(Container.DataItem) %>'>
                                        <a href='<%# Eval("URL") %>'>
                                            <%# Eval("Title") %>
                                        </a>
                                    </asp:PlaceHolder>

                                    <%-- Level 3 --%>
                                    <asp:Repeater ID="rptLevel3" runat="server">
                                        <HeaderTemplate><ul></HeaderTemplate>
                                        <ItemTemplate>
                                            <li class="mt-3">
                                                <a href='<%# Eval("URL") %>'>
                                                    <%# Eval("Title") %>
                                                </a>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate></ul></FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate></ul></FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

    </div>
</main>

<style>
    .sitemap-list,
    .sitemap-list ul {
        list-style: disc;
        padding-inline-start: 1.5rem;
    }
    .sitemap-list a {
        text-decoration: none;
    }
    .sitemap-list a:hover {
        text-decoration: underline;
    }
    .sitemap-list > li > a,
    .sitemap-list > li > span {
        font-size: 1.125rem;
    }
</style>
