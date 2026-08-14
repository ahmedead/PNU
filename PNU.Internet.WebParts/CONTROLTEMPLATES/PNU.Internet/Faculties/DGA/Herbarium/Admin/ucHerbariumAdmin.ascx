<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerbariumAdmin.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.ucHerbariumAdmin, $SharePoint.Project.AssemblyFullName$" %>

<section class="py-5 herbarium-admin">
    <div class="container">

        <div class="mb-4">
            <div class="d-flex justify-content-between align-items-center gap-2">
                <h2 class="mb-0"><asp:Literal ID="ltPageTitle" runat="server" /></h2>
            </div>
        </div>

        <asp:Panel ID="pnlDenied" runat="server" Visible="false">
            <div class="alert alert-warning" role="alert">
                <asp:Literal ID="ltDenied" runat="server" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlMain" runat="server">

            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div class="alert alert-success" role="alert">
                    <asp:Literal ID="ltMessage" runat="server" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger" role="alert">
                    <asp:Literal ID="ltError" runat="server" />
                </div>
            </asp:Panel>

            <div class="card mb-4">
                <div class="card-body">
                    <div class="row g-3 align-items-end">
                        <div class="col-12 col-md-6">
                            <asp:Label ID="lblListLabel" runat="server" CssClass="form-label" AssociatedControlID="ddlList" />
                            <asp:DropDownList ID="ddlList" runat="server" CssClass="form-select"
                                              AutoPostBack="true" OnSelectedIndexChanged="ddlList_SelectedIndexChanged" />
                        </div>
                        <div class="col-12 col-md-6 d-flex justify-content-md-end">
                            <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary" OnClick="btnNew_Click">
                                <i class="hgi hgi-stroke hgi-add-01 fs-5 align-middle" aria-hidden="true"></i>
                                <asp:Literal ID="ltNew" runat="server" />
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <%-- add / edit form --%>
            <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4">
                <div class="card-body">
                    <h3 class="card-title h5 mb-3"><asp:Literal ID="ltFormTitle" runat="server" /></h3>
                    <asp:Panel ID="pnlDynamicFields" runat="server" CssClass="row g-3 mb-4" />
                    <div class="d-flex justify-content-end gap-2">
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>
            </asp:Panel>

            <%-- items table --%>
            <div class="card">
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false"
                                      CssClass="table table-hover align-middle mb-0" GridLines="None"
                                      OnRowCommand="gvItems_RowCommand" OnRowDataBound="gvItems_RowDataBound">
                            <EmptyDataTemplate>
                                <div class="p-4 text-center text-muted">
                                    PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium.HerbariumHelper.GetRes("HerbariumAdmin_Empty", "لا توجد عناصر في هذه القائمة.", "No items found in this list.")
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </asp:Panel>

    </div>
</section>

<asp:HiddenField ID="hfMode" runat="server" />
<asp:HiddenField ID="hfItemId" runat="server" />
<asp:HiddenField ID="hfListBuilt" runat="server" />
