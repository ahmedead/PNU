<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClHeader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClHeader" %>

<div class="bg-primary-25 py-5">
    <div class="container">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2">
                <asp:Repeater ID="rptBreadcrumb" runat="server">
                    <ItemTemplate>
                        <li class='<%# Eval("IsLast").Equals(true) ? "breadcrumb-item small active" : "breadcrumb-item small" %>'
                            <%# Eval("IsLast").Equals(true) ? "aria-current=\"page\"" : "" %>>
                            <%# Eval("IsLast").Equals(true) ? "<span>" + Eval("Title") + "</span>" : "<a href=\"" + Eval("Url") + "\">" + Eval("Title") + "</a>" %>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ol>
        </nav>
        <div class="row g-4">
            <div class="col-12 col-lg-12">
                <div class="d-flex flex-column flex-lg-row align-items-start align-items-lg-center justify-content-between gap-3">
                    <h1 class="mb-0 fw-semibold h2"><%= Model.Title %></h1>
                    <div class="d-flex flex-wrap gap-2">
                        <% if (!string.IsNullOrEmpty(Model.Button1Text)) { %>
                            <a class="btn btn-primary" href="<%= Model.Button1Url %>" target="_blank" rel="noopener"><%= Model.Button1Text %></a>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(Model.Button2Text)) { %>
                            <a class="btn btn-outline-secondary" href="<%= Model.Button2Url %>" target="_blank" rel="noopener"><%= Model.Button2Text %></a>
                        <% } %>
                    </div>
                </div>
                <p class="mb-0 mt-3"><%= Model.Description %></p>
            </div>
        </div>
    </div>
</div>
