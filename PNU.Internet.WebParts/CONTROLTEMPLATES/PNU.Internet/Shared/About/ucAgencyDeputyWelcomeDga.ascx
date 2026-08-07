<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAgencyDeputyWelcomeDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About.ucAgencyDeputyWelcomeDga" %>

<section id="agency-deputy-welcome" aria-labelledby="agency-deputy-welcome-title">
    <div class="card mb-4 bg-primary-25 border-0">
        <div class="card-body p-4 p-lg-5">
            <div class="d-flex flex-column gap-3">
                <span class="icon-container bg-white">
                    <i class="hgi hgi-stroke hgi-quote-down fs-4" aria-hidden="true"></i>
                </span>
                <h2 id="agency-deputy-welcome-title" class="mb-0">
                    <asp:Literal ID="litDeputyHeading" runat="server" /></h2>
                <p class="lead mb-0 text-justify">
                    <asp:Literal ID="litDeputyBody" runat="server" /></p>
            </div>
            <div class="card-body">
                <div>
                    <h3 class="card-title"><asp:Literal ID="litDeputyRoleTitle" runat="server" /></h3>
                    <p class="card-text mb-0"><asp:Literal ID="litDeputyRoleSubtitle" runat="server" /></p>
                </div>
            </div>
        </div>
    </div>
</section>
