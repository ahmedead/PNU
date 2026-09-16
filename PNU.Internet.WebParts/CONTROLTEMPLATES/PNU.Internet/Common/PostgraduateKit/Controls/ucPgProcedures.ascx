<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgProcedures.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgProcedures" %>

<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-4 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>
    <% if (LeadVisible) { %>
        <p class="mb-0 fw-custom-bold fs-custom-4 mb-4"><%= LeadText %></p>
    <% } %>

    <asp:Repeater ID="rptGroups" runat="server">
        <ItemTemplate>
            <%# Eval("HasLabel").Equals(true) ? "<p class=\"my-3\"><small>" + Eval("Label") + "</small></p>" : "" %>

            <div class="row">
                <asp:Repeater runat="server" DataSource='<%# Eval("Items") %>'>
                    <ItemTemplate>
                        <div class="col-12 col-sm-6 mb-3">
                            <div class='<%# "card border-0 border-start border-primary " + Eval("AccentClass") + " border-5 border-custom-10 shadow-lg flex-fill h-100" %>'>
                                <div class="card-body gap-1 px-2 py-4">

                                    <h3 class="mb-0">
                                        <a class="d-flex pe-3 fw-custom-bold fs-custom-4 text-body custom-link-collapse" role="button"
                                           data-bs-toggle="collapse" aria-expanded="false"
                                           href='<%# Eval("CollapseHref") %>' aria-controls='<%# Eval("CollapseId") %>'>
                                            <span>
                                                <span><img src='<%# ToggleIconUrl %>' alt="" aria-hidden="true" loading="lazy" decoding="async"></span>
                                                <%# Eval("Title") %>
                                            </span>
                                            <%# Eval("HasBadge").Equals(true) ? "<span class=\"ms-auto badge bg-primary-text bg-custom-primary me-5\">" + Eval("Badge") + "</span>" : "" %>
                                        </a>
                                    </h3>

                                    <div class="collapse border-0" id='<%# Eval("CollapseId") %>'>
                                        <%# Eval("HasDescription").Equals(true) ? "<p class=\"mb-4 fs-custom-4\">" + Eval("Description") + "</p>" : "" %>

                                        <asp:PlaceHolder runat="server" Visible='<%# Eval("HasConditions") %>'>
                                            <div class="d-flex gap-2 my-5">
                                                <div><img src='<%# ConditionsIconUrl %>' alt="" aria-hidden="true" loading="lazy" decoding="async"></div>
                                                <div>
                                                    <h4 class="h6 fw-bold"><%# ConditionsLabel %></h4>
                                                    <%# Eval("ConditionsHtml") %>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>

                                        <asp:PlaceHolder runat="server" Visible='<%# Eval("HasArticle") %>'>
                                            <div class="d-flex gap-2 my-5">
                                                <div><img src='<%# ArticleIconUrl %>' alt="" aria-hidden="true" loading="lazy" decoding="async"></div>
                                                <div>
                                                    <h4 class="h6 fw-bold"><%# ArticleLabel %></h4>
                                                    <%# string.IsNullOrEmpty((string)Eval("ArticleTitle")) ? "" : "<p class=\"fw-bold mb-1\">" + Eval("ArticleTitle") + "</p>" %>
                                                    <%# Eval("ArticleTextHtml") %>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>

                                        <asp:PlaceHolder runat="server" Visible='<%# Eval("HasForms") %>'>
                                            <div class="d-flex gap-2 my-2">
                                                <div><img src='<%# FormsIconUrl %>' alt="" aria-hidden="true" loading="lazy" decoding="async"></div>
                                                <div><h4 class="h6 fw-bold mb-0"><%# Eval("FormsHeading") %></h4></div>
                                            </div>

                                            <asp:Repeater runat="server" DataSource='<%# Eval("Forms") %>'>
                                                <ItemTemplate>
                                                    <div class="list-group-item-primary my-2 py-2 px-3">
                                                        <div class="d-flex gap-2">
                                                            <div><img src='<%# FormIconUrl %>' alt="" aria-hidden="true" loading="lazy" decoding="async"></div>
                                                            <div><a class="text-body" href='<%# Eval("Url") %>'><%# Eval("Text") %></a></div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </asp:PlaceHolder>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
