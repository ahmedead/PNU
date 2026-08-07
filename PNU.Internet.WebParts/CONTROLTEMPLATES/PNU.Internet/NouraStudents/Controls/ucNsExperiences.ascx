<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNsExperiences.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls.ucNsExperiences, $SharePoint.Project.AssemblyFullName$" %>

<section id="secExperiences" runat="server" class="gray colored-section py-5" data-aos="fade-up" aria-labelledby="student-experiences-title">
    <div class="container">
        <div class="mb-4">
            <h2 id="student-experiences-title" class="mb-3"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
            <p class="mb-0"><asp:Literal ID="ltIntro" runat="server" /></p>
        </div>

        <div id="student-experiences-carousel" class="carousel slide" role="region"
             aria-roledescription="carousel" aria-label="<%# CarouselLabel %>" aria-live="off">

            <div class="carousel-inner">
                <asp:Repeater ID="rptExperiences" runat="server">
                    <ItemTemplate>
                        <div class='<%# Eval("SlideCss") %>' role="group" aria-roledescription="slide" aria-label='<%# Eval("SlideAriaLabel") %>'>
                            <div class="row g-4 g-lg-5 align-items-center justify-content-center">
                                <div class="col-12 col-lg-8">
                                    <article class="card">
                                        <div class="card-body">
                                            <div class="icon-container">
                                                <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                            </div>
                                            <h3 class="card-title"><%# Eval("Title") %></h3>
                                            <%# Eval("RoleHtml") %>
                                            <p class="card-text mb-0"><%# Eval("Description") %></p>
                                        </div>
                                    </article>
                                </div>
                                <div class="col-12 col-lg-3">
                                    <%# Eval("PortraitHtml") %>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="d-flex justify-content-between align-items-center mt-3">
                <div class="d-flex gap-1">
                    <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-prev"
                            data-bs-target="#student-experiences-carousel" data-bs-slide="prev"
                            aria-label="<%# PrevLabel %>">
                        <span class="d-inline-flex fs-4 lh-1">
                            <i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i>
                        </span>
                    </button>
                    <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-next"
                            data-bs-target="#student-experiences-carousel" data-bs-slide="next"
                            aria-label="<%# NextLabel %>">
                        <span class="d-inline-flex fs-4 lh-1">
                            <i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i>
                        </span>
                    </button>
                </div>

                <div class="carousel-indicators position-static m-0">
                    <asp:Repeater ID="rptIndicators" runat="server">
                        <ItemTemplate>
                            <button type="button" data-bs-target="#student-experiences-carousel"
                                    data-bs-slide-to='<%# Eval("SlideIndex") %>'
                                    class='<%# Eval("IndicatorCss") %>'
                                    aria-label='<%# Eval("IndicatorAriaLabel") %>' <%# Eval("IndicatorAriaCurrent") %>></button>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</section>
