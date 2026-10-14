<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ucNavCardsAdmin.ascx.cs"
    Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies.Admin.ucNavCardsAdmin" %>

<%-- Bootstrap Icons for the bi-* header/alert icons (same CDN as ucUnifiedContentAdmin); action buttons use inline SVG --%>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

<section class="py-4 policies-navcards-admin-control">
    <div class="container-fluid">

        <!-- Page Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 pb-3 border-bottom">
            <div>
                <h3 class="fw-bold mb-1 text-primary">
                    <i class="bi bi-shield-check me-2"></i>
                    <asp:Literal ID="ltAdminHeader" runat="server" Text="إدارة بطاقات التنقل - السياسات وحوكمة البيانات" />
                </h3>
                <p class="text-muted small mb-0">
                    إدارة وإضافة وتعديل وترتيب بطاقات صفحة سياسات الجامعة في موقع الأمن السيبراني (<span class="badge bg-light text-dark border">/ar/SECURITYCYBER/Lists/NavigationCards</span>).
                </p>
            </div>
            <asp:PlaceHolder ID="phHeaderActions" runat="server" Visible="false">
            <div class="d-flex gap-2">
                <asp:LinkButton ID="btnAddNewTop" runat="server" CssClass="btn btn-primary btn-sm rounded-3 px-3 fw-semibold" OnClick="btnAddNew_Click">
                    <i class="bi bi-plus-circle me-1"></i> إضافة بطاقة جديدة
                </asp:LinkButton>
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-outline-secondary btn-sm rounded-3 px-3" OnClick="btnRefresh_Click">
                    <i class="bi bi-arrow-clockwise me-1"></i> تحديث
                </asp:LinkButton>
                <asp:LinkButton ID="btnProvision" runat="server" CssClass="btn btn-outline-warning btn-sm rounded-3 px-3" OnClick="btnProvision_Click"
                                OnClientClick="return confirm('سيتم التحقق من القوائم والحقول وتحديث الروابط الناقصة في الموقع /ar/SECURITYCYBER. هل تريد المتابعة؟');">
                    <i class="bi bi-gear-wide-connected me-1"></i> تهيئة القائمة والروابط
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

            <!-- Card Form (Add / Edit) -->
            <asp:Panel ID="pnlEditForm" runat="server" Visible="false">
                <div class="card mb-4 shadow-sm border-0 rounded-4 border-top border-primary border-3">
                    <div class="card-header bg-white py-3 border-bottom d-flex justify-content-between align-items-center">
                        <h5 class="mb-0 fw-bold text-dark">
                            <i class="bi bi-pencil-square text-primary me-2"></i>
                            <asp:Literal ID="ltFormTitle" runat="server" Text="إضافة بطاقة جديدة" />
                        </h5>
                        <asp:LinkButton ID="btnCloseForm" runat="server" CssClass="btn-close" OnClick="btnCancelEdit_Click" aria-label="إغلاق"></asp:LinkButton>
                    </div>
                    <div class="card-body p-4">
                        <asp:HiddenField ID="hfCardId" runat="server" Value="0" />

                        <div class="row g-3">
                            <!-- Title AR -->
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان البطاقة (العربية - AR): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtTitleAr" runat="server" CssClass="form-control" Placeholder="مثال: سياسات حوكمة البيانات" />
                            </div>

                            <!-- Title EN -->
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان البطاقة (بالإنجليزية - EN):</label>
                                <asp:TextBox ID="txtTitleEn" runat="server" CssClass="form-control" Placeholder="e.g. Data Governance Policies" />
                            </div>

                            <!-- URL AR -->
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">رابط البطاقة (العربية - AR): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtUrlAr" runat="server" CssClass="form-control" Placeholder="مثال: /ar/Departments/DM/Pages/DMPolicy.aspx" />
                            </div>

                            <!-- URL EN -->
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">رابط البطاقة (بالإنجليزية - EN):</label>
                                <asp:TextBox ID="txtUrlEn" runat="server" CssClass="form-control" Placeholder="e.g. /en/Departments/DM/Pages/DMPolicy.aspx" />
                            </div>

                            <!-- Button Text AR -->
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-semibold">نص الزر (العربية - AR):</label>
                                <asp:TextBox ID="txtButtonTextAr" runat="server" CssClass="form-control" Text="عرض التفاصيل" />
                            </div>

                            <!-- Button Text EN -->
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-semibold">نص الزر (بالإنجليزية - EN):</label>
                                <asp:TextBox ID="txtButtonTextEn" runat="server" CssClass="form-control" Text="View Details" />
                            </div>

                            <!-- Target -->
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-semibold">نافذة الفتح (Target):</label>
                                <asp:DropDownList ID="ddlTarget" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="_blank" Text="نافذة جديدة (_blank)" Selected="True" />
                                    <asp:ListItem Value="_self" Text="نفس الصفحة (_self)" />
                                </asp:DropDownList>
                            </div>

                            <!-- Sort Order -->
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-semibold">الترتيب (Order):</label>
                                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="form-control" Text="1" TextMode="Number" />
                            </div>

                            <!-- Icon Class -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">فئة الأيقونة (Icon Class):</label>
                                <div class="input-group">
                                    <span class="input-group-text bg-light"><i class="bi bi-stars"></i></span>
                                    <asp:TextBox ID="txtIcon" runat="server" CssClass="form-control" Placeholder="مثال: hgi hgi-stroke hgi-shield-01" />
                                </div>
                                <div class="small text-muted mt-2 d-flex flex-wrap gap-2 align-items-center">
                                    <span>أيقونات شائعة:</span>
                                    <a href="javascript:void(0)" class="badge bg-light text-dark border text-decoration-none" onclick="document.getElementById('<%= txtIcon.ClientID %>').value='hgi hgi-stroke hgi-shield-01';">درع / سياسة الخصوصية</a>
                                    <a href="javascript:void(0)" class="badge bg-light text-dark border text-decoration-none" onclick="document.getElementById('<%= txtIcon.ClientID %>').value='hgi hgi-stroke hgi-database';">قواعد بيانات / حوكمة البيانات</a>
                                    <a href="javascript:void(0)" class="badge bg-light text-dark border text-decoration-none" onclick="document.getElementById('<%= txtIcon.ClientID %>').value='hgi hgi-stroke hgi-folder-security';">مجلد أمان / إدارة أصول المعلومات</a>
                                    <a href="javascript:void(0)" class="badge bg-light text-dark border text-decoration-none" onclick="document.getElementById('<%= txtIcon.ClientID %>').value='hgi hgi-stroke hgi-user-check-01';">مستخدم / الاستخدام المقبول</a>
                                    <a href="javascript:void(0)" class="badge bg-light text-dark border text-decoration-none" onclick="document.getElementById('<%= txtIcon.ClientID %>').value='hgi hgi-stroke hgi-computer-programming-01';">تقنية وحوكمة / IT Governance</a>
                                    <a href="javascript:void(0)" class="badge bg-light text-dark border text-decoration-none" onclick="document.getElementById('<%= txtIcon.ClientID %>').value='hgi hgi-stroke hgi-agreement-01';">اتفاقية / Terms of Use</a>
                                    <a href="javascript:void(0)" class="badge bg-light text-dark border text-decoration-none" onclick="document.getElementById('<%= txtIcon.ClientID %>').value='hgi hgi-stroke hgi-ai-generative';">ذكاء اصطناعي / Generative AI</a>
                                </div>
                            </div>

                            <!-- Actions -->
                            <div class="col-12 d-flex justify-content-end gap-2 pt-3 border-top mt-3">
                                <asp:Button ID="btnCancelEdit" runat="server" Text="إلغاء" CssClass="btn btn-outline-secondary px-4 rounded-3" OnClick="btnCancelEdit_Click" />
                                <asp:Button ID="btnSaveCard" runat="server" Text="حفظ البطاقة" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveCard_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Cards Table Card -->
            <div class="card shadow-sm border-0 rounded-4">
                <div class="card-header bg-white py-3 border-bottom d-flex justify-content-between align-items-center">
                    <h5 class="mb-0 fw-bold text-dark">
                        <i class="bi bi-grid-3x3-gap text-primary me-2"></i>
                        بطاقات السياسات المعروضة (<asp:Literal ID="ltCount" runat="server" Text="0" />)
                    </h5>
                    <asp:LinkButton ID="btnAddNew" runat="server" CssClass="btn btn-primary btn-sm rounded-3 px-3 fw-semibold" OnClick="btnAddNew_Click">
                        <i class="bi bi-plus-lg me-1"></i> إضافة بطاقة
                    </asp:LinkButton>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light">
                                <tr>
                                    <th style="width: 70px;" class="text-center">الترتيب</th>
                                    <th style="width: 70px;" class="text-center">الأيقونة</th>
                                    <th>العنوان (AR / EN)</th>
                                    <th>الرابط (AR / EN)</th>
                                    <th style="width: 140px;">الزر والهدف</th>
                                    <th style="width: 170px;" class="text-center">الإجراءات</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptCardsGrid" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center fw-bold text-secondary">
                                                <%# Text(Eval("SortOrder")) %>
                                            </td>
                                            <td class="text-center">
                                                <div class="p-2 rounded-3 bg-light d-inline-block text-primary">
                                                    <i class='<%# Attr(Eval("Icon")) %> fs-4' aria-hidden="true"></i>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="fw-bold text-dark"><%# Text(Eval("Title")) %></div>
                                                <div class="text-muted small"><%# Text(Eval("TitleEn")) %></div>
                                            </td>
                                            <td>
                                                <div class="text-truncate text-secondary small" style="max-width: 280px;" title='<%# Attr(Eval("Url")) %>'>
                                                    <i class="bi bi-link-45deg me-1"></i> <%# Text(Eval("Url")) %>
                                                </div>
                                                <div class="text-truncate text-muted small" style="max-width: 280px;" title='<%# Attr(Eval("UrlEn")) %>'>
                                                    <i class="bi bi-link-45deg me-1"></i> <%# Text(Eval("UrlEn")) %>
                                                </div>
                                            </td>
                                            <td>
                                                <span class="badge bg-secondary-subtle text-secondary mb-1 d-block text-truncate">
                                                    <%# Text(Eval("ButtonText")) %> / <%# Text(Eval("ButtonTextEn")) %>
                                                </span>
                                                <span class="badge bg-light text-dark border">
                                                    <%# Text(Eval("Target")) %>
                                                </span>
                                            </td>
                                            <td class="text-center">
                                                <div class="btn-group btn-group-sm" role="group">
                                                    <asp:LinkButton ID="btnMoveUp" runat="server" CssClass="btn btn-outline-secondary" ToolTip="تحريك للأعلى" CommandArgument='<%# Eval("ID") %>' OnClick="btnMoveUp_Click">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M12 19V5"/><path d="m5 12 7-7 7 7"/></svg><span class="visually-hidden">تحريك للأعلى</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnMoveDown" runat="server" CssClass="btn btn-outline-secondary" ToolTip="تحريك للأسفل" CommandArgument='<%# Eval("ID") %>' OnClick="btnMoveDown_Click">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M12 5v14"/><path d="m19 12-7 7-7-7"/></svg><span class="visually-hidden">تحريك للأسفل</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-outline-primary" ToolTip="تعديل" CommandArgument='<%# Eval("ID") %>' OnClick="btnEdit_Click">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M17 3a2.85 2.85 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/></svg><span class="visually-hidden">تعديل</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-outline-danger" ToolTip="حذف" CommandArgument='<%# Eval("ID") %>' OnClick="btnDelete_Click" OnClientClick="return confirm('هل أنت متأكد من حذف هذه البطاقة نهائياً؟');">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/><path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><path d="M10 11v6"/><path d="M14 11v6"/></svg><span class="visually-hidden">حذف</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:PlaceHolder ID="pnlNoData" runat="server" Visible='<%# rptCardsGrid.Items.Count == 0 %>'>
                                            <tr>
                                                <td colspan="6" class="text-center py-5 text-muted">
                                                    <i class="bi bi-inbox fs-1 d-block mb-2 text-secondary"></i>
                                                    لا توجد بطاقات مضافة حتى الآن. انقر على "تهيئة القائمة والروابط" أو "إضافة بطاقة جديدة".
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

        </asp:Panel>

    </div>
</section>
