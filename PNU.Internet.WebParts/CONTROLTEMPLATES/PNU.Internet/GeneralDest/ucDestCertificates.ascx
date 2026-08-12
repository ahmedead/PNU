<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDestCertificates.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.GeneralDest.ucDestCertificates" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section id="agency-certificates-awards" class="pnu-section-anchor mb-5">
    <h2><%= SPContext.Current.ListItem["Title"] %></h2>
    <div class="row g-4 mt-1">
        <asp:Repeater ID="rptCerts" runat="server">
            <ItemTemplate>
                <div class="col-12 col-lg-6 col-md-6">
                    <article class="card h-100 pnu-news-card">
                        <div class="card-body d-flex flex-column placeholder-glow h-100">
                            <div class="media-card__cover">
                                <img
                                    width="400"
                                    height="250"
                                    class="w-100 rounded-2"
                                    alt="<%# Eval("Title") %>"
                                    loading="lazy"
                                    fetchpriority="low"
                                    src="<%# Eval("ImageUrl") %>"
                                    sizes="(min-width: 1200px) 360px, (min-width: 768px) 45vw, 90vw"
                                    decoding="async" />
                            </div>
                            <div class="flex-grow-1">
                                <h3 class="card-title mb-0">
                                    <%# Eval("Title") %>
                                </h3>
                            </div>
                            <div class="mt-auto d-flex flex-column gap-4">
                                <small class="d-flex gap-2 align-items-center">
                                    <i class="hgi hgi-stroke hgi-calendar-03"
                                        aria-hidden="true"></i>
                                    <time>
                                        <%# Eval("Date") %>
                                    </time>
                                </small>
                                <div class="d-flex flex-wrap mt-auto gap-2">
                                    <span class="badge badge-info">
                                        <%# Eval("Type") %>
                                    </span>
                                </div>
                                <%--<div class="d-flex justify-content-start">
                                    <button
                                        class="btn btn-primary media-preview-trigger"
                                        type="button"
                                        data-bs-toggle="modal"
                                        data-bs-target="#mediaPreviewModal"
                                        data-media-title="<%# Eval("Title") %>"
                                        data-media-src="<%# Eval("ImageUrl") %>">
                                        <%= Request.Url.AbsolutePath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                                            ? "عرض"
                                            : "View" %>
                                    </button>
                                </div>--%>
                                <div class="d-flex justify-content-start">
                                    <button
                                        class="btn btn-primary"
                                        type="button"
                                        onclick='window.open("<%# Eval("URL") %>", "_blank", "noopener,noreferrer");'>
                                        <%= Request.Url.AbsolutePath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                                            ? "عرض"
                                            : "View" %>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </article>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>

<div class="modal fade" id="mediaPreviewModal" tabindex="-1" aria-labelledby="mediaPreviewModalTitle" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header p-3">
                <h3 class="modal-title h5 m-0 p-0" id="mediaPreviewModalTitle"></h3>
                <button type="button"
                        class="btn btn-secondary icon-btn"
                        data-bs-dismiss="modal"
                        aria-label="إغلاق">
                    <span class="d-inline-flex fs-5">
                        <i class="hgi hgi-stroke hgi-cancel-01" aria-hidden="true"></i>
                    </span>
                </button>
            </div>
            <div class="modal-body px-3 py-0 pb-3">
                <img id="mediaPreviewImage"
                     class="img-fluid w-100 rounded-3"
                     src=""
                     alt="">
            </div>
        </div>
    </div>
</div>

<script>

    document.querySelectorAll(".media-preview-trigger").forEach(function (button) {

        button.addEventListener("click", function () {

            document.getElementById("mediaPreviewModalTitle").textContent =
                this.dataset.mediaTitle;

            document.getElementById("mediaPreviewImage").src =
                this.dataset.mediaSrc;

            document.getElementById("mediaPreviewImage").alt =
                this.dataset.mediaTitle;

        });

    });

</script>