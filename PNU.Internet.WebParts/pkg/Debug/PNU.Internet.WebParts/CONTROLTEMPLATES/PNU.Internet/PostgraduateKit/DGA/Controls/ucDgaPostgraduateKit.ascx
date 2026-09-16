<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaPostgraduateKit.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls.ucDgaPostgraduateKit" %>
<%@ Register TagPrefix="pnu" TagName="KitHub" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitHub.ascx" %>
<%@ Register TagPrefix="pnu" TagName="KitApplicants" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitApplicants.ascx" %>
<%@ Register TagPrefix="pnu" TagName="KitEnrolled" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitEnrolled.ascx" %>
<%@ Register TagPrefix="pnu" TagName="KitGraduates" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitGraduates.ascx" %>

<asp:PlaceHolder ID="phHub" runat="server" Visible="false">
    <pnu:KitHub ID="ucHub" runat="server" />
</asp:PlaceHolder>

<asp:PlaceHolder ID="phApplicants" runat="server" Visible="false">
    <pnu:KitApplicants ID="ucApplicants" runat="server" />
</asp:PlaceHolder>

<asp:PlaceHolder ID="phEnrolled" runat="server" Visible="false">
    <pnu:KitEnrolled ID="ucEnrolled" runat="server" />
</asp:PlaceHolder>

<asp:PlaceHolder ID="phGraduates" runat="server" Visible="false">
    <pnu:KitGraduates ID="ucGraduates" runat="server" />
</asp:PlaceHolder>
