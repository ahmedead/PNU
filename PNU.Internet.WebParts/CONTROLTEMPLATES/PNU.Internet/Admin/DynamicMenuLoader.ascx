<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicMenuLoader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.DynamicMenuLoader" %>

<style type="text/css">
    .pnu-dml-wrapper {
        display: flex;
        flex-direction: row;
        width: 100%;
        min-height: 500px;
        font-family: 'Segoe UI', Tahoma, Arial, sans-serif;
    }
    .pnu-dml-wrapper.rtl { direction: rtl; }
    .pnu-dml-wrapper.ltr { direction: ltr; }

    .pnu-dml-sidemenu {
        width: 260px;
        flex-shrink: 0;
        background: #f4f6f9;
        border-right: 1px solid #e1e4e8;
        padding: 10px 0;
    }
    .pnu-dml-wrapper.rtl .pnu-dml-sidemenu {
        border-right: none;
        border-left: 1px solid #e1e4e8;
    }

    .pnu-dml-sidemenu ul {
        list-style: none;
        margin: 0;
        padding: 0;
    }
    .pnu-dml-sidemenu li a {
        display: block;
        padding: 12px 18px;
        color: #2b3a4a;
        text-decoration: none;
        border-left: 3px solid transparent;
        transition: background 0.15s, border-color 0.15s;
        font-size: 14px;
    }
    .pnu-dml-wrapper.rtl .pnu-dml-sidemenu li a {
        border-left: none;
        border-right: 3px solid transparent;
    }
    .pnu-dml-sidemenu li a:hover {
        background: #e8edf3;
    }
    .pnu-dml-sidemenu li a.active {
        background: #ffffff;
        border-left-color: #0078d4;
        font-weight: 600;
        color: #0078d4;
    }
    .pnu-dml-wrapper.rtl .pnu-dml-sidemenu li a.active {
        border-right-color: #0078d4;
    }

    .pnu-dml-content {
        flex: 1;
        padding: 20px 25px;
        background: #ffffff;
        min-width: 0;
    }
    .pnu-dml-empty {
        color: #888;
        text-align: center;
        padding: 40px 20px;
    }
    .pnu-dml-error {
        color: #b00020;
        background: #fdecea;
        border: 1px solid #f5c2c0;
        padding: 10px 14px;
        border-radius: 4px;
        margin-bottom: 12px;
    }
</style>

<div class="pnu-dml-wrapper <%= IsRtl ? "rtl" : "ltr" %>">
    <nav class="pnu-dml-sidemenu" aria-label="Side Menu">
        <asp:Literal ID="litErrors" runat="server" Visible="false" />
        <asp:Repeater ID="rptMenu" runat="server" OnItemDataBound="rptMenu_ItemDataBound">
            <HeaderTemplate><ul></HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:HyperLink ID="hlItem" runat="server" />
                </li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater>
    </nav>

    <section class="pnu-dml-content">
        <asp:PlaceHolder ID="phContent" runat="server" />
        <asp:Literal ID="litEmpty" runat="server" Visible="false"
            Text='<div class="pnu-dml-empty">Please select an item from the menu.</div>' />
    </section>
</div>
