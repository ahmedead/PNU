<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGenericPageContentEditor.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ContentManagement.ucGenericPageContentEditor" %>

<!-- DGA Fonts & Icons -->
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=IBM+Plex+Sans+Arabic:wght@400;500;600;700&display=swap">
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@hugeicons/core-free-icons@0.2.0/font/hgi-stroke-rounded.css">
<script src="https://cdnjs.cloudflare.com/ajax/libs/tinymce/6.8.2/tinymce.min.js" referrerpolicy="origin"></script>

<style>
    .pnu-editor-box {
        font-family: 'IBM Plex Sans Arabic', -apple-system, BlinkMacSystemFont, sans-serif;
        background: #ffffff;
        border: 1px solid #d1e2db;
        border-radius: 14px;
        padding: 24px;
        direction: rtl;
        text-align: right;
    }
    .pnu-editor-head {
        border-bottom: 2px solid #eef2f0;
        padding-bottom: 18px;
        margin-bottom: 20px;
    }
    .pnu-btn-primary {
        background-color: #006848 !important;
        border-color: #006848 !important;
        color: #ffffff !important;
        font-weight: 600;
        border-radius: 8px;
        padding: 8px 22px;
        transition: all 0.2s;
    }
    .pnu-btn-primary:hover {
        background-color: #004d35 !important;
    }
    .pnu-status-alert {
        border-radius: 10px;
        padding: 14px 18px;
        margin-bottom: 20px;
        display: flex;
        align-items: center;
        gap: 12px;
        font-size: 0.95rem;
        font-weight: 500;
    }
    .alert-success-pnu { background-color: #e6f4ea; color: #006848; border: 1px solid #b7e1cd; }
    .alert-warning-pnu { background-color: #fef7e0; color: #b06000; border: 1px solid #f9e2af; }
    .alert-danger-pnu { background-color: #fce8e6; color: #c5221f; border: 1px solid #f5c2c7; }

    /* Nav Tabs */
    .pnu-nav-tabs {
        display: flex;
        gap: 8px;
        border-bottom: 2px solid #e9ecef;
        margin-bottom: 16px;
        padding-bottom: 0;
    }
    .pnu-tab-btn {
        background: none;
        border: none;
        padding: 10px 18px;
        font-family: inherit;
        font-size: 0.95rem;
        font-weight: 600;
        color: #6c757d;
        border-bottom: 3px solid transparent;
        cursor: pointer;
        transition: all 0.2s;
        display: inline-flex;
        align-items: center;
        gap: 8px;
    }
    .pnu-tab-btn:hover {
        color: #006848;
    }
    .pnu-tab-btn.active {
        color: #006848;
        border-bottom-color: #006848;
    }

    .pnu-source-textarea {
        width: 100%;
        min-height: 520px;
        font-family: 'Consolas', 'Courier New', monospace;
        font-size: 13.5px;
        line-height: 1.5;
        direction: ltr;
        text-align: left;
        background: #1e1e1e;
        color: #d4d4d4;
        border: 1px solid #333;
        border-radius: 8px;
        padding: 16px;
        box-sizing: border-box;
        resize: vertical;
    }

    .pnu-preview-frame {
        width: 100%;
        min-height: 550px;
        background: #ffffff;
        border: 1px solid #d1e2db;
        border-radius: 8px;
        padding: 24px;
        box-sizing: border-box;
        overflow-y: auto;
    }
    
    .tox-tinymce {
        border-radius: 8px !important;
        border-color: #d1e2db !important;
    }
</style>

<div class="pnu-editor-box shadow-sm">
    <!-- Header Controls -->
    <div class="pnu-editor-head d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3">
        <div>
            <h3 class="h5 fw-bold text-dark mb-1 d-flex align-items-center gap-2">
                <i class="hgi hgi-stroke hgi-file-edit text-success fs-4"></i>
                <span>محرر محتوى صفحات البوابة الإلكترونية</span>
            </h3>
            <span class="text-muted small">تعديل محتوى الصفحة وعكس التغييرات عليها كمسودة تحت حسابك دون نشر للعامة</span>
        </div>

        <div class="d-flex align-items-center gap-2 w-100 w-md-auto">
            <asp:TextBox ID="txtPageUrl" runat="server" ClientIDMode="Static" CssClass="form-control" placeholder="أدخل رابط الصفحة (مثال: /ar/RegAdm/PGD/Pages/HumanitarianPror.aspx)" style="min-width: 360px; direction: ltr;"></asp:TextBox>
            <asp:Button ID="btnLoadPage" runat="server" Text="جلب المحتوى" CssClass="btn pnu-btn-primary flex-shrink-0" OnClick="btnLoadPage_Click" />
        </div>
    </div>

    <!-- Status Messages & Check-Out Warnings -->
    <asp:Panel ID="pnlStatus" runat="server" Visible="false">
        <div id="divStatusAlert" runat="server" class="pnu-status-alert">
            <asp:Literal ID="litStatusMessage" runat="server"></asp:Literal>
        </div>
    </asp:Panel>

    <!-- Always Rendered Hidden Field (outside pnlEditorArea to ensure availability in DOM) -->
    <asp:HiddenField ID="hfPageRawContent" runat="server" ClientIDMode="Static" />
    <!-- Remembers where the content was read from (field:<name> or cewp:<id>) so the save writes it back to the same place -->
    <asp:HiddenField ID="hfContentSource" runat="server" ClientIDMode="Static" />

    <!-- Editor Workspace -->
    <asp:Panel ID="pnlEditorArea" runat="server" Visible="false">
        <!-- Page Metadata & Version Bar -->
        <div class="card border rounded-3 p-3 bg-light mb-3">
            <div class="d-flex justify-content-between align-items-center flex-wrap gap-2 mb-2 pb-2 border-bottom">
                <div class="d-flex flex-column gap-1">
                    <div class="d-flex align-items-center gap-2 flex-wrap">
                        <span class="text-muted small">الصفحة الحالية:</span>
                        <strong class="text-dark fs-6" id="lblCurrentPageTitle" runat="server" clientidmode="Static"></strong>
                        <span id="spCheckOutBadge" runat="server" clientidmode="Static" class="badge bg-success-subtle text-success border border-success-subtle ms-1"></span>
                        <span class="badge bg-primary-subtle text-primary border border-primary-subtle ms-1">
                            <i class="hgi hgi-stroke hgi-git-commit me-1"></i><span id="lblCurrentVersionBadge" runat="server" clientidmode="Static">الإصدار المنشور</span>
                        </span>
                    </div>
                    <div class="d-flex align-items-center gap-3 text-muted small flex-wrap">
                        <span><i class="hgi hgi-stroke hgi-building-03 me-1"></i>الموقع: <strong id="lblCurrentWebInfo" runat="server" clientidmode="Static"></strong></span>
                        <span><i class="hgi hgi-stroke hgi-database me-1"></i>حقل البيانات: <strong id="lblDetectedField" runat="server" clientidmode="Static" class="text-primary"></strong></span>
                    </div>
                </div>
                <div class="d-flex align-items-center gap-2">
                    <asp:Button ID="btnDiscardCheckOut" runat="server" Text="إلغاء السحب والتراجع" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnDiscardCheckOut_Click" OnClientClick="return confirm('هل أنت متأكد من إلغاء السحب والتراجع عن التعديلات؟');" />
                    <asp:Button ID="btnSaveDraft" runat="server" Text="💾 حفظ التعديلات (مسودة Draft)" CssClass="btn btn-sm pnu-btn-primary shadow-sm" OnClick="btnSaveDraft_Click" OnClientClick="prepareContentForSave();" />
                </div>
            </div>

            <!-- Version History Selector -->
            <div class="d-flex align-items-center justify-content-between flex-wrap gap-2 pt-1">
                <div class="d-flex align-items-center gap-2 w-100 w-md-auto flex-grow-1">
                    <span class="text-secondary small fw-medium text-nowrap"><i class="hgi hgi-stroke hgi-time-02 me-1 text-primary"></i>سجل الإصدارات:</span>
                    <asp:DropDownList ID="ddlPageVersions" runat="server" ClientIDMode="Static" CssClass="form-select form-select-sm" style="max-width: 550px; font-size: 0.88rem;"></asp:DropDownList>
                    <asp:Button ID="btnLoadVersion" runat="server" Text="جلب هذا الإصدار للمحرر" CssClass="btn btn-sm btn-outline-primary text-nowrap" OnClick="btnLoadVersion_Click" OnClientClick="return confirm('سيتم استبدال المحتوى في المحرر بالإصدار المحدد، هل تود المتابعة؟');" />
                </div>
                <span class="text-muted small">✨ يتم جلب الإصدار الرئيسي المنشور للعامة (Major Version) افتراضياً</span>
            </div>
        </div>

        <!-- Navigation Tabs: Visual Editor / HTML Source / Live Preview -->
        <div class="pnu-nav-tabs">
            <button type="button" class="pnu-tab-btn active" id="btnTabVisual" onclick="switchEditorTab('visual');">
                <i class="hgi hgi-stroke hgi-view"></i> المحرر المرئي (WYSIWYG)
            </button>
            <button type="button" class="pnu-tab-btn" id="btnTabSource" onclick="switchEditorTab('source');">
                <i class="hgi hgi-stroke hgi-code"></i> كود HTML المصدري
            </button>
            <button type="button" class="pnu-tab-btn" id="btnTabPreview" onclick="switchEditorTab('preview');">
                <i class="hgi hgi-stroke hgi-eye"></i> معاينة مباشرة للصفحة
            </button>
        </div>

        <!-- Tab 1: TinyMCE Visual Editor -->
        <div id="panelVisualEditor" style="display: block;">
            <textarea id="tinymceEditorArea" style="width: 100%; min-height: 520px;"></textarea>
        </div>

        <!-- Tab 2: HTML Source Code View -->
        <div id="panelSourceEditor" style="display: none;">
            <asp:TextBox ID="txtSourceCode" runat="server" ClientIDMode="Static" TextMode="MultiLine" CssClass="pnu-source-textarea" spellcheck="false" EnableViewState="false"></asp:TextBox>
        </div>

        <!-- Tab 3: Live Preview View -->
        <div id="panelLivePreview" style="display: none;">
            <div id="divLivePreviewContainer" class="pnu-preview-frame"></div>
        </div>
    </asp:Panel>
</div>

<!-- Editor Scripts (Zero Code-Behind inline expressions) -->
<script type="text/javascript">
    var currentActiveTab = 'visual';
    // On a postback both the registered startup script and DOMContentLoaded call
    // initTinyMceEditor. tinymce.init is async, so a plain tinymce.get() check is not
    // enough to stop the second call creating a duplicate editor.
    var tinyMceBootstrapped = false;

    function initTinyMceEditor() {
        var hiddenField = document.getElementById('hfPageRawContent');
        var initialContent = (hiddenField && hiddenField.value) ? hiddenField.value : '';

        var ta = document.getElementById('tinymceEditorArea');
        if (!ta) return;

        ta.value = initialContent;
        currentActiveTab = 'visual';
        wireSourceTextareaSync();

        if (typeof tinymce !== 'undefined') {
            try {
                if (tinyMceBootstrapped) {
                    // Already initialising or initialised: just refresh the content.
                    var existing = tinymce.get('tinymceEditorArea');
                    if (existing) existing.setContent(initialContent);
                    return;
                }
                tinyMceBootstrapped = true;

                tinymce.init({
                    selector: '#tinymceEditorArea',
                    height: 560,
                    menubar: 'file edit view insert format tools table',
                    plugins: [
                        'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
                        'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
                        'insertdatetime', 'media', 'table', 'help', 'wordcount', 'directionality'
                    ],
                    toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | rtl ltr | bullist numlist outdent indent | table link image | code fullscreen preview',
                    directionality: 'rtl',
                    // The tinymce package ships no language packs, so the Arabic pack must be
                    // pointed at explicitly or TinyMCE 404s and silently falls back to English.
                    language: 'ar',
                    language_url: 'https://cdn.jsdelivr.net/npm/tinymce-i18n@23.7.3/langs6/ar.js',
                    content_style: `
                        @import url('https://fonts.googleapis.com/css2?family=IBM+Plex+Sans+Arabic:wght@400;500;600;700&display=swap');
                        @import url('https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css');
                        body {
                            font-family: 'IBM Plex Sans Arabic', -apple-system, BlinkMacSystemFont, sans-serif;
                            font-size: 15px;
                            direction: rtl;
                            text-align: right;
                            padding: 16px;
                            color: #212529;
                            background-color: #ffffff;
                        }
                        .table { width: 100%; border-collapse: collapse; margin-bottom: 1rem; }
                        .table td, .table th { border: 1px solid #dee2e6; padding: .75rem; vertical-align: middle; }
                    `,
                    verify_html: false,
                    valid_elements: '*[*]',
                    valid_children: '+body[style]',
                    extended_valid_elements: '*[*]',
                    setup: function (editor) {
                        editor.on('init', function () {
                            if (initialContent) {
                                editor.setContent(initialContent);
                            }
                        });
                        editor.on('change keyup', function () {
                            syncContentToFields(editor.getContent());
                        });
                    }
                });
            } catch (e) {
                tinyMceBootstrapped = false;
                console.error('TinyMCE initialization error: ', e);
            }
        }
    }

    function switchEditorTab(tab) {
        // Read from the tab we are LEAVING before switching, otherwise edits made in
        // that tab are read back from the wrong place and silently discarded.
        var content = getContentFromTab(currentActiveTab);
        syncContentToFields(content);

        currentActiveTab = tab;

        var btnVisual = document.getElementById('btnTabVisual');
        var btnSource = document.getElementById('btnTabSource');
        var btnPreview = document.getElementById('btnTabPreview');

        if (btnVisual) btnVisual.classList.remove('active');
        if (btnSource) btnSource.classList.remove('active');
        if (btnPreview) btnPreview.classList.remove('active');

        var pVisual = document.getElementById('panelVisualEditor');
        var pSource = document.getElementById('panelSourceEditor');
        var pPreview = document.getElementById('panelLivePreview');

        if (pVisual) pVisual.style.display = 'none';
        if (pSource) pSource.style.display = 'none';
        if (pPreview) pPreview.style.display = 'none';

        if (tab === 'visual') {
            if (btnVisual) btnVisual.classList.add('active');
            if (pVisual) pVisual.style.display = 'block';
            if (typeof tinymce !== 'undefined' && tinymce.get('tinymceEditorArea')) {
                tinymce.get('tinymceEditorArea').setContent(content);
            }
        } else if (tab === 'source') {
            if (btnSource) btnSource.classList.add('active');
            if (pSource) pSource.style.display = 'block';
            var txtSource = document.getElementById('txtSourceCode');
            if (txtSource) {
                txtSource.value = content;
            }
        } else if (tab === 'preview') {
            if (btnPreview) btnPreview.classList.add('active');
            if (pPreview) pPreview.style.display = 'block';
            var previewContainer = document.getElementById('divLivePreviewContainer');
            if (previewContainer) {
                previewContainer.innerHTML = content;
            }
        }
    }

    // Reads the live content of a specific tab. The preview tab holds no editable
    // state of its own, so it falls back to the hidden field.
    function getContentFromTab(tab) {
        if (tab === 'visual') {
            var editor = (typeof tinymce !== 'undefined') ? tinymce.get('tinymceEditorArea') : null;
            if (editor) return editor.getContent();
        } else if (tab === 'source') {
            var txtSource = document.getElementById('txtSourceCode');
            if (txtSource) return txtSource.value;
        }

        var hf = document.getElementById('hfPageRawContent');
        return hf ? hf.value : '';
    }

    function getCurrentContent() {
        return getContentFromTab(currentActiveTab);
    }

    function syncContentToFields(content) {
        var hf = document.getElementById('hfPageRawContent');
        if (hf) {
            hf.value = content;
        }
        var txtSource = document.getElementById('txtSourceCode');
        if (txtSource && txtSource.value !== content) {
            txtSource.value = content;
        }
    }

    // Typing in the HTML source tab must reach the hidden field too, otherwise the
    // server only ever sees the content the page was loaded with.
    function wireSourceTextareaSync() {
        var txtSource = document.getElementById('txtSourceCode');
        if (!txtSource || txtSource.getAttribute('data-sync-wired') === '1') return;

        txtSource.addEventListener('input', function () {
            var hf = document.getElementById('hfPageRawContent');
            if (hf) hf.value = txtSource.value;
        });
        txtSource.setAttribute('data-sync-wired', '1');
    }

    function prepareContentForSave() {
        var finalContent = getCurrentContent();
        syncContentToFields(finalContent);
        if (typeof tinymce !== 'undefined' && tinymce.get('tinymceEditorArea')) {
            tinymce.get('tinymceEditorArea').save();
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        initTinyMceEditor();
    });

    if (typeof (Sys) !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            // A partial postback replaces the DOM, so the previous editor is gone.
            var stale = (typeof tinymce !== 'undefined') ? tinymce.get('tinymceEditorArea') : null;
            if (stale) {
                try { stale.remove(); } catch (e) { }
            }
            tinyMceBootstrapped = false;
            initTinyMceEditor();
        });
    }
</script>
