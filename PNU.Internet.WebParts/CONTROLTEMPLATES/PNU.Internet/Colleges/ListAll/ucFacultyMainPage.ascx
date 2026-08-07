<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/ucCollegePrograms.ascx" TagPrefix="uc1" TagName="ucCollegePrograms" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/ucCollegeSections.ascx" TagPrefix="uc1" TagName="ucCollegeSections" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFacultyMainPage.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ListAll.ucFacultyMainPage" %>



<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


<style>
.faculty-members-section {
    padding: 100px 0;
    display: none;
}
</style>

<style>
.breadcrumbhide {display: none;}
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

        

        <section>
            <div class="container pt-5 my-5">
                <div class="d-flex align-items-start  mb-4 mt-5">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-5 me-md-4 flex-shrink-0">
                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AboutFaculty %>" />
			<span class="px-2 position-absolute mt-1 h2 text-primary">•</span></h1>
                </div>
                <p class="fs-5 mb-5 text-muted" style="text-align:justify;"><%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %> </p>
            </div>

        </section>
    </ItemTemplate>
</asp:Repeater>


<uc1:ucCollegeSections runat="server" id="ucCollegeSections" />



<uc1:ucCollegePrograms runat="server" id="ucCollegePrograms" />

