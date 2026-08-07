<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNsAcademicServices.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls.ucNsAcademicServices, $SharePoint.Project.AssemblyFullName$" %>

<section id="secAcademicServices" runat="server" class="gray colored-section py-5" data-aos="fade-up" aria-labelledby="academic-services-title">
    <div class="container">
        <div class="mb-4">
            <div class="d-flex justify-content-between align-items-start gap-2">
                <h2 id="academic-services-title" class="mb-0"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                <asp:HyperLink ID="lnkGuide" runat="server" CssClass="btn btn-outline-secondary fw-semibold" />
            </div>
        </div>

        <dga-swiper class="swiper dga-eservices-swiper pt-2">
            <swiper-container class="swiper-wrapper" aria-live="polite">
                <asp:Repeater ID="rptServices" runat="server">
                    <ItemTemplate>
                        <swiper-slide class="swiper-slide flex-grow-1" data-aos="fade-up" role="group">
                            <article class="card h-100">
                                <div class="card-body d-flex flex-column gap-4">
                                    <div class="icon-container">
                                        <span class="d-inline-flex fs-3">
                                            <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                        </span>
                                    </div>
                                    <div>
                                        <h3 class="card-title"><%# Eval("Title") %></h3>
                                        <%# Eval("DescriptionHtml") %>
                                    </div>
                                    <%# Eval("BadgeHtml") %>
                                    <%# Eval("ButtonHtml") %>
                                </div>
                            </article>
                        </swiper-slide>
                    </ItemTemplate>
                </asp:Repeater>
            </swiper-container>

            <div class="d-flex justify-content-between align-items-center mt-3">
                <div class="d-flex gap-1">
                    <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-prev" aria-label="Previous slide">
                        <span class="d-inline-flex fs-4 lh-1">
                            <i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i>
                        </span>
                    </button>
                    <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-next" aria-label="Next slide">
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
