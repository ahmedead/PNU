<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEserviceBrow2.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.ucEserviceBrow2" %>



<link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/pagelayouts15.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/corev15.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/bootstrapARABIC.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/bootstrap-gridARABIC.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/pnu-defult.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/nhad-Style.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/swiper.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/arstyle.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/NoraCustom.css" />


    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/font-awesome.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/jqueryConfirm.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/breadcrumb.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/extraheader.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/services-catalog.css" />

    <script src="../../../../_layouts/15/services-catalog/assets/js/popper.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/bootstrap.min.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/counter.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/datatables.min.js"></script>
    <%--<script src="../../../../_layouts/15/services-catalog/assets/js/moment-with-locales.min.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/moment-hijri.js"></script>--%>
    <script src="../../../../_layouts/15/services-catalog/assets/js/jqueryConfirm.min.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/blank.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/Notify.js"></script>


    <script src="../../../../_layouts/15/services-catalog/assets/js/swiper.min.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/pnu.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/extraheader.js"></script>
<script src="../../../../_layouts/15/services-catalog/assets/js/jquery-3.5.0.min.js"></script>
    <div class="services-catalog ">
        <div class="jumbotron text-center">
            <div class="container mb-5">
                <div class="my-5">
                    <h1 class="display-4 pt-4 font-weight-bolder pb-4"> دليل الخدمات الالكترونية</h1>
                    <p class="lead "> ابحث هنا للحصول على الخدمات المرادة </p>
                </div>

                <div class="pt-5">
                    <div class="row justify-content-center ">
                        <div class="col-12 col-md-10 col-lg-8">
                            <div class="search">
                                <form class="card card-sm m-0 p-0 border-0 rounded-pill shadow-app" method="post">
                                    <div class="card-body row no-gutters align-items-center p-2 px-0">
                                        <div class="input-group align-items-center">
                                            <div class="mt-2 col-auto pr-0 d-none d-md-block">
                                                <i class="fas fa-search fa-2x h6 search-icon" aria-hidden="true"></i>
                                            </div>
                                            <div class="col pl-0">
                                                <input name="searchword"
                                                    class="form-control-lg form-control-borderless form-control search-form-control "
                                                    type="search" placeholder=" بحث عن الخدمات">
                                            </div>
                                            <div class="col-auto pr-1">
                                                <span class="input-group-btn">
                                                    <button
                                                        class="btn  btn-lg btn-success rounded-pill btn-search">ابحث</button>
                                                </span>
                                            </div>
                                        </div> <input type="hidden" name="task" value="search">
                                        <input type="hidden" name="option" value="com_search">
                                        <input type="hidden" name="Itemid" value="2888">
                                    </div>
                                </form>
                                <div class="pb-5 mb-5">
                                    <p class="text-muted mt-3  text-left">
                                        <span class="mx-2">
                                            اشهر الخدمات:
                                        </span>
                                        <br class="d-md-none d-block">
                                        <a href="#" class="badge badge-pill badge-success badge-search py-2 px-3 m-1">
                                            القبول الالحاقي
                                        </a>
                                        <a href="#" class="badge badge-pill badge-success badge-search py-2 px-3 m-1">
                                            طلب اخلاء طرف
                                        </a>
                                        <a href="#" class="badge badge-pill badge-success badge-search py-2 px-3 m-1">
                                            تسجيل الطالبات في النادي الرياضي
                                        </a>

                                    </p>


                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container audience mb-5 pb-5">
            <div class="row justify-content-center ">
                <div class="col-md-3 col-sm-6 col-xs-6 col-6">
                    <div id="all"
                        class="card card-sm m-0 p-0 border-0 rounded-pill shadow-app audience-card audience-selected ">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> الكل </h5>
                            <p class="card-text"> 16 خدمة </p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-6  col-6">
                    <div id="rptprod" class="card card-sm m-0 p-0 border-0 rounded-pill shadow-app audience-card">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> الطالبات </h5>
                            <p class="card-text"> 16 خدمة </p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-6 col-6">
                    <div id="RepFMem" class="card card-sm m-0 p-0 border-0 rounded-pill shadow-app audience-card">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> المنسوبات </h5>
                            <p class="card-text"> 16 خدمة </p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-6 col-6">
                    <div id="RepVisitor" class="card card-sm m-0 p-0 border-0 rounded-pill shadow-app audience-card">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> الزوار </h5>
                            <p class="card-text"> 16 خدمة </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container  mb-5 pb-5  filter-services" data-toggle="rptprod">
            <div class="bg-title m-3 mb-5">
                <h5> الخدمات الخاصة بالطالبات </h5>
            </div>
            <div class="row">
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> طلب اخلاء طرف </h5>
                        <p> تمكن هذه الخدمة الإلكترونية كافة طالبات جامعة الأميرة نورة بنت عبدالرحمن من إتمام إجراءات
                            إخلاء الطرف، آليا عبر بوابة الخدمات الإلكترونية. </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12  ">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> حجز استشارة قانونية </h5>
                        <p>تهدف هذه الخدمة إلى رفع مستوى الوعي القانوني وتعزيز الثقافة القانونية في المجتمع السعودي كما
                            وتقدم العون القانوني للفئات المستحقة وذلك عن طريق إتاحة التقدم لحجز موعد
                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12  ">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> القبول الالحاقي </h5>
                        <p> تمكن الطالبات اللاتي لم يتم ترشيحهن للقبول في الجامعة من التقديم على برامج الدبلوم وتتيح
                            للطالبة متابعة طلب التقديم والتعديل عليه
                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12  ">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> الاستعلام عن الرقم الجامعي </h5>
                        <p> تمكن طالبات الجامعة من الاستعلام عن الرقم الجامعي بحيث تتيح للطالبة المستجدة معرفة الرقم
                            الجامعي في حال لم تصلها الرسالة النصية ببياناتها الاكاديمية, كما تتيح لها طلب الحصول على
                            كلمة المرور للبريد الجامعي </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container  mb-5 pb-5 filter-services" data-toggle="RepFMem">
            <div class="bg-title m-3 mb-5">
                <h5> الخدمات الخاصة بالمنسوبات </h5>
            </div>
            <div class="row">
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> طلب اخلاء طرف </h5>
                        <p> تمكن هذه الخدمة الإلكترونية كافة طالبات جامعة الأميرة نورة بنت عبدالرحمن من إتمام إجراءات
                            إخلاء الطرف، آليا عبر بوابة الخدمات الإلكترونية. </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12  ">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> حجز استشارة قانونية </h5>
                        <p>تهدف هذه الخدمة إلى رفع مستوى الوعي القانوني وتعزيز الثقافة القانونية في المجتمع السعودي كما
                            وتقدم العون القانوني للفئات المستحقة وذلك عن طريق إتاحة التقدم لحجز موعد
                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12  ">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> القبول الالحاقي </h5>
                        <p> تمكن الطالبات اللاتي لم يتم ترشيحهن للقبول في الجامعة من التقديم على برامج الدبلوم وتتيح
                            للطالبة متابعة طلب التقديم والتعديل عليه
                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div class="container  mb-5 pb-5 filter-services" data-toggle="RepVisitor">
            <div class="bg-title m-3 mb-5">
                <h5> الخدمات الخاصة بالزوار </h5>
            </div>
            <div class="row">
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> طلب اخلاء طرف </h5>
                        <p> تمكن هذه الخدمة الإلكترونية كافة طالبات جامعة الأميرة نورة بنت عبدالرحمن من إتمام إجراءات
                            إخلاء الطرف، آليا عبر بوابة الخدمات الإلكترونية. </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12  ">
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="assets/imgs/logo_small.svg" title="">
                        </div>
                        <h5> حجز استشارة قانونية </h5>
                        <p>تهدف هذه الخدمة إلى رفع مستوى الوعي القانوني وتعزيز الثقافة القانونية في المجتمع السعودي كما
                            وتقدم العون القانوني للفئات المستحقة وذلك عن طريق إتاحة التقدم لحجز موعد
                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.html" class="serv-btn  serv-link"> تفاصيل
                                الخدمة</a>
                            <a target="_blank" class="serv-btn "> إبدأ الخدمة</a>

                        </div>
                    </div>
                </div>


            </div>
        </div>

    </div>
   <script>






        $(`.audience-card`).on("click", function (e) {
            $(`.audience-card`).removeClass('audience-selected');
            $(this).toggleClass('audience-selected');
            console.log($(this).attr("id"))
            $('.filter-services').removeClass('d-none');

            if ($(this).attr("id") !== 'all') {
                $('.filter-services').addClass('d-none');
                $('.filter-services[data-toggle="' + $(this).attr("id") + '"]').removeClass('d-none')
            }
        });
   </script>

