<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDataLeakReportsView.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms.ucDataLeakReportsView" %>

<div class="data-leak-admin-wrapper" dir="rtl">

    <%-- Authorization gate --%>
    <asp:Panel ID="pnlUnauthorized" runat="server" Visible="false" CssClass="unauthorized-panel">
        <div class="unauthorized-icon">🔒</div>
        <h3>غير مصرح بالوصول</h3>
        <p>عذراً، لا تملك صلاحية عرض بلاغات تسريب البيانات.</p>
        <p>يرجى التواصل مع إدارة البيانات للحصول على الصلاحيات اللازمة.</p>
    </asp:Panel>

    <%-- Authorized content --%>
    <asp:Panel ID="pnlAuthorized" runat="server" Visible="false">

        <div class="admin-header">
            <h2>بلاغات حوادث تسريب البيانات</h2>
            <div class="admin-stats">
                <span>إجمالي البلاغات: <strong>
                    <asp:Literal ID="litTotalCount" runat="server" /></strong></span>
            </div>
        </div>

        <div class="admin-toolbar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input"
                placeholder="بحث بالاسم أو البريد أو رقم البلاغ..." />
            <asp:Button ID="btnSearch" runat="server" Text="بحث" CssClass="btn-search"
                OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="مسح" CssClass="btn-clear"
                OnClick="btnClear_Click" />
        </div>

        <div class="reports-table-wrapper">
            <asp:Repeater ID="rptReports" runat="server" OnItemCommand="rptReports_ItemCommand">
                <HeaderTemplate>
                    <table class="reports-table">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>رقم البلاغ</th>
                                <th>تاريخ التقديم</th>
                                <th>الاسم</th>
                                <th>البريد الإلكتروني</th>
                                <th>الجوال</th>
                                <th>الصفة</th>
                                <th>تاريخ الاكتشاف</th>
                                <th>إجراءات</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# Eval("Title") %></td>
                        <td><%# Eval("Created", "{0:yyyy-MM-dd HH:mm}") %></td>
                        <td><%# Eval("FullName") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><%# Eval("Mobile") %></td>
                        <td><%# Eval("Capacity") %></td>
                        <td><%# Eval("DiscoveryDate") %></td>
                        <td>
                            <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewDetails"
                                CommandArgument='<%# Eval("ID") %>' CssClass="btn-view"
                                Text="عرض التفاصيل" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlNoData" runat="server" Visible="false" CssClass="no-data">
                <p>لا توجد بلاغات لعرضها</p>
            </asp:Panel>
        </div>

        <%-- Detail Modal --%>
        <asp:Panel ID="pnlDetailsModal" runat="server" Visible="false" CssClass="modal-overlay">
            <div class="modal-content">
                <div class="modal-header">
                    <h3>تفاصيل البلاغ</h3>
                    <asp:Button ID="btnCloseModal" runat="server" Text="×" CssClass="btn-close"
                        OnClick="btnCloseModal_Click" />
                </div>
                <div class="modal-body">
                    <div class="detail-section">
                        <h4>بيانات مُقدم البلاغ</h4>
                        <div class="detail-row"><span>رقم البلاغ:</span>
                            <strong><asp:Literal ID="litRefNo" runat="server" /></strong></div>
                        <div class="detail-row"><span>تاريخ التقديم:</span>
                            <strong><asp:Literal ID="litCreated" runat="server" /></strong></div>
                        <div class="detail-row"><span>الاسم الكامل:</span>
                            <strong><asp:Literal ID="litFullName" runat="server" /></strong></div>
                        <div class="detail-row"><span>البريد الإلكتروني:</span>
                            <strong><asp:Literal ID="litEmail" runat="server" /></strong></div>
                        <div class="detail-row"><span>رقم الجوال:</span>
                            <strong><asp:Literal ID="litMobile" runat="server" /></strong></div>
                        <div class="detail-row"><span>الصفة:</span>
                            <strong><asp:Literal ID="litCapacity" runat="server" /></strong></div>
                    </div>

                    <div class="detail-section">
                        <h4>تفاصيل الحادثة</h4>
                        <div class="detail-row"><span>تاريخ اكتشاف التسريب:</span>
                            <strong><asp:Literal ID="litDiscoveryDate" runat="server" /></strong></div>
                        <div class="detail-row"><span>هل التسريب ما زال قائماً:</span>
                            <strong><asp:Literal ID="litStillActive" runat="server" /></strong></div>
                        <div class="detail-row"><span>نوع مصدر التسريب:</span>
                            <strong><asp:Literal ID="litSourceType" runat="server" /></strong></div>
                        <div class="detail-row"><span>اسم المصدر / الرابط:</span>
                            <strong><asp:Literal ID="litSourceName" runat="server" /></strong></div>
                        <div class="detail-row"><span>نوع البيانات المتأثرة:</span>
                            <strong><asp:Literal ID="litDataType" runat="server" /></strong></div>
                        <div class="detail-row"><span>صيغة البيانات:</span>
                            <strong><asp:Literal ID="litDataFormat" runat="server" /></strong></div>
                        <div class="detail-row"><span>طريقة الوصول:</span>
                            <strong><asp:Literal ID="litAccessMethod" runat="server" /></strong></div>
                        <div class="detail-row"><span>عدد المتأثرين:</span>
                            <strong><asp:Literal ID="litAffectedCount" runat="server" /></strong></div>
                        <div class="detail-row"><span>البيانات تخص:</span>
                            <strong><asp:Literal ID="litDataBelongsTo" runat="server" /></strong></div>
                    </div>

                    <div class="detail-section">
                        <h4>وصف الحادث</h4>
                        <div class="detail-description">
                            <asp:Literal ID="litDescription" runat="server" />
                        </div>
                    </div>

                    <div class="detail-section">
                        <h4>المرفقات</h4>
                        <asp:Literal ID="litAttachments" runat="server" />
                    </div>
                </div>
            </div>
        </asp:Panel>

    </asp:Panel>

</div>

<style>
    .data-leak-admin-wrapper {
        font-family: 'Tajawal', Tahoma, Arial, sans-serif;
        max-width: 1300px;
        margin: 20px auto;
        padding: 20px;
    }

    .data-leak-admin-wrapper .unauthorized-panel {
        background: #fff;
        border: 1px solid #f5c6cb;
        border-radius: 8px;
        padding: 40px;
        text-align: center;
        color: #721c24;
    }

    .data-leak-admin-wrapper .unauthorized-icon {
        font-size: 64px;
        margin-bottom: 20px;
    }

    .data-leak-admin-wrapper .admin-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        background: #fff;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        margin-bottom: 20px;
    }

    .data-leak-admin-wrapper .admin-header h2 {
        margin: 0;
        color: #007a87;
    }

    .data-leak-admin-wrapper .admin-stats {
        font-size: 14px;
        color: #555;
    }

    .data-leak-admin-wrapper .admin-stats strong {
        color: #007a87;
        font-size: 18px;
        margin-right: 6px;
    }

    .data-leak-admin-wrapper .admin-toolbar {
        background: #fff;
        padding: 15px;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        margin-bottom: 20px;
        display: flex;
        gap: 10px;
        align-items: center;
    }

    .data-leak-admin-wrapper .search-input {
        flex: 1;
        padding: 8px 12px;
        border: 1px solid #ccc;
        border-radius: 4px;
        font-size: 14px;
    }

    .data-leak-admin-wrapper .btn-search,
    .data-leak-admin-wrapper .btn-clear {
        padding: 8px 20px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-size: 14px;
    }

    .data-leak-admin-wrapper .btn-search {
        background: #007a87;
        color: #fff;
    }

    .data-leak-admin-wrapper .btn-clear {
        background: #6c757d;
        color: #fff;
    }

    .data-leak-admin-wrapper .reports-table-wrapper {
        background: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        overflow: auto;
    }

    .data-leak-admin-wrapper .reports-table {
        width: 100%;
        border-collapse: collapse;
        font-size: 14px;
    }

    .data-leak-admin-wrapper .reports-table thead {
        background: #007a87;
        color: #fff;
    }

    .data-leak-admin-wrapper .reports-table th,
    .data-leak-admin-wrapper .reports-table td {
        padding: 12px;
        text-align: right;
        border-bottom: 1px solid #eee;
    }

    .data-leak-admin-wrapper .reports-table tbody tr:hover {
        background: #f8f9fa;
    }

    .data-leak-admin-wrapper .btn-view {
        background: #007a87;
        color: #fff !important;
        padding: 6px 14px;
        border-radius: 4px;
        text-decoration: none;
        font-size: 13px;
        display: inline-block;
    }

    .data-leak-admin-wrapper .btn-view:hover {
        background: #005d66;
    }

    .data-leak-admin-wrapper .no-data {
        text-align: center;
        padding: 40px;
        color: #888;
    }

    /* Modal */
    .data-leak-admin-wrapper .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0,0,0,0.5);
        z-index: 9999;
        display: flex;
        justify-content: center;
        align-items: flex-start;
        padding: 30px;
        overflow-y: auto;
    }

    .data-leak-admin-wrapper .modal-content {
        background: #fff;
        border-radius: 8px;
        max-width: 800px;
        width: 100%;
        max-height: 90vh;
        overflow-y: auto;
        box-shadow: 0 5px 20px rgba(0,0,0,0.3);
    }

    .data-leak-admin-wrapper .modal-header {
        background: #007a87;
        color: #fff;
        padding: 15px 20px;
        display: flex;
        justify-content: space-between;
        align-items: center;
        border-radius: 8px 8px 0 0;
    }

    .data-leak-admin-wrapper .modal-header h3 {
        margin: 0;
    }

    .data-leak-admin-wrapper .btn-close {
        background: transparent;
        border: none;
        color: #fff;
        font-size: 28px;
        cursor: pointer;
        padding: 0 8px;
        line-height: 1;
    }

    .data-leak-admin-wrapper .modal-body {
        padding: 20px;
    }

    .data-leak-admin-wrapper .detail-section {
        margin-bottom: 25px;
        padding-bottom: 15px;
        border-bottom: 1px solid #eee;
    }

    .data-leak-admin-wrapper .detail-section:last-child {
        border-bottom: none;
    }

    .data-leak-admin-wrapper .detail-section h4 {
        color: #007a87;
        margin: 0 0 12px 0;
        padding-bottom: 8px;
        border-bottom: 2px solid #007a87;
    }

    .data-leak-admin-wrapper .detail-row {
        display: flex;
        padding: 8px 0;
        font-size: 14px;
    }

    .data-leak-admin-wrapper .detail-row span {
        flex: 0 0 200px;
        color: #666;
    }

    .data-leak-admin-wrapper .detail-row strong {
        flex: 1;
        color: #222;
    }

    .data-leak-admin-wrapper .detail-description {
        background: #f8f9fa;
        padding: 15px;
        border-radius: 4px;
        line-height: 1.8;
        white-space: pre-wrap;
    }
</style>
