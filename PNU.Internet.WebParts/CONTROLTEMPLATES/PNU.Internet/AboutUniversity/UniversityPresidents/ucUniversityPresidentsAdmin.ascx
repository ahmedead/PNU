<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUniversityPresidentsAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidents.ucUniversityPresidentsAdmin" %>


<h2 id="hMsg" runat="server" style="color:red" class="d-none">ليس لديك صلاحية الوصول لهذه الشاشة</h2>
<div dir="rtl" class="container py-4" id="dvForm" runat="server">

    <style>
        .admin-section {
            border: 1px solid #ddd;
            padding: 20px;
            margin-bottom: 25px;
            border-radius: 8px;
            background: #fafafa;
        }

        .admin-section h3 {
            margin-bottom: 20px;
        }

        .field-row {
            margin-bottom: 15px;
        }

        .field-row label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .form-control {
            width: 100%;
            min-height: 34px;
            padding: 6px 10px;
        }

        .btn {
            padding: 6px 14px;
            margin-top: 10px;
            cursor: pointer;
        }

        .btn-primary {
            background: #0d6efd;
            color: #fff;
            border: none;
        }

        .btn-success {
            background: #198754;
            color: #fff;
            border: none;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }

        .table th, .table td {
            border: 1px solid #ddd;
            padding: 8px;
            vertical-align: top;
        }

        .table th {
            background: #f2f2f2;
        }

        .message {
            display: block;
            margin-bottom: 20px;
            font-weight: bold;
        }
    </style>

    <div class="admin-section">
    <h3>إعدادات الموقع</h3>

    <div class="field-row">
        <label>رابط الموقع (Web URL)</label>
        <asp:TextBox ID="txtWebUrl" runat="server" CssClass="form-control" />
    </div>

    <asp:Button ID="btnLoadWebData" runat="server" Text="تحميل البيانات من الموقع"
        CssClass="btn btn-primary" OnClick="btnLoadWebData_Click" />
</div>


    <div class="admin-section">
    <h3>إنشاء صفحة</h3>

    <div class="field-row">
        <label>اسم الصفحة (بدون .aspx)</label>
        <asp:TextBox ID="txtNewPageName" runat="server" CssClass="form-control" />
    </div>

    <div class="field-row">
        <label>عنوان الصفحة</label>
        <asp:TextBox ID="txtNewPageTitle" runat="server" CssClass="form-control" />
    </div>

    <div class="field-row">
        <label>رابط الـ Page Layout</label>
        <asp:TextBox ID="txtPageLayoutUrl" runat="server" CssClass="form-control"
            Text="/_catalogs/masterpage/UniversityPresidents.aspx" />
    </div>

    <asp:Button ID="btnCreatePage" runat="server" Text="إنشاء الصفحة"
        CssClass="btn btn-primary" OnClick="btnCreatePage_Click" />
</div>



    <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

    <div class="admin-section">
        <h3>البيانات الرئيسية</h3>

        <div class="field-row">
            <label>اسم رئيسة الجامعة</label>
            <asp:TextBox ID="txtPresidentNameAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>المسمى الوظيفي</label>
            <asp:TextBox ID="txtPresidentTitleAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>اسم الجامعة</label>
            <asp:TextBox ID="txtUniversityNameAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>رابط الصورة</label>
            <asp:TextBox ID="txtImageUrl" runat="server" CssClass="form-control" />
        </div>

        <%--<div class="field-row">
            <label>الرابط الخارجي</label>
            <asp:TextBox ID="txtExternalLink" runat="server" CssClass="form-control" />
        </div>--%>

        <div class="field-row">
            <label>عنوان كلمة الرئيسة</label>
            <asp:TextBox ID="txtParagraphTitleAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>نص كلمة الرئيسة</label>
            <asp:TextBox ID="txtParagraphTextAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" />
        </div>

        <div class="field-row">
            <label>عنوان السيرة الذاتية</label>
            <asp:TextBox ID="txtBiographyTitleAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>نص السيرة الذاتية</label>
            <asp:TextBox ID="txtBiographyTextAr" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" />
        </div>

        <asp:Button ID="btnSaveProfile" runat="server" Text="حفظ البيانات الرئيسية"
            CssClass="btn btn-success" OnClick="btnSaveProfile_Click" />
    </div>

    <div class="admin-section">
        <h3>جهات التواصل</h3>

        <div class="field-row">
            <label>التسمية</label>
            <asp:TextBox ID="txtContactLabelAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>القيمة</label>
            <asp:TextBox ID="txtContactValue" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>النوع</label>
            <asp:DropDownList ID="ddlContactType" runat="server" CssClass="form-control">
                <asp:ListItem Text="Email" Value="Email" />
                <asp:ListItem Text="Phone" Value="Phone" />
                <asp:ListItem Text="Link" Value="Link" />
            </asp:DropDownList>
        </div>

        <div class="field-row">
            <label>الترتيب</label>
            <asp:TextBox ID="txtContactSortOrder" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnAddContact" runat="server" Text="إضافة"
            CssClass="btn btn-primary" OnClick="btnAddContact_Click" />

        <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="false" CssClass="table"
            OnRowCommand="gvContacts_RowCommand">
            <Columns>
                <asp:BoundField DataField="ContactLabelAr" HeaderText="التسمية" />
                <asp:BoundField DataField="ContactValue" HeaderText="القيمة" />
                <asp:BoundField DataField="ContactType" HeaderText="النوع" />
                <asp:BoundField DataField="SortOrder" HeaderText="الترتيب" />
                <asp:TemplateField HeaderText="حذف">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDeleteContact" runat="server"
                            CommandName="DeleteContact"
                            CommandArgument="<%# Container.DataItemIndex %>"
                            Text="حذف" ForeColor="Red" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="admin-section">
        <h3>العضويات</h3>

        <div class="field-row">
            <label>النص</label>
            <asp:TextBox ID="txtMembershipTextAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>الترتيب</label>
            <asp:TextBox ID="txtMembershipSortOrder" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnAddMembership" runat="server" Text="إضافة"
            CssClass="btn btn-primary" OnClick="btnAddMembership_Click" />

        <asp:GridView ID="gvMemberships" runat="server" AutoGenerateColumns="false" CssClass="table"
            OnRowCommand="gvMemberships_RowCommand">
            <Columns>
                <asp:BoundField DataField="MembershipTextAr" HeaderText="النص" />
                <asp:BoundField DataField="SortOrder" HeaderText="الترتيب" />
                <asp:TemplateField HeaderText="حذف">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDeleteMembership" runat="server"
                            CommandName="DeleteMembership"
                            CommandArgument="<%# Container.DataItemIndex %>"
                            Text="حذف" ForeColor="Red" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="admin-section">
        <h3>الأوسمة والجوائز</h3>

        <div class="field-row">
            <label>النص</label>
            <asp:TextBox ID="txtAwardTextAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>الترتيب</label>
            <asp:TextBox ID="txtAwardSortOrder" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnAddAward" runat="server" Text="إضافة"
            CssClass="btn btn-primary" OnClick="btnAddAward_Click" />

        <asp:GridView ID="gvAwards" runat="server" AutoGenerateColumns="false" CssClass="table"
            OnRowCommand="gvAwards_RowCommand">
            <Columns>
                <asp:BoundField DataField="AwardTextAr" HeaderText="النص" />
                <asp:BoundField DataField="SortOrder" HeaderText="الترتيب" />
                <asp:TemplateField HeaderText="حذف">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDeleteAward" runat="server"
                            CommandName="DeleteAward"
                            CommandArgument="<%# Container.DataItemIndex %>"
                            Text="حذف" ForeColor="Red" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="admin-section">
        <h3>الخبرات العملية</h3>

        <div class="field-row">
            <label>المسمى الوظيفي</label>
            <asp:TextBox ID="txtJobTitleAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>الجهة</label>
            <asp:TextBox ID="txtOrganizationAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>الموقع</label>
            <asp:TextBox ID="txtLocationAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>الفترة</label>
            <asp:TextBox ID="txtPeriodAr" runat="server" CssClass="form-control" />
        </div>

        <div class="field-row">
            <label>الترتيب</label>
            <asp:TextBox ID="txtExperienceSortOrder" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnAddExperience" runat="server" Text="إضافة"
            CssClass="btn btn-primary" OnClick="btnAddExperience_Click" />

        <asp:GridView ID="gvExperiences" runat="server" AutoGenerateColumns="false" CssClass="table"
            OnRowCommand="gvExperiences_RowCommand">
            <Columns>
                <asp:BoundField DataField="JobTitleAr" HeaderText="المسمى" />
                <asp:BoundField DataField="OrganizationAr" HeaderText="الجهة" />
                <asp:BoundField DataField="LocationAr" HeaderText="الموقع" />
                <asp:BoundField DataField="PeriodAr" HeaderText="الفترة" />
                <asp:BoundField DataField="SortOrder" HeaderText="الترتيب" />
                <asp:TemplateField HeaderText="حذف">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDeleteExperience" runat="server"
                            CommandName="DeleteExperience"
                            CommandArgument="<%# Container.DataItemIndex %>"
                            Text="حذف" ForeColor="Red" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <asp:Button ID="btnSaveAll" runat="server" Text="حفظ جميع البيانات"
        CssClass="btn btn-success" OnClick="btnSaveAll_Click" />

</div>