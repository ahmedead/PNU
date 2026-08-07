<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNsImportantDates.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls.ucNsImportantDates, $SharePoint.Project.AssemblyFullName$" %>

<section id="secImportantDates" runat="server" class="py-5" aria-labelledby="important-dates-title">
    <div class="container">
        <asp:PlaceHolder ID="phHeading" runat="server">
            <div class="mb-4">
                <div class="d-flex justify-content-between align-items-center gap-2">
                    <h2 id="important-dates-title" class="mb-0"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                </div>
            </div>
        </asp:PlaceHolder>

        <div class="row g-4">
            <asp:Repeater ID="rptGroups" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4">
                        <h3 class="h5 mb-3"><%# Eval("Title") %></h3>
                        <article class="card overflow-hidden">
                            <div class="list-group list-group-flush">
                                <asp:Repeater ID="rptDates" runat="server" DataSource='<%# Eval("Items") %>'>
                                    <ItemTemplate>
                                        <div class="list-group-item p-4">
                                            <div class="d-flex gap-3 align-items-center">
                                                <time datetime='<%# Eval("DateIso") %>' class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                                    <span class="d-block fs-3 fw-semibold lh-1"><%# Eval("DateDay") %></span>
                                                    <span class="small"><%# Eval("DateMonth") %></span>
                                                </time>
                                                <div class="flex-grow-1">
                                                    <h4 class="h6 mb-2"><%# Eval("Title") %></h4>
                                                    <small class="d-flex gap-2 align-items-center text-body-secondary">
                                                        <i class='hgi hgi-stroke <%# Eval("IconClass") %>' aria-hidden="true"></i>
                                                        <span><%# Eval("SubTitle") %></span>
                                                    </small>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
