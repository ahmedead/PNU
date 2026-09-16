<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/Sections/ucSectionPrograms.ascx" TagPrefix="uc1" TagName="ucSectionPrograms" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/Sections/ucSectionMembers.ascx" TagPrefix="uc1" TagName="ucSectionMembers" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/ucAcademicCredits.ascx" TagPrefix="uc1" TagName="ucAcademicCredits" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSectionHome.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.ucSectionHome" %>



<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>
<section id="faculty-department-0" class="pnu-section-anchor mb-5">
                                            
<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>
<h2><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h2>


        <%-- DGA page hero: breadcrumb + title --%>
        <div class="bg-primary-25 py-5" style="display:none">
            <div class="container">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-2">
                        <li class="breadcrumb-item small">
                            <a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>"><asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a>
                        </li>
                        <li class="breadcrumb-item small">
                            <a href="<%# String.Format("{0}Faculties/Pages/AllCollegesNew.aspx", SPFactory.GetSiteURL()) %>"><asp:Literal runat="server" Text="<%$ Resources: PNUres, Colleges %>" /></a>
                        </li>
                        <li class="breadcrumb-item small">
                            <a href="<%# String.Format("FacultyMain.aspx?Source={0}", Eval("COLL_CODE")) %>">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, FacultyTitle %>" />
                                <%# SPFactory.GetLocalizedTitle(Eval("COLL_DESC"), Eval("COLL_DESC_EN")) %>
                            </a>
                        </li>
                        <li class="breadcrumb-item small active" aria-current="page">
                            <span>
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, SectionTitle %>" />
                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                            </span>
                        </li>
                    </ol>
                </nav>
                <div class="content">
                    <h2 class="mb-0">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, SectionTitle %>" />
                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                    </h2>
                </div>
            </div>
        </div>

        <%-- About the department --%>
        <section class="container page-padding pnu-section-anchor">
            <h3 class="h5"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AboutSection %>" /></h3>
            <p class="mb-0"><%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %></p>
        </section>

    </ItemTemplate>
</asp:Repeater>

<uc1:ucSectionPrograms runat="server" id="ucSectionPrograms" />
<uc1:ucSectionMembers runat="server" id="ucSectionMembers" />
<uc1:ucAcademicCredits runat="server" id="ucAcademicCredits" />


</section>
<style>
    .breadcrumbhide { display: none; }
</style>
