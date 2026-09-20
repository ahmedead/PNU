<%@ Page Language="C#" %>
<%
    // صفحة "غير مصرح" مرئية للمستخدم: الدلالة الصحيحة 403 وليست 401،
    // لأن 401 الحقيقي يتطلب ترويسة WWW-Authenticate ولا يعرض HTML.
    // ملاحظة للنشر: إن كانت هذه الصفحة موصولة كمعالج 401 في IIS فراجع مسار
    // المصادقة قبل الاعتماد، وأعِد الحالة إلى 401 عند الحاجة.
    Response.StatusCode = 403;
    Response.StatusDescription = "Forbidden";
    Response.TrySkipIisCustomErrors = true;
    Response.Cache.SetCacheability(HttpCacheability.NoCache);
%>
<!DOCTYPE html>
<html lang="ar">
<head>
    <title>غير مصرح بالوصول (403) | جامعة الأميرة نورة بنت عبد الرحمن</title>
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
                <div class="pnf"> ليس لديك صلاحيات</div>
                <div class="homelink">عذراً ، ليس لديك الصلاحية لفتح هذه الصفحة.<br /><a href="/ar">الصفحة الرئيسية</a></div>
            </div>
            <div class="int_body_404_en">
                <div class="pnf">Access denied </div>
                <div class="homelink">You do not have permission to access this page <br /><a href="/en">Home Page</a></div>
            </div>
            <div class="clr"></div>
        </div>
    </div>
</body>
</html>
