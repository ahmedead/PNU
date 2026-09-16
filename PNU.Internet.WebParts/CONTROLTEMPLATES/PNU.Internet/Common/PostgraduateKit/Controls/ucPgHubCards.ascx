<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgHubCards.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgHubCards" %>

<section class="gray colored-section py-5 pg-kit" data-aos="fade-up" aria-labelledby="<%= HeadingId %>"
    <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <div class="container">

        <div class="mb-4">
            <% if (HeadingVisible) { %>
                <div class="d-flex justify-content-center align-items-center gap-2">
                    <h2 class="mb-0" id="<%= HeadingId %>"><%= HeadingText %></h2>
                </div>
            <% } %>
            <% if (LeadVisible) { %>
                <p class="mb-0 mt-3 text-center fs-6"><%= LeadText %></p>
            <% } %>
        </div>

        <dga-swiper class="swiper dga-eservices-swiper pt-2" role="region" aria-label="<%= HeadingText %>">
            <swiper-container class="swiper-wrapper">
                <asp:Repeater ID="rptCards" runat="server">
                    <ItemTemplate>
                        <swiper-slide class="swiper-slide flex-grow-1" data-aos="fade-up" data-aos-delay="0">
                            <article class="card h-100">
                                <div class="card-body d-flex flex-column gap-4 custom-card">
                                    <div class="icon-container">
                                        <span class="d-inline-flex fs-3">
                                            <i class="hgi hgi-stroke hgi-validation-approval fs-3" aria-hidden="true"></i>
                                            <%# Eval("HasIcon").Equals(true) ? "<img class=\"img-fluid custom-image\" src=\"" + Eval("IconUrl") + "\" alt=\"\" aria-hidden=\"true\" loading=\"lazy\" decoding=\"async\">" : "" %>
                                        </span>
                                    </div>

                                    <div>
                                        <h3 class="card-title"><%# Eval("Title") %></h3>
                                    </div>

                                    <%# Eval("HasButton").Equals(true)
                                        ? "<div class=\"d-flex gap-3 custom-flex-buttons\"><a class=\"btn btn-primary\" href=\"" + Eval("SafeLinkUrl") + "\">" + Eval("ButtonText") + "</a></div>"
                                        : "" %>

                                    <%# Eval("HasDescription").Equals(true) ? "<p class=\"card-text\">" + Eval("Description") + "</p>" : "" %>
                                </div>
                            </article>
                        </swiper-slide>
                    </ItemTemplate>
                </asp:Repeater>
            </swiper-container>

            <asp:PlaceHolder ID="phNav" runat="server">
                <div class="d-flex justify-content-between align-items-center mt-3">
                    <div class="d-flex gap-1">
                        <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-prev" aria-label="<%= PreviousLabel %>">
                            <span class="d-inline-flex fs-4 lh-1"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i></span>
                        </button>
                        <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-next" aria-label="<%= NextLabel %>">
                            <span class="d-inline-flex fs-4 lh-1"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i></span>
                        </button>
                    </div>
                    <div class="d-flex justify-content-end dga-swiper-pagination"></div>
                </div>
            </asp:PlaceHolder>
        </dga-swiper>

    </div>
</section>
