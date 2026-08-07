<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNouraStudents.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls.ucNouraStudents, $SharePoint.Project.AssemblyFullName$" %>

<%@ Register TagPrefix="pnu" TagName="NsNumbers"          Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsNumbers.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsAwards"           Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsAwards.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsAcademicServices" Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsAcademicServices.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsQuickLinks"       Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsQuickLinks.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsStudentServices"  Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsStudentServices.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsImportantDates"   Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsImportantDates.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsCampusLife"       Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsCampusLife.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsCareer"           Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsCareer.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsExperiences"      Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsExperiences.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsFinancialSupport" Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsFinancialSupport.ascx" %>
<%@ Register TagPrefix="pnu" TagName="NsContact"          Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNsContact.ascx" %>

<%-- The master page already renders <main id="main-content" class="dga-main-body">.
     This container therefore only emits <section> elements. --%>

<pnu:NsNumbers          ID="ucNumbers"          runat="server" />
<pnu:NsAwards           ID="ucAwards"           runat="server" />
<pnu:NsAcademicServices ID="ucAcademicServices" runat="server" />
<pnu:NsQuickLinks       ID="ucQuickLinks"       runat="server" />
<pnu:NsStudentServices  ID="ucStudentServices"  runat="server" />
<pnu:NsImportantDates   ID="ucImportantDates"   runat="server" />
<pnu:NsCampusLife       ID="ucCampusLife"       runat="server" />
<pnu:NsCareer           ID="ucCareer"           runat="server" />
<pnu:NsExperiences      ID="ucExperiences"      runat="server" />
<pnu:NsFinancialSupport ID="ucFinancialSupport" runat="server" />
<pnu:NsContact          ID="ucContact"          runat="server" />
