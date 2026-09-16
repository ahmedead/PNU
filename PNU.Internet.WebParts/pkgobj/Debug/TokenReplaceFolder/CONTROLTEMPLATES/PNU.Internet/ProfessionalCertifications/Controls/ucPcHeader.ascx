<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPcHeader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls.ucPcHeader" %>
<div class="bg-primary-25 py-5">
    <div class="container">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2">
                <asp:Repeater ID="rptBreadcrumb" runat="server">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("IsLast"))
                            ? "<li class=\"breadcrumb-item small active\" aria-current=\"page\"><span>" + Eval("Title") + "</span></li>"
                            : "<li class=\"breadcrumb-item small\"><a href=\"" + Eval("Url") + "\">" + Eval("Title") + "</a></li>" %>
                    </ItemTemplate>
                </asp:Repeater>
            </ol>
        </nav>
        <div class="d-flex flex-column flex-lg-row align-items-start justify-content-between gap-3">
            <h1 class="mb-0 fw-semibold h2"><asp:Literal ID="litTitle" runat="server" /></h1>
            <button class="btn btn-outline-secondary flex-shrink-0 gap-2" type="button" id="printCertificationListButton">
                <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-printer" aria-hidden="true"></i></span>
                <span><asp:Literal ID="litExportBtn" runat="server" /></span>
            </button>
        </div>
        <div class="col-12 col-lg-10 mt-4">
            <p><asp:Literal ID="litDesc" runat="server" /></p>
            <asp:Panel ID="pnlParagraph2" runat="server" Visible="false">
                <p class="mb-0"><asp:Literal ID="litParagraph2" runat="server" /></p>
            </asp:Panel>
        </div>
    </div>
</div>
