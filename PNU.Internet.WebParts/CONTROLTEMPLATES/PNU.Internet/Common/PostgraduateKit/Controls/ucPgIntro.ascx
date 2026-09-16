<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgIntro.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgIntro" %>

<div class="col-12 mb-5 pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <% if (HeadingVisible) { %>
        <h2 class="custom-title mb-4 fw-bold" id="<%= HeadingId %>"><%= HeadingText %></h2>
    <% } %>

    <div class="card border-0 border-start border-primary border-dark-blue border-5 shadow-lg">
        <div class="card-body gap-2 px-5 py-4 pg-intro-body">
            <%= Body %>
        </div>
    </div>
</div>
