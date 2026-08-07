<%@ Assembly Name="EserviceList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7abd535f11cceb57" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceDetailsUserControl.ascx.cs" Inherits="EserviceList.ServiceDetails.ServiceDetailsUserControl" %>
    


 <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/swiper.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/arstyle.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/NoraCustom.css" />

<link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/pnu-defult.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/font-awesome.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/jqueryConfirm.min.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/breadcrumb.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/assets/css/extraheader.css" />
    <link rel="stylesheet" type="text/css" href="../../../../_layouts/15/services-catalog/services-catalog.css" />


    <link rel="icon" href="https://pnu.edu.sa/_layouts/15/PNU_Theme/ar-sa/imgs/fav.png" />

    <script src="../../../../_layouts/15/services-catalog/assets/js/jquery3.3.1.js"></script>
   <script src="../../../../_layouts/15/services-catalog/assets/js/popper.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/bootstrap.min.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/counter.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/datatables.min.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/moment-with-locales.min.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/moment-hijri.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/jqueryConfirm.min.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/blank.js"></script>

    <script src="../../../../_layouts/15/services-catalog/assets/js/Notify.js"></script>


    <script src="../../../../_layouts/15/services-catalog/assets/js/swiper.min.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/pnu.js"></script>
    <script src="../../../../_layouts/15/services-catalog/assets/js/extraheader.js"></script>
<style>

.container {
        max-width:1140px
    }
    .main-btnD {
        border: 1px solid #999999;
        background-color: #cccccc;
        color: #666666;
        border-radius: 1rem;
        pointer-events:none;
        
       
    }
     .main-btnD:hover {
        border: 1px solid #999999;
        background-color: black;
        cursor:none!important;
        color: #666666;
        border-radius: 1rem;
        pointer-events:none;
       
    }

</style>

<style>
@font-face {
  src: url("../fonts/Cairo-Bold.ttf");
  font-family: CairoBold;
}
@font-face {
  src: url("../fonts/Cairo-Regular.ttf");
  font-family: CairoRegular;
}
@font-face {
  src: url("../fonts/almarai-v5-arabic-regular.ttf");
  font-family: Almarai;
}
body,
html {
  font-family: CairoRegular, sans-serif;
  color: #024d4c;
  direction: rtl;
}

.btnSeach  {
    color: #fff !important;
    font-size: 1.25rem !important;
}

.col-auto {
    -webkit-box-flex: 0;
    -ms-flex: 0 0 auto;
    flex: 0 0 auto;
    width: auto;
    max-width: none
}



span.mx-2 {
    font-family: CairoRegular, sans-serif;
    color: #024d4c;
    direction: rtl;
}

span {
    font-family: CairoRegular, sans-serif;
    color: #024d4c;
    direction: rtl;
}
.rounded-pill {
    border-radius: 2rem !important;
}

.card.card-sm.m-0.p-0.border-0.shadow-app {
    border-radius: 2rem;
}
p.lead , .txtInSearch{
  font-family: CairoRegular, sans-serif;
  color: #024d4c;
  direction: rtl;
}
.jumbotron {
    padding: 2rem 1rem;
    margin-bottom: 2rem;
    background-color: #e9ecef;
    border-radius: .3rem
}
p {
    margin-top: 0;
    margin-bottom: 1rem
}
.lead {
    font-size: 1.25rem;
    font-weight: 300
}

    .RepFlex
    {
         display:flex;
         flex-direction:row;
         flex-wrap:wrap;
    }
    .btnSeach, input[type=button]
    {
        background-color:#024D4C!important;
        border-radius:2rem!important;
        border-color:#28a745!important;
        box-shadow:none!important;
        padding: 0.5rem 1rem!important;
        font-size: 1.25rem!important;
        line-height: 1.5!important;
        cursor:pointer;
    }
    #Search, input[type=button]
    {
         background-color:#024D4C!important;
        border-radius:2rem!important;
        border-color:#28a745!important;
        box-shadow:none!important;
        padding: 0.5rem 1rem!important;
        font-size: 1.25rem!important;
        line-height: 1.5!important;
    }
   .txtInSearch
    {
        padding: 0.5rem 1rem;
    font-size: 1.25rem;
    line-height: 1.5;
    border-radius: 0.3rem;
    float:right;
    border:none!important;
    }
  
    @media (max-width: 800px) {
  .RepFlex {
    flex-direction: column;
  }
  
}


section.breadcrumb {
    display: none;
}
  

</style>

<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>


<div class="services-catalog ">
        <div class="jumbotron text-left">
            <div class="container mb-5">
                <div class="my-5">
                    <h1 class="display-4 pt-4 font-weight-bolder pb-4">
                        <asp:Label runat="server" ID="lblname" ></asp:Label>
                        <asp:Label runat="server" ID="lblquerys"></asp:Label>

                    </h1>
                    

                    <p class="lead ">


                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="index.html">الخدمات الالكترونية </a></li>
                            <li class="breadcrumb-item active font-weight-bolder" aria-current="page">
                                <asp:Label runat="server" ID="LabelBC" ></asp:Label>
                                
                            </li>
                        </ol>
                    </nav>
                    </p>
                </div>

            </div>
        </div>


        <div class="container  my-5 py-5 ">
            <div class="bg-title m-3 mb-5">
                <h5> 
                    <asp:Label runat="server" ID="LblSerTitle" ></asp:Label>

                </h5>
            </div>
            <div class="row justify-content-between">
                <div class="col-lg-7 col-md-12 col-sm-12 col-xs-12 mb-5">
                    <p class="text-justify">
                        <asp:Label runat="server" ID="lblDesc"></asp:Label>
                    </p>
                  
                    <i class="fas fa-external-link-alt"></i>
                     <a id="hreLevelAgr" class="mx-1" runat="server">اتفاقية مستوى الخدمة</a>


                </div>
                <div class="col-lg-4 col-md-12 col-sm-12 col-xs-12 mb-5">
                    
                    <a id="hreStart" style="color:white;" class="btn btn-lg btn-block p-3 main-btn" runat="server">إبدأ الخدمة</a>
                </div>
                <div class="row mt-3 ">
                    <div class="col-12">
                        <div class="card card-sm m-0 p-4 py-5  border-0 rounded-pill shadow-app  card-info mb-5">
                            <div class="row  text-center">
                                <div class="col-lg-3 col-md-12 col-sm-12 col-xs-12 mb-lg-0 mb-5">
                                    <h5 class="font-weight-bolder">
                                        <i class="fas fa-users mx-1"></i>
                                        الفئة المستهدفة
                                    </h5>
                                    <asp:Label CssClass="mb-0" ID="lbltarget" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-3 col-md-12 col-sm-12 col-xs-12 mb-lg-0 mb-5">
                                    <h5 class="font-weight-bolder">
                                        <i class="fas fa-users-cog  mx-1"></i>
                                        الجهة المسؤولة    
                                    </h5>
                                    <asp:Label runat="server" CssClass="mb-0" ID="lblResPar"></asp:Label>
                                </div>
                                <div class="col-lg-3 col-md-12 col-sm-12 col-xs-12 mb-lg-0 mb-5">
                                    <h5 class="font-weight-bolder">
                                        <i class="fas fa-clock mx-1"></i>
                                        وقت تنفيذ الخدمة
                                    </h5>
                                    <asp:Label runat="server" ID="lblDur" CssClass="mb-0"></asp:Label>
                                </div>
                                <div class="col-lg-3 col-md-12 col-sm-12 col-xs-12 mb-0">
                                    <h5 class="font-weight-bolder">
                                        <i class="fas fa-file-signature  mx-1"></i>
                                        قنوات تقديم الخدمة
                                    </h5>
                                    <asp:Label ID="lblChan" runat="server" CssClass="mb-0"></asp:Label>
                                </div>

                            </div>


                        </div>
                    </div>
                    <div class="col-lg-8 col-md-12 col-sm-12 col-xs-12">
                        <div class="card card-sm m-0 p-4 py-5  border-0 rounded-pill shadow-app  card-info mb-5 ">
                            <h5 class=" text-left mb-4 mx-2">
                                المستندات / البيانات المطلوبة
                            </h5>
                            <p id="preq1" runat="server" class="my-2 mx-2">
                                <i class="far fa-circle mx-2"></i>
                                <asp:Label runat="server" CssClass="my-2 mx-2" ID="LblReq1"></asp:Label>
                            </p>
                            <p id="preq2" runat="server" class="my-2 mx-2">
                                <i class="far fa-circle mx-2"></i>
                                <asp:Label runat="server" CssClass="my-2 mx-2" ID="LblReq2"></asp:Label>     
                            </p>
                            <p id="preq3" runat="server" class="my-2 mx-2">
                                <i class="far fa-circle mx-2"></i>
                                <asp:Label runat="server" CssClass="my-2 mx-2" ID="LblReq3"></asp:Label>
                            </p>


                        </div>
                    </div>

                    <div class="col-lg-4 col-md-12 col-sm-12 col-xs-12">
                        <div class="card card-sm m-0 p-4 py-4  border-0 rounded-pill shadow-app  card-info mb-5 d-flex flex-column">
                            <img  class="w-25 mt-3 mb-3" src="assets/imgs/guide.svg" title="" alt="">

                            <h5 class="text-left mb-2 mx-2">
                               دليل الاستخدام
                            </h5>
                            <p id="UserManP" runat="server" class="my-2 mx-2">
                                اضغط لتحميل دليل الاستخدام
                            </p>
                            <div class="service-btn-wrapper mt-3">
                                <a id="hrefUManual" style="color:white;" class="btn btn-md btn-block py-2 main-btn" runat="server">تحميل دليل الاستخدام</a>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>

    </div>



   
