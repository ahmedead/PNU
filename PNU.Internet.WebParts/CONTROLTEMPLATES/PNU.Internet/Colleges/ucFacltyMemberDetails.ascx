<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFacltyMemberDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucFacltyMemberDetails" %>


<section class="faculty-member-details">
    <asp:Repeater ID="rptFacultyMembers" runat="server">
        <ItemTemplate>


            <div class="container py-5 my-5">
                <div class="hero-section">
                    <div class="card border-0 p-md-3 p-lg-5">
                        <div class="card-body">
                            <h1 class="title text-dark fw-bold px-2 border-start border-primary"><%#DataBinder.Eval(Container.DataItem,"Name") %>
                <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                            </h1>
                            <span class="text-resonant-blue-400"><%#DataBinder.Eval(Container.DataItem,"Title") %></span>
                            <div class="row fs-4 fw-bold mt-4">
                                <div class="col-md-4 col-lg-3 col-xl-2 text-light-emphasis text-nowrap">البريد الإلكتروني :</div>
                                <div class="col"><%#DataBinder.Eval(Container.DataItem,"Email") %></div>
                            </div>
                            <div class="row fs-4 fw-bold mb-4">
                                <div class="col-md-4 col-lg-3 col-xl-2 text-light-emphasis text-nowrap">رقم الهاتف :</div>
                                <div class="col"><%#DataBinder.Eval(Container.DataItem,"Phone") %></div>
                            </div>
                            <a href="<%#DataBinder.Eval(Container.DataItem,"LinkedInUrl") %>">
                                <svg width="58" height="58">
                                    <use xlink:href="#linkedinCircle"></use>
                                </svg>
                            </a>
                        </div>
                    </div>
                </div>
                <h1 class="title text-dark fw-bold px-2 border-start border-primary mt-5 mb-4">السيرة الذاتية
          <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                </h1>
                <p class="text-light-emphasis col-lg-8"><%#DataBinder.Eval(Container.DataItem,"Bio") %>
                </p>

                <h1 class="title text-dark fw-bold px-2 border-start border-primary my-5">المواد الدراسية
          <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                </h1>
                <div class="d-flex align-items-center flex-wrap flex-sm-nowrap justify-content-center justify-content-sm-start">
                    <div class="text-in-circle flex-shrink-0 mb-4 mb-sm-0">
                        <div><%#DataBinder.Eval(Container.DataItem,"SubjectTitle1") %></div>
                    </div>
                    <p class="col-md-8 col-lg-4 text-light-emphasis mb-0 ms-4">
                        <%#DataBinder.Eval(Container.DataItem,"SubjectDesc1") %>
                    </p>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</section>
	
