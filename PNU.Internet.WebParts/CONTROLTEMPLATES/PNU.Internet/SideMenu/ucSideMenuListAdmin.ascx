<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSideMenuListAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu.ucSideMenuListAdmin" %>


<div class="side-menu-admin-container p-4 bg-white border rounded shadow-sm" dir="rtl">
    <div class="d-flex align-items-center justify-content-between mb-4 border-bottom pb-3">
        <h2 class="h4 mb-0 text-primary font-weight-bold">
            <i class="hgi hgi-stroke hgi-menu-02 me-2"></i>إدارة القائمة الجانبية (Side Menu Admin)
        </h2>
    </div>

    <!-- Message Alert Banner -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
        <asp:Literal ID="litAlertMessage" runat="server" />
    </asp:Panel>

    <!-- Detailed Execution Summary Panel (Bilingual: Arabic & English) -->
    <asp:Panel ID="pnlExecutionSummary" runat="server" Visible="false" CssClass="card mb-4 border-success shadow-sm">
        <div class="card-header bg-success text-white font-weight-bold d-flex justify-content-between align-items-center">
            <span>
                <i class="hgi hgi-stroke hgi-check-circle-01 me-2"></i>
                ملخص التنفيذ والتفاصيل (Execution Summary & Created Objects)
            </span>
            <div>
                <asp:Button ID="btnExportSummary" runat="server" Text="📥 تصدير إلى Excel" OnClick="btnExportSummary_Click" CssClass="btn btn-sm btn-warning text-dark font-weight-bold me-2" CausesValidation="false" />
                <asp:Button ID="btnCloseSummary" runat="server" Text="إغلاق ✕" OnClick="btnCloseSummary_Click" CssClass="btn btn-sm btn-light text-dark font-weight-bold" CausesValidation="false" />
            </div>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <label for="<%= txtPagesSummary.ClientID %>" class="form-label font-weight-bold text-dark mb-2">
                    📄 روابط الصفحات المنشأة (Created Pages & Links):
                </label>
                <asp:TextBox ID="txtPagesSummary" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control font-monospace p-3" style="direction: ltr; text-align: left; background-color: #f8f9fa; font-size: 13px; line-height: 1.6; border: 1px solid #ced4da;" ReadOnly="true" />
            </div>
            <asp:Literal ID="litExecutionSummaryContent" runat="server" />
        </div>
    </asp:Panel>

    <!-- 1. Target Site Selector Panel -->
    <div class="card mb-4 border-primary">
        <div class="card-header bg-primary text-white font-weight-bold d-flex justify-content-between align-items-center">
            <span>1. تحديد موقع الاستهداف (Target Site Selection)</span>
        </div>
        <div class="card-body">
            <div class="row align-items-center mb-3">
                <div class="col-md-7 mb-2">
                    <label for="<%= txtWebSiteURL.ClientID %>" class="form-label font-weight-bold">رابط الموقع (Web Site URL):</label>
                    <asp:TextBox ID="txtWebSiteURL" runat="server" CssClass="form-control" placeholder="https://..." />
                </div>
                <div class="col-md-3 mb-2">
                    <label for="<%= ddlSubwebs.ClientID %>" class="form-label font-weight-bold">اختر من المواقع الفرعية:</label>
                    <asp:DropDownList ID="ddlSubwebs" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSubwebs_SelectedIndexChanged" />
                </div>
                <div class="col-md-2 mb-2 d-flex align-items-end">
                    <asp:Button ID="btnLoadSite" runat="server" Text="فحص / تحميل" OnClick="btnLoadSite_Click" CssClass="btn btn-primary w-100 mt-4" />
                </div>
            </div>

            <!-- Multi-Site Selection List Section -->
            <div class="p-3 bg-light rounded border mb-2">
                <div class="d-flex justify-content-between align-items-center mb-2">
                    <label class="form-label font-weight-bold text-dark mb-0">
                        <i class="hgi hgi-stroke hgi-folder-02 me-1"></i>قائمة المواقع والمواقع الفرعية للتطبيق المتعدد (Multiple Target Sites Selection):
                    </label>
                    <div>
                        <asp:Button ID="btnSelectAllSubwebs" runat="server" Text="تحديد الكل" OnClick="btnSelectAllSubwebs_Click" CssClass="btn btn-sm btn-outline-primary me-1" CausesValidation="false" />
                        <asp:Button ID="btnDeselectAllSubwebs" runat="server" Text="إلغاء تحديد الكل" OnClick="btnDeselectAllSubwebs_Click" CssClass="btn btn-sm btn-outline-secondary" CausesValidation="false" />
                    </div>
                </div>
                <small class="text-muted d-block mb-2">
                    عند تحديد عدة مواقع من القائمة، سيتم تطبيق جميع العمليات (إنشاء القوائم، التعبئة الافتراضية للكليات/الوكالات/العمادات/الإدارات، إلخ) على كافة المواقع المحددة.
                </small>
                <div style="max-height: 180px; overflow-y: auto; background: #fff; padding: 10px; border: 1px solid #dee2e6; border-radius: 4px;">
                    <asp:CheckBoxList ID="cblSubwebs" runat="server" CssClass="form-check-group" RepeatLayout="UnorderedList" />
                </div>
            </div>

            <div class="mt-2 text-muted small">
                <asp:Label ID="lblCurrentLoadedSite" runat="server" Text="" />
            </div>
        </div>
    </div>

    <!-- 2. Provisioning & Status Panel -->
    <asp:Panel ID="pnlProvisioning" runat="server" CssClass="card mb-4 border-info">
        <div class="card-header bg-info text-white font-weight-bold">
            2. حالة القوائم والإعداد التلقائي (Lists Status & Provisioning)
        </div>
        <div class="card-body">
            <div class="mb-3">
                <asp:Label ID="lblListsStatus" runat="server" CssClass="font-weight-bold" />
            </div>
            <div class="d-flex flex-wrap gap-2">
                <asp:Button ID="btnCreateLists" runat="server" Text="إنشاء القوائم (Ensure Lists)" OnClick="btnCreateLists_Click" CssClass="btn btn-outline-primary me-2 mb-2" />
                <asp:Button ID="btnSeedCollege" runat="server" Text="تعبئة القائمة الافتراضية للكليات (Seed College)" OnClick="btnSeedCollege_Click" CssClass="btn btn-outline-success me-2 mb-2" />
                <asp:Button ID="btnSeedAgency" runat="server" Text="تعبئة القائمة الافتراضية للوكالات (Seed Agency)" OnClick="btnSeedAgency_Click" CssClass="btn btn-outline-info me-2 mb-2" />
                <asp:Button ID="btnSeedDeenships" runat="server" Text="تعبئة القائمة الافتراضية للعمادات (Seed Deenships)" OnClick="btnSeedDeenships_Click" CssClass="btn btn-outline-info me-2 mb-2" />
                <asp:Button ID="btnSeedDepartments" runat="server" Text="تعبئة القائمة الافتراضية للإدارات (Seed Departments)" OnClick="btnSeedDepartments_Click" CssClass="btn btn-outline-info me-2 mb-2" />
                <asp:Button ID="btnSeedCenters" runat="server" Text="تعبئة القائمة الافتراضية للمراكز (Seed Centers)" OnClick="btnSeedCenters_Click" CssClass="btn btn-outline-info me-2 mb-2" />
                <asp:Button ID="btnClearLists" runat="server" Text="مسح القوائم الحالية (Clear Menu Items)" OnClick="btnClearLists_Click" CssClass="btn btn-outline-danger mb-2" OnClientClick="return confirm('هل أنت تأكد من مسح جميع عناصر القائمة؟');" />
            </div>
        </div>
    </asp:Panel>

    <!-- 3. SubMenu Level 1 Management Panel -->
    <asp:Panel ID="pnlMainManagement" runat="server" Visible="false">
        <div class="card mb-4 border-secondary">
            <div class="card-header bg-secondary text-white font-weight-bold">
                3. عناصر المستوى الأول (SubMenu Level 1)
            </div>
            <div class="card-body">
                

                <!-- Form Add / Edit Level 1 -->
                <div class="p-3 bg-light rounded mb-3 border">
                    <h6 class="font-weight-bold text-dark mb-3">
                        <asp:Literal ID="litL1FormTitle" runat="server" Text="إضافة عنصر مستوى أول جديد (Add SubMenu Level 1)" />
                    </h6>
                    <asp:HiddenField ID="hfL1EditID" runat="server" Value="0" />
                    <div class="row">
                        <div class="col-md-3 mb-2">
                            <label class="form-label">العنوان بالعربية *:</label>
                            <asp:TextBox ID="txtL1TitleAr" runat="server" CssClass="form-control" placeholder="الرئيسية..." />
                        </div>
                        <div class="col-md-3 mb-2">
                            <label class="form-label">العنوان بالإنجليزية:</label>
                            <asp:TextBox ID="txtL1TitleEn" runat="server" CssClass="form-control" placeholder="Main..." />
                        </div>
                        <div class="col-md-3 mb-2">
                            <label class="form-label">الرابط (URL):</label>
                            <asp:TextBox ID="txtL1Url" runat="server" CssClass="form-control" placeholder="/ar/Faculties/..." />
                        </div>
                        <div class="col-md-2 mb-2">
                            <label class="form-label">الترتيب (Order):</label>
                            <asp:TextBox ID="txtL1Order" runat="server" CssClass="form-control" Text="1" TextMode="Number" />
                        </div>
                        <div class="col-md-1 mb-2 d-flex align-items-center">
                            <div class="form-check mt-3">
                                <asp:CheckBox ID="chkL1Visibility" runat="server" Checked="true" Text="ظاهر" CssClass="form-check-input" />
                            </div>
                        </div>
                    </div>

                    <div class="mt-3">
                        <asp:Button ID="btnAddLevel1" runat="server" Text="حفظ العنصر والصفحات" OnClick="btnAddLevel1_Click" CssClass="btn btn-success me-2" />
                        <asp:Button ID="btnUpdateLevel1" runat="server" Text="تحديث العنصر" OnClick="btnUpdateLevel1_Click" CssClass="btn btn-warning me-2" Visible="false" />
                        <asp:Button ID="btnCancelL1Edit" runat="server" Text="إلغاء" OnClick="btnCancelL1Edit_Click" CssClass="btn btn-secondary" Visible="false" />
                    </div>
                </div>

                <!-- GridView Level 1 Items -->
                <div class="table-responsive">
                    <asp:GridView ID="gvLevel1Items" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped align-middle"
                        OnRowCommand="gvLevel1Items_RowCommand" DataKeyNames="ID">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="Title" HeaderText="العنوان بالعربية" />
                            <asp:BoundField DataField="Title_EN" HeaderText="العنوان بالإنجليزية" />
                            <asp:BoundField DataField="URL" HeaderText="الرابط (URL)" />
                            <asp:BoundField DataField="ItemOrder" HeaderText="الترتيب" ItemStyle-Width="70px" />
                            <asp:CheckBoxField DataField="Visibility" HeaderText="ظاهر" ItemStyle-Width="60px" />
                            <asp:TemplateField HeaderText="الإجراءات" ItemStyle-Width="180px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSelect" runat="server" CommandName="SelectParent" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-info me-1" Text="تحديد الأبناء" />
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditL1" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-warning me-1" Text="تعديل" />
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteL1" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-danger" Text="حذف" OnClientClick="return confirm('هل تأكد من حذف هذا العنصر؟');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- 4. SubMenu Level 2 Management Panel -->
        <div class="card mb-4 border-dark">
            <div class="card-header bg-dark text-white font-weight-bold d-flex justify-content-between align-items-center">
                <span>4. عناصر المستوى الثاني (SubMenu Level 2)</span>
                <span class="badge bg-primary fs-6">
                    <asp:Literal ID="litSelectedParentBadge" runat="server" Text="يرجى تحديد عنصر المستوى الأول" />
                </span>
            </div>
            <div class="card-body">
                <!-- Form Add / Edit Level 2 -->
                <div class="p-3 bg-light rounded mb-3 border">
                    <h6 class="font-weight-bold text-dark mb-3">
                        <asp:Literal ID="litL2FormTitle" runat="server" Text="إضافة عنصر مستوى ثاني جديد (Add SubMenu Level 2)" />
                    </h6>
                    <asp:HiddenField ID="hfL2EditID" runat="server" Value="0" />
                    <div class="row">
                        <div class="col-md-3 mb-2">
                            <label class="form-label">العنصر الأب (Parent Level 1) *:</label>
                            <asp:DropDownList ID="ddlL2Parent" runat="server" CssClass="form-select" />
                        </div>
                        <div class="col-md-3 mb-2">
                            <label class="form-label">العنوان بالعربية *:</label>
                            <asp:TextBox ID="txtL2TitleAr" runat="server" CssClass="form-control" placeholder="الهيكل التنظيمي..." />
                        </div>
                        <div class="col-md-3 mb-2">
                            <label class="form-label">العنوان بالإنجليزية:</label>
                            <asp:TextBox ID="txtL2TitleEn" runat="server" CssClass="form-control" placeholder="Hierarchy..." />
                        </div>
                        <div class="col-md-3 mb-2">
                            <label class="form-label">الرابط (URL):</label>
                            <asp:TextBox ID="txtL2Url" runat="server" CssClass="form-control" placeholder="/ar/Faculties/Pages/..." />
                        </div>
                    </div>
                    <div class="row align-items-center">
                        <div class="col-md-2 mb-2">
                            <label class="form-label">الترتيب (Order):</label>
                            <asp:TextBox ID="txtL2Order" runat="server" CssClass="form-control" Text="1" TextMode="Number" />
                        </div>
                        <div class="col-md-2 mb-2">
                            <div class="form-check mt-4">
                                <asp:CheckBox ID="chkL2Visibility" runat="server" Checked="true" Text="ظاهر" CssClass="form-check-input" />
                            </div>
                        </div>
                    </div>

                    <div class="mt-3">
                        <asp:Button ID="btnAddLevel2" runat="server" Text="حفظ العنصر والصفحات" OnClick="btnAddLevel2_Click" CssClass="btn btn-success me-2" />
                        <asp:Button ID="btnUpdateLevel2" runat="server" Text="تحديث العنصر" OnClick="btnUpdateLevel2_Click" CssClass="btn btn-warning me-2" Visible="false" />
                        <asp:Button ID="btnCancelL2Edit" runat="server" Text="إلغاء" OnClick="btnCancelL2Edit_Click" CssClass="btn btn-secondary" Visible="false" />
                    </div>
                </div>
                <div class="row mb-3">
    <div class="col-md-6">
        <label for="<%= ddlSubMenuLevel1.ClientID %>" class="form-label font-weight-bold">اختر عنصر المستوى الأول لعرض أبنائه:</label>
        <asp:DropDownList ID="ddlSubMenuLevel1" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSubMenuLevel1_SelectedIndexChanged" />
    </div>
</div>
                <!-- GridView Level 2 Items -->
                <div class="table-responsive">
                    <asp:GridView ID="gvLevel2Items" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped align-middle"
                        OnRowCommand="gvLevel2Items_RowCommand" OnRowDataBound="gvLevel2Items_RowDataBound" DataKeyNames="ID">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="Title" HeaderText="العنوان بالعربية" />
                            <asp:BoundField DataField="Title_EN" HeaderText="العنوان بالإنجليزية" />
                            <asp:BoundField DataField="URL" HeaderText="الرابط (URL)" />
                            <asp:BoundField DataField="ItemOrder" HeaderText="الترتيب" ItemStyle-Width="70px" />
                            <asp:CheckBoxField DataField="Visibility" HeaderText="ظاهر" ItemStyle-Width="60px" />
                            <asp:TemplateField HeaderText="حالة الصفحة" ItemStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Literal ID="litPageStatusBadge" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="الإجراءات" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnCreatePageNow" runat="server" CommandName="CreatePageRow" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-outline-success me-1" Text="إنشاء الصفحة" ToolTip="إنشاء صفحة النشر المرتبطة" />
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditL2" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-warning me-1" Text="تعديل" />
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteL2" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-danger" Text="حذف" OnClientClick="return confirm('هل تأكد من حذف هذا العنصر؟');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- 5. Publishing Page Creation Settings Panel -->
        <div class="card mb-4 border-warning">
            <div class="card-header bg-warning text-dark font-weight-bold d-flex justify-content-between align-items-center">
                <span>5. إنشاء وتجهيز صفحات النشر (Publishing Page & UserControl Provisioning)</span>
                <i class="hgi hgi-stroke hgi-file-02"></i>
            </div>
            <div class="card-body">
                <div class="d-flex align-items-center mb-3">
                    <asp:CheckBox ID="chkCreatePage" runat="server" Checked="true" />
                    <label for="<%= chkCreatePage.ClientID %>" class="form-check-label font-weight-bold ms-2 me-2">تفعيل إنشاء الصفحة تلقائياً عند إضافة عنصر جديد بالقائمة</label>
                </div>

                <div class="row">
                    <div class="col-md-6 mb-3">
                        <label for="<%= ddlControlPresets.ClientID %>" class="form-label font-weight-bold">القوالب الجاهزة لعناصر التحكم (Preset User Controls):</label>
                        <asp:DropDownList ID="ddlControlPresets" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlControlPresets_SelectedIndexChanged" />
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="<%= txtPageName.ClientID %>" class="form-label font-weight-bold">اسم ملف الصفحة (Page Name):</label>
                        <asp:TextBox ID="txtPageName" runat="server" CssClass="form-control" placeholder="NewPage.aspx" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 mb-3">
                        <label for="<%= txtStandaloneTitleAr.ClientID %>" class="form-label font-weight-bold">عنوان الصفحة بالعربية (Title AR):</label>
                        <asp:TextBox ID="txtStandaloneTitleAr" runat="server" CssClass="form-control" placeholder="عنوان الصفحة..." />
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="<%= txtStandaloneTitleEn.ClientID %>" class="form-label font-weight-bold">عنوان الصفحة بالإنجليزية (Title EN):</label>
                        <asp:TextBox ID="txtStandaloneTitleEn" runat="server" CssClass="form-control" placeholder="Page Title..." />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4 mb-3">
                        <label for="<%= txtPageLayoutUrl.ClientID %>" class="form-label font-weight-bold">مسار قوالب الصفحات (Page Layout URL):</label>
                        <asp:TextBox ID="txtPageLayoutUrl" runat="server" CssClass="form-control" Text="/_catalogs/masterpage/NewSideMenu.aspx" />
                    </div>
                    <div class="col-md-4 mb-3">
                        <label for="<%= txtUserControlPath.ClientID %>" class="form-label font-weight-bold">مسار عنصر التحكم (User Control Path):</label>
                        <asp:TextBox ID="txtUserControlPath" runat="server" CssClass="form-control" placeholder="PNU.Internet/Colleges/DGA/ucCollegeContactDga.ascx" />
                    </div>
                    <div class="col-md-4 mb-3">
                        <label for="<%= txtUserControlProperties.ClientID %>" class="form-label font-weight-bold">خصائص عنصر التحكم (User Control Properties):</label>
                        <asp:TextBox ID="txtUserControlProperties" runat="server" CssClass="form-control" placeholder="ListName#AgencyAchievements" />
                    </div>
                </div>

                <!-- Explicit Create Page Button -->
                <div class="mt-3 pt-3 border-top d-flex justify-content-end">
                    <asp:Button ID="btnCreateStandalonePage" runat="server" Text="⚡ إنشاء الصفحة الآن (Create Page Now)" OnClick="btnCreateStandalonePage_Click" CssClass="btn btn-warning btn-lg font-weight-bold px-4 shadow-sm text-dark" />
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
