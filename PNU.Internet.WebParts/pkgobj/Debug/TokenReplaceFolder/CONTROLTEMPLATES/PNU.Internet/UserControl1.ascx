<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControl1.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.UserControl1" %>




<style>

strong {
    padding-bottom: 20px;
    /* padding: 10px; */
    padding-top: 20px;
}

</style>

      

<div id="form">


    <div class="row">
        <div class="col-12 col-md-6 mb-3">
            <span id="lblName" class="form-label required">اسم مقدم الطلب</span>
            <input name="txtName" type="text" maxlength="150" id="txtName" class="form-control">
            <span id="RegxNametxtName" class="requiredMsg" style="display: none;"></span>
            <span id="RequiredFieldValidator1" class="requiredMsg" style="display: none;">حقل مطلوب</span>
        </div>
        <div class="form-group col-md-6">
            <span id="Label6" class="form-label required">المنطقة التي تم تقديم الطلب منها</span>
            <select name="ddlCity1" id="ddlCity1" class="form-control">
                <option value="-1">اختر</option>
                <option value="الرياض">منطقة الرياض</option>
                <option value="مكة المكرمة">منطقة مكة المكرمة</option>
                <option value="المدينة المنورة">منطقة المدينة المنورة</option>
                <option value="القصيم">منطقة القصيم</option>
                <option value="الشرقية">المنطقة الشرقية</option>
                <option value="عسير">منطقة عسير</option>
                <option value="تبوك">منطقة تبوك</option>
                <option value="حائل">منطقة حائل</option>
                <option value="الحدود الشمالية">منطقة الحدود الشمالية</option>
                <option value="جازان">منطقة جازان</option>
                <option value="نجران">منطقة نجران</option>
                <option value="منطقة الباحة">منطقة الباحة</option>
                <option value="الجوف">منطقة الجوف</option>

            </select>
            <span id="RequiredFieldValidator15" class="requiredMsg" style="display: none;">حقل مطلوب</span>

        </div>
        <div class="col-12 col-md-6 mb-3">
            <span id="mobileNo" class="form-label">رقم الهاتف النقال</span>
            <input name="txtmobileNo" type="text" maxlength="10" id="txtmobileNo" class="form-control" style="margin-top: 4px;">
            <span id="RegxNotxtRegestrationNo" class="requiredMsg" style="display: none;"></span>
        </div>
        <div class="form-group col-md-6">
            <span id="Label3" class="form-label required">البريد الإلكتروني</span>
            <input name="textemail" type="text" maxlength="100" id="textemail" class="form-control">
            <span id="RegularExpressionValidator14" class="requiredMsg" style="display: none;">يرجى التحقق من صحة الايميل</span>
            <span id="RequiredFieldValidator18" class="requiredMsg" style="display: none;">حقل مطلوب</span>
        </div>
        <div class="col-12 mb-3">
            <span id="lblMessage" class="form-label required">غرض استخدام البيانات</span>
            <textarea name="txtMessage" rows="2" cols="20" onchange="javascript:setTimeout('__doPostBack(\'txtMessage\',\'\')', 0)" id="txtMessage" class="form-control"></textarea>
            <span id="RFVtxtMessage" class="requiredMsg" style="display: none;">حقل مطلوب</span>
            <span id="RegularExpressionValidator1" class="requiredMsg" style="display: none;">يمكنك فقط كتابة الأحرف العربة والأنجليزية </span>

        </div>
        <div class="col-12 col-md-6 mb-3">
            <span id="Label2" class="form-label required">العنوان</span>
            <input name="txtaddress" type="text" id="txtaddress" class="form-control">
            <span id="RequiredFieldValidator17" class="requiredMsg" style="display: none;">حقل مطلوب</span>

        </div>

        <div class="col-12 mb-3">
            <span id="Label1" class="form-label required">تفاصيل الطلب</span>
            <textarea name="txtdetails" rows="2" cols="20" id="txtdetails" class="form-control"></textarea>
            <span id="RequiredFieldValidator16" class="requiredMsg" style="display: none;">حقل مطلوب</span>
            <span id="RegularExpressionValidator13" class="requiredMsg" style="display: none;">يمكنك فقط كتابة الأحرف العربة والأنجليزية </span>

        </div>




    </div>





    <div class="col-12 col-md-12 mb-3">
        <label for="captcha" id="lblCapthca" class="form-label required"></label>
        <div class="captcha">
            <input name="txtCaptcha" type="text" maxlength="5" id="txtCaptcha" class="form-control" placeholder="الرجاء إدخال النص الظاهر في الصورة" autocomplete="off">

            <div class="captcha">


                <img src="/_LAYOUTS/15/PNU.Internet/Captcha.aspx?w=200&amp;h=50&amp;f=20&amp;d=Sun Aug 13 2023 11:33:34 GMT+0300 (Arabian Standard Time)" id="captchaimg"><a style="cursor: pointer" class="refresh-captcha"><img src="/Style%20Library/Portal/images/refresh.svg" alt="refresh"></a>

            </div>
            <script type="text/javascript">
                $('.captcha .refresh-captcha').click(RunCaptcha);
                function RunCaptcha() {
                    var d1 = new Date();
                    var img = document.getElementById('captchaimg');
                    img.src = '/_LAYOUTS/15/PNU.Internet/Captcha.aspx?w=200&h=50&f=20&d=' + d1;
                }
                RunCaptcha();
            </script>

            <span id="RequiredFieldValidator2" class="requiredMsg" style="display: none;">RequiredFieldValidator</span>
            <br>
        </div>
    </div>

    <div class="contact__form__footer">
        <input type="submit" name="btnSend" value="إرسال" onclick="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;btnSend&quot;, &quot;&quot;, true, &quot;AddRequest&quot;, &quot;&quot;, false, false))" id="btnSend" class="btn btn-primary">
    </div>

</div>
