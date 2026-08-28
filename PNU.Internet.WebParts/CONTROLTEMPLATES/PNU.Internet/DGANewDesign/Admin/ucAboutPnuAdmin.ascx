<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAboutPnuAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Admin.ucAboutPnuAdmin, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5 about-pnu-admin">
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
                <div class="alert alert-success alert-dismissible fade show" role="alert">
                    <asp:Literal ID="ltMessage" runat="server" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                    <asp:Literal ID="ltError" runat="server" />
                </div>
            </asp:Panel>

            <div class="card mb-4 shadow-sm border-0">
                <div class="card-body">
                    <div class="row g-3 align-items-end">
                        <div class="col-12 col-md-5">
                            <asp:Label ID="lblListLabel" runat="server" CssClass="form-label fw-bold" AssociatedControlID="ddlList" />
                            <asp:DropDownList ID="ddlList" runat="server" CssClass="form-select"
                                              AutoPostBack="true" OnSelectedIndexChanged="ddlList_SelectedIndexChanged" />
                        </div>
                        <div class="col-12 col-md-7 d-flex justify-content-md-end gap-2 flex-wrap">
                            <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary" OnClick="btnNew_Click">
                                <i class="hgi hgi-stroke hgi-add-01 me-1" aria-hidden="true"></i>
                                <asp:Literal ID="ltNew" runat="server" />
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnSeed" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnSeed_Click"
                                            OnClientClick="return confirm('هل أنت متأكد من إعادة تعيين البيانات الافتراضية؟ / Are you sure you want to re-seed default data?');">
                                <i class="hgi hgi-stroke hgi-refresh me-1" aria-hidden="true"></i>
                                <asp:Literal ID="ltSeed" runat="server" />
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <%-- ------------------------ Add / Edit Form ------------------------ --%>
            <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4 shadow-sm border-0">
                <div class="card-body">
                    <h3 class="h5 mb-4 border-bottom pb-2 text-primary">
                        <asp:Literal ID="ltFormTitle" runat="server" />
                    </h3>

                    <div class="row g-3">
                        <asp:PlaceHolder ID="phForm" runat="server" />
                    </div>

                    <div class="d-flex gap-2 flex-wrap mt-4 pt-3 border-top">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary px-4" OnClick="btnSave_Click">
                            <i class="hgi hgi-stroke hgi-tick-02 me-1" aria-hidden="true"></i>
                            <asp:Literal ID="ltSaveBtnText" runat="server" />
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary px-4" OnClick="btnCancel_Click" CausesValidation="false">
                            <asp:Literal ID="ltCancelBtnText" runat="server" />
                        </asp:LinkButton>
                    </div>
                </div>
            </asp:Panel>

            <%-- ------------------------------ Grid ----------------------------- --%>
            <div class="card shadow-sm border-0">
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light">
                                <tr>
                                    <th scope="col" style="width: 70px;">#</th>
                                    <asp:Repeater ID="rptHeader" runat="server">
                                        <ItemTemplate>
                                            <th scope="col"><%# Container.DataItem %></th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <th scope="col" class="text-end" style="min-width: 140px;"><asp:Literal ID="ltActions" runat="server" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-muted"><%# Eval("Id") %></td>
                                            <asp:Repeater ID="rptCells" runat="server" DataSource='<%# Eval("Cells") %>'>
                                                <ItemTemplate>
                                                    <td><%# Container.DataItem %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <td class="text-end text-nowrap">
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary me-1"
                                                                CommandName="EditItem" CommandArgument='<%# Eval("Id") %>'
                                                                ToolTip="تعديل / Edit">
                                                    <i class="hgi hgi-stroke hgi-pencil-edit-01" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-outline-danger"
                                                                CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>'
                                                                OnClientClick="return confirm('هل أنت متأكد من رغبتك بحذف هذا العنصر؟ / Are you sure you want to delete this item?');"
                                                                ToolTip="حذف / Delete">
                                                    <i class="hgi hgi-stroke hgi-delete-02" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="p-5 text-center text-muted">
                        <i class="hgi hgi-stroke hgi-inbox fs-1 d-block mb-2" aria-hidden="true"></i>
                        <asp:Literal ID="ltEmpty" runat="server" />
                    </asp:Panel>
                </div>
            </div>

            <asp:HiddenField ID="hfEditingId" runat="server" />
            <asp:HiddenField ID="hfListBuilt" runat="server" />

        </asp:Panel>

    </div>
</section>
