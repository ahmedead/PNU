<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBilingualPageCreator.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucBilingualPageCreator" %>

<div class="card shadow-sm border-0 rounded-3 p-4 my-4" dir="rtl">
    <!-- Header -->
    <div class="d-flex align-items-center justify-content-between border-bottom pb-3 mb-4">
        <div class="d-flex align-items-center gap-3">
            <div class="icon-container bg-primary-25 text-primary p-3 rounded-circle">
               <i class="hgi hgi-stroke hgi-translate fs-2 text-primary"></i>
            </div>
            <div>
                <h2 class="h4 fw-bold text-dark mb-1">أداة إنشاء ومزامنة الصفحات ثنائية اللغة (Bilingual Missing Pages Creator)</h2>
                <p class="text-muted small mb-0">مطابقة صفحات /ar/ و /en/ تلقائياً، استنساخ تخطيط الصفحة (Page Layout)، نقل Control Loader وإعداداته، ونقل Content Editor والمحتوى مع الترجمة الذكية.</p>
            </div>
        </div>
        <span class="badge bg-primary text-white px-3 py-2 fs-6">SharePoint Bilingual Engine</span>
    </div>

    <!-- Mode Tabs Navigation -->
    <ul class="nav nav-tabs mb-4" id="bilingualTabs" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active fw-semibold" id="tab-select-site" data-bs-toggle="tab" data-bs-target="#pane-select-site" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-hierarchy-square-02 me-1"></i> اختيار موقع فرعي من الشجرة / القائمة
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-semibold" id="tab-url-site" data-bs-toggle="tab" data-bs-target="#pane-url-site" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-link-02 me-1"></i> إدخال رابط الموقع مباشرة (Site URL)
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-semibold" id="tab-direct-pages" data-bs-toggle="tab" data-bs-target="#pane-direct-pages" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-file-edit me-1"></i> إدخال روابط الصفحات المفقودة (Multiline Textbox)
            </button>
        </li>
    </ul>

    <!-- Options Section -->
    <div class="p-3 bg-light rounded-3 mb-4 border">
        <div class="d-flex align-items-center justify-content-between flex-wrap gap-2 mb-3 border-bottom pb-2">
            <h3 class="h6 fw-bold text-dark mb-0">خيارات المعالجة والاستنساخ والترجمة:</h3>
            <span class="badge bg-success-subtle text-success-emphasis border border-success-subtle px-3 py-1 small">
                <i class="hgi hgi-stroke hgi-shield-check me-1"></i> الموقع العربي (/ar/) هو المصدر الأساسي للمحتوى
            </span>
        </div>

        <div class="row g-3 mb-3">
            <div class="col-md-12">
                <label class="form-label fw-semibold text-dark small">اتجاه المزامنة وإنشاء الصفحات:</label>
                <asp:DropDownList ID="ddlSyncDirection" runat="server" CssClass="form-select text-start fw-semibold border-primary-subtle">
                    <asp:ListItem Value="ArToEn" Selected="True">إنشاء الصفحات الإنجليزية المفقودة استناداً للمحتوى العربي (/ar/ ➔ /en/) [الافتراضي والرئيسي]</asp:ListItem>
                    <asp:ListItem Value="Both">مزامنة ثنائية في كلا الاتجاهين (/ar/ ↔ /en/)</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row g-3">
            <div class="col-md-4">
                <div class="form-check">
                    <asp:CheckBox ID="chkCopyControlLoader" runat="server" Checked="true" Text="استنساخ ControlLoader وإعداداته" CssClass="form-check-label text-dark small fw-semibold" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-check">
                    <asp:CheckBox ID="chkCopyContentEditor" runat="server" Checked="true" Text="استنساخ Content Editor ومحتوى الصفحة" CssClass="form-check-label text-dark small fw-semibold" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-check">
                    <asp:CheckBox ID="chkOverwrite" runat="server" Checked="false" Text="إعادة إنشاء إذا كانت الصفحة موجودة" CssClass="form-check-label text-dark small text-danger" />
                </div>
            </div>
        </div>
        <div class="row g-3 mt-1">
                <div class="form-check">
                    <asp:CheckBox ID="chkMajorCheckIn" runat="server" Checked="true" Text="عمل Major Check-in بإصدار رئيسي" CssClass="form-check-label text-dark small" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-check">
                    <asp:CheckBox ID="chkAutoPublish" runat="server" Checked="true" Text="نشر الصفحة تلقائياً (Publish)" CssClass="form-check-label text-dark small" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-check">
                    <asp:CheckBox ID="chkAutoApprove" runat="server" Checked="true" Text="اعتماد الصفحة تلقائياً (Approve)" CssClass="form-check-label text-dark small" />
                </div>
            </div>
        </div>
    </div>

    <!-- Tab Panes -->
    <div class="tab-content" id="bilingualTabsContent">
        
        <!-- PANE 1: SELECT SUBSITE -->
        <div class="tab-pane fade show active" id="pane-select-site" role="tabpanel">
            <div class="row g-3 align-items-end mb-4">
                <div class="col-md-6">
                    <label class="form-label fw-semibold text-dark small">اختر الموقع المطلوب فحصه ومقارنة صفحاته بين /ar/ و /en/:</label>
                    <asp:DropDownList ID="ddlSubsites" runat="server" CssClass="form-select text-start" dir="ltr" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnScanDropdownSite" runat="server" Text="فحص الصفحات المفقودة" CssClass="btn btn-outline-primary w-100 fw-semibold" OnClick="btnScanDropdownSite_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnCompareExistingDropdown" runat="server" Text="مقارنة الصفحات الموجودة (/ar/ &amp; /en/)" CssClass="btn btn-success w-100 fw-semibold" OnClick="btnCompareExistingDropdown_Click" />
                </div>
            </div>
        </div>

        <!-- PANE 2: ENTER SITE URL -->
        <div class="tab-pane fade" id="pane-url-site" role="tabpanel">
            <div class="row g-3 align-items-end mb-4">
                <div class="col-md-6">
                    <label class="form-label fw-semibold text-dark small">أدخل رابط الموقع الفرعي (Server Relative أو Absolute URL):</label>
                    <asp:TextBox ID="txtSiteUrl" runat="server" CssClass="form-control font-monospace text-start" placeholder="/ar/Faculties/CS أو https://pnu.edu.sa/ar/Deanship/Admission" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnScanUrlSite" runat="server" Text="فحص الصفحات المفقودة" CssClass="btn btn-outline-primary w-100 fw-semibold" OnClick="btnScanUrlSite_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnCompareExistingUrl" runat="server" Text="مقارنة الصفحات الموجودة (/ar/ &amp; /en/)" CssClass="btn btn-success w-100 fw-semibold" OnClick="btnCompareExistingUrl_Click" />
                </div>
            </div>
        </div>

        <!-- PANE 3: MULTILINE TEXTBOX DIRECT INPUT -->
        <div class="tab-pane fade" id="pane-direct-pages" role="tabpanel">
            <div class="alert alert-secondary py-2 px-3 small mb-3">
                <strong>تنسيق الإدخال:</strong> الصق روابط الصفحات المراد إنشاؤها في الجهة المقابلة (سطر لكل صفحة).
                <br /><small class="text-muted">مثال: <code>/ar/Faculties/CS/Pages/About.aspx</code> (سيتم فحصها وإنشاؤها في <code>/en/Faculties/CS/Pages/About.aspx</code> أو العكس تلقائياً).</small>
            </div>
            <div class="mb-3">
                <label class="form-label fw-semibold text-dark small">روابط الصفحات (سطر لكل رابط):</label>
                <asp:TextBox ID="txtDirectPages" runat="server" TextMode="MultiLine" Rows="6" CssClass="form-control font-monospace text-start" placeholder="/ar/Faculties/CS/Pages/About.aspx&#10;/ar/Faculties/CS/Pages/DeanSpeech.aspx&#10;/en/Deanship/Admission/Pages/Conditions.aspx" />
            </div>
            <div class="d-flex gap-2 mb-4">
                <asp:Button ID="btnProcessDirectPages" runat="server" Text="تحليل وتجهيز الصفحات المدخلة" CssClass="btn btn-primary px-4 fw-semibold" OnClick="btnProcessDirectPages_Click" />
            </div>
        </div>

    </div>

    <!-- MISSING PAGES REVIEW GRID PANEL -->
    <asp:Panel ID="pnlMissingPages" runat="server" Visible="false" CssClass="mt-4 border-top pt-4">
        <div class="d-flex align-items-center justify-content-between mb-3 flex-wrap gap-2">
            <div>
                <h3 class="h5 fw-bold text-dark mb-1">
                    <i class="hgi hgi-stroke hgi-checklist me-1 text-primary"></i> الصفحات المفقودة المكتشفة (<asp:Literal ID="litMissingCount" runat="server" Text="0" /> صفحة)
                </h3>
                <p class="text-muted small mb-0">راجع الصفحات المكتشفة وتفاصيلها قبل البدء في إنشائها.</p>
            </div>
            <div class="d-flex gap-2 flex-wrap align-items-center">
                <button type="button" class="btn btn-sm btn-outline-secondary" onclick="toggleSelectAllBilingual(true)">تحديد الكل</button>
                <button type="button" class="btn btn-sm btn-outline-secondary" onclick="toggleSelectAllBilingual(false)">إلغاء التحديد</button>
                <asp:Button ID="btnExportMissingPages" runat="server" Text="تصدير الصفحات (Excel / CSV)" CssClass="btn btn-outline-success fw-semibold" OnClick="btnExportMissingPages_Click" />
                <asp:Button ID="btnCreateSelectedPages" runat="server" Text="إنشاء ومزامنة الصفحات المحددة" CssClass="btn btn-success fw-semibold px-4" OnClick="btnCreateSelectedPages_Click" />
            </div>
        </div>

        <div class="table-responsive border rounded-3 mb-4">
            <table class="table table-hover table-striped mb-0 align-middle">
                <thead class="table-light">
                    <tr>
                        <th style="width: 40px;" class="text-center">#</th>
                        <th style="width: 40px;" class="text-center">تحديد</th>
                        <th>الصفحة والموقع المصدر (الموجودة)</th>
                        <th>الموقع المستهدف (المفقودة)</th>
                        <th>تخطيط الصفحة (Page Layout)</th>
                        <th>المكونات المكتشفة</th>
                        <th>الاتجاه</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptMissingPages" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="text-center text-muted small"><%# Container.ItemIndex + 1 %></td>
                                <td class="text-center">
                                    <asp:CheckBox ID="chkSelectPage" runat="server" Checked="true" />
                                    <asp:HiddenField ID="hfItemIndex" runat="server" Value="<%# Container.ItemIndex %>" />
                                </td>
                                <td>
                                    <div class="fw-bold text-dark"><%# Eval("SourcePageTitle") %></div>
                                    <div class="font-monospace small text-primary text-start" dir="ltr"><%# Eval("SourcePageUrl") %></div>
                                </td>
                                <td>
                                    <div class="font-monospace small text-success text-start fw-semibold" dir="ltr"><%# Eval("TargetPageUrl") %></div>
                                    <div class="small text-muted">عنوان الصفحة: <%# Eval("SourcePageTitle") %></div>
                                </td>
                                <td>
                                    <span class="badge bg-light text-dark border font-monospace small"><%# Eval("PageLayoutFileName") %></span>
                                </td>
                                <td>
                                    <%# (bool)Eval("HasControlLoader") ? "<span class='badge bg-info text-dark me-1'>ControlLoader</span>" : "" %>
                                    <%# (bool)Eval("HasContentEditor") ? "<span class='badge bg-warning text-dark me-1'>ContentEditor</span>" : "" %>
                                    <%# (bool)Eval("HasPageContent") ? "<span class='badge bg-secondary text-white me-1'>PageContent</span>" : "" %>
                                    <%# !(bool)Eval("HasControlLoader") && !(bool)Eval("HasContentEditor") && !(bool)Eval("HasPageContent") ? "<span class='text-muted small'>صفحة أساسية</span>" : "" %>
                                </td>
                                <td>
                                    <span class="badge <%# (string)Eval("Direction") == "ArToEn" ? "bg-primary" : "bg-success" %>">
                                        <%# (string)Eval("Direction") == "ArToEn" ? "عربي ← إنجليزي" : "إنجليزي ← عربي" %>
                                    </span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </asp:Panel>

    <!-- EXISTING PAGES COMPARISON GRID PANEL -->
    <asp:Panel ID="pnlExistingPages" runat="server" Visible="false" CssClass="mt-4 border-top pt-4">
        <div class="d-flex align-items-center justify-content-between mb-3 flex-wrap gap-2">
            <div>
                <h3 class="h5 fw-bold text-dark mb-1">
                    <i class="hgi hgi-stroke hgi-git-compare me-1 text-success"></i> مقارنة تخطيط ومحتوى الصفحات الموجودة في اللغتين (<asp:Literal ID="litExistingCount" runat="server" Text="0" /> صفحة)
                </h3>
                <p class="text-muted small mb-0">تحليل مقارن لتخطيطات الصفحات (Page Layout) والمكونات والمحتوى بين النسخة العربية والإنجليزية.</p>
            </div>
            <div class="d-flex gap-2 flex-wrap align-items-center">
                <asp:Button ID="btnExportExistingPages" runat="server" Text="تصدير المقارنة (Excel)" CssClass="btn btn-outline-success fw-semibold" OnClick="btnExportExistingPages_Click" />
                <asp:Button ID="btnSyncSelectedLayouts" runat="server" Text="مزامنة تخطيط الصفحات المحددة إلى DGA" CssClass="btn btn-primary fw-semibold px-4" OnClick="btnSyncSelectedLayouts_Click" />
            </div>
        </div>

        <!-- Summary Cards Row -->
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="card border-0 bg-light rounded-3 p-3 text-center">
                    <span class="text-muted small fw-semibold">إجمالي الصفحات المشتركة</span>
                    <h4 class="fw-bold text-dark mb-0 mt-1"><asp:Literal ID="litTotalExisting" runat="server" Text="0" /></h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card border-0 bg-success-subtle rounded-3 p-3 text-center">
                    <span class="text-success-emphasis small fw-semibold">تخطيط مطابق (Matching)</span>
                    <h4 class="fw-bold text-success mb-0 mt-1"><asp:Literal ID="litMatchingLayouts" runat="server" Text="0" /></h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card border-0 bg-danger-subtle rounded-3 p-3 text-center">
                    <span class="text-danger-emphasis small fw-semibold">اختلاف في التخطيط (Mismatch)</span>
                    <h4 class="fw-bold text-danger mb-0 mt-1"><asp:Literal ID="litDifferentLayouts" runat="server" Text="0" /></h4>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="card border-0 bg-warning-subtle rounded-3 p-3 text-center">
                    <span class="text-warning-emphasis small fw-semibold">اختلاف/نقص في المحتوى</span>
                    <h4 class="fw-bold text-dark mb-0 mt-1"><asp:Literal ID="litContentDifferences" runat="server" Text="0" /></h4>
                </div>
            </div>
        </div>

        <!-- Filter & Batch Bar -->
        <div class="d-flex align-items-center justify-content-between mb-3 flex-wrap gap-2 bg-light p-2 border rounded-3">
            <div class="d-flex align-items-center gap-2 flex-wrap">
                <span class="small fw-bold text-dark"><i class="hgi hgi-stroke hgi-filter me-1"></i> تصفية النتائج:</span>
                <asp:DropDownList ID="ddlExistingFilter" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlExistingFilter_SelectedIndexChanged" style="min-width: 220px;">
                    <asp:ListItem Value="All" Selected="True">جميع الصفحات المشتركة</asp:ListItem>
                    <asp:ListItem Value="DifferentLayouts">الصفحات ذات التخطيط المختلف فقط (Layout Mismatch)</asp:ListItem>
                    <asp:ListItem Value="DifferentContent">الصفحات ذات المحتوى المختلف أو الناقص فقط</asp:ListItem>
                    <asp:ListItem Value="LegacyLayouts">الصفحات بتخطيطات قديمة بحاجة لـ DGA</asp:ListItem>
                    <asp:ListItem Value="MatchingAll">الصفحات المتطابقة تماماً</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="d-flex gap-2">
                <button type="button" class="btn btn-sm btn-outline-secondary" onclick="toggleSelectAllExisting(true)">تحديد الكل</button>
                <button type="button" class="btn btn-sm btn-outline-secondary" onclick="toggleSelectAllExisting(false)">إلغاء التحديد</button>
            </div>
        </div>

        <div class="table-responsive border rounded-3 mb-4">
            <table class="table table-hover table-striped mb-0 align-middle">
                <thead class="table-light">
                    <tr>
                        <th style="width: 40px;" class="text-center">#</th>
                        <th style="width: 40px;" class="text-center">تحديد</th>
                        <th>اسم الصفحة والروابط</th>
                        <th>التخطيط العربي (/ar/)</th>
                        <th>التخطيط الإنجليزي (/en/)</th>
                        <th>حالة التخطيط</th>
                        <th>محتوى الصفحة العربي</th>
                        <th>محتوى الصفحة الإنجليزي</th>
                        <th>الحالة العامة</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptExistingPages" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="text-center text-muted small"><%# Container.ItemIndex + 1 %></td>
                                <td class="text-center">
                                    <asp:CheckBox ID="chkSelectExistingPage" runat="server" Checked='<%# (bool)Eval("LayoutIsDifferent") %>' />
                                    <asp:HiddenField ID="hfExistingIndex" runat="server" Value="<%# Container.ItemIndex %>" />
                                </td>
                                <td>
                                    <div class="fw-bold text-dark"><%# Eval("ArPageTitle") %></div>
                                    <div class="font-monospace small text-primary text-start" dir="ltr">
                                        <a href='<%# Eval("ArPageUrl") %>' target='_blank' class='text-decoration-none text-primary'>AR: <%# Eval("ArPageUrl") %></a>
                                    </div>
                                    <div class="font-monospace small text-success text-start" dir="ltr">
                                        <a href='<%# Eval("EnPageUrl") %>' target='_blank' class='text-decoration-none text-success'>EN: <%# Eval("EnPageUrl") %></a>
                                    </div>
                                </td>
                                <td>
                                    <span class="badge bg-light text-dark border font-monospace small"><%# Eval("ArPageLayoutFileName") %></span>
                                    <%# (bool)Eval("ArIsDgaLayout") ? "<span class='badge bg-success text-white small ms-1'>DGA</span>" : "<span class='badge bg-secondary text-white small ms-1'>Legacy</span>" %>
                                </td>
                                <td>
                                    <span class="badge bg-light text-dark border font-monospace small"><%# Eval("EnPageLayoutFileName") %></span>
                                    <%# (bool)Eval("EnIsDgaLayout") ? "<span class='badge bg-success text-white small ms-1'>DGA</span>" : "<span class='badge bg-secondary text-white small ms-1'>Legacy</span>" %>
                                </td>
                                <td>
                                    <%# (bool)Eval("LayoutIsDifferent") 
                                        ? "<span class='badge bg-danger text-white'><i class='hgi hgi-stroke hgi-alert-circle me-1'></i>تخطيط مختلف</span>" 
                                        : "<span class='badge bg-success text-white'><i class='hgi hgi-stroke hgi-tick-double me-1'></i>مطابق</span>" %>
                                </td>
                                <td>
                                    <%# (bool)Eval("ArHasPageContent") ? "<span class='badge bg-secondary text-white me-1' title='نص الصفحة: " + Eval("ArContentLength") + " حرف'>محتوى (" + Eval("ArContentLength") + ")</span>" : "<span class='badge bg-light text-muted border me-1'>فارغ</span>" %>
                                    <%# (bool)Eval("ArHasControlLoader") ? "<span class='badge bg-info text-dark me-1' title='" + Eval("ArUserControlPath") + "'>ControlLoader</span>" : "" %>
                                    <%# (bool)Eval("ArHasContentEditor") ? "<span class='badge bg-warning text-dark me-1'>ContentEditor</span>" : "" %>
                                </td>
                                <td>
                                    <%# (bool)Eval("EnHasPageContent") ? "<span class='badge bg-secondary text-white me-1' title='نص الصفحة: " + Eval("EnContentLength") + " حرف'>محتوى (" + Eval("EnContentLength") + ")</span>" : "<span class='badge bg-light text-danger border border-danger-subtle me-1'>بدون محتوى</span>" %>
                                    <%# (bool)Eval("EnHasControlLoader") ? "<span class='badge bg-info text-dark me-1' title='" + Eval("EnUserControlPath") + "'>ControlLoader</span>" : "" %>
                                    <%# (bool)Eval("EnHasContentEditor") ? "<span class='badge bg-warning text-dark me-1'>ContentEditor</span>" : "" %>
                                </td>
                                <td>
                                    <span class='badge <%# Eval("StatusBadgeClass") %>'><%# Eval("StatusBadgeText") %></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </asp:Panel>

    <!-- RESULTS & SUMMARY PANEL -->
    <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="mt-4 border-top pt-4">
        <h3 class="h5 fw-bold text-dark mb-3">نتائج المعالجة والإنشاء:</h3>
        
        <asp:Literal ID="litSummaryCard" runat="server" />

        <!-- CREATED PAGES URLS PANEL -->
        <asp:Panel ID="pnlCreatedUrls" runat="server" Visible="false" CssClass="card border-success-subtle bg-white shadow-sm mb-4">
            <div class="card-header bg-success-subtle d-flex align-items-center justify-content-between py-2 px-3 flex-wrap gap-2">
                <div class="d-flex align-items-center gap-2">
                    <i class="hgi hgi-stroke hgi-link-02 text-success fs-5"></i>
                    <span class="fw-bold text-success">روابط الصفحات التي تم إنشاؤها بنجاح (<asp:Literal ID="litCreatedUrlsCount" runat="server" Text="0" /> رابط)</span>
                </div>
                <button type="button" class="btn btn-sm btn-success fw-semibold d-flex align-items-center gap-1" onclick="copyCreatedUrlsToClipboard()">
                    <i class="hgi hgi-stroke hgi-copy-01"></i> نسخ جميع الروابط
                </button>
            </div>
            <div class="card-body p-3">
                <asp:TextBox ID="txtCreatedPagesUrls" runat="server" TextMode="MultiLine" Rows="6" CssClass="form-control font-monospace text-start small border-success-subtle" dir="ltr"></asp:TextBox>
                <div class="d-flex justify-content-between align-items-center mt-2 flex-wrap gap-2">
                    <div class="form-text text-muted small mb-0">
                        <i class="hgi hgi-stroke hgi-information-circle me-1"></i> يمكنك نسخ كافة الروابط المكتملة أعلاه دفعة واحدة أو تحديد ما ترغب بنسخه.
                    </div>
                    <span id="spnCopyFeedback" class="badge bg-success text-white px-3 py-2 d-none">تم النسخ بنجاح!</span>
                </div>
            </div>
        </asp:Panel>

        <div class="card border rounded-3 p-3 bg-light">
            <h4 class="h6 fw-bold text-dark mb-2 border-bottom pb-2">سجل التنفيذ التفصيلي:</h4>
            <div class="font-monospace small text-start overflow-auto" style="max-height: 400px;" dir="ltr">
                <asp:Literal ID="litDetailedLog" runat="server" />
            </div>
        </div>
    </asp:Panel>

</div>

<script type="text/javascript">
    function toggleSelectAllBilingual(check) {
        var checkboxes = document.querySelectorAll("input[id*='chkSelectPage']");
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = check;
        }
    }

    function toggleSelectAllExisting(check) {
        var checkboxes = document.querySelectorAll("input[id*='chkSelectExistingPage']");
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = check;
        }
    }

    function copyCreatedUrlsToClipboard() {
        var txt = document.getElementById('<%= txtCreatedPagesUrls.ClientID %>');
        if (txt && txt.value) {
            txt.select();
            txt.setSelectionRange(0, 99999);
            if (navigator.clipboard && window.isSecureContext) {
                navigator.clipboard.writeText(txt.value).then(function() {
                    showCopyBadge();
                }).catch(function() {
                    document.execCommand('copy');
                    showCopyBadge();
                });
            } else {
                document.execCommand('copy');
                showCopyBadge();
            }
        }
    }

    function showCopyBadge() {
        var badge = document.getElementById('spnCopyFeedback');
        if (badge) {
            badge.classList.remove('d-none');
            setTimeout(function() {
                badge.classList.add('d-none');
            }, 3000);
        }
    }
</script>
