<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNsContact.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls.ucNsContact, $SharePoint.Project.AssemblyFullName$" %>

<section id="secContact" runat="server" class="py-5" aria-labelledby="student-contact-title">
    <div class="container">
        <asp:PlaceHolder ID="phHeading" runat="server">
            <div class="mb-4">
                <div class="d-flex justify-content-between align-items-center gap-2">
                    <h2 id="student-contact-title" class="mb-0"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                </div>
            </div>
        </asp:PlaceHolder>

        <div class="row g-4">
            <asp:Repeater ID="rptContact" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-6 col-md-6">
                        <article class="card h-100">
                            <div class="card-body p-4 d-flex gap-3 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                </span>
                                <div>
                                    <h3 class="fw-bold mb-1 h6"><%# Eval("Title") %></h3>
                                    <p class="mb-0"><%# Eval("ContactValueHtml") %></p>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
