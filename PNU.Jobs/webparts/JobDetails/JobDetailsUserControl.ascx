<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobDetailsUserControl.ascx.cs" Inherits="PNU.Jobs.webparts.JobDetails.JobDetailsUserControl" %>

<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/swiper@8/swiper-bundle.min.css" />

<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/swiper@8/swiper-bundle.min.js"></script>
<script src="js/icons-svg.js"></script>

<script src="js/main.js"></script>

<section class=" py-5 my-5">
    <div class="container">
        <div class="row">
            <div class="col-lg-7 mb-3 mb-lg-5">
                <div class="card  job-details-card p-2 p-lg-5 ">
                    <div class="card-body p-0">
                        <div class="d-flex flex-wrap justify-content-between align-items-start">

                            <h3 class="fw-bold">
                                <asp:Label ID="lblJobTitle" runat="server"></asp:Label></h3>

                            <div class="text-end">

                                <p class="mb-1 text-muted">تاريخ النشر:
                                    <asp:Label ID="lblPublishingDate" runat="server"></asp:Label></p>
                                <p class="mb-0 text-muted">آخر موعد للتقديم:
                                    <asp:Label ID="lblDueDate" runat="server"></asp:Label></p>
                            </div>

                        </div>
                        <div class="mb-3 fs-5">
                            <svg width="20" height="20">
                                <use xlink:href="#locationIcon"></use>
                            </svg>
                            <span class="ms-2 text-muted">
                                <asp:Label ID="lblLocation" runat="server"></asp:Label></span>
                        </div>
                        <div class="mb-3">
                            <span class="badge rounded-pill bg-resonant-blue-100 text-secondary p-2 px-3">
                                <asp:Label ID="lblJobCategory" runat="server"></asp:Label></span>
                            <span class="badge rounded-pill bg-resonant-blue-100 text-secondary  p-2 px-3">
                                <asp:Label ID="lblJobType" runat="server"></asp:Label></span>
                        </div>
                        <div class="mb-3">

                            <p class="text-muted text-justify ">
                                <asp:Label ID="lblJobDescription" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>
                </div>
                <div class="card my-4  accordion-card">
                    <div class="accordion accordion-flush " id="accordionFlushExample">
                        <div class="accordion-item  mb-2 border-0">
                            <h2 class="accordion-header" id="flush-headingOne">
                                <button class="accordion-button collapsed border" type="button" data-bs-toggle="collapse"
                                    data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
                                    ما هي شروط التقديم
                     
                                </button>
                            </h2>
                            <div id="flush-collapseOne" class="accordion-collapse collapse " aria-labelledby="flush-headingOne"
                                data-bs-parent="#accordionFlushExample">


                                <asp:Repeater ID="repConditions" runat="server">
                                    <HeaderTemplate>
                                        <div class="accordion-body">
                                            <ul class="list">
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <li class="list-item h5 mb-3">
                                            <%#Eval("Title")%>
                          </li>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        </ul>
                                </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="accordion-item  my-2 border-0">
                            <h2 class="accordion-header" id="flush-headingTwo">
                                <button class="accordion-button collapsed border " type="button" data-bs-toggle="collapse"
                                    data-bs-target="#flush-collapseTwo" aria-expanded="false" aria-controls="flush-collapseTwo">
                                    ما هي المستندات المطلوبة
                     
                                </button>
                            </h2>
                            <div id="flush-collapseTwo" class="accordion-collapse collapse" aria-labelledby="flush-headingTwo"
                                data-bs-parent="#accordionFlushExample">

                                <asp:Repeater ID="repDocs" runat="server">
                                    <HeaderTemplate>
                                        <div class="accordion-body">
                                            <ul class="list">
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <li class="list-item h5 mb-3">
                                            <%#Eval("Title")%>
                          </li>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        </ul>
                                </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <%-- <div class="accordion-item  my-2 border-0">
                    <h2 class="accordion-header" id="flush-headingOne">
                      <button class="accordion-button collapsed border" type="button" data-bs-toggle="collapse"
                        data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
                        ما هي شروط التقديم
                      </button>
                    </h2>
                    <div id="flush-collapseOne" class="accordion-collapse collapse " aria-labelledby="flush-headingOne"
                      data-bs-parent="#accordionFlushExample">

                       
                    </div>
                  </div>--%>
                    </div>
                </div>
            </div>

        </div>
    </div>
</section>
