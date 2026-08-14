<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbariumContact.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbariumContact, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5" aria-labelledby="herbarium-contact-title">
    <div class="container">
        <h2 class="h3 mb-4" id="herbarium-contact-title"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
        <article class="card nav-card h-100">
            <div class="card-body d-flex flex-column gap-4">
                <div class="icon-container"><i class="<asp:Literal ID='ltContactIcon' runat='server' />" aria-hidden="true"></i></div>
                <div>
                    <h3 class="h5 card-title"><asp:Literal ID="ltContactTitle" runat="server" /></h3>
                    <p class="card-text"><asp:Literal ID="ltContactDesc" runat="server" /></p>
                    <div class="d-flex flex-column gap-2">
                        <asp:PlaceHolder ID="phEmail" runat="server">
                            <a class="d-flex align-items-center gap-2 text-decoration-none" href="mailto:<asp:Literal ID='ltEmailLink' runat='server' />">
                                <i class="hgi hgi-stroke hgi-mail-01 fs-5" aria-hidden="true"></i>
                                <span dir="ltr"><asp:Literal ID="ltEmailText" runat="server" /></span>
                            </a>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phPhone" runat="server">
                            <a class="d-flex align-items-center gap-2 text-decoration-none" href="tel:<asp:Literal ID='ltPhoneLink' runat='server' />">
                                <i class="hgi hgi-stroke hgi-call fs-5" aria-hidden="true"></i>
                                <span dir="ltr"><asp:Literal ID="ltPhoneText" runat="server" /></span>
                            </a>
                        </asp:PlaceHolder>
                    </div>
                </div>
                <div class="d-flex justify-content-end mt-auto">
                    <a class="btn btn-secondary stretched-link" href="mailto:<asp:Literal ID='ltButtonEmailLink' runat='server' />" aria-label="مراسلة المعشبة النباتية">
                        <span><asp:Literal ID="ltButtonText" runat="server" /></span>
                        <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                    </a>
                </div>
            </div>
        </article>
    </div>
</section>
