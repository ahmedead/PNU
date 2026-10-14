<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeNoraFutureAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.Admin.ucHomeNoraFutureAdmin" %>

<%-- Bootstrap Icons for the bi-* header/alert icons (same CDN as ucUnifiedContentAdmin); action buttons use inline SVG --%>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

<section class="py-4 nora-future-admin-control">
    <div class="container-fluid">

        <!-- Page Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 pb-3 border-bottom">
            <div>
                <h3 class="fw-bold mb-1 text-primary">
                    <i class="bi bi-mortarboard me-2"></i>
                    <asp:Literal ID="ltAdminHeader" runat="server" Text="إدارة قسم برامج ومستقبل نورة (الصفحة الرئيسية)" />
                </h3>
                <p class="text-muted small mb-0">
                    إدارة العنوان الرئيسي والبطاقات والروابط التفاعلية بقسم مستقبل نورة في موقع إدارة المحتوى (<span class="badge bg-light text-dark border">/ar/ContentAdmin</span>).
                </p>
            </div>
            <asp:PlaceHolder ID="phHeaderActions" runat="server" Visible="false">
            <div class="d-flex gap-2">
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-outline-secondary btn-sm rounded-3 px-3" OnClick="btnRefresh_Click">
                    <i class="bi bi-arrow-clockwise me-1"></i> تحديث
                </asp:LinkButton>
                <asp:LinkButton ID="btnProvision" runat="server" CssClass="btn btn-outline-warning btn-sm rounded-3 px-3" OnClick="btnProvision_Click"
                                OnClientClick="return confirm('سيتم إنشاء القوائم والحقول الناقصة. لا تُعبأ البيانات الافتراضية إلا في القوائم المنشأة حديثًا، ولن تتغير البيانات الحالية. هل تريد المتابعة؟');">
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

            <!-- 1. Section Title Settings Card -->
            <div class="card mb-4 shadow-sm border-0 rounded-4">
                <div class="card-header bg-white py-3 border-bottom d-flex justify-content-between align-items-center">
                    <h5 class="mb-0 fw-bold text-dark">
                        <i class="bi bi-type-h1 text-primary me-2"></i> إعدادات عنوان القسم (Home Nora Future Title)
                    </h5>
                    <span class="badge bg-secondary-subtle text-secondary small">القائمة: HomeNoraFutureTitle</span>
                </div>
                <div class="card-body p-4">
                    <div class="row g-3">
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">عنوان القسم (العربية - AR): <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtTitleAr" runat="server" CssClass="form-control" Placeholder="مثال: الدراسة في جامعة الأميرة نورة" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">عنوان القسم (بالإنجليزية - EN):</label>
                            <asp:TextBox ID="txtTitleEn" runat="server" CssClass="form-control" Placeholder="e.g. Study at Princess Nourah University" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">تسمية إمكانية الوصول / الوصف الصوتي (Aria Label - AR):</label>
                            <asp:TextBox ID="txtAriaLabelAr" runat="server" CssClass="form-control" Placeholder="مثال: برامج ومستقبل نورة" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">تسمية إمكانية الوصول (Aria Label - EN):</label>
                            <asp:TextBox ID="txtAriaLabelEn" runat="server" CssClass="form-control" Placeholder="e.g. PNU Programs & Future" />
                        </div>
                        <div class="col-12 d-flex justify-content-between align-items-center pt-2">
                            <div class="form-check form-switch">
                                <asp:CheckBox ID="chkShowSection" runat="server" Checked="true" />
                                <label class="form-check-label fw-semibold ms-2" for="<%= chkShowSection.ClientID %>">إظهار هذا القسم في الصفحة الرئيسية</label>
                            </div>
                            <asp:Button ID="btnSaveTitle" runat="server" Text="حفظ إعدادات العنوان" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveTitle_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- 2. Add / Edit Card Modal/Panel -->
            <asp:Panel ID="pnlCardForm" runat="server" Visible="false" CssClass="card mb-4 shadow-sm border-primary rounded-4">
                <div class="card-header bg-primary text-white py-3 rounded-top-4 d-flex justify-content-between align-items-center">
                    <h5 class="mb-0 fw-bold">
                        <i class="bi bi-pencil-square me-2"></i>
                        <asp:Literal ID="ltCardFormTitle" runat="server" Text="إضافة بطاقة جديدة" />
                    </h5>
                    <asp:LinkButton ID="btnCloseForm" runat="server" CssClass="btn-close btn-close-white" OnClick="btnCancelCard_Click" aria-label="Close"></asp:LinkButton>
                </div>
                <div class="card-body p-4">
                    <asp:HiddenField ID="hfCardId" runat="server" Value="0" />
                    <div class="row g-3">
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">عنوان البطاقة (العربية - AR): <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtCardTitleAr" runat="server" CssClass="form-control" Placeholder="مثال: برامج البكالوريوس" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">عنوان البطاقة (بالإنجليزية - EN): <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtCardTitleEn" runat="server" CssClass="form-control" Placeholder="e.g. Undergraduate Programs" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">الوصف / عدد البرامج (العربية - AR):</label>
                            <asp:TextBox ID="txtCardDescAr" runat="server" CssClass="form-control" Placeholder="مثال: أكثر من 40 برنامجاً أكاديمياً" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">الوصف / عدد البرامج (بالإنجليزية - EN):</label>
                            <asp:TextBox ID="txtCardDescEn" runat="server" CssClass="form-control" Placeholder="e.g. More than 40 academic programs" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">رابط الصفحة (العربية - AR): <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtCardLinkAr" runat="server" CssClass="form-control" Placeholder="مثال: /ar/RegAdm/RegBSC" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-semibold">رابط الصفحة (بالإنجليزية - EN):</label>
                            <asp:TextBox ID="txtCardLinkEn" runat="server" CssClass="form-control" Placeholder="e.g. /en/RegAdm/RegBSC" />
                        </div>

                        <div class="col-12 col-md-8">
                            <label class="form-label fw-semibold">فئة الأيقونة (HugeIcon / Bootstrap): <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtCardIconClass" runat="server" CssClass="form-control" Placeholder="مثال: hgi-book-01 أو hgi-diploma" />
                            <div class="form-text small mt-1">
                                أيقونات سريعة شائعة (انقر للاختيار):
                                <span class="badge bg-light text-primary border cursor-pointer me-1" onclick="selectIcon('hgi-book-01')">hgi-book-01</span>
                                <span class="badge bg-light text-primary border cursor-pointer me-1" onclick="selectIcon('hgi-book-02')">hgi-book-02</span>
                                <span class="badge bg-light text-primary border cursor-pointer me-1" onclick="selectIcon('hgi-diploma')">hgi-diploma</span>
                                <span class="badge bg-light text-primary border cursor-pointer me-1" onclick="selectIcon('hgi-school')">hgi-school</span>
                                <span class="badge bg-light text-primary border cursor-pointer me-1" onclick="selectIcon('hgi-certificate-01')">hgi-certificate-01</span>
                                <span class="badge bg-light text-primary border cursor-pointer me-1" onclick="selectIcon('hgi-award')">hgi-award</span>
                            </div>
                        </div>

                        <div class="col-12 col-md-2">
                            <label class="form-label fw-semibold">ترتيب العرض:</label>
                            <asp:TextBox ID="txtCardOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                        </div>

                        <div class="col-12 col-md-2 d-flex align-items-end">
                            <div class="form-check form-switch mb-2">
                                <asp:CheckBox ID="chkCardIsActive" runat="server" Checked="true" />
                                <label class="form-check-label fw-semibold ms-2" for="<%= chkCardIsActive.ClientID %>">مفعل</label>
                            </div>
                        </div>

                        <div class="col-12 d-flex justify-content-end gap-2 pt-3 border-top">
                            <asp:Button ID="btnCancelCard" runat="server" Text="إلغاء" CssClass="btn btn-outline-secondary px-4 rounded-3" OnClick="btnCancelCard_Click" CausesValidation="false" />
                            <asp:Button ID="btnSaveCard" runat="server" Text="حفظ البطاقة" CssClass="btn btn-primary px-4 rounded-3 fw-semibold" OnClick="btnSaveCard_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- 3. Program Cards Grid / Table -->
            <div class="card shadow-sm border-0 rounded-4">
                <div class="card-header bg-white py-3 border-bottom d-flex flex-wrap justify-content-between align-items-center gap-2">
                    <div class="d-flex align-items-center gap-2">
                        <h5 class="mb-0 fw-bold text-dark">
                            <i class="bi bi-collection text-primary me-2"></i> بطاقات البرامج المعروضة (Cards List)
                        </h5>
                        <asp:Label ID="lblCardCount" runat="server" CssClass="badge bg-primary-subtle text-primary rounded-pill px-3 py-2 fw-semibold" />
                    </div>
                    <div class="d-flex gap-2">
                        <asp:LinkButton ID="btnNewCard" runat="server" CssClass="btn btn-primary btn-sm rounded-3 px-3 py-2 fw-semibold" OnClick="btnNewCard_Click">
                            <i class="bi bi-plus-lg me-1"></i> إضافة بطاقة جديدة
                        </asp:LinkButton>
                    </div>
                </div>

                <!-- Search Bar -->
                <div class="card-body border-bottom bg-light p-3">
                    <div class="row g-2 align-items-center">
                        <div class="col-12 col-md-6 col-lg-4">
                            <div class="input-group input-group-sm">
                                <span class="input-group-text bg-white border-end-0"><i class="bi bi-search text-muted"></i></span>
                                <asp:TextBox ID="txtSearchCards" runat="server" CssClass="form-control border-start-0" Placeholder="بحث في العنوان أو الوصف أو الرابط..." />
                            </div>
                        </div>
                        <div class="col-12 col-md-6 col-lg-3 d-flex gap-2">
                            <asp:Button ID="btnSearchCards" runat="server" Text="بحث" CssClass="btn btn-secondary btn-sm px-3 rounded-3" OnClick="btnSearchCards_Click" />
                            <asp:Button ID="btnResetSearch" runat="server" Text="إلغاء التصفية" CssClass="btn btn-outline-secondary btn-sm px-3 rounded-3" OnClick="btnResetSearch_Click" />
                        </div>
                    </div>
                </div>

                <!-- Table Content -->
                <div class="table-responsive">
                    <asp:GridView ID="gvCards" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                  CssClass="table table-hover align-middle mb-0" GridLines="None"
                                  OnRowCommand="gvCards_RowCommand" OnRowDataBound="gvCards_RowDataBound">
                        <HeaderStyle CssClass="table-light text-secondary small text-nowrap" />
                        <Columns>
                            <asp:BoundField DataField="ItemOrder" HeaderText="الترتيب" ItemStyle-Width="70px" ItemStyle-CssClass="text-center fw-bold text-muted" HeaderStyle-CssClass="text-center" />

                            <asp:TemplateField HeaderText="الأيقونة" ItemStyle-Width="90px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <div class="d-inline-flex p-2 bg-light rounded-3 border text-primary fs-5">
                                        <i class='<%# FormatIconClass(Eval("IconClass")) %>'></i>
                                    </div>
                                    <div class="small text-muted" style="font-size:0.75rem;"><%# Enc(Eval("IconClass")) %></div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="عنوان البطاقة (AR / EN)">
                                <ItemTemplate>
                                    <div class="fw-bold text-dark"><%# Enc(Eval("Title")) %></div>
                                    <div class="text-muted small"><%# Enc(Eval("Title_EN")) %></div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الوصف / العدد (AR / EN)">
                                <ItemTemplate>
                                    <div class="text-secondary small"><%# Enc(Eval("Description")) %></div>
                                    <div class="text-muted small" style="font-size:0.8rem;"><%# Enc(Eval("Description_EN")) %></div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الرابط (Link)">
                                <ItemTemplate>
                                    <a href='<%# SafeHref(Eval("LinkUrl")) %>' target="_blank" rel="noopener" class="text-decoration-none small text-truncate d-inline-block" style="max-width: 200px;" title='<%# Enc(Eval("LinkUrl")) %>'>
                                        <i class="bi bi-box-arrow-up-right me-1"></i><%# Enc(Eval("LinkUrl")) %>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الحالة" ItemStyle-Width="100px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnToggleStatus" runat="server" CommandName="ToggleActive" CommandArgument='<%# Eval("ID") %>' CssClass="text-decoration-none">
                                        <span class='badge <%# Convert.ToBoolean(Eval("IsActive")) ? "bg-success-subtle text-success border border-success-subtle" : "bg-danger-subtle text-danger border border-danger-subtle" %> px-2 py-1 rounded-pill'>
                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "<i class='bi bi-check-circle me-1'></i>مفعل" : "<i class='bi bi-x-circle me-1'></i>غير مفعل" %>
                                        </span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الإجراءات" ItemStyle-Width="160px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <div class="btn-group btn-group-sm" role="group">
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditCard" CommandArgument='<%# Eval("ID") %>'
                                                        CssClass="btn btn-outline-primary" ToolTip="تعديل">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M17 3a2.85 2.85 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/></svg><span class="visually-hidden">تعديل</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteCard" CommandArgument='<%# Eval("ID") %>'
                                                        CssClass="btn btn-outline-danger" ToolTip="حذف"
                                                        OnClientClick="return confirm('هل أنت متأكد من حذف هذه البطاقة نهائياً؟ / Are you sure you want to delete this card?');">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-3px" aria-hidden="true" focusable="false"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/><path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><path d="M10 11v6"/><path d="M14 11v6"/></svg><span class="visually-hidden">حذف</span>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center py-5 text-muted">
                                <i class="bi bi-inbox fs-1 d-block mb-2 text-secondary"></i>
                                <h6>لا توجد بطاقات برامج مسجلة حتى الآن.</h6>
                                <p class="small mb-3">يمكنك إضافة بطاقة جديدة أو النقر على "تهيئة القوائم" لملء البيانات الافتراضية.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>

        </asp:Panel>

    </div>
</section>

<script type="text/javascript">
    function selectIcon(iconName) {
        var txt = document.getElementById('<%= txtCardIconClass.ClientID %>');
        if (txt) {
            txt.value = iconName;
        }
    }
</script>
