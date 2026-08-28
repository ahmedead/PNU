<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCentralLibrary.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucCentralLibrary" %>

<%@ Register TagPrefix="pnu" TagName="ClHeader" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClHeader.ascx" %>
<%@ Register TagPrefix="pnu" TagName="ClAbout" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClAbout.ascx" %>
<%@ Register TagPrefix="pnu" TagName="ClAwards" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClAwards.ascx" %>
<%@ Register TagPrefix="pnu" TagName="ClServices" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClServices.ascx" %>
<%@ Register TagPrefix="pnu" TagName="ClCollections" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClCollections.ascx" %>
<%@ Register TagPrefix="pnu" TagName="ClFacilities" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClFacilities.ascx" %>
<%@ Register TagPrefix="pnu" TagName="ClFaq" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClFaq.ascx" %>
<%@ Register TagPrefix="pnu" TagName="ClContact" Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClContact.ascx" %>

<asp:PlaceHolder ID="phHeader" runat="server">
    <pnu:ClHeader ID="ucHeader" runat="server" />
</asp:PlaceHolder>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <asp:PlaceHolder ID="phAbout" runat="server">
        <pnu:ClAbout ID="ucAbout" runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phAwards" runat="server">
        <pnu:ClAwards ID="ucAwards" runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phServices" runat="server">
        <pnu:ClServices ID="ucServices" runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phCollections" runat="server">
        <pnu:ClCollections ID="ucCollections" runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phFacilities" runat="server">
        <pnu:ClFacilities ID="ucFacilities" runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phFaq" runat="server">
        <pnu:ClFaq ID="ucFaq" runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phContact" runat="server">
        <pnu:ClContact ID="ucContact" runat="server" />
    </asp:PlaceHolder>
</main>
