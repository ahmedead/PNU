<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgJourney.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgJourney" %>

<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-4 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>
    <% if (LeadVisible) { %>
        <p class="fw-custom-bold fs-custom-4 custom-paragraph mb-5"><%= LeadText %></p>
    <% } %>

    <div class="row gap-5 steps">
        <asp:Repeater ID="rptSteps" runat="server">
            <ItemTemplate>
                <div class="d-flex gap-4 step">
                    <div class="d-flex align-items-center justify-content-center ps-5 position-relative step-icon">
                        <img src='<%# Eval("IconUrl") %>' alt="" aria-hidden="true" loading="lazy" decoding="async">
                    </div>
                    <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 border-custom-10 shadow-lg flex-fill" %>'>
                        <div class="card-body gap-1 px-5 py-4">
                            <%# Eval("HasSubTitle").Equals(true)
                                ? "<div class=\"d-flex gap-4 align-items-center\">"
                                    + "<div class=\"d-flex align-items-center justify-content-center\">"
                                        + "<span class=\"badge bg-primary-text bg-custom-primary\">" + Eval("SubTitle") + "</span>"
                                    + "</div>"
                                    + "<div class=\"\">"
                                        + "<h3 class=\"fw-bold\">" + Eval("TitleHtml") + "</h3>"
                                        + (Eval("HasDescription").Equals(true) ? "<p class=\"mb-0 fs-custom-4\">" + Eval("Description") + "</p>" : "")
                                    + "</div>"
                                + "</div>"
                                : "<h3 class=\"fw-bold\">" + Eval("TitleHtml") + "</h3>"
                                    + (Eval("HasDescription").Equals(true) ? "<p class=\"mb-0 fw-custom-bold fs-custom-4\">" + Eval("Description") + "</p>" : "")
                            %>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
