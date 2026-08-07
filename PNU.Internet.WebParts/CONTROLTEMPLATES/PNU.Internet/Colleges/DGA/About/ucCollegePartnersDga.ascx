<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegePartnersDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA.About.ucCollegePartnersDga" %>



<%-- International partners --%>
<asp:PlaceHolder ID="phPartners" runat="server">
    <section class="pb-5" data-aos="fade-up" aria-labelledby="international-partners-title">
        <div class="container">
            <div>
                <h2 id="international-partners-title" class="mb-4"><asp:Literal ID="ltrPartnersTitle" runat="server" /></h2>
                <p class="mb-4"><asp:Literal ID="ltrPartnersIntro" runat="server" /></p>
            </div>
            <div class="d-flex flex-wrap gap-3">
                <asp:Repeater ID="rptPartners" runat="server">
                    <ItemTemplate>
                        <div class="card text-center related-entity-card">
                            <div class="card-body placeholder-glow p-3">
                                <a target="_blank" rel="noopener noreferrer"
                                    class="d-flex align-items-center justify-content-center h-100"
                                    href='<%# Eval("LinkUrl") %>' aria-label='<%# Eval("DisplayTitle") %>'>
                                    <img class="img-fluid mx-auto d-block"
                                        src='<%# Eval("LogoUrl") %>'
                                        alt='<%# Eval("DisplayTitle") %>' loading="lazy" decoding="async">
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:PlaceHolder>

<%-- Accreditations --%>
<asp:PlaceHolder ID="phAccreditations" runat="server">
    <section class="pb-5" data-aos="fade-up" aria-labelledby="accreditations-title">
        <div class="container">
            <div>
                <h2 id="accreditations-title" class="mb-4"><asp:Literal ID="ltrAccredTitle" runat="server" /></h2>
                <p class="mb-4"><asp:Literal ID="ltrAccredIntro" runat="server" /></p>
            </div>
            <div class="row g-4">
                <asp:Repeater ID="rptAccreditations" runat="server">
                    <ItemTemplate>
                        <div class="col-12 col-lg-5 col-md-5">
                            <article class="card h-100 pnu-news-card">
                                <div class="card-body d-flex flex-column placeholder-glow h-100">
                                    <img width="400" height="250" class="rounded-2 js-medium-zoom"
                                        alt='<%# Eval("DisplayTitle") %>' loading="lazy"
                                        src='<%# Eval("ImageUrl") %>' decoding="async">
                                    <div class="flex-grow-1">
                                        <h3 class="card-title"><%# Eval("DisplayTitle") %></h3>
                                        <p class="card-text line-clamp max-clamp-line-4"><%# Eval("DisplayText") %></p>
                                    </div>
                                    <div class="mt-auto d-flex flex-column gap-3">
                                        <small class="d-flex gap-2 align-items-center">
                                            <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                            <time datetime='<%# Eval("DateIso") %>'><%# Eval("DateDisplay") %></time>
                                        </small>
                                    </div>
                                </div>
                            </article>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:PlaceHolder>
