<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResearchCenters.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.HomePage.ucResearchCenters" %>



<section class="research-centers-section position-relative bg-semi-light my-5 py-5">
    <div class="container">
        <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4">مراكز بحوث الكليات
          <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
        </h1>
        <div dir="rtl" class="swiper swiper-container-8">
            <div class="swiper-wrapper py-5">

                <asp:Repeater ID="rptcenters" runat="server">
                    <ItemTemplate>


                        <div class="swiper-slide h-auto">
                            <div class="card border-0 shadow-sm h-100">
                                <div class="card-body p-2">
                                    <img src="<%#DataBinder.Eval(Container.DataItem," ImageURL ") %>" class="object-fit-cover rounded-2 w-100" height="180">
                                    <div class="my-2 fs-4 fw-bold">
                                        <strong><%#DataBinder.Eval(Container.DataItem,"Title") %></strong>
                                    </div>
                                    <div class="d-flex mb-3">
                                        <svg width="16" height="17">
                                            <use xlink:href="#locationIcon" />
                                        </svg>
                                        <span><%#DataBinder.Eval(Container.DataItem,"Location") %></span>
                                    </div>
                                    <p>
                                        <%#DataBinder.Eval(Container.DataItem,"Desc") %>
                                    </p>
                                </div>
                            </div>
                        </div>



                    </ItemTemplate>
                </asp:Repeater>


               
            </div>
        </div>
        <div class="text-center">
            <a class="fw-bold text-decoration-none" href="/ar/SR/Pages/scientific-research-centers.aspx">كل المراكز
            <svg width="16" height="16" class="ms-2">
                <use xlink:href="#chevronLeft" />
            </svg>
            </a>
        </div>
    </div>
    <div class="swiper-research-navigation d-none d-lg-flex">
        <div class="swiper-prev-3 me-2">
            <svg width="50" height="30">
                <use xlink:href="#arrowRight" />
            </svg>
        </div>
        <div class=" swiper-next-3">
            <svg width="50" height="30">
                <use xlink:href="#arrowLeft" />
            </svg>
        </div>
    </div>
</section>









