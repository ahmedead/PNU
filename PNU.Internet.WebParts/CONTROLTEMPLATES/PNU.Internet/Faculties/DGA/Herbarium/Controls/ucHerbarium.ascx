<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbarium.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbarium, $SharePoint.Project.AssemblyFullName$" %>
<%@ Register TagPrefix="pnu" TagName="HerbariumHeader" Src="~/_controltemplates/15/PNU.Internet/Faculties/DGA/Herbarium/Controls/ucHerbariumHeader.ascx" %>
<%@ Register TagPrefix="pnu" TagName="HerbariumOverview" Src="~/_controltemplates/15/PNU.Internet/Faculties/DGA/Herbarium/Controls/ucHerbariumOverview.ascx" %>
<%@ Register TagPrefix="pnu" TagName="HerbariumMission" Src="~/_controltemplates/15/PNU.Internet/Faculties/DGA/Herbarium/Controls/ucHerbariumMission.ascx" %>
<%@ Register TagPrefix="pnu" TagName="HerbariumServices" Src="~/_controltemplates/15/PNU.Internet/Faculties/DGA/Herbarium/Controls/ucHerbariumServices.ascx" %>
<%@ Register TagPrefix="pnu" TagName="HerbariumMilestones" Src="~/_controltemplates/15/PNU.Internet/Faculties/DGA/Herbarium/Controls/ucHerbariumMilestones.ascx" %>
<%@ Register TagPrefix="pnu" TagName="HerbariumContact" Src="~/_controltemplates/15/PNU.Internet/Faculties/DGA/Herbarium/Controls/ucHerbariumContact.ascx" %>

<article aria-labelledby="herbarium-sub-title">
    <pnu:HerbariumHeader id="ucHeader" runat="server" />
    <pnu:HerbariumOverview id="ucOverview" runat="server" />
    <pnu:HerbariumMission id="ucMission" runat="server" />
    <pnu:HerbariumServices id="ucServices" runat="server" />
    <pnu:HerbariumMilestones id="ucMilestones" runat="server" />
    <pnu:HerbariumContact id="ucContact" runat="server" />
</article>
