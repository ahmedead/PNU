<%-- ValidateRequest="false": the editor posts raw page HTML back, which ASP.NET request
     validation would otherwise reject. Access is gated by RightsRequired in the code-behind
     and by a per-page EditListItems check inside the user control. --%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageContentEditor.aspx.cs" Inherits="PNU.Internet.WebParts.Layouts.PNU.Internet.ContentManagement.PageContentEditor" MasterPageFile="~masterurl/default.master" ValidateRequest="false" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/ContentManagement/ucGenericPageContentEditor.ascx" TagPrefix="pnu" TagName="ContentEditor" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    محرر محتوى صفحات البوابة | جامعة الأميرة نورة بنت عبد الرحمن
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    محرر صفحات البوابة الإلكترونية
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <%-- Required by SPUtility.ValidateFormDigest() in the control's postback handlers --%>
    <SharePoint:FormDigest ID="pnuFormDigest" runat="server" />
    <div style="padding: 20px; max-width: 1200px; margin: 0 auto;">
        <pnu:ContentEditor ID="pnuContentEditor" runat="server" />
    </div>
</asp:Content>
