<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDigitalMediaDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.DigitalMedia.ucDigitalMediaDetails" %>

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>
   <section class="breadcrumb">
      <div class="container py-5 my-3">
        <h1 class="display-6 fw-bold mb-3"> <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h1>
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb h6">
            <li class="breadcrumb-item"><a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>" class="text-decoration-none fw-bold">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a></li>
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}/{1}/", SPFactory.GetSiteURL(),"MediaCenter") %>" class="text-decoration-none fw-bold">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, MediaCenter %>" /></a></li>
                        
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}{1}", SPFactory.GetSiteURL(),"/MediaCenter/Pages/AllNews.aspx?Id=2") %>" class="text-decoration-none fw-bold">
						
						   <asp:Literal runat="server" Text="<%$ Resources: CommonGlobalResources, DigitalMedia %> " /></a></li>
            </li>
          </ol>
        </nav>
    </section>
	
	
    <section class="researcher-and-contributor ">
      <div class="container py-5 my-5">
        <div class="row">
          
             <div class="col-12  mt-3    mb-md-5 mb-2 d-flex justify-content-center" id="divImage" runat="server"  style='<%# "display:" +  Eval("ImgVisible") %>'>
            <div class="mx-4 pattern-border d-block video-container ">
              <img src='<%# Eval("AttachmentURL") %>' class=" rounded-3 " alt="..."  style='<%# "display:" +  Eval("ImgVisible") %>'>
            </div>
          </div>

		  <div class="col-12 order-lg-0 order-1 " id="divTitle" runat="server" >
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
              <span class="ms-2 text-muted"><%# Eval("MediaDate") %></span>
            </div>
            <p class="text-muted h5 mb-3 lh-base text-justify">
              <%# SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) %>
              <br>
              <br>
            </p>



          </div>

            <div id="divPublisher"   style='<%# "display:" +  Eval("PublisherVisible") %>'>
                <p><strong>الكاتب: <%# Eval("PublisherName") %></strong></p>
                <%--<p><strong><%# Eval("PublisherName") %></strong></p>--%>
                <p><strong><%# Eval("PublisherPosition") %></strong></p>
            </div>
		  
		 
		  <div id="divVideoYouTube" runat="server"  style='<%# "display:" +  Eval("MP4Visible") %>'>
		  
        <div class="video-wrapper" id="divVideo"  style='<%# "display:" +  Eval("VideoVisiable") %>'>
		
          <iframe class="responsive-iframe" src='<%# Eval("VideoURL") %>'></iframe>
        </div>
		
		</div>
          <div class="col-12  mt-3 mb-md-5 mb-2 d-flex  justify-content-center">
            <div class="mx-4 pattern-border d-block vplyr ">
              <div class=" rounded-3 " id="player2" data-plyr-provider="vimeo" data-plyr-embed-id="76979871"></div>
            </div>
          </div>
          

          


        </div>
        <br>
        
<style>
  .video-wrapper {
  position: relative;
  padding-bottom: 56.25%; /* 16:9 Aspect Ratio */
  height: 0;
  overflow: hidden;
}

.responsive-iframe {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  border: 0;
}

img.rounded-3
{
	max-width:700px;
	max-height:700px;
}
</style>
   
          
<style>
.breadcrumbhide {display: none;}
</style>
		
      </div>
    </section>
	
	
    </ItemTemplate>
</asp:Repeater>




<div class="row">
    <asp:Repeater ID="rptNews" runat="server">
        <ItemTemplate>
            <div class="col-md-6 col-lg-4 mb-4">
                <div class="card border rounded-4 h-100">
                    <div class="card-body p-2">
                        <div class="position-absolute top-0 start-0 m-4">
                            <span class="badge rounded-pill bg-tertiary text-white fs-6"><%# Eval("MediaTypes") %>
                            </span>
                        </div>
                        <img runat="server" id="dv_newsImg" src='<%# Eval("AttachmentURL") %>' class="object-fit-cover rounded-3 w-100" height="250" style='<%# ((Eval("IsVideo").ToString() == "True") ? "display:none": "display:block") %>'>
<div class="video-wrapper video-container" id="divVideo" runat="server" visible='<%# Eval("IsVideo") %>' >
    <video class="object-fit-cover for-thumbnail rounded-3 w-100" style="height: 250px; max-width: 100%; pointer-events: none;">
        <source src="<%# Eval("VideoURL") %>" />
    </video>
</div> 
                        <div class="my-2 fs-4 fw-bold">
                            <a href='<%#  Eval("DetailsURL")  %>' class="stretched-link text-decoration-none text-body strong">
                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> 
                            </a>
                        </div>
                        <div class="d-flex mb-3">
                            <svg width="19" height="20">
                                <use xlink:href="#calendar" />
                            </svg>
                            <span class="ms-2 text-muted"><%# Eval("MediaDate") %></span>
                        </div>


                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
           




<div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">
    <div class="py-3 d-flex flex-wrap d-md-block">
        <a id="AllMCNews" runat="server" class="btn btn-lg btn-outline-primary  px-5  me-3 mb-3  mb-md-5 flex-fill" href="">
            <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />

        </a>
    </div>
</div>
