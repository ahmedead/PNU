<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucKnowMore.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SR.ucKnowMore" %>



<section class="also-know">
    <div class="container py-5 ">
        <div class="pt-5 mt-5 d-flex justify-content-center">
            <div>
                <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                    <asp:Label ID="lblWPTitle" runat="server"></asp:Label>

                    <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                </h1>
            </div>
        </div>
        
        <asp:Repeater ID="rep" runat="server">
            <HeaderTemplate>
                <div class="row my-4 pb-5">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="col-lg-4 col-md-12 ">
                    <a href='<%# Eval("URL") %>'>
                        <div class="card mb-4 p-0  also-know-card">

                            <asp:Image ID="Image1" runat="server" class="img-fluid" ImageUrl='<%# Eval("ImageLink") %>' alt="..." />

                            <div class="position-absolute bottom-0 text-center end-0 start-0">
                                <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white ">
                                    <asp:Label ID="lblTitle1" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                </h2>
                            </div>
                        </div>
                    </a>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</section>
