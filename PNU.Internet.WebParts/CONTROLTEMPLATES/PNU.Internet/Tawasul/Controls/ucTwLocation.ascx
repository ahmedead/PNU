<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTwLocation.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls.ucTwLocation, $SharePoint.Project.AssemblyFullName$" %>

<section id="secLocation" runat="server" class="py-5" aria-labelledby="tw-location-title">
    <div class="container">
        <article class="card">
            <div class="card-body p-4">
                <h2 id="tw-location-title" class="mb-4"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>

                <asp:Literal ID="ltAddress" runat="server" />

                <asp:PlaceHolder ID="phMap" runat="server">
                    <div class="ratio ratio-16x9 rounded-3 overflow-hidden mt-4">
                        <asp:Literal ID="ltMap" runat="server" />
                    </div>
                </asp:PlaceHolder>
            </div>
        </article>
    </div>
</section>
