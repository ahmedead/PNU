<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucIntlBeforeArrival.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls.ucIntlBeforeArrival, $SharePoint.Project.AssemblyFullName$" %>

<section id="secBeforeArrival" runat="server" class="py-5" aria-labelledby="before-arrival-title">
    <div class="container">
        <asp:PlaceHolder ID="phHeading" runat="server">
            <div class="mb-4">
                <div class="d-flex justify-content-between align-items-center gap-2">
                    <h2 id="before-arrival-title" class="mb-0"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                </div>
            </div>
        </asp:PlaceHolder>

        <div class="row g-4">
            <asp:Repeater ID="rptBeforeArrival" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card nav-card h-100">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <span class="d-inline-flex fs-3">
                                        <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div>
                                    <h3 class="card-title"><%# Eval("Title") %></h3>
                                    <p class="card-text"><%# Eval("Description") %></p>
                                </div>
                                <%# Eval("ArrowLinkHtml") %>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
