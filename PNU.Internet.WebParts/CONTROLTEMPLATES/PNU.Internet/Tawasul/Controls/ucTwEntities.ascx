<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTwEntities.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls.ucTwEntities, $SharePoint.Project.AssemblyFullName$" %>

<section id="secEntities" runat="server" class="py-5" aria-labelledby="tw-entities-title">
    <div class="container">
        <asp:PlaceHolder ID="phHeading" runat="server">
            <div class="mb-4">
                <h2 id="tw-entities-title" class="mb-2"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                <p class="mb-0"><asp:Literal ID="ltSubtitle" runat="server" /></p>
            </div>
        </asp:PlaceHolder>

        <div class="row g-4">
            <asp:Repeater ID="rptEntities" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-6">
                        <article class="card h-100">
                            <div class="card-body p-4 d-flex flex-column gap-3">
                                <div>
                                    <h3 class="card-title h5 mb-2"><%# Eval("Title") %></h3>
                                </div>
                                <div class="d-flex flex-column gap-2 mt-auto">
                                    <%# Eval("EntityContactsHtml") %>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
