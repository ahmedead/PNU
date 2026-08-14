<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbariumHeader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbariumHeader, $SharePoint.Project.AssemblyFullName$" %>

<header class="mb-4">
    <h2 id="herbarium-sub-title" class="mb-3"><asp:Literal ID="ltTitle" runat="server" /></h2>
    <p class="mb-0"><asp:Literal ID="ltDescription" runat="server" /> <asp:PlaceHolder ID="phSubTitle" runat="server"><span dir="ltr"><asp:Literal ID="ltSubTitle" runat="server" /></span></asp:PlaceHolder></p>
</header>

<section class="py-5" aria-label="صورة المعشبة النباتية">
    <div class="container">
        <figure class="mb-0">
            <asp:Image ID="imgHero" runat="server" CssClass="img-fluid w-100 rounded-2" Width="1592" Height="835" />
            <figcaption class="small text-muted mt-2"><asp:Literal ID="ltCaption" runat="server" /></figcaption>
        </figure>
    </div>
</section>
