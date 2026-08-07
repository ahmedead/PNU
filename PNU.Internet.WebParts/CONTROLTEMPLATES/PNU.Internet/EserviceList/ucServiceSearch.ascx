<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucServiceSearch.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.ucServiceSearch" %>


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
<style>
    .RepFlex
    {
         display:flex;
         flex-direction:row;
         flex-wrap:wrap;
    }
    @media (max-width: 800px) {
  .RepFlex {
    flex-direction: column;
  }
}
  

</style>

<div class="services-catalog">




    <div class="container mb-5 pb-5 filter-services" data-toggle="rptprod">
        <div class="bg-title m-3 mb-5">
            <h5>نتائج البحث </h5>
        </div>
        <div class="RepFlex" style="">


            <asp:Repeater ID="rptprod" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="" style="display: flex; flex-basis: 33%; flex-direction: row; flex-wrap: wrap;">

                        <div class="container  mb-5 pb-5">



                            <div class="service-card">
                                <div class="serv-icon">
                                    <img src="../../../../_layouts/15/services-catalog/assets/imgs/logo_small.svg" title="">
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
                                    <a target="_blank" href="service-details.aspx?eti=<%#DataBinder.Eval(Container.DataItem,"ID") %>" class="serv-btn  serv-link">تفاصيل الخدمة</a>
                                    <a target="_blank" href="<%#DataBinder.Eval(Container.DataItem,"URL") %>  " class="serv-btn ">إبدأ الخدمة</a>
                                </div>
                            </div>
                        </div>



                    </div>


                </ItemTemplate>



            </asp:Repeater>
        </div>
    </div>

    <!-- end of Student Services -->


</div>
       
        

        

  
 

