<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgCategoryAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Admin.ucPgCategoryAdmin" %>

<div class="container-fluid py-4" dir="rtl" style="font-family: 'IBM Plex Sans Arabic', sans-serif;">
    <div class="card shadow-sm border-0 rounded-4">
        <!-- Card Header -->
        <div class="card-header bg-white border-bottom py-3 px-4 d-flex flex-wrap align-items-center justify-content-between gap-3">
            <div class="d-flex align-items-center gap-3">
                <div class="p-2 rounded-3 text-white" style="background-color: #006848;">
                    <i class="hgi hgi-stroke hgi-settings-02 fs-4"></i>
                </div>
                <div>
                    <h1 class="h4 fw-bold text-dark mb-0">لوحة إدارة برامج وتفاصيل الدراسات العليا</h1>
                    <p class="text-muted small mb-0">إدارة متكاملة لبيانات البرامج، الترويسات، الشروط، الخطط الدراسية، والفرص المهنية</p>
                </div>
            </div>

            <!-- Toolbar Buttons -->
            <div class="d-flex flex-wrap align-items-center gap-2">
                <asp:Button ID="btnOpenAddProgram" runat="server" Text="+ إضافة برنامج جديد" CssClass="btn text-white fw-medium px-3" Style="background-color: #006848; border-radius: 8px;" OnClick="btnOpenAddProgram_Click" />
                <asp:Button ID="btnOpenEditOverview" runat="server" Text="تعديل ترويسة الفئة والبانر" CssClass="btn btn-outline-success fw-medium px-3" Style="border-radius: 8px;" OnClick="btnOpenEditOverview_Click" />
                <asp:Button ID="btnReseedCategory" runat="server" Text="إعادة تعبئة هذه الفئة" CssClass="btn btn-outline-secondary fw-medium" Style="border-radius: 8px;" OnClick="btnReseedCategory_Click" OnClientClick="return confirm('هل أنت متأكد من رغبتك في إعادة تعبئة برامج هذه الفئة بالبيانات الافتراضية؟ سيتم تحديث برامج هذه الفئة.');" />
                <asp:Button ID="btnReseedAll" runat="server" Text="إعادة تعبئة جميع الفئات والتفاصيل" CssClass="btn btn-outline-danger fw-medium" Style="border-radius: 8px;" OnClick="btnReseedAll_Click" OnClientClick="return confirm('تحذير: سيتم حذف وإعادة تعبئة كافة البرامج والتفاصيل والترويسات لجميع الفئات بالبيانات الأصلية المحدثة. هل تود المتابعة؟');" />
            </div>
        </div>

        <div class="card-body p-4">
            <!-- Alert Message -->
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show mb-4" role="alert">
                <asp:Literal ID="litAlertMsg" runat="server" />
            </asp:Panel>

            <!-- Filters Bar -->
            <div class="row g-3 align-items-center mb-4 bg-light p-3 rounded-3 border">
                <div class="col-12 col-md-4">
                    <label class="form-label small fw-bold text-secondary mb-1">تصفية حسب الفئة:</label>
                    <asp:DropDownList ID="ddlCategoryFilter" runat="server" AutoPostBack="true" CssClass="form-select" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
                        <asp:ListItem Text="جميع الفئات" Value="All" />
                        <asp:ListItem Text="كوادر نورة" Value="NouraStaff" Selected="True" />
                        <asp:ListItem Text="البرامج الإنسانية" Value="Humanitarian" />
                        <asp:ListItem Text="البرامج البينية" Value="Interdisciplinary" />
                        <asp:ListItem Text="البرامج العلمية" Value="Scientific" />
                        <asp:ListItem Text="البرامج الصحية" Value="Health" />
                    </asp:DropDownList>
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label small fw-bold text-secondary mb-1">البحث السريع:</label>
                    <asp:TextBox ID="txtSearchKeyword" runat="server" CssClass="form-control" placeholder="اسم البرنامج أو الكلية أو القسم أو الرمز..." AutoPostBack="true" OnTextChanged="txtSearchKeyword_TextChanged" />
                </div>
                <div class="col-12 col-md-4 text-md-end pt-md-3">
                    <span class="badge bg-secondary px-3 py-2 fs-6">
                        إجمالي البرامج المعروضة: <asp:Literal ID="litProgramCount" runat="server" Text="0" />
                    </span>
                </div>
            </div>

            <!-- Program Quick Form Panel (Add / Basic Edit) -->
            <asp:Panel ID="pnlProgramForm" runat="server" Visible="false" CssClass="card border-primary border-2 mb-4 shadow-sm">
                <div class="card-header bg-primary text-white py-2 px-3 d-flex justify-content-between align-items-center">
                    <h2 class="h6 mb-0 fw-bold"><asp:Literal ID="litFormTitle" runat="server" Text="إضافة / تعديل برنامج" /></h2>
                    <asp:Button ID="btnCloseProgramForm" runat="server" Text="✕" CssClass="btn btn-sm btn-outline-light py-0 px-2" OnClick="btnCancelProgramForm_Click" />
                </div>
                <div class="card-body p-4">
                    <asp:HiddenField ID="hfProgramId" runat="server" Value="0" />
                    <div class="row g-3">
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">الفئة <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlFormCategory" runat="server" CssClass="form-select">
                                <asp:ListItem Text="كوادر نورة" Value="NouraStaff" />
                                <asp:ListItem Text="البرامج الإنسانية" Value="Humanitarian" />
                                <asp:ListItem Text="البرامج البينية" Value="Interdisciplinary" />
                                <asp:ListItem Text="البرامج العلمية" Value="Scientific" />
                                <asp:ListItem Text="البرامج الصحية" Value="Health" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">مجموعة الدرجة العلمية (مفتاح) <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlFormDegreeGroupKey" runat="server" CssClass="form-select">
                                <asp:ListItem Text="برامج الدكتوراه (PhD)" Value="PhD" />
                                <asp:ListItem Text="برامج الماجستير الأكاديمي (AcademicMaster)" Value="AcademicMaster" />
                                <asp:ListItem Text="برامج الماجستير المهني 1 (ProfessionalMaster1)" Value="ProfessionalMaster1" />
                                <asp:ListItem Text="برامج الماجستير المهني 2 (ProfessionalMaster2)" Value="ProfessionalMaster2" />
                                <asp:ListItem Text="برامج الدبلوم العالي (HigherDiploma)" Value="HigherDiploma" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">عنوان مجموعة الدرجة (بالعربي)</label>
                            <asp:TextBox ID="txtFormDegreeGroupTitleAr" runat="server" CssClass="form-control" placeholder="مثال: برامج الماجستير المهني 1" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">اسم البرنامج (بالعربي) <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtFormTitleAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">اسم البرنامج (بالإنجليزية)</label>
                            <asp:TextBox ID="txtFormTitleEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">الكلية (بالعربي)</label>
                            <asp:TextBox ID="txtFormCollegeAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">الكلية (بالإنجليزية)</label>
                            <asp:TextBox ID="txtFormCollegeEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">القسم (بالعربي)</label>
                            <asp:TextBox ID="txtFormDepartmentAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">القسم (بالإنجليزية)</label>
                            <asp:TextBox ID="txtFormDepartmentEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">النطاق / محلي أو عالمي</label>
                            <asp:DropDownList ID="ddlFormScope" runat="server" CssClass="form-select">
                                <asp:ListItem Text="محلي" Value="محلي" />
                                <asp:ListItem Text="عالمي" Value="عالمي" />
                                <asp:ListItem Text="عالمي (شراكة دولية)" Value="عالمي (شراكة دولية)" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">رابط التفاصيل (أو رمز الصفحة مثل pro55.aspx)</label>
                            <asp:TextBox ID="txtFormDetailsUrl" runat="server" CssClass="form-control" placeholder="/ar/RegAdm/PGD/Pages/pro55.aspx" dir="ltr" />
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">رابط التقديم على بوابة القبول</label>
                            <asp:TextBox ID="txtFormApplyUrl" runat="server" CssClass="form-control" placeholder="https://banssb.pnu.edu.sa/..." dir="ltr" />
                        </div>

                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">الترتيب</label>
                            <asp:TextBox ID="txtFormItemOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                        </div>
                        <div class="col-12 col-md-4 d-flex align-items-center pt-4">
                            <div class="form-check form-switch">
                                <asp:CheckBox ID="chkFormIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label fw-bold small me-2" for="<%= chkFormIsActive.ClientID %>">مفعّل ويظهر للزوار</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-footer bg-light py-3 px-4 d-flex justify-content-end gap-2">
                    <asp:Button ID="btnCancelProgramForm" runat="server" Text="إلغاء" CssClass="btn btn-secondary px-4" OnClick="btnCancelProgramForm_Click" />
                    <asp:Button ID="btnSaveProgram" runat="server" Text="حفظ البرنامج" CssClass="btn text-white px-4" Style="background-color: #006848;" OnClick="btnSaveProgram_Click" />
                </div>
            </asp:Panel>

            <!-- ========================================================================= -->
            <!-- 🌟 Comprehensive Program Details Full CRUD Form Panel 🌟 -->
            <!-- ========================================================================= -->
            <asp:Panel ID="pnlProgramDetailsForm" runat="server" Visible="false" CssClass="card border-success border-2 mb-4 shadow-lg">
                <div class="card-header text-white py-3 px-4 d-flex justify-content-between align-items-center" style="background: linear-gradient(135deg, #006848 0%, #0c4d36 100%);">
                    <div class="d-flex align-items-center gap-2">
                        <i class="hgi hgi-stroke hgi-document-attachment fs-4 text-warning"></i>
                        <h2 class="h5 mb-0 fw-bold"><asp:Literal ID="litDetailsFormTitle" runat="server" Text="تعديل تفاصيل البرنامج الكاملة (الخطة، الشروط، الرسالة، الإحصائيات)" /></h2>
                    </div>
                    <asp:Button ID="btnCloseDetailsForm" runat="server" Text="✕ إغلاق" CssClass="btn btn-sm btn-outline-light py-1 px-3" OnClick="btnCancelProgramDetails_Click" />
                </div>
                <div class="card-body p-4 bg-white">
                    <asp:HiddenField ID="hfDetailsId" runat="server" Value="0" />

                    <!-- Section 1: Basic Identifiers & Badges -->
                    <div class="border rounded-3 p-3 mb-4 bg-light">
                        <h3 class="h6 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-mortarboard-02"></i>
                            <span>1. البيانات الأساسية والشارات الأكاديمية</span>
                        </h3>
                        <div class="row g-3">
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">رمز البرنامج (Program Code / ASPX Page Key) <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDetProgramCode" runat="server" CssClass="form-control fw-bold" placeholder="مثال: pro55" dir="ltr" />
                                <span class="text-muted small">يستخدم للربط مع الصفحة pro55.aspx</span>
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">الفئة الأكاديمية</label>
                                <asp:DropDownList ID="ddlDetCategory" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="كوادر نورة" Value="NouraStaff" />
                                    <asp:ListItem Text="البرامج الإنسانية" Value="Humanitarian" />
                                    <asp:ListItem Text="البرامج البينية" Value="Interdisciplinary" />
                                    <asp:ListItem Text="البرامج العلمية" Value="Scientific" />
                                    <asp:ListItem Text="البرامج الصحية" Value="Health" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">الدرجة العلمية (Ar)</label>
                                <asp:TextBox ID="txtDetDegreeAr" runat="server" CssClass="form-control" placeholder="ماجستير العلوم في..." />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">الدرجة العلمية (En)</label>
                                <asp:TextBox ID="txtDetDegreeEn" runat="server" CssClass="form-control" placeholder="Master of Science in..." dir="ltr" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">اسم البرنامج الكامل (Ar) <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDetTitleAr" runat="server" CssClass="form-control fw-bold" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">اسم البرنامج الكامل (En)</label>
                                <asp:TextBox ID="txtDetTitleEn" runat="server" CssClass="form-control" dir="ltr" />
                            </div>

                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">الكلية (Ar)</label>
                                <asp:TextBox ID="txtDetCollegeAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">الكلية (En)</label>
                                <asp:TextBox ID="txtDetCollegeEn" runat="server" CssClass="form-control" dir="ltr" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">القسم (Ar)</label>
                                <asp:TextBox ID="txtDetDepartmentAr" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">القسم (En)</label>
                                <asp:TextBox ID="txtDetDepartmentEn" runat="server" CssClass="form-control" dir="ltr" />
                            </div>

                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">النطاق الأكاديمي</label>
                                <asp:DropDownList ID="ddlDetScope" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="محلي" Value="محلي" />
                                    <asp:ListItem Text="عالمي" Value="عالمي" />
                                    <asp:ListItem Text="عالمي (شراكة دولية)" Value="عالمي (شراكة دولية)" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 col-md-4 d-flex align-items-center pt-4">
                                <div class="form-check form-switch">
                                    <asp:CheckBox ID="chkDetIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label fw-bold small me-2" for="<%= chkDetIsActive.ClientID %>">مفعّل ويظهر في صفحة التفاصيل</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Section 2: Quick Meta Stats Bar -->
                    <div class="border rounded-3 p-3 mb-4 bg-light">
                        <h3 class="h6 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-clock-01"></i>
                            <span>2. شريط الإحصائيات السريعة (Quick Meta Stats)</span>
                        </h3>
                        <div class="row g-3">
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">الساعات المعتمدة (Ar)</label>
                                <asp:TextBox ID="txtDetCreditHoursAr" runat="server" CssClass="form-control" placeholder="مثال: 36 ساعة" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">الساعات المعتمدة (En)</label>
                                <asp:TextBox ID="txtDetCreditHoursEn" runat="server" CssClass="form-control" placeholder="e.g. 36 Credits" dir="ltr" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">المدة الزمنية (Ar)</label>
                                <asp:TextBox ID="txtDetDurationAr" runat="server" CssClass="form-control" placeholder="مثال: سنتان (4 فصول)" />
                            </div>

                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">المدة الزمنية (En)</label>
                                <asp:TextBox ID="txtDetDurationEn" runat="server" CssClass="form-control" placeholder="e.g. 2 Years (4 Semesters)" dir="ltr" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">لغة التدريس (Ar)</label>
                                <asp:TextBox ID="txtDetLanguageAr" runat="server" CssClass="form-control" placeholder="مثال: اللغة العربية / اللغة الإنجليزية" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">لغة التدريس (En)</label>
                                <asp:TextBox ID="txtDetLanguageEn" runat="server" CssClass="form-control" placeholder="e.g. English / Arabic" dir="ltr" />
                            </div>

                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">نظام الدراسة (Ar)</label>
                                <asp:TextBox ID="txtDetStudySystemAr" runat="server" CssClass="form-control" placeholder="مقررات ورسالة / مشروع بحثي" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">نظام الدراسة (En)</label>
                                <asp:TextBox ID="txtDetStudySystemEn" runat="server" CssClass="form-control" placeholder="Courses & Thesis" dir="ltr" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">الرسوم الدراسية (Ar)</label>
                                <asp:TextBox ID="txtDetTuitionFeesAr" runat="server" CssClass="form-control" placeholder="مجاني / 18,000 ريال" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-bold small">الرسوم الدراسية (En)</label>
                                <asp:TextBox ID="txtDetTuitionFeesEn" runat="server" CssClass="form-control" placeholder="Free / Standard Tuition" dir="ltr" />
                            </div>
                        </div>
                    </div>

                    <!-- Section 3: Overview, Mission & Objectives -->
                    <div class="border rounded-3 p-3 mb-4 bg-light">
                        <h3 class="h6 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-compass-02"></i>
                            <span>3. النبذة التعريفية والرسالة والأهداف</span>
                        </h3>
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">النبذة التعريفية للبرنامج (Ar)</label>
                                <asp:TextBox ID="txtDetOverviewAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">النبذة التعريفية للبرنامج (En)</label>
                                <asp:TextBox ID="txtDetOverviewEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" dir="ltr" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">رسالة البرنامج (Ar)</label>
                                <asp:TextBox ID="txtDetMissionAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">رسالة البرنامج (En)</label>
                                <asp:TextBox ID="txtDetMissionEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" dir="ltr" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">أهداف البرنامج (Ar - تقبل فقرات أو نقاط HTML)</label>
                                <asp:TextBox ID="txtDetObjectivesAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">أهداف البرنامج (En)</label>
                                <asp:TextBox ID="txtDetObjectivesEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" dir="ltr" />
                            </div>
                        </div>
                    </div>

                    <!-- Section 4: Admission Requirements -->
                    <div class="border rounded-3 p-3 mb-4 bg-light">
                        <h3 class="h6 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-clipboard-check"></i>
                            <span>4. شروط ومتطلبات القبول (الشروط العامة والخاصة والمقابلات)</span>
                        </h3>
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">شروط ومتطلبات القبول (Ar - تقبل نصوص أو نقاط HTML)</label>
                                <asp:TextBox ID="txtDetAdmissionRequirementsAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">شروط ومتطلبات القبول (En)</label>
                                <asp:TextBox ID="txtDetAdmissionRequirementsEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" dir="ltr" />
                            </div>
                        </div>
                    </div>

                    <!-- Section 5: Study Plan & PDF -->
                    <div class="border rounded-3 p-3 mb-4 bg-light">
                        <h3 class="h6 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-book-open-02"></i>
                            <span>5. الخطة الدراسية وتوزيع المقررات</span>
                        </h3>
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">الخطة الدراسية (Ar - تقبل جداول أو نقاط HTML)</label>
                                <asp:TextBox ID="txtDetStudyPlanAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">الخطة الدراسية (En)</label>
                                <asp:TextBox ID="txtDetStudyPlanEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" dir="ltr" />
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-bold small">رابط ملف الخطة الدراسية (PDF)</label>
                                <asp:TextBox ID="txtDetStudyPlanPdfUrl" runat="server" CssClass="form-control" placeholder="https://.../StudyPlan.pdf" dir="ltr" />
                            </div>
                        </div>
                    </div>

                    <!-- Section 6: Careers & Prospects -->
                    <div class="border rounded-3 p-3 mb-4 bg-light">
                        <h3 class="h6 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-briefcase-02"></i>
                            <span>6. مجالات العمل والفرص المهنية للخريجات</span>
                        </h3>
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">مجالات العمل والفرص الوظيفية (Ar)</label>
                                <asp:TextBox ID="txtDetCareerOpportunitiesAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">مجالات العمل والفرص الوظيفية (En)</label>
                                <asp:TextBox ID="txtDetCareerOpportunitiesEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" dir="ltr" />
                            </div>
                        </div>
                    </div>

                    <!-- Section 7: Admission Portal Link & International Partnership -->
                    <div class="border rounded-3 p-3 mb-4 bg-light">
                        <h3 class="h6 fw-bold mb-3 d-flex align-items-center gap-2" style="color: #006848;">
                            <i class="hgi hgi-stroke hgi-agreement-01"></i>
                            <span>7. بوابة القبول الموحدة (Banner SIS) والشراكة الدولية</span>
                        </h3>
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">رابط التقديم المباشر على بوابة القبول (Banner SIS)</label>
                                <asp:TextBox ID="txtDetApplyUrl" runat="server" CssClass="form-control" placeholder="https://banner-ssb.pnu.edu.sa/..." dir="ltr" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-bold small">البريد الإلكتروني للتواصل مع البرنامج</label>
                                <asp:TextBox ID="txtDetContactEmail" runat="server" CssClass="form-control" placeholder="dps-admissions@pnu.edu.sa" dir="ltr" />
                            </div>

                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">اسم الجامعة الشريكة المعتمدة (Ar)</label>
                                <asp:TextBox ID="txtDetPartnerUnivAr" runat="server" CssClass="form-control" placeholder="مثال: جامعة مدينة دبلن (Dublin City University)" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">اسم الجامعة الشريكة (En)</label>
                                <asp:TextBox ID="txtDetPartnerUnivEn" runat="server" CssClass="form-control" placeholder="Dublin City University" dir="ltr" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-bold small">رابط موقع الجامعة الشريكة</label>
                                <asp:TextBox ID="txtDetPartnerUnivUrl" runat="server" CssClass="form-control" placeholder="https://www.dcu.ie/..." dir="ltr" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="card-footer bg-light py-3 px-4 d-flex justify-content-end gap-2">
                    <asp:Button ID="btnCancelProgramDetails" runat="server" Text="إلغاء" CssClass="btn btn-secondary px-4" OnClick="btnCancelProgramDetails_Click" />
                    <asp:Button ID="btnSaveProgramDetails" runat="server" Text="💾 حفظ كافة تفاصيل البرنامج" CssClass="btn text-white px-4 fw-medium" Style="background-color: #006848;" OnClick="btnSaveProgramDetails_Click" />
                </div>
            </asp:Panel>

            <!-- Category Overview Form Panel -->
            <asp:Panel ID="pnlOverviewForm" runat="server" Visible="false" CssClass="card border-success border-2 mb-4 shadow-sm">
                <div class="card-header bg-success text-white py-2 px-3 d-flex justify-content-between align-items-center">
                    <h2 class="h6 mb-0 fw-bold">تعديل محتوى الترويسة والبانر والروابط السريعة للفئة</h2>
                    <asp:Button ID="btnCloseOverviewForm" runat="server" Text="✕" CssClass="btn btn-sm btn-outline-light py-0 px-2" OnClick="btnCancelOverviewForm_Click" />
                </div>
                <div class="card-body p-4">
                    <asp:HiddenField ID="hfOverviewId" runat="server" Value="0" />
                    <div class="row g-3">
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">الفئة المستهدفة</label>
                            <asp:DropDownList ID="ddlOverviewCategory" runat="server" AutoPostBack="true" CssClass="form-select" OnSelectedIndexChanged="ddlOverviewCategory_SelectedIndexChanged">
                                <asp:ListItem Text="كوادر نورة" Value="NouraStaff" />
                                <asp:ListItem Text="البرامج الإنسانية" Value="Humanitarian" />
                                <asp:ListItem Text="البرامج البينية" Value="Interdisciplinary" />
                                <asp:ListItem Text="البرامج العلمية" Value="Scientific" />
                                <asp:ListItem Text="البرامج الصحية" Value="Health" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-md-3">
                            <label class="form-label fw-bold small">شارة الفئة (Ar)</label>
                            <asp:TextBox ID="txtHeroTagAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-3">
                            <label class="form-label fw-bold small">شارة الفئة (En)</label>
                            <asp:TextBox ID="txtHeroTagEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">عنوان الترويسة الرئيسي (Ar)</label>
                            <asp:TextBox ID="txtHeroTitleAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">عنوان الترويسة الرئيسي (En)</label>
                            <asp:TextBox ID="txtHeroTitleEn" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">الوصف / الفقرة التعريفية (Ar)</label>
                            <asp:TextBox ID="txtHeroDescAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">الوصف / الفقرة التعريفية (En)</label>
                            <asp:TextBox ID="txtHeroDescEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" dir="ltr" />
                        </div>

                        <!-- Banner Settings -->
                        <div class="col-12"><hr class="my-2" /><h3 class="h6 fw-bold text-dark mb-0">إعدادات بانر القبول (Banner SIS)</h3></div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">عنوان البانر (Ar)</label>
                            <asp:TextBox ID="txtBannerTitleAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">وصف البانر (Ar)</label>
                            <asp:TextBox ID="txtBannerDescAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">نص زر البانر (Ar)</label>
                            <asp:TextBox ID="txtBannerBtnAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label class="form-label fw-bold small">رابط البانر</label>
                            <asp:TextBox ID="txtBannerUrl" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <!-- Quick Cards -->
                        <div class="col-12"><hr class="my-2" /><h3 class="h6 fw-bold text-dark mb-0">البطاقات السريعة السفلية</h3></div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">عنوان البطاقة 1</label>
                            <asp:TextBox ID="txtCard1TitleAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">وصف البطاقة 1</label>
                            <asp:TextBox ID="txtCard1DescAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">رابط البطاقة 1</label>
                            <asp:TextBox ID="txtCard1Url" runat="server" CssClass="form-control" dir="ltr" />
                        </div>

                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">عنوان البطاقة 2</label>
                            <asp:TextBox ID="txtCard2TitleAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">وصف البطاقة 2</label>
                            <asp:TextBox ID="txtCard2DescAr" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-4">
                            <label class="form-label fw-bold small">رابط البطاقة 2</label>
                            <asp:TextBox ID="txtCard2Url" runat="server" CssClass="form-control" dir="ltr" />
                        </div>
                    </div>
                </div>
                <div class="card-footer bg-light py-3 px-4 d-flex justify-content-end gap-2">
                    <asp:Button ID="btnCancelOverviewForm" runat="server" Text="إلغاء" CssClass="btn btn-secondary px-4" OnClick="btnCancelOverviewForm_Click" />
                    <asp:Button ID="btnSaveOverview" runat="server" Text="حفظ إعدادات الفئة" CssClass="btn btn-success px-4" OnClick="btnSaveOverview_Click" />
                </div>
            </asp:Panel>

            <!-- Programs Data Table -->
            <div class="table-responsive rounded-3 border">
                <table class="table table-hover align-middle mb-0 text-start">
                    <thead class="table-light">
                        <tr>
                            <th style="width: 50px;">#</th>
                            <th style="width: 120px;">الفئة</th>
                            <th>اسم البرنامج</th>
                            <th>الدرجة العلمية</th>
                            <th>الكلية / القسم</th>
                            <th style="width: 100px;" class="text-center">النطاق</th>
                            <th style="width: 80px;" class="text-center">الحالة</th>
                            <th style="width: 180px;" class="text-center">الإجراءات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptAdminPrograms" runat="server" OnItemCommand="rptAdminPrograms_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td class="text-muted small"><%# Eval("ItemOrder") %></td>
                                    <td>
                                        <span class="badge bg-light text-dark border">
                                            <%# PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgCategoryConstants.GetCategoryDisplayNameAr(PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgCategoryConstants.ParseCategory(Eval("CategoryKey") as string)) %>
                                        </span>
                                    </td>
                                    <td>
                                        <span class="fw-bold d-block text-dark"><%# Eval("TitleAr") %></span>
                                        <span class="text-muted small d-block" dir="ltr"><%# Eval("TitleEn") %></span>
                                    </td>
                                    <td>
                                        <span class="small text-secondary"><%# Eval("DegreeGroupTitleAr") %></span>
                                    </td>
                                    <td>
                                        <span class="small d-block text-dark"><%# Eval("CollegeAr") %></span>
                                        <span class="small d-block text-muted"><%# Eval("DepartmentAr") %></span>
                                    </td>
                                    <td class="text-center">
                                        <span class='badge <%# ((bool)Eval("IsInternational")) ? "bg-info-subtle text-primary" : "bg-light text-muted border" %>'>
                                            <%# Eval("ScopeAr") %>
                                        </span>
                                    </td>
                                    <td class="text-center">
                                        <span class='badge <%# ((bool)Eval("IsActive")) ? "bg-success-subtle text-success" : "bg-danger-subtle text-danger" %>'>
                                            <%# ((bool)Eval("IsActive")) ? "مفعّل" : "معطّل" %>
                                        </span>
                                    </td>
                                    <td class="text-center">
                                        <div class="d-flex justify-content-center gap-1">
                                            <asp:LinkButton ID="btnEditDetails" runat="server" CommandName="EditDetails" CommandArgument='<%# Eval("DetailsUrl") + "|" + Eval("TitleAr") %>' CssClass="btn btn-sm btn-outline-success py-1 px-2" ToolTip="تعديل التفاصيل الكاملة والشروط والخطة">
                                                <i class="hgi hgi-stroke hgi-document-attachment"></i> تفاصيل
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditProgram" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-primary py-1 px-2" ToolTip="تعديل سريع">
                                                <i class="hgi hgi-stroke hgi-edit-02"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteProgram" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-danger py-1 px-2" ToolTip="حذف" OnClientClick="return confirm('هل أنت متأكد من حذف هذا البرنامج؟');">
                                                <i class="hgi hgi-stroke hgi-delete-02"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

        </div>
    </div>
</div>
