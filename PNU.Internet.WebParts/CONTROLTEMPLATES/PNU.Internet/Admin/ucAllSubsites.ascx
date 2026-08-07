<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllSubsites.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucAllSubsites" %>




<div class="row">

<asp:TreeView ID="tvSubsites" runat="server" ShowCheckBoxes="None" ExpandDepth="1" 
              OnSelectedNodeChanged="tvSubsites_SelectedNodeChanged" CssClass="tree-view">
</asp:TreeView>

</div>
        <div class="row">
    <div class="form-group col-md-6">
        <asp:Label ID="lblDeptTitle" AssociatedControlID="txtDeptTitle" runat="server" CssClass="form-label required" Text="Department Title"></asp:Label>
        <asp:TextBox ID="txtDeptTitle" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtDeptTitle"  runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group col-md-6">
    <asp:Label ID="lblDeptCode" AssociatedControlID="txtDeptCode" runat="server" CssClass="form-label required" Text="Department Code"></asp:Label>
    <asp:TextBox ID="txtDeptCode" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtDeptCode"  runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
</div>

</div>



<style>
    td {
    padding: 5px;
}
</style>