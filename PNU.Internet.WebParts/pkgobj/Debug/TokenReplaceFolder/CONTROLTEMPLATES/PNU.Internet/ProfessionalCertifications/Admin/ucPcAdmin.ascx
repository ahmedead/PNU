<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPcAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls.ucPcAdmin" %>

<div class="pc-admin-container p-4 bg-white rounded shadow-sm">
    <div class="d-flex justify-content-between align-items-center mb-4 pb-2 border-bottom">
        <h2 class="h4 text-primary mb-0">إدارة دليل الشهادات الاحترافية</h2>
        <div class="d-flex gap-2">
            <asp:Button ID="btnSeed" runat="server" CssClass="btn btn-outline-success" Text="إدراج البيانات الافتراضية" OnClick="btnSeed_Click" OnClientClick="return confirm('هل أنت تأكد من إدراج البيانات الافتراضية للقسم المحدد؟');" />
            <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary" Text="+ إضافة عنصر جديد" OnClick="btnNew_Click" />
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-info alert-dismissible fade show" role="alert">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>
    
    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show" role="alert">
        <asp:Literal ID="litError" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <div class="mb-4">
        <label class="form-label fw-bold">اختر القائمة المراد إدارتها:</label>
        <asp:DropDownList ID="ddlList" runat="server" CssClass="form-select w-auto d-inline-block ms-2" AutoPostBack="true" OnSelectedIndexChanged="ddlList_SelectedIndexChanged">
        </asp:DropDownList>
    </div>

    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4 border-primary">
        <div class="card-header bg-primary text-white font-weight-bold">
            <asp:Literal ID="litFormTitle" runat="server" Text="إضافة / تعديل عنصر" />
        </div>
        <div class="card-body">
            <asp:HiddenField ID="hfEditItemId" runat="server" Value="0" />
            <div id="divFormFields" runat="server" class="row g-3"></div>
            <div class="mt-4 text-start">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" Text="حفظ البيانات" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary px-4 me-2" Text="إلغاء" OnClick="btnCancel_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="table-responsive">
        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover align-middle"
            OnRowCommand="gvData_RowCommand" OnRowDeleting="gvData_RowDeleting" EmptyDataText="لا توجد بيانات حالياً في هذه القائمة.">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="#" ItemStyle-Width="60px" />
                <asp:BoundField DataField="Title" HeaderText="العنوان / Title" />
                <asp:BoundField DataField="ItemOrder" HeaderText="الترتيب" ItemStyle-Width="80px" />
                <asp:TemplateField HeaderText="الإجراءات" ItemStyle-Width="160px">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditItem" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-outline-primary ms-1">تعديل</asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('هل أنت تأكد من حذف هذا العنصر؟');">حذف</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
