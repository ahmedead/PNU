<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOpenDataRequest.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms.ucOpenDataRequest" %>

<script src="https://www.google.com/recaptcha/api.js" async defer></script>

<style type="text/css">
    .open-data-wrapper {
        font-family: "IBM Plex Sans Arabic", "Segoe UI", Tahoma, sans-serif;
    }

    .open-data-wrapper h3, 
    .open-data-wrapper h4, 
    .open-data-wrapper h5 {
        font-family: "IBM Plex Sans Arabic", "Segoe UI", Tahoma, sans-serif;
    }

    .open-data-card {
        background: #ffffff;
        border-radius: 16px;
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.05);
        border: 1px solid #eef2f6;
    }

    .open-data-header-icon {
        width: 50px;
        height: 50px;
        border-radius: 12px;
        background: rgba(0, 122, 135, 0.1);
        color: #007a87;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-size: 1.5rem;
        flex-shrink: 0;
    }

    .open-data-wrapper .form-label {
        font-size: 0.9rem;
        font-weight: 600;
        color: #1e293b;
        margin-bottom: 0.4rem;
        display: block;
    }

    .open-data-wrapper .required-star::after {
        content: " *";
        color: #dc3545;
        font-weight: bold;
    }

    .open-data-wrapper .form-control,
    .open-data-wrapper .form-select {
        border-radius: 8px;
        border: 1px solid #cbd5e1;
        padding: 0.65rem 0.9rem;
        font-size: 0.925rem;
        transition: border-color 0.2s ease, box-shadow 0.2s ease;
    }

    .open-data-wrapper .form-control:focus,
    .open-data-wrapper .form-select:focus {
        border-color: #007a87;
        box-shadow: 0 0 0 0.2rem rgba(0, 122, 135, 0.15);
        outline: none;
    }

    .open-data-wrapper .invalid-feedback-msg {
        color: #dc3545;
        font-size: 0.8125rem;
        margin-top: 5px;
        display: block;
    }

    .open-data-wrapper .invalid-feedback-msg[style*="display: none"],
    .open-data-wrapper .invalid-feedback-msg[style*="display:none"] {
        display: none !important;
    }

    .open-data-group-box {
        background-color: #f8fafc;
        border: 1px solid #e2e8f0;
        border-radius: 12px;
        padding: 1.25rem;
    }

    .open-data-checkbox-list {
        width: 100%;
        border-collapse: separate;
        border-spacing: 0 8px;
        margin-bottom: 0;
    }

    .open-data-checkbox-list td {
        padding: 6px 12px;
        vertical-align: middle;
        border: none;
    }

    .open-data-checkbox-list input[type="checkbox"],
    .open-data-role-list input[type="checkbox"] {
        width: 18px;
        height: 18px;
        accent-color: #007a87;
        cursor: pointer;
        vertical-align: middle;
        margin-left: 8px;
    }

    .open-data-checkbox-list label,
    .open-data-role-list label {
        cursor: pointer;
        font-size: 0.925rem;
        color: #334155;
        font-weight: 500;
        margin-bottom: 0;
        vertical-align: middle;
    }

    .open-data-role-list {
        display: flex;
        flex-wrap: wrap;
        gap: 1.5rem;
        align-items: center;
    }

    .open-data-role-list span {
        display: inline-flex;
        align-items: center;
    }

    .btn-submit-pnu {
        background-color: #007a87;
        border-color: #007a87;
        color: #ffffff !important;
        font-weight: 600;
        padding: 0.75rem 2.75rem;
        border-radius: 8px;
        transition: all 0.2s ease-in-out;
    }

    .btn-submit-pnu:hover,
    .btn-submit-pnu:focus {
        background-color: #005c66;
        border-color: #005c66;
        color: #ffffff !important;
        box-shadow: 0 4px 12px rgba(0, 122, 135, 0.25);
    }
</style>

<div class="open-data-wrapper py-4">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 col-xl-10">

                <asp:Label ID="lblException" runat="server" CssClass="alert alert-danger d-block mb-4" Visible="false" />

                <!-- Success Panel -->
                <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
                    <div class="card open-data-card border-0 p-5 text-center my-4">
                        <div class="display-3 text-success mb-3">✓</div>
                        <h3 class="fw-bold text-primary mb-2">تم استلام طلبك بنجاح</h3>
                        <p class="text-muted lead mb-3">شكراً لتواصلك معنا. سيتم مراجعة طلب البيانات والتواصل معك قريباً.</p>
                        <asp:Label ID="lblSuccessMessage" runat="server" CssClass="text-success fw-medium d-block mb-4" />
                        <div>
                            <a href="javascript:location.reload();" class="btn btn-outline-primary px-4 py-2">تقديم طلب جديد</a>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Form Panel -->
                <asp:Panel ID="pnlData" runat="server">
                    <div class="card open-data-card border-0 p-4 p-md-5">
                        
                        <!-- Header Section -->
                        <div class="d-flex align-items-center gap-3 mb-4 pb-3 border-bottom">
                            <div class="open-data-header-icon">
                                <i class="hgi hgi-stroke hgi-database" aria-hidden="true"></i>
                            </div>
                            <div>
                                <h3 class="fw-bold mb-1 h4">طلب بيانات مفتوحة</h3>
                                <p class="text-muted small mb-0">يمكنكم من خلال هذا النموذج تقديم طلب للحصول على مجموعات بيانات مفتوحة من جامعة الأميرة نورة بنت عبدالرحمن</p>
                            </div>
                        </div>

                        <div class="row g-4">
                            <!-- Name -->
                            <div class="col-12 col-md-6">
                                <asp:Label runat="server" AssociatedControlID="txtName" Text="<%$ Resources: PNUres, txtName %>" CssClass="form-label required-star" />
                                <div class="form-control-container">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="<%$ Resources: PNUres, txtName %>" aria-label="<%$ Resources: PNUres, txtName %>" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="invalid-feedback-msg" ValidationGroup="AddRequest" ControlToValidate="txtName" runat="server" ErrorMessage="<%$ Resources: PnuInternetResources, res_RequiredField %>" Display="Dynamic" />
                                </div>
                            </div>

                            <!-- Email -->
                            <div class="col-12 col-md-6">
                                <asp:Label runat="server" AssociatedControlID="txtEmail" Text="<%$ Resources: PNUres, lblEmailAddress %>" CssClass="form-label required-star" />
                                <div class="form-control-container">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="<%$ Resources: PNUres, lblEmailAddress %>" aria-label="<%$ Resources: PNUres, lblEmailAddress %>" TextMode="Email" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="invalid-feedback-msg" ValidationGroup="AddRequest" ControlToValidate="txtEmail" runat="server" ErrorMessage="<%$ Resources: PnuInternetResources, res_RequiredField %>" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="txtEmail"
                                        ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                        ErrorMessage="<%$ Resources: PnuInternetResources, res_InvalidEmailFormat %>"
                                        CssClass="invalid-feedback-msg"
                                        ValidationGroup="AddRequest"
                                        Display="Dynamic" />
                                </div>
                            </div>

                            <!-- Role / Beneficiary Category -->
                            <div class="col-12">
                                <div class="open-data-group-box">
                                    <label class="form-label required-star mb-3">شريحة المستفيدين من البيانات المفتوحة</label>
                                    <asp:CheckBoxList ID="chkRole" runat="server" RepeatLayout="Flow" CssClass="open-data-role-list">
                                        <asp:ListItem Text="باحث" Value="باحث" />
                                        <asp:ListItem Text="طالب" Value="طالب" />
                                        <asp:ListItem Text="موظف" Value="موظف" />
                                        <asp:ListItem Text="أخرى" Value="Other" />
                                    </asp:CheckBoxList>

                                    <div id="divOtherContainer" style="display: none;" class="mt-3">
                                        <asp:TextBox ID="txtOther" runat="server" CssClass="form-control" placeholder="من فضلك حدد الشريحة في حالة اختيار 'أخرى'" />
                                    </div>

                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="invalid-feedback-msg mt-2" Visible="false" />
                                </div>
                            </div>

                            <!-- Datasets Required -->
                            <div class="col-12">
                                <div class="open-data-group-box">
                                    <label class="form-label mb-2">مجموعة البيانات المطلوبة</label>
                                    <p class="text-muted small mb-3">يمكنك اختيار مجموعة أو أكثر من المجموعات التالية:</p>
                                    <div class="table-responsive">
                                        <asp:CheckBoxList ID="chkDatasets" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="open-data-checkbox-list">
                                            <asp:ListItem Text="الجامعة في أرقام" Value="الجامعة في أرقام" />
                                            <asp:ListItem Text="الموظفين" Value="الموظفين" />
                                            <asp:ListItem Text="الطالبات" Value="الطالبات" />
                                            <asp:ListItem Text="الأبحاث" Value="الأبحاث" />
                                            <asp:ListItem Text="الخريجات" Value="الخريجات" />
                                            <asp:ListItem Text="الخريجات في سوق العمل" Value="الخريجات في سوق العمل" />
                                            <asp:ListItem Text="براءات الاختراع" Value="براءات الاختراع" />
                                            <asp:ListItem Text="الجوائز" Value="الجوائز" />
                                            <asp:ListItem Text="الشؤون الرياضية" Value="الشؤون الرياضية" />
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>

                            <!-- Extra Open Data Request -->
                            <div class="col-12">
                                <asp:Label runat="server" AssociatedControlID="txtOpenDataRequest" Text="<%$ Resources: PNUres, txtOpenDataRequest %>" CssClass="form-label" />
                                <div class="form-control-container">
                                    <asp:TextBox ID="txtOpenDataRequest" runat="server" placeholder="<%$ Resources: PNUres, txtOpenDataRequest %>" CssClass="form-control" aria-label="<%$ Resources: PNUres, txtOpenDataRequest %>" TextMode="MultiLine" Rows="4" />
                                </div>
                            </div>

                            <!-- Suggestion / Notes -->
                            <div class="col-12">
                                <asp:Label runat="server" AssociatedControlID="txtSuggest" Text="<%$ Resources: PNUres, txtSuggest %>" CssClass="form-label" />
                                <div class="form-control-container">
                                    <asp:TextBox ID="txtSuggest" runat="server" placeholder="<%$ Resources: PNUres, txtSuggest %>" CssClass="form-control" aria-label="<%$ Resources: PNUres, txtSuggest %>" TextMode="MultiLine" Rows="3" />
                                </div>
                            </div>

                            <!-- Google reCAPTCHA -->
                            <div class="col-12">
                                <div class="form-group w-100">
                                    <div class="g-recaptcha" data-sitekey="6Leg0vAsAAAAAI9NROAGxiBaSjQD4UMc7eBzL7GL"></div>
                                </div>
                            </div>

                            <!-- Submit Button -->
                            <div class="col-12 text-start pt-2">
                                <asp:Button ID="btnSend" runat="server" CssClass="btn btn-submit-pnu" ValidationGroup="AddRequest" Text="<%$ Resources: PNUres, btnSend %>" OnClick="btnSend_Click" />
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
        function initOtherRoleToggle() {
            var roleContainer = document.querySelector('.open-data-role-list');
            var otherDiv = document.getElementById('divOtherContainer');
            if (!roleContainer || !otherDiv) return;

            function updateVisibility() {
                var checkboxes = roleContainer.querySelectorAll('input[type="checkbox"]');
                var isOtherSelected = false;
                for (var i = 0; i < checkboxes.length; i++) {
                    if (checkboxes[i].value === 'Other' && checkboxes[i].checked) {
                        isOtherSelected = true;
                        break;
                    }
                }
                otherDiv.style.display = isOtherSelected ? 'block' : 'none';
            }

            roleContainer.addEventListener('change', updateVisibility);
            updateVisibility();
        }

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', initOtherRoleToggle);
        } else {
            initOtherRoleToggle();
        }
    })();
</script>
