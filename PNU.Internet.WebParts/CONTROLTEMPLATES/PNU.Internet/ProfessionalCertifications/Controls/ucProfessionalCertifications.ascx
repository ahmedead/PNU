<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProfessionalCertifications.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls.ucProfessionalCertifications" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/ProfessionalCertifications/Controls/ucPcHeader.ascx" TagPrefix="uc" TagName="ucPcHeader" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/ProfessionalCertifications/Controls/ucPcGuide.ascx"  TagPrefix="uc" TagName="ucPcGuide" %>

<div class="dga-professional-certifications-wrapper">
    <uc:ucPcHeader ID="ucHeader" runat="server" />
    <uc:ucPcGuide ID="ucGuide" runat="server" />
</div>
