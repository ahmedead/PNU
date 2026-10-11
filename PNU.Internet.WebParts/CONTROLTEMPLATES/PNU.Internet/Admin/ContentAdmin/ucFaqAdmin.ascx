<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFaqAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ContentAdmin.ucFaqAdmin, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-4 faq-admin-control">
    <div class="container-fluid">

        <!-- Page Header -->
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 pb-3 border-bottom">
            <div>
                <h3 class="fw-bold mb-1 text-primary">
                    <i class="bi bi-question-square-fill me-2"></i>
                    <asp:Literal ID="ltPageTitle" runat="server" Text="إدارة الأسئلة الشائعة" />
                </h3>
                <p class="text-muted small mb-0">إدارة قوائم التصنيفات والأسئلة وفقرات الإجابات الخاصة بصفحة الأسئلة الشائعة (FAQ)</p>
            </div>
            <div class="d-flex gap-2">
                <asp:LinkButton ID="btnRefreshHeader" runat="server" CssClass="btn btn-outline-secondary btn-sm rounded-3 px-3" OnClick="btnRefresh_Click">
                    <i class="bi bi-arrow-clockwise me-1"></i> تحديث
                </asp:LinkButton>
            </div>
        </div>

        <!-- Access Denied Panel -->
        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-warning shadow-sm border-0 rounded-4 p-4 text-center my-4">
                <i class="bi bi-shield-lock-fill text-warning fs-1 d-block mb-2"></i>
                <h4 class="fw-bold mb-2">تنبيه الصلاحيات</h4>
                <asp:Literal ID="ltDenied" runat="server" Text="ليس لديك صلاحية لإدارة محتوى الأسئلة الشائعة." />
            </div>
        </asp:Panel>

        <!-- Main Admin Panel -->
        <asp:Panel ID="pnlMain" runat="server">

            <!-- Alerts -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div class="alert alert-success alert-dismissible fade show shadow-sm border-0 rounded-3" role="alert">
                    <i class="bi bi-check-circle-fill me-2"></i>
                    <asp:Literal ID="ltMessage" runat="server" />
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger alert-dismissible fade show shadow-sm border-0 rounded-3" role="alert">
                    <i class="bi bi-exclamation-triangle-fill me-2"></i>
                    <asp:Literal ID="ltError" runat="server" />
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            </asp:Panel>

            <!-- Navigation Tabs & Management Bar -->
            <div class="card mb-4 shadow-sm border-0 rounded-4">
                <div class="card-body p-3">
                    <div class="row g-3 align-items-center">
                        <!-- List Selector Dropdown/Tabs -->
                        <div class="col-12 col-md-5">
                            <label class="form-label fw-bold text-secondary mb-1">
                                <i class="bi bi-layers me-1"></i> اختر قائمة FAQ المراد إدارتها:
                            </label>
                            <asp:DropDownList ID="ddlList" runat="server" CssClass="form-select form-select-lg rounded-3 fw-bold border-primary"
                                              AutoPostBack="true" OnSelectedIndexChanged="ddlList_SelectedIndexChanged">
                                <asp:ListItem Text="1. تصنيفات الأسئلة (FAQCategories)" Value="FAQCategories" />
                                <asp:ListItem Text="2. أسئلة FAQ (FAQQuestions)" Value="FAQQuestions" Selected="True" />
                                <asp:ListItem Text="3. فقرات وإجابات FAQ (FAQAnswerItems)" Value="FAQAnswerItems" />
                            </asp:DropDownList>
                        </div>

                        <!-- Action Buttons -->
                        <div class="col-12 col-md-7 d-flex justify-content-md-end gap-2 flex-wrap align-items-end">
                            <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary rounded-3 px-3 py-2 fw-semibold" OnClick="btnNew_Click">
                                <i class="bi bi-plus-lg me-1"></i> إضافة عنصر جديد
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnSeed" runat="server" CssClass="btn btn-outline-warning rounded-3 px-3 py-2" OnClick="btnSeed_Click"
                                            OnClientClick="return confirm('سيتم إنشاء قوائم وحقول FAQ الناقصة، وتعبئة القوائم المنشأة حديثًا فقط بالبيانات الافتراضية. لن تتغير البيانات الحالية. هل تريد المتابعة؟ / Missing FAQ lists and fields will be created and only newly created lists are seeded. Existing data is not changed. Continue?');">
                                <i class="bi bi-arrow-repeat me-1"></i> التحقق من القوائم وتهيئتها
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Filter Bar for Questions / Answers -->
            <asp:Panel ID="pnlFilterBar" runat="server" CssClass="card mb-4 shadow-sm border-0 rounded-4 bg-light">
                <div class="card-body p-3">
                    <div class="row g-3 align-items-center">
                        <div class="col-12 col-md-4">
                            <div class="input-group">
                                <span class="input-group-text bg-white border-end-0"><i class="bi bi-search text-muted"></i></span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control border-start-0" Placeholder="بحث في العنوان أو النص..." />
                            </div>
                        </div>
                        <asp:PlaceHolder ID="phCategoryFilter" runat="server">
                            <div class="col-12 col-md-4">
                                <asp:DropDownList ID="ddlFilterCategory" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged" />
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phQuestionFilter" runat="server" Visible="false">
                            <div class="col-12 col-md-4">
                                <asp:DropDownList ID="ddlFilterQuestion" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterQuestion_SelectedIndexChanged" />
                            </div>
                        </asp:PlaceHolder>
                        <div class="col-12 col-md-4 d-flex gap-2">
                            <asp:Button ID="btnSearch" runat="server" Text="بحث" CssClass="btn btn-secondary px-3 rounded-3" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnResetFilter" runat="server" Text="إلغاء التصفية" CssClass="btn btn-outline-secondary px-3 rounded-3" OnClick="btnResetFilter_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Dynamic Form Panel (Add / Edit) -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4 shadow-sm border-primary rounded-4">
                <div class="card-header bg-primary text-white py-3 rounded-top-4">
                    <h5 class="mb-0 fw-bold">
                        <i class="bi bi-pencil-square me-2"></i>
                        <asp:Literal ID="ltFormTitle" runat="server" />
                    </h5>
                </div>
                <div class="card-body p-4">

                    <!-- Fields for FAQCategories -->
                    <asp:PlaceHolder ID="phCategoryFields" runat="server" Visible="false">
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">اسم التصنيف (العربية): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCategoryTitle" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">اسم التصنيف (بالإنجليزية - TitleEn):</label>
                                <asp:TextBox ID="txtCategoryTitleEn" runat="server" CssClass="form-control" Placeholder="Category Name in English" />
                            </div>
                            <div class="col-12 col-md-8">
                                <label class="form-label fw-semibold">فئة أيقونة Bootstrap / HugeIcon:</label>
                                <asp:TextBox ID="txtCategoryIconClass" runat="server" CssClass="form-control" Placeholder="e.g. hgi-validation-approval or bi-question-circle" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">ترتيب العرض:</label>
                                <asp:TextBox ID="txtCategorySortOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <!-- Fields for FAQQuestions -->
                    <asp:PlaceHolder ID="phQuestionFields" runat="server" Visible="false">
                        <div class="row g-3">
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان السؤال / السؤال بالعربية: <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtQuestionTitle" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان السؤال (بالإنجليزية - TitleEn):</label>
                                <asp:TextBox ID="txtQuestionTitleEn" runat="server" CssClass="form-control" Placeholder="Question Title in English" />
                            </div>
                            <div class="col-12 col-md-8">
                                <label class="form-label fw-semibold">التصنيف التابع له: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlQuestionCategory" runat="server" CssClass="form-select" />
                            </div>
                            <div class="col-12 col-md-4">
                                <label class="form-label fw-semibold">ترتيب العرض:</label>
                                <asp:TextBox ID="txtQuestionSortOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">نص السؤال التفصيلي (العربية):</label>
                                <asp:TextBox ID="txtQuestionTextAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">نص السؤال التفصيلي (بالإنجليزية - QuestionTextEn):</label>
                                <asp:TextBox ID="txtQuestionTextEn" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" Placeholder="Detailed Question in English" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <!-- Fields for FAQAnswerItems -->
                    <asp:PlaceHolder ID="phAnswerFields" runat="server" Visible="false">
                        <div class="row g-3">
                            <div class="col-12 col-md-7">
                                <label class="form-label fw-semibold">السؤال التابع له الإجابة: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlAnswerQuestion" runat="server" CssClass="form-select" />
                            </div>
                            <div class="col-12 col-md-3">
                                <label class="form-label fw-semibold">نوع فقرة الإجابة: <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlAnswerItemType" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="فقرة نصية (paragraph)" Value="paragraph" Selected="True" />
                                    <asp:ListItem Text="نقطة في قائمة (bullet)" Value="bullet" />
                                    <asp:ListItem Text="مقدمة قائمة (intro)" Value="intro" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 col-md-2">
                                <label class="form-label fw-semibold">ترتيب الفقرة:</label>
                                <asp:TextBox ID="txtAnswerSortOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان مختصر / التسمية (العربية):</label>
                                <asp:TextBox ID="txtAnswerTitle" runat="server" CssClass="form-control" Placeholder="تسمية مختصرة للسطر" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">عنوان مختصر (بالإنجليزية - TitleEn):</label>
                                <asp:TextBox ID="txtAnswerTitleEn" runat="server" CssClass="form-control" Placeholder="Short label in English" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">محتوى الفقرة (العربية - ContentTextAr): <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtAnswerContentTextAr" runat="server" CssClass="form-control font-monospace" TextMode="MultiLine" Rows="4" />
                            </div>
                            <div class="col-12 col-md-6">
                                <label class="form-label fw-semibold">محتوى الفقرة (بالإنجليزية - ContentTextEn):</label>
                                <asp:TextBox ID="txtAnswerContentTextEn" runat="server" CssClass="form-control font-monospace" TextMode="MultiLine" Rows="4" Placeholder="Answer paragraph content in English" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <div class="d-flex gap-2 flex-wrap mt-4 pt-3 border-top">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-4 rounded-3" OnClick="btnSave_Click">
                            <i class="bi bi-check-lg me-1"></i> حفظ البيانات
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary px-4 rounded-3" OnClick="btnCancel_Click" CausesValidation="false">
                            <i class="bi bi-x-lg me-1"></i> إلغاء
                        </asp:LinkButton>
                    </div>

                </div>
            </asp:Panel>

            <!-- Items Data Grid -->
            <div class="card shadow-sm border-0 rounded-4 overflow-hidden">
                <div class="card-header bg-white py-3 border-bottom d-flex justify-content-between align-items-center">
                    <h6 class="fw-bold mb-0 text-secondary">
                        <i class="bi bi-table me-2"></i>
                        سجلات <asp:Literal ID="ltCurrentListNameDisplay" runat="server" />
                    </h6>
                    <span class="badge bg-primary rounded-pill px-3 py-2">
                        العدد الإجمالي: <asp:Literal ID="ltItemCount" runat="server" Text="0" />
                    </span>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light">
                                <tr>
                                    <th scope="col" style="width: 70px;">#</th>
                                    <asp:PlaceHolder ID="phColTitle" runat="server"><th>العنوان / السؤال</th></asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phColCategory" runat="server"><th>التصنيف / السؤال التابع</th></asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phColExtra" runat="server"><th>النوع / الأيقونة / النص</th></asp:PlaceHolder>
                                    <th scope="col" style="width: 100px;">الترتيب</th>
                                    <th scope="col" class="text-end" style="width: 140px;">الإجراءات</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-muted fw-bold"><%# Eval("ID") %></td>
                                            <td class="fw-semibold text-dark"><%# Eval("ColTitle") %></td>
                                            <td><span class="badge bg-light text-dark border"><%# Eval("ColCategory") %></span></td>
                                            <td class="small text-muted"><%# Eval("ColExtra") %></td>
                                            <td><span class="badge bg-secondary"><%# Eval("SortOrder") %></span></td>
                                            <td class="text-end text-nowrap">
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary me-1 rounded-2"
                                                                CommandName="EditItem" CommandArgument='<%# Eval("ID") %>' ToolTip="تعديل">
                                                    <i class="bi bi-pencil"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-outline-danger rounded-2"
                                                                CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>'
                                                                OnClientClick="return confirm('هل أنت متأكد من حذف هذا العنصر نهائيًا؟');" ToolTip="حذف">
                                                    <i class="bi bi-trash"></i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <!-- Empty State -->
                    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="p-5 text-center text-muted">
                        <i class="bi bi-inbox fs-1 d-block mb-3 text-secondary opacity-50"></i>
                        <h5>لا توجد بيانات متاحة حالياً</h5>
                        <p class="small text-muted">يمكنك إضافة عناصر جديدة أو التقرير عن بيانات default عن طريق الزر أعلاه.</p>
                    </asp:Panel>
                </div>
            </div>

            <!-- Hidden Fields -->
            <asp:HiddenField ID="hfEditingId" runat="server" />
            <asp:HiddenField ID="hfCurrentList" runat="server" Value="FAQQuestions" />

        </asp:Panel>

    </div>
</section>
