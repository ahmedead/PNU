<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClContact.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClContact" %>

<section class="py-5" aria-labelledby="central-library-visit-title">
    <div class="container">
        <% if (HeadingVisible) { %>
            <h2 class="h3 mb-4" id="central-library-visit-title"><%= HeadingText %></h2>
        <% } %>
        <div class="row g-4">
            <% if (HoursItem != null) { %>
                <div class="col-12 col-lg-6">
                    <article class="card h-100">
                        <div class="card-body d-flex flex-column gap-3">
                            <div class="icon-container"><i class='<%# "hgi hgi-stroke " + HoursItem.IconClass + " fs-3" %>' aria-hidden="true"></i></div>
                            <div>
                                <h3 class="h5 card-title"><%= HoursItem.Title %></h3>
                                <dl class="row mb-0">
                                    <% if (!string.IsNullOrEmpty(HoursItem.Row1Label)) { %>
                                        <dt class="col-5 col-sm-4 py-2"><%= HoursItem.Row1Label %></dt>
                                        <dd class="col-7 col-sm-8 py-2 mb-0"><span dir="ltr"><%= HoursItem.Row1Value %></span></dd>
                                    <% } %>
                                    <% if (!string.IsNullOrEmpty(HoursItem.Row2Label)) { %>
                                        <dt class="col-5 col-sm-4 py-2"><%= HoursItem.Row2Label %></dt>
                                        <dd class="col-7 col-sm-8 py-2 mb-0"><%= HoursItem.Row2Value %></dd>
                                    <% } %>
                                    <% if (!string.IsNullOrEmpty(HoursItem.Row3Label)) { %>
                                        <dt class="col-5 col-sm-4 py-2"><%= HoursItem.Row3Label %></dt>
                                        <dd class="col-7 col-sm-8 py-2 mb-0"><span dir="ltr"><%= HoursItem.Row3Value %></span></dd>
                                    <% } %>
                                </dl>
                            </div>
                        </div>
                    </article>
                </div>
            <% } %>

            <% if (ContactItem != null) { %>
                <div class="col-12 col-lg-6">
                    <article class="card nav-card h-100">
                        <div class="card-body d-flex flex-column gap-3">
                            <div class="icon-container"><i class='<%# "hgi hgi-stroke " + ContactItem.IconClass + " fs-3" %>' aria-hidden="true"></i></div>
                            <div>
                                <h3 class="h5 card-title"><%= ContactItem.Title %></h3>
                                <div class="d-flex flex-column gap-2">
                                    <% if (!string.IsNullOrEmpty(ContactItem.Email)) { %>
                                        <a class="d-flex align-items-center gap-2 text-decoration-none" href="mailto:<%= ContactItem.Email %>">
                                            <i class="hgi hgi-stroke hgi-mail-01 fs-5" aria-hidden="true"></i>
                                            <span dir="ltr"><%= ContactItem.Email %></span>
                                        </a>
                                    <% } %>
                                    <% if (!string.IsNullOrEmpty(ContactItem.Phone1)) { %>
                                        <a class="d-flex align-items-center gap-2 text-decoration-none" href="tel:<%= ContactItem.Phone1Tel %>">
                                            <i class="hgi hgi-stroke hgi-call fs-5" aria-hidden="true"></i>
                                            <span dir="ltr"><%= ContactItem.Phone1 %></span>
                                        </a>
                                    <% } %>
                                    <% if (!string.IsNullOrEmpty(ContactItem.Phone2)) { %>
                                        <a class="d-flex align-items-center gap-2 text-decoration-none" href="tel:<%= ContactItem.Phone2Tel %>">
                                            <i class="hgi hgi-stroke hgi-call fs-5" aria-hidden="true"></i>
                                            <span dir="ltr"><%= ContactItem.Phone2 %></span>
                                        </a>
                                    <% } %>
                                    <% if (!string.IsNullOrEmpty(ContactItem.LocationText)) { %>
                                        <div class="d-flex align-items-center gap-2">
                                            <i class="hgi hgi-stroke hgi-location-01 fs-5 text-primary" aria-hidden="true"></i>
                                            <span><%= ContactItem.LocationText %></span>
                                        </div>
                                    <% } %>
                                </div>
                            </div>
                            <% if (ContactItem.HasLink) { %>
                                <div class="d-flex justify-content-end mt-auto">
                                    <a class="btn btn-secondary stretched-link" href="<%= ContactItem.LinkUrl %>">
                                        <span><%= ContactItem.ButtonText %></span>
                                    </a>
                                </div>
                            <% } %>
                        </div>
                    </article>
                </div>
            <% } %>
        </div>
    </div>
</section>
