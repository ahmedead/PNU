<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUnivTimeline.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucUnivTimeline" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>





<section class=" ">
      <div class="container py-5 my-5">
        <div class=" pb-5 mb-5">
          <div class="d-flex justify-content-center">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5"> <asp:Literal runat="server" Text="<%$ Resources: PNUres, UniversityHistory %>" /> 
              <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
            </h1>
          </div>
        </div>
        <div class="timeline">
            <asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>
       
          <div class="timeline-row">
            <div class="timeline-content">
              <h4 class=" h4 text-turquoise-500 mb-3"> <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> </h5> 
                <p class="fs-5"><%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %></p>
                <div class="border-behind-img">
                  <img src='<%# Eval("PublishingRollupImage") %>' class=" img-fluid " alt="...">
                </div>
            </div>
          </div>
         


    </ItemTemplate>
</asp:Repeater>
        </div>
      </div>
    </section>

