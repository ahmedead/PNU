<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGetsSubsites.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucGetsSubsites" %>


<style>
    .form-group.col-md-4 {
    padding-top: 20px;
}
</style>

<div class="col-md-12 col-sm-12">
    <div class="comp-wp">
        <div class="row">
            <asp:PlaceHolder ID="phSubsiteDropdowns" runat="server">


                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel6" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel7" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel8" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel9" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control"></asp:DropDownList>
                </div>

            </asp:PlaceHolder>
        </div>

        <div class="row">
            <div class="form-group col-md-6">
                <asp:Label ID="lblDeptTitle" AssociatedControlID="txtDeptTitle" runat="server" CssClass="form-label required" Text="Department Title"></asp:Label>
                <asp:TextBox ID="txtDeptTitle" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtDeptTitle" runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group col-md-6">
                <asp:Label ID="lblDeptCode" AssociatedControlID="txtDeptCode" runat="server" CssClass="form-label required" Text="Department Code"></asp:Label>
                <asp:TextBox ID="txtDeptCode" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtDeptCode" runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

        </div>

        <div class="row">
            <div class="form-group col-md-4">
                <asp:Label ID="lblSiteTitle" AssociatedControlID="txtSiteTitle" runat="server" CssClass="form-label required" Text="Site Title"></asp:Label>
                <asp:TextBox ID="txtSiteTitle" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtSiteTitle" runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group col-md-4">
                <asp:Label ID="lblSiteName" AssociatedControlID="txtSiteName" runat="server" CssClass="form-label required" Text="SiteURL"></asp:Label>
                <asp:TextBox ID="txtSiteName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="txtSiteName" runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group col-md-4">
                <asp:Label ID="lblSubSiteTemplates" AssociatedControlID="ddlSubSiteTemplates" runat="server" CssClass="form-label required" Text="SubSite Templates"></asp:Label>
                <asp:DropDownList ID="ddlSubSiteTemplates" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group col-md-4">
                <asp:Button ID="btnCreateSiteFromTemplate" runat="server" Text="Create News Subsite" OnClick="btnCreateSiteFromTemplate_Click" ValidationGroup="AddRequest" />
            </div>

        </div>

        <div class="row" style="display:none">
            <div class="form-group col-md-4">
                <asp:Button ID="btnCreateSubsite" runat="server" Text="Create News Subsite" OnClick="btnCreateSubsite_Click" ValidationGroup="AddRequest" />
            </div>
            <div class="form-group col-md-4">
                <asp:Button ID="btnCreateAnnouncement" runat="server" Text="Create Announcement Subsite" OnClick="btnCreateAnnouncement_Click" ValidationGroup="AddRequest" />
            </div>

        </div>

        <div class="row" style="display:none">
            <div class="form-group col-md-4">
                <asp:Button ID="btnCreateSubsite_EN" runat="server" Text="Create News Subsite English" OnClick="btnCreateSubsite_EN_Click" ValidationGroup="AddRequest" />
            </div>
            <div class="form-group col-md-4">
                <asp:Button ID="btnCreateAnnouncement_EN" runat="server" Text="Create Announcement Subsite English" OnClick="btnCreateAnnouncement_EN_Click" ValidationGroup="AddRequest" />
            </div>

        </div>
    </div>
</div>


    <asp:Literal ID="litScript" runat="server" />