<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search-Criteria-WPUserControl.ascx.cs" Inherits="PNU_SP.webparts.Search_Criteria_WP.Search_Criteria_WPUserControl" %>


<div class="row">
    <div class ="col">
        <asp:TextBox ID="txt" runat="server"></asp:TextBox>
    </div>
    <div class ="col">
        <span>SubCategory : </span>
        <asp:DropDownList ID="ddl1" runat="server">
            <asp:ListItem Text="category1" Value="category1"></asp:ListItem>
             <asp:ListItem Text="category2" Value="category2"></asp:ListItem>
             <asp:ListItem Text="category3" Value="category3"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class ="col">
         <span>Faculty : </span>
         <asp:DropDownList ID="ddl2" runat="server">
            <asp:ListItem Text="Engineering" Value="Engineering"></asp:ListItem>
             <asp:ListItem Text="Medicine" Value="Medicine"></asp:ListItem>
             <asp:ListItem Text="Art" Value="Art"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class ="col">
        <asp:Calendar ID="cal" runat="server"></asp:Calendar>
    </div>

    <div class="col">
        <asp:Button ID="btn" runat="server" Text="search" OnClick="btn_Click" />
    </div>

</div>
<div class="row">
    <div class="col">
        <asp:Label ID="lbl1" runat="server"></asp:Label>
    </div>
    <div class="col">
        <asp:Label ID="lbl2" runat="server"></asp:Label>
    </div>
</div>