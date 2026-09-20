<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSiteQualityAuditor.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucSiteQualityAuditor" %>

<style>
    .pnu-audit-wrapper {
        font-family: var(--bs-body-font-family, 'IBM Plex Sans Arabic', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif);
        direction: rtl;
        text-align: right;
        color: #1a202c;
        padding: 24px;
        background: #f8fafc;
        border-radius: 16px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
        margin-bottom: 30px;
    }
    .pnu-audit-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: wrap;
        gap: 16px;
        margin-bottom: 24px;
        padding-bottom: 16px;
        border-bottom: 1px solid #e2e8f0;
    }
    .pnu-audit-title {
        font-size: 1.5rem;
        font-weight: 700;
        color: #0f172a;
        margin: 0;
        display: flex;
        align-items: center;
        gap: 10px;
    }
    .pnu-kpi-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
        gap: 16px;
        margin-bottom: 24px;
    }
    .pnu-kpi-card {
        background: #ffffff;
        padding: 20px;
        border-radius: 12px;
        border: 1px solid #e2e8f0;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.02);
        position: relative;
        overflow: hidden;
    }
    .pnu-kpi-card.score { border-right: 4px solid #10b981; }
    .pnu-kpi-card.critical { border-right: 4px solid #ef4444; }
    .pnu-kpi-card.warning { border-right: 4px solid #f59e0b; }
    .pnu-kpi-card.info { border-right: 4px solid #3b82f6; }
    .pnu-kpi-card.passed { border-right: 4px solid #059669; }
    .pnu-kpi-card.pages { border-right: 4px solid #6366f1; }
    .pnu-kpi-label {
        font-size: 0.85rem;
        color: #64748b;
        font-weight: 600;
        margin-bottom: 8px;
    }
    .pnu-kpi-val {
        font-size: 1.8rem;
        font-weight: 800;
        color: #0f172a;
    }
    .pnu-controls-bar {
        background: #ffffff;
        padding: 16px 20px;
        border-radius: 12px;
        border: 1px solid #e2e8f0;
        display: flex;
        flex-wrap: wrap;
        align-items: center;
        gap: 12px;
        margin-bottom: 24px;
    }
    .pnu-btn {
        padding: 10px 20px;
        border-radius: 8px;
        font-weight: 600;
        font-size: 0.9rem;
        cursor: pointer;
        border: none;
        display: inline-flex;
        align-items: center;
        gap: 8px;
        transition: all 0.2s ease;
    }
    .pnu-btn-primary {
        background: #10753a;
        color: #ffffff;
    }
    .pnu-btn-primary:hover {
        background: #0d5c2e;
    }
    .pnu-btn-secondary {
        background: #f1f5f9;
        color: #334155;
        border: 1px solid #cbd5e1;
    }
    .pnu-btn-secondary:hover {
        background: #e2e8f0;
    }
    .pnu-input, .pnu-select {
        padding: 8px 14px;
        border-radius: 8px;
        border: 1px solid #cbd5e1;
        font-size: 0.9rem;
        background: #ffffff;
        color: #1e293b;
        min-width: 140px;
    }
    .pnu-badge {
        padding: 4px 10px;
        border-radius: 20px;
        font-size: 0.75rem;
        font-weight: 700;
        display: inline-block;
    }
    .pnu-badge-critical { background: #fee2e2; color: #991b1b; }
    .pnu-badge-warning { background: #fef3c7; color: #92400e; }
    .pnu-badge-info { background: #dbeafe; color: #1e40af; }
    .pnu-badge-code { background: #f1f5f9; color: #475569; border: 1px solid #cbd5e1; }
    .pnu-table-container {
        background: #ffffff;
        border-radius: 12px;
        border: 1px solid #e2e8f0;
        overflow-x: auto;
    }
    .pnu-audit-table {
        width: 100%;
        border-collapse: collapse;
        font-size: 0.88rem;
    }
    .pnu-audit-table th {
        background: #f8fafc;
        color: #475569;
        padding: 12px 16px;
        font-weight: 600;
        border-bottom: 1px solid #e2e8f0;
        text-align: right;
    }
    .pnu-audit-table td {
        padding: 12px 16px;
        border-bottom: 1px solid #f1f5f9;
        vertical-align: middle;
    }
    .pnu-audit-table tr:hover {
        background: #f8fafc;
    }
    .pnu-pagination {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 16px 20px;
        background: #ffffff;
        border-top: 1px solid #e2e8f0;
        border-radius: 0 0 12px 12px;
    }
    .pnu-snippet {
        font-family: monospace;
        background: #f1f5f9;
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 0.8rem;
        color: #0f172a;
        max-width: 250px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        display: inline-block;
    }
</style>

<div class="pnu-audit-wrapper">
    <!-- Header -->
    <div class="pnu-audit-header">
        <div>
            <h1 class="pnu-audit-title">
                <span>🛡️ لوحة تدقيق جودة الموقع والـ SEO (فحص 74 معياراً)</span>
            </h1>
            <p style="margin: 4px 0 0 0; color: #64748b; font-size: 0.9rem;">
                نظام الفحص الذكي لمطابقة المعايير الفنية والـ SEO والأمان وإمكانية الوصول مع التسجيل المباشر في قاعدة البيانات.
            </p>
        </div>
        <div>
            <asp:Label ID="lblLastScan" runat="server" Style="color: #64748b; font-size: 0.85rem;" />
        </div>
    </div>

    <!-- Live Scanner Action Bar -->
    <div class="pnu-controls-bar">
        <label style="font-weight: 600; font-size: 0.9rem;">نطاق الفحص:</label>
        <asp:DropDownList ID="ddlScanScope" runat="server" CssClass="pnu-select">
            <asp:ListItem Text="جميع المواقع الفرعية (All Subsites)" Value="All" Selected="True" />
            <asp:ListItem Text="البوابة الرئيسية فقط (/ar/ & /en/)" Value="Root" />
        </asp:DropDownList>

        <asp:Button ID="btnStartScan" runat="server" Text="⚡ بدء الفحص وتحديث قاعدة البيانات" CssClass="pnu-btn pnu-btn-primary" OnClick="btnStartScan_Click" />
        <asp:Button ID="btnExportExcel" runat="server" Text="📊 تصدير النتائج إلى Excel" CssClass="pnu-btn pnu-btn-secondary" OnClick="btnExportExcel_Click" />
        <asp:Button ID="btnGenerateSitemap" runat="server" Text="🗺️ توليد وتحديث sitemap.xml" CssClass="pnu-btn pnu-btn-secondary" OnClick="btnGenerateSitemap_Click" />

        <asp:Label ID="lblScanStatus" runat="server" Font-Bold="true" ForeColor="#059669" Style="margin-right: auto;" />
    </div>

    <!-- KPI Summary Scorecards -->
    <div class="pnu-kpi-grid">
        <div class="pnu-kpi-card score">
            <div class="pnu-kpi-label">مؤشر الجودة العام (Health Score)</div>
            <div class="pnu-kpi-val" style="color: #10b981;">
                <asp:Literal ID="litHealthScore" runat="server" Text="100%" />
            </div>
        </div>
        <div class="pnu-kpi-card pages">
            <div class="pnu-kpi-label">الصفحات المفحوصة</div>
            <div class="pnu-kpi-val">
                <asp:Literal ID="litTotalPages" runat="server" Text="0" />
            </div>
        </div>
        <div class="pnu-kpi-card critical">
            <div class="pnu-kpi-label">ملاحظات حرجة (Critical)</div>
            <div class="pnu-kpi-val" style="color: #ef4444;">
                <asp:Literal ID="litCriticalCount" runat="server" Text="0" />
            </div>
        </div>
        <div class="pnu-kpi-card warning">
            <div class="pnu-kpi-label">تحذيرات (Warnings)</div>
            <div class="pnu-kpi-val" style="color: #f59e0b;">
                <asp:Literal ID="litWarningCount" runat="server" Text="0" />
            </div>
        </div>
        <div class="pnu-kpi-card info">
            <div class="pnu-kpi-label">تنبيهات (Info)</div>
            <div class="pnu-kpi-val" style="color: #3b82f6;">
                <asp:Literal ID="litInfoCount" runat="server" Text="0" />
            </div>
        </div>
        <div class="pnu-kpi-card passed">
            <div class="pnu-kpi-label">الفحوصات السليمة (Passed)</div>
            <div class="pnu-kpi-val" style="color: #059669;">
                <asp:Literal ID="litPassedCount" runat="server" Text="0" />
            </div>
        </div>
    </div>

    <!-- Issue Filtering Controls -->
    <div class="pnu-controls-bar" style="background: #f8fafc; border-color: #cbd5e1;">
        <label style="font-weight: 600; font-size: 0.85rem;">التصنيف:</label>
        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="pnu-select" AutoPostBack="true" OnSelectedIndexChanged="FilterChanged">
            <asp:ListItem Text="جميع التصنيفات" Value="All" Selected="True" />
            <asp:ListItem Text="SEO & Metadata" Value="SEO & Metadata" />
            <asp:ListItem Text="Security & Headers" Value="Security" />
            <asp:ListItem Text="Accessibility & Images" Value="Accessibility" />
            <asp:ListItem Text="URL & 404 Routing" Value="URL & Routing" />
            <asp:ListItem Text="Structure & Validation" Value="SEO & Structure" />
        </asp:DropDownList>

        <label style="font-weight: 600; font-size: 0.85rem;">مستوى الخطورة:</label>
        <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="pnu-select" AutoPostBack="true" OnSelectedIndexChanged="FilterChanged">
            <asp:ListItem Text="الكل" Value="All" Selected="True" />
            <asp:ListItem Text="حرجة (Critical)" Value="Critical" />
            <asp:ListItem Text="تحذير (Warning)" Value="Warning" />
            <asp:ListItem Text="معلومة (Info)" Value="Info" />
        </asp:DropDownList>

        <label style="font-weight: 600; font-size: 0.85rem;">بحث في الصفحة / الملاحظة:</label>
        <asp:TextBox ID="txtSearch" runat="server" CssClass="pnu-input" placeholder="اكتب للبحث..." Style="flex: 1;" />
        <asp:Button ID="btnSearch" runat="server" Text="🔍 بحث" CssClass="pnu-btn pnu-btn-secondary" OnClick="FilterChanged" />
    </div>

    <!-- Issues Data Grid -->
    <div class="pnu-table-container">
        <asp:Repeater ID="rptIssues" runat="server">
            <HeaderTemplate>
                <table class="pnu-audit-table">
                    <thead>
                        <tr>
                            <th style="width: 70px;">رمز #</th>
                            <th style="width: 90px;">الخطورة</th>
                            <th style="width: 140px;">التصنيف</th>
                            <th>نوع الملاحظة</th>
                            <th>الصفحة المعنية</th>
                            <th>تفاصيل المشكلة</th>
                            <th>الإجراء المقترح</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><span class="pnu-badge pnu-badge-code">#<%# Eval("IssueCode") %></span></td>
                    <td>
                        <span class='pnu-badge <%# GetSeverityBadgeClass(Eval("Severity").ToString()) %>'>
                            <%# Eval("Severity") %>
                        </span>
                    </td>
                    <td style="font-weight: 600;"><%# Eval("IssueCategory") %></td>
                    <td style="font-weight: 600; color: #0f172a;"><%# Eval("IssueName") %></td>
                    <td>
                        <a href='<%# Eval("PageUrl") %>' target="_blank" rel="noopener noreferrer" style="color: #10753a; text-decoration: none; font-weight: 600;">
                            <%# string.IsNullOrEmpty(Eval("PageTitle") as string) ? Eval("PageUrl") : Eval("PageTitle") %> ↗
                        </a>
                    </td>
                    <td style="font-size: 0.85rem; color: #475569;"><%# Eval("Details") %></td>
                    <td style="font-size: 0.85rem; color: #059669; font-weight: 600;"><%# Eval("RecommendedFix") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Panel ID="pnlNoIssues" runat="server" Visible="false" Style="padding: 40px; text-align: center; color: #64748b;">
            <p style="font-size: 1.1rem; font-weight: 600; margin: 0;">🎉 لا توجد ملاحظات مسجلة تطابق معايير البحث الحالية!</p>
        </asp:Panel>

        <!-- Pagination -->
        <div class="pnu-pagination">
            <div>
                <asp:Label ID="lblRecordCount" runat="server" Style="color: #64748b; font-size: 0.85rem;" />
            </div>
            <div style="display: flex; gap: 8px;">
                <asp:Button ID="btnPrevPage" runat="server" Text="السابق" CssClass="pnu-btn pnu-btn-secondary" OnClick="btnPrevPage_Click" />
                <asp:Label ID="lblCurrentPage" runat="server" Style="padding: 8px 12px; font-weight: 600; font-size: 0.9rem;" />
                <asp:Button ID="btnNextPage" runat="server" Text="التالي" CssClass="pnu-btn pnu-btn-secondary" OnClick="btnNextPage_Click" />
            </div>
        </div>
    </div>
</div>
