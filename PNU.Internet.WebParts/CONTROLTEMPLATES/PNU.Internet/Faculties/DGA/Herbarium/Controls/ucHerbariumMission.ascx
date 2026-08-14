<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbariumMission.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbariumMission, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5" aria-labelledby="herbarium-mission-title">
    <div class="container">
        <h2 class="h3 mb-4" id="herbarium-mission-title"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
        <div class="card bg-primary-25 border-0 mb-4">
            <div class="card-body p-4">
                <h3 class="h5 mb-3"><asp:Literal ID="ltMissionTitle" runat="server" /></h3>
                <p class="mb-0"><asp:Literal ID="ltMissionDesc" runat="server" /></p>
            </div>
        </div>
        <div class="accordion accordion-flush" id="herbariumObjectivesAccordion">
            <asp:Repeater ID="rptObjectives" runat="server">
                <ItemTemplate>
                    <div class="accordion-item">
                        <h3 class="accordion-header" id="<%# Eval("HeadingId") %>">
                            <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#<%# Eval("TargetId") %>" aria-expanded="false" aria-controls="<%# Eval("TargetId") %>">
                                <%# Eval("Title") %>
                            </button>
                        </h3>
                        <div class="accordion-collapse collapse" id="<%# Eval("TargetId") %>" aria-labelledby="<%# Eval("HeadingId") %>" data-bs-parent="#herbariumObjectivesAccordion">
                            <div class="accordion-body"><%# Eval("Description") %></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
