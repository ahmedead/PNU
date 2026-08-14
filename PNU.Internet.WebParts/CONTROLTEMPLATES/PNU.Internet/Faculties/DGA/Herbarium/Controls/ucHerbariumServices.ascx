<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbariumServices.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbariumServices, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5" aria-labelledby="herbarium-services-title">
    <div class="container">
        <h2 class="h3 mb-4" id="herbarium-services-title"><asp:Literal ID="ltServicesTitle" runat="server" /></h2>
        <div class="row g-4">
            <asp:Repeater ID="rptServices" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-4">
                        <article class="card h-100">
                            <div class="card-body d-flex flex-column gap-3">
                                <div class="icon-container"><i class="<%# Eval("IconClass") %>" aria-hidden="true"></i></div>
                                <div>
                                    <h3 class="h5 card-title"><%# Eval("Title") %></h3>
                                    <p class="card-text mb-0"><%# Eval("Description") %></p>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
