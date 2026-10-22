<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEservicesAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.Admin.ucEservicesAdmin, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-4 eservices-admin-control">
    <div class="container-fluid px-3 px-md-4">

        <%-- Header Title & Subtitle --%>
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 border-bottom pb-3">
            <div>
                <div class="d-flex align-items-center gap-2">
                    <span class="p-2 rounded-3 bg-primary bg-opacity-10 text-primary d-inline-flex">
                        <i class="bi bi-laptop fs-3" aria-hidden="true"></i>
                    </span>
                    <div>
                        <h2 class="h4 fw-bold mb-0 text-dark">
                            <asp:Literal ID="ltPageTitle" runat="server" Text="إدارة الخدمات الإلكترونية (E-Services)" />
                        </h2>
                        <p class="text-muted small mb-0">
                            <asp:Literal ID="ltPageSubtitle" runat="server" Text="إضافة وتعديل وحذف الخدمات الإلكترونية، وإدارة الروابط واتفاقيات مستوى الخدمة (SLA) العربية والإنجليزية" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="d-flex align-items-center gap-2 flex-wrap">
                <asp:LinkButton ID="btnProvision" runat="server" CssClass="btn btn-sm btn-outline-warning d-inline-flex align-items-center gap-1 rounded-3 px-3 py-2" OnClick="btnProvision_Click"
                                OnClientClick="return confirm('سيتم فحص أعمدة قائمة الخدمات الإلكترونية (بما فيها ServiceAgreementURL_EN) وإضافتها وتحديث طريقة العرض في حال عدم وجودها. هل ترغب بالمتابعة؟');">
                    <i class="bi bi-gear-wide-connected" aria-hidden="true"></i>
                    <span>فحص وتهيئة الحقول</span>
                </asp:LinkButton>
                <asp:LinkButton ID="btnRefreshHeader" runat="server" CssClass="btn btn-sm btn-outline-secondary d-inline-flex align-items-center gap-1 rounded-3 px-3 py-2" OnClick="btnRefresh_Click">
                    <i class="bi bi-arrow-clockwise" aria-hidden="true"></i>
                    <span>تحديث</span>
                </asp:LinkButton>
                <asp:LinkButton ID="btnNewHeader" runat="server" CssClass="btn btn-sm btn-primary d-inline-flex align-items-center gap-1 rounded-3 px-3 py-2" OnClick="btnNew_Click">
                    <i class="bi bi-plus-lg" aria-hidden="true"></i>
                    <span class="fw-semibold">إضافة خدمة جديدة</span>
                </asp:LinkButton>
            </div>
        </div>

        <%-- Access Denied Alert --%>
        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-warning d-flex align-items-center gap-3 p-4 rounded-3 shadow-sm" role="alert">
                <i class="bi bi-shield-lock-fill fs-1 text-warning" aria-hidden="true"></i>
                <div>
                    <h5 class="alert-heading fw-bold mb-1">صلاحية الوصول غير متوفرة</h5>
                    <div>
                        <asp:Literal ID="ltDenied" runat="server" Text="ليس لديك صلاحية لإدارة الخدمات الإلكترونية. يجب أن يكون حسابك مسجلاً في قائمة المشرفين." />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <%-- Main Administration Panel --%>
        <asp:Panel ID="pnlMain" runat="server">

            <%-- Success Banner --%>
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div class="alert alert-success alert-dismissible fade show d-flex align-items-center gap-2 rounded-3 shadow-sm mb-4" role="alert">
                    <i class="bi bi-check-circle-fill fs-5 text-success" aria-hidden="true"></i>
                    <div><asp:Literal ID="ltMessage" runat="server" /></div>
                    <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <%-- Error Banner --%>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger alert-dismissible fade show d-flex align-items-center gap-2 rounded-3 shadow-sm mb-4" role="alert">
                    <i class="bi bi-exclamation-triangle-fill fs-5 text-danger" aria-hidden="true"></i>
                    <div><asp:Literal ID="ltError" runat="server" /></div>
                    <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <%-- Summary Stats Cards --%>
            <div class="row g-3 mb-4">
                <div class="col-12 col-sm-6 col-xl-3">
                    <div class="card border-0 shadow-sm rounded-3 h-100 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-between p-3">
                            <div>
                                <span class="text-muted small d-block">إجمالي الخدمات</span>
                                <h3 class="fw-bold mb-0 text-dark"><asp:Literal ID="ltTotalCount" runat="server" Text="0" /></h3>
                            </div>
                            <span class="p-3 rounded-circle bg-primary bg-opacity-10 text-primary">
                                <i class="bi bi-collection fs-3" aria-hidden="true"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-xl-3">
                    <div class="card border-0 shadow-sm rounded-3 h-100 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-between p-3">
                            <div>
                                <span class="text-muted small d-block">الخدمات النشطة</span>
                                <h3 class="fw-bold mb-0 text-success"><asp:Literal ID="ltActiveCount" runat="server" Text="0" /></h3>
                            </div>
                            <span class="p-3 rounded-circle bg-success bg-opacity-10 text-success">
                                <i class="bi bi-check-circle fs-3" aria-hidden="true"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-xl-3">
                    <div class="card border-0 shadow-sm rounded-3 h-100 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-between p-3">
                            <div>
                                <span class="text-muted small d-block">معروضة بالصفحة الرئيسية</span>
                                <h3 class="fw-bold mb-0 text-info"><asp:Literal ID="ltHomeCount" runat="server" Text="0" /></h3>
                            </div>
                            <span class="p-3 rounded-circle bg-info bg-opacity-10 text-info">
                                <i class="bi bi-house-door fs-3" aria-hidden="true"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-xl-3">
                    <div class="card border-0 shadow-sm rounded-3 h-100 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-between p-3">
                            <div>
                                <span class="text-muted small d-block">اتفاقيات SLA محددة</span>
                                <h3 class="fw-bold mb-0 text-warning"><asp:Literal ID="ltSlaCount" runat="server" Text="0" /></h3>
                            </div>
                            <span class="p-3 rounded-circle bg-warning bg-opacity-10 text-warning">
                                <i class="bi bi-file-earmark-check fs-3" aria-hidden="true"></i>
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Action Toolbar & Filter Card --%>
            <div class="card border-0 shadow-sm rounded-3 mb-4">
                <div class="card-body p-3">
                    <div class="row g-3 align-items-center">
                        <div class="col-12 col-md-3">
                            <div class="input-group input-group-sm">
                                <span class="input-group-text bg-white border-end-0"><i class="bi bi-search text-muted"></i></span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control border-start-0" placeholder="بحث باسم الخدمة أو الوصف..." />
                            </div>
                        </div>
                        <div class="col-12 col-sm-6 col-md-3">
                            <asp:DropDownList ID="ddlFilterTargetGroup" runat="server" CssClass="form-select form-select-sm"
                                              AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged" />
                        </div>
                        <div class="col-12 col-sm-6 col-md-2">
                            <asp:DropDownList ID="ddlFilterActive" runat="server" CssClass="form-select form-select-sm"
                                              AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged">
                                <asp:ListItem Text="جميع الحالات" Value="" Selected="True" />
                                <asp:ListItem Text="نشط فقط" Value="1" />
                                <asp:ListItem Text="غير نشط فقط" Value="0" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-sm-6 col-md-2">
                            <asp:DropDownList ID="ddlFilterHome" runat="server" CssClass="form-select form-select-sm"
                                              AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged">
                                <asp:ListItem Text="عرض الرئيسية: الكل" Value="" Selected="True" />
                                <asp:ListItem Text="الرئيسية فقط" Value="1" />
                                <asp:ListItem Text="غير معروض بالرئيسية" Value="0" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-sm-6 col-md-2 d-flex gap-1 justify-content-end">
                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-sm btn-primary px-3" OnClick="btnSearch_Click">
                                <i class="bi bi-search me-1"></i> تصفية
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnResetFilter" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnResetFilter_Click" ToolTip="إعادة ضبط الفلاتر">
                                <i class="bi bi-x-circle"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Dynamic Add / Edit Service Form --%>
            <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card border-primary border-2 shadow-sm rounded-4 mb-4">
                <div class="card-header bg-primary text-white py-3 px-4 rounded-top-4 d-flex justify-content-between align-items-center">
                    <h5 class="mb-0 fw-bold d-flex align-items-center gap-2">
                        <i class="bi bi-pencil-square" aria-hidden="true"></i>
                        <asp:Literal ID="ltFormTitle" runat="server" Text="إضافة خدمة إلكترونية جديدة" />
                    </h5>
                    <asp:LinkButton ID="btnCloseFormHeader" runat="server" CssClass="btn-close btn-close-white" OnClick="btnCancel_Click" CausesValidation="false" />
                </div>
                <div class="card-body p-4">

                    <!-- Nav Tabs for Organizing Fields -->
                    <ul class="nav nav-pills nav-fill mb-4 bg-light p-2 rounded-3" id="eserviceAdminTabs" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active fw-semibold" id="tab-basic-tab" data-bs-toggle="pill" data-bs-target="#tab-basic" type="button" role="tab" aria-controls="tab-basic" aria-selected="true">
                                <i class="bi bi-info-circle me-1"></i> 1. البيانات الأساسية
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link fw-semibold" id="tab-urls-tab" data-bs-toggle="pill" data-bs-target="#tab-urls" type="button" role="tab" aria-controls="tab-urls" aria-selected="false">
                                <i class="bi bi-link-45deg me-1"></i> 2. الروابط واتفاقية SLA
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link fw-semibold" id="tab-details-tab" data-bs-toggle="pill" data-bs-target="#tab-details" type="button" role="tab" aria-controls="tab-details" aria-selected="false">
                                <i class="bi bi-card-checklist me-1"></i> 3. تصنيف ومواصفات الخدمة
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link fw-semibold" id="tab-steps-tab" data-bs-toggle="pill" data-bs-target="#tab-steps" type="button" role="tab" aria-controls="tab-steps" aria-selected="false">
                                <i class="bi bi-list-ol me-1"></i> 4. الخطوات والمتطلبات والشارات
                            </button>
                        </li>
                    </ul>

                    <div class="tab-content" id="eserviceAdminTabsContent">

                        <!-- TAB 1: Basic Information -->
                        <div class="tab-pane fade show active" id="tab-basic" role="tabpanel" aria-labelledby="tab-basic-tab">
                            <div class="row g-3">
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">اسم الخدمة (بالعربية) <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtARServiceName" runat="server" CssClass="form-control" placeholder="مثال: تقديم طلب التحويل الداخلي" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">اسم الخدمة (بالإنجليزية - ENServiceName)</label>
                                    <asp:TextBox ID="txtENServiceName" runat="server" CssClass="form-control" placeholder="e.g. Internal Transfer Request" />
                                </div>

                                <div class="col-12 col-md-4">
                                    <label class="form-label fw-semibold">العنوان البرمجي / الرمز (Title / Code)</label>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="اختياري - يترك فارغاً لاستخدام الاسم العربي" />
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label fw-semibold">ترتيب العرض (ItemOrder)</label>
                                    <asp:TextBox ID="txtItemOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label fw-semibold d-block">خيارات التفعيل والظهور</label>
                                    <div class="d-flex flex-wrap gap-3 pt-2">
                                        <div class="form-check form-switch">
                                            <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" Checked="true" />
                                            <label class="form-check-label fw-semibold" for="<%= chkActive.ClientID %>">نشط (Active)</label>
                                        </div>
                                        <div class="form-check form-switch">
                                            <asp:CheckBox ID="chkIsHome" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label fw-semibold" for="<%= chkIsHome.ClientID %>">الرئيسية (IsHome)</label>
                                        </div>
                                        <div class="form-check form-switch">
                                            <asp:CheckBox ID="chkNafathLogin" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label fw-semibold" for="<%= chkNafathLogin.ClientID %>">نفاذ (Nafath)</label>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">وصف الخدمة (بالعربية)</label>
                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="وصف موجز يوضح طبيعة وأهداف الخدمة..." />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">وصف الخدمة (بالإنجليزية - Desc_EN)</label>
                                    <asp:TextBox ID="txtDesc_EN" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Brief English description of the service..." />
                                </div>
                            </div>
                        </div>

                        <!-- TAB 2: URLs & Agreements -->
                        <div class="tab-pane fade" id="tab-urls" role="tabpanel" aria-labelledby="tab-urls-tab">
                            <div class="row g-3">
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">رابط بدء الخدمة (العربية - URL)</label>
                                    <asp:TextBox ID="txtURL" runat="server" CssClass="form-control" placeholder="https://..." />
                                    <small class="text-muted">الرابط المستخدم لبدء الخدمة في النسخة العربية.</small>
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">رابط بدء الخدمة (الإنجليزية - URL_EN)</label>
                                    <asp:TextBox ID="txtURL_EN" runat="server" CssClass="form-control" placeholder="https://..." />
                                    <small class="text-muted">الرابط المستخدم في النسخة الإنجليزية؛ إن تُرك فارغاً يُستخدم الرابط العربي.</small>
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold text-primary">اتفاقية مستوى الخدمة - العربية (ServiceAgreement)</label>
                                    <asp:TextBox ID="txtServiceAgreement" runat="server" CssClass="form-control" placeholder="https://pnu.edu.sa/ar/Pages/SLA.aspx" />
                                    <small class="text-muted">رابط صفحة اتفاقية مستوى الخدمة بالعربية.</small>
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold text-success">اتفاقية مستوى الخدمة - الإنجليزية (ServiceAgreementURL_EN)</label>
                                    <asp:TextBox ID="txtServiceAgreementURL_EN" runat="server" CssClass="form-control border-success" placeholder="https://pnu.edu.sa/en/Pages/SLA.aspx" />
                                    <small class="text-success">القيمة الافتراضية: <code>https://pnu.edu.sa/en/Pages/SLA.aspx</code> (في حال كانت فارغة ستسقط إلى الرابط العربي تلقائياً).</small>
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">رابط دليل المستخدم (UserManualURL)</label>
                                    <asp:TextBox ID="txtUserManualURL" runat="server" CssClass="form-control" placeholder="رابط تحميل أو استعراض دليل المستخدم..." />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">رابط اللوائح والأنظمة (RulesAndRegulationsURL)</label>
                                    <asp:TextBox ID="txtRulesAndRegulationsURL" runat="server" CssClass="form-control" placeholder="رابط اللائحة أو النظام المنظم للخدمة..." />
                                </div>
                            </div>
                        </div>

                        <!-- TAB 3: Delivery Details & Specifications -->
                        <div class="tab-pane fade" id="tab-details" role="tabpanel" aria-labelledby="tab-details-tab">
                            <div class="row g-3">
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">الفئة المستهدفة (بالعربية - TargetGroup)</label>
                                    <asp:TextBox ID="txtTargetGroup" runat="server" CssClass="form-control" placeholder="مثال: طالبات / أعضاء هيئة التدريس / موظفون / زائر" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">الفئة المستهدفة (بالإنجليزية - TargetGroup_EN)</label>
                                    <asp:TextBox ID="txtTargetGroup_EN" runat="server" CssClass="form-control" placeholder="e.g. Students / Faculty / Staff / Visitor" />
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">الفئة المستهدفة للعرض (DP_TargetGroup - AR)</label>
                                    <asp:TextBox ID="txtDP_TargetGroup" runat="server" CssClass="form-control" placeholder="نص عرض الفئة في البطاقة الجانبية بالعربية" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">الفئة المستهدفة للعرض (DP_TargetGroup_EN)</label>
                                    <asp:TextBox ID="txtDP_TargetGroup_EN" runat="server" CssClass="form-control" placeholder="Display target audience in English" />
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">الجهة المسؤولة (بالعربية - ResponsibleParty)</label>
                                    <asp:TextBox ID="txtResponsibleParty" runat="server" CssClass="form-control" placeholder="مثال: عمادة القبول والتسجيل" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">الجهة المسؤولة (بالإنجليزية - ResponsibleParty_EN)</label>
                                    <asp:TextBox ID="txtResponsibleParty_EN" runat="server" CssClass="form-control" placeholder="e.g. Deanship of Admission and Registration" />
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">مدة الخدمة (بالعربية - TimeDuration)</label>
                                    <asp:TextBox ID="txtTimeDuration" runat="server" CssClass="form-control" placeholder="مثال: فوري / 3 أيام عمل" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">مدة الخدمة (بالإنجليزية - TimeDuration_EN)</label>
                                    <asp:TextBox ID="txtTimeDuration_EN" runat="server" CssClass="form-control" placeholder="e.g. Instant / 3 Business Days" />
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">قنوات تقديم الخدمة (بالعربية - Channels)</label>
                                    <asp:TextBox ID="txtChannels" runat="server" CssClass="form-control" placeholder="مثال: البوابة الإلكترونية" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">قنوات تقديم الخدمة (بالإنجليزية - Channels_EN)</label>
                                    <asp:TextBox ID="txtChannels_EN" runat="server" CssClass="form-control" placeholder="e.g. Web Portal" />
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">لغة الخدمة (بالعربية - ServiceLanguage)</label>
                                    <asp:TextBox ID="txtServiceLanguage" runat="server" CssClass="form-control" placeholder="مثال: العربية والإنجليزية" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">لغة الخدمة (بالإنجليزية - ServiceLanguage_EN)</label>
                                    <asp:TextBox ID="txtServiceLanguage_EN" runat="server" CssClass="form-control" placeholder="e.g. Arabic & English" />
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">تكلفة الخدمة (بالعربية - ServiceCost)</label>
                                    <asp:TextBox ID="txtServiceCost" runat="server" CssClass="form-control" placeholder="مثال: مجانية" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">تكلفة الخدمة (بالإنجليزية - ServiceCost_EN)</label>
                                    <asp:TextBox ID="txtServiceCost_EN" runat="server" CssClass="form-control" placeholder="e.g. Free" />
                                </div>
                            </div>
                        </div>

                        <!-- TAB 4: Steps, Requirements & Badges -->
                        <div class="tab-pane fade" id="tab-steps" role="tabpanel" aria-labelledby="tab-steps-tab">
                            <div class="row g-3">
                                <!-- Badges Section -->
                                <div class="col-12">
                                    <h6 class="fw-bold text-secondary border-bottom pb-2 mb-3">شارات الخدمة (تظهر في ترويسة تفاصيل الخدمة)</h6>
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label small fw-semibold">الشارة 1 (AR / EN)</label>
                                    <asp:TextBox ID="txtBadge1" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Badge 1 (AR)" />
                                    <asp:TextBox ID="txtBadge1_EN" runat="server" CssClass="form-control form-control-sm" placeholder="Badge 1 (EN)" />
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label small fw-semibold">الشارة 2 (AR / EN)</label>
                                    <asp:TextBox ID="txtBadge2" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Badge 2 (AR)" />
                                    <asp:TextBox ID="txtBadge2_EN" runat="server" CssClass="form-control form-control-sm" placeholder="Badge 2 (EN)" />
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label small fw-semibold">الشارة 3 (AR / EN)</label>
                                    <asp:TextBox ID="txtBadge3" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Badge 3 (AR)" />
                                    <asp:TextBox ID="txtBadge3_EN" runat="server" CssClass="form-control form-control-sm" placeholder="Badge 3 (EN)" />
                                </div>

                                <!-- Requirements Section -->
                                <div class="col-12 mt-4">
                                    <h6 class="fw-bold text-secondary border-bottom pb-2 mb-3">شروط ومتطلبات الخدمة (Req 1, 2, 3)</h6>
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label small fw-semibold">المتطلب 1 (AR / EN)</label>
                                    <asp:TextBox ID="txtReq1" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="المتطلب الأول بالعربية" />
                                    <asp:TextBox ID="txtReq1_EN" runat="server" CssClass="form-control form-control-sm" placeholder="Requirement 1 (EN)" />
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label small fw-semibold">المتطلب 2 (AR / EN)</label>
                                    <asp:TextBox ID="txtReq2" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="المتطلب الثاني بالعربية" />
                                    <asp:TextBox ID="txtReq2_EN" runat="server" CssClass="form-control form-control-sm" placeholder="Requirement 2 (EN)" />
                                </div>
                                <div class="col-12 col-md-4">
                                    <label class="form-label small fw-semibold">المتطلب 3 (AR / EN)</label>
                                    <asp:TextBox ID="txtReq3" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="المتطلب الثالث بالعربية" />
                                    <asp:TextBox ID="txtReq3_EN" runat="server" CssClass="form-control form-control-sm" placeholder="Requirement 3 (EN)" />
                                </div>

                                <!-- Rich Steps & Documents -->
                                <div class="col-12 mt-4">
                                    <h6 class="fw-bold text-secondary border-bottom pb-2 mb-3">خطوات التقديم والمستندات المطلوبة (تدعم وسوم HTML)</h6>
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">خطوات الحصول على الخدمة (Steps - AR)</label>
                                    <asp:TextBox ID="txtSteps" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="خطوات التقديم بالعربية..." />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">خطوات الحصول على الخدمة (Steps_EN)</label>
                                    <asp:TextBox ID="txtSteps_EN" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Steps in English..." />
                                </div>

                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">المستندات المطلوبة (RequiredDocs - AR)</label>
                                    <asp:TextBox ID="txtRequiredDocs" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="المستندات والوثائق المطلوبة بالعربية..." />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label fw-semibold">المستندات المطلوبة (RequiredDocs_EN)</label>
                                    <asp:TextBox ID="txtRequiredDocs_EN" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Required Documents in English..." />
                                </div>
                            </div>
                        </div>

                    </div>

                    <!-- Action Buttons -->
                    <div class="d-flex justify-content-end gap-2 mt-4 pt-3 border-top">
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-outline-secondary px-4 rounded-3" OnClick="btnCancel_Click" CausesValidation="false">
                            <i class="bi bi-x-lg me-1"></i> إلغاء
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary px-4 rounded-3" OnClick="btnSave_Click">
                            <i class="bi bi-check-lg me-1"></i> حفظ بيانات الخدمة
                        </asp:LinkButton>
                    </div>

                </div>
            </asp:Panel>

            <%-- Services List Grid / Table --%>
            <div class="card border-0 shadow-sm rounded-4 mb-4">
                <div class="card-header bg-white py-3 px-4 rounded-top-4 border-bottom d-flex justify-content-between align-items-center">
                    <h5 class="mb-0 fw-bold text-dark d-flex align-items-center gap-2">
                        <i class="bi bi-table text-primary" aria-hidden="true"></i>
                        <span>قائمة الخدمات الإلكترونية</span>
                        <span class="badge bg-secondary ms-2"><asp:Literal ID="ltFilteredCount" runat="server" Text="0" /></span>
                    </h5>
                    <div class="text-muted small">
                        <span>ترتيب العرض الحالي: حسب الترتيب (ItemOrder) ثم اسم الخدمة</span>
                    </div>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light">
                                <tr>
                                    <th scope="col" style="width: 60px;" class="text-center">#</th>
                                    <th scope="col">اسم الخدمة (عربي / إنجليزي)</th>
                                    <th scope="col">الفئة المستهدفة</th>
                                    <th scope="col" class="text-center" style="width: 80px;">الترتيب</th>
                                    <th scope="col" class="text-center" style="width: 90px;">الحالة</th>
                                    <th scope="col" class="text-center" style="width: 90px;">الرئيسية</th>
                                    <th scope="col" class="text-center" style="width: 80px;">نفاذ</th>
                                    <th scope="col">اتفاقية SLA (عربي / إنجليزي)</th>
                                    <th scope="col" class="text-end" style="width: 140px;">الإجراءات</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptServices" runat="server" OnItemCommand="rptServices_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center text-muted fw-bold"><%# Eval("ID") %></td>
                                            <td>
                                                <div class="fw-bold text-dark"><%# Enc(Eval("ARServiceName")) %></div>
                                                <div class="text-muted small"><%# Enc(Eval("ENServiceName")) %></div>
                                                <div class="mt-1 d-flex gap-2 flex-wrap">
                                                    <%# LinkHtml(Eval("URL"), "badge bg-light text-primary border text-decoration-none", "<i class=\"bi bi-box-arrow-up-right me-1\"></i>رابط البدء (AR)", "") %>
                                                    <%# LinkHtml(Eval("URL_EN"), "badge bg-light text-info border text-decoration-none", "<i class=\"bi bi-box-arrow-up-right me-1\"></i>URL (EN)", "") %>
                                                </div>
                                            </td>
                                            <td>
                                                <span class="badge bg-light text-dark border"><%# !string.IsNullOrEmpty(Eval("TargetGroup") as string) ? Enc(Eval("TargetGroup")) : "-" %></span>
                                            </td>
                                            <td class="text-center">
                                                <span class="badge bg-secondary"><%# Eval("ItemOrder") %></span>
                                            </td>
                                            <td class="text-center">
                                                <%# (Eval("Active") != null && (Eval("Active").ToString() == "True" || Eval("Active").ToString() == "1"))
                                                    ? "<span class=\"badge bg-success\"><i class=\"bi bi-check-circle me-1\"></i>نشط</span>"
                                                    : "<span class=\"badge bg-danger\"><i class=\"bi bi-x-circle me-1\"></i>معطل</span>" %>
                                            </td>
                                            <td class="text-center">
                                                <%# (Eval("IsHome") != null && (Eval("IsHome").ToString() == "True" || Eval("IsHome").ToString() == "1"))
                                                    ? "<span class=\"badge bg-info text-dark\"><i class=\"bi bi-check me-1\"></i>نعم</span>"
                                                    : "<span class=\"text-muted small\">-</span>" %>
                                            </td>
                                            <td class="text-center">
                                                <%# (Eval("NafathLogin") != null && (Eval("NafathLogin").ToString() == "True" || Eval("NafathLogin").ToString() == "1"))
                                                    ? "<span class=\"badge bg-warning text-dark\"><i class=\"bi bi-shield-lock me-1\"></i>نعم</span>"
                                                    : "<span class=\"text-muted small\">-</span>" %>
                                            </td>
                                            <td>
                                                <div class="d-flex flex-column gap-1 small">
                                                    <div>
                                                        <span class="text-muted">AR: </span>
                                                        <%# LinkHtml(Eval("ServiceAgreement"), "text-primary text-truncate d-inline-block", null,
                                                                "<span class=\"text-muted fst-italic\">غير محدد</span>", "max-width: 170px;") %>
                                                    </div>
                                                    <div>
                                                        <span class="text-muted">EN: </span>
                                                        <%# LinkHtml(Eval("ServiceAgreementURL_EN"), "text-success text-truncate d-inline-block", null,
                                                                "<span class=\"badge bg-light text-muted border\">يسقط إلى AR</span>", "max-width: 170px;") %>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="text-end text-nowrap">
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary me-1 rounded-2"
                                                                CommandName="EditItem" CommandArgument='<%# Eval("ID") %>' ToolTip="تعديل بيانات الخدمة">
                                                    <i class="bi bi-pencil"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-outline-danger rounded-2"
                                                                CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>'
                                                                OnClientClick="return confirm('هل أنت متأكد من حذف هذه الخدمة نهائياً من قائمة الخدمات؟');" ToolTip="حذف الخدمة">
                                                    <i class="bi bi-trash"></i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <%-- Empty State Panel --%>
                    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="p-5 text-center text-muted">
                        <i class="bi bi-inbox fs-1 d-block mb-3 text-secondary opacity-50"></i>
                        <h5>لا توجد خدمات مطابقة لشروط البحث والتصفية</h5>
                        <p class="small text-muted">يمكنك إضافة خدمة جديدة من الزر أعلاه أو إعادة ضبط الفلاتر.</p>
                    </asp:Panel>
                </div>
            </div>

            <%-- Hidden Fields for Edit State --%>
            <asp:HiddenField ID="hfEditingId" runat="server" />
            <asp:HiddenField ID="hfMode" runat="server" Value="New" />

        </asp:Panel>

    </div>
</section>
