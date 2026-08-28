<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClAdmin, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5 cl-admin">
    <div class="container">

        <div class="mb-4">
            <div class="d-flex justify-content-between align-items-center gap-2">
                <h2 class="mb-0"><asp:Literal ID="ltPageTitle" runat="server" /></h2>
            </div>
        </div>

        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-warning" role="alert">
                <asp:Literal ID="ltDenied" runat="server" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlMain" runat="server">

            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div class="alert alert-success" role="alert">
                    <asp:Literal ID="ltMessage" runat="server" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger" role="alert">
                    <asp:Literal ID="ltError" runat="server" />
                </div>
            </asp:Panel>

            <div class="card mb-4">
                <div class="card-body">
                    <div class="row g-3 align-items-end">
                        <div class="col-12 col-md-6">
                            <asp:Label ID="lblListLabel" runat="server" CssClass="form-label" AssociatedControlID="ddlList" />
                            <asp:DropDownList ID="ddlList" runat="server" CssClass="form-select"
                                              AutoPostBack="true" OnSelectedIndexChanged="ddlList_SelectedIndexChanged" />
                        </div>
                        <div class="col-12 col-md-6 d-flex justify-content-md-end gap-2 flex-wrap">
                            <asp:LinkButton ID="btnSeed" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnSeed_Click" CausesValidation="false">
                                <i class="hgi hgi-stroke hgi-database-01 fs-5 align-middle" aria-hidden="true"></i>
                                <asp:Literal ID="ltSeed" runat="server" />
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary" OnClick="btnNew_Click">
                                <i class="hgi hgi-stroke hgi-add-01 fs-5 align-middle" aria-hidden="true"></i>
                                <asp:Literal ID="ltNew" runat="server" />
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <%-- add / edit form --%>
            <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4">
                <div class="card-body">
                    <h3 class="h5 mb-4"><asp:Literal ID="ltFormTitle" runat="server" /></h3>

                    <div class="row g-3">
                        <asp:PlaceHolder ID="phForm" runat="server" />
                    </div>

                    <div class="d-flex gap-3 flex-wrap mt-4">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>

            <%-- grid --%>
            <div class="card">
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table align-middle mb-0">
                            <thead>
                                <tr>
                                    <th scope="col">#</th>
                                    <asp:Repeater ID="rptHeader" runat="server">
                                        <ItemTemplate>
                                            <th scope="col"><%# Container.DataItem %></th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <th scope="col" class="text-end"><asp:Literal ID="ltActions" runat="server" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Id") %></td>
                                            <asp:Repeater ID="rptCells" runat="server" DataSource='<%# Eval("Cells") %>'>
                                                <ItemTemplate>
                                                    <td><%# Container.DataItem %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <td class="text-end text-nowrap">
                                                <asp:LinkButton runat="server" CssClass="btn btn-sm btn-secondary"
                                                                CommandName="EditItem" CommandArgument='<%# Eval("Id") %>'
                                                                Text='<%# Eval("EditText") %>' CausesValidation="false" />
                                                <asp:LinkButton runat="server" CssClass="btn btn-sm btn-danger"
                                                                CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>'
                                                                Text='<%# Eval("DeleteText") %>' CausesValidation="false"
                                                                OnClientClick='<%# Eval("DeleteConfirm") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="text-center py-4">
                        <asp:Literal ID="ltEmpty" runat="server" />
                    </asp:Panel>
                </div>
            </div>

            <asp:HiddenField ID="hfListBuilt" runat="server" Value="" />
            <asp:HiddenField ID="hfMode" runat="server" Value="" />
            <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
        </asp:Panel>
    </div>
</section>
