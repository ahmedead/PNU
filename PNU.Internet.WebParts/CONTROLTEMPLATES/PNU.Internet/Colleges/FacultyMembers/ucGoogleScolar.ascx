<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGoogleScolar.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucGoogleScolar" %>

<%-- Google Scholar profile block (HTML injected from code-behind) --%>
<article class="card mb-4" dir="auto">
    <div class="card-body" id="divMainProfile" runat="server">
    </div>
</article>

<asp:UpdatePanel ID="updatepnl" runat="server">
    <ContentTemplate>

        <asp:Repeater ID="rptData" runat="server">
            <ItemTemplate>
                <div class="d-flex flex-column gap-2 my-4 py-2" dir="auto">
                    <div class="d-flex flex-wrap gap-2">
                        <span class="badge badge-info"><%# DataBinder.Eval(Container.DataItem, "PublicationYear") %></span>
                        <span class="badge badge-success"><%# DataBinder.Eval(Container.DataItem, "Magazine") %></span>
                    </div>
                    <div>
                        <a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' target="_blank" rel="noopener noreferrer">
                            <%# DataBinder.Eval(Container.DataItem, "ID") %> - <%# DataBinder.Eval(Container.DataItem, "Title") %>
                        </a>
                    </div>
                    <p class="mb-0 line-clamp max-clamp-line-2">
                        <%# DataBinder.Eval(Container.DataItem, "Authors") %>
                    </p>
                    <div>
                        <span class="small text-body-secondary">
                            <i class="hgi hgi-stroke hgi-quote-up me-1" aria-hidden="true"></i>
                            Cited by: <%# DataBinder.Eval(Container.DataItem, "CitationCount") %>
                        </span>
                    </div>
                </div>
                <hr class="my-0">
            </ItemTemplate>
        </asp:Repeater>

        <%-- Kept for code-behind compatibility (Previous/Next/PageLabel) --%>
        <div class="visually-hidden" aria-hidden="true">
            <asp:LinkButton ID="lbPrevious" runat="server" OnClick="lbPrevious_Click" Visible="false">Previous</asp:LinkButton>
            <asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click" Visible="false">Next</asp:LinkButton>
            <asp:Label ID="lblpage" runat="server" Text="" Visible="false"></asp:Label>
        </div>

        <nav class="mt-5" aria-label="Publications pagination">
            <ul class="pagination justify-content-center align-items-center gap-2 dga-pagination">
                <li class="page-item">
                    <asp:LinkButton ID="lbFirst" CssClass="page-link" runat="server" OnClick="lbFirst_Click">&laquo;</asp:LinkButton>
                </li>
                <li class="page-item">
                    <asp:DataList ID="rptPaging" runat="server" OnItemCommand="rptPaging_ItemCommand" OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal" CssClass="dga-paging-list">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPaging" runat="server" CssClass="page-link" CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPage" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lbLast" CssClass="page-link" runat="server" OnClick="lbLast_Click">&raquo;</asp:LinkButton>
                </li>
            </ul>
        </nav>

    </ContentTemplate>
</asp:UpdatePanel>

<style>
    /* Google Scholar injected profile — DGA-friendly overrides */
    a#gsc_prf_btnf { display: none; }
    #gsc_prf_pup-img { width: 100px; height: 128px; border-radius: 50%; }
    #gsc_prf_w { padding: 16px 0; overflow: hidden; }
    #gsc_prf_in { font-size: 24px; line-height: 32px; font-weight: 600; word-wrap: break-word; }
    .gsc_md_pro_tt, #gsc_md_pro_lgtm, #gsc_md_pro_rev_n, #gsc_md_pro_save,
    #gsc_md_pro_ep, .gsc_md_pro_el, .gsc_md_pro_ev { display: none; }
    .gsc_prf_inta { margin-inline-end: 16px; white-space: nowrap; max-width: 200px; text-overflow: ellipsis; overflow: hidden; vertical-align: top; }
</style>
