<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMemberLibrary.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucMemberLibrary" %>

<div class="row g-4">
    <asp:Repeater ID="rptLibraryHours" runat="server">
        <ItemTemplate>
            <div class="col-12 col-md-6">
                <article class="card nav-card h-100">
                    <div class="card-body">
                        <div class="icon-container">
                            <i class="hgi hgi-stroke hgi-calendar-03 fs-3" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h3 class="card-title"><%# Eval("Days1") %></h3>
                            <p class="card-text mb-0">
                                <%# DataBinder.Eval(Container.DataItem, "TimeFrom") %> - <%# DataBinder.Eval(Container.DataItem, "TimeTo") %>
                            </p>
                        </div>
                    </div>
                </article>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
