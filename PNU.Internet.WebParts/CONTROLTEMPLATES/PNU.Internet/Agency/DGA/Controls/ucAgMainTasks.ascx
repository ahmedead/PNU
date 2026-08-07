<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAgMainTasks.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA.ucAgMainTasks, $SharePoint.Project.AssemblyFullName$" %>

<section id="secMainTasks" runat="server" class="pb-5" data-aos="fade-up" aria-labelledby="agency-main-tasks-title">
    <div class="container">
        <asp:PlaceHolder ID="phHeading" runat="server">
            <div>
                <h2 id="agency-main-tasks-title" class="mb-4"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
            </div>
        </asp:PlaceHolder>

        <div class="accordion accordion-flush" id="agency-main-tasks-accordion">
            <asp:Repeater ID="rptGroups" runat="server">
                <ItemTemplate>
                    <div class="accordion-item">
                        <h4 class="accordion-header" id='<%# Eval("HeadingId") %>'>
                            <button class='<%# Eval("ButtonCss") %>' type="button" data-bs-toggle="collapse"
                                    data-bs-target='<%# Eval("CollapseTarget") %>'
                                    aria-expanded='<%# Eval("AriaExpanded") %>'
                                    aria-controls='<%# Eval("CollapseId") %>'>
                                <%# Eval("Title") %>
                            </button>
                        </h4>
                        <div class='<%# Eval("CollapseCss") %>' id='<%# Eval("CollapseId") %>'
                             aria-labelledby='<%# Eval("HeadingId") %>'
                             data-bs-parent="#agency-main-tasks-accordion">
                            <div class="accordion-body">
                                <%# Eval("BodyHtml") %>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
