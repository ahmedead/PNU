<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Controls/Common/Captcha.ascx" TagPrefix="uc1" TagName="Captcha" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOpenDataRequest.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms.ucOpenDataRequest" %>



<style>
.col-md-12 {
    padding: 15px;
}
</style>
<div class="the-message p-2">
    <div class="mt-3 px-md-5 px-0">
	 

<asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green" Font-Size="Large" Visible="false" />
<asp:Label ID="lblException" runat="server" ForeColor="Red" Font-Size="Large" Visible="false" />

<asp:Panel ID="pnlData" runat="server">

    <%--<asp:UpdatePanel ID="updatepnl" runat="server">
        <ContentTemplate>--%>

            <section class=" pt-5 mt-5 mb-4">
                <div class="container">
                    <div class="row justify-content-center">

                        <div class="col-lg-8 mb-4 mb-lg-5 news-form">
                            <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                                <div class="row">
                                    <div class="col-md-12 text-start">
                                        <h4 class="card-title mb-4 fw-bold">طلب بيانات مفتوحة    
                                        </h4>
                                    </div>


                                    


                                    <div class="col-md-12">
                                        <div class="form-floating mb-4">
                                            <asp:TextBox ID="txtName" runat="server" placeholder="<%$ Resources: PNUres, txtName %>" onkeypress="return validateArabic(event)" CssClass="form-control" aria-label="<%$ Resources: PNUres, txtName %>"></asp:TextBox>
                                            <label for="floatingInputValue">
                                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, txtName %>" /></label>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtName" runat="server" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="form-floating mb-4">
                                            <asp:TextBox ID="txtEmail" runat="server" placeholder="<%$ Resources: PNUres, lblEmailAddress %>" CssClass="form-control" aria-label="<%$ Resources: PNUres, lblEmailAddress %>"></asp:TextBox>
                                            <label for="floatingInputValue">
                                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, lblEmailAddress %>" />
                                            </label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtEmail" runat="server" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <!-- Regular Expression Validator for Email Format -->
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                ControlToValidate="txtEmail"
                                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                                ErrorMessage="<%$Resources:PnuInternetResources, res_InvalidEmailFormat%>"
                                                CssClass="requiredMsg"
                                                ValidationGroup="AddRequest"
                                                Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>





                                    <div class="col-md-12">
                                        <div>
                                            <label for="chkRole">شريحة المستفيدين من البيانات المفتوحة</label>
                                            <asp:CheckBoxList ID="chkRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkRole_SelectedIndexChanged">
                                                <asp:ListItem Text="باحث" Value="باحث" ></asp:ListItem>
                                                <asp:ListItem Text="طالب" Value="طالب" ></asp:ListItem>
                                                <asp:ListItem Text="موظف" Value="موظف" ></asp:ListItem>
                                                <asp:ListItem Text="أخرى" Value="Other" ></asp:ListItem>
                                            </asp:CheckBoxList>
                                            <asp:TextBox ID="txtOther" runat="server" CssClass="form-control" placeholder="من فضلك أدخل قيمة في حالة اختيار أخرى"></asp:TextBox>

                                            <!-- Textbox for "Other" input (initially hidden) -->
                                            <div id="otherTextBoxContainer" style="display: none">
                                            </div>

                                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="false" />
                                            <%--<!-- CustomValidator to ensure at least one checkbox is selected -->
                                <asp:CustomValidator ID="cvRole" runat="server"
                                    ErrorMessage="من فضلك اختر شريحة واحدة علي الأقل"
                                    ValidationGroup="AddRequest"
                                    ClientValidationFunction="validateCheckBoxList"
                                    OnServerValidate="cvRole_ServerValidate"
                                    CssClass="requiredMsg"
                                    Display="Dynamic">
                                </asp:CustomValidator>--%>
                                        </div>
                                    </div>



                                    <div class="col-md-12">
                                        <div>
                                            <label for="lstDatasets">مجموعة البيانات المطلوبة</label>
                                            <asp:CheckBoxList ID="chkDatasets" runat="server">
                                                <asp:ListItem Text="الجامعة في أرقام" Value="الجامعة في أرقام"></asp:ListItem>
                                                <asp:ListItem Text="الموظفين" Value="الموظفين"></asp:ListItem>
                                                <asp:ListItem Text="الطالبات" Value="الطالبات"></asp:ListItem>
                                                <asp:ListItem Text="الأبحاث" Value="الأبحاث"></asp:ListItem>
                                                <asp:ListItem Text="الخريجات" Value="الخريجات"></asp:ListItem>
                                                <asp:ListItem Text="الخريجات في سوق العمل" Value="الخريجات في سوق العمل"></asp:ListItem>
                                                <asp:ListItem Text="براءات الاختراع" Value="براءات الاختراع"></asp:ListItem>
                                                <asp:ListItem Text="الجوائز" Value="الجوائز"></asp:ListItem>
                                                <asp:ListItem Text="الشؤون الرياضية" Value="الشؤون الرياضية"></asp:ListItem>
                                            </asp:CheckBoxList>

                                        </div>
                                    </div>





                                    <div class="col-md-12">

                                        <div class="form-floating mb-4">

                                            <asp:TextBox ID="txtOpenDataRequest" runat="server" placeholder="<%$ Resources: PNUres, txtOpenDataRequest %>" Style="height: 150px" CssClass="form-control" aria-label="<%$ Resources: PNUres, txtOpenDataRequest %>" TextMode="MultiLine"></asp:TextBox>

                                            <label for="floatingTextarea2">
                                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, txtOpenDataRequest %>" /></label>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtOpenDataRequest" runat="server" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>

                                    <div class="col-md-12">

                                        <div class="form-floating mb-4">

                                            <asp:TextBox ID="txtSuggest" runat="server" placeholder="<%$ Resources: PNUres, txtSuggest %>" Style="height: 150px" CssClass="form-control" aria-label="<%$ Resources: PNUres, txtSuggest %>" TextMode="MultiLine"></asp:TextBox>

                                            <label for="floatingTextarea2">
                                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, txtSuggest %>" /></label>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtSuggest" runat="server" ErrorMessage="<%$Resources:PnuInternetResources, res_RequiredField%>" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>

                                                                    <div class="col-md-12">
    <div class="mb-4">
        <uc1:Captcha runat="server" ValidationGroup="AddRequest" id="CaptchaControl" />
    </div>
</div>

                                    <div class="col-md-12 align-items-stretch text-center">
                                        <asp:Button ID="btnSend" runat="server" CssClass="btn btn-lg btn-primary  px-5  me-3  flex-fill w-100" ValidationGroup="AddRequest" Text="<%$ Resources: PNUres, btnSend %>" OnClick="btnSend_Click" />

                                    </div>







                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </section>

        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>

</asp:Panel>







    </div>
</div>




<%--
<script type="text/javascript">
    function validateArabic(event) {
        var key = event.keyCode || event.charCode; // Get the keycode of the pressed key
        var char = String.fromCharCode(key); // Get the character

        // Arabic letters range: 0x0600 - 0x06FF, 0x0750 - 0x077F, 0x08A0 - 0x08FF
        var regex = /^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\s]+$/;

        // If the character matches the Arabic letter range, allow it, otherwise block it
        if (regex.test(char)) {
            return true;
        } else {
            event.preventDefault(); // Block the key press if it's not Arabic
            return false;
        }
    }
</script>



<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#news2-tab-pane"]').tab('show');
        });
    }

</script>



<script>
    function validateCheckBoxList(source, args) {
        var checkBoxList = document.getElementById('<%= chkRole.ClientID %>');
        var checkboxes = checkBoxList.getElementsByTagName('input');
        var isValid = false;

        // Check if at least one checkbox is selected
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].checked) {
                isValid = true;
                break;
            }
        }

        args.IsValid = isValid;
    }
    $(document).ready(function () {
        $('#<%= chkRole.ClientID %> input[type="checkbox"]').change(function () {
            var otherCheckbox = $('#<%= chkRole.ClientID %> input[value="Other"]');
            var otherTextBoxContainer = $('#otherTextBoxContainer');
            var otherTextBox = $('#<%= txtOther.ClientID %>');

            // Show/hide the "Other" textbox and activate/deactivate the required validator
            if (otherCheckbox.is(':checked')) {
                otherTextBoxContainer.show();
                //requiredValidator.attr('enabled', 'true');  // Enable the validator
                //cvRole.removeAttr('enabled');  // Enable the validator

            } else {
                otherTextBoxContainer.hide();
                //requiredValidator.removeAttr('enabled');  // Disable the validator
                //cvRole.attr('enabled', 'true');  // Enable the validator
            }
        });
    });

</script>
--%>









