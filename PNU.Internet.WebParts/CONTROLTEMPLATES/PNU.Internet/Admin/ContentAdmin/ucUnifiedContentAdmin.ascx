<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUnifiedContentAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ContentAdmin.ucUnifiedContentAdmin" %>

<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

<style>
    .pnu-unified-admin {
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        direction: rtl;
    }
    .pnu-admin-sidebar-nav .nav-link {
        color: #2b303a;
        font-weight: 600;
        border-radius: 8px;
        margin-bottom: 6px;
        padding: 10px 14px;
        text-decoration: none;
        transition: all 0.2s ease-in-out;
        display: flex;
        align-items: center;
        justify-content: space-between;
        border: 1px solid transparent;
    }
    .pnu-admin-sidebar-nav .nav-link:hover {
        background-color: #f0f7f4;
        color: #007848;
        border-color: #d1e7dd;
    }
    .pnu-admin-sidebar-nav .nav-link.active {
        background: linear-gradient(135deg, #007848 0%, #004d2e 100%);
        color: #ffffff !important;
        box-shadow: 0 4px 12px rgba(0, 120, 72, 0.25);
    }
    .pnu-card-module {
        transition: transform 0.2s ease, box-shadow 0.2s ease;
        border: 1px solid #e2e8f0;
        border-radius: 12px;
    }
    .pnu-card-module:hover {
        transform: translateY(-4px);
        box-shadow: 0 10px 25px rgba(0,0,0,0.08) !important;
        border-color: #007848;
    }
    .pnu-icon-box {
        width: 48px;
        height: 48px;
        border-radius: 10px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.4rem;
        background-color: #f0f7f4;
        color: #007848;
    }
    .pnu-target-badge {
        font-size: 0.85rem;
        background-color: #f8f9fa;
        color: #495057;
        border: 1px solid #ced4da;
        border-radius: 6px;
        padding: 4px 10px;
    }
</style>

<div class="pnu-unified-admin container-fluid py-3">

    <%-- 1. Access Denied State --%>
    <asp:PlaceHolder ID="phDenied" runat="server" Visible="false">
        <div class="alert alert-danger shadow-sm border-0 rounded-4 p-4 text-center my-4">
            <i class="bi bi-shield-lock-fill text-danger fs-1 mb-2 d-block"></i>
            <h4 class="fw-bold mb-2">غير مصرح بالوصول</h4>
            <p class="mb-0 text-muted">
                يتطلب الوصول إلى لوحة الإدارة تسجيل حسابكم كمسؤول مفعل في القائمة (<asp:Literal ID="ltrAdminListName" runat="server" />) على الموقع (<asp:Literal ID="ltrAdminWebPath" runat="server" />).
            </p>
        </div>
    </asp:PlaceHolder>

    <%-- 2. Authorized Master Admin Workspace --%>
    <asp:PlaceHolder ID="phMain" runat="server" Visible="false">
        <asp:HiddenField ID="hfActiveModule" runat="server" />
        <asp:HiddenField ID="hfCurrentLayout" runat="server" />
        <asp:HiddenField ID="hfIsDetailView" runat="server" />

        <!-- Global Top Toolbar -->
        <div class="card shadow-sm border-0 rounded-3 mb-4">
            <div class="card-body p-3">
                <div class="row g-3 align-items-center">
                    <div class="col-12 col-md-5">
                        <label class="form-label fw-bold small text-muted mb-1" for="<%= ddlTargetWeb.ClientID %>">
                            <i class="bi bi-diagram-3 me-1"></i> الموقع المستهدف (Target Web):
                        </label>
                        <asp:DropDownList ID="ddlTargetWeb" runat="server" CssClass="form-select form-select-sm" 
                            AutoPostBack="true" OnSelectedIndexChanged="ddlTargetWeb_SelectedIndexChanged" />
                    </div>
                    <div class="col-12 col-md-4">
                        <label class="form-label fw-bold small text-muted mb-1" for="<%= ddlLayoutSelector.ClientID %>">
                            <i class="bi bi-layout-split me-1"></i> قالب العرض (Template):
                        </label>
                        <asp:DropDownList ID="ddlLayoutSelector" runat="server" CssClass="form-select form-select-sm" 
                            AutoPostBack="true" OnSelectedIndexChanged="ddlLayoutSelector_SelectedIndexChanged">
                            <asp:ListItem Text="قالب 1: مساحة العمل الجانبية (Sidebar Workspace)" Value="SidebarWorkspace" />
                            <asp:ListItem Text="قالب 2: شبكة البطاقات (Cards Dashboard)" Value="CardsDashboard" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 text-md-end">
                        <asp:LinkButton ID="btnProvisionActive" runat="server" CssClass="btn btn-sm btn-outline-success w-100" 
                            OnClick="btnProvisionActive_Click">
                            <i class="bi bi-database-check me-1"></i> تهيئة قوائم القسم
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <!-- Alert Notification Panel -->
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-info alert-dismissible fade show rounded-3 shadow-sm">
            <asp:Literal ID="ltrAlertMessage" runat="server" />
        </asp:Panel>

        <!-- ====================================================================== -->
        <!-- TEMPLATE 1: Sidebar Workspace Layout                                   -->
        <!-- ====================================================================== -->
        <asp:PlaceHolder ID="phTemplateSidebar" runat="server" Visible="false">
            <div class="row g-4">
                <!-- Sidebar Menu Column -->
                <div class="col-12 col-lg-3">
                    <div class="card shadow-sm border-0 rounded-3 p-3 bg-white">
                        <div class="d-flex align-items-center justify-content-between mb-3 border-bottom pb-2">
                            <span class="fw-bold text-secondary small"><i class="bi bi-grid-fill me-1"></i> الوحدات الإدارية</span>
                            <span class="badge bg-success rounded-pill"><asp:Literal ID="ltrModuleCount" runat="server" /></span>
                        </div>
                        <div class="nav flex-column pnu-admin-sidebar-nav">
                            <asp:Repeater ID="rptSidebarModules" runat="server" OnItemCommand="rptSidebarModules_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSelectModule" runat="server" 
                                        CommandName="SelectModule" CommandArgument='<%# Eval("Key") %>'
                                        CssClass='<%# GetSidebarNavClass(Eval("Key").ToString()) %>'>
                                        <span>
                                            <i class='<%# "bi " + Eval("Icon") + " me-2" %>'></i>
                                            <%# Eval("TitleAr") %>
                                        </span>
                                        <i class="bi bi-chevron-left small opacity-50"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

                <!-- Active Control Host Canvas Column -->
                <div class="col-12 col-lg-9">
                    <div class="card shadow-sm border-0 rounded-3 bg-white">
                        <div class="card-header bg-white border-bottom py-3 d-flex justify-content-between align-items-center">
                            <div>
                                <h5 class="mb-0 fw-bold text-success">
                                    <asp:Literal ID="ltrActiveTitle" runat="server" />
                                </h5>
                                <small class="text-muted"><asp:Literal ID="ltrActiveCategory" runat="server" /></small>
                            </div>
                            <asp:Label ID="lblTargetWebBadge" runat="server" CssClass="pnu-target-badge" />
                        </div>
                        <div class="card-body p-3 p-md-4">
                            <asp:PlaceHolder ID="phControlHost_Sidebar" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>

        <!-- ====================================================================== -->
        <!-- TEMPLATE 2: Cards Dashboard Grid Layout                                -->
        <!-- ====================================================================== -->
        <asp:PlaceHolder ID="phTemplateCards" runat="server" Visible="false">
            
            <%-- Dashboard Overview Grid --%>
            <asp:PlaceHolder ID="phCardsOverview" runat="server" Visible="true">
                <div class="row g-3 mb-4">
                    <asp:Repeater ID="rptCards" runat="server" OnItemCommand="rptCards_ItemCommand">
                        <ItemTemplate>
                            <div class="col-12 col-md-6 col-xl-4">
                                <div class="card h-100 pnu-card-module rounded-3 p-3 bg-white d-flex flex-column">
                                    <div class="d-flex align-items-center mb-3">
                                        <div class="pnu-icon-box me-3">
                                            <i class='<%# "bi " + Eval("Icon") %>'></i>
                                        </div>
                                        <div>
                                            <h6 class="fw-bold mb-0 text-dark"><%# Eval("TitleAr") %></h6>
                                            <small class="text-muted"><%# Eval("Category") %></small>
                                        </div>
                                    </div>
                                    <p class="text-muted small flex-grow-1 mb-3">
                                        <%# Eval("Description") %>
                                    </p>
                                    <div class="d-flex gap-2 pt-2 border-top">
                                        <asp:LinkButton ID="btnOpenCard" runat="server" CommandName="OpenModule" 
                                            CommandArgument='<%# Eval("Key") %>' CssClass="btn btn-sm btn-success flex-grow-1">
                                            <i class="bi bi-arrow-left-circle me-1"></i> فتح الإدارة
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCardProvision" runat="server" CommandName="ProvisionModule" 
                                            CommandArgument='<%# Eval("Key") %>' CssClass="btn btn-sm btn-outline-secondary" ToolTip="تهيئة القوائم">
                                            <i class="bi bi-database-check"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:PlaceHolder>

            <%-- Focused Detail View --%>
            <asp:PlaceHolder ID="phCardsDetailView" runat="server" Visible="false">
                <div class="mb-3">
                    <asp:LinkButton ID="btnBackToDashboard" runat="server" CssClass="btn btn-sm btn-outline-secondary" 
                        OnClick="btnBackToDashboard_Click">
                        <i class="bi bi-arrow-right me-1"></i> العودة لشبكة البطاقات
                    </asp:LinkButton>
                </div>
                <div class="card shadow-sm border-0 rounded-3 bg-white">
                    <div class="card-header bg-white border-bottom py-3 d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold text-success"><asp:Literal ID="ltrCardsDetailTitle" runat="server" /></h5>
                        <asp:Label ID="lblCardsDetailBadge" runat="server" CssClass="pnu-target-badge" />
                    </div>
                    <div class="card-body p-3 p-md-4">
                        <asp:PlaceHolder ID="phControlHost_Cards" runat="server" />
                    </div>
                </div>
            </asp:PlaceHolder>

        </asp:PlaceHolder>

    </asp:PlaceHolder>
</div>
