<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsLetterFromList.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucNewsLetterFromList" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<%
    var currentLanguage = SPContext.Current.Web.Language;
    bool isArabic = (currentLanguage == 1025);
    string mainTitle = isArabic ? "النشرات الدورية" : "Newsletters";
%>

<div class="d-flex flex-column gap-4">
    <h2 class="mb-4 fw-semibold"><%= mainTitle %></h2>

    <!-- Newsletter Cards -->
    <div class="row g-4">
        <asp:Repeater ID="rptNewsletters" runat="server">
            <ItemTemplate>
                <div class="col-12 col-md-6">
                    <div class="card h-100 border rounded-3 overflow-hidden shadow-sm">
                        <div class="card-header bg-light py-3 border-bottom text-center">
                            <h3 class="card-title h6 fw-bold mb-0 text-dark">
                                <%#: Eval("DisplayTitle") %>
                            </h3>
                        </div>
                        <div class="card-body p-3 d-flex flex-column align-items-center gap-3">
                            <div class="w-100 rounded-2 overflow-hidden border" style="height: 380px;">
                                <iframe allowfullscreen class="fp-iframe w-100 h-100 border-0"
                                        src="<%#: Eval("FlipUrl") %>"
                                        title="<%#: Eval("DisplayTitle") %>"></iframe>
                            </div>
                            <button type="button"
                                    class="btn btn-primary d-inline-flex align-items-center gap-2 px-4 mt-auto"
                                    data-bs-toggle="modal"
                                    data-bs-target="#flipbookModal_<%#: Eval("ModalId") %>">
                                <i class="hgi hgi-stroke hgi-book-open-01 fs-5" aria-hidden="true"></i>
                                <span><%# (SPContext.Current.Web.Language == 1025 ? "عرض النشرة" : "Open Newsletter") %></span>
                            </button>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Modals -->
    <asp:Repeater ID="rptModals" runat="server">
        <ItemTemplate>
            <div class="modal fade"
                 id="flipbookModal_<%#: Eval("ModalId") %>"
                 tabindex="-1"
                 aria-hidden="true">
                <div class="modal-dialog modal-xl modal-dialog-centered">
                    <div class="modal-content rounded-4 border-0 shadow">
                        <div class="modal-header border-bottom">
                            <h5 class="modal-title fw-semibold"><%#: Eval("DisplayTitle") %></h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body p-2 text-center">
                            <div class="w-100 rounded-3 overflow-hidden" style="height: 80vh;">
                                <iframe allowfullscreen class="fp-iframe w-100 h-100 border-0"
                                        src="<%#: Eval("FlipUrl") %>"
                                        title="<%#: Eval("DisplayTitle") %>"></iframe>
                            </div>
                        </div>
                        <div class="modal-footer border-top">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal"><%# (SPContext.Current.Web.Language == 1025 ? "إغلاق" : "Close") %></button>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>