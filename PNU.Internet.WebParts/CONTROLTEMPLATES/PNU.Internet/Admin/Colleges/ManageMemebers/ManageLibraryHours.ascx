<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageLibraryHours.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers.ManageLibraryHours" %>



<div id="Library_alert_container" runat="server"></div>

<div id="dvMain" runat="server">

    <article class="card mb-4">
        <div class="card-body">
            <h3 class="h5 fw-bold mb-4">
                <asp:Literal ID="lit_OfficeHours" runat="server" Text="<%$Resources:PnuInternetResources, res_OfficeHours%>"></asp:Literal>
            </h3>

            <div class="row g-4">
                <div class="col-12">
                    <asp:Label ID="lblEmail" runat="server" CssClass="form-control disabled bg-body-tertiary"></asp:Label>
                </div>

                <div class="col-12 col-md-8">
                    <label class="form-label">
                        <asp:Literal ID="lit_Address3" runat="server" Text="<%$Resources:PnuInternetResources, res_Address%>"></asp:Literal></label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvfTitle" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" ValidationGroup="libgroup" Style="color: red;" ControlToValidate="txtTitle" CssClass="required" Display="dynamic" />
                </div>

                <div class="col-12 col-md-4">
                    <label class="form-label">
                        <asp:Literal ID="lit_sort1" runat="server" Text="<%$Resources:PnuInternetResources, res_Sort%>"></asp:Literal></label>
                    <asp:TextBox ID="txtOrder" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" ValidationGroup="libgroup" Style="color: red" ControlToValidate="txtOrder" CssClass="required" Display="dynamic" />
                </div>

                <div class="col-12">
                    <label class="form-label">
                        <asp:Literal ID="lit_days" runat="server" Text="<%$Resources:PnuInternetResources, res_Days%>"></asp:Literal></label>
                    <asp:CheckBoxList ID="chkDays" runat="server" CssClass="dga-days-list" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Text="<%$Resources:PnuInternetResources, res_Saturday%>" Value="<%$Resources:PnuInternetResources, res_Saturday%>"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:PnuInternetResources, res_Sunday%>" Value="<%$Resources:PnuInternetResources, res_Sunday%>"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:PnuInternetResources, res_Monday%>" Value="<%$Resources:PnuInternetResources, res_Monday%>"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:PnuInternetResources, res_Tuesday%>" Value="<%$Resources:PnuInternetResources, res_Tuesday%>"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:PnuInternetResources, res_wednsday%>" Value="<%$Resources:PnuInternetResources, res_wednsday%>"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:PnuInternetResources, res_Thurthday%>" Value="<%$Resources:PnuInternetResources, res_Thurthday%>"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:PnuInternetResources, res_Friday%>" Value="<%$Resources:PnuInternetResources, res_Friday%>"></asp:ListItem>
                    </asp:CheckBoxList>
                    <asp:CustomValidator ID="rfvchkDays" runat="server" Enabled="true" ClientValidationFunction="ValidateDays"
                        Text="<%$Resources:PnuInternetResources, res_RequiredField%>"
                        ValidationGroup="libgroup" Display="Dynamic" Style="color: red" />
                </div>

                <div class="col-12 col-md-6">
                    <label class="form-label">
                        <asp:Literal ID="lit_from" runat="server" Text="<%$Resources:PnuInternetResources, res_From%>"></asp:Literal></label>
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" ValidationGroup="libgroup" Style="color: red" ControlToValidate="txtFrom" CssClass="required" Display="dynamic" />
                </div>

                <div class="col-12 col-md-6">
                    <label class="form-label">
                        <asp:Literal ID="lit_To" runat="server" Text="<%$Resources:PnuInternetResources, res_To%>"></asp:Literal></label>
                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" ValidationGroup="libgroup" Style="color: red" ControlToValidate="txtTo" CssClass="required" Display="dynamic" />
                </div>

                <div class="col-12 d-flex justify-content-center">
                    <asp:Button ID="btnSubmit" runat="server" ValidationGroup="libgroup" Text="<%$Resources:PnuInternetResources, res_Save%>" OnClick="btnSubmit_Click" CssClass="btn btn-primary px-5" />
                </div>
            </div>
        </div>
    </article>

    <article class="card">
        <div class="card-body">
            <asp:Repeater ID="rptLib" runat="server" OnItemCommand="rptLib_ItemCommand">
                <ItemTemplate>
                    <div class="d-flex flex-column gap-2 my-4 py-2">
                        <div><a><%# Eval("Title") %></a></div>
                        <div class="d-flex flex-wrap align-items-center gap-3">
                            <span class="small text-body-secondary">
                                <i class="hgi hgi-stroke hgi-calendar-01 me-1" aria-hidden="true"></i>
                                <%# Eval("Days1") %>
                            </span>
                            <span class="small text-body-secondary">
                                <i class="hgi hgi-stroke hgi-time-02 me-1" aria-hidden="true"></i>
                                <%# Eval("TimeFrom") %> - <%# Eval("TimeTo") %>
                            </span>
                            <asp:Button ID="btnDelLib" runat="server" CssClass="btn btn-outline-danger btn-sm px-3" Text="<%$Resources:PnuInternetResources, res_delete%>" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' />
                        </div>
                    </div>
                    <hr class="my-0">
                </ItemTemplate>
            </asp:Repeater>

            <%-- Kept for code-behind compatibility --%>
            <div class="visually-hidden" aria-hidden="true">
                <asp:LinkButton ID="lbPreviousLib" runat="server" OnClick="lbPrevious_Click" Visible="false">Previous</asp:LinkButton>
                <asp:LinkButton ID="lbNextLib" runat="server" OnClick="lbNext_Click" Visible="false">Next</asp:LinkButton>
                <asp:Label ID="lblpageLib" runat="server" Text="" Visible="false"></asp:Label>
            </div>

            <nav class="mt-4" aria-label="Office hours pagination">
                <ul class="pagination justify-content-center align-items-center gap-2 dga-pagination">
                    <li class="page-item">
                        <asp:LinkButton ID="lbFirstLib" CssClass="page-link" runat="server" OnClick="lbFirst_Click">&laquo;</asp:LinkButton>
                    </li>
                    <li class="page-item">
                        <asp:DataList ID="rptPaging" runat="server" OnItemCommand="rptPaging_ItemCommand" OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal" CssClass="dga-paging-list">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbPaging" runat="server" CssClass="page-link" CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPageLib" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </li>
                    <li class="page-item">
                        <asp:LinkButton ID="lbLastLib" CssClass="page-link" runat="server" OnClick="lbLast_Click">&raquo;</asp:LinkButton>
                    </li>
                </ul>
            </nav>
        </div>
    </article>

    <style>
        .dga-days-list label { margin-inline: 6px 16px; }
        .dga-days-list input[type="checkbox"] { margin-inline-end: 4px; }
    </style>
</div>
