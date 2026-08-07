<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeStatistics.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucHomeStatistics" %>




<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>
<section class="numbers-about-research" style="margin-bottom: -22rem; padding-bottom: 22rem;">
    <div class="py-5">
        <div class="container mb-4">
            <div class="row">
                <asp:Repeater ID="rptStatistics" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-3 col-md-6  mb-lg-0 mb-5 text-center">
                            <h3 class="text-black"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEn")) %></h3>
                            <p class="display-3 fw-bolder  text-primary"><%# Eval("Count") %> </p>
                            <hr class="mt-5 mb-0 d-md-none d-block ">
                        </div>

                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
    </div>
</section>


