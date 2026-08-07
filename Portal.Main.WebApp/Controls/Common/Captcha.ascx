<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Captcha.ascx.cs" Inherits="Portal.Main.WebApp.Controls.Common.Captcha" %>

<div style="width: 100%;">
    <img id="captchaImage" src="" class="captcha" />
    <img Style="cursor: pointer;" runat="server" id="img_refresh_captcha" src="/Style%20Library/Portal/images/refresh.png" meta:resourcekey="imgResource" class="refresh_captcha" onclick="RunCaptcha();"/>
</div>

<div class="btn_captcha">
    <asp:TextBox runat="server" ID="txtCaptcha" MaxLength="6" meta:resourcekey="txtCaptchaResource" autocomplete="off" CssClass="form-control"></asp:TextBox>
    <asp:RequiredFieldValidator  CssClass="requiredMsg" ID="rvtxtCaptcha" Display="Dynamic" runat="server" ControlToValidate="txtCaptcha" meta:resourcekey="vrequiredResource"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator  CssClass="requiredMsg" ID="rxptxtCaptcha" ValidationExpression="^[٠-٩ 0-9 a-z A-Z ء,ة,آ,أ-ي'.\s]{1,1000}$" runat="server" ControlToValidate="txtCaptcha" Display="Dynamic" meta:resourcekey="vrequiredResource"></asp:RegularExpressionValidator>
    <asp:CustomValidator runat="server" ID="cvCaptcha" OnServerValidate="cvCaptcha_ServerValidate" CssClass="requiredMsg" ControlToValidate="txtCaptcha" meta:resourcekey="cvCaptchaResource1" />   
</div>

<script type="text/javascript">
	function RunCaptcha() {
        var d1 = new Date();
        var imgC = document.getElementById('captchaImage');
        imgC.src ='/_LAYOUTS/15/LCGPA/Portal/Captcha.aspx?' + d1;
    }
	RunCaptcha();
</script>
