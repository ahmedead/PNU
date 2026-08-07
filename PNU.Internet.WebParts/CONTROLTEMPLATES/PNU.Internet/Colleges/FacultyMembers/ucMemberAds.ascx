<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMemberAds.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucMemberAds" %>

<asp:UpdatePanel ID="updatepn2" runat="server">
    <ContentTemplate>

        <asp:Repeater ID="rptTweets" runat="server">
            <ItemTemplate>
                <div class="d-flex flex-column gap-2 my-4 py-2">
                    <div class="d-flex flex-wrap gap-2">
                        <%-- TODO: replace with a resource key (e.g. res_Announcement) once added to PnuInternetResources --%>
                        <span class="badge badge-info">إعلان</span>
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
            <asp:LinkButton ID="lbPreviousTweets" runat="server" OnClick="lbPrevious_Click" Visible="false">Previous</asp:LinkButton>
            <asp:LinkButton ID="lbNextTweets" runat="server" OnClick="lbNext_Click" Visible="false">Next</asp:LinkButton>
            <asp:Label ID="lblpageTweets" runat="server" Text="" Visible="false"></asp:Label>
        </div>

        <nav class="mt-5" aria-label="Announcements pagination">
            <ul class="pagination justify-content-center align-items-center gap-2 dga-pagination">
                <li class="page-item">
                    <asp:LinkButton ID="lbFirstTweets" CssClass="page-link" runat="server" OnClick="lbFirst_Click">&laquo;</asp:LinkButton>
                </li>
                <li class="page-item">
                    <asp:DataList ID="rptPagingTweets" runat="server" OnItemCommand="rptPaging_ItemCommand" OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal" CssClass="dga-paging-list">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPagingTweets" runat="server" CssClass="page-link" CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPageTweets" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lbLastTweets" CssClass="page-link" runat="server" OnClick="lbLast_Click">&raquo;</asp:LinkButton>
                </li>
            </ul>
        </nav>

    </ContentTemplate>
</asp:UpdatePanel>
