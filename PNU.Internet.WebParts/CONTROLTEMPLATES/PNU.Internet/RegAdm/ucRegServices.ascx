<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRegServices.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.ucRegServices" %>



<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Panel ID="pnlData" runat="server">
<section class="mt-5 pt-5">
        <div class="container">
          <div class="d-flex align-items-start">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, RegDepartments %>" /> 
                
              <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
            </h1>
          </div>
          <div class="position-relative">
            <div dir="rtl" class="swiper swiper-container-9 col-lg-10">
              <div class="swiper-wrapper py-5">
  <asp:Repeater ID="rptServices" runat="server">
        <ItemTemplate>
            <div class="swiper-slide h-auto">
                <div class="card border h-100">
                    <a href="<%# String.Format("{0}", Eval("URL")) %>">
                    <div class="card-body p-2">
                        <img src="<%#DataBinder.Eval(Container.DataItem,"DisplayImage") %>" class="object-fit-cover rounded-2 w-100" height="180"
                           >
                        <div class="my-2 fs-4 fw-bold">
                            <strong>
                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                            </strong>
                        </div>
                        <p class="text-light-emphasis"><%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                        </p>
                    </div>
                    </a>
                </div>
            </div>

        </ItemTemplate>
    </asp:Repeater>
              </div>
            </div>
            <div class="swiper-faculty-members-navigation d-none d-lg-block ">
              <div class="swiper-prev-1">
                <svg width="50" height="30">
                  <use xlink:href="#arrowRight" />
                </svg>
              </div>
              <div class="swiper-next-1">
                <svg width="50" height="30">
                  <use xlink:href="#arrowLeft" />
                </svg>
              </div>
            </div>
            <div class="swiper-faculty-members-navigation d-block d-lg-none position-relative">
              <div class="swiper-prev-1">
                <svg width="50" height="30">
                  <use xlink:href="#arrowRight" />
                </svg>
              </div>
              <div class="swiper-next-1">
                <svg width="50" height="30">
                  <use xlink:href="#arrowLeft" />
                </svg>
              </div>
            </div>
          </div>
        </div>
      </section>


</asp:Panel>