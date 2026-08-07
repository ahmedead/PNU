<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucObjectivesOutcomes.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs.ucObjectivesOutcomes" %>






<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="my-5 py-5">
    <div class="container">
<div class="accordion-card">
    <asp:Repeater ID="rptMaster" runat="server">
        <ItemTemplate>
            <div class="accordion accordion-flush " id="accordionFlushObj<%# Eval("ID") %>">

                <div class="accordion-item  my-2 border-0">
                    <h2 class="accordion-header" id="flush-headingOne">
                        <button class="accordion-button collapsed border" type="button"
                            data-bs-toggle="collapse" data-bs-target="#flush-collapseObj<%# Eval("ID") %>" aria-expanded="false"
                            aria-controls="flush-collapseObj<%# Eval("ID") %>">
                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>  
                        </button>
                    </h2>

                    <div id="flush-collapseObj<%# Eval("ID") %>" class="accordion-collapse collapse "
                        aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushObj<%# Eval("ID") %>">
                        <div class="accordion-body p-0">


                            <table class="table rounded-bottom-4 fs-5 text-nowrap mb-0  table-striped-secondary overflow-hidden">
                                
                                
                                <tbody>
                                    <asp:Repeater ID="rptObjectivesOutcomes" runat="server" DataSource='<%# Eval("ObjectivesOutcomes") %>'>
                                        <ItemTemplate>
                                            <tr>
                                                
                                                <td class="text-start wrap-text"><%# Eval("Numbering") %></td>
                                                <td class="text-start wrap-text"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> 
                                                
                                            </tr>
                                            <tr>
                                            </tr>
                                            
                                        </ItemTemplate>
                                    </asp:Repeater>


                                </tbody>

                            </table>


                        </div>
                    </div>

                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>




</div>

        </div>
</section>
