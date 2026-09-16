<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgCategoryPrograms.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Controls.ucPgCategoryPrograms" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <section class="py-5" data-aos="fade-up" aria-labelledby="postgraduate-category-title">
        <div class="container">
            <div dir="<asp:Literal ID="litDir" runat="server" />" lang="<asp:Literal ID="litLang" runat="server" />" class="dga-content-wrapper d-flex flex-column gap-4 text-start">
                
                <!-- 1. Hero Overview Section -->
                <asp:Panel ID="pnlHero" runat="server" Visible="true">
                    <div class="card border-0 rounded-4 p-4 p-lg-5" style="border: 1px solid #d1e2db !important; background-color: #ffffff;">
                        <div class="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3">
                            <div class="d-flex align-items-center flex-wrap gap-2">
                                <span class="dga-tag px-3 py-1 rounded-pill small fw-medium" style="background-color: #e6f4ea; color: #006848; border: 1px solid #b7e1cd;">
                                    <i class="hgi hgi-stroke hgi-graduation-scroll me-1"></i>
                                    <asp:Literal ID="litHeroSubTag" runat="server" />
                                </span>
                                <span class="dga-tag px-3 py-1 rounded-pill small fw-medium" style="background-color: #f3f4f6; color: #374151; border: 1px solid #e5e7eb;">
                                    <asp:Literal ID="litHeroTag" runat="server" />
                                </span>
                            </div>
                            <div class="d-flex align-items-center gap-2 text-secondary small">
                                <i class="hgi hgi-stroke hgi-building-03 text-success"></i>
                                <span><asp:Literal ID="litUnivName" runat="server" /></span>
                            </div>
                        </div>
                        <h1 id="postgraduate-category-title" class="h2 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-award-02 flex-shrink-0" style="color: #c59b27;"></i>
                            <span><asp:Literal ID="litHeroTitle" runat="server" /></span>
                        </h1>
                        <p class="lead mb-0" style="color: #4b5563; font-size: 1.1rem; line-height: 1.8;">
                            <asp:Literal ID="litHeroDesc" runat="server" />
                        </p>
                    </div>
                </asp:Panel>

                <!-- 2. Electronic Admission Portal (Banner SIS) -->
                <asp:Panel ID="pnlBanner" runat="server" Visible="true">
                    <div class="card border rounded-3 p-4 bg-white shadow-sm" style="border-right: 4px solid #006848 !important; border-left: 1px solid #e5e7eb;">
                        <div class="d-flex flex-column flex-lg-row justify-content-between align-items-start align-items-lg-center gap-3">
                            <div class="d-flex align-items-start gap-3">
                                <div class="p-3 rounded-3 flex-shrink-0 d-flex align-items-center justify-content-center" style="background-color: #e6f4ea; color: #006848; width: 52px; height: 52px;">
                                    <i class="hgi hgi-stroke hgi-login-01 fs-3"></i>
                                </div>
                                <div>
                                    <h2 class="h5 fw-bold text-dark mb-1"><asp:Literal ID="litBannerTitle" runat="server" /></h2>
                                    <p class="text-muted small mb-0"><asp:Literal ID="litBannerDesc" runat="server" /></p>
                                </div>
                            </div>
                            <a id="btnBannerApply" runat="server" class="btn px-4 py-2 text-white fw-medium d-inline-flex align-items-center gap-2 flex-shrink-0 shadow-sm" target="_blank" rel="noopener noreferrer" style="background-color: #006848; border-color: #006848; border-radius: 8px; min-height: 44px;">
                                <span><asp:Literal ID="litBannerBtnText" runat="server" /></span>
                                <i class="hgi hgi-stroke hgi-arrow-up-right-01"></i>
                            </a>
                        </div>
                    </div>
                </asp:Panel>

                <!-- 3. Dynamic Program Tables by Degree Group -->
                <asp:Repeater ID="rptDegreeGroups" runat="server" OnItemDataBound="rptDegreeGroups_ItemDataBound">
                    <ItemTemplate>
                        <div class="mt-4">
                            <!-- Group Header -->
                            <div class="d-flex align-items-center gap-2 mb-3 pb-2 border-bottom">
                                <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-4' style="color: #006848;"></i>
                                <h2 class="h5 fw-bold text-dark mb-0"><%# Eval("GroupTitle") %></h2>
                                <span class="badge rounded-pill bg-light text-muted border ms-2">
                                    <%# ((PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgDegreeGroupViewModel)Container.DataItem).CountLabel(IsArabic) %>
                                </span>
                            </div>

                            <!-- Group Table -->
                            <div class="table-responsive rounded-3 border bg-white shadow-sm">
                                <table class="table table-hover align-middle mb-0 text-start">
                                    <thead style="background-color: #f8faf9; color: #1f2937; border-bottom: 2px solid #e5e7eb;">
                                        <tr>
                                            <th scope="col" class="py-3 px-4 fw-semibold"><%# IsArabic ? "اسم البرنامج" : "Program Name" %></th>
                                            <th scope="col" class="py-3 px-3 fw-semibold"><%# IsArabic ? "الكلية" : "College" %></th>
                                            <th scope="col" class="py-3 px-3 fw-semibold"><%# IsArabic ? "القسم" : "Department" %></th>
                                            <th scope="col" class="py-3 px-3 text-center fw-semibold"><%# IsArabic ? "محلي / عالمي" : "Scope" %></th>
                                            <th scope="col" class="py-3 px-4 text-center fw-semibold"><%# IsArabic ? "الإجراءات" : "Actions" %></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptPrograms" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="py-3 px-4">
                                                        <span class="fw-bold text-dark d-block">
                                                            <%# ((PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgProgramItemModel)Container.DataItem).GetTitle(IsArabic) %>
                                                        </span>
                                                    </td>
                                                    <td class="py-3 px-3 text-muted">
                                                        <%# ((PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgProgramItemModel)Container.DataItem).GetCollege(IsArabic) %>
                                                    </td>
                                                    <td class="py-3 px-3 text-muted">
                                                        <%# ((PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgProgramItemModel)Container.DataItem).GetDepartment(IsArabic) %>
                                                    </td>
                                                    <td class="py-3 px-3 text-center">
                                                        <span class='px-2 py-1 rounded small <%# ((PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgProgramItemModel)Container.DataItem).IsInternational ? "bg-info-subtle text-primary fw-medium" : "bg-light text-muted" %>' style='<%# ((PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgProgramItemModel)Container.DataItem).IsInternational ? "background-color:#e0f2fe!important; color:#0369a1!important;" : "background-color:#f3f4f6; color:#4b5563;" %>'>
                                                            <%# ((PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgProgramItemModel)Container.DataItem).GetScope(IsArabic) %>
                                                        </span>
                                                    </td>
                                                    <td class="py-3 px-4 text-center">
                                                        <div class="d-flex justify-content-center gap-2">
                                                            <%# !string.IsNullOrEmpty(Eval("DetailsUrl") as string) ? string.Format("<a href='{0}' class='btn btn-sm btn-outline-secondary d-inline-flex align-items-center gap-1' style='border-radius:6px;'><i class='hgi hgi-stroke hgi-information-circle'></i><span>{1}</span></a>", Eval("DetailsUrl"), IsArabic ? "التفاصيل" : "Details") : "" %>
                                                            <%# !string.IsNullOrEmpty(Eval("ApplyUrl") as string) ? string.Format("<a href='{0}' target='_blank' rel='noopener noreferrer' class='btn btn-sm text-white d-inline-flex align-items-center gap-1' style='background-color:#006848;border-radius:6px;'><i class='hgi hgi-stroke hgi-login-01'></i><span>{1}</span></a>", Eval("ApplyUrl"), IsArabic ? "قدّم الآن" : "Apply Now") : "" %>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <!-- 4. Bottom Quick Links / Resource Cards -->
                <asp:Panel ID="pnlQuickCards" runat="server" Visible="true">
                    <div class="row g-3 mt-2">
                        <div class="col-12 col-md-6">
                            <div class="card border rounded-3 p-3 text-start h-100 bg-white shadow-sm" style="transition: transform 0.2s, box-shadow 0.2s;">
                                <div class="d-flex justify-content-between align-items-center gap-3">
                                    <div class="d-flex align-items-center gap-3">
                                        <div class="p-2 rounded-3 flex-shrink-0 d-flex align-items-center justify-content-center" style="background-color: #f0fdf4; color: #006848; width: 44px; height: 44px;">
                                            <i class="hgi hgi-stroke hgi-clipboard fs-4"></i>
                                        </div>
                                        <div>
                                            <h3 class="h6 fw-bold text-dark mb-1"><asp:Literal ID="litCard1Title" runat="server" /></h3>
                                            <span class="text-muted small"><asp:Literal ID="litCard1Desc" runat="server" /></span>
                                        </div>
                                    </div>
                                    <a id="lnkCard1" runat="server" class="btn btn-light border flex-shrink-0 d-inline-flex align-items-center justify-content-center rounded-circle" style="width: 40px; height: 40px; color: #006848;">
                                        <i class='hgi hgi-stroke <asp:Literal ID="litArrowIcon1" runat="server" />'></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-md-6">
                            <div class="card border rounded-3 p-3 text-start h-100 bg-white shadow-sm" style="transition: transform 0.2s, box-shadow 0.2s;">
                                <div class="d-flex justify-content-between align-items-center gap-3">
                                    <div class="d-flex align-items-center gap-3">
                                        <div class="p-2 rounded-3 flex-shrink-0 d-flex align-items-center justify-content-center" style="background-color: #f0fdf4; color: #006848; width: 44px; height: 44px;">
                                            <i class="hgi hgi-stroke hgi-calendar-03 fs-4"></i>
                                        </div>
                                        <div>
                                            <h3 class="h6 fw-bold text-dark mb-1"><asp:Literal ID="litCard2Title" runat="server" /></h3>
                                            <span class="text-muted small"><asp:Literal ID="litCard2Desc" runat="server" /></span>
                                        </div>
                                    </div>
                                    <a id="lnkCard2" runat="server" class="btn btn-light border flex-shrink-0 d-inline-flex align-items-center justify-content-center rounded-circle" style="width: 40px; height: 40px; color: #006848;">
                                        <i class='hgi hgi-stroke <asp:Literal ID="litArrowIcon2" runat="server" />'></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

            </div>
        </div>
    </section>
</main>
