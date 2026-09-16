<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgHeader.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgHeader" %>

<div class="bg-primary-25 py-5 pg-kit">
    <div class="container">
        <div class="content">
            <h1 class="mb-0 fs-64"><%= Model.Title %></h1>
        </div>

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

        <% if (Model.HasDescription) { %>
            <p class="mb-0 mt-3 fw-custom-bold fs-custom-4"><%= Model.Description %></p>
        <% } %>
    </div>
</div>
