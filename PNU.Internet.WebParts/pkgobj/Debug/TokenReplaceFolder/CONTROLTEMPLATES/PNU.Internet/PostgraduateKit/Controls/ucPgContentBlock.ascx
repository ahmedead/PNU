<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgContentBlock.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgContentBlock" %>

<% if (RenderOuterContainer) { %>
<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-4 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>
    <% if (LeadVisible) { %>
        <p class="fw-custom-bold fs-custom-4 custom-paragraph mb-5"><%= LeadText %></p>
    <% } %>
<% } %>

    <asp:Repeater ID="rptBlocks" runat="server">
        <ItemTemplate>
            <%-- When rendered as a timeline step (e.g. within Axis Two) --%>
            <asp:PlaceHolder runat="server" Visible='<%# RenderAsStep %>'>
                <div class="d-flex gap-4 step">
                    <div class="d-flex align-items-center justify-content-center ps-5 position-relative step-icon">
                        <img src='<%# StepIconUrl %>' alt="" aria-hidden="true" loading="lazy" decoding="async">
                    </div>
                    <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 border-custom-10 shadow-lg flex-fill" %>'>
                        <div class="card-body gap-1 px-5 py-4">
                            <a class="d-flex pe-3 fw-custom-bold fs-custom-4 text-body custom-link-collapse" role="button"
                               data-bs-toggle="collapse" aria-expanded="false"
                               href='<%# Eval("CollapseHref") %>' aria-controls='<%# Eval("CollapseId") %>'>
                                <%# Eval("Title") %>
                                <%# Eval("HasBadge").Equals(true) ? "<span class=\"ms-auto badge bg-primary-text bg-custom-primary me-5\">" + Eval("Badge") + "</span>" : "" %>
                            </a>

                            <div class="collapse border-0" id='<%# Eval("CollapseId") %>'>
                                <%# Eval("ParagraphsHtml") %>
                                <%# Eval("BulletsHtml") %>
                                <asp:PlaceHolder runat="server" Visible='<%# Eval("HasFooterTitle") %>'>
                                    <h3 class="fw-bold mt-4"><%# Eval("FooterTitle") %></h3>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder runat="server" Visible='<%# Eval("HasFooter") %>'>
                                    <div class="list-group-item-primary my-4 py-2 px-3">
                                        <p class="fs-6 mb-0">
                                            <small><%# Eval("FooterParagraphsHtml") %></small>
                                        </p>
                                    </div>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>

            <%-- When rendered as a standalone card (e.g. EnglishCourse, PublishIncentive, FooterNote) --%>
            <asp:PlaceHolder runat="server" Visible='<%# !RenderAsStep %>'>
                <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 shadow-lg mb-4" %>'>

                    <%-- collapsible variant --%>
                    <asp:PlaceHolder runat="server" Visible='<%# Eval("Collapsible") %>'>
                        <div class="card-body gap-1 px-5 py-4">
                            <a class="d-flex pe-3 fw-custom-bold fs-custom-4 text-body custom-link-collapse" role="button"
                               data-bs-toggle="collapse" aria-expanded="false"
                               href='<%# Eval("CollapseHref") %>' aria-controls='<%# Eval("CollapseId") %>'>
                                <%# Eval("Title") %>
                                <%# Eval("HasBadge").Equals(true) ? "<span class=\"ms-auto badge bg-primary-text bg-custom-primary me-5\">" + Eval("Badge") + "</span>" : "" %>
                            </a>

                            <div class="collapse border-0" id='<%# Eval("CollapseId") %>'>
                                <%# Eval("ParagraphsHtml") %>
                                <%# Eval("BulletsHtml") %>
                                <asp:PlaceHolder runat="server" Visible='<%# Eval("HasFooterTitle") %>'>
                                    <h3 class="fw-bold mt-4"><%# Eval("FooterTitle") %></h3>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder runat="server" Visible='<%# Eval("HasFooter") %>'>
                                    <div class="list-group-item-primary my-4 py-2 px-3">
                                        <p class="fs-6 mb-0">
                                            <small><%# Eval("FooterParagraphsHtml") %></small>
                                        </p>
                                    </div>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <%-- plain variant --%>
                    <asp:PlaceHolder runat="server" Visible='<%# !(bool)Eval("Collapsible") %>'>
                        <div class="card-body gap-2 px-5 py-4">
                            <%# Eval("HasStatValue").Equals(true)
                                ? "<div class=\"d-flex gap-3 justify-content-end\"><div class=\"d-flex align-items-center\"><span class=\"h1 mb-0\">" + Eval("StatValue") + "</span></div>"
                                  + (Eval("HasIcon").Equals(true) ? "<div><img src=\"" + Eval("IconUrl") + "\" alt=\"\" aria-hidden=\"true\" loading=\"lazy\" decoding=\"async\"></div>" : "")
                                  + "</div>"
                                : "" %>

                            <%# Eval("HasTitle").Equals(true) ? "<h3 class=\"fw-bold fw-custom-bold fs-custom-4\">" + Eval("Title") + "</h3>" : "" %>
                            <%# Eval("HasSubTitle").Equals(true) ? "<h3 class=\"fw-bold fw-custom-bold fs-custom-4\">" + Eval("SubTitle") + "</h3>" : "" %>

                            <%# Eval("ParagraphsHtml") %>
                            <%# Eval("BulletsHtml") %>

                            <asp:Repeater runat="server" DataSource='<%# Eval("Highlights") %>' Visible='<%# Eval("HasHighlights") %>'>
                                <HeaderTemplate><ul class="list-group my-3"></HeaderTemplate>
                                <ItemTemplate>
                                    <li class="list-group-item list-group-item-light">
                                        <div class="d-flex gap-2">
                                            <%# Eval("HasIcon").Equals(true) ? "<div><img src=\"" + Eval("Icon") + "\" alt=\"\" aria-hidden=\"true\" loading=\"lazy\" decoding=\"async\"></div>" : "" %>
                                            <div class="text-body"><%# Eval("Text") %></div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate></ul></FooterTemplate>
                            </asp:Repeater>

                            <%# Eval("HasFooterTitle").Equals(true) ? "<h3 class=\"fw-bold fw-custom-bold fs-custom-4 mt-4\">" + Eval("FooterTitle") + "</h3>" : "" %>
                            <%# Eval("FooterParagraphsHtml") %>
                        </div>
                    </asp:PlaceHolder>

                </div>
            </asp:PlaceHolder>
        </ItemTemplate>
    </asp:Repeater>

<% if (RenderOuterContainer) { %>
</div>
<% } %>
