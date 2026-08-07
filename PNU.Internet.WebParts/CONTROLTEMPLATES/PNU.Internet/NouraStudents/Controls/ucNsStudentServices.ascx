<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNsStudentServices.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls.ucNsStudentServices, $SharePoint.Project.AssemblyFullName$" %>

<section id="secStudentServices" runat="server" class="py-5" aria-labelledby="student-services-title">
    <div class="container">
        <div class="card bg-primary-25 border-0">
            <div class="card-body p-4 p-lg-5">
                <div class="row g-5 align-items-center">
                    <div class="col-12 col-lg-7">
                        <asp:Image ID="imgSection" runat="server" CssClass="img-fluid rounded-3 w-100" />
                    </div>
                    <div class="col-12 col-lg-5">
                        <h2 id="student-services-title" class="mb-3"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                        <p><asp:Literal ID="ltIntro" runat="server" /></p>

                        <div class="d-flex flex-column gap-3">
                            <asp:Repeater ID="rptServices" runat="server">
                                <ItemTemplate>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                        </span>
                                        <span><%# Eval("Title") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
