<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucExcelAuditVerifier.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SiteAudit.ucExcelAuditVerifier" %>

<style>
    .pnu-verifier-box {
        font-family: 'IBM Plex Sans Arabic', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        direction: rtl;
        text-align: right;
        background: #f8fafc;
        border-radius: 16px;
        padding: 24px;
        color: #0f172a;
        box-shadow: 0 4px 20px rgba(0,0,0,0.05);
        margin-bottom: 30px;
    }
    .verifier-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: wrap;
        gap: 16px;
        padding-bottom: 20px;
        border-bottom: 1px solid #e2e8f0;
        margin-bottom: 24px;
    }
    .verifier-title {
        font-size: 1.4rem;
        font-weight: 800;
        color: #0f172a;
        display: flex;
        align-items: center;
        gap: 12px;
        margin: 0;
    }
    .verifier-kpis {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
        gap: 14px;
        margin-bottom: 24px;
    }
    .verifier-kpi-card {
        background: #ffffff;
        border-radius: 12px;
        padding: 16px 20px;
        border: 1px solid #e2e8f0;
        box-shadow: 0 1px 3px rgba(0,0,0,0.04);
        position: relative;
        overflow: hidden;
    }
    .verifier-kpi-card.total { border-right: 4px solid #10753a; }
    .verifier-kpi-card.resolved { border-right: 4px solid #10b981; }
    .verifier-kpi-card.unresolved { border-right: 4px solid #ef4444; }
    .verifier-kpi-card.pending { border-right: 4px solid #64748b; }
    
    .kpi-title { font-size: 0.8rem; color: #64748b; font-weight: 600; margin-bottom: 4px; }
    .kpi-num { font-size: 1.8rem; font-weight: 800; color: #0f172a; }

    .verifier-toolbar {
        background: #ffffff;
        border-radius: 12px;
        padding: 16px 20px;
        border: 1px solid #e2e8f0;
        display: flex;
        flex-wrap: wrap;
        gap: 12px;
        align-items: center;
        margin-bottom: 20px;
    }
    .v-select, .v-input {
        padding: 10px 14px;
        border-radius: 8px;
        border: 1px solid #cbd5e1;
        font-family: inherit;
        font-size: 0.88rem;
        background: #ffffff;
    }
    .v-select:focus, .v-input:focus {
        outline: none;
        border-color: #10753a;
        box-shadow: 0 0 0 2px rgba(16, 117, 58, 0.15);
    }
    .btn-action {
        font-family: inherit;
        padding: 10px 18px;
        border-radius: 8px;
        font-size: 0.88rem;
        font-weight: 600;
        cursor: pointer;
        border: none;
        display: inline-flex;
        align-items: center;
        gap: 6px;
        transition: all 0.2s ease;
    }
    .btn-verify { background: #10753a; color: #ffffff; }
    .btn-verify:hover { background: #0b4f27; }
    .btn-verify-all { background: #2563eb; color: #ffffff; }
    .btn-verify-all:hover { background: #1d4ed8; }
    .btn-secondary { background: #e2e8f0; color: #334155; }
    .btn-secondary:hover { background: #cbd5e1; }

    .verifier-table {
        width: 100%;
        border-collapse: separate;
        border-spacing: 0;
        background: #ffffff;
        border-radius: 12px;
        border: 1px solid #e2e8f0;
        overflow: hidden;
    }
    .verifier-table th {
        background: #f1f5f9;
        color: #334155;
        font-size: 0.84rem;
        font-weight: 700;
        padding: 14px 16px;
        border-bottom: 1px solid #e2e8f0;
        text-align: right;
    }
    .verifier-table td {
        padding: 14px 16px;
        border-bottom: 1px solid #f1f5f9;
        font-size: 0.86rem;
        vertical-align: middle;
    }
    .verifier-table tr:hover td {
        background: #f8fafc;
    }
    .url-link {
        color: #0284c7;
        text-decoration: none;
        word-break: break-all;
        font-family: monospace;
        font-size: 0.83rem;
        direction: ltr;
        text-align: left;
        display: block;
    }
    .url-link:hover { text-decoration: underline; }

    .badge-status {
        padding: 4px 10px;
        border-radius: 20px;
        font-size: 0.76rem;
        font-weight: 700;
        display: inline-flex;
        align-items: center;
        gap: 4px;
    }
    .badge-resolved { background: #dcfce7; color: #166534; border: 1px solid #86efac; }
    .badge-unresolved { background: #fee2e2; color: #991b1b; border: 1px solid #fca5a5; }
    .badge-pending { background: #f1f5f9; color: #475569; border: 1px solid #cbd5e1; }
    .badge-code { font-family: monospace; font-weight: 700; padding: 2px 6px; border-radius: 4px; background: #e2e8f0; }

    .alert-banner {
        padding: 14px 18px;
        border-radius: 10px;
        margin-bottom: 20px;
        font-size: 0.9rem;
        display: flex;
        align-items: center;
        gap: 10px;
    }
    .alert-success { background: #dcfce7; color: #166534; border: 1px solid #86efac; }
    .alert-info { background: #e0f2fe; color: #0369a1; border: 1px solid #bae6fd; }

    .loading-overlay {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 10px;
        background: rgba(255,255,255,0.85);
        padding: 20px;
        border-radius: 12px;
        font-weight: 700;
        color: #10753a;
    }
</style>

<asp:UpdatePanel ID="upnlVerifier" runat="server">
    <ContentTemplate>
        <div class="pnu-verifier-box">
            
            <!-- Header -->
            <div class="verifier-header">
                <h2 class="verifier-title">
                    <span>🔬</span>
                    <span>لوحة فحص الروابط والتحقق من المعالجة (Site Audit Verifier)</span>
                </h2>
                <div>
                    <asp:Button ID="btnRefreshData" runat="server" CssClass="btn-action btn-secondary" Text="🔄 تحديث البيانات" OnClick="btnRefreshData_Click" />
                </div>
            </div>

            <!-- Alert notification -->
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert-banner alert-info">
                <asp:Literal ID="litAlertMsg" runat="server" />
            </asp:Panel>

            <!-- KPI Cards -->
            <div class="verifier-kpis">
                <div class="verifier-kpi-card total">
                    <div class="kpi-title">إجمالي الروابط في المعيار</div>
                    <div class="kpi-num"><asp:Literal ID="litKpiTotal" runat="server" Text="0" /></div>
                </div>
                <div class="verifier-kpi-card resolved">
                    <div class="kpi-title">روابط تم حل مشكلتها بنجاح</div>
                    <div class="kpi-num" style="color: #10b981;"><asp:Literal ID="litKpiResolved" runat="server" Text="0" /></div>
                </div>
                <div class="verifier-kpi-card unresolved">
                    <div class="kpi-title">روابط ما زالت بحاجة لمعالجة</div>
                    <div class="kpi-num" style="color: #ef4444;"><asp:Literal ID="litKpiUnresolved" runat="server" Text="0" /></div>
                </div>
                <div class="verifier-kpi-card pending">
                    <div class="kpi-title">روابط بانتظار الفحص</div>
                    <div class="kpi-num" style="color: #64748b;"><asp:Literal ID="litKpiPending" runat="server" Text="0" /></div>
                </div>
            </div>

            <!-- Toolbar & Filter Controls -->
            <div class="verifier-toolbar">
                <div style="flex: 2; min-width: 260px;">
                    <label style="font-size: 0.78rem; font-weight: 700; color: #64748b; display: block; margin-bottom: 4px;">اختر المعيار المطلوب فحصه:</label>
                    <asp:DropDownList ID="ddlIssueGroups" runat="server" CssClass="v-select" AutoPostBack="true" OnSelectedIndexChanged="ddlIssueGroups_SelectedIndexChanged" Width="100%" />
                </div>

                <div style="flex: 1; min-width: 160px;">
                    <label style="font-size: 0.78rem; font-weight: 700; color: #64748b; display: block; margin-bottom: 4px;">تصفية بحسب الحالة:</label>
                    <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="v-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" Width="100%">
                        <asp:ListItem Text="جميع الحالات" Value="All" />
                        <asp:ListItem Text="⏳ بانتظار الفحص (Pending)" Value="Pending" />
                        <asp:ListItem Text="❌ لم يتم الحل (Unresolved)" Value="Unresolved" />
                        <asp:ListItem Text="✅ تم الحل بنجاح (Resolved)" Value="Resolved" />
                    </asp:DropDownList>
                </div>

                <div style="flex: 2; min-width: 200px;">
                    <label style="font-size: 0.78rem; font-weight: 700; color: #64748b; display: block; margin-bottom: 4px;">بحث في الروابط:</label>
                    <div style="display: flex; gap: 6px;">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="v-input" placeholder="ابحث بجزء من الرابط..." Width="100%" />
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn-action btn-secondary" Text="بحث" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>

            <!-- Actions Bar: Select All / Verify Selected -->
            <div style="display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 10px; margin-bottom: 16px;">
                <div style="display: flex; gap: 8px; align-items: center;">
                    <button type="button" class="btn-action btn-secondary" onclick="toggleAllCheckboxes(true)">☑️ تحديد الكل</button>
                    <button type="button" class="btn-action btn-secondary" onclick="toggleAllCheckboxes(false)">◻️ إلغاء التحديد</button>
                    <span id="selectedCounter" style="font-size: 0.85rem; font-weight: 700; color: #10753a; margin-right: 8px;"></span>
                </div>

                <div style="display: flex; gap: 8px;">
                    <asp:Button ID="btnVerifySelected" runat="server" CssClass="btn-action btn-verify" Text="⚡ فحص الروابط المحددة الآن" OnClick="btnVerifySelected_Click" />
                    <asp:Button ID="btnVerifyPendingOnly" runat="server" CssClass="btn-action btn-verify-all" Text="🚀 فحص أول 25 رابط معلق" OnClick="btnVerifyPendingOnly_Click" />
                </div>
            </div>

            <!-- Loading Indicator -->
            <asp:UpdateProgress ID="uprogVerifier" runat="server" AssociatedUpdatePanelID="upnlVerifier">
                <ProgressTemplate>
                    <div class="loading-overlay">
                        <span style="font-size: 1.5rem;">⏳</span>
                        <span>جاري فحص الروابط والتحقق من صفحات الموقع وتحديث قاعدة البيانات... يرجى الانتظار</span>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!-- Table of URLs -->
            <asp:Panel ID="pnlTable" runat="server">
                <table class="verifier-table">
                    <thead>
                        <tr>
                            <th style="width: 40px; text-align: center;">
                                <input type="checkbox" id="chkHeaderSelectAll" onclick="toggleAllCheckboxes(this.checked)" title="تحديد / إلغاء تحديد الكل" />
                            </th>
                            <th style="width: 60px;">#</th>
                            <th>رابط الصفحة المستهدفة</th>
                            <th style="width: 100px; text-align: center;">كود الاستجابة</th>
                            <th style="width: 150px; text-align: center;">حالة المعالجة</th>
                            <th>تفاصيل الفحص والتحقق</th>
                            <th style="width: 140px; text-align: center;">تاريخ الفحص</th>
                            <th style="width: 100px; text-align: center;">إجراء</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptIssues" runat="server" OnItemCommand="rptIssues_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="chkRowSelect" runat="server" CssClass="row-select-chk" />
                                        <asp:HiddenField ID="hfRowId" runat="server" Value='<%# Eval("Id") %>' />
                                        <asp:HiddenField ID="hfRowUrl" runat="server" Value='<%# Eval("PageUrl") %>' />
                                        <asp:HiddenField ID="hfRowIssueCode" runat="server" Value='<%# Eval("IssueCode") %>' />
                                    </td>
                                    <td style="font-weight: 700; color: #64748b;">
                                        <%# Container.ItemIndex + 1 + ((CurrentPageIndex - 1) * PageSize) %>
                                    </td>
                                    <td>
                                        <a href='<%# Eval("PageUrl") %>' target="_blank" rel="noopener noreferrer" class="url-link" title="فتح الرابط في نافذة جديدة">
                                            <%# Eval("PageUrl") %>
                                        </a>
                                    </td>
                                    <td style="text-align: center;">
                                        <span class="badge-code">
                                            <%# Eval("ResponseCode") ?? "-" %>
                                        </span>
                                    </td>
                                    <td style="text-align: center;">
                                        <%# GetStatusBadge(Convert.ToString(Eval("VerificationStatus"))) %>
                                    </td>
                                    <td style="font-size: 0.82rem; color: #334155;">
                                        <%# Eval("CheckResultDetails") ?? "<span style='color:#94a3b8;'>لم يتم فحص الرابط بعد</span>" %>
                                    </td>
                                    <td style="text-align: center; font-size: 0.8rem; color: #64748b;">
                                        <%# Eval("LastCheckedDate") != null ? Convert.ToDateTime(Eval("LastCheckedDate")).ToString("yyyy-MM-dd HH:mm") : "-" %>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:LinkButton ID="btnCheckSingle" runat="server" CommandName="VerifySingle" CommandArgument='<%# Eval("Id") + "|" + Eval("IssueCode") + "|" + Eval("PageUrl") %>' CssClass="btn-action btn-secondary" Style="padding: 4px 10px; font-size: 0.78rem;">
                                            🔍 فحص الآن
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr id="trEmpty" runat="server" visible='<%# rptIssues.Items.Count == 0 %>'>
                                    <td colspan="8" style="text-align: center; padding: 40px; color: #64748b;">
                                        لا توجد روابط مسجلة مطابقة للبحث في هذا المعيار.
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

                <!-- Pagination -->
                <div style="display: flex; justify-content: space-between; align-items: center; margin-top: 16px; padding: 0 6px;">
                    <div style="font-size: 0.85rem; color: #64748b; font-weight: 600;">
                        <asp:Literal ID="litPaginationInfo" runat="server" />
                    </div>
                    <div style="display: flex; gap: 6px;">
                        <asp:Button ID="btnPrevPage" runat="server" CssClass="btn-action btn-secondary" Text="◀ الصفحة السابقة" OnClick="btnPrevPage_Click" />
                        <asp:Button ID="btnNextPage" runat="server" CssClass="btn-action btn-secondary" Text="الصفحة التالية ▶" OnClick="btnNextPage_Click" />
                    </div>
                </div>
            </asp:Panel>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function toggleAllCheckboxes(checked) {
        var checkboxes = document.querySelectorAll('.row-select-chk input[type="checkbox"]');
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = checked;
        }
        var masterChk = document.getElementById('chkHeaderSelectAll');
        if (masterChk) masterChk.checked = checked;
        updateSelectedCounter();
    }

    function updateSelectedCounter() {
        var checkboxes = document.querySelectorAll('.row-select-chk input[type="checkbox"]:checked');
        var lbl = document.getElementById('selectedCounter');
        if (lbl) {
            lbl.innerText = checkboxes.length > 0 ? ('(تم تحديد ' + checkboxes.length + ' روابط)') : '';
        }
    }

    document.addEventListener('change', function(e) {
        if (e.target && e.target.type === 'checkbox') {
            updateSelectedCounter();
        }
    });
</script>
