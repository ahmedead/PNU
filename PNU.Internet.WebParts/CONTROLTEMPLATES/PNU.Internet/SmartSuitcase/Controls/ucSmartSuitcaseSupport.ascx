<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSmartSuitcaseSupport.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls.ucSmartSuitcaseSupport, $SharePoint.Project.AssemblyFullName$" %>

<section id="secSupport" runat="server" class="py-5" aria-labelledby="smart-suitcase-support-title">
    <div class="container">
        <asp:PlaceHolder ID="phHeading" runat="server">
            <h2 class="h3 mb-4" id="smart-suitcase-support-title"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
        </asp:PlaceHolder>
        <div class="row">
            <asp:Repeater ID="rptSupport" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-12">
                        <article class="card nav-card h-100">
                            <div class="card-body d-flex flex-column gap-4">
                                <div class="icon-container">
                                    <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                </div>
                                <div>
                                    <h3 class="h5 card-title"><%# Eval("Title") %></h3>
                                    <p class="card-text"><%# Eval("Description") %></p>
                                    <div class="d-flex flex-column gap-2">
                                        <a class="d-flex align-items-center gap-2 text-decoration-none" href='<%# "mailto:" + Eval("ContactValue") %>'>
                                            <i class="hgi hgi-stroke hgi-mail-01 fs-5" aria-hidden="true"></i>
                                            <span dir="ltr"><%# Eval("ContactValue") %></span>
                                        </a>
                                        <div class="d-flex align-items-center gap-2">
                                            <i class="hgi hgi-stroke hgi-call fs-5 text-primary" aria-hidden="true"></i>
                                            <span>التحويلة الداخلية: <span dir="ltr">555</span></span>
                                        </div>
                                    </div>
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
