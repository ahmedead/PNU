<%@ Page Language="C#" %>
<%
    // خطأ خادم حقيقي: يجب أن يبقى 500 حتى يفهم Googlebot أنه عارض مؤقت.
    // إعادة 404 هنا تعني شطب الصفحة من الفهرس عند أي انقطاع مؤقت.
    Response.StatusCode = 500;
    Response.StatusDescription = "Internal Server Error";
    Response.TrySkipIisCustomErrors = true;
    Response.Cache.SetCacheability(HttpCacheability.NoCache);
%>
<!DOCTYPE html>
<html lang="ar">
<head>
    <title>خطأ غير متوقع (500) | جامعة الأميرة نورة بنت عبد الرحمن</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="robots" content="noindex, follow" />
    <link rel="shortcut icon" href="/Style%20Library/Portal/images/favicon.ico" type="image/vnd.microsoft.icon" id="favicon">
    <meta name="viewport" content="width=device-width, height=device-height, initial-scale=0.7, maximum-scale=1">
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main_div">
        <div class="header">
			<a class="logo" href="/ar" title="الرئيسية"><img src="/_layouts/15/PNU_Theme/ar-sa/imgs/mainLogoAR.jpg"></a>
            <div class="clr"></div>
        </div>
        <div class="econts">
            <div class="int_body_404_ar">
                <div class="pnf"> خطأ غير متوقع</div>
                <div class="homelink">عذراً ، لقد حدث خطأ غير متوقع.<br /><a href="/ar">الصفحة الرئيسية</a></div>
            </div>
            <div class="int_body_404_en">
                <div class="pnf">Unexpected Error</div>
                <div class="homelink">Sorry, unexpected error occured !<br /><a href="/en">Home Page</a></div>
            </div>
            <div class="clr"></div>
        </div>
    </div>
</body>
</html>
