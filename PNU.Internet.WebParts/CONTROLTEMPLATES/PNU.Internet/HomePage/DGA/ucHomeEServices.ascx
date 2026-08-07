<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeEServices.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeEServices" %>





<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>




<section class="gray colored-section py-5" data-aos="fade-up">
    <div class="container">
        <div class="mb-4">
            <div class="d-flex justify-content-between align-items-start gap-2">
                <h2 class="mb-0">
                    
                    <%# IsArabic ? "اهم الخدمات الإلكترونية" : "Key ESservices" %>

                </h2>
                <a class="btn btn-outline-secondary fw-semibold" href="Eservice.aspx">
                    <%# IsArabic ? "دليل الخدمات الإلكترونية" : "ESservices Guide" %>
                    </a>
            </div>
            <!-- <p class="mb-0 mt-3">هنا يمكنك التعرف على منصاتنا الرقمية </p> -->
        </div>
        <dga-swiper class="swiper dga-eservices-swiper pt-2">
            <swiper-container class="swiper-wrapper">
                <asp:Repeater ID="rptAllData" runat="server">
                    <ItemTemplate>
                        <swiper-slide class="swiper-slide flex-grow-1" data-aos="fade-up" data-aos-delay="0">
                            <article class="card h-100">
                                <div class="card-body d-flex flex-column gap-4">
                                    <div class="icon-container">
                                        <span class="d-inline-flex fs-3">
                                            <i class="hgi hgi-stroke hgi-validation-approval"></i>
                                        </span>
                                    </div>
                                    <div>
                                        <h2 class="card-title">
                                            <%# SPFactory.GetLocalizedTitle(Eval("ARServiceName"), Eval("ENServiceName")) %>
                                        </h2>
                                        <p class="card-text">
                                            <%# SPFactory.GetLocalizedTitle(Eval("Desc"), Eval("Desc_EN")) %>
                                        </p>
                                    </div>
                                    <div class="d-flex flex-wrap mt-auto gap-2">
                                        <span class="badge badge-info"><%# SPFactory.GetLocalizedTitle(Eval("DP_TargetGroup"), Eval("DP_TargetGroup_EN")) %></span>
                                    </div>
                                    <div class="d-flex gap-3">

                                        <a target="_blank" rel="noopener" class="btn btn-primary"
                                            href="service-details.aspx?eti=<%#DataBinder.Eval(Container.DataItem,"ID") %>">

                                            <asp:Literal ID="lit_serviceDetails" runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDetails%>"></asp:Literal>
                                        </a>

                                        <a target="_blank" rel="noopener" class="btn btn-outline-primary"
                                            href="<%#DataBinder.Eval(Container.DataItem,"URL") %>  ">
                                            <asp:Literal ID="lit_startService" runat="server" Text="<%$Resources:PnuInternetResources, res_StartService%>"></asp:Literal>
                                        </a>
                                    </div>
                                </div>
                            </article>
                        </swiper-slide>
                    </ItemTemplate>
                </asp:Repeater>

            </swiper-container>
            <div class="d-flex justify-content-between align-items-center mt-3">
                <div class=" d-flex gap-1">
                    <button class="btn btn-primary rounded-circle icon-btn dga-button-prev">
                        <span class="d-inline-flex fs-4 lh-1">
                            <i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip"></i>
                        </span>
                    </button>
                    <button class="btn btn-primary rounded-circle icon-btn dga-button-next">
                        <span class="d-inline-flex fs-4 lh-1">
                            <i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip"></i>
                        </span>
                    </button>
                </div>
                <div class="d-flex justify-content-end dga-swiper-pagination">
                </div>
            </div>
        </dga-swiper>
    </div>
</section>
