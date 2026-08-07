<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOrgStructureAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About.OrgStructure.ucOrgStructureAdmin" %>
<%--
    ucOrgStructureAdmin — in-page content editor for the OrgStructureUnits list.
    Access is gated by the PortalAdmins list (same pattern as ucContentAdmin).
    Follows portal conventions: no <%= %>/<%# %> outside Eval(), no server
    controls referenced as fields inside the Repeater template, code-behind binds only.
--%>
<section class="py-4">
    <div class="container">

        <%-- Shown only when the current user is NOT an authorized content editor. --%>
        <asp:Panel ID="pnlDenied" runat="server" Visible="false" CssClass="alert alert-warning" role="alert">
            <asp:Literal ID="litDenied" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlAdmin" runat="server" Visible="false">

            <div class="d-flex align-items-center justify-content-between mb-3">
                <h2 class="h4 mb-0"><asp:Literal ID="litHeader" runat="server" /></h2>
                <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary" OnClick="btnNew_Click">
                    <i class="hgi hgi-stroke hgi-add-01 me-1" aria-hidden="true"></i>
                    <asp:Literal ID="litNew" runat="server" />
                </asp:LinkButton>
            </div>

            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-success" role="status">
                <asp:Literal ID="litMessage" runat="server" />
            </asp:Panel>

            <%-- ================= Editor form ================= --%>
            <asp:Panel ID="pnlEditor" runat="server" Visible="false" CssClass="card mb-4">
                <div class="card-body">
                    <asp:HiddenField ID="hidId" runat="server" />
                    <div class="row g-3">
                        <div class="col-md-2">
                            <label class="form-label"><asp:Literal ID="litLblOrder" runat="server" /></label>
                            <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" TextMode="Number" />
                        </div>
                        <div class="col-md-4">
                            <label class="form-label"><asp:Literal ID="litLblTheme" runat="server" /></label>
                            <asp:DropDownList ID="ddlTheme" runat="server" CssClass="form-select">
                                <asp:ListItem Value="primary">primary</asp:ListItem>
                                <asp:ListItem Value="sa">sa</asp:ListItem>
                                <asp:ListItem Value="saSoft">saSoft</asp:ListItem>
                                <asp:ListItem Value="saMuted">saMuted</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label"><asp:Literal ID="litLblIcon" runat="server" /></label>
                            <asp:TextBox ID="txtIcon" runat="server" CssClass="form-control" placeholder="hgi-hierarchy" />
                        </div>

                        <div class="col-12">
                            <label class="form-label"><asp:Literal ID="litLblSelector" runat="server" /></label>
                            <asp:TextBox ID="txtSelector" runat="server" CssClass="form-control" dir="ltr"
                                placeholder='rect[x="482.5"][y="79.5"][width="280"][height="56"]' />
                            <div class="form-text"><asp:Literal ID="litHintSelector" runat="server" /></div>
                        </div>

                        <div class="col-md-6">
                            <label class="form-label"><asp:Literal ID="litLblTitle" runat="server" /></label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label"><asp:Literal ID="litLblTitleEn" runat="server" /></label>
                            <asp:TextBox ID="txtTitleEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label"><asp:Literal ID="litLblDesc" runat="server" /></label>
                            <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label"><asp:Literal ID="litLblDescEn" runat="server" /></label>
                            <asp:TextBox ID="txtDescEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" dir="ltr" />
                        </div>

                        <div class="col-md-3">
                            <label class="form-label"><asp:Literal ID="litLblBadge" runat="server" /></label>
                            <asp:TextBox ID="txtBadge" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label"><asp:Literal ID="litLblBadgeEn" runat="server" /></label>
                            <asp:TextBox ID="txtBadgeEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label"><asp:Literal ID="litLblMeta" runat="server" /></label>
                            <asp:TextBox ID="txtMeta" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label"><asp:Literal ID="litLblMetaEn" runat="server" /></label>
                            <asp:TextBox ID="txtMetaEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-md-9">
                            <label class="form-label"><asp:Literal ID="litLblLink" runat="server" /></label>
                            <asp:TextBox ID="txtLinkUrl" runat="server" CssClass="form-control" dir="ltr" />
                        </div>
                        <div class="col-md-3 d-flex align-items-end">
                            <div class="form-check">
                                <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label"><asp:Literal ID="litLblActive" runat="server" /></label>
                            </div>
                        </div>
                    </div>

                    <div class="mt-3 d-flex gap-2">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click">
                            <asp:Literal ID="litSave" runat="server" />
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary" CausesValidation="false" OnClick="btnCancel_Click">
                            <asp:Literal ID="litCancel" runat="server" />
                        </asp:LinkButton>
                    </div>
                </div>
            </asp:Panel>

            <%-- ================= Units grid ================= --%>
            <div class="table-responsive">
                <table class="table table-hover align-middle">
                    <thead>
                        <tr>
                            <th scope="col"><asp:Literal ID="litColOrder" runat="server" /></th>
                            <th scope="col"><asp:Literal ID="litColTitle" runat="server" /></th>
                            <th scope="col"><asp:Literal ID="litColBadge" runat="server" /></th>
                            <th scope="col"><asp:Literal ID="litColActive" runat="server" /></th>
                            <th scope="col" class="text-end"><asp:Literal ID="litColActions" runat="server" /></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptUnits" runat="server" OnItemCommand="rptUnits_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Order") %></td>
                                    <td><%# Eval("Title") %></td>
                                    <td><span class="badge bg-primary-25 text-primary"><%# Eval("Badge") %></span></td>
                                    <td><%# (bool)Eval("Active") ? "&#10003;" : "&#8212;" %></td>
                                    <td class="text-end">
                                        <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-primary me-1"
                                            CommandName="EditUnit" CommandArgument='<%# Eval("Id") %>'>
                                            <i class="hgi hgi-stroke hgi-pencil-edit-02" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-danger"
                                            CommandName="DeleteUnit" CommandArgument='<%# Eval("Id") %>'
                                            OnClientClick="return confirm('تأكيد الحذف؟');">
                                            <i class="hgi hgi-stroke hgi-delete-02" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

        </asp:Panel>
    </div>
</section>
