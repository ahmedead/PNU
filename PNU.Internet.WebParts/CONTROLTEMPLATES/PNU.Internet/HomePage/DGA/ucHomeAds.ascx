<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeAds.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeAds" %>




<%@ Import Namespace="PNU.Internet.WebParts" %>


 <section class="gray colored-section py-5" data-aos="fade-up" aria-labelledby="ads-section-title">
   <div class="container">
     <div class="mb-4">
       <div class="d-flex justify-content-between align-items-center gap-2">
         <h2 id="ads-section-title" class="mb-0"><asp:Literal runat="server" Text="<%$ Resources: PNUres, LatestAdvs %>" /></h2>
         <a class="btn btn-outline-secondary fw-semibold"
           href="<%= SPFactory.GetSiteURL() %>MediaCenter/Pages/LatestAdvertisements.aspx">
             <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_MoreAllAdvsLink %>" />

         </a>
       </div>
     </div>

     <dga-swiper class="swiper dga-announcements-swiper pt-2" role="region" aria-label="<asp:Literal runat="server" Text="<%$ Resources: PNUres, LatestAdvs %>" />">
       <swiper-container class="swiper-wrapper">
                   <asp:Repeater ID="rptEvents" runat="server">
<ItemTemplate>
         <swiper-slide class="swiper-slide" data-aos="fade-up" data-aos-delay="0">
           <div class="card h-100 pnu-event-card">
             <div class="card-body d-flex flex-column gap-3">

               <div class="mt-0 d-flex flex-column gap-3">
                 <div>
                   <span class="badge badge-success "> <%# SPFactory.GetLocalizedTitle(Eval("dayAr"), Eval("dayEn")) %> </span>
                 </div>
                 <small class="d-flex gap-2 align-items-center">
                   <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                   <time datetime="<%# Eval("day") %> <%# Eval("FromDate") %>"><%# Eval("day") %> <%# Eval("FromDate") %></time>
                 </small>
                 <h3 class="card-title"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> </h3>
               </div>

               <div class="d-flex justify-content-start mt-auto">
                 <a class="btn btn-primary" href="<%# Eval("NavUrl") %>">
                   <asp:Literal runat="server" Text="<%$ Resources: PNUres, LatestAdvs %>" />
                 </a>
               </div>
             </div>
           </div>
         </swiper-slide>
                        </ItemTemplate>
</asp:Repeater>
       </swiper-container>
       <div class="d-flex justify-content-between align-items-center mt-3">
         <div class="d-flex gap-1">
           <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-prev" aria-label="السابق">
             <span class="d-inline-flex fs-4 lh-1">
               <i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i>
             </span>
           </button>
           <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-next" aria-label="التالي">
             <span class="d-inline-flex fs-4 lh-1">
               <i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i>
             </span>
           </button>
         </div>
         <div class="d-flex justify-content-end dga-swiper-pagination"></div>
       </div>
     </dga-swiper>
   </div>
 </section>

