<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPartnerDisplay.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Partners.ucPartnerDisplay" %>


<style>
    .tooltip {
        position: relative;
        display: inline-block;
        border-bottom: 1px dotted black;
    }

        .tooltip .tooltiptext {
            visibility: hidden;
            width: 120px;
            background-color: black;
            color: #fff;
            text-align: center;
            border-radius: 6px;
            padding: 5px 0;
            /* Position the tooltip */
            position: absolute;
            z-index: 1;
        }

        .tooltip:hover .tooltiptext {
            visibility: visible;
        }
</style>


<asp:Panel ID="pnlData" runat="server">
    <section class="my-5 py-5">
        <div class="container">
            <div class="d-lg-flex justify-content-between   ">
                <div class="d-flex justify-content-center">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                        <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_PnuPartnerTitle %>" />
                        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1>
                </div>
                <div class="text-center mb-0">
                    <div class="d-flex justify-content-center">
                      

                        <ul class="nav  nav-pills flex-nowrap  bg-semi-light p-1 rounded-2" id="myTab" role="tablist">

                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="internal-tab" data-bs-toggle="tab" data-bs-target="#z1-tab-pane" type="button" role="tab" aria-controls="internal-tab-pane" aria-selected="true">
                                    <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_LocalPartner %>" />
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="external-tab" data-bs-toggle="tab" data-bs-target="#z2-tab-pane" type="button" role="tab" aria-controls="external-tab-pane" aria-selected="false" tabindex="-1">
                                    <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_InternationalPartner %>" /></button>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="position-relative tab-content">
                <div class="tab-pane fade show active" id="z1-tab-pane" role="tabpanel" aria-labelledby="1-tab" tabindex="0">
                    <div dir="rtl" class="swiper swiper-container-11 col-lg-10 swiper-initialized swiper-horizontal swiper-pointer-events swiper-rtl swiper-backface-hidden">
                        <div class="swiper-wrapper py-4 align-items-center" id="swiper-wrapper-1b64988c6cb141ab" aria-live="off" style="height: 120px; transform: translate3d(0px, 0px, 0px); transition-duration: 0ms;">

                            <asp:Repeater ID="rptData" runat="server">
                                <ItemTemplate>



                                    <div class="swiper-slide d-flex justify-content-center align-items-start swiper-slide-active" role="group" aria-label="1 / 6" style="height: fit-content; width: 228px; margin-left: 60px;">
                                        <img src="<%# Eval("Logo") %>" class="img-fluid">
                                    </div>

                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="mt-5">
                            <div class="swiper-pagination swiper-pagination-bullets swiper-pagination-horizontal">
                                <span class="swiper-pagination-bullet swiper-pagination-bullet-active" aria-current="true"></span>
                                <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                                <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                            </div>
                        </div>
                    </div>

                </div>
           
                <div class="tab-pane fade" id="z2-tab-pane" role="tabpanel" aria-labelledby="2-tab" tabindex="1">
                    <div dir="rtl" class="swiper swiper-container-11 col-lg-10 swiper-initialized swiper-horizontal swiper-pointer-events swiper-rtl swiper-backface-hidden">
                        <div class="swiper-wrapper py-4 align-items-center" id="swiper-wrapper-1b64988c6cb141ab" aria-live="off" style="height: 120px; transform: translate3d(0px, 0px, 0px); transition-duration: 0ms;">

                            <asp:Repeater ID="rptInternational" runat="server">
                                <ItemTemplate>


                                    <div class="swiper-slide d-flex justify-content-center align-items-start swiper-slide-active" role="group" aria-label="1 / 6" style="height: fit-content; width: 228px; margin-left: 60px;">

                                        <img src="<%# Eval("Logo") %>" class="img-fluid">
                                    </div>

                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="mt-5">
                            <div class="swiper-pagination swiper-pagination-bullets swiper-pagination-horizontal">
                                <span class="swiper-pagination-bullet swiper-pagination-bullet-active" aria-current="true"></span>
                                <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                                <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
         <div class="d-flex justify-content-center    pt-lg-4 ">
           <p class="mb-0 "> <a class="btn btn-link fw-bolder text-primary text-decoration-none" href="allPartners.aspx">
                <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_All %>" /></a></p>
              
           </div>
          
        </div>
    </section>
</asp:Panel>




