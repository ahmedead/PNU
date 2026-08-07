<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAboutPNU.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.ucAboutPNU" %>


<main id="main-content" class="dga-main-body" tabindex="-1">

        <section class="py-5" data-aos="fade-up" aria-labelledby="reasons-section-title">
            <div class="container">
                <div class="row g-4 g-xl-5 align-items-center">
                    <div class="col-12 col-lg-12">
                        <h2 id="reasons-section-title" class="mb-4"> تاريخ الجامعة
                        </h2>
                        <p class="mb-4">

                            شهد تعليم المرأة في المملكة العربية السعودية اهتمامًا كبيرًا مكّنها من تحقيق إنجازات مميزة
                            محليًا وعالميًا، حيث برزت نماذج نسائية رائدة في مختلف مجالات العلم والمعرفة.

                            وتُعد جامعة الأميرة نورة بنت عبد الرحمن من أبرز ثمار هذا الاهتمام؛ إذ بدأت مسيرة تعليم
                            المرأة مبكرًا بإنشاء أول كلية تربوية للبنات عام 1970م، ثم توسع التعليم ليشمل عشرات الكليات
                            في مختلف مناطق المملكة. وفي عام 1427هـ صدر الأمر الملكي بإنشاء أول جامعة متكاملة للبنات في
                            الرياض، وتم تفعيلها عام 1428هـ.

                            وفي عام 1429هـ، وُضع حجر الأساس للمدينة الجامعية، ليُطلق عليها لاحقًا اسم "جامعة الأميرة
                            نورة بنت عبد الرحمن"، تخليدًا لاسم شقيقة الملك عبد العزيز -رحمه الله-، لتصبح اليوم صرحًا
                            علميًا رائدًا يعكس تمكين المرأة ودورها في التنمية.</p>

                        <div class="row g-3">
                            <div class="col-12 col-md-4">
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column placeholder-glow h-100">
                                        <div class="icon-container">
                                            <span class="d-inline-flex fs-3">
                                                <i class="hgi hgi-stroke hgi-target-01" aria-hidden="true"></i>
                                            </span>
                                        </div>
                                        <div class="flex-grow-1">
                                            <h3 class="card-title">البداية الأكاديمية</h3>
                                            <p class="card-text">إنشاء أول كلية تربوية للبنات إيذانًا بانطلاقة المسار
                                                الأكاديمي المنظم لتعليم المرأة.</p>
                                        </div>
                                        <div class="mt-auto d-flex flex-column gap-3">
                                            <small class="d-flex gap-2 align-items-center">
                                                <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                <time datetime="1970">1390 هـ / 1970 م</time>
                                            </small>
                                        </div>
                                    </div>
                                </article>
                            </div>
                            <div class="col-12 col-md-4">
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column placeholder-glow h-100">
                                        <div class="icon-container">
                                            <span class="d-inline-flex fs-3">
                                                <i class="hgi hgi-stroke hgi-hierarchy" aria-hidden="true"></i>
                                            </span>
                                        </div>
                                        <div class="flex-grow-1">
                                            <h3 class="card-title">تأسيس الجامعة</h3>
                                            <p class="card-text">صدور الأمر الملكي بإنشاء أول جامعة للبنات بالرياض تحت
                                                إشراف وزارة التعليم العالي.</p>
                                        </div>
                                        <div class="mt-auto d-flex flex-column gap-3">
                                            <small class="d-flex gap-2 align-items-center">
                                                <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                <time datetime="2006">1427 هـ</time>
                                            </small>
                                        </div>
                                    </div>
                                </article>
                            </div>
                            <div class="col-12 col-md-4">
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column placeholder-glow h-100">
                                        <div class="icon-container">
                                            <span class="d-inline-flex fs-3">
                                                <i class="hgi hgi-stroke hgi-chart" aria-hidden="true"></i>
                                            </span>
                                        </div>
                                        <div class="flex-grow-1">
                                            <h3 class="card-title">التفعيل والمدينة الجامعية</h3>
                                            <p class="card-text">تفعيل الجامعة، ثم وضع حجر الأساس للمدينة الجامعية
                                                واعتماد
                                                اسمها الحالي.</p>
                                        </div>
                                        <div class="mt-auto d-flex flex-column gap-3">
                                            <small class="d-flex gap-2 align-items-center">
                                                <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                <time datetime="2007/2008">1428 - 1429 هـ</time>
                                            </small>
                                        </div>
                                    </div>
                                </article>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-lg-12 d-none d-lg-block">
                        <figure class="figure w-100 mb-0">
                            <div class="overflow-hidden rounded-3 shadow-sm">
                                <picture>
                                    <img class="w-100  object-fit-cover"
                                        alt="واجهة من مرافق جامعة الأميرة نورة بنت عبد الرحمن" loading="eager"
                                        fetchpriority="high" decoding="async"
                                        src="/style%20library/dga/public/images/hero/hero-library-lg.avif">
                                </picture>
                            </div>
                            <figcaption class="figure-caption mt-3 mb-0">مشهد من مرافق الجامعة يعكس اتساع الحرم الجامعي
                                والطابع
                                المعماري للمباني الرئيسة.</figcaption>
                        </figure>
                    </div>
                </div>
            </div>
        </section>

        <section class="gray colored-section py-5" data-aos="fade-up">
            <div class="container">
                <div class="mb-4">
                    <div class="d-flex justify-content-between align-items-start gap-2">
                        <h2 class="mb-0">مرتكزات الجامعة</h2>

                    </div>
                    <p class="mb-0 mt-3">بطاقات مختصرة تلخص المحاور الأساسية التي تتكرر في الصفحات المرجعية المرتبطة
                        بالجامعة.</p>
                </div>
                <div class="row g-4">
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <span class="d-inline-flex fs-3">
                                        <i class="hgi hgi-stroke hgi-target-01" aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div>
                                    <h3 class="card-title">الرؤية</h3>
                                    <p class="card-text">هوية أكاديمية ومعرفية تقود إلى أثر مؤسسي ومجتمعي أوسع.</p>
                                </div>


                            </div>
                        </article>
                    </div>
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <span class="d-inline-flex fs-3">
                                        <i class="hgi hgi-stroke hgi-globe" aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div>
                                    <h3 class="card-title">الرسالة</h3>
                                    <p class="card-text">تجربة جامعية موثوقة تدعم التعليم والبحث والخدمة المجتمعية.</p>
                                </div>


                            </div>
                        </article>
                    </div>
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <span class="d-inline-flex fs-3">
                                        <i class="hgi hgi-stroke hgi-star" aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div>
                                    <h3 class="card-title">القيم</h3>
                                    <p class="card-text">التميز والاعتزاز بالهوية والمسؤولية والتعاون والشفافية.</p>
                                </div>


                            </div>
                        </article>
                    </div>
                </div>
            </div>
        </section>
        
</main>