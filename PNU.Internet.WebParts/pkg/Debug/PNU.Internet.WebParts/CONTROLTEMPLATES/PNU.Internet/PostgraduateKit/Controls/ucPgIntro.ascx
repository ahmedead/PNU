<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgIntro.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgIntro" %>

<div class="pg-kit" <%= SectionEnabled ? "" : "hidden=\"hidden\"" %>>
    <div class="card border-0 shadow-custom-lg">
        <div class="card-body gap-2 px-5 py-4 pg-intro-body">
            <% if (HeadingVisible) { %>
                <h2 class="mb-3" id="<%= HeadingId %>"><%= HeadingText %></h2>
            <% } %>
            <%= Body %>
        </div>
    </div>
</div>
