<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInternationalStudents.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls.ucInternationalStudents, $SharePoint.Project.AssemblyFullName$" %>

<%@ Register TagPrefix="pnu" TagName="IntlNumbers"       Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlNumbers.ascx" %>
<%@ Register TagPrefix="pnu" TagName="IntlFeatures"      Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlFeatures.ascx" %>
<%@ Register TagPrefix="pnu" TagName="IntlAdmission"     Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlAdmission.ascx" %>
<%@ Register TagPrefix="pnu" TagName="IntlBeforeArrival" Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlBeforeArrival.ascx" %>
<%@ Register TagPrefix="pnu" TagName="IntlOnArrival"     Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlOnArrival.ascx" %>
<%@ Register TagPrefix="pnu" TagName="IntlOffice"        Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlOffice.ascx" %>

<%@ Register TagPrefix="pnu" TagName="IntlExchange"      Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlExchange.ascx" %>
<%@ Register TagPrefix="pnu" TagName="IntlDocuments"     Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlDocuments.ascx" %>
<%@ Register TagPrefix="pnu" TagName="IntlContact"       Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlContact.ascx" %>

<%@ Register TagPrefix="pnu" TagName="IntlActivities"    Src="~/_controltemplates/15/PNU.Internet/International/Controls/ucIntlActivities.ascx"%>



<pnu:IntlNumbers       ID="ucNumbers"       runat="server" />
<pnu:IntlFeatures      ID="ucFeatures"      runat="server" />
<pnu:IntlAdmission     ID="ucAdmission"     runat="server" />
<pnu:IntlBeforeArrival ID="ucBeforeArrival" runat="server" />
<pnu:IntlOnArrival     ID="ucOnArrival"     runat="server" />
<pnu:IntlOffice        ID="ucOffice"        runat="server" />
<pnu:IntlActivities    ID="ucActivities"    runat="server" />
<pnu:IntlExchange      ID="ucExchange"      runat="server" />
<pnu:IntlDocuments     ID="ucDocuments"     runat="server" />
<pnu:IntlContact       ID="ucContact"       runat="server" />
