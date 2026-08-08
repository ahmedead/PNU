<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSmartSuitcasePage.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls.ucSmartSuitcasePage, $SharePoint.Project.AssemblyFullName$" %>

<%@ Register TagPrefix="ssc" TagName="Hero"     Src="~/_controltemplates/15/PNU.Internet/SmartSuitcase/Controls/ucSmartSuitcaseHero.ascx" %>
<%@ Register TagPrefix="ssc" TagName="Services" Src="~/_controltemplates/15/PNU.Internet/SmartSuitcase/Controls/ucSmartSuitcaseServices.ascx" %>
<%@ Register TagPrefix="ssc" TagName="Access"   Src="~/_controltemplates/15/PNU.Internet/SmartSuitcase/Controls/ucSmartSuitcaseAccess.ascx" %>
<%@ Register TagPrefix="ssc" TagName="Faq"      Src="~/_controltemplates/15/PNU.Internet/SmartSuitcase/Controls/ucSmartSuitcaseFaq.ascx" %>
<%@ Register TagPrefix="ssc" TagName="Support"  Src="~/_controltemplates/15/PNU.Internet/SmartSuitcase/Controls/ucSmartSuitcaseSupport.ascx" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <ssc:Hero     ID="ucHero"     runat="server" />
    <ssc:Services ID="ucServices" runat="server" />
    <ssc:Access   ID="ucAccess"   runat="server" />
    <ssc:Faq      ID="ucFaq"      runat="server" />
    <ssc:Support  ID="ucSupport"  runat="server" />
</main>
