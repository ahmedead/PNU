<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PressContactus.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Contactus.PressContactus" %>


<style>
    .user_card {
  /*height: 400px;
  width: 350px;*/
  margin-top: auto;
  margin-bottom: auto;
  background: rgba(72, 65, 65, 0.7);
  position: relative;
  display: flex;
  justify-content: center;
  flex-direction: column;
  padding: 10px;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
  -webkit-box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
  -moz-box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
  border-radius: 20px;
  box-shadow: 0 15px 25px rgba(0,0,0,0.5);
}

</style>
<div style="font-family: PNU">


    <div>
        <div id="dvMsg" runat="server" visible="false">
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
        <div class="user_card" >

        
            <div>


                <div class="input-group mb-3" style="width: 100%;">
                    <div class="input-group-append">
                        <span class="input-group-text"><i class="fas fa-user"></i></span>
                    </div>

                    <asp:TextBox ID="TextBoxName" runat="server" placeholder="الإسم" MaxLength="150" CssClass="form-control" AutoCompleteType="Search" AutoPostBack="True" ForeColor="Black"></asp:TextBox>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TextBoxName" ID="RegularExpressionValidator3" ValidationExpression="^[\s\S]{5,50}$" runat="server" ErrorMessage="يجب أن لا يزيد عدد الاحرف عن 150 " ValidationGroup="1" Visible="False">*</asp:RegularExpressionValidator>


                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب ملء حقل الإسم" Text="*"
                        ControlToValidate="TextBoxName" ForeColor="#FFA6A6" ValidationGroup="1"></asp:RequiredFieldValidator>

                </div>
                <div class="input-group mb-3" style="width: 100%;">
                    <div class="input-group-append">
                        <span class="input-group-text"><i class="fas fa-home"></i></span>
                    </div>

                    <asp:TextBox ID="TextBoxSide" runat="server" placeholder="الجهة" MaxLength="150" CssClass="form-control" AutoCompleteType="Search" AutoPostBack="True" ForeColor="Black"></asp:TextBox>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TextBoxSide" ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{5,50}$" runat="server" ErrorMessage="يجب أن لا يزيد عدد الاحرف عن 150 " ValidationGroup="1" Visible="False">*</asp:RegularExpressionValidator>


                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="يجب ملء حقل الإسم" Text="*"
                        ControlToValidate="TextBoxSide" ForeColor="#FFA6A6" ValidationGroup="1"></asp:RequiredFieldValidator>

                </div>



                <div class="input-group mb-3" style="width: 100%;">
                    <div class="input-group-append">
                        <span class="input-group-text"><i class="fas fa-at"></i></span>
                    </div>
                    <%--<input type="text" name="" class="form-control input_user" value="" placeholder="اسم المستخدم">--%>
                    <asp:TextBox ID="TextBoxEmail" runat="server" placeholder="البريد الإلكتروني" CssClass="form-control" AutoCompleteType="Search" AutoPostBack="True" ForeColor="Black"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="يجب ملء حقل البريد الإلكتروني" Text="*"
                        ControlToValidate="TextBoxEmail" ForeColor="#FFA6A6" ValidationGroup="1"></asp:RequiredFieldValidator>


                    <asp:RegularExpressionValidator ID="EmailAddressFormatValidator" runat="server"
                        ControlToValidate="TextBoxEmail" ErrorMessage="صيغة البريد غير صحيحة" Text="*"
                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="#FFA6A6" ValidationGroup="1">              
                    </asp:RegularExpressionValidator>

                </div>


                <div class="input-group mb-3" style="width: 100%;">
                    <div class="input-group-append">
                        <span class="input-group-text"><i class="fas fa-mobile-alt"></i></span>
                    </div>
                    <%--<input type="password" name="" class="form-control input_pass" value="" placeholder="كلمة السر">--%>
                    <asp:TextBox ID="TextBoxMobile" runat="server" placeholder="رقم الجوال" CssClass="form-control" MaxLength="10" ForeColor="Black"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب ملء رقم الجوال"
                        ControlToValidate="TextBoxMobile" ForeColor="#FFA6A6" ValidationGroup="1" Text="*"></asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                        ErrorMessage="رقم الجوال مكون من 10 خانات ويبدأ بـ 05" ControlToValidate="TextBoxMobile"
                        ValidationExpression="^(05)([0-9]{8})$" ForeColor="#FFA6A6" ValidationGroup="1" Text="*">
                    </asp:RegularExpressionValidator>



                </div>

                <div class="input-group mb-2" style="width: 100%;">
                    <div class="input-group-append">
                        <span class="input-group-text"><i class="fas fa-tasks"></i></span>
                    </div>

                    <asp:TextBox ID="TextBoxDesc" runat="server" placeholder="الرسالة" CssClass="form-control" MaxLength="10" TextMode="MultiLine" ForeColor="Black"></asp:TextBox>


                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="يجب كتابة الاستفسار"
                        ControlToValidate="TextBoxDesc" ForeColor="#FFA6A6" ValidationGroup="1" Text="*"></asp:RequiredFieldValidator>


                </div>


                <div class="d-flex justify-content-center mt-3 login_container">
                    <%--<button href="login.html" name="button" class="btn login_btn">تسجيل الدخول</button>--%>
                    <asp:Button ID="btn_submit" runat="server" Text="إرسال" CssClass="btn login_btn" ValidationGroup="1" />

                </div>
                
            </div>



        </div>



    </div>
</div>
