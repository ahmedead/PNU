<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePosts.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers.ManagePosts" %>




<div id="Posts_alert_container" runat="server"></div>

<div id="dvMain" runat="server">

    <article class="card mb-4">
        <div class="card-body">
            <h3 class="h5 fw-bold mb-4">
                <asp:Literal ID="lit_posts" runat="server" Text="<%$Resources:PnuInternetResources, res_Posts%>"></asp:Literal>
            </h3>

            <div class="row g-4">
                <div class="col-12">
                    <asp:Label ID="lblEmail" runat="server" CssClass="form-control disabled bg-body-tertiary"></asp:Label>
                </div>

                <div class="col-12 col-md-8">
                    <label class="form-label">
                        <asp:Literal ID="lit_Address1" runat="server" Text="<%$Resources:PnuInternetResources, res_Address%>"></asp:Literal></label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvfTitle" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" ValidationGroup="postsgroup" Style="color: red" ControlToValidate="txtTitle" CssClass="required" Display="dynamic" />
                </div>

                <div class="col-12 col-md-4">
                    <label class="form-label">
                        <asp:Literal ID="lit_sort2" runat="server" Text="<%$Resources:PnuInternetResources, res_Sort%>"></asp:Literal></label>
                    <asp:TextBox ID="txtOrder" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" ValidationGroup="postsgroup" Style="color: red" ControlToValidate="txtOrder" CssClass="required" Display="dynamic" />
                </div>

                <div class="col-12">
                    <label class="form-label">
                        <asp:Literal ID="Lit_postdate" runat="server" Text="<%$Resources:PnuInternetResources, res_PostDate%>"></asp:Literal></label>
                    <SharePoint:DateTimeControl ID="PostDate" runat="server" CssClassTextBox="form-control" IsRequiredField="false" />
                </div>

                <div class="col-12">
                    <label class="form-label">
                        <asp:Literal ID="lit_postdetails" runat="server" Text="<%$Resources:PnuInternetResources, res_PostDetails%>"></asp:Literal></label>
                    <asp:TextBox ID="txtSummary" TextMode="MultiLine" runat="server" Rows="3" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvfSummary" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" ValidationGroup="postsgroup" ControlToValidate="txtSummary" CssClass="required" Style="color: red" Display="dynamic" />
                </div>

                <div class="col-12 d-flex justify-content-center">
                    <asp:Button ID="btnSubmit" runat="server" ValidationGroup="postsgroup" Text="<%$Resources:PnuInternetResources, res_Save%>" OnClick="btnSubmit_Click" CssClass="btn btn-primary px-5" />
                </div>
            </div>
        </div>
    </article>

    <article class="card">
        <div class="card-body">
            <asp:Repeater ID="rptPosts" runat="server" OnItemCommand="rptPosts_ItemCommand">
                <ItemTemplate>
                    <div class="d-flex flex-column gap-2 my-4 py-2">
                        <div><a><%# Eval("Title") %></a></div>
                        <p class="mb-0 line-clamp max-clamp-line-2"><%# Eval("Desc") %></p>
                        <div class="d-flex flex-wrap align-items-center gap-3">
                            <span class="small text-body-secondary">
                                <i class="hgi hgi-stroke hgi-calendar-01 me-1" aria-hidden="true"></i>
                                <%# Eval("Date") %>
                            </span>
                            <asp:Button ID="btnDelPost" runat="server" CssClass="btn btn-outline-danger btn-sm px-3" Text="<%$Resources:PnuInternetResources, res_delete%>" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' />
                        </div>
                    </div>
                    <hr class="my-0">
                </ItemTemplate>
            </asp:Repeater>

            <%-- Kept for code-behind compatibility --%>
            <div class="visually-hidden" aria-hidden="true">
                <asp:LinkButton ID="lbPreviousPosts" runat="server" OnClick="lbPrevious_Click" Visible="false">Previous</asp:LinkButton>
                <asp:LinkButton ID="lbNextPosts" runat="server" OnClick="lbNext_Click" Visible="false">Next</asp:LinkButton>
                <asp:Label ID="lblpagePosts" runat="server" Text="" Visible="false"></asp:Label>
            </div>

            <nav class="mt-4" aria-label="Posts pagination">
                <ul class="pagination justify-content-center align-items-center gap-2 dga-pagination">
                    <li class="page-item">
                        <asp:LinkButton ID="lbFirstPosts" CssClass="page-link" runat="server" OnClick="lbFirst_Click">&laquo;</asp:LinkButton>
                    </li>
                    <li class="page-item">
                        <asp:DataList ID="rptPagingPosts" runat="server" OnItemCommand="rptPaging_ItemCommand" OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal" CssClass="dga-paging-list">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbPagingPosts" runat="server" CssClass="page-link" CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPage" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </li>
                    <li class="page-item">
                        <asp:LinkButton ID="lbLastPosts" CssClass="page-link" runat="server" OnClick="lbLast_Click">&raquo;</asp:LinkButton>
                    </li>
                </ul>
            </nav>
        </div>
    </article>

</div>
