<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgFaq.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgFaq" %>

<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-4 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>
    <% if (LeadVisible) { %>
        <p class="fw-custom-bold fs-custom-4 custom-paragraph mb-5"><%= LeadText %></p>
    <% } %>

    <div class="row gap-5 steps">
        <asp:Repeater ID="rptFaq" runat="server">
            <ItemTemplate>
                <div class="d-flex gap-4">
                    <div class="d-flex align-items-start justify-content-center ps-5 position-relative faq-icon">
                        <img src='<%# FaqIconUrl %>' alt="" aria-hidden="true" loading="lazy" decoding="async">
                    </div>
                    <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 border-custom-10 shadow-lg flex-fill" %>'>
                        <div class="card-body gap-1 px-5 py-4">
                            <h3 class="mb-0">
                                <a class="fw-custom-bold fs-custom-4 text-body custom-link-collapse d-block" role="button"
                                   data-bs-toggle="collapse" aria-expanded="false"
                                   href='<%# Eval("CollapseHref") %>' aria-controls='<%# Eval("CollapseId") %>'>
                                    <%# Eval("Question") %>
                                </a>
                            </h3>
                            <div class="collapse border-0" id='<%# Eval("CollapseId") %>'>
                                <p class="fs-custom-4 mb-0 pt-3"><%# Eval("Answer") %></p>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
