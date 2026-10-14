<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ucNavCards.ascx.cs"
    Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies.ucNavCards" %>

<section data-aos="fade-up" class="py-5">
    <div class="container">
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
