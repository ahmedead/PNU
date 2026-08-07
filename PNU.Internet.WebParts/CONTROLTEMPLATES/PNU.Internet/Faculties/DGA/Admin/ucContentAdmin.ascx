<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContentAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Admin.ucContentAdmin" %>

<%-- ucListEditor is loaded dynamically in OnInit from this control's own folder,
     so there is deliberately no @Register here: a hard-coded Src would break
     the page at parse time if the deployment folder ever changes. --%>

<section class="pnu-section-anchor page-padding">

    <%-- Shown when the visitor is not an approved administrator --%>
    <asp:PlaceHolder ID="phDenied" runat="server" Visible="false">
        <div class="alert alert-danger">
            <h2 class="h5 mb-2">غير مصرح بالوصول</h2>
            <p class="mb-0">
                هذه الصفحة مخصصة للمصرح لهم فقط. للحصول على صلاحية، يرجى التواصل مع
                الإدارة العامة لتقنية المعلومات لإضافتك إلى قائمة المصرح لهم.
            </p>
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phAdmin" runat="server" Visible="false">

        <h2 class="mb-2">إدارة محتوى صفحات الكليات</h2>
        <p class="text-body-secondary"><asp:Literal ID="ltrContext" runat="server" /></p>

        <%-- 1) target subweb, 2) page section --%>
        <div class="row g-3 mb-4">
            <div class="col-12 col-md-7">
                <label class="form-label" for="<%= ddlWeb.ClientID %>">موقع الكلية (الموقع الفرعي)</label>
                <asp:DropDownList ID="ddlWeb" runat="server" CssClass="form-select"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlWeb_Changed" />
                <div class="form-text">اختاري الموقع الذي تريدين إضافة البيانات إليه.</div>
            </div>
            <div class="col-12 col-md-5">
                <label class="form-label" for="<%= ddlSection.ClientID %>">القسم</label>
                <asp:DropDownList ID="ddlSection" runat="server" CssClass="form-select"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlSection_Changed" />
                <div class="form-text">اختاري القسم المراد تحرير محتواه.</div>
            </div>
        </div>

        <asp:PlaceHolder ID="phScopeWarning" runat="server" Visible="false">
            <div class="alert alert-danger">
                الموقع المحدد خارج نطاق مجموعة المواقع الحالية، ولا يمكن التحرير عليه.
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phLoadError" runat="server" Visible="false">
            <div class="alert alert-danger">
                <strong>خطأ فني:</strong> <asp:Literal ID="ltrLoadError" runat="server" />
            </div>
        </asp:PlaceHolder>

        <%-- one editor per list in the selected section, added in OnInit --%>
        <asp:PlaceHolder ID="phEditors" runat="server" />

    </asp:PlaceHolder>
</section>
