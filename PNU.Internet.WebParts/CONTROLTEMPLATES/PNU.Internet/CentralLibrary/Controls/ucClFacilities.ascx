<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClFacilities.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClFacilities" %>

<section class="py-5" aria-labelledby="central-library-facilities-title">
    <div class="container">
        <% if (HeadingVisible) { %>
            <div class="mb-4"><h2 class="mb-0" id="central-library-facilities-title"><%= HeadingText %></h2></div>
        <% } %>
        <div class="row g-4">
            <asp:Repeater ID="rptFacilities" runat="server" OnItemDataBound="rptFacilities_ItemDataBound">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100 pnu-news-card">
                            <div class="card-body d-flex flex-column placeholder-glow h-100">
                                <%# Eval("HasImage").Equals(true) ? "<img width=\"400\" height=\"250\" class=\"rounded-2 js-medium-zoom\" src=\"" + Eval("ImageUrl") + "\" alt=\"" + Eval("Title") + "\" loading=\"lazy\" sizes=\"(min-width: 1200px) 360px, (min-width: 768px) 45vw, 90vw\" decoding=\"async\">" : "" %>
                                <div class="flex-grow-1 mt-3">
                                    <h3 class="card-title"><%# Eval("Title") %></h3>
                                    <p class="card-text"><%# Eval("Description") %></p>
                                </div>
                                <div class="d-flex flex-wrap mt-auto gap-2 pt-3">
                                    <asp:Repeater ID="rptBadges" runat="server">
                                        <ItemTemplate>
                                            <span class="badge badge-neutral"><%# Container.DataItem %></span>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
