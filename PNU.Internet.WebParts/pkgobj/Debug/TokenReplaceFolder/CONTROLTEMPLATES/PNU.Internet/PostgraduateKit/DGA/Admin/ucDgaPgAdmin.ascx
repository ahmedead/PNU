<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaPgAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Admin.ucDgaPgAdmin" %>

<section class="py-4 dga-pg-admin font-sans" dir="rtl">
    <div class="container-fluid px-4">

        <%-- Header --%>
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center py-3">
                <div>
                    <h3 class="mb-0 fw-bold fs-4">إدارة محتوى الحقيبة الأكاديمية (DGA Postgraduate Kit)</h3>
                    <small class="text-white-50">لوحة تحكم إدارة القوائم، المحاور، الإجراءات الأكاديمية، الأسئلة الشائعة والمصادر</small>
                </div>
                <span class="badge bg-light text-primary fs-6 px-3 py-2">Deanship of Graduate Studies</span>
            </div>
        </div>

        <%-- Access Denied --%>
        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-warning" role="alert">
                <i class="hgi hgi-stroke hgi-alert-circle me-2"></i>
                <asp:Literal ID="ltDenied" runat="server" Text="ليس لديك صلاحية لإدارة محتوى الحقيبة الأكاديمية." />
            </div>
        </asp:Panel>

        <%-- Main Content --%>
        <asp:Panel ID="pnlMain" runat="server">

            <%-- Success / Error Alerts --%>
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show" role="alert">
                <i class="hgi hgi-stroke hgi-checkmark-circle-02 me-2"></i>
                <asp:Literal ID="litMessage" runat="server" />
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show" role="alert">
                <i class="hgi hgi-stroke hgi-alert-triangle me-2"></i>
                <asp:Literal ID="litError" runat="server" />
            </asp:Panel>

            <%-- Toolbar: List Selection & Top Actions --%>
            <div class="card shadow-sm border-0 mb-4">
                <div class="card-body p-3">
                    <div class="row g-3 align-items-center justify-content-between">
                        <div class="col-12 col-md-6 d-flex align-items-center gap-3">
                            <label class="form-label fw-bold mb-0 text-nowrap" for="<%= ddlLists.ClientID %>">اختر القائمة:</label>
                            <asp:DropDownList ID="ddlLists" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLists_SelectedIndexChanged" CssClass="form-select shadow-none">
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-md-6 d-flex justify-content-md-end gap-2 flex-wrap">
                            <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary d-inline-flex align-items-center gap-2" OnClick="btnNew_Click">
                                <i class="hgi hgi-stroke hgi-add-01 fs-5"></i>
                                <span>إضافة عنصر جديد</span>
                            </asp:LinkButton>
                            <asp:Button ID="btnReSeedSelected" runat="server" Text="إعادة تعبئة القائمة بالافتراضي" CssClass="btn btn-warning" OnClick="btnReSeedSelected_Click" OnClientClick="return confirm('تحذير: سيتم مسح التعديلات الحالية على هذه القائمة واستعادة البيانات الافتراضية. هل أنت متأكد؟');" />
                            <asp:Button ID="btnReSeedAll" runat="server" Text="إعادة تعبئة جميع القوائم" CssClass="btn btn-outline-danger" OnClick="btnReSeedAll_Click" OnClientClick="return confirm('تحذير شديد: سيتم مسح بيانات جميع قوائم الحقيبة الـ 14 واستعادة البيانات الافتراضية كاملة. هل أنت متأكد؟');" />
                        </div>
                    </div>
                </div>
            </div>

            <%-- Dynamic Add / Edit Form --%>
            <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card shadow-sm border-primary border-2 mb-4">
                <div class="card-header bg-light d-flex justify-content-between align-items-center py-3">
                    <h4 class="h5 mb-0 fw-bold text-primary">
                        <asp:Literal ID="ltFormTitle" runat="server" Text="إضافة عنصر جديد" />
                    </h4>
                    <asp:LinkButton ID="btnCloseFormTop" runat="server" CssClass="btn-close" OnClick="btnCancel_Click" CausesValidation="false" aria-label="Close"></asp:LinkButton>
                </div>
                <div class="card-body p-4">
                    <div class="row g-3">
                        <asp:PlaceHolder ID="phForm" runat="server" />
                    </div>

                    <div class="d-flex gap-2 flex-wrap mt-4 pt-3 border-top">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary px-4 d-inline-flex align-items-center gap-2" OnClick="btnSave_Click">
                            <i class="hgi hgi-stroke hgi-floppy-disk fs-5"></i>
                            <span>حفظ البيانات</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary px-4" OnClick="btnCancel_Click" CausesValidation="false">
                            <span>إلغاء</span>
                        </asp:LinkButton>
                    </div>
                </div>
            </asp:Panel>

            <%-- Items Table / Grid --%>
            <div class="card shadow-sm border-0">
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped table-hover align-middle mb-0">
                            <thead class="table-light">
                                <tr>
                                    <th scope="col" style="width: 70px;"># ID</th>
                                    <asp:Repeater ID="rptHeader" runat="server">
                                        <ItemTemplate>
                                            <th scope="col"><%# Container.DataItem %></th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <th scope="col" class="text-center" style="width: 180px;">الإجراءات</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="fw-bold text-center"><%# Eval("Id") %></td>
                                            <asp:Repeater ID="rptCells" runat="server" DataSource='<%# Eval("Cells") %>'>
                                                <ItemTemplate>
                                                    <td style="max-width: 320px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;"><%# Container.DataItem %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <td class="text-center text-nowrap">
                                                <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-primary me-1"
                                                                CommandName="EditItem" CommandArgument='<%# Eval("Id") %>'
                                                                CausesValidation="false">
                                                    <i class="hgi hgi-stroke hgi-edit-02 me-1"></i>تعديل
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-danger"
                                                                CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>'
                                                                CausesValidation="false"
                                                                OnClientClick='<%# Eval("DeleteConfirm") %>'>
                                                    <i class="hgi hgi-stroke hgi-delete-02 me-1"></i>حذف
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="text-center py-5 text-muted">
                        <i class="hgi hgi-stroke hgi-folder-open fs-1 d-block mb-2 text-secondary"></i>
                        <p class="mb-0 fs-6">لا توجد بيانات في هذه القائمة حالياً.</p>
                    </asp:Panel>
                </div>
            </div>

            <asp:HiddenField ID="hfListBuilt" runat="server" Value="" />
            <asp:HiddenField ID="hfMode" runat="server" Value="" />
            <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
        </asp:Panel>
    </div>
</section>
