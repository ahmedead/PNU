<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucIntlNumbers.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls.ucIntlNumbers, $SharePoint.Project.AssemblyFullName$" %>

<section id="secNumbers" runat="server" class="py-5 numbers-section" aria-labelledby="international-numbers-title">
    <div class="container">
        <asp:PlaceHolder ID="phHeading" runat="server">
            <div class="mb-4">
                <div class="d-flex justify-content-between align-items-center gap-2">
                    <h2 id="international-numbers-title" class="mb-0"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                </div>
            </div>
        </asp:PlaceHolder>

        <div class="g-4 row pt-2">
            <asp:Repeater ID="rptNumbers" runat="server">
                <ItemTemplate>
                    <div class="col-6 col-lg-4 col-md-6">
                        <div class="border-0 card rounded-0">
                            <div class="align-items-center card-body">
                                <div class="icon-container">
                                    <span class="d-inline-flex">
                                        <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-3' aria-hidden="true"></i>
                                    </span>
                                </div>
                                <div class="text-center">
                                    <h3 class="display-3 fw-normal text-nowrap" style="color:var(--dga-primary-800)"><%# Eval("StatValue") %></h3>
                                    <span style="color:var(--dga-gray-800)"><%# Eval("Title") %></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
