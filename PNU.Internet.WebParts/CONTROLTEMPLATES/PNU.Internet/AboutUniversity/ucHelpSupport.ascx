<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHelpSupport.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucHelpSupport" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <section class="py-5" data-aos="fade-up" aria-labelledby="help-support-title">
        <div class="container">
            <div class="row g-4">
                <asp:Repeater ID="rptHelpSupport" runat="server">
                    <ItemTemplate>
                        <div class="col-12 col-md-6 col-lg-4">
                            <div class="card nav-card h-100">
                                <div class="d-flex card-body flex-column gap-4">
                                    <div class="icon-container">
                                        <i class="hgi hgi-stroke <%# Eval("IconClass") %> fs-3" aria-hidden="true"></i>
                                    </div>
                                    <div>
                                        <h3 class="card-title"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEn")) %></h3>
                                        <p class="card-text"><%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("DescEn")) %></p>
                                    </div>
                                    <div class="d-flex justify-content-end mt-auto">
                                        <a class="btn btn-secondary stretched-link" href="<%# Eval("Link") %>" aria-label="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEn")) %>">
                                            <i class="hgi hgi-stroke <%# Eval("ArrowClass") %> fs-4" aria-hidden="true"></i>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</main>