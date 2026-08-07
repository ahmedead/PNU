<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EserviceBrowUserControl.ascx.cs" Inherits="EserviceList.EserviceBrow.EserviceBrowUserControl" %>


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

    <script src="../../../../_layouts/15/services-catalog/assets/js/popper.js"></script>

    
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
<script src="../../../../_layouts/15/services-catalog/assets/js/jquery3.3.1.js"></script>
<style>

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

 <div class="services-catalog">
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
                                <div class="card card-sm m-0 p-0 border-0  shadow-app">
                                    <div class="card-body row no-gutters align-items-center p-2 px-0">
                                        <div class="input-group align-items-center">
                                            <div class="mt-2 col-auto pr-0 d-none d-md-block">
                                                <i class="fas fa-search fa-2x h6 search-icon" aria-hidden="true"></i>
                                            </div>
                                            <div class="col pl-0">
                                                <asp:TextBox ID="txtSearch" placeholder=" بحث عن الخدمات"
                                                    AutoCompleteType="None" type="search"
                                                    CssClass="txtInSearch form-control-lg form-control-borderless form-control search-form-control"
                                                    runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-auto px-2">
                                                <span class="input-group-btn">
                                                    <asp:LinkButton ID="Search" BackColor="#024D4C"
                                                        CssClass="btnSeach btn btn-lg btn-success  btn-search"
                                                        runat="server" Text="ابحث" OnClick="Search_Click1" />
                                                </span>
                                            </div>

                                        </div>

                                        <input type="hidden" name="task" value="search">
                                        <input type="hidden" name="option" value="com_search">
                                        <input type="hidden" name="Itemid" value="2888">
                                    </div>


                                </div>
                                <div>
                                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtSearch"
                                        ForeColor="Red" SetFocusOnError="true" ErrorMessage=" Restricted" ID="rfvname"
                                        ValidationExpression="[^~`!?؟@#$%\^&\*\(\)\-+=\\\|\}\]\{\['&quot;:?.>,</]+">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="pb-5 mb-5">
                                    <p class="text-muted mt-3  text-left">
                                        <span class="mx-2">
                                            اشهر الخدمات:
                                        </span>
                                        <br class="d-md-none d-block">


                                        <a id="tophref1" runat="server" href="#"
                                            class="badge badge-pill badge-success badge-search py-2 px-3 m-1">
                                            <asp:Label ID="Top1" runat="server"></asp:Label>
                                        </a>

                                        <a id="tophref2" runat="server" href="#"
                                            class="badge badge-pill badge-success badge-search py-2 px-3 m-1">
                                            <asp:Label ID="Top2" runat="server"></asp:Label>
                                        </a>

                                        <a id="tophref3" runat="server" href="#"
                                            class="badge badge-pill badge-success badge-search py-2 px-3 m-1">
                                            <asp:Label ID="Top3" runat="server"></asp:Label>
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
                    <div  data-toggle="all" class="card card-sm m-0 p-0 border-0  shadow-app audience-card audience-selected ">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> الكل </h5>
                            <p class="card-text">
                            <asp:Label ID="lblTotal" CssClass="card-text" runat="server"></asp:Label>
                            خدمة
                            </p>
                            <%--<p class="card-text"> 16 خدمة </p>--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-6  col-6">
                    <div  data-toggle="rptprod" class="card card-sm m-0 p-0 border-0  shadow-app audience-card">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> الطالبات </h5>
                            <p class="card-text">
                            <asp:Label ID="lblStNser" CssClass="card-text" runat="server"></asp:Label>
                            خدمة
                            </p>
                            
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-6 col-6">
                    <div data-toggle="RepFMem" class="card card-sm m-0 p-0 border-0  shadow-app audience-card">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> المنسوبات </h5>
                             <p class="card-text">
                            <asp:Label ID="LblFaNSer" CssClass="card-text" runat="server"></asp:Label>
                            خدمة
                            </p>
                            <%--<p class="card-text"> 16 خدمة </p>--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-6 col-6">
                    <div data-toggle="RepVisitor" class="card card-sm m-0 p-0 border-0  shadow-app audience-card">
                        <i class="fas fa-check-circle p-2 text-right audience-check-circle"></i>
                        <div class="card-body">
                            <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" alt="..." class="w-25 border-0  mb-3">

                            <h5 class="card-title mb-1"> الزوار </h5>
                              <p class="card-text">
                            <asp:Label ID="LblVisN" CssClass="card-text" runat="server"></asp:Label>
                            خدمة
                            </p>
                            <%--<p class="card-text"> 16 خدمة </p>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
     
     <div class="container mb-5 pb-5 filter-services" data-toggle="rptprod" >
          <div class="bg-title m-3 mb-5">
                <h5> الخدمات الخاصة بالطالبات </h5>
            </div>
         <div class="RepFlex" style="">

         
     <asp:Repeater ID="rptprod" runat="server" > 
                          <HeaderTemplate>
                             
                              </HeaderTemplate>
         <ItemTemplate>
             <div class="" style="display:flex;flex-basis:33%;flex-direction:row;flex-wrap:wrap;">
             
                          <div class="container  mb-5 pb-5">
            
            
                
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" title="" alt="">
                        </div>
                        <h5>
                         <%#DataBinder.Eval(Container.DataItem,"ARServiceName") %>  
                           <%--  <%#DataBinder.Eval(Container.DataItem,"ID") %>  --%>
                            </h5>

                        <p>
                            <%#DataBinder.Eval(Container.DataItem,"Desc") %> 
                           <%-- تمكن هذه الخدمة الإلكترونية كافة طالبات جامعة الأميرة نورة بنت عبدالرحمن من إتمام إجراءات
                            إخلاء الطرف، آليا عبر بوابة الخدمات الإلكترونية.--%>

                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.aspx?eti=<%#DataBinder.Eval(Container.DataItem,"ID") %>" class="serv-btn  serv-link"> تفاصيل الخدمة</a>
                            <a target="_blank" href="<%#DataBinder.Eval(Container.DataItem,"URL") %>  " class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
              
            
              
                 </div>
       
        
             </ItemTemplate>

      
         
         </asp:Repeater>
             </div>
         </div>

     <!-- end of Student Services -->

      
     <div class="container mb-5 pb-5 filter-services" data-toggle="RepFMem" style="margin-top:-100px;" >
         <div class="bg-title m-3 mb-5">
                <h5> الخدمات الخاصة بالمنسوبات </h5>

            </div>
         <div class="RepFlex">    
     <asp:Repeater ID="RepFMem" runat="server" > 
                          <HeaderTemplate>
                             
                              </HeaderTemplate>
         <ItemTemplate>
             <div class="" style="display:flex;flex-basis:33%;flex-direction:row;flex-wrap:wrap;">
             
                          <div class="container  mb-5 pb-5">
            
            
                
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" title="" alt="">
                        </div>
                        <h5>
                         <%#DataBinder.Eval(Container.DataItem,"ARServiceName") %>  
                           <%--  <%#DataBinder.Eval(Container.DataItem,"ID") %>  --%>
                            </h5>

                        <p>
                            <%#DataBinder.Eval(Container.DataItem,"Desc") %> 
                           <%-- تمكن هذه الخدمة الإلكترونية كافة طالبات جامعة الأميرة نورة بنت عبدالرحمن من إتمام إجراءات
                            إخلاء الطرف، آليا عبر بوابة الخدمات الإلكترونية.--%>

                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.aspx?eti=<%#DataBinder.Eval(Container.DataItem,"ID") %>" class="serv-btn  serv-link"> تفاصيل الخدمة</a>
                            <a target="_blank" href="<%#DataBinder.Eval(Container.DataItem,"URL") %>  " class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
              
            
              
                 </div>
       
        
             </ItemTemplate>

      
         
         </asp:Repeater>
             </div>
         </div>
     <!-- End of Faculty -->

          <div class="container mb-5 pb-5 filter-services" data-toggle="RepVisitor" style="margin-top:-100px;">
         <div class="bg-title m-3 mb-5">
                <h5>الخدمات الخاصة بالزوار </h5>

            </div>
         <div class="RepFlex">    
     <asp:Repeater ID="RepVisitor"  runat="server" > 
                          <HeaderTemplate>
                             
                              </HeaderTemplate>
         <ItemTemplate>
             <div class="" style="display:flex;flex-basis:33%;flex-direction:row;flex-wrap:wrap;">
             
                          <div class="container  mb-5 pb-5">
            
            
                
                    <div class="service-card">
                        <div class="serv-icon">
                            <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" title="" alt="">
                        </div>
                        <h5>
                         <%#DataBinder.Eval(Container.DataItem,"ARServiceName") %>  
                           <%--  <%#DataBinder.Eval(Container.DataItem,"ID") %>  --%>
                            </h5>

                        <p>
                            <%#DataBinder.Eval(Container.DataItem,"Desc") %> 
                           <%-- تمكن هذه الخدمة الإلكترونية كافة طالبات جامعة الأميرة نورة بنت عبدالرحمن من إتمام إجراءات
                            إخلاء الطرف، آليا عبر بوابة الخدمات الإلكترونية.--%>

                        </p>
                        <div class="service-btn-wrapper">
                            <a target="_blank" href="service-details.aspx?eti=<%#DataBinder.Eval(Container.DataItem,"ID") %>" class="serv-btn  serv-link"> تفاصيل الخدمة</a>
                            <a target="_blank" href="<%#DataBinder.Eval(Container.DataItem,"URL") %>  " class="serv-btn "> إبدأ الخدمة</a>
                        </div>
                    </div>
                </div>
              
            
              
                 </div>
       
        
             </ItemTemplate>

      
         
         </asp:Repeater>
             </div>
         </div>

        

        

    </div>

   <script>






       $(`.audience-card`).on("click", function (e) {
           $(`.audience-card`).removeClass('audience-selected');
           $(this).toggleClass('audience-selected');
           console.log($(this).attr("id"))
           $('.filter-services').removeClass('d-none');

           if ($(this).attr("data-toggle") !== 'all') {
               $('.filter-services').addClass('d-none');
               $('.filter-services[data-toggle="' + $(this).attr("data-toggle") + '"]').removeClass('d-none')
           }
       });
   </script>

