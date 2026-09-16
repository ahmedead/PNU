<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllDeenShipsAndDepartments.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.ucAllDeenShipsAndDepartments" %>



<section class="our-sections position-relative ">
            <div class="d-flex justify-content-center pt-5 ">
              <div>
                <h1 class="title text-white fw-bold px-2 border-start border-primary mb-5"> <asp:Literal ID="lit_deanship" runat="server" Text="<%$Resources:PnuInternetResources, res_DeanshipAndDepartments%>"></asp:Literal> 
                  <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                </h1>
              </div>
            </div>
            <div class="position-relative  bg-transparent pb-5">
              <div class="swiperbox-bg-1 d-none d-lg-block"></div>
              <div dir="ltr" class="swiper swiper-container-5 me-5">
                <div class="swiper-wrapper">
                     <asp:Repeater ID="rpt0" runat="server">
                    <ItemTemplate>
                  <div class="swiper-slide bg-transparent">
                    <div class="card border-0 mb-4 p-0 overflow-hidden also-know-card">
                      <img src="<%# Eval("PublishingRollupImage") %>" alt="...">
                      <a href="<%# Eval("URL") %>" class="stretched-link text-decoration-none">
                        <div class="position-absolute bottom-0 text-start ms-5 start-0">
                            <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white "><%# Eval("Title") %></h2>
                        </div>
                      </a>
                    </div>
                  </div>
                    </ItemTemplate>
                </asp:Repeater>

                  <div class="swiper-slide bg-transparent">
                    <div class="row">
                      <div class="col-6 masonry-images-1">
                          <asp:Repeater ID="rpt1" runat="server">
                              <ItemTemplate>
                                  <div class="card border-0 mb-4 p-0 overflow-hidden also-know-card">
                                      <img src="<%# Eval("PublishingRollupImage") %>" alt="...">
                                      <a href="<%# Eval("URL") %>" class="stretched-link text-decoration-none">
                                          <div class="position-absolute bottom-0 text-start ms-5 start-0">
                                              <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white "><%# Eval("Title") %> </h2>
                                          </div>
                                      </a>
                                  </div>

                              </ItemTemplate>
                          </asp:Repeater>
                          <asp:Repeater ID="rpt2" runat="server">
                              <ItemTemplate>
                                  <div class="card border-0 mb-4 p-0 overflow-hidden also-know-card">
                                      <img src="<%# Eval("PublishingRollupImage") %>" class="h-100 object-fit-cover" alt="...">
                                      <a href="<%# Eval("URL") %>" class="stretched-link text-decoration-none">
                                          <div class="position-absolute bottom-0 text-start ms-5 start-0">
                                              <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white "><%# Eval("Title") %></h2>
                                          </div>
                                      </a>
                                  </div>

                              </ItemTemplate>
                          </asp:Repeater>


                      </div>
                      <div class="col-6 masonry-images-2">
                          <asp:Repeater ID="rpt3" runat="server">
                              <ItemTemplate>
                                  <div class="card border-0 mb-4 p-0 overflow-hidden also-know-card">
                                      <img src="<%# Eval("PublishingRollupImage") %>" class="h-100 object-fit-cover" alt="...">
                                      <a href="<%# Eval("URL") %>" class="stretched-link text-decoration-none">
                                          <div class="position-absolute bottom-0 text-start ms-5 start-0">
                                              <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white "><%# Eval("Title") %></h2>
                                          </div>
                                      </a>
                                  </div>
                              </ItemTemplate>
                          </asp:Repeater>

                          <asp:Repeater ID="rpt4" runat="server">
                              <ItemTemplate>
                                  <div class="card border-0 mb-4 p-0 overflow-hidden also-know-card">
                                      <img src="<%# Eval("PublishingRollupImage") %>" alt="...">
                                      <a href="<%# Eval("URL") %>" class="stretched-link text-decoration-none">
                                          <div class="position-absolute bottom-0 text-start ms-5 start-0">
                                              <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white "><%# Eval("Title") %></h2>
                                          </div>
                                      </a>
                                  </div>
                              </ItemTemplate>
                          </asp:Repeater>
                      </div>
                    </div>
                  </div>
                  

                    <asp:Repeater ID="rpt5" runat="server">
                        <ItemTemplate>

                            <div class="swiper-slide bg-transparent">
                                <div class="card border-0 mb-4 p-0 overflow-hidden also-know-card">
                                    <img src="<%# Eval("PublishingRollupImage") %>" alt="...">
                                    <a href="<%# Eval("URL") %>" class="stretched-link text-decoration-none">
                                        <div class="position-absolute bottom-0 text-start ms-5 start-0">
                                            <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white "><%# Eval("Title") %> </h2>
                                        </div>
                                    </a>
                                </div>
                            </div>

                        </ItemTemplate>
                    </asp:Repeater>
                  
                </div>
      
              </div>
            </div>
            <div class="swiper-sections-navigation d-none d-lg-flex">
              <div class=" swiper-next me-2">
                <svg width="50" height="30">
                  <use xlink:href="#arrowRight" />
                </svg>
              </div>
              <div class="swiper-prev">
                <svg width="50" height="30">
                  <use xlink:href="#arrowLeft" />
                </svg>
              </div>
            </div>
            <div class="swiperbox-bg-2 d-none d-lg-block"></div>
          </section>
