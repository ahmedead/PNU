<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeStatisticsNew.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucHomeStatisticsNew" %>





<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>




<section class="statistics my-1 py-1 py-md-5 my-md-5">
      <div class="container p-5 ">
        <div class="d-flex justify-content-between align-items-baseline mb-5">
          <div>
            <h2 class="title text-dark fw-bold px-2 border-start border-primary mt-md-0 mt-4 mb-5">
                 <asp:Literal runat="server" Text="<%$ Resources: PNUres, PNUinNumbers %>" />
            </h2>
          </div>

        </div>
        <div class="row">
             <asp:Repeater ID="rptStatistics" runat="server">
     <ItemTemplate>
          <div class="col-lg-3 col-md-6 mb-lg-0 mb-5 text-center">
            <div class="d-flex flex-column justify-content-center align-items-center">
              <div class="icon-container rounded-circle">
                <svg aria-hidden="true" focusable="false" class="bi mx-2 text-primary" width="32" height="32">
                  <use xlink:href="#degree3" />
                </svg>
              </div>
              <h3 class="display-5 my-4 text-primary"><%# Eval("Count") %></h3>
              <h5 class="text-black fw-light"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEn")) %></h5>
            </div>
            <hr class="mt-5 mb-0 d-md-none d-block" />
          </div>
                </ItemTemplate>
</asp:Repeater>
         
            </div>
      </div>
    </section>


