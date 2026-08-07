<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PNUCouncil.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.PNUCouncil" %>

 <main id="main-content" class="dga-main-body" tabindex="-1">
        <div class="container py-5">

<%-- ================== LEADERSHIP (Chair + Vice Chair) ================== --%>
<section class="mb-5" data-aos="fade-up" aria-labelledby="council-leadership-title">
    <div class="mb-4">
        <h2 id="council-leadership-title" class="mb-2">قيادة المجلس</h2>
        <p class="mb-0">الرئاسة ونائبة الرئيس كما وردت في صفحة مجلس الجامعة.</p>
    </div>
    <div class="row g-4">
        <asp:Repeater ID="rptLeadership" runat="server" OnItemDataBound="rptLeadership_ItemDataBound">
            <ItemTemplate>
                <div class="col-12 col-lg-6">
                    <article class="card h-100">
                        <div class="card-body">
                            <div class="icon-container">
                                <i runat="server" id="iconEl" aria-hidden="true"></i>
                            </div>
                            <div>
                                <h3 class="card-title mb-2">
                                    <asp:Literal ID="litName" runat="server" />
                                </h3>
                                <p class="card-text">
                                    <asp:Literal ID="litRole" runat="server" />
                                </p>
                            </div>
                        </div>
                    </article>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>


<%-- ================== MEMBERS GRID ================== --%>
<section class="mb-5" data-aos="fade-up" aria-labelledby="council-members-title">
    <div class="container px-0">
        <div class="d-flex flex-column flex-md-row justify-content-between gap-3 mb-4">
            <div>
                <h2 id="council-members-title" class="mb-2">الأعضاء</h2>
                <p class="mb-0">بطاقات مختصرة توضح الاسم والصفة داخل المجلس.</p>
            </div>
        </div>
        <div id="councilMembersGrid" class="row g-4">
            <asp:Repeater ID="rptMembers" runat="server" OnItemDataBound="rptMembers_ItemDataBound">
                <ItemTemplate>
                    <div class="col-12 col-md-6 col-xl-4">
                        <article class="card h-100">
                            <div class="card-body d-flex flex-row gap-3 align-items-center">
                                <span class="icon-container flex-shrink-0 fw-semibold">
                                    <asp:Literal ID="litNumber" runat="server" />
                                </span>
                                <div class="flex-grow-1">
                                    <h3 class="h6 card-title mb-2">
                                        <asp:Literal ID="litName" runat="server" />
                                    </h3>
                                    <p class="card-text small mb-0">
                                        <asp:Literal ID="litRole" runat="server" />
                                    </p>
                                    <p class="card-text small mb-0 text-muted">
                                        <asp:Literal ID="litType" runat="server" />
                                    </p>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>


<%-- ================== DOCUMENTS ================== --%>
<section data-aos="fade-up" aria-labelledby="council-documents-title">
    <div class="mb-4">
        <h2 id="council-documents-title" class="mb-2">مستندات المجلس</h2>
        <p class="mb-0">روابط مرجعية منشورة في الصفحة الرسمية لمجلس الجامعة.</p>
    </div>
    <div class="row g-4">
        <asp:Repeater ID="rptDocuments" runat="server" OnItemDataBound="rptDocuments_ItemDataBound">
            <ItemTemplate>
                <div class="col-12 col-md-6 col-xl-4">
                    <article class="card nav-card h-100">
                        <div class="card-body">
                            <div class="icon-container">
                                <i runat="server" id="iconEl" aria-hidden="true"></i>
                            </div>
                            <div>
                                <h3 class="card-title">
                                    <asp:Literal ID="litTitle" runat="server" />
                                </h3>
                            </div>
                            <div class="d-flex justify-content-end mt-auto">
                                <a runat="server" id="lnkDoc"
                                   class="btn btn-secondary stretched-link"
                                   target="_blank" rel="noopener noreferrer">
                                    <asp:Literal ID="litButton" runat="server" />
                                </a>
                            </div>
                        </div>
                    </article>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>

 </div>
    </main>