<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAgDeputyWord.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA.ucAgDeputyWord, $SharePoint.Project.AssemblyFullName$" %>

<section id="secDeputyWord" runat="server" aria-labelledby="agency-deputy-welcome-title">
    <div class="card mb-4 bg-primary-25 border-0">
        <div class="card-body p-4 p-lg-5">
            <div class="d-flex flex-column gap-3">
                <span class="icon-container bg-white">
                    <asp:Literal ID="ltIcon" runat="server" />
                </span>
                <h2 id="agency-deputy-welcome-title" class="mb-0"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                <p class="lead mb-0 text-justify"><asp:Literal ID="ltBody" runat="server" /></p>
            </div>
            <div class="card-body">
                <div>
                    <asp:Literal ID="ltRole" runat="server" />
                    <asp:Literal ID="ltOrg" runat="server" />
                </div>
            </div>
        </div>
    </div>
</section>
