<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucWebsitePagesDashboard.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucWebsitePagesDashboard" %>

<style>
    .pnu-dashboard-container {
        font-family: 'Segoe UI', Tahoma, Arial, sans-serif;
        color: #2c3e50;
        margin-bottom: 40px;
    }
    .pnu-card {
        background: #ffffff;
        border-radius: 12px;
        border: 1px solid #e2e8f0;
        box-shadow: 0 4px 15px rgba(0,0,0,0.04);
        padding: 20px;
        margin-bottom: 24px;
        transition: transform 0.2s, box-shadow 0.2s;
    }
    .pnu-card:hover {
        box-shadow: 0 6px 20px rgba(0,0,0,0.07);
    }
    .pnu-header-card {
        background: linear-gradient(135deg, #0d5c46 0%, #157358 100%);
        color: #ffffff;
        border-radius: 14px;
        padding: 25px 30px;
        margin-bottom: 25px;
        box-shadow: 0 8px 25px rgba(13, 92, 70, 0.2);
    }
    .kpi-card {
        border-radius: 12px;
        padding: 20px;
        color: #ffffff;
        position: relative;
        overflow: hidden;
        margin-bottom: 20px;
        box-shadow: 0 4px 15px rgba(0,0,0,0.08);
    }
    .kpi-primary { background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%); }
    .kpi-success { background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%); }
    .kpi-warning { background: linear-gradient(135deg, #f2994a 0%, #f2c94c 100%); color: #333 !important; }
    .kpi-danger  { background: linear-gradient(135deg, #eb3349 0%, #f45c43 100%); }
    .kpi-info    { background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%); }
    .kpi-purple  { background: linear-gradient(135deg, #654ea3 0%, #eaafc8 100%); }
    
    .kpi-title { font-size: 13px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.5px; opacity: 0.9; margin-bottom: 8px; }
    .kpi-value { font-size: 32px; font-weight: 700; line-height: 1.1; margin-bottom: 6px; }
    .kpi-desc  { font-size: 12px; opacity: 0.85; }

    .nav-tabs-custom {
        display: flex;
        flex-wrap: wrap;
        border-bottom: 2px solid #e2e8f0;
        margin-bottom: 25px;
        gap: 6px;
        padding-bottom: 2px;
    }
    .nav-tab-btn {
        background: #f8fafc;
        border: 1px solid #e2e8f0;
        border-bottom: none;
        border-radius: 8px 8px 0 0;
        padding: 12px 20px;
        font-weight: 600;
        font-size: 14px;
        color: #475569;
        cursor: pointer;
        text-decoration: none;
        display: inline-flex;
        align-items: center;
        gap: 8px;
        transition: all 0.2s;
    }
    .nav-tab-btn:hover {
        background: #edf2f7;
        color: #0d5c46;
    }
    .nav-tab-btn.active {
        background: #0d5c46;
        color: #ffffff;
        border-color: #0d5c46;
    }
    
    .filter-bar {
        background: #f8fafc;
        border: 1px solid #e2e8f0;
        border-radius: 10px;
        padding: 18px;
        margin-bottom: 20px;
    }
    .action-toolbar {
        background: #ecfdf5;
        border: 1px solid #a7f3d0;
        border-radius: 10px;
        padding: 16px 20px;
        margin-bottom: 20px;
    }
    .action-toolbar-danger {
        background: #fff1f2;
        border: 1px solid #fecdd3;
        border-radius: 10px;
        padding: 16px 20px;
        margin-bottom: 20px;
    }
    .filter-control {
        border-radius: 6px;
        border: 1px solid #cbd5e1;
        padding: 7px 12px;
        font-size: 13px;
        width: 100%;
        background-color: #fff;
    }
    .filter-label {
        font-weight: 600;
        font-size: 12px;
        color: #475569;
        margin-bottom: 5px;
        display: block;
    }

    .btn-action {
        border-radius: 6px;
        padding: 8px 18px;
        font-weight: 600;
        font-size: 13px;
        cursor: pointer;
        border: none;
        transition: all 0.2s;
        display: inline-flex;
        align-items: center;
        gap: 6px;
    }
    .btn-pnu-primary { background-color: #0d5c46; color: #fff; }
    .btn-pnu-primary:hover { background-color: #083e2f; color: #fff; }
    .btn-pnu-secondary { background-color: #64748b; color: #fff; }
    .btn-pnu-secondary:hover { background-color: #475569; color: #fff; }
    .btn-pnu-success { background-color: #10b981; color: #fff; }
    .btn-pnu-success:hover { background-color: #059669; color: #fff; }
    .btn-pnu-danger { background-color: #ef4444; color: #fff; }
    .btn-pnu-danger:hover { background-color: #dc2626; color: #fff; }
    .btn-pnu-outline { background-color: transparent; border: 1px solid #cbd5e1; color: #334155; }
    .btn-pnu-outline:hover { background-color: #f1f5f9; }

    .pnu-table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 15px;
        background: #fff;
        border-radius: 8px;
        overflow: hidden;
    }
    .pnu-table th {
        background-color: #f1f5f9;
        color: #334155;
        font-weight: 600;
        font-size: 13px;
        padding: 12px 14px;
        border-bottom: 2px solid #cbd5e1;
        text-align: right;
    }
    .pnu-table td {
        padding: 11px 14px;
        font-size: 13px;
        border-bottom: 1px solid #e2e8f0;
        vertical-align: middle;
        text-align: right;
    }
    .pnu-table tr:hover {
        background-color: #f8fafc;
    }

    .badge-dga {
        background-color: #dcfce7;
        color: #166534;
        font-weight: 700;
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 11px;
        display: inline-block;
    }
    .badge-legacy {
        background-color: #fef3c7;
        color: #92400e;
        font-weight: 600;
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 11px;
        display: inline-block;
    }
    .badge-yes {
        background-color: #d1fae5;
        color: #065f46;
        font-weight: 600;
        padding: 3px 8px;
        border-radius: 4px;
        font-size: 11px;
    }
    .badge-no {
        background-color: #fee2e2;
        color: #991b1b;
        font-weight: 600;
        padding: 3px 8px;
        border-radius: 4px;
        font-size: 11px;
    }

    .progress-track {
        background-color: #e2e8f0;
        border-radius: 10px;
        height: 10px;
        width: 100%;
        overflow: hidden;
        margin-top: 6px;
    }
    .progress-fill {
        height: 100%;
        border-radius: 10px;
        transition: width 0.5s ease-in-out;
    }
    .progress-fill-green { background: linear-gradient(90deg, #10b981, #059669); }
    .progress-fill-blue { background: linear-gradient(90deg, #3b82f6, #1d4ed8); }
    .progress-fill-orange { background: linear-gradient(90deg, #f59e0b, #d97706); }

    .console-log {
        background-color: #0f172a;
        color: #38bdf8;
        font-family: Consolas, Monaco, monospace;
        font-size: 12px;
        padding: 16px;
        border-radius: 8px;
        max-height: 280px;
        overflow-y: auto;
        white-space: pre-wrap;
        direction: ltr;
        text-align: left;
    }

    .grid-row-wrap {
        display: flex;
        flex-wrap: wrap;
        margin-right: -10px;
        margin-left: -10px;
    }
    .grid-col-4 { flex: 0 0 33.333333%; max-width: 33.333333%; padding: 0 10px; }
    .grid-col-6 { flex: 0 0 50%; max-width: 50%; padding: 0 10px; }
    .grid-col-12 { flex: 0 0 100%; max-width: 100%; padding: 0 10px; }
    .grid-col-3 { flex: 0 0 25%; max-width: 25%; padding: 0 10px; }

    @media (max-width: 992px) {
        .grid-col-4, .grid-col-3 { flex: 0 0 50%; max-width: 50%; }
    }
    @media (max-width: 768px) {
        .grid-col-4, .grid-col-6, .grid-col-3 { flex: 0 0 100%; max-width: 100%; }
    }
</style>

<script type="text/javascript">
    function toggleSelectAllPages(headerCheckbox) {
        var grid = document.getElementById('<%= gvPagesByLayout.ClientID %>');
        if (!grid) return;
        var inputs = grid.getElementsByTagName('input');
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type === 'checkbox' && inputs[i] !== headerCheckbox) {
                inputs[i].checked = headerCheckbox.checked;
            }
        }
    }
    function toggleSelectAllMissing(headerCheckbox) {
        var grid = document.getElementById('<%= gvMissingPages.ClientID %>');
        if (!grid) return;
        var inputs = grid.getElementsByTagName('input');
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type === 'checkbox' && inputs[i] !== headerCheckbox) {
                inputs[i].checked = headerCheckbox.checked;
            }
        }
    }
</script>

<div class="pnu-dashboard-container" dir="rtl">

    <%-- Header Card --%>
    <div class="pnu-header-card">
        <div style="display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 15px;">
            <div>
                <h1 style="font-size: 24px; font-weight: 700; margin: 0 0 6px 0;">
                    لوحة مؤشرات وتقارير صفحات البوابة وتخطيطاتها (Portal Pages & Layouts Dashboard)
                </h1>
                <p style="margin: 0; font-size: 14px; opacity: 0.9;">
                    تقرير شامل واستقصاء تفاعلي لصفحات البوابة الإلكترونية، توزيع التخطيطات (Page Layouts)، جرد الصفحات المفقودة ثنائياً، وتغيير التخطيط وإنشاء الصفحات فورياً.
                </p>
            </div>
            <div style="display: flex; gap: 10px; align-items: center;">
                <asp:Button ID="btnRefreshAll" runat="server" Text="تحديث البيانات ↺" OnClick="btnRefreshAll_Click" CssClass="btn-action btn-pnu-outline" Style="background: #fff; color: #0d5c46; font-weight: 700;" />
                <asp:Button ID="btnExportCurrentTab" runat="server" Text="تصدير التقرير الحالي إلى Excel ⤓" OnClick="btnExportCurrentTab_Click" CssClass="btn-action btn-pnu-success" />
            </div>
        </div>
        <div style="margin-top: 15px; font-size: 12px; opacity: 0.85; border-top: 1px solid rgba(255,255,255,0.2); padding-top: 10px;">
            <span>آخر عملية فحص وتحديث للفهرس: </span>
            <asp:Literal ID="litLastScanDate" runat="server" Text="-" />
            <span style="margin: 0 15px;">|</span>
            <span>بيئة التشغيل: SharePoint 2019 / Subscription</span>
            <span style="margin: 0 15px;">|</span>
            <span>قاعدة البيانات: PNU_SearchIndex (dbo.WebsitePages & dbo.MissingPagesOnEN)</span>
        </div>
    </div>

    <%-- Navigation Tabs --%>
    <div class="nav-tabs-custom">
        <asp:LinkButton ID="btnTabOverview" runat="server" CssClass="nav-tab-btn active" OnClick="SwitchTab_Click" CommandArgument="Overview">
            <span>📊 المؤشرات العامة والملخص التنفيذي</span>
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabLayouts" runat="server" CssClass="nav-tab-btn" OnClick="SwitchTab_Click" CommandArgument="Layouts">
            <span>📐 تقرير وإدارة تخطيطات الصفحات (Page Layouts)</span>
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabMissingPages" runat="server" CssClass="nav-tab-btn" OnClick="SwitchTab_Click" CommandArgument="MissingPages">
            <span>⚠️ تقرير وإنشاء الصفحات المفقودة (Missing Pages)</span>
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabUserControls" runat="server" CssClass="nav-tab-btn" OnClick="SwitchTab_Click" CommandArgument="UserControls">
            <span>🧩 تقرير عناصر التحكم (User Controls & Web Parts)</span>
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabSubsites" runat="server" CssClass="nav-tab-btn" OnClick="SwitchTab_Click" CommandArgument="Subsites">
            <span>🏛️ تقرير صحة المواقع الفرعية (Subsites Health)</span>
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabLiveScan" runat="server" CssClass="nav-tab-btn" OnClick="SwitchTab_Click" CommandArgument="LiveScan">
            <span>🔍 الفحص والمسح المباشر (Live Scanner)</span>
        </asp:LinkButton>
    </div>

    <%-- Notifications / Messages --%>
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="pnu-card" Style="border-right: 5px solid #0d5c46; padding: 15px 20px;">
        <asp:Literal ID="litAlertMessage" runat="server" />
    </asp:Panel>

    <%-- ========================================================================= --%>
    <%-- TAB 1: EXECUTIVE DASHBOARD & OVERVIEW                                      --%>
    <%-- ========================================================================= --%>
    <asp:Panel ID="pnlTabOverview" runat="server" Visible="true">
        
        <%-- KPI Metric Cards --%>
        <div class="grid-row-wrap">
            <div class="grid-col-4">
                <div class="kpi-card kpi-primary">
                    <div class="kpi-title">إجمالي صفحات الموقع (Total Pages)</div>
                    <div class="kpi-value"><asp:Literal ID="litKpiTotalPages" runat="server" Text="0" /></div>
                    <div class="kpi-desc">
                        <span>عربي: </span><strong><asp:Literal ID="litKpiArPages" runat="server" Text="0" /></strong>
                        <span style="margin: 0 8px;">|</span>
                        <span>إنجليزي: </span><strong><asp:Literal ID="litKpiEnPages" runat="server" Text="0" /></strong>
                    </div>
                </div>
            </div>
            <div class="grid-col-4">
                <div class="kpi-card kpi-success">
                    <div class="kpi-title">نسبة التوافق اللغوي (Bilingual Parity)</div>
                    <div class="kpi-value"><asp:Literal ID="litKpiParityPct" runat="server" Text="0%" /></div>
                    <div class="kpi-desc">
                        <span>صفحات EN مطابقة: </span><strong><asp:Literal ID="litKpiEnExists" runat="server" Text="0" /></strong>
                    </div>
                    <div class="progress-track" style="background: rgba(255,255,255,0.3);">
                        <div id="divParityProgress" runat="server" class="progress-fill" style="background: #fff; width: 0%;"></div>
                    </div>
                </div>
            </div>
            <div class="grid-col-4">
                <div class="kpi-card kpi-danger">
                    <div class="kpi-title">الصفحات المفقودة (Missing Pages Gap)</div>
                    <div class="kpi-value"><asp:Literal ID="litKpiMissingTotal" runat="server" Text="0" /></div>
                    <div class="kpi-desc">
                        <span>عربي بدون إنجليزي: </span><strong><asp:Literal ID="litKpiMissingEn" runat="server" Text="0" /></strong>
                        <span style="margin: 0 8px;">|</span>
                        <span>محلولة: </span><strong><asp:Literal ID="litKpiResolved" runat="server" Text="0" /></strong>
                    </div>
                </div>
            </div>
            <div class="grid-col-4">
                <div class="kpi-card kpi-info">
                    <div class="kpi-title">تخطيطات الصفحات المستخدمة (Layouts)</div>
                    <div class="kpi-value"><asp:Literal ID="litKpiLayoutsCount" runat="server" Text="0" /></div>
                    <div class="kpi-desc">تخطيطات مخصصة ونظامية عبر البوابة</div>
                </div>
            </div>
            <div class="grid-col-4">
                <div class="kpi-card kpi-purple">
                    <div class="kpi-title">امتثال معايير DGA (DGA Layout Compliance)</div>
                    <div class="kpi-value"><asp:Literal ID="litKpiDgaPct" runat="server" Text="0%" /></div>
                    <div class="kpi-desc">
                        <span>صفحات DGA المعتمدة: </span><strong><asp:Literal ID="litKpiDgaCount" runat="server" Text="0" /></strong>
                    </div>
                    <div class="progress-track" style="background: rgba(255,255,255,0.3);">
                        <div id="divDgaProgress" runat="server" class="progress-fill" style="background: #fff; width: 0%;"></div>
                    </div>
                </div>
            </div>
            <div class="grid-col-4">
                <div class="kpi-card kpi-warning">
                    <div class="kpi-title">عناصر التحكم المسجلة (User Controls)</div>
                    <div class="kpi-value"><asp:Literal ID="litKpiUcCount" runat="server" Text="0" /></div>
                    <div class="kpi-desc">
                        <span>المواقع المفحوصة: </span><strong><asp:Literal ID="litKpiSubsites" runat="server" Text="0" /></strong>
                    </div>
                </div>
            </div>
        </div>

        <%-- Overview Visual Breakdown Cards --%>
        <div class="grid-row-wrap">
            <div class="grid-col-6">
                <div class="pnu-card">
                    <h3 style="font-size: 16px; font-weight: 700; margin-bottom: 15px; color: #0d5c46;">
                        📐 ملخص توزيع تخطيطات الصفحات (Top Page Layouts)
                    </h3>
                    <asp:Repeater ID="rptTopLayouts" runat="server">
                        <ItemTemplate>
                            <div style="margin-bottom: 12px;">
                                <div style="display: flex; justify-content: space-between; font-size: 13px; font-weight: 600; margin-bottom: 4px;">
                                    <span>
                                        <%# Eval("LayoutFileName") %>
                                        <%# (bool)Eval("IsDgaCompliant") ? "<span class='badge-dga'>DGA</span>" : "<span class='badge-legacy'>Legacy</span>" %>
                                    </span>
                                    <span><%# Eval("PageCount") %> صفحة (<%# Eval("Percentage") %>%)</span>
                                </div>
                                <div class="progress-track">
                                    <div class='<%# (bool)Eval("IsDgaCompliant") ? "progress-fill progress-fill-green" : "progress-fill progress-fill-orange" %>' style='width: <%# Eval("Percentage") %>%;'></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="grid-col-6">
                <div class="pnu-card">
                    <h3 style="font-size: 16px; font-weight: 700; margin-bottom: 15px; color: #0d5c46;">
                        🏛️ مؤشرات أعلى المواقع الفرعية بعدد الصفحات (Top Subsites)
                    </h3>
                    <asp:Repeater ID="rptTopSubsites" runat="server">
                        <ItemTemplate>
                            <div style="margin-bottom: 12px; border-bottom: 1px dashed #e2e8f0; padding-bottom: 8px;">
                                <div style="display: flex; justify-content: space-between; font-size: 13px; font-weight: 600;">
                                    <span><%# Eval("WebUrl") %></span>
                                    <span><strong><%# Eval("TotalPages") %></strong> صفحة</span>
                                </div>
                                <div style="display: flex; gap: 15px; font-size: 11px; color: #64748b; margin-top: 4px;">
                                    <span>مطابقة إنجليزي: <strong style="color: #059669;"><%# Eval("ParityPercentage") %>%</strong></span>
                                    <span>مفقود إنجليزي: <strong style="color: #dc2626;"><%# Eval("MissingEnglishCount") %></strong></span>
                                    <span>مطابقة DGA: <strong style="color: #2563eb;"><%# Eval("DgaPercentage") %>%</strong></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

    </asp:Panel>

    <%-- ========================================================================= --%>
    <%-- TAB 2: PAGES REPORT & BULK LAYOUT CHANGER                                  --%>
    <%-- ========================================================================= --%>
    <asp:Panel ID="pnlTabLayouts" runat="server" Visible="false">
        <div class="pnu-card">
            <h3 style="font-size: 18px; font-weight: 700; color: #0d5c46; margin-bottom: 15px;">
                تقرير وإدارة تخطيطات صفحات الموقع (Pages & Layout Management)
            </h3>
            
            <%-- Filter Bar --%>
            <div class="filter-bar">
                <div class="grid-row-wrap">
                    <div class="grid-col-3">
                        <label class="filter-label">تخطيط الصفحة (Page Layout):</label>
                        <asp:DropDownList ID="ddlLayoutFilter" runat="server" CssClass="filter-control" AutoPostBack="true" OnSelectedIndexChanged="FilterPages_Changed" />
                    </div>
                    <div class="grid-col-3">
                        <label class="filter-label">الموقع الفرعي (Subsite URL):</label>
                        <asp:TextBox ID="txtPagesSubsiteFilter" runat="server" CssClass="filter-control" placeholder="مثال: /ar/Faculties/AD" />
                    </div>
                    <div class="grid-col-3">
                        <label class="filter-label">كلمة البحث (Page Title / URL):</label>
                        <asp:TextBox ID="txtPagesKeyword" runat="server" CssClass="filter-control" placeholder="بحث بالعنوان أو الرابط..." />
                    </div>
                    <div class="grid-col-3">
                        <label class="filter-label">حالة النسخة الإنجليزية:</label>
                        <asp:DropDownList ID="ddlPagesEnStatus" runat="server" CssClass="filter-control">
                            <asp:ListItem Text="الكل (All)" Value="All" Selected="True" />
                            <asp:ListItem Text="نسخة EN متوفرة (Exists)" Value="Exists" />
                            <asp:ListItem Text="نسخة EN مفقودة (Missing)" Value="Missing" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="margin-top: 15px; display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap;">
                    <div style="display: flex; gap: 10px;">
                        <asp:Button ID="btnFilterPages" runat="server" Text="تطبيق التصفية 🔍" OnClick="btnFilterPages_Click" CssClass="btn-action btn-pnu-primary" />
                        <asp:Button ID="btnResetPagesFilter" runat="server" Text="إعادة تعيين ↺" OnClick="btnResetPagesFilter_Click" CssClass="btn-action btn-pnu-outline" />
                    </div>
                    <div>
                        <span>عدد النتائج: </span><strong><asp:Literal ID="litPagesResultsCount" runat="server" Text="0" /></strong>
                    </div>
                </div>
            </div>

            <%-- Bulk Layout Change Toolbar --%>
            <div class="action-toolbar">
                <div style="font-weight: 700; color: #065f46; font-size: 14px; margin-bottom: 10px;">
                    ⚙️ أداة تغيير تخطيط الصفحة (فردي وجماعي للصفحات المحددة):
                </div>
                <div class="grid-row-wrap" style="align-items: flex-end;">
                    <div class="grid-col-4">
                        <label class="filter-label" style="color: #065f46;">التخطيط المستهدف (Target Page Layout):</label>
                        <asp:DropDownList ID="ddlTargetLayout" runat="server" CssClass="filter-control">
                            <asp:ListItem Text="DGANewBlankWebPartPage.aspx (DGA Blank - معتمد)" Value="/_catalogs/masterpage/DGANewBlankWebPartPage.aspx" Selected="True" />
                            <asp:ListItem Text="NewSideMenu.aspx (DGA Side Menu - معتمد)" Value="/_catalogs/masterpage/NewSideMenu.aspx" />
                            <asp:ListItem Text="BlankWebPartPage.aspx (Legacy Blank)" Value="/_catalogs/masterpage/BlankWebPartPage.aspx" />
                            <asp:ListItem Text="ArticleLeft.aspx" Value="/_catalogs/masterpage/ArticleLeft.aspx" />
                            <asp:ListItem Text="ArticleRight.aspx" Value="/_catalogs/masterpage/ArticleRight.aspx" />
                        </asp:DropDownList>
                    </div>
                    <div class="grid-col-4">
                        <div style="display: flex; gap: 12px; flex-wrap: wrap; margin-bottom: 8px;">
                            <label style="font-size: 12px; font-weight: 600; cursor: pointer;">
                                <asp:CheckBox ID="chkLayoutMajorCheckIn" runat="server" Checked="true" /> إيداع رئيسي (Major Check-In)
                            </label>
                            <label style="font-size: 12px; font-weight: 600; cursor: pointer;">
                                <asp:CheckBox ID="chkLayoutAutoPublish" runat="server" Checked="true" /> نشر فوري (Auto Publish)
                            </label>
                        </div>
                    </div>
                    <div class="grid-col-4" style="text-align: left;">
                        <asp:Button ID="btnBulkChangeLayout" runat="server" Text="تطبيق تغيير التخطيط على الصفحات المحددة 🚀" OnClick="btnBulkChangeLayout_Click" OnClientClick="return confirm('هل أنت متأكد من تغيير تخطيط جميع الصفحات المحددة إلى التخطيط المختار؟');" CssClass="btn-action btn-pnu-primary" Style="font-weight: 700;" />
                    </div>
                </div>
            </div>

            <%-- Layouts Summary Table (Compact) --%>
            <div style="margin-bottom: 20px;">
                <h4 style="font-size: 14px; font-weight: 700; color: #334155; margin-bottom: 10px;">
                    إحصائيات التخطيطات الإجمالية:
                </h4>
                <asp:GridView ID="gvLayoutsSummary" runat="server" AutoGenerateColumns="false" CssClass="pnu-table" Width="100%" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="تخطيط الصفحة (Page Layout)">
                            <ItemTemplate>
                                <strong><%# Eval("LayoutFileName") %></strong>
                                <div style="font-size: 11px; color: #64748b;"><%# Eval("PageLayout") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="النوع والمعايير">
                            <ItemTemplate>
                                <%# (bool)Eval("IsDgaCompliant") ? "<span class='badge-dga'>✓ معتمد بهوية DGA</span>" : "<span class='badge-legacy'>تخطيط قديم / Legacy</span>" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PageCount" HeaderText="عدد الصفحات" />
                        <asp:TemplateField HeaderText="النسبة من الإجمالي">
                            <ItemTemplate>
                                <span><%# Eval("Percentage") %>%</span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EnExistsCount" HeaderText="مطابقة EN" />
                        <asp:BoundField DataField="MissingEnCount" HeaderText="مفقود EN" />
                    </Columns>
                </asp:GridView>
            </div>

            <%-- Pages Details Grid --%>
            <h4 style="font-size: 14px; font-weight: 700; color: #334155; margin-bottom: 10px;">
                قائمة الصفحات المطابقة للتصفية (حدد الصفحات لتغيير التخطيط):
            </h4>
            <asp:GridView ID="gvPagesByLayout" runat="server" AutoGenerateColumns="false" CssClass="pnu-table" Width="100%" GridLines="None" AllowPaging="true" PageSize="25" OnPageIndexChanging="gvPagesByLayout_PageIndexChanging" OnRowCommand="gvPagesByLayout_RowCommand">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <input type="checkbox" id="chkSelectAllPagesHeader" onclick="toggleSelectAllPages(this);" title="تحديد الكل" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectPage" runat="server" />
                            <asp:HiddenField ID="hfPageUrl" runat="server" Value='<%# Eval("PageURL") %>' />
                            <asp:HiddenField ID="hfWebUrl" runat="server" Value='<%# Eval("WebUrl") %>' />
                            <asp:HiddenField ID="hfCurrentLayout" runat="server" Value='<%# Eval("PageLayout") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان ورابط الصفحة (AR)">
                        <ItemTemplate>
                            <div style="font-weight: 700; color: #0d5c46;"><%# Eval("PageTitle") %></div>
                            <a href='<%# Eval("PageURL") %>' target="_blank" style="font-size: 12px; color: #2563eb; text-decoration: none; word-break: break-all;">
                                <%# Eval("PageURL") %> ↗
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="تخطيط الصفحة (Layout)">
                        <ItemTemplate>
                            <span style="font-weight: 600;"><%# Eval("PageLayoutFileName") %></span>
                            <div style="margin-top: 3px;">
                                <%# (bool)Eval("IsDgaCompliant") ? "<span class='badge-dga'>DGA</span>" : "<span class='badge-legacy'>Legacy</span>" %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="النسخة الإنجليزية (EN Mirror)">
                        <ItemTemplate>
                            <div>
                                <%# (bool)Eval("EnExists") ? "<span class='badge-yes'>✓ متوفرة</span>" : "<span class='badge-no'>✗ مفقودة</span>" %>
                            </div>
                            <div style="font-size: 11px; color: #64748b; margin-top: 4px; word-break: break-all;">
                                <%# Eval("PageURLEn") %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنصر التحكم (User Control)">
                        <ItemTemplate>
                            <span style="font-size: 11px; color: #475569; word-break: break-all;">
                                <%# string.IsNullOrEmpty(Eval("UserControlPath") as string) ? "<span style='color:#94a3b8;'>-</span>" : Eval("UserControlPath") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="WebUrl" HeaderText="الموقع الفرعي" />
                    <asp:TemplateField HeaderText="إجراءات التخطيط">
                        <ItemTemplate>
                            <asp:Button ID="btnChangeSingleLayout" runat="server" Text="تغيير التخطيط ⚙️" CommandName="ChangeSingleLayout" CommandArgument='<%# Eval("PageURL") %>' CssClass="btn-action btn-pnu-outline" Style="font-size: 11px; padding: 4px 10px;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="padding: 25px; text-align: center; color: #64748b; font-size: 14px;">
                        لا توجد صفحات مطابقة لمعايير البحث والتصفية المحددة.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </asp:Panel>

    <%-- ========================================================================= --%>
    <%-- TAB 3: MISSING PAGES REPORT & CREATION ENGINE                              --%>
    <%-- ========================================================================= --%>
    <asp:Panel ID="pnlTabMissingPages" runat="server" Visible="false">
        <div class="pnu-card">
            <h3 style="font-size: 18px; font-weight: 700; color: #dc2626; margin-bottom: 15px;">
                ⚠️ تقرير وإنشاء الصفحات المفقودة ثنائياً (Missing Pages Gap Report & Creation Engine)
            </h3>
            <p style="font-size: 13px; color: #64748b; margin-bottom: 20px;">
                حصر شامل لجميع الصفحات الموجودة باللغة العربية والتي لا يوجد لها مقابل باللغة الإنجليزية في مسار `/en/`، أو العكس، مع إمكانية إنشاء الصفحات فورياً ونقل المحتوى وعناصر التحكم.
            </p>

            <%-- Filter Bar --%>
            <div class="filter-bar">
                <div class="grid-row-wrap">
                    <div class="grid-col-3">
                        <label class="filter-label">حالة الصفحة (Status):</label>
                        <asp:DropDownList ID="ddlMissingStatusFilter" runat="server" CssClass="filter-control">
                            <asp:ListItem Text="الكل (All)" Value="All" />
                            <asp:ListItem Text="مفقودة / معلقة (Pending/Missing)" Value="Pending" Selected="True" />
                            <asp:ListItem Text="تم إنشاؤها / محلولة (Created/Resolved)" Value="Created" />
                        </asp:DropDownList>
                    </div>
                    <div class="grid-col-3">
                        <label class="filter-label">اتجاه الفجوة اللغوية (Direction):</label>
                        <asp:DropDownList ID="ddlMissingDirectionFilter" runat="server" CssClass="filter-control">
                            <asp:ListItem Text="الكل (All Directions)" Value="All" Selected="True" />
                            <asp:ListItem Text="عربي بدون إنجليزي (AR -> EN Missing)" Value="ArToEn" />
                            <asp:ListItem Text="إنجليزي بدون عربي (EN -> AR Missing)" Value="EnToAr" />
                        </asp:DropDownList>
                    </div>
                    <div class="grid-col-3">
                        <label class="filter-label">الموقع الفرعي (Subsite URL):</label>
                        <asp:TextBox ID="txtMissingSubsiteFilter" runat="server" CssClass="filter-control" placeholder="مثال: /ar/Centers/..." />
                    </div>
                    <div class="grid-col-3">
                        <label class="filter-label">كلمة البحث (Search Keyword):</label>
                        <asp:TextBox ID="txtMissingKeyword" runat="server" CssClass="filter-control" placeholder="اسم الملف أو العنوان..." />
                    </div>
                </div>
                <div style="margin-top: 15px; display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap;">
                    <div style="display: flex; gap: 10px;">
                        <asp:Button ID="btnFilterMissing" runat="server" Text="تطبيق التصفية 🔍" OnClick="btnFilterMissing_Click" CssClass="btn-action btn-pnu-primary" />
                        <asp:Button ID="btnResetMissingFilter" runat="server" Text="إعادة تعيين ↺" OnClick="btnResetMissingFilter_Click" CssClass="btn-action btn-pnu-outline" />
                    </div>
                    <div>
                        <span>عدد الصفحات المفقودة: </span><strong><asp:Literal ID="litMissingCount" runat="server" Text="0" /></strong>
                    </div>
                </div>
            </div>

            <%-- Batch Creation Toolbar --%>
            <div class="action-toolbar">
                <div style="font-weight: 700; color: #065f46; font-size: 14px; margin-bottom: 10px;">
                    ⚡ أداة إنشاء الصفحات المفقودة (Create Missing Pages Engine):
                </div>
                <div class="grid-row-wrap" style="align-items: flex-end;">
                    <div class="grid-col-3">
                        <label class="filter-label" style="color: #065f46;">تخطيط الصفحة المستهدف:</label>
                        <asp:DropDownList ID="ddlMissingTargetLayout" runat="server" CssClass="filter-control">
                            <asp:ListItem Text="استخدام تخطيط الصفحة المصدر (Source Layout)" Value="SourceLayout" Selected="True" />
                            <asp:ListItem Text="DGANewBlankWebPartPage.aspx (DGA Blank - معتمد)" Value="/_catalogs/masterpage/DGANewBlankWebPartPage.aspx" />
                            <asp:ListItem Text="NewSideMenu.aspx (DGA Side Menu - معتمد)" Value="/_catalogs/masterpage/NewSideMenu.aspx" />
                            <asp:ListItem Text="BlankWebPartPage.aspx" Value="/_catalogs/masterpage/BlankWebPartPage.aspx" />
                        </asp:DropDownList>
                    </div>
                    <div class="grid-col-5">
                        <div style="display: flex; gap: 12px; flex-wrap: wrap; margin-bottom: 8px;">
                            <label style="font-size: 12px; font-weight: 600; cursor: pointer;">
                                <asp:CheckBox ID="chkCopyUserControls" runat="server" Checked="true" /> نسخ عناصر التحكم (User Controls)
                            </label>
                            <label style="font-size: 12px; font-weight: 600; cursor: pointer;">
                                <asp:CheckBox ID="chkCopyContentHtml" runat="server" Checked="true" /> نسخ محتوى الصفحة (Content HTML)
                            </label>
                            <label style="font-size: 12px; font-weight: 600; cursor: pointer;">
                                <asp:CheckBox ID="chkAutoPublishCreated" runat="server" Checked="true" /> نشر فوري (Auto Publish)
                            </label>
                        </div>
                    </div>
                    <div class="grid-col-4" style="text-align: left; display: flex; gap: 8px; justify-content: flex-end;">
                        <asp:Button ID="btnBulkCreateMissing" runat="server" Text="إنشاء الصفحات المحددة ⚡" OnClick="btnBulkCreateMissing_Click" OnClientClick="return confirm('سيتم إنشاء الصفحات المحددة تلقائياً في شيربوينت. هل أنت متأكد؟');" CssClass="btn-action btn-pnu-success" Style="font-weight: 700;" />
                        <asp:Button ID="btnCreateAllFilteredMissing" runat="server" Text="إنشاء جميع النتائج المفقودة 🚀" OnClick="btnCreateAllFilteredMissing_Click" OnClientClick="return confirm('تحذير: سيتم إنشاء جميع الصفحات المفقودة في النتائج الحالية تلقائياً. هل تريد المتابعة؟');" CssClass="btn-action btn-pnu-primary" />
                    </div>
                </div>
            </div>

            <%-- Missing Pages Grid --%>
            <asp:GridView ID="gvMissingPages" runat="server" AutoGenerateColumns="false" CssClass="pnu-table" Width="100%" GridLines="None" AllowPaging="true" PageSize="25" OnPageIndexChanging="gvMissingPages_PageIndexChanging" OnRowCommand="gvMissingPages_RowCommand">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <input type="checkbox" id="chkSelectAllMissingHeader" onclick="toggleSelectAllMissing(this);" title="تحديد الكل" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectMissing" runat="server" Enabled='<%# !(bool)Eval("IsCreated") %>' />
                            <asp:HiddenField ID="hfMissingId" runat="server" Value='<%# Eval("Id") %>' />
                            <asp:HiddenField ID="hfMissingTargetUrl" runat="server" Value='<%# Eval("TargetPageUrl") %>' />
                            <asp:HiddenField ID="hfMissingSourceUrl" runat="server" Value='<%# Eval("SourcePageUrl") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="اسم الصفحة والعنوان">
                        <ItemTemplate>
                            <div style="font-weight: 700; color: #1e293b;"><%# Eval("PageFileName") %></div>
                            <div style="font-size: 12px; color: #0d5c46;"><%# Eval("PageTitleAr") %></div>
                            <div style="font-size: 11px; color: #64748b;"><%# Eval("PageTitleEn") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="الرابط المفقود المستهدف (Target Missing URL)">
                        <ItemTemplate>
                            <span style="font-size: 12px; color: #dc2626; font-weight: 600; word-break: break-all;">
                                <%# Eval("TargetPageUrl") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="الرابط المصدر الموجود (Source Exists URL)">
                        <ItemTemplate>
                            <a href='<%# Eval("SourcePageFullUrl") %>' target="_blank" style="font-size: 12px; color: #2563eb; text-decoration: none; word-break: break-all;">
                                <%# Eval("SourcePageUrl") %> ↗
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="التخطيط وعنصر التحكم">
                        <ItemTemplate>
                            <span style="font-size: 12px; font-weight: 600;"><%# Eval("PageLayoutFileName") %></span>
                            <div style="font-size: 11px; color: #64748b; word-break: break-all; margin-top: 3px;">
                                <%# Eval("UserControlPath") %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="الحالة (Status)">
                        <ItemTemplate>
                            <%# (bool)Eval("IsCreated") ? "<span class='badge-yes'>✓ تم الإنشاء</span>" : "<span class='badge-no'>✗ مفقودة</span>" %>
                            <div style="font-size: 10px; color: #94a3b8; margin-top: 3px;">
                                كُشفت: <%# Eval("LastDiscoveredDate", "{0:yyyy-MM-dd}") %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="إجراءات الإنشاء والحل">
                        <ItemTemplate>
                            <div style="display: flex; gap: 6px; flex-wrap: wrap;">
                                <asp:Button ID="btnCreateSingle" runat="server" Text="إنشاء الصفحة ⚡" CommandName="CreateSingleMissing" CommandArgument='<%# Eval("Id") %>' CssClass="btn-action btn-pnu-success" Style="font-size: 11px; padding: 4px 10px;" Visible='<%# !(bool)Eval("IsCreated") %>' />
                                <asp:Button ID="btnMarkResolved" runat="server" Text="تحديد كمحلولة ✓" CommandName="ResolvePage" CommandArgument='<%# Eval("Id") + "|" + Eval("TargetPageUrl") %>' CssClass="btn-action btn-pnu-outline" Style="font-size: 11px; padding: 4px 10px;" Visible='<%# !(bool)Eval("IsCreated") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="padding: 25px; text-align: center; color: #166534; font-size: 14px;">
                        🎉 رائع! لا توجد صفحات مفقودة مطابقة لمعايير البحث والتصفية.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </asp:Panel>

    <%-- ========================================================================= --%>
    <%-- TAB 4: USER CONTROLS & WEB PARTS REPORT                                    --%>
    <%-- ========================================================================= --%>
    <asp:Panel ID="pnlTabUserControls" runat="server" Visible="false">
        <div class="pnu-card">
            <h3 style="font-size: 18px; font-weight: 700; color: #0d5c46; margin-bottom: 15px;">
                🧩 تقرير عناصر التحكم وقطع الويب (User Controls & Web Parts Inventory)
            </h3>
            <p style="font-size: 13px; color: #64748b; margin-bottom: 20px;">
                حصر لجميع عناصر التحكم المخصصة (`.ascx` / `ControlLoaderWebPart` / قطع الويب) المضمنة في صفحات البوابة مع عدد الصفحات التي تستخدم كل عنصر ونماذج منها.
            </p>

            <asp:GridView ID="gvUserControls" runat="server" AutoGenerateColumns="false" CssClass="pnu-table" Width="100%" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="عنصر التحكم (User Control / Component)">
                        <ItemTemplate>
                            <strong style="color: #0f172a;"><%# Eval("ControlFileName") %></strong>
                            <div style="font-size: 11px; color: #64748b; font-family: monospace; word-break: break-all;">
                                <%# Eval("UserControlPath") %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PageCount" HeaderText="إجمالي الصفحات المستخدمة" />
                    <asp:BoundField DataField="ArPagesCount" HeaderText="الصفحات العربية" />
                    <asp:BoundField DataField="EnPagesCount" HeaderText="الصفحات الإنجليزية" />
                    <asp:TemplateField HeaderText="نموذج صفحة تستخدم العنصر">
                        <ItemTemplate>
                            <a href='<%# Eval("SamplePageUrl") %>' target="_blank" style="font-size: 12px; color: #2563eb; text-decoration: none;">
                                <%# Eval("SamplePageTitle") ?? Eval("SamplePageUrl") %> ↗
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="padding: 25px; text-align: center; color: #64748b; font-size: 14px;">
                        لم يتم العثور على عناصر تحكم مسجلة في الفهرس حتى الآن.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </asp:Panel>

    <%-- ========================================================================= --%>
    <%-- TAB 5: SUBSITES HEALTH SCORECARD                                           --%>
    <%-- ========================================================================= --%>
    <asp:Panel ID="pnlTabSubsites" runat="server" Visible="false">
        <div class="pnu-card">
            <h3 style="font-size: 18px; font-weight: 700; color: #0d5c46; margin-bottom: 15px;">
                🏛️ تقرير مؤشرات صحة المواقع الفرعية (Subsites Health Scorecard)
            </h3>
            <p style="font-size: 13px; color: #64748b; margin-bottom: 20px;">
                تقييم شامل لكل موقع فرعي في البوابة (كليات، عمادات، مراكز، إدارات) من حيث إجمالي الصفحات، نسبة التوافق اللغوي، والامتثال لمعايير DGA.
            </p>

            <asp:GridView ID="gvSubsitesHealth" runat="server" AutoGenerateColumns="false" CssClass="pnu-table" Width="100%" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="WebUrl" HeaderText="رابط الموقع الفرعي (Subsite URL)" />
                    <asp:BoundField DataField="TotalPages" HeaderText="إجمالي الصفحات" />
                    <asp:BoundField DataField="HasEnglishCount" HeaderText="صفحات متوفرة بالإنجليزية" />
                    <asp:TemplateField HeaderText="صفحات مفقودة بالإنجليزية">
                        <ItemTemplate>
                            <span style='color: <%# Convert.ToInt32(Eval("MissingEnglishCount")) > 0 ? "#dc2626" : "#16a34a" %>; font-weight: 700;'>
                                <%# Eval("MissingEnglishCount") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="نسبة التوافق اللغوي (Parity %)">
                        <ItemTemplate>
                            <strong><%# Eval("ParityPercentage") %>%</strong>
                            <div class="progress-track" style="height: 6px;">
                                <div class="progress-fill progress-fill-green" style='width: <%# Eval("ParityPercentage") %>%;'></div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="امتثال DGA %">
                        <ItemTemplate>
                            <strong><%# Eval("DgaPercentage") %>%</strong>
                            <div class="progress-track" style="height: 6px;">
                                <div class="progress-fill progress-fill-blue" style='width: <%# Eval("DgaPercentage") %>%;'></div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <%-- ========================================================================= --%>
    <%-- TAB 6: LIVE SUBSITE SCANNER & SYNC ENGINE                                  --%>
    <%-- ========================================================================= --%>
    <asp:Panel ID="pnlTabLiveScan" runat="server" Visible="false">
        <div class="pnu-card">
            <h3 style="font-size: 18px; font-weight: 700; color: #0d5c46; margin-bottom: 15px;">
                🔍 ماسح المواقع المباشر وتحديث الفهرس (Live Subsite Scanner & Sync)
            </h3>
            <p style="font-size: 13px; color: #64748b; margin-bottom: 20px;">
                فحص مباشر لصفحات شيربوينت وتخطيطاتها وعناصر التحكم بها لموقع محدد أو للمجموعة بأكملها، وتحديث جداول التقارير (`dbo.WebsitePages` و `dbo.MissingPagesOnEN`).
            </p>

            <div class="filter-bar">
                <div class="grid-row-wrap">
                    <div class="grid-col-6">
                        <label class="filter-label">رابط الموقع المستهدف للفحص (Target Subsite URL):</label>
                        <asp:TextBox ID="txtLiveScanUrl" runat="server" CssClass="filter-control" placeholder="اتركه فارغاً لفحص كامل الموقع، أو أدخل مسار مثل: /ar/Faculties/CS" />
                    </div>
                    <div class="grid-col-6" style="display: flex; align-items: flex-end; gap: 15px;">
                        <div style="display: flex; align-items: center; gap: 8px; margin-bottom: 8px;">
                            <asp:CheckBox ID="chkCurrentSubsiteOnly" runat="server" />
                            <label for="<%= chkCurrentSubsiteOnly.ClientID %>" style="font-size: 13px; font-weight: 600; cursor: pointer;">
                                فحص الموقع المحدد فقط بدون المواقع المتفرعة عنه (Current Web Only)
                            </label>
                        </div>
                    </div>
                </div>
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnStartLiveScan" runat="server" Text="بدء الفحص والتحديث الفوري 🚀" OnClick="btnStartLiveScan_Click" CssClass="btn-action btn-pnu-primary" />
                </div>
            </div>

            <%-- Live Execution Console --%>
            <h4 style="font-size: 14px; font-weight: 700; color: #334155; margin-bottom: 8px;">
                سجل تنفيذ الفحص المباشر (Live Scan Execution Log):
            </h4>
            <div class="console-log">
                <asp:Literal ID="litLiveScanLog" runat="server" Text="Console idle. Click 'بدء الفحص والتحديث الفوري' to start live crawling and data indexing." />
            </div>
        </div>
    </asp:Panel>

</div>
