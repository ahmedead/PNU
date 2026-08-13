<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucDetails" %>




<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<style>
    img.ms-auto.d-block.me-5.img-fluid {
    max-width: 650px;
    min-width: 500px;
}
</style>

<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>
        <section class="breadcrumb">
            <div class="container py-5 my-3">
                <h1 class="display-6 fw-bold mb-3"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h1>
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb h6">
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>" class="text-decoration-none fw-bold">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a></li>
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}/{1}/", SPFactory.GetSiteURL(),"MediaCenter") %>" class="text-decoration-none fw-bold">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, MediaCenter %>" /></a></li>
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}{1}", SPFactory.GetSiteURL(),"/MediaCenter/Pages/AllNews.aspx") %>" class="text-decoration-none fw-bold">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, News %>" /></a></li>
                        <li class="breadcrumb-item active fw-bold text-turquoise-600" aria-current="page"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
            </li>
                    </ol>
                </nav>
        </section>
        <section class="researcher-and-contributor ">
      <div class="container py-5 my-5">
        <div class="">
          <div class=" mt-4 pb-5   ps-5 text-start float-end">
            <div class="faculty-dean-img position-relative">
              <img src='<%# Eval("AttachmentURL") %>' height="500" class="ms-auto d-block me-5 img-fluid" alt="...">
            </div>
          </div>
          <div class="col-12 order-lg-0 order-1 ">
            <div class=" mb-3 mt-md-0 mt-4">
              <h1 class="title text-dark fw-bold m-0">
                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
              </h1>
              <a href="#" class="d-block mt-3 text-decoration-none mb-3 mb-md-0">
                <svg width=" 18" height="16">
                  <use xlink:href="#share" />
                </svg>
                <span class="ms-2 text-primary">مشاركة</span>
              </a>
            </div>
            
            <div class="my-3">
              <svg width="19" height="20">
                <use xlink:href="#calendar" />
              </svg>
              <span class="ms-2 text-muted"><span class="ms-2 text-muted"><%# Eval("MediaDate") %></span></span>
            </div>
            <p class="text-muted h5 mb-3 lh-base text-justify">
               <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
              <br>
              <br>
            </p>
          </div>


        </div>
        <br>
        <p class="text-muted h5 mb-3 lh-base text-justify">
          <%# SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) %>
        </p>
        

          <div id="divVideo"  style='<%# "display:" +  Eval("VideoVisiable") %>'>
            

            <iframe height="450" src='<%# Eval("VideoURL") %>' frameborder="0" allowfullscreen="" style="width:100%;"></iframe>
			
			
        </div>
			

      </div>
    </section>


        
		
		
		
		
    </ItemTemplate>
</asp:Repeater>

<script type="text/javascript" src="https://lcgpa.gov.sa/Style%20Library/Portal/js/fancybox.umd.js"></script>
