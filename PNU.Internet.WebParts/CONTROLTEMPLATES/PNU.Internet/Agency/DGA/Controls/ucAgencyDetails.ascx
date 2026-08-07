<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAgencyDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA.ucAgencyDetails, $SharePoint.Project.AssemblyFullName$" %>

<%@ Register TagPrefix="pnu" TagName="AgOverview"   Src="~/_controltemplates/15/PNU.Internet/Agency/DGA/Controls/ucAgOverview.ascx" %>
<%@ Register TagPrefix="pnu" TagName="AgCards"      Src="~/_controltemplates/15/PNU.Internet/Agency/DGA/Controls/ucAgCards.ascx" %>
<%@ Register TagPrefix="pnu" TagName="AgObjectives" Src="~/_controltemplates/15/PNU.Internet/Agency/DGA/Controls/ucAgObjectives.ascx" %>
<%@ Register TagPrefix="pnu" TagName="AgDeputyWord" Src="~/_controltemplates/15/PNU.Internet/Agency/DGA/Controls/ucAgDeputyWord.ascx" %>
<%@ Register TagPrefix="pnu" TagName="AgMainTasks"  Src="~/_controltemplates/15/PNU.Internet/Agency/DGA/Controls/ucAgMainTasks.ascx" %>

<%-- Tab pane of the "تفاصيل الوكالة" tab set. The page layout owns the tab buttons;
     this control owns the pane and the sections inside it. --%>

<div class="tab-pane fade show active" id="agency-details-tabs-pane-0" role="tabpanel"
     aria-labelledby="agency-details-tabs-tab-0" tabindex="0">
    <div class="d-flex flex-column gap-4">

        <section class="mb-5" aria-labelledby="agency-overview-title">

            <pnu:AgOverview ID="ucOverview" runat="server" />

            <div class="row g-5">
                <pnu:AgCards      ID="ucCards"      runat="server" />
                <pnu:AgObjectives ID="ucObjectives" runat="server" />
                <pnu:AgDeputyWord ID="ucDeputyWord" runat="server" />
            </div>
        </section>

        <pnu:AgMainTasks ID="ucMainTasks" runat="server" />

    </div>
</div>
