<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucServiceDatails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.ucServiceDatails" %>


<section class=" pt-5">
    <div class="container pt-5 my-5">
        <div class="row" id="divData" runat="server">
        </div>
    </div>
</section>





<section class=" my-5 p-5 bg-semi-light position-relative">
    <div class="container my-5">
        <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4"><asp:Literal ID="lit_centerServices" runat="server" Text="<%$Resources:PnuInternetResources, res_CenterServices%>"></asp:Literal> 
                 
            <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
        </h1>
        <div dir="rtl" class="swiper swiper-container-8">
            <div class="swiper-wrapper py-5">



                <asp:Repeater ID="rptServices" runat="server">
                    <ItemTemplate>

                        <div class="swiper-slide h-auto">
                            <div class="card border-0 shadow-sm h-100">
                                <a href="ServiceDetails.aspx?ServiceId=<%#DataBinder.Eval(Container.DataItem,"ID") %>">
                                    <div class="card-body p-2">
                                        <img src="<%#DataBinder.Eval(Container.DataItem,"PublishingRollupImage") %>" class="object-fit-cover rounded-2 w-100" height="180">
                                        <div class="my-2 fs-4 fw-bold">
                                            <strong><%#DataBinder.Eval(Container.DataItem,"Title") %></strong>
                                        </div>
                                        <p><%#DataBinder.Eval(Container.DataItem,"Desc") %></p>
                                    </div>
                                </a>
                            </div>
                        </div>


                    </ItemTemplate>
                </asp:Repeater>


            </div>
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


