<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Gallery-WPUserControl.ascx.cs" Inherits="PNU_SP.webparts.Gallery_WP.Gallery_WPUserControl" %>


<div class="row">
    <div class="carousel slide" id="myCarousel" data-ride="carousel">
        <ul class="carousel-indicators" id="carouselIndicatordiv" runat="server"></ul>
        <div class="carousel-inner" id="carousaldiv" runat="server"></div>
        <a class="carousel-control left" href="#myCarousel" data-slide="prev" id="leftIcon" runat="server"></a>
        <a class="carousel-control right" href="#myCarousel" data-slide="next" id="rightIcon" runat="server"></a>
    </div>
</div>
