<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUpdatePageLayoutAndContent.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucUpdatePageLayoutAndContent" %>

<div class="card shadow-sm border-0 rounded-3 p-4 my-4" dir="rtl">
    <div class="d-flex align-items-center justify-content-between border-bottom pb-3 mb-4">
        <div class="d-flex align-items-center gap-3">
            <div class="icon-container bg-primary-25 text-primary p-3 rounded-circle">
               <i class="hgi hgi-stroke hgi-refresh fs-3 text-primary"></i>
            </div>
            <div>
                <h2 class="h4 fw-bold text-dark mb-1">أداة تحديث تخطيط الصفحات والمحتوى الرقمي (DGA Page Migrator)</h2>
                <p class="text-muted small mb-0">تغيير PageLayout إلى DGANewBlankWebPartPage، تحديث محتوى الصفحة (PublishingPageContent)، التحقق (CheckIn)، والنشر الفوري.</p>
            </div>
        </div>
        <span class="badge bg-primary text-white px-3 py-2 fs-6">SharePoint Publishing Engine</span>
    </div>

    <!-- Tabs Navigation -->
    <ul class="nav nav-tabs mb-4" id="migratorTabs" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active fw-semibold" id="predefined-tab" data-bs-toggle="tab" data-bs-target="#predefined-pane" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-layers-01 me-1"></i> البرامج المجهزة مسبقاً (12 صفحة)
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-semibold" id="bulk-tab" data-bs-toggle="tab" data-bs-target="#bulk-pane" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-file-import me-1"></i> معالجة مجمعة من ملف / نص (Bulk Importer)
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-semibold" id="single-tab" data-bs-toggle="tab" data-bs-target="#single-pane" type="button" role="tab">
                <i class="hgi hgi-stroke hgi-edit-02 me-1"></i> تحديث صفحة فردية (Single Page)
            </button>
        </li>
    </ul>

    <!-- Options Section -->
    <div class="p-3 bg-light rounded-3 mb-4 border">
        <h3 class="h6 fw-bold text-dark mb-2">خيارات المعالجة والنشر الإلزامية:</h3>
        <div class="d-flex flex-wrap gap-4">
            <div class="form-check">
                <asp:CheckBox ID="chkAutoCheckOut" runat="server" Checked="true" Text="عمل CheckOut تلقائي إذا كانت الصفحة غير محجوزة" CssClass="form-check-label text-dark small" />
            </div>
            <div class="form-check">
                <asp:CheckBox ID="chkMajorCheckIn" runat="server" Checked="true" Text="عمل Major Check-in بإصدار رئيسي" CssClass="form-check-label text-dark small" />
            </div>
            <div class="form-check">
                <asp:CheckBox ID="chkAutoPublish" runat="server" Checked="true" Text="نشر الصفحة تلقائياً (Publish)" CssClass="form-check-label text-dark small" />
            </div>
            <div class="form-check">
                <asp:CheckBox ID="chkAutoApprove" runat="server" Checked="true" Text="اعتماد الصفحة تلقائياً (Approve)" CssClass="form-check-label text-dark small" />
            </div>
        </div>
    </div>

    <!-- Tab Contents -->
    <div class="tab-content" id="migratorTabContent">
        
        <!-- TAB 1: PREDEFINED PGD PROGRAMS -->
        <div class="tab-pane fade show active" id="predefined-pane" role="tabpanel">
            <div class="alert alert-info py-2 px-3 small mb-3">
                <i class="hgi hgi-stroke hgi-information-circle me-1"></i>
                تم تجهيز المحتوى الكامل وتصاميم DGA للبرامج الستة (عربياً وإنجليزياً). حدد البرامج المطلوبة واضغط على بدء التنفيذ.
            </div>

            <div class="table-responsive border rounded-3 mb-3">
                <asp:CheckBoxList ID="cblPredefinedPages" runat="server" CssClass="table table-hover mb-0 align-middle" RepeatLayout="Table">
                </asp:CheckBoxList>
            </div>

            <div class="d-flex gap-2">
                <asp:Button ID="btnExecutePredefined" runat="server" Text="بدء تحديث ونشر البرامج المحددة" CssClass="btn btn-primary px-4 py-2 fw-semibold" OnClick="btnExecutePredefined_Click" />
                <button type="button" class="btn btn-outline-secondary" onclick="toggleSelectAllPredefined(true)">تحديد الكل</button>
                <button type="button" class="btn btn-outline-secondary" onclick="toggleSelectAllPredefined(false)">إلغاء التحديد</button>
            </div>
        </div>

        <!-- TAB 2: BULK TEXT / FILE IMPORTER -->
        <div class="tab-pane fade" id="bulk-pane" role="tabpanel">
            <div class="alert alert-secondary py-2 px-3 small mb-3">
                <strong>تنسيق الإدخال المدعوم:</strong> يمكنك لصق كتل الصفحات باستخدام المحدد <code>===PAGE===</code> أو مصفوفة JSON تحتوي على الحقول (<code>Url</code>, <code>Title</code>, <code>PageLayoutUrl</code>, <code>ContentHtml</code>).
            </div>

            <div class="mb-3">
                <label class="form-label fw-semibold text-dark small">قراءة من مسار ملف على الخادم (اختياري):</label>
                <asp:TextBox ID="txtServerFilePath" runat="server" CssClass="form-control font-monospace text-start" placeholder="C:\path\to\pages_data.txt" />
            </div>

            <div class="mb-3">
                <label class="form-label fw-semibold text-dark small">أو الصق محتوى الصفحات مباشرة هنا:</label>
                <asp:TextBox ID="txtBulkData" runat="server" TextMode="MultiLine" Rows="12" CssClass="form-control font-monospace text-start" placeholder="===PAGE===&#10;URL: /ar/RegAdm/PGD/Pages/HumanitarianPro3.aspx&#10;LAYOUT: /_catalogs/masterpage/DGANewBlankWebPartPage.aspx&#10;TITLE: الدبلوم العالي في التعلم الإلكتروني&#10;CONTENT:&#10;<div dir=&quot;rtl&quot; lang=&quot;ar-SA&quot; ...>...</div>&#10;===END===" />
            </div>

            <asp:Button ID="btnExecuteBulk" runat="server" Text="معالجة وتحديث القائمة المجمعة" CssClass="btn btn-success px-4 py-2 fw-semibold" OnClick="btnExecuteBulk_Click" />
        </div>

        <!-- TAB 3: SINGLE PAGE QUICK UPDATER -->
        <div class="tab-pane fade" id="single-pane" role="tabpanel">
            <div class="row g-3 mb-3">
                <div class="col-12 col-md-6">
                    <label class="form-label fw-semibold text-dark small">رابط الصفحة (Page URL):</label>
                    <asp:TextBox ID="txtSingleUrl" runat="server" CssClass="form-control font-monospace text-start" placeholder="/ar/RegAdm/PGD/Pages/HumanitarianPro3.aspx" />
                </div>
                <div class="col-12 col-md-6">
                    <label class="form-label fw-semibold text-dark small">تخطيط الصفحة (Target Page Layout):</label>
                    <asp:TextBox ID="txtSingleLayout" runat="server" CssClass="form-control font-monospace text-start" Text="/_catalogs/masterpage/DGANewBlankWebPartPage.aspx" />
                </div>
                <div class="col-12">
                    <label class="form-label fw-semibold text-dark small">عنوان الصفحة (Title - اختياري):</label>
                    <asp:TextBox ID="txtSingleTitle" runat="server" CssClass="form-control" placeholder="عنوان الصفحة" />
                </div>
                <div class="col-12">
                    <label class="form-label fw-semibold text-dark small">محتوى الصفحة الجديد (PublishingPageContent HTML):</label>
                    <asp:TextBox ID="txtSingleContent" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control font-monospace text-start" placeholder="<div dir=&quot;rtl&quot; lang=&quot;ar-SA&quot; class=&quot;d-flex flex-column gap-5 text-start&quot;>...</div>" />
                </div>
            </div>

            <asp:Button ID="btnExecuteSingle" runat="server" Text="تحديث ونشر الصفحة الآن" CssClass="btn btn-primary px-4 py-2 fw-semibold" OnClick="btnExecuteSingle_Click" />
        </div>

    </div>

    <!-- Results / Log Panel -->
    <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="mt-4 pt-4 border-top">
        <h3 class="h5 fw-bold text-dark mb-3">تقرير ونتائج المعالجة:</h3>
        <asp:Literal ID="litSummaryCard" runat="server" />
        <div class="border rounded-3 p-3 bg-light font-monospace small" style="max-height: 400px; overflow-y: auto;">
            <asp:Literal ID="litDetailedLog" runat="server" />
        </div>
    </asp:Panel>
</div>

<script type="text/javascript">
    function toggleSelectAllPredefined(check) {
        var container = document.getElementById('<%= cblPredefinedPages.ClientID %>');
        if (container) {
            var checkboxes = container.getElementsByTagName('input');
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type === 'checkbox') {
                    checkboxes[i].checked = check;
                }
            }
        }
    }
</script>
