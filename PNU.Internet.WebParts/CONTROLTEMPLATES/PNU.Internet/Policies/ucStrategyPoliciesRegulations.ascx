<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ucStrategyPoliciesRegulations.ascx.cs"
    Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies.ucStrategyPoliciesRegulations" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <asp:Repeater ID="rptSections" runat="server" OnItemDataBound="rptSections_ItemDataBound">
        <ItemTemplate>
            <section class='<%# Container.ItemIndex == 0 ? "py-5" : "pb-5" %>' data-aos="fade-up" aria-labelledby='<%# Attr(Eval("SectionKey")) %>-title'>
                <div class="container">
                    <div>
                        <h2 id='<%# Attr(Eval("SectionKey")) %>-title' class="mb-4"><%# Text(Eval("DisplayTitle")) %></h2>
                        <p class="mb-4"><%# Text(Eval("DisplayDescription")) %></p>
                    </div>
                    <div class="row g-4">
                        <asp:Repeater ID="rptCards" runat="server">
                            <ItemTemplate>
                                <div class="col-12 col-md-4 col-xl-4">
                                    <article class="card nav-card h-100">
                                        <div class="card-body">
                                            <div class="icon-container">
                                                <i class='<%# Attr(Eval("Icon")) %> fs-3' aria-hidden="true"></i>
                                            </div>
                                            <div>
                                                <h3 class="card-title"><%# Text(Eval("DisplayTitle")) %></h3>
                                                <%# RenderBadge(Eval("DisplayBadge"), Eval("BadgeClass")) %>
                                            </div>
                                            <div class="d-flex justify-content-end mt-auto">
                                                <a class="btn btn-secondary stretched-link"
                                                   href='<%# Href(Eval("DisplayUrl")) %>'
                                                   target='<%# Target(Eval("Target")) %>'
                                                   rel='<%# Attr(Eval("Rel")) %>'
                                                   aria-label='<%# Attr(Eval("AriaLabel")) %>'>
                                                    <%# Text(Eval("DisplayButtonText")) %>
                                                </a>
                                            </div>
                                        </div>
                                    </article>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </section>
        </ItemTemplate>
    </asp:Repeater>
</main>
