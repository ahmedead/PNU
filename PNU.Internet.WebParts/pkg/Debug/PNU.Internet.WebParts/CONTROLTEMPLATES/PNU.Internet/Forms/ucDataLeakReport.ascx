<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDataLeakReport.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms.ucDataLeakReport" %>


<script src="https://www.google.com/recaptcha/api.js" async defer></script>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
<script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
<script src="https://npmcdn.com/flatpickr/dist/l10n/ar.js"></script>

<style type="text/css">
    /* ==========================================================================
       DGA Unified Design System — Forms & Inputs
       Baseline: DGA_NewDesign/DGA_Forms_Inputs_Baseline.md
       Source of truth: /Style Library/DGA/public/css/style.min.css (loaded by the
       DGA master pages). This block deliberately does NOT restyle .form-control,
       .form-label, .form-text or .invalid-feedback — those come from the design
       system. It only (a) rebinds the primary token to the PNU palette and
       (b) adds the composites the design system does not ship (stepper, dropzone).
       ========================================================================== */

    .data-leak-wrapper {
        /* PNU brand rebind of the DGA primary ramp. Every rule below consumes the
           token, so retheming the form is a one-line change. */
        --dga-primary: #007a87;
        --dga-primary-800: #00646f;
        --dga-primary-900: #005059;
        --dga-primary-100: rgba(0, 122, 135, .10);
        --dga-primary-200: rgba(0, 122, 135, .18);

        font-family: "IBM Plex Sans Arabic", "Segoe UI", Tahoma, -apple-system, BlinkMacSystemFont, sans-serif;
        color: var(--dga-body-color, #0d121c);
        direction: rtl;
        text-align: right;
    }

    .data-leak-wrapper h2,
    .data-leak-wrapper h3 {
        font-family: inherit;
        font-weight: 700;
        color: var(--dga-gray-950, #0d121c);
    }

    .data-leak-card {
        background: var(--dga-body-bg, #fff);
        border-radius: var(--dga-border-radius-lg, .75rem);
        box-shadow: 0 10px 30px rgba(0, 0, 0, .05);
        border: 1px solid var(--dga-gray-200, #eef2f6);
    }

    /* ---------- Header ---------- */
    .data-leak-header-icon {
        width: 54px;
        height: 54px;
        border-radius: var(--dga-border-radius, .5rem);
        background: var(--dga-primary-100);
        color: var(--dga-primary);
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-size: 1.65rem;
        flex-shrink: 0;
    }

    /* ---------- Steps (DGA "steps" component) ---------- */
    .dga-stepper {
        list-style: none;
        display: flex;
        align-items: flex-start;
        justify-content: space-between;
        max-width: 650px;
        margin: 1.5rem auto 0;
        padding: 0;
    }

    .dga-stepper .dga-step-item {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: .5rem;
        position: relative;
        text-align: center;
    }

    /* Connector drawn from the item itself so the list stays semantic (<ol><li>). */
    .dga-stepper .dga-step-item + .dga-step-item::before {
        content: "";
        position: absolute;
        top: 18px;
        inset-inline-end: 50%;
        width: 100%;
        height: 2px;
        background-color: var(--dga-gray-200, #e2e8f0);
    }

    .dga-stepper .dga-step-item.completed + .dga-step-item::before,
    .dga-stepper .dga-step-item.active + .dga-step-item::before {
        background-color: var(--dga-primary);
    }

    .dga-step-circle {
        position: relative;
        z-index: 1;
        width: 36px;
        height: 36px;
        border-radius: 50%;
        background-color: var(--dga-gray-100, #f1f5f9);
        color: var(--dga-secondary-color, #64748b);
        border: 2px solid var(--dga-border-color, #d2d6db);
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: 700;
        font-size: .95rem;
    }

    .dga-step-item.active .dga-step-circle {
        background-color: var(--dga-primary);
        border-color: var(--dga-primary);
        color: #fff;
        box-shadow: 0 0 0 4px var(--dga-primary-200);
    }

    .dga-step-item.completed .dga-step-circle {
        background-color: var(--dga-primary);
        border-color: var(--dga-primary);
        color: #fff;
    }

    .dga-step-title {
        font-size: .85rem;
        font-weight: 600;
        color: var(--dga-secondary-color, #64748b);
    }

    .dga-step-item.active .dga-step-title,
    .dga-step-item.completed .dga-step-title {
        color: var(--dga-primary);
    }

    /* ---------- Confidentiality callout ---------- */
    .data-leak-notice-callout {
        background: var(--dga-primary-100);
        border: 1px solid var(--dga-primary-200);
        border-inline-start: 4px solid var(--dga-primary);
        border-radius: var(--dga-border-radius, .5rem);
        padding: .9rem 1.25rem;
        color: var(--dga-primary-900);
        font-size: .875rem;
        line-height: 1.6;
    }

    .data-leak-notice-callout .notice-icon {
        color: var(--dga-primary);
        font-size: 1.25rem;
        flex-shrink: 0;
    }

    /* ---------- Section shells ---------- */
    .data-leak-section-box {
        background: var(--dga-body-bg, #fff);
        border: 1px solid var(--dga-border-color, #d2d6db);
        border-radius: var(--dga-border-radius, .5rem);
        padding: 1.5rem;
        margin-bottom: 1.5rem;
    }

    .data-leak-section-header {
        border-bottom: 1px solid var(--dga-gray-100, #f1f5f9);
        padding-bottom: .85rem;
        margin-bottom: 1.25rem;
    }

    .data-leak-step-badge {
        width: 30px;
        height: 30px;
        border-radius: 50%;
        background: var(--dga-primary);
        color: #fff;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-size: .875rem;
        font-weight: 700;
        flex-shrink: 0;
    }

    /* Fieldsets must not inherit the browser's default legend/border chrome. */
    .data-leak-fieldset {
        border: 1px solid var(--dga-border-color, #d2d6db);
        border-radius: var(--dga-border-radius, .5rem);
        background-color: var(--dga-gray-25, #f8fafc);
        padding: 1.15rem;
        margin: 0;
        min-width: 0;
    }

    .data-leak-fieldset > legend {
        float: none;
        width: auto;
        padding: 0;
        margin-bottom: .25rem;
        font-size: 1rem;
        line-height: inherit;
        border: 0;
    }

    /* ---------- Radio & Checkbox (DGA "radio" / "checkbox" components) ----------
       RepeatLayout="UnorderedList" gives us <ul><li><input><label> — semantic,
       no layout tables, and grid-friendly. The generated <input> cannot carry
       .form-check-input, so the design-system appearance is reproduced here. */
    .dga-choice-list {
        list-style: none;
        margin: 0;
        padding: 0;
        display: grid;
        grid-template-columns: 1fr;
        gap: .5rem 1.5rem;
    }

    .dga-choice-list.inline {
        display: flex;
        flex-wrap: wrap;
        gap: .5rem 1.5rem;
    }

    @media (min-width: 768px) {
        .dga-choice-list.cols-2 { grid-template-columns: repeat(2, minmax(0, 1fr)); }
        .dga-choice-list.cols-3 { grid-template-columns: repeat(3, minmax(0, 1fr)); }
    }

    .dga-choice-list li {
        display: flex;
        align-items: flex-start;
        /* Keeps the interactive row at an accessible touch height. */
        min-height: 2.25rem;
        padding-block: .25rem;
    }

    .data-leak-wrapper .dga-choice-list input[type="radio"],
    .data-leak-wrapper .dga-choice-list input[type="checkbox"] {
        flex-shrink: 0;
        width: 1.5em;
        height: 1.5em;
        margin: 0;
        margin-inline-end: .625rem;
        appearance: none;
        -webkit-appearance: none;
        background-color: var(--dga-body-bg, #fff);
        background-repeat: no-repeat;
        background-position: center;
        background-size: contain;
        border: 1px solid var(--dga-border-color, #d2d6db);
        cursor: pointer;
        transition: background-color .15s ease-in-out, border-color .15s ease-in-out;
    }

    .data-leak-wrapper .dga-choice-list input[type="radio"] { border-radius: 50%; }
    .data-leak-wrapper .dga-choice-list input[type="checkbox"] { border-radius: .25rem; }

    .data-leak-wrapper .dga-choice-list input[type="radio"]:checked,
    .data-leak-wrapper .dga-choice-list input[type="checkbox"]:checked {
        background-color: var(--dga-primary);
        border-color: var(--dga-primary);
    }

    .data-leak-wrapper .dga-choice-list input[type="checkbox"]:checked {
        background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 20 20'%3e%3cpath fill='none' stroke='%23fff' stroke-linecap='round' stroke-linejoin='round' stroke-width='3' d='m6 10 3 3 6-6'/%3e%3c/svg%3e");
    }

    .data-leak-wrapper .dga-choice-list input[type="radio"]:checked {
        background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='-4 -4 8 8'%3e%3ccircle r='2' fill='%23fff'/%3e%3c/svg%3e");
    }

    .data-leak-wrapper .dga-choice-list input:hover:not(:checked):not(:disabled) {
        border-color: var(--dga-primary);
    }

    /* DGA ships a high-contrast black focus indicator — keep it, do not tint it. */
    .data-leak-wrapper .dga-choice-list input:focus-visible {
        outline: .125rem solid #000;
        outline-offset: 1px;
    }

    .data-leak-wrapper .dga-choice-list label {
        cursor: pointer;
        font-size: .925rem;
        font-weight: 500;
        line-height: 1.5;
        color: var(--dga-body-color, #334155);
        margin-bottom: 0;
        user-select: none;
    }

    /* ---------- "Other" reveal field ----------
       Bootstrap utilities can out-specify the UA's [hidden] rule, so pin it. */
    .data-leak-wrapper [hidden] {
        display: none !important;
    }

    /* ---------- Textarea counter (DGA "textarea" component) ---------- */
    .dga-counter-row {
        display: flex;
        justify-content: space-between;
        align-items: baseline;
        gap: 1rem;
    }

    .dga-char-counter {
        flex-shrink: 0;
        font-variant-numeric: tabular-nums;
    }

    .dga-char-counter.is-near-limit { color: var(--dga-form-invalid-color, #dc3545); }

    /* ---------- File uploader (DGA "file-uploader" component) ---------- */
    .data-leak-upload-box {
        background: var(--dga-gray-25, #f8fafc);
        border: 2px dashed var(--dga-border-color, #d2d6db);
        border-radius: var(--dga-border-radius, .5rem);
        padding: 1.5rem;
        transition: border-color .2s ease, background .2s ease;
    }

    .data-leak-upload-box:hover,
    .data-leak-upload-box:focus-within {
        border-color: var(--dga-primary);
        background: var(--dga-primary-100);
    }

    .upload-icon-badge {
        width: 44px;
        height: 44px;
        border-radius: var(--dga-border-radius, .5rem);
        background: var(--dga-primary-100);
        color: var(--dga-primary);
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-size: 1.35rem;
        flex-shrink: 0;
    }

    .dga-file-list {
        list-style: none;
        margin: .75rem 0 0;
        padding: 0;
        display: flex;
        flex-direction: column;
        gap: .375rem;
    }

    .dga-file-list li {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: .75rem;
        font-size: .85rem;
        background: var(--dga-body-bg, #fff);
        border: 1px solid var(--dga-border-color, #d2d6db);
        border-radius: .25rem;
        padding: .4rem .65rem;
    }

    .dga-file-list .file-size {
        color: var(--dga-secondary-color, #64748b);
        flex-shrink: 0;
        font-variant-numeric: tabular-nums;
    }

    .dga-file-list li.is-rejected {
        border-color: var(--dga-form-invalid-border-color, #dc3545);
        color: var(--dga-form-invalid-color, #dc3545);
    }

    /* ---------- Validation ----------
       ASP.NET validators toggle an inline `display:inline`, which would defeat
       .invalid-feedback's block layout. Promote it back without touching the
       design-system rule itself. */
    .data-leak-wrapper .invalid-feedback[style*="display: inline"],
    .data-leak-wrapper .invalid-feedback[style*="display:inline"] {
        display: block !important;
    }

    /* Mirror the design system's invalid affordance onto server-rendered inputs. */
    .data-leak-wrapper .form-control.is-invalid,
    .data-leak-wrapper .form-select.is-invalid {
        border-color: var(--dga-form-invalid-border-color, #dc3545);
    }

    .data-leak-wrapper fieldset.is-invalid {
        border-color: var(--dga-form-invalid-border-color, #dc3545);
    }

    /* ---------- Primary action ---------- */
    .btn-submit-pnu {
        background-color: var(--dga-primary);
        border: 1px solid var(--dga-primary);
        color: #fff !important;
        font-weight: 700;
        padding: .85rem 3.5rem;
        border-radius: var(--dga-border-radius, .5rem);
        font-size: 1.05rem;
        cursor: pointer;
        transition: background-color .2s ease-in-out;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        gap: .5rem;
    }

    .btn-submit-pnu:hover {
        background-color: var(--dga-primary-800);
        border-color: var(--dga-primary-800);
        color: #fff !important;
    }

    .btn-submit-pnu:active {
        background-color: var(--dga-primary-900);
        border-color: var(--dga-primary-900);
    }

    .btn-submit-pnu:focus-visible {
        outline: .125rem solid #000;
        outline-offset: 2px;
    }

    /* ---------- Success panel ---------- */
    .data-leak-success-icon {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        background: #ecfdf5;
        color: #10b981;
        font-size: 2.75rem;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        margin: 0 auto 1.5rem;
        border: 2px solid #a7f3d0;
    }

    /* ---------- Datepicker theme (DGA "datepicker" component) ---------- */
    .flatpickr-calendar {
        font-family: "IBM Plex Sans Arabic", "Segoe UI", Tahoma, sans-serif !important;
        border-radius: var(--dga-border-radius, .5rem) !important;
        box-shadow: 0 10px 30px rgba(0, 0, 0, .12) !important;
        border: 1px solid var(--dga-border-color, #d2d6db) !important;
        direction: rtl !important;
    }

    .flatpickr-day.selected,
    .flatpickr-day.selected:focus,
    .flatpickr-day.selected:hover {
        background: var(--dga-primary) !important;
        border-color: var(--dga-primary) !important;
    }

    .flatpickr-day:hover { background: var(--dga-primary-100) !important; }

    @media (prefers-reduced-motion: reduce) {
        .data-leak-wrapper * { transition: none !important; }
    }
</style>

<div class="data-leak-wrapper py-4" dir="rtl">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 col-xl-10">

                <asp:Label ID="lblException" runat="server" CssClass="alert alert-danger d-block mb-4" Visible="false" />

                <%-- ==================== Success Panel ==================== --%>
                <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
                    <div class="card data-leak-card border-0 p-5 text-center my-4" role="status">
                        <div class="data-leak-success-icon" aria-hidden="true">✓</div>
                        <h3 class="fw-bold mb-2">شكراً لك.. تم تسجيل بلاغك بنجاح</h3>
                        <p class="text-muted lead mb-3">
                            تم استلام تفاصيل البلاغ وسيقوم الفريق المعني بالتعامل معه بأعلى درجات السرية والاهتمام وفق السياسات والأنظمة الوطنية المعتمدة.
                        </p>
                        <asp:Label ID="lblSuccessMessage" runat="server" CssClass="text-success fw-medium d-block mb-4" />
                        <div>
                            <a href="javascript:location.reload();" class="btn btn-primary px-4 py-2">تقديم بلاغ جديد</a>
                        </div>
                    </div>
                </asp:Panel>

                <%-- ==================== Form Panel ==================== --%>
                <asp:Panel ID="pnlData" runat="server">
                    <div class="card data-leak-card border-0 p-4 p-md-5">

                        <!-- Header -->
                        <div class="pb-3 mb-4 border-bottom">
                            <div class="d-flex align-items-center gap-3">
                                <div class="data-leak-header-icon">
                                    <i class="hgi hgi-stroke hgi-shield-security" aria-hidden="true"></i>
                                </div>
                                <div>
                                    <h2 class="fw-bold mb-1 h3">الإبلاغ عن حوادث تسريب البيانات</h2>
                                    <p class="text-muted small mb-0">
                                        يهدف هذا النموذج إلى استقبال البلاغات المتعلقة بحوادث التسريب أو الاستخدام غير المصرح به للبيانات الشخصية لحماية خصوصية منسوبي الجامعة والحد من المخاطر.
                                    </p>
                                </div>
                            </div>

                            <div class="data-leak-notice-callout d-flex align-items-start gap-3 mt-3">
                                <div class="notice-icon" aria-hidden="true">
                                    <i class="hgi hgi-stroke hgi-security-check"></i>
                                </div>
                                <div>
                                    <strong>الالتزام بالسرية التامة:</strong>
                                    <span>سيتم التعامل مع جميع البلاغات والمعلومات المدخلة بسرية وموثوقية عالية وفق الأنظمة والسياسات الوطنية المعتمدة لحماية البيانات الشخصية.</span>
                                </div>
                            </div>

                            <%-- DGA Steps component --%>
                            <ol class="dga-stepper" aria-label="مراحل تقديم البلاغ">
                                <li class="dga-step-item active" aria-current="step">
                                    <span class="dga-step-circle">1</span>
                                    <span class="dga-step-title">بيانات مُقدم البلاغ</span>
                                </li>
                                <li class="dga-step-item active">
                                    <span class="dga-step-circle">2</span>
                                    <span class="dga-step-title">تفاصيل حادثة التسريب</span>
                                </li>
                                <li class="dga-step-item">
                                    <span class="dga-step-circle">3</span>
                                    <span class="dga-step-title">الإرسال والمتابعة</span>
                                </li>
                            </ol>
                        </div>

                        <%-- ==================== Section 1: Reporter data ==================== --%>
                        <div class="data-leak-section-box">
                            <div class="data-leak-section-header d-flex align-items-center gap-2">
                                <span class="data-leak-step-badge" aria-hidden="true">1</span>
                                <h3 class="h5 fw-bold mb-0">بيانات مُقدم البلاغ</h3>
                            </div>

                            <div class="row g-3">
                                <%-- Text input --%>
                                <div class="col-12 col-md-4">
                                    <label for="<%= txtFullName.ClientID %>" class="form-label text-sm-semibold required">الاسم الكامل</label>
                                    <div class="form-control-container">
                                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="اكتب الاسم الكامل"
                                            aria-required="true" aria-describedby="hintFullName" autocomplete="name" />
                                    </div>
                                    <div id="hintFullName" class="form-text">الاسم الثلاثي أو الرباعي لمقدم البلاغ</div>
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                        ValidationGroup="DataLeak" ErrorMessage="الاسم الكامل مطلوب" CssClass="invalid-feedback" Display="Dynamic" />
                                </div>

                                <%-- Telephone input --%>
                                <div class="col-12 col-md-4">
                                    <label for="<%= txtMobile.ClientID %>" class="form-label text-sm-semibold required">رقم الجوال</label>
                                    <div class="form-control-container">
                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" TextMode="Phone" placeholder="05xxxxxxxx"
                                            aria-required="true" aria-describedby="hintMobile"
                                            inputmode="tel" autocomplete="tel" maxlength="15" />
                                    </div>
                                    <div id="hintMobile" class="form-text">مثال: 0512345678</div>
                                    <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile"
                                        ValidationGroup="DataLeak" ErrorMessage="رقم الجوال مطلوب" CssClass="invalid-feedback" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revMobile" runat="server"
                                        ControlToValidate="txtMobile"
                                        ValidationGroup="DataLeak"
                                        ValidationExpression="^[0-9+\-\s()]+$"
                                        ErrorMessage="يرجى إدخال رقم جوال صحيح"
                                        CssClass="invalid-feedback" Display="Dynamic" />
                                </div>

                                <%-- Email input --%>
                                <div class="col-12 col-md-4">
                                    <label for="<%= txtEmail.ClientID %>" class="form-label text-sm-semibold required">البريد الإلكتروني</label>
                                    <div class="form-control-container">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="name@example.com"
                                            aria-required="true" aria-describedby="hintEmail"
                                            inputmode="email" autocomplete="email" dir="ltr" />
                                    </div>
                                    <div id="hintEmail" class="form-text">سيُستخدم للتواصل بشأن مستجدات البلاغ</div>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        ValidationGroup="DataLeak" ErrorMessage="البريد الإلكتروني مطلوب" CssClass="invalid-feedback" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ValidationGroup="DataLeak" ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                                        ErrorMessage="صيغة البريد الإلكتروني غير صحيحة" CssClass="invalid-feedback" Display="Dynamic" />
                                </div>

                                <%-- Radio group: a grouped choice needs fieldset/legend, not a bare label. --%>
                                <div class="col-12 mt-3">
                                    <fieldset class="data-leak-fieldset">
                                        <legend class="form-label text-sm-semibold required">الصفة</legend>
                                        <asp:RadioButtonList ID="rblCapacity" runat="server" RepeatLayout="UnorderedList"
                                            CssClass="dga-choice-list cols-3">
                                            <asp:ListItem Text="طالب" Value="طالب" />
                                            <asp:ListItem Text="موظف" Value="موظف" />
                                            <asp:ListItem Text="موظف سابق" Value="موظف سابق" />
                                            <asp:ListItem Text="جهة خارجية" Value="جهة خارجية" />
                                            <asp:ListItem Text="متعاقد / مزود خدمة" Value="متعاقد / مزود خدمة" />
                                            <asp:ListItem Text="أخرى" Value="أخرى" />
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="rfvCapacity" runat="server" ControlToValidate="rblCapacity"
                                            ValidationGroup="DataLeak" ErrorMessage="يرجى اختيار الصفة" CssClass="invalid-feedback" Display="Dynamic" />

                                        <div class="mt-3" id="wrapCapacityOther" hidden="hidden">
                                            <label for="<%= txtCapacityOther.ClientID %>" class="form-label text-sm-semibold">توضيح الصفة</label>
                                            <div class="form-control-container">
                                                <asp:TextBox ID="txtCapacityOther" runat="server" CssClass="form-control other-input"
                                                    placeholder="اكتب التوضيح هنا" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>

                        <%-- ==================== Section 2: Incident details ==================== --%>
                        <div class="data-leak-section-box">
                            <div class="data-leak-section-header d-flex align-items-center gap-2">
                                <span class="data-leak-step-badge" aria-hidden="true">2</span>
                                <h3 class="h5 fw-bold mb-0">تفاصيل حادثة تسريب البيانات الشخصية</h3>
                            </div>

                            <div class="row g-4">
                                <%-- Datepicker --%>
                                <div class="col-12 col-md-6">
                                    <label for="<%= txtDiscoveryDate.ClientID %>" class="form-label text-sm-semibold required">تاريخ اكتشاف التسريب</label>
                                    <div class="form-control-container has-icon">
                                        <span class="d-inline-flex fs-6 form-input-icon text-body-secondary" aria-hidden="true">
                                            <i class="hgi hgi-stroke hgi-calendar-03"></i>
                                        </span>
                                        <asp:TextBox ID="txtDiscoveryDate" runat="server" CssClass="form-control gregorian-date"
                                            placeholder="2026-05-14" autocomplete="off"
                                            aria-required="true" aria-describedby="hintDate" />
                                    </div>
                                    <div id="hintDate" class="form-text">التاريخ الميلادي بصيغة yyyy-MM-dd، ولا يمكن اختيار تاريخ مستقبلي.</div>
                                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDiscoveryDate"
                                        ValidationGroup="DataLeak" ErrorMessage="تاريخ اكتشاف التسريب مطلوب" CssClass="invalid-feedback" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revDate" runat="server" ControlToValidate="txtDiscoveryDate"
                                        ValidationGroup="DataLeak"
                                        ValidationExpression="^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$"
                                        ErrorMessage="صيغة التاريخ غير صحيحة (yyyy-MM-dd)" CssClass="invalid-feedback" Display="Dynamic" />
                                </div>

                                <%-- Radio group --%>
                                <div class="col-12 col-md-6">
                                    <fieldset class="data-leak-fieldset h-100">
                                        <legend class="form-label text-sm-semibold required">هل التسريب ما زال قائماً وقت الإبلاغ؟</legend>
                                        <asp:RadioButtonList ID="rblStillActive" runat="server" RepeatLayout="UnorderedList"
                                            CssClass="dga-choice-list inline">
                                            <asp:ListItem Text="نعم" Value="نعم" />
                                            <asp:ListItem Text="لا" Value="لا" />
                                            <asp:ListItem Text="غير متأكد" Value="غير متأكد" />
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="rfvStillActive" runat="server" ControlToValidate="rblStillActive"
                                            ValidationGroup="DataLeak" ErrorMessage="يرجى تحديد ما إذا كان التسريب قائماً" CssClass="invalid-feedback" Display="Dynamic" />
                                    </fieldset>
                                </div>

                                <%-- Checkbox group --%>
                                <div class="col-12">
                                    <fieldset class="data-leak-fieldset">
                                        <legend class="form-label text-sm-semibold required">نوع مصدر تسريب البيانات</legend>
                                        <div class="form-text mb-2">يمكن اختيار أكثر من خيار.</div>
                                        <asp:CheckBoxList ID="cblSourceType" runat="server" RepeatLayout="UnorderedList"
                                            CssClass="dga-choice-list cols-3">
                                            <asp:ListItem Text="موقع الكتروني" Value="موقع الكتروني" />
                                            <asp:ListItem Text="بريد الكتروني" Value="بريد الكتروني" />
                                            <asp:ListItem Text="نظام / منصة الكترونية" Value="نظام / منصة الكترونية" />
                                            <asp:ListItem Text="وسائط تخزين" Value="وسائط تخزين" />
                                            <asp:ListItem Text="أخرى" Value="أخرى" />
                                        </asp:CheckBoxList>
                                        <div class="mt-3" id="wrapSourceTypeOther" hidden="hidden">
                                            <label for="<%= txtSourceTypeOther.ClientID %>" class="form-label text-sm-semibold">توضيح نوع المصدر</label>
                                            <div class="form-control-container">
                                                <asp:TextBox ID="txtSourceTypeOther" runat="server" CssClass="form-control other-input"
                                                    placeholder="اكتب التوضيح هنا" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>

                                <%-- Text input --%>
                                <div class="col-12">
                                    <label for="<%= txtSourceName.ClientID %>" class="form-label text-sm-semibold required">اسم مصدر التسريب والرابط إن أمكن</label>
                                    <div class="form-control-container">
                                        <asp:TextBox ID="txtSourceName" runat="server" CssClass="form-control"
                                            placeholder="اكتب اسم الموقع أو النظام أو المنصة أو عنوان الرابط"
                                            aria-required="true" aria-describedby="hintSourceName" />
                                    </div>
                                    <div id="hintSourceName" class="form-text">مثال: موقع البوابة، رابط الخدمة، أو الحساب المعني</div>
                                    <asp:RequiredFieldValidator ID="rfvSourceName" runat="server" ControlToValidate="txtSourceName"
                                        ValidationGroup="DataLeak" ErrorMessage="اسم مصدر التسريب مطلوب" CssClass="invalid-feedback" Display="Dynamic" />
                                </div>

                                <%-- Checkbox group --%>
                                <div class="col-12">
                                    <fieldset class="data-leak-fieldset">
                                        <legend class="form-label text-sm-semibold required">نوع البيانات الشخصية المتأثرة</legend>
                                        <div class="form-text mb-2">يمكن اختيار أكثر من خيار.</div>
                                        <asp:CheckBoxList ID="cblDataType" runat="server" RepeatLayout="UnorderedList"
                                            CssClass="dga-choice-list cols-2">
                                            <asp:ListItem Text="اسم" Value="اسم" />
                                            <asp:ListItem Text="رقم الهوية / الرقم الجامعي" Value="رقم الهوية / الرقم الجامعي" />
                                            <asp:ListItem Text="تاريخ الميلاد" Value="تاريخ الميلاد" />
                                            <asp:ListItem Text="البريد الإلكتروني" Value="البريد الإلكتروني" />
                                            <asp:ListItem Text="رقم الهاتف" Value="رقم الهاتف" />
                                            <asp:ListItem Text="بيانات أكاديمية (سجلات، نتائج، مقررات)" Value="بيانات أكاديمية (سجلات، نتائج، مقررات)" />
                                            <asp:ListItem Text="بيانات مالية" Value="بيانات مالية" />
                                            <asp:ListItem Text="بيانات صحية" Value="بيانات صحية" />
                                            <asp:ListItem Text="بيانات وظيفية" Value="بيانات وظيفية" />
                                            <asp:ListItem Text="أخرى" Value="أخرى" />
                                        </asp:CheckBoxList>
                                        <div class="mt-3" id="wrapDataTypeOther" hidden="hidden">
                                            <label for="<%= txtDataTypeOther.ClientID %>" class="form-label text-sm-semibold">توضيح نوع البيانات</label>
                                            <div class="form-control-container">
                                                <asp:TextBox ID="txtDataTypeOther" runat="server" CssClass="form-control other-input"
                                                    placeholder="اكتب التوضيح هنا" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>

                                <%-- Checkbox group --%>
                                <div class="col-12">
                                    <fieldset class="data-leak-fieldset">
                                        <legend class="form-label text-sm-semibold required">صيغة البيانات التي تم تسريبها</legend>
                                        <div class="form-text mb-2">يمكن اختيار أكثر من خيار.</div>
                                        <asp:CheckBoxList ID="cblDataFormat" runat="server" RepeatLayout="UnorderedList"
                                            CssClass="dga-choice-list cols-2">
                                            <asp:ListItem Text="ملفات (Excel – Word – PDF – صور – قواعد بيانات)" Value="ملفات (Excel – Word – PDF – صور – قواعد بيانات)" />
                                            <asp:ListItem Text="نصوص مكتوبة مباشرة" Value="نصوص مكتوبة مباشرة" />
                                            <asp:ListItem Text="نسخ ورقية" Value="نسخ ورقية" />
                                            <asp:ListItem Text="روابط إلكترونية" Value="روابط إلكترونية" />
                                            <asp:ListItem Text="أخرى" Value="أخرى" />
                                        </asp:CheckBoxList>
                                        <div class="mt-3" id="wrapDataFormatOther" hidden="hidden">
                                            <label for="<%= txtDataFormatOther.ClientID %>" class="form-label text-sm-semibold">توضيح صيغة البيانات</label>
                                            <div class="form-control-container">
                                                <asp:TextBox ID="txtDataFormatOther" runat="server" CssClass="form-control other-input"
                                                    placeholder="اكتب التوضيح هنا" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>

                                <%-- Radio group --%>
                                <div class="col-12 col-md-6">
                                    <fieldset class="data-leak-fieldset h-100">
                                        <legend class="form-label text-sm-semibold required">طريقة الوصول إلى البيانات</legend>
                                        <asp:RadioButtonList ID="rblAccessMethod" runat="server" RepeatLayout="UnorderedList"
                                            CssClass="dga-choice-list">
                                            <asp:ListItem Text="متاحة للعامة" Value="متاحة للعامة" />
                                            <asp:ListItem Text="محمية بكلمة مرور" Value="محمية بكلمة مرور" />
                                            <asp:ListItem Text="أُرسلت بالخطأ" Value="أُرسلت بالخطأ" />
                                            <asp:ListItem Text="تم الحصول عليها دون تصريح" Value="تم الحصول عليها دون تصريح" />
                                            <asp:ListItem Text="غير معروف" Value="غير معروف" />
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="rfvAccessMethod" runat="server" ControlToValidate="rblAccessMethod"
                                            ValidationGroup="DataLeak" ErrorMessage="يرجى تحديد طريقة الوصول" CssClass="invalid-feedback" Display="Dynamic" />
                                    </fieldset>
                                </div>

                                <%-- Checkbox group --%>
                                <div class="col-12 col-md-6">
                                    <fieldset class="data-leak-fieldset h-100">
                                        <legend class="form-label text-sm-semibold">هل البيانات تخص</legend>
                                        <div class="form-text mb-2">يمكن اختيار أكثر من خيار.</div>
                                        <asp:CheckBoxList ID="cblDataBelongsTo" runat="server" RepeatLayout="UnorderedList"
                                            CssClass="dga-choice-list">
                                            <asp:ListItem Text="طالب واحد" Value="طالب واحد" />
                                            <asp:ListItem Text="موظف واحد" Value="موظف واحد" />
                                            <asp:ListItem Text="مجموعة طلاب" Value="مجموعة طلاب" />
                                            <asp:ListItem Text="مجموعة موظفين" Value="مجموعة موظفين" />
                                            <asp:ListItem Text="أكثر من فئة" Value="أكثر من فئة" />
                                        </asp:CheckBoxList>
                                    </fieldset>
                                </div>

                                <%-- Number input --%>
                                <div class="col-12 col-md-6">
                                    <label for="<%= txtAffectedCount.ClientID %>" class="form-label text-sm-semibold">عدد الأشخاص المتأثرين</label>
                                    <div class="form-control-container">
                                        <asp:TextBox ID="txtAffectedCount" runat="server" CssClass="form-control" TextMode="Number"
                                            min="1" step="1" inputmode="numeric" placeholder="مثال: 50"
                                            aria-describedby="hintAffectedCount" />
                                    </div>
                                    <div id="hintAffectedCount" class="form-text">عدد تقريبي إن أمكن معرفته — يمكن ترك الحقل فارغاً.</div>
                                </div>

                                <%-- Textarea --%>
                                <div class="col-12">
                                    <label for="<%= txtIncidentDescription.ClientID %>" class="form-label text-sm-semibold required">وصف الحادث</label>
                                    <div class="form-control-container">
                                        <asp:TextBox ID="txtIncidentDescription" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="6" placeholder="اكتب تفاصيل واقعة التسريب هنا..."
                                            aria-required="true" aria-describedby="hintDesc" />
                                    </div>
                                    <div class="dga-counter-row">
                                        <div id="hintDesc" class="form-text">يرجى وصف الواقعة بشكل مفصل لمساعدة الفريق في التحقق السريع.</div>
                                        <div class="form-text dga-char-counter" aria-live="polite">
                                            <span id="descCharCount">0</span> / 4000
                                        </div>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtIncidentDescription"
                                        ValidationGroup="DataLeak" ErrorMessage="وصف الحادث مطلوب" CssClass="invalid-feedback" Display="Dynamic" />
                                </div>

                                <%-- File uploader --%>
                                <div class="col-12">
                                    <div class="data-leak-upload-box">
                                        <div class="d-flex align-items-center gap-3">
                                            <div class="upload-icon-badge" aria-hidden="true">
                                                <i class="hgi hgi-stroke hgi-attachment"></i>
                                            </div>
                                            <div>
                                                <label for="<%= fuAttachment.ClientID %>" class="form-label text-sm-semibold mb-0">المرفقات الداعمة للبلاغ</label>
                                                <div class="form-text">أرفق لقطات شاشة أو مستندات توثق واقعة التسريب. يمكن اختيار عدة ملفات.</div>
                                            </div>
                                        </div>
                                        <div class="mt-3">
                                            <asp:FileUpload ID="fuAttachment" runat="server" CssClass="form-control" AllowMultiple="true"
                                                accept=".png,.jpg,.jpeg,.pdf,.doc,.docx,.xls,.xlsx"
                                                aria-describedby="hintUpload" />
                                        </div>
                                        <div id="hintUpload" class="form-text mt-2">
                                            الصيغ المقبولة: PNG, JPG, PDF, Word, Excel — بحد أقصى 10 ميجابايت لمجموع الملفات.
                                        </div>
                                        <ul id="uploadFileList" class="dga-file-list" aria-live="polite"></ul>
                                        <div id="uploadError" class="invalid-feedback" role="alert"></div>
                                    </div>
                                </div>

                                <div class="col-12">
                                    <div class="d-flex justify-content-center my-2">
                                        <div class="g-recaptcha" data-sitekey="6Leg0vAsAAAAAI9NROAGxiBaSjQD4UMc7eBzL7GL"></div>
                                    </div>
                                </div>

                                <div class="col-12 text-center pt-3">
                                    <asp:Button ID="btnSubmit" runat="server" Text="إرسال البلاغ" CssClass="btn btn-primary"
                                        OnClick="btnSubmit_Click" ValidationGroup="DataLeak" />
                                </div>

                            </div>
                        </div>

                    </div>
                </asp:Panel>

            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    (function () {
        'use strict';

        var OTHER_VALUE = 'أخرى';
        var MAX_DESC = 4000;
        var MAX_UPLOAD_BYTES = 10 * 1024 * 1024;
        var ALLOWED_EXT = ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx', 'xls', 'xlsx'];

        function byId(id) { return document.getElementById(id); }

        // classList.toggle's second argument is unreliable in older engines.
        function setClass(el, name, on) {
            if (!el) return;
            if (on) { el.classList.add(name); } else { el.classList.remove(name); }
        }

        function closestTag(el, tagName) {
            var node = el;
            while (node && node.tagName !== tagName) { node = node.parentNode; }
            return node && node.tagName === tagName ? node : null;
        }

        /* ---------- "Other" reveal fields ---------- */
        function initOtherToggles() {
            var mappings = [
                { groupId: '<%= rblCapacity.ClientID %>', otherInputId: '<%= txtCapacityOther.ClientID %>', wrapId: 'wrapCapacityOther' },
                { groupId: '<%= cblSourceType.ClientID %>', otherInputId: '<%= txtSourceTypeOther.ClientID %>', wrapId: 'wrapSourceTypeOther' },
                { groupId: '<%= cblDataType.ClientID %>', otherInputId: '<%= txtDataTypeOther.ClientID %>', wrapId: 'wrapDataTypeOther' },
                { groupId: '<%= cblDataFormat.ClientID %>', otherInputId: '<%= txtDataFormatOther.ClientID %>', wrapId: 'wrapDataFormatOther' }
            ];

            mappings.forEach(function (mapping) {
                var group = byId(mapping.groupId);
                var input = byId(mapping.otherInputId);
                var wrap = byId(mapping.wrapId);
                if (!group || !input || !wrap) return;

                var choices = group.querySelectorAll('input[type="radio"], input[type="checkbox"]');

                function labelTextFor(el) {
                    var label = document.querySelector('label[for="' + el.id + '"]');
                    return label ? label.textContent.trim() : (el.value || '');
                }

                function sync(focusOnEnable) {
                    var isOther = false;
                    Array.prototype.forEach.call(choices, function (el) {
                        if (el.checked && (labelTextFor(el) === OTHER_VALUE || el.value === OTHER_VALUE)) {
                            isOther = true;
                        }
                    });

                    // Progressive disclosure: reveal the field rather than
                    // disabling it. A server-side disabled control never posts
                    // its value back, and disabled fields are skipped by screen
                    // readers — revealing keeps both the data and the a11y tree
                    // intact.
                    wrap.hidden = !isOther;
                    if (!isOther) {
                        input.value = '';
                    } else if (focusOnEnable) {
                        input.focus();
                    }
                }

                Array.prototype.forEach.call(choices, function (el) {
                    el.addEventListener('change', function () { sync(true); });
                });

                // Re-apply on load so the state survives a postback.
                sync(false);
            });
        }

        /* ---------- Datepicker ---------- */
        function initDatePicker() {
            var dateInput = byId('<%= txtDiscoveryDate.ClientID %>');
            if (!dateInput || typeof flatpickr === 'undefined') return;

            var today = new Date();
            var maxDate = today.getFullYear() + '-' +
                String(today.getMonth() + 1).padStart(2, '0') + '-' +
                String(today.getDate()).padStart(2, '0');

            flatpickr(dateInput, {
                dateFormat: 'Y-m-d',
                maxDate: maxDate,
                allowInput: true,
                locale: 'ar',
                disableMobile: true
            });
        }

        /* ---------- Textarea character counter ---------- */
        function initCharCounter() {
            var textarea = byId('<%= txtIncidentDescription.ClientID %>');
            var counter = byId('descCharCount');
            if (!textarea || !counter) return;

            textarea.setAttribute('maxlength', MAX_DESC);

            function update() {
                var len = textarea.value.length;
                counter.textContent = len;
                setClass(counter.parentNode, 'is-near-limit', len > MAX_DESC * 0.9);
            }

            textarea.addEventListener('input', update);
            update();
        }

        /* ---------- File uploader ----------
           Client-side mirror of the limits stated in the UI. The server still
           owns the authoritative check. */
        function initFileUploader() {
            var fileInput = byId('<%= fuAttachment.ClientID %>');
            var list = byId('uploadFileList');
            var error = byId('uploadError');
            if (!fileInput || !list || !error) return;

            function formatSize(bytes) {
                if (bytes < 1024) return bytes + ' B';
                if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
                return (bytes / (1024 * 1024)).toFixed(2) + ' MB';
            }

            fileInput.addEventListener('change', function () {
                list.innerHTML = '';
                error.style.display = 'none';
                error.textContent = '';

                var files = fileInput.files;
                if (!files || !files.length) return;

                var total = 0;
                var problems = [];

                Array.prototype.forEach.call(files, function (file) {
                    var ext = (file.name.split('.').pop() || '').toLowerCase();
                    var badType = ALLOWED_EXT.indexOf(ext) === -1;
                    total += file.size;

                    var li = document.createElement('li');
                    if (badType) {
                        li.className = 'is-rejected';
                        problems.push('صيغة الملف "' + file.name + '" غير مقبولة.');
                    }

                    var name = document.createElement('span');
                    name.textContent = file.name;
                    var size = document.createElement('span');
                    size.className = 'file-size';
                    size.textContent = formatSize(file.size);

                    li.appendChild(name);
                    li.appendChild(size);
                    list.appendChild(li);
                });

                if (total > MAX_UPLOAD_BYTES) {
                    problems.push('مجموع حجم الملفات (' + formatSize(total) + ') يتجاوز الحد الأقصى 10 ميجابايت.');
                }

                if (problems.length) {
                    error.textContent = problems.join(' ');
                    error.style.display = 'block';
                }
            });
        }

        /* ---------- Bind ASP.NET validation to DGA error states ----------
           The design system shows the red underline via .is-invalid on the field
           (or on the fieldset for grouped choices). ASP.NET only toggles the
           message span, so mirror its verdict onto the control. */
        function initValidationStates() {
            if (typeof window.ValidatorUpdateDisplay !== 'function') return;

            var original = window.ValidatorUpdateDisplay;

            window.ValidatorUpdateDisplay = function (validator) {
                original(validator);
                try {
                    var targetId = validator.controltovalidate;
                    if (!targetId) return;

                    var target = byId(targetId);
                    if (!target) return;

                    // A field can carry several validators; it is invalid if any fail.
                    var invalid = false;
                    var all = window.Page_Validators || [];
                    for (var i = 0; i < all.length; i++) {
                        if (all[i].controltovalidate === targetId && all[i].isvalid === false) {
                            invalid = true;
                            break;
                        }
                    }

                    // RadioButtonList/CheckBoxList render a <ul>; flag its fieldset instead.
                    var host = target.tagName === 'UL' ? closestTag(target, 'FIELDSET') : target;
                    if (!host) host = target;

                    setClass(host, 'is-invalid', invalid);
                    if (target.tagName !== 'UL') {
                        target.setAttribute('aria-invalid', invalid ? 'true' : 'false');
                    }
                } catch (e) { /* never block validation */ }
            };
        }

        function init() {
            initOtherToggles();
            initDatePicker();
            initCharCounter();
            initFileUploader();
            initValidationStates();
        }

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', init);
        } else {
            init();
        }
    })();
</script>

<script type="text/javascript">
    (function () {
        function isFarmOrSystemAccount() {
            if (typeof _spPageContextInfo !== 'undefined') {
                // 1. SharePoint System Account ID is ALWAYS 1073741823 (0x3FFFFFFF)
                if (_spPageContextInfo.userId === 1073741823) {
                    return true;
                }
                // 2. Check systemUserKey (claims format e.g. i:0#.w|domain\sp_farm or sharepoint\system)
                var sysKey = (_spPageContextInfo.systemUserKey || '').toLowerCase();
                if (sysKey.indexOf('system') !== -1 || sysKey.indexOf('farm') !== -1 || sysKey.indexOf('spsvc') !== -1 || sysKey.indexOf('sp_farm') !== -1) {
                    return true;
                }
                // 3. Check userLoginName if populated
                var login = (_spPageContextInfo.userLoginName || '').toLowerCase();
                if (login === 'sharepoint\\system' || login.indexOf('\\system') !== -1 || login.indexOf('farm') !== -1) {
                    return true;
                }
                // 4. Check userDisplayName (e.g. "System Account" or "حساب النظام")
                var disp = (_spPageContextInfo.userDisplayName || '').toLowerCase();
                if (disp.indexOf('system') !== -1 || disp.indexOf('farm') !== -1 || disp.indexOf('النظام') !== -1) {
                    return true;
                }
            }
            return false;
        }

        function ensureCoreCssLoaded() {
            var hasCore = document.querySelector('link[href*="corev15"]');
            if (!hasCore) {
                var lcid = (typeof _spPageContextInfo !== 'undefined' && _spPageContextInfo.currentLanguage) ? _spPageContextInfo.currentLanguage : '1025';
                var link = document.createElement('link');
                link.rel = 'stylesheet';
                link.type = 'text/css';
                link.href = '/_layouts/15/' + lcid + '/styles/Themable/corev15.css';
                document.head.insertBefore(link, document.head.firstChild);
            }
        }

        if (isFarmOrSystemAccount()) {
            ensureCoreCssLoaded();
            return;
        }

        function purgeCoreCss() {
            if (isFarmOrSystemAccount()) {
                return;
            }
            var links = document.querySelectorAll('link[href*="corev15"], link[href*="corev14"]');
            for (var i = 0; i < links.length; i++) {
                links[i].disabled = true;
                if (links[i].parentNode) {
                    links[i].parentNode.removeChild(links[i]);
                }
            }
            if (document.styleSheets) {
                for (var j = 0; j < document.styleSheets.length; j++) {
                    try {
                        var sheet = document.styleSheets[j];
                        if (sheet && sheet.href && (sheet.href.indexOf('corev15') !== -1 || sheet.href.indexOf('corev14') !== -1)) {
                            sheet.disabled = true;
                        }
                    } catch (e) { }
                }
            }
        }

        purgeCoreCss();
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', purgeCoreCss);
        }
        window.addEventListener('load', purgeCoreCss);
        if (window.MutationObserver && document.head) {
            var observer = new MutationObserver(function (mutations) {
                if (isFarmOrSystemAccount()) {
                    observer.disconnect();
                    return;
                }
                mutations.forEach(function (mutation) {
                    for (var k = 0; k < mutation.addedNodes.length; k++) {
                        var node = mutation.addedNodes[k];
                        if (node.tagName === 'LINK' && node.href && (node.href.indexOf('corev15') !== -1 || node.href.indexOf('corev14') !== -1)) {
                            node.disabled = true;
                            if (node.parentNode) node.parentNode.removeChild(node);
                        }
                    }
                });
            });
            observer.observe(document.head, { childList: true, subtree: true });
        }
    })();
</script>
