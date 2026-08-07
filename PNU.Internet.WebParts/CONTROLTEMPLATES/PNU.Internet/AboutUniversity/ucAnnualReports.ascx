<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAnnualReports.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucAnnualReports" %>




                       

                       <table class="table rounded-bottom-4 fs-5 text-nowrap mb-0 border border-1  table-striped-secondary  overflow-hidden ">
                           <thead style=" --bs-table-accent-bg: white !important; ">
                               <tr class="my-2 align-middle" style="height: 60px;  --bs-table-accent-bg: white:  !important;">
                                   <th style=" --bs-table-accent-bg: white:  !important; " class="ps-4 text-start">
								   <asp:Literal  runat="server"
                                                               Text="<%$Resources:PNUres, res_FileName%>">
                                                           </asp:Literal>
								   </th>
                                   <th style=" --bs-table-accent-bg: white:  !important; " class="text-end pe-4">
								   <asp:Literal  runat="server"
                                                               Text="<%$Resources:PnuInternetResources, res_Download%>">
                                                           </asp:Literal>
								   </th>
                               </tr>
                           </thead>
                           <tbody>
                               <asp:Repeater ID="rptWeekly" runat="server">
                                   <ItemTemplate>
                                       <tr>
                                           <td class="ps-4 text-primary">        <%# Eval("FileName") %> </td>
                                           <td class="text-end pe-4">
                                               <a role="button" href="<%# Eval(" FilePath") %>" download="<%# Eval("FileName") %>" class="btn btn-link  ">
                                                   <div class="d-flex justify-content-center py-1">
                                                       <strong class="ms-2">
                                                           <asp:Literal ID="lit_download" runat="server"
                                                               Text="<%$Resources:PnuInternetResources, res_Download%>">
                                                           </asp:Literal>
                                                       </strong>
                                                     

                                                   </div>
                                               </a>
                                           </td>
                                       </tr>
                                   </ItemTemplate>
                               </asp:Repeater>
                           </tbody>
                       </table>
