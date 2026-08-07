<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServicesList.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices.Admin.ServicesList" %>


  <div class="container">
<asp:Repeater ID="rpt" runat="server">
    <HeaderTemplate>
        <table class="table table-bordered fs-5">
            <thead>
                <tr class="my-2 align-middle">
                    <th class="ps-4 text-start">Service Id</th>
                    <th class="text-center">Service Name</th>
                    <th class="text-center">Service Description</th>

                    <th class="text-center">Action</th>

                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td class="ps-4 text-primary">
                <%# Eval("Id") %>
            </td>
            <td class="ps-4 text-primary">
                <%# Eval("Title") %>
            </td>
            <td class="ps-4 text-primary">
                <%# Eval("Desc") %>
            </td>
            <td class="text-center">
                <a href='EditService.aspx?ServiceId=<%#Eval("UniqueId") %>'>Edit</a>
            </td>
        </tr>

    </ItemTemplate>
    <FooterTemplate>
        </tbody>
           </table>
    </FooterTemplate>
</asp:Repeater>
      </div>
