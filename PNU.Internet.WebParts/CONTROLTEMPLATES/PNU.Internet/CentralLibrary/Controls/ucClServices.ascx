<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClServices.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClServices" %>

<section class="py-5" aria-labelledby="central-library-services-title">
    <div class="container">
        <% if (HeadingVisible) { %>
            <div class="mb-4"><h2 class="mb-0" id="central-library-services-title"><%= HeadingText %></h2></div>
        <% } %>
        <dga-swiper class="swiper dga-eservices-swiper pt-2" role="region" aria-label="<%= HeadingText %>">
            <swiper-container class="swiper-wrapper">
                <asp:Repeater ID="rptServices" runat="server">
                    <ItemTemplate>
                        <swiper-slide class="swiper-slide flex-grow-1" data-aos="fade-up" data-aos-delay="0">
                            <article class="card h-100">
                                <div class="card-body d-flex flex-column gap-4">
                                    <div class="icon-container">
                                        <span class="d-inline-flex fs-3"><i class='<%# "hgi hgi-stroke " + Eval("IconClass") + " fs-3" %>' aria-hidden="true"></i></span>
                                    </div>
                                    <div>
                                        <h3 class="card-title"><%# Eval("Title") %></h3>
                                        <p class="card-text"><%# Eval("Description") %></p>
                                    </div>
                                    <%# Eval("HasBadge").Equals(true) ? "<div class=\"d-flex flex-wrap mt-auto gap-2\"><span class=\"badge badge-info\">" + Eval("BadgeText") + "</span></div>" : "" %>
                                    <%# Eval("ButtonHtml") %>
                                </div>
                            </article>
                        </swiper-slide>
                    </ItemTemplate>
                </asp:Repeater>
            </swiper-container>
            <div class="d-flex justify-content-between align-items-center mt-3">
                <div class="d-flex gap-1">
                    <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-prev" aria-label="الخدمة السابقة"><span class="d-inline-flex fs-4 lh-1"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i></span></button>
                    <button type="button" class="btn btn-primary rounded-circle icon-btn dga-button-next" aria-label="الخدمة التالية"><span class="d-inline-flex fs-4 lh-1"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i></span></button>
                </div>
                <div class="d-flex justify-content-end dga-swiper-pagination"></div>
            </div>
        </dga-swiper>
    </div>
</section>
