<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgDiscounts.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgDiscounts" %>

<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-4 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>
    <% if (LeadVisible) { %>
        <p class="fw-custom-bold fs-custom-4 custom-paragraph mb-5"><%= LeadText %></p>
    <% } %>

    <ul class="list-group">
        <li class="list-group-item list-group-item-primary" aria-current="true">
            <div class="d-flex justify-content-between gap-2">
                <div class="col text-center text-body fw-bold"><%= CategoryHeader %></div>
                <div class="col text-center text-body fw-bold"><%= ValueHeader %></div>
            </div>
        </li>
        <asp:Repeater ID="rptRows" runat="server">
            <ItemTemplate>
                <li class='<%# Eval("RowClass") %>'>
                    <div class="d-flex justify-content-between gap-2">
                        <div class="col text-center text-body fw-bold"><%# Eval("Category") %></div>
                        <div class="col text-center text-body fw-bold"><%# Eval("Value") %></div>
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>

    <asp:Panel ID="pnlNotes" runat="server" Visible="false">
        <p class="fw-custom-bold fs-custom-4 custom-paragraph my-4"><%= NotesHeading %></p>

        <div class="row align-items-stretch">
            <asp:Repeater ID="rptNotes" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-sm-6 mb-3">
                        <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 border-custom-10 shadow-lg flex-fill h-100" %>'>
                            <div class="card-body gap-1 px-3 py-2">
                                <p class="mb-0 fw-custom-bold fs-custom-4"><%# Eval("Title") %></p>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
</div>
