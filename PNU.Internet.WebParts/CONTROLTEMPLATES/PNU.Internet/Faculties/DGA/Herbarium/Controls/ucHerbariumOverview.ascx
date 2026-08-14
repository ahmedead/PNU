<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbariumOverview.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbariumOverview, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5" aria-labelledby="herbarium-overview-title">
    <div class="container">
        <h2 class="h3 mb-4" id="herbarium-overview-title"><asp:Literal ID="ltOverviewTitle" runat="server" /></h2>
        <div class="card mb-4">
            <div class="card-body d-flex flex-column gap-3">
                <div class="icon-container"><i class="<asp:Literal ID='ltOverviewIcon' runat='server' />" aria-hidden="true"></i></div>
                <p class="mb-0"><asp:Literal ID="ltOverviewDesc" runat="server" /></p>
            </div>
        </div>
        <div class="row g-4">
            <asp:Repeater ID="rptStats" runat="server">
                <ItemTemplate>
                    <div class="col-6 col-lg-3">
                        <div class="card h-100">
                            <div class="card-body text-center">
                                <strong class="d-block h4 text-primary mb-2"><%# Eval("StatValue") %></strong>
                                <span><%# Eval("Title") %></span>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
