<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgHub.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgHub" %>

<%@ Register TagPrefix="pnu" TagName="PgStyles" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgStyles.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgHubHero" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgHubHero.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgIntro" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgIntro.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgHubCards" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgHubCards.ascx" %>

<pnu:PgStyles ID="ucStyles" runat="server" />

<asp:PlaceHolder ID="phHero" runat="server">
    <pnu:PgHubHero ID="ucHero" runat="server" />
</asp:PlaceHolder>

<main id="main-content" class="dga-main-body pg-kit" tabindex="-1">

    <asp:PlaceHolder ID="phIntro" runat="server">
        <section class="gray colored-section py-5" data-aos="fade-up">
            <div class="container">
                <div class="row gap-5">
                    <pnu:PgIntro ID="ucIntro" runat="server" Phase="Hub" />
                </div>
            </div>
        </section>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phCards" runat="server">
        <pnu:PgHubCards ID="ucCards" runat="server" />
    </asp:PlaceHolder>

</main>
