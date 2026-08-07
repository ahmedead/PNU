<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemDetails-WPUserControl.ascx.cs" Inherits="PNU_SP.webparts.ItemDetails_WP.ItemDetails_WPUserControl" %>


<section class="researcher-and-contributor ">
      <div class="container pt-5 mt-5">
        <div class="row">
          <div class="col-12 col-lg-7 order-lg-0 order-1 ">
            <div class="d-flex flex-wrap justify-content-between align-items-center mb-2 mt-md-0 mt-4">
              <h1 class="title text-dark fw-bold m-0">
                   <asp:Label ID="lblTitle" runat="server"></asp:Label>
              </h1>
              <a href="#" class="text-decoration-none mb-3 mb-md-0">
                <svg width=" 18" height="16">
                  <use xlink:href="#share"></use>
                </svg>
                <span class="ms-2 text-primary">مشاركة</span>
              </a>
            </div>
            <div class="my-3">
              <span class="badge rounded-pill bg-turquoise-100 text-turquoise-900 fs-6">
                <svg class="align-bottom" width="20" height="18">
                  <use xlink:href="#wordIcon"></use>
                </svg>
                DOC. 6.8 MB
              </span>
            </div>
            <div class="my-3">
              <span class="badge rounded-pill bg-resonant-blue-100 text-secondary">
                  <asp:Label ID="lblTag" runat="server"></asp:Label>
              </span>
              
            </div>
            <div class="my-3">
              <svg width="19" height="20">
                <use xlink:href="#calendar"></use>
              </svg>
              <span class="ms-2 text-muted"> 
                  <asp:Label ID="lblDate" runat="server"></asp:Label>

              </span>
            </div>
            <p class="text-muted h5 mb-3 lh-base">
               <asp:Label ID="lblDesc" runat="server"></asp:Label>
            </p>
            
              <div class="d-flex justify-content-center py-1">
                <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" />
              </div>
         
          </div>
          <div class="col-12 col-lg-5 mt-4 pb-5  mb-5  text-start">
            <div class="faculty-dean-img position-relative">
                <asp:Image ID="img" runat="server"  height="500" class="ms-auto d-block me-5 img-fluid" />
            </div>
          </div>

        </div>
      </div>
    </section>