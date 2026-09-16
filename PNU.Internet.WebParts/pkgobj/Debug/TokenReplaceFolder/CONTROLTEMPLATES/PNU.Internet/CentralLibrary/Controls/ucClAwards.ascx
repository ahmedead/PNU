<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClAwards.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClAwards" %>

<section class="gray colored-section py-5" aria-labelledby="central-library-awards-title">
    <div class="container">
        <% if (HeadingVisible) { %>
            <div class="mb-4"><h2 class="mb-0" id="central-library-awards-title"><%= HeadingText %></h2></div>
        <% } %>
        <div class="row g-4">
            <asp:Repeater ID="rptAwards" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-lg-3 col-md-6">
                        <article class="card h-100 pnu-news-card">
                            <div class="card-body d-flex flex-column placeholder-glow h-100">
                                <img width="147" height="50" class="d-block align-self-start mb-4" src='<%# Eval("HasLogo").Equals(true) ? Eval("LogoUrl") : "/Style Library/DGA/public/images/pnu-logo-ar-h.svg" %>'
                                    alt="شعار جامعة الأميرة نورة بنت عبدالرحمن" loading="lazy" sizes="147px" decoding="async"
                                    style="width:auto;height:50px;aspect-ratio:auto;object-fit:contain">
                                <div class="flex-grow-1">
                                    <h3 class="card-title"><%# Eval("Title") %></h3>
                                    <p class="card-text line-clamp max-clamp-line-4"><%# Eval("Description") %></p>
                                </div>
                                <div class="mt-auto d-flex flex-column gap-4">
                                    <small class="d-flex gap-2 align-items-center">
                                        <i class="hgi hgi-stroke hgi-calendar-03" aria-hidden="true"></i>
                                        <span><%# Eval("SubTitle") %></span>
                                    </small>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
