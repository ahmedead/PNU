<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCheckOutCheckIn.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucCheckOutCheckIn" %>







<div class="row">
    <div class="form-group col-md-6">
    <asp:Label ID="lblWebURL" AssociatedControlID="txtWebURL" runat="server" CssClass="form-label required" Text="WebURL"></asp:Label>
    <asp:TextBox ID="txtWebURL" runat="server"></asp:TextBox>
    
</div>

    <div class="form-group col-md-6">
        <asp:Label ID="FolderPath" AssociatedControlID="txtFolderPath" runat="server" CssClass="form-label required" Text="Folder Path"></asp:Label>
        <asp:TextBox ID="txtFolderPath" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtFolderPath" runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    

</div>

<div class="row" >
    <div class="form-group col-md-4">
        <asp:Button ID="btnCheckIn" runat="server" Text="CheckIn" OnClick="btnCheckIn_Click" ValidationGroup="AddRequest" />
    </div>
    <div class="form-group col-md-4">
        <asp:Button ID="btnCheckOut" runat="server" Text="CheckOut" OnClick="btnCheckOut_Click" ValidationGroup="AddRequest" />
    </div>

</div>