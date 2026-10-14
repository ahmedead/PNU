<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeFooterAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.Admin.ucHomeFooterAdmin" %>

<%-- Bootstrap Icons for the bi-* header/alert icons (same CDN as ucUnifiedContentAdmin); action buttons use inline SVG --%>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

<section class="py-4 footer-admin-control">
    <div class="container-fluid">

        <!-- Page Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 pb-3 border-bottom">
            <div>
                <h3 class="fw-bold mb-1 text-primary">
                    <i class="bi bi-layout-text-window-reverse me-2"></i>
                    <asp:Literal ID="ltAdminHeader" runat="server" Text="إدارة الفوتر وقوائم الروابط (الصفحة الرئيسية)" />
                </h3>
                <p class="text-muted small mb-0">
                    إدارة وتعديل جميع أقسام الفوتر والروابط ومعلومات التواصل ومنصات التواصل الاجتماعي والشعارات السفلية باللغتين العربية والإنجليزية.
                </p>
            </div>
            <asp:PlaceHolder ID="phHeaderActions" runat="server" Visible="false">
            <div class="d-flex gap-2">
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-outline-secondary btn-sm rounded-3 px-3" OnClick="btnRefresh_Click">
                    <i class="bi bi-arrow-clockwise me-1"></i> تحديث
                </asp:LinkButton>
                <asp:LinkButton ID="btnProvision" runat="server" CssClass="btn btn-outline-warning btn-sm rounded-3 px-3" OnClick="btnProvision_Click"
                                OnClientClick="return confirm('سيتم التحقق من وجود كافة قوائم الفوتر وحقولها وتهيئتها إن كانت ناقصة. هل تريد المتابعة؟');">
                    <i class="bi bi-gear-wide-connected me-1"></i> تهيئة القوائم
                </asp:LinkButton>
            </div>
            </asp:PlaceHolder>
        </div>

        <!-- Access Denied Panel -->
        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-warning shadow-sm border-0 rounded-4 p-4 text-center my-4">
                <i class="bi bi-shield-lock-fill text-warning fs-1 d-block mb-2"></i>
                <h4 class="fw-bold mb-2">تنبيه الصلاحيات</h4>
                <asp:Literal ID="ltDenied" runat="server" Text="ليس لديك صلاحية لإدارة محتوى هذا القسم." />
            </div>
        </asp:Panel>

        <!-- Main Admin Panel -->
        <asp:Panel ID="pnlMain" runat="server">

            <!-- Alerts -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div class="alert alert-success alert-dismissible fade show shadow-sm border-0 rounded-3 mb-4" role="alert">
                    <i class="bi bi-check-circle-fill me-2"></i>
                    <asp:Literal ID="ltMessage" runat="server" />
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger alert-dismissible fade show shadow-sm border-0 rounded-3 mb-4" role="alert">
                    <i class="bi bi-exclamation-triangle-fill me-2"></i>
                    <asp:Literal ID="ltError" runat="server" />
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <!-- Navigation Tabs -->
            <ul class="nav nav-pills mb-4 gap-2 bg-light p-2 rounded-4 border" id="footerAdminTabs" role="tablist">
                <li class="nav-item" role="presentation">
                    <asp:LinkButton ID="tabBtnNavLinks" runat="server" CssClass="nav-link rounded-3 fw-semibold active" OnClick="tabBtnNavLinks_Click">
                        <i class="bi bi-link-45deg me-1"></i> روابط وأقسام الفوتر
                    </asp:LinkButton>
                </li>
                <li class="nav-item" role="presentation">
                    <asp:LinkButton ID="tabBtnContact" runat="server" CssClass="nav-link rounded-3 fw-semibold" OnClick="tabBtnContact_Click">
                        <i class="bi bi-telephone me-1"></i> معلومات التواصل
                    </asp:LinkButton>
                </li>
                <li class="nav-item" role="presentation">
                    <asp:LinkButton ID="tabBtnSocial" runat="server" CssClass="nav-link rounded-3 fw-semibold" OnClick="tabBtnSocial_Click">
                        <i class="bi bi-share me-1"></i> قنوات التواصل الاجتماعي
                    </asp:LinkButton>
                </li>
                <li class="nav-item" role="presentation">
                    <asp:LinkButton ID="tabBtnBottom" runat="server" CssClass="nav-link rounded-3 fw-semibold" OnClick="tabBtnBottom_Click">
                        <i class="bi bi-card-checklist me-1"></i> الروابط والشعارات السفلية
                    </asp:LinkButton>
                </li>
            </ul>

            <!-- ======================================================== -->
            <!-- TAB 1: Navigation Sections & Links                       -->
            <!-- ======================================================== -->
            <asp:Panel ID="pnlTabNavLinks" runat="server" Visible="true">

                <!-- 1.1 Section Titles Card -->
                <div class="card mb-4 shadow-sm border-0 rounded-4">
                    <div class="card-header bg-white py-3 border-bottom d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold text-dark">
                            <i class="bi bi-layout-three-columns text-primary me-2"></i> عناوين أقسام الفوتر (Footer Sections)
                        </h5>
                        <span class="badge bg-secondary-subtle text-secondary small">القائمة: FooterNavSections</span>
                    </div>
                    <div class="card-body p-4">
                        <div class="row g-3">
                            <div class="col-12 col-md-6 col-lg-3">
                                <label class="form-label fw-semibold small">قسم 1 (نظرة عامة - AR):</label>
                                <asp:TextBox ID="txtSecOverviewAr" runat="server" CssClass="form-control" />
                                <label class="form-label fw-semibold small mt-2">Section 1 (Overview - EN):</label>
                                <asp:TextBox ID="txtSecOverviewEn" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6 col-lg-3">
                                <label class="form-label fw-semibold small">قسم 2 (فرصك في نورة - AR):</label>
                                <asp:TextBox ID="txtSecOppAr" runat="server" CssClass="form-control" />
                                <label class="form-label fw-semibold small mt-2">Section 2 (Opportunities - EN):</label>
                                <asp:TextBox ID="txtSecOppEn" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6 col-lg-3">
                                <label class="form-label fw-semibold small">قسم 3 (الحياة الجامعية - AR):</label>
                                <asp:TextBox ID="txtSecLifeAr" runat="server" CssClass="form-control" />
                                <label class="form-label fw-semibold small mt-2">Section 3 (University life - EN):</label>
                                <asp:TextBox ID="txtSecLifeEn" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6 col-lg-3">
                                <label class="form-label fw-semibold small">قسم 4 (مواقع ذات صلة - AR):</label>
                                <asp:TextBox ID="txtSecRelAr" runat="server" CssClass="form-control" />
                                <label class="form-label fw-semibold small mt-2">Section 4 (Related Links - EN):</label>
                                <asp:TextBox ID="txtSecRelEn" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 text-end pt-2">
                                <asp:Button ID="btnSaveSections" runat="server" Text="حفظ عناوين الأقسام" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveSections_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- 1.2 Add / Edit NavLink Panel -->
                <asp:Panel ID="pnlNavLinkForm" runat="server" Visible="false" CssClass="card mb-4 shadow-sm border-primary rounded-4">
                    <div class="card-header bg-primary text-white py-3 rounded-top-4 d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold">
                            <i class="bi bi-pencil-square me-2"></i>
                            <asp:Literal ID="ltNavLinkFormTitle" runat="server" Text="إضافة رابط جديد" />
                        </h5>
                        <asp:LinkButton ID="btnCloseNavLinkForm" runat="server" CssClass="btn-close btn-close-white" OnClick="btnCancelNavLink_Click" aria-label="Close"></asp:LinkButton>
                    </div>
                    <div class="card-body p-4">
                        <asp:HiddenField ID="hfNavLinkId" runat="server" Value="0" />
                        <div class="row g-3">
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">القسم التابع له: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlNavSection" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="Overview" Text="نظرة عامة / Overview" />
                                    <asp:ListItem Value="Opportunities" Text="فرصك في نورة / Your opportunities in Noura" />
                                    <asp:ListItem Value="UniversityLife" Text="الحياة الجامعية / University life" />
                                    <asp:ListItem Value="RelatedLinks" Text="مواقع ذات صلة / Related Links" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">عنوان الرابط (العربية - AR): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNavLinkTitleAr" runat="server" CssClass="form-control" Placeholder="مثال: الأسئلة الشائعة" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">عنوان الرابط (بالإنجليزية - EN): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNavLinkTitleEn" runat="server" CssClass="form-control" Placeholder="e.g. FAQs" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الرابط العربي (URL - AR): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNavLinkUrlAr" runat="server" CssClass="form-control" Placeholder="مثال: /ar/FAQs/Pages/default.aspx أو https://..." />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الرابط الإنجليزي (URL - EN): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNavLinkUrlEn" runat="server" CssClass="form-control" Placeholder="e.g. /en/FAQs/Pages/default.aspx or https://..." />
                            </div>

                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">ترتيب العرض:</label>
                                <asp:TextBox ID="txtNavLinkOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                            </div>

                            <div class="col-12 col-md-4 d-flex align-items-end">
                                <div class="form-check form-switch mb-2">
                                    <asp:CheckBox ID="chkNavLinkNewTab" runat="server" />
                                    <label class="form-check-label fw-semibold ms-2" for="<%= chkNavLinkNewTab.ClientID %>">فتح في نافذة جديدة (Target: _blank)</label>
                                </div>
                            </div>

                            <div class="col-12 col-md-4 d-flex align-items-end">
                                <div class="form-check form-switch mb-2">
                                    <asp:CheckBox ID="chkNavLinkVisible" runat="server" Checked="true" />
                                    <label class="form-check-label fw-semibold ms-2" for="<%= chkNavLinkVisible.ClientID %>">مفعل / معروض</label>
                                </div>
                            </div>

                            <div class="col-12 d-flex justify-content-end gap-2 pt-3 border-top">
                                <asp:Button ID="btnCancelNavLink" runat="server" Text="إلغاء" CssClass="btn btn-outline-secondary px-4 rounded-3" OnClick="btnCancelNavLink_Click" CausesValidation="false" />
                                <asp:Button ID="btnSaveNavLink" runat="server" Text="حفظ الرابط" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveNavLink_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- 1.3 Navigation Links Grid -->
                <div class="card shadow-sm border-0 rounded-4">
                    <div class="card-header bg-white py-3 border-bottom d-flex flex-wrap justify-content-between align-items-center gap-2">
                        <div class="d-flex align-items-center gap-2">
                            <h5 class="mb-0 fw-bold text-dark">
                                <i class="bi bi-list-stars text-primary me-2"></i> قائمة الروابط في الفوتر (Links List)
                            </h5>
                            <asp:Label ID="lblNavLinkCount" runat="server" CssClass="badge bg-primary-subtle text-primary rounded-pill px-3 py-2 fw-semibold" />
                        </div>
                        <div class="d-flex gap-2">
                            <asp:DropDownList ID="ddlFilterSection" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterSection_SelectedIndexChanged">
                                <asp:ListItem Value="" Text="-- تصفية بحسب القسم (الكل) --" />
                                <asp:ListItem Value="Overview" Text="نظرة عامة / Overview" />
                                <asp:ListItem Value="Opportunities" Text="فرصك في نورة / Your opportunities in Noura" />
                                <asp:ListItem Value="UniversityLife" Text="الحياة الجامعية / University life" />
                                <asp:ListItem Value="RelatedLinks" Text="مواقع ذات صلة / Related Links" />
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnNewNavLink" runat="server" CssClass="btn btn-primary btn-sm rounded-3 px-3 py-2 fw-semibold" OnClick="btnNewNavLink_Click">
                                <i class="bi bi-plus-lg me-1"></i> إضافة رابط جديد
                            </asp:LinkButton>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvNavLinks" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                      CssClass="table table-hover align-middle mb-0" GridLines="None"
                                      OnRowCommand="gvNavLinks_RowCommand" OnRowDataBound="gvNavLinks_RowDataBound">
                            <HeaderStyle CssClass="table-light text-secondary small text-nowrap" />
                            <Columns>
                                <asp:BoundField DataField="ItemOrder" HeaderText="الترتيب" ItemStyle-Width="60px" ItemStyle-CssClass="text-center fw-bold text-muted" HeaderStyle-CssClass="text-center" />
                                <asp:TemplateField HeaderText="القسم" ItemStyle-Width="160px">
                                    <ItemTemplate>
                                        <span class="badge bg-light text-dark border"><%# Text(Eval("SectionKey")) %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Title" HeaderText="العنوان بالعربية" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="TitleEn" HeaderText="العنوان بالإنجليزية" />
                                <asp:TemplateField HeaderText="الرابط (AR / EN)">
                                    <ItemTemplate>
                                        <div class="small">
                                            <div><span class="badge bg-secondary-subtle text-secondary me-1">AR</span> <a href='<%# Href(Eval("UrlAr")) %>' target="_blank" rel="noopener" class="text-truncate d-inline-block" style="max-width:260px"><%# Text(Eval("UrlAr")) %></a></div>
                                            <div><span class="badge bg-secondary-subtle text-secondary me-1">EN</span> <a href='<%# Href(Eval("UrlEn")) %>' target="_blank" rel="noopener" class="text-truncate d-inline-block" style="max-width:260px"><%# Text(Eval("UrlEn")) %></a></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="نافذة جديدة" ItemStyle-Width="90px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewTab" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="الحالة" ItemStyle-Width="90px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVisible" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="إجراءات" ItemStyle-Width="130px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-sm">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-outline-primary" CommandName="EditItem" CommandArgument='<%# Eval("ID") %>' ToolTip="تعديل">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M17 3a2.85 2.85 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/></svg><span class="visually-hidden">تعديل</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnToggle" runat="server" CssClass="btn btn-outline-secondary" CommandName="ToggleVisibility" CommandArgument='<%# Eval("ID") %>' ToolTip="تغيير الحالة">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg><span class="visually-hidden">تغيير الحالة</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-outline-danger" CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>' ToolTip="حذف"
                                                            OnClientClick="return confirm('هل أنت متأكد من حذف هذا الرابط؟');">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/><path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><path d="M10 11v6"/><path d="M14 11v6"/></svg><span class="visually-hidden">حذف</span>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-5 text-muted">
                                    <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                                    لا توجد روابط مسجلة حالياً في هذا القسم.
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>

            </asp:Panel>

            <!-- ======================================================== -->
            <!-- TAB 2: Contact Info                                      -->
            <!-- ======================================================== -->
            <asp:Panel ID="pnlTabContact" runat="server" Visible="false">
                <div class="card mb-4 shadow-sm border-0 rounded-4">
                    <div class="card-header bg-white py-3 border-bottom d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold text-dark">
                            <i class="bi bi-telephone text-primary me-2"></i> معلومات وتفاصيل التواصل (Contact Us)
                        </h5>
                        <span class="badge bg-secondary-subtle text-secondary small">القائمة: ContactUsFooter</span>
                    </div>
                    <div class="card-body p-4">
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الرقم الموحد (Phone / Unified): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtContactPhone" runat="server" CssClass="form-control" Placeholder="920011114" />
                                <div class="form-text small">يظهر كرقم رئيسي قابل للاتصال في الفوتر.</div>
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الرقم الدولي / الإضافي (Phone International):</label>
                                <asp:TextBox ID="txtContactPhoneIntl" runat="server" CssClass="form-control" Placeholder="00966-11-8220011" />
                                <div class="form-text small">رقم التواصل المباشر من خارج المملكة.</div>
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">البريد الإلكتروني الرسمي (Official Email): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control" Placeholder="info@pnu.edu.sa" />
                            </div>
                            <div class="col-12 text-end pt-3 border-top">
                                <asp:Button ID="btnSaveContact" runat="server" Text="حفظ معلومات التواصل" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveContact_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- ======================================================== -->
            <!-- TAB 3: Social Media Links                                -->
            <!-- ======================================================== -->
            <asp:Panel ID="pnlTabSocial" runat="server" Visible="false">

                <!-- 3.1 Add / Edit Social Account Panel -->
                <asp:Panel ID="pnlSocialForm" runat="server" Visible="false" CssClass="card mb-4 shadow-sm border-primary rounded-4">
                    <div class="card-header bg-primary text-white py-3 rounded-top-4 d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold">
                            <i class="bi bi-pencil-square me-2"></i>
                            <asp:Literal ID="ltSocialFormTitle" runat="server" Text="إضافة منصة تواصل اجتماعي" />
                        </h5>
                        <asp:LinkButton ID="btnCloseSocialForm" runat="server" CssClass="btn-close btn-close-white" OnClick="btnCancelSocial_Click" aria-label="Close"></asp:LinkButton>
                    </div>
                    <div class="card-body p-4">
                        <asp:HiddenField ID="hfSocialId" runat="server" Value="0" />
                        <div class="row g-3">
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">اسم المنصة / المفتاح (Title / Key): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtSocialTitle" runat="server" CssClass="form-control" Placeholder="مثال: Facebook أو Twitter_Live" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">فئة الأيقونة (Icon Class):</label>
                                <asp:TextBox ID="txtSocialIconClass" runat="server" CssClass="form-control" Placeholder="hgi hgi-stroke hgi-new-twitter" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">الحساب التابع له (Parent Key للمجموعات):</label>
                                <asp:TextBox ID="txtSocialParentKey" runat="server" CssClass="form-control" Placeholder="فارغ للأيقونات الرئيسية، أو Twitter للحسابات الفرعية" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">رابط المنصة (URL):</label>
                                <asp:TextBox ID="txtSocialUrl" runat="server" CssClass="form-control" Placeholder="https://..." />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الوصف الصوتي (Aria Label - AR):</label>
                                <asp:TextBox ID="txtSocialAriaAr" runat="server" CssClass="form-control" Placeholder="مثال: حساب الجامعة في منصة اكس" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الوصف الصوتي (Aria Label - EN):</label>
                                <asp:TextBox ID="txtSocialAriaEn" runat="server" CssClass="form-control" Placeholder="e.g. PNU on X" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-semibold">ترتيب العرض:</label>
                                <asp:TextBox ID="txtSocialOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                            </div>
                            <div class="col-12 col-md-3 d-flex align-items-end">
                                <div class="form-check form-switch mb-2">
                                    <asp:CheckBox ID="chkSocialVisible" runat="server" Checked="true" />
                                    <label class="form-check-label fw-semibold ms-2" for="<%= chkSocialVisible.ClientID %>">مفعل / معروض</label>
                                </div>
                            </div>

                            <div class="col-12 d-flex justify-content-end gap-2 pt-3 border-top">
                                <asp:Button ID="btnCancelSocial" runat="server" Text="إلغاء" CssClass="btn btn-outline-secondary px-4 rounded-3" OnClick="btnCancelSocial_Click" CausesValidation="false" />
                                <asp:Button ID="btnSaveSocial" runat="server" Text="حفظ المنصة" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveSocial_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- 3.2 Social Media Grid -->
                <div class="card shadow-sm border-0 rounded-4">
                    <div class="card-header bg-white py-3 border-bottom d-flex flex-wrap justify-content-between align-items-center gap-2">
                        <div class="d-flex align-items-center gap-2">
                            <h5 class="mb-0 fw-bold text-dark">
                                <i class="bi bi-share text-primary me-2"></i> قنوات وحسابات التواصل الاجتماعي
                            </h5>
                            <asp:Label ID="lblSocialCount" runat="server" CssClass="badge bg-primary-subtle text-primary rounded-pill px-3 py-2 fw-semibold" />
                        </div>
                        <asp:LinkButton ID="btnNewSocial" runat="server" CssClass="btn btn-primary btn-sm rounded-3 px-3 py-2 fw-semibold" OnClick="btnNewSocial_Click">
                            <i class="bi bi-plus-lg me-1"></i> إضافة حساب / منصة
                        </asp:LinkButton>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvSocial" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                      CssClass="table table-hover align-middle mb-0" GridLines="None"
                                      OnRowCommand="gvSocial_RowCommand" OnRowDataBound="gvSocial_RowDataBound">
                            <HeaderStyle CssClass="table-light text-secondary small text-nowrap" />
                            <Columns>
                                <asp:BoundField DataField="ItemOrder" HeaderText="الترتيب" ItemStyle-Width="60px" ItemStyle-CssClass="text-center fw-bold text-muted" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Title" HeaderText="اسم الحساب" ItemStyle-CssClass="fw-bold" />
                                <asp:TemplateField HeaderText="النوع">
                                    <ItemTemplate>
                                        <%# string.IsNullOrEmpty(Eval("ParentKey") as string) ? "<span class='badge bg-primary-subtle text-primary'>رئيسي</span>" : "<span class='badge bg-light text-muted border'>تابع لـ " + Text(Eval("ParentKey")) + "</span>" %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IconClass" HeaderText="فئة الأيقونة" />
                                <asp:TemplateField HeaderText="الرابط">
                                    <ItemTemplate>
                                        <a href='<%# Href(Eval("Url")) %>' target="_blank" rel="noopener" class="small text-truncate d-inline-block" style="max-width:240px"><%# Text(Eval("Url")) %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AriaLabelAr" HeaderText="الوصف (AR)" />
                                <asp:TemplateField HeaderText="الحالة" ItemStyle-Width="90px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSocialVisible" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="إجراءات" ItemStyle-Width="130px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-sm">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-outline-primary" CommandName="EditItem" CommandArgument='<%# Eval("ID") %>' ToolTip="تعديل">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M17 3a2.85 2.85 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/></svg><span class="visually-hidden">تعديل</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnToggle" runat="server" CssClass="btn btn-outline-secondary" CommandName="ToggleVisibility" CommandArgument='<%# Eval("ID") %>' ToolTip="تغيير الحالة">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg><span class="visually-hidden">تغيير الحالة</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-outline-danger" CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>' ToolTip="حذف"
                                                            OnClientClick="return confirm('هل أنت متأكد من حذف هذه المنصة؟');">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/><path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><path d="M10 11v6"/><path d="M14 11v6"/></svg><span class="visually-hidden">حذف</span>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-5 text-muted">
                                    <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                                    لا توجد منصات مسجلة حالياً.
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>

            <!-- ======================================================== -->
            <!-- TAB 4: Bottom Bar Links & Logos                          -->
            <!-- ======================================================== -->
            <asp:Panel ID="pnlTabBottom" runat="server" Visible="false">

                <!-- 4.1 General Settings & Logos Card -->
                <div class="card mb-4 shadow-sm border-0 rounded-4">
                    <div class="card-header bg-white py-3 border-bottom d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold text-dark">
                            <i class="bi bi-gear-wide text-primary me-2"></i> إعدادات الشعارات وحقوق النشر (Settings & Logos)
                        </h5>
                        <span class="badge bg-secondary-subtle text-secondary small">القائمة: FooterSettings</span>
                    </div>
                    <div class="card-body p-4">
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">رابط سدايا (SDAIA URL - AR):</label>
                                <asp:TextBox ID="txtSdaiaUrlAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">SDAIA URL (EN):</label>
                                <asp:TextBox ID="txtSdaiaUrlEn" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">رابط رؤية 2030 (Vision 2030 URL - AR):</label>
                                <asp:TextBox ID="txtVisionUrlAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">Vision 2030 URL (EN):</label>
                                <asp:TextBox ID="txtVisionUrlEn" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">رابط شعار الجامعة (PNU Logo URL - AR):</label>
                                <asp:TextBox ID="txtPnuUrlAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">PNU Logo URL (EN):</label>
                                <asp:TextBox ID="txtPnuUrlEn" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">نص حقوق النشر (Copyright - AR):</label>
                                <asp:TextBox ID="txtCopyrightAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">Copyright Text (EN):</label>
                                <asp:TextBox ID="txtCopyrightEn" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">نص التطوير والصيانة (AR):</label>
                                <asp:TextBox ID="txtDevelopedAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">Developed By Text (EN):</label>
                                <asp:TextBox ID="txtDevelopedEn" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-12 text-end pt-3 border-top">
                                <asp:Button ID="btnSaveSettings" runat="server" Text="حفظ إعدادات الشعارات والنصوص" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveSettings_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- 4.2 Add / Edit Bottom Link Panel -->
                <asp:Panel ID="pnlDownLinkForm" runat="server" Visible="false" CssClass="card mb-4 shadow-sm border-primary rounded-4">
                    <div class="card-header bg-primary text-white py-3 rounded-top-4 d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold">
                            <i class="bi bi-pencil-square me-2"></i>
                            <asp:Literal ID="ltDownLinkFormTitle" runat="server" Text="إضافة رابط سفلي" />
                        </h5>
                        <asp:LinkButton ID="btnCloseDownLinkForm" runat="server" CssClass="btn-close btn-close-white" OnClick="btnCancelDownLink_Click" aria-label="Close"></asp:LinkButton>
                    </div>
                    <div class="card-body p-4">
                        <asp:HiddenField ID="hfDownLinkId" runat="server" Value="0" />
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان الرابط (العربية - AR): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDownLinkTitleAr" runat="server" CssClass="form-control" Placeholder="مثال: سياسات الجامعة" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان الرابط (بالإنجليزية - EN): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDownLinkTitleEn" runat="server" CssClass="form-control" Placeholder="e.g. Policies" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الرابط العربي (URL - AR): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDownLinkUrlAr" runat="server" CssClass="form-control" Placeholder="/ar/SECURITYCYBER/Pages/Privacy-Policy.aspx" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">الرابط الإنجليزي (URL - EN): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDownLinkUrlEn" runat="server" CssClass="form-control" Placeholder="/en/SECURITYCYBER/Pages/Privacy-Policy.aspx" />
                            </div>

                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">ترتيب العرض:</label>
                                <asp:TextBox ID="txtDownLinkOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                            </div>

                            <div class="col-12 col-md-4 d-flex align-items-end">
                                <div class="form-check form-switch mb-2">
                                    <asp:CheckBox ID="chkDownLinkNewTab" runat="server" />
                                    <label class="form-check-label fw-semibold ms-2" for="<%= chkDownLinkNewTab.ClientID %>">فتح في نافذة جديدة</label>
                                </div>
                            </div>

                            <div class="col-12 col-md-4 d-flex align-items-end">
                                <div class="form-check form-switch mb-2">
                                    <asp:CheckBox ID="chkDownLinkVisible" runat="server" Checked="true" />
                                    <label class="form-check-label fw-semibold ms-2" for="<%= chkDownLinkVisible.ClientID %>">مفعل / معروض</label>
                                </div>
                            </div>

                            <div class="col-12 d-flex justify-content-end gap-2 pt-3 border-top">
                                <asp:Button ID="btnCancelDownLink" runat="server" Text="إلغاء" CssClass="btn btn-outline-secondary px-4 rounded-3" OnClick="btnCancelDownLink_Click" CausesValidation="false" />
                                <asp:Button ID="btnSaveDownLink" runat="server" Text="حفظ الرابط" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveDownLink_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- 4.3 Bottom Links Grid -->
                <div class="card shadow-sm border-0 rounded-4">
                    <div class="card-header bg-white py-3 border-bottom d-flex flex-wrap justify-content-between align-items-center gap-2">
                        <div class="d-flex align-items-center gap-2">
                            <h5 class="mb-0 fw-bold text-dark">
                                <i class="bi bi-card-checklist text-primary me-2"></i> الروابط السفلية للشريط الأخير (Down Links)
                            </h5>
                            <asp:Label ID="lblDownLinkCount" runat="server" CssClass="badge bg-primary-subtle text-primary rounded-pill px-3 py-2 fw-semibold" />
                        </div>
                        <asp:LinkButton ID="btnNewDownLink" runat="server" CssClass="btn btn-primary btn-sm rounded-3 px-3 py-2 fw-semibold" OnClick="btnNewDownLink_Click">
                            <i class="bi bi-plus-lg me-1"></i> إضافة رابط سفلي
                        </asp:LinkButton>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvDownLinks" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                      CssClass="table table-hover align-middle mb-0" GridLines="None"
                                      OnRowCommand="gvDownLinks_RowCommand" OnRowDataBound="gvDownLinks_RowDataBound">
                            <HeaderStyle CssClass="table-light text-secondary small text-nowrap" />
                            <Columns>
                                <asp:BoundField DataField="ItemOrder" HeaderText="الترتيب" ItemStyle-Width="60px" ItemStyle-CssClass="text-center fw-bold text-muted" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Title" HeaderText="العنوان بالعربية" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="TitleEn" HeaderText="العنوان بالإنجليزية" />
                                <asp:TemplateField HeaderText="الرابط (AR / EN)">
                                    <ItemTemplate>
                                        <div class="small">
                                            <div><span class="badge bg-secondary-subtle text-secondary me-1">AR</span> <a href='<%# Href(Eval("UrlAr")) %>' target="_blank" rel="noopener" class="text-truncate d-inline-block" style="max-width:240px"><%# Text(Eval("UrlAr")) %></a></div>
                                            <div><span class="badge bg-secondary-subtle text-secondary me-1">EN</span> <a href='<%# Href(Eval("UrlEn")) %>' target="_blank" rel="noopener" class="text-truncate d-inline-block" style="max-width:240px"><%# Text(Eval("UrlEn")) %></a></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="الحالة" ItemStyle-Width="90px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDownVisible" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="إجراءات" ItemStyle-Width="130px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-sm">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-outline-primary" CommandName="EditItem" CommandArgument='<%# Eval("ID") %>' ToolTip="تعديل">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M17 3a2.85 2.85 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/></svg><span class="visually-hidden">تعديل</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnToggle" runat="server" CssClass="btn btn-outline-secondary" CommandName="ToggleVisibility" CommandArgument='<%# Eval("ID") %>' ToolTip="تغيير الحالة">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg><span class="visually-hidden">تغيير الحالة</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-outline-danger" CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>' ToolTip="حذف"
                                                            OnClientClick="return confirm('هل أنت متأكد من حذف هذا الرابط؟');">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/><path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><path d="M10 11v6"/><path d="M14 11v6"/></svg><span class="visually-hidden">حذف</span>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-5 text-muted">
                                    <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                                    لا توجد روابط مسجلة حالياً.
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>

            </asp:Panel>

        </asp:Panel>
    </div>
</section>
