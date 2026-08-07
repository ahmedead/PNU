<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PnuEServiceHome.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices.PnuEServiceHome" %>


<section class="research-services-center bg-semi-light mt-3 pt-5" style="margin-bottom: -12rem; padding-bottom: 16rem;">

    <div class="container py-5 ">
        <div class=" pt-2 mt-2">
            <div class="d-flex justify-content-center">
                <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                    <asp:Literal ID="lit_PnuServices" runat="server" Text="<%$Resources:PnuInternetResources, res_PnuEservices%>"></asp:Literal>
                    <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                </h1>
            </div>
        </div>

        <div class="row mt-3">

            <asp:Repeater ID="rpt" runat="server">
                <ItemTemplate>
                    <div class="col-md-6 col-lg-4 col-xxl-3 mb-4">
                        <div class="card border rounded-4 h-100">
                            <div class="card-body p-2">

                                <img src='<%# Eval("ImageUrl") %>' class="object-fit-cover rounded-3 w-100"
                                    height="250">
                                <div class="my-2 fs-4 fw-bold">
                                    <a href='ServiceDetails.aspx?ServiceId=<%#Eval("Id") %>' class="stretched-link text-decoration-none text-body strong">
                                        <%# Eval("Title") %>
                                    </a>
                                </div>

                                <p class="text-muted"><%# Eval("Desc") %></p>
                                <span class="  fs-5">
                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_BeneficiriesTitle%>"></asp:Literal></span>
                                </span>

                        <div class="d-flex flex-wrap align-items-center justify-content-between">
                            <div class="mb-2">
                                <asp:Repeater ID="RptBenef" runat="server" DataSource='<%# Eval("Beneficiaries") %>'>

                                    <ItemTemplate>
                                        <span class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#Eval("BeneficiaryTitle") %> 
                                        </span>
                                    </ItemTemplate>

                                </asp:Repeater>


                            </div>

                        </div>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>

            </asp:Repeater>



        </div>
        <div class="d-flex justify-content-center">
            <a class="btn btn-lg btn-primary px-3 mt-4" href="AllServices.aspx">
                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_AllServices%>"></asp:Literal>
            </a>

        </div>
    </div>
</section>