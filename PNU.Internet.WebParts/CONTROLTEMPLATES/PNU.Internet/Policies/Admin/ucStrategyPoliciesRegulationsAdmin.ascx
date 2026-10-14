<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ucStrategyPoliciesRegulationsAdmin.ascx.cs"
    Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies.Admin.ucStrategyPoliciesRegulationsAdmin" %>

<section class="py-4 strategy-policies-admin-control">
    <div class="container-fluid">

        <!-- Page Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 pb-3 border-bottom">
            <div>
                <h3 class="fw-bold mb-1 text-primary">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-2" style="vertical-align:-4px;"><path d="M4 19.5v-15A2.5 2.5 0 0 1 6.5 2H20v20H6.5a2.5 2.5 0 0 1-2.5-2.5Z"/><path d="M6 6h10"/><path d="M6 10h10"/></svg>
                    إدارة الاستراتيجيات والسياسات والأنظمة واللوائح
                </h3>
                <p class="text-muted small mb-0">
                    إدارة عناوين وأوصاف الأقسام وبطاقات الروابط باللغتين العربية والإنجليزية، وتحديد الترتيب وحالات العرض في صفحة الاستراتيجيات والسياسات والأنظمة واللوائح (<span class="badge bg-light text-dark border">/ar/SECURITYCYBER</span>).
                </p>
            </div>
            <asp:PlaceHolder ID="phHeaderActions" runat="server">
                <div class="d-flex gap-2">
                    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-outline-secondary btn-sm rounded-3 px-3" OnClick="btnRefresh_Click">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-1" style="vertical-align:-2px;"><path d="M3 12a9 9 0 0 1 9-9 9.75 9.75 0 0 1 6.74 2.74L21 8"/><path d="M21 3v5h-5"/><path d="M21 12a9 9 0 0 1-9 9 9.75 9.75 0 0 1-6.74-2.74L3 16"/><path d="M3 21v-5h5"/></svg>
                        تحديث
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnProvision" runat="server" CssClass="btn btn-outline-warning btn-sm rounded-3 px-3" OnClick="btnProvision_Click"
                                    OnClientClick="return confirm('سيتم التحقق من وجود القوائم والحقول وتهيئة البيانات الافتراضية إن كانت غير موجودة. هل تريد المتابعة؟');">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-1" style="vertical-align:-2px;"><path d="M12.22 2h-.44a2 2 0 0 0-2 2v.18a2 2 0 0 1-1 1.73l-.43.25a2 2 0 0 1-2 0l-.15-.08a2 2 0 0 0-2.73.73l-.22.38a2 2 0 0 0 .73 2.73l.15.1a2 2 0 0 1 1 1.72v.51a2 2 0 0 1-1 1.74l-.15.09a2 2 0 0 0-.73 2.73l.22.38a2 2 0 0 0 2.73.73l.15-.08a2 2 0 0 1 2 0l.43.25a2 2 0 0 1 1 1.73V20a2 2 0 0 0 2 2h.44a2 2 0 0 0 2-2v-.18a2 2 0 0 1 1-1.73l.43-.25a2 2 0 0 1 2 0l.15.08a2 2 0 0 0 2.73-.73l.22-.39a2 2 0 0 0-.73-2.73l-.15-.08a2 2 0 0 1-1-1.74v-.5a2 2 0 0 1 1-1.74l.15-.09a2 2 0 0 0 .73-2.73l-.22-.38a2 2 0 0 0-2.73-.73l-.15.08a2 2 0 0 1-2 0l-.43-.25a2 2 0 0 1-1-1.73V4a2 2 0 0 0-2-2z"/><circle cx="12" cy="12" r="3"/></svg>
                        تهيئة القوائم
                    </asp:LinkButton>
                </div>
            </asp:PlaceHolder>
        </div>

        <!-- Access Denied Panel -->
        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-danger rounded-3" role="alert">
                <h5 class="alert-heading fw-bold mb-1">غير مصرح بالوصول</h5>
                <asp:Literal ID="ltDenied" runat="server" Text="عفواً، ليس لديك الصلاحية الكافية لإدارة هذه الصفحة." />
            </div>
        </asp:Panel>

        <!-- Main Panel -->
        <asp:Panel ID="pnlMain" runat="server">

            <!-- Alerts -->
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show rounded-3 mb-4" role="alert">
                <asp:Literal ID="ltSuccessMsg" runat="server" />
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show rounded-3 mb-4" role="alert">
                <asp:Literal ID="ltErrorMsg" runat="server" />
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </asp:Panel>

            <!-- Navigation Tabs -->
            <ul class="nav nav-pills mb-4 gap-2" role="tablist">
                <li class="nav-item" role="presentation">
                    <asp:LinkButton ID="btnTabCards" runat="server" CssClass="nav-link active px-4 py-2 rounded-3" OnClick="btnTabCards_Click">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-1" style="vertical-align:-2px;"><rect width="18" height="18" x="3" y="3" rx="2"/><path d="M3 9h18"/><path d="M9 21V9"/></svg>
                        إدارة البطاقات والروابط
                    </asp:LinkButton>
                </li>
                <li class="nav-item" role="presentation">
                    <asp:LinkButton ID="btnTabSections" runat="server" CssClass="nav-link px-4 py-2 rounded-3" OnClick="btnTabSections_Click">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-1" style="vertical-align:-2px;"><path d="M4 6h16"/><path d="M4 12h16"/><path d="M4 18h16"/></svg>
                        إدارة الأقسام والعناوين
                    </asp:LinkButton>
                </li>
            </ul>

            <!-- ============================================================ -->
            <!-- TAB 1: CARDS MANAGEMENT                                      -->
            <!-- ============================================================ -->
            <asp:PlaceHolder ID="phTabCards" runat="server" Visible="true">

                <!-- Filter & Add Toolbar -->
                <div class="card border-0 shadow-sm rounded-3 mb-4">
                    <div class="card-body p-3">
                        <div class="row g-3 align-items-center justify-content-between">
                            <div class="col-12 col-md-auto d-flex align-items-center gap-2">
                                <label class="form-label mb-0 fw-semibold text-muted small">تصفية حسب القسم:</label>
                                <asp:DropDownList ID="ddlFilterSection" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterSection_SelectedIndexChanged" style="min-width: 200px;">
                                    <asp:ListItem Value="ALL" Text="جميع الأقسام" Selected="True" />
                                    <asp:ListItem Value="strategies" Text="الاستراتيجيات (Strategies)" />
                                    <asp:ListItem Value="policies" Text="السياسات (Policies)" />
                                    <asp:ListItem Value="regulations" Text="الأنظمة واللوائح (Regulations & Bylaws)" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 col-md-auto">
                                <asp:LinkButton ID="btnAddNewCard" runat="server" CssClass="btn btn-primary btn-sm rounded-3 px-3" OnClick="btnAddNewCard_Click">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-1" style="vertical-align:-2px;"><path d="M5 12h14"/><path d="M12 5v14"/></svg>
                                    إضافة بطاقة جديدة
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Card Edit/Add Form -->
                <asp:Panel ID="pnlCardForm" runat="server" Visible="false" CssClass="card border-primary shadow-sm rounded-3 mb-4">
                    <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center py-2 px-3">
                        <h6 class="mb-0 fw-bold">
                            <asp:Literal ID="ltCardFormTitle" runat="server" Text="إضافة بطاقة جديدة" />
                        </h6>
                        <asp:LinkButton ID="btnCloseCardForm" runat="server" CssClass="btn-close btn-close-white" OnClick="btnCancelCard_Click" aria-label="Close" />
                    </div>
                    <div class="card-body p-4">
                        <asp:HiddenField ID="hfCardId" runat="server" Value="0" />

                        <div class="row g-3">
                            <div class="col-12 col-md-4">
                                <label class="form-label small fw-bold">القسم <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlCardSection" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="strategies" Text="الاستراتيجيات (Strategies)" />
                                    <asp:ListItem Value="policies" Text="السياسات (Policies)" />
                                    <asp:ListItem Value="regulations" Text="الأنظمة واللوائح (Regulations & Bylaws)" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label small fw-bold">الأيقونة (CSS Class) <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCardIcon" runat="server" CssClass="form-control" placeholder="hgi hgi-stroke hgi-file-02" />
                            </div>
                            <div class="col-12 col-md-2">
                                <label class="form-label small fw-bold">الترتيب <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCardOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                            </div>
                            <div class="col-12 col-md-2">
                                <label class="form-label small fw-bold">طريقة الفتح</label>
                                <asp:DropDownList ID="ddlCardTarget" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="_self" Text="في نفس الصفحة (_self)" />
                                    <asp:ListItem Value="_blank" Text="في نافذة جديدة (_blank)" />
                                </asp:DropDownList>
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">العنوان بالعربية <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCardTitleAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">العنوان بالإنجليزية</label>
                                <asp:TextBox ID="txtCardTitleEn" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">الرابط بالعربية <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCardUrlAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="/ar/... أو https://..." />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">الرابط بالإنجليزية</label>
                                <asp:TextBox ID="txtCardUrlEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="/en/... أو https://..." />
                            </div>

                            <div class="col-12 col-md-3">
                                <label class="form-label small fw-bold">نص الزر بالعربية</label>
                                <asp:TextBox ID="txtCardBtnAr" runat="server" CssClass="form-control" placeholder="عرض التفاصيل / عرض الملف" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label small fw-bold">نص الزر بالإنجليزية</label>
                                <asp:TextBox ID="txtCardBtnEn" runat="server" CssClass="form-control" placeholder="View Details / View File" />
                            </div>

                            <div class="col-12 col-md-2">
                                <label class="form-label small fw-bold">الشارة بالعربية (Badge)</label>
                                <asp:TextBox ID="txtCardBadgeAr" runat="server" CssClass="form-control" placeholder="مؤرشفة" />
                            </div>
                            <div class="col-12 col-md-2">
                                <label class="form-label small fw-bold">الشارة بالإنجليزية</label>
                                <asp:TextBox ID="txtCardBadgeEn" runat="server" CssClass="form-control" placeholder="Archived" />
                            </div>
                            <div class="col-12 col-md-2">
                                <label class="form-label small fw-bold">فئة الشارة (CSS Class)</label>
                                <asp:TextBox ID="txtCardBadgeClass" runat="server" CssClass="form-control" placeholder="badge-warning" />
                            </div>

                            <div class="col-12">
                                <div class="form-check form-switch mt-2">
                                    <asp:CheckBox ID="chkCardVisibility" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label fw-semibold" for="<%= chkCardVisibility.ClientID %>">البطاقة نشطة وظاهرة للمستخدمين</label>
                                </div>
                            </div>

                            <div class="col-12 d-flex justify-content-end gap-2 pt-3 border-top">
                                <asp:Button ID="btnCancelCard" runat="server" Text="إلغاء" CssClass="btn btn-secondary px-4 rounded-3" OnClick="btnCancelCard_Click" />
                                <asp:Button ID="btnSaveCard" runat="server" Text="حفظ البطاقة" CssClass="btn btn-primary px-4 rounded-3" OnClick="btnSaveCard_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Cards Grid -->
                <div class="card border-0 shadow-sm rounded-3 overflow-hidden">
                    <div class="table-responsive">
                        <asp:Repeater ID="rptCardsAdmin" runat="server" OnItemCommand="rptCardsAdmin_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="table-light text-muted small">
                                        <tr>
                                            <th style="width: 70px;" class="text-center">#</th>
                                            <th style="width: 140px;">القسم</th>
                                            <th>العنوان (AR / EN)</th>
                                            <th style="width: 100px;">الشارة</th>
                                            <th>الرابط (AR / EN)</th>
                                            <th style="width: 110px;">الزر</th>
                                            <th style="width: 90px;" class="text-center">الفتح</th>
                                            <th style="width: 80px;" class="text-center">الحالة</th>
                                            <th style="width: 130px;" class="text-center">إجراءات</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="text-center">
                                        <div class="d-flex flex-column align-items-center">
                                            <span class="fw-bold small text-muted"><%# Eval("SortOrder") %></span>
                                            <div class="btn-group btn-group-sm mt-1">
                                                <asp:LinkButton ID="btnMoveUp" runat="server" CommandName="MoveUp" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-link p-0 text-decoration-none" ToolTip="تحريك لأعلى">
                                                    ▲
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnMoveDown" runat="server" CommandName="MoveDown" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-link p-0 text-decoration-none ms-1" ToolTip="تحريك لأسفل">
                                                    ▼
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <span class='badge <%# GetSectionBadgeClass(Eval("SectionKey")) %>'>
                                            <%# GetSectionDisplayName(Eval("SectionKey")) %>
                                        </span>
                                    </td>
                                    <td>
                                        <div class="fw-bold text-dark"><%# Text(Eval("Title")) %></div>
                                        <div class="small text-muted"><%# Text(Eval("TitleEn")) %></div>
                                    </td>
                                    <td>
                                        <%# string.IsNullOrEmpty(Eval("Badge") as string) ? "<span class='text-muted small'>-</span>" : RenderBadge(Eval("Badge"), Eval("BadgeClass")) %>
                                    </td>
                                    <td>
                                        <div class="small">
                                            <div><span class="badge bg-secondary-subtle text-secondary me-1">AR</span> <a href='<%# Href(Eval("Url")) %>' target="_blank" rel="noopener" class="text-truncate d-inline-block" style="max-width:240px"><%# Text(Eval("Url")) %></a></div>
                                            <div><span class="badge bg-secondary-subtle text-secondary me-1">EN</span> <a href='<%# Href(Eval("UrlEn")) %>' target="_blank" rel="noopener" class="text-truncate d-inline-block" style="max-width:240px"><%# Text(Eval("UrlEn")) %></a></div>
                                        </div>
                                    </td>
                                    <td>
                                        <span class="badge bg-light text-dark border"><%# Text(Eval("ButtonText")) %></span>
                                    </td>
                                    <td class="text-center">
                                        <span class="badge bg-light text-muted border"><%# Text(Eval("Target")) %></span>
                                    </td>
                                    <td class="text-center">
                                        <%# (bool)Eval("Visibility") ? "<span class='badge bg-success-subtle text-success'>نشط</span>" : "<span class='badge bg-secondary-subtle text-secondary'>مخفي</span>" %>
                                    </td>
                                    <td class="text-center">
                                        <div class="btn-group btn-group-sm">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-outline-primary" CommandName="EditCard" CommandArgument='<%# Eval("ID") %>' ToolTip="تعديل">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 3a2.85 2.85 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/></svg>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnToggle" runat="server" CssClass="btn btn-outline-secondary" CommandName="ToggleCard" CommandArgument='<%# Eval("ID") %>' ToolTip="تغيير الحالة">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-outline-danger" CommandName="DeleteCard" CommandArgument='<%# Eval("ID") %>' ToolTip="حذف"
                                                            OnClientClick="return confirm('هل أنت متأكد من حذف هذه البطاقة؟');">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/><path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/></svg>
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </asp:PlaceHolder>

            <!-- ============================================================ -->
            <!-- TAB 2: SECTIONS MANAGEMENT                                   -->
            <!-- ============================================================ -->
            <asp:PlaceHolder ID="phTabSections" runat="server" Visible="false">

                <!-- Section Edit Form -->
                <asp:Panel ID="pnlSectionForm" runat="server" Visible="false" CssClass="card border-primary shadow-sm rounded-3 mb-4">
                    <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center py-2 px-3">
                        <h6 class="mb-0 fw-bold">تعديل بيانات القسم</h6>
                        <asp:LinkButton ID="btnCloseSecForm" runat="server" CssClass="btn-close btn-close-white" OnClick="btnCancelSec_Click" aria-label="Close" />
                    </div>
                    <div class="card-body p-4">
                        <asp:HiddenField ID="hfSectionId" runat="server" Value="0" />

                        <div class="row g-3">
                            <div class="col-12 col-md-4">
                                <label class="form-label small fw-bold">معرف القسم (Key)</label>
                                <asp:TextBox ID="txtSecKey" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label small fw-bold">الترتيب</label>
                                <asp:TextBox ID="txtSecOrder" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>
                            <div class="col-12 col-md-4 d-flex align-items-end">
                                <div class="form-check form-switch mb-2">
                                    <asp:CheckBox ID="chkSecVisibility" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label fw-semibold" for="<%= chkSecVisibility.ClientID %>">القسم نشط وظاهر</label>
                                </div>
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">عنوان القسم بالعربية <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtSecTitleAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">عنوان القسم بالإنجليزية</label>
                                <asp:TextBox ID="txtSecTitleEn" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">وصف القسم بالعربية</label>
                                <asp:TextBox ID="txtSecDescAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label small fw-bold">وصف القسم بالإنجليزية</label>
                                <asp:TextBox ID="txtSecDescEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            </div>

                            <div class="col-12 d-flex justify-content-end gap-2 pt-3 border-top">
                                <asp:Button ID="btnCancelSec" runat="server" Text="إلغاء" CssClass="btn btn-secondary px-4 rounded-3" OnClick="btnCancelSec_Click" />
                                <asp:Button ID="btnSaveSec" runat="server" Text="حفظ القسم" CssClass="btn btn-primary px-4 rounded-3" OnClick="btnSaveSec_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Sections Grid -->
                <div class="card border-0 shadow-sm rounded-3 overflow-hidden">
                    <div class="table-responsive">
                        <asp:Repeater ID="rptSectionsAdmin" runat="server" OnItemCommand="rptSectionsAdmin_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="table-light text-muted small">
                                        <tr>
                                            <th style="width: 70px;" class="text-center">#</th>
                                            <th style="width: 130px;">المعرف</th>
                                            <th style="width: 220px;">العنوان (AR / EN)</th>
                                            <th>الوصف (AR / EN)</th>
                                            <th style="width: 80px;" class="text-center">الحالة</th>
                                            <th style="width: 90px;" class="text-center">إجراءات</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="text-center fw-bold text-muted"><%# Eval("ItemOrder") %></td>
                                    <td>
                                        <span class='badge <%# GetSectionBadgeClass(Eval("SectionKey")) %>'>
                                            <%# Text(Eval("SectionKey")) %>
                                        </span>
                                    </td>
                                    <td>
                                        <div class="fw-bold text-dark"><%# Text(Eval("Title")) %></div>
                                        <div class="small text-muted"><%# Text(Eval("TitleEn")) %></div>
                                    </td>
                                    <td>
                                        <div class="small text-dark mb-1"><%# Text(Eval("Description")) %></div>
                                        <div class="small text-muted"><%# Text(Eval("DescriptionEn")) %></div>
                                    </td>
                                    <td class="text-center">
                                        <%# (bool)Eval("Visibility") ? "<span class='badge bg-success-subtle text-success'>نشط</span>" : "<span class='badge bg-secondary-subtle text-secondary'>مخفي</span>" %>
                                    </td>
                                    <td class="text-center">
                                        <asp:LinkButton ID="btnEditSec" runat="server" CssClass="btn btn-outline-primary btn-sm" CommandName="EditSection" CommandArgument='<%# Eval("ID") %>' ToolTip="تعديل">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 3a2.85 2.85 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/></svg>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </asp:PlaceHolder>

        </asp:Panel>
    </div>
</section>
