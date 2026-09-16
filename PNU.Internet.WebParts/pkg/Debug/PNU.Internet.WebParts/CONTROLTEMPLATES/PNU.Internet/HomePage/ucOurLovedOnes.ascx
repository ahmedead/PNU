<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOurLovedOnes.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucOurLovedOnes" %>


<section class="darling d-none d-md-block ">
      <div class="container">
        <div class="swiper-sections-navigation d-none d-lg-flex top-0">
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
        <div class="row">
          <div class="col-7 offset-5  mt-0 justify-content-center align-items-center">
            <h1 class="title text-black fw-bold px-2 border-start border-primary mb-0 mt-md-0 mt-4"> <asp:Literal runat="server" Text="<%$ Resources: PNUres, OurLoveredOnes %>" /><span
                class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
            </h1>
          </div>
        </div>
        <div class="swiperbox-1"></div>

        <div class="row mt-3">
          <div class="col-12">
            <div class="content position-relative ">
              <div class="swiper swiper-container-2">
                <div class="swiper-wrapper">
                  <div class="swiper-slide bg-transparent">
                    <div>
                      <img loading="lazy"  src="images/unsplash_l1nRZW-hjGY_3.png" class=" img-fluid " alt="...">
                      <div class="card mb-4 pt-5 px-5 border-0 "
                        style="margin-right: 34%; margin-top: -30%; min-height: 360px;">
                        <div class="row g-0">
                          <div class="col-12">
                            <div class="card-body">
                              <h3 class="mb-2 text-dark fw-bolder">خريجتنا - أميرة عبدالله</h3>
                              <div class="row justify-content-between px-2  align-items-end mb-0">
                                <p class=" col h5 card-text text-muted">
                                  خضت في جامعة الأميرة نورة تجربة بيئة علمية عالمية،
                                  وأحسب
                                  لها
                                  شاكرة كل ما وفرته من تكاملات مع شركات كبرى محلية
                                  ودولية
                                  لتعزيز
                                  التجربة والتأهيل لسوق العمل. </p>
                                <div class="col-2 d-none d-lg-block">
                                  <svg class="bi mx-3 mb-0" width="90" height="90">
                                    <use xlink:href="#quote" />
                                  </svg>
                                </div>

                              </div>
                              <div
                                class="  position-absolute fixed-bottom  d-flex justify-content-between mt-5 align-items-end">
                                <div class="h5 p-5 pb-4">
                                  <a class="btn-link text-dark  text-decoration-none d-flex justify-content-end  align-items-center"
                                    href="#">
                                    <svg class="bi mx-2 text-dark" width="35" height="35">
                                      <use xlink:href="#play-dark" />
                                    </svg>
                                    شاهد الفيديو

                                  </a>
                                </div>
                                <div class="p-4  text-end ">
                                  <img loading="lazy"  height="44" src="images/pnu-logo-en.svg" alt="pnu-logo" />

                                </div>
                              </div>

                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="swiper-slide bg-transparent">
                    <div>
                      <img loading="lazy"  src="images/unsplash_l1nRZW-hjGY_3.png" class=" img-fluid " alt="...">
                      <div class="card mb-4 pt-5 px-5 border-0 position-absolute" style="margin-right: 34%;
                            margin-top: -30%;
                            min-height: 360px;">
                        <div class="row g-0">
                          <div class="col-12">
                            <div class="card-body">
                              <h3 class="mb-2 text-dark fw-bolder">خريجتنا - أميرة عبدالله</h3>
                              <div class="row justify-content-between px-2  align-items-end mb-0">
                                <p class=" col h5 card-text text-muted">
                                  خضت في جامعة الأميرة نورة تجربة بيئة علمية عالمية،
                                  وأحسب
                                  لها
                                  شاكرة كل ما وفرته من تكاملات مع شركات كبرى محلية
                                  ودولية
                                  لتعزيز
                                  التجربة والتأهيل لسوق العمل. </p>
                                <div class="col-2 d-none d-lg-block">
                                  <svg class="bi mx-3 mb-0" width="90" height="90">
                                    <use xlink:href="#quote" />
                                  </svg>
                                </div>

                              </div>
                              <div
                                class="  position-absolute fixed-bottom  d-flex justify-content-between mt-5 align-items-end">
                                <div class="h5 p-5 pb-4">
                                  <a class="btn-link text-dark  text-decoration-none d-flex justify-content-end  align-items-center"
                                    href="#">
                                    <svg class="bi mx-2 text-dark" width="35" height="35">
                                      <use xlink:href="#play-dark" />
                                    </svg>
                                    شاهد الفيديو

                                  </a>
                                </div>
                                <div class="p-4  text-end ">
                                  <img loading="lazy"  height="44" src="images/pnu-logo-en.svg" alt="pnu-logo" />

                                </div>
                              </div>

                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

              </div>
            </div>

          </div>
        </div>


      </div>
    </section>

