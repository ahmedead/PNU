<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Captcha.ascx.cs" Inherits="PNU.Internet.Web.Controls.Common.Captcha" %>
<div class="captcha">
    <img src="" id="captchaimg" /><a style="cursor: pointer" class="refresh-captcha"><i class="far fa-redo"></i></a>  
    <asp:TextBox CssClass="form-control" ID="txtCaptcha" MaxLength="5" runat="server" placeholder="Please enter the text shown in the image" autocomplete="off" meta:resourcekey="TextBox1Resource1"></asp:TextBox>
    <asp:RequiredFieldValidator Display="Dynamic" CssClass="requiredMsg" ID="RequiredFieldValidator1" ControlToValidate="txtCaptcha" runat="server" ErrorMessage="RequiredFieldValidator" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
    <asp:CustomValidator Display="Dynamic" CssClass="requiredMsg" ID="captchaCustomValidator" OnServerValidate="captchaCustomValidator_ServerValidate" runat="server" meta:resourcekey="captchaCustomValidatorResource1"></asp:CustomValidator>
</div>
<script type="text/javascript">
    $('.captcha .refresh-captcha').click(RunCaptcha);
	function RunCaptcha() {
        var d1 = new Date();
        var img = document.getElementById('captchaimg');
        img.src ='/_LAYOUTS/15/PNU.Internet/Captcha.aspx?w=200&h=50&f=20&d=' + d1;
    }
	RunCaptcha();    
</script>
