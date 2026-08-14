<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbariumMilestones.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbariumMilestones, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5" aria-labelledby="herbarium-milestones-title">
    <div class="container">
        <h2 class="h3 mb-4" id="herbarium-milestones-title"><asp:Literal ID="ltMilestonesTitle" runat="server" /></h2>
        <div class="row g-4">
            <asp:Repeater ID="rptMilestones" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-4">
                        <div class="card h-100">
                            <div class="card-body">
                                <span class="badge <%# Eval("BadgeClass") %> mb-3"><%# Eval("BadgeText") %></span>
                                <h3 class="h5"><%# Eval("Title") %></h3>
                                <p class="mb-0"><%# Eval("Description") %></p>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
