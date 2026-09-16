<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgResources.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgResources" %>

<% if (RenderOuterContainer) { %>
<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-1 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>
    <% if (LeadVisible) { %>
        <p class="fw-custom-bold fs-custom-4 custom-paragraph mb-5"><%= LeadText %></p>
    <% } %>

    <div class="row gap-5 steps">
<% } %>
        <asp:Repeater ID="rptResources" runat="server">
            <ItemTemplate>
                <div class="d-flex gap-4 step">
                    <div class="d-flex align-items-center justify-content-center ps-5 position-relative step-icon">
                        <img src='<%# Eval("IconUrl") %>' alt="" aria-hidden="true" loading="lazy" decoding="async">
                    </div>
                    <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 border-custom-10 shadow-lg flex-fill" %>'>
                        <div class="card-body gap-1 px-5 py-4">
                            <div class="d-flex justify-content-between gap-2">
                                <div>
                                    <h3 class="fw-bold"><%# Eval("TitleHtml") %></h3>
                                    <%# Eval("HasDescription").Equals(true) ? "<p class=\"mb-0 fw-custom-bold fs-custom-4\">" + Eval("DescriptionHtml") + "</p>" : "" %>
                                </div>
                                <%# (Eval("ShowDownload").Equals(true) && Eval("HasLink").Equals(true))
                                    ? "<div><a href=\"" + Eval("LinkUrl") + "\" aria-label=\"" + DownloadLabel + "\"><img src=\"" + DownloadIconUrl + "\" alt=\"\" aria-hidden=\"true\" loading=\"lazy\" decoding=\"async\"></a></div>"
                                    : "" %>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
<% if (RenderOuterContainer) { %>
    </div>

    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="text-muted"></asp:Panel>
</div>
<% } %>
