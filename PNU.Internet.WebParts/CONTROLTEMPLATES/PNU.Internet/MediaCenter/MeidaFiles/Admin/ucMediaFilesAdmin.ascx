<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMediaFilesAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.Admin.ucMediaFilesAdmin, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-4 mf-admin-container">
    <div class="container-fluid px-3 px-md-4">

        <%-- Header Title & Subtitle --%>
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 border-bottom pb-3">
            <div>
                <div class="d-flex align-items-center gap-2">
                    <span class="p-2 rounded-3 bg-primary bg-opacity-10 text-primary d-inline-flex">
                        <i class="hgi hgi-stroke hgi-file-02 fs-3" aria-hidden="true"></i>
                    </span>
                    <div>
                        <h2 class="h4 fw-bold mb-0 text-dark">
                            <asp:Literal ID="ltPageTitle" runat="server" Text="إدارة ملفات المركز الإعلامي" />
                        </h2>
                        <p class="text-muted small mb-0">
                            <asp:Literal ID="ltPageSubtitle" runat="server" Text="إضافة وتعديل وحذف الملفات الإعلامية الأسبوعية والشهرية وملفاتها المرفقة" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="d-flex align-items-center gap-2">
                <span class="badge bg-light text-secondary border px-3 py-2 d-inline-flex align-items-center gap-1" title="قائمة الصلاحيات المخصصة للمشرفين">
                    <i class="hgi hgi-stroke hgi-shield-tick text-success" aria-hidden="true"></i>
                    <span>المشرفون: <strong>MediaFileAdminUsers</strong> (/ar/ContentAdmin)</span>
                </span>
            </div>
        </div>

        <%-- Access Denied Alert --%>
        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-warning d-flex align-items-center gap-3 p-4 rounded-3 shadow-sm" role="alert">
                <i class="hgi hgi-stroke hgi-alert-circle fs-1 text-warning" aria-hidden="true"></i>
                <div>
                    <h5 class="alert-heading fw-bold mb-1">صلاحية الوصول غير متوفرة</h5>
                    <div>
                        <asp:Literal ID="ltDenied" runat="server" Text="ليس لديك صلاحية لإدارة ملفات المركز الإعلامي. يجب أن يكون حسابك مسجلاً ونشطاً في قائمة <code>MediaFileAdminUsers</code> على موقع <code>/ar/ContentAdmin</code>." />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <%-- Main Administration Panel --%>
        <asp:Panel ID="pnlMain" runat="server">

            <%-- Success Banner --%>
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div class="alert alert-success alert-dismissible fade show d-flex align-items-center gap-2 rounded-3 shadow-sm mb-4" role="alert">
                    <i class="hgi hgi-stroke hgi-tick-circle fs-4 text-success" aria-hidden="true"></i>
                    <div><asp:Literal ID="ltMessage" runat="server" /></div>
                    <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <%-- Error Banner --%>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger alert-dismissible fade show d-flex align-items-center gap-2 rounded-3 shadow-sm mb-4" role="alert">
                    <i class="hgi hgi-stroke hgi-alert-diamond fs-4 text-danger" aria-hidden="true"></i>
                    <div><asp:Literal ID="ltError" runat="server" /></div>
                    <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <%-- Summary Stats Cards --%>
            <div class="row g-3 mb-4">
                <div class="col-12 col-md-4">
                    <div class="card border-0 shadow-sm rounded-3 h-100 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-between p-3">
                            <div>
                                <span class="text-muted small d-block">إجمالي الملفات الإعلامية</span>
                                <h3 class="fw-bold mb-0 text-dark"><asp:Literal ID="ltTotalCount" runat="server" Text="0" /></h3>
                            </div>
                            <span class="p-3 rounded-circle bg-primary bg-opacity-10 text-primary">
                                <i class="hgi hgi-stroke hgi-folder-01 fs-3" aria-hidden="true"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-4">
                    <div class="card border-0 shadow-sm rounded-3 h-100 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-between p-3">
                            <div>
                                <span class="text-muted small d-block">الملفات الأسبوعية</span>
                                <h3 class="fw-bold mb-0 text-primary"><asp:Literal ID="ltWeeklyCount" runat="server" Text="0" /></h3>
                            </div>
                            <span class="p-3 rounded-circle bg-primary bg-opacity-10 text-primary">
                                <i class="hgi hgi-stroke hgi-calendar-03 fs-3" aria-hidden="true"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-4">
                    <div class="card border-0 shadow-sm rounded-3 h-100 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-between p-3">
                            <div>
                                <span class="text-muted small d-block">الملفات الشهرية</span>
                                <h3 class="fw-bold mb-0 text-info"><asp:Literal ID="ltMonthlyCount" runat="server" Text="0" /></h3>
                            </div>
                            <span class="p-3 rounded-circle bg-info bg-opacity-10 text-info">
                                <i class="hgi hgi-stroke hgi-calendar-02 fs-3" aria-hidden="true"></i>
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Action Toolbar & Filter Card --%>
            <div class="card border-0 shadow-sm rounded-3 mb-4">
                <div class="card-body p-3">
                    <div class="row g-3 align-items-center justify-content-between">
                        <div class="col-12 col-lg-7 d-flex flex-wrap align-items-center gap-2">
                            <div class="d-flex align-items-center gap-2 flex-grow-1" style="max-width: 260px;">
                                <label for="<%= ddlFilterType.ClientID %>" class="form-label small fw-semibold text-muted mb-0 text-nowrap">النوع:</label>
                                <asp:DropDownList ID="ddlFilterType" runat="server" CssClass="form-select form-select-sm"
                                                  AutoPostBack="true" OnSelectedIndexChanged="ddlFilterType_SelectedIndexChanged">
                                    <asp:ListItem Text="جميع الملفات" Value="" Selected="True" />
                                    <asp:ListItem Text="أسبوعي (Weekly)" Value="أسبوعي" />
                                    <asp:ListItem Text="شهري (Monthly)" Value="شهري" />
                                </asp:DropDownList>
                            </div>
                            <div class="d-flex align-items-center gap-1 flex-grow-1" style="max-width: 280px;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="بحث باسم الملف..." />
                                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnSearch_Click">
                                    <i class="hgi hgi-stroke hgi-search-01 align-middle" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                        </div>

                        <div class="col-12 col-lg-5 d-flex justify-content-lg-end gap-2 flex-wrap">
                            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnRefresh_Click" CausesValidation="false" ToolTip="تحديث القائمة">
                                <i class="hgi hgi-stroke hgi-refresh fs-6 align-middle" aria-hidden="true"></i>
                                <span class="d-none d-sm-inline">تحديث</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-sm btn-primary px-3" OnClick="btnNew_Click">
                                <i class="hgi hgi-stroke hgi-add-01 fs-6 align-middle me-1" aria-hidden="true"></i>
                                <span class="fw-semibold">إضافة ملف جديد</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Add / Edit Item Form Panel --%>
            <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card border-0 shadow rounded-3 mb-4 border-top border-primary border-4">
                <div class="card-header bg-transparent py-3 border-0">
                    <div class="d-flex justify-content-between align-items-center">
                        <h4 class="h5 fw-bold mb-0 text-dark">
                            <i class="hgi hgi-stroke hgi-edit-02 text-primary me-2" aria-hidden="true"></i>
                            <asp:Literal ID="ltFormTitle" runat="server" Text="إضافة ملف جديد" />
                        </h4>
                        <asp:LinkButton ID="btnCloseFormTop" runat="server" CssClass="btn-close" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
                <div class="card-body pt-0 px-4 pb-4">
                    <div class="row g-3">
                        <%-- File Name / Title --%>
                        <div class="col-12 col-md-6">
                            <label for="<%= txtTitle.ClientID %>" class="form-label fw-semibold">
                                اسم الملف / العنوان <span class="text-danger">*</span>
                            </label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="مثال: نورة في أسبوع ٦-١٢ يناير" MaxLength="250" />
                            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                                                        ErrorMessage="يرجى إدخال اسم أو عنوان الملف." Display="Dynamic"
                                                        CssClass="text-danger small mt-1" ValidationGroup="vgMediaFile" />
                        </div>

                        <%-- Media Type Choice --%>
                        <div class="col-12 col-md-3">
                            <label for="<%= ddlMediaType.ClientID %>" class="form-label fw-semibold">
                                نوع الملف <span class="text-danger">*</span>
                            </label>
                            <asp:DropDownList ID="ddlMediaType" runat="server" CssClass="form-select">
                                <asp:ListItem Text="أسبوعي (Weekly)" Value="أسبوعي" Selected="True" />
                                <asp:ListItem Text="شهري (Monthly)" Value="شهري" />
                            </asp:DropDownList>
                        </div>

                        <%-- Item Order --%>
                        <div class="col-12 col-md-3">
                            <label for="<%= txtItemOrder.ClientID %>" class="form-label fw-semibold">
                                الترتيب (Item Order)
                            </label>
                            <asp:TextBox ID="txtItemOrder" runat="server" CssClass="form-control" Text="1" TextMode="Number" />
                            <small class="text-muted">الأرقام الأصغر تظهر أولاً في العرض</small>
                        </div>

                        <%-- File Attachment Upload --%>
                        <div class="col-12">
                            <label for="<%= fuAttachment.ClientID %>" class="form-label fw-semibold">
                                الملف المرفق (PDF / Document / Media) <span class="text-danger">*</span>
                            </label>
                            <div class="input-group">
                                <asp:FileUpload ID="fuAttachment" runat="server" CssClass="form-control" accept=".pdf,.doc,.docx,.ppt,.pptx,.xls,.xlsx,.zip,.jpg,.png" />
                            </div>
                            <small class="text-muted d-block mt-1">
                                الملفات المدعومة: PDF, Word, PowerPoint, Excel, Images, ZIP. الحد الأقصى المسموح بحسب سياسة SharePoint.
                            </small>

                            <%-- Existing Attachment Notification when Editing --%>
                            <asp:Panel ID="pnlCurrentAttachment" runat="server" Visible="false" CssClass="mt-2 p-2 bg-light rounded-2 border d-inline-flex align-items-center gap-2">
                                <i class="hgi hgi-stroke hgi-attachment text-primary" aria-hidden="true"></i>
                                <span class="small text-muted">الملف الحالي:</span>
                                <asp:HyperLink ID="hlCurrentAttachment" runat="server" Target="_blank" CssClass="fw-semibold text-primary small text-decoration-none">
                                    <asp:Literal ID="ltCurrentAttachmentName" runat="server" />
                                </asp:HyperLink>
                                <span class="badge bg-secondary-subtle text-secondary small">اترك الحقل فارغاً للاحتفاظ بالملف الحالي</span>
                            </asp:Panel>
                        </div>
                    </div>

                    <%-- Form Submit / Cancel Buttons --%>
                    <div class="d-flex gap-2 flex-wrap mt-4 pt-3 border-top">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary px-4" OnClick="btnSave_Click" ValidationGroup="vgMediaFile">
                            <i class="hgi hgi-stroke hgi-tick-02 align-middle me-1" aria-hidden="true"></i>
                            <span>حفظ الملف</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-outline-secondary px-3" OnClick="btnCancel_Click" CausesValidation="false">
                            <span>إلغاء</span>
                        </asp:LinkButton>
                    </div>
                </div>
            </asp:Panel>

            <%-- Data Grid Card --%>
            <div class="card border-0 shadow-sm rounded-3">
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light text-muted small">
                                <tr>
                                    <th scope="col" style="width: 60px;" class="text-center">#</th>
                                    <th scope="col">اسم / عنوان الملف</th>
                                    <th scope="col" style="width: 140px;">نوع الملف</th>
                                    <th scope="col">الملف المرفق</th>
                                    <th scope="col" style="width: 90px;" class="text-center">الترتيب</th>
                                    <th scope="col" style="width: 140px;">تاريخ الإنشاء</th>
                                    <th scope="col" style="width: 130px;" class="text-end px-3">إجراءات</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center text-muted small"><%# Eval("ID") %></td>
                                            <td>
                                                <div class="fw-semibold text-dark"><%#: Eval("Title") %></div>
                                            </td>
                                            <td>
                                                <span class='badge <%# Convert.ToString(Eval("MediaType")) == "أسبوعي" ? "bg-primary-subtle text-primary border border-primary-subtle" : "bg-info-subtle text-info-emphasis border border-info-subtle" %> px-2 py-1 rounded-pill'>
                                                    <%#: Eval("MediaType") %>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:PlaceHolder ID="phAttachment" runat="server" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("AttachmentUrl"))) %>'>
                                                    <a href='<%#: Eval("AttachmentUrl") %>' target="_blank" class="btn btn-sm btn-outline-primary d-inline-flex align-items-center gap-1 py-1 px-2" title="معاينة وتحميل">
                                                        <i class="hgi hgi-stroke hgi-download-02" aria-hidden="true"></i>
                                                        <span class="small"><%#: Eval("AttachmentFileName") %></span>
                                                    </a>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="phNoAttachment" runat="server" Visible='<%# string.IsNullOrEmpty(Convert.ToString(Eval("AttachmentUrl"))) %>'>
                                                    <span class="text-muted small"><i class="hgi hgi-stroke hgi-alert-circle" aria-hidden="true"></i> لا يوجد مرفق</span>
                                                </asp:PlaceHolder>
                                            </td>
                                            <td class="text-center fw-semibold text-muted"><%# Eval("ItemOrder") %></td>
                                            <td class="small text-muted"><%# Eval("CreatedDate") %></td>
                                            <td class="text-end px-3 text-nowrap">
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-secondary py-1 px-2 me-1"
                                                                CommandName="EditItem" CommandArgument='<%# Eval("ID") %>'
                                                                ToolTip="تعديل هذا الملف" CausesValidation="false">
                                                    <i class="hgi hgi-stroke hgi-edit-02 align-middle" aria-hidden="true"></i>
                                                    <span class="small">تعديل</span>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-outline-danger py-1 px-2"
                                                                CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>'
                                                                OnClientClick="return confirm('هل أنت متأكد من رغبتك في حذف هذا الملف الإعلامي نهائياً؟');"
                                                                ToolTip="حذف هذا الملف" CausesValidation="false">
                                                    <i class="hgi hgi-stroke hgi-delete-02 align-middle" aria-hidden="true"></i>
                                                    <span class="small">حذف</span>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <%-- Empty State --%>
                    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="text-center py-5">
                        <div class="py-4">
                            <span class="p-3 rounded-circle bg-light text-muted d-inline-flex mb-3">
                                <i class="hgi hgi-stroke hgi-file-search fs-1" aria-hidden="true"></i>
                            </span>
                            <h5 class="fw-bold text-dark mb-1">لا توجد ملفات إعلامية حالياً</h5>
                            <p class="text-muted small mb-3">لم يتم العثور على أي ملفات مطابقة لمعايير البحث الحالية.</p>
                            <asp:LinkButton ID="btnEmptyNew" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnNew_Click">
                                <i class="hgi hgi-stroke hgi-add-01 me-1" aria-hidden="true"></i> إضافة أول ملف
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <%-- Hidden State Fields --%>
            <asp:HiddenField ID="hfMode" runat="server" Value="New" />
            <asp:HiddenField ID="hfItemId" runat="server" Value="0" />

        </asp:Panel>

    </div>
</section>
