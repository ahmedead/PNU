<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PnuEServiceDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices.PnuEServiceDetails" %>


<section class=" pt-5">
    <div class="container pt-5 my-5">
        <div class="row">

            <div class="col-lg-6 order-lg-0 order-1  ">
                <div class="ps-lg-4 mt-5 ">
                    <div class="d-flex align-items-start  mb-4 mt-5">
                        <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-5 me-md-4 flex-shrink-0">
                            <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_IntroService%>"></asp:Literal>

                            <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                        </h1>
                    </div>
                    <p class="fs-5 mb-5 text-muted" style="text-align:justify;">
                        <asp:Literal ID="lit_ServiceDescription" runat="server"></asp:Literal>

                    </p>
                    <div>

                        <span class="  fs-3">
                            <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_BeneficiriesTitle%>"></asp:Literal>
                        </span>

                        <div class="d-flex flex-wrap align-items-center justify-content-between">
                            <div class="mb-2 fs-4">

                                <asp:Repeater ID="RptBenef" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                            <tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <td class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#Eval("BeneficiaryTitle") %> </td>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-lg-6 mt-0 pb-md-5 pb-2   mb-md-5 mb-2 text-end">
                <div class=" position-relative ms-4 pattern-behind-img">

                    <asp:Image ID="ServiceImageUrl" runat="server" class="d-block w-100 rounded-4 object-fit-cover img-fluid ms-auto d-block mx-5" alt="..." />
                </div>
            </div>
        </div>
    </div>
</section>

<section class="fast-track-program py-3">
    <div class="container">
        <div class=" py-5 ">
            <div class="d-flex justify-content-center">
                <h1 class="title text-dark fw-bold px-2  border-primary mb-5">
                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_InfoService%>"></asp:Literal>
                </h1>
            </div>

            <div class="row">
                <div class="col-12">

                    <div class="accordion-card bg-transparent">
                        <div class="accordion accordion-flush bg-transparent" id="accordionFlushExample">
                            <div class="accordion-item  my-2 border-0  rounded-4">
                                <h2 class="accordion-header" id="flush-headingOne">
                                    <button class="accordion-button collapsed border" type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceConditions%>"></asp:Literal>

                                    </button>
                                </h2>
                                <div id="flush-collapseOne" class="accordion-collapse collapse " aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample">

                                    <div class="accordion-body">

                                        <asp:Repeater ID="rptConditions" runat="server">
                                            <HeaderTemplate>
                                                <ul class="list">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li class="list-item h5 mb-3">
                                                    <h5><%#Eval("Title") %>

                                                        </h5>
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                            <div class="accordion-item  my-2 border-0  rounded-4">
                                <h2 class="accordion-header" id="flush-headingTwo">
                                    <button class="accordion-button collapsed border " type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseTwo" aria-expanded="false" aria-controls="flush-collapseTwo">
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDocuments%>"></asp:Literal>

                                    </button>
                                </h2>
                                <div id="flush-collapseTwo" class="accordion-collapse collapse" aria-labelledby="flush-headingTwo" data-bs-parent="#accordionFlushExample">

                                    <div class="accordion-body">

                                        <asp:Repeater ID="rptDocuments" runat="server">
                                            <HeaderTemplate>
                                                <ul class="list">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li class="list-item h5 mb-3">
                                                    <h5><%#Eval("Title") %>

                                                        </h5>
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

            </div>
            <div class="d-flex justify-content-center mt-5">
                <h1 class="title text-dark fw-bold px-2  border-primary mb-5">
                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceSchedule%>"></asp:Literal>
                </h1>
            </div>

            <div class="row">
                <div class="col-12">

                    <div class="card p-3 rounded-4 border-0">
                        <div class="table-responsive b">

                            <asp:Repeater ID="rptSchedule" runat="server">
                                <HeaderTemplate>
                                    <table class="table  table-borderless fs-5 text-nowrap">
                                        <thead class="fs-4 text-primary">
                                            <tr>
                                                <th class="w-50">
                                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceProcedure%>"></asp:Literal></th>
                                                <th class="text-center w-25">
                                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDate%>"></asp:Literal>
                                                </th>
                                                <th class="text-center w-25">
                                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDay%>"></asp:Literal>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody style="vertical-align: middle;">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="d-flex align-items-start  my-1">
                                                <svg class="bi me-3 mt-1 align-self-center" width="14" height="14">
                                                    <use xlink:href="#circle-dots"></use>
                                                </svg>
                                                <p class="text-muted card-text fw-normal  h5 mb-0"><%#Eval("Title") %>   </p>
                                            </div>
                                        </td>
                                        <td class="text-center w-25"><%#Eval("Day") %></td>
                                        <td class="text-center w-25"><%#Eval("Date") %>  </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                     </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>

                    </div>

                </div>

            </div>
          
            <div class=" mt-4 d-flex justify-content-end">
               
                 <asp:LinkButton class="btn btn-lg btn-primary px-4"  ID="lnkApply" runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceApply%>"></asp:LinkButton>
            </div>
            
        </div>
    </div>

</section>
