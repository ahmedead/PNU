<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/RegAdm/ucStatistics.ascx" TagPrefix="uc1" TagName="ucStatistics" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/RegAdm/ucRegServices.ascx" TagPrefix="uc1" TagName="ucRegServices" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomePage.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.ucHomePage" %>

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

    <section class="about-scientific-research">
      <div class="container">

          <asp:Repeater ID="rptMainData" runat="server">
              <ItemTemplate>
                  <div class=" pt-5 mt-5">
                      <div class="d-flex justify-content-center">
                          <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5"><%#Eval("OverviewTitle") %>
                              <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                          </h1>
                      </div>
                      <div class="text-center mb-5">
                          <p class="h5 text-muted card-text lh-base">
                              <%# Eval("Overview") %>
                      </div>

                      <div class="row">
                          <div class="col-12">
                              <div class="card mb-4 p-3 research-services-center-card">
                                  <img src='<%# Eval("PublishingRollupImage") %>' class="img-fluid  w-100" alt="...">
                              </div>

                          </div>
                      </div>
                  </div>

              </ItemTemplate>
          </asp:Repeater>
      </div>
    </section>

<uc1:ucstatistics runat="server" id="ucStatistics" />


    <section class=" position-relative  my-5 py-5 ">
      <div class="container">
          <asp:Repeater ID="rptMainData1" runat="server">
              <ItemTemplate>
                  <div class="d-flex justify-content-center">
                      <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 me-md-4">
                          <%# Eval("VisionTitle") %>
                          <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                      </h1>
                  </div>

              </ItemTemplate>
          </asp:Repeater>
        <div class="position-relative my-5">
          <div class="row gy-4 mb-5">

              <asp:Repeater ID="rptServices" runat="server">
                  <ItemTemplate>
                      <div class="col-lg-6">
                          <a href='<%# Eval("URL") %>'>
                              <div class="card p-0 position-relative rounded-3">
                                  <img src='<%# Eval("PublishingRollupImage") %>' class="img-fluid  w-100" alt="...">
                                  <div
                                      class="position-absolute w-100 bottom-0 p-4 bg-primary bg-opacity-75 text-center rounded-3 rounded-top-0 rounded-bottom">
                                      <p class="fs-3  mb-0 lh-base text-white"><%# Eval("Title") %> </p>
                                  </div>
                              </div>
                          </a>
                      </div>

                  </ItemTemplate>
              </asp:Repeater>
          </div>
        </div>
      </div>
    </section>

<%--<uc1:ucRegServices runat="server" id="ucRegServices" />--%>
