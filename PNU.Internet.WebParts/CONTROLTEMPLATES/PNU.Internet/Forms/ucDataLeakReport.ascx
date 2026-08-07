<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDataLeakReport.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms.ucDataLeakReport" %>




<script src="https://www.google.com/recaptcha/api.js" async defer></script>

<div class="data-leak-form-wrapper" dir="rtl">

    <asp:Panel ID="pnlData" runat="server">

        <div class="form-header">
            <h2>الإبلاغ عن حوادث تسريب البيانات</h2>
            <p class="form-intro">
                يهدف هذا النموذج إلى استقبال البلاغات المتعلقة بحوادث التسريب أو الاستخدام غير المصرح به للبيانات الشخصية،
                وذلك لحماية خصوصية منسوبي الجامعة والحد من مخاطر تسريب البيانات.
                سيتم التعامل مع جميع البلاغات بسرية تامة وفق الأنظمة والسياسات المعتمدة.
            </p>
        </div>

        <%-- ============== Reporter Data ============== --%>
        <fieldset class="form-section">
            <legend>بيانات مُقدم البلاغ</legend>

            <div class="form-row">
                <label for="<%= txtFullName.ClientID %>">الاسم الكامل <span class="req">*</span></label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-row">
                <label for="<%= txtMobile.ClientID %>">رقم الجوال <span class="req">*</span></label>
                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" TextMode="Phone" />
                <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revMobile" runat="server" 
    ControlToValidate="txtMobile"
    ValidationGroup="DataLeak"
    ValidationExpression="^[0-9+\-\s()]+$"
    ErrorMessage="يرجى إدخال رقم جوال صحيح"
    CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-row">
                <label for="<%= txtEmail.ClientID %>">البريد الإلكتروني <span class="req">*</span></label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationGroup="DataLeak" ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                    ErrorMessage="بريد إلكتروني غير صحيح" CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-row">
                <label>الصفة <span class="req">*</span></label>
                <asp:RadioButtonList ID="rblCapacity" runat="server" RepeatDirection="Vertical" CssClass="radio-list">
                    <asp:ListItem Text="طالب" Value="طالب" />
                    <asp:ListItem Text="موظف" Value="موظف" />
                    <asp:ListItem Text="موظف سابق" Value="موظف سابق" />
                    <asp:ListItem Text="جهة خارجية" Value="جهة خارجية" />
                    <asp:ListItem Text="متعاقد / مزود خدمة" Value="متعاقد / مزود خدمة" />
                    <asp:ListItem Text="أخرى" Value="أخرى" />
                </asp:RadioButtonList>
                <asp:TextBox ID="txtCapacityOther" runat="server" CssClass="form-control other-input"
                    placeholder="يرجى التوضيح"  Enabled="false" />
                <asp:RequiredFieldValidator ID="rfvCapacity" runat="server" ControlToValidate="rblCapacity"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
            </div>
        </fieldset>

        <%-- ============== Incident Details ============== --%>
        <fieldset class="form-section">
            <legend>تفاصيل حادثة تسريب البيانات الشخصية</legend>

            <div class="form-row">
    <label for="<%= txtDiscoveryDate.ClientID %>">تاريخ اكتشاف التسريب <span class="req">*</span></label>
    <asp:TextBox ID="txtDiscoveryDate" runat="server" CssClass="form-control gregorian-date"
        placeholder="yyyy-MM-dd مثال: 2026-05-14" autocomplete="off" />
    <small style="color:#888;">التاريخ الميلادي فقط (yyyy-MM-dd)</small>
    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDiscoveryDate"
        ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
    <asp:RegularExpressionValidator ID="revDate" runat="server" ControlToValidate="txtDiscoveryDate"
        ValidationGroup="DataLeak"
        ValidationExpression="^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$"
        ErrorMessage="صيغة التاريخ غير صحيحة (yyyy-MM-dd)" CssClass="error-msg" Display="Dynamic" />
</div>

            <div class="form-row">
                <label>هل التسريب ما زال قائماً وقت الإبلاغ؟ <span class="req">*</span></label>
                <asp:RadioButtonList ID="rblStillActive" runat="server" RepeatDirection="Horizontal" CssClass="radio-list">
                    <asp:ListItem Text="نعم" Value="نعم" />
                    <asp:ListItem Text="لا" Value="لا" />
                    <asp:ListItem Text="غير متأكد" Value="غير متأكد" />
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvStillActive" runat="server" ControlToValidate="rblStillActive"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-row">
                <label>نوع مصدر تسريب البيانات <span class="req">*</span> <small>(يمكن اختيار أكثر من خيار)</small></label>
                <asp:CheckBoxList ID="cblSourceType" runat="server" RepeatDirection="Vertical" CssClass="checkbox-list">
                    <asp:ListItem Text="موقع الكتروني" Value="موقع الكتروني" />
                    <asp:ListItem Text="بريد الكتروني" Value="بريد الكتروني" />
                    <asp:ListItem Text="نظام / منصة الكترونية" Value="نظام / منصة الكترونية" />
                    <asp:ListItem Text="وسائط تخزين" Value="وسائط تخزين" />
                    <asp:ListItem Text="أخرى" Value="أخرى" />
                </asp:CheckBoxList>
                <asp:TextBox ID="txtSourceTypeOther" runat="server" CssClass="form-control other-input"
                    placeholder="يرجى التوضيح"  Enabled="false" />
            </div>

            <div class="form-row">
                <label for="<%= txtSourceName.ClientID %>">اسم مصدر التسريب والرابط إن أمكن <span class="req">*</span></label>
                <asp:TextBox ID="txtSourceName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvSourceName" runat="server" ControlToValidate="txtSourceName"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-row">
                <label>نوع البيانات الشخصية المتأثرة <span class="req">*</span> <small>(يمكن اختيار أكثر من خيار)</small></label>
                <asp:CheckBoxList ID="cblDataType" runat="server" RepeatDirection="Vertical" CssClass="checkbox-list">
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
                <asp:TextBox ID="txtDataTypeOther" runat="server" CssClass="form-control other-input"
                    placeholder="يرجى التوضيح"  Enabled="false" />
            </div>

            <div class="form-row">
                <label>صيغة البيانات التي تم تسريبها <span class="req">*</span> <small>(يمكن اختيار أكثر من خيار)</small></label>
                <asp:CheckBoxList ID="cblDataFormat" runat="server" RepeatDirection="Vertical" CssClass="checkbox-list">
                    <asp:ListItem Text="ملفات (Excel – Word – PDF – صور – قواعد بيانات)"
                        Value="ملفات (Excel – Word – PDF – صور – قواعد بيانات)" />
                    <asp:ListItem Text="نصوص مكتوبة مباشرة" Value="نصوص مكتوبة مباشرة" />
                    <asp:ListItem Text="نسخ ورقية" Value="نسخ ورقية" />
                    <asp:ListItem Text="روابط إلكترونية" Value="روابط إلكترونية" />
                    <asp:ListItem Text="أخرى" Value="أخرى" />
                </asp:CheckBoxList>
                <asp:TextBox ID="txtDataFormatOther" runat="server" CssClass="form-control other-input"
                    placeholder="يرجى التوضيح"  Enabled="false" />
            </div>

            <div class="form-row">
                <label>طريقة الوصول إلى البيانات <span class="req">*</span></label>
                <asp:RadioButtonList ID="rblAccessMethod" runat="server" RepeatDirection="Vertical" CssClass="radio-list">
                    <asp:ListItem Text="متاحة للعامة" Value="متاحة للعامة" />
                    <asp:ListItem Text="محمية بكلمة مرور" Value="محمية بكلمة مرور" />
                    <asp:ListItem Text="أُرسلت بالخطأ" Value="أُرسلت بالخطأ" />
                    <asp:ListItem Text="تم الحصول عليها دون تصريح" Value="تم الحصول عليها دون تصريح" />
                    <asp:ListItem Text="غير معروف" Value="غير معروف" />
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvAccessMethod" runat="server" ControlToValidate="rblAccessMethod"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-row">
                <label for="<%= txtAffectedCount.ClientID %>">عدد الأشخاص المتأثرين (إن أمكن)</label>
                <asp:TextBox ID="txtAffectedCount" runat="server" CssClass="form-control" />
            </div>

            <div class="form-row">
                <label>هل البيانات تخص <span class="req">*</span> <small>(يمكن اختيار أكثر من خيار)</small></label>
                <asp:CheckBoxList ID="cblDataBelongsTo" runat="server" RepeatDirection="Vertical" CssClass="checkbox-list">
                    <asp:ListItem Text="طالب واحد" Value="طالب واحد" />
                    <asp:ListItem Text="موظف واحد" Value="موظف واحد" />
                    <asp:ListItem Text="مجموعة طلاب" Value="مجموعة طلاب" />
                    <asp:ListItem Text="مجموعة موظفين" Value="مجموعة موظفين" />
                    <asp:ListItem Text="أكثر من فئة" Value="أكثر من فئة" />
                </asp:CheckBoxList>
            </div>

            <div class="form-row">
                <label for="<%= txtIncidentDescription.ClientID %>">وصف الحادث <span class="req">*</span></label>
                <small>يرجى وصف واقعة تسريب البيانات الشخصية بشكل واضح ومفصل</small>
                <asp:TextBox ID="txtIncidentDescription" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="6" />
                <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtIncidentDescription"
                    ValidationGroup="DataLeak" ErrorMessage="مطلوب" CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-row">
                <label for="<%= fuAttachment.ClientID %>">المرفقات</label>
                <small>يرجى إرفاق تصوير للشاشة أو أي مرفقات خاصة بعملية التسريب</small>
                <asp:FileUpload ID="fuAttachment" runat="server" CssClass="form-control" AllowMultiple="true" />
            </div>
            <div class="form-row">
<%--    <button class="g-recaptcha" 
 data-sitekey="6LcZPOwsAAAAANKWNl2zf4OAAhmseEf0rJDjdj1m" 
 data-callback='onSubmit' 
 data-action='submit'>Submit</button>--%>

                <div class="form-group w-100">
    <div class="g-recaptcha" data-sitekey="6Leg0vAsAAAAAI9NROAGxiBaSjQD4UMc7eBzL7GL"></div>
</div>
    </div>
        </fieldset>
       
        <div class="form-actions">
            <asp:Button ID="btnSubmit" runat="server" Text="إرسال البلاغ" CssClass="btn-submit"
                OnClick="btnSubmit_Click" ValidationGroup="DataLeak" />
        </div>

        <asp:Label ID="lblException" runat="server" CssClass="error-msg" Visible="false" />

    </asp:Panel>

    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="success-panel">
        <div class="success-icon">✓</div>
        <h3>شكراً لك..</h3>
        <p>تم تسجيل بلاغك بنجاح</p>
        <asp:Label ID="lblSuccessMessage" runat="server" />
    </asp:Panel>


    <script type="text/javascript">
    (function () {
        function initDataLeakOtherToggles() {
            // RadioButtonList -> Other text field mappings
            var radioMappings = [
                { groupId: '<%= rblCapacity.ClientID %>', otherInputId: '<%= txtCapacityOther.ClientID %>', otherValue: 'أخرى' }
            ];

            // CheckBoxList -> Other text field mappings
            var checkboxMappings = [
                { groupId: '<%= cblSourceType.ClientID %>', otherInputId: '<%= txtSourceTypeOther.ClientID %>', otherValue: 'أخرى' },
                { groupId: '<%= cblDataType.ClientID %>', otherInputId: '<%= txtDataTypeOther.ClientID %>', otherValue: 'أخرى' },
                { groupId: '<%= cblDataFormat.ClientID %>', otherInputId: '<%= txtDataFormatOther.ClientID %>', otherValue: 'أخرى' }
                ];

                function toggleOtherInput(otherInputId, enable) {
                    var input = document.getElementById(otherInputId);
                    if (!input) return;
                    input.disabled = !enable;
                    if (!enable) {
                        input.value = '';
                        input.classList.remove('enabled');
                    } else {
                        input.classList.add('enabled');
                        input.focus();
                    }
                }

                function checkRadioOther(mapping) {
                    var group = document.getElementById(mapping.groupId);
                    if (!group) return;
                    var radios = group.querySelectorAll('input[type="radio"]');
                    var isOtherSelected = false;
                    radios.forEach(function (r) {
                        // Match by adjacent label text since RadioButtonList renders labels next to inputs
                        var label = r.parentNode.querySelector('label') ||
                            (r.nextSibling && r.nextSibling.tagName === 'LABEL' ? r.nextSibling : null) ||
                            document.querySelector('label[for="' + r.id + '"]');
                        var labelText = label ? label.textContent.trim() : '';
                        if (r.checked && labelText === mapping.otherValue) {
                            isOtherSelected = true;
                        }
                    });
                    toggleOtherInput(mapping.otherInputId, isOtherSelected);
                }

                function checkCheckboxOther(mapping) {
                    var group = document.getElementById(mapping.groupId);
                    if (!group) return;
                    var checkboxes = group.querySelectorAll('input[type="checkbox"]');
                    var isOtherSelected = false;
                    checkboxes.forEach(function (c) {
                        var label = c.parentNode.querySelector('label') ||
                            document.querySelector('label[for="' + c.id + '"]');
                        var labelText = label ? label.textContent.trim() : '';
                        if (c.checked && labelText === mapping.otherValue) {
                            isOtherSelected = true;
                        }
                    });
                    toggleOtherInput(mapping.otherInputId, isOtherSelected);
                }

                // Wire up radio button lists
                radioMappings.forEach(function (mapping) {
                    var group = document.getElementById(mapping.groupId);
                    if (!group) return;
                    var radios = group.querySelectorAll('input[type="radio"]');
                    radios.forEach(function (r) {
                        r.addEventListener('change', function () { checkRadioOther(mapping); });
                    });
                    // Initial state check (handles postback retention)
                    checkRadioOther(mapping);
                });

                // Wire up checkbox lists
                checkboxMappings.forEach(function (mapping) {
                    var group = document.getElementById(mapping.groupId);
                    if (!group) return;
                    var checkboxes = group.querySelectorAll('input[type="checkbox"]');
                    checkboxes.forEach(function (c) {
                        c.addEventListener('change', function () { checkCheckboxOther(mapping); });
                    });
                    // Initial state check
                    checkCheckboxOther(mapping);
                });
            }

            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', initDataLeakOtherToggles);
            } else {
                initDataLeakOtherToggles();
            }
        })();
    </script>
<%-- Flatpickr CSS + JS from CDN --%>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
<script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
<script src="https://npmcdn.com/flatpickr/dist/l10n/ar.js"></script>

<script type="text/javascript">
    (function () {
        function initGregorianDatePicker() {
            var dateInput = document.getElementById('<%= txtDiscoveryDate.ClientID %>');
            if (!dateInput || typeof flatpickr === 'undefined') return;

            // Format today as yyyy-MM-dd in Gregorian (forced via Intl with gregory calendar)
            var today = new Date();
            var maxDateStr = today.getFullYear() + '-' +
                String(today.getMonth() + 1).padStart(2, '0') + '-' +
                String(today.getDate()).padStart(2, '0');

            flatpickr(dateInput, {
                dateFormat: "Y-m-d",        // Forces Gregorian yyyy-MM-dd output
                maxDate: maxDateStr,         // Prevents future dates
                allowInput: true,
                locale: "ar",                // Arabic UI labels, but Gregorian calendar
                disableMobile: true          // Use Flatpickr UI even on mobile (avoid native Hijri picker)
            });
        }

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', initGregorianDatePicker);
        } else {
            initGregorianDatePicker();
        }
    })();
</script>
<style>
    .data-leak-form-wrapper .other-input:disabled {
        background-color: #f5f5f5;
        cursor: not-allowed;
        opacity: 0.6;
    }
    .data-leak-form-wrapper .other-input.enabled {
        background-color: #fff;
        border-color: #007a87;
    }
</style>
</div>

<style>
    .data-leak-form-wrapper {
        font-family: 'Tajawal', Tahoma, Arial, sans-serif;
        max-width: 900px;
        margin: 20px auto;
        padding: 20px;
        background: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }

    .data-leak-form-wrapper .form-header {
        text-align: center;
        margin-bottom: 30px;
        padding-bottom: 20px;
        border-bottom: 2px solid #007a87;
    }

    .data-leak-form-wrapper .form-header h2 {
        color: #007a87;
        margin: 0 0 10px 0;
    }

    .data-leak-form-wrapper .form-intro {
        color: #555;
        line-height: 1.8;
        font-size: 14px;
    }

    .data-leak-form-wrapper .form-section {
        border: 1px solid #e0e0e0;
        border-radius: 6px;
        padding: 20px;
        margin-bottom: 20px;
        background: #fafafa;
    }

    .data-leak-form-wrapper .form-section legend {
        font-weight: bold;
        color: #007a87;
        padding: 0 10px;
        font-size: 16px;
    }

    .data-leak-form-wrapper .form-row {
        margin-bottom: 18px;
    }

    .data-leak-form-wrapper .form-row label {
        display: block;
        margin-bottom: 6px;
        font-weight: 600;
        color: #333;
    }

    .data-leak-form-wrapper .form-row small {
        color: #777;
        font-weight: normal;
        display: block;
        margin-bottom: 6px;
    }

    .data-leak-form-wrapper .req {
        color: #d9534f;
    }

    .data-leak-form-wrapper .form-control {
        width: 100%;
        padding: 8px 12px;
        border: 1px solid #ccc;
        border-radius: 4px;
        font-size: 14px;
        box-sizing: border-box;
    }

    .data-leak-form-wrapper .form-control:focus {
        border-color: #007a87;
        outline: none;
    }

    .data-leak-form-wrapper .other-input {
        margin-top: 8px;
    }

    .data-leak-form-wrapper .radio-list label,
    .data-leak-form-wrapper .checkbox-list label {
        display: inline-block;
        font-weight: normal;
        margin: 4px 8px 4px 0;
    }

    .data-leak-form-wrapper .radio-list input,
    .data-leak-form-wrapper .checkbox-list input {
        margin-left: 6px;
    }

    .data-leak-form-wrapper .error-msg {
        color: #d9534f;
        font-size: 13px;
        display: inline-block;
        margin-right: 8px;
    }

    .data-leak-form-wrapper .form-actions {
        text-align: center;
        margin-top: 20px;
    }

    .data-leak-form-wrapper .btn-submit {
        background: #007a87;
        color: #fff;
        border: none;
        padding: 12px 40px;
        border-radius: 4px;
        font-size: 16px;
        font-weight: bold;
        cursor: pointer;
        transition: background 0.3s;
    }

    .data-leak-form-wrapper .btn-submit:hover {
        background: #005d66;
    }

    .data-leak-form-wrapper .success-panel {
        text-align: center;
        padding: 40px 20px;
    }

    .data-leak-form-wrapper .success-icon {
        font-size: 60px;
        color: #5cb85c;
        background: #e7f7e7;
        width: 100px;
        height: 100px;
        line-height: 100px;
        border-radius: 50%;
        margin: 0 auto 20px;
    }

    .data-leak-form-wrapper .success-panel h3 {
        color: #007a87;
    }
</style>
