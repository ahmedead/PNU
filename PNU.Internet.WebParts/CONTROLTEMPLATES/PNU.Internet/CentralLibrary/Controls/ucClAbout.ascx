<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClAbout.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClAbout" %>

<section class="py-5" aria-labelledby="central-library-about-title">
    <div class="container">
        <% if (HeadingVisible) { %>
            <h2 class="h3 mb-4" id="central-library-about-title"><%= Model.Title %></h2>
        <% } %>
        <div class="col-12 col-lg-12">
            <% if (!string.IsNullOrEmpty(Model.Paragraph1)) { %>
                <p class="text-justify"><%= Model.Paragraph1 %></p>
            <% } %>
            <% if (!string.IsNullOrEmpty(Model.Paragraph2)) { %>
                <p class="text-justify mb-0"><%= Model.Paragraph2 %></p>
            <% } %>
        </div>

        <% if (!string.IsNullOrEmpty(Model.ImageUrlLg)) { %>
            <picture>
                <% if (!string.IsNullOrEmpty(Model.ImageUrlSm)) { %>
                    <source media="(max-width: 576px)" srcset="<%= Model.ImageUrlSm %>">
                <% } %>
                <% if (!string.IsNullOrEmpty(Model.ImageUrlMd)) { %>
                    <source media="(max-width: 1200px)" srcset="<%= Model.ImageUrlMd %>">
                <% } %>
                <img class="img-fluid w-100 rounded-2 mt-4" width="1920" height="640"
                    src="<%= Model.ImageUrlLg %>" alt="<%= Model.ImageAlt %>"
                    loading="eager" decoding="async" fetchpriority="high">
            </picture>
        <% } %>

        <div class="row g-4 mt-2 numbers-section" aria-label="<%= Model.NumbersTitle %>">
            <asp:Repeater ID="rptNumbers" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-sm-6 col-lg-3" data-library-stat-item>
                        <div class="card h-100" id='<%# "central-library-stat-" + Eval("Id") %>'>
                            <div class="card-body p-4 d-flex align-items-center gap-3">
                                <div class="icon-container mb-0 flex-shrink-0">
                                    <i class='<%# "hgi hgi-stroke " + Eval("IconClass") + " fs-3" %>' aria-hidden="true"></i>
                                </div>
                                <div>
                                    <strong class="h2 fw-normal d-block mb-1 text-nowrap text-center" style="color:var(--dga-primary-800)" dir="ltr"><%# Eval("StatValue") %></strong>
                                    <span class="small" style="color:var(--dga-gray-800)"><%# Eval("Title") %></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
