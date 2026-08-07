<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageGenerator.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.PageGenerator" %>


<div class="page-generator-panel" dir="rtl">
    <h3>مولد الصفحات من قائمة SharePoint</h3>
    
    <asp:Panel ID="pnlControls" runat="server" CssClass="controls">
        <asp:Label ID="lblListName" runat="server" Text="اسم القائمة:" />
        <asp:TextBox ID="txtListName" runat="server" Text="PagesToGenerate" Width="250" />
        <br /><br />
        
        <asp:CheckBox ID="chkOverwrite" runat="server" 
            Text="الكتابة فوق الصفحات الموجودة" Checked="false" />
        <br /><br />
        
        <asp:Button ID="btnGenerate" runat="server" 
            Text="إنشاء الصفحات" OnClick="btnGenerate_Click" 
            CssClass="ms-ButtonHeightWidth" />
    </asp:Panel>
    
    <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="results">
        <h4>نتائج المعالجة:</h4>
        <asp:Literal ID="litResults" runat="server" />
    </asp:Panel>
</div>

<style>
    .page-generator-panel { padding: 15px; font-family: Tahoma, Arial; }
    .results { margin-top: 20px; padding: 10px; background: #f5f5f5; border: 1px solid #ddd; }
    .success { color: green; }
    .error { color: red; }
    .warning { color: orange; }
</style>