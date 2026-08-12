<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsDetails1.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucNewsDetails1" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Repeater ID="rptMainData" runat="server">
    <ItemTemplate>

        <!-- ===== DGA Page Header + Breadcrumb ===== -->
        <div class="bg-primary-25 py-5">
            <div class="container">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-2">
                        <li class="breadcrumb-item small">
                            <a href='<%# SPFactory.GetSiteURL() %>'>
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a>
                        </li>
                        <li class="breadcrumb-item small">
                            <a href='<%# String.Format("{0}/{1}/", SPFactory.GetSiteURL(), "MediaCenter") %>'>
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, MediaCenter %>" /></a>
                        </li>
                        <li class="breadcrumb-item small">
                            <a href='<%# String.Format("{0}{1}", SPFactory.GetSiteURL(), "/MediaCenter/Pages/AllNews.aspx") %>'>
                                <%# Eval("DisplayCatName") %></a>
                        </li>
                        <li class="breadcrumb-item small active" aria-current="page">
                            <span><%# SPFactory.GetLocalizedTitle("تفاصيل الخبر", "News Details") %></span>
                        </li>
                    </ol>
                </nav>
                <div class="content" id="divTitle" runat="server" style='<%# "display:" + Eval("TitleVisible") %>'>
                    <h2 class="mb-0"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h2>
                </div>
            </div>
        </div>

        <!-- ===== Article Body ===== -->
        <main id="main-content" class="dga-main-body" tabindex="-1">
            <section class="py-5" data-aos="fade-up" aria-labelledby="news-detail-title">
                <div class="container">
                    <article class="pnu-detail-article">

                        <!-- Image -->
                        <div class="text-center" id="divImage" runat="server" style='<%# "display:" + Eval("ImgVisible") %>'>
                            <img class="img-fluid w-100 rounded-3"
                                src='<%# Eval("AttachmentURL") %>'
                                alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                loading="eager" fetchpriority="high" decoding="async" />
                        </div>

                        <!-- Meta row: badge + date + share -->
                        <div class="d-flex flex-column flex-lg-row gap-3 justify-content-between align-items-lg-center mt-3">
                            <div class="d-flex flex-wrap gap-3 align-items-center">
                                <span class="badge badge-info"><%# Eval("DisplayCatName") %></span>
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
                                    <span><%# SPFactory.GetLocalizedTitle("مشاركة الصفحة", "Share Page") %></span>
                                </button>
                                <ul class="dropdown-menu dropdown-menu-end px-0">
                                    <li class="d-flex align-items-center py-2 px-3">
                                        <a target="_blank" rel="noopener noreferrer" aria-label="Whatsapp"
                                            class="btn btn-link icon-btn pnu-share" data-share="whatsapp" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-whatsapp" aria-hidden="true"></i></span>
                                        </a>
                                        <a target="_blank" rel="noopener noreferrer" aria-label="Linkedin"
                                            class="btn btn-link icon-btn pnu-share" data-share="linkedin" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-linkedin-02" aria-hidden="true"></i></span>
                                        </a>
                                        <a aria-label="Email" class="btn btn-link icon-btn pnu-share" data-share="email" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-mail-01" aria-hidden="true"></i></span>
                                        </a>
                                        <a target="_blank" rel="noopener noreferrer" aria-label="X"
                                            class="btn btn-link icon-btn pnu-share" data-share="x" href="#">
                                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-new-twitter" aria-hidden="true"></i></span>
                                        </a>
                                        <a target="_blank" rel="noopener noreferrer" aria-label="Facebook"
                                            class="btn btn-link icon-btn pnu-share" data-share="facebook" href="#">
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

                        <!-- Video -->
                        <div id="divVideoYouTube" runat="server" class="mt-4" style='<%# "display:" + Eval("MP4Visible") %>'>
                            <div class="ratio ratio-16x9 rounded-3 overflow-hidden" id="divVideo" runat="server"
                                style='<%# "display:" + Eval("VideoVisiable") %>'>
                                <iframe src='<%# Eval("VideoURL") %>'
                                    title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                    allowfullscreen loading="lazy"></iframe>
                            </div>
                        </div>

                    </article>
                </div>
            </section>
        </main>
    </ItemTemplate>
</asp:Repeater>

<!-- ===== Related News (DGA cards) ===== -->
<section class="py-5" aria-label="أخبار ذات صلة">
    <div class="container">
        <div class="row g-4">
            <asp:Repeater ID="rptNews" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100 pnu-news-card">
                            <div class="card-body d-flex flex-column h-100">
                                <img width="400" height="250" class="w-100 rounded-2 object-fit-cover"
                                    src='<%# Eval("AttachmentURL") %>'
                                    alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                    loading="lazy" fetchpriority="low" decoding="async" />
                                <div class="flex-grow-1">
                                    <h3 class="card-title line-clamp max-clamp-line-2">
                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                    </h3>
                                </div>
                                <div class="mt-auto d-flex flex-column gap-4">
                                    <small class="d-flex gap-2 align-items-center">
                                        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        <time><%# Eval("MediaDate") %></time>
                                    </small>
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
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="d-flex justify-content-center mt-5">
            <a id="AllMCNews" runat="server" class="btn btn-secondary" href="">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />
            </a>
        </div>
    </div>
</section>

<script>
    // Share links built from current page URL/title (move to portal.js later if preferred)
    (function () {
        var u = encodeURIComponent(window.location.href);
        var t = encodeURIComponent(document.title);
        var map = {
            whatsapp: 'https://api.whatsapp.com/send?text=' + t + '%20' + u,
            linkedin: 'https://www.linkedin.com/sharing/share-offsite/?url=' + u,
            email: 'mailto:?subject=' + t + '&body=' + t + '%0A%0A' + u,
            x: 'https://x.com/intent/tweet?url=' + u + '&text=' + t,
            facebook: 'https://www.facebook.com/sharer/sharer.php?u=' + u
        };
        document.querySelectorAll('.pnu-share').forEach(function (a) {
            a.href = map[a.getAttribute('data-share')] || '#';
        });
    })();
</script>