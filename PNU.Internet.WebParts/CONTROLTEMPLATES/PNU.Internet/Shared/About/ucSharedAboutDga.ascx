<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSharedAboutDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About.ucSharedAboutDga" %>
<%@ Register TagPrefix="pnu" TagName="AgencyOverview"      Src="~/_controltemplates/15/PNU.Internet/Shared/About/ucAgencyOverviewDga.ascx" %>
<%@ Register TagPrefix="pnu" TagName="AgencyDeputyWelcome" Src="~/_controltemplates/15/PNU.Internet/Shared/About/ucAgencyDeputyWelcomeDga.ascx" %>
<%@ Register TagPrefix="pnu" TagName="AgencyTasks"         Src="~/_controltemplates/15/PNU.Internet/Shared/About/ucAgencyTasksDga.ascx" %>

<div class="d-flex flex-column gap-4">
    <pnu:AgencyOverview      ID="ucOverview" runat="server" />
    <pnu:AgencyDeputyWelcome ID="ucDeputy"   runat="server" />
    <pnu:AgencyTasks         ID="ucTasks"    runat="server" />
</div>
