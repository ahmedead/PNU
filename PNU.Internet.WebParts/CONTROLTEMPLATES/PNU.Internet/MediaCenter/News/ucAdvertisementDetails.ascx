<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdvertisementDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucAdvertisementDetails" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>

        <!-- DGA Breadcrumb Hero -->
        <div class="bg-primary-25 py-5 breadcrumbnew">
            <div class="container">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-2">
                        <li class="breadcrumb-item small">
                            <a href="<%# SPFactory.GetSiteURL() %>">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a>
                        </li>
                        <li class="breadcrumb-item small">
                            <a href="<%# String.Format("{0}/MediaCenter/", SPFactory.GetSiteURL()) %>">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, MediaCenter %>" /></a>
                        </li>
                        <li class="breadcrumb-item small">
                            <a href="<%# String.Format("{0}/MediaCenter/Pages/LatestAdvertisements.aspx", SPFactory.GetSiteURL()) %>">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, LatestAdvs %>" /></a>
                        </li>
                        <li class="breadcrumb-item small active" aria-current="page">
                            <span><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></span>
                        </li>
                    </ol>
                </nav>
                <div class="content">
                    <h2 class="mb-0"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h2>
                </div>
            </div>
        </div>

        <!-- DGA Detail Article -->
        <main id="main-content" class="dga-main-body" tabindex="-1">
            <section class="py-5" data-aos="fade-up" aria-labelledby="event-detail-title">
                <div class="container">
                    <article class="pnu-detail-article">

                        <!-- Image -->
                        <div class="text-center" id="divImage" runat="server" style='<%# "display:" + Eval("ImgVisible") %>'>
                            <img class="img-fluid w-100 rounded-3"
                                alt="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>"
                                loading="eager" fetchpriority="high" decoding="async"
                                src='<%# Eval("AttachmentURL") %>'
                                style='<%# "display:" + Eval("ImgVisible") %>' />
                        </div>

                        <!-- Meta + Share -->
                        <div class="d-flex flex-column flex-lg-row gap-3 justify-content-between align-items-lg-center mt-3"
                            id="divTitle" runat="server" style='<%# "display:" + Eval("TitleVisible") %>'>
                            <div class="d-flex flex-wrap gap-3 align-items-center">
                                <span class="badge badge-info"><%# Eval("MediaTypes") %></span>
                                <small class="d-flex gap-2 align-items-center text-body-secondary">
                                    <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                    <time><%# Eval("MediaDate") %></time>
                                </small>
                            </div>
                            <div class="dropdown">
                                <button type="button" data-bs-toggle="dropdown" aria-expanded="false"
                                    class="btn btn-outline-secondary gap-2">
                                    <span class="d-inline-flex fs-5">
                                        <i class="hgi hgi-stroke hgi-share-08" aria-hidden="true"></i>
                                    </span>
                                    <span>
                                        <asp:Literal runat="server" Text="<%$ Resources: PNUres, SharePage %>" />
                                    </span>
                                </button>
                                <ul class="dropdown-menu dropdown-menu-end px-0">
                                    <li class="d-flex align-items-center py-2 px-3">
                                        <a target="_blank" rel="noopener noreferrer" aria-label="Whatsapp"
                                            class="btn btn-link icon-btn pnu-share-link" data-share="whatsapp" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-whatsapp" aria-hidden="true"></i></span>
                                        </a>
                                        <a target="_blank" rel="noopener noreferrer" aria-label="Linkedin"
                                            class="btn btn-link icon-btn pnu-share-link" data-share="linkedin" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-linkedin-02" aria-hidden="true"></i></span>
                                        </a>
                                        <a target="_blank" rel="noopener noreferrer" aria-label="Email"
                                            class="btn btn-link icon-btn pnu-share-link" data-share="email" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-mail-01" aria-hidden="true"></i></span>
                                        </a>
                                        <a target="_blank" rel="noopener noreferrer" aria-label="X"
                                            class="btn btn-link icon-btn pnu-share-link" data-share="x" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-new-twitter" aria-hidden="true"></i></span>
                                        </a>
                                        <a target="_blank" rel="noopener noreferrer" aria-label="Facebook"
                                            class="btn btn-link icon-btn pnu-share-link" data-share="facebook" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-facebook-02" aria-hidden="true"></i></span>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>

                        <!-- Content -->
                        <div class="mt-5">
                            <p><%# SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) %></p>
                        </div>

                        <!-- Video (YouTube / MP4) -->
                        <div id="divVideoYouTube" runat="server" class="mt-4" style='<%# "display:" + Eval("MP4Visible") %>'>
                            <div class="pnu-video-wrapper rounded-3" id="divVideo" style='<%# "display:" + Eval("VideoVisiable") %>'>
                                <iframe class="pnu-responsive-iframe" src='<%# Eval("VideoURL") %>'
                                    title="<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>"
                                    allowfullscreen loading="lazy"></iframe>
                            </div>
                        </div>

                    </article>
                </div>
            </section>
        </main>

    </ItemTemplate>
</asp:Repeater>

<!-- Related Items — DGA event cards -->
<section class="py-5 bg-primary-25" aria-label="related-items">
    <div class="container">
        <div class="row g-4">
            <asp:Repeater ID="rptNews" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6">
                        <div class="card h-100 pnu-event-card">
                            <div class="card-body d-flex flex-column gap-3">
                                <div class="mt-0 d-flex flex-column gap-3">
                                    <small class="d-flex gap-2 align-items-center">
                                        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        <time><%# Eval("MediaDate") %></time>
                                    </small>
                                    <h3 class="card-title line-clamp max-clamp-line-2">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                    </h3>
                                </div>
                                <div class="mt-auto d-flex flex-column gap-4">
                                    <div class="d-flex flex-wrap mt-auto gap-2">
                                        <span class="badge badge-info"><%# Eval("MediaTypes") %></span>
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <a class="btn btn-primary" href='<%# Eval("DetailsURL") %>'>
                                            <%# SPFactory.GetLocalizedTitle("قراءة المزيد", "Read More") %>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="d-flex justify-content-center mt-5">
            <a id="AllMCNews" runat="server" class="btn btn-secondary" href="">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />
            </a>
            <a id="AllMCNews1" runat="server" class="btn btn-secondary ms-3">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />
            </a>
        </div>
    </div>
</section>

<style>
    .pnu-video-wrapper {
        position: relative;
        padding-bottom: 56.25%;
        height: 0;
        overflow: hidden;
    }

    .pnu-responsive-iframe {
        position: absolute;
        top: 0;
        inset-inline-start: 0;
        width: 100%;
        height: 100%;
        border: 0;
    }

    .pnu-detail-article img.rounded-3 {
        max-height: 512px;
        object-fit: cover;
    }
</style>

<script>
    document.addEventListener("DOMContentLoaded", function () {
        // Hide legacy/new breadcrumb depending on section
        if (window.location.href.indexOf("/MediaCenter/") !== -1) {
            document.querySelectorAll('.breadcrumbhide').forEach(function (el) { el.style.display = "none"; });
        } else {
            document.querySelectorAll('.breadcrumbnew').forEach(function (el) { el.style.display = "none"; });
        }

        // Build share links from current page
        var url = encodeURIComponent(window.location.href);
        var titleEl = document.querySelector('.pnu-detail-article') ?
            document.querySelector('.bg-primary-25 h2') : null;
        var title = encodeURIComponent(titleEl ? titleEl.textContent.trim() : document.title);

        var shareMap = {
            whatsapp: 'https://api.whatsapp.com/send?text=' + title + '%20' + url,
            linkedin: 'https://www.linkedin.com/sharing/share-offsite/?url=' + url,
            email: 'mailto:?subject=' + title + '&body=' + title + '%0A%0A' + url,
            x: 'https://x.com/intent/tweet?url=' + url + '&text=' + title,
            facebook: 'https://www.facebook.com/sharer/sharer.php?u=' + url
        };

        document.querySelectorAll('.pnu-share-link').forEach(function (a) {
            var type = a.getAttribute('data-share');
            if (shareMap[type]) a.setAttribute('href', shareMap[type]);
        });
    });
</script>
