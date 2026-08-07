<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search-WPUserControl.ascx.cs" Inherits="PNU_SP.webparts.Search_WP.Search_WPUserControl" %>

<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>
<table class="style1">
    <tr>
        <td>
            <asp:Label ID="lblEmpID" runat="server" Text="Enter the text:"></asp:Label>
&nbsp;&nbsp;
            <asp:TextBox ID="txtEmpId" runat="server"></asp:TextBox>
&nbsp;
            <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click"
                Text="Search" />
            <br />
            <br />
        </td>
    </tr>
    <tr>
        <td>
<asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False"
                EnableModelValidation="True">
    <Columns>
        <asp:BoundField DataField="Title" HeaderText="Name" />
    </Columns>
</asp:GridView>
        </td>
    </tr>
</table>