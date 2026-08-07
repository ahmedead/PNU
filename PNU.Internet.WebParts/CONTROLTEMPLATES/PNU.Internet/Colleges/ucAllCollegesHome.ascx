<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllCollegesHome.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucAllCollegesHome" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>
               


<style>
    img.d-block.w-100 {
        border-radius: 10px;
        height: 426px !important;
    }
</style>

<section class="colleges ">
      <div class="container">
        <div class="row py-4">
          <div class="col-lg-10 col-12 order-lg-0 order-1 ">
            <div class=" featured box me-0  me-lg-5">
              <div class="content position-relative">
                <div class="swiperbox-1"></div>
                <div class="swiper swiper-container-1  py-3  py-lg-5 ">
                    <div class="swiper-wrapper">


                        <asp:Repeater ID="rptColleges1" runat="server">
                    <ItemTemplate>
                        <div class="swiper-slide bg-transparent">
                            <a href="<%# String.Format("{1}Faculties/{0}/Pages/speech.aspx", Eval("Code"),SPFactory.GetSiteURL()) %>">
                                <div class="flip-card card item bg-transparent">
                                    <div class="flip-card-inner">
                                        <div class="flip-card-front">
                                            <div class="card item bg-transparent">
                                                <div class="thumb position-relative overflow-hidden">
                                                    <img src="<%#DataBinder.Eval(Container.DataItem,"DisplayImage") %>" class="d-block  w-100">
                                                </div>
                                                <h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                                    <span><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></span>
                                                </h2>
                                               
                                            </div>
                                        </div>
                                        <div class="flip-card-back py-4 px-4">
                                            <h2 class="mb-3 text-black"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h2>
                                            <p class="text-justify">
                                                <%# SPFactory.GetLocalizedTitle(Eval("DescriptionDisplay"), Eval("DescriptionDisplay_EN")) %>
                                            </p>
                                            <%--<div class="position-absolute fixed-bottom  text-end p-3 px-4">
                                                <img height="52" src="/Style Library/NewStyle/UI5/images/pnu-logo-en.svg" alt="pnu-logo" />

                                            </div>--%>
                                        </div>
                                    </div>
                                </div>

                            </a>
                        </div>


                        </ItemTemplate>
                </asp:Repeater>
                    </div>
                </div>

              </div>
            </div>
          </div>
          <div class="col-lg-2 col-12">
            <div class="py-0  py-lg-5 d-flex d-lg-block  justify-content-between">
              <h1 class="title fw-bold px-2 border-start border-primary mb-5 text-white"> 
				<asp:Literal runat="server" Text="<%$ Resources: PNUres, AllFacultiesAndInstitues %>" />
                <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
              </h1>
              <div>
                <h4 class="my-lg-4 my-0 fw-light text-white"><asp:Literal runat="server" Text="<%$ Resources: PNUres, IntroductoryTour %>" /></h4>
                <a id="allCollegesLink" runat="server">
    <button type="button" class="btn btn-outline-light me-2 px-3 py-2">
        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AllFacultiesAndInstitues %>" />
    </button>
</a>
                  
              </div>

            </div>
          </div>
        </div>
        <div class="swiperbox-2">
        </div>
      </div>
    </section>
