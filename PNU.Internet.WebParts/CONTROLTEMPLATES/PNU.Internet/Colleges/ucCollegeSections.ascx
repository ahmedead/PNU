<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeSections.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucCollegeSections" %>




<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


<style>
.faculty-members-section {
    padding: 100px 0;
    display: none;
}
</style>




<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>

        <section class="breadcrumb">
            <div class="container py-5 my-3">
                <h1 class="display-6 fw-bold mb-3">
                    <%--<asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyTitle %>" />
                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>--%>
                    <%# SPFactory.GetFacultyTitleLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                </h1>
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb h6">
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>" class="text-decoration-none fw-bold"><asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a></li>
                        <li class="breadcrumb-item"><a href="<%# String.Format("{0}Faculties/Pages/AllCollegesNew.aspx", SPFactory.GetSiteURL()) %>" class="text-decoration-none fw-bold"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Colleges %>" /></a></li>
                        <li class="breadcrumb-item active fw-bold text-turquoise-600" aria-current="page">
                            <%--<asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyTitle %>" />--%>
                            <%# SPFactory.GetFacultyTitleLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> 
                        </li>
                    </ol>
                </nav>
            </div>


        </section>

    </ItemTemplate>
</asp:Repeater>



<section class="our-sections position-relative">
    <div class="container">
        <div class="d-flex justify-content-center">
            <div>
                <h1 class="title text-white fw-bold px-2 border-start border-primary mb-5">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, OurSections %>" />
                    <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                </h1>
            </div>

        </div>
        <div class="position-relative also-know bg-transparent">
            <div class="swiperbox-bg-1 d-none d-lg-block"></div>
            <div dir="ltr" class="swiper swiper-container-5 me-5">
                <div class="swiper-wrapper">



                    <asp:Repeater ID="rptSections" runat="server">
                        <ItemTemplate>
                            <div class="swiper-slide bg-transparent">
                                <div class="card border-0 mb-4 p-0 overflow-hidden also-know-card">
                                    <img class="sec" src="<%# Eval("DisplayImage") %>"  alt="...">
                                    <a href=<%# String.Format("Sections.aspx?SecCode={0}&Source={1}", Eval("Code"),Eval("COLL_CODE")) %> " class="stretched-link text-decoration-none">
                                        <div class="position-absolute bottom-0 text-start ms-5 start-0">
                                            <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white "><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> </h2>
                                        </div>
                                    </a>
                                </div>
                            </div>

                        </ItemTemplate>
                    </asp:Repeater>





                </div>

            </div>
        </div>
        <div class="swiper-sections-navigation d-none d-lg-flex">
            <div class=" swiper-next me-2">
                <svg width="50" height="30">
                    <use xlink:href="#arrowRight" />
                </svg>
            </div>
            <div class="swiper-prev">
                <svg width="50" height="30">
                    <use xlink:href="#arrowLeft" />
                </svg>
            </div>
        </div>
        <div class="swiperbox-bg-2 d-none d-lg-block"></div>
    </div>

</section>


