<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteHierarchyNavigator.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.SiteHierarchyNavigator" %>



<div class="site-nav-panel" dir="rtl">
    <h3>متصفح هيكل المواقع وجرد الصفحات</h3>

    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <%-- Web Applications Dropdown --%>
            <div class="form-row">
                <label>تطبيق الويب:</label>
                <asp:DropDownList ID="ddlWebApps" runat="server" 
                    AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlWebApps_SelectedIndexChanged" 
                    CssClass="ms-long" Width="450" />
            </div>

            <%-- Site Collections (root sites of the web app) --%>
            <asp:Panel ID="pnlSiteCollections" runat="server" Visible="false" CssClass="form-row">
                <label>المجموعة الجذر (Site Collection):</label>
                <asp:DropDownList ID="ddlSiteCollections" runat="server" 
                    AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlSiteCollections_SelectedIndexChanged" 
                    CssClass="ms-long" Width="450" />
            </asp:Panel>

            <%-- Dynamic Subsite Dropdowns Container --%>
            <asp:PlaceHolder ID="phSubsiteDropdowns" runat="server" />

            <%-- Action Buttons --%>
            <asp:Panel ID="pnlActions" runat="server" Visible="false" CssClass="form-row actions">
                <asp:Label ID="lblSelectedSite" runat="server" CssClass="selected-site" />
                <br /><br />
                <asp:Button ID="btnListPages" runat="server" Text="عرض كل صفحات الموقع" 
                    OnClick="btnListPages_Click" CssClass="ms-ButtonHeightWidth primary" />
            </asp:Panel>

            <%-- Results Grid --%>
            <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="results-panel">
                <h4>الصفحات الموجودة (<asp:Literal ID="litCount" runat="server" />):</h4>
               <asp:GridView ID="gvPages" runat="server" AutoGenerateColumns="false" 
    CssClass="pages-grid" GridLines="Both" Width="100%">
    <Columns>
        <asp:BoundField DataField="Title" HeaderText="العنوان" />
        <asp:TemplateField HeaderText="الرابط (AR)">
            <ItemTemplate>
                <a href='<%# Eval("FullUrl") %>' target="_blank">
                    <%# Eval("Url") %>
                </a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="PageLayout" HeaderText="تخطيط الصفحة" />
        <asp:BoundField DataField="UserControlPath" 
            HeaderText="مسار User Control" />
        <asp:TemplateField HeaderText="نسخة EN موجودة؟">
            <ItemTemplate>
                <span class='<%# (bool)Eval("HasEnglishVersion") ? "yes-badge" : "no-badge" %>'>
                    <%# (bool)Eval("HasEnglishVersion") ? "✓ نعم" : "✗ لا" %>
                </span>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" Width="120px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="رابط EN المتوقع">
            <ItemTemplate>
                <small><%# Eval("ExpectedEnUrl") %></small>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <div class="empty">لا توجد صفحات في هذا الموقع</div>
    </EmptyDataTemplate>
</asp:GridView>

<br />
<asp:Label ID="lblMissingCount" runat="server" CssClass="missing-info" />
<br /><br />
<asp:Button ID="btnSaveToList" runat="server" 
    Text="حفظ في قائمة /ar/ITAdmin/" 
    OnClick="btnSaveToList_Click" 
    CssClass="ms-ButtonHeightWidth primary" />

<asp:Button ID="btnCreateMissingEn" runat="server" 
    Text="إنشاء صفحات EN المفقودة" 
    OnClick="btnCreateMissingEn_Click" 
    OnClientClick="return confirm('سيتم إنشاء جميع صفحات EN المفقودة بنفس المحتوى. هل أنت متأكد؟');"
    CssClass="ms-ButtonHeightWidth primary en-create-btn" />
            </asp:Panel>

            <%-- Messages --%>
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="message-panel">
                <asp:Literal ID="litMessage" runat="server" />
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<style>
    .yes-badge { color: #3c763d; font-weight: bold; padding: 3px 8px; 
    background: #dff0d8; border-radius: 3px; }
.no-badge { color: #a94442; font-weight: bold; padding: 3px 8px; 
    background: #f2dede; border-radius: 3px; }
.missing-info { color: #8a6d3b; font-weight: bold; }
.en-create-btn { background: #5cb85c !important; color: white; margin-right: 10px; }

    .site-nav-panel { padding: 15px; font-family: Tahoma, Arial; direction: rtl; }
    .form-row { margin-bottom: 12px; }
    .form-row label { display: inline-block; min-width: 180px; font-weight: bold; }
    .selected-site { color: #0072c6; font-weight: bold; }
    .results-panel { margin-top: 20px; padding: 15px; background: #f9f9f9; 
        border: 1px solid #ddd; }
    .pages-grid th { background: #0072c6; color: white; padding: 8px; }
    .pages-grid td { padding: 6px; }
    .message-panel { margin-top: 15px; padding: 10px; border-radius: 4px; }
    .message-panel.success { background: #dff0d8; color: #3c763d; 
        border: 1px solid #d6e9c6; }
    .message-panel.error { background: #f2dede; color: #a94442; 
        border: 1px solid #ebccd1; }
    .message-panel.info { background: #d9edf7; color: #31708f; 
        border: 1px solid #bce8f1; }
    .empty { padding: 20px; text-align: center; color: #999; }
    .subsite-level { margin-right: 20px; border-right: 2px solid #0072c6; 
        padding-right: 10px; }
</style>