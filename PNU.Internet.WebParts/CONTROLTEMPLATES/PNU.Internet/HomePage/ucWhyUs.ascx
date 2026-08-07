<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucWhyUs.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.ucWhyUs" %>



 <%@ Import Namespace="PNU.Internet.WebParts" %>


<section class="why-us">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12 col-md-6 mb-5 mb-md-0 mt-0 px-md-5 text-end">
                <div class="why-us-img position-relative">
                    <%-- Note: You can bind this image source dynamically if it's stored in the list --%>
                    <img loading="lazy"  src="/Style%20Library/Images/mage_xs4nrzxs4nrzxs4n.webp" class="img-fluid rounded-4 shadow" alt="PNU Environment" />
                </div>
            </div>

            <div class="col-12 col-md-6">
                <asp:Repeater ID="rptWhyUsMain" runat="server">
    <ItemTemplate>
                <h2 class="title fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 text-dark">
                     <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                </h2>
                <p class="text-muted h5 mb-5">
                    <%# SPFactory.GetLocalizedTitle(Eval("Desc"), Eval("Desc_EN")) %>
                </p>
                    </ItemTemplate>
</asp:Repeater>
                <div class="why-us-list">
                    <asp:Repeater ID="rptWhyUsDetails" runat="server">
                        <ItemTemplate>
                            <div class="d-flex align-items-center mb-4">
                                <div class="icon-box me-3">
                                    <svg aria-hidden="true" focusable="false" class="bi text-primary" width="45" height="45">
                                        <%-- Assumes your list has a field 'IconID' like #path3780 --%>
                                        <use xlink:href='<%# Eval("IconID") %>' />
                                    </svg>
                                </div>
                                <h3 class="text-dark fw-bold h5 mb-0">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                </h3>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</section>
