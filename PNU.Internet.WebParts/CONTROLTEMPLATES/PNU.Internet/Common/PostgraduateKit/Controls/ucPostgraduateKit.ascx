<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPostgraduateKit.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPostgraduateKit" %>

<%@ Register TagPrefix="pnu" TagName="PgStyles" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgStyles.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgHeader" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgHeader.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgIntro" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgIntro.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgJourney" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgJourney.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgResources" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgResources.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgContentBlock" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgContentBlock.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgProcedures" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgProcedures.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgFaq" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgFaq.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgDiscounts" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgDiscounts.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgServices" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgServices.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PgQuickLinks" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgQuickLinks.ascx" %>

<pnu:PgStyles ID="ucStyles" runat="server" />

<asp:PlaceHolder ID="phHeader" runat="server">
    <pnu:PgHeader ID="ucHeader" runat="server" />
</asp:PlaceHolder>

<main id="main-content" class="dga-main-body pg-kit" tabindex="-1">
    <section class="gray colored-section py-5" data-aos="fade-up">
        <div class="container">
            <div class="mb-4">
                <div class="row gap-5">

                    <asp:PlaceHolder ID="phIntro" runat="server">
                        <pnu:PgIntro ID="ucIntro" runat="server" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phJourney" runat="server">
                        <pnu:PgJourney ID="ucJourney" runat="server" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phPrograms" runat="server">
                        <pnu:PgResources ID="ucPrograms" runat="server" GroupKey="Programs" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phRegulations" runat="server">
                        <pnu:PgResources ID="ucRegulations" runat="server" GroupKey="Regulations" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phGraduateIdentity" runat="server">
                        <pnu:PgContentBlock ID="ucGraduateIdentity" runat="server" BlockKey="GraduateIdentity" ShowTitle="False" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phFinancialRules" runat="server">
                        <pnu:PgContentBlock ID="ucFinancialRules" runat="server" BlockKey="FinancialRules" ShowTitle="False" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phGradRequirements" runat="server">
                        <pnu:PgResources ID="ucGradRequirements" runat="server" GroupKey="GradRequirements" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phEnglishCourse" runat="server">
                        <pnu:PgContentBlock ID="ucEnglishCourse" runat="server" BlockKey="EnglishCourse" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phProcedures" runat="server">
                        <pnu:PgProcedures ID="ucProcedures" runat="server" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phFaq" runat="server">
                        <pnu:PgFaq ID="ucFaq" runat="server" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phPublishIncentive" runat="server">
                        <pnu:PgContentBlock ID="ucPublishIncentive" runat="server" BlockKey="PublishIncentive" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phGuidesEarly" runat="server">
                        <pnu:PgResources ID="ucGuidesEarly" runat="server" GroupKey="Guides" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phDiscounts" runat="server">
                        <pnu:PgDiscounts ID="ucDiscounts" runat="server" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phGuidesLate" runat="server">
                        <pnu:PgResources ID="ucGuidesLate" runat="server" GroupKey="Guides" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phServices" runat="server">
                        <pnu:PgServices ID="ucServices" runat="server" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phHousing" runat="server">
                        <pnu:PgQuickLinks ID="ucHousing" runat="server" SectionKey="Housing" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phContact" runat="server">
                        <pnu:PgQuickLinks ID="ucContact" runat="server" SectionKey="Contact" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phFooterNote" runat="server">
                        <pnu:PgContentBlock ID="ucFooterNote" runat="server" BlockKey="Footer" ShowTitle="False" />
                    </asp:PlaceHolder>

                </div>
            </div>
        </div>
    </section>
</main>
