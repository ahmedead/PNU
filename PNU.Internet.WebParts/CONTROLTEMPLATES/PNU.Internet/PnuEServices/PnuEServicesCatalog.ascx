<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PnuEServicesCatalog.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices.PnuEServicesCatalog" %>


<section class="py-5 mb-5 also-know bg-transparent">
    <div class="container  ">

        <div class="pt-5 mt-5 d-flex justify-content-center">
            <div>
                <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                    <asp:Literal ID="lit_PnuServices" runat="server" Text="<%$Resources:PnuInternetResources, res_PnuEservices%>"></asp:Literal>

                    <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                </h1>
            </div>
        </div>
        <div class="d-flex justify-content-center tabbable">
            <ul class="nav nav-tabs nav-pills  mb-5 bg-semi-light p-1 rounded-2" id="myTab" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="home-tab" data-bs-toggle="tab" data-bs-target="#z1-tab-pane" type="button" role="tab" aria-controls="z1-tab-pane" aria-selected="true">
                        <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_NewStd %>" />

                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="college2-tab" data-bs-toggle="tab" data-bs-target="#z2-tab-pane" type="button" role="tab" aria-controls="z2-tab-pane" aria-selected="false">
                        <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_ContinuingStd %>" />

                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="college3-tab" data-bs-toggle="tab" data-bs-target="#z3-tab-pane" type="button" role="tab" aria-controls="z3-tab-pane" aria-selected="false">
                        <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_GraduatedStd %>" />

                    </button>
                </li>

            </ul>
        </div>
        <div class="tab-content">
            <div class="tab-pane fade show active" id="z1-tab-pane" role="tabpanel" aria-labelledby="1-tab" tabindex="0">

                <div class="row my-4 pb-5">
                    <asp:Repeater ID="RepNewStd" runat="server">
                        <HeaderTemplate>
                            <table>
                                <tr class="col-md-6 col-lg-4 col-xxl-3 mb-4">
                        </HeaderTemplate>
                        <ItemTemplate>

                            <td>
                                <div class="card border rounded-4 h-100 university-service-card">
                                    <div class="card-body p-2 d-flex flex-column justify-content-between">
                                        <div>
                                            <img src='<%# Eval("ImageUrl") %>' class="object-fit-cover rounded-3 w-100" height="250">
                                            <div class="my-2 fs-4 fw-bold">
                                                <label class="stretched-link text-decoration-none text-body strong">
                                                    <%# Eval("Title") %>
                                                </label>
                                            </div>
                                            <p class="text-muted">
                                                <%# Eval("Desc") %>
                                            </p>
                                        </div>

                                        <div class="hide-on-hover ">

                                            <span class="  fs-5">
                                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_BeneficiriesTitle%>"></asp:Literal></span>

                                            <div class="d-flex flex-wrap align-items-center justify-content-between">
                                                <div class="mb-2">

                                                    <asp:Repeater ID="RptBenef" runat="server" DataSource='<%# Eval("Beneficiaries") %>'>
                                                        <HeaderTemplate>
                                                            <table>
                                                                <tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <td class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#Eval("BeneficiaryTitle") %> </td>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tr></table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>



                                            </div>
                                        </div>




                                    </div>
                                </div>
                                <div class="show-on-hover ">

                                    <a class="btn btn-primary px-4 w-100" href='ServiceDetails.aspx?ServiceId=<%#Eval("Id") %>'>
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDetails%>"></asp:Literal></a>
                                </div>
                            </td>

                        </ItemTemplate>
                        <FooterTemplate>
                            </tr></table>
                        </FooterTemplate>
                    </asp:Repeater>

                </div>

            </div>
            <div class="tab-pane fade" id="z2-tab-pane" role="tabpanel" aria-labelledby="2-tab" tabindex="1">

                <div class="row my-4 pb-5">
                    <asp:Repeater ID="RepContinuingStd" runat="server">
                        <HeaderTemplate>
                            <table>
                                <tr class="col-md-6 col-lg-4 col-xxl-3 mb-4">
                        </HeaderTemplate>
                        <ItemTemplate>

                            <td>
                                <div class="card border rounded-4 h-100 university-service-card">
                                    <div class="card-body p-2 d-flex flex-column justify-content-between">
                                        <div>
                                            <img src='<%# Eval("ImageUrl") %>' class="object-fit-cover rounded-3 w-100" height="250">
                                            <div class="my-2 fs-4 fw-bold">
                                                <label class="stretched-link text-decoration-none text-body strong">
                                                    <%# Eval("Title") %>
                                                </label>
                                            </div>
                                            <p class="text-muted">
                                                <%# Eval("Desc") %>
                                            </p>
                                        </div>

                                        <div class="hide-on-hover ">

                                            <span class="  fs-5">
                                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_BeneficiriesTitle%>"></asp:Literal></span>

                                            <div class="d-flex flex-wrap align-items-center justify-content-between">
                                                <div class="mb-2">

                                                    <asp:Repeater ID="RptBenef" runat="server" DataSource='<%# Eval("Beneficiaries") %>'>
                                                        <HeaderTemplate>
                                                            <table>
                                                                <tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <td class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#Eval("BeneficiaryTitle") %> </td>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tr></table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>



                                            </div>
                                        </div>




                                    </div>
                                </div>
                                <div class="show-on-hover ">

                                    <a class="btn btn-primary px-4 w-100" href='ServiceDetails.aspx?ServiceId=<%#Eval("Id") %>'>
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDetails%>"></asp:Literal></a>
                                </div>
                            </td>

                        </ItemTemplate>
                        <FooterTemplate>
                            </tr></table>
                        </FooterTemplate>
                    </asp:Repeater>

                </div>

            </div>
            <div class="tab-pane fade" id="z3-tab-pane" role="tabpanel" aria-labelledby="3-tab" tabindex="2">

                <div class="row my-4 pb-5">
                    <asp:Repeater ID="RepGraduatedStd" runat="server">
                        <HeaderTemplate>
                            <table>
                                <tr class="col-md-6 col-lg-4 col-xxl-3 mb-4">
                        </HeaderTemplate>
                        <ItemTemplate>

                            <td>
                                <div class="card border rounded-4 h-100 university-service-card">
                                    <div class="card-body p-2 d-flex flex-column justify-content-between">
                                        <div>
                                            <img src='<%# Eval("ImageUrl") %>' class="object-fit-cover rounded-3 w-100" height="250">
                                            <div class="my-2 fs-4 fw-bold">
                                                <label class="stretched-link text-decoration-none text-body strong">
                                                    <%# Eval("Title") %>
                                                </label>
                                            </div>
                                            <p class="text-muted">
                                                <%# Eval("Desc") %>
                                            </p>
                                        </div>

                                        <div class="hide-on-hover ">

                                            <span class="  fs-5">
                                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_BeneficiriesTitle%>"></asp:Literal></span>

                                            <div class="d-flex flex-wrap align-items-center justify-content-between">
                                                <div class="mb-2">

                                                    <asp:Repeater ID="RptBenef" runat="server" DataSource='<%# Eval("Beneficiaries") %>'>
                                                        <HeaderTemplate>
                                                            <table>
                                                                <tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <td class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#Eval("BeneficiaryTitle") %> </td>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tr></table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>



                                            </div>
                                        </div>




                                    </div>
                                </div>
                                <div class="show-on-hover ">

                                    <a class="btn btn-primary px-4 w-100" href='ServiceDetails.aspx?ServiceId=<%#Eval("Id") %>'>
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDetails%>"></asp:Literal></a>
                                </div>
                            </td>

                        </ItemTemplate>
                        <FooterTemplate>
                            </tr></table>
                        </FooterTemplate>
                    </asp:Repeater>

                </div>

            </div>
        </div>
    </div>
</section>

