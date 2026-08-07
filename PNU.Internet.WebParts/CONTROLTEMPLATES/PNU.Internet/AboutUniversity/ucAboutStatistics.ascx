<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAboutStatistics.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucAboutStatistics" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="  my-1 py-1  py-md-5 my-md-5"> 
   <div class="container p-5"> 
      <div class="row mb-5"> 
         <div class="d-flex justify-content-center "> 
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5"> <asp:Literal runat="server" Text="<%$ Resources: PNUres, PNUinNumbers %>" />  
               <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1> 
         </div> 
      </div> 
      <div> 
         <div class="row border p-5 rounded-4"> 
            <asp:Repeater ID="rptStatistics" runat="server">
    <ItemTemplate>
        <div class="col-lg-3 col-md-6  mb-lg-0 mb-5 text-center">
            <h3 class="text-black"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEn")) %></h3>
            <p class="display-3 fw-bolder  text-primary"><%# Eval("Count") %> </p>
            <hr class="mt-5 mb-0 d-md-none d-block ">
        </div>

    </ItemTemplate>
</asp:Repeater>
         </div> 

          <div>
    <p class="mb-0 text-end">
        <a href="<%# String.Format("{0}AboutUniversity/Pages/PNUinNumbers.aspx", SPFactory.GetSiteURL()) %>" runat="server" class="btn btn-link fw-bolder text-primary text-decoration-none">
            <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />

        </a>
    </p>
</div>

      </div> 
   </div> </section> 