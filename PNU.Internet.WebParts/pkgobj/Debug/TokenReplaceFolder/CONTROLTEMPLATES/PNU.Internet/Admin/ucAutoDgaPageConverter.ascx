<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAutoDgaPageConverter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucAutoDgaPageConverter" %>

<div class="container-fluid py-4" dir="rtl">
    <!-- Header Card -->
    <div class="card border-0 shadow-sm rounded-3 mb-4 bg-primary-25 p-4">
        <div class="d-flex align-items-center justify-content-between flex-wrap gap-3">
            <div class="d-flex align-items-center gap-3">
                <div class="icon-container bg-white rounded-circle p-3 shadow-sm text-primary">
                    <i class="hgi hgi-stroke hgi-magic-wand fs-2"></i>
                </div>
                <div>
                    <h2 class="h4 fw-bold text-dark mb-1">المحول الآلي الشامل لصفحات شيربوينت (DGA Auto Page Converter)</h2>
                    <p class="text-muted small mb-0">قراءة محتوى أي صفحة في شيربوينت تلقائياً، وإعادة هيكلتها وفق معايير DGA، وتغيير التخطيط إلى Blank Web Part Page ونشرها بضغطة زر واحدة.</p>
                </div>
            </div>
            <div class="d-flex align-items-center gap-2">
                <span class="badge bg-primary px-3 py-2 fs-6">SharePoint 2016 / 2019 / Subscription</span>
            </div>
        </div>
    </div>

    <!-- Navigation Tabs -->
    <ul class="nav nav-pills mb-4 gap-2 border-bottom pb-3" id="pills-tab" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active fw-bold px-4 py-2" id="tab-bulk-urls-btn" data-bs-toggle="pill" data-bs-target="#tab-bulk-urls" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-link-02 me-1"></i>تحويل جماعي لروابط صفحات (Bulk URLs Converter)
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-bold px-4 py-2" id="tab-single-url-btn" data-bs-toggle="pill" data-bs-target="#tab-single-url" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-file-search me-1"></i>تحويل وفحص رابط فردي (Single Page & Preview)
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-bold px-4 py-2" id="tab-subsite-scan-btn" data-bs-toggle="pill" data-bs-target="#tab-subsite-scan" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-folder-library me-1"></i>مسح موقع فرعي كامل (Subsite Scanner)
            </button>
        </li>
    </ul>

    <!-- Options Panel -->
    <div class="card border shadow-sm rounded-3 p-3 mb-4 bg-light">
        <div class="row g-3 align-items-center">
            <div class="col-12 col-md-3">
                <div class="form-check form-switch">
                    <asp:CheckBox ID="chkChangeLayout" runat="server" Checked="true" CssClass="form-check-input" />
                    <label class="form-check-label fw-bold text-dark" for="<%= chkChangeLayout.ClientID %>">تغيير تخطيط الصفحة (Layout)</label>
                </div>
            </div>
            <div class="col-12 col-md-3">
                <div class="form-check form-switch">
                    <asp:CheckBox ID="chkMajorCheckIn" runat="server" Checked="true" CssClass="form-check-input" />
                    <label class="form-check-label fw-bold text-dark" for="<%= chkMajorCheckIn.ClientID %>">حجز وإيداع رئيسي (Major Check-In)</label>
                </div>
            </div>
            <div class="col-12 col-md-3">
                <div class="form-check form-switch">
                    <asp:CheckBox ID="chkAutoPublish" runat="server" Checked="true" CssClass="form-check-input" />
                    <label class="form-check-label fw-bold text-dark" for="<%= chkAutoPublish.ClientID %>">نشر الصفحة تلقائياً (Auto Publish)</label>
                </div>
            </div>
            <div class="col-12 col-md-3">
                <div class="form-check form-switch">
                    <asp:CheckBox ID="chkAutoApprove" runat="server" Checked="true" CssClass="form-check-input" />
                    <label class="form-check-label fw-bold text-dark" for="<%= chkAutoApprove.ClientID %>">اعتماد النسخة تلقائياً (Auto Approve)</label>
                </div>
            </div>
        </div>
        <div class="row mt-2 pt-2 border-top">
            <div class="col-12 col-md-6">
                <label class="form-label small text-muted mb-1">مسار تخطيط الصفحة المستهدف (Target Page Layout):</label>
                <asp:TextBox ID="txtTargetLayout" runat="server" CssClass="form-control form-control-sm font-monospace text-start" 
                    Text="/_catalogs/masterpage/DGANewBlankWebPartPage.aspx"></asp:TextBox>
            </div>
        </div>
    </div>

    <!-- Tab Contents -->
    <div class="tab-content" id="pills-tabContent">
        <!-- TAB 1: BULK URLS CONVERTER -->
        <div class="tab-pane fade show active" id="tab-bulk-urls" role="tabpanel">
            <div class="card border shadow-sm rounded-3 p-4 mb-4">
                <h3 class="h5 fw-bold text-dark mb-2 d-flex align-items-center gap-2">
                    <i class="hgi hgi-stroke hgi-link-02 text-primary"></i><span>قائمة روابط الصفحات للتحويل التلقائي</span>
                </h3>
                <p class="text-muted small mb-3">ضع قائمة روابط الصفحات (رابط واحد في كل سطر). ستقوم الأداة بقراءة محتوى كل صفحة تلقائياً، وتنسيقه بهوية DGA، وتغيير التخطيط، وحفظها ونشرها فورياً.</p>
                
                <div class="mb-3">
                    <asp:TextBox ID="txtBulkUrls" runat="server" TextMode="MultiLine" Rows="10" 
                        CssClass="form-control font-monospace text-start" 
                        placeholder="https://pnu.edu.sa/en/Faculties/AD/Pages/CADBusiness.aspx&#10;/en/Deanship/Devandskilldean/Pages/StandardizedTests.aspx&#10;/ar/RegAdm/PGD/Pages/HumanitarianPro3.aspx"></asp:TextBox>
                </div>

                <div class="d-flex align-items-center justify-content-between flex-wrap gap-2">
                    <asp:Button ID="btnConvertBulkUrls" runat="server" Text="بدء القراءة والتحويل والنشر التلقائي لجميع الروابط" 
                        CssClass="btn btn-primary px-4 py-2 fw-bold" OnClick="btnConvertBulkUrls_Click" />
                    <span class="text-muted small">يدعم معالجة عشرات ومئات الروابط في عملية واحدة.</span>
                </div>
            </div>
        </div>

        <!-- TAB 2: SINGLE URL INSPECT & PREVIEW -->
        <div class="tab-pane fade" id="tab-single-url" role="tabpanel">
            <div class="card border shadow-sm rounded-3 p-4 mb-4">
                <h3 class="h5 fw-bold text-dark mb-2 d-flex align-items-center gap-2">
                    <i class="hgi hgi-stroke hgi-file-search text-primary"></i><span>فحص ومعاينة وتحديث صفحة مفردة</span>
                </h3>
                <p class="text-muted small mb-3">أدخل رابط الصفحة لقراءة محتواها الحالي ومعاينته جنباً إلى جنب مع الشكل المحول بهوية DGA قبل الحفظ.</p>
                
                <div class="input-group mb-3" dir="ltr">
                    <asp:TextBox ID="txtSingleUrl" runat="server" CssClass="form-control font-monospace text-start" 
                        placeholder="https://pnu.edu.sa/en/Deanship/Devandskilldean/Pages/StandardizedTests.aspx"></asp:TextBox>
                    <asp:Button ID="btnInspectSingleUrl" runat="server" Text="فحص ومعاينة المحتوى" 
                        CssClass="btn btn-outline-primary px-4" OnClick="btnInspectSingleUrl_Click" />
                </div>

                <asp:Panel ID="pnlPreview" runat="server" Visible="false" CssClass="mt-4 pt-3 border-top">
                    <div class="row g-4">
                        <div class="col-12 col-lg-6">
                            <div class="card p-3 border rounded-3 bg-light h-100">
                                <h4 class="h6 fw-bold text-muted mb-2"><i class="hgi hgi-stroke hgi-document-text me-1"></i>المحتوى الحالي المسترجع من الصفحة:</h4>
                                <div class="p-3 bg-white border rounded-2 text-start overflow-auto" style="max-height: 400px;" dir="ltr">
                                    <asp:Literal ID="litOriginalContent" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6">
                            <div class="card p-3 border rounded-3 bg-primary-25 h-100">
                                <h4 class="h6 fw-bold text-primary mb-2"><i class="hgi hgi-stroke hgi-sparkles me-1"></i>معاينة المحتوى المحول بتصميم DGA:</h4>
                                <div class="p-3 bg-white border rounded-2 text-start overflow-auto" style="max-height: 400px;" dir="ltr">
                                    <asp:Literal ID="litDgaPreview" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mt-4 text-center">
                        <asp:Button ID="btnApplySingleUrl" runat="server" Text="تطبيق التحويل وحفظ ونشر الصفحة الآن" 
                            CssClass="btn btn-success px-5 py-2 fw-bold" OnClick="btnApplySingleUrl_Click" />
                    </div>
                </asp:Panel>
            </div>
        </div>

        <!-- TAB 3: SUBSITE SCANNER -->
        <div class="tab-pane fade" id="tab-subsite-scan" role="tabpanel">
            <div class="card border shadow-sm rounded-3 p-4 mb-4">
                <h3 class="h5 fw-bold text-dark mb-2 d-flex align-items-center gap-2">
                    <i class="hgi hgi-stroke hgi-folder-library text-primary"></i><span>مسح صفحات الموقع الفرعي (Subsite Pages Scanner)</span>
                </h3>
                <p class="text-muted small mb-3">استكشاف كافة صفحات النشر (Publishing Pages) في مكتبة Pages لموقع فرعي معين لتحويلها دفعة واحدة.</p>
                
                <div class="input-group mb-3" dir="ltr">
                    <asp:TextBox ID="txtSubsiteUrl" runat="server" CssClass="form-control font-monospace text-start" 
                        placeholder="/en/Faculties/AD"></asp:TextBox>
                    <asp:Button ID="btnScanSubsite" runat="server" Text="استعراض كافة صفحات الموقع" 
                        CssClass="btn btn-outline-primary px-4" OnClick="btnScanSubsite_Click" />
                </div>

                <asp:Panel ID="pnlSubsiteResults" runat="server" Visible="false" CssClass="mt-3">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <span class="fw-bold text-dark">الصفحات المكتشفة في الموقع:</span>
                        <asp:Button ID="btnConvertScannedPages" runat="server" Text="تحويل الصفحات المحددة إلى DGA" 
                            CssClass="btn btn-primary btn-sm px-3" OnClick="btnConvertScannedPages_Click" />
                    </div>
                    <div class="table-responsive border rounded-3 bg-white">
                        <asp:CheckBoxList ID="cblSubsitePages" runat="server" CssClass="table table-hover mb-0" RepeatLayout="UnorderedList"></asp:CheckBoxList>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <!-- Processing Logs and Results Panel -->
    <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="card border shadow-sm rounded-3 p-4 mb-4">
        <div class="d-flex align-items-center justify-content-between flex-wrap gap-2 mb-3">
            <h3 class="h5 fw-bold text-dark mb-0 d-flex align-items-center gap-2">
                <i class="hgi hgi-stroke hgi-chart-bubble text-primary"></i><span>نتائج المعالجة وسجل العمليات</span>
            </h3>
            <div class="d-flex gap-2">
                <asp:Label ID="lblSuccessCount" runat="server" CssClass="badge bg-success px-3 py-2 fs-6" Text="الناجحة: 0"></asp:Label>
                <asp:Label ID="lblFailCount" runat="server" CssClass="badge bg-danger px-3 py-2 fs-6" Text="الفاشلة: 0"></asp:Label>
            </div>
        </div>
        <div class="table-responsive border rounded-3 bg-white" style="max-height: 400px; overflow-y: auto;">
            <asp:Literal ID="litLogs" runat="server"></asp:Literal>
        </div>
    </asp:Panel>
</div>
