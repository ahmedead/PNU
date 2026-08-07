<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsDetails1.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.MediaCenter.ucNewsDetails1" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>




    <%-- ============================================== --%>
    <%-- Breadcrumb --%>
    <%-- ============================================== --%>
    
    <asp:Repeater ID="rptBreadcrumb" runat="server">
        <ItemTemplate>

    <div class="bg-primary-25 py-5">
        <div class="container">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-2">
                    

                    <li class="breadcrumb-item small"><a href="<%# String.Format("{0}", SPFactory.GetSiteURL()) %>"><asp:Literal runat="server" Text="<%$ Resources: PNUres, HomePage %>" /></a></li>
                    <li class="breadcrumb-item small"><a href="<%# String.Format("{0}/{1}/", SPFactory.GetSiteURL(), "MediaCenter") %>"><asp:Literal runat="server" Text="<%$ Resources: PNUres, MediaCenter %>" /></a></li>
                    <li class="breadcrumb-item small"><a href="<%# String.Format("{0}{1}{2}", SPFactory.GetSiteURL(), "/MediaCenter/Pages/AllNews.aspx?Id=", Eval("CatID")) %>"><%# Eval("DisplayCatName") %></a></li>
                    
               

                    </ol>
            </nav>
            <div class="content">
                <h2 class="mb-0">تفاصيل الخبر</h2>
                <div class="text mt-4"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></div>
            </div>
        </div>
    </div>
                   </ItemTemplate>
</asp:Repeater>
<main id="main-content" class="dga-main-body" tabindex="-1">

    <%-- ============================================== --%>
    <%-- Main news details --%>
    <%-- ============================================== --%>
    <asp:Repeater ID="rptMainData" runat="server">
        <ItemTemplate>
            <div class="container page-padding">
                <div class="row g-4">

                    <%-- Left column (8 cols on lg): badges, summary, image, content, video --%>
                    <div class="col-12 col-lg-8">

                        <%-- Badges: Main Category + Faculty/Source --%>
                        <div class="mt-3 d-flex flex-wrap gap-2">
                            <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(Eval("DisplayCatName") as string) %>'>
                                <span class="badge badge-info"><%# Eval("DisplayCatName") %></span>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) as string) %>'>
                                <span class="badge badge-success">
                                    <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                </span>
                            </asp:PlaceHolder>
                        </div>

                        <%-- Title --%>
                        <h1 class="display-6 fw-bold mb-0 mt-3">
                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                        </h1>

                        <p class="mb-0 mt-3"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></p>

                        <%-- Summary --%>
                        <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) as string) %>'>
                            <p class="mb-0 mt-3">
                                <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                            </p>
                        </asp:PlaceHolder>

                        <%-- Cover image --%>
                        <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(Eval("AttachmentURL") as string) %>'>
                            <div class="mt-4">
                                <figure class="mb-0">
                                    <img src='<%# Eval("AttachmentURL") %>'
                                         alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                         class="img-fluid rounded-3 w-100"
                                         loading="lazy" decoding="async" />
                                    <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(SPFactory.GetLocalizedTitle(Eval("ImageCaption"), Eval("ImageCaption_EN")) as string) %>'>
                                        <figcaption class="small text-body-secondary mt-2">
                                            <%# SPFactory.GetLocalizedTitle(Eval("ImageCaption"), Eval("ImageCaption_EN")) %>
                                        </figcaption>
                                    </asp:PlaceHolder>
                                </figure>
                            </div>
                        </asp:PlaceHolder>

                        <%-- Video (YouTube / MP4) --%>
                        <asp:PlaceHolder runat="server" Visible='<%# Eval("VideoVisiable").ToString() == "block" %>'>
                            <div class="mt-4">
                                <div class="ratio ratio-16x9 overflow-hidden rounded-3 bg-dark">
                                    <iframe src='<%# Eval("VideoURL") %>'
                                            title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                                            allowfullscreen
                                            referrerpolicy="strict-origin-when-cross-origin"></iframe>
                                </div>
                            </div>
                        </asp:PlaceHolder>

                        <%-- Full content body --%>
                        <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) as string) %>'>
                            <div class="mt-4 news-body">
                                <%# SPFactory.GetLocalizedTitle(Eval("MediaContent"), Eval("MediaContent_EN")) %>
                            </div>
                        </asp:PlaceHolder>

                    </div>

                    <%-- Right column (4 cols on lg): meta side card --%>
                    <div class="col-12 col-lg-4">
                        <div class="card e-service-side-nav">
                            <div class="card-body p-5">
                                <div class="d-flex flex-column gap-3">

                                    <%-- Publish date --%>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-5 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6">
                                                <%# SPFactory.GetLocalizedTitle("تاريخ النشر", "Publish date") %>
                                            </h4>
                                            <p class="mb-0">
                                                <time datetime='<%# Eval("MediaDateISO") %>'><%# Eval("MediaDate") %></time>
                                            </p>
                                        </div>
                                    </div>

                                    <%-- Source / Faculty --%>
                                    <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) as string) %>'>
                                        <div class="d-flex gap-2 align-items-start">
                                            <span class="flex-shrink-0 d-inline-flex fs-5 lh-1 text-primary">
                                                <i class="hgi hgi-stroke hgi-office" aria-hidden="true"></i>
                                            </span>
                                            <div>
                                                <h4 class="fw-bold mb-1 h6">
                                                    <%# SPFactory.GetLocalizedTitle("المصدر", "Source") %>
                                                </h4>
                                                <p class="mb-0">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("FacultyName"), Eval("FacultyName_EN")) %>
                                                </p>
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>

                                    <%-- Share page --%>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-5 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-share-08" aria-hidden="true"></i>
                                        </span>
                                        <div class="flex-grow-1">
                                            <h4 class="fw-bold mb-1 h6">
                                                <%# SPFactory.GetLocalizedTitle("مشاركة الصفحة", "Share this page") %>
                                            </h4>
                                            <div class="d-flex flex-wrap gap-2 mt-2 share-buttons"
                                                 data-share-title='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                                                <a href="#" class="btn btn-secondary icon-btn share-btn" data-share="twitter"
                                                   aria-label="Twitter" target="_blank" rel="noopener">
                                                    <i class="hgi hgi-stroke hgi-new-twitter" aria-hidden="true"></i>
                                                </a>
                                                <a href="#" class="btn btn-secondary icon-btn share-btn" data-share="linkedin"
                                                   aria-label="LinkedIn" target="_blank" rel="noopener">
                                                    <i class="hgi hgi-stroke hgi-linkedin-01" aria-hidden="true"></i>
                                                </a>
                                                <a href="#" class="btn btn-secondary icon-btn share-btn" data-share="whatsapp"
                                                   aria-label="WhatsApp" target="_blank" rel="noopener">
                                                    <i class="hgi hgi-stroke hgi-whatsapp" aria-hidden="true"></i>
                                                </a>
                                                <button type="button" class="btn btn-secondary icon-btn share-btn"
                                                        data-share="copy" aria-label='<%# SPFactory.GetLocalizedTitle("نسخ الرابط", "Copy link") %>'>
                                                    <i class="hgi hgi-stroke hgi-link-04" aria-hidden="true"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <%-- ============================================== --%>
    <%-- Related news --%>
    <%-- ============================================== --%>
    <asp:PlaceHolder ID="phRelatedSection" runat="server" Visible="false">
        <section class="gray colored-section py-5" data-aos="fade-up">
            <div class="container">
                <div class="mb-4">
                    <div class="d-flex justify-content-between align-items-start gap-2">
                        <h2 class="mb-0">
                            <%# SPFactory.GetLocalizedTitle("أخبار مرتبطة", "Related news") %>
                        </h2>
                        <a id="AllMCNews" runat="server" class="btn btn-outline-primary" href="">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />
                        </a>
                    </div>
                </div>
                <div class="row g-4">
                    <asp:Repeater ID="rptNews" runat="server">
                        <ItemTemplate>
                            <div class="col-12 col-lg-4 col-md-6">
                                <article class="card h-100 pnu-news-card">
                                    <div class="card-body d-flex flex-column placeholder-glow h-100">
                                        <a href='<%# Eval("DetailsURL") %>' class="text-decoration-none text-body">
                                            <img width="400" height="250" class="rounded-2 js-medium-zoom w-100"
                                                 alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'
                                                 loading="lazy"
                                                 src='<%# Eval("AttachmentURL") %>'
                                                 sizes="(min-width: 1200px) 360px, (min-width: 768px) 45vw, 90vw"
                                                 decoding="async" />
                                            <div class="flex-grow-1 mt-3">
                                                <h3 class="card-title">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                </h3>
                                                <p class="card-text line-clamp max-clamp-line-4">
                                                    <%# SPFactory.GetLocalizedTitle(Eval("Summary"), Eval("Summary_EN")) %>
                                                </p>
                                            </div>
                                            <div class="mt-auto d-flex flex-column gap-3">
                                                <small class="d-flex gap-2 align-items-center">
                                                    <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                                    <time datetime='<%# Eval("MediaDateISO") %>'><%# Eval("MediaDate") %></time>
                                                </small>
                                            </div>
                                        </a>
                                    </div>
                                </article>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </section>
    </asp:PlaceHolder>

</main>

<style>
    .news-body p { margin-bottom: 1rem; }
    .news-body img { max-width: 100%; height: auto; border-radius: 0.5rem; }
    .card.e-service-side-nav { top: unset !important; }
</style>

<script>
    (function () {
        document.addEventListener('DOMContentLoaded', function () {
            var url = window.location.href;
            var titleEl = document.querySelector('.share-buttons');
            var pageTitle = titleEl ? (titleEl.getAttribute('data-share-title') || document.title) : document.title;

            var encUrl = encodeURIComponent(url);
            var encTitle = encodeURIComponent(pageTitle);

            var twitter = document.querySelector('.share-btn[data-share="twitter"]');
            if (twitter) twitter.href = 'https://twitter.com/intent/tweet?url=' + encUrl + '&text=' + encTitle;

            var linkedin = document.querySelector('.share-btn[data-share="linkedin"]');
            if (linkedin) linkedin.href = 'https://www.linkedin.com/sharing/share-offsite/?url=' + encUrl;

            var whatsapp = document.querySelector('.share-btn[data-share="whatsapp"]');
            if (whatsapp) whatsapp.href = 'https://wa.me/?text=' + encTitle + '%20' + encUrl;

            var copyBtn = document.querySelector('.share-btn[data-share="copy"]');
            if (copyBtn) {
                copyBtn.addEventListener('click', function () {
                    var done = function () {
                        var original = copyBtn.innerHTML;
                        copyBtn.innerHTML = '<i class="hgi hgi-stroke hgi-checkmark-circle-02" aria-hidden="true"></i>';
                        setTimeout(function () { copyBtn.innerHTML = original; }, 1500);
                    };
                    if (navigator.clipboard && navigator.clipboard.writeText) {
                        navigator.clipboard.writeText(url).then(done);
                    } else {
                        var ta = document.createElement('textarea');
                        ta.value = url;
                        document.body.appendChild(ta);
                        ta.select();
                        try { document.execCommand('copy'); done(); } catch (e) { }
                        document.body.removeChild(ta);
                    }
                });
            }
        });
    })();
</script>

