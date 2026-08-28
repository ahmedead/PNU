<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResearchPaths.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucResearchPaths" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<div class="d-flex flex-column gap-4">
    <!-- Header Level 1 -->
    <div id="div1" runat="server">
        <h2 class="mb-4 fw-semibold">
            <asp:Literal runat="server" Text="<%$ Resources: PNUres, ResearchPaths %>" />
        </h2>
    </div>

    <!-- Header Level 2 -->
    <div id="div2" runat="server">
        <h2 class="mb-4 fw-semibold">
            <asp:Literal runat="server" ID="ltr2" Text="<%$ Resources: PNUres, ResearchPaths %>" />
        </h2>
    </div>

    <!-- Header Level 3 -->
    <div id="div3" runat="server">
        <h2 class="mb-4 fw-semibold">
            <asp:Literal runat="server" ID="ltr3" Text="<%$ Resources: PNUres, ResearchPaths %>" />
        </h2>
    </div>

    <!-- Cards Grid -->
    <div class="row g-4">
        <asp:Repeater ID="rptCourses" runat="server">
            <ItemTemplate>
                <div class="col-12 col-md-6 col-xl-4">
                    <article class="card h-100 border rounded-3 overflow-hidden shadow-sm dga-hover-card position-relative">
                        <div class="position-relative overflow-hidden" style="height: 200px;">
                            <img src='<%# DataBinder.Eval(Container.DataItem,"ImageUrl") %>' class="w-100 h-100 object-fit-cover" alt='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title")) %>' loading="lazy" decoding="async">
                        </div>
                        <div class="card-body d-flex flex-column gap-3 p-4">
                            <h3 class="card-title h6 fw-bold mb-0">
                                <a href='<%# DataBinder.Eval(Container.DataItem,"LinkUrl") %>' class="stretched-link text-decoration-none text-body">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title")) %>
                                </a>
                            </h3>
                            <p class="card-text text-muted small line-clamp max-clamp-line-4 mb-0" id='<%# "paragraph_" + Container.ItemIndex %>'>
                                <%# SPFactory.GetLocalizedTitle(Eval("DescriptionDisplay"), Eval("DescriptionDisplay")) %>
                            </p>
                            <div class="mt-auto pt-2 d-flex justify-content-between align-items-center border-top text-primary">
                                <span class="small fw-medium"><%# (SPContext.Current.Web.Language == 1025 ? "استكشف المسار" : "Explore Track") %></span>
                                <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-left-02" aria-hidden="true"></i></span>
                            </div>
                        </div>
                    </article>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
