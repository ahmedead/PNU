<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteListDataEntry.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.SiteListDataEntry" %>

<style>
    .sld-wrapper { font-family: 'Segoe UI', sans-serif; direction: rtl; padding: 20px; }
    .sld-section { background: #f8f9fa; border: 1px solid #dee2e6; border-radius: 6px; padding: 20px; margin-bottom: 16px; }
    .sld-section h3 { margin: 0 0 14px 0; font-size: 15px; color: #333; border-bottom: 2px solid #0078d4; padding-bottom: 6px; display: flex; align-items: center; justify-content: space-between; }
    .sld-row { display: flex; align-items: center; gap: 12px; flex-wrap: wrap; margin-bottom: 10px; }
    .sld-label { min-width: 130px; font-weight: 600; color: #444; font-size: 13px; }
    .sld-select { flex: 1; min-width: 200px; padding: 6px 10px; border: 1px solid #ccc; border-radius: 4px; font-size: 13px; }
    .sld-input  { flex: 1; min-width: 300px; padding: 6px 10px; border: 1px solid #ccc; border-radius: 4px; font-size: 13px; direction: ltr; }
    .sld-btn { padding: 7px 20px; border: none; border-radius: 4px; cursor: pointer; font-size: 13px; font-weight: 600; }
    .sld-btn-primary   { background: #0078d4; color: #fff; }
    .sld-btn-primary:hover   { background: #005fa3; }
    .sld-btn-success   { background: #28a745; color: #fff; }
    .sld-btn-success:hover   { background: #1e7e34; }
    .sld-btn-secondary { background: #6c757d; color: #fff; }
    .sld-btn-secondary:hover { background: #565e64; }
    .sld-btn-sm { padding: 4px 12px; font-size: 12px; }
    .sld-btn-warning { background: #fd7e14; color: #fff; padding: 4px 12px; font-size: 12px; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; }
    .sld-btn-warning:hover { background: #e06910; }
    .sld-btn-danger  { background: #dc3545; color: #fff; padding: 4px 12px; font-size: 12px; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; }
    .sld-btn-danger:hover  { background: #b02a37; }
    /* Form */
    .sld-form-table { width: 100%; border-collapse: collapse; }
    .sld-form-table tr td { padding: 7px 10px; vertical-align: middle; }
    .sld-form-table tr td:first-child { width: 180px; font-weight: 600; color: #444; font-size: 13px; }
    .sld-form-table input[type=text], .sld-form-table input[type=number],
    .sld-form-table textarea, .sld-form-table select { width: 100%; padding: 6px 10px; border: 1px solid #ccc; border-radius: 4px; font-size: 13px; box-sizing: border-box; }
    .sld-form-table textarea { height: 80px; resize: vertical; }
    .sld-form-table input[type=file] { font-size: 13px; }
    .sld-img-preview { max-width: 120px; max-height: 120px; margin-top: 6px; border-radius: 4px; border: 1px solid #ccc; display: none; }
    /* Grid */
    .sld-grid { width: 100%; border-collapse: collapse; font-size: 13px; }
    .sld-grid th { background: #0078d4; color: #fff; padding: 8px 10px; text-align: right; white-space: nowrap; }
    .sld-grid td { padding: 7px 10px; border-bottom: 1px solid #dee2e6; vertical-align: middle; }
    .sld-grid tr:hover td { background: #e9f3fb; }
    .sld-grid tr.sld-edit-row td { background: #fff8e1; }
    .sld-grid .sld-actions { display: flex; gap: 6px; white-space: nowrap; }
    .sld-grid-img { max-width: 60px; max-height: 60px; border-radius: 3px; }
    .sld-grid-scroll { overflow-x: auto; }
    .sld-pager { display: flex; gap: 6px; align-items: center; margin-top: 10px; flex-wrap: wrap; }
    /* Messages */
    .sld-msg-success { background: #d4edda; color: #155724; border: 1px solid #c3e6cb; border-radius: 4px; padding: 10px 14px; margin-top: 10px; display: block; }
    .sld-msg-error   { background: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; border-radius: 4px; padding: 10px 14px; margin-top: 10px; display: block; }
    .sld-hint { font-size: 11px; color: #888; margin-top: 4px; }
</style>

<%-- Hidden fields carry the grid action back to the server without dynamic button wiring --%>
<asp:HiddenField ID="hfActionItemId" runat="server" Value="0" />
<asp:HiddenField ID="hfActionType"   runat="server" Value="" />

<%-- Invisible server button triggered by JS — the ONE stable postback entry point for grid actions --%>
<asp:Button ID="btnGridAction" runat="server" Text="" Style="display:none"
    OnClick="btnGridAction_Click" />

<div class="sld-wrapper">

    <%-- STEP 1: Site URL --%>
    <div class="sld-section">
        <h3>اختيار الموقع</h3>
        <div class="sld-row">
            <span class="sld-label">رابط الموقع:</span>
            <asp:TextBox ID="txtSiteUrl" runat="server" CssClass="sld-input" />
            <asp:Button ID="btnLoadLists" runat="server" Text="تحميل القوائم"
                CssClass="sld-btn sld-btn-primary" OnClick="btnLoadLists_Click" />
        </div>
        <div class="sld-hint">أدخل الرابط الكامل للموقع أو الموقع الفرعي ثم اضغط "تحميل القوائم"</div>
        <asp:Label ID="lblSiteError" runat="server" CssClass="sld-msg-error" Visible="false" />
    </div>

    <%-- STEP 2: List picker --%>
    <asp:Panel ID="pnlLists" runat="server" Visible="false">
        <div class="sld-section">
            <h3>اختيار القائمة</h3>
            <div class="sld-row">
                <span class="sld-label">القائمة:</span>
                <asp:DropDownList ID="ddlLists" runat="server" CssClass="sld-select">
                    <asp:ListItem Text="-- اختر قائمة --" Value="" />
                </asp:DropDownList>
                <asp:Button ID="btnLoadFields" runat="server" Text="تحميل الحقول والبيانات"
                    CssClass="sld-btn sld-btn-primary" OnClick="btnLoadFields_Click" />
            </div>
            <asp:Label ID="lblListError" runat="server" CssClass="sld-msg-error" Visible="false" />
        </div>
    </asp:Panel>

    <%-- STEP 3: Add / Edit form --%>
    <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <div class="sld-section">
            <h3>
                <asp:Label ID="lblFormTitle" runat="server" Text="إضافة عنصر جديد" />
                <asp:Button ID="btnCancelEdit" runat="server" Text="إلغاء" Visible="false"
                    CssClass="sld-btn sld-btn-secondary sld-btn-sm" OnClick="btnCancelEdit_Click" />
            </h3>

            <asp:Panel ID="pnlImageLibrary" runat="server" Visible="false">
                <div class="sld-row" style="margin-bottom:14px;padding-bottom:12px;border-bottom:1px solid #dee2e6;">
                    <span class="sld-label">مكتبة الصور:</span>
                    <asp:DropDownList ID="ddlImageLibrary" runat="server" CssClass="sld-select">
                        <asp:ListItem Text="-- اختر مكتبة الصور --" Value="" />
                    </asp:DropDownList>
                </div>
            </asp:Panel>

            <table class="sld-form-table">
                <asp:PlaceHolder ID="phFields" runat="server" />
            </table>

            <div class="sld-row" style="margin-top:14px;">
                <asp:Button ID="btnSave" runat="server" Text="حفظ"
                    CssClass="sld-btn sld-btn-success" OnClick="btnSave_Click" />
            </div>
            <asp:Label ID="lblFormMsg" runat="server" Visible="false" />
        </div>
    </asp:Panel>

    <%-- STEP 4: Data grid --%>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <div class="sld-section">
            <h3>
                بيانات القائمة
                <asp:Button ID="btnRefreshGrid" runat="server" Text="↻ تحديث"
                    CssClass="sld-btn sld-btn-secondary sld-btn-sm" OnClick="btnRefreshGrid_Click" />
            </h3>
            <asp:Label ID="lblGridMsg" runat="server" Visible="false" />
            <div class="sld-grid-scroll">
                <asp:PlaceHolder ID="phGrid" runat="server" />
            </div>
            <div class="sld-pager">
                <asp:Button ID="btnPrevPage" runat="server" Text="◀ السابق"
                    CssClass="sld-btn sld-btn-secondary sld-btn-sm" OnClick="btnPrevPage_Click" />
                <asp:Label ID="lblPageInfo" runat="server" />
                <asp:Button ID="btnNextPage" runat="server" Text="التالي ▶"
                    CssClass="sld-btn sld-btn-secondary sld-btn-sm" OnClick="btnNextPage_Click" />
            </div>
        </div>
    </asp:Panel>

</div>

<script>
    // Grid action — sets hidden fields then clicks the invisible server button
    function sldGridAction(action, itemId) {
        if (action === 'delete' && !confirm('هل أنت متأكد من حذف هذا العنصر؟')) return;
        document.getElementById('<%= hfActionType.ClientID %>').value = action;
        document.getElementById('<%= hfActionItemId.ClientID %>').value = itemId;
        document.getElementById('<%= btnGridAction.ClientID %>').click();
    }

    // Image preview on file pick
    document.addEventListener('change', function (e) {
        if (e.target && e.target.type === 'file' && e.target.accept === 'image/*') {
            var file = e.target.files[0];
            if (!file) return;
            var preview = e.target.parentNode.querySelector('.sld-img-preview');
            if (!preview) return;
            var reader = new FileReader();
            reader.onload = function (ev) { preview.src = ev.target.result; preview.style.display = 'block'; };
            reader.readAsDataURL(file);
        }
    });
</script>
