<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgQuickLinks.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgQuickLinks" %>

<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-4 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>
    <% if (LeadVisible) { %>
        <p class="fw-custom-bold fs-custom-4 custom-paragraph mb-5"><%= LeadText %></p>
    <% } %>

    <div class="row align-items-stretch">
        <asp:Repeater ID="rptLinks" runat="server">
            <ItemTemplate>
                <div class="col-12 mb-3">
                    <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 border-custom-10 shadow-lg flex-fill h-100" %>'>
                        <div class="card-body gap-1 px-3 py-2">
                            <div class="d-flex justify-content-between gap-2">
                                <div>
                                    <a href='<%# Eval("SafeLinkUrl") %>' class="mb-0 fw-custom-bold text-body fs-custom-4">
                                        <%# Eval("HasIcon").Equals(true) ? "<img src=\"" + Eval("IconUrl") + "\" width=\"42\" alt=\"\" aria-hidden=\"true\" loading=\"lazy\" decoding=\"async\">" : "" %>
                                        <%# Eval("Title") %>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
