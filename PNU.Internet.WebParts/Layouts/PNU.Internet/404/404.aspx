<%@ Page Language="C#" %>
<%
    Response.StatusCode = 404;
    Response.StatusDescription = "Not Found";
    Response.TrySkipIisCustomErrors = true;
    Response.Cache.SetCacheability(HttpCacheability.NoCache);
%>
<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>الصفحة غير موجودة (404) | جامعة الأميرة نورة بنت عبد الرحمن</title>
    <meta name="robots" content="noindex, follow" />
    <link rel="shortcut icon" href="/Style%20Library/DGA/public/favicon.ico" type="image/x-icon" id="favicon" />
    <link href="/_layouts/15/PNU.Internet/404/style.css" rel="stylesheet" type="text/css" />
    <style>
/* ==========================================================================
   Embedded Critical DGA Styles for 404.aspx (Self-Contained)
   ========================================================================== */
@import url('https://fonts.googleapis.com/css2?family=IBM+Plex+Sans+Arabic:wght@300;400;500;600;700&display=swap');

:root {
    --text-default: #161616;
    --text-secondary: #475467;
    --text-muted: #667085;
    --background-SA-Flag: #074D31;
    --background-SA-Flag-hover: #053b26;
    --text-oncolor-primary: #FFFFFF;
    --text-oncolor-secondary: rgba(255, 255, 255, 0.85);
    --border-oncolor-transparent-30: rgba(255, 255, 255, 0.30);
    --border-subtle: #EAECF0;
    --primary-green: #074D31;
    --primary-green-dark: #053B26;
    --font-family-base: 'IBM Plex Sans Arabic', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
}

*, *::before, *::after {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
}

html, body {
    height: 100%;
    margin: 0;
    padding: 0;
    direction: rtl;
    text-align: right;
    font-family: var(--font-family-base);
    background-color: #FFFFFF;
    color: var(--text-default);
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
}

body {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
}

#root {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    width: 100%;
}

.page-wrapper {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
}

a {
    color: inherit;
    text-decoration: none;
    transition: color 0.2s ease, opacity 0.2s ease;
}

img, svg {
    max-width: 100%;
}

/* Guard against unstyled SVG explosion */
svg {
    max-width: 24px;
    max-height: 24px;
}

/* Secondary Top Bar */
.Custom_header {
    width: 100%;
    background-color: #F8F9FA;
    border-bottom: 1px solid var(--border-subtle);
    font-size: 13px;
    color: var(--text-secondary);
}

.top-bar-container {
    max-width: 1400px;
    margin: 0 auto;
    padding: 8px 32px;
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.top-bar-items {
    display: flex;
    align-items: center;
    gap: 24px;
}

.top-bar-item {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    color: var(--text-secondary);
    font-weight: 400;
}

.top-bar-item svg {
    width: 18px !important;
    height: 18px !important;
    fill: #475467;
    flex-shrink: 0;
    display: inline-block;
}

.top-bar-actions {
    display: flex;
    align-items: center;
    gap: 12px;
}

.top-bar-btn {
    background: transparent;
    border: 1px solid #D0D5DD;
    color: #344054;
    border-radius: 6px;
    padding: 4px 10px;
    font-size: 12px;
    font-weight: 500;
    cursor: pointer;
    font-family: inherit;
    display: inline-flex;
    align-items: center;
    gap: 4px;
    transition: all 0.2s;
}

.top-bar-btn:hover {
    background: #EAECF0;
    color: var(--text-default);
}

/* Main Navigation */
.main-nav-header {
    background: #FFFFFF;
    border-bottom: 1px solid var(--border-subtle);
    position: sticky;
    top: 0;
    z-index: 100;
    box-shadow: 0 1px 3px rgba(16, 24, 40, 0.05);
}

.main-nav-container {
    max-width: 1400px;
    margin: 0 auto;
    padding: 14px 32px;
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.nav-brand {
    display: flex;
    align-items: center;
    gap: 16px;
}

.nav-brand img {
    height: 54px;
    width: auto;
    max-width: 220px;
    object-fit: contain;
    display: block;
}

.nav-menu {
    display: flex;
    align-items: center;
    gap: 28px;
    list-style: none;
}

.nav-menu-link {
    font-size: 15px;
    font-weight: 500;
    color: var(--text-default);
    padding: 8px 4px;
}

.nav-menu-link:hover {
    color: var(--primary-green);
}

.nav-actions {
    display: flex;
    align-items: center;
    gap: 12px;
}

.nav-lang-btn {
    border: 1px solid #D0D5DD;
    background: #FFF;
    color: #344054;
    padding: 8px 16px;
    border-radius: 6px;
    font-weight: 600;
    font-size: 14px;
    cursor: pointer;
    text-decoration: none;
    transition: all 0.2s;
}

.nav-lang-btn:hover {
    background: #F8F9FA;
    border-color: var(--primary-green);
    color: var(--primary-green);
}

/* Error Central Area */
main.error-main-area {
    flex: 1 0 auto;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 60px 20px;
    background: radial-gradient(circle at center, #FDFDFD 0%, #F8F9FA 100%);
}

.error-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    max-width: 580px;
    margin: 0 auto;
    text-align: center;
}

.error_Image {
    margin-bottom: 40px;
    max-width: 100%;
    animation: fadeInScale 0.6s cubic-bezier(0.16, 1, 0.3, 1);
}

.error_Image img {
    width: 380px !important;
    max-width: 100%;
    height: auto !important;
    margin: 0 auto;
    display: block;
}

.error_content {
    animation: fadeInUp 0.7s cubic-bezier(0.16, 1, 0.3, 1);
}

.error-title {
    color: var(--text-default);
    font-size: 32px;
    font-weight: 700;
    line-height: 44px;
    margin-bottom: 16px;
    letter-spacing: -0.01em;
}

.error-desc {
    color: var(--text-secondary);
    font-size: 18px;
    font-weight: 400;
    line-height: 30px;
    margin-bottom: 32px;
    max-width: 480px;
    margin-left: auto;
    margin-right: auto;
}

.error-actions {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 16px;
    flex-wrap: wrap;
    margin-bottom: 24px;
}

.dga-btn-primary {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    background-color: var(--background-SA-Flag);
    color: #FFFFFF !important;
    font-size: 16px;
    font-weight: 600;
    line-height: 24px;
    padding: 14px 36px;
    border-radius: 8px;
    text-decoration: none;
    box-shadow: 0 4px 14px rgba(7, 77, 49, 0.25);
    transition: all 0.25s cubic-bezier(0.16, 1, 0.3, 1);
    border: none;
    cursor: pointer;
}

.dga-btn-primary:hover {
    background-color: var(--background-SA-Flag-hover);
    transform: translateY(-2px);
    box-shadow: 0 6px 20px rgba(7, 77, 49, 0.35);
    color: #FFFFFF !important;
}

.dga-btn-secondary {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    background-color: #FFFFFF;
    color: var(--primary-green) !important;
    border: 1px solid var(--primary-green);
    font-size: 16px;
    font-weight: 600;
    line-height: 24px;
    padding: 14px 28px;
    border-radius: 8px;
    text-decoration: none;
    transition: all 0.2s ease;
}

.dga-btn-secondary:hover {
    background-color: #F0FDF4;
    border-color: var(--primary-green-dark);
    color: var(--primary-green-dark) !important;
}

.en-note {
    font-size: 14px;
    color: var(--text-muted);
    margin-top: 12px;
}

.en-note a {
    color: var(--primary-green);
    text-decoration: underline;
    font-weight: 500;
}

/* DGA National Footer */
.footer_ {
    margin-top: auto;
    width: 100%;
}

footer.dga-national-footer {
    background-color: var(--background-SA-Flag);
    color: var(--text-oncolor-primary);
    padding: 40px 80px 24px 80px;
}

.footer-main-grid {
    display: grid;
    grid-template-columns: repeat(12, 1fr);
    gap: 24px;
    padding-top: 16px;
    padding-bottom: 40px;
    margin-bottom: 48px;
    border-bottom: 1px solid var(--border-oncolor-transparent-30);
}

.footer-col {
    grid-column: span 3;
}

.footer-col-title {
    color: var(--text-oncolor-primary);
    font-size: 16px;
    font-weight: 600;
    line-height: 24px;
    padding-bottom: 10px;
    border-bottom: 1px solid var(--border-oncolor-transparent-30);
    margin-bottom: 16px;
}

.footer-links-list {
    display: flex;
    flex-direction: column;
    gap: 12px;
    list-style: none;
}

.footer-link {
    color: var(--text-oncolor-secondary);
    font-size: 14px;
    line-height: 22px;
    text-decoration: none;
    transition: all 0.2s;
}

.footer-link:hover {
    color: #FFFFFF;
    text-decoration: underline;
    padding-right: 4px;
}

.social-section-wrap {
    display: flex;
    flex-direction: column;
    gap: 28px;
}

.social-icons-row {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 10px;
}

.social-icon-btn {
    width: 40px;
    height: 40px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    border: 1px solid var(--border-oncolor-transparent-30);
    border-radius: 6px;
    color: #FFFFFF;
    background: transparent;
    cursor: pointer;
    transition: all 0.2s;
    text-decoration: none;
}

.social-icon-btn:hover {
    background: rgba(255, 255, 255, 0.15);
    border-color: #FFFFFF;
    transform: translateY(-2px);
}

.social-icon-btn svg {
    width: 20px !important;
    height: 20px !important;
    fill: #FFFFFF !important;
}

.footer-bottom-grid {
    display: grid;
    grid-template-columns: repeat(12, 1fr);
    gap: 24px;
    align-items: center;
}

.footer-bottom-info {
    grid-column: span 8;
}

.footer-bottom-logos {
    grid-column: span 4;
    display: flex;
    align-items: center;
    justify-content: flex-end;
    gap: 20px;
}

.footer-bottom-links {
    display: flex;
    align-items: center;
    gap: 20px;
    margin-bottom: 20px;
    flex-wrap: wrap;
}

.footer-bottom-links a {
    color: var(--text-oncolor-secondary);
    font-size: 13px;
    font-weight: 500;
}

.footer-bottom-links a:hover {
    color: #FFFFFF;
    text-decoration: underline;
}

.footer-copy-text {
    color: var(--text-oncolor-secondary);
    font-size: 13px;
    font-weight: 400;
    line-height: 22px;
    margin-bottom: 4px;
}

.footer-copy-text strong {
    font-weight: 600;
    color: #FFFFFF;
}

.footer-bottom-logos img {
    height: 44px;
    width: auto;
    object-fit: contain;
    filter: brightness(0) invert(1);
    opacity: 0.92;
    transition: opacity 0.2s;
    display: block;
}

.footer-bottom-logos img:hover {
    opacity: 1;
}

@keyframes fadeInScale {
    0% { opacity: 0; transform: scale(0.92); }
    100% { opacity: 1; transform: scale(1); }
}

@keyframes fadeInUp {
    0% { opacity: 0; transform: translateY(16px); }
    100% { opacity: 1; transform: translateY(0); }
}

/* Responsive */
@media (max-width: 1024px) {
    footer.dga-national-footer { padding: 32px 24px 20px 24px; }
    .footer-col { grid-column: span 6; }
    .footer-col.col-social { grid-column: span 12; }
    .footer-bottom-info { grid-column: span 12; }
    .footer-bottom-logos { grid-column: span 12; justify-content: flex-start; }
}

@media (max-width: 991px) {
    .nav-menu { display: none; }
    .top-bar-items { gap: 14px; }
}

@media (max-width: 640px) {
    .main-nav-container { padding: 12px 16px; }
    .top-bar-container { padding: 6px 16px; }
    .footer-col { grid-column: span 12; }
    .error-title { font-size: 24px; line-height: 34px; }
    .error-desc { font-size: 16px; line-height: 26px; }
    .error_Image img { width: 280px !important; }
    .dga-btn-primary { width: 100%; }
}
    </style>
</head>
<body style="overflow: auto;">
    <div id="root" style="height: auto; overflow: unset;">
        <div class="page-wrapper">
            
          
            <!-- Main Navigation Header -->
            <header class="main-nav-header" role="banner">
                <div class="main-nav-container">
                    <div class="nav-brand">
                        <a href="/ar" title="الانتقال إلى الصفحة الرئيسية - جامعة الأميرة نورة">
                            <img src="/_layouts/15/PNU.Internet/404/pnu-logo.svg" alt="شعار جامعة الأميرة نورة بنت عبد الرحمن" width="220" height="54" style="height:54px;width:auto;max-width:220px;" />
                        </a>
                    </div>
                    <nav class="nav-menu" aria-label="القائمة الرئيسية">
                        <a href="/ar" class="nav-menu-link">الرئيسية</a>
                        <a href="/ar/AboutUniversity/Pages/AboutLinks.aspx" class="nav-menu-link">عن الجامعة</a>
                        <a href="/ar/Faculties/Pages/AllCollegesNew.aspx" class="nav-menu-link">الكليات</a>
                        <a href="/ar/RegAdm/Pages/default.aspx" class="nav-menu-link">القبول والتسجيل</a>
                        <a href="/ar/Pages/Eservice.aspx" class="nav-menu-link">الخدمات الإلكترونية</a>
                        <a href="/ar/Pages/ContactUsForm.aspx" class="nav-menu-link">تواصل معنا</a>
                    </nav>
                    <div class="nav-actions">
                        <a href="/en" class="nav-lang-btn" title="English">English</a>
                    </div>
                </div>
            </header>

            <!-- Main Error Content Area -->
            <main class="error-main-area" role="main">
                <section class="error-container">
                    <!-- 404 Illustration -->
                    <div class="error_Image">
                        <img src="/_layouts/15/PNU.Internet/404/error.svg" alt="رمز الخطأ 404 - الصفحة غير موجودة" width="380" height="268" style="width:380px;max-width:100%;height:auto;margin:0 auto;" loading="eager" />
                    </div>

                    <!-- Error Text & Call to Actions -->
                    <div class="error_content">
                        <h1 class="error-title">الصفحة غير موجودة (404)</h1>
                        <p class="error-desc">عذراً، لم نستطع إيجاد الصفحة التي تبحث عنها. قد يكون الرابط المدخل غير صحيح، أو تم نقل الصفحة أو حذفها.</p>
                        
                        <div class="error-actions">
                            <a href="/ar" class="dga-btn-primary" title="العودة إلى الصفحة الرئيسية">
                                العودة إلى الصفحة الرئيسية
                            </a>
                            <a href="/ar/Search/Pages/SearchResults.aspx?q=" class="dga-btn-secondary" title="البحث في الموقع">
                                البحث في الموقع
                            </a>
                        </div>

                        <p class="en-note">
                            Sorry, the page you are looking for does not exist. &bull; <a href="/en">Back to Home</a>
                        </p>
                    </div>
                </section>
            </main>

            <!-- DGA National Portal Footer -->
            <div class="footer_">
                <footer class="dga-national-footer" role="contentinfo">
                    <div class="footer-main-grid">
                        
                        <!-- Col 1: Summary -->
                        <div class="footer-col">
                            <h3 class="footer-col-title">ملخص</h3>
                            <ul class="footer-links-list">
                                <li><a href="/ar/AboutUniversity/Pages/default.aspx" class="footer-link">حول جامعة الأميرة نورة</a></li>
                                <li><a href="/ar/Pages/PrivacyPolicy.aspx" class="footer-link">الخصوصية وشروط الاستخدام</a></li>
                                <li><a href="/ar/AboutUniversity/Pages/Support.aspx" class="footer-link">كيفية استخدام البوابة</a></li>
                                <li><a href="/ar/MediaCenter/News/Pages/default.aspx" class="footer-link">الأخبار والأحداث</a></li>
                                <li><a href="/ar/AboutUniversity/Pages/SLA.aspx" class="footer-link">إحصائيات اتفاقية مستوى الخدمة</a></li>
                            </ul>
                        </div>

                        <!-- Col 2: Important Links -->
                        <div class="footer-col">
                            <h3 class="footer-col-title">روابط مهمة</h3>
                            <ul class="footer-links-list">
                                <li><a href="https://www.my.gov.sa" target="_blank" rel="noopener noreferrer" class="footer-link">بوابة الخدمة الوطنية (Gov.sa)</a></li>
                                <li><a href="https://data.gov.sa" target="_blank" rel="noopener noreferrer" class="footer-link">البيانات الحكومية المفتوحة</a></li>
                                <li><a href="https://sdaia.gov.sa" target="_blank" rel="noopener noreferrer" class="footer-link">الاستراتيجية الوطنية للبيانات والذكاء الاصطناعي</a></li>
                                <li><a href="/ar/Eservices/Pages/default.aspx" class="footer-link">بوابة الخدمات الإلكترونية</a></li>
                                <li><a href="https://eservice.pnu.edu.sa" target="_blank" rel="noopener noreferrer" class="footer-link">بوابة المشاركة الإلكترونية</a></li>
                            </ul>
                        </div>

                        <!-- Col 3: Contact and Support -->
                        <div class="footer-col">
                            <h3 class="footer-col-title">الاتصال والدعم</h3>
                            <ul class="footer-links-list">
                                <li><a href="/ar/AboutUniversity/Pages/Support.aspx" class="footer-link">مركز الدعم والمساعدة</a></li>
                                <li><a href="/ar/Pages/ContactUsForm.aspx" class="footer-link">تواصل معنا</a></li>
                                <li><a href="https://tawasulnourah.pnu.edu.sa" target="_blank" rel="noopener noreferrer" class="footer-link">تواصل نورة</a></li>
                                <li><a href="https://eservice.pnu.edu.sa" target="_blank" rel="noopener noreferrer" class="footer-link">تقديم شكوى</a></li>
                                <li><a href="https://nazaha.gov.sa" target="_blank" rel="noopener noreferrer" class="footer-link">الإبلاغ عن الفساد (نزاهة)</a></li>
                            </ul>
                        </div>

                        <!-- Col 4: Social and Accessibility -->
                        <div class="footer-col col-social">
                            <div class="social-section-wrap">
                                <div>
                                    <h3 class="footer-col-title">تابعنا على</h3>
                                    <div class="social-icons-row">
                                        <!-- X / Twitter -->
                                        <a href="https://twitter.com/_PNU_KSA" target="_blank" rel="noopener noreferrer" class="social-icon-btn" aria-label="X - تويتر">
                                            <svg width="20" height="20" viewBox="0 0 24 24" style="width:20px;height:20px;display:inline-block;"><path d="M18.244 2.25h3.308l-7.227 8.26 8.502 11.24H16.17l-5.214-6.817L4.99 21.75H1.68l7.73-8.835L1.254 2.25H8.08l4.713 6.231zm-1.161 17.52h1.833L7.084 4.126H5.117z"/></svg>
                                        </a>
                                        <!-- YouTube -->
                                        <a href="https://www.youtube.com/channel/UC-0BTuZ46ApPXt6dWONTyGg" target="_blank" rel="noopener noreferrer" class="social-icon-btn" aria-label="يوتيوب">
                                            <svg width="20" height="20" viewBox="0 0 24 24" style="width:20px;height:20px;display:inline-block;"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
                                        </a>
                                        <!-- LinkedIn -->
                                        <a href="https://www.linkedin.com/school/princess-nourah-bint-abdulrahman-university/" target="_blank" rel="noopener noreferrer" class="social-icon-btn" aria-label="لينكد إن">
                                            <svg width="20" height="20" viewBox="0 0 24 24" style="width:20px;height:20px;display:inline-block;"><path d="M19 3a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h14m-.5 15.5v-5.3a3.26 3.26 0 0 0-3.26-3.26c-.85 0-1.84.52-2.28 1.3v-1.11h-2.79v8.37h2.79v-4.93c0-.77.62-1.4 1.39-1.4a1.4 1.4 0 0 1 1.4 1.4v4.93h2.75M6.46 8.76a1.45 1.45 0 0 0 1.46-1.45 1.45 1.45 0 0 0-1.46-1.45 1.46 1.46 0 0 0-1.47 1.45 1.46 1.46 0 0 0 1.47 1.45m1.39 9.74v-8.37H5.07v8.37h2.78z"/></svg>
                                        </a>
                                        <!-- Instagram -->
                                        <a href="https://www.instagram.com/_PNU_KSA/" target="_blank" rel="noopener noreferrer" class="social-icon-btn" aria-label="إنستغرام">
                                            <svg width="20" height="20" viewBox="0 0 24 24" style="width:20px;height:20px;display:inline-block;"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
                                        </a>
                                    </div>
                                </div>
                                <div>
                                    <h3 class="footer-col-title">أدوات الإتاحة والوصول</h3>
                                    <div class="social-icons-row">
                                        <button type="button" class="social-icon-btn" onclick="document.body.classList.toggle('high-contrast')" title="تباين عالٍ" aria-label="تباين عالٍ">
                                            <svg width="20" height="20" viewBox="0 0 24 24" style="width:20px;height:20px;display:inline-block;"><path d="M12 22c5.52 0 10-4.48 10-10S17.52 2 12 2 2 6.48 2 12s4.48 10 10 10zm1-17.93c3.94.49 7 3.85 7 7.93s-3.06 7.44-7 7.93V4.07z"/></svg>
                                        </button>
                                        <button type="button" class="social-icon-btn" onclick="window.print()" title="طباعة" aria-label="طباعة الصفحة">
                                            <svg width="20" height="20" viewBox="0 0 24 24" style="width:20px;height:20px;display:inline-block;"><path d="M19 8H5c-1.66 0-3 1.34-3 3v6h4v4h12v-4h4v-6c0-1.66-1.34-3-3-3zm-3 11H8v-5h8v5zm3-7c-.55 0-1-.45-1-1s.45-1 1-1 1 .45 1 1-.45 1-1 1zm-1-9H6v4h12V3z"/></svg>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <!-- Footer Bottom Bar -->
                    <div class="footer-bottom-grid">
                        <div class="footer-bottom-info">
                            <div class="footer-bottom-links">
                                <a href="/ar/Pages/Sitemap.aspx">خريطة الموقع</a>
                                <a href="/_layouts/15/PNU.Internet/Sitemap.aspx">RSS</a>
                                <a href="https://apps.apple.com" target="_blank" rel="noopener noreferrer">تطبيق الجوال</a>
                            </div>
                            <p class="footer-copy-text"><strong>جميع الحقوق محفوظة لجامعة الأميرة نورة بنت عبد الرحمن © <%= DateTime.Now.Year %></strong></p>
                            <p class="footer-copy-text">تم تطويره وصيانته بواسطة جامعة الأميرة نورة بنت عبد الرحمن</p>
                            <p class="footer-copy-text">تاريخ آخر تعديل: <%= DateTime.Now.ToString("dd/MM/yyyy") %></p>
                        </div>
                        <div class="footer-bottom-logos">
                            <img src="/_layouts/15/PNU.Internet/404/footerLogoAr.svg" alt="شعار هيئة الحكومة الرقمية" width="140" height="44" style="height:44px;width:auto;" loading="lazy" />
                            <img src="/_layouts/15/PNU.Internet/404/footerLogo2.png" alt="شعار رؤية المملكة 2030" width="140" height="44" style="height:44px;width:auto;" loading="lazy" />
                        </div>
                    </div>

                </footer>
            </div>

        </div>
    </div>

    <!-- Client-side Script for Date, Time and Accessibility -->
    <script>
        (function () {
            // Live Riyadh Date & Time
            function updateClock() {
                try {
                    var now = new Date();
                    var dateOpts = { day: 'numeric', month: 'long', year: 'numeric', timeZone: 'Asia/Riyadh' };
                    var timeOpts = { hour: '2-digit', minute: '2-digit', hour12: true, timeZone: 'Asia/Riyadh' };

                    var dateEl = document.getElementById('currentDateText');
                    var timeEl = document.getElementById('currentTimeText');

                    if (dateEl) {
                        dateEl.innerText = new Intl.DateTimeFormat('ar-SA', dateOpts).format(now);
                    }
                    if (timeEl) {
                        timeEl.innerText = new Intl.DateTimeFormat('ar-SA', timeOpts).format(now);
                    }
                } catch (e) {}
            }
            updateClock();
            setInterval(updateClock, 60000);

            // Font resize controls
            var currentZoom = 100;
            var btnPlus = document.getElementById('btnFontIncrease');
            var btnMinus = document.getElementById('btnFontDecrease');

            if (btnPlus) {
                btnPlus.addEventListener('click', function () {
                    if (currentZoom < 130) {
                        currentZoom += 10;
                        document.body.style.fontSize = currentZoom + '%';
                    }
                });
            }
            if (btnMinus) {
                btnMinus.addEventListener('click', function () {
                    if (currentZoom > 90) {
                        currentZoom -= 10;
                        document.body.style.fontSize = currentZoom + '%';
                    }
                });
            }
        })();
    </script>
</body>
</html>
