<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllCollegeAds.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA.ucAllCollegeAds" %>


<%--
    DGA event cards (pnu-event-card). Same DTO conventions as the news control:
      DayName  -> localized day name (ar-SA / en-US) computed in the DTO
      DateText / DateIso / NavUrl computed before binding.
--%>

<h2 class="mb-4">
    <asp:Literal runat="server"
        Text="<%# System.Web.HttpContext.GetGlobalResourceObject("PNUres", "AllAdvertisements") %>" />
</h2>

<div class="row g-4">
    <asp:Repeater ID="rptAds" runat="server">
        <ItemTemplate>
            <div class="col-12 col-lg-4 col-md-6">
                <div class="card h-100 pnu-event-card">
                    <div class="card-body d-flex flex-column gap-3">
                        <div class="mt-0 d-flex flex-column gap-3">
                            <div>
                                <span class="badge badge-success"><%# Eval("DayName") %></span>
                            </div>
                            <small class="d-flex gap-2 align-items-center">
                                <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                <time datetime='<%# Eval("DateIso") %>'><%# Eval("DateText") %></time>
                            </small>
                            <h3 class="card-title"><%# Eval("Title") %></h3>
                        </div>
                        <div class="d-flex justify-content-start mt-auto">
                            <a class="btn btn-primary" href='<%# Eval("NavUrl") %>'>
                                <asp:Literal runat="server"
                                    Text="<%# System.Web.HttpContext.GetGlobalResourceObject("PNUres", "ViewDetails") %>" />
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
