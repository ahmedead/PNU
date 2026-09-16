<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMemberTweets.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucMemberTweets" %>

<asp:UpdatePanel ID="updatepn2" runat="server">
    <ContentTemplate>

        <asp:Repeater ID="rptAdvs" runat="server">
            <ItemTemplate>
                <div class="d-flex flex-column gap-2 my-4 py-2">
                    <div class="d-flex flex-wrap gap-2">
                        <span class="badge badge-info">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, Tweets %>" />
                        </span>
                    </div>
                    <div><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>'><%# DataBinder.Eval(Container.DataItem, "Title") %></a></div>
                    <p class="mb-0 line-clamp max-clamp-line-2"><%# DataBinder.Eval(Container.DataItem, "Desc") %></p>
                    <div>
                        <span class="small text-body-secondary">
                            <i class="hgi hgi-stroke hgi-calendar-01 me-1" aria-hidden="true"></i>
                            <%# DataBinder.Eval(Container.DataItem, "Date") %>
                        </span>
                    </div>
                </div>
                <hr class="my-0">
            </ItemTemplate>
        </asp:Repeater>

        <%-- Kept for code-behind compatibility (Previous/Next/PageLabel) --%>
        <div class="visually-hidden" aria-hidden="true">
            <asp:LinkButton ID="lbPreviousAdv" runat="server" OnClick="lbPrevious_Click" Visible="false">Previous</asp:LinkButton>
            <asp:LinkButton ID="lbNextAdv" runat="server" OnClick="lbNext_Click" Visible="false">Next</asp:LinkButton>
            <asp:Label ID="lblpageAdv" runat="server" Text="" Visible="false"></asp:Label>
        </div>

        <nav class="mt-5" aria-label="Tweets pagination">
            <ul class="pagination justify-content-center align-items-center gap-2 dga-pagination">
                <li class="page-item">
                    <asp:LinkButton ID="lbFirstAdv" CssClass="page-link" runat="server" OnClick="lbFirst_Click">&laquo;</asp:LinkButton>
                </li>
                <li class="page-item">
                    <asp:DataList ID="rptPagingAdv" runat="server" OnItemCommand="rptPaging_ItemCommand" OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal" CssClass="dga-paging-list">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPagingAdv" runat="server" CssClass="page-link" CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPageAdv" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lbLastAdv" CssClass="page-link" runat="server" OnClick="lbLast_Click">&raquo;</asp:LinkButton>
                </li>
            </ul>
        </nav>

    </ContentTemplate>
</asp:UpdatePanel>
