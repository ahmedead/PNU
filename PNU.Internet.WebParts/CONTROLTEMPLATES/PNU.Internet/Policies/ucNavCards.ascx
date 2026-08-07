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
                                    <i class='<%# Eval("Icon") %> fs-3' aria-hidden="true"></i>
                                </div>
                                <div>
                                    <h3 class="card-title"><%# Eval("Title") %></h3>
                                </div>
                                <div class="d-flex justify-content-end mt-auto">
                                    <a class="btn btn-secondary stretched-link"
                                       href='<%# Eval("Url") %>'
                                       target='<%# Eval("Target") %>'
                                       rel='<%# Eval("Rel") %>'
                                       aria-label='<%# Eval("AriaLabel") %>'>
                                        <%# Eval("ButtonText") %>
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
